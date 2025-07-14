<%@ Page  Language="C#" AutoEventWireup="true" Title="Reporte de Estado de Orden de Retiro" CodeBehind="listado_orden_retiro_lookup.aspx.cs" Inherits="CSLSite.autorizaciones.listado_orden_retiro_lookup" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Reporte de Estado de Orden de Retiro</title>
    <link href="../shared/estilo/base-site.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/jquery.datetimepicker.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/jquery-1.7.1.min.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.datetimepicker.js" type="text/javascript"></script>
     <link href="../Inicio/css/font-awesome.min.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/aisv-general.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/work.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/tables.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/catalogoestadosolicitud.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/info.css" rel="stylesheet" type="text/css" />
    <link href="../Inicio/css/font-awesome.min.css" rel="stylesheet" type="text/css" />
    <link href="../Inicio/base-site.css" rel="stylesheet" type="text/css" />
    <link rel="../shortcut icon" type="image/x-icon" href="../favicon.ico" />
    <link href="../shared/estilo/aisv-general.css" rel="stylesheet" type="text/css" />


    <link href="lib/css/style.css" rel="stylesheet"/>
    <link href="lib/bootstrap/css/bootstrap.min.css" rel="stylesheet"/>

    <%-- <link href="lib/advanced-datatable/css/demo_page.css" rel="stylesheet" />
    <link href="lib/advanced-datatable/css/demo_table.css" rel="stylesheet" />
    <link rel="stylesheet" href="lib/advanced-datatable/css/DT_bootstrap.css" />
    <link href="lib/css/pagination.css" rel="stylesheet"/>--%>
      <%--<script src="../Scripts/pages.js" type="text/javascript"></script>--%>
 

    <style type="text/css">
     * input[type=text]
        {
            text-align:left!important;
        }
        .style1
        {
            border-bottom: 1px solid #CCC;
            width: 530px;
        }
    </style>


 

</head>
<body>
  <noscript><meta http-equiv="refresh" content="0; url=sinsoporte.htm" /> </noscript>
   <form id="wfconestsol" runat="server">
   <input id="zonaid" type="hidden" value="7" />
    <input id="IdEmpresa" type="hidden" value="" runat="server" clientidmode="Static" />
    <input id="RucEmpresa" type="hidden" value="" runat="server" clientidmode="Static" />
    <input id="NombreEmpresa" type="hidden" value="" runat="server" clientidmode="Static" />
    
      <input id="IdVehiculo" type="hidden" value="" runat="server" clientidmode="Static" />
      <input id="Placa" type="hidden" value="" runat="server" clientidmode="Static" />

      <input id="IdChofer" type="hidden" value="" runat="server" clientidmode="Static" />
      <input id="Licencia" type="hidden" value="" runat="server" clientidmode="Static" />
        <input id="NombreChofer" type="hidden" value="" runat="server" clientidmode="Static" />
    <asp:ToolkitScriptManager ID="tkscata" runat="server"></asp:ToolkitScriptManager>
   <%-- <div class="catabody" style=" height:100%">--%>
 
 <div class="catawrap" >
 <div>
 <table>
 <tr>
 <td>   

 </td>
 <td>
  <%--<i class="ico-titulo-1"></i><h2> </h2><h2>&nbsp;</h2><br />--%>
   <i class="ico-titulo-2"></i><h1>Reporte de Estado de Orden de Retiro</h1><br />
 </td>
 </tr>
 </table>
 </div>
 <div class="seccion">
     

       <div class="accion">
       <table class="xcontroles" cellspacing="0" cellpadding="1">
         <!--DWLayoutTable-->
    <%--  <tr><th height="27" colspan="5" >Línea Naviera: &nbsp;
          <asp:Label ID="LblLineaNaviera" runat="server" Text=""></asp:Label>
          </th></tr>--%>

       </table>
    
       
       </div>
       
 </div>


<div class="cataresult" >
 <asp:UpdatePanel ID="upresult" runat="server" ChildrenAsTriggers="true">
