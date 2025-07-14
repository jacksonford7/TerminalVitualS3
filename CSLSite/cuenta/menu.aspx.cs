using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BillionEntidades;

namespace CSLSite.cuenta
{
    public partial class menu : System.Web.UI.Page
    {
        public static usuario sUser = null;

        private string OError;

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                if (Response.IsClientConnected)
                {
                    
                    try
                    {
                        bool tiene = false;
                        sUser = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());
                        string ruc = string.Empty;
                        string pusuario = string.Empty;
                        if (sUser == null)
                        {
                            ruc = "";

                        }
                        else
                        {
                            ruc = sUser.ruc;
                            pusuario = sUser.loginname.Trim();
                        }
                        List<Cls_Pan_Contenedores> Listado = Cls_Pan_Contenedores.Pan_Tienen_Contenedores(ruc, out OError);
                        if (Listado != null)
                        {
                            foreach (var x in Listado)
                            {
                                int p = x.tiene_conteendores;
                                if (p > 0)
                                {
                                    tiene = true;
                                    mpedit.Show();
                                }
                                else {
                                    mpedit.Hide();
                                    tiene = false;
                                }
                            }

                        }
                        else {
                            tiene = false;
                            mpedit.Hide();
                        }

                        //valida si tiene politica de datos
                        if (!tiene)
                        {
                            string cMensajes;
                            List<Cls_Bil_Configuraciones> POLITICA_DATOS = Cls_Bil_Configuraciones.Get_Validacion("POLITICA_DATOS", out cMensajes);
                            if (!String.IsNullOrEmpty(cMensajes))
                            {
                                this.error.Visible = true;
                                this.error.InnerText = string.Format("Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible, en configuraciones.....{0}", cMensajes);
                                return;

                            }

                            bool activa_politica = false;

                            if (POLITICA_DATOS.Count != 0)
                            {
                                activa_politica = true;
                            }

                            if (activa_politica)
                            {
                                List<Cls_Politicas_Datos> Politicas = Cls_Politicas_Datos.Tiene_Politica(pusuario, out OError);
                                if (Politicas != null)
                                {
                                    if (Politicas.Count() != 0)
                                    {
                                        leydatos.Hide();
                                    }
                                    else
                                    {
                                        leydatos.Show();
                                    }
                                }
                            }

                               
                        }
                        //fin

                       

                    }
                    catch (Exception ex)
                    {
                       
                        this.error.Visible = true;
                        this.error.InnerText = string.Format("Se produjo un error durante la consulta de datos, por favor repórtelo con el siguiente código [E00-{0}]", csl_log.log_csl.save_log<Exception>(ex, "usuarios", "Page_Load", "Terminal", sUser.loginname));
                       

                    }
                }
            }
        }

        protected void BtnGrabar_Click(object sender, EventArgs e)
        {

            if (Response.IsClientConnected)
            {
                try
                {

                    sUser = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());
                    //graba log del servicio
                    Cls_Politicas_Datos ClsRegistra = new Cls_Politicas_Datos();
                    ClsRegistra.usuario = sUser.loginname.Trim();
                    ClsRegistra.ruc = sUser.ruc.Trim();
                  

                    string xerror;
                    var nResultado = ClsRegistra.SaveTransaction_Acepta(out xerror);
                    /*fin de nuevo proceso de grabado*/
                    if (!nResultado.HasValue || nResultado.Value <= 0)
                    {
                        leydatos.Hide();

                        this.error.Visible = true;
                        this.error.InnerText = string.Format("Se produjo un error durante el proceso de registro de aceptación de política");
                        return;
                    }

                    leydatos.Hide();

                }
                catch (Exception ex)
                {
                    this.error.Visible = true;
                    this.error.InnerText = string.Format("Se produjo un error durante el proceso de registro de política, por favor repórtelo con el siguiente código [E00-{0}]", csl_log.log_csl.save_log<Exception>(ex, "usuarios", "Page_Load", "Terminal", sUser.loginname));
                  

                }
            }




        }

        protected void btccerrar_Click(object sender, EventArgs e)
        {

            if (Response.IsClientConnected)
            {
                try
                {
                    sUser = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());
                    //graba log del servicio
                    Cls_Politicas_Datos ClsRegistra = new Cls_Politicas_Datos();
                    ClsRegistra.usuario = sUser.loginname.Trim();
                    ClsRegistra.ruc = sUser.ruc.Trim();


                    string xerror;
                    var nResultado = ClsRegistra.SaveTransaction_NoAcepta(out xerror);
                    /*fin de nuevo proceso de grabado*/
                    if (!nResultado.HasValue || nResultado.Value <= 0)
                    {
                        leydatos.Hide();

                        this.error.Visible = true;
                        this.error.InnerText = string.Format("Se produjo un error durante el proceso de registro de no aceptación de política");
                        return;
                    }


                    leydatos.Hide();

                }
                catch (Exception ex)
                {
                    this.error.Visible = true;
                    this.error.InnerText = string.Format("Se produjo un error durante el proceso de registro de política, por favor repórtelo con el siguiente código [E00-{0}]", csl_log.log_csl.save_log<Exception>(ex, "usuarios", "Page_Load", "Terminal", sUser.loginname));


                }
            }




        }

    }
}