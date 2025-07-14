using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlConexion;

namespace BillionEntidades
{
   public  class BTS_OTROS_Detalle_Rubros : Cls_Bil_Base
    {

  

        #region "Propiedades"

        public Int64 ID { get; set ; }
        public int Fila { get; set; }
        public string idNave { get; set; }
        public string ruc { get; set; }
        public string exportador { get; set; }
        public Int64 idStowageCab { get; set; }
        public int idExportador { get; set; }
        public decimal cantidad { get; set; }
        public string ruc_asume { get; set; }
        public string codigo { get; set; }
        public string codigoN4 { get; set; }
        public string nombre { get; set; }
        public string comentario { get; set; }
        public string value_tarifa_N4 { get; set; }
        public string invoicetype { get; set; }
        public string concepto { get; set; }
        public decimal valor { get; set; }
        public string codigo_tarifa_n4 { get; set; }
        public string linea { get; set; }
        public string referencia { get; set; }
        public string booking { get; set; }
        public decimal total { get; set; }
        public string numero_factura { get; set; }
        #endregion

        public BTS_OTROS_Detalle_Rubros()
        {
            init();
        }


        public static List<BTS_OTROS_Detalle_Rubros> Carga_DetalleRubros(Int64 idStowageCab, int idExportador, out string OnError)
        {
            parametros.Clear();
            parametros.Add("idStowageCab", idStowageCab);
            parametros.Add("idExportador", idExportador);
            return sql_puntero.ExecuteSelectControl<BTS_OTROS_Detalle_Rubros>(sql_puntero.Conexion_Local, 6000, "BTS_DETALLE_EXPORTADOR_SERVICIOS", parametros, out OnError);
        }

        public static List<BTS_OTROS_Detalle_Rubros> Carga_DetalleRubros_Otros( out string OnError)
        {
            parametros.Clear();
       
            return sql_puntero.ExecuteSelectControl<BTS_OTROS_Detalle_Rubros>(sql_puntero.Conexion_Local, 6000, "BTS_DETALLE_BODEGAS_RUBROS_N4_ADICIONALES", null, out OnError);
        }

        public static List<BTS_OTROS_Detalle_Rubros> Carga_DetalleRubros_Eventos(string Referencia, int idExportador, out string OnError)
        {
            parametros.Clear();
            parametros.Add("Referencia", Referencia);
            parametros.Add("idExportador", idExportador);
            return sql_puntero.ExecuteSelectControl<BTS_OTROS_Detalle_Rubros>(sql_puntero.Conexion_Local, 6000, "BTS_DETALLE_EXPORTADOR_SERVICIOS_EVENTOS", parametros, out OnError);
        }

        public static List<BTS_OTROS_Detalle_Rubros> Carga_DetalleRubros_Eventos_Agrupar(string Referencia, int idExportador, out string OnError)
        {
            parametros.Clear();
            parametros.Add("Referencia", Referencia);
            parametros.Add("idExportador", idExportador);
            return sql_puntero.ExecuteSelectControl<BTS_OTROS_Detalle_Rubros>(sql_puntero.Conexion_Local, 6000, "BTS_DETALLE_EXPORTADOR_SERVICIOS_EVENTOS_GRUPO", parametros, out OnError);
        }

    }
}
