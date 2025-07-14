using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlConexion;
using System.Data;
using System.Reflection;

namespace BillionEntidades
{
    public class Cls_Sol_Estado : Cls_Bil_Base
    {
        public string REFERENCIA { get; set; }
        public string CODIGO_BUQUE { get; set; }
        public string BUQUE { get; set; }
        public string ESTADO { get; set; }

        public Cls_Sol_Estado()
        {
            init();
        }

        private static void OnInit(string Base)
        {
            sql_puntero = (sql_puntero == null) ? Cls_Conexion.Conexion() : sql_puntero;
            parametros = new Dictionary<string, object>();
            nueva_conexion = Cls_Conexion.Nueva_Conexion(Base);
        }

        public bool PopulateMyData(out string OnError)
        {
            OnInit("N5");

            parametros.Clear();
            parametros.Add("REFERENCIA", this.REFERENCIA);

            var t = sql_puntero.ExecuteSelectOnly<Cls_Sol_Estado>(nueva_conexion, 6000, "[Bill].[SOL_ESTADO_NAVE]", parametros);
            if (t == null)
            {
                OnError = string.Format("No existe información de referencia a consultar: {0}", this.REFERENCIA);
                return false;
            }

            this.REFERENCIA = t.REFERENCIA;
            this.CODIGO_BUQUE = t.CODIGO_BUQUE;
            this.BUQUE = t.BUQUE;
            this.ESTADO = t.ESTADO;


            OnError = string.Empty;
            return true;
        }
    }
}
