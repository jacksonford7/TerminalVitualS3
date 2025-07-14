<%@ Page Title="" Language="C#" MasterPageFile="~/site.Master" AutoEventWireup="true" CodeBehind="facturacionexportador_otros.aspx.cs" Inherits="CSLSite.facturacionexportador_otros" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="placehead" runat="server">

  <link href="../img/favicon2.png" rel="icon"/>
  <link href="../img/icono.png" rel="apple-touch-icon"/>
 <!-- Bootstrap core CSS -->
  <link href="../css/bootstrap.min.css" rel="stylesheet"/>
  <link href="../css/dashboard.css" rel="stylesheet"/>
  <link href="../css/icons.css" rel="stylesheet"/>
  <link href="../css/style.css" rel="stylesheet"/>

  <!--external css-->
  <link href="../shared/estilo/jquery.datetimepicker.css" rel="stylesheet" type="text/css" />
  <script src="../Scripts/jquery.datetimepicker.js" type="text/javascript"></script>


 <link href="../lib/advanced-datatable/css/demo_page.css" rel="stylesheet" />
  <link href="../lib/advanced-datatable/css/demo_table.css" rel="stylesheet" />
  <link rel="stylesheet" href="../lib/advanced-datatable/css/DT_bootstrap2.css" />



<script type="text/javascript">
$(document).ready(function(){
  $('[data-toggle="tooltip"]').tooltip();   
});
</script>

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

      $('#<%= tablePagination.ClientID %> thead tr').each(function() {
        //this.insertBefore(nCloneTh, this.childNodes[0]);
      });

      $('#<%= tablePagination.ClientID %> tbody tr').each(function() {
        //this.insertBefore(nCloneTd.cloneNode(true), this.childNodes[0]);
      });

      /*
       * Initialse DataTables, with no sorting on the 'details' column
       */
      var oTable = $('#<%= tablePagination.ClientID %>').dataTable({
        "aoColumnDefs": [{
          "bSortable": false,
          "aTargets": [0]
        }],
        "aaSorting": [
          [1, 'asc']
        ]
      });

      /* Add event listener for opening and closing details
       * Note that the indicator for showing which row is open is not controlled by DataTables,
       * rather it is done here
       */
      $('#<%= tablePagination.ClientID %> tbody td img').live('click', function() {
        var nTr = $(this).parents('tr')[0];
        if (oTable.fnIsOpen(nTr)) {
          /* This row is already open - close it */
          this.src = "../lib/advanced-datatable/media/images/details_open.png";
          oTable.fnClose(nTr);
        } else {
          /* Open this row */
          this.src = "../lib/advanced-datatable/images/details_close.png";
          oTable.fnOpen(nTr, fnFormatDetails(oTable, nTr), 'details');
        }
      });
        });
    }
</script>


    

<script type="text/javascript">

    $(document).ready(function ()
    {
            $('#<%= tablePagination.ClientID %>').dataTable();
        });

    $(document).ready(function() {    
    $('#<%=tablePaginationBuscador.ClientID%>').DataTable({        
        language: {
                "lengthMenu": "_MENU_ REGISTROS POR PAGINA",
                "zeroRecords": "No se encontraron resultados",
                "info": "Mostrando registros del _START_ al _END_ de un total de _TOTAL_ registros",
                "infoEmpty": "Mostrando registros del 0 al 0 de un total de 0 registros",
                "infoFiltered": "(filtrado de un total de _MAX_ registros)",
                "sSearch": "Buscar:",
			     "sProcessing":"Procesando...",
            },
        
    });     
});


       
</script>


<script type="text/javascript">

    
    function BindFunctionsBuscar()
    {

        $(document).ready(function () {
            $('#<%=tablePaginationBuscador.ClientID%>').DataTable({
                language: {
                    "lengthMenu": "_MENU_ REGISTROS POR PAGINA",
                    "zeroRecords": "No se encontraron resultados",
                    "info": "Mostrando registros del _START_ al _END_ de un total de _TOTAL_ registros",
                    "infoEmpty": "Mostrando registros del 0 al 0 de un total de 0 registros",
                    "infoFiltered": "(filtrado de un total de _MAX_ registros)",
                    "sSearch": "Buscar:",
                    "sProcessing": "Procesando...",
                },

            });


        });
    
    }


