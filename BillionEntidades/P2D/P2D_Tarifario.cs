using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlConexion;

namespace BillionEntidades
{
    public class P2D_Tarifario : Cls_Bil_Base
    {
        #region "Variables"

        private Int64 _ID_TARIFA = 0;
        private int _SECUENCIA = 0;
        private decimal _VALOR = 0;
        private decimal _M3 = 0;
        private decimal _PESO = 0;

        private decimal _PRECIO = 0;
        private decimal _TOTAL_PAGAR = 0;
        private bool _EXPRESS = false;
        #endregion

        #region "Propiedades"
        public Int64 ID_TARIFA { get => _ID_TARIFA; set => _ID_TARIFA = value; }
        public int SECUENCIA { get => _SECUENCIA; set => _SECUENCIA = value; }

        public decimal VALOR { get => _VALOR; set => _VALOR = value; }
        public decimal M3 { get => _M3; set => _M3 = value; }
        public decimal PESO { get => _PESO; set => _PESO = value; }

        public decimal PRECIO { get => _PRECIO; set => _PRECIO = value; }
        public decimal TOTAL_PAGAR { get => _TOTAL_PAGAR; set => _TOTAL_PAGAR = value; }
        public bool EXPRESS { get => _EXPRESS; set => _EXPRESS = value; }
        #endregion

        public P2D_Tarifario()
        {
            init();
        }

        /* poblar */
        public bool PopulateMyData(out string OnError)
        {
          
            parametros.Clear();
            parametros.Add("M3", this.M3);
            parametros.Add("PESO", this.PESO);
            parametros.Add("EXPRESS", this.EXPRESS);

            var t = sql_puntero.ExecuteSelectOnly<P2D_Tarifario>(sql_puntero.Conexion_Local, 6000, "P2D_EXTRAER_TARIFA", parametros);
            if (t == null)
            {
                OnError = "No existe tarifa con los valores de pesos y volumen ingresados..";
                return false;
            }

            this.ID_TARIFA = t.ID_TARIFA;
            this.SECUENCIA = t.SECUENCIA;
            this.VALOR = t.VALOR;
            this.PRECIO = t.PRECIO;
            this.TOTAL_PAGAR = t.TOTAL_PAGAR;

            OnError = string.Empty;
            return true;
        }
    }
}
