using BillionEntidades;
using SqlConexion;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace BreakBulk
{
    public class roles : Cls_Bil_Base
    {
        #region "Propiedades"
        public long? Id { get; set; }
        public string Name { get; set; }
        public bool? Status { get; set; }
        public string Create_user { get; set; }
        public DateTime? Create_date { get; set; }
        public string Modifie_user { get; set; }
        public DateTime? Modifie_date { get; set; }
        public bool? SuperUser { get; set; }
        #endregion

        public roles()
        {
            init();
        }

        private static void OnInit(string Base)
        {
            sql_puntero = (sql_puntero == null) ? Cls_Conexion.Conexion() : sql_puntero;
            parametros = new Dictionary<string, object>();
            nueva_conexion = Cls_Conexion.Nueva_Conexion(Base);
        }

        public static roles GetRoles(long _id)
        {
            OnInit("N4Middleware");
            parametros.Clear();
            parametros.Add("i_id", _id);
            return sql_puntero.ExecuteSelectOnly<roles>(nueva_conexion, 4000, "[brbk].[consultarRoles]", parametros);
        }

        public static DataView consultaRoles()
        {
            OnInit("N4Middleware");
            SqlConnection cn = new SqlConnection(nueva_conexion);

            var d = new DataTable();
            using (var c = cn)
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandText = "[brbk].consultarRoles";
                try
                {
                    c.Open();
                    d.Load(coman.ExecuteReader(CommandBehavior.CloseConnection));
                }
                catch (SqlException ex)
                {
                    throw;
                }
                finally
                {
                    if (c.State == ConnectionState.Open)
                    {
                        c.Close();
                    }
                    c.Dispose();
                }
            }
            return d.DefaultView;
        }

    }
}
