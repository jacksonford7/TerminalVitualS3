<%@ Page Title="Asume Terceros" Language="C#" MasterPageFile="~/site.Master" AutoEventWireup="true" CodeBehind="asume_terceros_sav.aspx.cs" Inherits="CSLSite.sav.asume_terceros_sav" %>
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
            <li class="breadcrumb-item" id="opcion_principal" runat="server"><a href="#">Pagos a Terceros</a></li>
            <li class="breadcrumb-item active" aria-current="page" id="sub_opcion" runat="server">ASUMIR PAGO (SAV)</li>
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
                  <%--<span class="help-block">RUC:</span>--%>
						        <asp:TextBox ID="Txtruc" runat="server" class="form-control" 
                                placeholder=""  Font-Bold="true" disabled></asp:TextBox>
            </div>
            <div class="form-group col-md-4">
                <label for="inputAddress">EMPRESA:<span style="color: #FF0000; font-weight: bold;"></span></label>
                  <asp:TextBox ID="Txtempresa" runat="server" class="form-control"  
                                placeholder=""  Font-Bold="true" disabled></asp:TextBox>
            </div>
    </div>
    <%--<h5 class="mb">ASUMIR LA SIGUIENTE CARGA</h5> --%>
        
    <div class="form-title">
          ASUMIR LA SIGUIENTE CARGA
    </div>
    <asp:UpdatePanel ID="UPCARGA" runat="server"  UpdateMode="Conditional" ChildrenAsTriggers="true">
    <ContentTemplate>
         <div class="form-row">
          <%--   <div class="form-group col-md-4">
                 <label for="inputZip">CONTAINER<span style="color: #FF0000; font-weight: bold;">*</span></label>
                <asp:TextBox ID="TXTCONTAINER" runat="server" class="form-control" MaxLength="16"  onkeypress="return soloLetras(event,'abcdefghijklmnopqrstuvwxyz0123456789-_')"  
                                 placeholder="CONTENEDOR"></asp:TextBox>
            </div>--%>
             <div class="form-group   col-md-3"> 
                    <label for="inputAddress">Número de contenedor:<span style="color: #FF0000; font-weight: bold;"></span></label>

                    <div class="d-flex">
                        <asp:TextBox ID="TXTCONTAINER" runat="server" MaxLength="11" 
                            onkeypress="return soloLetras(event,'abcdefghijklmnñopqrstuvwxyz1234567890')" 
                            onBlur="checkDC(this,'valcont',true);"
                            placeholder="Contenedor"
                            class="form-control"
                            style="text-transform:uppercase;"
                            ClientIDMode="Static">
                        </asp:TextBox>
                        <span id="valcont" class="validacion"> * </span>
                    </div>
                </div>

             <div class="form-group   col-md-3"> 
                    <label for="inputAddress">Ruc Cliente:<span style="color: #FF0000; font-weight: bold;"></span></label>

                    <div class="d-flex">
                        <asp:TextBox ID="TXTCLIENTE_RUC" runat="server" MaxLength="100" 
                            placeholder="Ruc"
                            class="form-control"
                            style="text-transform:uppercase;"
                            ClientIDMode="Static">
                        </asp:TextBox>
                        <img alt="loading.." src="../shared/imgs/loader.gif" id="loader3" class="nover"  />
                        <asp:Button ID="btnBuscarCliente" runat="server"  CssClass="btn btn-primary"
                            Text="Buscar" OnClientClick="return prepareObjectRuc()"
                            ToolTip="Busca al Cliente por el RUC." OnClick="btnBuscar_Click"/>
                        
                    </div>
                </div>

             <div class="form-group   col-md-6"> 
                    <label for="inputAddress">Cliente:<span style="color: #FF0000; font-weight: bold;"></span></label>

                    <div class="d-flex">
                        <asp:TextBox ID="TXTCLIENTE_ASUMIDO" runat="server" MaxLength="100" 
                            placeholder="Cliente"
                            disabled
                            class="form-control"
                            style="text-transform:uppercase;"
                            ClientIDMode="Static">
                        </asp:TextBox>
                        
                    </div>
                </div>
             
                <div class="form-group col-md-2">
                 <label for="inputZip">&nbsp;</label>
                   <div class="d-flex">
                        <asp:Button ID="BtnBuscar" runat="server" class="btn btn-primary"  Text="ASUMIR"  OnClientClick="return mostrarloader('1')" OnClick="BtnAsumir_Click" />
                        <img alt="loading.." src="../lib/file-uploader/img/loading.gif" height="32px" width="32px"  id="ImgCarga" class="nover"   />
                   </div>
                </div>
                <div class="form-group col-md-2">
                </div>
         </div>
         <br/>
          <div class="row">
            <div class="col-md-12 d-flex justify-content-center">
                 <div class="alert alert-warning" id="banmsg" runat="server" clientidmode="Static"><b>Error!</b> >......</div>
            </div>
        </div>
     </ContentTemplate>
     <Triggers>
    <asp:AsyncPostBackTrigger ControlID="BtnBuscar" />
    </Triggers>
    </asp:UpdatePanel>


     <asp:UpdatePanel ID="UPDETALLE" runat="server"  >  
       <ContentTemplate>
        
           <section class="wrapper2">
            <h4 class="mb">DETALLE DE CARGAS ASUMIDAS</h4>
             <div class="row mb">
             <div class="col-lg-12">
              <div class="content-panel">
                  <div class="adv-table">
                       <asp:Repeater ID="tablePagination" runat="server"  onitemcommand="tablePagination_ItemCommand">
                       <HeaderTemplate>
                       <table cellpadding="0" cellspacing="0" border="0" class="display table table-bordered" id="hidden-table-info">
                           <thead>
                          <tr style="background-color:#F4F4F4">
                            <th class="center hidden-phone">ID</th>
                            <th class="center hidden-phone">CONTENEDOR</th>
                            <th class="center hidden-phone">RUC</th>
                            <th class="center hidden-phone">NOMBRE</th>
                            <th class="center hidden-phone">RUC BENEFICIARIO</th>
                            <th class="center hidden-phone">BENEFICIARIO</th>
                            <th class="center hidden-phone">FECHA ASIGNACIÓN</th>
                            <th class="center hidden-phone">USUARIO</th>
                            <th class="center hidden-phone">ACCION</th>
                          
                          </tr>
                        </thead>
                        <tbody>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr class="gradeC">
                                <td class="center hidden-phone"><asp:Label Text='<%#Eval("id_asignacion")%>' ID="LblAsignacion" runat="server"  /></td>
                                <td class="center hidden-phone"><%#Eval("carga")%></td>
                                <td class="center hidden-phone"><%#Eval("ruc")%> </td>
                                <td class="center hidden-phone"><%#Eval("nombre")%></td>
                                <td class="center hidden-phone"> <span><%#Eval("ruc_asumido")%></span></td>
                                <td class="center hidden-phone"> <span><%#Eval("nombre_asumido")%></span></td>
                                <td class="center hidden-phone"><%#Eval("fecha_asignado")%></td>
                                <td class="center hidden-phone"><%#Eval("login_asigna")%></td> 
                                <td class="center hidden-phone">  <asp:Button ID="BtnAprobar"    CommandArgument= '<%#Eval("id_asignacion")%>' runat="server" Text="Remover" 
                        class="btn btn-primary" ToolTip="Permite remover una carga" CommandName="Remover"   OnClientClick="return confirm('Esta seguro de que desea eliminar la asignación?');"  /></td> 
 
                             </tr>    
                       </ItemTemplate>
                       <FooterTemplate>
                        </tbody>
                      </table>
                     </FooterTemplate>
                    </asp:Repeater>
                </div><!--adv-table-->
               <%--   </section>--%>
               </div><!--content-panel-->
               </div><!--col-lg-12-->
             </div><!--row mt-->

        </section><!--wrapper2-->
     
            </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="BtnBuscar" />
                </Triggers>
            </asp:UpdatePanel>


    </div>

 <script type="text/javascript" src="../lib/datatables/datatables.min.js"></script>
  <script type="text/javascript" src="../lib/advanced-datatable/js/DT_bootstrap.js"></script>

   <script type="text/javascript" src="../lib/common-scripts.js"></script>
 
   <script type="text/javascript" src="../lib/pages.js" ></script>

   <script type="text/javascript" src="../lib/advanced-form-components.js"></script>




<script type="text/javascript">

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


      function prepareObjectRuc() {
            try {
                document.getElementById("loader3").className = '';
                var vals = document.getElementById('<%=TXTCLIENTE_RUC.ClientID %>').value;
                if (vals == null || vals == undefined || vals == '') {
                    alert('¡ Escriba el Ruc del Cliente a buscar.');
                    document.getElementById("loader3").className = 'nover';
                    document.getElementById('<%=TXTCLIENTE_RUC.ClientID %>').focus();
                    return false;
                }
                return true;
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