using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using System.Data;
using System.IO;
using System.Configuration;
using System.Globalization;

namespace CSLSite
{
    public partial class revisasolicitudvehiculodocumentos_new : System.Web.UI.Page
    {
        private GetFile.Service getFile = new GetFile.Service();
        public DataTable dtDocumentos
        {
            get { return (DataTable)Session["dtDocumentosrevisasolicitudvehiculodocumentos"]; }
            set { Session["dtDocumentosrevisasolicitudvehiculodocumentos"] = value; }
        }
        private String xmlDocumentos;
        public String splaca
        {
            get { return (String)Session["splacadocvehaprosolvehdoc"]; }
            set { Session["splacadocvehaprosolvehdoc"] = value; }
        }
        public String numsolicitudempresa
        {
            get { return (String)Session["numsolicitudempresaaprosolvehdoc"]; }
            set { Session["numsolicitudempresaaprosolvehdoc"] = value; }
        }
        public String idsolveh
        {
            get { return (String)Session["idsolvehaprosolvehdoc"]; }
            set { Session["idsolvehaprosolvehdoc"] = value; }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            //Está conectado y no es postback
            if (Response.IsClientConnected && !IsPostBack)
            {
                sinresultado.Visible = false;
                try
                {
                    ConsultaInfoSolicitud();
                }
                catch (Exception ex)
                {
                    this.Alerta(ex.Message);
                }
            }
        }
        private void ConsultaInfoSolicitud()
        {
            CultureInfo enUS = new CultureInfo("en-US");

            numsolicitudempresa = Request.QueryString["numsolicitud"];
            idsolveh = Request.QueryString["idsolveh"];
            splaca = Request.QueryString["placa"];
            hfPlaca.Value = splaca;
            dtDocumentos = credenciales.GetDocumentosVehiculoXNumSolicitud_New(numsolicitudempresa, idsolveh);
            tablePaginationDocumentos.DataSource = dtDocumentos;
            tablePaginationDocumentos.DataBind();
            dtDocumentos.Columns.Add("DocumentoRechazado");
            dtDocumentos.Columns.Add("Comentario");
            dtDocumentos.Columns.Add("Placa");
            dtDocumentos.Columns.Add("fechadocumento");
            DataTable dtPadre = Session["dtDocumentosrevisasolicitudvehiculo"] as DataTable;

            if (dtPadre != null)
            {
                var result = from myRow in dtPadre.AsEnumerable()
                             where myRow.Field<string>("Placa") == splaca
                             select myRow;
                DataTable dt = result.AsDataView().ToTable();
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i <= dt.Rows.Count - 1; i++)
                    {
                        dtDocumentos.Rows[i][13] = dt.Rows[i][13];
                        dtDocumentos.Rows[i][14] = dt.Rows[i][14];
                        dtDocumentos.Rows[i][15] = dt.Rows[i][15];
                        dtDocumentos.Rows[i][16] = dt.Rows[i][16];
                    }

                    foreach (RepeaterItem item in tablePaginationDocumentos.Items)
                    {
                        CheckBox chkRevisado = item.FindControl("chkRevisado") as CheckBox;
                        TextBox tcomentario = item.FindControl("tcomentario") as TextBox;
                        TextBox txtfecha = item.FindControl("txtfecha") as TextBox;

                        chkRevisado.Checked = Convert.ToBoolean(dtDocumentos.Rows[item.ItemIndex][13]);
                        if (!chkRevisado.Checked)
                        {
                            tcomentario.Text = "";
                            tcomentario.Text = dtDocumentos.Rows[item.ItemIndex][14].ToString();
                        }
                        else
                        {
                            tcomentario.Text = dtDocumentos.Rows[item.ItemIndex][14].ToString();
                        }

                        if (dtDocumentos.Rows[item.ItemIndex][16] != null)
                        {
                            string Fecha = dtDocumentos.Rows[item.ItemIndex][16].ToString();

                            if (!string.IsNullOrEmpty(Fecha))
                            {
                                Fecha = Fecha.Substring(0, 10);
                                DateTime FechaFactura;
                                if (!DateTime.TryParseExact(Fecha, "dd/MM/yyyy", enUS, DateTimeStyles.AdjustToUniversal, out FechaFactura))
                                {

                                }
                                else
                                {
                                    txtfecha.Text = FechaFactura.ToString("dd/MM/yyyy");

                                }
                            }
                        }

                    }
                }
                else
                {
                    foreach (RepeaterItem item in tablePaginationDocumentos.Items)
                    {

                        TextBox txtfecha = item.FindControl("txtfecha") as TextBox;

                        if (dtDocumentos.Rows[item.ItemIndex][12] != null)
                        {
                            string Fecha = dtDocumentos.Rows[item.ItemIndex][12].ToString();

                            if (!string.IsNullOrEmpty(Fecha))
                            {
                                Fecha = Fecha.Substring(0, 10);
                                DateTime FechaFactura;
                                if (!DateTime.TryParseExact(Fecha, "dd/MM/yyyy", enUS, DateTimeStyles.AdjustToUniversal, out FechaFactura))
                                {

                                }
                                else
                                {
                                    txtfecha.Text = FechaFactura.ToString("dd/MM/yyyy");
                                    dtDocumentos.Rows[item.ItemIndex][16] = FechaFactura;

                                }
                            }
                        }

                    }
                }

            }
            else
            {
                foreach (RepeaterItem item in tablePaginationDocumentos.Items)
                {
                    
                    TextBox txtfecha = item.FindControl("txtfecha") as TextBox;

                    if (dtDocumentos.Rows[item.ItemIndex][12] != null)
                    {
                        string Fecha = dtDocumentos.Rows[item.ItemIndex][12].ToString();

                        if (!string.IsNullOrEmpty(Fecha))
                        {
                            Fecha = Fecha.Substring(0, 10);
                            DateTime FechaFactura;
                            if (!DateTime.TryParseExact(Fecha, "dd/MM/yyyy", enUS, DateTimeStyles.AdjustToUniversal, out FechaFactura))
                            {

                            }
                            else
                            {
                                txtfecha.Text = FechaFactura.ToString("dd/MM/yyyy");
                                dtDocumentos.Rows[item.ItemIndex][16] = FechaFactura;

                            }
                        }
                    }

                }
            }
            //xfinder.Visible = true;
            alerta.Attributes["class"] = string.Empty;
            alerta.Attributes["class"] = "msg-info";
        }
        protected void btsalvar_Click(object sender, EventArgs e)
        {
            try
            {
                ExportFileUpload();
            }
            catch (Exception ex)
            {
                this.Alerta(ex.Message);
            }
        }
        private void ExportFileUpload()
        {
            CultureInfo enUS = new CultureInfo("en-US");
            string mensaje = null;

            xmlDocumentos = null;
            string directorio = string.Empty;
            foreach (RepeaterItem item in tablePaginationDocumentos.Items)
            {
                CheckBox chkRevisado = item.FindControl("chkRevisado") as CheckBox;
                TextBox tcomentario = item.FindControl("tcomentario") as TextBox;
                TextBox txtfecha = item.FindControl("txtfecha") as TextBox;

                string Fecha = string.Format("{0}", txtfecha.Text.Trim());
                DateTime FechaFactura;
                DateTime? _Fecha = null;

                if (!DateTime.TryParseExact(Fecha, "dd/MM/yyyy", enUS, DateTimeStyles.AdjustToUniversal, out FechaFactura))
                {
                    dtDocumentos.Rows[item.ItemIndex][16] = null;
                    _Fecha = null;
                }
                else
                {
                    dtDocumentos.Rows[item.ItemIndex][16] = FechaFactura;
                    _Fecha = FechaFactura;

                }

                dtDocumentos.Rows[item.ItemIndex][13] = chkRevisado.Checked;
                dtDocumentos.Rows[item.ItemIndex][14] = tcomentario.Text;
                dtDocumentos.Rows[item.ItemIndex][15] = splaca;

                //actualiza fecha
                if (!credenciales.ActualizarSolicitudVehiculo(
                        numsolicitudempresa,
                        idsolveh,
                        dtDocumentos.Rows[item.ItemIndex][3].ToString(),
                        _Fecha,
                        Page.User.Identity.Name.ToUpper(),
                        out mensaje))
                {

                }

            }

            dtDocumentos.AcceptChanges();
            DataTable dtPadre = Session["dtDocumentosrevisasolicitudvehiculo"] as DataTable;
            DataTable dtHijo = new DataTable();
            dtHijo = dtDocumentos.Copy().Clone();
            foreach (DataRow drh in dtDocumentos.Rows)
            {
                dtHijo.Rows.Add(drh.ItemArray);
            }
            if (dtPadre != null)
            {
                if (dtPadre.Rows.Count > 0)
                {
                    DataRow[] rows;
                    rows = dtPadre.Select("Placa='" + splaca + "'");
                    foreach (DataRow r in rows)
                        r.Delete();
                    foreach (DataRow drp in dtPadre.Rows)
                    {
                        dtHijo.Rows.Add(drp.ItemArray);
                    }
                }
            }
            Session["dtDocumentosrevisasolicitudvehiculodocumentos"] = new DataTable();
            Session["dtDocumentosrevisasolicitudvehiculo"] = dtHijo;
            Response.Write("<script language='JavaScript'>window.close();</script>");
        }
        private void ExportFiles(string path, string filename, string name, string extensionfile, string sidsolicitud, string siddocemp)
        {
            path = path + "\\";
            String dateServer = credenciales.GetDateServer();
            String rutaServer = ConfigurationManager.AppSettings["rutaserver"].ToString() + dateServer;
            var llistaDoc = new List<listaDoc>();
            var ilistaDoc = new listaDoc(Convert.ToInt32(sidsolicitud), Convert.ToInt32(siddocemp), rutaServer + filename);
            llistaDoc.Add(ilistaDoc);
            foreach (var itemlistaDoc in llistaDoc)
            {
                var rutafile = path + filename ;
                dtDocumentos.Rows.Add(splaca, itemlistaDoc.iddocemp, itemlistaDoc.idtipemp, path, name, extensionfile);
            }
        }
        private class listaDoc
        {
            public int idtipemp { get; set; }
            public int iddocemp { get; set; }
            public string rutafile { get; set; }
            public listaDoc(int idtipemp, int iddocemp, string rutafile) { this.idtipemp = idtipemp; this.iddocemp = iddocemp; this.rutafile = rutafile.Trim(); }
        }
    }
}