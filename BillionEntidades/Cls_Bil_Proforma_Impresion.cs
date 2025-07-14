using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlConexion;

namespace BillionEntidades
{
    public class Cls_Bil_Proforma_Impresion : Cls_Bil_Base
    {

        #region "Variables"

        private Int64 _PF_ID;
        private string _PF_NUMERO = string.Empty;
        private string _PF_FECHA = null;
        private string _PF_TIPO_CARGA = string.Empty;
        private string _PF_GLOSA = string.Empty;
        private string _PF_CONTENEDOR = string.Empty;
        private string _PF_ID_AGENTE = string.Empty;
        private string _PF_DESC_AGENTE = string.Empty;
        private string _PF_ID_CLIENTE = string.Empty;
        private string _PF_DESC_CLIENTE = string.Empty;
        private string _PF_ID_FACTURADO = string.Empty;
        private string _PF_DESC_FACTURADO = string.Empty;
        private string _PF_NUMERO_CARGA = string.Empty;

        private string _PF_ID_SERVICIO = string.Empty;
        private string _PF_DESC_SERVICIO = string.Empty;
        private decimal _PF_CANTIDAD = 0;
        private decimal _PF_PRECIO = 0;
        private decimal _PF_SUBTOTAL = 0;
        private decimal _PF_IVA = 0;


        private decimal _TOT_PF_SUBTOTAL = 0;
        private decimal _TOT_PF_IVA = 0;
        private decimal _TOT_PF_TOTAL = 0;

      
        #endregion

        #region "Propiedades"

        public Int64 PF_ID { get => _PF_ID; set => _PF_ID = value; }
        public string PF_NUMERO { get => _PF_NUMERO; set => _PF_NUMERO = value; }
        public string PF_FECHA { get => _PF_FECHA; set => _PF_FECHA = value; }
        public string PF_GLOSA { get => _PF_GLOSA; set => _PF_GLOSA = value; }
        public string PF_TIPO_CARGA { get => _PF_TIPO_CARGA; set => _PF_TIPO_CARGA = value; }
        public string PF_CONTENEDOR { get => _PF_CONTENEDOR; set => _PF_CONTENEDOR = value; }

        public string PF_ID_AGENTE { get => _PF_ID_AGENTE; set => _PF_ID_AGENTE = value; }
        public string PF_DESC_AGENTE { get => _PF_DESC_AGENTE; set => _PF_DESC_AGENTE = value; }
        public string PF_ID_CLIENTE { get => _PF_ID_CLIENTE; set => _PF_ID_CLIENTE = value; }
        public string PF_DESC_CLIENTE { get => _PF_DESC_CLIENTE; set => _PF_DESC_CLIENTE = value; }
        public string PF_ID_FACTURADO { get => _PF_ID_FACTURADO; set => _PF_ID_FACTURADO = value; }
        public string PF_DESC_FACTURADO { get => _PF_DESC_FACTURADO; set => _PF_DESC_FACTURADO = value; }
        public string PF_NUMERO_CARGA { get => _PF_NUMERO_CARGA; set => _PF_NUMERO_CARGA = value; }


     
        public string PF_ID_SERVICIO { get => _PF_ID_SERVICIO; set => _PF_ID_SERVICIO = value; }
        public string PF_DESC_SERVICIO { get => _PF_DESC_SERVICIO; set => _PF_DESC_SERVICIO = value; }
        public decimal PF_CANTIDAD { get => _PF_CANTIDAD; set => _PF_CANTIDAD = value; }
        public decimal PF_PRECIO { get => _PF_PRECIO; set => _PF_PRECIO = value; }
        public decimal PF_SUBTOTAL { get => _PF_SUBTOTAL; set => _PF_SUBTOTAL = value; }
        public decimal PF_IVA { get => _PF_IVA; set => _PF_IVA = value; }


        public decimal TOT_PF_SUBTOTAL { get => _TOT_PF_SUBTOTAL; set => _TOT_PF_SUBTOTAL = value; }
        public decimal TOT_PF_IVA { get => _TOT_PF_IVA; set => _TOT_PF_IVA = value; }
        public decimal TOT_PF_TOTAL { get => _TOT_PF_TOTAL; set => _TOT_PF_TOTAL = value; }


        private static String v_mensaje = string.Empty;


        #endregion

        public Cls_Bil_Proforma_Impresion()
        {
            init();

        }

        /*carga todas las proformas*/
        public static List<Cls_Bil_Proforma_Impresion> Datos_Proforma(Int64 PF_ID, out string OnError)
        {
            parametros.Clear();
            parametros.Add("PF_ID", PF_ID);
            return sql_puntero.ExecuteSelectControl<Cls_Bil_Proforma_Impresion>(sql_puntero.Conexion_Local, 4000, "sp_Bil_Rpt_Formato_ImprimirProforma", parametros, out OnError);

        }
    }
}
