using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlConexion;

namespace BillionEntidades
{
    public class Cls_TRANSP_Colaborador_Only : Cls_Bil_Base
    {
        private static Int64? lm = -3;

        #region "Propiedades"

     
        public int NOMINA_CAL { get; set; }
        public int NOMINA_AREA { get; set; }
        public int NOMINA_DEP { get; set; }
        public string NOMINA_CAL1 { get; set; }
        public string NOMINA_EMPE { get; set; }
        public string NOMINA_AREA1 { get; set; }
        public string NOMINA_DEP1 { get; set; }
        public DateTime? NOMINA_FING { get; set; }
        public DateTime? NOMINA_FCARD { get; set; }
        #endregion



        public Cls_TRANSP_Colaborador_Only()
        {
            init();
        }

        private static void OnInit(string Base)
        {
            sql_puntero = (sql_puntero == null) ? Cls_Conexion.Conexion() : sql_puntero;
            parametros = new Dictionary<string, object>();
            nueva_conexion = Cls_Conexion.Nueva_Conexion(Base);
        }

        public static List<Cls_TRANSP_Colaborador_Only> Datos_Colaborador(string CEDULA, out string OnError)
        {
            OnInit("MDA");
            parametros.Add("CEDULA", CEDULA);
            return sql_puntero.ExecuteSelectControl<Cls_TRANSP_Colaborador_Only>(nueva_conexion, 7000, "TRANSP_CONSULTA_CEDULA_COLABORADOR", parametros, out OnError);

        }


    }
}
