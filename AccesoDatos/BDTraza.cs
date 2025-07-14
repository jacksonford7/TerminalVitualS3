using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace AccesoDatos
{
    public class BDTraza
    {
        private static BDCon db = null;
        private static string cc_er = string.Empty;
        private static bool _errsql;
        private static void init()
        {
            db = new BDCon();
            _errsql  = db.SetBasica("DATACON").Exitoso;
        }
        public static Int64? LogEvent<T>(string usuario, string origen, string metodo, int resultado, string clase = null, Dictionary<string, object> parametros = null, string resultado_ejecucion = null, T _exception = null) where T : System.Exception
        {
            try
            {
                init();
                if (!_errsql) { return -2; }
                Int64 result = 0;
                string conn = db.CadenaBasica;
                conn = string.Format("{0};Enlist=\"false\"", conn);
                using (var conexion = new SqlConnection(conn))
                {
                    try
                    {
                        using (var comando = conexion.CreateCommand())
                        {
                            comando.CommandType = CommandType.StoredProcedure;
                            comando.CommandText = "pc_bil_traza_insert";
                            comando.Parameters.AddWithValue("@usuario", usuario);
                            comando.Parameters.AddWithValue("@origen", origen);
                            comando.Parameters.AddWithValue("@metodo", metodo);
                            comando.Parameters.AddWithValue("@resultado", resultado);
                            comando.Transaction = null;

                            //Objeto
                            if (!string.IsNullOrEmpty(clase))
                            {
                                comando.Parameters.AddWithValue("@clase", clase);
                            }
                            //parametros
                            if (parametros != null && parametros.Count > 0)
                            {
                                StringBuilder sbb = new StringBuilder();
                                sbb.Append("<parametros>");
                                foreach (var i in parametros)
                                {
                                    if (IsXml(i.Value?.ToString()))
                                    {
                                        sbb.AppendFormat("<parametro nombre=\"{0}\" >{1}</parametro>",i.Key,CommentXml(  i.Value?.ToString()));
                                    }
                                    else
                                    {
                                        sbb.Append(string.Format("<parametro nombre=\"{0}\" valor=\"{1}\" />", i.Key, i.Value?.ToString()));
                                    }

                                   
                                }
                                sbb.Append("</parametros>");
                                comando.Parameters.AddWithValue("@parametros_ejecucion", sbb.ToString());
                            }
                            if (_exception != null)
                            {
                                var s = _exception as SqlException;
                                if (_exception.GetType() == typeof(SqlException) && s != null)
                                {
                                    comando.Parameters.AddWithValue("@mensaje_excepcion_primaria",BDCon.SQLControl(s));
                                }
                                else
                                {
                                    comando.Parameters.AddWithValue("@mensaje_excepcion_primaria", _exception.Message);
                                }
                                comando.Parameters.AddWithValue("@mensaje_excepcion_secundaria", _exception.InnerException != null ? _exception.InnerException.Message : null);
                            }
                            if (!string.IsNullOrEmpty(resultado_ejecucion))
                            {
                                comando.Parameters.AddWithValue("@resultado_ejecucion", resultado_ejecucion);
                            }
                            if (conexion.State == ConnectionState.Closed) { conexion.Open(); }
                            result = Int64.Parse(comando.ExecuteScalar().ToString());
                            conexion.EnlistTransaction(null);
                            conexion.Close();
                            return result;
                        }
                    }
                    catch(Exception ex)
                    {
                        throw new Exception("Null", ex);
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
            catch
            {
                return null;
            }
        }
        public static Int64? TraceEvent<T>(string usuario, string origen, string metodo, T objeto, string resultado_ejecucion = null) where T : class
        {
            try
            {
                init();
                if (!_errsql) { return -2; }
                Int64 result = 0;
                string conn = db.CadenaBasica;
                conn = string.Format("{0};Enlist=\"false\"", conn);
                using (var conexion = new SqlConnection(conn))
                {
                    try
                    {
                        using (var comando = conexion.CreateCommand())
                        {
                            comando.CommandType = CommandType.StoredProcedure;
                            comando.CommandText = "pc_bil_traza_insert";
                            comando.Parameters.AddWithValue("@usuario", usuario);
                            comando.Parameters.AddWithValue("@origen", origen);
                            comando.Parameters.AddWithValue("@metodo", metodo);
                            comando.Parameters.AddWithValue("@resultado", 1);
                            comando.Transaction = null;
                            //Objeto
                            if (objeto != null)
                            {
                                comando.Parameters.AddWithValue("@clase", objeto?.GetType().Name);
                            }
                            //parametros
                            if (objeto != null)
                            {
                                var s = BDMap.Serializar<T>(objeto,out cc_er );
                                if (!string.IsNullOrEmpty(cc_er))
                                {
                                    s = cc_er;
                                }
                                comando.Parameters.AddWithValue("@parametros_ejecucion", s);
                            }
                            comando.Parameters.AddWithValue("@resultado_ejecucion", resultado_ejecucion);
                            if (conexion.State == ConnectionState.Closed) { conexion.Open(); }
                            result = Int64.Parse(comando.ExecuteScalar().ToString());
                            conexion.EnlistTransaction(null);
                            conexion.Close();
                            return result;
                        }
                    }
                    catch
                    {
                        return null;
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
            catch
            {
                return null;
            }
        }
        public static Int64? TraceMove(string usuario, string origen, string clase, string metodo,  string resultado_ejecucion) 
        {
            try
            {
                init();
                if (!_errsql) { return -2; }
                Int64 result = 0;
                string conn = db.CadenaBasica;
                conn = string.Format("{0};Enlist=\"false\"", conn);
                using (var conexion = new SqlConnection(conn))
                {
                    try
                    {
                        using (var comando = conexion.CreateCommand())
                        {
                            comando.CommandType = CommandType.StoredProcedure;
                            comando.CommandText = "pc_bil_traza_insert";
                            comando.Parameters.AddWithValue("@usuario", usuario);
                            comando.Parameters.AddWithValue("@origen", origen);
                            comando.Parameters.AddWithValue("@metodo", metodo);
                            comando.Parameters.AddWithValue("@resultado", 2);
                            comando.Transaction = null;
                            //Objeto
                            comando.Parameters.AddWithValue("@clase", clase);
                            //parametros

                            comando.Parameters.AddWithValue("@resultado_ejecucion", resultado_ejecucion);
                            if (conexion.State == ConnectionState.Closed) { conexion.Open(); }
                            result = Int64.Parse(comando.ExecuteScalar().ToString());
                            conexion.EnlistTransaction(null);
                            conexion.Close();
                            return result;
                        }
                    }
                    catch
                    {
                        return null;
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
            catch
            {
                return null;
            }
        }
        public static bool IsXml(string input)
        {
            return (input.StartsWith("<") && input.EndsWith(">"));
        }
        private static string CommentXml(object xm)
        {
            //<![CDATA[         characters with markup  ]]>
            if ( xm != null  && IsXml(xm.ToString()))
            {
                return string.Format("<![CDATA[{0}]]>",xm.ToString());
            }
            return xm.ToString();
        }
        //nuevo guarda un campo secuencia para pescar carga
        public static Int64? TraceMove(Int64 sq,string usuario, string origen, string clase, string metodo, string resultado_ejecucion)
        {
            try
            {
                init();
                if (!_errsql) { return -2; }
                Int64 result = 0;
                string conn = db.CadenaBasica;
                conn = string.Format("{0};Enlist=\"false\"", conn);
                using (var conexion = new SqlConnection(conn))
                {
                    try
                    {
                        using (var comando = conexion.CreateCommand())
                        {
                            comando.CommandType = CommandType.StoredProcedure;
                            comando.CommandText = "pc_bil_traza_insert";
                            comando.Parameters.AddWithValue("@usuario", usuario);
                            comando.Parameters.AddWithValue("@origen", origen);
                            comando.Parameters.AddWithValue("@metodo", metodo);
                            comando.Parameters.AddWithValue("@resultado", 2);
                            comando.Parameters.AddWithValue("@sqc", sq);
                            comando.Transaction = null;
                            //Objeto
                            comando.Parameters.AddWithValue("@clase", clase);
                            //parametros

                            comando.Parameters.AddWithValue("@resultado_ejecucion", resultado_ejecucion);
                            if (conexion.State == ConnectionState.Closed) { conexion.Open(); }
                            result = Int64.Parse(comando.ExecuteScalar().ToString());
                            conexion.EnlistTransaction(null);
                            conexion.Close();
                            return result;
                        }
                    }
                    catch
                    {
                        return null;
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
            catch
            {
                return null;
            }
        }

    }
}
