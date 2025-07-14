using BillionEntidades;
using SqlConexion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CSLSite.Cls_Container
{
    public class Cls_Container : Cls_Bil_Base
    {
        public Int64 CNTR_CONSECUTIVO { get; set; }
        public string CNTR_CONTAINER { get; set; }
        public string CNTR_TYPE { get; set; }
        public string CNTR_TYSZ_SIZE { get; set; }
        public string CNTR_TYSZ_ISO { get; set; }
        public string CNTR_TYSZ_TYPE { get; set; }
        public string CNTR_FULL_EMPTY_CODE { get; set; }
        public string CNTR_YARD_STATUS { get; set; }
        public double CNTR_TEMPERATURE { get; set; }
        public string CNTR_TYPE_DOCUMENT { get; set; }
        public string CNTR_DOCUMENT { get; set; }
        public string CNTR_VEPR_REFERENCE { get; set; }
        public string CNTR_CLNT_CUSTOMER_LINE { get; set; }
        public string CNTR_LCL_FCL { get; set; }
        public string CNTR_CATY_CARGO_TYPE { get; set; }
        public string CNTR_FREIGHT_KIND { get; set; }
        public Int32? CNTR_DD { get; set; }
        public string CNTR_BKNG_BOOKING { get; set; }
        public DateTime? FECHA_CAS { get; set; }
        public string  CNTR_AISV { get; set; }
        public Int32? CNTR_HOLD { get; set; }
        public string CNTR_REEFER_CONT { get; set; }
        public string CNTR_VEPR_VSSL_NAME { get; set; }
        public string CNTR_VEPR_VOYAGE { get; set; }
        public DateTime? CNTR_VEPR_ACTUAL_ARRIVAL { get; set; }
        public DateTime? CNTR_VEPR_ACTUAL_DEPARTED { get; set; }
        public decimal CNTR_GROSS_WEIGHT { get; set; }

        public string CNTR_CITY_UNLOADED { get; set; }

        public string CNTR_CITY_ARRIVE { get; set; }
        public string CNTR_CITY_LOADED { get; set; }
        public DateTime CNTR_VEPR_ESTIMADO_ARRIVAL { get; set; }
        public DateTime CNTR_VEPR_ESTIMADO_DEPARTED { get; set; }



        public Cls_Container() : base()
        {
            init();
        }

        private static void OnInit(string Base)
        {
            sql_puntero = (sql_puntero == null) ? Cls_Conexion.Conexion() : sql_puntero;
            parametros = new Dictionary<string, object>();
            nueva_conexion = Cls_Conexion.Nueva_Conexion(Base);
        }

        public static List<Cls_Container> CargaPorBooking(string booking, string ctnr)
        {

            OnInit("N5");

            parametros.Add("booking", booking);
            parametros.Add("CONTENEDOR", ctnr);
            string controlError = string.Empty;

            var resultadoConsulta = sql_puntero.ExecuteSelectControl<Cls_Container>(
                               nueva_conexion,
                               4000,
                               "[Bill].[container_expo_booking_id]",
                               parametros,
                               out controlError
                           );


            return resultadoConsulta ?? new List<Cls_Container>();
        }

    }


}