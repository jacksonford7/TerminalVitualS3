////window.onload = function () {


////    function validarCantidad() {
////        var cantidadModificada = parseInt(input.val());

////        if (cantidadModificada >= 0) {
////            if (cantidadInicial == 0) {
////                if (cantidadModificada > 0) {
////                    fila.find('.lblDisponible').text(cantidadModificada);
////                } else {
////                    fila.find('.lblDisponible').text(0);
////                }
////            } else {
////                if (disponible <= cantidadInicial) {
////                    if (cantidadModificada < Asignados) {
////                        mostrarAdvertencia("El valor ingresado no puede ser menor a los Turnos Asignados");
////                        input.val(cantidadInicial);
////                    } else {
////                        var diferencia = cantidadModificada - cantidadInicial;
////                        var nuevoDisponible = disponible + diferencia;
////                        fila.find('.lblDisponible').text(Math.max(nuevoDisponible, 0));
////                    }
////                }
////            }
////        } else {
////            mostrarAdvertencia("No se permiten valores negativos");
////            input.val(cantidadInicial);
////        }
////    }

////    function abrirCalendarioPantallaCompleta_DOS() {
////        // Abrir una nueva ventana con el calendario
////        var nuevaVentana = window.open('VBS_Calendario_Dias_DOS.aspx', '_blank', 'fullscreen=yes');

////        // Cambiar la nueva ventana a pantalla completa
////        if (nuevaVentana) {
////            if (nuevaVentana.document.documentElement.requestFullscreen) {
////                nuevaVentana.document.documentElement.requestFullscreen();
////            } else if (nuevaVentana.document.documentElement.mozRequestFullScreen) {
////                nuevaVentana.document.documentElement.mozRequestFullScreen();
////            } else if (nuevaVentana.document.documentElement.webkitRequestFullscreen) {
////                nuevaVentana.document.documentElement.webkitRequestFullscreen();
////            } else if (nuevaVentana.document.documentElement.msRequestFullscreen) {
////                nuevaVentana.document.documentElement.msRequestFullscreen();
////            }
////        }
////        nuevaVentana.focus();
////    }


////    function abrirCalendarioPantallaCompleta() {
////        // Abrir una nueva ventana con el calendario
////        var nuevaVentana = window.open('VBS_Calendario_Dias.aspx', '_blank', 'fullscreen=yes');

////        // Cambiar la nueva ventana a pantalla completa
////        if (nuevaVentana) {
////            if (nuevaVentana.document.documentElement.requestFullscreen) {
////                nuevaVentana.document.documentElement.requestFullscreen();
////            } else if (nuevaVentana.document.documentElement.mozRequestFullScreen) {
////                nuevaVentana.document.documentElement.mozRequestFullScreen();
////            } else if (nuevaVentana.document.documentElement.webkitRequestFullscreen) {
////                nuevaVentana.document.documentElement.webkitRequestFullscreen();
////            } else if (nuevaVentana.document.documentElement.msRequestFullscreen) {
////                nuevaVentana.document.documentElement.msRequestFullscreen();
////            }
////        }
////        nuevaVentana.focus();
////    }


////    function reload() {
////        location.reload(true);
////    }
////    var secuencia = 1; // Variable para generar la secuencia de forma incremental

////    function agregarDetalle() {
////        // Obtener los valores del detalle a través de los controles en la interfaz de usuario
////        alert("sda");
////        console.log("document");
////        var cboTipoCarga = document.getElementById("<%=cboTipoCarga.ClientID %>");
////        console.log("cboTipoCarga", cboTipoCarga);
////        var tipoCargas = cboTipoCarga.options[cboTipoCarga.selectedIndex].text;
////        var tipoCargaId = cboTipoCarga.value;

////        var cboTipoContenedor = document.getElementById("cboTipoContenedor");
////        var tipoContenedor = cboTipoContenedor.options[cboTipoContenedor.selectedIndex].text;
////        var tipoContenedorId = cboTipoContenedor.value;

////        var cantidad = document.getElementById('<%=TxtCantidad.ClientID %>').value;
////        if (cantidad === '') {
////            mostrarError('El campo CANTIDAD es requerido');
////            return false;
////        }

