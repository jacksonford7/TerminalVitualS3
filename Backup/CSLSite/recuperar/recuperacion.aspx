<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="recuperacion.aspx.cs" Inherits="CSLSite.recuperacion" %>


<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server" id="xhead">
    <meta http-equiv="X-UA-Compatible" content="IE=Edge" />
    <title></title>
    <link href="../shared/estilo/tables.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/aisv-general.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/work.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/catalogos.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/info.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/chosen.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/base-site.css" rel="stylesheet" type="text/css" />
    <link href="../Inicio/css/font-awesome.min.css" rel="stylesheet" type="text/css" />
    <link rel="shortcut icon" type="image/x-icon" href="favicon.ico" />
    <link href="../shared/estilo/jquery.datetimepicker.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/jquery-1.7.1.min.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.datetimepicker.js" type="text/javascript"></script>
    <script type='text/javascript'>
         //<![CDATA[
        /*funcion para la barra de carga*/
        (function ($) {
            $("#header").ready(function () { $("#cargando").stop().animate({ width: "25%" }, 1500) });
            $("#footer").ready(function () { $("#cargando").stop().animate({ width: "75%" }, 1500) });
            $(window).load(function () {
                $("#cargando").stop().animate({ width: "100%" }, 600, function () {
                    $("#cargando").fadeOut("fast", function () { $(this).remove(); });
                });
            });
        })($);
    </script>
</head>
<body>
    <div id="content">
        <form id="fmrinset" runat="server" method="post" accept-charset="UTF-8">
        <div id='cargando'>
        </div>
        <div id="wall-panel">
            <div class="xlogin">
                <div class="wraper-line">
                    
                </div>
            </div>
            <div id="wraper-panel">
                <div id="logoCGSA">
                </div>
                <br />
                <div class="left">
                   
                    <div class="zona-info">
                        <div>
                            <i class="ico-info"></i><span class="info-title">Informativo</span>
                            <p id="panel_info" runat="server" clientidmode="Static">
                                Cargando notificaciones..
                            </p>
                        </div>
                    </div>
                </div>
                <div class="right">
                    <asp:ToolkitScriptManager ID="tkcal" EnableScriptGlobalization="true" runat="server">
                    </asp:ToolkitScriptManager>
                    <input id="zonaid" type="hidden" value="2" />
                    <noscript>
                        <meta http-equiv="refresh" content="0; url=sinsoporte.htm" />
                    </noscript>
                    <asp:UpdatePanel ID="updConsultaUsuarios" runat="server">
                        <ContentTemplate>
                            <div>
                                <i class="ico-titulo-1"></i>
                                <h2>
                                    Recuperación de Contraseña
                                </h2>
                                <br />
                                <i class="ico-titulo-2"></i>
                                <h1>
                                    Recuperación de Contraseña
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
                                                Recuperación de Contraseña
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="level2">
                                                Recuperación de Contraseña
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
                                                Datos del Usuario.
                                            </th>
                                        </tr>
                                        <tr>
                                            <td class="bt-bottom  bt-right bt-left">
                                                1. Por favor ingrese su username:
                                            </td>
                                            <td class="bt-bottom">
                                                <asp:TextBox ID="txtUsuarioRecuperacion" runat="server" CssClass="mayusc" onpaste="return false;"
                                                    MaxLength="30" placeholder="USERNAME" Width="200px" onkeypress="return soloLetras(event,'01234567890abcdefghijklmnopqrstuvwxyzñ_',true)"
                                                    onblur="cadenareqerida(this,1,30,'valPas');"></asp:TextBox>
                                            </td>
                                            <td class="bt-bottom bt-right validacion">
                                                <span class="validacion" id="valPas">* obligatorio</span>
                                            </td>
                                        </tr>
                                        
                                    </table>
                                </div>
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
                                    <asp:Button ID="btbuscar" Text="Recuperar Contraseña" runat="server" 
                                        onclick="btbuscar_Click"  />
                                    &nbsp;
                                </div>
                                <div id="error" runat="server" class="msg-critico" visible="false">
                                </div>
                                <div class="msg-alerta" id="alerta" runat="server">
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
                </div>
                <div id="foot-panel">
                    <table class="tabla" cellpadding="1" cellspacing="1">
                        <tr>
                            <td class="nota-pie">
                                <p class="al-left ">
                                    Cualquier inquietud puede ser remitida a <a href="mailto: CGSA-CustomerService@cgsa.com.ec">
                                        CGSA-CustomerService@cgsa.com.ec.</a> Nos es grato poder servirle.
                                </p>
                            </td>
                            <td class="nota-pie">
                                <p class="al-right">
                                    © 2014 CONTECON S.A</p>
                            </td>
                        </tr>
                    </table>
                    <br />
                </div>
            </div>
        </div>
        </form>
    </div>
    <script type="text/javascript">
        $(window).load(function () {
            setInterval("AsyncMensenger()", 20000);
            $(document).keypress(function (e) {
                if (e.keyCode === 13) {
                    e.preventDefault();
                    return false;
                }
            });
        });
        window.onbeforeunload = function PostSession() {
            var params = {};
            params.tokenID = '<%=Session["tokenID"] %>';
            params = JSON.stringify(params);
            $.ajax({
                type: "POST",
                url: "../services/catalogo.asmx/PostSesion",
                data: params,
                contentType: "application/json; charset=utf-8",
                async: false
            });
        }
        function AsyncMensenger() {
            var params = {};
            var valor = 1;
            var idzona = document.getElementById('zonaid');
            if (idzona != null && idzona != undefined) {
                valor = idzona.value;
            }
            params.formulario = valor;
            params = JSON.stringify(params);
            $.ajax({
                type: "POST",
                url: "../services/catalogo.asmx/getalertas",
                data: params,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: true,
                success: function (response) {
                    document.getElementById('panel_info').innerHTML = response.d;
                }
            });
        }
    </script>
</body>
</html>
