using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlConexion;


namespace ClsAppCgsa
{
    public class MantenimientoPaqueteCliente : Base
    {
        #region "Variables"

        private Int64? _Id;
        private Int64? _PackageId;
        private string _ClientId;
        private string _Name;
        private string _Client;
        private string _Comment;
        private string _file_pdf;
        private string _Email;
        private static Int64? lm = -3;

        #endregion

        #region "Propiedades"

        public Int64? Id { get => _Id; set => _Id = value; }
        public Int64? PackageId { get => _PackageId; set => _PackageId = value; }
        public string ClientId { get => _ClientId; set => _ClientId = value; }
        public string Name { get => _Name; set => _Name = value; }
        public string Client { get => _Client; set => _Client = value; }
        public string Comment { get => _Comment; set => _Comment = value; }
        public string file_pdf { get => _file_pdf; set => _file_pdf = value; }
        public string Email { get => _Email; set => _Email = value; }
        public decimal Suscription { get; set; }
        #endregion

        private static void OnInit()
        {
            sql_puntero = (sql_puntero == null) ? Cls_Conexion.Conexion() : sql_puntero;
            parametros = new Dictionary<string, object>();
            nueva_conexion = Cls_Conexion.Nueva_Conexion("APPCGSA");
        }

        public MantenimientoPaqueteCliente()
        {
            init();
         

        }

        public static List<MantenimientoPaqueteCliente> Listado_clientes(string ClientId, out string OnError)
        {
            OnInit();

            parametros.Clear();
            parametros.Add("ClientId", ClientId);
            return sql_puntero.ExecuteSelectControl<MantenimientoPaqueteCliente>(nueva_conexion, 5000, "APC_BUSCA_PAQUETE_CLIENTE", parametros, out OnError);

        }

        public static List<MantenimientoPaqueteCliente> Listado_Paquetes_Clientes( out string OnError)
        {
            OnInit();
            parametros.Clear();
            return sql_puntero.ExecuteSelectControl<MantenimientoPaqueteCliente>(nueva_conexion, 5000, "APC_LISTADO_PAQUETE_CLIENTE", null, out OnError);

        }

        public static List<MantenimientoPaqueteCliente> Listado_Paquetes_Clientes_filtro(DateTime? fecha_desde, DateTime? fecha_hasta, string criterio, out string OnError)
        {
            OnInit();
            parametros.Clear();
            parametros.Add("fecha_desde", fecha_desde);
            parametros.Add("fecha_hasta", fecha_hasta);
            parametros.Add("criterio", criterio);
            return sql_puntero.ExecuteSelectControl<MantenimientoPaqueteCliente>(nueva_conexion, 5000, "APC_LISTADO_PAQUETE_CLIENTE_FILTRO", parametros, out OnError);

        }

        public static List<MantenimientoPaqueteCliente> Listado_Paquetes_Agentes_filtro(DateTime? fecha_desde, DateTime? fecha_hasta, string criterio, out string OnError)
        {
            OnInit();
            parametros.Clear();
            parametros.Add("fecha_desde", fecha_desde);
            parametros.Add("fecha_hasta", fecha_hasta);
            parametros.Add("criterio", criterio);
            return sql_puntero.ExecuteSelectControl<MantenimientoPaqueteCliente>(nueva_conexion, 5000, "APC_LISTADO_PAQUETE_CLIENTE_AGENTE_FILTRO", parametros, out OnError);
        }


        public static List<MantenimientoPaqueteCliente> Listado_Paquetes_Clientes_Inactivos(DateTime? fecha_desde, DateTime? fecha_hasta, string criterio, out string OnError)
        {
            OnInit();
            parametros.Clear();
            parametros.Add("fecha_desde", fecha_desde);
            parametros.Add("fecha_hasta", fecha_hasta);
            parametros.Add("criterio", criterio);
            return sql_puntero.ExecuteSelectControl<MantenimientoPaqueteCliente>(nueva_conexion, 5000, "APC_LISTADO_PAQUETE_CLIENTE_INACTIVOS", parametros, out OnError);

        }

        public static List<MantenimientoPaqueteCliente> Listado_Paquetes_Agentes_Inactivos(DateTime? fecha_desde, DateTime? fecha_hasta, string criterio, out string OnError)
        {
            OnInit();
            parametros.Clear();
            parametros.Add("fecha_desde", fecha_desde);
            parametros.Add("fecha_hasta", fecha_hasta);
            parametros.Add("criterio", criterio);
            return sql_puntero.ExecuteSelectControl<MantenimientoPaqueteCliente>(nueva_conexion, 5000, "APC_LISTADO_PAQUETE_AGENTES_INACTIVOS", parametros, out OnError);

        }

