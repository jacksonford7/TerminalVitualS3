using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

using System.Windows.Forms;
using MonitorVacios.Entidades;
using System.Web.Services;
using ConectorN4;
using System.Xml;
using System.Web;
using System.IO;

using System.ComponentModel;
using System.Data;
using System.Drawing;


namespace MonitorVacios.Entidades
{
    public class EdoTask
    {
        private static Entidades.Contenedor objConteendor = new Entidades.Contenedor();
        private static string v_mensaje = string.Empty;
        private static Entidades.ValidaEDO EDO;
        private static Int64 nTotal = 0;
        private static bool procesado = false;
        //private static bool 
        public static Action<IProgress<string>, int, string> SeleccionarMetodoPorID(string id)
        {
            Action<IProgress<string>, int, string> result;
            switch (id)
            {
                case "001":
                    result = Tarea001; // generar edo y autorizacion N$
                    break;
                case "002":
                    result = Tarea002; //Envio Mail
                    break;
               
                default:
                    result = null;
                    break;
            }

            return result;
        }

        public static void Tarea001(IProgress<string> repo, int maxregs, string code)
        {
            try
            {

                if (!procesado)
                {
                    List<Contenedor> Listado = Contenedor.CONSULTA_CONTENEDORES(maxregs, out v_mensaje);
                    if (!string.IsNullOrEmpty(v_mensaje))
                    {
                        // repo.Report(string.Format("{0} Error al leer contenedores pendientes,", DateTime.Now.ToString("yyyy/MM/dd HH:mm")));
                    }
                    else
                    {
                        string Mensaje_Resultado = string.Empty;
                        foreach (var Det in Listado)
                        {
                            procesado = true;

                            repo.Report(string.Format("{0} Procesando transacción: {1}, contenedor: {2}", DateTime.Now.ToString("yyyy/MM/dd HH:mm"), Det.ID, Det.CONTENEDOR));

                            bool Existe = ValidaEDO.Existe_EDO(Det.GKEY, Det.LINEA_NAVIERA, out Mensaje_Resultado);
                            if (Existe)
                            {
                                if (!string.IsNullOrEmpty(Mensaje_Resultado))
                                {
                                    repo.Report(string.Format("{0} Error al validar transacción: {1}, contenedor: {2} - Error: {3}", DateTime.Now.ToString("yyyy/MM/dd HH:mm"), Det.ID, Det.CONTENEDOR, Mensaje_Resultado));
                                }
                                var Ok = Procesa_EDO_Existente(repo, Det.LINEA_NAVIERA, Det.CONTENEDOR, Det.REFERENCIA, Det.ID, Det.SECUENCIA, Det.GRUPO, Det.AUTORIZACION, Det.REF_FINAL, Det.GKEY);
                                if (Ok)
                                {
                                    repo.Report(string.Format("{0}  transacción: {1}, contenedor: {2} OK.", DateTime.Now.ToString("yyyy/MM/dd HH:mm"), Det.ID, Det.CONTENEDOR));
                                }
                                else
                                {
                                    repo.Report(string.Format("{0}  transacción: {1}, contenedor: {2} ERROR.", DateTime.Now.ToString("yyyy/MM/dd HH:mm"), Det.ID, Det.CONTENEDOR));

                                }

                            }
                            else
                            {
                                var Ok = Genera_EDO(repo, Det.LINEA_NAVIERA, Det.CONTENEDOR, Det.REFERENCIA, Det.ID, Det.SECUENCIA, Det.GRUPO, Det.AUTORIZACION, Det.REF_FINAL, Det.GKEY);
                                if (Ok)
                                {
                                    repo.Report(string.Format("{0} transacción: {1}, contenedor: {2} OK.", DateTime.Now.ToString("yyyy/MM/dd HH:mm"), Det.ID, Det.CONTENEDOR));
                                }
                                else
                                {
                                    repo.Report(string.Format("{0} transacción: {1}, contenedor: {2} ERROR.", DateTime.Now.ToString("yyyy/MM/dd HH:mm"), Det.ID, Det.CONTENEDOR));

                                }
                            }
                            procesado = false;

                        }
                    }
                }
               
            }
            catch (Exception ex)
            {

                repo.Report(string.Format("{0} Monitoreo con error: {1}", DateTime.Now.ToString("yyyy/MM/dd HH:mm"), ex.Message));
            }
            finally
            {
                procesado = false;
            }

        }

