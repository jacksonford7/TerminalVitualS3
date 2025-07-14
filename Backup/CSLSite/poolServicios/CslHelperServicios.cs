using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CSLSite.services;
using System.Data;
using csl_log;


namespace CSLSite
{
    public class CslHelperServicios
    {
        //Return meses
        public static HashSet<Tuple<string, string>> getMonth()
        {
            var xlista = new HashSet<Tuple<string, string>>();
            xlista.Add(Tuple.Create("01", "Ene"));
            xlista.Add(Tuple.Create("02", "Feb"));
            xlista.Add(Tuple.Create("03", "Mar"));
            xlista.Add(Tuple.Create("04", "Abr"));
            xlista.Add(Tuple.Create("05", "May"));
            xlista.Add(Tuple.Create("06", "Jun"));
            xlista.Add(Tuple.Create("07", "Jul"));
            xlista.Add(Tuple.Create("08", "Ago"));
            xlista.Add(Tuple.Create("09", "Sep"));
            xlista.Add(Tuple.Create("10", "Oct"));
            xlista.Add(Tuple.Create("11", "Nov"));
            xlista.Add(Tuple.Create("12", "Dic"));
            return xlista;
        }
        //Return días
        public static HashSet<Tuple<string, string>> getDays()
        {
            var xlista = new HashSet<Tuple<string, string>>();
            for (int i = 1; i <= 31; i++)
            {
                xlista.Add(Tuple.Create(i.ToString(), i.ToString("00")));
            }
            return xlista;
        }
        //Return años
        public static HashSet<Tuple<string, string>> getYears()
        {
            var xlista = new HashSet<Tuple<string, string>>();
            xlista.Add(Tuple.Create((DateTime.Now.Year - 1).ToString(), (DateTime.Now.Year - 1).ToString()));
            xlista.Add(Tuple.Create((DateTime.Now.Year).ToString(), (DateTime.Now.Year).ToString()));
            xlista.Add(Tuple.Create((DateTime.Now.Year + 1).ToString(), (DateTime.Now.Year + 1).ToString()));
            return xlista;
        }
        //Return hours
        public static HashSet<Tuple<string, string>> getHours()
        {
            var xlista = new HashSet<Tuple<string, string>>();
            for (int i = 0; i <= 23; i++)
            {
                xlista.Add(Tuple.Create(i.ToString(), i.ToString("00")));
            }
            return xlista;
        }
        //Return minutos
        public static HashSet<Tuple<string, string>> getMinutes()
        {
            var xlista = new HashSet<Tuple<string, string>>();
            for (int i = 1; i <= 60; i++)
            {
                xlista.Add(Tuple.Create(i.ToString(), i.ToString("00")));
            }
            return xlista;
        }
        //Return provincias
        public static HashSet<Tuple<string, string>> getProvincias()
        {
            var xlista = new HashSet<Tuple<string, string>>();
            foreach (var i in services.dataServiceHelper.ReturnProvincias())
            {
                xlista.Add(Tuple.Create(i.Item1, i.Item2));
            }
            xlista.Add(Tuple.Create("00", "* Elija la Provincia *"));
            return xlista;
        }
        //Return cantones
        public static HashSet<Tuple<string, string>> getCantones(string provincia)
        {
            var xlista = new HashSet<Tuple<string, string>>();
            foreach (var i in services.dataServiceHelper.ReturnCantones(provincia))
            {
                xlista.Add(Tuple.Create(i.Item1, i.Item2));
            }
            return xlista;
        }
        //return instituciones
        public static HashSet<Tuple<string, string>> getInstitucion()
        {
            var xlista = new HashSet<Tuple<string, string>>();
            foreach (var i in services.dataServiceHelper.ReturnInstitucion())
            {
                xlista.Add(Tuple.Create(i.Item1, i.Item2));
            }
            xlista.Add(Tuple.Create("0", "*Seleccione*"));
            return xlista;
        }
        //return reglas
        public static HashSet<Tuple<string, string>> getReglas(string institucion)
        {
            var xlista = new HashSet<Tuple<string, string>>();
            foreach (var i in services.dataServiceHelper.ReturnReglas(institucion))
            {
                xlista.Add(Tuple.Create(i.Item1, i.Item2));
            }
            xlista.Add(Tuple.Create("0", "*Regla*"));
            return xlista;
        }
        //return bancos
        public static HashSet<Tuple<string, string>> getBancos()
        {
            var xlista = new HashSet<Tuple<string, string>>();
            foreach (var i in services.dataServiceHelper.ReturnBancos())
            {
                xlista.Add(Tuple.Create(i.Item1, i.Item2));
            }
            xlista.Add(Tuple.Create("0", "* Escoja entidad *"));
            return xlista;
        }
        //return refrigerado
        public static HashSet<Tuple<string, string>> getRefrigeracion()
        {
            var xlista = new HashSet<Tuple<string, string>>();
            foreach (var i in services.dataServiceHelper.ReturnRefrigeracion())
            {
                xlista.Add(Tuple.Create(i.Item1, i.Item2));
            }
            xlista.Add(Tuple.Create("0", " * No refrigerado *"));
            return xlista;
        }
        //return ventilación
        public static HashSet<Tuple<string, string>> getVentilacion()
        {
            var xlista = new HashSet<Tuple<string, string>>();
            for (var i = 1; i <= 100; i++)
            {
                xlista.Add(Tuple.Create(i.ToString(), string.Format("{0}%", i)));
            }
            xlista.Add(Tuple.Create("0", " * Sin ventilación (0%) *"));
            return xlista;
        }
        //retorna humedad
        public static HashSet<Tuple<string, string>> getHumedad()
        {
            var xlista = new HashSet<Tuple<string, string>>();
            for (var i = 1; i <= 100; i++)
            {
                xlista.Add(Tuple.Create(i.ToString(), string.Format("{0}", i)));
            }
            xlista.Add(Tuple.Create("0", " * Sin humedad (0) *"));
            return xlista;
        }
        //return iMOS
        public static HashSet<Tuple<string, string>> getImos()
        {
            var xlista = new HashSet<Tuple<string, string>>();
            foreach (var i in services.dataServiceHelper.ReturnImos())
            {
                xlista.Add(Tuple.Create(i.Item1, System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(i.Item2.ToLower())));
            }
            return xlista;
        }
        //return EMBALAJES
        public static HashSet<Tuple<string, string>> getEmbalajes()
        {
            var xlista = new HashSet<Tuple<string, string>>();
            foreach (var i in services.dataServiceHelper.ReturnEmbalajes())
            {
                xlista.Add(Tuple.Create(i.Item1, System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(i.Item2.ToLower())));
            }
            xlista.Add(Tuple.Create("0", "* Escoja tipo de embalaje *"));
            return xlista;
        }
        //return Depositos
        public static HashSet<Tuple<string, string>> getDepositos()
        {
            var xlista = new HashSet<Tuple<string, string>>();
            foreach (var i in services.dataServiceHelper.ReturnDepositos())
            {
                xlista.Add(Tuple.Create(i.Item1, i.Item2));
            }
            xlista.Add(Tuple.Create("0", "* Escoja del depósito *"));
            return xlista;
        }
        //Mensaje personalizado en string
        public static string JsonNewResponse(bool result, bool fluir, string data, string mensaje)
        {
            return string.Format("{{\"mensaje\": \"{0}\",  \"resultado\": {1},\"data\": \"{2}\",\"fluir\": \"{3}\"  }}", mensaje.Trim(), result.ToString().ToLower(), data.Trim(), fluir.ToString().ToLower());
        }
        //valida que la unidad y booking no se crucen
        public static bool Unitproceed(string unit, string booking, out string message)
        {
            var t = dataServiceHelper.ExistUnit(unit, booking);
            if (t == null)
            {
                message = "No se pudo comprobar la unidad hubo un problema de permisos";
                return false;
            }
            if (t.Value)
            {
                message = string.Format("El contenedor {0}, ya está registrado en el booking {1}. \nSi aún desea usar el contenedor primero proceda a anular el AISV", unit, booking);
                return false;
            }
            message = string.Empty;
            return !t.Value;
        }
        //esto es para cerrar form.
        public static string ExitForm(string mensaje)
        {
            return string.Format(@"<script type='text/javascript'>  alert('{0}');   window.returnValue = true; window.close();</script>", mensaje);
        }
        //obtener el shiper name
        public static string getShiperName(string shiperID)
        {
            return dataServiceHelper.getShipName(shiperID);
        }
        //obtener la lista de hzr
        public static HashSet<Tuple<string, string>> GetHzList(int hazid)
        {
            return dataServiceHelper.GetHazards(hazid);
        }


