using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ControlOPC.Entidades;

namespace ClsNotasCreditos
{
    public class credit_level_approval : Base
    {

        #region "Variables"

        private Int64 _nc_id;
        private Int64 _id_level;
        private Int64 _id_group;
        private int _IdUsuario;
        private string _Usuario = string.Empty;
        private int _level;
        private bool _aprobado;
        private decimal _nc_total;
        private int _id_concept;
        #endregion

        #region "Propiedades"

        public Int64 nc_id { get => _nc_id; set => _nc_id = value; }
        public Int64 id_level { get => _id_level; set => _id_level = value; }
        public Int64 id_group { get => _id_group; set => _id_group = value; }
        public int IdUsuario { get => _IdUsuario; set => _IdUsuario = value; }
        public int level { get => _level; set => _level = value; }
        public int id_concept { get => _id_concept; set => _id_concept = value; }
        public bool aprobado { get => _aprobado; set => _aprobado = value; }
        public string Usuario { get => _Usuario; set => _Usuario = value; }
        public decimal nc_total { get => _nc_total; set => _nc_total = value; }
        #endregion


        public credit_level_approval()
        {
            init();
        }

        private static void OnInit()
        {
            sql_pointer = (sql_pointer == null) ? SQLHandler.sql_handler.GetInstance() : sql_pointer;
            parametros = new Dictionary<string, object>();
            v_conexion = Extension.Nueva_Conexion("NOTA_CREDITO");
        }

        public credit_level_approval(Int64 _nc_id, Int64 _id_level, Int64 _id_group, int _IdUsuario, int _level, string _Usuario, bool _aprobado, int _id_concept)
        {
            this.nc_id = _nc_id;
            this.id_level = _id_level;
            this.id_group = _id_group;
            this.IdUsuario = _IdUsuario;
            this.level = _level;
            this.Usuario = _Usuario;
            this.aprobado = _aprobado;
            this.id_concept = _id_concept;
        }

        private int? PreValidations(out string msg)
        {

            if (this.nc_id <= 0)
            {
                msg = "Especifique el id de la nota de credito";
                return 0;
            }

            if (this.id_concept <= 0)
            {
                msg = "Especifique el id del concepto o motivo de la nota de credito";
                return 0;
            }

            if (string.IsNullOrEmpty(this.Create_user))
            {
                msg = "Especifique el usuario que crea la transacción";
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
            parametros.Add("i_nc_id", this.nc_id);
            parametros.Add("i_nc_total", this.nc_total);
            parametros.Add("i_state", this.state);
            parametros.Add("i_create_user", this.Create_user);
            parametros.Add("i_id_concept", this.id_concept);

            var db = sql_pointer.ExecuteInsertUpdateDeleteReturn(v_conexion, 4000, "nc_i_credit_level_approval", parametros, out OnError);
            if (!db.HasValue || db.Value < 0)
            {
                return null;
            }
            OnError = string.Empty;
            return db.Value;

        }

    }



}
