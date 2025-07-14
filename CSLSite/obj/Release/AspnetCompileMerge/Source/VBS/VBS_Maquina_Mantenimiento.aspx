 <%@ Page Title="" Language="C#" MasterPageFile="~/site.Master" AutoEventWireup="true" CodeBehind="VBS_Maquina_Mantenimiento.aspx.cs" Inherits="CSLSite.VBS_Maquina_Mantenimiento" %>

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
                <li class="breadcrumb-item active" aria-current="page" id="sub_opcion" runat="server">TABLA MAQUINAS</li>
            </ol>
        </nav>
    </div>

    <div class="container-fluid">

        <div class="row">
            <div class="col-md-12">
                <div class="card">
                    <div class="card-body">
                        <div id="card-title-1">
                            <h5 class="card-title" id="title-1" style="text-align: center">MAQUINAS VBS</h5>
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
                                        <th style="background-color: #E23B1B; color: white">Codigo </th>
                                        <th style="background-color: #E23B1B; color: white">Tipo</th>
                                        <th style="background-color: #E23B1B; color: white">Capacidad Operativa</th>
                                        <th style="text-align: center; background-color: #E23B1B; color: white">Estado</th>
                                        <th style="text-align: center; background-color: #E23B1B; color: white">Accion</th>
                                    </tr>
                                </thead>
                                <tbody id="tbodyMaquina">
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
                            <label for="inputZip">Tipo</label>
                            <asp:TextBox ID="txtTipo" runat="server" class="form-control" Style="text-align: left" placeholder=""></asp:TextBox>
                        </div>
                    </div>
                    <div class="row">
                        <div class="form-group col-md-12">
                            <label for="inputZip">Capacidad Operativa</label>
                            <asp:TextBox ID="txtCapacidad" runat="server" class="form-control" Style="text-align: left" placeholder=""></asp:TextBox>
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
                            <label for="inputZip">Tipo</label>
                            <asp:TextBox ID="txtTipoEdit" runat="server" class="form-control" Style="text-align: left" placeholder=""></asp:TextBox>
                        </div>
                    </div>
                    <div class="row">
                        <div class="form-group col-md-12">
                            <label for="inputZip">CapacidadOperativa</label>
                            <asp:TextBox ID="txtCapacidadOperativaEdit" runat="server" class="form-control" Style="text-align: left" placeholder=""></asp:TextBox>
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

                    <button class="btn btn-primary" onclick="EditRegistro()" type="button">GUARDAR</button>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-dismiss="modal">Cerrar</button>
                </div>
            </div>
        </div>
    </div>
    <script type="text/javascript">
        var idMaquina = 0;
        $(document).ready(function () {
            buscarTablas();
        });

        function buscarTablas() {
            var idParametro = 0;
            // Realizar solicitud Ajax
            $.ajax({
                url: "VBS_Maquina_Mantenimiento.aspx/GetListaMaquina",
                data: JSON.stringify({}),
                contentType: "application/json; charset=utf-8",
                type: 'POST',
                success: function (response) {
                    // La respuesta exitosa del servidora
                    var tableHTML = [];

                    var tbodyMaquina = document.getElementById('tbodyMaquina');
                    var data = JSON.parse(response.d);
                    if (data != null) {
                        var estado = '';
                        for (var i = 0; i < data.length; i++) {
                            tableHTML += '<tr>';
                            tableHTML += '<td>' + data[i].Codigo + '</td>';
                            tableHTML += '<td>' + data[i].Tipo + '</td>';
                            tableHTML += '<td >' + data[i].CapacidadOperativa + '</td>';
                            if (data[i].Estado == 'A') {
                                estado = 'Activo'
                            }
                            else {
                                estado = 'Inactivo'
                            }
                            tableHTML += '<td style="text-align: center;" class="editable-valor" data-id="' + data[i].IdMaquina + '"contenteditable="true">' + estado + '</td>'; // Editable td with data-id attribute
                            tableHTML += '<td style="text-align: center;" >' + '  <button id="btnEdit" class="btn btn-primary" onclick="levantarmodalEdit(this)" type="button">Editar</button>' + '</td>'; // Editable td with data-id attribute

                            tableHTML += '<td style="display: none;">' + data[i].IdMaquina + '</td>'; // Hidden input field
                            tableHTML += '</tr>';
                        }
                        // Asigna el HTML generado a los elementos <tbody> correspondientes
                        tbodyMaquina.innerHTML = tableHTML;
                    }
             
                },


                error: function (error) {
                    // El manejo de errores en caso de que la solicitud falle

                }
            });
        }
      

        function levantarmodal() {
            $('#modalt').modal('show');

        }

        function crearNuevoParametro() {

            var nuevoParametro = {
                Codigo: document.getElementById('<%= txtCodigo.ClientID %>').value,
                Tipo: document.getElementById('<%= txtTipo.ClientID %>').value,
                CapacidadOperativa: document.getElementById('<%= txtCapacidad.ClientID %>').value,
                Estado: document.getElementById('<%= cboEstado.ClientID %>').value
            };

            $.ajax({
                url: "VBS_Maquina_Mantenimiento.aspx/CrearMaquina",
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
                        document.getElementById('<%= txtTipo.ClientID %>').value = ''
                        document.getElementById('<%= txtCapacidad.ClientID %>').value = ''
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
            var tipo = cells[1].innerHTML;
            var capacidad = cells[2].innerHTML
            idMaquina = cells[5].innerHTML;
            $('#modalEdit').modal('show');
            document.getElementById('<%= txtCodigoEdit.ClientID %>').value = codigo,
                document.getElementById('<%= txtTipoEdit.ClientID %>').value = tipo,
                document.getElementById('<%= txtCapacidadOperativaEdit.ClientID %>').value = capacidad


        }

        function EditRegistro() {
            var nuevoParametro = {
                IdMaquina: idMaquina,
                Codigo: document.getElementById('<%= txtCodigoEdit.ClientID %>').value,
                Tipo: document.getElementById('<%= txtTipoEdit.ClientID %>').value,
                CapacidadOperativa: document.getElementById('<%= txtCapacidadOperativaEdit.ClientID %>').value,
                Estado: document.getElementById('<%= txtEstadoEdit.ClientID %>').value
            };

            $.ajax({
                url: "VBS_Maquina_Mantenimiento.aspx/EditMaquina",
                data: JSON.stringify({ obj: nuevoParametro }),
                contentType: "application/json; charset=utf-8",
                type: 'POST',
                success: function (response) {
                    $('#modalEdit').modal('hide');
                    tbodyMaquina.innerHTML = ''
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

