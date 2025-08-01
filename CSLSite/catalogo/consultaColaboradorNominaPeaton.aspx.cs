﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace CSLSite
{
    public partial class nominapeaton : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //Está conectado y no es postback
            if (Response.IsClientConnected && !IsPostBack)
            {
                var ruc = Session["srucempresacolpro"].ToString();
                sinresultado.Visible = false;
                var u = this.getUserBySesion();
                string error = string.Empty;
                string cedula = null;
                string nombres = null;
                string apellidos = null;
                if (!string.IsNullOrEmpty(Request.QueryString["sidcedula"]))
                {
                    cedula = Request.QueryString["sidcedula"];
                }
                if (!string.IsNullOrEmpty(Request.QueryString["sidnombres"]))
                {
                    nombres = Request.QueryString["sidnombres"];
                }
                if (!string.IsNullOrEmpty(Request.QueryString["sidapellidos"]))
                {
                    apellidos = Request.QueryString["sidapellidos"];
                }
                var t = credenciales.GetConsultaNominaPeaton(ruc, cedula, nombres, apellidos);
                try
                {
                    if (t.Rows.Count > 0)
                    {
                        t.Columns.Add("TIPOSANGRE");
                        t.Columns.Add("DIRECCIONDOM");
                        t.Columns.Add("TELFDOM");
                        t.Columns.Add("EMAIL");
                        t.Columns.Add("LUGARNAC");
                        t.Columns.Add("FECHANAC");
                        t.Columns.Add("CARGO");
                        t.Columns.Add("TIPOLICENCIA");
                        t.Columns.Add("FECHAEXPLICENCIA");
                        for (int i = 0; i < t.Rows.Count; i++)
                        {
                            var detalle = credenciales.GetConsultaDatosColaboradorSCA(ruc, t.Rows[i]["CEDULA"].ToString());
                            if (detalle.Rows.Count == 0)
                            {
                                t.Rows[i]["TIPOSANGRE"] = "";
                                t.Rows[i]["DIRECCIONDOM"] = "";
                                t.Rows[i]["TELFDOM"] = "";
                                t.Rows[i]["EMAIL"] = "";
                                t.Rows[i]["LUGARNAC"] = "";
                                t.Rows[i]["FECHANAC"] = "";
                                t.Rows[i]["CARGO"] = "";
                                t.Rows[i]["TIPOLICENCIA"] = "";
                                t.Rows[i]["FECHAEXPLICENCIA"] = "";
                            }
                            else
                            {
                                t.Rows[i]["TIPOSANGRE"] = detalle.Rows[0]["TIPOSANGRE"];
                                t.Rows[i]["DIRECCIONDOM"] = detalle.Rows[0]["DIRECCIONDOM"];
                                t.Rows[i]["TELFDOM"] = detalle.Rows[0]["TELFDOM"];
                                t.Rows[i]["EMAIL"] = detalle.Rows[0]["EMAIL"];
                                t.Rows[i]["LUGARNAC"] = detalle.Rows[0]["LUGARNAC"];
                                t.Rows[i]["FECHANAC"] = Convert.ToDateTime(detalle.Rows[0]["FECHANAC"]).ToString("dd/MM/yyyy");
                                //t.Rows[i]["CARGO"] = detalle.Rows[0]["CARGO"];
                                t.Rows[i]["TIPOLICENCIA"] = detalle.Rows[0]["TIPOLICENCIA"];
                                t.Rows[i]["FECHAEXPLICENCIA"] = Convert.ToDateTime(detalle.Rows[0]["FECHAEXPLICENCIA"]).ToString("dd/MM/yyyy");
                            }
                        }
                        this.tablePagination.DataSource = t;
                        this.tablePagination.DataBind();
                        xfinder.Visible = true;
                        sinresultado.Visible = false;

                        alerta.Attributes["class"] = string.Empty;
                        alerta.Attributes["class"] = "msg-info";
                        this.alerta.InnerHtml = "Listado de Colaboradores:"; //"Estimado cliente, si la fecha requerida para su reserva no aparece en esta lista. <br />" +
                                       //"Favor comunicarse con nuestra Àrea de Logistica y Almacenamiento <br />" +
                                       //           "Pbx: +593 (04) 6006300, 3901700 ext. 4002, 4021, 4005. <br />" +
                                       //           "Email: <a href='mailto:CGSA-Consolidaciones@cgsa.com.ec'>CGSA-Consolidaciones@cgsa.com.ec</a>;";
                        return;
                    }
                    xfinder.Visible = false;
                    sinresultado.Visible = true;
                }
                catch (Exception ex)
                {
                    xfinder.Visible = false;
                    this.sinresultado.Attributes["class"] = string.Empty;
                    this.sinresultado.Attributes["class"] = "msg-critico";
                    this.sinresultado.InnerText = string.Format("Se produjo un error durante la búsqueda, por favor repórtelo con el siguiente codigo [E00-{0}] ", csl_log.log_csl.save_log<Exception>(ex, "agente", "find_Click", "Sin parametro", u != null ? u.loginname : "userNull"));
                    sinresultado.Visible = true;
                }
            }
        }
      
    }
}