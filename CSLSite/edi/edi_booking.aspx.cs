using BillionEntidades;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CSLSite.edi
{
    public partial class edi_booking : System.Web.UI.Page
    {
        #region "Clases"
        usuario ClsUsuario;
        private Cls_Bil_Sesion objSesion = new Cls_Bil_Sesion();
        private EDI_bookingCab objCabecera = new EDI_bookingCab();
        private List<EDI_bookingDet> objDetalle = new List<EDI_bookingDet>();
        private List<EDI_hazard> objSubDetalle = new List<EDI_hazard>();
        #endregion

        #region "Variables"
        private string cMensajes;
        private static Int64? lm = -3;
        private string OError;
        private string Cliente_Ruc = string.Empty;
        private string Cliente_Rol = string.Empty;
        private string Cliente_Direccion = string.Empty;
        private string Cliente_Ciudad = string.Empty;
        private string Fecha = string.Empty;
        private string Contenedores = string.Empty;
        private string Numero_Carga = string.Empty;
        private string FechaPaidThruDay = string.Empty;
        private string CargabexuBlNbr = string.Empty;
        private string TipoServicio = string.Empty;
        private string LoginName = string.Empty;

        /*variables control*/
        private string sap_usuario = string.Empty;
        private string sap_clave = string.Empty;
        private string codCab = string.Empty;
        #endregion

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
        #endregion

        #region "Metodos"
        private void Actualiza_Paneles()
        {
            UPEDIT.Update();
            UPBOTONES.Update();
                       
            //UPCHK.Update();
            UPCHK1.Update();
            UPDET.Update();
            UPSUBDET.Update();

            UPEDIT_ITEM.Update();
            UPEDIT_HAZARD.Update();
        }

        private void Actualiza_Panele_Detalle()
        {
            UPDET.Update();
            UPSUBDET.Update();

            UPEDIT_ITEM.Update();
            UPEDIT_HAZARD.Update();
        }

        private void Limpia_Datos_cliente()
        {
            try

            {
                var ClsUsuario_ = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());
                ClsUsuario = ClsUsuario_;
            }
            catch
            {
                Response.Redirect("../login.aspx", false);
                return;
            }

            this.txtCodigoCab.Text = string.Empty; 
            this.txtNumber.Text = string.Empty;
            this.txtLineOperator.Text = ClsUsuario.ruc;
            this.txtVesselVisit.Text = string.Empty;
            this.txtPortOfLoad.Text = "ECGYE";
            this.txtPortOfDischarge.Text = string.Empty;
            this.txtSecportOfDischarge.Text = string.Empty;
            //this.txtPOD1.Text = string.Empty;
            this.txtShipper.Text = string.Empty;
            this.txtConsigneeId.Text = string.Empty;
            this.txtConsignee.Text = string.Empty;
            this.txtSpecialStow.Text = string.Empty;
            this.txtSpecialStow2.Text = string.Empty;
            this.txtSpecialStow3.Text = string.Empty;
            this.txtNotes.Text = string.Empty;
            this.cmbfreightKind.SelectedValue = "0";
            //this.chkOverideCutOff.Checked = false;
            this.cmbEstado.SelectedValue = "NUE";

            this.txtUn_na_number.Text = string.Empty;
            this.cmbImdgClass.SelectedValue = "0";
            this.cmbHazardNumberType.SelectedValue = "0";

            this.txtQty.Text = "0";
            this.txtEquipmentType.Text = string.Empty;
            //this.txtGrade.Text = string.Empty;
            this.txtCommodity.Text = string.Empty;
            this.txtCommodityDesc.Text = string.Empty;
            this.txtGrossWeightKg.Text = string.Empty;
            this.txtTempRequired.Text = string.Empty;
            this.txtVentilationRequired.Text = string.Empty;
            this.txtCO2Required.Text = string.Empty;
            this.txtO2Required.Text = string.Empty;
            this.txtHumidityRequired.Text = string.Empty;
            this.txtOverLongBack.Text = string.Empty;
            this.txtOverLongFront.Text = string.Empty;
            this.txtOverWideLeft.Text = string.Empty;
            this.txtOverWideRight.Text = string.Empty;
            this.txtOverHeight.Text = string.Empty;
            this.txtRemarks.Text = string.Empty;
            this.ChkisOOG.Checked = false;

            this.GrillaDetalle.DataSource = null;
            this.GrillaDetalle.DataBind();

            this.tablePagination.DataSource = null;
            this.tablePagination.DataBind();
        }

        private void ActivarCampos()
        {
            this.btnVisita.Visible = true;
            this.btnPort1.Visible = true;
            this.btnPort2.Visible = true;
            this.btnPort3.Visible = true;
            this.btnBuscarConsignatario.Visible = true;
            this.txtNumber.Attributes.Remove("disabled");
            //this.txtLineOperator.Attributes.Remove("disabled");
            //this.txtVesselVisit.Attributes.Remove("disabled");
            //this.txtPortOfLoad.Attributes.Remove("disabled");
            this.txtPortOfDischarge.Attributes.Remove("disabled");
            this.txtSecportOfDischarge.Attributes.Remove("disabled");
            //this.txtPOD1.Attributes.Remove("disabled");
            this.txtShipper.Attributes.Remove("disabled");
            this.btnBuscarConsignatario.Attributes.Remove("disabled");
            this.txtConsigneeId.Attributes.Remove("disabled");
            this.txtSpecialStow.Attributes.Remove("disabled");
            this.txtSpecialStow2.Attributes.Remove("disabled");
            this.txtSpecialStow3.Attributes.Remove("disabled");
            this.txtNotes.Attributes.Remove("disabled");
            this.cmbfreightKind.Attributes.Remove("disabled");
            //this.chkOverideCutOff.Disabled = _estado;
            //this.cmbEstado.Attributes.Remove("disabled");
            this.Actualiza_Paneles();
        }

        private void InativarCampos()
        {
            this.btnVisita.Visible = false;
            this.btnPort1.Visible = false;
            this.btnPort2.Visible = false;
            this.btnPort3.Visible = false;
            this.btnBuscarConsignatario.Visible = false;
            this.txtNumber.Attributes["disabled"] = "disabled";
            //this.txtLineOperator.Attributes["disabled"] = "disabled";
            //this.txtVesselVisit.Attributes["disabled"] = "disabled";
            this.txtPortOfLoad.Attributes["disabled"] = "disabled";
            this.txtPortOfDischarge.Attributes["disabled"] = "disabled";
            this.txtSecportOfDischarge.Attributes["disabled"] = "disabled";
            //this.txtPOD1.Attributes["disabled"] = "disabled";
            this.txtShipper.Attributes["disabled"] = "disabled";
            this.btnBuscarConsignatario.Attributes["disabled"] = "disabled";
            this.txtConsigneeId.Attributes["disabled"] = "disabled";
            this.txtSpecialStow.Attributes["disabled"] = "disabled";
            this.txtSpecialStow2.Attributes["disabled"] = "disabled";
            this.txtSpecialStow3.Attributes["disabled"] = "disabled";
            this.txtNotes.Attributes["disabled"] = "disabled";
            this.cmbfreightKind.Attributes["disabled"] = "disabled";
            //this.chkOverideCutOff.Attributes["disabled"] = "disabled";
            this.cmbEstado.Attributes["disabled"] = "disabled";
            this.Actualiza_Paneles();
        }

        private void OcultarLoading(string valor)
        {
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "ocultarloader('" + valor + "');", true);
        }

        public static string securetext(object number)
        {
            if (number == null)
            {
                return string.Empty;
            }
            return QuerySegura.EncryptQueryString(number.ToString());
        }

        private void Mostrar_Mensaje(string Mensaje)
        {
            this.banmsg.Visible = true;
            this.banmsg_det.Visible = true;
            this.msjErrorDetalle.Visible = true;
            this.msjErrorSubDetalle.Visible = true;

            this.banmsg.InnerHtml = Mensaje;
            this.banmsg_det.InnerHtml = Mensaje;
            this.msjErrorDetalle.InnerHtml = Mensaje;
            this.msjErrorSubDetalle.InnerHtml = Mensaje;

            OcultarLoading("1");
            OcultarLoading("2");

            this.Actualiza_Paneles();
        }

        private void Ocultar_Mensaje()
        {
            this.banmsg.InnerText = string.Empty;
            this.banmsg_det.InnerText = string.Empty;
            this.msjErrorDetalle.InnerText = string.Empty;
            this.msjErrorSubDetalle.InnerText = string.Empty;

            this.banmsg.Visible = false;
            this.banmsg_det.Visible = false;
            this.msjErrorDetalle.Visible = false;
            this.msjErrorSubDetalle.Visible = false;

            this.Actualiza_Paneles();
            OcultarLoading("1");
            OcultarLoading("2");
        }

        private void Crear_Sesion()
        {
            objSesion.USUARIO_CREA = ClsUsuario.loginname;
            nSesion = objSesion.SaveTransaction(out cMensajes);
            if (!nSesion.HasValue || nSesion.Value <= 0)
            {
                this.Mostrar_Mensaje(string.Format("<b>Error! No se pudo generar el id de la sesión, por favor comunicarse con el departamento de IT de CGSA... {0}</b>", cMensajes));
                return;
            }
            this.hf_BrowserWindowName.Value = nSesion.ToString().Trim();

            //cabecera de transaccion
            objCabecera = new EDI_bookingCab();
            Session["TransaccionCab" + this.hf_BrowserWindowName.Value] = objCabecera;
        }

        protected string jsarguments(object CNTR_BKNG_BOOKING, object CNTR_ID)
        {
            return string.Format("{0};{1}", CNTR_BKNG_BOOKING != null ? CNTR_BKNG_BOOKING.ToString().Trim() : "0", CNTR_ID != null ? CNTR_ID.ToString().Trim() : "0");
        }

        public void LlenaComboEstado()
        {
            try
            {
                cmbEstado.DataSource = EDI_estado.consultaEstados();
                cmbEstado.DataValueField = "ID";
                cmbEstado.DataTextField = "nombre";
                cmbEstado.DataBind();
                cmbEstado.Enabled = true;
            }
            catch (Exception ex)
            {
                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(LlenaComboEstado), "LlenadoCombo", false, null, null, ex.StackTrace, ex);
                OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));
            }
        }

        public void LlenaComboFK()
        {
            try
            {
                cmbfreightKind.DataSource = EDI_ISO.consultaFK();
                cmbfreightKind.DataValueField = "ID";
                cmbfreightKind.DataTextField = "nombre";
                cmbfreightKind.DataBind();
                //cmbfreightKind.Enabled = true;
            }
            catch (Exception ex)
            {
                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(LlenaComboEstado), "LlenadoCombo", false, null, null, ex.StackTrace, ex);
                OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));
            }
        }

        public void LlenaComboIMDGClass()
        {
            try
            {
                cmbImdgClass.DataSource = EDI_ISO.consultaIMDGClass();
                cmbImdgClass.DataValueField = "ID";
                cmbImdgClass.DataTextField = "nombre";
                cmbImdgClass.DataBind();
                //cmbImdgClass.Enabled = true;
            }
            catch (Exception ex)
            {
                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(LlenaComboEstado), "LlenadoCombo", false, null, null, ex.StackTrace, ex);
                OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));
            }
        }

        public void LlenaComboHazardsNumberType()
        {
            try
            {
                cmbHazardNumberType.DataSource = EDI_ISO.consultaHazardNumType();
                cmbHazardNumberType.DataValueField = "ID";
                cmbHazardNumberType.DataTextField = "nombre";
                cmbHazardNumberType.DataBind();
                //cmbHazardNumberType.Enabled = true;
            }
            catch (Exception ex)
            {
                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(LlenaComboEstado), "LlenadoCombo", false, null, null, ex.StackTrace, ex);
                OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));
            }
        }

        public void LlenaComboLength()
        {
            try
            {
                cmbLength.DataSource = EDI_ISO.consultaLength();
                cmbLength.DataValueField = "ID";
                cmbLength.DataTextField = "nombre";
                cmbLength.DataBind();
                //cmbLength.Enabled = true;
            }
            catch (Exception ex)
            {
                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(LlenaComboEstado), "LlenadoCombo", false, null, null, ex.StackTrace, ex);
                OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));
            }
        }

        public void LlenaComboHeight()
        {
            try
            {
                cmbHeight.DataSource = EDI_ISO.consultaHeight();
                cmbHeight.DataValueField = "ID";
                cmbHeight.DataTextField = "nombre";
                cmbHeight.DataBind();
                //cmbHeight.Enabled = true;
            }
            catch (Exception ex)
            {
                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(LlenaComboEstado), "LlenadoCombo", false, null, null, ex.StackTrace, ex);
                OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));
            }
        }

        public void LlenaComboISOgroup()
        {
            //try
            //{
            //    cmbISOgroup.DataSource = EDI_estado.consultaEstados();
            //    cmbISOgroup.DataValueField = "ID";
            //    cmbISOgroup.DataTextField = "nombre";
            //    cmbISOgroup.DataBind();
            //    //cmbISOgroup.Enabled = true;
            //}
            //catch (Exception ex)
            //{
            //    lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(LlenaComboEstado), "LlenadoCombo", false, null, null, ex.StackTrace, ex);
            //    OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
            //    this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));
            //}
        }

        public void LlenaComboMaterial()
        {
            try
            {
                //cmbMaterial.DataSource = EDI_estado.consultaEstados();
                //cmbMaterial.DataValueField = "ID";
                //cmbMaterial.DataTextField = "nombre";
                //cmbMaterial.DataBind();
                //cmbMaterial.Enabled = true;
            }
            catch (Exception ex)
            {
                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(LlenaComboEstado), "LlenadoCombo", false, null, null, ex.StackTrace, ex);
                OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));
            }
        }

        public void LlenaComboVentilationUnit()
        {
            try
            {
                cmbVentilationUnit.DataSource = EDI_ISO.consultaVentilationUnit();
                cmbVentilationUnit.DataValueField = "ID";
                cmbVentilationUnit.DataTextField = "nombre";
                cmbVentilationUnit.DataBind();
                //cmbtxtVentilationUnit.Enabled = true;
            }
            catch (Exception ex)
            {
                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(LlenaComboEstado), "LlenadoCombo", false, null, null, ex.StackTrace, ex);
                OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));
            }
        }

        private void LlenarDatosCliente(EDI_bookingCab oData)
        {
            this.txtNumber.Text = oData.number;
            this.txtLineOperator.Text = oData.lineOperator;
            this.txtVesselVisit.Text = oData.vesselVisit;
            this.txtPortOfLoad.Text = oData.portOfLoad;
            this.txtPortOfDischarge.Text = oData.portOfDischarge;
            this.txtSecportOfDischarge.Text = oData.secportOfDischarge;
            //this.txtPOD1.Text = oData.POD1;
            this.txtShipper.Text = oData.shipper;
            this.txtConsigneeId.Text = oData.consigneeId;
            this.txtConsignee.Text = oData.consignee;
            //this.chkOverideCutOff.Checked = bool.Parse(oData.overrideCutoff.ToString());
            this.txtSpecialStow.Text = oData.specialStow;
            this.txtSpecialStow2.Text = oData.specialStow2;
            this.txtSpecialStow3.Text = oData.specialStow3;
            this.txtNotes.Text = oData.notes;
            this.cmbfreightKind.SelectedValue = oData.freightKind;
            this.cmbEstado.SelectedValue = oData.estado;

            this.tablePagination.DataSource = oData.EDI_hazard;
            this.tablePagination.DataBind();

            this.GrillaDetalle.DataSource = oData.EDI_bookingDet;
            this.GrillaDetalle.DataBind();

            //this.btnAddHazard.Attributes["disabled"] = "disabled";
            this.btnAddHazard.Attributes.Remove("disabled");
            this.btnAddDetalle.Attributes.Remove("disabled");
            this.InativarCampos();
            this.Actualiza_Paneles();
        }

        private void ValidarControles()
        {
            try
            {
                var ClsUsuario_ = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());
                ClsUsuario = ClsUsuario_;
            }
            catch
            {
                Response.Redirect("../login.aspx", false);
                return;
            }

            Session["TransaccionCab" + this.hf_BrowserWindowName.Value] = null;
            var vNumber = EDI_bookingCab.GetBookingCabPorUser(ClsUsuario.loginname, out OError);

            if (string.IsNullOrEmpty(vNumber))
            {
                this.btnEditar.Visible = false;
                this.btnGrabar.Visible = true;
                this.btnGenerar.Visible = false;
                this.btnAnularDoc.Visible = false;
                this.btnAddDetalle.Attributes["disabled"] = "disabled";
                this.btnAddHazard.Attributes["disabled"] = "disabled";

                this.Limpia_Datos_cliente();
                this.ActivarCampos();
            }
            else
            {
                this.btnEditar.Visible = true;
                this.btnGrabar.Visible = false;
                this.btnGenerar.Visible = false;
                this.btnAnularDoc.Visible = false;
                this.btnAddDetalle.Attributes.Remove("disabled");
                this.btnAddHazard.Attributes.Remove("disabled");

                var oCabecera = EDI_bookingCab.GetBookingCabPorNumber(vNumber, out OError);

                hdf_CodigoCab.Value = string.Empty;
                if (oCabecera != null)
                {
                    hdf_CodigoCab.Value = oCabecera.id.ToString();
                    this.InativarCampos();
                    if (oCabecera.EDI_bookingDet != null)
                    {
                        if (oCabecera.EDI_bookingDet.Count > 0)
                        {
                            this.btnGenerar.Visible = true;
                        }
                    }
                    Session["TransaccionCab" + this.hf_BrowserWindowName.Value] = oCabecera;
                    this.LlenarDatosCliente(oCabecera);
                }
            }
        }

        private void creaTXT(EDI_bookingCab oCabecera, int accion)
        {
            string resultado;
            string filename;
            string path =string.Format(@"c:\temp\EDI_COPARN_{0}_{1}_{2}_{3}.txt", oCabecera.lineOperator,oCabecera.number,oCabecera.id.ToString(),DateTime.Now.ToString("yyyyMMddHHmmss"));

            string subPath = @"c:\temp";
            
            if (!Directory.Exists(subPath))
            {
                Directory.CreateDirectory(subPath);
            }

            if (!File.Exists(path))
            {
                // Create a file to write to.
                using (StreamWriter sw = File.CreateText(path))
                {
                    string verror = string.Empty;
                    Configuraciones.ModuloBase srv = new Configuraciones.ModuloBase();
                    srv.alterClase = "EDI";
                    srv.OnInstanceCreate();
                    srv.Accesorio.ConfiguracionBase = "BILLION";
                    srv.Accesorio.Inicializar(out verror);
                    var _linea = srv.Accesorio.ObtenerConfiguracion(/*oCabecera.lineOperator*/"LINE")?.valor;

                    sw.WriteLine(@"UNA:+,? '");
                    //sw.WriteLine(string.Format("UNB+UNOA:1+SMLU+CONTECON+220624:1435+{0}'", oCabecera.id?.ToString("D9")));
                    sw.WriteLine(string.Format("UNB+UNOA:1+{1}+220624:1435+{0}'", oCabecera.id?.ToString("D9"), _linea));
                    sw.WriteLine(string.Format("UNH+{0}00001+COPARN:D:95B:UN'", oCabecera.id?.ToString("D9")));
                    sw.WriteLine(string.Format("BGM+11+{0}+{1}'", DateTime.Now.ToString("yyyyMMddHmmss"), accion)); // nuevo 9 - actualizar 5 - anular 1
                    sw.WriteLine(string.Format("RFF+BN:{0}'", oCabecera.number));
                    sw.WriteLine(string.Format("TDT+20+{0}+1++{1}:172:20+++{2}:146:11:{3}'", oCabecera.EDI_Vessel.FirstOrDefault().VEPR_VOYAGE,oCabecera.lineOperator, oCabecera.EDI_Vessel.FirstOrDefault().VEPR_VSSL_VESSEL, oCabecera.EDI_Vessel.FirstOrDefault().VEPR_VSSL_NAME));
                    sw.WriteLine("LOC+88+ECGYE:139:6'");
                    sw.WriteLine("LOC+9+ECGYE:139:6'");
                    sw.WriteLine(string.Format("DTM+133:20230519000000:203'", oCabecera.id?.ToString("D9")));
                    sw.WriteLine(string.Format("NAD+CZ+{0}+{1}'", oCabecera.consigneeId,oCabecera.consignee));
                    sw.WriteLine(string.Format("NAD+CA+{0}:172:20'", oCabecera.lineOperator));
                    sw.WriteLine("GID+1'");
                    sw.WriteLine(string.Format("FTX+AAA+++{0}'", oCabecera.EDI_bookingDet.FirstOrDefault().commodityDesc));
                    sw.WriteLine(string.Format("EQD+CN++{0}:102:5+2+2+5'", oCabecera.EDI_bookingDet.FirstOrDefault().equipmentType));
                    sw.WriteLine(string.Format("RFF+BN:{0}'", oCabecera.number));
                    sw.WriteLine(string.Format("EQN+{0}'", oCabecera.EDI_bookingDet.FirstOrDefault().qty));
                    sw.WriteLine("TMD+3++2'");
                    sw.WriteLine(string.Format("DTM+181:{0}000000:203'", DateTime.Now.ToString("yyyyMMdd")));
                    sw.WriteLine(string.Format("LOC+8+{0}:139:6'", oCabecera.portOfDischarge));
                    sw.WriteLine("LOC+98+ECGYE:139:6'");
                    sw.WriteLine(string.Format("LOC+11+{0}:139:6'", oCabecera.secportOfDischarge));
                    sw.WriteLine(string.Format("MEA+AAE+G+KGM:{0}'", oCabecera.EDI_bookingDet.FirstOrDefault().grossWeightKg));
                    if (!string.IsNullOrEmpty(oCabecera.EDI_bookingDet.FirstOrDefault().tempRequired)) { sw.WriteLine(string.Format("TMP+2+{0}:CEL'", oCabecera.EDI_bookingDet.FirstOrDefault().tempRequired)); } 
                    sw.WriteLine(string.Format("FTX+AAA+++{0}'", oCabecera.EDI_bookingDet.FirstOrDefault().commodityDesc));
                    sw.WriteLine("CNT+29:1'");
                    sw.WriteLine(string.Format("UNT+29+{0}00001'", oCabecera.id?.ToString("D9")));
                    sw.WriteLine(string.Format("UNZ+1+{0}'", oCabecera.id?.ToString("D9")));
                }
            }

            // Open the file to read from.
            /*using (StreamReader sr = File.OpenText(path))
            {
                string s = "";
                while ((s = sr.ReadLine()) != null)
                {
                    Console.WriteLine(s);
                }
            }*/

            if (UploadFile(path, File.OpenText(path).BaseStream, out resultado, out filename))
            {
                string msj = string.Empty;

                oCabecera.estado = accion == 3 ? "ANU" : "ENP";
                oCabecera.Save_Update(out msj);

                if (string.IsNullOrEmpty(msj))
                {
                    EDI_bookingArchivo oBookingArchivo = new EDI_bookingArchivo();
                    oBookingArchivo.idBooking = oCabecera.id;
                    oBookingArchivo.path = resultado;
                    oBookingArchivo.filename = filename;
                    oBookingArchivo.estado = true;
                    oBookingArchivo.usuarioCrea = Page.User.Identity.Name;
                    oBookingArchivo.Save_Update(out msj);

                    if (string.IsNullOrEmpty(msj))
                    {
                        this.Alerta("Transacción exitosa");
                    }
                    else
                    {
                        this.Alerta("No se realizó la anulación: " + msj);
                    }
                    this.ValidarControles();
                }
                else
                {
                    this.Alerta("Error al intentar actualizar transacción: " + resultado + ", intente nuevamente por favor.");
                }
            }
            else
            {
                this.Alerta("Error al intentar subir archivo: " + resultado);
            }
        }

        public static bool UploadFile(string fullPath, Stream file, out string fileResume, out string filename)
        {
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; // .NET 4.5

                var ws_file = new GetFile.Service();
                string verror =  string.Empty;
                var br = new BinaryReader(file);
                byte[] inputStream = br.ReadBytes((Int32)file.Length);
                //String dateServer = credenciales.GetDateServer();

                Configuraciones.ModuloBase srv = new Configuraciones.ModuloBase();
                srv.alterClase = "EDI";
                srv.OnInstanceCreate();
                srv.Accesorio.ConfiguracionBase = "BILLION";
                srv.Accesorio.Inicializar( out verror);
                var sc = srv.Accesorio.ObtenerConfiguracion("RUTA_TXT")?.valor;

                String rutaServer = sc;
                filename = string.Format("{0}_{1}{2}", CSLSite.app_start.UIHelper.remove_invalid_path_char(Path.GetFileNameWithoutExtension(fullPath)), DateTime.Now.ToString("ddMMyyyyhhmmssff"), Path.GetExtension(fullPath));

                MemoryStream ms = new MemoryStream(inputStream, 0, inputStream.Length);
                ms.Write(inputStream, 0, inputStream.Length);


                //string rutaServerFinal = rutaServer + filename;

                //using (FileStream filestream = System.IO.File.Create(rutaServerFinal))
                //{
                //    ms.WriteTo(filestream);
                //}

                var s = ws_file.UploadFile(inputStream, rutaServer, filename);
                if (string.IsNullOrEmpty(s))
                {
                    fileResume = "El webservice de subir archivo no esta disponible";
                    return false;
                }
                s = s.ToLower();
                if (!s.Contains("successfully"))
                {
                    fileResume = s;
                    return false;
                }
                fileResume = string.Format("{0}{1}", rutaServer, filename);
                return true;


            }
            catch (Exception ex)
            {
                fileResume = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                filename = string.Empty;
                return false;
            }

        }

        #endregion

        #region "Form"
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            ViewStateUserKey = Session.SessionID;
        }
        protected void Page_Init(object sender, EventArgs e)
        {
            try
            {
                if (!Request.IsAuthenticated)
                {
                    Response.Redirect("../login.aspx", false);
                    return;
                }

                this.banmsg.Visible = IsPostBack;
                this.banmsg_det.Visible = IsPostBack;

                if (!Page.IsPostBack)
                {
                    this.banmsg.InnerText = string.Empty;
                    this.banmsg_det.InnerText = string.Empty;

                }

                ClsUsuario = Page.Tracker();
                Ocultar_Mensaje();
            }
            catch (Exception ex)
            {
                this.Mostrar_Mensaje(string.Format("<b>Error! </b>{0}", ex.Message));
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Server.HtmlEncode(this.txtNumber.Text.Trim());

                if (!Page.IsPostBack)
                {
                    /*secuencial de sesion*/
                    //var ClsUsuario = HttpContext.Current.Session["control"] as Cls_Usuario;
                    var ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());
                    this.Crear_Sesion();

                    ChkisOOG.Checked = false;
                    cmbEstado.SelectedValue = "NUE";

                    LlenaComboEstado();
                    LlenaComboFK();
                    LlenaComboHazardsNumberType();
                    LlenaComboHeight();
                    LlenaComboIMDGClass();
                    LlenaComboISOgroup();
                    LlenaComboLength();
                    LlenaComboMaterial();
                    LlenaComboVentilationUnit();

                    ListItem item = new ListItem();
                    item.Text = "-- Seleccionar --";
                    item.Value = "0";

                    cmbVentilationUnit.Items.Add(item);
                    //cmbMaterial.Items.Add(item);
                    cmbLength.Items.Add(item);
                    //cmbISOgroup.Items.Add(item);
                    cmbImdgClass.Items.Add(item);
                    cmbHeight.Items.Add(item);
                    cmbHazardNumberType.Items.Add(item);
                    cmbfreightKind.Items.Add(item);
                    cmbEstado.SelectedValue = "NUE";
                    this.Limpia_Datos_cliente();
                    this.ValidarControles();
                }
            }
            catch (Exception ex)
            {
                this.Mostrar_Mensaje(string.Format("<b>Error! </b>{0} - {1}", OError, ex.Message));
            }

            this.Actualiza_Paneles();
        }
        #endregion

        #region "Controles"
        protected void btnGrabar_Click(object sender, EventArgs e)
        {
            try
            {
                this.banmsg.Visible = false;
                this.banmsg_det.Visible = false;

                if (Response.IsClientConnected)
                {
                    if (HttpContext.Current.Request.Cookies["token"] == null)
                    {
                        System.Web.Security.FormsAuthentication.SignOut();
                        System.Web.Security.FormsAuthentication.RedirectToLoginPage();
                        Session.Clear();
                        OcultarLoading("1");
                        return;
                    }

                    if (string.IsNullOrEmpty(this.txtNumber.Text))
                    {
                        this.Alerta("Ingrese el Numero.");
                        this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>Por favor ingresar el Numero"));
                        this.txtNumber.Focus();
                        return;
                    }

                    if (string.IsNullOrEmpty(this.txtLineOperator.Text))
                    {
                        this.Alerta("Ingrese la Linea.");
                        this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>Por favor ingresar la linea"));
                        this.txtLineOperator.Focus();
                        return;
                    }

                    if (string.IsNullOrEmpty(this.txtVesselVisit.Text))
                    {
                        this.Alerta("Ingrese la Visita.");
                        this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>Por favor ingresar la visita"));
                        this.txtVesselVisit.Focus();
                        return;
                    }

                    if (string.IsNullOrEmpty(this.txtPortOfLoad.Text))
                    {
                        this.Alerta("Ingrese el Port Of Load.");
                        this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>Por favor ingresar el Port Of Load"));
                        this.txtPortOfLoad.Focus();
                        return;
                    }

                    if (string.IsNullOrEmpty(this.txtPortOfDischarge.Text))
                    {
                        this.Alerta("Ingrese Port Of Discharge.");
                        this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>Por favor ingresar Port Of Discharge"));
                        this.txtPortOfDischarge.Focus();
                        return;
                    }

                    if (string.IsNullOrEmpty(cmbfreightKind.SelectedValue) || (cmbfreightKind.SelectedValue == "0"))
                    {
                        this.Alerta("Seleccione el valor del Freight Kind.");
                        this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>Seleccione el Freight Kind."));
                        cmbfreightKind.Focus();
                        return;
                    }

                    if (string.IsNullOrEmpty(txtConsigneeId.Text) || string.IsNullOrEmpty(txtConsignee.Text))
                    {
                        this.Alerta("Ingrese el Consignatario");
                        this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>Ingrese el Consignatario."));
                        txtConsigneeId.Focus();
                        return;
                    }

                    EDI_bookingCab oCabecera = new EDI_bookingCab();

                    try
                    {
                        var ClsUsuario_ = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());
                        ClsUsuario = ClsUsuario_;
                    }
                    catch
                    {
                        Response.Redirect("../login.aspx", false);
                        return;
                    }

                    if (string.IsNullOrEmpty(txtCodigoCab.Text))
                    {
                        var vNumber = EDI_bookingCab.GetBookingCabPorUser(ClsUsuario.loginname, out OError);

                        if (!string.IsNullOrEmpty(vNumber))
                        {
                            this.btnEditar.Visible = true;
                            this.btnGrabar.Visible = false;
                            this.btnGenerar.Visible = false;
                            this.btnAddDetalle.Attributes.Remove("disabled");
                            this.btnAddHazard.Attributes.Remove("disabled");

                            oCabecera = EDI_bookingCab.GetBookingCabPorNumber(vNumber, out OError);

                            if (oCabecera != null)
                            {
                                //                            Session["TransaccionCab" + this.hf_BrowserWindowName.Value] = oCabecera;

                                if (oCabecera.EDI_bookingDet != null)
                                {
                                    if (oCabecera.EDI_bookingDet.Count > 0)
                                    {
                                        this.btnGenerar.Visible = true;
                                    }
                                }
                            }
                        }
                        else
                        {
                            oCabecera.number = this.txtNumber.Text;
                            oCabecera.estado = "NUE";
                        }
                    }
                    else
                    {
                        //SE HACE UN NUEVO DOCUMENTO DE COPARN REPLICANDO UN BOOKING EXISTENTE
                        oCabecera.docRef = long.Parse(txtCodigoCab.Text);
                        oCabecera.number = this.txtNumber.Text;
                        oCabecera.estado = "DRF";
                    }

                    oCabecera.lineOperator = this.txtLineOperator.Text;
                    oCabecera.vesselVisit = this.txtVesselVisit.Text;
                    oCabecera.portOfLoad = this.txtPortOfLoad.Text;
                    oCabecera.portOfDischarge =this.txtPortOfDischarge.Text;
                    oCabecera.secportOfDischarge = this.txtSecportOfDischarge.Text;
                    oCabecera.consigneeId = this.txtConsigneeId.Text;
                    oCabecera.consignee = this.txtConsignee.Text;
                    oCabecera.shipper = this.txtShipper.Text;
                    oCabecera.specialStow = this.txtSpecialStow.Text;
                    oCabecera.specialStow2 = this.txtSpecialStow2.Text;
                    oCabecera.specialStow3 = this.txtSpecialStow3.Text;
                    oCabecera.notes = this.txtNotes.Text;
                    oCabecera.freightKind  = this.cmbfreightKind.SelectedValue;
                    oCabecera.usuarioCrea = ClsUsuario.loginname;
                    oCabecera.usuarioModifica = ClsUsuario.loginname;

                    oCabecera.id = oCabecera.Save_Update(out OError);
                    this.btnCancelar.Visible = false;

                    if (OError != string.Empty)
                    {
                        this.Alerta(OError);
                        this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>{0}", OError));
                        this.txtNumber.Focus();
                        return;
                    }
                    else
                    {
                        Session["TransaccionCab" + this.hf_BrowserWindowName.Value] = oCabecera;
                        this.Limpia_Datos_cliente();
                        this.ValidarControles();
                        this.Alerta("Transacción exitosa");
                        this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>Datos Generales del Booking Number {0} se han registrado exitosamente.", oCabecera.number));
                        Session["TransaccionDet" + this.hf_BrowserWindowName.Value] = null;
                        this.btnAddDetalle.Attributes.Remove("disabled");
                        this.btnAddHazard.Attributes.Remove("disabled");
                    }
                }
                this.Actualiza_Paneles();
            }
            catch (Exception ex)
            {
                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(btnGrabar_Click), "btnGrabar_Click", false, null, null, ex.StackTrace, ex);
                OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));
                //this.Mostrar_Mensaje(string.Format("<b>Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", ex.Message));
            }
        }

        protected void btnEditar_Click(object sender, EventArgs e)
        {
            this.Ocultar_Mensaje();
            if(string.IsNullOrEmpty(txtCodigoCab.Text))
            {
                this.ValidarControles();
            }
            this.btnEditar.Visible = false;
            this.btnGenerar.Visible = false;
            this.btnGrabar .Visible = true;
            this.btnAddDetalle.Attributes["disabled"] = "disabled";
            this.btnAddHazard.Attributes["disabled"] = "disabled";
            this.txtLineOperator.Focus();
            this.ActivarCampos();

            this.txtNumber.Attributes["disabled"] = "disabled";
            this.btnCancelar.Visible = true;
            this.Actualiza_Paneles();
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Ocultar_Mensaje();
            this.btnCancelar.Visible = false;
            //if (string.IsNullOrEmpty(txtCodigoCab.Text))
            //{
                this.ValidarControles();
            //}
            //else
            //{
            //    this.btnGrabar.Visible = false;
            //    this.btnEditar.Visible = true;
            //    this.InativarCampos();
            //}
            //this.Actualiza_Paneles();
        }

        protected void btnGrabarDetalle_Click(object sender, EventArgs e)
        {
            try
            {
                Ocultar_Mensaje();
                if (Response.IsClientConnected)
                {
                    if (HttpContext.Current.Request.Cookies["token"] == null)
                    {
                        System.Web.Security.FormsAuthentication.SignOut();
                        System.Web.Security.FormsAuthentication.RedirectToLoginPage();
                        Session.Clear();
                        OcultarLoading("1");
                        return;
                    }

                    if (string.IsNullOrEmpty(this.txtQty.Text))
                    {
                        this.Alerta("Ingrese cantidad.");
                        this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>Por favor ingresar la cantidad"));
                        this.txtNumber.Focus();
                        return;
                    }

                    if (int.Parse(this.txtQty.Text) < 1)
                    {
                        this.Alerta("La cantidad debe ser mayor a cero.");
                        this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>Por favor, La cantidad debe ser mayor a cero."));
                        this.txtNumber.Focus();
                        return;
                    }

                    if (string.IsNullOrEmpty(cmbLength.SelectedValue) || (cmbLength.SelectedValue == "0"))
                    {
                        this.Alerta("Seleccione el valor Length.");
                        this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>Seleccione Length."));
                        cmbfreightKind.Focus();
                        return;
                    }

                    if (string.IsNullOrEmpty(cmbHeight.SelectedValue) || (cmbHeight.SelectedValue == "0"))
                    {
                        this.Alerta("Seleccione el valor Height.");
                        this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>Seleccione Height."));
                        cmbfreightKind.Focus();
                        return;
                    }

                    string msj;
                    var c = EDI_ISO.GetISO(txtEquipmentType.Text, out msj);
                    if (c == null)
                    {
                        this.Alerta("No se encontraron datos del ISO: " + txtEquipmentType.Text.Trim());
                        txtEquipmentType.Focus();
                        return;
                    }

                    if (string.IsNullOrEmpty(this.txtISOgroup.Text))
                    {
                        this.Alerta("Ingrese ISO Group.");
                        this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>Por favor ingresar ISO Group"));
                        this.txtNumber.Focus();
                        return;
                    }

                    EDI_bookingDet oDetalle = new EDI_bookingDet();

                    try
                    {
                        var ClsUsuario_ = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());
                        ClsUsuario = ClsUsuario_;
                    }
                    catch
                    {
                        Response.Redirect("../login.aspx", false);
                        return;
                    }
                    
                    objCabecera = Session["TransaccionCab" + this.hf_BrowserWindowName.Value] as EDI_bookingCab;

                    if (!string.IsNullOrEmpty(hdf_CodigoDet.Value))
                    {
                        oDetalle.id = long.Parse(hdf_CodigoDet.Value);
                    }

                    oDetalle.idBooking = objCabecera.id;
                    oDetalle.qty = int.Parse(this.txtQty.Text);
                    oDetalle.equipmentType = this.txtEquipmentType.Text;
                    oDetalle.length = this.cmbLength.SelectedValue;
                    oDetalle.height = this.cmbHeight.SelectedValue;
                    oDetalle.ISOgroup = this.txtISOgroup.Text;
                    oDetalle.grade = string.Empty;
                    oDetalle.material = string.Empty;
                    oDetalle.commodity = this.txtCommodity.Text;
                    oDetalle.commodityDesc = this.txtCommodityDesc.Text;
                    oDetalle.grossWeightKg = this.txtGrossWeightKg.Text;
                    oDetalle.tempRequired = this.txtTempRequired.Text;

                    oDetalle.ventilationRequired = this.txtVentilationRequired.Text;
                    oDetalle.ventilationUnit = this.cmbVentilationUnit.SelectedValue;
                    oDetalle.CO2Required = this.txtCO2Required.Text;
                    oDetalle.O2Required = this.txtO2Required.Text;
                    oDetalle.humidityRequired = this.txtHumidityRequired.Text;
                    oDetalle.overLongBack= this.txtOverLongBack.Text;
                    oDetalle.overLongFront = this.txtOverLongFront.Text;
                    oDetalle.overWideLeft = this.txtOverWideLeft.Text;
                    oDetalle.overWideRight = this.txtOverWideRight.Text;
                    oDetalle.overHeight = this.txtOverHeight.Text;
                    oDetalle.remarks = this.txtRemarks.Text;
                    oDetalle.isOOG = this.ChkisOOG.Checked;
                    oDetalle.usuarioCrea = ClsUsuario.loginname;
                    oDetalle.usuarioModifica = ClsUsuario.loginname;
                    oDetalle.estado = true;
                    msjErrorDetalle.Visible = false;

                    oDetalle.id = oDetalle.Save_Update(out OError);

                    if (OError != string.Empty)
                    {
                        this.Alerta(OError);
                        this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>{0}", OError));
                        this.txtNumber.Focus();
                        return;
                    }
                    else
                    {
                        objCabecera.EDI_bookingDet.Add(oDetalle);
                        Session["TransaccionCab" + this.hf_BrowserWindowName.Value] = objCabecera;
                        this.ValidarControles();
                        this.Alerta("Transacción exitosa");
                        this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>Item del booking number {0}, ha sido agregado exitosamente.", objCabecera.number));
                        Session["TransaccionDet" + this.hf_BrowserWindowName.Value] = null;
                        this.btnGrabarDetalle.Attributes["disabled"] = "disabled";
                        this.hdf_CodigoDet.Value = string.Empty;
                        this.ValidarControles();
                    }

                }
                this.Actualiza_Paneles();
            }
            catch (Exception ex)
            {
                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(btnGrabarDetalle_Click), "btnGrabarDetalle_Click", false, null, null, ex.StackTrace, ex);
                OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));
                //this.Mostrar_Mensaje(string.Format("<b>Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", ex.Message));
            }
        }

        protected void btnGrabarSubDet_Click(object sender, EventArgs e)
        {
            try
            {
                Ocultar_Mensaje();
                if (Response.IsClientConnected)
                {
                    if (HttpContext.Current.Request.Cookies["token"] == null)
                    {
                        System.Web.Security.FormsAuthentication.SignOut();
                        System.Web.Security.FormsAuthentication.RedirectToLoginPage();
                        Session.Clear();
                        OcultarLoading("1");
                        return;
                    }

                    if (string.IsNullOrEmpty(this.txtUn_na_number.Text))
                    {
                        this.Alerta("Ingrese UN/NA Number.");
                        this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>Por favor ingresar UN/NA Number"));
                        this.txtNumber.Focus();
                        return;
                    }

                    if (string.IsNullOrEmpty(cmbHazardNumberType.SelectedValue) || (cmbHazardNumberType.SelectedValue == "0"))
                    {
                        this.Alerta("Seleccione el Hazard Number Type.");
                        this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>Seleccione Hazard Number Type."));
                        cmbfreightKind.Focus();
                        return;
                    }

                    if (string.IsNullOrEmpty(cmbImdgClass.SelectedValue) || (cmbImdgClass.SelectedValue == "0"))
                    {
                        this.Alerta("Seleccione el IMDG Class.");
                        this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>Seleccione IMDG Class."));
                        cmbfreightKind.Focus();
                        return;
                    }

                    EDI_hazard oHazard = new EDI_hazard();

                    try
                    {
                        var ClsUsuario_ = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());
                        ClsUsuario = ClsUsuario_;
                    }
                    catch
                    {
                        Response.Redirect("../login.aspx", false);
                        return;
                    }


                    objCabecera = Session["TransaccionCab" + this.hf_BrowserWindowName.Value] as EDI_bookingCab;

                    if (!string.IsNullOrEmpty(hdf_CodigoSubDet.Value))
                    {
                        oHazard.id = long.Parse(hdf_CodigoSubDet.Value);
                    }

                    oHazard.idbooking = objCabecera.id;
                    oHazard.un_na_number = this.txtUn_na_number.Text;
                    oHazard.imdgClass = this.cmbImdgClass.SelectedValue;
                    oHazard.hazardNumberType = this.cmbHazardNumberType.SelectedValue;
                    oHazard.estado = true;
                    oHazard.usuarioCrea = ClsUsuario.loginname;
                    oHazard.usuarioModifica = ClsUsuario.loginname;
                    msjErrorSubDetalle.Visible = false;

                    oHazard.id = oHazard.Save_Update(out OError);

                    if (OError != string.Empty)
                    {
                        this.Alerta(OError);
                        this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>{0}", OError));
                        this.txtNumber.Focus();
                        return;
                    }
                    else
                    {
                        objCabecera.EDI_hazard.Add(oHazard);
                        Session["TransaccionCab" + this.hf_BrowserWindowName.Value] = objCabecera;
                        this.ValidarControles();
                        this.Alerta("Transacción exitosa");
                        this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>Hazards del Booking Number {0}, ha sido Agregado exitosamente.", objCabecera.number));
                        Session["TransaccionDet" + this.hf_BrowserWindowName.Value] = null;
                        this.btnGrabarSubDet.Attributes["disabled"] = "disabled";
                        this.ValidarControles();
                    }

                }
                this.Actualiza_Paneles();
            }
            catch (Exception ex)
            {
                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(btnGrabarSubDet_Click), "btnGrabarSubDet_Click", false, null, null, ex.StackTrace, ex);
                OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));
                //this.Mostrar_Mensaje(string.Format("<b>Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", ex.Message));
            }
        }

        protected void btnAddDetalle_Click(object sender, EventArgs e)
        {
            this.cmbHeight.SelectedValue = "0";
            this.cmbLength.SelectedValue = "0";
            //this.cmbISOgroup.SelectedValue = "0";
            this.cmbVentilationUnit.SelectedValue = "0";
            this.hdf_CodigoDet.Value = string.Empty;
            this.btnGrabarDetalle.Text = "GRABAR";

            if (GrillaDetalle.Rows.Count > 0)
            {
                this.btnGrabarDetalle.Attributes["disabled"] = "disabled";
                this.Alerta("Solo se permite agregar un 1 Item");
            }
            else
            {
                this.btnGrabarDetalle.Attributes.Remove("disabled");
            }
            
            this.Ocultar_Mensaje();
        }

        protected void btnAddHazard_Click(object sender, EventArgs e)
        {
            this.txtUn_na_number.Text = string.Empty;
            this.cmbHazardNumberType.SelectedValue = "0";
            this.cmbImdgClass.SelectedValue = "0";
            this.hdf_CodigoSubDet.Value = string.Empty;
            this.btnGrabarSubDet.Text = "GRABAR";
            this.btnGrabarSubDet.Attributes.Remove("disabled");
            this.Ocultar_Mensaje();
        }

        protected void btnBuscarISO_Click(object sender, EventArgs e)
        {
            try
            {
                cmbLength.SelectedValue = "0";
                cmbHeight.SelectedValue = "0";
                txtISOgroup.Text = string.Empty;
                msjErrorDetalle.Visible = false;
                if (string.IsNullOrEmpty(this.txtEquipmentType.Text))
                {
                    this.Alerta("Ingrese el tipo de carga.");
                    this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>Por favor ingrese el tipo de carga."));
                    this.txtEquipmentType.Focus();
                    return;
                }
                string msj;
                var c = EDI_ISO.GetISO(txtEquipmentType.Text, out msj);
                if (c == null)
                {
                    this.Alerta("No se encontraron datos del ISO: " + txtEquipmentType.Text.Trim());
                    //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getGifOcultaBuscar();", true);
                    txtEquipmentType.Focus();
                    return;
                }
                cmbLength.SelectedValue = c.length;
                cmbHeight.SelectedValue = c.height;
                txtISOgroup.Text = c.iso_group;
            }
            catch (Exception ex)
            {
                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(btnBuscarISO_Click), "PagoAsignado", false, null, null, ex.StackTrace, ex);
                OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));
            }
        }

        protected void btnBuscarConsignatario_Click(object sender, EventArgs e)
        {
            try
            {
                txtConsignee.Text= string.Empty;
                Ocultar_Mensaje();

                if (string.IsNullOrEmpty(this.txtConsigneeId.Text))
                {
                    this.Alerta("Ingrese el RUC del consignatario");
                    this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>Por favor ingrese el RUC del consignatario"));
                    this.txtConsigneeId.Focus();
                    return;
                }
                string msj;
                var c = EDI_clients_bill_sap.ListClientes(txtConsigneeId.Text).FirstOrDefault();
                if (c == null)
                {
                    this.Alerta("No se encontraron datos del Cliente: " + txtConsigneeId.Text.Trim());
                    //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "getGifOcultaBuscar();", true);
                    txtEquipmentType.Focus();
                    return;
                }
                txtConsignee.Text = c.CLNT_NAME;
            }
            catch (Exception ex)
            {
                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(btnBuscarConsignatario_Click), "BuscarConsignatario", false, null, null, ex.StackTrace, ex);
                OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));
            }
        }

        protected void btnAnular_Click(object sender, EventArgs e)
        {
            try
            {
                string msj = string.Empty;
                EDI_bookingCab oCabecera = EDI_bookingCab.GetBookingCab(long.Parse(hdf_CodigoCab.Value), out msj);
                oCabecera.EDI_Vessel = EDI_Vessel.ListaVessel(txtVesselVisit.Text);

                int accion = 1; //ANULAR

                creaTXT(oCabecera, accion);
            }
            catch (Exception ex)
            {
                //this.Mostrar_Mensaje("No se realizó la anulación: " + ex.Message);
                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(BtnProcesar_Click), "BtnProcesar_Click", false, null, null, ex.StackTrace, ex);
                OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));
            }
        }

        protected void btnAnularItem_Click(object sender, EventArgs e)
        {
            try
            {
                string msj = string.Empty;
                EDI_bookingDet oDet = new EDI_bookingDet();
                oDet.id = long.Parse(hdf_codigoDetalle.Value);
                oDet.estado = false;
                oDet.usuarioModifica = Page.User.Identity.Name;
                oDet.Save_Anula(out msj);

                if (string.IsNullOrEmpty(msj))
                {
                    this.Alerta("Transacción exitosa");
                }
                else
                {
                    this.Alerta("No se realizó la anulación: " + msj);
                }
                this.ValidarControles();
            }
            catch (Exception ex)
            {
                //this.Mostrar_Mensaje("No se realizó la anulación: " + ex.Message);
                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(btnAnularItem_Click), "btnAnularItem_Click", false, null, null, ex.StackTrace, ex);
                OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));
            }
        }

        protected void btnAnularHazard_Click(object sender, EventArgs e)
        {
            try
            {
                string msj = string.Empty;
                EDI_hazard oHazard = new EDI_hazard();
                oHazard.id = long.Parse(hdf_codigoSubDetalle.Value);
                oHazard.estado = false;
                oHazard.usuarioModifica = Page.User.Identity.Name;
                oHazard.Save_Anula(out msj);

                if (string.IsNullOrEmpty(msj))
                {
                    this.Alerta("Transacción exitosa");
                }
                else
                {
                    this.Alerta("No se realizó la anulación: " + msj);
                }
                this.ValidarControles();
            }
            catch(Exception ex)
            {
                //this.Mostrar_Mensaje("No se realizó la anulación: " + ex.Message);
                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(btnAnularHazard_Click), "btnAnularHazard_Click", false, null, null, ex.StackTrace, ex);
                OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));
            }
        }

        protected void BtnProcesar_Click(object sender, EventArgs e)
        {
            try
            { 
                string msj = string.Empty;
                EDI_bookingCab oCabecera = EDI_bookingCab.GetBookingCab(long.Parse(hdf_CodigoCab.Value), out msj);
                oCabecera.EDI_Vessel = EDI_Vessel.ListaVessel(txtVesselVisit.Text);

                int accion = 9; //Nuevo
                if (oCabecera.docRef > 0)
                {
                    accion = 5; //Actualiza
                }

                creaTXT(oCabecera, accion);
            }
            catch (Exception ex)
            {
                //this.Mostrar_Mensaje("No se realizó la anulación: " + ex.Message);
                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(BtnProcesar_Click), "BtnProcesar_Click", false, null, null, ex.StackTrace, ex);
                OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue? lm : -2));
                this.Mostrar_Mensaje(string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));
            }
        }

        protected void txtCodigoCab_TextChanged(object sender, EventArgs e)
        {
            try
            {
                var oCabecera = EDI_bookingCab.GetBookingCab(long.Parse(txtCodigoCab.Text), out OError);

                hdf_CodigoCab.Value = string.Empty;
                if (oCabecera != null)
                {
                    hdf_CodigoCab.Value = oCabecera.id.ToString();
                    this.InativarCampos();

                    if (oCabecera.estado == "ENP" || oCabecera.estado == "PRC")
                    {
                        this.btnAnularDoc.Visible = true;
                    }
                    Session["TransaccionCab" + this.hf_BrowserWindowName.Value] = oCabecera;
                    this.LlenarDatosCliente(oCabecera);

                    btnEditar.Visible = true;
                    btnGrabar.Visible = false;
                    btnGenerar.Visible = false;
                    btnAddDetalle.Attributes["disabled"] = "disabled";
                    btnAddHazard.Attributes["disabled"] = "disabled";

                    GrillaDetalle.Attributes["disabled"] = "disabled";
                    tablePagination.Attributes["disabled"] = "disabled";

                    this.Actualiza_Paneles();
                }
            }
            catch
            {

            }
        }
        #endregion

        #region"Eventos del Grid de Detalle"
        protected void GrillaDetalle_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Editar")
            {
                this.btnGrabarDetalle.Text = "ACTUALIZAR";
                this.btnGrabarDetalle.Attributes.Remove("disabled");
                long v_ID = long.Parse(e.CommandArgument.ToString());
                if (v_ID <= 0) { return; }
                objCabecera = Session["TransaccionCab" + this.hf_BrowserWindowName.Value] as EDI_bookingCab;
                if (objCabecera == null) { return; }

                var oDetalle = objCabecera.EDI_bookingDet.Where(a => a.id == v_ID).FirstOrDefault();
                Session["TransaccionDet" + this.hf_BrowserWindowName.Value] = oDetalle;

                this.hdf_CodigoDet.Value = oDetalle.id.ToString();
                this.txtQty.Text = oDetalle.qty.ToString();
                this.txtEquipmentType.Text = oDetalle.equipmentType;
                this.cmbLength.SelectedValue = oDetalle.length;
                this.cmbHeight.SelectedValue = oDetalle.height;
                this.txtISOgroup.Text = oDetalle.ISOgroup;
                this.txtCommodity.Text = oDetalle.commodity;
                this.txtCommodityDesc.Text = oDetalle.commodityDesc;
                this.txtGrossWeightKg.Text = oDetalle.grossWeightKg;
                this.txtTempRequired.Text = oDetalle.tempRequired;

                this.txtVentilationRequired.Text = oDetalle.ventilationRequired;
                this.cmbVentilationUnit.SelectedValue = oDetalle.ventilationUnit;
                this.txtCO2Required.Text = oDetalle.CO2Required;
                this.txtO2Required.Text = oDetalle.O2Required;
                this.txtHumidityRequired.Text = oDetalle.humidityRequired;
                this.txtOverLongBack.Text = oDetalle.overLongBack;
                this.txtOverLongFront.Text = oDetalle.overLongFront;
                this.txtOverWideLeft.Text = oDetalle.overWideLeft;
                this.txtOverWideRight.Text = oDetalle.overWideRight;
                this.txtOverHeight.Text = oDetalle.overHeight;
                this.txtRemarks.Text = oDetalle.remarks;
                this.ChkisOOG.Checked = bool.Parse(oDetalle.isOOG.ToString());
                
                msjErrorDetalle.Visible = false;

                this.Actualiza_Paneles();
            }

            if (e.CommandName == "Anular")
            {
                long v_ID = long.Parse(e.CommandArgument.ToString());
                this.hdf_codigoDetalle.Value = v_ID.ToString();
                this.UPMENSAJE2.Update();
            }
        }

        protected void GrillaDetalle_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Button btnEditar = (Button)e.Row.FindControl("btnEditarDetalle");
                Button btnAnular = (Button)e.Row.FindControl("btnAnularDetalle");

                if (!string.IsNullOrEmpty(txtCodigoCab.Text))
                {
                    btnEditar.Enabled = false;
                    btnAnular.Enabled = false;
                }

                this.Actualiza_Panele_Detalle();
            }
        }

        protected void tablePagination_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Editar")
            {
                this.btnGrabarSubDet.Text = "ACTUALIZAR";
                this.btnGrabarSubDet.Attributes.Remove("disabled");
                long v_ID = long.Parse(e.CommandArgument.ToString());
                if (v_ID <= 0) { return; }
                objCabecera = Session["TransaccionCab" + this.hf_BrowserWindowName.Value] as EDI_bookingCab;
                if (objCabecera == null) { return; }

                var oDetalle = objCabecera.EDI_hazard.Where(a => a.id == v_ID).FirstOrDefault();
                Session["TransaccionSubDet" + this.hf_BrowserWindowName.Value] = oDetalle;

                this.hdf_CodigoSubDet.Value = oDetalle.id.ToString();
                this.txtUn_na_number.Text = oDetalle.un_na_number.ToString();
                this.cmbImdgClass.SelectedValue = oDetalle.imdgClass;
                this.cmbHazardNumberType.SelectedValue = oDetalle.hazardNumberType;

                msjErrorSubDetalle.Visible = false;

                this.Actualiza_Paneles();
            }

            if (e.CommandName == "Anular")
            {
                long v_ID = long.Parse(e.CommandArgument.ToString());
                this.hdf_codigoSubDetalle.Value = v_ID.ToString();
                this.UPMENSAJE1.Update();
            }
        }

        protected void tablePagination_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Button btnEditar = (Button)e.Row.FindControl("btnEditarSubDetalle");
                Button btnAnular = (Button)e.Row.FindControl("btnAnularSubDetalle");

                if (!string.IsNullOrEmpty(txtCodigoCab.Text))
                {
                    btnEditar.Enabled = false;
                    btnAnular.Enabled = false;
                }
               
                this.Actualiza_Panele_Detalle();
            }
        }
        #endregion

       
        
    }
}