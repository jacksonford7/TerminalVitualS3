﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ClsAutorizaciones;

namespace CSLSite.autorizaciones
{
    public partial class lookup_choferes : System.Web.UI.Page
    {
        public static string v_mensaje = string.Empty;

        public string rucempresa
        {
            get { return (string)Session["rucempresa"]; }
            set { Session["rucempresa"] = value; }
        }

        private usuario u;
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

            if (!Page.IsPostBack)
            {
                string linea_naviera = Session["rucempresa"] as string;
                string ID_EMPRESA = Request.QueryString["ID_EMPRESA"];

                if (Request.QueryString["ID_EMPRESA"] == null || string.IsNullOrEmpty(ID_EMPRESA))
                {
                    xfinder.Visible = false;
                    this.sinresultado.Attributes["class"] = string.Empty;
                    this.sinresultado.Attributes["class"] = "msg-critico";
                    this.sinresultado.InnerText = string.Format("Debe seleccionar la empresa de transporte");
                    sinresultado.Visible = true;
                    return;
                }

                var lookup = Choferes.Buscador_Choferes(txtfinder.Text, ID_EMPRESA, out v_mensaje);
                try
                {
                    if (Response.IsClientConnected)
                    {
                        if (lookup != null && lookup.Count > 0)
                        {
                            //mensajes
                            List<Mensajes> ListMsg = Mensajes.Listar_Mensajes(out v_mensaje);
                            if (!String.IsNullOrEmpty(v_mensaje))
                            {
                                xfinder.Visible = false;
                                this.sinresultado.Attributes["class"] = string.Empty;
                                this.sinresultado.Attributes["class"] = "msg-critico";

                                var cMensaje2 = string.Format("Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible por favor reporte este código de servicio A01-{0}", csl_log.log_csl.save_log<InvalidOperationException>(new InvalidOperationException(v_mensaje), "consulta", "Page_Load", "No se pudo obtener parámetros", "anónimo"));

                                this.sinresultado.InnerText = string.Format("{0}", cMensaje2);
                                sinresultado.Visible = true;
                                return;
                                
                            }
                            //tipos de mensajes
                            var MSG_LICENCIA_EXP = (from Msg2 in ListMsg.Where(Msg2 => Msg2.CAMPO.Equals("EXPIRADO") && Msg2.TIPO == 2) select new { MENSAJE = Msg2.MENSAJE == null ? string.Empty : Msg2.MENSAJE }).FirstOrDefault();
                            var MSG_STATUS = (from Msg3 in ListMsg.Where(Msg3 => Msg3.CAMPO.Equals("STATUS") && Msg3.TIPO == 2) select new { MENSAJE = Msg3.MENSAJE == null ? string.Empty : Msg3.MENSAJE }).FirstOrDefault();
                            var MSG_LICENCIA_SUS = (from Msg4 in ListMsg.Where(Msg4 => Msg4.CAMPO.Equals("SUSPENDIDO") && Msg4.TIPO == 2) select new { MENSAJE = Msg4.MENSAJE == null ? string.Empty : Msg4.MENSAJE }).FirstOrDefault();

                            var Listlookup = (from p in lookup.Where(x => !string.IsNullOrEmpty(x.ID_CHOFER))
                                              select new
                                              {
                                                  ID = (p.ID==0  ? 0: p.ID),
                                                  ID_CHOFER= (p.ID_CHOFER == null ? string.Empty : p.ID_CHOFER),
                                                  NOMBRE_CHOFER = (p.NOMBRE_CHOFER == null ? string.Empty : p.NOMBRE_CHOFER),
                                                  ID_EMPRESA = (p.ID_EMPRESA == null ? string.Empty : p.ID_EMPRESA),
                                                  RAZON_SOCIAL = (p.RAZON_SOCIAL == null ? string.Empty : p.RAZON_SOCIAL),
                                                  MENSAJE = string.Format("{0} - {1} - {2}",
                                                            (!p.STATUS.Equals("OK") ? (MSG_STATUS.MENSAJE == null ? string.Empty : string.Format("{0}/{1}",MSG_STATUS.MENSAJE, p.STATUS)) : string.Empty),
                                                            (p.LICENCIA_EXPIRACION_MENSAJE.Equals("SI") ? (MSG_LICENCIA_EXP.MENSAJE == null ? string.Empty : string.Format("{0}:{1}",MSG_LICENCIA_EXP.MENSAJE,(p.LICENCIA_EXPIRACION==null? string.Empty : p.LICENCIA_EXPIRACION.Value.ToString("dd/MM/yyyy")))) : string.Empty),
                                                            (p.LICENCIA_SUSPENDIDA_MENSAJE.Equals("SI") ? (MSG_LICENCIA_SUS.MENSAJE == null ? string.Empty : string.Format("{0}:{1}",MSG_LICENCIA_SUS.MENSAJE,(p.LICENCIA_SUSPENDIDA==null? string.Empty : p.LICENCIA_SUSPENDIDA.Value.ToString("dd/MM/yyyy")))) : string.Empty)
                                                            )
                                              }).ToList().OrderBy(x => x.ID);

                            this.tablePagination.DataSource = Listlookup.ToList();
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
                    this.sinresultado.InnerText = string.Format("Se produjo un error durante la búsqueda, por favor repórtelo con el siguiente codigo [E00-{0}] ", csl_log.log_csl.save_log<Exception>(ex, "lookup_choferes", "page_load", txtfinder.Text, u != null ? u.loginname : "userNull"));
                    sinresultado.Visible = true;
                }

            }

        }
        protected void find_Click(object sender, EventArgs e)
        {

            string linea_naviera = Session["rucempresa"] as string;
            string ID_EMPRESA = Request.QueryString["ID_EMPRESA"];

            if (Request.QueryString["ID_EMPRESA"] == null || string.IsNullOrEmpty(ID_EMPRESA))
            {
                xfinder.Visible = false;
                this.sinresultado.Attributes["class"] = string.Empty;
                this.sinresultado.Attributes["class"] = "msg-critico";
                this.sinresultado.InnerText = string.Format("Debe seleccionar la empresa de transporte");
                sinresultado.Visible = true;
                return;
            }

            var lookup = Choferes.Buscador_Choferes(txtfinder.Text, ID_EMPRESA, out v_mensaje);
            try
            {
                if (Response.IsClientConnected)
                {
                    if (lookup != null && lookup.Count > 0)
                    {
                        this.tablePagination.DataSource = lookup;
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
                this.sinresultado.InnerText = string.Format("Se produjo un error durante la búsqueda, por favor repórtelo con el siguiente codigo [E00-{0}] ", csl_log.log_csl.save_log<Exception>(ex, "lookup_choferes", "find_Click", txtfinder.Text, u != null ? u.loginname : "userNull"));
                sinresultado.Visible = true;
            }
        }
    }
}