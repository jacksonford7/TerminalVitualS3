<%@ Page Title="AISV exportacion, consolidacion" Language="C#" MasterPageFile="~/site.Master" AutoEventWireup="true" CodeBehind="vacios.aspx.cs" Inherits="CSLSite.aisv.vacios" %>
<%@ MasterType VirtualPath="~/site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="placehead" runat="server">

 <link href="../shared/estilo/tables.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/aisv-general.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/work.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/catalogos.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/info.css" rel="stylesheet" type="text/css" />
    <link href="../img/favicon2.png" rel="icon" />
    <link href="../img/icono.png" rel="apple-touch-icon" />
    <link href="../css/bootstrap.min.css" rel="stylesheet" />
    <link href="../css/dashboard.css" rel="stylesheet" />
    <link href="../css/icons.css" rel="stylesheet" />
    <link href="../css/style.css" rel="stylesheet" />
    <link href="https://fonts.googleapis.com/css2?family=Roboto:wght@300;400;500;700&display=swap" rel="stylesheet" />
    <link href="../shared/estilo/jquery.datetimepicker.css" rel="stylesheet" type="text/css" />
    <!--Datatables-->
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/5.15.1/css/all.min.css" integrity="sha512-+4zCK9k+qNFUR5X+cKL9EIR+ZOhtIloNl9GIKS57V1MyNsYpYcUrUeQc9vNfzsWfV28IaLL3i96P9sdNyeRssA==" crossorigin="anonymous" />
    <link rel="stylesheet" type="text/css" href="../js/buttons/css/buttons.dataTables.min.css" />
    <script src="../lib/jquery/jquery.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="../js/datatables.js"></script>
    <script src="../Scripts/table_catalog.js" type="text/javascript"></script>
    <script type="text/javascript" src="../js/bootstrap.bundle.min.js"></script>
    <link href="../css/datatables.min.css" rel="stylesheet" />

          <script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/moment.js/2.29.1/moment.min.js"></script>
    <script type="text/javascript" src="https://cdn.jsdelivr.net/npm/sweetalert2@11.0.19/dist/sweetalert2.all.min.js"></script>
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/sweetalert2@11.0.19/dist/sweetalert2.min.css" />
    <script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/fullcalendar/3.10.2/fullcalendar.min.js"></script>
    <link href="https://cdnjs.cloudflare.com/ajax/libs/fullcalendar/3.10.2/fullcalendar.min.css" rel="stylesheet" />
      <script type="text/javascript" src="../js/Confirmaciones.js""></script>


       
    <style type="text/css">
        .modal-dialog.modal-xm {
            max-width: 950px; /* Ajusta el ancho máximo según tus necesidades */
            max-height: 650px;
        }
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="placebody" runat="server">
              <input id="zonaid" type="hidden" value="105" />
              <input id="bandera"     type="hidden"   runat="server" clientidmode="Static"  />
              <input id="procesar"    type="hidden"   runat="server" clientidmode="Static"  />
              <input id="itemT4"      type="hidden"   runat="server" clientidmode="Static"  />
              <input id="linea_validar"     type="hidden"   runat="server" clientidmode="Static"  />
    
    <div class="mt-4">         
        <nav class="mt-4" aria-label="breadcrumb">
          <ol class="breadcrumb">
            <li class="breadcrumb-item" id="opcion_principal" runat="server"><a href="#">Servicios a clientes de CGSA</a></li>
            <li class="breadcrumb-item active" aria-current="page" id="sub_opcion" runat="server">AISV para exportación o consolidación de contenedores</li>
          </ol>
        </nav>
      </div>
    <asp:HiddenField ID="hiddenTablaData" runat="server" />
    <!-- White Container -->
