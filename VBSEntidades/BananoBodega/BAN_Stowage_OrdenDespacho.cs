using BillionEntidades;
using SqlConexion;
using System;
using System.Collections.Generic;
using VBSEntidades.BananoMuelle;

namespace VBSEntidades.BananoBodega
{
    public class BAN_Stowage_OrdenDespacho  : Cls_Bil_Base
    {
        #region "Propiedades"
        public long? idOrdenDespacho { get; set; }
        public string idNave { get; set; }
        public int idExportador { get; set; }
        public int idBodega { get; set; }
        public int idBloque { get; set; }
        public int cantidadPalets { get; set; }
        public int cantidadBox { get; set; }
        public int arrastre { get; set; }
        public int pendiente { get; set; }
        public string estado { get; set; }
        public string usuarioCrea { get; set; }
        public DateTime fechaCreacion { get; set; }
        public string usuarioModifica { get; set; }
        public DateTime? fechaModifica { get; set; }
        public BAN_Catalogo_Estado oEstado { get; set; }
        public BAN_Catalogo_Exportador oExportador { get; set; }
        public BAN_Catalogo_Bodega oBodega { get; set; }
        public BAN_Catalogo_Bloque oBloque { get; set; }
        public BAN_Stowage_Movimiento oMovimiento { get; set; }
        public string booking { get; set; }

        public int filaDesde { get; set; }
        public int FilaHasta { get; set; }
        public int idAltura { get; set; }
        public int idMarca { get; set; }
        #endregion
        public BAN_Stowage_OrdenDespacho()
        {
            init();
        }

        private static void OnInit(string Base)
        {
            sql_puntero = (sql_puntero == null) ? Cls_Conexion.Conexion() : sql_puntero;
            parametros = new Dictionary<string, object>();
            nueva_conexion = Cls_Conexion.Nueva_Conexion(Base);
        }

        public static List<BAN_Stowage_OrdenDespacho> ConsultarOrdenDespacho(string desde, string hasta, long? idOrdenDespacho, out string OnError)
        {
            parametros.Clear();
            parametros.Add("i_fechaDesde", desde);
            parametros.Add("i_fechaHasta", hasta);
            parametros.Add("i_idOrdenDespacho", idOrdenDespacho);
            return sql_puntero.ExecuteSelectControl<BAN_Stowage_OrdenDespacho>(sql_punteroVBS.Conexion_LocalVBS, 8000, "[BAN_Stowage_OrdenDespacho_Buscar]", parametros, out OnError);
        }
        public static BAN_Stowage_OrdenDespacho GetEntidad(long _id)
        {
            parametros.Clear();
            parametros.Add("i_idOrdenDespacho", _id);
            var obj = sql_puntero.ExecuteSelectOnly<BAN_Stowage_OrdenDespacho>(sql_punteroVBS.Conexion_LocalVBS, 4000, "[BAN_Stowage_OrdenDespacho_Buscar]", parametros);
            return obj;
        }
        public Int64? Save_Update(BAN_Stowage_OrdenDespacho oEntidad, out string OnError)
        {
            using (var scope = new System.Transactions.TransactionScope())
            {
                parametros.Clear();
                parametros.Add("i_id", oEntidad.idOrdenDespacho);
                parametros.Add("i_idNave", oEntidad.idNave);
                parametros.Add("i_idExportador", oEntidad.idExportador);
                parametros.Add("i_idBodega", oEntidad.idBodega);
                parametros.Add("i_idBloque", oEntidad.idBloque);
                parametros.Add("i_cantidadPalets", oEntidad.cantidadPalets);
                parametros.Add("i_cantidadBox", oEntidad.cantidadBox);
                parametros.Add("i_arrastre", oEntidad.arrastre);
                parametros.Add("i_pendiente", oEntidad.pendiente);
                parametros.Add("i_estado", oEntidad.estado);
                parametros.Add("i_usuarioCrea", oEntidad.usuarioCrea);
                parametros.Add("i_usuarioModifica", oEntidad.usuarioModifica);
                parametros.Add("i_filaDesde", oEntidad.filaDesde);
                parametros.Add("i_filaHasta", oEntidad.FilaHasta);
                parametros.Add("i_altura", oEntidad.idAltura);
                parametros.Add("i_marca", oEntidad.idMarca);

                var db = sql_puntero.ExecuteInsertUpdateDeleteReturn(sql_punteroVBS.Conexion_LocalVBS, 6000, "[BAN_Stowage_OrdenDespacho_InsertarTV]", parametros, out OnError);
                if (!db.HasValue || db.Value < 0)
                {
                    return null;
                }
                OnError = string.Empty;


                scope.Complete();
                return db.Value;
            }
        }
    }
}
