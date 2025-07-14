using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlConexion;

namespace BillionEntidades
{
   public  class P2D_Proforma_Detalle : Cls_Bil_Base
    {

        #region "Variables"

        private Int64 _ID_PROFORMA;
        private int _SECUENCIA;
        private decimal _PESO = 0;
        private decimal _LARGO = 0;
        private decimal _ALTO = 0;
        private decimal _ANCHO = 0;
        private decimal _M3 = 0;
        private bool _ESTADO = false;
        private string _USUARIO_CREA = string.Empty;
        private string _USUARIO_MOD = string.Empty;
        private string _M3_TEXTO = string.Empty;


        #endregion

        #region "Propiedades"

        public Int64 ID_PROFORMA { get => _ID_PROFORMA; set => _ID_PROFORMA = value; }
        public int SECUENCIA { get => _SECUENCIA; set => _SECUENCIA = value; }
        public decimal PESO { get => _PESO; set => _PESO = value; }
        public decimal LARGO { get => _LARGO; set => _LARGO = value; }
        public decimal ALTO { get => _ALTO; set => _ALTO = value; }
        public decimal ANCHO { get => _ANCHO; set => _ANCHO = value; }
        public decimal M3 { get => _M3; set => _M3 = value; }
        public bool ESTADO { get => _ESTADO; set => _ESTADO = value; }
        public string USUARIO_CREA { get => _USUARIO_CREA; set => _USUARIO_CREA = value; }
        public string USUARIO_MOD { get => _USUARIO_MOD; set => _USUARIO_MOD = value; }
        public string M3_TEXTO { get => _M3_TEXTO; set => _M3_TEXTO = value; }
        #endregion

        public P2D_Proforma_Detalle()
        {
            init();
        }


        public P2D_Proforma_Detalle( Int64 _ID_PROFORMA,
         int _SECUENCIA,
         decimal _PESO,
         decimal _LARGO,
         decimal _ALTO,
         decimal _ANCHO,
         decimal _M3,
         bool _ESTADO,
         string _USUARIO_CREA,
         string _USUARIO_MOD,
         string _M3_TEXTO)
        {

            this.ID_PROFORMA = _ID_PROFORMA;
            this.SECUENCIA = _SECUENCIA;
            this.PESO = _PESO;
            this.LARGO = _LARGO;
            this.ALTO = _ALTO;

            this.ANCHO = _ANCHO;
            this.M3 = _M3;

            this.ESTADO = _ESTADO;
            this.USUARIO_CREA = _USUARIO_CREA;
            this.USUARIO_MOD = _USUARIO_MOD;
            this.M3_TEXTO = _M3_TEXTO;
        }


    }
}
