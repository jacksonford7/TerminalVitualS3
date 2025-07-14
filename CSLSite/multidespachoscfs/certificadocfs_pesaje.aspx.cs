using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BillionEntidades;
using System.Text;

namespace CSLSite
{
    public partial class certificadocfs_pesaje : System.Web.UI.Page
    {
        string sid;
        string lg;
        string _mrn = string.Empty;
        string _msn = string.Empty;
        string _hsn = string.Empty;
        string cMensajes;

        protected void Page_Init(object sender, EventArgs e)
        {
            if (!Request.IsAuthenticated)
            {
                var ex = new ApplicationException(string.Format("IP:{0},URL:{1},METODO:{2}, Tabla de impresiones vacía", Request.UserHostAddress, Request.Url, Request.HttpMethod));
                this.PersonalResponse(string.Format("Está intentando acceder a un área que requiere autenticación, por su seguridad los datos de su equipo han quedado registrados, gracias. <br/>{0} <br/>{1},<br/>{2}", Request.UserHostAddress, Request.Url, Request.HttpMethod), null);
                return;
            }

            try
            {
                _mrn = Request.QueryString["mrn"];
                _msn = Request.QueryString["msn"];
                _hsn = Request.QueryString["hsn"];

            

                if (Request.QueryString["mrn"] == null || string.IsNullOrEmpty(_mrn))
                {
                    var ex = new ApplicationException(string.Format("IP:{0},URL:{1},METODO:{2}, Id de AISV nó válido", Request.UserHostAddress, Request.Url, Request.HttpMethod));               
                    this.PersonalResponse(string.Format("Está intentando acceder a un área que requiere autenticación, por su seguridad los datos de su equipo han quedado registrados, gracias. <br/>{0} <br/>{1},<br/>{2}", Request.UserHostAddress, Request.Url, Request.HttpMethod), null);
                    return;
                }
            }
            catch
            {
               
                string close = CSLSite.CslHelper.ExitForm("Hubo un problema durante el inicio de la impresión");
                base.Response.Write(close);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Response.IsClientConnected)
            {
                if (!IsPostBack)

                {
                    try
                    {

                        var Tabla = cfs_ver_certificado.ver_certificado(_mrn, _msn, _hsn, out cMensajes);
                        if (Tabla == null)
                        {
                            string close = CSLSite.CslHelper.ExitForm("No existe información que mostrar");
                            base.Response.Write(close);
                            return;
                        }
                        if (Tabla.Count <= 0)
                        {
                            string close = CSLSite.CslHelper.ExitForm("No existe información que mostrar");
                            base.Response.Write(close);
                            return;
                        }

                        StringBuilder tab = new StringBuilder();

                        if (Tabla.Count > 0 && Tabla.Count <= 20)
                        {
                            var query = (from p in Tabla.Where(x => !string.IsNullOrEmpty(x.MRN))
                                                         select new { NUMERO_CARGA = string.Format("{0}-{1}-{2}", p.MRN, p.MSN, p.HSN),
                                                         AGENTE = string.Format("{0}-{1}", p.AGENTE, p.AGENTE_DESC),
                                                         CLIENTE = string.Format("{0}-{1}", p.FACTURADO, p.FACTURADO_DESC),
                                                             TEXTO1 = p.TEXTO1,
                                                             TEXTO2 = p.TEXTO2,
                                                             TITULO1 = p.TITULO1,
                                                             TITULO2 = p.TITULO2
                                                         }).FirstOrDefault();

                            tab.Append("<section class='certificado'>");
                            tab.Append("<header>");
                            tab.AppendFormat("<h1 style='text-align:center;'><strong><span>{0}</span></strong>", query.TITULO1);
                            tab.AppendFormat("<strong><span >{0}</span></strong>", query.TITULO2);
                            tab.Append("</h1>");
                            tab.Append("</header>");
                            tab.Append("<div>");
                            tab.Append("<br/>");
                            tab.Append("<ul>");
                            tab.Append("<br/>");
                            tab.AppendFormat("<li><strong><span runat='server'>Número Carga:</span></strong><span>{0}</span></li>", query.NUMERO_CARGA);
                            tab.AppendFormat("<li><strong><span runat='server'>Agente de Aduana:</span></strong><span>{0}</span></li>", query.AGENTE);
                            tab.AppendFormat("<li><strong> <span runat='server'>Cliente:</span></strong><span>{0}</span></li>", query.CLIENTE);
                            tab.Append("</ul>");
                            tab.Append("<br/>");

                            tab.Append("<table width='722' border='1' cellpadding='0' cellspacing='0'>");
                            tab.Append("<tr>" +
                                "<td width = '187' height = '22'  valign = 'top' bgcolor = '#F3F3F3'><div align = 'center'><strong># CERTIFICADO</strong></div></td>" +
                                "<td width = '110' valign = 'top' bgcolor = '#F3F3F3'><div align = 'center'><strong>ALTO</strong></div></td>" +
                                "<td width = '113' valign = 'top' bgcolor = '#F3F3F3'><div align = 'center'><strong>ANCHO</strong></div></td>" +
                                "<td width = '116' valign = 'top' bgcolor = '#F3F3F3'><div align = 'center'><strong>LARGO</strong></div></td>" +
                                "<td width = '111' valign = 'top' bgcolor = '#F3F3F3'><div align = 'center'><strong>PESO</strong></div></td>" +
                                "<td width = '120' valign = 'top' bgcolor = '#F3F3F3'><div align = 'center'><strong>VOLUMEN</strong></div></td>" +
                                "</tr>");

                            int i = 0;
                            foreach (var Det in Tabla)
                            {
                                if (i <= 20)
                                {
                                    tab.AppendFormat("<tr>" +
                                   "<td height='23' valign='top'><div align='center'>{0}</td>" +
                                   "<td valign='top'><div align='center'>{1}</div></td>" +
                                   "<td valign='top'><div align='center'>{2}</div></td>" +
                                   "<td valign='top'><div align='center'>{3}</div></td>" +
                                   "<td valign='top'><div align='center'>{4}</div></td>" +
                                   "<td valign='top'><div align='center'>{5}</div></td>" +
                                   "</tr>",
                                   String.IsNullOrEmpty(Det.NUMERO_CERTIFICADO) ? "..." : Det.NUMERO_CERTIFICADO,
                                    Det.P2D_ALTO == 0 ? "..." : Det.P2D_ALTO.ToString("N2"),
                                    Det.P2D_ANCHO == 0 ? "..." : Det.P2D_ANCHO.ToString("N2"),
                                    Det.P2D_LARGO == 0 ? "..." : Det.P2D_LARGO.ToString("N2"),
                                    Det.PESO == 0 ? "..." : Det.PESO.ToString("N2"),
                                    Det.P2D_VOLUMEN == 0 ? "..." : Det.P2D_VOLUMEN.ToString("N2")
                                    );
                                }
                               
                                i++;
                            }

                            tab.Append("</table>");
                            tab.Append("<div class='signature'>");
                            tab.AppendFormat("<span runat='server'{0}</span>", query.TEXTO1);
                            tab.Append("<br>");
                            tab.AppendFormat("<strong><span runat='server'>{0}</span></strong>", query.TEXTO2);
                            tab.Append("</div>");
                            tab.Append("<div class='logos'>");
                            tab.Append("<img src='../shared/imgs/carbono_img/logoContecon.jpg' width='230'>");
                            tab.Append("</div>");
                            tab.Append("</div>");
                            tab.Append("<footer>");
                           
                            tab.Append("</footer>");
                            tab.Append("</section>");
                   
                        }
                        else
                        {
                            var query = (from p in Tabla.Where(x => !string.IsNullOrEmpty(x.MRN))
                                         select new
                                         {
                                             NUMERO_CARGA = string.Format("{0}-{1}-{2}", p.MRN, p.MSN, p.HSN),
                                             AGENTE = string.Format("{0}-{1}", p.AGENTE, p.AGENTE_DESC),
                                             CLIENTE = string.Format("{0}-{1}", p.FACTURADO, p.FACTURADO_DESC),
                                             TEXTO1 = p.TEXTO1,
                                             TEXTO2 = p.TEXTO2,
                                             TITULO1 = p.TITULO1,
                                             TITULO2 = p.TITULO2
                                         }).FirstOrDefault();


                            tab.Append("<section class='certificado'>");
                            tab.Append("<header>");
                            tab.AppendFormat("<h1 style='text-align:center;'><strong><span>{0}</span></strong>", query.TITULO1);
                            tab.AppendFormat("<strong><span >{0}</span></strong>",query.TITULO2 );
                            tab.Append("</h1>");
                            tab.Append("</header>");
                            tab.Append("<div>");
                            tab.Append("<br/>");
                            tab.Append("<ul>");
                            tab.Append("<br/>");
                            tab.AppendFormat("<li><strong><span runat='server'>Número Carga:</span></strong><span>{0}</span></li>", query.NUMERO_CARGA);
                            tab.AppendFormat("<li><strong><span runat='server'>Agente de Aduana:</span></strong><span>{0}</span></li>", query.AGENTE);
                            tab.AppendFormat("<li><strong> <span runat='server'>Cliente:</span></strong><span>{0}</span></li>", query.CLIENTE);
                            tab.Append("</ul>");
                            tab.Append("<br/>");

                            tab.Append("<table width='722' border='1' cellpadding='0' cellspacing='0'>");
                            tab.Append("<tr>" +
                                "<td width = '187' height = '22'  valign = 'top' bgcolor = '#F3F3F3'><div align = 'center'><strong># CERTIFICADO</strong></div></td>" +
                                "<td width = '110' valign = 'top' bgcolor = '#F3F3F3'><div align = 'center'><strong>ALTO</strong></div></td>" +
                                "<td width = '113' valign = 'top' bgcolor = '#F3F3F3'><div align = 'center'><strong>ANCHO</strong></div></td>" +
                                "<td width = '116' valign = 'top' bgcolor = '#F3F3F3'><div align = 'center'><strong>LARGO</strong></div></td>" +
                                "<td width = '111' valign = 'top' bgcolor = '#F3F3F3'><div align = 'center'><strong>PESO</strong></div></td>" +
                                "<td width = '120' valign = 'top' bgcolor = '#F3F3F3'><div align = 'center'><strong>VOLUMEN</strong></div></td>" +
                                "</tr>");

                            int i = 0;
                            foreach (var Det in Tabla)
                            {
                                if (i <= 20)
                                {
                                    tab.AppendFormat("<tr>" +
                                   "<td height='23' valign='top'><div align='center'>{0}</td>" +
                                   "<td valign='top'><div align='center'>{1}</div></td>" +
                                   "<td valign='top'><div align='center'>{2}</div></td>" +
                                   "<td valign='top'><div align='center'>{3}</div></td>" +
                                   "<td valign='top'><div align='center'>{4}</div></td>" +
                                   "<td valign='top'><div align='center'>{5}</div></td>" +
                                   "</tr>",
                                   String.IsNullOrEmpty(Det.NUMERO_CERTIFICADO) ? "..." : Det.NUMERO_CERTIFICADO,
                                    Det.P2D_ALTO == 0 ? "..." : Det.P2D_ALTO.ToString("N2"),
                                    Det.P2D_ANCHO == 0 ? "..." : Det.P2D_ANCHO.ToString("N2"),
                                    Det.P2D_LARGO == 0 ? "..." : Det.P2D_LARGO.ToString("N2"),
                                    Det.PESO == 0 ? "..." : Det.PESO.ToString("N2"),
                                    Det.P2D_VOLUMEN == 0 ? "..." : Det.P2D_VOLUMEN.ToString("N2")
                                    );
                                }

                                i++;
                            }

                            tab.Append("</table>");
                            //tab.Append("<div class='signature'>");
                            //tab.Append("<span runat='server'>Javier Lancha de Micheo</span>");
                            //tab.Append("<br>");
                            //tab.Append("<strong><span runat='server'>C.E.O.</span></strong>");
                            //tab.Append("</div>");
                            tab.Append("<div class='logos'>");
                            tab.Append("<img src='../shared/imgs/carbono_img/logoContecon.jpg' width='230'>");
                            tab.Append("</div>");
                            tab.Append("</div>");
                            tab.Append("<footer>");
                            tab.Append("<div>");
                            tab.Append("<span runat='server'><strong>Página 1 de 2</strong></span>");
                            tab.Append("</div>");
                            tab.Append("</footer>");
                            tab.Append("</section>");


                            tab.Append("<section class='certificado'>");
                            tab.Append("<header>");
                            tab.AppendFormat("<h1 style='text-align:center;'><strong><span>{0}</span></strong>", query.TITULO1);
                            tab.AppendFormat("<strong><span >{0}</span></strong>", query.TITULO2);
                            tab.Append("</h1>");
                            tab.Append("</header>");
                            tab.Append("<div>");
                            tab.Append("<br/>");
                            tab.Append("<ul>");
                            tab.Append("<br/>");
                            tab.AppendFormat("<li><strong><span runat='server'>Número Carga:</span></strong><span>{0}</span></li>", query.NUMERO_CARGA);
                            tab.AppendFormat("<li><strong><span runat='server'>Agente de Aduana:</span></strong><span>{0}</span></li>", query.AGENTE);
                            tab.AppendFormat("<li><strong> <span runat='server'>Cliente:</span></strong><span>{0}</span></li>", query.CLIENTE);
                            tab.Append("</ul>");
                            tab.Append("<br/>");

                            tab.Append("<table width='722' border='1' cellpadding='0' cellspacing='0'>");
                            tab.Append("<tr>" +
                                "<td width = '187' height = '22'  valign = 'top' bgcolor = '#F3F3F3'><div align = 'center'><strong># CERTIFICADO</strong></div></td>" +
                                "<td width = '110' valign = 'top' bgcolor = '#F3F3F3'><div align = 'center'><strong>ALTO</strong></div></td>" +
                                "<td width = '113' valign = 'top' bgcolor = '#F3F3F3'><div align = 'center'><strong>ANCHO</strong></div></td>" +
                                "<td width = '116' valign = 'top' bgcolor = '#F3F3F3'><div align = 'center'><strong>LARGO</strong></div></td>" +
                                "<td width = '111' valign = 'top' bgcolor = '#F3F3F3'><div align = 'center'><strong>PESO</strong></div></td>" +
                                "<td width = '120' valign = 'top' bgcolor = '#F3F3F3'><div align = 'center'><strong>VOLUMEN</strong></div></td>" +
                                "</tr>");

                            i = 21;
                            foreach (var Det in Tabla)
                            {
                                if (i <= Tabla.Count)
                                {
                                    tab.AppendFormat("<tr>" +
                                   "<td height='23' valign='top'><div align='center'>{0}</td>" +
                                   "<td valign='top'><div align='center'>{1}</div></td>" +
                                   "<td valign='top'><div align='center'>{2}</div></td>" +
                                   "<td valign='top'><div align='center'>{3}</div></td>" +
                                   "<td valign='top'><div align='center'>{4}</div></td>" +
                                   "<td valign='top'><div align='center'>{5}</div></td>" +
                                   "</tr>",
                                   String.IsNullOrEmpty(Det.NUMERO_CERTIFICADO) ? "..." : Det.NUMERO_CERTIFICADO,
                                    Det.P2D_ALTO == 0 ? "..." : Det.P2D_ALTO.ToString("N2"),
                                    Det.P2D_ANCHO == 0 ? "..." : Det.P2D_ANCHO.ToString("N2"),
                                    Det.P2D_LARGO == 0 ? "..." : Det.P2D_LARGO.ToString("N2"),
                                    Det.PESO == 0 ? "..." : Det.PESO.ToString("N2"),
                                    Det.P2D_VOLUMEN == 0 ? "..." : Det.P2D_VOLUMEN.ToString("N2")
                                    );
                                }

                                i++;
                            }

                            tab.Append("</table>");
                            tab.Append("<div class='signature'>");
                            tab.AppendFormat("<span runat='server'>{0}</span>", query.TEXTO1);
                            tab.Append("<br>");
                            tab.AppendFormat("<strong><span runat='server'>{0}</span></strong>", query.TEXTO2);
                            tab.Append("</div>");
                            tab.Append("<div class='logos'>");
                            tab.Append("<img src='../shared/imgs/carbono_img/logoContecon.jpg' width='230'>");
                            tab.Append("</div>");
                            tab.Append("</div>");
                            tab.Append("<footer>");
                            tab.Append("<div>");
                            tab.Append("<br>");
                            tab.Append("<strong><span runat='server'>Página 2 de 2</span><strong>");
                            tab.Append("</div>");
                            tab.Append("</footer>");
                            tab.Append("</section>");

                        }

                      

                        //tab.Append("<table width='722' border='1' cellpadding='0' cellspacing='0'>");
                        //tab.Append("<tr>" +
                        //    "<td width = '187' height = '22'  valign = 'top' bgcolor = '#F3F3F3'><div align = 'center'><strong># CERTIFICADO</strong></div></td>" +
                        //    "<td width = '110' valign = 'top' bgcolor = '#F3F3F3'><div align = 'center'><strong>ALTO</strong></div></td>" +
                        //    "<td width = '113' valign = 'top' bgcolor = '#F3F3F3'><div align = 'center'><strong>ANCHO</strong></div></td>" +
                        //    "<td width = '116' valign = 'top' bgcolor = '#F3F3F3'><div align = 'center'><strong>LARGO</strong></div></td>" +
                        //    "<td width = '111' valign = 'top' bgcolor = '#F3F3F3'><div align = 'center'><strong>PESO</strong></div></td>" +
                        //    "<td width = '120' valign = 'top' bgcolor = '#F3F3F3'><div align = 'center'><strong>VOLUMEN</strong></div></td>"+
                        //    "</tr>");
                      

                        //foreach (var Det in Tabla)
                        //{
                        //    this.numero_carga.InnerText = string.Format("{0}-{1}-{2}",Det.MRN, Det.MSN, Det.HSN);
                        //    this.agente.InnerText = string.Format("{0}-{1}", Det.AGENTE, Det.AGENTE_DESC);
                        //    this.cliente.InnerText = string.Format("{0}-{1}", Det.FACTURADO, Det.FACTURADO_DESC);

                        //    tab.AppendFormat("<tr>" +
                        //   "<td height='23' valign='top'><div align='center'>{0}</td>" +
                        //   "<td valign='top'><div align='center'>{1}</div></td>" +
                        //   "<td valign='top'><div align='center'>{2}</div></td>" +
                        //   "<td valign='top'><div align='center'>{3}</div></td>" +
                        //   "<td valign='top'><div align='center'>{4}</div></td>" +
                        //   "<td valign='top'><div align='center'>{5}</div></td>" +
                        //   "</tr>",
                        //   String.IsNullOrEmpty(Det.NUMERO_CERTIFICADO) ? "..." : Det.NUMERO_CERTIFICADO,
                        //    Det.P2D_ALTO==0 ? "..." : Det.P2D_ALTO.ToString("N2"),
                        //    Det.P2D_ANCHO == 0 ? "..." : Det.P2D_ANCHO.ToString("N2"),
                        //    Det.P2D_LARGO == 0 ? "..." : Det.P2D_LARGO.ToString("N2"),
                        //    Det.PESO == 0 ? "..." : Det.PESO.ToString("N2"),
                        //    Det.P2D_VOLUMEN == 0 ? "..." : Det.P2D_VOLUMEN.ToString("N2")
                        //   );

                        //}

                        //tab.Append("</table>");

                        this.pagina.InnerHtml = tab.ToString();

                      

                    }
                    catch
                    {
                        string close = CSLSite.CslHelper.ExitForm("Hubo un problema general durante la conversión para la búsqueda");
                        base.Response.Write(close);
                    }
                }
            }
        }

        private string TextFinal(string cfname, List<dbconfig> cfgs, string lan)
        {
            string[] textos;
            string salida;
            bool arl = false;
            var sebusca = cfname.ToString().Trim().ToUpper();
            var cfg = cfgs.Where(a => a.config_name.Trim().ToUpper().Equals(sebusca)).FirstOrDefault();
            salida = cfg != null ? cfg.config_value : "";
            textos = salida.Split('+');
            arl = textos.Length > 1;
            salida = lan == "S" ? textos[0] : arl ? textos[1] : "-";
            return salida.Trim();
        }
    }
}