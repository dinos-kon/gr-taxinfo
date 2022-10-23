using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace gr.gsis.rgwspublic.responses
{
    /*[XmlRoot(ElementName = "error_code", Namespace = "http://rgwspublic2/RgWsPublic2")]
    public class Error_code
    {
        [XmlAttribute(AttributeName = "nil", Namespace = "http://www.w3.org/2001/XMLSchema-instance")]
        public string Nil { get; set; }
    }

    [XmlRoot(ElementName = "error_descr", Namespace = "http://rgwspublic2/RgWsPublic2")]
    public class Error_descr
    {
        [XmlAttribute(AttributeName = "nil", Namespace = "http://www.w3.org/2001/XMLSchema-instance")]
        public string Nil { get; set; }
    }*/

    [XmlRoot(ElementName = "error_rec", Namespace = "http://rgwspublic2/RgWsPublic2")]
    public class Error_rec
    {
        [XmlElement(ElementName = "error_code", Namespace = "http://rgwspublic2/RgWsPublic2")]
        public String Error_code { get; set; }
        [XmlElement(ElementName = "error_descr", Namespace = "http://rgwspublic2/RgWsPublic2")]
        public String Error_descr { get; set; }
    }

    [XmlRoot(ElementName = "afm_called_by_rec", Namespace = "http://rgwspublic2/RgWsPublic2")]
    public class Afm_called_by_rec
    {
        [XmlElement(ElementName = "token_username", Namespace = "http://rgwspublic2/RgWsPublic2")]
        public string Token_username { get; set; }
        [XmlElement(ElementName = "token_afm", Namespace = "http://rgwspublic2/RgWsPublic2")]
        public string Token_afm { get; set; }
        [XmlElement(ElementName = "token_afm_fullname", Namespace = "http://rgwspublic2/RgWsPublic2")]
        public string Token_afm_fullname { get; set; }
        [XmlElement(ElementName = "afm_called_by", Namespace = "http://rgwspublic2/RgWsPublic2")]
        public string Afm_called_by { get; set; }
        [XmlElement(ElementName = "afm_called_by_fullname", Namespace = "http://rgwspublic2/RgWsPublic2")]
        public string Afm_called_by_fullname { get; set; }
        [XmlElement(ElementName = "as_on_date", Namespace = "http://rgwspublic2/RgWsPublic2")]
        public string As_on_date { get; set; }
    }
    /* 
    [XmlRoot(ElementName = "commer_title", Namespace = "http://rgwspublic2/RgWsPublic2")]
    public class Commer_title
    {
        [XmlAttribute(AttributeName = "nil", Namespace = "http://www.w3.org/2001/XMLSchema-instance")]
        public string Nil { get; set; }
    }

    [XmlRoot(ElementName = "legal_status_descr", Namespace = "http://rgwspublic2/RgWsPublic2")]
    public class Legal_status_descr
    {
        [XmlAttribute(AttributeName = "nil", Namespace = "http://www.w3.org/2001/XMLSchema-instance")]
        public string Nil { get; set; }
    }

    [XmlRoot(ElementName = "postal_address", Namespace = "http://rgwspublic2/RgWsPublic2")]
    public class Postal_address
    {
        [XmlAttribute(AttributeName = "nil", Namespace = "http://www.w3.org/2001/XMLSchema-instance")]
        public string Nil { get; set; }
    }

    [XmlRoot(ElementName = "postal_address_no", Namespace = "http://rgwspublic2/RgWsPublic2")]
    public class Postal_address_no
    {
        [XmlAttribute(AttributeName = "nil", Namespace = "http://www.w3.org/2001/XMLSchema-instance")]
        public string Nil { get; set; }
    }

    [XmlRoot(ElementName = "postal_zip_code", Namespace = "http://rgwspublic2/RgWsPublic2")]
    public class Postal_zip_code
    {
        [XmlAttribute(AttributeName = "nil", Namespace = "http://www.w3.org/2001/XMLSchema-instance")]
        public string Nil { get; set; }
    }

    [XmlRoot(ElementName = "postal_area_description", Namespace = "http://rgwspublic2/RgWsPublic2")]
    public class Postal_area_description
    {
        [XmlAttribute(AttributeName = "nil", Namespace = "http://www.w3.org/2001/XMLSchema-instance")]
        public string Nil { get; set; }
    }

    [XmlRoot(ElementName = "regist_date", Namespace = "http://rgwspublic2/RgWsPublic2")]
    public class Regist_date
    {
        [XmlAttribute(AttributeName = "nil", Namespace = "http://www.w3.org/2001/XMLSchema-instance")]
        public string Nil { get; set; }
    }

    [XmlRoot(ElementName = "stop_date", Namespace = "http://rgwspublic2/RgWsPublic2")]
    public class Stop_date
    {
        [XmlAttribute(AttributeName = "nil", Namespace = "http://www.w3.org/2001/XMLSchema-instance")]
        public string Nil { get; set; }
    }

    [XmlRoot(ElementName = "normal_vat_system_flag", Namespace = "http://rgwspublic2/RgWsPublic2")]
    public class Normal_vat_system_flag
    {
        [XmlAttribute(AttributeName = "nil", Namespace = "http://www.w3.org/2001/XMLSchema-instance")]
        public string Nil { get; set; }
    }
    */
    [XmlRoot(ElementName = "basic_rec", Namespace = "http://rgwspublic2/RgWsPublic2")]
    public class Basic_rec
    {
        [XmlElement(ElementName = "afm", Namespace = "http://rgwspublic2/RgWsPublic2")]
        public string Afm { get; set; }
        [XmlElement(ElementName = "doy", Namespace = "http://rgwspublic2/RgWsPublic2")]
        public string Doy { get; set; }
        [XmlElement(ElementName = "doy_descr", Namespace = "http://rgwspublic2/RgWsPublic2")]
        public string Doy_descr { get; set; }
        [XmlElement(ElementName = "i_ni_flag_descr", Namespace = "http://rgwspublic2/RgWsPublic2")]
        public string I_ni_flag_descr { get; set; }
        [XmlElement(ElementName = "deactivation_flag", Namespace = "http://rgwspublic2/RgWsPublic2")]
        public string Deactivation_flag { get; set; }
        [XmlElement(ElementName = "deactivation_flag_descr", Namespace = "http://rgwspublic2/RgWsPublic2")]
        public string Deactivation_flag_descr { get; set; }
        [XmlElement(ElementName = "firm_flag_descr", Namespace = "http://rgwspublic2/RgWsPublic2")]
        public string Firm_flag_descr { get; set; }
        [XmlElement(ElementName = "onomasia", Namespace = "http://rgwspublic2/RgWsPublic2")]
        public string Onomasia { get; set; }
        [XmlElement(ElementName = "commer_title", Namespace = "http://rgwspublic2/RgWsPublic2")]
        public string Commer_title { get; set; }
        [XmlElement(ElementName = "legal_status_descr", Namespace = "http://rgwspublic2/RgWsPublic2")]
        public string Legal_status_descr { get; set; }
        [XmlElement(ElementName = "postal_address", Namespace = "http://rgwspublic2/RgWsPublic2")]
        public string Postal_address { get; set; }
        [XmlElement(ElementName = "postal_address_no", Namespace = "http://rgwspublic2/RgWsPublic2")]
        public string Postal_address_no { get; set; }
        [XmlElement(ElementName = "postal_zip_code", Namespace = "http://rgwspublic2/RgWsPublic2")]
        public string Postal_zip_code { get; set; }
        [XmlElement(ElementName = "postal_area_description", Namespace = "http://rgwspublic2/RgWsPublic2")]
        public string Postal_area_description { get; set; }
        [XmlElement(ElementName = "regist_date", Namespace = "http://rgwspublic2/RgWsPublic2")]
        public string Regist_date { get; set; }
        [XmlElement(ElementName = "stop_date", Namespace = "http://rgwspublic2/RgWsPublic2")]
        public string Stop_date { get; set; }
        [XmlElement(ElementName = "normal_vat_system_flag", Namespace = "http://rgwspublic2/RgWsPublic2")]
        public string Normal_vat_system_flag { get; set; }
    }

    [XmlRoot(ElementName = "item", Namespace = "http://rgwspublic2/RgWsPublic2")]
    public class Item
    {
        [XmlElement(ElementName = "firm_act_code", Namespace = "http://rgwspublic2/RgWsPublic2")]
        public string Firm_act_code { get; set; }
        [XmlElement(ElementName = "firm_act_descr", Namespace = "http://rgwspublic2/RgWsPublic2")]
        public string Firm_act_descr { get; set; }
        [XmlElement(ElementName = "firm_act_kind", Namespace = "http://rgwspublic2/RgWsPublic2")]
        public string Firm_act_kind { get; set; }
        [XmlElement(ElementName = "firm_act_kind_descr", Namespace = "http://rgwspublic2/RgWsPublic2")]
        public string Firm_act_kind_descr { get; set; }
    }

    [XmlRoot(ElementName = "firm_act_tab", Namespace = "http://rgwspublic2/RgWsPublic2")]
    public class Firm_act_tab
    {
        [XmlElement(ElementName = "item", Namespace = "http://rgwspublic2/RgWsPublic2")]
        public List<Item> Item { get; set; }
    }

    [XmlRoot(ElementName = "rg_ws_public2_result_rtType", Namespace = "http://rgwspublic2/RgWsPublic2")]
    public class Rg_ws_public2_result_rtType
    {
        [XmlElement(ElementName = "call_seq_id", Namespace = "http://rgwspublic2/RgWsPublic2")]
        public string Call_seq_id { get; set; }
        [XmlElement(ElementName = "error_rec", Namespace = "http://rgwspublic2/RgWsPublic2")]
        public Error_rec Error_rec { get; set; }
        [XmlElement(ElementName = "afm_called_by_rec", Namespace = "http://rgwspublic2/RgWsPublic2")]
        public Afm_called_by_rec Afm_called_by_rec { get; set; }
        [XmlElement(ElementName = "basic_rec", Namespace = "http://rgwspublic2/RgWsPublic2")]
        public Basic_rec Basic_rec { get; set; }
        [XmlElement(ElementName = "firm_act_tab", Namespace = "http://rgwspublic2/RgWsPublic2")]
        public Firm_act_tab Firm_act_tab { get; set; }
    }

    [XmlRoot(ElementName = "result", Namespace = "http://rgwspublic2/RgWsPublic2Service")]
    public class Result
    {
        [XmlElement(ElementName = "rg_ws_public2_result_rtType", Namespace = "http://rgwspublic2/RgWsPublic2")]
        public Rg_ws_public2_result_rtType Rg_ws_public2_result_rtType { get; set; }
    }

    [XmlRoot(ElementName = "rgWsPublic2AfmMethodResponse", Namespace = "http://rgwspublic2/RgWsPublic2Service")]
    public class RgWsPublic2AfmMethodResponse
    {
        [XmlElement(ElementName = "result", Namespace = "http://rgwspublic2/RgWsPublic2Service")]
        public Result Result { get; set; }
        [XmlAttribute(AttributeName = "xmlns")]
        public string Xmlns { get; set; }
        [XmlAttribute(AttributeName = "srvc", Namespace = "http://www.w3.org/2000/xmlns/")]
        public string Srvc { get; set; }
    }

    [XmlRoot(ElementName = "Body", Namespace = "http://www.w3.org/2003/05/soap-envelope")]
    public class Body
    {
        [XmlElement(ElementName = "rgWsPublic2AfmMethodResponse", Namespace = "http://rgwspublic2/RgWsPublic2Service")]
        public RgWsPublic2AfmMethodResponse RgWsPublic2AfmMethodResponse { get; set; }
    }

    [XmlRoot(ElementName = "Envelope", Namespace = "http://www.w3.org/2003/05/soap-envelope")]
    public class Envelope
    {
        [XmlElement(ElementName = "Header", Namespace = "http://www.w3.org/2003/05/soap-envelope")]
        public string Header { get; set; }
        [XmlElement(ElementName = "Body", Namespace = "http://www.w3.org/2003/05/soap-envelope")]
        public Body Body { get; set; }
        [XmlAttribute(AttributeName = "env", Namespace = "http://www.w3.org/2000/xmlns/")]
        public string Env { get; set; }
        [XmlAttribute(AttributeName = "xsd", Namespace = "http://www.w3.org/2000/xmlns/")]
        public string Xsd { get; set; }
        [XmlAttribute(AttributeName = "xsi", Namespace = "http://www.w3.org/2000/xmlns/")]
        public string Xsi { get; set; }
    }

}
