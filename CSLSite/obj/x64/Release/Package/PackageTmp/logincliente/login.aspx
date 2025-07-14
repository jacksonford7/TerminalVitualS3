<%@ Page Language="C#" AutoEventWireup="true" Title="Login"
CodeBehind="login.aspx.cs" Inherits="CSLSite.logincliente.login" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    
  <meta http-equiv="X-UA-Compatible" content="IE=Edge" />
  <meta name="viewport" content="width=device-width, initial-scale=1.0"/>
  <meta name="description" content=""/>
  <meta name="author" content="Contecon S.A."/>
  <meta name="keyword" content="Contecon, Terminal Virtual, cgsa, contecon, guayaquil"/>
  <title>Terminal Virtual - CONTECON S.A </title>

  <link href="../img/favicon2.png" rel="icon"/>
  <link href="../img/icono.png" rel="apple-touch-icon"/>
 <!-- Bootstrap core CSS -->
    <link href="../css/bootstrap.min.css"  rel="stylesheet"/>
    <link href="../css/signin.css" rel="stylesheet"/>
    <link href="../css/icons.css" rel="stylesheet"/>
    <link href="../css/style.css" rel="stylesheet"/>
    <link href="https://fonts.googleapis.com/css2?family=Roboto:wght@300;400;500;700&display=swap" rel="stylesheet"/>
    
  <link href="../lib/font-awesome/css/font-awesome.css" rel="stylesheet" />

       <!--mensajes-->
        <link href="https://cdn.jsdelivr.net/npm/alertifyjs@1.11.0/build/css/alertify.min.css" rel="stylesheet"/>
        <script type="text/javascript" src="https://cdn.jsdelivr.net/npm/alertifyjs@1.11.0/build/alertify.min.js"></script>
        <script src="../lib/jquery/jquery.min.js" type="text/javascript"></script>

  
</head>
 <body class="flex-column sign-in">
     <img class="mb-4" src="../img/logo-contecon-white.png" alt="" width="194" height="52"/>
    <h1 class="text-white my-4">Terminal Virtual</h1>
  <div class="container-login">

    <form id="bookingfrm" class="form-signin" runat="server" autocomplete="off" accept-charset="UTF-8">
         <h1 class="h3 mb-3 font-weight-normal">Iniciar Sesión</h1>
         <p> Ingrese el usuario y clave temporal enviado en el mail, 
              para poder validar los 
              datos y los motivos del rechazo de la solicitud.</p>

        <div class="text-left">
                <label for="">Usuario</label>
                <div class="input-group mb-3">
                  <div class="input-group-prepend">
                    <span class="input-group-text"><i class="icon user"></i></span>
                       
                  </div>
                    <asp:TextBox class="form-control" placeholder="nombre de usuario" ID="user"  autofocus="autofocus"  ClientIDMode="Static" runat="server"
                    onkeypress="return soloLetras(event,'1234567890abcdefghijklmnñopqrstuvwxyz-_',true)"
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
         
                         <asp:TextBox  class="form-control pwd" placeholder="clave" ID="pass" runat="server"   onpaste="return false;" MaxLength="500"
                            ClientIDMode="Static" 
                             onkeypress="return soloLetras(event,'1234567890abcdefghijklmnñopqrstuvwxyz-_*+!?¡¿!#$%&@',true)"
                            TextMode="password"  type="text" required ></asp:TextBox>
                           
                      

                      <div class="input-group-prepend" id="button-addon3">
                        <span class="input-group-text eye-btn"><i class="icon eye"></i></span>
                      </div>
                    </div>
        </div> 

         <asp:button class="btn btn-lg btn-primary btn-block"  ID="btseguir" runat="server" Text="Continuar" onclick="btstart_Click" OnClientClick="return validar();"/>
        <span id="imagen"></span><br/>
         <div >
                             Si usted no recuerda su usuario/contraseña, 
                             favor revise el correo enviado por el Departamento de Credenciales.
          </div>
          <div class="alert alert-warning" id="banmsg" runat="server" clientidmode="Static">Falló el nombre de usuario y contraseña</div>
   
  
       
           <div id="passlogin" >

           </div>
          

    
    </form>
 
  </div>
       <div class="footer-login">
        <div class="container">
          <div class="row">
            <span class="text-muted col-md-8">Copyright 2020 © CONTECON an ICTSI Group Company. Todos los derechos reservados.</span>
            <div class="col-md-4 d-flex"><a href="#">Atención al Cliente &nbsp;</a> &nbsp;| &nbsp;<a href="mailto:ec.sac@contecon.com.ec"> &nbsp;Soporte Técnico</a></div>
          </div>
        </div>
      </div>

     <script src="../lib/jquery/jquery.min.js" type="text/javascript"></script>
     <script src="../../Scripts/pages.js" type="text/javascript"></script>
 <%--   <script src="../../Scripts/credenciales.js" type="text/javascript"></script>--%>
   <%-- <script src="../../Scripts/jquery-1.7.1.min.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery.tablesorter.min.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery.tablesorter.pager.js" type="text/javascript"></script>--%>
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
                alertify.alert('Por favor complete la información requerida, digite el usuario.!').set('label', 'Aceptar');
                document.getElementById('user').focus();
                return false;
            }
            if (document.getElementById('pass').value.trim().length <= 0) {
                //document.getElementById('banmsg').innerHTML = "Escriba el nombre de usuario y contraseña.";
                alertify.alert('Por favor complete la información requerida, digite la clave.!').set('label', 'Aceptar');
                document.getElementById('pass').focus();
                return false;
            }
            document.getElementById('imagen').innerHTML = '<img alt="loading.."" src="../../shared/imgs/loader.gif"/>';
            return true;
        }
   </script>
      
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


</body>
</html>
