using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlConexion;

namespace BillionEntidades
{
    public class P2D_Tiene_Factura : Cls_Bil_Base
    {
        #region "Variables"
        private static Int64? lm = -3;
        private Int64 _ID_PROFORMA;
        private Int64 _IV_ID;
        private string _IV_GLOSA = string.Empty;
        private DateTime? _IV_FECHA = null;
        private string _IV_TIPO_CARGA = string.Empty;
        private string _IV_ID_AGENTE = string.Empty;
        private string _IV_DESC_AGENTE = string.Empty;
        private string _IV_ID_CLIENTE = string.Empty;
        private string _IV_DESC_CLIENTE = string.Empty;
        private string _IV_EMAIL_CLIENTE = string.Empty;
        private string _IV_ID_FACTURADO = string.Empty;
        private string _IV_DESC_FACTURADO = string.Empty;
        private decimal _IV_SUBTOTAL = 0;
        private decimal _IV_IVA = 0;
        private decimal _IV_TOTAL = 0;
        private string _IV_NUMERO_CARGA = string.Empty;
        private string _IV_CONTENEDORES = string.Empty;
        private DateTime? _IV_FECHA_HASTA;
        private string _IV_DRAFT = string.Empty;
        private string _IV_FACTURA = string.Empty;

        private string _IV_BL = string.Empty;
        private string _IV_BUQUE = string.Empty;
        private string _IV_VIAJE = string.Empty;
        private string _IV_FECHA_ARRIBO = string.Empty;
        private string _IV_DIR_FACTURADO = string.Empty;
        private string _IV_EMAIL_FACTURADO = string.Empty;
        private string _IV_CIUDAD_FACTURADO = string.Empty;
        private Int64 _IV_DIAS_CREDITO = 0;
        private string _IV_HORA_HASTA = string.Empty;
        private string _IV_IP = string.Empty;
        private string _INVOICE_TYPE = string.Empty;

        private decimal _IV_TOTAL_BULTOS = 0;
        #endregion

        #region "Propiedades"
        public Int64 ID_PROFORMA { get => _ID_PROFORMA; set => _ID_PROFORMA = value; }
        public Int64 IV_ID { get => _IV_ID; set => _IV_ID = value; }
        public string IV_GLOSA { get => _IV_GLOSA; set => _IV_GLOSA = value; }
        public DateTime? IV_FECHA { get => _IV_FECHA; set => _IV_FECHA = value; }
        public string IV_TIPO_CARGA { get => _IV_TIPO_CARGA; set => _IV_TIPO_CARGA = value; }
        public string IV_ID_AGENTE { get => _IV_ID_AGENTE; set => _IV_ID_AGENTE = value; }
        public string IV_DESC_AGENTE { get => _IV_DESC_AGENTE; set => _IV_DESC_AGENTE = value; }
        public string IV_ID_CLIENTE { get => _IV_ID_CLIENTE; set => _IV_ID_CLIENTE = value; }
        public string IV_DESC_CLIENTE { get => _IV_DESC_CLIENTE; set => _IV_DESC_CLIENTE = value; }
        public string IV_EMAIL_CLIENTE { get => _IV_EMAIL_CLIENTE; set => _IV_EMAIL_CLIENTE = value; }
        public string IV_ID_FACTURADO { get => _IV_ID_FACTURADO; set => _IV_ID_FACTURADO = value; }
        public string IV_DESC_FACTURADO { get => _IV_DESC_FACTURADO; set => _IV_DESC_FACTURADO = value; }


        public decimal IV_SUBTOTAL { get => _IV_SUBTOTAL; set => _IV_SUBTOTAL = value; }
        public decimal IV_IVA { get => _IV_IVA; set => _IV_IVA = value; }
        public decimal IV_TOTAL { get => _IV_TOTAL; set => _IV_TOTAL = value; }


