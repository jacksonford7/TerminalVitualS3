<%@ Page Title="Reporte de Niveles de Aprobación" Language="C#" MasterPageFile="~/site.Master" AutoEventWireup="true" 
    CodeBehind="frm_rpt_niveles_aprobacion.aspx.cs" Inherits="CSLSite.frm_rpt_niveles_aprobacion" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="placehead" runat="server">
    <link href="../shared/estilo/tables.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/aisv-general.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/work.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/catalogos.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/info.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/opc_tables.css" rel="stylesheet" />

        <script type="text/javascript">
            function BindFunctions() {
                $(document).ready(function () {
                    document.getElementById('imagen').innerHTML = '';
                    $('#tablasort').tablesorter({ cancelSelection: true, cssAsc: "ascendente", cssDesc: "descendente", headers: { 8: { sorter: false }, 9: { sorter: false}} }).tablesorterPager({ container: $('#pager'), positionFixed: false, pagesize: 10 });
                });
            }
    </script>
    <style type="text/css">
        
  #progressBackgroundFilter {
    position:fixed;
    bottom:0px;
    right:0px;
    overflow:hidden;
    z-index:1000;
    top: 0;
    left: 0;
    background-color: #CCC;
    opacity: 0.8;
    filter: alpha(opacity=80);
    text-align:center;
}
#processMessage 
{
    text-align:center;
    position:fixed;
    top:30%;
    left:43%;
    z-index:1001;
    border: 5px solid #67CFF5;
    width: 200px;
    height: 100px;
    background-color: White;
    padding:0;
}
    
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="placebody" runat="server">
    <asp:ToolkitScriptManager ID="tkcal" EnableScriptGlobalization="true" runat="server"> </asp:ToolkitScriptManager>
     
    <div>
   <i class="ico-titulo-1"></i><h2>Listo de niveles de Aprobación</h2>  <br /> 
   <i class="ico-titulo-2"></i><h1>Usuarios Por Grupos y Niveles</h1><br />
 </div>
  <div class="seccion">
       <div class="accion">

       <table class="xcontroles" cellspacing="0" cellpadding="1">
        <tr><th class="bt-bottom bt-right  bt-left bt-top" colspan="5">CRITERIOS DE BÚSQUEDA</th></tr>
         
            <tr>
         <td  class="bt-bottom bt-left bt-right" >Motivo/Concepto:</td>
         <td class="bt-bottom" >
           <asp:DropDownList ID="CboConcepto" runat="server" Width="250px" AutoPostBack="False"
                                            Height="28px"  DataTextField='description' DataValueField='id_concept'  
                                            Font-Size="Small" > </asp:DropDownList> 
          </td>
          
         
         <td class="bt-bottom bt-right validacion " colspan="3"><span id="valran2" class="validacion"> * obligatorio</span></td>
         </tr>
        

         </table>


         <div class="botonera">
          <span id="imagen"></span>
             <asp:Button ID="btbuscar" runat="server" Text="Iniciar la búsqueda"   onclick="btbuscar_Click"  />
         </div>
         
         <div class="cataresult" >



               <asp:UpdatePanel ID="upresult" runat="server">
                     <ContentTemplate>
                        <script type="text/javascript">
                            Sys.Application.add_load(BindFunctions); 
                      </script>
             <div id="xfinder" runat="server" visible="false" >
             
             <div class="findresult" >
             <div class="booking" >
                  <div class="separator">Documentos encontrados</div>
                 <div class="bokindetalle">

                  <rsweb:reportviewer ID="ReportViewer1" runat="server" Font-Names="Verdana" 
                    Font-Size="8pt" InteractiveDeviceInfos="(Colección)" 
                    WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt" 
                    >
                    <LocalReport ReportPath="nota_credito\Rpt_Niveles_Aprobacion.rdlc">
                        <DataSources>
                            <rsweb:ReportDataSource DataSourceId="SqlDataSource_Niveles" 
                                Name="nc_c_list_niveles_grupos" />
                        </DataSources>
                    </LocalReport>
                </rsweb:reportviewer>
                <asp:SqlDataSource ID="SqlDataSource_Niveles" runat="server" ConnectionString="<%$ ConnectionStrings:NOTA_CREDITO %>" SelectCommand="nc_c_list_niveles_grupos" SelectCommandType="StoredProcedure">
                     <SelectParameters>  
                      
                              <asp:SessionParameter DbType="Int32" Name="i_id_concept" SessionField="id_concept" />
                    </SelectParameters>
                </asp:SqlDataSource>
                </div>
             </div>
             </div>
             
              </div>
               <div id="sinresultado" runat="server" class="msg-info"></div>
                  </ContentTemplate>
                    
                 </asp:UpdatePanel>
             </div>
      </div>

  </div>
    <script src="../Scripts/pages.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.tablesorter.min.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.tablesorter.pager.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
              $('.datetimepicker').datetimepicker().datetimepicker({ lang: 'es', timepicker: false, closeOnDateSelect: true, format: 'd/m/Y' });
        });
  </script>

 
  </asp:Content>
