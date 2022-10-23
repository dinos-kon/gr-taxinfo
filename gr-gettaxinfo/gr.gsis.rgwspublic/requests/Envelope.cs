using System.Xml.Serialization;

namespace gr.gsis.rgwspublic.requests
{
    [XmlRoot(ElementName = "UsernameToken", Namespace = "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd")]
    public class UsernameToken
    {
        [XmlElement(ElementName = "Username", Namespace = "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd")]
        public string username { get; set; }
        [XmlElement(ElementName = "Password", Namespace = "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd")]
        public string password { get; set; }
    }

    [XmlRoot(ElementName = "Security", Namespace = "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd")]
    public class Security
    {
        [XmlElement(ElementName = "UsernameToken", Namespace = "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd")]
        public UsernameToken usernameToken { get; set; }
    }

    [XmlRoot(ElementName = "Header", Namespace = "http://www.w3.org/2003/05/soap-envelope")]
    public class Header
    {
        [XmlElement(ElementName = "Security", Namespace = "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd")]
        public Security security { get; set; }
    }

    [XmlRoot(ElementName = "INPUT_REC", Namespace = "http://rgwspublic2/RgWsPublic2Service")]
    public class INPUT_REC
    {
        [XmlElement(ElementName = "afm_called_by", Namespace = "http://rgwspublic2/RgWsPublic2")]
        public string afm_called_by { get; set; }
        [XmlElement(ElementName = "afm_called_for", Namespace = "http://rgwspublic2/RgWsPublic2")]
        public string afm_called_for { get; set; }
        [XmlElement(ElementName = "as_on_date", Namespace = "http://rgwspublic2/RgWsPublic2")]
        public string as_on_date { get; set; }
    }

    [XmlRoot(ElementName = "rgWsPublic2AfmMethod", Namespace = "http://rgwspublic2/RgWsPublic2Service")]
    public class RgWsPublic2AfmMethod
    {
        [XmlElement(ElementName = "INPUT_REC", Namespace = "http://rgwspublic2/RgWsPublic2Service")]
        public INPUT_REC input_rec { get; set; }
    }

    [XmlRoot(ElementName = "Body", Namespace = "http://www.w3.org/2003/05/soap-envelope")]
    public class Body
    {
        [XmlElement(ElementName = "rgWsPublic2AfmMethod", Namespace = "http://rgwspublic2/RgWsPublic2Service")]
        public RgWsPublic2AfmMethod rgWsPublic2AfmMethod { get; set; }
    }

    [XmlRoot(ElementName = "Envelope", Namespace = "http://www.w3.org/2003/05/soap-envelope")]
    public class Envelope
    {
        [XmlElement(ElementName = "Header", Namespace = "http://www.w3.org/2003/05/soap-envelope")]
        public Header header { get; set; }
        [XmlElement(ElementName = "Body", Namespace = "http://www.w3.org/2003/05/soap-envelope")]
        public Body body { get; set; }
        [XmlAttribute(AttributeName = "env", Namespace = "http://www.w3.org/2000/xmlns/")]
        public string env { get; set; }
        [XmlAttribute(AttributeName = "ns1", Namespace = "http://www.w3.org/2000/xmlns/")]
        public string ns1 { get; set; }
        [XmlAttribute(AttributeName = "ns2", Namespace = "http://www.w3.org/2000/xmlns/")]
        public string ns2 { get; set; }
        [XmlAttribute(AttributeName = "ns3", Namespace = "http://www.w3.org/2000/xmlns/")]
        public string ns3 { get; set; }
    }

}
