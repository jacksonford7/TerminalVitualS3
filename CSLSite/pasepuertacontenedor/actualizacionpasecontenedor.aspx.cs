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
  

    public partial class actualizacionpasecontenedor : System.Web.UI.Page
    {

        #region "Clases"

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
        private DateTime FechaModSalida;
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
            UPFECHAANTTURNO.Update();
            UPHORAANTTURNO.Update();

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
            this.TxtHoraAntturno.Text = string.Empty;
            this.TxtFechaAntTurno.Text = string.Empty;

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
            Session["ActuPaseContenedor" + this.hf_BrowserWindowName.Value] = objPaseContenedor;
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
                objPaseContenedor = Session["ActuPaseContenedor" + this.hf_BrowserWindowName.Value] as Cls_Bil_PasePuertaContenedor_Cabecera;
                var Detalle = objPaseContenedor.Detalle.FirstOrDefault(f => f.CONTENEDOR.Equals(ContenedorSelec));
                if (Detalle != null)
                {
                    //si es desaduanamiento directo
                    if (Detalle.CNTR_DD)
                    {
                        this.Turno_Default();
                       

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
                        var FechaCas = N4.Importacion.container.ObtenerFechaCas(Int64.Parse(Detalle.ID_CARGA.ToString()), ClsUsuario.loginname);
                        if (FechaCas.Exitoso)
                        {
                            DateTime? Cas = FechaCas.Resultado;
                            if (FechaActualSalida.Date > Cas.Value)
                            {
                                this.Turno_Default();
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

                        this.CboTurnos.Focus();
                        this.Actualiza_Paneles();
                        return;
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
                        var FechaCas = N4.Importacion.container.ObtenerFechaCas(Int64.Parse(Detalle.ID_CARGA.ToString()), ClsUsuario.loginname);
                        if (FechaCas.Exitoso)
                        {
                            DateTime? Cas = FechaCas.Resultado;
                            if (FechaActualSalida.Date > Cas.Value)
                            {
                                this.Turno_Default();
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


                        var Turnos = PasePuerta.TurnoVBS.ObtenerTurnos(ClsUsuario.ruc, ContenedorSelec, Int64.Parse(Detalle.ID_CARGA.ToString()), FechaActualSalida);
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

                string IdEmpresa = string.Empty;
                string DesEmpresa = string.Empty;
                string IdChofer = string.Empty;
                string DesChofer = string.Empty;
                EmpresaSelect = string.Empty;
                ChoferSelect = string.Empty;
                PlacaSelect = string.Empty;

                List_Turnos = new List<Cls_Bil_Turnos>();
                List_Turnos.Clear();

                if (string.IsNullOrEmpty(ContenedorSelec))
                {
                    this.Mostrar_Mensaje(2, string.Format("<b>Informativo! Debe seleccionar el contenedor para poder agregar la información </b>"));
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

                    TurnoSelect = this.CboTurnos.SelectedValue;

                }
                else//DD
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

                    TurnoSelect = this.CboTurnos.SelectedValue;
                }


                var ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());
                //valida que exista Empresa Transporte 
                if (!string.IsNullOrEmpty(Txtempresa.Text))
                {
                    EmpresaSelect = this.Txtempresa.Text.Trim();
                    if (EmpresaSelect.Split('-').ToList().Count > 1)
                    {
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


               
                //si modifica turno
                if (this.CboTurnos.SelectedIndex !=0)
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
                objPaseContenedor = Session["ActuPaseContenedor" + this.hf_BrowserWindowName.Value] as Cls_Bil_PasePuertaContenedor_Cabecera;
                var Detalle = objPaseContenedor.Detalle.FirstOrDefault(f => f.CONTENEDOR.Equals(ContenedorSelec));
                if (Detalle != null)
                {

                    if (TxtDesaduanamiento.Text.Equals("NO"))
                    {
                        //si modifica fecha del turno actual
                        if (FechaActualSalida.Date != Detalle.FECHA_EXPIRACION.Value.Date)
                        {
                            //si no selecciona turno
                            if (this.CboTurnos.SelectedIndex == 0)
                            {
                                this.Mostrar_Mensaje(2, string.Format("<b>Informativo! Debe seleccionar un nuevo turno </b>"));
                                this.CboTurnos.Focus();
                                return;
                            }
                        }

                    }

                    Detalle.VISTO = true;

                    if (!string.IsNullOrEmpty(EmpresaSelect)) {
                        Detalle.CIATRANS = EmpresaSelect;
                        Detalle.CIATRASNSP = EmpresaSelect;
                        Detalle.ID_CIATRANS = IdEmpresa;
                        Detalle.ID_EMPRESA = IdEmpresa;
                        Detalle.TRANSPORTISTA_DESC = DesEmpresa;
                    }
                    if (!string.IsNullOrEmpty(ChoferSelect))
                    {
                        Detalle.CHOFER = ChoferSelect;
                        Detalle.CONDUCTOR = ChoferSelect;
                        Detalle.ID_CHOFER = IdChofer;
                        Detalle.CHOFER_DESC = DesChofer;
                    }

                    if (!string.IsNullOrEmpty(PlacaSelect))
                    {
                        Detalle.PLACA = PlacaSelect;
                    }

                    //desaduanamiento directo no lleva turno
                    if (Detalle.CNTR_DD)
                    {
                        Detalle.D_TURNO = string.Empty;
                        Detalle.TURNO = 0;
                        Detalle.ID_TURNO = 0;
                        Detalle.TURNO_DESDE = null;
                        Detalle.TURNO_HASTA = null;
                       // Detalle.FECHA_SALIDA_PASE = null;
                        Detalle.FECHA_SALIDA_PASE = FechaActualSalida;

                        Detalle.FECHA_TURNO_NEW = null;
                        Detalle.HORA_TURNO_NEW = null;
                        Detalle.ID_PLAN_NEW = null;
                        Detalle.ID_SECUENCIA_NEW = null;

                        HoraHasta = "23:59";
                        Fecha = string.Format("{0} {1}", this.TxtFechaHasta.Text.Trim(), HoraHasta);
                        if (!DateTime.TryParseExact(Fecha, "MM-dd-yyyy HH:mm", enUS, DateTimeStyles.AdjustToUniversal, out FechaModSalida))
                        {
                            this.Turno_Default();
                            this.Mostrar_Mensaje(2, string.Format("<b>Informativo! Debe seleccionar una fecha valida del turno actual, para poder agregar la información, Mes/Día/año </b>"));
                            this.TxtFechaHasta.Focus();
                            return;
                        }

                        Detalle.FECHA_EXPIRACION = FechaModSalida;

                        //valida cas
                        var FechaCas = N4.Importacion.container.ObtenerFechaCas(Int64.Parse(Detalle.ID_CARGA.ToString()), ClsUsuario.loginname);
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
                        }
                        else
                        {
                            this.Turno_Default();
                            this.TxtFechaHasta.Text = Detalle.FECHA_SALIDA.Value.ToString("MM-dd-yyyy");
                            this.Mostrar_Mensaje(2, string.Format("<b>Error! No se pudo encontrar fecha de CAS para el contenedor: {0}, Aún no se ha ingresado la fecha de la CAS </b>", Detalle.CONTENEDOR));
                            this.CboTurnos.Focus();
                            return;
                        }


                        if (FechaActualSalida.Date > Detalle.FACTURADO_HASTA.Value)
                        {
                            this.Turno_Default();
                            this.TxtFechaHasta.Text = Detalle.FACTURADO_HASTA.Value.ToString("MM-dd-yyyy");
                            this.Mostrar_Mensaje(2, string.Format("<b>Informativo! La fecha de salida: {0}, no puede ser mayor que la fecha tope de facturación: {1} </b>", FechaActualSalida.ToString("MM-dd-yyyy"), Detalle.FACTURADO_HASTA.Value.ToString("MM-dd-yyyy")));
                            this.TxtFechaHasta.Focus();
                            return;
                        }

                    }
                    else
                    {
                        
                        //si modifica turno
                        if (this.CboTurnos.SelectedIndex != 0)
                        {

                            Detalle.D_TURNO = this.CboTurnos.SelectedItem.ToString();
                            Detalle.TURNO = Convert.ToInt64(TurnoSelect.Split('-').ToList()[0].Trim());
                            Detalle.ID_TURNO = Convert.ToInt16(TurnoSelect.Split('-').ToList()[1].Trim());
                            Detalle.TURNO_DESDE = FechaTurnoInicio;
                            Detalle.TURNO_HASTA = FechaTurnoFinal;
                            Detalle.FECHA_SALIDA_PASE = FechaActualSalida;

                            Detalle.FECHA_TURNO_NEW = FechaActualSalida;
                            Detalle.HORA_TURNO_NEW = this.CboTurnos.SelectedItem.ToString();
                            Detalle.ID_PLAN_NEW = Convert.ToInt32(TurnoSelect.Split('-').ToList()[0].Trim());
                            Detalle.ID_SECUENCIA_NEW = Convert.ToInt32(TurnoSelect.Split('-').ToList()[1].Trim());

                            HoraHasta = this.CboTurnos.SelectedItem.ToString();
                            Fecha = string.Format("{0} {1}", this.TxtFechaHasta.Text.Trim(), HoraHasta);
                            if (!DateTime.TryParseExact(Fecha, "MM-dd-yyyy HH:mm", enUS, DateTimeStyles.AdjustToUniversal, out FechaModSalida))
                            {
                                this.Turno_Default();
                                this.Mostrar_Mensaje(2, string.Format("<b>Informativo! Debe seleccionar una fecha valida del turno actual, para poder agregar la información, Mes/Día/año </b>"));
                                this.TxtFechaHasta.Focus();
                                return;
                            }

                            Detalle.FECHA_EXPIRACION = FechaModSalida;
                            Detalle.HTURNO = this.CboTurnos.SelectedItem.ToString();

                            //valida cas
                            var FechaCas = N4.Importacion.container.ObtenerFechaCas(Int64.Parse(Detalle.ID_CARGA.ToString()), ClsUsuario.loginname);
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
                            }
                            else
                            {
                                this.Turno_Default();
                                this.TxtFechaHasta.Text = Detalle.FECHA_SALIDA.Value.ToString("MM-dd-yyyy");
                                this.Mostrar_Mensaje(2, string.Format("<b>Error! No se pudo encontrar fecha de CAS para el contenedor: {0}, Aún no se ha ingresado la fecha de la CAS </b>", Detalle.CONTENEDOR));
                                this.CboTurnos.Focus();
                                return;
                            }


                            if (FechaActualSalida.Date > Detalle.FACTURADO_HASTA.Value)
                            {
                                this.Turno_Default();
                                this.TxtFechaHasta.Text = Detalle.FACTURADO_HASTA.Value.ToString("MM-dd-yyyy");
                                this.Mostrar_Mensaje(2, string.Format("<b>Informativo! La fecha de salida: {0}, no puede ser mayor que la fecha tope de facturación: {1} </b>", FechaActualSalida.ToString("MM-dd-yyyy"), Detalle.FACTURADO_HASTA.Value.ToString("MM-dd-yyyy")));
                                this.TxtFechaHasta.Focus();
                                return;
                            }

                            //reserva turno
                            int ID_CNTR = Int32.Parse(Detalle.ID_CARGA.ToString());
                            int TURNO = Detalle.ID_PLAN_NEW.Value;
                            int ID_TURNO = Detalle.ID_SECUENCIA_NEW.Value;

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
                        else
                        {

                            Detalle.FECHA_TURNO_NEW = null;
                            Detalle.HORA_TURNO_NEW = null;
                            Detalle.ID_PLAN_NEW = null;
                            Detalle.ID_SECUENCIA_NEW = null;
                        }

                       
                    }

                    /*Detalle.ID_CIATRANS = IdEmpresa;
                    Detalle.ID_EMPRESA = IdEmpresa;
                    Detalle.ID_CHOFER = IdChofer;
                    Detalle.TRANSPORTISTA_DESC = DesEmpresa;
                    Detalle.CHOFER_DESC = DesChofer;*/

                }

                tablePagination.DataSource = objPaseContenedor.Detalle;
                tablePagination.DataBind();

                Session["ActuPaseContenedor" + this.hf_BrowserWindowName.Value] = objPaseContenedor;

                //limpiar datos seleccionados
                List_Turnos = new List<Cls_Bil_Turnos>();
                List_Turnos.Clear();

                this.TxtFechaHasta.Text = String.Empty;
                this.TxtContenedorSeleccionado.Text = string.Empty;
                this.TxtFechaAntTurno.Text = string.Empty;
                this.TxtHoraAntturno.Text = string.Empty;
                this.TxtDesaduanamiento.Text = string.Empty;
                this.TxtFechaCas.Text = string.Empty;
                this.Turno_Default();
                this.Pintar_Grilla();
                this.Actualiza_Paneles();

            }
            catch (Exception ex)
            {
                this.Mostrar_Mensaje(2, string.Format("<b>Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible..{0} </b>", ex.Message));

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

                    //limpio temporales controles
                    this.hf_chofer.Value = string.Empty;
                    this.hf_transportista.Value = string.Empty;
                    this.hf_placa.Value = string.Empty;
                    this.TxtChofer.Text = string.Empty;
                    this.Txtempresa.Text = string.Empty;
                    this.TxtPlaca.Text = string.Empty;
                    this.TxtFechaCas.Text = string.Empty;
                    this.TxtDesaduanamiento.Text = string.Empty;
                    this.TxtFechaAntTurno.Text = string.Empty;
                    this.TxtHoraAntturno.Text = string.Empty;
                    this.IdTxtChofer.Value = string.Empty;
                    this.IdTxtempresa.Value = string.Empty;
                    this.IdTxtPlaca.Value = string.Empty;

                    //ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "AsignaTransporte('" + string.Empty + "');", true);
                    //ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "AsignaChofer('" + string.Empty + "');", true);
                    //ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "AsignaPlaca('" + string.Empty + "');", true);
                    //TINICIA = (p.Field<string>("TINICIA") == null ? null : p.Field<string>("TINICIA")),
                    //                    TFIN = (p.Field<string>("TFIN") == null ? null : p.Field<string>("TFIN")),
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
                    var Contenedores = PasePuerta.Pase_Container.ObtenerListaEditable(this.TXTMRN.Text.Trim(), this.TXTMSN.Text.Trim(), this.TXTHSN.Text.Trim(), ClsUsuario.ruc, ClsUsuario.loginname,null);
                    if (Contenedores.Exitoso)
                    {
                        /*********************************************NUEVA VALIDACION************************************************/
                        /*estado de la unidad*/
                        List<Int64> Lista = new List<Int64>();
                        var LinqGkey = (from p in Contenedores.Resultado.AsEnumerable()
                                       where !string.IsNullOrEmpty(p.Field<string>("CONTENEDOR"))
                                       select new
                                       {
                                           GKEY = p.Field<Decimal>("ID_CARGA")
                                       }).Distinct();

                        if (LinqGkey != null && LinqGkey.Count() > 0)
                        {
                            foreach (var Det in LinqGkey)
                            {
                                Lista.Add(Int64.Parse(Det.GKEY.ToString()));
                            }
                        }
                        var EstadoUnidad = N4.Importacion.container.ValidarEstadoContenedor(Lista, ClsUsuario.loginname.Trim());

                        
                        var LinqUnidades = from p in EstadoUnidad.Resultado.AsEnumerable()
                                           where p.Item1 != 0
                                           select new {
                                            ID_CARGA = p.Item1,
                                            UBICACION = p.Item2,
                                            MENSAJE = p.Item3,
                                            ESTADO = p.Item4
                        };

                        /*********************************************FIN NUEVA VALIDACION************************************************/
                        
                        var LinqQuery = (from p in Contenedores.Resultado.AsEnumerable()
                                         join c in LinqUnidades on Int64.Parse(p.Field<Decimal>("ID_CARGA").ToString()) equals c.ID_CARGA into TmpFinal
                                         from Final in TmpFinal.DefaultIfEmpty()
                                         where !string.IsNullOrEmpty(p.Field<string>("CONTENEDOR"))
                                    select new
                                    {
                                        ID_PASE = p.Field<Decimal>("ID_PASE"),
                                        MRN = p.Field<string>("MRN") == null ? "" : p.Field<string>("MRN").Trim(),
                                        MSN = p.Field<string>("MSN") == null ? "" : p.Field<string>("MSN").Trim(),
                                        HSN = p.Field<string>("HSN") == null ? "" : p.Field<string>("HSN").Trim(),
                                        CONTENEDOR = p.Field<string>("CONTENEDOR") == null ? "" : p.Field<string>("CONTENEDOR").Trim(),
                                        FACTURADO_HASTA = p.Field<DateTime?>("FACTURADO_HASTA"),
                                        TIPO = p.Field<string>("TIPO") == null ? "" : p.Field<string>("TIPO").Trim(),
                                        FECHA_TURNO = p.Field<DateTime?>("FECHA_TURNO"),
                                        HTURNO = p.Field<string>("HTURNO") == null ? "" : p.Field<string>("HTURNO").Trim(),
                                        CIATRASNSP = p.Field<string>("CIATRASNSP") == null ? "" : p.Field<string>("CIATRASNSP").Trim(),
                                        CONDUCTOR = p.Field<string>("CONDUCTOR") == null ? "" : p.Field<string>("CONDUCTOR").Trim(),
                                        PLACA = p.Field<string>("PLACA") == null ? "" : p.Field<string>("PLACA").Trim(),
                                        ID_CHOFER = p.Field<string>("ID_CHOFER") == null ? "" : p.Field<string>("ID_CHOFER").Trim(),
                                        ID_EMPRESA = p.Field<string>("ID_EMPRESA") == null ? "" : p.Field<string>("ID_EMPRESA").Trim(),
                                        ID_PLAN = p.Field<Int64?>("ID_PLAN"),
                                        ID_SECUENCIA = p.Field<Int32?>("ID_SECUENCIA"),
                                        NUMERO_PASE_N4 = p.Field<string>("NUMERO_PASE_N4") == null ? "" : p.Field<string>("NUMERO_PASE_N4").Trim(),
                                        ID_CARGA = p.Field<Decimal>("ID_CARGA") ,
                                        FECHA_EXPIRACION = p.Field<DateTime?>("FECHA_EXPIRACION"),
                                        TINICIA = p.Field<DateTime?>("TINICIA"),
                                        TFIN = p.Field<DateTime?>("TFIN"),
                                        TID = p.Field<Int64?>("TID"),
                                        AGENTE = p.Field<string>("AGENTE") == null ? "" : p.Field<string>("AGENTE").Trim(),
                                        CNTR_DD = p.Field<bool?>("CNTR_DD") == null ? false : p.Field<bool?>("CNTR_DD"),
                                        ESTADO = p.Field<string>("ESTADO") == null ? "" : p.Field<string>("ESTADO").Trim(),
                                        SERVICIO = p.Field<bool?>("SERVICIO") == null ? false : p.Field<bool?>("SERVICIO"),
                                        IN_OUT = (Final == null) ? string.Empty : Final.UBICACION,
                                        PATIO = (Final == null) ? string.Empty : Final.MENSAJE
                                    });
                                    

                        if (LinqQuery != null && LinqQuery.Count() > 0)
                        {
                            //agrego todos los contenedores a la clase cabecera
                            objPaseContenedor = Session["ActuPaseContenedor" + this.hf_BrowserWindowName.Value] as Cls_Bil_PasePuertaContenedor_Cabecera;
                            objPaseContenedor.FECHA = DateTime.Now;
                            objPaseContenedor.MRN = this.TXTMRN.Text;
                            objPaseContenedor.MSN = this.TXTMSN.Text;
                            objPaseContenedor.HSN = this.TXTHSN.Text;
                            objPaseContenedor.IV_USUARIO_CREA = ClsUsuario.loginname;
                            objPaseContenedor.SESION = this.hf_BrowserWindowName.Value;
                            objPaseContenedor.Detalle.Clear();

                            Int64 pValor = 0;

                            foreach (var Det in LinqQuery)
                            {

                                List<Cls_Bil_PasePuertaContenedor_InOut> PaseUsado = Cls_Bil_PasePuertaContenedor_InOut.Pase_Utilizado(Int64.Parse(Det.ID_CARGA.ToString()), out cMensajes);

                                if (PaseUsado != null)
                                {
                                    pValor = (from Tbl in PaseUsado select new { VALOR = (Tbl.VALOR == 0) ? 0 : Tbl.VALOR }).FirstOrDefault().VALOR;
                                }
                                else { pValor = 0; }


                                objDetallePaseContenedor = new Cls_Bil_PasePuertaContenedor_Detalle();
                                objDetallePaseContenedor.ID_PASE = (Int64)Det.ID_PASE;
                                objDetallePaseContenedor.MRN = Det.MRN;
                                objDetallePaseContenedor.MSN = Det.MSN;
                                objDetallePaseContenedor.HSN = Det.HSN;
                                objDetallePaseContenedor.CARGA = string.Format("{0}-{1}-{2}", Det.MRN, Det.MSN, Det.HSN);
                                objDetallePaseContenedor.CONTENEDOR = Det.CONTENEDOR;
                                objDetallePaseContenedor.FACTURADO_HASTA = Det.FACTURADO_HASTA;
                                objDetallePaseContenedor.TIPO = Det.TIPO;
                                objDetallePaseContenedor.TIPO_CNTR = Det.TIPO;
                                objDetallePaseContenedor.FECHA_TURNO = Det.FECHA_TURNO;
                                objDetallePaseContenedor.HTURNO = Det.HTURNO;
                                objDetallePaseContenedor.CIATRASNSP = string.IsNullOrEmpty(Det.CIATRASNSP) ?  string.Empty : string.Format("{0} - {1}", Det.ID_EMPRESA, Det.CIATRASNSP);
                                objDetallePaseContenedor.CONDUCTOR = string.IsNullOrEmpty(Det.CONDUCTOR) ? string.Empty :  string.Format("{0} - {1}", Det.ID_CHOFER, Det.CONDUCTOR);

                                if (Det.PLACA == null)
                                {
                                    objDetallePaseContenedor.PLACA = string.Empty;
                                }
                                else
                                {
                                    objDetallePaseContenedor.PLACA =  Det.PLACA;
                                }
                                objDetallePaseContenedor.PLACA = string.IsNullOrEmpty(Det.PLACA) ? string.Empty :  (Det.PLACA==null ? string.Empty : Det.PLACA);
                                objDetallePaseContenedor.ID_CHOFER = Det.ID_CHOFER;
                                objDetallePaseContenedor.CHOFER_DESC = Det.CONDUCTOR;
                                objDetallePaseContenedor.ID_EMPRESA = Det.ID_EMPRESA;
                                objDetallePaseContenedor.ID_CIATRANS = Det.ID_EMPRESA;
                                objDetallePaseContenedor.TRANSPORTISTA_DESC = Det.CIATRASNSP;
                                objDetallePaseContenedor.ID_PLAN = Det.ID_PLAN;
                                objDetallePaseContenedor.ID_SECUENCIA = Det.ID_SECUENCIA;
                                objDetallePaseContenedor.NUMERO_PASE_N4 = (string.IsNullOrEmpty(Det.NUMERO_PASE_N4) ? 0 : double.Parse(Det.NUMERO_PASE_N4));
                                objDetallePaseContenedor.ID_CARGA = Det.ID_CARGA;
                                objDetallePaseContenedor.FECHA_EXPIRACION = Det.FECHA_EXPIRACION;
                                objDetallePaseContenedor.FECHA_SALIDA_PASE = Det.FECHA_EXPIRACION;
                                objDetallePaseContenedor.TINICIA = (Det.TINICIA.HasValue ? Det.TINICIA : null) ;
                                objDetallePaseContenedor.TFIN = (Det.TFIN.HasValue ? Det.TFIN : null);
                                objDetallePaseContenedor.TID = Det.TID;
                                objDetallePaseContenedor.AGENTE = Det.AGENTE;
                                objDetallePaseContenedor.USUARIO_MOD = ClsUsuario.loginname;
                                objDetallePaseContenedor.ID_TURNO = Det.ID_SECUENCIA;
                                objDetallePaseContenedor.TURNO = Det.ID_PLAN;
                                objDetallePaseContenedor.D_TURNO = Det.HTURNO;
                                objDetallePaseContenedor.CNTR_DD = Det.CNTR_DD.Value ;
                                objDetallePaseContenedor.FECHA_SALIDA = Det.FACTURADO_HASTA;
                                objDetallePaseContenedor.ESTADO = (Det.ESTADO.Equals("GN") ? "GENERADO" : "EXPIRADO");
                                objDetallePaseContenedor.MOSTRAR_MENSAJE = (Det.ESTADO.Equals("GN") ? "GENERADO" : "EXPIRADO");
                                objDetallePaseContenedor.ORDENAMIENTO = (Det.ESTADO.Equals("GN") ? "1" : "2");
                                objDetallePaseContenedor.IN_OUT = Det.IN_OUT;

                                if (pValor == 1)
                                {
                                    objDetallePaseContenedor.MOSTRAR_MENSAJE = string.Format("{0} - {1} - Unidad: {2}", objDetallePaseContenedor.ESTADO, "PASE UTILIZADO", Det.PATIO);
                                    objDetallePaseContenedor.ORDENAMIENTO = string.Format("{0}-{1}", objDetallePaseContenedor.ORDENAMIENTO, "1");
                                }
                                else
                                {
                                    objDetallePaseContenedor.MOSTRAR_MENSAJE = string.Format("{0} - {1} - Unidad: {2}", objDetallePaseContenedor.ESTADO, "SIN USAR", Det.PATIO);
                                    objDetallePaseContenedor.ORDENAMIENTO = string.Format("{0}-{1}", objDetallePaseContenedor.ORDENAMIENTO, "2");
                                }



                                objDetallePaseContenedor.SERVICIO = Det.SERVICIO;
                                //desaduanamiento directo 
                                if (Det.CNTR_DD.Value)
                                {
                                   // objDetallePaseContenedor.FECHA_SALIDA_PASE = null;
                                    objDetallePaseContenedor.FECHA_SALIDA_PASE = Det.FECHA_EXPIRACION;
                                }
                                else
                                {
                                    objDetallePaseContenedor.FECHA_SALIDA_PASE = Det.FECHA_EXPIRACION;
                                }
                                //si no tiene nada pendiente de facturar
                                if (!objDetallePaseContenedor.SERVICIO.Value)
                                {
                                    objPaseContenedor.Detalle.Add(objDetallePaseContenedor);
                                }
                                

                            
                            }

                            tablePagination.DataSource = objPaseContenedor.Detalle.Where(x => x.SERVICIO == false).OrderBy(p => p.ORDENAMIENTO);
                            tablePagination.DataBind();

                            Session["ActuPaseContenedor" + this.hf_BrowserWindowName.Value] = objPaseContenedor;

                            this.Pintar_Grilla();
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
                objPaseContenedor = Session["ActuPaseContenedor" + this.hf_BrowserWindowName.Value] as Cls_Bil_PasePuertaContenedor_Cabecera;
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

                /*VALIDACION NUEVA, VEHICULO ESTA FUERA 20-05-2020*/
                /*estado de la unidad*/
                List<Int64> Lista = new List<Int64>();
                var LinqGkey = (from p in objPaseContenedor.Detalle.Where(x => x.VISTO == true)
                                select new { GKEY = Int64.Parse(p.ID_CARGA.ToString()),
                                    CONTENEDOR = p.CONTENEDOR }).ToList();
               

                if (LinqGkey.Count() > 0)
                {
                    foreach (var Det in LinqGkey)
                    {
                        Lista.Add(Det.GKEY);
                    }
                }

                var EstadoUnidad = N4.Importacion.container.ValidarEstadoContenedor(Lista, LoginName);
                var LinqUnidades =(from p in EstadoUnidad.Resultado.AsEnumerable()
                                   join c in LinqGkey on p.Item1 equals c.GKEY into TmpFinal
                                   from Final in TmpFinal.DefaultIfEmpty()
                                   where p.Item1 != 0
                                   select new
                                   {
                                       ID_CARGA = p.Item1,
                                       UBICACION = p.Item2,
                                       MENSAJE = p.Item3,
                                       ESTADO = p.Item4,
                                       CONTENEDOR =  (Final == null) ? string.Empty : Final.CONTENEDOR
                                   });
                foreach (var Det in LinqUnidades)
                {
                    if (!Det.ESTADO)
                    {
                        this.Mostrar_Mensaje(1, string.Format("<b>Informativo! No se puede actualizar el contenedor:{0}, la unidad tiene estado: {1}, desmarque la unidad, para continuar con el proceso.. </b>", Det.CONTENEDOR, Det.MENSAJE));
                        return;
                    }
                }
                /**********************FIN VALIDACION*************************************/


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
                                     CHOFER = (Tbl.CHOFER == null) ? null : Tbl.CHOFER,
                                     PLACA = (Tbl.PLACA == null) ? null : Tbl.PLACA,
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
                                     ID_CHOFER = (Tbl.ID_CHOFER == null) ? null : Tbl.ID_CHOFER,
                                     ID_CIATRANS = (Tbl.ID_CIATRANS == null) ? null : Tbl.ID_CIATRANS,
                                     USUARIO = Tbl.USUARIO_ING,
                                     TURNO_DESDE = (Tbl.TURNO_DESDE.HasValue ? Tbl.TURNO_DESDE : null),
                                     TURNO_HASTA = (Tbl.TURNO_HASTA.HasValue ? Tbl.TURNO_HASTA : null) ,
                                     CNTR_DD = Tbl.CNTR_DD,
                                     AGENTE_DESC = (Tbl.AGENTE_DESC == null) ? null : Tbl.AGENTE_DESC ,
                                     FACTURADO_DESC = (Tbl.FACTURADO_DESC == null) ? string.Empty : Tbl.FACTURADO_DESC,
                                     IMPORTADOR = (Tbl.IMPORTADOR == null) ? null: Tbl.IMPORTADOR,
                                     IMPORTADOR_DESC =  (Tbl.IMPORTADOR_DESC == null) ? null : Tbl.IMPORTADOR_DESC,
                                     TRANSPORTISTA_DESC = (Tbl.TRANSPORTISTA_DESC == null) ?null : Tbl.TRANSPORTISTA_DESC,
                                     CHOFER_DESC = (Tbl.CHOFER_DESC == null) ? null : Tbl.CHOFER_DESC,
                                     FECHA_TURNO_NEW = (Tbl.FECHA_TURNO_NEW.HasValue ? Tbl.FECHA_TURNO_NEW : null),
                                     HORA_TURNO_NEW = (Tbl.HORA_TURNO_NEW == null) ? null : Tbl.HORA_TURNO_NEW,
                                     ID_PLAN_NEW = (Tbl.ID_PLAN_NEW == null) ? null : Tbl.ID_PLAN_NEW,
                                     ID_SECUENCIA_NEW  = (Tbl.ID_SECUENCIA_NEW == null) ? null : Tbl.ID_SECUENCIA_NEW,
                                     ID_CARGA = (Tbl.ID_CARGA == 0) ? 0 : Tbl.ID_CARGA,
                                     FECHA_EXPIRACION = (Tbl.FECHA_EXPIRACION.HasValue ? Tbl.FECHA_EXPIRACION : null),
                                     NUMERO_PASE_N4 = (Tbl.NUMERO_PASE_N4 == null ? 0 : Tbl.NUMERO_PASE_N4),
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
                        this.IdTxtempresa.Focus();
                        return;
                    }

                }
                /***********************************************************************************************************************************************
                *generar pase puerta
                **********************************************************************************************************************************************/
                this.BtnGrabar.Attributes["disabled"] = "disabled";
                this.UPBOTONES.Update();

                var ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());
                int nTotal = 0;
                foreach (var Det in LinqQuery)
                {

                    Pase_Container pase = new Pase_Container();
                    pase.ID_CARGA = Det.ID_CARGA;
                    pase.ESTADO = "GN";
                    pase.NUMERO_PASE_N4 = Det.NUMERO_PASE_N4.ToString();
                    pase.NUMERO_PASE_N4_ANTERIOR = Det.NUMERO_PASE_N4.ToString();
                    //validaciones desaduanamiento directo 
                    if (!Det.CNTR_DD)
                    {
                        pase.FECHA_EXPIRACION = Det.FECHA_EXPIRACION;
                    }
                    else
                    {
                        pase.FECHA_EXPIRACION = Det.FECHA_EXPIRACION;
                    }
                    pase.ID_PASE = Decimal.Parse(Det.ID_PASE.ToString());
                    pase.ID_PLACA = (string.IsNullOrEmpty(Det.PLACA) ? null : Det.PLACA);
                    
                    pase.ID_CHOFER = (string.IsNullOrEmpty(Det.ID_CHOFER) ? null : Det.ID_CHOFER);
                    pase.ID_EMPRESA = (string.IsNullOrEmpty(Det.ID_CIATRANS) ? null : Det.ID_CIATRANS);
                    pase.CANTIDAD_CARGA = 0;
                    pase.USUARIO_REGISTRO = ClsUsuario.loginname;
                    pase.TIPO_CARGA = "CNTR";
                    pase.TINICIA = Det.TURNO_DESDE;
                    pase.TFIN = Det.TURNO_HASTA;
                    pase.TID = Det.TURNO.Value;
                    pase.CONSIGNATARIO_ID = (string.IsNullOrEmpty(Det.IMPORTADOR) ? null : Det.IMPORTADOR);
                    pase.CONSIGNARIO_NOMBRE = (string.IsNullOrEmpty(Det.IMPORTADOR_DESC) ? null : Det.IMPORTADOR_DESC);
                    pase.TRANSPORTISTA_DESC = (string.IsNullOrEmpty(Det.TRANSPORTISTA_DESC) ? null : Det.TRANSPORTISTA_DESC);
                    

                    pase.CHOFER_DESC = (string.IsNullOrEmpty(Det.CHOFER_DESC) ? null : Det.CHOFER_DESC);

                    DateTime? Fecha = (Det.FECHA_TURNO_NEW.HasValue ? Det.FECHA_TURNO_NEW : null);
                    int? ID_PLAN_NEW = (Det.ID_PLAN_NEW == null ? null : Det.ID_PLAN_NEW);
                    int? ID_SECUENCIA_NEW = (Det.ID_SECUENCIA_NEW == null ? null : Det.ID_SECUENCIA_NEW);
                    string HORA_TURNO_NEW =(Det.HORA_TURNO_NEW == null ? null : Det.HORA_TURNO_NEW);

                    var Resultado = pase.Actualizar(ClsUsuario.loginname ,Det.CONTENEDOR, Fecha, HORA_TURNO_NEW, ID_PLAN_NEW, ID_SECUENCIA_NEW);
                    if (Resultado.Exitoso)
                    {
                        nTotal++;
                        if (nTotal == 1) {
                            id_carga = securetext(Det.CARGA);
                        }
                    }
                    else
                    {
                        this.Mostrar_Mensaje(2, string.Format("<b>Error! Se presentaron los siguientes problemas, no se pudo generar el pase de puerta para el contenedor: {0}, Existen los siguientes problemas: {1}, {2} </b>", Det.CONTENEDOR, Resultado.MensajeInformacion, Resultado.MensajeProblema));
                        return;
                    }

                }

                if (nTotal !=0)
                {
                    string link = string.Format("<a href='../pasepuertacontenedor/imprimirpasecontenedor.aspx?id_carga={0}' target ='_parent'>Imprimir Pase Puerta</a>", id_carga);
                   
                    //limpiar
                    objPaseContenedor.Detalle.Clear();
                    Session["ActuPaseContenedor" + this.hf_BrowserWindowName.Value] = objPaseContenedor;
                    tablePagination.DataSource = objPaseContenedor.Detalle;
                    tablePagination.DataBind();

                    this.Limpia_Campos();


                    this.Mostrar_Mensaje(2, string.Format("<b>Informativo! Se procedieron con la actualización de {0} pase de puerta con éxito, para proceder a imprimir los mismo, <br/>por favor dar click en el siguiente link: {1} </b>", nTotal, link));
                    return;
                }     

               
            }
            catch (Exception ex)
            {
                this.Mostrar_Mensaje(2, string.Format("<b>Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible..{0} </b>", ex.Message));

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
                objPaseContenedor = Session["ActuPaseContenedor" + this.hf_BrowserWindowName.Value] as Cls_Bil_PasePuertaContenedor_Cabecera;
                var Detalle = objPaseContenedor.Detalle.FirstOrDefault(f => f.CONTENEDOR.Equals(ContenedorSelec));
                if (Detalle != null)
                {
                    Detalle.VISTO = chkPase.Checked;

                }

                tablePagination.DataSource = objPaseContenedor.Detalle.OrderBy(p => p.ORDENAMIENTO);
                tablePagination.DataBind();

                Session["ActuPaseContenedor" + this.hf_BrowserWindowName.Value] = objPaseContenedor;

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
                        //limpio temporales controles
                        this.hf_chofer.Value = string.Empty;
                        this.hf_transportista.Value = string.Empty;
                        this.hf_placa.Value = string.Empty;

                        objPaseContenedor = Session["ActuPaseContenedor" + this.hf_BrowserWindowName.Value] as Cls_Bil_PasePuertaContenedor_Cabecera;
                        var Detalle = objPaseContenedor.Detalle.FirstOrDefault(f => f.CONTENEDOR.Equals(t.ToString()));
                        if (Detalle != null)
                        {
                            Tipo_Contenedor = Detalle.TIPO_CNTR;

                            //desaduanamiento directo no lleva turnos
                            if (Detalle.CNTR_DD)
                            {
                                /*this.TxtDesaduanamiento.Text = "SI";
                                this.TxtFechaHasta.Text = string.Empty;
                                this.TxtFechaAntTurno.Text = string.Empty;
                                this.TxtHoraAntturno.Text = string.Empty;*/
                                FechaFacturaHasta = Detalle.FECHA_SALIDA_PASE.Value;
                                FechaMenosHoras = FechaFacturaHasta.AddHours(-3);
                                HoraHasta = FechaMenosHoras.ToString("HH:mm");
                                this.TxtDesaduanamiento.Text = "SI";
                                this.TxtFechaHasta.Text = Detalle.FECHA_SALIDA_PASE.Value.ToString("MM-dd-yyyy");
                                this.TxtFechaAntTurno.Text = Detalle.FECHA_SALIDA_PASE.Value.ToString("MM-dd-yyyy");
                                this.TxtHoraAntturno.Text = (Detalle.HTURNO==null ? string.Empty : Detalle.HTURNO);
                            }
                            else
                            {
                                FechaFacturaHasta = Detalle.FECHA_SALIDA_PASE.Value;
                                FechaMenosHoras = FechaFacturaHasta.AddHours(-3);
                                HoraHasta = FechaMenosHoras.ToString("HH:mm");
                                this.TxtDesaduanamiento.Text = "NO";
                                this.TxtFechaHasta.Text = Detalle.FECHA_SALIDA_PASE.Value.ToString("MM-dd-yyyy");
                                this.TxtFechaAntTurno.Text = Detalle.FECHA_SALIDA_PASE.Value.ToString("MM-dd-yyyy");
                                this.TxtHoraAntturno.Text = Detalle.HTURNO;
                            }

                            if (!string.IsNullOrEmpty(Detalle.CIATRASNSP))
                            {
                                this.Txtempresa.Text = Detalle.CIATRASNSP;
                                //this.hf_transportista.Value = Detalle.CIATRASNSP;
                                //this.IdTxtempresa.Value = Detalle.ID_CIATRANS;
                                //ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "AsignaTransporte('" + Detalle.CIATRASNSP + "');", true);
                            }
                            if (!string.IsNullOrEmpty(Detalle.CONDUCTOR))
                            {
                                this.TxtChofer.Text = Detalle.CONDUCTOR;
                                //this.hf_chofer.Value = Detalle.CONDUCTOR;
                                //this.IdTxtChofer.Value = Detalle.ID_CHOFER;
                                //ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "AsignaChofer('" + Detalle.CONDUCTOR + "');", true);
                            }
                            if (!string.IsNullOrEmpty(Detalle.PLACA))
                            {
                                this.TxtPlaca.Text = Detalle.PLACA;
                                //this.hf_placa.Value = Detalle.PLACA;
                                //this.IdTxtPlaca.Value = Detalle.PLACA;
                                //ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "AsignaPlaca('" + Detalle.PLACA + "');", true);
                            }

                            //desaduanamiento directo no lleva turnos
                            if (!Detalle.CNTR_DD)
                            {

                                //valida cas
                                var FechaCas = N4.Importacion.container.ObtenerFechaCas(Int64.Parse(Detalle.ID_CARGA.ToString()), ClsUsuario.loginname);
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


                                var Turnos = PasePuerta.TurnoVBS.ObtenerTurnos(ClsUsuario.ruc, t.ToString(), Int64.Parse(Detalle.ID_CARGA.ToString()), Detalle.FECHA_SALIDA_PASE.Value);
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
                                //this.Turno_Default();
                                //valida cas
                                var FechaCas = N4.Importacion.container.ObtenerFechaCas(Int64.Parse(Detalle.ID_CARGA.ToString()), ClsUsuario.loginname);
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
                            }

                        }
                        else
                        {

                            this.Turno_Default();
                            this.Mostrar_Mensaje(1, string.Format("<b>Error! No se pudo encontrar fecha de salida para el contenedor: {0} </b>", t.ToString()));
                            return;
                        }

                        this.Actualiza_Paneles();

                    }//fin actualizar

                    //nuevos servicios
                    if (e.CommandName == "Facturar")
                    {
                        objPaseContenedor = Session["ActuPaseContenedor" + this.hf_BrowserWindowName.Value] as Cls_Bil_PasePuertaContenedor_Cabecera;
                        var Detalle = objPaseContenedor.Detalle.FirstOrDefault(f => f.CONTENEDOR.Equals(t.ToString()));
                        if (Detalle != null)
                        {
                            var Resultado = Pase_Container.Marcar_Servicio(Detalle.USUARIO_MOD, Detalle.CONTENEDOR, Int64.Parse(Detalle.ID_PASE.Value.ToString()));
                            if (Resultado.Exitoso)
                            {
                                Detalle.SERVICIO = true;

                                tablePagination.DataSource = objPaseContenedor.Detalle.Where(x => x.SERVICIO == false).OrderBy(p => p.ORDENAMIENTO);
                                tablePagination.DataBind();

                                Session["ActuPaseContenedor" + this.hf_BrowserWindowName.Value] = objPaseContenedor;


                                string id_carga = securetext(Detalle.CARGA.Replace("-", "+"));  
                                string link = string.Format("<a href='../contenedor/contenedorimportacionadicional.aspx?ID_CARGA={0}' target ='_parent'>Facturar E-Pass Vencido</a>", id_carga);
                                this.Mostrar_Mensaje(2, string.Format("<b>Informativo! Se procedieron a generar nuevos eventos para emitir nueva factura adicional con éxito, para proceder a emitir la misma, <br/>por favor dar click en el siguiente link: {0} </b>", link));
                                 return;
                            }
                            else
                            {
                                this.Mostrar_Mensaje(2, string.Format("<b>Error! Se presentaron los siguientes problemas, no se pudo generar nuevos eventos para poder emitir nueva factura: {0}, Existen los siguientes problemas: {1}, {2} </b>", Detalle.CONTENEDOR, Resultado.MensajeInformacion, Resultado.MensajeProblema));
                                return;
                            }
                        }
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
                Label Estado2 = e.Item.FindControl("LblIn_Out") as Label;
                Button Btn = e.Item.FindControl("BtnActualizar") as Button;
                Button BtnEvento = e.Item.FindControl("BtnEvento") as Button;
                if (Estado.Text.Equals("EXPIRADO") || Estado2.Text.Equals("DEPARTED"))
                {
                    Chk.Enabled = false;
                    //Btn.Enabled = false;
                    Btn.Attributes["disabled"] = "disabled";

                    if (Estado2.Text.Equals("DEPARTED"))
                    {
                        BtnEvento.Visible = false;
                    }
                    else { BtnEvento.Visible = true; }
                    
                }
                else { BtnEvento.Visible = false; }

            }
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

                if (ChkVisto.Checked == true || LblEstado.Text == "EXPIRADO")
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