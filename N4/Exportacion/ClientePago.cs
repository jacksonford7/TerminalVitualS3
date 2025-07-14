using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Configuraciones;
using Respuesta;
using AccesoDatos;
using System.Reflection;
using System.Xml.Linq;

namespace N4.Exportacion
{
  public  class ClientePago:ModuloBase
    {
        public override void OnInstanceCreate()
        {
            this.alterClase = "N4";
            base.OnInstanceCreate();
            this.Accesorio.ConfiguracionBase = "DATACON";
        }
        public string TERCEROS { get; set; }
        public string PROFORMA { get; set; }
        public string AISV { get; set; }
        public string BOOKING { get; set; }
        public string ID { get; set; }
        public static ResultadoOperacion<List<ClientePago>> ObtenerClientesPago(string referencia, string usuario)
        {
            var p = new ClientePago();
            p.actualMetodo = MethodBase.GetCurrentMethod().Name;
            string pv;
            if (!p.Accesorio.Inicializar(out pv))
            {
                return ResultadoOperacion<List<ClientePago>>.CrearFalla(pv);
            }
            p.Parametros.Clear();
            p.Parametros.Add(nameof(referencia),referencia);
            var bcon = p.Accesorio.ObtenerConfiguracion("PORTAL")?.valor;

            pv = "Referencia es nulo o vacio";
            if (string.IsNullOrEmpty(referencia))
            {
                return ResultadoOperacion<List<ClientePago>>.CrearFalla(pv);
            }
#if DEBUG
            p.LogEvent(usuario, p.actualMetodo, referencia);
#endif
            //tengo la lista de clientes desde N4
            var rp = BDOpe.ComandoSelectALista<ClientePago>(bcon, "[Bill].[clientes_pago_referencia]", p.Parametros);
            return rp.Exitoso ? ResultadoOperacion<List<ClientePago>>.CrearResultadoExitoso(rp.Resultado) : ResultadoOperacion<List<ClientePago>>.CrearFalla(rp.MensajeProblema);
        }
        public static HashSet<string> PurgarListaClientes(List<ClientePago> clientes_pago)
        {
            var l1 = clientes_pago?.Where(d => !string.IsNullOrEmpty(d.TERCEROS))?.Distinct()?.Select(f => f.TERCEROS.Trim());
            var l2 = clientes_pago?.Where(d => !string.IsNullOrEmpty(d.PROFORMA))?.Distinct()?.Select(f => f.PROFORMA.Trim());
            var l3 = clientes_pago?.Where(d => !string.IsNullOrEmpty(d.AISV))?.Distinct()?.Select(f => f.AISV.Trim());

            var r = new HashSet<string>();

            if (l1 != null)
            {
                foreach (var i in l1)
                {
                    if (!r.Contains(i))
                        r.Add(i);
                }
            }

            if (l2 != null)
            {           
                foreach (var j in l2)
                {
                    if (!r.Contains(j))
                        r.Add(j);
                }
            }

            if (l3 != null)
            {
                foreach (var k in l3)
                {
                    if (!r.Contains(k))
                        r.Add(k);
                }
            }
            return r;
        }



