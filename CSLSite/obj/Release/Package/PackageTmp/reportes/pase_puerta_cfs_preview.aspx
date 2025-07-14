<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="pase_puerta_cfs_preview.aspx.cs" Inherits="CSLSite.pase_puerta_cfs_preview" %>
<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxtoolkit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>Imprimir Pases de Puerta de Carga Suelta (CFS)</title>
     
     <link href="../img/favicon2.png" rel="icon"/>
  <link href="../img/icono.png" rel="apple-touch-icon"/>
 <!-- Bootstrap core CSS -->
  <link href="../css/bootstrap.min.css" rel="stylesheet"/>
  <link href="../css/dashboard.css" rel="stylesheet"/>
  <link href="../css/icons.css" rel="stylesheet"/>
  <link href="../css/style.css" rel="stylesheet"/>

 
   
    <style type="text/css">
        .auto-style1 {
            font-size: 11px;
        }
    </style>
</head>
<body>
     

  <form id="FrmPase" runat="server" method="post" accept-charset="UTF-8" autocomplete="off"  >
   <ajaxtoolkit:ToolkitScriptManager ID="tkajax" runat="server" EnableScriptGlobalization="true" CombineScripts="false" AsyncPostBackTimeout="500000" ScriptMode="Release">
    </ajaxtoolkit:ToolkitScriptManager>

   
     <div id="div_BrowserWindowName" style="visibility:hidden;">
        <asp:HiddenField ID="hf_BrowserWindowName" runat="server" />
  </div>

  <div class="dashboard-container p-4" id="cuerpo" runat="server">
       <div class="row">
          <div class="col-md-12 d-flex justify-content-center">
              <asp:UpdatePanel ID="UPMENSAJE" runat="server"  UpdateMode="Conditional" >
                         <ContentTemplate>
                             <div class="alert alert-warning" id="banmsg" runat="server" clientidmode="Static"><b>Error!</b> ......</div>
                              </ContentTemplate>
                        </asp:UpdatePanel>   
          </div>
     </div>
     <div class="row">
         <div class="col-md-12 d-flex justify-content-center"> 
             <asp:UpdatePanel ID="UPPASEPUERTA" runat="server" UpdateMode="Conditional" >  
                            <ContentTemplate>
                            <div id="imagen" style="align-content:center;" runat="server" clientidmode="Static">
                                 <p align="center">
                                <img src="../img/print_pase.png" width="350" height="450" alt ="" /></p>
                           </div>
                          <rsweb:ReportViewer ID="rwReporte" runat="server" Width="100%" Height="880px"
                            DocumentMapCollapsed="True" ShowDocumentMapButton="False" 
                        ShowParameterPrompts="False" ShowPromptAreaButton="False">
                        <LocalReport EnableExternalImages="True" ReportPath="rptpasepuertacfs.rdlc" >
                        </LocalReport>
                        </rsweb:ReportViewer>

                        </ContentTemplate>
                        </asp:UpdatePanel>   
         </div>
     </div>
    
</div>
    </form>

   

</body>
</html>
