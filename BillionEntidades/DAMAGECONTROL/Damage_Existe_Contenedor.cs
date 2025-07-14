using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlConexion;


namespace BillionEntidades
{
    public class Damage_Existe_Contenedor : Cls_Bil_Base
    {
        #region "Variables"
        private Int64 _gkey;
        private string _contenedor = string.Empty;
        private int _servicio;
        private bool _existe;
        #endregion


        #region "Propiedades"
        public Int64 gkey { get => _gkey; set => _gkey = value; }
        public string contenedor { get => _contenedor; set => _contenedor = value; }
        public int servicio { get => _servicio; set => _servicio = value; }
        public bool existe { get => _existe; set => _existe = value; }
        #endregion

        public Damage_Existe_Contenedor()
        {
            init();
        }

        private static void OnInit(string Base)
        {
            sql_puntero = (sql_puntero == null) ? Cls_Conexion.Conexion() : sql_puntero;
            parametros = new Dictionary<string, object>();
            nueva_conexion = Cls_Conexion.Nueva_Conexion(Base);
        }

        public static List<Damage_Existe_Contenedor> Existen_Imagenes_Contenedores(string XmlContenedor, out string OnError)
        {

            parametros.Clear();
            parametros.Add("XmlContenedor", XmlContenedor);
            return sql_puntero.ExecuteSelectControl<Damage_Existe_Contenedor>(sql_puntero.Conexion_Local, 6000, "DAMAGE_EXISTEN_CONTENEDORES", parametros, out OnError);

        }

        public static List<Damage_Existe_Contenedor> Validacion_Servicio_Imagen(string XmlContenedor, out string OnError)
        {
            OnInit("N5");

            parametros.Clear();
            parametros.Add("XmlContenedor", XmlContenedor);
            return sql_puntero.ExecuteSelectControl<Damage_Existe_Contenedor>(nueva_conexion, 6000, "[Bill].[Validacion_Damage_Control]", parametros, out OnError);

        }

    }
}
