using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace CSLSite.N4Object
{
    [Serializable]
    [XmlRoot(ElementName = "bill-of-lading")]
    public class billoflading
    {
       public billoflading()
        {
            this.items = new List<blitem>(1);
            this.goodsbl = new List<goodsbl>();
        }
        [XmlArray("items")]
        [XmlArrayItem("item", typeof(blitem))]
        public List<blitem> items { get; set; }
        [XmlElement(ElementName = "goods-bl")]
        public List<goodsbl> goodsbl { get; set; }
        [XmlAttribute(AttributeName = "nbr")]
        public string nbr { get; set; }
        [XmlAttribute(AttributeName = "line")]
        public string line { get; set; }
        [XmlAttribute(AttributeName = "carrier-visit")]
        public string carriervisit { get; set; }
        [XmlAttribute(AttributeName = "category")]
        public category category { get; set; }
        [XmlAttribute(AttributeName = "consignee-id")]
        public string consigneeid  { get; set; }
        [XmlAttribute(AttributeName = "consignee-name")]
        public string consigneename { get; set; }
        [XmlAttribute(AttributeName = "shipper-id")]
        public string shipperid { get; set; }
        [XmlAttribute(AttributeName = "shipper-name")]
        public string shippername { get; set; }
        [XmlAttribute(AttributeName = "pol")]
        public string pol { get; set; }
        [XmlAttribute(AttributeName = "pod-1")]
        public string pod1{ get; set; }
        [XmlAttribute(AttributeName = "pod-2")]
        public string pod2 { get; set; }
        [XmlAttribute(AttributeName = "released-quantity")]
        public string releasedquantity { get; set; }
        [XmlAttribute(AttributeName = "entered-quantity")]
        public string enteredquantity{ get; set; }
        [XmlAttribute(AttributeName = "notes")]
        public string notes { get; set; }
        [XmlAttribute(AttributeName = "origin")]
        public string origin { get; set; }
        //origin
    }
    #region "Clases auxiliares"
    [Serializable]
    public partial class goodsbl
    {
        [XmlAttribute(AttributeName = "unit-id")]
        public string unitid {get;set;}

        [XmlAttribute(AttributeName = "unit-key")]
        public string unitkey {get;set;}
    }
    [Serializable]
    public partial class blitem
    {
        public blitem()
        {
            this.cargolots =  new List<cargolot>();
        }
        [XmlArray("cargo-lots")]
        [XmlArrayItem("cargo-lot", typeof(cargolot))]
        public List<cargolot> cargolots { get; set; }

        [XmlAttribute(AttributeName = "commodity-id")]
        public string commodityid{ get; set; }

        [XmlAttribute(AttributeName = "nbr")]
        public string nbr { get; set; }

        [XmlAttribute(AttributeName = "is-bulk")]
        public string isbulk { get; set; }

        [XmlAttribute(AttributeName = "quantity")]
        public string quantity { get; set; }

        ////[XmlAttribute(AttributeName = "package-type")]
        ////public string packagetype{ get; set; }

        //[XmlAttribute(AttributeName = "package-weight-kg")]
        //public decimal packageweightkg{ get; set; }

        //[XmlAttribute(AttributeName = "product-type")]
        //public string producttype { get; set; }

        [XmlAttribute(AttributeName = "notes")]
        public string notes{ get; set; }

        [XmlAttribute(AttributeName = "marks-and-numbers")]
        public string marksandnumbers{ get; set; }

        [XmlAttribute(AttributeName = "weight-total-kg")]
        public string weighttotalkg{ get; set; }

        [XmlAttribute(AttributeName = "commodity-note")]
        public string commoditynote{ get; set; }

    }
    [Serializable]
    public partial class cargolot
    {

        public cargolot()
        {
            this.position = new position();
        }
        [XmlElement(ElementName = "position")]
        public position position{ get; set;}

        [XmlAttribute(AttributeName = "quantity")]
        public string quantity { get; set; }

        [XmlAttribute(AttributeName = "unit-id")]
        public string unitid{get;set;}

        [XmlAttribute(AttributeName = "lot-id")]
        public string lotid { get; set; }

        [XmlAttribute(AttributeName = "quantity-manifiested")]
        public string quantitymanifiested{get;set;}

        [XmlAttribute(AttributeName = "is-default-lot")]
        public string isdefaultlot {get;set;}

        [XmlAttribute(AttributeName = "lot-weight-total-kg")]
        public string lotweighttotalkg {get;set;}

    }
    public partial class position
    {
        [XmlAttribute(AttributeName = "loc-type")]
        public locacion loctype {get;set;}

        [XmlAttribute(AttributeName = "location")]
        public string location {get;set;}

        [XmlAttribute(AttributeName = "slot")]
        public string slot {get;set;}

        [XmlAttribute(AttributeName = "orientation")]
        public string orientation {get;set;}

        [XmlAttribute(AttributeName = "carrier-id")]
        public string carrierid {get;set;}

    }
    #endregion
    #region "Enumeraciones"
    [Serializable]
    public enum locacion
    {
        YARD,
        VESSEL,
        TRUCK,
        RAILCAR,
        TRAIN,
        CONTAINER,
    }
    #endregion
}