        public static ResultadoOperacion<List<ClientePago>> ObtenerClientesPagoPorBookings(List<string> bookings, string usuario)
        {
            var p = new ClientePago();
            p.actualMetodo = MethodBase.GetCurrentMethod().Name;
            string pv;
            if (!p.Accesorio.Inicializar(out pv))
            {
                return ResultadoOperacion<List<ClientePago>>.CrearFalla(pv);
            }

            pv = "Bookings es nulo o vacio";
            if (bookings== null || bookings.Count<=0)
            {
                return ResultadoOperacion<List<ClientePago>>.CrearFalla(pv);
            }
            pv = "Usuario es nulo o vacio";
            if (string.IsNullOrEmpty(usuario))
            {
                return ResultadoOperacion<List<ClientePago>>.CrearFalla(pv);
            }
            StringBuilder bb = new StringBuilder();
            bb.Append("<bookings>");
            foreach (var b in bookings)
            {
                bb.AppendFormat("<booking id=\"{0}\" />",b);
            }
            bb.Append("</bookings>");
            p.Parametros.Clear();
            p.Parametros.Add(nameof(bookings), bb.ToString());
            var bcon = p.Accesorio.ObtenerConfiguracion("PORTAL")?.valor;
#if DEBUG
            p.LogEvent(usuario, p.actualMetodo, bb.ToString());
#endif
            //tengo la lista de clientes desde N4
            var rp = BDOpe.ComandoSelectALista<ClientePago>(bcon, "[Bill].[clientes_pago_booking_mas]", p.Parametros);
            return rp.Exitoso ? ResultadoOperacion<List<ClientePago>>.CrearResultadoExitoso(rp.Resultado) : ResultadoOperacion<List<ClientePago>>.CrearFalla(rp.MensajeProblema);
        }
        public static ResultadoOperacion<List<ClientePago>> ObtenerClientesPagoPorDAE(List<string> daes, string usuario)
        {
            var p = new ClientePago();
            p.actualMetodo = MethodBase.GetCurrentMethod().Name;
            string pv;
            if (!p.Accesorio.Inicializar(out pv))
            {
                return ResultadoOperacion<List<ClientePago>>.CrearFalla(pv);
            }

            pv = "Bookings es nulo o vacio";
            if (daes == null || daes.Count <= 0)
            {
                return ResultadoOperacion<List<ClientePago>>.CrearFalla(pv);
            }
            pv = "Usuario es nulo o vacio";
            if (string.IsNullOrEmpty(usuario))
            {
                return ResultadoOperacion<List<ClientePago>>.CrearFalla(pv);
            }
            StringBuilder bb = new StringBuilder();
            bb.Append("<daes>");
            foreach (var b in daes)
            {
                bb.AppendFormat("<dae id=\"{0}\" />", b);
            }
            bb.Append("</daes>");
            p.Parametros.Clear();
            p.Parametros.Add(nameof(daes), bb.ToString());
            //VA A OBTENER DESDE ECUAPASS CON DAE.
            var bcon = p.Accesorio.ObtenerConfiguracion("ECUAPASS")?.valor;
#if DEBUG
            p.LogEvent(usuario, p.actualMetodo, bb.ToString());
#endif
            //tengo la lista de clientes desde N4
            var rp = BDOpe.ComandoSelectALista<ClientePago>(bcon, "[Bill].[clientes_pago_dae_mas]", p.Parametros);

            //Mismas DAES
            bcon = p.Accesorio.ObtenerConfiguracion("PORTAL")?.valor;
             var sp = BDOpe.ComandoSelectALista<ClientePago>(bcon, "[Bill].[clientes_dato_dae]", p.Parametros);
            if (sp.Exitoso && sp.Resultado.Count > 0)
            {
                foreach (var td in rp.Resultado)
                {
                    var c = sp.Resultado.Where(m=>m.ID.Equals(td.ID)).FirstOrDefault();
                    if (c != null)
                    {
                        td.BOOKING = c.BOOKING;
                    }
                }
            }
            return rp.Exitoso ? ResultadoOperacion<List<ClientePago>>.CrearResultadoExitoso(rp.Resultado) : ResultadoOperacion<List<ClientePago>>.CrearFalla(rp.MensajeProblema);
        }
        private static ResultadoOperacion<List<ClientePago>> ObtenerClientesDatoDae(List<string> daes, string usuario)
        {
            var p = new ClientePago();
            p.actualMetodo = MethodBase.GetCurrentMethod().Name;
            string pv;
            if (!p.Accesorio.Inicializar(out pv))
            {
                return ResultadoOperacion<List<ClientePago>>.CrearFalla(pv);
            }

            pv = "Bookings es nulo o vacio";
            if (daes == null || daes.Count <= 0)
            {
                return ResultadoOperacion<List<ClientePago>>.CrearFalla(pv);
            }
            pv = "Usuario es nulo o vacio";
            if (string.IsNullOrEmpty(usuario))
            {
                return ResultadoOperacion<List<ClientePago>>.CrearFalla(pv);
            }
            StringBuilder bb = new StringBuilder();
            bb.Append("<daes>");
            foreach (var b in daes)
            {
                bb.AppendFormat("<dae id=\"{0}\" />", b);
            }
            bb.Append("</daes>");
            p.Parametros.Clear();
            p.Parametros.Add(nameof(daes), bb.ToString());
            var bcon = p.Accesorio.ObtenerConfiguracion("PORTAL")?.valor;
#if DEBUG
            p.LogEvent(usuario, p.actualMetodo, bb.ToString());
#endif
            //tengo la lista de clientes desde N4
            var rp = BDOpe.ComandoSelectALista<ClientePago>(bcon, "[Bill].[clientes_dato_daes]", p.Parametros);
            return rp.Exitoso ? ResultadoOperacion<List<ClientePago>>.CrearResultadoExitoso(rp.Resultado) : ResultadoOperacion<List<ClientePago>>.CrearFalla(rp.MensajeProblema);
        }
        public static ResultadoOperacion<List<ClientePago>> ObtenerClientesPagoPorAISV(List<string> aisvs, string usuario)
        {
            var p = new ClientePago();
            p.actualMetodo = MethodBase.GetCurrentMethod().Name;
            string pv;
            if (!p.Accesorio.Inicializar(out pv))
            {
                return ResultadoOperacion<List<ClientePago>>.CrearFalla(pv);
            }

            pv = "AISVS es nulo o vacio";
            if (aisvs == null || aisvs.Count <= 0)
            {
                return ResultadoOperacion<List<ClientePago>>.CrearFalla(pv);
            }
            pv = "Usuario es nulo o vacio";
            if (string.IsNullOrEmpty(usuario))
            {
                return ResultadoOperacion<List<ClientePago>>.CrearFalla(pv);
            }
            StringBuilder bb = new StringBuilder();
            bb.Append("<aisvs>");
            foreach (var b in aisvs)
            {
                bb.AppendFormat("<aisv id=\"{0}\" />", b);
            }
            bb.Append("</aisvs>");
            p.Parametros.Clear();
            p.Parametros.Add(nameof(aisvs), bb.ToString());
            var bcon = p.Accesorio.ObtenerConfiguracion("PORTAL")?.valor;
#if DEBUG
            p.LogEvent(usuario, p.actualMetodo, bb.ToString());
#endif
            //tengo la lista de clientes desde N4
            var rp = BDOpe.ComandoSelectALista<ClientePago>(bcon, "[Bill].[clientes_pago_aisv_mas]", p.Parametros);
            return rp.Exitoso ? ResultadoOperacion<List<ClientePago>>.CrearResultadoExitoso(rp.Resultado) : ResultadoOperacion<List<ClientePago>>.CrearFalla(rp.MensajeProblema);
        }
        public static ResultadoOperacion<List<ClientePago>> ObtenerClientesPagoPorDAE(Dictionary<string,string> dae_booking, string usuario)
        {
            var p = new ClientePago();
            p.actualMetodo = MethodBase.GetCurrentMethod().Name;
            string pv;
            if (!p.Accesorio.Inicializar(out pv))
            {
                return ResultadoOperacion<List<ClientePago>>.CrearFalla(pv);
            }

            pv = "Bookings es nulo o vacio";
            if (dae_booking == null || dae_booking.Count <= 0)
            {
                return ResultadoOperacion<List<ClientePago>>.CrearFalla(pv);
            }
            pv = "Usuario es nulo o vacio";
            if (string.IsNullOrEmpty(usuario))
            {
                return ResultadoOperacion<List<ClientePago>>.CrearFalla(pv);
            }
            StringBuilder bb = new StringBuilder();
            bb.Append("<daes>");
            foreach (var b in dae_booking)
            {
                bb.AppendFormat("<dae id=\"{0}\" booking=\"{1}\" />", b.Key,b.Value);
            }
            bb.Append("</daes>");
            p.Parametros.Clear();
            p.Parametros.Add("daes", bb.ToString());
            //VA A OBTENER DESDE ECUAPASS CON DAE.
            var bcon = p.Accesorio.ObtenerConfiguracion("ECUAPASS")?.valor;
#if DEBUG
            p.LogEvent(usuario, p.actualMetodo, bb.ToString());
#endif
            //tengo la lista de clientes desde N4
            var rp = BDOpe.ComandoSelectALista<ClientePago>(bcon, "[Bill].[clientes_pago_dae_mas]", p.Parametros);
            return rp.Exitoso ? ResultadoOperacion<List<ClientePago>>.CrearResultadoExitoso(rp.Resultado) : ResultadoOperacion<List<ClientePago>>.CrearFalla(rp.MensajeProblema);
        }
        public static ResultadoOperacion<List<ClientePago>> ObtenerClientesPagoPorAISV(Dictionary<string,string> aisvs, string usuario)
        {
            var p = new ClientePago();
            p.actualMetodo = MethodBase.GetCurrentMethod().Name;
            string pv;
            if (!p.Accesorio.Inicializar(out pv))
            {
                return ResultadoOperacion<List<ClientePago>>.CrearFalla(pv);
            }

            pv = "AISVS es nulo o vacio";
            if (aisvs == null || aisvs.Count <= 0)
            {
                return ResultadoOperacion<List<ClientePago>>.CrearFalla(pv);
            }
            pv = "Usuario es nulo o vacio";
            if (string.IsNullOrEmpty(usuario))
            {
                return ResultadoOperacion<List<ClientePago>>.CrearFalla(pv);
            }
            StringBuilder bb = new StringBuilder();
            bb.Append("<aisvs>");
            foreach (var b in aisvs)
            {
                bb.AppendFormat("<aisv id=\"{0}\" booking=\"{1}\" />", b.Key, b.Value);
            }
            bb.Append("</aisvs>");
            p.Parametros.Clear();
            p.Parametros.Add(nameof(aisvs), bb.ToString());
            var bcon = p.Accesorio.ObtenerConfiguracion("PORTAL")?.valor;
#if DEBUG
            p.LogEvent(usuario, p.actualMetodo, bb.ToString());
#endif
            //tengo la lista de clientes desde N4
            var rp = BDOpe.ComandoSelectALista<ClientePago>(bcon, "[Bill].[clientes_pago_aisv_mas]", p.Parametros);
            return rp.Exitoso ? ResultadoOperacion<List<ClientePago>>.CrearResultadoExitoso(rp.Resultado) : ResultadoOperacion<List<ClientePago>>.CrearFalla(rp.MensajeProblema);
        }
        public static ResultadoOperacion<List<ClientePago>> ObtenerClientesPagoPorBooking(string booking, string usuario)
        {
            var p = new ClientePago();
            p.actualMetodo = MethodBase.GetCurrentMethod().Name;
            string pv;
            if (!p.Accesorio.Inicializar(out pv))
            {
                return ResultadoOperacion<List<ClientePago>>.CrearFalla(pv);
            }

            pv = "Booking";
            if (string.IsNullOrEmpty(booking))
            {
                return ResultadoOperacion<List<ClientePago>>.CrearFalla(pv);
            }
            pv = "Usuario";
            if (string.IsNullOrEmpty(usuario))
            {
                return ResultadoOperacion<List<ClientePago>>.CrearFalla(pv);
            }
         
            p.Parametros.Add(nameof(booking),booking);
            var bcon = p.Accesorio.ObtenerConfiguracion("PORTAL")?.valor;
#if DEBUG
            p.LogEvent(usuario, p.actualMetodo,booking);
#endif
            //tengo la lista de clientes desde N4
            var rp = BDOpe.ComandoSelectALista<ClientePago>(bcon, "[Bill].[clientes_pago_booking]", p.Parametros);
            return rp.Exitoso ? ResultadoOperacion<List<ClientePago>>.CrearResultadoExitoso(rp.Resultado) : ResultadoOperacion<List<ClientePago>>.CrearFalla(rp.MensajeProblema);
        }



