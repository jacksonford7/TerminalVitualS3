<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="damagedescuentosnivel1.aspx.cs" Inherits="CSLSite.damagedescuentosnivel1" %>
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
  <link rel="stylesheet" type="text/css" href="../lib/gritter/css/jquery.gritter.css" />



 
   <link href="../shared/estilo/jquery.datetimepicker.css" rel="stylesheet" type="text/css" />
   <script src="../Scripts/jquery.datetimepicker.js" type="text/javascript"></script>

   <link href="https://fonts.googleapis.com/css2?family=Roboto:wght@300;400;500;700&display=swap" rel="stylesheet"/>
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/5.15.1/css/all.min.css" integrity="sha512-+4zCK9k+qNFUR5X+cKL9EIR+ZOhtIloNl9GIKS57V1MyNsYpYcUrUeQc9vNfzsWfV28IaLL3i96P9sdNyeRssA==" crossorigin="anonymous" />


  <link href="../css/datatables.min.css" rel="stylesheet" />
  <link rel="stylesheet" type="text/css" href="..js/buttons/css1.6.4/buttons.dataTables.min.css"/>

 

<script type="text/javascript">


 function fechas()
   {
    $(document).ready(function () {
            $('.datetimepicker').datetimepicker().datetimepicker({ lang: 'es', step: 30, format: 'd/m/Y H:i' });
        });
      
        $(document).ready(function () {
            $('.datetimepickerAlt').datetimepicker().datetimepicker({ lang: 'es', closeOnDateSelect: true, timepicker: false, step: 30, format: 'd/m/Y' });
        });
     
        $(document).ready(function () {
            $('.datepicker').datetimepicker().datetimepicker({ lang: 'es', timepicker: false, format: 'd/m/Y', closeOnDateSelect: true, minDate: '0' });

        });

    }



