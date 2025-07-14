using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlConexion;


namespace BillionEntidades
{
    public class Cls_Bil_Invoice_Duplicados : Cls_Bil_Base
    {

        #region "Variables"

        private Int64 _INV_CODIGO;
        private DateTime? _INV_FECHA_GENERACION = null;
        private string _INV_NUMERO_FACTURA = string.Empty;
        private string _INV_RUC_CLIENTE = string.Empty;

        private string _INV_NOMBRE_CLIENTE = string.Empty;

        private decimal _INV_TOTAL = 0;
        private decimal _INV_TOT_PRECIO = 0;

        private int _EXISTE = 0;

        private string _RESULTADO = string.Empty;

        #endregion

        #region "Propiedades"

        public Int64 INV_CODIGO { get => _INV_CODIGO; set => _INV_CODIGO = value; }
        public string INV_NUMERO_FACTURA { get => _INV_NUMERO_FACTURA; set => _INV_NUMERO_FACTURA = value; }
        public DateTime? INV_FECHA_GENERACION { get => _INV_FECHA_GENERACION; set => _INV_FECHA_GENERACION = value; }
        public string INV_RUC_CLIENTE { get => _INV_RUC_CLIENTE; set => _INV_RUC_CLIENTE = value; }
  
        public string INV_NOMBRE_CLIENTE { get => _INV_NOMBRE_CLIENTE; set => _INV_NOMBRE_CLIENTE = value; }
        public decimal INV_TOTAL { get => _INV_TOTAL; set => _INV_TOTAL = value; }
        public decimal INV_TOT_PRECIO { get => _INV_TOT_PRECIO; set => _INV_TOT_PRECIO = value; }

        public int EXISTE { get => _EXISTE; set => _EXISTE = value; }

        public string RESULTADO { get => _RESULTADO; set => _RESULTADO = value; }

        #endregion

        public Cls_Bil_Invoice_Duplicados()
        {
            init();

        }

        /*carga contenedores con fecha y ultima factura billion*/
        public static List<Cls_Bil_Invoice_Duplicados> Existe_Factura(string cliente, string xmlContenedores,out string OnError)
        {
            parametros.Clear();
            parametros.Add("cliente", cliente);
            parametros.Add("XmlContenedor", xmlContenedores);
            return sql_puntero.ExecuteSelectControl<Cls_Bil_Invoice_Duplicados>(sql_puntero.Conexion_Local, 4000, "sp_Bil_Validacion_Factura", parametros, out OnError);

        }

        public static List<Cls_Bil_Invoice_Duplicados> Primera_Factura(string idBBK, out string OnError)
        {
            parametros.Clear();
            parametros.Add("idBBK", idBBK);
            return sql_puntero.ExecuteSelectControl<Cls_Bil_Invoice_Duplicados>(sql_puntero.Conexion_Local, 6000, "sp_Bil_Validacion_Factura_bbk", parametros, out OnError);

        }

        public static List<Cls_Bil_Invoice_Duplicados> Valida_Servicios_Brbk(string TIPO, string xmlServicios, out string OnError)
        {
            parametros.Clear();
            parametros.Add("TIPO", TIPO);
            parametros.Add("xmlServicios", xmlServicios);
            return sql_puntero.ExecuteSelectControl<Cls_Bil_Invoice_Duplicados>(sql_puntero.Conexion_Local, 4000, "[Bill].[VALIDA_SERVIICOS_BRBK]", parametros, out OnError);

        }
    }
}
