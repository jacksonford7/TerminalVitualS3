using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SQLHandler
{
    //conection_basic=> nombre de la cadena de conexion en el archivo

    public class sql_handler
    {
        #region "Fields"
        private string basicCon;
        public string basic_con { get { return this.basicCon; } }

        #endregion
        #region "Singleton"
        private static sql_handler instance = null;

        public static sql_handler GetInstance()
        {
            if (instance == null)
                instance = new sql_handler();
            return instance;
        }

       

        private sql_handler()
        {
             if (string.IsNullOrEmpty(basicCon) && System.Configuration.ConfigurationManager.ConnectionStrings["conection_basic"] != null)
            {
                basicCon = System.Configuration.ConfigurationManager.ConnectionStrings["conection_basic"].ConnectionString;
                var s = TestServer();
                if (!string.IsNullOrEmpty(s))
                {
                    throw new ApplicationException(s);
                }
            }
            else
            {
                throw new ApplicationException("La cadena de conexión conection_basic, no esta presente en el archivo de configuración");
            }
        }

      
        #endregion

        #region "TestMethod"
        public string TestServer(string comando = null)
        {
            using (SqlConnection c = new SqlConnection(this.basicCon))
            {
                try
                {
                    c.Open();
                    var xc = c.CreateCommand();
                    xc.CommandText = string.IsNullOrEmpty(comando) ? "SELECT 1" : comando;
                    xc.ExecuteScalar();
                    xc.Dispose();
                    return string.Empty;
                }
                catch (SqlException e)
                {
                    return SQLControl(e);
                }
                catch (Exception e)
                {
                    if (c.State == ConnectionState.Open) { c.Close(); }
                    return e.Message;
                }
            }
        }
#endregion


#region "Return methods"
        public List<DbDataRecord> ExecuteSelect(string conection, int timeout, string pc_name, Dictionary<string, object> parameters, Action<string> control_error)
        {

            var lx = new List<DbDataRecord>();
            SqlCommand sp_com = new SqlCommand();
            var sql = new SqlConnection(conection);
            sp_com.CommandType = CommandType.StoredProcedure;
            sp_com.Connection = sql;
            sp_com.CommandTimeout = timeout;
            sp_com.CommandText = pc_name;
            if (parameters != null && parameters.Count > 0)
            {
                sp_com.Parameters.AddRange(parseComands(parameters));
            }
            using (sql)
            {
                try
                {
                    sql.Open();
                    SqlDataReader re = sp_com.ExecuteReader(CommandBehavior.CloseConnection);
                    if (re.HasRows)
                    {
                        foreach (DbDataRecord record in re)
                        {
                            lx.Add(record);
                        }
                    }
                    return lx;
                }
                catch (SqlException e)
                {
                    var n = LogEvent<SqlException>("SQL", "ExecuteSelect<DBRecord>", pc_name, false, null, parameters, null, e);
                    control_error?.Invoke(string.Format("Ha ocurrido la novedad número:{0}", n));
                    return null;
                }
                catch (Exception e)
                {
                    var n = LogEvent<Exception>("SQL", "ExecuteSelect<DBRecord>", pc_name, false, null, parameters, null, e);
                    control_error?.Invoke(string.Format("Ha ocurrido la novedad número:{0}", n));
                    return null;
                }
                finally
                {
                    sp_com.Dispose();
                    if (sql.State == ConnectionState.Open)
                        sql.Close();
                }
            }
        }
        public object EscalarFunction(string conection, int timeout, string functionText, Dictionary<string, object> parameters, Action<string> control_error)
        {

            using (var con = new SqlConnection(conection))
            {
                var sp_com = con.CreateCommand();
                sp_com.CommandType = System.Data.CommandType.Text;
                sp_com.Connection = con;
                sp_com.CommandText = functionText;
                sp_com.CommandTimeout = timeout;
                if (parameters != null && parameters.Count > 0)
                {
                    sp_com.Parameters.AddRange(parseComands(parameters));
                }
                try
                {
                    con.Open();
                    var sb = sp_com.ExecuteScalar();
                    if (sb.GetType() == typeof(DBNull))
                    {
                        control_error?.Invoke("Resultado es nulo (DBNULL)");
                        return null;
                    }
                    return sb;
                }
                catch (SqlException e)
                {
                    var n = LogEvent<SqlException>("SQL", nameof(EscalarFunction), functionText, false, null, parameters, null, e);
                    control_error?.Invoke(  string.Format("Ha ocurrido la novedad número:{0}", n));
                    return null;
                }
                catch (Exception e)
                {
                    var n = LogEvent<Exception>("SQL", nameof(EscalarFunction), functionText, false, null, parameters, null, e);
                    control_error?.Invoke(string.Format("Ha ocurrido la novedad número:{0}", n));
                    return null;
                }
                finally
                {
                    if (con.State == System.Data.ConnectionState.Open)
                    {
                        con.Close();
                    }
                    sp_com.Dispose();
                }
            }
        }
        public object EscalarFunction(string conection, int timeout, string functionText, Dictionary<string, object> parameters, out string novedad)
        {

            using (var con = new SqlConnection(conection))
            {
                var sp_com = con.CreateCommand();
                sp_com.CommandType = System.Data.CommandType.Text;
                sp_com.Connection = con;
                sp_com.CommandText = functionText;
                sp_com.CommandTimeout = timeout;
                if (parameters != null && parameters.Count > 0)
                {
                    sp_com.Parameters.AddRange(parseComands(parameters));
                }
                try
                {
                    con.Open();
                    var sb = sp_com.ExecuteScalar();
                    if (sb.GetType() == typeof(DBNull))
                    {
                        novedad = "Retorno es DBNULL";
                        return null;
                    }
                    novedad = "";
                    return sb;
                }
                catch (SqlException e)
                {
                    var n = LogEvent<SqlException>("SQL", nameof(EscalarFunction), functionText, false, null, parameters, null, e);
                    novedad = string.Format("Ha ocurrido la novedad número:{0}", n);
                    return null;
                }
                catch (Exception e)
                {
                    var n = LogEvent<Exception>("SQL", nameof(EscalarFunction), functionText, false, null, parameters, null, e);
                    novedad = string.Format("Ha ocurrido la novedad número:{0}", n);
                    return null;
                }
                finally
                {
                    if (con.State == System.Data.ConnectionState.Open)
                    {
                        con.Close();
                    }
                    sp_com.Dispose();
                }
            }
        }
        public int? ExecuteInsertUpdateDelete(string conection, int timeout, string pc_name, Dictionary<string, object> parameters, Action<string> control_error)
        {

            SqlCommand sp_com = new SqlCommand();
            var sql = new SqlConnection(conection);
            sp_com.CommandType = CommandType.StoredProcedure;
            sp_com.Connection = sql;
            sp_com.CommandTimeout = timeout;
            sp_com.CommandText = pc_name;
            if (parameters != null && parameters.Count > 0)
            {
                sp_com.Parameters.AddRange(parseComands(parameters));
            }
            using (sql)
            {
                try
                {
                    sql.Open();
                    //insert = -1, up/del rw afectted
                    var t = sp_com.ExecuteNonQuery();
                    if (t < 0) { t = 1; }
                    return t;
                }
                catch (SqlException e)
                {
                    var n = LogEvent<SqlException>("SQL", nameof(ExecuteInsertUpdateDelete), pc_name, false, null, parameters, null, e);
                    control_error?.Invoke(string.Format("Ha ocurrido la novedad número:{0}", n));
                    return null;
                }
                catch (Exception e)
                {
                    var n = LogEvent<Exception>("SQL", nameof(ExecuteInsertUpdateDelete), pc_name, false, null, parameters, null, e);
                    control_error?.Invoke(string.Format("Ha ocurrido la novedad número:{0}", n));
                    return null;
                }
                finally
                {
                    sp_com.Dispose();
                    if (sql.State == ConnectionState.Open)
                        sql.Close();
                }
            }
        }
        public int? ExecuteInsertUpdateDelete(string conection, int timeout, string pc_name, Dictionary<string, object> parameters, out string Error)
        {
            Error = string.Empty;
            SqlCommand sp_com = new SqlCommand();
            var sql = new SqlConnection(conection);
            sp_com.CommandType = CommandType.StoredProcedure;
            sp_com.Connection = sql;
            sp_com.CommandTimeout = timeout;
            sp_com.CommandText = pc_name;
            if (parameters != null && parameters.Count > 0)
            {
                sp_com.Parameters.AddRange(parseComands(parameters));
            }
            using (sql)
            {
                try
                {
                    sql.Open();
                    //insert = -1, up/del rw afectted
                    var t = sp_com.ExecuteNonQuery();
                    
                    //si se insertó entonces continue
                    if(t < 0) { t = 1; }
                    return t;
                }
                catch (SqlException e)
                {
                    var n = LogEvent<SqlException>("SQL", nameof(ExecuteInsertUpdateDelete), pc_name, false, null, parameters, null, e);
                    Error=  string.Format("Ha ocurrido la novedad número:{0}", n);
                    return null;
                }
                catch (Exception e)
                {
                    var n = LogEvent<Exception>("SQL", nameof(ExecuteInsertUpdateDelete), pc_name, false, null, parameters, null, e);
                    Error = string.Format("Ha ocurrido la novedad número:{0}", n);
                    return null;
                }
                finally
                {
                    sp_com.Dispose();
                    if (sql.State == ConnectionState.Open)
                        sql.Close();
                }
            }
        }
        public Int64? ExecuteInsertUpdateDeleteReturn(string conection, int timeout, string pc_name, Dictionary<string, object> parameters, Action<string> control_error)
        {
            //for sql ->  select @@identity in normal transaction;

            SqlCommand sp_com = new SqlCommand();
            var sql = new SqlConnection(conection);
            sp_com.CommandType = CommandType.StoredProcedure;
            sp_com.Connection = sql;
            sp_com.CommandTimeout = timeout;
            sp_com.CommandText = pc_name;
            if (parameters != null && parameters.Count > 0)
            {
                sp_com.Parameters.AddRange(parseComands(parameters));
            }
            using (sql)
            {
                try
                {
                    sql.Open();
                    var t = sp_com.ExecuteScalar() as Int64?;
                    return t;
                }
                catch (SqlException e)
                {
                    var n = LogEvent<SqlException>("SQL", nameof(ExecuteInsertUpdateDeleteReturn), pc_name, false, null, parameters, null, e);
                    control_error?.Invoke(string.Format("Ha ocurrido la novedad número:{0}", n));
                    return null;
                }
                catch (Exception e)
                {
                    var n = LogEvent<Exception>("SQL",nameof(ExecuteInsertUpdateDeleteReturn), pc_name, false, null, parameters, null, e);
                    control_error?.Invoke(string.Format("Ha ocurrido la novedad número:{0}", n));
                    return null;
                }
                finally
                {
                    sp_com.Dispose();
                    if (sql.State == ConnectionState.Open)
                        sql.Close();
                }
            }
        }
        public Int64? ExecuteInsertUpdateDeleteReturn(string conection, int timeout, string pc_name, Dictionary<string, object> parameters, out string control_error)
        {
            //for sql ->  select @@identity in normal transaction;

            SqlCommand sp_com = new SqlCommand();
            var sql = new SqlConnection(conection);
            sp_com.CommandType = CommandType.StoredProcedure;
            sp_com.Connection = sql;
            sp_com.CommandTimeout = timeout;
            sp_com.CommandText = pc_name;
            if (parameters != null && parameters.Count > 0)
            {
                sp_com.Parameters.AddRange(parseComands(parameters));
            }
            using (sql)
            {
                try
                {
                    sql.Open();
                    var t = sp_com.ExecuteScalar() as Int64?;
                    control_error = string.Empty;
                    return t;
                }
                catch (SqlException e)
                {
                    var n = LogEvent<Exception>("SQL", nameof(ExecuteInsertUpdateDeleteReturn), pc_name, false, null, parameters, null, e);
                    control_error = string.Format("Ha ocurrido la novedad número:{0}", n);
                    return null;
                }
                catch (Exception e)
                {
                    var n = LogEvent<Exception>("SQL", nameof(ExecuteInsertUpdateDeleteReturn), pc_name, false, null, parameters, null, e);
                    control_error = string.Format("Ha ocurrido la novedad número:{0}", n);
                    return null;
                }
                finally
                {
                    sp_com.Dispose();
                    if (sql.State == ConnectionState.Open)
                        sql.Close();
                }
            }
        }
        public SqlParameter[] parseComands(Dictionary<string, object> plist)
        {
            if (plist == null || plist.Count <= 0)
            {
                return null;
            }
            SqlParameter[] colectionPars = new SqlParameter[plist.Count];
            var i = 0;
            foreach (var f in plist)
            {
                var pq = new SqlParameter();
                pq.Direction = ParameterDirection.Input;
                pq.ParameterName = string.Format("@{0}", f.Key);
                pq.Value = f.Value;
                colectionPars[i] = pq;
                i++;
            }
            return colectionPars;
        }
        private static SqlParameter[] parseOntrace(Dictionary<string, object> plist)
        {
            if (plist == null || plist.Count <= 0)
            {
                return null;
            }
            SqlParameter[] colectionPars = new SqlParameter[plist.Count];
            var i = 0;
            foreach (var f in plist)
            {
                var pq = new SqlParameter();
                pq.Direction = ParameterDirection.Input;
                pq.ParameterName = string.Format("@{0}", f.Key);
                pq.Value = f.Value;
                colectionPars[i] = pq;
                i++;
            }
            return colectionPars;
        }
        private static string SQLControl(SqlException s)
        {
            StringBuilder sb = new StringBuilder();
            foreach (SqlError ei in s.Errors)
            {
                sb.AppendLine(string.Format("{0}, {1}, {2}, {3}", ei.LineNumber, ei.Message, ei.Procedure, ei.Server));
            }
            return sb.ToString();
        }
        //For not sequential -select-, db files
        public IEnumerable<DbDataRecord> ExecuteSelectOnline(string conection, int timeout, string pc_name, Dictionary<string, object> parameters)
        {

            SqlCommand sp_com = new SqlCommand();
            var sql = new SqlConnection(conection);
            sp_com.CommandType = CommandType.StoredProcedure;
            sp_com.Connection = sql;
            sp_com.CommandTimeout = timeout;
            sp_com.CommandText = pc_name;
            if (parameters != null && parameters.Count > 0)
            {
                sp_com.Parameters.AddRange(parseComands(parameters));
            }
            using (sql)
            {
                sql.Open();
                using (var registers = sp_com.ExecuteReader())
                {
                    foreach (DbDataRecord register in registers)
                    {
                        yield return register;
                    }
                }
            }
            sp_com.Dispose();
        }
        public IEnumerable<T> ExecuteSelectOnline<T>(string conection, int timeout, string pc_name, Dictionary<string, object> parameters)
        {

            SqlCommand sp_com = new SqlCommand();
            var sql = new SqlConnection(conection);
            sp_com.CommandType = CommandType.StoredProcedure;
            sp_com.Connection = sql;
            sp_com.CommandTimeout = timeout;
            sp_com.CommandText = pc_name;
            if (parameters != null && parameters.Count > 0)
            {
                sp_com.Parameters.AddRange(parseComands(parameters));
            }
            using (sql)
            {
                sql.Open();
                using (var registers = sp_com.ExecuteReader())
                {
                    foreach (DbDataRecord register in registers)
                    {
                        yield return DataRecordToObject<T>(register);
                    }
                }
            }
            sp_com.Dispose();
        }
        //For sequential list of objects
        public List<T> ExecuteSelect<T>(string conection, int timeout, string pc_name, Dictionary<string, object> parameters, Action<string> control_error)
        {

            var lx = new List<DbDataRecord>();
            SqlCommand sp_com = new SqlCommand();
            var sql = new SqlConnection(conection);
            sp_com.CommandType = CommandType.StoredProcedure;
            sp_com.Connection = sql;
            sp_com.CommandTimeout = timeout;
            sp_com.CommandText = pc_name;
            if (parameters != null && parameters.Count > 0)
            {
                sp_com.Parameters.AddRange(parseComands(parameters));
            }
            using (sql)
            {
                try
                {
                    sql.Open();
                    SqlDataReader re = sp_com.ExecuteReader(CommandBehavior.CloseConnection);
                    if (re.HasRows)
                    {
                        return DataReaderToToList<T>(re);
                    }
                    return new List<T>();
                }
                catch (SqlException e)
                {
                    var n = LogEvent<SqlException>("SQL", "ExecuteSelect<T>", pc_name, false, null, parameters, null, e);
                    control_error?.Invoke(string.Format("Ha ocurrido la novedad número:{0}", n));
                    return null;
                }
                catch (Exception e)
                {
                    var n = LogEvent<Exception>("SQL", "ExecuteSelect<T>", pc_name, false, null, parameters, null, e);
                    control_error?.Invoke(string.Format("Ha ocurrido la novedad número:{0}", n));
                    return null;
                }
                finally
                {
                    sp_com.Dispose();
                    if (sql.State == ConnectionState.Open)
                        sql.Close();
                }

            }
        }
        public List<T> ExecuteSelectControl<T>(string conection, int timeout, string pc_name, Dictionary<string, object> parameters, out string OError)
        {

            var lx = new List<DbDataRecord>();
            SqlCommand sp_com = new SqlCommand();
            var sql = new SqlConnection(conection);
            sp_com.CommandType = CommandType.StoredProcedure;
            sp_com.Connection = sql;
            sp_com.CommandTimeout = timeout;
            sp_com.CommandText = pc_name;
            if (parameters != null && parameters.Count > 0)
            {
                sp_com.Parameters.AddRange(parseComands(parameters));
            }

            using (sql)
            {
                try
                {
                    sql.Open();
                    SqlDataReader re = sp_com.ExecuteReader(CommandBehavior.CloseConnection);
                    OError = string.Empty;
                    if (re.HasRows)
                    {
                        return DataReaderToToList<T>(re);
                    }
                    return new List<T>();
                }
                catch (SqlException e)
                {
                    var n = LogEvent<SqlException>("SQL", nameof(ExecuteSelectControl), pc_name, false, null, parameters, null, e);
                    OError = string.Format("Ha ocurrido la novedad número:{0}", n);
                    return null;
                }
                catch (Exception e)
                {
                    var n = LogEvent<Exception>("SQL", nameof(ExecuteSelectControl), pc_name, false, null, parameters, null, e);
                    OError = string.Format("Ha ocurrido la novedad número:{0}", n);
                    return null;
                }
                finally
                {
                    sp_com.Dispose();
                    if (sql.State == ConnectionState.Open)
                        sql.Close();
                }

            }
        }
        public data_message ExecuteSelectOnly(string conection, int timeout, string pc_name, Dictionary<string, object> parameters)
        {

            var lx = new List<DbDataRecord>();
            SqlCommand sp_com = new SqlCommand();
            var sql = new SqlConnection(conection);
            sp_com.CommandType = CommandType.StoredProcedure;
            sp_com.Connection = sql;
            sp_com.CommandTimeout = timeout;
            sp_com.CommandText = pc_name;
            if (parameters != null && parameters.Count > 0)
            {
                sp_com.Parameters.AddRange(parseComands(parameters));
            }

            using (sql)
            {
                try
                {
                    sql.Open();
                    SqlDataReader re = sp_com.ExecuteReader(CommandBehavior.CloseConnection);
                    if (re.HasRows)
                    {
                        re.Read();
                        return new data_message() { code = re.GetInt32(0), message = re.GetString(1) };
                    }
                    return new data_message() { code = -1, message = "La búsqueda no obtuvo resultados, cambie los parámetros y reintente" };
                }
                catch (SqlException e)
                {
                    var n = LogEvent<Exception>("SQL", nameof(ExecuteSelectOnly), pc_name, false, null, parameters, null, e);
                    return new data_message() { code = -2, message = string.Format("Ha ocurrido la novedad número:{0}", n) };
                }
                catch (Exception e)
                {
                    var n=  LogEvent<Exception>("SQL", nameof(ExecuteSelectOnly), pc_name, false, null, parameters, null, e);
                    return new data_message() { code = -2, message = string.Format("Ha ocurrido la novedad número:{0}", n) };
                }
                finally
                {
                    sp_com.Dispose();
                    if (sql.State == ConnectionState.Open)
                        sql.Close();
                }

            }
        }
        public T ExecuteSelectOnly<T>(string conection, int timeout, string pc_name, Dictionary<string, object> parameters) where T : class
        {
            /* NULL, hubo un error / o no encontrado   */

            SqlCommand sp_com = new SqlCommand();
            var sql = new SqlConnection(conection);
            sp_com.CommandType = CommandType.StoredProcedure;
            sp_com.Connection = sql;
            sp_com.CommandTimeout = timeout;
            sp_com.CommandText = pc_name;
            if (parameters != null && parameters.Count > 0)
            {
                sp_com.Parameters.AddRange(parseComands(parameters));
            }
            using (sql)
            {
                try
                {
                    sql.Open();
                    SqlDataReader re = sp_com.ExecuteReader(CommandBehavior.CloseConnection);
                    if (re.HasRows)
                    {
                        re.Read();
                        return DataReaderToObject<T>(re);
                    }
                    return null;
                }
                catch (SqlException e)
                {
                    LogEvent<SqlException>("SQL", nameof(ExecuteSelect), pc_name,false,null,parameters,null,e);
                    return null;
                }
                catch (Exception e)
                {
                    LogEvent<Exception>("SQL", nameof(ExecuteSelect), pc_name, false, null, parameters, null, e);
                    return null;
                }
                finally
                {
                    sp_com.Dispose();
                    if (sql.State == ConnectionState.Open)
                        sql.Close();
                }

            }
        }

#endregion

#region "Map Methods"
        private T DataRecordToObject<T>(DbDataRecord dr)
        {
            var columns = Enumerable.Range(0, dr.FieldCount)
                    .Select(dr.GetName)
                    .ToList();
            T obj = default(T);
            obj = Activator.CreateInstance<T>();
            foreach (PropertyInfo prop in obj.GetType().GetProperties())
            {
                if (columns.FindIndex(s => s.ToLower().Equals(prop.Name.ToLower())) >= 0 && !object.Equals(dr[prop.Name.ToLower()], DBNull.Value))
                {
                    prop.SetValue(obj, dr[prop.Name], null);
                }
            }
            return obj;
        }

        private T DataReaderToObject<T>(IDataReader dr)
        {
            var columns = Enumerable.Range(0, dr.FieldCount)
                    .Select(dr.GetName)
                    .ToList();
            T obj = default(T);
            obj = Activator.CreateInstance<T>();
            foreach (PropertyInfo prop in obj.GetType().GetProperties())
            {
                if (columns.FindIndex(s => s.ToLower().Equals(prop.Name.ToLower())) >= 0 && !object.Equals(dr[prop.Name.ToLower()], DBNull.Value))
                {
                    prop.SetValue(obj, dr[prop.Name], null);
                }
            }
            return obj;
        }

        private List<T> DataReaderToToList<T>(IDataReader dr)
        {
            List<T> list = new List<T>();
            var columns = Enumerable.Range(0, dr.FieldCount)
                        .Select(dr.GetName)
                        .ToList();
            T obj = default(T);
            while (dr.Read())
            {
                obj = Activator.CreateInstance<T>();
                foreach (PropertyInfo prop in obj.GetType().GetProperties())
                {
                    if (columns.FindIndex(s => s.ToLower().Equals(prop.Name.ToLower())) >= 0 && !object.Equals(dr[prop.Name.ToLower()], DBNull.Value))
                    {
                        prop.SetValue(obj, dr[prop.Name], null);
                    }
                }
                list.Add(obj);
            }
            return list;
        }
#endregion

#region "Trace"
        public static Int64? LogEvent<T>(string _user,string _source,string _method, bool _result, string _object=null, Dictionary<string, object> _parameter_input=null, string _result_execution_message=null, T _exception = null) where T : System.Exception
        {
            try
            {
                string conn = System.Configuration.ConfigurationManager.ConnectionStrings["conection_basic"]?.ConnectionString;
                if (string.IsNullOrEmpty(conn))
                {
                    return -2;
                }
                Int64 result = 0;
                conn = string.Format("{0};Enlist=\"false\"",conn);

                using (var conexion = new SqlConnection(conn))
                {
                  
                    try
                    {
                        using (var comando = conexion.CreateCommand())
                        {
                            comando.CommandType = CommandType.StoredProcedure;
                            comando.CommandText = "PC_I_TB_APLICATION_LOG";
                            comando.Parameters.AddWithValue("@user", _user);
                            comando.Parameters.AddWithValue("@source", _source);
                            comando.Parameters.AddWithValue("@method", _method);
                            comando.Parameters.AddWithValue("@result", _result);
                            comando.Transaction = null;
                            
                            //Objeto
                            if (!string.IsNullOrEmpty(_object))
                            {
                                comando.Parameters.AddWithValue("@object", _object);
                            }
                            //parametros
                            if (_parameter_input != null && _parameter_input.Count > 0)
                            {
                                StringBuilder sbb = new StringBuilder();
                                foreach (var i in _parameter_input)
                                {
                                    sbb.Append(string.Format("{0}={1}, ", i.Key, i.Value?.ToString()));
                                }
                                comando.Parameters.AddWithValue("@parameter_string", _result);
                            }
                            if (_exception != null)
                            {
                                var s = _exception as SqlException;
                                if (_exception.GetType() == typeof(SqlException) && s != null)
                                {
                                    comando.Parameters.AddWithValue("@primary_exception_message", SQLControl(s));
                                }
                                else
                                {
                                    comando.Parameters.AddWithValue("@primary_exception_message", _exception.Message);
                                }
                                comando.Parameters.AddWithValue("@secondary_exception_message", _exception.InnerException != null ? _exception.InnerException.Message : null);
                           }
                            if (!string.IsNullOrEmpty(_result_execution_message))
                            {
                                comando.Parameters.AddWithValue("@result_execution_message", _result_execution_message);
                            }
                            if (conexion.State == ConnectionState.Closed) { conexion.Open(); }
                            result = Int64.Parse(comando.ExecuteScalar().ToString());
                            conexion.EnlistTransaction(null);
                            //conexion.EnlistDistributedTransaction(null);
                            conexion.Close();
                            return result;
                        }
                    }
                    catch (Exception ex)
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
#endregion

    }
}