        //obtiener de 1 AISV
        public static ResultadoOperacion<ClientePago> ObtenerClientePagoPorAISV(string aisv, string usuario)
        {
            var p = new ClientePago();
            p.actualMetodo = MethodBase.GetCurrentMethod().Name;
            string pv;
            if (!p.Accesorio.Inicializar(out pv))
            {
                return ResultadoOperacion<ClientePago>.CrearFalla(pv);
            }

            pv = "AISVS es nulo o vacio";
            if (string.IsNullOrEmpty(aisv))
            {
                return ResultadoOperacion<ClientePago>.CrearFalla(pv);
            }
            pv = "Usuario es nulo o vacio";
            if (string.IsNullOrEmpty(usuario))
            {
                return ResultadoOperacion<ClientePago>.CrearFalla(pv);
            }
          
            p.Parametros.Clear();
            p.Parametros.Add(nameof(aisv), aisv);
            var bcon = p.Accesorio.ObtenerConfiguracion("PORTAL")?.valor;
#if DEBUG
            p.LogEvent(usuario, p.actualMetodo, aisv);
#endif
            //tengo la lista de clientes desde N4
            var rp = BDOpe.ComandoSelectAEntidad<ClientePago>(bcon, "[Bill].[cliente_pago_aisv]", p.Parametros);
            return rp.Exitoso ? ResultadoOperacion<ClientePago>.CrearResultadoExitoso(rp.Resultado) : ResultadoOperacion<ClientePago>.CrearFalla(rp.MensajeProblema);
        }
        //OBTIENE DE 1 DAE
        public static ResultadoOperacion<ClientePago> ObtenerClientePagoPorDAE(string dae, string usuario)
        {
            var p = new ClientePago();
            p.actualMetodo = MethodBase.GetCurrentMethod().Name;
            string pv;
            if (!p.Accesorio.Inicializar(out pv))
            {
                return ResultadoOperacion<ClientePago>.CrearFalla(pv);
            }

            pv = "dae es nulo o vacio";
            if (string.IsNullOrEmpty(dae))
            {
                return ResultadoOperacion<ClientePago>.CrearFalla(pv);
            }
            pv = "Usuario es nulo o vacio";
            if (string.IsNullOrEmpty(usuario))
            {
                return ResultadoOperacion<ClientePago>.CrearFalla(pv);
            }
          
            p.Parametros.Clear();
            p.Parametros.Add(nameof(dae), dae);
            //VA A OBTENER DESDE ECUAPASS CON DAE.
            var bcon = p.Accesorio.ObtenerConfiguracion("ECUAPASS")?.valor;
#if DEBUG
            p.LogEvent(usuario, p.actualMetodo, dae);
#endif
            //tengo la lista de clientes desde N4
            var rp = BDOpe.ComandoSelectAEntidad<ClientePago>(bcon, "[Bill].[cliente_pago_dae]]", p.Parametros);
            return rp.Exitoso ? ResultadoOperacion<ClientePago>.CrearResultadoExitoso(rp.Resultado) : ResultadoOperacion<ClientePago>.CrearFalla(rp.MensajeProblema);
        }
        //oBTENER ClIENTE dAE SI FALLO


