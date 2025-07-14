<%@ Page Title="Configuración de Lineas de VBS Banano" Language="C#" MasterPageFile="~/site.Master" AutoEventWireup="true" CodeBehind="VBS_BAN_Ubicacion.aspx.cs" Inherits="CSLSite.VBS_BAN_Ubicacion" %>
<%@ MasterType VirtualPath="~/site.Master" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>

<asp:Content ID="Content3" ContentPlaceHolderID="placehead" runat="server">

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
          $(document).ready(function () {

              if (!$.fn.DataTable.isDataTable('#tablePagination')) {
                  $('#tablePagination').DataTable(
                      {

                          language: {
                              "lengthMenu": "_MENU_ REGISTROS POR PAGINA",
                              "zeroRecords": "No se encontraron resultados",
                              "info": "Mostrando registros del _START_ al _END_ de un total de _TOTAL_ registros",
                              "infoEmpty": "Mostrando registros del 0 al 0 de un total de 0 registros",
                              "infoFiltered": "(filtrado de un total de _MAX_ registros)",
                              "sSearch": "Filtrar:",
                              "sProcessing": "Procesando...",

                          },
                          //para usar los botones   
                          responsive: "true",
                          dom: 'Bfrtilp',
                          buttons: [
                              {
                                  extend: 'excel',
                                  text: '<i class="fa fa-file-excel-o"></i> ',
                                  titleAttr: 'Exportar a Excel',
                                  className: 'btn btn-primary'
                              },
                              {
                                  extend: 'pdf',
                                  text: '<i class="fa fa-file-pdf-o"></i> ',
                                  titleAttr: 'Exportar a PDF',
                                  className: 'btn btn-primary',
                                  orientation: 'landscape',
                                  pageSize: 'LEGAL'
                              },
                              {
                                  extend: 'print',
                                  text: '<i class="fa fa-print"></i> ',
                                  titleAttr: 'Imprimir',
                                  className: 'btn btn-primary',
                                  size: 'landscape'
                              },
                          ],
                          pageLength: 50,
                          initComplete: function () {
                              // Alínea los botones a la derecha con CSS
                              //$('.dt-buttons').css('float', 'right');
                              // Alínea el filtro a la derecha con CSS
                              //$('.dataTables_filter').css({'float': 'right'});

                          }


                      });
              }
          });

      }
  </script>

      

</asp:Content>


