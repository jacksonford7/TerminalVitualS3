using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Aduanas;
using N4;
using CSLSite;

namespace CSLSite
{
    public partial class cargaspendientesimpo : System.Web.UI.Page
    {
        usuario ClsUsuario;


        private void Carga_Pendientes(string Usuario, string Ruc, string idAgente)
        {
            try
            {
                var p = new Aduana.Importacion.ecu_validacion_cntr();
                var ListaEcuapass = p.CargaPorRucImpo(Usuario.Trim(), Ruc.Trim(), idAgente.Trim());
                if (ListaEcuapass.Exitoso)
                {
                    var LinqQuery = from Tbl in ListaEcuapass.Resultado.Where(Tbl => !String.IsNullOrEmpty(Tbl.mrn))
                                              select new { MRN = Tbl.mrn,
                                                  MSN = Tbl.msn,
                                                  HSN = Tbl.hsn,
                                                  CARGA = (Tbl.mrn.Trim() + "-" + Tbl.msn.Trim() + "-" + Tbl.hsn.Trim()),
                                                  TIPO = (Tbl.tipo == "C") ? "CONTENEDOR" : (Tbl.tipo == "S") ? "CARGA SUELTA (CFS)" : "BREAK BULK",
                                                  REFERENCIA = (Tbl.referencia == null) ? "" : Tbl.referencia.Trim() ,
                                                  IMPORTADOR = (Tbl.importador_id == null) ? "(ERROR) SIN IMPORTADOR" : Tbl.importador_id.Trim() + " - " + Tbl.importador.Trim(),
                                                  ID_IMPORTADOR = (Tbl.importador_id == null) ? "(ERROR)" : Tbl.importador_id.Trim() ,
                                                  DESC_IMPORTADOR = (Tbl.importador == null) ? "(ERROR)" : Tbl.importador.Trim(),
                                                  DECLARACION = (Tbl.declaracion == null) ? "" : Tbl.declaracion.Trim(),
                                                  BL = (Tbl.documento_bl == null) ? "" : Tbl.documento_bl.Trim(),
                                                  TOTAL_PARTIDAS = (Tbl.total_partida == null) ? "" : Tbl.total_partida.ToString(),
                                                  LLAVE = string.Format("{0}+{1}+{2}+{3}+{4}+{5}+{6}",Tbl.mrn.Trim() ,Tbl.msn.Trim(),Tbl.hsn.Trim(),((Tbl.importador_id == null) ? "(ERROR)" : Tbl.importador_id.Trim()) , ((Tbl.agente_id == null) ? "" : Tbl.agente_id.Trim()), ((Tbl.importador_id == null) ? "(ERROR)" : Tbl.importador_id.Trim()),((Tbl.importador == null) ? "(ERROR)" : Tbl.importador.Trim())),
                                                  ID_AGENTE = (Tbl.agente_id == null) ? "" : Tbl.agente_id.Trim(),
                                              };
                    if (LinqQuery != null && LinqQuery.Count() > 0)
                    {
                        tablePagination.DataSource = LinqQuery;
                        tablePagination.DataBind();
                    }
                    else
                    {
                        tablePagination.DataSource = null;
                        tablePagination.DataBind();
                    }

                }
               

              
            }
            catch (Exception ex)
            {
                //sg = string.Format("Ha ocurrido la excepción #{0}, por favor intente más tarde ", csl_log.log_csl.save_log<Exception>(ex, "group", "Carga_ListadoNiveles", "Hubo un error al cargar niveles de aprobación", user2 != null ? user2.loginname : "Nologin"));
               // this.Alerta(sg);
                return;
            }

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

            ClsUsuario = Page.Tracker();
            if (ClsUsuario != null)
            {

                this.Txtcliente.Text = string.Format("{0} {1}", ClsUsuario.nombres, ClsUsuario.apellidos);
                this.Txtruc.Text = string.Format("{0}", ClsUsuario.ruc);
                //this.TxtTipo.Text = string.Format("{0}", "Contado");
            }

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    string IdAgente = string.Empty;
                    ClsUsuario = Page.Tracker();
                    if (ClsUsuario != null)
                    {

                        var Agente = N4.Entidades.Agente.ObtenerAgentePorRuc(ClsUsuario.loginname, ClsUsuario.ruc);
                        if (Agente.Exitoso)
                        {
                            var ListaAgente = Agente.Resultado;
                            if (ListaAgente != null)
                            {
                                IdAgente = ListaAgente.codigo;
                            }
                        }
                        

                        this.Carga_Pendientes(ClsUsuario.loginname, ClsUsuario.ruc, IdAgente);
                    }
                      
                }
              
            }
            catch (Exception ex)
            {

             }
        }

        public static string securetext(object number)
        {
            if (number == null)
            {
                return string.Empty;
            }
            return QuerySegura.EncryptQueryString(number.ToString());
        }

    }
}