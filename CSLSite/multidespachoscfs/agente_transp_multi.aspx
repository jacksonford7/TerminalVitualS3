<%@ Page Title="Paquetes" Language="C#" MasterPageFile="~/site.Master" AutoEventWireup="true" CodeBehind="agente_transp_multi.aspx.cs" Inherits="CSLSite.agente_transp_multi" %>
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
            <li class="breadcrumb-item" id="opcion_principal" runat="server"><a href="#">MULTIDESPACHO</a></li>
            <li class="breadcrumb-item active" aria-current="page" id="sub_opcion" runat="server">AGREGAR EMPRESA TRANSPORTE/FACTURACIÓN</li>
          </ol>
        </nav>
      </div>

 <div class="dashboard-container p-4" id="cuerpo" runat="server">
        <div class="form-title">
            DATOS DEL USUARIO
        </div>
         <div class="form-row">
                <div class="form-group col-md-6"> 
                    <label for="inputAddress">ESTIMADO CLIENTE:<span style="color: #FF0000; font-weight: bold;"></span></label>
                       <asp:TextBox ID="Txtcliente" runat="server" class="form-control" 
                                    placeholder=""  Font-Bold="true" disabled ></asp:TextBox>
                </div>
                <div class="form-group col-md-2">
                    <label for="inputAddress">RUC:<span style="color: #FF0000; font-weight: bold;"></span></label>
                
						            <asp:TextBox ID="Txtruc" runat="server" class="form-control" 
                                    placeholder=""  Font-Bold="true" disabled></asp:TextBox>
                </div>
                <div class="form-group col-md-4">
                    <label for="inputAddress">EMPRESA:<span style="color: #FF0000; font-weight: bold;"></span></label>
                      <asp:TextBox ID="Txtempresa" runat="server" class="form-control"  
                                    placeholder=""  Font-Bold="true" disabled></asp:TextBox>
                </div>
        </div>
       <div class="content-panel">

      <div class="form-title">
            DATOS DE LA EMPRESA DE TRANSPORTE
       </div>
       <div class="form-row"> 
           <div class="form-group col-md-12">
                        <label for="inputZip">TRANSPORTE:<span style="color: #FF0000; font-weight: bold;"> &nbsp;*</span></label>
                        
                       <asp:TextBox ID="TxtEmpresaTransporte"  runat="server" class="form-control"  autocomplete="off" onkeypress="return soloLetras(event,'abcdefghijklmnñopqrstuvwxyz0123456789_')"></asp:TextBox>                    
                        <asp:HiddenField ID="IdTxtempresa" runat="server" ClientIDMode="Static"/> 

                </div>
       </div>

       <asp:UpdatePanel ID="UPCARGA" runat="server"  UpdateMode="Conditional" ChildrenAsTriggers="true">
        <ContentTemplate>
       
            <div class="row">
                <div class="col-md-12 d-flex justify-content-center">
                        <div class="alert alert-danger" id="banmsg" runat="server" clientidmode="Static"><b>Error!</b> >......</div>
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
                     <asp:Button ID="BtnNuevo" runat="server" class="btn btn-primary"   Text="Nuevo"   OnClick="BtnNuevo_Click" OnClientClick="clearTextBox();" />     &nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:Button ID="BtnGrabar" runat="server" class="btn btn-primary"   Text="Grabar"  OnClientClick="return mostrarloader('1')" OnClick="BtnGrabar_Click" />   
                    
                </div><!--btn-group-justified-->
                </div><!--showback-->
                 
            </ContentTemplate>
      </asp:UpdatePanel>   
           
  
        
        
    <h3>DETALLE DE EMPRESA TRANSPORTE</h3>
         
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
                            <th class="center hidden-phone">RUC AGENTE</th>
                            <th class="center hidden-phone">AGENTE</th>
                            <th class="center hidden-phone">RUC TRANSP.</th>
                            <th class="center hidden-phone">TRANSPORTISTA</th>
                            <th class="center hidden-phone">USUARIO</th>
                            <th class="center hidden-phone">FECHA CREA</th>
                            <th class="center hidden-phone">Eliminar</th>
                            
                          </tr>
                        </thead>
                        <tbody>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr class="gradeC">
                                
 
                                <td class="center hidden-phone"><asp:Label Text='<%#Eval("ID")%>' ID="LblId" runat="server"  /></td>
                                <td class="center hidden-phone"><asp:Label Text='<%#Eval("RUC_AGENTE")%>' ID="LblName" runat="server"  /></td>
                                <td class="center hidden-phone"><asp:Label Text='<%#Eval("DESC_AGENTE")%>' ID="LblEventoN4" runat="server"  /></td>
                                <td class="center hidden-phone"><asp:Label Text='<%#Eval("RUC_TRANSPORTE")%>' ID="LblCreado" runat="server"  /></td>
                                <td class="center hidden-phone"><asp:Label Text='<%#Eval("DESC_TRANSPORTE")%>' ID="Label1" runat="server"  /></td>
                                <td class="center hidden-phone"><asp:Label Text='<%#Eval("USUARIO_ING")%>' ID="Label2" runat="server"  /></td>
                                <td class="center hidden-phone"><asp:Label Text='<%#DataBinder.Eval(Container.DataItem, "FECHA_ING", "{0:yyyy/MM/dd}")%>' ID="LblFechaCreado" runat="server"  /></td>
                                
                                
                                <td class="center hidden-phone">
                                    <asp:Button ID="BtnEvento" CommandArgument= '<%#Eval("Id")%>' runat="server" Text="Eliminar" 
                                        OnClientClick="return confirm('Está seguro de inactivar el Transportista?');"
                                      
                                        class="btn btn-primary" ToolTip="Inactivar Empresa de Transporte" CommandName="Eliminar"
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

      function clearTextBox()
      {
           this.document.getElementById('<%= TxtEmpresaTransporte.ClientID %>').value = "";
    
    }
 
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

             this.document.getElementById('<%= TxtEmpresaTransporte.ClientID %>').value = "";
        }
        catch (e) {
                alert('Lamentamos comunicarle que ha ocurrido el siguiente problema:\n' + e.Message);
            }
    }

    


</script>


 <script type="text/javascript">

     $(function () {
        $('[id*=TxtEmpresaTransporte]').typeahead({
            hint: true,
            highlight: true,
            minLength: 5,
            source: function (request, response) {
                $.ajax({
                    url: '<%=ResolveUrl("agente_transp_multi.aspx/GetEmpresas") %>',
                    data: "{ 'prefix': '" + request + "'}",
                    dataType: "json",
                    type: "POST",
                    maxJsonLength: 1,
                    contentType: "application/json; charset=utf-8",
                    success: function (data) {
                        items = [];
                        map = {};
                        $.each(data.d, function (i, item) {
                            var id = item.split('+')[0];
                            var name = item.split('+')[1];
                            map[name] = { id: id, name: name };
                            items.push(name);
                        });
                        response(items);
                        $(".dropdown-menu").css("height", "auto");
                    },
                    error: function (response) {
                        alert(response.responseText);
                    },
                    failure: function (response) {
                        alert(response.responseText);
                    }
                });
            },
            updater: function (item) {
                $('[id*=IdTxtempresa]').val(map[item].id);
               
                return item;
            }
        });
     });

</script>



</asp:Content>