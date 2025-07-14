<%@ Page Title="Aprobación de Nota de Crédito" Language="C#" MasterPageFile="~/site.Master" AutoEventWireup="true" 
    CodeBehind="frm_aprobar_nota_credito_respaldo.aspx.cs" Inherits="CSLSite.frm_aprobar_nota_credito_respaldo" %>

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
   <i class="ico-titulo-1"></i><h2>Detalle de Transacciones pendientes de Aprobar</h2>  <br /> 
   <i class="ico-titulo-2"></i><h1>Aprobación, Visualización, Anulación de Nota de Crédito</h1><br />
 </div>
  <div class="seccion">
       <div class="accion">

         <table class="xcontroles" cellspacing="0" cellpadding="1">
        <tr><th class="bt-bottom bt-right  bt-left bt-top" colspan="5">CRITERIOS DE BÚSQUEDA</th></tr>
         <tr>
         <td  class="bt-bottom bt-left bt-right" >Número interno</td>
         <td class="bt-bottom" >
             <asp:TextBox ID="TxtNumero" runat="server" Width="120px" MaxLength="10" 
             onkeypress="return soloLetras(event,'01234567890',true)"></asp:TextBox>
          </td>
          
         
         <td class="bt-bottom bt-right validacion " colspan="3"><span id="valran" class="opcional"> * opcional</span></td>
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
         
        <asp:UpdatePanel ID="UdMotivo" runat="server" ChildrenAsTriggers="true" >
        <ContentTemplate>   
             <input id="_nc_id" type="hidden" value="" runat="server" clientidmode="Static" />
         <table class="xcontroles" cellspacing="0" cellpadding="1" id="motivo" runat="server" visible="false">
            <tr><th class="bt-bottom bt-right  bt-left bt-top" colspan="5">MOTIVO DE ANULACIÓN</th></tr>
             <tr>
            <td  class="bt-bottom bt-left bt-right" >Motivo:</td>
            <td class="bt-bottom" colspan="3">
                <asp:TextBox ID="TxtMotivo" runat="server" Width="460px" MaxLength="150" ></asp:TextBox>
            </td>
            </tr>
         </table>
          <div class="botonera" id="botones" runat="server" visible="false">
          <span id="imagen2" ></span>
             <asp:Button ID="BtnProcesar" runat="server" Text="Procesar"   onclick="btnprocesar_Click"  OnClientClick="return confirm('Esta seguro de que desea anular la nota de crédito?');"  />
         </div>
         </ContentTemplate>
          <Triggers>
              <asp:AsyncPostBackTrigger ControlID="BtnProcesar" />
            </Triggers>
         </asp:UpdatePanel>  
         <div class="cataresult" >



               <asp:UpdatePanel ID="upresult" runat="server" ChildrenAsTriggers="true" >
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
                 <th>Concepto</th>
                 <%--<th>Glosa</th>--%>
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

                 <th colspan="3">Acciones</th>
                 </tr>
                 </thead> 
                 <tbody>
                 </HeaderTemplate>
                 <ItemTemplate>
                 <tr class="point" >
                  <td><%#Eval("nc_id")%></td>
                  <td><%#Eval("nc_date_text")%></td>
                  <%--<td><a href='<%#Eval("ruta_documento") %>' style=" width:80px;"  target="_blank"> <%#Eval("description")%></a></td>--%>
                  <td><%#Eval("ruta_documento")%></a></td>
                  <td><%#Eval("num_factura")%></td>
                  <td><%#Eval("nombre_cliente")%></td>
                  <td><%#Eval("nc_total")%></td>
                  <td><%#Eval("usuarios_aprobados")%></td>
                  <td><%#Eval("usuarios_pendientes")%></td>
                  <td><%#Eval("level_text")%></td>

                  <td class="nover"><asp:Label Text='<%#Eval("id_level")%>' ID="lbl_id_level" runat="server" /></td>   
                  <td class="nover"><asp:Label Text='<%#Eval("id_group")%>' ID="lbl_id_group" runat="server" /></td>   
                  <td class="nover"><asp:Label Text='<%#Eval("IdUsuario")%>' ID="lbl_IdUsuario" runat="server" /></td>   
                  <td class="nover"><asp:Label Text='<%#Eval("level")%>' ID="lbl_level" runat="server" /></td>  

                  <td> <a href="../nota_credito/nota_credito_preview.aspx?nc_id=<%#securetext(Eval("nc_id")) %>" class="imprimir" target="_blank">Imprimir</a></td>
                  <td> <asp:Button ID="BtnAprobar"   OnClientClick="return confirm('Esta seguro de que desea aprobar la nota de crédito?');"  
                      CommandArgument= '<%#Eval("nc_id")%>' runat="server" Text="Aprobar" 
                       CssClass="Anular" ToolTip="Permite aprobar una nota de crédito" CommandName="Aprobar"  />
                  </td>
                     <td> <asp:Button ID="BtnAnular"   CommandArgument= '<%#Eval("nc_id")%>' runat="server" Text="Anular" 
                       CssClass="Anular" ToolTip="Permite anular una nota de crédito" CommandName="Anular"  />
                  </td>
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

  <%--<asp:updateprogress associatedupdatepanelid="upresult"
    id="updateProgress" runat="server">
    <progresstemplate>
        <div id="progressBackgroundFilter"></div>
        <div id="processMessage"> Estamos procesando la tarea que solicitó, por favor 
            espere...<br />
             <img alt="Loading" src="../shared/imgs/loader.gif" style="margin:0 auto;" />
        </div>
    </progresstemplate>
</asp:updateprogress>--%>
  </asp:Content>
