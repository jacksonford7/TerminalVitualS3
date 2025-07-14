<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="login.aspx.cs" Inherits="CSLSite.login" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
  <meta http-equiv="X-UA-Compatible" content="IE=Edge" />
  <meta name="viewport" content="width=device-width, initial-scale=1.0"/>
  <meta name="description" content=""/>
  <meta name="author" content="Contecon S.A."/>
  <meta name="keyword" content="Contecon, Terminal Virtual, cgsa, contecon, guayaquil"/>
  <title>Terminal Virtual - CONTECON S.A </title>

  <link href="img/favicon2.png" rel="icon"/>
  <link href="img/icono.png" rel="apple-touch-icon"/>
 <!-- Bootstrap core CSS -->
    <link href="css/bootstrap.min.css"  rel="stylesheet"/>
    <link href="css/signin.css" rel="stylesheet"/>
    <link href="css/icons.css" rel="stylesheet"/>
    <link href="css/style.css" rel="stylesheet"/>
    <link href="https://fonts.googleapis.com/css2?family=Roboto:wght@300;400;500;700&display=swap" rel="stylesheet"/>
    
  <link href="lib/font-awesome/css/font-awesome.css" rel="stylesheet" />


</head>
 <body class="flex-column sign-in">
 <img class="mb-3" src="img/logo-contecon-white.png" alt="" width="194" height="52"/>
 <h1 class="text-white my-2">Terminal Virtual</h1>
   <div class="container-login">
 
    <form id="FrmLogin" class="form-signin" runat="server" autocomplete="off" accept-charset="UTF-8">

          <input id="puser" type="hidden" value="DESP01" runat="server" clientidmode="Static" />
       
         <h1 class="h3 mb-2 font-weight-normal">Iniciar Sesión</h1>
         <p>Ingrese su nombre de usuario y contraseña</p>
              <div class="text-left">
                <label for="">Usuario</label>
                <div class="input-group mb-3">
                  <div class="input-group-prepend">
                    <span class="input-group-text"><i class="icon user"></i></span>
                       
                  </div>
                    <asp:TextBox class="form-control" placeholder="nombre de usuario" ID="user"  autofocus="autofocus"  ClientIDMode="Static" runat="server"
                    onkeypress="return soloLetras(event,'1234567890abcdefghijklmnñopqrstuvwxyz-_.',true)"
                      MaxLength="30"  minlength="2"  type="text" required
                    ></asp:TextBox> 
                  
                </div>
              </div>
             <div class="text-left">
                    <label>Contraseña</label>
                    <div class="input-group mb-3" id="show_hide_password">
                      <div class="input-group-prepend">
                        <span class="input-group-text"><i class="icon lock"></i></span>
                      </div>
         
                         <asp:TextBox  class="form-control pwd" placeholder="clave" ID="pass" runat="server"   onpaste="return false;" 
                            ClientIDMode="Static" 
                             onkeypress="return soloLetras(event,'1234567890abcdefghijklmnñopqrstuvwxyz-_*+!?¡¿!#$%&@.',true)"
                            TextMode="password"  type="text" required ></asp:TextBox>
                      <div class="input-group-prepend" id="button-addon3">
                        <span class="input-group-text eye-btn"><i class="icon eye"></i></span>
                      </div>
                    </div>
                  </div> 
               <div class="d-flex justify-content-between">
                <div class="checkbox mb-3">
                   
                    <label class="checkbox-container"> &nbsp;
                      
                    </label>
                </div>
                <a href="recuperar/recuperacion.aspx">¿Olvidaste tu contraseña?</a>
              </div>
              
         <div class="form-row">  
                   <div class="col-md-12 d-flex justify-content-center">
                     
                    <div data-theme="light" class="g-recaptcha" data-sitekey="6LfibkEUAAAAAIK-pu90AlJAbjvMSoKVIGkrov__" data-callback="recaptchaCallback"></div>
                             <input type="hidden" class="hiddenRecaptcha required" name="hiddenRecaptcha" id="hiddenRecaptcha">
                             <span id="msgCaptcha" style="color:red; font-size:small;"></span>
                             <asp:HiddenField runat="server" ID="imagencaptcha" />
                 </div> 
            </div>
         <br/>
       
            <asp:button class="btn btn-lg btn-primary btn-block"  ID="btstart" runat="server" Text="Ingresar" onclick="btstart_Click"  OnClientClick="return prepareObject()"/>
         
            <br/>
          <div class="alert alert-warning" id="banmsg" runat="server" clientidmode="Static"><b>Warning!</b> Mensaje del Sistema......</div>
            <div >
                 <%-- <a  href="https://www.cgsa.com.ec:6443/AISV"><p class="fa fa-hand-o-right lead">&nbsp;&nbsp;Ir a versión clásica S3</p></a><br/>--%>
                   <a href="https://apps.cgsa.com.ec/Terminal/cliente/solicitudempresa.aspx" target="_blank"><p class="fa fa-hand-o-right lead">&nbsp;&nbsp;No tengo usuario</p></a>
              </div>

            

    </form>
   
  </div>
       <div class="footer-login">
        <div class="container">
          <div class="row">
            <span class="text-muted col-md-8">Copyright 2020 © CONTECON an ICTSI Group Company. Todos los derechos reservados.</span>
            <div class="col-md-4 d-flex"><a href="#">Atención al Cliente &nbsp;</a> &nbsp;| &nbsp;<a href="#"> &nbsp;Soporte Técnico</a></div>
          </div>
        </div>
      </div>

 <script src="lib/jquery/jquery.min.js" type="text/javascript"></script>




  <!--common script for all pages-->
