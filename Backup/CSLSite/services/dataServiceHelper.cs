using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using csl_log;
using System.Text;

namespace CSLSite.services
{
    public class dataServiceHelper
    {
        //devolver conexion de data
        private static SqlConnection getConector()
        {
            if (System.Configuration.ConfigurationManager.ConnectionStrings["service"] == null)
            {
                return null;
            }
            return new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["service"].ConnectionString);
        }
        private static SqlConnection getConector2()
        {
            if (System.Configuration.ConfigurationManager.ConnectionStrings["n4catalog"] == null)
            {
                return null;
            }
            return new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["n4catalog"].ConnectionString);
        }
        private static SqlConnection getSPConector()
        {
            if (System.Configuration.ConfigurationManager.ConnectionStrings["catalogo"] == null)
            {
                return null;
            }
            return new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["catalogo"].ConnectionString);
        }
        private static SqlConnection getSPConectorMaster()
        {
            if (System.Configuration.ConfigurationManager.ConnectionStrings["portalMaster"] == null)
            {
                return null;
            }
            return new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["portalMaster"].ConnectionString);
        }
        private static SqlConnection getSPConectorSca()
        {
            if (System.Configuration.ConfigurationManager.ConnectionStrings["portalSca"] == null)
            {
                return null;
            }
            return new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["portalSca"].ConnectionString);
        }
        //devolver provincias
        public static IEnumerable<Tuple<string, string>> ReturnProvincias()
        {
            var xsalida = new HashSet<Tuple<string, string>>();
            var con = getConector();
            if (con == null)
            {
                xsalida.Add(Tuple.Create("0","Catalogo vacío [get]"));
                return xsalida;
            }
            using (con)
            {
                var com = con.CreateCommand();
                com.CommandType = CommandType.StoredProcedure;
                com.CommandTimeout = 120;
                com.CommandText = "sp_get_provincias";
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
                    log_csl.save_log<Exception>(new Exception(sb.ToString()), "dataServiceHelper", "ReturnProvincias", "null", "N4");
                    xsalida.Add(Tuple.Create("0", "Catalogo incompleto [set]"));
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
        //devolver cantones
        public static IEnumerable<Tuple<string, string>> ReturnCantones(string provincia)
        {

        
            var xsalida = new HashSet<Tuple<string, string>>();
            if (provincia == "00")
            {
                 xsalida.Add(Tuple.Create("00", "* Escoja provincia *"));
                 return xsalida;
            }
            var con = getConector();
            if (con == null)
            {
                xsalida.Add(Tuple.Create("0", "Catalogo vacío [get]"));
                return xsalida;
            }
            using (con)
            {
                var com = con.CreateCommand();
                com.CommandType = CommandType.StoredProcedure;
                com.CommandTimeout = 120;
                com.CommandText = "sp_get_cantones";
                com.Parameters.AddWithValue("@provincia", provincia);
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
                    log_csl.save_log<Exception>(new Exception(sb.ToString()), "dataServiceHelper", "ReturnCantones", provincia, "N4");
                    xsalida.Add(Tuple.Create("0", "Catalogo incompleto [set]"));
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
        //devolver instituciones
        public static IEnumerable<Tuple<string, string>> ReturnInstitucion()
        {
            var xsalida = new HashSet<Tuple<string, string>>();
            var con = getConector();
            if (con == null)
            {
                xsalida.Add(Tuple.Create("0", "Catalogo vacío [get]"));
                return xsalida;
            }
            using (con)
            {
                var com = con.CreateCommand();
                com.CommandType = CommandType.StoredProcedure;
                com.CommandTimeout = 120;
                com.CommandText = "sp_get_institucion";
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
                    log_csl.save_log<Exception>(new Exception(sb.ToString()), "dataServiceHelper", "ReturnInstitucion", "null", "N4");
                    xsalida.Add(Tuple.Create("0", "Catalogo incompleto [set]"));
                    return xsalida;
                }
                finally
                {
                    if (con.State == ConnectionState.Open)
                    {
                        con.Close();
                    }
                    con.Dispose();
                }
            }
            return xsalida;
        }
        //devolver reglas
        public static IEnumerable<Tuple<string, string>> ReturnReglas(string institucion)
        {
            var xsalida = new HashSet<Tuple<string, string>>();
            var con = getConector();
            if (con == null)
            {
                xsalida.Add(Tuple.Create("0", "Catalogo vacío [get]"));
                return xsalida;
            }
            using (con)
            {
                var com = con.CreateCommand();
                com.CommandType = CommandType.StoredProcedure;
                com.CommandTimeout = 120;
                com.CommandText = "sp_get_regla";
                com.Parameters.AddWithValue("@institucion", institucion);
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
                    log_csl.save_log<Exception>(new Exception(sb.ToString()), "dataServiceHelper", "ReturnReglas", institucion, "N4");
                    xsalida.Add(Tuple.Create("0", "Catalogo incompleto [set]"));
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
            xsalida.Add(Tuple.Create("0", "*Regla*"));
            return xsalida;
        }
        //devolver tipo refrigerado
        public static IEnumerable<Tuple<string, string>> ReturnRefrigeracion()
        {
            var xsalida = new HashSet<Tuple<string, string>>();
            var con = getConector();
            if (con == null)
            {
                xsalida.Add(Tuple.Create("0", "Catalogo vacío [get]"));
                return xsalida;
            }
            using (con)
            {
                var com = con.CreateCommand();
                com.CommandType = CommandType.StoredProcedure;
                com.CommandTimeout = 120;
                com.CommandText = "sp_get_refrigerado";
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
                    log_csl.save_log<Exception>(new Exception(sb.ToString()), "dataServiceHelper", "ReturnRefrigeracion", "null", "N4");
                    xsalida.Add(Tuple.Create("0", "Catalogo incompleto [set]"));
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
        //devolver tipo refrigerado
        public static IEnumerable<Tuple<string, string>> ReturnBancos()
        {
            var xsalida = new HashSet<Tuple<string, string>>();
            var con = getConector();
            if (con == null)
            {
                xsalida.Add(Tuple.Create("0", "Catalogo vacío [get]"));
                return xsalida;
            }
            using (con)
            {
                var com = con.CreateCommand();
                com.CommandType = CommandType.StoredProcedure;
                com.CommandTimeout = 120;
                com.CommandText = "sp_get_banco";
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
                    log_csl.save_log<Exception>(new Exception(sb.ToString()), "dataServiceHelper", "ReturnBancos", "null", "N4");
                    xsalida.Add(Tuple.Create("0", "Catalogo incompleto [set]"));
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
        //devolver los embalajes
        public static IEnumerable<Tuple<string, string>> ReturnEmbalajes()
        {
            var xsalida = new HashSet<Tuple<string, string>>();
            var con = getConector();
            if (con == null)
            {
                xsalida.Add(Tuple.Create("0", "Catalogo vacío [get]"));
                return xsalida;
            }
            using (con)
            {
                var com = con.CreateCommand();
                com.CommandType = CommandType.StoredProcedure;
                com.CommandTimeout = 120;
                com.CommandText = "sp_get_embalajes";
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
                    log_csl.save_log<Exception>(new Exception(sb.ToString()), "dataServiceHelper", "ReturnEmbalajes", "null", "N4");
                    xsalida.Add(Tuple.Create("0", "Catalogo incompleto [set]"));
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
        //devolver los imos
        public static IEnumerable<Tuple<string, string>> ReturnImos()
        {
            var xsalida = new HashSet<Tuple<string, string>>();
            var con = getConector();
            if (con == null)
            {
                xsalida.Add(Tuple.Create("0", "Catalogo vacío [get]"));
                return xsalida;
            }
            using (con)
            {
                var com = con.CreateCommand();
                com.CommandType = CommandType.StoredProcedure;
                com.CommandTimeout = 120;
                com.CommandText = "sp_get_imo";
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
                    log_csl.save_log<Exception>(new Exception(sb.ToString()), "dataServiceHelper", "ReturnImos", "null", "N4");
                    xsalida.Add(Tuple.Create("0", "Catalogo incompleto [set]"));
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
        //devolver los depositos
        public static IEnumerable<Tuple<string, string>> ReturnDepositos()
        {
            var xsalida = new HashSet<Tuple<string, string>>();
            var con = getConector();
            if (con == null)
            {
                xsalida.Add(Tuple.Create("0", "Catalogo vacío [get]"));
                return xsalida;
            }
            using (con)
            {
                var com = con.CreateCommand();
                com.CommandType = CommandType.StoredProcedure;
                com.CommandTimeout = 120;
                com.CommandText = "sp_get_deposito";
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
                    log_csl.save_log<Exception>(new Exception(sb.ToString()), "dataServiceHelper", "ReturnDepositos", "null", "N4");
                    xsalida.Add(Tuple.Create("0", "Catalogo incompleto [set]"));
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
        //comprobar si esta unidad ya existe.
        public static bool? ExistUnit(string unidad, string booking)
        {
            var con = getConector();
            if (con == null)
            {
                return null;
            }
            using (con)
            {
                var com = con.CreateCommand();
                com.CommandType = CommandType.Text;
                com.CommandTimeout = 120;
                com.CommandText = "select [csl_services].[dbo].[fx_ExistUnit](@unidad,@booking)";
                com.Parameters.AddWithValue("@unidad", unidad);
                com.Parameters.AddWithValue("@booking", booking);
                try
                {
                    con.Open();
                    var result = com.ExecuteScalar().ToString();
                    con.Close();
                    return  bool.Parse(result);
                }
                catch (SqlException ex)
                {
                    var sb = new StringBuilder();
                    foreach (SqlError e in ex.Errors)
                    {
                        sb.AppendLine(string.Format("{0}, {1}, {2}, {3}", e.LineNumber, e.Message, e.Procedure, e.Server));
                    }
                    log_csl.save_log<Exception>(new Exception(sb.ToString()), "dataServiceHelper", "ExistUnit", string.Format("{0}.{1}", unidad, booking), "N4");
                    return false;
                }
                finally
                {
                    if (con.State == ConnectionState.Open)
                    {
                        con.Close();
                    }
                   
                }
            }
        
        }
        //devolver los bokings para vacíos/lcl
        public static HashSet<bkObject> GetBookins(string referencia, string fk)
        {
            //gkey,disponible,boking,iso,descripcion,referencia
            var xsalida = new HashSet<bkObject>();
            var con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["n4catalog"].ConnectionString);
            if (con == null)
            {
                return xsalida;
            }
            using (con)
            {
                var com = con.CreateCommand();
                com.CommandType = CommandType.StoredProcedure;
                com.CommandTimeout = 120;
                com.CommandText = "sp_populate_booking";
                com.Parameters.AddWithValue("@booking", DBNull.Value);
                com.Parameters.AddWithValue("@line", DBNull.Value);
                com.Parameters.AddWithValue("@referencia", referencia.Trim().ToUpper());
                com.Parameters.AddWithValue("@fk", fk.Trim().ToUpper());
                try
                {
                    con.Open();
                    using (var reader = com.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var bk = new bkObject();
                            bk.gkey = reader[19] != DBNull.Value ? reader.GetInt64(19).ToString() : string.Empty;
                            bk.cantidad = reader[17] != DBNull.Value ? reader.GetInt64(17).ToString() : string.Empty;
                            bk.nbr = reader[1] != DBNull.Value ? reader[1] as string : string.Empty;
                            bk.iso = reader[20] != null ? reader[20] as string : string.Empty;
                            bk.imo = reader[12] != DBNull.Value ? reader[12] as string : "0";
                            bk.linea = reader[2] != DBNull.Value ? reader[2] as string : string.Empty;
                            bk.referencia = reader[3] != DBNull.Value ? reader[3] as string : string.Empty;
                            bk.descripcion = reader[21] != DBNull.Value ? reader[21] as string : string.Empty;
                            if (reader.GetInt64(17) > 0)
                            {
                                xsalida.Add(bk);
                            }
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
                    log_csl.save_log<Exception>(new Exception(sb.ToString()), "dataServiceHelper", "GetBookins", referencia+":" + fk, "N4");
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
        //exec [dbo].[sp_get_aviso]
        public static string getAvisosCGSA(string formulario=null)
        {
            var xsalida = string.Empty;
            var con = getConector();
            if (con == null)
            {
                return "El Sistema de Solicitud de Servicios, Contecon S.A <br/>le agradece su visita..";
            }
            using (con)
            {
                var com = con.CreateCommand();
                com.CommandType = CommandType.StoredProcedure;
                com.CommandTimeout = 120;
                com.CommandText = "sp_get_aviso";
                int u =0;
                if (!string.IsNullOrEmpty(formulario) && int.TryParse(formulario, out u))
                {
                    com.Parameters.AddWithValue("@zona", u);
                }
                try
                {
                    con.Open();
                    object sale = com.ExecuteScalar();
                    con.Close();
                    if (sale != null && sale.GetType() != typeof(DBNull))
                    {
                        return sale.ToString();
                    }
                    com.Dispose();
                }
                catch (SqlException ex)
                {
                    var sb = new StringBuilder();
                    foreach (SqlError e in ex.Errors)
                    {
                        sb.AppendLine(string.Format("{0}, {1}, {2}, {3}", e.LineNumber, e.Message, e.Procedure, e.Server));
                    }
                    log_csl.save_log<Exception>(new Exception(sb.ToString()), "dataServiceHelper", "getAvisosCGSA", "null", "N4");
                    return "Sistema de Solicitud de Servicios, Contecon S.A <br/>le agradece su visita..";
                }
                finally
                {
                    if (con.State == ConnectionState.Open)
                    {
                        con.Close();
                    }
                    con.Dispose();
                }
            }
            return xsalida;
           
        }
        //pide los datos bbulik
        public static string getBulkCount(int gkeyitem)
        {
            var con = getConector();
            if (con == null)
            {
                return string.Empty;
            }
            if (gkeyitem <= 0)
            {
                return string.Empty;
            }
            using (con)
            {
                var com = con.CreateCommand();
                com.CommandType = CommandType.Text;
                com.CommandTimeout = 60;
                com.CommandText = "select [csl_services].[dbo].[fx_getbbulkCount](@itemgkey)";
                com.Parameters.AddWithValue("@itemgkey", gkeyitem);
                try
                {
                    con.Open();
                    var result = com.ExecuteScalar().ToString();
                    con.Close();
                    return result;
                }
                catch (SqlException ex)
                {
                    var sb = new StringBuilder();
                    foreach (SqlError e in ex.Errors)
                    {
                        sb.AppendLine(string.Format("{0}, {1}, {2}, {3}", e.LineNumber, e.Message, e.Procedure, e.Server));
                    }
                    log_csl.save_log<Exception>(new Exception(sb.ToString()), "dataServiceHelper", "getBulkCount", string.Format("{0}", gkeyitem), "N4");
                    return string.Empty;
                }
                finally
                {
                    if (con.State == ConnectionState.Open)
                    {
                        con.Close();
                    }
                   
                }
            }
        }
        public static string getShipName(string shipID)
        {
            //gkey,disponible,boking,iso,descripcion,referencia
            var xsalida = string.Empty;
            var con = getConector2();
            if (con == null)
            {
                return null;
            }
            using (con)
            {
                var com = con.CreateCommand();
                com.CommandType = CommandType.Text;
                com.CommandTimeout = 120;
                com.CommandText = " select [n5].[dbo].[fxGetShiper](@shipID)";
                com.Parameters.AddWithValue("@shipID", shipID);
                try
                {
                    con.Open();
                    xsalida = com.ExecuteScalar().ToString();
                    con.Close();
                }
                catch (SqlException ex)
                {
                    var sb = new StringBuilder();
                    foreach (SqlError e in ex.Errors)
                    {
                        sb.AppendLine(string.Format("{0}, {1}, {2}, {3}", e.LineNumber, e.Message, e.Procedure, e.Server));
                    }
                    log_csl.save_log<Exception>(new Exception(sb.ToString()), "dataServiceHelper", "getShipName", shipID, "N4");
                    return null;
                }
                finally
                {
                    if (con.State == ConnectionState.Open)
                    {
                        con.Close();
                    }
                   
                } return xsalida;
            }
        }
        //devolver la tabla de riesgos de una unidad.
        public static HashSet<Tuple<string,string>> GetHazards(int hzkey)
        {
            //gkey,disponible,boking,iso,descripcion,referencia
            var xsalida = new HashSet<Tuple<string, string>>();
            var con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["n4catalog"].ConnectionString);
            if (con == null)
            {
                return xsalida;
            }
            using (con)
            {
                var com = con.CreateCommand();
                com.CommandType = CommandType.StoredProcedure;
                com.CommandTimeout = 120;
                com.CommandText = "sp_populate_hazard";
                com.Parameters.AddWithValue("@hzkey", hzkey);
                try
                {
                    con.Open();
                    using (var reader = com.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                           xsalida.Add(Tuple.Create(reader[0] as string,reader[1] as string));
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
                    log_csl.save_log<Exception>(new Exception(sb.ToString()), "dataServiceHelper", "GetHazards", hzkey.ToString(), "N4");
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




        //JCA, 02/12/2015
        //Devolver los datos de detalle catalogo según nombre del catalogo        
        public static IEnumerable<Tuple<string, string>> returnDetalleCatalogo(string nombreCatalogo)
        {
            var xsalida = new HashSet<Tuple<string, string>>();
            var con = getSPConectorSca();
            if (con == null)
            {
                xsalida.Add(Tuple.Create("0", "Catalogo vacío [get]"));
                return xsalida;
            }
            using (con)
            {
                var com = con.CreateCommand();
                com.CommandType = CommandType.StoredProcedure;
                com.CommandTimeout = 120;
                com.CommandText = "CS_ConsultaDetalleCatalogo";
                com.Parameters.AddWithValue("@nombreCatalogo", nombreCatalogo);
                try
                {
                    con.Open();
                    using (var reader = com.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            xsalida.Add(Tuple.Create(reader.GetString(0).ToString(), reader.GetString(1)));
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
                    log_csl.save_log<Exception>(new Exception(sb.ToString()), "dataServiceHelper", "returnDetalleCatalogo", "null", "N4");
                    xsalida.Add(Tuple.Create("0", "Catalogo incompleto [set]"));
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

        public static IEnumerable<Tuple<string, string>> returnServicios()
        {
            var xsalida = new HashSet<Tuple<string, string>>();
            var con = getSPConectorSca();
            if (con == null)
            {
                xsalida.Add(Tuple.Create("0", "Catalogo vacío [get]"));
                return xsalida;
            }
            using (con)
            {
                var com = con.CreateCommand();
                com.CommandType = CommandType.StoredProcedure;
                com.CommandTimeout = 120;
                com.CommandText = "CS_ConsultaServicios";                
                try
                {
                    con.Open();
                    using (var reader = com.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            xsalida.Add(Tuple.Create(reader.GetString(0).ToString(), reader.GetString(1)));
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
                    log_csl.save_log<Exception>(new Exception(sb.ToString()), "dataServiceHelper", "returnServicios", "null", "N4");
                    xsalida.Add(Tuple.Create("0", "Catalogo incompleto [set]"));
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
        

    }
}