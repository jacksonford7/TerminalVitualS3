using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlConexion;

namespace BillionEntidades
{
   public  class Cls_Bil_DetalleClientes : Cls_Bil_Base
    {

        #region "Variables"

  
        private string _ruc = string.Empty;
        private string _nombres = string.Empty;
        private string _email = string.Empty;
        private bool _servicio = false;
        private bool _visto = false;
        private bool _inactivo = false;
        #endregion

        #region "Propiedades"

        public string ruc { get => _ruc; set => _ruc = value; }
        public string nombres { get => _nombres; set => _nombres = value; }
        public string email { get => _email; set => _email = value; }
        public bool servicio { get => _servicio; set => _servicio = value; }
        public bool visto { get => _visto; set => _visto = value; }
        public bool inactivo { get => _inactivo; set => _inactivo = value; }

        #endregion

        public Cls_Bil_DetalleClientes()
        {
            init();
        }

        public Cls_Bil_DetalleClientes( string _ruc, string _nombres, string _email, bool _servicio, bool _visto, bool _inactivo)

        {
            this.ruc = _ruc;
            this.nombres = _nombres;
            this.email = _email;
            this.servicio = _servicio;
            this.visto = _visto;
            this.inactivo = _inactivo;

        }

        /*carga contenedores con fecha y ultima factura billion*/
        public static List<Cls_Bil_DetalleClientes> List_Clientes(string xmlCliente, out string OnError)
        {
            parametros.Clear();
            parametros.Add("xmlCliente", xmlCliente);
            return sql_puntero.ExecuteSelectControl<Cls_Bil_DetalleClientes>(sql_puntero.Conexion_Local, 6000, "Bil_valida_freight_forwarder", parametros, out OnError);

        }

    }
}
