using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlConexion;

namespace BillionEntidades
{
    public class P2D_Precios : Cls_Bil_Base
    {
        #region "Variables"

        private Int64 _IDX_ROW_ID = 0;
        private string _ID_PRECIOS = string.Empty;
        private string _DESC_PRECIO = string.Empty;
        private decimal _VALOR = 0;
        private bool _ESTADO = false;

        #endregion

        #region "Propiedades"

        public Int64 IDX_ROW_ID { get => _IDX_ROW_ID; set => _IDX_ROW_ID = value; }
        public string ID_PRECIOS { get => _ID_PRECIOS; set => _ID_PRECIOS = value; }
        public string DESC_PRECIO { get => _DESC_PRECIO; set => _DESC_PRECIO = value; }
        public decimal VALOR { get => _VALOR; set => _VALOR = value; }
        public bool ESTADO { get => _ESTADO; set => _ESTADO = value; }

        #endregion


        public P2D_Precios()
        {
            init();
        }

      
        /*poblar */
        public bool PopulateMyData(out string OnError)
        {

            parametros.Clear();
           
            var t = sql_puntero.ExecuteSelectOnly<P2D_Precios>(sql_puntero.Conexion_Local, 6000, "P2D_CONSULTA_PRECIO", null);
            if (t == null)
            {
                OnError = "No existe precios establecidos para el servicio de P2D";
                return false;
            }

            this.ID_PRECIOS = t.ID_PRECIOS;
            this.DESC_PRECIO = t.DESC_PRECIO;
            this.VALOR = t.VALOR;
            this.ESTADO = t.ESTADO;

            OnError = string.Empty;
            return true;
        }

    }
}
