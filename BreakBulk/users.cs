using BillionEntidades;
using SqlConexion;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace BreakBulk
{
    public class users : Cls_Bil_Base
    {
        public long? Id { get; set; }
        public string Names { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Identification { get; set; }
        public string Phone { get; set; }
        public string Create_user { get; set; }
        public string Modifie_user { get; set; }
        public DateTime? Create_date { get; set; }
        public DateTime? Modifie_date { get; set; }
        public bool? Status { get; set; }
        public long? PositionId { get; set; }
        public long? RoleId { get; set; }
        public long? CompanyId { get; set; }
        public long? IdUserLeader { get; set; }
        public string Email { get; set; }
        public DateTime? WorkDate { get; set; }
        public positions Position { get; set; }
        public roles Role { get; set; }
        public company Company { get; set; }
        public users() : base()
        {
            init();
        }

        private static void OnInit(string Base)
        {
            sql_puntero = (sql_puntero == null) ? Cls_Conexion.Conexion() : sql_puntero;
            parametros = new Dictionary<string, object>();
            nueva_conexion = Cls_Conexion.Nueva_Conexion(Base);
        }

        public static List<users> listadoUsers(out string OnError)
        {
            OnInit("N4Middleware");
            parametros.Clear();
            return sql_puntero.ExecuteSelectControl<users>(nueva_conexion, 4000, "[brbk].[consultarUser]", parametros, out OnError);
        }

        public static users GetUsuario(long _id)
        {
            OnInit("N4Middleware"); 
            parametros.Clear();
            parametros.Add("i_id", _id);
            var obj = sql_puntero.ExecuteSelectOnly<users>(nueva_conexion, 4000, "[brbk].[consultarUser]", parametros);
            try
            {
                obj.Position = positions.GetPositions(long.Parse(obj.PositionId.ToString()));
                obj.Role = roles.GetRoles(long.Parse(obj.RoleId.ToString()));
                obj.Company = company.GetCompany(long.Parse(obj.CompanyId.ToString()));
            }
            catch
            {

            }
            return obj;
        }

        public Int64? Save_Update(out string OnError)
        {

            OnInit("N4Middleware");
            parametros.Clear();
            parametros.Add("i_id", this.Id);
            parametros.Add("i_Names", this.Names);
            parametros.Add("i_Username", this.Username);
            parametros.Add("i_Password", this.Password);
            parametros.Add("i_Identification", this.Identification);
            parametros.Add("i_Phone", this.Phone);
            parametros.Add("i_Create_user", this.Create_user);
            parametros.Add("i_Modifie_user", this.Modifie_user);
            parametros.Add("i_Status", this.Status);
            parametros.Add("i_PositionId", this.PositionId);
            parametros.Add("i_RoleId", this.RoleId);
            parametros.Add("i_CompanyId", this.CompanyId);
            parametros.Add("i_IdUserLeader", this.IdUserLeader);
            parametros.Add("i_Email", this.Email);
           
            var db = sql_puntero.ExecuteInsertUpdateDeleteReturn(nueva_conexion, 6000, "[brbk].insertarUser", parametros, out OnError);
            if (!db.HasValue || db.Value < 0)
            {
                return null;
            }
            OnError = string.Empty;
            return db.Value;

        }


    }
}
