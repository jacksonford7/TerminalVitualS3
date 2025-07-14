using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlConexion;

namespace BillionEntidades
{
   public  class BTS_Detalle_Eventos : Cls_Bil_Base
    {



        #region "Propiedades"
        public int Fila { get; set; }
        public Int64 id { get; set ; }
        public string referencia { get; set; }
        public DateTime? fecha { get; set; }
        public Int64 id_servicio { get; set; }
        public int id_exportador { get; set; }
        public string desc_exportador { get; set; }
        public int cajas { get; set; }
        public int idModalidad { get; set; }
        public string comentario { get; set; }
        public bool estado { get; set; }
        public string usuario_reg { get; set; }
        public string usuario_mod { get; set; }
        public DateTime? fecha_mod { get; set; }
        public string numero_factura { get; set; }
        public string VALUE_TARIJA_N4 { get; set; }
        public string DESCRIP_TARIFA_N4 { get; set; }
        public string tarifas { get; set; }
        public string ruc { get; set; }
        public string linea { get; set; }
        public decimal valor { get; set; }
        #endregion

        public BTS_Detalle_Eventos()
        {
            init();
        }



        public static List<BTS_Detalle_Eventos> Listado_Eventos(string REFERENCIA, out string OnError)
        {
            parametros.Clear();
            parametros.Add("REFERENCIA", REFERENCIA);
            return sql_puntero.ExecuteSelectControl<BTS_Detalle_Eventos>(sql_puntero.Conexion_Local, 6000, "BTS_LISTADO_EVENTOS_REGISTRADOS", parametros, out OnError);
        }


    }
}
