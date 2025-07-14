using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using csl_log;
using System.Data;
using System.Text;
using Newtonsoft;
using System.Text.RegularExpressions;
using System.Globalization;
using ConectorN4;
using CSLSite.app_start;
using CSLSite.N4Object;
using CSLSite.XmlTool;

namespace CSLSite
{
    public class jSolicitud
    {
        #region "Cabecera"
        public jSolicitud()
        {
            this.lines = new List<jSolicitudDetalle>();
        }

        public string id { get; set; } //servicio del combo--> @serv_codigo
        public string service { get; set; } //servicio del combo--> @serv_codigo
        public string imo { get; set; } // codigo de la nave--> @nave_codigo
        public string vIn { get; set; } //viaje in-->@nave_codigoViajeIn
        public string vOut { get; set; } //viaje out-->@nave_codigoViajeOut
        public string imrn { get; set; }//mrn-->@cae_manifiestoImpo
        public string emrn { get; set; }//mrn-->@cae_manifiestoExpo
        public string anio { get; set; }//anio-->@apg_anio
        public string regis { get; set; } //registro-->@apg_registro
        public string eta { get; set; } //ETA-->@veo_eta
        public string etb { get; set; }//ETB-->@veo_etb
        public string uso { get; set; }//USO-->@veo_horasUsoMuelle
        public string ets { get; set; }//>ETS-->@veo_ets
        public string lport { get; set; }//last port-->@pto_codeUltimo
        public string nport { get; set; }//next port-->@pto_codeProximo
        public string pebruto { get; set; }//peso bruto-->@nave_pesoBruto
        public string sign { get; set; } //call-->@nave_callSign
        public string pnum { get; set; } // pbip num-->@cert_numero
        public string phasta { get; set; }//pbip hasta-->@cert_validez
        public string pprov { get; set; }//provisional->@cert_esProvisional Y-N
        public string pseg { get; set; }//secur level--> @cert_nivel 1,2,3

        public string autor { get; set; }//secur level--> @cert_nivel 1,2,3

        public string mail1 { get; set; }//secur level--> mail
        public string mail2 { get; set; }//secur level--> @cert_nivel 1,2,3
        public string mail3 { get; set; }//secur level--> @cert_nivel 1,2,3
        public string mail4 { get; set; }//secur level--> @cert_nivel 1,2,3
        public string mail5 { get; set; }//secur level--> @cert_nivel 1,2,3
        //toneladas netas
        public string peneto { get; set; } // codigo de la nave
        public string userid { get; set; }//secur level--> @id
        //NOMBRE DEL BUQUE
        public string uline { get; set; } //nombre de la nave 


        //NOMBRE DEL BUQUE
        public string nombre { get; set; } //nombre de la nave 
        //FLAG DEL BUQUE
        public string flag { get; set; } //flag de la nave
        //eslora de la nave
        public string eslora { get; set; } //eslora de la nave
        //tipo_b
        public string tipo { get; set; } //calado maximo

        public string qgkey { get; set; } //gkey del buque
        public string qline { get; set; } //linea del buque

        public string agencia { get; set; } //linea del buque

        public string nservicio { get; set; } //linea del buque
        public string seguro { get; set; } //seguro del catalogo

        //NUEVOS CAMPOS 28/03/2024
        public string operacion { get; set; }
        public string fecembarque { get; set; }
        public string fecbanano { get; set; }

        /*
         @usr_id int,
	@usr_alias varchar(20),
         */


        #endregion
        #region "Lineas asociadas"
        public List<jSolicitudDetalle> lines { get; set; }
        #endregion

