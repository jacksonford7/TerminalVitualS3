using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlConexion;

namespace BillionEntidades
{
    public class Cls_Bil_CasManual : Cls_Bil_Base
    {

        #region "Variables"

        private int _total_items_manifiesto;
        private string _carga = string.Empty;
        private string _MRN = string.Empty;
        private string _MSN = string.Empty;
        private string _HSN = string.Empty;
        private DateTime? _fecha_manifiesto = null;
        private string _consignatario_manifiesto = string.Empty;
        private string _consignatario_manifiesto_id = string.Empty;
        private string _contenedor_manifiesto = string.Empty;
        private string _bl_manifiesto = string.Empty;
        private string _descripcion_manifiesto = string.Empty;  
        private string _desconsolidador_manifiesto = string.Empty;
        private Int64? _id_manifiesto = 0;
        private Int64? _id_manifiesto_detalle = 0;
        private Int64? _id = 0;
        private bool _visto = false;
        private string _llave = string.Empty;
        private string _usuario_libera = string.Empty;
        private DateTime? _fecha_registro = null;
        private decimal? _peso_total = 0;
        private DateTime? _fecha_libera = null;
        private string _facturas= string.Empty;
        private string _desconsolidador_asigna_id = string.Empty;
        private string _desconsolidador_asigna_nombre  = string.Empty;
        private string _desconsolidador_naviera = string.Empty;
        #endregion

        #region "Propiedades"

        public int total_items_manifiesto { get => _total_items_manifiesto; set => _total_items_manifiesto = value; }
        public string carga { get => _carga; set => _carga = value; }
        public string MRN { get => _MRN; set => _MRN = value; }
        public string MSN { get => _MSN; set => _MSN = value; }
        public string HSN { get => _HSN; set => _HSN = value; }
        public DateTime? fecha_manifiesto { get => _fecha_manifiesto; set => _fecha_manifiesto = value; }
        public string consignatario_manifiesto { get => _consignatario_manifiesto; set => _consignatario_manifiesto = value; }
        public string consignatario_manifiesto_id { get => _consignatario_manifiesto_id; set => _consignatario_manifiesto_id = value; }
        public string contenedor_manifiesto { get => _contenedor_manifiesto; set => _contenedor_manifiesto = value; }
        public string bl_manifiesto { get => _bl_manifiesto; set => _bl_manifiesto = value; }
        public string descripcion_manifiesto { get => _descripcion_manifiesto; set => _descripcion_manifiesto = value; }
        public string desconsolidador_manifiesto { get => _desconsolidador_manifiesto; set => _desconsolidador_manifiesto = value; }

        public string llave { get => _llave; set => _llave = value; }
        public Int64? id_manifiesto { get => _id_manifiesto; set => _id_manifiesto = value; }
        public Int64? id_manifiesto_detalle { get => _id_manifiesto_detalle; set => _id_manifiesto_detalle = value; }
        public Int64? id { get => _id; set => _id = value; }
        public bool visto { get => _visto; set => _visto = value; }
        public string usuario_libera { get => _usuario_libera; set => _usuario_libera = value; }
        public DateTime? fecha_registro { get => _fecha_registro; set => _fecha_registro = value; }
        public DateTime? fecha_libera { get => _fecha_libera; set => _fecha_libera = value; }
        public decimal? peso_total { get => _peso_total; set => _peso_total = value; }
        public string facturas { get => _facturas; set => _facturas = value; }

        public string desconsolidador_asigna_id { get => _desconsolidador_asigna_id; set => _desconsolidador_asigna_id = value; }
        public string desconsolidador_asigna_nombre { get => _desconsolidador_asigna_nombre; set => _desconsolidador_asigna_nombre = value; }

        public string desconsolidador_naviera { get => _desconsolidador_naviera; set => _desconsolidador_naviera = value; }

        #endregion

        public Cls_Bil_CasManual()
        {
            init();
        }

        public Cls_Bil_CasManual( int _total_items_manifiesto,string _MRN,string _MSN,string _HSN,DateTime? _fecha_manifiesto,string _consignatario_manifiesto,
            string _consignatario_manifiesto_id,string _contenedor_manifiesto, string _bl_manifiesto , string _descripcion_manifiesto,string _desconsolidador_manifiesto,
         Int64? _id_manifiesto, Int64? _id_manifiesto_detalle, string _usuario_libera, DateTime? _fecha_registro, decimal? _peso_total, DateTime? _fecha_libera, string _facturas,
         string _desconsolidador_naviera)

        {
            this.total_items_manifiesto = _total_items_manifiesto;
            this.MRN = _MRN;
            this.MSN = _MSN;
            this.HSN = _HSN;
            this.fecha_manifiesto = _fecha_manifiesto;
            this.consignatario_manifiesto = _consignatario_manifiesto;

            this.consignatario_manifiesto_id = _consignatario_manifiesto_id;
            this.contenedor_manifiesto = _contenedor_manifiesto;
            this.bl_manifiesto = _bl_manifiesto;
            this.descripcion_manifiesto = _descripcion_manifiesto;
            this.desconsolidador_manifiesto = _desconsolidador_manifiesto;
            this.id_manifiesto = _id_manifiesto;
            this.id_manifiesto_detalle = _id_manifiesto_detalle;
            this.usuario_libera = _usuario_libera;
            this.fecha_registro = _fecha_registro;
            this.peso_total = _peso_total;
            this.fecha_libera = _fecha_libera;
            this.facturas = _facturas;
            this.desconsolidador_naviera = _desconsolidador_naviera;

        }


