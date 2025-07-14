using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ControlOPC.Entidades
{
    public class OPCOperador:Base
    {
        public Int64 id { get; set; }
        public Int64 id_grupo { get; set; }
        public string ope_id { get; set; }
        public string ope_nombre { get; set; }
        public string ope_apellido { get; set; }
        public bool ope_supervisor { get; set; }
        public OPCOperador(int _grupo)
        {
            this.id_grupo = _grupo;
            OnInit();
        }
        public OPCOperador()
        {
            OnInit();
        }
        private static void OnInit()
        {
            sql_pointer = (sql_pointer == null) ? SQLHandler.sql_handler.GetInstance() : sql_pointer;
            parametros = new Dictionary<string, object>();
        }
        private int? PreValidations(out string msg)
        {
            //1.->Validaciones de objetos.
            if (this.id_grupo <= 0)
            {
                msg = "Especifique el id del grupo";
                return 0;
            }

            if (string.IsNullOrEmpty(this.ope_id))
            {
                msg = "Especifique la ci/pasaporte del operario";
                return 0;
            }
            if (string.IsNullOrEmpty(this.ope_nombre))
            {
                msg = "Especifique el nombre del operario";
                return 0;
            }
            if (string.IsNullOrEmpty(this.ope_apellido))
            {
                msg = "Especifique los apellidos del operario";
                return 0;
            }
            if (string.IsNullOrEmpty(this.Create_user))
            {
                msg = "Especifique el usuario creador";
                return 0;
            }

            msg = string.Empty;
            return 1;
        }
        public Int64? Save(out string OnError)
        {
            bool update = false;
            if (this.PreValidations(out OnError) != 1)
            {
                return -1;
            }
            parametros.Clear();
            parametros.Add("i_id_grupo", id_grupo); //grupo al que pertenece
            parametros.Add("i_ope_id", ope_id); // ci o pas
            parametros.Add("i_ope_nombre", ope_nombre); //nombre
            parametros.Add("i_ope_apellido", this.ope_apellido);//apellidos
            parametros.Add("i_ope_supervisor", this.ope_supervisor);// es supervisor
            parametros.Add("i_create_user", Create_user); // quien crea

            if (this.id > 0)
            {
                parametros.Add("i_id", this.id);
                parametros.Add("i_active", this.active);
                update = true;
            }


            var db = sql_pointer.ExecuteInsertUpdateDeleteReturn(sql_pointer.basic_con, 4000, "PC_M_Grupo_Operador", parametros, out OnError);
            if (!db.HasValue || db.Value < 0)
            {
                //null error en base de datos, el mensaje ya fue en onError
                return null;
            }
            OnError = string.Empty;
            this.id = update? this.id:  db.Value;
            return this.id;
            // PC_I_Grupo_Operador
        }
        static internal bool Delete( Int64 id)
        {
            OnInit();
            parametros.Clear();
            string OnError;
            parametros.Add("i_idgrupo", id);
            var db = sql_pointer.ExecuteInsertUpdateDelete(sql_pointer.basic_con, 4000, "PC_D_GrupoOperador", parametros, out OnError);
            if (!db.HasValue || db.Value < 0) { return false; }
            return true;
        }

        public static bool? OperarioEnGrupo(string _cedula)
        {
            OnInit();
            parametros.Clear();
            parametros.Add("i_ope_id", _cedula);
            string dt;
            var re = sql_pointer.EscalarFunction(sql_pointer.basic_con, 4000, "select  [dbo].[fx_ope_in_group](@i_ope_id)", parametros, out dt);
            if (re == null)
            {
                return null;
            }
            bool r = true;
            if (!bool.TryParse(re.ToString(), out r))
            {
                return null;
            }

            return r;
        }
    }
}