</script>
 


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

     <asp:HiddenField ID="manualHide" runat="server" />

  <div id="div_BrowserWindowName" style="visibility:hidden;">
    <asp:HiddenField ID="hf_BrowserWindowName" runat="server" />
     
  </div>

     <div class="mt-4">         
        <nav class="mt-4" aria-label="breadcrumb">
          <ol class="breadcrumb">
            <li class="breadcrumb-item" id="opcion_principal" runat="server"><a href="#">DAMAGE CONTROL</a></li>
            <li class="breadcrumb-item active" aria-current="page" id="sub_opcion" runat="server">APROBACIÓN DE POLÍTICAS DE DESCUENTOS (NIVEL 1)</li>
          </ol>
        </nav>
      </div>

 <div class="dashboard-container p-4" id="cuerpo" runat="server">

     <div class="form-title">
          DETALLE DE DESCUENTOS PENDIENTES
     </div>

      <div class="row">
       <div class="col-md-12 d-flex justify-content-center">
           <asp:UpdatePanel ID="UPMENSAJE" runat="server"  UpdateMode="Conditional" >  
            <ContentTemplate>
           <div class="alert alert-warning" id="banmsg" runat="server" clientidmode="Static"><b>Error!</b> >......</div>
            </ContentTemplate>
           </asp:UpdatePanel>
       </div>
     </div>


      <div class="form-row">
          <div class="form-group col-md-12">
         <asp:UpdatePanel ID="UPDETALLE" runat="server"  >  
            <ContentTemplate>

             <script type="text/javascript">
                     Sys.Application.add_load(BindFunctions); 
            </script>

                       <asp:Repeater ID="grilla" runat="server" onitemcommand="grilla_ItemCommand" >
                       <HeaderTemplate>
                       <table cellpadding="0" cellspacing="0" border="0" class="table table-bordered table-sm table-contecon" id="grilla" width="100%" >
                        <thead>
                          <tr >
                            <th class="center hidden-phone">ID DESCUENTO</th>
                            <th class="center hidden-phone">FECHA</th>
                            <th class="center hidden-phone">DESDE</th>
                            <th class="center hidden-phone">HASTA</th>
                            <th class="center hidden-phone">CONCEPTO</th>
                            <th class="center hidden-phone">USUARIO</th>
                            <th class="center hidden-phone">ESTADO</th>
                            <th class="center hidden-phone">VER DETALLE</th>
                            <th class="center hidden-phone">APROBAR</th>
                            <th class="center hidden-phone">CANCELAR</th>
                          </tr>
                        </thead>
                        <tbody>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr >
                                <td class="center hidden-phone"><asp:Label Text='<%#Eval("DESC_ID")%>' ID="DESC_ID" runat="server"  style="align-content:center"   /></td>
                                <td class="center hidden-phone"><asp:Label Text='<%#DataBinder.Eval(Container.DataItem, "DESC_DATE_CREA", "{0:dd/MM/yyyy}")%>' ID="DESC_DATE_CREA" runat="server"  style="text-align:center"  /></td>
                                <td class="center hidden-phone"><asp:Label Text='<%#DataBinder.Eval(Container.DataItem, "DESC_DESDE", "{0:yyyy/MM/dd HH:mm}")%>' ID="DESC_DESDE" runat="server"  style="text-align:center"  /></td>
                                <td class="center hidden-phone"><asp:Label Text='<%#DataBinder.Eval(Container.DataItem, "DESC_HASTA", "{0:yyyy/MM/dd HH:mm}")%>' ID="DESC_HASTA" runat="server"  style="text-align:center"  /></td>
                                <td class="center hidden-phone"><asp:Label Text='<%#Eval("DESC_NOTA")%>' ID="DESC_NOTA" runat="server"  style="text-align:center"  /></td>
                                <td class="center hidden-phone"><asp:Label Text='<%#Eval("DESC_USER_CREA")%>' ID="DESC_USER_CREA" runat="server"  style="text-align:center"  /></td> 
                                <td class="center hidden-phone"><asp:Label Text='<%#Eval("DESC_ESTADO")%>' ID="DESC_ESTADO" runat="server"  style="text-align:center"  /></td> 
                               
                                 <td class="center hidden-phone"> 
                                     <asp:Button ID="BtnDetalle"
                                    CommandArgument= '<%#Eval("DESC_ID")%>' runat="server" Text="VER DETALLE"  class="btn btn-primary" data-toggle="modal" data-target="#exampleModalToggle" ToolTip="VER DETALLE" CommandName="Ver" />
                                </td>

                                 <td class="center hidden-phone"> 
                                     <asp:Button ID="BtnAprobar"
                                       OnClientClick="return confirm('Esta seguro que desea aprobar la política de descuento?');" 
                                    CommandArgument= '<%#Eval("DESC_ID")%>' runat="server" Text="APROBAR" 
                                        class="btn btn-primary"
                                         ToolTip="APROBAR" CommandName="Aprobar" />
                                </td>

                                 <td class="center hidden-phone"> 
                                     <asp:Button ID="BtnCancelar"
                                       OnClientClick="return confirm('Esta seguro que desea rechazar la política de descuento?');" 
                                    CommandArgument= '<%#Eval("DESC_ID")%>' runat="server" Text="RECHAZAR" class="btn btn-primary" ToolTip="RECHAZAR" CommandName="Cancelar" />
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
  <div class="modal-dialog modal-dialog-scrollable" style="max-width: 1200px">
    <div class="modal-content">
       <div class="modal-header">
           <asp:UpdatePanel ID="UPTITULO" runat="server"  UpdateMode="Conditional" ChildrenAsTriggers="true">
           <ContentTemplate>
                        <h5 class="modal-title" id="Titulo" runat="server">POLÍTICAS DE DESCUENTOS PENDIENTE DE APROBAR</h5>
                       
               </ContentTemplate>
         </asp:UpdatePanel>
            <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                        </button>
       </div>
      <div class="modal-body">

          <asp:UpdatePanel ID="UPCARGA" runat="server"  UpdateMode="Conditional" ChildrenAsTriggers="true">
          <ContentTemplate>
               <script type="text/javascript">
                            Sys.Application.add_load(fechas); 
            </script>
                 <div class="form-row">

                        <div class="form-group col-md-6"> 
                                <label for="inputEmail4"># POLÍTICA</label>
                                 <div class="d-flex">
                                      <asp:TextBox ID="TxtNumeroPolitica" runat="server"  class="datetimepicker form-control"  MaxLength="10"  placeholder="dd/MM/yyyy HH:mm"  onkeypress="return soloLetras(event,'1234567890')" disabled></asp:TextBox>
                                 </div> 
                          </div> 

                        <div class="form-group col-md-6"> 
                                <label for="inputEmail4">FECHA</label>
                                 <div class="d-flex">
                                      <asp:TextBox ID="TxtFecha" runat="server"  class="datetimepicker form-control"  MaxLength="10"  placeholder="dd/MM/yyyy HH:mm"  onkeypress="return soloLetras(event,'1234567890/:')" disabled></asp:TextBox>
                                 </div> 
                          </div> 

                          <div class="form-group col-md-3"> 
                                <label for="inputEmail4">RANGO DESDE</label>
                                 <div class="d-flex">
                                      <asp:TextBox ID="TxtFechaDescuentoDesde" runat="server"  class="datetimepicker form-control"  MaxLength="10"  placeholder="dd/MM/yyyy HH:mm"  onkeypress="return soloLetras(event,'1234567890/:')" disabled></asp:TextBox>
                                 </div> 
                          </div> 
                         <div class="form-group col-md-3"> 
                                <label for="inputEmail4">RANGO HASTA</label>
                                 <div class="d-flex">
                                      <asp:TextBox ID="TxtFechaDescuentoHasta" runat="server"  class="datetimepicker form-control"  MaxLength="10"  placeholder="dd/MM/yyyy HH:mm"  onkeypress="return soloLetras(event,'1234567890/:')" disabled></asp:TextBox>
                                 </div> 
                          </div> 

                        <div class="form-group col-md-6">
                            <label for="inputZip">CONCEPTO<span style="color: #FF0000; font-weight: bold;">*</span></label>
                             <asp:TextBox ID="TxtMotivo" runat="server" class="form-control" 
                                onkeypress="return soloLetras(event,'abcdefghijklmnopqrstuvwxyzñ1234567890 -',true)"  MaxLength="200" disabled></asp:TextBox>
                        </div>


                         <div class="form-group col-md-2">
                          <label for="inputZip">ESTADO</label>
                            <asp:DropDownList runat="server" ID="CboEstados"    AutoPostBack="false"  class="form-control" disabled >
                            <asp:ListItem Text="NUEVO" Value="N"></asp:ListItem>
                            <asp:ListItem Text="PENDIENTE" Value="P"></asp:ListItem>
                            <asp:ListItem Text="RECHAZADO" Value="R"></asp:ListItem>
                            <asp:ListItem Text="APROBADO" Value="A"></asp:ListItem>
                        </asp:DropDownList>
                        </div>

                         
                        <div class="form-group col-md-3">
                          <label for="inputZip">ELABORADO POR</label>
                           <asp:TextBox ID="TxtElaborado" runat="server" class="form-control"  MaxLength="2"   style="text-align:center" 
                                            placeholder="" disabled></asp:TextBox>
                        </div> 
                         <div class="form-group col-md-3">
                          <label for="inputZip">FECHA Y HORA</label>
             
                            <asp:TextBox ID="TxtFechaCrea" runat="server" class="form-control"  MaxLength="3"  onkeypress="return soloLetras(event,'0123456789-_')" style="text-align:center" 
                                            placeholder="" disabled></asp:TextBox>
                        </div> 
                        <div class="form-group col-md-2">
                          <label for="inputZip">&nbsp;&nbsp;</label>
                              <div class="d-flex">
                                    <asp:Button ID="BtnExcel" runat="server"  class="btn btn-primary"  Text="EXPORTAR EXCEL"  OnClick="BtnExcel_Click" />      
                              </div> 
                        </div> 

                       <br/>
                          <div class="form-group col-md-12">
                            <div class="alert alert-warning" id="banmsg2" runat="server" clientidmode="Static"><b>Error!</b> >....</div>
                        </div> 
                 </div>
       </ContentTemplate>
        
    </asp:UpdatePanel>


           <asp:UpdatePanel ID="UPDATOSCLIENTE" runat="server"  UpdateMode="Conditional" ChildrenAsTriggers="true">
      <ContentTemplate>
         <div class="form-row">
                <div class="col-md-12 d-flex justify-content-center">
                 
                  
                    
              </div>
        </div>
     </ContentTemplate>
        <Triggers>
        <asp:AsyncPostBackTrigger ControlID="BtnExcel" />
    </Triggers>
    </asp:UpdatePanel>

          <div class="form-row">
          <div class="form-group col-md-12">
            <asp:UpdatePanel ID="UPDETTURNOS" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">  
            <ContentTemplate>

                       <asp:Repeater ID="tablePagination" runat="server"  >
                       <HeaderTemplate>
                       <table cellpadding="0" cellspacing="0" border="0" class="table table-bordered invoice" id="hidden-table-info">
                           <thead>
                          <tr >
                            <th class="center hidden-phone">#</th>
                            <th class="center hidden-phone">LÍNEA NAVIERA</th>
                            <th class="center hidden-phone">% DESCUENTO</th>

                          </tr>
                        </thead>
                        <tbody>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr >
                                <td class="center hidden-phone"><asp:Label Text='<%#Eval("DESC_FILA")%>' ID="DESC_FILA" runat="server"  style="align-content:center"   /></td>
                                <td class="center hidden-phone"><asp:Label Text='<%#Eval("DESC_LIN_DESCRIP")%>' ID="DESC_LIN_DESCRIP" runat="server"  style="text-align:center"  /></td>
                                <td class="center hidden-phone"><asp:Label Text='<%#Eval("DESC_PORCENTAJE")%>' ID="DESC_PORCENTAJE" runat="server"  style="text-align:center"  /></td> 

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
      <div class="modal-footer">
        <asp:Button ID="BtnAprobar" runat="server"  class="btn btn-primary"  Text="APROBAR"  OnClientClick="return confirmacion()"   OnClick="BtnAprobar_Click" />    
        <button type="button" class="btn btn-outline-primary " data-dismiss="modal">CERRAR</button>
        
      </div>
    </div>
  </div>
