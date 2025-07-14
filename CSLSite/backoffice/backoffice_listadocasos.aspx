<%@ Page  Title="CONSULTA DE CASOS"  Language="C#" MasterPageFile="~/site.Master" AutoEventWireup="true" CodeBehind="backoffice_listadocasos.aspx.cs" Inherits="CSLSite.backoffice.backoffice_listadocasos" %>
<%@ MasterType VirtualPath="~/site.Master" %>
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
  <link href="../lib/font-awesome/css/font-awesome.css" rel="stylesheet" />
     <link href="../shared/estilo/jquery.datetimepicker.css" rel="stylesheet" type="text/css" />
   <script src="../Scripts/jquery.datetimepicker.js" type="text/javascript"></script>
 


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
                "oPaginate": {
                    "sFirst": "Primero",
                    "sLast":"Último",
                    "sNext":"Siguiente",
                    "sPrevious": "Anterior"
			     },
			     "sProcessing":"Procesando...",
            },
        //para usar los botones   
        responsive: "true",
        dom: 'Bfrtilp',       
        buttons:[ 
			{
				extend:    'excelHtml5',
				text:      '<i class="fa fa-file-excel-o"></i> ',
				titleAttr: 'Exportar a Excel',
				className: 'btn btn-success'
			},
			{
				extend:    'pdfHtml5',
				text:      '<i class="fa fa-file-pdf-o"></i> ',
				titleAttr: 'Exportar a PDF',
				className: 'btn btn-danger'
			},
			{
				extend:    'print',
				text:      '<i class="fa fa-print"></i> ',
				titleAttr: 'Imprimir',
				className: 'btn btn-info'
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
            <li class="breadcrumb-item" id="opcion_principal" runat="server"><a href="#">BackOffice</a></li>
            <li class="breadcrumb-item active" aria-current="page" id="sub_opcion" runat="server">CONSULTA DE CASOS</li>
          </ol>
        </nav>
      </div>
    <div class="dashboard-container p-4" id="cuerpo" runat="server">

         <div class="form-title">
             CRITERIOS DE BÚSQUEDAS
        </div>
         <div class="form-row"> 
              <div class="form-group col-md-6"> 
                    <label for="inputEmail4">RANGO DE FECHAS:</label>
                     <div class="d-flex">
                          <asp:TextBox ID="TxtFechaDesde" runat="server"  class="datetimepicker form-control"  MaxLength="10"  placeholder="dd/MM/yyyy" onkeypress="return soloLetras(event,'1234567890/:')"></asp:TextBox>
                          <div class="ml-3 mr-1">Hasta</div>&nbsp;
                         <asp:TextBox ID="TxtFechaHasta" runat="server"  class="datetimepicker form-control"   placeholder="dd/MM/yyyy" onkeypress="return soloLetras(event,'1234567890/:')"></asp:TextBox>
                     </div> 
              </div> 
             <div class="form-group col-md-6">
                     <label for="inputEmail4">USUARIO:</label>
                      <asp:DropDownList runat="server" ID="CboUsuarios"    AutoPostBack="false"  class="form-control"  ></asp:DropDownList>
                      
              </div> 
        </div> 
         <div class="row">
             <div class="col-md-12 d-flex justify-content-center">
                      <asp:UpdatePanel ID="UPACCION" runat="server"  UpdateMode="Conditional" ChildrenAsTriggers="true">
                        <ContentTemplate>
                              <asp:Button ID="BtnBuscar" runat="server" class="btn btn-primary"  Text="BUSCAR"  OnClientClick="return mostrarloader('1')" OnClick="BtnBuscar_Click" />                             
                               <img alt="loading.." src="../lib/file-uploader/img/loading.gif" height="32px" width="32px"  id="ImgCarga" class="nover"   />
                        </ContentTemplate>
                         <Triggers>
                          <asp:AsyncPostBackTrigger ControlID="BtnBuscar" />
                        </Triggers>
                        </asp:UpdatePanel>
              </div>
         </div>    
        <br/>
       <div class="row">
           <div class="col-md-12 d-flex justify-content-center">
                <asp:UpdatePanel ID="UPCARGA" runat="server"  UpdateMode="Conditional" ChildrenAsTriggers="true">
                <ContentTemplate>
                          
                         <div class="alert alert-warning" id="banmsg" runat="server" clientidmode="Static"><b>Error!</b>......</div>
    
                        </ContentTemplate>
                         <Triggers>
                          <asp:AsyncPostBackTrigger ControlID="BtnBuscar" />
                 </Triggers>
                 </asp:UpdatePanel>
            </div>
       </div>
      
			
				
					 
						
                     
                        
                      
            <div class="form-row">
        <div class="form-group col-md-12">
     <asp:UpdatePanel ID="UPDETALLE" runat="server" UpdateMode="Conditional" >  
       <ContentTemplate>
             <script type="text/javascript">
                            Sys.Application.add_load(BindFunctions); 
            </script>
          
           <!-- page start-->
        <div class="chat-room mt">
          <aside class="mid-side">
            <div class="chat-room-head">
              <h3>DETALLE DE CASOS</h3>
            
            </div>
            <div class="room-desk" id="htmlcasos" runat="server">
             
         
                    
           </div>
            
          </aside>
         
        </div>
        <!-- page end-->


           </ContentTemplate>
     </asp:UpdatePanel>   
            </div>
    </div>
	    
    
   
   </div>

    <script type="text/javascript">
    
    function mostrarloader(Valor) {

        try {
            if (Valor == "1") {
                document.getElementById("ImgCarga").className = 'ver';
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