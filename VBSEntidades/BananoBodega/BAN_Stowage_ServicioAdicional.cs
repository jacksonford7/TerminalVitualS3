using BillionEntidades;
using SqlConexion;
using System;
using System.Collections.Generic;
using VBSEntidades.BananoMuelle;

namespace VBSEntidades.BananoBodega
{
    public class BAN_Stowage_ServicioAdicional : Cls_Bil_Base
    {
        #region "Propiedades"
        public long? idliquidacion { get; set; }
        public long idStowageDet { get; set; }
        public BAN_Stowage_Plan_Det oStowageDet { get; set; }
        public string aisv { get; set; }
        public decimal? cantidad { get; set; }
        public string idExportador { get; set; }
        public BAN_Catalogo_Exportador oExportador { get; set; }
        public int idServicio { get; set; }
        public BAN_Catalogo_Servicio oServicio { get; set; }
        public string comentario { get; set; }
        public bool estado { get; set; }
        public string usuarioCrea { get; set; }
        public DateTime? fechaCreacion { get; set; }
        public string usuarioModifica { get; set; }
        public DateTime? fechaModifica { get; set; }
        public Int64 BRBK_CONSECUTIVO { get; set; }
        #endregion


        public BAN_Stowage_ServicioAdicional() : base()
        {
            init();
        }
        private static void OnInit(string Base)
        {
            sql_puntero = (sql_puntero == null) ? Cls_Conexion.Conexion() : sql_puntero;
            parametros = new Dictionary<string, object>();
            nueva_conexion = Cls_Conexion.Nueva_Conexion(Base);
        }

        public static List<BAN_Stowage_ServicioAdicional> Consultar(long _idStowageDet, out string OnError)
        {
            parametros.Clear();
            parametros.Add("i_idStowageDet", _idStowageDet);
            return sql_puntero.ExecuteSelectControl<BAN_Stowage_ServicioAdicional>(sql_punteroVBS.Conexion_LocalVBS, 4000, "BAN_Stowage_ServicioAdicional_Consultar", parametros, out OnError);
        }

        public static BAN_Stowage_ServicioAdicional GetEntidad(long _id)
        {
            parametros.Clear();
            parametros.Add("i_idliquidacion", _id);
            var obj = sql_puntero.ExecuteSelectOnly<BAN_Stowage_ServicioAdicional>(sql_punteroVBS.Conexion_LocalVBS, 4000, "BAN_Stowage_ServicioAdicional_Consultar", parametros);
            try
            {
                obj.oServicio = BAN_Catalogo_Servicio.GetEntidad(obj.idServicio);
                obj.oStowageDet = BAN_Stowage_Plan_Det.GetEntidad(obj.idStowageDet);
            }
            catch { }
            return obj;
        }


      
        public Int64? Save_Update(out string OnError)
        {
            parametros.Clear();
            parametros.Add("i_idliquidacion", this.idliquidacion);
            parametros.Add("i_idStowageDet", this.oStowageDet.idStowageDet);
            parametros.Add("i_aisv", this.aisv);
            parametros.Add("i_cantidad", this.cantidad);
            parametros.Add("i_idExportador", this.oStowageDet.idExportador);
            parametros.Add("i_idServicio", this.idServicio);
            parametros.Add("i_comentario", this.comentario);
            parametros.Add("i_estado", this.estado);
            parametros.Add("i_usuarioCrea", this.usuarioCrea);
            parametros.Add("i_usuarioModifica", this.usuarioModifica);

            var db = sql_puntero.ExecuteInsertUpdateDeleteReturn(sql_punteroVBS.Conexion_LocalVBS, 6000, "BAN_Stowage_ServicioAdicional_insertar", parametros, out OnError);
            if (!db.HasValue || db.Value < 0)
            {
                return null;
            }
            OnError = string.Empty;
            return db.Value;
        }
    }

  
}