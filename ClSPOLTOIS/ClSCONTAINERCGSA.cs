using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Xml;
using System.Xml.Linq;
using ConectorN4;
//using gendatos;

namespace ClSPOLTOIS
{
    public class ClSCONTAINERCGSA
    {
        public DataSet GetContainerCgsaN4(String wxml)
        {

            //Declaracion de Parametros

            DataSet wvresult = new DataSet();
            ClSUTIL wutil = new ClSUTIL();
            DBMPORTALCGSADataContext dbmCgsa = new DBMPORTALCGSADataContext();
            DBMMIDN4DataContext dbmN4 = new DBMMIDN4DataContext();

            String wvalor;
            int wisecuencial = -1;
            String wscontenedor = null;
            String Mrn = null;
            String Msn = null;
            String Hsn = null;
            String Referencia = null;
            String EstadoCntr = null;



            //Obtencion de Valores

            if (wutil.ConsXml("ContainerCgsaN4", "SECUENCIA", wxml, out wvalor) == true)
            {
                wisecuencial = int.Parse(wvalor);
            }
            wutil.ConsXml("ContainerCgsaN4", "CONTENEDOR", wxml, out wscontenedor);
            wutil.ConsXml("ContainerCgsaN4", "MRN", wxml, out Mrn);
            wutil.ConsXml("ContainerCgsaN4", "MSN", wxml, out Msn);
            wutil.ConsXml("ContainerCgsaN4", "HSN", wxml, out Hsn);
            wutil.ConsXml("ContainerCgsaN4", "REFERENCIA", wxml, out Referencia);
            wutil.ConsXml("ContainerCgsaN4", "ESTADOCNTR", wxml, out EstadoCntr);






            //CNTR=CONTENEDRO BRBK=BREAKBUKLL CFS=CFS

            //Consulta Pase a Puerta N4
            var wvqueryn4NCA = (from vbs_t_pase in dbmN4.VBS_T_PASE_PUERTA
                                where vbs_t_pase.ESTADO != "CA" && vbs_t_pase.TIPO_CARGA == "CNTR"
                                select new { vbs_t_pase.ID_PASE, vbs_t_pase.ID_CARGA, vbs_t_pase.TIPO_CARGA, vbs_t_pase.ID_RESERVA, RESERVA = (DateTime.Today.CompareTo(vbs_t_pase.FECHA_EXPIRACION) == -1 ? vbs_t_pase.RESERVA : Boolean.Parse("false")), vbs_t_pase.ESTADO_RESERVA, vbs_t_pase.FECHA_EXPIRACION, }).ToList();
            //Consulta Factura
            var wvqueryn4Factura = (from vbs_Invoice in dbmN4.BIL_INVOICE
                                    where vbs_Invoice.TIPO_CARGA == "CNTR"
                                    group vbs_Invoice by vbs_Invoice.ID_CARGA into grs_Invoice
                                    select new
                                    {
                                        FECHA_FACTURA = grs_Invoice.Max(g => g.FECHA_INGRESO),
                                        CONTENEDOR = grs_Invoice.Key.ToString(),
                                    }).ToList();

            //Consulta Contenedores  Portal Cgsa
            var wvqueryportal = (from wv_csl_bl in dbmCgsa.VW_CSC_BL_CSL
                                 where
                         wv_csl_bl.MNFT_VEPR_REFERENCE == (Referencia == null ? wv_csl_bl.MNFT_VEPR_REFERENCE : Referencia) &&
                         wv_csl_bl.ESTADO_CONTR == (EstadoCntr == null ? wv_csl_bl.ESTADO_CONTR : EstadoCntr) &&
                         wv_csl_bl.MRN == (Mrn == null ? wv_csl_bl.MRN : Mrn) &&
                         wv_csl_bl.MSN == (Msn == null ? wv_csl_bl.MSN : Msn) &&
                         wv_csl_bl.HSN == (Hsn == null ? wv_csl_bl.HSN : Hsn) &&
                         wv_csl_bl.CNTR_CONSECUTIVO == (wisecuencial == -1 ? wv_csl_bl.CNTR_CONSECUTIVO : wisecuencial) &&
                         wv_csl_bl.Contenedor == (wscontenedor == null ? wv_csl_bl.Contenedor : wscontenedor)
                                 select new { wv_csl_bl.Contenedor, wv_csl_bl.CNTR_CONSECUTIVO }).Distinct().ToList();

            //Consulta Container N4 y Cgsa

            var wvqueryresul = (from wportal in wvqueryportal
                                join wn4 in wvqueryn4NCA.Where(n4 => n4.TIPO_CARGA == "CNTR")
                                 on wportal.CNTR_CONSECUTIVO equals wn4.ID_CARGA into temp
                                from wn41 in temp.DefaultIfEmpty()
                                join wfn4 in wvqueryn4Factura
                                on wportal.Contenedor equals wfn4.CONTENEDOR into temp1
                                from wn42 in temp1.DefaultIfEmpty()
                                select new
                                {
                                    ASIGNADO = Boolean.Parse("false"),
                                    ASIGNADOPN = Boolean.Parse("false"),
                                    CONSECUTIVO = (String)wportal.CNTR_CONSECUTIVO.ToString(),
                                    CONTENEDOR = (String)wportal.Contenedor,
                                    FECHA_SALIDA = (String)DateTime.Now.ToString("MM/dd/yyyy"),
                                    TURNO = (String)"",
                                    IDTURNO = (String)"0",
                                    EMPRESA = (String)"",
                                    IDEMPRESA = (String)"",
                                    PLACA = (String)"",
                                    CHOFER = (String)"",
                                    IDCHOFER = (String)"",
                                    CAS = (String)"",
                                    CONPAS = wn41 == null ? Boolean.Parse("false") : Boolean.Parse("true"),
                                    FECHA_FACTURA = wn42 == null ? "" : wn42.FECHA_FACTURA.ToString("MM/dd/yyyy"),

                                }
                                    ).OrderBy(X => X.CONTENEDOR).ToList();


            wvresult.Tables.Add(wutil.LINQToDataTable(wvqueryresul));

            return wvresult;
        }
        public DataSet GetContainerN4(String wxml, String wxmlbooking)
        {
            DataSet wvresult = new DataSet();
            ClSUTIL wutil = new ClSUTIL();

            DBMMIDN4DataContext dbmN4 = new DBMMIDN4DataContext();

            String wvalor;
            int wisecuencial = -1;
            String wscontenedor = null;
            String wscontenedorexpo = null;
            String wsbooking = null;
            String wmjerrorCAS = "";

            String Mrn = null;
            String Msn = null;
            String Hsn = null;
            String Referencia = null;
            String Reefers = null;
            String EstadoCntr = null;
            String Tipo = null;
            DataTable wdtportal = new DataTable();
            Boolean wpase = Boolean.Parse(dbmN4.vbs_fun_vbs_enabled(DateTime.Now.Date).ToString());

            DateTime Wfechas;

            DataTable DTCAS = new DataTable("DTCAS");
            DTCAS.Columns.Add(new DataColumn("FECHA", Type.GetType("System.String")));
            DTCAS.Columns.Add(new DataColumn("CONTENEDOR", Type.GetType("System.String")));
            DTCAS.Columns.Add(new DataColumn("VALIDA_CAS", Type.GetType("System.Boolean")));
            DTCAS.AcceptChanges();


            DateTime.TryParseExact(DateTime.Now.ToString("MM/dd/yyyy"), "MM/dd/yyyy", new System.Globalization.CultureInfo("es-EC"), System.Globalization.DateTimeStyles.None, out Wfechas);
            //quitar//
            dbmN4.CommandTimeout = 600;

            var wpara = (from genc in dbmN4.GEN_C_PARAMETROS
                         where genc.P_TIPO == "CAS_LINEAS" && genc.P_ESTADO == "A"
                         select genc).ToList();


            //Obtencion de Valores



            //CNTR=CONTENEDRO BRBK=BREAKBUKLL CFS=CFS

            //Consulta Pase a Puerta N4
            //var wvqueryn4NCA = (from vbs_t_pase in dbmN4.VBS_T_PASE_PUERTA
            //                    where (vbs_t_pase.ESTADO != "CA" || (vbs_t_pase.RESERVA == Boolean.Parse("true") && vbs_t_pase.ESTADO_RESERVA == Boolean.Parse("false"))) && vbs_t_pase.TIPO_CARGA == "CNTR" && vbs_t_pase.FECHA_EXPIRACION >= Wfechas
            //                    select new { vbs_t_pase.ID_PASE, vbs_t_pase.ID_CARGA, vbs_t_pase.TIPO_CARGA, vbs_t_pase.NUMERO_PASE_N4, vbs_t_pase.ID_RESERVA, RESERVA = (DateTime.Today.CompareTo(vbs_t_pase.FECHA_EXPIRACION) == -1 ? vbs_t_pase.RESERVA : Boolean.Parse("false")), vbs_t_pase.ESTADO_RESERVA, vbs_t_pase.FECHA_EXPIRACION, }).ToList();
            //Consulta Factura
            //var wvqueryn4Factura = (from vbs_Invoice in dbmN4.BIL_INVOICE
            //                        where vbs_Invoice.TIPO_CARGA == "CNTR"
            //                        group vbs_Invoice by vbs_Invoice.ID_CARGA into grs_Invoice
            //                        select new
            //                        {
            //                            FECHA_FACTURA = grs_Invoice.Max(g => g.FECHA_INGRESO),
            //                            CONTENEDOR = grs_Invoice.Key.ToString(),
            //                        }).ToList();



            //wvqueryportal

            if (String.IsNullOrEmpty(wxmlbooking) != true)
            {


                wutil.ConsXml("ConsultaCarga", "CONTENEDOREXPO", wxmlbooking, out wscontenedorexpo);
                wutil.ConsXml("ConsultaCarga", "BOOKING", wxmlbooking, out wsbooking);
                wutil.ConsXml("ConsultaCarga", "REFERENCIA", wxmlbooking, out Referencia);
                wutil.ConsXml("ConsultaCarga", "REEFERS", wxmlbooking, out Reefers);
                
                XDocument xdoc = XDocument.Parse(wxmlbooking);
                IEnumerable<String> wresul = (from invoice in xdoc.Descendants("BOOKING")

                                              select (String)invoice.Attribute("ID_BOOKING").Value);

             

                if (wresul != null && wresul.Count() > 0)
                {

                    var wvqueryportal = (from wv_container in dbmN4.FNA_FUN_CONTAINERS_BILL_N5(null, null, null, wscontenedorexpo, wsbooking, Referencia, Reefers, wxmlbooking)// CONTAINERS_BILL
                                         select new
                                         {
                                             ASIGNADO = Boolean.Parse("false"),
                                             ASIGNADOPN = Boolean.Parse("false"),
                                             NUMERO_PASE = "",
                                             CONSECUTIVO = wv_container.CNTR_CONSECUTIVO,
                                             Contenedor = wv_container.CNTR_CONTAINER,
                                             wv_container.CNTR_CONSECUTIVO,
                                             DOCUMENTO = wv_container.CNTR_DOCUMENT,
                                             MRN = wv_container.CNTR_MRN,
                                             MSN = wv_container.CNTR_MSN,
                                             HSN = wv_container.CNTR_HSN,
                                             NAVE = wv_container.CNTR_VEPR_REFERENCE,
                                             Bloqueo =  Boolean.Parse("false"),//wv_container.CNTR_HOLD,
                                             FECHA_SALIDA = "",
                                             FECHA_FACURA = (String)DateTime.Now.ToString("MM/dd/yyyy"),
                                             TIPO_CARGA = (String)(wv_container.CNTR_CATY_CARGO_TYPE == null ? "" : wv_container.CNTR_CATY_CARGO_TYPE),
                                             TURNO = (String)"",
                                             IDTURNO = (String)"0",
                                             EMPRESA = (String)"",
                                             IDEMPRESA = (String)"",
                                             PLACA = (String)"",
                                             CHOFER = (String)"",
                                             IDCHOFER = (String)"",
                                             CAS = (String)"",
                                             RF = (String)"N",
                                             Tipe_Cart = wv_container.CNTR_CATY_CARGO_TYPE,
                                             ESTADO = wv_container.CNTR_YARD_STATUS,
                                             ESTADON4 = wv_container.CNTR_YARD_STATUS.Equals("IN") ? "YARD" : wv_container.CNTR_YARD_STATUS.Equals("PRE") ? "INBOUND" : "RETIRED",
                                             DDA = wv_container.CNTR_DD,
                                             FEC_CAS = (DateTime?)(wv_container.CNTR_FECHA_CAS.HasValue ? wv_container.CNTR_FECHA_CAS : null),
                                             CONPAS = wv_container.NUMERO_PASE == null ? Boolean.Parse("false") : Boolean.Parse("true"),
                                             LINEA = wv_container.CNTR_CLNT_CUSTOMER_LINE,
                                             MJERRORCAS = (String)wmjerrorCAS,
                                             BL = wv_container.cntr_bl,
                                             ID_RESERVA = wv_container == null ? "0" : wv_container.ID_RESERVA.ToString(),
                                             RESERVA = wv_container == null ? Boolean.Parse("false") : Boolean.Parse("true"),
                                             ULT_FECHA_FACTURA = (DateTime?)(wv_container.ULT_FECHA_FACTURA.HasValue ? wv_container.ULT_FECHA_FACTURA : null),
                                             incas = (Boolean)(wv_container.CNTR_FECHA_CAS.HasValue ? Boolean.Parse("false") : Boolean.Parse("True")),
                                             ACTPASE = wpase,
                                         }).ToList();
                    wvresult.Tables.Add(wutil.LINQToDataTable(wvqueryportal));  

                    //var wvqueryportal = (from wv_container in dbmN4.CONTAINERS_BILL.Where(contenedor => (wresul.Contains(contenedor.CNTR_BKNG_BOOKING)) && contenedor.CNTR_TYPE == "EXPO" && contenedor.CNTR_YARD_STATUS != "PRE" && contenedor.CNTR_LCL_FCL=="FCL")                    
                    //                     where wv_container.CNTR_CATY_CARGO_TYPE  == (String.IsNullOrEmpty(Reefers)?wv_container.CNTR_CATY_CARGO_TYPE:Reefers)
                    //                     select new
                    //                     {
                    //                         Contenedor = wv_container.CNTR_CONTAINER,
                    //                         wv_container.CNTR_CONSECUTIVO,
                    //                         DOCUMENTO = wv_container.CNTR_DOCUMENT,
                    //                         MRN = wv_container.CNTR_MRN,
                    //                         MSN = wv_container.CNTR_MSN,
                    //                         HSN = wv_container.CNTR_HSN,
                    //                         NAVE = wv_container.CNTR_VEPR_REFERENCE,
                    //                         Bloqueo = wv_container.CNTR_HOLD,
                    //                         Tipe_Cart = wv_container.CNTR_CATY_CARGO_TYPE,
                    //                         ESTADO = wv_container.CNTR_YARD_STATUS,
                    //                         ESTADON4 = wv_container.CNTR_YARD_STATUS.Equals("IN") ? "YARD" : wv_container.CNTR_YARD_STATUS.Equals("PRE") ? "INBOUND" : "RETIRED",
                    //                         DDA = wv_container.CNTR_DD,
                    //                         FEC_CAS = (DateTime?)(wv_container.CNTR_FECHA_CAS.HasValue?wv_container.CNTR_FECHA_CAS:null),
                    //                         LINEA = wv_container.CNTR_CLNT_CUSTOMER_LINE,
                    //                         BL = wv_container.cntr_bl,
                    //                         incas = (Boolean)(wv_container.CNTR_FECHA_CAS.HasValue ? Boolean.Parse("false") : Boolean.Parse("True")),
                    //                     }).Distinct().ToList();



                    //wvqueryportal = wvqueryportal.Concat(from wv_container in dbmN4.CONTAINERS_BILL.Where(contenedor => (((contenedor.CNTR_CONTAINER == wscontenedorexpo && contenedor.CNTR_VEPR_REFERENCE.Trim() == Referencia) || contenedor.CNTR_BKNG_BOOKING == wsbooking) && contenedor.CNTR_TYPE == "EXPO" && contenedor.CNTR_YARD_STATUS != "PRE" && contenedor.CNTR_LCL_FCL == "FCL"))
                    //                                      where wv_container.CNTR_CATY_CARGO_TYPE == (String.IsNullOrEmpty(Reefers) ? wv_container.CNTR_CATY_CARGO_TYPE : Reefers)
                    //                     select new
                    //                     {
                    //                         Contenedor = wv_container.CNTR_CONTAINER,
                    //                         wv_container.CNTR_CONSECUTIVO,
                    //                         DOCUMENTO = wv_container.CNTR_DOCUMENT,
                    //                         MRN = wv_container.CNTR_MRN,
                    //                         MSN = wv_container.CNTR_MSN,
                    //                         HSN = wv_container.CNTR_HSN,
                    //                         NAVE = wv_container.CNTR_VEPR_REFERENCE,
                    //                         Bloqueo = wv_container.CNTR_HOLD,
                    //                         Tipe_Cart = wv_container.CNTR_CATY_CARGO_TYPE,
                    //                         ESTADO = wv_container.CNTR_YARD_STATUS,
                    //                         ESTADON4 = wv_container.CNTR_YARD_STATUS.Equals("IN") ? "YARD" : wv_container.CNTR_YARD_STATUS.Equals("PRE") ? "INBOUND" : "RETIRED",
                    //                         DDA = wv_container.CNTR_DD,
                    //                         FEC_CAS = (DateTime?)(wv_container.CNTR_FECHA_CAS.HasValue?wv_container.CNTR_FECHA_CAS:null),
                    //                         LINEA = wv_container.CNTR_CLNT_CUSTOMER_LINE,
                    //                         BL = wv_container.cntr_bl,
                    //                         incas = (Boolean)(wv_container.CNTR_FECHA_CAS.HasValue ? Boolean.Parse("false") : Boolean.Parse("True")),
                    //                     }).Distinct().ToList();





             

                    //if (wvqueryportal != null && wscontenedorexpo != null)
                    //{
                    //    wdtportal = (wutil.LINQToDataTable(wvqueryportal.Take(1)));
                    //}
                    //else
                    //{
                    //    wdtportal = (wutil.LINQToDataTable(wvqueryportal.Distinct().ToList()));
                    //}

                }
                else
                {

                    var wvqueryportal = (from wv_container in dbmN4.FNA_FUN_CONTAINERS_BILL_N5(null, null, null, wscontenedorexpo, wsbooking, Referencia, Reefers, wxmlbooking)// CONTAINERS_BILL
                                         select new
                                         {
                                             ASIGNADO = Boolean.Parse("false"),
                                             ASIGNADOPN = Boolean.Parse("false"),
                                             NUMERO_PASE = "",
                                             Contenedor = wv_container.CNTR_CONTAINER,
                                             CONSECUTIVO = wv_container.CNTR_CONSECUTIVO,
                                             wv_container.CNTR_CONSECUTIVO,
                                             DOCUMENTO = wv_container.CNTR_DOCUMENT,
                                             REFERNCIA = (String)wv_container.CNTR_VEPR_REFERENCE,
                                             FECHA_SALIDA = "",
                                             FECHA_FACURA = (String)DateTime.Now.ToString("MM/dd/yyyy"),
                                             MRN = wv_container.CNTR_MRN,
                                             MSN = wv_container.CNTR_MSN,
                                             HSN = wv_container.CNTR_HSN,
                                             NAVE = wv_container.CNTR_VEPR_REFERENCE,
                                             //Bloqueo = wv_container.CNTR_HOLD,
                                             Bloqueo = Boolean.Parse("false"),
                                             TIPO_CARGA = (String)(wv_container.CNTR_CATY_CARGO_TYPE == null ? "" : wv_container.CNTR_CATY_CARGO_TYPE),
                                             TURNO = (String)"",
                                             IDTURNO = (String)"0",
                                             EMPRESA = (String)"",
                                             IDEMPRESA = (String)"",
                                             PLACA = (String)"",
                                             CHOFER = (String)"",
                                             IDCHOFER = (String)"",
                                             CAS = (String)"",
                                             RF = (String)"N",
                                             Tipe_Cart = wv_container.CNTR_CATY_CARGO_TYPE,
                                             ESTADO = wv_container.CNTR_YARD_STATUS,
                                             ESTADON4 = wv_container.CNTR_YARD_STATUS.Equals("IN") ? "YARD" : wv_container.CNTR_YARD_STATUS.Equals("PRE") ? "INBOUND" : "RETIRED",
                                             DDA = (Boolean)(wv_container.CNTR_DD > 0 ? Boolean.Parse("true") : Boolean.Parse("false")),
                                            CONPAS = wv_container.NUMERO_PASE == null ? Boolean.Parse("false") : Boolean.Parse("true"),
                                             ULT_FECHA_FACTURA = (DateTime?)(wv_container.ULT_FECHA_FACTURA.HasValue ? wv_container.ULT_FECHA_FACTURA : null),
                                             FEC_CAS = (DateTime?)(wv_container.CNTR_FECHA_CAS.HasValue ? wv_container.CNTR_FECHA_CAS : null),
                                             LINEA = wv_container.CNTR_CLNT_CUSTOMER_LINE,
                                             BL = wv_container.cntr_bl,
                                             MJERRORCAS = (String)wmjerrorCAS,
                                             ID_RESERVA = wv_container == null ? "0" : wv_container.ID_RESERVA.ToString(),
                                             RESERVA = wv_container == null ? Boolean.Parse("false") : Boolean.Parse("true"),
                                             incas = (Boolean)(wv_container.CNTR_FECHA_CAS.HasValue ? Boolean.Parse("false") : Boolean.Parse("True")),
                                             ACTPASE = wpase,
                                         }).ToList();
                    wvresult.Tables.Add(wutil.LINQToDataTable(wvqueryportal));  
                    //var wvqueryportal = (from wv_container in dbmN4.CONTAINERS_BILL.Where(contenedor => (contenedor.CNTR_CONTAINER == (wscontenedorexpo == null ? contenedor.CNTR_CONTAINER : wscontenedorexpo) && contenedor.CNTR_BKNG_BOOKING == (wsbooking == null ? contenedor.CNTR_BKNG_BOOKING : wsbooking)) && contenedor.CNTR_TYPE == "EXPO"  && contenedor.CNTR_YARD_STATUS!="PRE" && contenedor.CNTR_VEPR_REFERENCE.Trim() == (Referencia == null ? contenedor.CNTR_VEPR_REFERENCE.Trim() : Referencia))
                    //                     where wv_container.CNTR_CATY_CARGO_TYPE == (String.IsNullOrEmpty(Reefers) ? wv_container.CNTR_CATY_CARGO_TYPE : Reefers)
                    //                     select new
                    //                     {
                    //                         Contenedor = wv_container.CNTR_CONTAINER,
                    //                         wv_container.CNTR_CONSECUTIVO,
                    //                         DOCUMENTO = wv_container.CNTR_DOCUMENT,
                    //                         MRN = wv_container.CNTR_MRN,
                    //                         MSN = wv_container.CNTR_MSN,
                    //                         HSN = wv_container.CNTR_HSN,
                    //                         NAVE = wv_container.CNTR_VEPR_REFERENCE,
                    //                         Bloqueo = wv_container.CNTR_HOLD,
                    //                         Tipe_Cart = wv_container.CNTR_CATY_CARGO_TYPE,               
                    //                         ESTADO = wv_container.CNTR_YARD_STATUS,
                    //                         ESTADON4 = wv_container.CNTR_YARD_STATUS.Equals("IN") ? "YARD" : wv_container.CNTR_YARD_STATUS.Equals("PRE") ? "INBOUND" : "RETIRED",
                    //                         DDA = wv_container.CNTR_DD,
                    //                         FEC_CAS = (DateTime?)(wv_container.CNTR_FECHA_CAS.HasValue?wv_container.CNTR_FECHA_CAS:null),
                    //                         LINEA = wv_container.CNTR_CLNT_CUSTOMER_LINE,
                    //                         BL = wv_container.cntr_bl,
                    //                         incas = (Boolean)(wv_container.CNTR_FECHA_CAS.HasValue ? Boolean.Parse("false") : Boolean.Parse("True")),
                    //                     }).Distinct().ToList();

                    //if (wvqueryportal != null && wscontenedorexpo != null)
                    //{
                    //    wdtportal = (wutil.LINQToDataTable(wvqueryportal.Take(1)));
                    //}
                    //else
                    //{
                    //    wdtportal = (wutil.LINQToDataTable(wvqueryportal));
                    //}
                }
                


                //Consulta Container N4 y Cgsa


            }
            else
                {
                wutil.ConsXml("ConsultaCarga", "CONTENEDOR", wxml, out wscontenedor);
                wutil.ConsXml("ConsultaCarga", "MRN", wxml, out Mrn);
                wutil.ConsXml("ConsultaCarga", "MSN", wxml, out Msn);
                wutil.ConsXml("ConsultaCarga", "HSN", wxml, out Hsn);
                wutil.ConsXml("ConsultaCarga", "Type", wxml, out Tipo);
                wutil.ConsXml("ConsultaCarga", "REFERENCIA", wxml, out Referencia);
                wutil.ConsXml("ConsultaCarga", "ESTADOCNTR", wxml, out EstadoCntr);


                //Consulta Contenedores  Portal Cgsa

                var wvqueryportal = (from wv_container in dbmN4.FNA_FUN_CONTAINERS_BILL_N5(Mrn, Msn, Hsn,null,null,null,null,null)// CONTAINERS_BILL
                                     select new
                                     {
                                         PASEWEB = Boolean.Parse("false"),
                                         ASIGNADO = Boolean.Parse("false"),
                                         ASIGNADOPN = Boolean.Parse("false"),
                                         CONSECUTIVO = wv_container.CNTR_CONSECUTIVO,
                                         CONTENEDOR = (String)wv_container.CNTR_CONTAINER,
                                         DOCUMENTO = (String)wv_container.CNTR_DOCUMENT,
                                         MRN = (String)wv_container.CNTR_MRN,
                                         MSN = (String)wv_container.CNTR_MSN,
                                         HSN = (String)wv_container.CNTR_HSN,
                                         REFERNCIA = (String)wv_container.CNTR_VEPR_REFERENCE,
                                         NAVE = (String)wv_container.CNTR_VEPR_REFERENCE,
                                         FECHA_SALIDA = "",
                                         FECHA_FACURA = (String)DateTime.Now.ToString("MM/dd/yyyy"),
                                         TURNO = (String)"",
                                         IDTURNO = (String)"0",
                                         EMPRESA = (String)"",
                                         IDEMPRESA = (String)"",
                                         PLACA = (String)"",
                                         CHOFER = (String)"",
                                         IDCHOFER = (String)"",
                                         CAS = (String)"",
                                         RF = (String)wv_container.CNTR_REEFER_CONT,
                                         CONPAS = wv_container.NUMERO_PASE == null ? Boolean.Parse("false") : Boolean.Parse("true"),
                                         ULT_FECHA_FACTURA = (DateTime?)(wv_container.ULT_FECHA_FACTURA.HasValue ? wv_container.ULT_FECHA_FACTURA : null),
                                         NUMERO_PASE = wv_container.NUMERO_PASE == null ? "" : wv_container.NUMERO_PASE.ToString(),
                                         TIPO_CARGA = (String)(wv_container.CNTR_CATY_CARGO_TYPE == null ? "" : wv_container.CNTR_CATY_CARGO_TYPE),
                                         Bloqueo = (Boolean)(wv_container.CNTR_HOLD > 0 ? Boolean.Parse("true") : Boolean.Parse("false")),
                                         ESTADO = (String)(wv_container.CNTR_YARD_STATUS == null ? "" : wv_container.CNTR_YARD_STATUS.ToString()),
                                         ESTADON4 = (String)(wv_container.CNTR_YARD_STATUS.Equals("IN") ? "YARD" : wv_container.CNTR_YARD_STATUS.Equals("PRE") ? "INBOUND" : "DEPARTED"),
                                         DDA = (Boolean)(wv_container.CNTR_DD > 0 ? Boolean.Parse("true") : Boolean.Parse("false")),
                                         FEC_CAS = /*(DateTime?)*/(wv_container.CNTR_FECHA_CAS.HasValue ? wv_container.CNTR_FECHA_CAS : null),
                                         VALICAS = (Boolean)(wv_container.VALICAS > 0 ? Boolean.Parse("true") : Boolean.Parse("false")),
                                         //VALICAS = Boolean.Parse("true"),
                                         MJERRORCAS = (String)wmjerrorCAS,
                                         ID_RESERVA = wv_container == null ? "0" : wv_container.ID_RESERVA.ToString(),
                                         RESERVA = wv_container.FECHA_EXPIRACION == 1 ? Boolean.Parse("true") : Boolean.Parse("false"),
                                         //RESERVA = (wv_container.FECHA_EXPIRACION == 1 ? (Boolean)wv_container.RESERVA : Boolean.Parse("false")),

                                         /*
                                         ((DateTime)currentStatRow["FEC_CAS"]).ToString("MM/dd/yyyy hh:mm:ss") 
                                          RESERVA = (DateTime.Today.CompareTo(vbs_t_pase.FECHA_EXPIRACION) == -1 ? vbs_t_pase.RESERVA : Boolean.Parse("false"))
                                          */
                                         //RESERVA = wv_container == null ? Boolean.Parse("false") : wv_container,
                                         ACTPASE = wpase,

                                     }).ToList();

                wvresult.Tables.Add(wutil.LINQToDataTable(wvqueryportal));  
                /*var wvqueryportal = (from wv_container in dbmN4.CONTAINERS_BILL
                                     where
                             wv_container.CNTR_VEPR_REFERENCE.Trim() == (Referencia == null ? wv_container.CNTR_VEPR_REFERENCE.Trim() : Referencia) &&
                             wv_container.CNTR_YARD_STATUS.Trim() == (EstadoCntr == null ? wv_container.CNTR_YARD_STATUS.Trim() : EstadoCntr) &&
                                           wv_container.CNTR_MRN.Trim() == (Mrn == null ? wv_container.CNTR_MRN.Trim() : Mrn) &&
                                          wv_container.CNTR_MSN == (Msn == null ? wv_container.CNTR_MSN : Msn) &&
                                          wv_container.CNTR_HSN == (Hsn == null ? wv_container.CNTR_HSN : Hsn) &&
                                          wv_container.CNTR_TYPE == (Tipo == null ? wv_container.CNTR_TYPE : Tipo) &&
                                         wv_container.CNTR_CONSECUTIVO == (wisecuencial == -1 ? wv_container.CNTR_CONSECUTIVO : wisecuencial) &&
                             wv_container.CNTR_CONTAINER.Trim() == (wscontenedor == null ? wv_container.CNTR_CONTAINER.Trim() : wscontenedor)
                                     select new
                                     {
                                         Contenedor = wv_container.CNTR_CONTAINER,
                                         wv_container.CNTR_CONSECUTIVO,
                                         DOCUMENTO = wv_container.CNTR_DOCUMENT,
                                         MRN = wv_container.CNTR_MRN,
                                         MSN = wv_container.CNTR_MSN,
                                         HSN = wv_container.CNTR_HSN,
                                         NAVE = wv_container.CNTR_VEPR_REFERENCE,
                                         Bloqueo = wv_container.CNTR_HOLD,
                                         Tipe_Cart = wv_container.CNTR_CATY_CARGO_TYPE,
                                         ESTADO = wv_container.CNTR_YARD_STATUS,
                                         ESTADON4 = wv_container.CNTR_YARD_STATUS.Equals("IN") ? "YARD" : wv_container.CNTR_YARD_STATUS.Equals("PRE") ? "INBOUND" : "DEPARTED",
                                         DDA = wv_container.CNTR_DD,
                                         FEC_CAS = (DateTime?)(wv_container.CNTR_FECHA_CAS.HasValue?wv_container.CNTR_FECHA_CAS:null),
                                         LINEA = wv_container.CNTR_CLNT_CUSTOMER_LINE,
                                         BL = wv_container.cntr_bl,
                                         incas = (Boolean)(wv_container.CNTR_FECHA_CAS.HasValue ? Boolean.Parse("false") : Boolean.Parse("True")),
                                     }).Distinct().ToList();

                if (wvqueryportal != null && wscontenedor != null)
                {
                    wdtportal = (wutil.LINQToDataTable(wvqueryportal.Take(1)));
                }
                else
                {


                    var wresultsgrup = (from p in wvqueryportal
                                        group p by new { p.CNTR_CONSECUTIVO, p.Contenedor, p.NAVE, p.MRN, p.MSN, p.Bloqueo, p.Tipe_Cart, p.ESTADO, p.ESTADON4, p.DOCUMENTO, p.DDA,  p.FEC_CAS,p.LINEA,p.BL,p.incas } into grps
                                        select new
                                        {

                                            Contenedor = grps.Key.Contenedor,
                                            CNTR_CONSECUTIVO = grps.Key.CNTR_CONSECUTIVO,
                                            DOCUMENTO = grps.Key.DOCUMENTO,
                                            MRN = grps.Key.MRN,
                                            MSN = grps.Key.MSN,
                                            HSN = grps.Max(g => g.HSN),
                                            NAVE = grps.Key.NAVE,
                                            Bloqueo = grps.Key.Bloqueo,
                                            Tipe_Cart = grps.Key.Tipe_Cart,
                                            ESTADO = grps.Key.ESTADO,
                                            ESTADON4 = grps.Key.ESTADON4,
                                            DDA = grps.Key.DDA,
                                            FEC_CAS = (DateTime?)grps.Key.FEC_CAS,
                                            linea=grps.Key.LINEA,
                                            bl=grps.Key.BL,
                                            incas = grps.Key.incas,
                                        }).ToList();


                    wdtportal = (wutil.LINQToDataTable(wresultsgrup));
                }

                

                var resultfind = (from part in wpara
                                  join wdatacas in wdtportal.AsEnumerable() on part.P_SUBTIPO equals wdatacas.Field<String>("linea")
                                  where wdatacas.Field<Boolean>("incas").Equals(true)
                                  select new
                                  {
                                      proceso = (Boolean)(part.P_VALOR3 == "WEBSERVICE" ? Boolean.Parse("true") : Boolean.Parse("false")),
                                      valida = Boolean.Parse("true"),
                                      web = part.P_VALOR4,
                                      linea = part.P_SUBTIPO,
                                      MRN = wdatacas.Field<String>("MRN"),
                                      bl = wdatacas.Field<String>("bl"),
                                      CONTENEDOR=wdatacas.Field<String>("Contenedor"),
                                  }).ToList();

                foreach( var irsult in resultfind )
                {
                    String PV_MENSAJE = "";
                    bool PV_OK = false;
                    String PV_CODERROR = "";

                    if (irsult.proceso)
                    {
                        try
                        {
                            EcasWS.Ecas_business objecas = new EcasWS.Ecas_business();
                            objecas.WS_Ecas(irsult.web, irsult.CONTENEDOR, irsult.bl, ref PV_MENSAJE, irsult.MRN, irsult.linea, ref PV_OK, ref PV_CODERROR);
                        }
                        catch (Exception ex)
                        {
                            
                        }

                        if (PV_CODERROR == null || !PV_CODERROR.Equals("0"))
                        {
                            wmjerrorCAS = wmjerrorCAS + irsult.CONTENEDOR + " : " + PV_MENSAJE + " || ";
                        }

                    }
                 

                    DTCAS.Rows.Add(new String[] { PV_CODERROR !=null && PV_CODERROR.Equals("0")==true?PV_MENSAJE:"", irsult.CONTENEDOR, irsult.valida.ToString() });
                    DTCAS.AcceptChanges();
                   
                }*/

               
                
                

                
            }

           /* DataTable DTPCas = new DataTable();

            DTPCas = wutil.LINQToDataTable(wpara);

            var wvqueryportalresult = (from portal in wdtportal.AsEnumerable()
                                       join wcvcas in DTPCas.AsEnumerable()
                                on portal.Field<String>("LINEA") equals wcvcas.Field<String>("P_SUBTIPO") into temp3
                                       from wnvlicas in temp3.DefaultIfEmpty()
                                       select new
                                       {
                                           Contenedor = portal.Field<String>("Contenedor"),
                                           CNTR_CONSECUTIVO = portal.Field<long>("CNTR_CONSECUTIVO"),
                                           DOCUMENTO = portal.Field<String>("DOCUMENTO"),
                                           MRN = portal.Field<String>("MRN"),
                                           MSN = portal.Field<String>("MSN"),
                                           HSN = portal.Field<String>("HSN"),
                                           NAVE = portal.Field<String>("NAVE"),
                                           Bloqueo = portal.Field<int>("Bloqueo"),
                                           Tipe_Cart = portal.Field<String>("Tipe_Cart"),
                                           ESTADO = portal.Field<String>("ESTADO"),
                                           ESTADON4 = portal.Field<String>("ESTADON4"),
                                           DDA = portal.Field<int>("DDA"),
                                           FEC_CAS = portal.Field<DateTime?>("FEC_CAS"),
                                           LINEA = portal.Field<String>("linea"),
                                           VALICAS= (Boolean)(wnvlicas != null ? Boolean.Parse("true") : Boolean.Parse("false") )                  
                                       }).ToList();

            var wvcas = (from wcas in DTCAS.AsEnumerable()
                                       select new
                                       {
                                            FECHA = wcas.Field<String>("FECHA"),
                                           CONTENEDOR = wcas.Field<String>("CONTENEDOR"),
                                           VALIDA_CAS = wcas.Field<Boolean>("VALIDA_CAS"),

                                       }).ToList();



            var wvqueryresul = (from wportal in wvqueryportalresult
                                join wn4 in wvqueryn4NCA.Where(n4 => n4.TIPO_CARGA == "CNTR")
                                 on wportal.CNTR_CONSECUTIVO equals wn4.ID_CARGA into temp
                                from wn41 in temp.DefaultIfEmpty()
                                join wfn4 in wvqueryn4Factura
                                on wportal.Contenedor equals wfn4.CONTENEDOR into temp1
                                from wn42 in temp1.DefaultIfEmpty()
                                join wcasi in wvcas
                                on wportal.Contenedor equals wcasi.CONTENEDOR into temp2
                                from wncas in temp2.DefaultIfEmpty()
                                

                                select new
                                {
                                    ASIGNADO = Boolean.Parse("false"),
                                    ASIGNADOPN = Boolean.Parse("false"),
                                    CONSECUTIVO = (String)wportal.CNTR_CONSECUTIVO.ToString(),
                                    CONTENEDOR = (String)wportal.Contenedor,
                                    DOCUMENTO = (String)wportal.DOCUMENTO,
                                    MRN = (String)wportal.MRN,
                                    MSN = (String)wportal.MSN,
                                    HSN = (String)wportal.HSN,
                                    REFERNCIA = (String)wportal.NAVE,
                                    FECHA_SALIDA = "",
                                    FECHA_FACURA = (String)DateTime.Now.ToString("MM/dd/yyyy"),
                                    TURNO = (String)"",
                                    IDTURNO = (String)"0",
                                    EMPRESA = (String)"",
                                    IDEMPRESA = (String)"",
                                    PLACA = (String)"",
                                    CHOFER = (String)"",
                                    IDCHOFER = (String)"",
                                    CAS = (String)"",
                                    CONPAS = wn41 == null ? Boolean.Parse("false") : Boolean.Parse("true"),
                                    ULT_FECHA_FACTURA = wn42 == null ? "" : wn42.FECHA_FACTURA.ToString("MM/dd/yyyy hh:mm:ss"),
                                    NUMERO_PASE = wn41 == null ? "" : (String)wn41.NUMERO_PASE_N4,
                                    TIPO_CARGA = (String)(wportal.Tipe_Cart == null ? "" : wportal.Tipe_Cart.ToString()),
                                    Bloqueo = (Boolean)(wportal.Bloqueo > 0 ? Boolean.Parse("true") : Boolean.Parse("false")),
                                    ESTADO = (String)(wportal.ESTADO == null ? "" : wportal.ESTADO.ToString()),
                                    ESTADON4 = (String)wportal.ESTADON4,
                                    ACTPASE = wpase,
                                    DDA = (Boolean)(wportal.DDA > 0 ? Boolean.Parse("true") : Boolean.Parse("false")),
                                    FEC_CAS = (String)(wportal.FEC_CAS != null ?((DateTime) wportal.FEC_CAS).ToString("MM/dd/yyyy hh:mm:ss") : wncas != null ? wncas.FECHA : ""),
                                    VALICAS = wportal.VALICAS,
                                    MJERRORCAS=(String) wmjerrorCAS,
                                    ID_RESERVA = wn41 == null ? "0" : (String)wn41.ID_RESERVA.ToString(),
                                    RESERVA = wn41 == null ? Boolean.Parse("false") : (Boolean)wn41.RESERVA,
                                    //ESTADO_RESERVA = (Boolean)wn41.ESTADO_RESERVA,
                                    

                                }
                                        ).Distinct().OrderBy(X => X.CONTENEDOR).ToList();


            wvresult.Tables.Add(wutil.LINQToDataTable(wvqueryresul));*/

            return wvresult;
        }
        public Boolean IsManifContenedor(String wxml)
        {
            ClSUTIL wutil = new ClSUTIL();
            DBMMIDN4DataContext dbmN4 = new DBMMIDN4DataContext();
            String wscontenedor = null;
            String Tipo = null;
            Boolean wvresult = false;

            wutil.ConsXml("ConsultaCarga", "CONTENEDOR", wxml, out wscontenedor);
            wutil.ConsXml("ConsultaCarga", "Type", wxml, out Tipo);

            //Consulta Contenedores  Portal Cgsa
            dbmN4.CommandTimeout = 3600;
            var wvquery = (from wv_container in dbmN4.CONTAINERS_BILL
                           where
                                   wv_container.CNTR_MRN == null && wv_container.CNTR_MSN == null &&
                                   wv_container.CNTR_HSN == null && wv_container.CNTR_TYPE == Tipo &&
                                   wv_container.CNTR_CONTAINER.Trim() == (wscontenedor == null ? wv_container.CNTR_CONTAINER.Trim() : wscontenedor)
                           select wv_container).Count();
            if ((int)wvquery > 0)
            {
                wvresult = true;

            }

            return wvresult;
        }
        public DataSet GetBreakBulk(String wxml, String wxmlbooking)
        {


            DataSet wvresult = new DataSet();
            ClSUTIL wutil = new ClSUTIL();

            DBMMIDN4DataContext dbmN4 = new DBMMIDN4DataContext();

            String wvalor;
            int wisecuencial = -1;
            String Mrn = null;
            String Msn = null;
            String Hsn = null;
            String Referencia = null;
            String EstadoCntr = null;
            String Tipo = null;
            DataTable wdtportal = new DataTable();
            String wscontenedor = null;
            String wscontenedorexpo = null;
            String wsbooking = null;

            DateTime Wfechas;


            DateTime.TryParseExact(DateTime.Now.ToString("MM/dd/yyyy"), "MM/dd/yyyy", new System.Globalization.CultureInfo("es-EC"), System.Globalization.DateTimeStyles.None, out Wfechas);





            //Obtencion de Valores



            //CNTR=CONTENEDRO BRBK=BREAKBUKLL CFS=CFS

            //Consulta Pase a Puerta N4

            var wvqueryn4NCA = (from vbs_t_pase in dbmN4.VBS_T_PASE_PUERTA
                                where vbs_t_pase.ESTADO != "CA" && (vbs_t_pase.TIPO_CARGA == "BRBK" || vbs_t_pase.TIPO_CARGA == "CFS") && vbs_t_pase.FECHA_EXPIRACION.HasValue && vbs_t_pase.FECHA_EXPIRACION.Value.Date >= Wfechas.Date
                                group vbs_t_pase by new { ID_CARGA = Convert.ToString(vbs_t_pase.ID_CARGA), vbs_t_pase.TIPO_CARGA } into grs
                                select new { grs.Key.ID_CARGA, grs.Key.TIPO_CARGA, CANTIDAD = grs.Sum(g => g.CANTIDAD_CARGA), }).ToList();



            //  TOTAL = grps.Sum(g =>Decimal.Parse(g.Field<String>("TOTAL") !=null?g.Field<String>("TOTAL"):"0")),
            //Consulta CFS
            var wcfs = (from tarja in dbmN4.CFS_TARJA
                        join item in dbmN4.CFS_TARJA_ITEM on tarja.CODIGO_TARJA equals item.CODIGO_TARJA 
                        select new { MRN = tarja.MRN, tarja.MSN, HSN=item.HSN==null?"0000":item.HSN }).Distinct().ToList();
                      
                      //SEGUNDA
            //Consulta Factura
            var wvqueryn4Factura = (from vbs_Invoice in dbmN4.BIL_INVOICE
                                    where vbs_Invoice.TIPO_CARGA == "BRBK" || vbs_Invoice.TIPO_CARGA == "CFS"
                                    group vbs_Invoice by vbs_Invoice.ID_CARGA into grs_Invoice
                                    select new
                                    {
                                        FECHA_FACTURA = grs_Invoice.Max(g => g.FECHA_INGRESO),
                                        CARGA = grs_Invoice.Key.ToString(),
                                    }).ToList();



            //wvqueryportal

            if (String.IsNullOrEmpty(wxmlbooking) != true)
            {
                wutil.ConsXml("ConsultaCarga", "CONTENEDOREXPO", wxmlbooking, out wscontenedorexpo);
                wutil.ConsXml("ConsultaCarga", "BOOKING", wxmlbooking, out wsbooking);
                wutil.ConsXml("ConsultaCarga", "REFERENCIA", wxmlbooking, out Referencia);

                XDocument xdoc = XDocument.Parse(wxmlbooking);
                IEnumerable<String> wresul = (from invoice in xdoc.Descendants("BOOKING")

                                              select (String)invoice.Attribute("ID_BOOKING").Value);
                 if (wresul != null && wresul.Count() > 0)
                {

                var wvqueryportal = (from wv_breakbulk in dbmN4.BREAK_BULKS_BILL.Where(BREAKBULK => wresul.Contains(BREAKBULK.BRBK_BKNG_BOOKING) &&  BREAKBULK.BRBK_VEPR_REFERENCE.Trim() == (Referencia == null ? BREAKBULK.BRBK_VEPR_REFERENCE.Trim() : Referencia) && BREAKBULK.BRBK_TYPE == "EXPO")
                                     select new
                                     {
                                         CONSECUTIVO = wv_breakbulk.BRBK_CONSECUTIVO.ToString(),
                                         MRN = wv_breakbulk.BRBK_MRN,
                                         MSN = wv_breakbulk.BRBK_MSN,
                                         HSN = wv_breakbulk.BRBK_HSN,
                                         MARCA = wv_breakbulk.BRBK_MARK,
                                         DESCRIPCION = wv_breakbulk.BRBK_DESCRIPTION,
                                         STATUS = (String)null,
                                         SEGUNDA = (String)null,
                                         PRIMERA = (String)null,
                                         DOCUMENTO = wv_breakbulk.BRBK_DOCUMENT,
                                         CANTIDAD = (Decimal)(wv_breakbulk.BRBK_QUANTITY == null ? 0 : wv_breakbulk.BRBK_QUANTITY),

                                     }).Distinct().ToList();

                

                if (wvqueryportal != null && wscontenedorexpo != null)
                {
                    wdtportal = (wutil.LINQToDataTable(wvqueryportal.Take(1)));
                }
                else
                {
                    wdtportal = (wutil.LINQToDataTable(wvqueryportal));
                }

                }
                 else
                 {


                     var wvqueryportal = (from wv_breakbulk in dbmN4.BREAK_BULKS_BILL.Where(BREAKBULK =>  BREAKBULK.BRBK_BKNG_BOOKING == (wsbooking == null ? BREAKBULK.BRBK_BKNG_BOOKING : wsbooking)   &&  BREAKBULK.BRBK_VEPR_REFERENCE.Trim() == (Referencia == null ? BREAKBULK.BRBK_VEPR_REFERENCE.Trim() : Referencia) && BREAKBULK.BRBK_TYPE == "EXPO")
                                          select new
                                          {
                                              CONSECUTIVO = wv_breakbulk.BRBK_CONSECUTIVO.ToString(),
                                              MRN = wv_breakbulk.BRBK_MRN,
                                              MSN = wv_breakbulk.BRBK_MSN,
                                              HSN = wv_breakbulk.BRBK_HSN,
                                              MARCA = wv_breakbulk.BRBK_MARK,
                                              DESCRIPCION = wv_breakbulk.BRBK_DESCRIPTION,
                                              STATUS = (String)wv_breakbulk.BRBK_YARD_STATUS,
                                              SEGUNDA = (String)null,
                                              PRIMERA = (String)(wv_breakbulk.CARGO_TYPE_M + " - "+ wv_breakbulk.OPERATION),
                                              DOCUMENTO = wv_breakbulk.BRBK_DOCUMENT,
                                              CANTIDAD = (Decimal)(wv_breakbulk.BRBK_QUANTITY == null ? 0 : wv_breakbulk.BRBK_QUANTITY),

                                          }).Distinct().ToList();


               

                     if (wvqueryportal != null && wscontenedorexpo != null)
                     {
                         wdtportal = (wutil.LINQToDataTable(wvqueryportal.Take(1)));
                     }
                     else
                     {
                         wdtportal = (wutil.LINQToDataTable(wvqueryportal));
                     }
                 }

                //Consulta Container N4 y Cgsa
            }
            else
            {
                //if (wutil.ConsXml("ContainerCgsaN4", "SECUENCIA", wxml, out wvalor) == true)
                //{
                //    wisecuencial = int.Parse(wvalor);
                //}

                wutil.ConsXml("ConsultaCarga", "MRN", wxml, out Mrn);
                wutil.ConsXml("ConsultaCarga", "MSN", wxml, out Msn);
                wutil.ConsXml("ConsultaCarga", "HSN", wxml, out Hsn);
                wutil.ConsXml("ConsultaCarga", "Type", wxml, out Tipo);
                wutil.ConsXml("ConsultaCarga", "REFERENCIA", wxml, out Referencia);
                wutil.ConsXml("ConsultaCarga", "ESTADOCNTR", wxml, out EstadoCntr);

                //Consulta BREAKBULK
                var wvqueryportal = (from wv_breakbulk in dbmN4.FNA_FUN_BREAKBULK_BILL_N5(Mrn, Msn, Hsn)                                     
                                     select new
                                     {
                                         ASIGNADO = Boolean.Parse("false"),
                                         ASIGNADOPN = Boolean.Parse("false"),
                                         CONSECUTIVO = (String)wv_breakbulk.BRBK_CONSECUTIVO.ToString(),
                                         MRN = wv_breakbulk.BRBK_MRN,
                                         MSN = wv_breakbulk.BRBK_MSN,
                                         HSN = wv_breakbulk.BRBK_HSN,
                                         MARCA = wv_breakbulk.BRBK_MARK == null ? "" : wv_breakbulk.BRBK_MARK,
                                         DESCRIPCION = wv_breakbulk.BRBK_DESCRIPTION == null ? "" : wv_breakbulk.BRBK_DESCRIPTION,
                                         STATUS = (String)wv_breakbulk.BRBK_YARD_STATUS,
                                         //SEGUNDA = (String)null,
                                         SEGUNDA = (String)wv_breakbulk.SEGUNDA,
                                         PRIMERA = (String)(String)(wv_breakbulk.CARGO_TYPE_M + " - " + wv_breakbulk.OPERATION),
                                         DOCUMENTO = wv_breakbulk.BRBK_DOCUMENT == null ? "" : wv_breakbulk.BRBK_DOCUMENT,
                                         //  CANTIDAD = Decimal.Parse(wv_breakbulk.BRBK_QUANTITY <=0 ? "0" : Convert.ToString(wv_breakbulk.BRBK_QUANTITY)),
                                         CANTIDAD = (Decimal)wv_breakbulk.BRBK_QUANTITY,
                                         FECHA_SALIDA = (String)DateTime.Now.ToString("MM/dd/yyyy"),
                                         FECHA_FACURA = (String)DateTime.Now.ToString("MM/dd/yyyy"),
                                         CONPAS = wv_breakbulk == null ? Boolean.Parse("false") : Boolean.Parse("true"),
                                         //ULT_FECHA_FACTURA = wv_breakbulk == null ? "" : wv_breakbulk.FECHA_FACTURA.ToString("MM/dd/yyyy hh:mm:ss"),
                                         ULT_FECHA_FACTURA = (DateTime?)(wv_breakbulk.ULT_FECHA_FACTURA.HasValue ? wv_breakbulk.ULT_FECHA_FACTURA : null),

                                     }).Distinct().ToList();

               // wdtportal = (wutil.LINQToDataTable(wvqueryportal));

                wvresult.Tables.Add(wutil.LINQToDataTable(wvqueryportal));
                //Consulta Container N4 y Cgsa
            }
            /*




            var wvqueryportalresult = (from portal in wdtportal.AsEnumerable()
                                       join cfs in wcfs on new { MRNP=portal.Field<String>("MRN"),
                                                                 MSNP=portal.Field<String>("MSN"),
                                                                 HSNP = portal.Field<String>("HSN")
                                       }
                                       equals new { MRNP = cfs.MRN, MSNP = cfs.MSN, HSNP = cfs.HSN } into fr
                                       from cf in fr.DefaultIfEmpty()
                                       select new
                                       {
                                           CONSECUTIVO = portal.Field<String>("CONSECUTIVO"),
                                           MRN = portal.Field<String>("MRN"),
                                           MSN = portal.Field<String>("MSN"),
                                           HSN = portal.Field<String>("HSN"),
                                           MARCA = portal.Field<String>("MARCA"),
                                           DESCRIPCION = portal.Field<String>("DESCRIPCION"),
                                           STATUS = portal.Field<String>("STATUS"),
                                           SEGUNDA =(String)( cf==null?"BRBK":"CFS"), //portal.Field<String>("SEGUNDA"),
                                           PRIMERA = portal.Field<String>("PRIMERA"),
                                           DOCUMENTO = portal.Field<String>("DOCUMENTO"),
                                           CANTIDAD = portal.Field<Decimal>("CANTIDAD"),
                                           Carga = portal.Field<String>("MRN") + "-" + portal.Field<String>("MSN") + "-" + portal.Field<String>("HSN"),
                                       }).ToList();




            var wvqueryresul = (from wportal in wvqueryportalresult
                                join wn4 in wvqueryn4NCA.Where(n4 => n4.TIPO_CARGA == "BRBK")
                                 on wportal.CONSECUTIVO equals wn4.ID_CARGA into temp
                                from wn41 in temp.DefaultIfEmpty()
                                join wfn4 in wvqueryn4Factura
                                on wportal.Carga equals wfn4.CARGA into temp1
                                from wn42 in temp1.DefaultIfEmpty()
                                select new
                                {
                                    ASIGNADO = Boolean.Parse("false"),
                                    ASIGNADOPN = Boolean.Parse("false"),
                                    CONSECUTIVO = (String)wportal.CONSECUTIVO.ToString(),
                                    MRN = (String)wportal.MRN == null ? "" : wportal.MRN.ToString(),
                                    MSN = (String)wportal.MSN == null ? "" : wportal.MSN.ToString(),
                                    HSN = (String)wportal.HSN == null ? "" : wportal.HSN.ToString(),
                                    MARCA = (String)wportal.MARCA == null ? "" : wportal.MARCA.ToString(),
                                    DESCRIPCION = (String)wportal.DESCRIPCION == null ? "" : wportal.DESCRIPCION.ToString(),
                                    STATUS = (String)wportal.STATUS == null ? "" : wportal.STATUS.ToString(),
                                    SEGUNDA = (String)wportal.SEGUNDA == null ? "" : wportal.SEGUNDA.ToString(),
                                    PRIMERA = (String)wportal.PRIMERA == null ? "" : wportal.PRIMERA.ToString(),
                                    DOCUMENTO = (String)wportal.DOCUMENTO == null ? "" : wportal.DOCUMENTO.ToString(),
                                    CANTIDAD = Convert.ToString(wportal.CANTIDAD - (wn41 == null ? 0 : wn41.CANTIDAD)),
                                    FECHA_SALIDA = (String)DateTime.Now.ToString("MM/dd/yyyy"),
                                    FECHA_FACURA = (String)DateTime.Now.ToString("MM/dd/yyyy"),
                                    CONPAS = wn41 == null ? Boolean.Parse("false") : Boolean.Parse("true"),
                                    ULT_FECHA_FACTURA = wn42 == null ? "" : wn42.FECHA_FACTURA.ToString("MM/dd/yyyy hh:mm:ss"),



                                }
                                        ).Distinct().OrderBy(X => X.MRN).ToList();

            */

            

            return wvresult;
        }
        public DataSet GetEmpresa(String wxml)
        {
            //Consulta Empresa Parametros

            DBMMIDN4DataContext dbmN4 = new DBMMIDN4DataContext();
            ClSUTIL wutil = new ClSUTIL();
            DataSet wvresult = new DataSet();


            //Obtencion de Valores
            String wsclnt_type = null;
            String wsclnt_active = null;
            String wsclnt_ROLE = null;
            wutil.ConsXml("GetEmpresa", "CLNT_TYPE", wxml, out wsclnt_type);//"OTHR"
            wutil.ConsXml("GetEmpresa", "CLNT_ACTIVE", wxml, out wsclnt_active);
            wutil.ConsXml("GetEmpresa", "ROLE", wxml, out wsclnt_ROLE);


            //Consulta Empresa  Portal Cgsa
            if (wsclnt_type == null)
            {
                var wvqueryempresa = (from wvempresa in dbmN4.CLIENTS_BILL
                                      where
                                       wvempresa.CLNT_ACTIVE == (String.IsNullOrEmpty(wsclnt_active) == true ? wvempresa.CLNT_ACTIVE : wsclnt_active) &&
                                       wvempresa.ROLE == (String.IsNullOrEmpty(wsclnt_ROLE) == true ? wvempresa.ROLE : wsclnt_ROLE)

                                      select new
                                      {
                                          IDEMPRESA = (String)wvempresa.CLNT_CUSTOMER,
                                          EMPRESA = (String)wvempresa.CLNT_CUSTOMER + " - " + wvempresa.CLNT_NAME,
                                          DIRECCION = (String)wvempresa.CLNT_ADRESS1,
                                          CIUDAD = (String)wvempresa.CLNT_CITY,
                                          PROVINCIA = wvempresa.CLNT_STATE,
                                          MAIL = (String)wvempresa.CLNT_EMAIL,
                                          ROLE = (String)wvempresa.ROLE,
                                          EBILLING = (String)wvempresa.CLNT_EBILLING,

                                      }).ToList();

                wvresult.Tables.Add(wutil.LINQToDataTable(wvqueryempresa));


            }

            else
            {
                wsclnt_type = wsclnt_type.Trim();
                String[] wsType = wsclnt_type.Split(',');
            

                var wvqueryempresa = (from wvempresa in dbmN4.CLIENTS_BILL
                                      where wvempresa.CLNT_TYPE != null && wsType.Contains(wvempresa.CLNT_TYPE.ToString().Trim()) &&
                                       wvempresa.CLNT_ACTIVE == (String.IsNullOrEmpty(wsclnt_active) == true ? wvempresa.CLNT_ACTIVE.Trim() : wsclnt_active) &&
                                        wvempresa.ROLE == (String.IsNullOrEmpty(wsclnt_ROLE) == true ? wvempresa.ROLE.Trim() : wsclnt_ROLE)

                                      select new
                                      {
                                          IDEMPRESA = (String)Convert.ToString(wvempresa.CLNT_CUSTOMER).Trim(),
                                          EMPRESA = (String) Convert.ToString(wvempresa.CLNT_CUSTOMER).Trim() + " - " + Convert.ToString(wvempresa.CLNT_NAME),
                                          DIRECCION = (String)wvempresa.CLNT_ADRESS1,
                                          CIUDAD = (String)wvempresa.CLNT_CITY,
                                          PROVINCIA = wvempresa.CLNT_STATE,
                                          MAIL = (String)wvempresa.CLNT_EMAIL,
                                          ROLE = (String)wvempresa.ROLE,
                                          EBILLING = (String)wvempresa.CLNT_EBILLING,

                                      }).Distinct().ToList();

                wvresult.Tables.Add(wutil.LINQToDataTable(wvqueryempresa));
            }
            return wvresult;
        }
        public DataSet GetPlaca()
        {
            //Consulta Empresa Parametros
            DBMMIDN4DataContext dbmN4 = new DBMMIDN4DataContext();
            ClSUTIL wutil = new ClSUTIL();
            DataSet wvresult = new DataSet();

            //Consulta Placa  N4
            var wvqueryn4 = (from vehiculo in dbmN4.VEH_CAMION_AUTORIZA
                             select new
                             {
                                 PLACA = (String) (vehiculo.PLACA !=null? vehiculo.PLACA : "") ,
                             }).ToList();

            var wquery=(from veh in wvqueryn4
                        select new {
                            PLACA =  (String)(!String.IsNullOrEmpty(veh.PLACA)?veh.PLACA.ToString().Trim():"")
                        }
                        ).ToList();
            wvresult.Tables.Add(wutil.LINQToDataTable(wquery));

            return wvresult;
        }
        public DataSet GetChofer()
        {
            //Consulta Empresa Parametros
            DBMMIDN4DataContext dbmN4 = new DBMMIDN4DataContext();
            ClSUTIL wutil = new ClSUTIL();
            DataSet wvresult = new DataSet();

            //Consulta N4
            var wvqueryn4 = (from wvchofer in dbmN4.VEH_TRANSPORT_AUTORIZA
                             select new
                            {
                                IDCHOFER = wvchofer.CHOFER.ToString(),
                                CHOFER = (wvchofer.CHOFER == null ? "" : wvchofer.CHOFER.ToString()) + " - " + (wvchofer.APELLIDO == null ? "" : wvchofer.APELLIDO.ToString()) + " - " + (wvchofer.NOMBRE == null ? "" : wvchofer.NOMBRE.ToString())
                            }).ToList();


            wvresult.Tables.Add(wutil.LINQToDataTable(wvqueryn4));
            return wvresult;
        }
        public DataSet GetTurno(String wxml, String wuser)
        {


            //Declaracion de Parametros
            DBMMIDN4DataContext dbmN4 = new DBMMIDN4DataContext();

            XElement welement = XElement.Parse(wxml);

            DataSet wvresult = new DataSet();
            ClSUTIL wutil = new ClSUTIL();


            //Consulta Pase a Puerta N4
            var wvqueryn4NCA = dbmN4.N4_P_Cons_Turnos_Disponibles(welement);

            var wvqueryresul = (from wturno in wvqueryn4NCA
                                select new
                                {
                                    IDTURNO = wturno.Plan_id.ToString() + "-" + wturno.Turno_Secuencia.ToString(),
                                    TURNO = wturno.Turno,

                                }
                         ).ToList();



            var wresul = wvqueryresul.ToList().Concat(new[] { new { IDTURNO = "0", TURNO = " Seleccione" } });



            wvresult.Tables.Add(wutil.LINQToDataTable(wresul.OrderBy(Tur => Tur.TURNO)));

            return wvresult;
        }
        public DataSet GetFactura(String[] wxml)
        {
            DataSet wvresult = new DataSet();
            DataSet wvresultgroupy = new DataSet();
            ClSUTIL wutil = new ClSUTIL();
            wsN4 wsn4 = new wsN4();
            XDocument xdoc = null;
            XDocument xdoccarga = null;
            Boolean wbandera = true;



            String wmsg = string.Empty;
            String ruta = @"ICT/ECU/GYE/CGSA";
            XNamespace nsa = "http://www.navis.com/argo";


            try
            {

               // wsn4.Url = "";
                
                foreach (String Xmlparameter in wxml)
                {
                    var i = wsn4.CallBasicService(ruta, Xmlparameter, ref wmsg);

                    try
                    {
                        wbandera = true;
                        xdoc = XDocument.Parse(wmsg);
                        xdoccarga = XDocument.Parse(Xmlparameter);
                    }

                    catch (Exception exc)
                    {

                        wbandera = false;
                    }

                    if (wutil.argo_response(wmsg) == true && wbandera == true)
                    {
                        String wsCarga = null;
                        String wfecha = null;
                        String wInvoicetype = null;
                        wfecha = (from wfecFac in xdoccarga.Descendants("invoiceParameter")
                                  select new
                                           {
                                               fecha = (String)wfecFac.Element("PaidThruDay").Value.ToString()
                                           }).FirstOrDefault().fecha;




                        wInvoicetype = (from wInvoiceType in xdoccarga.Descendants("generate-invoice-request")
                                  select new
                                  {
                                      fecha = (String)wInvoiceType.Element("invoiceTypeId").Value.ToString()
                                  }).FirstOrDefault().fecha;



                        


                        wsCarga = (from wfecFac in xdoccarga.Descendants("invoiceParameter")
                                   select new
                                   {
                                       carga = (String)wfecFac.Element("bexuBlNbr") == null ? null : wfecFac.Element("bexuBlNbr").Value
                                   }).FirstOrDefault().carga;

          
                        var wresul = (from invoice in xdoc.Descendants(nsa + "invoiceCharge")

                                      select new
                                      {
                                          TOTAL = (String)invoice.Attribute(nsa + "totalCharged"),
                                          SERVICIO = (String)invoice.Attribute(nsa + "description"),
                                          CARGA = String.IsNullOrEmpty(wsCarga) != true ? wsCarga : (String)invoice.Attribute(nsa + "chargeEntityId"),  //(String)invoice.Attribute(nsa + "chargeEntityId"),
                                          CODIGO = (String)invoice.Attribute(nsa + "chargeGlCode"),
                                          CANTIDAD = (String)invoice.Attribute(nsa + "quantityBilled"),
                                          PRECIO = (String)invoice.Attribute(nsa + "rateBilled"),
                                          IVA = (String)invoice.Attribute(nsa + "totalTaxes"),
                                          FECHA = (String)wfecha,
                                          INVOICETYPE= wInvoicetype,
                                      }
                          ).ToList();
                        if (wresul != null && wresul.Count > 0)
                        {

                            if (wvresult.Tables.Count <= 0)
                            {
                                wvresult.Tables.Add(wutil.LINQToDataTable(wresul));
                            }
                            else
                            {

                                foreach (DataRow wrow in wutil.LINQToDataTable(wresul).Rows)
                                {
                                    wvresult.Tables[0].ImportRow(wrow);
                                }

                                wvresult.Tables[0].AcceptChanges();


                            }
                        }
                    }

                }

                if (wvresult.Tables.Count > 0 && wvresult.Tables[0].Rows.Count > 0)
                {
                    System.Globalization.CultureInfo info = System.Globalization.CultureInfo.GetCultureInfo("en-US");


                    var wresultsgrup = (from p in wvresult.Tables[0].AsEnumerable()
                                        group p by new { CARGA = p.Field<string>("CARGA"), CODIGO = p.Field<string>("CODIGO"), SERVICIO = p.Field<string>("SERVICIO"), PRECIO = p.Field<String>("PRECIO"), FECHA = p.Field<String>("FECHA"), INVOICETYPE = p.Field<String>("INVOICETYPE") } into grps
                                        select new
                                        {
                                            //                                       TOTAL = grps.Sum(g =>Decimal.Round(Decimal.Parse(g.Field<String>("TOTAL") !=null?g.Field<String>("TOTAL").Replace(",","."):"0"),10)),
                                            TOTAL = grps.Sum(g => Decimal.Round(Decimal.Parse(g.Field<String>("TOTAL") != null ? g.Field<String>("TOTAL") : "0", info), 2)),
                                            SERVICIO = grps.Key.SERVICIO.ToString().Trim(),
                                            CARGA = grps.Key.CARGA,
                                            CODIGO = grps.Key.CODIGO.ToString().Trim(),
                                            CANTIDAD = grps.Sum(g => Decimal.Round(Decimal.Parse(g.Field<String>("CANTIDAD") != null ? g.Field<String>("CANTIDAD") : "0", info), 2)),
                                            PRECIO = Decimal.Round(Decimal.Parse(grps.Key.PRECIO != null ? grps.Key.PRECIO : "0", info), 2),
                                            IVA = grps.Sum(g => Decimal.Round(Decimal.Parse(g.Field<String>("IVA") != null ? g.Field<String>("IVA") : "0", info), 2)),
                                            FECHA = grps.Key.FECHA,
                                            INVOICETYPE=grps.Key.INVOICETYPE,
                                        }).ToList();
                    wvresultgroupy.Tables.Add(wutil.LINQToDataTable(wresultsgrup));
                }


            }
            catch (Exception exc)
            {
                throw new DataException(string.Format("Error N4 Factura {0} ",  exc.Message.ToString()));
            }

            return wvresultgroupy;
        }
        public DataSet SaveFactura(String[] wxml, String wuser, String wtype,String wagente, String wcomentario)
        {
            DataSet wvresult = new DataSet();

            ClSUTIL wutil = new ClSUTIL();
            wsN4 wsn4 = new wsN4();
            XDocument xdoc = null;
            XDocument xdocarga = null;
            Boolean wbandera = true;
            DBMMIDN4DataContext dbmN4 = new DBMMIDN4DataContext();
            String wsfactura = null;
            String wmsg = string.Empty;
            String ruta = @"ICT/ECU/GYE/CGSA";
            XNamespace nsa = "http://www.navis.com/argo";
            String wfecha = null;

            try
            {

                foreach (String Xmlparameter in wxml)
                {
                    var i = wsn4.CallBasicService(ruta, Xmlparameter, ref wmsg);

                    try
                    {
                        wbandera = true;
                        xdoc = XDocument.Parse(wmsg);
                        xdocarga = XDocument.Parse(Xmlparameter);
                    }

                    catch (Exception exc)
                    {

                        wbandera = false;
                    }

                    if (wutil.argo_response(wmsg) == true && wbandera == true)
                    {
                        String wsCarga = null;
                        
                        if (wtype == "CNTR")
                        {
                            var wCarga_cont = (from dcarga in xdocarga.Descendants("invoiceParameter")

                                               select new
                                               {
                                                   carga = dcarga.Element("EquipmentId").Value,
                                                   
                                               }).FirstOrDefault();
                            wsCarga = wCarga_cont.carga.ToString();
                            
                        }
                        else
                        {
                            var wCarga_breakbulk = (from dcarga in xdocarga.Descendants("invoiceParameter")

                                                    select new
                                                    {
                                                        carga = dcarga.Element("bexuBlNbr").Value,
                                                        
                                                    }).FirstOrDefault();
                            wsCarga = wCarga_breakbulk.carga.ToString();
                            
                        }

                        wfecha = (from wfecFac in xdocarga.Descendants("invoiceParameter")
                                  select new
                                  {
                                      fecha = (String)wfecFac.Element("PaidThruDay").Value.ToString()
                                  }).FirstOrDefault().fecha;


                        //REsultado Para el Draft
                        var wresul = (from invoice in xdoc.Descendants(nsa + "invoice")
                                      select new
                                      {
                                          NUMERDRAFT = (String)invoice.Attribute(nsa + "draftNumber"),
                                          TYPE = (String)invoice.Attribute(nsa + "type"),
                                          CARGA = wsCarga,
                                          FECHA = wfecha, 

                                      }
                                    ).ToList();

                        //REsultado Para el Reporte
                        var wresulReport = (from invoice in xdoc.Descendants(nsa + "invoiceCharge")

                                            select new
                                            {
                                                TOTAL = (String)invoice.Attribute(nsa + "totalCharged"),
                                                SERVICIO = (String)invoice.Attribute(nsa + "description"),
                                                CARGA = wsCarga,
                                                CODIGO = (String)(invoice.Attribute(nsa + "chargeGlCode") != null ? (String)invoice.Attribute(nsa + "chargeGlCode") : ""),
                                                CANTIDAD = (String)invoice.Attribute(nsa + "quantityBilled"),
                                                PRECIO = (String)invoice.Attribute(nsa + "rateBilled"),
                                                IVA = (String)invoice.Attribute(nsa + "totalTaxes"),
                                            }
                         ).ToList();

                        //REsultado Tabla draft
                        if (wvresult.Tables.Count <= 0)
                        {
                            wvresult.Tables.Add(wutil.LINQToDataTable(wresul));
                        }
                        else
                        {
                            foreach (DataRow wrow in wutil.LINQToDataTable(wresul).Rows)
                            {
                                wvresult.Tables[0].ImportRow(wrow);
                            }

                            wvresult.Tables[0].AcceptChanges();
                        }

                        //REsultado Tabla Reporte

                        if (wvresult.Tables.Count <= 1)
                        {
                            wvresult.Tables.Add(wutil.LINQToDataTable(wresulReport));
                        }
                        else
                        {
                            foreach (DataRow wrow in wutil.LINQToDataTable(wresulReport).Rows)
                            {
                                wvresult.Tables[1].ImportRow(wrow);
                            }

                            wvresult.Tables[1].AcceptChanges();

                        }

                    }

                }

                if (wvresult != null && wvresult.Tables.Count > 0)
                {

                    String docXMLN4 = new System.Xml.Linq.XDocument(new System.Xml.Linq.XDeclaration("1.0", "UTF-8", "yes"),
                                      new System.Xml.Linq.XElement("billing", new System.Xml.Linq.XElement("merge-invoices-to-new-invoice-request",
                                      new System.Xml.Linq.XElement("drftInvoiceNbrs", from p in wvresult.Tables[0].AsEnumerable()
                                                                                      select new System.Xml.Linq.XElement("drftInvoiceNbr", p.Field<String>("NUMERDRAFT").ToString())),
                                                     new System.Xml.Linq.XElement("invoiceTypeId", (from p in wvresult.Tables[0].AsEnumerable()
                                                                                                    select p.Field<String>("TYPE").ToString()).Take<String>(1))
                              ))).ToString();


                    var resultdraft = wsn4.CallBasicService(ruta, docXMLN4, ref wmsg);

                    try
                    {
                        wbandera = true;
                        xdoc = XDocument.Parse(wmsg);
                    }
                    catch (Exception exc)
                    {
                        wbandera = false;
                    }

                    if (wutil.argo_response(wmsg) == true && wbandera == true)
                    {
                        var wDRAFT = (from dcarga in xdoc.Descendants(nsa + "invoice")
                                      select new { NUMERDRAFT = dcarga.Attribute(nsa + "draftNumber").Value, }).FirstOrDefault();


                        String docXMLFinal = new System.Xml.Linq.XDocument(new System.Xml.Linq.XDeclaration("1.0", "UTF-8", "yes"),
                                 new System.Xml.Linq.XElement("billing",
                                     new System.Xml.Linq.XElement("finalize-invoice-request",
                                     new System.Xml.Linq.XElement("drftInvoiceNbr", wDRAFT.NUMERDRAFT.ToString()),
                                    new System.Xml.Linq.XElement("finalizeDate", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"))))).ToString();

                        var resultfinal = wsn4.CallBasicService(ruta, docXMLFinal, ref wmsg);

                        try
                        {
                            wbandera = true;
                            xdoc = XDocument.Parse(wmsg);
                        }
                        catch (Exception exc)
                        {
                            wbandera = false;
                        }

                        if (wutil.argo_response(wmsg) == true && wbandera == true)
                        {
                            var wfactura = (from dfactura in xdoc.Descendants(nsa + "finalize-invoice-response")
                                            select new { NUMFACTURA = dfactura.Element("invoice-final-nbr").Value, }).FirstOrDefault();
                            wsfactura = wfactura.NUMFACTURA.ToString();
                        }
                        if (wsfactura != null)
                        {
                            if (wvresult != null && wvresult.Tables[0].Rows.Count > 0)
                            {

                                 var wbil_invoice = (from p in wvresult.Tables[0].AsEnumerable()
                                                      let car = p.Field<String>("CARGA").Split(',')
                                                   select new { NUMERDRAFT = p.Field<String>("NUMERDRAFT"), FECHA = p.Field<String>("FECHA"), CARGA = car }).ToList();


                                String WBILINVOICE = new System.Xml.Linq.XDocument(new System.Xml.Linq.XDeclaration("1.0", "UTF-8", "yes"),
                                                     new System.Xml.Linq.XElement("BIL_INVOICE", from p in wbil_invoice
                                                                                                 select p.CARGA.Select(x=> 
                                                                                                     new System.Xml.Linq.XElement("VBS_P_BIL_INVOICE",
                                                                                                     new System.Xml.Linq.XAttribute("ID_CARGA", x.Trim()),
                                                                                                     new System.Xml.Linq.XAttribute("TIPO_CARGA", wtype),
                                                                                                     new System.Xml.Linq.XAttribute("DRAFT_CARGA", p.NUMERDRAFT),
                                                                                                     new System.Xml.Linq.XAttribute("DRAFT_INVOICE", wDRAFT.NUMERDRAFT.ToString()),
                                                                                                     new System.Xml.Linq.XAttribute("FECHA_INGRESO", DateTime.Parse(p.FECHA).ToString("MM/dd/yyyy HH:mm:ss")),
                                                                                                     new System.Xml.Linq.XAttribute("USUARIO_INGRESO", wuser),
                                                                                                     new System.Xml.Linq.XAttribute("NUMERO_INVOICE", wsfactura),
                                                                                                     new System.Xml.Linq.XAttribute("AGENTE", String.IsNullOrEmpty(wagente) == true ? "" : wagente),
                                                                                                     new System.Xml.Linq.XAttribute("COMENTARIO", String.IsNullOrEmpty(wcomentario) == true ? "" : wcomentario),
                                                                                                     new System.Xml.Linq.XAttribute("flag", "I"))))).ToString();

                               XElement welement = XElement.Parse(WBILINVOICE);
                               dbmN4.VBS_P_BIL_INVOICE(welement);
                            }

                            if (wvresult.Tables[1].Rows.Count > 0)
                            {
                                System.Globalization.CultureInfo info = System.Globalization.CultureInfo.GetCultureInfo("en-US");

                                var wresultsgrup = (from p in wvresult.Tables[1].AsEnumerable()
                                                    group p by new { CODIGO = p.Field<string>("CODIGO"), SERVICIO = p.Field<string>("SERVICIO"), PRECIO = p.Field<String>("PRECIO") } into grps
                                                    select new
                                                    {
                                                        TOTAL = grps.Sum(g => Decimal.Round(Decimal.Parse(g.Field<String>("TOTAL") != null ? g.Field<String>("TOTAL") : "0", info), 2)),
                                                        SERVICIO = grps.Key.SERVICIO,
                                                        CODIGO = grps.Key.CODIGO,
                                                        CANTIDAD = grps.Sum(g => Decimal.Round(Decimal.Parse(g.Field<String>("CANTIDAD") != null ? g.Field<String>("CANTIDAD") : "0", info), 2)),
                                                        PRECIO = Decimal.Round(Decimal.Parse(grps.Key.PRECIO != null ? grps.Key.PRECIO : "0", info), 2),
                                                        IVA = grps.Sum(g => Decimal.Round(Decimal.Parse(g.Field<String>("IVA") != null ? g.Field<String>("IVA") : "0", info), 2)),
                                                        FACTURA = (String)"00" + wsfactura.Substring(0, 1) + "-" + wsfactura.Substring(1, 3) + "-" + wsfactura.Substring(4, 9),
                                                        ORFACTURA = (String)wsfactura,
                                                        AGENTE = (String)wagente,
                                                        COMENTARIO=(String) wcomentario,
                                                    }).ToList();

                                wvresult.Tables.RemoveAt(1);
                                wvresult.AcceptChanges();

                                wvresult.Tables.Add(wutil.LINQToDataTable(wresultsgrup));
                            }
                        }
                        else
                        {
                            wvresult = null;
                        }
                    }

                }

            }
            catch (Exception exc)
            {
                throw new DataException(string.Format("Error N4 Factura {0} ",  exc.Message.ToString()));
            }

            return wvresult;
        }
        public DataSet SavePasePuerta(String wxml, String[] wxmln4, String Wtype)
        {
            //Declaracion de Parametros
            DataTable dtDetallePase = new DataTable();
            DBMMIDN4DataContext dbmN4 = new DBMMIDN4DataContext();
            wsN4 wsn4 = new wsN4();
            XElement welement = XElement.Parse(wxml);

            DataSet wvresult = new DataSet();
            DataSet wvrporte = new DataSet();
            ClSUTIL wutil = new ClSUTIL();
            XDocument xdoc = null;
            XDocument xdocarga = null;
            Boolean wbandera = true;
            DataTable DTError = new DataTable("DTError");
            DTError.Columns.Add(new DataColumn("CARGA", Type.GetType("System.String")));
            DTError.Columns.Add(new DataColumn("MENSAJE", Type.GetType("System.String")));
            DTError.AcceptChanges();
            //Genera Pase a Puerta N4
            foreach (String Xmlparameter in wxmln4)
            {
                try
                {
                    String wmsg = string.Empty;
                    String ruta = @"ICT/ECU/GYE/CGSA";

                    var i = wsn4.CallBasicService(ruta, Xmlparameter, ref wmsg);
                    try
                    {
                        wbandera = true;
                        xdocarga = XDocument.Parse(Xmlparameter);
                        if (Wtype == "CNTR")
                        {
                            xdoc = XDocument.Parse(wmsg);

                        }
                        else
                        {
                            if (!wmsg.Contains("OK"))
                            {
                                wbandera = false;

                                String cargaerror = (from invoice in xdocarga.Descendants("parameter")
                                                     where invoice.Attribute("id").Value == "BLs"
                                                     select new
                                                     {
                                                         CARGA = (String)invoice.Attribute("value").Value == null ? "" : invoice.Attribute("value").Value,
                                                     }).FirstOrDefault().CARGA.ToString();

                                if (wmsg.Contains("ERRKEY_QTY_ORDERED_EXCEEDS_AVAILABLE_QTY_FOR_CARGO_LOT"))
                                {
                                    DTError.Rows.Add(new String[] { cargaerror, "Carga sin stock para Pase a Puerta" });
                                    DTError.AcceptChanges();
                                }

                            }
                        }
                    }

                    catch (Exception exc)
                    {

                        wbandera = false;
                        //wbandera = true;

                    }

                    DataTable Dtemp = new DataTable();
                    if (wutil.argo_response(wmsg) == true && wbandera == true && Wtype == "CNTR")
                    {
                        var wresul = (from invoice in xdoc.Descendants("create-appointment-response")
                                      select new
                                      {
                                          PASE = (String)invoice.Element("appointment-nbr").Value,
                                          CARGA = (String)invoice.Element("container").Attribute("eqid").Value,
                                          CodSubitem=(String)"",
                                          idempresa=(String)"",
                                          Cantidad=(String)"",
                                          placa = (String)"",
                                          chofer = (String)"",
                                          tipo_carga = (String)"",
                                          fecha=(String)"",
                                          consecutivo = (String)"",
                                          usuer = (String)"",
                                      }).ToList();

                        Dtemp = wutil.LINQToDataTable(wresul);

                    }
                    else
                    {
                        //if (Wtype != "CNTR")
                        if (wbandera == true && Wtype != "CNTR")
                        {
                            String codSubitem="";
                            String codEmpresa="";
                            String cantidad="";
                            String tipo_carga = "";
                            String chofer = "";
                            String placa = "";
                            String fecha = "";
                            String consecutivo = "";
                            String usuer = "";
                            String wicarga =  (wmsg.Split('-').ToList()[wmsg.Split('-').Length - 1]);
                            wicarga = wicarga.Substring(0, wicarga.IndexOf("\n\r"));


                            var item = (from invoice in xdocarga.Descendants("parameter")
                                           where invoice.Attribute("id").Value == "codsubitem"
                                           select new
                                           {

                                               codsubitem = (String)invoice.Attribute("value").Value,
                                           }).FirstOrDefault();

                             var itemagencia = (from invoice in xdocarga.Descendants("parameter")
                                           where invoice.Attribute("id").Value == "agencia"
                                           select new
                                           {

                                               codempresa = (String)invoice.Attribute("value").Value,
                                           }).FirstOrDefault();

                             var itemacantidad = (from invoice in xdocarga.Descendants("parameter")
                                           where invoice.Attribute("id").Value == "QTY"
                                           select new
                                           {

                                               codcantidad = (String)invoice.Attribute("value").Value,
                                           }).FirstOrDefault();

                            var itemaplaca = (from invoice in xdocarga.Descendants("parameter")
                                           where invoice.Attribute("id").Value == "placa"
                                           select new
                                           {

                                               codplaca = (String)invoice.Attribute("value").Value,
                                           }).FirstOrDefault();

                  

                            
                            var itemachofer = (from invoice in xdocarga.Descendants("parameter")
                                           where invoice.Attribute("id").Value == "chofer"
                                           select new
                                           {

                                               codchofer = (String)invoice.Attribute("value").Value,
                                           }).FirstOrDefault();


                                     var itematipocarga = (from invoice in xdocarga.Descendants("parameter")
                                           where invoice.Attribute("id").Value == "tipo_carga"
                                           select new
                                           {

                                               codtipoCarga = (String)invoice.Attribute("value").Value,
                                           }).FirstOrDefault();

                                     var itemafecha = (from invoice in xdocarga.Descendants("parameter")
                                                           where invoice.Attribute("id").Value == "fecha"
                                                           select new
                                                           {

                                                               fecha = (String)invoice.Attribute("value").Value,
                                                           }).FirstOrDefault();

                                     var itemaconsecutivo = (from invoice in xdocarga.Descendants("parameter")
                                                       where invoice.Attribute("id").Value == "consecutivo"
                                                       select new
                                                       {

                                                           consecutivo = (String)invoice.Attribute("value").Value,
                                                       }).FirstOrDefault();



                                         var itemacantusuer = (from invoice in xdocarga.Descendants("parameter")
                                                       where invoice.Attribute("id").Value == "usuer"
                                                       select new
                                                       {

                                                           usuer = (String)invoice.Attribute("value").Value,
                                                       }).FirstOrDefault();


                       


                                     if (itemaconsecutivo != null)
                                     {
                                         consecutivo = itemaconsecutivo.consecutivo.ToString();
                                     }

                                     if (itemafecha != null)
                                     {
                                         fecha = itemafecha.fecha.ToString();
                                     }


                                     if (itemaplaca != null)
                                     {
                                         placa = itemaplaca.codplaca.ToString();
                                     }
                                     if (itemachofer != null)
                                     {
                                         chofer = itemachofer.codchofer.ToString();
                                     }
                                     if (itematipocarga != null)
                                     {
                                         tipo_carga = itematipocarga.codtipoCarga.ToString();
                                     }

                             


                            if (item != null)
                            {
                                codSubitem = item.codsubitem.ToString();
                            }

                            if (itemagencia != null)
                            {
                                codEmpresa = itemagencia.codempresa.ToString();
                            }
                            if (itemacantidad != null)
                            {
                                cantidad = itemacantidad.codcantidad.ToString();
                            }

                            if (itemacantusuer != null)
                            {
                                usuer = itemacantusuer.usuer.ToString();
                            }


                            

                            var wresult = (from invoice in xdocarga.Descendants("parameter")
                                           where invoice.Attribute("id").Value == "BLs"
                                           select new
                                           {
                                               
                                               PASE = wicarga,
                                               CARGA = (String)invoice.Attribute("value").Value,
                                               CodSubitem = codSubitem,
                                               idempresa = (String)codEmpresa,
                                               Cantidad = (String)cantidad,
                                               placa=(String)placa,
                                               chofer = (String)chofer,
                                               tipo_carga = (String)tipo_carga,
                                               fecha = (String)fecha,
                                               consecutivo = (String)consecutivo,
                                               usuer = (String)usuer,
                                           }).ToList();
                            Dtemp = wutil.LINQToDataTable(wresult);
                            //dtDetallePase = Dtemp;
                        }
                    }

                    if (wvresult.Tables.Count <= 0)
                    {
                        wvresult.Tables.Add(Dtemp);
                    }
                    else
                    {
                        foreach (DataRow wrow in Dtemp.Rows)
                        {
                            wvresult.Tables[0].ImportRow(wrow);
                        }
                        wvresult.Tables[0].AcceptChanges();
                    }
                }
                catch (Exception exc)
                {
                    throw new DataException(string.Format("Error N4 Pase a Puerta {0} ",  exc.Message.ToString()));
                }
            }

            if (wvresult.Tables.Count > 0 && wvresult.Tables[0].Rows.Count > 0)
            {

                System.Xml.Linq.XDocument docXML = new System.Xml.Linq.XDocument(new System.Xml.Linq.XDeclaration("1.0", "UTF-8", "yes"),
             new System.Xml.Linq.XElement("PASEPUERTAN4", from p in wvresult.Tables[0].AsEnumerable()
                                                          select new System.Xml.Linq.XElement("VBS_P_PASE_PUERTA_N4",
          new System.Xml.Linq.XAttribute("CARGA", p.Field<String>("CARGA") == null ? null : p.Field<String>("CARGA").ToString()),
          new System.Xml.Linq.XAttribute("PASE", p.Field<String>("PASE") == null ? null : p.Field<String>("PASE").ToString()),
          new System.Xml.Linq.XAttribute("CODSUBITEM", p.Field<String>("CodSubitem") == null ? null : p.Field<String>("CodSubitem").ToString()),
          new System.Xml.Linq.XAttribute("IDEMPRESA", p.Field<String>("idempresa") == null ? null : p.Field<String>("idempresa").ToString()),
          new System.Xml.Linq.XAttribute("CANTIDAD", p.Field<String>("Cantidad") == null ? null : p.Field<String>("Cantidad").ToString()),
          new System.Xml.Linq.XAttribute("ID_PLACA", p.Field<String>("placa") == null ? null : p.Field<String>("placa").ToString()),
          new System.Xml.Linq.XAttribute("ID_CHOFER", p.Field<String>("chofer") == null ? null : p.Field<String>("chofer").ToString()),
          new System.Xml.Linq.XAttribute("TIPO_CARGA", p.Field<String>("tipo_carga") == null ? null : p.Field<String>("tipo_carga").ToString()),
          new System.Xml.Linq.XAttribute("FECHA_EXPIRACION", p.Field<String>("fecha") == null ? null : p.Field<String>("fecha").ToString()),
          new System.Xml.Linq.XAttribute("ID_CARGA", p.Field<String>("consecutivo") == null ? null : p.Field<String>("consecutivo").ToString()),
          new System.Xml.Linq.XAttribute("USUARIO_REGISTRO", p.Field<String>("usuer") == null ? null : p.Field<String>("usuer").ToString())
          )));

                XElement welementn4 = XElement.Parse(docXML.ToString());

                //Genera Pase a Puerta CGSA
                try
                {
                    dbmN4.CommandTimeout = 3600;
                    if (Wtype == "CNTR")
                    {

                        dbmN4.VBS_P_PASE_PUERTA(welement, welementn4);
                        
                        var wvqueryn4NCA = dbmN4.VBS_REPORT_PASE_PUERTA(welementn4, Wtype);
                        var wreporte = (from wresult in wvqueryn4NCA
                                        select wresult).ToList();

                        wvrporte.Tables.Add(wutil.LINQToDataTable(wreporte));
                    }
                    else
                    {
                        dbmN4.VBS_P_PASE_PUERTA_BREAKBULK(welement, welementn4);
                        var wvqueryn4NCAbrk = dbmN4.VBS_REPORT_PASE_PUERTA_BREAK (welementn4, Wtype);
                        var wreporte_brk = (from wresult in wvqueryn4NCAbrk
                                        select wresult).ToList();
                        wvrporte.Tables.Add(wutil.LINQToDataTable(wreporte_brk));
                    }
                 
                }
                catch (Exception exc)
                {
                    throw new DataException(string.Format("Error WCF Pase a Puerta {0} ",  exc.Message.ToString()));
                }

            }

            wvrporte.Tables.Add(DTError);
            wvrporte.AcceptChanges();
            return wvrporte;
        }
        public void SaveRefeer(String[] wxmlSNK)
        {
            //Declaracion de Parametros
            DBMMIDN4DataContext dbmN4 = new DBMMIDN4DataContext();
            wsN4 wsn4 = new wsN4();
            String wmsg = string.Empty;
            String ruta = @"ICT/ECU/GYE/CGSA";
            ClSUTIL wutil = new ClSUTIL();
            try
            {

                //Genera Pase a Puerta N4
                foreach (String Xmlparameter in wxmlSNK)
                { var i = wsn4.CallBasicService(ruta, Xmlparameter, ref wmsg); }

      

            }
            catch (Exception exc)
            {



            }



        }
        public DataSet SaveCancelarPasePuerta(String wxml, String[] wxmln4, String[] wxmln4Break)
        {
            //Declaracion de Parametros
            DBMMIDN4DataContext dbmN4 = new DBMMIDN4DataContext();
            wsN4 wsn4 = new wsN4();
            XElement welement = XElement.Parse(wxml);
            DataTable Dtemp = new DataTable();
            DataSet wvresult = new DataSet();
            DataSet wvrporte = new DataSet();
            ClSUTIL wutil = new ClSUTIL();
            XDocument xdoc = null;
            XDocument xdocarga = null;
            Boolean wbandera = true;
            String wmsg = string.Empty;
            String ruta = @"ICT/ECU/GYE/CGSA";
            //Genera Pase a Puerta N4 Contenedores
            foreach (String Xmlparameter in wxmln4)
            {
                try
                {
                    wmsg = string.Empty;


                    var i = wsn4.CallBasicService(ruta, Xmlparameter, ref wmsg);
                    try
                    {
                        wbandera = true;
                        xdocarga = XDocument.Parse(Xmlparameter);
                        
                        if (!wmsg.Contains("already Expired"))
                        {
                            xdoc = XDocument.Parse(wmsg);
                        }

                    }

                    catch (Exception exc)
                    {
                        wbandera = false;
                    }

                    Dtemp = new DataTable();
                    if (wbandera == true)
                    {
                        

                        var wresulTE = (from invoice in xdocarga.Descendants("appointment")
                                      select new
                                      {
                                          PASE = (String)invoice.Attribute("appointment-nbr").Value,
                                         
                                          TIPO = (String)"CNTR",
                                      }).ToList();

                        Dtemp = wutil.LINQToDataTable(wresulTE);
                    }
                    else
                    {

                        if (wutil.argo_response(wmsg) == true && wbandera == true)
                        {


                            var wresul = (from invoice in xdoc.Descendants("appointment")
                                          select new
                                          {
                                              PASE = (String)invoice.Attribute("appointment-nbr").Value,
                                              
                                              TIPO = (String)"CNTR",
                                          }).ToList();

                            Dtemp = wutil.LINQToDataTable(wresul);

                        }
                    }


                    if (wvresult.Tables.Count <= 0)
                    {
                        wvresult.Tables.Add(Dtemp);
                    }
                    else
                    {
                        foreach (DataRow wrow in Dtemp.Rows)
                        {
                            wvresult.Tables[0].ImportRow(wrow);
                        }
                        wvresult.Tables[0].AcceptChanges();
                    }
                }
                catch (Exception exc)
                {
                    throw new DataException(string.Format("Error N4 Pase a Puerta Contenedores {0} ",  exc.Message.ToString()));
                }
            }


            //Genera Pase a Puerta N4 BreakBulk
            foreach (String Xmlparameter in wxmln4Break)
            {
                try
                {
                    wmsg = string.Empty;


                    var i = wsn4.CallBasicService(ruta, Xmlparameter, ref wmsg);
                    try
                    {
                        wbandera = true;
                        xdocarga = XDocument.Parse(Xmlparameter);

                        if (!wmsg.Contains("OK"))
                        {
                            wbandera = false;
                        }

                    }

                    catch (Exception exc)
                    {

                        wbandera = false;
                    }

                    Dtemp = new DataTable();

                    if (wbandera == true)
                    {

                        String wicarga = (wmsg.Split('-').ToList()[wmsg.Split('-').Length - 1]);
                        wicarga = wicarga.Substring(0, wicarga.IndexOf("\n\r"));
                        String tipo_carga = "";
                        var item = (from invoice in xdocarga.Descendants("parameter")
                                    where invoice.Attribute("id").Value == "tipo_carga"
                                    select new
                                    {

                                        tipo_carga = (String)invoice.Attribute("value").Value,
                                    }).FirstOrDefault();
                        if (item != null)
                        {
                            tipo_carga = item.tipo_carga.ToString();
                        }

                        Dtemp.Columns.Add(new DataColumn("PASE", Type.GetType("System.String")));
                        Dtemp.Columns.Add(new DataColumn("TIPO", Type.GetType("System.String")));
                        Dtemp.AcceptChanges();
                        Dtemp.Rows.Add(new String[] { wicarga, tipo_carga });
                    }


                    if (wvresult.Tables.Count <= 0)
                    {
                        wvresult.Tables.Add(Dtemp);
                    }
                    else
                    {
                        foreach (DataRow wrow in Dtemp.Rows)
                        {
                            wvresult.Tables[0].ImportRow(wrow);
                        }
                        wvresult.Tables[0].AcceptChanges();
                    }
                }
                catch (Exception exc)
                {
                    throw new DataException(string.Format("Error N4 Pase a Puerta BreakBulk{0} ",  exc.Message.ToString()));
                }
            }




            if (wvresult != null && wvresult.Tables.Count > 0 && wvresult.Tables[0].Rows.Count > 0)
            {

                System.Xml.Linq.XDocument docXML = new System.Xml.Linq.XDocument(new System.Xml.Linq.XDeclaration("1.0", "UTF-8", "yes"),
             new System.Xml.Linq.XElement("PASEPUERTAN4", from p in wvresult.Tables[0].AsEnumerable()
                                                          select new System.Xml.Linq.XElement("VBS_P_PASE_PUERTA_N4",
             new System.Xml.Linq.XAttribute("PASE", p.Field<String>("PASE") == null ? null : p.Field<String>("PASE").ToString()),
             new System.Xml.Linq.XAttribute("TIPO", p.Field<String>("TIPO") == null ? null : p.Field<String>("TIPO").ToString())
          )));

                XElement welementn4 = XElement.Parse(docXML.ToString());

                //Genera Pase a Puerta CGSA
                try
                {

                    dbmN4.VBS_P_CAN_PASE_PUERTA(welement, welementn4);

                }
                catch (Exception exc)
                {
                    throw new DataException(string.Format("Error WCF Pase a Puerta {0} ",  exc.Message.ToString()));
                }

            }
            return wvrporte;
        }
        public DataSet SaveActualizarPasePuerta(String wxml, String user)
        {
            //Declaracion de Parametros
            DBMMIDN4DataContext dbmN4 = new DBMMIDN4DataContext();
            XElement welement = XElement.Parse(wxml);
            DataTable Dtemp = new DataTable();
            DataSet wvresult = new DataSet();
            DataSet wvrporte = new DataSet();
            //ClSUTIL wutil = new ClSUTIL();
            Boolean wbandera = true;
            String wmsg = string.Empty;


                System.Xml.Linq.XDocument docXML = new System.Xml.Linq.XDocument(new System.Xml.Linq.XDeclaration("1.0", "UTF-8", "yes"),
             new System.Xml.Linq.XElement("PASEPUERTAN4", from p in wvresult.Tables[0].AsEnumerable()
                                                          select new System.Xml.Linq.XElement("VBS_P_PASE_PUERTA_N4",
             new System.Xml.Linq.XAttribute("PASE", p.Field<String>("PASE") == null ? null : p.Field<String>("PASE").ToString()),
             new System.Xml.Linq.XAttribute("TIPO", p.Field<String>("TIPO") == null ? null : p.Field<String>("TIPO").ToString())
          )));

                XElement welementn4 = XElement.Parse(docXML.ToString());

                //Genera Pase a Puerta CGSA
                try
                {

                    dbmN4.VBS_P_CAN_PASE_PUERTA(welement, welementn4);
                    dbmN4.SP_CANCELA_PASE_DE_PUERTA_WEB(welement, user);

                }
                catch (Exception exc)
                {
                    throw new DataException(string.Format("Error WCF Pase a Puerta {0} ", exc.Message.ToString()));
                }

            
            return wvrporte;
        }
        public void SaveTempTurno(String wxml)
        {
            //Declaracion de Parametros
            DBMMIDN4DataContext dbmN4 = new DBMMIDN4DataContext();
            ClSUTIL wutil = new ClSUTIL();
            //Genera Turno Temporal
            try
            {
                XElement welement = XElement.Parse(wxml);
                dbmN4.VBS_P_TEMP_TURNO(welement);

            }
            catch (Exception exc)
            {
                throw new DataException(string.Format("Error WCF Temp Turno {0} ",  exc.Message.ToString()));
            }

        }
        public void SaveDAI(String[] wxmln4DAI)
        {
            //Declaracion de Parametros
            DBMMIDN4DataContext dbmN4 = new DBMMIDN4DataContext();
            wsN4 wsn4 = new wsN4();
            ClSUTIL wutil = new ClSUTIL();

            //Actualiza DAI
            foreach (String Xmlparameter in wxmln4DAI)
            {
                try
                {
                    String wmsg = string.Empty;
                    String ruta = @"ICT/ECU/GYE/CGSA";

                    var i = wsn4.CallBasicService(ruta, Xmlparameter, ref wmsg);

                }
                catch (Exception exc)
                {

                }
            }

        }
        public Boolean IsTempTurno(String wxml)
        {
            //Declaracion de Parametros
            DBMMIDN4DataContext dbmN4 = new DBMMIDN4DataContext();
            ClSUTIL wutil = new ClSUTIL();
            Boolean wresult = true;
            String wsPLAN = null;
            String wsPLANsECUENCIA = null;
            try
            {


                //Obtencion de Valores



                wutil.ConsXml("GetTurnoTemp", "ID_PLAN", wxml, out wsPLAN);
                wutil.ConsXml("GetTurnoTemp", "ID_PLAN_SECUENCIA", wxml, out wsPLANsECUENCIA);

                int wPlan = int.Parse(wsPLAN);
                int wPLANsECUENCIA = int.Parse(wsPLANsECUENCIA);

                var wcount = (from Temp_turno in dbmN4.VBS_TEMP_TURNO
                              where Temp_turno.ID_PLAN == wPlan && Temp_turno.ID_PLAN_SECUENCIA == wPLANsECUENCIA
                              select Temp_turno).Count();
                if ((int)wcount > 0)
                {
                    wresult = false;
                }


            }
            catch (Exception exc)
            {
                wresult = false;
            }

            return wresult;
        }
        public DataSet RIDT(String wxml)
        {
            //Declaracion de Parametros
            DBMECUAPASSDataContext dbmecuapass = new DBMECUAPASSDataContext();
            DBMMIDN4DataContext dbmn4 = new DBMMIDN4DataContext();

            ClSUTIL wutil = new ClSUTIL();
            DataSet wresult = new DataSet();
            String MRN = null;
            String MSN = null;
            String HSN = null;
            String CONTENEDOR = null;

            try
            {

                //Obtencion de Valores
                wutil.ConsXml("RIDT", "MRN", wxml, out MRN);
                wutil.ConsXml("RIDT", "MSN", wxml, out MSN);
                wutil.ConsXml("RIDT", "HSN", wxml, out HSN);
                wutil.ConsXml("RIDT", "CONTENEDOR", wxml, out CONTENEDOR);


                if (((String.IsNullOrEmpty(MRN) == true) || (String.IsNullOrEmpty(MSN) == true) || (String.IsNullOrEmpty(HSN) == true)) && String.IsNullOrEmpty(CONTENEDOR) !=true)
                {

                    var wCarga = (from wcarga in dbmn4.CONTAINERS_BILL.Where(n4 => n4.CNTR_CONTAINER == CONTENEDOR)
                                  select wcarga).FirstOrDefault();
                    MRN = wCarga == null ? null : wCarga.CNTR_MRN;
                    MSN = wCarga == null ? null : wCarga.CNTR_MSN;
                    HSN = wCarga == null ? null : wCarga.CNTR_HSN;
                }


                var wresultvar = (from wdatos in dbmecuapass.ECU_RIDT.Where(ecu => ecu.MRN == MRN && ecu.MSN == MSN && ecu.HSN == HSN)
                                  select wdatos).ToList();

                wresult.Tables.Add(wutil.LINQToDataTable(wresultvar));


            }
            catch (Exception exc)
            {
                throw new DataException(string.Format("Error WCF RIDT {0} ",  exc.Message.ToString()));
            }

            return wresult;
        }
        public DataSet GetServices(String wxml)
        {


            //Declaracion de Parametros
            DBMMIDN4DataContext dbmN4 = new DBMMIDN4DataContext();
            ClSUTIL wutil = new ClSUTIL();
            DataSet wvresult = new DataSet();
            String ESTADO = null;

            //Obtencion de Valores
            wutil.ConsXml("SERVICE", "ESTADO", wxml, out ESTADO);

            //Consulta Servicios

            var wvqueryresul = (from Tmp in dbmN4.SERVICES_BILL.Where(tem => tem.life_cycle_state == (String.IsNullOrEmpty(ESTADO) != true ? ESTADO : tem.life_cycle_state))
                                select new
                                {
                                    ID_SERVICE = (String)Tmp.id.ToString(),
                                    SERVICE = (String)Tmp.description,
                                }).ToList();

            wvresult.Tables.Add(wutil.LINQToDataTable(wvqueryresul.OrderBy(Serv => Serv.ID_SERVICE)));

            return wvresult;
        }
        public DataSet GetBooking(String wxml)
        {


            //Declaracion de Parametros
            DBMMIDN4DataContext dbmN4 = new DBMMIDN4DataContext();
            ClSUTIL wutil = new ClSUTIL();
            DataSet wvresult = new DataSet();
            String REFERENCIA = null;
            String EXPORTADOR = null;

            //Obtencion de Valores
            wutil.ConsXml("BOOKING", "REFERENCIA", wxml, out REFERENCIA);
            wutil.ConsXml("BOOKING", "EXPORTADOR", wxml, out EXPORTADOR);



            //Consulta Servicios

               var wvqueryresul = (from Tmp in dbmN4.BOOKINGS_BILL.Where(tem => (tem.BKNG_CLTE_CLNT_CUSTOMER == (String.IsNullOrEmpty(EXPORTADOR) != true ? EXPORTADOR : tem.BKNG_CLTE_CLNT_CUSTOMER) ||

                tem.BKNG_CLTE_CLNT_CUSTOMER_LINE == (String.IsNullOrEmpty(EXPORTADOR) != true ? EXPORTADOR : tem.BKNG_CLTE_CLNT_CUSTOMER_LINE)) &&
                                                                                   tem.BKNG_VEPR_REFERENCE == (String.IsNullOrEmpty(REFERENCIA) != true ? REFERENCIA : tem.BKNG_VEPR_REFERENCE)/* && tem.BKNG_freight_kind=="FCL"*/)
                                select new
                                {
                                    ID_BOOKING = (String)Tmp.BKNG_BOOKING.ToString(),
                                    BOOKING = (String)Tmp.BKNG_BOOKING.ToString(),
                                }).ToList();



            wvresult.Tables.Add(wutil.LINQToDataTable(wvqueryresul.OrderBy(Serv => Serv.ID_BOOKING)));

            return wvresult;
        }
        
        public DataSet GetSysproControlCredito(String wxml)
        {
            DBMSYSPRODataContext dbmSyspro = new DBMSYSPRODataContext();
            DBMMIDN4DataContext   dbmN4 = new DBMMIDN4DataContext();
            ClSUTIL wutil = new ClSUTIL();
            DataSet wvresult = new DataSet();
            DataTable Dtresult = new DataTable();
            String EMPRESA = null;
            String TOTAL = null;
            String FECPAGO=null;
            String wmensaje = null;
            Boolean wresul = false;
            decimal WTOTAL = 0;
            String wuser = "";

            //Obtencion de Valores
            wutil.ConsXml("SYSPRO", "IDEMPRESA", wxml, out EMPRESA);
            wutil.ConsXml("SYSPRO", "TOTAL", wxml, out TOTAL);
            wutil.ConsXml("SYSPRO", "USER", wxml, out wuser);
            
            wresul=Boolean.Parse(dbmN4.FNA_FUN_LOCK_CLIENTS(EMPRESA).ToString());
            if (wresul)
            {
                wmensaje = "No esta Autorizado para la Facturación";
            }
            else
            {
                wmensaje = string.Empty;
            }
            Dtresult.Columns.Add(new DataColumn("LOCK_CLIENTS", Type.GetType("System.String")));
            Dtresult.Rows.Add(new String[] { wmensaje });
            Dtresult.AcceptChanges();
            wvresult.Tables.Add(Dtresult);
            wvresult.AcceptChanges();
            return wvresult;
        }
        
        /*
        public DataSet GetSysproControlCredito(String wxml)
        {
            DBMSYSPRODataContext dbmSyspro = new DBMSYSPRODataContext();
            DBMMIDN4DataContext   dbmN4 = new DBMMIDN4DataContext();
            ClSUTIL wutil = new ClSUTIL();
            DataSet wvresult = new DataSet();
            DataTable Dtresult = new DataTable();
            String EMPRESA = null;
            String TOTAL = null;
            String FECPAGO=null;
            String wmensaje = null;
            Boolean wresul = false;
            decimal WTOTAL = 0;
            String wuser = "";


            //Obtencion de Valores
            wutil.ConsXml("SYSPRO", "IDEMPRESA", wxml, out EMPRESA);
            wutil.ConsXml("SYSPRO", "TOTAL", wxml, out TOTAL);
            wutil.ConsXml("SYSPRO", "USER", wxml, out wuser);

            
            wresul=Boolean.Parse(dbmN4.FNA_FUN_LOCK_CLIENTS(EMPRESA).ToString());

            if (wresul)
            {
                wmensaje = "No esta Autorizado para la Facturación";

            }
            else
            {
                wresul = Boolean.Parse(dbmN4.FNA_FUN_LIBERATION_CLIENTS(EMPRESA).ToString());

                if (String.IsNullOrEmpty(TOTAL) != true)
                {
                    WTOTAL = decimal.Parse(TOTAL);
                }
                dbmSyspro.SYP_PRO_CONTROL_CREDITO(EMPRESA,WTOTAL,ref FECPAGO,ref wmensaje);


                if (!wresul)
                {

                    if (!String.IsNullOrEmpty(wmensaje))
                    {
                        try
                        {

                            var vmail = (from mensa in dbmN4.GEN_C_PARAMETROS
                                         where mensa.P_SUBTIPO == "CTRLCRE" && mensa.P_TIPO == "CTRLCRE"
                                         select new { mensa.P_VALOR3 }).FirstOrDefault();

                            if (vmail != null)
                            {
                                String lv_mensaje = "<html>Sistema de Control de Crédito CGSA,<br/><br/>";
                                lv_mensaje = lv_mensaje + "<strong>Mensaje: </strong> " + wmensaje + "<br/><br/><br/>";
                                lv_mensaje = lv_mensaje + "<br/><br/>" + "Mensaje Sistema de Control de Crédito emitido el : " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
                                lv_mensaje = lv_mensaje + "</HTML>";
                                String lmail = (String)vmail.P_VALOR3;
                                String le_error = "";
                                bool mail_ok = gendatos.obtiene_datos.cgsa_send_mail(lmail, null, "Control de Crédito CGSA - " + wuser, lv_mensaje, out le_error);
                            }
                        }
                        catch (Exception r)
                        {

                        }
                    }

                }
                else
                {
                    wmensaje = "";
                }
            }
            Dtresult.Columns.Add(new DataColumn("FECHA", Type.GetType("System.String")));
            Dtresult.Columns.Add(new DataColumn("MENSAJE", Type.GetType("System.String")));
            Dtresult.Rows.Add(new String[] { FECPAGO, wmensaje });
            Dtresult.AcceptChanges();
            wvresult.Tables.Add(Dtresult);
            wvresult.AcceptChanges();
            return wvresult;

        }
        */
        public DataSet GetPN(String wxml)
        {


            DataSet wvresult = new DataSet();
            ClSUTIL wutil = new ClSUTIL();

            DBMMIDN4DataContext dbmN4 = new DBMMIDN4DataContext();

            String wscontenedor = null;
            String wsPN = null;
            String Mrn = null;
            String Msn = null;
            String Hsn = null;
            String Referencia = null;
            String Tipo = null;

            wutil.ConsXml("ConsultaCancelarPN", "CONTENEDOR", wxml, out wscontenedor);
            wutil.ConsXml("ConsultaCancelarPN", "MRN", wxml, out Mrn);
            wutil.ConsXml("ConsultaCancelarPN", "MSN", wxml, out Msn);
            wutil.ConsXml("ConsultaCancelarPN", "HSN", wxml, out Hsn);
            wutil.ConsXml("ConsultaCancelarPN", "Type", wxml, out Tipo);
            wutil.ConsXml("ConsultaCancelarPN", "REFERENCIA", wxml, out Referencia);
            wutil.ConsXml("ConsultaCancelarPN", "PN", wxml, out wsPN);


            //Consulta Pase a Puerta N4


            //var wvqueryPase = (from vbs_t_pase in dbmN4.VBS_T_PASE_PUERTA.Where(Pase => Pase.TIPO_CARGA == "CNTR")
            //                   join vbs_contenedor in dbmN4.CONTAINERS_BILL on vbs_t_pase.ID_CARGA equals vbs_contenedor.CNTR_CONSECUTIVO
            //                   where vbs_t_pase.ESTADO != "CA" &&
            //                         vbs_t_pase.NUMERO_PASE_N4 == wsPN && vbs_t_pase.TIPO_CARGA == "CNTR"
            //                   select new { vbs_t_pase.ID_PASE, vbs_t_pase.ID_CARGA, CARGA = vbs_contenedor.CNTR_CONTAINER, vbs_t_pase.TIPO_CARGA, vbs_t_pase.NUMERO_PASE_N4, vbs_t_pase.FECHA_EXPIRACION, vbs_t_pase.USUARIO_REGISTRO, vbs_t_pase.ID_RESERVA, RESERVA = (DateTime.Today.CompareTo(vbs_t_pase.FECHA_EXPIRACION) == -1 ? Boolean.Parse("true") : Boolean.Parse("false")), CANCELAR = Boolean.Parse("false"), }).ToList();


            var wvqueryPase = (from vbs_t_pase in dbmN4.FNA_FUN_CONTAINERS_BILL_PASE_N5(Mrn, Msn, Hsn, wscontenedor)
                               select new { vbs_t_pase.ID_PASE, vbs_t_pase.ID_CARGA, CARGA = vbs_t_pase.CNTR_CONTAINER, vbs_t_pase.TIPO_CARGA, vbs_t_pase.NUMERO_PASE_N4, vbs_t_pase.FECHA_EXPIRACION, vbs_t_pase.USUARIO_REGISTRO, vbs_t_pase.ID_RESERVA, INFORMACION = (String)"", RESERVA = (DateTime.Today.CompareTo(vbs_t_pase.FECHA_EXPIRACION) == -1 ? Boolean.Parse("true") : Boolean.Parse("false")), CANCELAR = Boolean.Parse("false"), }).ToList();

            if (wsPN == null)
            {

      //var container =  ( from vbs_contenedor in dbmN4.CONTAINERS_BILL
      //                             where  vbs_contenedor.CNTR_VEPR_REFERENCE.Trim() == (Referencia == null ? vbs_contenedor.CNTR_VEPR_REFERENCE.Trim() : Referencia) &&
      //                               vbs_contenedor.CNTR_MRN.Trim() == (Mrn == null ? vbs_contenedor.CNTR_MRN.Trim() : Mrn) &&
      //                               vbs_contenedor.CNTR_MSN == (Msn == null ? vbs_contenedor.CNTR_MSN : Msn) &&
      //                               vbs_contenedor.CNTR_HSN == (Hsn == null ? vbs_contenedor.CNTR_HSN : Hsn) &&
      //                               vbs_contenedor.CNTR_CONTAINER.Trim() == (wscontenedor == null ? vbs_contenedor.CNTR_CONTAINER.Trim() : wscontenedor)
      //                   select new { CNTR_CONSECUTIVO = vbs_contenedor.CNTR_CONSECUTIVO, vbs_contenedor.CNTR_CONTAINER }).Distinct().ToList();

      var container = (from vbs_contenedor in dbmN4.FNA_FUN_CONTAINERS_BILL_N5(Mrn, Msn, Hsn, null, null, null, null, null)
                       select new { CNTR_CONSECUTIVO = vbs_contenedor.CNTR_CONSECUTIVO, vbs_contenedor.CNTR_CONTAINER }).Distinct().ToList();



                if(container !=null && container.Count()>0){

                    //var wpase = (from vbs_t_pase in dbmN4.VBS_T_PASE_PUERTA.Where(Pase => Pase.TIPO_CARGA == "CNTR" && Pase.ESTADO != "CA" && Pase.FECHA_EXPIRACION >= DateTime.Today)
                    //            select new { vbs_t_pase.ID_PASE, vbs_t_pase.ID_CARGA, vbs_t_pase.TIPO_CARGA, vbs_t_pase.NUMERO_PASE_N4, vbs_t_pase.FECHA_EXPIRACION, vbs_t_pase.USUARIO_REGISTRO, vbs_t_pase.ID_RESERVA, RESERVA = (DateTime.Today.CompareTo(vbs_t_pase.FECHA_EXPIRACION) == -1 ? Boolean.Parse("true") : Boolean.Parse("false")), CANCELAR = Boolean.Parse("false"), }).ToList();

                    //wvqueryPase = wvqueryPase.Concat((from vbs_t_pase in wpase
                    //                                  join cnt in container on vbs_t_pase.ID_CARGA equals cnt.CNTR_CONSECUTIVO
                    //                                  select new { vbs_t_pase.ID_PASE, vbs_t_pase.ID_CARGA, CARGA = cnt.CNTR_CONTAINER, vbs_t_pase.TIPO_CARGA, vbs_t_pase.NUMERO_PASE_N4, vbs_t_pase.FECHA_EXPIRACION, vbs_t_pase.USUARIO_REGISTRO, vbs_t_pase.ID_RESERVA, RESERVA = (DateTime.Today.CompareTo(vbs_t_pase.FECHA_EXPIRACION) == -1 ? Boolean.Parse("true") : Boolean.Parse("false")), CANCELAR = Boolean.Parse("false"), }).ToList()).ToList(); 

                    wvqueryPase = (from vbs_t_pase in dbmN4.FNA_FUN_CONTAINERS_BILL_PASE_N5(Mrn, Msn, Hsn, wscontenedor)
                                    select new { vbs_t_pase.ID_PASE, vbs_t_pase.ID_CARGA, CARGA = vbs_t_pase.CNTR_CONTAINER, vbs_t_pase.TIPO_CARGA, vbs_t_pase.NUMERO_PASE_N4, vbs_t_pase.FECHA_EXPIRACION, vbs_t_pase.USUARIO_REGISTRO, vbs_t_pase.ID_RESERVA, INFORMACION = (String)"", RESERVA = (DateTime.Today.CompareTo(vbs_t_pase.FECHA_EXPIRACION) == -1 ? Boolean.Parse("true") : Boolean.Parse("false")), CANCELAR = Boolean.Parse("false"), }).ToList();
                }
            }


            
            //if (String.IsNullOrEmpty(wscontenedor))
            //{
            //    //dbmN4.VBS_CONSULTA_PASE_BREAK(Mrn, Msn, Hsn)
            //     var wvqueryPase_brbk = (from vbs_t_pase in dbmN4.VBS_CONSULTA_PASE_BREAK(Mrn, Msn, Hsn)
            //                   select new { vbs_t_pase.ID_PASE, vbs_t_pase.ID_CARGA, CARGA = vbs_t_pase.BRBK_MRN + "-" + vbs_t_pase.BRBK_MSN + "-" + vbs_t_pase.BRBK_HSN, vbs_t_pase.TIPO_CARGA, vbs_t_pase.NUMERO_PASE_N4, vbs_t_pase.FECHA_EXPIRACION, vbs_t_pase.USUARIO_REGISTRO, vbs_t_pase.ID_RESERVA, RESERVA = (DateTime.Today.CompareTo(vbs_t_pase.FECHA_EXPIRACION) == -1 ? Boolean.Parse("true") : Boolean.Parse("false")), CANCELAR = Boolean.Parse("false"), }).ToList();
            //}

            if (wvqueryPase != null && wvqueryPase.Count > 0 )
            {

                //wvresult.Tables.Add((wutil.LINQToDataTable(wvqueryPase.Take(1))));
                wvresult.Tables.Add((wutil.LINQToDataTable(wvqueryPase)));
            }
            else
            {
                var wvqueryPase_brbk = (from vbs_t_pase in dbmN4.VBS_CONSULTA_PASE_BREAK(Mrn, Msn, Hsn)
                                        select new { vbs_t_pase.ID_PASE, vbs_t_pase.ID_CARGA, CARGA = vbs_t_pase.BRBK_MRN + "-" + vbs_t_pase.BRBK_MSN + "-" + vbs_t_pase.BRBK_HSN, vbs_t_pase.TIPO_CARGA, vbs_t_pase.NUMERO_PASE_N4, vbs_t_pase.FECHA_EXPIRACION, vbs_t_pase.USUARIO_REGISTRO, vbs_t_pase.ID_RESERVA, INFORMACION = (String)vbs_t_pase.INFORMACION, RESERVA = (DateTime.Today.CompareTo(vbs_t_pase.FECHA_EXPIRACION) == -1 ? Boolean.Parse("true") : Boolean.Parse("false")), CANCELAR = Boolean.Parse("false"), }).ToList();

                wvresult.Tables.Add((wutil.LINQToDataTable(wvqueryPase_brbk.Distinct().ToList())));
            }
            return wvresult;
        }
        public DataSet RImprePasePuerta(String xmlreporte, String Wtype)
        {
            DataSet wvrporte = new DataSet();
            try
            {
                DBMMIDN4DataContext dbmN4 = new DBMMIDN4DataContext();
                XElement welementn4 = XElement.Parse(xmlreporte);
                ClSUTIL wutil = new ClSUTIL();

                if (Wtype == "CNTR")
                {
                    dbmN4.CommandTimeout = 1800;
              
                    var wvqueryn4NCA = dbmN4.VBS_REPORT_PASE_PUERTA(welementn4, Wtype);
                    var wreporte = (from wresult in wvqueryn4NCA
                                    select wresult).ToList();

                    wvrporte.Tables.Add(wutil.LINQToDataTable(wreporte));
                }
                else
                {                    
                    var wvqueryn4NCAbrk = dbmN4.VBS_REPORT_PASE_PUERTA_BREAK(welementn4, Wtype);
                    var wreporte_brk = (from wresult in wvqueryn4NCAbrk
                                        select wresult).ToList();
                    wvrporte.Tables.Add(wutil.LINQToDataTable(wreporte_brk));
                }

                
                
            }
            catch (Exception exc)
            {
                throw new DataException(string.Format("Error WCF ReImprePase a Puerta {0} ",  exc.Message.ToString()));
            }
            

            return wvrporte;
        }
        public void EXECICU(String xmlparameter)
        {
            try
            {
                XDocument xdoc = new XDocument();
                string wruta = @"ICT/ECU/GYE/CGSA";
                xdoc = XDocument.Parse(xmlparameter);
                ClSUTIL wutil = new ClSUTIL();
                String lv_event_type = System.Configuration.ConfigurationManager.AppSettings["EvenCancelaPN"].ToString();

                wsN4 g = new wsN4();

                var wresul = (from ICU in xdoc.Descendants("icu")
                              select new
                              {
                                  CARGA = (String)ICU.Attribute("CARGA"),
                                  USUARIO = (String)ICU.Attribute("USUARIO"),
                                  FECHA = (String)DateTime.Now.ToString("yyyy-MM-dd"),
                              }).ToList();
                DataTable DtResult = new DataTable();
                DtResult = wutil.LINQToDataTable(wresul);
                foreach (DataRow DROW in DtResult.Rows)
                {
                    String a = string.Format("<icu><units><unit-identity id=\"{0}\" type=\"CONTAINERIZED\"/>" +
                               "</units><properties><property tag=\"UnitRemark\" value=\"PROGRAMACION CONTENEDORES\"/>" +
                               "</properties><event id=\"{3}\" note=\"PROGRAMACION DE CONTENEDORES\" time-event-applied=\"{1}\" " +
                               "user-id=\"{2}\" /></icu>", DROW["CARGA"].ToString(), DROW["USUARIO"].ToString(), DROW["FECHA"].ToString(), lv_event_type);
                    string me = string.Empty;
                    var i = g.CallBasicService(wruta, a, ref me);
                }

            }
            catch (Exception)
            {
                throw;
            }
        }
        public void EXECICU_tmp(String xmlparameter,int tipo)
        {
            try 
	         {
                    XDocument xdoc = new XDocument();
                    string wruta = @"ICT/ECU/GYE/CGSA";
                    xdoc = XDocument.Parse(xmlparameter);
                    ClSUTIL wutil = new ClSUTIL();
	                String lv_event_type_primero = System.Configuration.ConfigurationManager.AppSettings["EvenCancelaPN"].ToString();
                    String lv_event_type_segundo = System.Configuration.ConfigurationManager.AppSettings["EvenCancelaPNOthers"].ToString();
                    String lv_event_type;
                    if(tipo==1)
                        lv_event_type = lv_event_type_primero;
                    else
                        lv_event_type = lv_event_type_segundo;

                    wsN4 g = new wsN4();
                
            var wresul = (from ICU in xdoc.Descendants("icu")
                          select new
                          {   CARGA = (String)ICU.Attribute("CARGA"),
                              USUARIO = (String)ICU.Attribute("USUARIO"),
                              FECHA = (String)DateTime.Now.ToString("yyyy-MM-dd"),
                          } ).ToList();
                DataTable DtResult = new DataTable();
                DtResult = wutil.LINQToDataTable(wresul);
                foreach (DataRow DROW in DtResult.Rows)
            {    String a = string.Format("<icu><units><unit-identity id=\"{0}\" type=\"CONTAINERIZED\"/>" +
                            "</units><properties><property tag=\"UnitRemark\" value=\"PROGRAMACION CONTENEDORES\"/>" +
                            "</properties><event id=\"{3}\" note=\"PROGRAMACION DE CONTENEDORES\" time-event-applied=\"{1}\" " +
                            "user-id=\"{2}\" /></icu>", DROW["CARGA"].ToString(), DROW["USUARIO"].ToString(), DROW["FECHA"].ToString(), lv_event_type);
                string me = string.Empty;
                var i = g.CallBasicService(wruta, a, ref me);
            }    
     
        }
	    catch (Exception)
	    { throw;
	    }
       }
        public Boolean  ISDateValidoLiblockCliente(String wxml)
        {
            Boolean wresult = true;

            DBMMIDN4DataContext dbmN4 = new DBMMIDN4DataContext();
            ClSUTIL wutil = new ClSUTIL();

            String wsCliente = null;
            String wsFecha = null;
            String wsTipo = null;
            DateTime wFecha;
            int wicont = 0;
            try
            {

                wutil.ConsXml("GetCliente", "ID_Cliente", wxml, out wsCliente);
                wutil.ConsXml("GetCliente", "Fecha", wxml, out wsFecha);
                wutil.ConsXml("GetCliente", "Tipo", wxml, out wsTipo);

               
                DateTime.TryParseExact(wsFecha, "MM/dd/yyyy", new System.Globalization.CultureInfo("es-EC"), System.Globalization.DateTimeStyles.None, out wFecha);


                if (wsTipo == "LIB")
                {
                    var wcount = (from Lib_cliente in dbmN4.FNA_LIBERATION_CLIENTS
                                  where wFecha >= Lib_cliente.LCLNT_START_DATE && wFecha <= Lib_cliente.LCLNT_END_DATE &&
                                  wsCliente==Lib_cliente.LCLNT_CUSTOMER
                                  select Lib_cliente).Count();

                    wicont = (int)wcount;

                }
                else
                {
                    var wcount = (from Lib_cliente in dbmN4.FNA_LOCK_CLIENTS
                                  where wFecha >= Lib_cliente.LCLNT_START_DATE && wFecha <= Lib_cliente.LCLNT_END_DATE &&
                                  wsCliente == Lib_cliente.LCLNT_CUSTOMER
                                  select Lib_cliente).Count();

                    wicont = (int)wcount;
                }
                
                    
                if ((int)wicont > 0)
                {
                    wresult = false;
                }


            }
            catch (Exception exc)
            {
                wresult = false;
            }
            return wresult;


        }
        public void SaveLiberacion_Clientes(String wxml)
        {
            //Declaracion de Parametros
            DBMMIDN4DataContext dbmN4 = new DBMMIDN4DataContext();
            ClSUTIL wutil = new ClSUTIL();
            
            try
            {
                XElement welement = XElement.Parse(wxml);
                dbmN4.FNA_P_LIBERATION_CLIENTS(welement);

            }
            catch (Exception exc)
            {
                throw new DataException(string.Format("Error WCF Liberacion de Clientes {0} ", exc.Message.ToString()));
            }

        }
        public void SaveLock_Clientes(String wxml)
        {
            //Declaracion de Parametros
            DBMMIDN4DataContext dbmN4 = new DBMMIDN4DataContext();
            ClSUTIL wutil = new ClSUTIL();

            try
            {
                XElement welement = XElement.Parse(wxml);
                dbmN4.FNA_P_LOCK_CLIENTS(welement);

            }
            catch (Exception exc)
            {
                throw new DataException(string.Format("Error WCF Lock de Clientes {0} ", exc.Message.ToString()));
            }

        }
        public DataSet GetLibClientes(String wxml)
        {
            //Declaracion de Parametros
            DBMMIDN4DataContext dbmN4 = new DBMMIDN4DataContext();
            ClSUTIL wutil = new ClSUTIL();
            DataSet wvresult = new DataSet();
            String wsCliente = null;

            //Obtencion de Valores
            wutil.ConsXml("GetCliente", "ID_Cliente", wxml, out wsCliente);

            var wvqueryempresa = (from wvempresa in dbmN4.CLIENTS_BILL
                                  where wvempresa.CLNT_CUSTOMER==wsCliente &&   wvempresa.CLNT_ACTIVE == "Y"
                                  select new
                                  {                                  
                                      EMPRESA = (String)wvempresa.CLNT_CUSTOMER + " - " + wvempresa.CLNT_NAME,
                                  }).FirstOrDefault();

            var wvqueryresul = (from Tmp in dbmN4.FNA_LIBERATION_CLIENTS.Where(tem => (tem.LCLNT_CUSTOMER ==wsCliente))
                                select new
                                {
                                    SECUENCIAL   = (String)Tmp.LCLNT_SECUENCIAL.ToString(),
                                    CUSTOMER     = (String)Tmp.LCLNT_CUSTOMER.ToString(),
                                    CUSTOMERNAME = (String)( wvqueryempresa.EMPRESA==null?"": wvqueryempresa.EMPRESA).ToString(),
                                    DATE         = (String)Tmp.LCLNT_START_DATE.ToString(),
                                    END_DATE     = (String)Tmp.LCLNT_END_DATE.ToString(),
                                    COMMENTS     = (String)Tmp.LCLNT_COMMENTS.ToString(),
                                    STATUS       = Boolean.Parse( (Tmp.LCLNT_STATUS=="N"? "false":"true").ToString()),
                                }).ToList();

            wvresult.Tables.Add(wutil.LINQToDataTable(wvqueryresul.OrderBy(Serv => Serv.SECUENCIAL)));

            return wvresult;
        }
        public DataSet GetLockClientes(String wxml)
        {
            //Declaracion de Parametros
            DBMMIDN4DataContext dbmN4 = new DBMMIDN4DataContext();
            ClSUTIL wutil = new ClSUTIL();
            DataSet wvresult = new DataSet();
            String wsCliente = null;

            //Obtencion de Valores
            wutil.ConsXml("GetCliente", "ID_Cliente", wxml, out wsCliente);

            var wvqueryempresa = (from wvempresa in dbmN4.CLIENTS_BILL
                                  where wvempresa.CLNT_CUSTOMER == wsCliente && wvempresa.CLNT_ACTIVE == "Y"
                                  select new
                                  {
                                      EMPRESA = (String)wvempresa.CLNT_CUSTOMER + " - " + wvempresa.CLNT_NAME,
                                  }).FirstOrDefault();

            var wvqueryresul = (from Tmp in dbmN4.FNA_LOCK_CLIENTS.Where(tem => (tem.LCLNT_CUSTOMER == wsCliente))
                                select new
                                {
                                    SECUENCIAL = (String)Tmp.LCLNT_SECUENCIAL.ToString(),
                                    CUSTOMER = (String)Tmp.LCLNT_CUSTOMER.ToString(),
                                    CUSTOMERNAME = (String)(wvqueryempresa.EMPRESA == null ? "" : wvqueryempresa.EMPRESA).ToString(),
                                    DATE = (String)Tmp.LCLNT_START_DATE.ToString(),
                                    END_DATE = (String)Tmp.LCLNT_END_DATE.ToString(),
                                    COMMENTS = (String)Tmp.LCLNT_COMMENTS.ToString(),
                                    STATUS = Boolean.Parse((Tmp.LCLNT_STATUS == "N" ? "false" : "true").ToString()),
                                }).ToList();

            wvresult.Tables.Add(wutil.LINQToDataTable(wvqueryresul.OrderBy(Serv => Serv.SECUENCIAL)));

            return wvresult;
        }
        public DataSet RLibLockCliente(String wxml)
        {
            

            DBMMIDN4DataContext dbmN4 = new DBMMIDN4DataContext();
            ClSUTIL wutil = new ClSUTIL();
            String wsCliente = null;
            String wsFecha = null;
            String wsFechaFin = null;
            String wsTipo = null;
            DateTime wFecha;
            DateTime wFechaFin;
            String wstatus = null;
            DataSet wvrporte = new DataSet();
            
            try
            {

                wutil.ConsXml("REPORT_CLIENT", "ID_Cliente", wxml, out wsCliente);
                wutil.ConsXml("REPORT_CLIENT", "Fecha", wxml, out wsFecha);
                wutil.ConsXml("REPORT_CLIENT", "FechaFin", wxml, out wsFechaFin);
                wutil.ConsXml("REPORT_CLIENT", "Tipo", wxml, out wsTipo);

                DateTime.TryParseExact(wsFecha, "MM/dd/yyyy", new System.Globalization.CultureInfo("es-EC"), System.Globalization.DateTimeStyles.None, out wFecha);
                DateTime.TryParseExact(wsFechaFin, "MM/dd/yyyy", new System.Globalization.CultureInfo("es-EC"), System.Globalization.DateTimeStyles.None, out wFechaFin);

                if (wsTipo == "LIB")
                {
                    var wresult = dbmN4.FNA_PRO_LIBERATION_CLIENTS(wFecha, wFechaFin, wstatus, wsCliente);

                    wvrporte.Tables.Add(wutil.LINQToDataTable(wresult));
                }
                else
                {
                    var wresult = dbmN4.FNA_PRO_LOCK_CLIENTS(wFecha, wFechaFin, wstatus, wsCliente);
                     wvrporte.Tables.Add(wutil.LINQToDataTable(wresult));
                }

            }
            catch (Exception exc)
            {
                throw new DataException(string.Format("Error WCF en el Reporte {0} ", exc.Message.ToString()));
            }
            return wvrporte;

        }      
        public DataSet GetCatalogoCgsa(String wxml)
        {

            //Declaracion de Parametros

            DataSet wvresult = new DataSet();
            ClSUTIL wutil = new ClSUTIL();
            
            DBMMIDN4DataContext dbmN4 = new DBMMIDN4DataContext();

            String wvTabla;
            String wsEstado = null;
            try
            {

                //Obtencion de Valores
                wutil.ConsXml("CatalogoCgsa", "TABLA", wxml, out wvTabla);
                wutil.ConsXml("CatalogoCgsa", "ESTADO", wxml, out wsEstado);

                var wcatalogo = dbmN4.cgsa_fn_return_catalogo(wvTabla, wsEstado);
                wvresult.Tables.Add(wutil.LINQToDataTable(wcatalogo));
            }
            catch (Exception exc)
            {
                throw new DataException(string.Format("Error WCF al Obtener el Catalogo  {0} ", exc.Message.ToString()));
            }

            
            return wvresult;
        }
        public DataSet GetCFSSubItem(String wxml)
        {
            //Declaracion de Parametros
            DBMMIDN4DataContext dbmN4 = new DBMMIDN4DataContext();
            ClSUTIL wutil = new ClSUTIL();
            DataSet wvresult = new DataSet();
            String wsMrn = null;
            String wsMsn = null;
            String wshsn = null;
            String wsidcarga = null;
            try
            {
                //Obtencion de Valores
                wutil.ConsXml("GetCfsSubItem", "MRN", wxml, out wsMrn);
                wutil.ConsXml("GetCfsSubItem", "MSN", wxml, out wsMsn);
                wutil.ConsXml("GetCfsSubItem", "HSN", wxml, out wshsn);
                wutil.ConsXml("GetCfsSubItem", "ID_CARGA", wxml, out wsidcarga);

                DateTime Wfechas;
                DateTime.TryParseExact(DateTime.Now.ToString("MM/dd/yyyy"), "MM/dd/yyyy", new System.Globalization.CultureInfo("es-EC"), System.Globalization.DateTimeStyles.None, out Wfechas);

                //Obtencion de Valores
                //CNTR=CONTENEDRO BRBK=BREAKBUKLL CFS=CFS
                //Consulta Pase a Puerta N4
                 /*IEnumerable<String> wvqueryn4NCA = (from vbs_t_pase in dbmN4.VBS_T_PASE_PUERTA
                                    join vbs_d_pase in dbmN4.VBS_DET_PASE_PUERTA on  vbs_t_pase.ID_PASE equals vbs_d_pase.ID_PASE
                                                     where vbs_t_pase.ID_CARGA.ToString() == wsidcarga &&
                                                           vbs_t_pase.ESTADO != "CA" &&
                                                           vbs_t_pase.TIPO_CARGA == "CFS" &&
                                                           vbs_t_pase.FECHA_EXPIRACION >= Wfechas
                                    select (String) vbs_d_pase.ID_SUBSEC_CFS.ToString());*/

                var wdpp = (from vbs_t_pase in dbmN4.VBS_T_PASE_PUERTA
                                    join vbs_d_pase in dbmN4.VBS_DET_PASE_PUERTA on  vbs_t_pase.ID_PASE equals vbs_d_pase.ID_PASE
                                                     where vbs_t_pase.ID_CARGA.ToString() == wsidcarga &&
                                                           vbs_t_pase.ESTADO != "CA" &&
                                                           vbs_t_pase.TIPO_CARGA == "CFS" &&
                                                           vbs_t_pase.FECHA_EXPIRACION >= Wfechas
                                    select (String) vbs_d_pase.ID_SUBSEC_CFS.ToString()).ToList();
                List<string> lwdpp = new List<string>();
                lwdpp.AddRange(wdpp);

                /*
                 IEnumerable<String> wcfs = (from tarja in dbmN4.CFS_TARJA.Where(XA => (XA.MRN == wsMrn && XA.MSN == wsMsn))
                                             join item in dbmN4.CFS_TARJA_ITEM.Where(XB => (XB.HSN == null ? "0000" : XB.HSN) == wshsn) on tarja.CODIGO_TARJA equals item.CODIGO_TARJA
                                             join subtiten in dbmN4.CFS_TARJA_SUBITEM.Where(XE => !wvqueryn4NCA.Contains(XE.CODIGO_TARJA_SUBITEM.ToString())) on new { item.CODIGO_TARJA, item.CODIGO_TARJA_ITEM } equals new { subtiten.CODIGO_TARJA, subtiten.CODIGO_TARJA_ITEM }
                                             where subtiten.ESTADO == true
                                             select (String)  tarja.MRN + "-" + tarja.MSN + "-" + (item.HSN == null ? "0000" : item.HSN).ToString()
                                             );*/

                var wcfs = (from tarja in dbmN4.CFS_TARJA.Where(XA=> (XA.MRN==wsMrn && XA.MSN==wsMsn ))
                            join item in dbmN4.CFS_TARJA_ITEM.Where(XB => (XB.HSN == null ? "0000" : XB.HSN) == wshsn) on tarja.CODIGO_TARJA equals item.CODIGO_TARJA
                            //join subtiten in dbmN4.CFS_TARJA_SUBITEM.Where(XE => !wvqueryn4NCA.Contains(XE.CODIGO_TARJA_SUBITEM.ToString())) on new 
                            join subtiten in dbmN4.CFS_TARJA_SUBITEM on //new
                            //item.CODIGO_TARJA equals subtiten.CODIGO_TARJA 
                            item.CODIGO_TARJA_ITEM equals subtiten.CODIGO_TARJA_ITEM
                            //&& subtiten.CODIGO_TARJA_SUBITEM

                            where subtiten.ESTADO == true && !lwdpp.Contains(subtiten.CODIGO_TARJA_SUBITEM.ToString())
                            select new { CARGA = tarja.MRN +"-"+ tarja.MSN + "-"+ (item.HSN == null ? "0000" : item.HSN),
                                         CONSECUTIVO = subtiten.CODIGO_TARJA_SUBITEM,
                                         CANTIDAD = subtiten.CANTIDAD,
                                         ASIGNADOPN = Boolean.Parse("false"),
                                         EMPRESA= (String)null,
                                         PLACA = (String)null,
                                         CHOFER = (String)null,
                            }).ToList();
                

               wvresult.Tables.Add(wutil.LINQToDataTable(wcfs));
            }
            catch (Exception ex )
            {

                throw new DataException(string.Format("Error WCF al Obtener el Subtitem CFS  {0} ", ex.Message.ToString()));
            }

            return wvresult;
        }
        public DataSet GetObserFactura(String wxml)
        {
            //Declaracion de Parametros
            DBMMIDN4DataContext dbmN4 = new DBMMIDN4DataContext();
            ClSUTIL wutil = new ClSUTIL();
            DataSet wvresult = new DataSet();
            String wsMrn = null;
            
            try
            {
                //Obtencion de Valores
                wutil.ConsXml("GetObserFactura", "MRN", wxml, out wsMrn);

                var vss = (from vssprm in dbmN4.VESSELS_PROGRAM
                           where vssprm.VEPR_CUSTOM_IMPO_REGISTER == wsMrn || vssprm.VEPR_CUSTOM_EXPO_REGISTER == wsMrn
                           select new { NAVE= vssprm.VEPR_VSSL_NAME,
                                        VIAJE= vssprm.VEPR_VOYAGE,
                                        FEC_ARRIBO= vssprm.VEPR_ACTUAL_DEPART,
                                        REFERENCIA= vssprm.VEPR_REFERENCE }
                           ).ToList();

                wvresult.Tables.Add(wutil.LINQToDataTable(vss));
            }
            catch (Exception ex)
            {

                throw new DataException(string.Format("Error WCF al Obtener el Observaciones de Factura:  {0} ", ex.Message.ToString()));
            }

            return wvresult;
        }
        public DataSet GetPNAsignacion(String wxml)
        {


            DataSet wvresult = new DataSet();
            ClSUTIL wutil = new ClSUTIL();

            DBMMIDN4DataContext dbmN4 = new DBMMIDN4DataContext();

            String wscontenedor = null;
            String wsPN = null;
            
            
             wutil.ConsXml("ConsultaAsgPN", "PN", wxml, out wsPN);



             try
             {

                 Decimal wdPN = Decimal.Parse(wsPN);
            
                 var wvqueryPase = (from vbs_t_pase in dbmN4.VBS_T_PASE_PUERTA.Where(Pase => Pase.TIPO_CARGA == "CNTR")
                               join vbs_contenedor in dbmN4.CONTAINERS_BILL on vbs_t_pase.ID_CARGA equals vbs_contenedor.CNTR_CONSECUTIVO
                               where vbs_t_pase.ESTADO != "CA" &&
                               vbs_t_pase.ID_PASE == wdPN && vbs_t_pase.TIPO_CARGA == "CNTR" && vbs_t_pase.FECHA_EXPIRACION.HasValue  && vbs_t_pase.ID_RESERVA > 0
                                    
                                    select new { vbs_t_pase.ID_PASE, vbs_t_pase.ID_CARGA, CARGA = vbs_contenedor.CNTR_CONTAINER, vbs_t_pase.TIPO_CARGA, vbs_t_pase.NUMERO_PASE_N4, FECHA_EXPIRACION =vbs_t_pase.FECHA_EXPIRACION.Value.Date, vbs_t_pase.USUARIO_REGISTRO, vbs_t_pase.ID_RESERVA, }).ToList();

                 var wvquery= (from wquer in wvqueryPase
                              select new { wquer.ID_PASE, wquer.ID_CARGA,wquer.CARGA, wquer.TIPO_CARGA, wquer.NUMERO_PASE_N4,FECHA_EXPIRACION= wquer.FECHA_EXPIRACION.ToString("MM/dd/yyyy"),wquer.USUARIO_REGISTRO,ID_RESERVA=Decimal.Parse(wquer.ID_RESERVA.ToString()), }).ToList();
                wvresult.Tables.Add((wutil.LINQToDataTable(wvquery.Take(1))));
            
             }
             catch (Exception ex)
             {
                 throw new DataException(string.Format("Error WCF Asignacion Turno {0} ", ex.Message.ToString()));
                 
             }
             return wvresult;

        }
        public void SavePNAsignacion(String wxml)
        {
            //Declaracion de Parametros
            DBMMIDN4DataContext dbmN4 = new DBMMIDN4DataContext();
            ClSUTIL wutil = new ClSUTIL();

            try
            {
                XElement welement = XElement.Parse(wxml);
                dbmN4.VBS_P_ASIG_TURNO(welement);

            }
            catch (Exception exc)
            {
                throw new DataException(string.Format("Error WCF Asignacion de Turno {0} ", exc.Message.ToString()));
            }

        }
        public DataSet GetUserFactura(String wxml)
        {
            DBMMIDN4DataContext dbmN4 = new DBMMIDN4DataContext();
            ClSUTIL wutil = new ClSUTIL();
            String wsFactura = null;
            DataSet wvrporte = new DataSet();

            try
            {

                wutil.ConsXml("REPORT_USER", "FACTURA", wxml, out wsFactura);

                dbmN4.CommandTimeout = 3600;

                var wresult = (from bil in dbmN4.BIL_INVOICE
                               where bil.NUMERO_INVOICE.Contains(wsFactura)
                               select new { CARGA = bil.ID_CARGA, TIPO_CARGA = bil.TIPO_CARGA, DRAFT = bil.DRAFT_CARGA, DRAFT_FINAL = bil.DRAFT_INVOICE, FACTURA = bil.NUMERO_INVOICE, FECHA_FACTURADA = bil.FECHA_INGRESO, USUARIO = bil.USUARIO_INGRESO, AGENTE=bil.AGENTE,COMENTARIO= bil.COMENTARIO }).ToList();

                wvrporte.Tables.Add(wutil.LINQToDataTable(wresult));


            }
            catch (Exception exc)
            {
                throw new DataException(string.Format("Error WCF en el Reporte Usuario Factura{0} ", exc.Message.ToString()));
            }
            return wvrporte;

        }
    }
}
 