        #region "metodos propios"
        public static bool ValidateSolicitudData(jSolicitud solicitud, out string validacionError)
        {
            //la cultura del server
            CultureInfo enUS = new CultureInfo("en-US");
            //el stylo de numero normal
            DateTime fecha;
            decimal numero = 0;
            bool booleano = false;


            //linea/ruc de usuario
            if (string.IsNullOrEmpty(solicitud.uline) )
            {
                validacionError = "Usuario:\nLínea o agencia naviera desconocida";
                return false;
            }
            //login de usuario
            if (string.IsNullOrEmpty(solicitud.autor))
            {
                validacionError = "Usuario:\nDatos de usuario no fueron encontrados";
                return false;
            }

            //servicio
            if (string.IsNullOrEmpty(solicitud.service) || !decimal.TryParse(solicitud.service, out  numero))
            {
                validacionError = "Solicitud:\nCódigo de servicio incorrecto (2)";
                return false;
            }
            //Validar líneas si existen
            if (solicitud.lines == null || solicitud.lines.Count <= 0)
            {
                validacionError = "Solicitud:\nNo se encontró líneas asociadas al servicio (Sección 2)";
                return false;
            }
            //validar lineas que no sean vacias
            foreach (var l in solicitud.lines)
            {
                if (string.IsNullOrEmpty(l.line) || string.IsNullOrEmpty(l.viajeIn) || string.IsNullOrEmpty(l.viajeOut))
                {
                    validacionError = "Solicitud:\nDatos los datos de viaje de las líneas asociadas incompleto (Sección 2)";
                    return false;
                }
            }
            //valida IMO
            if (string.IsNullOrEmpty(solicitud.imo))
            {
                validacionError = "Solicitud:\nDatos de la nave [IMO], no fueron encontrados (3)";
                return false;
            }

            //nave viaje iN
            if (string.IsNullOrEmpty(solicitud.vIn))
            {
                validacionError = "Solicitud:\nDatos de viaje de la nave incompletos (11) ";
                return false;
            }

            //nave viaje Out
            if (string.IsNullOrEmpty(solicitud.vOut))
            {
                validacionError = "Solicitud:\nDatos de viaje de la nave incompletos (12) ";
                return false;
            }


            if (ViajeExists(solicitud.imo, solicitud.vIn, solicitud.vOut))
            {
                validacionError = "Solicitud:\nLa combinación Nave + Viaje entrante + Viaje saliente, ya se encuentra registrada en otra solicitud del año actual.\nSi necesita mas detalles comuníquese con nuestro departamento de planificación";
                return false;
            }


            //nave viaje Out
            if (string.IsNullOrEmpty(solicitud.pnum))
            {
                validacionError = "Solicitud:\nDatos de PBIP incompletos (13)";
                return false;
            }

            //Valido Hasta
            if (!DateTime.TryParseExact(solicitud.phasta.Replace("-", "/"), "dd/MM/yyyy", enUS, DateTimeStyles.None, out fecha))
            {
                validacionError = "Solicitud:\nDatos de PBIP incorrectos fecha no válida (14)";
                return false;
            }
            if (fecha < DateTime.Now)
            {
                validacionError = "Solicitud:\nDatos de PBIP incorrectos documento expirado (14)";
                return false;
            }
            solicitud.phasta = fecha.ToString("yyyy/MM/dd");


            //provisional
            if (string.IsNullOrEmpty(solicitud.pprov) || !Boolean.TryParse(solicitud.pprov,out booleano))
            {
                solicitud.pprov = "true";
            }

            //seguridad
            if (string.IsNullOrEmpty(solicitud.pseg) || !decimal.TryParse(solicitud.pseg, out  numero))
            {
                validacionError = "Solicitud:\nNivel de seguridad incorrecto (16)";
                return false;
            }

            if (string.IsNullOrEmpty(solicitud.seguro) || solicitud.seguro.Equals("0000000000000"))
            {
                validacionError = "Solicitud:\nSeleccione Compañía de Seguros (17)";
                return false;
            }

            //puertos
            if (string.IsNullOrEmpty(solicitud.lport))
            {
                validacionError = "Solicitud:\nDatos de puertos incompletos (18)";
                return false;
            }

            if (string.IsNullOrEmpty(solicitud.nport))
            {
                validacionError = "Solicitud:\nDatos de puertos incompletos (19)";
                return false;
            }

            //MRN--> primero vacío
            if (string.IsNullOrEmpty(solicitud.imrn))
            {
                validacionError = "Solicitud:\nDatos de manifiesto de importación incompletos (20)";
                return false;
            }
            if (IsMrnExist(solicitud.imrn))
            {
                validacionError = string.Format("Solicitud:\nDatos de manifiesto de importación {0} ya se encuentran en otra referencia comuníquese con planificación (20)",solicitud.imrn); ;
                return false;
            
            }
            //expo mrn
            if (string.IsNullOrEmpty(solicitud.emrn))
            {
                validacionError = "Solicitud:\nDatos de manifiesto de exportación incompletos (21)";
                return false;
            }

            //año
            if (string.IsNullOrEmpty(solicitud.anio) || !decimal.TryParse(solicitud.anio, out  numero))
            {
                validacionError = "Solicitud:\nInformación de APG incompleta | incorrecta (22)";
                return false;
            }

            //registro
            if (string.IsNullOrEmpty(solicitud.regis))
            {
                validacionError = "Solicitud:\nInformación de APG incompleta (23)";
                return false;
            }


            DateTime eta;
            DateTime etb;
            int horas = 0;
            //Eta*
            if (!DateTime.TryParseExact(solicitud.eta.Replace("-", "/"), "dd/MM/yyyy HH:mm", enUS, DateTimeStyles.None, out eta))
            {
                validacionError = "*Solicitud*\nLa fecha de ETA no tiene el formato correcto (24)";
                return false;
            }
            solicitud.eta = eta.ToString("yyyy/MM/dd HH:mm");
           
            if (!DateTime.TryParseExact(solicitud.etb.Replace("-", "/"), "dd/MM/yyyy HH:mm", enUS, DateTimeStyles.None, out etb))
            {
                validacionError = "*Solicitud*\nLa fecha de ETB no tiene el formato correcto (25)";
                return false;
            }
            solicitud.etb = etb.ToString("yyyy/MM/dd HH:mm");
           
            //año
            if (string.IsNullOrEmpty(solicitud.uso) || !int.TryParse(solicitud.uso, out  horas) || horas <=0)
            {
                validacionError = "Solicitud:\nUso de muelle incompleto | incorrecto (26)";
                return false;
            }

            /*if (eta >= etb)
            {
                validacionError = "Solicitud:\nFecha ETB debe ser mayor a ETA (24)";
                return false;
            }*/
            // ETB vs ETA
            if (etb >= eta)
            {
                validacionError = "Solicitud:\nFecha ETB debe ser mayor a ETA (24)";
                return false;
            }

            //calculo de fecha ETS.
            DateTime ets = eta.AddHours(horas);
            //ETS vs ETB
            if (ets <= eta)
            {
                validacionError = "Solicitud:\nFecha ETS debe ser mayor que ETB (27)";
                return false;
            
            }
            //ETS vs ETA
            if (ets <= etb)
            {
                validacionError = "Solicitud:\nFecha ETS debe ser mayor que ETB (27)";
                return false;
            }

            /*if (ets <= eta)
            {
                validacionError = "Solicitud:\nFecha ETS debe ser mayor que ETA (27)";
                return false;
            }*/


            solicitud.ets = ets.ToString("yyyy/MM/dd HH:mm");


            //NUEVA VALIDACIONES 01-04-2024
            if (!string.IsNullOrEmpty(solicitud.operacion))
            {
                if (solicitud.operacion.Equals("TRUE"))
                {
                    //valida cion de fechas
                    DateTime fecha_embarque;
                    DateTime fecha_banano;

                    //Fecha de embarque de contenedores planificado
                    if (string.IsNullOrEmpty(solicitud.fecembarque))
                    {
                        validacionError = "*Solicitud*\nDebe ingresar Fecha de embarque de contenedores planificado (29)";
                        return false;
                    }

                    //Fecha de Cutfoff  (BBK)
                    if (string.IsNullOrEmpty(solicitud.fecbanano))
                    {
                        validacionError = "*Solicitud*\nDebe ingresar  Cutfoff (BBK) (30)";
                        return false;
                    }

                    if (!DateTime.TryParseExact(solicitud.fecembarque.Replace("-", "/"), "dd/MM/yyyy HH:mm", enUS, DateTimeStyles.None, out fecha_embarque))
                    {
                        validacionError = "*Solicitud*\nLa fecha de Fecha de embarque de contenedores planificado, no tiene el formato correcto (29)";
                        return false;
                    }

                    solicitud.fecembarque = fecha_embarque.ToString("yyyy/MM/dd HH:mm");

                    if (!DateTime.TryParseExact(solicitud.fecbanano.Replace("-", "/"), "dd/MM/yyyy HH:mm", enUS, DateTimeStyles.None, out fecha_banano))
                    {
                        validacionError = "*Solicitud*\nLa fecha Cutfoff (BBK) no tiene el formato correcto (30)";
                        return false;
                    }
                    solicitud.fecbanano = fecha_banano.ToString("yyyy/MM/dd HH:mm");

                    if (fecha_banano > ets)
                    {
                        validacionError = "*Solicitud*\nLa fecha Cutfoff (BBK) (30) no puede ser mayor que ETD Fecha estimada de zarpe(27)";
                        return false;
                    }

                    if (fecha_embarque > ets)
                    {
                        validacionError = "*Solicitud*\nLa fecha de Fecha de embarque de contenedores planificado (29) no puede ser mayor que ETD Fecha estimada de zarpe(27)";
                        return false;
                    }
                }



            }

            validacionError = string.Empty;
            return true;
        }
        public bool add(out string number)
        {
            try
            {
                string cadena = string.Empty;
                if (System.Configuration.ConfigurationManager.ConnectionStrings["validar"] == null)
                {
                    number = string.Format("El servidor de datos no esta disponible por el momento");
                    return false;
                }

                cadena = System.Configuration.ConfigurationManager.ConnectionStrings["validar"].ConnectionString;
                //validar
                //CREAR EL PREAVISO, O BL DEPENDIENDO DEL TIPO DE AISV.
                using (var conexion = new SqlConnection(cadena))
                {
                    try
                    {
                        using (var comando = conexion.CreateCommand())
                        {
                            comando.CommandType = CommandType.StoredProcedure;
                            comando.CommandText = "ATRQ_Solicitud_Ingresar_Nueva";
                            //parametro de salida.
                            var sqc = new SqlParameter();
                            sqc.Direction = ParameterDirection.Output;
                            sqc.ParameterName = "sol_codigo";
                            sqc.SqlDbType = SqlDbType.VarChar;
                            sqc.Size = 10;
                            comando.Parameters.Add(sqc);

                            float conversion = 0;
                           


                            comando.Parameters.AddWithValue("@lnav_codigo", this.uline);
                            comando.Parameters.AddWithValue("@serv_codigo", this.service);
                            comando.Parameters.AddWithValue("@nave_codigo", this.imo);
                            comando.Parameters.AddWithValue("@nave_caladoArribo", 0);

                            if(!float.TryParse(this.pebruto.Replace(",", "."),out conversion))
                            {
                                conversion = 0;
                            }
                            comando.Parameters.AddWithValue("@nave_pesoBruto", conversion);


                            comando.Parameters.AddWithValue("@nave_callSign", this.sign);
                            comando.Parameters.AddWithValue("@nave_tipo", "C");
                            comando.Parameters.AddWithValue("@nave_codigoViajeIn", this.vIn);
                            comando.Parameters.AddWithValue("@nave_codigoViajeOut", this.vOut);
                            comando.Parameters.AddWithValue("@cert_numero ", this.pnum);
                            comando.Parameters.AddWithValue("@cert_validez", this.phasta);
                            comando.Parameters.AddWithValue("@cert_esProvisional", this.pprov.Contains("T")?"Y":"N");
                            comando.Parameters.AddWithValue("@cert_nivel", this.pseg);
                            comando.Parameters.AddWithValue("@pto_codeUltimo", this.lport);
                            comando.Parameters.AddWithValue("@pto_codeProximo", this.nport);
                            comando.Parameters.AddWithValue("@cae_manifiestoImpo", this.imrn);
                            comando.Parameters.AddWithValue("@cae_manifiestoExpo", this.emrn);
                            comando.Parameters.AddWithValue("@apg_anio", this.anio);
                            comando.Parameters.AddWithValue("@apg_registro", this.regis);
                            comando.Parameters.AddWithValue("@usr_id", this.userid);
                            comando.Parameters.AddWithValue("@usr_alias", this.autor);
                            comando.Parameters.AddWithValue("@veo_eta", this.eta);
                            comando.Parameters.AddWithValue("@veo_etb", this.etb);
                            comando.Parameters.AddWithValue("@veo_ets", this.ets);
                            comando.Parameters.AddWithValue("@veo_horasUsoMuelle", this.uso);
                            comando.Parameters.AddWithValue("@nave_nombre", this.nombre);
                            comando.Parameters.AddWithValue("@bandera", this.flag);
                            comando.Parameters.AddWithValue("@eslora", this.eslora);
                            comando.Parameters.AddWithValue("@mail1", this.mail1);
                            comando.Parameters.AddWithValue("@mail2", this.mail2);
                            comando.Parameters.AddWithValue("@mail3", this.mail3);
                            comando.Parameters.AddWithValue("@mail4", this.mail4);
                            comando.Parameters.AddWithValue("@mail5", this.mail5);

                            conversion = 0;
                            if (!float.TryParse(this.peneto.Replace(",", "."), out conversion))
                            {
                                conversion = 0;
                            }
                            comando.Parameters.AddWithValue("@ton_neto", conversion);

                            conversion = 0;
                            if (!float.TryParse(this.pebruto.Replace(",", "."), out conversion))
                            {
                                conversion = 0;
                            }
                            comando.Parameters.AddWithValue("@ton_bruto", conversion);

                            comando.Parameters.AddWithValue("@agencia", this.agencia);
                            comando.Parameters.AddWithValue("@tipo_bu", this.tipo);
                            comando.Parameters.AddWithValue("@nave_line", this.qline);
                            comando.Parameters.AddWithValue("@bkey", this.qgkey);
                            comando.Parameters.AddWithValue("@servicio_nombre", this.nservicio);

                            //nuevo cambio 2018 LINEA DE SEGUROS
                            comando.Parameters.AddWithValue("@seguro", this.seguro);

                            //nuevo cambio 01-04-2024
                            
                            comando.Parameters.AddWithValue("@tipooperacion", this.operacion);
                            comando.Parameters.AddWithValue("@embarqueplanificado", this.fecembarque);
                            comando.Parameters.AddWithValue("@cutoffbbk", this.fecbanano);
                            
                            conexion.Open();
                            using (var tran = conexion.BeginTransaction())
                            {
                                try
                                {
                                    comando.Transaction = tran;
                                    comando.ExecuteNonQuery();
                                    foreach (var l in this.lines)
                                    {
                                        var comH = conexion.CreateCommand();
                                        comH.CommandText = "ATRQ_LineaAsociada_Ingresar";
                                        comH.CommandType = CommandType.StoredProcedure;
                                        comH.Parameters.AddWithValue("@sol_codigo", sqc.Value);
                                        comH.Parameters.AddWithValue("@la_linea", l.line);
                                        comH.Parameters.AddWithValue("@la_voyageIn", l.viajeIn);
                                        comH.Parameters.AddWithValue("@la_voyageOut", l.viajeOut);
                                        comH.Parameters.AddWithValue("@usr_alias", this.autor);
                                        comH.Transaction = tran;
                                        comH.ExecuteNonQuery();
                                    }

                                    tran.Commit();
                                }
                                catch (SqlException ex)
                                {
                                    tran.Rollback();
                                    StringBuilder sb = new StringBuilder();
                                    foreach (SqlError e in ex.Errors)
                                    {
                                        sb.AppendLine(string.Format("{0}, {1}, {2}, {3}", e.LineNumber, e.Message, e.Procedure, e.Server));
                                    }
                                    var t = log_csl.save_log<Exception>(ex, "jSolicitud", "add", this.imo, this.autor);
                                    number = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este este código de servicio D00-{0}", t);
                                    return false;
                                }
                            }
                            conexion.Close();
                            this.id = sqc.Value.ToString();
                            number = this.id;
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
                        var t = log_csl.save_log<Exception>(ex, "jSolicitud", "add", this.imo, this.autor);
                        number = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este este código de servicio D00-{0}", t);
                        return false;
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
                //todo loguear que pasó!
                var t = log_csl.save_log<Exception>(ex, "jSolicitud", "add", this.imo, this.autor);
                number = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este este código de servicio E00-{0}", t);
                return false;
            }
        }
        #endregion
        #region "metodos accesorios"
        //obtiene las líneas
        public static HashSet<jCatalogoObjeto> getPoolLines(string idservicio)
        {
            try
            {
                var salida = new HashSet<jCatalogoObjeto>();
                using (var conexion = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["service"].ConnectionString))
                {
                    var xx = conexion.ConnectionTimeout;
                    try
                    {
                        using (var comando = conexion.CreateCommand())
                        {
                            comando.CommandType = CommandType.StoredProcedure;
                            comando.CommandText = "sp_get_aisv_detalles";
                            comando.Parameters.AddWithValue("@servicio", idservicio);
                            conexion.Open();
                            using (var result = comando.ExecuteReader())
                            {
                                var c = 1;
                                while (result.Read())
                                {
                                    var tx = new jCatalogoObjeto();
                                    tx.id = c.ToString();
                                    tx.codigo = result[0] as string;
                                    tx.descripcion = cleanJsonString(result[1].ToString());
                                    tx.notas = cleanJsonString(result[2].ToString());
                                    salida.Add(tx);
                                    c++;
                                }
                            }
                            return salida;
                        }
                    }
                    catch (SqlException ex)
                    {
                        StringBuilder sb = new StringBuilder();
                        foreach (SqlError e in ex.Errors)
                        {
                            sb.AppendLine(string.Format("{0}, {1}, {2}, {3}", e.LineNumber, e.Message, e.Procedure, e.Server));
                        }
                        log_csl.save_log<Exception>(ex, "jSolicitud", "Sql_getPoolLines",idservicio, "N4");
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
            catch (Exception ex)
            {
                log_csl.save_log<Exception>(ex, "jSolicitud", "getPoolLines", idservicio, "N4");
                return null;
            }
        }
        //limpia el Json
        public static string cleanJsonString(string entrada)
        {
            try
            {
                Regex replace_vars = new Regex(@"[&|*|?|""""|'|#|%|<|>|¡|{|}|~|\|!|¿|:]", RegexOptions.Compiled);
                return  replace_vars.Replace(entrada, string.Empty);
            }
            catch (Exception ex)
            {
                return ex.Message;

            }
        }
        //nuevo validacionde la placa
        public static string servicioNombre(Int64 service)
        {
            using (var con = new System.Data.SqlClient.SqlConnection())
            {
                //n4catalog
                con.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["N5"].ConnectionString;
                var comando = con.CreateCommand();
                comando.CommandType = System.Data.CommandType.Text;
                comando.Connection = con;
                comando.CommandText = "select [N5].[dbo].[fx_service_name](@service)";
                comando.Parameters.AddWithValue("@service", service);
                try
                {
                    con.Open();
                    object sale = comando.ExecuteScalar();
                    con.Close();
                    if (sale != null && sale.GetType() != typeof(DBNull))
                    {
                        return sale.ToString();
                    }
                    else
                    {
                        return null;
                    }
                }
                catch (Exception ex)
                {
                    log_csl.save_log<Exception>(ex, "jSolicitud", "servicioNombre", service.ToString(), "N4");
                    return null;
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
        public static bool IsMrnExist(string mrn)
        {
            using (var con = new System.Data.SqlClient.SqlConnection())
            {
                //n4catalog
                con.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["N5"].ConnectionString;
                var comando = con.CreateCommand();
                comando.CommandType = System.Data.CommandType.Text;
                comando.Connection = con;
                comando.CommandText = "select [N5].[dbo].[fx_mrn_existe](@mrn)";
                comando.Parameters.AddWithValue("@mrn", mrn);
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
                    log_csl.save_log<Exception>(ex, "jSolicitud", "IsMrnExist", mrn, "N4");
                    return false;
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
        public static bool ViajeExists(string imo, string inviaje, string outviaje)
        {
            using (var con = new System.Data.SqlClient.SqlConnection())
            {
                //n4catalog
                con.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["service"].ConnectionString;
                var comando = con.CreateCommand();
                comando.CommandType = System.Data.CommandType.Text;
                comando.Connection = con;
                comando.CommandText = "select csl_services.dbo.fx_viaje_existe(@imo, @inviaje, @outviaje)";
                comando.Parameters.AddWithValue("@imo", imo);
                comando.Parameters.AddWithValue("@inviaje", inviaje);
                comando.Parameters.AddWithValue("@outviaje", outviaje);
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
                    log_csl.save_log<Exception>(ex, "jSolicitud", "ViajeExists", imo, "N4");
                    return false;
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
        #endregion
        #region "transporte"
        public bool TransaportToN4(ObjectSesion user, out string mensaje)
        {
            //HASTA 3 CARACTERES EN LA LINEA DE  LA NAVE
            var vs = new VesselVisit();
            vs.id = this.id; //-> SECUENCIA GENERADA DEL INSERT
            vs.visitphase = "CREATED"; //ok
            vs.vesselid = this.imo; //ok
            long srn=0;
            if(!Int64.TryParse(this.service,out srn))
            {
                mensaje = "Hubo un problema durante la conexión y fue imposible encontrar el servicio solicitado, comuníquese con CGSA";
                return false;
            }
            var snn = servicioNombre(srn);
            if(string.IsNullOrEmpty(snn))
            {
                mensaje = "Hubo un problema durante la conexión y fue imposible encontrar el nombre del servicio solicitado, comuníquese con CGSA";
                return false;
            
            }
            vs.serviceid = snn; //ok
            DateTime fechaz;
            fechaz = DateTime.Parse(this.eta);

            vs.eta = fechaz.ToString("yyyy-MM-ddTHH:mm");

            //vs.timeoffportarrive = fechaz.ToString("yyyy-MM-ddTHH:mm");

            //ETA-12 HORAS
            vs.timecargocutoff = fechaz.AddHours(-12).ToString("yyyy-MM-ddTHH:mm");
            vs.timehazcutoff = vs.timecargocutoff;
            vs.timereefercutoff = vs.timecargocutoff;

            fechaz = DateTime.Parse(this.ets);
            vs.etd = fechaz.ToString("yyyy-MM-ddTHH:mm");

         
            fechaz = DateTime.Parse(this.etb);
            vs.timeoffportarrive = fechaz.ToString("yyyy-MM-ddTHH:mm");
            //vs.eta = fechaz.ToString("yyyy-MM-ddTHH:mm");

            //nuevos cambios 01-04-2024
            if (!string.IsNullOrEmpty(this.operacion))
            {
                if (this.operacion.Equals("TRUE"))
                {
                    fechaz = DateTime.Parse(this.fecbanano);
                    vs.timehazcutoff = fechaz.ToString("yyyy-MM-ddTHH:mm");

                    DateTime _fecembarque;
                    _fecembarque = DateTime.Parse(this.fecembarque);
                    vs.timecargocutoff = _fecembarque.AddHours(-8).ToString("yyyy-MM-ddTHH:mm");
                    vs.timereefercutoff = vs.timecargocutoff;


                    vs.vvFlexDate02 = _fecembarque.ToString("yyyy-MM-ddTHH:mm");
                }
                else {
                    vs.vvFlexDate02 = null;
                }
                
            }
            else {
                vs.vvFlexDate02 = null;
            }

                vs.operatorid = this.uline; //LINEA DEL CLIENTE ACTUAL, AGENCIAS.
            vs.iscommoncarrier = "N";
            vs.isdrayoff = "N";
            vs.isnoclientaccess = "N";
            vs.outcallnumber = "1";
            vs.outvoynbr = this.vOut;
            vs.incallnumber = "1";
            vs.invoynbr = this.vIn;
            vs.classification = "DEEPSEA";
            vs.facility = "GYE";
            vs.incustomvoynbr = this.imrn.Trim();
            vs.outcustomvoynbr = this.emrn.Trim();
            vs.vvflexstring1 = this.regis;
            vs.notes = string.Format("Creada desde CSL por usuario {0}",this.autor);
            
            foreach (var l in this.lines)
            {
                var l4 = new line();
                l4.id = l.line;
                l4.invoynbr = l.viajeIn;
                l4.outvoynbr = l.viajeOut;
                vs.lines.Add(l4);
            }
            mensaje = string.Empty;
            long errorNumber = 0;
            var webService = new n4WebService();
            //Validación 6 -> Serialización de la unidad
            var f = xmlHelper.SerializeAsString<VesselVisit>(vs, out errorNumber);
            if (string.IsNullOrEmpty(f))
            {
                mensaje = string.Format("*Lo sentimos, algo salió mal.*\nEstamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio X00-{0}", errorNumber);
                return false;
            }

            //Validación del pre-aviso->3 chao
            var n4estado = webService.InvokeN4Service(user, f, ref mensaje, this.id);
            //verificar la unidad
            //si es advertencia
            if (n4estado == 2)
            {
                mensaje = string.Format("La solicitud {0} no ha sido creada por la siguiente advertencia:[{1}]\n, ", this.id, mensaje);
                return false;
            }
            //si es 3->Chao
            if (n4estado > 2)
            {
                return false;
            }

            mensaje = string.Empty;
            return true;
        }
        #endregion
    }
    public class jSolicitudDetalle
    {
        public string id { get; set; }
        public string line { get; set; }
        public string viajeIn { get; set; }
        public string viajeOut { get; set; }
        public string lnombre { get; set; }
        
    }
    public class jCatalogoObjeto
    {
        public string id { get; set; }
        public string codigo { get; set; }
        public string descripcion { get; set; }
        public string notas { get; set; }
    }

 
}