using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ControlOPC.Entidades;
namespace ClsAutorizaciones
{
    public class Choferes : Base
    {

        #region "Variables"

        private Int64 _ID = 0;
        private string _ID_CHOFER = string.Empty;
        private string _NOMBRE_CHOFER = string.Empty;
        private string _STATUS = string.Empty;
        private DateTime? _LICENCIA_EXPIRACION;
        private DateTime? _LICENCIA_SUSPENDIDA;

        private string _ID_EMPRESA = string.Empty;
        private string _RAZON_SOCIAL = string.Empty;
        private string _LICENCIA_EXPIRACION_MENSAJE = string.Empty;
        private string _LICENCIA_SUSPENDIDA_MENSAJE = string.Empty;

        private string _MENSAJE = string.Empty;

        #endregion

        #region "Propiedades"
        public Int64 ID { get => _ID; set => _ID = value; }
        public string ID_CHOFER { get => _ID_CHOFER; set => _ID_CHOFER = value; }
        public string NOMBRE_CHOFER { get => _NOMBRE_CHOFER; set => _NOMBRE_CHOFER = value; }
        public string STATUS { get => _STATUS; set => _STATUS = value; }


        public DateTime? LICENCIA_EXPIRACION { get => _LICENCIA_EXPIRACION; set => _LICENCIA_EXPIRACION = value; }
        public DateTime? LICENCIA_SUSPENDIDA { get => _LICENCIA_SUSPENDIDA; set => _LICENCIA_SUSPENDIDA = value; }
        public string ID_EMPRESA { get => _ID_EMPRESA; set => _ID_EMPRESA = value; }
        public string RAZON_SOCIAL { get => _RAZON_SOCIAL; set => _RAZON_SOCIAL = value; }
        public string LICENCIA_EXPIRACION_MENSAJE { get => _LICENCIA_EXPIRACION_MENSAJE; set => _LICENCIA_EXPIRACION_MENSAJE = value; }
        public string LICENCIA_SUSPENDIDA_MENSAJE { get => _LICENCIA_SUSPENDIDA_MENSAJE; set => _LICENCIA_SUSPENDIDA_MENSAJE = value; }
        public string MENSAJE { get => _MENSAJE; set => _MENSAJE = value; }
        #endregion

        private static String v_mensaje = string.Empty;

        public Choferes()
        {
            init();
        }

        private static void OnInit()
        {
            sql_pointer = (sql_pointer == null) ? SQLHandler.sql_handler.GetInstance() : sql_pointer;
            parametros = new Dictionary<string, object>();

            v_conexion = Extension.Nueva_Conexion("N5_DB");
        }

        /*listado*/
        public static List<Choferes> Buscador_Choferes(string _criterio, string _ID_EMPRESA, out string OnError)
        {
            OnInit();
            parametros.Clear();
            parametros.Add("ID_EMPRESA", _ID_EMPRESA);
            parametros.Add("pista", _criterio);
            return sql_pointer.ExecuteSelectControl<Choferes>(v_conexion, 6000, "RVA_BUSCADOR_CHOFERES", parametros, out OnError);
        }

        /*poblar */
        public bool PopulateMyData(out string OnError)
        {
            OnInit();
            parametros.Clear();
            parametros.Add("ID_EMPRESA", this.ID_EMPRESA);
            parametros.Add("ID", this.ID);
            parametros.Add("LICENCIA", this.ID_CHOFER);

            var t = sql_pointer.ExecuteSelectOnly<Choferes>(v_conexion, 6000, "RVA_BUSCADOR_CHOFERES_UNICO", parametros);
            if (t == null)
            {
                OnError = "No fue posible obtener los datos del Chofer ";
                return false;
            }

            this.ID = t.ID;
            this.ID_CHOFER = t.ID_CHOFER;//LICENCIA
            this.NOMBRE_CHOFER = t.NOMBRE_CHOFER;
            this.ID_EMPRESA = t.ID_EMPRESA;
            this.RAZON_SOCIAL = t.RAZON_SOCIAL;
            this.STATUS = t.STATUS;
            this.LICENCIA_EXPIRACION = t.LICENCIA_EXPIRACION;
            this.LICENCIA_SUSPENDIDA = t.LICENCIA_SUSPENDIDA;
            this.LICENCIA_EXPIRACION_MENSAJE = t.LICENCIA_EXPIRACION_MENSAJE;
            this.LICENCIA_SUSPENDIDA_MENSAJE = t.LICENCIA_SUSPENDIDA_MENSAJE;

            //MENSAJES
            List<Mensajes> ListMsg = Mensajes.Listar_Mensajes(out v_mensaje);
            if (String.IsNullOrEmpty(v_mensaje))
            {
               
                var MSG_LICENCIA_EXP = (from Msg2 in ListMsg.Where(Msg2 => Msg2.CAMPO.Equals("EXPIRADO") && Msg2.TIPO == 2) select new { MENSAJE = Msg2.MENSAJE == null ? string.Empty : Msg2.MENSAJE }).FirstOrDefault();
                var MSG_STATUS = (from Msg3 in ListMsg.Where(Msg3 => Msg3.CAMPO.Equals("STATUS") && Msg3.TIPO == 2) select new { MENSAJE = Msg3.MENSAJE == null ? string.Empty : Msg3.MENSAJE }).FirstOrDefault();
                var MSG_LICENCIA_SUS = (from Msg4 in ListMsg.Where(Msg4 => Msg4.CAMPO.Equals("SUSPENDIDO") && Msg4.TIPO == 2) select new { MENSAJE = Msg4.MENSAJE == null ? string.Empty : Msg4.MENSAJE }).FirstOrDefault();


                this.MENSAJE = string.Format("{0} - {1} - {2}",
                                                            (!this.STATUS.Equals("OK") ? (MSG_STATUS.MENSAJE == null ? string.Empty : string.Format("{0}/{1}", MSG_STATUS.MENSAJE, this.STATUS)) : string.Empty),
                                                            (this.LICENCIA_EXPIRACION_MENSAJE.Equals("SI") ? (MSG_LICENCIA_EXP.MENSAJE == null ? string.Empty : string.Format("{0}:{1}", MSG_LICENCIA_EXP.MENSAJE, (this.LICENCIA_EXPIRACION == null ? string.Empty : this.LICENCIA_EXPIRACION.Value.ToString("dd/MM/yyyy")))) : string.Empty),
                                                            (this.LICENCIA_SUSPENDIDA_MENSAJE.Equals("SI") ? (MSG_LICENCIA_SUS.MENSAJE == null ? string.Empty : string.Format("{0}:{1}", MSG_LICENCIA_SUS.MENSAJE, (this.LICENCIA_SUSPENDIDA == null ? string.Empty : this.LICENCIA_SUSPENDIDA.Value.ToString("dd/MM/yyyy")))) : string.Empty));

            }
            OnError = string.Empty;
            return true;
        }

    }
}
