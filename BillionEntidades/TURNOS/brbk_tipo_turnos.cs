using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlConexion;

namespace BillionEntidades
{
    public class brbk_tipo_turnos : Cls_Bil_Base
    {
        #region "Variables"

        private Int64 _ID ;
        private string _TIPO = string.Empty;


        #endregion

        #region "Propiedades"
        public Int64 ID { get => _ID; set => _ID = value; }
        public string TIPO { get => _TIPO; set => _TIPO = value; }

        #endregion


        public brbk_tipo_turnos()
        {
            init();
        }

        public static List<brbk_tipo_turnos> CboTipoTurnos(out string OnError)
        {
            parametros.Clear();
            return sql_puntero.ExecuteSelectControl<brbk_tipo_turnos>(sql_puntero.Conexion_Local, 6000, "[BRBK].[CARGA_TIPO_TURNOS]", null, out OnError);
        }

     

    }
}
