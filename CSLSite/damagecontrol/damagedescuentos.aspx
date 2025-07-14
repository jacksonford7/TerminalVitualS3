<%@ Page Title="" Language="C#" MasterPageFile="~/site.Master" AutoEventWireup="true" CodeBehind="damagedescuentos.aspx.cs" Inherits="CSLSite.damagedescuentos" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="placehead" runat="server">

   <link href="../img/favicon2.png" rel="icon"/>
  <link href="../img/icono.png" rel="apple-touch-icon"/>
 <!-- Bootstrap core CSS -->
  <link href="../css/bootstrap.min.css" rel="stylesheet"/>
  <link href="../css/dashboard.css" rel="stylesheet"/>
  <link href="../css/icons.css" rel="stylesheet"/>
  <link href="../css/style.css" rel="stylesheet"/>

       <link href="../lib/font-awesome/css/font-awesome.css" rel="stylesheet" />
  <link rel="stylesheet" type="text/css" href="../lib/gritter/css/jquery.gritter.css" />

    <link href="https://fonts.googleapis.com/css2?family=Roboto:wght@300;400;500;700&display=swap" rel="stylesheet"/>
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/5.15.1/css/all.min.css" integrity="sha512-+4zCK9k+qNFUR5X+cKL9EIR+ZOhtIloNl9GIKS57V1MyNsYpYcUrUeQc9vNfzsWfV28IaLL3i96P9sdNyeRssA==" crossorigin="anonymous" />


  <!--external css-->
  <link href="../shared/estilo/jquery.datetimepicker.css" rel="stylesheet" type="text/css" />
  <script src="../Scripts/jquery.datetimepicker.js" type="text/javascript"></script>



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




 
  

</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="placebody" runat="server" >

   <asp:ToolkitScriptManager ID="tkscata" runat="server"></asp:ToolkitScriptManager>

  <div id="div_BrowserWindowName" style="visibility:hidden;">
    <asp:HiddenField ID="hf_BrowserWindowName" runat="server" />
     
  </div>
    <asp:HiddenField ID="manualHide" runat="server" />
     <input id="ID_NOVEDAD" type="hidden" value="" runat="server" clientidmode="Static" />
    <input id="NOVEDAD" type="hidden" value="" runat="server" clientidmode="Static" />

    <div class="mt-4">         
        <nav class="mt-4" aria-label="breadcrumb">
          <ol class="breadcrumb">
            <li class="breadcrumb-item" id="opcion_principal" runat="server"><a href="#">DAMAGE CONTROL</a></li>
            <li class="breadcrumb-item active" aria-current="page" id="sub_opcion" runat="server">POLÍTICAS DE DESCUENTOS</li>
          </ol>
        </nav>
      </div>
