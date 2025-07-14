using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Threading;

namespace CSLSite.app_start
{
    public class dataHelper
    {
        //Mapeo de Objeto a Clase
        public static T crearInstancia<T>(IDataRecord reader)
        {
            var properties = typeof(T).GetProperties();
            var item = Activator.CreateInstance<T>();
            foreach (var property in typeof(T).GetProperties())
            {
                try
                {
                    var o = reader.GetOrdinal(property.Name);
                    if (!reader.IsDBNull(reader.GetOrdinal(property.Name)))
                    {
                        Type convertTo = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;
                        property.SetValue(item, Convert.ChangeType(reader[property.Name], convertTo), null);
                    }
                }
                catch (System.IndexOutOfRangeException)
                {


                }
                catch (Exception)
                { }

            }
            return item;
        }
        //cadena de conexion, comando o sp, parametros, es sp 1-0, metodo en caso de error
        public static List<T> selectData<T>(string cadena, string comando, Dictionary<string, object> parametros, bool sp, Action<string, string, string, string, Exception> onError, string clase, string metodo, string nota)
        {

            if (System.Configuration.ConfigurationManager.ConnectionStrings[cadena] == null)
            {
                if (onError != null)
                {
                    onError(clase, metodo, nota, "CONF", new ApplicationException(string.Format("La cadena de conexión [{0}], no está registrada en el archivo de configuración", cadena)));
                }
                return null;
            }
            var resultado = new List<T>();
            cadena = System.Configuration.ConfigurationManager.ConnectionStrings[cadena].ConnectionString;
            try
            {
                using (var conexion = new SqlConnection(cadena))
                {
                    var Sqlcomando = conexion.CreateCommand();
                    Sqlcomando.CommandType = sp ? System.Data.CommandType.StoredProcedure : System.Data.CommandType.Text;
                    Sqlcomando.CommandText = comando.Trim();
                    foreach (var c in parametros)
                    {
                        Sqlcomando.Parameters.AddWithValue(string.Format("@{0}", c.Key), c.Value);
                    }
                    conexion.Open();
                    var reader = Sqlcomando.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                    while (reader.Read())
                    {
                        resultado.Add(crearInstancia<T>(reader));
                    }
                    return resultado;
                }
            }
            catch (SqlException e)
            {
                if (onError != null)
                {
                    onError(clase, metodo, nota, "SQL", e);
                }
                return null;
            }
            catch (Exception e)
            {
                if (onError != null)
                {
                    onError(clase, metodo, nota, "GRAL", e);
                }
                return null;
            }
        }
        //metodo que ejecuta insert
        public static bool insertData(string cadena, string comando, Dictionary<string, object> parametros, bool sp, Action<string, string, string, string, Exception> onError, string clase, string metodo, string nota)
        {

            if (System.Configuration.ConfigurationManager.ConnectionStrings[cadena] == null)
            {
                if (onError != null)
                {
                    onError(clase, metodo, nota, "CONF", new ApplicationException(string.Format("La cadena de conexión [{0}], no está registrada en el archivo de configuración", cadena)));
                }
                return false;
            }
            cadena = System.Configuration.ConfigurationManager.ConnectionStrings[cadena].ConnectionString;


            var async = false;
            if (parametros.ContainsKey("async"))
            {
                parametros.Remove("async");
                async = true;
            }
            var flag = false;
            if (async)
            {
                ThreadPool.QueueUserWorkItem(delegate
                {
                    using (var conexion = new SqlConnection(cadena))
                    {
                        using (SqlCommand command = new SqlCommand(comando.Trim(), conexion))
                        {
                            command.CommandType = sp ? System.Data.CommandType.StoredProcedure : System.Data.CommandType.Text;

                            try
                            {
                                conexion.Open();
                                foreach (var c in parametros)
                                {
                                    command.Parameters.AddWithValue(string.Format("@{0}", c.Key), c.Value);
                                }
                                command.ExecuteNonQuery();
                                flag = true;
                            }
                            catch (Exception ex)
                            {

                                if (onError != null)
                                {
                                    onError(clase, metodo, nota, "SQL-async", ex);
                                }
                                flag = false;
                            }
                        }
                    }
                });
                return flag;
            }
            try
            {
                using (var conexion = new SqlConnection(cadena))
                {
                    var Sqlcomando = conexion.CreateCommand();
                    Sqlcomando.CommandType = sp ? System.Data.CommandType.StoredProcedure : System.Data.CommandType.Text;
                    Sqlcomando.CommandText = comando.Trim();
                    foreach (var c in parametros)
                    {
                        Sqlcomando.Parameters.AddWithValue(string.Format("@{0}", c.Key), c.Value);
                    }
                    conexion.Open();
                    Sqlcomando.ExecuteNonQuery();
                    conexion.Close();
                    Sqlcomando.Dispose();
                    return true;
                }

            }
            catch (SqlException e)
            {
                if (onError != null)
                {
                    onError(clase, metodo, nota, "SQL", e);
                }
                return false;
            }
            catch (Exception e)
            {
                if (onError != null)
                {
                    onError(clase, metodo, nota, "GRAL", e);
                }
                return true;
            }
        }
        //metodo ejecuta valor escalar
        public static T selectScalar<T>(string cadena, string comando, Dictionary<string, object> parametros, bool sp, Action<string, string, string, string, Exception> onError, string clase, string metodo, string nota)
        {
            if (System.Configuration.ConfigurationManager.ConnectionStrings[cadena] == null)
            {
                if (onError != null)
                {
                    onError(clase, metodo, nota, "CONF", new ApplicationException(string.Format("La cadena de conexión [{0}], no está registrada en el archivo de configuración", cadena)));
                }
                return default(T);
            }
            var resultado = new List<T>();
            cadena = System.Configuration.ConfigurationManager.ConnectionStrings[cadena].ConnectionString;
            try
            {
                using (var conexion = new SqlConnection(cadena))
                {
                    var Sqlcomando = conexion.CreateCommand();
                    Sqlcomando.CommandType = sp ? System.Data.CommandType.StoredProcedure : System.Data.CommandType.Text;
                    Sqlcomando.CommandText = comando.Trim();
                    foreach (var c in parametros)
                    {
                        Sqlcomando.Parameters.AddWithValue(string.Format("@{0}", c.Key), c.Value);
                    }
                    conexion.Open();
                    var reader = Sqlcomando.ExecuteScalar();
                    T salida = default(T);
                    if (reader != null && reader.GetType() != typeof(DBNull))
                    {
                        salida = (T)Convert.ChangeType(reader, typeof(T));
                    }
                    return salida;
                }
            }
            catch (SqlException e)
            {
                if (onError != null)
                {
                    onError(clase, metodo, nota, "SQL", e);
                }
                return default(T);
            }
            catch (Exception e)
            {
                if (onError != null)
                {
                    onError(clase, metodo, nota, "GRAL", e);
                }
                return default(T);
            }
        }


    }
}