</script>


<script type="text/javascript">

    
    function BindFunctionsRubros()
    {

        $(document).ready(function () {
            $('#<%=tablePaginationRubros.ClientID%>').DataTable({
                language: {
                    "lengthMenu": "_MENU_ REGISTROS POR PAGINA",
                    "zeroRecords": "No se encontraron resultados",
                    "info": "Mostrando registros del _START_ al _END_ de un total de _TOTAL_ registros",
                    "infoEmpty": "Mostrando registros del 0 al 0 de un total de 0 registros",
                    "infoFiltered": "(filtrado de un total de _MAX_ registros)",
                    "sSearch": "Buscar:",
                    "sProcessing": "Procesando...",
                },

            });


        });
    }


</script>


</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="placebody" runat="server" >
    <asp:ToolkitScriptManager ID="tkscata" runat="server"></asp:ToolkitScriptManager>

  <div id="div_BrowserWindowName" style="visibility:hidden;">
    <asp:HiddenField ID="hf_BrowserWindowName" runat="server" />
     
  </div>
    <asp:HiddenField ID="manualHide" runat="server" />
      <input id="ID_FILA" type="hidden" value="" runat="server" clientidmode="Static" />

      <div class="mt-4">         
        <nav class="mt-4" aria-label="breadcrumb">
          <ol class="breadcrumb">
            <li class="breadcrumb-item" id="opcion_principal" runat="server"><a href="#">Bodega BTS</a></li>
            <li class="breadcrumb-item active" aria-current="page" id="sub_opcion" runat="server">FACTURACIÓN POR EXPORTADOR</li>
          </ol>
        </nav>
      </div>
