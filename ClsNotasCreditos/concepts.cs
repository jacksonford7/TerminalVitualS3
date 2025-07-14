using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ControlOPC.Entidades;


namespace ClsNotasCreditos
{
    public class concepts : Base
    {
        #region "Variables"

            private int _id_concept;
            private string _description = string.Empty;
        #endregion

        #region "Propiedades"
            public int id_concept { get => _id_concept; set => _id_concept = value; }
            public string description { get => _description ; set =>_description = value; }
        #endregion

        private static String v_mensaje = string.Empty;

        public concepts()
        {
            init();
        }
        public concepts(int _id_concept, string _description = null)
        {
            this.id_concept = _id_concept; this.description = _description;
        }

        #region "Metodos"

        private static void OnInit()
        {
            sql_pointer = (sql_pointer == null) ? SQLHandler.sql_handler.GetInstance() : sql_pointer;
            parametros = new Dictionary<string, object>();

            v_conexion = Extension.Nueva_Conexion("NOTA_CREDITO");
        }

        public static List<concepts> ListConceptos()
        {
            OnInit();

            return sql_pointer.ExecuteSelectControl<concepts>(v_conexion, 2000, "nc_c_concepts", null, out v_mensaje);

        }

        public static List<concepts> ListConceptosGeneral()
        {
            OnInit();

            return sql_pointer.ExecuteSelectControl<concepts>(v_conexion, 2000, "nc_c_list_concepts", null, out v_mensaje);

        }
        public static List<concepts> ListConceptosActivos()
        {
            OnInit();

            return sql_pointer.ExecuteSelectControl<concepts>(v_conexion, 2000, "nc_c_concepts_activo", null, out v_mensaje);

        }

        public static List<concepts> Max_Id_Concepto()
        {
            OnInit();

            return sql_pointer.ExecuteSelectControl<concepts>(v_conexion, 2000, "nc_max_concepts", null, out v_mensaje);

        }

        public static List<concepts> Get_Concepto(Int16 _id_concept)
        {
            OnInit();
            string msg;
            parametros.Clear();
            parametros.Add("i_id_concept", _id_concept);
            return sql_pointer.ExecuteSelectControl<concepts>(v_conexion, 2000, "nc_get_concepts", parametros, out msg);
        }


        private int? PreValidations(out string msg)
        {

            if (this.id_concept <= 0)
            {
                msg = "Especifique el id del concepto";
                return 0;
            }

            if (string.IsNullOrEmpty(this.description))
            {
                msg = "Especifique la descripción del concepto";
                return 0;
            }

            if (string.IsNullOrEmpty(this.Create_user))
            {
                msg = "Especifique el usuario creador del registro";
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
            parametros.Add("i_id_concept", this.id_concept);
            parametros.Add("i_description", this.description);
            parametros.Add("i_state", this.state);
            parametros.Add("i_create_user", this.Create_user);
            parametros.Add("i_action", this.Action);

            using (var scope = new System.Transactions.TransactionScope())
            {

                var db = sql_pointer.ExecuteInsertUpdateDeleteReturn(v_conexion, 4000, "nc_i_concepts", parametros, out OnError);
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
            parametros.Add("i_id_concept", this.id_concept);
            parametros.Add("i_mod_user", this.Mod_user);
            using (var scope = new System.Transactions.TransactionScope())
            {
                var db = sql_pointer.ExecuteInsertUpdateDelete(v_conexion, 4000, "nc_d_concepts", parametros, out OnError);
                if (!db.HasValue || db.Value < 0)
                {
                    return false;
                }
                scope.Complete();
            }
            return true;
        }



        #endregion

    }
}