////        // Verificar si ya se ha ingresado un detalle con los mismos valores de tipoContenedorId y tipoCargaId
////        var detallesExistentes = document.getElementById('tbodyDetalles').getElementsByTagName('tr');
////        for (var i = 0; i < detallesExistentes.length; i++) {
////            var detalle = detallesExistentes[i];
////            var detalleTipoContenedorId = detalle.getAttribute('data-tipo-contenedor-id');
////            var detalleTipoCargaId = detalle.getAttribute('data-tipo-carga-id');
////            if (detalleTipoContenedorId === tipoContenedorId && detalleTipoCargaId === tipoCargaId) {
////                mostrarAdvertencia('No se pueden ingresar registros repetidos');
////                return false;
////            }
////        }

////        // Crear una nueva fila con los valores del detalle
////        var fila = document.createElement('tr');
////        fila.innerHTML = `
////        <td class="center hidden-phone">${secuencia}</td>
////         <td class="center hidden-phone">${tipoCargas}</td>
////        <td class="center hidden-phone">${tipoContenedor}</td>
       
////        <td class="center hidden-phone">${cantidad}</td>
////        <td class="center hidden-phone">
////            <button class="btn btn-primary" onclick="eliminarDetalle(this)">QUITAR</button>
////        </td>
////    `;

////        // Agregar los datos del detalle como atributos personalizados a la fila
////        fila.setAttribute('data-tipo-contenedor-id', tipoContenedorId);
////        fila.setAttribute('data-tipo-carga-id', tipoCargaId);

////        // Agregar la fila al cuerpo de la tabla
////        document.getElementById('tbodyDetalles').appendChild(fila);

////        // Incrementar la secuencia para el siguiente detalle
////        secuencia++;

////        return false;
////    }

////    function enviarDatosAlServidor() {
////        // Obtener los datos de la tabla
////        var detalles = [];
////        var tabla = document.getElementById('tablaDetalles');
////        var filas = tabla.getElementsByTagName('tr');
////        var tieneTipoContenedorTodos = false;
////        var banderaDayClick = document.getElementById('<%=banderaDayClick.ClientID %>').value
////        for (var i = 0; i < filas.length; i++) {
////            var fila = filas[i];
////            var celdas = fila.getElementsByTagName('td');

////            // Verificar si se encontraron celdas en la fila actual
////            if (celdas.length > 0) {
////                var secuencia = celdas[0].innerText;
////                var tipoCargas = celdas[1].innerText;
////                var tipo_Contenedor = celdas[2].innerText;
////                var cantidad = celdas[3].innerText;

////                // Obtener los ID de tipoContenedor y tipoCarga de los atributos personalizados de la fila
////                var tipoContenedorId = fila.getAttribute('data-tipo-contenedor-id');
////                var tipoCargaId = fila.getAttribute('data-tipo-carga-id');

////                var detalle = {
////                    secuencia: secuencia,
////                    tipoContenedorId: tipoContenedorId,
////                    tipoContenedor: tipo_Contenedor,
////                    tipoCargaId: tipoCargaId,
////                    tipoCargas: tipoCargas,
////                    cantidad: cantidad
////                };

////                detalles.push(detalle);
////                // Verificar si el tipo de contenedor es 'Todos'
////                if (tipo_Contenedor === 'TODOS') {
////                    tieneTipoContenedorTodos = true;
////                }
////            }
////        }

////        // Verificar si no se encontró ningún tipo de contenedor igual a 'Todos'
////        if (!tieneTipoContenedorTodos) {
////            mostrarAdvertencia('Debe ingresar al menos un registro para el tipo de contenedor "Todos"');
////            return;
////        }

////        var vigenciaInicial = document.getElementById('<%=txtVigenciaInicial.ClientID %>').value;
////        var vigenciaFinal = document.getElementById('<%=txtVigenciaFinal.ClientID %>').value;

////        if (detalles.length === 0) {
////            mostrarAdvertencia('No hay detalles para enviar');
////            return;
////        }
////        mostrarConfirmacion('¿Está seguro de generar los turnos?', function () {
////            $('#loader').show();
////            $.ajax({
////                url: "VBS_Calendario_Edit.aspx/GuardarDatosTabla1",
////                type: 'POST',
////                data: JSON.stringify({
////                    vigenciaInicial: vigenciaInicial,
////                    vigenciaFinal: vigenciaFinal,
////                    banderaDayClick: banderaDayClick,
////                    detalles: detalles
////                }), // Enviar los detalles como un objeto en lugar de una cadena JSON
////                contentType: 'application/json; charset=utf-8',
////                dataType: 'json',
////                success: function (data) {
////                    mostrarExito('Datos guardados exitosamente', function () {
////                        var modal = document.getElementById('modalt');
////                        modal.style.display = 'none';
////                        location.reload();
////                    });
////                },
////                error: function (error) {
////                    // Manejar el error de la solicitud AJAX
////                    var modal = document.getElementById('modalt');
////                    mostrarError('Error al guardar los datos');
////                    modal.style.display = 'none';
////                    location.reload();

