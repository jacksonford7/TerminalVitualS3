using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlConexion;

namespace BillionEntidades
{
    public class Cls_CFS_Turnos_Banano : Cls_Bil_Base
    {

        private static Int64? lm = -3;

        #region "Propiedades"

        public Int64 idLoadingDet { get ; set ; }
        public string horaInicio { get ; set ; }
        public string horaFin { get ; set ; }
        public int box { get ; set ; }
        public string aisvUsuarioCrea { get; set; }
        public string aisv_codigo { get; set; }
        public DateTime? fecha { get; set; }
        public DateTime? vbs_fecha_cita { get; set; }
        public Int64? vbs_id_hora_cita { get; set; }
        public int? vbs_destino { get; set; }
        public int aisv_cant_bult { get; set; }
        public string idNave { get; set; }
        public string estado { get; set; }
        public string bodega { get; set; }

        public string idLoadingDet_remanente { get; set; }
        public int fila { get; set; }
        public int saldo { get; set; }
        public int saldo_nuevo { get; set; }
        public int reservado { get; set; }
        public int cantidad_inicial { get; set; }

        //propiedades Bodega
        public Int64 idStowageAisv { get; set; }
        public Int64 idStowageDet { get; set; }
        public Int64 idStowagePlanTurno { get; set; }
        public int idHoraInicio { get; set; }
        public int idHoraFin { get; set; }
        public int disponible { get; set; }
        public int boxExtra { get; set; }
        public string dae { get; set; }
        public string booking { get; set; }
        public string placa { get; set; }
        public string idChofer { get; set; }
        public string chofer { get; set; }
        public bool isActive { get; set; }
        public long idCapacidadHorafecha { get; set; }
        public long idCapacidadHoraBodega { get; set; }
        public string aisv_referencia { get; set; }
        #endregion



        public Cls_CFS_Turnos_Banano()
        {
            init();
        }

        private static void OnInit(string Base)
        {
            sql_puntero = (sql_puntero == null) ? Cls_Conexion.Conexion() : sql_puntero;
            parametros = new Dictionary<string, object>();
            nueva_conexion = Cls_Conexion.Nueva_Conexion(Base);
        }

        #region "Listados"

        public static List<Cls_CFS_Turnos_Banano> Carga_Turnos(string i_refencia, string i_exportador, DateTime i_fecha, int i_cantidad, out string OnError)
        {
            OnInit("VBS");
            parametros.Add("i_refencia", i_refencia);
            parametros.Add("i_exportador", i_exportador);
            parametros.Add("i_fecha", i_fecha);
            parametros.Add("i_cantidad", i_cantidad);
            return sql_puntero.ExecuteSelectControl<Cls_CFS_Turnos_Banano>(nueva_conexion, 7000, "BAN_CONSULTAR_TURNO_CLIENTE_AISV", parametros, out OnError);

        }

        public static List<Cls_CFS_Turnos_Banano> Carga_Turnos_Bodega(string i_refencia, string i_exportador, DateTime i_fecha, int i_cantidad, out string OnError)
        {
            OnInit("VBS");
            parametros.Add("i_refencia", i_refencia);
            parametros.Add("i_exportador", i_exportador);
            parametros.Add("i_fecha", i_fecha);
            parametros.Add("i_cantidad", i_cantidad);
            return sql_puntero.ExecuteSelectControl<Cls_CFS_Turnos_Banano>(nueva_conexion, 7000, "BAN_CONSULTAR_TURNO_CLIENTE_AISV_BODEGA", parametros, out OnError);
        }

        public static List<Cls_CFS_Turnos_Banano> Carga_Turnos_Remanente(string i_refencia, string i_exportador, DateTime i_fecha, int i_cantidad, out string OnError)
        {
            OnInit("VBS");
            parametros.Add("i_refencia", i_refencia);
            parametros.Add("i_exportador", i_exportador);
            parametros.Add("i_fecha", i_fecha);
            parametros.Add("i_cantidad", i_cantidad);
            return sql_puntero.ExecuteSelectControl<Cls_CFS_Turnos_Banano>(nueva_conexion, 7000, "BAN_CONSULTAR_TURNO_CLIENTE_AISV_REMANENTE", parametros, out OnError);

        }

