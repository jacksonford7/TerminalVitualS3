using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlConexion;

namespace BillionEntidades
{
    public class brbk_depositos : Cls_Bil_Base
    {
        #region "Variables"

        private string _CODIGO = string.Empty;
        private string _DESCRIPCION = string.Empty;


        #endregion

        #region "Propiedades"
        public string CODIGO { get => _CODIGO; set => _CODIGO = value; }
        public string DESCRIPCION { get => _DESCRIPCION; set => _DESCRIPCION = value; }

        #endregion


        public brbk_depositos()
        {
            init();
        }

        public static List<brbk_depositos> CboBodega(bool TODOS, out string OnError)
        {
            parametros.Clear();
            parametros.Add("TODOS", TODOS);
            return sql_puntero.ExecuteSelectControl<brbk_depositos>(sql_puntero.Conexion_Local, 6000, "[BRBK].[CARGA_BODEGA]", parametros, out OnError);
        }

     

    }
}