////                },

////                complete: function () {
////                    // Ocultar el indicador de carga al finalizar la solicitud AJAX
////                    $('#loader').hide();
////                }
////            });
////        })

////    }

////    function LlenarTabla(eventos) {
////        var cuerpoTabla = $("#cuerpoTabla");
////        cuerpoTabla.empty();

////        // Iterar sobre los eventos y crear las filas de la tabla
////        $.each(eventos, function (index, evento) {
////            var secuencia = index + 1; // Obtener la secuencia sumando 1 al índice

////            var fila = $("<tr>");
////            fila.attr("data-idTurno", evento.IdTurno);
////            document.getElementById('<%=txtTipoCargas.ClientID %>').value = evento.TipoCargas;

////            document.getElementById('<%=txtTipoContenedor.ClientID %>').value = evento.TipoContenedor;
////            fila.append("<td class='center hidden-phone'>" + secuencia + "</td>");
////            fila.append("<td class='center hidden-phone'>" + evento.Horario + "</td>");

////            // Crear un input editable para la cantidad
////            var inputCantidad = $("<input type='text' class='form-control cantidad-input'>").val(evento.Cantidad);
////            fila.append($("<td class='center hidden-phone'>").append(inputCantidad));

////            fila.append("<td class='center hidden-phone lblDisponible'>" + evento.Disponible + "</td>");
////            fila.append("<td class='center hidden-phone lblAsignados'>" + evento.Asignados + "</td>");

////            cuerpoTabla.append(fila);
////        });

////    }




////    function guardarCambiosTabla() {
////        var eventos = [];

////        // Obtener los datos de la tabla
////        $("#cuerpoTabla tr").each(function () {
////            var fila = $(this);
////            var idTurno = fila.attr("data-idTurno");
////            var cantidad = fila.find(".cantidad-input").val().trim();
////            var disponible = fila.find(".lblDisponible").text().trim();
////            var secuencia = fila.find("td:nth-child(1)").text().trim();

////            // Crear un objeto con los datos del evento
////            var evento = {
////                secuencia: secuencia,
////                cantidad: cantidad,
////                idTurno: idTurno,
////                disponible: disponible
////            };

////            // Agregar el evento al arreglo
////            eventos.push(evento);
////        });

////        mostrarConfirmacion("¿Deseas guardar los cambios en la tabla?", function () {
////            $.ajax({
////                type: "POST",
////                url: "VBS_Calendario_Edit.aspx/GuardarDatos",
////                data: JSON.stringify({ datosTabla: eventos }),
////                contentType: "application/json; charset=utf-8",
////                dataType: "json",
////                success: function (response) {

////                    if (response.d === "success") {

////                        mostrarExito("Cambios guardados correctamente.", function () {
////                            $("#modalActu").modal("hide");
////                            location.reload(true);

////                        })
////                    }
////                    else {
////                        mostrarError("Error al guardar los cambios.")

////                    }
////                },
////                error: function (xhr, ajaxOptions, thrownError) {
////                    mostrarError("Error en la solicitud al servidor.");
////                }
////            });
////        });

////    }

////    function filtrarTabla() {
////        // Obtener el valor de búsqueda del input de búsqueda
////        var filtro = document.getElementById('inputBusqueda').value.toUpperCase();
////        // Obtener la tabla y las filas de la tabla
////        var tabla = document.getElementById('cuerpoTabla');

////        var filas = tabla.getElementsByTagName('tr');

////        // Recorrer las filas de la tabla y mostrar u ocultar según el filtro
////        for (var i = 0; i < filas.length; i++) {
////            var celdas = filas[i].getElementsByTagName('td');

////            var mostrarFila = false;

////            for (var j = 0; j < celdas.length; j++) {
////                var contenidoCelda = celdas[j].textContent || celdas[j].innerText;

////                if (contenidoCelda.toUpperCase().indexOf(filtro) > -1) {
////                    mostrarFila = true;
////                    break;
////                }
////            }

