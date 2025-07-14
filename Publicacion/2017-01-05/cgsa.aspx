<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="cgsa.aspx.cs" Inherits="CSLSite.cgsa" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<meta http-equiv="X-UA-Compatible" content="IE=Edge" />
    <title>
         Página de inicio Contecon S.A
    </title>
    <link href="shared/estilo/ppt.css" rel="stylesheet" type="text/css" />
    <script src="Scripts/jquery-1.7.1.min.js" type="text/javascript"></script>
    <script src="../Scripts/jquery-1.7.1.min.js" type="text/javascript"></script>
    <link href="Inicio/base-site.css" rel="stylesheet" type="text/css" />
    <link href="Inicio/css/font-awesome.min.css" rel="stylesheet" type="text/css" />
    <link rel="shortcut icon" type="image/x-icon" href="favicon.ico" />
    <link href="shared/estilo/work.css" rel="stylesheet" type="text/css" />
    <link href="../Inicio/base-site.css" rel="stylesheet" type="text/css" />
    <link href="../Inicio/css/font-awesome.min.css" rel="stylesheet" type="text/css" />
    <link rel="../shortcut icon" type="image/x-icon" href="favicon.ico" />
    <link href="../shared/estilo/work.css" rel="stylesheet" type="text/css" />
    <link href="shared/estilo/tabs.css" rel="stylesheet" type="text/css" />
    <script src="Scripts/jquery.easytabs.min.js" type="text/javascript"></script>

      <link href="../shared/estilo/tabs.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/jquery.easytabs.min.js" type="text/javascript"></script>
      <script type="text/javascript">
          $(document).ready(function () {
              $('#tab-container').easytabs({   animate: false  }  );    
          });
  </script>
 
</head>
<body>
    <form id="form1" runat="server" autocomplete="off" accept-charset="UTF-8">
 <noscript><meta http-equiv="refresh" content="0; url=sinsoporte.htm" /> </noscript>
