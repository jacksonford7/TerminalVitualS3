<%@ Page Title="Administrar Usuarios STC" Language="C#" MasterPageFile="~/site.Master" AutoEventWireup="true" CodeBehind="consola_impo.aspx.cs" Inherits="CSLSite.consola_impo" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="placehead" runat="server">
  
    <link href="../img/favicon2.png" rel="icon"/>
  <link href="../img/icono.png" rel="apple-touch-icon"/>
 <!-- Bootstrap core CSS -->
  <link href="../css/bootstrap.min.css" rel="stylesheet"/>
  <link href="../css/dashboard.css" rel="stylesheet"/>
  <link href="../css/icons.css" rel="stylesheet"/>
  <link href="../css/style.css" rel="stylesheet"/>
    
<%--  <link href="css/datatables.min.css" rel="stylesheet"/>--%>
  <!--external css-->
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
    $('#tablasort').DataTable({        
       
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
    <asp:ToolkitScriptManager ID="tkcal" EnableScriptGlobalization="true" runat="server"> </asp:ToolkitScriptManager>
           <div class="mt-4">         
        <nav class="mt-4" aria-label="breadcrumb">
          <ol class="breadcrumb">
            <li class="breadcrumb-item" id="opcion_principal" runat="server"><a href="#">CGSApp</a></li>
             <li class="breadcrumb-item active" aria-current="page" id="sub_opcion" runat="server">Consulta de Clientes</li>
          </ol>
        </nav>
      </div>
     <div class="dashboard-container p-4" id="cuerpo" runat="server">

         <div class="form-title">Datos del cliente buscado</div>
		 
		  <div class="form-row">
		  
		   <div class="form-group col-md-6"> 
		   	  <label for="inputAddress">RUC No.<span style="color: #FF0000; font-weight: bold;"></span></label>
			  <div class="d-flex">
                    <asp:TextBox ID="txtruc" runat="server" MaxLength="15"  CssClass="form-control"
             onkeypress="return soloLetras(event,'01234567890',true)"></asp:TextBox>

			  </div>
		   </div>
 </div>
         
          <div class="form-row">
		  
		   <div class="form-group col-md-6"> 
		   	  <label for="inputAddress">Nombres o Descripción<span style="color: #FF0000; font-weight: bold;"></span></label>
			  <div class="d-flex">
                            <asp:TextBox ID="txtnombre" runat="server" MaxLength="100"   CssClass="form-control"
             
                  onkeypress="return soloLetras(event,'01234567890abcdefghijklmnopqrstuvwxyz',true)"></asp:TextBox>


			  </div>
		   </div>
		  </div>

           <div class="form-row">
		   <div class="col-md-12 d-flex justify-content-center"> 
		     <div class="d-flex">

                           <span id="imagen"></span>
             <asp:Button ID="btbuscar" runat="server" Text="Buscar"   CssClass="btn btn-primary" 
                 onclick="btbuscar_Click" OnClientClick="return validarUsuario();" />

		     </div>
		   </div> 
		   </div>
		 
           
          <asp:UpdatePanel ID="upresult" runat="server" ChildrenAsTriggers="true">
                     <ContentTemplate>
                        <script type="text/javascript">
                            Sys.Application.add_load(BindFunctions); 
                      </script>


             <div id="xfinder" runat="server" visible="false" >

                  <div class=" form-title">Clientes encontrados</div>
                 <div id="sinresultado" runat="server" class="alert alert-danger"></div><br/>
                 <asp:Repeater ID="tablePagination" runat="server" 
                         onitemcommand="tablePagination_ItemCommand" >
                 <HeaderTemplate>
                 <table id="tablasort" cellpadding="0" cellspacing="0"  border="0" class="table table-bordered table-sm table-contecon" width="100%">
                 <thead>
                 <tr>
                
                 <th>ID</th>
                 <th>RUC</th>
                 <th>Nombres/Descripción</th>
                 <th>Categoría</th>
                 <th>Registrado</th>
                 <th>Estado</th>
                 <th>Comentario</th>
                 <th>Acciones</th>
                 </tr>
                 </thead> 
                 <tbody>
                 </HeaderTemplate>
                 <ItemTemplate>
                 <tr class="point" >
                  
                  <td><%#Eval("cliente_id")%></td>
                  <td><%#Eval("cliente_ruc")%></td>
                  <td><%#Eval("cliente_nombres")%></td>
                  <td><%#Eval("cliente_categoria")%></td>
                  <td><%#Eval("cliente_creado")%></td>
                  <td  ><%# anulado(Eval("cliente_estado"))%></td>
                  <td ><asp:TextBox ID="Txtcomentario" runat="server" class="form-control"  ForeColor="Red" ToolTip='<%#Eval("notas")%>' Text='<%#Eval("notas")%>' onkeypress="return soloLetras(event,'abcdefghijklnmñopqrstuvwxyz01234567890-_/ ',true)"></asp:TextBox></td>

                  <td>
                   <div class="tcomand">
                       <%--<a href="../sna/plus_usuario.aspx?sid=<%# securetext(Eval("cliente_id")) %>" 
                           class=" btn btn-link" target="_blank">Editar</a>|--%>
                  
                       <asp:Button ID="Activar"  
                       OnClientClick="return confirm('Esta seguro que desea actualizar este registro?');" 
                       CommandArgument ='<%# jsarguments( Eval("cliente_id"),Eval("cliente_estado"),Eval("cliente_categoria") )%>'
                       CommandName="update"
                       runat="server" 
                       Text='<%# botonText(Eval("cliente_estado"))%>'  
                       CssClass=" btn btn-secondary" 
                       ToolTip="Permite anular este documento" 
                           />
                     
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
               
                  </ContentTemplate>
                     <Triggers>
                     <asp:AsyncPostBackTrigger ControlID="btbuscar" />
                     </Triggers>
                 </asp:UpdatePanel>
          
		 
		
		 
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


  <asp:updateprogress associatedupdatepanelid="upresult"
    id="updateProgress" runat="server">
    <progresstemplate>
        <div id="progressBackgroundFilter"></div>
        <div id="processMessage"> Estamos procesando la tarea que solicitó, por favor 
            espere...<br />
             <img alt="Loading" src="../shared/imgs/loader.gif" style="margin:0 auto;" />
        </div>
    </progresstemplate>
</asp:updateprogress>


 
  </asp:Content>
