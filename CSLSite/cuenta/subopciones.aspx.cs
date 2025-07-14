using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ServiceModel;
using System.Text;
using System.Data;
using System.IO;
using System.Collections;

using System.Web.Security;
using System.Configuration;
using System.Globalization;
using System.Xml.Linq;
using System.Xml;


namespace CSLSite.cuenta
{
    public partial class subopciones : System.Web.UI.Page
    {
        private string IdServicios = string.Empty;
        private string IdFiltro= string.Empty;
        private string Mensaje_Error = string.Empty;
        int IdNServicio = 0;
        int IdNFiltro = 0;
        //private void mensaje(string error)
        //{
        //    Session["error"] = Cls_QuerySegura.securetext(error);
        //    Response.Redirect(string.Format("~/errores/error.aspx?error={0}", Session["error"]), false);
        //}

        //private string Opciones(Usuario user, int acceso_servicio, string Token)
        //{

        //    string opcion = string.Empty;

        //    string error = string.Empty;

        //    var div = new StringBuilder();
        //    if (user != null)
        //    {
        //        try
        //        { 
        //            var zonas_user = Zona.Listar(user.IdCorporacion.Value, user.empresa.Value, user.IdUsuario.Value);
        //            if (!zonas_user.Exitoso)
        //            {
        //                Mensaje_Error = string.Format("No tiene accesos definidos. {0}", zonas_user.MensajeProblema);
        //                this.mensaje(Mensaje_Error);
        //                return Mensaje_Error;
        //            }
        //            else
        //            {
        //                var opciones_user = Servicio.Listar(acceso_servicio, user.IdUsuario.Value);
        //                if (!opciones_user.Exitoso)
        //                {
        //                    Mensaje_Error = string.Format("No tiene accesos definidos. {0}", opciones_user.MensajeProblema);
        //                    this.mensaje(Mensaje_Error);
        //                    return Mensaje_Error;
        //                }
        //                else
        //                {
        //                    var datos_zona = zonas_user.Resultado.Where(a => a.servicio == acceso_servicio).FirstOrDefault();
        //                    string nombre_zona = string.Empty;
        //                    nombre_zona = datos_zona.zonatitulo.Trim().Replace("&nbsp;", "").Trim();
        //                    IdServicio = Request.QueryString["opcion"];
        //                    this.opcion_principal.InnerHtml = string.Format("<a href=\"../menu/subopciones.aspx?opcion={0}\">{1}</a>", IdServicio, nombre_zona);
        //                    this.sub_opcion.InnerHtml = string.Empty;

        //                    div.AppendFormat("<div class=\"row mt-4\">");

        //                    foreach (var item in opciones_user.Resultado)
        //                    {
        //                        opcion = item.descripcion.Trim().Replace("&nbsp;", "");
        //                        div.AppendFormat("<div class=\"col-md-3\"><div class=\"card p-3 m-2\">");
        //                        div.AppendFormat("<img src=\"../img/solicitud.png\" width=\"30\"><div class=\"card-body p-0 mt-3\"><a class=\"bold\" href =\"{0}?opcion={1}\"><h5 class=\"card-title m-0\">{2}</h5></a>", item.url, IdServicio, opcion);
        //                        div.AppendFormat("<p class=\"card-text\">{0}</p>", item.textointro);
        //                        div.AppendFormat("</div>");
        //                        div.AppendFormat("</div></div>");
        //                    }

        //                    div.AppendFormat("</div>");
        //                }
        //            }


        //        }
        //        //error local
        //        catch (Exception ex)
        //        {

        //            lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(Opciones), "Opciones", false, null, null, ex.StackTrace, ex);
        //            OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
        //            Mensaje_Error = string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError);
        //            this.mensaje(Mensaje_Error);
        //            return Mensaje_Error;
        //        }

        //    }
        //    else
        //    {
        //        return string.Format("No tiene accesos definidos...no existe información del usuario ");

        //    }

        //    return div.ToString();
        //}

        private void mensaje()
        {
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "mensaje('');", true);
        }

