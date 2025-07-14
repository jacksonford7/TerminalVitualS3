using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ControlOPC.Entidades;

namespace ClsAutorizaciones
{
    public class Auditoria : Base
    {

        #region "Variables"

        private Int64 _ID = 0;
        private string _USUARIO = string.Empty;
        private string _OPCION = string.Empty;
        private string _ACCION = string.Empty;
        private bool _ESTADO = false;
        private DateTime _FECHA;
        //auditoria
        #endregion

        #region "Propiedades"
        public Int64 ID { get => _ID; set => _ID = value; }
        public string USUARIO { get => _USUARIO; set => _USUARIO = value; }
        public string OPCION { get => _OPCION; set => _OPCION = value; }
        public string ACCION { get => _ACCION; set => _ACCION = value; }
        public bool ESTADO { get => _ESTADO; set => _ESTADO = value; }
        public DateTime FECHA_ { get => _FECHA; set => _FECHA = value; }
        #endregion

        private static String v_mensaje = string.Empty;

        public Auditoria()
        {
            init();
        }

        private static void OnInit()
        {
            sql_pointer = (sql_pointer == null) ? SQLHandler.sql_handler.GetInstance() : sql_pointer;
            parametros = new Dictionary<string, object>();

            v_conexion = Extension.Nueva_Conexion("PORTAL_N4");
        }

        public Int64? Save(out string OnError)
        {
           

            OnInit();

            parametros.Clear();
            parametros.Add("USUARIO", this.USUARIO);
            parametros.Add("OPCION", this.OPCION);
            parametros.Add("ACCION", this.ACCION);
           

            using (var scope = new System.Transactions.TransactionScope())
            {

                var db = sql_pointer.ExecuteInsertUpdateDeleteReturn(v_conexion, 4000, "RVA_GRABA_AUDITORIA", parametros, out OnError);
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
