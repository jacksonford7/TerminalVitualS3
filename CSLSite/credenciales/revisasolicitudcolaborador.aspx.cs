using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using System.Data;
using System.IO;
using System.Configuration;
using System.Globalization;
using csl_log;

namespace CSLSite
{

    public partial class revisasolicitudcolaborador : System.Web.UI.Page
    {
        private static Int64? lm = -3;
        private string OError;
        private GetFile.Service getFile = new GetFile.Service();
        private OnlyControl.OnlyControlService onlyControl = new OnlyControl.OnlyControlService();
        private String xmlDocumentos;
        public DataTable dtDocumentos
        {
            get { return (DataTable)Session["dtDocumentosrevisasolicitudcolaboradorPermanente"]; }
            set { Session["dtDocumentosrevisasolicitudcolaboradorPermanente"] = value; }
        }
        public String numsolicitudempresa
        {
            get { return (String)Session["numsolicitudrevisasolicitudcolaborador"]; }
            set { Session["numsolicitudrevisasolicitudcolaborador"] = value; }
        }
        public string rucempresa
        {
            get { return (string)Session["rucempresarevisasolicitudcolaborador"]; }
            set { Session["rucempresarevisasolicitudcolaborador"] = value; }
        }
        public string useremail
        {
            get { return (string)Session["useremailrevisasolicitudcolaborador"]; }
            set { Session["useremailrevisasolicitudcolaborador"] = value; }
        }
        public string mensajefac
        {
            get { return (string)Session["mensajefacrevisasolicitudcolaborador"]; }
            set { Session["mensajefacrevisasolicitudcolaborador"] = value; }
        }
        public string mensajeok
        {
            get { return (string)Session["mensajeokrevisasolicitudcolaborador"]; }
            set { Session["mensajeokrevisasolicitudcolaborador"] = value; }
        }
        protected void Page_Init(object sender, EventArgs e)
        {
            if (!Request.IsAuthenticated)
            {
                csl_log.log_csl.save_log<ApplicationException>(new ApplicationException("No autenticado"), "container", "Page_Init", "No autenticado", "No disponible");
                this.PersonalResponse("Para acceder a esta área necesita estar autenticado, será redireccionado a la página de login", "../login.aspx", true);
            }
           // this.IsAllowAccess();
            var user = Page.Tracker();

            if (user != null && !string.IsNullOrEmpty(user.nombregrupo))
            {
                var t = CslHelper.getShiperName(user.ruc);
                if (string.IsNullOrEmpty(user.ruc))
                {
                    csl_log.log_csl.save_log<ApplicationException>(new ApplicationException("Usuario sin línea"), "turnos", "Page_Init", user.ruc, user.loginname);
                    this.PersonalResponse("Su cuenta de usuario no tiene asociada una línea, arregle esto primero con Customer Services", "../login.aspx", true);
                }
                //this.agencia.Value = user.ruc;
                foreach (string var in Request.QueryString)
                {
                    rucempresa = Request.QueryString["ruc"];
                }
                //rucempresa = rucempresa;
                useremail = user.email;
                
            }
            if (!IsPostBack)
            {
                this.IsCompatibleBrowser();
                Page.SslOn();
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            //Está conectado y no es postback
            if (Response.IsClientConnected && !IsPostBack)
            {
                Session["dtDocumentosrevisasolicitudcolaboradorPermanente"] = new DataTable();
                LlenaCombos();
                ConsultaInfoSolicitud();
            }
            factura.Visible = false;
            txttipcli.Text = Session["txttipcli.Text"] as string;
        }
        private void ConsultaInfoSolicitud()
        {
            try
            {
                foreach (string var in Request.QueryString)
                {
                    numsolicitudempresa = Request.QueryString["numsolicitud"];
                }
                if (!credenciales.GetTipoSolicitud(numsolicitudempresa))
                {
                    if (!credenciales.GetSolicitudEstado(numsolicitudempresa))
                    {
                        botonera.Visible = false;
                        salir.Visible = true;
                        //factura.Visible = false;
                    }
                    else
                    {
                        //factura.Visible = false;
                        btsalvar.Text = "Finalizar";
                        btsalvar.ToolTip = "Finaliza la solicitud.";
                        mensajefac = "finalización";
                        mensajeok = "finalizada";
                    }
                }
                else
                {
                    if (!credenciales.GetSolicitudEstado(numsolicitudempresa))
                    {
                        botonera.Visible = false;
                        salir.Visible = true;
                        //factura.Visible = false;
                    }
                    else
                    {
                        alertafu.InnerHtml = "Adjunte el archivo en formato PDF";
                        //factura.Visible = true;
                    }
                    mensajefac = "facturación";
                    mensajeok = "facturada";
                }

                var DescripcionTipoSolicitud = credenciales.GetDescripcionTipoSolicitudXNumSolicitud(numsolicitudempresa);
                for (int i = 0; i <= DescripcionTipoSolicitud.Rows.Count - 1; i++)
                {
                    Session["txttipcli.Text"] = DescripcionTipoSolicitud.Rows[i][0].ToString(); ;
                    txttipcli.Text = DescripcionTipoSolicitud.Rows[i][0].ToString();
                }
                //this.alerta.InnerHtml = "Confirme que los documentos del colaborador(es) sean los correctos.";
                Mostrar_Mensaje(1, "Confirme que los documentos del colaborador(es) sean los correctos.");
                Session["dtDocumentosrevisasolicitudcolaboradorPermanente"] = new DataTable();
                var tablixVehiculo = credenciales.GetSolicitudColaborador(numsolicitudempresa);
                tablixVehiculo.Columns.Add("TIPOD");
                tablixVehiculo.Columns.Add("PERMISO");
                for (int i = 0; i < tablixVehiculo.Rows.Count; i++)
                {
                    var dtvalhuellafoto = aso_transportistas.GetValHuellaFoto(numsolicitudempresa, tablixVehiculo.Rows[i]["CIPAS"].ToString());
                    if (dtvalhuellafoto.Rows[0]["HUELLA"].ToString() == "1" && dtvalhuellafoto.Rows[0]["FOTO"].ToString() == "1" && dtvalhuellafoto.Rows[0]["BLOQUEO"].ToString() == "0")
                    {
                        var dtpermiso = aso_transportistas.GetFechasPermisoDeAcceso(tablixVehiculo.Rows[i]["RUCCIPAS"].ToString(), tablixVehiculo.Rows[i]["CIPAS"].ToString());
                        if (dtpermiso.Rows.Count > 0)
                        {
                            if (dtpermiso.Rows[0]["ESTADO"].ToString() == "ACTIVA")
                            {
                                var permiso = "Desde: " + Convert.ToDateTime(dtpermiso.Rows[0]["FECHAINGRESO"]).ToString("dd-MM-yyyy") + " - " +
                                              "Hasta: " + Convert.ToDateTime(dtpermiso.Rows[0]["FECHASALIDA"]).ToString("dd-MM-yyyy");
                                tablixVehiculo.Rows[i]["PERMISO"] = permiso;
                                tablixVehiculo.Rows[i]["TIPOD"] = tablixVehiculo.Rows[i]["TIPO"].ToString() + " - PERMISO TEMPORAL CREADO";
                            }
                            else
                            {
                                tablixVehiculo.Rows[i]["PERMISO"] = "";
                                tablixVehiculo.Rows[i]["TIPOD"] = tablixVehiculo.Rows[i]["TIPO"].ToString();
                            }
                        }
                    }
                    else
                    {
                        if (dtvalhuellafoto.Rows[0]["BLOQUEO"].ToString() == "1")
                        {
                            tablixVehiculo.Rows[i]["PERMISO"] = "BLOQUEADO";
                            tablixVehiculo.Rows[i]["TIPOD"] = tablixVehiculo.Rows[i]["TIPO"].ToString();
                        }
                        else
                        {
                            tablixVehiculo.Rows[i]["PERMISO"] = "";
                            tablixVehiculo.Rows[i]["TIPOD"] = tablixVehiculo.Rows[i]["TIPO"].ToString();
                        }
                    }
                }
                tablePagination.DataSource = tablixVehiculo;
                tablePagination.DataBind();
                foreach (RepeaterItem item in tablePagination.Items)
                {
                    CheckBox chkRevisado = item.FindControl("chkRevisado") as CheckBox;
                    TextBox tcomentario = item.FindControl("tcomentario") as TextBox;
                    if (chkRevisado.Checked)
                    {
                        //chkRevisado.Enabled = false;
                        tcomentario.Enabled = false;
                    }
                    CheckBox chkHuellaEstado = item.FindControl("chkHuellaEstado") as CheckBox;
                    CheckBox chkFotoEstado = item.FindControl("chkFotoEstado") as CheckBox;
                    var dtvalestadohuella = aso_transportistas.GetValidaHuella(tablixVehiculo.Rows[item.ItemIndex]["RUCCIPAS"].ToString(), tablixVehiculo.Rows[item.ItemIndex]["CIPAS"].ToString());
                    var dtvalestadofoto = aso_transportistas.GetValidaFoto(tablixVehiculo.Rows[item.ItemIndex]["RUCCIPAS"].ToString(), tablixVehiculo.Rows[item.ItemIndex]["CIPAS"].ToString());
                    if (dtvalestadohuella.Rows.Count > 0)
                    {
                        if (dtvalestadohuella.Rows[0][0].ToString() == "1")
                        {
                            chkHuellaEstado.Checked = true;
                            chkHuellaEstado.Text = "HUELLA [OK]";
                        }
                        else
                        {
                            chkHuellaEstado.Checked = false;
                            chkHuellaEstado.Text = "HUELLA [NO]";
                        }
                    }
                    if (dtvalestadofoto.Rows.Count > 0)
                    {
                        if (dtvalestadofoto.Rows[0][0].ToString() == "1")
                        {
                            chkFotoEstado.Checked = true;
                            chkFotoEstado.Text = "FOTO [OK]";
                        }
                        else
                        {
                            chkFotoEstado.Checked = false;
                            chkFotoEstado.Text = "FOTO [NO]";
                        }
                    }
                }

                HiddenField1.Value = ObtenerTarifa();
                //UPPRINCIPAL.Update();
                
            }
            catch (Exception ex)
            {
                Session["dtDocumentosrevisasolicitudcolaboradorPermanente"] = new DataTable();
                Response.Write("<script language='JavaScript'>alert('" + ex.Message + "');</script>");
            }
        }


        public string ObtenerTarifa()
        {
            string v_tarifa = "";
            try
            {
                var oServicio = serviciosCredenciales.GetServicio(long.Parse(numsolicitudempresa));
                rucempresa = oServicio.RUCCIPAS;
                var tipoEmpresa = credenciales.getTipoEmpresa(rucempresa);
                var tarifa = TarifarioN4.GetTarifa(int.Parse(oServicio.id.ToString()), int.Parse(tipoEmpresa.FirstOrDefault().Item1.ToString()), long.Parse(numsolicitudempresa), cmbNorma.SelectedValue, cmbTipo.SelectedValue);
                if (tarifa == null)
                {
                    this.Alerta("No se encontro tarifario N4 configurado.\\n" + "Revise la configuración del tarifario antes de continuar con la solicitud de " + oServicio.notasN4 + " para " + tipoEmpresa.FirstOrDefault().Item2 + ".");
                    //Response.Write("<script language='JavaScript'>alert('" + "Funk1o"+ "');</script>");
                    return "No se encontro tarifario N4 configurado. \n" + "Revise la configuración del tarifario antes de continuar con la solicitud de " + oServicio.notasN4 + " para " + tipoEmpresa.FirstOrDefault().Item2 + ".";
                }
                v_tarifa = tarifa.ID.ToString() + " - " + tarifa.TARIFA_N4;
            }
            catch
            {
                return "No se encontro tarifario N4 configurado.";
            }

            return "¿Esta seguro de procesar la solicitud? \n La tarifa que se aplicará es: " + v_tarifa;
        }

    
        protected void btsalvar_Click(object sender, EventArgs e)
        {
            //UPdetalle.Update();
            try
            {
                Boolean banderafac = false;
                //if (factura.Visible)
                //{
                    //if (!fuAdjuntarFactura.HasFile)
                    //{
                    //    this.Alerta("Adjunte la factura por favor.");
                    //    return;
                    //}

                    //if (string.IsNullOrEmpty(this.TxtNumdocumento.Text))
                    //{
                    //    this.Alerta("Ingreseo el número de la factura por favor.");
                    //    return;
                    //}

                    banderafac = true;
                //}
                foreach (string var in Request.QueryString)
                {
                    numsolicitudempresa = Request.QueryString["numsolicitud"];
                }

                string Numero_Factura = this.TxtNumdocumento.Text.Trim();

                List<string> listCedulas = new List<string>();
                DataTable dtDocSol = Session["dtDocumentosrevisasolicitudcolaboradorPermanente"] as DataTable;

                foreach (DataRow item in dtDocSol.Rows)
                {
                    if (bool.Parse( item["DocumentoRechazado"].ToString()) == true)
                    {
                        foreach (RepeaterItem fila in tablePagination.Items)
                        {
                            Label lblcipasColab = fila.FindControl("lblcipas") as Label;
                            CheckBox chkRevisadoColab = fila.FindControl("chkRevisado") as CheckBox;

                            if (item["Cedula"].ToString() == lblcipasColab.Text)
                            {
                                chkRevisadoColab.Checked = true;
                                UPdetalle.Update();
                            }
                        }
                    }

                }


                foreach (RepeaterItem item in tablePagination.Items)
                {
                    CheckBox chkRevisado = item.FindControl("chkRevisado") as CheckBox;
                    Label lblcipas = item.FindControl("lblcipas") as Label;
                    TextBox tcomentario = item.FindControl("tcomentario") as TextBox;
                    if (!chkRevisado.Checked)
                    {
                        var nomina = credenciales.GetConsultaNomina(rucempresa, lblcipas.Text);
                        if (nomina.Rows[0]["MENSAJE"].ToString() == "NUEVO" || nomina.Rows[0]["MENSAJE"].ToString() == "SEMPRESA" || nomina.Rows[0]["MENSAJE"].ToString() == "NEMPRESA")
                        {
                            if (nomina.Rows[0]["MENSAJE"].ToString() == "NEMPRESA")
                            {
                                //Emisión
                                foreach (string var in Request.QueryString)
                                {
                                    this.Alerta("La Solicitud pertenece a la Empresa:\\n *" + Request.QueryString["razonsocial"].ToString() + "\\nSin embargo el Colaborador:\\n *" + lblcipas.Text + " - " + nomina.Rows[0]["NOMBRES"].ToString() + "\\nEsta registrado en la Empresa:\\n *" + nomina.Rows[0]["EMPE_NOM"].ToString());
                                    return;
                                }
                            }
                        }
                    }
                    //if (chkRevisado.Checked == false && !string.IsNullOrEmpty(tcomentario.Text))
                    //{
                    //    this.Alerta("Marque la casilla del Comentario de rechazo. \\n Cedula: " + lblcipas.Text);
                    //    return;
                    //}
                    //if (chkRevisado.Checked == true && string.IsNullOrEmpty(tcomentario.Text))
                    //{
                    //    this.Alerta("Escriba el Comentario de la Cedula: *" + lblcipas.Text);
                    //    return;
                    //}
                    else if (chkRevisado.Checked == true)
                    {
                        listCedulas.Add(lblcipas.Text);
                    }
                    if (chkRevisado.Checked == false && (dtDocSol != null || dtDocSol.Rows.Count > 0))
                    {
                        var result = from myRow in dtDocSol.AsEnumerable()
                                     where myRow.Field<string>("Cedula") == lblcipas.Text
                                     select myRow;
                        DataTable dt = result.AsDataView().ToTable();
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            if (Convert.ToBoolean(dt.Rows[item.ItemIndex][10]) == true)
                            {
                                this.Alerta("La Cedula: *" + lblcipas.Text + "*\\nTiene documentos rechazados.\\n" + "Revise la información antes de continuar con la " + mensajefac + " de la solicitud.\\n Si quiere continuar de click en Rechazar.");
                                return;
                            }
                        }
                    }
                }

                DataTable dtColaboradores = credenciales.GetSolicitudColaborador(numsolicitudempresa);
                var resultAprobados = from myRow in dtColaboradores.AsEnumerable()
                                      where !listCedulas.Contains(myRow.Field<string>("CIPAS"))
                                      select myRow;
                DataTable dtAprobados = resultAprobados.AsDataView().ToTable();
                if (dtAprobados.Rows.Count == 0)
                {
                    this.Alerta("Tiene un unico colaborador rechazado.\\n" + "Revise la información antes de continuar con la " + mensajefac + " de la solicitud.\\n Si quiere continuar de click en Rechazar.");
                    return;
                }

                //****************************************************
                //ACTUALIZA ESTADO DE COLABORADOR RECHAZADO
                //****************************************************
                string v_mensaje = string.Empty;
                foreach (RepeaterItem item in tablePagination.Items)
                {
                    CheckBox chkRevisado = item.FindControl("chkRevisado") as CheckBox;
                    Label txtNumeroSolicitudColab = item.FindControl("txtNumeroSolicitudColab") as Label;
                    Label txtNumeroSolicitudColColab = item.FindControl("txtNumeroSolicitudColColab") as Label;
                    TextBox tcomentario = item.FindControl("tcomentario") as TextBox;
                    if (chkRevisado.Checked)
                    {
                        credenciales.ActualizarEstadoColaborador(long.Parse(txtNumeroSolicitudColab.Text), long.Parse(txtNumeroSolicitudColColab.Text), "R", tcomentario.Text, Page.User.Identity.Name.ToUpper(), out v_mensaje);
                    }
                }

                var coloboradores = credenciales.GetSolicitudColaborador(numsolicitudempresa);
                coloboradores.Columns.Add("Comentario");
                foreach (RepeaterItem item in tablePagination.Items)
                {
                    CheckBox chkRevisado = item.FindControl("chkRevisado") as CheckBox;
                    Label lblcipas = item.FindControl("lblcipas") as Label;
                    TextBox tcomentario = item.FindControl("tcomentario") as TextBox;
                    if (chkRevisado.Checked == true)
                    {
                        coloboradores.Rows[item.ItemIndex]["Comentario"] = tcomentario.Text;
                    }
                }
                DataTable dtCol = new DataTable();
                var resultListaAprobados = from myRow in coloboradores.AsEnumerable()
                            where listCedulas.Contains(myRow.Field<string>("CIPAS"))
                            select myRow;
                dtCol = resultListaAprobados.AsDataView().ToTable();
                dtCol.AcceptChanges();
                dtCol.TableName = "Colaboradores";
                StringWriter sw = new StringWriter();
                dtCol.WriteXml(sw);
                String xmlColaboradores = sw.ToString();
                if (banderafac)
                {
                    New_ExportFileUpload();//23-11-2020

                    //ExportFileUpload();
                }
                else
                {
                    //Boolean validasolicitud = false;
                    //RegistraColaboradoresOnlyControl(out validasolicitud);
                    //if (validasolicitud)
                    //{
                    //    return;
                    //}
                }
                sw = new StringWriter();
                dtDocSol.TableName = "Documentos";
                dtDocSol.WriteXml(sw);
                String xmlDocumentosRechazados = sw.ToString();
                string mensaje = null;
                //string nombreempresa = CslHelper.getShiperName(rucempresa);

                //########################################################
                // PROCESO PARA CARGAR SERVICIO A FACTURAR EN BILLING N4
                //########################################################
                decimal v_monto = 0;
                string v_Error = "";
                var oServicio = serviciosCredenciales.GetServicio(long.Parse(numsolicitudempresa));
                rucempresa = oServicio.RUCCIPAS;
                var tipoEmpresa = credenciales.getTipoEmpresa(rucempresa);
                var tarifa = TarifarioN4.GetTarifa(int.Parse(oServicio.id.ToString()), int.Parse(tipoEmpresa.FirstOrDefault().Item1.ToString()), long.Parse(numsolicitudempresa), cmbNorma.SelectedValue, cmbTipo.SelectedValue);
                
                if (tarifa == null)
                {
                    this.Alerta("No se encontro tarifario N4 configurado.\\n" + "Revise la configuración del tarifario antes de continuar con la solicitud de " + oServicio.notasN4+ " para "+ tipoEmpresa.FirstOrDefault().Item2 + ".\\n Si quiere continuar de click en Rechazar.");
                    return;
                }

                string request = string.Empty;
                string response = string.Empty;
                Respuesta.ResultadoOperacion<bool> resp;
                if (!credenciales.ValidaFacturacion(numsolicitudempresa))
                {
                    resp = ServicioSCA.CargarServicioCredencial(oServicio.codigoN4, rucempresa, tarifa.TARIFA_N4, dtAprobados.Rows.Count.ToString(), Page.User.Identity.Name.ToUpper(), out request, out response);
                    var oTrace = TraceSet.SaveTrace(long.Parse(numsolicitudempresa), rucempresa, cmbNorma.SelectedValue, cmbTipo.SelectedValue, int.Parse(tarifa.ID.ToString()), tarifa.TARIFA_N4, request, response, resp.Exitoso, Page.User.Identity.Name, out v_Error);
                }
                else
                {
                    resp = Respuesta.ResultadoOperacion<bool>.CrearFalla("La factura ya a sido creada, se está intentando duplicar la facturación de la solicitud "+ numsolicitudempresa);
                }

                if (!string.IsNullOrEmpty(v_Error))
                {
                    this.Alerta(v_Error);
                }

                if (resp.Exitoso)
                {
                    var v_result = resp.MensajeInformacion.Split(',');
                    Numero_Factura = "902500"+v_result[0];
                    v_monto = decimal.Parse(v_result[1].ToString());

                    if (!credenciales.ApruebaSolicitudColaborador_New(
                        numsolicitudempresa,
                        rucempresa,
                        Numero_Factura,
                        v_monto,
                        //nombreempresa,
                        //useremail,
                        xmlColaboradores,
                        xmlDocumentos,
                        xmlDocumentosRechazados,
                        Page.User.Identity.Name.ToUpper(),
                        banderafac,
                        out mensaje))
                    {
                        //this.Alerta(mensaje);

                        lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(btsalvar_Click), "btsalvar_Click", false, null, null, mensaje, null);
                        OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                        Response.Write("<script language='JavaScript'>alert('" + mensaje + " - " + OError + "');</script>");
                    }
                    else
                    {
                        //this.Alerta("Solicitud aprobada exitosamente, en unos minutos recibirá una notificación via mail.");
                        //botonera.Visible = false;
                        //factura.Visible = false;
                        //salir.Visible = true;
                        //if (!string.IsNullOrEmpty(error))
                        //{
                        //    Response.Write("<script language='JavaScript'>var r=alert('Solicitud " + mensajeok + " exitosamente, en unos minutos recibirá una notificación via mail. \\n" + " *Sin embargo " + error + "');if(r==true){window.close()}else{window.close()};</script>");
                        //}
                        //else
                        //{
                        //ConsultaInfoSolicitud();
                        Response.Write("<script language='JavaScript'>var r=alert('Solicitud " + mensajeok + " exitosamente.');if(r==true){window.close()}else{window.close()};</script>");
                        //}
                    }
                }
                else
                {
                    //this.Alerta("No se logró realizar la facturación en N4.\\n" + "Respuesta de N4: " + resp.MensajeProblema + ".\\n favor verificar.");
                    Response.Write("<script language='JavaScript'>var r=alert('No se logró realizar la facturación en N4.\\n" + "Respuesta de N4: " + resp.MensajeProblema + ".\\n favor verificar.');if(r==true){window.close()}else{window.close()};</script>");
                    return;
                }
            }
            catch (Exception ex)
            {
                //Response.Write("<script language='JavaScript'>alert('" + ex.Message + "');</script>");

                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(btsalvar_Click), "btsalvar_Click", false, null, null, ex.StackTrace, ex);
                OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                Response.Write("<script language='JavaScript'>alert('" + OError + "');</script>");
            }
        }
        protected void btnRechazar_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (string var in Request.QueryString)
                {
                    numsolicitudempresa = Request.QueryString["numsolicitud"];
                }
                DataTable dtDocSol = Session["dtDocumentosrevisasolicitudcolaboradorPermanente"] as DataTable;
                if (dtDocSol == null)
                {
                    this.Alerta("NO tiene documentos rechazados.\\n" + "Revise la información antes de continuar con el rechazo de la solicitud.");
                    return;
                }
                if (dtDocSol.Rows.Count == 0)
                {
                    this.Alerta("NO tiene documentos rechazados.\\n" + "Revise la información antes de continuar con el rechazo de la solicitud.");
                    return;
                }
                foreach (RepeaterItem item in tablePagination.Items)
                {
                    CheckBox chkRevisado = item.FindControl("chkRevisado") as CheckBox;
                    Label lblcipas = item.FindControl("lblcipas") as Label;
                    TextBox tcomentario = item.FindControl("tcomentario") as TextBox;
                    var result = from myRow in dtDocSol.AsEnumerable()
                                 where Convert.ToBoolean(myRow.Field<string>("DocumentoRechazado")) == true && myRow.Field<string>("Cedula") == lblcipas.Text
                                 select myRow;
                    DataTable dt = result.AsDataView().ToTable();
                    if (dt.Rows.Count == 0)
                    {
                        this.Alerta("La Cedula: *" + lblcipas.Text + "*\\nNO Tiene documentos rechazados.\\n" + "Revise la información antes de continuar con el rechazo de la solicitud.");
                        return;
                    }
                }
                var resultVehiculo = from myRow in dtDocSol.AsEnumerable()
                                     where Convert.ToBoolean(myRow.Field<string>("DocumentoRechazado")) == true
                                     select myRow;
                DataTable dtColaboradores = resultVehiculo.AsDataView().ToTable();
                dtColaboradores.AcceptChanges();
                dtColaboradores.TableName = "Colaboradores";
                StringWriter sw = new StringWriter();
                dtColaboradores.WriteXml(sw);
                String xmlColaboradores = sw.ToString();
                string nombreempresa = CslHelper.getShiperName(rucempresa);
                string mensaje = null;
                if (!credenciales.RechazaSolicitudColaborador(
                    numsolicitudempresa,
                    rucempresa,
                    nombreempresa,
                    useremail,
                    xmlColaboradores,
                    Page.User.Identity.Name.ToUpper(),
                    out mensaje))
                {
                    this.Alerta(mensaje);
                }
                else
                {
                    //this.Alerta("Solicitud rechazada exitosamente, en unos minutos recibirá una notificación via mail.");
                    //botonera.Visible = false;
                    //factura.Visible = false;
                    //salir.Visible = true;
                    //Response.Write("<script language='JavaScript'>window.close();</script>");
                    Response.Write("<script language='JavaScript'>var r=alert('Solicitud rechazada exitosamente.');if(r==true){window.close()}else{window.close()};</script>");
                }
            }
            catch (Exception ex)
            {
                Response.Write("<script language='JavaScript'>alert('" + ex.Message + "');</script>");
            }
        }
        protected void btnSalir_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Write("<script language='JavaScript'>window.close();</script>");
            }
            catch (Exception ex)
            {
                Response.Write("<script language='JavaScript'>alert('" + ex.Message + "');</script>");
            }
        }

        private void New_ExportFileUpload()
        {

            xmlDocumentos = null;
            dtDocumentos = new DataTable();
            dtDocumentos.Columns.Add("IdSolicitud");
            dtDocumentos.Columns.Add("Ruta");

            if (fuAdjuntarFactura.HasFile)
            {
                string rutafile = Server.MapPath(fuAdjuntarFactura.FileName);
                string finalname;
                var p = CSLSite.app_start.CredencialesHelper.UploadFile(Server.MapPath(fuAdjuntarFactura.FileName), fuAdjuntarFactura.PostedFile.InputStream, out finalname);
                if (!p)
                {
                    this.Alerta(finalname);
                    return;
                }
                dtDocumentos.Rows.Add(numsolicitudempresa, finalname);

            }

            dtDocumentos.AcceptChanges();
            dtDocumentos.TableName = "Documentos";
            StringWriter sw = new StringWriter();
            dtDocumentos.WriteXml(sw);
            xmlDocumentos = sw.ToString();
        }

        private void ExportFileUpload()
        {
            xmlDocumentos = null;
            dtDocumentos = new DataTable();
            dtDocumentos.Columns.Add("IdSolicitud");
            dtDocumentos.Columns.Add("Ruta");
            if (fuAdjuntarFactura.HasFile)
            {
                string rutafile = Server.MapPath(fuAdjuntarFactura.FileName);
                string extensionfile = Path.GetExtension(fuAdjuntarFactura.PostedFile.FileName);
                string directorio = Path.GetDirectoryName(rutafile);
                string nombrearchivo = Path.GetFileNameWithoutExtension(fuAdjuntarFactura.FileName);
                if (File.Exists(rutafile))
                {
                    File.Delete(rutafile);
                }
                fuAdjuntarFactura.SaveAs(rutafile);
                string[] files = Directory.GetFiles(directorio, "*" + nombrearchivo + extensionfile);
                foreach (string s in files)
                {
                    FileInfo fi = null;
                    try
                    {
                        fi = new FileInfo(s);
                        ExportFiles(fi.Directory.ToString(), fi.Name, numsolicitudempresa, nombrearchivo, extensionfile);
                        if (File.Exists(rutafile))
                        {
                            File.Delete(rutafile);
                        }
                    }
                    catch (FileNotFoundException ex)
                    {
                        this.Alerta(ex.Message);
                        return;
                    }
                }
                if (File.Exists(rutafile))
                {
                    File.Delete(rutafile);
                }
                //fsupload.SaveAs(savePath += fsupload.FileName);
                //savePath = @"C:\TemporalFileSystemCsaCGSA\";
                //this.Alerta("Your file was saved as " + fileName);
            }
            else
            {
                //this.Alerta("Seleccione los documentos requeridos.");
                //fsupload.Focus();
                //return;
                //return;
            }
            dtDocumentos.AcceptChanges();
            dtDocumentos.TableName = "Documentos";
            StringWriter sw = new StringWriter();
            dtDocumentos.WriteXml(sw);
            xmlDocumentos = sw.ToString();
            //System.IO.File.Delete(savePath);
        }
        private void ExportFiles(string path, string filename, string sidsolicitud, string nombrearchivo, string extension)
        {
            path = path + "\\";
            if (File.Exists(path + filename))
            {
                FileStream fileStream;
                String dateServer = credenciales.GetDateServer();
                String rutaServer = ConfigurationManager.AppSettings["rutaserver"].ToString() + dateServer;
                nombrearchivo = nombrearchivo + "_" + DateTime.Now.ToString("ddMMyyyyhhmmssff") + extension;
                dtDocumentos.Rows.Add(sidsolicitud, dateServer + nombrearchivo);
               getFile.UploadFile(credenciales.ReadBinaryFile(path, filename, out fileStream), rutaServer, nombrearchivo);
                fileStream.Close();
    
            }
        }
        private void RegistraColaboradoresOnlyControl(out bool validacolaboradores)
        {
            validacolaboradores = false;
            String errorvehiculo = string.Empty;
            DataTable dtAprobados = new DataTable();
            List<string> listCedulas = new List<string>();
            Int32 registros_actualizados_correcto = 0;
            Int32 registros_actualizados_incorrecto = 0;
            String error_consulta = string.Empty;
            String error = string.Empty;
            DataSet dsColaboradores = new DataSet();
            DataSet dsErrorAC_R_PERSONA_PEATON = new DataSet();
            DataTable dtColaboradoresAprobados = new DataTable();
            dtColaboradoresAprobados.Columns.Add("CEDULA");
            dtColaboradoresAprobados.Columns.Add("APELLIDOS");
            dtColaboradoresAprobados.Columns.Add("NOMBRES");
            dtColaboradoresAprobados.Columns.Add("EMPRESA");
            dtColaboradoresAprobados.Columns.Add("CARGO");
            var nomeemp = CslHelper.getShiperName(rucempresa);
            dtAprobados = credenciales.GetSolicitudColaboradorOnlyControl(numsolicitudempresa);
            for (int i = 0; i < dtAprobados.Rows.Count; i++)
            {
                dtColaboradoresAprobados.Rows.Add(dtAprobados.Rows[i][2], dtAprobados.Rows[i][3], dtAprobados.Rows[i][3], nomeemp, dtAprobados.Rows[i][11]);
            }
            dsColaboradores = new DataSet();
            dsColaboradores.Tables.Add(dtColaboradoresAprobados);
            dsErrorAC_R_PERSONA_PEATON = onlyControl.AC_R_PERSONA_PEATON(dsColaboradores, ref registros_actualizados_correcto, ref registros_actualizados_incorrecto, ref error_consulta) as DataSet;
            if (registros_actualizados_incorrecto > 0)
            {
                this.Alerta(error_consulta);
                validacolaboradores = true;
                return;
            }
            else if (!string.IsNullOrEmpty(error_consulta))
            {
                this.Alerta(error_consulta);
                validacolaboradores = true;
                return;
            }
            DataTable dt2 = new DataTable();
            dt2 = dsErrorAC_R_PERSONA_PEATON.Tables[0];
            if (dt2.Rows.Count > 0)
            {
                for (int i = 0; i < dt2.Rows.Count; i++)
                {
                    if (dt2.Rows[i][5].ToString().Substring(0, 27).ToUpper() != "Error: Cedula ya registrada".ToUpper())
                    {
                        errorvehiculo = dt2.Rows[i][5].ToString();
                        var t = credenciales.SaveLog(errorvehiculo, "credenciales", "onlyControl.AC_R_PERSONA_PEATON()", DateTime.Now.ToShortDateString(), "sistema");
                        validacolaboradores = true;
                        return;
                    }
                }
            }
            if (!string.IsNullOrEmpty(errorvehiculo))
            {
                this.Alerta("Hubo un error al intentar registrar los siguientes vehiculos en el metodo AC_R_PERSONA_PEATON, revise a continuación. <br />" + errorvehiculo);
                //validacolaboradores = true;
                return;
            }
        }

        public void LlenaCombos()
        {
            try
            {
                cmbNorma.DataSource = TarifarioN4.consultaTarifarioTipo("BASC");
                cmbNorma.DataValueField = "codigo";
                cmbNorma.DataTextField = "descripcion";
                cmbNorma.DataBind();

                cmbTipo.DataSource = TarifarioN4.consultaTarifarioTipo("TIPO");
                cmbTipo.DataValueField = "codigo";
                cmbTipo.DataTextField = "descripcion";
                cmbTipo.DataBind();
            }
            catch (Exception ex)
            {
                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(LlenaCombos), "LlenadoCombo", false, null, null, ex.StackTrace, ex);
                OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                Response.Write("<script language='JavaScript'>alert('" + OError + "');</script>");
              
            }
        }

        protected void cmbNorma_SelectedIndexChanged(object sender, EventArgs e)
        {
            HiddenField1.Value = ObtenerTarifa();
            //UPPRINCIPAL.Update();
        }

        protected void cmbTipo_SelectedIndexChanged(object sender, EventArgs e)
        {
            HiddenField1.Value = ObtenerTarifa();
            //UPPRINCIPAL.Update();
        }

        private void Mostrar_Mensaje(int Tipo, string Mensaje)
        {
            if (Tipo == 1)
            {
                this.alerta.Attributes["class"] = string.Empty;
                this.alerta.Attributes["class"] = "alert alert-info";
            }
            else
            {
                this.alerta.Attributes["class"] = string.Empty;
                this.alerta.Attributes["class"] = "alert alert-danger";

            }
            //class="alert alert-danger"
            this.alerta.Visible = true;
            this.alerta.InnerHtml = Mensaje;
            this.UPdetalle.Update();
        }

        protected void tablePagination_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            try
            {
                var usn = new usuario();
                HttpCookie token = HttpContext.Current.Request.Cookies["token"];
                usn = this.getUserBySesion();

                if (usn == null)
                {
                    var ex = new ApplicationException(string.Format("IP:{0},URL:{1},METODO:{2}, El usuario NO EXISTE", Request.UserHostAddress, Request.Url, Request.HttpMethod));
                    var number = log_csl.save_log<Exception>(ex, "pases_zal", "RepeaterBooking_ItemCommand", "No usuario", Request.UserHostAddress);
                    this.PersonalResponse(string.Format("Está intentando acceder a un área que requiere autenticación, por su seguridad los datos de su equipo han quedado registrados, gracias. <br/>{0} <br/>{1},<br/>{2}", Request.UserHostAddress, Request.Url, Request.HttpMethod), null);
                    return;
                }
                if (token == null)
                {
                    this.Alerta("Estimado Cliente,Su sesión ha expirado, sera redireccionado a la pagina de login", true);
                    System.Web.Security.FormsAuthentication.SignOut();
                    Session.Clear();
                    return;
                }

                Mostrar_Mensaje(1, "Confirme que los documentos del colaborador(es) sean los correctos.");

                if (e.CommandName == "Rechazar")
                {
                    var item = e.Item;
                    TextBox tcomentario = item.FindControl("tcomentario") as TextBox;

                    if (salir.Visible)
                    {
                        this.Alerta("Acción no permitida, La solicitud ya ha sido procesada");
                        Mostrar_Mensaje(2, "Acción no permitida, La solicitud ya ha sido procesada.");
                        return;
                    }

                    if (string.IsNullOrEmpty(tcomentario.Text))
                    {
                        //Response.Write("<script language='JavaScript'>alert('Se debe agregar un comentario para rechazo especifico');</script>");
                        //this.Alerta("Se debe agregar un comentario para rechazo especifico");
                        //this.Alerta("No se encontro tarifario N4 configurado.\\n" + "Revise la configuración del tarifario antes de continuar con la solicitud de  para .");
                        //this.alerta.InnerHtml = "Se debe agregar un comentario para rechazo especifico.";
                        Mostrar_Mensaje(2, "Se debe agregar un comentario para rechazo especifico.");
                        return;
                    }
                    int v_accion = 0;
                    var v_argumentos = e.CommandArgument.ToString().Split(',');
                    string v_idSol = v_argumentos[0].ToString();
                    string v_idSolcol = v_argumentos[1].ToString();
                    string mensaje;
                    if (!credenciales.RechazarColaboradorEspecifico(
                        v_idSol,
                        v_idSolcol,
                        tcomentario.Text,
                        Page.User.Identity.Name,
                        out mensaje))
                    {
                        //this.Alerta(mensaje);
                        v_accion = 1;
                    }
                    else
                    {
                        //this.Alerta("Colaborador rechazado exitosamente.");
                        v_accion = 2;
                    }
                    ConsultaInfoSolicitud();

                    if (v_accion == 1)
                    {
                        Mostrar_Mensaje(2, mensaje);
                    }
                    else
                    {
                        Mostrar_Mensaje(1, "Colaborador rechazado exitosamente.");
                    }
                    UPdetalle.Update();
                }
            }
            catch (Exception ex)
            {
                var number = log_csl.save_log<Exception>(ex, "autoriza_booking", "RepeaterBooking_ItemCommand()", DateTime.Now.ToShortDateString(), Page.User.Identity.Name);
                //this.Alerta(string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number.ToString()));
                Mostrar_Mensaje(2, string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number.ToString()));
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getGifOculta();", true);
            }
        }
    }
}