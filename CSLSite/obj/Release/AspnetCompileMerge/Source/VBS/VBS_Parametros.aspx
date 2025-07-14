 <%@ Page Title="" Language="C#" MasterPageFile="~/site.Master" AutoEventWireup="true" CodeBehind="VBS_Parametros.aspx.cs" Inherits="CSLSite.VBS_Parametros" %>

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
                <li class="breadcrumb-item active" aria-current="page" id="sub_opcion" runat="server">TABLA PARAMETROS</li>
            </ol>
        </nav>
    </div>

    <div class="container-fluid">

        <div class="row">
            <div class="col-md-12">
                <div class="card">
                    <div class="card-body">
                        <div id="card-title-1">
                            <h5 class="card-title" id="title-1" style="text-align: center">Parametros VBS</h5>
                        </div>
                        <div class="row">
                            <div class="col-md-3">
                                <button class="btn btn-primary" onclick="levantarmodal()" type="button">NUEVO</button>

                            </div>
                            <br />
                            <br />
                        </div>
                        <div class="table-responsive">
                            <table class="table">
                                <thead>
                                    <tr>
                                        <th style="background-color: #E23B1B; color: white">Tipo </th>
                                        <th style="background-color: #E23B1B; color: white">Parametro</th>
                                        <th style="background-color: #E23B1B; color: white">Descripcion</th>
                                        <th style="text-align: center; background-color: #E23B1B; color: white">Valor</th>
                                    </tr>
                                </thead>
                                <tbody id="tbodyParametros">
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
                            <label for="inputZip">TIPO</label>
                            <asp:DropDownList runat="server" ID="cboTipo" AutoPostBack="false" class="form-control">
                                <asp:ListItem Text="EXPO" Value="EXPO"></asp:ListItem>
                                <asp:ListItem Text="IMPO" Value="IMPO"></asp:ListItem>
                                <asp:ListItem Text="BANANO" Value="BANANO"></asp:ListItem>

                            </asp:DropDownList>

                        </div>
                    </div>
                    <div class="row">
                        <div class="form-group col-md-12">
                            <label for="inputZip">PARAMETRO</label>
                            <asp:TextBox ID="txtParametro" runat="server" class="form-control" Style="text-align: left" placeholder=""></asp:TextBox>
                        </div>
                    </div>
                    <div class="row">
                        <div class="form-group col-md-12">
                            <label for="inputZip">DESCRIPCION</label>
                            <asp:TextBox ID="txtDescripcion" runat="server" class="form-control" Style="text-align: left" placeholder=""></asp:TextBox>
                        </div>
                    </div>
                    <div class="row">
                        <div class="form-group col-md-12">
                            <label for="inputZip">VALOR</label>
                            <asp:TextBox ID="txtValor" runat="server" class="form-control" Style="text-align: left" placeholder=""></asp:TextBox>
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
    <script type="text/javascript">
        $(document).ready(function () {
            buscarTablas();
        });

        function buscarTablas() {

            // Realizar solicitud Ajax
            $.ajax({
                url: "VBS_Parametros.aspx/getTablaParametros",
                data: JSON.stringify({}),
                contentType: "application/json; charset=utf-8",
                type: 'POST',
                success: function (response) {
                    // La respuesta exitosa del servidor
                    // Aquí puedes procesar la respuesta y generar la tabla
                    var tableHTML = [];

                    var tbodyParametros = document.getElementById('tbodyParametros');
                    var data = JSON.parse(response.d);

                    for (var i = 0; i < data.length; i++) {
                        tableHTML += '<tr>';
                        tableHTML += '<td>' + data[i].Tipo + '</td>';
                        tableHTML += '<td>' + data[i].Parametro + '</td>';
                        tableHTML += '<td >' + data[i].Descripcion + '</td>';
                        tableHTML += '<td style="text-align: center;" class="editable-valor" data-id="' + data[i].IdParametro + '"contenteditable="true">' + data[i].Valor + '</td>'; // Editable td with data-id attribute
                        tableHTML += '<td style="display: none;">' + data[i].IdParametro + '</td>'; // Hidden input field
                        tableHTML += '</tr>';
                    }
                    // Asigna el HTML generado a los elementos <tbody> correspondientes
                    tbodyParametros.innerHTML = tableHTML;

                    var editableValorElements = document.getElementsByClassName('editable-valor');
                    for (var j = 0; j < editableValorElements.length; j++) {
                        editableValorElements[j].addEventListener('blur', function (event) {
                            var tdElement = event.target;
                            var valor = tdElement.innerText;
                            var idParametro = tdElement.getAttribute('data-id');

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

            var nuevoParametro = {
                Tipo: document.getElementById('<%= cboTipo.ClientID %>').value,
                Parametro: document.getElementById('<%= txtParametro.ClientID %>').value,
                Descripcion: document.getElementById('<%= txtDescripcion.ClientID %>').value,
                Valor: document.getElementById('<%= txtValor.ClientID %>').value
            };

            $.ajax({
                url: "VBS_Parametros.aspx/CrearParametros",
                data: JSON.stringify({ obj: nuevoParametro }),
                contentType: "application/json; charset=utf-8",
                type: 'POST',
                success: function (response) {
                    console.log("response", response);
                    
                    if (response.d == "Existe") {
                        mostrarAdvertencia("No puede crear, parámetro ya existe intente con un nombre de parametro diferente.");
                        $('#modalt').modal('hide');
                        buscarTablas();
                    }
                    else
                    {
                        mostrarExito("Nuevo parámetro creado exitosamente.");
                        $('#modalt').modal('hide');
                        buscarTablas();
                    }
               

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