        private static bool Procesa_EDO_Existente(IProgress<string> repo, string Linea_Naviera, string Contenedor, string Referencia, Int64 Id, Int64 Secuencia, string Grupo, string Autorizacion, string Referencia_Final, Int64 GKEY)
        {
            bool Ok = true;

            try
            {
                //if (procesado)
                //{
                    string xml_n4 = null;

                    string Error = string.Empty;

                    wsN4 Ws = new wsN4();
                    string XmlDoc = xml_n4;
                    string Mensaje = string.Empty;


                    //SI EXISTE ACTUALIZO Y PONGO PROCESADO: OK 
                    objConteendor = new Contenedor();
                    objConteendor.ID = Id;
                    objConteendor.SECUENCIA = Secuencia;
                    objConteendor.MENSAJE = "OK";
                    objConteendor.PROCESO = "P";
                    objConteendor.ESTADO_PROCESO = "OK";
                    objConteendor.Marcar(out v_mensaje);
                    if (v_mensaje != string.Empty)
                    {
                        //repo.Report(string.Format("{0} Error al marcar contenedor: {1} / {2}", DateTime.Now.ToString("yyyy/MM/dd HH:mm"), Contenedor, v_mensaje.ToString()));
                    }

                    //actualiza autorizacion por validacion de linea naviera
                    objConteendor = new Contenedor();
                    objConteendor.Tipo = 2;
                    objConteendor.LINEA_NAVIERA = Linea_Naviera;
                    if (objConteendor.Valida_Linea(out v_mensaje))
                    {
                        string Xml_Booking = string.Empty;
                        Xml_Booking = Genera_Xml_Autorizacion_Booking(Autorizacion, Contenedor, Referencia, Referencia_Final, Linea_Naviera);
                        Ws = new wsN4();
                        XmlDoc = Xml_Booking;
                        Mensaje = string.Empty;
                        var ResultadoBk = Ws.CallBasicService("ICT/ECU/GYE/CGSA", XmlDoc, ref Mensaje);
                        if (!Mensaje.ToUpper().Contains("OK"))
                        {
                            Error = string.Format("Error al actualizar en N4 EDO por Booking, Contenedor: {0}, Referencia {1}.{2} {3} {4} ", Contenedor, Referencia, System.Environment.NewLine, ResultadoBk, Mensaje);
                            objConteendor = new Contenedor();
                            objConteendor.ID = Id;
                            objConteendor.SECUENCIA = Secuencia;
                            objConteendor.MENSAJE = Error;
                            objConteendor.PROCESO = "P";
                            objConteendor.ESTADO_PROCESO = "OK";
                            objConteendor.Marcar(out v_mensaje);
                            if (v_mensaje != string.Empty)
                            {
                               // repo.Report(string.Format("{0} Error al marcar contenedor: {1} / {2}", DateTime.Now.ToString("yyyy/MM/dd HH:mm"), Contenedor, v_mensaje.ToString()));
                            }

                            Ok = false;
                            return Ok;
                        }
                    }
                    else
                    {
                        string Xml_Autorizacion = string.Empty;
                        Xml_Autorizacion = Ge_Xml_Autorizacion(Autorizacion, Contenedor, Referencia, Referencia_Final);
                        Ws = new wsN4();
                        XmlDoc = Xml_Autorizacion;
                        Mensaje = string.Empty;
                        var ResultadoAut = Ws.CallBasicService("ICT/ECU/GYE/CGSA", XmlDoc, ref Mensaje);
                        if (!Mensaje.ToUpper().Contains("OK"))
                        {
                            Error = string.Format("Error al actualizar en N4 EDO por Autorización, Contenedor: {0}, Referencia {1}.{2} {3} {4} ", Contenedor, Referencia, System.Environment.NewLine, ResultadoAut, Mensaje);
                            objConteendor = new Contenedor();
                            objConteendor.ID = Id;
                            objConteendor.SECUENCIA = Secuencia;
                            objConteendor.MENSAJE = Error;
                            objConteendor.PROCESO = "P";
                            objConteendor.ESTADO_PROCESO = "OK";
                            objConteendor.Marcar(out v_mensaje);
                            if (v_mensaje != string.Empty)
                            {
                                //repo.Report(string.Format("{0} Error al marcar contenedor: {1} / {2}", DateTime.Now.ToString("yyyy/MM/dd HH:mm"), Contenedor, v_mensaje.ToString()));
                            }

                            Ok = false;
                            return Ok;

                        }
                    }//fin por linea
                //}
                
            }
            catch (Exception ex)
            {
                Ok = false;
                repo.Report(string.Format("{0} Monitoreo con error: {1}", DateTime.Now.ToString("yyyy/MM/dd HH:mm"), ex.Message));
            }
            finally
            {

                //procesado = false;
            }

            return Ok;

        }


