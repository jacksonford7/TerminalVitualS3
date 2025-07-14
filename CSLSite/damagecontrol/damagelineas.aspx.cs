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


    public partial class damagelineas : System.Web.UI.Page
    {

        #region "Clases"

        private Cls_Bil_Sesion objSesion = new Cls_Bil_Sesion();
        usuario ClsUsuario;

        private string cMensajes;
        private Damage_ListaLineas objCab = new Damage_ListaLineas();

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

            objCab = new Damage_ListaLineas();


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

        public static string getTablaLineas()
        {
            Damage_ListaLineas objCab = new Damage_ListaLineas();
            var listParametros = objCab.GetListaLineas();

            if (listParametros.Resultado != null)
            {
                string json = JsonConvert.SerializeObject(listParametros.Resultado);
                return json;
            }
            return null;
        }

        [WebMethod]
        public static string EditarLineas(Damage_ListaLineas obj)
        {
            Damage_ListaLineas objCab = new Damage_ListaLineas();

            var ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());
            obj.LIN_USER_UPD = ClsUsuario.loginname;
            obj.LIN_ESTADO = obj.LIN_ESTADO2.Equals("ACTIVO") ? true : false;
            var editParametro = objCab.EditarLinea(obj);

            if (editParametro.Resultado != null)
            {
                return "ok";
            }
            return null;
        }

        [WebMethod]
        public static string CrearLinea(Damage_ListaLineas obj)
        {
            Damage_ListaLineas objCab = new Damage_ListaLineas();
            var revisarSiexiste = objCab.GetLineaValida(obj.LIN_CODIGO);
            if (revisarSiexiste != null)
            {
                return "Existe";
            }
            else
            {
                var ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());
                obj.LIN_USER_CREA = ClsUsuario.loginname;

                var crearNuevoParametro = objCab.GuardarLinea(obj);
            }

            return "OK";

        }
    }
}
