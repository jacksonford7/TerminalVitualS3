using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlConexion;


namespace BillionEntidades
{
    public class Cls_Valida_Servicio_Naves : Cls_Bil_Base
    {
        #region "Variables"

        private int _resultado;
        private string _NAVE;
        private string _EVENTO;

        #endregion

        #region "Propiedades"

        public int resultado { get => _resultado; set => _resultado = value; }
        public string NAVE { get => _NAVE; set => _NAVE = value; }
        public string EVENTO { get => _EVENTO; set => _EVENTO = value; }

        #endregion

        public Cls_Valida_Servicio_Naves()
        {
            init();
        }

        private static void OnInit()
        {
            sql_puntero = (sql_puntero == null) ? Cls_Conexion.Conexion() : sql_puntero;
            parametros = new Dictionary<string, object>();
            nueva_conexion = Cls_Conexion.Nueva_Conexion("N5");
        }

        public bool PopulateMyData(out string OnError)
        {
            OnInit();
            parametros.Clear();
            parametros.Add("NAVE", this.NAVE);
            parametros.Add("EVENTO", this.EVENTO);

            var t = sql_puntero.ExecuteSelectOnly<Cls_Valida_Servicio_Naves>(nueva_conexion, 6000, "[Bill].[Validacion_Servicio_Naves]", parametros);
            if (t == null)
            {
                OnError = "No fue posible validar el servicio de Inspección Subacuática";
                return false;
            }

            this.resultado = t.resultado;

            OnError = string.Empty;
            return true;
        }

    }
}