        //private static  bool Genera_EDO(string Linea_Naviera, string Contenedor, string Referencia, Int64 Id, Int64 Secuencia, string Grupo, string Autorizacion, string Referencia_Final, Int64 GKEY, out string Mensaje_Error)
        //{
        //    bool Ok = true;

        //    try
        //    {

        //        string xml_n4 = null;
        //        Mensaje_Error = string.Empty;
        //        string Error = string.Empty;

        //        if (Genera_Xml(Linea_Naviera, Contenedor, Referencia).ToString() != "error")
        //        {
        //            xml_n4 = Genera_Xml(Linea_Naviera, Contenedor, Referencia);
        //            Ok = true;
        //        }
        //        else
        //        {
        //            Error = string.Format("Error al generar xml, para EDO en N4, Contenedor: {0}, Referencia {1} ", Contenedor, Referencia);
        //            objConteendor = new Contenedor();
        //            objConteendor.ID = Id;
        //            objConteendor.SECUENCIA = Secuencia;
        //            objConteendor.MENSAJE = Error;
        //            objConteendor.PROCESO = "E";
        //            objConteendor.ESTADO_PROCESO = "ERROR";
        //            objConteendor.Marcar(out v_mensaje);
        //            if (v_mensaje != string.Empty)
        //            {
        //                Mensaje_Error = string.Format("{0} Error al marcar contenedor: {1} / {2}", DateTime.Now.ToString("yyyy/MM/dd HH:mm"), Contenedor, v_mensaje.ToString());
        //                Ok = false;
        //                return Ok;
        //            }
        //        }

        //        wsN4 Ws = new wsN4();
        //        string XmlDoc = xml_n4;
        //        string Mensaje = string.Empty;
        //        //Int64 nExiste = 0;

        //        if (Grupo.Equals("N"))
        //        {

        //                //se transmite el primer xml
        //                var Resultado = Ws.CallBasicService("ICT/ECU/GYE/CGSA", XmlDoc, ref Mensaje);
        //                //si da error
        //                if (!Mensaje.ToUpper().Contains("OK") & !Mensaje.Contains("GROOVY_RESULT Severity:INFO  Detail: Successfully processed the request"))
        //                {
        //                    Error = string.Format("Error al actualizar en N4, Contenedor: {0}, Referencia {1}.{2} {3} {4} ", Contenedor, Referencia, System.Environment.NewLine, Resultado, Mensaje);
        //                    objConteendor = new Contenedor();
        //                    objConteendor.ID = Id;
        //                    objConteendor.SECUENCIA = Secuencia;
        //                    objConteendor.MENSAJE = Error;
        //                    objConteendor.PROCESO = "E";
        //                    objConteendor.ESTADO_PROCESO = "ERROR";
        //                    objConteendor.Marcar(out v_mensaje);
        //                    if (v_mensaje != string.Empty)
        //                    {
        //                        Mensaje_Error = string.Format("{0} Error al marcar contenedor: {1} / {2}", DateTime.Now.ToString("yyyy/MM/dd HH:mm"), Contenedor, v_mensaje.ToString());
        //                    }

        //                    Ok = false;
        //                    return Ok;
        //                }
        //                else
        //                {
        //                    //actualiza autorizacion por validacion de linea naviera
        //                    objConteendor = new Contenedor();
        //                    objConteendor.Tipo = 2;
        //                    objConteendor.LINEA_NAVIERA = Linea_Naviera;
        //                    if (objConteendor.Valida_Linea(out v_mensaje))
        //                    {
        //                        string Xml_Booking = string.Empty;
        //                        Xml_Booking = Genera_Xml_Autorizacion_Booking(Autorizacion, Contenedor, Referencia, Referencia_Final, Linea_Naviera);
        //                        Ws = new wsN4();
        //                        XmlDoc = Xml_Booking;
        //                        Mensaje = string.Empty;
        //                        var ResultadoBk = Ws.CallBasicService("ICT/ECU/GYE/CGSA", XmlDoc, ref Mensaje);
        //                        if (!Mensaje.ToUpper().Contains("OK"))
        //                        {
        //                            Error = string.Format("Error al actualizar en N4 EDO por Booking, Contenedor: {0}, Referencia {1}.{2} {3} {4} ", Contenedor, Referencia, System.Environment.NewLine, ResultadoBk, Mensaje);
        //                            objConteendor = new Contenedor();
        //                            objConteendor.ID = Id;
        //                            objConteendor.SECUENCIA = Secuencia;
        //                            objConteendor.MENSAJE = Error;
        //                            objConteendor.PROCESO = "E";
        //                            objConteendor.ESTADO_PROCESO = "ERROR";
        //                            objConteendor.Marcar(out v_mensaje);
        //                            if (v_mensaje != string.Empty)
        //                            {
        //                                Mensaje_Error = string.Format("{0} Error al marcar contenedor: {1} / {2}", DateTime.Now.ToString("yyyy/MM/dd HH:mm"), Contenedor, v_mensaje.ToString());
        //                            }

