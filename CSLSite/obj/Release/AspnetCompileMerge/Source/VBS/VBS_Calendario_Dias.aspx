<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VBS_Calendario_Dias.aspx.cs" Inherits="CSLSite.VBS.VBS_Calendario_Dias" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link href="https://cdnjs.cloudflare.com/ajax/libs/fullcalendar/3.10.2/fullcalendar.min.css" rel="stylesheet" />
    <script type="text/javascript" src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/moment.js/2.29.1/moment.min.js"></script>
    <script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/fullcalendar/3.10.2/fullcalendar.min.js"></script>
    <style type="text/css">
        .event-title {
            font-size: 14px; /* Tamaño de fuente ajustable */
        }

        .scroll-container {
            overflow-y: scroll;
            height: calc(100vh - 20px); /* Altura del contenedor: 100% del viewport - 20px de margen */
        }

        .selected-time {
            background-color: black; /* Color de fondo del rango seleccionado */
            opacity: 0.3; /* Opacidad del color de fondo */
        }
    </style>
</head>
<body>
    <div hidden="hidden">
      <input id="txtHoras"  />
    </div>
    <div class="scroll-container">
        <div id="calendar"></div>
    </div>
    

    <script type="text/javascript">
     
        $(document).ready(function () {
            var currentDate = moment().format("YYYY-MM-DD");
            var fechaActual = new Date();
            var fechaSiguiente = new Date();
            fechaSiguiente.setDate(fechaActual.getDate() + 1);
            var selectedEvents = [];
            var TimeForTwoDay = 0;
            var inputHoras = document.getElementById("txtHoras");

            // Lógica para obtener el valor de inputHoras usando AJAX
            $.ajax({
                url: 'VBS_Calendario_Dias.aspx/ConsultarParametroHorasTwoDay',
                type: 'POST',
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                success: function (data) {
                    console.log("data", data);
                    inputHoras.value = data.d;
                    console.log("inputhoras", inputHoras.value)
                    TimeForTwoDay = data.d;
                    console.log("timedas", TimeForTwoDay)
                    moment.locale('es', {
                        weekdays: [
                            'Domingo', 'Lunes', 'Martes', 'Miércoles', 'Jueves', 'Viernes', 'Sábado'
                        ],
                        weekdaysShort: ['Dom', 'Lun', 'Mar', 'Mié', 'Jue', 'Vie', 'Sáb'],
                        weekdaysMin: ['Do', 'Lu', 'Ma', 'Mi', 'Ju', 'Vi', 'Sá'],
                        months: [
                            'Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'
                        ],
                        monthsShort: [
                            'Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic'
                        ]
                    });
                    // Inicializar el calendario y configurar defaultView después de obtener el valor de inputHoras
                    $('#calendar').fullCalendar({
                        locale: 'es',
                        defaultView: fechaActual.getHours() >= inputHoras.value ? 'agendaTwoDay' : 'agendaDay',
                        header: false,
                        views: {
                            agendaTwoDay: {
                                type: 'agenda',
                                duration: { days: 2 },
                                buttonText: '2 días'
                            }
                        },

                        displayEventTime: false,
                        defaultDate: fechaActual,
                        selectable: true,
                        selectOverlap: false,
                        events: function (start, end, timezone, callback) {
                            // Realizar solicitud al servidor para obtener los eventos
                            $.ajax({
                                url: 'VBS_Calendario_Dias.aspx/ConsultarEventosDias',
                                type: 'POST',
                                data: JSON.stringify({ start: start.toISOString(), end: end.toISOString() }),
                                contentType: 'application/json; charset=utf-8',
                                dataType: 'json',
                                success: function (data) {
                                    console.log("data", data);
                                    var events = [];
                                    var jsonArray = JSON.parse(data.d);
                                    for (var i = 0; i < jsonArray.length; i++) {
                                        var evento = jsonArray[i];
                                        var startDate = moment(evento.start + ' ' + evento.horario, 'YYYY-MM-DD HH:mm:ss');
                                        var endDate = moment(startDate).add(1, 'hour');

                                        if (startDate.isAfter(moment())) { // Verificar si la fecha de inicio es posterior a la hora actual
                                            var event = {
                                                id: evento.idDetalle,
                                                title: evento.title,
                                                start: startDate,
                                                end: endDate,
                                                color: evento.color,
                                                className: 'event-title'
                                            };
                                            events.push(event);
                                        }
                                    }
                                    callback(events);  // Devolver los eventos al calendario
                                },
                                error: function (xhr, status, error) {
                                }
                            });
                        },
                        viewRender: function (view, element) {
                            // Obtener la hora actual formateada
                            var currentTime = moment();

                            // Filtrar los elementos para encontrar la hora actual
                            var targetHour = $('#calendar').find('.fc-axis.fc-time.fc-widget-content span').filter(function () {
                                var spanText = $(this).parent()[0].outerText.trim();
                                var spanTime = moment(spanText, 'h:mm a');
                                var spanTimePlusOneHour = moment(spanTime).add(1, 'hour');

                                return currentTime.isBetween(spanTime, spanTimePlusOneHour);
                            }).parent();

                            console.log("targetHour", targetHour);

                            if (targetHour.length > 0) {
                                // Ocultar horas anteriores a la hora actual en el grid de horas
                                var prevHours = targetHour.prevAll();
                                prevHours.hide();

                                // Eliminar las filas de horas anteriores en el DOM
                                var slatsContainer = $('.fc-time-grid .fc-slats');
                                var rowsToRemove = slatsContainer.find('tr').slice(0, prevHours.length);
                                rowsToRemove.remove();

                                var scrollContainer = $('.scroll-container');
                                var scrollPosition = targetHour.position().top;
                                scrollContainer.animate({ scrollTop: scrollPosition }, 500);
                            
                            }
                        }
                    });
                },
                error: function (xhr, status, error) {
                }
            });

            $('#calendar').css({
                width: '100%',
                height: '100vh'
            });
            setInterval(function () {
                $('#calendar').fullCalendar('refetchEvents');
                location.reload();
            }, 120000); // 120000 milisegundos = 2 minutos
        });

    </script>
</body>
</html>
