using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ControlOPC.Entidades;

namespace ClsAutorizaciones
{
    public class Vehiculos : Base
    {


        #region "Variables"

        private string _ID = string.Empty;
        private string _ID_VEHICULO = string.Empty;
        private string _PLACA = string.Empty;
        private string _TAG = string.Empty;
        private string _STATUS = string.Empty;
        private DateTime? _LICENCIA_EXPIRACION;
        private DateTime? _CABEZAL_EXPIRACION;
        private string _ID_EMPRESA = string.Empty;
        private string _RAZON_SOCIAL = string.Empty;
        private string _LICENCIA_EXPIRACION_MENSAJE = string.Empty;
        private string _CABEZAL_EXPIRACION_MENSAJE = string.Empty;
        private string _MENSAJE = string.Empty;


        #endregion

        #region "Propiedades"
        public string ID { get => _ID; set => _ID = value; }
        public string PLACA { get => _PLACA; set => _PLACA = value; }
        public string TAG { get => _TAG; set => _TAG = value; }
        public string STATUS { get => _STATUS; set => _STATUS = value; }


        public DateTime? LICENCIA_EXPIRACION { get => _LICENCIA_EXPIRACION; set => _LICENCIA_EXPIRACION = value; }
        public DateTime? CABEZAL_EXPIRACION { get => _CABEZAL_EXPIRACION; set => _CABEZAL_EXPIRACION = value; }
        public string ID_EMPRESA { get => _ID_EMPRESA; set => _ID_EMPRESA = value; }
        public string RAZON_SOCIAL { get => _RAZON_SOCIAL; set => _RAZON_SOCIAL = value; }
        public string LICENCIA_EXPIRACION_MENSAJE { get => _LICENCIA_EXPIRACION_MENSAJE; set => _LICENCIA_EXPIRACION_MENSAJE = value; }
        public string CABEZAL_EXPIRACION_MENSAJE { get => _CABEZAL_EXPIRACION_MENSAJE; set => _CABEZAL_EXPIRACION_MENSAJE = value; }
        public string MENSAJE { get => _MENSAJE; set => _MENSAJE = value; }
        #endregion

        private static String v_mensaje = string.Empty;

        public Vehiculos()
        {
            init();
        }

        private static void OnInit()
        {
            sql_pointer = (sql_pointer == null) ? SQLHandler.sql_handler.GetInstance() : sql_pointer;
            parametros = new Dictionary<string, object>();

            v_conexion = Extension.Nueva_Conexion("N5_DB");
        }

        /*listado */
        public static List<Vehiculos> Buscador_Vehiculos(string _criterio, string _ID_EMPRESA, out string OnError)
        {
            OnInit();
            parametros.Clear();
            parametros.Add("ID_EMPRESA", _ID_EMPRESA);
            parametros.Add("pista", _criterio);
            return sql_pointer.ExecuteSelectControl<Vehiculos>(v_conexion, 6000, "RVA_BUSCADOR_VEHICULOS", parametros, out OnError);
        }

        /*poblar */
        public bool PopulateMyData(out string OnError)
        {
            OnInit();
            parametros.Clear();
            parametros.Add("ID_EMPRESA", this.ID_EMPRESA);
            parametros.Add("ID_VEHICULO", this.ID);
            parametros.Add("PLACA", this.PLACA);


            var t = sql_pointer.ExecuteSelectOnly<Vehiculos>(v_conexion, 6000, "RVA_BUSCADOR_VEHICULOS_UNICO", parametros);
            if (t == null)
            {
                OnError = "No fue posible obtener los datos del Vehículos ";
                return false;
            }
            this.ID = t.ID;
            this.PLACA = t.PLACA;
            this.TAG = t.TAG;
            this.STATUS = t.STATUS;
            this.LICENCIA_EXPIRACION = t.LICENCIA_EXPIRACION;
            this.CABEZAL_EXPIRACION = t.CABEZAL_EXPIRACION;
            this.ID_EMPRESA = t.ID_EMPRESA;
            this.RAZON_SOCIAL = t.RAZON_SOCIAL;
            this.LICENCIA_EXPIRACION_MENSAJE = t.LICENCIA_EXPIRACION_MENSAJE;
            this.CABEZAL_EXPIRACION_MENSAJE = t.CABEZAL_EXPIRACION_MENSAJE;

            //MENSAJES
            List<Mensajes> ListMsg = Mensajes.Listar_Mensajes(out v_mensaje);
            if (String.IsNullOrEmpty(v_mensaje))
            {
                var MSG_TAGID = (from Msg in ListMsg.Where(Msg => Msg.CAMPO.Equals("TAGID") && Msg.TIPO == 1) select new { MENSAJE = Msg.MENSAJE == null ? string.Empty : Msg.MENSAJE }).FirstOrDefault();
                var MSG_LICENCIA_EXP = (from Msg2 in ListMsg.Where(Msg2 => Msg2.CAMPO.Equals("LICENCIA_EXP") && Msg2.TIPO == 1) select new { MENSAJE = Msg2.MENSAJE == null ? string.Empty : Msg2.MENSAJE }).FirstOrDefault();
                var MSG_STATUS = (from Msg3 in ListMsg.Where(Msg3 => Msg3.CAMPO.Equals("STATUS") && Msg3.TIPO == 1) select new { MENSAJE = Msg3.MENSAJE == null ? string.Empty : Msg3.MENSAJE }).FirstOrDefault();
                var MSG_EXP_CABEZAL = (from Msg4 in ListMsg.Where(Msg4 => Msg4.CAMPO.Equals("EXP_CABEZAL") && Msg4.TIPO == 1) select new { MENSAJE = Msg4.MENSAJE == null ? string.Empty : Msg4.MENSAJE }).FirstOrDefault();

                this.MENSAJE = string.Format("{0} - {1} - {2} - {3}",
                                                            (string.IsNullOrEmpty(this.TAG) ? (MSG_TAGID.MENSAJE == null ? string.Empty : MSG_TAGID.MENSAJE) : string.Empty),
                                                            (!this.STATUS.Equals("OK") ? (MSG_STATUS.MENSAJE == null ? string.Empty : string.Format("{0}/{1}", MSG_STATUS.MENSAJE, this.STATUS)) : string.Empty),
                                                            (this.LICENCIA_EXPIRACION_MENSAJE.Equals("SI") ? (MSG_LICENCIA_EXP.MENSAJE == null ? string.Empty : string.Format("{0}:{1}", MSG_LICENCIA_EXP.MENSAJE, (this.LICENCIA_EXPIRACION == null ? string.Empty : this.LICENCIA_EXPIRACION.Value.ToString("dd/MM/yyyy")))) : string.Empty),
                                                            (this.CABEZAL_EXPIRACION_MENSAJE.Equals("SI") ? (MSG_EXP_CABEZAL.MENSAJE == null ? string.Empty : string.Format("{0}:{1}", MSG_EXP_CABEZAL.MENSAJE, (this.CABEZAL_EXPIRACION == null ? string.Empty : this.CABEZAL_EXPIRACION.Value.ToString("dd/MM/yyyy")))) : string.Empty));
                                                            
            }
            OnError = string.Empty;
            return true;
        }
    }
}
