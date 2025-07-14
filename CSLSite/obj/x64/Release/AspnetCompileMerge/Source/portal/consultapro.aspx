<%@ Page Title="Consultar Proformas-Liq" Language="C#" MasterPageFile="~/site.Master" AutoEventWireup="true" CodeBehind="consultapro.aspx.cs" Inherits="CSLSite.consultapro" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="placehead" runat="server">
       <!--Datatables-->
       <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/5.15.1/css/all.min.css" integrity="sha512-+4zCK9k+qNFUR5X+cKL9EIR+ZOhtIloNl9GIKS57V1MyNsYpYcUrUeQc9vNfzsWfV28IaLL3i96P9sdNyeRssA==" crossorigin="anonymous" />
       <link href="../css/datatables.min.css" rel="stylesheet" />
       <link rel="stylesheet" type="text/css" href="..js/buttons/css1.6.4/buttons.dataTables.min.css"/>
        <script src="../lib/jquery/jquery.min.js" type="text/javascript"></script>
       <script type="text/javascript"  src="../js/datatables.js"></script>
       <script src="../Scripts/table_catalog.js" type="text/javascript"></script>  
       <script type="text/javascript" src="../js/bootstrap.bundle.min.js"></script>
    <!--Fin-->
    
    
    
    
    
    <link href="../shared/estilo/jquery.datetimepicker.css" rel="stylesheet" type="text/css" />




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

           <div class="mt-4">         
        <nav class="mt-4" aria-label="breadcrumb">
          <ol class="breadcrumb">
            <li class="breadcrumb-item" id="opcion_principal" runat="server"><a href="#">Servicios</a></li>
             <li class="breadcrumb-item active" aria-current="page" id="sub_opcion" runat="server">Consulta, reimpresión y anulación de proformas</li>
          </ol>
        </nav>
      </div>

        <div class="dashboard-container p-4" id="cuerpo" runat="server">

         <div class="form-title">Datos del documento buscado</div>
		  <div class="form-row">


		 
		  
		   <div class="form-group  col-md-3"> 
		   	  <label for="inputAddress">Desde<span style="color: #FF0000; font-weight: bold;"></span></label>
			  <div class="d-flex">
                  <asp:TextBox ID="desded" runat="server"  MaxLength="10" CssClass="datetimepicker form-control"
             onkeypress="return soloLetras(event,'01234567890/')" 
                 onblur="valDate(this,true,valdate);" ClientIDMode="Static"></asp:TextBox>

			  </div>
		   </div>
          
		  
		   <div class="form-group col-md-3">
                
		   	  <label for="inputAddress">Hasta<span style="color: #FF0000; font-weight: bold;"></span></label>
			   <div class="d-flex"> 
               <asp:TextBox ID="hastad" runat="server" ClientIDMode="Static" MaxLength="15"
                    CssClass="datetimepicker form-control"
             onkeypress="return soloLetras(event,'01234567890/')" 
                  onblur="valDate(this,true,valdate);"></asp:TextBox>
               <span id="valdate" class="validacion"> *</span>
                    </div>
		   </div>
              		   <div class="form-group col-md-4"> 
		   	  <label for="inputAddress">Booking<span style="color: #FF0000; font-weight: bold;"></span></label>
			  <div class="d-flex">
<span id="numbook" class=" form-control  col-md-10" onclick="clear();">...</span>
                   <input id="nbrboo" type="hidden" value="" runat="server" clientidmode="Static"/>
                    <a  class="btn btn-outline-primary mr-4" target="popup" 
                        onclick="window.open('../catalogo/bookingPro.aspx','name','width=850,height=880')" >
          <span class='fa fa-search' style='font-size:24px'></span> 

                    </a>
			  </div>
		   </div>




                      		   <div class="form-group col-md-2"> 
                                     		   	  <label for="inputAddress">&nbsp;<span style="color: #FF0000; font-weight: bold;"></span></label>

               <div class="d-flex">
             <asp:Button ID="btbuscar" runat="server"   CssClass="btn btn-primary"
                 Text="Buscar"   
                 onclick="btbuscar_Click" 
                 OnClientClick="return validateDatesRange('desded','hastad','imagen');" />
                                     <span id="imagen"></span></div>
		   </div> 
	

		  </div>


               <div class="form-row">
		   <div class="form-group col-md-12 "> 
                              <asp:UpdatePanel ID="upresult" runat="server" ChildrenAsTriggers="true">
                     <ContentTemplate>
                                 <script type="text/javascript">
                                     Sys.Application.add_load(BindFunctions); 
                      </script>
             <div id="xfinder" runat="server" visible="false" >
             <div class=" alert alert-warning" id="alerta" runat="server" >
               Confirme que los datos sean correctos. En caso de error, favor comuníquese 
               con el Departamento de Servicio al cliente a los teléfonos: +593 (04) 6006300, 3901700 
             </div>

                 
        
                 <asp:Repeater ID="tablePagination" runat="server"  onitemcommand="tablePagination_ItemCommand" >
                 <HeaderTemplate>
                 <table id="tablasort" cellspacing="1" cellpadding="1" class="table table-bordered table-sm table-contecon">
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
                 <th></th>
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
                       <a href="../portal/printproforma.aspx?sid=<%# securetext(Eval("IdProforma")) %>" class=" btn btn-link" target="_blank">Imprimir</a>|
                       <div class='<%# boton( Eval("estado"))%>' >
                       <asp:Button ID="btanula"  
                       OnClientClick="return confirm('Esta seguro que desea eliminar este documento?');" 
                       CommandArgument='<%# setparametros(Eval("bokingnbr"), Eval("IdProforma"),Eval("secuencia"), Eval("liquidacion") )%>'  runat="server" Text="Anular" CssClass=" btn btn-secondary" ToolTip="Permite anular este documento" />
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
               <div id="sinresultado" runat="server" class=" alert alert-primary"></div>
                  </ContentTemplate>
                     <Triggers>
                     <asp:AsyncPostBackTrigger ControlID="btbuscar" />
                     </Triggers>
                 </asp:UpdatePanel>

		   </div> 
		   </div>
          
		 
     </div>


     <script src="../Scripts/jquery.datetimepicker.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
              $('.datetimepicker').datetimepicker().datetimepicker({ lang: 'es', timepicker: false, closeOnDateSelect: true, format: 'd/m/Y' });
          });
        function popupCallback(objeto, catalogo) {
            if (objeto == null || objeto==undefined) {
              alertify.alert('Hubo un problema al setear un objeto de catalogo');
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
