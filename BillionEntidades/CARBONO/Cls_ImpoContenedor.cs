using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlConexion;

namespace BillionEntidades
{
   public class Cls_ImpoContenedor : Cls_Bil_Base
    {
        private static String v_mensaje = string.Empty;

        #region "Variables"
        private static Int64? lm = -3;
        private Int64 _ID;
        private string _ClientId = string.Empty;
        private string _Client = string.Empty;
        private string _Create_user = string.Empty;
        private string _file_pdf = string.Empty;
        private string _Email = string.Empty;
        private bool _activo = false;

        private string _creadopor = string.Empty;
        private string _modificadopor = string.Empty;
        private DateTime? _creado;
        private DateTime? _modificado;
        private string _Comment = string.Empty;

        private string _ruc= string.Empty;
        private string _nombres = string.Empty;
        private Int64 _gkey;
        private string _numero_carga = string.Empty;
        private string _cntr = string.Empty;
        private string _usuarioing = string.Empty;
        #endregion

        #region "Propiedades"

        public Int64 ID { get => _ID; set => _ID = value; }
        public string ClientId { get => _ClientId; set => _ClientId = value; }
        public string Client { get => _Client; set => _Client = value; }
        public string Create_user { get => _Create_user; set => _Create_user = value; }
        public string file_pdf { get => _file_pdf; set => _file_pdf = value; }
        public string Email { get => _Email; set => _Email = value; }
        public bool activo { get => _activo; set => _activo = value; }

        public string creadopor { get => _creadopor; set => _creadopor = value; }
        public string modificadopor { get => _modificadopor; set => _modificadopor = value; }

        public DateTime? creado { get => _creado; set => _creado = value; }
        public DateTime? modificado { get => _modificado; set => _modificado = value; }
        public string Comment { get => _Comment; set => _Comment = value; }

        public string ruc { get => _ruc; set => _ruc = value; }
        public string nombres { get => _nombres; set => _nombres = value; }
        public string numero_carga { get => _numero_carga; set => _numero_carga = value; }
        public string cntr { get => _cntr; set => _cntr = value; }
        public string usuarioing { get => _usuarioing; set => _usuarioing = value; }
        public Int64 gkey { get => _gkey; set => _gkey = value; }
        #endregion

        public Cls_ImpoContenedor()
        {
            init();

        }
        private static void OnInit(string Base)
        {
            sql_puntero = (sql_puntero == null) ? Cls_Conexion.Conexion() : sql_puntero;
            parametros = new Dictionary<string, object>();
            nueva_conexion = Cls_Conexion.Nueva_Conexion(Base);
        }

        #region "Validacion si es cliente"
        public static bool ExisteUsuarioCarbono(string ruc,  out string OnError)
        {

            parametros.Clear();
            parametros.Add("ruc", ruc);
         
            var Tabla = sql_puntero.ComandoSelectEscalar(sql_puntero.Conexion_Local, 6000, "SELECT dbo.fx_usuario_carbono (@ruc);", parametros, out OnError);
            if (Tabla == null)
            {
                if (!string.IsNullOrEmpty(OnError))
                {
                    return false;
                }
                else
                {
                    OnError = string.Empty;
                    return false;
                }

            }

            var r = Tabla;
            OnError = string.Empty;
            return bool.Parse(r.ToString());
        }
        #endregion

        #region "Validacion si el cliente esta inactivo"
        public static bool ExisteUsuarioCarbonoInactivo(string ruc, out string OnError)
        {

            parametros.Clear();
            parametros.Add("ruc", ruc);

            var Tabla = sql_puntero.ComandoSelectEscalar(sql_puntero.Conexion_Local, 6000, "SELECT dbo.fx_usuario_carbono_inactivo (@ruc);", parametros, out OnError);
            if (Tabla == null)
            {
                if (!string.IsNullOrEmpty(OnError))
                {
                    return false;
                }
                else
                {
                    OnError = string.Empty;
                    return false;
                }

            }

            var r = Tabla;
            OnError = string.Empty;
            return bool.Parse(r.ToString());
        }
        #endregion