<div class="dashboard-container p-4" id="cuerpo" runat="server">
    <div class="form-title">
         CRITERIO DE BUSQUEDA
     </div>
		
       <asp:UpdatePanel ID="UPCARGA" runat="server"  UpdateMode="Conditional" ChildrenAsTriggers="true">
       <ContentTemplate>
       <div class="form-row"> 
           <div class="form-group col-md-4">
              <label for="inputZip">REFERENCIA<span style="color: #FF0000; font-weight: bold;">*</span></label>
                <asp:TextBox ID="TXTMRN" runat="server" class="form-control" MaxLength="16"  onkeypress="return soloLetras(event,'abcdefghijklmnopqrstuvwxyz0123456789-_')"  
                                 placeholder="NAVE REFERENCIA"></asp:TextBox>
            </div>
             
            <div class="form-group col-md-2">
                 <label for="inputZip">&nbsp;</label>
                   <div class="d-flex">
                        <asp:Button ID="BtnBuscar" runat="server" class="btn btn-primary"   Text="BUSCAR"  OnClientClick="return mostrarloader('1')" OnClick="BtnBuscar_Click" />   
                         <img alt="loading.." src="../lib/file-uploader/img/loading.gif" height="32px" width="32px"  id="ImgCarga" class="nover"   />
                         
                  </div>
            </div>
       </div>
		 <br/>
        <div class="row">
            <div class="col-md-12 d-flex justify-content-center">
                 <div class="alert alert-danger" id="banmsg" runat="server" clientidmode="Static"><b>Error!</b> >Debe ingresar la referencia......</div>
            </div>
         </div>				
                
        </ContentTemplate>
            <Triggers>
            <asp:AsyncPostBackTrigger ControlID="BtnBuscar" />
        </Triggers>
        </asp:UpdatePanel>
                        
     
      
   
    <div class="form-row">  

      
        

        <div class="form-group col-md-12">
     <asp:UpdatePanel ID="UPDETALLE" runat="server" UpdateMode="Conditional" >  
       <ContentTemplate>
         
            
           <h4 id="LabelTotal" runat="server" class="mb">DETALLE EXPORTADORES</h4>
 
             <asp:Repeater ID="tablePagination" runat="server" onitemcommand="tablePagination_ItemCommand" onitemdatabound="tablePagination_ItemDataBound">
                       <HeaderTemplate>
                       <table cellpadding="0" cellspacing="0" border="0" class="table table-bordered invoice" id="tablePagination">
                           <thead>
                          <tr>
                            
                            <th class="center hidden-phone">#</th>
                             <th class="center hidden-phone"># FACTURA</th>
                            <th class="center hidden-phone">RUC</th>
                            <th class="center hidden-phone">EXPORTADOR</th>
                            <th class="center hidden-phone">RUC ASUME</th>
                            <th class="center hidden-phone">EXPORTADOR ASUME</th>
                            <th class="center hidden-phone">LINEA</th>
                            <th class="center hidden-phone">CANTIDAD</th>
                            <th class="center hidden-phone">DETALLE</th>
                            <th class="center hidden-phone">FACTURAR</th>
                            <th class="center hidden-phone">IMPRIMIR</th>
                          </tr>
                        </thead>
                        <tbody>
                        </HeaderTemplate>
                        <ItemTemplate>
                           <tr class="gradeC"> 
                                <td class="center hidden-phone"><asp:Label Text='<%#Eval("Fila")%>' ID="Lblfila" runat="server"  /></td>
                               <td class="center hidden-phone"><asp:Label Text='<%#Eval("numero_factura")%>' ID="LblFactura" runat="server"  /></td>
                                <td class="center hidden-phone"><asp:Label Text='<%#Eval("ruc")%>' ID="LblRuc" runat="server"  /></td>
                                <td class="center hidden-phone"><asp:Label Text='<%#Eval("exportador")%>' ID="LblExportador" runat="server"  /></td>
                                <td class="center hidden-phone">
                                  
                                     <div class="d-flex">
                                         <asp:TextBox ID="TxtRucExportador" runat="server" Text='<%#Eval("ruc_asume")%>' MaxLength="20"  class="form-control"   disabled ></asp:TextBox>  

                                         <asp:UpdatePanel ID="UpdatePanel1" runat="server" ChildrenAsTriggers="true">
                                        <ContentTemplate>
                                           <script type="text/javascript">
                                                  Sys.Application.add_load(BindFunctionsBuscar); 
                                            </script>
                                           <asp:Button ID="BtnBuscador"
                                                CommandArgument= '<%#Eval("Fila")%>' runat="server" Text=".."  class="btn btn-outline-primary" data-toggle="modal" 
                                               data-target="#exampleModalToggle" ToolTip="BUSCAR" CommandName="Ver" />
                                         </ContentTemplate>
                                              <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="BtnDetalle" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                      </div> 

                                </td>
                                <td class="center hidden-phone"><asp:Label Text='<%#Eval("exportador_asume")%>' ID="LblExportadorAsumen" runat="server"  /></td>
                                <td class="center hidden-phone"><asp:Label Text='<%#Eval("linea")%>' ID="LblLinea" runat="server"  /></td>
                                <td class="center hidden-phone"><asp:Label Text='<%#Eval("cantidad")%>' ID="LblCantidad" runat="server"  /></td>
                                <td class="center hidden-phone">  
                                     <asp:Button ID="BtnDetalle" CommandArgument= '<%#Eval("Fila")%>' runat="server" Text="Ver Detalle" 
                                        class="btn btn-primary" data-toggle="modal" data-target="#exampleModalToggleRubros" CommandName="Detalle" />
                                </td> 
                                <td class="center hidden-phone">  
                                <asp:Button ID="BtnFacturarExp" CommandArgument= '<%#Eval("Fila")%>' runat="server" Text="GENERAR FACTURA" class="btn btn-primary" ToolTip="generar la factura" CommandName="Grabar" 
                                  OnClientClick="return confirmacion()" 
                               />   
                                </td> 
                                  <td class="center hidden-phone">  
                                      
                                     <asp:Button ID="BtnImprimir" CommandArgument= '<%#Eval("Fila")%>' runat="server" Text="Imprimir"  OnClientClick="Imprimir(this);"
                                        class="btn btn-primary"  />
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
     </div><!--content-panel-->
     
   
    </div><!--row mb-->
   
    
    <asp:UpdatePanel ID="UPBOTONES" runat="server" UpdateMode="Conditional" >  
             <ContentTemplate>

               <div class="row">
                <div class="col-md-12 d-flex justify-content-center">
                         <div class="alert alert-danger" id="banmsg_det" runat="server" clientidmode="Static"><b>Error!</b>Debe ingresar el número de la carga MRN......</div>
                </div>
              </div>
              <br/>

             <div class="row">
                <div class="col-md-12 d-flex justify-content-center">
                      <div class="alert alert-danger" id="banmsg_Pase" runat="server" clientidmode="Static"><b>Error!</b>.</div>
                 </div>
             </div>
             <br/>

             <div class="row">
                <div class="col-md-12 d-flex justify-content-center">
                             <img alt="loading.." src="../lib/file-uploader/img/loading.gif" height="32px" width="32px"  id="ImgCargaDet" class="nover"  />
                 </div>
            </div>    
            <br/>


             <div class="row">
             <div class="col-md-12 d-flex justify-content-center">
                             
                    <asp:Button ID="BtnNuevo" runat="server" class="btn btn-outline-primary mr-4"  Text="NUEVA TRANSACCION"  OnClick="BtnNuevo_Click"  />

                   <asp:Button ID="BtnCotizar" runat="server" class="btn btn-outline-primary mr-4" Text="GENERAR PROFORMA" OnClientClick="return mostrarloader('2')" OnClick="BtnCotizar_Click" Visible="false"/>

                    
                   <asp:Button ID="BtnFacturar" runat="server" class="btn btn-primary" Text="GENERAR FACTURAS"  OnClientClick="return confirmacion();"  OnClick="BtnFacturar_Click" />
               </div> 
             </div>
            </ContentTemplate>
             </asp:UpdatePanel>   
   

       
    