<asp:Content ID="Content4" ContentPlaceHolderID="placebody" runat="server" >
 
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></asp:ToolkitScriptManager>
    
  <div id="div_BrowserWindowName" style="visibility:hidden;">
    <asp:HiddenField ID="HiddenField1" runat="server" />
     
  </div>

    <div class="mt-4">         
        <nav class="mt-4" aria-label="breadcrumb">
          <ol class="breadcrumb">
            <li class="breadcrumb-item" id="Li1" runat="server"><a href="#">VBS Banano</a></li>
            <li class="breadcrumb-item active" aria-current="page" id="Li2" runat="server">UBICACIÓN DE BODEGA</li>
          </ol>
        </nav>
      </div>

    <div class="dashboard-container p-4" id="Div1" runat="server">
        
    <div class="form-title">
          DATOS DE LA UBICACIÓN EN BODEGA
    </div>
    <asp:UpdatePanel ID="UPCARGA" runat="server"  UpdateMode="Conditional" ChildrenAsTriggers="true">
    <ContentTemplate>
         <div class="form-row">

             <div class="form-group col-md-2"> 
                <label for="inputAddress">Bodega :<span style="color: #FF0000; font-weight: bold;"></span></label>
                <div class="d-flex">
                    <asp:DropDownList ID="cmbBodega" class="form-control"  runat="server" Font-Size="Medium" AutoPostBack="true" Font-Bold="true" OnSelectedIndexChanged="cmbBodega_SelectedIndexChanged" >
                    </asp:DropDownList>
                    <a class="tooltip" ><span class="classic" >Nombre de Bodega</span><img alt='' src='../shared/imgs/info.gif' class='datainfo'/></a>
                </div>
            </div>

             <div class="form-group col-md-2"> 
                <label for="inputAddress">Bloque :<span style="color: #FF0000; font-weight: bold;"></span></label>
                <div class="d-flex">
                    <asp:DropDownList ID="cmbBloque" class="form-control"  runat="server" Font-Size="Medium" AutoPostBack="true" Font-Bold="true" OnSelectedIndexChanged="cmbBloque_SelectedIndexChanged">
                    </asp:DropDownList>
                    <a class="tooltip" ><span class="classic" >Nombre de Bloque</span><img alt='' src='../shared/imgs/info.gif' class='datainfo'/></a>
                </div>
            </div>

             <div class="form-group col-md-2"> 
                <label for="inputAddress">Fila :<span style="color: #FF0000; font-weight: bold;"></span></label>
                <div class="d-flex">
                    <asp:DropDownList ID="cmbFila" class="form-control"  runat="server" Font-Size="Medium" AutoPostBack="true" Font-Bold="true" OnSelectedIndexChanged="cmbFila_SelectedIndexChanged" >
                    </asp:DropDownList>
                    <a class="tooltip" ><span class="classic" >Nombre de Fila</span><img alt='' src='../shared/imgs/info.gif' class='datainfo'/></a>
                </div>
            </div>

             <div class="form-group col-md-2"> 
                <label for="inputAddress">Altura :<span style="color: #FF0000; font-weight: bold;"></span></label>
                <div class="d-flex">
                    <asp:DropDownList ID="cmbAltura" class="form-control"  runat="server" Font-Size="Medium" AutoPostBack="false" Font-Bold="true"  >
                    </asp:DropDownList>
                    <a class="tooltip" ><span class="classic" >Nombre de Altura</span><img alt='' src='../shared/imgs/info.gif' class='datainfo'/></a>
                </div>
            </div>

             <div class="form-group col-md-2"> 
                <label for="inputAddress">Profundidad :<span style="color: #FF0000; font-weight: bold;"></span></label>
                <div class="d-flex">
                    <asp:DropDownList ID="cmbProfundidad" class="form-control"  runat="server" Font-Size="Medium" AutoPostBack="false" Font-Bold="true" >
                    </asp:DropDownList>
                    <a class="tooltip" ><span class="classic" >Nombre de Profundidad</span><img alt='' src='../shared/imgs/info.gif' class='datainfo'/></a>
                </div>
            </div>

             <div class="form-group col-md-1" style="display: none;"> 
                <label for="inputAddress">Barcode :<span style="color: #FF0000; font-weight: bold;"></span></label>
                  <div class="d-flex">
                    <asp:TextBox ID="txtBarcode" disabled runat="server" MaxLength="50" 
                            onkeypress="return soloLetras(event,'abcdefghijklmnñopqrstuvwxyz1234567890')" 
                    
                            placeholder="Barcode"
                            class="form-control"
                            style="text-transform:uppercase;"
                            ClientIDMode="Static">
                    </asp:TextBox>
                    <span id="valcont2" class="validacion"> * </span>      
                </div>
             </div>
         
             <div class="form-group col-md-4" style="display: none;"> 
                <label for="inputAddress">Descripción :<span style="color: #FF0000; font-weight: bold;"></span></label>
                  <div class="d-flex">
                    <asp:TextBox ID="txtDescripcion" runat="server" MaxLength="50" 
                            onkeypress="return soloLetras(event,'abcdefghijklmnñopqrstuvwxyz1234567890')" 
                    
                            placeholder="Descripción"
                            class="form-control"
                            style="text-transform:uppercase;"
                            ClientIDMode="Static">
                    </asp:TextBox>
                    <span id="valcont1" class="validacion"> * </span>      
                </div>
             </div>

            <div class="form-group col-md-1"> 
                <label for="inputAddress">Disponible :<span style="color: #FF0000; font-weight: bold;"></span></label>
                  <div class="d-flex">
                        <asp:DropDownList ID="cmbDisponible" class="form-control" runat="server" Font-Size="Medium"  Font-Bold="true" >
                            <asp:ListItem Value="True">ACTIVO</asp:ListItem>
                            <asp:ListItem Value="False">INACTIVO</asp:ListItem>
                        </asp:DropDownList>
                        <%--<a class="tooltip" ><span class="classic" >Maniobras asociado al producto</span><img alt='' src='../shared/imgs/info.gif' class='datainfo'/></a>--%>
                  </div>
            </div>

            <div class="form-group col-md-1"> 
                <label for="inputAddress">Estado :<span style="color: #FF0000; font-weight: bold;"></span></label>
                  <div class="d-flex">
                        <asp:DropDownList ID="cmbEstado" class="form-control" runat="server" Font-Size="Medium"  Font-Bold="true" >
                            <asp:ListItem Value="True">ACTIVO</asp:ListItem>
                            <asp:ListItem Value="False">INACTIVO</asp:ListItem>
                        </asp:DropDownList>
                        <%--<a class="tooltip" ><span class="classic" >Maniobras asociado al producto</span><img alt='' src='../shared/imgs/info.gif' class='datainfo'/></a>--%>
                  </div>
             </div>

             <div class="form-group   col-md-1" style="display: none;"> 
                <label for="inputAddress">Capacidad:<span style="color: #FF0000; font-weight: bold;"></span></label>

                <div class="d-flex">
                    <asp:TextBox ID="txtCapacidadBox" runat="server" MaxLength="9" 
                            onkeypress="return soloLetras(event,'1234567890',true)" 
                            placeholder="Capacidad"
                            class="form-control" >
                    </asp:TextBox>
                    <span id="valcont" class="validacion"> * </span>
                </div>
                  
                  
            </div>

            <div class="form-group   col-md-2" style="display: none;"> 
                <label for="inputAddress">Mt2:<span style="color: #FF0000; font-weight: bold;"></span></label>

                <div class="d-flex">
                    <asp:TextBox ID="txtMt2" runat="server" MaxLength="9" 
                            onkeypress="return soloLetras(event,'1234567890',true)" 
                            placeholder="METROS 2"
                            class="form-control" >
                    </asp:TextBox>
                    <span id="valcont0" class="validacion"> * </span>
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
            <script type="text/javascript">
                Sys.Application.add_load(BindFunctions);
            </script>
            
            <div class="form-group col-md-12"> 
                <div class="form-title">LISTA DE UBICACIONES</div>
            </div>

            <section class="wrapper2">
                
                    <div id="xfinder" runat="server" visible="true" >
                        <div class="findresult" >
                            <div class="booking" >                                
                                      
                                <div class="bokindetalle" style=" width:100%; overflow:auto">
                                    <asp:Repeater ID="tablePagination" runat="server" onitemcommand ="tablePagination_ItemCommand" >
                                        <HeaderTemplate>
                                        <table cellpadding="0" cellspacing="0" border="0" class="table table-bordered table-sm table-contecon" id="tablePagination" width="100%">
                                            <thead style="background: #B4B4B4; color: white">
                                                <tr>
                                                    <th class="th-sm"></th>
                                                    <th class="center hidden-phone">CODIGO</th>
                                                    <th class="center hidden-phone">BODEGA</th>
                                                    <th class="center hidden-phone">BLOQUE</th>
                                                    <th class="center hidden-phone">FILA</th>
                                                    <th class="center hidden-phone">ALTURA</th>
                                                    <th class="center hidden-phone">PROFUNDIDAD</th>
                                                    <th class="center hidden-phone">BARCODE</th>
                                                    <th class="center hidden-phone">DISPONIBLE</th>
                                                    <th class="center hidden-phone">ESTADO</th>
                                                    <th class="center hidden-phone">ACCION</th>
                                                 </tr>
                                            </thead>
                                        <tbody>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <tr>
                                                <td class="center hidden-phone"><asp:Label Text='<%# Container.ItemIndex + 1 %>' ID="lblSecuencia" runat="server"  /></td>
                                                <td class="center hidden-phone"><asp:Label Text='<%#Eval("id")%>' ID="LblAsignacion" runat="server"  /></td>
                                                <td class="center hidden-phone"><%#Eval("bodega")%></td>
                                                <td class="center hidden-phone"><%#Eval("bloque")%></td>
                                                <td class="center hidden-phone"><%#Eval("fila")%></td>
                                                <td class="center hidden-phone"><%#Eval("altura")%></td>
                                                <td class="center hidden-phone"><%#Eval("profundidad")%></td>
                                                <td class="center hidden-phone"><%#Eval("barcode")%></td>
                                                <td class="center hidden-phone"><%#Eval("disponible")%></td>
                                                <%--<td class="center hidden-phone"> <asp:CheckBox ID="CheckBox1" runat="server" Checked='<%# Eval("disponible") %>'  CssClass="center hidden-phone" Enabled="false"/></td>--%>
                                                <td class="center hidden-phone"> <asp:CheckBox ID="CHKPRO" runat="server" Checked='<%# Eval("estado") %>'  CssClass="center hidden-phone" Enabled="false"/></td>
                                
                                                <%--<td class="center hidden-phone"><%#Eval("fechaCreacion")%></td>
                                                <td class="center hidden-phone"><%#Eval("fechaModificacion")%></td>--%>
                                
                                                <td class="center hidden-phone">  
                                                    <asp:Button ID="btnModificar"  CommandArgument= '<%#Eval("id")%>' runat="server" Text="Modificar" 
                                                        class="btn btn-primary" ToolTip="Permite Editar" CommandName="Modificar"   
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

                </section>
     
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="BtnAdd" />
        </Triggers>
    </asp:UpdatePanel>


    </div>

    <script type="text/javascript" src="../lib/datatables/datatables.min.js"></script>
    <%--<script type="text/javascript" src="../lib/advanced-datatable/js/DT_bootstrap.js"></script>--%>
    <script type="text/javascript" src="../lib/common-scripts.js"></script>
    <script type="text/javascript" src="../lib/pages.js" ></script>
    <script type="text/javascript" src="../lib/advanced-form-components.js"></script>

    <script src="../Scripts/pages.js" type="text/javascript"></script>
    <script src="../Scripts/credenciales.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.tablesorter.min.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.tablesorter.pager.js" type="text/javascript"></script>


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
                var vals = document.getElementById('<%=txtBarcode.ClientID %>').value;
                if (vals == null || vals == undefined || vals == '') {
                    alert('¡ Escriba el barcode.');
                    document.getElementById("loader3").className = 'nover';
                    document.getElementById('<%=txtBarcode.ClientID %>').focus();
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