        //                            Ok = false;
        //                            return Ok;
        //                        }
        //                    }
        //                    else
        //                    {
        //                        string Xml_Autorizacion = string.Empty;
        //                        Xml_Autorizacion = Ge_Xml_Autorizacion(Autorizacion, Contenedor, Referencia, Referencia_Final);
        //                        Ws = new wsN4();
        //                        XmlDoc = Xml_Autorizacion;
        //                        Mensaje = string.Empty;
        //                        var ResultadoAut = Ws.CallBasicService("ICT/ECU/GYE/CGSA", XmlDoc, ref Mensaje);
        //                        if (!Mensaje.ToUpper().Contains("OK"))
        //                        {
        //                            Error = string.Format("Error al actualizar en N4 EDO por Autorización, Contenedor: {0}, Referencia {1}.{2} {3} {4} ", Contenedor, Referencia, System.Environment.NewLine, ResultadoAut, Mensaje);
        //                            objConteendor = new Contenedor();
        //                            objConteendor.ID = Id;
        //                            objConteendor.SECUENCIA = Secuencia;
        //                            objConteendor.MENSAJE = Error;
        //                            objConteendor.PROCESO = "E";
        //                            objConteendor.ESTADO_PROCESO = "ERROR";
        //                            objConteendor.Marcar(out v_mensaje);
        //                            if (v_mensaje != string.Empty)
        //                            {
        //                                Mensaje_Error = string.Format("{0} Error al marcar contenedor: {1} / {2}", DateTime.Now.ToString("yyyy/MM/dd HH:mm"), Contenedor, v_mensaje.ToString());
        //                            }

        //                            Ok = false;
        //                            return Ok;

        //                        }
        //                    }//fin por linea

        //                    //marcar como ok
        //                    objConteendor = new Contenedor();
        //                    objConteendor.ID = Id;
        //                    objConteendor.SECUENCIA = Secuencia;
        //                    objConteendor.MENSAJE = "OK";
        //                    objConteendor.PROCESO = "P";
        //                    objConteendor.ESTADO_PROCESO = "OK";
        //                    objConteendor.Marcar(out v_mensaje);
        //                    if (v_mensaje != string.Empty)
        //                    {
        //                        Mensaje_Error = string.Format("{0} Error al marcar contenedor: {1} / {2}", DateTime.Now.ToString("yyyy/MM/dd HH:mm"), Contenedor, v_mensaje.ToString());
        //                    }


        //                }//genero edo (ok)
                    
                   
        //        }
        //        else
        //        {

        //            //actualiza autorizacion por validacion de linea naviera
        //            objConteendor = new Contenedor();
        //            objConteendor.Tipo = 2;
        //            objConteendor.LINEA_NAVIERA = Linea_Naviera;
        //            if (objConteendor.Valida_Linea(out v_mensaje))
        //            {
        //                string Xml_Booking = string.Empty;
        //                Xml_Booking = Genera_Xml_Autorizacion_Booking(Autorizacion, Contenedor, Referencia, Referencia_Final, Linea_Naviera);
        //                Ws = new wsN4();
        //                XmlDoc = Xml_Booking;
        //                Mensaje = string.Empty;
        //                var ResultadoBk = Ws.CallBasicService("ICT/ECU/GYE/CGSA", XmlDoc, ref Mensaje);
        //                if (!Mensaje.ToUpper().Contains("OK"))
        //                {
        //                    Error = string.Format("Error al actualizar en N4 EDO por Booking, Contenedor: {0}, Referencia {1}.{2} {3} {4} ", Contenedor, Referencia, System.Environment.NewLine, ResultadoBk, Mensaje);
        //                    objConteendor = new Contenedor();
        //                    objConteendor.ID = Id;
        //                    objConteendor.SECUENCIA = Secuencia;
        //                    objConteendor.MENSAJE = Error;
        //                    objConteendor.PROCESO = "E";
        //                    objConteendor.ESTADO_PROCESO = "ERROR";
        //                    objConteendor.Marcar(out v_mensaje);
        //                    if (v_mensaje != string.Empty)
        //                    {
        //                        Mensaje_Error = string.Format("{0} Error al marcar contenedor: {1} / {2}", DateTime.Now.ToString("yyyy/MM/dd HH:mm"), Contenedor, v_mensaje.ToString());
        //                    }

