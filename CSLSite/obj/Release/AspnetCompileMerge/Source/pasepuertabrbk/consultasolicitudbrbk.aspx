<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="consultasolicitudbrbk.aspx.cs" Inherits="CSLSite.consultasolicitudbrbk" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>


<asp:Content ID="Content1" ContentPlaceHolderID="placehead" runat="server">

  <link href="../img/favicon2.png" rel="icon"/>
  <link href="../img/icono.png" rel="apple-touch-icon"/>

  <link href="../css/bootstrap.min.css" rel="stylesheet"/>
  <link href="../css/dashboard.css" rel="stylesheet"/>
  <link href="../css/icons.css" rel="stylesheet"/>
  <link href="../css/style.css" rel="stylesheet"/>

  <link href="../shared/estilo/jquery.datetimepicker.css" rel="stylesheet" type="text/css" />
  <script src="../Scripts/jquery.datetimepicker.js" type="text/javascript"></script>

    
     <link href="../lib/font-awesome/css/font-awesome.css" rel="stylesheet" />
  <link rel="stylesheet" type="text/css" href="../lib/gritter/css/jquery.gritter.css" />

 
 <link href="../css/calendario_ajax.css" rel="stylesheet"/>


  <link href="https://fonts.googleapis.com/css2?family=Roboto:wght@300;400;500;700&display=swap" rel="stylesheet"/>
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/5.15.1/css/all.min.css" integrity="sha512-+4zCK9k+qNFUR5X+cKL9EIR+ZOhtIloNl9GIKS57V1MyNsYpYcUrUeQc9vNfzsWfV28IaLL3i96P9sdNyeRssA==" crossorigin="anonymous" />


  <link href="../css/datatables.min.css" rel="stylesheet" />
  <link rel="stylesheet" type="text/css" href="..js/buttons/css1.6.4/buttons.dataTables.min.css"/>


<script type="text/javascript">

 function BindFunctions() {
     $(document).ready(function() {
      /*
       * Insert a 'details' column to the table
       */
      var nCloneTh = document.createElement('th');
      var nCloneTd = document.createElement('td');
      nCloneTd.innerHTML = '<img src="../lib/advanced-datatable/images/details_open.png">';
      nCloneTd.className = "center";

      $('#hidden-table-info thead tr').each(function() {

      });

      $('#hidden-table-info tbody tr').each(function() {
       
      });

      /*
       * Initialse DataTables, with no sorting on the 'details' column
       */
      var oTable = $('#hidden-table-info').dataTable({
        "aoColumnDefs": [{
          "bSortable": false,
          "aTargets": [0]
        }],
        "aaSorting": [
          [1, 'asc']
        ]
      });     
        });
    }
</script>


<script type="text/javascript">

function BindFunctions_Lista()
   {
   $(document).ready(function() {    
    $('#grilla').DataTable({        
        language: {
                "lengthMenu": "_MENU_ REGISTROS POR PAGINA",
                "zeroRecords": "No se encontraron resultados",
                "info": "Mostrando registros del _START_ al _END_ de un total de _TOTAL_ registros",
                "infoEmpty": "Mostrando registros del 0 al 0 de un total de 0 registros",
                "infoFiltered": "(filtrado de un total de _MAX_ registros)",
                "sSearch": "Buscar:",
			     "sProcessing":"Procesando...",
            },
        //para usar los botones   
        responsive: "true",
        dom: 'Bfrtilp',    
        buttons: [  
            {
				extend:    'excel',
				text:      '<i class="fa fa-file-excel-o"></i> ',
				titleAttr: 'Exportar a Excel',
				className: 'btn btn-primary'
			},
			{
				extend:    'pdf',
				text:      '<i class="fa fa-file-pdf-o"></i> ',
				titleAttr: 'Exportar a PDF',
				className: 'btn btn-primary'
			},
			{
				extend:    'print',
				text:      '<i class="fa fa-print"></i> ',
				titleAttr: 'Imprimir',
				className: 'btn btn-primary'
			},
        ]	 
    });     
});

    }

</script>
 
 

