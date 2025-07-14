 <%@ Page Title="" Language="C#" MasterPageFile="~/site.Master" AutoEventWireup="true" CodeBehind="VBS_Bloque_Mantenimiento.aspx.cs" Inherits="CSLSite.VBS_Bloque_Mantenimiento" %>

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
                <li class="breadcrumb-item" id="opcion_principal" runat="server"><a href="#">PARAMETROS VBS</a></li>
                <li class="breadcrumb-item active" aria-current="page" id="sub_opcion" runat="server">TABLA BLOQUES</li>
            </ol>
        </nav>
    </div>

    <div class="container-fluid">

        <div class="row">
            <div class="col-md-12">
                <div class="card">
                    <div class="card-body">
                        <div id="card-title-1">
                            <h5 class="card-title" id="title-1" style="text-align: center">BLOQUES VBS</h5>
                        </div>
                        <div class="row">
                            <div class="col-md-3">
                                <button class="btn btn-primary" onclick="levantarmodal()" type="button">NUEVO</button>

                            </div>
                            <br />
                            <br />
                        </div>
                        <div class="table-responsive" id="tableBloque">
                            <table class="table">
                                <thead>
                                    <tr>
                                        <th style="background-color: #E23B1B; color: white">Codigo </th>
                                        <th style="background-color: #E23B1B; color: white">Numero De Filas</th>
                                        <th style="background-color: #E23B1B; color: white">Numero De Columnas</th>
                                        <th style="text-align: center; background-color: #E23B1B; color: white">Estado</th>
                                        <th style="text-align: center; background-color: #E23B1B; color: white">Visible</th>
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
                    <h5 class="modal-title" id="modal-event-label">Crear Nuevo Registro</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <div class="form-group col-md-12">
                            <label for="inputZip">Codigo</label>
                              <asp:TextBox ID="txtCodigo" runat="server" class="form-control" Style="text-align: left" placeholder=""></asp:TextBox>
                 

                        </div>
                    </div>
                    <div class="row">
                        <div class="form-group col-md-12">
                            <label for="inputZip">Numero De Filas</label>
                            <asp:TextBox ID="txtNumFilas" runat="server" class="form-control" Style="text-align: left" placeholder=""></asp:TextBox>
                        </div>
                    </div>
                    <div class="row">
                        <div class="form-group col-md-12">
                            <label for="inputZip">Numero De Columnas</label>
                            <asp:TextBox ID="txtNumColumnas" runat="server" class="form-control" Style="text-align: left" placeholder=""></asp:TextBox>
                        </div>
                    </div>
                    <div class="row">
                        <div class="form-group col-md-12">
                            <label for="inputZip">Estado</label>
                          <asp:DropDownList  runat="server" ID="cboEstado" AutoPostBack="false" class="form-control">

                     <asp:ListItem Text="Activo" Value="1"></asp:ListItem>
                     <asp:ListItem Text="Inactivo" Value="2"></asp:ListItem>
                   
                      </asp:DropDownList>
                            
                        </div>
                    </div>
                     <div class="row">
                        <div class="form-group col-md-12">
                            <label for="inputZip">Visible</label>
                          <asp:DropDownList  runat="server" ID="cboVisible" AutoPostBack="false" class="form-control">

                     <asp:ListItem Text="Si" Value="1"></asp:ListItem>
                     <asp:ListItem Text="No" Value="0"></asp:ListItem>
                   
                      </asp:DropDownList>
                            
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
                    <h5 class="modal-title" id="modal-event-label2">Editar Registro</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <div class="form-group col-md-12">
                            <label for="inputZip">Codigo</label>
                              <asp:TextBox ID="txtCodigoEdit" runat="server" class="form-control" Style="text-align: left" placeholder=""></asp:TextBox>
                 

                        </div>
                    </div>
                    <div class="row">
                        <div class="form-group col-md-12">
                            <label for="inputZip">Numero De Filas</label>
                            <asp:TextBox ID="txtNumFilEdit" runat="server" class="form-control" Style="text-align: left" placeholder=""></asp:TextBox>
                        </div>
                    </div>
                    <div class="row">
                        <div class="form-group col-md-12">
                            <label for="inputZip">Numero De Columnas</label>
                            <asp:TextBox ID="txtNumColumnEdit" runat="server" class="form-control" Style="text-align: left" placeholder=""></asp:TextBox>
                        </div>
                    </div>
                    <div class="row">
                        <div class="form-group col-md-12">
                            <label for="inputZip">Estado</label>
                          <asp:DropDownList  runat="server" ID="txtEstadoEdit" AutoPostBack="false" class="form-control">

                     <asp:ListItem Text="Activo" Value="A"></asp:ListItem>
                     <asp:ListItem Text="Inactivo" Value="I"></asp:ListItem>
                   
                      </asp:DropDownList>
                            
                        </div>
                    </div>
                            <div class="row">
                        <div class="form-group col-md-12">
                            <label for="inputZip">Visible</label>
                          <asp:DropDownList  runat="server" ID="cboVisibleEdit" AutoPostBack="false" class="form-control">

                     <asp:ListItem Text="Si" Value="1"></asp:ListItem>
                     <asp:ListItem Text="No" Value="0"></asp:ListItem>
                   
                      </asp:DropDownList>
                            
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
                url: "VBS_Bloque_Mantenimiento.aspx/GetListaBloque",
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
                        tableHTML += '<td>' + data[i].Codigo + '</td>';
                        tableHTML += '<td>' + data[i].NumeroFilas + '</td>';
                        tableHTML += '<td >' + data[i].NumeroColumnas + '</td>';
                        if (data[i].Estado == 'A') {
                            estado = 'Activo'
                        }
                        if (data[i].EsVisible == true) {
                            visible = 'Si'
                        }
                        else {
                            visible = 'No'
                        }
                        tableHTML += '<td style="text-align: center;" class="editable-valor" data-id="' + data[i].IdBloque + '"contenteditable="true">' + estado + '</td>'; // Editable td with data-id attribute
                        tableHTML += '<td style="text-align: center;" >' + visible + '</td>'; // Editable td with data-id attribute

                        tableHTML += '<td style="text-align: center;" >' + '  <button id="btnEdit" class="btn btn-primary" onclick="levantarmodalEdit(this)" type="button">Editar</button>' + '</td>'; // Editable td with data-id attribute

                        tableHTML += '<td style="display: none;">' + data[i].IdBloque + '</td>'; // Hidden input field
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
        function levantarModalEditar() {

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
            var visible = true;
            console.log("visible", document.getElementById('<%= cboVisible.ClientID %>').value)
            if (document.getElementById('<%= cboVisible.ClientID %>').value == "1")
                visible = true;
            else {
                visible = false;
            }
            document.getElementById('<%= cboVisible.ClientID %>').value
            var nuevoParametro = {
                Codigo: document.getElementById('<%= txtCodigo.ClientID %>').value,
                NumeroFilas: document.getElementById('<%= txtNumFilas.ClientID %>').value,
                NumeroColumnas: document.getElementById('<%= txtNumColumnas.ClientID %>').value,
                Estado: document.getElementById('<%= cboEstado.ClientID %>').value,
                EsVisible: visible
            };

            $.ajax({
                url: "VBS_Bloque_Mantenimiento.aspx/CrearBloque",
                data: JSON.stringify({ obj: nuevoParametro }),
                contentType: "application/json; charset=utf-8",
                type: 'POST',
                success: function (response) {


                    if (response.d == "Existe") {
                        mostrarAdvertencia("No puede crear Bloque, ya existe un bloque con este Codigo.");
                        $('#modalt').modal('hide');
                        buscarTablas();
                    }
                    else {
                        mostrarExito("Nuevo parámetro creado exitosamente.");
                        $('#modalt').modal('hide');
                        buscarTablas();
                        document.getElementById('<%= txtCodigo.ClientID %>').value = ''
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
            var codigo = cells[0].innerHTML;
            var numeroFilas = cells[1].innerHTML;
            var numeroColumnas = cells[2].innerHTML
            idBloque = cells[6].innerHTML;
            $('#modalEdit').modal('show');
            document.getElementById('<%= txtCodigoEdit.ClientID %>').value = codigo,
                document.getElementById('<%= txtNumFilEdit.ClientID %>').value = numeroFilas,
                document.getElementById('<%= txtNumColumnEdit.ClientID %>').value = numeroColumnas


        }

        function EditRegistro() {
            console.log("visible", document.getElementById('<%= cboVisible.ClientID %>').value)
            var visible = true;
            if (document.getElementById('<%= cboVisibleEdit.ClientID %>').value == "1")
                visible = true;
            else {
                visible = false;
            }
            var nuevoParametro = {
                IdBloque: idBloque,
                Codigo: document.getElementById('<%= txtCodigoEdit.ClientID %>').value,
                NumeroFilas: document.getElementById('<%= txtNumFilEdit.ClientID %>').value,
                NumeroColumnas: document.getElementById('<%= txtNumColumnEdit.ClientID %>').value,
                Estado: document.getElementById('<%= txtEstadoEdit.ClientID %>').value,
                EsVisible: visible
            };

            $.ajax({
                url: "VBS_Bloque_Mantenimiento.aspx/EditBloque",
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
       
    </script>

   

    <script type="text/javascript" src="../lib/datatables/datatables.min.js"></script>
    <%-- <script type="text/javascript" src="../lib/advanced-datatable/js/DT_bootstrap.js"></script>--%>

    <script type="text/javascript" src="../lib/common-scripts.js"></script>

    <script type="text/javascript" src="../lib/pages.js"></script>

    <script type="text/javascript" src="../lib/advanced-form-components.js"></script>

    <script type="text/javascript" src='../lib/autocompletar/bootstrap3-typeahead.min.js'></script>



    <!--common script for all pages-->
    <script type="text/javascript" src="../lib/gritter/js/jquery.gritter.js"></script>
    <script type="text/javascript" src="../lib/gritter-conf.js"></script>
    <!--script for this page-->

</asp:Content>

