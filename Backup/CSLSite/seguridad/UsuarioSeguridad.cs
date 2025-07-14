using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CSLSite
{
    public class UsuarioSeguridad
    {
        public int idUsuario { get; set; }
        public string usuario { get; set; }
        public string nombres { get; set; }
        public string codigoEmpresa { get; set; }
        public string nombreEmpresa { get; set; }
        public string usuarioIdentificacion { get; set; }
        public string tipoUsuario { get; set; }
        public string desTipoUsuario { get; set; }
        public string estado { get; set; }
        public string usuarioCorreo { get; set; }
        public string correoEmpresa { get; set; }
        public string pais { get; set; }
        public int ciudad { get; set; }
        public string direccionEmpresa { get; set; }
        public string telefonoEmpresa { get; set; }
        public string faxEmpresa { get; set;}
        public string websiteEmpresa { get; set; }
        public string nombreUsuario { get; set; }
        public string apellidoUsuario { get; set; }
        public string ipCreacion { get; set; }
        public string password { get; set; }
        public string registradoPor { get; set; }
        public int idProvincia { get; set; }
    }
}