using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace CSLSite
{
    [Serializable]
    public class usuario
    {
        private int isautorice;

        public int id { get; set; }
        public int? idcorporacion { get; set; }
        public int? idempresa { get; set; }
        public string loginname { get; set; }
        public string nombres { get; set; }
        public string apellidos { get; set; }
        public string cambiarpass { get; set; }
        public int? intentos { get; set; }
        public DateTime? primerIntento { get; set; }
        public string codigoempresa { get; set; }
        public string acceso_aisv { get; set; }
        public string email { get; set; }
        public string ruc { get; set; }
        public string actividad { get; set; }
        public string tipousuario { get; set; }
        public int? grupo { get; set; }
        public string nombregrupo { get; set; }
        public bool IsCredito { get; set; }

        public bool bloqueo_cartera { get; set; }

        public bool IsPaidLock { get; set; }
        public override string ToString()
        {
            try
            {
                return QuerySegura.EncryptQueryString(JsonConvert.SerializeObject(this));
            }
            catch (Exception ex)
            {
                csl_log.log_csl.save_log<Exception>(ex, "usuario", "ToString", this.loginname, this.loginname);
                return null;
            }
        }
        public int IsAutorize
        {
            get { return this.isautorice; }
        }
        public usuario() { this.idcorporacion = 1; this.idempresa = 1; ;this.IsCredito = false; }
        //va y comprueba el nombre de usuario y contraseña.
        public int autenticate( string pass)
        {
            if(string.IsNullOrEmpty(this.loginname) || string.IsNullOrEmpty(pass))
            {
                return 0;
            }
            Type componente = Type.GetTypeFromProgID("AISVUtils.GSS");
            if (componente == null)
            {
                csl_log.log_csl.save_log<ApplicationException>(new ApplicationException("El componente AISVUtils.GSS ha fallado"), "usuario", "autenticate", loginname, loginname);
                //  throw new Exception("Control1");
                return 5;
            }
            try
            {
                dynamic instancia = Activator.CreateInstance(componente);
                var dcpass = instancia.Encrypt(this.loginname.Trim(), pass.Trim()) as string;
                if (string.IsNullOrEmpty(dcpass))
                {
                    csl_log.log_csl.save_log<ApplicationException>(new ApplicationException("Hubo un problema durante la desencripción, el componente AISVUtils.GSS ha fallado"), "usuario", "autenticate", pass, loginname);
                    // throw new Exception("Control2");
                    return 5;
                }
                instancia = null;
                componente = null;
                var rsp = CLSData.ValorEscalar("sp_user_autenticate_v1", new Dictionary<string, string>() { { "login", this.loginname }, { "pass", dcpass } }, tComando.Procedure);
                //ahora si la parte de datos.
                if(string.IsNullOrEmpty(rsp))
                {
                    //es nulo o vacío ha fallado algun elemento.
                    //  throw new Exception("Control3");
                    csl_log.log_csl.save_log<ApplicationException>(new ApplicationException("rsp era nulo"), "usuario", "autenticate", pass, loginname);
                    return 5;
                }
                int t = 0;
                if (!int.TryParse(rsp, out t))  
                {
                    // throw new Exception("Control4");
                    csl_log.log_csl.save_log<ApplicationException>(new ApplicationException("t es nulo"), "usuario", "autenticate", pass, loginname);
                    return 5;
                }
                var dt = string.Empty;

                this.isautorice = t;
                populate(dcpass, out dt);
                return t;
            }
            catch (Exception ex)
            {
                // throw new Exception(ex.Message);
                csl_log.log_csl.save_log<Exception>(ex, "usuario", "autenticate", pass, loginname);
                return 5;
            }
        }
        //esta opción es interna pobla los datos del usuario
        private void populate(string pass,out string message)
        {
            if (isautorice == 4 || isautorice == 6 || isautorice == 7)
            {
                if (isautorice == 7)
                {
                    foreach (var item in CLSData.ValorLecturas("sp_user_populate_admin_tmp", tComando.Procedure, new Dictionary<string, string>() { { "login", this.loginname }, { "pass", pass } }))
                    {
                        this.idcorporacion = item[0] as int?;
                        this.id = !item.IsDBNull(1) ? item.GetInt32(1) : 0;
                        this.cambiarpass = item[2] as string;
                        this.intentos = item[3] as int?;
                        this.primerIntento = item[4] as DateTime?;
                        this.nombres = item[5] as string;
                        this.apellidos = item[6] as string;
                        this.codigoempresa = item[7] as string;
                        this.acceso_aisv = item[8] as string;
                        this.email = item[9] as string;
                        this.ruc = item[10] as string;
                        this.actividad = item[11] as string;
                        this.tipousuario = item[12] as string;
                        this.grupo = item[13] as int?;
                        this.idempresa = item[14] as int?;
                        this.nombregrupo = item[15] as string;
                    }
                }
                else
                {
                    foreach (var item in CLSData.ValorLecturas("sp_user_populate_v1_tmp", tComando.Procedure, new Dictionary<string, string>() { { "login", this.loginname }, { "pass", pass } }))
                    {
                        this.idcorporacion = item[0] as int?;
                        this.id = !item.IsDBNull(1) ? item.GetInt32(1) : 0;
                        this.cambiarpass = item[2] as string;
                        this.intentos = item[3] as int?;
                        this.primerIntento = item[4] as DateTime?;
                        this.nombres = item[5] as string;
                        this.apellidos = item[6] as string;
                        this.codigoempresa = item[7] as string;
                        this.acceso_aisv = item[8] as string;
                        this.email = item[9] as string;
                        this.ruc = item[10] as string;
                        this.actividad = item[11] as string;
                        this.tipousuario = item[12] as string;
                        this.grupo = item[13] as int?;
                        this.idempresa = item[14] as int?;
                        this.nombregrupo = item[15] as string;
                    }
                }
                //nuevo cargar el tipo de cliente.  
                var sruc = string.IsNullOrEmpty(this.ruc) ? this.codigoempresa : this.ruc;
                //no es nulo o vacío ahora el ruc es el codigo limpio
                if (!string.IsNullOrEmpty(sruc)) { sruc = sruc.Trim(); this.ruc = sruc; }

                //determina si es cliente de credito en Billing
                this.IsCredito = Credito(sruc);
                //determina si esta en la lista negra de CGSA
                this.IsPaidLock = FaltaPago(sruc);
            }
            message = string.Empty;
        }
        //va y obtiene las zonas autorizadas para el usuario
        public HashSet<zona> autorized_zones()
        {
            var rs = new HashSet<zona>();
            foreach (var item in CLSData.ValorLecturas("[TV].[sp_user_zones_tv_tmp]", tComando.Procedure, new Dictionary<string, string>() { { "IdCorporacion", this.idcorporacion.Value.ToString() }, { "IdEmpresa", this.idempresa.Value.ToString() }, { "IdUsuario", this.id.ToString() } }))
            {
                var z = new zona();
                z.idcorp = item[0] as int?;
                z.idempre = item[1] as int?;
                z.idzona = item[2] as int?;
                z.idservicio = item[3] as int?;
                z.titulo = item[4] as string;
                z.icono = item[5] as string;
                rs.Add(z);
            }
            return rs;
        }
        //va y obtiene las opciones de zona de este usuario
        public HashSet<permisos> autorized_access()
        {
            //obtengo todos los permisos   
            var rs = new HashSet<permisos>();
            string grupo = (this.grupo.HasValue ? this.grupo.Value.ToString() : "0");

            var lista = CLSData.ValorLecturasOffLine("sp_user_access", tComando.Procedure, new Dictionary<string, string>() { { "IdCorporacion", this.idcorporacion.Value.ToString() }, { "IdEmpresa", this.idempresa.Value.ToString() }, { "IdGrupo", grupo } });
            foreach (var item in lista)
            {
                var z = new permisos();
                z.idzona = item[0] as int?;
                z.idservicio = item[1] as int?;
                z.opciones = !item.IsNull(2)?item[2].ToString().ToCharArray():null;
                rs.Add(z);
            }
            return rs;
        }
        //deserializa el objeto que esta en string a un usuario normal.
        public static usuario Deserialize(string json)
        {
            try
            {
                return JsonConvert.DeserializeObject<usuario>(QuerySegura.DecryptQueryString(json));
            }
            catch (Exception ex)
            {
                csl_log.log_csl.save_log<Exception>(ex, "usuario", "Deserialize",json, "N4");
                return null;
            }
        }

        public static bool Credito(string cliente)
        {
            //return false;
            if (string.IsNullOrEmpty(cliente))
            {
                csl_log.log_csl.save_log<Exception>(new ApplicationException("Ruc Cliente es cadena vacía"), "usuario", "IsCredito", cliente, "N4");
                return false;
            }
            using (var con = new System.Data.SqlClient.SqlConnection())
            {
                //n4catalog
                var existe = System.Configuration.ConfigurationManager.ConnectionStrings["Billing"] != null;
                if (!existe)
                {
                    csl_log.log_csl.save_log<Exception>(new ApplicationException("La cadena Billing no existe"), "usuario", "IsCredito", cliente, "N4");
                    return false;
                }
                con.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["Billing"].ConnectionString;
                var comando = con.CreateCommand();
                comando.CommandType = System.Data.CommandType.Text;
                comando.Connection = con;
                // comando.CommandText = "select top 1 1 from [Billing].[dbo].[bil_customer](nolock) where  role='SHIPPER' and id= @id";
                comando.CommandText = "select top 1 1 from [Billing].[dbo].[bil_customer](nolock) where life_cycle_state='ACT' and due_date_value > 0 and role='SHIPPER' and id= @id";
                comando.Parameters.AddWithValue("@id", cliente.Trim());
                try
                {
                    con.Open();
                    object sale = comando.ExecuteScalar();
                    con.Close();
                    if (sale != null && sale.GetType() != typeof(DBNull))
                    {
                        if (sale.ToString().Contains("1"))
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    csl_log.log_csl.save_log<Exception>(ex, "usuario", "IsCredito", cliente, "N4");
                    return false;
                }
                finally
                {
                    if (con.State == System.Data.ConnectionState.Open)
                    {
                        con.Close();
                    }
                    con.Dispose();
                    comando.Dispose();
                }
            }
        }
        public static bool FaltaPago(string cliente)
        {

            if (string.IsNullOrEmpty(cliente))
            {
                return false;
            }
            using (var con = new System.Data.SqlClient.SqlConnection())
            {
                //n4catalog
                var existe = System.Configuration.ConfigurationManager.ConnectionStrings["midle"] != null;
                if (!existe)
                {
                    csl_log.log_csl.save_log<Exception>(new ApplicationException("La cadena de conexion Middleware no existe"), "usuario", "FaltaPago", cliente, "N4");
                    return false;
                }
                con.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["midle"].ConnectionString;
                var comando = con.CreateCommand();
                comando.CommandType = System.Data.CommandType.Text;
                comando.Connection = con;
                comando.CommandText = "select [dbo].[FNA_FUN_LOCK_CLIENTS_AISV](@lv_customer)";
                comando.Parameters.AddWithValue("@lv_customer", cliente.Trim());
                try
                {
                    con.Open();
                    object sale = comando.ExecuteScalar();
                    con.Close();
                    if (sale != null && sale.GetType() != typeof(DBNull))
                    {
                        return bool.Parse(sale.ToString());
                    }
                    else
                    {
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    csl_log.log_csl.save_log<Exception>(ex, "usuario", "FaltaPago", cliente, "N4");
                    return false;
                }
                finally
                {
                    if (con.State == System.Data.ConnectionState.Open)
                    {
                        con.Close();
                    }
                    con.Dispose();
                    comando.Dispose();
                }
            }

        }

        /*   AGREGADO POR LMO */
        //va y obtiene las opciones de zona de este usuario
        public HashSet<permisos> autorized_access_admin()
        {
            //obtengo todos los permisos   
            var rs = new HashSet<permisos>();
            var lista = CLSData.ValorLecturasOffLine("sp_user_access", tComando.Procedure, new Dictionary<string, string>() { { "IdCorporacion", this.idcorporacion.Value.ToString() }, { "IdEmpresa", this.idempresa.Value.ToString() }, { "IdGrupo", this.grupo.Value.ToString() } });
            foreach (var item in lista)
            {
                var z = new permisos();
                z.idzona = item[0] as int?;
                z.idservicio = item[1] as int?;
                z.opciones = !item.IsNull(2) ? item[2].ToString().ToCharArray() : null;
                rs.Add(z);
            }
            return rs;
        }
        public HashSet<zona> autorized_zones_admin()
        {
            var rs = new HashSet<zona>();
            foreach (var item in CLSData.ValorLecturas("sp_user_zones_admin", tComando.Procedure, new Dictionary<string, string>() { { "IdCorporacion", this.idcorporacion.Value.ToString() }, { "IdEmpresa", this.idempresa.Value.ToString() }, { "IdUsuario", this.id.ToString() } }))
            {
                var z = new zona();
                z.idcorp = item[0] as int?;
                z.idempre = item[1] as int?;
                z.idzona = item[2] as int?;
                z.idservicio = item[3] as int?;
                z.titulo = item[4] as string;
                z.icono = item[5] as string;
                rs.Add(z);
            }
            return rs;
        }
        public List<Tuple<int, int, string, string>> block_options()
        {
            var rs = new List<Tuple<int, int,string,string>>();
            foreach (var item in CLSData.ValorLecturas("pc_bloqueos_s3", tComando.Procedure, new Dictionary<string, string>() { { "cliente", this.ruc }, { "rol", this.nombregrupo} }))
            {
                var z = Tuple.Create<int, int, string, string>(item.GetInt32(0), item.GetInt32(1), item.GetString(2), item.GetString(3));
                rs.Add(z);
            }
            return rs;
        }

        public usuario ObtenerUser(string _login, string _ruc)
        {
            //obtengo todos los permisos   
            //var rs = new HashSet<permisos>();
            var lista = CLSData.ValorLecturasOffLine("sp_user_datos", tComando.Procedure, new Dictionary<string, string>() { { "login", _login }, { "ruc", _ruc } });
            var z = new usuario();
            foreach (var item in lista)
            {
                
                z.idcorporacion = item[0] as int?;
                z.id = !item.IsNull(1) ? int.Parse(item[1].ToString())  : 0;
                z.cambiarpass = item[2] as string;
                z.intentos = item[3] as int?;
                z.primerIntento = item[4] as DateTime?;
                z.nombres = item[5] as string;
                z.apellidos = item[6] as string;
                z.codigoempresa = item[7] as string;
                z.acceso_aisv = item[8] as string;
                z.email = item[9] as string;
                z.ruc = item[10] as string;
                z.actividad = item[11] as string;
                z.tipousuario = item[12] as string;
                z.grupo = item[13] as int?;
                z.idempresa = item[14] as int?;
                z.nombregrupo = item[15] as string;
            }
            return z;
        }

       

    }

}