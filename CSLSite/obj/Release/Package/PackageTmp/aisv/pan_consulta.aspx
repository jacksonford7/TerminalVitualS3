<%@ Page Title="Consulta de Contenedores - PAN" Language="C#" MasterPageFile="~/site.Master" AutoEventWireup="true" CodeBehind="pan_consulta.aspx.cs" Inherits="CSLSite.pan_consulta" %>

<%@ MasterType VirtualPath="~/site.Master" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="placehead" runat="server">

  <%--  <link href="../shared/estilo/tables.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/aisv-general.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/work.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/catalogos.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/info.css" rel="stylesheet" type="text/css" />
    <link href="../../shared/estilo/chosen.css" rel="stylesheet" type="text/css" />

    <link href="../shared/estilo/proforma.css" rel="stylesheet" type="text/css" />--%>

    <link href="../img/favicon2.png" rel="icon"/>
    <link href="../img/icono.png" rel="apple-touch-icon"/>
    <link href="../css/bootstrap.min.css" rel="stylesheet"/>
    <link href="../css/dashboard.css" rel="stylesheet"/>
    <link href="../css/icons.css" rel="stylesheet"/>
    <link href="../css/style.css" rel="stylesheet"/>
    <link href="https://fonts.googleapis.com/css2?family=Roboto:wght@300;400;500;700&display=swap" rel="stylesheet"/>
    
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/5.15.1/css/all.min.css" integrity="sha512-+4zCK9k+qNFUR5X+cKL9EIR+ZOhtIloNl9GIKS57V1MyNsYpYcUrUeQc9vNfzsWfV28IaLL3i96P9sdNyeRssA==" crossorigin="anonymous" />
    <link href="../css/datatables.min.css" rel="stylesheet" />
  <link rel="stylesheet" type="text/css" href="..js/buttons/css1.6.4/buttons.dataTables.min.css"/>
    
     <link href="../shared/estilo/jquery.datetimepicker.css" rel="stylesheet" type="text/css" />
  <script src="../Scripts/jquery.datetimepicker.js" type="text/javascript"></script>

  

