using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using VBSEntidades;
using VBSEntidades.Calendario;
using VBSEntidades.ClaseEntidades;

namespace CSLSite
{
    public partial class VBS_Calendario_Monitor_Expo : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                //    this.getTurnosMonitor();
            }
            else
            {


            }

        }

        [WebMethod]
        public static string getTurnosMonitorExpo()
        {
            VBS_CabeceraPlantilla objCab = new VBS_CabeceraPlantilla();

            DateTime fechaActual = DateTime.Now;
            // fechaActual = fechaActual.AddDays(1); // Sumar un día a la fecha actual
            string fechaActualStr = fechaActual.ToString("MM-dd-yyyy");
            int idTipoCarga = 0;
            var listTurnos1 = objCab.GetListaTurnosExpoTBL1(fechaActualStr, fechaActualStr, idTipoCarga);
            var listTurnos2 = objCab.GetListaTurnosExpoTBL2(fechaActualStr, fechaActualStr, idTipoCarga);
            if (listTurnos1 != null && listTurnos2!=null)
            {
                VBS_TURNOS_MONITOR_UNIDOS turnosData = new VBS_TURNOS_MONITOR_UNIDOS
                {
                    Tabla1 = listTurnos1.Resultado,
                    Tabla2 = listTurnos2.Resultado
                };


                string json = JsonConvert.SerializeObject(turnosData);
                return json;
            }
            else
            {
                return null;
            }
         
            
        }
    }
}