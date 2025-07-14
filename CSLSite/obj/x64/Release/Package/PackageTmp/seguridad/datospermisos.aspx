<%@ Page Title="Permisos" Language="C#" MasterPageFile="~/site.Master" AutoEventWireup="true"
    CodeBehind="datospermisos.aspx.cs" Inherits="CSLSite.datospermisos" %>

<%@ MasterType VirtualPath="~/site.Master" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="placehead" runat="server">
    <link href="../shared/estilo/tables.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/catalogos.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/info.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/aisv-general.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/work.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        function BindFunctions() {

            $(document).ready(function () {
                //inicia los fecha-hora

                //colapsar y expandir
                $("div.colapser").toggle(function () { $(this).removeClass("colapsa").addClass("expande"); $(this).next().hide(); }
                                  , function () { $(this).removeClass("expande").addClass("colapsa"); $(this).next().show(); });
                //poner valor en campo

            });

            $(document).ready(function () {
                $("#tblCustomers [id*=chkHeader]").click(function () {
                    if ($(this).is(":checked")) {
                        $("#tblCustomers [id*=chkRow]").attr("checked", "checked");
                    } else {
                        $("#tblCustomers [id*=chkRow]").removeAttr("checked");
                    }
                });

                $("#tblCustomers [id*=chkRow]").click(function () {
                    if ($("#tblCustomers [id*=chkRow]").length == $("#tblCustomers [id*=chkRow]:checked").length) {
                        $("#tblCustomers [id*=chkHeader]").attr("checked", "checked");
                    } else {
                        $("#tblCustomers [id*=chkHeader]").removeAttr("checked");
                    }
                });
            });
        }

        
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="placebody" runat="server">
    <asp:ToolkitScriptManager ID="tkcal" EnableScriptGlobalization="true" runat="server">
    </asp:ToolkitScriptManager>
    <input id="zonaid" type="hidden" value="908" />
    <noscript>
        <meta http-equiv="refresh" content="0; url=sinsoporte.htm" />
    </noscript>
    <div>
        <i class="ico-titulo-1"></i>
        <h2>
            Asignación de permisos
        </h2>
        <br />
        <i class="ico-titulo-2"></i>
        <h1>
            Ingreso/Modificación de permisos a roles
        </h1>
        <br />
    </div>
    <%--<div class=" msg-alerta">
        <span id="dtlo" runat="server">Estimado usuario:</span>
        <br />
        ???? <strong>??? </strong>???
    </div>--%>
    <asp:UpdatePanel ID="udpPermisos" runat="server">
        <ContentTemplate>
            <div class="seccion" id="PERSONAL">
                <div class="informativo">
                    <table>
                        <tr>
                            <td rowspan="2" class="inum">
                                <div class="number">
                                    1</div>
                            </td>
                            <td class="level1">
                                Asignación de permisos
                            </td>
                        </tr>
                        <tr>
                            <td class="level2">
                                Asigna los permisos que tendrá asociado el siguiente rol
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
                                Datos de asignación.
                            </th>
                        </tr>
                        <tr>
                            <td class="bt-bottom  bt-right bt-left">
                                1. Rol a asignar:
                            </td>
                            <td class="bt-bottom">
                                <asp:DropDownList ID="ddlGrupo" runat="server" Width="205px" AppendDataBoundItems="true"
                                    onblur="opcionrequerida(this,0,'valGrupo');">
                                </asp:DropDownList>
                            </td>
                            <td class="bt-bottom bt-right validacion">
                                <span class="validacion" id="valGrupo">* obligatorio</span>
                            </td>
                        </tr>
                        <tr>
                            <td class="bt-bottom  bt-right bt-left">
                                2. Área a asignar:
                            </td>
                            <td class="bt-bottom">
                                <asp:DropDownList ID="ddlArea" runat="server" Width="205px" AppendDataBoundItems="true"
                                    onblur="opcionrequerida(this,0,'valArea');">
                                </asp:DropDownList>
                            </td>
                            <td class="bt-bottom bt-right validacion">
                                <span class="validacion" id="valArea">* obligatorio</span>
                            </td>
                        </tr>
                    </table>
                    <div class="botonera">
                        <asp:UpdateProgress AssociatedUpdatePanelID="udpPermisos" ID="updateProgress" runat="server">
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
                        &nbsp;
                        <asp:Button ID="btnLimpiar" Text="Limpiar" runat="server" OnClick="btnLimpiar_Click" />
                        &nbsp;
                    </div>
                    <div class="msg-alerta" id="alerta" runat="server">
                    </div>
                    <div id="error" runat="server" class="msg-critico" visible="false">
                    </div>
                    <div class="cataresult">
                        <asp:UpdatePanel ID="upresult" runat="server" ChildrenAsTriggers="true">
                            <ContentTemplate>
                                <script type="text/javascript">                                    Sys.Application.add_load(BindFunctions);  </script>
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
                                                        <table id="tblCustomers" cellspacing="1" cellpadding="1" class="tabRepeat">
                                                            <thead>
                                                                <tr>
                                                                    <th style="display: none;">
                                                                        idServicio
                                                                    </th>
                                                                    <th style="display: none;">
                                                                        idGrupo
                                                                    </th>
                                                                    <th style="display: none;">
                                                                        idOpcion
                                                                    </th>
                                                                    <th>
                                                                        Opción
                                                                    </th>
                                                                    <th>
                                                                        Descripción
                                                                    </th>
                                                                    <th>
                                                                        <asp:CheckBox ID="chkHeader" runat="server" />
                                                                    </th>
                                                                </tr>
                                                            </thead>
                                                            <tbody>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <tr class="point">
                                                            <td style="display: none;">
                                                                <asp:Label ID="lblIdServicio" runat="server" Text='<%#Eval("idServicio")%>'></asp:Label>
                                                            </td>
                                                            <td style="display: none;">
                                                                <asp:Label ID="lblIdGrupo" runat="server" Text='<%#Eval("idGrupo")%>'></asp:Label>
                                                            </td>
                                                            <td style="display: none;">
                                                                <asp:Label ID="lblIdOpcion" runat="server" Text='<%#Eval("idOpcion")%>'></asp:Label>
                                                            </td>
                                                            <td>
                                                                <%#Eval("nombreOpcion")%>
                                                            </td>
                                                            <td>
                                                                <%#Eval("descripcion")%>
                                                            </td>
                                                            <td>
                                                                <asp:CheckBox ID="chkRow" runat="server" Checked='<%#Convert.ToBoolean(Eval("permiso"))%>' />
                                                            </td>
                                                        </tr>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        </tbody> </table>
                                                    </FooterTemplate>
                                                </asp:Repeater>
                                            </div>
                                        </div>
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
                    <div class="botonera" id="divbotoneraguardar" runat="server">
                        <asp:UpdateProgress AssociatedUpdatePanelID="udpPermisos" ID="updateProgress1" runat="server">
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
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <script src="../Scripts/pages.js" type="text/javascript"></script>
    <script src="../Scripts/pages_mod.js" type="text/javascript"></script>
    <script type="text/javascript">



        var ced_count = 0;
        var jAisv = {};
        $(window).load(function () {
            //objeto a transportar.
            $(document).ready(function () {

                //colapsar y expandir
                $("div.colapser").toggle(function () { $(this).removeClass("colapsa").addClass("expande"); $(this).next().hide(); }
                                  , function () { $(this).removeClass("expande").addClass("colapsa"); $(this).next().show(); });

            });
        });
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