</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="placebody" runat="server" >
    
    <asp:ToolkitScriptManager ID="tkscata" runat="server"></asp:ToolkitScriptManager>
      <asp:HiddenField ID="manualHide" runat="server" />
     <script type="text/javascript">
              Sys.Application.add_load(Calendario); 
     </script>  

  <div id="div_BrowserWindowName" style="visibility:hidden;">
    <asp:HiddenField ID="hf_BrowserWindowName" runat="server" />
    <asp:HiddenField ID="hf_idsolicitudgenerada" runat="server" />
  </div>
    

      <div class="mt-4">         
        <nav class="mt-4" aria-label="breadcrumb">
          <ol class="breadcrumb">
            <li class="breadcrumb-item" id="opcion_principal" runat="server"><a href="#">BREAK BULK</a></li>
            <li class="breadcrumb-item active" aria-current="page" id="sub_opcion" runat="server">CONSULTA DE SOLICITUD DE TURNOS (BRBK)</li>
          </ol>
        </nav>
      </div>



 <div class="dashboard-container p-4" id="cuerpo_lista" runat="server">   
     <div class="form-title">
             CRITERIOS DE BÚSQUEDAS
     </div>

     <div class="form-row"> 
        <div class="form-group col-md-6"> 
                 <label for="inputEmail4">FECHA DESDE:</label>
                <asp:TextBox ID="TxtFechaConDesde" runat="server"  class="datetimepicker form-control"  MaxLength="10"  placeholder="dd/MM/yyyy" onkeypress="return soloLetras(event,'1234567890/:')"></asp:TextBox>
        </div>
        <div class="form-group col-md-6">  
                <label for="inputEmail4">FECHA HASTA:</label>
                <asp:TextBox ID="TxtFechaConHasta" runat="server"  class="datetimepicker form-control" MaxLength="10"  placeholder="dd/MM/yyyy" onkeypress="return soloLetras(event,'1234567890/:')"></asp:TextBox>
                          
        </div>
     </div>

     <asp:UpdatePanel ID="UpdatePanel1" runat="server"  UpdateMode="Conditional" ChildrenAsTriggers="true">
     <ContentTemplate>

     <div class="row">
            <div class="col-md-12 d-flex justify-content-center">
                  <asp:Button ID="BtnBuscar" runat="server" class="btn btn-primary"   Text="BUSCAR"  OnClientClick="return mostrarloader('1')" OnClick="BtnBuscar_Click" />                             
                  <img alt="loading.." src="../lib/file-uploader/img/loading.gif" height="32px" width="32px"  id="ImgCarga" class="nover"   />
            </div>    
     </div>    
           <br/>
      <div class="row">
        <div class="col-md-12 d-flex justify-content-center">
              <div class="alert alert-warning" id="banmsg_lista" runat="server" clientidmode="Static"><b>Error!</b> >......</div>
        </div>
      </div>

    </ContentTemplate>
    <Triggers>
    <asp:AsyncPostBackTrigger ControlID="BtnBuscar" />