        //JCA, 02/12/2015
        //Devolver los datos de detalle catalogo según nombre del catalogo  
        public static HashSet<Tuple<string, string>> getDetalleCatalogo(string nombreCatalogo)
        {
            var xlista = new HashSet<Tuple<string, string>>();
            foreach (var i in services.dataServiceHelper.returnDetalleCatalogo(nombreCatalogo))
            {
                xlista.Add(Tuple.Create(i.Item1, i.Item2));
            }
            xlista.Add(Tuple.Create("0", "* Seleccione *"));
            return xlista;
        }

        //----------------------------------------------------- JCA -----------------------------------------------------//

        //Función que consulta el sp para regresar todos los servicios disponibles para la solicitud
        public static HashSet<Tuple<string, string>> getServicios()
        {
            var xlista = new HashSet<Tuple<string, string>>();
            foreach (var i in services.dataServiceHelper.returnServicios())
            {
                xlista.Add(Tuple.Create(i.Item1, i.Item2));
            }
            xlista.Add(Tuple.Create("0", "* Seleccione servicios *"));
            return xlista;
        }

        //va y obtiene los contenedores según los parametros que se envien
        public static List<contenedor> contenedores(string contenedor, string tipo, string trafico, string msr = null, string msn = null, string hsn = null, string booking = null, string ruc = null)
        {
            var rs = new List<contenedor>();
            foreach (var item in CLSDataCentroSolicitud.ValorLecturaConexion("CS_ConsultaContenedores", tComando.Procedure, tConexion.Sca, new Dictionary<string, string>() { { "tipoContenedorC", tipo }, { "tipo", trafico }, { "unidad", contenedor }, { "mrnC", msr }, { "msnC", msn }, { "hsnC", hsn }, { "bookingC", booking }, { "rucC", ruc } }))
            {
                var c = new contenedor();
                c.idConsecutivo = item[0].ToString();
                c.nombrecont = item[1] as string;
                c.pesoContenedor = item[3].ToString();
                c.booking = item[4] as string;
                c.tipoContenedor = item[5] as string;
                rs.Add(c);
            }
            return rs;
        }

        //va y obtiene los contenedores según los parametros que se envien
        //public static List<contenedor> contenedores(string contenedor, string tipo, string trafico)
        //{
        //    var rs = new List<contenedor>();
        //    foreach (var item in CLSDataCentroSolicitud.ValorLectura("CGNDB02.N5.dbo.FNA_FUN_CONTAINERS_RSTW", tComando.Procedure, new Dictionary<string, string>() { { "tipoContenedor", tipo }, { "TYPE", trafico }, { "unit", contenedor } }))
        //    {
        //        var c = new contenedor();
        //        c.idConsecutivo = item[0].ToString();
        //        c.nombrecont = item[1] as string;
        //        c.pesoContenedor = item[3].ToString();
        //        c.booking = item[4] as string;
        //        c.tipoContenedor = item[5] as string;                 
        //        rs.Add(c);
        //    }
        //    return rs;
        //}

        //va y obtiene los contenedores según el parametro que se envie
        //public static List<contenedor> contenedoresConsulta(string contenedor, string usuario = null)
        //{
        //    var rs = new List<contenedor>();
        //    foreach (var item in CLSDataCentroSolicitud.ValorLectura("CGNDB02.N5.dbo.FNA_FUN_CONTAINERS_DATOS", tComando.Procedure, new Dictionary<string, string>() { { "unit", contenedor } }))
        //    {
        //        var c = new contenedor();
        //        c.idConsecutivo = item[0].ToString();
        //        c.nombrecont = item[1] as string;
        //        c.pesoContenedor = item[3].ToString();
        //        c.booking = item[4] as string;
        //        c.tipoContenedor = item[5] as string;                 
        //        rs.Add(c);
        //    }
        //    return rs;
        //}

        //va y obtiene los contenedores según el parametro que se envie
        public static List<contenedor> contenedoresConsulta(string contenedor, string usuario = null, string idContenedor = null, string consultarContenedor = "0")
        {
            var rs = new List<contenedor>();
            foreach (var item in CLSDataCentroSolicitud.ValorLecturaConexion("CS_ConsultaContenedoresDatos", tComando.Procedure, tConexion.Sca, new Dictionary<string, string>() { { "unitC", contenedor }, { "idUnitC", idContenedor }, { "consultarContenedor", consultarContenedor } }))
            {
                var c = new contenedor();
                c.idConsecutivo = item[0].ToString();
                c.nombrecont = item[1] as string;
                c.pesoContenedor = item[5].ToString();
                c.booking = item[3] as string;
                c.tipoContenedor = item[4] as string;
                c.sello1 = item[6] as string;
                c.sello2 = item[7] as string;
                c.sello3 = item[8] as string;
                c.sello4 = item[9] as string;
                rs.Add(c);
            }
            return rs;
        }

        //Guarda las cabeceras de solicitudes, con tipo de servicios REESTIBA
        public static List<datosCabecera> cabeceraSolicitudReestiba(int idSolicitud, string TipoServicio, string TipoTrafico, string nombreUsuario, string estado, string tipoUsuario, string txtNumDocAduana, string fechaPropuesta, string tipoProductoEmbalaje, string comentario, string nombreDocumento)
        {
            //int flagError = 0;
            var rs = new List<datosCabecera>();
            foreach (var item in CLSDataCentroSolicitud.ValorLecturaConexion("CS_IngresoServiciosCabecera_Reestiba", tComando.Procedure, tConexion.Sca, new Dictionary<string, string>() { { "IdSolicitud", idSolicitud.ToString() }, { "tipoServicio", TipoServicio }, { "tipoTrafico", TipoTrafico }, { "nombreUsuario", nombreUsuario }, { "idTipoUsuario", tipoUsuario.ToString() }, { "Estado", estado }, { "numDocAduana", txtNumDocAduana }, { "fechaPropuesta", fechaPropuesta.Trim() }, { "tipoProducto", tipoProductoEmbalaje }, { "comentarios", comentario }, { "nombreDocumento", nombreDocumento } }))
            {
                var c = new datosCabecera();
                c.idSolicitud = item[0].ToString();
                c.codigoSolicitud = item[1] as string;
                rs.Add(c);
            }
            return rs;
            //return flagError = Int32.Parse(CLSDataCentroSolicitud.ValorEscalar("CS_IngresoServiciosCabecera_Reestiba", new Dictionary<string, string>() { { "IdSolicitud", idSolicitud.ToString() }, { "tipoServicio", TipoServicio }, { "tipoTrafico", TipoTrafico }, { "nombreUsuario", nombreUsuario }, { "idTipoUsuario", tipoUsuario.ToString() }, { "Estado", estado }, { "numDocAduana", txtNumDocAduana } }, tComando.Procedure, tConexion.Sca));
        }

        //Guarda los detalles de las solicitudes, del tipo de servicios REESTIBA
        public static int detalleSolicitudReestiba(int idSolicitud, int contenedor1, int contenedor2, string nombreUsuario)
        {
            int flagError = 0;
            //var rs = new List<reestiba>();
            foreach (var item in CLSDataCentroSolicitud.ValorEscalar("CS_IngresoServiciosDetalle_Reestiba", new Dictionary<string, string>() { { "IdSolicitud", idSolicitud.ToString() }, { "contenedor1", contenedor1.ToString() }, { "contenedor2", contenedor2.ToString() }, { "nombreUsuario", nombreUsuario } }, tComando.Procedure, tConexion.Sca))
            {
                flagError = Int32.Parse(item.ToString());
            }
            return flagError;
        }

        //Consulta todas las solicitudes que el usuario haya realizado. 
        public static List<consultaCabeceraUsuario> consultaSolicitudUsuario(string numSolicitud, int idContenedor, string TipoServicio, string fechaInicio, string fechaFin, string Estado, Boolean checkTodos, string userName, string idSolicitud)
        {
            var rs = new List<consultaCabeceraUsuario>();
            foreach (var item in CLSDataCentroSolicitud.ValorLecturaConexion("CS_ConsultaSolicitudes_Cabecera_Usuarios", tComando.Procedure, tConexion.Sca, new Dictionary<string, string>() { { "numSolicitud", numSolicitud }, { "idContenedor", idContenedor.ToString() }, { "TipoServicio", TipoServicio }, { "fechaInicio", fechaInicio }, { "fechaFin", fechaFin }, { "Estado", Estado }, { "checkTodos", checkTodos.ToString() }, { "userName", userName }, { "idSolicitud", idSolicitud } }))
            {
                var c = new consultaCabeceraUsuario();
                c.idSolicitud = item[0].ToString();
                c.trafico = item[1] as string;
                c.servicio = item[2] as string;
                c.noBooking = item[3] as string;
                c.noCarga = item[4] as string;
                c.fechaSolicitud = item[5] as string;
                c.estado = item[6] as string;
                c.nombreUsuario = item[7] as string;
                c.correoUsuario = item[9] as string;
                c.codigoSolicitud = item[11] as string;
                c.fechaPropuesta = item[12] as string;
                c.comentarios = item[13] as string;
                c.tipoEmbalaje = item[14] as string;
                c.observacion = item[15] as string;
                c.referencia = item[16] as string;
                c.codigoServicio = item[10].ToString();
                rs.Add(c);
            }
            return rs;
        }

