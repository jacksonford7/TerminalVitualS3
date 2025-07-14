using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ControlOPC.Entidades;

namespace ReceptioMtyStock
{
    public class Line_Depot : Base 
    {
        #region "Variables"

            private Int64 _Id_line_depo;
            private int _Id_line;
            private int _Id_depot;
            private string _name;
            private string _estado;
            private int _Stock ;
        #endregion

        #region "Propiedades"
        public Int64 Id_line_depo { get => _Id_line_depo; set => _Id_line_depo = value; }
            public int Id_line { get => _Id_line; set => _Id_line = value; }
            public int Id_depot { get => _Id_depot; set => _Id_depot = value; }
            public string Name { get => _name; set => _name = value; }
            public string Estado { get => _estado; set => _estado = value; }
            public int Stock { get => _Stock; set => _Stock = value; }
        #endregion

        #region "Constructores"
        public Line_Depot()
            {

                base.init();
            }

            public Line_Depot(int _id_line_depo, int _id_line, int _id_depot, bool _active, string _create_user, string _mod_user)
            {
                this.Id_line_depo = _id_line_depo;
                this.Id_line = _id_line;
                this.Id_depot = _id_depot;
                this.active = _active;
                this.Create_user = _create_user;
                this.Mod_user = _mod_user;

            }
        #endregion

        #region "Metodos"

            private static void OnInit()
            {
                sql_pointer = (sql_pointer == null) ? SQLHandler.sql_handler.GetInstance() : sql_pointer;
                parametros = new Dictionary<string, object>();
                v_conexion = Extension.Nueva_Conexion("RECEPTIO");
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

                if (string.IsNullOrEmpty(this.Create_user))
                {
                    msg = "Especifique el usuario creador de la transaccion";
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
                parametros.Add("i_id_line_depo", Id_line_depo);
                parametros.Add("i_id_line", Id_line);
                parametros.Add("i_id_depot", Id_depot);
                parametros.Add("i_active", active);
                parametros.Add("i_create_user", Create_user);

                using (var scope = new System.Transactions.TransactionScope())
                {
                    
                    var db = sql_pointer.ExecuteInsertUpdateDeleteReturn(v_conexion, 4000, "pc_i_line_depots", parametros, out OnError);
                    if (!db.HasValue || db.Value < 0)
                    {
                     
                        return null;
                    }
                    OnError = string.Empty;
                    scope.Complete();
                    return db.Value;
                }
                
              
            }

            public bool Delete(out string OnError)
            {
                parametros.Clear();
                parametros.Add("i_id_line_depo", this.Id_line_depo);
                parametros.Add("i_mod_user", this.Mod_user);
                using (var scope = new System.Transactions.TransactionScope())
                {                 
                    var db = sql_pointer.ExecuteInsertUpdateDelete(v_conexion, 4000, "pc_d_line_depots", parametros, out OnError);
                    if (!db.HasValue || db.Value < 0)
                    {
                        return false;
                    }      
                   scope.Complete();
                }
                return true;
            }


        public static List<Line_Depot> ListLineDepot(Int64 _i_id_line)
            {
                OnInit();
                string msg;
                parametros.Clear();
                parametros.Add("i_id_line", _i_id_line); 
                return sql_pointer.ExecuteSelectControl<Line_Depot>(v_conexion, 2000, "pc_c_line_depots", parametros, out msg);
            }

        public static List<Line_Depot> ListLineDepotReports(Int64 _i_id_line)
        {
            OnInit();
            string msg;
            parametros.Clear();
            parametros.Add("i_id_line", _i_id_line);
            return sql_pointer.ExecuteSelectControl<Line_Depot>(v_conexion, 2000, "pc_c_line_depots_reports", parametros, out msg);
        }


        public bool ExistLineDepot(out string OnError)
            {
                
                if (this.Id_line <= 0)
                {
                    OnError = "Debe establecer el campo ID de la línea naviera";
                    return false;
                }
                if (this.Id_depot <= 0)
                {
                    OnError = "Debe establecer el campo ID de la bodega";
                    return false;
                }

                parametros.Clear();
                parametros.Add("i_id_line", this.Id_line);
                parametros.Add("i_id_depot", this.Id_depot);

                var t = sql_pointer.ExecuteSelectOnly<Line_Depot>(v_conexion, 4000, "pc_c_exist_line_depots", parametros);
                if (t == null)
                {
                    OnError = string.Empty;
                    return false;
                }
                this.Id_line = t.Id_line;
                this.Id_line_depo = t.Id_line_depo;
                this.Id_depot = t._Id_depot;
                this.Name = t.Name;
                this.Estado = t.Estado;  
                this.Create_date = t.Create_date;
                this.Create_user = t.Create_user;  
                this.Mod_user = t.Mod_user;
                this.Mod_date = t.Mod_date;
              
                OnError = string.Empty;
                return true;
            }


        public bool ExistMovStock(out string OnError)
        {

            if (this.Id_line <= 0)
            {
                OnError = "Debe establecer el campo ID de la línea naviera";
                return false;
            }
            if (this.Id_depot <= 0)
            {
                OnError = "Debe establecer el campo ID de la bodega";
                return false;
            }

            parametros.Clear();
            parametros.Add("i_id_line", this.Id_line);
            parametros.Add("i_id_depot", this.Id_depot);

            var t = sql_pointer.ExecuteSelectOnly<Line_Depot>(v_conexion, 4000, "pc_c_movements_depots_stock", parametros);
            if (t == null)
            {
                OnError = string.Empty;
                return false;
            }
            this.Stock = t.Stock;
           
            OnError = string.Empty;
            return true;
        }

        #endregion



    }

    
}