        private static void OnInit()
        {
            sql_puntero = (sql_puntero == null) ? Cls_Conexion.Conexion() : sql_puntero;
            parametros = new Dictionary<string, object>();
            nueva_conexion = Cls_Conexion.Nueva_Conexion("PORTAL_BILLION");
        }

        /*lista todas las cartas autorizadas por rango de fechas y desconsolidadora*/
        public static List<Cls_Bil_CasManual> Listado_Autorizaciones_desconsolidadora(DateTime FECHA_DESDE, DateTime FECHA_HASTA, string desconsolidador_id, out string OnError)
        {

            OnInit();
            parametros.Clear();
            parametros.Add("FECHA_DESDE", FECHA_DESDE);
            parametros.Add("FECHA_HASTA", FECHA_HASTA);
            parametros.Add("desconsolidador_id", desconsolidador_id);
            return sql_puntero.ExecuteSelectControl<Cls_Bil_CasManual>(sql_puntero.Conexion_Local, 6000, "sp_Bil_Rpt_listar_autorizadas_cas_des", parametros, out OnError);

        }

        /*lista todas las cartas autorizadas por rango de fechas*/
        public static List<Cls_Bil_CasManual> Listado_Autorizaciones(DateTime FECHA_DESDE, DateTime FECHA_HASTA,  out string OnError)
        {

            OnInit();
            parametros.Clear();
            parametros.Add("FECHA_DESDE", FECHA_DESDE);
            parametros.Add("FECHA_HASTA", FECHA_HASTA);
            
            return sql_puntero.ExecuteSelectControl<Cls_Bil_CasManual>(sql_puntero.Conexion_Local, 6000, "sp_Bil_Rpt_listar_autorizadas_cas", parametros, out OnError);

        }

        /*lista todas las cartas autorizadas */
        public static List<Cls_Bil_CasManual> Listado_Autorizaciones_mrn(string mrn, string desconsolidador_id, out string OnError)
        {

            OnInit();
            parametros.Clear();
            parametros.Add("mrn", mrn);
            parametros.Add("desconsolidador_id", desconsolidador_id);

            return sql_puntero.ExecuteSelectControl<Cls_Bil_CasManual>(sql_puntero.Conexion_Local, 6000, "[Bill].[listar_autorizadas_cas]", parametros, out OnError);

        }

        public bool Anular(out string OnError)
        {
            parametros.Clear();
            parametros.Add("id", this.id);
            
            using (var scope = new System.Transactions.TransactionScope())
            {
                var db = sql_puntero.ExecuteInsertUpdateDelete(sql_puntero.Conexion_Local, 6000, "[Bill].[anula_cas_manual]", parametros, out OnError);
                if (!db.HasValue || db.Value < 0)
                {
                    return false;
                }
                scope.Complete();
            }
            return true;
        }


        #region "break bulk"
        /*lista todas las cartas autorizadas por rango de fechas y desconsolidadora*/
        public static List<Cls_Bil_CasManual> Listado_Autorizaciones_Brbk(DateTime FECHA_DESDE, DateTime FECHA_HASTA, string desconsolidador_id, out string OnError)
        {

            OnInit();
            parametros.Clear();
            parametros.Add("FECHA_DESDE", FECHA_DESDE);
            parametros.Add("FECHA_HASTA", FECHA_HASTA);
            parametros.Add("desconsolidador_id", desconsolidador_id);
            return sql_puntero.ExecuteSelectControl<Cls_Bil_CasManual>(sql_puntero.Conexion_Local, 6000, "sp_Bil_Rpt_listar_autorizadas_cas_brbk", parametros, out OnError);

        }

        /*lista todas las cartas autorizadas */
        public static List<Cls_Bil_CasManual> Listado_Autorizaciones_brbk_mrn(string mrn, string desconsolidador_id, out string OnError)
        {

            OnInit();
            parametros.Clear();
            parametros.Add("mrn", mrn);
            parametros.Add("desconsolidador_id", desconsolidador_id);

            return sql_puntero.ExecuteSelectControl<Cls_Bil_CasManual>(sql_puntero.Conexion_Local, 6000, "[Bill].[listar_autorizadas_cas_brbk]", parametros, out OnError);

        }

        public bool Anular_Brbk(out string OnError)
        {
            parametros.Clear();
            parametros.Add("id", this.id);

            using (var scope = new System.Transactions.TransactionScope())
            {
                var db = sql_puntero.ExecuteInsertUpdateDelete(sql_puntero.Conexion_Local, 6000, "[Bill].[anula_cas_manual_brbk]", parametros, out OnError);
                if (!db.HasValue || db.Value < 0)
                {
                    return false;
                }
                scope.Complete();
            }
            return true;
        }

        public static List<Cls_Bil_CasManual> Listado_Autorizaciones_Brbk(DateTime FECHA_DESDE, DateTime FECHA_HASTA, out string OnError)
        {

            OnInit();
            parametros.Clear();
            parametros.Add("FECHA_DESDE", FECHA_DESDE);
            parametros.Add("FECHA_HASTA", FECHA_HASTA);

            return sql_puntero.ExecuteSelectControl<Cls_Bil_CasManual>(sql_puntero.Conexion_Local, 6000, "sp_Bil_Rpt_listar_autorizadas_cas_brbk_des", parametros, out OnError);

        }

        #endregion

    }
}
