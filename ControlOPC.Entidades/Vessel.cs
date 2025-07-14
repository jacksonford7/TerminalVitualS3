using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Globalization;

namespace ControlOPC.Entidades
{
    public class Vessel : Base
    {

        #region "Variables"

        private long _GKEY = 0;
        private string _REFERENCE = string.Empty;
        private string _ACTIVE_N4 = string.Empty;
        private DateTime? _ESTIMATED_ARRIVAL;
        private DateTime? _ESTIMATED_DEPART;
        private string _NAME = string.Empty;
        private DateTime? _ACTUAL_ARRIVAL;
        private DateTime? _ACTUAL_DEPART;      
        private string _VOYAGE = string.Empty;
        private DateTime? _LAST_TRANSACTION;
        private DateTime? _DEADLINE;
        private string _BERTH = string.Empty;
        private DateTime? _INITIAL_OPERATION_DATE;
        private DateTime? _FINAL_OPERATION_DATE;
        private string _VOYAGE_IN = string.Empty;
        private string _VOYAGE_OUT = string.Empty;

        private static string _mensaje = string.Empty;


        #endregion

        #region "Propiedades"


        public long GKEY
        {
            get
            {
                return this._GKEY;
            }
            set
            {
                this._GKEY = value;
            }
        }

        public string REFERENCE
        {
            get
            {
                return this._REFERENCE;
            }
            set
            {
                this._REFERENCE = value;
            }
        }

        public string ACTIVE_N4
        {
            get
            {
                return this._ACTIVE_N4;
            }
            set
            {
                this._ACTIVE_N4 = value;
            }
        }

        public DateTime? ETA
        {
            get
            {
                return this._ESTIMATED_ARRIVAL;
            }
            set
            {
                this._ESTIMATED_ARRIVAL = value;
            }
        }

        public DateTime? ETD
        {
            get
            {
                return this._ESTIMATED_DEPART;
            }
            set
            {
                this._ESTIMATED_DEPART = value;
            }
        }

        public string NAME
        {
            get
            {
                return this._NAME;
            }
            set
            {
                this._NAME = value;
            }
        }

      
        public DateTime? ATA
        {
            get
            {
                return this._ACTUAL_ARRIVAL;
            }
            set
            {
                this._ACTUAL_ARRIVAL = value;
            }
        }

        public DateTime? ATD
        {
            get
            {
                return this._ACTUAL_DEPART;
            }
            set
            {
                this._ACTUAL_DEPART = value;
            }
        }

        public string VOYAGE
        {
            get
            {
                return this._VOYAGE;
            }
            set
            {
                this._VOYAGE = value;
            }
        }

        public DateTime? LAST_TRANSACTION
        {
            get
            {
                return this._LAST_TRANSACTION;
            }
            set
            {
                this._LAST_TRANSACTION = value;
            }
        }

        public DateTime? DEADLINE
        {
            get
            {
                return this._DEADLINE;
            }
            set
            {
                this._DEADLINE = value;
            }
        }

        public string BERTH
        {
            get
            {
                return this._BERTH;
            }
            set
            {
                this._BERTH = value;
            }
        }

        public DateTime? START_WORK
        {
            get
            {
                return this._INITIAL_OPERATION_DATE;
            }
            set
            {
                this._INITIAL_OPERATION_DATE = value;
            }
        }

        public DateTime? END_WORK
        {
            get
            {
                return this._FINAL_OPERATION_DATE;
            }
            set
            {
                this._FINAL_OPERATION_DATE = value;
            }
        }

        public string VOYAGE_IN
        {
            get
            {
                return this._VOYAGE_IN;
            }
            set
            {
                this._VOYAGE_IN = value;
            }
        }

        public string VOYAGE_OUT
        {
            get
            {
                return this._VOYAGE_OUT;
            }
            set
            {
                this._VOYAGE_OUT = value;
            }
        }


        #endregion

        public Vessel()
        {
            init();
        }

        private static void OnInit()
        {
            sql_pointer = (sql_pointer == null) ? SQLHandler.sql_handler.GetInstance() : sql_pointer;
        }


        public static List<Vessel> ListaVessel(string pREFERENCE)
        {

            OnInit();

            var v_conexion = app_configurations.get_configuration("N5_DB");

            Dictionary<string, object> Parametros = new Dictionary<string, object>();
           
            Parametros.Add("REFERENCE", pREFERENCE == null? "": pREFERENCE);
           

            return sql_pointer.ExecuteSelectControl<Vessel>(v_conexion.value, 2000, "PC_C_VESSEL", Parametros, out _mensaje);
        }

    }
}