        //complementa con datos adicionales para poder obtener x dae y x aisv de modo individual
        private static ResultadoOperacion<BookingComplemento> ObtenerDataBooking(string booking, string usuario)
        {
            var p = new ClientePago();
            p.actualMetodo = MethodBase.GetCurrentMethod().Name;
            string pv;
            if (!p.Accesorio.Inicializar(out pv))
            {
                return ResultadoOperacion<BookingComplemento>.CrearFalla(pv);
            }

            pv = "Booking";
            if (string.IsNullOrEmpty(booking))
            {
                return ResultadoOperacion<BookingComplemento>.CrearFalla(pv);
            }
            pv = "Usuario";
            if (string.IsNullOrEmpty(usuario))
            {
                return ResultadoOperacion<BookingComplemento>.CrearFalla(pv);
            }

            p.Parametros.Add(nameof(booking), booking);
            var bcon = p.Accesorio.ObtenerConfiguracion("PORTAL")?.valor;
#if DEBUG
            p.LogEvent(usuario, p.actualMetodo, booking);
#endif
            //tengo la lista de clientes desde N4
            var rp = BDOpe.ComandoSelectAEntidad<BookingComplemento>(bcon, "[Bill].[clientes_data_booking]", p.Parametros);
            return rp.Exitoso ? ResultadoOperacion<BookingComplemento>.CrearResultadoExitoso(rp.Resultado) : ResultadoOperacion<BookingComplemento>.CrearFalla(rp.MensajeProblema);
        }


