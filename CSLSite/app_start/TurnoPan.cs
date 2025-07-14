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
    public class TurnoPan
    {
        public int tipo_id { get; set; }
        public string cntr { get; set; }
        public string aisv { get; set; }
        public string booking { get; set; }
        public string fecha { get; set; }
        public string usuario { get; set; }
        public string descripcion { get; set; }
        public string horario { get; set; }
        public string nota { get; set; }
        public bool activo { get; set; }
        public string expoid { get; set; }
        public string exponame { get; set; }

        //metodo para grabar en la base de CGNDB05
        //graba el refrigerado en cgndb05 middleware
        public static Int64 salvar_refrigerado(string cntr, string booking, string aisv, int tipo,string usuario, string exporid, string exporname)
        {
            try
            {
                if (tipo <= 0)
                {
                    return -2;
                }
                var cn = System.Configuration.ConfigurationManager.ConnectionStrings["midle"]?.ConnectionString;
                if (cn == null)
                {
                    return -4;
                }
                if (string.IsNullOrEmpty(cntr) || string.IsNullOrEmpty(booking))
                {
                    return -1;
                }

                using (var conexion = new SqlConnection(cn))
                {
                    try
                    {
                        using (var comando = conexion.CreateCommand())
                        {
                            comando.CommandType = CommandType.StoredProcedure;
                            comando.CommandText = "PNA_INSERTAR_REFRIGERADO";
                            comando.Parameters.AddWithValue("@aisv_contenedor", cntr);
                            comando.Parameters.AddWithValue("@unidad_gkey", -1);
                            comando.Parameters.AddWithValue("@aisv_codigo", aisv);
                            comando.Parameters.AddWithValue("@aisv_numero_booking", booking);
                            comando.Parameters.AddWithValue("@tipo", tipo);
                            comando.Parameters.AddWithValue("@usuario", usuario);

                            if (!string.IsNullOrEmpty(exporid))
                            {
                                comando.Parameters.AddWithValue("@expoid", exporid);
                            }
                            if (!string.IsNullOrEmpty(exporname))
                            {
                                comando.Parameters.AddWithValue("@exponom", exporname);
                            }

                            //@usuario
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
                        var t = log_csl.save_log<ApplicationException>(new ApplicationException(sb.ToString()), "PNA_INSERTAR_REFRIGERADO", "salvar_refrigerado", cntr, "sistema");
                        return t * -1;
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
                var t = log_csl.save_log<Exception>(ex, "PNA_INSERTAR_REFRIGERADO", "salvar_refrigerado", cntr, "sistema");
                return t * -1;
            }
        }

        public static HashSet<Tuple<string, string>> RetornarTipos()
        {
            var xsalida = new HashSet<Tuple<string, string>>();
            var cn = System.Configuration.ConfigurationManager.ConnectionStrings["midle"]?.ConnectionString;

            if (string.IsNullOrEmpty(cn))
            {
                xsalida.Add(new Tuple<string, string>("-1","Error de conexión"));
            }
                  using (var con = new SqlConnection(cn))
            { 
                var com = con.CreateCommand();
                com.CommandType = CommandType.StoredProcedure;
                com.CommandTimeout = 120;
                com.CommandText = "pc_tipos_refrigerados";
                try
                {
                    con.Open();
                    using (var reader = com.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            xsalida.Add(Tuple.Create(reader.GetString(0), reader.GetString(1)));
                        }
                    }
                    con.Close();
                    com.Dispose();
                }
                catch (SqlException ex)
                {
                    var sb = new StringBuilder();
                    foreach (SqlError e in ex.Errors)
                    {
                        sb.AppendLine(string.Format("{0}, {1}, {2}, {3}", e.LineNumber, e.Message, e.Procedure, e.Server));
                    }
                    log_csl.save_log<Exception>(new Exception(sb.ToString()), "TurnoPan", "RetornarTipos", "pc_tipos_refrigerados", "N4");
                    xsalida.Add(Tuple.Create("-2", "Error general"));
                    return xsalida;
                }
                finally
                {
                    if (con.State == ConnectionState.Open)
                    {
                        con.Close();
                    }

                }
            }
            return xsalida;
        }


        public static TurnoPan ObtenerData(string aisv)
        {
            var tp = new TurnoPan();
            var cn = System.Configuration.ConfigurationManager.ConnectionStrings["midle"]?.ConnectionString;

            if (string.IsNullOrEmpty(aisv))
            {
                return null;
            }
            using (var con = new SqlConnection(cn))
            {
                var com = con.CreateCommand();
                com.CommandType = CommandType.StoredProcedure;
                com.CommandTimeout = 600;
                com.CommandText = "pc_consulta_refrigerado";
                com.Parameters.AddWithValue("@aisv",aisv);
                try
                {
                    con.Open();
                    using (var reader = com.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            //aca llenar propiedades.
                            /*cntr,aisv,booking,usuario,fecha,tipo,descripcion,horario*/
                            tp.cntr = reader.GetString(0);
                            tp.aisv = reader.GetString(1);
                            tp.booking = reader.GetString(2);
                            tp.usuario = reader.GetString(3);
                            tp.fecha = reader.GetString(4);
                            tp.tipo_id = reader.GetInt32(5);
                            tp.descripcion = reader.GetString(6);
                            tp.horario = reader.GetString(7);
                            tp.nota = reader.GetString(8);
                            tp.activo = reader.GetBoolean(9);
                            tp.expoid = reader.GetString(10);
                            tp.exponame = reader.GetString(11);
                        }
                    }
                    con.Close();
                    com.Dispose();
                }
                catch (SqlException ex)
                {
                    var sb = new StringBuilder();
                    foreach (SqlError e in ex.Errors)
                    {
                        sb.AppendLine(string.Format("{0}, {1}, {2}, {3}", e.LineNumber, e.Message, e.Procedure, e.Server));
                    }
                    log_csl.save_log<Exception>(new Exception(sb.ToString()), "TurnoPan", "ObtenerData", "pc_consulta_refrigerado", "N4");
                    return null;

                }
                finally
                {
                    if (con.State == ConnectionState.Open)
                    {
                        con.Close();
                    }

                }
            }
            return tp;
        }


        public  Int64 Actualizar()
        {
            try
            {
         
                var cn = System.Configuration.ConfigurationManager.ConnectionStrings["midle"]?.ConnectionString;
                if (cn == null)
                {
                    return -4;
                }
                if (string.IsNullOrEmpty(this.cntr) || string.IsNullOrEmpty(this.aisv))
                {
                    return -1;
                }

                using (var conexion = new SqlConnection(cn))
                {
                    try
                    {
                        using (var comando = conexion.CreateCommand())
                        {
                            comando.CommandType = CommandType.StoredProcedure;
                            comando.CommandText = "PNA_ACTUALIZAR_REFRIGERADO";
                            comando.Parameters.AddWithValue("@aisv_contenedor", cntr);
                            comando.Parameters.AddWithValue("@aisv_codigo", aisv);
                            comando.Parameters.AddWithValue("@usuario", this.usuario);
                            comando.Parameters.AddWithValue("@tipo", this.tipo_id);
                            comando.Parameters.AddWithValue("@nota", this.nota);
                            comando.Parameters.AddWithValue("@activo", this.activo);
                            conexion.Open();
                            object sale = comando.ExecuteNonQuery();
                            conexion.Close();
                            return 1;
                        }
                    }
                    catch (SqlException ex)
                    {
                        StringBuilder sb = new StringBuilder();
                        foreach (SqlError e in ex.Errors)
                        {
                            sb.AppendLine(string.Format("{0}, {1}, {2}, {3}", e.LineNumber, e.Message, e.Procedure, e.Server));
                        }
                        var t = log_csl.save_log<ApplicationException>(new ApplicationException(sb.ToString()), "PNA_ACTUALIZAR_REFRIGERADO", "Actualizar", cntr, "Terminal Virtual");
                        return t * -1;
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
                var t = log_csl.save_log<Exception>(ex, "PNA_ACTUALIZAR_REFRIGERADO", "Actualizar", cntr, "Terminal Virtual");
                return t * -1;
            }
        }


        public static Int64 update_aisv(string aisv, int tipo)
        {
            try
            {
                var cn = System.Configuration.ConfigurationManager.ConnectionStrings["service"]?.ConnectionString;
                if (cn == null)
                {
                    return -4;
                }
                if (string.IsNullOrEmpty(aisv) )
                {
                    return -1;
                }

                using (var conexion = new SqlConnection(cn))
                {
                    try
                    {
                        using (var comando = conexion.CreateCommand())
                        {
                            comando.CommandType = CommandType.StoredProcedure;
                            comando.CommandText = "pc_update_aisv_refrigerado";
                            comando.Parameters.AddWithValue("@aisv_codigo", aisv);
                            comando.Parameters.AddWithValue("@tipo", tipo);
                            conexion.Open();
                            object sale = comando.ExecuteNonQuery();
                            conexion.Close();
                            return 1;
                        }
                    }
                    catch (SqlException ex)
                    {
                        StringBuilder sb = new StringBuilder();
                        foreach (SqlError e in ex.Errors)
                        {
                            sb.AppendLine(string.Format("{0}, {1}, {2}, {3}", e.LineNumber, e.Message, e.Procedure, e.Server));
                        }
                        var t = log_csl.save_log<ApplicationException>(new ApplicationException(sb.ToString()), "pc_update_aisv_refrigerado", "Actualizar", aisv, "Terminal Virtual");
                        return t * -1;
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
                var t = log_csl.save_log<Exception>(ex, "pc_update_aisv_refrigerado", "Actualizar-aisv", aisv, "Terminal Virtual");
                return t * -1;
            }

        }


    }
}