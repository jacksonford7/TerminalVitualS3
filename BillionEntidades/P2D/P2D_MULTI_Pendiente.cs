using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlConexion;

namespace BillionEntidades
{
    public class P2D_MULTI_Pendiente : Cls_Bil_Base
    {

        #region "Variables"
        private static Int64? lm = -3;

        private Int64 _IV_ID;
        private string _IV_FECHA = string.Empty;
        private decimal _IV_TOTAL;
        private string _IV_NUMERO_CARGA = string.Empty;
        private string _IV_FACTURA = string.Empty;

        #endregion

        #region "Propiedades"

        public Int64 IV_ID { get => _IV_ID; set => _IV_ID = value; }
        public string IV_FECHA { get => _IV_FECHA; set => _IV_FECHA = value; }
        public decimal IV_TOTAL { get => _IV_TOTAL; set => _IV_TOTAL = value; }
        public string IV_NUMERO_CARGA { get => _IV_NUMERO_CARGA; set => _IV_NUMERO_CARGA = value; }
        public string IV_FACTURA { get => _IV_FACTURA; set => _IV_FACTURA = value; }
        #endregion



        public P2D_MULTI_Pendiente()
        {
            init();



        }


        #region "Listados"

        public static List<P2D_MULTI_Pendiente> cargar_facturas_emitirpase(string ruc, out string OnError)
        {

            parametros.Add("ruc", ruc);
            return sql_puntero.ExecuteSelectControl<P2D_MULTI_Pendiente>(sql_puntero.Conexion_Local, 6000, "PD2_MULTI_Carga_Facturas_PasePuerta", parametros, out OnError);

        }
        public static List<P2D_MULTI_Pendiente> Valida_TieneFactura(string MRN, string MSN, string HSN, out string OnError)
        {

            parametros.Clear();
            parametros.Add("MRN", MRN);
            parametros.Add("MSN", MSN);
            parametros.Add("HSN", HSN);
            return sql_puntero.ExecuteSelectControl<P2D_MULTI_Pendiente>(sql_puntero.Conexion_Local, 6000, "P2D_MULTI_Valida_Carga", parametros, out OnError);

        }
        #endregion








    }
}
