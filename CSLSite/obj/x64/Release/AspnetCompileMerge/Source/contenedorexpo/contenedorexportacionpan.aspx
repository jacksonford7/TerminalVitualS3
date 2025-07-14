<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="contenedorexportacionpan.aspx.cs" Inherits="CSLSite.contenedorexpo.contenedorexportacionpan" %>
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

    <link href="../shared/estilo/tables.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/aisv-general.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/work.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/catalogos.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/info.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/proforma.css" rel="stylesheet" type="text/css" />

    <link href="../img/favicon2.png" rel="icon"/>
    <link href="../img/icono.png" rel="apple-touch-icon"/>
    <link href="../css/bootstrap.min.css" rel="stylesheet"/>
    <link href="../css/dashboard.css" rel="stylesheet"/>
    <link href="../css/icons.css" rel="stylesheet"/>
    <link href="../css/style.css" rel="stylesheet"/>
    <link href="https://fonts.googleapis.com/css2?family=Roboto:wght@300;400;500;700&display=swap" rel="stylesheet"/>

    <link href="../shared/estilo/Reset.css" rel="stylesheet" />
    <link href="../shared/estilo/jquery.datetimepicker.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/tables.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/info.css" rel="stylesheet" type="text/css" />




  
    <meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no"/>
    <link rel="canonical" href="https://getbootstrap.com/docs/4.5/examples/dashboard/"/>
    <!-- Custom styles for this template -->
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/5.15.1/css/all.min.css" integrity="sha512-+4zCK9k+qNFUR5X+cKL9EIR+ZOhtIloNl9GIKS57V1MyNsYpYcUrUeQc9vNfzsWfV28IaLL3i96P9sdNyeRssA==" crossorigin="anonymous" />
    <link href="../css/datatables.min.css" rel="stylesheet"/>


   <%-- <link href="../img/favicon2.png" rel="icon"/>
    <link href="../img/icono.png" rel="apple-touch-icon"/>
    <!-- Bootstrap core CSS -->
    <link href="../lib/bootstrap/css/bootstrap.min.css" rel="stylesheet"/>
    <!--external css-->
    <link href="../lib/font-awesome/css/font-awesome.css" rel="stylesheet" />
    <link rel="stylesheet" type="text/css" href="../lib/gritter/css/jquery.gritter.css" />


    <link href="../lib/advanced-datatable/css/demo_page.css" rel="stylesheet" />
    <link href="../lib/advanced-datatable/css/demo_table.css" rel="stylesheet" />
    <link rel="stylesheet" href="../lib/advanced-datatable/css/DT_bootstrap.css" />

   
    <link rel="stylesheet" type="text/css" href="../lib/bootstrap-datepicker/css/datepicker.css" />
    <link rel="stylesheet" type="text/css" href="../lib/bootstrap-daterangepicker/daterangepicker.css" />
    <link rel="stylesheet" type="text/css" href="../lib/bootstrap-timepicker/compiled/timepicker.css" />
    <link rel="stylesheet" type="text/css" href="../lib/bootstrap-datetimepicker/datertimepicker.css" />

    <link rel="stylesheet" href="../lib/file-uploader/css/jquery.fileupload.css"/>
    <link rel="stylesheet" href="../lib/file-uploader/css/jquery.fileupload-ui.css"/>


    <link href="../css/style.css" rel="stylesheet"/>
    <link href="../css/style-responsive.css" rel="stylesheet"/>
    <link href="../css/pagination.css" rel="stylesheet"/>  --%>

    <script type="text/javascript">
        function BindFunctions() {
            $(document).ready(function () {
                /*
                 * Insert a 'details' column to the table
                 */
                var nCloneTh = document.createElement('th');
                var nCloneTd = document.createElement('td');
                nCloneTd.innerHTML = '<img src="../lib/advanced-datatable/images/details_open.png">';
                nCloneTd.className = "center";

                $('#<%= tablePagination.ClientID %> thead tr').each(function() {
          });

          $('#<%= tablePagination.ClientID %> tbody tr').each(function() {
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

    <div class="mt-4">         
        <nav class="mt-4" aria-label="breadcrumb">
            <ol class="breadcrumb">
                <li class="breadcrumb-item" id="opcion_principal" runat="server"><a href="#">Expo Contenedores</a></li>
                <li class="breadcrumb-item active" aria-current="page" id="sub_opcion" runat="server">FACTURACIÓN DE EXPORTACIÓN PAN</li>
            </ol>
        </nav>
    </div>

    <div class="dashboard-container p-4" id="cuerpo" runat="server">
        
        <div class="form-title">
            CRITERIO DE BUSQUEDA
        </div>

        <asp:UpdatePanel ID="UPCARGA" runat="server"  UpdateMode="Conditional" ChildrenAsTriggers="true">
            <ContentTemplate>
                <asp:Panel ID="PanelCualquiera" runat="server" DefaultButton ="BtnBuscar">
                       <div >
                        <div class="form-row" >
                                     
                            <div class="form-group col-md-4">
                                <br />
                                <div class="d-flex">
                                    &nbsp;
                                    <asp:TextBox ID="TXTNAVE" runat="server" class="form-control" MaxLength="16" onkeypress="return soloLetras(event,'abcdefghijklmnopqrstuvwxyz0123456789-_')" placeholder="NAVE REFERENCIA"></asp:TextBox>
                                    &nbsp;
                                    <asp:Button ID="BtnBuscar" runat="server" class="btn btn-primary"  Text="BUSCAR"  OnClientClick="return mostrarloader('1')" OnClick="BtnBuscar_Click" />                             
                                    &nbsp;
                                    <img alt="loading.." src="../lib/file-uploader/img/loading.gif" height="32px" width="32px"  id="ImgCarga" class="nover"   />
                                    
                                </div>
                            </div>
                        </div>
                    </div>
                    <div><br /></div>
                    
                    <div class="alert alert-warning" id="banmsg" runat="server" clientidmode="Static"><b>Error!</b> >Debe ingresar el número de la carga MRN......</div>
                </asp:Panel>
                             
            </ContentTemplate>

            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="BtnBuscar" />
            </Triggers>
        </asp:UpdatePanel>

         <div class="form-title">
            DATOS GENERALES
        </div>


        <asp:UpdatePanel ID="UPDATOSCLIENTE" runat="server"  UpdateMode="Conditional" ChildrenAsTriggers="true">
                <ContentTemplate>
                    <div >
                        <div class="form-row" >
              
                            <div class="form-group col-md-2">
                            
                                <div class ="d-flex">
                                    &nbsp;
                                    <label for="inputZip">NAVE REFERENCIA<span style="color: #FF0000; font-weight: bold;"></span></label>
                                </div>
                                <div class ="d-flex">
                                    &nbsp;
                                    <asp:TextBox ID="TXTREFERENCIA" runat="server"  class="form-control" disabled MaxLength="150" placeholder=""></asp:TextBox>
                                </div>                            
                            </div>

                            <div class="form-group col-md-4">
                                <label for="inputZip">VSSL NAME<span style="color: #FF0000; font-weight: bold;"></span></label>
                                <asp:TextBox ID="TXTVSSLNAME" runat="server"  class="form-control" disabled MaxLength="150" placeholder=""></asp:TextBox>
                            </div>

                            <div class="form-group col-md-2">
                                <label for="inputZip">VOYAGE<span style="color: #FF0000; font-weight: bold;"></span></label>
                                <asp:TextBox ID="TXTVOYAGE" runat="server"  class="form-control"  disabled MaxLength="150" placeholder=""></asp:TextBox>
                            </div>

                            <div class="form-group col-md-2">
                                <label for="inputZip">ARRIVAL<span style="color: #FF0000; font-weight: bold;"></span></label>
                                <asp:TextBox ID="TXTARRIVAL" runat="server"  class="form-control" disabled MaxLength="150" placeholder=""></asp:TextBox>
                            </div>

                            <div class="form-group col-md-2">
                                <label for="inputZip">DEPARTED<span style="color: #FF0000; font-weight: bold;"></span></label>
                                <asp:TextBox ID="TXTDEPARTED" runat="server"  class="form-control" disabled MaxLength="150" placeholder=""></asp:TextBox>
                            </div>     
      
                            <div class="form-group col-md-6">
                                <span class="help-block">&nbsp;</span>
                          
                                    <asp:UpdatePanel ID="UPFECHA" runat="server"  UpdateMode="Conditional" ChildrenAsTriggers="true">
                                        <ContentTemplate>
                                            &nbsp;&nbsp;<asp:CheckBox ID="ChkTodos" runat="server" class="list-child" Text=" Facturar Todos"  OnCheckedChanged="ChkTodos_CheckedChanged"    AutoPostBack="True" />
                                        </ContentTemplate> 
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="ChkTodos" />
                                        </Triggers>
                                    </asp:UpdatePanel> 
                                  
		                    </div>

                        </div>
                    </div>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="BtnBuscar" />
                </Triggers>
            </asp:UpdatePanel>

        <div class="form-title">
            DETALLE MOVIMIENTOS PAN
        </div>

        <asp:UpdatePanel ID="UPDETALLE" runat="server" UpdateMode="Conditional" >  
            <ContentTemplate>
    
                <asp:UpdatePanel ID="UPBUSCARREPORTE" runat="server" UpdateMode="Conditional" >  
                    <ContentTemplate>
                        <div >
                            <div class="form-row" >
                                <div><br /></div>
                                <div class="form-group col-md-12">
                                    <div class="d-flex">
                                        <asp:radiobutton  ID ="rbBooking" text="Booking" runat="server" GroupName="gender"/> 
                                        &nbsp;&nbsp;&nbsp;
                                        <asp:radiobutton ID ="rbLinea" text="Linea" runat="server" GroupName="gender"/> 
                                        &nbsp;&nbsp;&nbsp;
                                        <asp:radiobutton ID ="rbContenedor" text="Contenedor" runat="server" GroupName="gender" /> 
						            </div>
                        
                                                
                                </div>

                                <div class="form-group col-md-4">
                                    <div class="d-flex">
                                        <asp:TextBox ID="txtFiltro" runat="server" class="form-control" MaxLength="16" onkeypress="return soloLetras(event,'abcdefghijklmnopqrstuvwxyz0123456789-_')" placeholder="FILTRO"></asp:TextBox>
                                        <asp:LinkButton runat="server" ID="btn_Filtrar" Text="<span class='fa fa-search' style='font-size:24px'></span>"  OnClientClick="return mostrarloader('2')"  OnClick="BtnFiltrar_Click" class="btn btn-primary" />
                                        <img alt="loading.." src="../lib/file-uploader/img/loading.gif" height="32px" width="32px"  id="ImgCargaDet" class="nover"  />
                                    </div>
                                </div>
                            </div>
                        </div>
   
			        </ContentTemplate>
                </asp:UpdatePanel>
                                
                <div class="bokindetalle" style=" width:100%; overflow:auto">       
                    <asp:GridView ID="tablePagination" runat="server" AutoGenerateColumns="False"  DataKeyNames="CNTR_BKNG_BOOKING"
                                            GridLines="None" OnPageIndexChanging="tablePagination_PageIndexChanging" OnPreRender="tablePagination_PreRender"  OnRowCommand="tablePagination_RowCommand"   OnRowDataBound="tablePagination_RowDataBound"
                                            PageSize="9"
                                            AllowPaging="True"
                                            CssClass="table table-bordered invoice">
                            <PagerStyle HorizontalAlign = "Right" CssClass="pagination-ys"  />
                            <RowStyle  BackColor="#F0F0F0" />
                            <alternatingrowstyle  BackColor="#FFFFFF" />
                           
                            <Columns>
                                <asp:TemplateField HeaderText="FA" ItemStyle-CssClass="center hidden-phone">
                                    <ItemTemplate>
                                        <asp:UpdatePanel ID="UPSELECCIONAR" runat="server" ChildrenAsTriggers="true">
                                            <ContentTemplate>
                                                <asp:CheckBox ID="CHKFA" runat="server" Checked='<%# Bind("VISTO") %>' OnCheckedChanged="CHKFA_CheckedChanged"    AutoPostBack="True" CssClass="center hidden-phone" />
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="CHKFA" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="CLIENTE" ItemStyle-CssClass="center hidden-phone">
                                    <EditItemTemplate>  
                                        <asp:TextBox ID="TextBox3" runat="server"></asp:TextBox>  
                                    </EditItemTemplate>  
                                    <ItemTemplate>  
                                        <asp:DropDownList ID="cmbCliente" class="form-control"
                                        runat="server"  DataTextField='CNTR_CLIENT' DataValueField='CNTR_CLIENT_ID2' AutoPostBack="True"  OnSelectedIndexChanged="cmbCliente_SelectedIndexChanged">
                                        </asp:DropDownList>  
                                    </ItemTemplate>  
                                </asp:TemplateField>  

                                <asp:BoundField DataField="CNTR_BKNG_BOOKING" HeaderText="BOOKING"  SortExpression="CNTR_BKNG_BOOKING" HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>                                                
                                <asp:BoundField DataField="CNTR_CONTENEDOR20" HeaderText="CONT 20"  HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>
                                <asp:BoundField DataField="CNTR_CONTENEDOR40" HeaderText="CONT 40"  HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>
                                <asp:BoundField DataField="CNTR_CLNT_CUSTOMER_LINE" HeaderText="LINEA"  HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>                                               

                                <asp:TemplateField HeaderText="PROCESADO" ItemStyle-CssClass="center hidden-phone">
                                    <ItemTemplate>
                                        <asp:UpdatePanel ID="UPPRO" runat="server" ChildrenAsTriggers="true">
                                            <ContentTemplate>
                                                <asp:CheckBox ID="CHKPRO" runat="server" Checked='<%# Bind("CNTR_PROCESADO") %>' CssClass="center hidden-phone" Enabled="false"/>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="CHKFA" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Seleccionar" ItemStyle-CssClass="center hidden-phone">
                                    <ItemTemplate>
                                        <asp:Button runat="server" ID="IncreaseButton" Text="Ver Detalle" CommandName="Seleccionar" CommandArgument='<%# Bind("CNTR_BKNG_BOOKING") %>' class="btn btn-primary" data-toggle="modal" data-target="#myModal2" />

        
                                    </ItemTemplate>
                                </asp:TemplateField>

                            </Columns>
                    </asp:GridView>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>

    
        <asp:UpdatePanel ID="UPBOTONES" runat="server" UpdateMode="Conditional" >  
            <ContentTemplate>  
                <div class="form-group">
                    <div class="alert alert-warning" id="banmsg_det" runat="server" clientidmode="Static"><b>Error!</b> >Debe ingresar el número de la carga MRN......</div>
                </div>
                <div class="white-panel mt">
                    <div class="panel-body">
                        <div align="center">    
                            <img alt="loading.." src="../lib/file-uploader/img/loading.gif" height="32px" width="32px"  id="ImgCargaDet" class="nover"  />
                        </div>
                    </div>    
                </div>  


                <div class="form-group col-md-12"> 
                    <div class="row">
                        <div class="col-md-12 d-flex justify-content-center">
                            <asp:Button ID="BtnFacturar" runat="server" class="btn btn-primary" Text="GENERAR FACTURA" OnClientClick="return mostrarloader('1')"  OnClick="BtnFacturar_Click" />
                        </div>
                    </div><!--btn-group-justified-->
                </div><!--showback-->
            </ContentTemplate>
        </asp:UpdatePanel>
          
     
        <div id="myModal2" class="modal fade" tabindex="-1" role="dialog">
            <div class="modal-dialog" role="document" style="max-width: 1000px"> <!-- Este tag style controla el ancho del modal -->
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title">DETALLE DE CONTENEDORES DE EXPORTACIÓN</h5>
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                        </button>
                    </div>
                    <div class="modal-body">
                            
                        <asp:UpdatePanel ID="UPMODAL" runat="server" UpdateMode="Conditional" >  
                            <ContentTemplate>
                                <asp:Panel ID="pnlEdit" runat="server">         
                                    <p>BOOKING:<asp:TextBox ID="txtID"  runat="server" class="form-control"   placeholder="" size="16" Font-Bold="false" disabled></asp:TextBox></p>
                                </asp:Panel>
                                    <div class="bokindetalle" style=" width:100%; overflow:auto">       
                                        <asp:GridView ID="GrillaDetalle" runat="server" AutoGenerateColumns="False"  DataKeyNames="CNTR_CONTAINER"
                                                                GridLines="None" 
                                                                PageSize="20"
                                                                AllowPaging="True"
                                                                CssClass="table table-bordered invoice"
                                            OnPageIndexChanging="GrillaDetalle_PageIndexChanging" OnPreRender="GrillaDetalle_PreRender">
                                                <PagerStyle HorizontalAlign = "Right" CssClass="pagination-ys"  />
                                                <RowStyle  BackColor="#F0F0F0" />
                                                <alternatingrowstyle  BackColor="#FFFFFF" />
                           
                                            <Columns>                              
                                                <asp:BoundField DataField="CNTR_CONTAINER"  HeaderText="CONTENEDOR" SortExpression="CNTR_CONTAINER" HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>
                                                <asp:BoundField DataField="CNTR_TYSZ_SIZE" HeaderText="SIZE"  HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>
                                                <asp:BoundField DataField="CNTR_TYSZ_ISO" HeaderText="ISO"  HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>
                                                <asp:BoundField DataField="CNTR_TYSZ_TYPE" HeaderText="TIPO" HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>
                                                <asp:BoundField DataField="CNTR_FULL_EMPTY_CODE" HeaderText="FULL/EMPTY" HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>
                                                <asp:BoundField DataField="CNTR_YARD_STATUS" HeaderText="ESTADO"  HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>
                                                <asp:BoundField DataField="CNTR_AISV" HeaderText="AISV" HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>
                                                <asp:BoundField DataField="CNTR_HOLD" HeaderText="HOLD" HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>
                                                <asp:BoundField DataField="CNTR_REEFER_CONT" HeaderText="REEFER_CONT"   HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>
                                                    <asp:BoundField DataField="CNTR_PROFORMA" HeaderText="PROFORMA"   HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>
                                                            
                                                            
                                                            
                                            </Columns>
                                        </asp:GridView>
                                    </div>   
                            </ContentTemplate>   
                        </asp:UpdatePanel>   
                    </div>
                    <div class="modal-footer">        
                        <button type="button" class="btn btn-primary" data-dismiss="modal">CERRAR</button>
                    </div>
                </div>
            </div>
        </div>

    </div>


    <script type="text/javascript" src="../lib/jquery/jquery.min.js"></script>
    <script type="text/javascript" src="../lib/bootstrap/js/bootstrap.min.js"></script>
    <script  type="text/javascript" src="../lib/jquery.dcjqaccordion.2.7.js"></script>
    <script type="text/javascript" src="../lib/jquery.scrollTo.min.js"></script>
    <script type="text/javascript" src="../lib/jquery.nicescroll.js" ></script>
    <script type="text/javascript" src="../lib/common-scripts.js"></script>
    <script type="text/javascript" src="../lib/gritter/js/jquery.gritter.js"></script>
    <script type="text/javascript" src="../lib/gritter-conf.js"></script>
    <!--script for this page-->
    <script type="text/javascript" src="../lib/pages.js" ></script>
    <script type="text/javascript" src="../lib/advanced-form-components.js"></script>

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

        function confirmacion() {
            if (confirm("Está seguro de que desea generar la factura?")) {
                mostrarloader();
                return true;
            } else {
                ocultarloader();
                return false;
            }
        }
    </script>



    <!--SCRIPT PARA MODAL-->
    <script type="text/javascript" src="https://code.jquery.com/jquery-3.5.1.slim.min.js" integrity="sha384-DfXdz2htPH0lsSSs5nCTpuj/zy4C+OGpamoFVy38MVBnE+IbbVYUew+OrCXaRkfj" crossorigin="anonymous"></script>
    <script type="text/javascript" src="js/bootstrap.bundle.min.js"></script>
    <script type="text/javascript" src="js/datatables.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $('#dtBasicExample').DataTable();
            $('.dataTables_length').addClass('bs-select');
        });
    </script>
    
    <script type="text/javascript">
        var $table = $('#table')

        $(function () {
            $('#modalTable').on('shown.bs.modal', function () {
                $table.bootstrapTable('resetView')
            })
        })
    </script>


 

</asp:Content>