</Triggers>
</asp:UpdatePanel>

      <div class="form-title">
          DETALLE DE SOLICITUDES PENDIENTES
     </div>

     


         <div class="form-row">
          <div class="form-group col-md-12">
         <asp:UpdatePanel ID="UPLISTA" runat="server"  >  
            <ContentTemplate>

             <script type="text/javascript">
                     Sys.Application.add_load(BindFunctions_Lista); 
            </script>

                       <asp:Repeater ID="grilla" runat="server" onitemcommand="grilla_ItemCommand" >
                       <HeaderTemplate>
                       <table cellpadding="0" cellspacing="0" border="0" class="table table-bordered table-sm table-contecon" id="grilla" width="100%" >
                        <thead>
                          <tr >
                            <th class="center hidden-phone">SOLICITUD</th>
                            <th class="center hidden-phone">CARGA</th>
                            <th class="center hidden-phone">FECHA</th>
                            <th class="center hidden-phone"># VEHICULO</th>
                            <th class="center hidden-phone">TOT/BULTOS</th>
                            <th class="center hidden-phone">BODEGA</th>
                            <th class="center hidden-phone">TIP/PRODUCTO</th>
                            <th class="center hidden-phone">AGENTE</th>
                            <th class="center hidden-phone">USUARIO</th>
                            <th class="center hidden-phone">APROBADOS</th>
                            <th class="center hidden-phone">PENDIENTES</th>
                            <th class="center hidden-phone">VER DETALLE</th>
                          </tr>
                        </thead>
                        <tbody>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr >
                                <td class="center hidden-phone"><asp:Label Text='<%#Eval("ID_SOL")%>' ID="ID_SOL" runat="server"  style="align-content:center"   /></td>
                                <td class="center hidden-phone"><asp:Label Text='<%#Eval("NUMERO_CARGA")%>' ID="NUMERO_CARGA" runat="server"  style="text-align:center"  /></td>
                                <td class="center hidden-phone"><asp:Label Text='<%#DataBinder.Eval(Container.DataItem, "FECHA_SOL", "{0:dd/MM/yyyy}")%>' ID="FECHA_SOL" runat="server"  style="text-align:center"  /></td>
                                <td class="center hidden-phone"><asp:Label Text='<%#Eval("CANTIDAD_VEHI")%>' ID="CANTIDAD_VEHI" runat="server"  style="text-align:center"  /></td> 
                                <td class="center hidden-phone"><asp:Label Text='<%#Eval("TOTAL_BUTLOS")%>' ID="TOTAL_BUTLOS" runat="server"  style="text-align:center"  /></td> 
                                <td class="center hidden-phone"><asp:Label Text='<%#Eval("BODEGA")%>' ID="BODEGA" runat="server"  style="text-align:center"  /></td> 
                                <td class="center hidden-phone"><asp:Label Text='<%#Eval("TIPO_PRODUCTO")%>' ID="TIPO_PRODUCTO" runat="server"  style="text-align:center"  /></td> 
                                <td class="center hidden-phone"><asp:Label Text='<%#Eval("AGENTE_DESC")%>' ID="ESTADO" runat="server"  style="text-align:center"  /></td> 
                                <td class="center hidden-phone"><asp:Label Text='<%#Eval("USUARIO_DESC")%>' ID="Label1" runat="server"  style="text-align:center"  /></td> 
                                <td class="center hidden-phone"><asp:Label Text='<%#Eval("APROBADOS")%>' ID="Label2" runat="server"  style="text-align:center"  /></td> 
                                <td class="center hidden-phone"><asp:Label Text='<%#Eval("PENDIENTES")%>' ID="Label3" runat="server"  style="text-align:center"  /></td> 
                                <td class="center hidden-phone"> 
                                    <asp:Button ID="BtnDetalle"
                                    CommandArgument= '<%#Eval("ID_SOL")%>' runat="server" Text="VER DETALLE"  class="btn btn-primary" data-toggle="modal" data-target="#exampleModalToggle" ToolTip="VER DETALLE" CommandName="Ver" />
                                </td>
                               
                             </tr>    
                       </ItemTemplate>
                       <FooterTemplate>
                        </tbody>
                      </table>
                     </FooterTemplate>
                    </asp:Repeater>

          </ContentTemplate>
       
           </asp:UpdatePanel>
         </div>
     </div>




 </div>


