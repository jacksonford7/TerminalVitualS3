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

using System.Data;
using System.Web.Script.Services;
using System.IO;
using System.Drawing.Imaging;
using System.Drawing;
using System.Drawing.Text;
using System.Collections;
using PasePuerta;
using CSLSite;

namespace CSLSite
{
  

    public partial class emisionpasecontenedor : System.Web.UI.Page
    {

        #region "Clases"
        private static Int64? lm = -3;
        private string OError;
        private Cls_Bil_Sesion objSesion = new Cls_Bil_Sesion();

        usuario ClsUsuario;
        private Cls_Bil_PasePuertaContenedor_Cabecera objPaseContenedor = new Cls_Bil_PasePuertaContenedor_Cabecera();
        private Cls_Bil_PasePuertaContenedor_Detalle objDetallePaseContenedor = new Cls_Bil_PasePuertaContenedor_Detalle();
        private List<Cls_Bil_Turnos> List_Turnos { set; get; }

        #endregion

        #region "Variables"

        private string numero_carga = string.Empty;
        private string cMensajes;

        //private Int64 Gkey = 0;
        private string Cliente_Ruc = string.Empty;
        private string Cliente_Rol = string.Empty;
        private string Cliente_Direccion = string.Empty;
        private string Cliente_Ciudad = string.Empty;
        private string Fecha = string.Empty;
        private string Contenedores = string.Empty;
        private string Numero_Carga = string.Empty;
        private string FechaPaidThruDay = string.Empty;
        private string CargabexuBlNbr = string.Empty;
        //private int Fila = 1;
        private string TipoServicio = string.Empty;

       
        private DateTime FechaFacturaHasta;
        private string HoraHasta = "00:00";
        private string LoginName = string.Empty;
        private string Tipo_Contenedor = string.Empty;
        private DateTime FechaActualSalida;
        private DateTime FechaMenosHoras;
        private TimeSpan HorasDiferencia;
        private int TotalHoras = 0;
        private DateTime FechaInicial;
        private DateTime FechaFinal;
        private string ContenedorSelec = string.Empty;
        private string EmpresaSelect = string.Empty;
        private string ChoferSelect = string.Empty;
        private string PlacaSelect = string.Empty;
        private string TurnoSelect = string.Empty;
        private DateTime FechaTurnoInicio;
        private DateTime FechaTurnoFinal;
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

        #region "Metodos Web Services"


        [System.Web.Services.WebMethod]
        public static string[] GetEmpresas(string prefix)
        {
            List<string> StringResultado = new List<string>();

            try
            {
                var ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());
                var Transportista = N4.Entidades.CompaniaTransporte.ObtenerCompanias(ClsUsuario.loginname, prefix);
                if (Transportista.Exitoso)
                {
                    var LinqQuery = (from Tbl in Transportista.Resultado.Where(Tbl => Tbl.ruc != null)
                                        select new
                                        {
                                            EMPRESA = string.Format("{0} - {1}", Tbl.ruc.Trim(), Tbl.razon_social.Trim()),
                                            RUC = Tbl.ruc.Trim(),
                                            NOMBRE = Tbl.razon_social.Trim(),
                                            ID = Tbl.ruc.Trim()
                                        });
                    foreach (var Det in LinqQuery)
                    {
                        StringResultado.Add(string.Format("{0}+{1}", Det.ID, Det.EMPRESA));
                    }

                }
            }
            catch (Exception ex)
            {
                StringResultado.Add(string.Format("{0}+{1}", ex.Message, ex.Message));
            }
            return StringResultado.ToArray();
        }

