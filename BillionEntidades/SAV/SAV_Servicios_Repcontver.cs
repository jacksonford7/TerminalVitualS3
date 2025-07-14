using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillionEntidades
{
   public class SAV_Servicios_Repcontver : Cls_Bil_Base
    {
    
        public Int64 ID_SERVICIO { get; set; }   
        public string CODIGO_TARIJA_N4 { get; set; }   
        public string VALUE_TARIJA_N4 { get; set; }
        public string DESCRIP_TARIFA_N4 { get; set; }
        public decimal IVA { get; set; }
        public decimal VALOR { get; set; }
        public decimal CANTIDAD { get; set; }
        public string INVOICETYPE { get; set; }

        public SAV_Servicios_Repcontver()
        {
            init();
        }

        public static List<SAV_Servicios_Repcontver> Carga_DetalleRubros_Otros(string LINEA, out string OnError)
        {
            parametros.Clear();
            parametros.Add("LINEA", LINEA);
            return sql_puntero.ExecuteSelectControl<SAV_Servicios_Repcontver>(sql_puntero.Conexion_Local, 6000, "REPCON_SAV_SERVICIOS", parametros, out OnError);
        }

    }
}
