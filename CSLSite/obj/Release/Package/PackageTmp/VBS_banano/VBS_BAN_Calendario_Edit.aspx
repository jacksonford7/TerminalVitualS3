 <%@ Page Title="Turnos Banano" Language="C#" MasterPageFile="~/site.Master" AutoEventWireup="true" CodeBehind="VBS_BAN_Calendario_Edit.aspx.cs" Inherits="CSLSite.VBS_BAN_Calendario_Edit" %>

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
    <script type="text/javascript" src="../lib/moment/moment_2_29_1.min.js"></script><%--<script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/moment.js/2.29.1/moment.min.js"></script>--%>
    <script type="text/javascript" src="../Scripts/fullcalendar.min.js"></script>
    <link href="../css/sweetalert2.min.css" rel="stylesheet" /> <%--<link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/sweetalert2@11.0.19/dist/sweetalert2.min.css" />--%>
    
   
    
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/5.15.4/css/all.min.css"/>
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
        var secuencia = 1; // Variable para generar la secuencia de forma incremental


        $(document).ready(function () {

            $('#calendarImpo').hide();
            
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
            $('#calendar').fullCalendar({
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

                    var v_txtnave = document.getElementById('<%=txtNave.ClientID %>').value;
                    var v_txtmrn = document.getElementById('<%=TXTMRN.ClientID %>').value;


                    var year = start.year();
                    var month = start.month() + 1;
                    // Realiza una llamada AJAX para obtener los eventos del servidor
                        $.ajax({
                            url: 'VBS_BAN_Calendario_Edit.aspx/GetCalendarEvents',
                            type: 'POST',
                            data: JSON.stringify({ year: year, month: month, inicial: initialLoad, referencia: v_txtnave, op: 1 }),
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
                dayClick: function (date, jsEvent, view) {
                    $('#tbodyBooking').empty();
                    $('#tablaDetalles2').hide();
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

                    var v_txtnave = document.getElementById('<%=txtNave.ClientID %>').value;
                    var v_txtmrn = document.getElementById('<%=TXTMRN.ClientID %>').value;

                    if (v_txtnave == null || v_txtnave == undefined || v_txtnave == '') {
                        mostrarAdvertencia("Se debe seleccionar una nave.");
                        return;
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
                                    <td class="center hidden-phone">${evento.LineaNaviera}</td>
                                    <td class="center hidden-phone">${evento.horarioInicial}</td>
                                    <td class="center hidden-phone">${evento.horarioFinal}</td>
       
                                    <td class="center hidden-phone">${evento.cantidad}</td>
                                    <td class="center hidden-phone">
                                    <button class="btn btn-primary" onclick="eliminarDetalle(this)" disabled>QUITAR</button>
                                    </td>
                                       `;

                                    // Agregar los datos del detalle como atributos personalizados a la fila
                                    fila.setAttribute('data-horario-inicial-id', evento.horarioInicialId);
                                    fila.setAttribute('data-horario-final-id', evento.horarioFinalId);

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
                eventClick: function (calEvent, jsEvent, view)
                {
                    var currentDate = moment().startOf('day'); // Obtener la fecha actual sin la hora
                    var selectedDate = calEvent.start.format(); // Obtener la fecha seleccionada sin la hora

                    var currentDateFormatted = currentDate.format('YYYY-MM-DD'); // Formatear la fecha actual
                    // Formatear la fecha seleccionada

                    var v_txtnave = document.getElementById('<%=txtNave.ClientID %>').value;
                    var v_txtmrn = document.getElementById('<%=TXTMRN.ClientID %>').value;

                    if (v_txtnave == null || v_txtnave == undefined || v_txtnave == '')
                    {
                        mostrarAdvertencia("Se debe seleccionar una nave.");
                        return;
                    }

                    if (selectedDate < currentDateFormatted)
                    {
                        mostrarAdvertencia("No puede seleccionar un rango de fecha anterior al actual");
                        return; // Salir de la función sin mostrar el modal
                    }
                    $('#modalActu').modal('show');

                    document.getElementById('<%=txtFechaActu.ClientID %>').value = calEvent.start.format();
                    document.getElementById('<%=txtFechaActu.ClientID %>').readOnly = true;

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
                            error: function () {

                            }
                        });
                    }

                    else if (view.name === 'month') {

                        var v_txtnave = document.getElementById('<%=txtNave.ClientID %>').value;
                        var v_txtmrn = document.getElementById('<%=TXTMRN.ClientID %>').value;


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
                                data: JSON.stringify({ year: year, month: month, inicial: initialLoad, referencia: v_txtnave, op: 2 }),
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

                select: function (start, end, jsEvent, view) {
                    var currentDate = moment().startOf('day'); // Obtener la fecha actual sin la hora
                    var selectedDate = moment(start).startOf('day'); // Obtener la fecha seleccionada sin la hora

                    var currentDateFormatted = currentDate.format('YYYY-MM-DD'); // Formatear la fecha actual
                    var selectedDateFormatted = selectedDate.format('YYYY-MM-DD'); // Formatear la fecha seleccionada


                    if (selectedDateFormatted < currentDateFormatted) {
                        mostrarAdvertencia("No puede seleccionar un rango de fecha anterior al actual");
                        return; // Salir de la función sin mostrar el modal
                    }

                    var v_txtnave = document.getElementById('<%=txtNave.ClientID %>').value;
                    var v_txtmrn = document.getElementById('<%=TXTMRN.ClientID %>').value;

                    if (v_txtnave == null || v_txtnave == undefined || v_txtnave == '') {
                        mostrarAdvertencia("Se debe seleccionar una nave.");
                        return;
                    }

                    var selectedEvents = $('#calendar').fullCalendar('clientEvents', function (event) {
                        return event.start.isBetween(start, end, 'day', '[]');
                    });

                    if (selectedEvents.length > 0)
                    {
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
        });

        $(document).ready(function () {


            // Evento de clic para el botón "Cerrar" del modal
            $('.modal').on('click', '[data-dismiss="modal"]', function () {
                // Limpiar la tabla de detalles
                document.getElementById('<%=TxtCantidad.ClientID %>').value = null;

                $('#tbodyDetalles').empty();

                secuencia = 1;
            });
        });

        $(document).ready(function () {
            // Manejar el evento de clic en el botón "Guardar"
            $("#btnGuardar").click(function () {
                guardarCambiosTabla();
            });
        });
        $(document).ready(function () {
            // Manejar el evento de clic en el botón "Guardar"
            $("#btnGuardarImport").click(function () {
                guardarCambiosTablaImport();
            });
        });

       
        $(document).ready(function () {
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
                    
                    if (cantidadModificada < cantidadAsignada) {
                        alert("No se permiten valores menores a lo ya asignado");
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
                            input.val(cantidadInicial);
                        }
                    }
                } catch (error) {
                    input.val(cantidadInicial);
                }
            }

            function moveToNextRow() {
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
                if (selectedValue === "1") {
                    // Show the "Exportación" calendar and hide the others
                    $('#calendar').show();
                    $('#calendar').fullCalendar('refetchEvents');
                    $('#calendar').fullCalendar(''); // Update the size of the calendar
                    $('#calendarImpo').hide();
                    
                } else if (selectedValue === "2") {
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

                                if (initialLoad)
                                {
                                    initialLoad = false; // Desactivar la bandera de carga inicial
                                }
                                else {

                                 /*
                                  var month = parseInt(view.start.format('MM'));
                                    month = month;
                                    //month = month + 1;
                                    var year = parseInt(view.start.format('YYYY'));
                                    // Remover los eventos existentes
                                    $('#calendarImpo').fullCalendar('removeEvents');
                                    $.ajax({
                                        url: 'VBS_BAN_Calendario_Edit.aspx/GetCalendarEventsImport',
                                        type: 'POST',
                                        data: JSON.stringify({ year: year, month: month, inicial: initialLoad }),
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
                                  */
                                  
                                  
                                   
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

                
                }
                
            });
        };


        function LlenarTabla(eventos) {
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
                fila.append("<td class='center hidden-phone lblLineaNaviera'>" + evento.LineaNaviera + "</td>");

                
                document.getElementById('<%=txtHorarioInicial.ClientID %>').value = evento.HoraInicio;
                document.getElementById('<%=txtHorarioFinal.ClientID %>').value = evento.HoraFinal;
                
                //fila.append("<td class='center hidden-phone'></td>"); // Celda vacía para las filas que no son "VACIOS"

                // Agregar una columna con ComboBox (select)
                /*var comboBoxColumna = $("<td class='center hidden-phone'>");
                var comboBox = $("<select class='form-control'></select>");

                // Agregar opciones al ComboBox (puedes personalizar esto)
                comboBox.append("<option value='opcion1'>Opción 1</option>");
                comboBox.append("<option value='opcion2'>Opción 2</option>");
                comboBox.append("<option value='opcion3'>Opción 3</option>");
                comboBoxColumna.append(comboBox);
                fila.append(comboBoxColumna);*/

                cuerpoTabla.append(fila);
            });
        }


        

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
        function agregarDetalle() {
            // Obtener los valores del detalle a través de los controles en la interfaz de usuario
            var cboLineaNaviera = document.getElementById("<%= cmbLineaNaviera.ClientID %>");
            var vLineaNaviera = cboLineaNaviera.options[cboLineaNaviera.selectedIndex].text;
            var lineaNavieraId = cboLineaNaviera.value;

            if (lineaNavieraId === '0') {
                mostrarError('Se debe seleccionar la linea');
                return false;
            }

            var cboHorarioInicial = document.getElementById("<%= cboHorarioInicial.ClientID %>");
            var vHorarioInicial = cboHorarioInicial.options[cboHorarioInicial.selectedIndex].text;
            var horarioInicialId = cboHorarioInicial.value;

            var cboHorarioFinal = document.getElementById("<%= cboHorarioFinal.ClientID %>");
            var vHorarioFinal = cboHorarioFinal.options[cboHorarioFinal.selectedIndex].text;
            var horarioFinalId = cboHorarioFinal.value;


            var cantidad = document.getElementById('<%=TxtCantidad.ClientID %>').value;
            if (cantidad === '') {
                mostrarError('El campo CANTIDAD es requerido');
                return false;
            }

            if (cantidad === '0') {
                mostrarError('El campo CANTIDAD debe ser mayor a cero');
                return false;
            }

            // Verificar si ya se ha ingresado un detalle con los mismos valores de horarioFinalId y horarioInicialId
            var detallesExistentes = document.getElementById('tbodyDetalles').getElementsByTagName('tr');
            for (var i = 0; i < detallesExistentes.length; i++) {
                var detalle = detallesExistentes[i];
                
                var detalleHorarioFinalId = detalle.getAttribute('data-horario-final-id');
                var detalleHorarioInicialId = detalle.getAttribute('data-horario-inicial-id');

                //alert(detalleHorarioFinalId.toString() + ' =' + horarioFinalId.toString());


                if (detalleHorarioFinalId === horarioFinalId && detalleHorarioInicialId === horarioInicialId) {
                    mostrarAdvertencia('No se pueden ingresar horarios repetidos');
                    return false;
                }
            }

            // Crear una nueva fila con los valores del detalle
            var fila = document.createElement('tr');
            fila.innerHTML = `
                        <td class="center hidden-phone">${secuencia}</td>
                        <td class="center hidden-phone">${vLineaNaviera}</td>
                        <td class="center hidden-phone">${vHorarioInicial}</td>
                        <td class="center hidden-phone">${vHorarioFinal}</td>
       
                        <td class="center hidden-phone">${cantidad}</td>
                        <td class="center hidden-phone">
                        <button class="btn btn-primary" onclick="eliminarDetalle(this)">QUITAR</button>
                        </td>  `;

            // Agregar los datos del detalle como atributos personalizados a la fila
            fila.setAttribute('data-horario-final-id', horarioFinalId);
            fila.setAttribute('data-horario-inicial-id', horarioInicialId);
            fila.setAttribute('data-linea-naviera-id', lineaNavieraId);

            // Agregar la fila al cuerpo de la tabla
            document.getElementById('tbodyDetalles').appendChild(fila);
             
            // Incrementar la secuencia para el siguiente detalle
            secuencia++;

            return false;
        }
        function agregarDetalleImpo() {
            // Obtener los valores del detalle a través de los controles en la interfaz de usuario
            var cboTipoBloque = document.getElementById("<%= cboBloque.ClientID %>");
            var vHorarioInicial = cboTipoBloque.options[cboTipoBloque.selectedIndex].text;
            var tipoBloqueId = cboTipoBloque.value;

            var frecuencia = document.getElementById('<%=txtFrecuenciaImport.ClientID %>').value;

            if (frecuencia === '') {
                mostrarError('El campo FRECUENCIA es requerido');
                return false;
            }

            // Verificar si ya se ha ingresado un detalle con los mismos valores de horarioFinalId y horarioInicialId
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
                          <td class="center hidden-phone">${vHorarioInicial}</td>
                         <td class="center hidden-phone">${frecuencia}</td>
                         <td class="center hidden-phone">
                             <button class="btn btn-primary" onclick="eliminarDetalle(this)">QUITAR</button>
                         </td>
                     `;

            // Agregar los datos del detalle como atributos personalizados a la fila
            //  fila.setAttribute('data-horario-final-id', horarioFinalId);
            fila.setAttribute('data-tipo-Bloque-id', tipoBloqueId);

            // Agregar la fila al cuerpo de la tabla
            document.getElementById('tbodyDetallesIMPORT').appendChild(fila);

            // Incrementar la secuencia para el siguiente detalle
            secuencia++;

            return false;
        }
        function enviarDatosAlServidor() {
            // Obtener los datos de la tabla
            var detalles = [];
            var tabla = document.getElementById('tablaDetalles');
            var filas = tabla.getElementsByTagName('tr');
            var tieneTipoContenedorTodos = false;
            var banderaDayClick = document.getElementById('<%=banderaDayClick.ClientID %>').value
            for (var i = 0; i < filas.length; i++) {
                var fila = filas[i];
                var celdas = fila.getElementsByTagName('td');

                // Verificar si se encontraron celdas en la fila actual
                if (celdas.length > 0) {

                    //Valida  que sea una fila nueva
                    var boton = celdas[5].getElementsByTagName('button')[0];

                    // Verificar si el botón está desactivado
                    var estaDesactivado = boton.disabled;

                    // Puedes hacer algo con la variable 'estaDesactivado', por ejemplo:
                    if (estaDesactivado) {
                        // Realizar acciones adicionales si el botón está desactivado
                    } else {

                        var secuencia = celdas[0].innerText;
                        var lineaNaviera = celdas[1].innerText;
                        var vHorarioInicial = celdas[2].innerText;
                        var vHorarioFinal = celdas[3].innerText;
                        var cantidad = celdas[4].innerText;

                        // Obtener los ID de tipoContenedor y tipoCarga de los atributos personalizados de la fila
                        var horarioFinalId = fila.getAttribute('data-horario-final-id');
                        var horarioInicialId = fila.getAttribute('data-horario-inicial-id');
                        var lineaNavieraId = fila.getAttribute('data-linea-naviera-id');

                        var detalle = {
                            secuencia: secuencia,
                            horarioFinalId: horarioFinalId,
                            vHorarioFinal: vHorarioFinal,
                            horarioInicialId: horarioInicialId,
                            vHorarioInicial: vHorarioInicial,
                            cantidad: cantidad,
                            lineaNavieraId: lineaNavieraId,
                            lineaNaviera: lineaNaviera
                        };

                        detalles.push(detalle);
                        // Verificar si el tipo de contenedor es 'Todos'
                        if (vHorarioFinal === 'TODOS') {
                            tieneTipoContenedorTodos = true;
                        }
                    }
                }
            }

            // Verificar si no se encontró ningún tipo de contenedor igual a 'Todos'
            //if (!tieneTipoContenedorTodos) {
            //    mostrarAdvertencia('Debe ingresar al menos un registro para el tipo de contenedor "Todos"');
            //    return;
            //}

            var vigenciaInicial = document.getElementById('<%=txtVigenciaInicial.ClientID %>').value;
            var vigenciaFinal = document.getElementById('<%=txtVigenciaFinal.ClientID %>').value;

            var vTxtruc = document.getElementById('<%=Txtruc.ClientID %>').value;
            var vTxtempresa = document.getElementById('<%=Txtempresa.ClientID %>').value;
            var vTxtcliente = document.getElementById('<%=Txtcliente.ClientID %>').value;
            var vTXTMRN = document.getElementById('<%=TXTMRN.ClientID %>').value;
            var vtxtNave = document.getElementById('<%=txtNave.ClientID %>').value;
            var vtxtDescripcionNave = document.getElementById('<%=txtDescripcionNave.ClientID %>').value;
            var vfecETA = document.getElementById('<%=fecETA.ClientID %>').value;

            if (detalles.length === 0) {
                mostrarAdvertencia('No hay detalles para enviar');
                return;
            }
            mostrarConfirmacion('¿Está seguro de generar las Citas agregadas?',
                function () {
                    $('#loader').show();
                    $.ajax({
                        url: "VBS_BAN_Calendario_Edit.aspx/GuardarDatosTabla1",
                        type: 'POST',
                        data: JSON.stringify({
                            vigenciaInicial: vigenciaInicial,
                            vigenciaFinal: vigenciaFinal,
                            banderaDayClick: banderaDayClick,
                            detalles: detalles,
                            vTxtruc: vTxtruc,
                            vTxtempresa: vTxtempresa,
                            vTxtcliente: vTxtcliente,
                            vTXTMRN: null,
                            vtxtNave: vtxtNave,
                            vtxtDescripcionNave: null,
                            vfecETA: null
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
                    var horarioInicialId = fila.getAttribute('data-tipo-Bloque-id');

                    var detalle = {
                        secuencia: secuencia,
                        bloqueId: horarioInicialId,
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
            mostrarConfirmacion('¿Está seguro de generar los Citas.?',
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

        function cboHorarioInicialChanged() {
            var cboHorarioInicial = document.getElementById("<%= cboHorarioInicial.ClientID %>");
            var horarioInicialId = cboHorarioInicial.value;

            // Llamar al servidor utilizando AJAX
            var xhr = new XMLHttpRequest();
            xhr.open("GET", "VBS_BAN_Calendario_Edit.aspx?horarioInicialId=" + horarioInicialId, true);
            xhr.onreadystatechange = function () {
                if (xhr.readyState === 4 && xhr.status === 200) {
                    // La respuesta del servidor ha sido recibida correctamente
                    var cboHorarioFinal = document.getElementById("<%= cboHorarioFinal.ClientID %>");
                    cboHorarioFinal.innerHTML = "";

                    // Agregar las nuevas opciones al DropDownList
                    cboHorarioFinal.innerHTML = xhr.responseText;

                }
            };
            xhr.send();
        }

        function eliminarDetalle(button) {
            // Obtener la fila padre del botón
            var fila = button.parentNode.parentNode;

            // Eliminar la fila de la tabla
            fila.remove();
        }



        function buscarBooking() {
            var fecha = document.getElementById('<%=txtFechaDesde.ClientID %>').value;

            // Realizar solicitud Ajax
            $.ajax({
                url: "VBS_BAN_Calendario_Edit.aspx/ConsultarBookingCalendario",
                data: JSON.stringify({ fecha: fecha }),
                contentType: "application/json; charset=utf-8",
                type: 'POST',
                success: function (response) {
                    var tableHTML = '';

                    // Verifica si la respuesta contiene datos
                    if (response && response.d) {
                        var data = JSON.parse(response.d);

                        // Generar la tabla a partir de los datos
                        for (var i = 0; i < data.length; i++) {
                            tableHTML += '<tr>';
                            tableHTML += '<td>' + data[i].description + '</td>';
                            tableHTML += '<td>' + data[i].reference + '</td>';
                            tableHTML += '<td>' + data[i].ETA + '</td>';
                            tableHTML += '<td>' + data[i].qty + '</td>';
                            tableHTML += '</tr>';
                        }
                    }

                    var tbodyBooking = document.getElementById('tbodyBooking');

                    tbodyBooking.innerHTML = tableHTML;

                    // Mostrar la tabla cambiando su estilo a display: table-row-group
                    var tablaDetalles = document.getElementById('tablaDetalles2');
                    tablaDetalles.style.display = 'table';
                },
                error: function (error) {

                }
            });
        }


    </script>


</asp:Content>



<asp:Content ID="Content2" ContentPlaceHolderID="placebody" runat="server">
    <asp:ToolkitScriptManager ID="tkscata" runat="server"></asp:ToolkitScriptManager>
     <asp:HiddenField ID="hf_mes" runat="server" />


    <div class="mt-4">         
        <nav class="mt-4" aria-label="breadcrumb">
            <ol class="breadcrumb">
                <li class="breadcrumb-item" id="Li1" runat="server"><a href="#">VBS Banano</a></li>
                <li class="breadcrumb-item active" aria-current="page" id="Li2" runat="server">CALENDARIO DE CITAS BANANO</li>
                <%--<li style="text-align:center; width:200px" class="breadcrumb-item active" aria-current="page" id="LiDropDownList1" runat="server">
                    <asp:DropDownList  runat="server" ID="DropDownList1" AutoPostBack="false" class="form-control" ClientIDMode="Static">
                        <asp:ListItem Text="Banano" Value="1"></asp:ListItem>
                        <asp:ListItem Text="Banano Bodega" Value="2"></asp:ListItem>
                    </asp:DropDownList>
                </li>--%>
            </ol>
        </nav>
    </div>

  <div class="dashboard-container p-4">
<%--      <div class="row">
    <div class="col-md-3">
        <div class="card p-1 ">
            <button id="btnFullScreen" onclick="abrirCalendarioPantallaCompleta_DOS()" style="border: none; background: none; display: flex; align-items: center;">
                <i class="fas fa-desktop" style="color: white; font-size: 50px; padding:2px; background-color: #2BADC6;  border-radius: 5px;"></i> 
                <span class="breadcrumb-item" style="flex: 1; display: flex; justify-content: center;">Monitor</span>
            </button>
        </div> 
    </div>
</div>--%>
        <div id="div_BrowserWindowName" style="visibility: hidden; height: 1px;">
            <asp:HiddenField ID="hf_BrowserWindowName" runat="server" />
            <asp:HiddenField ID="hf_BrowserWindowName2" runat="server" />
            <asp:TextBox ID="txtVigenciaInicial" runat="server" class="form-control" Style="text-align: left"></asp:TextBox>
            <asp:TextBox ID="txtVigenciaFinal" runat="server" class="form-control" Style="text-align: left"></asp:TextBox>
            <asp:TextBox ID="Txtcliente"  runat="server" class="form-control" placeholder=""  Font-Bold="true" ></asp:TextBox>
            <asp:TextBox ID="Txtruc" runat="server" class="form-control" placeholder=""  Font-Bold="true" ></asp:TextBox>
            <asp:TextBox ID="Txtempresa" runat="server" class="form-control" placeholder=""  Font-Bold="true" ></asp:TextBox>
            <asp:TextBox  ID="TXTMRN" runat="server" class="form-control" disabled MaxLength="16"  onkeypress="return soloLetras(event,'abcdefghijklmnopqrstuvwxyz0123456789-_')" placeholder="MRN"></asp:TextBox>
            <asp:TextBox class="form-control" runat="server" ID="fecETA" disabled   AutoPostBack="false" MaxLength="10"  onkeypress="return soloLetras(event,'0123456789/')" ></asp:TextBox>
        </div>

       

        <div class="form-title">
              DATOS GENERALES
        </div>

        <asp:UpdatePanel ID="UPCAB" runat="server"  UpdateMode="Conditional" ChildrenAsTriggers="true">
            <ContentTemplate>
                <div class="form-row">

                    <div class="form-group col-md-2"> 
                        <label for="inputAddress"> Tipo Turno<span style="color: #FF0000; font-weight: bold;">*</span><span style="color: #FF0000; font-weight: bold;"></span></label>
                        <div class="d-flex"> 
                            <li style="text-align:center; width:200px" class="breadcrumb-item active" aria-current="page" id="LiDropDownList1" runat="server">
                                <asp:DropDownList  runat="server" ID="DropDownList1" AutoPostBack="false" class="form-control" ClientIDMode="Static">
                                    <asp:ListItem Text="Banano Muelle" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="Banano Bodega" Value="2"></asp:ListItem>
                                </asp:DropDownList>
                            </li>
                        </div>
                    </div>
         
                    <div class="form-group col-md-2"> 
                        <label for="inputAddress"> Referencia<span style="color: #FF0000; font-weight: bold;">*</span><span style="color: #FF0000; font-weight: bold;"></span></label>
                        <div class="d-flex"> 
                            <asp:TextBox  class="form-control" disabled  ID="txtNave" AutoPostBack="true" runat="server" MaxLength="30" onkeypress="return soloLetras(event,'abcdefghijklmnopqrstuvwxyz01234567890/')" ></asp:TextBox>
                        </div>
                    </div>

                    <div class="form-group col-md-6"> 
                        <label for="inputAddress">Nombre de Nave<span style="color: #FF0000; font-weight: bold;">*</span><span style="color: #FF0000; font-weight: bold;"></span></label>
                        <div class="d-flex"> 
                            <asp:TextBox  class="form-control"  ID="txtDescripcionNave" disabled runat="server" MaxLength="200" onkeypress="return soloLetras(event,'abcdefghijklmnopqrstuvwxyz01234567890/')"></asp:TextBox>
                            &nbsp;
                            <a class="btn btn-outline-primary mr-4" style='font-size:24px' target="popup" onclick="window.open('../catalogo/naves.aspx','name','width=900,height=880')"><span class="fas fa-search" style='font-size:24px'></span> </a>
                        </div>
                    </div>

                    
                    

                    <%--<div class="form-group col-md-2">
                        <label for="inputZip">MRN<span style="color: #FF0000; font-weight: bold;">*</span></label>
                        <asp:TextBox Visible ="false" ID="TXTMRN" runat="server" class="form-control" disabled MaxLength="16"  onkeypress="return soloLetras(event,'abcdefghijklmnopqrstuvwxyz0123456789-_')" placeholder="MRN"></asp:TextBox>
                    </div>

                    <div class="form-group   col-md-2"> 
                        <label for="inputAddress"> ETA:<span style="color: #FF0000; font-weight: bold;"></span></label>

                        <div class="d-flex">
                            <asp:TextBox class="form-control" Visible ="false" runat="server" ID="fecETA" disabled   AutoPostBack="false" MaxLength="10"  onkeypress="return soloLetras(event,'0123456789/')" ></asp:TextBox>

                            <asp:CalendarExtender ID="CAGTFECHAFASA"  runat="server"
                                CssClass="cal_Theme1" Format="dd/MM/yyyy" TargetControlID="fecETA">
                            </asp:CalendarExtender>      
                        </div>
                    </div>--%>

                    <div class="form-group col-md-2"> 
                        <div class="d-flex">
                            <asp:DropDownList Visible ="false" ID="cmbEstado" class="form-control" runat="server" Font-Size="Medium" disabled  Font-Bold="true" >
                            </asp:DropDownList>
                            <a class="tooltip" ><span class="classic" >Estados generales</span><img alt='' src='../shared/imgs/info.gif' class='datainfo'/></a>
                        </div>
                    </div>
             
                </div>

                <div></div>

                <div class="row">
                    <div class="col-md-12 d-flex justify-content-center">
                        <asp:Button ID="btnLimpiar" runat="server" class="btn btn-primary"  Text="Limpiar" OnClick="btnLimpiar_Click" />
                         &nbsp;
                        <asp:Button Visible="false" ID="btnBuscar" runat="server" class="btn btn-primary"  Text="Buscar"  OnClientClick="return mostrarloader('1')"  />
                        <img alt="loading.." src="../lib/file-uploader/img/loading.gif" height="32px" width="32px"  id="ImgCargas" class="nover"  />
                        <span id="imagen"></span>
                    </div>
                </div>

                 <br/>
                <div class="row">
                    <div class="col-md-12 d-flex justify-content-center">
                        <div class="alert alert-warning" id="banmsg" runat="server" clientidmode="Static"><b>Error!</b> >......</div>
                    </div>
                </div>

             </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnBuscar" />
            </Triggers>
        </asp:UpdatePanel>


          <div id="calendar"></div>
          <div id="calendarImpo"></div>
          
    <!-- Modal para mostrar Actualizar turnos -->

        <div class="modal fade" id="modalActu" tabindex="-1" role="dialog" aria-labelledby="modal-event-label" aria-hidden="true" data-backdrop="static" data-keyboard="false">
            <div class="modal-dialog modal-xl" role="document">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title" id="modal-Actu-label">Actualización de Citas creadas.</h5>
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
                                                              <label for="inputZip">HORA INICIO</label>
                                <asp:TextBox ID="txtHorarioInicial" runat="server" class="form-control" MaxLength="2" onkeypress="return soloLetras(event,'0123456789')" Style="text-align: left"
                                    placeholder="">
                                </asp:TextBox>
                            </div>
                               <div class="form-group col-md-3">
                                                              <label for="inputZip">HORA FIN</label>
                                <asp:TextBox ID="txtHorarioFinal" runat="server" class="form-control" MaxLength="2" onkeypress="return soloLetras(event,'0123456789')" Style="text-align: left"
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
                                                    <th class="center hidden-phone">LINEA </th>
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


        <div class="modal fade" id="modalActuImport" tabindex="-1" role="dialog" aria-labelledby="modal-event-label" aria-hidden="true" data-backdrop="static" data-keyboard="false">
            <div class="modal-dialog modal-xl" role="document">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title" id="modal-Actu-label-Import">Actualización de Citas creadas</h5>
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

    <!-- Modal para mostrar crear turnos -->
        <div class="modal fade" id="modalt" tabindex="-1" role="dialog" aria-labelledby="modal-event-label" aria-hidden="true" data-backdrop="static" data-keyboard="false">
            <div class="modal-dialog modal-lg" role="document">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title" id="modal-event-label">Creación de Citas</h5>
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span>
                        </button>
                    </div>
                    <div class="modal-body">
                        <div class="row">
                            <div class="form-group col-md-3">
                                <label for="inputZip">LINEA</label>
                                <asp:DropDownList runat="server" ID="cmbLineaNaviera" AutoPostBack="false" class="form-control" ClientIDMode="Static"></asp:DropDownList>
                            </div>
                            <div class="form-group col-md-4">
                                <label for="inputZip">FECHA DESDE</label>
                                <asp:TextBox ID="txtFechaDesde" runat="server" class="form-control" MaxLength="2" onkeypress="return soloLetras(event,'0123456789')" Style="text-align: left" placeholder=""></asp:TextBox>
                            </div> 
                            <div class="form-group col-md-4">
                                <label for="inputZip">FECHA HASTA</label>
                                <asp:TextBox ID="txtFechaHasta" runat="server" class="form-control" MaxLength="2" onkeypress="return soloLetras(event,'0123456789')" Style="text-align: left" placeholder=""></asp:TextBox>
                            </div>
                           <%-- <div class="form-group col-md-4">
                                <label for="inputZip">Información Naves</label>
                                <button class="btn btn-primary" onclick="buscarBooking()" type="button">BUSCAR NAVE</button>
                            </div>--%>
                        </div>
                        <div class="form-row">
                            <div class="form-group col-md-3">
                                <label for="inputZip">HORARIO INICIAL</label>
                                <asp:DropDownList runat="server" ID="cboHorarioInicial" AutoPostBack="false" class="form-control" OnChange="cboHorarioInicialChanged()" ClientIDMode="Static"></asp:DropDownList>
                            </div>
                            <div class="form-group col-md-3">
                                <label for="inputZip">HORARIO FINAL</label>
                                <asp:DropDownList runat="server" ID="cboHorarioFinal" AutoPostBack="false" class="form-control" ClientIDMode="Static"></asp:DropDownList>
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
                                            <th class="center hidden-phone">LINEA</th>
                                            <th class="center hidden-phone">HORARIO INICIAL</th>
                                            <th class="center hidden-phone">HORARIO FINAL</th>
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
                        <div class="form-row">
                            <div class="form-group col-md-12">
                                <table id="tablaDetalles2" class="table table-bordered invoice" style="display: none;">
                                    <!-- Cabecera de la tabla -->
                                    <thead>
                                        <tr>
                                            <th class="center hidden-phone">TIPO</th>
                                            <th class="center hidden-phone">REFERENCIA</th>
                                            <th class="center hidden-phone">ETA</th>
                                            <th class="center hidden-phone">CANTIDAD</th>

                                        </tr>
                                    </thead>
                                    <!-- Cuerpo de la tabla -->
                                    <tbody id="tbodyBooking">
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

        <div class="modal fade" id="modaltImport" tabindex="-1" role="dialog" aria-labelledby="modal-event-label" aria-hidden="true" data-backdrop="static" data-keyboard="false">
            <div class="modal-dialog modal-lg" role="document">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title" id="modal-event-label-Import">Creación de Citas</h5>
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
                            <div class="form-group col-md-4">
                                <label for="inputZip">Información Naves</label>
                                <button class="btn btn-primary" onclick="buscarBooking()" type="button">BUSCAR NAVE</button>
                            </div>
                        </div>
                        <div class="form-row">
                            <div class="form-group col-md-3">
                                <label for="inputZip">TIPO DE BLOQUE</label>
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
                                    <!-- Cuerpo de la tabla -->
                                    <tbody id="tbodyBookingIMPORT2">
                                    </tbody>
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
  

     <script type="text/javascript">

        function mostrarloader(Valor) {

            try {
                if (Valor == "1") {
                    document.getElementById("ImgCarga").className = 'ver';
                    document.getElementById("ImgCargas").className = 'ver';
                }
                else {
                      document.getElementById("ImgCargaDet").className='ver';            
                }
                
                } catch (e) {
                    alert('Lamentamos comunicarle que ha ocurrido el siguiente problema:\n' + e.Message);
                }
        }

        function ocultarloader(Valor) {
            try {

                if (Valor == "1") {
                    document.getElementById("ImgCarga").className = 'nover';
                    document.getElementById("ImgCargas").className = 'nover';
                }
                else {
                    document.getElementById("ImgCargaDet").className='nover';
                }

                 } catch (e) {
                    alert('Lamentamos comunicarle que ha ocurrido el siguiente problema:\n' + e.Message);
                }
        }

        function prepareObjectRuc() {
            try {
                document.getElementById("loader3").className = '';
                var vals = document.getElementById('<%=TXTMRN.ClientID %>').value;
                if (vals == null || vals == undefined || vals == '') {
                    alert('¡ Escriba el MRN.');
                    document.getElementById("loader3").className = 'nover';
                    document.getElementById('<%=TXTMRN.ClientID %>').focus();
                    return false;
                }
                return true;
            } catch (e) {
                alert('Lamentamos comunicarle que ha ocurrido el siguiente problema:\n' + e.Message);
            }
        }

        function popupCallback(catalogo) {
            if (catalogo == null || catalogo == undefined) {
                alert('Hubo un problema al setaar un objeto de catalogo');
                return;
            }
            this.document.getElementById('<%= txtNave.ClientID %>').value = catalogo.codigo;
            this.document.getElementById('<%= txtDescripcionNave.ClientID %>').value = catalogo.descripcion;

             $('#calendar').fullCalendar('refetchEvents');
        }

    </script>

    <script type="text/javascript">
        $(document).ready(function () {
                $('.datetimepicker').datetimepicker().datetimepicker({ lang: 'es', timepicker: false, step: 30, format: 'm/d/Y' });
            });    
    </script>
    
</asp:Content>
  