using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlConexion;

namespace BillionEntidades
{
    public class Cls_TRANSP_Colaborador : Cls_Bil_Base
    {
        private static Int64? lm = -3;

        #region "Propiedades"

     
        public string RUC_EMPRESA { get; set; }
        public string NOMBRE_EMPRESA { get; set; }
        public string NOMINA_COD { get; set; }
        public string COLABORADOR { get; set; }
        public string NOMBRES { get; set; }
        public string FECHA_CADUCIDAD { get; set; }      
        public string ESTADO { get; set; }
        public string NOVEDAD { get; set; }
        public string ESTADO2 { get; set; }
        public int ORDEN { get; set; }
        public string APELLIDOS { get; set; }
        public string TIPOSANGRE { get; set; }
        public string DIRECCIONDOM { get; set; }
        public string TELFDOM { get; set; }
        public DateTime FECHANAC { get; set; }
        public string CARGO { get; set; }
        #endregion



        public Cls_TRANSP_Colaborador()
        {
            init();
        }

        private static void OnInit(string Base)
        {
            sql_puntero = (sql_puntero == null) ? Cls_Conexion.Conexion() : sql_puntero;
            parametros = new Dictionary<string, object>();
            nueva_conexion = Cls_Conexion.Nueva_Conexion(Base);
        }

        public static List<Cls_TRANSP_Colaborador> Carga_Colaborador(string RUC, out string OnError)
        {
            OnInit("MDA");
            parametros.Add("RUC", RUC);
            return sql_puntero.ExecuteSelectControl<Cls_TRANSP_Colaborador>(nueva_conexion, 7000, "TRANSP_CONSULTA_DATOS_COLABORADOR", parametros, out OnError);

        }


    }
}
