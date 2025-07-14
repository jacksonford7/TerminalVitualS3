using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ControlOPC.Entidades;
using System.Data;
using System.Reflection;

namespace ClsAutorizaciones
{
    public class MantenimientoChoferes : Base
    {
        #region "Variables"

        private Int64 _ID = 0;
        private Int64 _ID_CHOFER = 0;
        private string _LICENCIA = string.Empty;
        private string _NOMBRES  = string.Empty;
        private string _STATUS = string.Empty;
        private DateTime? _LICENCIA_EXPIRACION;
        private DateTime? _CHOFER_SUSPENDIDO;
        private string _ID_EMPRESA = string.Empty;
        private string _RAZON_SOCIAL = string.Empty;
        private string _LICENCIA_EXPIRACION_MENSAJE = string.Empty;
        private string _LICENCIA_SUSPENDIDA_MENSAJE = string.Empty;
        private string _USUARIO_CRE = string.Empty;
        private bool _ESTADO = false;
        private string _LINEA_NAVIERA = string.Empty;
        private DateTime _FECHA_CREA;
        private string _MENSAJE = string.Empty;
        private string _MOTIVO = string.Empty;

        #endregion

        #region "Propiedades"
        public Int64 ID { get => _ID; set => _ID = value; }
        public Int64 ID_CHOFER { get => _ID_CHOFER; set => _ID_CHOFER = value; }
        public string LICENCIA { get => _LICENCIA; set => _LICENCIA = value; }
        public string NOMBRES { get => _NOMBRES; set => _NOMBRES = value; }
        public string STATUS { get => _STATUS; set => _STATUS = value; }


        public DateTime? LICENCIA_EXPIRACION { get => _LICENCIA_EXPIRACION; set => _LICENCIA_EXPIRACION = value; }
        public DateTime? CHOFER_SUSPENDIDO { get => _CHOFER_SUSPENDIDO; set => _CHOFER_SUSPENDIDO = value; }
        public string ID_EMPRESA { get => _ID_EMPRESA; set => _ID_EMPRESA = value; }
        public string RAZON_SOCIAL { get => _RAZON_SOCIAL; set => _RAZON_SOCIAL = value; }
        public string LICENCIA_EXPIRACION_MENSAJE { get => _LICENCIA_EXPIRACION_MENSAJE; set => _LICENCIA_EXPIRACION_MENSAJE = value; }
        public string LICENCIA_SUSPENDIDA_MENSAJE { get => _LICENCIA_SUSPENDIDA_MENSAJE; set => _LICENCIA_SUSPENDIDA_MENSAJE = value; }

        public string USUARIO_CRE { get => _USUARIO_CRE; set => _USUARIO_CRE = value; }
        public bool ESTADO { get => _ESTADO; set => _ESTADO = value; }
        public string LINEA_NAVIERA { get => _LINEA_NAVIERA; set => _LINEA_NAVIERA = value; }
        public DateTime FECHA_CREA { get => _FECHA_CREA; set => _FECHA_CREA = value; }
        public string MENSAJE { get => _MENSAJE; set => _MENSAJE = value; }
        public string MOTIVO { get => _MOTIVO; set => _MOTIVO = value; }
        #endregion

        private static String v_mensaje = string.Empty;

        public MantenimientoChoferes()
        {
            init();
        }

        private static void OnInit()
        {
            sql_pointer = (sql_pointer == null) ? SQLHandler.sql_handler.GetInstance() : sql_pointer;
            parametros = new Dictionary<string, object>();

            v_conexion = Extension.Nueva_Conexion("PORTAL_N4");
        }

        public bool Existe_Chofer(out string OnError)
        {

            OnInit();
            parametros.Clear();
            parametros.Add("ID_EMPRESA", this.ID_EMPRESA);
            parametros.Add("ID_CHOFER", this.ID_CHOFER);
            parametros.Add("LICENCIA", this.LICENCIA);
            parametros.Add("LINEA_NAVIERA", this.LINEA_NAVIERA);

            var t = sql_pointer.ExecuteSelectOnly<MantenimientoChoferes>(v_conexion, 6000, "RVA_VALIDA_CHOFER", parametros);
            if (t == null)
            {
                OnError = string.Empty;
                return false;
            }

            this.ID = t.ID;
            this.ID_CHOFER = t.ID_CHOFER;
            this.LICENCIA = t.LICENCIA;
            this.NOMBRES = t.NOMBRES;

            OnError = string.Empty;
            return true;
        }

        private int? PreValidations(out string msg)
        {
            if (this.ID_CHOFER==0)
            {
                msg = "Debe especificar código del chofer";
                return 0;
            }
            if (string.IsNullOrEmpty(this.LICENCIA))
            {
                msg = "Debe especificar la licencia del chofer";
                return 0;
            }
            if (string.IsNullOrEmpty(this.ID_EMPRESA))
            {
                msg = "Debe especificar el ruc de la empresa de transporte";
                return 0;
            }


            if (string.IsNullOrEmpty(this.LINEA_NAVIERA))
            {
                msg = "Debe especificar la línea naviera";
                return 0;
            }
            if (string.IsNullOrEmpty(this.USUARIO_CRE))
            {
                msg = "Especifique el usuario creador del registro";
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
            parametros.Add("ID_CHOFER", this.ID_CHOFER);
            parametros.Add("LICENCIA", this.LICENCIA);
            parametros.Add("NOMBRES", this.NOMBRES);
            parametros.Add("ID_EMPRESA", this.ID_EMPRESA);
            parametros.Add("STATUS", this.STATUS);
            parametros.Add("LICENCIA_EXPIRACION", this.LICENCIA_EXPIRACION);
            parametros.Add("CHOFER_SUSPENDIDO", this.CHOFER_SUSPENDIDO);
            parametros.Add("USUARIO_CRE", this.USUARIO_CRE);
            parametros.Add("ESTADO", this.ESTADO);
            parametros.Add("LINEA_NAVIERA", this.LINEA_NAVIERA);
            parametros.Add("MENSAJE", this.MENSAJE);
            parametros.Add("MOTIVO", this.MOTIVO);
            using (var scope = new System.Transactions.TransactionScope())
            {

                var db = sql_pointer.ExecuteInsertUpdateDeleteReturn(v_conexion, 4000, "RVA_GRABA_NO_AUTORIZACION_CHOFER", parametros, out OnError);
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
            parametros.Clear();
            parametros.Add("ID", this.ID);
            parametros.Add("USUARIO_CRE", this.USUARIO_CRE);
            using (var scope = new System.Transactions.TransactionScope())
            {
                var db = sql_pointer.ExecuteInsertUpdateDelete(v_conexion, 6000, "RVA_ELIMINA_AUTORIZACION_CHOFER", parametros, out OnError);
                if (!db.HasValue || db.Value < 0)
                {
                    return false;
                }
                scope.Complete();
            }
            return true;
        }



        /*listado para mostrar choferes excluidos y poder filtrar*/
        public static List<MantenimientoChoferes> Listado_Choferes(string criterio, string LINEA_NAVIERA, out string OnError)
        {
            OnInit();
            parametros.Clear();
            parametros.Add("LINEA_NAVIERA", LINEA_NAVIERA);
            parametros.Add("criterio", criterio);
            return sql_pointer.ExecuteSelectControl<MantenimientoChoferes>(v_conexion, 6000, "RVA_LISTAR_CHOFERES_NOAUTORIZADOS", parametros, out OnError);
        }

    }
}
