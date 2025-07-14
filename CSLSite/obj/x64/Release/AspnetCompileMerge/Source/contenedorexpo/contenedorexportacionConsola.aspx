<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="contenedorexportacionConsola.aspx.cs" Inherits="CSLSite.contenedorexpo.contenedorexportacionConsola" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>


<asp:Content ID="Content1" ContentPlaceHolderID="placehead" runat="server">
<%--    <link href="../img/favicon2.png" rel="icon"/>
  <link href="../img/icono.png" rel="apple-touch-icon"/>
  <link href="../lib/bootstrap/css/bootstrap.min.css" rel="stylesheet"/>
  <link href="../lib/font-awesome/css/font-awesome.css" rel="stylesheet" />
  <link rel="stylesheet" type="text/css" href="../lib/gritter/css/jquery.gritter.css" />   
  <link rel="stylesheet" href="../lib/file-uploader/css/jquery.fileupload.css"/>
  <link rel="stylesheet" href="../lib/file-uploader/css/jquery.fileupload-ui.css"/>
  <link href="../css/style.css" rel="stylesheet"/>
  <link href="../css/style-responsive.css" rel="stylesheet"/>
  <link href="../css/table-responsive.css" rel="stylesheet"/>
  <link href="../css/calendario_ajax.css" rel="stylesheet"/>--%>