        public DateTime? IV_FECHA_HASTA { get => _IV_FECHA_HASTA; set => _IV_FECHA_HASTA = value; }
        public string IV_NUMERO_CARGA { get => _IV_NUMERO_CARGA; set => _IV_NUMERO_CARGA = value; }
        public string IV_CONTENEDORES { get => _IV_CONTENEDORES; set => _IV_CONTENEDORES = value; }

        public string IV_DRAFT { get => _IV_DRAFT; set => _IV_DRAFT = value; }
        public string IV_FACTURA { get => _IV_FACTURA; set => _IV_FACTURA = value; }

        public string IV_BL { get => _IV_BL; set => _IV_BL = value; }
        public string IV_BUQUE { get => _IV_BUQUE; set => _IV_BUQUE = value; }
        public string IV_VIAJE { get => _IV_VIAJE; set => _IV_VIAJE = value; }
        public string IV_FECHA_ARRIBO { get => _IV_FECHA_ARRIBO; set => _IV_FECHA_ARRIBO = value; }
        public string IV_DIR_FACTURADO { get => _IV_DIR_FACTURADO; set => _IV_DIR_FACTURADO = value; }
        public string IV_EMAIL_FACTURADO { get => _IV_EMAIL_FACTURADO; set => _IV_EMAIL_FACTURADO = value; }
        public string IV_CIUDAD_FACTURADO { get => _IV_CIUDAD_FACTURADO; set => _IV_CIUDAD_FACTURADO = value; }
        public Int64 IV_DIAS_CREDITO { get => _IV_DIAS_CREDITO; set => _IV_DIAS_CREDITO = value; }
        public string IV_HORA_HASTA { get => _IV_HORA_HASTA; set => _IV_HORA_HASTA = value; }
        public string IV_IP { get => _IV_IP; set => _IV_IP = value; }
        private static String v_mensaje = string.Empty;
        public string INVOICE_TYPE { get => _INVOICE_TYPE; set => _INVOICE_TYPE = value; }

        public decimal IV_TOTAL_BULTOS { get => _IV_TOTAL_BULTOS; set => _IV_TOTAL_BULTOS = value; }

        #endregion

        public P2D_Tiene_Factura()
        {
            init();
        }

        /*poblar */
        public bool PopulateMyData(out string OnError)
        {

            parametros.Clear();
            parametros.Add("ID_PROFORMA", this.ID_PROFORMA);
            var t = sql_puntero.ExecuteSelectOnly<P2D_Tiene_Factura>(sql_puntero.Conexion_Local, 6000, "P2D_CARGA_TIENE_FACTURA", parametros);
            if (t == null)
            {
                OnError = "";
                return false;
            }

            this.IV_ID = t.IV_ID;
            this.IV_GLOSA = t.IV_GLOSA;
            this.IV_FECHA = t.IV_FECHA;
            this.IV_TIPO_CARGA = t.IV_TIPO_CARGA;
            this.IV_ID_AGENTE = t.IV_ID_AGENTE;
            this.IV_DESC_AGENTE = t.IV_DESC_AGENTE;
            this.IV_ID_CLIENTE = t.IV_ID_CLIENTE;
            this.IV_DESC_CLIENTE = t.IV_DESC_CLIENTE;
            this.IV_ID_FACTURADO = t.IV_ID_FACTURADO;
            this.IV_DESC_FACTURADO = t.IV_DESC_FACTURADO;
            this.IV_SUBTOTAL = t.IV_SUBTOTAL;
            this.IV_IVA = t.IV_IVA;
            this.IV_TOTAL = t.IV_TOTAL;
            this.IV_DRAFT = t.IV_DRAFT;
            this.IV_FACTURA = t.IV_FACTURA;
            this.IV_NUMERO_CARGA = t.IV_NUMERO_CARGA;
            this.IV_FECHA_HASTA = t.IV_FECHA_HASTA;
            this.IV_TOTAL_BULTOS = t.IV_TOTAL_BULTOS;
            

            OnError = string.Empty;
            return true;
        }
    }
}
