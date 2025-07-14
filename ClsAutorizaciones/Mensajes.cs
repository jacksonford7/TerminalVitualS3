using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ControlOPC.Entidades;

namespace ClsAutorizaciones
{
    public class Mensajes : Base
    {
        #region "Variables"

        private Int64 _ID_MENSAJE =0;
        private int _TIPO = 1;
        private string _CAMPO = string.Empty;
        private string _MENSAJE = string.Empty;
       


        #endregion

        #region "Propiedades"
        public Int64 ID_MENSAJE { get => _ID_MENSAJE; set => _ID_MENSAJE = value; }
        public int TIPO { get => _TIPO; set => _TIPO = value; }
        public string CAMPO { get => _CAMPO; set => _CAMPO = value; }
        public string MENSAJE { get => _MENSAJE; set => _MENSAJE = value; }

        #endregion

        private static String v_mensaje = string.Empty;

        public Mensajes()
        {
            init();
        }

        private static void OnInit()
        {
            sql_pointer = (sql_pointer == null) ? SQLHandler.sql_handler.GetInstance() : sql_pointer;
            parametros = new Dictionary<string, object>();

            v_conexion = Extension.Nueva_Conexion("PORTAL_N4");
        }

        /*listado mensajes*/
        public static List<Mensajes> Listar_Mensajes(out string OnError)
        {
            OnInit();     
            return sql_pointer.ExecuteSelectControl<Mensajes>(v_conexion, 6000, "RVA_LISTA_AUTORIZA_MENSAJES", null, out OnError);
        }

    }
}