<%--    <link href="../img/favicon2.png" rel="icon"/>
    <link href="../img/icono.png" rel="apple-touch-icon"/>
    <!-- Bootstrap core CSS -->
    <link href="../css/bootstrap.min.css" rel="stylesheet"/>
    <link href="../css/dashboard.css" rel="stylesheet"/>
    <link href="../css/icons.css" rel="stylesheet"/>
    <link href="../css/style.css" rel="stylesheet"/>--%>

    <!--external css-->
    <%--<link href="../shared/estilo/jquery.datetimepicker.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/jquery.datetimepicker.js" type="text/javascript"></script>--%>


   <%--<link href="../lib/advanced-datatable/css/demo_page.css" rel="stylesheet" />
    <link href="../lib/advanced-datatable/css/demo_table.css" rel="stylesheet" />
    <link rel="stylesheet" href="../lib/advanced-datatable/css/DT_bootstrap2.css" />

  <link href="../shared/estilo/tables.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/aisv-general.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/work.css" rel="stylesheet" type="text/css" />--%>
    <link href="../shared/estilo/catalogos.css" rel="stylesheet" type="text/css" />
   <%-- <link href="../shared/estilo/info.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/proforma.css" rel="stylesheet" type="text/css" />--%>

    <link href="../img/favicon2.png" rel="icon"/>
    <link href="../img/icono.png" rel="apple-touch-icon"/>
    <link href="../css/bootstrap.min.css" rel="stylesheet"/>
    <link href="../css/dashboard.css" rel="stylesheet"/>
    <link href="../css/icons.css" rel="stylesheet"/>
    <link href="../css/style.css" rel="stylesheet"/>
    <link href="https://fonts.googleapis.com/css2?family=Roboto:wght@300;400;500;700&display=swap" rel="stylesheet"/>

    <%--<link href="../shared/estilo/Reset.css" rel="stylesheet" />--%>
    <%--<link href="../shared/estilo/jquery.datetimepicker.css" rel="stylesheet" type="text/css" />--%>
    <%--<link href="../shared/estilo/tables.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/info.css" rel="stylesheet" type="text/css" />--%>




  
   <%-- <meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no"/>
    <link rel="canonical" href="https://getbootstrap.com/docs/4.5/examples/dashboard/"/>--%>
    <!-- Custom styles for this template -->
    <%--<link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/5.15.1/css/all.min.css" integrity="sha512-+4zCK9k+qNFUR5X+cKL9EIR+ZOhtIloNl9GIKS57V1MyNsYpYcUrUeQc9vNfzsWfV28IaLL3i96P9sdNyeRssA==" crossorigin="anonymous" />--%>
    <%--<link href="../css/datatables.min.css" rel="stylesheet"/>--%>


  
 

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


    

     <script type="text/javascript">
         $(document).ready(function () {
             $('#<%= GridView1.ClientID %>').dataTable();
         });

    </script>


        

     <script type="text/javascript">
         $(document).ready(function () {
             $('#<%= GridView2.ClientID %>').dataTable();
         });

    </script>


        

     <script type="text/javascript">
         $(document).ready(function () {
             $('#<%= GridView3.ClientID %>').dataTable();
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
       <%-- 
        <div class="form-title">
            CRITERIO DE BUSQUEDA
        </div>--%>

         
            
             <asp:UpdatePanel ID="UPCARGA" runat="server"  UpdateMode="Conditional" ChildrenAsTriggers="true">
                <ContentTemplate>
                    <asp:Panel ID="PanelCualquiera" runat="server" DefaultButton ="BtnBusca">
                        <%--<div class="catawrap">--%>
                            <div class="form-row" >
                                     
                                <div class="form-group col-md-3">
                                    <br />
                                    <div class="d-flex">
                                        &nbsp;
                                        <asp:TextBox ID="TXTNAVE" runat="server" class="form-control" MaxLength="16" onkeypress="return soloLetras(event,'abcdefghijklmnopqrstuvwxyz0123456789-_')"  placeholder="NAVE REFERENCIA"></asp:TextBox>
                                        &nbsp;
                                        <asp:LinkButton runat="server" ID="BtnBusca" Text="<span class='fa fa-search' style='font-size:24px'></span>"  OnClientClick="return mostrarloader('1')" OnClick="BtnBuscar_Click" class="btn btn-primary" />
                                        &nbsp;
                                        <img alt="loading.." src="../lib/file-uploader/img/loading.gif" height="32px" width="32px"  id="ImgCarga" class="nover"/>
                                    
                                    </div>
                                </div>
                            </div>
                        <%--</div>--%>
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
               <div class="col-sm-4" >
                

                     <div class="dashboard-container p-4" id="Div1" runat="server" style="height:1050px" >

                            <asp:UpdatePanel ID="UPBUSCARREPORTE" runat="server" UpdateMode="Conditional" >  
                                <ContentTemplate>
                                     
                                                        
                                    <%--<div >--%>

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

                                  <%--  </div>--%>

                                   
                                       
			                    </ContentTemplate>
                            </asp:UpdatePanel>
                      
                           
		                                              
                                <div class="form-title ">
                                    Detalle de Contenedores de Exportación
                                </div>
                               <%-- <ul class="nav nav-pills nav-stacked labels-info ">
                                     <li>
                                        <h4>Detalle de Contenedores de Exportación                         
                                        </h4> 
                                     </li>   
                                </ul>--%>

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
                                                                    <%-- <Triggers>
                                                                        <asp:AsyncPostBackTrigger ControlID="CHKPRO" />
                                                                    </Triggers>--%>
                                                                </asp:UpdatePanel>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderText="" ItemStyle-CssClass="center hidden-phone">
                                                            <ItemTemplate>
                                                                <asp:LinkButton runat="server" ID="btnVisualizar" Height="30px"  CommandName="Seleccionar" Text="<span  style='align-content:center;' class='fa fa-eye'></span>"  CommandArgument='<%#jsarguments( Eval("CNTR_BKNG_BOOKING"), Eval("CNTR_ID") )%>' CssClass="btn btn-primary"  />
                                                                <%--<asp:Button runat="server" ID="IncreaseButton" Text="" CommandName="Seleccionar" CommandArgument='<%# Bind("CNTR_BKNG_BOOKING") %>' class="btn btn-theme08" /> --%>
                                                               
                                                            </ItemTemplate>
                                                        </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="" ItemStyle-CssClass="center hidden-phone">
                                                            <ItemTemplate>
                                                                <%--<asp:Button runat="server" ID="btnActualizar" Text="<span class='fa fa-check' style='font-size:24px'></span>"   CommandName="Actualizar" CommandArgument='<%# Bind("CNTR_ID") %>' class="btn btn-theme08" /> --%>
                                                                <%--<asp:LinkButton runat="server" ID="btnActualizar" Height="30px" CommandName="Actualizar" Text="<span class='fa fa-refresh'></span>"  CommandArgument='<%# Bind("CNTR_ID") %>' CssClass="btn btn-theme06" class="btn btn-theme08"  data-toggle="modal" data-target="#myModal" />--%>
                                                                <asp:Button runat="server" ID="IncreaseButton3"  Height="30px" Text="RW"  CommandName="Actualizar" CommandArgument='<%#Eval("CNTR_ID")%>' class="btn btn-primary" data-toggle="modal" data-target="#myModal" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderText="" ItemStyle-CssClass="center hidden-phone">
                                                            <ItemTemplate>
                                                                    <%--<asp:LinkButton runat="server" ID="btnFactura" Height="30px" CommandName="Factura" Text="<span class='fa fa-clone'></span>"  CommandArgument='<%# Bind("CNTR_ID") %>' CssClass="btn btn-theme06"  data-toggle="modal" data-target="#myModal3"  />                                                                       --%>
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

            
              <div class="col-sm-8">
                   
                        <div class="dashboard-container p-4" id="Div2" runat="server" style="height:1050px">
                       <%--      <div class="panel-body minimal">
                      
                                 <div class="table-inbox-wrap">
                           


                                     <div class="modal-body" >--%>
                            
                                        <asp:UpdatePanel ID="UPDET" runat="server" UpdateMode="Conditional" >  
                                            <ContentTemplate>

                                                <%--<div >--%>
                                                     <div class="form-group col-md-12">
                                                         <label for="inputZip">BOOKING:<span style="color: #FF0000; font-weight: bold;"></span></label>
                                                         <div class="d-flex">
                                                            <asp:TextBox ID="txtID"  runat="server" class="form-control"   placeholder="" size="16"  Font-Bold="false" disabled></asp:TextBox>
                                                        </div>
                                                    </div>
                                                <%--</div>--%>
                                        
                                                <br />
<%--                                                <section class="wrapper2">
                                                    <div class="row mb"> 
                                                        <div class="content-panel">
                                                            <div class="adv-table">--%>
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
                                                      <%--      </div>
                                                        </div><!--content-panel-->
                                                    </div><!--row mb-->
                                                </section><!--wrapper2-->--%>
                                            </ContentTemplate>   
                                        </asp:UpdatePanel>   
                        <%--            </div>



					            </div><!-- table-inbox-wrap-->
                             </div><!-- panel-body minimal-->--%>
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
	    


            <%--<div class="modal fade" id="myModal2" tabindex="-1" role="dialog" aria-labelledby="myModalLabel2" aria-hidden="true">
                    <div class="modal-dialog">
                        <div class="modal-content" style="width:1100px">
                            <div class="modal-header">
                                <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                                <h4 class="modal-title" id="myModalLabel2">HISTORIAL DE PROCESAMIENTO</h4>
                            </div>
                            <div class="modal-body" >
                            
                                <asp:UpdatePanel ID="UPMODAL" runat="server" UpdateMode="Conditional" >  
                                    <ContentTemplate>
                                        <asp:Panel ID="Panel1" Visible="false"  runat="server">         
                                            <p>CONTAINER:<asp:TextBox ID="txtContainers"  runat="server" class="form-control"   placeholder="" size="16"  Width="300px" Font-Bold="false" disabled></asp:TextBox></p>
                                        </asp:Panel>
                      
                                        <section class="wrapper2">
                                            <div class="row mb"> 
                                                <div class="content-panel">
                                                    <div class="adv-table">
                                            
                                                        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False"  DataKeyNames="CNTR_CONSECUTIVO"
                                                                                GridLines="None" 
                                                                                PageSize="20"
                                                                                AllowPaging="True"
                                                                                CssClass="display table table-bordered">
                                                                <PagerStyle HorizontalAlign = "Right" CssClass="pagination-ys"  />
                                                                <RowStyle  BackColor="#F0F0F0" />
                                                                <alternatingrowstyle  BackColor="#FFFFFF" />
                           
                                                            <Columns>     
                                                                <asp:BoundField DataField="CNTR_CAB_ID" HeaderText="CNTR_CAB_ID"  Visible ="false" HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>
                                                                <asp:BoundField DataField="CNTR_CONSECUTIVO" HeaderText="CNTR_CONSECUTIVO"  Visible ="false" HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>
                                                                <asp:BoundField DataField="CNTR_CONTAINER"  HeaderText="CONTENEDOR" SortExpression="CNTR_CONTAINER" HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>
                                                                <asp:BoundField DataField="CNTR_TYSZ_SIZE" HeaderText="SIZE"  HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>
                                                                <asp:BoundField DataField="CNTR_TYSZ_ISO" HeaderText="ISO"  HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>
                                                                <asp:BoundField DataField="CNTR_TYSZ_TYPE" HeaderText="TIPO" HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>
                                                                <asp:BoundField DataField="CNTR_FULL_EMPTY_CODE" HeaderText="FULL/EMPTY" HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>
                                                                <asp:BoundField DataField="CNTR_YARD_STATUS" HeaderText="ESTADO"  HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>
                                                                <asp:BoundField DataField="CNTR_AISV" HeaderText="AISV" HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>
                                                                <asp:BoundField DataField="CNTR_HOLD" Visible ="false" HeaderText="HOLD" HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>
                                                                <asp:BoundField DataField="CNTR_REEFER_CONT" HeaderText="OFF POWER"   HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>
                                                                <asp:BoundField DataField="ESTADO_ERROR" HeaderText="ESTADO_ERROR"  Visible="false" HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>

                                                               <!-- <asp:BoundField DataField="CNTR_CONSECUTIVO"  HeaderText="CODIGO" Visible ="true" SortExpression="CNTR_CONSECUTIVO" HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>
                                                                <asp:BoundField DataField="SERVICIOS_ACTUALES" HeaderText="SERVICIOS ACTUALES"  HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>
                                                                <asp:BoundField DataField="TOTAL" HeaderText="TOTAL" HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>
                                                                <asp:BoundField DataField="MSG_DUPLICADO" HeaderText="MSG DUPLICADO" HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>
                                                                <asp:BoundField DataField="MSG_FALTANTE" HeaderText="MSG FALTANTE"  HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>
                                                                <asp:BoundField DataField="MSG_OTROS" HeaderText="MSG OTROS" HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>
                                                                <asp:BoundField DataField="FALTANTES" HeaderText="FALTANTES" HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>
                                                                <asp:BoundField DataField="FECHA_REGISTRO" HeaderText="FECHA REGISTRO"   HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>
                                                                <asp:BoundField DataField="USUARIO_REGISTRA" HeaderText="USUARIO REGISTRA"   HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>
                                                                <asp:BoundField DataField="ESTADO_REGISTRO" HeaderText="ESTADO REGISTRO" Visible ="false"  HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>-->
                                                            </Columns>
                                                        </asp:GridView>
                                                    </div>
                                                </div><!--content-panel-->
                                            </div><!--row mb-->
                                        </section><!--wrapper2-->


                                        <section class="wrapper2">
                                            <div class="row mb"> 
                                                <div class="content-panel">
                                                    <div class="adv-table">
                                            
                                                        <asp:GridView ID="GridView2" runat="server" AutoGenerateColumns="False"  DataKeyNames="CNTR_CONSECUTIVO"
                                                                                GridLines="None" 
                                                                                PageSize="2"
                                                                                AllowPaging="True"
                                                                                CssClass="display table table-bordered"
                                                                                OnRowDataBound="GridView2_RowDataBound"
                                                                                OnRowCommand="GridView2_RowCommand" 
                                                                                OnPageIndexChanging="GridView2_PageIndexChanging" 
                                                                                OnPreRender="GridView2_PreRender">
                                                                <PagerStyle HorizontalAlign = "Right" CssClass="pagination-ys"  />
                                                                <RowStyle  BackColor="#F0F0F0" />
                                                                <alternatingrowstyle  BackColor="#FFFFFF" />                           
                                                                <Columns>     
                                                                    <asp:BoundField DataField="CNTR_CAB_ID" HeaderText="CNTR_CAB_ID"  Visible ="false" HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>
                                                                    <asp:BoundField DataField="CNTR_CONSECUTIVO" HeaderText="CNTR_CONSECUTIVO"  Visible ="false" HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>
                                                                    <asp:BoundField DataField="TIPO_EVENTO"  HeaderText="TIPO EVENTO" SortExpression="CNTR_CONTAINER" HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>
                                                                    <asp:BoundField DataField="FECHA_REGISTRO" HeaderText="F. PROCESO"  HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>
                                                                    <asp:BoundField DataField="USUARIO_REGISTRA"  HeaderText="USUARIO" HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>
                                                                    <asp:BoundField DataField="NOVEDADES" HeaderText="NOVEDADES"  HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>
                                                                

                                                                   <!-- <asp:BoundField DataField="CNTR_CONSECUTIVO"  HeaderText="CODIGO" Visible ="true" SortExpression="CNTR_CONSECUTIVO" HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>
                                                                    <asp:BoundField DataField="SERVICIOS_ACTUALES" HeaderText="SERVICIOS ACTUALES"  HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>
                                                                    <asp:BoundField DataField="TOTAL" HeaderText="TOTAL" HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>
                                                                    <asp:BoundField DataField="MSG_DUPLICADO" HeaderText="MSG DUPLICADO" HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>
                                                                    <asp:BoundField DataField="MSG_FALTANTE" HeaderText="MSG FALTANTE"  HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>
                                                                    <asp:BoundField DataField="MSG_OTROS" HeaderText="MSG OTROS" HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>
                                                                    <asp:BoundField DataField="FALTANTES" HeaderText="FALTANTES" HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>
                                                                    <asp:BoundField DataField="FECHA_REGISTRO" HeaderText="FECHA REGISTRO"   HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>
                                                                    <asp:BoundField DataField="USUARIO_REGISTRA" HeaderText="USUARIO REGISTRA"   HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>
                                                                    <asp:BoundField DataField="ESTADO_REGISTRO" HeaderText="ESTADO REGISTRO" Visible ="false"  HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>-->
                                                                </Columns>
                                                        </asp:GridView>
                                                    </div>
                                                </div><!--content-panel-->
                                            </div><!--row mb-->
                                        </section>

                                    </ContentTemplate>   
                                </asp:UpdatePanel>   
                            </div>
                            <div class="modal-footer">        
                                <button type="button" class="btn btn-primary" data-dismiss="modal">CERRAR</button>
                            </div>
                        </div>
                    </div>
                </div>--%>

        <div id="myModal2" class="modal fade" tabindex="-1" role="dialog">
            <div class="modal-dialog" role="document" style="max-width: 1000px"> <!-- Este tag style controla el ancho del modal -->
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title">HISTORIAL DE PROCESAMIENTO</h5>
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                        </button>
                    </div>
                    <div class="modal-body">

                         <asp:UpdatePanel ID="UPMODAL" runat="server" UpdateMode="Conditional" >  
                                        <ContentTemplate>
                                            <asp:Panel ID="Panel1" Visible="false"  runat="server">         
                                                <p>CONTAINER:<asp:TextBox ID="txtContainers"  runat="server" class="form-control"   placeholder="" size="16"  Width="300px" Font-Bold="false" disabled></asp:TextBox></p>
                                            </asp:Panel>
                      
                                          <%--  <section class="wrapper2">
                                                <div class="row mb"> 
                                                    <div class="content-panel">
                                                        <div class="adv-table">--%>
                                                            <div class="bokindetalle" style="width:100%; overflow:auto">       
                                                                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False"  DataKeyNames="CNTR_CONSECUTIVO"
                                                                                        GridLines="None" 
                                                                                        PageSize="20"
                                                                                        AllowPaging="True"
                                                                                        CssClass="table table-bordered invoice">
                                                                        <PagerStyle HorizontalAlign = "Right" CssClass="table table-bordered invoice"  />
                                                                        <RowStyle  BackColor="#F0F0F0" />
                                                                        <alternatingrowstyle  BackColor="#FFFFFF" />
                           
                                                                    <Columns>     
                                                                        <asp:BoundField DataField="CNTR_CAB_ID" HeaderText="CNTR_CAB_ID"  Visible ="false" HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>
                                                                        <asp:BoundField DataField="CNTR_CONSECUTIVO" HeaderText="CNTR_CONSECUTIVO"  Visible ="false" HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>
                                                                        <asp:BoundField DataField="CNTR_CONTAINER"  HeaderText="CONTENEDOR" SortExpression="CNTR_CONTAINER" HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>
                                                                        <asp:BoundField DataField="CNTR_TYSZ_SIZE" HeaderText="SIZE"  HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>
                                                                        <asp:BoundField DataField="CNTR_TYSZ_ISO" HeaderText="ISO"  HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>
                                                                        <asp:BoundField DataField="CNTR_TYSZ_TYPE" HeaderText="TIPO" HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>
                                                                        <asp:BoundField DataField="CNTR_FULL_EMPTY_CODE" HeaderText="FULL/EMPTY" HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>
                                                                        <asp:BoundField DataField="CNTR_YARD_STATUS" HeaderText="ESTADO"  HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>
                                                                        <asp:BoundField DataField="CNTR_AISV" HeaderText="AISV" HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>
                                                                        <asp:BoundField DataField="CNTR_HOLD" Visible ="false" HeaderText="HOLD" HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>
                                                                        <asp:BoundField DataField="CNTR_REEFER_CONT" HeaderText="OFF POWER"   HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>
                                                                        <asp:BoundField DataField="ESTADO_ERROR" HeaderText="ESTADO_ERROR"  Visible="false" HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>

                                                                  
                                                                    </Columns>
                                                                </asp:GridView>
                                                            </div>
                                                        <%--</div>
                                                    </div><!--content-panel-->
                                                </div><!--row mb-->
                                            </section><!--wrapper2-->--%>


<%--                                            <section class="wrapper2">
                                                <div class="row mb"> 
                                                    <div class="content-panel">
                                                        <div class="adv-table">--%>
                                                            <div class="bokindetalle" style="width:100%; overflow:auto">       
                                                                <asp:GridView ID="GridView2" runat="server" AutoGenerateColumns="False"  DataKeyNames="CNTR_CONSECUTIVO"
                                                                                        GridLines="None" 
                                                                                        PageSize="200"
                                                                                        AllowPaging="True"
                                                                                        CssClass="table table-bordered invoice"
                                                                                        OnRowDataBound="GridView2_RowDataBound"
                                                                                        OnRowCommand="GridView2_RowCommand" 
                                                                                        OnPageIndexChanging="GridView2_PageIndexChanging" 
                                                                                        OnPreRender="GridView2_PreRender">
                                                                        <PagerStyle HorizontalAlign = "Right" CssClass="table table-bordered invoice"  />
                                                                        <RowStyle  BackColor="#F0F0F0" />
                                                                        <alternatingrowstyle  BackColor="#FFFFFF" />                           
                                                                        <Columns>     
                                                                            <asp:BoundField DataField="CNTR_CAB_ID" HeaderText="CNTR_CAB_ID"  Visible ="false" HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>
                                                                            <asp:BoundField DataField="CNTR_CONSECUTIVO" HeaderText="CNTR_CONSECUTIVO"  Visible ="false" HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>
                                                                            <asp:BoundField DataField="TIPO_EVENTO"  HeaderText="TIPO EVENTO" SortExpression="CNTR_CONTAINER" HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>
                                                                            <asp:BoundField DataField="FECHA_REGISTRO" HeaderText="F. PROCESO"  HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>
                                                                            <asp:BoundField DataField="USUARIO_REGISTRA"  HeaderText="USUARIO" HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>
                                                                            <asp:BoundField DataField="NOVEDADES" HeaderText="NOVEDADES"  HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>
                                                                

                                                                      
                                                                        </Columns>
                                                                </asp:GridView>
                                                            </div>
                                                        <%--</div>
                                                    </div><!--content-panel-->
                                                </div><!--row mb-->
                                            </section>--%>

                                        </ContentTemplate>   
                                    </asp:UpdatePanel>

                    </div>
                    <div class="modal-footer d-flex justify-content-center">
                        <button type="button" class="btn btn-outline-primary " data-dismiss="modal">Cancelar</button>
                    </div>
                </div>
            </div>
        </div>

        <div id="myModal3" class="modal fade" tabindex="-1" role="dialog">
            <div class="modal-dialog" role="document" style="max-width: 1000px"> <!-- Este tag style controla el ancho del modal -->
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title">FACTURAS</h5>
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                        </button>
                    </div>
                    <div class="modal-body">

                        <asp:UpdatePanel ID="UPFAC" runat="server" UpdateMode="Conditional" >  
                                    <ContentTemplate>
                                        <asp:Panel ID="Panel2" runat="server">         
                                            <p>NAVE REFERENCIA:<asp:TextBox ID="TXTBOOK"  runat="server" class="form-control"   placeholder="" size="16"  Width="300px" Font-Bold="false" disabled></asp:TextBox></p>
                                        </asp:Panel>
                      
                                        <section class="wrapper2">
                                            <div class="row mb"> 
                                                <div class="content-panel">
                                                    <div class="adv-table">
                                            
                                                        <asp:GridView ID="GridView3" runat="server" AutoGenerateColumns="False"  DataKeyNames="Codigo"
                                                                                GridLines="None" 
                                                                                PageSize="100"
                                                                                AllowPaging="True"
                                                                                CssClass="table table-bordered invoice"
                                                                                OnRowDataBound="GridView3_RowDataBound"
                                                                                OnRowCommand="GridView3_RowCommand" 
                                                                                OnPageIndexChanging="GridView3_PageIndexChanging" 
                                                                                OnPreRender="GridView3_PreRender">
                                                                <PagerStyle HorizontalAlign = "Right" CssClass="table table-bordered invoice"  />
                                                                <RowStyle  BackColor="#F0F0F0" />
                                                                <alternatingrowstyle  BackColor="#FFFFFF" />
                           
                                                            <Columns>     
                                                                <asp:BoundField DataField="Referencia" HeaderText="NAVE"  Visible ="false" HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>
                                                                <asp:BoundField DataField="Codigo" HeaderText="Codigo" Visible ="false" HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>
                                                                <asp:BoundField DataField="NumeroFactura" HeaderText="#FACTURA" HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>
                                                                <asp:BoundField DataField="TipoFactura" HeaderText="TIPO" HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>
                                                                <asp:BoundField DataField="FechaFactura"  HeaderText="F.FACTURA" SortExpression="CNTR_CONTAINER" HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>
                                                                <asp:BoundField DataField="Monto" HeaderText="MONTO"  HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>
                                                                <asp:BoundField DataField="FacturadoA" HeaderText="FACTURADO A"  HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>
                                                                <asp:BoundField DataField="Mail" HeaderText="MAIL" HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>


                                                                 <asp:TemplateField HeaderText="ENVIADO" ItemStyle-CssClass="center hidden-phone" >
                                                                    <ItemTemplate>
                                                                        <asp:UpdatePanel ID="UPPROFAC" runat="server" ChildrenAsTriggers="true">
                                                                            <ContentTemplate>
                                                                                <asp:CheckBox ID="CHKENVIADO" runat="server" Checked='<%# Bind("Enviado") %>'  CssClass="center hidden-phone" Enabled="false"/>
                                                                            </ContentTemplate>
                                                                            <%--  <Triggers>
                                                                                <asp:AsyncPostBackTrigger ControlID="CHKPRO" />
                                                                            </Triggers>--%>
                                                                        </asp:UpdatePanel>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="Seleccionar" ItemStyle-CssClass="center hidden-phone">
                                                                    <ItemTemplate>
                                                                        <a href="../reportesexpo/factura_preview.aspx?id_comprobante=<%#securetext(Eval("Codigo")) %>"  target="_blank"><button type="button" class="btn btn-theme06"><i class="fa fa-print"></i> Imprimir</button></a>
                                                                        <%--<asp:Button runat="server" ID="btnImprimir" Text="Imprimir" CommandName="Imprimir" CommandArgument='<%#Eval("NumeroFactura")%>' class="btn btn-theme08" OnClientClick="BtnVisualizar_Click" OnClick ="BtnVisualizar_Click" />--%>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                            </Columns>
                                                        </asp:GridView>
                                                    </div>
                                                </div><!--content-panel-->
                                            </div><!--row mb-->
                                        </section><!--wrapper2-->


                                        

                                    </ContentTemplate>   
                                </asp:UpdatePanel>


                     </div>
                    <div class="modal-footer d-flex justify-content-center">
                        <button type="button" class="btn btn-outline-primary " data-dismiss="modal">Cancelar</button>
                    </div>
                </div>
            </div>
        </div>

           <%-- <div class="modal fade" id="myModal3" tabindex="-1" role="dialog" aria-labelledby="myModalLabel3" aria-hidden="true">
                    <div class="modal-dialog">
                        <div class="modal-content" style="width:1100px">
                            <div class="modal-header">
                                <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                                <h4 class="modal-title" id="myModalLabel3">FACTURAS</h4>
                            </div>
                            <div class="modal-body" >
                            
                                <asp:UpdatePanel ID="UPFAC" runat="server" UpdateMode="Conditional" >  
                                    <ContentTemplate>
                                        <asp:Panel ID="Panel2" runat="server">         
                                            <p>NAVE REFERENCIA:<asp:TextBox ID="TXTBOOK"  runat="server" class="form-control"   placeholder="" size="16"  Width="300px" Font-Bold="false" disabled></asp:TextBox></p>
                                        </asp:Panel>
                      
                                        <section class="wrapper2">
                                            <div class="row mb"> 
                                                <div class="content-panel">
                                                    <div class="adv-table">
                                            
                                                        <asp:GridView ID="GridView3" runat="server" AutoGenerateColumns="False"  DataKeyNames="Codigo"
                                                                                GridLines="None" 
                                                                                PageSize="100"
                                                                                AllowPaging="True"
                                                                                CssClass="display table table-bordered"
                                                                                OnRowDataBound="GridView3_RowDataBound"
                                                                                OnRowCommand="GridView3_RowCommand" 
                                                                                OnPageIndexChanging="GridView3_PageIndexChanging" 
                                                                                OnPreRender="GridView3_PreRender">
                                                                <PagerStyle HorizontalAlign = "Right" CssClass="pagination-ys"  />
                                                                <RowStyle  BackColor="#F0F0F0" />
                                                                <alternatingrowstyle  BackColor="#FFFFFF" />
                           
                                                            <Columns>     
                                                                <asp:BoundField DataField="Referencia" HeaderText="NAVE"  Visible ="false" HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>
                                                                <asp:BoundField DataField="Codigo" HeaderText="Codigo" Visible ="false" HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>
                                                                <asp:BoundField DataField="NumeroFactura" HeaderText="#FACTURA" HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>
                                                                <asp:BoundField DataField="TipoFactura" HeaderText="TIPO" HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>
                                                                <asp:BoundField DataField="FechaFactura"  HeaderText="F.FACTURA" SortExpression="CNTR_CONTAINER" HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>
                                                                <asp:BoundField DataField="Monto" HeaderText="MONTO"  HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>
                                                                <asp:BoundField DataField="FacturadoA" HeaderText="FACTURADO A"  HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>
                                                                <asp:BoundField DataField="Mail" HeaderText="MAIL" HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>


                                                                 <asp:TemplateField HeaderText="ENVIADO" ItemStyle-CssClass="center hidden-phone" >
                                                                    <ItemTemplate>
                                                                        <asp:UpdatePanel ID="UPPROFAC" runat="server" ChildrenAsTriggers="true">
                                                                            <ContentTemplate>
                                                                                <asp:CheckBox ID="CHKENVIADO" runat="server" Checked='<%# Bind("Enviado") %>'  CssClass="center hidden-phone" Enabled="false"/>
                                                                            </ContentTemplate>
                                                                            <!--  <Triggers>
                                                                                <asp:AsyncPostBackTrigger ControlID="CHKPRO" />
                                                                            </Triggers>-->
                                                                        </asp:UpdatePanel>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="Seleccionar" ItemStyle-CssClass="center hidden-phone">
                                                                    <ItemTemplate>
                                                                        <a href="../reportesexpo/factura_preview.aspx?id_comprobante=<%#securetext(Eval("Codigo")) %>"  target="_blank"><button type="button" class="btn btn-theme06"><i class="fa fa-print"></i> Imprimir</button></a>
                                                                        <!--<asp:Button runat="server" ID="btnImprimir" Text="Imprimir" CommandName="Imprimir" CommandArgument='<%#Eval("NumeroFactura")%>' class="btn btn-theme08" OnClientClick="BtnVisualizar_Click" OnClick ="BtnVisualizar_Click" />-->
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                            </Columns>
                                                        </asp:GridView>
                                                    </div>
                                                </div><!--content-panel-->
                                            </div><!--row mb-->
                                        </section><!--wrapper2-->


                                        

                                    </ContentTemplate>   
                                </asp:UpdatePanel>   
                            </div>
                            <div class="modal-footer">        
                                <button type="button" class="btn btn-primary" data-dismiss="modal">CERRAR</button>
                            </div>
                        </div>
                    </div>
                </div>--%>
            


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

<%--  <script type="text/javascript" src="../lib/jquery/jquery.min.js"></script>
  <script type="text/javascript" src="../lib/bootstrap/js/bootstrap.min.js"></script>
  <script  type="text/javascript" src="../lib/jquery.dcjqaccordion.2.7.js"></script>
  <script type="text/javascript" src="../lib/jquery.scrollTo.min.js"></script>
  <script type="text/javascript" src="../lib/jquery.nicescroll.js" ></script>
  <script type="text/javascript" src="../lib/jquery.sparkline.js"></script>
  <!--common script for all pages-->
  <script type="text/javascript" src="../lib/common-scripts.js"></script>
  <script type="text/javascript" src="../lib/gritter/js/jquery.gritter.js"></script>
  <script type="text/javascript" src="../lib/gritter-conf.js"></script>--%>
  <!--script for this page--> 
  <script type="text/javascript" src="../lib/pages.js" ></script>
 <%-- <script type="text/javascript" src="../lib/bootstrap-datepicker/js/bootstrap-datepicker.js"></script> 
  <script type="text/javascript" src="../lib/advanced-form-components.js"></script>
  <script type="text/javascript" src="../lib/popup_script_cta.js" ></script>
  <script type="text/javascript" src='../lib/autocompletar/bootstrap3-typeahead.min.js'></script>--%>

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

     <!--SCRIPT PARA MODAL-->
<%--    <script type="text/javascript" src="https://code.jquery.com/jquery-3.5.1.slim.min.js" integrity="sha384-DfXdz2htPH0lsSSs5nCTpuj/zy4C+OGpamoFVy38MVBnE+IbbVYUew+OrCXaRkfj" crossorigin="anonymous"></script>--%>


</asp:Content>
