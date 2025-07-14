<%@ Page Title="" Language="C#" MasterPageFile="~/site.Master" AutoEventWireup="true" CodeBehind="p2d_excluir_pase.aspx.cs" Inherits="CSLSite.p2d_excluir_pase" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="placehead" runat="server">

   <link href="../img/favicon2.png" rel="icon"/>
  <link href="../img/icono.png" rel="apple-touch-icon"/>
 <!-- Bootstrap core CSS -->
  <link href="../css/bootstrap.min.css" rel="stylesheet"/>
  <link href="../css/dashboard.css" rel="stylesheet"/>
  <link href="../css/icons.css" rel="stylesheet"/>
  <link href="../css/style.css" rel="stylesheet"/>

   <link href="https://fonts.googleapis.com/css2?family=Roboto:wght@300;400;500;700&display=swap" rel="stylesheet"/>

  <!--external css-->
  <link href="../shared/estilo/jquery.datetimepicker.css" rel="stylesheet" type="text/css" />
  <script src="../Scripts/jquery.datetimepicker.js" type="text/javascript"></script>


 <link href="../lib/advanced-datatable/css/demo_page.css" rel="stylesheet" />
  <link href="../lib/advanced-datatable/css/demo_table.css" rel="stylesheet" />
  <link rel="stylesheet" href="../lib/advanced-datatable/css/DT_bootstrap2.css" />

 

       
 
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
        $(document).ready(function () {
            $('#<%= tablePagination.ClientID %>').dataTable();
        });
       
    </script>

 
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="placebody" runat="server" >

   <asp:ToolkitScriptManager ID="tkscata" runat="server"></asp:ToolkitScriptManager>

  <div id="div_BrowserWindowName" style="visibility:hidden;">
    <asp:HiddenField ID="hf_BrowserWindowName" runat="server" />
     
  </div>
    <asp:HiddenField ID="manualHide" runat="server" />
   
    <div class="mt-4">         
        <nav class="mt-4" aria-label="breadcrumb">
          <ol class="breadcrumb">
            <li class="breadcrumb-item" id="opcion_principal" runat="server"><a href="#">P2D</a></li>
            <li class="breadcrumb-item active" aria-current="page" id="sub_opcion" runat="server">ORDENES/PASE PUERTA</li>
          </ol>
        </nav>
      </div>