<div id='cargando'></div>
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
       <div class="left borde-all" style=" height:520px;" >
        <i class="element"></i> <span class="icon-text">Nuevo sistema de atención a clientes CGSA</span>
             
             <div id="tab-container" class='tab-container'>
             <ul class='etabs'>
               <li class='tab'><a href="#tabs-1">Navieras</a></li>
               <li class='tab'><a href="#tabs-2">Exportadores</a></li>
               <li class='tab'><a href="#tabs-3">Importadores</a></li>
             </ul>
               <div id="tabs-1">
               <ul>
                <li>Creación de Usuarios Operadores.&nbsp; [<a target='blank' href='http://www.cgsa.com.ec/files/guia_tramites/servicio_al_cliente/S3MUsuariosRoles.pdf'>PDF</a> ]</li>
                <li>Administración del Listado de Embarque.</li>
                <li>Solicitud de Revisión Técnica Refrigerada. &nbsp; [<a target='blank' href='http://www.cgsa.com.ec/files/guia_tramites/servicio_al_cliente/rtr_m.pdf'>PDF</a> ]</li>
                <li>Solicitud de Reestiba. &nbsp; [<a target='blank' href='http://www.cgsa.com.ec/files/guia_tramites/servicio_al_cliente/S3MReestiba.pdf'>PDF</a> ]</li>
                <li>Solicitud de Etiquetado y Desetiquetado IMO.&nbsp; [<a target='blank' href='http://www.cgsa.com.ec/files/guia_tramites/servicio_al_cliente/etiquetado_m.pdf'>PDF</a> ]</li>
               </ul>
               </div>
               <div id="tabs-2">
               <ul>
               <li>Creación de Usuarios Operadores.&nbsp; [<a target='blank' href='http://www.cgsa.com.ec/files/guia_tramites/servicio_al_cliente/S3MUsuariosRoles.pdf'>PDF</a> ]</li>
               <li>Correcciones de Ingreso de Exportación.&nbsp; [<a target='blank' href='http://www.cgsa.com.ec/files/guia_tramites/servicio_al_cliente/S3MCorregirIIE.pdf'>PDF</a> ]</li>
               <li>Solicitud de Reestiba. &nbsp; [<a target='blank' href='http://www.cgsa.com.ec/files/guia_tramites/servicio_al_cliente/S3MReestiba.pdf'>PDF</a> ]</li>
			   <li>Solicitud de Repesaje.</li>
			   <li>Solicitud de Verficación de Sellos.</li>
               </ul>
               </div>
               <div id="tabs-3">
               <ul>
               <li>Creación de Usuarios Operadores.&nbsp; [<a target='blank' href='http://www.cgsa.com.ec/files/guia_tramites/servicio_al_cliente/S3MUsuariosRoles.pdf'>PDF</a> ]</li>
			   <li>Solicitud de Reestiba. &nbsp; [<a target='blank' href='http://www.cgsa.com.ec/files/guia_tramites/servicio_al_cliente/S3MReestiba.pdf'>PDF</a> ]</li>
			   <li>Solicitud de Repesaje.</li>
				<li>Solicitud de Verficación de Sellos.</li>
               </ul>
               
               </div>
             </div>



      </div>
        <div class="display">
      <div  class="right borde-all">
           <p id="xtoper">Inicio de sesión</p>
           <div id="passlogin" >&nbsp;&nbsp;
           Escriba su nombre de usuario y contraseña
                    <table cellspacing="2" cellpadding="1" class="tabla">
                    <tr><td class="centrar">
                        <asp:TextBox placeholder="nombre de usuario" ID="user"  autofocus="autofocus"  ClientIDMode="Static" runat="server" Width="80%"
                     onkeypress="return soloLetras(event,'1234567890abcdefghijklmnñopqrstuvwxyz-_',true)"
                    onblur="cadenareqerida(this,1,20,'valuse');" MaxLength="30"  
                    ></asp:TextBox> 
                   
                    </td></tr>
                    <tr><td class="centrar"> <span id='valuse' class="validacion"></span></td></tr>
                    <tr><td class="centrar"> 
                        <asp:TextBox  placeholder="clave" ID="pass" runat="server"   onpaste="return false;" 
                            onblur="cadenareqerida(this,1,20,'valpas');" ClientIDMode="Static" 
                            TextMode="Password" Width="80%" MaxLength="25"></asp:TextBox>
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
                       <tr>     <td colspan="2" class="centrar" >
                        Olvidó su contraseña, por favor de click <a href="../aisv/csl/recuperacioncliente"> <i class="element-menu"></i> aquí</a>
                       </td></tr>

                   </table>

                   <div class="demotext">
                             Si usted es usuario nuevo, 
                             favor envíe un correo a <a href="mailto:s3user@cgsa.com.ec">s3user@cgsa.com.ec</a> 
                             para que su solicitud sea atendida. 
                   </div>
                 <br />        
           </div>
         </div>
      <div class="right2 borde-all">
       <i class="secc"></i><span class="icon-text">Notas</span>
       <p>
       Sus solicitudes serán atendidas dentro del horario del departamento de Servicio al Cliente, de Lunes a Viernes de 08h30 – 19h00.
       </p>
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
    <script src="Scripts/pages.js" type="text/javascript"></script>
     <script src="../Scripts/pages.js" type="text/javascript"></script>
      <script type='text/javascript'>
         //<![CDATA[
          /*funcion para la barra de*/
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
              if (document.getElementById('user').value.trim().length <= 0 || document.getElementById('pass').value.trim().length <= 0) {
                  alert('Por favor complete la información requerida!');
                  return false;
              }
              document.getElementById('imagen').innerHTML = '<img alt="" src="shared/imgs/loader.gif"/>';
                    return true;
                }
             
</script>
<script type="text/javascript">
    $(document).ready(function () {
        
            $('#popup').fadeIn('slow');
            $('.popup-overlay').fadeIn('slow');
            $('.popup-overlay').height($(window).height());
           
        

        $('#close').click(function () {
            $('#popup').fadeOut('slow');
            $('.popup-overlay').fadeOut('slow');
            return false;
        });
    });
</script>

<div id="popup" style="display: none;">
    <div class="content-popup">
        <div class="close"><a href="#" id="close"><img src="images/close.png"/></a></div>
        <div>
           <h2>Contenido POPUP</h2>
           ...
        </div>
    </div>
</div>
</body>
</html>
