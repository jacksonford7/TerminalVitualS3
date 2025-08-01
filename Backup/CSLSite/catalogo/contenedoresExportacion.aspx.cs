﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CSLSite
{
    public partial class contenedoresExportacion : System.Web.UI.Page
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
                string[] parametros = { "contenido" };
                Page.ReviewQS(parametros, "No se puede buscar un booking sin el tráfico seleccionado.");
                if (Request.QueryString["contenido"] != null)
                {

                    if (Request.QueryString["contenido"] == "FCL")
                    {
                        sinresultado.InnerText = string.Format("No se han encontrado resultados, por favor asegúrese que el nombre del contenedor que está buscando sea de contenedores llenos (FCL){0}", Request.QueryString["trafico"] != null ? " y la línea naviera registrada en el booking sea la que usted seleccionó [" + Request.QueryString["trafico"] + "] y que el contenedor no se encuentre en una solicitud activa" : ".");
                    }
                    else if (Request.QueryString["contenido"] == "MTY")
                    {
                        sinresultado.InnerText = string.Format("No se han encontrado resultados, por favor asegúrese que el booking que está buscando sea de contenedores vacíos (MTY) {0}", Request.QueryString["trafico"] != null ? " y la línea naviera registrada en el booking sea la que usted seleccionó [" + Request.QueryString["trafico"] + "]" : ".");
                    }
                    else
                    {
                        sinresultado.InnerText = "Por favor comuníquese de inmediato con el departamento de planificación en CGSA";
                    }
                }
            }
        }

        protected void find_Click(object sender, EventArgs e)
        {
            string contenido = "";
            string aisv = "";
            if (Request.QueryString["contenido"] != null)
            {
                contenido = Request.QueryString["contenido"].ToString();
            }

            if (Request.QueryString["aisv"] != null)
            {
                aisv = Request.QueryString["aisv"].ToString().Replace("...","");
            }
            var u = this.getUserBySesion();
            List<contenedor> table = CslHelperServicios.contenedoresExportacion(txtfinder.Text.Trim(), contenido, Request.QueryString["trafico"].ToString(), Request.QueryString["correccion"].ToString(), aisv, int.Parse(Request.QueryString["usuario"].ToString()));

          
            try
            {
                if (Response.IsClientConnected)
                {
                    //ta.ClearBeforeFill = true;
                    //ta.Fill(table, "REFERENCIA", txtfinder.Text.Trim(), txtfinder.Text.Trim());
                    if (table.Count > 0)
                    {
                        this.tablePagination.DataSource = table;
                        this.tablePagination.DataBind();
                        xfinder.Visible = true;
                        sinresultado.Visible = false;
                        return;
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
                this.sinresultado.InnerText = string.Format("Se produjo un error durante la búsqueda, por favor repórtelo con el siguiente codigo [E00-{0}] ", csl_log.log_csl.save_log<Exception>(ex, "lineas", "find_Click", txtfinder.Text, u != null ? u.loginname : "userNull"));
                sinresultado.Visible = true;
            }
            finally
            {

            }
        }

        public static string getClass(object valor, object fk, object inactive)
        {
            if (fk == null || fk.ToString().Trim().Length <= 0 || fk.ToString().Trim().ToUpper().Contains("BBK"))
            {
                return "point";
            }
            if (fk.ToString().Trim().ToUpper().Contains("LCL") && inactive != null)
            {
                return "point";
            }
            if (valor == null)
            {
                return "point rowdis";
            }
            int i = 0;
            if (!int.TryParse(valor.ToString(), out i))
            {
                return "point rowdis";
            }
            if (i > 0)
            {
                return "point";
            }
            return "point rowdis";
        }
    }
}