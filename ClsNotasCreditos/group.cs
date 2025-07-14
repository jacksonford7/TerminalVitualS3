using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ControlOPC.Entidades;

namespace ClsNotasCreditos
{
    public class group : Base 
    {
        #region "Variables"

        private Int64 _id_group;
        private string _group_name = string.Empty;
        private bool _group_state;
        private string _usuarios = string.Empty;
        private string _aux_group_name = string.Empty;
        #endregion

        #region "Propiedades"
        public Int64 id_group { get => _id_group; set => _id_group = value; }
        public string group_name { get => _group_name; set => _group_name = value; }
        public bool group_state { get => _group_state; set => _group_state = value; }
        public string usuarios { get => _usuarios; set => _usuarios = value; }
        public string aux_group_name { get => _aux_group_name; set => _aux_group_name = value; }

        private static String v_mensaje = string.Empty;

        #endregion

        public List<group_detail> Detalle { get; set; }

        public group(int _id_group, string _group_name, bool _group_state)
        {
            this.id_group = _id_group;
            this.group_name = _group_name;
            this.group_state = _group_state;
            this.Detalle = new List<group_detail>();
            OnInit();
        }

        /*conexion nota de credito*/
       
        private static void OnInit()
        {
            sql_pointer = (sql_pointer == null) ? SQLHandler.sql_handler.GetInstance() : sql_pointer;
            parametros = new Dictionary<string, object>();
            v_conexion = Extension.Nueva_Conexion("NOTA_CREDITO");
        }

        public group()
        {
            OnInit();
            this.Detalle = new List<group_detail>();
        }

        public static List<group> Max_Id_Group()
        {
            OnInit();

            return sql_pointer.ExecuteSelectControl<group>(v_conexion, 2000, "nc_max_group_head", null, out v_mensaje);

        }

        /*listado simplificado almacenados en la base de notas de credito*/
        public static List<group> ListGrupos()
        {
            OnInit();
            return sql_pointer.ExecuteSelectControl<group>(v_conexion, 2000, "nc_c_group_head", null, out v_mensaje);
        }

        private int? PreValidationsTransaction(out string msg)
        {
            //validaciones de cabecera.

            if (this.id_group <= 0)
            {
                msg = "Especifique el id del grupo";
                return 0;
            }

            if (string.IsNullOrEmpty(this.group_name))
            {
                msg = "Debe especificar el nombre del grupo";
                return 0;

            }
           
            if (string.IsNullOrEmpty(this.Create_user))
            {
                msg = "Debe especificar el usuario que crea este grupo";
                return 0;
            }

            if (this.group_name != this.aux_group_name)
            {
                msg = this.Validate_Name(out msg);
                if (msg != string.Empty)
                {
                    return 0;
                }
            }
          

            //cuenta solo los activos
            var nRegistros = this.Detalle.Where(d => d.IdUsuario != 0).Count();

            if (this.Detalle == null || nRegistros <= 0)
            {
                msg = "No se puede agregar el detalle de usuarios que formaran el grupo, no existen registros, debe agregar un usuario";
                return 0;
            }

           
            msg = string.Empty;
            return 1;
        }

        private Int64? Save(out string OnError)
        {
            
            parametros.Clear();
            parametros.Add("i_id_group", this.id_group);
            parametros.Add("i_group_name", this.group_name);
            parametros.Add("i_group_state", this.group_state); 
            parametros.Add("i_create_user", Create_user); 
            parametros.Add("i_action", this.Action);
           
            var db = sql_pointer.ExecuteInsertUpdateDeleteReturn(v_conexion, 4000, "nc_i_group_head", parametros, out OnError);
            if (!db.HasValue || db.Value < 0) {
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
                    this.id_group  = id.Value;

                    //si no falla la cabecera entonces añada los items
                    foreach (var i in this.Detalle)
                    {
                        i.id_group = id.Value;
                        i.Create_user = this.Create_user;
                        
                        var il = i.Save(out OnError);
                        if (!il.HasValue || il.Value <= 0)
                        {
                            return 0;
                        }
                        i.id_group = il.Value;
                    }
                    //fin de la transaccion
                    scope.Complete();
                    OnError = string.Empty;
                    return this.id_group;
                }
            }
            catch (Exception ex)
            {
                result_ = false;
                resultado_others = ex.Message; ;
                var r = SQLHandler.sql_handler.LogEvent<Exception>(this.Create_user, nameof(group), nameof(SaveTransaction), result_, this.id_group.ToString(), null, resultado_others, ex);
                OnError = string.Format("Ha ocurrido la excepción #{0}", r);
                return null;
            }
        }

        /*busca un grupo en especifico de la base de datos de nota de credito*/
        public static List<group> GetGroup_head(int _id_group)
        {
            OnInit();
            string msg;
            parametros.Clear();
            parametros.Add("i_id_group", _id_group);
            return sql_pointer.ExecuteSelectControl<group>(v_conexion, 2000, "nc_get_group_head", parametros, out msg);
        }

        /*existe grupo activo con el mismo nombre*/
        public static List<group> GetGroup_name(string _i_group_name)
        {
            OnInit();
            string msg;
            parametros.Clear();
            parametros.Add("i_group_name", _i_group_name);
            return sql_pointer.ExecuteSelectControl<group>(v_conexion, 2000, "nc_validate_group_exist", parametros, out msg);
        }

        /*valida que no exista el mismo nombre de grupo ingresado varias veces*/
        public string Validate_Name(out string OnError)
        {

            parametros.Clear();
            parametros.Add("i_group_name", this.group_name);
          
            var db = sql_pointer.ExecuteSelectOnly(v_conexion, 4000, "nc_validate_group_exist", parametros);
            if (db == null)
            {
                OnError = "Erro al validar nombre del grupo: Validate_Name";
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

        /*listado para mostrar buscador de grupos creados*/
        public static List<group> ListaGeneral_Grupos(string _Criterio)
        {
            OnInit();
            string msg;
            parametros.Clear();
            parametros.Add("i_Criterio", _Criterio);
            return sql_pointer.ExecuteSelectControl<group>(v_conexion, 2000, "nc_c_level_list_group", parametros, out msg);
        }

    }
}
