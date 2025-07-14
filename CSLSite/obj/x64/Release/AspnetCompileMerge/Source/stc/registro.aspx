<%@ Page Title="Registro de Empresa" Language="C#" AutoEventWireup="true" CodeBehind="registro.aspx.cs" Inherits="CSLSite.registro" %>

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
                             
                         <h1 class="h3 mb-3 font-weight-normal">Registro de Empresa</h1>
                            <div class="text-left"> 
                                           <label for="inputAddress">1. Ruc:<span style="color: #FF0000; font-weight: bold;"> *</span></label>
                                            <div class="input-group mb-3">
                                                  <asp:TextBox ID="txtruccipas" runat="server" MaxLength="15" onpaste="return false" class="form-control" style="text-align: center;text-transform:uppercase" 
                                                onkeypress="return soloLetras(event,'01234567890',true)"></asp:TextBox> 
                                             <span class="validacion" id="valruccipas" ></span>
                                            </div> 
                           </div>
                            <div class="text-left"> 
                                            <label for="inputAddress">2. Nombre/Razón Social:<span style="color: #FF0000; font-weight: bold;">*</span></label>
                                             <div class="input-group mb-3">
                                                <asp:TextBox ID="txtrazonsocial" runat="server" MaxLength="500" onblur="checkcajalarge(this,'valrazsocial',true);"  class="form-control"
                                                 onkeypress="return soloLetras(event,'abcdefghijklnmñopqrstuvwxyz01234567890.-_/& ',true)" Style="text-transform:uppercase"></asp:TextBox>
                                                <span id="valrazsocial" class="validacion"></span>
                                            </div>
                                           
                           </div>           
                            <div class="text-left"> 
                                           <label for="inputAddress">3. Email:<span style="color: #FF0000; font-weight: bold;">*</span></label>
                                             <div class="input-group mb-3">
                                                <asp:TextBox id='txtmail' runat="server"  class="form-control" placeholder="mail@mail.com" 
                                                onkeypress="return soloLetras(event,'abcdefghijklmnñopqrstuvwxyz1234567890_$@.;',true)"  MaxLength="200"/>
                                                <span id="valmaileb" class="validacion"></span>
                                            </div>
                                           
                             </div>
                             <div class="text-left"> 
                                  <div class="form-group col-md-12">
                                     Estimado Cliente, suscríbase a STC y reciba notificaciones en tiempo real de la trazabilidad de sus contenedores de importación.
                                </div>
                                <%-- <div class="form-group col-md-12">
                                      Toda la información de su carga, al alcance de su mano a través de mail, app o nuestro portal de clientes.
                                </div>--%>
                                 <div class="form-group col-md-12"> 
         
                                      <div class="d-flex"> 
                                            <label class="radiobutton-container" >
                                               Acepto  Servicio de trazabilidad de carga de importación <input  id="rbpSiAcepto" runat="server"  type="checkbox"  checked="true"  clientidmode="Static" />
                                                <span class="checkmark"></span>
                                            </label>
               
                                      </div> 
                                </div>



                             </div> 

                             <div class="text-left">  
                                     <div data-theme="light" class="g-recaptcha" data-sitekey="6LfibkEUAAAAAIK-pu90AlJAbjvMSoKVIGkrov__" data-callback="recaptchaCallback"></div>
                                     <input type="hidden" class="hiddenRecaptcha required" name="hiddenRecaptcha" id="hiddenRecaptcha">
                                     <span id="msgCaptcha" style="color:red; font-size:small;"></span>
                                     <asp:HiddenField runat="server" ID="imagencaptcha" />
                             </div>
                            <div class="text-left"> 
                                <div class="botonera">
                                     <asp:Button ID="btngrabar" Text="Registrar Empresa" runat="server" 
                                          OnClientClick="return prepareObject('¿Esta seguro de registrar la empresa?')"
                                         OnClick="btngrabar_Click" class="btn btn-lg btn-primary btn-block"/>
                                     <img alt="loading.." src="../shared/imgs/loader.gif" id="loader" style=" display:none" />
                                </div>
                            </div>
                            <div class="text-left"> 
                                <div class="alert alert-warning" id="alerta" runat="server"></div>
                            </div>
                         
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <script src="../Scripts/pages.js" type="text/javascript"></script>
                    <script src="../Scripts/pages_mod.js" type="text/javascript"></script>
                 
             
                </div>
                 <div class="row">
                  <span class="text-muted col-md-12">Cualquier inquietud puede ser remitida a 
                </div>
            </div>
        </div>
        </form>
    </div>
  </div>
     
   
     <script src='https://www.google.com/recaptcha/api.js'></script>
     <script src="../Scripts/credenciales.js" type="text/javascript"></script>

       <script type="text/javascript">


           function recaptchaCallback()
           {
                   document.getElementById('hiddenRecaptcha').value= grecaptcha.getResponse();
                   document.getElementById('msgCaptcha').innerHTML ='';
          }

       
      
     
        //esta futura funcion va a preparar el objeto a transportar.
        var registroempresa = {};
        var lista = [];
        var cblarray = [];
        var carray = [];
        function prepareObject(valor) {
            try {

                if (confirm(valor) == false) {
                    return false;
                }

                mostrarloader();
                
                var vals = document.getElementById('<%=txtrazonsocial.ClientID %>').value;
                if (vals == '' || vals == null || vals == undefined) {
                    alertify.alert('* Datos de Registro de Empresa: *\n * Escriba la Razon Social *').set('label', 'Aceptar');
                    document.getElementById('<%=txtrazonsocial.ClientID %>').focus();
                    document.getElementById("loader").className = 'nover';
                    return false;
                }
                if (!valruccipasservidor()) {
                    return false;
                };
               
                var mail1 = document.getElementById('<%=txtmail.ClientID %>').value;
                if (mail1 == null || mail1 == undefined || mail1 == '') {
                    alertify.alert('* Datos de Registro de Empresa: *\n * Escriba al menos un mail *').set('label', 'Aceptar');
                    document.getElementById('<%=txtmail.ClientID %>').focus();
                    document.getElementById("loader").className = 'nover';
                    return false;
                }
                if (mail1.trim().length > 0) {
                    if (!validarEmail(mail1)) {
                        alertify.alert('* Datos de Registro de Empresa: *\n * Mail no parece correcto *').set('label', 'Aceptar');
                        document.getElementById('<%=txtmail.ClientID %>').focus();
                        document.getElementById("loader").className = 'nover';
                        return false;
                    }
                }


                var captura = document.getElementById("hiddenRecaptcha").value;

              
                if (captura == '')
                {
                    document.getElementById('msgCaptcha').innerHTML = "<span>Por favor confirme que usted no es un robot</span>";
                    document.getElementById("loader").className = 'nover';
                    return false;
                }
                $.getScript("https://www.google.com/recaptcha/api.js");
                grecaptcha.reset(); 

                document.getElementById("loader").className = '';
                return true;


            } catch (e) {
                alertify.alert('Lamentamos comunicarle que ha ocurrido el siguiente problema:\n' + e.Message).set('label', 'Aceptar');
            }
           }


 

        function valruccipasservidor() {
           
            var valruccipas = document.getElementById('<%=txtruccipas.ClientID %>').value;
          
                if (!/^([0-9])*$/.test(valruccipas)) {
                    alertify.alert('* Datos de Registro de Empresa: *\n * No. RUC No es un Numero. *').set('label', 'Aceptar');
                    document.getElementById('<%=txtruccipas.ClientID %>').focus();
                    document.getElementById("loader").className = 'nover';
                    return false;
                }
                else if (valruccipas == '' || valruccipas == null || valruccipas == undefined) {
                    alertify.alert('* Datos de Registro de Empresa: *\n * Escriba el No. de RUC. *').set('label', 'Aceptar');
                    document.getElementById('<%=txtruccipas.ClientID %>').focus();
                    document.getElementById("loader").className = 'nover';
                    return false;
                }
                var numeroProvincias = 24;
                var numprov = valruccipas.substr(0, 2);
                if (numprov > numeroProvincias) {
                    alertify.alert('* Datos de Registro de Empresa: *\n * El código de la provincia (dos primeros dígitos) es inválido! *').set('label', 'Aceptar');
                    document.getElementById('<%=txtruccipas.ClientID %>').focus();
                    document.getElementById("loader").className = 'nover';
                    return false;
                }
                if (!validadocrucservidor(valruccipas)) {
                    return false;
                };
            

            
            return true;
        }
        function validadocrucservidor(campo) {

            var numero = campo;
            var suma = 0;
            var residuo = 0;
            var pri = false;
            var pub = false;
            var nat = false;
            var numeroProvincias = 24;
            var modulo = 11;

            if (campo.length < 13) {
                alertify.alert('* Datos de Registro de Empresa: *\n * No. RUC. INCOMPLETO. *').set('label', 'Aceptar');
                document.getElementById('<%=txtruccipas.ClientID %>').focus();
                document.getElementById("loader").className = 'nover';
                return false;
            }
            if (campo.length > 13) {
                alertify.alert('* Datos de Registro de Empresa: *\n * El valor no corresponde a un No. de RUC. *').set('label', 'Aceptar');
                document.getElementById('<%=txtruccipas.ClientID %>').focus();
                document.getElementById("loader").className = 'nover';
                return false;
            }
            /* Verifico que el campo no contenga letras */

            /* Aqui almacenamos los digitos de la cedula en variables. */
            d1 = numero.substr(0, 1);
            d2 = numero.substr(1, 1);
            d3 = numero.substr(2, 1);
            d4 = numero.substr(3, 1);
            d5 = numero.substr(4, 1);
            d6 = numero.substr(5, 1);
            d7 = numero.substr(6, 1);
            d8 = numero.substr(7, 1);
            d9 = numero.substr(8, 1);
            d10 = numero.substr(9, 1);

            /* El tercer digito es: */
            /* 9 para sociedades privadas y extranjeros */
            /* 6 para sociedades publicas */
            /* menor que 6 (0,1,2,3,4,5) para personas naturales */

            if (d3 == 7 || d3 == 8) {
                //alertify.alert('El tercer dígito ingresado es inválido');
                alertify.alert('* Datos de Registro de Empresa: *\n * El tercer dígito ingresado es inválido. *').set('label', 'Aceptar');
                document.getElementById('<%=txtruccipas.ClientID %>').focus();
                <%--document.getElementById('<%=txtruccipas.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:200;";--%>
                document.getElementById("loader").className = 'nover';
                return false;
            }

            /* Solo para personas naturales (modulo 10) */
            if (d3 < 6) {
                nat = true;
                p1 = d1 * 2; if (p1 >= 10) p1 -= 9;
                p2 = d2 * 1; if (p2 >= 10) p2 -= 9;
                p3 = d3 * 2; if (p3 >= 10) p3 -= 9;
                p4 = d4 * 1; if (p4 >= 10) p4 -= 9;
                p5 = d5 * 2; if (p5 >= 10) p5 -= 9;
                p6 = d6 * 1; if (p6 >= 10) p6 -= 9;
                p7 = d7 * 2; if (p7 >= 10) p7 -= 9;
                p8 = d8 * 1; if (p8 >= 10) p8 -= 9;
                p9 = d9 * 2; if (p9 >= 10) p9 -= 9;
                modulo = 10;
            }

            /* Solo para sociedades publicas (modulo 11) */
            /* Aqui el digito verficador esta en la posicion 9, en las otras 2 en la pos. 10 */
            else if (d3 == 6) {
                pub = true;
                p1 = d1 * 3;
                p2 = d2 * 2;
                p3 = d3 * 7;
                p4 = d4 * 6;
                p5 = d5 * 5;
                p6 = d6 * 4;
                p7 = d7 * 3;
                p8 = d8 * 2;
                p9 = 0;
            }

            /* Solo para entidades privadas (modulo 11) */
            else if (d3 == 9) {
                pri = true;
                p1 = d1 * 4;
                p2 = d2 * 3;
                p3 = d3 * 2;
                p4 = d4 * 7;
                p5 = d5 * 6;
                p6 = d6 * 5;
                p7 = d7 * 4;
                p8 = d8 * 3;
                p9 = d9 * 2;
            }

            suma = p1 + p2 + p3 + p4 + p5 + p6 + p7 + p8 + p9;
            residuo = suma % modulo;

            /* Si residuo=0, dig.ver.=0, caso contrario 10 - residuo*/
            digitoVerificador = residuo == 0 ? 0 : modulo - residuo;

            /* ahora comparamos el elemento de la posicion 10 con el dig. ver.*/
            if (pub == true) {
                if (digitoVerificador != d9) {
                    alertify.alert('* Datos de Registro de Empresa: *\n * El RUC de la empresa del sector público es incorrecto. *').set('label', 'Aceptar');
                    document.getElementById('<%=txtruccipas.ClientID %>').focus();
                    document.getElementById("loader").className = 'nover';
                    return false;
                }
                /* El ruc de las empresas del sector publico terminan con 0001*/
                if (numero.substr(9, 4) != '0001') {
                    alertify.alert('* Datos de Registro de Empresa: *\n * El RUC de la empresa del sector público debe terminar con 0001. *').set('label', 'Aceptar');
                    document.getElementById('<%=txtruccipas.ClientID %>').focus();
                    document.getElementById("loader").className = 'nover';
                    return false;
                }
            }
            else if (pri == true) {
                if (digitoVerificador != d10) {
                    alertify.alert('* Datos de Registro de Empresa: *\n * El RUC de la empresa del sector privado es incorrecto. *').set('label', 'Aceptar');
                    document.getElementById('<%=txtruccipas.ClientID %>').focus();
                    document.getElementById("loader").className = 'nover';
                    return false;
                }
                if (numero.substr(10, 3) != '001') {
                    alertify.alert('* Datos de Registro de Empresa: *\n * El RUC de la empresa del sector privado debe terminar con 001. *').set('label', 'Aceptar');
                    document.getElementById('<%=txtruccipas.ClientID %>').focus();
                    document.getElementById("loader").className = 'nover';
                    return false;
                }
            }
            else if (nat == true) {
                if (digitoVerificador != d10) {
                    alertify.alert('* Datos de Registro de Empresa: *\n * El número de cédula de la persona natural es incorrecto. *').set('label', 'Aceptar');
                    document.getElementById('<%=txtruccipas.ClientID %>').focus();
                    document.getElementById("loader").className = 'nover';
                    return false;
                }
                if (numero.length > 10 && numero.substr(10, 3) != '001') {
                    alertify.alert('* Datos de Registro de Empresa: *\n * El RUC de la persona natural debe terminar con 001. *').set('label', 'Aceptar');
                    document.getElementById('<%=txtruccipas.ClientID %>').focus();
                    document.getElementById("loader").className = 'nover';
                    return false;
                }
            }
            return true;
        }
        function validarEmail(email) {
            expr = /^([a-zA-Z0-9@;_\.\-])+\@(([a-zA-Z0-9@;\-])+\.)+([a-zA-Z0-9@;]{2,4})+$/; ;
            if (!expr.test(email)) {
                return false;
            }
            return true;
           }

        function get_action(form) {
            var v = grecaptcha.getResponse();
            if (v.length == 0) {
                alertify.alert("No se puede dejar vacío Código Captcha.").set('label', 'Aceptar');
                return false;
            }
            if (v.length != 0) {

                return true;
            }
        }
        function getGif() {
            //document.getElementById('imagen').innerHTML = '<img alt="" src="../shared/imgs/loader.gif"/>';
            return true;
        }
        function getGifOculta() {
            //document.getElementById('imagen').innerHTML = '<img alt="" src=""/>';
            return true;
           }


      function mostrarloader()
        {

        try {
            
                document.getElementById("loader").className = 'ver';
            
                
        }
        catch (e) {
                alert('Lamentamos comunicarle que ha ocurrido el siguiente problema:\n' + e.Message);
            }
      }

        function ocultarloader() {
        try {

                document.getElementById("loader").className = 'nover';
           
             } catch (e) {
                alert('Lamentamos comunicarle que ha ocurrido el siguiente problema:\n' + e.Message);
            }
        }

      
    </script>

</body>
</html>
