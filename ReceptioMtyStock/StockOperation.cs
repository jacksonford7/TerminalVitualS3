using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ControlOPC.Entidades;

namespace ReceptioMtyStock
{
    public class StockOperation : Base
    {

        #region "Variables"

        private int _Id_operation;
        private string _Operation;
        private int _Multiplier;
        private string _Notes;
        private bool _Active;
        private static String v_mensaje = string.Empty;
        private bool _Screen;

        #endregion

        #region "Propiedades"
        public int Id_operation { get => _Id_operation; set => _Id_operation = value; }
        public string Operation { get => _Operation; set => _Operation = value; }
        public int Multiplier { get => _Multiplier; set => _Multiplier = value; }
        public string Notes { get => _Notes; set => _Notes = value; }
        public bool Active { get => _Active; set => _Active = value; }
        public bool Screen { get => _Screen; set => _Screen = value; }
        #endregion


        #region "Constructores"
        public StockOperation()
        {

            base.init();
        }

        public StockOperation(int _Id_operation, string _Operation, int _Multiplier, string _Notes, bool _Active)
        {
            this.Id_operation = _Id_operation;
            this.Operation = _Operation;
            this.Multiplier = _Multiplier;
            this.Notes = _Notes;
            this.Active = _Active;
          

        }
        #endregion

        #region "Metodos"

        private static void OnInit()
        {
            sql_pointer = (sql_pointer == null) ? SQLHandler.sql_handler.GetInstance() : sql_pointer;
            parametros = new Dictionary<string, object>();
            v_conexion = Extension.Nueva_Conexion("RECEPTIO");
        }


        public static List<StockOperation> ListStockOperation()
        {
            OnInit();
            return sql_pointer.ExecuteSelectControl<StockOperation>(v_conexion, 2000, "pc_c_stock_operation", null, out v_mensaje);
        }

        public int GetOperation(int _Id_Operation, out string OnError)
        {

          
            parametros.Clear();
            parametros.Add("i_id_operation", _Id_Operation);

            var t = sql_pointer.ExecuteSelectOnly<StockOperation>(v_conexion, 4000, "pc_get_stock_operation", parametros);
            if (t == null)
            {
                OnError = "ocurrio un error al obtener multiplicador";
                this.Multiplier = 0;
                return this.Multiplier;
            }
            this.Multiplier = t.Multiplier;
            
            OnError = string.Empty;
            return this.Multiplier;
        }



        #endregion

    }
}
