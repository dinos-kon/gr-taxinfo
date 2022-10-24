using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using gr.gsis.rgwspublic.requests;
using System.Net.Http;
using System.Text;
using System.Xml.Serialization;
using System.Net;
using Azure.Security.KeyVault.Secrets;
using Azure.Identity;
using Azure;

namespace gr.gsis.rgwspublic
{

    public static class GetTaxInfo
    {


        [FunctionName("GetTaxInfo")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);

            string CallerTIN = data?.CallerTIN;
            string QueriedTIN = data?.QueriedTIN;
            string CallerUsername = data?.CallerUsername;
            string CallerPassword = data?.CallerPassword;
            string queryForDate = (data?.queryForDate) ?? DateTime.Now.ToString("yyyy-MM-dd");
            string endpointURL = (data?.endpointURL) ?? Environment.GetEnvironmentVariable("endpointUri");

            #region Create request
            var request = new gr.gsis.rgwspublic.requests.Envelope()
            {
                header = new gr.gsis.rgwspublic.requests.Header()
                {
                    security = new Security()
                    {
                        usernameToken = new UsernameToken()
                        {
                            username = "username",
                            password = "password"
                        }
                    }
                },
                body = new gr.gsis.rgwspublic.requests.Body()
                {
                    rgWsPublic2AfmMethod = new RgWsPublic2AfmMethod()
                    {
                        input_rec = new INPUT_REC()
                        {
                            afm_called_by = "TIN of Caller/ το ΑΦΜ σας",
                            afm_called_for = "TIN Queried/ το ΑΦΜ προς επιβεβαίωση",
                            as_on_date = "date in yyyy-MM-dd format"
                        }
                    }
                }
            };
            #endregion 
            #region Handle missing parameters
            if (String.IsNullOrEmpty(CallerPassword) || String.IsNullOrEmpty(CallerUsername)
                || String.IsNullOrEmpty(CallerTIN)
                || String.IsNullOrEmpty(QueriedTIN)
                || String.IsNullOrEmpty(queryForDate)
                )
            {
                // Console.WriteLine($"Could not retrieve details for VAT#: {request.body.rgWsPublic2AfmMethod.input_rec.afm_called_for}, response was: {rawWebResponse} because: {e.ToString()}");
                rgwspublic.responses.Rg_ws_public2_result_rtType response = new rgwspublic.responses.Rg_ws_public2_result_rtType();
                response.Error_rec = new rgwspublic.responses.Error_rec() { Error_code = "MISSING_PARAMETERS", Error_descr = $"An unexpected error occurred. We are expecting the following parameters {Service.expectedRequestBody}" };

                return new BadRequestObjectResult(response);
            }
            #endregion

            #region Update request
             
            request.header.security.usernameToken.username = CallerUsername;
            request.header.security.usernameToken.password = CallerPassword;
            request.body.rgWsPublic2AfmMethod.input_rec.afm_called_by = CallerTIN;
            request.body.rgWsPublic2AfmMethod.input_rec.afm_called_for = QueriedTIN;
            request.body.rgWsPublic2AfmMethod.input_rec.as_on_date = queryForDate;
            #endregion 
             

            gr.gsis.rgwspublic.responses.Rg_ws_public2_result_rtType info = await Service.GetInfo_internal(log, request);

