using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlOPC.Entidades
{
    public class Vessel_Crane : Base
    {

        #region "Variables"
        private long _id = 0;
        private long _vessel_visit_id = 0;
        private long _crane_id = 0;
        private string _crane_name = string.Empty;
        private int _crane_time_qty = 0;
        private DateTime? _starWork = null;
        private DateTime? _endwork = null;
        private DateTime? _datework = null;
        private string _notes = string.Empty;
        private bool active = false;
        private string _status = string.Empty;

     
        //private string 
        #endregion

        #region "Propiedades"

        public long Id { get => _id; set => _id = value; }
        public long VesselVisit_ID { get => _vessel_visit_id; set => _vessel_visit_id = value; }
        public long Crane_Gkey{ get => _crane_id; set => _crane_id = value;       }
        public string Crane_name {  get => _crane_name; set => _crane_name = value;}
        public int Crane_time_qty { get => _crane_time_qty; set => _crane_time_qty = value; }
        public DateTime? StarWork { get => _starWork; set => _starWork = value; }
        public DateTime? EndWork { get => _endwork; set => _endwork = value; }
        public DateTime? DateWork { get => _datework; set => _datework = value; }
        public string Notes { get => _notes; set => _notes = value; }
        public bool Active { get => active; set => active = value; }
        public string Status { get => _status; set => _status = value; }
        public bool vlock { get; set; }
        public bool vunlock { get; set; }
        public long vessel_visit_id { get => _vessel_visit_id; set => _vessel_visit_id = value; }
        public long crane_id { get => _crane_id; set => _crane_id = value; }
        #endregion

        private static void OnInit()
        {
          
            parametros = new Dictionary<string, object>();

        }

        //PC_I_Turn
        public Vessel_Crane()
        {
            base.init();
        }


        public Vessel_Crane(Int64 _vessel_visit_id)
        {
            this._vessel_visit_id = _vessel_visit_id;
        }
        public static List<Vessel_Crane> ListVesselCrane(long _vessel_visit_id, out string OnError)
        {
            return sql_pointer.ExecuteSelectControl<Vessel_Crane>(sql_pointer.basic_con, 2000, "PC_C_Vessel_crane", new Dictionary<string, object>() { { "i_vessel_visit_id", _vessel_visit_id } }, out OnError);
        }

        public Int64? Save(out string OnError)
        {
            if (this.PreValidations(out OnError) != 1)
            {
                return -1;
            }
            parametros.Clear();
            parametros.Add("i_vessel_visit_id", VesselVisit_ID);
            parametros.Add("i_crane_id", Crane_Gkey);
            parametros.Add("i_crane_name", Crane_name);
            parametros.Add("i_crane_time_qty", Crane_time_qty);
            parametros.Add("i_starwork", StarWork);
            parametros.Add("i_endwork", EndWork);
            parametros.Add("i_datework", DateWork);
            parametros.Add("i_notes", Notes);
            parametros.Add("i_status", Status);
            parametros.Add("i_create_user", Create_user);
            var db = sql_pointer.ExecuteInsertUpdateDeleteReturn(sql_pointer.basic_con, 4000, "PC_I_Vessel_crane", parametros, out OnError);
            if (!db.HasValue || db.Value < 0)
            {
                //null error en base de datos, el mensaje ya fue en onError
                return null;
            }
            OnError = string.Empty;
            return db.Value;
            // PC_I_Turn
        }

        public Int64? Update(out string OnError)
        {
            if (this.PreValidations(out OnError) != 1)
            {
                return -1;
            }
            parametros.Clear();
            parametros.Add("i_id", Id);
            //parametros.Add("i_vessel_visit_id", VesselVisit_ID);
            //parametros.Add("i_crane_id", Crane_Gkey);
            //parametros.Add("i_crane_name", Crane_name);
            //parametros.Add("i_crane_time_qty", Crane_time_qty);
            //parametros.Add("i_starwork", StarWork);
            //parametros.Add("i_endwork", EndWork);
            //parametros.Add("i_datework", DateWork);
            //parametros.Add("i_notes", Notes);
            parametros.Add("i_status", Status);
            parametros.Add("i_mod_user", Mod_user );
            var db = sql_pointer.ExecuteInsertUpdateDeleteReturn(sql_pointer.basic_con, 4000, "PC_U_Vessel_crane", parametros, out OnError);
            if (!db.HasValue || db.Value < 0)
            {
                //null error en base de datos, el mensaje ya fue en onError
                return null;
            }
            OnError = string.Empty;
            return db.Value;
            // PC_I_Turn
        }

        private int? PreValidations(out string msg)
        {
            //1.->Validaciones de objetos.
            if (this._vessel_visit_id <= 0)
            {
                msg = "Especifique el id de visita";
                return 0;
            }
            if (this._crane_id <= 0)
            {
                msg = "Especifique el id de la grúa";
                return 0;
            }
            if (this._crane_name == string.Empty)
            {
                msg = "Especifique el nombre de la grúa";
                return 0;
            }

            if (_crane_time_qty < 1)
            {
                msg = "El tiempo transcurrido debe ser mayor a cero";
                return 0;
            }

            
            if (_id == 0)
            {
                Id = -1;
            }
            if (this._starWork.HasValue && this._endwork.HasValue)
            {
                if (this._endwork < this._starWork)
                {
                    msg = "La fecha de inicio de trabajo no puede ser mayor a la final de trabajo ";
                    return 0;
                }
            }
            msg = string.Empty;
            return 1;
        }


        public bool PopulateMyData(out string OnError)
        {
            //cargar todos los datos este documento
            if (this.Id <= 0)
            {
                OnError = "Debe establecer el campo ID de la Grúa";
                return false;
            }
            parametros.Clear();
            parametros.Add("i_id", this.Id);

            var t = sql_pointer.ExecuteSelectOnly<Vessel_Crane>(sql_pointer.basic_con, 4000, "PC_C_crane", parametros);
            if (t == null)
            {
                OnError = "No fue posible obtener los datos de la visita";
                return false;
            }

            this.Id = t.Id;
            this.vessel_visit_id = t.vessel_visit_id;
            this.crane_id = t.crane_id;
            this.Crane_name = t.Crane_name;
            this.Crane_time_qty = t.Crane_time_qty;
            this.Status = t.Status;
            this.Active = t.Active;
            this.Create_date = t.Create_date;
            this.Create_user = t.Create_user;
            this.StarWork = t.StarWork;
            this.EndWork = t.EndWork;
            this.DateWork = t.DateWork;
            this.Notes = t.Notes;

            OnError = string.Empty;
            return true;
        }
    }
}