<div class="modal fade" id="exampleModalToggle" tabindex="-1" role="dialog" >
<div class="modal-dialog modal-dialog-scrollable" style="max-width: 1500px">
   <div class="modal-content">

        <div class="dashboard-container p-4" id="Div1" runat="server">   
             <div class="modal-header">
               <asp:UpdatePanel ID="UPTITULO" runat="server"  UpdateMode="Conditional" ChildrenAsTriggers="true">
               <ContentTemplate>
                            <h5 class="modal-title" id="Titulo" runat="server">SOLICITUD PENDIENTE DE APROBAR</h5>
                       
                   </ContentTemplate>
             </asp:UpdatePanel>
                <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span>
                </button>
            </div>



             <div class="form-title">
                   DATOS DE LA CARGA
             </div>

             <div class="modal-body">
                 <asp:UpdatePanel ID="UPCARGA" runat="server"  UpdateMode="Conditional" >
                    <ContentTemplate>
			            <div class="form-row">
                            <div class="form-group col-md-4">
                            <label for="inputZip">MRN<span style="color: #FF0000; font-weight: bold;">*</span></label>
                            <asp:TextBox ID="TXTMRN" runat="server" class="form-control" MaxLength="16"  onkeypress="return soloLetras(event,'abcdefghijklmnopqrstuvwxyz0123456789-_')"  
                                                placeholder="MRN" disabled></asp:TextBox>
                        </div>
                            <div class="form-group col-md-2">
                                <label for="inputZip">MSN<span style="color: #FF0000; font-weight: bold;">*</span></label>
                                <asp:TextBox ID="TXTMSN" runat="server" class="form-control"  MaxLength="4"  onkeypress="return soloLetras(event,'abcdefghijklmnopqrstuvwxyz0123456789-_')" 
                                                placeholder="MSN" disabled></asp:TextBox>
                            </div>
                        <div class="form-group col-md-2">
                                <label for="inputZip">HSN<span style="color: #FF0000; font-weight: bold;">*</span></label>
                                <asp:TextBox ID="TXTHSN" runat="server" class="form-control"  MaxLength="4"  onkeypress="return soloLetras(event,'abcdefghijklmnopqrstuvwxyz0123456789-_')" 
                                                placeholder="HSN" disabled></asp:TextBox>
                        </div>
                      
     
		            </div>
			            <br/>
                       <div class="row">
                        <div class="col-md-12 d-flex justify-content-center">
                                <div class="alert alert-danger" id="banmsg" runat="server" clientidmode="Static"><b>Error!</b></div>
                        </div>
                        </div>

                          
                     
                    </ContentTemplate> 
                    </asp:UpdatePanel>
     
    
  
                 <div class="form-row"> 
        
                       <div class="form-group col-md-1">
                         <label for="inputZip">Pagado:<span style="color: #FF0000; font-weight: bold;"></span></label>
                           <asp:UpdatePanel ID="UPPAGADO" runat="server"  UpdateMode="Conditional" >
                                       <ContentTemplate>
                                        <asp:TextBox ID="TxtPagado" runat="server" class="form-control"    
                                            placeholder=""  Font-Bold="true" style="text-align:center" disabled ></asp:TextBox>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                     </div>

                      <div class="form-group col-md-2">
                         <label for="inputZip">Fact. Hasta:<span style="color: #FF0000; font-weight: bold;"></span></label>
                           <asp:UpdatePanel ID="UPCAS" runat="server"  UpdateMode="Conditional" >
                                       <ContentTemplate>
                                        <asp:TextBox ID="TxtFechaCas" runat="server" class="form-control"    
                                            placeholder=""  Font-Bold="true" style="text-align:center" disabled ></asp:TextBox>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                     </div>
                     <div class="form-group col-md-2">
                         <label for="inputZip">Total Bultos:<span style="color: #FF0000; font-weight: bold;"></span></label>
                           <asp:UpdatePanel ID="UPCONTENEDOR" runat="server"  UpdateMode="Conditional" >
                                       <ContentTemplate>
                                        <asp:TextBox ID="TxtContenedorSeleccionado" runat="server" class="form-control"    
                                            placeholder=""  Font-Bold="true" style="text-align:center" disabled ></asp:TextBox>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                     </div>
                      <div class="form-group col-md-2">
                         <label for="inputZip">Retirados:<span style="color: #FF0000; font-weight: bold;"></span></label>
                           <asp:UpdatePanel ID="UPRETIRADOS" runat="server"  UpdateMode="Conditional" >
                                       <ContentTemplate>
                                        <asp:TextBox ID="TxtRetirados" runat="server" class="form-control"    
                                            placeholder=""  Font-Bold="true" style="text-align:center" disabled ></asp:TextBox>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                     </div>
                      <div class="form-group col-md-1">
                         <label for="inputZip">Saldo:<span style="color: #FF0000; font-weight: bold;"></span></label>
                           <asp:UpdatePanel ID="UPSALDO" runat="server"  UpdateMode="Conditional" >
                                       <ContentTemplate>
                                        <asp:TextBox ID="TxtSaldo" runat="server" class="form-control"    
                                            placeholder=""  Font-Bold="true" style="text-align:center" disabled ></asp:TextBox>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                     </div>
                     <div class="form-group col-md-2">
                         <label for="inputZip">Ubicación:<span style="color: #FF0000; font-weight: bold;"></span></label>
                           <asp:UpdatePanel ID="UPBODEGA" runat="server"  UpdateMode="Conditional" >
                                       <ContentTemplate>
                                        <asp:TextBox ID="TxtBodega" runat="server" class="form-control"    
                                            placeholder=""  Font-Bold="true" style="text-align:center" disabled ></asp:TextBox>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                     </div>
                      <div class="form-group col-md-2">
                         <label for="inputZip">Producto:<span style="color: #FF0000; font-weight: bold;"></span></label>
                           <asp:UpdatePanel ID="UPPRODUCTO" runat="server"  UpdateMode="Conditional" >
                                       <ContentTemplate>
                                        <asp:TextBox ID="TxtTipoProducto" runat="server" class="form-control"    
                                            placeholder=""  Font-Bold="true" style="text-align:center" disabled ></asp:TextBox>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                     </div>
                 </div>

                 <%--  <h4 class="mb">APROBACION DE TURNOS</h4>--%>

           <%--      <div class="form-row"> 
                     
                      <div class="form-group col-md-2">
                                <label for="inputZip">Fecha Salida:<span style="color: #FF0000; font-weight: bold;">*</span></label>         
                                <asp:UpdatePanel ID="UPFECHASALIDA" runat="server"  UpdateMode="Conditional"  >
                                <ContentTemplate>
                            
                                    <asp:TextBox runat="server" ID="TxtFechaHasta"  AutoPostBack="true" MaxLength="10" 
                                            onkeypress="return soloLetras(event,'0123456789-')"    class="form-control"  
                                        ontextchanged="TxtFechaHasta_TextChanged"></asp:TextBox>

                                        <asp:CalendarExtender ID="CAGTFECHAFASA"  runat="server"
                                            CssClass="red" Format="MM/dd/yyyy" TargetControlID="TxtFechaHasta">
                                        </asp:CalendarExtender>
                                      
                                     <asp:TextBox ID="TxtSecuencia" runat="server" class="form-control"   
                                                placeholder=""  Font-Bold="true" style="text-align:center" Visible="false" ></asp:TextBox>

                                </ContentTemplate>
                                   
                                </asp:UpdatePanel> 
                       </div>    
                      <div class="form-group col-md-2">
                                <label for="inputZip">Tipo Horario:<span style="color: #FF0000; font-weight: bold;">*</span></label>                                 
                                <asp:UpdatePanel ID="UPTIPOHORARIO" runat="server"  UpdateMode="Conditional"  >
                                <ContentTemplate>
                                <asp:DropDownList runat="server" ID="CboTipoTurno"   AutoPostBack="true" OnSelectedIndexChanged="CboTipoTurno_SelectedIndexChanged" class="form-control"  ></asp:DropDownList>
                                </ContentTemplate>
                                </asp:UpdatePanel> 
                      </div>
                      <div class="form-group col-md-1">
                                <label for="inputZip">Horarios:<span style="color: #FF0000; font-weight: bold;">*</span></label>                                 
                                <asp:UpdatePanel ID="UPTURNO" runat="server"  UpdateMode="Conditional"  >
                                <ContentTemplate>
                                <asp:DropDownList runat="server" ID="CboTurnos"   AutoPostBack="false"  class="form-control"  ></asp:DropDownList>
                                </ContentTemplate>
                                </asp:UpdatePanel> 
                      </div>
                     <div class="form-group col-md-2">
                             <label for="inputZip">No. Vehículos:<span style="color: #FF0000; font-weight: bold;"></span></label>
                               <asp:UpdatePanel ID="UPVEHICULOS" runat="server"  UpdateMode="Conditional" >
                                           <ContentTemplate>
                                            <asp:TextBox ID="TxtNumeroVehiculos" runat="server" class="form-control"   OnTextChanged="TxtNumeroVehoiculos_TextChanged"   onkeypress="return soloLetras(event,'0123456789')" MaxLength="5"
                                                placeholder=""  Font-Bold="true" style="text-align:center" AutoPostBack="true" ></asp:TextBox>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                         </div>
                      <div class="form-group col-md-2">
                             <label for="inputZip">Cant. Bul. X Veh.:<span style="color: #FF0000; font-weight: bold;"></span></label>
                               <asp:UpdatePanel ID="UPCANTIDADRETIRAR" runat="server"  UpdateMode="Conditional" >
                                           <ContentTemplate>
                                            <asp:TextBox ID="TxtCantidadRetirar" runat="server" class="form-control"  OnTextChanged="TxtCantidadRetirar_TextChanged"   onkeypress="return soloLetras(event,'0123456789')" MaxLength="5"
                                                placeholder=""  Font-Bold="true" style="text-align:center"  AutoPostBack="true"></asp:TextBox>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                         </div>
                         <div class="form-group col-md-1">
                             <label for="inputZip">Tot. Bultos:<span style="color: #FF0000; font-weight: bold;"></span></label>
                               <asp:UpdatePanel ID="UPTOTALBULTOS" runat="server"  UpdateMode="Conditional" >
                                           <ContentTemplate>
                                            <asp:TextBox ID="TxtTotalBultosRetirar" runat="server" class="form-control"    onkeypress="return soloLetras(event,'0123456789')" MaxLength="5"
                                                placeholder=""  Font-Bold="true" style="text-align:center" disabled ></asp:TextBox>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                         </div>
                             <div class="form-group col-md-2">
                                  <label for="inputZip">&nbsp;<span style="color: #FF0000; font-weight: bold;"></span></label>     
                                       <asp:UpdatePanel ID="UPAGREGATURNO" runat="server"  UpdateMode="Conditional" >
                                        <ContentTemplate>
                                                <asp:Button ID="BtnAgregaTruno" runat="server" class="btn btn-primary"   Text="APROBAR"  OnClientClick="return confirm('Esta seguro que desea aprobar el Turno?');" onclick="BtnAgregaTruno_Click"/>      
                                            </ContentTemplate>
                                    </asp:UpdatePanel>
                             </div>
           
                 </div>--%>


             <asp:UpdatePanel ID="UPTEXTO" runat="server"  UpdateMode="Conditional" >
               <ContentTemplate>
                <h4 class="mb" id="text_detalle" runat="server">TURNOS AGREGADOS</h4>
                </ContentTemplate>
               </asp:UpdatePanel>



                 <asp:UpdatePanel ID="UPDETALLE" runat="server"  >  
                   <ContentTemplate>
           
                          <div class="form-row">
                             <div class="form-group col-md-12">
                                   <asp:Repeater ID="tablePagination" runat="server"  onitemcommand="tablePagination_ItemCommand" onitemdatabound="tablePagination_ItemDataBound">
                                   <HeaderTemplate>
                                   <table cellpadding="0" cellspacing="0" border="0" class="table table-bordered invoice" id="hidden-table-info">
                                       <thead>
                                      <tr>
                                                       
                                        <th class="center hidden-phone">FACTURADO <br/> HASTA</th>
                                        <th class="center hidden-phone">FECHA<br/>HORARIO</th>
                                        <th class="center hidden-phone">HORARIO</th>
                                        <th class="center hidden-phone">CIA. TRANSPORTE</th>
                                        <th class="center hidden-phone">VEHICULOS</th>
                                        <th class="center hidden-phone">BULTOS X VEH.</th>
                                        <th class="center hidden-phone">TOTAL BULTOS</th>
                                        <th class="center hidden-phone">PASES</th>
                                        <th class="center hidden-phone">ESTADO</th>
                                        <th >USUARIO</th>
                                        <th >FECHA USUARIO</th>
                                      </tr>
                                    </thead>
                                    <tbody>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <tr class="gradeC"> 
                                            
                                            <td class="center hidden-phone"><asp:Label Text='<%#DataBinder.Eval(Container.DataItem, "FECHA_SALIDA", "{0:yyyy/MM/dd}")%>' ID="LblFechaSalida" runat="server"  /></td>
                                            <td class="center hidden-phone"><asp:Label Text='<%#DataBinder.Eval(Container.DataItem, "FECHA_SALIDA_PASE", "{0:yyyy/MM/dd}")%>' ID="LblFechaturno" runat="server"  /></td>
                                            <td class="center hidden-phone"><asp:Label Text='<%#Eval("D_TURNO")%>' ID="LblTurno" runat="server"  /></td><%--D_TURNO--%>
                                            <td class="center hidden-phone"><asp:Label Text='<%#Eval("CIATRANS")%>' ID="LblEmpresa" runat="server"  /> </td>
                                              <td class="center hidden-phone"><asp:Label Text='<%#Eval("CANTIDAD_VEHICULOS")%>' ID="LblContenedor" runat="server"  /></td>
                                            <td class="center hidden-phone"><asp:Label Text='<%#Eval("CANTIDAD_BULTOS")%>' ID="LblChofer" runat="server"  />  </td> 
                                            <td class="center hidden-phone"><asp:Label Text='<%#Eval("CANTIDAD")%>' ID="LblPlaca" runat="server"  /> </td> 
                                            <td class="center hidden-phone"><asp:Label Text='<%#Eval("NUMERO_PASE_N4")%>' ID="LblPases" runat="server"  /> </td> 
                                           <td class="center hidden-phone">
                                               <asp:Label Text='<%#Eval("ESTADO_PASE")%>' ID="LblEstadoPase" runat="server"  />
                                               <asp:Label Text='<%#Eval("PASE_EXPIRADO")%>' ID="LblPaseExpirado" runat="server"   />
                                           </td> 
                                           <td class="center hidden-phone">  
                                            <asp:Label Text='<%#Eval("USUARIO_MOD")%>' ID="Label4" runat="server"  />
                                            </td> 
                                              <td class="center hidden-phone">  
                                             
                                                <asp:Label Text='<%#Eval("FECHA_MOD")%>' ID="Label5" runat="server"  />
                                                  
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
                            
                           <div class="row">
                                <div class="col-md-12 d-flex justify-content-center">
                                         <div class="alert alert-danger" id="banmsg_det" runat="server" clientidmode="Static"><b>Error!</b>Debe ingresar el número de la carga MRN......</div>
                                </div>
                            </div>

                        </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="BtnBuscar" />
                            </Triggers>
                        </asp:UpdatePanel>

                 

                
             </div>

             <div class="modal-footer">
                   <asp:UpdatePanel ID="UPBOTONES" runat="server" UpdateMode="Conditional" >  
                         <ContentTemplate>
                             
                             <br/>
                            <div class="row">
                                <div class="col-md-12 d-flex justify-content-center">
                                             <img alt="loading.." src="../lib/file-uploader/img/loading.gif" height="32px" width="32px"  id="ImgCargaDet" class="nover"  />
                                 </div>
                            </div>
                              <div class="row">
                                 <div class="col-md-12 d-flex justify-content-center">
                                      
                                  
                                     &nbsp;&nbsp;
                                       
                                       &nbsp;&nbsp;
                                     <button type="button" class="btn btn-outline-primary " data-dismiss="modal">Aceptar</button>
                              </div>
                            </div>
                        </ContentTemplate>
                         </asp:UpdatePanel>   
            </div>
        </div>
    </div>

   

  </div>

