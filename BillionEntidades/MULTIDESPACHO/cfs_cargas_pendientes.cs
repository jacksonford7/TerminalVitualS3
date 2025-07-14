using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlConexion;

namespace BillionEntidades
{
    public class cfs_cargas_pendientes : Cls_Bil_Base
    {

        #region "Variables"
        private static Int64? lm = -3;
       

        private string _mrn = string.Empty;
        private string _msn = string.Empty;
        private string _hsn = string.Empty;
        private string _cntr = string.Empty;
        private string _importador_id = string.Empty;
        private string _importador_name = string.Empty;
        private DateTime _descarga;
        private string _descripcion = string.Empty;
        private int _total_partida = 1;
        private decimal _volumen = 0;
        private decimal _peso = 0;
        private bool _visto =false;

        private string _USUARIO_CREA = string.Empty;
        private string _USUARIO_MOD = string.Empty;
        private string _numero_carga = string.Empty;

        private string _xmlCabecera;
        private string _xmlDetalle;

        #endregion

        #region "Propiedades"

        public string mrn { get => _mrn; set => _mrn = value; }
        public string msn { get => _msn; set => _msn = value; }
        public string hsn { get => _hsn; set => _hsn = value; }
        public string cntr { get => _cntr; set => _cntr = value; }
        public string importador_id { get => _importador_id; set => _importador_id = value; }
        public string importador_name { get => _importador_name; set => _importador_name = value; }
        public DateTime descarga { get => _descarga; set => _descarga = value; }
        public string descripcion { get => _descripcion; set => _descripcion = value; }
        public int total_partida { get => _total_partida; set => _total_partida = value; }
      
        public decimal volumen { get => _volumen; set => _volumen = value; }
        public decimal peso { get => _peso; set => _peso = value; }
        public bool visto { get => _visto; set => _visto = value; }

        public string USUARIO_CREA { get => _USUARIO_CREA; set => _USUARIO_CREA = value; }
        public string USUARIO_MOD { get => _USUARIO_MOD; set => _USUARIO_MOD = value; }
        public string numero_carga { get => _numero_carga; set => _numero_carga = value; }

        public string xmlCabecera { get => _xmlCabecera; set => _xmlCabecera = value; }
        public string xmlDetalle { get => _xmlDetalle; set => _xmlDetalle = value; }
        #endregion

        public List<cfs_cargas_pendientes_detalle> Detalle { get; set; }

        public cfs_cargas_pendientes()
        {
            init();

            this.Detalle = new List<cfs_cargas_pendientes_detalle>();

        }

        private static void OnInit(string Base)
        {
            sql_puntero = (sql_puntero == null) ? Cls_Conexion.Conexion() : sql_puntero;
            parametros = new Dictionary<string, object>();
            nueva_conexion = Cls_Conexion.Nueva_Conexion(Base);
        }


        #region "Listados"
        public static List<cfs_cargas_pendientes> cargas_pendientes(string ruc, out string OnError)
        {
            OnInit("ADUANA");
            parametros.Add("ruc", ruc);
            return sql_puntero.ExecuteSelectControl<cfs_cargas_pendientes>(nueva_conexion, 6000, "[Bill].[validacion_cntr_cfs_impo_yard]", parametros, out OnError);

        }
        #endregion


        private Int64? Save_factura_manual(out string OnError)
        {

            parametros.Clear();
            parametros.Add("xmlCab", this.xmlCabecera);
            parametros.Add("xmlDet", this.xmlDetalle);
        
            var db = sql_puntero.ExecuteInsertUpdateDeleteReturn(sql_puntero.Conexion_Local, 8000, "cfs_bil_procesa_Invoice_multidespacho", parametros, out OnError);
            if (!db.HasValue || db.Value < 0)
            {
                return null;
            }
            OnError = string.Empty;
            return db.Value;

        }

        public Int64? SaveTransaction_MultiDespacho(out string OnError)
        {

            Int64 ID = 0;
            try
            {

                //grabar transaccion.
                var id = this.Save_factura_manual(out OnError);
                if (!id.HasValue)
                {
                    OnError = "*** Error: al grabar factura de multidespacho CFS ****";
                    return 0;
                }
                ID = id.Value;

                return ID;

            }
            catch (Exception ex)
            {

                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>("SQL", nameof(SaveTransaction_MultiDespacho), "SaveTransaction_MultiDespacho", false, null, null, ex.StackTrace, ex);
                OnError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));

                return null;
            }
        }








    }
}