        public static ResultadoOperacion<Dictionary<string,string>> ObtenerClientesIndividual(string booking, string usuario)
        {
            var p = new ClientePago();
            p.actualMetodo = MethodBase.GetCurrentMethod().Name;
            string pv;
            if (!p.Accesorio.Inicializar(out pv))
            {
                return ResultadoOperacion<Dictionary<string, string>>.CrearFalla(pv);
            }

            pv = "Booking";
            if (string.IsNullOrEmpty(booking))
            {
                return ResultadoOperacion<Dictionary<string, string>>.CrearFalla(pv);
            }
            pv = "Usuario";
            if (string.IsNullOrEmpty(usuario))
            {
                return ResultadoOperacion<Dictionary<string, string>>.CrearFalla(pv);
            }
            var obdta = ObtenerDataBooking(booking, usuario);
            if (!obdta.Exitoso)
            {
                return ResultadoOperacion<Dictionary<string, string>>.CrearFalla(obdta.MensajeProblema);
            }

            var xl = new List<ClientePago>();

            //obtener individuales
          
            var ca = ObtenerClientePagoPorAISV(obdta.Resultado.aisv, usuario);
            if (!ca.Exitoso)
            {
                return ResultadoOperacion<Dictionary<string, string>>.CrearFalla(ca.MensajeProblema);
            }

            if (ca.Resultado == null)
            {
                //buscar por DAE
                var cd = ObtenerClientePagoPorDAE(obdta.Resultado.dae, usuario);
                if (cd.Exitoso && cd.Resultado != null)
                {
                    p = cd.Resultado;
                }
            }
            else
            {
                p = ca.Resultado;
            }



            var dx = new Dictionary<string, string>();
            if (string.IsNullOrEmpty(p.AISV) & string.IsNullOrEmpty(p.PROFORMA) & string.IsNullOrEmpty(p.TERCEROS))
            {
                //los 3 son vacios-->
                dx.Add("-1", "No se encontraron registros");

            }
            else
            {
                if (!string.IsNullOrEmpty(p.AISV))
                {
                    dx.Add("AISV", p.AISV);
                }
                if (!string.IsNullOrEmpty(p.TERCEROS))
                {
                    dx.Add("TERCEROS", p.TERCEROS);
                }
                if (!string.IsNullOrEmpty(p.PROFORMA))
                {
                    dx.Add("PROFORMA", p.PROFORMA);
                }
            }
            return ResultadoOperacion<Dictionary<string, string>>.CrearResultadoExitoso( dx);
        }


