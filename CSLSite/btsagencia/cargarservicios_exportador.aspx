<%@ Page Title="" Language="C#" MasterPageFile="~/site.Master" AutoEventWireup="true" CodeBehind="cargarservicios_exportador.aspx.cs" Inherits="CSLSite.cargarservicios_exportador" %>
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



        <%--<style type="text/css">
        body
        {
            /*font-family: Arial;
            font-size: 10pt;*/
        }
        .modal
        {
            position: fixed;
            z-index: 999;
            height: 100%;
            width: 100%;
            top: 0;
            background-color: Black;
            filter: alpha(opacity=60);
            opacity: 0.6;
            -moz-opacity: 0.8;
        }

        .modalBackground
        {
            background-color: Black;
            filter: alpha(opacity=60);
            opacity: 0.6;
        }
        .modalPopup
        {
            background-color: #FFFFFF;
            width: 726px;
            border: 3px solid #FF3720;
            padding: 0;
        }
        .modalPopup .header
        {
            background-color: #2FBDF1;
            height: 30px;
            color: White;
            line-height: 30px;
            text-align: center;
            font-weight: bold;
        }
        .modalPopup .body
        {
            min-height: 50px;
            line-height: 25px;
            text-align: center;
            /*font-weight: bold;*/
            margin-bottom: 5px;
        }
    </style>
 --%>
    
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
      
      });

      $('#<%= tablePagination.ClientID %> tbody tr').each(function() {
      });

  
      var oTable = $('#<%= tablePagination.ClientID %>').dataTable({
        "aoColumnDefs": [{
          "bSortable": false,
          "aTargets": [0]
        }],
        "aaSorting": [
          [1, 'asc']
        ]
      });

  
      $('#<%= tablePagination.ClientID %> tbody td img').live('click', function() {
        var nTr = $(this).parents('tr')[0];
        if (oTable.fnIsOpen(nTr)) {
          this.src = "../lib/advanced-datatable/media/images/details_open.png";
          oTable.fnClose(nTr);
        } else {
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


    $(document).ready(function ()
    {
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


     $(document).ready(function ()
    {
        $('#<%=tablePaginationReferencias.ClientID%>').DataTable({        
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

    
    function BindFunctionsBuscar_Referencia()
    {

        $(document).ready(function () {
            $('#<%=tablePaginationReferencias.ClientID%>').DataTable({
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
            <li class="breadcrumb-item active" aria-current="page" id="sub_opcion" runat="server">AGREGAR OTROS SERVICIOS - EXPORTADOR</li>
          </ol>
        </nav>
      </div>
<div class="dashboard-container p-4" id="cuerpo" runat="server">
    <div class="form-title">
         CRITERIO DE BUSQUEDA
     </div>
		
       <asp:UpdatePanel ID="UPCARGA" runat="server"  UpdateMode="Conditional" >
       <ContentTemplate>
       <div class="form-row"> 
           <div class="form-group col-md-4">
              <label for="inputZip">REFERENCIA<span style="color: #FF0000; font-weight: bold;">*</span></label>
               
                <div class="d-flex">
                     <asp:TextBox ID="TXTMRN" runat="server" class="form-control" MaxLength="16"  onkeypress="return soloLetras(event,'abcdefghijklmnopqrstuvwxyz0123456789-_')"  
                                 placeholder="NAVE REFERENCIA" disabled></asp:TextBox>
						 &nbsp;
                      <asp:LinkButton runat="server" ID="BtnBuscarRef" Text="<span class='fa fa-search' style='font-size:24px'></span>" data-toggle="modal" 
                            data-target="#exampleModalToggleRef" class="btn btn-primary"   />
                </div>

            </div>
             
           <div class="form-group col-md-12"> 
                 <label for="inputAddress">EVENTO:<span style="color: #FF0000; font-weight: bold;">*</span></label>
  
                    <asp:DropDownList runat="server" ID="CboEvento"    AutoPostBack="false"  class="form-control"  >
                        </asp:DropDownList>
             </div> 

           <div class="form-group col-md-4">
              <label for="inputZip">EXPORTADOR<span style="color: #FF0000; font-weight: bold;">*</span></label>
               
                <div class="d-flex">
                     <asp:TextBox ID="TxtIdExportador" runat="server" class="form-control" MaxLength="16"  onkeypress="return soloLetras(event,'abcdefghijklmnopqrstuvwxyz0123456789-_')"  
                                 placeholder="EXPORTADOR" disabled></asp:TextBox>
						 &nbsp;
                      <asp:LinkButton runat="server" ID="BtnBuscarExportador" Text="<span class='fa fa-search' style='font-size:24px'></span>" data-toggle="modal" 
                            data-target="#exampleModalToggle" class="btn btn-primary"   />
                   
                </div>
               
            </div>
           <div class="form-group col-md-8">
                <label for="inputZip"> &nbsp;<span style="color: #FF0000; font-weight: bold;"></span></label>
                <asp:TextBox ID="TxtDescExportador" runat="server" class="form-control"  
                                 placeholder="EXPORTADOR" disabled></asp:TextBox>
           </div>
            <div class="form-group col-md-12"> 
              <label for="inputAddress">CAJAS:<span style="color: #FF0000; font-weight: bold;">*</span></label>  
                <asp:TextBox ID="TxtCantidad" runat="server" class="form-control"    
                                placeholder=""  MaxLength="10" Font-Bold="false" onkeypress="return soloLetras(event,'0123456789')"
                                    ClientIDMode="Static"></asp:TextBox>
           </div>
            <div class="form-group col-md-12"> 
              <label for="inputAddress">COMENTARIO:<span style="color: #FF0000; font-weight: bold;"></span></label> 
                <asp:TextBox ID="Txtcomentario" runat="server" class="form-control"    
                                placeholder=""  MaxLength="250" Font-Bold="false" onkeypress="return soloLetras(event,'abcdefghijklmnñopqrstuvwxyz0123456789_')"
                                    ClientIDMode="Static"></asp:TextBox>
            </div> 

            <div class="form-group col-md-2">
                 <label for="inputZip">&nbsp;</label>
                   <div class="d-flex">
                         <img alt="loading.." src="../lib/file-uploader/img/loading.gif" height="32px" width="32px"  id="ImgCarga" class="nover"   />
                         
                  </div>
            </div>
       </div>
        <div class="row">
            <div class="col-md-12 d-flex justify-content-center">
                 <div class="alert alert-danger" id="banmsg" runat="server" clientidmode="Static"><b>Error!</b> >Debe ingresar la referencia......</div>
            </div>
         </div>				
                
        </ContentTemplate>
          
        </asp:UpdatePanel>
                        
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
                             <img alt="loading.." src="../lib/file-uploader/img/loading.gif" height="32px" width="32px"  id="ImgCargaDet" class="nover"  />
                 </div>
            </div>    
          

             <div class="row">
             <div class="col-md-12 d-flex justify-content-center">
                             
                    <asp:Button ID="BtnNuevo" runat="server" class="btn btn-outline-primary mr-4"  Text="NUEVA TRANSACCION"  OnClick="BtnNuevo_Click"  />
                    
                   <asp:Button ID="BtnGrabar" runat="server" class="btn btn-primary" Text="REGISTRAR SERVICIOS"  OnClientClick="return confirmacion();"  OnClick="BtnGrabar_Click" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;

                   <asp:Button ID="BtnExportar" runat="server" class="btn btn-outline-primary mr-4" Text="EXPORTAR A EXCEL" OnClientClick="exportar();"  />

               </div> 
             </div>
            </ContentTemplate>
             </asp:UpdatePanel>   
      
    <!--detalle de rubros de la referencia seleccionada-->
    <div class="form-row">  

        <div class="form-group col-md-12">
          <asp:UpdatePanel ID="UPDETALLE" runat="server" UpdateMode="Conditional" >  
       <ContentTemplate>
         
            
           <h4 id="LabelTotal" runat="server" class="mb">DETALLE DE EVENTOS EXPORTADORES</h4>
 
             <asp:Repeater ID="tablePagination" runat="server" onitemcommand="tablePagination_ItemCommand" onitemdatabound="tablePagination_ItemDataBound">
                       <HeaderTemplate>
                       <table cellpadding="0" cellspacing="0" border="0" class="table table-bordered invoice" id="tablePagination">
                           <thead>
                          <tr>
                            
                            <th class="center hidden-phone">#</th>
                             <th class="center hidden-phone"># FACTURA</th>
                            <th class="center hidden-phone">RUC</th>
                            <th class="center hidden-phone">EXPORTADOR</th>
                            <th class="center hidden-phone">RUBRO</th>
                            <th class="center hidden-phone">VALOR</th>
                            <th class="center hidden-phone">LINEA</th>
                            <th class="center hidden-phone">CAJAS</th>
                            <th class="center hidden-phone">F/REGISTRO</th>
                            <th class="center hidden-phone">USUARIO</th>
                            <th class="center hidden-phone">QUITAR</th>
                          </tr>
                        </thead>
                        <tbody>
                        </HeaderTemplate>
                        <ItemTemplate>
                           <tr class="gradeC"> 
                                <td class="center hidden-phone"><asp:Label Text='<%#Eval("Fila")%>' ID="Lblfila" runat="server"  /></td>
                               <td class="center hidden-phone"><asp:Label Text='<%#Eval("numero_factura")%>' ID="LblFactura" runat="server"  /></td>
                                <td class="center hidden-phone"><asp:Label Text='<%#Eval("ruc")%>' ID="LblRuc" runat="server"  /></td>
                                <td class="center hidden-phone"><asp:Label Text='<%#Eval("desc_exportador")%>' ID="LblExportador" runat="server"  /></td>
                                <td class="center hidden-phone"><asp:Label Text='<%#Eval("tarifas")%>' ID="LblTarifa" runat="server"  /> </td>
                                <td class="center hidden-phone"><asp:Label Text='<%#Eval("valor")%>' ID="LblValor" runat="server"  /></td>
                                <td class="center hidden-phone"><asp:Label Text='<%#Eval("linea")%>' ID="LblLinea" runat="server"  /></td>
                                <td class="center hidden-phone"><asp:Label Text='<%#Eval("cajas")%>' ID="LblCantidad" runat="server"  /></td>
                               <td class="center hidden-phone"><asp:Label Text='<%#Eval("fecha")%>' ID="LblFecha" runat="server"  /></td>
                               <td class="center hidden-phone"><asp:Label Text='<%#Eval("usuario_reg")%>' ID="LblUsuario" runat="server"  /></td>

                              
                             
                                  <td class="center hidden-phone">  
                                      
                                     <asp:Button ID="BtnQuitar" CommandArgument= '<%#Eval("id")%>' runat="server" Text="Quitar"  class="btn btn-primary" CommandName="Delete" OnClientClick="quitar();"
                                         />
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




 <!--buscar referencias-->
 <div class="modal fade" id="exampleModalToggleRef" tabindex="-1" role="dialog" aria-hidden="true">
    <div class="modal-dialog" role="document" style="max-width: 800px">
         <div class="modal-content">
              <div class="dashboard-container p-4" id="Div3" runat="server">  
                   <div class="modal-header">
                       <asp:UpdatePanel ID="UPTIT_REFERENCIA" runat="server"  UpdateMode="Conditional" ChildrenAsTriggers="true">
                       <ContentTemplate>
                                    <h5 class="modal-title" id="H2" runat="server">Buscar Referencias</h5>
                           </ContentTemplate>
                        </asp:UpdatePanel>
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                    <span aria-hidden="true">&times;</span>
                        </button>
                    </div>
                   <div class="modal-body">
                        <asp:UpdatePanel ID="UPDET_REFERENCIA" runat="server" UpdateMode="Conditional" > 
                        <ContentTemplate>
                              <script type="text/javascript">
                                  
                                   Sys.Application.add_load(BindFunctionsBuscar_Referencia); 
                            </script>
                             <div class="form-row">
                                  <div class="form-group col-md-12">
                                       <label for="inputEmail4">Referencia:</label>
                                       <div class="d-flex">
                                            <asp:TextBox ID="TxtFiltraReferencia" 
                                             runat="server"  
                                            class="form-control"
                                             onkeypress="return soloLetras(event,'abcdefghijklmnñopqrstuvwxyz1234567890-*.')" 
                                             MaxLength="30" 
                                             onkeyup="msgfinder(this,'valintro');"
                                              ></asp:TextBox>  
                                            <asp:LinkButton runat="server" ID="BtnFiltrarRef" Text="<span class='fa fa-search' style='font-size:24px'></span>"  class="btn btn-outline-primary mr-4"   
                                                  onclick="BtnFiltrarRef_Click" OnClientClick="return validaReferencia();"/>
                                           
                                           <asp:TextBox ID="TxtFiltraReferencia2" 
                                             runat="server"  
                                            class="form-control"
                                             onkeypress="return soloLetras(event,'abcdefghijklmnñopqrstuvwxyz1234567890-*.')" 
                                             MaxLength="30" 
                                             Visible="False"
                                              ></asp:TextBox>  

                                            <span id="imagen2"></span>
                                    </div>  
                                  </div>
                                 

                                 <div class="form-group col-md-12">
                                     <asp:Repeater ID="tablePaginationReferencias" runat="server" onitemcommand="tablePaginationReferencias_ItemCommand" >
                                   <HeaderTemplate>
                                   <table cellpadding="0" cellspacing="0" border="0" class="table table-bordered invoice" id="tablePaginationReferencias">
                                       <thead>
                                      <tr>
                                        <th class="center hidden-phone">REFERENCIA</th>                          
                                      
                                        <th >SELECCIONAR</th>
                                      </tr>
                                    </thead>
                                    <tbody>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <tr class="gradeC"> 
                                            <td class="center hidden-phone"><asp:Label Text='<%#Eval("idNave")%>' ID="LblReferencia" runat="server"  /></td>
                                           
                                             <td> 
                                                 <asp:Button ID="BtnModificar"
                                                CommandArgument= '<%#Eval("idNave")%>' runat="server" Text="SELECCIONAR" 
                                                      OnClientClick="CierraPopupReferencia();"
                                                     class="btn btn-primary" ToolTip="SELECCIONAR REFERENCIA" CommandName="Ver"  />
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
                                         <div class="alert alert-danger" id="banmsg_buscador_referencia" runat="server" clientidmode="Static"><b>Error!</b></div>
                                </div>
                            </div>

                         </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                   <div class="modal-footer">
                   <asp:UpdatePanel ID="UPPIE_REFERENCIA" runat="server" UpdateMode="Conditional" >  
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
        var opcion = confirm("Esta seguro que desea registrar el evento ?");
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


      function quitar()
    {
        var mensaje;
        var opcion = confirm("Esta seguro que desea eliminar el registrar del evento ?");
        if (opcion == true)
        {
            return true;
        } else
        {
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


        function validaReferencia()
        {
          if (document.getElementById('TxtFiltraReferencia').value.trim().length <= 0) {
               alertify.alert('Advertencia','Escriba una o varias letras de la referencia para iniciar la búsqueda').set('label', 'Aceptar');
              return false;
          }
          document.getElementById('imagen2').innerHTML = '<img alt="" src="../shared/imgs/loader.gif">';
     }

        function CierraPopupReferencia()
    {
        $("#exampleModalToggleRef").modal('hide');//ocultamos el modal
        $('body').removeClass('modal-open');//eliminamos la clase del body para poder hacer scroll
        $('.modal-backdrop').remove();//eliminamos el backdrop del modal
        }

  </script>


<script type="text/javascript">

           function Imprimir(btn)
           {
               var row = btn.closest("tr"); // Obtener la fila padre del botón
               var factura = row.cells[1].textContent;
               var link = "../btsagencia/facturacionexportador_print.aspx?id_comprobante=" + factura;

               //alert(link);

               window.open(link, 'name', 'width=850,height=480');
                
              
             }


           $(document).ready(function ()
           {
               $('.datetimepicker').datetimepicker().datetimepicker({ lang: 'es', timepicker: false, step: 30, format: 'm/d/Y' });

            

              });    
</script>

 <script type="text/javascript">

        function exportar()
        {
            var Referencia = document.getElementById('<%= TXTMRN.ClientID %>').value;
           

            var xhr = new XMLHttpRequest();
            xhr.open('POST', 'cargarservicios_exportador.aspx/ExportarExcel', true);
            xhr.setRequestHeader('Content-Type', 'application/json; charset=utf-8');
            xhr.responseType = 'blob';

            xhr.onload = function ()
            {
                var fileName = "Reporte_Eventos.xlsx";
                if (xhr.status === 200) {
                    var blob = xhr.response;

                  
                    var link = document.createElement('a');
                    link.href = window.URL.createObjectURL(blob);
                    link.download = fileName;
                    link.style.display = 'none';
                    document.body.appendChild(link);
                    link.click();
                    document.body.removeChild(link);
                } else {
                    console.log('Error al exportar el archivo Excel. Código de estado: ' + xhr.status);
                }
            };

            xhr.onerror = function () {
                console.log('Error al enviar la solicitud de exportación del archivo Excel.');
            };

            xhr.send(JSON.stringify({ pReferencia: Referencia }));
        }
</script>
    

</asp:Content>