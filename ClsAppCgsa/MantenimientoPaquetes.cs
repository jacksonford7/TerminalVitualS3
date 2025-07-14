using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlConexion;

namespace ClsAppCgsa
{
    public class MantenimientoPaquetes : Base
    {
        #region "Variables"

        private Int64? _Id = 0;
        private string _Name = string.Empty;
        private string _IdEventsN4 = string.Empty;
        private string _EventoN4 = string.Empty;
        private string _Action = string.Empty;
        #endregion

        #region "Propiedades"
        public Int64? Id { get => _Id; set => _Id = value; }
        public string Name { get => _Name; set => _Name = value; }
        public string IdEventsN4 { get => _IdEventsN4; set => _IdEventsN4 = value; }
        public string EventoN4 { get => _EventoN4; set => _EventoN4 = value; }
        public string Action { get => _Action; set => _Action = value; }

        #endregion

        private static String v_mensaje = string.Empty;

        public MantenimientoPaquetes()
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
                msg = "Debe especificar la descripción";
                return 0;
            }
            if (string.IsNullOrEmpty(this.IdEventsN4))
            {
                msg = "Debe especificar el código del evento";
                return 0;
            }
            if (string.IsNullOrEmpty(this.EventoN4))
            {
                msg = "Debe especificar la descripción del evento";
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
            parametros.Add("IdEventsN4", this.IdEventsN4);
            parametros.Add("EventoN4", this.EventoN4);
            parametros.Add("Create_user", this.Create_user);
            parametros.Add("Action", this.Action);

            using (var scope = new System.Transactions.TransactionScope())
            {

                var db = sql_puntero.ExecuteInsertUpdateDeleteReturn(nueva_conexion, 4000, "APC_MANTENIMIENTO_PAQUETES", parametros, out OnError);
                if (!db.HasValue || db.Value < 0)
                {

                    return null;
                }
                OnError = string.Empty;
                scope.Complete();
                return db.Value;
            }

        }

        public static List<MantenimientoPaquetes> Listado_Paquetes(out string OnError)
        {
            OnInit();

            parametros.Clear();
            return sql_puntero.ExecuteSelectControl<MantenimientoPaquetes>(nueva_conexion, 5000, "APC_LISTA_PAQUETES", null, out OnError);

        }

        public static List<MantenimientoPaquetes> Buscador_Paquetes(string _criterio, out string OnError)
        {
            OnInit();

            parametros.Clear();
            parametros.Add("pista", _criterio);
            return sql_puntero.ExecuteSelectControl<MantenimientoPaquetes>(nueva_conexion, 5000, "APC_BUSCADOR_PAQUETES", parametros, out OnError);

        }

        public static List<MantenimientoPaquetes> Combo_Paquetes(out string OnError)
        {
            OnInit();

            parametros.Clear();
            return sql_puntero.ExecuteSelectControl<MantenimientoPaquetes>(nueva_conexion, 5000, "APC_COMBO_PAQUETES", null, out OnError);

        }

        public static List<MantenimientoPaquetes> Combo_Paquetes_Agente(out string OnError)
        {
            OnInit();

            parametros.Clear();
            return sql_puntero.ExecuteSelectControl<MantenimientoPaquetes>(nueva_conexion, 5000, "APC_COMBO_PAQUETES_AGENTE", null, out OnError);

        }

        public bool PopulateMyData(out string OnError)
        {
            OnInit();
            parametros.Clear();
            parametros.Add("Id", this.Id);

            var t = sql_puntero.ExecuteSelectOnly<MantenimientoPaquetes>(nueva_conexion, 6000, "APC_BUSCA_PAQUETE", parametros);
            if (t == null)
            {
                OnError = "No fue posible obtener los datos del paquete";
                return false;
            }

            this.Id = t.Id;
            this.Name = t.Name;
            this.IdEventsN4 = t.IdEventsN4;
            this.EventoN4 = t.EventoN4;
            this.Create_user = t.Create_user;
            this.Create_date = t.Create_date;
            this.Modifie_user = t.Modifie_user;
            this.Modifie_date = t.Modifie_date;
            this.Status = t.Status;
           


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
                var db = sql_puntero.ExecuteInsertUpdateDelete(nueva_conexion, 6000, "APC_ELIMINA_PAQUETE", parametros, out OnError);
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