////            filas[i].style.display = mostrarFila ? '' : 'none';
////        }
////    }
////    function cboTipoCargaChanged() {
////        var cboTipoCarga = document.getElementById("<%= cboTipoCarga.ClientID %>");
////        var tipoCargaId = cboTipoCarga.value;

////        // Llamar al servidor utilizando AJAX
////        var xhr = new XMLHttpRequest();
////        xhr.open("GET", "VBS_Calendario_Edit.aspx?tipoCargaId=" + tipoCargaId, true);
////        xhr.onreadystatechange = function () {
////            if (xhr.readyState === 4 && xhr.status === 200) {
////                // La respuesta del servidor ha sido recibida correctamente
////                var cboTipoContenedor = document.getElementById("<%= cboTipoContenedor.ClientID %>");
////                cboTipoContenedor.innerHTML = "";

////                // Agregar las nuevas opciones al DropDownList
////                cboTipoContenedor.innerHTML = xhr.responseText;

////            }
////        };
////        xhr.send();
////    }

////    function eliminarDetalle(button) {
////        // Obtener la fila padre del botón
////        var fila = button.parentNode.parentNode;

////        // Eliminar la fila de la tabla
////        fila.remove();
////    }



////    function buscarBooking() {
////        var fecha = document.getElementById('<%=txtFechaDesde.ClientID %>').value;

////        // Realizar solicitud Ajax
////        $.ajax({
////            url: "VBS_Calendario_Edit.aspx/ConsultarBookingCalendario",
////            data: JSON.stringify({ fecha: fecha }),
////            contentType: "application/json; charset=utf-8",
////            type: 'POST',
////            success: function (response) {
////                var tableHTML = '';

////                // Verifica si la respuesta contiene datos
////                if (response && response.d) {
////                    var data = JSON.parse(response.d);

////                    // Generar la tabla a partir de los datos
////                    for (var i = 0; i < data.length; i++) {
////                        tableHTML += '<tr>';
////                        tableHTML += '<td>' + data[i].description + '</td>';
////                        tableHTML += '<td>' + data[i].reference + '</td>';
////                        tableHTML += '<td>' + data[i].ETA + '</td>';
////                        tableHTML += '<td>' + data[i].qty + '</td>';
////                        tableHTML += '</tr>';
////                    }
////                }

////                var tbodyBooking = document.getElementById('tbodyBooking');
////                console.log("tbody", tbodyBooking);
////                tbodyBooking.innerHTML = tableHTML;

////                // Mostrar la tabla cambiando su estilo a display: table-row-group
////                var tablaDetalles = document.getElementById('tablaDetalles2');
////                tablaDetalles.style.display = 'table';
////            },
////            error: function (error) {

////            }
////        });
////    }




////    // Función para mostrar un mensaje de confirmación
////    function mostrarConfirmacion(mensaje, callback) {
////        Swal.fire({
////            title: "Confirmación",
////            text: mensaje,
////            icon: "question",
////            iconColor: "#E23B1B",
////            showCancelButton: true,
////            confirmButtonText: "Sí",
////            confirmButtonColor: "#E23B1B",
////            cancelButtonText: "No",
////            cancelButtonColor: "#E23B1B"
////        }).then((result) => {
////            if (result.isConfirmed) {
////                callback();
////            }
////        });
////    }

////    // Función para mostrar un mensaje de advertencia
////    function mostrarAdvertencia(mensaje) {
////        Swal.fire({
////            title: "Advertencia",
////            text: mensaje,
////            icon: "warning",
////            iconColor: "#E23B1B",
////            confirmButtonText: "Aceptar",
////            confirmButtonColor: "#E23B1B"
////        });
////    }

////    // Función para mostrar un mensaje de error
////    function mostrarError(mensaje) {
////        Swal.fire({
////            title: "Error",
////            text: mensaje,
////            icon: "error",
////            iconColor: "#E23B1B",
////            confirmButtonText: "Aceptar",
////            confirmButtonColor: "#E23B1B"
////        });
////    }

////    // Función para mostrar un mensaje de éxito
////    function mostrarExito(mensaje, callback) {
////        Swal.fire({
////            title: "Éxito",
////            text: mensaje,
////            icon: "success",
////            iconColor: "#E23B1B",
////            confirmButtonText: "Aceptar",
////            confirmButtonColor: "#E23B1B"
////        }).then((result) => {
////            if (result.isConfirmed) {
////                callback();
////            }
////        });
////    }

////};


