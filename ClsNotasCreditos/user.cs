using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ControlOPC.Entidades;

namespace ClsNotasCreditos
{
    public class user : Base
    {
        #region "Variables"

        private int _IdUsuario;
        private string _Usuario = string.Empty;
        private string _Nombre = string.Empty;
        private string _Apellido = string.Empty;
        private string _Email = string.Empty;
        private string _Nombres = string.Empty;
        #endregion

        #region "Propiedades"
        public int IdUsuario { get => _IdUsuario; set => _IdUsuario = value; }
        public string Usuario { get => _Usuario; set => _Usuario = value; }
        public string Nombre { get => _Nombre; set => _Nombre = value; }
        public string Nombres { get => _Nombres; set => _Nombres = value; }
        public string Apellido { get => _Apellido; set => _Apellido = value; }
        public string Email { get => _Email; set => _Email = value; }

        #endregion

        private static String v_mensaje = string.Empty;

        public user()
        {
            init();
        }
        public user(int _IdUsuario, string _Usuario = null, string _Nombre = null, string _Apellido = null, string _Email = null)
        {
            this.IdUsuario = _IdUsuario; this.Usuario = _Usuario; this.Nombre = _Nombre; this.Apellido = _Apellido; this.Email = _Email;
        }

        #region "Metodos"
        /*conexion nota de credito*/
        private static void OnInit()
        {
            sql_pointer = (sql_pointer == null) ? SQLHandler.sql_handler.GetInstance() : sql_pointer;
            parametros = new Dictionary<string, object>();

            v_conexion = Extension.Nueva_Conexion("NOTA_CREDITO");
        }
        /*conexion base del portal web*/
        private static void OnInit_Portal()
        {
            sql_pointer = (sql_pointer == null) ? SQLHandler.sql_handler.GetInstance() : sql_pointer;
            parametros = new Dictionary<string, object>();

            v_conexion = Extension.Nueva_Conexion("PORTAL_MASTER");
        }
        /*listado general de usuarios del portal*/
        public static List<user> Get_Usuario(string _Criterio)
        {
            OnInit_Portal();
            string msg;
            parametros.Clear();
            parametros.Add("i_Criterio", _Criterio);
            return sql_pointer.ExecuteSelectControl<user>(v_conexion, 2000, "nc_get_user", parametros, out msg);
        }
        /*usuario del portal*/
        public static List<user> Get_Poblar_Usuario(int _Criterio)
        {
            OnInit_Portal();
            string msg;
            parametros.Clear();
            parametros.Add("i_IdUsuario", _Criterio);
            return sql_pointer.ExecuteSelectControl<user>(v_conexion, 2000, "nc_get_user_lookup", parametros, out msg);
        }

        private int? PreValidations(out string msg)
        {
            OnInit();

            if (this.IdUsuario <= 0)
            {
                msg = "Especifique el id del Usuario";
                return 0;
            }

            if (string.IsNullOrEmpty(this.Usuario))
            {
                msg = "Especifique el Usuario";
                return 0;
            }

            if (string.IsNullOrEmpty(this.Email))
            {
                msg = "Especifique el correo del usuario";
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
            parametros.Add("i_IdUsuario", this.IdUsuario);
            parametros.Add("i_Usuario", this.Usuario);
            parametros.Add("i_Nombre", this.Nombre);
            parametros.Add("i_Apellido", this.Apellido);
            parametros.Add("i_email", this.Email);
            parametros.Add("i_state", this.state);
            parametros.Add("i_create_user", this.Create_user);
            parametros.Add("i_action", this.Action);

            using (var scope = new System.Transactions.TransactionScope())
            {

                var db = sql_pointer.ExecuteInsertUpdateDeleteReturn(v_conexion, 4000, "nc_i_user", parametros, out OnError);
                if (!db.HasValue || db.Value < 0)
                {

                    return null;
                }
                OnError = string.Empty;
                scope.Complete();
                return db.Value;
            }

        }

        /*listado simplificado almacenados en la base de notas de credito*/
        public static List<user> ListUsuarios()
        {
            OnInit();
            return sql_pointer.ExecuteSelectControl<user>(v_conexion, 2000, "nc_c_user", null, out v_mensaje);
        }

        public static List<user> ListUsuarios(string _Criterio)
        {
            OnInit();
            string msg;
            parametros.Clear();
            parametros.Add("i_Criterio", _Criterio);
            return sql_pointer.ExecuteSelectControl<user>(v_conexion, 2000, "nc_c_user_group", parametros, out msg);
        }


        /*busca un usuario en especifico de la base de datos de nota de credito*/
        public static List<user> GetUsuario(int _Criterio)
        {
            OnInit();
            string msg;
            parametros.Clear();
            parametros.Add("i_IdUsuario", _Criterio);
            return sql_pointer.ExecuteSelectControl<user>(v_conexion, 2000, "nc_get_user", parametros, out msg);
        }

        #endregion
    }
}
