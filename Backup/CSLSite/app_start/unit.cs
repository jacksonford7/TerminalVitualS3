using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace CSLSite.N4Object
{
    [Serializable]
    [XmlRoot(ElementName = "unit")]
    public partial class UnitAdvice
    {
        public UnitAdvice()
        {
            this.unitflex = new unitflex();
            this.flags = new HashSet<flag>();
            this.sello = new sellos();
            this.contents = new unitcontent();
            this.routing = new routing();
            this.equipment = new equipment();
            this.booking = new booking();
            this.eventos = new List<evento>();
            this.oog = new oog();
            this.reefer = new reefer();
            this.handling = new handling();
            this.hazards = new HashSet<hazard>();
            this.specialstow = new HashSet<stow>();
        }
        [XmlAttribute(AttributeName = "id")]
        public string id { get; set; }
        [XmlAttribute(AttributeName = "created-by")]
        public string createdby { get; set; }
        [XmlAttribute(AttributeName = "category")]
        public category category { get; set; }
        [XmlAttribute(AttributeName = "transit-state")]
        public transitstate transitstate { get; set; }
        [XmlAttribute(AttributeName = "line")]
        public string line { get; set; }
        [XmlAttribute(AttributeName = "freight-kind")]
        public freightkind freightkind {get;set;}
        [XmlElement(ElementName = "equipment")]
        public equipment equipment { get; set; }
        [XmlElement(ElementName = "handling")]
        public handling handling{get;set;}
        [XmlElement(ElementName = "routing")]
        public routing routing { get; set; }
        [XmlArray("flags")]
        [XmlArrayItem("hold", typeof(flag))]
        public HashSet<flag> flags {get;set;}
        [XmlArray("special-stows")]
        [XmlArrayItem("special-stow-3", typeof(stow))]
        public HashSet<stow> specialstow { get; set; }
        [XmlArray("hazards")]
        [XmlArrayItem("hazard", typeof(hazard))]
        public HashSet<hazard> hazards { get; set; }
        [XmlElement(ElementName = "booking")]
        public booking booking {get;set;}
        [XmlElement(ElementName = "oog")]
        public oog oog { get; set; }
        [XmlElement(ElementName = "contents")]
        public unitcontent contents {get;set;}
        [XmlElement(ElementName = "seals")]
        public sellos sello  {get;set;}
        [XmlElement(ElementName = "unit-flex")]
        public unitflex unitflex {get;set;}
        [XmlElement(ElementName = "event")]
        public List<evento> eventos { get; set; }
        [XmlElement(ElementName = "reefer")]
        public reefer reefer { get; set; }
    }
    [Serializable]
    public class equipment
    {
        public equipment()
        {
            this.clase = "CTR";
            this.role = "PRIMARY";
            this.reefer = new erefeer();
            this.ownership = new ownership();
        }
        [XmlAttribute(AttributeName = "eqid")]
        public string eqid { get; set; }
        [XmlAttribute(AttributeName = "type")]
        public string tipo { get; set; }
        [XmlAttribute(AttributeName = "class")]
        public string clase { get; set; }
        [XmlAttribute(AttributeName = "role")]
        public string role { get; set; }
        [XmlElement(ElementName = "reefer")]
        public erefeer reefer { get; set; }

        [XmlElement(ElementName = "ownership")]
        public ownership ownership { get; set; }
    }
    [Serializable]
    public class reefer
    {
        public reefer()
        {
            this.tempdisplayunit = "C";
            this.extendedtimemonitors = "N";
            this.wantedispower = "N";
            this.ispower = "N";
        }
        [XmlAttribute(AttributeName = "temp-reqd-c")]
        public string tempreqdc { get; set; }
        [XmlAttribute(AttributeName = "temp-min-c")]
        public string tempminc { get; set; }
        [XmlAttribute(AttributeName = "temp-max-c")]
        public string tempmaxc { get; set; }
        [XmlAttribute(AttributeName = "temp-display-unit")]
        public string tempdisplayunit { get; set; }
        [XmlAttribute(AttributeName = "extended-time-monitors")]
        public string extendedtimemonitors { get; set; }
        [XmlAttribute(AttributeName = "is-power")]
        public string ispower { get; set; }
        [XmlAttribute(AttributeName = "wanted-is-power")]
        public string wantedispower { get; set; }
        [XmlAttribute(AttributeName = "humidity-pct")]
        public string humiditypct { get; set; }
        [XmlAttribute(AttributeName = "vent-required-value")]
        public string ventrequiredvalue { get; set; }
        [XmlAttribute(AttributeName = "vent-required-unit")]
        public string  ventrequiredunit { get; set; }
    }
    [Serializable]
    public class carrier
    {
        public carrier()
        {
            this.facility = "GYE";
            this.mode = "VESSEL";

        }
        [XmlAttribute(AttributeName = "id")]
        public string id { get; set; }
        [XmlAttribute(AttributeName = "facility")]
        public string facility { get; set; }
        [XmlAttribute(AttributeName = "mode")]
        public string mode { get; set; }
        [XmlAttribute(AttributeName = "direction")]
        public string direction { get; set; }
        [XmlAttribute(AttributeName = "qualifier")]
        public string qualifier { get; set; }
    }
    [Serializable]
    public class booking
    {
        [XmlAttribute(AttributeName = "id")]
        public string id { get; set; }
    }
    [Serializable]
    public class unitflex
    {
        [XmlAttribute(AttributeName = "unit-flex-1")]
        public string flex1 { get; set; }
        [XmlAttribute(AttributeName = "unit-flex-2")]
        public string flex2 { get; set; }
        [XmlAttribute(AttributeName = "unit-flex-11")]
        public string flex11 { get; set; }
        [XmlAttribute(AttributeName = "unit-flex-14")]
        public string flex14 { get; set; }
    }
    [Serializable]
    public class handling
    {
        [XmlAttribute(AttributeName = "remark")]
        public string remark { get; set; }
    }
    [Serializable]
    public class evento
    {
        [XmlAttribute(AttributeName = "id")]
        public string id { get; set; }
        [XmlAttribute(AttributeName = "time-event-applied")]
        public string timeeventapplied { get; set; }
        [XmlAttribute(AttributeName = "is-billable")]
        public string isbillable { get; set; }
        [XmlAttribute(AttributeName = "user-id")]
        public string userid { get; set; }
        public evento()
        {
            this.userid = "user:admin";
            this.isbillable = "N";
        }
    }
    [Serializable]
    public class routing
    {
        public routing() 
        {
            this.carrier = new List<carrier>();
        }
        [XmlElement(ElementName = "carrier")]
        public List<carrier> carrier { get; set; }
        [XmlAttribute(AttributeName = "pol")]
        public string pol {get;set;}
        [XmlAttribute(AttributeName = "pod-1")]
        public string pod1 { get; set; }
        [XmlAttribute(AttributeName = "pod-2")]
        public string pod2 { get; set; }
        [XmlAttribute(AttributeName = "group")]
        public string group { get; set; }
        

    }
    [Serializable]
    public partial class flag
    {
        [XmlAttribute(AttributeName = "id")]
        public string id {get;set;}
        [XmlAttribute(AttributeName = "time-applied")]
        public string timeapplied { get; set; }
        [XmlAttribute(AttributeName = "notes")]
        public string notes { get; set; }
    }
    [Serializable]
    public class unitcontent
    {
        [XmlAttribute(AttributeName = "weight-kg")]
        public string weightkg { get; set; }
        [XmlAttribute(AttributeName = "shipper-id")]
        public string shipperid { get; set; }
    }
    [Serializable]
    public partial class sellos
    {
        [XmlAttribute(AttributeName = "seal-1")]
        public string seal1{get;set;}
        [XmlAttribute(AttributeName = "seal-2")]
        public string seal2 { get; set; }
        [XmlAttribute(AttributeName = "seal-3")]
        public string seal3 { get; set; }
        [XmlAttribute(AttributeName = "seal-4")]
        public string seal4 { get; set; }
    }
    [Serializable]
    public partial class erefeer
    {
        [XmlAttribute(AttributeName = "is-controlled-atmosphere")]
        public string iscontrolledatmosphere { get; set; }
        [XmlAttribute(AttributeName = "is-starvent")]
        public string isstarvent { get; set; }
        [XmlAttribute(AttributeName = "is-super-freeze")]
        public string issuperfreeze { get; set; }
        [XmlAttribute(AttributeName = "is-temperature-controlled")]
        public string istemperaturecontrolled { get; set; }
        [XmlAttribute(AttributeName = "rfr-type")]
        public refertype rfrtype { get; set; }

        public erefeer()
        {
            this.iscontrolledatmosphere = "N";
            this.isstarvent = "N";
            this.issuperfreeze = "N";
            this.istemperaturecontrolled="N";
        }
    }
    [Serializable]
    public partial class oog
    {
        [XmlAttribute(AttributeName = "left-cm")]
        public string leftcm { get; set; }
        [XmlAttribute(AttributeName = "right-cm")]
        public string rightcm { get; set; }
        [XmlAttribute(AttributeName = "top-cm")]
        public string topcm { get; set; }
        [XmlAttribute(AttributeName = "front-cm")]
        public string frontcm { get; set; }
        [XmlAttribute(AttributeName = "back-cm")]
        public string backcm { get; set; }
    }
    [Serializable]
    public partial class hazard
    {
        [XmlAttribute(AttributeName = "imdg")]
        public string imdg { get; set; }
        [XmlAttribute(AttributeName = "un")]
        public string un { get; set; }
        [XmlAttribute(AttributeName = "ltd-qty-flag")]
        public string ltdqtyflag { get; set; }
        [XmlAttribute(AttributeName = "marine-pollutants")]
        public string marinepollutants { get; set; }
        public hazard()
        {
            this.ltdqtyflag = "N";
            this.marinepollutants = "N";
            this.un = "1";
        }

    }
    [Serializable]
    public partial class stow
    {
        [XmlAttribute(AttributeName = "id")]
        public string id { get; set; }
       
    }
    [Serializable]
    public class ownership
    {
        [XmlAttribute(AttributeName = "owner")]
        public string owner { get; set; }
        [XmlAttribute(AttributeName = "operator")]
        public string operador { get; set; }
    }
    #region "enumeraciones"
    public enum transitstate
    {

        /// <comentarios/>
        YARD,

        /// <comentarios/>
        ADVISED,

        /// <comentarios/>
        LOADED,

        /// <comentarios/>
        INBOUND,

        /// <comentarios/>
        ECIN,

        /// <comentarios/>
        ECOUT,

        /// <comentarios/>
        DEPARTED,

        /// <comentarios/>
        RETIRED,
    }
    public enum category
    {

        /// <comentarios/>
        IMPORT,

        /// <comentarios/>
        STORAGE,

        /// <comentarios/>
        EXPORT,

        /// <comentarios/>
        TRANSSHIP,

        /// <comentarios/>
        THROUGH,

        /// <comentarios/>
        DOMESTIC,
    }
    public enum freightkind
    {

        /// <comentarios/>
        FCL,

        /// <comentarios/>
        MTY,

        /// <comentarios/>
        BBK,

        /// <comentarios/>
        LCL,
    }
    public enum refertype
    {

        /// <comentarios/>
        NON_RFR,

        /// <comentarios/>
        PORTHOLE,

        /// <comentarios/>
        FANTAINER,

        /// <comentarios/>
        INTEG_UNK,

        /// <comentarios/>
        INTEG_H20_SINGLE,

        /// <comentarios/>
        INTEG_H20,

        /// <comentarios/>
        INTEG_AIR_SINGLE,

        /// <comentarios/>
        INTEG_AIR,
    }
    #endregion
}