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
using System.Text;

namespace CSLSite
{
    public partial class revisasolicitudpermisoprovisional : System.Web.UI.Page
    {
        private GetFile.Service getFile = new GetFile.Service();
        private OnlyControl.OnlyControlService onlyControl = new OnlyControl.OnlyControlService();
        private String xmlDocumentos;

        public DataSet dsListaPermisos
        {
            get { return (DataSet)Session["dsListaPermisosPP"]; }
            set { Session["dsListaPermisosPP"] = value; }
        }

        public DataTable dtDocumentos
        {
            get { return (DataTable)Session["dtDocumentosrevisasolicitudpermisoprovisional"]; }
            set { Session["dtDocumentosrevisasolicitudpermisoprovisional"] = value; }
        }
        public String numsolicitudempresa
        {
            get { return (String)Session["numsolicitudrevisasolicitudpermisoprovisional"]; }
            set { Session["numsolicitudrevisasolicitudpermisoprovisional"] = value; }
        }
        public string codigousuario
        {
            get { return (string)Session["codigousuariorevisasolicitudpermisoprovisional"]; }
            set { Session["codigousuariorevisasolicitudpermisoprovisional"] = value; }
        }
        public string rucempresa
        {
            get { return (string)Session["rucempresarevisasolicitudpermisoprovisional"]; }
            set { Session["rucempresarevisasolicitudpermisoprovisional"] = value; }
        }
        public string useremail
        {
            get { return (string)Session["useremailrevisasolicitudpermisoprovisional"]; }
            set { Session["useremailrevisasolicitudpermisoprovisional"] = value; }
        }
        public string mensajefac
        {
            get { return (string)Session["mensajefacrevisasolicitudpermisoprovisional"]; }
            set { Session["mensajefacrevisasolicitudpermisoprovisional"] = value; }
        }
        public string mensajeok
        {
            get { return (string)Session["mensajeokrevisasolicitudpermisoprovisional"]; }
            set { Session["mensajeokrevisasolicitudpermisoprovisional"] = value; }
        }

