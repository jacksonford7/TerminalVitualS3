using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlConexion;

namespace BillionEntidades
{
    public class cfs_ver_certificado : Cls_Bil_Base
    {

        #region "Variables"

        
        private string _MRN = string.Empty;
        private string _MSN = string.Empty;
        private string _HSN = string.Empty;
        private Int64 _CONSECUTIVO;
        private int _CANTIDAD = 0;
        private decimal _P2D_ALTO=0;
        private decimal _P2D_ANCHO = 0;
        private decimal _P2D_LARGO = 0;
        private decimal _PESO = 0;
        private decimal _P2D_VOLUMEN = 0;

        private string _NUMERO_CERTIFICADO = string.Empty;
        private string _AGENTE = string.Empty;
        private string _AGENTE_DESC = string.Empty;
        private string _FACTURADO = string.Empty;
        private string _FACTURADO_DESC = string.Empty;
        private string _IMPORTADOR = string.Empty;
        private string _IMPORTADOR_DESC = string.Empty;

        private string _TEXTO1 = string.Empty;
        private string _TEXTO2 = string.Empty;
        private string _TITULO1 = string.Empty;
        private string _TITULO2 = string.Empty;
        #endregion

        #region "Propiedades"


        public string MRN { get => _MRN; set => _MRN = value; }
        public string MSN { get => _MSN; set => _MSN = value; }
        public string HSN { get => _HSN; set => _HSN = value; }
        public Int64 CONSECUTIVO { get => _CONSECUTIVO; set => _CONSECUTIVO = value; }
        public int CANTIDAD { get => _CANTIDAD; set => _CANTIDAD = value; }
        public decimal P2D_ALTO { get => _P2D_ALTO; set => _P2D_ALTO = value; }
        public decimal P2D_ANCHO { get => _P2D_ANCHO; set => _P2D_ANCHO = value; }
        public decimal P2D_LARGO { get => _P2D_LARGO; set => _P2D_LARGO = value; }
        public decimal PESO { get => _PESO; set => _PESO = value; }
        public decimal P2D_VOLUMEN { get => _P2D_VOLUMEN; set => _P2D_VOLUMEN = value; }

        public string NUMERO_CERTIFICADO { get => _NUMERO_CERTIFICADO; set => _NUMERO_CERTIFICADO = value; }
        public string AGENTE { get => _AGENTE; set => _AGENTE = value; }
        public string AGENTE_DESC { get => _AGENTE_DESC; set => _AGENTE_DESC = value; }
        public string FACTURADO { get => _FACTURADO; set => _FACTURADO = value; }
        public string FACTURADO_DESC { get => _FACTURADO_DESC; set => _FACTURADO_DESC = value; }

        public string IMPORTADOR { get => _IMPORTADOR; set => _IMPORTADOR = value; }
        public string IMPORTADOR_DESC { get => _IMPORTADOR_DESC; set => _IMPORTADOR_DESC = value; }

        public string TEXTO1 { get => _TEXTO1; set => _TEXTO1 = value; }
        public string TEXTO2 { get => _TEXTO2; set => _TEXTO2 = value; }
        public string TITULO1 { get => _TITULO1; set => _TITULO1 = value; }
        public string TITULO2 { get => _TEXTO2; set => _TEXTO2 = value; }
        #endregion

        public cfs_ver_certificado()
        {
            init();

        }

      

        public static List<cfs_ver_certificado> ver_certificado(string mrn, string msn, string hsn, out string OnError)
        {
          
            parametros.Clear();
            parametros.Add("mrn", mrn);
            parametros.Add("msn", msn);
            parametros.Add("hsn", hsn);
            return sql_puntero.ExecuteSelectControl<cfs_ver_certificado>(sql_puntero.Conexion_Local, 6000, "[Bill].[ver_certificados_pesos]", parametros, out OnError);

        }

      
    }
}
