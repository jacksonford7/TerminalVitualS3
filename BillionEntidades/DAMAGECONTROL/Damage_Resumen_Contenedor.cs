using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlConexion;

namespace BillionEntidades
{
    public class Damage_Resumen_Contenedor : Cls_Bil_Base
    {

        #region "Variables"
        private static Int64? lm = -3;

        private Int64 _GKEY;
        private string _CONTENEDOR = string.Empty;
        private DateTime? _FECHA_REGISTRO = null;

     

        #endregion

        #region "Propiedades"
     
           
        public Int64 GKEY { get => _GKEY; set => _GKEY = value; }
        public string CONTENEDOR { get => _CONTENEDOR; set => _CONTENEDOR = value; }
        public DateTime? FECHA_REGISTRO { get => _FECHA_REGISTRO; set => _FECHA_REGISTRO = value; }

      

        private static String v_mensaje = string.Empty;


        #endregion


        public Damage_Resumen_Contenedor()
        {
            init();

        }

        public static List<Damage_Resumen_Contenedor> Lista_Resumen_Contenedor(string RUC, out string OnError)
        {
            
            parametros.Add("IMPORTADOR", RUC);
            return sql_puntero.ExecuteSelectControl<Damage_Resumen_Contenedor>(sql_puntero.Conexion_Local, 7000, "DAMAGE_RESUMEN_CONTENEDORES_IMPORTADOR", parametros, out OnError);

        }

    }
}
