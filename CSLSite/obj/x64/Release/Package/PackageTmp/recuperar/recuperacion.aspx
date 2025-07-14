<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="recuperacion.aspx.cs" Inherits="CSLSite.recuperacion" %>


<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server" id="xhead">
    <meta http-equiv="X-UA-Compatible" content="IE=Edge" />
    <title></title>
     <link href="../img/favicon2.png" rel="icon"/>
    <link href="../css/signin.css" rel="stylesheet"/>
      <link href="../img/icono.png" rel="apple-touch-icon"/>
      <link href="../css/bootstrap.min.css" rel="stylesheet"/>
      <link href="../css/dashboard.css" rel="stylesheet"/>
      <link href="../css/icons.css" rel="stylesheet"/>
      <link href="../css/style.css" rel="stylesheet"/>
      <link href="https://fonts.googleapis.com/css2?family=Roboto:wght@300;400;500;700&display=swap" rel="stylesheet"/>

       <!--external css-->
      <link href="../lib/font-awesome/css/font-awesome.css" rel="stylesheet" />
      <link rel="stylesheet" type="text/css" href="../lib/gritter/css/jquery.gritter.css" />

        <!--mensajes-->
      <link href="https://cdn.jsdelivr.net/npm/alertifyjs@1.11.0/build/css/alertify.min.css" rel="stylesheet"/>
      <script type="text/javascript" src="https://cdn.jsdelivr.net/npm/alertifyjs@1.11.0/build/alertify.min.js"></script>
      <script src="../lib/jquery/jquery.min.js" type="text/javascript"></script>

   <%-- <link href="../shared/estilo/tables.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/aisv-general.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/work.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/catalogos.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/info.css" rel="stylesheet" type="text/css" />--%>
    <%--<link href="../shared/estilo/chosen.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/base-site.css" rel="stylesheet" type="text/css" />
    <link href="../Inicio/css/font-awesome.min.css" rel="stylesheet" type="text/css" />
    <link rel="shortcut icon" type="image/x-icon" href="favicon.ico" />
    <link href="../shared/estilo/jquery.datetimepicker.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/jquery-1.7.1.min.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.datetimepicker.js" type="text/javascript"></script>--%>
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
<body class="flex-column sign-in">
    
    <br/>
     <div class="container-login">

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
                     <img class="mb-4" src="../img/logo_02.jpg" alt="" />
                   <%-- <div  class="alert alert-warning" id="mensajes" runat="server" clientidmode="Static">--%>
                    <div class="zona-info">
                       <%-- <div>
                            <i class="ico-info"></i><span class="info-title">Informativo</span>
                            <p id="panel_info" runat="server" clientidmode="Static">
                                Cargando notificaciones..
                            </p>
                        </div>--%>
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
                                <div class="mt-4">         
                                <nav class="mt-4" aria-label="breadcrumb">
                                  <ol class="breadcrumb">
                                  <%--  <li class="breadcrumb-item" id="opcion_principal" runat="server"><a href="#">Reportes</a></li>--%>
                                    <li class="breadcrumb-item active" aria-current="page" id="sub_opcion" runat="server">Recuperación de Contraseña</li>
                                  </ol>
                                </nav>
                              </div>
 
                            </div>
                             <h1 class="h3 mb-3 font-weight-normal"> Datos del Usuario.</h1>
                             <div class="seccion" id="PERSONAL">
                                <div class="text-left"> 
                                     <label for="">Por favor ingrese su username:</label>
                                    <div class="input-group mb-3">
                                      <asp:TextBox ID="txtUsuarioRecuperacion" runat="server" class="form-control" onpaste="return false;"
                                                    MaxLength="30" placeholder="USERNAME"  onkeypress="return soloLetras(event,'01234567890abcdefghijklmnopqrstuvwxyzñ_',true)"
                                                    onblur="cadenareqerida(this,1,30,'valPas');" aria-label="Amount (to the nearest dollar)"></asp:TextBox>
                                        <span class="validacion" id="valPas"></span>
                                    </div>
                               </div>
                               
                                <div class="botonera">
                                   <asp:UpdateProgress AssociatedUpdatePanelID="updConsultaUsuarios" ID="updateProgress"
                        runat="server">
                        <ProgressTemplate>
                            <div id="progressBackgroundFilter">
                            </div>
                            <div id="processMessage">
                                Estamos procesando la tarea que solicitó, por favor espere...
                                <img alt="loading.." src="../lib/file-uploader/img/loading.gif" height="32px" width="32px"  id="ImgCargaDet" class="nover" style="margin: 0 auto;" />
                            </div>
                        </ProgressTemplate>
                    </asp:UpdateProgress>
                                    <asp:Button ID="btbuscar" Text="Recuperar Contraseña" runat="server" class="btn btn-lg btn-primary btn-block"
                                        onclick="btbuscar_Click"  />

                                    &nbsp;  
                                </div>
                                <div id="error" runat="server" class="alert alert-danger" visible="false">
                                </div>
                                <div class="alert alert-danger" id="alerta" runat="server">
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                  
                    <script src="../Scripts/pages.js" type="text/javascript"></script>
                    <script src="../Scripts/pages_mod.js" type="text/javascript"></script>
                   <%-- <script src="../Scripts/jquery-1.7.1.min.js" type="text/javascript"></script>
                    <script src="../Scripts/jquery.tablesorter.min.js" type="text/javascript"></script>
                    <script src="../Scripts/jquery.tablesorter.pager.js" type="text/javascript"></script>--%>
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
                 <div class="row">
                  <span class="text-muted col-md-12">Cualquier inquietud puede ser remitida a <a href="mailto:ec.sac@contecon.com.ec"> ec.sac@contecon.com.ec.</a> Nos es grato poder servirle.</span>
              
                </div>
            </div>
        </div>
        </form>
    </div>
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
