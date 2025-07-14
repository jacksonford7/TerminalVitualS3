<%@ Page Title="Notas de Créditos Pendientes de Aprobaciones" Language="C#" MasterPageFile="~/site.Master" AutoEventWireup="true" 
    CodeBehind="frm_pendientes_nota_credito.aspx.cs" Inherits="CSLSite.frm_pendientes_nota_credito" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
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
   <i class="ico-titulo-1"></i><h2>Seguimiento a Transacciones pendientes de Aprobar</h2>  <br /> 
   <i class="ico-titulo-2"></i><h1>Detalle de Notas de Créditos Emitidas</h1><br />
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
                 <asp:Repeater ID="tablePagination" runat="server" 
                         onitemcommand="tablePagination_ItemCommand" >
                 <HeaderTemplate>
                 <table id="tablasort" cellspacing="1" cellpadding="1" class="tabRepeat">
                 <thead>
                 <tr>
                 <th>No.</th>
                 <th>Fecha</th>
                 <th>Concepto<br>Archivo</th>
                 <th>Factura</th>
                 <th>Cliente</th>
                 <th>Total $</th>
                 <th>Aprobados</th>
                 <th>Pendientes</th>
                 <th>Nivel Actual</th>

                 <th class="nover">Nivel Aprob</th>
                 <th class="nover">Grupo</th>
                 <th class="nover">Usuario</th>
                 <th class="nover">Nivel</th>
                  <th class="nover">Estado</th>
                 <th colspan="3">Acciones</th>
                 </tr>
                 </thead> 
                 <tbody>
                 </HeaderTemplate>
                 <ItemTemplate>
                 <tr class="point" >
                  <td><asp:Label Text='<%#Eval("nc_id")%>' ID="lbl_nc_id" runat="server" /></td>
                  <td><asp:Label Text='<%#Eval("nc_date_text")%>' ID="lbl_nc_date_text" runat="server" /></td>
                  <td><asp:Label Text='<%#Eval("description")%>' ID="lbl_description" runat="server" /><br>
                      <a href="../nota_credito/lookup_download_archivo.aspx?nc_id=<%#securetext(Eval("nc_id")) %>"  target="_blank" >Descargar Archivos</a>
                  </td>
                  <td><asp:Label Text='<%#Eval("num_factura")%>' ID="lbl_num_factura" runat="server" /></td>
                  <td><asp:Label Text='<%#Eval("nombre_cliente")%>' ID="lbl_nombre_cliente" runat="server" /></td>
                  <td><asp:Label Text='<%#Eval("nc_total")%>' ID="lbl_nc_total" runat="server" /></td>
                  <td><asp:Label Text='<%#Eval("usuarios_aprobados")%>' ID="lbl_usuarios_aprobados" runat="server" /></td>
                  <td><asp:Label Text='<%#Eval("usuarios_pendientes")%>' ID="lbl_usuarios_pendientes" runat="server" /></td>
                  <td><asp:Label Text='<%#Eval("level_text")%>' ID="lbl_level_text" runat="server" /></td>

                  <td class="nover"><asp:Label Text='<%#Eval("id_level")%>' ID="lbl_id_level" runat="server" /></td>   
                  <td class="nover"><asp:Label Text='<%#Eval("id_group")%>' ID="lbl_id_group" runat="server" /></td>   
                  <td class="nover"><asp:Label Text='<%#Eval("IdUsuario")%>' ID="lbl_IdUsuario" runat="server" /></td>   
                  <td class="nover"><asp:Label Text='<%#Eval("level")%>' ID="lbl_level" runat="server" /></td>  
                  <td class="nover"><asp:Label Text='<%#Eval("estado")%>' ID="lbl_estado" runat="server" /></td>  

                  <td> <a href="../nota_credito/nota_credito_preview.aspx?nc_id=<%#securetext(Eval("nc_id")) %>" class="imprimir" target="_blank">Imprimir</a></td>  
                 </tr>
                  </ItemTemplate>
                 <FooterTemplate>
                 </tbody>
                 </table>
                 </FooterTemplate>
         </asp:Repeater>
                </div>
             </div>
             </div>
             <div id="pager">
               Registros por página
                  <select class="pagesize">
                  <option selected="selected" value="10">10</option>
                  <option value="20">20</option>
                  </select>
                 <img alt="" src="../shared/imgs/first.gif" class="first"/>
                 <img alt="" src="../shared/imgs/prev.gif" class="prev"/>
                 <input  type="text" class="pagedisplay" size="5px"/>
                 <img alt="" src="../shared/imgs/next.gif" class="next"/>
                 <img alt="" src="../shared/imgs/last.gif" class="last"/>
                 <input clientidmode="Static" id="dataexport" onclick="getTable('resultado');" type="button" value="Exportar" runat="server" />
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