<div class="dashboard-container p-4" id="cuerpo" runat="server">
    <div class="form-title">
           CREAR DESCUENTOS
     </div>
       <div class="form-row"> 
           <div class="form-group col-md-6"> 
                <label for="inputZip">RANGO DESDE<span style="color: #FF0000; font-weight: bold;">*</span></label>
               <asp:TextBox runat="server" ID="TxtFechaDescuentoDesde"   MaxLength="16" 
                                         onkeypress="return soloLetras(event,'0123456789-')"    class="datetimepicker form-control" ></asp:TextBox>
           </div>
           <div class="form-group col-md-6"> 
                <label for="inputZip">RANGO HASTA<span style="color: #FF0000; font-weight: bold;">*</span></label>
               <asp:TextBox runat="server" ID="TxtFechaDescuentoHasta"   MaxLength="16" 
                                         onkeypress="return soloLetras(event,'0123456789-')"    class="datetimepicker form-control" ></asp:TextBox>
           </div>
       </div>
      <asp:UpdatePanel ID="UPCARGA" runat="server"  UpdateMode="Conditional" >
       <ContentTemplate>
        <div class="form-row"> 

              <div class="form-group col-md-6">
                <label for="inputZip">CONCEPTO<span style="color: #FF0000; font-weight: bold;">*</span></label>
                 <asp:TextBox ID="TxtMotivo" runat="server" class="form-control" 
                    onkeypress="return soloLetras(event,'abcdefghijklmnopqrstuvwxyzñ1234567890 -',true)"  MaxLength="200"></asp:TextBox>
            </div>


           <div class="form-group col-md-6">
              <label for="inputZip">ESTADO<span style="color: #FF0000; font-weight: bold;">*</span></label>
                <asp:DropDownList runat="server" ID="CboEstados"    AutoPostBack="false"  class="form-control" disabled >
                    <asp:ListItem Text="NUEVO" Value="N"></asp:ListItem>
                    <asp:ListItem Text="PENDIENTE" Value="P"></asp:ListItem>
                    <asp:ListItem Text="RECHAZADO" Value="R"></asp:ListItem>
                    <asp:ListItem Text="APROBADO" Value="A"></asp:ListItem>
                </asp:DropDownList>
            </div>
       </div>

        </ContentTemplate>  
        </asp:UpdatePanel>

   

   
     <div class="form-title">
          LÍNEA NAVIERA
     </div>

    <asp:UpdatePanel ID="UPDESCUENTO" runat="server"  UpdateMode="Conditional" ChildrenAsTriggers="true">
    <ContentTemplate>
     <div class="form-row"> 

         <div class="form-group col-md-3">
                <label for="inputEmail4">LÍNEA NAVIERA:</label>
                  <asp:DropDownList runat="server" ID="CboLineaNaviera"     AutoPostBack="true"  class="form-control"    >
                  </asp:DropDownList>      
          </div>

          <div class="form-group col-md-3"> 
              <label for="inputAddress">% DESCUENTO:<span style="color: #FF0000; font-weight: bold;">*</span></label>  
                <asp:TextBox ID="TxtCantidad" runat="server" class="form-control"    
                                placeholder=""  MaxLength="10" Font-Bold="false" onkeypress="return soloLetras(event,'0123456789')"
                                    ClientIDMode="Static"></asp:TextBox>
           </div>
        
        
          <div class="form-group col-md-1"> 
              <label for="inputZip">&nbsp;</label>
               <div class="d-flex">
                        <asp:Button ID="BtnBuscar" runat="server" class="btn btn-primary"   Text="AGREGAR"   OnClick="BtnBuscar_Click" />   
                         <img alt="loading.." src="../lib/file-uploader/img/loading.gif" height="32px" width="32px"  id="ImgCarga" class="nover"   />
                         
                  </div>
              
          </div>
     </div> 

    </ContentTemplate>
          <Triggers>
            <asp:AsyncPostBackTrigger ControlID="BtnBuscar" />
        </Triggers>
    </asp:UpdatePanel>

      <asp:UpdatePanel ID="UPMENSAJE" runat="server"  UpdateMode="Conditional" ChildrenAsTriggers="true">
       <ContentTemplate>
             <div class="row">
                <div class="col-md-12 d-flex justify-content-center">
                     <div class="alert alert-danger" id="banmsg" runat="server" clientidmode="Static"><b>Error!</b> >Debe ingresar información......</div>
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
                    
       <div class="form-row">
          <div class="form-group col-md-12">
          <h3 id="LabelTotal" runat="server">DETALLE DE DESCUENTOS</h3>
              <asp:Repeater ID="tablePagination" runat="server"  onitemcommand="tablePagination_ItemCommand">
                       <HeaderTemplate>
                       <table cellpadding="0" cellspacing="0" border="0" class="table table-bordered invoice" id="hidden-table-info-tarja">
                           <thead>
                          <tr>
                            
                            <th class="center hidden-phone">#</th>
                            <th class="center hidden-phone">LÍNEA NAVIERA</th>
                            <th class="center hidden-phone">% DESCUENTO</th>
                            <th class="center hidden-phone">QUITAR</th>
                           
                          </tr>
                        </thead>
                        <tbody>
                        </HeaderTemplate>
                        <ItemTemplate>
                           <tr class="gradeC"> 
                                <td class="center hidden-phone"><asp:Label Text='<%#Eval("DESC_FILA")%>' ID="DESC_FILA" runat="server"  /></td>
                                <td class="center hidden-phone"><asp:Label Text='<%#Eval("DESC_LIN_DESCRIP")%>' ID="DESC_LIN_DESCRIP" runat="server"  /></td>
                                <td class="center hidden-phone"><asp:Label Text='<%#Eval("DESC_PORCENTAJE")%>' ID="DESC_PORCENTAJE" runat="server"  /></td>
                              <td class="center hidden-phone">  
                                <asp:Button ID="BtnActualizar" CommandArgument= '<%#Eval("DESC_FILA")%>' runat="server" Text="QUITAR" class="btn btn-primary" ToolTip="Quitar" CommandName="Eliminar" />
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
     </ContentTemplate>
     </asp:UpdatePanel>   
    </div>
    </div>



     <asp:UpdatePanel ID="UPBOTONES" runat="server" UpdateMode="Conditional" >  
             <ContentTemplate>
                
            
             <div class="row">
                <div class="col-md-12 d-flex justify-content-center">
                      <div class="alert alert-danger" id="banmsg_pie" runat="server" clientidmode="Static"><b>Error!</b>.</div>
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
                    
                    
                        <asp:Button ID="BtnGrabar" runat="server" class="btn btn-primary"  Text="GRABAR TRANSACCION"  OnClientClick="return confirmacion()"  OnClick="BtnGrabar_Click"/>
                   
                  
                  </div><!--btn-group-justified-->
                </div><!--showback-->
            </ContentTemplate>
             </asp:UpdatePanel>   
   

    
 
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
   

    function confirmacion()
    {
        var mensaje;
        var opcion = confirm("Estimado usuario, está seguro que desea grabar la transación. ?");
        if (opcion == true)
        {
       
            return true;
        } else
        {
            //loader();
	         return false;
        }

       
    }

    function mostrarloader(Valor)
    {

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


    function ocultarloader(Valor)
    {
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


  
  </script>


     

      <script type="text/javascript">
              $(document).ready(function () {
                  $('.datetimepicker').datetimepicker().datetimepicker({ lang: 'es', timepicker: true, step: 30, format: 'm/d/Y H:i' });
              });
      </script>

</asp:Content>