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

namespace N4Ws.Entidad
{
   public class Servicios : ModuloBase
    {

        public override void OnInstanceCreate()
        {
            this.alterClase = "N4_SERVICE";
            base.OnInstanceCreate();
            this.Accesorio.ConfiguracionBase = "DATACON";
        }
        //inicializa de la instancia de servicio
        private static Servicios InicializaServicio(out string erno)
        {
            var p = new Servicios();
            if (!p.Accesorio.Inicializar(out erno))
            {
                return null;
            }
            return p;
        }
        //inicializa una las configuraciones de N4
        private static N4Configuration ObtenerInicializador( Servicios ser, out string novedad)
        {
            if (ser == null)
            {
                novedad = "Objeto inicializador es nulo";
                return null;
            }
            if (!ser.Accesorio.ExistenConfiguraciones)
            {
                novedad = "No existen configuraciones de inicialización";
                return null;
            }
            var ur = ser.Accesorio.ObtenerConfiguracion("URL")?.valor;
            var us = ser.Accesorio.ObtenerConfiguracion("USUARIO")?.valor;
            var pas = ser.Accesorio.ObtenerConfiguracion("PASSWORD")?.valor;
            var sc = ser.Accesorio.ObtenerConfiguracion("SCOPE")?.valor;
           novedad = string.Empty;
          return  N4Configuration.GetInstance(us, pas, ur, sc);

        }

        public static N4_BasicResponse N4ServicioBasico<T>(T entidad,  string usuario) where T:class
        {
            //paso 1-> Inicializar instancia de servicio
            string pv;
            var p = InicializaServicio(out pv);
            var n = new N4_BasicResponse();
            p.actualMetodo = MethodBase.GetCurrentMethod().Name;
            if (p == null)
            {
                n.status = 3;
                n.status_id = "SEVERE";
                n.messages.Add(new N4_response_message("NO_INITIALIZED_DATA", "SEVERE", pv));
                return n;
            }
            //paso 2 -> inicializar instancia de n4ws
            var n4 = ObtenerInicializador(p, out pv);
            if (n4 == null)
            {
                n.status = 3;
                n.status_id = "SEVERE";
                n.messages.Add(new N4_response_message("NO_INITIALIZED_N4_INSTANCE", "SEVERE", pv));
                return n;
            }
            var nbb = N4Basic.GetInstance(n4.usuario, n4.password,n4.url, n4.scope);
            var n4r = nbb.BasicInvokeService<T>(entidad,usuario,p.myClase,p.actualMetodo, 7000);
            return n4r;
        }

        public static N4_Bill_TransactionReponse<billingTransaction> N4ServicioBasico(billing entidad,  string usuario)                                                                                                   
        {
            var ni = new N4_Bill_TransactionReponse<billingTransaction>();
            //paso 1-> Inicializar instancia de servicio
            string pv;
            var p = InicializaServicio(out pv);
        //    var n = new N4_BasicResponse();
            p.actualMetodo = MethodBase.GetCurrentMethod().Name;
            if (p == null)
            {
                ni.status = 3;
                ni.status_id = "SEVERE";
                ni.messages.Add(new N4_response_message("NO_INITIALIZED_DATA", "SEVERE", pv));
                return ni;
            }

            //paso 2 -> inicializar instancia de n4configurariones
            var n4 = ObtenerInicializador(p, out pv);
            if (n4 == null)
            {
                ni.status = 3;
                ni.status_id = "SEVERE";
                ni.messages.Add(new N4_response_message("NO_INITIALIZED_N4_INSTANCE", "SEVERE", pv));
                return ni;
            }
            var nbb = N4Basic.GetInstance(n4.usuario, n4.password, n4.url, n4.scope);

            var lbt = nbb.BasicInvokeService(entidad,p.myClase,p.actualMetodo,usuario, 7500);
            ni.status = lbt.status; ni.status_id = lbt.status_id;
            ni.messages = lbt.messages;
            ni.trace = lbt.trace;
            if (lbt.response != null)
            {
                ni.response = lbt.response.FirstOrDefault();
            }
            else { ni.response = null; }
           
            return ni;
        }

        //la entidad billing debe venir con la solicitud de finalizar
        public static N4_Bill_TransactionReponse<FinalizeInvoiceResponse> N4ServicioBasicoFinalize(billing entidad,  string usuario)
        {
            var n = new N4_Bill_TransactionReponse<FinalizeInvoiceResponse>();
            //paso 1-> Inicializar instancia de servicio
            string pv;
            var p = InicializaServicio(out pv);
            p.actualMetodo = MethodBase.GetCurrentMethod().Name;
            if (p == null)
            {
                n.status = 3;
                n.status_id = "SEVERE";
                n.messages.Add(new N4_response_message("NO_INITIALIZED_DATA", "SEVERE", pv));
                return n;
            }
            //paso 2 -> inicializar instancia de n4configurariones
            var n4 = ObtenerInicializador(p, out pv);
            if (n4 == null)
            {
                n.status = 3;
                n.status_id = "SEVERE";
                n.messages.Add(new N4_response_message("NO_INITIALIZED_N4_INSTANCE", "SEVERE", pv));
                return n;
            }
            //me quedo con el basico
            entidad.Request = null;
            if (entidad.FinalizeRequest == null)
            {
                n.status = 3;
                n.status_id = "SEVERE";
                n.messages.Add(new N4_response_message("NO_FINALIZE_REQUEST ", "SEVERE","NO SE ENCONTRO LA SOLICITUD PARA FINALIZAR"));
                return n;
            }

            var nbb = N4Basic.GetInstance(n4.usuario, n4.password, n4.url, n4.scope);
            var lbt = nbb.BasicInvokeService<billing>(entidad,usuario,p.myClase,p.actualMetodo, 7500);
            n.status = lbt.status;
            n.status_id = lbt.status_id;
            n.messages = lbt.messages;
            var xdoc = lbt.response;
            n.response = N4_Bill_TransactionReponse<billing>.FinalizeSerialResponse(lbt.response);
            return n;
        }

        public static N4_Bill_TransactionReponse<billingTransaction> N4ServicioBasicoFinalizeTransaction(billing entidad, string usuario)
        {
            var n = new N4_Bill_TransactionReponse<billingTransaction>();
            //paso 1-> Inicializar instancia de servicio
            string pv;
            var p = InicializaServicio(out pv);
            p.actualMetodo = MethodBase.GetCurrentMethod().Name;
            if (p == null)
            {
                n.status = 3;
                n.status_id = "SEVERE";
                n.trace = p.LogError<ApplicationException>(new ApplicationException("Falló Inicializador"), p.actualMetodo, usuario);
                n.messages.Add(new N4_response_message("NO_INITIALIZED_DATA", "SEVERE", pv));
                return n;
            }
            //paso 2 -> inicializar instancia de n4configurariones
            var n4 = ObtenerInicializador(p, out pv);
            if (n4 == null)
            {
                n.status = 3;
                n.status_id = "SEVERE";
                n.trace = p.LogError<ApplicationException>(new ApplicationException("Falló Inicializador"), p.actualMetodo, usuario);
                n.messages.Add(new N4_response_message("NO_INITIALIZED_N4_INSTANCE", "SEVERE", pv));
                return n;
            }


            var nbb = N4Basic.GetInstance(n4.usuario, n4.password, n4.url, n4.scope);
            var lbt = nbb.BasicInvokeService<billing>(entidad, usuario, p.myClase, p.actualMetodo, 7500);




            //RECUPERA LA FACTURA->DRAFT:INVOICE
            var inv = (from iv in lbt.response.Descendants()
                       where iv.Name.LocalName == "invoice"
                       select iv).FirstOrDefault();

            //NO ENCONTRO -> INVOICE
            if (inv == null)
            {
                n.status = 3;
                n.status_id = "SEVERE";
                n.trace = p.LogError<ApplicationException>(new ApplicationException("No se encontro INVOICE"), p.actualMetodo, usuario);
                n.messages.Add(new N4_response_message("NO_INVOICE_ITEM_ON_RESPONSE ", "SEVERE", "NO SE ENCONTRO EL ITEM INVOICE EN LA RESPUESTA DEL MERGE N4"));
                return n;
            }

            //RECUPERA EL ATRIBUTO ->NUMERO DE DRAFT
            var att = (from at in inv.Attributes()
                       where at.Name.LocalName == "draftNumber"
                       select at.Value).FirstOrDefault();
            if (att == null)
            {
                n.status = 3;
                n.status_id = "SEVERE";
                n.trace = p.LogError<ApplicationException>(new ApplicationException("No se encontro DRAFTNUMBER"), p.actualMetodo, usuario);
                n.messages.Add(new N4_response_message("NO_DRAFTNBR_ATTRIBUTE_ON_RESPONSE ", "SEVERE", "NO SE ENCONTRO EL ATRIBUTO DRAFTNBR"));
                return n;
            }
            //CONVIERTE TODO EL XML A BILLING TRASNACION
            var invo_n4 = N4_Bill_TransactionReponse<billingTransaction>.BillingTransactionData(lbt.response);
            if (invo_n4 == null)
            {
                n.status = 3;
                n.status_id = "SEVERE";
                n.trace = p.LogError<ApplicationException>(new ApplicationException("Error de conversion"), p.actualMetodo, usuario);
                n.messages.Add(new N4_response_message("CONVERSION_DATA_ERROR ", "SEVERE", "NO SE PUDO CONVERTIR XML EN BILLINGTRANSACTION"));
                return n;
            }

            if (invo_n4.billInvoice != null && !string.IsNullOrEmpty( invo_n4.billInvoice.totalTotal) && invo_n4.billInvoice.totalTotal.Equals("0.0"))
            {
                n.status = 3;
                n.status_id = "SEVERE";
                n.trace = p.LogError<ApplicationException>(new ApplicationException("INVALID_DRAFT"), p.actualMetodo, usuario);
                n.messages.Add(new N4_response_message("EMPTY_DRAFT", "SEVERE", string.Format("El Draft {0}, resulta en valor {1}, favor Borrar",invo_n4.billInvoice.draftNumber,invo_n4.billInvoice.totalTotal)));
                return n;
            }

            //PREPARA LA ENTIDAD PARA FINALIZAR
            entidad = null;
            entidad = new billing();
            entidad.FinalizeRequest = new FinalizeInvoiceRequest();
            entidad.FinalizeRequest.drftInvoiceNbr = att;
            entidad.FinalizeRequest.finalizeDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
            entidad.Request = null;

            bool finalizar_draft = false;
            //si existen configuraciones
            if (p.Accesorio.ExistenConfiguraciones)
            {
              var cc = p.Accesorio.ObtenerConfiguracion("FINALIZAR");
                if (cc != null && cc.valor.Equals("SI"))
                {
                    finalizar_draft = true;
                }
            }
            if (finalizar_draft == false)
            {
                n.status = 0;
                n.status_id = "OK";
                n.messages = new List<N4_response_message>();
                invo_n4.billInvoice.finalNumber = "00000000000001";
                n.response = invo_n4;
                n.trace = -1;
                return n;
            }

            else
            {
                //SECUENCIA DE FINALIZACION
                lbt = nbb.BasicInvokeService<billing>(entidad, usuario, p.myClase, p.actualMetodo, 7500);
                n.status = lbt.status;
                n.status_id = lbt.status_id;
                n.messages = lbt.messages;
                var xdoc = lbt.response;
                invo_n4.billInvoice.finalNumber = N4_Bill_TransactionReponse<billingTransaction>.FinalizarNumeroFinal(lbt.response);
                n.response = invo_n4;
                n.trace = lbt.trace;
                return n;
            }
        }
        private static N4_BasicResponse N4ICUService(ICU_API icu, string usuario)
        {
            var n = new N4_BasicResponse();
            string pv;
            var p = InicializaServicio(out pv);
            p.actualMetodo = MethodBase.GetCurrentMethod().Name;
            if (p == null)
            {
                n.status = 3;
                n.status_id = "SEVERE";
                n.messages.Add(new N4_response_message("NO_INITIALIZED_DATA", "SEVERE", pv));
                return n;
            }
            //paso 2 -> inicializar instancia de n4configurariones
            var n4 = ObtenerInicializador(p, out pv);
            if (n4 == null)
            {
                n.status = 3;
                n.status_id = "SEVERE";
                n.messages.Add(new N4_response_message("NO_INITIALIZED_N4_INSTANCE", "SEVERE", pv));
                return n;
            }
            if (icu == null)
            {
                n.status = 3;
                n.status_id = "SEVERE";
                n.messages.Add(new N4_response_message("NO_ICU_INSTANCE", "SEVERE", "NO SE ENCONTRO OBJETO ICU DE API"));
                return n;
            }
            var nbb = N4Basic.GetInstance(n4.usuario, n4.password, n4.url, n4.scope);



            return nbb.BasicInvokeService<ICU_API>(icu, usuario, p.myClase, p.actualMetodo, 7500);

        }


