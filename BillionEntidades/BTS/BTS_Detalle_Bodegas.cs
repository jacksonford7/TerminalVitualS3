using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlConexion;

namespace BillionEntidades
{
    public class BTS_Detalle_Bodegas : Cls_Bil_Base
    {


        #region "Propiedades"
        public int Fila { get; set; }
        public string idNave { get; set; }
        public string codLine { get; set; }
        public string nave { get; set; }
        public string ruc { get; set; }
        public string Exportador { get; set; }
        public string booking { get; set; }
        public int idModalidad { get; set; }
        public string desc_modalidad { get; set; }
        public int id_bodega { get; set; }
        public string desc_bodega { get; set; }
        public string tipo_bodega { get; set; }
        public int id_tipo_bodega { get; set; }
        public int QTY_out { get; set; }
        public string referencia { get; set; }
        public string linea { get; set; }
        public int cajas { get; set; }
        #endregion


        public BTS_Detalle_Bodegas()
        {
            init();
        }

        public static List<BTS_Detalle_Bodegas> Carga_DetalleBodegas(string REFERENCIA, string LINEA, out string OnError)
        {
            parametros.Clear();
            parametros.Add("REFERENCIA", REFERENCIA);
            parametros.Add("LINEA", LINEA);
            return sql_puntero.ExecuteSelectControl<BTS_Detalle_Bodegas>(sql_puntero.Conexion_Local, 6000, "BTS_DETALLE_BODEGAS", parametros, out OnError);
        }

        public static List<BTS_Detalle_Bodegas> Carga_DetalleBodegas_Referencia(string REFERENCIA,  out string OnError)
        {
            parametros.Clear();
            parametros.Add("REFERENCIA", REFERENCIA);
            return sql_puntero.ExecuteSelectControl<BTS_Detalle_Bodegas>(sql_puntero.Conexion_Local, 6000, "BTS_REPORTE_DETALLE_BODEGAS", parametros, out OnError);
        }


        public static List<BTS_Detalle_Bodegas> Carga_DetalleBodegas_Exportadores(string REFERENCIA, out string OnError)
        {
            parametros.Clear();
            parametros.Add("REFERENCIA", REFERENCIA);
            return sql_puntero.ExecuteSelectControl<BTS_Detalle_Bodegas>(sql_puntero.Conexion_Local, 6000, "BTS_EXPORTADOR_DETALLE_BODEGAS", parametros, out OnError);
        }
    }

   
}
