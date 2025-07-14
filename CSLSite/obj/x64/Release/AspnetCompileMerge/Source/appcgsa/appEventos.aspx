<%@ Page Title="Eventos" Language="C#" MasterPageFile="~/site.Master" AutoEventWireup="true" CodeBehind="appEventos.aspx.cs" Inherits="CSLSite.appEventos" %>
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


 <%--<link href="../lib/advanced-datatable/css/demo_page.css" rel="stylesheet" />
  <link href="../lib/advanced-datatable/css/demo_table.css" rel="stylesheet" />
  <link rel="stylesheet" href="../lib/advanced-datatable/css/DT_bootstrap2.css" />--%>


 <link href="../css/calendario_ajax.css" rel="stylesheet"/>


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

      $('#hidden-table-info thead tr').each(function() {

      });

      $('#hidden-table-info tbody tr').each(function() {
       
      });

      /*
       * Initialse DataTables, with no sorting on the 'details' column
       */
      var oTable = $('#hidden-table-info').dataTable({
        "aoColumnDefs": [{
          "bSortable": false,
          "aTargets": [0]
        }],
        "aaSorting": [
          [1, 'asc']
        ]
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

     <div class="mt-4">         
        <nav class="mt-4" aria-label="breadcrumb">
          <ol class="breadcrumb">
            <li class="breadcrumb-item" id="opcion_principal" runat="server"><a href="#">App CGSA</a></li>
            <li class="breadcrumb-item active" aria-current="page" id="sub_opcion" runat="server">MANTENIMIENTO DE EVENTOS</li>
          </ol>
        </nav>
      </div>

 <div class="dashboard-container p-4" id="cuerpo" runat="server">
        <div class="form-title">
           DATOS DEL EVENTO
     </div>

       <div class="content-panel">

       <asp:UpdatePanel ID="UPCARGA" runat="server"  UpdateMode="Conditional" ChildrenAsTriggers="true">
        <ContentTemplate>
        <div class="form-row">
                <div class="form-group col-md-8">
                        <label for="inputZip">EVENTO<span style="color: #FF0000; font-weight: bold;">&nbsp; *</span></label>
                        <asp:HiddenField ID="IdEvento" runat="server" ClientIDMode="Static"/> 

                        <asp:TextBox ID="TXTMRN" runat="server" class="form-control" MaxLength="150"  onkeypress="return soloLetras(event,'abcdefghijklmnopqrstuvwxyz0123456789-_')"  
                                            placeholder="Evento"></asp:TextBox>
                </div>
                  
		    </div>
			    <br/>
            <div class="row">
                <div class="col-md-12 d-flex justify-content-center">
                        <div class="alert alert-danger" id="banmsg" runat="server" clientidmode="Static"><b>Error!</b> >Debe ingresar el evento......</div>
                </div>
            </div>
						
        </ContentTemplate>
            <Triggers>
            <asp:AsyncPostBackTrigger ControlID="BtnBuscar" />
        </Triggers>
        </asp:UpdatePanel>
        
     <div class="form-row">   
          <div class="form-group col-md-10">
                 <label for="inputZip">&nbsp;</label>
                 <div class="d-flex">
                          
                         
               </div>
          </div>
    </div>
               
	 <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" >  
             <ContentTemplate>

                  <div class="row">
                    <div class="col-md-12 d-flex justify-content-center">
                             <img alt="loading.." src="../lib/file-uploader/img/loading.gif" height="32px" width="32px"  id="ImgCarga" class="nover"   />
                    </div>
                </div>

                 <div class="row">
                <div class="col-md-12 d-flex justify-content-center">
                     <asp:Button ID="BtnNuevo" runat="server" class="btn btn-primary"   Text="Nuevo"   OnClick="BtnNuevo_Click" />     &nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:Button ID="BtnBuscar" runat="server" class="btn btn-primary"   Text="Grabar"  OnClientClick="return mostrarloader('1')" OnClick="BtnBuscar_Click" />   
                    
                </div><!--btn-group-justified-->
                </div><!--showback-->
                 
            </ContentTemplate>
      </asp:UpdatePanel>   
           
  
        
        
    <h3>DETALLE DE EVENTOS</h3>
         
     <asp:UpdatePanel ID="UPDETALLE" runat="server"  >  
       <ContentTemplate>

               <div class="form-row">
                  <div class="form-group col-md-12">
                       <asp:Repeater ID="tablePagination" runat="server"  onitemcommand="tablePagination_ItemCommand" onitemdatabound="tablePagination_ItemDataBound" >
                       <HeaderTemplate>
                       <table cellpadding="0" cellspacing="0" border="0" class="table table-bordered invoice" id="hidden-table-info">
                           <thead>
                           <tr >
                            
                            <th class="center hidden-phone">Id</th>
                            <th class="center hidden-phone">Evento</th>
                            <th class="center hidden-phone">Creado Por</th>
                            <th class="center hidden-phone">Fecha Creación</th>
                            <th class="center hidden-phone">Modificado Por</th>
                            <th class="center hidden-phone">Fecha Modificación</th>
                            <th class="center hidden-phone">Actualizar</th>
                            <th class="center hidden-phone">Eliminar</th>
                            
                          </tr>
                        </thead>
                        <tbody>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr class="gradeC">
                                
 
                                <td class="center hidden-phone"><asp:Label Text='<%#Eval("Id")%>' ID="LblId" runat="server"  /></td>
                                <td class="center hidden-phone"><asp:Label Text='<%#Eval("Name")%>' ID="LblName" runat="server"  /></td>
                                <td class="center hidden-phone"><asp:Label Text='<%#Eval("Create_user")%>' ID="LblCreado" runat="server"  /></td>
                                <td class="center hidden-phone"><asp:Label Text='<%#DataBinder.Eval(Container.DataItem, "Create_date", "{0:yyyy/MM/dd}")%>' ID="LblFechaCreado" runat="server"  /></td>
                                
                                <td class="center hidden-phone"><asp:Label Text='<%#Eval("Modifie_user")%>' ID="LblModificado" runat="server"  /></td><%--D_TURNO--%>
                                <td class="center hidden-phone"><asp:Label Text='<%#DataBinder.Eval(Container.DataItem, "Modifie_date", "{0:yyyy/MM/dd}")%>' ID="LblFechaModifica" runat="server"  /></td>
                                 <td class="center hidden-phone">  
                                <asp:Button ID="BtnActualizar" CommandArgument= '<%#Eval("Id")%>' runat="server" Text="Actualizar" class="btn btn-primary"  ToolTip="Actualizar Datos" CommandName="Actualizar" />
                           </td> 
                                <td class="center hidden-phone">
                                    <asp:Button ID="BtnEvento" CommandArgument= '<%#Eval("Id")%>' runat="server" Text="Eliminar" 
                                        OnClientClick="return confirm('Está seguro de inactivar el evento?');"
                                      
                                        class="btn btn-primary" ToolTip="Inactivar un evento" CommandName="Eliminar"
                                          />

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
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="BtnBuscar" />
                </Triggers>
            </asp:UpdatePanel>


            
   

       </div> <!--content-panel-->
	    
    
   </div>
 

  <script type="text/javascript" src="../lib/datatables/datatables.min.js"></script>
  <script type="text/javascript" src="../lib/advanced-datatable/js/DT_bootstrap.js"></script>

   <script type="text/javascript" src="../lib/common-scripts.js"></script>
 
   <script type="text/javascript" src="../lib/pages.js" ></script>

   <script type="text/javascript" src="../lib/advanced-form-components.js"></script>

 <script type="text/javascript" src='../lib/autocompletar/bootstrap3-typeahead.min.js'></script>



<script type="text/javascript">

 
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

   <%-- function AsignaTransporte(Valor) {
        $("#" + "<%= Txtempresa.ClientID %>").val(Valor);
      
    }

     function AsignaChofer(Valor) {
        $("#" + "<%= TxtChofer.ClientID %>").val(Valor);
        
    }

    function AsignaPlaca(Valor) {
        $("#" + "<%= TxtPlaca.ClientID %>").val(Valor);
        
    }--%>
</script>



</asp:Content>