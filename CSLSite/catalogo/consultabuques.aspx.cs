﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ControlOPC.Entidades;
using System.Data;

namespace CSLSite
{
    public partial class consultabuques : System.Web.UI.Page
    {
      
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            ViewStateUserKey = Session.SessionID;

        }
        protected void Page_Load(object sender, EventArgs e)
        {
            //Está conectado y no es postback
            if (Response.IsClientConnected && !IsPostBack)
            {
                sinresultado.Visible = false;
            }

            try
            {
               
                List<Vessel> Lista = Vessel.ListaVessel(null);
                DataTable dt = App_Extension.LINQToDataTable(Lista);
                if (dt != null && dt.Rows.Count > 0)
                {
                    this.tablePagination.DataSource = dt;
                    this.tablePagination.DataBind();
                    xfinder.Visible = true;
                    sinresultado.Visible = false;

                    alerta.Attributes["class"] = string.Empty;
                    alerta.Attributes["class"] = "msg-info";

                    return;
                }

                alerta.Attributes["class"] = string.Empty;
                alerta.Attributes["class"] = "msg-info";
                xfinder.Visible = false;
                sinresultado.Visible = true;

            }
            catch (Exception ex)
            {
               
            }

        }
        protected void find_Click(object sender, EventArgs e)
        {
            if (Response.IsClientConnected)
            {
                var u = new usuario();
#if !DEBUG

           
             u = this.getUserBySesion();
#endif



                var t = aso_transportistas.GetCiaTransAso(txtname.Text.Trim());
              

                try
                {
                    if ( t!=null &&  t.Rows.Count > 0)
                    {
                        this.tablePagination.DataSource = t;
                        this.tablePagination.DataBind();
                        xfinder.Visible = true;
                        sinresultado.Visible = false;

                        alerta.Attributes["class"] = string.Empty;
                        alerta.Attributes["class"] = "msg-info";

                        /*
                        this.alerta.InnerHtml = "Estimado cliente, si el número de booking que esta buscando no aparece en esta lista. <br />" +
                                       "Favor comunicarse con el Area de Planificacion de CGSA <br />" +
                                                  "Pbx: +593 (04) 6006300, 3901700 ext. 4039, 4040, 4060. <br />" +
                                                  "Email: <a href='mailto:AfterDock@cgsa.com.ec'>AfterDock@cgsa.com.ec</a>;";
                        */

                        return;
                    }
                    alerta.Attributes["class"] = string.Empty;
                    alerta.Attributes["class"] = "msg-info";
                    xfinder.Visible = false;
                    sinresultado.Visible = true;
                }
                catch (Exception ex)
                {
                    xfinder.Visible = false;
                    this.sinresultado.Attributes["class"] = string.Empty;
                    this.sinresultado.Attributes["class"] = "msg-critico";
                    this.sinresultado.InnerText = string.Format("Se produjo un error durante la búsqueda, por favor repórtelo con el siguiente codigo [E00-{0}] ", csl_log.log_csl.save_log<Exception>(ex, "agente", "find_Click", txtname.Text, u != null ? u.loginname : "userNull"));
                    sinresultado.Visible = true;
                }
            }
        }

      
    }
}