            return new OkObjectResult(info);
        }
    }


    public static class GetTaxInfoUsingVault
    {
        #region Constants (settings and parameter names)
        /* default service uri */
        public const string endpointUri_preset = "https://www1.gsis.gr:443/wsaade/RgWsPublic2/RgWsPublic2";

        /* Azure property settings */
        public const string VaultUri_settingName = "VaultUri";
        public const string gsisCallerTIN_settingName = "gr-gsis-CallerTIN";

        /* Vault secrets */
        public static string gsisUsername_settingName = "gr-gsis-username";
        public static string gsisPassword_settingName = "gr-gsis-password";

        /* POST parameters */
        public const string CallerTIN_paramName = "CallerTIN";
        public const string username_paramName = "CallerUsername";
        public const string password_paramName = "CallerPassword";
        public const string QueriedTIN_paramName = "QueriedTIN";
        public const string endpointURL_paramName = "endpointUrl";

        #endregion

        [FunctionName("GetTaxInfoUsingVault")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,
            ILogger log)
        {

            responses.Rg_ws_public2_result_rtType info = new responses.Rg_ws_public2_result_rtType();

            #region Welcome
            log.LogInformation("C# HTTP trigger function processed a request.");
            #endregion

            #region Read request.body parameters
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);

            string CallerTIN = data?.CallerTIN?? GetCallerTIN_FromEnvironment(log);
            string TINQueried = data?.TINQueried;
            string username = data?.username;
            string password = data?.password;
            string queryForDate = (data?.queryForDate) ?? DateTime.Now.ToString("yyyy-MM-dd");
            string endpointURL = (data?.endpointURL) ?? Environment.GetEnvironmentVariable("endpointUri");
            #endregion read request.body parameters

            #region Retrieve credentials if needed (if not provided in post body already)
            // Retrieve credentials if needed
            //      read Vault
            string vaultURL = String.Empty;
            vaultURL = GetVaultURL_FromEnvironment(log); 

            if (!String.IsNullOrEmpty(vaultURL))
                if ((String.IsNullOrEmpty(username)) || (String.IsNullOrEmpty(password)))
                {
                    var credentials = GetCredentials_FromVault(log, vaultURL);
                    username = credentials.username;
                    password = credentials.password;
                }
            #endregion retrieve credentials if needed (if not provided in post body already)

            #region Create request
            var request = new gr.gsis.rgwspublic.requests.Envelope()
            {
                header = new gr.gsis.rgwspublic.requests.Header()
                {
                    security = new Security()
                    {
                        usernameToken = new UsernameToken()
                        {
                            username = "username",
                            password = "password"
                        }
                    }
                },
                body = new gr.gsis.rgwspublic.requests.Body()
                {
                    rgWsPublic2AfmMethod = new RgWsPublic2AfmMethod()
                    {
                        input_rec = new INPUT_REC()
                        {
                            afm_called_by = "TIN of Caller/ το ΑΦΜ σας",
                            afm_called_for = "TIN Queried/ το ΑΦΜ προς επιβεβαίωση",
                            as_on_date = "date in yyyy-MM-dd format"
                        }
                    }
                }
            };
            #endregion 


            #region Handle missing parameters
            if (String.IsNullOrEmpty(password) || String.IsNullOrEmpty(username)
                || String.IsNullOrEmpty(CallerTIN)
                || String.IsNullOrEmpty(TINQueried)
                || String.IsNullOrEmpty(queryForDate)
                )
            {
                // Console.WriteLine($"Could not retrieve details for VAT#: {request.body.rgWsPublic2AfmMethod.input_rec.afm_called_for}, response was: {rawWebResponse} because: {e.ToString()}");
                info = new rgwspublic.responses.Rg_ws_public2_result_rtType();
                info.Error_rec = new rgwspublic.responses.Error_rec() { Error_code = "MISSING_PARAMETERS", Error_descr = $"An unexpected error occurred. We are expecting the following parameters {Service.expectedRequestBody}" };

                return new BadRequestObjectResult(info);
            }
            #endregion

            #region Update request
            request.header.security.usernameToken.username = username;
            request.header.security.usernameToken.password = password;
            request.body.rgWsPublic2AfmMethod.input_rec.afm_called_by = CallerTIN;
            request.body.rgWsPublic2AfmMethod.input_rec.afm_called_for = TINQueried;
            request.body.rgWsPublic2AfmMethod.input_rec.as_on_date = queryForDate;
            #endregion 

            info = await Service.GetInfo_internal(log, request, endpointURL);

            return new OkObjectResult(info);
        }

        private static string GetCallerTIN_FromEnvironment(ILogger log/*, string now*/)
        {
            string CallerTIN = Environment.GetEnvironmentVariable(gsisCallerTIN_settingName);
            // log.LogTrace($"{now}: TIN of Caller: {CallerTIN}");
            log.LogTrace($"TIN of Caller: {CallerTIN}");
            return CallerTIN;
        }

        private static string GetVaultURL_FromEnvironment(ILogger log/*, string now*/)
        {
            string vaultURL = Environment.GetEnvironmentVariable(VaultUri_settingName);
            // log.LogTrace($"{now}: Vault URL: {vaultURL}");
            log.LogTrace($"Vault URL: {vaultURL}");
            return vaultURL;
        }

        private static Credentials GetCredentials_FromVault(ILogger log, string vaultURL)
        {
            Credentials creds = new Credentials();
            var watch = System.Diagnostics.Stopwatch.StartNew();
            SecretClientOptions options = new SecretClientOptions()
            {
                Retry =
                    {
                        Delay= TimeSpan.FromSeconds(2),
                        MaxDelay = TimeSpan.FromSeconds(16),
                        MaxRetries = 5,
                        Mode = Azure.Core.RetryMode.Exponential
                    }
            };
            var client = new SecretClient(new Uri(vaultURL), new Azure.Identity.DefaultAzureCredential(), options);
            if (String.IsNullOrEmpty(creds.username))
                try
                {
                    KeyVaultSecret secret_username = client.GetSecret(gsisUsername_settingName);
                    creds.username = creds.username ?? secret_username?.Value;
                    log.LogTrace($"UserName from vault: {creds.username}");
                }
                catch (Exception ex1)
                {
                    log.LogError(@$"Could not read {username_paramName} from the Vault (""{vaultURL}""), because: {ex1.ToString()}");
                }
            if (String.IsNullOrEmpty(creds.password))
                try
                {
                    KeyVaultSecret secret_password = client.GetSecret(gsisPassword_settingName);

                    creds.password = creds.password ?? secret_password?.Value;
                    log.LogTrace($"Password from vault: {(creds.password?.Length > 0 ? "ok" : "empty")}");
                }
                catch (Exception ex1)
                {
                    log.LogError(@$"Could not read {password_paramName} from the Vault (""{vaultURL}""), because: {ex1.ToString()}");
                }
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            log.LogMetric("GetCredentials_FromVault", elapsedMs);
            log.LogInformation($"Credentials from Vault read in {elapsedMs}");
            return creds;
        }

        private class Credentials
        {
            public string username { get; internal set; }

            public string password { get; internal set; }
        }
    }

    public static class Service
    {
        public const string expectedRequestBody = @"{""CallerTIN"":""Mandatory:999999999"", ""CallerUsername"":""Mandatory:UUUUUUUUU"", ""CallerPassword"":""Mandatory:PPPPPPPPP"", ""QueriedTIN"":""Mandatory:999999999"", ""queryForDate"":""optional:yyyy-MM-dd"",""endpointUrl"":""optional url of the gsis service"" }";

        /// <summary>
        /// returns information of a person or legal entity given their TIN. Calls the corresponding service described in this address: 
        /// https://www.aade.gr/epiheiriseis/forologikes-ypiresies/mitroo/anazitisi-basikon-stoiheion-mitrooy-epiheiriseon
        /// Με τη χρήση αυτής της υπηρεσίας, τα νομικά πρόσωπα, οι νομικές οντότητες, και τα φυσικά πρόσωπα με εισόδημα από επιχειρηματική δραστηριότητα 
        /// μπορούν να αναζητήσουν βασικές πληροφορίες, προκειμένου να διακριβώσουν τη φορολογική ή την επαγγελματική υπόσταση άλλων νομικών προσώπων 
        /// ή νομικών οντοτήτων ή  φορολογουμένων/φυσικών προσώπων που ασκούν επιχειρηματική δραστηριότητα. 
        /// </summary>
        /// <param name="request">the request containing the caller, the TIN investigated and the date of interest</param>
        /// <param name="endpointUrl">preset to https://www1.gsis.gr:443/wsaade/RgWsPublic2/RgWsPublic2</param>
        /// <returns></returns>
        internal static async Task<gr.gsis.rgwspublic.responses.Rg_ws_public2_result_rtType> GetInfo_internal(
            ILogger log,
            gr.gsis.rgwspublic.requests.Envelope request,
            string endpointUrl = @"https://www1.gsis.gr:443/wsaade/RgWsPublic2/RgWsPublic2")
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            var outgoingEnvelope = request;

            String rawWebResponse = String.Empty;
            gr.gsis.rgwspublic.responses.Rg_ws_public2_result_rtType response = new gr.gsis.rgwspublic.responses.Rg_ws_public2_result_rtType();
            using (StringWriter textWriter = new StringWriter())
            {
                gr.gsis.rgwspublic.responses.Envelope incomingEnvelope = new gr.gsis.rgwspublic.responses.Envelope();
                try
                {
                    using StringWriter outTextWriter = new StringWriter();
                    XmlSerializer outSerializer = new XmlSerializer(typeof(gr.gsis.rgwspublic.requests.Envelope));
                    outSerializer.Serialize(outTextWriter, outgoingEnvelope);
                    var stringContent = new StringContent(outTextWriter.ToString(), Encoding.UTF8, "application/soap+xml");
                    HttpClient client = new HttpClient();

                    using HttpResponseMessage webResponse = await client.PostAsync(endpointUrl, stringContent);
                    webResponse.EnsureSuccessStatusCode();
                    rawWebResponse = await webResponse.Content.ReadAsStringAsync();

                    XmlSerializer serializer = new XmlSerializer(typeof(gr.gsis.rgwspublic.responses.Envelope));
                    using (MemoryStream memStream = new MemoryStream(Encoding.UTF8.GetBytes(rawWebResponse)))
                    {
                        incomingEnvelope = (gr.gsis.rgwspublic.responses.Envelope)serializer.Deserialize(memStream);
                    }
                    response = incomingEnvelope?.Body.RgWsPublic2AfmMethodResponse.Result.Rg_ws_public2_result_rtType;
                    
                    if (!String.IsNullOrEmpty(incomingEnvelope?.Body.RgWsPublic2AfmMethodResponse.Result.Rg_ws_public2_result_rtType.Error_rec?.Error_code))
                    {
                        Console.WriteLine($@"{incomingEnvelope?.Body.RgWsPublic2AfmMethodResponse.Result.Rg_ws_public2_result_rtType.Error_rec.Error_code}: {incomingEnvelope?.Body.RgWsPublic2AfmMethodResponse.Result.Rg_ws_public2_result_rtType.Error_rec?.Error_descr}");
                    }
                    if (!String.IsNullOrEmpty(incomingEnvelope?.Body.RgWsPublic2AfmMethodResponse.Result.Rg_ws_public2_result_rtType.Basic_rec.Afm))
                    {
                        Console.WriteLine($@"{incomingEnvelope?.Body.RgWsPublic2AfmMethodResponse.Result.Rg_ws_public2_result_rtType.Basic_rec.Afm}: {incomingEnvelope?.Body.RgWsPublic2AfmMethodResponse.Result.Rg_ws_public2_result_rtType.Basic_rec.Commer_title}");
                    }
                    else
                    {
                        Console.WriteLine($"RAW: {rawWebResponse}\n");
                        using (StringWriter elseTextWriter = new StringWriter())
                        {
                            if (incomingEnvelope != null)
                            {
                                outSerializer.Serialize(elseTextWriter, incomingEnvelope.Body.RgWsPublic2AfmMethodResponse.Result.Rg_ws_public2_result_rtType);
                                Console.WriteLine($"{elseTextWriter.ToString()}\n");
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    // Console.WriteLine($"Could not retrieve details for VAT#: {request.body.rgWsPublic2AfmMethod.input_rec.afm_called_for}, response was: {rawWebResponse} because: {e.ToString()}");
                    response = new rgwspublic.responses.Rg_ws_public2_result_rtType();
                    response.Error_rec = new rgwspublic.responses.Error_rec() { Error_code = "OTHER", Error_descr = $"An unexpected error occurred: {e.ToString()}" };
                }
            }

            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            log.LogMetric("GetINFO_FromGSIS", elapsedMs);
            log.LogInformation($"INFO from GSIS read in {elapsedMs}");
            return response;
        }
        

    }
}
