using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlConexion;

namespace BillionEntidades
{
    public class Cls_TRANSP_PasePuertaImpo_Detalle : Cls_Bil_Base
    {

        #region "Variables"

        private Int64 _ID_PPWEB;
        private Int64? _ID_PASE;
        private string _MRN = string.Empty;
        private string _MSN = string.Empty;
        private string _HSN = string.Empty;
        private string _FACTURA = string.Empty;
        private string _CARGA = string.Empty;
        private string _NUMERO_CARGA = string.Empty;
        private string _AGENTE = string.Empty;

        private Int64? _GKEY;
       
        private string _CONTENEDOR = string.Empty;
     
        private string _CIATRANS = string.Empty;
        private string _CHOFER = string.Empty;
        private string _ID_CIATRANS = string.Empty;
        private string _ID_CHOFER = string.Empty;
        private string _PLACA = string.Empty;
       
      
        private string _TIPO_CNTR = string.Empty;
       
        private string _TURNO;
   
     
        private string _TIPO_CARGA = string.Empty;
       
       

        private bool _CNTR_DD = false;
        private string _AGENTE_DESC = string.Empty;
        private string _FACTURADO_DESC = string.Empty;
        private string _IMPORTADOR = string.Empty;
        private string _IMPORTADOR_DESC = string.Empty;
        private string _TRANSPORTISTA_DESC = string.Empty;
        private string _CHOFER_DESC = string.Empty;

        private string _ESTADO_PAGO = string.Empty;

        private string _MOSTRAR_MENSAJE = string.Empty;
        private string _ORDENAMIENTO = string.Empty;
        private string _TIPO_CONSULTA = string.Empty;
      
        #endregion


        #region "Variables Modifica Pase Puerta"

        private DateTime? _FACTURADO_HASTA;
        private string _TIPO = string.Empty;
        private DateTime? _FECHA_TURNO;
        private string _HTURNO = string.Empty;
        private string _CIATRASNSP = string.Empty;
        private string _CONDUCTOR = string.Empty;
        private string _ID_EMPRESA = string.Empty;
        private string _NUMERO_PASE_N4;
      

    


        #endregion

        #region "Propiedades"

        public Int64 ID_PPWEB { get => _ID_PPWEB; set => _ID_PPWEB = value; }
        public string MRN { get => _MRN; set => _MRN = value; }
        public string MSN { get => _MSN; set => _MSN = value; }
        public string HSN { get => _HSN; set => _HSN = value; }
        public string FACTURA { get => _FACTURA; set => _FACTURA = value; }
        public string CARGA { get => _CARGA; set => _CARGA = value; }
        public string NUMERO_CARGA { get => _NUMERO_CARGA; set => _NUMERO_CARGA = value; }
        public string AGENTE { get => _AGENTE; set => _AGENTE = value; }
       
        public Int64? GKEY { get => _GKEY; set => _GKEY = value; }
       
        public string CIATRANS { get => _CIATRANS; set => _CIATRANS = value; }
        public string CHOFER { get => _CHOFER; set => _CHOFER = value; }
        public string ID_CIATRANS { get => _ID_CIATRANS; set => _ID_CIATRANS = value; }
        public string ID_CHOFER { get => _ID_CHOFER; set => _ID_CHOFER = value; }
        public string PLACA { get => _PLACA; set => _PLACA = value; }
      
        public string TIPO_CNTR { get => _TIPO_CNTR; set => _TIPO_CNTR = value; }
       
        public string TURNO { get => _TURNO; set => _TURNO = value; }
     
        public Int64? ID_PASE { get => _ID_PASE; set => _ID_PASE = value; }
      
        public string TIPO_CARGA { get => _TIPO_CARGA; set => _TIPO_CARGA = value; }
       

        public bool CNTR_DD { get => _CNTR_DD; set => _CNTR_DD = value; }
        public string AGENTE_DESC { get => _AGENTE_DESC; set => _AGENTE_DESC = value; }
        public string FACTURADO_DESC { get => _FACTURADO_DESC; set => _FACTURADO_DESC = value; }
        public string IMPORTADOR { get => _IMPORTADOR; set => _IMPORTADOR = value; }
        public string IMPORTADOR_DESC { get => _IMPORTADOR_DESC; set => _IMPORTADOR_DESC = value; }

        public string TRANSPORTISTA_DESC { get => _TRANSPORTISTA_DESC; set => _TRANSPORTISTA_DESC = value; }
        public string CHOFER_DESC { get => _CHOFER_DESC; set => _CHOFER_DESC = value; }

        public string MOSTRAR_MENSAJE { get => _MOSTRAR_MENSAJE; set => _MOSTRAR_MENSAJE = value; }
        public string TIPO_CONSULTA { get => _TIPO_CONSULTA; set => _TIPO_CONSULTA = value; }
        public string CONTENEDOR { get => _CONTENEDOR; set => _CONTENEDOR = value; }
        #endregion


        #region "Propiedades Modifica Pase Puerta"
        public DateTime? FACTURADO_HASTA { get => _FACTURADO_HASTA; set => _FACTURADO_HASTA = value; }
        public string TIPO { get => _TIPO; set => _TIPO = value; }
        public DateTime? FECHA_TURNO { get => _FECHA_TURNO; set => _FECHA_TURNO = value; }
        public string HTURNO { get => _HTURNO; set => _HTURNO = value; }
        public string CIATRASNSP { get => _CIATRASNSP; set => _CIATRASNSP = value; }
        public string CONDUCTOR { get => _CONDUCTOR; set => _CONDUCTOR = value; }
        public string ID_EMPRESA { get => _ID_EMPRESA; set => _ID_EMPRESA = value; }
    
        public string NUMERO_PASE_N4 { get => _NUMERO_PASE_N4; set => _NUMERO_PASE_N4 = value; }
       

   

        public string ESTADO_PAGO { get => _ESTADO_PAGO; set => _ESTADO_PAGO = value; }
       

        public string ORDENAMIENTO { get => _ORDENAMIENTO; set => _ORDENAMIENTO = value; }

     

        #endregion


        public Cls_TRANSP_PasePuertaImpo_Detalle()
        {
            init();
        }

        
    }
}
