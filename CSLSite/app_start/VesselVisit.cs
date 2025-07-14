using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace CSLSite.app_start
{
    [Serializable]
    [XmlRoot(ElementName = "vessel-visit")]
    public class VesselVisit
    {
        public VesselVisit()
        {
            this.lines = new HashSet<line>();
            this.classification = "DEEPSEA";
            this.facility = "GYE";
            this.iscommoncarrier = "N";
            this.isdrayoff = "N";
            isnoclientaccess = "N";
        }
        [XmlAttribute(AttributeName = "id")]
        public string id { get; set; }
        
        [XmlAttribute(AttributeName = "visit-phase")]
        public string visitphase { get; set; }
        
        [XmlAttribute(AttributeName = "vessel-id")]
        public string vesselid { get; set; }

        [XmlAttribute(AttributeName = "service-id")]
        public string serviceid { get; set; }

        [XmlAttribute(AttributeName = "ETA")]
        public string eta { get; set; }

        [XmlAttribute(AttributeName = "ETD")]
        public string etd { get; set; }

        [XmlAttribute(AttributeName = "operator-id")]
        public string operatorid { get; set; }

        [XmlAttribute(AttributeName = "is-common-carrier")]
        public string iscommoncarrier { get; set; }

        [XmlAttribute(AttributeName = "is-dray-off")]
        public string isdrayoff { get; set; }

        [XmlAttribute(AttributeName = "is-no-client-access")]
        public string isnoclientaccess { get; set; }

        [XmlAttribute(AttributeName = "out-call-number")]
        public string outcallnumber { get; set; }

        [XmlAttribute(AttributeName = "out-voy-nbr")]
        public string outvoynbr { get; set; }

        [XmlAttribute(AttributeName = "in-call-number")]
        public string incallnumber { get; set; }

        [XmlAttribute(AttributeName = "in-voy-nbr")]
        public string invoynbr { get; set; }

        [XmlAttribute(AttributeName = "classification")]
        public string classification { get; set; }

        [XmlAttribute(AttributeName = "facility")]
        public string facility { get; set; }

        [XmlAttribute(AttributeName = "in-custom-voy-nbr")]
        public string incustomvoynbr { get; set; }

        [XmlAttribute(AttributeName = "out-custom-voy-nbr")]
        public string outcustomvoynbr { get; set; }

        [XmlAttribute(AttributeName = "vv-flex-string-1")]
        public string vvflexstring1 { get; set; }

        [XmlAttribute(AttributeName = "vv-flex-date-2")]
        public string vvFlexDate02 { get; set; }

        [XmlAttribute(AttributeName = "time-off-port-arrive")]
        public string timeoffportarrive { get; set; }


        [XmlAttribute(AttributeName = "time-cargo-cutoff")]
        public string timecargocutoff { get; set; }

        [XmlAttribute(AttributeName = "notes")]
        public string notes { get; set; }

        //time-reefer-cutoff
        [XmlAttribute(AttributeName = "time-reefer-cutoff")]
        public string timereefercutoff { get; set; }

        //time-haz-cutoff/
        [XmlAttribute(AttributeName = "time-haz-cutoff")]
        public string timehazcutoff { get; set; }

        [XmlArray("lines")]
        [XmlArrayItem("line", typeof(line))]
        public HashSet<line> lines { get; set; }
    }


    public class line
    {
        [XmlAttribute(AttributeName = "id")]
        public string id { get; set; }

        [XmlAttribute(AttributeName = "in-voy-nbr")]
        public string invoynbr { get; set; }

        [XmlAttribute(AttributeName = "out-voy-nbr")]
        public string outvoynbr { get; set; }
    }
}