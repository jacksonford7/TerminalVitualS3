<%@ Page Title="Paquetes" Language="C#" MasterPageFile="~/site.Master" AutoEventWireup="true" CodeBehind="p2d_mante_ciudad.aspx.cs" Inherits="CSLSite.p2d_mante_ciudad" %>
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
 
  <link href="https://fonts.googleapis.com/css2?family=Roboto:wght@300;400;500;700&display=swap" rel="stylesheet"/>


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
            <li class="breadcrumb-item" id="opcion_principal" runat="server"><a href="#">P2D</a></li>
            <li class="breadcrumb-item active" aria-current="page" id="sub_opcion" runat="server">MANTENIMIENTO DE CIUDADES</li>
          </ol>
        </nav>
      </div>

 <div class="dashboard-container p-4" id="cuerpo" runat="server">
        <div class="form-title">
           DATOS
     </div>

       <div class="content-panel">

       <asp:UpdatePanel ID="UPCARGA" runat="server"  UpdateMode="Conditional" ChildrenAsTriggers="true">
        <ContentTemplate>
        <div class="form-row">
                <div class="form-group col-md-12">
                        <label for="inputZip">CIUDAD<span style="color: #FF0000; font-weight: bold;"> &nbsp;*</span></label>
                        
                    <asp:HiddenField ID="IdPaquete" runat="server" ClientIDMode="Static"/> 
                        <asp:TextBox ID="TxtCiudad" runat="server" class="form-control" MaxLength="100"  onkeypress="return soloLetras(event,'abcdefghijklmnopqrstuvwxyz0123456789-_')"  
                                            placeholder="Ciudad"></asp:TextBox>
                </div>
                
               

		    </div>
			    <br/>
            <div class="row">
                <div class="col-md-12 d-flex justify-content-center">
                        <div class="alert alert-danger" id="banmsg" runat="server" clientidmode="Static"><b>Error!</b> >Debe ingresar la ciudad......</div>
                </div>
            </div>
						
        </ContentTemplate>
            <Triggers>
            <asp:AsyncPostBackTrigger ControlID="BtnGrabar" />
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
                    <asp:Button ID="BtnGrabar" runat="server" class="btn btn-primary"   Text="Grabar"  OnClientClick="return mostrarloader('1')" OnClick="BtnGrabar_Click" />   
                    
                </div><!--btn-group-justified-->
                </div><!--showback-->
                 
            </ContentTemplate>
      </asp:UpdatePanel>   
           
  
        
        
    <h3>DETALLE DE CIUDADES</h3>
         
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
                            <th class="center hidden-phone">Descripción</th>
                            <th class="center hidden-phone">Estado</th>
                            <th class="center hidden-phone">Creado Por</th>
                            <th class="center hidden-phone">Fecha Creación</th>
                         
                            <th class="center hidden-phone">Actualizar</th>
                            <th class="center hidden-phone">Eliminar</th>
                            
                          </tr>
                        </thead>
                        <tbody>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr class="gradeC">
                                
 
                                <td class="center hidden-phone"><asp:Label Text='<%#Eval("ID_CIUDAD")%>' ID="LblId" runat="server"  /></td>
                                <td class="center hidden-phone"><asp:Label Text='<%#Eval("DESC_CIUDAD")%>' ID="LblName" runat="server"  /></td>
                                <td class="center hidden-phone"><asp:Label Text='<%#Eval("ESTADO_TEXTO")%>' ID="LblEventoN4" runat="server"  /></td>
                                <td class="center hidden-phone"><asp:Label Text='<%#Eval("USUARIO_CREA")%>' ID="LblCreado" runat="server"  /></td>
                                <td class="center hidden-phone"><asp:Label Text='<%#DataBinder.Eval(Container.DataItem, "FECHA_CREA", "{0:yyyy/MM/dd}")%>' ID="LblFechaCreado" runat="server"  /></td>
                                
                              
                                 <td class="center hidden-phone">  
                                <asp:Button ID="BtnActualizar" CommandArgument= '<%#Eval("ID_CIUDAD")%>' runat="server" Text="Actualizar" class="btn btn-primary"  ToolTip="Actualizar Datos" CommandName="Actualizar" />
                           </td> 
                                <td class="center hidden-phone">
                                    <asp:Button ID="BtnEvento" CommandArgument= '<%#Eval("ID_CIUDAD")%>' runat="server" Text="Eliminar" 
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
                    <asp:AsyncPostBackTrigger ControlID="BtnGrabar" />
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



</asp:Content>