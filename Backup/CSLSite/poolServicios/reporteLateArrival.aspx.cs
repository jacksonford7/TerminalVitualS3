using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ConectorN4;
using System.Text;
using System.Configuration;
using System.Data;

namespace CSLSite
{
    public partial class reporteLateArrival : System.Web.UI.Page
    {
        public static usuario sUser = null;
        public static UsuarioSeguridad us;
        public List<contenedorLate> GridContenedoresOperador
        {
            get { return (List<contenedorLate>)Session["GridContenedoresOperador"]; }
            set { Session["GridContenedoresOperador"] = value; }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            ViewStateUserKey = Session.SessionID;
        }
        protected void Page_Init(object sender, EventArgs e)
        {
            if (!Request.IsAuthenticated)
            {
                csl_log.log_csl.save_log<ApplicationException>(new ApplicationException("No autenticado"), "container", "Page_Init", "No autenticado", "No disponible");
                this.PersonalResponse("Para acceder a esta área necesita estar autenticado, será redireccionado a la página de login", "../csl/login", true);
            }
            this.IsAllowAccess();
            var user = Page.Tracker();

            if (user != null && !string.IsNullOrEmpty(user.nombregrupo))
            {
                var t = CslHelper.getShiperName(user.ruc);
                if (string.IsNullOrEmpty(user.ruc))
                {
                    csl_log.log_csl.save_log<ApplicationException>(new ApplicationException("Usuario sin línea"), "turnos", "Page_Init", user.ruc, user.loginname);
                    this.PersonalResponse("Su cuenta de usuario no tiene asociada una línea, arregle esto primero con Customer Services", "../csl/login", true);
                }
                //this.agencia.Value = user.ruc;
            }
            if (!IsPostBack)
            {
                this.IsCompatibleBrowser();
                Page.SslOn();
            }
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                sUser = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());
                Seguridad s = new Seguridad();
                us = s.consultarUsuarioPorId(sUser.id);
                sinresultado.Visible = false;
                alerta.Visible = false;

                divBotonera.Visible = false;

