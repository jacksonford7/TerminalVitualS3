using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MonitorVacios.Entidades;
using System.Web.Services;
using ConectorN4;
using System.Xml.Linq;
using System.Xml;
using System.Web;
using System.IO;
using System.Threading.Tasks;
using System.Threading;

using System.Configuration;
using System.ComponentModel;
using System.Data;
using System.Drawing;


using System.Globalization;



namespace MonitorVacios
{
    public partial class FrmMonitor : Form
    {
        private Entidades.Contenedor objConteendor = new Entidades.Contenedor();
        public static string v_mensaje = string.Empty;

        CancellationTokenSource setcancel = new System.Threading.CancellationTokenSource();
        CancellationToken token;

        public FrmMonitor()
        {
            InitializeComponent();
        }

        private void ckTogle_CheckedChanged(object sender, EventArgs e)
        {

            try
            {

                ckTogle.Text = ckTogle.Checked ? "Detener" : "Monitorear";
                Progress<string> report = new Progress<string>(ReportFiller);

                if (ckTogle.Checked)
                {
                    lsMensajes.Items.Clear();
                    ReportFiller(string.Format("{0} Monitoreo Iniciado", DateTime.Now.ToString("yyyy/MM/dd HH:mm")));

                    setcancel = new System.Threading.CancellationTokenSource();
                    token = setcancel.Token;

                    var me = EdoTask.SeleccionarMetodoPorID("001");

                    var NRegistros = int.Parse(System.Configuration.ConfigurationManager.AppSettings["Max_Registro"]) ;
                    var Segundos = int.Parse(System.Configuration.ConfigurationManager.AppSettings["Segundos"]);
                    TaskTrigger.TaskLiteAlter(Segundos, me, report, NRegistros, "001", setcancel, token,"001");
                   

                }
                else
                {
                    ReportFiller(string.Format("{0} Monitoreo detenido", DateTime.Now.ToString("yyyy/MM/dd HH:mm")));
                    setcancel.Cancel();

                }
            }
            catch (Exception ex)
            {

                ReportFiller(string.Format("{0} Monitoreo con error: {1}", DateTime.Now.ToString("yyyy/MM/dd HH:mm"),ex.Message));
            }

           

           
        }

  
        private void ReportFiller(string report)
        {
            var toItem = report.Split(';').ToList();
            if (lsMensajes.Items.Count > 100) { lsMensajes.Items.Clear(); }
            foreach (var to in toItem)
            {
                if (!string.IsNullOrEmpty(to) && to.Length > 2)
                    this.lsMensajes.Items.Add(to.TrimEnd(';'));
            }
        }

     
        private void FrmMonitor_FormClosing(object sender, FormClosingEventArgs e)
        {
            setcancel.Cancel();
            GC.Collect();
        }
    }
}