        public static N4_Bill_TransactionReponse<FinalizeInvoiceResponse> N4ServicioBasicoMergeAndFinalize(billing entidad,  string usuario)
        {
 
            var n = new N4_Bill_TransactionReponse<FinalizeInvoiceResponse>();
            //paso 1-> Inicializar instancia de servicio
            string pv;
            var p = InicializaServicio(out pv);
            p.actualMetodo = MethodBase.GetCurrentMethod().Name;
            if (p == null)
            {
                n.status = 3;
                n.status_id = "SEVERE";
                n.messages.Add(new N4_response_message("NO_INITIALIZED_DATA", "SEVERE", pv));
                return n;
            }
            //paso 2 -> inicializar instancia de n4configurariones
            var n4 = ObtenerInicializador(p, out pv);
            if (n4 == null)
            {
                n.status = 3;
                n.status_id = "SEVERE";
                n.messages.Add(new N4_response_message("NO_INITIALIZED_N4_INSTANCE", "SEVERE", pv));
                return n;
            }

           /*
            if (string.IsNullOrEmpty(entidad.Request.InvoiceTypeId))
            {
                n.status = 3;
                n.status_id = "SEVERE";
                n.messages.Add(new N4_response_message("NO_INVOICE_TYPE_ID", "SEVERE", "NO SE ENOCNTRÓ EL NOMBRE DEL INVOICE TYPE EN EL ELEMENTO BILLING REQUEST"));
                return n;
            }
            */
            //aqui comprobar que la lista de draftVenga

            if (entidad.MergeInvoiceRequest == null || entidad.MergeInvoiceRequest.drftInvoiceNbrs.Count < 0)
            {
                n.status = 3;
                n.status_id = "SEVERE";
                n.messages.Add(new N4_response_message("NO_DRAFT_NBRS", "SEVERE", "NO SE ENCONTRO LOS NUMEROS DE DRAFT"));
                return n;
            }

            var it = entidad.Request != null ? entidad.Request.InvoiceTypeId : null;
            if (entidad.MergeInvoiceRequest == null)
            {
                n.status = 3;
                n.status_id = "SEVERE";
                n.messages.Add(new N4_response_message("NO_MERGE_REQUEST ", "SEVERE", "NO SE ENCONTRO LA SOLICITUD PARA MERGE"));
                return n;
            }

           // entidad.MergeInvoiceRequest.invoiceTypeId = entidad.Request.InvoiceTypeId;
            entidad.Request = null;
            entidad.FinalizeRequest = null;
            if (string.IsNullOrEmpty(it))
            {
                it = entidad.MergeInvoiceRequest.invoiceTypeId != null ? entidad.MergeInvoiceRequest.invoiceTypeId : null;
            }
            else
            {
                entidad.MergeInvoiceRequest.invoiceTypeId = entidad.MergeInvoiceRequest.invoiceTypeId == null ? it : null;
            }

            if (string.IsNullOrEmpty(it))
            {
                n.status = 3;
                n.status_id = "SEVERE";
                n.messages.Add(new N4_response_message("NO_INVOICE_TYPE_IN_MERGE_OR_BILLING_REQUEST ", "SEVERE", "NO SE ENCONTRO EL INVOICETYPEID, NI EN BILLING REQUEST O MERGE REQUEST"));
                return n;
            }
            var nbb = N4Basic.GetInstance(n4.usuario, n4.password, n4.url, n4.scope);
            string findate = entidad.MergeInvoiceRequest.finalizeDate;
            entidad.MergeInvoiceRequest.finalizeDate = null;

            //manda el merge
            var lbt = nbb.BasicInvokeService<billing>(entidad, usuario, p.myClase, p.actualMetodo, 7500);
            //EL MERGE ME REGresa una factura completa el documento, solo entreguemos el numero final o la factura?

            //hubo un error en el merge
            if (lbt.status > 2)
            {
                n.status = lbt.status;
                n.status_id = string.Format("MERGE_ERROR:{0}",lbt.status_id);
                n.messages = lbt.messages;
                return n;
            }
            //RECUPERA LA FACTURA->DRAFT
            var inv = (from iv in lbt.response.Descendants()
                       where iv.Name.LocalName == "invoice"
                       select iv).FirstOrDefault();


            if (inv == null)
            {
                n.status = 3;
                n.status_id = "SEVERE";
                n.messages.Add(new N4_response_message("NO_INVOICE_ITEM_ON_RESPONSE ", "SEVERE", "NO SE ENCONTRO EL ITEM INVOICE EN LA RESPUESTA DEL MERGE N4"));
                return n;
            }

            //RECUPERA EL ATRIBUTO ->NUMERO
            var att = (from at in inv.Attributes()
                       where at.Name.LocalName == "draftNumber"
                       select at.Value).FirstOrDefault();
            if (att == null)
            {
                n.status = 3;
                n.status_id = "SEVERE";
                n.messages.Add(new N4_response_message("NO_DRAFTNBR_ATTRIBUTE_ON_RESPONSE ", "SEVERE", "NO SE ENCONTRO EL ATRIBUTO DRAFTNBR"));
                return n;
            }
            
            entidad = null;
            entidad = new billing();
            entidad.FinalizeRequest = new FinalizeInvoiceRequest();
            entidad.FinalizeRequest.drftInvoiceNbr = att;
            entidad.FinalizeRequest.finalizeDate = findate;
            entidad.Request = null;

            //ahora invoca la finalizacion
            lbt = nbb.BasicInvokeService<billing>(entidad, usuario, p.myClase, p.actualMetodo, 7500);
            n.status = lbt.status;
            n.status_id = lbt.status_id;
            n.messages = lbt.messages;
            var xdoc = lbt.response;
            n.response = N4_Bill_TransactionReponse<billing>.FinalizeSerialResponse(lbt.response);
            return n;
        }



