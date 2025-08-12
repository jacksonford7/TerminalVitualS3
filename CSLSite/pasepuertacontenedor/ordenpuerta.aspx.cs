using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using System.Globalization;
using BillionEntidades;
using AccesoDatos;
using SqlConexion;
using PasePuerta;
using System.Web;

namespace CSLSite
{
    public partial class ordenpuerta : System.Web.UI.Page
    {
        private static Cls_Conexion sql_puntero;
        private static Dictionary<string, object> parametros;
        private static string nueva_conexion;

        private List<Cls_Bil_PasePuertaContenedor_Detalle> Contenedores
        {
            get => Session["PaseContenedor" + hf_BrowserWindowName.Value] as List<Cls_Bil_PasePuertaContenedor_Detalle>;
            set => Session["PaseContenedor" + hf_BrowserWindowName.Value] = value;
        }

        private List<Cls_Bil_PasePuertaContenedor_Detalle> ContenedoresSeleccionados
        {
            get => Session["ContenedoresSeleccionados" + hf_BrowserWindowName.Value] as List<Cls_Bil_PasePuertaContenedor_Detalle>;
            set => Session["ContenedoresSeleccionados" + hf_BrowserWindowName.Value] = value;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                hf_BrowserWindowName.Value = Guid.NewGuid().ToString();
                Contenedores = new List<Cls_Bil_PasePuertaContenedor_Detalle>();
                ContenedoresSeleccionados = new List<Cls_Bil_PasePuertaContenedor_Detalle>();
                gvSeleccionados.DataSource = ContenedoresSeleccionados; // Inicializar
                gvSeleccionados.DataBind();
            }
        }

        private void OnInit(string baseDatos)
        {
            if (sql_puntero == null)
                sql_puntero = Cls_Conexion.Conexion();

            parametros = new Dictionary<string, object>();
            nueva_conexion = Cls_Conexion.Nueva_Conexion(baseDatos);
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            OnInit("PORTAL_BILLION");
            parametros.Add("RucImportador", txtRucImportador.Text.Trim());

            var rp = BDOpe.ComandoSelectALista<Cls_Bil_PasePuertaContenedor_Detalle>(
                nueva_conexion,
                "sp_ConsultarContenedoresDisponiblesPase",
                parametros
            );

            if (rp.Exitoso && rp.Resultado != null && rp.Resultado.Any())
            {
                Contenedores = rp.Resultado;
                gvContenedores.DataSource = rp.Resultado;
                gvContenedores.DataBind();
            }
            else
            {
                Mostrar_Mensaje(rp.MensajeProblema ?? "No se encontraron resultados.");
            }

            UPSEARCH.Update();
        }

