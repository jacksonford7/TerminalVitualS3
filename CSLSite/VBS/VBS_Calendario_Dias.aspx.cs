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

namespace CSLSite.VBS
{
    public partial class VBS_Calendario_Dias : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }


        [WebMethod]
        public static string ConsultarEventosDias(string start, string end)
        {
            try
            {

                List<EventoCalendario> eventos = new List<EventoCalendario>();


                DateTime fechadesde = DateTime.ParseExact(start, "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture);

                string fechaDesdestring = fechadesde.ToString("yyyy-MM-dd");
                DateTime fechaHasta = DateTime.ParseExact(end, "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture).AddDays(-1);

                string fechaHastaString = fechaHasta.ToString("yyyy-MM-dd");

                VBS_CabeceraPlantilla objCab = new VBS_CabeceraPlantilla();
                var consultaTurnosDetalle = objCab.GetListaTurnosPorDosDias(fechaDesdestring, fechaHastaString);

                if (consultaTurnosDetalle.Resultado != null)
                {
                    foreach (VBS_TurnosDetalle detalle in consultaTurnosDetalle.Resultado)
                    {
                        EventoCalendario evento = new EventoCalendario();
                        evento.title = $"{detalle.TipoCargas} - {detalle.TipoContenedor} - {detalle.Cantidad} {"Disponible"} - ({detalle.Disponible})"; // Combinar tipo_contenedor y total_turnos
                        evento.start = detalle.VigenciaInicial.ToString("yyyy-MM-dd") + " " + detalle.Horario.ToString(@"hh\:mm\:ss");
                        evento.end = detalle.VigenciaInicial.ToString("yyyy-MM-dd") + " " + detalle.Horario.Add(TimeSpan.FromHours(1)).ToString(@"hh\:mm\:ss");

                        if (detalle.TipoCargaId == 1)
                            evento.color = "#336BFF";
                        if (detalle.TipoCargaId == 2)
                            evento.color = "#17a2b8";
                        if (detalle.TipoCargaId == 3)
                            evento.color = "#dc3545";

                        evento.horario = detalle.Horario.ToString(@"hh\:mm");

                        eventos.Add(evento);
                    }

                }

                JavaScriptSerializer serializer = new JavaScriptSerializer();
                string json = serializer.Serialize(eventos.ToArray());


                // Serializar los datos de eventos paginados a JSON

                return json;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al consultar eventos en el servidor", ex);
            }
        }


        [WebMethod]

        public static string ConsultarParametroHorasTwoDay()
        {
            VBS_CabeceraPlantilla objCab = new VBS_CabeceraPlantilla();
            var consultaTurnosDetalle = objCab.GetParametrosValida("Calendario_monitor");
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string json = serializer.Serialize(Convert.ToInt32(consultaTurnosDetalle));


            // Serializar los datos de eventos paginados a JSON

            return json;
        }
    }
}