<%@ Page Title="" Language="C#" MasterPageFile="~/site.Master" AutoEventWireup="true" CodeBehind="p2d_listadofactura_freightforwarder.aspx.cs" Inherits="CSLSite.p2d_listadofactura_freightforwarder" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="placehead" runat="server">

 <link href="../img/favicon2.png" rel="icon"/>
  <link href="../img/icono.png" rel="apple-touch-icon"/>
 <!-- Bootstrap core CSS -->
  <link href="../css/bootstrap.min.css" rel="stylesheet"/>
  <link href="../css/dashboard.css" rel="stylesheet"/>
  <link href="../css/icons.css" rel="stylesheet"/>
  <link href="../css/style.css" rel="stylesheet"/>

 
  <link href="../shared/estilo/jquery.datetimepicker.css" rel="stylesheet" type="text/css" />
  <script src="../Scripts/jquery.datetimepicker.js" type="text/javascript"></script>



   <link href="https://fonts.googleapis.com/css2?family=Roboto:wght@300;400;500;700&display=swap" rel="stylesheet"/>
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/5.15.1/css/all.min.css" integrity="sha512-+4zCK9k+qNFUR5X+cKL9EIR+ZOhtIloNl9GIKS57V1MyNsYpYcUrUeQc9vNfzsWfV28IaLL3i96P9sdNyeRssA==" crossorigin="anonymous" />


  <link href="../css/datatables.min.css" rel="stylesheet" />
  <link rel="stylesheet" type="text/css" href="..js/buttons/css1.6.4/buttons.dataTables.min.css"/>



<script type="text/javascript">



 function BindFunctions()
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

      <div class="mt-4">         
        <nav class="mt-4" aria-label="breadcrumb">
          <ol class="breadcrumb">
            <li class="breadcrumb-item" id="opcion_principal" runat="server"><a href="#">P2D</a></li>
            <li class="breadcrumb-item active" aria-current="page" id="sub_opcion" runat="server">LISTADO DE FACTURAS CARGA SUELTA (FREIGHT FORWARDER)</li>
          </ol>
        </nav>
      </div>
<div class="dashboard-container p-4" id="cuerpo" runat="server">
     <div class="form-title">
             CRITERIOS DE BÚSQUEDAS
     </div>

     <div class="form-row"> 
        <div class="form-group col-md-6"> 
                 <label for="inputEmail4">FECHA DESDE:</label>
                <asp:TextBox ID="TxtFechaDesde" runat="server"  class="datetimepicker form-control"  MaxLength="10"  placeholder="dd/MM/yyyy" onkeypress="return soloLetras(event,'1234567890/:')"></asp:TextBox>
        </div>
        <div class="form-group col-md-6">  
                <label for="inputEmail4">FECHA HASTA:</label>
                <asp:TextBox ID="TxtFechaHasta" runat="server"  class="datetimepicker form-control" MaxLength="10"  placeholder="dd/MM/yyyy" onkeypress="return soloLetras(event,'1234567890/:')"></asp:TextBox>
                          
        </div>
     </div> 
    <asp:UpdatePanel ID="UPCARGA" runat="server"  UpdateMode="Conditional" ChildrenAsTriggers="true">
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
              <div class="alert alert-warning" id="banmsg" runat="server" clientidmode="Static"><b>Error!</b> >......</div>
        </div>
      </div>

    </ContentTemplate>
    <Triggers>
    <asp:AsyncPostBackTrigger ControlID="BtnBuscar" />
