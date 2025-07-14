<%@ Page Title="Usuarios" Language="C#" MasterPageFile="~/site.Master" AutoEventWireup="true"
    CodeBehind="consultausuario.aspx.cs" Inherits="CSLSite.consultausuario" %>

<%@ MasterType VirtualPath="~/site.Master" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="placehead" runat="server">
    <link href="../shared/estilo/tables.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/aisv-general.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/work.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/catalogos.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/info.css" rel="stylesheet" type="text/css" />
    <link href="../../shared/estilo/chosen.css" rel="stylesheet" type="text/css" />
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
    <input id="zonaid" type="hidden" value="901" />
    <noscript>
        <meta http-equiv="refresh" content="0; url=sinsoporte.htm" />
    </noscript>
    <asp:UpdatePanel ID="updConsultaUsuarios" runat="server">
        <ContentTemplate>
            <div>
                <i class="ico-titulo-1"></i>
                <h2>
                    Consulta de usuarios
                </h2>
                <br />
                <i class="ico-titulo-2"></i>
                <h1>
                    Consulta de usuarios
                </h1>
                <br />
            </div>
            
            <div class="seccion" id="PERSONAL">
                <div class="informativo">
                    <table>
                        <tr>
                            <td rowspan="2" class="inum">
                                <div class="number">
                                    1</div>
                            </td>
                            <td class="level1">
                                Consulta de usuarios
                            </td>
                        </tr>
                        <tr>
                            <td class="level2">
                                Consulta de usuarios
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
                                Criterios de búsqueda.
                            </th>
                        </tr>
                        <tr>
                            <td class="bt-bottom  bt-right bt-left">
                                1. Usuario:
                            </td>
                            <td class="bt-bottom bt-right" colspan="3">
                                <asp:TextBox ID="txtUsuario" runat="server"  
                               
                                    MaxLength="20" placeholder="USUARIO" 
                                    Width="200px" onkeypress="return soloLetras(event,'abcdefghijklmnñopqrstuvwxyz1234567890_',true)"></asp:TextBox>
                                <asp:HiddenField ID="hdfIdUsuario" runat="server" />
                                <asp:HiddenField ID="hdfUserName" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="bt-bottom  bt-right bt-left">
                                2. Nombre del Usuario:
                            </td>
                            <td class="bt-bottom bt-right" colspan="3">
                                <asp:TextBox ID="txtNombres" runat="server"  
                                 onkeypress="return soloLetras(event,'abcdefghijklmnñopqrstuvwxyz1234567890')"
                                    MaxLength="100" placeholder="NOMBRE" Width="200px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="bt-bottom  bt-right bt-left">
                                3. Identificación:
                            </td>
                            <td class="bt-bottom bt-right" colspan="3">
                                <asp:TextBox ID="txtIdentificacion" runat="server"  
                                    placeholder="IDENTIFICACION" MaxLength="15" Width="200px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="bt-bottom  bt-right bt-left">
                                4. Empresa:
                            </td>
                            <td class="bt-bottom bt-right" colspan="3">
                                <asp:TextBox ID="txtNombreEmpresa" 
                                runat="server"   
                                onkeypress="return soloLetras(event,'abcdefghijklmnñopqrstuvwxyz1234567890',true)"
                                    MaxLength="255" placeholder="EMPRESA" Width="200px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="bt-bottom  bt-right bt-left">
                                5. Tipo de Usuario:
                            </td>
                            <td class="bt-bottom bt-right" colspan="3">
                                <asp:DropDownList ID="ddlTipoUsuario" runat="server" Width="205px" AppendDataBoundItems="true">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td class="bt-bottom  bt-right bt-left">
                                6. Estado:
                            </td>
                            <td class="bt-bottom bt-right" colspan="3">
                                <asp:DropDownList ID="ddlEstado" runat="server" Width="205px">
                                    <asp:ListItem Text="TODOS" Value="0"></asp:ListItem>
                                    <asp:ListItem Text="ACTIVO" Value="A"></asp:ListItem>
                                    <asp:ListItem Text="INACTIVO" Value="I"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                    <div class="botonera">
                        <asp:UpdateProgress AssociatedUpdatePanelID="updConsultaUsuarios" ID="updateProgress"
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
                        <asp:Button ID="Button1" Text="Crear Usuario" runat="server" OnClick="Button1_Click" />
                        <asp:Button ID="btbuscar" Text="Iniciar búsqueda" runat="server" OnClick="btbuscar_Click" />
                        <asp:Button ID="btSetearId" Text="Iniciar búsqueda" runat="server" Style="display: none;"
                            OnClick="btSetearId_Click" />
                        <asp:Button ID="btResetear" Text="Iniciar búsqueda" runat="server" Style="display: none;"
                            OnClick="btResetear_Click" />
                        &nbsp;
                        
                    </div>
                    <div id="error" runat="server" class="msg-critico" visible="false">
                    </div>
                    <div class="cataresult">
                        <asp:UpdatePanel ID="upresult" runat="server" ChildrenAsTriggers="true">
                            <ContentTemplate>
                                <script type="text/javascript">                                    Sys.Application.add_load(BindFunctions); </script>
                                <input id="idlin" type="hidden" runat="server" clientidmode="Static" />
                                <input id="diponible" type="hidden" runat="server" clientidmode="Static" />
                                <div class="msg-alerta" id="alerta" runat="server">
                                </div>
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
                                                                        idUsuario
                                                                    </th>
                                                                    <th>
                                                                        Usuario
                                                                    </th>
                                                                    <th>
                                                                        Nombres
                                                                    </th>
                                                                    <th>
                                                                        Identificación
                                                                    </th>
                                                                    <th>
                                                                        Empresa
                                                                    </th>
                                                                    <th style="display: none;">
                                                                        Email
                                                                    </th>
                                                                    <th>
                                                                        Tipo de Usuario
                                                                    </th>
                                                                    <th>
                                                                        Estado
                                                                    </th>
                                                                    <th>
                                                                    </th>
                                                                    <th>
                                                                    </th>
                                                                </tr>
                                                            </thead>
                                                            <tbody>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <tr class="point"  id="trUsuario">
                                                            <td style="display: none;">
                                                                <%#Eval("idUsuario")%>
                                                            </td>
                                                            <td>
                                                                <%#Eval("usuario")%>
                                                            </td>
                                                            <td>
                                                                <%#Eval("nombres")%>
                                                            </td>
                                                            <td>
                                                                <%#Eval("usuarioIdentificacion")%>
                                                            </td>
                                                            <td>
                                                                <%#Eval("nombreEmpresa")%>
                                                            </td>
                                                            <td style="display: none;">
                                                                <%#Eval("usuarioCorreo")%>
                                                            </td>
                                                            <td>
                                                                <%#Eval("desTipoUsuario")%>
                                                            </td>
                                                            <td>
                                                                <%#Eval("estado")%>
                                                            </td>
                                                            <td>
                                                                <div class="tcomand">
                                                                    <input type="button" id="btVerUsusario" value="Modificar" tooltip="Carga la información del usuario para modificarla"
                                                                        onclick="setUser('<%#Eval("idUsuario")%>')" />
                                                                </div>
                                                            </td>
                                                            <td>
                                                                <div class="tcomand">
                                                                    <input type="button" id="btRestablecer" class="Anular" value="Reestablecer Contraseña"
                                                                        ToolTip="Reestablecer contraseña del usuario" onclick="resetPassword('<%#Eval("idUsuario")%>')" />
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
        function setUser(user) {
            
            $("#<%=hdfIdUsuario.ClientID%>").val(user); //set value
            $("#<%=btSetearId.ClientID%>").click(); //set value
            location.href = "../seguridad/datosusuario";
        }

        function resetPassword(user) {

            var r = confirm("¿Está seguro que desea reestablecer la contraseña del usuario seleccionado?");
            if (r == true) {
                $("#<%=hdfIdUsuario.ClientID%>").val(user); //set value
                $("#<%=btResetear.ClientID%>").click(); //set value
            } 
           
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