        private int? PreValidations(out string msg)
        {

            if (string.IsNullOrEmpty(this.Client))
            {
                msg = "Debe especificar el cliente";
                return 0;
            }
            if (string.IsNullOrEmpty(this.ClientId))
            {
                msg = "Debe especificar el código del cliente/ruc";
                return 0;
            }
            if (!this.PackageId.HasValue)
            {
                msg = "Debe especificar el paquete";
                return 0;
            }

            msg = string.Empty;
            return 1;

        }

        public Int64? Save(out string OnError)
        {
            if (this.PreValidations(out OnError) != 1)
            {
                return -1;
            }

            OnInit();

            parametros.Clear();
            parametros.Add("PackageId", this.PackageId);
            parametros.Add("ClientId", this.ClientId);
            parametros.Add("Client", this.Client);
            parametros.Add("Create_user", this.Create_user);
            parametros.Add("file_pdf", this.file_pdf);
            parametros.Add("Email", this.Email);
            using (var scope = new System.Transactions.TransactionScope())
            {

                var db = sql_puntero.ExecuteInsertUpdateDeleteReturn(nueva_conexion, 4000, "APC_MANTENIMIENTO_PAQUETESCLIENTES", parametros, out OnError);
                if (!db.HasValue || db.Value < 0)
                {

                    return null;
                }
                OnError = string.Empty;
                scope.Complete();
                return db.Value;
            }

        }

        public bool Delete(out string OnError)
        {
            OnInit();
            parametros.Clear();
            parametros.Add("Id", this.Id);
            parametros.Add("Create_user", this.Create_user);
            parametros.Add("Comment", this.Comment);
            parametros.Add("file_pdf", this.file_pdf);
            using (var scope = new System.Transactions.TransactionScope())
            {
                var db = sql_puntero.ExecuteInsertUpdateDelete(nueva_conexion, 6000, "APC_ELIMINA_PAQUETESCLIENTES", parametros, out OnError);
                if (!db.HasValue || db.Value < 0)
                {
                    return false;
                }
                scope.Complete();
            }
            return true;
        }

        public bool DeleteCFs(out string OnError)
        {
            OnInit();
            parametros.Clear();
            parametros.Add("ClientId", this.ClientId);
            parametros.Add("Create_user", this.Create_user);
          
            using (var scope = new System.Transactions.TransactionScope())
            {
                var db = sql_puntero.ExecuteInsertUpdateDelete(nueva_conexion, 6000, "APC_DESACTIVA_PAQUETESCLIENTES_CFS", parametros, out OnError);
                if (!db.HasValue || db.Value < 0)
                {
                    return false;
                }
                scope.Complete();
            }
            return true;
        }

        public bool DeleteAgente(out string OnError)
        {
            OnInit();
            parametros.Clear();
            parametros.Add("Id", this.Id);
            parametros.Add("Create_user", this.Create_user);
            parametros.Add("Comment", this.Comment);
            parametros.Add("file_pdf", this.file_pdf);
            using (var scope = new System.Transactions.TransactionScope())
            {
                var db = sql_puntero.ExecuteInsertUpdateDelete(nueva_conexion, 6000, "APC_ELIMINA_PAQUETESAGENTES", parametros, out OnError);
                if (!db.HasValue || db.Value < 0)
                {
                    return false;
                }
                scope.Complete();
            }
            return true;
        }

        public Int64? SaveAgente(out string OnError)
        {
            if (this.PreValidations(out OnError) != 1)
            {
                return -1;
            }

            OnInit();

            parametros.Clear();
            parametros.Add("PackageId", this.PackageId);
            parametros.Add("ClientId", this.ClientId);
            parametros.Add("Client", this.Client);
            parametros.Add("Create_user", this.Create_user);
            parametros.Add("file_pdf", this.file_pdf);
            parametros.Add("Email", this.Email);
            using (var scope = new System.Transactions.TransactionScope())
            {

                var db = sql_puntero.ExecuteInsertUpdateDeleteReturn(nueva_conexion, 4000, "APC_MANTENIMIENTO_PAQUETESAGENTE", parametros, out OnError);
                if (!db.HasValue || db.Value < 0)
                {

                    return null;
                }
                OnError = string.Empty;
                scope.Complete();
                return db.Value;
            }

        }
    }
}