</div>






  
 
	    


   <script type="text/javascript" src="../lib/datatables/datatables.min.js"></script>


   <script type="text/javascript" src="../lib/common-scripts.js"></script>
 
   <script type="text/javascript" src="../lib/pages.js" ></script>

   <script type="text/javascript" src="../lib/advanced-form-components.js"></script>

 <script type="text/javascript" src='../lib/autocompletar/bootstrap3-typeahead.min.js'></script>

     <script type="text/javascript" src="../js/buttons/1.6.4/dataTables.buttons.min.js"></script>  
     <script type="text/javascript" src="../js/buttons/1.6.4/jszip.min.js"></script>  
     <script type="text/javascript" src="../js/buttons/1.6.4/pdfmake.min.js"></script>  
    <script type="text/javascript" src="../js/buttons/1.6.4/vfs_fonts.js"></script>  

    <script type="text/javascript" src="../js/buttons/1.6.4/buttons.print.min.js"></script>  
     <script type="text/javascript" src="../js/buttons/1.6.4/buttons.html5.min.js"></script>  
     <script type="text/javascript" src="../js/buttons/1.6.4/buttons.colVis.min.js"></script>  
    
 
  <script type="text/javascript">
            $(document).ready(function () {
                 $('.datetimepicker').datetimepicker().datetimepicker({ lang: 'es', timepicker: false, step: 30, format: 'm/d/Y' });
              });    
      </script>


<script type="text/javascript">

   function confirmacion()
    {
        var mensaje;
        var opcion = confirm("Estimado cliente, está seguro que desea generar la solicitud de turnos Break Bulk. ?");
        if (opcion == true)
        {
            loader();
            return true;
        } else
        {
            //loader();
	         return false;
        }

       
    }

    function mostrarloader(Valor) {

        try {
            if (Valor == "1") {
               
            }
            else {
                document.getElementById("ImgCargaDet").className='ver';
            }
                
            } catch (e) {
                alert('Lamentamos comunicarle que ha ocurrido el siguiente problema:\n' + e.Message);
            }
        }

    function 
        
        (Valor) {
        try {

            if (Valor == "1") {
               
            }
            else {
                document.getElementById("ImgCargaDet").className='nover';
            }

             } catch (e) {
                alert('Lamentamos comunicarle que ha ocurrido el siguiente problema:\n' + e.Message);
            }
        }

   

</script>

 

</asp:Content>