using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlConexion;

namespace BillionEntidades
{

    public class P2D_MULTI_buscar_tarifas : Cls_Bil_Base
    {
        #region "Propiedades"

        public Int64 ID_TARIFA { get; set; }
        public Int64 ID_CIUDAD { get; set; }
        public Int64 ID_ZONA { get; set; }


        public string TIPO { get ; set ; }
        public string INVOICETYPE { get; set; }

        public decimal CANTIDAD { get ; set; }
        public decimal FACTOR { get; set; }
        public decimal VALOR { get; set; }
        public decimal IVA { get; set; }
        public string CODIGO_TARIJA_N4 { get ; set; }
        public bool ESTADO { get ; set; }

      

        public string USUARIO { get ; set; }
        public DateTime? FECHA_REGISTRO { get ; set ; }

        public decimal VALOR_IVA { get; set; }
        public decimal VALOR_TOTAL { get; set ; }
        public string DESCRIPCION { get; set; }
        public string ID_TIPO { get; set; }
        #endregion

        public P2D_MULTI_buscar_tarifas()
        {
            init();



        }

        public static P2D_MULTI_buscar_tarifas GetServicio(Int64 _ID_CIUDAD, int _CANTIDAD, string _TIPO)
        {

            parametros.Clear();
            parametros.Add("ID_CIUDAD", _ID_CIUDAD);
            parametros.Add("CANTIDAD", _CANTIDAD);
            parametros.Add("TIPO", _TIPO);

            return sql_puntero.ExecuteSelectOnly<P2D_MULTI_buscar_tarifas>(sql_puntero.Conexion_Local, 6000, "P2D_MULTI_buscar_tarifas", parametros);
        }



    }
}
