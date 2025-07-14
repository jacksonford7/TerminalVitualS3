using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Respuesta;
using AccesoDatos;
using Configuraciones;
using System.Xml.Serialization;
using System.Reflection;

namespace N4.Exportacion
{
    public class expo_invoice_detail:ModuloBase
    {
        public Int64? id { get; set; }
        public Int64? bil_expo_cab_invoice_id { get; set; }
        public string totalCharged { get; set; }
        public string billingDate { get; set; }
        public string description { get; set; }
        public string chargeEntityId { get; set; }
        public string chargeEventTypeId { get; set; }
        public string customerTariffId { get; set; }
        public string notes { get; set; }
        public string flatRateAmount { get; set; }
        public string eventPerformedFrom { get; set; }
        public string chargeGlCode { get; set; }
        public string eventPerformedTo { get; set; }
        public string gkey { get; set; }
        public string extractGkey { get; set; }
        public string extractClass { get; set; }
        public string quantity { get; set; }
        public string quantityBilled { get; set; }
        public string quantityUnit { get; set; }
        public string rateBilled { get; set; }
        public string isFlatRate { get; set; }
        public string totalTaxes { get; set; }
        public string tariff_id { get; set; }
        public string tariff_amount { get; set; }
        public string tariff_longDescription { get; set; }
        public DateTime? created_date { get; set; }
        public string created_user { get; set; }
        public bool? register_Active { get; set; }

        public override void OnInstanceCreate()
        {
            this.alterClase = "N4";
            base.OnInstanceCreate();
            this.Accesorio.ConfiguracionBase = "DATACON";
        }
        public expo_invoice_detail()
        {

        }

    }
    /*nuevo_bil_expo_cab_invoice_charge*/
}
