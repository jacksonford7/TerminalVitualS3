using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ControlOPC.Entidades;

namespace ClsNotasCreditos
{
    public class level_approval_detail : Base 
    {
        #region "Variables"

        private Int64 _id_level;
        private Int64 _id_group;

        private int _sequence;
        private int _level;

   
        private string _group_name = string.Empty;
        private string _usuarios= string.Empty;
      

        #endregion

        #region "Propiedades"

        public Int64 id_level { get => _id_level; set => _id_level = value; }
        public Int64 id_group { get => _id_group; set => _id_group = value; }
        public int sequence { get => _sequence; set => _sequence = value; }
        public int level { get => _level; set => _level = value; }

        public string group_name { get => _group_name; set => _group_name = value; }
        public string usuarios { get => _usuarios; set => _usuarios = value; }

        #endregion

        public level_approval_detail()
        {
            init();
        }

        private static void OnInit()
        {
            sql_pointer = (sql_pointer == null) ? SQLHandler.sql_handler.GetInstance() : sql_pointer;
            parametros = new Dictionary<string, object>();
            v_conexion = Extension.Nueva_Conexion("NOTA_CREDITO");
        }

        public level_approval_detail(int _id_level, int _id_group, int _sequence, int _level, string _group_name, string _usuarios)
        {
            this.id_level = _id_level; this.id_group = _id_group;  this.sequence = _sequence; this.level = _level; this.group_name = _group_name; this.usuarios = _usuarios;
        }

        private int? PreValidations(out string msg)
        {

            if (this.id_group <= 0)
            {
                msg = "Especifique el id del grupo";
                return 0;
            }

            if (this.id_level <= 0)
            {
                msg = "Especifique el id del nivel de aprobación";
                return 0;
            }

            if (this.level <= 0)
            {
                msg = "Especifique el nivel de aprobación de detalle";
                return 0;
            }

            if (this.sequence <= 0)
            {
                msg = "Especifique la secuencia del registro de detalle";
                return 0;
            }

            if (string.IsNullOrEmpty(this.Create_user))
            {
                msg = "Especifique el usuario que crea la transacción";
                return 0;
            }

            if (string.IsNullOrEmpty(this.group_name))
            {
                msg = "Especifique el nombre del grupo que formara parte del nivel";
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
            parametros.Add("i_id_level", this.id_level);
            parametros.Add("i_id_group", this.id_group);
            parametros.Add("i_sequence", this.sequence);
            parametros.Add("i_level", this.level);
            parametros.Add("i_state", this.state);
            parametros.Add("i_create_user", this.Create_user);

            var db = sql_pointer.ExecuteInsertUpdateDeleteReturn(v_conexion, 4000, "nc_i_level_approval_detail", parametros, out OnError);
            if (!db.HasValue || db.Value < 0)
            {
                return null;
            }
            OnError = string.Empty;
            return db.Value;

        }

        /*busca un nivel en especifico de la base de datos de nota de credito*/
        public static List<level_approval_detail> GetLevel_detail(int _id_level)
        {
            OnInit();
            string msg;
            parametros.Clear();
            parametros.Add("i_id_level", _id_level);
            return sql_pointer.ExecuteSelectControl<level_approval_detail>(v_conexion, 2000, "nc_get_level_approval_detail", parametros, out msg);
        }



    }
}
