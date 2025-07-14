
<%@ Page Title="Stowage Plan" Language="C#" MasterPageFile="~/site.Master" AutoEventWireup="true" CodeBehind="VBS_BAN_LoadingStowage.aspx.cs" Inherits="CSLSite.VBS_BAN_LoadingStowage" %>
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
        function fechas() {
            $(document).ready(function () {
                $('.datetimepicker').datetimepicker().datetimepicker({ lang: 'es', step: 30, format: 'm/d/Y' });
            });

            $(document).ready(function () {
                $('.datetimepickerAlt').datetimepicker().datetimepicker({ lang: 'es', closeOnDateSelect: true, timepicker: false, step: 30, format: 'm/d/Y' });
            });

            $(document).ready(function () {
                $('.datepicker').datetimepicker().datetimepicker({ lang: 'es', timepicker: false, format: 'm/d/Y', closeOnDateSelect: true, minDate: '0' });

            });

            $(document).ready(function () {
                $('.datetimepickerDate').datetimepicker().datetimepicker({ lang: 'es', step: 30, format: 'm/d/Y H:i' });
            });
        }
    </script>

    <script type="text/javascript">

        function BindFunctions()
        {
            $(document).ready(function ()
            {
                    
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
                            pageLength: 100,
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

<asp:Content ID="Content2" ContentPlaceHolderID="placebody" runat="server" >
 
    <asp:ToolkitScriptManager ID="tkscata" runat="server"></asp:ToolkitScriptManager>
    
    <div id="div_BrowserWindowName" style="visibility:hidden;">
        <asp:HiddenField ID="hf_BrowserWindowName" runat="server" />
    </div>

    <div class="mt-4">         
        <nav class="mt-4" aria-label="breadcrumb">
            <ol class="breadcrumb">
                <li class="breadcrumb-item" id="opcion_principal" runat="server"><a href="#">VBS Banano</a></li>
                <li class="breadcrumb-item active" aria-current="page" id="sub_opcion" runat="server">STOWAGE CONFIGURATION</li>
            </ol>
        </nav>
    </div>

    <div class="dashboard-container p-4" id="cuerpo" runat="server">
        <div class="form-title">
            DATOS DEL USUARIO
        </div>
        <div class="form-row" >
            <div class="form-group col-md-6"> 
                <label for="inputAddress">ESTIMADO CLIENTE:<span style="color: #FF0000; font-weight: bold;"></span></label>
                <asp:TextBox ID="Txtcliente" runat="server" class="form-control" placeholder=""  Font-Bold="true" disabled ></asp:TextBox>
            </div>

            <div class="form-group col-md-2">
                <label for="inputAddress">RUC:<span style="color: #FF0000; font-weight: bold;"></span></label>
				<asp:TextBox ID="Txtruc" runat="server" class="form-control" placeholder=""  Font-Bold="true" disabled></asp:TextBox>
            </div>

            <div class="form-group col-md-4">
                <label  for="inputAddress">EMPRESA:<span style="color: #FF0000; font-weight: bold;"></span></label>
                <asp:TextBox ID="Txtempresa" runat="server" class="form-control" placeholder=""  Font-Bold="true" disabled></asp:TextBox>
            </div>
        </div>

        <div class="form-title">
              DATOS DE NAVE
        </div>
        <asp:UpdatePanel ID="UPCAB" runat="server"  UpdateMode="Conditional" ChildrenAsTriggers="true">
            <ContentTemplate>
                 <script type="text/javascript">
                     Sys.Application.add_load(fechas);
                 </script>
                <div class="form-row">

                    <div class="form-group col-md-2"> 
                        <label for="inputAddress"> Referencia<span style="color: #FF0000; font-weight: bold;">*</span><span style="color: #FF0000; font-weight: bold;"></span></label>
                        <div class="d-flex"> 
                            <asp:TextBox  class="form-control" disabled  ID="txtNave" AutoPostBack="true" runat="server" MaxLength="30" onkeypress="return soloLetras(event,'abcdefghijklmnopqrstuvwxyz01234567890/')" ></asp:TextBox>
                            
                        </div>
                    </div>

                    <div class="form-group col-md-4"> 
                        <label for="inputAddress">Nombre de Nave<span style="color: #FF0000; font-weight: bold;">*</span><span style="color: #FF0000; font-weight: bold;"></span></label>
                        <div class="d-flex"> 
                            <asp:TextBox  class="form-control"  ID="txtDescripcionNave" disabled runat="server" MaxLength="50" onkeypress="return soloLetras(event,'abcdefghijklmnopqrstuvwxyz01234567890/')"></asp:TextBox>
                            &nbsp;
                            
                            <a class="btn btn-outline-primary mr-4" style='font-size:24px' target="popup" onclick="document.getElementById('<%= btnLimpiar.ClientID %>').click(),window.open('../catalogo/naves.aspx','name','width=900,height=880')">
                                    <span class='fa fa-search' style='font-size:24px'></span> </a>
                        </div>
                    </div>

                    <div class="form-group col-md-2" style="display: none;">
                        <label for="inputZip">MRN<span style="color: #FF0000; font-weight: bold;">*</span></label>
                        <asp:TextBox ID="TXTMRN" Visible ="false" runat="server" class="form-control" disabled MaxLength="16"  onkeypress="return soloLetras(event,'abcdefghijklmnopqrstuvwxyz0123456789-_')" placeholder="MRN"></asp:TextBox>
                    </div>

                    <div class="form-group   col-md-2" style="display: none;"> 
                        <label for="inputAddress"> ETA:<span style="color: #FF0000; font-weight: bold;"></span></label>

                        <div class="d-flex">
                            <asp:TextBox class="form-control" Visible ="false"  runat="server" ID="fecETA" disabled   AutoPostBack="false" MaxLength="10"  onkeypress="return soloLetras(event,'0123456789/')" ></asp:TextBox>

                            <asp:CalendarExtender ID="CAGTFECHAFASA"  runat="server"
                                CssClass="cal_Theme1" Format="dd/MM/yyyy" TargetControlID="fecETA">
                            </asp:CalendarExtender>      
                        </div>
                    </div>

                    <div class="form-group col-md-4"> 
                        <label for="inputAddress">Linea :<span style="color: #FF0000; font-weight: bold;"></span></label>
                        <div class="d-flex">
                            <asp:DropDownList ID="cmbLinea" class="form-control"  runat="server" Font-Size="Medium" AutoPostBack="true" Font-Bold="true" OnSelectedIndexChanged="cmbLinea_SelectedIndexChanged">
                            </asp:DropDownList>
                            <a class="tooltip" ><span class="classic" >Nombre de Linea</span><img alt='' src='../shared/imgs/info.gif' class='datainfo'/></a>
                        </div>
                    </div>

                 
                </div>

                <div></div>

                <div class="row">
                    <div class="col-md-12 d-flex justify-content-center">
                        <asp:Button ID="btnLimpiar" runat="server" class="btn btn-outline-primary mr-2" Text="Limpiar" OnClick="btnLimpiar_Click"   />
                        <asp:Button ID="btnBuscar" runat="server" class="btn btn-primary mr-2"  Text="Buscar"  OnClientClick="return mostrarloader('1')" OnClick="btnBuscar_Click"  />
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
                <asp:AsyncPostBackTrigger ControlID="btnBuscar" />
            </Triggers>
        </asp:UpdatePanel>

        <asp:UpdatePanel ID="UPDETALLE" runat="server"  UpdateMode="Conditional"  >  
            <ContentTemplate>
                <script type="text/javascript">
                    Sys.Application.add_load(BindFunctions);
                </script>


                <div class="form-group col-md-12"> 
                    <div class="form-title">CONFIGURACIÓN DEL STOWAGE PLAN</div>
                </div>

                <div class="form-group">
                    <div class="alert alert-warning" id="banmsg_det" runat="server" clientidmode="Static"><b>Error!</b> >Debe ingresar el número de la carga MRN......</div>
                </div>

        
                <section class="wrapper2">
                
                    <div id="xfinder" runat="server" visible="true" >
                        <div class="findresult" >
                            <div class="booking" >                                
                                      
                                <div class="bokindetalle" style=" width:100%; overflow:auto">

                                    <asp:Repeater ID="tablePagination" runat="server" onitemcommand ="tablePagination_ItemCommand" OnItemDataBound="tablePagination_ItemDataBound">
                                        <HeaderTemplate>
                                        <table cellpadding="0" cellspacing="0" border="0" class="table table-bordered table-sm table-contecon" id="tablePagination" width="100%">
                                            <thead style="background: #B4B4B4; color: white">
                                                <tr>
                                                    <th class="th-sm"></th>
                                                    <th class="nover">ID</th>
                                                    <th class="nover">HOLD_ID</th>
                                                    <th class="th-sm">DESTINATION</th>
                                                    <th class="th-sm">SHIPPER</th>
                                                    <th class="th-sm">BOXES</th>
                                                    <th class="th-sm">CARGO</th>
                                                    <th class="th-sm">CNEE</th>
                                                    <th class="th-sm">HOLD</th>
                                                    <th class="th-sm">BRAND</th>
                                                    <th class="th-sm">DECK</th>
                                                    <th class="th-sm">AISV</th>
                                                    <th class="th-sm">DATE</th>
                                                    <th class="th-sm">TIME</th>
                                                    <th class="nover"></th>
                                                    <th class="center hidden-phone"></th>
                                                    <th class="nover"></th>
                                                    <th class="nover"></th>
                                                </tr>
                                            </thead>
                                        <tbody>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <tr>
                                                <td class="center hidden-phone"><asp:Label Text='<%# Container.ItemIndex + 1 %>' ID="lblSecuencia" runat="server"  /></td>
                                                <td class="nover"><asp:Label Text='<%#Eval("id")%>' ID="lblId" runat="server"  /></td>
                                                <td class="nover"> <%#Eval("idHold")%></td> 
                                                <td class="center hidden-phone"><asp:Label Text='<%#Eval("destino")%>' ID="Label4" runat="server"  /></td>      
                                                <td class="center hidden-phone"><asp:Label Text='<%#Eval("exportador")%>' ID="lblexportador" runat="server"  /></td>      
                                                <td class="center hidden-phone"><asp:Label Text='<%#Eval("box")%>' ID="lblbox" runat="server"  /></td>             
                                                <td class="center hidden-phone"><asp:Label Text='<%#Eval("cargo")%>' ID="lblcargo" runat="server"  /></td>           
                                                <td class="center hidden-phone"><asp:Label Text='<%#Eval("consignatario")%>' ID="lblconsignatario" runat="server"  /></td>   
                                                <td class="center hidden-phone"><asp:Label Text='<%#Eval("Hold")%>' ID="lblHold" runat="server"  /></td>            
                                                <td class="center hidden-phone"><asp:Label Text='<%#Eval("marca")%>' ID="lblmarca" runat="server"  /></td>           
                                                <td class="center hidden-phone"><asp:Label Text='<%#Eval("deck")%>' ID="lbldeck" runat="server"  /></td>        
                                                <td class="center hidden-phone"><asp:Label Text='<%#Eval("aisv")%>' ID="lblBodega" runat="server"  /></td>    
                                                <td class="center hidden-phone"><asp:Label Text='<%#Eval("fecha")%>' ID="Label3" runat="server"  /></td>      
                                                <td class="center hidden-phone"><asp:Label Text='<%#Eval("time")%>' ID="lblAisv" runat="server"  /></td>   
                                               
                                                <td class="nover">  
                                                    <asp:Button runat="server" ID="btnEditar" Height="30px" CommandName="Editar" Text="Editar"  CommandArgument='<%# Bind("id") %>' class="btn btn-outline-primary mr-2"  data-toggle="modal" data-target="#myModal"  />
                                                </td>
                                                <td class="center hidden-phone"> 
                                                    <asp:Button runat="server" ID="btnDetalle" Height="30px" CommandName="Aisv" Text="Turnos"  CommandArgument='<%# Bind("id") %>' class="btn btn-primary"  data-toggle="modal" data-target="#myModal1"  />
                                                </td> 
                                                <td class="nover"> 
                                                    <asp:Button runat="server" ID="btnServicios" Height="30px" CommandName="Servicios" Text="Servicios"  CommandArgument='<%# Bind("id") %>' class="btn btn-primary"  data-toggle="modal" data-target="#myModal2"  />
                                                </td> 
                                                <td class="nover"> 
                                                    <asp:Button runat="server" ID="btnPublicar" Height="30px" CommandName="Publicar" Text="Publicar"  CommandArgument='<%# Bind("id") %>' class="btn btn-primary"  data-toggle="modal" data-target="#myModal3"  />
                                                </td> 
                                            </tr>    
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            
                                        </tbody>
                                            <tr>
                                                <td colspan="2">Total:</td>
                                                    <td>
                                                        <asp:Label ID="lblTotalBoxes" Text="0" Font-Bold="true" runat="server"></asp:Label>

                                                    </td>
                                                <td colspan="6"></td>
                                            </tr>
                                        </table>
                                            
                                        </FooterTemplate>
                                    </asp:Repeater>

                                    <asp:GridView ID="dgvTotales" runat="server" AutoGenerateColumns="False"  GridLines="None" ShowHeader ="false"
                                        CssClass="display table table-bordered invoice">
                                        <PagerStyle HorizontalAlign = "Right" CssClass="display table table-bordered invoice"  />
                                        <RowStyle  BackColor="#F0F0F0" />
                                        <alternatingrowstyle  BackColor="#FFFFFF" />
                           
                                        <Columns>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <%# Container.DataItemIndex + 1 %>
                                                </ItemTemplate>

                                            </asp:TemplateField>
                                            <asp:BoundField DataField="Hold" HeaderText="HOLD"  HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>
                                            <asp:BoundField DataField="TotalBoxes" HeaderText="TOTAL"  HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>

                                        </Columns>
                                    </asp:GridView>
                                </div><!--adv-table-->
                            <%--   </section>--%>
                            </div><!--content-panel-->
                        </div><!--col-lg-12-->

                         
                    </div><!--row mt-->
                     <div id="sinresultado" runat="server" class="alert alert-info">
                        No se encontraron resultados, por favor verifique los criterios de búsqueda
                    </div>

                </section><!--wrapper2-->
     
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnBuscar" />
            </Triggers>
        </asp:UpdatePanel>

        <div id="myModal" class="modal fade" tabindex="-1" role="dialog">

            <div class="modal-dialog" role="document" style="max-width: 1000px"> <!-- Este tag style controla el ancho del modal -->
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title">DATOS DEL ITEM STOWAGE</h5>
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                        </button>
                    </div>
                    <div class="modal-body">

                        <asp:UpdatePanel ID="UPEDIT" runat="server" UpdateMode="Conditional" >  
                                    <ContentTemplate>
                                        <asp:Panel ID="Panel2" runat="server">         
                                                <div id="div_Codigos" style="visibility:hidden;">
                                                    <asp:HiddenField ID="hdf_CodigoCab" runat="server" />
                                                    <asp:HiddenField ID="hdf_CodigoDet" runat="server" />
                                                </div>

                                            <div class="form-row">

                                                <div class="form-group col-md-3">
                                                    <span class="help-block">&nbsp;</span>

                                                    <label for="inputZip">HOLD</label>
                                                    <div class="d-flex"> 
                                                        <asp:DropDownList  ID="cmbDetHold" disabled class="form-control"  runat="server" Font-Size="Medium" Font-Bold="true">
                                                        </asp:DropDownList>
                                                    </div>
		                                        </div>
                                                <div class="form-group col-md-3">
                                                    <span class="help-block">&nbsp;</span>

                                                    <label for="inputZip">Piso</label>
                                                    <div class="d-flex"> 
                                                        <asp:TextBox class="form-control"  disabled ID="txtDetPiso" runat="server" MaxLength="50" onkeypress="return soloLetras(event,'abcdefghijklmnopqrstuvwxyz01234567890/')"></asp:TextBox>
                                                    </div>
		                                        </div>

                                                <div class="form-group col-md-3">
                                                    <span class="help-block">&nbsp;</span>

                                                    <label for="inputZip">Cargo</label>
                                                    <div class="d-flex"> 
                                                        <asp:DropDownList  ID="cmbDetCargo" disabled class="form-control"  runat="server" Font-Size="Medium" Font-Bold="true"   >
                                                        </asp:DropDownList>
                                                    </div>
		                                        </div>

                                                <div class="form-group col-md-3">
                                                    <span class="help-block">&nbsp;</span>

                                                    <label for="inputZip">Exportador</label>
                                                    <div class="d-flex"> 
                                                        <asp:TextBox class="form-control" disabled  ID="txtDetExportador" runat="server" MaxLength="50" onkeypress="return soloLetras(event,'abcdefghijklmnopqrstuvwxyz01234567890/')"></asp:TextBox>
                                                    </div>
		                                        </div>

                                                <div class="form-group col-md-3">
                                                    <span class="help-block">&nbsp;</span>

                                                    <label for="inputZip">Marca</label>
                                                    <div class="d-flex"> 
                                                        <asp:TextBox class="form-control" disabled  ID="txtDetMarca" runat="server" MaxLength="50" onkeypress="return soloLetras(event,'abcdefghijklmnopqrstuvwxyz01234567890/')"></asp:TextBox>
                                                    </div>
		                                        </div>
                                                

                                                <div class="form-group col-md-6">
                                                    <span class="help-block">&nbsp;</span>

                                                    <label for="inputZip">Consignatario</label>
                                                    <div class="d-flex"> 
                                                        <asp:TextBox class="form-control" disabled  ID="txtDetConsignatario" runat="server" MaxLength="50" onkeypress="return soloLetras(event,'abcdefghijklmnopqrstuvwxyz01234567890/')"></asp:TextBox>
                                                    </div>
		                                        </div>

                                                <div class="form-group col-md-3">
                                                    <span class="help-block">&nbsp;</span>

                                                    <label for="inputZip">Cantidad</label>
                                                    <div class="d-flex"> 
                                                        <asp:TextBox ID="txtDetCantidad" runat="server" MaxLength="9" class="form-control" onkeypress="return soloLetras(event,'1234567890',true)" placeholder="CANTIDAD"></asp:TextBox>
                                                    </div>
		                                        </div>
                    
             
                                                <div class="form-group col-md-12"> 
                                                    <label for="inputAddress">Observación<span style="color: #FF0000; font-weight: bold;"></span></label>
                                                    <div class="d-flex"> 
                                                        <asp:TextBox TextMode="MultiLine"  class="form-control"  ID="txtDetobservacion" runat="server" MaxLength="100" onkeypress="return soloLetras(event,'abcdefghijklmnopqrstuvwxyz01234567890/')"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            
                                        </asp:Panel>
                      
                                        <section class="wrapper2">
                                                <div class="content-panel">
                                                
                                                    <div class="row">
                                                        <div class="col-md-12 d-flex justify-content-center">
                                                            <asp:Button ID="btnActualizar" runat="server" class="btn btn-primary"  Text="ACTUALIZAR"  OnClientClick="return mostrarloader('1')" OnClick="btnActualizar_Click"  />
                                                            <img alt="loading.." src="../lib/file-uploader/img/loading.gif" height="32px" width="32px"  id="ImgCargas" class="nover"   />
                                                            <span id="imagens"></span>
                                                        </div>
                                                    </div>

                                                     <br/>
                                                    <div class="row">
                                                        <div class="col-md-12 d-flex justify-content-center">
                                                            <div class="alert alert-warning" id="msjErrorDetalle" runat="server" clientidmode="Static"><b>Error!</b> >......</div>
                                                        </div>
                                                    </div>


                                                </div><!--content-panel-->
                                        </section><!--wrapper2-->
                                    </ContentTemplate>   
                                </asp:UpdatePanel>


                     </div>
                    <div class="modal-footer d-flex justify-content-center">
                        <button type="button" class="btn btn-outline-primary " data-dismiss="modal">Salir</button>
                    </div>
                </div>
            </div>
        </div>

        <div id="myModal1" class="modal fade" tabindex="-1" role="dialog">

            <div class="modal-dialog" role="document" style="max-width: 1400px"> <!-- Este tag style controla el ancho del modal -->
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title">DETALLE DE CONFIGURACIÓN DEL STOWAGE PLAN</h5>
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                        </button>
                    </div>
                    <div class="modal-body">

                        <asp:UpdatePanel ID="UPBOTONES" runat="server" UpdateMode="Conditional" >  
                            <ContentTemplate> 

                              <%--  <div class="form-group col-md-12"> 
                                    <div class="form-title">DETALLE DE CONFIGURACIÓN DEL LOADING</div>
                                </div>
                                --%>
                                <div class="form-row">

                                     <div class="form-group col-md-4">
                                        <span class="help-block">&nbsp;</span>

                                        <label for="inputZip">Exportador</label>
                                        <div class="d-flex"> 
                                            <asp:TextBox class="form-control"  ID="txtExportadorSeleccionado"  disabled runat="server" MaxLength="50" onkeypress="return soloLetras(event,'abcdefghijklmnopqrstuvwxyz01234567890/')"></asp:TextBox>
                                        </div>
		                            </div>


                                    <div class="form-group col-md-2">
                                        <span class="help-block">&nbsp;</span>

                                        <label for="inputZip">Autorizado</label>
                                        <div class="d-flex"> 
                                            <asp:TextBox ID="txtCantidadAutorizada"  disabled runat="server" MaxLength="9" class="form-control" onkeypress="return soloLetras(event,'1234567890',true)" placeholder="CANTIDAD"></asp:TextBox>
                                        </div>
		                            </div>

                                    <div class="form-group col-md-1">
                                        <span class="help-block">&nbsp;</span>

                                        <label for="inputZip">Reservado</label>
                                        <div class="d-flex"> 
                                            <asp:TextBox ID="txtReservado"  disabled runat="server" MaxLength="9" class="form-control" onkeypress="return soloLetras(event,'1234567890',true)" placeholder="CANTIDAD"></asp:TextBox>
                                        </div>
		                            </div>

                                    <div class="form-group col-md-1">
                                        <span class="help-block">&nbsp;</span>

                                        <label for="inputZip">Disponible</label>
                                        <div class="d-flex"> 
                                            <asp:TextBox ID="txtDisponible"  disabled runat="server" MaxLength="9" class="form-control" onkeypress="return soloLetras(event,'1234567890',true)" placeholder="CANTIDAD"></asp:TextBox>
                                        </div>
		                            </div>

                                    <div class="form-group col-md-2"> 
                                        <label for="inputEmail4">Fecha </label>
                                        <asp:TextBox ID="TxtFechaDesde" runat="server"  class="datepicker form-control"  MaxLength="10"  placeholder="MM/dd/yyyy" onkeypress="return soloLetras(event,'1234567890/:')"></asp:TextBox>
                                    </div>
                                    
                                    <div class="form-group col-md-2">
                                        <span class="help-block">&nbsp;</span>

                                        <label for="inputZip">&nbsp;</label>
                                        <div class="d-flex"> 
                                            <asp:Button ID="btnInfoCapacidadHora" runat="server"  Text="Capacidad Hora"  class="btn btn-outline-primary mr-2" OnClick="btnInfoCapacidadHora_Click"/>
                                        </div>
		                            </div>

                                </div>
                            
                                  <div class="form-group">
                                    <div class="alert alert-warning" id="msjErrorAisv" runat="server" clientidmode="Static"><b>Error!</b> >Debe ingresar el número de la carga MRN......</div>
                                </div>

                                

                            </ContentTemplate>
                        </asp:UpdatePanel>

                        <asp:UpdatePanel ID="UPCAPACIDADHORA" runat="server"  UpdateMode="Conditional"  >  
                            <ContentTemplate>
        
                                <section class="wrapper2">
                
                                    <div id="Div1" runat="server" visible="true" >
                                        <div class="findresult" >
                                            <div class="booking" >
             

                                                <div class="form-group col-md-12"> 
                                                    <div class="form-title">CONFIGURACIÓN DE CAPACIDAD HORA</div>
                                                </div>
                                                  
                                                <div class="bokindetalle" style=" width:100%; overflow:auto">

                                                    <asp:GridView ID="dgvCapacidadHora" runat="server" AutoGenerateColumns="False"  
                                                        DataKeyNames="id"
                                                        GridLines="None" 
                                                        OnRowCommand="dgvCapacidaHora_RowCommand"   
                                                        OnRowDataBound="dgvCapacidaHora_RowDataBound"
                                                        PageSize="100"
                                                        AllowPaging="false"
                                                        CssClass="display table table-bordered invoice">
                                                        <PagerStyle HorizontalAlign = "Right" CssClass="display table table-bordered invoice"  />
                                                        <RowStyle  BackColor="#F0F0F0" />
                                                        <alternatingrowstyle  BackColor="#FFFFFF" />
                           
                                                        <Columns>
                                                            <asp:TemplateField>
                                                                <ItemTemplate>
                                                                    <%# Container.DataItemIndex + 1 %>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:BoundField DataField="id" HeaderText="ID" Visible="false"  HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>
                                                            <asp:BoundField DataField="idNave" Visible="false" HeaderText="REFERENCIA"  HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>
                                                            <asp:BoundField DataField="nave" Visible="false" HeaderText="NAVE" HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>
                                                            <asp:BoundField DataField="horaInicio" HeaderText="HORA INICIO"  HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>
                                                            <asp:BoundField DataField="horaFin"  HeaderText="HORA FIN" HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>
                                                            <asp:BoundField DataField="bodegaBloque"  HeaderText="BODEGA" HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>
                                                            <asp:BoundField DataField="idBloque" HeaderText="BLOQUE_ID" Visible="false"  SortExpression="idHold" HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>
                                                            <%--<asp:BoundField DataField="Bloque" HeaderText="BLOQUE"  SortExpression="Bloque" HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>   --%>                                                             
                                                            <%--<asp:BoundField DataField="box" HeaderText="BOX"  SortExpression="box" HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>                                                                --%>
                                           
                                                            <asp:BoundField DataField="reservado" HeaderText="RESERVA" SortExpression="reservado" HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>
                                                            <asp:BoundField DataField="disponible" HeaderText="DISPONIBLE" SortExpression="disponible" HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>

                                                            <asp:TemplateField HeaderText="BOXES" ItemStyle-CssClass="center hidden-phone">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="txtBoxesDet" runat="server" MaxLength="9" class="form-control" AutoPostBack="true"
                                                                        onkeypress="return soloLetras(event,'1234567890',true)" 
                                                                        placeholder="CANTIDAD"
                                                                        OnTextChanged="txtBoxesDet_TextChanged"
                                                                        onkeydown="moverFoco(event, '<%= dgvCapacidadHora.ClientID %>');"
                                                                        onfocus="seleccionarTextoYCursorAlFinal(this);"
                                                                        ></asp:TextBox>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="" ItemStyle-CssClass="center hidden-phone">
                                                                <ItemTemplate>
                                                                    <asp:Button runat="server" ID="btnSeleccionar" Height="30px" CommandName="Seleccionar" Text="Agregar" CommandArgument='<%# Bind("Id")  %>' class="btn btn-primary"/>                                                                       
                                                                </ItemTemplate>
                                                            </asp:TemplateField>

                                                        </Columns>
                                                    </asp:GridView>


                                                </div><!--adv-table-->
                                            <%--   </section>--%>
                                            </div><!--content-panel-->
                                        </div><!--col-lg-12-->

                         
                                    </div><!--row mt-->
                                     <div id="msjSinResultadosDetalle" runat="server" class="alert alert-info">
                                        No se encontraron resultados, 
                                        asegurese de haber seleccionado una fecha.
                                    </div>

                                </section><!--wrapper2-->
     
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnBuscar" />
                            </Triggers>
                        </asp:UpdatePanel>

                        <asp:UpdatePanel ID="UPTURNOS" runat="server"  UpdateMode="Conditional"  >  
                                <ContentTemplate>
                                    <script type="text/javascript">
                                        Sys.Application.add_load(BindFunctions); 
                                    </script>
        
                                    <section class="wrapper2">
                
                                        <%--<div id="Div2" runat="server" visible="true" >--%>
                                            <div class="findresult" >
                                                <div class="booking" >                                
                                      
                                                    <div class="bokindetalle" style=" width:100%; overflow:auto">

                                                        <div class="form-group col-md-12"> 
                                                            <div class="form-title">TURNOS ASIGNADOS</div>
                                                        </div>
                                     

                                                        <asp:Repeater ID="dgvTurnos" runat="server" onitemcommand ="dgvTurnos_ItemCommand" OnItemDataBound="dgvTurnos_ItemDataBound">
                                                            <HeaderTemplate>
                                                            <table cellpadding="0" cellspacing="0" border="0" class="table table-bordered table-sm table-contecon" id="dgvTurnos" width="100%">
                                                                <thead style="background: #B4B4B4; color: white">
                                                                    <tr>
                                                                        <th class="th-sm"></th>
                                                                        <th class="nover">ID</th>
                                                                        <th class="th-sm">DATE</th>
                                                                        <th class="th-sm">START</th>
                                                                        <th class="th-sm">END</th>
                                                                        <th class="th-sm">BOXES</th>
                                                                        <th class="th-sm">DRAG</th>
                                                                        <th class="th-sm">PENDING</th>
                                                                       <%-- <th class="th-sm">AISV</th>
                                                                        <th class="th-sm">DAE</th>
                                                                        <th class="th-sm">BOOKING</th>
                                                                        <th class="th-sm">IIE</th>
                                                                        <th class="th-sm">DAE</th>--%>
                                                                        <%--<th  class="nover">ESTADO</th>--%>
                                                                        <th class="center hidden-phone"></th>
                                                                       <%-- <th class="center hidden-phone"></th>
                                                                        <th class="center hidden-phone"></th>--%>
                                                                    </tr>
                                                                </thead>
                                                            <tbody>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <tr>
                                                                    <td class="center hidden-phone"><asp:Label Text='<%# Container.ItemIndex + 1 %>' ID="lblSecuencia" runat="server"  /></td>
                                                                    <td class="nover"><asp:Label Text='<%#Eval("idStowagePlanTurno")%>' ID="lblId" runat="server"  /></td>
                                                                    <td class="center hidden-phone"><asp:Label Text='<%#Eval("fecha", "{0:yyyy-MM-dd}")%>' ID="lblfecha" runat="server"  /></td>
                                                                    <td class="center hidden-phone"><asp:Label Text='<%#Eval("horaInicio")%>' ID="lblhoraInicio" runat="server"  /></td>
                                                                    <td class="center hidden-phone"><asp:Label Text='<%#Eval("horaFin")%>' ID="lblhoraFin" runat="server"  /></td>            
                                                                    <td class="center hidden-phone"><asp:Label Text='<%#Eval("box")%>' ID="lblbox" runat="server"  /></td>             
                                                                    <td class="center hidden-phone"><asp:Label Text='<%#Eval("reservado")%>' ID="Label1" runat="server"  /></td>             
                                                                    <td class="center hidden-phone"><asp:Label Text='<%#Eval("disponible")%>' ID="Label2" runat="server"  /></td>             
                                                                    <%--<td class="center hidden-phone"><asp:Label Text='<%#Eval("aisv")%>' ID="lblAisv" runat="server"  /></td>   
                                                                    <td class="center hidden-phone"><asp:Label Text='<%#Eval("dae")%>' ID="lbldae" runat="server"  /></td>            
                                                                    <td class="center hidden-phone"><asp:Label Text='<%#Eval("booking")%>' ID="lblbooking" runat="server"  /></td>           
                                                                    <td class="center hidden-phone"><asp:CheckBox ID="CHKIIE" runat="server" Checked='<%# Eval("IIEAutorizada") %>'  CssClass="center hidden-phone" Enabled="false"/></td>
                                                                    <td class="center hidden-phone"><asp:CheckBox ID="CHKDAE" runat="server" Checked='<%# Eval("daeAutorizada") %>'  CssClass="center hidden-phone" Enabled="false"/></td>
                                                           --%>
                                                                    <td class="center hidden-phone"> 
                                                                        <asp:Button runat="server" ID="btnQuitar" Height="30px" CommandName="Quitar" Text="Quitar"  CommandArgument='<%# Bind("idStowagePlanTurno") %>' class="btn btn-primary"/> <%--data-toggle="modal" data-target="#myModal"--%>
                                                                    </td> 
                                                                    
                                                                </tr>    
                                                            </ItemTemplate>
                                                            <FooterTemplate>
                                            
                                                            </tbody>
                                                                <tr>
                                                                    <td colspan="4">Total:</td>
                                                                        <td>
                                                                            <asp:Label ID="lblTotalBoxes" Text="0" Font-Bold="true" runat="server"></asp:Label>
                                                                        </td>
                                                                    <td colspan="6"></td>
                                                                </tr>
                                                            </table>
                                            
                                                            </FooterTemplate>
                                                        </asp:Repeater>

                                                    </div><!--adv-table-->
                                                <%--   </section>--%>
                                                </div><!--content-panel-->
                                            </div><!--col-lg-12-->

                         
                                        <%--</div>--%><!--row mt-->
                                         <div id="SinResultadoTurnos" runat="server" class="alert alert-info">
                                            No se encontraron resultados.
                                        </div>

                                    </section><!--wrapper2-->
     
                                </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnBuscar" />
                            </Triggers>
                        </asp:UpdatePanel>

                        
                        <asp:UpdatePanel ID="UPAISV" runat="server"  UpdateMode="Conditional"  >  
                                <ContentTemplate>
                                    <script type="text/javascript">
                                        Sys.Application.add_load(BindFunctions); 
                                    </script>
        
                                    <section class="wrapper2">
                
                                        <%--<div id="Div2" runat="server" visible="true" >--%>
                                            <div class="findresult" >
                                                <div class="booking" >                                
                                      
                                                    <div class="bokindetalle" style=" width:100%; overflow:auto">

                                                        <div class="form-group col-md-12"> 
                                                            <div class="form-title">DOCUMENTOS DE AISV GENERADOS</div>
                                                        </div>
                                     

                                                            <asp:GridView ID="dgvAisv" runat="server" AutoGenerateColumns="False" ShowFooter="True"  DataKeyNames="vbs_id_hora_cita" OnRowCommand="dgvAisv_RowCommand" OnRowDataBound="dgvAisv_RowDataBound"
                                                                        GridLines="None" 
                                                                        PageSize="20"
                                                                        AllowPaging="false"
                                                                        CssClass="table table-bordered invoice">
                                                                    <PagerStyle HorizontalAlign = "Right" CssClass="table table-bordered invoice"  />
                                                                    <RowStyle  BackColor="#F0F0F0" />
                                                                    <alternatingrowstyle  BackColor="#FFFFFF" />
                           
                                                                <Columns>     
                                                                    <asp:TemplateField>
                                                                        <ItemTemplate>
                                                                            <%# Container.DataItemIndex + 1 %>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:BoundField  DataField="vbs_id_hora_cita" HeaderText="ID"  Visible ="false" HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>
                                                                    <asp:BoundField DataField="fecha" HeaderText="FECHA"  HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>
                                                                    <asp:BoundField DataField="time" HeaderText="TIME"  HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>
                                                                    <asp:BoundField DataField="aisv_codigo" HeaderText="AISV"  HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>
                                                                    <%--<asp:BoundField DataField="vbs_box" HeaderText="BOX"  HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>--%>
                                                                     <asp:TemplateField HeaderText="BOX">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblTotalBoxesItem" runat="server" Text='<%# Eval("vbs_box") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <FooterTemplate>
                                                                            <asp:Label ID="lblTotalBoxesFooter" runat="server" Font-Bold="true"></asp:Label>
                                                                        </FooterTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:BoundField DataField="aisv_numero_booking" HeaderText="BOOKING"  HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>
                                                                    <asp:BoundField DataField="aisv_dae" HeaderText="CNTR" HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>
                                                                    <asp:BoundField DataField="aisv_cedul_chof"  HeaderText="LICENCIA" HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>
                                                                    <asp:BoundField DataField="aisv_nombr_chof" HeaderText="CHOFER"  HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>
                                                                    <asp:BoundField DataField="aisv_placa_vehi" HeaderText="PLACA" HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>
                                                                    <asp:BoundField DataField="aisv_estado" HeaderText="ESTADO" Visible="true" HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>
                                                                     <asp:TemplateField HeaderText="" ItemStyle-CssClass="center hidden-phone">
                                                                        <ItemTemplate>
                                                                            <asp:Button runat="server" ID="btnAutorizarIng" Height="30px" CommandName="Autorizar" Text="Autorizar Ingreso" CommandArgument='<%# Eval("vbs_id_hora_cita") + "," + Eval("aisv_codigo") %>' class="btn btn-primary" />                                                                       
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>

                                                                    


                                                                </Columns>
                                                            </asp:GridView>

                                                    </div><!--adv-table-->
                                                <%--   </section>--%>
                                                </div><!--content-panel-->
                                            </div><!--col-lg-12-->

                         
                                        <%--</div>--%><!--row mt-->
                                         <div id="SinResultadoAISV" runat="server" class="alert alert-info">
                                            No se encontraron resultados.
                                        </div>

                                    </section><!--wrapper2-->
     
                                </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnBuscar" />
                            </Triggers>
                        </asp:UpdatePanel>

                        
                     </div>
                    <div class="modal-footer d-flex justify-content-center">
                        <button type="button" class="btn btn-outline-primary " data-dismiss="modal">Salir</button>
                    </div>
                </div>
            </div>
        </div>

        <div id="myModal2" class="modal fade" tabindex="-1" role="dialog">

            <div class="modal-dialog" role="document" style="max-width: 1000px"> <!-- Este tag style controla el ancho del modal -->
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title">SERVICIOS ADICIONALES</h5>
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                        </button>
                    </div>
                    <div class="modal-body">

                        <asp:UpdatePanel ID="UPLIQ" runat="server" UpdateMode="Conditional" >  
                            <ContentTemplate>
                                <asp:Panel ID="Panel3" runat="server">      
                                    <div id="div_Codigos1" style="visibility:hidden;">
                                        <asp:HiddenField ID="hdf_idStowageDet" runat="server" />
                                    </div>
                                                
                                    <div class="form-row">

                                        <div class="form-group col-md-5">
                                            <label for="inputAddress">Shipper:<span style="color: #FF0000; font-weight: bold;"></span></label>
                                            <div class="d-flex">
                                                <asp:TextBox ID="txtLiqShipper"  runat="server" class="form-control"   placeholder=""  Font-Bold="false" disabled></asp:TextBox>
                                            </div>
                                        </div>

                                        <div class="form-group   col-md-3"> 
                                            <label for="inputAddress">Cantidad<span style="color: #FF0000; font-weight: bold;"></span></label>
                                            <asp:TextBox ID="txtLiqCantidad" runat="server" class="form-control" 
                                                onkeypress="return soloLetras(event,'1234567890',true)" 
                                                placeholder="CANTIDAD"
                                                ></asp:TextBox>
                                        </div>
                                       
                                        <div class="form-group col-md-4"> 
                                            <label for="inputAddress">Servicio :<span style="color: #FF0000; font-weight: bold;"></span></label>
                                            <div class="d-flex">
                                                <asp:DropDownList ID="cmbLiqServicio" class="form-control" runat="server" Font-Size="Medium"  Font-Bold="true" ></asp:DropDownList>
                                            </div>
                                        </div>

                                       <%-- <div class="form-group col-md-3">
                                            <label for="inputAddress">DOC (AISV/BOOKING/DAE):<span style="color: #FF0000; font-weight: bold;"></span></label>
                                            <div class="d-flex">
                                                <asp:TextBox ID="txtLiqAisv"  runat="server" class="form-control"   placeholder=""  Font-Bold="false" ></asp:TextBox>
                                            </div>
                                        </div>--%>
                                        <div class="form-group col-md-3"> 
                                            <label for="inputAddress">AISV :<span style="color: #FF0000; font-weight: bold;"></span></label>
                                            <div class="d-flex">
                                                <asp:DropDownList ID="cmbLiqAisv" class="form-control" runat="server" Font-Size="Medium"  Font-Bold="true" ></asp:DropDownList>
                                            </div>
                                        </div>

                                       <div class="form-group col-md-9">
                                            <label for="inputZip">Comentario</label>
                                            <asp:TextBox ID="txtLiqComentario" runat="server" class="form-control"  MaxLength="300"  onkeypress="return soloLetras(event,'abcdefghijklmnopqrstuvwxyz0123456789-_')" placeholder="INGRESE COMENTARIO"></asp:TextBox>
                                        </div>
                                                
                                    </div>
                                            
                                </asp:Panel>

                                <div></div>
                                <br />

                                <div class="row">
                                    <div class="col-md-12 d-flex justify-content-center">
                                        <asp:Button ID="btnAddLiquidacion" runat="server" class="btn btn-primary"  Text="Agregar"  OnClientClick="return mostrarloader('1')" OnClick="btnAddLiquidacion_Click"  />
                                        <img alt="loading.." src="../lib/file-uploader/img/loading.gif" height="32px" width="32px"  id="ImgCargas1" class="nover"   />
                                        <span id="imagenes"></span>
                                    </div>
                                </div>

                                <br/>
                                <div class="row">
                                    <div class="col-md-12 d-flex justify-content-center">
                                        <div class="alert alert-warning" id="msjErrorLiquidacion" runat="server" clientidmode="Static"><b>Error!</b> >......</div>
                                    </div>
                                </div>
                      
                                <section class="wrapper2">
                                    <%--<div class="row mb"> --%>
                                        <div class="content-panel">
                                            <div class="bokindetalle" style="width:100%; overflow:auto">       
                                                <asp:GridView ID="dgvLiquidacion" runat="server" AutoGenerateColumns="False"  DataKeyNames="idliquidacion"
                                                                        GridLines="None" 
                                                                        PageSize="20"
                                                                        AllowPaging="false"
                                                                        CssClass="table table-bordered invoice">
                                                        <PagerStyle HorizontalAlign = "Right" CssClass="table table-bordered invoice"  />
                                                        <RowStyle  BackColor="#F0F0F0" />
                                                        <alternatingrowstyle  BackColor="#FFFFFF" />
                           
                                                    <Columns>     
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <%# Container.DataItemIndex + 1 %>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="idliquidacion" HeaderText="idliquidacion"  Visible ="false" HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>
                                                        <asp:BoundField DataField="idStowageDet" HeaderText="idStowageDet"  Visible ="false" HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>
                                                        <asp:BoundField DataField="aisv"  HeaderText="DOC" Visible ="true" SortExpression="aisv" HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>
                                                        <asp:BoundField DataField="cantidad" HeaderText="CANT"  HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>
                                                        <%--<asp:BoundField DataField="peso" HeaderText="PESO"  HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>
                                                        <asp:BoundField DataField="cubicaje" HeaderText="CUBICAJE" HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>
                                                        <asp:BoundField DataField="consignatario" Visible ="false" HeaderText="CLIENTE" HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>
                                                        <asp:BoundField Visible="false" DataField="ubicacion" HeaderText="UBICACIÓN"  HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>--%>
                                                        <asp:BoundField DataField="Servicio" HeaderText="SERVICIO" HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>
                                                        
                                                        <asp:TemplateField HeaderText="ESTADO" ItemStyle-CssClass="center hidden-phone" >
                                                            <ItemTemplate>
                                                                <asp:UpdatePanel ID="UPOVERSIZED" runat="server" ChildrenAsTriggers="true">
                                                                    <ContentTemplate>
                                                                        <asp:CheckBox ID="CHKOVERSIZED" runat="server" Checked='<%# Bind("estados") %>'  CssClass="center hidden-phone" Enabled="false"/>
                                                                    </ContentTemplate>
                                                                </asp:UpdatePanel>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>

                                                        <asp:BoundField DataField="comentario" HeaderText="COMENTARIO" HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>
                                                        <asp:BoundField DataField="usuarioCrea" HeaderText="USUARIO" HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>
                                                        <asp:BoundField DataField="fechaCreacion" HeaderText="REGISTRA"   HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>
                                                    </Columns>
                                                </asp:GridView>
                                            </div>

                                        </div><!--content-panel-->
                                    <%--</div>--%><!--row mb-->
                                </section><!--wrapper2-->

                            </ContentTemplate>   
                        </asp:UpdatePanel>
                     </div>
                    <div class="modal-footer d-flex justify-content-center">
                        <button type="button" class="btn btn-outline-primary " data-dismiss="modal">Salir</button>
                    </div>
                </div>
            </div>
        </div>

        <asp:UpdatePanel ID="UPMENSAJE" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">  
             <ContentTemplate>

                  <div class="modal fade" id="myModal3" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
                    <div class="modal-dialog">
                      <div class="modal-content">
                        <div class="modal-header">
                            <h4 class="modal-title" id="myModalLabel">Confirmar Publicación</h4>
                            <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                          
                        </div>
                        <div class="modal-body">
                            <br>
                            </br/>
                            Si usted da click en SI, se procederá a publicar los turnos configurados  &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                            <br>
                            </br/>
                        </div>
                        <div class="modal-footer">
                             <asp:Button ID="btnPublicar" runat="server" class="btn btn-default"  Text="SI" OnClick="btnPublicar_Click"  UseSubmitBehavior="false" data-dismiss="modal" />
                          <button type="button" class="btn btn-primary" data-dismiss="modal">NO</button>
                        </div>
                      </div>
                    </div>
                  </div>
             </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnPublicar" />
            </Triggers>
         </asp:UpdatePanel>   
        
        <asp:UpdatePanel ID="UPAUTORIZAR" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">  
             <ContentTemplate>

                  <div class="modal fade" id="myModal5" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
                    <div class="modal-dialog">
                      <div class="modal-content">
                        <div class="modal-header">
                            <h4 class="modal-title" id="myModalLabel2">Autorizar Ingreso</h4>
                            <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                          
                        </div>
                        <div class="modal-body">
                            <br>
                            </br/>
                            Si usted da click en SI, se procederá a autorizar el ingreso del vehiculo registrado en el AISV seleccionado <label for="inputAddress" id="lblAISV" runat="server"><span style="color: #FF0000; font-weight: bold;"></span></label> &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                            <%--<br>
                            </br/>--%>

                           <%-- <div class="form-row">
                                 <div class="form-group col-md-12"> 
                                    <label for="inputAddress">Comentario<span style="color: #FF0000; font-weight: bold;"></span></label>
                                    <div class="d-flex"> 
                                        <asp:TextBox TextMode="MultiLine"  class="form-control"  ID="txtComentarioAutoriza1" runat="server" MaxLength="100" onkeypress="return soloLetras(event,'abcdefghijklmnopqrstuvwxyz01234567890/')"></asp:TextBox>
                                    </div>
                                </div>
                            </div>--%>

                        </div>
                        <div class="modal-footer">
                             <asp:Button ID="btnAutorizarIngreso" runat="server" class="btn btn-default"  Text="SI" OnClick="btnAutorizarIngreso_Click"  UseSubmitBehavior="false" data-dismiss="modal" />
                          <button type="button" class="btn btn-primary" data-dismiss="modal">NO</button>
                        </div>
                      </div>
                    </div>
                  </div>
             </ContentTemplate>
            <Triggers>
                <%--<asp:AsyncPostBackTrigger ControlID="btnAutorizarEdicion" />--%>
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
                document.getElementById("ImgCargas").className = 'ver';
                document.getElementById("ImgCargas1").className = 'ver';
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
                document.getElementById("ImgCargas").className = 'nover';
                document.getElementById("ImgCargas1").className = 'nover';
            }
            else {
                document.getElementById("ImgCargaDet").className='nover';
            }

             } catch (e) {
                alert('Lamentamos comunicarle que ha ocurrido el siguiente problema:\n' + e.Message);
            }
    }


      function prepareObjectRuc() {
            try {
                document.getElementById("loader3").className = '';
                var vals = document.getElementById('<%=TXTMRN.ClientID %>').value;
                if (vals == null || vals == undefined || vals == '') {
                    alert('¡ Escriba el MRN.');
                    document.getElementById("loader3").className = 'nover';
                    document.getElementById('<%=TXTMRN.ClientID %>').focus();
                    return false;
                }
                return true;
            } catch (e) {
                alert('Lamentamos comunicarle que ha ocurrido el siguiente problema:\n' + e.Message);
            }
    }

    function popupCallback(catalogo)
    {
        if (catalogo == null || catalogo == undefined) {
            alert('Hubo un problema al setaar un objeto de catalogo');
            return;
        }

        this.document.getElementById('<%= txtNave.ClientID %>').value = catalogo.codigo;
        this.document.getElementById('<%= txtDescripcionNave.ClientID %>').value = catalogo.descripcion;


        //document.getElementById("btnBuscar").click();
        //var btnBuscar = document.getElementById('<%= btnBuscar.ClientID %>');
        //btnBuscar.click();
            ////si catalogos es booking
            //if (catalogo == 'bk') {
            //    document.getElementById('numbook').textContent = objeto.nbr;
            //    document.getElementById('nbrboo').value = objeto.nbr;
            //    return;
            //}

            ////si catalogos es booking
            //if (catalogo == 'cc') {
            //    document.getElementById('txtfecha').textContent = objeto.fecha;
            //    document.getElementById('xfecha').value = objeto.fecha;
            //    return;
            //}

    }

