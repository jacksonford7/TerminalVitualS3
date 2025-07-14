using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlConexion;

namespace BillionEntidades
{
    public class Cls_Bil_Invoice_Pase_Puerta : Cls_Bil_Base
    {
        #region "Variables"

        private Int64 _ID_PPWEB;
        private Int64 _GKEY;
        private string _CONTENEDOR = string.Empty;
        private string _NUMERO_PASE_N4 = string.Empty;
        private decimal _ID_PASE = 0;

        #endregion

        #region "Propiedades"

        public Int64 ID_PPWEB { get => _ID_PPWEB; set => _ID_PPWEB = value; }
        public string CONTENEDOR { get => _CONTENEDOR; set => _CONTENEDOR = value; }       
        public string NUMERO_PASE_N4 { get => _NUMERO_PASE_N4; set => _NUMERO_PASE_N4 = value; }
        public Int64 GKEY { get => _GKEY; set => _GKEY = value; }
        public decimal ID_PASE { get => _ID_PASE; set => _ID_PASE = value; }


        #endregion

        public Cls_Bil_Invoice_Pase_Puerta()
        {
            init();

        }

        /*carga contenedores con fecha ultimos pases de puerta contenedor*/
        public static List<Cls_Bil_Invoice_Pase_Puerta> List_Pase_Puerta(string IV_NUMERO_CARGA, out string OnError)
        {
            parametros.Clear();
            parametros.Add("IV_NUMERO_CARGA", IV_NUMERO_CARGA);
            return sql_puntero.ExecuteSelectControl<Cls_Bil_Invoice_Pase_Puerta>(sql_puntero.Conexion_Local, 5000, "sp_Bil_consulta_pase_puerta", parametros, out OnError);

        }
        /*carga ultimos pase puerta cfs*/
        public static List<Cls_Bil_Invoice_Pase_Puerta> List_Pase_Puerta_cfs(string IV_NUMERO_CARGA, out string OnError)
        {
            parametros.Clear();
            parametros.Add("IV_NUMERO_CARGA", IV_NUMERO_CARGA);
            return sql_puntero.ExecuteSelectControl<Cls_Bil_Invoice_Pase_Puerta>(sql_puntero.Conexion_Local, 5000, "sp_Bil_consulta_pase_puerta_cfs", parametros, out OnError);

        }

        /*carga contenedores con fecha ultimos pases de puerta contenedor carga no exportada*/
        public static List<Cls_Bil_Invoice_Pase_Puerta> List_Pase_Puerta_NoExportada(string IV_NUMERO_CARGA, out string OnError)
        {
            parametros.Clear();
            parametros.Add("IV_NUMERO_CARGA", IV_NUMERO_CARGA);
            return sql_puntero.ExecuteSelectControl<Cls_Bil_Invoice_Pase_Puerta>(sql_puntero.Conexion_Local, 5000, "sp_Bil_consulta_pase_puerta_Expo", parametros, out OnError);

        }

        /*carga ultimos pase puerta break bulk*/
        public static List<Cls_Bil_Invoice_Pase_Puerta> List_Pase_Puerta_brbk(string IV_NUMERO_CARGA, out string OnError)
        {
            parametros.Clear();
            parametros.Add("IV_NUMERO_CARGA", IV_NUMERO_CARGA);
            return sql_puntero.ExecuteSelectControl<Cls_Bil_Invoice_Pase_Puerta>(sql_puntero.Conexion_Local, 5000, "sp_Bil_consulta_pase_puerta_brbk", parametros, out OnError);

        }

    }



}
