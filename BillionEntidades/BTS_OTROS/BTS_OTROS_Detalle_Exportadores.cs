using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlConexion;

namespace BillionEntidades
{
   public  class BTS_OTROS_Detalle_Exportadores : Cls_Bil_Base
    {

  

        #region "Propiedades"

        public Int64 ID { get; set ; }
        public int Fila { get; set; }
        public string idNave { get; set; }
        public string codLine { get; set; }
        public string linea { get; set; }
        public string ruc { get; set; }
        public string exportador { get; set; }
        public Int64 idStowageCab { get; set; }
        public int idExportador { get; set; }
        public int cantidad { get; set; }
        public string ruc_asume { get; set; }
        public string exportador_asume { get; set; }
        public string numero_factura { get; set; }
        public int idExportador_asume { get; set; }
        public string booking { get; set; }
        public string referencia { get; set; }
        #endregion

        public BTS_OTROS_Detalle_Exportadores()
        {
            init();
        }


        public static List<BTS_OTROS_Detalle_Exportadores> Carga_CabecersExportadores(string REFERENCIA,  out string OnError)
        {
            parametros.Clear();
            parametros.Add("REFERENCIA", REFERENCIA);
            return sql_puntero.ExecuteSelectControl<BTS_OTROS_Detalle_Exportadores>(sql_puntero.Conexion_Local, 6000, "BTS_DETALLE_EXPORTADOR_ADICIONALES", parametros, out OnError);
        }

        public static List<BTS_OTROS_Detalle_Exportadores> Carga_CabecersExportadores_Eventos(string REFERENCIA, out string OnError)
        {
            parametros.Clear();
            parametros.Add("REFERENCIA", REFERENCIA);
            return sql_puntero.ExecuteSelectControl<BTS_OTROS_Detalle_Exportadores>(sql_puntero.Conexion_Local, 6000, "BTS_DETALLE_EXPORTADOR_ADICIONALES_EVENTOS", parametros, out OnError);
        }


    }
}
