<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="confirmacionRecepcionCarga.aspx.cs" Inherits="CSLSite.confirmacionRecepcionCarga" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="placehead" runat="server">

    <link href="../shared/estilo/catalogos.css" rel="stylesheet" type="text/css" />
    <link href="../img/favicon2.png" rel="icon"/>
    <link href="../img/icono.png" rel="apple-touch-icon"/>
    <link href="../css/bootstrap.min.css" rel="stylesheet"/>
    <link href="../css/dashboard.css" rel="stylesheet"/>
    <link href="../css/icons.css" rel="stylesheet"/>
    <link href="../css/style.css" rel="stylesheet"/>
    <link href="https://fonts.googleapis.com/css2?family=Roboto:wght@300;400;500;700&display=swap" rel="stylesheet"/>

    <script type="text/javascript">
        function BindFunctions() {
                $(document).ready(function ()
                {
                    /*
                    * Insert a 'details' column to the table
                    */
                    var nCloneTh = document.createElement('th');
                    var nCloneTd = document.createElement('td');
                    nCloneTd.innerHTML = '<img src="../lib/advanced-datatable/images/details_open.png">';
                    nCloneTd.className = "center";

                    $('#hidden-table-info thead tr').each(function() {});
                    $('#hidden-table-info tbody tr').each(function() {});

                    /*
                    * Initialse DataTables, with no sorting on the 'details' column
                    */
                    var oTable = $('#hidden-table-info').dataTable(
                        {
                                "aoColumnDefs":
                                [
                                {
                                    "bSortable": false,
                                    "aTargets": [0]
                                }
                                ],
                                "aaSorting":
                                [
                                [1, 'asc']
                                ]
                        }
                    );
            });
        }
    </script>

    <script type="text/javascript">
         $(document).ready(function () {
             $('#<%= tablePagination.ClientID %>').dataTable();
         });
    </script>    

    <script type="text/javascript">
            $(document).ready(function () {
                $('#<%= GrillaDetalle.ClientID %>').dataTable();
            });
    </script>    

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="placebody" runat="server" >
    <asp:ToolkitScriptManager ID="tkscata" runat="server"></asp:ToolkitScriptManager>
     <script type="text/javascript">
        Sys.Application.add_load(Calendario); 
    </script>  
    <div id="div_BrowserWindowName" style="visibility:hidden;">
        <asp:HiddenField ID="hf_BrowserWindowName" runat="server" />
    </div>


    <div class="mt-4">         
        <nav class="mt-4" aria-label="breadcrumb">
            <ol class="breadcrumb">
                <li class="breadcrumb-item" id="opcion_principal" runat="server"><a href="#">Expo Contenedores</a></li>
                <li class="breadcrumb-item active" aria-current="page" id="sub_opcion" runat="server">CONSOLA DE CARGA DE EXPORTACIÓN</li>
            </ol>
        </nav>
    </div>

    <div class="dashboard-container p-4" id="cuerpo" runat="server">
             
        <asp:UpdatePanel ID="UPCARGA" runat="server"  UpdateMode="Conditional" ChildrenAsTriggers="true">
            <ContentTemplate>
                <asp:Panel ID="PanelCualquiera" runat="server" DefaultButton ="BtnBusca">
                     
                    <div class="form-row" >
                          
                        <div class="form-group col-md-4">
                            <label for="inputZip">MRN<span style="color: #FF0000; font-weight: bold;">*</span></label>
                            <asp:TextBox ID="TXTMRN" runat="server" class="form-control" MaxLength="16"  onkeypress="return soloLetras(event,'abcdefghijklmnopqrstuvwxyz0123456789-_')"  
                                                placeholder="MRN"></asp:TextBox>
                        </div>
                        <div class="form-group col-md-2">
                            <label for="inputZip">MSN<span style="color: #FF0000; font-weight: bold;">*</span></label>
                            <asp:TextBox ID="TXTMSN" runat="server" class="form-control"  MaxLength="4"  onkeypress="return soloLetras(event,'abcdefghijklmnopqrstuvwxyz0123456789-_')" 
                                            placeholder="MSN"></asp:TextBox>
                        </div>
                        <div class="form-group col-md-2">
                            <label for="inputZip">HSN<span style="color: #FF0000; font-weight: bold;">*</span></label>
                            <div class="d-flex">
                                <asp:TextBox ID="TXTHSN" runat="server" class="form-control"  MaxLength="4"  onkeypress="return soloLetras(event,'abcdefghijklmnopqrstuvwxyz0123456789-_')" placeholder="HSN"></asp:TextBox>
                                     
                                &nbsp;

                                <asp:LinkButton runat="server" ID="BtnBusca" Text="<span class='fa fa-search' style='font-size:24px'></span>"  OnClientClick="return mostrarloader('1')" OnClick="BtnBuscar_Click" class="btn btn-primary" />
                                &nbsp;
                                <img alt="loading.." src="../lib/file-uploader/img/loading.gif" height="32px" width="32px"  id="ImgCarga" class="nover"/>
                            </div>
                        </div>
                                 
                    </div>
                   
                    <div><br /></div>
                    
                    <div class="alert alert-warning" id="banmsg" runat="server" clientidmode="Static"><b>Error!</b> >Debe ingresar el número de la carga MRN......</div>
                    
                </asp:Panel>            
            </ContentTemplate>
                <Triggers>
                <asp:AsyncPostBackTrigger ControlID="BtnBusca" />
            </Triggers>
        </asp:UpdatePanel>   
        
    </div>

    <br />

    <section id="main-content">
        <section class="wrapper">

            <div class="row mt">
                <div class="col-sm-6" >

                    <div class="dashboard-container p-4" id="Div1" runat="server" style="height:1050px" >
   <%--
                        <asp:UpdatePanel ID="UPBUSCARREPORTE" runat="server" UpdateMode="Conditional" >  
                            <ContentTemplate>
                                     
                             <div >
                                    <br />
                                    <div class="form-group">
                                        <asp:radiobutton ID ="rbBooking" text="&nbsp; Booking" runat="server" GroupName="gender"/>&nbsp;&nbsp;&nbsp;| &nbsp;
                                        <asp:radiobutton ID ="rbLinea" text="&nbsp; Linea" runat="server" GroupName="gender"/>&nbsp;&nbsp;&nbsp;| &nbsp;
                                        <asp:radiobutton ID ="rbContenedor" text="&nbsp; Contenedor" runat="server" GroupName="gender" /> 
						            </div>
                                </div>
                                                       
                                <br />

                                <div class="d-flex">
                                        <asp:TextBox ID="txtFiltro" runat="server" class="form-control" MaxLength="16" onkeypress="return soloLetras(event,'abcdefghijklmnopqrstuvwxyz0123456789-_')" placeholder="FILTRO"></asp:TextBox>
						                &nbsp;
                                        <asp:LinkButton runat="server" ID="btn_Filtrar" Text="<span class='fa fa-search' style='font-size:24px'></span>"  OnClientClick="return mostrarloader('2')"  OnClick="BtnFiltrar_Click" class="btn btn-primary" />
                                        <img alt="loading.." src="../lib/file-uploader/img/loading.gif" height="32px" width="32px"  id="ImgCargaDet" class="nover"  />
                                        &nbsp;
                                        <asp:Button ID="btn_Filtrar1_" height="10" runat="server" class="btn btn-round btn-danger"  data-toggle="tooltip" data-placement="top" title="Registros con error" OnClick="BtnFiltrar1_Click"  UseSubmitBehavior="false" data-dismiss="modal" />
                                        <asp:Button ID="btn_Filtrar2_" height="10" runat="server" class="btn btn-round btn-warning" data-toggle="tooltip" data-placement="top" title="Registros en cola " OnClick="BtnFiltrar2_Click"  UseSubmitBehavior="false" data-dismiss="modal" />
                                        <asp:Button ID="btn_Filtrar3_" height="10" runat="server" class="btn btn-round btn-default" data-toggle="tooltip" data-placement="top" title="Registros pendientes" OnClick="BtnFiltrar3_Click"  UseSubmitBehavior="false" data-dismiss="modal" />
                                        <asp:Button ID="btn_Filtrar4_" height="10" runat="server" class="btn btn-round btn-success" data-toggle="tooltip" data-placement="top" title="Registros Validados/Facturados" OnClick="BtnFiltrar4_Click"  UseSubmitBehavior="false" data-dismiss="modal" />
                                </div>
			                </ContentTemplate>
                        </asp:UpdatePanel>
                      --%>
                        <div class="form-title ">
                            Detalle de Contenedores de Exportación
                        </div>
                           
                        <asp:UpdatePanel ID="UPCAB"  runat="server"  UpdateMode="Conditional">
                            <ContentTemplate>        
                                <div class="bokindetalle" style="height:750px; width:100%; overflow:auto">    
                                    <asp:GridView ID="tablePagination" runat="server" AutoGenerateColumns="False"  
                                                DataKeyNames="CNTR_BKNG_BOOKING"
                                                GridLines="None" 
                                                OnPageIndexChanging="tablePagination_PageIndexChanging" 
                                                OnPreRender="tablePagination_PreRender"     
                                                OnRowCommand="tablePagination_RowCommand"   
                                                OnRowDataBound="tablePagination_RowDataBound"
                                                PageSize="8"
                                                AllowPaging="True"
                                            CssClass="display table table-bordered">
                                                <%--CssClass="display table table-bordered">--%>
                                            <PagerStyle HorizontalAlign = "Right" CssClass="display table table-bordered"  />
                                            <RowStyle  BackColor="#F0F0F0" />
                                            <alternatingrowstyle  BackColor="#FFFFFF" />
                           
                                            <Columns>
                                                <asp:BoundField DataField="CNTR_ID" HeaderText="CODIGO" Visible="false"  HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>
                                                <asp:BoundField DataField="CNTR_ESTADO" HeaderText="ESTADO" Visible="false"  HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>
                                                <asp:BoundField DataField="CNTR_BKNG_BOOKING" HeaderText="BOOKING" Visible="false"  SortExpression="CNTR_BKNG_BOOKING" HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>                                                                
                                                <asp:BoundField DataField="BOOKINGLINE" HeaderText="LINEA BOOKING" ItemStyle-Font-Size="9px" SortExpression="BOOKINGLINE" HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>                                                                                                       
                                                <asp:BoundField DataField="CNTR_SIZE_RF" HeaderText="CONT SIZE"  SortExpression="CNTR_SIZE_RF" HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>                                                                

                                                <asp:TemplateField HeaderText="" ItemStyle-CssClass="center hidden-phone" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:UpdatePanel ID="UPPRO" runat="server" ChildrenAsTriggers="true">
                                                            <ContentTemplate>
                                                                <asp:CheckBox ID="CHKPRO" runat="server" Checked='<%# Bind("CNTR_PROCESADO") %>'  CssClass="center hidden-phone" Enabled="false"/>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="" ItemStyle-CssClass="center hidden-phone">
                                                    <ItemTemplate>
                                                        <asp:LinkButton runat="server" ID="btnVisualizar" Height="30px"  CommandName="Seleccionar" Text="<span  style='align-content:center;' class='fa fa-eye'></span>"  CommandArgument='<%#jsarguments( Eval("CNTR_BKNG_BOOKING"), Eval("CNTR_ID") )%>' CssClass="btn btn-primary"  />
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="" ItemStyle-CssClass="center hidden-phone">
                                                    <ItemTemplate>
                                                        <asp:Button runat="server" ID="IncreaseButton3"  Height="30px" Text="RW"  CommandName="Actualizar" CommandArgument='<%#Eval("CNTR_ID")%>' class="btn btn-primary" data-toggle="modal" data-target="#myModal" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="" ItemStyle-CssClass="center hidden-phone">
                                                    <ItemTemplate>
                                                        <asp:Button runat="server" ID="btnFactura" Height="30px" CommandName="Factura" Text="FA"  CommandArgument='<%# Bind("CNTR_ID") %>' class="btn btn-primary"  data-toggle="modal" data-target="#myModal3"  />                                                                       
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                            </Columns>
                                    </asp:GridView>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>

               
                </div> <!-- col-sm-3-->
            
                <div class="col-sm-6">
                   
                        <div class="dashboard-container p-4" id="Div2" runat="server" style="height:1050px">
                            <asp:UpdatePanel ID="UPDET" runat="server" UpdateMode="Conditional" >  
                                <ContentTemplate>

                                    <div class="form-group col-md-12">
                                            <label for="inputZip">BOOKING:<span style="color: #FF0000; font-weight: bold;"></span></label>
                                            <div class="d-flex">
                                            <asp:TextBox ID="txtID"  runat="server" class="form-control"   placeholder="" size="16"  Font-Bold="false" disabled></asp:TextBox>
                                        </div>
                                    </div>
                                        
                                    <br />

                                    <div class="bokindetalle" style="height:890px; width:100%; overflow:auto">       
                                        <asp:GridView ID="GrillaDetalle" runat="server" AutoGenerateColumns="False"  DataKeyNames="CNTR_CONSECUTIVO"
                                                                GridLines="None" 
                                                                PageSize="200"
                                                                AllowPaging="True"
                                                                CssClass="table table-bordered invoice"
                                                                OnRowDataBound="GrillaDetalle_RowDataBound"
                                                                OnRowCommand="GrillaDetalle_RowCommand" 
                                                                OnPageIndexChanging="GrillaDetalle_PageIndexChanging" 
                                                                OnPreRender="GrillaDetalle_PreRender">
                                                <PagerStyle HorizontalAlign = "Right" CssClass="table table-bordered invoice"  />
                                                <RowStyle  BackColor="#F0F0F0" />
                                                <alternatingrowstyle  BackColor="#FFFFFF" />
                           
                                            <Columns>       

                                                <asp:BoundField DataField="CNTR_CONTAINER"  HeaderText="CONTENEDOR" SortExpression="CNTR_CONTAINER" HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>
                                                <asp:BoundField DataField="SERVICIOS_ACTUALES" HeaderText="SERVICIOS OK"  HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>
                                                <asp:BoundField DataField="TOTAL" HeaderText="TOTAL" Visible="false" HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>
                                                <asp:BoundField DataField="MSG_DUPLICADO" HeaderText="SERVICIOS DUPLICADOS" HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>
                                                <asp:BoundField DataField="MSG_FALTANTE" Visible="false" HeaderText="MSG FALTANTE"  HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>
                                                <asp:BoundField DataField="FALTANTES" HeaderText="SERVICIOS FALTANTES" HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>
                                                <asp:BoundField DataField="MSG_OTROS" HeaderText="MSG OTROS" HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>
                                                <asp:BoundField DataField="FECHA_REGISTRO" HeaderText="F. ULTIMO PROCESO"   HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>
                                                <asp:BoundField DataField="USUARIO_REGISTRA" HeaderText="USUARIO PROCESO"   HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>
                                                <asp:BoundField DataField="ESTADO_REGISTRO" HeaderText="ESTADO REGISTRO" Visible ="false"  HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>

                                                <asp:TemplateField HeaderText="Seleccionar" ItemStyle-CssClass="center hidden-phone">
                                                    <ItemTemplate>
                                                        <asp:Button runat="server" ID="IncreaseButton" Text="Historial" CommandName="Seleccionar" CommandArgument='<%#Eval("CNTR_CAB_ID")+","+ Eval("CNTR_CONSECUTIVO")+","+ Eval("CNTR_CONTAINER")%>' class="btn btn-primary" data-toggle="modal" data-target="#myModal2" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                                    
                                </ContentTemplate>   
                            </asp:UpdatePanel>   
                        </div>
                   
                </div> <!-- col-sm-9-->
            </div><!-- row mt-->

            <asp:UpdatePanel ID="UPBOTONES" runat="server" UpdateMode="Conditional" >  
                <ContentTemplate>
                
                    <div class="form-group">
                        <div class="alert alert-warning" id="banmsg_det" runat="server" clientidmode="Static"><b>Error!</b> >Debe ingresar el número de la carga MRN......</div>
                    </div>
                  
                </ContentTemplate>
            </asp:UpdatePanel>       
            
           
            

            <asp:UpdatePanel ID="UPMENSAJE" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">  
                     <ContentTemplate>

                          <div class="modal fade" id="myModal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
                            <div class="modal-dialog">
                              <div class="modal-content">
                                <div class="modal-header">
                                  <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                                  <h4 class="modal-title" id="myModalLabel">Confirmar Generación</h4>
                                </div>
                                <div class="modal-body">
                                  Si usted da click en SI, se procederá a generar la factura
                                </div>
                                <div class="modal-footer">
                                     <asp:Button ID="BtnProcesar" runat="server" class="btn btn-default"  Text="SI" OnClick="BtnActualizar_Click"  UseSubmitBehavior="false" data-dismiss="modal" />
                                  <button type="button" class="btn btn-primary" data-dismiss="modal">NO</button>
                                </div>
                              </div>
                            </div>
                          </div>
                     </ContentTemplate>
                            <Triggers>
                                  <asp:AsyncPostBackTrigger ControlID="BtnProcesar" />
                                </Triggers>
            </asp:UpdatePanel>   

        </section><!--wrapper site-min-height-->
    </section><!--main-content-->

  <script type="text/javascript" src="../lib/pages.js" ></script>
 
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
</script>

</asp:Content>