<div class="dashboard-container p-4" id="cuerpo" runat="server">
    <div class="form-title">
           ORDEN LIFTIF
     </div>
       
      <asp:UpdatePanel ID="UPCARGA" runat="server"  UpdateMode="Conditional" ChildrenAsTriggers="true">
       <ContentTemplate>
        <div class="form-row"> 
           <div class="form-group col-md-4">
               <label for="inputEmail4"># DE ORDEN:</label>
               <div class="d-flex">
                    <asp:TextBox ID="TXTORDEN" runat="server" class="form-control" MaxLength="16"  onkeypress="return soloLetras(event,'0123456789')"  
                                     placeholder="ORDEN"    type="text" >
                      </asp:TextBox>
                        &nbsp;
                        <asp:Button ID="BtnBuscarOrden" runat="server" class="btn btn-primary"  Text="BUSCAR"  OnClick="BtnBuscarOrden_Click" />                             
                        &nbsp;
               </div>
            </div>
       </div>

        </ContentTemplate>
            <Triggers>
            <asp:AsyncPostBackTrigger ControlID="BtnBuscarOrden" />
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
            <asp:AsyncPostBackTrigger ControlID="BtnBuscarOrden" />
        </Triggers>
        </asp:UpdatePanel>

  
    
     <div class="form-row">  
    <div class="form-group col-md-12">

     <asp:UpdatePanel ID="UPDETALLE" runat="server" UpdateMode="Conditional" >  
       <ContentTemplate>
         
            
       <div class="form-row">
                 <div class="form-group col-md-12">
            <h3 id="LabelTotal" runat="server">DETALLE DE PASE DE PUERTA</h3>
                <asp:Repeater ID="tablePagination" runat="server"  onitemcommand="tablePagination_ItemCommand" onitemdatabound="tablePagination_ItemDataBound">
                       <HeaderTemplate>
                       <table cellpadding="0" cellspacing="0" border="0" class="table table-bordered invoice" id="hidden-table-info-tarja">
                           <thead>
                          <tr>
                            
                            <th class="center hidden-phone"># ORDEN</th>
                             <th class="center hidden-phone"># CARGA</th>
                            <th class="center hidden-phone"># PASE</th>
                            <th class="center hidden-phone">ESTADO</th>
                            <th class="center hidden-phone">FECHA TURNO</th>
                            <th class="center hidden-phone">EMPRESA TRANSPORTE</th>
                            <th class="center hidden-phone">CLIENTE</th>
                            <th class="center hidden-phone">PLACA</th>
                            <th class="center hidden-phone">QUITAR</th>
                           
                          </tr>
                        </thead>
                        <tbody>
                        </HeaderTemplate>
                        <ItemTemplate>
                           <tr class="gradeC"> 
                                <td class="center hidden-phone"><asp:Label Text='<%#Eval("ORDEN")%>' ID="LblOrden" runat="server"  /></td>
                               <td class="center hidden-phone"><asp:Label Text='<%#Eval("CARGA")%>' ID="LblCarga" runat="server"  /></td>
                                <td class="center hidden-phone"><asp:Label Text='<%#Eval("ID_PASE")%>' ID="LblPase" runat="server"  /></td>
                                <td class="center hidden-phone"><asp:Label Text='<%#Eval("ESTADO_PASE")%>' ID="LblEstado" runat="server"  /></td>
                                <td class="center hidden-phone"><asp:Label Text='<%#Eval("FECHA_TURNO")%>' ID="LblFecha" runat="server"  /> </td>
                                <td class="center hidden-phone"><asp:Label Text='<%#Eval("TRANSPORTISTA_DESC")%>' ID="LblEmpresa" runat="server"  />  </td> 
                               <td class="center hidden-phone"><asp:Label Text='<%#Eval("CLIENTE")%>' ID="LblCliente" runat="server"  />  </td> 
                               <td class="center hidden-phone"><asp:Label Text='<%#Eval("PLACA")%>' ID="LblPlaca" runat="server"  />  </td> 
                              <td class="center hidden-phone">  
                                <asp:Button ID="BtnActualizar"
                                       OnClientClick="return confirm('Esta seguro que desea quitar el registro?');" 
                                    CommandArgument= '<%#Eval("ID_PASE")%>' runat="server" Text="QUITAR" class="btn btn-primary" ToolTip="Quitar" CommandName="Eliminar" />
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
                                   
                        <asp:Button ID="BtnNuevo" runat="server" class="btn btn-outline-primary mr-4"  Text="NUEVA CONSULTA"  OnClick="BtnNuevo_Click"  />
                    
                    
                  </div><!--btn-group-justified-->
                </div><!--showback-->
            </ContentTemplate>
             </asp:UpdatePanel>   
   

    
 
</div>

   <script type="text/javascript" src="../lib/common-scripts.js"></script>
 
   <script type="text/javascript" src="../lib/pages.js" ></script>

   <script type="text/javascript" src="../lib/advanced-form-components.js"></script>

         <script type="text/javascript" src="../lib/bootstrap/js/bootstrap.min.js"></script>

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

    function mostrarloader(Valor) {

        try {
            
                document.getElementById("ImgCargaDet").className='ver';
           
                
            } catch (e) {
                alert('Lamentamos comunicarle que ha ocurrido el siguiente problema:\n' + e.Message);
            }
        }


    function ocultarloader(Valor) {
        try {

           
                document.getElementById("ImgCargaDet").className='nover';
            

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