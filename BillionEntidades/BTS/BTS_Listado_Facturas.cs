using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlConexion;

namespace BillionEntidades
{
    public class BTS_Listado_Facturas : Cls_Bil_Base
    {


        #region "Propiedades"


        public Int64 ID { get; set; }
        public string GLOSA { get; set; }
        public string FECHA { get; set; }
        public string TIPO_CARGA { get; set; }
        public string ID_CLIENTE { get; set; }
        public string DESC_CLIENTE { get; set; }
        public string ID_FACTURADO { get; set; }
        public string DESC_FACTURADO { get; set; }
        public string REFERENCIA { get; set; }
        public string VENCIMIENTO { get; set; }
        public decimal SUBTOTAL { get; set; }
        public decimal IVA { get; set; }
        public decimal TOTAL { get; set; }



        public string DIR_FACTURADO { get; set; }
        public string EMAIL_FACTURADO { get; set; }
        public string CIUDAD_FACTURADO { get; set; }
        public Int64 DIAS_CREDITO { get; set; }
        public string SESION { get; set; }

  
    

        public int CAJAS_BODEGA { get; set; }
        public int CAJAS_MUELLE { get; set; }
        public string DRAF { get; set; }
        public string FACTURA { get; set; }


        private static String v_mensaje = string.Empty;


        #endregion

        public BTS_Listado_Facturas()
        {
            init();

        }

        /*carga todas los draft*/
        public static List<BTS_Listado_Facturas> Datos_Draft(DateTime FECHA_DESDE, DateTime FECHA_HASTA, string USUARIO, string RUC, out string OnError)
        {
            parametros.Clear();
            parametros.Add("IV_FECHA_DESDE", FECHA_DESDE);
            parametros.Add("IV_FECHA_HASTA", FECHA_HASTA);
            parametros.Add("IV_USUARIO", USUARIO);
            parametros.Add("IV_RUC", RUC);
            return sql_puntero.ExecuteSelectControl<BTS_Listado_Facturas>(sql_puntero.Conexion_Local, 4000, "Bts_Listado_DraftAgencia", parametros, out OnError);

        }

        /*carga todas las facturas*/
        public static List<BTS_Listado_Facturas> Datos_Facturas(DateTime FECHA_DESDE, DateTime FECHA_HASTA, string USUARIO, string RUC, out string OnError)
        {
            parametros.Clear();
            parametros.Add("IV_FECHA_DESDE", FECHA_DESDE);
            parametros.Add("IV_FECHA_HASTA", FECHA_HASTA);
            parametros.Add("IV_USUARIO", USUARIO);
            parametros.Add("IV_RUC", RUC);
            return sql_puntero.ExecuteSelectControl<BTS_Listado_Facturas>(sql_puntero.Conexion_Local, 4000, "Bts_Listado_FacturasAgencia", parametros, out OnError);

        }

    }
}
