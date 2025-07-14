<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="menu_cotizaciones.aspx.cs" Inherits="CSLSite.menu_cotizaciones" Title="Menú Cotizaciones" %>


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
        <form id="fmrinset" runat="server" method="post" accept-charset="UTF-8">
     
        <div id="wall-panel">
            
            <div id="wraper-panel">
                <div id="logoCGSA">
                </div>
                <br />
                <div class="left">
                     <img class="mb-4" src="../img/logo_02.jpg" alt="" />
                </div>
                <div class="right">
                    <asp:ToolkitScriptManager ID="tkcal" EnableScriptGlobalization="true" runat="server">
                    </asp:ToolkitScriptManager>
                   
                  
                    <asp:UpdatePanel ID="updConsultaUsuarios" runat="server">
                        <ContentTemplate>
                            <div>
                                <div class="h3 mb-3">         
                                <nav class="h3 mb-3" aria-label="breadcrumb">
                                  <ol class="breadcrumb">
                                    <li class="breadcrumb-item active" aria-current="page"  runat="server" style="align-content:center">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Menú Cotizaciones</li>
                                  </ol>
                                </nav>
                              </div>
                            </div>
                             <h1 class="h3 mb-3 font-weight-normal">Click en las opciones para realizar una cotización.</h1>
                             <div class="seccion" id="PERSONAL">
                                <div class="text-center"> 
                                     <label for="">&nbsp;</label>
                                    <div class="h3 mb-3">
                                        <asp:Button ID="BtnImpoContenedor" runat="server" class="btn btn-outline-primary mr-4"   Text="Cotización Contenedor Importación"  OnClick="BtnImpoContenedor_Click"   />
                                    </div>
                               </div>
                                <div class="text-center"> 
                                    <%-- <label for="">&nbsp;</label>--%>
                                    <div class="h3 mb-3">
                                        <asp:Button ID="BtnImpoCargaSuelta" runat="server" class="btn btn-outline-primary mr-4"   Text="Cotización Carga Suelta Importación" OnClick="BtnImpoCargaSuelta_Click"    />
                                    </div>
                                    <br/>
                                </div>
                              </div>  
                               

                        </ContentTemplate>
                    </asp:UpdatePanel>
                  
                    <script src="../Scripts/pages.js" type="text/javascript"></script>
                    
                   
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