        //REVISAR
        public static N4_Bill_TransactionReponse<billingTransaction> N4ServicioBasicoMergeAndFinalizeTransaction(billing entidad, string usuario)
        {

            var n = new N4_Bill_TransactionReponse<billingTransaction>();
            //paso 1-> Inicializar instancia de servicio
            string pv;
            var p = InicializaServicio(out pv);
            p.actualMetodo = MethodBase.GetCurrentMethod().Name;
            if (p == null)
            {
                n.status = 3;
                n.status_id = "SEVERE";
                n.messages.Add(new N4_response_message("NO_INITIALIZED_DATA", "SEVERE", pv));
                n.trace = p.LogError<ApplicationException>(new ApplicationException("Falló Inicializador"), p.actualMetodo, usuario);
                return n;
            }
            //paso 2 -> inicializar instancia de n4configurariones
            var n4 = ObtenerInicializador(p, out pv);
            if (n4 == null)
            {
                n.status = 3;
                n.status_id = "SEVERE";
                n.trace = p.LogError<ApplicationException>(new ApplicationException("Falló Inicializador"), p.actualMetodo, usuario);
              
                n.messages.Add(new N4_response_message("NO_INITIALIZED_N4_INSTANCE", "SEVERE", pv));
                return n;
            }

            /*
             if (string.IsNullOrEmpty(entidad.Request.InvoiceTypeId))
             {
                 n.status = 3;
                 n.status_id = "SEVERE";
                 n.messages.Add(new N4_response_message("NO_INVOICE_TYPE_ID", "SEVERE", "NO SE ENOCNTRÓ EL NOMBRE DEL INVOICE TYPE EN EL ELEMENTO BILLING REQUEST"));
                 return n;
             }
             */
            //aqui comprobar que la lista de draftVenga

            if (entidad.MergeInvoiceRequest == null || entidad.MergeInvoiceRequest.drftInvoiceNbrs.Count < 0)
            {
                n.status = 3;
                n.status_id = "SEVERE";
                n.trace = p.LogError<ApplicationException>(new ApplicationException("No hay numeros de DRAFT"), p.actualMetodo, usuario);
                n.messages.Add(new N4_response_message("NO_DRAFT_NBRS", "SEVERE", "NO SE ENCONTRO LOS NUMEROS DE DRAFT"));
                return n;
            }

            var it = entidad.Request != null ? entidad.Request.InvoiceTypeId : null;
            if (entidad.MergeInvoiceRequest == null)
            {
                n.status = 3;
                n.status_id = "SEVERE";
                n.trace = p.LogError<ApplicationException>(new ApplicationException("SIN SOLICITUD DE MERGE"), p.actualMetodo, usuario);
                n.messages.Add(new N4_response_message("NO_MERGE_REQUEST ", "SEVERE", "NO SE ENCONTRO LA SOLICITUD PARA MERGE"));
                return n;
            }

        //    entidad.MergeInvoiceRequest.invoiceTypeId = entidad.Request.InvoiceTypeId;
            entidad.Request = null;
            entidad.FinalizeRequest = null;
            if (string.IsNullOrEmpty(it))
            {
                it = entidad.MergeInvoiceRequest.invoiceTypeId != null ? entidad.MergeInvoiceRequest.invoiceTypeId : null;
            }
            else
            {
                entidad.MergeInvoiceRequest.invoiceTypeId = entidad.MergeInvoiceRequest.invoiceTypeId == null ? it : null;
            }

            if (string.IsNullOrEmpty(it))
            {
                n.status = 3;
                n.status_id = "SEVERE";
                n.trace = p.LogError<ApplicationException>(new ApplicationException("SIN INVOICETYPEID"), p.actualMetodo, usuario);
                n.messages.Add(new N4_response_message("NO_INVOICE_TYPE_IN_MERGE_OR_BILLING_REQUEST ", "SEVERE", "NO SE ENCONTRO EL INVOICETYPEID, NI EN BILLING REQUEST O MERGE REQUEST"));
                return n;
            }
            var nbb = N4Basic.GetInstance(n4.usuario, n4.password, n4.url, n4.scope);
            string findate = entidad.MergeInvoiceRequest.finalizeDate;
            entidad.MergeInvoiceRequest.finalizeDate = null;

            //manda el merge
            var lbt = nbb.BasicInvokeService<billing>(entidad, usuario, p.myClase, p.actualMetodo, 7500);
            //EL MERGE ME REGresa una factura completa el documento, solo entreguemos el numero final o la factura?

            //hubo un error en el merge
            if (lbt.status > 2)
            {
                n.status = lbt.status;
                n.trace = p.LogError<ApplicationException>(new ApplicationException(lbt.status_id), p.actualMetodo, usuario);
                n.status_id = string.Format("MERGE_ERROR:{0}", lbt.status_id);
                n.trace = lbt.trace;
                n.messages = lbt.messages;
                return n;
            }
            //RECUPERA LA FACTURA->DRAFT:INVOICE
            var inv = (from iv in lbt.response.Descendants()
                       where iv.Name.LocalName == "invoice"
                       select iv).FirstOrDefault();

            //NO ENCONTRO -> INVOICE
            if (inv == null)
            {
                n.status = 3;
                n.status_id = "SEVERE";
                n.trace = p.LogError<ApplicationException>(new ApplicationException("No se encontró elemento INVOICE"), p.actualMetodo, usuario);
                n.messages.Add(new N4_response_message("NO_INVOICE_ITEM_ON_RESPONSE ", "SEVERE", "NO SE ENCONTRO EL ITEM INVOICE EN LA RESPUESTA DEL MERGE N4"));
                return n;
            }

            //RECUPERA EL ATRIBUTO ->NUMERO DE DRAFT
            var att = (from at in inv.Attributes()
                       where at.Name.LocalName == "draftNumber"
                       select at.Value).FirstOrDefault();
            if (att == null)
            {
                n.status = 3;
                n.status_id = "SEVERE";
                n.trace = p.LogError<ApplicationException>(new ApplicationException("No se encontro DRAFTNUMBER"), p.actualMetodo, usuario);
                n.messages.Add(new N4_response_message("NO_DRAFTNBR_ATTRIBUTE_ON_RESPONSE ", "SEVERE", "NO SE ENCONTRO EL ATRIBUTO DRAFTNBR"));
                return n;
            }
            //CONVIERTE TODO EL XML A BILLING TRASNACION
            var invo_n4 = N4_Bill_TransactionReponse<billingTransaction>.BillingTransactionData(lbt.response);
            if (invo_n4 == null)
            {
                n.status = 3;
                n.status_id = "SEVERE";
                n.trace = p.LogError<ApplicationException>(new ApplicationException("ERROR DE CONVERSION"), p.actualMetodo, usuario);
                n.messages.Add(new N4_response_message("CONVERSION_DATA_ERROR ", "SEVERE", "NO SE PUDO CONVERTIR XML EN BILLINGTRANSACTION"));
                return n;
            }
            //PREPARA LA ENTIDAD PARA FINALIZAR
            entidad = null;
            entidad = new billing();
            entidad.FinalizeRequest = new FinalizeInvoiceRequest();
            entidad.FinalizeRequest.drftInvoiceNbr = att;
            entidad.FinalizeRequest.finalizeDate = findate;
            entidad.Request = null;

            //ahora invoca la finalizacion
            lbt = nbb.BasicInvokeService<billing>(entidad, usuario, p.myClase, p.actualMetodo, 7500);
            //AQUI YA FINALIZÓ
            n.status = lbt.status;
            n.status_id = lbt.status_id;
            n.messages = lbt.messages;
            var xdoc = lbt.response;
            //RECUPERO EL NUMERO FINAL Y LO PONGO AL INVOICE
            invo_n4.billInvoice.finalNumber = N4_Bill_TransactionReponse<billingTransaction>.FinalizarNumeroFinal(lbt.response);
            n.response = invo_n4;
            return n;
        }

        //con keyword
        public static N4_Bill_TransactionReponse<billingTransaction> N4ServicioBasicoMergeAndFinalizeTransaction(billing entidad, string usuario, DateTime? fecha_inicio=null, string palabra_clave=null)
        {

            var n = new N4_Bill_TransactionReponse<billingTransaction>();
            //paso 1-> Inicializar instancia de servicio
            string pv;
            var p = InicializaServicio(out pv);
            p.actualMetodo = MethodBase.GetCurrentMethod().Name;
            if (p == null)
            {
                n.status = 3;
                n.status_id = "SEVERE";
                n.messages.Add(new N4_response_message("NO_INITIALIZED_DATA", "SEVERE", pv));
                n.trace = p.LogError<ApplicationException>(new ApplicationException("Falló Inicializador"), p.actualMetodo, usuario);
                return n;
            }
            //paso 2 -> inicializar instancia de n4configurariones
            var n4 = ObtenerInicializador(p, out pv);
            if (n4 == null)
            {
                n.status = 3;
                n.status_id = "SEVERE";
                n.trace = p.LogError<ApplicationException>(new ApplicationException("Falló Inicializador"), p.actualMetodo, usuario);

                n.messages.Add(new N4_response_message("NO_INITIALIZED_N4_INSTANCE", "SEVERE", pv));
                return n;
            }

            /*
             if (string.IsNullOrEmpty(entidad.Request.InvoiceTypeId))
             {
                 n.status = 3;
                 n.status_id = "SEVERE";
                 n.messages.Add(new N4_response_message("NO_INVOICE_TYPE_ID", "SEVERE", "NO SE ENOCNTRÓ EL NOMBRE DEL INVOICE TYPE EN EL ELEMENTO BILLING REQUEST"));
                 return n;
             }
             */
            //aqui comprobar que la lista de draftVenga

            if (entidad.MergeInvoiceRequest == null || entidad.MergeInvoiceRequest.drftInvoiceNbrs.Count < 0)
            {
                n.status = 3;
                n.status_id = "SEVERE";
                n.trace = p.LogError<ApplicationException>(new ApplicationException("No hay numeros de DRAFT"), p.actualMetodo, usuario);
                n.messages.Add(new N4_response_message("NO_DRAFT_NBRS", "SEVERE", "NO SE ENCONTRO LOS NUMEROS DE DRAFT"));
                return n;
            }

            var it = entidad.Request != null ? entidad.Request.InvoiceTypeId : null;
            if (entidad.MergeInvoiceRequest == null)
            {
                n.status = 3;
                n.status_id = "SEVERE";
                n.trace = p.LogError<ApplicationException>(new ApplicationException("SIN SOLICITUD DE MERGE"), p.actualMetodo, usuario);
                n.messages.Add(new N4_response_message("NO_MERGE_REQUEST ", "SEVERE", "NO SE ENCONTRO LA SOLICITUD PARA MERGE"));
                return n;
            }

            //    entidad.MergeInvoiceRequest.invoiceTypeId = entidad.Request.InvoiceTypeId;
            entidad.Request = null;
            entidad.FinalizeRequest = null;
            if (string.IsNullOrEmpty(it))
            {
                it = entidad.MergeInvoiceRequest.invoiceTypeId != null ? entidad.MergeInvoiceRequest.invoiceTypeId : null;
            }
            else
            {
                entidad.MergeInvoiceRequest.invoiceTypeId = entidad.MergeInvoiceRequest.invoiceTypeId == null ? it : null;
            }

            if (string.IsNullOrEmpty(it))
            {
                n.status = 3;
                n.status_id = "SEVERE";
                n.trace = p.LogError<ApplicationException>(new ApplicationException("SIN INVOICETYPEID"), p.actualMetodo, usuario);
                n.messages.Add(new N4_response_message("NO_INVOICE_TYPE_IN_MERGE_OR_BILLING_REQUEST ", "SEVERE", "NO SE ENCONTRO EL INVOICETYPEID, NI EN BILLING REQUEST O MERGE REQUEST"));
                return n;
            }
            var nbb = N4Basic.GetInstance(n4.usuario, n4.password, n4.url, n4.scope);
            string findate = entidad.MergeInvoiceRequest.finalizeDate;
            entidad.MergeInvoiceRequest.finalizeDate = null;

            //manda el merge
            var lbt = nbb.BasicInvokeService<billing>(entidad, usuario, p.myClase, p.actualMetodo, 7500,fecha_inicio,palabra_clave);
            //EL MERGE ME REGresa una factura completa el documento, solo entreguemos el numero final o la factura?

            //hubo un error en el merge
            if (lbt.status > 2)
            {
                n.status = lbt.status;
                n.trace = p.LogError<ApplicationException>(new ApplicationException(lbt.status_id), p.actualMetodo, usuario);
                n.status_id = string.Format("MERGE_ERROR:{0}", lbt.status_id);
                n.trace = lbt.trace;
                n.messages = lbt.messages;
                return n;
            }
            //RECUPERA LA FACTURA->DRAFT:INVOICE
            var inv = (from iv in lbt.response.Descendants()
                       where iv.Name.LocalName == "invoice"
                       select iv).FirstOrDefault();

            //NO ENCONTRO -> INVOICE
            if (inv == null)
            {
                n.status = 3;
                n.status_id = "SEVERE";
                n.trace = p.LogError<ApplicationException>(new ApplicationException("No se encontró elemento INVOICE"), p.actualMetodo, usuario);
                n.messages.Add(new N4_response_message("NO_INVOICE_ITEM_ON_RESPONSE ", "SEVERE", "NO SE ENCONTRO EL ITEM INVOICE EN LA RESPUESTA DEL MERGE N4"));
                return n;
            }

            //RECUPERA EL ATRIBUTO ->NUMERO DE DRAFT
            var att = (from at in inv.Attributes()
                       where at.Name.LocalName == "draftNumber"
                       select at.Value).FirstOrDefault();
            if (att == null)
            {
                n.status = 3;
                n.status_id = "SEVERE";
                n.trace = p.LogError<ApplicationException>(new ApplicationException("No se encontro DRAFTNUMBER"), p.actualMetodo, usuario);
                n.messages.Add(new N4_response_message("NO_DRAFTNBR_ATTRIBUTE_ON_RESPONSE ", "SEVERE", "NO SE ENCONTRO EL ATRIBUTO DRAFTNBR"));
                return n;
            }
            //CONVIERTE TODO EL XML A BILLING TRASNACION
            var invo_n4 = N4_Bill_TransactionReponse<billingTransaction>.BillingTransactionData(lbt.response);
            if (invo_n4 == null)
            {
                n.status = 3;
                n.status_id = "SEVERE";
                n.trace = p.LogError<ApplicationException>(new ApplicationException("ERROR DE CONVERSION"), p.actualMetodo, usuario);
                n.messages.Add(new N4_response_message("CONVERSION_DATA_ERROR ", "SEVERE", "NO SE PUDO CONVERTIR XML EN BILLINGTRANSACTION"));
                return n;
            }
            //PREPARA LA ENTIDAD PARA FINALIZAR
            entidad = null;
            entidad = new billing();
            entidad.FinalizeRequest = new FinalizeInvoiceRequest();
            entidad.FinalizeRequest.drftInvoiceNbr = att;
            entidad.FinalizeRequest.finalizeDate = findate;
            entidad.Request = null;

            //ahora invoca la finalizacion
            lbt = nbb.BasicInvokeService<billing>(entidad, usuario, p.myClase, p.actualMetodo, 7500);
            //AQUI YA FINALIZÓ
            n.status = lbt.status;
            n.status_id = lbt.status_id;
            n.messages = lbt.messages;
            var xdoc = lbt.response;
            //RECUPERO EL NUMERO FINAL Y LO PONGO AL INVOICE
            invo_n4.billInvoice.finalNumber = N4_Bill_TransactionReponse<billingTransaction>.FinalizarNumeroFinal(lbt.response);
            n.response = invo_n4;
            return n;
        }


