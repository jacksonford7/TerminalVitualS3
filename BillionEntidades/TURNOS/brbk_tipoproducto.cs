using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlConexion;

namespace BillionEntidades
{
    public class brbk_tipoproducto : Cls_Bil_Base
    {
        #region "Variables"

        private int _CODIGO ;
        private string _DESCRIPCION = string.Empty;


        #endregion

        #region "Propiedades"
        public int CODIGO { get => _CODIGO; set => _CODIGO = value; }
        public string DESCRIPCION { get => _DESCRIPCION; set => _DESCRIPCION = value; }

        #endregion


        public brbk_tipoproducto()
        {
            init();
        }

        public static List<brbk_tipoproducto> CboTipoProducto(bool TODOS, out string OnError)
        {
            parametros.Clear();
            parametros.Add("TODOS", TODOS);
            return sql_puntero.ExecuteSelectControl<brbk_tipoproducto>(sql_puntero.Conexion_Local, 6000, "[BRBK].[TIPO_PRODUCTO]", parametros, out OnError);
        }
    }
}
