using BillionEntidades;
using SqlConexion;
using System;
using System.Collections.Generic;
using VBSEntidades.BananoMuelle;

namespace VBSEntidades.BananoBodega
{
    public class BAN_Stowage_Plan_Turno : Cls_Bil_Base
    {
        #region "Propiedades"
        public long? idStowagePlanTurno { get; set; }
        public long idStowageDet { get; set; }
        public DateTime fecha { get; set; }
        public int idHoraInicio { get; set; }
        public string horaInicio { get; set; }
        public int idHoraFin { get; set; }
        public string horaFin { get; set; }
        public int box { get; set; }
        public int reservado { get; set; }
        public int disponible { get; set; }
        public long? idCapacidadHorafecha { get; set; }
        public long? idCapacidadHoraBodega { get; set; }
        public string estado { get; set; }
        public string usuarioCrea { get; set; }
        public DateTime fechaCreacion { get; set; }
        public string usuarioModifica { get; set; }
        public DateTime? fechaModifica { get; set; }
        //public int arrastre { get; set; }
        //public int pendiente { get; set; }
        
        public BAN_HorarioInicial oHoraInicio { get; set; }
        public BAN_HorarioFinal oHoraFin { get; set; }
        public BAN_Stowage_Plan_Det oStowage_Plan_Det { get; set; }
        public List<BAN_Stowage_Movimiento> oListaStowage_Movimiento { get; set; }
        public BAN_Catalogo_Estado oEstados { get; set; }
        public bool isActive { get; set; }
        #endregion

        public BAN_Stowage_Plan_Turno()
        {
            init();
        }

        private static void OnInit(string Base)
        {
            sql_puntero = (sql_puntero == null) ? Cls_Conexion.Conexion() : sql_puntero;
            parametros = new Dictionary<string, object>();
            nueva_conexion = Cls_Conexion.Nueva_Conexion(Base);
        }

        public static List<BAN_Stowage_Plan_Turno> ConsultarLista(long idStowageDet, out string OnError)
        {
            parametros.Clear();
            parametros.Add("i_idStowageDet", idStowageDet);
            return sql_puntero.ExecuteSelectControl<BAN_Stowage_Plan_Turno>(sql_punteroVBS.Conexion_LocalVBS, 8000, "[BAN_Stowage_Plan_Turno_Consultar]", parametros, out OnError);
        }


        public static BAN_Stowage_Plan_Turno GetEntidad(long _id)
        {
            parametros.Clear();
            parametros.Add("i_idStowagePlanTurno", _id);
            var obj = sql_puntero.ExecuteSelectOnly<BAN_Stowage_Plan_Turno>(sql_punteroVBS.Conexion_LocalVBS, 4000, "[BAN_Stowage_Plan_Turno_Consultar]", parametros);
            return obj;
        }

        public Int64? Save_Update(out string OnError)
        {
            parametros.Clear();
            parametros.Add("i_idStowagePlanTurno", this.idStowagePlanTurno);
            parametros.Add("i_idStowageDet", this.idStowageDet);
            parametros.Add("i_fecha", this.fecha);
            parametros.Add("i_idHoraInicio", this.idHoraInicio);
            parametros.Add("i_horaInicio", this.horaInicio);
            parametros.Add("i_idHoraFin", this.idHoraFin);
            parametros.Add("i_horaFin", this.horaFin);
            parametros.Add("i_box", this.box);
            parametros.Add("i_idCapacidadHorafecha", this.idCapacidadHorafecha);
            parametros.Add("i_idCapacidadHoraBodega", this.idCapacidadHoraBodega);
            parametros.Add("i_estado", this.estado);
            parametros.Add("i_usuarioCrea", this.usuarioCrea);
            parametros.Add("i_usuarioModifica", this.usuarioModifica);

            var db = sql_puntero.ExecuteInsertUpdateDeleteReturn(sql_punteroVBS.Conexion_LocalVBS, 6000, "[BAN_Stowage_Plan_Turno_Insertar]", parametros, out OnError);
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
            parametros.Add("i_idStowagePlanTurno", this.idStowagePlanTurno);
            parametros.Add("i_usuarioModifica", this.usuarioModifica);

            var db = sql_puntero.ExecuteInsertUpdateDeleteReturn(sql_punteroVBS.Conexion_LocalVBS, 6000, "[BAN_Stowage_Plan_Turno_Eliminar]", parametros, out OnError);
            if (!db.HasValue || db.Value < 0)
            {
                return null;
            }
            OnError = string.Empty;
            return db.Value;
        }
    }
}
