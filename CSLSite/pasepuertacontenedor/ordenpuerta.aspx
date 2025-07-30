<%@ Page Title="" Language="C#" MasterPageFile="~/site.Master" AutoEventWireup="true" CodeBehind="ordenpuerta.aspx.cs" Inherits="CSLSite.ordenpuerta" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="placehead" runat="server">
    <link href="../img/favicon2.png" rel="icon" />
    <link href="../img/icono.png" rel="apple-touch-icon" />
    <link href="../css/bootstrap.min.css" rel="stylesheet" />
    <link href="../css/icons.css" rel="stylesheet" />
    <link href="../css/style.css" rel="stylesheet" />
    <link href="../css/calendario_ajax.css" rel="stylesheet" />
    <script src="https://code.jquery.com/jquery-3.6.0.min.js" integrity="sha256-/xUj+3OJU5yExlq6GSYGSHk7tPXikynS7ogEvDej/m4=" crossorigin="anonymous"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/bootstrap-3-typeahead/4.0.2/bootstrap3-typeahead.min.js"></script>
  
    <link href="../lib/advanced-datatable/css/demo_page.css" rel="stylesheet" />
    <link href="../lib/advanced-datatable/css/demo_table.css" rel="stylesheet" />
    <link rel="stylesheet" href="../lib/advanced-datatable/css/DT_bootstrap2.css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="placebody" runat="server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></asp:ToolkitScriptManager>

    <div id="div_BrowserWindowName" style="visibility:hidden;">
        <asp:HiddenField ID="hf_BrowserWindowName" runat="server" />
        <asp:HiddenField ID="IdTxtCiaTrans" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="IdTxtChofer" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="IdTxtPlaca" runat="server" ClientIDMode="Static" />
    </div>

    <div class="mt-4">
        <nav class="mt-4" aria-label="breadcrumb">
            <ol class="breadcrumb">
                <li class="breadcrumb-item" id="opcion_principal" runat="server"><a href="#">Impo Contenedores</a></li>
                <li class="breadcrumb-item active" aria-current="page" id="sub_opcion" runat="server">ORDEN E-PASS</li>
            </ol>
        </nav>
    </div>

    <div class="dashboard-container p-4" id="cuerpo" runat="server">
        <asp:UpdatePanel ID="UPSEARCH" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <!-- Formulario de búsqueda -->
                <div class="form-row">
                    <div class="form-group col-md-4">
                        <label>RUC Importador</label>
                        <asp:TextBox ID="txtRucImportador" runat="server" CssClass="form-control" />
                    </div>
                    <div class="form-group col-md-2">
                        <label>&nbsp;</label>
                        <asp:Button ID="btnBuscar" runat="server" CssClass="btn btn-primary" Text="Buscar" OnClick="btnBuscar_Click" />
                    </div>
                </div>

                <hr />
                <h4>Contenedores disponibles</h4>
                <asp:GridView ID="gvContenedores" runat="server" AutoGenerateColumns="False" 
                    CssClass="table table-bordered table-hover table-striped"
                    HeaderStyle-CssClass="table-primary text-center"
                    RowStyle-CssClass="text-center"
                    OnRowDataBound="gvContenedores_RowDataBound"
                    AllowPaging="true" PageSize="10" 
                    PagerStyle-CssClass="pagination-ys">
                    <Columns>
                        <asp:TemplateField HeaderText="Seleccionar">
                            <HeaderTemplate>
                                <asp:CheckBox ID="chkAll" runat="server" OnClick="toggleAll(this)" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="chkSeleccion" runat="server" CssClass="chkItem" AutoPostBack="true" OnCheckedChanged="chkSeleccion_CheckedChanged" />
                            </ItemTemplate>
                            <ItemStyle CssClass="text-center" />
                            <HeaderStyle CssClass="text-center" />
                        </asp:TemplateField>
                        <asp:BoundField DataField="NumeroContenedor" HeaderText="Contenedor" />
                        <asp:BoundField DataField="vin" HeaderText="Vin" />                        
                        <asp:BoundField DataField="Manifiesto" HeaderText="Manifiesto" />
                        <asp:BoundField DataField="BL" HeaderText="BL" />
                        <asp:BoundField DataField="ID" HeaderText="ID" Visible="false" />

                    </Columns>
                </asp:GridView>

                <hr />
                <asp:Label ID="lblDebug" runat="server" ForeColor="Green" />
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnBuscar" EventName="Click" />
            </Triggers>
        </asp:UpdatePanel>

        <hr />
       <hr />
<asp:UpdatePanel ID="UPSeleccionados" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <h4>Contenedores seleccionados</h4>
        <asp:GridView ID="gvSeleccionados" runat="server" AutoGenerateColumns="False" 
            CssClass="table table-bordered table-hover table-striped"
            HeaderStyle-CssClass="table-success text-center"
            RowStyle-CssClass="text-center">
            <Columns>
                <asp:BoundField DataField="NumeroContenedor" HeaderText="Contenedor" />
                <asp:BoundField DataField="ID" HeaderText="ID" Visible="false" />
            </Columns>
        </asp:GridView>
    </ContentTemplate>