        //PANTALLA DE DOCUMENTOS
        public DataTable dtDocumentosUP
        {
            get { return (DataTable)Session["dtDocumentosrevisasolicitudpermisoprovisionaldocumentos"]; }
            set { Session["dtDocumentosrevisasolicitudpermisoprovisionaldocumentos"] = value; }
        }
        private String xmlDocumentosUP;
        public String cedula
        {
            get { return (String)Session["cedularevisasolicitudpermisoprovisionaldocumentos"]; }
            set { Session["cedularevisasolicitudpermisoprovisionaldocumentos"] = value; }
        }
        public String numsolicitudempresaUP
        {
            get { return (String)Session["numsolicitudrevisasolicitudpermisoprovisionaldocumentos"]; }
            set { Session["numsolicitudrevisasolicitudpermisoprovisionaldocumentos"] = value; }
        }
        public String idsolcolUP
        {
            get { return (String)Session["idsolcoldocrevisasolicitudpermisoprovisionaldocumentos"]; }
            set { Session["idsolcoldocrevisasolicitudpermisoprovisionaldocumentos"] = value; }
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            if (!Request.IsAuthenticated)
            {
                csl_log.log_csl.save_log<ApplicationException>(new ApplicationException("No autenticado"), "container", "Page_Init", "No autenticado", "No disponible");
                this.PersonalResponse("Para acceder a esta área necesita estar autenticado, será redireccionado a la página de login", "../login.aspx", true);
            }
          //  this.IsAllowAccess();
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
                rucempresa = user.ruc;
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
                Session["dtDocumentosrevisasolicitudcolaborador"] = new DataTable();
                this.btnGrabarVerificacion.Attributes["disabled"] = "disabled";
                //CARGA COMBO DE PERMISOS 
                dsListaPermisos = serviciosCredenciales.ConsultaPermiso();
                dsListaPermisos.Tables[0].Rows.Add("0", " * Elija * ", "0");

                cmbPermiso.DataSource = dsListaPermisos;
                cmbPermiso.DataValueField = "ID_PERMISO";
                cmbPermiso.DataTextField = "DESCRIPCION";
                cmbPermiso.DataBind();
                cmbPermiso.SelectedValue = "0";

                cmbPermiso1.DataSource = dsListaPermisos;
                cmbPermiso1.DataValueField = "ID_PERMISO";
                cmbPermiso1.DataTextField = "DESCRIPCION";
                cmbPermiso1.DataBind();
                cmbPermiso1.SelectedValue = "0";
                ConsultaInfoSolicitud();
                this.UPFotos.Update();
            }
        }
        private void ConsultaInfoSolicitud()
        {
            try
            {
                foreach (string var in Request.QueryString)
                {
                    numsolicitudempresa = Request.QueryString[var];
                }
                if (!credenciales.GetTipoSolicitud(numsolicitudempresa))
                {
                    if (!credenciales.GetSolicitudEstado(numsolicitudempresa))
                    {
                        botonera.Visible = false;
                        salir.Visible = true;
                        factura.Visible = false;
                    }
                    else
                    {
                        factura.Visible = false;
                        btsalvar.Text = "Crear Permiso";
                        btsalvar.ToolTip = "Crea el permiso provisional.";
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
                        factura.Visible = false;
                    }
                    else
                    {
                        alertafu.InnerHtml = "Adjunte el archivo en formato PDF";
                        factura.Visible = true;
                    }
                    mensajefac = "facturación";
                    mensajeok = "facturada";
                }

                var DescripcionTipoSolicitud = credenciales.GetDescripcionTipoSolicitudXNumSolicitud(numsolicitudempresa);
                for (int i = 0; i <= DescripcionTipoSolicitud.Rows.Count - 1; i++)
                {
                    txttipcli.Text = DescripcionTipoSolicitud.Rows[i][0].ToString();
                }
                this.alerta.InnerHtml = "Confirme que los documentos del colaborador(es) sean los correctos.";

                DataTable dtDocSol = new DataTable();
                try
                {
                    dtDocSol = Session["dtDocumentosrevisasolicitudcolaborador"] as DataTable;
                }
                catch
                {
                    dtDocSol = null;
                }

                if (dtDocSol != null)
                {
                    if (!(dtDocSol.Rows.Count > 0))
                    {
                        Session["dtDocumentosrevisasolicitudcolaborador"] = new DataTable();
                    }
                }
                    
                var tablixVehiculo = credenciales.GetSolicitudColaboradorPermisoProvisional_New(numsolicitudempresa);

                //SE CARGAN LOS COLABORADORES RECHAZADOS EN LA CONSULTA RECIENTE
                RepeaterItemCollection myItemCollection = tablePagination.Items;
                RepeaterItem[] myItemArray = new RepeaterItem[myItemCollection.Count];
                myItemCollection.CopyTo(myItemArray, 0);

                tablePagination.DataSource = tablixVehiculo;
                tablePagination.DataBind();

                //SE RECORRE LA LISTA DE COLABORADORES DE LA SOLICITUD PARA VERIFICAR SI ESTAN RECHAZADOS y PONERLE EL ESTADO "E"-EXCLUIDO AL DATATABLE
                foreach (RepeaterItem filaCopy in myItemArray)
                {
                    Label lblcipasColabCopy = filaCopy.FindControl("lblcipas") as Label;
                    CheckBox chkRevisadoColabCopy = filaCopy.FindControl("chkRevisado") as CheckBox;

                    //SI ESTA RECHAZADO EL COLABORADOR SE PROCEDE A REFRESCAR LA NUEVA DATA QUE SE CARGARÁ EN EL GRID
                    if (chkRevisadoColabCopy.Checked)
                    {
                        //SE BUSCA POR CEDULA LINEA A LINEA AL COLABORADOR PARA MARCAR EL CHECK DE LA NUEVA DATA CARGADA EN EL GRID
                        foreach (RepeaterItem fila in tablePagination.Items)
                        {
                            Label lblcipasColab = fila.FindControl("lblcipas") as Label;
                            CheckBox chkRevisadoColab = fila.FindControl("chkRevisado") as CheckBox;

                            if (lblcipasColabCopy.Text == lblcipasColab.Text)
                            {
                                foreach (DataRow itemdr in tablixVehiculo.Rows)
                                {
                                    if (itemdr["CIPAS"].ToString() == lblcipasColabCopy.Text)
                                    {
                                        foreach (DataColumn col in tablixVehiculo.Columns)
                                        {
                                            col.ReadOnly = false;
                                        }

                                        itemdr["ESTADO"] = "E";
                                        itemdr["ESTADODESC"] = "<b style='color:#339D28';>EXCLUIDO</b>";
                                    }
                                }
                                tablixVehiculo.AcceptChanges();
                            }
                        }
                    }
                }

                if (!botonera.Visible)
                {
                    foreach (DataRow itemdr in tablixVehiculo.Rows)
                    {
                        if (itemdr["estadoColaborador"].ToString() == "R")
                        {
                            foreach (DataColumn col in tablixVehiculo.Columns)
                            {
                                col.ReadOnly = false;
                            }

                            itemdr["ESTADO"] = "E";
                            itemdr["ESTADODESC"] = "<b style='color:#339D28';>EXCLUIDO</b>";
                        }
                    }
                    tablixVehiculo.AcceptChanges();
                }

                //SE REFRESCA LA DATA DE GRID DE COLABORADORES
                tablePagination.DataSource = tablixVehiculo;
                tablePagination.DataBind();

                
                //SE RECORRE LA LISTA DE COLABORADORES DE LA SOLICITUD PARA VERIFICAR SI ESTAN RECHAZADOS y PONERLE EL CHECK DE RECHAZADOS
                foreach (RepeaterItem filaCopy in myItemArray)
                {
                    Label lblcipasColabCopy = filaCopy.FindControl("lblcipas") as Label;
                    CheckBox chkRevisadoColabCopy = filaCopy.FindControl("chkRevisado") as CheckBox;

                    //SI ESTA RECHAZADO EL COLABORADOR SE PROCEDE A REFRESCAR LA NUEVA DATA QUE SE CARGARÁ EN EL GRID
                    if (chkRevisadoColabCopy.Checked)
                    {
                        //SE BUSCA POR CEDULA LINEA A LINEA AL COLABORADOR PARA MARCAR EL CHECK DE LA NUEVA DATA CARGADA EN EL GRID
                        foreach (RepeaterItem fila in tablePagination.Items)
                        {
                            Label lblcipasColab = fila.FindControl("lblcipas") as Label;
                            CheckBox chkRevisadoColab = fila.FindControl("chkRevisado") as CheckBox;

                            if (lblcipasColabCopy.Text == lblcipasColab.Text)
                            {
                                chkRevisadoColab.Checked = true;

                                //foreach ( DataRow itemdr in tablixVehiculo.Rows)
                                //{
                                //    if (itemdr["CIPAS"].ToString() == lblcipasColabCopy.Text)
                                //    {
                                //        foreach (DataColumn col in tablixVehiculo.Columns)
                                //        {
                                //            col.ReadOnly = false;
                                //        }

                                //        itemdr["ESTADO"] = "E";
                                //        itemdr["ESTADODESC"] = "<b style='color:#339D28';>EXCLUIDO</b>";
                                //        //itemdr.EndEdit();
                                //    }
                                //}
                                UPdetalle.Update();
                                //tablixVehiculo.AcceptChanges();
                            }
                        }
                    }
                }

                if (!botonera.Visible)
                {
                    foreach (RepeaterItem fila in tablePagination.Items)
                    {
                        Label lblcipasColab = fila.FindControl("lblcipas") as Label;
                        CheckBox chkRevisadoColab = fila.FindControl("chkRevisado") as CheckBox;

                        string v_estado = tablixVehiculo.Select(" CIPAS = '" + lblcipasColab.Text +"'").FirstOrDefault()["estadoColaborador"].ToString();

                        if (v_estado != "A")
                        {
                            chkRevisadoColab.Checked = true;
                            //fila.DataItem[2] = "";
                            UPdetalle.Update();
                        }
                    }
                }

                if (tablixVehiculo.Rows.Count > 0)
                {
                    txtusuariosolper.Text = tablixVehiculo.Rows[0]["NOMBRESREPLEGAL"].ToString();
                    txtarea.Text = tablixVehiculo.Rows[0]["AREA"].ToString();
                    txtactper.Text = tablixVehiculo.Rows[0]["ACTIVIDAD"].ToString();
                    txtfecing.Text = Convert.ToDateTime(tablixVehiculo.Rows[0]["FECHAINGRESO"]).ToString("dd/MM/yyyy");
                    txtfecsal.Text = Convert.ToDateTime(tablixVehiculo.Rows[0]["FECHASALIDA"]).ToString("dd/MM/yyyy");

                    txtfecing1.Text = Convert.ToDateTime(tablixVehiculo.Rows[0]["FECHAINGRESO"]).ToString("dd/MM/yyyy");
                    txtfecsal1.Text = Convert.ToDateTime(tablixVehiculo.Rows[0]["FECHASALIDA"]).ToString("dd/MM/yyyy");

                    txttipcli.Text = CslHelper.getShiperName(rucempresa);
                    codigousuario = tablixVehiculo.Rows[0]["CODIGOUSUARIO"].ToString();
                }
                var turnoOnlyControl = credenciales.GetConsultaTurno();
                populateDropDownList(ddlTurnoOnlyControl, turnoOnlyControl, "* Elija *", "TUR_ID", "TUR_D", false);
                ddlTurnoOnlyControl.SelectedItem.Text = "* Elija *";

                ddlTurnoOnlyControl1.DataSource = turnoOnlyControl;
                ddlTurnoOnlyControl1.DataTextField = "TUR_D";
                ddlTurnoOnlyControl1.DataValueField = "TUR_ID";
                ddlTurnoOnlyControl1.DataBind();
                ddlTurnoOnlyControl1.SelectedItem.Text = "* Elija *";

                string error_consulta = "";
                error_consulta = string.Empty;
                var dptoOnlyControl = credenciales.GetDptoOnlyControl(rucempresa, out error_consulta);
                if (!string.IsNullOrEmpty(error_consulta))
                {
                    Response.Write("<script language='JavaScript'>alert('" + error_consulta + "');</script>");
                }
                error_consulta = string.Empty;
                var cargoOnlyControl = credenciales.GetCargoOnlyControl(rucempresa, out error_consulta);
                if (!string.IsNullOrEmpty(error_consulta))
                {
                    Response.Write("<script language='JavaScript'>alert('" + error_consulta + "');</script>");
                }
               //var turnoOnlyControl = credenciales.GetConsultaTurno();
                var areaOnlyControl = onlyControl.AC_C_AREA("%", 1, ref error_consulta); //credenciales.GetConsultaArea();
                populateDropDownList(ddlAreaOnlyControl, areaOnlyControl.Tables[0], "* Elija *", "AREA_ID", "AREA_NOM", false);
                populateDropDownList(ddlDepartamentoOnlyControl, dptoOnlyControl, "* Elija *", "DEP_ID", "DEP_NOM", true);
                populateDropDownList(ddlCargoOnlyControl, cargoOnlyControl, "* Elija *", "CALI_ID", "CALI_NOM", false);
                //populateDropDownList(ddlTurnoOnlyControl, turnoOnlyControl, "* Elija *", "TUR_ID", "TUR_D", false);
                ddlAreaOnlyControl.SelectedItem.Text = "* Elija *";
                ddlDepartamentoOnlyControl.SelectedItem.Text = "* Elija *";
                ddlCargoOnlyControl.SelectedItem.Text = "* Elija *";
                //ddlTurnoOnlyControl.SelectedItem.Text = "* Elija *";
                //var tiempocre = credenciales.GetTiempoCaducidadCredencialPermanente();
                //txtfecing.Text = DateTime.Now.ToString("dd/MM/yyyy");
                //txtfecsal.Text = DateTime.Now.AddDays(tiempocre).ToString("dd/MM/yyyy");

                

                ddlAreaOnlyControl1.DataSource = areaOnlyControl.Tables[0];
                ddlAreaOnlyControl1.DataTextField = "AREA_NOM";
                ddlAreaOnlyControl1.DataValueField = "AREA_ID";
                ddlAreaOnlyControl1.DataBind();

                ddlDepartamentoOnlyControl1.DataSource = dptoOnlyControl;
                ddlDepartamentoOnlyControl1.DataTextField = "DEP_NOM";
                ddlDepartamentoOnlyControl1.DataValueField = "DEP_ID";
                ddlDepartamentoOnlyControl1.DataBind();

                ddlCargoOnlyControl1.DataSource = cargoOnlyControl;
                ddlCargoOnlyControl1.DataTextField = "CALI_NOM";
                ddlCargoOnlyControl1.DataValueField = "CALI_ID";
                ddlCargoOnlyControl1.DataBind();

                ddlAreaOnlyControl1.SelectedItem.Text = "* Elija *";
                ddlDepartamentoOnlyControl1.SelectedItem.Text = "* Elija *";
                ddlCargoOnlyControl1.SelectedItem.Text = "* Elija *";

                if (botonera.Visible)
                {
                    if (tablixVehiculo.Select(" ESTADO IN ('V','E') ").Count() < tablixVehiculo.Rows.Count)
                    {
                        xfinderpagado.Visible = false;
                    }
                    else
                    {
                        if (tablixVehiculo.Select(" ESTADO IN ('E') ").Count() < tablixVehiculo.Rows.Count)
                        {
                            xfinderpagado.Visible = true;
                        }
                        else
                        {
                            xfinderpagado.Visible = false;
                        }

                    }
                }

                if (salir.Visible)
                {
                    xfinderpagado.Visible = true;
                }
            }
            catch (Exception ex)
            {
                Session["dtDocumentosrevisasolicitudcolaborador"] = new DataTable();
                Response.Write("<script language='JavaScript'>alert('" + ex.Message + "');</script>");
            }
            UPdetalle.Update();
            UPPAGADO.Update();
        }
        private void populateDropDownList(DropDownList dp, DataTable origen, string mensaje, string id, string descripcion, bool val)
        {
            if (val)
            {
                origen.Rows.Add("0", "0", mensaje);
            }
            else
            {
                origen.Rows.Add("0", mensaje);
            }
            DataView dvorigen = new DataView();
            dvorigen = origen.DefaultView;
            dvorigen.Sort = descripcion;
            dp.DataSource = dvorigen;
            dp.DataValueField = id;
            dp.DataTextField = descripcion;
            dp.DataBind();
        }
        protected void btsalvar_Click(object sender, EventArgs e)
        {
            if (!xfinderpagado.Visible)
            {
                //Response.Write("<script language='JavaScript'>var r=alert('Confirmación de pago enviado exitosamente.');if(r==true){window.close()}else{window.close()};</script>");
                this.Alerta("No se puede crear el permiso, Existen Fotos Pendientes de verificar o Rechazadas.");
                return;
            }

            DataTable dtDocSol = Session["dtDocumentosrevisasolicitudcolaborador"] as DataTable;

 /*           foreach (RepeaterItem item in tablePagination.Items)
            {
                CheckBox chkRevisado = item.FindControl("chkRevisado") as CheckBox;

                if (chkRevisado.Checked)
                {
                    if (dtDocSol == null)
                    {
                        Response.Write("<script language='JavaScript'>var r=alert('Existen documentos rechazados los cuales no se cargaron.  \\nPor favor, volver a intentar');if(r==true){window.close()}else{window.close()};</script>");
                        return;
                    }

                    if (!(dtDocSol.Rows.Count > 0))
                    {
                        Response.Write("<script language='JavaScript'>var r=alert('Existen documentos rechazados los cuales no se cargaron.  \\nPor favor, volver a intentar');if(r==true){window.close()}else{window.close()};</script>");
                        return;
                    }
                }
                
            }
*/
            try
            {
                CultureInfo enUS = new CultureInfo("en-US");
                DateTime fechaing;
                if (!DateTime.TryParseExact(txtfecing.Text, "dd/MM/yyyy", enUS, DateTimeStyles.None, out fechaing))
                {
                    this.Alerta(string.Format("EL FORMATO DE FECHA DEBE SER dia/Mes/Anio{0}\\n", txtfecing.Text));
                    txtfecing.Focus();
                    return;
                }
                DateTime fechacad;
                if (!DateTime.TryParseExact(txtfecsal.Text, "dd/MM/yyyy", enUS, DateTimeStyles.None, out fechacad))
                {
                    this.Alerta(string.Format("EL FORMATO DE FECHA DEBE SER dia/Mes/Anio{0}\\n", txtfecsal.Text));
                    txtfecsal.Focus();
                    return;
                }
                TimeSpan tsDias = fechacad - fechaing;
                int diferenciaEnDias = tsDias.Days;
                if (diferenciaEnDias < 0)
                {
                    this.Alerta("La Fecha de Ingreso: " + txtfecing.Text + "\\nNO deber ser mayor a la\\nFecha de Caducidad: " + txtfecsal.Text);
                    return;
                }
                foreach (string var in Request.QueryString)
                {
                    numsolicitudempresa = Request.QueryString[var];
                }
                Boolean banderafac = false;
                List<string> listCedulas = new List<string>();
                //DataTable dtDocSol = Session["dtDocumentosrevisasolicitudcolaborador"] as DataTable;
                foreach (RepeaterItem item in tablePagination.Items)
                {
                    CheckBox chkRevisado = item.FindControl("chkRevisado") as CheckBox;
                    Label lblcipas = item.FindControl("lblcipas") as Label;
                    TextBox tcomentario = item.FindControl("tcomentario") as TextBox;
                    if (chkRevisado.Checked == false && !string.IsNullOrEmpty(tcomentario.Text))
                    {
                        this.Alerta("Marque la casilla del Comentario de rechazo. \\n Cedula: " + lblcipas.Text);
                        return;
                    }
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
                DataTable dtColaboradores = credenciales.GetSolicitudColaboradorPermisoProvisional(numsolicitudempresa);
                var resultAprobados = from myRow in dtColaboradores.AsEnumerable()
                                      where !listCedulas.Contains(myRow.Field<string>("CIPAS"))
                                      select myRow;
                DataTable dtAprobados = resultAprobados.AsDataView().ToTable();
                if (dtAprobados.Rows.Count == 0)
                {
                    this.Alerta("Tiene un unico colaborador rechazado.\\n" + "Revise la información antes de continuar con la " + mensajefac + " de la solicitud.\\n Si quiere continuar de click en Rechazar.");
                    return;
                }
                var coloboradores = dtColaboradores;//credenciales.GetSolicitudColaboradorPermisoProvisional(numsolicitudempresa);
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
                String errorvehiculo = string.Empty;
                Int32 registros_actualizados_correcto = 0;
                Int32 registros_actualizados_incorrecto = 0;
                String error_consulta = string.Empty;
                String error = string.Empty;
                var empresa = credenciales.GetEmpresaColaborador(numsolicitudempresa).Rows[0]["RAZONSOCIAL"].ToString();
                if (!string.IsNullOrEmpty(error_consulta))
                {
                    var ex = new ApplicationException(string.Format("Error al usar el Metodo onlyControl.AC_C_EMPRESA():{0}", error_consulta));
                    var number = log_csl.save_log<Exception>(ex, "revisasolicitudpermisoprovisional", "btsalvar_Click", "1", Request.UserHostAddress);
                    this.Alerta(string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number.ToString()));
                    return;}           
                DataSet dsErrorAC_R_PERSONA_PEATON = new DataSet();
                DataTable dtAC_R_PERSONA_PEATON = new DataTable();
                DataSet dsAC_R_PERSONA_PEATON = new DataSet();
                dtAC_R_PERSONA_PEATON.Columns.Add("CEDULA");
                dtAC_R_PERSONA_PEATON.Columns.Add("APELLIDOS");
                dtAC_R_PERSONA_PEATON.Columns.Add("NOMBRES");
                dtAC_R_PERSONA_PEATON.Columns.Add("EMPRESA");
                dtAC_R_PERSONA_PEATON.Columns.Add("CARGO");
                DataSet dsErrorAC_R_PERSONA = new DataSet();
                DataTable dtAC_R_PERSONA = new DataTable();
                DataSet dsAC_R_PERSONA = new DataSet();
                dtAC_R_PERSONA.Columns.Add("CEDULA");
                dtAC_R_PERSONA.Columns.Add("APELLIDOS");
                dtAC_R_PERSONA.Columns.Add("NOMBRES");
                dtAC_R_PERSONA.Columns.Add("EMPRESA");
                dtAC_R_PERSONA.Columns.Add("AREA");
                dtAC_R_PERSONA.Columns.Add("DPTO");
                dtAC_R_PERSONA.Columns.Add("CARGO");
                dtAC_R_PERSONA.Columns.Add("EXPIRACION");
                dtAC_R_PERSONA.Columns.Add("TIPO_SANGRE");
                dtAC_R_PERSONA.Columns.Add("TIPO_LICENCIA");
                dtAC_R_PERSONA.Columns.Add("FECHA_INGRESO");
                dtAC_R_PERSONA.Columns.Add("FECHA_CADUCIDAD");
                foreach (RepeaterItem item in tablePagination.Items)
                {
                    CheckBox chkRevisado = item.FindControl("chkRevisado") as CheckBox;
                    Label lblcipas = item.FindControl("lblcipas") as Label;
                    if (!chkRevisado.Checked)
                    {
                        var results = from myRow in dtColaboradores.AsEnumerable()
                                      where myRow.Field<string>("CIPAS") == lblcipas.Text
                                      select myRow;
                        DataTable dtresults = results.AsDataView().ToTable();
                        dtAC_R_PERSONA_PEATON.Rows.Add(dtresults.Rows[0]["CIPAS"], dtresults.Rows[0]["APELLIDOS"], dtresults.Rows[0]["NOMBRES"], empresa, dtresults.Rows[0]["CARGO"]);
                        //dtAC_R_PERSONA.Rows.Add(dtresults.Rows[0]["CIPAS"], dtresults.Rows[0]["APELLIDOS"], dtresults.Rows[0]["NOMBRES"], empresa, dtresults.Rows[0]["AREA"], "PPP PERMISO PEATONAL PROVISIONAL", dtresults.Rows[0]["CARGO"], DateTime.Now.ToString("yyyy-MM-dd"), dtresults.Rows[0]["TIPOSANGRE"], "", fechaing.ToString("yyyy-MM-dd"), fechacad.ToString("yyyy-MM-dd"));
                    }
                }
                //dsAC_R_PERSONA.Tables.Add(dtAC_R_PERSONA);
                dsAC_R_PERSONA_PEATON.Tables.Add(dtAC_R_PERSONA_PEATON);
                ////Nomina, Sistema Azul
                //dsErrorAC_R_PERSONA = onlyControl.AC_R_PERSONA_NOMINA(dsAC_R_PERSONA, ref registros_actualizados_correcto, ref registros_actualizados_incorrecto, ref error_consulta) as DataSet;
                //if (OnlyControlError(dsErrorAC_R_PERSONA, registros_actualizados_incorrecto, error_consulta, "AC_R_PERSONA_NOMINA"))
                //{
                //    return;
                //}
                //dsErrorAC_R_PERSONA = onlyControl.AC_R_PERSONA_NOMINA(dsAC_R_PERSONA, ref registros_actualizados_correcto, ref registros_actualizados_incorrecto, ref error_consulta) as DataSet;
                //if (OnlyControlError(dsErrorAC_R_PERSONA, registros_actualizados_incorrecto, error_consulta, "AC_R_PERSONA_NOMINA"))
                //{
                //    return;
                //}
                //Nomina Sistema Celeste
                dsErrorAC_R_PERSONA_PEATON = onlyControl.AC_R_PERSONA_PEATON(dsAC_R_PERSONA_PEATON, ref registros_actualizados_correcto, ref registros_actualizados_incorrecto, ref error_consulta) as DataSet;
                if (OnlyControlError(dsErrorAC_R_PERSONA_PEATON, registros_actualizados_incorrecto, error_consulta, "AC_R_PERSONA_PEATON"))
                {
                    return;
                }       
                DataTable dtAC_R_PERMISO_TEMPORAL = new DataTable();
                DataSet dsAC_R_PERMISO_TEMPORAL = new DataSet();
                DataSet dsErrorAC_R_PERMISO_TEMPORAL = new DataSet();
                dtAC_R_PERMISO_TEMPORAL.Columns.Add("ID_PERMISO");
                dtAC_R_PERMISO_TEMPORAL.Columns.Add("EMPRESA");
                dtAC_R_PERMISO_TEMPORAL.Columns.Add("AREA");
                dtAC_R_PERMISO_TEMPORAL.Columns.Add("ID_SOLICITANTE");
                dtAC_R_PERMISO_TEMPORAL.Columns.Add("CEDULA_SOLICITANTE");
                dtAC_R_PERMISO_TEMPORAL.Columns.Add("SOLICITANTE");
                dtAC_R_PERMISO_TEMPORAL.Columns.Add("ACTIVIDAD");
                dtAC_R_PERMISO_TEMPORAL.Columns.Add("FECHA_INICIO");
                dtAC_R_PERMISO_TEMPORAL.Columns.Add("FECHA_FIN");
                var replegal = txtusuariosolper.Text;
                var arrglo = replegal.Split(';');
                var cedulareplegal = arrglo[0];
                var nombresreplegal = arrglo[1];
                dtAC_R_PERMISO_TEMPORAL.Rows.Add("", empresa, ddlAreaOnlyControl.SelectedItem, "", cedulareplegal, nombresreplegal, txtactper.Text, fechaing.ToString("yyyy-MM-dd") + " 00:00", fechacad.ToString("yyyy-MM-dd") + " 23:59");
                dsAC_R_PERMISO_TEMPORAL.Tables.Add(dtAC_R_PERMISO_TEMPORAL);
                dsErrorAC_R_PERMISO_TEMPORAL = onlyControl.AC_R_PERMISO_TEMPORAL(dsAC_R_PERMISO_TEMPORAL, ref registros_actualizados_correcto, ref registros_actualizados_incorrecto, ref error_consulta) as DataSet;
                if (OnlyControlError(dsErrorAC_R_PERMISO_TEMPORAL, registros_actualizados_incorrecto, error_consulta, "AC_R_PERSONA_NOMINA"))
                {
                    return;
                }   
                DataTable dtAC_R_ASIGNA_PERSONA = new DataTable();
                DataSet dsAC_R_ASIGNA_PERSONA = new DataSet();
                DataSet dsErrorAC_R_ASIGNA_PERSONA = new DataSet();
                dtAC_R_ASIGNA_PERSONA.Columns.Add("ID_PERMISO");
                dtAC_R_ASIGNA_PERSONA.Columns.Add("CEDULA");
                dtAC_R_ASIGNA_PERSONA.Columns.Add("ID_PERSONA");
                dtAC_R_ASIGNA_PERSONA.Columns.Add("PERSONA");
                dtAC_R_ASIGNA_PERSONA.Columns.Add("USUARIO");
                foreach (RepeaterItem item in tablePagination.Items)
                {
                    CheckBox chkRevisado = item.FindControl("chkRevisado") as CheckBox;
                    Label lblcipas = item.FindControl("lblcipas") as Label;
                    Label lblNombres = item.FindControl("lblNombres") as Label;
                    if (!chkRevisado.Checked)
                    {
                        dtAC_R_ASIGNA_PERSONA.Rows.Add(dsErrorAC_R_PERMISO_TEMPORAL.Tables[0].Rows[0]["ID_PERMISO"], lblcipas.Text, "", lblNombres.Text, codigousuario);

                    }
                }
                dsAC_R_ASIGNA_PERSONA.Tables.Add(dtAC_R_ASIGNA_PERSONA);
                dsErrorAC_R_ASIGNA_PERSONA = onlyControl.AC_R_ASIGNA_PERSONA(dsAC_R_ASIGNA_PERSONA, ref registros_actualizados_correcto, ref registros_actualizados_incorrecto, ref error_consulta) as DataSet;
                if (OnlyControlError(dsErrorAC_R_ASIGNA_PERSONA, registros_actualizados_incorrecto, error_consulta, "AC_R_ASIGNA_PERSONA"))
                {
                    return;
                }  
                 DataTable dtPermiso = new DataTable();
                /*dtPermiso.Columns.Add("ID");
               dtPermiso.Columns.Add("F_INGRESO");
               dtPermiso.Columns.Add("F_SALIDA");
               dtPermiso.Columns.Add("HORARIO");
               dtPermiso.Columns.Add("TIPO_CONTROL");*/
                dtPermiso.Columns.Add(new DataColumn("ID", typeof(String)));
                dtPermiso.Columns.Add(new DataColumn("F_HORARIO", typeof(Int32)));
                dtPermiso.Columns.Add(new DataColumn("F_INGRESO", typeof(DateTime)));
                dtPermiso.Columns.Add(new DataColumn("F_SALIDA", typeof(DateTime)));
                dtPermiso.Columns.Add(new DataColumn("TIPO_CONTROL", typeof(Int32))); //1 LIBRE (NO CADUCA O NO VALIDA FECHA) - 2 CONTROLADO
                dtPermiso.Columns.Add(new DataColumn("COD_PERMISO", typeof(Int32))); // NEW - EJ: 65
                dtPermiso.Columns.Add(new DataColumn("ID_PERMISO", typeof(String))); // NEW - EJ: FA
                dtPermiso.Columns.Add(new DataColumn("AREAS", typeof(String))); // NEW
                //var dsPersonasOnlyControl = onlyControl.AC_C_PERSONA(empresa, string.Empty, 1, ref error_consulta);
                //if (!string.IsNullOrEmpty(error_consulta))
                //{
                //    var ex = new ApplicationException(string.Format("Error al usar el Metodo onlyControl.AC_C_PERSONA():{0}", error_consulta));
                //    var number = log_csl.save_log<Exception>(ex, "consultacomprobantedepago", "RegistraPermisosDeAccesoOnlyControl", "1", Request.UserHostAddress);
                //    this.Alerta(string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number.ToString()));
                //    return;
                //}
                DataRow dr;
                for (int i = 0; i < dtAC_R_ASIGNA_PERSONA.Rows.Count; i++)
                {
                    //var resultPersonasOnlyControl = from myRow in dsPersonasOnlyControl.DefaultViewManager.DataSet.Tables[0].AsEnumerable()
                    //                                where myRow.Field<string>("CEDULA") == dtAC_R_ASIGNA_PERSONA.Rows[i]["CEDULA"].ToString()
                    //                                select myRow;
                    //DataTable dt = resultPersonasOnlyControl.AsDataView().ToTable();
                    //if (resultPersonasOnlyControl.AsDataView().ToTable().Rows.Count == 0)
                    //{
                    //    var ex = new ApplicationException(string.Format("Error al usar el Metodo onlyControl.AC_C_PERSONA():{0}", "No se encontro el colaborador: " + dtAC_R_ASIGNA_PERSONA.Rows[i]["CEDULA"].ToString()));
                    //    var number = log_csl.save_log<Exception>(ex, "consultacomprobantedepago", "RegistraPermisosDeAccesoOnlyControl", "1", Request.UserHostAddress);
                    //    this.Alerta(string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number.ToString()));
                    //    return;
                    //}
                    //                    var id = resultPersonasOnlyControl.AsDataView().ToTable().Rows[0]["ID"];
                    var id = onlyControl.GetIdAc_NominaPeaton(dtAC_R_ASIGNA_PERSONA.Rows[i]["CEDULA"].ToString());
                    string setoc = onlyControl.SetPersonaPeatonProvisional(id);
                    //dtPermiso.Rows.Add(id, fechaing.ToString("yyyy-MM-dd"), fechacad.ToString("yyyy-MM-dd"), ddlTurnoOnlyControl.SelectedValue, "2");

                    dr = dtPermiso.NewRow();
                    dr["ID"] = id;//"999999";
                    dr["F_HORARIO"] = ddlTurnoOnlyControl.SelectedValue;// 7
                    dr["F_INGRESO"] = fechaing.ToString("yyyy-MM-dd");// Now.Date
                    dr["F_SALIDA"] = fechacad.ToString("yyyy-MM-dd");// DateAdd("d", 1, Now.Date)
                    dr["TIPO_CONTROL"] = 2;
                    dr["COD_PERMISO"] = txtIdPermiso.Value;// 64
                    dr["ID_PERMISO"] = cmbPermiso.SelectedValue;// "H"
                    dr["AREAS"] = ddlAreaOnlyControl.SelectedValue;// "916" '"873, 874"
                    dtPermiso.Rows.Add(dr);
                }
                DataSet dsPermiso = new DataSet();
                DataSet dsErrorAC_R_PERMISO = new DataSet();
                dsPermiso.Tables.Add(dtPermiso);
                dsErrorAC_R_PERMISO = onlyControl.AC_R_PERMISO_NEW(dsPermiso, ref registros_actualizados_correcto, ref registros_actualizados_incorrecto, ref error_consulta) as DataSet;
                if (OnlyControlError(dsErrorAC_R_PERMISO, registros_actualizados_incorrecto, error_consulta, "AC_R_PERMISO"))
                {
                    if (dsErrorAC_R_PERMISO.Tables[0].Rows[0]["ERROR"].ToString().Substring(0, 15) != "Error Duplicado")
                    {
                        return;   
                    }
                } 
                string mensaje = null;
                //string nombreempresa = CslHelper.getShiperName(rucempresa);

                DataTable dtCola = new DataTable();
                var resultListaApr = from myRow in dtDocSol.AsEnumerable()
                                     where Convert.ToBoolean(myRow.Field<string>("DocumentoRechazado")) == true
                                     select myRow;
                dtCola = resultListaApr.AsDataView().ToTable();
                dtCola.AcceptChanges();
                dtCola.TableName = "Colaboradores";
                StringWriter swC = new StringWriter();
                dtCola.WriteXml(swC);
                xmlDocumentos = swC.ToString();

                StringWriter swAcceso = new StringWriter();
                dsErrorAC_R_ASIGNA_PERSONA.Tables[0].WriteXml(swAcceso);
                String xmlAcceso = swAcceso.ToString();
                if (!credenciales.ApruebaSolicitudColaboradorPaseProvisional_New(
                    numsolicitudempresa,
                    rucempresa,
                    //nombreempresa,
                    //useremail,
                    xmlColaboradores,
                    xmlDocumentos,
                    xmlAcceso,
                    Page.User.Identity.Name.ToUpper(),
                    banderafac,
                    fechaing.ToString("yyyy-MM-dd"),
                    fechacad.ToString("yyyy-MM-dd"),
                    out mensaje))
                {
                    this.Alerta(mensaje);
                }
                else
                {
                    //****************************************************
                    //ACTUALIZA ESTADO DE COLABORADOR RECHAZADO
                    //****************************************************
                    foreach (RepeaterItem item in tablePagination.Items)
                    {
                        CheckBox chkRevisado = item.FindControl("chkRevisado") as CheckBox;
                        Label txtNumeroSolicitudColab = item.FindControl("txtNumeroSolicitudColab") as Label;
                        Label txtNumeroSolicitudColColab = item.FindControl("txtNumeroSolicitudColColab") as Label;
                        TextBox tcomentario = item.FindControl("tcomentario") as TextBox;
                        if (chkRevisado.Checked)
                        {
                            credenciales.ActualizarEstadoColaborador(long.Parse(txtNumeroSolicitudColab.Text), long.Parse(txtNumeroSolicitudColColab.Text), "R", tcomentario.Text, Page.User.Identity.Name.ToUpper(),out mensaje);
                        }
                    }

                    //#####################################################################
                    // SE PROCEDE A REALIZAR EL REGISTRO FACIAL EN LA BASE DE ONLY CONTROL
                    //#####################################################################
                    string v_retornaMsj = string.Empty;
                    
                    RegistroFacialOnlyControl_New(out v_retornaMsj);
                    Response.Write("<script language='JavaScript'>var r=alert('Solicitud " + mensajeok + " exitosamente.  \\n " + v_retornaMsj + "');if(r==true){window.close()}else{window.close()};</script>");
                }
            }
            catch (Exception ex)
            {
                Response.Write("<script language='JavaScript'>alert('" + ex.Message + "');</script>");
            }
        }
        protected void btnRechazar_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (string var in Request.QueryString)
                {
                    numsolicitudempresa = Request.QueryString[var];
                }
                DataTable dtDocSol = Session["dtDocumentosrevisasolicitudcolaborador"] as DataTable;
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
                if (!credenciales.RechazaSolicitudColaboradorPaseProvisional(
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
        private void RegistroFacialOnlyControl_New(out string resultadoStr)
        {
            DataTable dtDocumentos = new DataTable();
            resultadoStr = "";
            String error_consulta = string.Empty;
            CultureInfo enUS = new CultureInfo("en-US");

            foreach (string var in Request.QueryString)
            {
                numsolicitudempresa = Request.QueryString[var];
            }

            var empresa = credenciales.GetEmpresaColaborador(numsolicitudempresa).Rows[0]["RAZONSOCIAL"].ToString();
            var dsPersonasOnlyControl = onlyControl.AC_C_PERSONA(empresa, string.Empty, 1, ref error_consulta);
            if (!string.IsNullOrEmpty(error_consulta))
            {
                var ex = new ApplicationException(string.Format("Error al usar el Metodo onlyControl.AC_C_PERSONA():{0}", error_consulta));
                var number = log_csl.save_log<Exception>(ex, "consultacomprobantedepagocolaborador", "RegistroFacialOnlyControl_New", "1", Request.UserHostAddress);
                resultadoStr = string.Format("Lo sentimos, algo salió mal y no se logró realizar el registro facial. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number.ToString());
                return;
            }

            //Label numsolicitud = gvComprobantes.Rows[0].FindControl("lblIdSolicitud") as Label;
            var dtPermisosDeAcceso = credenciales.GetSolicitudColaboradorOnlyControl(numsolicitudempresa);

            for (int i = 0; i < dtPermisosDeAcceso.Rows.Count; i++)
            {
                var resultPersonasOnlyControl = from myRow in dsPersonasOnlyControl.DefaultViewManager.DataSet.Tables[0].AsEnumerable()
                                                where myRow.Field<string>("CEDULA") == dtPermisosDeAcceso.Rows[i]["CIPAS"].ToString()
                                                select myRow;
                DataTable dt = resultPersonasOnlyControl.AsDataView().ToTable();
                if (resultPersonasOnlyControl.AsDataView().ToTable().Rows.Count == 0)
                {
                    //var ex = new ApplicationException(string.Format("Error al usar el Metodo onlyControl.AC_C_PERSONA():{0}", "No se encontro el colaborador: " + dtPermisosDeAcceso.Rows[i]["CIPAS"].ToString()));
                    //var number = log_csl.save_log<Exception>(ex, "consultacomprobantedepagocolaborador", "RegistroFacialOnlyControl_New", "1", Request.UserHostAddress);
                    //resultadoStr = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number.ToString());
                    //resultadoStr = resultadoStr + string.Format("\\n RF SW OC: {1}{2} de Imagenes del ID {0} :", dtPermisosDeAcceso.Rows[i]["CIPAS"].ToString(), "EMPLEADO NO PERTENECE A LA EMPRESA ", empresa);
                    //string id = onlyControl.GetIdAc_NominaPeaton(dtAC_R_ASIGNA_PERSONA.Rows[i]["CEDULA"].ToString());
                    //continue;
                }
                
                OnlyControl.OnlyControlService oc = new OnlyControl.OnlyControlService();
                string id = oc.GetIdAc_NominaPeaton(dtPermisosDeAcceso.Rows[i]["CIPAS"].ToString());//resultPersonasOnlyControl.AsDataView().ToTable().Rows[0]["ID"].ToString();
                //string id = onlyControl.GetIdAc_NominaPeaton(dtAC_R_ASIGNA_PERSONA.Rows[i]["CEDULA"].ToString());

                DataTable dtFotos = credenciales.GetFotosSolicitudColaboradorOnlyControl(numsolicitudempresa, dtPermisosDeAcceso.Rows[i]["CIPAS"].ToString());

                //###################################################
                //  GRABA REGISTRO FACIAL EN LA BASE DE ONLY CONTROL
                //###################################################
                string img1 = null; string img2 = null; string img3 = null; string imgt1 = string.Empty; string imgt2 = string.Empty; string imgt3 = string.Empty;
                for (int x = 0; x < dtFotos.Rows.Count; x++)
                {
                    //                    string a = SerializeObjectToXmlString(dtFotos.Rows[x]["RF"]);
                    //byte[] v_image = (byte[])dtFotos.Rows[x]["RF"];
                    ////string v_imageStr = dtFotos.Rows[x]["RF"].ToString(); //Convert.ToBase64String(v_image);
                    //string v_imageStr = a.Replace("<?xml version=\"1.0\" encoding=\"utf-16\"?>","");
                    //v_imageStr = v_imageStr.Replace("<base64Binary>", "");
                    //v_imageStr = v_imageStr.Replace("</base64Binary>", "");
                    /*byte[] bData;
                    BinaryReader br = new BinaryReader(System.IO.File.OpenRead(dtFotos.Rows[x]["rutaLocal"].ToString()));
                    bData = br.ReadBytes((int)br.BaseStream.Length);// br.ReadBytes(br.BaseStream.Length)
                    string v_imageStr = Convert.ToBase64String(bData);*/

                    //bData = br.ReadBytes(br.BaseStream.Length());
                    string v_imageStr = dtFotos.Rows[x]["RF"].ToString();
                    if (x == 0) { img1 = v_imageStr; imgt1 = dtFotos.Rows[x]["template"].ToString(); }
                    if (x == 1) { img2 = v_imageStr; imgt2 = dtFotos.Rows[x]["template"].ToString(); }
                    if (x == 2) { img3 = v_imageStr; imgt3 = dtFotos.Rows[x]["template"].ToString(); }
                }
                string strResult;

                RespuestaSwNeuro obRfResult = serviciosCredenciales.ActualizaFace(id, img1, img2, img3, imgt1, imgt2, imgt3);
                credenciales.ActualizarCodigoNominaRegistroFacial(long.Parse(dtPermisosDeAcceso.Rows[i]["NUMSOLICITUD"].ToString()), long.Parse(dtPermisosDeAcceso.Rows[i]["IDSOLCOL"].ToString()), id, out strResult);

                if (obRfResult != null)
                {
                    if (obRfResult.codigo != "1")
                    {
                        var ex = new ApplicationException(string.Format("Error al usar el Metodo onlyControl.ActualizaFace():{0}", obRfResult.mensaje));
                        var number = log_csl.save_log<Exception>(ex, "consultacomprobantedepagocolaborador", "RegistroFacialOnlyControl_New", "1", Request.UserHostAddress);
                        //this.Alerta(string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number.ToString()));
                    }

                    resultadoStr = resultadoStr + string.Format("\\n RF SW OC: {1} de Imagenes del ID {0} ", dtPermisosDeAcceso.Rows[i]["CIPAS"].ToString(), obRfResult.mensaje);
                }
            }
        }
        private bool OnlyControlError(DataSet dsError, int registros_actualizados_incorrecto, string error_consulta, string metodo)
        {
            if (dsError.Tables[0].Rows.Count != 0)
            {
                if (!string.IsNullOrEmpty(dsError.Tables[0].Rows[0]["ERROR"].ToString()))
                {
                    if (registros_actualizados_incorrecto > 0)
                    {
                        if (string.IsNullOrEmpty(error_consulta))
                        {
                            var dserror = dsError.DefaultViewManager.DataSet.Tables;
                            var dterror = dserror[0];
                            var error = string.Empty;
                            for (int i = 0; i < dterror.Rows.Count; i++)
                            {
                                error = error + " \\n *" +
                                       "Error: " + dterror.Rows[0]["ERROR"].ToString() + " \\n ";
                            }
                            var ex = new ApplicationException(string.Format("Error al usar el Metodo" + metodo + ":{0}", error));
                            var number = log_csl.save_log<Exception>(ex, "revisasolicitudpermisoprovisional", "OnlyControlError", registros_actualizados_incorrecto.ToString(), Request.UserHostAddress);
                            this.Alerta(string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number.ToString()));
                            return true;
                        }
                        var ex2 = new ApplicationException(string.Format("Error al usar el Metodo" + metodo + ":{0}", error_consulta));
                        var number2 = log_csl.save_log<Exception>(ex2, "revisasolicitudpermisoprovisional", "OnlyControlError", registros_actualizados_incorrecto.ToString(), Request.UserHostAddress);
                        this.Alerta(string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number2.ToString()));
                        return true;
                    }
                    else if (!string.IsNullOrEmpty(error_consulta))
                    {
                        var ex = new ApplicationException(string.Format("Error al usar el Metodo" + metodo + ":{0}", error_consulta));
                        var number = log_csl.save_log<Exception>(ex, "revisasolicitudpermisoprovisional", "OnlyControlError", registros_actualizados_incorrecto.ToString(), Request.UserHostAddress);
                        this.Alerta(string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number.ToString()));
                        return true;
                    }
                }
            }
            return false;
        }
        protected void tablePagination_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            msjErrores.InnerHtml = "";
            msjErrores.Visible = false;
            UPFotos.Update();
            try
            {
                /*valida cotizacion*/
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



                if (e.CommandName == "Fotos")
                {
                    var v_argumentos = e.CommandArgument.ToString().Split(',');
                    lblidSolicitud.Value = v_argumentos[0].ToString();
                    lblidSolcol.Value = v_argumentos[1].ToString();
                    lblEstado.Value = v_argumentos[2].ToString();

                    if (lblEstado.Value == "A")
                    {
                        xfinderDet.Visible = true;
                        this.btnGrabarVerificacion.Attributes.Remove("disabled");
                    }
                    else
                    {
                        xfinderDet.Visible = false;
                        this.btnGrabarVerificacion.Attributes["disabled"] = "disabled";
                    }

                    UPFotos.Update();

                    if (Response.IsClientConnected)
                    {
                        try
                        {
                            if (HttpContext.Current.Request.Cookies["token"] == null)
                            {
                                System.Web.Security.FormsAuthentication.SignOut();
                                Session.Clear();
                                Response.Redirect("../login.aspx", false);
                                //OcultarLoading("1");
                                return;
                            }

                            if (lblidSolicitud.Value == "" || lblidSolcol.Value == "")
                            {
                                sinresultadoDespacho.Visible = true;
                                xfinderDes.Visible = false;
                                UPFotos.Update();
                                this.Alerta("Debe ingresar el número de carga");
                                return;
                            }

                            var oTarjaDet = credenciales.GetRegistroFacialXNumSolicitudCliente(lblidSolicitud.Value, lblidSolcol.Value);

                            if (oTarjaDet == null)
                            {
                                sinresultadoDespacho.Visible = true;
                                xfinderDes.Visible = false;
                                UPFotos.Update();
                                this.Alerta(string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>{0}", "No se encontró resultados con el número de carga ingresado"));
                                return;
                            }

                            if (oTarjaDet.Rows.Count <= 0)
                            {
                                sinresultadoDespacho.Visible = true;
                                xfinderDes.Visible = false;
                                UPFotos.Update();
                                return;
                            }

                            StringBuilder tab = new StringBuilder();

                            string v_cuerpo = string.Empty;
                            string v_detalle = string.Empty;
                            string v_html = @"  <div class='bs-example'>
                                        <div class='accordion' id='accordionExample'>";

                            foreach (var Det in oTarjaDet.Rows)
                            {
                                DataRow drFila = (DataRow)Det;

                                //HABILITA SWITCH DE RECHAZO/AUTORIZADO
                                if (drFila["secuencia"].ToString() == "1") { if (drFila["estado"].ToString() != "A") { this.customSwitch.Attributes["disabled"] = "disabled"; } else { this.customSwitch.Attributes.Remove("disabled"); } }
                                if (drFila["secuencia"].ToString() == "2") { if (drFila["estado"].ToString() != "A") { this.customSwitch2.Attributes["disabled"] = "disabled"; } else { this.customSwitch2.Attributes.Remove("disabled"); } }
                                if (drFila["secuencia"].ToString() == "3") { if (drFila["estado"].ToString() != "A") { this.customSwitch3.Attributes["disabled"] = "disabled"; } else { this.customSwitch3.Attributes.Remove("disabled"); } }

                                v_detalle = string.Empty;

                                //v_detalle = v_detalle + "<p>" + ConsultarFotosDespacho(drFila["ruta"].ToString())  + "</p>";
                                //v_detalle = v_detalle + "<p>" + "<center><a href='" + drFila["ruta"].ToString() + "'  class='topopup' target='_blank'><i class='fa fa-search'></i> Ver Tamaño Completo</a></center>" + "</p>";

                                v_detalle = v_detalle + "<p><span class='text-muted'>Solicitud :</span> " + drFila["idSolicitud"].ToString() + " - " + drFila["idSolcol"].ToString() + " - " + drFila["secuencia"].ToString() +
                               " <br/> <span class='text-muted'>Documento :</span> " + drFila["documento"].ToString() +
                               " <br/> <span class='text-muted'>Fecha Registro :</span> " + ((DateTime)drFila["fechaCreacion"]).ToString("dd/MM/yyyy hh:mm") +
                               " <br/> <span class='text-muted'>Observación :</span> " + drFila["comentarios"].ToString().ToUpper();

                                v_detalle = v_detalle + "";
                                //string barra = @"  &nbsp;  &nbsp;";

                                v_cuerpo = v_cuerpo + @"
                                                <div class='card'>
                                                    <div class='card-header' id='heading{X1}'>
                                                        <h6 class='mb-0'>
                                                            <a class='collapsed card-link' data-toggle='collapse' data-target='#collapse{X1}'>
                                                                <i class='fa fa-plus'></i> "
                                                                            + string.Format("<span class='text-muted'>Código :</span>   {0} &nbsp;  &nbsp; &nbsp;  &nbsp;" +
                                                                                            "<span class='text-muted'>Identificación :</span>   {1} &nbsp;  &nbsp; &nbsp;  &nbsp;" +
                                                                                            "<span class='text-muted'>Estado :</span>  <b style='color:{3}';>{2}</b>  &nbsp;  &nbsp; &nbsp;  &nbsp;" +
                                                                                            "<a href='{4} '  class='topopup' target='_blank'><i class='fa fa-search'></i> Ver Imagen</a>"
                                                                                            //"<span class='text-muted'>NOTAS : </span>   {3}  "
                                                                                            , drFila["idSolicitud"].ToString().ToUpper() + "-" + drFila["idSolcol"].ToString() + "-" + drFila["secuencia"].ToString()
                                                                                            , drFila["identificacion"].ToString().ToUpper()
                                                                                            , drFila["EstadoDesc"].ToString()
                                                                                            //, drFila["comentarios"].ToString().ToUpper()
                                                                                            , drFila["Estado"].ToString() != "V" ? drFila["Estado"].ToString() == "A" ? "#F27C00" : "#FF0000" : "#339D28"
                                                                                            , drFila["ruta"].ToString())
                                                                    + @"</a>
                                                        </h6>
                                                    </div>
                                                    <div id = 'collapse{X1}' class='collapse' aria-labelledby='heading{X1}' data-parent='#accordionExample'>
                                                        <div class='card-body'>
                                                            " + v_detalle + @"
                                                        </div>
                                                    </div>
                                                </div>
                                                ";

                                v_cuerpo = v_cuerpo.Replace("{X1}", drFila["idSolicitud"].ToString() + drFila["idSolcol"].ToString() + drFila["secuencia"].ToString());
                            }

                            v_html = v_html + v_cuerpo;
                            v_html = v_html + @" </div>
                        </div>";
                            tab.Append(v_html);
                            this.htmlDespachos.InnerHtml = tab.ToString();
                            xfinderDes.Visible = true;
                            sinresultadoDespacho.Visible = false;
                            UPFotos.Update();
                        }
                        catch (Exception ex)
                        {
                            this.Alerta(string.Format("<b>Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", ex.Message));

                        }
                    }
                }

                if (e.CommandName == "Documentos")
                {
                    xfinderDoc.Visible = false;
                    sinresultado.Visible = false;

                    msjInformativo.InnerHtml = "Confirme que los documentos sean los correctos.";
                    msjInformativo.Attributes["class"] = string.Empty;
                    msjInformativo.Attributes["class"] = "alert alert-warning";
                    this.btnSalvarDocument.Attributes.Remove("disabled");
                    try
                    {
                        var v_argumentos = e.CommandArgument.ToString().Split(',');
                        txtIdSolDoc.Value = v_argumentos[0].ToString();
                        txtIdentificacionDoc.Value = v_argumentos[1].ToString();
                        txtSolColDoc.Value = v_argumentos[2].ToString();
                        UPDOC.Update();
                        ConsultaInfoSolicitudUP();
                    }
                    catch (Exception ex)
                    {
                        sinresultado.Visible = false;
                        msjError.Visible = true;
                        msjError.InnerHtml = "Error!<br/> " + ex.Message;
                    }
                    UPDOC.Update();
                }

                if (e.CommandName == "Rechazar")
                {
                    var item = e.Item;
                    TextBox tcomentario = item.FindControl("tcomentario") as TextBox;

                    if (salir.Visible)
                    {
                        this.Alerta("Acción no permitida, La solicitud ya ha sido procesada");
                        return;
                    }

                    if (string.IsNullOrEmpty(tcomentario.Text))
                    {
                        this.Alerta("Se debe agregar un comentario para rechazo especifico");
                        return;
                    }

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
                        this.Alerta(mensaje);
                    }
                    else
                    {
                        this.Alerta("Colaborador rechazado exitosamente.");
                    }
                    ConsultaInfoSolicitud();
                    UPdetalle.Update();
                }
                UPFotos.Update();
            }
            catch (Exception ex)
            {
                var number = log_csl.save_log<Exception>(ex, "autoriza_booking", "RepeaterBooking_ItemCommand()", DateTime.Now.ToShortDateString(), Page.User.Identity.Name);
                this.Alerta(string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number.ToString()));
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getGifOculta();", true);
            }
        }
        protected void dptipoevento_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dptipoevento.SelectedItem.ToString() == "PERMANENTE")
            {
                txtfecing.Enabled = true;
                txtfecsal.Enabled = true;
                txtfecing.BackColor = System.Drawing.Color.White;
                txtfecsal.BackColor = System.Drawing.Color.White;
            }
            else
            {
                txtfecing.Enabled = false;
                txtfecsal.Enabled = false;
                txtfecing.BackColor = System.Drawing.Color.Silver;
                txtfecsal.BackColor = System.Drawing.Color.Silver;
            }
        }
        private string ConsultarFotosDespacho(string imagen)
        {
            if (string.IsNullOrEmpty(imagen))
            {
                return string.Empty;
            }



            string v_divImagenes = string.Empty;

            v_divImagenes += @"<div class='carousel-item active'>
                                    <img src = '" + imagen + @"' class='d-block w-100' style='height:540px; width:360px; overflow:auto' alt='...'/>
                                    <div class='carousel-caption d-none d-md-block'>
                                        <!-- <h5>Second slide label</h5>
                                        <p>Lorem ipsum dolor sit amet, consectetur adipiscing elit.</p>-->
                                    </div>
                                </div> ";


            StringBuilder tab = new StringBuilder();

            string v_cuerpo = string.Empty;
            v_cuerpo = v_cuerpo + @"<div class='mb-5'>
                                    <div id='carouselExampleCaptions{X1}' class='carousel slide' data-ride='carousel'>
                                    <ol class='carousel-indicators'>
                                        <li data-target='#carouselExampleCaptions{X1}' data-slide-to='0' class='active'></li>
                                      
                                    </ol>
                                    <div class='carousel-inner'>
                                           
                                        " + v_divImagenes + @"
                                    </div>
                                    <a class='carousel-control-prev' href='#carouselExampleCaptions{X1}' role='button' data-slide='prev'>
                                        <span class='carousel-control-prev-icon' aria-hidden='true'></span>
                                        <span class='sr-only'>Previous</span>
                                    </a>
                                    <a class='carousel-control-next' href='#carouselExampleCaptions{X1}' role='button' data-slide='next'>
                                        <span class='carousel-control-next-icon' aria-hidden='true'></span>
                                        <span class='sr-only'>Next</span>
                                    </a>
                                </div>
                            </div>
                            ";
            v_cuerpo = v_cuerpo.Replace("{X1}", imagen.ToString());

            string v_html = string.Empty;
            v_html = v_html + v_cuerpo;

            tab.Append(v_html);
            return tab.ToString();
        }
        protected void btnGrabarVerificacion_Click(object sender, EventArgs e)
        {
            try
            {
                msjErrores.Visible = false;
                if ((!customSwitch.Checked) || (!customSwitch2.Checked) || (!customSwitch3.Checked))
                {
                    if (string.IsNullOrEmpty(txtREchazoFotoMotivo.Text))
                    {
                        //msjErrores.Attributes["class"] = string.Empty;
                        //msjErrores.Attributes["class"] = "msg-critico";
                        msjErrores.InnerText = string.Format("Escriba el motivo de rechazo.");
                        msjErrores.Visible = true;
                        UPFotos.Update();
                        return;
                    }
                }

                string mensajeMostrar = null;
                string mensaje = null;
                string v_estado = string.Empty;
                bool v_cheked = false;
                string v_motivo = string.Empty;
                for (int i = 1; i < 4; i++)
                {
                    if (i == 1) { v_estado = customSwitch.Checked ? "V" : "R"; v_cheked = customSwitch.Disabled; }
                    if (i == 2) { v_estado = customSwitch2.Checked ? "V" : "R"; v_cheked = customSwitch2.Disabled; }
                    if (i == 3) { v_estado = customSwitch3.Checked ? "V" : "R"; v_cheked = customSwitch3.Disabled; }
                    if (v_estado == "R") { v_motivo = txtREchazoFotoMotivo.Text; } else { v_motivo = string.Empty; }

                    if (!v_cheked)
                    {
                        if (!credenciales.ActualizarEstadoFotosRegistroFacial(long.Parse(lblidSolicitud.Value), long.Parse(lblidSolcol.Value), i, v_estado, v_motivo, Page.User.Identity.Name, out mensaje))
                        {
                            mensajeMostrar += string.Format("| Imagen {0}: {1} <br/>", i, mensaje);
                        }
                        else
                        {
                            mensajeMostrar += string.Format("| Imagen {0}: {1} <br/>", i, "Transacción exitosa");
                            //Response.Write("<script language='JavaScript'>var r=alert('Confirmación de pago rechazada exitosamente.');if(r==true){window.close()}else{window.close()};</script>");
                        }
                    }
                }
                this.btnGrabarVerificacion.Attributes["disabled"] = "disabled";
                msjErrores.InnerHtml = string.Format(mensajeMostrar);
                msjErrores.Visible = true;
                ConsultaInfoSolicitud();
                UPFotos.Update();
            }
            catch (Exception ex)
            {
                var number = log_csl.save_log<Exception>(ex, "consultacomprobantedepago", "btsalvar_Click", "1", Request.UserHostAddress);
                this.Alerta(string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number.ToString()));
            }
        }
        protected void cmbPermiso_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataRow permisoSeleccioando;
            try
            {
                string filtro = cmbPermiso.SelectedValue.ToString();
                permisoSeleccioando = dsListaPermisos?.Tables[0].Select(" id_permiso = '" + filtro + "'").FirstOrDefault();
                txtIdPermiso.Value = permisoSeleccioando["COD_PERMISO"].ToString();

                string filtro1 = cmbPermiso1.SelectedValue.ToString();
                permisoSeleccioando = dsListaPermisos?.Tables[0].Select(" id_permiso = '" + filtro1 + "'").FirstOrDefault();
                txtIdPermiso.Value = permisoSeleccioando["COD_PERMISO"].ToString();
            }
            catch
            {
                return;
            }
        }
        //PANTALLA DE DOCUMENTOS
        private void ConsultaInfoSolicitudUP()
        {
            msjError.Visible = false;
            sinresultado.Visible = false;
            numsolicitudempresaUP = txtIdSolDoc.Value;
            idsolcolUP = txtSolColDoc.Value;
            cedula = txtIdentificacionDoc.Value;

            hfCedula.Value = cedula;
            dtDocumentosUP = credenciales.GetDocumentosColaboradorXNumSolicitudPaseProvisional(numsolicitudempresaUP, idsolcolUP);
            tablePaginationDocumentos.DataSource = dtDocumentosUP;
            tablePaginationDocumentos.DataBind();
            dtDocumentosUP.Columns.Add("DocumentoRechazado");
            dtDocumentosUP.Columns.Add("Comentario");
            dtDocumentosUP.Columns.Add("Cedula");

            DataTable dtPadre = Session["dtDocumentosrevisasolicitudcolaborador"] as DataTable;

            if (dtPadre ==null)
            {
                dtPadre = new DataTable();
            }

            if (dtPadre != null)
            {
                var result = from myRow in dtPadre.AsEnumerable()
                             where myRow.Field<string>("Cedula") == cedula
                             select myRow;
                DataTable dt = result.AsDataView().ToTable();
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i <= dt.Rows.Count - 1; i++)
                    {
                        dtDocumentosUP.Rows[i][12] = dt.Rows[i][12];
                        dtDocumentosUP.Rows[i][13] = dt.Rows[i][13];
                    }
                    foreach (RepeaterItem item in tablePaginationDocumentos.Items)
                    {
                        CheckBox chkRevisado = item.FindControl("chkRevisado") as CheckBox;
                        TextBox tcomentario = item.FindControl("tcomentario") as TextBox;
                        chkRevisado.Checked = Convert.ToBoolean(dtDocumentosUP.Rows[item.ItemIndex][12]);
                        if (!chkRevisado.Checked)
                        {
                            tcomentario.Text = "";
                        }
                        else
                        {
                            this.btnSalvarDocument.Attributes["disabled"] = "disabled";
                            tcomentario.Text = dtDocumentosUP.Rows[item.ItemIndex][13].ToString();
                        }
                    }
                    xfinderDoc.Visible = true;
                    sinresultado.Visible = false;
                }
                else
                {
                    sinresultado.Visible = true;
                    xfinderDoc.Visible = false;

                    if (dtDocumentosUP != null)
                    {
                        if (dtDocumentosUP.Rows.Count > 0)
                        {
                            sinresultado.Visible = false;
                            xfinderDoc.Visible = true;

                            if (!botonera.Visible)
                            {
                                this.btnSalvarDocument.Attributes["disabled"] = "disabled";
                                UPDOC.Update();
                                return;
                            }
                        }
                        else
                        {
                            if (!botonera.Visible)
                            {
                                this.btnSalvarDocument.Attributes["disabled"] = "disabled";
                                UPDOC.Update();
                                return;
                            }

                            foreach (RepeaterItem fila in tablePagination.Items)
                            {
                                Label lblcipasColab = fila.FindControl("lblcipas") as Label;
                                CheckBox chkRevisadoColab = fila.FindControl("chkRevisado") as CheckBox;

                                if (txtIdentificacionDoc.Value == lblcipasColab.Text)
                                {
                                    chkRevisadoColab.Checked = true;
                                    //fila.DataItem[2] = "";
                                    UPdetalle.Update();
                                }
                            }
                            ConsultaInfoSolicitud();
                            this.btnSalvarDocument.Attributes["disabled"] = "disabled";
                            UPDOC.Update();
                        }
                    }
                }
            }
            else
            {
                if (!botonera.Visible)
                {
                    this.btnSalvarDocument.Attributes["disabled"] = "disabled";
                    UPDOC.Update();
                    return;
                }
            }
            //xfinder.Visible = true;
            alerta.Attributes["class"] = string.Empty;
            alerta.Attributes["class"] = "msg-info";
            UPDOC.Update();
        }
        protected void btsalvarDoc_Click(object sender, EventArgs e)
        {
            try
            {
                ExportFileUploadUP();
                ConsultaInfoSolicitud();
            }
            catch (Exception ex)
            {
                this.Alerta(ex.Message);
            }
        }
        private void ExportFileUploadUP()
        {
            xmlDocumentosUP = null;
            string directorio = string.Empty;
            foreach (RepeaterItem item in tablePaginationDocumentos.Items)
            {
                CheckBox chkRevisado = item.FindControl("chkRevisado") as CheckBox;
                TextBox tcomentario = item.FindControl("tcomentario") as TextBox;
                //Label lblcipas = item.FindControl("lblcipas") as Label;
                //TextBox txtiddocemp = item.FindControl("txtiddocemp") as TextBox;
                dtDocumentosUP.Rows[item.ItemIndex][12] = chkRevisado.Checked;
                dtDocumentosUP.Rows[item.ItemIndex][13] = tcomentario.Text;
                dtDocumentosUP.Rows[item.ItemIndex][14] = cedula;

                if (chkRevisado.Checked)
                {
                    foreach (RepeaterItem fila in tablePagination.Items)
                    {
                        Label lblcipasColab = fila.FindControl("lblcipas") as Label;
                        CheckBox chkRevisadoColab = fila.FindControl("chkRevisado") as CheckBox;

                        if (txtIdentificacionDoc.Value == lblcipasColab.Text)
                        {
                            chkRevisadoColab.Checked = true;
                            UPdetalle.Update();
                        }
                    }
                }
            }
            dtDocumentosUP.AcceptChanges();

            
            DataTable dtPadre = Session["dtDocumentosrevisasolicitudcolaborador"] as DataTable;
            DataTable dtHijo = new DataTable();
            dtHijo = dtDocumentosUP.Copy().Clone();
            foreach (DataRow drh in dtDocumentosUP.Rows)
            {
                dtHijo.Rows.Add(drh.ItemArray);
            }
            if (dtPadre != null || dtPadre.Rows.Count > 0)
            {
                if (dtPadre.Rows.Count > 0)
                {
                    DataRow[] rows;
                    rows = dtPadre.Select("Cedula='" + cedula + "'");
                    foreach (DataRow r in rows)
                        r.Delete();
                    foreach (DataRow drp in dtPadre.Rows)
                    {
                        dtHijo.Rows.Add(drp.ItemArray);
                    }
                }
            }
            Session["dtDocumentosrevisasolicitudcolaboradordocumentos"] = new DataTable();
            Session["dtDocumentosrevisasolicitudcolaborador"] = dtHijo;
            //Response.Write("<script language='JavaScript'>window.close();</script>");
            //Response.Write("<script type='text/javascript'>$('#myModalDoc').modal('hide');</script>");
            msjInformativo.InnerHtml = "Transacción exitosa";
            msjInformativo.Attributes["class"] = string.Empty;
            msjInformativo.Attributes["class"] = "alert alert-info";
            this.btnSalvarDocument.Attributes["disabled"] = "disabled";
            UPDOC.Update();
        }
        protected void btnCrePermisoOld_Click(object sender, EventArgs e)
        {
            if (xfinderpagado.Visible)
            {
                this.Alerta("No se puede crear el permiso omitiendo el registro facial, ya existen imagenes procesadas.");
                return;
            }

            DataTable dtDocSol = Session["dtDocumentosrevisasolicitudcolaborador"] as DataTable;

            try
            {
                CultureInfo enUS = new CultureInfo("en-US");
                DateTime fechaing;
                if (!DateTime.TryParseExact(txtfecing1.Text, "dd/MM/yyyy", enUS, DateTimeStyles.None, out fechaing))
                {
                    this.Alerta(string.Format("EL FORMATO DE FECHA DEBE SER dia/Mes/Anio{0}\\n", txtfecing1.Text));
                    txtfecing1.Focus();
                    return;
                }
                DateTime fechacad;
                if (!DateTime.TryParseExact(txtfecsal1.Text, "dd/MM/yyyy", enUS, DateTimeStyles.None, out fechacad))
                {
                    this.Alerta(string.Format("EL FORMATO DE FECHA DEBE SER dia/Mes/Anio{0}\\n", txtfecsal1.Text));
                    txtfecsal1.Focus();
                    return;
                }
                TimeSpan tsDias = fechacad - fechaing;
                int diferenciaEnDias = tsDias.Days;
                if (diferenciaEnDias < 0)
                {
                    this.Alerta("La Fecha de Ingreso: " + txtfecing1.Text + "\\nNO deber ser mayor a la\\nFecha de Caducidad: " + txtfecsal1.Text);
                    return;
                }
                foreach (string var in Request.QueryString)
                {
                    numsolicitudempresa = Request.QueryString[var];
                }
                Boolean banderafac = false;
                List<string> listCedulas = new List<string>();

                foreach (RepeaterItem item in tablePagination.Items)
                {
                    CheckBox chkRevisado = item.FindControl("chkRevisado") as CheckBox;
                    Label lblcipas = item.FindControl("lblcipas") as Label;
                    TextBox tcomentario = item.FindControl("tcomentario") as TextBox;
                    if (chkRevisado.Checked == false && !string.IsNullOrEmpty(tcomentario.Text))
                    {
                        this.Alerta("Marque la casilla del Comentario de rechazo. \\n Cedula: " + lblcipas.Text);
                        return;
                    }
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
                DataTable dtColaboradores = credenciales.GetSolicitudColaboradorPermisoProvisional(numsolicitudempresa);
                var resultAprobados = from myRow in dtColaboradores.AsEnumerable()
                                      where !listCedulas.Contains(myRow.Field<string>("CIPAS"))
                                      select myRow;
                DataTable dtAprobados = resultAprobados.AsDataView().ToTable();
                if (dtAprobados.Rows.Count == 0)
                {
                    this.Alerta("Tiene un unico colaborador rechazado.\\n" + "Revise la información antes de continuar con la " + mensajefac + " de la solicitud.\\n Si quiere continuar de click en Rechazar.");
                    return;
                }
                var coloboradores = dtColaboradores;//credenciales.GetSolicitudColaboradorPermisoProvisional(numsolicitudempresa);
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
                String errorvehiculo = string.Empty;
                Int32 registros_actualizados_correcto = 0;
                Int32 registros_actualizados_incorrecto = 0;
                String error_consulta = string.Empty;
                String error = string.Empty;
                var empresa = credenciales.GetEmpresaColaborador(numsolicitudempresa).Rows[0]["RAZONSOCIAL"].ToString();
                if (!string.IsNullOrEmpty(error_consulta))
                {
                    var ex = new ApplicationException(string.Format("Error al usar el Metodo onlyControl.AC_C_EMPRESA():{0}", error_consulta));
                    var number = log_csl.save_log<Exception>(ex, "revisasolicitudpermisoprovisional", "btsalvar_Click", "1", Request.UserHostAddress);
                    this.Alerta(string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, por favor reporte este código de servicio: A00-{0}", number.ToString()));
                    return;
                }
                DataSet dsErrorAC_R_PERSONA_PEATON = new DataSet();
                DataTable dtAC_R_PERSONA_PEATON = new DataTable();
                DataSet dsAC_R_PERSONA_PEATON = new DataSet();
                dtAC_R_PERSONA_PEATON.Columns.Add("CEDULA");
                dtAC_R_PERSONA_PEATON.Columns.Add("APELLIDOS");
                dtAC_R_PERSONA_PEATON.Columns.Add("NOMBRES");
                dtAC_R_PERSONA_PEATON.Columns.Add("EMPRESA");
                dtAC_R_PERSONA_PEATON.Columns.Add("CARGO");
                DataSet dsErrorAC_R_PERSONA = new DataSet();
                DataTable dtAC_R_PERSONA = new DataTable();
                DataSet dsAC_R_PERSONA = new DataSet();
                dtAC_R_PERSONA.Columns.Add("CEDULA");
                dtAC_R_PERSONA.Columns.Add("APELLIDOS");
                dtAC_R_PERSONA.Columns.Add("NOMBRES");
                dtAC_R_PERSONA.Columns.Add("EMPRESA");
                dtAC_R_PERSONA.Columns.Add("AREA");
                dtAC_R_PERSONA.Columns.Add("DPTO");
                dtAC_R_PERSONA.Columns.Add("CARGO");
                dtAC_R_PERSONA.Columns.Add("EXPIRACION");
                dtAC_R_PERSONA.Columns.Add("TIPO_SANGRE");
                dtAC_R_PERSONA.Columns.Add("TIPO_LICENCIA");
                dtAC_R_PERSONA.Columns.Add("FECHA_INGRESO");
                dtAC_R_PERSONA.Columns.Add("FECHA_CADUCIDAD");
                foreach (RepeaterItem item in tablePagination.Items)
                {
                    CheckBox chkRevisado = item.FindControl("chkRevisado") as CheckBox;
                    Label lblcipas = item.FindControl("lblcipas") as Label;
                    if (!chkRevisado.Checked)
                    {
                        var results = from myRow in dtColaboradores.AsEnumerable()
                                      where myRow.Field<string>("CIPAS") == lblcipas.Text
                                      select myRow;
                        DataTable dtresults = results.AsDataView().ToTable();
                        dtAC_R_PERSONA_PEATON.Rows.Add(dtresults.Rows[0]["CIPAS"], dtresults.Rows[0]["APELLIDOS"], dtresults.Rows[0]["NOMBRES"], empresa, dtresults.Rows[0]["CARGO"]);
                    }
                }
                dsAC_R_PERSONA_PEATON.Tables.Add(dtAC_R_PERSONA_PEATON);
                dsErrorAC_R_PERSONA_PEATON = onlyControl.AC_R_PERSONA_PEATON(dsAC_R_PERSONA_PEATON, ref registros_actualizados_correcto, ref registros_actualizados_incorrecto, ref error_consulta) as DataSet;
                if (OnlyControlError(dsErrorAC_R_PERSONA_PEATON, registros_actualizados_incorrecto, error_consulta, "AC_R_PERSONA_PEATON"))
                {
                    return;
                }
                DataTable dtAC_R_PERMISO_TEMPORAL = new DataTable();
                DataSet dsAC_R_PERMISO_TEMPORAL = new DataSet();
                DataSet dsErrorAC_R_PERMISO_TEMPORAL = new DataSet();
                dtAC_R_PERMISO_TEMPORAL.Columns.Add("ID_PERMISO");
                dtAC_R_PERMISO_TEMPORAL.Columns.Add("EMPRESA");
                dtAC_R_PERMISO_TEMPORAL.Columns.Add("AREA");
                dtAC_R_PERMISO_TEMPORAL.Columns.Add("ID_SOLICITANTE");
                dtAC_R_PERMISO_TEMPORAL.Columns.Add("CEDULA_SOLICITANTE");
                dtAC_R_PERMISO_TEMPORAL.Columns.Add("SOLICITANTE");
                dtAC_R_PERMISO_TEMPORAL.Columns.Add("ACTIVIDAD");
                dtAC_R_PERMISO_TEMPORAL.Columns.Add("FECHA_INICIO");
                dtAC_R_PERMISO_TEMPORAL.Columns.Add("FECHA_FIN");
                var replegal = txtusuariosolper.Text;
                var arrglo = replegal.Split(';');
                var cedulareplegal = arrglo[0];
                var nombresreplegal = arrglo[1];
                dtAC_R_PERMISO_TEMPORAL.Rows.Add("", empresa, ddlAreaOnlyControl1.SelectedItem, "", cedulareplegal, nombresreplegal, txtactper.Text, fechaing.ToString("yyyy-MM-dd") + " 00:00", fechacad.ToString("yyyy-MM-dd") + " 23:59");
                dsAC_R_PERMISO_TEMPORAL.Tables.Add(dtAC_R_PERMISO_TEMPORAL);
                dsErrorAC_R_PERMISO_TEMPORAL = onlyControl.AC_R_PERMISO_TEMPORAL(dsAC_R_PERMISO_TEMPORAL, ref registros_actualizados_correcto, ref registros_actualizados_incorrecto, ref error_consulta) as DataSet;
                if (OnlyControlError(dsErrorAC_R_PERMISO_TEMPORAL, registros_actualizados_incorrecto, error_consulta, "AC_R_PERSONA_NOMINA"))
                {
                    return;
                }
                DataTable dtAC_R_ASIGNA_PERSONA = new DataTable();
                DataSet dsAC_R_ASIGNA_PERSONA = new DataSet();
                DataSet dsErrorAC_R_ASIGNA_PERSONA = new DataSet();
                dtAC_R_ASIGNA_PERSONA.Columns.Add("ID_PERMISO");
                dtAC_R_ASIGNA_PERSONA.Columns.Add("CEDULA");
                dtAC_R_ASIGNA_PERSONA.Columns.Add("ID_PERSONA");
                dtAC_R_ASIGNA_PERSONA.Columns.Add("PERSONA");
                dtAC_R_ASIGNA_PERSONA.Columns.Add("USUARIO");
                foreach (RepeaterItem item in tablePagination.Items)
                {
                    CheckBox chkRevisado = item.FindControl("chkRevisado") as CheckBox;
                    Label lblcipas = item.FindControl("lblcipas") as Label;
                    Label lblNombres = item.FindControl("lblNombres") as Label;
                    if (!chkRevisado.Checked)
                    {
                        dtAC_R_ASIGNA_PERSONA.Rows.Add(dsErrorAC_R_PERMISO_TEMPORAL.Tables[0].Rows[0]["ID_PERMISO"], lblcipas.Text, "", lblNombres.Text, codigousuario);
                    }
                }
                dsAC_R_ASIGNA_PERSONA.Tables.Add(dtAC_R_ASIGNA_PERSONA);
                dsErrorAC_R_ASIGNA_PERSONA = onlyControl.AC_R_ASIGNA_PERSONA(dsAC_R_ASIGNA_PERSONA, ref registros_actualizados_correcto, ref registros_actualizados_incorrecto, ref error_consulta) as DataSet;
                if (OnlyControlError(dsErrorAC_R_ASIGNA_PERSONA, registros_actualizados_incorrecto, error_consulta, "AC_R_ASIGNA_PERSONA"))
                {
                    return;
                }
                DataTable dtPermiso = new DataTable();
          
                dtPermiso.Columns.Add(new DataColumn("ID", typeof(String)));
                dtPermiso.Columns.Add(new DataColumn("F_HORARIO", typeof(Int32)));
                dtPermiso.Columns.Add(new DataColumn("F_INGRESO", typeof(DateTime)));
                dtPermiso.Columns.Add(new DataColumn("F_SALIDA", typeof(DateTime)));
                dtPermiso.Columns.Add(new DataColumn("TIPO_CONTROL", typeof(Int32))); //1 LIBRE (NO CADUCA O NO VALIDA FECHA) - 2 CONTROLADO
                dtPermiso.Columns.Add(new DataColumn("COD_PERMISO", typeof(Int32))); // NEW - EJ: 65
                dtPermiso.Columns.Add(new DataColumn("ID_PERMISO", typeof(String))); // NEW - EJ: FA
                dtPermiso.Columns.Add(new DataColumn("AREAS", typeof(String))); // NEW
              
                DataRow dr;
                for (int i = 0; i < dtAC_R_ASIGNA_PERSONA.Rows.Count; i++)
                {
                    var id = onlyControl.GetIdAc_NominaPeaton(dtAC_R_ASIGNA_PERSONA.Rows[i]["CEDULA"].ToString());
                    string setoc = onlyControl.SetPersonaPeatonProvisional(id);

                    dr = dtPermiso.NewRow();
                    dr["ID"] = id;//"999999";
                    dr["F_HORARIO"] = ddlTurnoOnlyControl1.SelectedValue;// 7
                    dr["F_INGRESO"] = fechaing.ToString("yyyy-MM-dd");// Now.Date
                    dr["F_SALIDA"] = fechacad.ToString("yyyy-MM-dd");// DateAdd("d", 1, Now.Date)
                    dr["TIPO_CONTROL"] = 2;
                    dr["COD_PERMISO"] = txtIdPermiso.Value;// 64
                    dr["ID_PERMISO"] = cmbPermiso1.SelectedValue;// "H"
                    dr["AREAS"] = ddlAreaOnlyControl1.SelectedValue;// "916" '"873, 874"
                    dtPermiso.Rows.Add(dr);
                }
                DataSet dsPermiso = new DataSet();
                DataSet dsErrorAC_R_PERMISO = new DataSet();
                dsPermiso.Tables.Add(dtPermiso);
                dsErrorAC_R_PERMISO = onlyControl.AC_R_PERMISO_NEW(dsPermiso, ref registros_actualizados_correcto, ref registros_actualizados_incorrecto, ref error_consulta) as DataSet;
                if (OnlyControlError(dsErrorAC_R_PERMISO, registros_actualizados_incorrecto, error_consulta, "AC_R_PERMISO"))
                {
                    if (dsErrorAC_R_PERMISO.Tables[0].Rows[0]["ERROR"].ToString().Substring(0, 15) != "Error Duplicado")
                    {
                        return;
                    }
                }
                string mensaje = null;
                DataTable dtCola = new DataTable();
                var resultListaApr = from myRow in dtDocSol.AsEnumerable()
                                     where Convert.ToBoolean(myRow.Field<string>("DocumentoRechazado")) == true
                                     select myRow;
                dtCola = resultListaApr.AsDataView().ToTable();
                dtCola.AcceptChanges();
                dtCola.TableName = "Colaboradores";
                StringWriter swC = new StringWriter();
                dtCola.WriteXml(swC);
                xmlDocumentos = swC.ToString();

                StringWriter swAcceso = new StringWriter();
                dsErrorAC_R_ASIGNA_PERSONA.Tables[0].WriteXml(swAcceso);
                String xmlAcceso = swAcceso.ToString();
                if (!credenciales.ApruebaSolicitudColaboradorPaseProvisional_New(
                    numsolicitudempresa,
                    rucempresa,
                    xmlColaboradores,
                    xmlDocumentos,
                    xmlAcceso,
                    Page.User.Identity.Name.ToUpper(),
                    banderafac,
                    fechaing.ToString("yyyy-MM-dd"),
                    fechacad.ToString("yyyy-MM-dd"),
                    out mensaje))
                {
                    this.Alerta(mensaje);
                }
                else
                {
                    //****************************************************
                    //ACTUALIZA ESTADO DE COLABORADOR RECHAZADO
                    //****************************************************
                    foreach (RepeaterItem item in tablePagination.Items)
                    {
                        CheckBox chkRevisado = item.FindControl("chkRevisado") as CheckBox;
                        Label txtNumeroSolicitudColab = item.FindControl("txtNumeroSolicitudColab") as Label;
                        Label txtNumeroSolicitudColColab = item.FindControl("txtNumeroSolicitudColColab") as Label;
                        TextBox tcomentario = item.FindControl("tcomentario") as TextBox;
                        if (chkRevisado.Checked)
                        {
                            credenciales.ActualizarEstadoColaborador(long.Parse(txtNumeroSolicitudColab.Text), long.Parse(txtNumeroSolicitudColColab.Text), "R", tcomentario.Text, Page.User.Identity.Name.ToUpper(), out mensaje);
                        }
                    }

                    //#####################################################################
                    // SE PROCEDE A REALIZAR EL REGISTRO FACIAL EN LA BASE DE ONLY CONTROL
                    //#####################################################################
                    string v_retornaMsj = string.Empty;

                   // RegistroFacialOnlyControl_New(out v_retornaMsj);
                    Response.Write("<script language='JavaScript'>var r=alert('Solicitud " + mensajeok + " exitosamente.  \\n " + v_retornaMsj + "');if(r==true){window.close()}else{window.close()};</script>");
                }
            }
            catch (Exception ex)
            {
                Response.Write("<script language='JavaScript'>alert('" + ex.Message + "');</script>");
            }
        }
    }
}