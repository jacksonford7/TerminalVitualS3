<%@ Page Title="Consultar Porformas" Language="C#" MasterPageFile="~/site.Master" AutoEventWireup="true" CodeBehind="consultapro.aspx.cs" Inherits="CSLSite.consultapro" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="placehead" runat="server">
    <link href="../shared/estilo/tables.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/aisv-general.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/work.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/catalogos.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/info.css" rel="stylesheet" type="text/css" />
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
      <input id="zonaid" type="hidden" value="801" />
    <div>
   <i class="ico-titulo-1"></i><h2>Proformas por servicios de Exportación </h2>  <br /> 
   <i class="ico-titulo-2"></i><h1>Consulta, reimpresión y anulación de proformas</h1><br />
 </div>
  <div class="seccion">
       <div class="accion">
            <table class="xcontroles" cellspacing="0" cellpadding="1">
        <tr><th class="bt-bottom bt-right  bt-left bt-top" colspan="5"> Datos del documento buscado</th></tr>
          <tr>
         <td class="bt-bottom bt-left bt-right"  >Booking No. :</td>
         <td class="bt-bottom bt-right" colspan="3" >
                   <span id="numbook" class="caja" onclick="clear();">...</span>
                   <input id="nbrboo" type="hidden" value="" runat="server" clientidmode="Static"/>
          </td>
         <td class="bt-bottom bt-right validacion ">
        <a  class="topopup" target="popup" onclick="window.open('../catalogo/reserva','name','width=850,height=480')" >
          <i class="ico-find" ></i> Buscar </a>
         </td>
         </tr>
         <tr>
         <td class="bt-bottom  bt-right bt-left">Generadas desde el día:</td>
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
         <td class="bt-bottom bt-right validacion ">
         <span id="valdate" class="validacion"> * obligatorio</span>
         </td>
         </tr>

         </table>
         <div class="botonera">
          <span id="imagen"></span>
             <asp:Button ID="btbuscar" runat="server" Text="Iniciar la búsqueda"   onclick="btbuscar_Click" OnClientClick="return validateDatesRange('desded','hastad','imagen');" />
         </div>
             <div class="cataresult" >
               <asp:UpdatePanel ID="upresult" runat="server" ChildrenAsTriggers="true">
                     <ContentTemplate>
                                 <script type="text/javascript">
                                     Sys.Application.add_load(BindFunctions); 
                      </script>
             <div id="xfinder" runat="server" visible="false" >
             <div class="msg-alerta" id="alerta" runat="server" >
               Confirme que los datos sean correctos. En caso de error, favor comuníquese 
               con el Departamento de Servicio al cliente a los teléfonos: +593 (04) 6006300, 3901700 
             </div>
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
                 <th>#</th>
                 <th>Secuencia</th>
                 <th>RUC</th>
                 <th>Booking</th>
                 <th>Referencia</th>
                 <th>Reservas</th>
                 <th>Cant. Prof.</th>
                 <th>Fecha gen.</th>
                 <th>Estado</th>
                 <th>Acciones</th>
                 </tr>
                 </thead> 
                 <tbody>
                 </HeaderTemplate>
                 <ItemTemplate>
                 <tr class="point" >
                  <td><%#Eval("item")%></td>
                  <td><%#Eval("secuencia")%></td>
                  <td><%#Eval("ruc")%></td>
                  <td><%#Eval("bokingnbr")%></td>
                  <td><%#Eval("referencia")%></td>
                  <td><%#Eval("reservas")%> </td>
                  <td><%#Eval("cantidad")%> </td>
                  <td><%#DataBinder.Eval(Container.DataItem, "fecha", "{0:dd-MM-yyyy HH:mm}")%></td>
                  <td><%# anulado(Eval("estado")) %></td>
                  <td>
                   <div class="tcomand">
                       <a href="../portal/printproforma.aspx?sid=<%# securetext(Eval("IdProforma")) %>" class="imprimir" target="_blank">Imprimir</a>|
                       <div class='<%# boton( Eval("estado"))%>' >
                       <asp:Button ID="btanula"  
                       OnClientClick="return confirm('Esta seguro que desea eliminar este documento?');" 
                       CommandArgument='<%# setparametros(Eval("bokingnbr"), Eval("IdProforma"),Eval("secuencia") )%>'  runat="server" Text="Anular" CssClass="Anular" ToolTip="Permite anular este documento" />
                       </div>
                   </div>
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
        function popupCallback(objeto, catalogo) {
            if (objeto == null || objeto==undefined) {
                alert('Hubo un problema al setear un objeto de catalogo');
                return;
            }
            //si catalogos es booking
            if (catalogo == 'bk') {
                document.getElementById('numbook').textContent = objeto.boking;
                document.getElementById('nbrboo').value = objeto.boking;
                return;
            }
        }

        function clear(){
            document.getElementById('numbook').textContent = '...';
            document.getElementById('nbrboo').value = '';
        }
  </script>

  <asp:updateprogress associatedupdatepanelid="upresult"
    id="updateProgress" runat="server">
    <progresstemplate>
        <div id="progressBackgroundFilter"></div>
        <div id="processMessage"> Estamos procesando la tarea que solicitó, por favor espere...<br />
             <img alt="Loading" src="../shared/imgs/loader.gif" style="margin:0 auto;" />
        </div>
    </progresstemplate>
</asp:updateprogress>
  </asp:Content>