        //Consulta el detalle de una solicitud seleccionada por un usuario.        
        public static List<consultaDetalleUsuario> consultaSolicitudDetalleUsuario(int numSolicitud, string userName, string idDetalleSolicitud = "0", int idContainer = 0, string inbound = "0")
        {
            var rs = new List<consultaDetalleUsuario>();
            foreach (var item in CLSDataCentroSolicitud.ValorLecturaConexion("CS_ConsultaSolicitudes_Detalle_Usuarios", tComando.Procedure, tConexion.Sca, new Dictionary<string, string>() { { "numSolicitud", numSolicitud.ToString() }, { "userName", userName }, { "idDetalleSolicitud", idDetalleSolicitud }, { "idContainer", idContainer.ToString() }, { "transitStateInboundC", inbound.Trim() } }))
            {
                var c = new consultaDetalleUsuario();
                c.idDetalleSolicitud = item[0].ToString();
                c.idContainer = item[1].ToString();
                c.numContainer = item[2] as string;
                c.descripcionContenedor = item[3] as string;
                c.confirmado = item[4] as string;
                c.observacion = item[5] as string;
                c.noFila = item[6].ToString();
                c.tipoVerificacion = item[7] as string;
                c.sello1 = item[8].ToString();
                c.sello2 = item[9].ToString();
                c.sello3 = item[10].ToString();
                c.sello4 = item[11].ToString();
                c.iso = item[12].ToString();

                rs.Add(c);
            }
            return rs;
        }

        //Consulta los contenedores de cada solicitud que este en estado ingresado o en proceso.
        public static List<consultaSolicitudOperador> consultaSolicitudOperador(int idContenedor, string TipoServicio, string fechaInicio, string fechaFin, string Estado, Boolean checkTodos)
        {
            var rs = new List<consultaSolicitudOperador>();
            foreach (var item in CLSDataCentroSolicitud.ValorLecturaConexion("CS_ConsultaSolicitudes_Operador", tComando.Procedure, tConexion.Sca, new Dictionary<string, string>() { { "idContenedor", idContenedor.ToString() }, { "TipoServicio", TipoServicio }, { "fechaInicio", fechaInicio }, { "fechaFin", fechaFin }, { "Estado", Estado }, { "checkTodos", checkTodos.ToString() } }))
            {
                var c = new consultaSolicitudOperador();
                c.idDetalleSolicitud = item[0].ToString();
                c.tipoContenedor = item[1] as string;
                c.servicio = item[2] as string;
                c.contenedor = item[5] as string;
                c.confirmacion = item[6] as string;
                //c.fechaSolicitud = item[4] as string;
                //c.estado = item[6] as string;
                c.observacion = item[8] as string;
                c.numSolicitud = item[9].ToString();
                c.noFila = item[10].ToString();

                c.sello1 = item[11] as string;
                c.sello2 = item[12] as string;
                c.sello3 = item[13] as string;
                c.sello4 = item[14] as string;
                c.numeroCarga = item[15] as string;
                c.peso = item[16] as string;
                c.tipoVerificacion = item[17] as string;
                c.estadoSolicitud = item[18] as string;
                c.iso = item[19] as string;
                c.imo = item.GetBoolean(20);// item[19] as string;
                rs.Add(c);
            }
            return rs;
        }

        //Guarda los detalles de las solicitudes, del tipo de servicios REESTIBA
        public static string actualizacionSolicitudOperario(string idDetalleSolicitud, string observacion, string confirmacion, string nombreUsuario, string tipoVerificacion, string servicio = null)
        {
            //int flagError = 0;
            string retorno = CLSDataCentroSolicitud.ValorEscalar("CS_ActualizacionServiciosOperario", new Dictionary<string, string>() { { "IdDetalleSolicitud", idDetalleSolicitud.ToString() }, { "observacion", observacion }, { "confirmacion", @confirmacion }, { "nombreUsuario", nombreUsuario }, { "tipoVerificacion", tipoVerificacion }, { "cargaServicio", servicio } }, tComando.Procedure, tConexion.Sca);
            return retorno;
        }

        //Consulta todas las solicitudes para que el analista pueda verlas y trabajarlas.        
        public static List<consultaCabeceraAnalista> consultaSolicitudAnalista(string numSolicitud, string TipoServicio, string fechaInicio, string fechaFin, string tipoUsuario, string Estado, Boolean checkTodos, string userName, int idSolicitud, string rutaArchivo = null)
        {
            var rs = new List<consultaCabeceraAnalista>();
            foreach (var item in CLSDataCentroSolicitud.ValorLecturaConexion("CS_ConsultaSolicitudes_Analistas", tComando.Procedure, tConexion.Sca, new Dictionary<string, string>() { { "numSolicitud", numSolicitud }, { "TipoServicio", TipoServicio }, { "fechaInicio", fechaInicio }, { "fechaFin", fechaFin }, { "tipoUsuario", tipoUsuario }, { "Estado", Estado }, { "checkTodos", checkTodos.ToString() }, { "userName", userName }, { "idSolicitud", idSolicitud.ToString() }, {"rutaArchivo", rutaArchivo} }))
            {
                var c = new consultaCabeceraAnalista();
                c.idSolicitud = item[0].ToString();
                c.trafico = item[1] as string;
                c.servicio = item[2] as string;
                c.noBooking = item[3] as string;
                c.noCarga = item[4] as string;
                c.fechaSolicitud = item[5] as string;
                c.estado = item[6] as string;
                c.usuario = item[7] as string;
                c.idEstado = item[8] as string;
                c.observacion = item[9] as string;
                c.nombreDocumento = item[10] as string;
                c.numSolicitud = item[11] as string;

                c.exportador = item[13] as string;
                rs.Add(c);
            }
            return rs;
        }

        public static List<consultaCabeceraAnalista> consultaSolicitudAnalistaCerrojo(string numSolicitud, string TipoServicio, string fechaInicio, string fechaFin, string tipoUsuario, string Estado, Boolean checkTodos, string userName, int idSolicitud, string rutaArchivo = null)
        {
            var rs = new List<consultaCabeceraAnalista>();
            foreach (var item in CLSDataCentroSolicitud.ValorLecturaConexion("CS_ConsultaSolicitudes_Analistas_Cerrojo", tComando.Procedure, tConexion.Sca, new Dictionary<string, string>() { { "numSolicitud", numSolicitud }, { "TipoServicio", TipoServicio }, { "fechaInicio", fechaInicio }, { "fechaFin", fechaFin }, { "tipoUsuario", tipoUsuario }, { "Estado", Estado }, { "checkTodos", checkTodos.ToString() }, { "userName", userName }, { "idSolicitud", idSolicitud.ToString() }, { "rutaArchivo", rutaArchivo } }))
            {
                var c = new consultaCabeceraAnalista();
                c.idSolicitud = item[0].ToString();
                c.trafico = item[1] as string;
                c.servicio = item[2] as string;
                c.noBooking = item[3] as string;
                c.noCarga = item[4] as string;
                c.fechaSolicitud = item[5] as string;
                c.estado = item[6] as string;
                c.usuario = item[7] as string;
                c.idEstado = item[8] as string;
                c.observacion = item[9] as string;
                c.nombreDocumento = item[10] as string;
                c.numSolicitud = item[11] as string;

                rs.Add(c);
            }
            return rs;
        }

        //Actualiza la solicitud, cambiando el estado y la observación.
        public static int actualizacionSolicitudAnalista(int idDetalleSolicitud, string estado, string observacion, string nombreUsuario)
        {
            int flagError = 0;
            /*foreach (var item in CLSDataCentroSolicitud.ValorEscalar("CS_ActualizacionSolicitudAnalista", new Dictionary<string, string>() { { "IdSolicitud", idDetalleSolicitud.ToString() }, { "observacion", observacion }, { "estado", estado }, { "nombreUsuario", nombreUsuario } }, tComando.Procedure, tConexion.Sca))
            {
                flagError = Int32.Parse(item.ToString());
            }*/
            return flagError = Int32.Parse(CLSDataCentroSolicitud.ValorEscalar("CS_ActualizacionSolicitudAnalista", new Dictionary<string, string>() { { "IdSolicitud", idDetalleSolicitud.ToString() }, { "observacion", observacion }, { "estado", estado }, { "nombreUsuario", nombreUsuario } }, tComando.Procedure, tConexion.Sca));
        }

