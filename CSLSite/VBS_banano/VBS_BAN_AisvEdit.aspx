
<%@ Page Title="Pre Stowage" Language="C#" MasterPageFile="~/site.Master" AutoEventWireup="true" CodeBehind="VBS_BAN_AisvEdit.aspx.cs" Inherits="CSLSite.VBS_BAN_AisvEdit" %>
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
                $('#tablePagination').DataTable(
                {
       
                        language: {
                            "lengthMenu": "_MENU_ REGISTROS POR PAGINA",
                            "zeroRecords": "No se encontraron resultados",
                            "info": "Mostrando registros del _START_ al _END_ de un total de _TOTAL_ registros",
                            "infoEmpty": "Mostrando registros del 0 al 0 de un total de 0 registros",
                            "infoFiltered": "(filtrado de un total de _MAX_ registros)",
                            "sSearch": "Filtrar:",
			                    "sProcessing":"Procesando...",

                        },
                    //para usar los botones   
                    responsive: "true",
                    dom: 'Bfrtilp',    
                    buttons: [  
                        {
				            extend:    'excel',
				            text:      '<i class="fa fa-file-excel-o"></i> ',
				            titleAttr: 'Exportar a Excel',
				            className: 'btn btn-primary'
			            },
			            {
				            extend:    'pdf',
				            text:      '<i class="fa fa-file-pdf-o"></i> ',
				            titleAttr: 'Exportar a PDF',
                            className: 'btn btn-primary',
                            orientation: 'landscape',
                            pageSize: 'LEGAL'
			            },
			            {
				            extend:    'print',
				            text:      '<i class="fa fa-print"></i> ',
				            titleAttr: 'Imprimir',
                            className: 'btn btn-primary',
                            size: 'landscape'
			            },
                    ],	 
                        pageLength: 100,
                    initComplete: function() {
                        // Alínea los botones a la derecha con CSS
                        //$('.dt-buttons').css('float', 'right');
                        // Alínea el filtro a la derecha con CSS
                        //$('.dataTables_filter').css({'float': 'right'});

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

    <div class="mt-4">         
        <nav class="mt-4" aria-label="breadcrumb">
            <ol class="breadcrumb">
                <li class="breadcrumb-item" id="opcion_principal" runat="server"><a href="#">VBS Banano</a></li>
                <li class="breadcrumb-item active" aria-current="page" id="sub_opcion" runat="server">PRE STOWAGE CONFIGURATION</li>
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

        <asp:UpdatePanel ID="UPBOTONES" runat="server" UpdateMode="Conditional" >  
            <ContentTemplate> 
                

                <div class="form-group col-md-12"> 
                    <div class="form-title">DETALLE DE CONFIGURACIÓN DEL STOWAGE PLAN</div>
                </div>
                                  
                
                <div class="form-group">
                    <div class="alert alert-warning" id="banmsg_det" runat="server" clientidmode="Static"><b>Error!</b> >Debe ingresar el número de la carga MRN......</div>
                </div>

                <div class="form-group col-md-12"> 
                    <div class="row">
                        <div class="col-md-12 d-flex justify-content-center">
                            <asp:Button ID="btnLimpiarDataAdd" runat="server" class="btn btn-outline-primary mr-2"  Text="Limpiar" OnClick="btnLimpiarDataAdd_Click"   />
                            <asp:Button ID="btnGrabar" runat="server" class="btn btn-primary" Text="Agregar" OnClientClick="return mostrarloader('2')" OnClick="btnGrabar_Click"   />
                            <img alt="loading.." src="../lib/file-uploader/img/loading.gif" height="32px" width="32px"  id="ImgCargaDet" class="nover"  />
                        </div>
                    </div><!--btn-group-justified-->
                </div><!--showback-->
            </ContentTemplate>
        </asp:UpdatePanel>

        <asp:UpdatePanel ID="UPDETALLE" runat="server"  UpdateMode="Conditional"  >  
            <ContentTemplate>
                <script type="text/javascript">
                    Sys.Application.add_load(BindFunctions); 
                </script>
        
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
                                                    <th class="th-sm">DATE</th>
                                                    <th class="nover">TIME</th>
                                                    <th class="th-sm">SHIPPER</th>
                                                    <th class="th-sm">BRAND</th>
                                                    <th class="nover">BOXES</th>
                                                    <th class="nover">DRAG</th>
                                                    <th class="th-sm">PENDING</th>
                                                    <th class="th-sm">AISV</th>
                                                    <th class="th-sm">DAE</th>
                                                    <th class="nover">BOOKING</th>
                                                    <th class="nover">AUT DAE</th>
                                                    <th class="nover">AUT IIE</th>
                                                    <th class="th-sm">PLACA</th>
                                                    <th class="nover">CHOFER</th>
                                                    <th class="center hidden-phone"></th>
                                                    <th class="center hidden-phone"></th>
                                                </tr>
                                            </thead>
                                        <tbody>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <tr>
                                                <td class="center hidden-phone"><asp:Label Text='<%# Container.ItemIndex + 1 %>' ID="lblSecuencia" runat="server"  /></td>
                                                <td class="nover"><asp:Label Text='<%#Eval("id")%>' ID="lblId" runat="server"  /></td>
                                                <td class="center hidden-phone"><asp:Label Text='<%#Eval("fecha")%>' ID="lblfecha" runat="server"  /></td>
                                                <td class="nover"><asp:Label Text='<%#Eval("time")%>' ID="lbltime" runat="server"  /></td>
                                                <td class="center hidden-phone"><asp:Label Text='<%#Eval("exportador")%>' ID="lblexportador" runat="server"  /></td>   
                                                <td class="center hidden-phone"><asp:Label Text='<%#Eval("marca")%>' ID="lblmarca" runat="server"  /></td>   
                                                <td class="nover"><asp:Label Text='<%#Eval("box")%>' ID="lblbox" runat="server"  /></td>             
                                                <td class="nover"><asp:Label Text='<%#Eval("arrastre")%>' ID="lblarrastre" runat="server"  /></td>             
                                                <td class="center hidden-phone"><asp:Label Text='<%#Eval("pendiente")%>' ID="lblpendiente" runat="server"  /></td>      
                                                <td class="center hidden-phone"><asp:Label Text='<%#Eval("aisv")%>' ID="lblaisv" runat="server"  /></td>           
                                                <td class="center hidden-phone"><asp:Label Text='<%#Eval("dae")%>' ID="lbldae" runat="server"  /></td>   
                                                <td class="nover"><asp:Label Text='<%#Eval("booking")%>' ID="lblbooking" runat="server"  /></td>   
                                                <td  class="nover"> <asp:CheckBox ID="lbliieAut" runat="server" Checked='<%# Eval("iieAut") %>'  CssClass="center hidden-phone" Enabled="false"/></td>
                                                <td  class="nover"> <asp:CheckBox ID="lbldaeAut" runat="server" Checked='<%# Eval("daeAut") %>'  CssClass="center hidden-phone" Enabled="false"/></td>
                                                <td class="center hidden-phone"><asp:Label Text='<%#Eval("placa")%>' ID="lblplaca" runat="server"  /></td>   
                                                <td class="nover"><asp:Label Text='<%#Eval("chofer")%>' ID="lblchofer" runat="server"  /></td>   
                                                <td class="center hidden-phone">  
                                                    <asp:Button runat="server" ID="btnEditar" Height="30px" CommandName="Editar" Text="Editar"  CommandArgument='<%# Bind("id") %>' class="btn btn-outline-primary mr-2"  data-toggle="modal" data-target="#myModal3"  />
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

                         
                    </div><!--row mt-->
                     <div id="sinresultado" runat="server" class="alert alert-info">
                        No se encontraron resultados, el documento Stowage esta listo para su configuración inicial. debe agregar informacion al detalle
                    </div>

                </section><!--wrapper2-->
     
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnBuscar" />
            </Triggers>
        </asp:UpdatePanel>

        <div id="myModal1" class="modal fade" tabindex="-1" role="dialog">

            <div class="modal-dialog" role="document" style="max-width: 1400px"> <!-- Este tag style controla el ancho del modal -->
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title">LISTA DE AISV</h5>
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                        </button>
                    </div>
                    <div class="modal-body">

                        <asp:UpdatePanel ID="UPAISV" runat="server" UpdateMode="Conditional" >  
                            <ContentTemplate>
                                <asp:Panel ID="Panel1" runat="server">         
                                                
                                    <div class="form-row">

                                        <div class="form-group col-md-2">
                                            <label for="inputAddress">ITEM :<span style="color: #FF0000; font-weight: bold;"></span></label>
                                            <div class="d-flex">
                                                <asp:TextBox ID="txtIdDetalle"  runat="server" class="form-control"   placeholder=""  Font-Bold="false" disabled></asp:TextBox>
                                            </div>
                                        </div>

                                        <div class="form-group col-md-2">
                                            <label for="inputAddress">DATE :<span style="color: #FF0000; font-weight: bold;"></span></label>
                                            <div class="d-flex">
                                                <asp:TextBox ID="txtDetAisvFecha"  runat="server" class="form-control"   placeholder=""  Font-Bold="false" disabled></asp:TextBox>
                                            </div>
                                        </div>

                                        <div class="form-group   col-md-3"> 
                                            <label for="inputAddress">TIME<span style="color: #FF0000; font-weight: bold;"></span></label>
                                            <asp:TextBox ID="txtDetAisvTime" runat="server" class="form-control" disabled
                                                onkeypress="return soloLetras(event,'1234567890',true)" 
                                                placeholder="TIME"
                                                ></asp:TextBox>
                                        </div>

                                        <div class="form-group   col-md-3"> 
                                            <label for="inputAddress">HOLD<span style="color: #FF0000; font-weight: bold;"></span></label>
                                            <asp:TextBox ID="txtDetAisvHold" runat="server" MaxLength="3" class="form-control" disabled
                                                onkeypress="return soloLetras(event,'1234567890',true)" 
                                                placeholder="HOLD"
                                                ></asp:TextBox>
                                        </div>

                                        <div class="form-group   col-md-2"> 
                                            <label for="inputAddress">BOXES<span style="color: #FF0000; font-weight: bold;"></span></label>
                                            <asp:TextBox ID="txtDetAisvCantidad" runat="server" MaxLength="3" class="form-control" disabled
                                                onkeypress="return soloLetras(event,'1234567890.',true)" 
                                                placeholder="BOXES"
                                                ></asp:TextBox>
                                        </div>
                                                
                                    </div>
                                            
                                </asp:Panel>

                                <div></div>
                                <br />

                                <br/>
                                <div class="row">
                                    <div class="col-md-12 d-flex justify-content-center">
                                        <div class="alert alert-warning" id="msjErrorLiquidacion" runat="server" clientidmode="Static"><b>Error!</b> >......</div>
                                    </div>
                                </div>
                      
                                <section class="wrapper2">
                                 
                                        <div id="SinResultadoAISV" runat="server" class="alert alert-info">
                                            No se encontraron resultados de AISV generados para este turno.
                                        </div>
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
        
        <div id="myModal3" class="modal fade" tabindex="-1" role="dialog">

            <div class="modal-dialog" role="document" style="max-width: 1000px"> <!-- Este tag style controla el ancho del modal -->
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title">DATOS DEL ITEM LOADING</h5>
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

                                                <div class="form-group col-md-2">
                                                    <span class="help-block">&nbsp;</span>

                                                    <label for="inputZip">HOLD</label>
                                                    <div class="d-flex"> 
                                                        <asp:DropDownList  ID="cmbDetHold" class="form-control"  runat="server" Font-Size="Medium" Font-Bold="true">
                                                        </asp:DropDownList>
                                                    </div>
		                                        </div>
                                                <div class="form-group col-md-2">
                                                    <span class="help-block">&nbsp;</span>

                                                    <label for="inputZip">Piso</label>
                                                    <div class="d-flex"> 
                                                        <asp:TextBox class="form-control"  ID="txtDetPiso" runat="server" MaxLength="50" onkeypress="return soloLetras(event,'abcdefghijklmnopqrstuvwxyz01234567890/')"></asp:TextBox>
                                                    </div>
		                                        </div>

                                                <div class="form-group col-md-6">
                                                    <span class="help-block">&nbsp;</span>

                                                    <label for="inputZip">Exportador</label>
                                                    <div class="d-flex"> 
                                                        <asp:DropDownList  ID="cmbDetExportador" class="form-control"  runat="server" Font-Size="Medium" Font-Bold="true">
                                                        </asp:DropDownList>
                                                    </div>
		                                        </div>

                                                <div class="form-group col-md-2">
                                                    <span class="help-block">&nbsp;</span>

                                                    <label for="inputZip">Cargo</label>
                                                    <div class="d-flex"> 
                                                        <asp:DropDownList  ID="cmbDetCargo" class="form-control"  runat="server" Font-Size="Medium" Font-Bold="true"   >
                                                        </asp:DropDownList>
                                                    </div>
		                                        </div>

                                                <div class="form-group col-md-4">
                                                    <span class="help-block">&nbsp;</span>

                                                    <label for="inputZip">Marca</label>
                                                    <div class="d-flex"> 
                                                        <asp:DropDownList  ID="cmbDetMarca" class="form-control"  runat="server" Font-Size="Medium" Font-Bold="true">
                                                        </asp:DropDownList>
                                                    </div>
		                                        </div>

                                                <div class="form-group col-md-6">
                                                    <span class="help-block">&nbsp;</span>

                                                    <label for="inputZip">Consignatario</label>
                                                    <div class="d-flex"> 
                                                        <asp:DropDownList  ID="cmbDetConsignatario" class="form-control"  runat="server" Font-Size="Medium" Font-Bold="true">
                                                        </asp:DropDownList>
                                                    </div>
		                                        </div>

                                                <div class="form-group col-md-2">
                                                    <span class="help-block">&nbsp;</span>

                                                    <label for="inputZip">Cantidad</label>
                                                    <div class="d-flex"> 
                                                        <asp:TextBox ID="txtDetCantidad" runat="server" MaxLength="9" class="form-control" onkeypress="return soloLetras(event,'1234567890',true)" placeholder="CANTIDAD"></asp:TextBox>
                                                    </div>
		                                        </div>

                                                <div class="form-group col-md-4"> 
                                                    <label for="inputAddress">Bodega :<span style="color: #FF0000; font-weight: bold;"></span></label>
                                                    <div class="d-flex">
                                                        <asp:DropDownList ID="cmbDetBodega"  class="form-control"  runat="server" Font-Size="Medium" AutoPostBack="true" Font-Bold="true" OnSelectedIndexChanged="cmbDetBodega_SelectedIndexChanged" >
                                                        </asp:DropDownList>
                                                        <a class="tooltip" ><span class="classic" >Nombre de Bodega</span><img alt='' src='../shared/imgs/info.gif' class='datainfo'/></a>
                                                    </div>
                                                </div>

                                                 <div class="form-group col-md-4"> 
                                                    <label for="inputAddress">Bloque :<span style="color: #FF0000; font-weight: bold;"></span></label>
                                                    <div class="d-flex">
                                                        <asp:DropDownList ID="cmbDetBloque" class="form-control"  runat="server" Font-Size="Medium" AutoPostBack="false" Font-Bold="true" >
                                                        </asp:DropDownList>
                                                        <a class="tooltip" ><span class="classic" >Nombre de Bloque</span><img alt='' src='../shared/imgs/info.gif' class='datainfo'/></a>
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
                                            <%--<div class="row mb"> --%>
                                                <div class="content-panel">
                                                    <%--<div class="form-row">
                                            
                                                       <div class="form-group col-md-12"> 
                                                            <label for="inputAddress">Ubicacion<span style="color: #FF0000; font-weight: bold;"></span></label>
                                                            <div class="d-flex"> 
                                                                <asp:TextBox  class="form-control"  ID="txtUbicacion" disabled runat="server" MaxLength="100" onkeypress="return soloLetras(event,'abcdefghijklmnopqrstuvwxyz01234567890/')"></asp:TextBox>
                                                            </div>
                                                        </div>

                                                    </div>


                                                    <div></div>--%>

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

        <asp:UpdatePanel ID="UPActualizar" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">  
             <ContentTemplate>

                  <div class="modal fade" id="myModal4" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
                    <div class="modal-dialog">
                      <div class="modal-content">
                        <div class="modal-header">
                            <h4 class="modal-title" id="myModalLabel1">Confirmar Actualización</h4>
                            <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                          
                        </div>
                        <div class="modal-body">
                            <br>
                            </br/>
                            Si usted da click en SI, se procederá a autorizar la edición del Loading Program durante las 24 horas siguientes  &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                            <br>
                            </br/>
                        </div>
                        <div class="modal-footer">
                             <asp:Button ID="btnAutorizarEdicion" runat="server" class="btn btn-default"  Text="SI" OnClick="btnAutorizarEdicion_Click"  UseSubmitBehavior="false" data-dismiss="modal" />
                          <button type="button" class="btn btn-primary" data-dismiss="modal">NO</button>
                        </div>
                      </div>
                    </div>
                  </div>
             </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnAutorizarEdicion" />
            </Triggers>
         </asp:UpdatePanel>

    </div>

    <script type="text/javascript" src="../lib/datatables/datatables.min.js"></script>
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

      
        

       <%-- function cboHorarioInicialChanged() {
            var cboHorarioInicial = document.getElementById("<%= cboHorarioInicial.ClientID %>");
            var horarioInicialId = cboHorarioInicial.value;

            // Llamar al servidor utilizando AJAX
            var xhr = new XMLHttpRequest();
            xhr.open("GET", "VBS_BAN_Loading.aspx?horarioInicialId=" + horarioInicialId, true);
            xhr.onreadystatechange = function () {
                if (xhr.readyState === 4 && xhr.status === 200) {
                    // La respuesta del servidor ha sido recibida correctamente
                    var cboHorarioFinal = document.getElementById("<%= cboHorarioFinal.ClientID %>");
                    cboHorarioFinal.innerHTML = "";

                    // Agregar las nuevas opciones al DropDownList
                    cboHorarioFinal.innerHTML = xhr.responseText;

                }
            };
            xhr.send();
        }--%>

    </script>


    <script type="text/javascript">
            

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