        //                    Ok = false;
        //                    return Ok;
        //                }
        //            }
        //            else
        //            {
        //                string Xml_Autorizacion = string.Empty;
        //                Xml_Autorizacion = Ge_Xml_Autorizacion(Autorizacion, Contenedor, Referencia, Referencia_Final);
        //                Ws = new wsN4();
        //                XmlDoc = Xml_Autorizacion;
        //                Mensaje = string.Empty;
        //                var ResultadoAut = Ws.CallBasicService("ICT/ECU/GYE/CGSA", XmlDoc, ref Mensaje);
        //                if (!Mensaje.ToUpper().Contains("OK"))
        //                {
        //                    Error = string.Format("Error al actualizar en N4 EDO por Autorización, Contenedor: {0}, Referencia {1}.{2} {3} {4} ", Contenedor, Referencia, System.Environment.NewLine, ResultadoAut, Mensaje);
        //                    objConteendor = new Contenedor();
        //                    objConteendor.ID = Id;
        //                    objConteendor.SECUENCIA = Secuencia;
        //                    objConteendor.MENSAJE = Error;
        //                    objConteendor.PROCESO = "E";
        //                    objConteendor.ESTADO_PROCESO = "ERROR";
        //                    objConteendor.Marcar(out v_mensaje);
        //                    if (v_mensaje != string.Empty)
        //                    {
        //                        Mensaje_Error = string.Format("{0} Error al marcar contenedor: {1} / {2}", DateTime.Now.ToString("yyyy/MM/dd HH:mm"), Contenedor, v_mensaje.ToString());
        //                    }

        //                    Ok = false;
        //                    return Ok;

        //                }
        //            }//fin por linea

        //            //marcar como ok
        //            objConteendor = new Contenedor();
        //            objConteendor.ID = Id;
        //            objConteendor.SECUENCIA = Secuencia;
        //            objConteendor.MENSAJE = "OK";
        //            objConteendor.PROCESO = "P";
        //            objConteendor.ESTADO_PROCESO = "OK";
        //            objConteendor.Marcar(out v_mensaje);
        //            if (v_mensaje != string.Empty)
        //            {
        //                Mensaje_Error = string.Format("{0} Error al marcar contenedor: {1} / {2}", DateTime.Now.ToString("yyyy/MM/dd HH:mm"), Contenedor, v_mensaje.ToString());
        //            }

        //            Ok = true;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Ok = false;
        //        Mensaje_Error = string.Format("{0} Monitoreo con error: {1}", DateTime.Now.ToString("yyyy/MM/dd HH:mm"), ex.Message);
        //    }

        //    return Ok;

        //}

        private static string Genera_Xml(string Linea, string Contenedor, string referencia)
        {
            try
            {
                string vref = referencia;
                if (vref == "GEN_TRUCK")
                {
                    vref = "CGSAEDOSTORAGE";
                }
                else if (vref == "GEN_DMG")
                {
                    vref = "CGSAEDODMG";
                }
                else
                {
                    vref = "CGSAEDO";
                }

                XDocument chXML = new XDocument();

                chXML = new XDocument(new XDeclaration("1.0", "utf-8", "yes"),
                new XElement("groovy",
                new XAttribute("class-location", "database"),
                new XAttribute("class-name", vref.Trim().ToUpper()),
                new XElement("parameters",
                     new XElement("parameter",
                        new XAttribute("id", "orderLine"),
                        new XAttribute("value", Linea.Trim().ToUpper())),
                     new XElement("parameter",
                        new XAttribute("id", "container"),
                        new XAttribute("value", Contenedor.Trim().ToUpper()))
                        )));


                return chXML.ToString();
            }
            catch (Exception)
            {
                return "error";
            }
        }

