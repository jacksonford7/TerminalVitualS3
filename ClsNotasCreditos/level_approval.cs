using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ControlOPC.Entidades;

namespace ClsNotasCreditos
{
    public class level_approval : Base 
    {
        #region "Variables"

        private Int64 _id_level;
        private string _level_name = string.Empty;
        private bool _level_state;
        private string _usuarios = string.Empty;
        private string _grupos = string.Empty;
        private string _aux_level_name = string.Empty;
        private decimal _init_value = 0;
        private decimal _init_end = 0;
        private Int16 _id_concept;
        private string _description = string.Empty;
        #endregion

        #region "Propiedades"
        public Int64 id_level { get => _id_level; set => _id_level = value; }
        public string level_name { get => _level_name; set => _level_name = value; }
        public bool level_state { get => _level_state; set => _level_state = value; }
        public string usuarios { get => _usuarios; set => _usuarios = value; }
        public string aux_level_name { get => _aux_level_name; set => _aux_level_name = value; }
        public string grupos { get => _grupos; set => _grupos = value; }
        public decimal init_value { get => _init_value; set => _init_value = value; }
        public decimal init_end { get => _init_end; set => _init_end = value; }
        public Int16 id_concept { get => _id_concept; set => _id_concept = value; }
        public string description { get => _description; set => _description = value; }
        private static String v_mensaje = string.Empty;

        #endregion

        public List<level_approval_detail> Detalle { get; set; }

        public level_approval(Int64 _id_level, string _level_name, bool _level_state, string _grupos, decimal _init_value, decimal _init_end, Int16 _id_concept)
        {
            this.id_level = _id_level;
            this.level_name = _level_name;
            this.level_state = _level_state;
            this.grupos = _grupos;
            this.init_value = _init_value;
            this.init_end = _init_end;
            this.id_concept = _id_concept;

            this.Detalle = new List<level_approval_detail>();
            OnInit();
        }

        public level_approval()
        {
            OnInit();
            this.Detalle = new List<level_approval_detail>();
        }

        /*conexion nota de credito*/

        private static void OnInit()
        {
            sql_pointer = (sql_pointer == null) ? SQLHandler.sql_handler.GetInstance() : sql_pointer;
            parametros = new Dictionary<string, object>();
            v_conexion = Extension.Nueva_Conexion("NOTA_CREDITO");
        }



        public static List<level_approval> Max_Id_Nivel()
        {
            OnInit();

            return sql_pointer.ExecuteSelectControl<level_approval>(v_conexion, 2000, "nc_max_level_approval_head", null, out v_mensaje);

        }

        private int? PreValidationsTransaction(out string msg)
        {
            //validaciones de cabecera.
            if (this.id_level <= 0)
            {
                msg = "Especifique el id del nivel";
                return 0;
            }

            if (string.IsNullOrEmpty(this.level_name))
            {
                msg = "Debe especificar el nombre del nivel de aprobación";
                return 0;

            }

            if (this.init_value <= 0)
            {
                msg = "Especifique el valor inicial";
                return 0;
            }

            if (this.init_end <= 0)
            {
                msg = "Especifique el valor final";
                return 0;
            }

            //validaciones de id de motivo.
            if (this.id_concept <= 0)
            {
                msg = "Especifique el id del concepto o motivo";
                return 0;
            }


            if (string.IsNullOrEmpty(this.Create_user))
            {
                msg = "Debe especificar el usuario que crea este grupo";
                return 0;
            }

            if (this.level_name != this.aux_level_name)
            {
                msg = this.Validate_Name(out msg);
                if (msg != string.Empty)
                {
                    return 0;
                }
            }

            msg = this.Validate_Value (out msg);
            if (msg != string.Empty)
            {
                return 0;
            }

            //cuenta solo los activos
            var nRegistros = this.Detalle.Where(d => d.id_group != 0).Count();

            if (this.Detalle == null || nRegistros <= 0)
            {
                msg = "No se puede agregar el detalle de grupos que formaran el nivel de aprobación, no existen registros, debe agregar un grupo";
                return 0;
            }


            msg = string.Empty;
            return 1;
        }

