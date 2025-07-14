using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlConexion;

namespace BillionEntidades
{
    public class BTS_Detalle_Muelle : Cls_Bil_Base
    {

        #region "Propiedades"

        public Int64 ID { get; set; }
        public int Fila { get; set; }
        public string idNave { get; set; }
        public string codLine { get; set; }
        public string nave { get; set; }
        public string aisv_codig_clte { get; set; }
        public string aisv_nom_expor { get; set; }
        public string aisv_estado { get; set; }
        public int cajas { get; set; }
        public int cajas_confirmar { get; set; }
        public int idExportador { get; set; }
        public string Exportador { get; set; }
        public string ruc { get; set; }
        public string referencia { get; set; }
        public string linea { get; set; }
        public int cajas_paletizado { get; set; }
        #endregion

        public BTS_Detalle_Muelle()
        {
            init();
        }

        private static void OnInit(string Base)
        {
            sql_puntero = (sql_puntero == null) ? Cls_Conexion.Conexion() : sql_puntero;
            parametros = new Dictionary<string, object>();
            nueva_conexion = Cls_Conexion.Nueva_Conexion(Base);
        }



        public static List<BTS_Detalle_Muelle> Carga_DetalleMuelle(string REFERENCIA, string LINEA, out string OnError)
        {
            OnInit("Portal_Servicios");

            parametros.Clear();
            parametros.Add("REFERENCIA", REFERENCIA);
            parametros.Add("LINEA", LINEA);
            return sql_puntero.ExecuteSelectControl<BTS_Detalle_Muelle>(nueva_conexion, 6000, "BTS_DETALLE_MUELLE", parametros, out OnError);
        }

        public static List<BTS_Detalle_Muelle> Carga_DetalleMuelle_Referencia(string REFERENCIA, out string OnError)
        {
            OnInit("Portal_Servicios");

            parametros.Clear();
            parametros.Add("REFERENCIA", REFERENCIA);
            return sql_puntero.ExecuteSelectControl<BTS_Detalle_Muelle>(nueva_conexion, 6000, "BTS_REPORTE_DETALLE_MUELLE", parametros, out OnError);
        }


        public static List<BTS_Detalle_Muelle> Carga_DetalleMuelle_Confirmar(string REFERENCIA,  out string OnError)
        {
            OnInit("Portal_Servicios");

            parametros.Clear();
            parametros.Add("REFERENCIA", REFERENCIA);
            return sql_puntero.ExecuteSelectControl<BTS_Detalle_Muelle>(nueva_conexion, 6000, "BTS_DETALLE_MUELLE_CONFIRMAR", parametros, out OnError);
        }

        public static List<BTS_Detalle_Muelle> Carga_DetalleMuelle_Exportadores(string REFERENCIA, out string OnError)
        {
            OnInit("Portal_Servicios");

            parametros.Clear();
            parametros.Add("REFERENCIA", REFERENCIA);
            return sql_puntero.ExecuteSelectControl<BTS_Detalle_Muelle>(nueva_conexion, 6000, "BTS_EXPORTADOR_MUELLE_ADICIONALES", parametros, out OnError);
        }


    }


}
