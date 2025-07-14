using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CSLSite.CatalogosTableAdapters;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Text;
using csl_log;


namespace CSLSite
{
    public class turnoConsolidacion
    {
        public string booking { get; set; }
        public string fecha_pro { get; set; }
        public string mail { get; set; }
        public string idlinea { get; set; }
        public string linea { get; set; }
        public string total { get; set; }
        public string usuario { get; set; }
        public List<tdetalle> detalles { get; set; }
        public static bool validar(turnoConsolidacion trn, out string validacion_error)
        {
            CultureInfo enUS = new CultureInfo("en-US");
            DateTime fecha;
            int valida = 0;
            int valida1 = 0;
            int valot = 0;

            //validar booking----
            if (string.IsNullOrEmpty(trn.usuario))
            {
                validacion_error = "*Problema de conexión*\nPor favor salga y vuelva a entrar al sistema";
                return false;
            }


            //validar booking----
            if (string.IsNullOrEmpty(trn.booking))
            {
                validacion_error = "*Datos de programación*\n Escriba el número de booking";
                return false;
            }
            //valida fecha-----
            if (!DateTime.TryParseExact(trn.fecha_pro.Replace("-", "/"), "dd/MM/yyyy", enUS, DateTimeStyles.None, out fecha))
            {
                validacion_error = "*Datos del programación*\nLa fecha de programación no tiene el formato correcto";
                return false;
            }

            /*
            //validar id_linea que viene en sspp----
            if (string.IsNullOrEmpty(trn.idlinea))
            {
                validacion_error = "*Datos de programación*\n No fue posible encontrar el código de línea";
                return false;
            }
            */
            //validar linea codigo---------
            if (string.IsNullOrEmpty(trn.linea))
            {
                validacion_error = "*Datos de programación*\n No fue posible encontrar el código de línea";
                return false;
            }

            //validar mail------------
            if (string.IsNullOrEmpty(trn.mail))
            {
                validacion_error = "*Datos de programación*\n Por favor escriba el correo eletrónico";
                return false;
            }

            /*
            //validar total_disponible------
            if (!int.TryParse(trn.total.Replace(",","."), out valida))
            {
                validacion_error = "*Datos de programación*\n No fué posible encontrar la disponibilidad de programación";
                return false;
            }
            */

            if (trn.detalles == null)
            {
                validacion_error = "*Datos de programación*\n No fué posible encontrar los horarios de asignados";
                return false;
            }

            foreach (var d in trn.detalles)
            {
                if (string.IsNullOrEmpty(d.idd) || string.IsNullOrEmpty(d.idh) || string.IsNullOrEmpty(d.dispone))
                {
                    validacion_error = "*Datos de programación*\n Hubo un problema de comunicación con el servidor\nPor favor intente salir y volver entrar a la aplicacion.";
                    return false;
                }
                if (!string.IsNullOrEmpty(d.reserva))
                { 
                   //validar q no sobrepase su linea dispone
                    if (!int.TryParse(d.reserva, out valida))
                    {
                        validacion_error =string.Format("El valor de reserva del horario {0}-{1}, tiene un valor NO VALIDO [{2}]",d.desde,d.hasta,d.reserva);
                        return false;
                    }
                    //total de linea
                    valida1 = int.Parse(d.dispone);
                    if (valida > valida1)
                    {
                        validacion_error = string.Format("El Horario {0}-{1}, excede su disponibilidad, favor verifique [{2}]", d.desde, d.hasta,valida);
                        return false;
                    }
                    valot += valida;
                 }
            }
            //valida aqui el total <= cupo
            //valida1 = int.Parse(trn.total);
            if (valot > valida1)
            {
                validacion_error = string.Format("*Reserva*\nLa cantidad de reserva excede el cupo disponible \n Cupo: {0}\n Reserva:{1}",valida1,valot);
                return false;
            }
            //mayor q cero
            if (valot <= 0)
            {
                validacion_error = "* Reserva *\n La cantidad de reservas debe ser mayor que 0";
                return false;
            }
            //aqui todo ok se debe guardar---->
            validacion_error = string.Empty;
            return true;
        }
        public bool add(out string number)
        {
            HashSet<SqlCommand> lista_c = new HashSet<SqlCommand>();
            try
            {
                number = string.Empty;
                using (var xcon = conexion())
                {
                    try
                    {
                        foreach (var det in this.detalles)
                        {
                           var xi =0;
                           if (!string.IsNullOrEmpty(det.reserva) && int.TryParse(det.reserva, out xi) && xi > 0)
                           {
                               var comando = xcon.CreateCommand();
                               comando.CommandType = CommandType.StoredProcedure;
                               comando.CommandText = "dbo.PROCESO_CONSOLIDACION_EXPORTADORES";
                               comando.Parameters.AddWithValue("@TIPO", 5);
                               comando.Parameters.AddWithValue("@ID_HORARIO_DET", det.idd);
                               comando.Parameters.AddWithValue("@RUC", this.linea);
                               comando.Parameters.AddWithValue("@BOOKING", this.booking);
                               comando.Parameters.AddWithValue("@CANT_CNTR", det.reserva);
                               comando.Parameters.AddWithValue("@MAIL", this.mail);
                               comando.Parameters.AddWithValue("@USUARIO_ING", this.usuario);
                               lista_c.Add(comando);
                           }

                        }
                        if (xcon.State != ConnectionState.Open)
                        {
                            xcon.Open();
                        }
                        using (var tx = xcon.BeginTransaction())
                        {
                            try
                            {
                                foreach (var c in lista_c)
                                {
                                    c.Transaction = tx;
                                    c.ExecuteNonQuery();
                                }
                                tx.Commit();
                            }
                            catch (SqlException ex)
                            {
                                tx.Rollback();
                                StringBuilder sb = new StringBuilder();
                                foreach (SqlError e in ex.Errors)
                                {
                                    sb.AppendLine(string.Format("{0}, {1}, {2}, {3}", e.LineNumber, ex.Message, e.Procedure, e.Server));
                                }
                                var t = log_csl.save_log<SqlException>(ex, "turno", "add-Trx", this.idlinea, this.linea);
                                string serror = ex.Message.Substring(0, 7);
                                if (serror == "error{}")
                                {
                                    number = ex.Message.Substring(7).ToString();
                                    return false;
                                }
                                else
                                {
                                    number = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este este código de servicio D00-{0}", t);
                                    return false;
                                }
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
                        var t = log_csl.save_log<SqlException>(ex, "turno", "add", this.idlinea, this.linea);
                        number = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este este código de servicio D00-{0}", t);
                        return false;

                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                var t = log_csl.save_log<Exception>(ex, "turno", "add-gral", this.idlinea, this.linea);
                number = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este este código de servicio D00-{0}", t);
                return false;
            }
        
        }
        public static bool addMail(out string number, string mailpara, string asunto, string htmlmensaje, string copiaspara, string usuario, string idlinea, string linea)
        {
            HashSet<SqlCommand> lista_c = new HashSet<SqlCommand>();
            try
            {
                number = string.Empty;
                using (var xcon = serviceCsl())
                {
                    try
                    {
                        var comando = xcon.CreateCommand();
                        comando.CommandType = CommandType.StoredProcedure;
                        comando.CommandText = "dbo.sp_insert_mail_log";
                        comando.Parameters.AddWithValue("@asunto", asunto);
                        comando.Parameters.AddWithValue("@htmlmsg", htmlmensaje);
                        comando.Parameters.AddWithValue("@mailpara", mailpara);
                        comando.Parameters.AddWithValue("@copiaspara", copiaspara);
                        comando.Parameters.AddWithValue("@usuario", usuario);
                        lista_c.Add(comando);
                        if (xcon.State != ConnectionState.Open)
                        {
                            xcon.Open();
                        }
                        using (var tx = xcon.BeginTransaction())
                        {
                            try
                            {
                                foreach (var c in lista_c)
                                {
                                    c.Transaction = tx;
                                    c.ExecuteNonQuery();
                                }
                                tx.Commit();
                            }
                            catch (SqlException ex)
                            {
                                tx.Rollback();
                                StringBuilder sb = new StringBuilder();
                                foreach (SqlError e in ex.Errors)
                                {
                                    sb.AppendLine(string.Format("{0}, {1}, {2}, {3}", e.LineNumber, e.Message, e.Procedure, e.Server));
                                }
                                var t = log_csl.save_log<SqlException>(ex, "turno", "add-Trx", idlinea , linea);
                                number = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este este código de servicio D00-{0}", t);
                                return false;
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
                        var t = log_csl.save_log<SqlException>(ex, "turno", "add", idlinea, linea);
                        number = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este este código de servicio D00-{0}", t);
                        return false;

                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                var t = log_csl.save_log<Exception>(ex, "turno", "add-gral", idlinea, linea);
                number = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este este código de servicio D00-{0}", t);
                return false;
            }

        }
        private static SqlConnection conexionN5()
        {
            return new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["N5"].ConnectionString);
        }
        private static SqlConnection conexionPortalServicio()
        {
            return new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["catalogo"].ConnectionString);
        }
        private static SqlConnection conexion()
        {
            return new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["midle"].ConnectionString);
        }
        private static SqlConnection conexionN4()
        {
            return new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["n4catalog"].ConnectionString);
        }
        private static SqlConnection serviceCsl()
        {
            return new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["service"].ConnectionString);
        }
        public static DataTable GetHorarios(DateTime fecha, string agencia, string bkg, out string validacion)
        {
            var d = new DataTable();
            using (var c = conexion())
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandText = "CONSULTA_DETALLE_HORARIOS_RH_CONEXP";
                coman.Parameters.AddWithValue("@FECHA_PRG", fecha.Date);
                coman.Parameters.AddWithValue("@BOOKING", bkg);
                coman.Parameters.AddWithValue("@RUC", agencia);
                validacion = string.Empty;
               
                var psql = new SqlParameter();
                psql.Direction = ParameterDirection.Output;
                psql.ParameterName = "@MSG";
                psql.SqlDbType = SqlDbType.NVarChar;
                psql.Size = 500;
       
                coman.Parameters.Add(psql);
                //@MSG
                try
                {
                    c.Open();
                    d.Load(coman.ExecuteReader(CommandBehavior.CloseConnection));
                    validacion = psql.Value.ToString();
                }
                catch (SqlException ex)
                {
                    csl_log.log_csl.save_log<SqlException>(ex, "turno", "GetHorarios", fecha.ToShortDateString(), "gs");
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
            return d;
        }
        public static List<Tuple<Int64, string, Decimal>> GetProxHorario(DateTime fecha, string linea, string booking)
        {
            var ls = new List<Tuple<Int64, string, Decimal>>();
            using (var c = conexion())
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandText = "CONSULTA_DETALLE_HORARIOS_X_DIAS_CONEXP";
                coman.Parameters.AddWithValue("@FECHA_DESDE", fecha.Date);
                coman.Parameters.AddWithValue("@LINEA", linea);
                coman.Parameters.AddWithValue("@BOOKING", booking);
                try
                {
                    c.Open();
                    var r = coman.ExecuteReader(CommandBehavior.CloseConnection);
                    while (r.Read())
                    {
                        ls.Add(Tuple.Create(r.GetInt64(0), r.GetDateTime(1).Date.ToString("dd/MM/yyyy"), r.GetDecimal(2)));
                    }
                    return ls;
                    
                }
                catch (SqlException ex)
                {
                    csl_log.log_csl.save_log<SqlException>(ex, "turno", "GetHorarios", fecha.ToShortDateString(), "gs");
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
            return null;
        }
        public static DataTable GetHorariosProgramados(string booking, string linea_nav, DateTime fecha, out string validacion)
        {
            var d = new DataTable();
            using (var c = conexion())
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandText = "CONSULTA_DETALLE_HORARIOS_CH_CONEXP";
                coman.Parameters.AddWithValue("@FECHA_PRG", fecha.Date);
                coman.Parameters.AddWithValue("@BOOKING", booking);
                coman.Parameters.AddWithValue("@LINEA", linea_nav);
                validacion = string.Empty;
                var psql = new SqlParameter();
                psql.Direction = ParameterDirection.Output;
                psql.ParameterName = "@MSG";
                psql.SqlDbType = SqlDbType.NVarChar;
                psql.Size = 500;

                coman.Parameters.Add(psql);
                try
                {
                    c.Open();
                    d.Load(coman.ExecuteReader(CommandBehavior.CloseConnection));
                    validacion = psql.Value.ToString();
                }
                catch (SqlException ex)
                {
                    csl_log.log_csl.save_log<SqlException>(ex, "turno", "GetHorariosProgramados", fecha.ToShortDateString(), "gs");
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
            return d;
        }
        public static bool Cancelar(string booking, DateTime fecha, string linea, string usuario, Int64 id_horario, out string mensaje)
        {
            try
            { 
               using (var conn = conexion())
                {
                    try
                    {
                        using (var comando = conn.CreateCommand())
                        {
                            comando.CommandType = CommandType.StoredProcedure;
                            comando.CommandText = "PROCESO_CONSOLIDACION_EXPORTADORES";
                            comando.Parameters.AddWithValue("@TIPO", 6);
                            comando.Parameters.AddWithValue("@FECHA_PRG", fecha.Date);
                            comando.Parameters.AddWithValue("@RUC", linea);
                            comando.Parameters.AddWithValue("@USUARIO_MOD", usuario);
                            comando.Parameters.AddWithValue("@ID_HORARIO_DET ", id_horario);
                            comando.Parameters.AddWithValue("@BOOKING", booking);
                            conn.Open();
                            var result = comando.ExecuteNonQuery().ToString();
                            conn.Close();
                            mensaje = result;
                            return true;
                        }
                    }
                    catch (SqlException ex)
                    {
                        StringBuilder sb = new StringBuilder();
                        foreach (SqlError e in ex.Errors)
                        {
                            sb.AppendLine(string.Format("{0}, {1}, {2}, {3}", e.LineNumber, e.Message, e.Procedure, e.Server));
                        }
                        var t = log_csl.save_log<Exception>(ex, "turno", "Cancelar", id_horario.ToString(), "sistema");
                         mensaje = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este este código de servicio D00-{0}", t);
                         return false;
                    }
                    finally
                    {
                        if (conn.State == ConnectionState.Open)
                        {
                            conn.Close();
                        }
                        conn.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                //todo loguear que pasó!
                var t = log_csl.save_log<Exception>(ex, "jAisvContainer", "delete",id_horario.ToString(), "sistema");
                mensaje = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este este código de servicio E00-{0}", t);
                return false;
            }
        }
        public static bool Modificar(string booking, int cantidad, DateTime fecha, string linea, string usuario, Int64 id_horario, out string mensaje)
        {
            try
            {
                using (var conn = conexion())
                {
                    try
                    {
                        using (var comando = conn.CreateCommand())
                        {
                            comando.CommandType = CommandType.StoredProcedure;
                            comando.CommandText = "PROCESO_CONSOLIDACION_EXPORTADORES";
                            comando.Parameters.AddWithValue("@TIPO", 9);
                            comando.Parameters.AddWithValue("@FECHA_PRG", fecha.Date);
                            comando.Parameters.AddWithValue("@RUC", linea);
                            comando.Parameters.AddWithValue("@USUARIO_MOD", usuario);
                            comando.Parameters.AddWithValue("@ID_HORARIO_DET ", id_horario);
                            comando.Parameters.AddWithValue("@CANT_CNTR ", cantidad);
                            comando.Parameters.AddWithValue("@BOOKING", booking);
                            conn.Open();
                            var result = comando.ExecuteNonQuery().ToString();
                            conn.Close();
                            mensaje = result;
                            return true;
                        }
                    }
                    catch (SqlException ex)
                    {
                        StringBuilder sb = new StringBuilder();
                        foreach (SqlError e in ex.Errors)
                        {
                            sb.AppendLine(string.Format("{0}, {1}, {2}, {3}", e.LineNumber, e.Message, e.Procedure, e.Server));
                        }
                        var t = log_csl.save_log<Exception>(ex, "turno", "Cancelar", id_horario.ToString(), "sistema");
                        mensaje = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este este código de servicio D00-{0}", t);
                        return false;
                    }
                    finally
                    {
                        if (conn.State == ConnectionState.Open)
                        {
                            conn.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //todo loguear que pasó!
                var t = log_csl.save_log<Exception>(ex, "jAisvContainer", "delete", id_horario.ToString(), "sistema");
                mensaje = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este este código de servicio E00-{0}", t);
                return false;
            }
        }
        public static DataTable Programacion(DateTime fecha, string linea, string bookin)
        {
            var d = new DataTable();
            using (var c = conexion())
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandText = "RPT_CONSOLIDACIONES_X_RESERVA_CONEXP";
                coman.Parameters.AddWithValue("@FECHA_PRG", fecha.Date);
                coman.Parameters.AddWithValue("@BOOKING", bookin);
                coman.Parameters.AddWithValue("@RUC", linea);
                try
                {
                    c.Open();
                    d.Load(coman.ExecuteReader(CommandBehavior.CloseConnection));
                }
                catch (SqlException ex)
                {
                    csl_log.log_csl.save_log<SqlException>(ex, "turno", "GetHorariosProgramados", fecha.ToShortDateString(), "gs");
                }
                finally
                {
                    if (c.State == ConnectionState.Open)
                    {
                        c.Close();
                    }
                   
                }
            }
            return d;
        }
        public static List<Tuple<Int64, Int64, string, string, string, string>> GetBookings(string bookin, string linea)
        {
            var resultado = new List<Tuple<Int64, Int64, string, string, string, string>>();

            using (var c = conexionN4())
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandText = "n4_sp_get_booking";
                coman.Parameters.AddWithValue("@booking", bookin);
                coman.Parameters.AddWithValue("@fkind", "LCL");
                coman.Parameters.AddWithValue("@linea", linea);
                try
                {
                    c.Open();
                    var rea = coman.ExecuteReader(CommandBehavior.CloseConnection);
                    while (rea.Read())
                    {
                        resultado.Add(Tuple.Create(rea.GetInt64(0), 
                                                   rea.GetInt64(1), 
                                                   rea.GetString(2),
                                                   rea.GetString(3),
                                                   rea.GetString(4), 
                                                   rea.GetDateTime(5).ToString("dd/MM/yyyy HH:mm:ss")
                                                   ));
                    }
                }
                catch (SqlException ex)
                {
                    csl_log.log_csl.save_log<SqlException>(ex, "turno", "GetBookings", bookin, "Sistema_to_SQL");
                    return null;
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
            return resultado;
        }
        public static DataTable GetBookingsList(string bookin, string linea)
        {
            var d = new DataTable();
            using (var c = conexion())
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandText = "n4m_sp_get_booking_conexp";
                coman.Parameters.AddWithValue("@booking", bookin);
                coman.Parameters.AddWithValue("@fkind", "LCL");
                coman.Parameters.AddWithValue("@linea", linea);
                try
                {
                    c.Open();
                    d.Load(coman.ExecuteReader(CommandBehavior.CloseConnection));
                }
                catch (SqlException ex)
                {
                    csl_log.log_csl.save_log<SqlException>(ex, "turno", "GetBookings", bookin, "Sistema_to_SQL");
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
            return d;
        }
        public static string GetMails()
        {
            string mails = string.Empty;
            using (var c = conexion())
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandText = "PROCESO_CONSOLIDACION_EXPORTADORES";
                coman.Parameters.AddWithValue("@TIPO", "7");

                var psql = new SqlParameter();
                psql.Direction = ParameterDirection.Output;
                psql.ParameterName = "@MAILS";
                psql.SqlDbType = SqlDbType.NVarChar;
                psql.Size = 500;

                coman.Parameters.Add(psql);
                try
                {
                    c.Open();
                    coman.ExecuteReader(CommandBehavior.CloseConnection);
                    mails = psql.Value.ToString();
                }
                catch (SqlException ex)
                {
                    csl_log.log_csl.save_log<SqlException>(ex, "turno", "GetMails", DateTime.Now.ToShortDateString(), "gs");
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
            if (string.IsNullOrEmpty(mails))
            {
                mails = "CGSA-Supervisores CFS@cgsa.com.ec;roger.reyes@outlook.com;jrodriguez@cgsa.com.ec";
            }
            return mails;
        }
        public static string GetInfoBkgList()
        {
            string info = string.Empty;
            using (var c = conexion())
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandText = "PROCESO_CONSOLIDACION_EXPORTADORES";
                coman.Parameters.AddWithValue("@TIPO", "12");

                var psql = new SqlParameter();
                psql.Direction = ParameterDirection.Output;
                psql.ParameterName = "@INFOBKGLIST";
                psql.SqlDbType = SqlDbType.NVarChar;
                psql.Size = 50000;

                coman.Parameters.Add(psql);
                try
                {
                    c.Open();
                    coman.ExecuteReader(CommandBehavior.CloseConnection);
                    info = psql.Value.ToString();
                }
                catch (SqlException ex)
                {
                    csl_log.log_csl.save_log<SqlException>(ex, "turno", "GetMails", DateTime.Now.ToShortDateString(), "gs");
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
            if (string.IsNullOrEmpty(info))
            {
                info = "Estimado cliente, si el número de booking que esta buscando no aparece en esta lista. <br />" +
                       "Favor comunicarse con el Area de Planificacion de CGSA <br />" +
                       "Pbx: +593 (04) 6006300, 3901700 ext. 4039, 4040, 4060. <br />" +
                       "Email: <a href='mailto:AfterDock@cgsa.com.ec'>AfterDock@cgsa.com.ec</a>; y <a href='mailto:AuxiliaresPlanning@cgsa.com.ec'>AuxiliaresPlanning@cgsa.com.ec</a>";
            }
            return info;
        }
        public static string GetInfoCalendarList()
        {
            string info = string.Empty;
            using (var c = conexion())
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandText = "PROCESO_CONSOLIDACION_EXPORTADORES";
                coman.Parameters.AddWithValue("@TIPO", "13");

                var psql = new SqlParameter();
                psql.Direction = ParameterDirection.Output;
                psql.ParameterName = "@INFOCALENDARIO";
                psql.SqlDbType = SqlDbType.NVarChar;
                psql.Size = 50000;

                coman.Parameters.Add(psql);
                try
                {
                    c.Open();
                    coman.ExecuteReader(CommandBehavior.CloseConnection);
                    info = psql.Value.ToString();
                }
                catch (SqlException ex)
                {
                    csl_log.log_csl.save_log<SqlException>(ex, "turno", "GetMails", DateTime.Now.ToShortDateString(), "gs");
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
            if (string.IsNullOrEmpty(info))
            {
                info = "Estimado cliente, si la fecha requerida para su reserva no aparece en esta lista. <br />" +
                       "Favor comunicarse con nuestra Àrea de Logistica y Almacenamiento <br />" +
                       "Pbx: +593 (04) 6006300, 3901700 ext. 4002, 4021, 4005. <br />" +
                       "Email: <a href='mailto:CGSA-Consolidaciones@cgsa.com.ec'>CGSA-Consolidaciones@cgsa.com.ec</a>;";
            }
            return info;
        }
        public static string GetInfoCancelacion()
        {
            string info = string.Empty;
            using (var c = conexion())
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandText = "PROCESO_CONSOLIDACION_EXPORTADORES";
                coman.Parameters.AddWithValue("@TIPO", "16");

                var psql = new SqlParameter();
                psql.Direction = ParameterDirection.Output;
                psql.ParameterName = "@INFOCANCELACION";
                psql.SqlDbType = SqlDbType.NVarChar;
                psql.Size = 50000;

                coman.Parameters.Add(psql);
                try
                {
                    c.Open();
                    coman.ExecuteReader(CommandBehavior.CloseConnection);
                    info = psql.Value.ToString();
                }
                catch (SqlException ex)
                {
                    csl_log.log_csl.save_log<SqlException>(ex, "turno", "GetMails", DateTime.Now.ToShortDateString(), "gs");
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
            if (string.IsNullOrEmpty(info))
            {
                info = "Estimado cliente, Toda cancelaciòn de reserva debe procesarse 24 horas antes de la operación. <br />" +
                       "Favor comunicarse con nuestra Àrea de Planificación <br />" +
                       "Pbx: (+593)4 6006300, 3901700 ext. 4002, 4003. <br />" +
                       "Email: <a href='mailto:CGSA-Yard Planners@cgsa.com.ec'>CGSA-Yard Planners@cgsa.com.ec</a>;'";
            }
            return info;
        }
        public static int GetMaxHoraCancelacion()
        {
            int hora_max_cancel = 0;
            using (var c = conexion())
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandText = "PROCESO_CONSOLIDACION_EXPORTADORES";
                coman.Parameters.AddWithValue("@TIPO", "8");

                var psql = new SqlParameter();
                psql.Direction = ParameterDirection.Output;
                psql.ParameterName = "@HORA_MAX_CANCELACION";
                psql.SqlDbType = SqlDbType.Int;
                psql.Size = 500;

                coman.Parameters.Add(psql);
                try
                {
                    c.Open();
                    coman.ExecuteReader(CommandBehavior.CloseConnection);
                    hora_max_cancel = Convert.ToInt32(psql.Value.ToString());
                }
                catch (SqlException ex)
                {
                    csl_log.log_csl.save_log<SqlException>(ex, "turno", "GetMaxHoraCancelacion", DateTime.Now.ToShortDateString(), "gs");
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
            return hora_max_cancel;
        }
        public static int GetHoraActualCancelacion()
        {
            int hora_actual_cancel = 0;
            using (var c = conexion())
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandText = "PROCESO_CONSOLIDACION_EXPORTADORES";
                coman.Parameters.AddWithValue("@TIPO", "17");

                var psql = new SqlParameter();
                psql.Direction = ParameterDirection.Output;
                psql.ParameterName = "@HORA_ACTUAL_CANCELACION";
                psql.SqlDbType = SqlDbType.Int;
                psql.Size = 500;

                coman.Parameters.Add(psql);
                try
                {
                    c.Open();
                    coman.ExecuteReader(CommandBehavior.CloseConnection);
                    hora_actual_cancel = Convert.ToInt32(psql.Value.ToString());
                }
                catch (SqlException ex)
                {
                    csl_log.log_csl.save_log<SqlException>(ex, "turno", "GetHoraActualCancelacion", DateTime.Now.ToShortDateString(), "gs");
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
            return hora_actual_cancel;
        }
        public static DateTime GetFechaActualCancelacion()
        {
            DateTime fecha_actual_cancel = new DateTime();
            using (var c = conexion())
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandText = "PROCESO_CONSOLIDACION_EXPORTADORES";
                coman.Parameters.AddWithValue("@TIPO", "18");

                var psql = new SqlParameter();
                psql.Direction = ParameterDirection.Output;
                psql.ParameterName = "@FECHA_ACTUAL";
                psql.SqlDbType = SqlDbType.Date;
                psql.Size = 500;

                coman.Parameters.Add(psql);
                try
                {
                    c.Open();
                    coman.ExecuteReader(CommandBehavior.CloseConnection);
                    fecha_actual_cancel = Convert.ToDateTime(psql.Value.ToString());
                }
                catch (SqlException ex)
                {
                    csl_log.log_csl.save_log<SqlException>(ex, "turno", "GetFechaActualCancelacion", DateTime.Now.ToShortDateString(), "gs");
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
            return fecha_actual_cancel;
        }
        public static Tuple<Int64, Int32, Int32> GetLimite(DateTime fecha, string linea, string booking)
        {
            using (var c = conexion())
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandText = "PROCESO_CONSOLIDACION_EXPORTADORES";
                coman.Parameters.AddWithValue("@FECHA_PRG", fecha.Date);
                coman.Parameters.AddWithValue("@TIPO", 4);
                coman.Parameters.AddWithValue("@LINEA", linea);
                coman.Parameters.AddWithValue("@BOOKING", booking);
                try
                {
                    c.Open();
                    var r = coman.ExecuteReader(CommandBehavior.CloseConnection);
                    if (r.HasRows)
                    {
                        r.Read();
                        return Tuple.Create(r.GetInt64(0), r.GetInt32(1), r.GetInt32(2));
                    }
                }
                catch (SqlException ex)
                {
                    csl_log.log_csl.save_log<SqlException>(ex, "turno", "GetHorarios", fecha.ToShortDateString(), "gs");
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
            return null;
        }
        public static Int32 GetCantMaxBkg(string linea, string booking)
        {
            Int32 cantmaxcntr = 0;
            using (var c = conexion())
            {
                var coman = c.CreateCommand();

                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandText = "PROCESO_CONSOLIDACION_EXPORTADORES";
                coman.Parameters.AddWithValue("@TIPO", 10);
                coman.Parameters.AddWithValue("@LINEA", linea);
                coman.Parameters.AddWithValue("@BOOKING", booking);

                var psql = new SqlParameter();
                psql.Direction = ParameterDirection.Output;
                psql.ParameterName = "@CANTBKG";
                psql.SqlDbType = SqlDbType.Int;
                //psql.Size = 500;

                coman.Parameters.Add(psql);
                try
                {
                    c.Open();
                    coman.ExecuteReader(CommandBehavior.CloseConnection);
                    cantmaxcntr = Convert.ToInt32(psql.Value);
                }
                catch (SqlException ex)
                {
                    csl_log.log_csl.save_log<SqlException>(ex, "turno", "GetCantMaxBkg", DateTime.Now.ToShortDateString(), "gs");
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
            return cantmaxcntr;
        }
        public static Int32 GetSumCantReserva(string booking)
        {
            Int32 cantmaxcntr = 0;
            using (var c = conexion())
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandText = "PROCESO_CONSOLIDACION_EXPORTADORES";
                coman.Parameters.AddWithValue("@TIPO", 11);
                coman.Parameters.AddWithValue("@BOOKING", booking);

                var psql = new SqlParameter();
                psql.Direction = ParameterDirection.Output;
                psql.ParameterName = "@SUMCANTRESERVA";
                psql.SqlDbType = SqlDbType.Int;
                //psql.Size = 500;

                coman.Parameters.Add(psql);
                try
                {
                    c.Open();
                    coman.ExecuteReader(CommandBehavior.CloseConnection);
                    cantmaxcntr = Convert.ToInt32(psql.Value);
                }
                catch (SqlException ex)
                {
                    csl_log.log_csl.save_log<SqlException>(ex, "turno", "GetSumCantReserva", DateTime.Now.ToShortDateString(), "gs");
                }
                finally
                {
                    if (c.State == ConnectionState.Open)
                    {
                        c.Close();
                    }
                   
                }
            }
            return cantmaxcntr;
        }
        public static string GetExportador(int idusuario)
        {
            string nombre = "";
            using (var c = conexionPortalServicio())
            {
                try
                {
                    if (c.State == ConnectionState.Open)
                    {
                        c.Close();
                    }
                    using (SqlCommand comm = new SqlCommand("dbo.frt_shipname", c))
                    {
                        comm.CommandType = CommandType.StoredProcedure;
                        SqlParameter IdUsuario = new SqlParameter("@IdUsuario", SqlDbType.Int);
                        SqlParameter NombreEmpresa = new SqlParameter("@NombreEmpresa", SqlDbType.NVarChar);

                        IdUsuario.Direction = ParameterDirection.Input;
                        NombreEmpresa.Direction = ParameterDirection.ReturnValue;
                        IdUsuario.Value = idusuario;

                        comm.Parameters.Add(IdUsuario);
                        comm.Parameters.Add(NombreEmpresa);

                        c.Open();
                        comm.ExecuteNonQuery();

                        if (NombreEmpresa.Value != DBNull.Value)
                        {
                            nombre = (string)NombreEmpresa.Value;
                        }
                    }
                }
                catch (SqlException ex)
                {
                    csl_log.log_csl.save_log<SqlException>(ex, "turno", "GetExportador", DateTime.Now.ToShortDateString(), "gs");
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
            return nombre;
        }
        public static string GetNombreEmpresa(string bkg)
        {
            string shipname = "";
            using (var c = conexionN5())
            {
                try
                {
                    if (c.State == ConnectionState.Open)
                    {
                        c.Close();
                    }
                    using (SqlCommand comm = new SqlCommand("dbo.frt_nombre_empresa", c))
                    {
                        comm.CommandType = CommandType.StoredProcedure;
                        SqlParameter booking = new SqlParameter("@bkg", SqlDbType.NVarChar);
                        SqlParameter nameship = new SqlParameter("@shipname", SqlDbType.NVarChar);

                        booking.Direction = ParameterDirection.Input;
                        nameship.Direction = ParameterDirection.ReturnValue;
                        booking.Value = bkg;

                        comm.Parameters.Add(booking);
                        comm.Parameters.Add(nameship);

                        c.Open();
                        comm.ExecuteNonQuery();

                        if (nameship.Value != DBNull.Value)
                        {
                            shipname = (string)nameship.Value;
                        }
                    }
                }
                catch (SqlException ex)
                {
                    csl_log.log_csl.save_log<SqlException>(ex, "turno", "GetNombreEmpresa", DateTime.Now.ToShortDateString(), "gs");
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
            return shipname;
        }
        public static DataTable GetRptReservas(DateTime fechaini, DateTime fechafin, string booking, string linea)
        {
            var d = new DataTable();
            using (var c = conexion())
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandText = "RPT_RESERVA_CLIENTE_CONEXP";
                coman.Parameters.AddWithValue("@FECHA_DESDE", fechaini.Date);
                coman.Parameters.AddWithValue("@FECHA_HASTA", fechafin.Date);
                coman.Parameters.AddWithValue("@RUC", linea);
                if (string.IsNullOrEmpty(booking))
                {
                    booking = null;
                }
                coman.Parameters.AddWithValue("@BOOKING", booking);
                try
                {
                    c.Open();
                    d.Load(coman.ExecuteReader(CommandBehavior.CloseConnection));
                }
                catch (SqlException ex)
                {
                    csl_log.log_csl.save_log<SqlException>(ex, "turno", "GetRptReservas", fechaini.ToShortDateString() + "|" + fechaini.ToShortDateString()+ "|" +booking, "gs");
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
            return d;
        }
        public static DataTable GetRptReservasFull(string fechaini, string fechafin, string booking, string linea, bool valida, bool detalle)
        {
            var d = new DataTable();
            using (var c = conexion())
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandText = "RPT_RESERVA_CLIENTE_CONEXP_CNTR";
                coman.Parameters.AddWithValue("@RUC", linea);
                coman.Parameters.AddWithValue("@DETALLE", detalle);
                if (valida)
                {
                    coman.Parameters.AddWithValue("@VALIDA", valida);
                }
                if (string.IsNullOrEmpty(booking))
                {
                    booking = null;
                }
                if (string.IsNullOrEmpty(fechaini))
                {
                    fechaini = null;
                    coman.Parameters.AddWithValue("@FECHA_DESDE", fechaini);
                }
                else
                {
                    CultureInfo enUS = new CultureInfo("en-US");
                    DateTime dfechaini;
                    if (!DateTime.TryParseExact(fechaini, "dd/MM/yyyy", enUS, DateTimeStyles.None, out dfechaini))
                    {
                    }
                    coman.Parameters.AddWithValue("@FECHA_DESDE", dfechaini);
                }
                if (string.IsNullOrEmpty(fechafin))
                {
                    fechafin = null;
                    coman.Parameters.AddWithValue("@FECHA_HASTA", fechafin);
                }
                else
                {
                    CultureInfo enUS = new CultureInfo("en-US");
                    DateTime dfechafin;
                    if (!DateTime.TryParseExact(fechafin, "dd/MM/yyyy", enUS, DateTimeStyles.None, out dfechafin))
                    {
                    }
                    coman.Parameters.AddWithValue("@FECHA_HASTA", dfechafin);
                }
                coman.Parameters.AddWithValue("@BOOKING", booking);
                
                try
                {
                    c.Open();
                    d.Load(coman.ExecuteReader(CommandBehavior.CloseConnection));
                }
                catch (SqlException ex)
                {
                    csl_log.log_csl.save_log<SqlException>(ex, "turno", "GetRptReservasFull", fechaini + "|" + fechafin + "|" + booking, "gs");
                }
                finally
                {
                    if (c.State == ConnectionState.Open)
                    {
                        c.Close();
                    }
                   
                }
            }
            return d;
        }
    }
    public class tdetalleConsolidacion
    {
        public string num { get; set; }
        public string idh { get; set; }
        public string idd { get; set; }
        public string desde { get; set; }
        public string hasta { get; set; }
        public string total { get; set; }
        public string reserva { get; set; }
        public string dispone { get; set; }
    }
}