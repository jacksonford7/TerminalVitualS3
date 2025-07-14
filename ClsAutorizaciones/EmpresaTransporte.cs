using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ControlOPC.Entidades;


namespace ClsAutorizaciones
{
    public class EmpresaTransporte : Base
    {
        #region "Variables"

        private Int64 _id;
        private string _ruc = string.Empty;
        private string _razon_social = string.Empty;
        private string _contacto = string.Empty;
        private string _direccion = string.Empty;
        private string _ciudad = string.Empty;
        private string _telefono = string.Empty;
        private string _mail = string.Empty;
        #endregion

        #region "Propiedades"
        public Int64 id { get => _id; set => _id = value; }
        public string ruc { get => _ruc; set => _ruc = value; }
        public string razon_social { get => _razon_social; set => _razon_social = value; }
        public string contacto { get => _contacto; set => _contacto = value; }
        public string direccion { get => _direccion; set => _direccion = value; }
        public string ciudad { get => _ciudad; set => _ciudad = value; }
        public string telefono { get => _telefono; set => _telefono = value; }
        public string mail { get => _mail; set => _mail = value; }
        #endregion

        private static String v_mensaje = string.Empty;

        public EmpresaTransporte()
        {
            init();
        }


        #region "Metodos"
        private static void OnInit()
        {
            sql_pointer = (sql_pointer == null) ? SQLHandler.sql_handler.GetInstance() : sql_pointer;
            parametros = new Dictionary<string, object>();

            v_conexion = Extension.Nueva_Conexion("N5_DB");
        }

        public static List<EmpresaTransporte> Lista_Empresa(string pista, out string OnError)
        {
            OnInit();

            parametros.Clear();
            parametros.Add("pista", pista);
            return sql_pointer.ExecuteSelectControl<EmpresaTransporte>(v_conexion, 6000, "RVA_COMPANIA_LISTADO", parametros, out OnError);

        }

        public static List<EmpresaTransporte> Valida_Empresa(string ID_EMPRESA, out string OnError)
        {
            OnInit();

            parametros.Clear();
            parametros.Add("ID_EMPRESA", ID_EMPRESA);
            return sql_pointer.ExecuteSelectControl<EmpresaTransporte>(v_conexion, 6000, "RVA_VALIDA_COMPANIA_TRANSPORTE", parametros, out OnError);

        }

        #endregion
    }
}
