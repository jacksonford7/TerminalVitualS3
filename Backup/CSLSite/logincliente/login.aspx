<%@ Page Language="C#" AutoEventWireup="true" Title="Login"
CodeBehind="login.aspx.cs" Inherits="CSLSite.logincliente.login" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Login</title>
    <link href="../shared/estilo/catalogosolicitudempresa.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/work.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/aisv-general.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/tables.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/info.css" rel="stylesheet" type="text/css" />
    <script src="Scripts/jquery-1.7.1.min.js" type="text/javascript"></script>
    <script src="../Scripts/jquery-1.7.1.min.js" type="text/javascript"></script>
    <link href="Inicio/base-site.css" rel="stylesheet" type="text/css" />
    <link href="Inicio/css/font-awesome.min.css" rel="stylesheet" type="text/css" />
    <link rel="shortcut icon" type="image/x-icon" href="favicon.ico" />
    <link href="shared/estilo/work.css" rel="stylesheet" type="text/css" />
    <link href="../Inicio/base-site.css" rel="stylesheet" type="text/css" />
    <link href="../Inicio/css/font-awesome.min.css" rel="stylesheet" type="text/css" />
    <link rel="../shortcut icon" type="image/x-icon" href="../favicon.ico" />
    <link href="../shared/estilo/work.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
     * input[type=text]
        {
            text-align:left!important;
        }
    </style>
    <script type="text/javascript">
        function BindFunctions() {
            document.getElementById('imagen').innerHTML = '';
            $(document).ready(function () {
                $('#tablasort').tablesorter({ cancelSelection: true, cssAsc: "ascendente", cssDesc: "descendente", headers: { 5: { sorter: false }, 6: { sorter: false}} }).tablesorterPager({ container: $('#pager'), positionFixed: false, pagesize: 10 });
            });
        }
    </script>