        public static N4_BasicResponse CrearNuevoAppointment(gate gt, string usuario)
        {
            var n = new N4_BasicResponse();
            //paso 1-> Inicializar instancia de servicio
            string pv;
            var p = InicializaServicio(out pv);
            p.actualMetodo = MethodBase.GetCurrentMethod().Name;
            if (p == null)
            {
                n.status = 3;
                n.status_id = "SEVERE";
                n.messages.Add(new N4_response_message("NO_INITIALIZED_DATA", "SEVERE", pv));
                return n;
            }
            //paso 2 -> inicializar instancia de n4configurariones
            var n4 = ObtenerInicializador(p, out pv);
            if (n4 == null)
            {
                n.status = 3;
                n.status_id = "SEVERE";
                n.messages.Add(new N4_response_message("NO_INITIALIZED_N4_INSTANCE", "SEVERE", pv));
                return n;
            }
            if (gt== null || gt.appointment== null)
            {
                n.status = 3;
                n.status_id = "SEVERE";
                n.messages.Add(new N4_response_message("NO_APPOINTMET_FOR_GATE", "SEVERE", "gate es nulo"));
                return n;

            }
            if (!gt.appointment.appointment_date.HasValue)
            {
                n.status = 3;
                n.status_id = "SEVERE";
                n.messages.Add(new N4_response_message("NO_DATE_FOR_APPOINTMENT", "SEVERE", "NO ha fecha de appointment"));
                return n;

            }
            if (string.IsNullOrEmpty( gt.appointment.container_id))
            {
                n.status = 3;
                n.status_id = "SEVERE";
                n.messages.Add(new N4_response_message("NO_EQUIP_FOR_APPOINTMENT", "SEVERE", "NO ha fecha de appointment"));
                return n;
            }
            var nbb = N4Basic.GetInstance(n4.usuario, n4.password, n4.url, n4.scope);
            var gs = gt.ToString();

           var aprr = nbb.BasicInvokeService(gs, p.myClase, p.actualMetodo, usuario, 7000);

            var fir = aprr.response.Descendants().Where(nf => nf.Name.LocalName== "appointment-nbr").FirstOrDefault();
            if (fir != null)
            {
                aprr.messages.Add(new N4_response_message("appointment", fir.Value, "appointment-nbr"));
            }

            return aprr ;
        }

        public static N4_BasicResponse CancelarAppointments(gate gt, string usuario)
        {
            var n = new N4_BasicResponse();
            //paso 1-> Inicializar instancia de servicio
            string pv;
            var p = InicializaServicio(out pv);
            p.actualMetodo = MethodBase.GetCurrentMethod().Name;
            if (p == null)
            {
                n.status = 3;
                n.status_id = "SEVERE";
                n.messages.Add(new N4_response_message("NO_INITIALIZED_DATA", "SEVERE", pv));
                return n;
            }
            //paso 2 -> inicializar instancia de n4configurariones
            var n4 = ObtenerInicializador(p, out pv);
            if (n4 == null)
            {
                n.status = 3;
                n.status_id = "SEVERE";
                n.messages.Add(new N4_response_message("NO_INITIALIZED_N4_INSTANCE", "SEVERE", pv));
                return n;
            }

            if (gt.appointments== null || gt.appointments.Count<=0)
            {
                n.status = 3;
                n.status_id = "SEVERE";
                n.messages.Add(new N4_response_message("NO_APPOINTMENTS_FOR_CANCEL","SEVERE","No existen appointments que cancelar"));
                return n;

            }
            var nbb = N4Basic.GetInstance(n4.usuario, n4.password, n4.url, n4.scope);
            var gs = gt.ToString();
            return nbb.BasicInvokeService(gs,p.myClase,p.actualMetodo,usuario, 7000);
        }

        private static N4_BasicResponse ReeferHours(CGSAComputeAndSplitReeferHoursWS rh, string usuario)
        {
            var n = new N4_BasicResponse();
            if (rh == null)
            {
                n.status = 3;
                n.status_id = "SEVERE";
                n.messages.Add(new N4_response_message("NO_PARAMETERS", "SEVERE", "Entidad es nula RH"));
               return n;

            }
            //paso 1-> Inicializar instancia de servicio
            string pv;
            var p = InicializaServicio(out pv);
            p.actualMetodo = MethodBase.GetCurrentMethod().Name;
            if (p == null)
            {
                n.status = 3;
                n.status_id = "SEVERE";
                n.messages.Add(new N4_response_message("NO_INITIALIZED_DATA", "SEVERE", pv));
                return n;
            }
            //paso 2 -> inicializar instancia de n4configurariones
            var n4 = ObtenerInicializador(p, out pv);
            if (n4 == null)
            {
                n.status = 3;
                n.status_id = "SEVERE";
                n.messages.Add(new N4_response_message("NO_INITIALIZED_N4_INSTANCE", "SEVERE", pv));
                return n;
            }

            var nbb = N4Basic.GetInstance(n4.usuario, n4.password, n4.url, n4.scope);
            var gs = rh.ToString();
            return nbb.BasicInvokeService(gs,p.myClase,p.actualMetodo,usuario, 7000);
        }

        
        //retorna nulo es error!!!
        public static void ReeferImpoHour( List<Int64> gkeys, string reference, DateTime cutDay, string usuario)
        {
            var s = new Servicios();
            string pv;
            var p = InicializaServicio(out pv);
            p.actualMetodo = MethodBase.GetCurrentMethod().Name;
            if (p == null)
            {
                p.LogError<ApplicationException>(new ApplicationException("No fue posible inicializar objeto servicios"), p.actualMetodo, usuario);
                //trace log not init
            }
            var dd = new Dictionary<string, string>();
            if (gkeys.Count <= 0)
            {
                //save log no count
                p.LogError<ApplicationException>(new ApplicationException("La lista de gkey es vacia"), p.actualMetodo, usuario);
            }
            foreach (var g in gkeys)
            {
                var cg = new CGSAComputeAndSplitReeferHoursWS(g.ToString(), cutDay.ToString("yyyy-MM-dd HH:mm:ss"), reference);
                var n4r = ReeferHours(cg, usuario);
                if (n4r.status > 2)
                {
                    //log problem
                    p.LogError<ApplicationException>(new ApplicationException (string.Format("{0},{1}",g, n4r.status_id)), p.actualMetodo, usuario);
                }
            }
        }


        public static Respuesta.ResultadoOperacion<bool> PonerEventoPasePuerta(string contenedor, string usuario)
        {
            string pv;
            var p = InicializaServicio(out pv);
            p.actualMetodo = MethodBase.GetCurrentMethod().Name;
            if (p == null)
            {
                p.LogError<ApplicationException>(new ApplicationException("No fue posible inicializar objeto servicios"), p.actualMetodo, usuario);
                //trace log not init
                return Respuesta.ResultadoOperacion<bool>.CrearFalla("La inicialización del objeto N4 falló");
            }

            if (!p.Accesorio.ExistenConfiguraciones)
            {
                return Respuesta.ResultadoOperacion<bool>.CrearFalla("No existen configuraciones");
            }

            var evento = p.Accesorio.ObtenerConfiguracion("PASE_VENCIDO")?.valor;
            var notas = p.Accesorio.ObtenerConfiguracion("NOTA_EVENTO")?.valor;
            var propiedad = p.Accesorio.ObtenerConfiguracion("PROPIEDAD")?.valor;
            var tipo = p.Accesorio.ObtenerConfiguracion("TIPO")?.valor;

            var icu = new ICU_API();
            icu.evento = new ICU_EVENT();
            icu.evento.id = evento;
            icu.evento.timeeventapplied = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
            icu.evento.userid = usuario;
            icu.evento.note = notas;
            


            icu.units = new List<ICU_UNIT>();
            icu.units.Add(new ICU_UNIT() { id = contenedor, type = tipo });

            icu.properties = new List<ICU_PROPERTY>();
            icu.properties.Add(new ICU_PROPERTY() { tag=propiedad, value= notas});

            var tr = N4ICUService(icu, usuario);
            if (tr.status > 2)
            {
                return Respuesta.ResultadoOperacion<bool>.CrearFalla(tr.status_id);
            }


            return Respuesta.ResultadoOperacion<bool>.CrearResultadoExitoso(true,   "Éxito al aplicar el evento");
        }