        public static List<Cls_CFS_Turnos_Banano> Carga_Turnos_Remanente(long i_idStowageDet, DateTime i_fecha, int i_cantidad, out string OnError)
        {
            OnInit("VBS");
            parametros.Add("i_idStowageDet", i_idStowageDet);
            parametros.Add("i_fecha", i_fecha.ToString("yyyyMMdd"));
            parametros.Add("i_cantidad", i_cantidad);
            return sql_puntero.ExecuteSelectControl<Cls_CFS_Turnos_Banano>(nueva_conexion, 7000, "BAN_Stowage_Plan_Aisv_Turnos", parametros, out OnError);

        }
        #endregion

        #region "Consulta turno Remanente"
        public bool PopulateMyData_Remante(out string OnError)
        {
            OnInit("VBS");
            parametros.Clear();
            parametros.Add("idLoadingDet_remanente", this.idLoadingDet_remanente);

            var t = sql_puntero.ExecuteSelectOnly<Cls_CFS_Turnos_Banano>(nueva_conexion, 6000, "BAN_CONSULTAR_ID_TURNO_REMANTEN", parametros);
            if (t == null)
            {
                OnError = "No existe TURNO..";
                return false;
            }

         
            this.box = t.box;
           
            OnError = string.Empty;
            return true;
        }

        #endregion

        #region "Actualizar turno remanente"
        private Int64? actualiza_turno_remanente(out string OnError)
        {

            parametros.Clear();

            parametros.Add("idLoadingDet_remanente", this.idLoadingDet_remanente);
            parametros.Add("aisv_codigo", this.aisv_codigo);
            parametros.Add("cantidad", this.box);
            parametros.Add("aisvUsuarioCrea", this.aisvUsuarioCrea);

            var db = sql_puntero.ExecuteInsertUpdateDeleteReturn(sql_puntero.Conexion_Local, 6000, "vbs_actualiza_reserva_remanente", parametros, out OnError);
            if (!db.HasValue || db.Value < 0)
            {
                return null;
            }
            OnError = string.Empty;
            return db.Value;

        }
        public Int64? SaveTransaction_VBS_remanente(out string OnError)
        {

            Int64 ID = 0;
            try
            {

                using (var scope = new System.Transactions.TransactionScope())
                {
                    //grabar cabecera de la transaccion.
                    var id = this.actualiza_turno_remanente(out OnError);
                    if (!id.HasValue)
                    {
                        OnError = "*** Error: al actualizar reserva de banano ****";
                        return 0;
                    }
                    ID = id.Value;

                    //fin de la transaccion
                    scope.Complete();

                    return ID;
                }
            }
            catch (Exception ex)
            {

                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>("SQL", nameof(SaveTransaction_VBS), "SaveTransaction_VBS", false, null, null, ex.StackTrace, ex);
                OnError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));

