using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ControlOPC.Entidades;


namespace ClsNotasCreditos
{
    public class group_detail : Base 
    {

        #region "Variables"

        private Int64 _id_group;
        private int _sequence;
        private string _description = string.Empty;
        private int _IdUsuario;
        private string _Usuario = string.Empty;
        private string _Nombre = string.Empty;
        private string _Apellido = string.Empty;
        private string _Email = string.Empty;

        #endregion

        #region "Propiedades"

        public Int64 id_group { get => _id_group; set => _id_group = value; }
        public int sequence { get => _sequence; set => _sequence = value; }
        public int IdUsuario { get => _IdUsuario; set => _IdUsuario = value; }
        public string Usuario { get => _Usuario; set => _Usuario = value; }
        public string Nombre { get => _Nombre; set => _Nombre = value; }
        public string Apellido { get => _Apellido; set => _Apellido = value; }
        public string Email { get => _Email; set => _Email = value; }
        #endregion

        public group_detail()
        {
            init();
        }

        public group_detail(int _id_group, int _sequence, int _IdUsuario, string _Usuario)
        {
            this.id_group = _id_group; this.sequence = _sequence; this.IdUsuario = _IdUsuario; this.Usuario = _Usuario;
        }


        private static void OnInit()
        {
         
            v_conexion = Extension.Nueva_Conexion("NOTA_CREDITO");
        }


        private int? PreValidations(out string msg)
        {
           
            if (this.id_group <= 0)
            {
                msg = "Especifique el id del grupo";
                return 0;
            }

            if (this.IdUsuario <= 0)
            {
                msg = "Especifique el id del usuario que formara parte del grupo";
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

            if (string.IsNullOrEmpty(this.Usuario))
            {
                msg = "Especifique usuario que formara parte del grupo";
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
            parametros.Add("i_id_group", this.id_group);
            parametros.Add("i_sequence", this.sequence);
            parametros.Add("i_IdUsuario", this.IdUsuario);
            parametros.Add("i_Usuario", this.Usuario);
            parametros.Add("i_state", this.state);
            parametros.Add("i_create_user", this.Create_user);

            var db = sql_pointer.ExecuteInsertUpdateDeleteReturn(v_conexion, 4000, "nc_i_group_detail", parametros, out OnError);
            if (!db.HasValue || db.Value < 0)
            {
                return null;
            }
            OnError = string.Empty;
            return db.Value;
            
        }

        /*busca un grupo en especifico de la base de datos de nota de credito*/
        public static List<group_detail> GetGroup_detail(int _id_group)
        {
            OnInit();
            string msg;
            parametros.Clear();
            parametros.Add("i_id_group", _id_group);
            return sql_pointer.ExecuteSelectControl<group_detail>(v_conexion, 2000, "nc_get_group_detail", parametros, out msg);
        }

    }
}