</Triggers>
</asp:UpdatePanel>
         
     
			
       
     <asp:UpdatePanel ID="UPDETALLE" runat="server" UpdateMode="Conditional" >  
       <ContentTemplate>
             <script type="text/javascript">
                            Sys.Application.add_load(BindFunctions); 
            </script>
          
                 <div class="form-title">
                    DETALLE DE FACTURAS
                </div>
            
              <div class="bokindetalle" style=" width:100%; overflow:auto">  
                      <asp:Repeater ID="grilla" runat="server"  >
                       <HeaderTemplate>
                       <table cellpadding="0" cellspacing="0" border="0" class="table table-bordered table-sm table-contecon" id="grilla" width="100%">
                           <thead>
                          <tr>
                            <th class="th-sm">FECHA</th>
                            <th class="th-sm">FACTURA</th>
                            <th class="th-sm">TIPO CARGA</th>
                            <th class="th-sm">AGENTE</th>
                            <th class="th-sm">CLIENTE</th>
                            <th class="th-sm">FACTURADO</th>
                            <th class="th-sm">CARGA</th>
                            <th class="th-sm">SUBTOTAL</th>
                            <th class="th-sm">IVA</th>
                            <th class="th-sm">TOTAL</th>
                            <th class="th-sm">USUARIO</th>
                            <th class="th-sm">PRINT</th>
                          </tr>
                        </thead>
                        <tbody>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr class="gradeC">
                                <td class="center hidden-phone"> <%#Eval("IV_FECHA")%></td>
                                <td class="center hidden-phone"> <%#Eval("IV_COMPROBANTE")%></td>
                                <td class="center hidden-phone"> <%#Eval("IV_TIPO_CARGA")%></td>
                                <td class="center hidden-phone"> <%#Eval("IV_DESC_AGENTE")%></td>
                                <td class="center hidden-phone"> <%#Eval("IV_DESC_CLIENTE")%></td> 
                                <td class="center hidden-phone"> <%#Eval("IV_DESC_FACTURADO")%></td> 
                                <td class="center hidden-phone"> <%#Eval("IV_NUMERO_CARGA")%></td>
                                <td class="center hidden-phone"> <%#DataBinder.Eval(Container.DataItem, "TOT_IV_SUBTOTAL", "{0:c}")%></td> 
                                <td class="center hidden-phone"> <%#DataBinder.Eval(Container.DataItem, "TOT_IV_IVA", "{0:c}")%></td> 
                                <td class="center hidden-phone"> <%#DataBinder.Eval(Container.DataItem, "TOT_IV_TOTAL", "{0:c}")%></td> 
                                <td class="center hidden-phone"> <%#Eval("IV_USUARIO_CREA")%></td>
                                <td class="center hidden-phone">
                                    <a href="../p2d/p2d_facturafreightforwarder_preview.aspx?id_comprobante=<%#securetext(Eval("IV_ID")) %>"  target="_blank"><button type="button" class="btn btn-primary"><i class="fa fa-print"></i> </button></a>
                               
                                </td> 
                             </tr>    
                       </ItemTemplate>
                       <FooterTemplate>
                        </tbody>
                      </table>
                     </FooterTemplate>
                    </asp:Repeater>
                  </div>

              
         
     
           </ContentTemplate>
     </asp:UpdatePanel>   

      
	


   </div>

 <%--<script type="text/javascript" src="../js/bootstrap.bundle.min.js"></script>--%>
    <script type="text/javascript"  src="../js/datatables.js"></script>


    <script type="text/javascript" src="../lib/common-scripts.js"></script>
 
  <script type="text/javascript" src="../lib/pages.js" ></script>

 <script type="text/javascript" src="../lib/advanced-form-components.js"></script>

  <script type="text/javascript" src="../js/buttons/1.6.4/dataTables.buttons.min.js"></script>  
     <script type="text/javascript" src="../js/buttons/1.6.4/jszip.min.js"></script>  
     <script type="text/javascript" src="../js/buttons/1.6.4/pdfmake.min.js"></script>  
    <script type="text/javascript" src="../js/buttons/1.6.4/vfs_fonts.js"></script>  

    <script type="text/javascript" src="../js/buttons/1.6.4/buttons.print.min.js"></script>  
     <script type="text/javascript" src="../js/buttons/1.6.4/buttons.html5.min.js"></script>  
     <script type="text/javascript" src="../js/buttons/1.6.4/buttons.colVis.min.js"></script>  



    <script type="text/javascript">
    
    function mostrarloader() {

        try {
            
                document.getElementById("ImgCarga").className = 'ver';
            
            
            } catch (e) {
                alert('Lamentamos comunicarle que ha ocurrido el siguiente problema:\n' + e.Message);
            }
        }


    function ocultarloader(Valor) {
        try {

           
                document.getElementById("ImgCarga").className = 'nover';
           
            
             } catch (e) {
                alert('Lamentamos comunicarle que ha ocurrido el siguiente problema:\n' + e.Message);
            }
        }


  </script>

      <script type="text/javascript">
            $(document).ready(function () {
                 $('.datetimepicker').datetimepicker().datetimepicker({ lang: 'es', timepicker: false, step: 30, format: 'm/d/Y' });
              });    
      </script>

</asp:Content>