<div class="dashboard-container p-4" id="cuerpo" runat="server">
      <div class="form-title">Datos para procesar</div>
           
     
     <div class="form-row">
          <div class="form-group col-md-6">
                          <div class="form-group col-md-12">
                    <label for="inputAddress">Booking:<span style="color: #FF0000; font-weight: bold;"> *</span></label>
                    <div class="d-flex">
                         <span id="refnumber" class="form-control col-md-11 " runat="server" clientidmode="Static" enableviewstate="true">...</span>
                         <a  class="btn btn-outline-primary mr-4" target="popup" onclick="onBook();"  >
                                <span class='fa fa-search' style='font-size:24px'></span> </a>
                        <span id="valnave" class="validacion"></span>
                    </div>
         </div>   
              
              

         </div> 

           <div class="col-md-6"> 
                  <label for="inputEmail4">Operación</label>
		     <div class="d-flex">
                     
                     <div class="d-flex">
                         <label class="checkbox-container">
                               <input runat="server" clientidmode="Static" enableviewstate="true" id="vacio" name="fk" type="radio" value="MTY" checked="true" class="xradio" />&nbsp;Exportación
                                <span class="checkmark"></span>
                        </label><label for="inputEmail4">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</label>
                         <label class="checkbox-container"> 
                               <input runat="server" clientidmode="Static" enableviewstate="true" id="consolida" name="fk" type="radio" value="LCL"  class="xradio"/>&nbsp;Consolidación
                                <span class="checkmark"></span>
                        </label>
                         <span id="valope" class="validacion"> </span>
                     </div> 
		     </div>
		   </div> 

                
        </div>
     <div class="form-row">


         <div class="form-group col-md-12">
                    <label for="inputAddress">Archivo CSV:<span style="color: #FF0000; font-weight: bold;"> *</span></label>
                    <div class="d-flex">
                           <input class="uploader form-control " id="fsupload" title="Escoja el archivo CSV (Excel separado por comas)" accept="csv" type="file"  runat="server" clientidmode="Static" />
                          <asp:Button ID="btup" runat="server" Text="Cargar" onclick="btup_Click"  OnClientClick="return sendform();"
                             ToolTip="Carga el archivo y valida la información" class="btn  btn-primary"/>
                       <span id="valdae" class="validacion"></span>
                    </div>
         </div> 
 </div>
      <div class="form-row">
         <div class="form-group col-md-12">
                   <label for="inputAddress">Mail notificación adicional<span style="color: #FF0000; font-weight: bold;"></span></label>
               <input type='text' id='textbox1' name='textbox1'  runat="server" class="form-control "
                enableviewstate="false" clientidmode="Static"
               onkeypress="return soloLetras(event,'abcdefghijklmnñopqrstuvwxyz1234567890_$@.',true)"  
                onblur="maildata(this,'valmailz');" maxlength="50"/>
             <span id="valmailz" class="validacion"></span>
         </div> 

    </div>
 
  
      <div class="form-title">Resultados de la verificación del archivo </div>
          <div class="form-row" id="xfinder" runat="server" visible="true">
   
     <div id="unit_ok" class="form-group col-md-4" runat="server" clientidmode="Static" >
                                    </div>
     <div id="unit_error" class="form-group col-md-8" runat="server" clientidmode="Static">
                                    </div>
                             
                             
                
 
     </div>

    <div class="form-row" >
            <div class="form-group col-md-3"> 
                 <asp:TextBox ID="txtISO" runat="server"  CssClass="datetimepicker form-control" 
             onkeypress="return soloLetras(event,'01234567890/')" 
                 onblur="valDate(this,true,valdate);" ClientIDMode="Static" hidden="hidden"></asp:TextBox>
          </div>
    </div>


       <div class="form-row">
		   <div class="col-md-12 d-flex justify-content-center"> 
		         <input clientidmode="Static" id="dataexport" onclick="prepareSessionVariable(); getfile('resultado');" type="button" value="Exportar" runat="server" class="btn btn-secondary"/>
		   </div> 
		   </div>

    <div class="form-row">
		   <div class="col-md-12 d-flex justify-content-center"> 
		                    <div id="sinresultado" runat="server" class="alert alert-warning" clientidmode="Static"></div>

		   </div> 
		   </div>

    <div class="form-row">
                    <div class="col-md-12 d-flex justify-content-center">
                     <asp:Button ID="btbuscar" runat="server" Text="Generar documentos"  onclick="btbuscar_Click" 
                           OnClientClick="return getprocesa();"  
                             ToolTip="Confirma la información y genera los preavisos"  class="btn  btn-primary"/>
                    </div>
               </div>
   