                Session["identificacionAgencia"] = "0";

            }
        }

        public void cargarContenedores()
        {
            //AQUI VA EL CARGADO DE LOS CONTAINERS EN LA PANTALLA 
            List<contenedorLate> table = CslHelperServicios.consultaBuquesContenedorLate(txtReferencia.Text.Trim(), ddlTransito.SelectedValue);
            var u = this.getUserBySesion();
            try
            {
                if (Response.IsClientConnected)
                {

                    DataTable dtLate = new DataTable();
                    dtLate.Columns.AddRange(new DataColumn[] { new DataColumn("Exportador", typeof(string)), new DataColumn("Contenedor", typeof(string)), new DataColumn("Linea", typeof(string)),
                                                                   new DataColumn("Cliente", typeof(string)), new DataColumn("DAE", typeof(string)), new DataColumn("Booking", typeof(string)),
                                                                    new DataColumn("FechaIngreso", typeof(string)), new DataColumn("FechaCutoff", typeof(string)),
                                                                    new DataColumn("FechaCutoffMaximo", typeof(string)), new DataColumn("EstadoLateArrival", typeof(string)),new DataColumn("EstadoContenedor", typeof(string))
                                                                    });



                    if (table.Count > 0)
                    {

                        foreach (contenedorLate item in table)
                        {
                            dtLate.Rows.Add(item.exportador.Trim(), item.contenedor.Trim(), item.linea.Trim(), item.cliente.Trim(), item.dae.Trim(), item.booking.Trim(),
                                            item.ingreso.Trim(), item.cutoff.Trim(), item.cutoffMaximo.Trim(), item.lateArrival.Trim(), item.estado.Trim());

                        }

                        Session["resultadoListaLate"] = dtLate;

                        grvContenedores.PageIndex = 0;
                        this.grvContenedores.DataSource = table;
                        this.grvContenedores.DataBind();
                        GridContenedoresOperador = table;

                        hdfTransito.Value = "T";
                        xfinder.Visible = true;
                        sinresultado.Visible = false;
                        divBotonera.Visible = true;

                        return;
                    }
                    else
                    {

                        divBotonera.Visible = false;
                        this.sinresultado.Attributes["class"] = string.Empty;
                        this.sinresultado.Attributes["class"] = "msg-info";
                        this.sinresultado.InnerText = "No hay contenedores asociados a la referencia seleccionada.";
                    }
                    xfinder.Visible = false;
                    sinresultado.Visible = true;
                }
            }
            catch (Exception ex)
            {
                xfinder.Visible = false;
                this.sinresultado.Attributes["class"] = string.Empty;
                this.sinresultado.Attributes["class"] = "msg-critico";
                this.sinresultado.InnerText = string.Format("Se produjo un error durante la búsqueda, por favor repórtelo con el siguiente codigo [E00-{0}] ", csl_log.log_csl.save_log<Exception>(ex, "referencias", "find_Click", txtReferencia.Text, u != null ? u.loginname : "userNull"));
                sinresultado.Visible = true;
            }
        }

        protected void btgenerar_Click(object sender, EventArgs e)
        {

        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtReferencia.Text.Trim()))
            {
                cargarContenedores();
            }
            else
            {
                this.sinresultado.Attributes["class"] = string.Empty;
                this.sinresultado.Attributes["class"] = "msg-info";
                this.sinresultado.InnerText = "Por favor llene la referencia para poder realizar la consulta.";
            }
        }

        protected void grvContenedores_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grvContenedores.PageIndex = e.NewPageIndex;
            this.grvContenedores.DataSource = GridContenedoresOperador;
            this.grvContenedores.DataBind();
        }

        public void actualizarDataTableSession()
        {


        }

        protected void btnBuscarContenedor_Click(object sender, EventArgs e)
        {
            this.grvContenedores.DataSource = GridContenedoresOperador.ToList();
            this.grvContenedores.DataBind();
        }

        protected void grvContenedores_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }

        protected void btnBuscarHF_Click(object sender, EventArgs e)
        {
            //grvContenedores.PageIndex = grvContenedores.PageIndex;
            actualizarDataTableSession();

            this.grvContenedores.DataSource = GridContenedoresOperador.ToList();
            this.grvContenedores.DataBind();
        }

        public string desbloquearN4(string contenedor)
        {
            wsN4 g = new wsN4();
            String a = string.Format("<hpu><entities><units><unit id=\"{0}\"/></units></entities><flags><flag hold-perm-id=\"{1}\" action=\"{2}\"/></flags></hpu>", contenedor.ToString(), "CGSA_EXPO_PAGO", "RELEASE_HOLD");
            string me = string.Empty;

            //Invocación del web service para desbloquear contenedor en el sistema N4.

            var i = g.CallBasicService("ICT/ECU/GYE/CGSA", a, ref me);

            if (i > 0)
            {
                return me.Replace("\n", "").Replace("\r", "");
            }
            else
            {
                return "ok";
            }


        }

        public string invocacionEvento(string descripcionContenedor)
        {
            wsN4 g = new wsN4();

            string me = string.Empty;
            string errorN4 = string.Empty;

            StringBuilder newa = new StringBuilder();
            newa.Append(string.Format("<icu><units><unit-identity id=\"{0}\" type=\"CONTAINERIZED\"/></units><properties><property tag=\"{1}\" value=\"Y\"/></properties></icu>", descripcionContenedor, ConfigurationManager.AppSettings["CostoN4"].ToString()));
            var i = g.CallBasicService("ICT/ECU/GYE/CGSA", newa.ToString(), ref me);

            /*I ES LA RESPUESTA QUE DEVUELVE EL N4 Y me ES LA DESCRIPCION DEL MENSAJE DE ERROR*/
            if (i > 0)
            {
                errorN4 = me;
            }

            return errorN4;
        }

        protected void btnBuscarContenedores_Click(object sender, EventArgs e)
        {

            if (!string.IsNullOrEmpty(txtReferencia.Text.Trim()))
            {
                sinresultado.Visible = false;
                cargarContenedores();
            }
            else
            {
                this.sinresultado.Attributes["class"] = string.Empty;
                this.sinresultado.Attributes["class"] = "msg-info";
                this.sinresultado.InnerText = "Por favor llene la referencia para poder realizar la consulta.";
                sinresultado.Visible = true;
            }

        }

    }
}