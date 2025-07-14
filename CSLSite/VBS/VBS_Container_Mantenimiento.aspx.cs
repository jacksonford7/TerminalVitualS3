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
using VBSEntidades;
using System.Web.UI.HtmlControls;
using VBSEntidades.Plantilla;
using VBSEntidades.ClaseEntidades;
using System.Web.Services;
using Newtonsoft.Json;

namespace CSLSite
{


    public partial class VBS_Container_Mantenimiento : System.Web.UI.Page
    {

        #region "Clases"

        private Cls_Bil_Sesion objSesion = new Cls_Bil_Sesion();
        usuario ClsUsuario;

        private string cMensajes;
        private VBS_CabeceraPlantilla objCab = new VBS_CabeceraPlantilla();

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



        private void Crear_Sesion()
        {
            objSesion.USUARIO_CREA = ClsUsuario.loginname;
            nSesion = objSesion.SaveTransaction(out cMensajes);
            if (!nSesion.HasValue || nSesion.Value <= 0)
            {
                return;
            }
            this.hf_BrowserWindowName.Value = nSesion.ToString().Trim();

            objCab = new VBS_CabeceraPlantilla();


        }

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


                    this.Crear_Sesion();
                }
            }
            catch (Exception ex)
            {
            }
        }


        [WebMethod]

        public static string GetListaContainer()
        {
            VBS_CabeceraPlantilla objCab = new VBS_CabeceraPlantilla();
            var listContainer = objCab.GetListaContainer();

            if (listContainer.Resultado != null)
            {
                string json = JsonConvert.SerializeObject(listContainer.Resultado);
                return json;
            }
            return null;
        }
 
        [WebMethod]
        public static string CrearContainer(VBS_CONTAINER_VIP obj)
        {
            
            VBS_CabeceraPlantilla objCab = new VBS_CabeceraPlantilla();
            var ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());
          
            var revisarSiexiste = objCab.GetContainerValida(obj.container);
            if (revisarSiexiste != null)
            {
                return "Existe"; 
            }
            else
            {
                obj.crea_user = ClsUsuario.loginname;
                var crearNuevoParametro = objCab.GuardarContainer(obj);
            }

            return "OK";

        }
        [WebMethod]
        public static string EditContainer(VBS_CONTAINER_VIP obj)
        {
            VBS_CabeceraPlantilla objCab = new VBS_CabeceraPlantilla();
            var ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());

               obj.crea_user = ClsUsuario.loginname;
            var crearNuevoParametro = objCab.EditarContainer(obj);
            

            return "OK";

        }


    }
}