<ContentTemplate>

        <input id="idlin" type="hidden" runat="server" clientidmode="Static" />
        <input id="diponible" type="hidden" runat="server" clientidmode="Static" />
        <div class="msg-alerta" id="alerta" runat="server" style=" display:none" ></div>
       
          <div class="seccion">                
           <div class="accion">
             <div class="cataresult" >
              <asp:UpdatePanel ID="UPVEHICULOS" runat="server" ChildrenAsTriggers="true">
                         <ContentTemplate>
                          <%--  <script type="text/javascript">
                                Sys.Application.add_load(BindFunctions); 
                          </script>--%>
                 <div id="xfinder1" runat="server" visible="true" >

                <div class="findresult" >
                  <div class="booking" >
                      <table width="100%" border="0" cellpadding="0" cellspacing="0">
                      <!--DWLayoutTable-->
                      
  
                      <tr>
                        <td height="25" colspan="5" valign="middle" bgcolor="#EBEBEB">
                             <div id="sinresultado" runat="server" class="msg-info"></div>
                        </td>
                      </tr>
                      
                      <tr>
                        <td height="40" colspan="5" valign="top">
                           
                                        <div class="bokindetalle">
                                           <div class="content-panel" runat="server" id="vehiculo" visible="true">
                                                        <rsweb:reportviewer ID="ReportViewer1" runat="server" Font-Names="Verdana" 
                                                            Font-Size="8pt" InteractiveDeviceInfos="(Colección)" 
                                                            WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt" 
                                                          Visible="true"   >
                                                            <LocalReport ReportPath="autorizaciones\Rpt_Orden_Retiro.rdlc">
                                                                <DataSources>
                                                                    <rsweb:ReportDataSource DataSourceId="SqlDataSource_Detalle" 
                                                                        Name="RVA_REPORTE_ORDEN_SERVICIO" />
                                                                </DataSources>
                                                            </LocalReport>
                                                        </rsweb:reportviewer>
                                                              <asp:SqlDataSource ID="SqlDataSource_Detalle" runat="server" ConnectionString="<%$ ConnectionStrings:midle %>" SelectCommand="RVA_REPORTE_ORDEN_SERVICIO" SelectCommandType="StoredProcedure">        
                                                                 <SelectParameters>  
                                                                  <asp:SessionParameter  Type="String" Name="XmlContenedor"  SessionField="XmlContenedor" />
                                                                  <asp:SessionParameter  Name="ID" SessionField="ID" DbType="Int64"  />      
                                                              
                                                            </SelectParameters>
                                                              </asp:SqlDataSource>          

                                           </div><!--content-panel-->
                                           
                                        </div>

                        </td>
                      </tr>
  
                      <tr>
                        <td height="1"></td>
                        <td><img src="lib/advanced-datatable/images/spacer.gif" alt="" width="386" height="1" /></td>
                        <td><img src="lib/advanced-datatable/images/spacer.gif" alt="" width="131" height="1" /></td>
                        <td><img src="lib/advanced-datatable/images/spacer.gif" alt="" width="141" height="1" /></td>
                        <td><img src="lib/advanced-datatable/images/spacer.gif" alt="" width="563" height="1" /></td>
                      </tr>
                    </table>
                  
                  </div>
                 </div>
                 </div> 
                  </ContentTemplate>
                       
                     </asp:UpdatePanel>
              </div>
            </div>       
           </div>

        </ContentTemplate>
                   
        </asp:UpdatePanel>
</div>

</div>
 
  </form>
    <script type="text/javascript" src="lib/jquery/jquery.min.js"></script>
   <script type="text/javascript" src="lib/bootstrap/js/bootstrap.min.js"></script>
<%--  <script type="text/javascript" src='lib/autocompletar/bootstrap3-typeahead.min.js'></script>--%>
 
   
 

<script type="text/javascript">

    function mostrarloader() {

        try {
           
                document.getElementById("ImgCarga").className = 'ver';
                
            } catch (e) {
                alert('Lamentamos comunicarle que ha ocurrido el siguiente problema:\n' + e.Message);
            }
        }

    function ocultarloader() {
        try {

           
                document.getElementById("ImgCarga").className = 'nover';
           
             } catch (e) {
                alert('Lamentamos comunicarle que ha ocurrido el siguiente problema:\n' + e.Message);
            }
        } 
   
   
   
</script>



    <script type="text/javascript">
        

         
        function getGif() {document.getElementById('imagen').innerHTML = '<img alt="" src="../shared/imgs/loader.gif"/>';
            return true;
        }
        function getGifOculta() {
            document.getElementById('imagen').innerHTML = '<img alt="" src=""/>';
            return true;
        }
    </script>
    </body>
</html>