        //Actualiza la solicitud, consultando si todos los containers ya están trabajados.
        public static string actualizacionEstadoSolicitudOperador(string idDetalleSolicitud, string nombreUsuario)
        {
            string flagError = "";
            flagError = CLSDataCentroSolicitud.ValorEscalar("CS_ActualizacionEstadoSolicitud_Operador", new Dictionary<string, string>() { { "IdDetalleSolicitud", idDetalleSolicitud.ToString() }, { "userName", nombreUsuario } }, tComando.Procedure, tConexion.Sca);
            return flagError;
        }

        public static List<Grupo> consultarGrupo(string descripcion, string estado)
        {
            try
            {
                string resultado = string.Empty;
                List<Grupo> grupoTemp = new List<Grupo>();

                foreach (var item in CLSDataCentroSolicitud.ValorLecturaConexion("MA_SE_ConsultaGrupos", tComando.Procedure, tConexion.Master, new Dictionary<string, string>() { { "descripcion", descripcion }, { "estado", estado } }))
                {

                    Grupo g = new Grupo();
                    g.codigo = int.Parse(item[0].ToString().Trim());
                    g.descripcion = item[1].ToString().Trim();
                    g.estado = item[2].ToString().Trim();
                    grupoTemp.Add(g);
                }

                return grupoTemp;
            }
            finally
            { 
            
            }
        }

        //Guarda las cabeceras de solicitudes, con cualquier tipo de servicios
        public static List<datosCabecera> cabeceraSolicitud(int idSolicitud, string TipoServicio, string TipoTrafico, string TipoCarga, string numBooking, string carga, string nombreUsuario, string estado, string tipoUsuario, string referencia = null, string fechaCO = null, string fechaCOHasta = null)
        {

            var rs = new List<datosCabecera>();
            var dp = CLSDataCentroSolicitud.ValorLecturaConexion("CS_IngresoServiciosCabecera_Solicitud", tComando.Procedure, tConexion.Sca, new Dictionary<string, string>() { { "IdSolicitud", idSolicitud.ToString() }, { "tipoServicio", TipoServicio }, { "tipoTrafico", TipoTrafico }, { "tipoCarga", TipoCarga }, { "numBooking", numBooking }, { "numcarga", carga }, { "nombreUsuario", nombreUsuario }, { "idTipoUsuario", tipoUsuario.ToString() }, { "Estado", estado }, { "referenciaCU", referencia }, { "fechaCO", fechaCO }, { "fechaCOHasta", fechaCOHasta } });

            foreach (var item in dp)
            {
                var c = new datosCabecera();
                c.idSolicitud = item[0].ToString();
                c.codigoSolicitud = item[1] as string;
                rs.Add(c);
            }
            return rs;
        }

        //Guarda los detalles de las solicitudes, de cualquier tipo de servicios
        public static int detalleSolicitud(int idSolicitud, int IDcontenedor, string nombreUsuario)
        {
            int flagError = 0;

            foreach (var item in CLSDataCentroSolicitud.ValorEscalar("CS_IngresoServiciosDetalle_Solicitud", new Dictionary<string, string>() { { "IdSolicitud", idSolicitud.ToString() }, { "contenedor", IDcontenedor.ToString() }, { "nombreUsuario", nombreUsuario } }, tComando.Procedure, tConexion.Sca))
            {
                flagError = Int32.Parse(item.ToString());
            }
            return flagError;
        }

        public static int detalleSolicitud(int idSolicitud, int IDcontenedor, string nombreUsuario, string imo)
        {
            int flagError = 0;

            foreach (var item in CLSDataCentroSolicitud.ValorEscalar("CS_IngresoServiciosDetalle_Solicitud", new Dictionary<string, string>() { { "IdSolicitud", idSolicitud.ToString() }, { "contenedor", IDcontenedor.ToString() }, { "nombreUsuario", nombreUsuario }, { "imo", imo } }, tComando.Procedure, tConexion.Sca))
            {
                flagError = Int32.Parse(item.ToString());
            }
            return flagError;
        }
        public static List<contenedoresCerrojoElectronico> consultarGrupoExcel(DataTable dt, string inbound = null)
        {
            try
            {
                string resultado = string.Empty;
                List<contenedoresCerrojoElectronico> rs = new List<contenedoresCerrojoElectronico>();


                //Obtener configuraciones

                foreach (var item in CLSDataCentroSolicitud.ValorLecturaCerrojoElectronico("CS_VerificarContenedor_CerrojoElec", tComando.Procedure, tConexion.Sca, dt, inbound))
                {

                    var c = new contenedoresCerrojoElectronico();
                    c.noContenedor = item[0].ToString();
                    c.idCodigoContenedor = item[1].ToString();
                    c.descripcion = item[2].ToString() as string;
                    c.observacion = String.IsNullOrEmpty(item[3].ToString()) ? item[4].ToString() as string : item[3].ToString() as string;
                    c.grupo = item[5] as string;
                    rs.Add(c);
                }

                return rs;
            }
            finally
            { 
            
            }
        }
        #region Solicitud Correccion Ingreso de Exportacion

        public static List<contenedor> contenedoresExportacion(string contenedor, string tipo, string trafico, string correccion, string aisv, int idUsuario)
        {
            var rs = new List<contenedor>();
            foreach (var item in CLSDataCentroSolicitud.ValorLecturaConexion("CS_ConsultaContenedoresExportador", tComando.Procedure, tConexion.Sca, new Dictionary<string, string>() { { "tipoContenedorC", tipo }, { "tipo", trafico }, { "unidad", contenedor }, { "bookingC", "" }, { "correccionC", correccion }, { "aisvC", aisv }, { "idUsuario", idUsuario.ToString() } }))
            {
                var c = new contenedor();
                c.idConsecutivo = item[0].ToString();
                c.nombrecont = item[1] as string;
                c.pesoContenedor = item[3].ToString();
                c.booking = item[4] as string;
                c.tipoContenedor = item[5] as string;
                c.dae = item[6] as string;
                c.sello1 = item[7] as string;
                c.sello2 = item[8] as string;
                c.sello3 = item[9] as string;
                c.sello4 = item[10] as string;
                c.codigo_carga = item[11] as string;
                c.peso = item[12] as string;
                c.aisv = item[13] as string;
                c.iso = item[14].ToString();
                rs.Add(c);
            }
            return rs;
        }


        //Guarda los detalles de las solicitudes, del tipo de servicios REESTIBA
        public static int detalleSolicitudExportacionDae(int idSolicitud, int contenedor1, string telefono, string mail, string aisv, string daeAnterior, string daeNueva, string tipoCorreccion, string nombreUsuario, string numeroEntrega, string descripcionContenedor)
        {
            int flagError = 0;
            //var rs = new List<reestiba>();
            foreach (var item in CLSDataCentroSolicitud.ValorEscalar("CS_IngresoServiciosDetalle_ExportacionDAE", new Dictionary<string, string>() { { "IdSolicitud", idSolicitud.ToString() }, { "contenedor", contenedor1.ToString() }, { "aisv", aisv.ToString() }, { "daeAnt", daeAnterior.ToString() }, { "daeNue", daeNueva.ToString() }, { "telefono", telefono.ToString() }, { "mail", mail.ToString() }, { "tipoCorreccion", tipoCorreccion.ToString() }, { "nombreUsuario", nombreUsuario }, { "numeroEntrega", numeroEntrega }, { "descripcionContenedor", descripcionContenedor } }, tComando.Procedure, tConexion.Sca))
            {
                flagError = Int32.Parse(item.ToString());
            }
            return flagError;
        }


        public static string consultaEventoPorServicio(string grupo)
        {
            try
            {
                string resultado = string.Empty;
                foreach (var item in CLSDataCentroSolicitud.ValorLecturaConexion("CS_ConsultaEvento", tComando.Procedure, tConexion.Sca, new Dictionary<string, string>() { { "codigoServicio", grupo } }))
                {
                   resultado = item[0].ToString();
                }

                return resultado;
            }
            catch (Exception ex)
            {
                log_csl.save_log<Exception>(ex, "CslHelperServicios", "consultaEventoPorServicio", ex.Message, "N4");
                return null;
            }

        }