        #region "Registra Cliente"
        private Int64? Save(out string OnError)
        {
            
            parametros.Clear();

            parametros.Add("ClientId", this.ClientId);
            parametros.Add("Client", this.Client);
            parametros.Add("Create_user", this.Create_user);
            parametros.Add("file_pdf", this.file_pdf);
            parametros.Add("Email", this.Email);
            parametros.Add("activo", this.activo);
            var db = sql_puntero.ExecuteInsertUpdateDeleteReturn(sql_puntero.Conexion_Local, 6000, "st_usuario_carbono_reg_upt", parametros, out OnError);
            if (!db.HasValue || db.Value < 0)
            {
                return null;
            }
            OnError = string.Empty;
            return db.Value;

        }

        public Int64? SaveTransaction(out string OnError)
        {

            Int64 ID = 0;
            try
            {

                //grabar transaccion.
                var id = this.Save(out OnError);
                if (!id.HasValue)
                {
                    OnError = "*** Error: al registrar cliente de Carbono Neutro ****";
                    return 0;
                }
                ID = id.Value;

                return ID;

            }
            catch (Exception ex)
            {

                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>("SQL", nameof(SaveTransaction), "SaveTransaction", false, null, null, ex.StackTrace, ex);
                OnError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));

                return null;
            }
        }
        #endregion

        #region "Registra Agente y Verifica Existencia"
        public Int64? SaveAgente(out string OnError)
        {
            OnInit("APPCGSA");

            parametros.Clear();

            parametros.Add("ClientId", this.ClientId);
            parametros.Add("Client", this.Client);
            parametros.Add("Create_user", this.Create_user);
            parametros.Add("file_pdf", this.file_pdf);
            parametros.Add("comentario", this.Comment);
            parametros.Add("activo", this.activo);
            var db = sql_puntero.ExecuteInsertUpdateDeleteReturn(nueva_conexion, 6000, "insertarPackageClientsAgents", parametros, out OnError);
            if (!db.HasValue || db.Value < 0)
            {
                return null;
            }
            OnError = string.Empty;
            return db.Value;

        }

        //metodo para verificar si es agente
        public static bool? VerificaSiExisteAgente(string ruc, out string OnError)
        {
            OnInit("APPCGSA");
            parametros.Clear();
            parametros.Add("ruc", ruc);
            return sql_puntero.ExecuteSelectOnlyBool(nueva_conexion, 6000, "existePackageClientsAgents", parametros, out OnError);

        }
        #endregion

        #region "Listados"
        public static List<Cls_ImpoContenedor> Listado_Paquetes_Clientes_filtro(DateTime? fecha_desde, DateTime? fecha_hasta, string criterio, out string OnError)
        {
    
            parametros.Clear();
            parametros.Add("fecha_desde", fecha_desde);
            parametros.Add("fecha_hasta", fecha_hasta);
            parametros.Add("criterio", criterio);
            return sql_puntero.ExecuteSelectControl<Cls_ImpoContenedor>(sql_puntero.Conexion_Local, 6000, "bill_Listado_cliente_carbo_neutro", parametros, out OnError);

        }
        #endregion


        #region "Cancelar Servicio"
        public bool Delete(out string OnError)
        {
           
            parametros.Clear();
            parametros.Add("Id", this.ID);
            parametros.Add("Create_user", this.Create_user);
            parametros.Add("Comment", this.Comment);
            parametros.Add("file_pdf", this.file_pdf);
            using (var scope = new System.Transactions.TransactionScope())
            {
                var db = sql_puntero.ExecuteInsertUpdateDelete(sql_puntero.Conexion_Local, 6000, "bil_inactiva_carbono_neutro", parametros, out OnError);
                if (!db.HasValue || db.Value < 0)
                {
                    return false;
                }
                scope.Complete();
            }
            return true;
        }
        #endregion

        #region "Listado de clientes Inactivos"
        public static List<Cls_ImpoContenedor> Listado_Paquetes_Clientes_Inactivos(DateTime? fecha_desde, DateTime? fecha_hasta, string criterio, out string OnError)
        {
         
            parametros.Clear();
            parametros.Add("fecha_desde", fecha_desde);
            parametros.Add("fecha_hasta", fecha_hasta);
            parametros.Add("criterio", criterio);
            return sql_puntero.ExecuteSelectControl<Cls_ImpoContenedor>(sql_puntero.Conexion_Local, 5000, "bill_Listado_cliente_inactivo_carbo_neutro", parametros, out OnError);

        }

        #endregion


        #region "Servicio de imagenes de sellos"
        #region "Validacion si es cliente"
        public static bool ExisteUsuarioSellos(string ruc, out string OnError)
        {

            parametros.Clear();
            parametros.Add("ruc", ruc);

            var Tabla = sql_puntero.ComandoSelectEscalar(sql_puntero.Conexion_Local, 6000, "SELECT dbo.sellos_fx_usuario_imagenes (@ruc);", parametros, out OnError);
            if (Tabla == null)
            {
                if (!string.IsNullOrEmpty(OnError))
                {
                    return false;
                }
                else
                {
                    OnError = string.Empty;
                    return false;
                }

            }

            var r = Tabla;
            OnError = string.Empty;
            return bool.Parse(r.ToString());
        }
        #endregion

        #region "Validacion si el cliente esta inactivo"
        public static bool ExisteUsuarioSellosInactivo(string ruc, out string OnError)
        {

            parametros.Clear();
            parametros.Add("ruc", ruc);

            var Tabla = sql_puntero.ComandoSelectEscalar(sql_puntero.Conexion_Local, 6000, "SELECT dbo.sellos_fx_usuario_imagenes_inactivo (@ruc);", parametros, out OnError);
            if (Tabla == null)
            {
                if (!string.IsNullOrEmpty(OnError))
                {
                    return false;
                }
                else
                {
                    OnError = string.Empty;
                    return false;
                }

            }

            var r = Tabla;
            OnError = string.Empty;
            return bool.Parse(r.ToString());
        }
        #endregion

        #region "Registra Cliente"
        private Int64? Save_Sellos(out string OnError)
        {

            parametros.Clear();

            parametros.Add("ClientId", this.ClientId);
            parametros.Add("Client", this.Client);
            parametros.Add("Create_user", this.Create_user);
            parametros.Add("file_pdf", this.file_pdf);
            parametros.Add("Email", this.Email);
            parametros.Add("activo", this.activo);
            var db = sql_puntero.ExecuteInsertUpdateDeleteReturn(sql_puntero.Conexion_Local, 6000, "sellos_registro_usuario", parametros, out OnError);
            if (!db.HasValue || db.Value < 0)
            {
                return null;
            }
            OnError = string.Empty;
            return db.Value;

        }

        public Int64? SaveTransaction_Sellos(out string OnError)
        {

            Int64 ID = 0;
            try
            {

                //grabar transaccion.
                var id = this.Save_Sellos(out OnError);
                if (!id.HasValue)
                {
                    OnError = "*** Error: al registrar cliente de Imágenes de Sellos ****";
                    return 0;
                }
                ID = id.Value;

                return ID;

            }
            catch (Exception ex)
            {

                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>("SQL", nameof(SaveTransaction_Sellos), "SaveTransaction_Sellos", false, null, null, ex.StackTrace, ex);
                OnError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));

                return null;
            }
        }
        #endregion

        #region "Registra Cargas y Contenedor"
        private Int64? Save_Contenedor(out string OnError)
        {

            parametros.Clear();

            parametros.Add("RUC", this.ruc);
            parametros.Add("NOMBRES", this.nombres);
            parametros.Add("GKEY", this.gkey);
            parametros.Add("NUMERO_CARGA", this.numero_carga);
            parametros.Add("CNTR", this.cntr);
            parametros.Add("USUARIOING", this.usuarioing);
            var db = sql_puntero.ExecuteInsertUpdateDeleteReturn(sql_puntero.Conexion_Local, 6000, "sellos_registro_contenedores_servicio", parametros, out OnError);
            if (!db.HasValue || db.Value < 0)
            {
                return null;
            }
            OnError = string.Empty;
            return db.Value;

        }

        public Int64? SaveTransaction_Contenedor(out string OnError)
        {

            Int64 ID = 0;
            try
            {

                //grabar transaccion.
                var id = this.Save_Contenedor(out OnError);
                if (!id.HasValue)
                {
                    OnError = "*** Error: al registrar contenedores por servicio de imágenes de sellos ****";
                    return 0;
                }
                ID = id.Value;

                return ID;

            }
            catch (Exception ex)
            {

                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>("SQL", nameof(SaveTransaction_Contenedor), "SaveTransaction_Contenedor", false, null, null, ex.StackTrace, ex);
                OnError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));

                return null;
            }
        }
        #endregion

        #region "Registra Cargas y Contenedor no aplicables"
        private Int64? Save_Contenedor_NoAplica(out string OnError)
        {

            parametros.Clear();

            parametros.Add("RUC", this.ruc);
            parametros.Add("NOMBRES", this.nombres);
            parametros.Add("GKEY", this.gkey);
            parametros.Add("NUMERO_CARGA", this.numero_carga);
            parametros.Add("CNTR", this.cntr);
            parametros.Add("USUARIOING", this.usuarioing);
            var db = sql_puntero.ExecuteInsertUpdateDeleteReturn(sql_puntero.Conexion_Local, 6000, "sellos_registro_contenedores_noservicio", parametros, out OnError);
            if (!db.HasValue || db.Value < 0)
            {
                return null;
            }
            OnError = string.Empty;
            return db.Value;

        }

        public Int64? SaveTransaction_Contenedor_NoAplica(out string OnError)
        {

            Int64 ID = 0;
            try
            {

                //grabar transaccion.
                var id = this.Save_Contenedor_NoAplica(out OnError);
                if (!id.HasValue)
                {
                    OnError = "*** Error: al registrar contenedores historial por servicio de imágenes de sellos ****";
                    return 0;
                }
                ID = id.Value;

                return ID;

            }
            catch (Exception ex)
            {

                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>("SQL", nameof(SaveTransaction_Contenedor_NoAplica), "SaveTransaction_Contenedor_NoAplica", false, null, null, ex.StackTrace, ex);
                OnError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));

                return null;
            }
        }
        #endregion

        #region "Listados"
        public static List<Cls_ImpoContenedor> Listado_Sellos_Clientes_filtro(DateTime? fecha_desde, DateTime? fecha_hasta, string criterio, out string OnError)
        {

            parametros.Clear();
            parametros.Add("fecha_desde", fecha_desde);
            parametros.Add("fecha_hasta", fecha_hasta);
            parametros.Add("criterio", criterio);
            return sql_puntero.ExecuteSelectControl<Cls_ImpoContenedor>(sql_puntero.Conexion_Local, 6000, "sellos_Listado_cliente_servicios", parametros, out OnError);

        }
        #endregion

        #region "Cancelar Servicio"
        public bool Delete_Sellos(out string OnError)
        {

            parametros.Clear();
            parametros.Add("Id", this.ID);
            parametros.Add("Create_user", this.Create_user);
            parametros.Add("Comment", this.Comment);
            parametros.Add("file_pdf", this.file_pdf);
            using (var scope = new System.Transactions.TransactionScope())
            {
                var db = sql_puntero.ExecuteInsertUpdateDelete(sql_puntero.Conexion_Local, 6000, "sellos_inactiva_servicio", parametros, out OnError);
                if (!db.HasValue || db.Value < 0)
                {
                    return false;
                }
                scope.Complete();
            }
            return true;
        }
        #endregion

        #endregion

    }
}