</asp:UpdatePanel>


        <hr />
        <asp:UpdatePanel ID="UPFORM" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="form-row">
                    <div class="form-group col-md-3">
                        <label>Cia. Trans<span style="color: #FF0000; font-weight: bold;">*</span></label>
                        <asp:TextBox ID="txtCiaTrans" runat="server" CssClass="form-control" 
                            autocomplete="off" onkeypress="return soloLetras(event,'abcdefghijklmnñopqrstuvwxyz0123456789_')" />
                    </div>
                    <div class="form-group col-md-3">
                        <label>Chofer (Opcional)</label>
                        <asp:TextBox ID="txtChofer" runat="server" CssClass="form-control" 
                            autocomplete="off" oncopy="return false;" onpaste="return false;" 
                            onkeypress="return soloLetras(event,'abcdefghijklmnñopqrstuvwxyz0123456789_')" />
                    </div>
                  <div class="form-group col-md-3">
    <label>Placa (Opcional)</label>
    <asp:TextBox ID="txtPlaca" runat="server" CssClass="form-control" 
        autocomplete="off" oncopy="return false;" onpaste="return false;" 
       />
</div>
                    <div class="form-group col-md-3">
                        <label>Fecha Salida<span style="color: #FF0000; font-weight: bold;">*</span></label>
                        <asp:TextBox ID="txtFechaSalida" runat="server" CssClass="form-control" 
                            AutoPostBack="true" MaxLength="10" 
                            onkeypress="return soloLetras(event,'0123456789-')" 
                            OnTextChanged="txtFechaSalida_TextChanged" />
                        <asp:CalendarExtender ID="CAGTFECHASALIDA" runat="server" 
                            CssClass="red" Format="MM-dd-yyyy" TargetControlID="txtFechaSalida" />
                    </div>
                </div>
                <asp:Button ID="btnGenerar" runat="server" CssClass="btn btn-primary" Text="Generar e-Pass" OnClick="btnGenerar_Click" />
                <br /><br />
                <div id="lblMensaje" runat="server" class="alert alert-danger" visible="false"></div>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="txtFechaSalida" EventName="TextChanged" />
                <asp:AsyncPostBackTrigger ControlID="btnGenerar" EventName="Click" />
            </Triggers>
        </asp:UpdatePanel>

        <script type="text/javascript">
            $(document).ready(function () {
                console.log("Documento listo, intentando inicializar typeahead para txtCiaTrans");
                if (typeof $.fn.typeahead === 'function') {
                    console.log("Typeahead está disponible");
                    var $txtCiaTrans = $('[id*=txtCiaTrans]');
                    if ($txtCiaTrans.length) {
                        console.log("Campo txtCiaTrans encontrado, inicializando typeahead");
                        var itemMap = {}; // Variable externa para almacenar el mapeo
                        $txtCiaTrans.typeahead({
                            hint: true,
                            highlight: true,
                            minLength: 1,
                            source: function (query, process) {
                                console.log("Iniciando AJAX para query: " + query);
                                $.ajax({
                                    url: '<%=ResolveUrl("ordenpuerta.aspx/GetEmpresas") %>',
                        data: JSON.stringify({ prefix: query }),
                        dataType: "json",
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        success: function (data) {
                            console.log("Datos recibidos: ", data);
                            var items = [];
                            itemMap = {}; // Reinicia el mapeo
                            if (data.d && Array.isArray(data.d)) {
                                $.each(data.d, function (i, item) {
                                    var parts = item.split('+');
                                    if (parts.length === 2) {
                                        var id = parts[0];
                                        var name = parts[1];
                                        itemMap[name] = { id: id, name: name };
                                        items.push(name);
                                    }
                                });
                            } else {
                                console.log("Datos no válidos o vacíos: ", data);
                            }
                            process(items);
                        },
                        error: function (xhr, status, error) {
                            console.log("Error AJAX: ", status, error, xhr.responseText);
                            alert("Error al cargar datos: " + xhr.responseText);
                        }
                    });
                },
                updater: function (item) {
                    console.log("Seleccionando item: ", item);
                    $('[id*=IdTxtCiaTrans]').val(itemMap[item].id);
                    return item;
                }
            }).on('typeahead:select', function (ev, item) {
                console.log("Item seleccionado: ", item);
            }).on('typeahead:change', function (ev, item) {
                console.log("Cambio detectado: ", item);
            });
                 } else {
                     console.log("Campo txtCiaTrans NO encontrado en el DOM");
                 }
             } else {
                 console.log("Typeahead NO está disponible. Verifica la carga del plugin.");
             }
         });

            $(document).ready(function () {
                console.log("Documento listo, intentando inicializar typeahead para txtChofer");
                if (typeof $.fn.typeahead === 'function') {
                    console.log("Typeahead está disponible");
                    var $txtChofer = $('[id*=txtChofer]');
                    if ($txtChofer.length) {
                        console.log("Campo txtChofer encontrado, inicializando typeahead");
                        var choferMap = {}; // Variable externa para almacenar el mapeo de choferes
                        $txtChofer.typeahead({
                            hint: true,
                            highlight: true,
                            minLength: 1, // Ajustado a 1 para pruebas, puedes volver a 5 si es necesario
                            source: function (query, process) {
                                console.log("Iniciando AJAX para query: " + query);
                                $.ajax({
                                    url: '<%=ResolveUrl("ordenpuerta.aspx/GetChofer") %>',
                        data: JSON.stringify({ prefix: query, idempresa: $("#IdTxtCiaTrans").val() }),
                        dataType: "json",
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        success: function (data) {
                            console.log("Datos recibidos: ", data);
                            var items = [];
                            choferMap = {}; // Reinicia el mapeo
                            if (data.d && Array.isArray(data.d)) {
                                $.each(data.d, function (i, item) {
                                    var parts = item.split('+');
                                    if (parts.length === 2) {
                                        var id = parts[0];
                                        var name = parts[1];
                                        choferMap[name] = { id: id, name: name };
                                        items.push(name);
                                    }
                                });
                            } else {
                                console.log("Datos no válidos o vacíos: ", data);
                            }
                            process(items);
                        },
                        error: function (xhr, status, error) {
                            console.log("Error AJAX: ", status, error, xhr.responseText);
                            alert("Error al cargar datos: " + xhr.responseText);
                        }
                    });
                },
                updater: function (item) {
                    console.log("Seleccionando item: ", item);
                    $('[id*=IdTxtChofer]').val(choferMap[item].id);
                    return item;
                }
            }).on('typeahead:select', function (ev, item) {
                console.log("Item seleccionado: ", item);
            }).on('typeahead:change', function (ev, item) {
                console.log("Cambio detectado: ", item);
            });
              } else {
                  console.log("Campo txtChofer NO encontrado en el DOM");
              }
          } else {
              console.log("Typeahead NO está disponible. Verifica la carga del plugin.");
          }
      });

            $(document).ready(function () {
                console.log("Documento listo, intentando inicializar typeahead para txtPlaca");
                if (typeof $.fn.typeahead === 'function') {
                    console.log("Typeahead está disponible");
                    var $txtPlaca = $('[id*=txtPlaca]');
                    if ($txtPlaca.length) {
                        console.log("Campo txtPlaca encontrado, inicializando typeahead");
                        var placaMap = {}; // Variable externa para almacenar el mapeo de placas
                        $txtPlaca.typeahead({
                            hint: true,
                            highlight: true,
                            minLength: 1, // Ajustado a 1 para pruebas, puedes volver a 3 si es necesario
                            source: function (query, process) {
                                console.log("Iniciando AJAX para query: " + query);
                                $.ajax({
                                    url: '<%=ResolveUrl("ordenpuerta.aspx/GetPlaca") %>',
                        data: JSON.stringify({ prefix: query, idempresa: $("#IdTxtCiaTrans").val() }),
                        dataType: "json",
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        success: function (data) {
                            console.log("Datos recibidos: ", data);
                            var items = [];
                            placaMap = {}; // Reinicia el mapeo
                            if (data.d && Array.isArray(data.d)) {
                                $.each(data.d, function (i, item) {
                                    var parts = item.split('+');
                                    if (parts.length === 2) {
                                        var id = parts[0];
                                        var name = parts[1];
                                        placaMap[name] = { id: id, name: name };
                                        items.push(name);
                                    }
                                });
                            } else {
                                console.log("Datos no válidos o vacíos: ", data);
                            }
                            process(items);
                        },
                        error: function (xhr, status, error) {
                            console.log("Error AJAX: ", status, error, xhr.responseText);
                            alert("Error al cargar datos: " + xhr.responseText);
                        }
                    });
                },
                updater: function (item) {
                    console.log("Seleccionando item: ", item);
                    $('[id*=IdTxtPlaca]').val(placaMap[item].id);
                    return item;
                }
            }).on('typeahead:select', function (ev, item) {
                console.log("Item seleccionado: ", item);
            }).on('typeahead:change', function (ev, item) {
                console.log("Cambio detectado: ", item);
            });
                    } else {
                        console.log("Campo txtPlaca NO encontrado en el DOM");
                    }
                } else {
                    console.log("Typeahead NO está disponible. Verifica la carga del plugin.");
                }
            });

            function soloLetras(e, chars) {
                var key = window.event ? e.keyCode : e.which;
                var keychar = String.fromCharCode(key);
                if (chars.indexOf(keychar) == -1 && key != 8 && key != 9 && key != 13 && key != 37 && key != 39)
                    return false;
                return true;
            }
        </script>
    </div>
</asp:Content>