        //Guarda los detalles de las solicitudes, del tipo de servicios REESTIBA
        public static int ingresoOperacionExportacionDAE(string codigoCarga, string codigoTrafico, string codigoTipoSolicitud, string usuario, string contenedor)
        {
            int flagError = 0;
            //var rs = new List<reestiba>();
            foreach (var item in CLSDataCentroSolicitud.ValorEscalar("CS_InsercionOperacionExportacionDAE", new Dictionary<string, string>() { { "codigoCarga", codigoCarga.ToString() }, { "contenedor", contenedor.ToString() }, { "codigoTrafico", codigoTrafico.ToString() }, { "codigoTipoSolicitud", codigoTipoSolicitud.ToString() }, { "usuario", usuario.ToString() } }, tComando.Procedure, tConexion.Ecuapass))
            {
                flagError = Int32.Parse(item.ToString());
            }
            return flagError;
        }

        //Consulta el detalle de una solicitud seleccionada por un usuario.        
        public static List<consultaDetalleUsuarioExportacion> consultaSolicitudDetalleUsuarioExportacion(int numSolicitud, string userName)
        {
            var rs = new List<consultaDetalleUsuarioExportacion>();
            foreach (var item in CLSDataCentroSolicitud.ValorLecturaConexion("CS_ConsultaSolicitudes_Detalle_Usuarios_Exportacion", tComando.Procedure, tConexion.Sca, new Dictionary<string, string>() { { "numSolicitud", numSolicitud.ToString() }, { "userName", userName } }))
            {
                var c = new consultaDetalleUsuarioExportacion();
                c.idDetalleSolicitud = item[0].ToString();
                c.idContainer = item[1].ToString();
                c.descripcionContenedor = item[2] as string;
                c.noFila = item[3].ToString();
                c.dae = item[4] as string;
                c.aisv = item[5] as string;
                c.numeroEntrega = item[6] as string;
                rs.Add(c);
            }
            return rs;
        }


        #endregion

        #region Lista de embarque

        public static List<Buque> consultaReferenciaBuques(string linea, string anio, string buque, string identificacion)
        {
            var rs = new List<Buque>();
            foreach (var item in CLSDataCentroSolicitud.ValorLecturaConexion("CS_ConsultaBuques", tComando.Procedure, tConexion.Sca, new Dictionary<string, string>() { { "referenciaC", linea.Trim().ToUpper() + anio.Trim().ToUpper() }, { "nombreBuqueC", buque }, { "identificacionC", identificacion } }))
            {
                var c = new Buque();
                c.noFila = item[0].ToString();
                c.referencia = item[1].ToString();
                c.agencia = item[4].ToString();
                c.buque = item[3].ToString();
                c.viaje = item[5].ToString();
                var fecha = item.GetDateTime(6);
                c.cutoff = fecha.ToString("yyyy-MM-dd HH:mm");
                c.tipoReporte = item[7].ToString();
                rs.Add(c);
            }
            return rs;
        }

        public static List<contenedorBuque> consultaBuquesContenedor(string referencia, string idsuario)
        {
            var rs = new List<contenedorBuque>();
            foreach (var item in CLSDataCentroSolicitud.ValorLecturaConexion("CS_ConsultaBuquesContenedores", 
                tComando.Procedure, tConexion.Sca, new Dictionary<string, string>() { { "referenciaC", referencia }, { "identificacionC", idsuario } }))
            {
                var c = new contenedorBuque();
                c.referencia = item[0].ToString();
                c.trafico = item[1].ToString();
                c.linea = item[2].ToString();
                c.unidad = item[3].ToString();
                c.booking = item[4].ToString();
                c.tipo = item[5].ToString();
                c.dae = item[6].ToString();
                c.ingreso = item[7].ToString();
                c.exportador = item[8].ToString();
                c.cliente_aisv = item[9].ToString();
                c.stuff = item[10].ToString();
                c.tipo_roleo = item[12].ToString();
                c.roleo = Convert.ToBoolean(int.Parse((item[11].ToString().Trim())));
                c.bloqueo = Convert.ToBoolean(int.Parse((item[13].ToString().Trim())));
                c.gkey = int.Parse(item[14].ToString().Trim());
                c.asumoCosto = bool.Parse(item[15].ToString().Trim());
                c.habilitar = !bool.Parse(item[15].ToString().Trim());
                c.tipo_bloqueo = item[16].ToString().Trim();
                c.backColor = item[17].ToString().Trim();
                c.tipoCliente = item[18].ToString().Trim();
                rs.Add(c);
            }
            return rs;
        }

        public static int ingresoListadoEmbarque(DataTable dtEmbarque, string referencia, int idUsuario)
        {
            int flagError = 0;
            //var rs = new List<reestiba>();
            foreach (var item in CLSDataSeguridad.ValorEscalarConjunto(tConexion.Sca, "CS_IngresoModificacionListadoEmbarque", new Dictionary<string, string>() { { "referencia", referencia.ToString() }, { "idUsuarioOperacion", idUsuario.ToString() } }, "contenedores", dtEmbarque, tComando.Procedure))
            {
                flagError = Int32.Parse(item.ToString());
            }
            return flagError;
        }


        public static List<contenedorLate> consultaBuquesContenedorLate(string referencia, string estado)
        {
            var rs = new List<contenedorLate>();
            foreach (var item in CLSDataCentroSolicitud.ValorLecturaConexion("CS_ConsultaBuquesContenedoresLate", tComando.Procedure, tConexion.Sca, new Dictionary<string, string>() { { "referenciaC", referencia },{"transitoC", estado} }))
            {
                var c = new contenedorLate();
                c.exportador = item[0].ToString();
                c.contenedor = item[1].ToString();
                c.dae = item[2].ToString();
                c.ingreso = item[3].ToString();
                c.cutoff = item[4].ToString();
                c.cutoffMaximo = item[5].ToString();
                c.lateArrival = item[6].ToString();
                c.cliente = item[7].ToString();
                c.booking = item[8].ToString();
                c.gkey = int.Parse(item[9].ToString());
                c.estado = item[10].ToString();
                c.linea = item[11].ToString();
                rs.Add(c);
            }
            return rs;
        }

        #endregion
        //Consulta si el contenedor tiene un proceso de IMDT, retorna el ID del mismo en caso de que exista
        public static List<camposSolicitudTransaccion> consultaIMDT(string idContenedor, string descripcionContenedor)
        {
            var rs = new List<camposSolicitudTransaccion>();
            foreach (var item in CLSDataCentroSolicitud.ValorLecturaConexion("CS_PROCESO_CIIS_VERIFICACION_SELLOS", tComando.Procedure, tConexion.Ecuapass, new Dictionary<string, string>() { { "paso", "IMDT" }, { "idContenedor", idContenedor }, { "descripcionContenedor", descripcionContenedor } }))
            {
                var c = new camposSolicitudTransaccion();
                c.codigoSolicitudTransaccion = item[0] as string;
                c.codigoCarga = item[1] as string;
                c.contenedor = item[12] as string;
                c.mrn = item[14] as string;
                c.msn = item[15] as string;
                c.hsn = item[16] as string;
                c.numeroEntrega = item[17] as string;
                rs.Add(c);
            }
            return rs;
        }

        //Hace el ingreso para el proceso de SOLI
        public static string ingresoSOLI(string idContenedor, string descripcionContenedor, string mrm, string msn, string hsn, string username)
        {
            string codigoSolicitudTransaccion = "";
            codigoSolicitudTransaccion = CLSDataCentroSolicitud.ValorEscalar("CS_PROCESO_CIIS_VERIFICACION_SELLOS", new Dictionary<string, string>() { { "paso", "SOLI" }, { "idContenedor", idContenedor }, { "descripcionContenedor", descripcionContenedor }, { "mrn", mrm }, { "msn", msn }, { "hsn", hsn }, { "username", username } }, tComando.Procedure, tConexion.Ecuapass);
            return codigoSolicitudTransaccion;
        }

        //Hace el ingreso para el proceso de TRAN
        public static string ingresoTRAN(string idContenedor, string codigoSolicitudGenerado, string codigoSolicitud, string numeroEntrega, string descripcionContenedor, string mrn, string msn, string hsn)
        {
            string secuenciaNumerica = "";
            secuenciaNumerica = CLSDataCentroSolicitud.ValorEscalar("CS_PROCESO_CIIS_VERIFICACION_SELLOS", new Dictionary<string, string>() { { "paso", "TRAN" }, { "codigoSolicitudTGenerado", codigoSolicitudGenerado }, { "codigoSolicitudTransaccion", codigoSolicitud }, { "numeroEntrega", numeroEntrega }, { "idContenedor", idContenedor }, { "descripcionContenedor", descripcionContenedor }, { "mrn", mrn }, { "msn", msn }, { "hsn", hsn } }, tComando.Procedure, tConexion.Ecuapass);
            return secuenciaNumerica;
        }

