using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlConexion;

namespace BillionEntidades
{
    public class ZAL_Cabecera_Factura : Cls_Bil_Base
    {



        #region "Propiedades"


        public int fila { get; set; }
        public string turno_id { get; set; }
        public DateTime? turno_fecha { get; set; }
        public string turno_hora { get; set; }
        public string unidad_id { get; set; }
        public string unidad_tamano { get; set; }
        public string unidad_linea { get; set; }
        public string unidad_booking { get; set; }
        public string unidad_referencia { get; set; }
        public string unidad_estatus { get; set; }
        public Int64? unidad_key { get; set; }

        public string chofer_licencia { get; set; }
        public string chofer_nombre { get; set; }
        public string vehiculo_placa { get; set; }
        public string vehiculo_desc { get; set; }

        public string creado_usuario { get; set; }
        public DateTime? creado_fecha { get; set; }

        public Int64? n4_unit_key { get; set; }
        public string n4_message { get; set; }
        public bool active { get; set; }
        public int id { get; set; }
        public string documento_id { get; set; }
        public string estado_pago { get; set; }
        public string estado { get; set; }

        public string ruc_cliente { get; set; }
        public string name_cliente { get; set; }
        public string ruc_asume { get; set; }
        public string name_asume { get; set; }
        public int cantidad { get; set; }
        public string ruc_facturar { get; set; }
        public string cliente_facturar { get; set; }
        //public string numero_factura { get; set; }

        public string asume_ruc_facturar { get; set; }
        public string asume_cliente_facturar { get; set; }
        public Int64? id_facturar { get; set; }
        public bool visto { get; set; }
        //public string invoice_type { get; set; }

        public string SESION { get ; set ; }
     
        private static String v_mensaje = string.Empty;


        #endregion


        #region "Propiedes de factura"
        public Int64 ID { get; set; }
        public string GLOSA { get; set; }
        public DateTime? FECHA { get; set; }
        public string TIPO_CARGA { get; set; }
        public string ID_CLIENTE { get; set; }
        public string DESC_CLIENTE { get; set; }
        public string ID_FACTURADO { get; set; }
        public string DESC_FACTURADO { get; set; }
        public string REFERENCIA { get; set; }

        public decimal SUBTOTAL { get; set; }
        public decimal IVA { get; set; }
        public decimal TOTAL { get; set; }

        public string DIR_FACTURADO { get; set; }
        public string EMAIL_FACTURADO { get; set; }
        public string CIUDAD_FACTURADO { get; set; }
        public Int64 DIAS_CREDITO { get; set; }
    
        public string INVOICE_TYPE { get; set; }

        public string RUC_USUARIO { get; set; }
        public string DESC_USUARIO { get; set; }

        public int TOTAL_CONTENEDOR { get; set; }
        public string CONTENEDORES { get; set; }
        public string DRAF { get; set; }
        public string NUMERO_FACTURA { get; set; }

        public string LINEA { get; set; }
        public string BOOKING { get; set; }


        public Int64 ID_DEPOSITO { get; set; }

        public string xmlCabecera { get; set; }
        public string xmlServicios { get; set; }
        public string xmlPases { get; set; }
        private static Int64? lm = -3;

        #endregion


        public List<ZAL_Detalle_Factura> Detalle_Pases { get; set; }
        public List<ZAL_Clientes> Detalle_Clientes { get; set; }
        public List<ZAL_Detalle_Pases> Detalle_Factura { get; set; }
        public List<ZAL_Detalle_Error> Detalle_Errores { get; set; }
        public List<ZAL_Detalle_Servicios> Detalle_Servicios { get; set; }
        public List<ZAL_Detalle_Ok> Detalle_Ok { get; set; }

        public ZAL_Cabecera_Factura()
        {
            init();

            this.Detalle_Pases = new List<ZAL_Detalle_Factura>();
            this.Detalle_Clientes = new List<ZAL_Clientes>();
            this.Detalle_Factura = new List<ZAL_Detalle_Pases>();
            this.Detalle_Errores = new List<ZAL_Detalle_Error>();
            this.Detalle_Servicios = new List<ZAL_Detalle_Servicios>();
            this.Detalle_Ok = new List<ZAL_Detalle_Ok>();

        }

        private Int64? Save_Factura(out string OnError)
        {

            parametros.Clear();

            parametros.Add("xmlCabecera", this.xmlCabecera);
            parametros.Add("xmlServicios", this.xmlServicios);
            parametros.Add("xmlPases", this.xmlPases);


            var db = sql_puntero.ExecuteInsertUpdateDeleteReturn(sql_puntero.Conexion_Local, 8000, "Zal_Procesa_Factura", parametros, out OnError);
            if (!db.HasValue || db.Value < 0)
            {
                return null;
            }
            OnError = string.Empty;
            return db.Value;

        }

        public Int64? SaveTransaction(out string OnError)
        {

            Int64 ID = 0;
            try
            {

                //grabar transaccion.
                var id = this.Save_Factura(out OnError);
                if (!id.HasValue)
                {
                    OnError = "*** Error: al grabar factura de retiro de contenedores ****";
                    return 0;
                }
                ID = id.Value;

                return ID;

            }
            catch (Exception ex)
            {

                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>("SQL", nameof(SaveTransaction), "SaveTransaction", false, null, null, ex.StackTrace, ex);
                OnError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));

                return null;
            }
        }




    }
}