<script type="text/javascript">

     

 function BindFunctions()
 {
       $(document).ready(function () {
                 $('.datetimepicker').datetimepicker().datetimepicker({ lang: 'es', timepicker: false, step: 30, format: 'd/m/Y' });
       });    

   

   $(document).ready(function() {    
    $('#tablePagination').DataTable({        
       
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
<asp:Content ID="Content2" ContentPlaceHolderID="placebody" runat="server">
    <asp:ToolkitScriptManager ID="tkcal" EnableScriptGlobalization="true" runat="server">
    </asp:ToolkitScriptManager>
    <input id="zonaid" type="hidden" value="901" />
    <noscript>
       
    </noscript>
    <asp:UpdatePanel ID="updConsultaUsuarios" runat="server">
        <ContentTemplate>
        
            <div class="mt-4">         
                <nav class="mt-4" aria-label="breadcrumb">
                    <ol class="breadcrumb">
                    <li class="breadcrumb-item" id="opcion_principal" runat="server"><a href="#">PNA</a></li>
                        <li class="breadcrumb-item active" aria-current="page" id="sub_opcion" runat="server">CONSULTA CONTENEDORES</li>
                    </ol>
                </nav>
            </div>
            
            <div class="dashboard-container p-4" id="cuerpo" runat="server">

                <div class="form-title">
                   	Consulta de usuarios
                </div>
                <h6>Criterios de Búsqueda.</h6>

                            <div class="form-row">
                                    <div class="form-group col-md-6"> 
                                          <label for="inputAddress">Generados desde el día:<span style="color: #FF0000; font-weight: bold;"></span></label>
                                           <asp:TextBox runat="server" ID="TxtFechaDesde"   MaxLength="16" 
                                                        onkeypress="return soloLetras(event,'0123456789-')"    class="datetimepicker form-control" ></asp:TextBox>
                                    </div>
                                    <div class="form-group col-md-6"> 
                                         <label for="inputAddress">Hasta:<span style="color: #FF0000; font-weight: bold;"></span></label>
                                                       <asp:TextBox ID="TxtFechaHasta" runat="server" ClientIDMode="Static"  MaxLength="15" CssClass="datetimepicker form-control" 
                                                    onkeypress="return soloLetras(event,'0123456789-')" 
                                                          ></asp:TextBox>
                                          <span id="valdate" class="validacion"></span>
                                   </div>
                                  <div class="form-group col-md-6"> 
                                       <label for="inputAddress">Contenedor:<span style="color: #FF0000; font-weight: bold;"></span></label>
                                        <asp:TextBox ID="TxtBusCliente" runat="server"  MaxLength="100"  class="form-control"             
                                                      onkeypress="return soloLetras(event,'01234567890abcdefghijklmnopqrstuvwxyz.',true)"></asp:TextBox>
                                        <span id="valran" class="opcional"></span>
                                 </div>
                                                
                                 <div class="form-group col-md-2">
                                      <label for="inputAddress">&nbsp;</label>
                                      <asp:Button ID="BtnBuscarCliente" runat="server" class="btn btn-primary form-control" Text="BUSCAR" OnClientClick="return mostrarloader()" OnClick="BtnBuscarCliente_Click"/>
                                                    
                                 </div>
                                 <div class="form-group col-md-2">
                                                    
                                 </div>
                                <div class="form-group col-md-2">
                                                    
                                 </div>
                               
                            </div>
                             <div class="row">
                                    <div class="col-md-12 d-flex justify-content-center">
                                                 <img alt="loading.." src="../lib/file-uploader/img/loading.gif" height="32px" width="32px"  id="ImgCarga" class="nover"   />
                                     </div>
                                </div>    
         

                <div><br /></div>


                <div class="accion">
                   
                    <div id="error" runat="server" class="alert alert-danger" visible="false">
                    </div>
                       
                        <br/>
                        <asp:UpdatePanel ID="upresult" runat="server" ChildrenAsTriggers="true">
                            <ContentTemplate>
                                <script type="text/javascript">Sys.Application.add_load(BindFunctions); </script>
                                <input id="idlin" type="hidden" runat="server" clientidmode="Static" />
                                <input id="diponible" type="hidden" runat="server" clientidmode="Static" />
                                <div class="alert alert-warning" id="alerta" runat="server">
                                </div>
                                <div id="xfinder" runat="server" visible="false">

                                                <asp:Repeater ID="tablePagination" runat="server"   >
                                                    <HeaderTemplate>
                                                        <table  cellspacing="1" cellpadding="1" class="table table-bordered table-sm table-contecon" id="tablePagination" >
                                                            <thead>
                                                                <tr>
                                                                    <th>
                                                                        FECHA
                                                                    </th>
                                                                    <th>
                                                                        CONTENEDOR 
                                                                    </th>
                                                                    <th>
                                                                        TIPO
                                                                    </th>
                                                                    <th>
                                                                        REFERENCIA
                                                                    </th>
                                                                    <th>
                                                                       ESTADO
                                                                    </th>
                                                                   
                                                                 
                                                                   
                                                                </tr>
                                                            </thead>
                                                            <tbody>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <tr class="point"  id="trUsuario">
                                                            <td>
                                                                <%#Eval("FECHA_ACCION")%>
                                                            </td>
                                                            <td>
                                                                <%#Eval("CONTENEDOR")%>
                                                            </td>
                                                            <td>
                                                                <%#Eval("ACCION")%>
                                                            </td>
                                                            <td>
                                                                <%#Eval("REFERENCIA")%>
                                                            </td>
                                                            <td>
                                                                <%#Eval("ESTADO")%>
                                                            </td>

                                                        </tr>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        </tbody> </table>
                                                    </FooterTemplate>
                                                </asp:Repeater>
                                        
                                           
                                    
                                </div>
                                <div id="sinresultado" runat="server" class="alert alert-info">
                                </div>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="BtnBuscarCliente" />
                            </Triggers>
                        </asp:UpdatePanel>
                   
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

      <script type="text/javascript" src="../js/bootstrap.bundle.min.js"></script>
    <script type="text/javascript"  src="../js/datatables.js"></script>

       <script type="text/javascript" src="../js/buttons/1.6.4/dataTables.buttons.min.js"></script>  
     <script type="text/javascript" src="../js/buttons/1.6.4/jszip.min.js"></script>  
     <script type="text/javascript" src="../js/buttons/1.6.4/pdfmake.min.js"></script>  
    <script type="text/javascript" src="../js/buttons/1.6.4/vfs_fonts.js"></script>  

    <script type="text/javascript" src="../js/buttons/1.6.4/buttons.print.min.js"></script>  
     <script type="text/javascript" src="../js/buttons/1.6.4/buttons.html5.min.js"></script>  
     <script type="text/javascript" src="../js/buttons/1.6.4/buttons.colVis.min.js"></script>  

     <script src="../Scripts/pages.js" type="text/javascript"></script>
     


    <script type="text/javascript">
            $(document).ready(function () {
                 $('.datetimepicker').datetimepicker().datetimepicker({ lang: 'es', timepicker: false, step: 30, format: 'd/m/Y' });
              });    
      </script>

      <script type="text/javascript">
    

    function mostrarloader() {

        try {
            
                document.getElementById("ImgCarga").className = 'ver';
            
                
        }
        catch (e) {
                alert('Lamentamos comunicarle que ha ocurrido el siguiente problema:\n' + e.Message);
            }
        }


    function ocultarloader() {
        try {

           
                document.getElementById("ImgCarga").className = 'nover';
           

             } catch (e) {
                alert('Lamentamos comunicarle que ha ocurrido el siguiente problema:\n' + e.Message);
            }
        }

      
  </script>

</asp:Content>
