using BillionEntidades;
using SqlConexion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VBSEntidades.BananoMuelle
{
    public class BAN_Loading_Program_Det : Cls_Bil_Base
    {
        #region "Propiedades"
        public BAN_Loading_Program_Cab Cabecera { get; set; }
        public long? idLoadingDet { get; set; }
        public long? idLoadingCab { get; set; }
        public DateTime fecha { get; set; }
        public BAN_HorarioInicial oHoraInicio { get; set; }
        public int idHoraInicio { get; set; }
        public string horaInicio { get; set; }
        public BAN_HorarioFinal oHoraFin { get; set; }
        public int idHoraFin { get; set; }
        public string horaFin { get; set; }
        public BAN_Catalogo_Hold oHold { get; set; }
        public int idHold { get; set; }
        public BAN_Catalogo_Piso oPiso { get; set; }
        public int idPiso { get; set; }
        public int box { get; set; }
        public BAN_Catalogo_Cargo oCargo { get; set; }
        public int idCargo { get; set; }
        public BAN_Catalogo_Exportador oExportador { get; set; }
        public int idExportador { get; set; }
        public BAN_Catalogo_Marca oMarca { get; set; }
        public int idMarca { get; set; }
        public BAN_Catalogo_Consignatario oConsignatario { get; set; }
        public int idConsignatario { get; set; }
        public string comentario { get; set; }
        public string aisv { get; set; }
        public DateTime? aisvFechaCrea { get; set; }
        public string aisvUsuarioCrea { get; set; }
        public bool estado { get; set; }
        public int fechaDocumento { get; set; }
        public string usuarioCrea { get; set; }
        public DateTime fechaCreacion { get; set; }
        public string usuarioModifica { get; set; }
        public DateTime? fechaModifica { get; set; }
        public List<BAN_Loading_Program_Aisv> ListaAISV { get; set; }
        #endregion

        public BAN_Loading_Program_Det()
        {
            init();
        }

        private static void OnInit(string Base)
        {
            sql_puntero = (sql_puntero == null) ? Cls_Conexion.Conexion() : sql_puntero;
            parametros = new Dictionary<string, object>();
            nueva_conexion = Cls_Conexion.Nueva_Conexion(Base);
        }

        public static List<BAN_Loading_Program_Det> ConsultarListadoDetalleLoading(long idCabecera, out string OnError)
        {
            parametros.Clear();
            parametros.Add("i_idLoadingCab", idCabecera);
            return sql_puntero.ExecuteSelectControl<BAN_Loading_Program_Det>(sql_punteroVBS.Conexion_LocalVBS, 8000, "[BAN_Loading_Program_Det_Consultar]", parametros, out OnError);
        }

        public static BAN_Loading_Program_Det GetDetalleEspecifico(long _id)
        {
            parametros.Clear();
            parametros.Add("i_idLoadingDet", _id);
            var obj = sql_puntero.ExecuteSelectOnly<BAN_Loading_Program_Det>(sql_punteroVBS.Conexion_LocalVBS, 4000, "[BAN_Loading_Program_Det_Consultar]", parametros);

            return obj;
        }

        public Int64? Save_Update(out string OnError)
        {
            parametros.Clear();
            parametros.Add("i_idLoadingDet", this.idLoadingDet);
            parametros.Add("i_idLoadingCab", this.idLoadingCab);
            parametros.Add("i_fecha", this.fecha);
            parametros.Add("i_idHoraInicio", this.idHoraInicio);
            parametros.Add("i_horaInicio", this.horaInicio);
            parametros.Add("i_idHoraFin", this.idHoraFin);
            parametros.Add("i_horaFin", this.horaFin);
            parametros.Add("i_idHold", this.idHold);
            parametros.Add("i_idPiso", this.idPiso);
            parametros.Add("i_box", this.box);
            parametros.Add("i_idCargo", this.idCargo);
            parametros.Add("i_idExportador", this.idExportador);
            parametros.Add("i_idMarca", this.idMarca);
            parametros.Add("i_idConsignatario", this.idConsignatario);
            parametros.Add("i_comentario", this.comentario);
            parametros.Add("i_fechaDocumento", this.fechaDocumento);
            parametros.Add("i_usuarioCrea", this.usuarioCrea);
            parametros.Add("i_usuarioModifica", this.usuarioModifica);
            parametros.Add("i_estado", this.estado);

            var db = sql_puntero.ExecuteInsertUpdateDeleteReturn(sql_punteroVBS.Conexion_LocalVBS, 6000, "[BAN_Loading_Program_Det_Insertar]", parametros, out OnError);
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
            parametros.Add("i_idLoadingDet", this.idLoadingDet);
            parametros.Add("i_usuarioAnulacion", this.usuarioModifica);

            var db = sql_puntero.ExecuteInsertUpdateDeleteReturn(sql_punteroVBS.Conexion_LocalVBS, 6000, "[BAN_Loading_Program_Det_Eliminar]", parametros, out OnError);
            if (!db.HasValue || db.Value < 0)
            {
                return null;
            }
            OnError = string.Empty;
            return db.Value;
        }
    }

}
