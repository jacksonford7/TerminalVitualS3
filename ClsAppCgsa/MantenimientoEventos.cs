using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlConexion;

namespace ClsAppCgsa
{
    public class MantenimientoEventos : Base
    {

        #region "Variables"

        private Int64? _Id = 0;
        private string _Name = string.Empty;
        private bool? _isPhoto = false;
        private string _Action = string.Empty;
        #endregion

        #region "Propiedades"
        public Int64? Id { get => _Id; set => _Id = value; }
        public string Name { get => _Name; set => _Name = value; }
        public bool? isPhoto { get => _isPhoto; set => _isPhoto = value; }
        public string Action { get => _Action; set => _Action = value; }

        #endregion

        private static String v_mensaje = string.Empty;

        public MantenimientoEventos()
        {
            init();
        }

        private static void OnInit()
        {
            sql_puntero = (sql_puntero == null) ? Cls_Conexion.Conexion() : sql_puntero;
            parametros = new Dictionary<string, object>();
            nueva_conexion = Cls_Conexion.Nueva_Conexion("APPCGSA");
        }

        private int? PreValidations(out string msg)
        {
           
            if (string.IsNullOrEmpty(this.Name))
            {
                msg = "Debe especificar el nombre del evento";
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
            parametros.Add("Id", this.Id);
            parametros.Add("Name", this.Name);
            parametros.Add("Create_user", this.Create_user);
            parametros.Add("isPhoto", this.isPhoto);
            parametros.Add("Action", this.Action);
          
            using (var scope = new System.Transactions.TransactionScope())
            {

                var db = sql_puntero.ExecuteInsertUpdateDeleteReturn(nueva_conexion, 4000, "APC_MANTENIMIENTO_EVENTO", parametros, out OnError);
                if (!db.HasValue || db.Value < 0)
                {

                    return null;
                }
                OnError = string.Empty;
                scope.Complete();
                return db.Value;
            }

        }

        public static List<MantenimientoEventos> Listado_Eventos(out string OnError)
        {
            OnInit();

            parametros.Clear();
            return sql_puntero.ExecuteSelectControl<MantenimientoEventos>(nueva_conexion, 5000, "APC_LISTA_EVENTO", null, out OnError);

        }

        public static List<MantenimientoEventos> Listado_PaquetesEventos(Int64 PackageId, out string OnError)
        {
            OnInit();

            parametros.Clear();
            parametros.Add("PackageId", PackageId);
            return sql_puntero.ExecuteSelectControl<MantenimientoEventos>(nueva_conexion, 5000, "APC_BUSCA_PAQUETESEVENTOS", parametros, out OnError);

        }

        public bool PopulateMyData(out string OnError)
        {
            OnInit();
            parametros.Clear();
            parametros.Add("Id", this.Id);
           
            var t = sql_puntero.ExecuteSelectOnly<MantenimientoEventos>(nueva_conexion, 6000, "APC_BUSCA_EVENTO", parametros);
            if (t == null)
            {
                OnError = "No fue posible obtener los datos del Evento ";
                return false;
            }

            this.Id = t.Id;
            this.Name = t.Name;
            this.Create_user = t.Create_user;
            this.Create_date = t.Create_date;
            this.Modifie_user = t.Modifie_user;
            this.Modifie_date = t.Modifie_date;
            this.Status = t.Status;
            this.isPhoto = t.isPhoto;


            OnError = string.Empty;
            return true;
        }

        public bool Delete(out string OnError)
        {
            OnInit();
            parametros.Clear();
            parametros.Add("Id", this.Id);
            parametros.Add("Create_user", this.Create_user);
            using (var scope = new System.Transactions.TransactionScope())
            {
                var db = sql_puntero.ExecuteInsertUpdateDelete(nueva_conexion, 6000, "APC_ELIMINA_EVENTO", parametros, out OnError);
                if (!db.HasValue || db.Value < 0)
                {
                    return false;
                }
                scope.Complete();
            }
            return true;
        }
    }
}