        [System.Web.Services.WebMethod]
        public static string[] GetChofer(string idempresa, string prefix)
        {
            List<string> StringResultado = new List<string>();

            try
            {
                var ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());
                var Chofer = N4.Entidades.Chofer.ObtenerChoferes(ClsUsuario.loginname, String.Empty, idempresa);
                if (Chofer.Exitoso)
                {
                    var LinqQuery = (from Tbl in Chofer.Resultado.Where(Tbl => Tbl.numero != null)
                                     select new
                                     {
                                         EMPRESA = string.Format("{0} - {1}", Tbl.numero.Trim(), Tbl.nombres.Trim()),
                                         RUC = Tbl.numero.Trim(),
                                         NOMBRE = Tbl.nombres.Trim(),
                                         ID = Tbl.numero.Trim()
                                     });
                    foreach (var Det in LinqQuery)
                    {
                        StringResultado.Add(string.Format("{0}+{1}", Det.ID, Det.EMPRESA));
                    }
      
                }
            }
            catch (Exception ex)
            {
                StringResultado.Add(string.Format("{0}+{1}", ex.Message, ex.Message));
            }
            return StringResultado.ToArray();
        }

        [System.Web.Services.WebMethod]
        public static string[] GetPlaca(string idempresa, string prefix)
        {
            List<string> StringResultado = new List<string>();

            try
            {
                var ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());
                var Camion = N4.Entidades.Camion.ObtenerCamiones(ClsUsuario.loginname, prefix, idempresa);
                if (Camion.Exitoso)
                {
                    var LinqQuery = (from Tbl in Camion.Resultado.Where(Tbl => Tbl.numero != null)
                                     select new
                                     {
                                         EMPRESA = string.Format("{0}", Tbl.numero.Trim()),
                                         RUC = Tbl.numero.Trim(),
                                         NOMBRE = Tbl.numero.Trim(),
                                         ID = Tbl.numero.Trim()
                                     });
                    foreach (var Det in LinqQuery)
                    {
                        StringResultado.Add(string.Format("{0}+{1}", Det.ID, Det.EMPRESA));
                    }

                }
            }
            catch (Exception ex)
            {
                StringResultado.Add(string.Format("{0}+{1}", ex.Message, ex.Message));
            }
            return StringResultado.ToArray();
        }

        #endregion


        #region "Metodos"

        private void Actualiza_Paneles()
        {
            UPCARGA.Update();
            UPDATOSCLIENTE.Update();
            UPDESADUANAMIENTO.Update();
            UPBOTONES.Update();
            UPCONTENEDOR.Update();
            UPFECHASALIDA.Update();
            UPTURNO.Update();
            UPCAS.Update();

        }


        private void Limpia_Campos()
        {
            this.TXTMRN.Text = string.Empty;
            this.TXTMSN.Text = string.Empty;
            if (string.IsNullOrEmpty(this.TXTHSN.Text))
            {
                this.TXTHSN.Text = string.Format("{0}", "0000");
            }
            this.Txtempresa.Text = string.Empty;
            this.TxtChofer.Text = string.Empty;
            this.TxtPlaca.Text = string.Empty;
            this.TxtFechaHasta.Text = string.Empty;
            this.TxtContenedorSeleccionado.Text = string.Empty;
            this.TxtDesaduanamiento.Text = string.Empty;

            List_Turnos = new List<Cls_Bil_Turnos>();
            List_Turnos.Clear();

            List_Turnos.Add(new Cls_Bil_Turnos { IdPlan = string.Format("{0}-{1}-{2}-{3}", "0", "0", "", ""), Turno = "* Seleccione *" });

            this.CboTurnos.DataSource = List_Turnos;
            this.CboTurnos.DataTextField = "Turno";
            this.CboTurnos.DataValueField = "IdPlan";
            this.CboTurnos.DataBind();

            this.Actualiza_Paneles();

        }

        private void OcultarLoading(string valor)
        {
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "ocultarloader('" + valor + "');", true);
        }

        public static string securetext(object number)
        {
            if (number == null)
            {
                return string.Empty;
            }
            return QuerySegura.EncryptQueryString(number.ToString());
        }

        private void Mostrar_Mensaje(int Tipo, string Mensaje)
        {
            this.banmsg_det.Visible = true;
            this.banmsg.Visible = true;
            this.banmsg.InnerHtml = Mensaje;
            OcultarLoading("1");
           
            this.banmsg_det.InnerHtml = Mensaje;
            OcultarLoading("2");
      
            this.Actualiza_Paneles();
        }

        private void Ocultar_Mensaje()
        {

            this.banmsg.InnerText = string.Empty;
            this.banmsg_det.InnerText = string.Empty;
            this.banmsg.Visible = false;
            this.banmsg_det.Visible = false;
            this.Actualiza_Paneles();
            OcultarLoading("1");
            OcultarLoading("2");
        }

        private void Crear_Sesion()
        {
            objSesion.USUARIO_CREA = ClsUsuario.loginname;
            nSesion = objSesion.SaveTransaction(out cMensajes);
            if (!nSesion.HasValue || nSesion.Value <= 0)
            {
                this.Mostrar_Mensaje(2, string.Format("<b>Error! No se pudo generar el id de la sesión, por favor comunicarse con el departamento de IT de CGSA... {0}</b>", cMensajes));
                return;
            }
            this.hf_BrowserWindowName.Value = nSesion.ToString().Trim();

            //cabecera de transaccion
            objPaseContenedor = new Cls_Bil_PasePuertaContenedor_Cabecera();
            Session["PaseContenedor" + this.hf_BrowserWindowName.Value] = objPaseContenedor;
        }

        private void Turno_Default()
        {
            //turno por defecto
            List_Turnos.Add(new Cls_Bil_Turnos { IdPlan = string.Format("{0}-{1}-{2}-{3}", "0", "0", "", ""), Turno = "* Seleccione *" });
            this.CboTurnos.DataSource = List_Turnos;
            this.CboTurnos.DataTextField = "Turno";
            this.CboTurnos.DataValueField = "IdPlan";
            this.CboTurnos.DataBind();
        }

        private void Pintar_Grilla()
        {
            foreach (RepeaterItem xitem in tablePagination.Items)
            {
                CheckBox ChkVisto = xitem.FindControl("chkPase") as CheckBox;

                Label LblCarga = xitem.FindControl("LblCarga") as Label;
                Label LblContenedor = xitem.FindControl("LblContenedor") as Label;
                Label LblFechaSalida = xitem.FindControl("LblFechaSalida") as Label;
                Label LblTipo = xitem.FindControl("LblTipo") as Label;
                Label LblTurno = xitem.FindControl("LblTurno") as Label;
                Label LblEmpresa = xitem.FindControl("LblEmpresa") as Label;
                Label LblChofer = xitem.FindControl("LblChofer") as Label;
                Label LblPlaca = xitem.FindControl("LblPlaca") as Label;
                Label LblFechaturno = xitem.FindControl("LblFechaturno") as Label;
                Label LblEstado = xitem.FindControl("LblEstado") as Label;
                if (ChkVisto.Checked == true)
                {

                    LblCarga.ForeColor = System.Drawing.Color.PaleVioletRed;
                    LblContenedor.ForeColor = System.Drawing.Color.PaleVioletRed;
                    LblFechaSalida.ForeColor = System.Drawing.Color.PaleVioletRed;
                    LblTipo.ForeColor = System.Drawing.Color.PaleVioletRed;
                    LblTurno.ForeColor = System.Drawing.Color.PaleVioletRed;
                    LblEmpresa.ForeColor = System.Drawing.Color.PaleVioletRed;
                    LblChofer.ForeColor = System.Drawing.Color.PaleVioletRed;
                    LblPlaca.ForeColor = System.Drawing.Color.PaleVioletRed;
                    LblFechaturno.ForeColor = System.Drawing.Color.PaleVioletRed;
                    LblEstado.ForeColor = System.Drawing.Color.PaleVioletRed;
                }

            }
        }
      
        #endregion



        #region "Eventos del Formulario"

      

        protected void TxtFechaHasta_TextChanged(object sender, EventArgs e)
        {

            try
            { 

                CultureInfo enUS = new CultureInfo("en-US");

                this.Ocultar_Mensaje();
               
                List_Turnos = new List<Cls_Bil_Turnos>();
                List_Turnos.Clear();

                ContenedorSelec = this.TxtContenedorSeleccionado.Text;
                if (string.IsNullOrEmpty(ContenedorSelec))
                {
                    this.Mostrar_Mensaje(2, string.Format("<b>Informativo! Debe seleccionar el contenedor, para poder seleccionar un turno</b>"));
                    this.Txtempresa.Focus();
                    return;
                }

                if (string.IsNullOrEmpty(TxtFechaHasta.Text))
                {
                    this.Turno_Default();
                    this.Actualiza_Paneles();
                    return;
                }



                var ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());
                objPaseContenedor = Session["PaseContenedor" + this.hf_BrowserWindowName.Value] as Cls_Bil_PasePuertaContenedor_Cabecera;
                var Detalle = objPaseContenedor.Detalle.FirstOrDefault(f => f.CONTENEDOR.Equals(ContenedorSelec));
                if (Detalle != null)
                {
                    //si es desaduanamiento directo
                    if (Detalle.CNTR_DD)
                    {
                        this.Turno_Default();
                        /* this.TxtFechaHasta.Text = string.Empty;
                         this.CboTurnos.Focus();
                         this.Actualiza_Paneles();
                         return;*/
                        if (!string.IsNullOrEmpty(TxtFechaHasta.Text))
                        {
                            HoraHasta = "23:59";
                            Fecha = string.Format("{0} {1}", this.TxtFechaHasta.Text.Trim(), HoraHasta);
                            if (!DateTime.TryParseExact(Fecha, "MM-dd-yyyy HH:mm", enUS, DateTimeStyles.AdjustToUniversal, out FechaActualSalida))
                            {
                                this.Turno_Default();
                                this.Mostrar_Mensaje(2, string.Format("<b>Informativo! Debe seleccionar una fecha valida para la generación del pase Mes/Día/año</b>"));
                                this.TxtFechaHasta.Focus();
                                return;
                            }

                        }

                        FechaFacturaHasta = Detalle.FECHA_SALIDA.Value;
                        FechaMenosHoras = FechaFacturaHasta.AddHours(-3);
                        HoraHasta = FechaMenosHoras.ToString("HH:mm");
                        Tipo_Contenedor = Detalle.TIPO_CNTR;

                        if (FechaActualSalida.Date < System.DateTime.Now.Date)
                        {
                            this.Turno_Default();
                            this.TxtFechaHasta.Text = Detalle.FECHA_SALIDA.Value.ToString("MM-dd-yyyy");
                            this.Mostrar_Mensaje(2, string.Format("<b>Informativo! La fecha de salida: {0}, no puede ser menor que la fecha actual: {1} </b>", FechaActualSalida.ToString("MM-dd-yyyy"), System.DateTime.Now.ToString("MM-dd-yyyy")));
                            this.TxtFechaHasta.Focus();
                            return;
                        }

                        if (FechaActualSalida.Date > FechaFacturaHasta.Date)
                        {
                            this.Turno_Default();
                            this.TxtFechaHasta.Text = Detalle.FECHA_SALIDA.Value.ToString("MM-dd-yyyy");
                            this.Mostrar_Mensaje(2, string.Format("<b>Informativo! La fecha de salida: {0}, no puede ser mayor que la fecha tope de facturación: {1} </b>", FechaActualSalida.ToString("MM-dd-yyyy"), FechaFacturaHasta.ToString("MM-dd-yyyy")));
                            this.TxtFechaHasta.Focus();
                            return;
                        }
                        //valida cas
                        var FechaCas = N4.Importacion.container.ObtenerFechaCas(Detalle.GKEY.Value, ClsUsuario.loginname);
                        if (FechaCas.Exitoso)
                        {
                            DateTime? Cas = FechaCas.Resultado;
                            if (FechaActualSalida.Date > Cas.Value)
                            {
                                this.Turno_Default();
                                //this.TxtFechaHasta.Text = Cas.Value.ToString("MM-dd-yyyy");
                                this.TxtFechaHasta.Text = Detalle.FECHA_SALIDA.Value.ToString("MM-dd-yyyy");
                                this.Mostrar_Mensaje(2, string.Format("<b>Informativo! La fecha de salida: {0}, no puede ser mayor que la fecha de la CAS: {1} </b>", FechaActualSalida.ToString("MM-dd-yyyy"), Cas.Value.ToString("MM-dd-yyyy")));
                                this.TxtFechaHasta.Focus();
                                return;
                            }
                        }
                        else
                        {
                            this.Turno_Default();
                            this.TxtFechaHasta.Text = Detalle.FECHA_SALIDA.Value.ToString("MM-dd-yyyy");
                            this.Mostrar_Mensaje(2, string.Format("<b>Error! No se pudo encontrar fecha de CAS para el contenedor: {0}, Aún no se ha ingresado la fecha de la CAS </b>", Detalle.CONTENEDOR));
                            this.CboTurnos.Focus();
                            return;
                        }

                        this.Actualiza_Paneles();

                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(TxtFechaHasta.Text))
                        {
                            HoraHasta = "23:59";
                            Fecha = string.Format("{0} {1}", this.TxtFechaHasta.Text.Trim(), HoraHasta);
                            if (!DateTime.TryParseExact(Fecha, "MM-dd-yyyy HH:mm", enUS, DateTimeStyles.AdjustToUniversal, out FechaActualSalida))
                            {
                                this.Turno_Default();
                                this.Mostrar_Mensaje(2, string.Format("<b>Informativo! Debe seleccionar una fecha valida para la generación del pase Mes/Día/año</b>"));
                                this.TxtFechaHasta.Focus();
                                return;
                            }

                        }

                        FechaFacturaHasta = Detalle.FECHA_SALIDA.Value;
                        FechaMenosHoras = FechaFacturaHasta.AddHours(-3);
                        HoraHasta = FechaMenosHoras.ToString("HH:mm");
                        Tipo_Contenedor = Detalle.TIPO_CNTR;

                        if (FechaActualSalida.Date < System.DateTime.Now.Date)
                        {
                            this.Turno_Default();
                            this.TxtFechaHasta.Text = Detalle.FECHA_SALIDA.Value.ToString("MM-dd-yyyy");
                            this.Mostrar_Mensaje(2, string.Format("<b>Informativo! La fecha de salida: {0}, no puede ser menor que la fecha actual: {1} </b>", FechaActualSalida.ToString("MM-dd-yyyy"), System.DateTime.Now.ToString("MM-dd-yyyy")));
                            this.TxtFechaHasta.Focus();
                            return;
                        }

                        if (FechaActualSalida.Date > FechaFacturaHasta.Date)
                        {
                            this.Turno_Default();
                            this.TxtFechaHasta.Text = Detalle.FECHA_SALIDA.Value.ToString("MM-dd-yyyy");
                            this.Mostrar_Mensaje(2, string.Format("<b>Informativo! La fecha de salida: {0}, no puede ser mayor que la fecha tope de facturación: {1} </b>", FechaActualSalida.ToString("MM-dd-yyyy"), FechaFacturaHasta.ToString("MM-dd-yyyy")));
                            this.TxtFechaHasta.Focus();
                            return;
                        }

                        //valida cas
                        var FechaCas = N4.Importacion.container.ObtenerFechaCas(Detalle.GKEY.Value, ClsUsuario.loginname);
                        if (FechaCas.Exitoso)
                        {
                            DateTime? Cas = FechaCas.Resultado;
                            if (FechaActualSalida.Date > Cas.Value)
                            {
                                this.Turno_Default();
                                //this.TxtFechaHasta.Text = Cas.Value.ToString("MM-dd-yyyy");
                                this.TxtFechaHasta.Text = Detalle.FECHA_SALIDA.Value.ToString("MM-dd-yyyy");
                                this.Mostrar_Mensaje(2, string.Format("<b>Informativo! La fecha de salida: {0}, no puede ser mayor que la fecha de la CAS: {1} </b>", FechaActualSalida.ToString("MM-dd-yyyy"), Cas.Value.ToString("MM-dd-yyyy")));
                                this.TxtFechaHasta.Focus();
                                return;
                            }
                        }
                        else
                        {
                            this.Turno_Default();
                            this.TxtFechaHasta.Text = Detalle.FECHA_SALIDA.Value.ToString("MM-dd-yyyy");
                            this.Mostrar_Mensaje(2, string.Format("<b>Error! No se pudo encontrar fecha de CAS para el contenedor: {0}, Aún no se ha ingresado la fecha de la CAS </b>", Detalle.CONTENEDOR));
                            this.CboTurnos.Focus();
                            return;
                        }


                        var Turnos = PasePuerta.TurnoVBS.ObtenerTurnos(ClsUsuario.ruc, ContenedorSelec, Detalle.GKEY.Value, FechaActualSalida);
                        if (Turnos.Exitoso)
                        {

                            //turno por defecto
                            List_Turnos.Add(new Cls_Bil_Turnos { IdPlan = string.Format("{0}-{1}-{2}-{3}", "0", "0", "", ""), Turno = "* Seleccione *" });

                            //si es contenedor reefer
                            if (Tipo_Contenedor.Trim() == "RF")
                            {
                                //si es el mismo dia de la fecha tope de facturacion
                                FechaInicial = FechaActualSalida.Date;
                                FechaFinal = FechaMenosHoras;
                                HorasDiferencia = FechaFinal.Subtract(FechaInicial);
                                TotalHoras = HorasDiferencia.Hours;
                                var Horas = HorasDiferencia.TotalHours;

                                if (Horas >= 24)
                                {
                                    var LinqQuery = (from Tbl in Turnos.Resultado.Where(Tbl => !String.IsNullOrEmpty(Tbl.Turno))
                                                     select new
                                                     {
                                                         IdPlan = string.Format("{0}-{1}-{2}-{3}", Tbl.IdPlan, Tbl.Secuencia, Tbl.Inicio.ToString("yyyy/MM/dd HH:mm"), Tbl.Fin.ToString("yyyy/MM/dd HH:mm")),
                                                         Turno = Tbl.Turno
                                                     }).ToList().OrderBy(x => x.Turno);

                                    foreach (var Items in LinqQuery)
                                    {
                                        List_Turnos.Add(new Cls_Bil_Turnos { IdPlan = Items.IdPlan, Turno = Items.Turno });
                                    }
                                }
                                else
                                {
                                    if (Horas < 0)
                                    {

                                    }
                                    else
                                    {
                                        var LinqQuery = (from Tbl in Turnos.Resultado.Where(Tbl => !String.IsNullOrEmpty(Tbl.Turno) && (String.Compare(Tbl.Turno, HoraHasta) <= 0))
                                                         select new
                                                         {
                                                             IdPlan = string.Format("{0}-{1}-{2}-{3}", Tbl.IdPlan, Tbl.Secuencia, Tbl.Inicio.ToString("yyyy/MM/dd HH:mm"), Tbl.Fin.ToString("yyyy/MM/dd HH:mm")),
                                                             Turno = Tbl.Turno
                                                         }).ToList().OrderBy(x => x.Turno);

                                        foreach (var Items in LinqQuery)
                                        {
                                            List_Turnos.Add(new Cls_Bil_Turnos { IdPlan = Items.IdPlan, Turno = Items.Turno });
                                        }
                                    }
                                }


                            }
                            else
                            {
                                var LinqQuery = (from Tbl in Turnos.Resultado.Where(Tbl => !String.IsNullOrEmpty(Tbl.Turno))
                                                 select new
                                                 {
                                                     IdPlan = string.Format("{0}-{1}-{2}-{3}", Tbl.IdPlan, Tbl.Secuencia, Tbl.Inicio.ToString("yyyy/MM/dd HH:mm"), Tbl.Fin.ToString("yyyy/MM/dd HH:mm")),
                                                     Turno = Tbl.Turno
                                                 }).ToList().OrderBy(x => x.Turno);

                                foreach (var Items in LinqQuery)
                                {
                                    List_Turnos.Add(new Cls_Bil_Turnos { IdPlan = Items.IdPlan, Turno = Items.Turno });
                                }
                            }

                            this.CboTurnos.DataSource = List_Turnos;
                            this.CboTurnos.DataTextField = "Turno";
                            this.CboTurnos.DataValueField = "IdPlan";
                            this.CboTurnos.DataBind();
                        }
                        else
                        {
                            this.Turno_Default();
                            this.Mostrar_Mensaje(1, string.Format("<b>Informativo! No existe información de turnos para el contenedor: {0}, fecha: {1}, mensaje: {2} </b>", ContenedorSelec, this.TxtFechaHasta.Text, Turnos.MensajeProblema));
                            return;
                        }

                    }//fin //si es desaduanamiento directo


                }
                else
                {
                    this.Turno_Default();
                    this.Mostrar_Mensaje(1, string.Format("<b>Error! No se pudo encontrar fecha de salida para el contenedor: {0} </b>", ContenedorSelec));
                    return;
                }

                this.Actualiza_Paneles();


            }
            catch (Exception ex)
            {
                this.Mostrar_Mensaje(1, string.Format("<b>Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0} </b>", ex.Message));
                return;

            }

}

        protected void BtnAgregar_Click(object sender, EventArgs e)
        {

            try
            {
                CultureInfo enUS = new CultureInfo("en-US");
                ContenedorSelec = this.TxtContenedorSeleccionado.Text;
                this.Ocultar_Mensaje();
                List_Turnos = new List<Cls_Bil_Turnos>();
                List_Turnos.Clear();

                string IdEmpresa = string.Empty;
                string DesEmpresa = string.Empty;
                string IdChofer = string.Empty;
                string DesChofer = string.Empty;

                if (string.IsNullOrEmpty(ContenedorSelec))
                {
                    this.Mostrar_Mensaje(2, string.Format("<b>Informativo! Debe seleccionar el contenedor para poder agregar la información </b>"));
                    this.Txtempresa.Focus();
                    return;
                }

                if (string.IsNullOrEmpty(Txtempresa.Text))
                {
                    this.Mostrar_Mensaje(2, string.Format("<b>Informativo! Debe seleccionar la Compañía de Transporte para poder agregar la información </b>"));
                    this.Txtempresa.Focus();
                    return;
                }

                if (TxtDesaduanamiento.Text.Equals("NO"))
                {
                    if (string.IsNullOrEmpty(TxtFechaHasta.Text))
                    {
                        this.Mostrar_Mensaje(2, string.Format("<b>Informativo! Debe seleccionar una fecha para poder agregar la información </b>"));
                        this.TxtFechaHasta.Focus();
                        return;
                    }

                    if (!string.IsNullOrEmpty(TxtFechaHasta.Text))
                    {
                        HoraHasta = "23:59";
                        Fecha = string.Format("{0} {1}", this.TxtFechaHasta.Text.Trim(), HoraHasta);
                        if (!DateTime.TryParseExact(Fecha, "MM-dd-yyyy HH:mm", enUS, DateTimeStyles.AdjustToUniversal, out FechaActualSalida))
                        {
                            this.Turno_Default();
                            this.Mostrar_Mensaje(2, string.Format("<b>Informativo! Debe seleccionar una fecha valida para poder agregar la información, Mes/Día/año </b>"));
                            this.TxtFechaHasta.Focus();
                            return;
                        }

                    }

                    if (this.CboTurnos.SelectedIndex == 0)
                    {
                        this.Mostrar_Mensaje(2, string.Format("<b>Informativo! Debe seleccionar un turno para poder agregar la información </b>"));
                        this.CboTurnos.Focus();
                        return;
                    }
                    else
                    {
                        TurnoSelect = this.CboTurnos.SelectedValue;
                    }



                }
                else
                {
                    if (string.IsNullOrEmpty(TxtFechaHasta.Text))
                    {
                        this.Mostrar_Mensaje(2, string.Format("<b>Informativo! Debe seleccionar una fecha para poder agregar la información </b>"));
                        this.TxtFechaHasta.Focus();
                        return;
                    }

                    if (!string.IsNullOrEmpty(TxtFechaHasta.Text))
                    {
                        HoraHasta = "23:59";
                        Fecha = string.Format("{0} {1}", this.TxtFechaHasta.Text.Trim(), HoraHasta);
                        if (!DateTime.TryParseExact(Fecha, "MM-dd-yyyy HH:mm", enUS, DateTimeStyles.AdjustToUniversal, out FechaActualSalida))
                        {
                            this.Turno_Default();
                            this.Mostrar_Mensaje(2, string.Format("<b>Informativo! Debe seleccionar una fecha valida para poder agregar la información, Mes/Día/año </b>"));
                            this.TxtFechaHasta.Focus();
                            return;
                        }

                    }
                }

                var ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());
                //valida que exista Empresa Transporte 
                if (!string.IsNullOrEmpty(Txtempresa.Text))
                {
                    EmpresaSelect = this.Txtempresa.Text.Trim();
                    if (EmpresaSelect.Split('-').ToList().Count > 1)
                    {
                        //Int32 p = EmpresaSelect.Split('-').ToList().Count;
                        IdEmpresa = EmpresaSelect.Split('-').ToList()[0].Trim();
                        DesEmpresa = EmpresaSelect.Split('-').ToList()[1].Trim();
                        var EmpresaTransporte = N4.Entidades.CompaniaTransporte.ObtenerCompania(ClsUsuario.loginname, IdEmpresa);
                        if (!EmpresaTransporte.Exitoso)
                        {
                            this.Mostrar_Mensaje(2, string.Format("<b>Informativo! Debe seleccionar una compañía de transporte valida para agregar la información </b>"));
                            this.Txtempresa.Focus();
                            return;
                        }
                    }
                    else
                    {
                        this.Mostrar_Mensaje(2, string.Format("<b>Informativo! Debe seleccionar una compañía de transporte valida para agregar la información </b>"));
                        this.Txtempresa.Focus();
                        return;
                    }
                }

                //valida que exista un chofer
                if (!string.IsNullOrEmpty(TxtChofer.Text))
                {
                    ChoferSelect = this.TxtChofer.Text.Trim();
                    if (ChoferSelect.Split('-').ToList().Count > 1)
                    {
                        //Int32 p = ChoferSelect.Split('-').ToList().Count;
                        IdChofer = ChoferSelect.Split('-').ToList()[0].Trim();
                        DesChofer = ChoferSelect.Split('-').ToList()[1].Trim();
                        var ChoferTransporte = N4.Entidades.Chofer.ObtenerChofer(ClsUsuario.loginname, IdChofer);
                        if (!ChoferTransporte.Exitoso)
                        {
                            this.Mostrar_Mensaje(2, string.Format("<b>Informativo! Debe seleccionar un chofer valido para agregar la información </b>"));
                            this.Txtempresa.Focus();
                            return;
                        }
                    }
                    else
                    {
                        this.Mostrar_Mensaje(2, string.Format("<b>Informativo! Debe seleccionar un chofer valido para agregar la información </b>"));
                        this.Txtempresa.Focus();
                        return;
                    }
                }

                //valida que exista una placa
                if (!string.IsNullOrEmpty(TxtPlaca.Text))
                {
                    PlacaSelect = this.TxtPlaca.Text.Trim();
                    if (PlacaSelect.Split('-').ToList().Count > 1)
                    {
                        string IdPlaca = PlacaSelect.Split('-').ToList()[0].Trim();
                        var PlacaTransporte = N4.Entidades.Camion.ObtenerCamion(ClsUsuario.loginname, IdPlaca);
                        if (!PlacaTransporte.Exitoso)
                        {
                            this.Mostrar_Mensaje(2, string.Format("<b>Informativo! Debe seleccionar una placa de vehículo valida, para poder agregar la información </b>"));
                            this.Txtempresa.Focus();
                            return;
                        }
                    }
                    else
                    {
                        this.Mostrar_Mensaje(2, string.Format("<b>Informativo! Debe seleccionar una placa de vehículo valida, para poder agregar la información </b>"));
                        this.Txtempresa.Focus();
                        return;
                    }
                }

                if (TxtDesaduanamiento.Text.Equals("NO"))
                {
                    string FechaIni = TurnoSelect.Split('-').ToList()[2].Trim();
                    if (!DateTime.TryParseExact(FechaIni, "yyyy/MM/dd HH:mm", enUS, DateTimeStyles.AdjustToUniversal, out FechaTurnoInicio))
                    {

                        this.Mostrar_Mensaje(2, string.Format("<b>Informativo! La fecha de inicio del turno seleccionado no es valida, Mes/Día/año </b>"));
                        this.CboTurnos.Focus();
                        return;
                    }
                    string FechaFin = TurnoSelect.Split('-').ToList()[3].Trim();
                    if (!DateTime.TryParseExact(FechaFin, "yyyy/MM/dd HH:mm", enUS, DateTimeStyles.AdjustToUniversal, out FechaTurnoFinal))
                    {

                        this.Mostrar_Mensaje(2, string.Format("<b>Informativo! La fecha final del turno seleccionado no es valida, Mes/Día/año </b>"));
                        this.CboTurnos.Focus();
                        return;
                    }

                }

                //actualiza datos del contenedor
                objPaseContenedor = Session["PaseContenedor" + this.hf_BrowserWindowName.Value] as Cls_Bil_PasePuertaContenedor_Cabecera;
                var Detalle = objPaseContenedor.Detalle.FirstOrDefault(f => f.CONTENEDOR.Equals(ContenedorSelec));
                if (Detalle != null)
                {
                    Detalle.VISTO = true;
                    Detalle.CIATRANS = EmpresaSelect;
                    Detalle.CHOFER = ChoferSelect;
                    Detalle.PLACA = PlacaSelect;

                    //desaduanamiento directo no lleva turno
                    if (Detalle.CNTR_DD)
                    {
                        Detalle.D_TURNO = string.Empty;
                        Detalle.TURNO = 0;
                        Detalle.ID_TURNO = 0;
                        Detalle.TURNO_DESDE = null;
                        Detalle.TURNO_HASTA = null;
                        //Detalle.FECHA_SALIDA_PASE = null;
                        Detalle.FECHA_SALIDA_PASE = FechaActualSalida;
                        //valida cas
                        var FechaCas = N4.Importacion.container.ObtenerFechaCas(Detalle.GKEY.Value, ClsUsuario.loginname);
                        if (FechaCas.Exitoso)
                        {
                            DateTime? Cas = FechaCas.Resultado;
                            if (FechaActualSalida.Date > Cas.Value.Date)
                            {
                                this.Turno_Default();
                                this.TxtFechaHasta.Text = Cas.Value.ToString("MM-dd-yyyy");
                                this.Mostrar_Mensaje(2, string.Format("<b>Informativo! La fecha de salida: {0}, no puede ser mayor que la fecha de la CAS: {1} </b>", FechaActualSalida.ToString("MM-dd-yyyy"), Cas.Value.ToString("MM-dd-yyyy")));
                                this.TxtFechaHasta.Focus();
                                return;
                            }
                            else
                            {
                                Detalle.CAS = Cas.Value;
                            }
                        }
                        else
                        {
                            this.Turno_Default();
                            this.TxtFechaHasta.Text = Detalle.FECHA_SALIDA.Value.ToString("MM-dd-yyyy");
                            this.Mostrar_Mensaje(2, string.Format("<b>Error! No se pudo encontrar fecha de CAS para el contenedor: {0}, Aún no se ha ingresado la fecha de la CAS </b>", Detalle.CONTENEDOR));
                            this.CboTurnos.Focus();
                            return;
                        }

                    }
                    else
                    {
                        Detalle.D_TURNO = this.CboTurnos.SelectedItem.ToString();
                        Detalle.TURNO = Convert.ToInt64(TurnoSelect.Split('-').ToList()[0].Trim());
                        Detalle.ID_TURNO = Convert.ToInt16(TurnoSelect.Split('-').ToList()[1].Trim());
                        Detalle.TURNO_DESDE = FechaTurnoInicio;
                        Detalle.TURNO_HASTA = FechaTurnoFinal;
                        //Detalle.FECHA_SALIDA_PASE = FechaActualSalida;

                        HoraHasta = Detalle.D_TURNO;
                        Fecha = string.Format("{0} {1}", FechaActualSalida.Date.ToString("MM-dd-yyyy"), HoraHasta);
                        if (!DateTime.TryParseExact(Fecha, "MM-dd-yyyy HH:mm", enUS, DateTimeStyles.AdjustToUniversal, out FechaActualSalida))
                        {
                            this.Turno_Default();
                            this.Mostrar_Mensaje(2, string.Format("<b>Informativo! Debe seleccionar una fecha valida para poder agregar la información, Mes/Día/año </b>"));
                            this.TxtFechaHasta.Focus();
                            return;
                        }

                        Detalle.FECHA_SALIDA_PASE = FechaActualSalida;
                        //valida cas
                        var FechaCas = N4.Importacion.container.ObtenerFechaCas(Detalle.GKEY.Value, ClsUsuario.loginname);
                        if (FechaCas.Exitoso)
                        {
                            DateTime? Cas = FechaCas.Resultado;
                            if (FechaActualSalida.Date > Cas.Value.Date)
                            {
                                this.Turno_Default();
                                this.TxtFechaHasta.Text = Cas.Value.ToString("MM-dd-yyyy");
                                this.Mostrar_Mensaje(2, string.Format("<b>Informativo! La fecha de salida: {0}, no puede ser mayor que la fecha de la CAS: {1} </b>", FechaActualSalida.ToString("MM-dd-yyyy"), Cas.Value.ToString("MM-dd-yyyy")));
                                this.TxtFechaHasta.Focus();
                                return;
                            }
                            else
                            {
                                Detalle.CAS = Cas.Value;
                            }
                        }
                        else
                        {
                            this.Turno_Default();
                            this.TxtFechaHasta.Text = Detalle.FECHA_SALIDA.Value.ToString("MM-dd-yyyy");
                            this.Mostrar_Mensaje(2, string.Format("<b>Error! No se pudo encontrar fecha de CAS para el contenedor: {0}, Aún no se ha ingresado la fecha de la CAS </b>", Detalle.CONTENEDOR));
                            this.CboTurnos.Focus();
                            return;
                        }


                        //reserva turno
                        int ID_CNTR = (int)Detalle.GKEY;
                        int TURNO = (int)Detalle.TURNO;
                        int ID_TURNO = (int)Detalle.ID_TURNO;

                        var Reserva = PasePuerta.Pase_Container.ReservarTurno(ClsUsuario.ruc, TURNO, ID_TURNO, ID_CNTR, FechaActualSalida);
                        if (Reserva.Exitoso)
                        {

                        }
                        else
                        {
                            this.Mostrar_Mensaje(2, string.Format("<b>Informativo! No se pudo realizar la reserva de turnos para el contenedor: {0}, {1} {2} </b>", Detalle.CONTENEDOR, Reserva.MensajeInformacion, Reserva.MensajeProblema));
                            return;
                        }
                    }

                    Detalle.ID_CIATRANS = IdEmpresa;
                    Detalle.ID_CHOFER = IdChofer;
                    Detalle.TRANSPORTISTA_DESC = DesEmpresa;
                    Detalle.CHOFER_DESC = DesChofer;
                }

                tablePagination.DataSource = objPaseContenedor.Detalle;
                tablePagination.DataBind();

                Session["PaseContenedor" + this.hf_BrowserWindowName.Value] = objPaseContenedor;

                //limpiar datos seleccionados
                List_Turnos = new List<Cls_Bil_Turnos>();
                List_Turnos.Clear();

                this.TxtFechaHasta.Text = String.Empty;
                this.TxtContenedorSeleccionado.Text = string.Empty;
                this.TxtDesaduanamiento.Text = string.Empty;
                this.TxtFechaCas.Text = string.Empty;

                this.Turno_Default();
                this.Pintar_Grilla();
                this.Actualiza_Paneles();

            }
            catch (Exception ex)
            {
                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(BtnAgregar_Click), "AgregarTurno", false, null, null, ex.StackTrace, ex);
                OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));

                this.Mostrar_Mensaje(2, string.Format("<b>Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible..Debe seleccionar la empresa de transporte-chofer valido.. {0} </b>", OError));

            }

        }

      

        protected void BtnBuscar_Click(object sender, EventArgs e)
        {

            if (Response.IsClientConnected)
            {
                try
                {
                    this.BtnGrabar.Attributes.Remove("disabled");
                    this.Actualiza_Paneles();

                    OcultarLoading("2");

                    if (HttpContext.Current.Request.Cookies["token"] == null)
                    {
                        System.Web.Security.FormsAuthentication.SignOut();
                        System.Web.Security.FormsAuthentication.RedirectToLoginPage();
                        Session.Clear();
                        OcultarLoading("1");
                        return;
                    }

                    if (string.IsNullOrEmpty(this.TXTMRN.Text))
                    {
                        this.Mostrar_Mensaje(1, string.Format("<b>Informativo! </b>Por favor ingresar el número de la carga MRN"));
                        this.TXTMRN.Focus();
                        return;
                    }
                    if (string.IsNullOrEmpty(this.TXTMSN.Text))
                    {
                        this.Mostrar_Mensaje(1, string.Format("<b>Informativo! </b>Por favor ingresar el número de la carga MSN"));
                        this.TXTMSN.Focus();
                        return;
                    }
                    if (string.IsNullOrEmpty(this.TXTHSN.Text))
                    {

                        this.Mostrar_Mensaje(1, string.Format("<b><b>Informativo! </b>Por favor ingresar el número de la carga HSN"));
                        this.TXTHSN.Focus();
                        return;
                    }

                    tablePagination.DataSource = null;
                    tablePagination.DataBind();

                    //busca contenedores por ruc de usuario
                    var ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());
                    var Contenedores = PasePuerta.Pase_Web.ObtenerCargaPase(this.TXTMRN.Text.Trim(), this.TXTMSN.Text.Trim(), this.TXTHSN.Text.Trim(), ClsUsuario.ruc);

                    if (Contenedores.Exitoso)
                    {


                        /*query contenedores*/
                        var LinqQuery = (from Tbl in Contenedores.Resultado.Where(Tbl => !String.IsNullOrEmpty(Tbl.CONTENEDOR))
                                         select new
                                         {
                                             ID_PPWEB = Tbl.ID_PPWEB,
                                             CARGA = string.Format("{0}-{1}-{2}", Tbl.MRN, Tbl.MSN, Tbl.HSN),
                                             MRN = Tbl.MRN,
                                             MSN = Tbl.MSN,
                                             HSN = Tbl.HSN,
                                             FACTURA = (Tbl.FACTURA == null) ? string.Empty : Tbl.FACTURA,
                                             AGENTE = (Tbl.AGENTE == null) ? string.Empty : Tbl.AGENTE,
                                             FACTURADO = (Tbl.FACTURADO == null) ? string.Empty : Tbl.FACTURADO,
                                             PAGADO = Tbl.PAGADO,
                                             GKEY = (Tbl.GKEY == null) ? 0 : Tbl.GKEY,
                                             REFERENCIA = (Tbl.REFERENCIA == null) ? string.Empty : Tbl.REFERENCIA,
                                             CONTENEDOR = (Tbl.CONTENEDOR == null) ? string.Empty : Tbl.CONTENEDOR,
                                             DOCUMENTO = (Tbl.DOCUMENTO == null) ? string.Empty : Tbl.DOCUMENTO,
                                             CIATRANS = (Tbl.CIATRANS == null) ? string.Empty : Tbl.CIATRANS,
                                             CHOFER = (Tbl.CHOFER == null) ? string.Empty : Tbl.CHOFER,
                                             PLACA = (Tbl.PLACA == null) ? string.Empty : Tbl.PLACA,
                                             CAS = (Tbl.CAS.HasValue ? Tbl.CAS : null),
                                             CNTR_DD = (Tbl.CNTR_DD == null) ? false : Tbl.CNTR_DD,
                                             AGENTE_DESC = (Tbl.AGENTE_DESC == null) ? string.Empty : Tbl.AGENTE_DESC,
                                             FACTURADO_DESC = (Tbl.FACTURADO_DESC == null) ? string.Empty : Tbl.FACTURADO_DESC,
                                             IMPORTADOR = (Tbl.IMPORTADOR == null) ? string.Empty : Tbl.IMPORTADOR,
                                             IMPORTADOR_DESC = (Tbl.IMPORTADOR_DESC == null) ? string.Empty : Tbl.IMPORTADOR_DESC,
                                             FECHA_SALIDA = (Tbl.FECHA_SALIDA.HasValue ? Tbl.FECHA_SALIDA : null),
                                             FECHA_SALIDA_PASE = (Tbl.FECHA_SALIDA.HasValue ? Tbl.FECHA_SALIDA : null),
                                             FECHA_AUT_PPWEB = (Tbl.FECHA_AUT_PPWEB.HasValue ? Tbl.FECHA_AUT_PPWEB : null),
                                             HORA_AUT_PPWEB = (Tbl.HORA_AUT_PPWEB == null) ? string.Empty : Tbl.HORA_AUT_PPWEB,
                                             TIPO_CNTR = (Tbl.TIPO_CNTR == null) ? string.Empty : Tbl.TIPO_CNTR,
                                             ID_TURNO = (Tbl.ID_TURNO == null) ? 0 : Tbl.ID_TURNO,
                                             TURNO = (Tbl.TURNO == null) ? 0 : Tbl.TURNO,
                                             D_TURNO = (Tbl.D_TURNO == null) ? string.Empty : Tbl.D_TURNO,
                                             ID_PASE = (Tbl.ID_PASE == null) ? 0 : Tbl.ID_PASE,
                                             ESTADO = Tbl.ESTADO,
                                             ENVIADO = Tbl.ENVIADO,
                                             AUTORIZADO = Tbl.AUTORIZADO,
                                             VENTANILLA = Tbl.VENTANILLA,
                                             USUARIO_ING = ClsUsuario.loginname,
                                             USUARIO_MOD = ClsUsuario.loginname,
                                             ESTADO_PAGO = (Tbl.PAGADO==true ? "SI" : "NO")
                                         }).ToList().OrderBy(x => x.CONTENEDOR);

                        if (LinqQuery != null && LinqQuery.Count() > 0)
                        {


                            //agrego todos los contenedores a la clase cabecera
                            objPaseContenedor = Session["PaseContenedor" + this.hf_BrowserWindowName.Value] as Cls_Bil_PasePuertaContenedor_Cabecera;

                            objPaseContenedor.FECHA = DateTime.Now;
                            objPaseContenedor.MRN = this.TXTMRN.Text;
                            objPaseContenedor.MSN = this.TXTMSN.Text;
                            objPaseContenedor.HSN = this.TXTHSN.Text;
                            objPaseContenedor.IV_USUARIO_CREA = ClsUsuario.loginname;
                            objPaseContenedor.SESION = this.hf_BrowserWindowName.Value;


                            objPaseContenedor.Detalle.Clear();

                            foreach (var Det in LinqQuery)
                            {

                                objDetallePaseContenedor = new Cls_Bil_PasePuertaContenedor_Detalle();
                                objDetallePaseContenedor.ID_PPWEB = Det.ID_PPWEB;
                                objDetallePaseContenedor.MRN = Det.MRN;
                                objDetallePaseContenedor.MSN = Det.MSN;
                                objDetallePaseContenedor.HSN = Det.HSN;
                                objDetallePaseContenedor.CARGA = Det.CARGA;
                                objDetallePaseContenedor.FACTURA = Det.FACTURA;
                                objDetallePaseContenedor.AGENTE = Det.AGENTE;
                                objDetallePaseContenedor.FACTURADO = Det.FACTURADO;
                                objDetallePaseContenedor.PAGADO = Det.PAGADO;
                                objDetallePaseContenedor.GKEY = Det.GKEY;
                                objDetallePaseContenedor.REFERENCIA = Det.REFERENCIA;
                                objDetallePaseContenedor.CONTENEDOR = Det.CONTENEDOR;
                                objDetallePaseContenedor.DOCUMENTO = Det.DOCUMENTO;
                                //objDetallePaseContenedor.CIATRANS = Det.CIATRANS;
                                //objDetallePaseContenedor.CHOFER = Det.CHOFER;
                                //objDetallePaseContenedor.PLACA = Det.PLACA;
                                objDetallePaseContenedor.CIATRANS = string.Empty;
                                objDetallePaseContenedor.CHOFER = string.Empty;
                                objDetallePaseContenedor.PLACA = string.Empty;
                                objDetallePaseContenedor.CAS = Det.CAS;
                                objDetallePaseContenedor.FECHA_SALIDA = Det.FECHA_SALIDA;
                                objDetallePaseContenedor.CNTR_DD = Det.CNTR_DD.Value;
                                objDetallePaseContenedor.AGENTE_DESC = Det.AGENTE_DESC;
                                objDetallePaseContenedor.FACTURADO_DESC = Det.FACTURADO_DESC;
                                objDetallePaseContenedor.IMPORTADOR = Det.IMPORTADOR;
                                objDetallePaseContenedor.IMPORTADOR_DESC = Det.IMPORTADOR_DESC;
                                //desaduanamiento directo 
                                if (Det.CNTR_DD.Value)
                                {
                                    //objDetallePaseContenedor.FECHA_SALIDA_PASE = null;
                                    objDetallePaseContenedor.FECHA_SALIDA_PASE = Det.FECHA_SALIDA_PASE;
                                }
                                else
                                {
                                    objDetallePaseContenedor.FECHA_SALIDA_PASE = Det.FECHA_SALIDA_PASE;
                                }
                               
                                objDetallePaseContenedor.FECHA_AUT_PPWEB = Det.FECHA_AUT_PPWEB;
                                objDetallePaseContenedor.HORA_AUT_PPWEB = Det.HORA_AUT_PPWEB;

                                objDetallePaseContenedor.TIPO_CNTR = Det.TIPO_CNTR;
                                objDetallePaseContenedor.ID_TURNO = Det.ID_TURNO;
                                objDetallePaseContenedor.TURNO = Det.TURNO;
                                objDetallePaseContenedor.D_TURNO = Det.D_TURNO;
                                objDetallePaseContenedor.ID_PASE = Det.ID_PASE;
                                objDetallePaseContenedor.ESTADO = Det.ESTADO;
                                objDetallePaseContenedor.ENVIADO = Det.ENVIADO;
                                objDetallePaseContenedor.AUTORIZADO = Det.AUTORIZADO;
                                objDetallePaseContenedor.VENTANILLA = Det.VENTANILLA;
                                objDetallePaseContenedor.USUARIO_ING = Det.USUARIO_ING;
                                objDetallePaseContenedor.FECHA_ING = System.DateTime.Now.Date;
                                objDetallePaseContenedor.USUARIO_MOD = Det.USUARIO_MOD;
                                objDetallePaseContenedor.ESTADO_PAGO = Det.ESTADO_PAGO;

                                if (Det.CNTR_DD.Value)
                                {
                                    objDetallePaseContenedor.TIPO_CNTR = string.Format("{0} - {1}",Det.TIPO_CNTR, "Desaduanamiento Directo");
                                }
                                
                                objPaseContenedor.Detalle.Add(objDetallePaseContenedor);

                            }

                            tablePagination.DataSource = objPaseContenedor.Detalle;
                            tablePagination.DataBind();

                            Session["PaseContenedor" + this.hf_BrowserWindowName.Value] = objPaseContenedor;
                            
                        }
                        else
                        {
                            tablePagination.DataSource = null;
                            tablePagination.DataBind();
                        }


                    }
                    else
                    {
                        this.Mostrar_Mensaje(1, string.Format("<b>Informativo! </b>No existe información para mostrar con el número de la carga ingresada..{0}", Contenedores.MensajeProblema));
                       
                        return;
                    }

                    this.Ocultar_Mensaje();


                }
                catch (Exception ex)
                {
                    this.Mostrar_Mensaje(1, string.Format("<b>Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", ex.Message));

                }
            }




        }

        protected void BtnGrabar_Click(object sender, EventArgs e)
        {

            CultureInfo enUS = new CultureInfo("en-US");
            string id_carga = string.Empty;

            if (Response.IsClientConnected)
            {
                if (HttpContext.Current.Request.Cookies["token"] == null)
                {
                    System.Web.Security.FormsAuthentication.SignOut();
                    System.Web.Security.FormsAuthentication.RedirectToLoginPage();
                    Session.Clear();
                    return;
                }

                try
                {

                    if (string.IsNullOrEmpty(this.TXTMRN.Text))
                    {
                        this.Mostrar_Mensaje(1, string.Format("<b>Informativo! Por favor ingresar el número de la carga MRN </b>"));
                        this.TXTMRN.Focus();
                        return;
                    }
                    if (string.IsNullOrEmpty(this.TXTMSN.Text))
                    {
                        this.Mostrar_Mensaje(1, string.Format("<b>Informativo! Por favor ingresar el número de la carga MSN </b>"));
                        this.TXTMSN.Focus();
                        return;
                    }
                    if (string.IsNullOrEmpty(this.TXTHSN.Text))
                    {
                        this.Mostrar_Mensaje(1, string.Format("<b><b>Informativo! Por favor ingresar el número de la carga HSN </b>"));
                        this.TXTHSN.Focus();
                        return;
                    }


                    //instancia sesion
                    objPaseContenedor = Session["PaseContenedor" + this.hf_BrowserWindowName.Value] as Cls_Bil_PasePuertaContenedor_Cabecera;
                    if (objPaseContenedor == null)
                    {
                        this.Mostrar_Mensaje(2, string.Format("<b>Informativo! Debe generar la consulta, para poder generar los pase a puerta por contenedor </b>"));
                        return;
                    }
                    if (objPaseContenedor.Detalle.Count == 0)
                    {
                        this.Mostrar_Mensaje(2, string.Format("<b>Informativo! No existe detalle de contenedores, para poder generar los pase a puerta </b>"));
                        return;
                    }

                    var LinqListContenedor = (from p in objPaseContenedor.Detalle.Where(x => x.VISTO == true)
                                              select p.CONTENEDOR).ToList();

                    if (LinqListContenedor.Count == 0)
                    {
                        this.Mostrar_Mensaje(2, string.Format("<b>Informativo! Debe seleccionar los contenedores a generar los pases a puerta </b>"));
                        return;
                    }


                    LoginName = objPaseContenedor.IV_USUARIO_CREA.Trim();

                    /*contenedores seleccionados para emitir pase*/
                    var LinqQuery = (from Tbl in objPaseContenedor.Detalle.Where(Tbl => !String.IsNullOrEmpty(Tbl.CONTENEDOR) && Tbl.VISTO == true)
                                     select new
                                     {
                                         ID_PPWEB = Tbl.ID_PPWEB,
                                         CARGA = string.Format("{0}-{1}-{2}", Tbl.MRN, Tbl.MSN, Tbl.HSN),
                                         MRN = Tbl.MRN,
                                         MSN = Tbl.MSN,
                                         HSN = Tbl.HSN,
                                         FACTURA = (Tbl.FACTURA == null) ? string.Empty : Tbl.FACTURA,
                                         AGENTE = (Tbl.AGENTE == null) ? string.Empty : Tbl.AGENTE,
                                         FACTURADO = (Tbl.FACTURADO == null) ? string.Empty : Tbl.FACTURADO,
                                         PAGADO = Tbl.PAGADO,
                                         GKEY = (Tbl.GKEY == null) ? 0 : Tbl.GKEY,
                                         REFERENCIA = (Tbl.REFERENCIA == null) ? string.Empty : Tbl.REFERENCIA,
                                         CONTENEDOR = (Tbl.CONTENEDOR == null) ? string.Empty : Tbl.CONTENEDOR,
                                         DOCUMENTO = (Tbl.DOCUMENTO == null) ? string.Empty : Tbl.DOCUMENTO,
                                         CIATRANS = (Tbl.CIATRANS == null) ? string.Empty : Tbl.CIATRANS,
                                         CHOFER = (Tbl.CHOFER == null) ? string.Empty : Tbl.CHOFER,
                                         PLACA = (Tbl.PLACA == null) ? string.Empty : Tbl.PLACA,
                                         CAS = (Tbl.CAS.HasValue ? Tbl.CAS : null),
                                         FECHA_SALIDA = (Tbl.FECHA_SALIDA.HasValue ? Tbl.FECHA_SALIDA : null),
                                         FECHA_SALIDA_PASE = (Tbl.FECHA_SALIDA_PASE.HasValue ? Tbl.FECHA_SALIDA_PASE : null),
                                         FECHA_AUT_PPWEB = (Tbl.FECHA_AUT_PPWEB.HasValue ? Tbl.FECHA_AUT_PPWEB : null),
                                         HORA_AUT_PPWEB = (Tbl.HORA_AUT_PPWEB == null) ? string.Empty : Tbl.HORA_AUT_PPWEB,
                                         TIPO_CNTR = (Tbl.TIPO_CNTR == null) ? string.Empty : Tbl.TIPO_CNTR,
                                         ID_TURNO = (Tbl.ID_TURNO == null) ? 0 : Tbl.ID_TURNO,
                                         TURNO = (Tbl.TURNO == null) ? 0 : Tbl.TURNO,
                                         D_TURNO = (Tbl.D_TURNO == null) ? string.Empty : Tbl.D_TURNO,
                                         ID_PASE = (Tbl.ID_PASE == null) ? 0 : Tbl.ID_PASE,
                                         ESTADO = Tbl.ESTADO,
                                         ENVIADO = Tbl.ENVIADO,
                                         AUTORIZADO = Tbl.AUTORIZADO,
                                         VENTANILLA = Tbl.VENTANILLA,
                                         ID_CHOFER = (Tbl.ID_CHOFER == null) ? string.Empty : Tbl.ID_CHOFER,
                                         ID_CIATRANS = (Tbl.ID_CIATRANS == null) ? string.Empty : Tbl.ID_CIATRANS,
                                         USUARIO = Tbl.USUARIO_ING,
                                         TURNO_DESDE = (Tbl.TURNO_DESDE.HasValue ? Tbl.TURNO_DESDE : null),
                                         TURNO_HASTA = (Tbl.TURNO_HASTA.HasValue ? Tbl.TURNO_HASTA : null),
                                         CNTR_DD = Tbl.CNTR_DD,
                                         AGENTE_DESC = (Tbl.AGENTE_DESC == null) ? string.Empty : Tbl.AGENTE_DESC,
                                         FACTURADO_DESC = (Tbl.FACTURADO_DESC == null) ? string.Empty : Tbl.FACTURADO_DESC,
                                         IMPORTADOR = (Tbl.IMPORTADOR == null) ? string.Empty : Tbl.IMPORTADOR,
                                         IMPORTADOR_DESC = (Tbl.IMPORTADOR_DESC == null) ? string.Empty : Tbl.IMPORTADOR_DESC,
                                         TRANSPORTISTA_DESC = (Tbl.TRANSPORTISTA_DESC == null) ? string.Empty : Tbl.TRANSPORTISTA_DESC,
                                         CHOFER_DESC = (Tbl.CHOFER_DESC == null) ? string.Empty : Tbl.CHOFER_DESC

                                     }).ToList().OrderBy(x => x.CONTENEDOR);

                    /***********************************************************************************************************************************************
                    *valida generar pase puerta
                    **********************************************************************************************************************************************/
                    foreach (var Det in LinqQuery)
                    {
                        //validaciones desaduanamiento directo 
                        if (!Det.CNTR_DD)
                        {
                            if (string.IsNullOrEmpty(Det.D_TURNO))
                            {
                                this.Mostrar_Mensaje(1, string.Format("<b>Informativo! No se ha ingresado el turno para el contenedor seleccionado: {0} </b>", Det.CONTENEDOR));
                                this.CboTurnos.Focus();
                                return;
                            }

                        }

                        if (string.IsNullOrEmpty(Det.ID_CIATRANS))
                        {
                            this.Mostrar_Mensaje(1, string.Format("<b>Informativo! No se ha ingresado la empresa de transporte para el contenedor seleccionado: {0} </b>", Det.CONTENEDOR));
                            //this.IdTxtempresa.Focus();
                            return;
                        }

                    }

                    this.BtnGrabar.Attributes["disabled"] = "disabled";
                    this.UPBOTONES.Update();

                    /***********************************************************************************************************************************************
                    *generar pase puerta
                    **********************************************************************************************************************************************/
                    int nTotal = 0;
                    foreach (var Det in LinqQuery)
                    {

                        Pase_Container pase = new Pase_Container();
                        pase.ID_CARGA = Det.GKEY;
                        pase.ESTADO = "GN";
                        //validaciones desaduanamiento directo 
                        if (!Det.CNTR_DD)
                        {
                            pase.FECHA_EXPIRACION = Det.FECHA_SALIDA_PASE;
                        }
                        else
                        {
                           // pase.FECHA_EXPIRACION = Det.FECHA_SALIDA;
                            pase.FECHA_EXPIRACION = Det.FECHA_SALIDA_PASE;
                        }
                        pase.ID_PLACA = Det.PLACA;
                        pase.ID_CHOFER = Det.ID_CHOFER;
                        pase.ID_EMPRESA = Det.ID_CIATRANS;
                        pase.CANTIDAD_CARGA = 0;
                        pase.USUARIO_REGISTRO = Det.USUARIO;
                        pase.TIPO_CARGA = "CNTR";
                        pase.TINICIA = Det.TURNO_DESDE;
                        pase.TFIN = Det.TURNO_HASTA;
                        pase.TID = Det.TURNO.Value;
                        pase.CONSIGNATARIO_ID = Det.IMPORTADOR;
                        pase.CONSIGNARIO_NOMBRE = Det.IMPORTADOR_DESC;
                        pase.TRANSPORTISTA_DESC = Det.TRANSPORTISTA_DESC;
                        pase.CHOFER_DESC = Det.CHOFER_DESC;
                        pase.PPW = Det.ID_PPWEB;

                        var Resultado = pase.Insertar(Det.TURNO.Value, Det.ID_TURNO.Value, Det.CONTENEDOR, Det.D_TURNO, Det.CAS.Value, Det.CNTR_DD);
                        if (Resultado.Exitoso)
                        {
                            nTotal++;
                            if (nTotal == 1)
                            {
                                id_carga = securetext(Det.CARGA);
                            }
                        }
                        else
                        {
                            this.Mostrar_Mensaje(2, string.Format("<b>Error! Se presentaron los siguientes problemas, no se pudo generar el pase de puerta para el contenedor: {0}, Existen los siguientes problemas: {1}, {2} </b>", Det.CONTENEDOR, Resultado.MensajeInformacion, Resultado.MensajeProblema));
                            return;
                        }

                    }

                    if (nTotal != 0)
                    {
                        string link = string.Format("<a href='../pasepuertacontenedor/imprimirpasecontenedor.aspx?id_carga={0}' target ='_parent'>Imprimir Pase Puerta</a>", id_carga);

                        //limpiar
                        objPaseContenedor.Detalle.Clear();
                        Session["PaseContenedor" + this.hf_BrowserWindowName.Value] = objPaseContenedor;
                        tablePagination.DataSource = objPaseContenedor.Detalle;
                        tablePagination.DataBind();

                        this.Limpia_Campos();


                        this.Mostrar_Mensaje(2, string.Format("<b>Informativo! Se procedieron a generar {0} pase de puerta con éxito, para proceder a imprimir los mismo, <br/>por favor dar click en el siguiente link: {1} </b>", nTotal, link));
                        return;
                    }


                }
                catch (Exception ex)
                {
                    lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(BtnGrabar_Click), "Grabar", false, null, null, ex.StackTrace, ex);
                    OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));

                    this.Mostrar_Mensaje(2, string.Format("<b>Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible..{0} </b>", OError));

                }
            }
           


        }

        #region "Eventos de la grilla"

        protected void chkPase_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                CheckBox chkPase = (CheckBox)sender;
                RepeaterItem item = (RepeaterItem)chkPase.NamingContainer;
                Label LblContenedor = (Label)item.FindControl("LblContenedor");

                ContenedorSelec = LblContenedor.Text;
                var ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());

                //actualiza datos del contenedor
                objPaseContenedor = Session["PaseContenedor" + this.hf_BrowserWindowName.Value] as Cls_Bil_PasePuertaContenedor_Cabecera;
                var Detalle = objPaseContenedor.Detalle.FirstOrDefault(f => f.CONTENEDOR.Equals(ContenedorSelec));
                if (Detalle != null)
                {
                    Detalle.VISTO = chkPase.Checked;
                    Detalle.CIATRANS = string.Empty;
                    Detalle.CHOFER = string.Empty;
                    Detalle.PLACA = string.Empty;
                    Detalle.D_TURNO = string.Empty;
                    Detalle.TURNO = 0;
                    Detalle.ID_TURNO = 0;
                    Detalle.TURNO_DESDE = null;
                    Detalle.TURNO_HASTA = null;
                }

                tablePagination.DataSource = objPaseContenedor.Detalle;
                tablePagination.DataBind();

                Session["PaseContenedor" + this.hf_BrowserWindowName.Value] = objPaseContenedor;

                this.Pintar_Grilla();



            }
            catch (Exception ex)
            {
                this.Mostrar_Mensaje(2, string.Format("<b>Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible..{0}", ex.Message));

            }
        }


        protected void tablePagination_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (Response.IsClientConnected)
            {
                try
                {
                    this.Ocultar_Mensaje();
                    if (HttpContext.Current.Request.Cookies["token"] == null)
                    {
                        System.Web.Security.FormsAuthentication.SignOut();
                        Session.Clear();
                        System.Web.Security.FormsAuthentication.RedirectToLoginPage();
                        return;
                    }

                    List_Turnos = new List<Cls_Bil_Turnos>();
                    List_Turnos.Clear();

                    var ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());

                    if (e.CommandArgument == null)
                    {
                        this.Mostrar_Mensaje(1, string.Format("<b>Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0} </b>", "no existen argumentos"));
                        return;
                    }

                    var t = e.CommandArgument.ToString();
                    if (String.IsNullOrEmpty(t))
                    {
                        this.Mostrar_Mensaje(1, string.Format("<b>Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0} </b>", "argumento null"));
                        return;

                    }

                    if (e.CommandName == "Actualizar")
                    {
                        this.TxtContenedorSeleccionado.Text = t.ToString();


                        objPaseContenedor = Session["PaseContenedor" + this.hf_BrowserWindowName.Value] as Cls_Bil_PasePuertaContenedor_Cabecera;
                        var Detalle = objPaseContenedor.Detalle.FirstOrDefault(f => f.CONTENEDOR.Equals(t.ToString()));
                        if (Detalle != null)
                        {
                            Tipo_Contenedor = Detalle.TIPO_CNTR;

                            //desaduanamiento directo no lleva turnos
                            if (Detalle.CNTR_DD)
                            {
                                this.TxtDesaduanamiento.Text = "SI";
                                FechaFacturaHasta = Detalle.FECHA_SALIDA_PASE.Value;
                                FechaMenosHoras = FechaFacturaHasta.AddHours(-3);
                                HoraHasta = FechaMenosHoras.ToString("HH:mm");
                                this.TxtFechaHasta.Text = Detalle.FECHA_SALIDA_PASE.Value.ToString("MM-dd-yyyy");
                            }
                            else
                            {
                                FechaFacturaHasta = Detalle.FECHA_SALIDA_PASE.Value;
                                FechaMenosHoras = FechaFacturaHasta.AddHours(-3);
                                HoraHasta = FechaMenosHoras.ToString("HH:mm");
                                this.TxtDesaduanamiento.Text = "NO";
                                this.TxtFechaHasta.Text = Detalle.FECHA_SALIDA_PASE.Value.ToString("MM-dd-yyyy");
                            }


                            if (!string.IsNullOrEmpty(Detalle.CIATRANS))
                            {
                                this.Txtempresa.Text = Detalle.CIATRANS;
                            }
                            if (!string.IsNullOrEmpty(Detalle.CHOFER))
                            {
                                this.TxtChofer.Text = Detalle.CHOFER;
                            }
                            if (!string.IsNullOrEmpty(Detalle.PLACA))
                            {
                                this.TxtPlaca.Text = Detalle.PLACA;
                            }

                            //desaduanamiento directo no lleva turnos
                            if (!Detalle.CNTR_DD)
                            {

                                //valida cas
                                var FechaCas = N4.Importacion.container.ObtenerFechaCas(Detalle.GKEY.Value, ClsUsuario.loginname);
                                if (FechaCas.Exitoso)
                                {
                                    DateTime? Cas = FechaCas.Resultado;
                                    this.TxtFechaCas.Text = Cas.Value.ToString("MM-dd-yyyy");

                                }
                                else
                                {
                                    this.Turno_Default();
                                    this.TxtFechaCas.Text = string.Empty;
                                    this.TxtFechaHasta.Text = string.Empty;
                                    this.Mostrar_Mensaje(2, string.Format("<b>Error! No se pudo encontrar fecha de CAS para el contenedor: {0}, Aún no se ha ingresado la fecha de la CAS </b>", Detalle.CONTENEDOR));
                                    this.CboTurnos.Focus();
                                    return;
                                }
                                //fin valida cas


                                var Turnos = PasePuerta.TurnoVBS.ObtenerTurnos(ClsUsuario.ruc, t.ToString(), Detalle.GKEY.Value, Detalle.FECHA_SALIDA_PASE.Value);
                                if (Turnos.Exitoso)
                                {
                                    //turno por defecto
                                    List_Turnos.Add(new Cls_Bil_Turnos { IdPlan = string.Format("{0}-{1}-{2}-{3}", "0", "0", "", ""), Turno = "* Seleccione *" });

                                    //si es contenedor reefer
                                    if (Tipo_Contenedor.Trim() == "RF")
                                    {
                                        //si es el mismo dia de la fecha tope de facturacion
                                        FechaInicial = FechaFacturaHasta.Date;
                                        FechaFinal = FechaMenosHoras;
                                        HorasDiferencia = FechaFinal.Subtract(FechaInicial);
                                        TotalHoras = HorasDiferencia.Hours;
                                        var Horas = HorasDiferencia.TotalHours;

                                        if (Horas >= 24)
                                        {
                                            var LinqQuery = (from Tbl in Turnos.Resultado.Where(Tbl => !String.IsNullOrEmpty(Tbl.Turno))
                                                             select new
                                                             {
                                                                 IdPlan = string.Format("{0}-{1}-{2}-{3}", Tbl.IdPlan, Tbl.Secuencia, Tbl.Inicio.ToString("yyyy/MM/dd HH:mm"), Tbl.Fin.ToString("yyyy/MM/dd HH:mm")),
                                                                 Turno = Tbl.Turno
                                                             }).ToList().OrderBy(x => x.Turno);

                                            foreach (var Items in LinqQuery)
                                            {
                                                List_Turnos.Add(new Cls_Bil_Turnos { IdPlan = Items.IdPlan, Turno = Items.Turno });
                                            }
                                        }
                                        else
                                        {
                                            if (Horas < 0)
                                            {

                                            }
                                            else
                                            {
                                                var LinqQuery = (from Tbl in Turnos.Resultado.Where(Tbl => !String.IsNullOrEmpty(Tbl.Turno) && (String.Compare(Tbl.Turno, HoraHasta) <= 0))
                                                                 select new
                                                                 {
                                                                     IdPlan = string.Format("{0}-{1}-{2}-{3}", Tbl.IdPlan, Tbl.Secuencia, Tbl.Inicio.ToString("yyyy/MM/dd HH:mm"), Tbl.Fin.ToString("yyyy/MM/dd HH:mm")),
                                                                     Turno = Tbl.Turno
                                                                 }).ToList().OrderBy(x => x.Turno);

                                                foreach (var Items in LinqQuery)
                                                {
                                                    List_Turnos.Add(new Cls_Bil_Turnos { IdPlan = Items.IdPlan, Turno = Items.Turno });
                                                }
                                            }
                                        }


                                    }
                                    else
                                    {
                                        var LinqQuery = (from Tbl in Turnos.Resultado.Where(Tbl => !String.IsNullOrEmpty(Tbl.Turno))
                                                         select new
                                                         {
                                                             IdPlan = string.Format("{0}-{1}-{2}-{3}", Tbl.IdPlan, Tbl.Secuencia, Tbl.Inicio.ToString("yyyy/MM/dd HH:mm"), Tbl.Fin.ToString("yyyy/MM/dd HH:mm")),
                                                             Turno = Tbl.Turno
                                                         }).ToList().OrderBy(x => x.Turno);

                                        foreach (var Items in LinqQuery)
                                        {
                                            List_Turnos.Add(new Cls_Bil_Turnos { IdPlan = Items.IdPlan, Turno = Items.Turno });
                                        }
                                    }

                                    this.CboTurnos.DataSource = List_Turnos;
                                    this.CboTurnos.DataTextField = "Turno";
                                    this.CboTurnos.DataValueField = "IdPlan";
                                    this.CboTurnos.DataBind();

                                }
                                else
                                {
                                    this.Turno_Default();
                                    this.Mostrar_Mensaje(1, string.Format("<b>Informativo! No existe información de turnos para el contenedor: {0}, fecha: {1}, mensaje: {2} </b>", t.ToString(), this.TxtFechaHasta.Text, Turnos.MensajeProblema));
                                    return;
                                }
                            }
                            else
                            {
                                this.Turno_Default();
                                //valida cas
                                var FechaCas = N4.Importacion.container.ObtenerFechaCas(Detalle.GKEY.Value, ClsUsuario.loginname);
                                if (FechaCas.Exitoso)
                                {
                                    //pruebas
                                   /* DateTime valor = DateTime.Parse("2020/08/08");
                                    this.TxtFechaCas.Text = valor.ToString("MM-dd-yyyy");
                                    */
                                    DateTime? Cas = FechaCas.Resultado;
                                    this.TxtFechaCas.Text = Cas.Value.ToString("MM-dd-yyyy");

                                }
                                else
                                {
                                    this.Turno_Default();
                                    this.TxtFechaCas.Text = string.Empty;
                                    this.TxtFechaHasta.Text = string.Empty;
                                    this.Mostrar_Mensaje(2, string.Format("<b>Error! No se pudo encontrar fecha de CAS para el contenedor: {0}, Aún no se ha ingresado la fecha de la CAS </b>", Detalle.CONTENEDOR));
                                    this.CboTurnos.Focus();
                                    return;
                                }
                                //fin valida cas

                            }

                        }
                        else
                        {

                            this.Turno_Default();
                            this.Mostrar_Mensaje(1, string.Format("<b>Error! No se pudo encontrar fecha de salida para el contenedor: {0} </b>", t.ToString()));
                            return;
                        }

                        this.Actualiza_Paneles();

                    }

                }
                catch (Exception ex)
                {
                    this.Mostrar_Mensaje(1, string.Format("<b>Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0} </b>", ex.Message));
                    return;

                }
            }

        }

        protected void tablePagination_ItemDataBound(object source, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                CheckBox Chk = e.Item.FindControl("chkPase") as CheckBox;
                Label Estado = e.Item.FindControl("LblEstado") as Label;
                Button Btn = e.Item.FindControl("BtnActualizar") as Button;
                if (Estado.Text.Equals("NO"))
                {
                    Chk.Enabled = false;
                    Btn.Attributes["disabled"] = "disabled";
                }
            }
        }

        #endregion

        #endregion


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

            this.banmsg.Visible = IsPostBack;
            this.banmsg_det.Visible = IsPostBack;

            if (!Page.IsPostBack)
            {
                this.banmsg.InnerText = string.Empty;
                this.banmsg_det.InnerText = string.Empty;
                
            }

            ClsUsuario = Page.Tracker();
            if (ClsUsuario != null)
            {
                if (!Page.IsPostBack)
                {
                    this.Limpia_Campos();
                }
                   
            }

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                

                if (Response.IsClientConnected)
                {
                   
                    //banmsg.Visible = false;
                }

                Server.HtmlEncode(this.TXTMRN.Text.Trim());
                Server.HtmlEncode(this.TXTMSN.Text.Trim());
                Server.HtmlEncode(this.TXTHSN.Text.Trim());
                Server.HtmlEncode(this.Txtempresa.Text.Trim());
                Server.HtmlEncode(this.TxtChofer.Text.Trim());
                Server.HtmlEncode(this.TxtPlaca.Text.Trim());
                Server.HtmlEncode(this.TxtFechaHasta.Text.Trim());
                Server.HtmlEncode(this.TxtContenedorSeleccionado.Text.Trim());

                if (!Page.IsPostBack)
                {
                    this.Crear_Sesion();
                }

            }
            catch (Exception ex)
            {
                this.Mostrar_Mensaje(1, string.Format("<b>Error! </b>{0}",ex.Message));
            }
        }


   
     
    }

}