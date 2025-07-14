using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using SqlConexion;
using Newtonsoft.Json;


namespace BillionAutenticacion
{
    //[Serializable]
    public class Cls_Usuario : Cls_Base
    {

        #region "Variables"

        private int? _IdUsuario = 0;
        private int? _Idcorporacion = 0;
        private string _Usuario = string.Empty;
        private string _CambiarContrasena = string.Empty;
        private Int16? _ContadorIntentos = 0;
        private DateTime? _FechaPrimerIntento;
        private string _Nombres = string.Empty;
        private string _Apellidos = string.Empty;
        private string _CodigoEmpresa = string.Empty;
        private string _Email = string.Empty;
        private string _Ruc = string.Empty;
        private string _TipoActividadExpoImpo = string.Empty;
        private string _TipoUsuarioEmpresa = string.Empty;
        private int? _Grupo = 0;
        private int? _Empresa = 0;
        private string _Gruponombre = string.Empty;
        private bool _Cliente_Credito = false;
        private static String v_mensaje = string.Empty;
        private string _Nombreempresa = string.Empty;
        #endregion

        #region "Propiedades"

        public int? IdUsuario { get => _IdUsuario; set => _IdUsuario = value; }
        public int? Idcorporacion { get => _Idcorporacion; set => _Idcorporacion = value; }
        public string Usuario { get => _Usuario; set => _Usuario = value; }
        public string CambiarContrasena { get => _CambiarContrasena; set => _CambiarContrasena = value; }
        public Int16? ContadorIntentos { get => _ContadorIntentos; set => _ContadorIntentos = value; }
        public DateTime? FechaPrimerIntento { get => _FechaPrimerIntento; set => _FechaPrimerIntento = value; }
        public string Nombres { get => _Nombres; set => _Nombres = value; }
        public string Apellidos { get => _Apellidos; set => _Apellidos = value; }
        public string CodigoEmpresa { get => _CodigoEmpresa; set => _CodigoEmpresa = value; }
        public string Email { get => _Email; set => _Email = value; }
        public string Ruc { get => _Ruc; set => _Ruc = value; }
        public string TipoActividadExpoImpo { get => _TipoActividadExpoImpo; set => _TipoActividadExpoImpo = value; }
        public string TipoUsuarioEmpresa { get => _TipoUsuarioEmpresa; set => _TipoUsuarioEmpresa = value; }
        public int? Grupo { get => _Grupo; set => _Grupo = value; }
        public int? Empresa { get => _Empresa; set => _Empresa = value; }
        public string Gruponombre { get => _Gruponombre; set => _Gruponombre = value; }
        public bool Cliente_Credito { get => _Cliente_Credito; set => _Cliente_Credito = value; }
        public string Nombreempresa { get => _Nombreempresa; set => _Nombreempresa = value; }
        #endregion

        public Cls_Usuario()
        {
            init();
        }

       

        private static void OnInit()
        {
            sql_puntero = (sql_puntero == null) ? Cls_Conexion.Conexion() : sql_puntero;
            parametros = new Dictionary<string, object>();
            nueva_conexion = Cls_Conexion.Nueva_Conexion("PORTAL_MASTER");
        }

        private static void OnBillion()
        {
            sql_puntero = (sql_puntero == null) ? Cls_Conexion.Conexion() : sql_puntero;
            parametros = new Dictionary<string, object>();
            nueva_conexion = Cls_Conexion.Nueva_Conexion("PORTAL_BILLION");
        }

        private static void OnOtros(string Config)
        {
            sql_puntero = (sql_puntero == null) ? Cls_Conexion.Conexion() : sql_puntero;
            parametros = new Dictionary<string, object>();
            nueva_conexion = Cls_Conexion.Nueva_Conexion(Config);
        }


        public static int Autentificacion(string pUsuario, string pClave, out string OnError)
        {
            int nValor = 0;
            OnInit();

            Type componente = Type.GetTypeFromProgID("AISVUtils.GSS");
            if (componente == null)
            {

                OnError = "El componente AISVUtils.GSS ha fallado";
                nValor = 5;
            }

            dynamic instancia = Activator.CreateInstance(componente);
            var dcpass = instancia.Encrypt(pUsuario.Trim(), pClave.Trim()) as string;

            if (string.IsNullOrEmpty(dcpass))
            {
                OnError = "Hubo un problema durante la desencripción, el componente AISVUtils.GSS ha fallado";
                nValor = 5;
            }

            parametros.Clear();
            parametros.Add("usuario", pUsuario);
            parametros.Add("clave", dcpass);

            var db = sql_puntero.ExecuteSelectOnly(nueva_conexion, 4000, "sp_Bil_Autentifica_Usuario", parametros);
            if (db == null)
            {
                OnError = string.Format("Error al validar usuario {0} y clave {1} ", pUsuario.Trim(), pClave.Trim());
                nValor = 5;
            }
            else
            {
                nValor = db.codigo;
                OnError = db.mensaje;
            }

            instancia = null;
            componente = null;


            return nValor;
        }

