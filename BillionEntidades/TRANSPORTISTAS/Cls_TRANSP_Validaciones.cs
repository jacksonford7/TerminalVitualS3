using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlConexion;

namespace BillionEntidades
{
    public class Cls_TRANSP_Validaciones : Cls_Bil_Base
    {
        private static Int64? lm = -3;

        #region "Propiedades"

     
        public string CHOFER { get; set; }
        public string EMPRESA { get; set; }
        public DateTime? FECHA_CADUCIDAD  { get; set; }
        public DateTime? EXPIRACION_LICENCIA { get; set; }
        public string LICENCIA { get; set; }
        public string STATUS { get; set; }
        public string MOTIVO_BLOQUEO { get; set; }

        public DateTime? EXPIRACION_POLIZA { get; set; }
        public string PLACA { get; set; }


        public DateTime? EXPIRACION_MTOP { get; set; }
        #endregion



        public Cls_TRANSP_Validaciones()
        {
            init();
        }

        private static void OnInit(string Base)
        {
            sql_puntero = (sql_puntero == null) ? Cls_Conexion.Conexion() : sql_puntero;
            parametros = new Dictionary<string, object>();
            nueva_conexion = Cls_Conexion.Nueva_Conexion(Base);
        }

        public static List<Cls_TRANSP_Validaciones> Valida_Licencia(string CEDULA, string EMPRESA, out string OnError)
        {
            OnInit("N5");
            parametros.Add("CEDULA", CEDULA);
            parametros.Add("EMPRESA", EMPRESA);
            return sql_puntero.ExecuteSelectControl<Cls_TRANSP_Validaciones>(nueva_conexion, 7000, "TRANSP_VALIDA_FECHA_LICENCIA", parametros, out OnError);

        }

        public static List<Cls_TRANSP_Validaciones> Valida_Poliza(string VEHICULO, string EMPRESA, out string OnError)
        {
            OnInit("N5");
            parametros.Add("VEHICULO", VEHICULO);
            parametros.Add("EMPRESA", EMPRESA);
            return sql_puntero.ExecuteSelectControl<Cls_TRANSP_Validaciones>(nueva_conexion, 7000, "TRANSP_VALIDA_FECHA_POLIZA", parametros, out OnError);

        }

        public static List<Cls_TRANSP_Validaciones> Valida_Mtop(string VEHICULO, string EMPRESA, out string OnError)
        {
            OnInit("N5");
            parametros.Add("VEHICULO", VEHICULO);
            parametros.Add("EMPRESA", EMPRESA);
            return sql_puntero.ExecuteSelectControl<Cls_TRANSP_Validaciones>(nueva_conexion, 7000, "TRANSP_VALIDA_FECHA_MTOP", parametros, out OnError);

        }

    }
}
