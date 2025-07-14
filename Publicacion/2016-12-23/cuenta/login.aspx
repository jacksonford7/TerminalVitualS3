<%@ Page Title="Iniciar sesion" Language="C#" AutoEventWireup="true" CodeBehind="login.aspx.cs" Inherits="CSLSite.cuenta.login" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<title></title>
    <link href="../shared/estilo/login.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/pages.js" type="text/javascript"></script>
</head>
<body>
    <div id="loginform">
    <div id="bantop">Sistema de Solicitud de Servicios Contecon Guayaquil</div>
            <div id="facebook"><div id="connect">&nbsp;</div></div>
            <div id="mainlogin">
            <h1>Escriba su nombre de usuario y contraseña</h1>
            <form action="#" id="xformlogin" runat="server">
            <asp:TextBox ID="user" runat="server"></asp:TextBox>
            <asp:TextBox ID="pass" TextMode="Password" runat="server"></asp:TextBox>
            <div id="deboton">
             <asp:Button ID="btstart" 
             runat="server" Text="Iniciar sesión" 
             OnClientClick ="return checkdata()" 
             onclick="btstart_Click" />
            </div>
            </form>
            </div>
            <br  />
      <div id="banmsg" class="banmsg" runat="server">panel de mensaje</div>
    </div>
    <script type="text/javascript">
        function checkdata() {
           var xuser = document.getElementById('<%=user.ClientID %>').value;
           var xpass = document.getElementById('<%=pass.ClientID %>').value;
           if (xuser == undefined || xuser == null || xuser.trim().length <= 0) {
               document.getElementById('banmsg').textContent = 'Por favor escriba su nombre de usuario.';
               return false;
           }
           if (xpass == undefined || xpass == null || xpass.trim().length <= 0) {
               document.getElementById('banmsg').textContent = 'Por favor escriba su contraseña.';
               return false;
           }
            document.getElementById('banmsg').textContent='';
            return true;
        }
    </script>
</body>
</html>
