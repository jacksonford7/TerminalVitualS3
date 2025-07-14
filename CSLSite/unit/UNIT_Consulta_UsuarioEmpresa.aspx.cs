using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BillionEntidades;
using BreakBulk;
using CSLSite.EmpresaInfoCls;

namespace CSLSite.unit
{
    public partial class UNIT_Consulta_UsuarioEmpresa : System.Web.UI.Page
    {
        #region "Clases"

        private Cls_Bil_Sesion objSesion = new Cls_Bil_Sesion();

        usuario ClsUsuario;

        private Cls_Bil_Invoice_Cabecera objFactura = new Cls_Bil_Invoice_Cabecera();
        private Cls_Bil_Cabecera objCabecera = new Cls_Bil_Cabecera();
        #region "variables"
        DataSet dsContenedor;
        DateTime dtFechaHoy;
        DateTime dtFechaFin;
        int numeroDias = 0;


        private string cMensajes;
        private string sddlvalor = string.Empty;


        private DateTime dfecha_desde;
        private DateTime dfecha_hasta;
        private string snumero_carga;

        #endregion "variables"

        #region "Propiedades"

        private Int64? nSesion
        {
            get
            {
                return (Int64)Session["nSesion"];
            }
            set
            {
                Session["nSesion"] = value;
            }

        }

        public bool UnidadDesconectada { get; private set; }

        private void OcultarLoading(string valor)
        {
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "ocultarloader('" + valor + "');", true);
        }

        private void Actualiza_Paneles()
        {
            UPDETALLE.Update();
        }

        private void Mostrar_Mensaje(int Tipo, string Mensaje)
        {

            this.banmsg.Visible = true;
            this.banmsg.InnerHtml = Mensaje;
            OcultarLoading("1");

            this.Actualiza_Paneles();
        }

        private void Crear_Sesion()
        {
            objSesion.USUARIO_CREA = ClsUsuario.loginname;
            nSesion = objSesion.SaveTransaction(out cMensajes);
            if (!nSesion.HasValue || nSesion.Value <= 0)
            {
                this.Mostrar_Mensaje(2, string.Format("<b>Error! No se pudo generar el id de la sesión, por favor comunicarse con el departamento de IT de CGSA... {0}</b>", cMensajes));
                return;
            }
            this.hf_BrowserWindowName.Value = nSesion.ToString().Trim();

            objCabecera = new Cls_Bil_Cabecera();
            Session["Transaccion" + this.hf_BrowserWindowName.Value] = objCabecera;
        }

        #endregion

