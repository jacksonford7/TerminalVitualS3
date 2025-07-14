using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlConexion;


namespace ClsAppCgsa
{
    public class EventoN4 : Base
    {

        #region "Variables"

        private string _ID = string.Empty;
        private string _NAME = string.Empty;

        private string _MENSAJE = string.Empty;

        #endregion

        #region "Propiedades"
        public string ID { get => _ID; set => _ID = value; }
        public string NAME { get => _NAME; set => _NAME = value; }

        #endregion

        

        public EventoN4()
        {
            init();
        }

        private static void OnInit()
        {
            sql_puntero = (sql_puntero == null) ? Cls_Conexion.Conexion() : sql_puntero;
            parametros = new Dictionary<string, object>();
            nueva_conexion = Cls_Conexion.Nueva_Conexion("N5");
        }

        /*listado*/
        public static List<EventoN4> Buscador_Eventos(string _criterio, out string OnError)
        {
            OnInit();
            parametros.Clear();
            parametros.Add("pista", _criterio);
            return sql_puntero.ExecuteSelectControl<EventoN4>(nueva_conexion, 6000, "SP_GET_INFO_EVENT", parametros, out OnError);
        }

    }
}
