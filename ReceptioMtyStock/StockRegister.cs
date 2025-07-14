using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ControlOPC.Entidades;

namespace ReceptioMtyStock
{
    public class StockRegister : Base
    {
        #region "Variables"

        private Int64 _Id;
        private int _Id_line;
        private int _Id_depot;
        private int _Id_operation;

        private string _operation_user;
        private string _operation_objetc;
        private string _operation_notes;
        private int _Qty;
        private int _Multiplier;
        private int _Create_year;
        private int _Create_month;
        private int _Create_week;
        private bool _Active;
        private DateTime? _Create_Date;

        private string _name;
        private string _notes;
        private string _ccant_ing;
        private string _ccant_egr;
        private string _ctotal;
        private string _cid;
        private string _ccreate_date;
        private string _CCreate_week;
        #endregion

        #region "Propiedades"
        public Int64 Id { get => _Id; set => _Id = value; }
        public int Id_line { get => _Id_line; set => _Id_line = value; }
        public int Id_depot { get => _Id_depot; set => _Id_depot = value; }
        public int Id_operation { get => _Id_operation; set => _Id_operation = value; }
        public string Operation_user { get => _operation_user; set => _operation_user = value; }
        public string Operation_objetc { get => _operation_objetc; set => _operation_objetc = value; }
        public string Operation_notes { get => _operation_notes; set => _operation_notes = value; }
        public int Qty { get => _Qty; set => _Qty = value; }
        public int Multiplier { get => _Multiplier; set => _Multiplier = value; }
        public bool Active { get => _Active; set => _Active = value; }
        public DateTime? Create_Date { get => _Create_Date; set => _Create_Date = value; }
        public int Create_year { get => _Create_year; set => _Create_year = value; }
        public int Create_month { get => _Create_month; set => _Create_month = value; }
        public int Create_week { get => _Create_week; set => _Create_week = value; }
        public string CCreate_week { get => _CCreate_week; set => _CCreate_week = value; }

        public string Name { get => _name; set => _name = value; }
        public string Notes { get => _notes; set => _notes = value; }
        public string Ccant_ing { get => _ccant_ing; set => _ccant_ing = value; }
        public string Ccant_egr { get => _ccant_egr; set => _ccant_egr = value; }
        public string Ctotal { get => _ctotal; set => _ctotal = value; }
        public string Cid { get => _cid; set => _cid = value; }
        public string Ccreate_date { get => _ccreate_date; set => _ccreate_date = value; }
        #endregion


        #region "Constructores"
        public StockRegister()
        {

            base.init();
        }

        public StockRegister(Int64 _id, int _id_line, int _id_depot, int _Id_operation, string _operation_user, string _operation_objetc, string _operation_notes, int _Qty, int _Multiplier, bool _Active, DateTime? _Create_Date)
        {
            this.Id = _id;
            this.Id_line = _id_line;
            this.Id_depot = _id_depot;
            this.Id_operation = _Id_operation;

            this.Operation_user = _operation_user;
            this.Operation_objetc = _operation_objetc;
            this.Operation_notes = _operation_notes;
            this.Qty = _Qty;
            this.Multiplier = _Multiplier;
            this.Active = _Active;
            this.Create_Date = _Create_Date;
        }
        #endregion

        #region "Metodos"

        private static void OnInit()
        {
            sql_pointer = (sql_pointer == null) ? SQLHandler.sql_handler.GetInstance() : sql_pointer;
            parametros = new Dictionary<string, object>();
            v_conexion = Extension.Nueva_Conexion("RECEPTIO");
        }

        public string ValidateStock(out string OnError)
        {

            parametros.Clear();
            parametros.Add("i_id_line", this.Id_line);
            parametros.Add("i_id_depot", this.Id_depot);
            parametros.Add("i_qty", this.Qty);
            parametros.Add("i_id_operation", this.Id_operation);
            parametros.Add("i_create_date", this.Create_Date);
            var db = sql_pointer.ExecuteSelectOnly(v_conexion, 4000, "pc_c_valida_stock_register", parametros);
            if (db == null)
            {
                OnError = "Erro al validar stock metodo: ValidateStock";
                return null;
            }
            else
            {
                if (db.code == 1)
                {
                    OnError = db.message;
                }
                else
                {
                    OnError = string.Empty;
                }

            } 
            return OnError;

        }