</div>


        

<script type="text/javascript">
   
    function confirmacion()
    {
        var mensaje;
        var opcion = confirm("Estimado cliente, está seguro que desea aprobar la política de descuento. ?");
        if (opcion == true)
        {
            return true;
        } else
        {
	         return false;
        }
 
    }

</script>

  <!--common script for all pages-->
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
  <!--script for this page-->
 

<script type="text/javascript">

  $(document).ready(function () {
            $('.datetimepicker').datetimepicker().datetimepicker({ lang: 'es', step: 30, format: 'd/m/Y H:i' });
        });
      
        $(document).ready(function () {
            $('.datetimepickerAlt').datetimepicker().datetimepicker({ lang: 'es', closeOnDateSelect: true, timepicker: false, step: 30, format: 'd/m/Y' });
        });
     
        $(document).ready(function () {
            $('.datepicker').datetimepicker().datetimepicker({ lang: 'es', timepicker: false, format: 'd/m/Y', closeOnDateSelect: true, minDate: '0' });

        });


    function mostrarloader(Valor) {

        try {
            if (Valor == "1") {
                document.getElementById("ImgCarga").className = 'ver';
            }
            else {
                
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
                
            }

             } catch (e) {
                alert('Lamentamos comunicarle que ha ocurrido el siguiente problema:\n' + e.Message);
            }
        }

</script>

 <script type="text/javascript">
        function descarga(fname, hname, tbname) {
            var iframe = document.createElement("iframe");
            iframe.src = "../handler/fileExcel.ashx?name="+fname+"&page="+hname+"&obj="+tbname
            iframe.style.display = "none";
            document.body.appendChild(iframe);
        }
    </script>



</asp:Content>