using BillionEntidades;
using SqlConexion;
using System;
using System.Collections.Generic;

namespace VBSEntidades.BananoBodega
{
    public class BAN_Capacidad_Hora_Bodega : Cls_Bil_Base
    {
        #region "Propiedades"
        public long? id { get; set; }
        public long? idCapacidadHorafecha { get; set; }
        public string idNave { get; set; }
        public DateTime? fecha { get; set; }
        public string nave { get; set; }
        public int? idHoraInicio { get; set; }
        public string horaInicio { get; set; }
        public int? idHoraFin { get; set; }
        public string horaFin { get; set; }
        public int? idBodega { get; set; }
        public int? idBloque { get; set; }
        public int? box { get; set; }
        public int? boxExtra { get; set; }
        public int? boxSeleccionado { get; set; }
        public int? reservado { get; set; }
        public int? disponible { get; set; }
        public bool? estado { get; set; }
        public string usuarioCrea { get; set; }
        public DateTime? fechaCreacion { get; set; }
        public string usuarioModifica { get; set; }
        public DateTime? fechaModifica { get; set; }
        public BAN_Catalogo_Bloque oBloque { get; set; }
        #endregion

        public BAN_Capacidad_Hora_Bodega()
        {
            init();
        }

        private static void OnInit(string Base)
        {
            sql_puntero = (sql_puntero == null) ? Cls_Conexion.Conexion() : sql_puntero;
            parametros = new Dictionary<string, object>();
            nueva_conexion = Cls_Conexion.Nueva_Conexion(Base);
        }

        public static List<BAN_Capacidad_Hora_Bodega> ConsultarListadoCapacidadPorNave(string idNave, int bloque, out string OnError)
        {
            parametros.Clear();
            parametros.Add("i_idNave", idNave);
            parametros.Add("i_idBloque", bloque);
            return sql_puntero.ExecuteSelectControl<BAN_Capacidad_Hora_Bodega>(sql_punteroVBS.Conexion_LocalVBS, 8000, "[BAN_Capacidad_Hora_Bodega_Consultar]", parametros, out OnError);
        }

        public static List<BAN_Capacidad_Hora_Bodega> ConsultarConfiguracionCapacidadPorNave(string fecha, int bloque, string idNave, out string OnError)
        {
            parametros.Clear();
            parametros.Add("i_fecha", fecha);
            parametros.Add("i_idBloque", bloque);
            parametros.Add("i_idNave", idNave);
            return sql_puntero.ExecuteSelectControl<BAN_Capacidad_Hora_Bodega>(sql_punteroVBS.Conexion_LocalVBS, 8000, "[BAN_Capacidad_Hora_Bodega_Disponible]", parametros, out OnError);
        }

        public static BAN_Capacidad_Hora_Bodega GetCapacidadHoraEspecifico(long _idCapacidad_Horafecha, long _idCapacidad_Hora_Bodega)
        {
            parametros.Clear();
            parametros.Add("i_idCapacidad_Horafecha", _idCapacidad_Horafecha);
            parametros.Add("i_idCapacidad_Hora_Bodega", _idCapacidad_Hora_Bodega);
            var obj = sql_puntero.ExecuteSelectOnly<BAN_Capacidad_Hora_Bodega>(sql_punteroVBS.Conexion_LocalVBS, 4000, "[BAN_Capacidad_Hora_Bodega_ConsultarEsp]", parametros);

            return obj;
        }

        public Int64? Save_Update(out string OnError)
        {
            parametros.Clear();
            parametros.Add("i_id", this.id);
            parametros.Add("i_idNave", this.idNave);
            parametros.Add("i_nave", this.nave);
            parametros.Add("i_idHoraInicio", this.idHoraInicio);
            parametros.Add("i_horaInicio", this.horaInicio);
            parametros.Add("i_idHoraFin", this.idHoraFin);
            parametros.Add("i_horaFin", this.horaFin);
            parametros.Add("i_idBodega", this.idBodega);
            parametros.Add("i_idBloque", this.idBloque);
            parametros.Add("i_box", this.box);
            parametros.Add("i_boxExtra", this.boxExtra);
            parametros.Add("i_reservado", this.reservado);
            parametros.Add("i_disponible", this.disponible);
            parametros.Add("i_usuarioCrea", this.usuarioCrea);
            parametros.Add("i_usuarioModifica", this.usuarioModifica);
            parametros.Add("i_estado", this.estado);

            var db = sql_puntero.ExecuteInsertUpdateDeleteReturn(sql_punteroVBS.Conexion_LocalVBS, 6000, "[BAN_Capacidad_Hora_Bodega_Insertar]", parametros, out OnError);
            if (!db.HasValue || db.Value < 0)
            {
                return null;
            }
            OnError = string.Empty;
            return db.Value;
        }

        public static List<BAN_Capacidad_Hora_Bodega> Save_Update(string xmlDatos, out string OnError)
        {
            parametros.Clear();
            parametros.Add("i_Datos", xmlDatos);
            return sql_puntero.ExecuteSelectControl<BAN_Capacidad_Hora_Bodega>(sql_punteroVBS.Conexion_LocalVBS, 8000, "[]", parametros, out OnError);
        }
    }

    public class BAN_Capacidad_Hora_BodegaFecha : Cls_Bil_Base
    {
        public long? id { get; set; }
        public long idCapacidadHorafecha { get; set; }
        public long idCapacidadHoraBodega { get; set; }
        public int? reservado { get; set; }

        public BAN_Capacidad_Hora_BodegaFecha()
        {
            init();
        }

        private static void OnInit(string Base)
        {
            sql_puntero = (sql_puntero == null) ? Cls_Conexion.Conexion() : sql_puntero;
            parametros = new Dictionary<string, object>();
            nueva_conexion = Cls_Conexion.Nueva_Conexion(Base);
        }

        public Int64? Save_Update(out string OnError)
        {
            parametros.Clear();
            parametros.Add("i_idCapacidadHorafecha", this.idCapacidadHorafecha);
            parametros.Add("i_idCapacidadHoraBodega", this.idCapacidadHoraBodega);
            parametros.Add("i_reservado", this.reservado);

            var db = sql_puntero.ExecuteInsertUpdateDeleteReturn(sql_punteroVBS.Conexion_LocalVBS, 6000, "[BAN_Capacidad_Hora_BodegaFecha_Insertar]", parametros, out OnError);
            if (!db.HasValue || db.Value < 0)
            {
                return null;
            }
            OnError = string.Empty;
            return db.Value;
        }
    }
}