        public static N4_BasicResponse EjecutarCODEExtension(GroovyCodeExtension co, string usuario)
        {
            var n = new N4_BasicResponse();
            //paso 1-> Inicializar instancia de servicio
            string pv;
            var p = InicializaServicio(out pv);
            p.actualMetodo = MethodBase.GetCurrentMethod().Name;
            if (p == null)
            {
                n.status = 3;
                n.status_id = "SEVERE";
                n.messages.Add(new N4_response_message("NO_INITIALIZED_DATA", "SEVERE", pv));
                return n;
            }
            //paso 2 -> inicializar instancia de n4configurariones
            var n4 = ObtenerInicializador(p, out pv);
            if (n4 == null)
            {
                n.status = 3;
                n.status_id = "SEVERE";
                n.messages.Add(new N4_response_message("NO_INITIALIZED_N4_INSTANCE", "SEVERE", pv));
                return n;
            }
            if (co == null)
            {
                n.status = 3;
                n.status_id = "SEVERE";
                n.messages.Add(new N4_response_message("NO_CODE_EXENSION_NULL", "SEVERE", "CODE_EXTENSON es nulo"));
                return n;

            }
            if (string.IsNullOrEmpty(co.location))
            {
                n.status = 3;
                n.status_id = "SEVERE";
                n.messages.Add(new N4_response_message("NO_LOCATION_FOR_CODE_EXTENSION", "SEVERE", "NO HAY LOCACION"));
                return n;

            }
            if (string.IsNullOrEmpty(co.name))
            {
                n.status = 3;
                n.status_id = "SEVERE";
                n.messages.Add(new N4_response_message("NO_NAME_FOR_CODE_EXTENSION", "SEVERE", "NO HAY NOMBRE"));
                return n;
            }
            var nbb = N4Basic.GetInstance(n4.usuario, n4.password, n4.url, n4.scope);
            var gs = co.ToString();

            var aprr = nbb.BasicInvokeService(gs, p.myClase, p.actualMetodo, usuario, 7000);

            //obtener el manifiesto
            var bil = co.parameters.Where(s => s.Key.Equals("BLs")).FirstOrDefault();

            var fir = aprr.response.Descendants().Where(nf => nf.Name.LocalName == "result").FirstOrDefault();
            if (fir != null && !string.IsNullOrEmpty(fir.Value))
            {
                Int32 numero;
                aprr.messages.Clear();
                if (fir.Value.Contains("OK-") || int.TryParse(fir.Value,out numero))
                {
                    fir.Value = fir.Value.Replace("OK-", "");
                    aprr.messages.Add(new N4_response_message("pase_id", fir.Value, "result"));
                }
                else
                {
                    if (fir.Value.Contains("ERRKEY_QTY_ORDERED_EXCEEDS_AVAILABLE_QTY_FOR_CARGO_LOT") || fir.Value.Contains("ERRKEY_QTY_ORDERED_EXCEEDS_AVAILABLE_QTY_FOR_BL_ITEM"))
                    {
                        aprr.status = 3;
                        aprr.status_id = "ITEMS_QTY_EXCEDED";
                        aprr.messages.Add(new N4_response_message("STOCK EXCEDEED", "SEVERE", string.Format("Carga {0} no tiene stock disponible", bil.Value)));
                    }
                }
            }
            return aprr;
        }


        public static N4_BasicResponse EjecutarCODEExtensionGenerico(GroovyCodeExtension co, string usuario)
        {
            var n = new N4_BasicResponse();
            //paso 1-> Inicializar instancia de servicio
            string pv;
            var p = InicializaServicio(out pv);
            p.actualMetodo = MethodBase.GetCurrentMethod().Name;
            if (p == null)
            {
                n.status = 3;
                n.status_id = "SEVERE";
                n.messages.Add(new N4_response_message("NO_INITIALIZED_DATA", "SEVERE", pv));
                return n;
            }
            //paso 2 -> inicializar instancia de n4configurariones
            var n4 = ObtenerInicializador(p, out pv);
            if (n4 == null)
            {
                n.status = 3;
                n.status_id = "SEVERE";
                n.messages.Add(new N4_response_message("NO_INITIALIZED_N4_INSTANCE", "SEVERE", pv));
                return n;
            }
            if (co == null)
            {
                n.status = 3;
                n.status_id = "SEVERE";
                n.messages.Add(new N4_response_message("NO_CODE_EXENSION_NULL", "SEVERE", "CODE_EXTENSON es nulo"));
                return n;

            }
            if (string.IsNullOrEmpty(co.location))
            {
                n.status = 3;
                n.status_id = "SEVERE";
                n.messages.Add(new N4_response_message("NO_LOCATION_FOR_CODE_EXTENSION", "SEVERE", "NO HAY LOCACION"));
                return n;

            }
            if (string.IsNullOrEmpty(co.name))
            {
                n.status = 3;
                n.status_id = "SEVERE";
                n.messages.Add(new N4_response_message("NO_NAME_FOR_CODE_EXTENSION", "SEVERE", "NO HAY NOMBRE"));
                return n;
            }
            var nbb = N4Basic.GetInstance(n4.usuario, n4.password, n4.url, n4.scope);
            var gs = co.ToString();

            var aprr = nbb.BasicInvokeService(gs, p.myClase, p.actualMetodo, usuario, 7000);
            return aprr;
        }