        private static string Genera_Xml_Autorizacion_Booking(string Autorizacion, string Contenedor, string Referencia, string Referencia_Final, string Booking)
        {
            var vmode = Referencia;
            vmode = "VESSEL";
            if (Referencia == "GEN_DMG")
            {
                Referencia = Referencia_Final;
            }
            try
            {
                XDocument ndXML = new XDocument(new XDeclaration("1.0", "utf-8", "yes"),
                new XElement("icu",
                    new XElement("units",
                        new XElement("unit-identity", new XAttribute("id", Contenedor.Trim().ToUpper()), new XAttribute("type", "CONTAINERIZED"),
                            new XElement("carrier", new XAttribute("direction", "IB"), new XAttribute("qualifier", "DECLARED"),
                                                      new XAttribute("mode", vmode), new XAttribute("id", Referencia.Trim().ToUpper())))),
                    new XElement("properties",
                        new XElement("property", new XAttribute("tag", "UnitFlexString01"), new XAttribute("value", Autorizacion)
                        ),
                        new XElement("property", new XAttribute("tag", "UnitFlexString05"), new XAttribute("value", Booking)
                        ))
                        ));

                return ndXML.ToString();
            }
            catch (Exception)
            {
                return "error";
            }
        }

        private static string Ge_Xml_Autorizacion(string Autorizacion, string Contenedor, string Referencia, string Referencia_Final)
        {
            var vmode = Referencia;
            if (Referencia == "GEN_TRUCK")
            {
                vmode = "TRUCK";
            }
            else
            {
                vmode = "VESSEL";
                if (Referencia == "GEN_DMG")
                {
                    Referencia = Referencia_Final;
                }
            }
            try
            {
                XDocument ndXML = new XDocument(new XDeclaration("1.0", "utf-8", "yes"),
                new XElement("icu",
                    new XElement("units",
                        new XElement("unit-identity", new XAttribute("id", Contenedor.Trim().ToUpper()), new XAttribute("type", "CONTAINERIZED"),
                            new XElement("carrier", new XAttribute("direction", "IB"), new XAttribute("qualifier", "DECLARED"),
                                                      new XAttribute("mode", vmode), new XAttribute("id", Referencia.Trim().ToUpper())))),
                    new XElement("properties",
                        new XElement("property", new XAttribute("tag", "UnitFlexString01"), new XAttribute("value", Autorizacion)
                        ))));

                return ndXML.ToString();
            }
            catch (Exception)
            {
                return "error";
            }
        }


        public static void Tarea002(IProgress<string> repo, int maxregs, string code)
        {

        }