</div>

 
 <!--exportadores-->
 <div class="modal fade" id="exampleModalToggle" tabindex="-1" role="dialog" aria-hidden="true">
    <div class="modal-dialog" role="document" style="max-width: 800px">
         <div class="modal-content">
              <div class="dashboard-container p-4" id="Div1" runat="server">  
                   <div class="modal-header">
                       <asp:UpdatePanel ID="UPTITULO" runat="server"  UpdateMode="Conditional" ChildrenAsTriggers="true">
                       <ContentTemplate>
                                    <h5 class="modal-title" id="Titulo" runat="server">Buscar Exportador</h5>
                           </ContentTemplate>
                        </asp:UpdatePanel>
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                    <span aria-hidden="true">&times;</span>
                        </button>
                    </div>
                   <div class="modal-body">
                        <asp:UpdatePanel ID="UPBUSCADOR" runat="server" UpdateMode="Conditional" > 
                        <ContentTemplate>
                              <script type="text/javascript">
                                  
                                   Sys.Application.add_load(BindFunctionsBuscar); 
                            </script>
                             <div class="form-row">
                                  <div class="form-group col-md-12">
                                       <label for="inputEmail4">Novedad:</label>
                                       <div class="d-flex">
                                            <asp:TextBox ID="txtfinder" 
                                             runat="server"  
                                            class="form-control"
                                             onkeypress="return soloLetras(event,'abcdefghijklmnñopqrstuvwxyz1234567890-*.')" 
                                             MaxLength="30" 
                                             onkeyup="msgfinder(this,'valintro');"
                                              ></asp:TextBox>  
                                            <asp:LinkButton runat="server" ID="find" Text="<span class='fa fa-search' style='font-size:24px'></span>"  class="btn btn-outline-primary mr-4"   
                                                  onclick="find_Click" OnClientClick="return initFinder();"/>
                                            <asp:TextBox ID="TxtFila" 
                                             runat="server"  
                                            class="form-control"
                                             onkeypress="return soloLetras(event,'abcdefghijklmnñopqrstuvwxyz1234567890-*.')" 
                                             MaxLength="30" 
                                             Visible="False"
                                              ></asp:TextBox>  
                                            <span id="imagen"></span>
                                    </div>  
                                  </div>
                                 

                                 <div class="form-group col-md-12">
                                     <asp:Repeater ID="tablePaginationBuscador" runat="server" onitemcommand="tablePaginationBuscador_ItemCommand" >
                                   <HeaderTemplate>
                                   <table cellpadding="0" cellspacing="0" border="0" class="table table-bordered invoice" id="tablePaginationBuscador">
                                       <thead>
                                      <tr>
                                        <th class="center hidden-phone">ID</th>                          
                                        <th class="center hidden-phone">RUC</th>
                                        <th class="center hidden-phone">EXPORTADOR</th>
                                        <th class="center hidden-phone">LINEA</th>
                                        <th >SELECCIONAR</th>
                                      </tr>
                                    </thead>
                                    <tbody>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <tr class="gradeC"> 
                                            <td class="center hidden-phone"><asp:Label Text='<%#Eval("id")%>' ID="LblId" runat="server"  /></td>
                                            <td class="center hidden-phone"><asp:Label Text='<%#Eval("ruc")%>' ID="LblRucExp" runat="server"  /> </td>
                                            <td class="center hidden-phone"><asp:Label Text='<%#Eval("nombre")%>' ID="LblNombreExp" runat="server"  /></td>
                                           <td class="center hidden-phone"><asp:Label Text='<%#Eval("linea")%>' ID="LblLineaExp" runat="server"  /></td>
                                             <td> 
                                                 <asp:Button ID="BtnModificar"
                                                CommandArgument= '<%#Eval("id")%>' runat="server" Text="SELECCIONAR" 
                                                      OnClientClick="CierraPopup();"
                                                     class="btn btn-primary" ToolTip="SELECCIONAR EXPORTADOR" CommandName="Ver"  />
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
                                         <div class="alert alert-danger" id="banmsg_buscador" runat="server" clientidmode="Static"><b>Error!</b></div>
                                </div>
                            </div>

                         </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                   <div class="modal-footer">
                   <asp:UpdatePanel ID="UPBOTONESBUSCADOR" runat="server" UpdateMode="Conditional" >  
                         <ContentTemplate>
                             
                             <br/>
                           
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


