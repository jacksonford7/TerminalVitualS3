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
using ControlPagos.Importacion;
using Salesforces;
using System.Data;
using System.Net;
using SqlConexion;
using CasManual;
using System.IO;
using OfficeOpenXml;

namespace CSLSite
{


    public partial class backoffice_horas_reefer : System.Web.UI.Page
    {

        #region "Clases"

        private Cls_Bil_Sesion objSesion = new Cls_Bil_Sesion();
        usuario ClsUsuario;
        private Cls_Bil_Cabecera objCab = new Cls_Bil_Cabecera();
        private Cls_Horas_Reefer objDetalleHoras = new Cls_Horas_Reefer();
        private string cMensajes;

        #endregion

        #region "Variables"

        private string numero_carga = string.Empty;
       
        private string Cliente_Ruc = string.Empty;
        private string Cliente_Rol = string.Empty;
        private string Cliente_Direccion = string.Empty;
        private string Cliente_Ciudad = string.Empty;
        private string Fecha = string.Empty;
        private string Contenedores = string.Empty;
        private string Numero_Carga = string.Empty;
        private string FechaPaidThruDay = string.Empty;
        private string CargabexuBlNbr = string.Empty;

        private string TipoServicio = string.Empty;

        private string LoginName = string.Empty;
        

        /*variables control de credito*/
        private string sap_usuario = string.Empty;
        private string sap_clave = string.Empty;
        private static Int64? lm = -3;
        private string OError;

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


        private string ContenidoArchivo
        {
            get
            {
                return (string)Session["ContenidoArchivo"];
            }
            set
            {
                Session["ContenidoArchivo"] = value;
            }

        }


        #endregion

        #region "Metodos"

     
        private void Actualiza_Paneles()
        {
            //this.UPDETALLE.Update();
           // this.UPCARGA.Update();
           

        }


        private void OcultarLoading(string valor)
        {
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "ocultarloader('"+valor+"');", true);
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
            if (Tipo == 1)//cabecera
            {
                this.banmsg.Visible = true;
                this.banmsg.InnerHtml = Mensaje;
                OcultarLoading("1");
            }
            if (Tipo == 2)//cabecera
            {
                this.banmsg2.Visible = true;
                this.banmsg2.InnerHtml = Mensaje;
                OcultarLoading("1");
            }
            this.Actualiza_Paneles();
        }

        private void Ocultar_Mensaje()
        {
            this.banmsg.InnerText = string.Empty;          
            this.banmsg.Visible = false;
            this.banmsg2.InnerText = string.Empty;
            this.banmsg2.Visible = false;
            this.Actualiza_Paneles();
            OcultarLoading("1");
          
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
            objCab = new Cls_Bil_Cabecera();
            Session["HorasReefer" + this.hf_BrowserWindowName.Value] = objCab;
        
        }



