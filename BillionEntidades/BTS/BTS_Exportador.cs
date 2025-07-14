using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlConexion;

namespace BillionEntidades
{
    public class BTS_Exportador : Cls_Bil_Base
    {

        #region "Variables"

        private int _id;
        private string _ruc = string.Empty;
        private string _nombre = string.Empty;
        private string _linea = string.Empty;


   
        #endregion

        #region "Propiedades"
        public int id { get => _id; set => _id = value; }
        public string ruc { get => _ruc; set => _ruc = value; }
        public string nombre { get => _nombre; set => _nombre = value; }
        public string linea { get => _linea; set => _linea = value; }

  

        #endregion
        private static String v_mensaje = string.Empty;

        public BTS_Exportador()
        {
            init();

        }

        public static List<BTS_Exportador> Buscador_Exportador(string _criterio, out string OnError)
        {
        
            parametros.Clear();
            parametros.Add("pista", _criterio);
            return sql_puntero.ExecuteSelectControl<BTS_Exportador>(sql_puntero.Conexion_Local, 6000, "BTS_BUSCADOR_EXPORTADOR", parametros, out OnError);

        }


        public bool PopulateMyData(out string OnError)
        {

            parametros.Clear();
            parametros.Add("id", this.id);

            var t = sql_puntero.ExecuteSelectOnly<BTS_Exportador>(sql_puntero.Conexion_Local, 6000, "BTS_CARGA_EXPORTADOR", parametros);
            if (t == null)
            {
                OnError = "No fue posible obtener datos del exportador";
                return false;
            }

            this.id = t.id;
            this.ruc = t.ruc;
            this.nombre = t.nombre;
            this.linea = t.linea;

            OnError = string.Empty;
            return true;
        }

        public static List<BTS_Exportador> Cargar_Exportador(string _criterio, out string OnError)
        {

            parametros.Clear();
            parametros.Add("pista", _criterio);
            return sql_puntero.ExecuteSelectControl<BTS_Exportador>(sql_puntero.Conexion_Local, 6000, "BTS_CARGAR_EXPORTADOR", parametros, out OnError);

        }

    }
}
