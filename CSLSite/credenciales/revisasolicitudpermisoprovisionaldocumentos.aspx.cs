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
    public partial class revisasolicitudpermisoprovisionaldocumentos : System.Web.UI.Page
    {
        private GetFile.Service getFile = new GetFile.Service();
        public DataTable dtDocumentos
        {
            get { return (DataTable)Session["dtDocumentosrevisasolicitudpermisoprovisionaldocumentos"]; }
            set { Session["dtDocumentosrevisasolicitudpermisoprovisionaldocumentos"] = value; }
        }
        private String xmlDocumentos;
        public String cedula
        {
            get { return (String)Session["cedularevisasolicitudpermisoprovisionaldocumentos"]; }
            set { Session["cedularevisasolicitudpermisoprovisionaldocumentos"] = value; }
        }
        public String numsolicitudempresa
        {
            get { return (String)Session["numsolicitudrevisasolicitudpermisoprovisionaldocumentos"]; }
            set { Session["numsolicitudrevisasolicitudpermisoprovisionaldocumentos"] = value; }
        }
        public String idsolcol
        {
            get { return (String)Session["idsolcoldocrevisasolicitudpermisoprovisionaldocumentos"]; }
            set { Session["idsolcoldocrevisasolicitudpermisoprovisionaldocumentos"] = value; }
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
            numsolicitudempresa = Request.QueryString["numsolicitud"];
            idsolcol = Request.QueryString["idsolcol"];
            cedula = Request.QueryString["cedula"];
            hfCedula.Value = cedula;
            dtDocumentos = credenciales.GetDocumentosColaboradorXNumSolicitudPaseProvisional(numsolicitudempresa, idsolcol);
            tablePaginationDocumentos.DataSource = dtDocumentos;
            tablePaginationDocumentos.DataBind();
            dtDocumentos.Columns.Add("DocumentoRechazado");
            dtDocumentos.Columns.Add("Comentario");
            dtDocumentos.Columns.Add("Cedula");
            DataTable dtPadre = Session["dtDocumentosrevisasolicitudcolaborador"] as DataTable;
            if (dtPadre != null)
            {
                var result = from myRow in dtPadre.AsEnumerable()
                             where myRow.Field<string>("Cedula") == cedula
                             select myRow;
                DataTable dt = result.AsDataView().ToTable();
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i <= dt.Rows.Count - 1; i++)
                    {
                        dtDocumentos.Rows[i][12] = dt.Rows[i][12];
                        dtDocumentos.Rows[i][13] = dt.Rows[i][13];
                    }
                    foreach (RepeaterItem item in tablePaginationDocumentos.Items)
                    {
                        CheckBox chkRevisado = item.FindControl("chkRevisado") as CheckBox;
                        TextBox tcomentario = item.FindControl("tcomentario") as TextBox;
                        chkRevisado.Checked = Convert.ToBoolean(dtDocumentos.Rows[item.ItemIndex][12]);
                        if (!chkRevisado.Checked)
                        {
                            tcomentario.Text = "";
                        }
                        else
                        {
                            tcomentario.Text = dtDocumentos.Rows[item.ItemIndex][13].ToString();
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
            xmlDocumentos = null;
            string directorio = string.Empty;
            foreach (RepeaterItem item in tablePaginationDocumentos.Items)
            {
                CheckBox chkRevisado = item.FindControl("chkRevisado") as CheckBox;
                TextBox tcomentario = item.FindControl("tcomentario") as TextBox;
                TextBox txtidsolicitud = item.FindControl("txtidsolicitud") as TextBox;
                TextBox txtiddocemp = item.FindControl("txtiddocemp") as TextBox;
                dtDocumentos.Rows[item.ItemIndex][12] = chkRevisado.Checked;
                dtDocumentos.Rows[item.ItemIndex][13] = tcomentario.Text;
                dtDocumentos.Rows[item.ItemIndex][14] = cedula;
            }
            dtDocumentos.AcceptChanges();
            DataTable dtPadre = Session["dtDocumentosrevisasolicitudcolaborador"] as DataTable;
            DataTable dtHijo = new DataTable();
            dtHijo = dtDocumentos.Copy().Clone();
            foreach (DataRow drh in dtDocumentos.Rows)
            {
                dtHijo.Rows.Add(drh.ItemArray);
            }
            if (dtPadre != null || dtPadre.Rows.Count > 0)
            {
                if (dtPadre.Rows.Count > 0)
                {
                    DataRow[] rows;
                    rows = dtPadre.Select("Cedula='" + cedula + "'");
                    foreach (DataRow r in rows)
                        r.Delete();
                    foreach (DataRow drp in dtPadre.Rows)
                    {
                        dtHijo.Rows.Add(drp.ItemArray);
                    }
                }
            }
            Session["dtDocumentosrevisasolicitudcolaboradordocumentos"] = new DataTable();
            Session["dtDocumentosrevisasolicitudcolaborador"] = dtHijo;
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
                var rutafile = path + filename /*+ '_' + DateTime.Now.ToString("dd-MM-yyyy")*/;
                dtDocumentos.Rows.Add(cedula, itemlistaDoc.iddocemp, itemlistaDoc.idtipemp, path, name, extensionfile);
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