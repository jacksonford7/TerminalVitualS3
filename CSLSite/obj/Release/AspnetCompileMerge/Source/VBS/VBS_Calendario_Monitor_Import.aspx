<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VBS_Calendario_Monitor_Import.aspx.cs" Inherits="CSLSite.VBS_Calendario_Monitor_Import" %>

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
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
    <link href="https://getbootstrap.com/docs/5.3/assets/css/docs.css" rel="stylesheet" />

    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js"></script>
    <script src="https://stackpath.bootstrapcdn.com/bootstrap/4.3.1/js/bootstrap.min.js"></script>
    <link rel="stylesheet" href="https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/css/bootstrap.min.css" />
    <style type="text/css">
        .event-title {
            font-size: 1px; /* Tamaño de fuente ajustable */
        }
       
        .scroll-container {
            overflow-y: scroll;
            height: calc(100vh - 20px); /* Altura del contenedor: 100% del viewport - 20px de margen */
        }

        .selected-time {
            background-color: black; /* Color de fondo del rango seleccionado */
            opacity: 0.3; /* Opacidad del color de fondo */
        }

        body {
            margin: 0;
            padding: 0;
        }

        .container-fluid,
        .row,
        .col-md-12 {
            height: 100vh;
            margin-bottom: -50px;
        }

        .card {
            height: 85%;
        }

        .card-body {
            display: flex;
            flex-direction: column;
            justify-content: center;
            align-items: center;
            padding-top: 0px;
            padding-right: 0px;
            padding-left: 0px;
            padding-bottom: 0px;
        }

        .header {
            text-align: center;
            padding-bottom: 0px;
            font-size: 30px;
            font-weight: bold;
        }

        .clock {
            text-align: center;
            font-size: 30px;
            padding-bottom: 10px;
        }

        th {
            padding: 8px;
            height: 1px;
            font-size: 13px;
        }

        td {
            padding: 8px;
            text-align: left;
            font-size: 11px; /* Tamaño de fuente para las celdas */
            height: 1px; /* Altura de las filas */
        }

    </style>
    <title></title>