</script>

    <script type="text/javascript">
    function moverFoco(event, gridId) {
        if (event.key === "Enter") {
            var target = event.target;
            var cellIndex = target.parentElement.cellIndex;
            var rowIndex = target.parentElement.parentElement.rowIndex;
             //alert(rowIndex );
            var grid = document.getElementById('<%= dgvCapacidadHora.ClientID %>');
                var nextRow = grid.rows[rowIndex + 1];
                //alert(nextRow );
                if (nextRow) {
                    var nextCell = nextRow.cells[cellIndex];
                    var nextTextBox = nextCell.querySelector("input[type='text']");

                    if (nextTextBox) {
                        nextTextBox.focus();
                        event.preventDefault(); // Evitar el comportamiento predeterminado del Enter
                    }
                }
            }
        }



        $(document).ready(function () {
            $('.datetimepicker').datetimepicker().datetimepicker({ lang: 'es', step: 30, format: 'm/d/Y H:i' });
        });
        $(document).ready(function () {
            $('.datetimepickerAlt').datetimepicker().datetimepicker({ lang: 'es', closeOnDateSelect: true, timepicker: false, step: 30, format: 'm/d/Y' });
        });

        $(document).ready(function () {
            $('.datepicker').datetimepicker().datetimepicker({ lang: 'es', timepicker: false, format: 'm/d/Y', closeOnDateSelect: true, minDate: '0' });

        });

    </script>
    
   

</asp:Content>