        protected void gvContenedores_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            // Personalización de fila si se desea
        }

        protected void chkSeleccion_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chk = (CheckBox)sender;
            GridViewRow row = (GridViewRow)chk.NamingContainer;
            string cntr = row.Cells[1].Text.Trim();

            var det = Contenedores.FirstOrDefault(c => c.NumeroContenedor == cntr);
            if (det == null) return;

            if (chk.Checked)
            {
                if (!ContenedoresSeleccionados.Any(c => c.NumeroContenedor == cntr))
                    ContenedoresSeleccionados.Add(det);
            }
            else
            {
                ContenedoresSeleccionados.RemoveAll(c => c.NumeroContenedor == cntr);
            }

            gvSeleccionados.DataSource = ContenedoresSeleccionados;
            gvSeleccionados.DataBind();
            UPSeleccionados.Update(); // ✅ Asegura la actualización en pantalla
            UPSEARCH.Update();
        }


        protected void txtFechaSalida_TextChanged(object sender, EventArgs e)
        {
            try
            {
                lblMensaje.Visible = false;

                if (string.IsNullOrEmpty(txtFechaSalida.Text))
                {
                    Mostrar_Mensaje("<b>Informativo! Debe seleccionar una fecha válida para la generación del pase Mes/Día/año</b>");
                    txtFechaSalida.Focus();
                    return;
                }

                CultureInfo enUS = new CultureInfo("en-US");
                string fecha = $"{txtFechaSalida.Text.Trim()} 23:59";
                if (!DateTime.TryParseExact(fecha, "MM-dd-yyyy HH:mm", enUS, DateTimeStyles.AdjustToUniversal, out DateTime fechaSalida))
                {
                    Mostrar_Mensaje("<b>Informativo! Debe seleccionar una fecha válida para la generación del pase Mes/Día/año</b>");
                    txtFechaSalida.Focus();
                    return;
                }

                // Validar que la fecha no sea anterior a la actual
                if (fechaSalida.Date < DateTime.Now.Date)
                {
                    Mostrar_Mensaje($"<b>Informativo! La fecha de salida: {fechaSalida:MM-dd-yyyy}, no puede ser menor que la fecha actual: {DateTime.Now:MM-dd-yyyy}</b>");
                    txtFechaSalida.Focus();
                    return;
                }

                // Validar contra fecha CAS (si aplica)
                var contenedor = ContenedoresSeleccionados.FirstOrDefault();
                if (contenedor != null && contenedor.CAS.HasValue && fechaSalida.Date > contenedor.CAS.Value.Date)
                {
                    Mostrar_Mensaje($"<b>Informativo! La fecha de salida: {fechaSalida:MM-dd-yyyy}, no puede ser mayor que la fecha CAS: {contenedor.CAS.Value:MM-dd-yyyy}</b>");
                    txtFechaSalida.Focus();
                    return;
                }

                // Si necesitas manejar turnos, implementa aquí (por ejemplo, llenar un DropDownList CboTurnos)
                UPFORM.Update();
            }
            catch (Exception ex)
            {
                Mostrar_Mensaje($"<b>Error! Lo sentimos, algo salió mal: {ex.Message}</b>");
            }
        }

        protected void btnGenerar_Click(object sender, EventArgs e)
        {
            try
            {
                lblMensaje.Visible = false;

                // Validar que haya contenedores seleccionados
                if (ContenedoresSeleccionados == null || !ContenedoresSeleccionados.Any())
                {
                    Mostrar_Mensaje("<b>Informativo! Debe seleccionar al menos un contenedor para generar el e-Pass</b>");
                    return;
                }

                // Validar campos obligatorios
                if (string.IsNullOrEmpty(txtCiaTrans.Text) || string.IsNullOrEmpty(IdTxtCiaTrans.Value))
                {
                    Mostrar_Mensaje("<b>Informativo! Debe seleccionar una Compañía de Transporte válida</b>");
                    txtCiaTrans.Focus();
                    return;
                }

                if (string.IsNullOrEmpty(txtFechaSalida.Text))
                {
                    Mostrar_Mensaje("<b>Informativo! Debe seleccionar una fecha válida para la generación del pase</b>");
                    txtFechaSalida.Focus();
                    return;
                }

                CultureInfo enUS = new CultureInfo("en-US");
                string fecha = $"{txtFechaSalida.Text.Trim()} 23:59";
                if (!DateTime.TryParseExact(fecha, "MM-dd-yyyy HH:mm", enUS, DateTimeStyles.AdjustToUniversal, out DateTime fechaSalida))
                {
                    Mostrar_Mensaje("<b>Informativo! Debe seleccionar una fecha válida para la generación del pase Mes/Día/año</b>");
                    txtFechaSalida.Focus();
                    return;
                }

                // Validar fecha contra CAS (si aplica)
                var contenedor = ContenedoresSeleccionados.FirstOrDefault();
                if (contenedor != null && contenedor.CAS.HasValue && fechaSalida.Date > contenedor.CAS.Value.Date)
                {
                    Mostrar_Mensaje($"<b>Informativo! La fecha de salida: {fechaSalida:MM-dd-yyyy}, no puede ser mayor que la fecha CAS: {contenedor.CAS.Value:MM-dd-yyyy}</b>");
                    txtFechaSalida.Focus();
                    return;
                }

                // Validar compañía de transporte
                var usuarioLogin = usuario.Deserialize(Session["control"].ToString()).loginname;
                var empresaTransporte = N4.Entidades.CompaniaTransporte.ObtenerCompania(usuarioLogin, IdTxtCiaTrans.Value);
                if (!empresaTransporte.Exitoso)
                {
                    Mostrar_Mensaje("<b>Informativo! Debe seleccionar una compañía de transporte válida</b>");
                    txtCiaTrans.Focus();
                    return;
                }

                // Validar chofer (opcional)
                string idChofer = string.Empty;
                string desChofer = string.Empty;
                if (!string.IsNullOrEmpty(txtChofer.Text))
                {
                    string choferSelect = txtChofer.Text.Trim();
                    if (choferSelect.Split('-').Length > 1)
                    {
                        idChofer = choferSelect.Split('-')[0].Trim();
                        desChofer = choferSelect.Split('-')[1].Trim();
                        var choferTransporte = N4.Entidades.Chofer.ObtenerChofer(usuarioLogin, idChofer);
                        if (!choferTransporte.Exitoso)
                        {
                            Mostrar_Mensaje("<b>Informativo! Debe seleccionar un chofer válido</b>");
                            txtChofer.Focus();
                            return;
                        }
                    }
                    else
                    {
                        Mostrar_Mensaje("<b>Informativo! Debe seleccionar un chofer válido</b>");
                        txtChofer.Focus();
                        return;
                    }
                }

                // Validar placa (opcional)
                string idPlaca = string.Empty;
                if (!string.IsNullOrEmpty(txtPlaca.Text))
                {
                    //string placaSelect = txtPlaca.Text.Trim();
                    //if (placaSelect.Split('-').Length > 1)
                    //{
                    //    idPlaca = placaSelect.Split('-')[0].Trim();
                    //    var placaTransporte = N4.Entidades.Camion.ObtenerCamion(usuarioLogin, idPlaca);
                    //    if (!placaTransporte.Exitoso)
                    //    {
                    //        Mostrar_Mensaje("<b>Informativo! Debe seleccionar una placa de vehículo válida</b>");
                    //        txtPlaca.Focus();
                    //        return;
                    //    }
                    //}
                    //else
                    //{
                    //    Mostrar_Mensaje("<b>Informativo! Debe seleccionar una placa de vehículo válida</b>");
                    //    txtPlaca.Focus();
                    //    return;
                    //}
                    string placaSelect = txtPlaca.Text.Trim();
                    if (placaSelect.Length > 1)
                    {
                        idPlaca = placaSelect.Trim();
                        var placaTransporte = N4.Entidades.Camion.ObtenerCamion(usuarioLogin, idPlaca);
                        if (!placaTransporte.Exitoso)
                        {
                            Mostrar_Mensaje("<b>Informativo! Debe seleccionar una placa de vehículo válida</b>");
                            txtPlaca.Focus();
                            return;
                        }
                    }
                    else
                    {
                        Mostrar_Mensaje("<b>Informativo! Debe seleccionar una placa de vehículo válida</b>");
                        txtPlaca.Focus();
                        return;
                    }
                }

                // Generar el e-Pass para cada contenedor seleccionado
                long idPaseGenerado = 0;
                foreach (var det in ContenedoresSeleccionados)
                {
                    //Console.WriteLine($"Debug: NumeroContenedor = {det.NumeroContenedor}"); // Depuración
                    //if (!long.TryParse(det.NumeroContenedor, out long numeroContenedor))
                    //{
                    //    Mostrar_Mensaje($"<b>Error! El número de contenedor {det.NumeroContenedor} no tiene un formato válido.</b>");
                    //    return;
                    //}

                    //var validar = N4.Importacion.container.ValidarEstadoContenedor(
                    //    new List<long> { numeroContenedor },
                    //    usuarioLogin
                    //);
                    //if (!validar.Exitoso)
                    //{
                    //    Mostrar_Mensaje(validar.MensajeProblema);
                    //    return;
                    //}

                    Pase_Container pase = new Pase_Container
                    {
                        ID_CARGA = det.GKEY,
                        ESTADO = "GN",
                        FECHA_EXPIRACION = fechaSalida,
                        ID_PLACA = idPlaca,
                        ID_CHOFER = idChofer,
                        ID_EMPRESA = IdTxtCiaTrans.Value,
                        CANTIDAD_CARGA = 0,
                        USUARIO_REGISTRO = usuarioLogin,
                        TIPO_CARGA = "CNTR",
                        TINICIA = det.TURNO_DESDE,
                        TFIN = det.TURNO_HASTA,
                        TID = det.TURNO ?? 0,
                        CONSIGNATARIO_ID = det.IMPORTADOR,
                        CONSIGNARIO_NOMBRE = det.IMPORTADOR_DESC,
                        TRANSPORTISTA_DESC = empresaTransporte.Resultado?.razon_social ?? txtCiaTrans.Text,
                        CHOFER_DESC = desChofer,
                        PPW = det.ID_PPWEB,
                        DetalleTarjaID = det.ID ?? 0

                    };

                    var r = pase.Insertar(det.TURNO ?? 0, det.ID_TURNO ?? 0, det.NumeroContenedor, det.D_TURNO, det.CAS ?? DateTime.Now, det.CNTR_DD);
                    if (!r.Exitoso)
                    {
                        Mostrar_Mensaje(r.MensajeProblema);
                        return;
                    }
                    idPaseGenerado = r.Resultado;
                }
                string id_carga = Server.UrlEncode($"{ContenedoresSeleccionados.First().MRN}-{ContenedoresSeleccionados.First().MSN}-{ContenedoresSeleccionados.First().HSN}");
                string link = $"<a href='../pasepuertacontenedor/imprimirpasecontenedordespacho.aspx?id_carga={id_carga}' target='_blank'>Imprimir Pase Puerta Despacho</a>";
                string linkPreview = idPaseGenerado > 0 ? $"<a href='../pasepuertacontenedor/pase_puerta_orden_preview.aspx?id_pase={idPaseGenerado}' target='_blank'>Vista previa Pase Puerta</a>" : string.Empty;

                Mostrar_Mensaje($"<b>Informativo! El pase fue generado con éxito. Para imprimirlo, haga clic en el siguiente enlace: {link} {linkPreview}</b>");

                ContenedoresSeleccionados.Clear();
                gvSeleccionados.DataSource = ContenedoresSeleccionados;
                gvSeleccionados.DataBind();
                UPFORM.Update();
                UPSEARCH.Update();

            }
            catch (Exception ex)
            {
                Mostrar_Mensaje($"<b>Error! Lo sentimos, algo salió mal: {ex.Message}</b>");
            }
        }

        private void Mostrar_Mensaje(string mensaje)
        {
            lblMensaje.InnerHtml = mensaje;
            lblMensaje.Visible = true;
            UPFORM.Update();
        }

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
    }
}