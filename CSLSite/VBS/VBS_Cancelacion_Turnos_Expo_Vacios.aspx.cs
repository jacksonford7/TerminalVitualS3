using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Globalization;
using BillionEntidades;
using N4;
using N4Ws;
using N4Ws.Entidad;
using System.Xml.Linq;
using System.Xml;
using ControlPagos.Importacion;
using Salesforces;
using System.Data;
using System.Net;
using SqlConexion;
using CasManual;

using System.Reflection;
using System.ComponentModel;
using VBSEntidades.Plantilla;
using VBSEntidades;
using VBSEntidades.ClaseEntidades;
using System.Web.Services;
using System.Data.SqlClient;
using OfficeOpenXml;
using System.IO;
using OfficeOpenXml.Style;
using System.Drawing;

namespace CSLSite
{


    public partial class VBS_Cancelacion_Turnos_Expo_Vacios : System.Web.UI.Page
    {

        #region "Clases"

        private Cls_Bil_Sesion objSesion = new Cls_Bil_Sesion();
        usuario ClsUsuario;

        private string cMensajes;


        #endregion



        private string LoginName = string.Empty;

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            ViewStateUserKey = Session.SessionID;

        }


        protected void Page_Init(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Page.SslOn();
            }

            if (!Request.IsAuthenticated)
            {
                Response.Redirect("../login.aspx", false);

                return;
            }

#if !DEBUG
                this.IsAllowAccess();
#endif

            if (!Page.IsPostBack)
            {
                ClsUsuario = Page.Tracker();

            }

        }



        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {


                if (!Page.IsPostBack)
                {
               //     this.Carga_CboBloques();
                    this.Carga_CboTipoCargas();
                }
                else
                {


                }

            }
            catch (Exception ex)
            {
                throw new Exception("Ocrurrio un error en la pagina", ex);
            }
        }

        private void Carga_CboTipoCargas()
        {
            try
            {
                List<VBS_ConsultarLineas> Listado = VBS_ConsultarLineas.ConsultarLineasTodas(out cMensajes);


                this.cboTipoCargaExpo.DataSource = Listado;
                this.cboTipoCargaExpo.DataTextField = "Linea";
                this.cboTipoCargaExpo.DataValueField = "IdLinea";
                this.cboTipoCargaExpo.DataBind();
            }
            catch (Exception ex)
            {
                var t = this.getUserBySesion();
                string Error = string.Format("Ha ocurrido un problema, por favor repórtelo con este código E00-{0}, gracias por entender.", csl_log.log_csl.save_log<Exception>(ex, "consulta", "Carga_CboTipoCargas", "Hubo un error al cargar Tipo de cargas", t.loginname));
               
            }
        }

      
        [WebMethod]
        public static string InactivarHorasExpoVacios(string fechaDesde, string fechaHasta, string cboTipoCargaID)
        {
            try
            {

                string IdLinea =cboTipoCargaID;
                VBS_CabeceraPlantilla objCab = new VBS_CabeceraPlantilla();
       

                DateTime fechaDesde_ = DateTime.Parse(fechaDesde);
                TimeSpan horaDesde = fechaDesde_.TimeOfDay;
                DateTime fechaHasta_ = DateTime.Parse(fechaHasta);
                TimeSpan horaHasta = fechaHasta_.TimeOfDay;
                DateTime fechaSoloDesde = fechaDesde_.Date;
                DateTime fechaSoloHasta = fechaHasta_.Date;




                var editar = objCab.EditarHorasExpoVacios(fechaSoloDesde, horaDesde, fechaSoloHasta, horaHasta, IdLinea);
              
                return "Succes";
            }
             catch (Exception ex)
            {

                throw new Exception("Error", ex);
            }
        }


    }
}