        //Hace el ingreso para el proceso de ARGU
        public static string ingresoARGU(string codigoSolicitudTransaccion, string argumento, string valor)
        {
            string secuenciaNumerica = "";
            secuenciaNumerica = CLSDataCentroSolicitud.ValorEscalar("CS_PROCESO_CIIS_VERIFICACION_SELLOS", new Dictionary<string, string>() { { "paso", "ARGU" }, { "codigoSolicitudTransaccion", codigoSolicitudTransaccion }, { "argumento", argumento }, { "valor", valor } }, tComando.Procedure, tConexion.Ecuapass);
            return secuenciaNumerica;
        }

        public static List<listaContenedoresSellos> consultaSellosAnteriores(string idContenedor)
        {
            var rs = new List<listaContenedoresSellos>();
            foreach (var item in CLSDataCentroSolicitud.ValorLecturaConexion("FNA_FUN_SOLICITUD_CIIS", tComando.Procedure, tConexion.N4Catalog, new Dictionary<string, string>() { { "idUnit", idContenedor } }))
            {
                var c = new listaContenedoresSellos();
                c.codigoSolicitudTransaccion = item[0].ToString();
                c.valor = item[1] as string;
                c.IQ = item[3].ToString();
                rs.Add(c);
            }
            return rs;
        }
        //Consulta si el contenedor tiene un proceso de IMDT, retorna el ID del mismo en caso de que exista. (Primer Intento)
        public static consultaIMDT consultaProcesoIMDTUno(string idContenedor, string descripcionContenedor)
        {
            var c = new consultaIMDT();
            foreach (var item in CLSDataCentroSolicitud.ValorLecturaConexion("CS_PROCESO_IMPADT_VERIFICACION_SELLOS", tComando.Procedure, tConexion.Ecuapass, new Dictionary<string, string>() { { "paso", "IMDTU" }, { "idContenedor", idContenedor }, { "descripcionContenedor", descripcionContenedor } }))
            {
                c.mrn = item[0] as string;
                c.msn = item[1] as string;
                c.hsn = item[2] as string;
                c.contenedor = item[3] as string;
                c.codigoCarga = item[4] as string;
                c.numeroEntrega = item[5] as string;
            }
            return c;
        }

        //Consulta si el contenedor tiene un proceso de IMDT, retorna el ID del mismo en caso de que exista. (Segundo Intento)
        public static consultaIMDT consultaProcesoIMDTDos(string idContenedor, string descripcionContenedor)
        {
            var c = new consultaIMDT();
            foreach (var item in CLSDataCentroSolicitud.ValorLecturaConexion("CS_PROCESO_IMPADT_VERIFICACION_SELLOS", tComando.Procedure, tConexion.Ecuapass, new Dictionary<string, string>() { { "paso", "IMDTD" }, { "idContenedor", idContenedor }, { "descripcionContenedor", descripcionContenedor } }))
            {
                c.mrn = item[0] as string;
                c.msn = item[1] as string;
                c.hsn = item[2] as string;
                c.contenedor = item[3] as string;
                c.codigoCarga = item[4] as string;
                c.referencia = item[5] as string;
            }
            return c;
        }

        //Consulta si el contenedor tiene un proceso de IMDT, retorna el ID del mismo en caso de que exista. (Tercer y Último Intento)
        public static consultaIMDT consultaProcesoIMDTTres(string idContenedor, string descripcionContenedor)
        {
            var c = new consultaIMDT();
            foreach (var item in CLSDataCentroSolicitud.ValorLecturaConexion("CS_PROCESO_IMPADT_VERIFICACION_SELLOS", tComando.Procedure, tConexion.Ecuapass, new Dictionary<string, string>() { { "paso", "IMDTT" }, { "idContenedor", idContenedor }, { "descripcionContenedor", descripcionContenedor } }))
            {
                c.mrn = item[0] as string;
                c.msn = item[1] as string;
                c.hsn = item[2] as string;
                c.contenedor = item[3] as string;
                c.secuencia = item[4].ToString();
            }
            return c;
        }

        //Hace el ingreso para el proceso de SOLI de IMPDT
        public static string ingresoIMPDTSOLI(string idContenedor, string descripcionContenedor, string mrm, string msn, string hsn, string username)
        {
            string codigoSolicitudTransaccion = "";
            codigoSolicitudTransaccion = CLSDataCentroSolicitud.ValorEscalar("CS_PROCESO_IMPADT_VERIFICACION_SELLOS", new Dictionary<string, string>() { { "paso", "SOLI" }, { "idContenedor", idContenedor }, { "descripcionContenedor", descripcionContenedor }, { "mrn", mrm }, { "msn", msn }, { "hsn", hsn }, { "username", username } }, tComando.Procedure, tConexion.Ecuapass);
            return codigoSolicitudTransaccion;
        }

        //Hace el ingreso para el proceso de TRAN de IMPDT
        public static string ingresoIMPDTTRAN(string idContenedor, string codigoSolicitudGenerado, string codigoSolicitud, string numeroEntrega, string descripcionContenedor, string mrn, string msn, string hsn)
        {
            string secuenciaNumerica = "";
            secuenciaNumerica = CLSDataCentroSolicitud.ValorEscalar("CS_PROCESO_IMPADT_VERIFICACION_SELLOS", new Dictionary<string, string>() { { "paso", "TRAN" }, { "codigoSolicitudTGenerado", codigoSolicitudGenerado }, { "codigoSolicitudTransaccion", codigoSolicitud }, { "numeroEntrega", numeroEntrega }, { "idContenedor", idContenedor }, { "descripcionContenedor", descripcionContenedor }, { "mrn", mrn }, { "msn", msn }, { "hsn", hsn } }, tComando.Procedure, tConexion.Ecuapass);
            return secuenciaNumerica;
        }

        //Hace el ingreso para el proceso de ARGU de IMPDT
        public static string ingresoIMPDTARGU(string codigoSolicitudTransaccion, string argumento, string valor)
        {
            string secuenciaNumerica = "";
            secuenciaNumerica = CLSDataCentroSolicitud.ValorEscalar("CS_PROCESO_IMPADT_VERIFICACION_SELLOS", new Dictionary<string, string>() { { "paso", "ARGU" }, { "codigoSolicitudTransaccion", codigoSolicitudTransaccion }, { "argumento", argumento }, { "valor", valor } }, tComando.Procedure, tConexion.Ecuapass);
            return secuenciaNumerica;
        }

        public static string generarIIEa(string gKey, string descripcionContenedor, string peso, string mrn, string msn, string hsn, string username, string fechaFormato, string sello1, string sello2, string sello3, string sello4, string iso)
        {
            string secuenciaNumerica = "";
            secuenciaNumerica = CLSDataCentroSolicitud.ValorEscalar("CS_PROCESO_IIE_CS", new Dictionary<string, string>() { { "idContenedor", gKey }, { "descripcionContenedor", descripcionContenedor }, { "peso", peso }, { "mrn", mrn }, { "msn", msn }, { "hsn", hsn }, { "username", username }, { "fechaFormato", fechaFormato }, { "sello1", sello1 }, { "sello2", sello2 }, { "sello3", sello3 }, { "sello4", sello4 }, { "iso", iso } }, tComando.Procedure, tConexion.Ecuapass);
            return secuenciaNumerica;
        }


        public static bool generarIIE(Int64 ikey,string ibooking, string iuser, out string tranmen, out string tradae )
        {
            bool ok = true;

            tranmen = "";
            tradae = "";
            return ok;
        }


        public static string generarCII(string gKey, string descripcionContenedor, string peso, string mrn, string msn, string hsn, string username, string sello1, string sello2, string sello3, string sello4)
        {
           

            string secuenciaNumerica = "";
            secuenciaNumerica = CLSDataCentroSolicitud.ValorEscalar("CS_PROCESO_CII_ELIMINACION_CS", new Dictionary<string, string>() { { "idContenedor", gKey }, { "descripcionContenedor", descripcionContenedor }, { "peso", peso }, { "mrn", mrn }, { "msn", msn }, { "hsn", hsn }, { "username", username }, { "sello1", sello1 }, { "sello2", sello2 }, { "sello3", sello3 }, { "sello4", sello4 } }, tComando.Procedure, tConexion.Ecuapass);
            return secuenciaNumerica;
        }

