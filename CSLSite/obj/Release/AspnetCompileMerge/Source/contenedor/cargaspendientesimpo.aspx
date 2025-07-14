<%@ Page Title="" Language="C#" MasterPageFile="~/site.Master" AutoEventWireup="true" CodeBehind="cargaspendientesimpo.aspx.cs" Inherits="CSLSite.cargaspendientesimpo" %>
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


 <link href="https://fonts.googleapis.com/css2?family=Roboto:wght@300;400;500;700&display=swap" rel="stylesheet"/>
 <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/5.15.1/css/all.min.css" integrity="sha512-+4zCK9k+qNFUR5X+cKL9EIR+ZOhtIloNl9GIKS57V1MyNsYpYcUrUeQc9vNfzsWfV28IaLL3i96P9sdNyeRssA==" crossorigin="anonymous" />


  <link href="../css/datatables.min.css" rel="stylesheet" />
  <link rel="stylesheet" type="text/css" href="..js/buttons/css1.6.4/buttons.dataTables.min.css"/>

<%--  <link href="../css/loader.css" rel="stylesheet"/>--%>


</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="placebody" runat="server" >

   <asp:ToolkitScriptManager ID="tkscata" runat="server"></asp:ToolkitScriptManager>

     <div class="loader"></div>

     <div class="mt-4">         
        <nav class="mt-4" aria-label="breadcrumb">
          <ol class="breadcrumb">
            <li class="breadcrumb-item" id="opcion_principal" runat="server"><a href="#">Impo Contenedores</a></li>
            <li class="breadcrumb-item active" aria-current="page" id="sub_opcion" runat="server">CARGAS PENDIENTES DE FACTURAR</li>
          </ol>
        </nav>
      </div>

<div class="dashboard-container p-4" id="cuerpo" runat="server">
    <div class="form-title">
            CARGAS PENDIENTES DE FACTURAR
    </div>

   
       
			 <h4 class="mb">DATOS DEL USUARIO</h4>
              <div class="form-row">
                <div class="form-group col-md-6"> 
                    <label for="inputAddress">ESTIMADO CLIENTE:<span style="color: #FF0000; font-weight: bold;"></span></label>
						<asp:TextBox ID="Txtcliente" runat="server" class="form-control"  size="80" 
                        placeholder="JORGE ALVARADO"  Font-Bold="true" disabled ></asp:TextBox>
				</div>
                <div class="form-group col-md-6"> 
                    <label for="inputAddress">RUC:<span style="color: #FF0000; font-weight: bold;"></span></label>
						<asp:TextBox ID="Txtruc" runat="server" class="form-control"  size="25" 
                        placeholder="0923370530"  Font-Bold="true" disabled></asp:TextBox>

				</div>                                 
             </div> 
       
      <div class="form-title">
          DETALLE DE CARGAS
     </div>
      
          <div class="form-row">
          <div class="form-group col-md-12">
            <script type="text/javascript">
                            Sys.Application.add_load(BindFunctions); 
            </script>
              <asp:UpdatePanel ID="UPDETALLE" runat="server" UpdateMode="Conditional" >  
                <ContentTemplate>
              <asp:Repeater ID="tablePagination" runat="server"  >
                       <HeaderTemplate>
                       <table cellpadding="0" cellspacing="0" border="0" class="table table-bordered invoice" id="hidden-table-info">
                           <thead>
                          <tr>
                            <th class="center hidden-phone">NUMERO DE CARGA</th>
                            <th class="center hidden-phone">TIPO DE CARGA</th>
                            <th class="center hidden-phone">REFERENCIA</th>
                            <th class="center hidden-phone">IMPORTADOR</th>
                            <th class="center hidden-phone">DECLARACION ADUANERA</th>
                            <th class="center hidden-phone">BILL OF LADING</th>
                            <th class="center hidden-phone">TOTAL PARTIDAS</th>
                            <th class="center hidden-phone">ACCION</th>
                          </tr>
                        </thead>
                        <tbody>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr >
                                <td class="center hidden-phone"> <%#Eval("CARGA")%></td>
                                <td class="center hidden-phone"> <%#Eval("TIPO")%></td>
                                <td class="center hidden-phone"> <%#Eval("REFERENCIA")%></td>
                                <td class="center hidden-phone"> <%#Eval("IMPORTADOR")%></td>
                                <td class="center hidden-phone"> <%#Eval("DECLARACION")%></td>
                                <td class="center hidden-phone"> <%#Eval("BL")%></td>
                                <td class="center hidden-phone"> <%#Eval("TOTAL_PARTIDAS")%></td>
                                <td class="center hidden-phone">
                                    <a href="../contenedor/contenedorimportacion.aspx?ID_CARGA=<%#securetext(Eval("LLAVE")) %>"  target="_parent"><button type="button" class="btn btn-primary"><i class="fa fa-file-o"></i> Facturar</button></a>
                                   
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
   

   <script type="text/javascript" src="../js/bootstrap.bundle.min.js"></script>
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
    
      
 function BindFunctions()
{
           $(document).ready(function() {    
            $('#hidden-table-info').DataTable({        
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

    //$(document).ready(function() {
     
    //  var nCloneTh = document.createElement('th');
    //  var nCloneTd = document.createElement('td');
    //  nCloneTd.innerHTML = '<img src="../lib/advanced-datatable/images/details_open.png">';
    //  nCloneTd.className = "center";

    //  $('#hidden-table-info thead tr').each(function() {
       
    //  });

    //  $('#hidden-table-info tbody tr').each(function() {
        
    //  });

     
    //  var oTable = $('#hidden-table-info').dataTable({
    //    "aoColumnDefs": [{
    //      "bSortable": false,
    //      "aTargets": [0]
    //    }],
    //    "aaSorting": [
    //      [1, 'asc']
    //    ]
    //  });

     
  </script>

<%--    <script type="text/javascript">
$(window).load(function() {
    $(".loader").fadeOut("slow");
});
</script>--%>

<script type="text/javascript">
            $(document).ready(function () {
                 $('.datetimepicker').datetimepicker().datetimepicker({ lang: 'es', timepicker: false, step: 30, format: 'm/d/Y' });
              });    
      </script>

</asp:Content>