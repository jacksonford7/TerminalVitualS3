 <%@ Page Title="" Language="C#" MasterPageFile="~/site.Master" AutoEventWireup="true" CodeBehind="VBS_Container_Mantenimiento.aspx.cs" Inherits="CSLSite.VBS_Container_Mantenimiento" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="content1" ContentPlaceHolderID="placehead" runat="server">
    <link href="../img/favicon2.png" rel="icon" />
    <link href="../img/icono.png" rel="apple-touch-icon" />
    <!-- Bootstrap core CSS -->
    <link href="../css/bootstrap.min.css" rel="stylesheet" />
    <link href="../css/dashboard.css" rel="stylesheet" />
    <link href="../css/icons.css" rel="stylesheet" />
    <link href="../css/style.css" rel="stylesheet" />

    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/5.15.3/css/all.min.css"/>

        <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/sweetalert2@11.0.19/dist/sweetalert2.min.css" />
    <script type="text/javascript" src='https://maps.google.com/maps/api/js?key=AIzaSyA0f3IQRMX1fmn-35UxyLJSDvKv3BbKBhI&sensor=false&libraries=places'></script>
    <script type="text/javascript" src="../maps/locationpicker.jquery.js"></script>
    <script type="text/javascript" src="../js/Confirmaciones.js""></script>
    <link rel="stylesheet" href="https://cdn.datatables.net/1.10.25/css/jquery.dataTables.min.css"/>
<script type="text/javascript" src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
<script type="text/javascript" src="https://cdn.datatables.net/1.10.25/js/jquery.dataTables.min.js"></script>

</asp:Content>


