using csl_log;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;

namespace CSLSite.app_start
{
    public class SNA_Usuarios
    {
        public Int64? cliente_id { get; set; }  
        public string cliente_nombres { get; set; }
        public string cliente_ruc { get; set; }
        public string cliente_email { get; set; }
        public string cliente_telefono { get; set; }

        public bool? cliente_estado { get; set; }
        public string cliente_categoria { get; set; }
        public DateTime? cliente_creado { get; set; }
        public string cliente_creadopor { get; set; }
        public DateTime? cliente_modificado { get; set; }
        public string cliente_modificadopor { get; set; }
        public string notas { get; set; }

        public Int64? Guardar() {

            try
            {
                var cn = System.Configuration.ConfigurationManager.ConnectionStrings["ecuapass"]?.ConnectionString;
                if (cn == null)
                {
                    return -4;
                }
                using (var conexion = new SqlConnection(cn))
                {
                    try
                    {
                        using (var comando = conexion.CreateCommand())
                        {
                            comando.CommandType = CommandType.StoredProcedure;
                            comando.CommandText = "SNA_Upsert_Cliente_full";
                            comando.Parameters.AddWithValue("@usuario", string.IsNullOrEmpty(this.cliente_creadopor)?this.cliente_modificadopor:this.cliente_creadopor);
                            if (this.cliente_id.HasValue && this.cliente_id.Value > 0)
                            {
                                comando.Parameters.AddWithValue("@id", this.cliente_id);
                            }
                            if (!string.IsNullOrEmpty(this.cliente_ruc))
                            {
                                comando.Parameters.AddWithValue("@ruc", this.cliente_ruc);
                            }
                            if (!string.IsNullOrEmpty(this.cliente_nombres))
                            {
                                comando.Parameters.AddWithValue("@nombres", this.cliente_nombres);
                            }
                            if (!string.IsNullOrEmpty(this.cliente_categoria))
                            {
                                comando.Parameters.AddWithValue("@categoria", this.cliente_categoria);
                            }

                            if (!string.IsNullOrEmpty(this.cliente_email))
                            {
                                comando.Parameters.AddWithValue("@email", this.cliente_email);
                            }
                            if (!string.IsNullOrEmpty(this.cliente_telefono))
                            {
                                comando.Parameters.AddWithValue("@telefono", this.cliente_telefono);
                            }
                            if (this.cliente_estado.HasValue)
                            {
                                comando.Parameters.AddWithValue("@activo", this.cliente_estado.Value);
                            }
                            conexion.Open();
                            object sale = comando.ExecuteScalar();
                            conexion.Close();
                            if (sale != null && sale.GetType() != typeof(DBNull))
                            {
                                return Int64.Parse(sale.ToString());
                            }
                            else
                            {
                                return -2;
                            }
                        }
                    }
                    catch (SqlException ex)
                    {
                        StringBuilder sb = new StringBuilder();
                        foreach (SqlError e in ex.Errors)
                        {
                            sb.AppendLine(string.Format("{0}, {1}, {2}, {3}", e.LineNumber, e.Message, e.Procedure, e.Server));
                        }
                        var t = log_csl.save_log<ApplicationException>(new ApplicationException(sb.ToString()), "sna_usuario", "Guardar", this.cliente_ruc, "sistema");
                        return -1;
                    }
                    finally
                    {
                        if (conexion.State == ConnectionState.Open)
                        {
                            conexion.Close();
                        }
                        conexion.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                var t = log_csl.save_log<Exception>(ex, "ecuapass_dae", "Guardar", this.cliente_ruc, "sistema");
                return -1;
            }

        }

        public static bool UsuarioExiste(string ruc)
        {
            using (var con = new System.Data.SqlClient.SqlConnection())
            {
                //n4catalog
                con.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ecuapass"].ConnectionString;
                var comando = con.CreateCommand();
                comando.CommandType = System.Data.CommandType.Text;
                comando.Connection = con;
                comando.CommandText = "select [ecuapass].[dbo].[fx_sna_plus_existe](@ruc)";
                comando.Parameters.AddWithValue("@ruc", ruc);
                try
                {
                    con.Open();
                    object sale = comando.ExecuteScalar();
                    con.Close();
                    if (sale != null && sale.GetType() != typeof(DBNull))
                    {
                        return bool.Parse(sale.ToString());
                    }
                    else
                    {
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    log_csl.save_log<Exception>(ex, "Sna_usuario", "UsuarioExiste", ruc, "s3");
                    return true;
                }
                finally
                {
                    if (con.State == System.Data.ConnectionState.Open)
                    {
                        con.Close();
                    }
                    con.Dispose();
                    comando.Dispose();
                }
            }
        }

        public Int64? Guardar_App()
        {

            try
            {
                var cn = System.Configuration.ConfigurationManager.ConnectionStrings["ecuapass"]?.ConnectionString;
                if (cn == null)
                {
                    return -4;
                }
                using (var conexion = new SqlConnection(cn))
                {
                    try
                    {
                        using (var comando = conexion.CreateCommand())
                        {
                            comando.CommandType = CommandType.StoredProcedure;
                            comando.CommandText = "Actualizar_Cliente_impo";
                            comando.Parameters.AddWithValue("@usuario", string.IsNullOrEmpty(this.cliente_creadopor) ? this.cliente_modificadopor : this.cliente_creadopor);
                            if (this.cliente_id.HasValue && this.cliente_id.Value > 0)
                            {
                                comando.Parameters.AddWithValue("@id", this.cliente_id);
                            }
                            if (!string.IsNullOrEmpty(this.cliente_ruc))
                            {
                                comando.Parameters.AddWithValue("@ruc", this.cliente_ruc);
                            }
                            if (!string.IsNullOrEmpty(this.cliente_nombres))
                            {
                                comando.Parameters.AddWithValue("@nombres", this.cliente_nombres);
                            }
                            if (!string.IsNullOrEmpty(this.cliente_categoria))
                            {
                                comando.Parameters.AddWithValue("@categoria", this.cliente_categoria);
                            }

                            if (!string.IsNullOrEmpty(this.cliente_email))
                            {
                                comando.Parameters.AddWithValue("@email", this.cliente_email);
                            }
                            if (!string.IsNullOrEmpty(this.cliente_telefono))
                            {
                                comando.Parameters.AddWithValue("@telefono", this.cliente_telefono);
                            }
                            if (this.cliente_estado.HasValue)
                            {
                                comando.Parameters.AddWithValue("@activo", this.cliente_estado.Value);
                            }

                            if (!string.IsNullOrEmpty(this.notas))
                            {
                                comando.Parameters.AddWithValue("@notas", this.notas);
                            }
                            conexion.Open();
                            object sale = comando.ExecuteScalar();
                            conexion.Close();
                            if (sale != null && sale.GetType() != typeof(DBNull))
                            {
                                return Int64.Parse(sale.ToString());
                            }
                            else
                            {
                                return -2;
                            }
                        }
                    }
                    catch (SqlException ex)
                    {
                        StringBuilder sb = new StringBuilder();
                        foreach (SqlError e in ex.Errors)
                        {
                            sb.AppendLine(string.Format("{0}, {1}, {2}, {3}", e.LineNumber, e.Message, e.Procedure, e.Server));
                        }
                        var t = log_csl.save_log<ApplicationException>(new ApplicationException(sb.ToString()), "sna_usuario", "Guardar", this.cliente_ruc, "sistema");
                        return -1;
                    }
                    finally
                    {
                        if (conexion.State == ConnectionState.Open)
                        {
                            conexion.Close();
                        }
                        conexion.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                var t = log_csl.save_log<Exception>(ex, "ecuapass_dae", "Guardar", this.cliente_ruc, "sistema");
                return -1;
            }

        }

        public Int64? Guardar_App2()
        {

            try
            {
                var cn = System.Configuration.ConfigurationManager.ConnectionStrings["ecuapass"]?.ConnectionString;
                if (cn == null)
                {
                    return -4;
                }
                using (var conexion = new SqlConnection(cn))
                {
                    try
                    {
                        using (var comando = conexion.CreateCommand())
                        {
                            comando.CommandType = CommandType.StoredProcedure;
                            comando.CommandText = "SNA_Upsert_Cliente_full_tv";
                            comando.Parameters.AddWithValue("@usuario", string.IsNullOrEmpty(this.cliente_creadopor) ? this.cliente_modificadopor : this.cliente_creadopor);
                            if (this.cliente_id.HasValue && this.cliente_id.Value > 0)
                            {
                                comando.Parameters.AddWithValue("@id", this.cliente_id);
                            }
                            if (!string.IsNullOrEmpty(this.cliente_ruc))
                            {
                                comando.Parameters.AddWithValue("@ruc", this.cliente_ruc);
                            }
                            if (!string.IsNullOrEmpty(this.cliente_nombres))
                            {
                                comando.Parameters.AddWithValue("@nombres", this.cliente_nombres);
                            }
                            if (!string.IsNullOrEmpty(this.cliente_categoria))
                            {
                                comando.Parameters.AddWithValue("@categoria", this.cliente_categoria);
                            }

                            if (!string.IsNullOrEmpty(this.cliente_email))
                            {
                                comando.Parameters.AddWithValue("@email", this.cliente_email);
                            }
                            if (!string.IsNullOrEmpty(this.cliente_telefono))
                            {
                                comando.Parameters.AddWithValue("@telefono", this.cliente_telefono);
                            }
                            if (this.cliente_estado.HasValue)
                            {
                                comando.Parameters.AddWithValue("@activo", this.cliente_estado.Value);
                            }

                            if (!string.IsNullOrEmpty(this.notas))
                            {
                                comando.Parameters.AddWithValue("@notas", this.notas);
                            }


                            conexion.Open();
                            object sale = comando.ExecuteScalar();
                            conexion.Close();
                            if (sale != null && sale.GetType() != typeof(DBNull))
                            {
                                return Int64.Parse(sale.ToString());
                            }
                            else
                            {
                                return -2;
                            }
                        }
                    }
                    catch (SqlException ex)
                    {
                        StringBuilder sb = new StringBuilder();
                        foreach (SqlError e in ex.Errors)
                        {
                            sb.AppendLine(string.Format("{0}, {1}, {2}, {3}", e.LineNumber, e.Message, e.Procedure, e.Server));
                        }
                        var t = log_csl.save_log<ApplicationException>(new ApplicationException(sb.ToString()), "sna_usuario", "Guardar", this.cliente_ruc, "sistema");
                        return -1;
                    }
                    finally
                    {
                        if (conexion.State == ConnectionState.Open)
                        {
                            conexion.Close();
                        }
                        conexion.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                var t = log_csl.save_log<Exception>(ex, "ecuapass_dae", "Guardar", this.cliente_ruc, "sistema");
                return -1;
            }

        }
    }
}