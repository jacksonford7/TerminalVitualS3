using BillionEntidades;
using SqlConexion;
using System;
using System.Collections.Generic;
using VBSEntidades.BananoMuelle;

namespace VBSEntidades.BananoBodega
{
    public class BAN_Stowage_Movimiento : Cls_Bil_Base
    {
        #region "Propiedades"
        public long? idMovimiento { get; set; }
        public long idStowageCab { get; set; }
        public long idStowageDet { get; set; }
        public long idStowagePlanTurno { get; set; }
        public long idStowageAisv { get; set; }
        public int? idUbicacion { get; set; }
        public int? fechaProceso { get; set; }
        public int? anio { get; set; }
        public int? mes { get; set; }
        public int? dia { get; set; }
        public string barcode { get; set; }
        public int idModalidad { get; set; }
        public string tipo { get; set; }
        public int cantidad { get; set; }
        public string observacion  { get; set; }
        public string estado { get; set; }
        public bool active { get; set; }
        public long idOrdenDespacho { get; set; }
        public bool isOrdenActive { get; set; }
        public string usuarioCrea { get; set; }
        public DateTime? fechaCreacion { get; set; }
        public string usuarioModifica { get; set; }
        public DateTime? fechaModifica { get; set; }
        public string usuarioPreDespacho { get; set; }
        public DateTime? fechaPreDespacho { get; set; }
        public string usuarioDespacho { get; set; }
        public DateTime? fechaDespacho { get; set; }
        public BAN_Stowage_Plan_Cab oStowageCab { get; set; }
        public BAN_Stowage_Plan_Det oStowageDet { get; set; }
        public BAN_Stowage_Plan_Turno oStowageTurno { get; set; }
        public BAN_Catalogo_Ubicacion oUbicacion { get; set; }
        public BAN_Stowage_Plan_Aisv oStowage_Plan_Aisv { get; set; }
        public BAN_Catalogo_Estado oEstado { get; set; }
        public BAN_Catalogo_Modalidad oModalidad { get; set; }

        public List<fotoRecepcionVBS> Fotos { get; set; }

        public BAN_Catalogo_Bloque oBloque { get; set; }
        public BAN_Catalogo_Exportador oExportador { get; set; }
        public int palets { get; set; }
        public int idExportador { get; set; }
        public string idNave { get; set; }
        public string booking { get; set; }
        public bool isMix { get; set; }
        public string referencia { get; set; }
        public long roleoReferencia { get; set; }

        #endregion

        public BAN_Stowage_Movimiento()
        {
            init();
        }

        private static void OnInit(string Base)
        {
            sql_puntero = (sql_puntero == null) ? Cls_Conexion.Conexion() : sql_puntero;
            parametros = new Dictionary<string, object>();
            nueva_conexion = Cls_Conexion.Nueva_Conexion(Base);
        }

        public static List<BAN_Stowage_Movimiento> ConsultarLista(string idNave, int idLinea, out string OnError)
        {
            parametros.Clear();
            parametros.Add("i_idNave", idNave);
            parametros.Add("i_idLinea", idLinea);
            return sql_puntero.ExecuteSelectControl<BAN_Stowage_Movimiento>(sql_punteroVBS.Conexion_LocalVBS, 8000, "[BAN_Stowage_Movimiento_ConsultaGen]", parametros, out OnError);
        }

        public static System.Data.DataTable ConsultarListaReporte(string idNave, int idLinea, out string OnError)
        {
            parametros.Clear();
            parametros.Add("i_idNave", idNave);
            parametros.Add("i_idLinea", idLinea);
            return sql_puntero.ComadoSelectADatatable(sql_punteroVBS.Conexion_LocalVBS, 6000, "BAN_Stowage_Movimiento_ConsultaExcel", parametros, out OnError);
        }

        public static List<BAN_Stowage_Movimiento> ConsultarLista(long i_idStowageAisv, out string OnError)
        {
            parametros.Clear();
            parametros.Add("i_idStowageAisv", i_idStowageAisv);
            return sql_puntero.ExecuteSelectControl<BAN_Stowage_Movimiento>(sql_punteroVBS.Conexion_LocalVBS, 8000, "[BAN_Stowage_Movimiento_Consultar]", parametros, out OnError);
        }
        public static BAN_Stowage_Movimiento GetEntidad(long _id)
        {
            parametros.Clear();
            parametros.Add("i_idMovimiento", _id);
            var obj = sql_puntero.ExecuteSelectOnly<BAN_Stowage_Movimiento>(sql_punteroVBS.Conexion_LocalVBS, 4000, "[BAN_Stowage_Movimiento_ConsultaGen]", parametros);
            return obj;
        }
        public static List<BAN_Stowage_Movimiento> ConsultarMovimientosXOrdenDespacho(long idOrdenDespacho, out string OnError)
        {
            parametros.Clear();
            parametros.Add("i_idOrdenDespacho", idOrdenDespacho);
            return sql_puntero.ExecuteSelectControl<BAN_Stowage_Movimiento>(sql_punteroVBS.Conexion_LocalVBS, 8000, "[BAN_Stowage_MovimientoPorOrden_ConsultarTV]", parametros, out OnError);
        }

        public static bool Save_Roleo(string nave, string naveName, string booking, string xml, string usuario, out string OnError)
        {
            parametros.Clear();
            parametros.Add("i_nave", nave);
            parametros.Add("i_naveName", naveName);
            parametros.Add("i_booking", booking);
            parametros.Add("i_xmlData", xml);
            parametros.Add("i_usuarioCrea", usuario);

            var db = sql_puntero.ExecuteInsertUpdateDeleteReturn(sql_punteroVBS.Conexion_LocalVBS, 6000, "[BAN_Roleo_Procesar]", parametros, out OnError);
            if (!db.HasValue || db.Value < 0)
            {
                return false;
            }
            else
            {
                OnError = string.Empty;
                return true;
            }
        }
    }

    [Serializable]
    public class fotoRecepcionVBS : Cls_Bil_Base
    {
        #region "Propiedades"                 
        public long? id { get; set; }
        public long idMovimiento { get; set; }
        public BAN_Stowage_Movimiento Movimiento { get; set; }
        public byte[] foto { get; set; }
        public string ruta { get; set; }
        public string estado { get; set; }
        public string usuarioCrea { get; set; }
        public DateTime? fechaCreacion { get; set; }
        public string usuarioModifica { get; set; }
        public DateTime? fechaModifica { get; set; }
        #endregion

    }
}