<%--  <script src="lib/common-scripts.js" type="text/javascript"></script>--%>

  <!--BACKSTRETCH-->
  <!-- You can use an image of whatever size. This script will stretch to fit in any screen size.-->
 <%-- <script type="text/javascript" src="lib/jquery.backstretch.min.js"></script>--%>
  <script src="lib/pages.js" type="text/javascript"></script>

 <%-- <script type="text/javascript">
    $.backstretch("img/login-bg.jpg", {
      speed: 500
      });


  </script>--%>
      <script>
        $(".eye-btn").on('click',function() {
            var $pwd = $(".pwd");
            if ($pwd.attr('type') === 'password') {
                $pwd.attr('type', 'text');
            } else {
                $pwd.attr('type', 'password');
            }
        });
      </script>

      <div id="ventana_popup" style="display: none;">
    <div id="ventana_content-popup" style="height:450px" >
        <div>
		 <p id='manage_ventana_popup'> Alerta por cartera vencida!</p>
		    <div id='borde_ventana_popup' style=" font-size:1.4em!important; height:370px;" > 
			 Estimado Cliente:
             <br /> 
             Al momento usted presenta facturas vencidas.
             <table border="1" cellspacing="1" cellpadding="1" style=" width:90%;">
             
             <tr>
              <td>Facturas por Vencer</td>
              <td><span id="fac_pend" runat="server" clientidmode="Static">0</span></td>
              </tr>

              <tr>
              <td>Facturas Vencidas</td>
              <td><span id="fac_ven" runat="server" clientidmode="Static">0</span></td>
              </tr>

              <tr>
              <td>Monto Total</td>
              <td><span id="monto_fac" runat="server" clientidmode="Static">$0.00</span></td>
              </tr>

             </table>
               Favor proceder con el pago de las facturas detalladas y compensar las mismas. En caso de requerir revisión con el departamento Tesorería, contactar a <br /> (<a href="mailto:tesoreria@cgsa.com.ec?Subject=Aviso falta de pago" >tesoreria@cgsa.com.ec</a>)
			   y teléfono +593 4 6006300 - Opción 3 en horario lunes a viernes de 8am a 5:30pm.
               <br />
                Para mayor información visite la opción <a href="/AISV/PagoEnLinea/ConsultaSaldos">Consulta de Saldos Pendientes</a>
                <span id='cliente_ruc'></span>
			 <div  id="close" >CONTINUAR</div>
  			</div>
        </div>
    </div>
   </div>
   <div id="popup-overlay" style="display: none;"></div>
   <script src="../shared/avisos/popup_script_cta.js" type="text/javascript"></script>
     <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.6.2/jquery.min.js"></script>
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
     <script src="https://www.google.com/recaptcha/api.js" type="text/javascript"></script>

      <script type="text/javascript">

          function recaptchaCallback() {
                   document.getElementById('hiddenRecaptcha').value= grecaptcha.getResponse();
                   document.getElementById('msgCaptcha').innerHTML ='';
          };


          function prepareObject()
          {
                var user = document.getElementById("puser").value;
                var login = document.getElementById("user").value;

              //if (user != login)
              //{
              //  var captura = document.getElementById("hiddenRecaptcha").value;
              //  if(captura == '')
              //  {
              //      grecaptcha.reset();
              //      document.getElementById('msgCaptcha').innerHTML = "<span>Por favor confirme que usted no es un robot</span>";
              //      return false;

              //  }
              //  $.getScript("https://www.google.com/recaptcha/api.js");
              //  grecaptcha.reset(); 
              //}
               
          }
      </script>

</body>
</html>