        public static IIE validarIIE(string gKey)
        {
            var c = new IIE();
            foreach (var item in CLSDataCentroSolicitud.ValorLecturaConexion("CS_VALIDA_IIE_CS", tComando.Procedure, tConexion.Ecuapass, new Dictionary<string, string>() { { "idContenedor", gKey } }))
            {
                c.estado = item[0].ToString();
                c.gKey = item[1].ToString();
            }
            return c;
        }


        public static string consultaEventoPorTipoServicio(string grupo, string tipoServicio)
        {
            try
            {
                string resultado = string.Empty;
                //List<evento> rs = new List<evento>();

                foreach (var item in CLSDataCentroSolicitud.ValorLecturaConexion("CS_ConsultaEventoTipoServicio", tComando.Procedure, tConexion.Sca, new Dictionary<string, string>() { { "codigoServicio", grupo }, { "tipoServicio", tipoServicio } }))
                {

                    /*var c = new evento();
                    c.descripcion = item[0].ToString();
                    rs.Add(c);*/
                    resultado = item[0].ToString();
                }

                return resultado;
            }
            finally
            { 
            
            }

        }


        public static List<string> consultaEventoPorTipoServicio_multiple(string grupo, string tipoServicio)
        {
            try
            {
                var resultado = new List<string>();
                foreach (var item in CLSDataCentroSolicitud.ValorLecturaConexion("CS_ConsultaEventoTipoServicio", tComando.Procedure, tConexion.Sca, new Dictionary<string, string>() { { "codigoServicio", grupo }, { "tipoServicio", tipoServicio } }))
                {
                    resultado.Add(item[0].ToString());
                }
                return resultado;
            }
            finally
            {

            }

        }
    

    }

    public class listaContenedoresSellos
    {

        public string codigoSolicitudTransaccion { get; set; }
        public string argumento { get; set; }
        public string valor { get; set; }
        public string IQ { get; set; }
    }

    public class camposSolicitudTransaccion
    {
        public string codigoSolicitudTransaccion { get; set; }
        public string codigoCarga { get; set; }
        public string contenedor { get; set; }
        public string mrn { get; set; }
        public string msn { get; set; }
        public string hsn { get; set; }
        public string numeroEntrega { get; set; }
    }

    public class evento
    {
        public string descripcion { get; set; }
    }

    public class contenedor
    {
        public string idConsecutivo { get; set; }
        public string nombrecont { get; set; }
        public string booking { get; set; }
        public string tipoContenedor { get; set; }
        public string pesoContenedor { get; set; }
        public bool check { get; set; }
        public string dae { get; set; }
        public string sello1 { get; set; }
        public string sello2 { get; set; }
        public string sello3 { get; set; }
        public string sello4 { get; set; }
        public string peso { get; set; }
        public string codigo_carga { get; set; }
        public string aisv { get; set; }
        public string iso { get; set; }
    }

    public class consultaCabeceraUsuario
    {
        public string idSolicitud { get; set; }
        public string trafico { get; set; }
        public string servicio { get; set; }
        public string noBooking { get; set; }
        public string noCarga { get; set; }
        public string fechaSolicitud { get; set; }
        public string estado { get; set; }
        public string nombreUsuario { get; set; }
        public string correoUsuario { get; set; }
        public string codigoServicio { get; set; }
        public string codigoSolicitud { get; set; }
        public string fechaPropuesta { get; set; }
        public string comentarios { get; set; }
        public string tipoEmbalaje { get; set; }
        public string observacion { get; set; }
        public string referencia { get; set; }
      
    }

    public class consultaDetalleUsuario {

        public string idDetalleSolicitud { get; set; }
        public string idContainer { get; set; }
        public string numContainer { get; set; }
        public string descripcionContenedor { get; set; }
        public string confirmado { get; set; }
        public string observacion { get; set; }
        public string noFila { get; set; }
        public string tipoVerificacion { get; set; }
        public string sello1 { get; set; }
        public string sello2 { get; set; }
        public string sello3 { get; set; }
        public string sello4 { get; set; }
        public string iso { get; set; }
    }

    public class consultaSolicitudOperador {

        public string idDetalleSolicitud { get; set; }
        public string contenedor { get; set; }
        public string tipoContenedor { get; set; }
        public string servicio { get; set; }
        public string fechaSolicitud { get; set; }
        public string estado { get; set; }
        public string observacion { get; set; }
        public string confirmacion { get; set; }
        public string numSolicitud { get; set; }
        public string noFila { get; set; }
        public string sello1 { get; set; }
        public string sello2 { get; set; }
        public string sello3 { get; set; }
        public string sello4 { get; set; }
        public string numeroCarga { get; set; }
        public string peso { get; set; }
        public string tipoVerificacion { get; set; }
        public string estadoSolicitud { get; set; }
        public string iso { get; set; }
        public bool imo { get; set; }
    }

    public class consultaCabeceraAnalista
    {
        public string idSolicitud { get; set; }
        public string trafico { get; set; }
        public string servicio { get; set; }
        public string noBooking { get; set; }
        public string noCarga { get; set; }
        public string fechaSolicitud { get; set; }
        public string estado { get; set; }
        public string usuario { get; set; }
        public string idEstado { get; set; }
        public string observacion { get; set; }
        public string nombreDocumento { get; set; }
        public string numSolicitud { get; set; }
        public string contenedor { get; set; }
        public string exportador { get; set; }


    }

    public class contenedoresCerrojoElectronico
    {
        public string noContenedor { get; set; }
        public string idCodigoContenedor { get; set; }
        public string descripcion { get; set; }
        public string observacion { get; set; }
        public string grupo { get; set; }
    }

    public class datosCabecera
    {



        public string idSolicitud { get; set; }



        public string codigoSolicitud { get; set; }

    }
    public class consultaDetalleUsuarioExportacion
    {

        public string idDetalleSolicitud { get; set; }
        public string idContainer { get; set; }
        public string numContainer { get; set; }
        public string descripcionContenedor { get; set; }
        public string aisv { get; set; }
        public string dae { get; set; }
        public string numeroEntrega { get; set; }
        public string noFila { get; set; }
    }

    public class Buque
    {
        public string referencia { get; set; }
        public string noFila { get; set; }
        public string agencia { get; set; }
        public string buque { get; set; }
        public string viaje { get; set; }
        public string cutoff { get; set; }
        public string tipoReporte { get; set; }
    }

    public class contenedorBuque
    {
        public string referencia { get; set; }
        public string linea { get; set; }
        public string trafico { get; set; }
        public string unidad { get; set; }
        public string booking { get; set; }
        public string ingreso { get; set; }
        public string dae { get; set; }
        public string exportador { get; set; }
        public string cliente_aisv { get; set; }
        public string tipo { get; set; }
        public string stuff { get; set; }
        public string tipo_roleo { get; set; }
        public bool roleo { get; set; }
        public string tipo_bloqueo { get; set; }
        public bool bloqueo { get; set; }
        public int gkey { get; set; }
        public bool asumoCosto { get; set; }
        public bool habilitar { get; set; }
        public string backColor { get; set; }
        public string tipoCliente { get; set; }
    }

    public class consultaIMDT
    {

        public string mrn { get; set; }
        public string msn { get; set; }
        public string hsn { get; set; }
        public string contenedor { get; set; }
        public string codigoCarga { get; set; }
        public string numeroEntrega { get; set; }
        public string referencia { get; set; }
        public string secuencia { get; set; }
    }




    public class IIE
    {

        public string gKey { get; set; }
        public string estado { get; set; }
    }

    public class contenedorLate
    {
        public string exportador { get; set; }
        public string contenedor { get; set; }
        public string dae { get; set; }
        public string ingreso { get; set; }
        public string cutoff { get; set; }
        public string cutoffMaximo { get; set; }
        public string lateArrival { get; set; }
        public string cliente { get; set; }
        public string booking { get; set; }
        public int gkey {get;set;}
        public string estado { get; set; }
        public string linea { get; set;}
    }


    public class unidadN4
    {
        public Int64 gkey { get; set; }
        public string cntr { get; set; }
        public string categoria { get; set; }
        public string fk { get; set; }
        public string s1 { get; set; }
        public string s2 { get; set; }
        public string s3 { get; set; }
        public string s4 { get; set; }
        public string iso { get; set; }
        public string grupo { get; set; }
        public float peso { get; set; }
        public string boking { get; set; }
        public string mrn { get; set; }
        public string msn { get; set; }
        public string hsn { get; set; }
        public string propID { get; set; }
        public string propNombre { get; set; }
        public string doc { get; set; }
        public string aisv { get; set; }
        public string referencia { get; set; }
        public string expoUser { get; set; }
        public Int64 imokey { get; set; }
        public bool esImo { get; set; }
        public bool esRefer { get; set; }
        public string linea { get; set; }
        public string patio { get; set; }
        public bool check { get;set;}

