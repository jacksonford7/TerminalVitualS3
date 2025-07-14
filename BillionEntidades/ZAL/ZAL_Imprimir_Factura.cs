using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlConexion;

namespace BillionEntidades
{
    public class ZAL_Imprimir_Factura : Cls_Bil_Base
    {

        #region "Propiedades"


        public Int64 ID { get; set; }
        public string GLOSA { get; set; }
        public DateTime? FECHA { get; set; }
        public DateTime? FECHA_HASTA { get; set; }
        public string TIPO_CARGA { get; set; }
        public string ID_CLIENTE { get; set; }
        public string DESC_CLIENTE { get; set; }
        public string ID_FACTURADO { get; set; }
        public string DESC_FACTURADO { get; set; }
        public string REFERENCIA { get; set; }

        public decimal CAB_SUBTOTAL { get; set; }
        public decimal CAB_IVA { get; set; }
        public decimal CAB_TOTAL { get; set; }

        public string DRAF { get; set; }

        public string DIR_FACTURADO { get; set; }
        public string EMAIL_FACTURADO { get; set; }
        public string CIUDAD_FACTURADO { get; set; }
        public int DIAS_CREDITO { get; set; }
        public string SESION { get; set; }


        public string INVOICE_TYPE { get; set; }

        public string RUC_USUARIO { get; set; }
        public string DESC_USUARIO { get; set; }

        public int TOTAL_CONTENEDORES { get; set; }

        public string CONTENEDORES { get; set; }
        public string NUMERO_FACTURA { get; set; }
        public string LINEA { get; set; }
        public string BOOKING { get; set; }

        public int FILA { get; set; }
        public string ID_SERVICIO { get; set; }
        public string DESC_SERVICIO { get; set; }
        public string CARGA { get; set; }
        public DateTime? FECHA_DETALLE { get; set; }
        public string TIPO_SERVICIO { get; set; }

      

        public decimal? CANTIDAD { get; set; }
        public decimal? PRECIO { get; set; }
        public decimal? SUBTOTAL { get; set; }
        public decimal? IVA { get; set; }

        public string USUARIO_CREA { get; set; }
      

        private static String v_mensaje = string.Empty;

        #endregion

        public ZAL_Imprimir_Factura()
        {
            init();

        }

        public static List<ZAL_Imprimir_Factura> Carga_Factura_Zal(string FACTURA, out string OnError)
        {
            parametros.Clear();
            parametros.Add("FACTURA", FACTURA);
            return sql_puntero.ExecuteSelectControl<ZAL_Imprimir_Factura>(sql_puntero.Conexion_Local, 6000, "zal_imprimir_factura", parametros, out OnError);

        }


    }
}
