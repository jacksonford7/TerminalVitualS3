<%@ Page Title="Usuarios" Language="C#" MasterPageFile="~/site.Master" AutoEventWireup="true"
    CodeBehind="datosusuario.aspx.cs" Inherits="CSLSite.datosusuario" %>

<%@ MasterType VirtualPath="~/site.Master" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="placehead" runat="server">
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
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="placebody" runat="server">
    <asp:ToolkitScriptManager ID="tkcal" EnableScriptGlobalization="true" runat="server">
    </asp:ToolkitScriptManager>
    <input id="zonaid" type="hidden" value="910" />
    <noscript>
        <meta http-equiv="refresh" content="0; url=sinsoporte.htm" />
    </noscript>
    <div>
        <i class="ico-titulo-1"></i>
        <h2>
            Usuarios de acceso al sistema
        </h2>
        <br />
        <i class="ico-titulo-2"></i>
        <h1>
            Ingreso/Modificación de Usuarios
        </h1>
        <br />
    </div>
    
    <asp:UpdatePanel ID="updUsuario" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <script type="text/javascript">                Sys.Application.add_load(BindFunctions); </script>
            <div class="seccion" id="SESION">
                <div class="informativo">
                    <table>
                        <tr>
                            <td rowspan="2" class="inum">
                                <div class="number">
                                    1</div>
                            </td>
                            <td class="level1">
                                Ingreso/Modificación de Datos de Sesión
                            </td>
                        </tr>
                        <tr>
                            <td class="level2">
                                Ingrese o modifique los datos de sesión del usuario.
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
                                Datos de Sesión
                            </th>
                        </tr>
                        <tr>
                            <td class="bt-bottom  bt-right bt-left">
                                1. Username:
                            </td>
                            <td class="bt-bottom ">
                                <asp:TextBox ID="txtUsuario" runat="server"  onkeypress="return soloLetras(event,'abcdefghijklmnñopqrstuvwxyz1234567890_',true)"
                                    MaxLength="20"  onblur="cadenareqerida(this,1,20,'valsel1');"
                                    placeholder="Usuario" Width="200px" autocomplete="off"></asp:TextBox>
                            </td>
                            <td class="bt-bottom bt-right validacion">
                                <span class="validacion" id="valsel1">* obligatorio</span>
                            </td>
                        </tr>
                        <tr>
                            <td class="bt-bottom  bt-right bt-left">
                                2. Contraseña:
                            </td>
                            <td class="bt-bottom">
                            <input type="text" name="prevent_autofill" id="prevent_autofill" value="" style="display:none;" />
                            <input type="password" name="password_fake" id="password_fake" value="" style="display:none;" />

                                <asp:TextBox ID="txtPassword" runat="server"  TextMode="Password"
                                    onkeypress="return soloLetras(event,'abcdefghijklmnñopqrstuvwxyz1234567890',true)"
                                    MaxLength="50" onpaste="return false;" onblur="cadenareqerida(this,1,50,'valcon');"
                                    placeholder="Contraseña" Width="200px"  autocomplete="new-password" ></asp:TextBox>
                            </td>
                            <td class="bt-bottom bt-right validacion">
                                <span class="validacion" id="valcon">* obligatorio</span>
                            </td>
                        </tr>
                        <tr>
                            <td class="bt-bottom  bt-right bt-left">
                                3. Tipo de Usuario:
                            </td>
                            <td class="bt-bottom">
                                <asp:DropDownList ID="ddlTipoUsuario" runat="server" Width="205px" AppendDataBoundItems="true"
                                    onblur="opcionrequerida(this,0,'valTipoUsuario');">
                                </asp:DropDownList>
                            </td>
                            <td class="bt-bottom bt-right validacion">
                                <span class="validacion" id="valTipoUsuario">* obligatorio</span>
                            </td>
                        </tr>
                        <tr>
                            <td class="bt-bottom  bt-right bt-left">
                                4. Estado:
                            </td>
                            <td class="bt-bottom bt-right" colspan="3">
                                <asp:DropDownList ID="ddlEstado" runat="server" Width="205px">
                                    <asp:ListItem Text="ACTIVO" Value="A"></asp:ListItem>
                                    <asp:ListItem Text="INACTIVO" Value="I"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
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
                                Ingreso/Modificación de Datos Personales
                            </td>
                        </tr>
                        <tr>
                            <td class="level2">
                                Ingreso/Modificación de datos personales del usuario.
                            </td>
                        </tr>
                    </table>
                </div>
                <div class="colapser colapsa">
                </div>
                <div class="accion">
                    <asp:UpdatePanel ID="updCombos" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <table class="controles" cellspacing="0" cellpadding="1" style="width: 707px;">
                                <tr>
                                    <th class="bt-bottom bt-right bt-top bt-left" colspan="3">
                                        Datos Personales
                                    </th>
                                </tr>
                                <tr>
                                    <td class="bt-bottom  bt-right bt-left" style="width: 230px;">
                                        5. Nombres:
                                    </td>
                                    <td class="bt-bottom">
                                        <asp:TextBox ID="txtUsuarioNombre" runat="server"  
                                        onkeypress="return soloLetras(event,'abcdefghijklmnñopqrstuvwxyz1234567890áéíóú ',true)"
                                            MaxLength="50"  onblur="cadenareqerida(this,1,50,'valnom');"
                                            placeholder="NOMBRES" Width="300px" ></asp:TextBox>
                                    </td>
                                    <td class="bt-bottom bt-right validacion">
                                        <span class="validacion" id="valnom">* obligatorio</span>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="bt-bottom  bt-right bt-left">
                                        6. Apellidos:
                                    </td>
                                    <td class="bt-bottom">
                                        <asp:TextBox ID="txtUsuarioApellido" 
                                        runat="server" 
                                        onkeypress="return soloLetras(event,'abcdefghijklmnñopqrstuvwxyz1234567890áéíóú ',true)"
                                            MaxLength="50"  onblur="cadenareqerida(this,1,50,'valape');"
                                            placeholder="Apellidos" Width="300px"></asp:TextBox>
                                    </td>
                                    <td class="bt-bottom bt-right validacion">
                                        <span class="validacion" id="valape">* obligatorio</span>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="bt-bottom  bt-right bt-left">
                                        7. Identificación:
                                    </td>
                                    <td class="bt-bottom">
                                        <script language='javascript' type="text/javascript">

                                            var g_identificacionID = '<%=hdfIdentificacion.ClientID%>'

                                        </script>
                                        <asp:HiddenField ID="hdfIdentificacion" runat="server" />
                                        <asp:TextBox ID="txtUsuarioRuc" 
                                        runat="server"  onkeypress="return soloLetras(event,'1234567890',true)"
                                            MaxLength="15" onblur="validarIdentificacion(this,'validIden', document.getElementById(g_identificacionID));"
                                            placeholder="IDENTIFICACION" Width="300px"></asp:TextBox>
                                    </td>
                                    <td class="bt-bottom bt-right validacion">
                                        <span class="validacion" id="validIden">* obligatorio</span>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="bt-bottom  bt-right bt-left">
                                        8. País:
                                    </td>
                                    <td class="bt-bottom bt-right" colspan="3">
                                        <asp:DropDownList ID="ddlPais" runat="server" Width="305px" AppendDataBoundItems="true"
                                            OnSelectedIndexChanged="ddlPais_SelectedIndexChanged" AutoPostBack="true" onchange="refrescar_div();">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="bt-bottom  bt-right bt-left">
                                        9. Provincia:
                                    </td>
                                    <td class="bt-bottom bt-right" colspan="3">
                                        <asp:DropDownList ID="ddlProvincia" runat="server" Width="305px" AppendDataBoundItems="true"
                                            AutoPostBack="true" OnSelectedIndexChanged="ddlProvincia_SelectedIndexChanged"
                                            onchange="refrescar_div();">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="bt-bottom  bt-right bt-left">
                                        10. Ciudad:
                                    </td>
                                    <td class="bt-bottom bt-right" colspan="3">
                                        <asp:DropDownList ID="ddlCiudad" runat="server" Width="305px" AppendDataBoundItems="true"
                                            onchange=" Sys.Application.add_load(BindFunctions);">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="bt-bottom  bt-right bt-left">
                                        11. Email:
                                    </td>
                                    <td class="bt-bottom">
                                        <script language='javascript' type="text/javascript">

                                            var g_correoUsuarioID = '<%=hdfCorreoUsuario.ClientID%>'

                                        </script>
                                        <asp:HiddenField ID="hdfCorreoUsuario" runat="server" />
                                        <asp:TextBox ID="txtEmail" 
                                        runat="server"  onkeypress="return soloLetras(event,'abcdefghijklmnñopqrstuvwxyz1234567890_-$@.',true)"
                                            MaxLength="50"  onblur="validarEmail(this,'valemailusu', document.getElementById(g_correoUsuarioID));"
                                            placeholder="MAIL@MAIL.COM" Width="300px"></asp:TextBox>
                                    </td>
                                    <td class="bt-bottom bt-right validacion">
                                        <span class="validacion" id="valemailusu">* obligatorio</span>
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="ddlPais" />
                            <asp:AsyncPostBackTrigger ControlID="ddlProvincia" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="seccion" id="EMPRESA">
                <div class="informativo">
                    <table>
                        <tr>
                            <td rowspan="2" class="inum">
                                <div class="number">
                                    3</div>
                            </td>
                            <td class="level1">
                                Ingreso/Modificación de Datos de Empresa
                            </td>
                        </tr>
                        <tr>
                            <td class="level2">
                                Ingreso/Modificación de los datos de la empresa relacionada al usuario
                            </td>
                        </tr>
                    </table>
                </div>
                <div class="colapser colapsa">
                </div>
                <div class="accion" id="ADU">
                    <asp:UpdatePanel ID="updDatosEmpresa" runat="server">
                        <ContentTemplate>
                            <table class="controles" cellspacing="0" cellpadding="1" style="width: 100%;">
                                <tr>
                                    <th class="bt-bottom bt-right bt-top bt-left" colspan="5">
                                        Datos de la Empresa
                                    </th>
                                </tr>
                                <tr>
                                    <td class="bt-bottom  bt-right bt-left">
                                        12. Empresa:
                                    </td>
                                    <td class="bt-bottom">
                                        <span id="empresa" class="caja" style="width: 200px;">...</span>
                                        <%--<asp:DropDownList ID="ddlEmpresa" runat="server" Width="305px" OnSelectedIndexChanged="ddlEmpresa_SelectedIndexChanged"
                                    AutoPostBack="true">
                                    <asp:ListItem Selected="True" Value="0">EMPRESA</asp:ListItem>
                                </asp:DropDownList>--%>
                                    </td>
                                    <td class="bt-bottom finder">
                                        <div id="td_buscar" runat="server">
                                            <a class="topopup" target="popup" onclick="window.open('../catalogo/empresas','name','width=850,height=510')">
                                                <i class="ico-find"></i>Buscar </a>
                                        </div>
                                    </td>
                                    <td class="bt-bottom bt-right validacion">
                               
                                    </td>
                                </tr>
                                <tr>
                                    <td class="bt-bottom  bt-right bt-left">
                                        13. Identificación de la Empresa:
                                    </td>
                                    <td class="bt-bottom" colspan="2">
                                        <asp:TextBox ID="txtEmpresaIdentificacion" runat="server" Width="300px" onkeydown="return false;"></asp:TextBox>
                                    </td>
                                    <td class="bt-bottom bt-right validacion">
                                        <span class="validacion" id="valempid">* obligatorio</span>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="bt-bottom  bt-right bt-left">
                                        14. Nombre de la Empresa:
                                    </td>
                                    <td class="bt-bottom" colspan="2">
                                        <asp:TextBox ID="txtEmpresaNombre" runat="server" onkeypress="return soloLetras(event,'abcdefghijklmnñopqrstuvwxyz1234567890 .',true)"
                                            MaxLength="255" 
                                            placeholder="Nombre" Width="300px"></asp:TextBox>
                                    </td>
                                    <td class="bt-bottom bt-right validacion">
                                       
                                    </td>
                                </tr>
                                <tr>
                                    <td class="bt-bottom  bt-right bt-left">
                                        15. Dirección de la Empresa:
                                    </td>
                                    <td class="bt-bottom bt-right" colspan="3">
                                        <asp:TextBox ID="txtEmpresaDireccion" runat="server"  onkeypress="return soloLetras(event,'abcdefghijklmnñopqrstuvwxyz1234567890áéíóú -/#.',true)"
                                            MaxLength="255"  placeholder="Direccion" Width="300px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="bt-bottom  bt-right bt-left">
                                        16. Teléfono de la Empresa:
                                    </td>
                                    <td class="bt-bottom bt-right" colspan="3">
                                        <asp:TextBox ID="txtEmpresaTelefono" runat="server"  onkeypress="return soloLetras(event,'abcdefghijklmnñopqrstuvwxyz1234567890-/ ',true)"
                                            MaxLength="20"  placeholder="Telefono" Width="300px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr class="nover" >
                                    <td class="bt-bottom  bt-right bt-left">
                                        00. Fax de la Empresa:
                                    </td>
                                    <td class="bt-bottom bt-right" colspan="3">
                                        <asp:TextBox ID="txtEmpresaFax" runat="server"  onkeypress="return soloLetras(event,'abcdefghijklmnñopqrstuvwxyz1234567890-/ ',true)"
                                            MaxLength="20" placeholder="FAX" Width="300px" Enabled="False">000</asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="bt-bottom  bt-right bt-left">
                                        17. Correo E-Billing:
                                    </td>
                                    <td class="bt-bottom bt-right" colspan="3">
                                        <asp:TextBox ID="txtEmpresaWebSite" runat="server"  onkeypress="return soloLetras(event,'abcdefghijklmnñopqrstuvwxyz1234567890- ./@_',true)"
                                            MaxLength="50"  placeholder="WEBSITE" Width="300px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="bt-bottom  bt-right bt-left">
                                        18. Correo SNA:
                                    </td>
                                    <td class="bt-bottom" colspan="2">
                                        <div id='Div1'>
                                            <div id="Div2" class="cntmail">
                                                <script language='javascript' type="text/javascript">

                                                    var g_correoEmpresaID = '<%=hdfCorreoEmpresa.ClientID%>'
                                                    $("txtEmpresaIdentificacion").keydown(false);
                                                </script>
                                                <asp:HiddenField ID="hdfCorreoEmpresa" runat="server" />
                                                <asp:TextBox ID='txtEmpresaCorreo' runat="server" Style="width: 300px;" 
                                                onkeypress="return soloLetras(event,'abcdefghijklmnñopqrstuvwxyz1234567890- _$@.',true)"
                                                   
                                                    placeholder="mail@mail.com" MaxLength="50" />
                                            </div>
                                        </div>
                                    </td>
                                    <td class="bt-bottom bt-right validacion ">
                                      
                                    </td>
                                </tr>
                            </table>
                            <div class="botonera">
                                <asp:UpdateProgress AssociatedUpdatePanelID="updUsuario" ID="updateProgress" runat="server">
                                    <ProgressTemplate>
                                        <div id="progressBackgroundFilter">
                                        </div>
                                        <div id="processMessage">
                                            Estamos procesando la tarea que solicitó, por favor espere...
                                            <img alt="Loading" src="../shared/imgs/loader.gif" style="margin: 0 auto;" />
                                        </div>
                                    </ProgressTemplate>
                                </asp:UpdateProgress>
                                <asp:Button ID="btnGuardar" runat="server" Text="Guardar" OnClick="btnGuardar_Click" />
                                &nbsp;
                                <asp:Button ID="btnLimpiar" runat="server" Text="Limpiar" OnClick="btnLimpiar_Click" />
                                &nbsp;
                                <asp:Button ID="btnRegresar" runat="server" Text="Regresar" OnClick="btnRegresar_Click" />
                            </div>
                            <div class="msg-alerta" id="alerta" runat="server">
                            </div>
                            <div id="error" runat="server" class="msg-critico" visible="false">
                            </div>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnGuardar" />
                        </Triggers>
                    </asp:UpdatePanel>
                    <div id="secciones">
                        Secciones:&nbsp; <a href="#SESION">Datos de Sesión</a> <a href="#PERSONAL">Datos Personales</a>
                        <a href="#EMPRESA">Datos de la Empresa</a>
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
                //inicia los fecha-hora


            });
        });
        //Esta funcion va a validar que cuando presionen booking debe poner los 3 parametros
        function validateBook(objeto) {

        }

        function refrescar_div() {
            Sys.Application.add_load(BindFunctions);
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

        function btclear_onclick() {

        }

        function setDataEmpresa(nombre, identificacion, direccion, fax, telefono, correo, website) {
            $("#<%=txtEmpresaIdentificacion.ClientID%>").val(identificacion); //set value
            $("#<%=txtEmpresaNombre.ClientID%>").val(nombre); //set value
            $("#<%=txtEmpresaDireccion.ClientID%>").val(direccion); //set value
            $("#<%=txtEmpresaTelefono.ClientID%>").val(telefono); //set value
            $("#<%=txtEmpresaFax.ClientID%>").val(fax); //set value
            $("#<%=txtEmpresaWebSite.ClientID%>").val(website); //set value
            $("#<%=txtEmpresaCorreo.ClientID%>").val(correo); //set value

        }

    </script>
</asp:Content>