        public static ResultadoOperacion<string> ValidarLiquidacionPagada(long cab_id, Dictionary<Int64,string> containers, string usuario)
        {
           
            if (containers == null || containers.Count<=0)
            {
                return ResultadoOperacion<string>.CrearFalla("La lista de unidades no puede ser nula o vacía");
            }
            if (string.IsNullOrEmpty(usuario))
            {
                return ResultadoOperacion<string>.CrearFalla("El usuario no puede ser nulo");
            }
            var p = new ClientePago();
            p.actualMetodo = MethodBase.GetCurrentMethod().Name;
            string pv;
            if (!p.Accesorio.Inicializar(out pv))
            {
                return ResultadoOperacion<string>.CrearFalla(pv);
            }

            StringBuilder bb = new StringBuilder();
            bb.Append("<containers>");
            foreach (var c in containers)
            {
                bb.AppendFormat("<container gkey=\"{0}\" proforma=\"{1}\" header=\"{2}\" />", c.Key, c.Value, cab_id);
            }
            bb.Append("</containers>");
            var bcon = p.Accesorio.ObtenerConfiguracion("ecuapass")?.valor;
            p.Parametros.Clear();
            p.Parametros.Add(nameof(containers), bb.ToString());
            var rsql = BDOpe.ComandoSelectEscalarRef<string>(bcon, "Select [Bill].[unit_proforma_pagada](@containers);", p.Parametros);
            if (!rsql.Exitoso)
            {
                return ResultadoOperacion<string>.CrearFalla(rsql.MensajeProblema);
            }
            if (!XmlTools.IsXml(rsql.Resultado))
            {
                p.LogError<ApplicationException>(new ApplicationException(rsql.Resultado), p.actualMetodo, usuario);
                return ResultadoOperacion<string>.CrearFalla("El contenido de la variable respuesta no corresponde a un documento XML");
            }
           return ResultadoOperacion<string>.CrearResultadoExitoso(rsql.Resultado);
        }


    }

    public class BookingComplemento
    {
        public string aisv { get; set; }
        public string dae { get; set; }
        public string booking { get; set; }
    }
}