        public static Respuesta.ResultadoOperacion<bool> PonerEventoPasePuertaCFS(Int64 GKEY, string USUARIO)
        {
            string pv;
            var p = InicializaServicio(out pv);
            p.actualMetodo = MethodBase.GetCurrentMethod().Name;
            if (p == null)
            {
                p.LogError<ApplicationException>(new ApplicationException("No fue posible inicializar objeto servicios"), p.actualMetodo, USUARIO);
                //trace log not init
                return Respuesta.ResultadoOperacion<bool>.CrearFalla("La inicialización del objeto N4 falló");
            }
            if (!p.Accesorio.ExistenConfiguraciones)
            {
                return Respuesta.ResultadoOperacion<bool>.CrearFalla("No existen configuraciones");
            }
            var evento = p.Accesorio.ObtenerConfiguracion("PASE_VENCIDO_CFS")?.valor;
            var notas = p.Accesorio.ObtenerConfiguracion("NOTA_EVENTO_CFS")?.valor;
             N4Ws.Entidad.GroovyCodeExtension code = new N4Ws.Entidad.GroovyCodeExtension();
            code.name = "CGSAUnitEventBBKWS";
            code.location = "code-extension";
            code.parameters.Add("UNIT", GKEY.ToString());
            code.parameters.Add("USER", USUARIO);
            code.parameters.Add("NOTES", notas);
            code.parameters.Add("DATE", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            code.parameters.Add("EVENT", evento);
            //Poner el evento
            var n4r = N4Ws.Entidad.Servicios.EjecutarCODEExtensionGenerico(code, USUARIO);
            if (n4r.status != 1)
            {
                var ex = new ApplicationException(n4r.status_id);
                var i =  p.LogError<ApplicationException>(ex, "PonerEventoPasePuertaCFS", USUARIO);
                var emsg = string.Format("Ha ocurrido la novedad número {0} durante el proceso favor comuníquese con facturacion",i.HasValue?i.Value:-1);
                return Respuesta.ResultadoOperacion<bool>.CrearFalla(emsg);
            }
            return Respuesta.ResultadoOperacion<bool>.CrearResultadoExitoso(true,   "Éxito al aplicar el evento");
        }


        public static Respuesta.ResultadoOperacion<bool> PonerEventoPasePuertaBRBK(Int64 GKEY, string USUARIO)
        {
            string pv;
            var p = InicializaServicio(out pv);
            p.actualMetodo = MethodBase.GetCurrentMethod().Name;
            if (p == null)
            {
                p.LogError<ApplicationException>(new ApplicationException("No fue posible inicializar objeto servicios"), p.actualMetodo, USUARIO);
                //trace log not init
                return Respuesta.ResultadoOperacion<bool>.CrearFalla("La inicialización del objeto N4 falló");
            }
            if (!p.Accesorio.ExistenConfiguraciones)
            {
                return Respuesta.ResultadoOperacion<bool>.CrearFalla("No existen configuraciones");
            }
            var evento = p.Accesorio.ObtenerConfiguracion("PASE_VENCIDO_BRBK")?.valor;
            var notas = p.Accesorio.ObtenerConfiguracion("NOTA_EVENTO_BRBK")?.valor;
            N4Ws.Entidad.GroovyCodeExtension code = new N4Ws.Entidad.GroovyCodeExtension();
            code.name = "CGSAUnitEventBBKWS";
            code.location = "code-extension";
            code.parameters.Add("UNIT", GKEY.ToString());
            code.parameters.Add("USER", USUARIO);
            code.parameters.Add("NOTES", notas);
            code.parameters.Add("DATE", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            code.parameters.Add("EVENT", evento);
            //Poner el evento
            var n4r = N4Ws.Entidad.Servicios.EjecutarCODEExtensionGenerico(code, USUARIO);
            if (n4r.status != 1)
            {
                var ex = new ApplicationException(n4r.status_id);
                var i = p.LogError<ApplicationException>(ex, "PonerEventoPasePuertaBRBK", USUARIO);
                var emsg = string.Format("Ha ocurrido la novedad número {0} durante el proceso favor comuníquese con facturacion", i.HasValue ? i.Value : -1);
                return Respuesta.ResultadoOperacion<bool>.CrearFalla(emsg);
            }
            return Respuesta.ResultadoOperacion<bool>.CrearResultadoExitoso(true, "Éxito al aplicar el evento");
        }

        public static N4_BasicResponse EjecutarHPU(List<string> unidades, Dictionary<string, bool> holds)
        {
            var n = new N4_BasicResponse();
            //paso 1-> Inicializar instancia de servicio
            string pv;
            var p = InicializaServicio(out pv);
            p.actualMetodo = MethodBase.GetCurrentMethod().Name;
            if (p == null)
            {
                n.status = 3;
                n.status_id = "SEVERE";
                n.messages.Add(new N4_response_message("NO_INITIALIZED_DATA", "SEVERE", pv));
                return n;
            }
            //paso 2 -> inicializar instancia de n4configurariones
            var n4 = ObtenerInicializador(p, out pv);
            if (n4 == null)
            {
                n.status = 3;
                n.status_id = "SEVERE";
                n.messages.Add(new N4_response_message("NO_INITIALIZED_N4_INSTANCE", "SEVERE", pv));
                return n;
            }

            if (unidades == null || unidades.Count <= 0)
            {
                n.status = 3;
                n.status_id = "SEVERE";
                n.messages.Add(new N4_response_message("NO_UNITS_FOR_APPLY_ACTION", "SEVERE", pv));
                return n;
            }

            var rpu = new hpu();
            unidades.ForEach(u => {
                rpu.entities.Add(new unit() { id = u });
            });
            foreach (var h in holds)
            {
                rpu.flags.Add(new flag() { hold_perm_id = h.Key, action= h.Value? _Action.APPLY_HOLD:_Action.RELEASE_HOLD });
            }
            var nbb = N4Basic.GetInstance(n4.usuario, n4.password, n4.url, n4.scope);
            var gs = rpu.ToString();
            n = nbb.BasicInvokeService(gs, p.myClase, p.actualMetodo, "admin", 7000);
            return n;
        }


        public static N4_BasicResponse EjecutarGroovyHoras(PowerLineHour Item, string usuario)
        {
            var n = new N4_BasicResponse();
            //paso 1-> Inicializar instancia de servicio
            string pv;
            var p = InicializaServicio(out pv);
            p.actualMetodo = MethodBase.GetCurrentMethod().Name;
            if (p == null)
            {
                n.status = 3;
                n.status_id = "SEVERE";
                n.messages.Add(new N4_response_message("NO_INITIALIZED_DATA", "SEVERE", pv));
                return n;
            }
            //paso 2 -> inicializar instancia de n4configurariones
            var n4 = ObtenerInicializador(p, out pv);
            if (n4 == null)
            {
                n.status = 3;
                n.status_id = "SEVERE";
                n.messages.Add(new N4_response_message("NO_INITIALIZED_N4_INSTANCE", "SEVERE", pv));
                return n;
            }
            var nbb = N4Basic.GetInstance(n4.usuario, n4.password, n4.url, n4.scope);
            var gs = Item.ToString();
            n = nbb.BasicInvokeService(gs, p.myClase, p.actualMetodo, "admin", 7000);

            var fir = n.response.Descendants().Where(nf => nf.Name.LocalName == "result").FirstOrDefault();
            if (fir != null && !string.IsNullOrEmpty(fir.Value))
            {
                if (!fir.Value.Contains("OK"))
                {
                    n.status = 3;
                    n.status_id = fir.Value;
                }
            }

            return n;
        }


        //nuevo para eventos carbono neutro contenedor
        public static Respuesta.ResultadoOperacion<bool> PonerEventoCarbonoNeutro(List<string> unidades, string usuario)
        {
            string pv;
            var p = InicializaServicio(out pv);
            p.actualMetodo = MethodBase.GetCurrentMethod().Name;
            if (p == null)
            {
                p.LogError<ApplicationException>(new ApplicationException("No fue posible inicializar objeto servicios"), p.actualMetodo, usuario);
                //trace log not init
                return Respuesta.ResultadoOperacion<bool>.CrearFalla("La inicialización del objeto N4 falló");
            }

            if (!p.Accesorio.ExistenConfiguraciones)
            {
                return Respuesta.ResultadoOperacion<bool>.CrearFalla("No existen configuraciones");
            }

            if (unidades == null || unidades.Count <= 0)
            {
                return Respuesta.ResultadoOperacion<bool>.CrearFalla("No existen unidades para agregar evento de carbono neutro");
            }


            var evento = p.Accesorio.ObtenerConfiguracion("CERT_CARBONO")?.valor;
            var notas = p.Accesorio.ObtenerConfiguracion("NOTA_CERTIFICADO")?.valor;
            var propiedad = p.Accesorio.ObtenerConfiguracion("PROPIEDAD")?.valor;
            var tipo = p.Accesorio.ObtenerConfiguracion("TIPO")?.valor;

            var icu = new ICU_API();
            icu.evento = new ICU_EVENT();
            icu.evento.id = evento;
            icu.evento.timeeventapplied = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
            icu.evento.userid = usuario;
            icu.evento.note = notas;



            icu.units = new List<ICU_UNIT>();

            //agreagar conteendores
            unidades.ForEach(u => {
                icu.units.Add(new ICU_UNIT() { id = u, type = tipo });
            });

            icu.properties = new List<ICU_PROPERTY>();
            icu.properties.Add(new ICU_PROPERTY() { tag = propiedad, value = notas });

            var tr = N4ICUService(icu, usuario);
            if (tr.status > 2)
            {
                return Respuesta.ResultadoOperacion<bool>.CrearFalla(tr.status_id);
            }


            return Respuesta.ResultadoOperacion<bool>.CrearResultadoExitoso(true, "Éxito al aplicar el evento");
        }

        //nuevo para eventos carbono neutro cfs
        public static Respuesta.ResultadoOperacion<bool> PonerEventoCarbonoNeutroCFS(Int64 id_unidades, string usuario)
        {
            string pv;
            var p = InicializaServicio(out pv);
            p.actualMetodo = MethodBase.GetCurrentMethod().Name;
            if (p == null)
            {
                p.LogError<ApplicationException>(new ApplicationException("No fue posible inicializar objeto servicios"), p.actualMetodo, usuario);
                //trace log not init
                return Respuesta.ResultadoOperacion<bool>.CrearFalla("La inicialización del objeto N4 falló");
            }

            if (!p.Accesorio.ExistenConfiguraciones)
            {
                return Respuesta.ResultadoOperacion<bool>.CrearFalla("No existen configuraciones");
            }


            var evento = p.Accesorio.ObtenerConfiguracion("CERT_CARBONO_CFS")?.valor;
            var notas = p.Accesorio.ObtenerConfiguracion("NOTA_CERTIFICADO_CFS")?.valor;

            N4Ws.Entidad.GroovyCodeExtension code = new N4Ws.Entidad.GroovyCodeExtension();
            code.name = "CGSAUnitEventBBKWS";
            code.location = "code-extension";
            code.parameters.Add("UNIT", id_unidades.ToString());
            code.parameters.Add("USER", usuario);
            code.parameters.Add("NOTES", notas);
            code.parameters.Add("DATE", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            code.parameters.Add("EVENT", evento);

            //Poner el evento
            var n4r = N4Ws.Entidad.Servicios.EjecutarCODEExtensionGenerico(code, usuario);
            if (n4r.status != 1)
            {
                var ex = new ApplicationException(n4r.status_id);
                var i = p.LogError<ApplicationException>(ex, "PonerEventoCarbonoNeutroCFS", usuario);
                var emsg = string.Format("Ha ocurrido la novedad número {0} durante el proceso favor comuníquese con facturacion", i.HasValue ? i.Value : -1);
                return Respuesta.ResultadoOperacion<bool>.CrearFalla(emsg);
            }

            return Respuesta.ResultadoOperacion<bool>.CrearResultadoExitoso(true, "Éxito al aplicar el evento");
        }

        //nuevo para eventos appcgsa contenedor
        public static Respuesta.ResultadoOperacion<bool> PonerEventoAppCgsa(List<string> unidades, string usuario)
        {
            string pv;
            var p = InicializaServicio(out pv);
            p.actualMetodo = MethodBase.GetCurrentMethod().Name;
            if (p == null)
            {
                p.LogError<ApplicationException>(new ApplicationException("No fue posible inicializar objeto servicios"), p.actualMetodo, usuario);
                //trace log not init
                return Respuesta.ResultadoOperacion<bool>.CrearFalla("La inicialización del objeto N4 falló");
            }

            if (!p.Accesorio.ExistenConfiguraciones)
            {
                return Respuesta.ResultadoOperacion<bool>.CrearFalla("No existen configuraciones");
            }

            if (unidades == null || unidades.Count <= 0)
            {
                return Respuesta.ResultadoOperacion<bool>.CrearFalla("No existen unidades para agregar evento de appcgsa contenedor");
            }


            var evento = p.Accesorio.ObtenerConfiguracion("SERV_APPCGSA")?.valor;
            var notas = p.Accesorio.ObtenerConfiguracion("NOTA_APPCGSA")?.valor;
            var propiedad = p.Accesorio.ObtenerConfiguracion("PROPIEDAD")?.valor;
            var tipo = p.Accesorio.ObtenerConfiguracion("TIPO")?.valor;

            var icu = new ICU_API();
            icu.evento = new ICU_EVENT();
            icu.evento.id = evento;
            icu.evento.timeeventapplied = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
            icu.evento.userid = usuario;
            icu.evento.note = notas;



            icu.units = new List<ICU_UNIT>();

            //agreagar conteendores
            unidades.ForEach(u => {
                icu.units.Add(new ICU_UNIT() { id = u, type = tipo });
            });

            icu.properties = new List<ICU_PROPERTY>();
            icu.properties.Add(new ICU_PROPERTY() { tag = propiedad, value = notas });

            var tr = N4ICUService(icu, usuario);
            if (tr.status > 2)
            {
                return Respuesta.ResultadoOperacion<bool>.CrearFalla(tr.status_id);
            }


            return Respuesta.ResultadoOperacion<bool>.CrearResultadoExitoso(true, "Éxito al aplicar el evento");
        }

        //nuevo para eventos appcgsa  cfs
        public static Respuesta.ResultadoOperacion<bool> PonerEventoAppCgsaCFS(List<string> unidades, string usuario)
        {
            string pv;
            var p = InicializaServicio(out pv);
            p.actualMetodo = MethodBase.GetCurrentMethod().Name;
            if (p == null)
            {
                p.LogError<ApplicationException>(new ApplicationException("No fue posible inicializar objeto servicios"), p.actualMetodo, usuario);
                //trace log not init
                return Respuesta.ResultadoOperacion<bool>.CrearFalla("La inicialización del objeto N4 falló");
            }

            if (!p.Accesorio.ExistenConfiguraciones)
            {
                return Respuesta.ResultadoOperacion<bool>.CrearFalla("No existen configuraciones");
            }

            if (unidades == null || unidades.Count <= 0)
            {
                return Respuesta.ResultadoOperacion<bool>.CrearFalla("No existen unidades para agregar evento de appcgsa cfs");
            }


            var evento = p.Accesorio.ObtenerConfiguracion("SERV_APPCGSACFS")?.valor;
            var notas = p.Accesorio.ObtenerConfiguracion("NOTA_APPCGSA")?.valor;
            var propiedad = p.Accesorio.ObtenerConfiguracion("PROPIEDAD")?.valor;
            var tipo = p.Accesorio.ObtenerConfiguracion("TIPO")?.valor;

            var icu = new ICU_API();
            icu.evento = new ICU_EVENT();
            icu.evento.id = evento;
            icu.evento.timeeventapplied = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
            icu.evento.userid = usuario;
            icu.evento.note = notas;



            icu.units = new List<ICU_UNIT>();

            //agreagar conteendores
            unidades.ForEach(u => {
                icu.units.Add(new ICU_UNIT() { id = u, type = tipo });
            });

            icu.properties = new List<ICU_PROPERTY>();
            icu.properties.Add(new ICU_PROPERTY() { tag = propiedad, value = notas });

            var tr = N4ICUService(icu, usuario);
            if (tr.status > 2)
            {
                return Respuesta.ResultadoOperacion<bool>.CrearFalla(tr.status_id);
            }


            return Respuesta.ResultadoOperacion<bool>.CrearResultadoExitoso(true, "Éxito al aplicar el evento");
        }


        public static Respuesta.ResultadoOperacion<bool> PonerEventoAppCgsaCFSNinal(Int64 id_unidades, string usuario)
        {
            string pv;
            var p = InicializaServicio(out pv);
            p.actualMetodo = MethodBase.GetCurrentMethod().Name;
            if (p == null)
            {
                p.LogError<ApplicationException>(new ApplicationException("No fue posible inicializar objeto servicios"), p.actualMetodo, usuario);
                //trace log not init
                return Respuesta.ResultadoOperacion<bool>.CrearFalla("La inicialización del objeto N4 falló");
            }

            if (!p.Accesorio.ExistenConfiguraciones)
            {
                return Respuesta.ResultadoOperacion<bool>.CrearFalla("No existen configuraciones");
            }


            var evento = p.Accesorio.ObtenerConfiguracion("SERV_APPCGSACFS")?.valor;
            var notas = p.Accesorio.ObtenerConfiguracion("NOTA_APPCGSA")?.valor;

            N4Ws.Entidad.GroovyCodeExtension code = new N4Ws.Entidad.GroovyCodeExtension();
            code.name = "CGSAUnitEventBBKWS";
            code.location = "code-extension";
            code.parameters.Add("UNIT", id_unidades.ToString());
            code.parameters.Add("USER", usuario);
            code.parameters.Add("NOTES", notas);
            code.parameters.Add("DATE", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            code.parameters.Add("EVENT", evento);

            //Poner el evento
            var n4r = N4Ws.Entidad.Servicios.EjecutarCODEExtensionGenerico(code, usuario);
            if (n4r.status != 1)
            {
                var ex = new ApplicationException(n4r.status_id);
                var i = p.LogError<ApplicationException>(ex, "PonerEventoAppCgsaCFSNinal", usuario);
                var emsg = string.Format("Ha ocurrido la novedad número {0} durante el proceso favor comuníquese con facturacion", i.HasValue ? i.Value : -1);
                return Respuesta.ResultadoOperacion<bool>.CrearFalla(emsg);
            }

            return Respuesta.ResultadoOperacion<bool>.CrearResultadoExitoso(true, "Éxito al aplicar el evento");
        }

        //nuevo para eventos carbono neutro contenedor de exportacion
        public static Respuesta.ResultadoOperacion<bool> PonerEventoCarbonoNeutroExpo(List<string> unidades, string usuario)
        {
            string pv;
            var p = InicializaServicio(out pv);
            p.actualMetodo = MethodBase.GetCurrentMethod().Name;
            if (p == null)
            {
                p.LogError<ApplicationException>(new ApplicationException("No fue posible inicializar objeto servicios"), p.actualMetodo, usuario);
                //trace log not init
                return Respuesta.ResultadoOperacion<bool>.CrearFalla("La inicialización del objeto N4 falló");
            }

            if (!p.Accesorio.ExistenConfiguraciones)
            {
                return Respuesta.ResultadoOperacion<bool>.CrearFalla("No existen configuraciones");
            }

            if (unidades == null || unidades.Count <= 0)
            {
                return Respuesta.ResultadoOperacion<bool>.CrearFalla("No existen unidades para agregar evento de carbono neutro exportación");
            }


            var evento = p.Accesorio.ObtenerConfiguracion("CERT_CARBONO_EXPO")?.valor;
            var notas = p.Accesorio.ObtenerConfiguracion("NOTA_CERTIFICADO_EXPO")?.valor;
            var propiedad = p.Accesorio.ObtenerConfiguracion("PROPIEDAD_EXPO")?.valor;
            var tipo = p.Accesorio.ObtenerConfiguracion("TIPO_EXPO")?.valor;

            var icu = new ICU_API();
            icu.evento = new ICU_EVENT();
            icu.evento.id = evento;
            icu.evento.timeeventapplied = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
            icu.evento.userid = usuario;
            icu.evento.note = notas;



            icu.units = new List<ICU_UNIT>();

            //agreagar conteendores
            unidades.ForEach(u => {
                icu.units.Add(new ICU_UNIT() { id = u, type = tipo });
            });

            icu.properties = new List<ICU_PROPERTY>();
            icu.properties.Add(new ICU_PROPERTY() { tag = propiedad, value = notas });

            var tr = N4ICUService(icu, usuario);
            if (tr.status > 2)
            {
                return Respuesta.ResultadoOperacion<bool>.CrearFalla(tr.status_id);
            }


            return Respuesta.ResultadoOperacion<bool>.CrearResultadoExitoso(true, "Éxito al aplicar el evento");
        }

        //nuevo para evento de transporte P2D
        public static Respuesta.ResultadoOperacion<bool> PonerEvento_P2D(Int64 id_unidades, string usuario, decimal qty, int tipo)
        {
            string pv;
            var p = InicializaServicio(out pv);
            p.actualMetodo = MethodBase.GetCurrentMethod().Name;
            if (p == null)
            {
                p.LogError<ApplicationException>(new ApplicationException("No fue posible inicializar objeto servicios"), p.actualMetodo, usuario);
                //trace log not init
                return Respuesta.ResultadoOperacion<bool>.CrearFalla("La inicialización del objeto N4 falló");
            }

            if (!p.Accesorio.ExistenConfiguraciones)
            {
                return Respuesta.ResultadoOperacion<bool>.CrearFalla("No existen configuraciones");
            }
            //normal
            var evento = string.Empty;

            if (tipo == 0)
            {
                 evento = p.Accesorio.ObtenerConfiguracion("P2D_TRANSPORTE")?.valor;
            }
            else
            {
                 evento = p.Accesorio.ObtenerConfiguracion("P2D_TRANSPORTE_EXPRESS")?.valor;
            }

            //var evento = p.Accesorio.ObtenerConfiguracion("P2D_TRANSPORTE")?.valor;
            var notas = p.Accesorio.ObtenerConfiguracion("P2D_NOTA")?.valor;

            N4Ws.Entidad.GroovyCodeExtension code = new N4Ws.Entidad.GroovyCodeExtension();
            code.name = "CGSAUnitEventBBKWSQT";
            code.location = "code-extension";
            code.parameters.Add("UNIT", id_unidades.ToString());
            code.parameters.Add("USER", usuario);
            code.parameters.Add("NOTES", notas);
            code.parameters.Add("QTY", qty.ToString("N2"));
            code.parameters.Add("DATE", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            code.parameters.Add("EVENT", evento);

            //Poner el evento
            var n4r = N4Ws.Entidad.Servicios.EjecutarCODEExtensionGenerico(code, usuario);
            if (n4r.status != 1)
            {
                var ex = new ApplicationException(n4r.status_id);
                var i = p.LogError<ApplicationException>(ex, "PonerEvento_P2D", usuario);
                var emsg = string.Format("Ha ocurrido la novedad número {0} durante el proceso favor comuníquese con facturacion", i.HasValue ? i.Value : -1);
                return Respuesta.ResultadoOperacion<bool>.CrearFalla(emsg);
            }

            return Respuesta.ResultadoOperacion<bool>.CrearResultadoExitoso(true, "Éxito al aplicar el evento");
        }

        //nuevo para eventos carbono neutro break bulk
        public static Respuesta.ResultadoOperacion<bool> PonerEventoCarbonoNeutroBrbk(Int64 id_unidades, string usuario)
        {
            string pv;
            var p = InicializaServicio(out pv);
            p.actualMetodo = MethodBase.GetCurrentMethod().Name;
            if (p == null)
            {
                p.LogError<ApplicationException>(new ApplicationException("No fue posible inicializar objeto servicios"), p.actualMetodo, usuario);
                //trace log not init
                return Respuesta.ResultadoOperacion<bool>.CrearFalla("La inicialización del objeto N4 falló");
            }

            if (!p.Accesorio.ExistenConfiguraciones)
            {
                return Respuesta.ResultadoOperacion<bool>.CrearFalla("No existen configuraciones");
            }


            var evento = p.Accesorio.ObtenerConfiguracion("CERT_CARBONO_BRBK")?.valor;
            var notas = p.Accesorio.ObtenerConfiguracion("NOTA_CERTIFICADO_BRBK")?.valor;

            N4Ws.Entidad.GroovyCodeExtension code = new N4Ws.Entidad.GroovyCodeExtension();
            code.name = "CGSAUnitEventBBKWS";
            code.location = "code-extension";
            code.parameters.Add("UNIT", id_unidades.ToString());
            code.parameters.Add("USER", usuario);
            code.parameters.Add("NOTES", notas);
            code.parameters.Add("DATE", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            code.parameters.Add("EVENT", evento);

            //Poner el evento
            var n4r = N4Ws.Entidad.Servicios.EjecutarCODEExtensionGenerico(code, usuario);
            if (n4r.status != 1)
            {
                var ex = new ApplicationException(n4r.status_id);
                var i = p.LogError<ApplicationException>(ex, "PonerEventoCarbonoNeutrBrbk", usuario);
                var emsg = string.Format("Ha ocurrido la novedad número {0} durante el proceso favor comuníquese con facturacion", i.HasValue ? i.Value : -1);
                return Respuesta.ResultadoOperacion<bool>.CrearFalla(emsg);
            }

            return Respuesta.ResultadoOperacion<bool>.CrearResultadoExitoso(true, "Éxito al aplicar el evento");
        }

        //nuevo appcgsa break bulk
        public static Respuesta.ResultadoOperacion<bool> PonerEventoAppCgsaBrbk(Int64 id_unidades, string usuario)
        {
            string pv;
            var p = InicializaServicio(out pv);
            p.actualMetodo = MethodBase.GetCurrentMethod().Name;
            if (p == null)
            {
                p.LogError<ApplicationException>(new ApplicationException("No fue posible inicializar objeto servicios"), p.actualMetodo, usuario);
                //trace log not init
                return Respuesta.ResultadoOperacion<bool>.CrearFalla("La inicialización del objeto N4 falló");
            }

            if (!p.Accesorio.ExistenConfiguraciones)
            {
                return Respuesta.ResultadoOperacion<bool>.CrearFalla("No existen configuraciones");
            }


            var evento = p.Accesorio.ObtenerConfiguracion("SERV_APPCGSABRBK")?.valor;
            var notas = p.Accesorio.ObtenerConfiguracion("NOTA_APPCGSA_BRBK")?.valor;

            N4Ws.Entidad.GroovyCodeExtension code = new N4Ws.Entidad.GroovyCodeExtension();
            code.name = "CGSAUnitEventBBKWS";
            code.location = "code-extension";
            code.parameters.Add("UNIT", id_unidades.ToString());
            code.parameters.Add("USER", usuario);
            code.parameters.Add("NOTES", notas);
            code.parameters.Add("DATE", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            code.parameters.Add("EVENT", evento);

            //Poner el evento
            var n4r = N4Ws.Entidad.Servicios.EjecutarCODEExtensionGenerico(code, usuario);
            if (n4r.status != 1)
            {
                var ex = new ApplicationException(n4r.status_id);
                var i = p.LogError<ApplicationException>(ex, "PonerEventoAppCgsaBrbk", usuario);
                var emsg = string.Format("Ha ocurrido la novedad número {0} durante el proceso favor comuníquese con facturación", i.HasValue ? i.Value : -1);
                return Respuesta.ResultadoOperacion<bool>.CrearFalla(emsg);
            }

            return Respuesta.ResultadoOperacion<bool>.CrearResultadoExitoso(true, "Éxito al aplicar el evento");
        }

        //nuevo para evento de Evento Inspección Subacuática
        public static Respuesta.ResultadoOperacion<bool> PonerEvento_InspSubAcuatica(string VISIT, string EVENTO_NAVE, string usuario, int qty)
        {
            string pv;
            var p = InicializaServicio(out pv);
            p.actualMetodo = MethodBase.GetCurrentMethod().Name;
            if (p == null)
            {
                p.LogError<ApplicationException>(new ApplicationException("No fue posible inicializar objeto servicios"), p.actualMetodo, usuario);
                //trace log not init
                return Respuesta.ResultadoOperacion<bool>.CrearFalla("La inicialización del objeto N4 falló");
            }

            if (!p.Accesorio.ExistenConfiguraciones)
            {
                return Respuesta.ResultadoOperacion<bool>.CrearFalla("No existen configuraciones");
            }


         
            N4Ws.Entidad.GroovyCodeExtension code = new N4Ws.Entidad.GroovyCodeExtension();
            code.name = "CGSAVesselVisitRecordEventWS";
            code.location = "code-extension";
            code.parameters.Add("VISIT", VISIT);
            code.parameters.Add("EVENT", EVENTO_NAVE);
            code.parameters.Add("QTY", qty.ToString("N0"));
             
            //Poner el evento
            var n4r = N4Ws.Entidad.Servicios.EjecutarCODEExtensionGenerico(code, usuario);
            if (n4r.status != 1)
            {
                var ex = new ApplicationException(n4r.status_id);
                var i = p.LogError<ApplicationException>(ex, "PonerEvento_InspSubAcuatica", usuario);
                var emsg = string.Format("Ha ocurrido la novedad número {0} durante el proceso favor comuníquese con facturacion", i.HasValue ? i.Value : -1);
                return Respuesta.ResultadoOperacion<bool>.CrearFalla(emsg);
            }

            return Respuesta.ResultadoOperacion<bool>.CrearResultadoExitoso(true, "Éxito al aplicar el evento");
        }

        //nuevo para evento de Evento Inspección Subacuática
        public static Respuesta.ResultadoOperacion<bool> Actualizar_Vessel_Visit(string VISIT, string ETB, string HOUR , string EBB, string IMRN, string OMRN, 
            string IVYG,  string OVYG, string usuario, string TIPO, string BANANO, string EMBARQUE)
        {
            string pv;
            var p = InicializaServicio(out pv);
            p.actualMetodo = MethodBase.GetCurrentMethod().Name;
            if (p == null)
            {
                p.LogError<ApplicationException>(new ApplicationException("No fue posible inicializar objeto servicios"), p.actualMetodo, usuario);
                //trace log not init
                return Respuesta.ResultadoOperacion<bool>.CrearFalla("La inicialización del objeto N4 falló");
            }

            if (!p.Accesorio.ExistenConfiguraciones)
            {
                return Respuesta.ResultadoOperacion<bool>.CrearFalla("No existen configuraciones");
            }



            N4Ws.Entidad.GroovyCodeExtension code = new N4Ws.Entidad.GroovyCodeExtension();
            code.name = "CGSAVesselVisitUpdateWS";
            code.location = "code-extension";
            code.parameters.Add("VISIT", VISIT);
            code.parameters.Add("ETB", ETB);
            code.parameters.Add("HOUR", HOUR);
            code.parameters.Add("EBB", EBB);
            code.parameters.Add("IMRN", IMRN);
            code.parameters.Add("OMRN", OMRN);
            code.parameters.Add("IVYG", IVYG);
            code.parameters.Add("OVYG", OVYG);
            code.parameters.Add("USER", usuario);
            code.parameters.Add("SCL", TIPO);
            code.parameters.Add("BCT", BANANO);
            code.parameters.Add("PLC", EMBARQUE);

            //Poner el evento
            var n4r = N4Ws.Entidad.Servicios.EjecutarCODEExtensionGenerico(code, usuario);
            if (n4r.status != 1)
            {
                var ex = new ApplicationException(n4r.status_id);
                var i = p.LogError<ApplicationException>(ex, "Actualizar_Vessel_Visit", usuario);
                var emsg = string.Format("Ha ocurrido la novedad número {0} durante el proceso de actualizar vessel visit", i.HasValue ? i.Value : -1);
                return Respuesta.ResultadoOperacion<bool>.CrearFalla(emsg);
            }
            else
            {
                XDocument xds = XDocument.Parse(n4r.response.ToString());

                var result = from Mensaje in xds.Descendants("argo-response")
                             select new
                             {
                                 result = Mensaje.Element("result").Value
                             };

                if (result != null)
                {
                    foreach (var item in result)
                    {
                        
                        int nResult = 0;
                        if (!int.TryParse(item.result, out nResult))
                        {

                            var ex = new ApplicationException(n4r.status_id);
                            var i = p.LogError<ApplicationException>(ex, "Actualizar_Vessel_Visit", usuario);
                            var emsg = string.Format("Ha ocurrido la novedad número {0} durante el proceso de actualizar vessel visit. {1}", i.HasValue ? i.Value : -1, item.result);
                            return Respuesta.ResultadoOperacion<bool>.CrearFalla(emsg);

                        }
                    }
                }

            }

            return Respuesta.ResultadoOperacion<bool>.CrearResultadoExitoso(true, "Éxito al aplicar el evento");
        }


        //nuevo para evento de transporte P2D
        public static Respuesta.ResultadoOperacion<bool> PonerEvento_MultiDespacho(Int64 id_unidades, string usuario, decimal qty)
        {
            string pv;
            var p = InicializaServicio(out pv);
            p.actualMetodo = MethodBase.GetCurrentMethod().Name;
            if (p == null)
            {
                p.LogError<ApplicationException>(new ApplicationException("No fue posible inicializar objeto servicios"), p.actualMetodo, usuario);
                //trace log not init
                return Respuesta.ResultadoOperacion<bool>.CrearFalla("La inicialización del objeto N4 falló");
            }

            if (!p.Accesorio.ExistenConfiguraciones)
            {
                return Respuesta.ResultadoOperacion<bool>.CrearFalla("No existen configuraciones");
            }
            //normal
            var evento = string.Empty;

            evento = p.Accesorio.ObtenerConfiguracion("MULTIDESPACHO")?.valor;
            
            var notas = p.Accesorio.ObtenerConfiguracion("MULTIDESPACHO_NOTA")?.valor;

            N4Ws.Entidad.GroovyCodeExtension code = new N4Ws.Entidad.GroovyCodeExtension();
            code.name = "CGSAUnitEventBBKWSQT";
            code.location = "code-extension";
            code.parameters.Add("UNIT", id_unidades.ToString());
            code.parameters.Add("USER", usuario);
            code.parameters.Add("NOTES", notas);
            code.parameters.Add("QTY", qty.ToString("N2"));
            code.parameters.Add("DATE", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            code.parameters.Add("EVENT", evento);

            //Poner el evento
            var n4r = N4Ws.Entidad.Servicios.EjecutarCODEExtensionGenerico(code, usuario);
            if (n4r.status != 1)
            {
                var ex = new ApplicationException(n4r.status_id);
                var i = p.LogError<ApplicationException>(ex, "PonerEvento_MultiDespacho", usuario);
                var emsg = string.Format("Ha ocurrido la novedad número {0} durante el proceso favor comuníquese con facturacion", i.HasValue ? i.Value : -1);
                return Respuesta.ResultadoOperacion<bool>.CrearFalla(emsg);
            }

            return Respuesta.ResultadoOperacion<bool>.CrearResultadoExitoso(true, "Éxito al aplicar el evento");
        }


        //nuevo para eventos damage control
        public static Respuesta.ResultadoOperacion<bool> PonerEventoDamageControl(List<string> unidades, string usuario)
        {
            string pv;
            var p = InicializaServicio(out pv);
            p.actualMetodo = MethodBase.GetCurrentMethod().Name;
            if (p == null)
            {
                p.LogError<ApplicationException>(new ApplicationException("No fue posible inicializar objeto servicios"), p.actualMetodo, usuario);
                //trace log not init
                return Respuesta.ResultadoOperacion<bool>.CrearFalla("La inicialización del objeto N4 falló");
            }

            if (!p.Accesorio.ExistenConfiguraciones)
            {
                return Respuesta.ResultadoOperacion<bool>.CrearFalla("No existen configuraciones");
            }

            if (unidades == null || unidades.Count <= 0)
            {
                return Respuesta.ResultadoOperacion<bool>.CrearFalla("No existen unidades para agregar evento de Imágenes Damage Control");
            }


            var evento = p.Accesorio.ObtenerConfiguracion("DAMAGE_IMAGENES")?.valor;
            var notas = p.Accesorio.ObtenerConfiguracion("DAMAGE_NOTA")?.valor;
            var propiedad = p.Accesorio.ObtenerConfiguracion("DAMAGE_PROPIEDAD")?.valor;
            var tipo = p.Accesorio.ObtenerConfiguracion("DAMAGE_TIPO")?.valor;

            var icu = new ICU_API();
            icu.evento = new ICU_EVENT();
            icu.evento.id = evento;
            icu.evento.timeeventapplied = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
            icu.evento.userid = usuario;
            icu.evento.note = notas;



            icu.units = new List<ICU_UNIT>();

            //agreagar conteendores
            unidades.ForEach(u => {
                icu.units.Add(new ICU_UNIT() { id = u, type = tipo });
            });

            icu.properties = new List<ICU_PROPERTY>();
            icu.properties.Add(new ICU_PROPERTY() { tag = propiedad, value = notas });

            var tr = N4ICUService(icu, usuario);
            if (tr.status > 2)
            {
                return Respuesta.ResultadoOperacion<bool>.CrearFalla(tr.status_id);
            }


            return Respuesta.ResultadoOperacion<bool>.CrearResultadoExitoso(true, "Éxito al aplicar el evento");
        }

        //nuevo para eventos imagenes de sellos contenedor
        public static Respuesta.ResultadoOperacion<bool> PonerEventoImagenesSellos(List<string> unidades, string usuario)
        {
            string pv;
            var p = InicializaServicio(out pv);
            p.actualMetodo = MethodBase.GetCurrentMethod().Name;
            if (p == null)
            {
                p.LogError<ApplicationException>(new ApplicationException("No fue posible inicializar objeto servicios"), p.actualMetodo, usuario);
                //trace log not init
                return Respuesta.ResultadoOperacion<bool>.CrearFalla("La inicialización del objeto N4 falló");
            }

            if (!p.Accesorio.ExistenConfiguraciones)
            {
                return Respuesta.ResultadoOperacion<bool>.CrearFalla("No existen configuraciones");
            }

            if (unidades == null || unidades.Count <= 0)
            {
                return Respuesta.ResultadoOperacion<bool>.CrearFalla("No existen unidades para agregar evento de Imágenes de Sellos");
            }


            var evento = p.Accesorio.ObtenerConfiguracion("SELLOS_EVENTO")?.valor;
            var notas = p.Accesorio.ObtenerConfiguracion("NOTA_SELLO")?.valor;
            var propiedad = p.Accesorio.ObtenerConfiguracion("PROPIEDAD")?.valor;
            var tipo = p.Accesorio.ObtenerConfiguracion("TIPO")?.valor;

            var icu = new ICU_API();
            icu.evento = new ICU_EVENT();
            icu.evento.id = evento;
            icu.evento.timeeventapplied = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
            icu.evento.userid = usuario;
            icu.evento.note = notas;



            icu.units = new List<ICU_UNIT>();

            //agreagar conteendores
            unidades.ForEach(u => {
                icu.units.Add(new ICU_UNIT() { id = u, type = tipo });
            });

            icu.properties = new List<ICU_PROPERTY>();
            icu.properties.Add(new ICU_PROPERTY() { tag = propiedad, value = notas });

            var tr = N4ICUService(icu, usuario);
            if (tr.status > 2)
            {
                return Respuesta.ResultadoOperacion<bool>.CrearFalla(tr.status_id);
            }


            return Respuesta.ResultadoOperacion<bool>.CrearResultadoExitoso(true, "Éxito al aplicar el evento");
        }
    }




}
