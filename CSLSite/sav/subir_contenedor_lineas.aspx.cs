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


    public partial class subir_contenedor_lineas : System.Web.UI.Page
    {

        #region "Clases"

        private Cls_Bil_Sesion objSesion = new Cls_Bil_Sesion();
        usuario ClsUsuario;
        private Cls_Bil_Cabecera objCab = new Cls_Bil_Cabecera();
        private Cls_Horas_Reefer objDetalleHoras = new Cls_Horas_Reefer();

        private Cls_Bil_CabeceraMsc objCabMsc = new Cls_Bil_CabeceraMsc();
        private Cls_Bil_DetalleMsc objDetalleMsc = new Cls_Bil_DetalleMsc();
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
            objCabMsc = new Cls_Bil_CabeceraMsc();
            Session["ContenedoresMsc" + this.hf_BrowserWindowName.Value] = objCabMsc;
        
        }



        #endregion



        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            ViewStateUserKey = Session.SessionID;

          

#if !DEBUG
                this.IsAllowAccess();
#endif


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

            objCabMsc = Session["ContenedoresMsc" + this.hf_BrowserWindowName.Value] as Cls_Bil_CabeceraMsc;
            if (objCabMsc != null)
            {
                objCab.Detalle.Clear();
            }

            this.Ocultar_Mensaje();
            this.det1.Visible = false;
           

            this.BtnGrabar.Attributes["disabled"] = "disabled";
            this.BtnCargar.Attributes.Remove("disabled");
            Response.Redirect("~/sav/subir_contenedor_lineas.aspx", true);
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

                            CultureInfo enUS = new CultureInfo("en-US");
                            //DateTime Fecha_Retiro;

                            var ClsUsuario = usuario.Deserialize(HttpContext.Current.Session["control"].ToString());
                            var Contenedores = new List<Cls_Bil_DetalleMsc>();

                            int fila = 1;
                            for (int i = 0; i < tbl.Rows.Count; i++)
                            {
                                string unidad = tbl.Rows[i][0] == null ? "" : tbl.Rows[i][0].ToString().Trim();

                                if (!string.IsNullOrEmpty(unidad)) 
                                {
                                    string bl = tbl.Rows[i][1] == null ? "" : tbl.Rows[i][1].ToString().Trim();
                                    string cliente = tbl.Rows[i][2] == null ? "" : tbl.Rows[i][2].ToString().Trim();
                                    string tipo = tbl.Rows[i][3] == null ? "" : tbl.Rows[i][3].ToString().Trim();
                                    string nave = tbl.Rows[i][4] == null ? "" : tbl.Rows[i][4].ToString().Trim();
                                    string viaje = tbl.Rows[i][5] == null ? "" : tbl.Rows[i][5].ToString().Trim();

                                    string fecha = tbl.Rows[i][6].ToString();

                                    DateTime Fecha_Retiro;
                                    if (!DateTime.TryParseExact(fecha, "yyyy/MM/dd", enUS, DateTimeStyles.AdjustToUniversal, out Fecha_Retiro))
                                    {
                                        this.Mostrar_Mensaje(1, string.Format("<b>Error! </b>Lo sentimos, algo salió mal. la fecha de retiro de la unidad {0}, no tiene el formato día/Mes/Año: {1}", unidad, fecha));
                                        return;
                                    }



                                    var det = new Cls_Bil_DetalleMsc();

                                    det.fila = fila;
                                    det.contenedor = unidad.Trim();
                                    det.fecha_despacho = Fecha_Retiro;//System.DateTime.Now;
                                    det.estado = true;
                                    det.linea = ClsUsuario.ruc;

                                    det.bl = bl;
                                    det.cliente = cliente;
                                    det.tipo = tipo;
                                    det.nave = nave;
                                    det.viaje = viaje;

                                    Contenedores.Add(det);
                                }
                                
                                fila++;
                            }

                            if (Contenedores.Count != 0)
                            {
                                //agrego todos los contenedores a la clase cabecera
                                objCabMsc = Session["ContenedoresMsc" + this.hf_BrowserWindowName.Value] as Cls_Bil_CabeceraMsc;
                                objCabMsc.FECHA = DateTime.Now;
                                objCabMsc.IV_USUARIO_CREA = ClsUsuario.loginname;
                                objCabMsc.SESION = this.hf_BrowserWindowName.Value;
                                objCabMsc.ARCHIVO = FileUpload1.FileName;
                                objCabMsc.LINEA = ClsUsuario.ruc;

                                objCabMsc.Detalle.Clear();

                                foreach (var Det in Contenedores)
                                {
                                    objDetalleMsc = new Cls_Bil_DetalleMsc();
                                    objDetalleMsc.id = 0;
                                    objDetalleMsc.fila = Det.fila;
                                    objDetalleMsc.contenedor = Det.contenedor;
                                    objDetalleMsc.fecha_despacho = Det.fecha_despacho;
                                    objDetalleMsc.estado = Det.estado;
                                    objDetalleMsc.IV_USUARIO_CREA = ClsUsuario.loginname;
                                    objDetalleMsc.linea = Det.linea;

                                    objDetalleMsc.bl = Det.bl;
                                    objDetalleMsc.cliente = Det.cliente;
                                    objDetalleMsc.tipo = Det.tipo;
                                    objDetalleMsc.nave = Det.nave;
                                    objDetalleMsc.viaje = Det.viaje;

                                    objCabMsc.Detalle.Add(objDetalleMsc);
                                }

                                tablePagination.DataSource = objCabMsc.Detalle;
                                tablePagination.DataBind();
                                OcultarLoading("1");

                                Session["ContenedoresMsc" + this.hf_BrowserWindowName.Value] = objCabMsc;

                                this.BtnGrabar.Attributes.Remove("disabled");
                                this.BtnCargar.Attributes["disabled"] = "disabled";
                                this.det1.Visible = true;
                               
                                this.Actualiza_Paneles();
                            }
                            else
                            {
                                tablePagination.DataSource = null;
                                tablePagination.DataBind();

                                this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>Se presentaron los siguientes problemas: {0}  {1}", "No existe detalle de contenedores para procesar", ""));
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
                    objCabMsc = Session["ContenedoresMsc" + this.hf_BrowserWindowName.Value] as Cls_Bil_CabeceraMsc;

                    if (objCabMsc == null)
                    {
                        this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Informativo! Debe generar la consulta, para poder subir los contenedores </b>"));
                        return;
                    }
                    if (objCabMsc.Detalle.Count == 0)
                    {
                        this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Informativo! No existe detalle de contenedores, para poder grabar.. </b>"));
                        return;
                    }

                    /*nuevo proceso de grabado*/
                    System.Xml.Linq.XDocument XMLCabecera = new System.Xml.Linq.XDocument(new System.Xml.Linq.XDeclaration("1.0", "UTF-8", "yes"),
                    new System.Xml.Linq.XElement("CABECERA",
                                                           new System.Xml.Linq.XElement("CABECERA",
                                                           new System.Xml.Linq.XAttribute("fecha", objCabMsc.FECHA == null ? System.DateTime.Today : objCabMsc.FECHA),
                                                           new System.Xml.Linq.XAttribute("usuario", objCabMsc.IV_USUARIO_CREA == null ? "TERMINAL" : objCabMsc.IV_USUARIO_CREA),
                                                           new System.Xml.Linq.XAttribute("archivo", objCabMsc.ARCHIVO == null ? "" : objCabMsc.ARCHIVO),
                                                           new System.Xml.Linq.XAttribute("linea", objCabMsc.LINEA == null ? "" : objCabMsc.LINEA),
                                                           new System.Xml.Linq.XAttribute("flag", "I"))));

                    System.Xml.Linq.XDocument xmlDetalle = new System.Xml.Linq.XDocument(new System.Xml.Linq.XDeclaration("1.0", "UTF-8", "yes"),
                               new System.Xml.Linq.XElement("DETALLE", from p in objCabMsc.Detalle.AsEnumerable().AsParallel()
                                                                              select new System.Xml.Linq.XElement("DETALLE",
                                                                                 new System.Xml.Linq.XAttribute("fila", p.fila),
                                                                                 new System.Xml.Linq.XAttribute("contenedor", p.contenedor == null ? "" : p.contenedor.ToString().Trim()),
                                                                                 new System.Xml.Linq.XAttribute("fecha_despacho", p.fecha_despacho == null ? DateTime.Parse("1900/01/01") : p.fecha_despacho),
                                                                                 new System.Xml.Linq.XAttribute("estado", p.estado),
                                                                                 new System.Xml.Linq.XAttribute("usuario", p.IV_USUARIO_CREA),
                                                                                 new System.Xml.Linq.XAttribute("linea", p.linea),
                                                                                 new System.Xml.Linq.XAttribute("bl", p.bl),
                                                                                 new System.Xml.Linq.XAttribute("cliente", p.cliente),
                                                                                 new System.Xml.Linq.XAttribute("tipo", p.tipo),
                                                                                 new System.Xml.Linq.XAttribute("nave", p.nave),
                                                                                 new System.Xml.Linq.XAttribute("viaje", p.viaje),
                                                                                 new System.Xml.Linq.XAttribute("flag", "I"))));

                    objCabMsc.xmlCabecera = XMLCabecera.ToString();
                    objCabMsc.xmlDetalle = xmlDetalle.ToString();
                    var nProceso = objCabMsc.SaveTransaction_Lineas(out cMensajes);

                    /*fin de nuevo proceso de grabado*/
                    if (!nProceso.HasValue || nProceso.Value <= 0)
                    {

                        this.det1.Visible = true;


                        this.Mostrar_Mensaje(1, string.Format("<i class='fa fa-warning'></i><b> Error! Existen contenedores que presentan problemas y no se procesaron: {0} </b>", cMensajes));

                        this.BtnGrabar.Attributes["disabled"] = "disabled";

                        this.Actualiza_Paneles();

                        return;
                    }
                    else
                    {

                        objCabMsc.Detalle.Clear();

                        Session["ContenedoresMsc" + this.hf_BrowserWindowName.Value] = objCabMsc;

                        this.Mostrar_Mensaje(2, string.Format("<i class='fa fa-warning'></i><b> Informativo! </b>Contenedores procesados con éxito"));
                        this.BtnGrabar.Attributes["disabled"] = "disabled";

                        this.Actualiza_Paneles();
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

            
        }
    }
}