        #endregion



        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            ViewStateUserKey = Session.SessionID;

        }

        protected void Page_Init(object sender, EventArgs e)

        {

            if (!Request.IsAuthenticated)
            {
                Response.Redirect("../login.aspx", false);

                return;
            }

            if (!IsPostBack)
            {
                Page.SslOn();
            }

            this.banmsg.Visible = IsPostBack;
            this.banmsg2.Visible = IsPostBack;

            if (!Page.IsPostBack)
            {
                this.det1.Visible = false;
                this.det2.Visible = false;
                this.banmsg.InnerText = string.Empty;

                ClsUsuario = Page.Tracker();
                if (ClsUsuario != null)
                {

                    this.Txtcliente.Text = string.Format("{0} {1}", ClsUsuario.nombres, ClsUsuario.apellidos);
                    this.Txtruc.Text = string.Format("{0}", ClsUsuario.ruc);
                    this.Txtempresa.Text = string.Format("{0}", ClsUsuario.codigoempresa);
                }

            }

            if (Session["FileUpload1"] == null && FileUpload1.HasFile)
            {
                Session["FileUpload1"] = FileUpload1;
                LblRuta.Text = FileUpload1.FileName;
                this.ContenidoArchivo = new StreamReader(FileUpload1.PostedFile.InputStream).ReadToEnd();
            }

            else if (Session["FileUpload1"] != null && (!FileUpload1.HasFile))
            {

                FileUpload1 = (FileUpload)Session["FileUpload1"];
                LblRuta.Text = FileUpload1.FileName;

            }

            else if (FileUpload1.HasFile)
            {
                Session["FileUpload1"] = FileUpload1;
                LblRuta.Text = FileUpload1.FileName;
                this.ContenidoArchivo = new StreamReader(FileUpload1.PostedFile.InputStream).ReadToEnd();
            }



        }

    
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {

                if (!Page.IsPostBack)
                {
                    

                    this.Crear_Sesion();
                    this.BtnGrabar.Attributes["disabled"] = "disabled";
                }
                else
                {
                    var ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());
                    if (ClsUsuario != null)
                    {   
                        this.Txtcliente.Text = string.Format("{0} {1}", ClsUsuario.nombres, ClsUsuario.apellidos);
                        this.Txtruc.Text = string.Format("{0}", ClsUsuario.ruc);
                        this.Txtempresa.Text = string.Format("{0}", ClsUsuario.codigoempresa);

                    }
                }


              

            }
            catch (Exception ex)
            {
                this.Mostrar_Mensaje(1, string.Format("<b>Error! </b>{0}",ex.Message));
            }
        }

 
     
        protected void BtnNuevo_Click(Object sender, EventArgs e)
        {
            Session["FileUpload1"] = null;
            objCab = Session["HorasReefer" + this.hf_BrowserWindowName.Value] as Cls_Bil_Cabecera;
            if (objCab != null)
            {
                objCab.Detalle_Horas_Reefer.Clear();
            }
            this.Ocultar_Mensaje();
            this.det1.Visible = false;
            this.det2.Visible = false;

            this.BtnGrabar.Attributes["disabled"] = "disabled";
            this.BtnCargar.Attributes.Remove("disabled");
            Response.Redirect("~/backoffice/backoffice_horas_reefer.aspx", true);
        }

       

        protected void BtnCargar_Click(Object sender, EventArgs e)
        {
            if (Response.IsClientConnected)
            {
                try
                {
                    OcultarLoading("1");
                    this.Ocultar_Mensaje();
                    this.BtnGrabar.Attributes["disabled"] = "disabled";

                    if (HttpContext.Current.Request.Cookies["token"] == null)
                    {
                        System.Web.Security.FormsAuthentication.SignOut();
                        System.Web.Security.FormsAuthentication.RedirectToLoginPage();
                        Session.Clear();
                        OcultarLoading("2");
                        return;
                    }
                    //fsuploadarchivo
                    if (FileUpload1.HasFile && Path.GetExtension(FileUpload1.FileName) == ".xlsx")
                    {
                        using (var Excel = new ExcelPackage(FileUpload1.PostedFile.InputStream))
                        {
                            var tbl = new DataTable();

                            // Obtengo la hoja a trabajar
                            var ws = Excel.Workbook.Worksheets.First();

                            // Calculo dimensiones del archivo
                            ExcelCellAddress startCell = ws.Dimension.Start;
                            ExcelCellAddress endCell = ws.Dimension.End;


                            // instancio las columnas
                            for (int col = startCell.Column; col <= endCell.Column; col++)
                            {
                                string Titulo = col.ToString();
                                tbl.Columns.Add(String.Format("Column {0}", Titulo));
                            }


                            // agrego la data
                            for (int row = startCell.Row + 1; row <= endCell.Row; row++)
                            {
                                DataRow dr = tbl.NewRow();
                                int x = 0;
                                for (int col = startCell.Column; col <= endCell.Column; col++)
                                {
                                    dr[x++] = (ws.Cells[row, col].Value == null ? string.Empty : ws.Cells[row, col].Value);
                                }
                                tbl.Rows.Add(dr);
                            }

                            var ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());
                            var PowerLineHour = new List<N4Ws.Entidad.PowerLineHour>();

                            for (int i = 0; i < tbl.Rows.Count; i++)
                            {
                                string linea = tbl.Rows[i][0].ToString();
                                string referencia = tbl.Rows[i][1].ToString();
                                string categoria = tbl.Rows[i][2].ToString();
                                string contenedor = tbl.Rows[i][3].ToString();
                                string cantidad = tbl.Rows[i][4].ToString();

                                var Horas = new N4Ws.Entidad.PowerLineHour();

                                Horas.CUSTOMPOWERH_LINE = linea.Trim();
                                Horas.CUSTOMPOWERH_VVISIT = referencia.Trim();
                                Horas.CUSTOMPOWERH_CATEG = categoria.Trim().ToUpper().Equals("EXPORT") ? N4Ws.Entidad.UNIT_CATEG.EXPORT : N4Ws.Entidad.UNIT_CATEG.IMPORT;
                                Horas.CUSTOMPOWERH_UNITID = contenedor.Trim();

                                double CantHoras = 0;
                                if (!double.TryParse(cantidad, out CantHoras))
                                {
                                    //campo no es numerico
                                    this.Mostrar_Mensaje(1,string.Format("<b>Error! </b>Lo sentimos, algo salió mal. La cantidad de horas reefer debe tener un valor numérico {0}", cantidad));
                                    return;
                                }

                                Horas.CUSTOMPOWERH_HOURS = CantHoras;
                                PowerLineHour.Add(Horas);

                            }

                          
                            var Resultado = N4Ws.Entidad.PowerLineHour.ValidarUnidades(PowerLineHour, ClsUsuario.loginname);
                            if (Resultado != null)
                            {
                                if (Resultado.Exitoso)
                                {

                                    /*query contenedores*/
                                    var LinqQuery = (from p in Resultado.Resultado.AsEnumerable()
                                                     select new
                                                     {
                                                         id = p.Field<string>("id") == null ? "" : p.Field<string>("id").Trim(),
                                                         linea = p.Field<string>("linea") == null ? "" : p.Field<string>("linea").Trim(),
                                                         trafico = p.Field<string>("trafico") == null ? "" : p.Field<string>("trafico").Trim(),
                                                         referencia = p.Field<string>("referencia") == null ? "" : p.Field<string>("referencia").Trim(),
                                                         gkey = p.Field<Int64?>("gkey") == null ? 0 : p.Field<Int64?>("gkey"),
                                                         horas = p.Field<double?>("horas") ==null ? 0 : p.Field<double?>("horas"),
                                                         valido = p.Field<bool?>("valido") == null ? false : p.Field<bool?>("valido"),
                                                         novedad = p.Field<string>("novedad") == null ? "" : p.Field<string>("novedad").Trim()
                                                     });

                                    if (LinqQuery != null)
                                    {
                                        //agrego todos los contenedores a la clase cabecera
                                        objCab = Session["HorasReefer" + this.hf_BrowserWindowName.Value] as Cls_Bil_Cabecera;
                                        objCab.FECHA = DateTime.Now;
                                        objCab.IV_USUARIO_CREA = ClsUsuario.loginname;
                                        objCab.SESION = this.hf_BrowserWindowName.Value;

                                        objCab.Detalle_Horas_Reefer.Clear();
                                        foreach (var Det in LinqQuery)
                                        {
                                            objDetalleHoras = new Cls_Horas_Reefer();
                                            objDetalleHoras.id = Det.id;
                                            objDetalleHoras.linea = Det.linea;
                                            objDetalleHoras.trafico = Det.trafico;
                                            objDetalleHoras.referencia = Det.referencia;
                                            objDetalleHoras.gkey = Det.gkey.Value;
                                            objDetalleHoras.horas = Det.horas.Value;
                                            objDetalleHoras.valido = Det.valido.Value;
                                            objDetalleHoras.novedad = Det.novedad;
                                            objCab.Detalle_Horas_Reefer.Add(objDetalleHoras);
                                        }
                                    }


                                    tablePagination.DataSource = objCab.Detalle_Horas_Reefer;
                                    tablePagination.DataBind();
                                    OcultarLoading("1");

                                    Session["HorasReefer" + this.hf_BrowserWindowName.Value] = objCab;

                                    this.BtnGrabar.Attributes.Remove("disabled");
                                    this.BtnCargar.Attributes["disabled"] = "disabled";
                                    this.det1.Visible = true;
                                    this.det2.Visible = false;
                                    this.Actualiza_Paneles();

                                }
                                else {
                                    tablePagination.DataSource = null;
                                    tablePagination.DataBind();

                                    this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>Se presentaron los siguientes problemas: {0}  {1}", Resultado.MensajeProblema, Resultado.QueHacer));
                                    return;
                                }
                            }
                            else
                            {
                                tablePagination.DataSource = null;
                                tablePagination.DataBind();

                                this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>validación de unidades, no retorno información....: {0}  {1}", Resultado.MensajeProblema, Resultado.QueHacer));
                                return;
                            }


                        }
                    }
                    else
                    {
                        this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>El formato del archivo invalido, el formato a subir debe ser de tipo xlsx"));
                        return;
                    }

                }
                catch (Exception ex)
                {

                    lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(BtnCargar_Click), "BtnCargar_Click", false, null, null, ex.StackTrace, ex);
                    OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                    this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));
                }
                

            }
               
        }

        protected void BtnGrabar_Click(Object sender, EventArgs e)
        {
            if (Response.IsClientConnected)
            {
                try
                {
                    var ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());
                    //instancia sesion
                    objCab = Session["HorasReefer" + this.hf_BrowserWindowName.Value] as Cls_Bil_Cabecera;
                    if (objCab == null)
                    {
                        this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Informativo! Debe generar la consulta, para poder subir las horas reefer </b>"));
                        return;
                    }
                    if (objCab.Detalle_Horas_Reefer.Count == 0)
                    {
                        this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Informativo! No existe detalle de horas reefer, para poder grabar.. </b>"));
                        return;
                    }

                    //valida que seleccione las cargas a facturar
                    var LinqValidaContenedor = (from p in objCab.Detalle_Horas_Reefer.Where(x => x.valido == false)
                                                select p.id).ToList();

                    if (LinqValidaContenedor.Count != 0)
                    {
                        string Contenedores = string.Join(", ", LinqValidaContenedor);
                        this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Informativo! Existen contenedores que presentan problemas: {0} </b>", Contenedores));
                        return;
                    }

                    var PowerLineHour = new List<N4Ws.Entidad.PowerLineHour>();

                    var LinqDetalle = (from p in objCab.Detalle_Horas_Reefer.Where(x => x.valido == true)
                                       select p).ToList();

                    foreach (var Det in LinqDetalle)
                    {
                        string linea = Det.linea.Trim();
                        string referencia = Det.referencia.Trim();
                        string categoria = Det.trafico.Trim();
                        string contenedor = Det.id.Trim() ;
                        double cantidad = Det.horas;

                        var Horas = new N4Ws.Entidad.PowerLineHour();

                        Horas.CUSTOMPOWERH_LINE = linea.Trim();
                        Horas.CUSTOMPOWERH_VVISIT = referencia.Trim();
                        Horas.CUSTOMPOWERH_CATEG = categoria.Trim().ToUpper().Equals("EXPORT") ? N4Ws.Entidad.UNIT_CATEG.EXPORT : N4Ws.Entidad.UNIT_CATEG.IMPORT;
                        Horas.CUSTOMPOWERH_UNITID = contenedor.Trim();
                        Horas.CUSTOMPOWERH_GKEY = Det.gkey;
                        Horas.CUSTOMPOWERH_HOURS = cantidad;
                        PowerLineHour.Add(Horas);

                    }

                    var Resultado = N4Ws.Entidad.PowerLineHour.ProcesarPLH(PowerLineHour, ClsUsuario.loginname);
                    if (Resultado != null)
                    {
                        if (Resultado.Exitoso)
                        {

                            //grabar log
                            DataTable DtProcesados = new DataTable();
                            DataRow row;
                            DtProcesados.Columns.Add("gkey", typeof(Int64));
                            DtProcesados.Columns.Add("CUSTOMPOWERH_GKEY", typeof(Int64));
                            DtProcesados.Columns.Add("CUSTOMPOWERH_LINE", typeof(string));
                            DtProcesados.Columns.Add("CUSTOMPOWERH_HOURS", typeof(float));
                            DtProcesados.Columns.Add("CUSTOMPOWERH_UNITID", typeof(string));
                            DtProcesados.Columns.Add("CUSTOMPOWERH_VVISIT", typeof(string));
                            DtProcesados.Columns.Add("CUSTOMPOWERH_CATEG", typeof(string));
                            DtProcesados.Columns.Add("PWLogUID", typeof(Int64));
                            DtProcesados.Columns.Add("novedad", typeof(string));
                            DtProcesados.Columns.Add("valid", typeof(bool));

                            /*query contenedores*/
                            var LinqQuery = (from p in Resultado.Resultado.AsEnumerable()
                                             select new
                                             {
                                                 id = p.Field<string>("CUSTOMPOWERH_UNITID") == null ? "" : p.Field<string>("CUSTOMPOWERH_UNITID").Trim(),
                                                 linea = p.Field<string>("CUSTOMPOWERH_LINE") == null ? "" : p.Field<string>("CUSTOMPOWERH_LINE").Trim(),
                                                 trafico = p.Field<string>("CUSTOMPOWERH_CATEG") == null ? "" : p.Field<string>("CUSTOMPOWERH_CATEG").Trim(),
                                                 referencia = p.Field<string>("CUSTOMPOWERH_VVISIT") == null ? "" : p.Field<string>("CUSTOMPOWERH_VVISIT").Trim(),
                                                 gkey = p.Field<Int64>("CUSTOMPOWERH_GKEY"),
                                                 horas = p.Field<float?>("CUSTOMPOWERH_HOURS") == null ? 0 : p.Field<float?>("CUSTOMPOWERH_HOURS"),
                                                 valido = p.Field<bool?>("valid") == null ? false : p.Field<bool?>("valid"),
                                                 novedad = p.Field<string>("novedad") == null ? "" : p.Field<string>("novedad").Trim()
                                             });

                            if (LinqQuery != null)
                            {
                                //agrego todos los contenedores a la clase cabecera
                                objCab = Session["HorasReefer" + this.hf_BrowserWindowName.Value] as Cls_Bil_Cabecera;
                                objCab.FECHA = DateTime.Now;
                                objCab.IV_USUARIO_CREA = ClsUsuario.loginname;
                                objCab.SESION = this.hf_BrowserWindowName.Value;

                                objCab.Detalle_Horas_Errores.Clear();
                                foreach (var Det in LinqQuery)
                                {
                                    objDetalleHoras = new Cls_Horas_Reefer();
                                    objDetalleHoras.id = Det.id;
                                    objDetalleHoras.linea = Det.linea;
                                    objDetalleHoras.trafico = Det.trafico;
                                    objDetalleHoras.referencia = Det.referencia;
                                    objDetalleHoras.gkey = Det.gkey;
                                    objDetalleHoras.horas2 = Det.horas.Value;
                                    objDetalleHoras.valido = Det.valido.Value;
                                    objDetalleHoras.novedad = Det.novedad;
                                    objCab.Detalle_Horas_Errores.Add(objDetalleHoras);

                                    row = DtProcesados.NewRow();
                                    row["valid"] = Det.valido.Value;
                                    row["gkey"] = Det.gkey;
                                    row["CUSTOMPOWERH_GKEY"] = Det.gkey;
                                    row["CUSTOMPOWERH_LINE"] = Det.linea;
                                    row["CUSTOMPOWERH_HOURS"] = Det.horas.Value;
                                    row["CUSTOMPOWERH_UNITID"] = Det.id;
                                    row["CUSTOMPOWERH_VVISIT"] = Det.referencia;
                                    row["CUSTOMPOWERH_CATEG"] = Det.trafico;
                                    row["novedad"] = Det.novedad;
                                    DtProcesados.Rows.Add(row);

                                }

                                //graba log 
                                N4.Entidades.PLH_Log PLH_Log = new N4.Entidades.PLH_Log();
                                PLH_Log.usuario = ClsUsuario.loginname;
                                PLH_Log.GrabarLog(DtProcesados);

                                //valida que seleccione las cargas a facturar
                                var LinqError = (from p in objCab.Detalle_Horas_Errores.Where(x => x.valido == false)
                                                 select p.id).ToList();

                                if (LinqError.Count != 0)
                                {
                                    this.det1.Visible = true;
                                    this.det2.Visible = true;
                                    string Contenedores = string.Join(", ", LinqError);
                                    this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Error! Existen contenedores que presentan problemas y no se procesaron las horas reefer.: {0} </b>", Contenedores));
                                    this.BtnGrabar.Attributes["disabled"] = "disabled";
                                    tablePagination2.DataSource = objCab.Detalle_Horas_Errores.Where(x => x.valido == false);
                                    tablePagination2.DataBind();

                                }

                                //valida que seleccione las cargas a facturar
                                var LinqOk = (from p in objCab.Detalle_Horas_Errores.Where(x => x.valido == true)
                                              select p.id).ToList();

                                if (LinqOk.Count != 0)
                                {
                                    this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>Horas reefer procesadas con éxito: {0} unidades procesadas..,con errores: {1}", LinqOk.Count, LinqError.Count));
                                    this.BtnGrabar.Attributes["disabled"] = "disabled";
                                    this.Actualiza_Paneles();
                                }

                            }
                            else
                            {
                                this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>Se presentaron los siguientes problemas: {0}", "Al obtener contenedores con horas reefer a cargar, retorno de inserción de datos."));
                                return;
                            }

                            

                        }
                        else
                        {
                            this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>Se presentaron los siguientes problemas: {0}  {1}", Resultado.MensajeProblema, Resultado.QueHacer));
                            return;
                        }
                    }
                    else
                    {
                        this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>Error al procesar horas reefer....: {0}  {1}", Resultado.MensajeProblema, Resultado.QueHacer));
                        return;
                    }

                }
                catch (Exception ex)
                {

                    lm = SqlConexion.Cls_Conexion.LogEvent<Exception>(Page.User.Identity.Name, nameof(BtnGrabar_Click), "BtnGrabar_Click", false, null, null, ex.StackTrace, ex);
                    OError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                    this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Error! </b>Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible....{0}", OError));
                }
               

            }
        }
        protected void tablePagination_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (Response.IsClientConnected)
            {


                try
                {

                    if (e.Row.RowType == DataControlRowType.DataRow)
                    {

                        string valido = DataBinder.Eval(e.Row.DataItem, "valido").ToString();
                        string novedad = DataBinder.Eval(e.Row.DataItem, "novedad").ToString();
                       
                        if (!valido.Equals("True"))
                        {
                            e.Row.ForeColor = System.Drawing.Color.IndianRed;     
                        }

                    }
                }
                catch (Exception ex)
                {
                    this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Error! Lo sentimos, algo salió mal. Estamos trabajando para solucionarlo lo más pronto posible..{0}</b>", ex.Message));

                }
            }
        }
    }
}