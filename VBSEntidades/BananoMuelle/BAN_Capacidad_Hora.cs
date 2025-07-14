using BillionEntidades;
using SqlConexion;
using System;
using System.Collections.Generic;


namespace VBSEntidades.BananoMuelle
{
    public class BAN_Capacidad_Hora : Cls_Bil_Base
    {
        #region "Propiedades"
        public long? id { get; set; }
        public string idNave { get; set; }
        public string nave { get; set; }
        public int idHold { get; set; }
        public int box { get; set; }
        public bool estado { get; set; }
        public string usuarioCrea { get; set; }
        public DateTime fechaCreacion { get; set; }
        public string usuarioModifica { get; set; }
        public DateTime? fechaModifica { get; set; }
        public BAN_Catalogo_Hold oHold { get; set; }
        #endregion

        public BAN_Capacidad_Hora()
        {
            init();
        }

        private static void OnInit(string Base)
        {
            sql_puntero = (sql_puntero == null) ? Cls_Conexion.Conexion() : sql_puntero;
            parametros = new Dictionary<string, object>();
            nueva_conexion = Cls_Conexion.Nueva_Conexion(Base);
        }

        public static List<BAN_Capacidad_Hora> ConsultarListadoCapacidadPorNave(string idNave, out string OnError)
        {
            parametros.Clear();
            parametros.Add("i_idNave", idNave);
            return sql_puntero.ExecuteSelectControl<BAN_Capacidad_Hora>(sql_punteroVBS.Conexion_LocalVBS, 8000, "[BAN_Capacidad_Hora_Consultar]", parametros, out OnError);
        }

        public static List<BAN_Capacidad_Hora> ConsultarConfiguracionCapacidadPorNave(string idNave, out string OnError)
        {
            parametros.Clear();
            parametros.Add("i_idNave", idNave);
            return sql_puntero.ExecuteSelectControl<BAN_Capacidad_Hora>(sql_punteroVBS.Conexion_LocalVBS, 8000, "[BAN_Capacidad_Hora_Get]", parametros, out OnError);
        }

        public static BAN_Capacidad_Hora GetCapacidadHoraEspecifico(long _id)
        {
            parametros.Clear();
            parametros.Add("i_id", _id);
            var obj = sql_puntero.ExecuteSelectOnly<BAN_Capacidad_Hora>(sql_punteroVBS.Conexion_LocalVBS, 4000, "[BAN_Capacidad_Hora_Consultar]", parametros);

            return obj;
        }

        public Int64? Save_Update(out string OnError)
        {
            parametros.Clear();
            parametros.Add("i_id", this.id);
            parametros.Add("i_idNave", this.idNave);
            parametros.Add("i_nave", this.nave);
            parametros.Add("i_idHold", this.idHold);
            parametros.Add("i_box", this.box);
            parametros.Add("i_usuarioCrea", this.usuarioCrea);
            parametros.Add("i_usuarioModifica", this.usuarioModifica);
            parametros.Add("i_estado", this.estado);

            var db = sql_puntero.ExecuteInsertUpdateDeleteReturn(sql_punteroVBS.Conexion_LocalVBS, 6000, "[BAN_Capacidad_Hora_Insertar]", parametros, out OnError);
            if (!db.HasValue || db.Value < 0)
            {
                return null;
            }
            OnError = string.Empty;
            return db.Value;
        }

        public static List<BAN_Capacidad_Hora> Save_Update(string xmlDatos, out string OnError)
        {
            parametros.Clear();
            parametros.Add("i_Datos", xmlDatos);
            return sql_puntero.ExecuteSelectControl<BAN_Capacidad_Hora>(sql_punteroVBS.Conexion_LocalVBS, 8000, "[BAN_Capacidad_Hora_Consultar]", parametros, out OnError);
        }
    }
}