        public string ValidateDeleteStock(out string OnError)
        {

            parametros.Clear();
            parametros.Add("i_id", this.Id);       
            var db = sql_pointer.ExecuteSelectOnly(v_conexion, 4000, "pc_c_valida_delete_stock_register", parametros);
            if (db == null)
            {
                OnError = "Erro al validar eliminar registros de stock en metodo: ValidateDeleteStock";
                return null;
            }
            else
            {
                if (db.code == 1)
                {
                    OnError = db.message;
                }
                else
                {
                    OnError = string.Empty;
                }

            }
            return OnError;

        }

        private int? PreValidations(out string msg)
        {

            if (this.Id_line <= 0)
            {
                msg = "Especifique el id de la linea naviera";
                return 0;
            }
            if (this.Id_depot <= 0)
            {
                msg = "Especifique el id de la bodega principal";
                return 0;
            }

            if (this.Id_operation <= 0)
            {
                msg = "Especifique el id de la operación (+/-) a realizar";
                return 0;
            }

            if (string.IsNullOrEmpty(this.Operation_user))
            {
                msg = "Especifique el usuario creador de la transaccion";
                return 0;
            }

            if (this.Qty == 0)
            {
                msg = "La cantidad de la transacción, no puede ser cero";
                return 0;
            }

            if (!this.Create_Date.HasValue)
            {
               
                msg = "La fecha de la transacción no es valida";
                return 0;
            }

            msg = this.ValidateStock(out msg);
            if (msg != string.Empty)
            {
                return 0;
            }

            msg = string.Empty;
            return 1;
        }

        private int? PreValidationsDelete(out string msg)
        {

            if (this.Id <= 0)
            {
                msg = "Especifique el id de la transacción";
                return 0;
            }
           
            msg = this.ValidateDeleteStock(out msg);
            if (msg != string.Empty)
            {
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

            parametros.Clear();
            parametros.Add("i_id_line", this.Id_line);
            parametros.Add("i_id_depot", this.Id_depot);
            parametros.Add("i_id_operation", this.Id_operation);
            parametros.Add("i_operation_user", this.Operation_user);
            parametros.Add("i_operation_notes", this.Operation_notes);
            parametros.Add("i_qty", this.Qty);
            parametros.Add("i_create_date", this.Create_Date);

            using (var scope = new System.Transactions.TransactionScope())
            {

                var db = sql_pointer.ExecuteInsertUpdateDeleteReturn(v_conexion, 4000, "pc_i_stock_register", parametros, out OnError);
                if (!db.HasValue || db.Value < 0)
                {

                    return null;
                }
                OnError = string.Empty;
                scope.Complete();
                return db.Value;
            }


        }

        public Int64? Delete(out string OnError)
        {

            if (this.PreValidationsDelete(out OnError) != 1)
            {
                return -1;
            }

            parametros.Clear();
            parametros.Add("i_id", this.Id);

            using (var scope = new System.Transactions.TransactionScope())
            {
                var db = sql_pointer.ExecuteInsertUpdateDelete(v_conexion, 4000, "pc_d_stock_register", parametros, out OnError);
                if (!db.HasValue || db.Value < 0)
                {
                    return null;
                }
                OnError = string.Empty;
                scope.Complete();
                return db.Value;
            }  
        }


        public static List<StockRegister> ListStockRegister(Int64 _i_id_line, Int64 _i_id_depot)
        {
            OnInit();
            string msg;
            parametros.Clear();
            parametros.Add("i_id_line", _i_id_line);
            parametros.Add("i_id_depot", _i_id_depot);
            return sql_pointer.ExecuteSelectControl<StockRegister>(v_conexion, 2000, "pc_c_stock_register", parametros, out msg);
        }

        public static List<StockRegister> RptListStockRegister(Int64 _i_id_line, Int64 _i_id_depot, DateTime i_fecha_desde, DateTime i_fecha_hasta)
        {
            OnInit();
            string msg;
            parametros.Clear();
            parametros.Add("i_id_line", _i_id_line);
            parametros.Add("i_id_depot", _i_id_depot);
            parametros.Add("i_fecha_desde", i_fecha_desde);
            parametros.Add("i_fecha_hasta", i_fecha_hasta);
            return sql_pointer.ExecuteSelectControl<StockRegister>(v_conexion, 2000, "pc_rpt_stock_register", parametros, out msg);
        }


        #endregion


    }
}