        protected string jsarguments(object idServicio, object idFiltro)
        {
            return string.Format("{0};{1}", idServicio != null ? idServicio.ToString().Trim() : "0", idFiltro != null ? idFiltro.ToString().Trim() : "0");
        }


        public  string SubOpciones(usuario user, int acceso_servicio, ref HashSet<string> p_lista, int? filtro)
        {
           
            string opcion = string.Empty;
            int? Filtro_Default = null;

            var div = new StringBuilder();
            if (user != null)
            {
                //var permisos = user.autorized_access();
                //if (permisos == null)
                //{
                //    return string.Format("No tiene accesos definidos");
                //}

                filtro = (filtro.Value.Equals(0) ? null : filtro);

                //var opciones_user = CLSData.ValorLecturas2("[TV].[sp_user_options_tv_tmp]", tComando.Procedure,  acceso_servicio , user.id,   filtro  );
                //if (opciones_user == null)
                //{
                //    return string.Format("No tiene accesos definidos");
                //}

                var datos_zona = user.autorized_zones().Where(a => a.idservicio == acceso_servicio).FirstOrDefault();

                string nombre_zona = string.Empty;
                nombre_zona = datos_zona.titulo.Trim().Replace("&nbsp;", "").Trim();

                var xpars = Request.QueryString["opcion"].ToString().Split(';');
                IdServicios = xpars[0];

                this.opcion_principal.InnerHtml = string.Format("<a href=\"\">{0}</a>", nombre_zona);
               // this.opcion_principal.InnerHtml = string.Format("<a href=\"../cuenta/subopciones.aspx?opcion={0}\">{1}</a>", IdServicios, nombre_zona);
                this.sub_opcion.InnerHtml = string.Empty;

                div.AppendFormat("<div class=\"row mt-4\">");

                //arma estructura de botones de filtro
                var OpcionesFiltros = CLSData.ValorLecturas("[TV].[sp_user_filtro_tv_tmp]", tComando.Procedure, new Dictionary<string, string>() { { "idservicio", acceso_servicio.ToString() }, { "idusuario", user.id.ToString() } });
                if (OpcionesFiltros != null)
                {
                    div.AppendFormat("<div class=\"col-md-12 d-flex left-content-between\">");
                    div.AppendFormat("<h3 class=\"mt-4\">&nbsp;</h3>");
                    div.AppendFormat("<div class=\"d-flex align-items-center\">");
                    div.AppendFormat("<span class=\"mr-2\">Filtrar por: </span>");
                    int i = 1;
                    foreach (var item in OpcionesFiltros)
                    {
                        int? IdFiltro = item[0] as int?;

                        if (i.Equals(1))
                        {
                            Filtro_Default = IdFiltro;
                        }
                        
                        string NombreOpcion = item[1] as string;
                        string TextIdFiltro = QuerySegura.EncryptQueryString(IdFiltro.ToString());
                        string estilo = "btn btn-outline-primary mr-2 py-1 px-4";
                        if (filtro.HasValue)
                        {
                            if (filtro.Value.Equals(IdFiltro))
                            {
                                estilo = "btn btn-primary mr-2 py-1 px-4";
                            }
                        }
                        else
                        {
                            if (Filtro_Default.HasValue && i.Equals(1))
                            {
                               estilo = "btn btn-primary mr-2 py-1 px-4";  
                            }
                        }
                        string boton = string.Format("<a class=\"{0}\" id=\"{1}\" href=\"../cuenta/subopciones.aspx?opcion={2}\">{3}</a>", estilo, IdFiltro.ToString(),jsarguments(IdServicios, TextIdFiltro), NombreOpcion);
                        //div.AppendFormat(string.Format("<button type=\"button\" class=\"btn btn-outline-primary mr-2 py-1 px-4\">{0}</button>", NombreOpcion));
                        div.AppendFormat(boton);
                        i++;
                    }
                    div.AppendFormat("</div>");
                    div.AppendFormat("</div>");
                }

                var opciones_user_Filtro = CLSData.ValorLecturas2("[TV].[sp_user_options_tv_tmp]", tComando.Procedure, acceso_servicio, user.id, (!filtro.HasValue ? Filtro_Default : filtro));
                if (opciones_user_Filtro == null)
                {
                    return string.Format("No tiene accesos definidos");
                }

                foreach (var item in opciones_user_Filtro)
                {
                    var z = new opcion();
                    z.idservicio = item[0] as int?;
                    z.idopcion = item[1] as int?;
                    z.descripcion = item[2] as string;
                    z.icono = item[3] as string;
                    z.textointro = item[4] as string;
                    z.url = item[5] as string;

                    opcion = z.descripcion.Trim().Replace("&nbsp;", "");

                    //z.icono = "../img/p2d.png";
                    string Icono = string.IsNullOrEmpty(z.icono) ? "../img/solicitud.png" : z.icono;

                    div.AppendFormat("<div class=\"col-md-3\"><div class=\"card p-3 m-2\">");
                    div.AppendFormat("<img src=" + Icono + " width=\"30\"><div class=\"card-body p-0 mt-3\"><a class=\"bold\" href =\"{0}?opcion={1}\"><h5 class=\"card-title m-0\">{2}</h5></a>", z.url, IdServicios, opcion);
                    div.AppendFormat("<p class=\"card-text\">{0}</p>", z.textointro);
                    div.AppendFormat("</div>");
                    div.AppendFormat("</div></div>");

                    if (!string.IsNullOrEmpty(z.url))
                    {
                        if (p_lista != null)
                        {
                            p_lista.Add(z.url.Split('/').LastOrDefault());
                        }
                    }

                }

                div.AppendFormat("</div>");

            }
            return div.ToString();
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                try
                {
                    if (Session["control"] == null)
                    {
                        Response.Redirect("../login.aspx");
                    }

                    var xpars = Request.QueryString["opcion"].ToString().Split(';');
                    //string opciones = QuerySegura.DecryptQueryString(Request.QueryString["opcion"].ToString());
                    
                    IdServicios = QuerySegura.DecryptQueryString(xpars[0]);
                    IdFiltro = QuerySegura.DecryptQueryString(xpars[1]);

                    if (IdServicios == null || string.IsNullOrEmpty(IdServicios))
                    {
                        Response.Redirect("../login.aspx");
                        return;
                    }
                    else
                    {
                        IdServicios = IdServicios.Trim().Replace("\0", string.Empty);
                        IdFiltro = IdFiltro.Trim().Replace("\0", string.Empty);

                        if (!int.TryParse(IdServicios, out IdNServicio))
                        {
                            this.sub_menu.InnerHtml = string.Format("<b>Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible..error al convertir {0}", IdServicios);
                            return;
                        }

                        if (!int.TryParse(IdFiltro, out IdNFiltro))
                        {
                            this.sub_menu.InnerHtml = string.Format("<b>Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible..error al convertir {0}", IdFiltro);
                            return;
                        }

                        var hs = new HashSet<string>();

                        var user = usuario.Deserialize(Session["control"].ToString());
                        this.sub_menu.InnerHtml = SubOpciones(user, IdNServicio, ref hs, IdNFiltro);
                        Session["acceso"] = hs;

                        if (IdNFiltro == 10)
                        {
                            //mensaje();
                        }


                    }

                }
                //error local
                catch (Exception ex)
                {

                    var t = this.getUserBySesion();
                    this.PersonalResponse(string.Format("Ops.. ha ocurrido un error inesperado, por favor repórtelo con este código EX-{0} y reintente en unos minutos.", csl_log.log_csl.save_log<Exception>(ex, "zones", "Page_Load", "Hubo un error al cargar menú", t.loginname)), "../login.aspx", true);

                }

            }
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            if (!Request.IsAuthenticated)
            {
                csl_log.log_csl.save_log<ApplicationException>(new ApplicationException("No autenticado"), "zones", "Page_Init", "No autenticado", "No disponible");
                this.PersonalResponse("Para acceder a esta área necesita estar autenticado, será redireccionado a la página de login", "../login.aspx", true);
                return;
            }
            Page.SslOn();

        }
    }
}