using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.UI.WebControls;
using csl_log;



namespace CSLSite.sav
{
    public class TurnoSav
    {
        public SqlConnection conn;
        public SqlTransaction sqlTransaction;

        public SqlDataReader dr;
        public SqlCommand cmd;
        public SqlDataSource ds;
        public DataSet dss;
        public SqlDataAdapter da;

        public System.Text.StringBuilder Alerta(string msg)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append("alert('");
            sb.Append(msg.Replace("\n", "\\n").Replace("\r", "").Replace("'", "\\'"));
            sb.Append("');");

            return sb;
        }
              
        public DataTable ConsultarHorariosDisponibles(DateTime fechaIng, int Deposito)
        {
            var d = new DataTable();
            var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["midle"].ConnectionString);
            cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "dbo.SAV_TURNOS";
            cmd.Parameters.Clear();
            cmd.Parameters.Add("@TIPO", SqlDbType.VarChar).Value = "1";
            cmd.Parameters.Add("@DEPOSITO", SqlDbType.Int).Value = Deposito;
            cmd.Parameters.Add("@FECHAING", SqlDbType.DateTime).Value = fechaIng;
            try
            {
                conn.Open();
                d.Load(cmd.ExecuteReader(CommandBehavior.CloseConnection));
            }
            catch (SqlException ex)
            {
                csl_log.log_csl.save_log<SqlException>(ex, "service", "SAV_TURNOS", DateTime.Now.ToString(), "ConsultarHorariosDisponibles");
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
                conn.Dispose();
            }
            
        return d;
        }

        public void RegistraDetalleDeHorarios(string HORA, string MINUTO, int CNTR_DISP, string DIA, int TIPO, string DIA_DET,string _user)
        {
            try
            {
                var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["midle"].ConnectionString);
                conn.Open();
                sqlTransaction = conn.BeginTransaction();
                cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "dbo.SAV_TURNOS";
                cmd.Parameters.Clear();
                cmd.Parameters.Add("@TIPO", SqlDbType.VarChar).Value = "5";
                cmd.Parameters.Add("@HORA", SqlDbType.Char).Value = HORA + ":" + MINUTO;
                cmd.Parameters.Add("@CNTR_DISP", SqlDbType.TinyInt).Value = CNTR_DISP;
                cmd.Parameters.Add("@DIA", SqlDbType.VarChar).Value = DIA;
                cmd.Parameters.Add("@TIPO_HORARIO", SqlDbType.VarChar).Value = TIPO;
                cmd.Parameters.Add("@REFERENCIA", SqlDbType.VarChar).Value = DIA_DET;
                cmd.Parameters.Add("@i_usuarioCrea", SqlDbType.VarChar).Value = _user;
                cmd.Parameters.Add("@i_usuarioModifica", SqlDbType.VarChar).Value = _user;

                cmd.Transaction = sqlTransaction;
                cmd.ExecuteNonQuery();
                sqlTransaction.Commit();
                cmd.Parameters.Clear();
                conn.Close();
                conn.Dispose();
            }
            catch (Exception ex)
            {
                sqlTransaction.Rollback();
                throw;
            }
        }

        public void ActualizaDetalleDeHorarios(string HORA, string DIA,string _user)
        {
            try
            {
                var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["midle"].ConnectionString);
                conn.Open();
                sqlTransaction = conn.BeginTransaction();
                cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "dbo.SAV_TURNOS";
                cmd.Parameters.Clear();
                cmd.Parameters.Add("@TIPO", SqlDbType.VarChar).Value = "5";
                cmd.Parameters.Add("@HORA", SqlDbType.Char).Value = HORA;
                cmd.Parameters.Add("@DIA", SqlDbType.VarChar).Value = DIA;
                cmd.Parameters.Add("@i_usuarioCrea", SqlDbType.VarChar).Value = _user;
                cmd.Parameters.Add("@i_usuarioModifica", SqlDbType.VarChar).Value = _user;
                cmd.Transaction = sqlTransaction;
                cmd.ExecuteNonQuery();
                sqlTransaction.Commit();
                cmd.Parameters.Clear();
                conn.Close();
                conn.Dispose();
            }
            catch (Exception ex)
            {
                sqlTransaction.Rollback();
                throw;
            }
        }

        public void ActualizaDetalleDeHorarios(string HORA, string DIA, int TIPO, string DIA_DET, string _user)
        {
            try
            {
                var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["midle"].ConnectionString);
                conn.Open();
                sqlTransaction = conn.BeginTransaction();
                cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "dbo.SAV_TURNOS";
                cmd.Parameters.Clear();
                cmd.Parameters.Add("@TIPO", SqlDbType.VarChar).Value = "6";
                cmd.Parameters.Add("@HORA", SqlDbType.Char).Value = HORA;
                cmd.Parameters.Add("@DIA", SqlDbType.VarChar).Value = DIA;
                cmd.Parameters.Add("@TIPO_HORARIO", SqlDbType.VarChar).Value = TIPO;
                cmd.Parameters.Add("@REFERENCIA", SqlDbType.VarChar).Value = DIA_DET;
                cmd.Parameters.Add("@i_usuarioCrea", SqlDbType.VarChar).Value = _user;
                cmd.Parameters.Add("@i_usuarioModifica", SqlDbType.VarChar).Value = _user;
                cmd.Transaction = sqlTransaction;
                cmd.ExecuteNonQuery();
                sqlTransaction.Commit();
                cmd.Parameters.Clear();
                conn.Close();
                conn.Dispose();
            }
            catch (Exception ex)
            {
                sqlTransaction.Rollback();
                throw;
            }
        }

       

    }
}