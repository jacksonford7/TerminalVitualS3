<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="aceptar.aspx.cs"
    Inherits="CSLSite.aceptar" %>

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
        <form id="fmrinset" runat="server" class="form-signin" method="post" accept-charset="UTF-8">
        <div id='cargando'>
        </div>

        <div id="wall-panel">
            <div class="xlogin">
                <div class="wraper-line">
                    <span class="loginame" id="namelogin" runat="server"></span>
                    <asp:Button ID="closebt" runat="server" class="btn btn-lg btn-primary btn-block" Text="Salir"
                         OnClick="btexit_Click" />
                </div>
            </div>
            <div id="wraper-panel">
                <br/>
                 <img class="mb-4" src="../img/logo_02.jpg" alt="" />
               

                <div class="right">
                    <asp:ToolkitScriptManager ID="tkcal" EnableScriptGlobalization="true" runat="server">
                    </asp:ToolkitScriptManager>
                    <input id="zonaid" type="hidden" value="502" />
                   
                    <asp:UpdatePanel ID="updConsultaUsuarios" runat="server">
                        <ContentTemplate>
                             
                                  <h1 class="h3 mb-3 font-weight-normal">Gracias por su confianza.</h1>


                               <%-- <div id="error" runat="server" class="alert alert-danger" visible="false">
                                </div>--%>
                                <div class="alert alert-warning" id="alerta" runat="server">
                                </div>
                         
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <script src="../Scripts/pages.js" type="text/javascript"></script>
                    <script src="../Scripts/pages_mod.js" type="text/javascript"></script>
                 
             
                </div>
                 <div class="row">
                  <span class="text-muted col-md-12">Cualquier inquietud puede ser remitida a <a href="mailto:ec.sac@contecon.com.ec"> ec.sac@contecon.com.ec.</a> Nos es grato poder servirle.</span>
              
                </div>
            </div>
        </div>
        </form>
    </div>
  </div>

 
</body>
</html>
