<%@ Page Title="Configuración de Bodegas de VBS Banano" Language="C#" MasterPageFile="~/site.Master" AutoEventWireup="true" CodeBehind="VBS_BAN_Bodega.aspx.cs" Inherits="CSLSite.VBS_BAN_Bodega" %>
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
 
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></asp:ToolkitScriptManager>
    
  <div id="div_BrowserWindowName" style="visibility:hidden;">
    <asp:HiddenField ID="HiddenField1" runat="server" />
     
  </div>

    <div class="mt-4">         
        <nav class="mt-4" aria-label="breadcrumb">
          <ol class="breadcrumb">
            <li class="breadcrumb-item" id="Li1" runat="server"><a href="#">VBS Banano</a></li>
            <li class="breadcrumb-item active" aria-current="page" id="Li2" runat="server">BODEGAS</li>
          </ol>
        </nav>
      </div>

    <div class="dashboard-container p-4" id="Div1" runat="server">
        
        <div class="form-title">
              DATOS DE LA BODEGA
        </div>

        <asp:UpdatePanel ID="UPCARGA" runat="server"  UpdateMode="Conditional" ChildrenAsTriggers="true">
            <ContentTemplate>
                 <div class="form-row">
         
                     <div class="form-group col-md-2"> 
                        <label for="inputAddress">Código :<span style="color: #FF0000; font-weight: bold;"></span></label>
                          <div class="d-flex">
                            <asp:TextBox ID="txtCodigo" runat="server" MaxLength="250" 
                                    onkeypress="return soloLetras(event,'abcdefghijklmnñopqrstuvwxyz1234567890')" 
                    
                                    placeholder="CÓDIGO DE BODEGA"
                                    class="form-control"
                                    style="text-transform:uppercase;"
                                    ClientIDMode="Static">
                            </asp:TextBox>
                            <span id="valcont1" class="validacion"> * </span>      
                        </div>
                     </div>

                     <div class="form-group   col-md-3"> 
                        <label for="inputAddress">Nombre:<span style="color: #FF0000; font-weight: bold;"></span></label>

                        <div class="d-flex">
                            <asp:TextBox ID="txtNombre" runat="server" MaxLength="150" 
                                onkeypress="return soloLetras(event,'abcdefghijklmnñopqrstuvwxyz1234567890')" 
                    
                                placeholder="Nombre de la bodega"
                                class="form-control"
                                style="text-transform:uppercase;"
                                ClientIDMode="Static">
                            </asp:TextBox>
                            <span id="valcont" class="validacion"> * </span>
                        </div>
                    </div>

                     <div class="form-group col-md-3"> 
                                <label for="inputAddress">Tipo Bodega :<span style="color: #FF0000; font-weight: bold;"></span></label>
                                <div class="d-flex">
                                    <asp:DropDownList ID="cmbTipobodega" class="form-control"  runat="server" Font-Size="Medium" AutoPostBack="true" Font-Bold="true" >
                                    </asp:DropDownList>
                                    <a class="tooltip" ><span class="classic" >Nombre de tipo de bodega</span><img alt='' src='../shared/imgs/info.gif' class='datainfo'/></a>
                                </div>
                            </div>
               
                     <div class="form-group col-md-2">
                        <span class="help-block">&nbsp;</span>

                        <label for="inputZip">Cantidad</label>
                        <div class="d-flex"> 
                            <asp:TextBox ID="txtBox" runat="server" MaxLength="9" class="form-control" onkeypress="return soloLetras(event,'1234567890',true)"  placeholder="CANTIDAD"></asp:TextBox>
                        </div>
		            </div>

                    <div class="form-group col-md-2"> 
                        <label for="inputAddress">Estado :<span style="color: #FF0000; font-weight: bold;"></span></label>
                          <div class="d-flex">
                                <asp:DropDownList ID="cmbEstado" class="form-control" runat="server" Font-Size="Medium"  Font-Bold="true" >
                                    <asp:ListItem Value="True">ACTIVO</asp:ListItem>
                                    <asp:ListItem Value="False">INACTIVO</asp:ListItem>
                                </asp:DropDownList>
                                <a class="tooltip" ><span class="classic" >Maniobras asociado al producto</span><img alt='' src='../shared/imgs/info.gif' class='datainfo'/></a>
                          </div>
                     </div>
             
                 </div>

                <div></div>

                 <div class="row">
                    <div class="col-md-12 d-flex justify-content-center">
                        <asp:Button ID="btnLimpiar" runat="server" class="btn btn-primary"  Text="Limpiar" OnClick="btnLimpiar_Click"  />
                         &nbsp;
                        <asp:Button ID="BtnAdd" runat="server" class="btn btn-primary"  Text="Grabar"  OnClientClick="return mostrarloader('1')" OnClick="BtnAdd_Click" />
                        <img alt="loading.." src="../lib/file-uploader/img/loading.gif" height="32px" width="32px"  id="ImgCarga" class="nover"   />
                        <span id="imagen"></span>
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
                <asp:AsyncPostBackTrigger ControlID="BtnAdd" />
            </Triggers>
        </asp:UpdatePanel>

         <asp:UpdatePanel ID="UpdatePanel1" runat="server"  >  
           <ContentTemplate>
        
               <section class="wrapper2">
                <h4 class="mb">LISTA DE BODEGAS</h4>
                 <div class="row mb">
                 <div class="col-lg-12">
                  <div class="content-panel">
                      <div class="adv-table">
                           <asp:Repeater ID="tablePagination" runat="server"  onitemcommand="tablePagination_ItemCommand">
                           <HeaderTemplate>
                           <table cellpadding="0" cellspacing="0" border="0" class="table table-bordered" id="hidden-table-info">
                               <thead>
                              <tr style="background-color:#F4F4F4">
                                <th class="center hidden-phone">ID</th>
                                <th class="center hidden-phone">CÓDIGO</th>
                                <th class="center hidden-phone">NOMBRE</th>
                                <th class="center hidden-phone">TIPO</th>
                                <th class="center hidden-phone">CAPACIDAD</th>
                                  <th class="center hidden-phone">ESTADO</th>
                                <%--<th class="center hidden-phone">USUARIO</th>--%>
                                <th class="center hidden-phone"></th>
                                <th class="center hidden-phone"></th>
                              </tr>
                            </thead>
                            <tbody>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <tr class="gradeC">
                                    <td class="center hidden-phone"><asp:Label Text='<%#Eval("id")%>' ID="LblAsignacion" runat="server"  /></td>
                                    <td class="center hidden-phone"><%#Eval("codigo")%></td>
                                    <td class="center hidden-phone"><%#Eval("nombre")%></td>
                                    <td class="center hidden-phone"><%#Eval("tipo")%></td>
                                    <td class="center hidden-phone"><%#Eval("capacidad")%></td>
                                    <td class="center hidden-phone"> <asp:CheckBox ID="CHKPRO" runat="server" Checked='<%# Eval("estado") %>'  CssClass="center hidden-phone" Enabled="false"/></td>
                                    <td class="center hidden-phone">  
                                        <asp:Button ID="btnModificar"  CommandArgument= '<%#Eval("id")%>' runat="server" Text="Modificar" class="btn btn-primary" CommandName="Modificar"/> 
                                    </td> 
                                    <td class="center hidden-phone"> 
                                        <asp:Button runat="server" ID="btnBloque" CommandName="Bloques" Text="Bloques"   CommandArgument= '<%#Eval("id")%>' class="btn btn-primary"  data-toggle="modal" data-target="#myModal1"  />
                                    </td> 
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
                        <asp:AsyncPostBackTrigger ControlID="BtnAdd" />
                    </Triggers>
                </asp:UpdatePanel>

    </div>


    <div id="myModal1" class="modal fade" tabindex="-1" role="dialog">

            <div class="modal-dialog" role="document" style="max-width: 1000px"> <!-- Este tag style controla el ancho del modal -->
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title">DETALLE DE BLOQUES POR BODEGA</h5>
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                        </button>
                    </div>
                    <div class="modal-body">

                        <asp:UpdatePanel ID="UPEDIT" runat="server" UpdateMode="Conditional" >  
                            <ContentTemplate>
                                <asp:Panel ID="Panel2" runat="server">         
                                        <div id="div_Codigos" style="visibility:hidden;">
                                            <asp:HiddenField ID="hdf_CodigoBodega" runat="server" />
                                        </div>

                                    <div class="form-row">
                                            
                                        <div class="form-group col-md-3">
                                            <span class="help-block">&nbsp;</span>

                                            <label for="inputZip">Bodega</label>
                                            <div class="d-flex"> 
                                                <asp:DropDownList  ID="cmbBloqueBodega" disabled class="form-control"  runat="server" Font-Size="Medium" Font-Bold="true">
                                                </asp:DropDownList>
                                            </div>
		                                </div>
             
                                        <div class="form-group col-md-6"> 
                                            <label for="inputAddress">Nombre<span style="color: #FF0000; font-weight: bold;"></span></label>
                                            <div class="d-flex"> 
                                                <asp:TextBox class="form-control"  ID="txtBloqueNombre" runat="server" MaxLength="100" onkeypress="return soloLetras(event,'abcdefghijklmnopqrstuvwxyz01234567890/')"></asp:TextBox>
                                            </div>
                                        </div>

                                        <div class="form-group col-md-2"> 
                                            <label for="inputAddress">Estado :<span style="color: #FF0000; font-weight: bold;"></span></label>
                                                <div class="d-flex">
                                                    <asp:DropDownList ID="cmbBloqueEstado" class="form-control" runat="server" Font-Size="Medium"  Font-Bold="true" >
                                                        <asp:ListItem Value="True">ACTIVO</asp:ListItem>
                                                        <asp:ListItem Value="False">INACTIVO</asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
             
                                    </div>

                                    <div></div>

                                    <div class="row">
                                        <div class="col-md-12 d-flex justify-content-center">
                                            <asp:Button ID="btnBloqueLimpiar" runat="server" class="btn btn-primary"  Text="Limpiar" OnClick="btnBloqueLimpiar_Click"  />
                                                &nbsp;
                                            <asp:Button ID="btnBloqueAdd" runat="server" class="btn btn-primary"  Text="Grabar"  OnClientClick="return mostrarloader('2')" OnClick="btnBloqueAdd_Click" />
                                            <img alt="loading.." src="../lib/file-uploader/img/loading.gif" height="32px" width="32px"  id="ImgCargas" class="nover"   />
                                            <span id="imagen1"></span>
                                        </div>
                                    </div>

                                    <br/>
                                    <div class="row">
                                        <div class="col-md-12 d-flex justify-content-center">
                                            <div class="alert alert-warning" id="banmsgDet" runat="server" clientidmode="Static"><b>Error!</b> >......</div>
                                        </div>
                                    </div>
                                            
                                </asp:Panel>

                            </ContentTemplate>   
                        </asp:UpdatePanel>

                        <asp:UpdatePanel ID="UPEditDet" runat="server" UpdateMode="Conditional" >  
                            <ContentTemplate>
        
                                <section class="wrapper2">
                                    <h4 class="mb">LISTA DE BLOQUES</h4>
                                    <div class="row mb">
                                        <div class="col-lg-12">
                                            <div class="content-panel">
                                                <div class="adv-table">
                                                    <asp:Repeater ID="tablePginacionDetalle" runat="server"  onitemcommand="tablePginacionDetalle_ItemCommand">
                                                    <HeaderTemplate>
                                                    <table cellpadding="0" cellspacing="0" border="0" class="table table-bordered" id="hidden-table-info">
                                                        <thead>
                                                        <tr style="background-color:#F4F4F4">
                                                        <th class="center hidden-phone">ID</th>
                                                        <th class="center hidden-phone">BODEGA</th>
                                                        <th class="center hidden-phone">NOMBRE</th>
                                                            <th class="center hidden-phone">ESTADO</th>
                                                        <%--<th class="center hidden-phone">USUARIO</th>--%>
                                                        <th class="center hidden-phone">ACCION</th>
                          
                                                        </tr>
                                                    </thead>
                                                    <tbody>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <tr class="gradeC">
                                                            <td class="center hidden-phone"><asp:Label Text='<%#Eval("id")%>' ID="LblAsignacion" runat="server"  /></td>
                                                            <td class="center hidden-phone"><%#Eval("bodega")%></td>
                                                            <td class="center hidden-phone"><%#Eval("nombre")%></td>
                                                            <td class="center hidden-phone"> <asp:CheckBox ID="CHKPRO" runat="server" Checked='<%# Eval("estado") %>'  CssClass="center hidden-phone" Enabled="false"/></td>
                                                            <td class="center hidden-phone">  
                                                                <asp:Button ID="btnModificar"  CommandArgument= '<%#Eval("id")%>' runat="server" Text="Modificar" class="btn btn-primary" ToolTip="Permite remover una carga" CommandName="Modificar"/>
                                                            </td> 
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
                                        <asp:AsyncPostBackTrigger ControlID="BtnAdd" />
                                    </Triggers>
                                </asp:UpdatePanel>

                     </div>
                    <div class="modal-footer d-flex justify-content-center">
                        <button type="button" class="btn btn-outline-primary " data-dismiss="modal">Salir</button>
                    </div>
                </div>
            </div>
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
                document.getElementById("ImgCargas").className = 'ver';
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
                document.getElementById("ImgCargas").className = 'nover';
            }

             } catch (e) {
                alert('Lamentamos comunicarle que ha ocurrido el siguiente problema:\n' + e.Message);
            }
    }


      function prepareObjectRuc() {
            try {
                document.getElementById("loader3").className = '';
                var vals = document.getElementById('<%=txtNombre.ClientID %>').value;
                if (vals == null || vals == undefined || vals == '') {
                    alert('¡ Escriba el nombre del producto.');
                    document.getElementById("loader3").className = 'nover';
                    document.getElementById('<%=txtNombre.ClientID %>').focus();
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

