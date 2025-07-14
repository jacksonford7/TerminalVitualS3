using BillionEntidades;
using SqlConexion;
using System;
using System.Collections.Generic;

namespace VBSEntidades.BananoBodega
{
    public class BAN_Capacidad_Hora_Fecha : Cls_Bil_Base
    {
        #region "Propiedades"
        public long? id { get; set; }
        public string idNave { get; set; }
        public string nave { get; set; }
        public DateTime fecha { get; set; }
        public int? idHoraInicio { get; set; }
        public string horaInicio { get; set; }
        public int? idHoraFin { get; set; }
        public string horaFin { get; set; }
        public bool? estado { get; set; }
        public string usuarioCrea { get; set; }
        public DateTime? fechaCreacion { get; set; }
        public string usuarioModifica { get; set; }
        public DateTime? fechaModifica { get; set; }
        #endregion

        public BAN_Capacidad_Hora_Fecha()
        {
            init();
        }

        private static void OnInit(string Base)
        {
            sql_puntero = (sql_puntero == null) ? Cls_Conexion.Conexion() : sql_puntero;
            parametros = new Dictionary<string, object>();
            nueva_conexion = Cls_Conexion.Nueva_Conexion(Base);
        }

        public static List<BAN_Capacidad_Hora_Fecha> ConsultarLista(string idNave, out string OnError)
        {
            parametros.Clear();
            parametros.Add("i_idNave", idNave);
            return sql_puntero.ExecuteSelectControl<BAN_Capacidad_Hora_Fecha>(sql_punteroVBS.Conexion_LocalVBS, 8000, "[BAN_Capacidad_HoraFecha_Consultar]", parametros, out OnError);
        }

        public Int64? Save_Update(out string OnError)
        {
            parametros.Clear();
            parametros.Add("i_id", this.id);
            parametros.Add("i_idNave", this.idNave);
            parametros.Add("i_nave", this.nave);
            parametros.Add("i_fecha", this.fecha);
            parametros.Add("i_idHoraInicio", this.idHoraInicio);
            parametros.Add("i_horaInicio", this.horaInicio);
            parametros.Add("i_idHoraFin", this.idHoraFin);
            parametros.Add("i_horaFin", this.horaFin);
            parametros.Add("i_usuarioCrea", this.usuarioCrea);
            parametros.Add("i_usuarioModifica", this.usuarioModifica);
            parametros.Add("i_estado", this.estado);

            var db = sql_puntero.ExecuteInsertUpdateDeleteReturn(sql_punteroVBS.Conexion_LocalVBS, 6000, "[BAN_Capacidad_HoraFecha_Insertar]", parametros, out OnError);
            if (!db.HasValue || db.Value < 0)
            {
                return null;
            }
            OnError = string.Empty;
            return db.Value;
        }

        public static List<BAN_Capacidad_Hora_Fecha> Save_Update(string xmlDatos, out string OnError)
        {
            parametros.Clear();
            parametros.Add("i_Datos", xmlDatos);
            return sql_puntero.ExecuteSelectControl<BAN_Capacidad_Hora_Fecha>(sql_punteroVBS.Conexion_LocalVBS, 8000, "[BAN_Capacidad_HoraFecha_Insertar]", parametros, out OnError);
        }
    }
}

