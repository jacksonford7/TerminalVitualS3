<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="edi_booking.aspx.cs" Inherits="CSLSite.edi.edi_booking" %>
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

    <%-- <script type="text/javascript">
         $(document).ready(function () {
             $('#<%= GridView1.ClientID %>').dataTable();
         });

    </script>
    
    <script type="text/javascript">
         $(document).ready(function () {
             $('#<%= GridView2.ClientID %>').dataTable();
         });

    </script>--%>

    <%-- <script type="text/javascript">
         $(document).ready(function () {
             $('#<%= GridView3.ClientID %>').dataTable();
         });

    </script>--%>

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
                <li class="breadcrumb-item" id="opcion_principal" runat="server"><a href="#">Mis Naves</a></li>
                <li class="breadcrumb-item active" aria-current="page" id="sub_opcion" runat="server">BOOKING MANUAL (COPARN)</li>
            </ol>
        </nav>
    </div>

    <div class="dashboard-container p-4" id="cuerpo" runat="server">
        
        <div class="form-title">
            DATOS GENERALES
        </div>

        <asp:UpdatePanel ID="UPEDIT" runat="server" UpdateMode="Conditional" >  
            <ContentTemplate>
                <asp:Panel ID="Panel3" runat="server">         
                        <div id="div_Codigos" style="visibility:hidden;">
                            <asp:HiddenField ID="hdf_CodigoCab" runat="server" />
                        </div>

                    <div class="form-row">
                        <div class="form-group col-md-3">
                            <label for="inputAddress">Numero :<span style="color: #FF0000; font-weight: bold;"></span></label>
                            <div class="d-flex">

                                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" >  
                                    <ContentTemplate>
                                        <asp:TextBox ID="txtCodigoCab" Width="5" disabled runat="server" class="form-control" MaxLength="50" AutoPostBack="true" OnTextChanged="txtCodigoCab_TextChanged"  Font-Bold="false" ></asp:TextBox>
                                    </ContentTemplate>
                                    </asp:UpdatePanel>
            
                                <asp:TextBox ID="txtNumber"  runat="server" class="form-control" MaxLength="50"   placeholder="NUMBER" Font-Bold="false" ></asp:TextBox>
                                &nbsp;
                                <a id="btnBuscarBooking" runat="server" class="btn btn-outline-primary mr-4" style='font-size:24px' target="popup" onclick="window.open('../catalogo/bookinCoparn.aspx','name','width=900,height=880')">
                                    <span class='fa fa-search' style='font-size:24px'></span> </a>
                            </div>
                        </div>

                        <div class="form-group col-md-3">
                            <label for="inputAddress">Linea:<span style="color: #FF0000; font-weight: bold;"></span></label>
                            <div class="d-flex">
                                <asp:TextBox ID="txtLineOperator"  runat="server" class="form-control" MaxLength="100" placeholder="LINE OPERATOR" disabled Font-Bold="false" ></asp:TextBox>
                                
                            </div>
                        </div>

                                
                        <div class="form-group   col-md-3"> 
                            <label for="inputAddress">Visita :<span style="color: #FF0000; font-weight: bold;"></span></label>
                            <div class="d-flex">
                                <asp:TextBox ID="txtVesselVisit" runat="server" class="form-control" MaxLength="100" placeholder="VESSEL VISIT" disabled Font-Bold="false"></asp:TextBox>
                                &nbsp;
                                <a id="btnVisita" runat="server" class="btn btn-outline-primary mr-4" style='font-size:24px' target="popup" onclick="window.open('../catalogo/naves.aspx','name','width=900,height=880')">
                                    <span class='fa fa-search' style='font-size:24px'></span> </a>
                            </div>
                        </div>

                        <div class="form-group col-md-3">
                            <label for="inputAddress">Puerto de Carga :<span style="color: #FF0000; font-weight: bold;"></span></label>
                            <div class="d-flex">
                                <asp:TextBox ID="txtPortOfLoad" Text="ECGYE" disabled runat="server" class="form-control" MaxLength="100" placeholder="PORT OF LOAD" Font-Bold="false" ></asp:TextBox>
                                &nbsp;
                                <a id="btnPort1" runat="server" class="btn btn-outline-primary mr-4" target="popup" onclick="setPort(30);" ><span class='fa fa-search' style='font-size:24px'></span></a>
                            </div>
                        </div>
                                
                        <div class="form-group col-md-3">
                            <label for="inputAddress">Puerto de Descarga (POD1) :<span style="color: #FF0000; font-weight: bold;"></span></label>
                            <div class="d-flex">
                                <asp:TextBox ID="txtPortOfDischarge"  runat="server" class="form-control" MaxLength="100" placeholder="PORT OF DISCHARGE" Font-Bold="false" ></asp:TextBox>
                                 &nbsp;
                                <a id="btnPort2" runat="server"  class="btn btn-outline-primary mr-4" target="popup" onclick="setPort(10);" ><span class='fa fa-search' style='font-size:24px'></span></a>
                            </div>
                        </div>
                        <div class="form-group col-md-3">
                            <label for="inputAddress">Segundo Puerto de Descarga (POD2) :<span style="color: #FF0000; font-weight: bold;"></span></label>
                            <div class="d-flex">
                                <asp:TextBox ID="txtSecportOfDischarge"  runat="server" class="form-control" MaxLength="100" placeholder="SECOUND PORT OF DISCHARGE" Font-Bold="false" ></asp:TextBox>
                                 &nbsp;
                                <a id="btnPort3" runat="server"  class="btn btn-outline-primary mr-4" target="popup" onclick="setPort(20);" ><span class='fa fa-search' style='font-size:24px'></span></a>
                            </div>
                        </div>

                        <div class="form-group col-md-3"> 
                            <label for="inputAddress">Tipo Carga :<span style="color: #FF0000; font-weight: bold;"></span></label>
                            <div class="d-flex">
                                <asp:DropDownList ID="cmbfreightKind" class="form-control" runat="server" Font-Size="Medium" Font-Bold="true" ></asp:DropDownList>
                            </div>
                        </div>

                        <%--<div class="form-group col-md-3">
                            <label for="inputAddress">POD 1 :<span style="color: #FF0000; font-weight: bold;"></span></label>
                            <div class="d-flex">
                                <asp:TextBox ID="txtPOD1"  runat="server" class="form-control" MaxLength="100" placeholder="POD 1" Font-Bold="false" ></asp:TextBox>
                            </div>

                        </div>--%>

                        <div class="form-group col-md-3">
                            <label for="inputAddress">Remitente :<span style="color: #FF0000; font-weight: bold;"></span></label>
                            <div class="d-flex">
                                <asp:TextBox ID="txtShipper"  runat="server" class="form-control" MaxLength="100" placeholder="SHIPPER" Font-Bold="false" ></asp:TextBox>
                            </div>
                        </div>
                        
                        <%--<div class="form-group col-md-3">
                            <label for="inputAddress">Consignatario :<span style="color: #FF0000; font-weight: bold;"></span></label>
                            <div class="d-flex">
                                <asp:TextBox ID="txtConsignee"  runat="server" class="form-control" MaxLength="100" placeholder="CONSIGNEE" Font-Bold="false" ></asp:TextBox>
                            </div>
                        </div>--%>

                        <div class="form-group   col-md-3"> 
                            <label for="inputAddress">Consignatario:<span style="color: #FF0000; font-weight: bold;"></span></label>
                            <div class="d-flex">
                                <asp:TextBox ID="txtConsigneeId" runat="server" MaxLength="20" 
                                    placeholder="CONSIGNEEID"
                                    onkeypress="return soloLetras(event,'1234567890',true)"    
                                    class="form-control"
                                    style="text-transform:uppercase;"
                                    ClientIDMode="Static" >
                                </asp:TextBox>
                                    &nbsp;
                                <img alt="loading.." src="../shared/imgs/loader.gif" id="loader4" class="nover"  />
                                <%--<asp:Button ID="btnBuscarConsignatario" runat="server"  CssClass="btn btn-primary" Text="Buscar" OnClientClick="return prepareObjectRuc()" ToolTip="Busca al Consignatario por el RUC." OnClick="btnBuscarConsignatario_Click"/>--%>

                                <asp:LinkButton ID="btnBuscarConsignatario" CssClass="btn btn-outline-primary mr-4" OnClientClick="return prepareObjectRuc()" ToolTip="Busca al Consignatario por el RUC." OnClick="btnBuscarConsignatario_Click" runat="server"><span class='fa fa-search' style='font-size:24px'></span></asp:LinkButton>

                                <%--<a id="A1" runat="server"  class="btn btn-outline-primary mr-4" target="popup" OnClick="btnBuscarConsignatario_Click"  ><span class='fa fa-search' style='font-size:24px'></span></a>--%>
                            </div>
                        </div>

                        <div class="form-group col-md-3">
                            <label for="inputAddress">Estiba Especial :<span style="color: #FF0000; font-weight: bold;"></span></label>
                            <div class="d-flex">
                                <asp:TextBox ID="txtConsignee"  runat="server" class="form-control" MaxLength="200" placeholder="CONSIGNEE" Font-Bold="false" disabled></asp:TextBox>
                            </div>
                        </div>

                        <div class="form-group col-md-3">
                            <label for="inputAddress">Estiba Especial :<span style="color: #FF0000; font-weight: bold;"></span></label>
                            <div class="d-flex">
                                <asp:TextBox ID="txtSpecialStow"  runat="server" class="form-control" MaxLength="100" placeholder="SPECIAL STOW" Font-Bold="false" ></asp:TextBox>
                            </div>
                        </div>

                        <div class="form-group col-md-3">
                            <label for="inputAddress">Estiba Especial 2 :<span style="color: #FF0000; font-weight: bold;"></span></label>
                            <div class="d-flex">
                                <asp:TextBox ID="txtSpecialStow2"  runat="server" class="form-control" MaxLength="100" placeholder="SPECIAL STOW 2" Font-Bold="false" ></asp:TextBox>
                            </div>
                        </div>

                        <div class="form-group col-md-3">
                            <label for="inputAddress">Estiba Especial 3 :<span style="color: #FF0000; font-weight: bold;"></span></label>
                            <div class="d-flex">
                                <asp:TextBox ID="txtSpecialStow3"  runat="server" class="form-control" MaxLength="100" placeholder="SPECIAL STOW 3" Font-Bold="false" ></asp:TextBox>
                            </div>
                        </div>

                         <div class="form-group col-md-3"> 
                            <label for="inputAddress">Acción :<span style="color: #FF0000; font-weight: bold;"></span></label>
                            <div class="d-flex">
                                <asp:DropDownList ID="cmbEstado" disabled class="form-control" runat="server" Font-Size="Medium"   Font-Bold="true" ></asp:DropDownList> 
                                <a class="tooltip" ><span class="classic" >Estados generales</span><img alt='' src='../shared/imgs/info.gif' class='datainfo'/></a>
                            </div>
                        </div>

                        <div class="form-group col-md-6"> 
                            <label for="inputAddress">Notas<span style="color: #FF0000; font-weight: bold;"></span></label>
                            <div class="d-flex"> 
                                <asp:TextBox TextMode="MultiLine" class="form-control"  ID="txtNotes" runat="server" MaxLength="100" onkeypress="return soloLetras(event,'abcdefghijklmnopqrstuvwxyz01234567890/')"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                                            
                </asp:Panel>
                      
                <section class="wrapper2">
                    <%--<div class="row mb"> --%>
                        <div class="content-panel">
                            <%--<div class="form-row">

                               

                            </div>--%>

                                                    

                            <div></div>

                            <div class="row">
                                <div class="col-md-12 d-flex justify-content-center">
                                    <asp:Button ID="btnEditar" runat="server" class="btn btn-outline-primary"  Text="EDITAR" OnClick="btnEditar_Click" />
                                    <asp:Button runat="server" ID="btnAnularDoc" CommandName="Generar" Text="ANULAR" height="40" class="btn btn-primary ml-2 " data-toggle="modal" data-target="#myModal6"  />
                                    <asp:Button ID="btnGrabar" runat="server" class="btn btn-primary ml-2 "  Text="GRABAR"  OnClientClick="return mostrarloader('1')" OnClick="btnGrabar_Click"/>
                                    <asp:Button runat="server" ID="btnGenerar" CommandName="Generar" Text="PROCESAR" height="40" class="btn btn-primary ml-2 " data-toggle="modal" data-target="#myModal3"  />
                                    <asp:Button ID="btnCancelar" Visible ="false" runat="server" class="btn btn-outline-primary ml-2"  Text="CANCELAR" OnClick="btnCancelar_Click" />
                                    <img alt="loading.." src="../lib/file-uploader/img/loading.gif" height="32px" width="32px"  id="ImgCarga" class="nover"/>
                                    <span id="imagens"></span>
                                </div>
                            </div>

                                <br/>
                            <div class="row">
                                <div class="col-md-12 d-flex justify-content-center">
                                    <div class="alert alert-warning" id="banmsg" runat="server" clientidmode="Static"><b>Error!</b> >Debe ingresar los datos requeridos......</div>
                                </div>
                            </div>


                        </div><!--content-panel-->
                    <%--</div>--%><!--row mb-->
                </section><!--wrapper2-->

            </ContentTemplate>   
        </asp:UpdatePanel>
            
                  
        
        

    </div>

    <br />

    <section id="main-content">
        <section class="wrapper">


            <div class="row mt">
                <div class="col-sm-4" >

                    <div class="dashboard-container p-4" id="Div1" runat="server" style="height:100%" >
                        <asp:UpdatePanel ID="UPSUBDET"  runat="server"  UpdateMode="Conditional">
                            <ContentTemplate>  
                                <div class="form-title ">
                                    <asp:Button runat="server" ID="btnAddHazard" Text="AGREGAR HAZARDS" height="40" class="btn btn-outline-primary ml-2" data-toggle="modal" data-target="#myModal2" OnClick="btnAddHazard_Click" />
                                </div>
                     
                                <%-- OnPageIndexChanging="tablePagination_PageIndexChanging" 
                                                        OnPreRender="tablePagination_PreRender"     
                                                        OnRowCommand="tablePagination_RowCommand"   
                                                        OnRowDataBound="tablePagination_RowDataBound"--%>
                             
                                <div class="bokindetalle" style="height:100%; width:100%; overflow:auto">    
                                    <asp:GridView ID="tablePagination" runat="server" AutoGenerateColumns="False"  
                                                DataKeyNames="id"
                                                GridLines="None" 
                                                PageSize="8"
                                                AllowPaging="True"
                                                OnRowCommand="tablePagination_RowCommand" 
                                                OnRowDataBound="tablePagination_RowDataBound"
                                                CssClass="display table table-bordered">
                                               
                                            <PagerStyle HorizontalAlign = "Right" CssClass="display table table-bordered"  />
                                            <RowStyle  BackColor="#F0F0F0" />
                                            <alternatingrowstyle  BackColor="#FFFFFF" />
                           
                                            <Columns>
                                                <asp:BoundField DataField="id" visible ="false" HeaderText="QTY" SortExpression="qty" HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>
                                                <asp:BoundField DataField="idBooking" visible ="false" HeaderText="QTY" SortExpression="qty" HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>
                                                <asp:BoundField DataField="un_na_number" HeaderText="UN/NA NUMBER" HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>
                                                <asp:BoundField DataField="imdgClass" HeaderText="IMDG" HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>
                                                <asp:BoundField DataField="hazardNumberType" HeaderText="TYPE" SortExpression="hazardNumberType" HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>                                                                

                                                <asp:TemplateField HeaderText="" ItemStyle-CssClass="center hidden-phone">
                                                    <ItemTemplate>
                                                        <asp:Button runat="server" ID="btnEditarSubDetalle"  CommandName="Editar" Text="Editar"  CommandArgument='<%# Bind("id") %>' class="btn btn-primary"  data-toggle="modal" data-target="#myModal2"  />

                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="" ItemStyle-CssClass="center hidden-phone">
                                                    <ItemTemplate>
                                                        <asp:Button runat="server" ID="btnAnularSubDetalle"  CommandName="Anular" Text="Anular"  CommandArgument='<%# Bind("id") %>' class="btn btn-primary"  data-toggle="modal" data-target="#myModal4"  />
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                            </Columns>
                                    </asp:GridView>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
               
                </div> <!-- col-sm-3-->
            
                <div class="col-sm-8">
                   
                    <div class="dashboard-container p-4" id="Div2" runat="server" style="height:100%">
                            <asp:UpdatePanel ID="UPDET" runat="server" UpdateMode="Conditional" >  
                                <ContentTemplate>

                                    <div class="form-title ">
                                        <asp:Button runat="server" ID="btnAddDetalle" Text="AGREGAR BOOKING ITEMS" height="40" class="btn btn-outline-primary ml-2" data-toggle="modal" data-target="#myModal" OnClick="btnAddDetalle_Click" />
                                    </div>

                                               
                                    <div class="bokindetalle" style="height:100%; width:100%; overflow:auto">       

                                        <asp:GridView ID="GrillaDetalle" runat="server" AutoGenerateColumns="False"  
                                                                DataKeyNames="id"
                                                                GridLines="None" 
                                                                PageSize="200"
                                                                AllowPaging="True"
                                                                OnRowDataBound="GrillaDetalle_RowDataBound"
                                                                OnRowCommand="GrillaDetalle_RowCommand" 
                                                                CssClass="display table table-bordered">

                                                                                           
                                                <%--<PagerStyle HorizontalAlign = "Right" CssClass="table table-bordered invoice"  />--%>
                                                <PagerStyle HorizontalAlign = "Right" CssClass="display table table-bordered"  />
                                                <RowStyle  BackColor="#F0F0F0" />
                                                <alternatingrowstyle  BackColor="#FFFFFF" />
                           
                                            <Columns>       
                                                              
                                                <asp:BoundField DataField="id" visible ="false" HeaderText="QTY" SortExpression="qty" HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>
                                                <asp:BoundField DataField="idBooking" visible ="false" HeaderText="QTY" SortExpression="qty" HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>
                                                                            
                                                <asp:BoundField DataField="qty"  HeaderText="QTY" SortExpression="qty" HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>
                                                <asp:BoundField DataField="equipmentType" HeaderText="ISO"  HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>
                                                <asp:BoundField DataField="ISOgroup" HeaderText="ISO GROUP" HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>
                                                <asp:BoundField DataField="length" HeaderText="LENGTH" Visible="true" HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>
                                                <asp:BoundField DataField="height" HeaderText="HEIGHT" HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>
                                                <%--<asp:BoundField DataField="isOOG"  HeaderText="is OOG"  HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>--%>
                                                <asp:TemplateField HeaderText="is OOG" ItemStyle-CssClass="center hidden-phone" >
                                                    <ItemTemplate>
                                                        <asp:UpdatePanel ID="UPisOOG" runat="server" ChildrenAsTriggers="true">
                                                            <ContentTemplate>
                                                                <asp:CheckBox ID="CHKGridisOOG" runat="server" Checked='<%# Bind("isOOG") %>'  CssClass="center hidden-phone" Enabled="false"/>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:BoundField DataField="commodity" HeaderText="COMMODITY" HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>
                                                <%--<asp:BoundField DataField="FECHA_REGISTRO" HeaderText="F. ULTIMO PROCESO"   HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>
                                                <asp:BoundField DataField="USUARIO_REGISTRA" HeaderText="USUARIO PROCESO"   HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>
                                                <asp:BoundField DataField="ESTADO_REGISTRO" HeaderText="ESTADO REGISTRO" Visible ="false"  HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>--%>

                                                <asp:TemplateField HeaderText="" ItemStyle-CssClass="center hidden-phone">
                                                    <ItemTemplate>
                                                        <asp:Button runat="server" ID="btnEditarDetalle"  CommandName="Editar" Text="Editar"  CommandArgument='<%# Bind("id") %>' class="btn btn-primary"  data-toggle="modal" data-target="#myModal"  />                                                                                                                           

                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="" ItemStyle-CssClass="center hidden-phone">
                                                    <ItemTemplate>
                                                        <asp:Button runat="server" ID="btnAnularDetalle"  CommandName="Anular" Text="Anular"  CommandArgument='<%# Bind("id") %>' class="btn btn-primary"  data-toggle="modal" data-target="#myModal5"  />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                            <%--      </div>
                                            </div><!--content-panel-->
                                        </div><!--row mb-->
                                    </section><!--wrapper2-->--%>
                                </ContentTemplate>   
                            </asp:UpdatePanel>   
                      
                    </div>
                   
                </div> <!-- col-sm-9-->
          </div><!-- row mt-->
        
            <br />
            <asp:UpdatePanel ID="UPBOTONES" runat="server" UpdateMode="Conditional" >  
                <ContentTemplate>
                
                    <div class="form-group">
                        <div class="alert alert-warning" id="banmsg_det" runat="server" clientidmode="Static"><b>Error!</b> >Debe ingresar el número de la carga MRN......</div>
                    </div>
                  
                </ContentTemplate>
            </asp:UpdatePanel>   
	    
            <div id="myModal" class="modal fade" tabindex="-1" role="dialog">
                <div class="modal-dialog" role="document" style="max-width: 1000px"> <!-- Este tag style controla el ancho del modal -->
                    <div class="modal-content">
                        <div class="modal-header">
                            <h5 class="modal-title">BOOKING ITEMS</h5>
                            <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span>
                            </button>
                        </div>
                        <div class="modal-body">

                         
                            <asp:UpdatePanel ID="UPEDIT_ITEM" runat="server" UpdateMode="Conditional" >  
                                <ContentTemplate>
                                    <div id="div_Codigos1" style="visibility:hidden;">
                                        <asp:HiddenField ID="hdf_CodigoDet" runat="server" />
                                    </div>

                                   
                                    <div class="form-row">
                                        <div class="form-group col-md-2">
                                            <label for="inputAddress">Cantidad :<span style="color: #FF0000; font-weight: bold;"></span></label>
                                            <div class="d-flex">
                                                <asp:TextBox ID="txtQty"  runat="server" class="form-control" MaxLength="9" 
                                                    onkeypress="return soloLetras(event,'1234567890',true)"    
                                                    placeholder="QUANTITY" Font-Bold="false" ></asp:TextBox>
                                            </div>
                                        </div>
                                        <%--<div class="form-group col-md-3">
                                            <label for="inputAddress">Tipo Equipo :<span style="color: #FF0000; font-weight: bold;"></span></label>
                                            <div class="d-flex">
                                                <asp:TextBox ID="txtEquipmentType"  runat="server" class="form-control" MaxLength="50"   placeholder="EQUIPMENT TYPE" Font-Bold="false" ></asp:TextBox>
                                            </div>
                                        </div>--%>
                                        <div class="form-group   col-md-4"> 
                                            <label for="inputAddress">Tipo Equipo:<span style="color: #FF0000; font-weight: bold;"></span></label>
                                            <div class="d-flex">
                                                <asp:TextBox ID="txtEquipmentType" runat="server" MaxLength="50" 
                                                    placeholder="EQUIPMENT TYPE"
                                                    class="form-control"
                                                    style="text-transform:uppercase;"
                            
                                                    ClientIDMode="Static" >
                                                </asp:TextBox>
                                                    &nbsp;
                                                <img alt="loading.." src="../shared/imgs/loader.gif" id="loader3" class="nover"  />
                                                <asp:Button ID="btnBuscarISO" runat="server"  CssClass="btn btn-primary" Text="Buscar" OnClientClick="return prepareObjectRuc()" ToolTip="Busca al Cliente por el RUC." OnClick="btnBuscarISO_Click"/>
                        
                                            </div>
                                        </div>



                                        <div class="form-group col-md-3"> 
                                            <label for="inputAddress">Longitud :<span style="color: #FF0000; font-weight: bold;"></span></label>
                                            <div class="d-flex">
                                                <asp:DropDownList ID="cmbLength" class="form-control" runat="server" Font-Size="Medium" Font-Bold="true" disabled ></asp:DropDownList>
                                            </div>
                                        </div>

                                        <div class="form-group col-md-3"> 
                                            <label for="inputAddress"> Altura:<span style="color: #FF0000; font-weight: bold;"></span></label>
                                            <div class="d-flex">
                                                <asp:DropDownList ID="cmbHeight" class="form-control" runat="server" Font-Size="Medium" Font-Bold="true" disabled ></asp:DropDownList>
                                            </div>
                                        </div>                                   

                                        <div class="form-group col-md-2">
                                            <label for="inputAddress">Producto :<span style="color: #FF0000; font-weight: bold;"></span></label>
                                            <div class="d-flex">
                                                <asp:TextBox ID="txtCommodity"  runat="server" class="form-control" MaxLength="50"   placeholder="COMMODITY" Font-Bold="false" ></asp:TextBox>
                                            </div>
                                        </div>

                                        <div class="form-group col-md-10">
                                            <label for="inputAddress">Producto Descripción :<span style="color: #FF0000; font-weight: bold;"></span></label>
                                            <div class="d-flex">
                                                <asp:TextBox ID="txtCommodityDesc"  runat="server" class="form-control" MaxLength="50"   placeholder="COMMODITY DESCRIPTION" Font-Bold="false" ></asp:TextBox>
                                            </div>
                                        </div>

                                        <div class="form-group col-md-3"> 
                                            <label for="inputAddress">ISO Grupo :<span style="color: #FF0000; font-weight: bold;"></span></label>
                                            <div class="d-flex">
                                                <%--<asp:DropDownList ID="cmbISOgroup" class="form-control" runat="server" Font-Size="Medium" Font-Bold="true" ></asp:DropDownList>--%>
                                                <asp:TextBox ID="txtISOgroup"  runat="server" class="form-control" MaxLength="50"   placeholder="ISO GROUP" Font-Bold="false" disabled ></asp:TextBox>
                                            </div>
                                        </div>

                                        <div class="form-group col-md-3">
                                            <label for="inputAddress">Peso Bruto (kg) :<span style="color: #FF0000; font-weight: bold;"></span></label>
                                            <div class="d-flex">
                                                <asp:TextBox ID="txtGrossWeightKg"  runat="server" class="form-control" MaxLength="50"   placeholder="GROSS WEIGHT KG" Font-Bold="false" ></asp:TextBox>
                                            </div>
                                        </div>

                                        <div class="form-group col-md-3">
                                            <label for="inputAddress">Temperatura Requerida :<span style="color: #FF0000; font-weight: bold;"></span></label>
                                            <div class="d-flex">
                                                <asp:TextBox ID="txtTempRequired"  runat="server" class="form-control" MaxLength="50"   placeholder="TEMP REQUIRED" Font-Bold="false" ></asp:TextBox>
                                            </div>
                                        </div>

                                        <div class="form-group col-md-3">
                                            <label for="inputAddress">Ventilación Requerida :<span style="color: #FF0000; font-weight: bold;"></span></label>
                                            <div class="d-flex">
                                                <asp:TextBox ID="txtVentilationRequired"  runat="server" class="form-control" MaxLength="9" 
                                                    onkeypress="return soloLetras(event,'1234567890',true)"   
                                                    placeholder="VENTILATION REQUIRED" Font-Bold="false" ></asp:TextBox>
                                            </div>
                                        </div>

                                        <div class="form-group col-md-3"> 
                                            <label for="inputAddress">Ventilación Unidad :<span style="color: #FF0000; font-weight: bold;"></span></label>
                                            <div class="d-flex">
                                                <asp:DropDownList ID="cmbVentilationUnit" class="form-control" runat="server" Font-Size="Medium" Font-Bold="true" ></asp:DropDownList>
                                            </div>
                                        </div>

                                        <div class="form-group col-md-3">
                                            <label for="inputAddress">CO2 Requerido :<span style="color: #FF0000; font-weight: bold;"></span></label>
                                            <div class="d-flex">
                                                <asp:TextBox ID="txtCO2Required"  runat="server" class="form-control" MaxLength="9" 
                                                    onkeypress="return soloLetras(event,'1234567890',true)"  
                                                    placeholder="CO2 REQUIRED" Font-Bold="false" ></asp:TextBox>
                                            </div>
                                        </div>

                                        <div class="form-group col-md-3">
                                            <label for="inputAddress">O2 Requerido :<span style="color: #FF0000; font-weight: bold;"></span></label>
                                            <div class="d-flex">
                                                <asp:TextBox ID="txtO2Required"  runat="server" class="form-control" MaxLength="9" 
                                                    onkeypress="return soloLetras(event,'1234567890',true)"
                                                    placeholder="O2 REQUIRED" Font-Bold="false" ></asp:TextBox>
                                            </div>
                                        </div>

                                        <div class="form-group col-md-3">
                                            <label for="inputAddress">Humedad Requerida :<span style="color: #FF0000; font-weight: bold;"></span></label>
                                            <div class="d-flex">
                                                <asp:TextBox ID="txtHumidityRequired"  runat="server" class="form-control" MaxLength="9" 
                                                    onkeypress="return soloLetras(event,'1234567890',true)"
                                                    placeholder="HUMIDITY REQUIRED" Font-Bold="false" ></asp:TextBox>
                                            </div>
                                        </div>

                                        <div class="form-group col-md-3">
                                            <label for="inputAddress">Over Long Back :<span style="color: #FF0000; font-weight: bold;"></span></label>
                                            <div class="d-flex">
                                                <asp:TextBox ID="txtOverLongBack"  runat="server" class="form-control" MaxLength="50"   placeholder="OVER LONG BACK" Font-Bold="false" ></asp:TextBox>
                                            </div>
                                        </div>

                                        <div class="form-group col-md-3">
                                            <label for="inputAddress">Over Long Front :<span style="color: #FF0000; font-weight: bold;"></span></label>
                                            <div class="d-flex">
                                                <asp:TextBox ID="txtOverLongFront"  runat="server" class="form-control" MaxLength="50"   placeholder="OVER LONG FRONT" Font-Bold="false" ></asp:TextBox>
                                            </div>
                                        </div>

                                        <div class="form-group col-md-3">
                                            <label for="inputAddress">Over Wide Left :<span style="color: #FF0000; font-weight: bold;"></span></label>
                                            <div class="d-flex">
                                                <asp:TextBox ID="txtOverWideLeft"  runat="server" class="form-control" MaxLength="50"   placeholder="OVER WIDE LEFT" Font-Bold="false" ></asp:TextBox>
                                            </div>
                                        </div>

                                        <div class="form-group col-md-3">
                                            <label for="inputAddress">Over Wide Right :<span style="color: #FF0000; font-weight: bold;"></span></label>
                                            <div class="d-flex">
                                                <asp:TextBox ID="txtOverWideRight"  runat="server" class="form-control" MaxLength="50"   placeholder="OVER WIDE RIGHT" Font-Bold="false" ></asp:TextBox>
                                            </div>
                                        </div>

                                        <div class="form-group col-md-3">
                                            <label for="inputAddress">Over Height :<span style="color: #FF0000; font-weight: bold;"></span></label>
                                            <div class="d-flex">
                                                <asp:TextBox ID="txtOverHeight"  runat="server" class="form-control" MaxLength="50"   placeholder="OVER HEIGHT" Font-Bold="false" ></asp:TextBox>
                                            </div>
                                        </div>

                                        <div class="form-group col-md-3">
                                            <label for="inputEmail4"> &nbsp;<span style="color: #FF0000; font-weight: bold;"> &nbsp;</span></label>
                                            <asp:UpdatePanel ID="UPCHK1" runat="server"  UpdateMode="Conditional" ChildrenAsTriggers="true">
                                                <ContentTemplate>
                                                    <label class="checkbox-container" runat="server" id="Servicio" >
                                                        <input  id="ChkisOOG" class="form-check-input" type="checkbox"  runat="server" value="false" />
                                                            <span class="checkmark"></span>
                                                        <label class="form-check-label" for="inlineCheckbox1">Is OOG</label>  
                                                    </label>
                                                </ContentTemplate> 
                                                <Triggers>
                                                    <%--<asp:AsyncPostBackTrigger ControlID="BtnBuscar" />--%>
                                                </Triggers>
                                            </asp:UpdatePanel> 
                                        </div>

                                        <div class="form-group col-md-12">
                                            <label for="inputAddress">Observaciones :<span style="color: #FF0000; font-weight: bold;"></span></label>
                                            <div class="d-flex">
                                                <%--<asp:TextBox ID="txtRemarks"  runat="server" class="form-control" MaxLength="50"   placeholder="REMARKS" Font-Bold="false" ></asp:TextBox>--%>
                                                <asp:TextBox TextMode="MultiLine" class="form-control"  ID="txtRemarks" runat="server" MaxLength="50" placeholder="REMARKS" onkeypress="return soloLetras(event,'abcdefghijklmnopqrstuvwxyz01234567890/')"></asp:TextBox>
                                            </div>
                                        </div>

                                    </div>


                                    <div></div>

                                    <div class="row">
                                        <div class="col-md-12 d-flex justify-content-center">
                                            <asp:Button ID="btnGrabarDetalle" runat="server" class="btn btn-primary"  Text="GRABAR ITEM"  OnClientClick="return mostrarloader('2')"  OnClick="btnGrabarDetalle_Click"  />
                                            <img alt="loading.." src="../lib/file-uploader/img/loading.gif" height="32px" width="32px"  id="ImgCargaDet" class="nover"   />
                                            <span id="imagens2"></span>
                                        </div>
                                    </div>

                                        <br/>
                                    <div class="row">
                                        <div class="col-md-12 d-flex justify-content-center">
                                            <div class="alert alert-warning" id="msjErrorDetalle" runat="server" clientidmode="Static"><b>Error!</b> >......</div>
                                        </div>
                                    </div>

			                    </ContentTemplate>
                            </asp:UpdatePanel>
                      


                        </div>
                        <div class="modal-footer d-flex justify-content-center">
                            <button type="button" class="btn btn-outline-primary " data-dismiss="modal">Cancelar</button>                            
                        </div>
                    </div>
                </div>
            </div>

            <div id="myModal2" class="modal fade" tabindex="-1" role="dialog">
                <div class="modal-dialog" role="document" style="max-width: 1000px"> <!-- Este tag style controla el ancho del modal -->
                    <div class="modal-content">
                        <div class="modal-header">
                            <h5 class="modal-title">HAZARD</h5>
                            <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span>
                            </button>
                        </div>
                        <div class="modal-body">

                         
                            <asp:UpdatePanel ID="UPEDIT_HAZARD" runat="server" UpdateMode="Conditional" >  
                                <ContentTemplate>

                                    <div id="div_Codigos1" style="visibility:hidden;">
                                        <asp:HiddenField ID="hdf_CodigoSubDet" runat="server" />
                                    </div>
                                   
                                    <div class="form-row">
                                        <div class="form-group col-md-12">
                                            <%--<label for="inputAddress">UN/NA Numero :<span style="color: #FF0000; font-weight: bold;"></span></label>--%>
                                            <div class="d-flex">
                                                <asp:TextBox ID="txtUn_na_number"  runat="server" class="form-control" MaxLength="50"   placeholder="UN/NA NUMBER" Font-Bold="false" ></asp:TextBox>
                                                    &nbsp;
                                               <%-- <asp:Button ID="btnAddHazard" Text="Agregar" runat="server" class="btn btn-primary"  data-toggle="tooltip" data-placement="top" title="Registra Hazard"  UseSubmitBehavior="false" data-dismiss="modal" />--%>
                                            </div>
                                        </div>

                                        <div class="form-group col-md-12"> 
                                            <label for="inputAddress">Hazard Number Type :<span style="color: #FF0000; font-weight: bold;"></span></label>
                                            <div class="d-flex">
                                                <asp:DropDownList ID="cmbHazardNumberType" class="form-control" runat="server" Font-Size="Medium" Font-Bold="true" ></asp:DropDownList>
                                            </div>
                                        </div>

                                        <div class="form-group col-md-12"> 
                                            <label for="inputAddress">IMDG Class :<span style="color: #FF0000; font-weight: bold;"></span></label>
                                            <div class="d-flex">
                                                <asp:DropDownList ID="cmbImdgClass" class="form-control" runat="server" Font-Size="Medium" Font-Bold="true" ></asp:DropDownList>
                                            </div>
                                        </div>
                                        
                                    </div>

                                    <div></div>

                                    <div class="row">
                                        <div class="col-md-12 d-flex justify-content-center">
                                            <asp:Button ID="btnGrabarSubDet" runat="server" class="btn btn-primary"  Text="GRABAR HAZARD"  OnClientClick="return mostrarloader('2')"  OnClick="btnGrabarSubDet_Click"  />
                                            <img alt="loading.." src="../lib/file-uploader/img/loading.gif" height="32px" width="32px"  id="ImgCargaSubDet" class="nover"   />
                                            <span id="imagens1"></span>
                                        </div>
                                    </div>

                                        <br/>
                                    <div class="row">
                                        <div class="col-md-12 d-flex justify-content-center">
                                            <div class="alert alert-warning" id="msjErrorSubDetalle" runat="server" clientidmode="Static"><b>Error!</b> >......</div>
                                        </div>
                                    </div>

			                    </ContentTemplate>
                            </asp:UpdatePanel>
                      


                        </div>
                        <div class="modal-footer d-flex justify-content-center">
                            <button type="button" class="btn btn-outline-primary " data-dismiss="modal">Cancelar</button>
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
                                    <h4 class="modal-title" id="myModalLabel1">Confirmar Generación</h4>
                                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                                  
                                </div>
                                <div class="modal-body">
                                    <br />
                                  Si usted da click en SI, se procederá a generar el archivo EDI COPARN, está seguro?
                                    <br />
                                    <br />
                                </div>
                                <div class="modal-footer">
                                     <asp:Button ID="BtnProcesar" runat="server" class="btn btn-outline-primary"  Text="SI" UseSubmitBehavior="false" data-dismiss="modal" OnClick="BtnProcesar_Click" />
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

            <asp:UpdatePanel ID="UPMENSAJE1" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">  
                     <ContentTemplate>

                         
                         <div id="div_Codigos3" style="visibility:hidden;">
                            <asp:HiddenField ID="hdf_codigoSubDetalle" runat="server" />
                         </div>

                          <div class="modal fade" id="myModal4" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
                            <div class="modal-dialog">
                              <div class="modal-content">
                                <div class="modal-header">
                                    <h4 class="modal-title" id="myModalLabel">Confirmar Anulación</h4>
                                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                                  
                                </div>
                                <div class="modal-body">
                                    <br />
                                  Si usted da click en SI, se procederá a anular el hazard, está seguro?
                                    <br />
                                    <br />
                                </div>
                                <div class="modal-footer">
                                     <asp:Button ID="btnAnularHazard" runat="server" class="btn btn-outline-primary" OnClick="btnAnularHazard_Click"  Text="SI" UseSubmitBehavior="false" data-dismiss="modal" />
                                  <button type="button" class="btn btn-primary" data-dismiss="modal">NO</button>
                                </div>
                              </div>
                            </div>
                          </div>
                     </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnAnularHazard" />
                        </Triggers>
            </asp:UpdatePanel>

            <asp:UpdatePanel ID="UPMENSAJE2" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">  
                     <ContentTemplate>

                         <div id="div_Codigos2" style="visibility:hidden;">
                            <asp:HiddenField ID="hdf_codigoDetalle" runat="server" />
                         </div>

                          <div class="modal fade" id="myModal5" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
                            <div class="modal-dialog">
                              <div class="modal-content">
                                <div class="modal-header">
                                    <h4 class="modal-title" id="myModalLabel2">Confirmar Anulación</h4>
                                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                                  
                                </div>
                                <div class="modal-body">
                                    <br />
                                  Si usted da click en SI, se procederá a anular el item, está seguro?
                                    <br />
                                    <br />
                                </div>
                                <div class="modal-footer">
                                     <asp:Button ID="btnAnularItem" runat="server" class="btn btn-outline-primary" OnClick="btnAnularItem_Click"  Text="SI" UseSubmitBehavior="false" data-dismiss="modal" />
                                  <button type="button" class="btn btn-primary" data-dismiss="modal">NO</button>
                                </div>
                              </div>
                            </div>
                          </div>
                     </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnAnularItem" />
                        </Triggers>
            </asp:UpdatePanel>

            <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">  
                     <ContentTemplate>

                         <div id="div_Codigos4" style="visibility:hidden;">
                            <asp:HiddenField ID="HiddenField1" runat="server" />
                         </div>

                          <div class="modal fade" id="myModal6" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
                            <div class="modal-dialog">
                              <div class="modal-content">
                                <div class="modal-header">
                                    <h4 class="modal-title" id="myModalLabel3">Confirmar Anulación</h4>
                                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                                  
                                </div>
                                <div class="modal-body">
                                    <br />
                                  Si usted da click en SI, se procederá a generar la anulación, está seguro?
                                    <br />
                                    <br />
                                </div>
                                <div class="modal-footer">
                                     <asp:Button ID="btnAnular" runat="server" class="btn btn-outline-primary" OnClick="btnAnular_Click"  Text="SI" UseSubmitBehavior="false" data-dismiss="modal" />
                                  <button type="button" class="btn btn-primary" data-dismiss="modal">NO</button>
                                </div>
                              </div>
                            </div>
                          </div>
                     </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnAnularItem" />
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
                document.getElementById("ImgCargaSubDet").className = 'nover';
                document.getElementById("ImgCargaDet").className='nover';
            }

             } catch (e) {
                alert('Lamentamos comunicarle que ha ocurrido el siguiente problema:\n' + e.Message);
            }
    }

    function popupCallback(catalogo) {
        if (catalogo == null || catalogo == undefined) {
            alert('Hubo un problema al setaar un objeto de catalogo');
            return;
        }

        this.document.getElementById('<%= txtVesselVisit.ClientID %>').value = catalogo.codigo;
    }

    function setPort(valor) {
        sw = valor;
        window.open('../catalogo/puerto.aspx', 'name', 'width=850,height=880');
    }

    function popupCallback2(objeto, catname) {
        if (catname == 'puerto') {
            var opt = objeto.opcion;
            if (opt == null || opt == undefined) {
                return;
            }
            if (opt == 10) {
                document.getElementById('<%= txtPortOfDischarge.ClientID %>').value = objeto.codigo;
            }

            if (opt == 20) {
                document.getElementById('<%= txtSecportOfDischarge.ClientID %>').value = objeto.codigo;
            }

            if (opt == 30) {
                document.getElementById('<%= txtPortOfLoad.ClientID %>').value = objeto.codigo;
            }
            return;
        }

        if (catname == 'coparn') {
            document.getElementById('<%= txtCodigoCab.ClientID %>').value = objeto.row;
            document.getElementById('<%= txtNumber.ClientID %>').value = objeto.nbr;
            document.getElementById('<%= txtCodigoCab.ClientID %>').onchange();
                //document.getElementById('numbook').textContent = objeto.nbr;
                //document.getElementById('nbrboo').value = objeto.nbr;
                //var a = objeto.line.split("-");
                //document.getElementById('referencia').textContent = a[0].toString();
                //document.getElementById('xreferencia').value = a[0].toString(); ;
                //document.getElementById('linea').textContent = a[1].toString();
                //document.getElementById('xlinea').value = a[1].toString();
                //document.getElementById('ruc').value = objeto.ruc;
                //document.getElementById('nbqty').textContent = objeto.cant_bkg;
                //document.getElementById('bkqty').value = objeto.cant_bkg;
                //document.getElementById('cantr').textContent = objeto.reservado;
                //document.getElementById('resqty').value = objeto.reservado;
                //document.getElementById('cantd').textContent = objeto.despachado;
                //document.getElementById('desqty').value = objeto.despachado;
                //document.getElementById('cants').textContent = objeto.cant_bkg - objeto.reservado - objeto.despachado;
                //document.getElementById('salqty').value = objeto.cant_bkg - objeto.reservado - objeto.despachado
                //document.getElementById('nave').value = objeto.nave;
                return;
            }
            
        }

</script>

     <!--SCRIPT PARA MODAL-->
<%--    <script type="text/javascript" src="https://code.jquery.com/jquery-3.5.1.slim.min.js" integrity="sha384-DfXdz2htPH0lsSSs5nCTpuj/zy4C+OGpamoFVy38MVBnE+IbbVYUew+OrCXaRkfj" crossorigin="anonymous"></script>--%>


</asp:Content>