                return null;
            }
        }

        private Int64? actualiza_turno_remanenteBodega(out string OnError)
        {

            parametros.Clear();

            parametros.Add("i_idStowagePlanTurno", this.idStowagePlanTurno);
            parametros.Add("i_aisv_codigo", this.aisv_codigo);
            parametros.Add("i_reservado", this.box);
            parametros.Add("i_disponible", this.disponible);
            parametros.Add("i_isActive", this.isActive);

            var db = sql_puntero.ExecuteInsertUpdateDeleteReturn(sql_punteroVBS.Conexion_LocalVBS, 6000, "[BAN_Stowage_Plan_Turno_Update]", parametros, out OnError);
            if (!db.HasValue || db.Value < 0)
            {
                return null;
            }
            OnError = string.Empty;
            return db.Value;

        }

        public Int64? BAN_Stowage_Plan_Aisv_Save_Update(out string OnError)
        {
            parametros.Clear();
            parametros.Add("i_idStowageAisv", this.idStowageAisv);
            parametros.Add("i_idStowageDet", this.idStowageDet);
            parametros.Add("i_idStowagePlanTurno", this.idStowagePlanTurno);
            parametros.Add("i_fecha", this.fecha);
            parametros.Add("i_idHoraInicio", this.idHoraInicio);
            parametros.Add("i_horaInicio", this.horaInicio);
            parametros.Add("i_idHoraFin", this.idHoraFin);
            parametros.Add("i_horaFin", this.horaFin);
            parametros.Add("i_box", this.box);
            parametros.Add("i_comentario", string.Empty);
            parametros.Add("i_aisv", this.aisv_codigo);
            parametros.Add("i_dae", this.dae);
            parametros.Add("i_booking", this.booking);
            parametros.Add("i_IIEAutorizada", false);
            parametros.Add("i_daeAutorizada", false);
            parametros.Add("i_placa", this.placa);
            parametros.Add("i_idChofer", this.idChofer);
            parametros.Add("i_chofer", this.chofer);
            parametros.Add("i_idCapacidadHorafecha", this.idCapacidadHorafecha);
            parametros.Add("i_idCapacidadHoraBodega", this.idCapacidadHoraBodega);
            parametros.Add("i_estado", this.estado);
            parametros.Add("i_usuarioCrea", this.aisvUsuarioCrea);
            parametros.Add("i_usuarioModifica", this.aisvUsuarioCrea);
            parametros.Add("i_aisv_referencia", this.aisv_referencia);

            var db = sql_puntero.ExecuteInsertUpdateDeleteReturn(sql_punteroVBS.Conexion_LocalVBS, 6000, "[BAN_Stowage_Plan_Aisv_Insertar]", parametros, out OnError);
            if (!db.HasValue || db.Value < 0)
            {
                return null;
            }
            OnError = string.Empty;
            return db.Value;
        }

        public Int64? SaveTransaction_VBS_remanenteBodega(out string OnError)
        {

            Int64 ID = 0;
            string OError;
            try
            {

                using (var scope = new System.Transactions.TransactionScope())
                {
                    var idStowageAisv = this.BAN_Stowage_Plan_Aisv_Save_Update(out OError);//oEntidad.Save_Update(out OError);
                    if (OError != string.Empty)
                    {
                        new Exception(OError);
                        //return null;
                    }

                    //grabar cabecera de la transaccion.
                    var id = this.actualiza_turno_remanenteBodega(out OnError);
                    if (!id.HasValue)
                    {
                        OnError = OnError + " *** Error: al actualizar reserva de banano Bodega****";
                        return 0;
                    }
                    ID = id.Value;
                                        

                    if (OError != string.Empty)
                    {
                        new Exception (OError);
                        return null;
                    }
                    else
                    {
                        scope.Complete();
                    }

                    //fin de la transaccion
                    //scope.Complete();

                    return ID;
                }
            }
            catch (Exception ex)
            {

                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>("SQL", nameof(SaveTransaction_VBS), "SaveTransaction_VBS", false, null, null, ex.StackTrace, ex);
                OnError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));

                return null;
            }
        }
        #endregion


        public bool PopulateMyData(out string OnError)
        {
            OnInit("VBS");
            parametros.Clear();
            parametros.Add("i_idLoadingDet", this.idLoadingDet);

            var t = sql_puntero.ExecuteSelectOnly<Cls_CFS_Turnos_Banano>(nueva_conexion, 6000, "BAN_CONSULTAR_ID_TURNO", parametros);
            if (t == null)
            {
                OnError = "No existe TURNO..";
                return false;
            }

            this.idLoadingDet = t.idLoadingDet;
            this.box = t.box;
            this.horaInicio = t.horaInicio;
            this.horaFin = t.horaFin;
            this.fecha = t.fecha;
            this.idNave = t.idNave;
            this.aisv_cant_bult = t.aisv_cant_bult;
            OnError = string.Empty;
            return true;
        }

        public bool PopulateMyDataBodega(out string OnError)
        {
            OnInit("VBS");
            parametros.Clear();
            parametros.Add("i_idStowagePlanTurno", this.idStowagePlanTurno);

            var t = sql_puntero.ExecuteSelectOnly<Cls_CFS_Turnos_Banano>(nueva_conexion, 6000, "BAN_CONSULTAR_ID_TURNO_BODEGA", parametros);
            if (t == null)
            {
                OnError = "No existe TURNO..";
                return false;
            }

            this.idStowagePlanTurno = t.idStowagePlanTurno;
            this.box = t.box;
            this.horaInicio = t.horaInicio;
            this.horaFin = t.horaFin;
            this.fecha = t.fecha;
            this.idNave = t.idNave;
            this.aisv_cant_bult = t.aisv_cant_bult;
            OnError = string.Empty;
            return true;
        }

        public bool PopulateMyData_Aisv(out string OnError)
        {
            OnInit("SERVICE");
            parametros.Clear();
            parametros.Add("aisv_codigo", this.aisv_codigo);

            var t = sql_puntero.ExecuteSelectOnly<Cls_CFS_Turnos_Banano>(nueva_conexion, 6000, "aisv_carga_datos", parametros);
            if (t == null)
            {
                OnError = "No existe TURNO..";
                return false;
            }

            this.idLoadingDet = t.idLoadingDet;
            this.box = t.box;
            this.horaInicio = t.horaInicio;
            this.horaFin = t.horaFin;
            this.fecha = t.fecha;
            this.vbs_destino = t.vbs_destino;
            this.aisv_cant_bult = t.aisv_cant_bult;
            this.vbs_fecha_cita = t.vbs_fecha_cita;
            this.vbs_id_hora_cita = t.vbs_id_hora_cita;
            this.estado = t.estado;
            OnError = string.Empty;
            return true;
        }

        private Int64? actualiza_turno(out string OnError)
        {

            parametros.Clear();

            parametros.Add("idLoadingDet", this.idLoadingDet);
            parametros.Add("aisv_codigo", this.aisv_codigo);
            parametros.Add("cantidad", this.box);
            parametros.Add("aisvUsuarioCrea", this.aisvUsuarioCrea);
           
            var db = sql_puntero.ExecuteInsertUpdateDeleteReturn(sql_puntero.Conexion_Local, 6000, "vbs_actualiza_reserva", parametros, out OnError);
            if (!db.HasValue || db.Value < 0)
            {
                return null;
            }
            OnError = string.Empty;
            return db.Value;

        }

        public Int64? SaveTransaction_VBS(out string OnError)
        {

            Int64 ID = 0;
            try
            {
               
                using (var scope = new System.Transactions.TransactionScope())
                {
                    //grabar cabecera de la transaccion.
                    var id = this.actualiza_turno(out OnError);
                    if (!id.HasValue)
                    {
                        OnError = "*** Error: al actualizar reserva de banano ****";
                        return 0;
                    }
                    ID = id.Value;

                    //fin de la transaccion
                    scope.Complete();

                    return ID;
                }
            }
            catch (Exception ex)
            {

                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>("SQL", nameof(SaveTransaction_VBS), "SaveTransaction_VBS", false, null, null, ex.StackTrace, ex);
                OnError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));

                return null;
            }
        }


        #region "Actualiza remanente VBS"
        private Int64? modifica_turno_remanente(out string OnError)
        {

            parametros.Clear();

            parametros.Add("idLoadingDet_remanente", this.idLoadingDet_remanente);
            parametros.Add("aisv_codigo", this.aisv_codigo);
            parametros.Add("cantidad", this.box);
            parametros.Add("aisvUsuarioCrea", this.aisvUsuarioCrea);

            var db = sql_puntero.ExecuteInsertUpdateDeleteReturn(sql_puntero.Conexion_Local, 6000, "vbs_cambio_reserva_remanente", parametros, out OnError);
            if (!db.HasValue || db.Value < 0)
            {
                return null;
            }
            OnError = string.Empty;
            return db.Value;

        }

        public Int64? SaveTransaction_VBS_Modifica_remanente(out string OnError)
        {

            Int64 ID = 0;
            try
            {

                using (var scope = new System.Transactions.TransactionScope())
                {
                    //grabar cabecera de la transaccion.
                    var id = this.modifica_turno_remanente(out OnError);
                    if (!id.HasValue)
                    {
                        OnError = "*** Error: al actualizar reserva de banano ****";
                        return 0;
                    }
                    ID = id.Value;

                    //fin de la transaccion
                    scope.Complete();

                    return ID;
                }
            }
            catch (Exception ex)
            {

                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>("SQL", nameof(SaveTransaction_VBS), "SaveTransaction_VBS", false, null, null, ex.StackTrace, ex);
                OnError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));

                return null;
            }
        }
        #endregion


        #region "Actualizar AISV BANANO"
        private Int64? actualiza_turno_aisv(out string OnError)
        {

            OnInit("SERVICE");

            parametros.Clear();

            parametros.Add("aisv_codigo", this.aisv_codigo);
            parametros.Add("vbs_fecha_cita", this.vbs_fecha_cita);
            parametros.Add("vbs_id_hora_cita", this.vbs_id_hora_cita);
            parametros.Add("vbs_destino", this.vbs_destino);
            parametros.Add("vbs_box", this.box);
            parametros.Add("usuario", this.aisvUsuarioCrea);
            var db = sql_puntero.ExecuteInsertUpdateDeleteReturn(nueva_conexion, 6000, "pc_update_aisv_2019", parametros, out OnError);
            if (!db.HasValue || db.Value < 0)
            {
                return null;
            }
            OnError = string.Empty;
            return db.Value;

        }

        public Int64? SaveTransaction_AISV(out string OnError)
        {

            Int64 ID = 0;
            try
            {

               
                    //grabar cabecera de la transaccion.
                    var id = this.actualiza_turno_aisv(out OnError);
                    if (!id.HasValue)
                    {
                        OnError = "*** Error: al actualizar reserva de turno de banano ****";
                        return 0;
                    }
                    ID = id.Value;


                    return ID;
                
            }
            catch (Exception ex)
            {

                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>("SQL", nameof(SaveTransaction_AISV), "SaveTransaction_AISV", false, null, null, ex.StackTrace, ex);
                OnError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));

                return null;
            }
        }
        #endregion

        #region "Actualizar AISV BANANO REMANENTE"
        private Int64? actualiza_turno_aisv_remanente(out string OnError)
        {

            OnInit("SERVICE");

            parametros.Clear();

            parametros.Add("aisv_codigo", this.aisv_codigo);
            parametros.Add("vbs_fecha_cita", this.vbs_fecha_cita);
            parametros.Add("vbs_id_hora_cita", this.idLoadingDet_remanente);
            parametros.Add("vbs_destino", this.vbs_destino);
            parametros.Add("vbs_box", this.box);
            parametros.Add("usuario", this.aisvUsuarioCrea);
            var db = sql_puntero.ExecuteInsertUpdateDeleteReturn(nueva_conexion, 6000, "pc_update_aisv_2019_Remanente", parametros, out OnError);
            if (!db.HasValue || db.Value < 0)
            {
                return null;
            }
            OnError = string.Empty;
            return db.Value;

        }

        public Int64? SaveTransaction_AISV_Remanente(out string OnError)
        {

            Int64 ID = 0;
            try
            {


                //grabar cabecera de la transaccion.
                var id = this.actualiza_turno_aisv_remanente(out OnError);
                if (!id.HasValue)
                {
                    OnError = "*** Error: al actualizar reserva de turno de banano ****";
                    return 0;
                }
                ID = id.Value;


                return ID;

            }
            catch (Exception ex)
            {

                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>("SQL", nameof(SaveTransaction_AISV_Remanente), "SaveTransaction_AISV_Remanente", false, null, null, ex.StackTrace, ex);
                OnError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));

                return null;
            }
        }
        #endregion


        #region "Actualizar VBS turno cancelado"
        private Int64? cancela_turno(out string OnError)
        {

            parametros.Clear();

            parametros.Add("idLoadingDet", this.idLoadingDet);
            parametros.Add("aisv_codigo", this.aisv_codigo);
            parametros.Add("cantidad", this.box);
            parametros.Add("aisvUsuarioCrea", this.aisvUsuarioCrea);

            var db = sql_puntero.ExecuteInsertUpdateDeleteReturn(sql_puntero.Conexion_Local, 6000, "vbs_cancela_reserva", parametros, out OnError);
            if (!db.HasValue || db.Value < 0)
            {
                return null;
            }
            OnError = string.Empty;
            return db.Value;

        }

        public Int64? SaveTransaction_CancelaVBS(out string OnError)
        {

            Int64 ID = 0;
            try
            {

                using (var scope = new System.Transactions.TransactionScope())
                {
                    //grabar cabecera de la transaccion.
                    var id = this.cancela_turno(out OnError);
                    if (!id.HasValue)
                    {
                        OnError = "*** Error: al cancelar reserva de banano ****";
                        return 0;
                    }
                    ID = id.Value;

                    //fin de la transaccion
                    scope.Complete();

                    return ID;
                }
            }
            catch (Exception ex)
            {

                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>("SQL", nameof(SaveTransaction_CancelaVBS), "SaveTransaction_CancelaVBS", false, null, null, ex.StackTrace, ex);
                OnError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));

                return null;
            }
        }

        private Int64? cancela_turnoBanano(out string OnError)
        {

            parametros.Clear();

            parametros.Add("i_idStowagePlanTurno", this.idStowagePlanTurno);
            parametros.Add("i_aisv_codigo", this.aisv_codigo);
            parametros.Add("i_cantidad", this.box);
            parametros.Add("i_aisvUsuarioCrea", this.aisvUsuarioCrea);

            var db = sql_puntero.ExecuteInsertUpdateDeleteReturn(sql_punteroVBS.Conexion_LocalVBS, 6000, "BAN_Cancela_reservaBanano", parametros, out OnError);
            if (!db.HasValue || db.Value < 0)
            {
                return null;
            }
            OnError = string.Empty;
            return db.Value;

        }

        public Int64? SaveTransaction_CancelaVBSBanano(out string OnError)
        {

            Int64 ID = 0;
            try
            {

                using (var scope = new System.Transactions.TransactionScope())
                {
                    //grabar cabecera de la transaccion.
                    var id = this.cancela_turnoBanano(out OnError);
                    if (!id.HasValue)
                    {
                        OnError = "*** Error: al cancelar reserva de banano ****";
                        return 0;
                    }
                    ID = id.Value;

                    //fin de la transaccion
                    scope.Complete();

                    return ID;
                }
            }
            catch (Exception ex)
            {

                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>("SQL", nameof(SaveTransaction_CancelaVBS), "SaveTransaction_CancelaVBS", false, null, null, ex.StackTrace, ex);
                OnError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));

                return null;
            }
        }
        #endregion
    }

    public class Cls_SuscripcionBanano : Cls_Bil_Base
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
        #endregion

        public Cls_SuscripcionBanano()
        {
            init();
        }
        private static void OnInit(string Base)
        {
            sql_puntero = (sql_puntero == null) ? Cls_Conexion.Conexion() : sql_puntero;
            parametros = new Dictionary<string, object>();
            nueva_conexion = Cls_Conexion.Nueva_Conexion(Base);
        }

        #region "Registra Agente y Verifica Existencia"
        public Int64? SaveCliente(out string OnError)
        {
            OnInit("APPCGSA");

            parametros.Clear();

            parametros.Add("ClientId", ClientId);
            parametros.Add("Client", this.Client);
            parametros.Add("Create_user", this.Create_user);
            parametros.Add("file_pdf", this.file_pdf);
            parametros.Add("comentario", this.Comment);
            parametros.Add("activo", this.activo);
            var db = sql_puntero.ExecuteInsertUpdateDeleteReturn(nueva_conexion, 6000, "BAN_insertarPackageClientsAgents", parametros, out OnError);
            if (!db.HasValue || db.Value < 0)
            {
                return null;
            }
            OnError = string.Empty;
            return db.Value;

        }

        //metodo para verificar si es agente
        public static bool? VerificaSiExisteCliente(string ruc, out string OnError)
        {
            OnInit("APPCGSA");
            parametros.Clear();
            parametros.Add("ruc", ruc);
            return sql_puntero.ExecuteSelectOnlyBool(nueva_conexion, 6000, "BAN_existePackageClientsAgents", parametros, out OnError);

        }
        #endregion






    }
}
