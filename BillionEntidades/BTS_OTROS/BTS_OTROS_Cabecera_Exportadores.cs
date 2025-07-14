using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlConexion;

namespace BillionEntidades
{
    public class BTS_OTROS_Cabecera_Exportadores : Cls_Bil_Base
    {

    

        #region "Propiedades"

        public Int64 ID { get; set ; }
        public string GLOSA { get ; set; }
        public DateTime? FECHA { get ; set ; }      
        public string TIPO_CARGA { get; set; }    
        public string ID_CLIENTE { get; set; }
        public string DESC_CLIENTE { get ; set ; }
        public string ID_FACTURADO { get; set ; }
        public string DESC_FACTURADO { get ; set ; }
        public string REFERENCIA { get; set ; }
        
        public decimal SUBTOTAL { get ; set ; }
        public decimal IVA { get ; set; }
        public decimal TOTAL { get ; set ; }


     
        public string DIR_FACTURADO { get ; set; }
        public string EMAIL_FACTURADO { get ; set; }
        public string CIUDAD_FACTURADO { get ; set; }
        public Int64 DIAS_CREDITO { get ; set ; }
        public string SESION { get ; set ; }
     
        private static String v_mensaje = string.Empty;

        public string INVOICE_TYPE { get; set ; }

        public string RUC_USUARIO { get ; set ; }
        public string DESC_USUARIO { get ; set ; }

        public int TOTAL_CAJA { get; set; }

        public string DRAF { get; set; }
        public string NUMERO_FACTURA { get; set; }

        public string LINEA { get; set; }
        public string BOOKING { get; set; }

        #endregion



        public List<BTS_OTROS_Detalle_Exportadores> Detalle_Exportador { get; set; }
        public List<BTS_OTROS_Detalle_Rubros> Detalle_Rubros { get; set; }
        public List<BTS_OTROS_Detalle_ExportadoresUnico> Detalle_Exportador_Unico { get; set; }
        public List<BTS_OTROS_Detalle_Rubros_Factura> Detalle_Rubros_Factura { get; set; }

        public List<BTS_OTROS_Detalle_Rubros> Detalle_Rubros_Adicionales { get; set; }

        public BTS_OTROS_Cabecera_Exportadores()
        {
            init();

            this.Detalle_Exportador = new List<BTS_OTROS_Detalle_Exportadores>();
            this.Detalle_Rubros = new List<BTS_OTROS_Detalle_Rubros>();
            this.Detalle_Exportador_Unico = new List<BTS_OTROS_Detalle_ExportadoresUnico>();
            this.Detalle_Rubros_Factura = new List<BTS_OTROS_Detalle_Rubros_Factura>();

            this.Detalle_Rubros_Adicionales = new List<BTS_OTROS_Detalle_Rubros>();

        }



  


    }
}
