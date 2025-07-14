<%@ Page Title="Consultar, Imprimir y cancelar turnos" Language="C#" MasterPageFile="~/site.Master" AutoEventWireup="true" CodeBehind="cancelar.aspx.cs" Inherits="CSLSite.turno_cancela" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="placehead" runat="server">
    <link href="../shared/estilo/tables.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/aisv-general.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/work.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/catalogos.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/info.css" rel="stylesheet" type="text/css" />
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
  #aprint {
 	     color: #666;    
	     border: 1px solid #ccc;    
	     -moz-border-radius: 3px;    
	     -webkit-border-radius: 3px;    
	     background-color: #f6f6f6;    
	     padding: 0.3125em 1em;    
	     cursor: pointer;   
	     white-space: nowrap;   
	     overflow: visible;   
	     font-size: 1em;    
	     outline: 0 none /* removes focus outline in IE */;    
	     margin: 0px;    
	     line-height: 1.6em;    
	     background-image: url(../shared/imgs/action_print.gif);
	     background-repeat: no-repeat;
	     background-position:left center;
	     text-decoration:none;
	     padding:5px 2px 5px 30px;
	  
}

.tooltip{
      display: inline;
      position: relative;
  }
  
  .tooltip:hover:after{
      background: #333;
      background: rgba(0,0,0,.8);
      bottom: 26px;
      color: #fff;
      content: attr(title);
      left: 20%;
      padding: 5px 15px;
      position: absolute;
      z-index: 98;
      width: 220px;
  }
  
  .tooltip:hover:before{
      border: solid;
      border-color: #333 transparent;
      border-width: 6px 6px 0 6px;
      bottom: 20px;
      content: "";
      left: 50%;
      position: absolute;
      z-index: 99;
  }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="placebody" runat="server">
