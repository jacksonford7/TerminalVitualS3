using BillionEntidades;
using SqlConexion;
using System;
using System.Collections.Generic;
using VBSEntidades.BananoMuelle;

namespace VBSEntidades.BananoBodega
{
    public class BAN_Stowage_Plan_Det : Cls_Bil_Base
    {
        #region "Propiedades"
        public long? idStowageDet { get; set; }
        public long? idStowageCab { get; set; }
        public int idHold { get; set; }
        public string piso { get; set; }
        public int? boxSolicitado { get; set; }
        public int idCargo { get; set; }
        public int idExportador { get; set; }
        public int idMarca { get; set; }
        public int idConsignatario { get; set; }
        public int? idBodega { get; set; }
        public int? idBloque { get; set; }
        public int? boxAutorizado { get; set; }
        public int? reservado { get; set; }
        public int? disponible { get; set; }
        public string comentario { get; set; }
        public int? fechaDocumento { get; set; }
        public string estado { get; set; }
        public string usuarioCrea { get; set; }
        public DateTime? fechaCreacion { get; set; }
        public string usuarioModifica { get; set; }
        public DateTime? fechaModifica { get; set; }
        public string usuarioAnulacion { get; set; }
        public DateTime? fechaAnulacion { get; set; }
        public BAN_Catalogo_Bodega oBodega { get; set; }
        public BAN_Catalogo_Bloque oBloque { get; set; }
        public BAN_Catalogo_Hold oHold { get; set; }
        public BAN_Catalogo_Cargo oCargo { get; set; }
        public BAN_Catalogo_Exportador oExportador { get; set; }
        public BAN_Catalogo_Marca oMarca { get; set; }
        public BAN_Catalogo_Consignatario oConsignatario { get; set; }
        public BAN_Stowage_Plan_Cab oStowage_Plan_Cab { get; set; }
        public List<BAN_Stowage_Plan_Turno> ListaTurno { get; set; }
        public List<BAN_Stowage_Plan_Aisv> ListaAISV { get; set; }
        public BAN_Catalogo_Estado oEstado { get; set; }
        #endregion

        public BAN_Stowage_Plan_Det()
        {
            init();
        }

        private static void OnInit(string Base)
        {
            sql_puntero = (sql_puntero == null) ? Cls_Conexion.Conexion() : sql_puntero;
            parametros = new Dictionary<string, object>();
            nueva_conexion = Cls_Conexion.Nueva_Conexion(Base);
        }

        public static List<BAN_Stowage_Plan_Det> ConsultarLista(long _idCab ,out string OnError)
        {
            parametros.Clear();
            parametros.Add("i_idStowageCab", _idCab);
            return sql_puntero.ExecuteSelectControl<BAN_Stowage_Plan_Det>(sql_punteroVBS.Conexion_LocalVBS, 8000, "[BAN_Stowage_Plan_Det_Consultar]", parametros, out OnError);
        }
        public static List<BAN_Stowage_Plan_Det> ConsultarLista(out string OnError)
        {
            parametros.Clear();
            return sql_puntero.ExecuteSelectControl<BAN_Stowage_Plan_Det>(sql_punteroVBS.Conexion_LocalVBS, 8000, "[BAN_Stowage_Plan_Det_Consultar]", parametros, out OnError);
        }

        public static List<BAN_Stowage_Plan_Det> ConsultarLista(string _idNave, string _ruc, out string OnError)
        {
            parametros.Clear();
            parametros.Add("i_idNave", _idNave);
            parametros.Add("i_ruc", _ruc);
            return sql_puntero.ExecuteSelectControl<BAN_Stowage_Plan_Det>(sql_punteroVBS.Conexion_LocalVBS, 8000, "[BAN_Stowage_Plan_Det_Turnos]", parametros, out OnError);
        }

        public static BAN_Stowage_Plan_Det GetEntidad(long _id)
        {
            parametros.Clear();
            parametros.Add("i_id", _id);
            var obj = sql_puntero.ExecuteSelectOnly<BAN_Stowage_Plan_Det>(sql_punteroVBS.Conexion_LocalVBS, 4000, "[BAN_Stowage_Plan_Det_Consultar]", parametros);
            return obj;
        }

        public Int64? Save_Update(out string OnError)
        {
            parametros.Clear();
            parametros.Add("i_idStowageDet", this.idStowageDet);
            parametros.Add("i_idStowageCab", this.idStowageCab);
            parametros.Add("i_idHold", this.idHold);
            parametros.Add("i_piso", this.piso);
            parametros.Add("i_boxSolicitado", this.boxSolicitado);
            parametros.Add("i_idCargo", this.idCargo);
            parametros.Add("i_idExportador", this.idExportador);
            parametros.Add("i_idMarca", this.idMarca);
            parametros.Add("i_idConsignatario", this.idConsignatario);
            parametros.Add("i_idBodega", this.idBodega);
            parametros.Add("i_idBloque", this.idBloque);
            parametros.Add("i_boxAutorizado", this.boxAutorizado);
            parametros.Add("i_reservado", this.reservado);
            parametros.Add("i_disponible", this.disponible);
            parametros.Add("i_comentario", this.comentario);
            parametros.Add("i_fechaDocumento", this.fechaDocumento);
            parametros.Add("i_estado", this.estado);
            parametros.Add("i_usuarioCrea", this.usuarioCrea);
            parametros.Add("i_usuarioModifica", this.usuarioModifica);

            var db = sql_puntero.ExecuteInsertUpdateDeleteReturn(sql_punteroVBS.Conexion_LocalVBS, 6000, "[BAN_Stowage_Plan_Det_Insertar]", parametros, out OnError);
            if (!db.HasValue || db.Value < 0)
            {
                return null;
            }
            OnError = string.Empty;
            return db.Value;
        }

        public Int64? Save_Anulacion(out string OnError)
        {
            parametros.Clear();
            parametros.Add("i_idStowageDet", this.idStowageDet);
            parametros.Add("i_usuarioAnulacion", this.usuarioModifica);

            var db = sql_puntero.ExecuteInsertUpdateDeleteReturn(sql_punteroVBS.Conexion_LocalVBS, 6000, "[BAN_Stowage_Plan_Det_Eliminar]", parametros, out OnError);
            if (!db.HasValue || db.Value < 0)
            {
                return null;
            }
            OnError = string.Empty;
            return db.Value;
        }


        public static bool? VerificaSiExisteAgente(string _numBooking, out string OnError)
        {
            parametros.Clear();
            parametros.Add("i_booking", _numBooking);
            return sql_puntero.ExecuteSelectOnlyBool(sql_punteroVBS.Conexion_LocalVBS, 6000, "BAN_ValidaContenedorAcopio", parametros, out OnError);

        }
    }
}
