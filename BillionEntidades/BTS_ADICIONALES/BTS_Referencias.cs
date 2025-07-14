using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlConexion;

namespace BillionEntidades
{
    public class BTS_Referencias : Cls_Bil_Base
    {

        #region "Variables"

        private int _id;
        private string _idNave = string.Empty;
      

   
        #endregion

        #region "Propiedades"
        public int id { get => _id; set => _id = value; }
        public string idNave { get => _idNave; set => _idNave = value; }
       
  

        #endregion
        private static String v_mensaje = string.Empty;

        public BTS_Referencias()
        {
            init();

        }

        public static List<BTS_Referencias> Buscador_Referencia(string _criterio, out string OnError)
        {
        
            parametros.Clear();
            parametros.Add("pista", _criterio);
            return sql_puntero.ExecuteSelectControl<BTS_Referencias>(sql_puntero.Conexion_Local, 6000, "BTS_BUSCADOR_REFERENCIAS", parametros, out OnError);

        }


    

    }
}