        private Int64? Save(out string OnError)
        {

            parametros.Clear();
            parametros.Add("i_id_level", this.id_level);
            parametros.Add("i_level_name", this.level_name);
            parametros.Add("i_level_state", this.level_state);
            parametros.Add("i_init_value", this.init_value);
            parametros.Add("i_init_end", this.init_end);
            parametros.Add("i_id_concept", this.id_concept);
            parametros.Add("i_create_user", Create_user);
            parametros.Add("i_action", this.Action);

            var db = sql_pointer.ExecuteInsertUpdateDeleteReturn(v_conexion, 4000, "nc_i_level_approval_head", parametros, out OnError);
            if (!db.HasValue || db.Value < 0)
            {
                return null;
            }
            OnError = string.Empty;
            return db.Value;

        }

        public Int64? SaveTransaction(out string OnError)
        {
            bool result_ = false;
            string resultado_others = null;
            try
            {
                //validaciones previas
                if (this.PreValidationsTransaction(out OnError) != 1)
                {
                    return 0;
                }
                using (var scope = new System.Transactions.TransactionScope())
                {
                    //grabar cabecera.
                    var id = this.Save(out OnError);
                    if (!id.HasValue)
                    {
                        return 0;
                    }
                    this.id_level = id.Value;

                    //si no falla la cabecera entonces añada los items
                    foreach (var i in this.Detalle)
                    {
                        i.id_level = id.Value;
                        i.Create_user = this.Create_user;

                        var il = i.Save(out OnError);
                        if (!il.HasValue || il.Value <= 0)
                        {
                            return 0;
                        }
                        i.id_level = il.Value;
                    }
                    //fin de la transaccion
                    scope.Complete();
                    OnError = string.Empty;
                    return this.id_level;
                }
            }
            catch (Exception ex)
            {
                result_ = false;
                resultado_others = ex.Message; ;
                var r = SQLHandler.sql_handler.LogEvent<Exception>(this.Create_user, nameof(level_approval), nameof(SaveTransaction), result_, this.id_level.ToString(), null, resultado_others, ex);
                OnError = string.Format("Ha ocurrido la excepción #{0}", r);
                return null;
            }
        }


        /*valida que no exista el mismo nombre del nivel ingresado varias veces*/
        public string Validate_Name(out string OnError)
        {

            parametros.Clear();
            parametros.Add("i_level_name", this.level_name);
            parametros.Add("i_id_concept", this.id_concept);

            var db = sql_pointer.ExecuteSelectOnly(v_conexion, 4000, "nc_validate_level_approval_exist", parametros);
            if (db == null)
            {
                OnError = "Erro al validar nombre del nivel: Validate_Name";
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

        /*valida que no exista el mismo rango nicial o final varias veces*/
        public string Validate_Value(out string OnError)
        {

            parametros.Clear();
            parametros.Add("i_id_level", this.id_level);
            parametros.Add("i_init_value", this.init_value);
            parametros.Add("i_init_end", this.init_end);
            parametros.Add("i_id_concept", this.id_concept);

            var db = sql_pointer.ExecuteSelectOnly(v_conexion, 4000, "nc_validate_level_approval_init", parametros);
            if (db == null)
            {
                OnError = "Erro al validar rango de nivel: Validate_Value";
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

        /*listado simplificado almacenados en la base de notas de credito*/
        public static List<level_approval> ListNivelesAprobacion()
        {
            OnInit();
            return sql_pointer.ExecuteSelectControl<level_approval>(v_conexion, 2000, "nc_c_level_approval_head", null, out v_mensaje);
        }

        /*busca un nivel en especifico de la base de datos de nota de credito*/
        public static List<level_approval> GetLevel_head(int _id_level)
        {
            OnInit();
            string msg;
            parametros.Clear();
            parametros.Add("i_id_level", _id_level);
            return sql_pointer.ExecuteSelectControl<level_approval>(v_conexion, 2000, "nc_get_level_approval_head", parametros, out msg);
        }


    }
}
