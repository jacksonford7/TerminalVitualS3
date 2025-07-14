using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ControlOPC.Entidades;

namespace ClsAutorizaciones
{
    public class Contenedor_Detalle : Base
    {
        #region "Variables"


        private Int64 _ID = 0;
        private Int64 _SECUENCIA=0;
        private string _CONTENEDOR = string.Empty;
        private Int64 _GKEY = 0;
        private bool _ESTADO = false;
        private string _PROCESO = string.Empty;
        private string _GRUPO = string.Empty;
        private string _REF_FINAL = string.Empty;
        #endregion

        #region "Propiedades"
        public Int64 ID { get => _ID; set => _ID = value; }
        public Int64 SECUENCIA { get => _SECUENCIA; set => _SECUENCIA = value; }
        public string CONTENEDOR { get => _CONTENEDOR; set => _CONTENEDOR = value; }
        public Int64 GKEY { get => _GKEY; set => _GKEY = value; }
        public bool ESTADO { get => _ESTADO; set => _ESTADO = value; }
        public string PROCESO { get => _PROCESO; set => _PROCESO = value; }
        public string GRUPO { get => _GRUPO; set => _GRUPO = value; }
        public string REF_FINAL { get => _REF_FINAL; set => _REF_FINAL = value; }
        #endregion


        public Contenedor_Detalle()
        {
            init();
            OnInit();

        }

        private static void OnInit()
        {
            sql_pointer = (sql_pointer == null) ? SQLHandler.sql_handler.GetInstance() : sql_pointer;
            parametros = new Dictionary<string, object>();

            v_conexion = Extension.Nueva_Conexion("PORTAL_N4");
        }

        private int? PreValidations(out string msg)
        {

            if (this.ID <= 0)
            {
                msg = "Especifique el id de la transacción";
                return 0;
            }

            if (this.GKEY <= 0)
            {
                msg = "Especifique el GKEY del contenedor";
                return 0;
            }

            if (string.IsNullOrEmpty(this.CONTENEDOR))
            {
                msg = "Especifique el contenedor";
                return 0;
            }

           
            msg = string.Empty;
            return 1;
        }


        public Int64? Save(out string OnError)
        {
            if (this.PreValidations(out OnError) != 1)
            {
                return -1;
            }

           // OnInit();

            parametros.Clear();
            parametros.Add("ID", this.ID);
            parametros.Add("CONTENEDOR", this.CONTENEDOR);
            parametros.Add("GKEY", this.GKEY);
            parametros.Add("ESTADO", this.ESTADO);
            parametros.Add("GRUPO", this.GRUPO);
            parametros.Add("REF_FINAL", this.REF_FINAL);

            var db = sql_pointer.ExecuteInsertUpdateDeleteReturn(v_conexion, 6000, "RVA_GRABA_CONTENEDOR_DET", parametros, out OnError);
            if (!db.HasValue || db.Value < 0)
            {
                return null;
            }
            OnError = string.Empty;         
            return db.Value;
            


          

        }

    }
}
