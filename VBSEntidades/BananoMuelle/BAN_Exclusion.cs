using BillionEntidades;
using SqlConexion;
using System;
using System.Collections.Generic;

namespace VBSEntidades.BananoMuelle
{
    public class BAN_Exclusion : Cls_Bil_Base
    {
        #region "Propiedades"
        public long? id { get; set; }
        public string codigo { get; set; }
        public long? idLoadingCab { get; set; }
        public long? idLoadingDet { get; set; }
        public long? idStowageCab { get; set; }
        public long? idStowageDet { get; set; }
        public long? idStowageTurno { get; set; }
        public int? idMotivo { get; set; }
        public string aisv { get; set; }
        public string comentario { get; set; }
        public bool estado { get; set; }
        public string usuarioCrea { get; set; }
        public DateTime? fechaCreacion { get; set; }
        public string usuarioModifica { get; set; }
        public DateTime? fechaModifica { get; set; }
        #endregion

        public BAN_Exclusion()
        {
            init();
        }

        private static void OnInit(string Base)
        {
            sql_puntero = (sql_puntero == null) ? Cls_Conexion.Conexion() : sql_puntero;
            parametros = new Dictionary<string, object>();
            nueva_conexion = Cls_Conexion.Nueva_Conexion(Base);
        }

        public static List<BAN_Exclusion> ConsultarListadoExclusiones(long idCabecera, out string OnError)
        {
            parametros.Clear();
            parametros.Add("i_idLoadingCab", idCabecera);
            return sql_puntero.ExecuteSelectControl<BAN_Exclusion>(sql_punteroVBS.Conexion_LocalVBS, 8000, "[BAN_Exclusiones_Consultar]", parametros, out OnError);
        }

        public static List<BAN_Exclusion> ConsultarListadoExclusionesST(long idStowageCab, out string OnError)
        {
            parametros.Clear();
            parametros.Add("i_idStowageCab", idStowageCab);
            return sql_puntero.ExecuteSelectControl<BAN_Exclusion>(sql_punteroVBS.Conexion_LocalVBS, 8000, "[BAN_Exclusiones_ConsultarST]", parametros, out OnError);
        }

        public Int64? Save_Autorizacion(out string OnError)
        {
            parametros.Clear();
            parametros.Add("i_codigo", this.codigo);
            parametros.Add("i_idLoadingCab", this.idLoadingCab);
            parametros.Add("i_idLoadingDet", this.idLoadingDet);
            parametros.Add("i_idMotivo", this.idMotivo);
            parametros.Add("i_aisv", this.aisv);
            parametros.Add("i_comentario", this.comentario);
            parametros.Add("i_estado", this.estado);
            parametros.Add("i_usuarioCrea", this.usuarioCrea);

            var db = sql_puntero.ExecuteInsertUpdateDeleteReturn(sql_punteroVBS.Conexion_LocalVBS, 6000, "[BAN_Exclusion_insert]", parametros, out OnError);
            if (!db.HasValue || db.Value < 0)
            {
                return null;
            }
            OnError = string.Empty;
            return db.Value;
        }

        public Int64? Save_AutorizacionST(out string OnError)
        {
            parametros.Clear();
            parametros.Add("i_codigo", this.codigo);
            parametros.Add("i_idStowageCab", idStowageCab);
            parametros.Add("i_idStowageDet", idStowageDet);
            parametros.Add("i_idStowageTurno", idStowageTurno);
            parametros.Add("i_idMotivo", this.idMotivo);
            parametros.Add("i_aisv", this.aisv);
            parametros.Add("i_comentario", this.comentario);
            parametros.Add("i_estado", this.estado);
            parametros.Add("i_usuarioCrea", this.usuarioCrea);

            var db = sql_puntero.ExecuteInsertUpdateDeleteReturn(sql_punteroVBS.Conexion_LocalVBS, 6000, "[BAN_Exclusion_insertST]", parametros, out OnError);
            if (!db.HasValue || db.Value < 0)
            {
                return null;
            }
            OnError = string.Empty;
            return db.Value;
        }

    }



}
