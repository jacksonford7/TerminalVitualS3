<%@ Page Title="Consultar, Imprimir y cancelar turnos" Language="C#" MasterPageFile="~/site.Master" AutoEventWireup="true" CodeBehind="cancelar.aspx.cs" Inherits="CSLSite.turno_cancela" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="placehead" runat="server">
    <link href="../shared/estilo/tables.css" rel="stylesheet" type="text/css" />
   
    <link href="../shared/estilo/catalogos.css" rel="stylesheet" type="text/css" />

          <link href="../shared/estilo/jquery.datetimepicker.css" rel="stylesheet" type="text/css" />
     <!--mensajes-->
        <link href="https://cdn.jsdelivr.net/npm/alertifyjs@1.11.0/build/css/alertify.min.css" rel="stylesheet"/>
        <script type="text/javascript" src="https://cdn.jsdelivr.net/npm/alertifyjs@1.11.0/build/alertify.min.js"></script>

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
    <input id="agencia" type="hidden" runat="server"  clientidmode="Static"/>
    <asp:ToolkitScriptManager ID="tkcal" EnableScriptGlobalization="true" runat="server"> </asp:ToolkitScriptManager>

         <div class="mt-4">         
        <nav class="mt-4" aria-label="breadcrumb">
          <ol class="breadcrumb">
            <li class="breadcrumb-item" id="opcion_principal" runat="server"><a href="#">Exportación</a></li>
             <li class="breadcrumb-item active" aria-current="page" id="sub_opcion" runat="server">Consulta y cancelación de horarios (CFS)</li>
          </ol>
        </nav>
      </div>

     <div class="dashboard-container p-4" id="cuerpo" runat="server">

         <div class="form-title">Datos del documento buscado</div>
		 
		  <div class="form-row">
		  
		   <div class="form-group col-md-6"> 
		   	  <label for="inputAddress">Booking<span style="color: #FF0000; font-weight: bold;"></span></label>
			  <div class="d-flex">
                              <span id="numbook" class=" form-control col-md-10"  onclick="clear();">...</span>
                   <input id="nbrboo" type="hidden" value="" runat="server" clientidmode="Static" />
                           <a  class="btn btn-outline-primary mr-4" target="popup" 
             onclick="window.open('../catalogo/bookinListConsolidacion.aspx','name','width=850,height=880')" >
         <span class='fa fa-search' style='font-size:24px'></span> 

         </a>


			  </div>
		   </div>
		
		  
		   <div class="form-group col-md-6"> 
		   	  <label for="inputAddress">Fecha de programación<span style="color: #FF0000; font-weight: bold;"></span></label>
			  <div class="d-flex">
                         <asp:TextBox 
          
             ID="tfecha" runat="server" ClientIDMode="Static"  MaxLength="15" 
                 CssClass="datetimepicker  form-control"
             onkeypress="return soloLetras(event,'01234567890/')" 
                  onblur="valDate(this,true,valdate);"></asp:TextBox>
                           <span id="valdate" class="validacion"> * </span>

			  </div>
		   </div>
		  </div>
           <div class="form-row">
		  
		   <div class="form-group col-md-12"> 
		   	  <label for="inputAddress">Mail informativo<span style="color: #FF0000; font-weight: bold;"></span></label>
			  <div class="d-flex">
                    <input 
                placeholder="Para enviar a varios mails separelos con (;)"
                type='text' id='tmail' 
                name='tmail'  runat="server"  class="date  form-control"
                enableviewstate="false" clientidmode="Static" 
                onkeypress="return soloLetras(event,'abcdefghijklmnopqrstuvwxyz1234567890_@.;',true)"  
                onblur="maildatavarios(this,'valmailz');" maxlength="250"
                />
                           <span id="valmailz" class="validacion"></span>

			  </div>
		   </div>
		  </div>
		   <div class="form-row">
		   <div class="col-md-12 d-flex justify-content-center"> 
                   
             <asp:Button ID="btbuscar" runat="server" Text="Iniciar la búsqueda"   
             OnClientClick="return basicData();" onclick="btbuscar_Click" 
                  CssClass="btn btn-primary"
             
             />
                <span id="imagen"></span>
		   </div> 
		   </div>

              <div class="cataresult" >
               <asp:UpdatePanel ID="upresult" runat="server" ChildrenAsTriggers="true">
                     <ContentTemplate>
             <div id="xfinder" runat="server" visible="false" >
             <div class=" alert-primary" id="alerta" runat="server" >
              Seleccione booking y fecha, luego presione el botón [Iniciar la búsqueda], si desea imprimir la reserva utilice el botón
              del parte inferior [Imprimir].
             </div>
           
             <div class="booking" >
                  <div class="form-title">Programaciones encontradas</div>
                 <div class="bokindetalle">
                 <asp:Repeater ID="tablePagination" runat="server"  onitemcommand="tablePagination_ItemCommand" >
                 <HeaderTemplate>
                 <table id="tablasort" cellspacing="1" cellpadding="1" class="table table-bordered invoice">
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
                      CssClass="form-control"
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
                       CommandArgument= '<%#Eval("id_horario_det")%>' runat="server"
                           Text="Modificar" CssClass=" btn btn-secondary" ToolTip="Permite modificar este documento" />
                   </div>
                  </td>
                  <td>
                   <div class="tcomand">
                       <asp:Button ID="btanula"   
                       OnClientClick="return confirm('Esta seguro que desea eliminar este documento?');" 
                       CommandName ='Anula'
                       CommandArgument= '<%#Eval("id_horario_det")%>' runat="server" Text="Anular" 
                           CssClass=" btn  btn-secondary" ToolTip="Permite anular este documento" />
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
        <a href="#" class="btn btn-secondary" target="_blank" runat="server" id="aprint" clientidmode="Static" >Imprimir</a>
        
        </fieldset>
              
     
                </div>
             </div>
        
              </div>
                         <div class="alert-modal">
 <div id="sinresultado" runat="server" class=" alert-secondary"></div>
                         </div>
              
                  </ContentTemplate>
                     <Triggers>
                     <asp:AsyncPostBackTrigger ControlID="btbuscar" />
                     </Triggers>
                 </asp:UpdatePanel>
             </div>
     </div>





   
 
    
 
    <script src="../Scripts/pages.js" type="text/javascript"></script>
        <script src="../Scripts/jquery.datetimepicker.js" type="text/javascript"></script>
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
                   alertify.alert('Escriba la fecha de programación');
                  return false;
              }
              if (nbook.trim().length <= 0) {
                   alertify.alert('Escriba el número de booking');
                  return false;
              }
              return true;
          }

          function popupCallback(objeto, catalogo) {
              if (objeto == null || objeto == undefined) {
               alertify.alert('Hubo un problema al setaar un objeto de catalogo');
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
