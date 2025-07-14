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
    public class repNavieras
    {
        public string booking { get; set; }
        public string fecha_pro { get; set; }
        public string mail { get; set; }
        public string idlinea { get; set; }
        public string linea { get; set; }
        public string total { get; set; }
        public string usuario { get; set; }
        public List<tdetalle> detalles { get; set; }
        public static bool validar(repNavieras trn, out string validacion_error)
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
                                var t = log_csl.save_log<SqlException>(ex, "repNavieras", "add-Trx", this.idlinea, this.linea);
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
                        var t = log_csl.save_log<SqlException>(ex, "repNavieras", "add", this.idlinea, this.linea);
                        number = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este este código de servicio D00-{0}", t);
                        return false;

                    }
                    finally
                    {
                        xcon.Dispose();
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                var t = log_csl.save_log<Exception>(ex, "repNavieras", "add-gral", this.idlinea, this.linea);
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
                                var t = log_csl.save_log<SqlException>(ex, "repNavieras", "add-Trx", idlinea, linea);
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
                        var t = log_csl.save_log<SqlException>(ex, "repNavieras", "add", idlinea, linea);
                        number = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este este código de servicio D00-{0}", t);
                        return false;

                    }
                    finally
                    {
                        xcon.Dispose();
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                var t = log_csl.save_log<Exception>(ex, "repNavieras", "add-gral", idlinea, linea);
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
        private static SqlConnection conexionPortal()
        {
            return new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["validar"].ConnectionString);
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
                        var t = log_csl.save_log<Exception>(ex, "repNavieras", "Cancelar", id_horario.ToString(), "sistema");
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
                        var t = log_csl.save_log<Exception>(ex, "repNavieras", "Cancelar", id_horario.ToString(), "sistema");
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
                var t = log_csl.save_log<Exception>(ex, "jAisvContainer", "delete", id_horario.ToString(), "sistema");
                mensaje = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este este código de servicio E00-{0}", t);
                return false;
            }
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
                    csl_log.log_csl.save_log<SqlException>(ex, "turnoConsolidacion", "GetNombreEmpresa", DateTime.Now.ToShortDateString(), "gs");
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


        public static DataTable GetRptGateIn(string linea, string fechaini, string fechafin, string booking, string contain, bool valida)
        {
            var d = new DataTable();
            using (var c = conexionN5())
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandText = "INF_N5_Rep_GateIn";
                coman.Parameters.AddWithValue("@CodNaviera", linea);

                if (string.IsNullOrEmpty(booking))
                {
                    booking = null;
                }
                if (string.IsNullOrEmpty(fechaini))
                {
                    fechaini = DBNull.Value.ToString();
                    //coman.Parameters.AddWithValue("@FechaIni", fechaini);
                }
                else
                {
                    CultureInfo enUS = new CultureInfo("en-US");
                    DateTime dfechaini;
                    if (!DateTime.TryParseExact(fechaini, "dd/MM/yyyy", enUS, DateTimeStyles.None, out dfechaini))
                    {
                    }
                    coman.Parameters.AddWithValue("@FechaIni", dfechaini);
                }
                if (string.IsNullOrEmpty(fechafin))
                {
                    fechafin = DBNull.Value.ToString();
                    //coman.Parameters.AddWithValue("@FechaFin", fechafin);
                }
                else
                {
                    CultureInfo enUS = new CultureInfo("en-US");
                    DateTime dfechafin;
                    if (!DateTime.TryParseExact(fechafin, "dd/MM/yyyy", enUS, DateTimeStyles.None, out dfechafin))
                    {
                    }
                    coman.Parameters.AddWithValue("@FechaFin", dfechafin);
                }
                if (string.IsNullOrEmpty(contain))
                {
                    contain = null;
                }
                coman.Parameters.AddWithValue("@VesselRef", booking);
                coman.Parameters.AddWithValue("@Contenedor", contain);
                
                try
                {
                    c.Open();
                    d.Load(coman.ExecuteReader(CommandBehavior.CloseConnection));
                }
                catch (SqlException ex)
                {
                    csl_log.log_csl.save_log<SqlException>(ex, "ingresoCont", "GetRptGateIn", fechaini + "|" + fechafin + "|" + booking, "gs");
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

        public static DataTable GetRptGateIn_New(string linea, string fechaini, string fechafin, string booking, string contain, bool valida)
        {
            var d = new DataTable();
            using (var c = conexionN5())
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandText = "INF_N5_Rep_GateIn_New";
                coman.Parameters.AddWithValue("@CodNaviera", linea);

                if (string.IsNullOrEmpty(booking))
                {
                    booking = null;
                }
                if (string.IsNullOrEmpty(fechaini))
                {
                    fechaini = DBNull.Value.ToString();
                    //coman.Parameters.AddWithValue("@FechaIni", fechaini);
                }
                else
                {
                    CultureInfo enUS = new CultureInfo("en-US");
                    DateTime dfechaini;
                    if (!DateTime.TryParseExact(fechaini, "dd/MM/yyyy", enUS, DateTimeStyles.None, out dfechaini))
                    {
                    }
                    coman.Parameters.AddWithValue("@FechaIni", dfechaini);
                }
                if (string.IsNullOrEmpty(fechafin))
                {
                    fechafin = DBNull.Value.ToString();
                    //coman.Parameters.AddWithValue("@FechaFin", fechafin);
                }
                else
                {
                    CultureInfo enUS = new CultureInfo("en-US");
                    DateTime dfechafin;
                    if (!DateTime.TryParseExact(fechafin, "dd/MM/yyyy", enUS, DateTimeStyles.None, out dfechafin))
                    {
                    }
                    coman.Parameters.AddWithValue("@FechaFin", dfechafin);
                }
                if (string.IsNullOrEmpty(contain))
                {
                    contain = null;
                }
                coman.Parameters.AddWithValue("@VesselRef", booking);
                coman.Parameters.AddWithValue("@Contenedor", contain);
                
                try
                {                
                    c.Open();
                    d.Load(coman.ExecuteReader(CommandBehavior.CloseConnection));
                }
                catch (SqlException ex)
                {
                    csl_log.log_csl.save_log<SqlException>(ex, "ingresoCont", "GetRptGateIn", fechaini + "|" + fechafin + "|" + booking, "gs");
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



        public static DataTable GetRptGateOut(string linea, string fechaini, string fechafin, string booking, string contain)
        {
        var d = new DataTable();
        using (var c = conexionN5())
        {
            var coman = c.CreateCommand();
            coman.CommandType = CommandType.StoredProcedure;
            coman.CommandText = "INF_N5_Rep_GateOut";
            coman.Parameters.AddWithValue("@CodNaviera", linea);

            if (string.IsNullOrEmpty(booking))
            {
                booking = null;
            }
            if (string.IsNullOrEmpty(fechaini))
            {
                fechaini = null;
                //coman.Parameters.AddWithValue("@FechaIni", fechaini);
            }
            else
            {
                CultureInfo enUS = new CultureInfo("en-US");
                DateTime dfechaini;
                if (!DateTime.TryParseExact(fechaini, "dd/MM/yyyy", enUS, DateTimeStyles.None, out dfechaini))
                {
                }
                coman.Parameters.AddWithValue("@FechaIni", dfechaini);
            }
            if (string.IsNullOrEmpty(fechafin))
            {
                fechafin = null;
                //coman.Parameters.AddWithValue("@FechaFin", fechafin);
            }
            else
            {
                CultureInfo enUS = new CultureInfo("en-US");
                DateTime dfechafin;
                if (!DateTime.TryParseExact(fechafin, "dd/MM/yyyy", enUS, DateTimeStyles.None, out dfechafin))
                {
                }
                coman.Parameters.AddWithValue("@FechaFin", dfechafin);
            }

            coman.Parameters.AddWithValue("@VesselRef", booking);
            if (string.IsNullOrEmpty(contain))
            {
                contain = null;
            }
            coman.Parameters.AddWithValue("@Contenedor", contain);

            try
            {
                c.Open();
                coman.CommandTimeout = 3000;
                d.Load(coman.ExecuteReader(CommandBehavior.CloseConnection));
            }
            catch (SqlException ex)
            {
                csl_log.log_csl.save_log<SqlException>(ex, "salidaCont", "GetRptGateOut", fechaini + "|" + fechafin + "|" + booking, "gs");
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
        public static DataTable GetRptRfCont(string linea, string fechaini, string fechafin, string booking, string contain)
        {
            var d = new DataTable();
            using (var c = conexionPortal())
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandText = "INF_N5_Rep_RefrigeratedContainers";
                coman.Parameters.AddWithValue("@CodNaviera", linea);
                //coman.Parameters.AddWithValue("@DETALLE", detalle);
                //if (valida)
                //{
                //    coman.Parameters.AddWithValue("@VALIDA", valida);
                //}
                if (string.IsNullOrEmpty(booking))
                {
                    booking = null;
                }
                if (string.IsNullOrEmpty(fechaini))
                {
                    fechaini = null;
                    //coman.Parameters.AddWithValue("@FechaIni", fechaini);
                }
                else
                {
                    CultureInfo enUS = new CultureInfo("en-US");
                    DateTime dfechaini;
                    if (!DateTime.TryParseExact(fechaini, "dd/MM/yyyy", enUS, DateTimeStyles.None, out dfechaini))
                    {
                    }
                    coman.Parameters.AddWithValue("@FechaIni", dfechaini);
                }
                if (string.IsNullOrEmpty(fechafin))
                {
                    fechafin = null;
                    //coman.Parameters.AddWithValue("@FechaFin", fechafin);
                }
                else
                {
                    CultureInfo enUS = new CultureInfo("en-US");
                    DateTime dfechafin;
                    if (!DateTime.TryParseExact(fechafin, "dd/MM/yyyy", enUS, DateTimeStyles.None, out dfechafin))
                    {
                    }
                    coman.Parameters.AddWithValue("@FechaFin", dfechafin);
                }

                coman.Parameters.AddWithValue("@VesselRef", booking);

                if (string.IsNullOrEmpty(contain))
                {
                    contain = null;
                }
                coman.Parameters.AddWithValue("@Contenedor", contain);

                try
                {
                    c.Open();
                    d.Load(coman.ExecuteReader(CommandBehavior.CloseConnection));
                }
                catch (SqlException ex)
                {
                    csl_log.log_csl.save_log<SqlException>(ex, "reefer_cont", "GetRptRfCont", fechaini + "|" + fechafin + "|" + booking, "gs");
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
        public static DataTable GetRptFullCntLoad(string linea, string booking)
        {
            var d = new DataTable();
            using (var c = conexionN5())
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandText = "INF_N5_Rep_FullContainersLoad";
                coman.Parameters.AddWithValue("@CodNaviera", linea);
                if (string.IsNullOrEmpty(booking))
                {
                    booking = null;
                }

                coman.Parameters.AddWithValue("@VesselRef", booking);

                try
                {
                    c.Open();
                    d.Load(coman.ExecuteReader(CommandBehavior.CloseConnection));
                }
                catch (SqlException ex)
                {
                    csl_log.log_csl.save_log<SqlException>(ex, "turnoConsolidacion", "GetRptFullCntLoad", booking, "gs");
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

        public static DataTable GetRptFullCntLoad_New(string linea, string booking)
        {
            var d = new DataTable();
            using (var c = conexionN5())
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandText = "INF_N5_Rep_FullContainersLoad_2022";
                coman.Parameters.AddWithValue("@CodNaviera", linea);
                if (string.IsNullOrEmpty(booking))
                {
                    booking = null;
                }

                coman.Parameters.AddWithValue("@VesselRef", booking);

                try
                {
                    c.Open();
                    d.Load(coman.ExecuteReader(CommandBehavior.CloseConnection));
                }
                catch (SqlException ex)
                {
                    csl_log.log_csl.save_log<SqlException>(ex, "turnoConsolidacion", "GetRptFullCntLoad", booking, "gs");
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

        public static DataTable GetRptEmptyCntLoad(string linea, string booking, string tipo)
        {
            var d = new DataTable();
            using (var c = conexionN5())
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandText = "INF_N5_Rep_EmptyContainersLoad";
                coman.Parameters.AddWithValue("@CodNaviera", linea);

                if (string.IsNullOrEmpty(booking))
                {
                    booking = null;
                }

                coman.Parameters.AddWithValue("@VesselRef", booking);
                coman.Parameters.AddWithValue("@FreightKind", tipo);

                try
                {
                    c.Open();
                    d.Load(coman.ExecuteReader(CommandBehavior.CloseConnection));
                }
                catch (SqlException ex)
                {
                    csl_log.log_csl.save_log<SqlException>(ex, "turnoConsolidacion", "GetRptEmptyCntLoad", booking, "gs");
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
        public static DataTable GetRptEmptyCntDesc(string linea, string booking, string tipo)
        {
            var d = new DataTable();
            using (var c = conexionN5())
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandText = "INF_N5_Rep_EmptyContainersDischarge";
                coman.Parameters.AddWithValue("@CodNaviera", linea);

                if (string.IsNullOrEmpty(booking))
                {
                    booking = null;
                }

                coman.Parameters.AddWithValue("@VesselRef", booking);
                coman.Parameters.AddWithValue("@FreightKind", tipo);

                try
                {
                    c.Open();
                    d.Load(coman.ExecuteReader(CommandBehavior.CloseConnection));
                }
                catch (SqlException ex)
                {
                    csl_log.log_csl.save_log<SqlException>(ex, "turnoConsolidacion", "GetRptEmptyCntDesc", booking, "gs");
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
        public static DataTable GetRptFullCntDesc(string linea, string booking, string tipo)
        {
            var d = new DataTable();
            using (var c = conexionN5())
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandText = "INF_N5_Rep_FullContainersDischarge";
                coman.Parameters.AddWithValue("@CodNaviera", linea);

                if (string.IsNullOrEmpty(booking))
                {
                    booking = null;
                }

                coman.Parameters.AddWithValue("@VesselRef", booking);
                coman.Parameters.AddWithValue("@FreightKind", tipo);

                try
                {
                    c.Open();
                    d.Load(coman.ExecuteReader(CommandBehavior.CloseConnection));
                }
                catch (SqlException ex)
                {
                    csl_log.log_csl.save_log<SqlException>(ex, "turnoConsolidacion", "GetRptEmptyCntDesc", booking, "gs");
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
        public static DataTable GetCasDate(string cont, string bl)
        {
            var d = new DataTable();
            using (var c = conexionN5())
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandText = "CSL_N5_P_Cons_Cas";
            
                if (string.IsNullOrEmpty(cont))
                {
                    cont = null;
                }
                coman.Parameters.AddWithValue("@CONTAINER", cont);
                if (string.IsNullOrEmpty(bl))
                {
                    bl = null;// "";// DBNull.Value.ToString();
                }
                coman.Parameters.AddWithValue("@BL", bl);

                try
                {
                    c.Open();
                    d.Load(coman.ExecuteReader(CommandBehavior.CloseConnection));
                }
                catch (SqlException ex)
                {
                    csl_log.log_csl.save_log<SqlException>(ex, "ConsultaCas", "GetCasDate", cont, "gs");
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
        public static DataTable GetTurnDate(string cont, string fec)
        {
            var d = new DataTable();
            using (var c = conexionPortal())
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandText = "CSL_P_Cons_Turnos_Disp_New";

                if (string.IsNullOrEmpty(cont))
                {
                    cont = "";//null;
                }
                coman.Parameters.AddWithValue("@CONTAINER", cont);

                if (string.IsNullOrEmpty(fec))
                {
                    fec = null;// DBNull.Value.ToString();
                    coman.Parameters.AddWithValue("@FECHA", fec);
                }
                else
                {
                    CultureInfo enUS = new CultureInfo("en-US");
                    DateTime dfecha;
                    if (!DateTime.TryParseExact(fec, "dd/MM/yyyy", enUS, DateTimeStyles.None, out dfecha))
                    {
                    }
                    coman.Parameters.AddWithValue("@FECHA", dfecha);
                }
            

                try
                {
                    c.Open();
                    d.Load(coman.ExecuteReader(CommandBehavior.CloseConnection));
                }
                catch (SqlException ex)
                {
                    csl_log.log_csl.save_log<SqlException>(ex, "ConsultaTurn", "GetTurnDate", cont, "gs");
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
        public static DataTable GetReqService(string cont)
        {
            var d = new DataTable();
            using (var c = conexionPortal())
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandText = "REQSERV_Requerimientos_Cons_New";

                if (string.IsNullOrEmpty(cont))
                {
                    cont = "";//null;
                }
                coman.Parameters.AddWithValue("@CONTENEDOR", cont);

                try
                {
                    c.Open();
                    d.Load(coman.ExecuteReader(CommandBehavior.CloseConnection));
                }
                catch (SqlException ex)
                {
                    csl_log.log_csl.save_log<SqlException>(ex, "ConsultaReqServ", "GetReqService", cont, "gs");
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
        public static DataTable GetReqServDet(string cont, string req)
        {
            var d = new DataTable();
            using (var c = conexionPortal())
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandText = "REQSERV_Requerimiento_ConsInf_New";

                if (string.IsNullOrEmpty(cont))
                {
                    cont = "";//null;
                }
                coman.Parameters.AddWithValue("@cntr_consecutivo", cont);
                coman.Parameters.AddWithValue("@rqse_request", req);

                try
                {
                    c.Open();
                    d.Load(coman.ExecuteReader(CommandBehavior.CloseConnection));
                }
                catch (SqlException ex)
                {
                    csl_log.log_csl.save_log<SqlException>(ex, "ConsultaReqServ", "GetReqService", cont, "gs");
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
    }
    public class tdetalleNavieras
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