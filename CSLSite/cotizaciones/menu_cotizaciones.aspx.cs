using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Configuration;

namespace CSLSite
{
    public partial class menu_cotizaciones : System.Web.UI.Page
    {
        /// <summary>
        /// Código que se ejecuta al cargar la página
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                
                
            }

        }

        /// <summary>
        /// Código que llama al método para realizar la recuperación de la contraseña
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
  
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            //ViewStateUserKey = Session.SessionID;
        }
        protected void BtnImpoContenedor_Click(object sender, EventArgs e)
        {
           
           Response.Redirect("~/cotizaciones/cotizacion_default01.aspx", false);

        }
        protected void BtnImpoCargaSuelta_Click(object sender, EventArgs e)
        {

            Response.Redirect("~/cotizaciones/cotizacion_default02.aspx", false);

        }

    }
}