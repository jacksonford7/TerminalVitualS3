using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlConexion;
using PasePuerta;


namespace BillionEntidades
{
    public class Cls_Bil_PasePuertaCFS_Validacion : Cls_Bil_Base
    {
        #region "Variables"


        private Int64 _NUMERO_PASE_N4 = 0;
        private string _UBICACION = string.Empty;
        private string _MENSAJE = string.Empty;
        private bool _ESTADO = false;
        #endregion

        #region "Propiedades"


        public Int64 NUMERO_PASE_N4 { get => _NUMERO_PASE_N4; set => _NUMERO_PASE_N4 = value; }
        public string UBICACION { get => _UBICACION; set => _UBICACION = value; }
        public string MENSAJE { get => _MENSAJE; set => _MENSAJE = value; }
        public bool ESTADO { get => _ESTADO; set => _ESTADO = value; }

        private static String v_mensaje = string.Empty;

        #endregion
    }
}
