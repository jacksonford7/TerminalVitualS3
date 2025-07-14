using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlConexion;
using PasePuerta;


namespace BillionEntidades
{
    public class Cls_Bil_PasePuertaContenedor_Cabecera : Cls_Bil_Base
    {
        #region "Variables"

        private DateTime? _FECHA = null;
        private string _MRN = string.Empty;
        private string _MSN = string.Empty;
        private string _HSN = string.Empty;
        private string _SESION = string.Empty;
        #endregion

        #region "Propiedades"


        public string MRN { get => _MRN; set => _MRN = value; }
        public string MSN { get => _MSN; set => _MSN = value; }
        public string HSN { get => _HSN; set => _HSN = value; }
        public string SESION { get => _SESION; set => _SESION = value; }
        public DateTime? FECHA { get => _FECHA; set => _FECHA = value; }

        private static String v_mensaje = string.Empty;

        #endregion
        public List<Cls_Bil_PasePuertaContenedor_Detalle> Detalle { get; set; }

        public Cls_Bil_PasePuertaContenedor_Cabecera()
        {
            init();
            this.Detalle = new List<Cls_Bil_PasePuertaContenedor_Detalle>();

        }

        public Cls_Bil_PasePuertaContenedor_Cabecera( string _MRN, string _MSN, string _HSN)

        {
            this.MRN = _MRN;
            this.MSN = _MSN;
            this.HSN = _HSN;
           
            this.Detalle = new List<Cls_Bil_PasePuertaContenedor_Detalle>();
          

        }
    }
}