<!--detalle de rubros-->
 <div class="modal fade" id="exampleModalToggleRubros" tabindex="-1" role="dialog" aria-hidden="true">
    <div class="modal-dialog" role="document" style="max-width: 1000px">
         <div class="modal-content">
              <div class="dashboard-container p-4" id="Div2" runat="server">  
                   <div class="modal-header">
                       <asp:UpdatePanel ID="UPTITULO_RUBROS" runat="server"  UpdateMode="Conditional" ChildrenAsTriggers="true">
                       <ContentTemplate>
                                    <h5 class="modal-title" id="H1" runat="server">DETALLE DE RUBROS</h5>
                           </ContentTemplate>
                        </asp:UpdatePanel>
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span> </button>
                    </div>
                   <div class="modal-body">
                        <asp:UpdatePanel ID="UPDETALLE_RUBROS" runat="server" UpdateMode="Conditional" > 
                        <ContentTemplate>
                               <script type="text/javascript">
                                   Sys.Application.add_load(BindFunctionsRubros); 
                            </script>
                             <div class="form-row">
                               
                                 <div class="form-group col-md-12">
                                     <asp:Repeater ID="tablePaginationRubros" runat="server"  >
                                   <HeaderTemplate>
                                   <table cellpadding="0" cellspacing="0" border="0" class="table table-bordered invoice" id="tablePaginationRubros">
                                       <thead>
                                      <tr>
                                        <th class="center hidden-phone">#</th>                          
                                        <th class="center hidden-phone">RUC</th>
                                        <th class="center hidden-phone">EXPORTADOR</th>
                                        <th class="center hidden-phone">CANTIDAD</th>
                                        <th class="center hidden-phone">CODIGO</th>
                                        <th class="center hidden-phone">SERVICIO</th>
                                        <th class="center hidden-phone">TARIFA</th>
                                      </tr>
                                    </thead>
                                    <tbody>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <tr class="gradeC"> 
                                            <td class="center hidden-phone"><asp:Label Text='<%#Eval("Fila")%>' ID="Label1" runat="server"  /></td>
                                            <td class="center hidden-phone"><asp:Label Text='<%#Eval("ruc")%>' ID="Label2" runat="server"  /> </td>
                                            <td class="center hidden-phone"><asp:Label Text='<%#Eval("exportador")%>' ID="Label3" runat="server"  /></td>
                                            <td class="center hidden-phone"><asp:Label Text='<%#Eval("cantidad")%>' ID="Label4" runat="server"  /></td>
                                            <td class="center hidden-phone"><asp:Label Text='<%#Eval("codigoN4")%>' ID="Label5" runat="server"  /></td>
                                            <td class="center hidden-phone"><asp:Label Text='<%#Eval("nombre")%>' ID="Label6" runat="server"  /></td>
                                            <td class="center hidden-phone"><asp:Label Text='<%#Eval("valor")%>' ID="Label7" runat="server"  /></td>
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
                                         <div class="alert alert-danger" id="banmsg_rubros" runat="server" clientidmode="Static"><b>Error!</b></div>
                                </div>
                            </div>

                         </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                   <div class="modal-footer">
                   <asp:UpdatePanel ID="UPBOTONES_RUBROS" runat="server" UpdateMode="Conditional" >  
                         <ContentTemplate>
                             
                             <br/>
                           
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


   <script type="text/javascript" src="../lib/common-scripts.js"></script>
 
   <script type="text/javascript" src="../lib/pages.js" ></script>

   <script type="text/javascript" src="../lib/advanced-form-components.js"></script>

 

    <script type="text/javascript">

    function confirmacion()
    {
        var mensaje;
        var opcion = confirm("Esta seguro que desea generar la factura ?");
        if (opcion == true)
        {
            document.getElementById("ImgCargaDet").className = 'ver';
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
                document.getElementById("ImgCarga").className = 'ver';
            }
            else {
                document.getElementById("ImgCargaDet").className='ver';
            }
                
            } catch (e) {
                alert('Lamentamos comunicarle que ha ocurrido el siguiente problema:\n' + e.Message);
            }
        }


    function ocultarloader(Valor) {
        try {

            if (Valor == "1") {
                document.getElementById("ImgCarga").className = 'nover';
            }
            else {
                document.getElementById("ImgCargaDet").className='nover';
            }

             } catch (e) {
                alert('Lamentamos comunicarle que ha ocurrido el siguiente problema:\n' + e.Message);
            }
        }

     function initFinder() {
          if (document.getElementById('txtfinder').value.trim().length <= 0) {
               alertify.alert('Advertencia','Escriba una o varias letras para iniciar la búsqueda').set('label', 'Aceptar');
              return false;
          }
          document.getElementById('imagen').innerHTML = '<img alt="" src="../shared/imgs/loader.gif">';
     }

    function CierraPopup()
    {
        $("#exampleModalToggle").modal('hide');//ocultamos el modal
        $('body').removeClass('modal-open');//eliminamos la clase del body para poder hacer scroll
        $('.modal-backdrop').remove();//eliminamos el backdrop del modal
     }


      
  </script>
       <script type="text/javascript">

           function Imprimir(btn)
           {
               var row = btn.closest("tr"); // Obtener la fila padre del botón
               var factura = row.cells[1].textContent;
               var link = "../btsagencia/facturacionexportador_otros_print.aspx?id_comprobante=" + factura;

               //alert(link);

               window.open(link, 'name', 'width=850,height=480');
                
              
             }


           $(document).ready(function ()
           {
               $('.datetimepicker').datetimepicker().datetimepicker({ lang: 'es', timepicker: false, step: 30, format: 'm/d/Y' });

            

              });    
      </script>

     
</asp:Content>