        public static List<unidadN4> consultaPortalIMPO(string mrn, string msn, string hsn, string cntr, string ruc, string linea)
        {
            var lst = new List<unidadN4>();
            //pc_consulta_impo_expo
            //csl_service
            var par = new Dictionary<string,object>();
            if (!string.IsNullOrEmpty(mrn))
            {
                par.Add("mrn", mrn);
            }
            if (!string.IsNullOrEmpty(msn))
            {
                par.Add("msn", msn);
            }
            if(!string.IsNullOrEmpty(hsn))
            {
             par.Add("hsn",hsn);
            }
            if (!string.IsNullOrEmpty(ruc))
            {
                par.Add("rucU", ruc);
            }
            if (!string.IsNullOrEmpty(linea))
            {
                par.Add("linea", linea);
            }
            if (!string.IsNullOrEmpty(cntr))
            {
                par.Add("id", cntr);
            }
             if (par.Count <= 0)
            {
                //no agregó parametros
                return lst;
            }
            par.Add("trafico", "IMPRT");
            var parx = string.Format("mrn:{0}|msn:{1}|hsn:{2}|cntr:{3}|ruc:{4}|linea:{5}",mrn,msn,hsn,cntr,ruc,linea);
            lst= app_start.dataHelper.selectData<unidadN4>("service", "pc_consulta_impo_expo", par, true, OnError, "unidadN4", "consultaPortalIMPO", parx);
            return lst;
        
        }
        public static List<unidadN4> consultaPortalEXPO(string boking,  string cntr, string ruc, string linea)
        {
            var lst = new List<unidadN4>();
            //pc_consulta_impo_expo
            //csl_service
            var par = new Dictionary<string, object>();

            if (!string.IsNullOrEmpty(ruc))
            {
                par.Add("rucU", ruc);
            }
            if (!string.IsNullOrEmpty(linea))
            {
                par.Add("linea", linea);
            }
            if (!string.IsNullOrEmpty(cntr))
            {
                par.Add("id", cntr);
            }
            if (!string.IsNullOrEmpty(boking))
            {
                par.Add("boking", boking);
            }

            if (par.Count <= 0)
            {
                //no agregó parametros
                return lst;
            }
            par.Add("trafico", "EXPRT");
            var parx = string.Format("cntr:{0}|ruc:{1}|linea:{2}", cntr, ruc, linea);
            lst = app_start.dataHelper.selectData<unidadN4>("service", "pc_consulta_impo_expo", par, true, OnError, "unidadN4", "consultaPortalEXPO", parx);
            return lst;

        }


        /*Nuevo*/
        public static List<unidadN4> consultaPortal(string trafico, string boking,string mrn, string msn, string hsn, string cntr, string ruc, string linea, string servicio)
        {
            var lst = new List<unidadN4>();
            //pc_consulta_impo_expo
            //csl_service
            var par = new Dictionary<string, object>();

            if (!string.IsNullOrEmpty(ruc))
            {
                par.Add("rucU", ruc);
            }
            if (!string.IsNullOrEmpty(linea))
            {

                if (!linea.Contains("0992506717001"))
                par.Add("linea", linea);
            }
            if (!string.IsNullOrEmpty(cntr))
            {
                par.Add("id", cntr);
            }
            if (!string.IsNullOrEmpty(boking))
            {
                par.Add("boking", boking);
            }

            if (!string.IsNullOrEmpty(mrn))
            {
                par.Add("mrn", mrn.Trim());
                par.Add("msn", msn.Trim());
                par.Add("hsn", hsn.Trim());
            }

            if (!string.IsNullOrEmpty(servicio))
            {
                par.Add("servicio", servicio.Trim().ToUpper());
            }


            if (!string.IsNullOrEmpty(trafico))
            {
                par.Add("trafico", trafico.Trim().ToUpper());
            }

            if (par.Count <= 0)
            {
                //no agregó parametros
                return lst;
            }
            var parx = string.Format("cntr:{0}|ruc:{1}|linea:{2}", cntr, ruc, linea);
            lst = app_start.dataHelper.selectData<unidadN4>("service", "pc_consulta_impo_expo_servicio", par, true, OnError, "unidadN4", "consultaPortal", parx);
            return lst;

        }



        public static void OnError(string clase, string metodo, string input, string user, Exception ex)
        {
            csl_log.log_csl.save_log<Exception>(ex, clase, metodo, input, user);
        }
        //obtiene los datos de 1 unidd en particlar en ese momento del tiempo
        public unidadN4 getData(string id , string trafico )
        {
            var uni = new unidadN4();

            var lst = new List<unidadN4>();
            //pc_consulta_impo_expo
            //csl_service
            var par = new Dictionary<string, object>();


            if (!string.IsNullOrEmpty(linea))
            {
                par.Add("linea", linea);
            }
            if (!string.IsNullOrEmpty(cntr))
            {
                par.Add("id", cntr);
            }
            if (par.Count <= 0)
            {
                //no agregó parametros
                return uni;
            }
            par.Add("trafico", trafico);
            var parx = string.Format("cntr:{0}|linea:{1}|trafico:{2}", cntr, linea,trafico);
            lst = app_start.dataHelper.selectData<unidadN4>("service", "pc_consulta_impo_expo", par, true, OnError, "unidadN4", "getData", parx);
            if (lst != null && lst.Count > 0)
            {
               uni= lst.OrderByDescending(d => d.gkey).FirstOrDefault();
            }
            return uni;
        }
        public static List<unitFull> consultaUnidadesN4(string trafico, string estado, List<string> contenedores, string booking=null)
        {
            var lst = new List<unitFull>();
            //pc_consulta_impo_expo
            //csl_service
            var par = new Dictionary<string, object>();
            if (!string.IsNullOrEmpty(trafico))
            {
                par.Add("trafico", trafico);
            }


            if (!string.IsNullOrEmpty(estado))
            {
                par.Add("estado", estado);
            }

            if (contenedores == null || contenedores.Count <= 0)
            {
                return lst;
            }

            if (!string.IsNullOrEmpty(booking))
            {
                par.Add("boking", booking.Trim());
            }

            var xml = new System.Text.StringBuilder();
            xml.Append("<cntrs>");
            contenedores.ForEach(a => xml.AppendFormat("<cntr id=\"{0}\"/>",a));
            xml.Append("</cntrs>");
            par.Add("unidades",xml.ToString());


            if (par.Count <= 0)
            {
                //no agregó parametros
                return lst;
            }
          


            var parx = string.Format("trafico:{0}|estado:{1}|cntr:{2}", trafico,estado, xml.ToString());
            lst = app_start.dataHelper.selectData<unitFull>("N5", "pc_cntr_full_info", par, true, OnError, "unidadN4", "consultaUnidadesN4", parx);
            return lst;

        }

    }

    public class unitFull
    {
        public Int64 gkey { get; set; }
        public string id { get; set; }
        public string visitaE { get; set; }
        public string tEstado { get; set; }
        public DateTime creado { get; set; }
        public string fk { get; set; }
        public bool requirePower { get; set; }
        public bool overDim { get; set; }
        public string linea { get; set; }
        public string refIBfActual { get; set; }
        public string refOBfActual { get; set; }
        public string refDeclare { get; set; }
        public string refCarrier { get; set; }
        public string pol { get; set; }
        public string pod1 { get; set; }
        public string pod2 { get; set; }
        public string iso { get; set; }
        public string isoGrp { get; set; }
        public float peso { get; set; }
        public float tara { get; set; }
        public string ecas { get; set; }
        public string doc { get; set; }
        public string aisv { get; set; }
        public string sCGSA { get; set; }
        public string s1 { get; set; }
        public string s2 { get; set; }
        public string s3 { get; set; }
        public string s4 { get; set; }
        public string sManif { get; set; }
        public string entrega { get; set; }
        public string pesoMan { get; set; }
        public string pesoBP { get; set; }
        public string blFisico { get; set; }
        public string exportador { get; set; }
        public string proforma { get; set; }
        public string grupo { get; set; }
        public string consignatario { get; set; }
        public string mailImpo { get; set; }
        public string mailBill { get; set; }
        public string boking { get; set; }
        public string bl { get; set; }
        public string patio { get; set; }
        public bool imo { get; set; }
        public float minTemp { get; set; }
        public float maxTempo { get; set; }
        public string expID { get; set; }
       

    }
}
