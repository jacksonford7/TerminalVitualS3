<%@ Page Title="Opciones Servicios" Language="C#" MasterPageFile="~/site.Master"
    AutoEventWireup="true" CodeBehind="datosopcionservicios.aspx.cs" Inherits="CSLSite.datosopcionservicios" %>

<%@ MasterType VirtualPath="~/site.Master" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="placehead" runat="server">
    <link href="../shared/estilo/tables.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/aisv-general.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/work.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/catalogos.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/info.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/chosen.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        function BindFunctions() {
            $(document).ready(function () {
                $('#tablasort').tablesorter({ cancelSelection: true, cssAsc: "ascendente", cssDesc: "descendente", headers: { 5: { sorter: false }, 6: { sorter: false}} }).tablesorterPager({ container: $('#pager'), positionFixed: false, pagesize: 10 });
            });

            $(document).ready(function () {
                //inicia los fecha-hora

                //colapsar y expandir
                $("div.colapser").toggle(function () { $(this).removeClass("colapsa").addClass("expande"); $(this).next().hide(); }
                                  , function () { $(this).removeClass("expande").addClass("colapsa"); $(this).next().show(); });
                //poner valor en campo

            });
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="placebody" runat="server">
    <asp:ToolkitScriptManager ID="tkcal" EnableScriptGlobalization="true" runat="server">
    </asp:ToolkitScriptManager>
    <input id="zonaid" type="hidden" value="2" />
    <noscript>
        <meta http-equiv="refresh" content="0; url=sinsoporte.htm" />
    </noscript>
    <asp:UpdatePanel ID="updOpciones" runat="server">
        <ContentTemplate>
            <div>
                <i class="ico-titulo-1"></i>
                <h2>
                    Opciones de servicios del sistema
                </h2>
                <br />
                <i class="ico-titulo-2"></i>
                <h1>
                    Ingreso/Modificación de Opciones de Servicio
                </h1>
                <br />
            </div>
            <%--<div class=" msg-alerta">
                <span id="dtlo" runat="server">Estimado usuario:</span>
                <br />
                ???? <strong>??? </strong>???
            </div>--%>
            <div class="seccion" id="SESION">
                <div class="informativo">
                    <table>
                        <tr>
                            <td rowspan="2" class="inum">
                                <div class="number">
                                    1</div>
                            </td>
                            <td class="level1">
                                Ingreso/Modificación de opciones del sistema.
                            </td>
                        </tr>
                        <tr>
                            <td class="level2">
                                Ingrese o modifique los datos de las opciones.
                            </td>
                        </tr>
                    </table>
                </div>
                <div class="colapser colapsa">
                </div>
                <div class="accion">
                    <table class="controles" cellspacing="0" cellpadding="1">
                        <tr>
                            <th class="bt-bottom bt-right bt-top bt-left" colspan="4">
                                Datos de la Opción
                            </th>
                        </tr>
                        <tr>
                            <td class="bt-bottom  bt-right bt-left">
                                1. Nombre de Opción:
                            </td>
                            <td class="bt-bottom">
                                <asp:HiddenField ID="idOpcion" runat="server" />
                                <asp:TextBox ID="txtOpcionNombre" runat="server" MaxLength="100"
                                    onkeypress="return soloLetras(event,'abcdefghijklmnñopqrstuvwxyz1234567890áéíóú,;.:()!¡* -/',true)"
                                     onblur="cadenareqerida(this,1,100,'valOpNom');" placeholder="OPCION"
                                    Width="200px"></asp:TextBox>
                            </td>
                            <td class="bt-bottom bt-right validacion">
                                <span class="validacion" id="valOpNom">* obligatorio</span>
                            </td>
                        </tr>
                        <tr>
                            <td class="bt-bottom  bt-right bt-left">
                                2. Descripción:
                            </td>
                            <td class="bt-bottom">
                                <asp:TextBox ID="txtTextoIntro" runat="server"  onkeypress="return soloLetras(event,'abcdefghijklmnñopqrstuvwxyz1234567890áéíóú,;.:()!¡* -/',true)"
                                    MaxLength="200" onblur="cadenareqerida(this,1,200,'valOpDes');"
                                    placeholder="DESCRIPCION" Width="200px"></asp:TextBox>
                            </td>
                            <td class="bt-bottom bt-right validacion">
                                <span class="validacion" id="valOpDes">* obligatorio</span>
                            </td>
                        </tr>
                        <tr>
                            <td class="bt-bottom  bt-right bt-left">
                                3. Formulario a redireccionar:
                            </td>
                            <td class="bt-bottom">
                                <asp:TextBox ID="txtFormASP" runat="server"  onkeypress="return soloLetras(event,'abcdefghijklmnñopqrstuvwxyz1234567890./_',true)"
                                    MaxLength="200" onblur="cadenareqerida(this,1,200,'valOpFor');"
                                    placeholder="FORMULARIO" Width="200px"></asp:TextBox>
                            </td>
                            <td class="bt-bottom bt-right validacion">
                                <span class="validacion" id="valOpFor">* obligatorio</span>
                            </td>
                        </tr>
                        <tr>
                            <td class="bt-bottom  bt-right bt-left">
                                4. Título:
                            </td>
                            <td class="bt-bottom">
                                <asp:TextBox ID="txtTitulo" runat="server"  onkeypress="return soloLetras(event,'abcdefghijklmnñopqrstuvwxyz1234567890áéíóú,;.:()!¡* -/',true)"
                                    MaxLength="200"  onblur="cadenareqerida(this,1,200,'valOpTit');"
                                    placeholder="TITULO" Width="200px"></asp:TextBox>
                            </td>
                            <td class="bt-bottom bt-right validacion">
                                <span class="validacion" id="valOpTit">* obligatorio</span>
                            </td>
                        </tr>
                        <tr>
                            <td class="bt-bottom  bt-right bt-left">
                                5. Servicio:
                            </td>
                            <td class="bt-bottom">
                                <asp:DropDownList ID="ddlServicio" runat="server" Width="205px" AppendDataBoundItems="true"
                                    onblur="opcionrequerida(this,0,'valOpSer');">
                                </asp:DropDownList>
                            </td>
                            <td class="bt-bottom bt-right validacion">
                                <span class="validacion" id="valOpSer">* obligatorio</span>
                            </td>
                        </tr>
                        <tr>
                            <td class="bt-bottom  bt-right bt-left">
                                6. Estado:
                            </td>
                            <td class="bt-bottom">
                                <asp:DropDownList ID="ddlEstado" runat="server" Width="205px" AutoPostBack="true">
                                    <asp:ListItem Text="ACTIVO" Value="A"></asp:ListItem>
                                    <asp:ListItem Text="INACTIVO" Value="I"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td class="bt-bottom bt-right validacion">
                                <span class="validacion" id="Span2">* obligatorio</span>
                            </td>
                        </tr>
                    </table>
                    <div class="botonera">
                        <asp:UpdateProgress AssociatedUpdatePanelID="updOpciones" ID="updateProgress"
                            runat="server">
                            <ProgressTemplate>
                                <div id="progressBackgroundFilter">
                                </div>
                                <div id="processMessage">
                                    Estamos procesando la tarea que solicitó, por favor espere...
                                    <img alt="Loading" src="../shared/imgs/loader.gif" style="margin: 0 auto;" />
                                </div>
                            </ProgressTemplate>
                        </asp:UpdateProgress>
                        <asp:Button ID="btnGuardar" Text="Guardar" runat="server" OnClick="btnGuardar_Click" />
                        &nbsp;
                        <asp:Button ID="btnLimpiar" Text="Limpiar" runat="server" OnClick="btnLimpiar_Click" />
                        &nbsp;
                    </div>
                    <div class="msg-alerta" id="alerta" runat="server">
                    </div>
                    <div id="error" runat="server" class="msg-critico" visible="false">
                    </div>
                </div>
            </div>
            <div class="seccion" id="PERSONAL">
                <div class="informativo">
                    <table>
                        <tr>
                            <td rowspan="2" class="inum">
                                <div class="number">
                                    2</div>
                            </td>
                            <td class="level1">
                                Consulta de Opciones
                            </td>
                        </tr>
                        <tr>
                            <td class="level2">
                                Consulta de Opciones
                            </td>
                        </tr>
                    </table>
                </div>
                <div class="colapser colapsa">
                </div>
                <div class="accion">
                    <table class="controles" cellspacing="0" cellpadding="1">
                        <tr>
                            <th class="bt-bottom bt-right bt-top bt-left" colspan="4">
                                Criterios de Búsqueda
                            </th>
                        </tr>
                        <tr>
                            <td class="bt-bottom  bt-right bt-left">
                                1. Nombre de Opción:
                            </td>
                            <td class="bt-bottom bt-right" colspan="3">
                                <asp:TextBox ID="txtNombreOpcionBuscar" runat="server"
                                  onkeypress="return soloLetras(event,'abcdefghijklmnñopqrstuvwxyz1234567890áéíóú ./',true)"
                                    MaxLength="100"  placeholder="OPCION" Width="200px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="bt-bottom  bt-right bt-left">
                                2. Servicio:
                            </td>
                            <td class="bt-bottom bt-right" colspan="3">
                                <asp:TextBox ID="txtServicioBuscar" runat="server"  onkeypress="return soloLetras(event,'abcdefghijklmnñopqrstuvwxyz1234567890 .-/',true)"
                                    MaxLength="100" onpaste="return false;" placeholder="NOMBRE" Width="200px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="bt-bottom  bt-right bt-left">
                                3. Estado:
                            </td>
                            <td class="bt-bottom bt-right" colspan="3">
                                <asp:DropDownList ID="ddlEstadoBuscar" runat="server" Width="205px">
                                    <asp:ListItem Text="TODOS" Value="0"></asp:ListItem>
                                    <asp:ListItem Text="ACTIVO" Value="A"></asp:ListItem>
                                    <asp:ListItem Text="INACTIVO" Value="I"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                    <div class="botonera">
                        <asp:UpdateProgress AssociatedUpdatePanelID="updOpciones" ID="updateProgress1"
                            runat="server">
                            <ProgressTemplate>
                                <div id="progressBackgroundFilter">
                                </div>
                                <div id="processMessage">
                                    Estamos procesando la tarea que solicitó, por favor espere...
                                    <img alt="Loading" src="../shared/imgs/loader.gif" style="margin: 0 auto;" />
                                </div>
                            </ProgressTemplate>
                        </asp:UpdateProgress>
                        <asp:Button ID="btbuscar" Text="Iniciar búsqueda" runat="server" OnClick="btbuscar_Click" />
                        <asp:Button ID="btDeshabilitar" Text="Iniciar búsqueda" runat="server" OnClick="btDeshabilitar_Click"
                            Style="display: none;" />
                        &nbsp;
                    </div>
                    <div class="cataresult">
                        <asp:UpdatePanel ID="upresult" runat="server">
                            <ContentTemplate>
                                <script type="text/javascript">                                    Sys.Application.add_load(BindFunctions); </script>
                                <input id="idlin" type="hidden" runat="server" clientidmode="Static" />
                                <input id="diponible" type="hidden" runat="server" clientidmode="Static" />
                                <div id="xfinder" runat="server" visible="false">
                                    <div class="findresult">
                                        <div class="booking">
                                            <div class="separator">
                                                Listado de Opciones:</div>
                                            <div class="bokindetalle">
                                                <asp:Repeater ID="tablePagination" runat="server">
                                                    <HeaderTemplate>
                                                        <table id="tablasort" cellspacing="1" cellpadding="1" class="tabRepeat">
                                                            <thead>
                                                                <tr>
                                                                    <th style="display: none;">
                                                                        idOpcion
                                                                    </th>
                                                                    <th style="display: none;">
                                                                        idServicio
                                                                    </th>
                                                                    <th style="display: none;">
                                                                        formulario
                                                                    </th>
                                                                    <th style="display: none;">
                                                                        titulo
                                                                    </th>
                                                                    <th>
                                                                        Opción
                                                                    </th>
                                                                    <th>
                                                                        Descripción
                                                                    </th>
                                                                    <th>
                                                                        Servicio
                                                                    </th>
                                                                    <th>
                                                                        Estado
                                                                    </th>
                                                                    <th>
                                                                    </th>
                                                                </tr>
                                                            </thead>
                                                            <tbody>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <tr class="point" onclick="setOption(this);">
                                                            <td style="display: none;">
                                                                <%#Eval("idOpcion")%>
                                                            </td>
                                                            <td style="display: none;">
                                                                <%#Eval("idServicio")%>
                                                            </td>
                                                            <td style="display: none;">
                                                                <%#Eval("formulario")%>
                                                            </td>
                                                            <td style="display: none;">
                                                                <%#Eval("titulo")%>
                                                            </td>
                                                            <td>
                                                                <%#Eval("nombreOpcion")%>
                                                            </td>
                                                            <td>
                                                                <%#Eval("descripcion")%>
                                                            </td>
                                                            <td>
                                                                <%#Eval("nombreServicio")%>
                                                            </td>
                                                            <td>
                                                                <%#Eval("estado")%>
                                                            </td>
                                                            <td>
                                                                <div class="tcomand">
                                                                    <asp:Button ID="btnModificar" runat="server" Text="Modificar" CssClass="Anular" ToolTip="Carga la información de la opción para modificarla" />
                                                                </div>
                                                            </td>
                                                        </tr>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        </tbody> </table>
                                                    </FooterTemplate>
                                                </asp:Repeater>
                                            </div>
                                            <%--<div class="botonera" runat="server" id="btnera">
        <img alt="loading.." src="../shared/imgs/loader.gif" id="loader" class="nover"  />
        <input id="btsalvar" type="button" value="Proceder y Asignar"  onclick="prepareObject();encerar();" /> &nbsp;
        </div>--%>
                                        </div>
                                    </div>
                                    <div id="pager">
                                        Registros por página
                                        <select class="pagesize">
                                            <option selected="selected" value="10">10</option>
                                            <option value="20">20</option>
                                        </select>
                                        <img alt="" src="../shared/imgs/first.gif" class="first" />
                                        <img alt="" src="../shared/imgs/prev.gif" class="prev" />
                                        <input type="text" class="pagedisplay" size="5px" />
                                        <img alt="" src="../shared/imgs/next.gif" class="next" />
                                        <img alt="" src="../shared/imgs/last.gif" class="last" />
                                    </div>
                                </div>
                                <div id="sinresultado" runat="server" class="msg-info">
                                </div>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btbuscar" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <script src="../Scripts/pages.js" type="text/javascript"></script>
    <script src="../Scripts/pages_mod.js" type="text/javascript"></script>
    <script src="../Scripts/jquery-1.7.1.min.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.tablesorter.min.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.tablesorter.pager.js" type="text/javascript"></script>
    <script src="../Scripts/chosen.jquery.js" type="text/javascript"></script>
    <script type="text/javascript">
        var ced_count = 0;
        var jAisv = {};
        $(window).load(function () {
            //objeto a transportar.
            $(document).ready(function () {
                //inicia los fecha-hora

                //colapsar y expandir
                $("div.colapser").toggle(function () { $(this).removeClass("colapsa").addClass("expande"); $(this).next().hide(); }
                                  , function () { $(this).removeClass("expande").addClass("colapsa"); $(this).next().show(); });
                //poner valor en campo

            });
        });


        //Esta funcion cargara los datos del servicio seleccionado para modificar
        function setOption(row) {
            var celColect = row.getElementsByTagName('td');
            var opcion = {
                codigo: celColect[0].textContent.trim(),
                idServicio: celColect[1].textContent.trim(),
                formulario: celColect[2].textContent.trim(),
                titulo: celColect[3].textContent.trim(),
                nombreOpcion: celColect[4].textContent.trim(),
                descripcion: celColect[5].textContent.trim(),
                nombreServicio: celColect[6].textContent.trim(),
                estado: celColect[7].textContent.trim() == 'ACTIVO' ? 'A' : 'I'
            };
            $("#<%=idOpcion.ClientID%>").val(opcion.codigo); //set value
            $("#<%=txtOpcionNombre.ClientID%>").val(opcion.nombreOpcion); //set value
            $("#<%=txtTextoIntro.ClientID%>").val(opcion.descripcion); //set value
            $("#<%=txtFormASP.ClientID%>").val(opcion.formulario); //set value
            $("#<%=txtTitulo.ClientID%>").val(opcion.titulo); //set value
            $("#<%=ddlServicio.ClientID%>").val(opcion.idServicio); //set value
            $("#<%=ddlServicio.ClientID%>").empty();
            $("#<%=ddlServicio.ClientID%>").append("<option value='" + opcion.idServicio + "'>" + opcion.nombreServicio + "</option>");
            $("#<%=ddlEstado.ClientID%>").val(opcion.estado); //set value


        }



        //Esta funcion va a validar que cuando presionen booking debe poner los 3 parametros
        function validateBook(objeto) {

        }

        //Imprimir.......................
        function imprimir() {

            //Si es contenedor validar cedula

        }

        //esta futura funcion va a preparar el objeto a transportar.
        function prepareObject() {






        }
        function popupCallback(data, control) {
            this.document.getElementById(control).value = data;
        }

    </script>
</asp:Content>