        public static Cls_Usuario CargarDatos(string pUsuario, string pClave, out string OnError)
        {
            OnInit();

            Cls_Usuario ClsUsuario = new Cls_Usuario();

            Type componente = Type.GetTypeFromProgID("AISVUtils.GSS");
            if (componente == null)
            {
                OnError = "El componente AISVUtils.GSS ha fallado";
                return ClsUsuario;
            }

            dynamic instancia = Activator.CreateInstance(componente);
            var dcpass = instancia.Encrypt(pUsuario.Trim(), pClave.Trim()) as string;

            if (string.IsNullOrEmpty(dcpass))
            {
                OnError = "Hubo un problema durante la desencripción, el componente AISVUtils.GSS ha fallado";
                return ClsUsuario;
            }

            parametros.Clear();
            parametros.Add("usuario", pUsuario);
            parametros.Add("clave", dcpass);

            ClsUsuario = sql_puntero.ExecuteSelectOnly<Cls_Usuario>(nueva_conexion, 4000, "sp_Bil_Consulta_Usuario", parametros);
            if (ClsUsuario == null)
            {
                OnError = "No existen registros con los criterios de búsqueda";
                return ClsUsuario;
            }


            //nuevo cargar el tipo de cliente.  
            var sruc = string.IsNullOrEmpty(ClsUsuario.Ruc) ? ClsUsuario.CodigoEmpresa : ClsUsuario.Ruc;
            //no es nulo o vacío ahora el ruc es el codigo limpio
            if (!string.IsNullOrEmpty(sruc)) { sruc = sruc.Trim(); ClsUsuario.Ruc = sruc; }


            OnError = string.Empty;
            return ClsUsuario;
        }


        public static Int64? Envia_Mail_Usuario(Cls_Usuario ClsUsuario, out string OnError)
        {
            OnBillion();

            if (string.IsNullOrEmpty(ClsUsuario.Usuario))
            {
                OnError = "El usuario no puede estar vacío";
                return null;
            }

            parametros.Clear();
            parametros.Add("Usuario", ClsUsuario.Usuario.Trim());
            parametros.Add("Nombres", ClsUsuario.Nombres.Trim() + " " + ClsUsuario.Apellidos.Trim());
            parametros.Add("Email_Usuario", ClsUsuario.Email.Trim());

            var db = sql_puntero.ExecuteInsertUpdateDeleteReturn(nueva_conexion, 4000, "sp_Bil_Mail_Usuario_Bloqueado", parametros, out OnError);
            if (!db.HasValue || db.Value < 0)
            {
                return null;
            }

            OnError = string.Empty;
            return db.Value;
        }

        public static Int64? Envia_Mail_Sistemas(string Mensaje, out string OnError)
        {
            OnBillion();

         
            parametros.Clear();
            parametros.Add("Mensaje", Mensaje.Trim());
         
            var db = sql_puntero.ExecuteInsertUpdateDeleteReturn(nueva_conexion, 4000, "sp_Bil_Mail_Sistemas", parametros, out OnError);
            if (!db.HasValue || db.Value < 0)
            {
                return null;
            }

            OnError = string.Empty;
            return db.Value;
        }


        //va y obtiene las opciones de zona de este usuario
        public static HashSet<Cls_Permisos> autorized_access(Cls_Usuario ClsUsuario, out string OnError)
        {
            OnOtros("SERVICE");
            parametros.Clear();
            parametros.Add("IdCorporacion", ClsUsuario.Idcorporacion);
            parametros.Add("IdEmpresa", ClsUsuario.Empresa);
            parametros.Add("Idgrupo", ClsUsuario.Grupo);
            var lista =sql_puntero.ExecuteSelectControl<Cls_Permisos>(nueva_conexion, 4000, "sp_user_access", parametros, out OnError);
            //obtengo todos los permisos   
            var rs = new HashSet<Cls_Permisos>();
            foreach (var item in lista)
            {
                var z = new Cls_Permisos();
                z.idzona = item.idzona == null ? null : item.idzona;
                z.idservicio = item.idservicio == null ? null : item.idservicio;
                z.opciones = item.opciones == null ? null : item.opciones;
                rs.Add(z);
            }
            return rs;
        }

        public static List<Tuple<int, int, string, string>> block_options(Cls_Usuario ClsUsuario, out string OnError)
        {
            var rs = new List<Tuple<int, int, string, string>>();
            OnOtros("SERVICE");
            parametros.Clear();
            parametros.Add("cliente", ClsUsuario.Ruc);
            parametros.Add("rol", ClsUsuario.Gruponombre);

            var lista = sql_puntero.ExecuteSelectControl<Cls_Bloqueos>(nueva_conexion, 4000, "pc_bloqueos_s3", parametros, out OnError);
            foreach (var item in lista)
            {
                var z = Tuple.Create<int, int, string, string>(item.id_servicio, item.id_opcion, item.tipo, item.subtipo);
                rs.Add(z);
            }

            return rs;
        }

        //va y obtiene las zonas autorizadas para el usuario
        public static HashSet<Cls_Zona> autorized_zones(Cls_Usuario ClsUsuario, out string OnError)
        {
            OnOtros("SERVICE");
            parametros.Clear();
            parametros.Add("IdCorporacion", ClsUsuario.Idcorporacion);
            parametros.Add("IdEmpresa", ClsUsuario.Empresa);
            parametros.Add("IdUsuario", ClsUsuario.IdUsuario);
            var lista = sql_puntero.ExecuteSelectControl<Cls_Zona>(nueva_conexion, 4000, "sp_user_zones_billion", parametros, out OnError);
            
            var rs = new HashSet<Cls_Zona>();
            foreach (var item in lista)
            {
                var z = new Cls_Zona();
                z.corporacion = item.corporacion;
                z.empresa = item.empresa;
                z.zona = item.zona;
                z.servicio = item.servicio;
                z.zonatitulo = item.zonatitulo;
                z.icono = item.icono;
                rs.Add(z);
            }
               
            return rs;
        }


        //va y obtiene las opciones de zona de este usuario
        public static List<Cls_Opcion> Opciones_Menu(int idservicio, out string OnError)
        {
            OnOtros("SERVICE");
            parametros.Clear();
            parametros.Add("idservicio", idservicio);
            var lista = sql_puntero.ExecuteSelectControl<Cls_Opcion>(nueva_conexion, 4000, "sp_user_options_billion", parametros, out OnError);
 
            return lista;
        }

    }
}