<asp:Content ID="content2" ContentPlaceHolderID="placebody" runat="server">
       <script type="text/javascript" src="https://cdn.jsdelivr.net/npm/sweetalert2@11.0.19/dist/sweetalert2.all.min.js"></script>
  

       <script type="text/javascript" src="../js/Confirmaciones.js""></script>
    <asp:ToolkitScriptManager ID="tkscata" runat="server"></asp:ToolkitScriptManager>

    <asp:HiddenField ID="manualHide" runat="server" />
    <div id="div_BrowserWindowName" style="visibility: hidden;">
        <asp:HiddenField ID="hf_BrowserWindowName" runat="server" />

    </div>
    <div class="mt-4">
        <nav class="mt-4" aria-label="breadcrumb">
            <ol class="breadcrumb">
                <li class="breadcrumb-item" id="opcion_principal" runat="server"><a href="#">MANTENIMIENTOS</a></li>
                <li class="breadcrumb-item active" aria-current="page" id="sub_opcion" runat="server">CONTENEDORES SIN TURNO</li>
            </ol>
        </nav>
    </div>

    <div class="container-fluid">

        <div class="row">
            <div class="col-md-12">
                <div class="card">
                    <div class="card-body">
                        <div id="card-title-1">
                            <h5 class="card-title" id="title-1" style="text-align: center">CONTENEDORES</h5>
                        </div>
                        <div class="row">
                            <div class="col-md-1">
                                <button class="btn btn-primary" onclick="levantarmodal()" type="button">NUEVO</button>

                            </div>
                            <br />
                              <div class="form-group col-md-3">
                            <input type="text" id="inputBusqueda" style="align-content: flex-start" class="form-control" onkeyup="filtrarTabla()" placeholder="Buscar..." />

                        </div>
                        </div>

                        <div class="table-responsive" id="tableBloque">
                            <table class="table">
                                <thead>
                                    <tr>
                                        <th style="background-color: #E23B1B; color: white">Codigo </th>
                                        <th style="background-color: #E23B1B; color: white">Contenedor</th>
                                        <th style="background-color: #E23B1B; color: white">Descripción</th>
                                        <th style="text-align: center; background-color: #E23B1B; color: white">Usuario</th>
                                        <th style="text-align: center; background-color: #E23B1B; color: white">Fecha</th>
                                        <th style="text-align: center; background-color: #E23B1B; color: white">Accion</th>
                                    </tr>
                                </thead>
                                <tbody id="tbodyBloque">
                                </tbody>
                            </table>
                        </div>


                    </div>
                </div>
            </div>
        </div>
    </div>

    <!-- Modal -->
    <div class="modal fade" id="modalt" tabindex="-1" role="dialog" aria-labelledby="modal-event-label" aria-hidden="true" data-backdrop="static" data-keyboard="false">
        <div class="modal-dialog modal-xs" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="modal-event-label">Crear Nuevo Contenedor</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                  
                    <div class="row">
                        <div class="form-group col-md-12">
                            <label for="inputZip">Contenedor</label>
                            <asp:TextBox ID="txtNumFilas" runat="server" class="form-control" Style="text-align: left" placeholder="" MaxLength ="20"
                                 onkeypress="return soloLetras(event,'abcdefghijklmnopqrstuvwxyz0123456789')" onpaste="return false;"  ></asp:TextBox>
                        </div>
                    </div>
                    <div class="row">
                        <div class="form-group col-md-12">
                            <label for="inputZip">Descripción</label>
                            <asp:TextBox ID="txtNumColumnas" runat="server" class="form-control" Style="text-align: left" placeholder="" MaxLength ="150"
                                 onkeypress="return soloLetras(event,'abcdefghijklmnopqrstuvwxyz0123456789')" onpaste="return false;" ></asp:TextBox>
                        </div>
                    </div>
                  
                  


                    <button class="btn btn-primary" onclick="crearNuevoParametro()" type="button">GUARDAR</button>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-dismiss="modal">Cerrar</button>
                </div>



            </div>
        </div>
    </div>


        <div class="modal fade" id="modalEdit" tabindex="-1" role="dialog" aria-labelledby="modal-event-label" aria-hidden="true" data-backdrop="static" data-keyboard="false">
        <div class="modal-dialog modal-xs" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="modal-event-label2">Editar Registro de Contenedor</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                   
                    <div class="row">
                        <div class="form-group col-md-12">
                            <label for="inputZip">Contenedor</label>
                            <asp:TextBox ID="txtNumFilEdit" runat="server" class="form-control" Style="text-align: left" placeholder="" MaxLength ="20"
                                 onkeypress="return soloLetras(event,'abcdefghijklmnopqrstuvwxyz0123456789')" onpaste="return false;" ></asp:TextBox>
                        </div>
                    </div>
                    <div class="row">
                        <div class="form-group col-md-12">
                            <label for="inputZip">Descripción</label>
                            <asp:TextBox ID="txtNumColumnEdit" runat="server" class="form-control" Style="text-align: left" placeholder="" MaxLength ="150"
                                 onkeypress="return soloLetras(event,'abcdefghijklmnopqrstuvwxyz0123456789')" onpaste="return false;" ></asp:TextBox>
                        </div>
                    </div>
                    
                           

                    <button class="btn btn-primary" onclick="EditRegistro()" type="button">GUARDAR</button>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-dismiss="modal">Cerrar</button>
                </div>
            </div>
        </div>
    </div>
    <script type="text/javascript">
        var idBloque = 0;
        $(document).ready(function () {
            buscarTablas();
        });

        function buscarTablas() {
            var idParametro = 0;
            // Realizar solicitud Ajax
            $.ajax({
                url: "VBS_Container_Mantenimiento.aspx/GetListaContainer",
                data: JSON.stringify({}),
                contentType: "application/json; charset=utf-8",
                type: 'POST',
                success: function (response) {
                    // La respuesta exitosa del servidora
                    var tableHTML = [];

                    var tbodyBloque = document.getElementById('tbodyBloque');
                    var data = JSON.parse(response.d);
                    var estado = '';
                    var visible = '';
                    for (var i = 0; i < data.length; i++) {
                        tableHTML += '<tr>';
                        tableHTML += '<td>' + data[i].config_id + '</td>';
                        tableHTML += '<td>' + data[i].container + '</td>';
                        tableHTML += '<td >' + data[i].description + '</td>';
                        tableHTML += '<td >' + data[i].crea_user + '</td>';
                        tableHTML += '<td >' + data[i].crea_date + '</td>';
                        estado = 'ACTIVO';
 

                        tableHTML += '<td style="text-align: center;" >' + '  <button id="btnEdit" class="btn btn-primary" onclick="levantarmodalEdit(this)" type="button">Editar</button>' + '</td>'; // Editable td with data-id attribute

                        tableHTML += '<td style="display: none;">' + data[i].config_id + '</td>'; // Hidden input field
                        tableHTML += '</tr>';
                    }
                    // Asigna el HTML generado a los elementos <tbody> correspondientes
                    tbodyBloque.innerHTML = tableHTML;

                    var editableValorElements = document.getElementsByClassName('editable-valor');
                    for (var j = 0; j < editableValorElements.length; j++) {
                        editableValorElements[j].addEventListener('blur', function (event) {
                            var tdElement = event.target;
                            var valor = tdElement.innerText;
                            idParametro = tdElement.getAttribute('data-id');

                            // Call the function to send the updated value via Ajax
                            sendUpdatedValue(idParametro, valor);
                        });
                    }
                },


                error: function (error) {
                    // El manejo de errores en caso de que la solicitud falle

                }
            });
        }
        function levantarModalEditar()
        {

        }

        function sendUpdatedValue(idParametro, valor) {

            // Make the Ajax request to send the updated value
            $.ajax({
                url: "VBS_Parametros.aspx/EditarParametros",
                data: JSON.stringify({ idParametro: idParametro, valor: valor }),
                contentType: "application/json; charset=utf-8",
                type: 'POST',
                success: function (response) {
                    // Handle the success response

                },
                error: function (error) {
                    // Handle the error response

                }
            });
        }


        function levantarmodal() {
            $('#modalt').modal('show');

        }

        function crearNuevoParametro() {

            var container = document.getElementById('<%=txtNumFilas.ClientID %>').value;
            var description = document.getElementById('<%=txtNumColumnas.ClientID %>').value;

            if (container.length === 0) {

                mostrarAdvertencia('Debe ingresar el contenedor.');
                return;
            }

             if (description.length === 0) {
                mostrarAdvertencia('Debe ingresar la descripción contenedor.');
                return;
            }

            var nuevoParametro = {
                
                container: document.getElementById('<%= txtNumFilas.ClientID %>').value,
                description: document.getElementById('<%= txtNumColumnas.ClientID %>').value
               
            };

            $.ajax({
                url: "VBS_Container_Mantenimiento.aspx/CrearContainer",
                data: JSON.stringify({ obj: nuevoParametro }),
                contentType: "application/json; charset=utf-8",
                type: 'POST',
                success: function (response) {


                    if (response.d == "Existe") {
                        mostrarAdvertencia("No puede crear contenedor, ya existe el mismo.");
                        $('#modalt').modal('hide');
                        buscarTablas();
                    }
                    else {
                        mostrarExito("Nuevo Contenedor creado exitosamente.");
                        $('#modalt').modal('hide');
                        buscarTablas();
                       
                        document.getElementById('<%= txtNumFilas.ClientID %>').value = ''
                        document.getElementById('<%= txtNumColumnas.ClientID %>').value = ''
                    }


                },
                error: function (error) {
                    // Manejar el error
                    console.log('Error:', error);
                }
            });
        }


        function levantarmodalEdit(button) {
            // Get the button that was clicked.
            var row = button.parentNode.parentNode;

            var cells = row.getElementsByTagName("td");
            var config_id = cells[0].innerHTML;
            var container = cells[1].innerHTML;
            var description = cells[2].innerHTML
            idBloque = cells[6].innerHTML;
            $('#modalEdit').modal('show');

                document.getElementById('<%= txtNumFilEdit.ClientID %>').value = container,
                document.getElementById('<%= txtNumColumnEdit.ClientID %>').value = description


        }

        function EditRegistro() {
     
            var container = document.getElementById('<%=txtNumFilEdit.ClientID %>').value;
            var description = document.getElementById('<%=txtNumColumnEdit.ClientID %>').value;

            if (container.length === 0) {

                mostrarAdvertencia('Debe ingresar el contenedor.');
                return;
            }

             if (description.length === 0) {
                mostrarAdvertencia('Debe ingresar la descripción contenedor.');
                return;
            }

            var nuevoParametro = {
                config_id: idBloque,
                container: document.getElementById('<%= txtNumFilEdit.ClientID %>').value,
                description: document.getElementById('<%= txtNumColumnEdit.ClientID %>').value
            };

            $.ajax({
                url: "VBS_Container_Mantenimiento.aspx/EditContainer",
                data: JSON.stringify({ obj: nuevoParametro }),
                contentType: "application/json; charset=utf-8",
                type: 'POST',
                success: function (response) {
                    $('#modalEdit').modal('hide');
                    tbodyBloque.innerHTML = ''
                    buscarTablas();
                },
                
                error: function (error) {
                    // Manejar el error
                    console.log('Error:', error);
                }
            });
        }
        function filtrarTabla() {
            // Obtener el valor de búsqueda del input de búsqueda
            var filtro = document.getElementById('inputBusqueda').value.toUpperCase();
            // Obtener la tabla y las filas de la tabla
            var tabla = document.getElementById('tbodyBloque');

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

       
    </script>

   

    <script type="text/javascript" src="../lib/datatables/datatables.min.js"></script>

    <script type="text/javascript" src="../lib/common-scripts.js"></script>

    <script type="text/javascript" src="../lib/pages.js"></script>

    <script type="text/javascript" src="../lib/advanced-form-components.js"></script>

    <script type="text/javascript" src='../lib/autocompletar/bootstrap3-typeahead.min.js'></script>


    <!--common script for all pages-->
    <script type="text/javascript" src="../lib/gritter/js/jquery.gritter.js"></script>
    <script type="text/javascript" src="../lib/gritter-conf.js"></script>
    <!--script for this page-->

</asp:Content>

