<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="VBS_BAN_Calendario_Edit.aspx.cs" Inherits="CSLSite.VBS_BAN_Calendario_Edit" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Import Namespace="System.Web.Services" %>
<%@ Import Namespace="System.Web.Script.Services" %>

<asp:Content ID="Content1" ContentPlaceHolderID="placehead" runat="server">

    <link href="../img/favicon2.png" rel="icon" />
    <link href="../img/icono.png" rel="apple-touch-icon" />
    <!-- Bootstrap core CSS -->

    <!--external css-->
    <link href="../lib/font-awesome/css/font-awesome.css" rel="stylesheet" />
    <link rel="stylesheet" type="text/css" href="../lib/gritter/css/jquery.gritter.css" />

    <!-- Incluye los estilos CSS -->
    <link href="../css/icons.css" rel="stylesheet" />
    <link href="../css/style.css" rel="stylesheet" />
    <link href="../css/fullcalendar.min.css" rel="stylesheet" />
    
    <!-- Incluye los scripts JavaScript -->
    <script type="text/javascript" src="../lib/jquery/jquery-3.6.0.min.js"></script>
    <script type="text/javascript" src="../lib/moment/moment_2_29_1.min.js"></script>
    <script type="text/javascript" src="../Scripts/fullcalendar.min.js"></script>
    <link href="../css/sweetalert2.min.css" rel="stylesheet" /> 
    
    <link href="../css/all.min.css" rel="stylesheet" />
    <script type="text/javascript" src="../js/Confirmaciones.js""></script>
    <script type="text/javascript" src="../lib/pages.js"></script>
    <link rel="stylesheet" type="text/css" href="../css/calendarioStyle.css"/>

    <style type="text/css">
        .modal-vertical-right .modal-dialog {
            top: 0;
            right: 0;
            margin-right: 20vh;
            transform: translateX(100%);
            max-width: 40%;
            height: 100%;
        }

        .modal-vertical-right .modal-content {
            height: 100%;
            overflow-y: auto;
        }

        .modal-vertical-right .modal-body {
            padding: 15px;
        }
    </style>

    <script type="text/javascript">

        var tablaData;
        var idDetalle;
        var fechaIni;
        var fechaFin;
        var tipoCargaVacios;
        var HorarioVacios;
        var idDetalleVacios;
        var secuencia = 1; // Variable para generar la secuencia de forma incremental

        $(document).ready(function ()
        {
            $('#calendarImpo').hide();
            $('#calendarlineas').hide();

            var initialLoad = true; // Bandera para controlar la carga inicial
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

            $('#calendar').fullCalendar(
            {
                locale: 'es',
                header: {
                    left: 'prev,next',
                    center: 'title',

                    right: 'month,agendaWeek,agendaDay'
                },
                defaultDate: moment().format('YYYY-MM-DD'),
                buttonIcons: true,
                weekNumbers: false,
                editable: true,
                eventLimit: true,
                selectable: true,
                eventTextColor: '#000000',
                slotDuration: '01:00:00',
                slotEventOverlap: false,
                displayEventTime: false,
                buttonText: {
                    month: 'Mes',
                    week: 'Semana',
                    day: 'Día'
                },

                events: function (start, end, timezone, callback)
                {
                    var year = start.year();
                    var month = start.month() + 1;
                    // Realiza una llamada AJAX para obtener los eventos del servidor
                        $.ajax({
                            url: 'VBS_BAN_Calendario_Edit.aspx/GetCalendarEvents',
                            type: 'POST',
                            data: JSON.stringify({ year: year, month: month, inicial: initialLoad }),
                            contentType: 'application/json; charset=utf-8',
                            dataType: 'json',
                            success: function (data) {
                                var jsonArray = JSON.parse(data.d);
                                callback(jsonArray);
                            },
                            error: function () {
                            }
                        });
                    },

                dayClick: function (date, jsEvent, view)
                {
                    var start = date.toISOString(); // Fecha de inicio del día seleccionado

                    var currentDate = moment().startOf('day'); // Obtener la fecha actual sin la hora

                    var currentDateFormatted = currentDate.format('YYYY-MM-DD'); // Formatear la fecha actual
                    var events = $('#calendar').fullCalendar('clientEvents', function (event) {


                        return event.start.isSame(date, 'day');
                    });


                    if (start < currentDateFormatted) {

                        mostrarAdvertencia("No puede seleccionar un rango de fecha anterior al actual");
                        return; // Salir de la función sin mostrar el modal
                    }

                    if (events.length > 0) {

                        $.ajax({
                            url: 'VBS_BAN_Calendario_Edit.aspx/ConsultarTablaExpoPorDia',
                            type: 'POST',
                            data: JSON.stringify({ start: start }),
                            contentType: 'application/json; charset=utf-8',
                            dataType: 'json',
                            success: function (data) {
                                var events = [];
                                var jsonArray = JSON.parse(data.d);
                                for (var i = 0; i < jsonArray.length; i++) {
                                    var evento = jsonArray[i];
                                    var fila = document.createElement('tr');
                                    fila.innerHTML = `
                                    <td class="center hidden-phone">${secuencia}</td>
                                    <td class="center hidden-phone">${evento.codigoBloque}</td>
                                    <td class="center hidden-phone">${evento.cantidad}</td>
                                    <td class="center hidden-phone">
                                    <button class="btn btn-primary" onclick="eliminarDetalle(this)" disabled>QUITAR</button>
                                    </td>
                                       `;

                                    // Agregar los datos del detalle como atributos personalizados a la fila                                    
                                    fila.setAttribute('data-tipo-carga-id', evento.tipoBloqueId);

                                    // Agregar la fila al cuerpo de la tabla
                                    document.getElementById('tbodyDetalles').appendChild(fila);

                                    // Incrementar la secuencia para el siguiente detalle
                                    secuencia++;
                                }
                                // Agregar los nuevos eventos uno por uno sin eliminar los existentes
                                for (var j = 0; j < events.length; j++) {
                                    $('#calendar').fullCalendar('renderEvent', events[j], true);
                                }
                            },
                            error: function () {
                            }
                        });
                    }
                    // Tu código para el evento dayClick...
                    $('#modalt').modal('show');
                    document.getElementById('<%=txtVigenciaInicial.ClientID %>').value = start;
                    document.getElementById('<%=txtVigenciaFinal.ClientID %>').value = start;

                    // Fecha original
                    var fechaOriginalDesde = new Date(start);
                    fechaOriginalDesde.setDate(fechaOriginalDesde.getDate() + 1);
                    // Convertir a la nueva fecha
                    var fechaDesde = fechaOriginalDesde.toLocaleDateString('en-CA');

                    var fechaOriginalHasta = new Date(start);

                    // Convertir a la nueva fecha
                    var fechaHasta = fechaOriginalHasta.toLocaleDateString('en-CA');

                    document.getElementById('<%=txtFechaDesde.ClientID %>').value = fechaDesde;
                    document.getElementById('<%=txtFechaHasta.ClientID %>').value = fechaDesde;
                    document.getElementById('<%=txtFechaDesde.ClientID %>').readOnly = true;
                    document.getElementById('<%=txtFechaHasta.ClientID %>').readOnly = true;

                    document.getElementById('<%=banderaDayClick.ClientID %>').value = 1;
                },

                eventClick: function (calEvent, jsEvent, view) {


                        var currentDate = moment().startOf('day'); // Obtener la fecha actual sin la hora
                        var selectedDate = calEvent.start.format(); // Obtener la fecha seleccionada sin la hora

                        var currentDateFormatted = currentDate.format('YYYY-MM-DD'); // Formatear la fecha actual
                        // Formatear la fecha seleccionada

                        if (selectedDate < currentDateFormatted) {

                            mostrarAdvertencia("No puede seleccionar un rango de fecha anterior al actual");
                            return; // Salir de la función sin mostrar el modal
                        }
                        $('#modalActu').modal('show');

                        document.getElementById('<%=txtFechaActu.ClientID %>').value = calEvent.start.format();
                        document.getElementById('<%=txtFechaActu.ClientID %>').readOnly = true;

                    
                        document.getElementById('<%=txtTipoCargas.ClientID %>').value = calEvent.codigoBloque;
                        document.getElementById('<%=txtTipoCargas.ClientID %>').readOnly = true;
                    
                        idDetalle = calEvent.idDetalle;
                        fechaIni = calEvent.start.format();
                        fechaFin = calEvent.end.format();

                   
                        $.ajax({
                            url: 'VBS_BAN_Calendario_Edit.aspx/ConsultarEventos',
                            type: 'POST',
                            data: JSON.stringify({ idDetalle: calEvent.idDetalle, start: calEvent.start.format(), end: calEvent.end.format() }),
                            contentType: 'application/json; charset=utf-8',
                            dataType: 'json',
                            success: function (data) {
                                var eventos = JSON.parse(data.d);
                                tablaData = eventos;
                                $('#cuerpoTabla').empty();
                                // Llenar la tabla con los datos de eventos y pasar los parámetros de paginación
                                LlenarTabla(eventos);
                            },
                            error: function () {

                            }
                        });
                    },

                viewRender: function (view, element)
                {
                    $('.fc-axis.fc-time.fc-widget-content').css('height', 60 + 'px');
                    if (view.name === 'agendaDay')
                    {
                        // Ajustar la altura de las filas de la tabla para mostrar más eventos
                        var rowHeight = 60; // Altura en píxeles de cada fila
                        var eventCount = 4; // Número máximo de eventos que se mostrarán en cada hora

                        // Establecer la altura de las filas de la tabla
                        $('.fc-axis.fc-time.fc-widget-content').css('height', 60 + 'px');


                        var selectedDate = view.start.format('YYYY-MM-DD'); // Obtener la fecha del día en formato deseado
                        // Realizar la consulta específica cuando se cambie a la vista de agenda por día
                        $.ajax({
                            url: 'VBS_BAN_Calendario_Edit.aspx/ConsultarEventosPorDia',
                            type: 'POST',
                            data: JSON.stringify({ start: selectedDate }),
                            contentType: 'application/json; charset=utf-8',
                            dataType: 'json',
                            success: function (data) {
                                var events = [];
                                var jsonArray = JSON.parse(data.d);
                                for (var i = 0; i < jsonArray.length; i++) {
                                    var evento = jsonArray[i];
                                    var startDateTime = moment(selectedDate + ' ' + evento.horario, 'YYYY-MM-DD HH:mm:ss');
                                    var endDateTime = startDateTime.clone().add(1, 'hour');
                                    var event = {
                                        id: evento.idDetalle,
                                        title: evento.title.trim(),
                                        start: startDateTime,
                                        end: endDateTime,
                                        color: evento.color
                                    };
                                    events.push(event);
                                }
                                // Agregar los nuevos eventos uno por uno sin eliminar los existentes
                                for (var j = 0; j < events.length; j++) {
                                    $('#calendar').fullCalendar('renderEvent', events[j], true);
                                }
                            },
                            error: function ()
                            {

                            }
                        });
                    }

                    else if (view.name === 'month')
                    {

                        if (initialLoad) {
                            initialLoad = false; // Desactivar la bandera de carga inicial
                        } else {
                            var month = parseInt(view.start.format('MM'));
                            month = month + 1;
                            var year = parseInt(view.start.format('YYYY'));
                            // Remover los eventos existentes
                            $('#calendar').fullCalendar('removeEvents');
                            $.ajax({
                                url: 'VBS_BAN_Calendario_Edit.aspx/GetCalendarEvents',
                                type: 'POST',
                                data: JSON.stringify({ year: year, month: month, inicial: initialLoad }),
                                contentType: 'application/json; charset=utf-8',
                                dataType: 'json',
                                success: function (data) {
                                    var jsonArray = JSON.parse(data.d);
                                    // Agregar los nuevos eventos al FullCalendar
                                    $('#calendar').fullCalendar('addEventSource', jsonArray);
                                },
                                error: function () {

                                }
                            });
                        }
                    }

                },

                select: function (start, end, jsEvent, view)
                {
                    var currentDate = moment().startOf('day'); // Obtener la fecha actual sin la hora
                    var selectedDate = moment(start).startOf('day'); // Obtener la fecha seleccionada sin la hora

                    var currentDateFormatted = currentDate.format('YYYY-MM-DD'); // Formatear la fecha actual
                    var selectedDateFormatted = selectedDate.format('YYYY-MM-DD'); // Formatear la fecha seleccionada


                    if (selectedDateFormatted < currentDateFormatted) {
                        mostrarAdvertencia("No puede seleccionar un rango de fecha anterior al actual");
                        return; // Salir de la función sin mostrar el modal
                    }


                    var selectedEvents = $('#calendar').fullCalendar('clientEvents', function (event) {
                        return event.start.isBetween(start, end, 'day', '[]');
                    });

                    if (selectedEvents.length > 0) {
                        // Si hay eventos seleccionados, puedes agregar lógica adicional aquí si es necesario
                    } else {

                        $('#modalt').modal('show');
                        document.getElementById('<%=txtVigenciaInicial.ClientID %>').value = start;
                        document.getElementById('<%=txtVigenciaFinal.ClientID %>').value = end;

                        // Fecha original
                        var fechaOriginalDesde = new Date(start);
                        fechaOriginalDesde.setDate(fechaOriginalDesde.getDate() + 1);
                        // Convertir a la nueva fecha
                        var fechaDesde = fechaOriginalDesde.toLocaleDateString('en-CA');

                        var fechaOriginalHasta = new Date(end);

                        // Convertir a la nueva fecha
                        var fechaHasta = fechaOriginalHasta.toLocaleDateString('en-CA');

                        document.getElementById('<%=txtFechaDesde.ClientID %>').value = fechaDesde;
                        document.getElementById('<%=txtFechaHasta.ClientID %>').value = fechaHasta;
                        document.getElementById('<%=txtFechaDesde.ClientID %>').readOnly = true;
                        document.getElementById('<%=txtFechaHasta.ClientID %>').readOnly = true;
                        document.getElementById('<%=banderaDayClick.ClientID %>').value = 0;
                    }

                },

                eventMouseover: function (calEvent, jsEvent)
                {
                    // Obtener el elemento HTML del evento
                    var eventElement = $(this);

                    // Aplicar estilos de resaltado al evento
                    eventElement.addClass('highlight-event');

                    // Obtener el contenido completo del evento
                    var eventTitle = calEvent.title.trim();


                    // Crear un elemento emergente para mostrar el contenido completo
                    var tooltip = $('<div class="event-tooltip"></div>');
                    tooltip.append('<div class="event-title">' + eventTitle + '</div>');


                    // Posicionar y mostrar el elemento emergente
                    $('body').append(tooltip);
                    tooltip.css({
                        top: jsEvent.pageY,
                        left: jsEvent.pageX
                    }).show();
                },

                eventMouseout: function ()
                {
                    // Remover los estilos de resaltado del evento
                    $(this).removeClass('highlight-event');

                    // Remover el elemento emergente
                    $('.event-tooltip').remove();
                },

            });

        });//fin calencadio expo


        $(document).ready(function ()
        {


            // Evento de clic para el botón "Cerrar" del modal
            $('.modal').on('click', '[data-dismiss="modal"]', function () {
                // Limpiar la tabla de detalles
                document.getElementById('<%=TxtCantidad.ClientID %>').value = null;

                $('#tbodyDetalles').empty();

                secuencia = 1;
            });
        });


        $(document).ready(function ()
        {
            // Manejar el evento de clic en el botón "Guardar"
            $("#btnGuardar").click(function () {
                guardarCambiosTabla();
            });
        });

        $(document).ready(function ()
        {
            // Manejar el evento de clic en el botón "Guardar"
            $("#btnGuardarImport").click(function () {
                guardarCambiosTablaImport();
            });
        });

        //expo por lineas
         $(document).ready(function ()
        {
            // Manejar el evento de clic en el botón "Guardar"
            $("#btnGuardarLineas").click(function () {
                guardarCambiosTablaLineas();
            });
         });

        $(document).ready(function ()
        {
            // Manejar el evento de clic en el botón "Guardar"
            $("#btnModalVaciosActu").click(function () {
                guardarCambiosTablaVacios();
            });
        });



        $(document).ready(function ()
        {
            var fila = null; // Mantener fila en un ámbito más amplio para que sea accesible en ambos eventos

            // Manejar el evento keydown en los elementos .cantidad-input
            $(document).on('keydown', '.cantidad-input', function (event) {
                var input = $(this);
                fila = input.closest('tr'); // Asignar fila aquí para que esté disponible en ambos eventos
                var cantidadInicial = parseInt(input.val());

                if ((event.which === 9 || event.which === 13) && input.val() !== "") {
                    //       event.preventDefault(); // Evitar el comportamiento predeterminado del Tab o Enter
                    validarCantidad(input); // Pasar input como argumento a la función
                    moveToNextRow();
                }
            });

            // Manejar el evento click en cualquier parte del documento
            $(document).on('click', function (event) {

                if (typeof fila !== 'undefined' && fila !== null) {
                    var input = fila.find('.cantidad-input'); // Obtener el input asociado a la fila
                    if (input.length && !input.is(event.target)) {
                        validarCantidad(input); // Pasar input como argumento a la función
                    }
                }
            });

            function validarCantidad(input) {
                try {
                    var cantidadModificada = parseInt(input.val());

                    var cantidadAsignada = parseInt(fila.find('.lblAsignados').text());
                    var cantidadDisponible = parseInt(fila.find('.lblDisponible').text());

                    if (cantidadModificada < cantidadAsignada)
                    {
                        var cantidadInicial = (cantidadAsignada + cantidadDisponible);
                        alert("No se permiten valores menores a lo ya asignado " + cantidadAsignada + ", sugerido: " +  cantidadInicial);
                        input.val(cantidadInicial);
                    }
                    else {
                        if (isNaN(cantidadModificada)) {
                            cantidadModificada = cantidadInicial;
                        }

                        if (cantidadModificada >= 0) {
                            var nuevaDisponible = cantidadModificada - cantidadAsignada;
                            fila.find('.lblDisponible').text(nuevaDisponible);
                        } else {
                            alert("No se permiten valores negativos");
                            var cantidadInicial = (cantidadAsignada + cantidadDisponible);
                            input.val(cantidadInicial);
                        }
                    }
                } catch (error) {
                    input.val(cantidadInicial);
                }
            }

            function moveToNextRow()
            {
                var rows = $('table#miTabla tr');
                var currentRowIndex = rows.index(fila);
                if (currentRowIndex < rows.length - 1) {
                    var nextRow = rows.eq(currentRowIndex + 1);
                    var nextInput = nextRow.find('.cantidad-input:first');
                    if (nextInput.length) {
                        nextInput.focus();
                    }
                }
            }
        });




        window.onload = function () {
       
            document.getElementById("DropDownList1").addEventListener("change", function () {
                var selectedValue = this.value;
                var initialLoad = true;
                // Update the calendar based on the selected value
                if (selectedValue === "1")
                {
                    // Show the "Exportación" calendar and hide the others
                    $('#calendar').show();
                    $('#calendar').fullCalendar('refetchEvents');
                    $('#calendar').fullCalendar(''); // Update the size of the calendar
                    $('#calendarImpo').hide();
                    $('#calendarlineas').hide();
                }
                else if (selectedValue === "2")
                {
                    $('#calendarImpo').show();
                    $('#calendarImpo').fullCalendar('refetchEvents');
                    $('#calendar').hide();
                    
                    //var initialLoad = true;
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
                    $('#calendarImpo').fullCalendar({
                        locale: 'es',
                        header: {
                            left: 'prev,next',
                            center: 'title',
                            right: 'month,agendaWeek,agendaDay'
                        },
                        defaultDate: moment().format('YYYY-MM-DD'),
                        buttonIcons: true,
                        weekNumbers: false,
                        editable: true,
                        eventLimit: true,
                        selectable: true,
                        eventTextColor: '#000000',
                        slotDuration: '01:00:00',
                        slotEventOverlap: false,
                        displayEventTime: false,
                        buttonText: {
                            month: 'Mes',
                            week: 'Semana',
                            day: 'Día'
                        },
                     
                        events: function (start, end, timezone, callback) {
                            var allEvents = []; // Variable para almacenar todos los eventos
                            var pageSize = 300;
                            var pageIndex = 0;
                            var year = start.year();
                            var month = start.month() + 1;
                            var initialLoad = true; // Variable para la primera carga

                            function fetchEvents() {
                                $.ajax({
                                    url: 'VBS_BAN_Calendario_Edit.aspx/GetCalendarEventsImport',
                                    type: 'POST',
                                    data: JSON.stringify({ year: year, month: month, inicial: initialLoad, pageIndex: pageIndex, pageSize: pageSize }),
                                    contentType: 'application/json; charset=utf-8',
                                    dataType: 'json',
                                    success: function (data) {
                                        var jsonArray = JSON.parse(data.d);
                                        console.log("Datos:", jsonArray);

                                        // Agregar los eventos al arreglo
                                        allEvents = allEvents.concat(jsonArray);

                                        // Incrementar el índice de página para la siguiente llamada
                                        pageIndex++;

                                        // Si hay más eventos, solicitar la siguiente página
                                        if (jsonArray.length === pageSize) {
                                            // Llama de nuevo para la siguiente página
                                            fetchEvents();
                                        } else {
                                            // Si no hay más eventos, llama al callback con todos los eventos obtenidos
                                            callback(allEvents);
                                        }
                                    },
                                    error: function () {
                                        // Manejo de errores
                                    }
                                });
                            }

                            fetchEvents(); // Inicia la primera carga
                        },

                        dayClick: function (date, jsEvent, view)
                        {
                           
                            var cuerpoTabla = $("#tbodyDetallesIMPORT");
                            cuerpoTabla.empty();
                           // $('#tbodyDetallesIMPORT').empty();//nuevo

                            var start = date.toISOString(); // Fecha de inicio del día seleccionado

                            var currentDate = moment().startOf('day'); // Obtener la fecha actual sin la hora

                            var currentDateFormatted = currentDate.format('YYYY-MM-DD'); // Formatear la fecha actual
                            var events = $('#calendarImpo').fullCalendar('clientEvents', function (event) {


                                return event.start.isSame(date, 'day');
                            });


                            if (start < currentDateFormatted) {

                                mostrarAdvertencia("No puede seleccionar un rango de fecha anterior al actual");
                                return; // Salir de la función sin mostrar el modal
                            }

                            if (events.length > 0) {

                                
                                $.ajax({
                                    url: 'VBS_BAN_Calendario_Edit.aspx/ConsultarTablaImportPorDia',
                                    type: 'POST',
                                    data: JSON.stringify({ start: start }),
                                    contentType: 'application/json; charset=utf-8',
                                    dataType: 'json',
                                    success: function (data) {


                                        var events = [];
                                        var jsonArray = JSON.parse(data.d);
                                        for (var i = 0; i < jsonArray.length; i++) {
                                            var evento = jsonArray[i];
                                            var fila = document.createElement('tr');
                                            fila.innerHTML = `
                                    <td class="center hidden-phone">${secuencia}</td>
                                    <td class="center hidden-phone">${evento.codigoBloque}</td>
                                    <td class="center hidden-phone">${evento.cantidad}</td>
                                    <td class="center hidden-phone">
                                    <button class="btn btn-primary" onclick="eliminarDetalle(this)" disabled>QUITAR</button>
                                    </td>
                                       `;
                                            // Agregar los datos del detalle como atributos personalizados a la fila
                                            fila.setAttribute('data-tipo-Bloque-id', evento.tipoBloqueId);

                                            // Agregar la fila al cuerpo de la tabla
                                            document.getElementById('tbodyDetallesIMPORT').appendChild(fila);

                                            // Incrementar la secuencia para el siguiente detalle
                                            secuencia++;
                                        }
                                        // Agregar los nuevos eventos uno por uno sin eliminar los existentes
                                        for (var j = 0; j < events.length; j++) {
                                            $('#calendar').fullCalendar('renderEvent', events[j], true);
                                        }
                                    },
                                    error: function () {

                                    }
                                });
                            }
                            // Tu código para el evento dayClick...
                            $('#modaltImport').modal('show');
                            document.getElementById('<%=txtVigenciaInicial.ClientID %>').value = start;
                            document.getElementById('<%=txtVigenciaFinal.ClientID %>').value = start;

                            // Fecha original
                            var fechaOriginalDesde = new Date(start);
                            fechaOriginalDesde.setDate(fechaOriginalDesde.getDate() + 1);
                            // Convertir a la nueva fecha
                            var fechaDesde = fechaOriginalDesde.toLocaleDateString('en-CA');

                            var fechaOriginalHasta = new Date(start);

                            // Convertir a la nueva fecha
                            var fechaHasta = fechaOriginalHasta.toLocaleDateString('en-CA');

                            document.getElementById('<%=txtFechaDImport.ClientID %>').value = fechaDesde;
                            document.getElementById('<%=txtFechaHImport.ClientID %>').value = fechaDesde;
                            document.getElementById('<%=txtFechaDImport.ClientID %>').readOnly = true;
                            document.getElementById('<%=txtFechaHImport.ClientID %>').readOnly = true;

                            document.getElementById('<%=banderaDayClick.ClientID %>').value = 1;
                        },

                        eventClick: function (calEvent, jsEvent, view)
                        {
                            //alert("ww");

                            var currentDate = moment().startOf('day'); // Obtener la fecha actual sin la hora
                            var selectedDate = calEvent.start.format(); // Obtener la fecha seleccionada sin la hora

                            var currentDateFormatted = currentDate.format('YYYY-MM-DD'); // Formatear la fecha actual
                            // Formatear la fecha seleccionada

                            if (selectedDate < currentDateFormatted) {

                                mostrarAdvertencia("No puede seleccionar un rango de fecha anterior al actual");
                                return; // Salir de la función sin mostrar el modal
                            }
                            $('#modalActuImport').modal('show');

                            document.getElementById('<%=txtFechaImport.ClientID %>').value = calEvent.start.format();
                            document.getElementById('<%=txtFechaImport.ClientID %>').readOnly = true;

                            idDetalle = calEvent.idDetalle;

                            $.ajax({
                                url: 'VBS_BAN_Calendario_Edit.aspx/ConsultarEventosImport',
                                type: 'POST',
                                data: JSON.stringify({ idDetalle: calEvent.idDetalle, start: calEvent.start.format(), end: calEvent.end.format() }),
                                contentType: 'application/json; charset=utf-8',
                                dataType: 'json',
                                success: function (data) {
                                    var eventos = JSON.parse(data.d);

                                   // $('#cuerpoTablaImport').empty();//nuevo
                                    // $('#tbodyDetallesIMPORT').remove(rows);
                                    LlenarTablaImport(eventos);
                                },
                                error: function () {

                                }
                            });
                        },
                        viewRender: function (view, element) {
                            $('.fc-axis.fc-time.fc-widget-content').css('height', 60 + 'px');
                            if (view.name === 'agendaDay') {
                                // Ajustar la altura de las filas de la tabla para mostrar más eventos
                                var rowHeight = 60; // Altura en píxeles de cada fila
                                var eventCount = 4; // Número máximo de eventos que se mostrarán en cada hora

                                // Establecer la altura de las filas de la tabla
                                $('.fc-axis.fc-time.fc-widget-content').css('height', 60 + 'px');


                                var selectedDate = view.start.format('YYYY-MM-DD'); // Obtener la fecha del día en formato deseado
                                // Realizar la consulta específica cuando se cambie a la vista de agenda por día
                                $.ajax({
                                    url: 'VBS_BAN_Calendario_Edit.aspx/ConsultarEventosPorDiaImport',
                                    type: 'POST',
                                    data: JSON.stringify({ start: selectedDate }),
                                    contentType: 'application/json; charset=utf-8',
                                    dataType: 'json',
                                    success: function (data) {
                                        var events = [];
                                        var jsonArray = JSON.parse(data.d);
                                        for (var i = 0; i < jsonArray.length; i++) {
                                            var evento = jsonArray[i];
                                            var startDateTime = moment(selectedDate + ' ' + evento.horario, 'YYYY-MM-DD HH:mm:ss');
                                            var endDateTime = startDateTime.clone().add(1, 'hour');
                                            var event = {
                                                //  id: evento.idDetalle,
                                                title: evento.title.trim(),
                                                start: startDateTime,
                                                end: endDateTime,
                                                color: evento.color
                                            };
                                            events.push(event);
                                        }
                                        // Agregar los nuevos eventos uno por uno sin eliminar los existentes
                                        for (var j = 0; j < events.length; j++) {
                                            $('#calendarImpo').fullCalendar('renderEvent', events[j], true);
                                        }
                                    },
                                    error: function () {

                                    }
                                });
                            }

                            else if (view.name === 'month') {

                                if (initialLoad) {
                                    initialLoad = false; // Desactivar la bandera de carga inicial
                                }
                                else {
                                    var pageSize = 300;
                                    var pageIndex = 0;
                                    var month = parseInt(view.start.format('MM'));
                                    month = month + 1;
                                    var year = parseInt(view.start.format('YYYY'));
                                    // Remover los eventos existentes
                                    $('#calendarImpo').fullCalendar('removeEvents');
                                    $.ajax({
                                        url: 'VBS_BAN_Calendario_Edit.aspx/GetCalendarEventsImport',
                                        type: 'POST',
                                        data: JSON.stringify({ year: year, month: month, inicial: initialLoad, pageIndex: pageIndex, pageSize: pageSize }),
                                        contentType: 'application/json; charset=utf-8',
                                        dataType: 'json',
                                        success: function (data) {
                                            var jsonArray = JSON.parse(data.d);
                                            // Agregar los nuevos eventos al FullCalendar
                                            $('#calendarImpo').fullCalendar('addEventSource', jsonArray);
                                        },
                                        error: function () {

                                        }
                                    });
                                }
                            }

                        },

                        select: function (start, end, jsEvent, view) {
                            var currentDate = moment().startOf('day'); // Obtener la fecha actual sin la hora
                            var selectedDate = moment(start).startOf('day'); // Obtener la fecha seleccionada sin la hora

                            var currentDateFormatted = currentDate.format('YYYY-MM-DD'); // Formatear la fecha actual
                            var selectedDateFormatted = selectedDate.format('YYYY-MM-DD'); // Formatear la fecha seleccionada


                            if (selectedDateFormatted < currentDateFormatted) {
                                mostrarAdvertencia("No puede seleccionar un rango de fecha anterior al actual");
                                return; // Salir de la función sin mostrar el modal
                            }


                            var selectedEvents = $('#calendarImpo').fullCalendar('clientEvents', function (event) {
                                return event.start.isBetween(start, end, 'day', '[]');
                            });

                            if (selectedEvents.length > 0) {
                                // Si hay eventos seleccionados, puedes agregar lógica adicional aquí si es necesario
                            }
                            else {

                                $('#modaltImport').modal('show');
                                document.getElementById('<%=txtVigenciaInicial.ClientID %>').value = start;
                                document.getElementById('<%=txtVigenciaFinal.ClientID %>').value = end;

                                // Fecha original
                                var fechaOriginalDesde = new Date(start);
                                fechaOriginalDesde.setDate(fechaOriginalDesde.getDate() + 1);
                                // Convertir a la nueva fecha
                                var fechaDesde = fechaOriginalDesde.toLocaleDateString('en-CA');

                                var fechaOriginalHasta = new Date(end);

                                // Convertir a la nueva fecha
                                var fechaHasta = fechaOriginalHasta.toLocaleDateString('en-CA');

                                document.getElementById('<%=txtFechaDImport.ClientID %>').value = fechaDesde;
                                document.getElementById('<%=txtFechaHImport.ClientID %>').value = fechaHasta;
                                document.getElementById('<%=txtFechaDImport.ClientID %>').readOnly = true;
                                document.getElementById('<%=txtFechaHImport.ClientID %>').readOnly = true;
                                document.getElementById('<%=banderaDayClick.ClientID %>').value = 0;

                                

                            }

                        },

                        eventMouseover: function (calEvent, jsEvent) {
                            // Obtener el elemento HTML del evento
                            var eventElement = $(this);

                            // Aplicar estilos de resaltado al evento
                            eventElement.addClass('highlight-event');

                            // Obtener el contenido completo del evento
                            var eventTitle = calEvent.title.trim();


                            // Crear un elemento emergente para mostrar el contenido completo
                            var tooltip = $('<div class="event-tooltip"></div>');
                            tooltip.append('<div class="event-title">' + eventTitle + '</div>');


                            // Posicionar y mostrar el elemento emergente
                            $('body').append(tooltip);
                            tooltip.css({
                                top: jsEvent.pageY,
                                left: jsEvent.pageX
                            }).show();
                        },
                        eventMouseout: function () {
                            // Remover los estilos de resaltado del evento
                            $(this).removeClass('highlight-event');

                            // Remover el elemento emergente
                            $('.event-tooltip').remove();
                        },
                    });

                    // Show the "Importación" calendar and hide the others
                    //$('#calendar').hide();
                    //$('#calendarImpo').show();
                    //$('#calendarImpo').fullCalendar(); // Update the size of the calendar
                    
                }
                else if (selectedValue === "3")//turnos de exportacion por lineas
                {
                    $('#calendarlineas').show();
                    $('#calendarlineas').fullCalendar('refetchEvents');
                    $('#calendar').hide();
                    $('#calendarImpo').hide();

                    //var initialLoad = true;
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

                    $('#calendarlineas').fullCalendar({
                        locale: 'es',
                        header: {
                            left: 'prev,next',
                            center: 'title',
                            right: 'month,agendaWeek,agendaDay'
                        },
                        defaultDate: moment().format('YYYY-MM-DD'),
                        buttonIcons: true,
                        weekNumbers: false,
                        editable: true,
                        eventLimit: true,
                        selectable: true,
                        eventTextColor: '#000000',
                        slotDuration: '01:00:00',
                        slotEventOverlap: false,
                        displayEventTime: false,
                        buttonText: {
                            month: 'Mes',
                            week: 'Semana',
                            day: 'Día'
                        },
                     
                        events: function (start, end, timezone, callback)
                        {
                            var allEvents = []; // Variable para almacenar todos los eventos
                            var pageSize = 300;
                            var pageIndex = 0;
                            var year = start.year();
                            var month = start.month() + 1;
                            var initialLoad = true; // Variable para la primera carga

                            function fetchEvents() {
                                $.ajax({
                                    url: 'VBS_BAN_Calendario_Edit.aspx/GetCalendarEventsExpoLineas',
                                    type: 'POST',
                                    data: JSON.stringify({ year: year, month: month, inicial: initialLoad, pageIndex: pageIndex, pageSize: pageSize }),
                                    contentType: 'application/json; charset=utf-8',
                                    dataType: 'json',
                                    success: function (data) {
                                        var jsonArray = JSON.parse(data.d);
                                        console.log("Datos:", jsonArray);

                                        // Agregar los eventos al arreglo
                                        allEvents = allEvents.concat(jsonArray);

                                        // Incrementar el índice de página para la siguiente llamada
                                        pageIndex++;

                                        // Si hay más eventos, solicitar la siguiente página
                                        if (jsonArray.length === pageSize) {
                                            // Llama de nuevo para la siguiente página
                                            fetchEvents();
                                        } else {
                                            // Si no hay más eventos, llama al callback con todos los eventos obtenidos
                                            callback(allEvents);
                                        }
                                    },
                                    error: function () {
                                        // Manejo de errores
                                    }
                                });
                            }

                            fetchEvents(); // Inicia la primera carga
                        },

                        dayClick: function (date, jsEvent, view)
                        {
                           
                            var cuerpoTabla = $("#tbodyDetallesLineas");
                            cuerpoTabla.empty();
                         

                            var start = date.toISOString(); // Fecha de inicio del día seleccionado

                            var currentDate = moment().startOf('day'); // Obtener la fecha actual sin la hora

                            var currentDateFormatted = currentDate.format('YYYY-MM-DD'); // Formatear la fecha actual
                            var events = $('#calendarlineas').fullCalendar('clientEvents', function (event) {


                                return event.start.isSame(date, 'day');
                            });


                            if (start < currentDateFormatted) {

                                mostrarAdvertencia("No puede seleccionar un rango de fecha anterior al actual");
                                return; // Salir de la función sin mostrar el modal
                            }

                            if (events.length > 0) {

                                
                                $.ajax({
                                    url: 'VBS_BAN_Calendario_Edit.aspx/ConsultarTablaLineasPorDia',
                                    type: 'POST',
                                    data: JSON.stringify({ start: start }),
                                    contentType: 'application/json; charset=utf-8',
                                    dataType: 'json',
                                    success: function (data) {


                                        var events = [];
                                        var jsonArray = JSON.parse(data.d);
                                        for (var i = 0; i < jsonArray.length; i++) {
                                            var evento = jsonArray[i];
                                            var fila = document.createElement('tr');
                                            fila.innerHTML = `
                                    <td class="center hidden-phone">${secuencia}</td>
                                    <td class="center hidden-phone">${evento.Bodega}</td>
                                    <td class="center hidden-phone">${evento.cantidad}</td>
                                    <td class="center hidden-phone">
                                    <button class="btn btn-primary" onclick="eliminarDetalle(this)" disabled>QUITAR</button>
                                    </td>
                                       `;
                                            // Agregar los datos del detalle como atributos personalizados a la fila
                                            fila.setAttribute('data-tipo-linea-id', evento.IdBodega);

                                            // Agregar la fila al cuerpo de la tabla
                                            document.getElementById('tbodyDetallesLineas').appendChild(fila);

                                            // Incrementar la secuencia para el siguiente detalle
                                            secuencia++;
                                        }
                                        // Agregar los nuevos eventos uno por uno sin eliminar los existentes
                                        for (var j = 0; j < events.length; j++) {
                                            $('#calendarlineas').fullCalendar('renderEvent', events[j], true);
                                        }
                                    },
                                    error: function () {

                                    }
                                });
                            }

                            // Tu código para el evento dayClick...
                            $('#modaltExpoLinea').modal('show');
                            document.getElementById('<%=txtVigenciaInicial.ClientID %>').value = start;
                            document.getElementById('<%=txtVigenciaFinal.ClientID %>').value = start;

                            // Fecha original
                            var fechaOriginalDesde = new Date(start);
                            fechaOriginalDesde.setDate(fechaOriginalDesde.getDate() + 1);
                            // Convertir a la nueva fecha
                            var fechaDesde = fechaOriginalDesde.toLocaleDateString('en-CA');

                            var fechaOriginalHasta = new Date(start);

                            // Convertir a la nueva fecha
                            var fechaHasta = fechaOriginalHasta.toLocaleDateString('en-CA');

                            document.getElementById('<%=txtFechaDesdeLineas.ClientID %>').value = fechaDesde;
                            document.getElementById('<%=txtFechaHastaLineas.ClientID %>').value = fechaDesde;
                            document.getElementById('<%=txtFechaDesdeLineas.ClientID %>').readOnly = true;
                            document.getElementById('<%=txtFechaHastaLineas.ClientID %>').readOnly = true;

                            document.getElementById('<%=banderaDayClickLineas.ClientID %>').value = 1;
                        },

                        eventClick: function (calEvent, jsEvent, view)
                        {
                            //alert("ww");

                            var currentDate = moment().startOf('day'); // Obtener la fecha actual sin la hora
                            var selectedDate = calEvent.start.format(); // Obtener la fecha seleccionada sin la hora

                            var currentDateFormatted = currentDate.format('YYYY-MM-DD'); // Formatear la fecha actual
                            // Formatear la fecha seleccionada

                            if (selectedDate < currentDateFormatted) {

                                mostrarAdvertencia("No puede seleccionar un rango de fecha anterior al actual");
                                return; // Salir de la función sin mostrar el modal
                            }
                            $('#modalActuExpoLineas').modal('show');

                            document.getElementById('<%=TxtFechaPorLinea.ClientID %>').value = calEvent.start.format();
                            document.getElementById('<%=TxtFechaPorLinea.ClientID %>').readOnly = true;

                            IdLinea = calEvent.IdLinea;

                           

                            $.ajax({
                                url: 'VBS_BAN_Calendario_Edit.aspx/ConsultarEventosLineas',
                                type: 'POST',
                                data: JSON.stringify({ IdBodega: calEvent.IdBodega, start: calEvent.start.format(), end: calEvent.end.format() }),
                                contentType: 'application/json; charset=utf-8',
                                dataType: 'json',
                                success: function (data) {
                                    var eventos = JSON.parse(data.d);

                                 
                                    LlenarTablaExpoLineas(eventos);
                                },
                                error: function () {

                                }
                            });
                        },

                        viewRender: function (view, element)
                        {
                            $('.fc-axis.fc-time.fc-widget-content').css('height', 60 + 'px');
                            if (view.name === 'agendaDay') {
                                // Ajustar la altura de las filas de la tabla para mostrar más eventos
                                var rowHeight = 60; // Altura en píxeles de cada fila
                                var eventCount = 4; // Número máximo de eventos que se mostrarán en cada hora

                                // Establecer la altura de las filas de la tabla
                                $('.fc-axis.fc-time.fc-widget-content').css('height', 60 + 'px');


                                var selectedDate = view.start.format('YYYY-MM-DD'); // Obtener la fecha del día en formato deseado
                                // Realizar la consulta específica cuando se cambie a la vista de agenda por día
                                $.ajax({
                                    url: 'VBS_BAN_Calendario_Edit.aspx/ConsultarEventosPorDiaLineas',
                                    type: 'POST',
                                    data: JSON.stringify({ start: selectedDate }),
                                    contentType: 'application/json; charset=utf-8',
                                    dataType: 'json',
                                    success: function (data) {
                                        var events = [];
                                        var jsonArray = JSON.parse(data.d);
                                        for (var i = 0; i < jsonArray.length; i++) {
                                            var evento = jsonArray[i];
                                            var startDateTime = moment(selectedDate + ' ' + evento.horario, 'YYYY-MM-DD HH:mm:ss');
                                            var endDateTime = startDateTime.clone().add(1, 'hour');
                                            var event = {
                                                //  id: evento.idDetalle,
                                                title: evento.title.trim(),
                                                start: startDateTime,
                                                end: endDateTime,
                                                color: evento.color
                                            };
                                            events.push(event);
                                        }
                                        // Agregar los nuevos eventos uno por uno sin eliminar los existentes
                                        for (var j = 0; j < events.length; j++) {
                                            $('#calendarlineas').fullCalendar('renderEvent', events[j], true);
                                        }
                                    },
                                    error: function () {

                                    }
                                });
                            }

                            else if (view.name === 'month') {

                                if (initialLoad)
                                {
                                    initialLoad = false; // Desactivar la bandera de carga inicial
                                }
                                else {
                                    var pageSize = 300;
                                    var pageIndex = 0;
                                    var month = parseInt(view.start.format('MM'));
                                    month = month + 1;
                                    var year = parseInt(view.start.format('YYYY'));
                                    // Remover los eventos existentes
                                    $('#calendarlineas').fullCalendar('removeEvents');
                                    $.ajax({
                                        url: 'VBS_BAN_Calendario_Edit.aspx/GetCalendarEventsExpoLineas',
                                        type: 'POST',
                                        data: JSON.stringify({ year: year, month: month, inicial: initialLoad, pageIndex: pageIndex, pageSize: pageSize }),
                                        contentType: 'application/json; charset=utf-8',
                                        dataType: 'json',
                                        success: function (data) {
                                            var jsonArray = JSON.parse(data.d);
                                            // Agregar los nuevos eventos al FullCalendar
                                            $('#calendarlineas').fullCalendar('addEventSource', jsonArray);
                                        },
                                        error: function () {

                                        }
                                    });
                                   
                                }
                            }

                        },

                        select: function (start, end, jsEvent, view) {
                            var currentDate = moment().startOf('day'); // Obtener la fecha actual sin la hora
                            var selectedDate = moment(start).startOf('day'); // Obtener la fecha seleccionada sin la hora

                            var currentDateFormatted = currentDate.format('YYYY-MM-DD'); // Formatear la fecha actual
                            var selectedDateFormatted = selectedDate.format('YYYY-MM-DD'); // Formatear la fecha seleccionada


                            if (selectedDateFormatted < currentDateFormatted) {
                                mostrarAdvertencia("No puede seleccionar un rango de fecha anterior al actual");
                                return; // Salir de la función sin mostrar el modal
                            }


                            var selectedEvents = $('#calendarlineas').fullCalendar('clientEvents', function (event) {
                                return event.start.isBetween(start, end, 'day', '[]');
                            });

                            if (selectedEvents.length > 0) {
                                // Si hay eventos seleccionados, puedes agregar lógica adicional aquí si es necesario
                            }
                            else {

                                $('#modaltExpoLinea').modal('show');
                                document.getElementById('<%=txtVigenciaInicial.ClientID %>').value = start;
                                document.getElementById('<%=txtVigenciaFinal.ClientID %>').value = end;

                                // Fecha original
                                var fechaOriginalDesde = new Date(start);
                                fechaOriginalDesde.setDate(fechaOriginalDesde.getDate() + 1);
                                // Convertir a la nueva fecha
                                var fechaDesde = fechaOriginalDesde.toLocaleDateString('en-CA');

                                var fechaOriginalHasta = new Date(end);

                                // Convertir a la nueva fecha
                                var fechaHasta = fechaOriginalHasta.toLocaleDateString('en-CA');

                                document.getElementById('<%=txtFechaDesdeLineas.ClientID %>').value = fechaDesde;
                                document.getElementById('<%=txtFechaHastaLineas.ClientID %>').value = fechaHasta;
                                document.getElementById('<%=txtFechaDesdeLineas.ClientID %>').readOnly = true;
                                document.getElementById('<%=txtFechaHastaLineas.ClientID %>').readOnly = true;
                                document.getElementById('<%=banderaDayClickLineas.ClientID %>').value = 0;

                                
                            }

                        },

                        eventMouseover: function (calEvent, jsEvent) {
                            // Obtener el elemento HTML del evento
                            var eventElement = $(this);

                            // Aplicar estilos de resaltado al evento
                            eventElement.addClass('highlight-event');

                            // Obtener el contenido completo del evento
                            var eventTitle = calEvent.title.trim();


                            // Crear un elemento emergente para mostrar el contenido completo
                            var tooltip = $('<div class="event-tooltip"></div>');
                            tooltip.append('<div class="event-title">' + eventTitle + '</div>');


                            // Posicionar y mostrar el elemento emergente
                            $('body').append(tooltip);
                            tooltip.css({
                                top: jsEvent.pageY,
                                left: jsEvent.pageX
                            }).show();
                        },
                        eventMouseout: function () {
                            // Remover los estilos de resaltado del evento
                            $(this).removeClass('highlight-event');

                            // Remover el elemento emergente
                            $('.event-tooltip').remove();
                        },
                    });

                   
                }

            });
        };


        function LlenarTabla(eventos)
        {
            var cuerpoTabla = $("#cuerpoTabla");
            
            $.each(eventos, function (index, evento) {
                var secuencia = index + 1; // Obtener la secuencia sumando 1 al índice

                var fila = $("<tr>");
                fila.attr("data-idTurno", evento.IdTurno);

                fila.append("<td class='center hidden-phone'>" + secuencia + "</td>");
                fila.append("<td class='center hidden-phone'>" + evento.Horario + "</td>");

                // Crear un input editable para la cantidad
                var inputCantidad = $("<input type='text' class='form-control cantidad-input' onkeypress='javascript:return isNumberKey(event);' MaxLength='3' >").val(evento.Cantidad);
                fila.append($("<td class='center hidden-phone'>").append(inputCantidad));

                fila.append("<td class='center hidden-phone lblDisponible'>" + evento.Disponible + "</td>");
                fila.append("<td class='center hidden-phone lblAsignados'>" + evento.Asignados + "</td>");
                <%--document.getElementById('<%=txtTipoCargas.ClientID %>').value = evento.tipoBloque;--%>

                if (evento.TipoCargas === "VACIOS") {
                    tipoCargaVacios = evento.TipoCargas;
                    var accionCelda = $("<td class='center hidden-phone'>");
                    var detalleBoton = $("<button class='btn btn-primary detalleVaciosBtn'>Detalle de vacíos</button>");

                    accionCelda.append(detalleBoton);
                    fila.append(accionCelda);
                }
                else {
                    fila.append("<td class='center hidden-phone'></td>"); // Celda vacía para las filas que no son "VACIOS"
                }

                cuerpoTabla.append(fila);
            });
        }

        $(document).on('click', '.cantidad-editable', function () {
            var input = $(this);
            var fila = input.closest('tr');
            var asignado = parseInt(input.attr("data-asignado"));
      
            $(this).on('keydown', function (event) {
                if (event.which === 13) {
                    event.preventDefault(); // Evitar el comportamiento predeterminado del Enter
                    validarCantidadVacios(input, asignado);
                }
            });

            $(document).on('click', function (event) {
                if (!input.is(event.target)) {
                    validarCantidadVacios(input, asignado);
                }
            });

            function validarCantidadVacios(input, asignado)
            {
               

                var cantidadModificada = parseInt(input.val());
             
                if (cantidadModificada < asignado) {
                    mostrarAdvertencia("La cantidad ingresada no puede ser menor a la cantidad asignada: " + asignado);
                    input.val(asignado); // Restaurar el valor asignado
                }
            }
        });

        $(document).ready(function () {
            $('#modalVacios').on('hidden.bs.modal', function () {
       
                $('#cuerpoTabla').empty();
                $('#modalActu').modal('show');

                LlenarTabla(tablaData)

            });
        });

        // Evento al cerrar el modal de detalle

        function abrirCalendarioPantallaCompleta_DOS() {
            // Abrir una nueva ventana con el calendario
            var nuevaVentana = window.open('VBS_Calendario_Monitor_Expo.aspx', '_blank', 'fullscreen=yes');

            // Cambiar la nueva ventana a pantalla completa
            if (nuevaVentana) {
                if (nuevaVentana.document.documentElement.requestFullscreen) {
                    nuevaVentana.document.documentElement.requestFullscreen();
                }
                else if (nuevaVentana.document.documentElement.mozRequestFullScreen) {
                    nuevaVentana.document.documentElement.mozRequestFullScreen();
                }
                else if (nuevaVentana.document.documentElement.webkitRequestFullscreen) {
                    nuevaVentana.document.documentElement.webkitRequestFullscreen();
                }
                else if (nuevaVentana.document.documentElement.msRequestFullscreen) {
                    nuevaVentana.document.documentElement.msRequestFullscreen();
                }
            }
            nuevaVentana.focus();
        }


        function abrirCalendarioPantallaCompleta_DOS() {
            // Obtener el DropDownList por su ID
            var dropdown = document.getElementById('DropDownList1');

            // Obtener el valor seleccionado en el DropDownList
            var selectedValue = dropdown.value;

            // Validar el valor seleccionado y abrir la página correspondiente
            if (selectedValue === '2') {
                // Si el valor es '2' (Importación), abrir la página VBS_Calendario_Monitor_Import.aspx
                var nuevaVentana = window.open('VBS_Calendario_Monitor_Import.aspx', '_blank', 'fullscreen=yes');
                if (nuevaVentana) {
                    if (nuevaVentana.document.documentElement.requestFullscreen) {
                        nuevaVentana.document.documentElement.requestFullscreen();
                    }
                    else if (nuevaVentana.document.documentElement.mozRequestFullScreen) {
                        nuevaVentana.document.documentElement.mozRequestFullScreen();
                    }
                    else if (nuevaVentana.document.documentElement.webkitRequestFullscreen) {
                        nuevaVentana.document.documentElement.webkitRequestFullscreen();
                    }
                    else if (nuevaVentana.document.documentElement.msRequestFullscreen) {
                        nuevaVentana.document.documentElement.msRequestFullscreen();
                    }
                }
                nuevaVentana.focus();
            }
            else if (selectedValue === '1') {
                // Si el valor es '1' (Exportación), abrir la página VBS_Calendario_Monitor_Expo.aspx
                var nuevaVentana = window.open('VBS_Calendario_Monitor_Expo.aspx', '_blank', 'fullscreen=yes');
                if (nuevaVentana) {
                    if (nuevaVentana.document.documentElement.requestFullscreen) {
                        nuevaVentana.document.documentElement.requestFullscreen();
                    }
                    else if (nuevaVentana.document.documentElement.mozRequestFullScreen) {
                        nuevaVentana.document.documentElement.mozRequestFullScreen();
                    }
                    else if (nuevaVentana.document.documentElement.webkitRequestFullscreen) {
                        nuevaVentana.document.documentElement.webkitRequestFullscreen();
                    }
                    else if (nuevaVentana.document.documentElement.msRequestFullscreen) {
                        nuevaVentana.document.documentElement.msRequestFullscreen();
                    }
                }
                nuevaVentana.focus();
            }
            // Si el valor es '3' (Banano), no hacer nada.
        }



        function reload() {
            location.reload(true);
        }

        function agregarDetalle()
        {
            // Obtener los valores del detalle a través de los controles en la interfaz de usuario
            var cboTipoCarga = document.getElementById("<%= cboTipoCarga.ClientID %>");
            var tipoCargas = cboTipoCarga.options[cboTipoCarga.selectedIndex].text;
            var tipoCargaId = cboTipoCarga.value;


            var tipoContenedor = '';
            var tipoContenedorId = 0;

            var cantidad = document.getElementById('<%=TxtCantidad.ClientID %>').value;
            if (cantidad === '') {
                mostrarError('El campo CANTIDAD es requerido');
                return false;
            }

            // Verificar si ya se ha ingresado un detalle con los mismos valores tipoCargaId
            var detallesExistentes = document.getElementById('tbodyDetalles').getElementsByTagName('tr');
            for (var i = 0; i < detallesExistentes.length; i++) {
                var detalle = detallesExistentes[i];

                var detalleTipoCargaId = detalle.getAttribute('data-tipo-carga-id');
                if (detalleTipoCargaId === tipoCargaId) {
                    mostrarAdvertencia('No se pueden ingresar registros repetidos');
                    return false;
                }
            }

            // Crear una nueva fila con los valores del detalle
            var fila = document.createElement('tr');
            fila.innerHTML = `
                        <td class="center hidden-phone">${secuencia}</td>
                        <td class="center hidden-phone">${tipoCargas}</td>
                     
       
                        <td class="center hidden-phone">${cantidad}</td>
                        <td class="center hidden-phone">
                        <button class="btn btn-primary" onclick="eliminarDetalle(this)">QUITAR</button>
                        </td>  `;

            // Agregar los datos del detalle como atributos personalizados a la fila
            fila.setAttribute('data-tipo-carga-id', tipoCargaId);

            // Agregar la fila al cuerpo de la tabla
            document.getElementById('tbodyDetalles').appendChild(fila);

            // Incrementar la secuencia para el siguiente detalle
            secuencia++;

            return false;
        }


        function agregarDetalleImpo()
        {
            // Obtener los valores del detalle a través de los controles en la interfaz de usuario
            var cboTipoBloque = document.getElementById("<%= cboBloque.ClientID %>");
            var tipoCargas = cboTipoBloque.options[cboTipoBloque.selectedIndex].text;
            var tipoBloqueId = cboTipoBloque.value;

            var frecuencia = document.getElementById('<%=txtFrecuenciaImport.ClientID %>').value;

            if (frecuencia === '') {
                mostrarError('El campo FRECUENCIA es requerido');
                return false;
            }

            // Verificar si ya se ha ingresado un detalle con los mismos valores de tipoContenedorId y tipoCargaId
            var detallesExistentes = document.getElementById('tbodyDetallesIMPORT').getElementsByTagName('tr');
            for (var i = 0; i < detallesExistentes.length; i++) {
                var detalle = detallesExistentes[i];

                var detalleTipoBloqueId = detalle.getAttribute('data-tipo-Bloque-id');
                if (detalleTipoBloqueId === tipoBloqueId) {
                    mostrarAdvertencia('No se pueden ingresar registros repetidos');
                    return false;
                }
            }

            // Crear una nueva fila con los valores del detalle
            var fila = document.createElement('tr');
            fila.innerHTML = `
                         <td class="center hidden-phone">${secuencia}</td>
                          <td class="center hidden-phone">${tipoCargas}</td>
                         <td class="center hidden-phone">${frecuencia}</td>
                         <td class="center hidden-phone">
                             <button class="btn btn-primary" onclick="eliminarDetalle(this)">QUITAR</button>
                         </td>
                     `;

            // Agregar los datos del detalle como atributos personalizados a la fila
            //  fila.setAttribute('data-tipo-contenedor-id', tipoContenedorId);
            fila.setAttribute('data-tipo-Bloque-id', tipoBloqueId);

            // Agregar la fila al cuerpo de la tabla
            document.getElementById('tbodyDetallesIMPORT').appendChild(fila);

            // Incrementar la secuencia para el siguiente detalle
            secuencia++;

            return false;
        }

        //exportaciones por lineas navieras
        function agregarDetalleXLineas()
        {
            // Obtener los valores del detalle a través de los controles en la interfaz de usuario
            var cboTipoLineas = document.getElementById("<%= CboLineas.ClientID %>");
            var desLineas = cboTipoLineas.options[cboTipoLineas.selectedIndex].text;
            var LineaId = cboTipoLineas.value;


            var cantidad = document.getElementById('<%=TxtCantidadLineas.ClientID %>').value;
            if (cantidad === '') {
                mostrarError('El campo CANTIDAD es requerido');
                return false;
            }

            // Verificar si ya se ha ingresado un detalle con los mismos valores de tipoContenedorId y tipoCargaId

            var detallesExistentes = document.getElementById('tbodyDetallesLineas').getElementsByTagName('tr');

            for (var i = 0; i < detallesExistentes.length; i++)
            {
                var detalle = detallesExistentes[i];

                var detalleLineaId = detalle.getAttribute('data-tipo-linea-id');

                if (detalleLineaId === LineaId) {
                    mostrarAdvertencia('No se pueden ingresar registros repetidos');
                    return false;
                }
            }

            // Crear una nueva fila con los valores del detalle
            var fila = document.createElement('tr');
            fila.innerHTML = `
                        <td class="center hidden-phone">${secuencia}</td>
                        <td class="center hidden-phone">${desLineas}</td>
                        <td class="center hidden-phone">${cantidad}</td>
                        <td class="center hidden-phone">
                        <button class="btn btn-primary" onclick="eliminarDetalle(this)">QUITAR</button>
                        </td>  `;

            // Agregar los datos del detalle como atributos personalizados a la fila
            fila.setAttribute('data-tipo-linea-id', LineaId);

            // Agregar la fila al cuerpo de la tabla
            document.getElementById('tbodyDetallesLineas').appendChild(fila);
             
            // Incrementar la secuencia para el siguiente detalle
            secuencia++;

            return false;
        }

        //fin

        //exportaciones
        function enviarDatosAlServidor()
        {
            // Obtener los datos de la tabla
            var detalles = [];
            var tabla = document.getElementById('tablaDetalles');
            var filas = tabla.getElementsByTagName('tr');
            var banderaDayClick = document.getElementById('<%=banderaDayClick.ClientID %>').value

            for (var i = 0; i < filas.length; i++) {
                var fila = filas[i];
                var celdas = fila.getElementsByTagName('td');

                // Verificar si se encontraron celdas en la fila actual
                if (celdas.length > 0) {
                    var secuencia = celdas[0].innerText;
                    var tipoCargas = celdas[1].innerText;
                    /*var tipo_Contenedor = celdas[2].innerText;*/
                    var cantidad = celdas[2].innerText;
                   
                    // Obtener los ID de tipoContenedor y tipoCarga de los atributos personalizados de la fila
                    /*var tipoContenedorId = fila.getAttribute('data-tipo-contenedor-id');*/
                    var tipoCargaId = fila.getAttribute('data-tipo-carga-id');

                    var detalle = {
                        secuencia: secuencia,
                        //tipoContenedorId: 0,
                        //tipoContenedor: '',
                        tipoCargaId: tipoCargaId,
                        tipoCargas: tipoCargas,
                        cantidad: cantidad
                       
                    };

                    detalles.push(detalle);
                    // Verificar si el tipo de contenedor es 'Todos'
                    //if (tipo_Contenedor === 'TODOS') {
                    //    tieneTipoContenedorTodos = true;
                    //}
                }
            }

           

            var vigenciaInicial = document.getElementById('<%=txtVigenciaInicial.ClientID %>').value;
            var vigenciaFinal = document.getElementById('<%=txtVigenciaFinal.ClientID %>').value;

            if (detalles.length === 0) {
                mostrarAdvertencia('No hay detalles para enviar');
                return;
            }
            mostrarConfirmacion('¿Está seguro de generar las Citas para Consolidación/Acopio?',
                function () {
                    $('#loader').show();
                    $.ajax({
                        url: "VBS_BAN_Calendario_Edit.aspx/GuardarDatosTabla1",
                        type: 'POST',
                        data: JSON.stringify({
                            vigenciaInicial: vigenciaInicial,
                            vigenciaFinal: vigenciaFinal,
                            banderaDayClick: banderaDayClick,
                            detalles: detalles
                        }), // Enviar los detalles como un objeto en lugar de una cadena JSON
                        contentType: 'application/json; charset=utf-8',
                        dataType: 'json',
                        success: function (data) {
                            mostrarExito('Datos guardados exitosamente', function () {
                                var modal = document.getElementById('modalt');
                                setTimeout(function () {
                                    var closeButton = modal.querySelector('.close'); // Seleccionar el botón de cierre
                                    if (closeButton) {
                                        closeButton.click(); // Simular un clic en el botón de cierre
                                    }
                                    $('.modal-backdrop').remove();
                                    $('body').removeClass('modal-open');
                                }, 1000); // 2000 milisegundos (2 segundos)
                                $('#calendar').fullCalendar('refetchEvents');

                            });
                        },
                        error: function (error) {
                            // Manejar el error de la solicitud AJAX
                            var modal = document.getElementById('modalt');
                            mostrarError('Error al guardar los datos');
                            modal.style.display = 'none';
                            $('.modal-backdrop').remove();
                            $('body').removeClass('modal-open');
                            $('#calendar').fullCalendar('refetchEvents');

                        },

                        complete: function () {
                            // Ocultar el indicador de carga al finalizar la solicitud AJAX
                            $('#loader').hide();
                        }
                    });
                })
        }


        //importaciones
        function enviarDatosAlServidorImpor() {
            // Obtener los datos de la tabla
            var detalles = [];
            var tabla = document.getElementById('tablaDetallesIMPORT');
            var filas = tabla.getElementsByTagName('tr');

            var banderaDayClick = document.getElementById('<%=banderaDayClick.ClientID %>').value
            for (var i = 0; i < filas.length; i++) {
                var fila = filas[i];
                var celdas = fila.getElementsByTagName('td');

                // Verificar si se encontraron celdas en la fila actual
                if (celdas.length > 0) {
                    var secuencia = celdas[0].innerText;
                    var bloque = celdas[1].innerText;
                    var frecuencia = celdas[2].innerText;


                    // Obtener los ID de tipoContenedor y tipoCarga de los atributos personalizados de la fila
                    var tipoCargaId = fila.getAttribute('data-tipo-Bloque-id');

                    var detalle = {
                        secuencia: secuencia,
                        bloqueId: tipoCargaId,
                        tipoBloque: bloque,
                        frecuencia: frecuencia,
                    };

                    detalles.push(detalle);
                    // Verificar si el tipo de contenedor es 'Todos'

                }
            }



            var vigenciaInicial = document.getElementById('<%=txtVigenciaInicial.ClientID %>').value;
            var vigenciaFinal = document.getElementById('<%=txtVigenciaFinal.ClientID %>').value;

           

            if (detalles.length === 0) {
                mostrarAdvertencia('No hay detalles para enviar');
                return;
            }
            mostrarConfirmacion('¿Está seguro de generar los Citas para Consolidación CFS?',
                function () {
                    $('#loader').show();
                    $.ajax({
                        url: "VBS_BAN_Calendario_Edit.aspx/GuardarDatosTablaImport",
                        type: 'POST',
                        data: JSON.stringify({
                            vigenciaInicial: vigenciaInicial,
                            vigenciaFinal: vigenciaFinal,
                            banderaDayClick: banderaDayClick,
                            detalles: detalles
                        }), // Enviar los detalles como un objeto en lugar de una cadena JSON
                        contentType: 'application/json; charset=utf-8',
                        dataType: 'json',
                        success: function (data) {
                            mostrarExito('Datos guardados exitosamente', function () {
                                var modal = document.getElementById('modaltImport');

                                setTimeout(function () {
                                    var closeButton = modal.querySelector('.close'); // Seleccionar el botón de cierre
                                    if (closeButton) {
                                        closeButton.click(); // Simular un clic en el botón de cierre
                                    }
                                    $('.modal-backdrop').remove();
                                    $('body').removeClass('modal-open');
                                }, 1000); // 2000 milisegundos (2 segundos)

                                $('#calendarImpo').fullCalendar('refetchEvents');
                            });
                        },
                        error: function (error) {
                            // Manejar el error de la solicitud AJAX
                            var modal = document.getElementById('modaltImport');
                            mostrarError('Error al guardar los datos');
                            modal.style.display = 'none';
                            location.reload();

                        },

                        complete: function () {
                            // Ocultar el indicador de carga al finalizar la solicitud AJAX
                            $('#loader').hide();
                        }
                    });
                })

        }



        //exportaciones por linea naviera
        function enviarDatosAlServidorLineas()
        {
            // Obtener los datos de la tabla
            var detalles = [];
            var tabla = document.getElementById('tablaDetallesLineas');
            var filas = tabla.getElementsByTagName('tr');

            

            var banderaDayClick = document.getElementById('<%=banderaDayClickLineas.ClientID %>').value

            for (var i = 0; i < filas.length; i++)
            {
                var fila = filas[i];
                var celdas = fila.getElementsByTagName('td');

                // Verificar si se encontraron celdas en la fila actual
                if (celdas.length > 0) {
                    var secuencia = celdas[0].innerText;
                    var descLinea = celdas[1].innerText;
                    var cantidad = celdas[2].innerText;

                    // Obtener los ID de lineas de los atributos personalizados de la fila
                    var tipoLineaId = fila.getAttribute('data-tipo-linea-id');

                    var detalle = {
                        secuencia: secuencia,
                        tipoLineaId: tipoLineaId,
                        descLinea: descLinea,
                        cantidad: cantidad
                    };

                    detalles.push(detalle);

                }
            }

           
            var vigenciaInicial = document.getElementById('<%=txtVigenciaInicial.ClientID %>').value;
            var vigenciaFinal = document.getElementById('<%=txtVigenciaFinal.ClientID %>').value;

      
          
            if (detalles.length === 0)
            {
                mostrarAdvertencia('No hay detalles para enviar');
                return;
            }

            mostrarConfirmacion('¿Está seguro de generar las Citas para Carga Expo. BRBK.?',
                function () {
                    $('#loader').show();
                    $.ajax({
                        url: "VBS_BAN_Calendario_Edit.aspx/GuardarDatosTablaLineas",
                        type: 'POST',
                        data: JSON.stringify({
                            vigenciaInicial: vigenciaInicial,
                            vigenciaFinal: vigenciaFinal,
                            banderaDayClick: banderaDayClick,
                            detalles: detalles
                        }), // Enviar los detalles como un objeto en lugar de una cadena JSON
                        contentType: 'application/json; charset=utf-8',
                        dataType: 'json',
                        success: function (data) {
                            mostrarExito('Datos guardados exitosamente', function () {
                                var modal = document.getElementById('modaltExpoLinea');
                                setTimeout(function () {
                                    var closeButton = modal.querySelector('.close'); // Seleccionar el botón de cierre
                                    if (closeButton) {
                                        closeButton.click(); // Simular un clic en el botón de cierre
                                    }
                                    $('.modal-backdrop').remove();
                                    $('body').removeClass('modal-open');
                                }, 1000); // 2000 milisegundos (2 segundos)
                                $('#calendarlineas').fullCalendar('refetchEvents');

                            });
                        },
                        error: function (error) {
                            // Manejar el error de la solicitud AJAX
                            var modal = document.getElementById('modaltExpoLinea');
                            mostrarError('Error al guardar los datos');
                            modal.style.display = 'none';
                            $('.modal-backdrop').remove();
                            $('body').removeClass('modal-open');
                            $('#calendarlineas').fullCalendar('refetchEvents');

                        },

                        complete: function () {
                            // Ocultar el indicador de carga al finalizar la solicitud AJAX
                            $('#loader').hide();
                        }
                    });
                })

        }
        //fin



        function LlenarTablaImport(eventos)
        {
            var cuerpoTabla = $("#cuerpoTablaImport");
            cuerpoTabla.empty();

        //    $('#tbodyDetallesIMPORT').remove(rows);

            // Iterar sobre los eventos y crear las filas de la tabla
            $.each(eventos, function (index, evento) {
                var secuencia = index + 1; // Obtener la secuencia sumando 1 al índice


                var fila = $("<tr>");
                fila.attr("data-idTurno", evento.IdTurno);
                document.getElementById('<%=txtTipoBloque.ClientID %>').value = evento.CodigoBloque;

                fila.append("<td class='center hidden-phone'>" + secuencia + "</td>");
                fila.append("<td class='center hidden-phone'>" + evento.Horario + "</td>");

                // Crear un input editable para la cantidad
                var inputCantidad = $("<input type='text' id='cantidad' class='form-control cantidad-input'  onkeypress='javascript:return isNumberKey(event);' MaxLength='3'  >").val(evento.Cantidad);
               


                fila.append($("<td class='center hidden-phone'>").append(inputCantidad));

               

                fila.append("<td class='center hidden-phone lblDisponible'>" + evento.Disponible + "</td>");
                fila.append("<td class='center hidden-phone lblAsignados'>" + evento.Asignados + "</td>");


                cuerpoTabla.append(fila);
            });

        }

        function LlenarTablaExpoLineas(eventos)
        {
            var cuerpoTabla = $("#cuerpoTablaExpoLineas");
            cuerpoTabla.empty();

   
            // Iterar sobre los eventos y crear las filas de la tabla
            $.each(eventos, function (index, evento)
            {
                var secuencia = index + 1; // Obtener la secuencia sumando 1 al índice


                var fila = $("<tr>");
                fila.attr("data-idTurno", evento.IdTurno);

                //alert(evento.IdTurno);

                document.getElementById('<%=TxtLineaDefault.ClientID %>').value = evento.Bodega;

                fila.append("<td class='center hidden-phone'>" + secuencia + "</td>");
                fila.append("<td class='center hidden-phone'>" + evento.Horario + "</td>");

                // Crear un input editable para la cantidad
                var inputCantidad = $("<input type='text' id='cantidad' class='form-control cantidad-input'  onkeypress='javascript:return isNumberKey(event);' MaxLength='3'  >").val(evento.Cantidad);
                fila.append($("<td class='center hidden-phone'>").append(inputCantidad));

               

                fila.append("<td class='center hidden-phone lblDisponible'>" + evento.Disponible + "</td>");
                fila.append("<td class='center hidden-phone lblAsignados'>" + evento.Asignados + "</td>");

                fila.append("<td class='center hidden-phone'></td>");

                cuerpoTabla.append(fila);
            });

        }

      
         function isNumberKey(evt) {
        var charCode = (evt.which) ? evt.which : event.keyCode;
        if (charCode > 31 && (charCode < 48 || charCode > 57)) {
            return false;
        }
        return true;
        }


        function guardarCambiosTabla() {
            var eventos = [];

            // Obtener los datos de la tabla
            $("#cuerpoTabla tr").each(function () {
                var fila = $(this);
                var idTurno = fila.attr("data-idTurno");
                var cantidad = fila.find(".cantidad-input").val().trim();
                var disponible = fila.find(".lblDisponible").text().trim();
                var secuencia = fila.find("td:nth-child(1)").text().trim();

                // Crear un objeto con los datos del evento
                var evento = {
                    secuencia: secuencia,
                    cantidad: cantidad,
                    idTurno: idTurno,
                    disponible: disponible
                };

                // Agregar el evento al arreglo
                eventos.push(evento);
            });

            mostrarConfirmacion("¿Deseas guardar los cambios en la tabla?", function () {
                $.ajax({
                    type: "POST",
                    url: "VBS_BAN_Calendario_Edit.aspx/GuardarDatos",
                    data: JSON.stringify({ datosTabla: eventos }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {

                        if (response.d === "success") {

                            mostrarExito("Cambios guardados correctamente.", function () {
                                $("#modalActu").modal("hide");
                                var modal = document.getElementById('modalActu');
                                setTimeout(function () {
                                    var closeButton = modal.querySelector('.close'); // Seleccionar el botón de cierre
                                    if (closeButton) {
                                        closeButton.click(); // Simular un clic en el botón de cierre
                                    }
                                    $('.modal-backdrop').remove();
                                    $('body').removeClass('modal-open');
                                }, 1000); // 2000 milisegundos (2 segundos)
                                $('#calendar').fullCalendar('refetchEvents');

                            })
                        }
                        else {
                            mostrarError("Error al guardar los cambios.")

                        }
                    },
                    error: function (xhr, ajaxOptions, thrownError) {
                        mostrarError("Error en la solicitud al servidor.");
                    }
                });
            });

        }


        function guardarCambiosTablaImport() {
            var eventos = [];

            // Obtener los datos de la tabla
            $("#cuerpoTablaImport tr").each(function () {
                var fila = $(this);
                var idTurno = fila.attr("data-idTurno");
                var cantidad = fila.find(".cantidad-input").val().trim();
                var disponible = fila.find(".lblDisponible").text().trim();
                var secuencia = fila.find("td:nth-child(1)").text().trim();

                // Crear un objeto con los datos del evento
                var evento = {
                    secuencia: secuencia,
                    cantidad: cantidad,
                    idTurno: idTurno,
                    disponible: disponible
                };

                // Agregar el evento al arreglo
                eventos.push(evento);
            });

            mostrarConfirmacion("¿Deseas guardar los cambios en la tabla?", function () {
                $.ajax({
                    type: "POST",
                    url: "VBS_BAN_Calendario_Edit.aspx/GuardarDatosImport",
                    data: JSON.stringify({ datosTabla: eventos }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {

                        if (response.d === "success") {

                            mostrarExito("Cambios guardados correctamente.", function () {
                                $("#modalActuImport").modal("hide");
                                var modal = document.getElementById('modalActuImport');
                                setTimeout(function () {
                                    var closeButton = modal.querySelector('.close'); // Seleccionar el botón de cierre
                                    if (closeButton) {
                                        closeButton.click(); // Simular un clic en el botón de cierre
                                    }
                                    $('.modal-backdrop').remove();
                                    $('body').removeClass('modal-open');
                                }, 1000); // 2000 milisegundos (2 segundos)
                                $('#calendarImpo').fullCalendar('refetchEvents');

                            })
                        }
                        else {
                            mostrarError("Error al guardar los cambios.")

                        }
                    },
                    error: function (xhr, ajaxOptions, thrownError) {
                        mostrarError("Error en la solicitud al servidor.");
                    }
                });
            });

        }


        function guardarCambiosTablaVacios() {
            var eventos = [];

            // Obtener los datos de la tabla
            $("#cuerpoTablaVacios tr").each(function () {
                var fila = $(this);
                var idTurno = fila.data("idturno"); // Utiliza data() para acceder al atributo data-idturno
                var cantidad = fila.find(".cantidad-editable").val().trim();
                var descripcion = fila.find("td:first-child").text().trim();

                // Crear un objeto con los datos del evento
                var evento = {
                    idTurno: idTurno,
                    cantidad: cantidad,
                    //descripcion: descripcion
                };

                // Agregar el evento al arreglo
                eventos.push(evento);
            });

            mostrarConfirmacion("¿Deseas guardar los cambios en la tabla?", function () {
                $.ajax({
                    type: "POST",
                    url: "VBS_BAN_Calendario_Edit.aspx/GuardarDatosVacios",
                    data: JSON.stringify({ datosTabla: eventos }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {

                        if (response.d === "success") {

                            mostrarExito("Cambios guardados correctamente.", function () {
                                $("#modalVacios").modal("hide");
                                $('#cuerpoTabla').empty();
                                $('#modalActu').modal('show');

                                $.ajax({
                                    url: 'VBS_BAN_Calendario_Edit.aspx/ConsultarEventos',
                                    type: 'POST',
                                    data: JSON.stringify({ idDetalle: idDetalle, start: fechaIni, end: fechaFin }),
                                    contentType: 'application/json; charset=utf-8',
                                    dataType: 'json',
                                    success: function (data) {
                                        var eventos = JSON.parse(data.d);
                                        tablaData = eventos;
                                        $('#cuerpoTabla').empty();
                                        // Llenar la tabla con los datos de eventos y pasar los parámetros de paginación
                                        LlenarTabla(eventos);
                                    },
                                    error: function () {

                                    }
                                });


                            })
                        }
                        else {
                            mostrarError("Error al guardar los cambios.")

                        }
                    },
                    error: function (xhr, ajaxOptions, thrownError) {
                        mostrarError("Error en la solicitud al servidor.");
                    }
                });
            });

        }


        function guardarCambiosTablaLineas() {
            var eventos = [];

            // Obtener los datos de la tabla
            $("#cuerpoTablaExpoLineas tr").each(function ()
            {
                var fila = $(this);
                var idTurno = fila.attr("data-idTurno");
                var cantidad = fila.find(".cantidad-input").val().trim();
                var disponible = fila.find(".lblDisponible").text().trim();
                var secuencia = fila.find("td:nth-child(1)").text().trim();

                // Crear un objeto con los datos del evento
                var evento = {
                    secuencia: secuencia,
                    cantidad: cantidad,
                    idTurno: idTurno,
                    disponible: disponible
                };

                // Agregar el evento al arreglo
                eventos.push(evento);
            });

            mostrarConfirmacion("¿Deseas guardar los cambios en la tabla?", function ()
            {
                $.ajax({
                    type: "POST",
                    url: "VBS_BAN_Calendario_Edit.aspx/GuardarDatosPorlineas",
                    data: JSON.stringify({ datosTabla: eventos }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {

                        if (response.d === "success") {

                            mostrarExito("Cambios guardados correctamente.", function ()
                            {
                                $("#modalActuExpoLineas").modal("hide");
                                var modal = document.getElementById('modalActuExpoLineas');
                                setTimeout(function () {
                                    var closeButton = modal.querySelector('.close'); // Seleccionar el botón de cierre
                                    if (closeButton) {
                                        closeButton.click(); // Simular un clic en el botón de cierre
                                    }
                                    $('.modal-backdrop').remove();
                                    $('body').removeClass('modal-open');
                                }, 1000); // 2000 milisegundos (2 segundos)
                                $('#calendarlineas').fullCalendar('refetchEvents');

                            })
                        }
                        else {
                            mostrarError("Error al guardar los cambios.")

                        }
                    },
                    error: function (xhr, ajaxOptions, thrownError) {
                        mostrarError("Error en la solicitud al servidor.");
                    }
                });
            });

        }


        function filtrarTabla() {
            // Obtener el valor de búsqueda del input de búsqueda
            var filtro = document.getElementById('inputBusqueda').value.toUpperCase();
            // Obtener la tabla y las filas de la tabla
            var tabla = document.getElementById('cuerpoTabla');

            var filas = tabla.getElementsByTagName('tr');

            // Recorrer las filas de la tabla y mostrar u ocultar según el filtro
            for (var i = 0; i < filas.length; i++) {
                var celdas = filas[i].getElementsByTagName('td');

                var mostrarFila = false;

                for (var j = 0; j < celdas.length; j++) {
                    var contenidoCelda = celdas[j].textContent || celdas[j].innerText;

                    if (contenidoCelda.toUpperCase().indexOf(filtro) > -1) {
                        mostrarFila = true;
                        break;
                    }
                }

                filas[i].style.display = mostrarFila ? '' : 'none';
            }
        }


        function filtrarTablaImport() {
            // Obtener el valor de búsqueda del input de búsqueda
            var filtro = document.getElementById('inputBusquedaImport').value.toUpperCase();
            // Obtener la tabla y las filas de la tabla
            var tabla = document.getElementById('cuerpoTablaImport');

            var filas = tabla.getElementsByTagName('tr');

            // Recorrer las filas de la tabla y mostrar u ocultar según el filtro
            for (var i = 0; i < filas.length; i++) {
                var celdas = filas[i].getElementsByTagName('td');

                var mostrarFila = false;

                for (var j = 0; j < celdas.length; j++) {
                    var contenidoCelda = celdas[j].textContent || celdas[j].innerText;

                    if (contenidoCelda.toUpperCase().indexOf(filtro) > -1) {
                        mostrarFila = true;
                        break;
                    }
                }

                filas[i].style.display = mostrarFila ? '' : 'none';
            }
        }


        function filtrarTablaLineas()
        {
            // Obtener el valor de búsqueda del input de búsqueda
            var filtro = document.getElementById('inputBusquedaLineas').value.toUpperCase();
            // Obtener la tabla y las filas de la tabla
            var tabla = document.getElementById('cuerpoTablaExpoLineas');

            var filas = tabla.getElementsByTagName('tr');

            // Recorrer las filas de la tabla y mostrar u ocultar según el filtro
            for (var i = 0; i < filas.length; i++) {
                var celdas = filas[i].getElementsByTagName('td');

                var mostrarFila = false;

                for (var j = 0; j < celdas.length; j++) {
                    var contenidoCelda = celdas[j].textContent || celdas[j].innerText;

                    if (contenidoCelda.toUpperCase().indexOf(filtro) > -1) {
                        mostrarFila = true;
                        break;
                    }
                }

                filas[i].style.display = mostrarFila ? '' : 'none';
            }
        }



        function eliminarDetalle(button) {
            // Obtener la fila padre del botón
            var fila = button.parentNode.parentNode;

            // Eliminar la fila de la tabla
            fila.remove();
        }

    </script>


</asp:Content>



<asp:Content ID="Content2" ContentPlaceHolderID="placebody" runat="server">
    <asp:ToolkitScriptManager ID="tkscata" runat="server"></asp:ToolkitScriptManager>
     <asp:HiddenField ID="hf_mes" runat="server" />

    <div class="mt-4">         
        <nav class="mt-4" aria-label="breadcrumb">
            <ol class="breadcrumb">
                <li class="breadcrumb-item" id="opcion_principal" runat="server"><a href="#">VBS </a></li>
                <li class="breadcrumb-item active" aria-current="page" id="Li1" runat="server">CALENDARIO DE CITAS</li>
            </ol>
        </nav>
    </div>

    <div class="dashboard-container p-4">

        <div class="form-title">
            TIPO DE CALENDARIO DE CITAS
        </div>

       <%-- <div class="row" style="visibility: hidden;">
            <div class="col-md-3">
                <div class="card p-1 ">
                    <button id="btnFullScreen" onclick="abrirCalendarioPantallaCompleta_DOS()" style="border: none; background: none; display: flex; align-items: center;">
                        <i class="fas fa-desktop" style="color: white; font-size: 50px; padding:2px; background-color: #2BADC6;  border-radius: 5px;"></i> 
                        <span class="breadcrumb-item" style="flex: 1; display: flex; justify-content: center;">Monitor</span>
                    </button>
                </div> 
            </div>
        </div>--%>

        <%--<div class="mt-4">
            <nav class="mt-4" aria-label="breadcrumb">
                <ol class="breadcrumb">--%>
 
                    <%--<li style="text-align:center" class="breadcrumb-item active" aria-current="page" id="sub_opcion" runat="server">CALENDARIO DE CITAS</li>--%>
         
                    <li style="text-align:center; width:300px" class="breadcrumb-item active" aria-current="page" id="LiDropDownList1" runat="server">
                        <asp:DropDownList  runat="server" ID="DropDownList1" AutoPostBack="false" class="form-control" Font-Bold="true" ClientIDMode="Static">
                             <asp:ListItem Text="Consolidación/Acopio" Value="1"></asp:ListItem>
                             <asp:ListItem Text="Consolidación CFS" Value="2"></asp:ListItem>
                             <asp:ListItem Text="Carga Expo BBK" Value="3"></asp:ListItem>
                        </asp:DropDownList>
                    </li>
              <%--   </ol>
           </nav>
        </div>--%>

        <br />

        <div id="div_BrowserWindowName" style="visibility: hidden; height: 1px;">
            <asp:HiddenField ID="hf_BrowserWindowName" runat="server" />
            <asp:HiddenField ID="hf_BrowserWindowName2" runat="server" />
            <asp:TextBox ID="txtVigenciaInicial" runat="server" class="form-control" Style="text-align: left"></asp:TextBox>
            <asp:TextBox ID="txtVigenciaFinal" runat="server" class="form-control" Style="text-align: left"></asp:TextBox>
        </div>

       

        

        <div id="calendar"></div>
        <div id="calendarImpo"></div>
        <div id="calendarlineas"></div>

        <!-- Modal para mostrar Actualizar turnos -->
        <!-- consolidación/Acopio-->
        <div class="modal fade" id="modalActu" tabindex="-1" role="dialog" aria-labelledby="modal-event-label" aria-hidden="true" data-backdrop="static" data-keyboard="false">
            <div class="modal-dialog modal-xl" role="document">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title" id="modal-Actu-label1">Actualización de Citas Consolidación/Acopio</h5>
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span>
                        </button>
                    </div>
                    <div class="modal-body">

                        <div class="form-row">
                            <div class="form-group col-md-3">
                                     <label for="inputZip">Filtrar Tabla</label>
                                <input type="text" id="inputBusqueda" style="align-content: flex-start" class="form-control" onkeyup="filtrarTabla()" placeholder="Buscar..." />

                            </div>
                      
                              <div class="form-group col-md-3">
                                                              <label for="inputZip">FECHA</label>
                                <asp:TextBox ID="txtFechaActu" runat="server" class="form-control" MaxLength="2" onkeypress="return soloLetras(event,'0123456789')" Style="text-align: left"
                                    placeholder="">
                                </asp:TextBox>
                            </div>
                               <div class="form-group col-md-3">
                                                              <label for="inputZip">TIPO DE CARGAS</label>
                                <asp:TextBox ID="txtTipoCargas" runat="server" class="form-control" MaxLength="2" onkeypress="return soloLetras(event,'0123456789')" Style="text-align: left"
                                    placeholder="">
                                </asp:TextBox>
                            </div>
                               

                            <br />
                        </div>


                        <div class="form-row">
                            <div class="form-group col-md-12">
                                <asp:UpdatePanel ID="UPDETALLE" runat="server">
                                    <ContentTemplate>
                                        <table cellpadding="0" cellspacing="0" border="0" class="table table-bordered invoice" id="hidden-table-info2">
                                            <thead>
                                                <tr>
                                                    <th class="center hidden-phone">#</th>
                                                    <th class="center hidden-phone">HORARIO</th>
                                                    <th class="center hidden-phone">CAPACIDAD</th>
                                                    <th class="center hidden-phone">DISPONIBLE </th>
                                                    <th class="center hidden-phone">ASIGNADOS </th>
                                                    <th class="center hidden-phone">ACCION </th>
                                                </tr>
                                            </thead>
                                            <tbody id="cuerpoTabla">
                                                <asp:Repeater ID="tablePagination" runat="server">
                                                    <ItemTemplate>
                                           
                                                    </ItemTemplate>
                                                </asp:Repeater>
                                            </tbody>
                                        </table>


                                    </ContentTemplate>

                                </asp:UpdatePanel>
                            </div>
                        </div>

                        <button class="btn btn-primary" id="btnGuardar" <%--onclick="enviarDatosAlServidor()"--%> type="button">Guardar</button>

              
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-secondary" data-dismiss="modal">Cerrar</button>
                    </div>
                </div>
            </div>
        </div>

        <!-- consolidación CFS-->
        <div class="modal fade" id="modalActuImport" tabindex="-1" role="dialog" aria-labelledby="modal-event-label" aria-hidden="true" data-backdrop="static" data-keyboard="false">
            <div class="modal-dialog modal-xl" role="document">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title" id="modal-Actu-label-Import">Actualización de Citas creadas consolidación CFS.</h5>
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span>
                        </button>
                    </div>
                    <div class="modal-body">

                        <div class="form-row">
                            <div class="form-group col-md-3">
                                     <label for="inputZip">Filtrar Tabla</label>
                                <input type="text" id="inputBusquedaImport" style="align-content: flex-start" class="form-control" onkeyup="filtrarTablaImport()" placeholder="Buscar..." />

                            </div>
                      
                              <div class="form-group col-md-3">
                                                              <label for="inputZip">FECHA</label>
                                <asp:TextBox ID="txtFechaImport" runat="server" class="form-control" MaxLength="2" onkeypress="return soloLetras(event,'0123456789')" Style="text-align: left"
                                    placeholder="">
                                </asp:TextBox>
                            </div>
                               <div class="form-group col-md-3">
                                                              <label for="inputZip">TIPO DE BLOQUE</label>
                                <asp:TextBox ID="txtTipoBloque" runat="server" class="form-control" MaxLength="2" onkeypress="return soloLetras(event,'0123456789')" Style="text-align: left"
                                    placeholder="" ReadOnly="true">
                                </asp:TextBox>
                            </div>
                          

                            <br />
                        </div>


                        <div class="form-row">
                            <div class="form-group col-md-12">
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                    <ContentTemplate>
                                        <table cellpadding="0" cellspacing="0" border="0" class="table table-bordered invoice" id="hidden-table-info1">
                                            <thead>
                                                <tr>
                                                    <th class="center hidden-phone">#</th>
                                                    <th class="center hidden-phone">HORARIO</th>
                                                    <th class="center hidden-phone">CAPACIDAD</th>
                                                    <th class="center hidden-phone">DISPONIBLE </th>
                                                    <th class="center hidden-phone">ASIGNADOS </th>
                                             
                                                </tr>
                                            </thead>
                                            <tbody id="cuerpoTablaImport">
                                                <asp:Repeater ID="Repeater1" runat="server">
                                                    <ItemTemplate>
                                           
                                                    </ItemTemplate>
                                                </asp:Repeater>
                                            </tbody>
                                        </table>


                                    </ContentTemplate>

                                </asp:UpdatePanel>
                            </div>
                        </div>

                        <button class="btn btn-primary" id="btnGuardarImport" <%--onclick="enviarDatosAlServidor()"--%> type="button">Guardar</button>

              
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-secondary" data-dismiss="modal">Cerrar</button>
                    </div>
                </div>
            </div>
        </div>

         <!-- carga expo BRBK-->
        <div class="modal fade" id="modalActuExpoLineas" tabindex="-1" role="dialog" aria-labelledby="modal-event-label" aria-hidden="true" data-backdrop="static" data-keyboard="false">
            <div class="modal-dialog modal-xl" role="document">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title" id="modal-Actu-label">Actualización de Citas creadas Carga Expo. BRBK.</h5>
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span>
                        </button>
                    </div>
                    <div class="modal-body">

                        <div class="form-row">
                            <div class="form-group col-md-3">
                                     <label for="inputZip">Filtrar Tabla</label>
                                <input type="text" id="inputBusquedaLineas" style="align-content: flex-start" class="form-control" onkeyup="filtrarTablaLineas()" placeholder="Buscar..." />

                            </div>
                      
                              <div class="form-group col-md-3">
                                                              <label for="inputZip">FECHA</label>
                                <asp:TextBox ID="TxtFechaPorLinea" runat="server" class="form-control" MaxLength="2" onkeypress="return soloLetras(event,'0123456789')" Style="text-align: left"
                                    placeholder="">
                                </asp:TextBox>
                            </div>
                               <div class="form-group col-md-3">
                                                              <label for="inputZip">LINEA</label>
                                <asp:TextBox ID="TxtLineaDefault" runat="server" class="form-control" MaxLength="2" onkeypress="return soloLetras(event,'0123456789')" Style="text-align: left"
                                    placeholder="">
                                </asp:TextBox>
                            </div>
                          

                            <br />
                        </div>


                        <div class="form-row">
                            <div class="form-group col-md-12">
                                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                    <ContentTemplate>
                                        <table cellpadding="0" cellspacing="0" border="0" class="table table-bordered invoice" id="hidden-table-info3">
                                            <thead>
                                                <tr>
                                                    <th class="center hidden-phone">#</th>
                                                    <th class="center hidden-phone">HORARIO</th>
                                                    <th class="center hidden-phone">CAPACIDAD</th>
                                                    <th class="center hidden-phone">DISPONIBLE </th>
                                                    <th class="center hidden-phone">ASIGNADOS </th>
                                                    <th class="center hidden-phone">ACCION </th>
                                                </tr>
                                            </thead>
                                            <tbody id="cuerpoTablaExpoLineas">
                                                <asp:Repeater ID="Repeater2" runat="server">
                                                    <ItemTemplate>
                                           
                                                    </ItemTemplate>
                                                </asp:Repeater>
                                            </tbody>
                                        </table>


                                    </ContentTemplate>

                                </asp:UpdatePanel>
                            </div>
                        </div>

                        <button class="btn btn-primary" id="btnGuardarLineas" <%--onclick="enviarDatosAlServidor()"--%> type="button">Guardar</button>

              
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-secondary" data-dismiss="modal">Cerrar</button>
                    </div>
                </div>
            </div>
        </div>


        <!-- Modal para mostrar crear turnos -->
        <!-- consolidación/Acopio-->
        <div class="modal fade" id="modalt" tabindex="-1" role="dialog" aria-labelledby="modal-event-label" aria-hidden="true" data-backdrop="static" data-keyboard="false">
            <div class="modal-dialog modal-lg" role="document">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title" id="modal-event-label1">Creación de Citas Consolidación/Acopio</h5>
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span>
                        </button>
                    </div>
                    <div class="modal-body">
                        <div class="row">
                            <div class="form-group col-md-4">
                                <label for="inputZip">FECHA DESDE</label>
                                <asp:TextBox ID="txtFechaDesde" runat="server" class="form-control" MaxLength="2" onkeypress="return soloLetras(event,'0123456789')" Style="text-align: left" placeholder=""></asp:TextBox>
                            </div> 
                            <div class="form-group col-md-4">
                                <label for="inputZip">FECHA HASTA</label>
                                <asp:TextBox ID="txtFechaHasta" runat="server" class="form-control" MaxLength="2" onkeypress="return soloLetras(event,'0123456789')" Style="text-align: left" placeholder=""></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-row">
                            <div class="form-group col-md-4">
                                <label for="inputZip">UBICACIÓN</label>
                                <asp:DropDownList runat="server" ID="cboTipoCarga" AutoPostBack="false" class="form-control" ClientIDMode="Static"></asp:DropDownList>
                            </div>
                        
                            <div class="form-group col-md-2">
                                <label for="inputZip">CAPACIDAD</label>
                                <asp:TextBox ID="TxtCantidad" runat="server" class="form-control" MaxLength="2"  onkeypress="return soloLetras(event,'1234567890')"  Style="text-align: left" placeholder=""></asp:TextBox>
                            </div>
                            <div class="form-group col-md-2">
                                <label for="inputZip">&nbsp;</label>
                                <div class="d-flex">
                                    <button class="btn btn-primary" onclick="agregarDetalle()" type="button">AGREGAR</button>
                                    &nbsp;&nbsp;
                                    <img alt="loading.." src="../lib/file-uploader/img/loading.gif" height="32px" width="32px" id="ImgCarga" class="nover" />
                                </div>
                            </div>
                        </div>
              
                        <div class="form-row">
                            <div class="form-group col-md-12">
                                <table id="tablaDetalles" class="table table-bordered invoice">
                                    <!-- Cabecera de la tabla -->
                                    <thead>
                                        <tr>
                                            <th class="center hidden-phone">#</th>
                                            <th class="center hidden-phone">UBICACIÓN</th>
                                            <th class="center hidden-phone">CAPACIDAD</th>
                                            <th class="center hidden-phone">QUITAR</th>
                                        </tr>
                                    </thead>
                                    <!-- Cuerpo de la tabla -->
                                    <tbody id="tbodyDetalles">
                                    </tbody>
                                </table>
                            </div>
                        </div>
                       
                        <button class="btn btn-primary" onclick="enviarDatosAlServidor()" type="button">GENERAR CITAS</button>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-secondary" data-dismiss="modal">Cerrar</button>
                    </div>
   
                            <asp:TextBox type="hidden"  ID="banderaDayClick" runat="server" class="form-control" MaxLength="2" onkeypress="return soloLetras(event,'0123456789')" Style="text-align: left" placeholder=""></asp:TextBox>
                   
    
                </div>
            </div>
        </div>

        <!-- consolidación CFS-->
        <div class="modal fade" id="modaltImport" tabindex="-1" role="dialog" aria-labelledby="modal-event-label" aria-hidden="true" data-backdrop="static" data-keyboard="false">
            <div class="modal-dialog modal-lg" role="document">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title" id="modal-event-label-Import">Creación de Citas Consolidación CFS</h5>
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span>
                        </button>
                    </div>
                    <div class="modal-body">
                        <div class="row">
                            <div class="form-group col-md-4">
                                <label for="inputZip">FECHA DESDE</label>
                                <asp:TextBox ID="txtFechaDImport" runat="server" class="form-control" MaxLength="2" onkeypress="return soloLetras(event,'0123456789')" Style="text-align: left" placeholder=""></asp:TextBox>
                            </div> 
                            <div class="form-group col-md-4">
                                <label for="inputZip">FECHA HASTA</label>
                                <asp:TextBox ID="txtFechaHImport" runat="server" class="form-control" MaxLength="2" onkeypress="return soloLetras(event,'0123456789')" Style="text-align: left" placeholder=""></asp:TextBox>
                            </div>
                           
                        </div>
                        <div class="form-row">
                            <div class="form-group col-md-3">
                                <label for="inputZip">BODEGA</label>
                                <asp:DropDownList runat="server" ID="cboBloque" AutoPostBack="false" class="form-control"  ClientIDMode="Static"></asp:DropDownList>
                            </div>
                            <div class="form-group col-md-3">
                                <label for="inputZip">FRECUENCIA</label>
                                <asp:TextBox ID="txtFrecuenciaImport" class="form-control" runat="server" onkeypress="return soloLetras(event,'0123456789')" Style="text-align: center" placeholder=""  ></asp:TextBox>
                                </div>
                 
                            <div class="form-group col-md-2">
                                <label for="inputZip">&nbsp;</label>
                                <div class="d-flex">
                                    <button class="btn btn-primary" onclick="agregarDetalleImpo()" type="button">AGREGAR</button>
                                               </div>
                            </div>
                        </div>
             
                        <div class="form-row">
                            <div class="form-group col-md-12">
                                <table id="tablaDetallesIMPORT" class="table table-bordered invoice">
                                    <!-- Cabecera de la tabla -->
                                    <thead>
                                        <tr>
                                            <th class="center hidden-phone">#</th>
                                            <th class="center hidden-phone">TIPO DE BLOQUE</th>
                                            <th class="center hidden-phone">FRECUENCIA</th>
                                            <th class="center hidden-phone">QUITAR</th>
                                        </tr>
                                    </thead>
                                    <!-- Cuerpo de la tabla -->
                                    <tbody id="tbodyDetallesIMPORT">
                                    </tbody>
                                </table>
                            </div>
                        </div>
                        <div class="form-row">
                            <div class="form-group col-md-12">
                                <table id="tablaDetallesIMPORT2" class="table table-bordered invoice" style="display: none;">
                                    <!-- Cabecera de la tabla -->
                                    <thead>
                                        <tr>
                                            <th class="center hidden-phone">TIPO</th>
                                            <th class="center hidden-phone">REFERENCIA</th>
                                            <th class="center hidden-phone">ETA</th>
                                            <th class="center hidden-phone">CANTIDAD</th>

                                        </tr>
                                    </thead>
                                </table>
                            </div>
                        </div>
                        <button class="btn btn-primary" onclick="enviarDatosAlServidorImpor()" type="button">GENERAR CITAS</button>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-secondary" data-dismiss="modal">Cerrar</button>
                    </div>
   
                            <asp:TextBox type="hidden"  ID="TextBox4" runat="server" class="form-control" MaxLength="2" onkeypress="return soloLetras(event,'0123456789')" Style="text-align: left" placeholder=""></asp:TextBox>
                   
    
                </div>
            </div>
        </div>

        <!-- carga expo BRBK-->
        <div class="modal fade" id="modaltExpoLinea" tabindex="-1" role="dialog" aria-labelledby="modal-event-label" aria-hidden="true" data-backdrop="static" data-keyboard="false">
            <div class="modal-dialog modal-lg" role="document">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title" id="modal-event-label">Creación de Citas Carga Expo. BRBK</h5>
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span>
                        </button>
                    </div>
                    <div class="modal-body">
                        <div class="row">
                            <div class="form-group col-md-6">
                                <label for="inputZip">FECHA DESDE</label>
                                <asp:TextBox ID="txtFechaDesdeLineas" runat="server" class="form-control" MaxLength="2" onkeypress="return soloLetras(event,'0123456789')" Style="text-align: left" placeholder=""></asp:TextBox>
                            </div> 
                            <div class="form-group col-md-6">
                                <label for="inputZip">FECHA HASTA</label>
                                <asp:TextBox ID="txtFechaHastaLineas" runat="server" class="form-control" MaxLength="2" onkeypress="return soloLetras(event,'0123456789')" Style="text-align: left" placeholder=""></asp:TextBox>
                            </div>
                        
                        </div>
                        <div class="form-row">
                            <div class="form-group col-md-4">
                                <label for="inputZip">BODEGA</label>
                                <asp:DropDownList runat="server" ID="CboLineas" AutoPostBack="false" class="form-control"  ClientIDMode="Static"></asp:DropDownList>
                            </div>
                       
                            <div class="form-group col-md-4">
                                <label for="inputZip">CAPACIDAD</label>
                                <asp:TextBox ID="TxtCantidadLineas" runat="server" class="form-control" MaxLength="2"  onkeypress="return soloLetras(event,'1234567890')"  Style="text-align: left" placeholder=""></asp:TextBox>
                            </div>
                            <div class="form-group col-md-4">
                                <label for="inputZip">&nbsp;</label>
                                <div class="d-flex">
                                    <button class="btn btn-primary" onclick="agregarDetalleXLineas()" type="button">AGREGAR</button>
                                    &nbsp;&nbsp;
                                    <img alt="loading.." src="../lib/file-uploader/img/loading.gif" height="32px" width="32px" id="ImgCargaLineas" class="nover" />
                                </div>
                            </div>
                        </div>
              
                        <div class="form-row">
                            <div class="form-group col-md-12">
                                <table id="tablaDetallesLineas" class="table table-bordered invoice">
                                    <!-- Cabecera de la tabla -->
                                    <thead>
                                        <tr>
                                            <th class="center hidden-phone">#</th>
                                            <th class="center hidden-phone">BODEGA</th>
                                            <th class="center hidden-phone">CAPACIDAD</th>
                                            <th class="center hidden-phone">QUITAR</th>
                                        </tr>
                                    </thead>
                                    <!-- Cuerpo de la tabla -->
                                    <tbody id="tbodyDetallesLineas">
                                    </tbody>
                                </table>
                            </div>
                        </div>
                   
                        <button class="btn btn-primary" onclick="enviarDatosAlServidorLineas()" type="button">GENERAR CITAS</button>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-secondary" data-dismiss="modal">Cerrar</button>
                    </div>
   
                            <asp:TextBox type="hidden"  ID="banderaDayClickLineas" runat="server" class="form-control" MaxLength="2" onkeypress="return soloLetras(event,'0123456789')" Style="text-align: left" placeholder=""></asp:TextBox>
                   
    
                </div>
            </div>
        </div>

          <!-- Fin Modal para mostrar crear turnos -->

        <div id="loader" style="display: none;">
           <p>Cargando</p>            
            <!-- Agrega aquí tu imagen de carga o mensaje -->
        </div>


    </div>  
   
    <script type="text/javascript" src="../lib/pages.js"></script>
    <script type="text/javascript" src="../lib/advanced-form-components.js"></script>
    <script type="text/javascript" src="../lib/popup_script_cta.js"></script>
 
    <!-- Agrega el enlace al archivo de script de SweetAlert -->
    <script type="text/javascript" src="https://cdn.jsdelivr.net/npm/sweetalert2@11.0.19/dist/sweetalert2.all.min.js"></script>
    
</asp:Content>
  