</head>
<body onload="buscarTablasImport()">
    <div class="header">Citas Importación</div>
    <div class="clock">
        <script type="text/javascript">
            function updateClock() {
                var now = new Date();
                var hours = now.getHours();
                var minutes = now.getMinutes();
                var seconds = now.getSeconds();

                var timeString =
                    ("0" + hours).slice(-2) +
                    ":" +
                    ("0" + minutes).slice(-2) +
                    ":" +
                    ("0" + seconds).slice(-2);

                document.getElementById("clock").textContent = timeString;
            }

            setInterval(updateClock, 1000);
        </script>

        <span id="clock"></span>
    </div>

    <div class="container-fluid" >
        <div class="row">
            <div class="col-md-12">
                <div class="card">
                    <div id="cadr-body1" class="card-body" style="padding-top: 0px">
                        <!-- Tabla 1 -->
                        <div class="table-responsive">
                            <table class="table">
                                <thead id="Thead1">
                                    <tr>
                                        <th colspan="1"></th>
                                        <th id="horaInicio1" style="text-align: center"></th>
                                        <th id="horaInicio2"></th>
                                        <th id="horaInicio3"></th>
                                        <th id="horaInicio4" style="text-align: center"></th>
                                        <th id="horaInicio5"></th>
                                        <th id="horaInicio6"></th>
                                        <th id="horaInicio7" style="text-align: center"></th>
                                        <th id="horaInicio8"></th>
                                        <th id="horaInicio9"></th>
                                        <th id="horaInicio10" style="text-align: center"></th>
                                        <th id="horaInicio11"></th>
                                        <th id="horaInicio12"></th>
                                        <th id="horaInicio13" style="text-align: center"></th>
                                        <th id="horaInicio14"></th>
                                        <th id="horaInicio15"></th>
                                        <th id="horaInicio16" style="text-align: center"></th>
                                        <th id="horaInicio17"></th>
                                        <th id="horaInicio18"></th>
                                    </tr>
                                    <tr>
                                        <th style="background-color: orangered; color: white">Tipo De bloque</th>
                                        <th style="text-align: center; background-color: orangered; color: white">P</th> 
                                        <th style="text-align: center; background-color: orangered; color: white">R</th>
                                        <th style="text-align: center; background-color: orangered; color: white">D</th>
                                        <th style="text-align: center; background-color: orangered; color: white">P</th>
                                        <th style="text-align: center; background-color: orangered; color: white">R</th>
                                        <th style="text-align: center; background-color: orangered; color: white">D</th>
                                        <th style="text-align: center; background-color: orangered; color: white">P</th>
                                        <th style="text-align: center; background-color: orangered; color: white">R</th>
                                        <th style="text-align: center; background-color: orangered; color: white">D</th>
                                        <th style="text-align: center; background-color: orangered; color: white">P</th>
                                        <th style="text-align: center; background-color: orangered; color: white">R</th>
                                        <th style="text-align: center; background-color: orangered; color: white">D</th>
                                        <th style="text-align: center; background-color: orangered; color: white">P</th>
                                        <th style="text-align: center; background-color: orangered; color: white">R</th>
                                        <th style="text-align: center; background-color: orangered; color: white">D</th>
                                        <th style="text-align: center; background-color: orangered; color: white">P</th>
                                        <th style="text-align: center; background-color: orangered; color: white">R</th>
                                        <th style="text-align: center; background-color: orangered; color: white">D</th>
                                    </tr>
                                </thead>
                                <tbody id="tbody1">
                                </tbody>
                            </table>
                        </div>

                      
                    </div>
                </div>
            </div>
        </div>
    </div>

    <script type="text/javascript">
        function buscarTablasImport() {

            $.ajax({
                url: "VBS_Calendario_Monitor_Import.aspx/getTurnosMonitorImport",
                data: JSON.stringify({}),
                contentType: "application/json; charset=utf-8",
                type: 'POST',
                success: function (response) {
                    // La respuesta exitosa del servidor
                    // Aquí puedes procesar la respuesta y generar la tabla
                
                    var tableHTML = [];
                    if (response && response.d) {
                        var data = JSON.parse(response.d);

                        // Llenar Tabla 1
                        var tabla1Data = data.Tabla1;

                        var Thead2 = document.getElementById('Thead2');

                        var Thead1 = document.getElementById('Thead1');
                        var thElement1 = Thead1.getElementsByTagName('th');

                        // Obtener el cuerpo de la tabla 1
                        var tbody1 = document.getElementById('tbody1');
                        if (tbody1) {
                            // Limpiar el contenido existente en el cuerpo de la tabla 1
                            tbody1.innerHTML = '';

                            // Iterar sobre los registros de Tabla 1
                            for (var i = 0; i < tabla1Data.length; i++) {

                                var Thead1 = document.getElementById('Thead1');
                                var thElement1 = Thead1.getElementsByTagName('th');

                                // Rango de horas 1-3
                                thElement1[1].innerHTML = ' (' + tabla1Data[i].Inicio1 + ' - ' + tabla1Data[i].Fin1 + ')';
                                thElement1[1].colSpan = 3;
                                thElement1[2].style.textAlign = 'center';
                                thElement1[2].style.display = 'none';
                                thElement1[3].style.display = 'none';

                                // Rango de horas 4-6
                                thElement1[4].innerHTML = '(' + tabla1Data[i].Inicio2 + ' - ' + tabla1Data[i].Fin2 + ')';
                                thElement1[4].colSpan = 3;
                                thElement1[4].style.textAlign = 'center';
                                thElement1[5].style.display = 'none';
                                thElement1[6].style.display = 'none';

                                // Rango de horas 7-9
                                thElement1[7].innerHTML = '(' + tabla1Data[i].Inicio3 + ' - ' + tabla1Data[i].Fin3 + ')';
                                thElement1[7].colSpan = 3;
                                thElement1[7].style.textAlign = 'center';
                                thElement1[8].style.display = 'none';
                                thElement1[9].style.display = 'none';

                                // Rango de horas 10-12
                                thElement1[10].innerHTML = '(' + tabla1Data[i].Inicio4 + ' - ' + tabla1Data[i].Fin4 + ')';
                                thElement1[10].colSpan = 3;
                                thElement1[10].style.textAlign = 'center';
                                thElement1[11].style.display = 'none';
                                thElement1[12].style.display = 'none';

                                // Rango de horas 10-12
                                thElement1[13].innerHTML = '(' + tabla1Data[i].Inicio5 + ' - ' + tabla1Data[i].Fin5 + ')';
                                thElement1[13].colSpan = 3;
                                thElement1[13].style.textAlign = 'center';
                                thElement1[14].style.display = 'none';
                                thElement1[15].style.display = 'none';


                                // Rango de horas 10-12
                                thElement1[16].innerHTML = '(' + tabla1Data[i].Inicio6 + ' - ' + tabla1Data[i].Fin6 + ')';
                                thElement1[16].colSpan = 3;
                                thElement1[16].style.textAlign = 'center';
                                thElement1[17].style.display = 'none';
                                thElement1[18].style.display = 'none';


                                var CodigoBloque = tabla1Data[i].CodigoBloque
                                var planeado1 = tabla1Data[i].Planeado1;           //1
                                var reservado1 = tabla1Data[i].Reservado1;         //2
                                var disponible1 = tabla1Data[i].Disponible1;       //3
                                var planeado2 = tabla1Data[i].Planeado2;           //4
                                var reservado2 = tabla1Data[i].Reservado2;         //5
                                var disponible2 = tabla1Data[i].Disponible2;       //6
                                var planeado3 = tabla1Data[i].Planeado3;           //7
                                var reservado3 = tabla1Data[i].Reservado3;         //8
                                var disponible3 = tabla1Data[i].Disponible3;       //9
                                var planeado4 = tabla1Data[i].Planeado4;           //10
                                var reservado4 = tabla1Data[i].Reservado4;         //11
                                var disponible4 = tabla1Data[i].Disponible4;       //12
                                var planeado5 = tabla1Data[i].Planeado5;           //13
                                var reservado5 = tabla1Data[i].Reservado5;         //14
                                var disponible5 = tabla1Data[i].Disponible5;       //15
                                var planeado6 = tabla1Data[i].Planeado6;           //16
                                var reservado6 = tabla1Data[i].Reservado6;         //17
                                var disponible6 = tabla1Data[i].Disponible6;       //18

                                // Crea una nueva fila en el cuerpo de la tabla 1
                                var rowHTML = '<tr>'; 
                                rowHTML += '<td>' + CodigoBloque + '</td>';
                                rowHTML += '<td style="background-color: #59B653; text-align: center; ">' + planeado1 + '</td>';   //1
                                rowHTML += '<td style="background-color: #5AABD9; text-align: center; ">' + reservado1 + '</td>';  //2
                                rowHTML += '<td style="background-color: #D9DB63; text-align: center; ">' + disponible1 + '</td>'; //3
                                rowHTML += '<td style="background-color: #59B653; text-align: center; ">' + planeado2 + '</td>';   //4
                                rowHTML += '<td style="background-color: #5AABD9; text-align: center; ">' + reservado2 + '</td>';  //5
                                rowHTML += '<td style="background-color: #D9DB63; text-align: center; ">' + disponible2 + '</td>'; //6
                                rowHTML += '<td style="background-color: #59B653; text-align: center; ">' + planeado3 + '</td>';   //7
                                rowHTML += '<td style="background-color: #5AABD9; text-align: center; ">' + reservado3 + '</td>';  //8
                                rowHTML += '<td style="background-color: #D9DB63; text-align: center; ">' + disponible3 + '</td>'; //9
                                rowHTML += '<td style="background-color: #59B653; text-align: center; ">' + planeado4 + '</td>';   //10  
                                rowHTML += '<td style="background-color: #5AABD9; text-align: center; ">' + reservado4 + '</td>';  //11
                                rowHTML += '<td style="background-color: #D9DB63; text-align: center; ">' + disponible4 + '</td>'; //12
                                rowHTML += '<td style="background-color: #59B653; text-align: center; ">' + planeado5 + '</td>';   //13
                                rowHTML += '<td style="background-color: #5AABD9; text-align: center; ">' + reservado5 + '</td>';  //14
                                rowHTML += '<td style="background-color: #D9DB63; text-align: center; ">' + disponible5 + '</td>'; //15
                                rowHTML += '<td style="background-color: #59B653; text-align: center; ">' + planeado6 + '</td>';   //16
                                rowHTML += '<td style="background-color: #5AABD9; text-align: center; ">' + reservado6 + '</td>';  //17
                                rowHTML += '<td style="background-color: #D9DB63; text-align: center; ">' + disponible6 + '</td>'; //18
                                rowHTML += '</tr>'; 

                                // Agrega la fila al cuerpo de la tabla 1
                                tbody1.innerHTML += rowHTML;
                            }
                        }

                   
                    }

                },

                error: function (error) {
                    // El manejo de errores en caso de que la solicitud falle
                    console.log('Error:', error);
                }
            });
        }





        function getFormattedHour(hour) {
            return (hour < 10 ? "0" : "") + hour + ":00";
        }


        function recargarPagina() {
            setTimeout(function () {
                location.reload();
            }, 120000); // 120000 milisegundos = 2 minutos  //10000 milisegundos = 10 segundos
        }
        recargarPagina();
    </script>

</body>
</html>