        protected void Page_Init(object sender, EventArgs e)
        {

            try
            {
                if (!IsPostBack)
                {
                    Page.SslOn();
                    sddlvalor = "Ctrn";
                }

                if (!Request.IsAuthenticated)
                {
                    Response.Redirect("../login.aspx", false);

                    return;
                }

                this.banmsg.Visible = IsPostBack;

                if (!Page.IsPostBack)
                {
                    this.banmsg.InnerText = string.Empty;

                    try
                    {

                    }
                    catch (Exception ex)
                    {

                    }

                }
#if !DEBUG
                this.IsAllowAccess();
#endif

                ClsUsuario = Page.Tracker();
                if (ClsUsuario != null)
                {
                    if (!Page.IsPostBack)
                    {

                        this.Actualiza_Paneles();
                    }

                }
                else
                {
                    ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());
                }

            }
            catch (Exception ex)
            {
                this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Error! </b>{0}", ex.Message));
            }


        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                try
                {
                    this.Crear_Sesion();

                    var listInfo = Cls_Empresa_Info.ObtenerEmpresaInfo(ClsUsuario.ruc);
                    var empresaInfo = listInfo.FirstOrDefault();

                    if (empresaInfo != null)
                    {
                        Session["EmpresaOriginal"] = empresaInfo;
                        // Sección 1: Tipo de Cliente
                        string tipoCliente = empresaInfo.TipoCliente?.ToUpperInvariant();

                        chkExportador.Checked = tipoCliente.Contains("EXPORTADOR");
                        chkImportador.Checked = tipoCliente.Contains("IMPORTADOR");
                        chkOperador.Checked = tipoCliente.Contains("OPERADOR");
                        chkProveedor.Checked = tipoCliente.Contains("PROVEEDOR");

                        // Sección 2: Información del Cliente
                        txtRazonSocial.Text = empresaInfo.RazonSocial;
                        txtIdentificacion.Text = empresaInfo.RucCiPasaporte;

                        // Lógica simple para marcar tipo de identificación
                        if (!string.IsNullOrEmpty(empresaInfo.RucCiPasaporte))
                        {
                            if (empresaInfo.RucCiPasaporte.Length == 13)
                                chkRuc.Checked = true;
                            else if (empresaInfo.RucCiPasaporte.Length == 10)
                                chkCedula.Checked = true;
                            else
                                chkPasaporte.Checked = true;
                        }

                        txtActividadComercial.Text = empresaInfo.ActividadComercial;
                        txtDireccionOficina.Text = empresaInfo.DireccionOficina;
                        txtTelefonoOficina.Text = empresaInfo.TelefonoOficina;
                        txtPersonaContacto.Text = empresaInfo.PersonaContacto;
                        txtCelularContacto.Text = empresaInfo.CelularContacto;
                        txtMailContacto.Text = empresaInfo.EmailContacto;
                        txtMailEbilling.Text = empresaInfo.EmailEBilling;
                        txtCertificaciones.Text = empresaInfo.Certificaciones;
                        txtSitioWeb.Text = empresaInfo.SitioWeb;
                        txtGremios.Text = empresaInfo.AfiGremios;
                        txtReferenciaComercial.Text = empresaInfo.ReferenciaComercial;

                        // Sección 3: Representante Legal
                        txtRepresentanteLegal.Text = empresaInfo.RepresentanteLegal;
                        txtTelefonoDomicilio.Text = empresaInfo.TelefonoDomicilio;
                        txtDireccionDomiciliaria.Text = empresaInfo.DireccionDomiciliaria;
                        txtIdentificacionRep.Text = empresaInfo.CedulaRepresentanteLegal;

                        // Identificación del representante
                        if (!string.IsNullOrEmpty(empresaInfo.CedulaRepresentanteLegal))
                        {
                            if (empresaInfo.CedulaRepresentanteLegal.Length == 10)
                                chkCedulaRep.Checked = true;
                            else
                                chkPasaporteRep.Checked = true;
                        }

                        txtMailRepresentante.Text = empresaInfo.EmailRepresentanteLegal;
                    }
                }
                catch (Exception ex)
                {
                    this.Mostrar_Mensaje(4, $"<i class='fa fa-warning'></i><b> Error! {ex.Message}</b>");
                }
            }
        }

        private Cls_Empresa_Info ObtenerDatosDesdeFormulario()
        {
            return new Cls_Empresa_Info
            {
                TipoCliente = ObtenerTipoClienteDesdeCheckboxes(),
                RazonSocial = txtRazonSocial.Text.Trim(),
                RucCiPasaporte = txtIdentificacion.Text.Trim(),
                ActividadComercial = txtActividadComercial.Text.Trim(),
                DireccionOficina = txtDireccionOficina.Text.Trim(),
                TelefonoOficina = txtTelefonoOficina.Text.Trim(),
                PersonaContacto = txtPersonaContacto.Text.Trim(),
                CelularContacto = txtCelularContacto.Text.Trim(),
                EmailContacto = txtMailContacto.Text.Trim(),
                EmailEBilling = txtMailEbilling.Text.Trim(),
                Certificaciones = txtCertificaciones.Text.Trim(),
                SitioWeb = txtSitioWeb.Text.Trim(),
                AfiGremios = txtGremios.Text.Trim(),
                ReferenciaComercial = txtReferenciaComercial.Text.Trim(),
                RepresentanteLegal = txtRepresentanteLegal.Text.Trim(),
                TelefonoDomicilio = txtTelefonoDomicilio.Text.Trim(),
                DireccionDomiciliaria = txtDireccionDomiciliaria.Text.Trim(),
                CedulaRepresentanteLegal = txtIdentificacionRep.Text.Trim(),
                EmailRepresentanteLegal = txtMailRepresentante.Text.Trim()
            };
        }

        private string ObtenerTipoClienteDesdeCheckboxes()
        {
            List<string> tipos = new List<string>();
            if (chkExportador.Checked) tipos.Add("EXPORTADOR");
            if (chkImportador.Checked) tipos.Add("IMPORTADOR");
            if (chkOperador.Checked) tipos.Add("OPERADOR PORTUARIO");
            if (chkProveedor.Checked) tipos.Add("PROVEEDOR CGSA");
            return string.Join(";", tipos);
        }

        protected void btnEnviarSolicitud_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "showLoader", "mostrarLoaderSwal();", true);
            var original = Session["EmpresaOriginal"] as Cls_Empresa_Info;
            var actual = ObtenerDatosDesdeFormulario();

            if (original != null && actual != null && !CompararEmpresas(original, actual))
            {
                var respuesta = Cls_Empresa_Info.ProcesarCambios(actual);
                if (respuesta)
                {
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "hideLoader", "ocultarLoaderSwal();", true);

                    // Esperamos 500ms antes de mostrar el mensaje de éxito
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "exitoRegistro", @"
                setTimeout(function() {
                    mostrarExito('Registro actualizado correctamente.', function() {
                        location.reload(); // o window.location.href = 'otraPagina.aspx';
                    });
                }, 500);", true);
                    return;
                }
                else
                {
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "hideLoader", "ocultarLoaderSwal();", true);
                    MostrarError("Ocurrió un error al actualizar.");
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "hideLoader", "ocultarLoaderSwal();", true);
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "sinCambios", @"
            setTimeout(function() {
                mostrarError('Para enviar una actualización, debe modificar al menos un campo.');
            }, 300);", true);
            }
        }


        private bool CompararEmpresas(Cls_Empresa_Info a, Cls_Empresa_Info b)
        {
            return a.TipoCliente == b.TipoCliente &&
                   a.RazonSocial == b.RazonSocial &&
                   a.RucCiPasaporte == b.RucCiPasaporte &&
                   a.ActividadComercial == b.ActividadComercial &&
                   a.DireccionOficina == b.DireccionOficina &&
                   a.TelefonoOficina == b.TelefonoOficina &&
                   a.PersonaContacto == b.PersonaContacto &&
                   a.CelularContacto == b.CelularContacto &&
                   a.EmailContacto == b.EmailContacto &&
                   a.EmailEBilling == b.EmailEBilling &&
                   a.Certificaciones == b.Certificaciones &&
                   a.SitioWeb == b.SitioWeb &&
                   a.AfiGremios == b.AfiGremios &&
                   a.ReferenciaComercial == b.ReferenciaComercial &&
                   a.RepresentanteLegal == b.RepresentanteLegal &&
                   a.TelefonoDomicilio == b.TelefonoDomicilio &&
                   a.DireccionDomiciliaria == b.DireccionDomiciliaria &&
                   a.CedulaRepresentanteLegal == b.CedulaRepresentanteLegal &&
                   a.EmailRepresentanteLegal == b.EmailRepresentanteLegal;
        }

        public void limpiarDivs()
        {


        }

        public void MostrarError(string mensaje)
        {
            string script = $"mostrarError('{mensaje.Replace("'", "\\'")}');";
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "mostrarError", script, true);

        }

    }
}


#endregion