<input id="zonaid" type="hidden" value="1201" />
    <asp:ToolkitScriptManager ID="tkcal" EnableScriptGlobalization="true" runat="server"> </asp:ToolkitScriptManager>
    <div>
       <input id="agencia" type="hidden" runat="server"  clientidmode="Static"/>
   <i class="ico-titulo-1"></i><h2>Carga suelta </h2>  <br /> 
   <i class="ico-titulo-2"></i><h1>Consulta y cancelación de turnos</h1><br />
 </div>
  <div class="seccion">
       <div class="accion">
        <table class="xcontroles" cellspacing="0" cellpadding="1">
        <tr><th class="bt-bottom bt-right  bt-left bt-top" colspan="6"> Datos del documento buscado</th></tr>
         <tr>
         <td class="bt-bottom  bt-right bt-left">Booking No. :</td>
         <td class="bt-bottom bt-right " >
            <span id="numbook" class="caja"  onclick="clear();">...</span>
                   <input id="nbrboo" type="hidden" value="" runat="server" clientidmode="Static" />
             </td>
         <td class="bt-bottom bt-right validacion ">
         <a  class="topopup" target="popup" onclick="window.open('../catalogo/book','name','width=850,height=480')" >
          <i class="ico-find" ></i> Buscar </a>
         </td>
         </tr>
         <tr>
         <td class="bt-bottom  bt-right bt-left">Fecha de programación:</td>
         <td class="bt-bottom bt-right " >
             <asp:TextBox 
             style="text-align: center" 
             ID="tfecha" runat="server" ClientIDMode="Static" width="277px" MaxLength="15" CssClass="datetimepicker"
             onkeypress="return soloLetras(event,'01234567890/')" 
                  onblur="valDate(this,true,valdate);"></asp:TextBox>
         </td>
         <td class="bt-bottom bt-right validacion ">
         <span id="valdate" class="validacion"> * obligatorio</span>
         </td>
         </tr>
         <tr>
         <td class="bt-bottom  bt-right bt-left">Mail informativo:</td>
         <td class="bt-bottom bt-right " >
               <input 
                placeholder="Para enviar a varios mails separelos con (;)"
                type='text' id='tmail' 
                name='tmail'  runat="server" style= 'width:400px; text-align: left' class="date"
                enableviewstate="false" clientidmode="Static" 
                onkeypress="return soloLetras(event,'abcdefghijklmnopqrstuvwxyz1234567890_@.;',true)"  
                onblur="maildatavarios(this,'valmailz');" maxlength="250"
                />
         </td>
         <td class="bt-bottom bt-right validacion ">
         <span id="valmailz" class="validacion"></span>
         </td>
         </tr>
         </table>
         <div class="botonera">
          <span id="imagen"></span>
             <asp:Button ID="btbuscar" runat="server" Text="Iniciar la búsqueda"   
             OnClientClick="return basicData();" onclick="btbuscar_Click" 
             
             />
         </div>
         <div class="cataresult" >
               <asp:UpdatePanel ID="upresult" runat="server" ChildrenAsTriggers="true">
                     <ContentTemplate>
             <div id="xfinder" runat="server" visible="false" >
             <div class="msg-alerta" id="alerta" runat="server" >
              Seleccione booking y fecha, luego presione el botón [Iniciar la búsqueda], si desea imprimir la reserva utilice el botón
              del parte inferior [Imprimir].
             </div>
             <div class="findresult" >
             <div class="booking" >
                  <div class="separator">Programaciones encontradas</div>
                 <div class="bokindetalle">
                 <asp:Repeater ID="tablePagination" runat="server"  onitemcommand="tablePagination_ItemCommand" >
                 <HeaderTemplate>
                 <table id="tablasort" cellspacing="1" cellpadding="1" class="tabRepeat">
                 <thead>
                 <tr>
                 <th>#</th>
                 <th>Desde</th>
                 <th>Hasta</th>
                 <th class="nover">Total</th>
                 <th>Reservado</th>
                 <th class="nover">Disponible</th>
                 <th>Acciones</th>
                 <th>Acciones</th>
                 </tr>
                 </thead> 
                 <tbody>
                 </HeaderTemplate>
                 <ItemTemplate>
                 <tr class="point" >
                  <td><%#Eval("row")%><asp:HiddenField ID="hdrow" runat="server" Value='<%#Eval("row")%>' /></td>
                  <td><%#Eval("desde")%></td>
                  <td><%#Eval("hasta")%></td>
                  <td class="nover"><%#Eval("total")%></td>
                  <%--<td><%#Eval("reservado")%></td>--%>
                  <td align="center">
                      <asp:TextBox 
                      style="text-align: center" 
                      Text='<%#Eval("reservado")%>'
                      ID="caja" 
                      onblur="cajaControl(this);" 
                      runat="server" 
                      MaxLength="2" 
                      onkeypress="return soloLetras(event,'123456789',true)" >
                      </asp:TextBox>
                      <asp:HiddenField ID="hdreservado" runat="server" Value='<%#Eval("reservado")%>' />
                      <%--CssClass="suma" --%>
                      <%--xval='<%#Eval("reservado")%>' --%>
                  </td>
                  <td class="nover"><%#Eval("disponible")%></td>
                  <td>
                   <div class="tcomand">
                       <asp:Button ID="btmodifica"  
                       OnClientClick="return confirm('Esta seguro que desea modificar este documento?');" 
                       CommandName ='Modifica'
                       CommandArgument= '<%#Eval("id_horario_det")%>' runat="server" Text="Modificar" CssClass="Anular" ToolTip="Permite modificar este documento" />
                   </div>
                  </td>
                  <td>
                   <div class="tcomand">
                       <asp:Button ID="btanula"  
                       OnClientClick="return confirm('Esta seguro que desea eliminar este documento?');" 
                       CommandName ='Anula'
                       CommandArgument= '<%#Eval("id_horario_det")%>' runat="server" Text="Anular" CssClass="Anular" ToolTip="Permite anular este documento" />
                   </div>
                  </td>
                 </tr>
                  </ItemTemplate>
                 <FooterTemplate>
                 </tbody>
                 </table>
                 </FooterTemplate>
         </asp:Repeater>

        <fieldset>
        <br />
        <a href="#" target="_blank" runat="server" id="aprint" clientidmode="Static" >Imprimir</a>
        
        </fieldset>
              
     
                </div>
             </div>
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
          function basicData() {
              var fecha = document.getElementById('tfecha').value;
              var nbook = document.getElementById('nbrboo').value;
              if (fecha.trim().length <= 0) {
                  alert('Escriba la fecha de programación');
                  return false;
              }
              if (nbook.trim().length <= 0) {
                  alert('Escriba el número de booking');
                  return false;
              }
              return true;
          }

          function popupCallback(objeto, catalogo) {
              if (objeto == null || objeto == undefined) {
                  alert('Hubo un problema al setaar un objeto de catalogo');
                  return;
              }
              //si catalogos es booking
              if (catalogo == 'bk') {
                  document.getElementById('numbook').textContent = objeto.nbr;
                  document.getElementById('nbrboo').value = objeto.nbr;
                  return;
              }
          }

          function clear() {
              document.getElementById('numbook').textContent = '...';
              document.getElementById('nbrboo').value = '';
          }
  </script>

<asp:updateprogress  id="updateProgress" runat="server">
    <progresstemplate>
        <div id="progressBackgroundFilter"></div>
        <div id="processMessage"> Estamos procesando la tarea que solicitó, por favor espere...<br />
             <img alt="Loading" src="../shared/imgs/loader.gif" style="margin:0 auto;" />
        </div>
    </progresstemplate>
</asp:updateprogress>
  </asp:Content>