        private static bool Genera_EDO(IProgress<string> repo, string Linea_Naviera, string Contenedor, string Referencia, Int64 Id, Int64 Secuencia, string Grupo, string Autorizacion, string Referencia_Final, Int64 GKEY)
        {
            bool Ok = true;

            try
            {
                //if (procesado)
                //{
                    string xml_n4 = null;

                    string Error = string.Empty;

                    if (Genera_Xml(Linea_Naviera, Contenedor, Referencia).ToString() != "error")
                    {
                        xml_n4 = Genera_Xml(Linea_Naviera, Contenedor, Referencia);
                        Ok = true;
                    }
                    else
                    {
                        Error = string.Format("Error al generar xml, para EDO en N4, Contenedor: {0}, Referencia {1} ", Contenedor, Referencia);
                        objConteendor = new Contenedor();
                        objConteendor.ID = Id;
                        objConteendor.SECUENCIA = Secuencia;
                        objConteendor.MENSAJE = Error;
                        objConteendor.PROCESO = "P";
                        objConteendor.ESTADO_PROCESO = "OK";
                        objConteendor.Marcar(out v_mensaje);
                        if (v_mensaje != string.Empty)
                        {
                            //repo.Report(string.Format("{0} Error al marcar contenedor: {1} / {2}", DateTime.Now.ToString("yyyy/MM/dd HH:mm"), Contenedor, v_mensaje.ToString()));
                            Ok = false;
                            return Ok;
                        }
                    }

                    wsN4 Ws = new wsN4();
                    string XmlDoc = xml_n4;
                    string Mensaje = string.Empty;
                    //Int64 nExiste = 0;

                    if (Grupo.Equals("N"))
                    {

                        //se transmite el primer xml
                        var Resultado = Ws.CallBasicService("ICT/ECU/GYE/CGSA", XmlDoc, ref Mensaje);
                        //si da error
                        if (!Mensaje.ToUpper().Contains("OK") & !Mensaje.Contains("GROOVY_RESULT Severity:INFO  Detail: Successfully processed the request"))
                        {
                            Error = string.Format("Error al actualizar en N4, Contenedor: {0}, Referencia {1}.{2} {3} {4} ", Contenedor, Referencia, System.Environment.NewLine, Resultado, Mensaje);
                            objConteendor = new Contenedor();
                            objConteendor.ID = Id;
                            objConteendor.SECUENCIA = Secuencia;
                            objConteendor.MENSAJE = Error;
                            objConteendor.PROCESO = "P";
                            objConteendor.ESTADO_PROCESO = "OK";
                            objConteendor.Marcar(out v_mensaje);
                            if (v_mensaje != string.Empty)
                            {
                               // repo.Report(string.Format("{0} Error al marcar contenedor: {1} / {2}", DateTime.Now.ToString("yyyy/MM/dd HH:mm"), Contenedor, v_mensaje.ToString()));
                            }

                            Ok = false;
                            return Ok;
                        }
                        else
                        {
                            //actualiza autorizacion por validacion de linea naviera
                            objConteendor = new Contenedor();
                            objConteendor.Tipo = 2;
                            objConteendor.LINEA_NAVIERA = Linea_Naviera;
                            if (objConteendor.Valida_Linea(out v_mensaje))
                            {
                                string Xml_Booking = string.Empty;
                                Xml_Booking = Genera_Xml_Autorizacion_Booking(Autorizacion, Contenedor, Referencia, Referencia_Final, Linea_Naviera);
                                Ws = new wsN4();
                                XmlDoc = Xml_Booking;
                                Mensaje = string.Empty;
                                var ResultadoBk = Ws.CallBasicService("ICT/ECU/GYE/CGSA", XmlDoc, ref Mensaje);
                                if (!Mensaje.ToUpper().Contains("OK"))
                                {
                                    Error = string.Format("Error al actualizar en N4 EDO por Booking, Contenedor: {0}, Referencia {1}.{2} {3} {4} ", Contenedor, Referencia, System.Environment.NewLine, ResultadoBk, Mensaje);
                                    objConteendor = new Contenedor();
                                    objConteendor.ID = Id;
                                    objConteendor.SECUENCIA = Secuencia;
                                    objConteendor.MENSAJE = Error;
                                    objConteendor.PROCESO = "P";
                                    objConteendor.ESTADO_PROCESO = "OK";
                                    objConteendor.Marcar(out v_mensaje);
                                    if (v_mensaje != string.Empty)
                                    {
                                        //repo.Report(string.Format("{0} Error al marcar contenedor: {1} / {2}", DateTime.Now.ToString("yyyy/MM/dd HH:mm"), Contenedor, v_mensaje.ToString()));
                                    }

                                    Ok = false;
                                    return Ok;
                                }
                            }
                            else
                            {
                                string Xml_Autorizacion = string.Empty;
                                Xml_Autorizacion = Ge_Xml_Autorizacion(Autorizacion, Contenedor, Referencia, Referencia_Final);
                                Ws = new wsN4();
                                XmlDoc = Xml_Autorizacion;
                                Mensaje = string.Empty;
                                var ResultadoAut = Ws.CallBasicService("ICT/ECU/GYE/CGSA", XmlDoc, ref Mensaje);
                                if (!Mensaje.ToUpper().Contains("OK"))
                                {
                                    Error = string.Format("Error al actualizar en N4 EDO por Autorización, Contenedor: {0}, Referencia {1}.{2} {3} {4} ", Contenedor, Referencia, System.Environment.NewLine, ResultadoAut, Mensaje);
                                    objConteendor = new Contenedor();
                                    objConteendor.ID = Id;
                                    objConteendor.SECUENCIA = Secuencia;
                                    objConteendor.MENSAJE = Error;
                                    objConteendor.PROCESO = "P";
                                    objConteendor.ESTADO_PROCESO = "OK";
                                    objConteendor.Marcar(out v_mensaje);
                                    if (v_mensaje != string.Empty)
                                    {
                                       // repo.Report(string.Format("{0} Error al marcar contenedor: {1} / {2}", DateTime.Now.ToString("yyyy/MM/dd HH:mm"), Contenedor, v_mensaje.ToString()));
                                    }

                                    Ok = false;
                                    return Ok;

                                }
                            }//fin por linea

                            //marcar como ok
                            objConteendor = new Contenedor();
                            objConteendor.ID = Id;
                            objConteendor.SECUENCIA = Secuencia;
                            objConteendor.MENSAJE = "OK";
                            objConteendor.PROCESO = "P";
                            objConteendor.ESTADO_PROCESO = "OK";
                            objConteendor.Marcar(out v_mensaje);
                            if (v_mensaje != string.Empty)
                            {
                                //repo.Report(string.Format("{0} Error al marcar contenedor: {1} / {2}", DateTime.Now.ToString("yyyy/MM/dd HH:mm"), Contenedor, v_mensaje.ToString()));
                            }


                        }//genero edo (ok)


                    }
                    else
                    {

                        //nuevo 03/09/2020
                        //se transmite el primer xml
                       
                        //fin nuevo 03/09/2020

                        //actualiza autorizacion por validacion de linea naviera
                        objConteendor = new Contenedor();
                        objConteendor.Tipo = 2;
                        objConteendor.LINEA_NAVIERA = Linea_Naviera;
                        if (objConteendor.Valida_Linea(out v_mensaje))
                        {
                            string Xml_Booking = string.Empty;
                            Xml_Booking = Genera_Xml_Autorizacion_Booking(Autorizacion, Contenedor, Referencia, Referencia_Final, Linea_Naviera);
                            Ws = new wsN4();
                            XmlDoc = Xml_Booking;
                            Mensaje = string.Empty;
                            var ResultadoBk = Ws.CallBasicService("ICT/ECU/GYE/CGSA", XmlDoc, ref Mensaje);
                            if (!Mensaje.ToUpper().Contains("OK"))
                            {
                                Error = string.Format("Error al actualizar en N4 EDO por Booking, Contenedor: {0}, Referencia {1}.{2} {3} {4} ", Contenedor, Referencia, System.Environment.NewLine, ResultadoBk, Mensaje);
                                objConteendor = new Contenedor();
                                objConteendor.ID = Id;
                                objConteendor.SECUENCIA = Secuencia;
                                objConteendor.MENSAJE = Error;
                                objConteendor.PROCESO = "P";
                                objConteendor.ESTADO_PROCESO = "OK";
                                objConteendor.Marcar(out v_mensaje);
                                if (v_mensaje != string.Empty)
                                {
                                   // repo.Report(string.Format("{0} Error al marcar contenedor: {1} / {2}", DateTime.Now.ToString("yyyy/MM/dd HH:mm"), Contenedor, v_mensaje.ToString()));
                                }

                                Ok = false;
                                return Ok;
                            }
                        }
                        else
                        {
                            string Xml_Autorizacion = string.Empty;
                            Xml_Autorizacion = Ge_Xml_Autorizacion(Autorizacion, Contenedor, Referencia, Referencia_Final);
                            Ws = new wsN4();
                            XmlDoc = Xml_Autorizacion;
                            Mensaje = string.Empty;
                            var ResultadoAut = Ws.CallBasicService("ICT/ECU/GYE/CGSA", XmlDoc, ref Mensaje);
                            if (!Mensaje.ToUpper().Contains("OK"))
                            {
                                Error = string.Format("Error al actualizar en N4 EDO por Autorización, Contenedor: {0}, Referencia {1}.{2} {3} {4} ", Contenedor, Referencia, System.Environment.NewLine, ResultadoAut, Mensaje);
                                objConteendor = new Contenedor();
                                objConteendor.ID = Id;
                                objConteendor.SECUENCIA = Secuencia;
                                objConteendor.MENSAJE = Error;
                                objConteendor.PROCESO = "P";
                                objConteendor.ESTADO_PROCESO = "OK";
                                objConteendor.Marcar(out v_mensaje);
                                if (v_mensaje != string.Empty)
                                {
                                    //repo.Report(string.Format("{0} Error al marcar contenedor: {1} / {2}", DateTime.Now.ToString("yyyy/MM/dd HH:mm"), Contenedor, v_mensaje.ToString()));
                                }

                                Ok = false;
                                return Ok;

                            }
                        }//fin por linea

                        //marcar como ok
                        objConteendor = new Contenedor();
                        objConteendor.ID = Id;
                        objConteendor.SECUENCIA = Secuencia;
                        objConteendor.MENSAJE = "OK";
                        objConteendor.PROCESO = "P";
                        objConteendor.ESTADO_PROCESO = "OK";
                        objConteendor.Marcar(out v_mensaje);
                        if (v_mensaje != string.Empty)
                        {
                           // repo.Report(string.Format("{0} Error al marcar contenedor: {1} / {2}", DateTime.Now.ToString("yyyy/MM/dd HH:mm"), Contenedor, v_mensaje.ToString()));
                        }

                        Ok = true;
                    }
                //}
               
            }
            catch (Exception ex)
            {
                Ok = false;
               
                repo.Report(string.Format("{0} Monitoreo con error: {1}", DateTime.Now.ToString("yyyy/MM/dd HH:mm"), ex.Message));
            }
            finally
            {

                //procesado = false;
            }
            return Ok;

        }

    }
}
