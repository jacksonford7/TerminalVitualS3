﻿
<%@ Page Title="Completar Data BL" Language="C#" MasterPageFile="~/site.Master" AutoEventWireup="true" CodeBehind="brbkDatosDescarga.aspx.cs" Inherits="CSLSite.brbk.brbkDatosDescarga" %>
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


    <style type="text/css">
    .cal_Theme1 .ajax__calendar_container   {
    background-color: #DEF1F4;
    border:solid 1px #77D5F7;
    }

    .cal_Theme1 .ajax__calendar_header  {
    background-color: #ffffff;
    margin-bottom: 4px;
    }

    .cal_Theme1 .ajax__calendar_title,
    .cal_Theme1 .ajax__calendar_next,
    .cal_Theme1 .ajax__calendar_prev    {
    color: #004080;
    padding-top: 3px;
    }

    .cal_Theme1 .ajax__calendar_body    {
    background-color: #ffffff;
    border: solid 1px #77D5F7;
    }

    .cal_Theme1 .ajax__calendar_dayname {
    text-align:center;
    font-weight:bold;
    margin-bottom: 4px;
    margin-top: 2px;
    color: #004080;
    }

    .cal_Theme1 .ajax__calendar_day {
    color: #004080;
    text-align:center;
    }

    .cal_Theme1 .ajax__calendar_hover .ajax__calendar_day,
    .cal_Theme1 .ajax__calendar_hover .ajax__calendar_month,
    .cal_Theme1 .ajax__calendar_hover .ajax__calendar_year,
    .cal_Theme1 .ajax__calendar_active  {
    color: #004080;
    font-weight: bold;
    background-color: #DEF1F4;
    }


    .cal_Theme1 .ajax__calendar_today   {
    font-weight:bold;
    }

    .cal_Theme1 .ajax__calendar_other,
    .cal_Theme1 .ajax__calendar_hover .ajax__calendar_today,
    .cal_Theme1 .ajax__calendar_hover .ajax__calendar_title {
    color: #bbbbbb;
    }
    .warning { background-color:Yellow;  color:Red;}

    #progressBackgroundFilter {
        position:fixed;
        bottom:0px;
        right:0px;
        overflow:hidden;
        z-index:1000;
        top: 0;
        left: 0;
        background-color: #CCC;
        opacity: 0.8;
        filter: alpha(opacity=80);
        text-align:center;
    }
    #processMessage 
    {
        text-align:center;
        position:fixed;
        top:30%;
        left:43%;
        z-index:1001;
        border: 5px solid #67CFF5;
        width: 200px;
        height: 100px;
        background-color: White;
        padding:0;
    }
    * input[type=text]
    {
        text-align:left!important;
        }
        
        .autocomplete_completionListElement
        {
            margin: 0px !important;
            background-color: inherit;
            color: windowtext;
            border: buttonshadow;
            border-width: 1px;
            border-style: solid;
            cursor: 'default';
            overflow: auto;
            height: auto;
            text-align: left;
            list-style-type: none;
        }
        
        /* AutoComplete highlighted item */
        .autocomplete_highlightedListItem
        {
            background-color: #ffff99;
            color: black;
            padding: 1px;
        }
        
        /* AutoComplete item */
        .autocomplete_listItem
        {
            background-color: window;
            color: windowtext;
            padding: 1px;
             z-index:2000 !important;
        }
        .AutoExtender
        {
            font-family: Verdana, Helvetica, sans-serif;
            font-size: xx-small;
            font-weight: normal;
            border: solid 1px #006699;
            line-height: 20px;
            padding: 10px;
            background-color: White;
            margin-left: 10px;
            z-index: 200000 !important;
        }
        .AutoExtenderList
        {
            border-bottom: dotted 1px #006699;
            cursor: pointer;
            /*color: Maroon;*/
            color:Black;
            font-style: normal;
            font-size: xx-small;
            font-family:Arial!important;
            z-index: 200000 !important;
        }
        .AutoExtenderHighlight
        {
            color: White;
            background-color: #006699;
            cursor: pointer;
            z-index: 200000 !important;
        }
    </style>
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
                <li class="breadcrumb-item active" aria-current="page" id="sub_opcion" runat="server">DATOS DE DESCARGA</li>
            </ol>
        </nav>
    </div>

    <div class="dashboard-container p-4" id="cuerpo" runat="server">
        <!--<div class="form-title">
            DATOS DEL USUARIO
        </div>
        <div class="form-row" >
            <div class="form-group col-md-6"> 
                <label for="inputAddress">ESTIMADO CLIENTE:<span style="color: #FF0000; font-weight: bold;"></span></label>-->
                <asp:TextBox ID="Txtcliente" Visible="false" runat="server" class="form-control" placeholder=""  Font-Bold="true" disabled ></asp:TextBox>
            <!--</div>-->

            <!--<div class="form-group col-md-2">
                <label for="inputAddress">RUC:<span style="color: #FF0000; font-weight: bold;"></span></label>-->
				<asp:TextBox ID="Txtruc" Visible="false" runat="server" class="form-control" placeholder=""  Font-Bold="true" disabled></asp:TextBox>
            <!--</div>-->

            <!--<div class="form-group col-md-4">
                <label  for="inputAddress">EMPRESA:<span style="color: #FF0000; font-weight: bold;"></span></label>-->
                <asp:TextBox ID="Txtempresa" Visible="false" runat="server" class="form-control" placeholder=""  Font-Bold="true" disabled></asp:TextBox>
            <!--</div>
        </div>-->

        <div class="form-title">
              DATOS DE DESCARGA
        </div>
        <asp:UpdatePanel ID="UPCAB" runat="server"  UpdateMode="Conditional" ChildrenAsTriggers="true">
            <ContentTemplate>
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
                            <asp:TextBox  class="form-control"  ID="txtDescripcionNave" disabled runat="server" MaxLength="200" onkeypress="return soloLetras(event,'abcdefghijklmnopqrstuvwxyz01234567890/')"></asp:TextBox>
                            &nbsp;
                            
                            <a class="btn btn-outline-primary mr-4" style='font-size:24px' target="popup" onclick="window.open('../catalogo/naves.aspx','name','width=900,height=880')">
                                    <span class='fa fa-search' style='font-size:24px'></span> </a>
                        </div>
                    </div>


                    <div class="form-group col-md-2">
                        <label for="inputZip">MRN<span style="color: #FF0000; font-weight: bold;">*</span></label>
                        <asp:TextBox ID="TXTMRN" runat="server" class="form-control" disabled MaxLength="16"  onkeypress="return soloLetras(event,'abcdefghijklmnopqrstuvwxyz0123456789-_')" placeholder="MRN"></asp:TextBox>
                    </div>

                    <div class="form-group   col-md-2"> 
                        <label for="inputAddress"> ETA:<span style="color: #FF0000; font-weight: bold;"></span></label>

                        <div class="d-flex">
                            <asp:TextBox class="form-control" runat="server" ID="fecETA" disabled   AutoPostBack="false" MaxLength="10"  onkeypress="return soloLetras(event,'0123456789/')" ></asp:TextBox>

                            <asp:CalendarExtender ID="CAGTFECHAFASA"  runat="server"
                                CssClass="cal_Theme1" Format="dd/MM/yyyy" TargetControlID="fecETA">
                            </asp:CalendarExtender>      
                        </div>
                    </div>

               

                    <div class="form-group col-md-2"> 
                        <!--<label for="inputAddress">Estado :<span style="color: #FF0000; font-weight: bold;"></span></label>-->
                        <div class="d-flex">
                            <asp:DropDownList Visible ="false" ID="cmbEstado" class="form-control" runat="server" Font-Size="Medium" disabled  Font-Bold="true" >
                            </asp:DropDownList>
                            <a class="tooltip" ><span class="classic" >Estados generales</span><img alt='' src='../shared/imgs/info.gif' class='datainfo'/></a>
                        </div>
                    </div>
             
                </div>

                <div></div>

                <div class="row">
                    <div class="col-md-12 d-flex justify-content-center">
                        <asp:Button ID="btnLimpiar" runat="server" class="btn btn-primary"  Text="Limpiar" OnClick="btnLimpiar_Click"   />
                         &nbsp;
                        <asp:Button ID="btnBuscar" runat="server" class="btn btn-primary"  Text="Buscar"  OnClientClick="return mostrarloader('1')" OnClick="btnBuscar_Click"  />
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
        
                <section class="wrapper2">
                
                    <div id="xfinder" runat="server" visible="true" >
                        <div class="findresult" >
                            <div class="booking" >
             

                                <div class="form-group col-md-12"> 
                                    <div class="form-title">DETALLE DE BL</div>
                                </div>

                                <div class="form-row">
                                    <%--<div class="form-group col-md-2">
                                        <label for="inputZip">MRN<span style="color: #FF0000; font-weight: bold;">*</span></label>
                                        <div class="d-flex"> 
                                            <asp:TextBox ID="txtFiltroMRN" runat="server" class="form-control"  MaxLength="16"  onkeypress="return soloLetras(event,'abcdefghijklmnopqrstuvwxyz0123456789-_')" placeholder="MRN"></asp:TextBox>
                                        </div>
                                    </div>--%>
                                    <div class="form-group col-md-2">
                                        <label for="inputZip">MSN<span style="color: #FF0000; font-weight: bold;">*</span></label>
                                        <div class="d-flex"> 
                                            <asp:TextBox ID="txtFiltroMSN" runat="server" class="form-control"  MaxLength="16"  onkeypress="return soloLetras(event,'abcdefghijklmnopqrstuvwxyz0123456789-_')" placeholder="MRN"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="form-group col-md-2">
                                        <label for="inputZip">HSN<span style="color: #FF0000; font-weight: bold;">*</span></label>
                                        <div class="d-flex"> 
                                            <asp:TextBox ID="txtFiltroHSN" runat="server" class="form-control"  MaxLength="16"  onkeypress="return soloLetras(event,'abcdefghijklmnopqrstuvwxyz0123456789-_')" placeholder="MRN"></asp:TextBox>
                                            &nbsp;
                                            <asp:Button ID="btnFiltar" runat="server" class="btn btn-primary"  Text="Filtar"  OnClick="btnFiltar_Click"  />
                                        </div>
                                    </div>

                                     <div class="form-group col-md-3">
                                        <span class="help-block">&nbsp;</span>

                                        <label for="inputZip">ESTADOS</label>
                                        <div class="d-flex"> 
                                            <asp:DropDownList  ID="cmbFiltroEstados" class="form-control"  runat="server" Font-Size="Medium" Font-Bold="true" AutoPostBack="true" OnSelectedIndexChanged="cmbFiltroEstados_SelectedIdexChange" >
                                            </asp:DropDownList>
                                        </div>
		                            </div>
                                </div>

                                <div class="bokindetalle" style=" width:100%; overflow:auto">

                                    <asp:GridView ID="tablePagination" runat="server" AutoGenerateColumns="False"  
                                        DataKeyNames="idTarjaDet"
                                        GridLines="None" 
                                        OnPageIndexChanging="tablePagination_PageIndexChanging" 
                                        OnPreRender="tablePagination_PreRender"     
                                        OnRowCommand="tablePagination_RowCommand"   
                                        OnRowDataBound="tablePagination_RowDataBound"
                                        PageSize="100"
                                        AllowPaging="True"
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
                                            <asp:BoundField DataField="idTarjaDet" HeaderText="ID" Visible="false"  HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>
                                            <asp:BoundField DataField="idTarja" HeaderText="CODIGO" Visible="false"  HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>
                                            <asp:BoundField DataField="bl" HeaderText="BL" HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>
                                            <asp:BoundField DataField="mrn" HeaderText="MRN" HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>
                                            <asp:BoundField DataField="msn" HeaderText="MSN" SortExpression="msn" HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>                                                                
                                            <asp:BoundField DataField="hsn" HeaderText="HSN" SortExpression="hsn" HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>                                                                                                       
                                            <asp:BoundField DataField="idConsignatario" HeaderText="RUC CONSIGNATARIO"  SortExpression="idConsignatario" HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>                                                                
                                            <asp:BoundField DataField="Consignatario" HeaderText="CONSIGNATARIO"  SortExpression="Consignatario" HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>                                                                
                                            <asp:BoundField DataField="estado" HeaderText="ESTADO"  Visible="false" HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>                                                                
                                    
                                            <asp:TemplateField HeaderText="VERIFICADO" ItemStyle-CssClass="center hidden-phone" >
                                                <ItemTemplate>
                                                    <asp:UpdatePanel ID="UPPRO" runat="server" ChildrenAsTriggers="true">
                                                        <ContentTemplate>
                                                            <asp:CheckBox ID="CHKPRO" runat="server" Checked="false" CssClass="center hidden-phone" Enabled="false"/>
                                                        </ContentTemplate>
                                                        <%-- <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="CHKPRO" />
                                                        </Triggers>--%>
                                                    </asp:UpdatePanel>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="" ItemStyle-CssClass="center hidden-phone">
                                                <ItemTemplate>
                                                    <asp:Button runat="server" ID="btnEditar" Height="30px" CommandName="Editar" Text="Editar"  CommandArgument='<%# Bind("idTarjaDet") %>' class="btn btn-primary"  data-toggle="modal" data-target="#myModal3"  />                                                                       
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                        </Columns>
                                    </asp:GridView>


                                </div><!--adv-table-->
                            <%--   </section>--%>
                            </div><!--content-panel-->
                        </div><!--col-lg-12-->

                         
                    </div><!--row mt-->
                     <div id="sinresultado" runat="server" class="alert alert-info">
                        No se encontraron resultados, 
                        asegurese que ha escrito correctamente el MRN y la NAVE
                        buscada 
                    </div>

                </section><!--wrapper2-->
     
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnBuscar" />
            </Triggers>
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
                            <asp:Button ID="btnGrabar" runat="server" class="btn btn-primary" Text="GENERAR TRANSACCIÓN" OnClientClick="return mostrarloader('1')" OnClick="btnGrabar_Click"   />
                        </div>
                    </div><!--btn-group-justified-->
                </div><!--showback-->
            </ContentTemplate>
        </asp:UpdatePanel> 


        <div id="myModal3" class="modal fade" tabindex="-1" role="dialog">

            <div class="modal-dialog" role="document" style="max-width: 1000px"> <!-- Este tag style controla el ancho del modal -->
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title">DATOS DEL BL</h5>
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
                                                    <label for="inputAddress">BL<span style="color: #FF0000; font-weight: bold;"></span></label>
                                                    <div class="d-flex">
                                                        <asp:TextBox ID="txtBL"  runat="server" class="form-control"   placeholder="" Font-Bold="false" ></asp:TextBox>
                                                    </div>
                                                </div>

                                                <div class="form-group col-md-9">
                                                    <label for="inputAddress">No Carga :<span style="color: #FF0000; font-weight: bold;"></span></label>
                                                    <div class="d-flex">
                                                        <asp:TextBox ID="txtmrnDet"  runat="server" class="form-control"   placeholder="MRN" Font-Bold="false" disabled></asp:TextBox>
                                                        &nbsp;
                                                        <asp:TextBox ID="txtmsnDet"  runat="server" class="form-control"   placeholder="MSN" Font-Bold="false" disabled></asp:TextBox>
                                                        &nbsp;
                                                        <asp:TextBox ID="txthsnDet"  runat="server" class="form-control"   placeholder="HSN" Font-Bold="false" disabled></asp:TextBox>
                                                    </div>
                                                </div>

                                               <%-- <div class="form-group col-md-3">
                                                    <label for="inputZip">Ruc Consignatario<span style="color: #FF0000; font-weight: bold;">*</span></label>
                                                    <asp:TextBox ID="txtRucConsignatario" runat="server" class="form-control" disabled MaxLength="16"  onkeypress="return soloLetras(event,'abcdefghijklmnopqrstuvwxyz0123456789-_')" placeholder="RUC"></asp:TextBox>
                                                </div>

                                                <div class="form-group col-md-9">
                                                    <label for="inputZip">Consignatario</label>
                                                    <asp:TextBox ID="txtConsignatario" runat="server" class="form-control" disabled MaxLength="16"  onkeypress="return soloLetras(event,'abcdefghijklmnopqrstuvwxyz0123456789-_')" placeholder="CONSIGNATARIO"></asp:TextBox>
                                                </div>--%>

                                                <div class="form-group   col-md-3"> 
                                                    <label for="inputAddress">Ruc Cliente:<span style="color: #FF0000; font-weight: bold;"></span></label>
                                                    <div class="d-flex">
                                                        <asp:TextBox ID="txtRucConsignatario" runat="server" MaxLength="30" 
                                                            placeholder="Ruc"
                                                            class="form-control"
                                                            style="text-transform:uppercase;"
                            
                                                            ClientIDMode="Static" >
                                                        </asp:TextBox>
                                                         &nbsp;
                                                        <img alt="loading.." src="../shared/imgs/loader.gif" id="loader3" class="nover"  />
                                                        <asp:Button ID="btnBuscarCliente" runat="server"  CssClass="btn btn-primary"
                                                            Text="Buscar" OnClientClick="return prepareObjectRuc()" 
                                                            ToolTip="Busca al Cliente por el RUC." OnClick="btnBuscarCliente_Click"/>
                        
                                                    </div>
                                                </div>

                                             <div class="form-group   col-md-9"> 
                                                    <label for="inputAddress">Cliente:<span style="color: #FF0000; font-weight: bold;"></span></label>

                                                    <div class="d-flex">
                                                        <asp:TextBox ID="txtConsignatario" runat="server" MaxLength="100" 
                                                            placeholder="Cliente"
                                                            disabled
                                                            class="form-control"
                                                            style="text-transform:uppercase;"
                                                            ClientIDMode="Static">
                                                        </asp:TextBox>
                        
                                                    </div>
                                                </div>

                                                 <div class="form-group col-md-3"> 
                                                    <label  for="inputAddress">  <span style="color: #FF0000; font-weight: bold;"></span></label>
                                                    <div class="d-flex">
                                                        <asp:DropDownList visible="false" ID="cmbImo" class="form-control" runat="server" Font-Size="Medium"   Font-Bold="true" ></asp:DropDownList>
                                                    </div>
                                                </div>
                                                

                                                <div class="form-group col-md-12">
                                                    <label for="inputZip">Producto Declarado<span style="color: #FF0000; font-weight: bold;">*</span></label>
                                                    <asp:TextBox TextMode="MultiLine" Rows="3" ID="txtProductoEcuapass" runat="server" class="form-control" MaxLength="300"  onkeypress="return soloLetras(event,'abcdefghijklmnopqrstuvwxyz0123456789-_')" placeholder="PRODUCTO DECLARADO"></asp:TextBox>
                                                </div>

                                                <div class="form-group col-md-3"> 
                                                    <label for="inputAddress">Producto :<span style="color: #FF0000; font-weight: bold;"></span></label>
                                                    <div class="d-flex">
                                                        <asp:DropDownList ID="cmbProducto" class="form-control" runat="server" Font-Size="Medium" AutoPostBack="true" Font-Bold="true" OnSelectedIndexChanged="cmbProducto_SelectedIdexChange" >
                                                        </asp:DropDownList>
                                                        <a class="tooltip" ><span class="classic" >Producto que se espera</span><img alt='' src='../shared/imgs/info.gif' class='datainfo'/></a>
                                                    </div>
                                                </div>

                                                <div class="form-group col-md-3"> 
                                                    <label for="inputAddress">Maniobra :<span style="color: #FF0000; font-weight: bold;"></span></label>
                                                    <div class="d-flex">
                                                        <asp:DropDownList ID="cmbManiobra" class="form-control" runat="server" Font-Size="Medium" disabled Font-Bold="true" ></asp:DropDownList>
                                                    </div>
                                                </div>

                                                <div class="form-group col-md-3"> 
                                                    <label for="inputAddress">Item :<span style="color: #FF0000; font-weight: bold;"></span></label>
                                                    <div class="d-flex">
                                                        <asp:DropDownList ID="cmbItem" class="form-control" runat="server" Font-Size="Medium" disabled  Font-Bold="true" ></asp:DropDownList>
                                                    </div>
                                                </div>

                                                <div class="form-group col-md-3"> 
                                                    <label for="inputAddress">Condición :<span style="color: #FF0000; font-weight: bold;"></span></label>
                                                    <div class="d-flex">
                                                        <asp:DropDownList ID="cmbCondicion" class="form-control" runat="server" Font-Size="Medium"   Font-Bold="true" ></asp:DropDownList>
                                                    </div>
                                                </div>

                                                <div class="form-group   col-md-3"> 
                                                    <label for="inputAddress">Cantidad<span style="color: #FF0000; font-weight: bold;"></span></label>
                                                    <asp:TextBox ID="txtCantidad" runat="server" MaxLength="9" class="form-control"
                                                        onkeypress="return soloLetras(event,'1234567890',true)" 
                                                        placeholder="CANTIDAD"
                                                        ></asp:TextBox>
                                                </div>

                                                <div class="form-group   col-md-3"> 
                                                    <label for="inputAddress">Peso(Kilos)<span style="color: #FF0000; font-weight: bold;"></span></label>
                                                    <asp:TextBox ID="txtKilos" runat="server" MaxLength="9" class="form-control"
                                                        onkeypress="return soloLetras(event,'1234567890',true)" 
                                                        placeholder="KL"
                                                        ></asp:TextBox>
                                                </div>

                                                <div class="form-group   col-md-3"> 
                                                    <label for="inputAddress">Cubicaje<span style="color: #FF0000; font-weight: bold;"></span></label>
                                                    <asp:TextBox ID="txtCubicaje" runat="server" MaxLength="9" class="form-control"
                                                        onkeypress="return soloLetras(event,'1234567890.',true)" 
                                                        placeholder="CUBICAJE"
                                                        ></asp:TextBox>
                                                </div>

                                                <div class="form-group   col-md-3"> 
                                                    <%--<label for="inputAddress">Tonelaje<span style="color: #FF0000; font-weight: bold;"></span></label>--%>
                                                    <asp:TextBox Visible="false" ID="txtTonelaje" runat="server" MaxLength="9" class="form-control"
                                                        onkeypress="return soloLetras(event,'1234567890.',true)" 
                                                        placeholder="TONELAJE"
                                                        ></asp:TextBox>
                                                </div>

                                                <div class="form-group col-md-12"> 
                                                    <label for="inputAddress">Descripción</label>
                                                    <div class="d-flex"> 
                                                        <asp:TextBox TextMode="MultiLine"  class="form-control"  ID="txtDescripcion" runat="server" MaxLength="200" onkeypress="return soloLetras(event,'abcdefghijklmnopqrstuvwxyz01234567890/')"></asp:TextBox>
                                                    </div>
                                                </div>

                                                <div class="form-group col-md-12"> 
                                                    <label for="inputAddress">Contenido<span style="color: #FF0000; font-weight: bold;"></span></label>
                                                    <div class="d-flex"> 
                                                        <asp:TextBox TextMode="MultiLine" class="form-control"  ID="txtContenido" runat="server" MaxLength="100" onkeypress="return soloLetras(event,'abcdefghijklmnopqrstuvwxyz01234567890/')"></asp:TextBox>
                                                    </div>
                                                </div>
             
                                                <div class="form-group col-md-12"> 
                                                    <label for="inputAddress">Observación<span style="color: #FF0000; font-weight: bold;"></span></label>
                                                    <div class="d-flex"> 
                                                        <asp:TextBox TextMode="MultiLine"  class="form-control"  ID="txtobservacion" runat="server" MaxLength="100" onkeypress="return soloLetras(event,'abcdefghijklmnopqrstuvwxyz01234567890/')"></asp:TextBox>
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


    </div>

    <script type="text/javascript" src="../lib/datatables/datatables.min.js"></script>
    <script type="text/javascript" src="../lib/advanced-datatable/js/DT_bootstrap.js"></script>
    <script type="text/javascript" src="../lib/common-scripts.js"></script>
    <script type="text/javascript" src="../lib/pages.js" ></script>
    <script type="text/javascript" src="../lib/advanced-form-components.js"></script>

    <script src="../Scripts/pages.js" type="text/javascript"></script>
    <script src="../Scripts/credenciales.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.tablesorter.min.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.tablesorter.pager.js" type="text/javascript"></script>


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

    function popupCallback(catalogo) {
            if (catalogo == null || catalogo == undefined) {
                alert('Hubo un problema al setaar un objeto de catalogo');
                return;
            }

        this.document.getElementById('<%= txtNave.ClientID %>').value = catalogo.codigo;
        this.document.getElementById('<%= txtDescripcionNave.ClientID %>').value = catalogo.descripcion;

      
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
            $(document).ready(function () {
                 $('.datetimepicker').datetimepicker().datetimepicker({ lang: 'es', timepicker: false, step: 30, format: 'm/d/Y' });
              });    
      </script>

</asp:Content>