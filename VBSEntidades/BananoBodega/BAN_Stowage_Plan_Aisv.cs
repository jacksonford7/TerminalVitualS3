using BillionEntidades;
using SqlConexion;
using System;
using System.Collections.Generic;
using VBSEntidades.BananoMuelle;

namespace VBSEntidades.BananoBodega
{
    public class BAN_Stowage_Plan_Aisv : Cls_Bil_Base
    {
        #region "Propiedades"
        public long? idStowageAisv { get; set; }
        public long idStowageDet { get; set; }
        public long idStowagePlanTurno { get; set; }
        public DateTime fecha { get; set; }
        public int idHoraInicio { get; set; }
        public string horaInicio { get; set; }
        public int idHoraFin { get; set; }
        public string horaFin { get; set; }
        public int box { get; set; }
        public string comentario { get; set; }
        public string aisv { get; set; }
        public string dae { get; set; }
        public string booking { get; set; }
        public bool IIEAutorizada { get; set; }
        public bool daeAutorizada { get; set; }
        public string placa { get; set; }
        public string idChofer { get; set; }
        public string chofer { get; set; }
        public string estado { get; set; }
        public string usuarioCrea { get; set; }
        public DateTime fechaCreacion { get; set; }
        public string usuarioModifica { get; set; }
        public DateTime? fechaModifica { get; set; }
        public int arrastre { get; set; }
        public int pendiente { get; set; }
        public long? idCapacidadHoraBodega { get; set; }
        public BAN_HorarioInicial oHoraInicio { get; set; }
        public BAN_HorarioFinal oHoraFin { get; set; }
        public BAN_Stowage_Plan_Det oStowage_Plan_Det { get; set; }
        public List<BAN_Stowage_Movimiento> oListaStowage_Movimiento { get; set; }
        public BAN_Catalogo_Estado oEstados { get; set; }
        #endregion

        public BAN_Stowage_Plan_Aisv()
        {
            init();
        }

        private static void OnInit(string Base)
        {
            sql_puntero = (sql_puntero == null) ? Cls_Conexion.Conexion() : sql_puntero;
            parametros = new Dictionary<string, object>();
            nueva_conexion = Cls_Conexion.Nueva_Conexion(Base);
        }

        public static List<BAN_Stowage_Plan_Aisv> ConsultarLista(long idStowageDet, out string OnError)
        {
            parametros.Clear();
            parametros.Add("i_idStowageDet", idStowageDet);
            return sql_puntero.ExecuteSelectControl<BAN_Stowage_Plan_Aisv>(sql_punteroVBS.Conexion_LocalVBS, 8000, "[BAN_Stowage_Plan_Aisv_Consultar]", parametros, out OnError);
        }

        public static List<BAN_Stowage_Plan_Aisv> ConsultarLista(string idNave, out string OnError)
        {
            parametros.Clear();
            parametros.Add("i_idNave", idNave);
            return sql_puntero.ExecuteSelectControl<BAN_Stowage_Plan_Aisv>(sql_punteroVBS.Conexion_LocalVBS, 8000, "[BAN_Stowage_Plan_Aisv_ConsultarGenTV]", parametros, out OnError);
        }

        public static List<BAN_Stowage_Plan_Aisv> ConsultarListaXAISV(string aisv, out string OnError)
        {
            parametros.Clear();
            parametros.Add("i_aisv", aisv);
            return sql_puntero.ExecuteSelectControl<BAN_Stowage_Plan_Aisv>(sql_punteroVBS.Conexion_LocalVBS, 8000, "[BAN_Stowage_Plan_Aisv_Consultar]", parametros, out OnError);
        }
        public static List<BAN_Stowage_Plan_Aisv> LlenaComboAisv(string idNave,string placa, string chofer, string idExportador, out string OnError)
        {
            parametros.Clear();
            parametros.Add("i_idNave", idNave);
            parametros.Add("i_placa", placa);
            parametros.Add("i_chofer", chofer);
            parametros.Add("i_exportador", idExportador);
            return sql_puntero.ExecuteSelectControl<BAN_Stowage_Plan_Aisv>(sql_punteroVBS.Conexion_LocalVBS, 8000, "[BAN_Stowage_Plan_Aisv_LlenaComboAisv]", parametros, out OnError);
        }

        public static BAN_Stowage_Plan_Aisv GetEntidad(long _id)
        {
            parametros.Clear();
            parametros.Add("i_id", _id);
            var obj = sql_puntero.ExecuteSelectOnly<BAN_Stowage_Plan_Aisv>(sql_punteroVBS.Conexion_LocalVBS, 4000, "[BAN_Stowage_Plan_Aisv_Consultar]", parametros);
            return obj;
        }

        public Int64? Save_Update(out string OnError)
        {
            parametros.Clear();
            parametros.Add("i_idStowageAisv", this.idStowageAisv);
            parametros.Add("i_idStowageDet", this.idStowageDet);
            parametros.Add("i_fecha", this.fecha);
            parametros.Add("i_idHoraInicio", this.idHoraInicio);
            parametros.Add("i_horaInicio", this.horaInicio);
            parametros.Add("i_idHoraFin", this.idHoraFin);
            parametros.Add("i_horaFin", this.horaFin);
            parametros.Add("i_box", this.box);
            parametros.Add("i_comentario", this.comentario);
            parametros.Add("i_aisv", this.aisv);
            parametros.Add("i_dae", this.dae);
            parametros.Add("i_booking", this.booking);
            parametros.Add("i_IIEAutorizada", this.IIEAutorizada);
            parametros.Add("i_daeAutorizada", this.daeAutorizada);
            parametros.Add("i_placa", this.placa);
            parametros.Add("i_idChofer", this.idChofer);
            parametros.Add("i_chofer", this.chofer);
            parametros.Add("i_idCapacidadHoraBodega", this.idCapacidadHoraBodega);
            parametros.Add("i_estado", this.estado);
            parametros.Add("i_usuarioCrea", this.usuarioCrea);
            parametros.Add("i_usuarioModifica", this.usuarioModifica);

            var db = sql_puntero.ExecuteInsertUpdateDeleteReturn(sql_punteroVBS.Conexion_LocalVBS, 6000, "[BAN_Stowage_Plan_Aisv_Insertar]", parametros, out OnError);
            if (!db.HasValue || db.Value < 0)
            {
                return null;
            }
            OnError = string.Empty;
            return db.Value;
        }

        public Int64? Save_anulacion(out string OnError)
        {
            parametros.Clear();
            parametros.Add("i_idStowageAisv", this.idStowageAisv);
            parametros.Add("i_usuarioModifica", this.usuarioModifica);

            var db = sql_puntero.ExecuteInsertUpdateDeleteReturn(sql_punteroVBS.Conexion_LocalVBS, 6000, "[BAN_Stowage_Plan_Aisv_Eliminar]", parametros, out OnError);
            if (!db.HasValue || db.Value < 0)
            {
                return null;
            }
            OnError = string.Empty;
            return db.Value;
        }
    }
}