</div>
    
    <script src="../Scripts/pages.js" type="text/javascript"></script>        

                             <div class="dashboard-container p-4" id="BodyModal" runat="server">

                   <div class="modal fade" id="modalActu" tabindex="-1" role="dialog" aria-labelledby="modal-event-label" aria-hidden="true" data-backdrop="static" data-keyboard="false">
        <div class="modal-dialog modal-xm" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="modal-Actu-label">Creación de turnos</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">

                    <div class=" form-row">
           
                    </div>

                    <div class="form-row">
           

                  

                        <div class="form-group col-md-3"> 
           <label for="inputAddress">Fecha Nuevo turno:<span style="color: #FF0000; font-weight: bold;"></span></label>
               <asp:TextBox ID="txtFechaNuevoTurno" runat="server"  CssClass="datetimepickerAlt form-control" 
             onkeypress="return soloLetras(event,'01234567890/')"  
                 ClientIDMode="Static"></asp:TextBox>
                               <span class="opcional" id="valfecfum"></span>
          </div>
                          <div class=" form-group col-md-2">
                        <label for="inputHorllgda">Hora llegada turno:<span style="color: #FF0000; font-weight: bold;"></span></label>
               <asp:TextBox ID="txtHorallegada" runat="server" 
                ClientIDMode="Static" CssClass="form-control"></asp:TextBox>
                        </div>
                       <div class="form-group col-md-2">
                               <label for="btnActualizar">Crear:<span style="color: #FF0000; font-weight: bold;"></span></label>
       
                             <button class="btn btn-primary" id="btnActualizar" <%--onclick="enviarDatosAlServidor()"--%> type="button">Crear Turno</button>
                     
                       </div>
                       <%-- <div class=" form-group col-md-1">
                            <input runat="server" ID="txtAisv" hidden="hidden"/>

                        </div>--%>
                         
                        <div class=" form-group col-md-1">
                            <input runat="server" ID="txtIdTurno" hidden="hidden"/>

                        </div>
                        <br />
                    </div>

                    <div class="row">
                           <div id="calendar"></div>
                    </div>
                         
                
              
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-dismiss="modal">Cerrar</button>
                </div>
            </div>
        </div>
    </div>
        </div>
              


  
    
    <script type="text/javascript"> 
        var positionboton = null;
        var botonIdGlobal;
        var TurnoDisponible;
        var contadorBoton = 0;
        function crearTurno(btn) {
            var row = $(btn).closest("tr");
            var posicionBoton = row.index(); // Obtener el índice de la fila en la tabla

            botonIdGlobal = btn.id; // Asignar el ID del botón a la variable global

            console.log("posicionboton", posicionBoton);
            console.log("botonIdGlobal", botonIdGlobal);

            mostrarModalFecha(posicionBoton);
        }

        function mostrarModalFecha(posicionBoton) {
            // Crear el elemento del modal y el datepicker
            positionboton = posicionBoton;
            $('#txtHorallegada').val('');
            $('#txtIdTurno').val('');
            document.getElementById('<%=txtFechaNuevoTurno.ClientID %>').value = '';
            $('#calendar').fullCalendar('destroy');
            $('#modalActu').modal('show');
        }

        $('#txtFechaNuevoTurno').on('input change', function () {
            var fechaSelecci = $(this).val();
            fechaSeleccionada(fechaSelecci);
        });

        function fechaSeleccionada(fecha) {
            var ISO = document.getElementById('txtISO').value;
            if (fecha == '') {
                // Realizar acciones si la fecha está vacía
            } else {
                // Obtener la fecha actual sin las horas
                var fechaActual = new Date();
                fechaActual.setHours(0, 0, 0, 0);

                // Convertir la fecha seleccionada en un objeto Date
                var partesFecha = fecha.split('/');
                var fechaSeleccionadaObj = new Date(partesFecha[2], partesFecha[1] - 1, partesFecha[0]);
                fechaSeleccionadaObj.setHours(0, 0, 0, 0);

                // Comparar las fechas
                if (fechaSeleccionadaObj < fechaActual) {
                    mostrarAdvertencia("La fecha seleccionada no debe ser menor a la actual");
                    // Realizar las acciones necesarias si la fecha es menor
                } else {
                    var anio = fechaSeleccionadaObj.getFullYear();
                    var mes = ('0' + (fechaSeleccionadaObj.getMonth() + 1)).slice(-2); // El mes comienza en 0, por lo que se suma 1
                    var dia = ('0' + fechaSeleccionadaObj.getDate()).slice(-2);
                    var fechaFormateada = anio + '-' + mes + '-' + dia;

                    // Realizar la consulta AJAX para obtener los eventos
                    $.ajax({
                        url: 'vacios.aspx/ConsultarEventosPorDiaAISV',
                        type: 'POST',
                        data: JSON.stringify({ start: fechaFormateada, varTodos: ISO }),
                        contentType: 'application/json; charset=utf-8',
                        dataType: 'json',
                        success: function (data) {
                            var jsonArray = JSON.parse(data.d);
                            var allTurnosZero = true;
                            var events = [];

                            for (var i = 0; i < jsonArray.length; i++) {
                                var evento = jsonArray[i];
                                var startDateTime = moment(fechaFormateada + ' ' + evento.horario, 'YYYY-MM-DD HH:mm:ss');
                                var endDateTime = startDateTime.clone().add(1, 'hour');
                                var color = evento.cantidad === 0 ? "#FF0000" : evento.color;
                                var event = {
                                    id: evento.idDetalle,
                                    title: evento.title,
                                    start: startDateTime,
                                    end: endDateTime,
                                    color: color,
                                    cantidad: evento.cantidad
                                };
                                events.push(event);

                                if (evento.cantidad !== 0) {
                                    allTurnosZero = false;
                                }
                            }

                            // Verificar si todos los turnos son cero
                            if (allTurnosZero) {
                                mostrarConfirmacion('No tenemos Turnos Disponibles para la fecha seleccionada, ¿Desea buscar nuevos datos?', function () {
                                    // Realizar una nueva consulta AJAX para obtener los nuevos eventos
                                    $.ajax({
                                        url: 'vacios.aspx/ConsultarEventosPorDiaAISV',
                                        type: 'POST',
                                        data: JSON.stringify({ start: fechaFormateada, varTodos: 'TODOS' }),
                                        contentType: 'application/json; charset=utf-8',
                                        dataType: 'json',
                                        success: function (data) {
                                            var newEvents = [];
                                            var jsonArray = JSON.parse(data.d);
                                            var allTurnosZero = true;

                                            for (var i = 0; i < jsonArray.length; i++) {
                                                var evento = jsonArray[i];
                                                var startDateTime = moment(fechaFormateada + ' ' + evento.horario, 'YYYY-MM-DD HH:mm:ss');
                                                var endDateTime = startDateTime.clone().add(1, 'hour');
                                                var color = evento.cantidad === 0 ? "#FF0000" : evento.color;
                                                var event = {
                                                    id: evento.idDetalle,
                                                    title: evento.title,
                                                    start: startDateTime,
                                                    end: endDateTime,
                                                    color: color,
                                                    cantidad: evento.cantidad
                                                };
                                                newEvents.push(event);

                                                if (evento.cantidad !== 0) {
                                                    allTurnosZero = false;
                                                }
                                            }

                                            // Actualizar el calendario con los nuevos eventos
                                            $('#calendar').fullCalendar('destroy');
                                            $('.fc-axis.fc-time.fc-widget-content').css('height', 60 + 'px');
                                            $('#calendar').fullCalendar({
                                                defaultView: 'agendaDay',
                                                defaultDate: fechaFormateada,
                                                header: {
                                                    left: '',
                                                    center: 'title',
                                                    right: ''
                                                },
                                                editable: false,
                                                events: newEvents,
                                                displayEventTime: false,
                                                eventClick: function (calEvent, jsEvent, view) {
                                                    var horaSeleccionada = calEvent.start.format('HH:mm');
                                                    var idTurno = calEvent.id;
                                                    TurnoDisponible = calEvent.cantidad;

                                                    if (TurnoDisponible == 0) {
                                                        mostrarAdvertencia("Turno que eligió ya no tiene disponibilidad, elija otra hora");
                                                    } else {
                                                        mostrarConfirmacion('¿Está de acuerdo con seleccionar la hora ' + horaSeleccionada + '?', function () {
                                                            var headerRow = $('#tablasort thead tr');
                                                            // Insertar los encabezados en las celdas respectivas
                                                         
                                                            $('#txtHorallegada').val(horaSeleccionada);
                                                            $('#txtIdTurno').val(idTurno);
                                                            document.getElementById('<%=txtIdTurno.ClientID %>').value = idTurno;
                                                            $('#myModal').modal('hide');
                                                        });
                                                    }
                                                }
                                            });

                                            // Realizar acciones adicionales si todos los turnos son cero
                                            if (allTurnosZero) {
                                                // ...
                                            }
                                        },
                                        error: function () {
                                            mostrarError('Error al realizar la consulta');
                                        }
                                    });
                                }, function () {
                                    $('#myModal').modal('hide'); // Cerrar el modal si el usuario selecciona "No"
                                });
                            } else {
                                // Crear el calendario con los eventos obtenidos
                                $('#calendar').fullCalendar('destroy');
                                $('.fc-axis.fc-time.fc-widget-content').css('height', 60 + 'px');
                                $('#calendar').fullCalendar({
                                    defaultView: 'agendaDay',
                                    defaultDate: fechaFormateada,
                                    header: {
                                        left: '',
                                        center: 'title',
                                        right: ''
                                    },
                                    editable: false,
                                    events: events,
                                    displayEventTime: false,
                                    eventClick: function (calEvent, jsEvent, view) {
                                        var horaSeleccionada = calEvent.start.format('HH:mm');
                                        var idTurno = calEvent.id;
                                        TurnoDisponible = calEvent.cantidad;

                                        if (TurnoDisponible == 0) {
                                            mostrarAdvertencia("Turno que eligió ya no tiene disponibilidad, elija otra hora");
                                        } else {
                                            mostrarConfirmacion('¿Está de acuerdo con seleccionar la hora ' + horaSeleccionada + '?', function () {
                                                $('#txtHorallegada').val(horaSeleccionada);
                                                $('#txtIdTurno').val(idTurno);
                                                document.getElementById('<%=txtIdTurno.ClientID %>').value = idTurno;
                                                $('#myModal').modal('hide');
                                            });
                                        }
                                    }
                                });
                            }
                        },
                        error: function () {
                            mostrarError('Error al realizar la consulta');
                        }
                    });
                }
            }
        }

        var btnActualizar = document.getElementById('btnActualizar');
        btnActualizar.addEventListener('click', enviarSolicitudAJAX);

        function enviarSolicitudAJAX() {
            // Variable para contar el número de veces que se ha presionado el botón
            if (botonIdGlobal === "btnCrearMasivo") {
                contadorBoton++;

                var filaIndex = positionboton + 2;

                var tabla = document.getElementById('tablasort');
                var numFilas = tabla.rows.length;

                if (contadorBoton > 1) {
                    var filasVacias = [];

                    // Encontrar las posiciones de las filas vacías a partir de la posición del botón
                    for (var i = filaIndex; i < numFilas; i++) {
                        var fila = tabla.rows[i];
                        var fechaLlegadaCell = fila.cells[2];
                        var horaLlegadaCell = fila.cells[3];
                      

                        if (!fechaLlegadaCell || fechaLlegadaCell.textContent === "" || !horaLlegadaCell || horaLlegadaCell.textContent === "") {
                            filasVacias.push(i);
                        }
                    }


                    for (var i = 0; i < TurnoDisponible && i < filasVacias.length; i++) {
                        filaIndex = filasVacias[i];

                      

                        // Obtener los valores del turno
                        var IdTurno = document.getElementById('<%=txtIdTurno.ClientID %>').value;
                        var Fecha = document.getElementById('<%=txtFechaNuevoTurno.ClientID %>').value;
                        var Horallegada = document.getElementById('<%=txtHorallegada.ClientID %>').value;

                        // Obtener la fila correspondiente a la posición
                        var fila = tabla.rows[filaIndex];


                        // Verificar si las celdas de fecha y hora de llegada ya existen y no están vacías
                        var fechaLlegadaCell = fila.cells[2];
                        var horaLlegadaCell = fila.cells[3];
                        var idTurnoCell = fila.cells[4];

                        console.log("idTurno",idTurnoCell)
                        // Verificar si las celdas están vacías
                        var fechaLlegadaVacia = !fechaLlegadaCell || fechaLlegadaCell.textContent === "";
                        var horaLlegadaVacia = !horaLlegadaCell || horaLlegadaCell.textContent === "";

                        // Actualizar los valores de las celdas existentes o crear nuevos elementos
                        if (fechaLlegadaVacia && horaLlegadaVacia) {
                            // Crear un nuevo <td> para la fecha de llegada
                            fechaLlegadaCell = fila.insertCell(2);
                            fechaLlegadaCell.textContent = Fecha;
                        
                            // Crear un nuevo <td> para la hora de llegada
                            horaLlegadaCell = fila.insertCell(3);
                            horaLlegadaCell.textContent = Horallegada;

                            idTurnoCell = fila.insertCell(4);
                            idTurnoCell.textContent = IdTurno
                            idTurnoCell.style.display = "none";
                        }

                        // Actualizar los valores de los campos ocultos
                        $('#txtHorallegada').val(Horallegada);
                        $('#txtIdTurno').val(IdTurno);
                    }
                } else {
                    console.log("turnoDisponible", TurnoDisponible);
                    for (var i = 0; i < TurnoDisponible && filaIndex < numFilas; i++, filaIndex++) {
                        console.log("filaIndex", filaIndex);
                        console.log("i", i);

                        // Obtener los valores del turno
                        var IdTurno = document.getElementById('<%=txtIdTurno.ClientID %>').value;
                           var Fecha = document.getElementById('<%=txtFechaNuevoTurno.ClientID %>').value;
                        var Horallegada = document.getElementById('<%=txtHorallegada.ClientID %>').value;

                        // Obtener la fila correspondiente a la posición
                        var fila = tabla.rows[filaIndex];

                        // Crear un nuevo <td> para la fecha de llegada
                        var fechaLlegadaCell = fila.insertCell(2);
                        fechaLlegadaCell.textContent = Fecha;

                        // Crear un nuevo <td> para la hora de llegada
                        var horaLlegadaCell = fila.insertCell(3);
                        horaLlegadaCell.textContent = Horallegada;

                        var idTurnoCell = fila.insertCell(4);
                        idTurnoCell.textContent = IdTurno
                        idTurnoCell.style.display = "none";
                        // Actualizar los valores de los campos ocultos
                        $('#txtHorallegada').val(Horallegada);
                        $('#txtIdTurno').val(IdTurno);
                    }
                }

                // Recorrer la variable turnoDisponible y llenar los datos en la tabla

                console.log("tablaLength", tabla.rows.length);
                // Cerrar el modal
                $('#modalActu').modal('hide');
                if (tabla.rows.length > TurnoDisponible) {
                    mostrarAdvertencia("Turnos Disponibles para esta fecha igual a: " + TurnoDisponible + ". Por favor ingrese manualmente o vuelva a generar masivos para el sobrante que falta por ingresar en la tabla");
                }

            }


            else {
                // Código para el caso else (sin cambios)
                var IdTurno = document.getElementById('<%=txtIdTurno.ClientID %>').value;
                var Fecha = document.getElementById('<%=txtFechaNuevoTurno.ClientID %>').value;
                var Horallegada = document.getElementById('<%=txtHorallegada.ClientID %>').value;

                // Obtener la posición de la fila del botón
                var filaIndex = positionboton + 1;

                // Obtener la tabla por su id
                var tabla = document.getElementById('tablasort');

                // Obtener la fila correspondiente a la posición
                var fila = tabla.rows[filaIndex];

                // Verificar si las celdas de fecha y hora de llegada ya existen
                var fechaLlegadaCell = fila.cells[2];
                var horaLlegadaCell = fila.cells[3];
                var idTurnoLlegadaCell = fila.cells[4];
                // Actualizar los valores de las celdas existentes o crear nuevos elementos
                if (fechaLlegadaCell && horaLlegadaCell) {
                    fechaLlegadaCell.textContent = Fecha;
                    horaLlegadaCell.textContent = Horallegada;
                    idTurnoLlegadaCell.textContent = IdTurno
                    idTurnoLlegadaCell.style.display = "none";
                } else {
                    // Crear un nuevo <td> para la fecha de llegada
                    fechaLlegadaCell = document.createElement('td');
                    fechaLlegadaCell.textContent = Fecha;

                    // Crear un nuevo <td> para la hora de llegada
                    horaLlegadaCell = document.createElement('td');
                    horaLlegadaCell.textContent = Horallegada;

                    idTurnoLlegadaCell = document.createElement('td');
                    idTurnoLlegadaCell.textContent = IdTurno;
                    idTurnoLlegadaCell.style.display = "none";
                    // Insertar los nuevos <td> en la fila en las columnas correspondientes
                    fila.insertBefore(fechaLlegadaCell, fila.cells[2]);
                    fila.insertBefore(horaLlegadaCell, fila.cells[3]);
                    fila.insertBefore(idTurnoLlegadaCell, fila.cells[4]);
                }

                $('#txtHorallegada').val(Horallegada);
                $('#txtIdTurno').val(IdTurno);

                // Cerrar el modal
                $('#modalActu').modal('hide');
            }
        }



        $(document).ready(function () {
            BindFunctions();

            var t = document.getElementById('bandera');
            if (t.value != undefined && t.value != null && t.value.trim().length > 0) {
                $('span.validacion').text('');
            }

        });

        function sendform() {

            var t = document.getElementById('bandera').value;
            var lin = document.getElementById('linea_validar').value;


            if (t == undefined || t == null || t.trim().length <= 0) {
                alertify.alert('Informativo', 'Por favor seleccione el booking..!').set('label', 'Aceptar');
                return false;
            }
            var xvac = document.getElementById('vacio').checked;

            if (xvac) {
                if (t != 'MTY') {
                    alertify.alert('Informativo', 'El tipo de booking [LCL] no va acorde a la operación [Evacuación]').set('label', 'Aceptar');
                    return false;
                }
            }
            else {
                if (t != 'LCL' && lin != 'MSC') {
                    alertify.alert('Informativo', 'El tipo de booking [MTY] no va acorde a la operación [Consolidación]').set('label', 'Aceptar');
                    return false;
                }
            }
            if (!ValidateFile('fsupload')) {
                return false;
            }

            return true;
        }

        function getprocesa() {

            var tablaData = ObtenerDatosTabla();

            // Agrega los datos de la tabla a un campo oculto en el formulario
            var hiddenField = document.getElementById('<%= hiddenTablaData.ClientID %>');
            hiddenField.value = tablaData;


            var i = document.getElementById('procesar');
            if (i == undefined || i == null) {
                alertify.alert('Informativo', 'No se encontró el control asociado!!').set('label', 'Aceptar');
                return false;
            }
            i = i.value;
            if (i == '0') {
                alertify.alert('Informativo', 'Por favor realice todos los pasos antes de proceder:\n\t 1.Booking \n\t2.Operación\n\t3.Archivo Csv').set('label', 'Aceptar');
                return false;
            }
            if (confirm('Está seguro de generar los preavisos para las unidades en la lista?')) {
                alertify.alert('Informativo', 'En algunos minutos recibirá un mail confirmando la lista de unidades preavisadas y si existe algún contenedor con error').set('label', 'Aceptar');
                return true;
            }
            return false;
        }
        function prepareSessionVariable() {
            var datosTabla = ObtenerDatosTablaExportar();
            console.log("datostabla", datosTabla);

            $.ajax({
                url: 'vacios.aspx/EnviarDatosTabla',  // Ruta a tu método en el servidor
                type: 'POST',
                data: JSON.stringify({ datosTabla: datosTabla}) ,
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                success: function (response) {
                
                },
                error: function (xhr, status, error) {
                    console.log('Error al enviar los datos de la tabla: ' + error);
                }
            });
        }



        function ObtenerDatosTablaExportar() {
            var table = document.getElementById('tablasort');
            var rows = table.getElementsByTagName('tr');
            var data = [];

            for (var i = 0; i < rows.length; i++) {
                var cells = rows[i].getElementsByTagName('td');

                if (cells.length > 0 && cells[0].innerText !== "") {
                    var rowData = {
                        contenedor: cells[0].innerText,
                        fecha: cells[2].innerText,
                        hora: cells[3].innerText,
                        idTurno: cells[4].innerText
                    };

                    data.push(rowData);
                }
            }

            return JSON.stringify(data);
        }
        function ObtenerDatosTabla() {
            var table = document.getElementById('tablasort'); 
            var rows = table.getElementsByTagName('tr');
            var data = [];

            for (var i = 0; i < rows.length; i++) {
                var cells = rows[i].getElementsByTagName('td');
                var rowData = [];

                for (var j = 0; j < cells.length; j++) {
                    rowData.push(cells[j].innerText);
                }

                data.push(rowData);
            }

        
            var jsonData = JSON.stringify(data);

            return jsonData;
        }
        function onBook() {
            tipo = 'MTY';
            if (document.getElementById('consolida').checked) {
                tipo = 'LCL';
            }
            var w = window.open('../catalogo/bookingmas.aspx?tipo=' + tipo + '&v=1', 'Bookings', 'width=850,height=880');
            w.focus();
        }
        function validateBook(objeto) {
            //stringnifiobjeto
            var bokIt = {
                number: objeto.numero,
                linea: objeto.bline,
                referencia: objeto.referencia,
                gkey: objeto.gkey,
                pod: objeto.pod,
                pod1: objeto.pod1,
                shiperID: objeto.shipid,
                temp: objeto.temp,
                fkind: objeto.fk,
                imo: objeto.imo,
                refer: objeto.refer,
                dispone: objeto.dispone,
                iso: objeto.aqt,
                cutOff: objeto.cutoff,
                temp: objeto.temp,
                hume: objeto.hume,
                vent_pc: objeto.vent_pc,
                ventu: objeto.ventu,
                gkey: objeto.gkey
            };
            document.getElementById('refnumber').textContent = objeto.numero + '/' + objeto.referencia;
            document.getElementById('itemT4').value = JSON.stringify(bokIt);
            document.getElementById('bandera').value = objeto.fk;
            document.getElementById('linea_validar').value = objeto.bline;
            document.getElementById('txtISO').value = objeto.iso;
            alertify.alert('Informativo', 'Se le comunica que la disponibilidad máxima unidades del item de booking elegido es: ' + objeto.dispone).set('label', 'Aceptar');
        }
        $('form').live("submit", function () { ShowProgress(); });


    </script>

   <script src="../Scripts/jquery.datetimepicker.js" type="text/javascript"></script>


 <script type="text/javascript">
     $(document).ready(function () {
         $('.datetimepickerAlt').datetimepicker().datetimepicker({ lang: 'es', closeOnDateSelect: true, timepicker: false, step: 30, format: 'd/m/Y' });
     });


 </script>


 <div class="loading" align="center">
    Estamos verificando toda la información 
    que nos facilitó,por favor espere unos segundos<br />
    <img src="../shared/imgs/loader.gif" alt="x" />
</div>
</asp:Content>