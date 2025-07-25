﻿<%@ Page Title="Configuración de Usuarios BRBK" Language="C#" MasterPageFile="~/site.Master" AutoEventWireup="true" CodeBehind="brbkUsers.aspx.cs" Inherits="CSLSite.brbk.brbkUsers" %>
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
            <li class="breadcrumb-item" id="opcion_principal" runat="server"><a href="#">Cargas Break Bulk</a></li>
            <li class="breadcrumb-item active" aria-current="page" id="sub_opcion" runat="server">USUARIOS APP</li>
          </ol>
        </nav>
      </div>

    <div class="dashboard-container p-4" id="cuerpo" runat="server">
        
    <div class="form-title">
          DATOS DE USUARIO
    </div>
    <asp:UpdatePanel ID="UPCARGA" runat="server"  UpdateMode="Conditional" ChildrenAsTriggers="true">
    <ContentTemplate>
         <div class="form-row">
            
             
             <div class="form-group   col-md-2"> 
                <label for="inputAddress">Identificación:<span style="color: #FF0000; font-weight: bold;"></span></label>

                <div class="d-flex">
                    <asp:TextBox ID="txtIdentificacion" runat="server" MaxLength="15" 
                        onkeypress="return soloLetras(event,'abcdefghijklmnñopqrstuvwxyz1234567890')" 
                    
                        placeholder="Cédula"
                        class="form-control"
                        style="text-transform:uppercase;"
                        ClientIDMode="Static">
                    </asp:TextBox>
                    <span id="valcont0" class="validacion"> * </span>
                </div>
            </div>

             <div class="form-group   col-md-4"> 
                <label for="inputAddress">Nombre:<span style="color: #FF0000; font-weight: bold;"></span></label>

                <div class="d-flex">
                    <asp:TextBox ID="txtNombre" runat="server" MaxLength="150" 
                        onkeypress="return soloLetras(event,'abcdefghijklmnñopqrstuvwxyz1234567890')" 
                    
                        placeholder="Nombre Usuario"
                        class="form-control"
                        style="text-transform:uppercase;"
                        ClientIDMode="Static">
                    </asp:TextBox>
                    <span id="valcont2" class="validacion"> * </span>
                </div>
            </div>

             <div class="form-group   col-md-2"> 
                <label for="inputAddress">Usuario:<span style="color: #FF0000; font-weight: bold;"></span></label>

                <div class="d-flex">
                    <asp:TextBox ID="txtUsers" runat="server" MaxLength="10" 
                        onkeypress="return soloLetras(event,'abcdefghijklmnñopqrstuvwxyz1234567890')" 
                    
                        placeholder="Usuario"
                        class="form-control"
                        style="text-transform:uppercase;"
                        ClientIDMode="Static">
                    </asp:TextBox>
                    <span id="valcont1" class="validacion"> * </span>
                </div>
            </div>

             <div class="form-group   col-md-2"> 
                <label for="inputAddress">Clave:<span style="color: #FF0000; font-weight: bold;"></span></label>

                <div class="d-flex">
                    <asp:TextBox ID="txtPassword" runat="server" MaxLength="100" 
                        onkeypress="return soloLetras(event,'abcdefghijklmnñopqrstuvwxyz1234567890@_.')" 
                        TextMode="Password"
                        placeholder="Clave"
                        class="form-control"
                        style="text-transform:uppercase;"
                        ClientIDMode="Static">
                    </asp:TextBox>
                    <span id="valcont11" class="validacion"> * </span>
                </div>
            </div>

             

             <div class="form-group   col-md-2"> 
                <label for="inputAddress">Telefono:<span style="color: #FF0000; font-weight: bold;"></span></label>

                <div class="d-flex">
                    <asp:TextBox ID="txtPhone" runat="server" MaxLength="150" 
                        onkeypress="return soloLetras(event,'abcdefghijklmnñopqrstuvwxyz1234567890-/')" 
                    
                        placeholder="Número Teléfono"
                        class="form-control"
                        style="text-transform:uppercase;"
                        ClientIDMode="Static">
                    </asp:TextBox>
                    <span id="valcont3" class="validacion"> * </span>
                </div>
            </div>

             <div class="form-group   col-md-3"> 
                <label for="inputAddress">Email:<span style="color: #FF0000; font-weight: bold;"></span></label>

                <div class="d-flex">
                    <asp:TextBox ID="txtEmail" runat="server" MaxLength="150" 
                        onkeypress="return soloLetras(event,'abcdefghijklmnñopqrstuvwxyz1234567890@_.-')" 
                    
                        placeholder="Correo electrónico"
                        class="form-control"
                        style="text-transform:uppercase;"
                        ClientIDMode="Static">
                    </asp:TextBox>
                    <span id="valcont4" class="validacion"> * </span>
                </div>
            </div>

            <div class="form-group col-md-3"> 
                <label for="inputAddress">Compañia :<span style="color: #FF0000; font-weight: bold;"></span></label>
                  <div class="d-flex">
                        <asp:DropDownList ID="cmbCompañia" class="form-control" runat="server" Enabled="false" Font-Size="Medium"  Font-Bold="true" ></asp:DropDownList>
                        <a class="tooltip" ><span class="classic" >Compañia asociada al usuario</span><img alt='' src='../shared/imgs/info.gif' class='datainfo'/></a>
                  </div>
             </div>

             <div class="form-group col-md-2"> 
                <label for="inputAddress">Posición :<span style="color: #FF0000; font-weight: bold;"></span></label>
                  <div class="d-flex">
                        <asp:DropDownList ID="cmbPosicion" class="form-control" runat="server" Enabled="false" Font-Size="Medium" Font-Bold="true" ></asp:DropDownList>
                        <a class="tooltip" ><span class="classic" >Posición de usuario</span><img alt='' src='../shared/imgs/info.gif' class='datainfo'/></a>
                  </div>
             </div>

              <div class="form-group col-md-2"> 
                <label for="inputAddress">Rol :<span style="color: #FF0000; font-weight: bold;"></span></label>
                  <div class="d-flex">
                        <asp:DropDownList ID="cmbRol" class="form-control" runat="server" Enabled="false" Font-Size="Medium"  Font-Bold="true" ></asp:DropDownList>
                        <a class="tooltip" ><span class="classic" >Rol asociado al usuario</span><img alt='' src='../shared/imgs/info.gif' class='datainfo'/></a>
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


     <asp:UpdatePanel ID="UPDETALLE" runat="server"  >  
       <ContentTemplate>
        
           <section class="wrapper2">
            <h4 class="mb">LISTA DE USUARIOS</h4>
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
                            <th class="center hidden-phone">IDENTIFICACIÓN</th>
                            <th class="center hidden-phone">USUARIO</th>
                            <th class="center hidden-phone">NOMBRE</th>
                              <th class="center hidden-phone">POSICIÓN</th>
                              <th class="center hidden-phone">ROL</th>
                              <th class="center hidden-phone">COMPAÑIA</th>
                            <th class="center hidden-phone">ESTADO</th>
                            <th class="center hidden-phone">FECHA CREACIÓN</th>
                            <th class="center hidden-phone">USUARIO</th>
                            <th class="center hidden-phone">ACCION</th>
                          
                          </tr>
                        </thead>
                        <tbody>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr class="gradeC">
                                <td class="center hidden-phone"><asp:Label Text='<%#Eval("id")%>' ID="LblAsignacion" runat="server"  /></td>
                                <td class="center hidden-phone"><%#Eval("identification")%></td>
                                <td class="center hidden-phone"><%#Eval("username")%></td>
                                <td class="center hidden-phone"><%#Eval("nombre")%></td>
                                <td class="center hidden-phone"><%#Eval("position")%></td>
                                <td class="center hidden-phone"><%#Eval("rol")%></td>
                                <td class="center hidden-phone"><%#Eval("company")%></td>
                                <td class="center hidden-phone"> <asp:CheckBox ID="CHKPRO" runat="server" Checked='<%# Eval("estado") %>'  CssClass="center hidden-phone" Enabled="false"/></td>
                                
                                <td class="center hidden-phone"><%#Eval("fechaCreacion")%></td>
                                <td class="center hidden-phone"><%#Eval("usuarioCrea")%></td> 
                                <td class="center hidden-phone">  
                                    <asp:Button ID="btnModificar"  CommandArgument= '<%#Eval("id")%>' runat="server" Text="Modificar" 
                                        class="btn btn-primary" ToolTip="Permite remover una carga" CommandName="Modificar"   
                                          /> <%--OnClientClick="return confirm('Esta seguro de que desea modificar el item seleccioando?');"--%>
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