</head>
<body>
    <noscript><meta http-equiv="refresh" content="0; url=sinsoporte.htm" /> </noscript>
    <form id="bookingfrm" runat="server">
    <div id="wall-panel">
    <div id="wraper-panel">
    <div id="logoCGSA"> 
    </div>
      <div id="menu-panel">
       <p class="calendar" > <span id="dia">00</span><em id="mesx">Mes</em></p>
      <br />
      Sistema de Solicitud de Servicios - S3
      
      <hr />
      </div>
      <div>
       <div class="left borde-all" >
        <i class="element"></i> <span class="icon-text">Estimado Cliente:</span>
        <h3></h3>
        <p>
              Ingrese el usuario y clave temporal enviado en el mail, 
              para poder validar los 
              datos y los motivos del rechazo de la solicitud.</p>

        <fieldset style=" border:white">
        <br />
        <ul style="display:none">
         <li>Contenedores Llenos <a href="http://www.cgsa.com.ec/files/ZonaDescarga/CSL/videos/AISV-CONTENEDOR-LLENO.zip" target="_blank">VIDEO</a> (3516Kb) <a href="http://www.cgsa.com.ec/Files/ZonaDescarga/CSL/GUIA-DE-AISV-DE-CONTENEDOR-LLENO.pdf" target="_blank">PDF</a> (913 Kb)</li>
         <li>Contenedores Vacíos <a href="http://www.cgsa.com.ec/files/ZonaDescarga/CSL/videos/AISV-DE-CONTENEDOR-VACIO.zip" target="_blank">VIDEO</a> (1752Kb) </li>
         <li>Carga Suelta (AISV de carga suelta <a href="http://www.cgsa.com.ec/files/ZonaDescarga/CSL/videos/AISV-DE-CARGA-SUELTA.zip" target="_blank">VIDEO</a> (2908Kb) y AISV carga suelta contenedor de acopio <a href="http://www.cgsa.com.ec/files/ZonaDescarga/CSL/videos/AISV-DE-CARGA-SUELTA-(CONT-DE-ACOPIO).zip" target="_blank">VIDEO</a> (3371Kb) ) <a href="http://www.cgsa.com.ec/Files/ZonaDescarga/CSL/GUIA-DE-AISV-DE-CARGA-SUELTA.pdf" target="_blank">PDF</a> (973 Kb) </li>
         <li>Carga a Consolidar (AISV de carga a consolidar) <a href="http://www.cgsa.com.ec/files/ZonaDescarga/CSL/videos/AISV-DE-CARGA-A-CONSOLIDAR.zip" target="_blank">VIDEO</a> (2940Kb) y AISV de carga a consolidar contenedores de acopio <a href="http://www.cgsa.com.ec/files/ZonaDescarga/CSL/videos/AISV-CARGA-A-CONSOLIDAR-(ACOPIO).zip" target="_blank">VIDEO</a> (2821Kb) ) <a href="http://www.cgsa.com.ec/Files/ZonaDescarga/CSL/GUIA-DE-AISV-DE-CARGA-A-CONSOLIDAR.pdf" target="_blank">PDF</a> (973 Kb)</li>
         <li>AISV Consolidadoras <a href="http://www.cgsa.com.ec/files/ZonaDescarga/CSL/videos/AISV_CONSOLIDADORAS.zip" target="_blank">VIDEO</a>(4443Kb)</li>
        </ul>
        </fieldset>

      </div>
        <div class="display" style="height:268px">
      <div  class="right borde-all">
           <p id="xtoper">Inicio de sesión</p>
           <div id="passlogin" >&nbsp;&nbsp;
           Escriba su nombre de usuario y contraseña
                    <table cellspacing="2" cellpadding="1" class="tabla">
                    <tr><td class="centrar">
                        <asp:TextBox placeholder="nombre de usuario" ID="user"  autofocus="autofocus"  ClientIDMode="Static" runat="server" Width="80%"
                     onkeypress="return soloLetras(event,'1234567890abcdefghijklmnñopqrstuvwxyz-_',true)"
                    onblur="cadenareqerida(this,1,50,'valuse');" MaxLength="50"
                    ></asp:TextBox> 
                   
                    </td></tr>
                    <tr><td class="centrar"> <span id='valuse' class="validacion"></span></td></tr>
                    <tr><td class="centrar"> 
                        <asp:TextBox  placeholder="clave" ID="pass" runat="server"
                            onblur="cadenareqerida(this,1,500,'valpas');" ClientIDMode="Static" 
                            TextMode="Password" Width="80%" MaxLength="500"></asp:TextBox>
                            </td>
                           </tr>
                           <tr><td class="centrar"><span id="valpas" class="validacion"></span></td></tr>
                   
                   
                     <tr><td colspan="2" class="centrar" >
                     <span id="imagen"></span>
                         <asp:Button ID="btseguir" runat="server" Text="Continuar" Width="200px" 
                        
                             onclick="btstart_Click" OnClientClick="return validar();" />
                         </td></tr> 
                       <tr>     <td colspan="2" class="centrar" >
                       <div class="msg-alerta" id="banmsg" runat="server" clientidmode="Static">Falló el nombre de usuario y contraseña</div>
                       </td></tr>
                   </table>
                   <br />
           </div>
           <br />
           <br />
           <div class="demotext">
                             Si usted no recuerda su usuario/contraseña, 
                             favor revise el correo enviado por el Departamento de Credenciales.
                   </div>
         </div>
      </div>
      </div>
      <div id="foot-panel">
        <table class="tabla" cellpadding="1" cellspacing ="1">
        <tr>
        <td class="nota-pie">
        <p class="al-left ">
                   Cualquier inquietud puede ser remitida a  <a href="mailto: CGSA-CustomerService@cgsa.com.ec">CGSA-CustomerService@cgsa.com.ec.</a> Nos es grato poder servirle. 
                  
        </p>
        </td>
        <td class="nota-pie">
           <p class="al-right">
           © 2014 CONTECON S.A
           </p>
        </td>
        </tr>
        </table>
      <br />
      </div>
    </div>
    </div>
    </form>
    <script src="../../Scripts/pages.js" type="text/javascript"></script>
    <script src="../../Scripts/credenciales.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.7.1.min.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery.tablesorter.min.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery.tablesorter.pager.js" type="text/javascript"></script>
    <script type="text/javascript" >
        (function ($) {
            $("#header").ready(function () { $("#cargando").stop().animate({ width: "25%" }, 1500) });
            $("#footer").ready(function () { $("#cargando").stop().animate({ width: "75%" }, 1500) });
            $(window).load(function () {
                $("#cargando").stop().animate({ width: "100%" }, 600, function () {
                    $("#cargando").fadeOut("fast", function () { $(this).remove(); });
                });
            });
        })($);
        //]]>
        /*Esto solo es para mostrar el calendario con el día actual*/
        $(document).ready(function () {
            var f = new Date();
            var meses = new Array("ENERO", "FEBRERO", "MARZO", "ABRIL", "MAYO", "JUNIO", "JULIO", "AGOSTO", "SEPTIEMBRE", "OCTUBRE", "NOVIEMBRE", "DICIEMBRE");
            document.getElementById('dia').innerHTML = f.getDate();
            document.getElementById('mesx').innerHTML = meses[f.getMonth()];
        });

        function validar() {
            if (document.getElementById('user').value.trim().length <= 0) {
                //document.getElementById('banmsg').innerHTML = "Escriba el nombre de usuario y contraseña.";
                alert('Por favor complete la información requerida, digite el usuario.!');
                document.getElementById('user').focus();
                return false;
            }
            if (document.getElementById('pass').value.trim().length <= 0) {
                //document.getElementById('banmsg').innerHTML = "Escriba el nombre de usuario y contraseña.";
                alert('Por favor complete la información requerida, digite la clave.!');
                document.getElementById('pass').focus();
                return false;
            }
            document.getElementById('imagen').innerHTML = '<img alt="loading.."" src="../../shared/imgs/loader.gif"/>';
            return true;
        }
   </script>
</body>
</html>
