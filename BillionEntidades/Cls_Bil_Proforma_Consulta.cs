using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlConexion;
namespace BillionEntidades
{
    public class Cls_Bil_Proforma_Consulta : Cls_Bil_Base
    {


        #region "Variables"

        private Int64 _PF_ID;
        private string _PF_GLOSA = string.Empty;
        private DateTime? _PF_FECHA = null;
        private DateTime? _PF_FECHA_CREA = null;
        private string _PF_TIPO_CARGA = string.Empty;
        private string _PF_CODIGO_AGENTE = string.Empty;
        private string _PF_ID_AGENTE = string.Empty;
        private string _PF_DESC_AGENTE = string.Empty;
        private string _PF_ID_CLIENTE = string.Empty;
        private string _PF_DESC_CLIENTE = string.Empty;
        private string _PF_ID_FACTURADO = string.Empty;
        private string _PF_DESC_FACTURADO = string.Empty;
        private string _PF_USUARIO_CREA = string.Empty;
        private string _PF_CONTENEDOR = string.Empty;
        private int _PF_LINEA;
        private string _PF_ID_SERVICIO = string.Empty;
        private string _PF_DESC_SERVICIO = string.Empty;
        private string _PF_CARGA = string.Empty;
        private string _PF_TIPO_SERVICIO = string.Empty;
        private decimal _PF_SER_CANTIDAD = 0;
        private decimal _PF_SER_PRECIO = 0;
        private decimal _PF_SER_SUBTOTAL = 0;
        private decimal _PF_SER_IVA = 0;

        private decimal _PF_SUBTOTAL = 0;
        private decimal _PF_IVA = 0;
        private decimal _PF_TOTAL = 0;
        private string _PF_NUMERO_CARGA = string.Empty;
        private DateTime? _PF_FECHA_HASTA = null;
        private string _PF_SESION;

        #endregion

        #region "Propiedades"

        public Int64 PF_ID { get => _PF_ID; set => _PF_ID = value; }
        public string PF_GLOSA { get => _PF_GLOSA; set => _PF_GLOSA = value; }
        public DateTime? PF_FECHA { get => _PF_FECHA; set => _PF_FECHA = value; }
        public DateTime? PF_FECHA_HASTA { get => _PF_FECHA_HASTA; set => _PF_FECHA_HASTA = value; }
        public DateTime? PF_FECHA_CREA { get => _PF_FECHA_CREA; set => _PF_FECHA_CREA = value; }
        public string PF_TIPO_CARGA { get => _PF_TIPO_CARGA; set => _PF_TIPO_CARGA = value; }
        public string PF_ID_AGENTE { get => _PF_ID_AGENTE; set => _PF_ID_AGENTE = value; }
        public string PF_CODIGO_AGENTE { get => _PF_CODIGO_AGENTE; set => _PF_CODIGO_AGENTE = value; }
        public string PF_DESC_AGENTE { get => _PF_DESC_AGENTE; set => _PF_DESC_AGENTE = value; }
        public string PF_ID_CLIENTE { get => _PF_ID_CLIENTE; set => _PF_ID_CLIENTE = value; }
        public string PF_DESC_CLIENTE { get => _PF_DESC_CLIENTE; set => _PF_DESC_CLIENTE = value; }
        public string PF_ID_FACTURADO { get => _PF_ID_FACTURADO; set => _PF_ID_FACTURADO = value; }
        public string PF_DESC_FACTURADO { get => _PF_DESC_FACTURADO; set => _PF_DESC_FACTURADO = value; }
        public string PF_USUARIO_CREA { get => _PF_USUARIO_CREA; set => _PF_USUARIO_CREA = value; }
        public string PF_CONTENEDOR { get => _PF_CONTENEDOR; set => _PF_CONTENEDOR = value; }

        public int PF_LINEA { get => _PF_LINEA; set => _PF_LINEA = value; }
        public string PF_ID_SERVICIO { get => _PF_ID_SERVICIO; set => _PF_ID_SERVICIO = value; }
        public string PF_DESC_SERVICIO { get => _PF_DESC_SERVICIO; set => _PF_DESC_SERVICIO = value; }
        public string PF_CARGA { get => _PF_CARGA; set => _PF_CARGA = value; }
        public string PF_TIPO_SERVICIO { get => _PF_TIPO_SERVICIO; set => _PF_TIPO_SERVICIO = value; }
        public decimal PF_SER_CANTIDAD { get => _PF_SER_CANTIDAD; set => _PF_SER_CANTIDAD = value; }
        public decimal PF_SER_PRECIO { get => _PF_SER_PRECIO; set => _PF_SER_PRECIO = value; }
        public decimal PF_SER_SUBTOTAL { get => _PF_SER_SUBTOTAL; set => _PF_SER_SUBTOTAL = value; }
        public decimal PF_SER_IVA { get => _PF_SER_IVA; set => _PF_SER_IVA = value; }


        public decimal PF_SUBTOTAL { get => _PF_SUBTOTAL; set => _PF_SUBTOTAL = value; }
        public decimal PF_IVA { get => _PF_IVA; set => _PF_IVA = value; }
        public decimal PF_TOTAL { get => _PF_TOTAL; set => _PF_TOTAL = value; }

        public string PF_NUMERO_CARGA { get => _PF_NUMERO_CARGA; set => _PF_NUMERO_CARGA = value; }
        public string PF_SESION { get => _PF_SESION; set => _PF_SESION = value; }

        private static String v_mensaje = string.Empty;


        #endregion

        public Cls_Bil_Proforma_Consulta()
        {
            init();
 
        }

        /*carga todas las notas de credito pendientes de aprobar de un uusario*/
        public static List<Cls_Bil_Proforma_Consulta> List_Proforma(Int64 PF_ID, out string OnError)
        {
            parametros.Clear();
            parametros.Add("PF_ID", PF_ID);     
            return sql_puntero.ExecuteSelectControl<Cls_Bil_Proforma_Consulta>(sql_puntero.Conexion_Local, 4000, "sp_Bil_consulta_proforma", parametros, out OnError);

        }
    }
}
