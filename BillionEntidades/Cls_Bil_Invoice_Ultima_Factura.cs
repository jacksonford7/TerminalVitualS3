using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlConexion;

namespace BillionEntidades
{
    public class Cls_Bil_Invoice_Ultima_Factura : Cls_Bil_Base
    {
        #region "Variables"

        private Int64 _IV_ID;
        private DateTime? _IV_FECHA = null;
        private Int64 _IV_GKEY;
        private string _IV_NUMERO_CARGA = string.Empty;
        private string _IV_FACTURA = string.Empty;
        private DateTime? _IV_FECHA_HASTA = null;
        private string _IV_MODULO = string.Empty;

        #endregion

        #region "Propiedades"

        public Int64 IV_ID { get => _IV_ID; set => _IV_ID = value; }
        public string IV_NUMERO_CARGA { get => _IV_NUMERO_CARGA; set => _IV_NUMERO_CARGA = value; }
        public DateTime? IV_FECHA { get => _IV_FECHA; set => _IV_FECHA = value; }
        public string IV_FACTURA { get => _IV_FACTURA; set => _IV_FACTURA = value; }
        public Int64 IV_GKEY { get => _IV_GKEY; set => _IV_GKEY = value; }
        public DateTime? IV_FECHA_HASTA { get => _IV_FECHA_HASTA; set => _IV_FECHA_HASTA = value; }
        public string IV_MODULO { get => _IV_MODULO; set => _IV_MODULO = value; }


        #endregion

        public Cls_Bil_Invoice_Ultima_Factura()
        {
            init();

        }

        /*carga contenedores con fecha y ultima factura billion*/
        public static List<Cls_Bil_Invoice_Ultima_Factura> List_Ultima_Factura(string IV_NUMERO_CARGA, out string OnError)
        {
            parametros.Clear();
            parametros.Add("IV_NUMERO_CARGA", IV_NUMERO_CARGA);
            return sql_puntero.ExecuteSelectControl<Cls_Bil_Invoice_Ultima_Factura>(sql_puntero.Conexion_Local, 6000, "sp_Bil_consulta_ultima_factura", parametros, out OnError);

        }

        /*carga contenedores con fecha y ultima factura billion carga cfs*/
        public static List<Cls_Bil_Invoice_Ultima_Factura> List_Ultima_Factura_cfs(string IV_NUMERO_CARGA, out string OnError)
        {
            parametros.Clear();
            parametros.Add("IV_NUMERO_CARGA", IV_NUMERO_CARGA);
            return sql_puntero.ExecuteSelectControl<Cls_Bil_Invoice_Ultima_Factura>(sql_puntero.Conexion_Local, 6000, "sp_Bil_consulta_ultima_factura_cfs", parametros, out OnError);

        }

        /*carga contenedores con fecha y ultima factura billion*/
        public static List<Cls_Bil_Invoice_Ultima_Factura> List_Ultima_Factura_Expo(string BOOKING, out string OnError)
        {
            parametros.Clear();
            parametros.Add("BOOKING", BOOKING);
            return sql_puntero.ExecuteSelectControl<Cls_Bil_Invoice_Ultima_Factura>(sql_puntero.Conexion_Local, 6000, "sp_Bil_consulta_ultima_factura_expo", parametros, out OnError);

        }

        /*carga contenedores con fecha y ultima factura billion carga no exportada*/
        public static List<Cls_Bil_Invoice_Ultima_Factura> List_Ultima_Factura_Expo_NoExportada(string BOOKING, out string OnError)
        {
            parametros.Clear();
            parametros.Add("IV_NUMERO_CARGA", BOOKING);
            return sql_puntero.ExecuteSelectControl<Cls_Bil_Invoice_Ultima_Factura>(sql_puntero.Conexion_Local, 6000, "sp_Bil_consulta_ultima_factura_expo_new", parametros, out OnError);

        }

        /*carga contenedores con fecha y ultima factura billion carga brbk*/
        public static List<Cls_Bil_Invoice_Ultima_Factura> List_Ultima_Factura_brbk(string IV_NUMERO_CARGA, out string OnError)
        {
            parametros.Clear();
            parametros.Add("IV_NUMERO_CARGA", IV_NUMERO_CARGA);
            return sql_puntero.ExecuteSelectControl<Cls_Bil_Invoice_Ultima_Factura>(sql_puntero.Conexion_Local, 6000, "sp_Bil_consulta_ultima_factura_brbk", parametros, out OnError);

        }


        /*carga contenedores con fecha y ultima factura billion*/
        //public static List<Cls_Bil_Invoice_Ultima_Factura> List_Ultima_Factura_Middleware(string IV_NUMERO_CARGA, out string OnError)
        //{
        //    parametros.Clear();
        //    parametros.Add("IV_NUMERO_CARGA", IV_NUMERO_CARGA);
        //    return sql_puntero.ExecuteSelectControl<Cls_Bil_Invoice_Ultima_Factura>(sql_puntero.Conexion_Local, 4000, "sp_Bil_consulta_ultima_factura_N4Middleware", parametros, out OnError);

        //}

    }
}
