<%@ Page Title="Listado Detallado de Nota de Crédito" Language="C#" MasterPageFile="~/site.Master" AutoEventWireup="true" 
    CodeBehind="frm_listado_det_nota_credito.aspx.cs" Inherits="CSLSite.frm_listado_det_nota_credito" %>

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
   <i class="ico-titulo-1"></i><h2>Listado Detallado de Nota de Crédito</h2>  <br /> 
        <%--  <i class="ico-titulo-2"></i><h1>Podra visualizar </h1><br />--%>
 </div>
  <div class="seccion">
       <div class="accion">

         <table class="xcontroles" cellspacing="0" cellpadding="1">
        <tr><th class="bt-bottom bt-right  bt-left bt-top" colspan="5">CRITERIOS DE BÚSQUEDA</th></tr>
         <tr>
         <td  class="bt-bottom bt-left bt-right" >Estado</td>
         <td class="bt-bottom" >
             <asp:DropDownList ID="CboEstado" runat="server" Width="150px" AutoPostBack="False"
                                            Height="28px"  DataTextField='name' DataValueField='id'  
                                            Font-Size="Small" 
                                            >
                                        </asp:DropDownList>
          </td>
          
         
         <td class="bt-bottom bt-right validacion " colspan="3"><span id="valran" class="validacion"> * obligatorio</span></td>
         </tr>
            <tr>
         <td  class="bt-bottom bt-left bt-right" >Motivo/Concepto:</td>
         <td class="bt-bottom" >
           <asp:DropDownList ID="CboConcepto" runat="server" Width="250px" AutoPostBack="False"
                                            Height="28px"  DataTextField='description' DataValueField='id_concept'  
                                            Font-Size="Small" > </asp:DropDownList> 
          </td>
          
         
         <td class="bt-bottom bt-right validacion " colspan="3"><span id="valran2" class="validacion"> * obligatorio</span></td>
         </tr>
         <tr>
         <td class="bt-bottom  bt-right bt-left">Desde:</td>
         <td class="bt-bottom">
            <asp:TextBox ID="desded" runat="server" Width="120px" MaxLength="10" CssClass="datetimepicker"
             onkeypress="return soloLetras(event,'01234567890/')" 
                 onblur="valDate(this,true,valdate);" ClientIDMode="Static"></asp:TextBox>

             </td>
          <td class="bt-bottom" >Hasta:</td>
          <td class="bt-bottom " >
             <asp:TextBox ID="hastad" runat="server" ClientIDMode="Static" Width="120px" MaxLength="15" CssClass="datetimepicker"
             onkeypress="return soloLetras(event,'01234567890/')" 
                  onblur="valDate(this,true,valdate);"></asp:TextBox>
   
          </td>
         <td class="bt-bottom bt-right validacion " colspan ="2">
         <span id="valdate" class="validacion"> * obligatorio</span>
         </td>
         </tr>

         </table>
            
         <div class="botonera">
          <span id="imagen"></span>
             <asp:Button ID="btbuscar" runat="server" Text="Iniciar la búsqueda"   onclick="btbuscar_Click" OnClientClick="return validateDatesRange('desded','hastad','imagen');" />
         </div>
         
               <div class="cataresult" >



               <asp:UpdatePanel ID="upresult" runat="server" ChildrenAsTriggers="true" >
                     <ContentTemplate>
                        <script type="text/javascript">
                            Sys.Application.add_load(BindFunctions); 
                      </script>
             <div id="xfinder" runat="server" visible="false" >
             
             <div class="findresult" >
             <div class="booking" >
                 <div class="bokindetalle">
                <rsweb:reportviewer ID="ReportViewer1" runat="server" Font-Names="Verdana" 
                    Font-Size="8pt" InteractiveDeviceInfos="(Colección)" 
                    WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt" 
                    >
                    <LocalReport ReportPath="nota_credito\Rpt_Detalle_Nota_Credito.rdlc">
                        <DataSources>
                            <rsweb:ReportDataSource DataSourceId="SqlDataSource_Detalle" 
                                Name="nc_c_list_credit_detallada" />
                        </DataSources>
                    </LocalReport>
                </rsweb:reportviewer>
                      <asp:SqlDataSource ID="SqlDataSource_Detalle" runat="server" ConnectionString="<%$ ConnectionStrings:NOTA_CREDITO %>" SelectCommand="nc_c_list_credit_detallada" SelectCommandType="StoredProcedure">        
                         <SelectParameters>  
                       <asp:SessionParameter DbType="Int32" Name="i_Estado" SessionField="estado" />
                          <asp:SessionParameter  Type="String"  Name="i_Desde"   SessionField="fecha_desde" />
                          <asp:SessionParameter   Type="String"  Name="i_Hasta" SessionField="fecha_hasta" />      
                              <asp:SessionParameter DbType="Int32" Name="i_id_concept" SessionField="id_concept" />
                    </SelectParameters>
                      </asp:SqlDataSource>
                </div>
             </div>
             </div>
             
              </div>
               <div id="sinresultado" runat="server" class="msg-info"></div>
                  </ContentTemplate>
                     <Triggers>
                     <asp:AsyncPostBackTrigger ControlID="btbuscar" />
                     <%--<asp:AsyncPostBackTrigger ControlID="BtnProcesar" />--%>
                     </Triggers>
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

  <%--<asp:AsyncPostBackTrigger ControlID="BtnProcesar" />--%>
  </asp:Content>
