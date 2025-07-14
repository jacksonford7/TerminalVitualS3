using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlConexion;

namespace BillionEntidades
{
    public class Cls_TRANSP_Vehiculo : Cls_Bil_Base
    {
        private static Int64? lm = -3;

        #region "Propiedades"

     
        public string RUC_EMPRESA { get; set; }
        public string NOMBRE_EMPRESA { get; set; }
        public string PLACA { get; set; }
        public string VE_POLIZA { get; set; }
        public string ESTADO { get; set; }
        public string NOVEDAD { get; set; }      
        public int ORDEN { get; set; }

        public string CLASETIPO { get; set; }
        public string MARCA { get; set; }
        public string MODELO { get; set; }
        public string COLOR { get; set; }
        public string TIPOCERTIFICADO { get; set; }
        public string CERTIFICADO { get; set; }
        public string CATEGORIA { get; set; }
        public string DESCRIPCIONCATEGORIA { get; set; }

        public DateTime? FECHAPOLIZA { get; set; }
        public DateTime? FECHAMTOP { get; set; }
        public string TIPO { get; set; }
        #endregion



        public Cls_TRANSP_Vehiculo()
        {
            init();
        }

        private static void OnInit(string Base)
        {
            sql_puntero = (sql_puntero == null) ? Cls_Conexion.Conexion() : sql_puntero;
            parametros = new Dictionary<string, object>();
            nueva_conexion = Cls_Conexion.Nueva_Conexion(Base);
        }

        public static List<Cls_TRANSP_Vehiculo> Carga_Vehiculo(string RUC, out string OnError)
        {
            OnInit("MDA");
            parametros.Add("RUC", RUC);
            return sql_puntero.ExecuteSelectControl<Cls_TRANSP_Vehiculo>(nueva_conexion, 7000, "TRANSP_CONSULTA_DATOS_VEHICULO", parametros, out OnError);

        }


    }
}
