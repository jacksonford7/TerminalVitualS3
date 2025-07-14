<%@ Page Title="Asignación Sellos" Language="C#" MasterPageFile="~/site.Master" AutoEventWireup="true" CodeBehind="mtyAsignacionSellosFotos.aspx.cs" Inherits="CSLSite.mtyAsignacionSellosFotos " %>
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

     <script type="text/javascript">

            function BindFunctions() {

                 $(document).ready(function () {
                // Add minus icon for collapse element which is open by default
                $(".collapse.show").each(function () {
                    $(this).prev(".card-header").find(".fa").addClass("fa-minus").removeClass("fa-plus");
                });

                // Toggle plus minus icon on show hide of collapse element
                $(".collapse").on('show.bs.collapse', function () {
                    $(this).prev(".card-header").find(".fa").removeClass("fa-plus").addClass("fa-minus");
                }).on('hide.bs.collapse', function () {
                    $(this).prev(".card-header").find(".fa").removeClass("fa-minus").addClass("fa-plus");
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

    <style type="text/css">
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
 #aprint {
 	     color: #666;    
	     border: 1px solid #ccc;    
	     -moz-border-radius: 3px;    
	     -webkit-border-radius: 3px;    
	     background-color: #f6f6f6;    
	     padding: 0.3125em 1em;    
	     cursor: pointer;   
	     white-space: nowrap;   
	     overflow: visible;   
	     font-size: 1em;    
	     outline: 0 none /* removes focus outline in IE */;    
	     margin: 0px;    
	     line-height: 1.6em;    
	     background-image: url(../shared/imgs/action_print.gif);
	     background-repeat: no-repeat;
	     background-position:left center;
	     text-decoration:none;
	     padding:5px 2px 5px 30px;
	  
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
                <li class="breadcrumb-item" id="opcion_principal" runat="server"><a href="#">Importación</a></li>
                <li class="breadcrumb-item active" aria-current="page" id="sub_opcion" runat="server">SELLOS ASIGNADOS </li>
            </ol>
        </nav>
    </div>

    <div class="dashboard-container p-4" id="cuerpo" runat="server">
        <%--<div class="form-title">
            DATOS DEL USUARIO
        </div>--%>

        <div class="form-title">
              CONSULTA DE SELLOS POR NÚMERO DE CONTENEDOR
        </div>
        
        <asp:TextBox visible="false" ID="Txtcliente" runat="server" class="form-control" placeholder=""  Font-Bold="true" disabled ></asp:TextBox>
        <asp:TextBox visible="false" ID="Txtruc" runat="server" class="form-control" placeholder=""  Font-Bold="true" disabled></asp:TextBox>
        <asp:TextBox visible="false" ID="Txtempresa" runat="server" class="form-control" placeholder=""  Font-Bold="true" disabled></asp:TextBox>

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

                   <%-- <div class="form-group col-md-4"> 
                        <label for="inputAddress">Nombre de Nave<span style="color: #FF0000; font-weight: bold;">*</span><span style="color: #FF0000; font-weight: bold;"></span></label>
                           <div class="d-flex">
                     
                             <div class="d-flex">
                                 <label class="checkbox-container">
                                       <input runat="server" clientidmode="Static" enableviewstate="true" id="vacio" name="fk" type="radio" value="MTY" checked="true" class="xradio" />&nbsp;Exportación
                                        <span class="checkmark"></span>
                                </label><label for="inputEmail4">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</label>
                           
                             </div> 
		                </div>
                    </div>--%>

                    

                     

         
                   <%-- <div class="form-group   col-md-2"> 
                        <label for="inputAddress"> DESDE:<span style="color: #FF0000; font-weight: bold;"></span></label>

                        <div class="d-flex">
                            <asp:TextBox class="form-control" runat="server" ID="dtpFechadesde" AutoPostBack="false" MaxLength="10"  onkeypress="return soloLetras(event,'0123456789/')" ></asp:TextBox>

                            <asp:CalendarExtender ID="CAGTFECHAFASA"  runat="server"
                                CssClass="cal_Theme1" Format="dd/MM/yyyy" TargetControlID="dtpFechadesde">
                            </asp:CalendarExtender>      
                        </div>
                    </div>

                    <div class="form-group   col-md-2"> 
                        <label for="inputAddress"> HASTA:<span style="color: #FF0000; font-weight: bold;"></span></label>

                        <div class="d-flex">
                            <asp:TextBox class="form-control" runat="server" ID="txtFechaHasta" AutoPostBack="false" MaxLength="10"  onkeypress="return soloLetras(event,'0123456789/')" ></asp:TextBox>

                            <asp:CalendarExtender ID="CalendarExtender1"  runat="server"
                                CssClass="cal_Theme1" Format="dd/MM/yyyy" TargetControlID="txtFechaHasta">
                            </asp:CalendarExtender>      
                        </div>
                    </div>--%>

                    <div class="form-group col-md-3">
                        <label for="inputZip">CONTENEDOR<span style="color: #FF0000; font-weight: bold;">*</span></label>
                        <asp:TextBox ID="txtContenedor" runat="server" class="form-control"  MaxLength="20"  onkeypress="return soloLetras(event,'abcdefghijklmnopqrstuvwxyz0123456789-_')" placeholder="CONTAINER"></asp:TextBox>
                    </div>

                  <%--  <div class="form-group col-md-3"> 
                        <label for="inputAddress">Divergencia<span style="color: #FF0000; font-weight: bold;">*</span><span style="color: #FF0000; font-weight: bold;"></span></label>
                           <div class="d-flex">
                     
                             <div class="d-flex">
                                 <label class="checkbox-container">
                                       <input runat="server" clientidmode="Static"  id="vacio" name="fk" type="radio" value="MTY" checked="true" class="xradio" />&nbsp;Diferencias
                                        <span class="checkmark"></span>
                                </label><label for="inputEmail4">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</label>
                           
                             </div> 
		                </div>
                    </div>--%>

                    <div class="form-group col-md-2">
                        <span class="help-block">&nbsp;</span>
                          
                            <asp:UpdatePanel ID="UPCHK" runat="server"  UpdateMode="Conditional" ChildrenAsTriggers="true">
                                <ContentTemplate>
                                    &nbsp;&nbsp;<asp:CheckBox ID="chkDiferentes" Checked="true" runat="server" class="list-child" Text=" &nbsp;Diferencia"  />
                                </ContentTemplate> 
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="chkDiferentes" />
                                </Triggers>
                            </asp:UpdatePanel> 
                                  
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
                                    <div class="form-title">DETALLE DE SELLOS</div>
                                </div>

                                <div class="bokindetalle" style=" width:100%; overflow:auto">
                                             

                                    <asp:GridView ID="tablePagination" runat="server" AutoGenerateColumns="False"  
                                        DataKeyNames="id"
                                        GridLines="None" 
                                        OnPageIndexChanging="tablePagination_PageIndexChanging" 
                                        OnPreRender="tablePagination_PreRender"     
                                        OnRowCommand ="tablePagination_RowCommand"   
                                        OnRowDataBound = "tablePagination_RowDataBound"
                                        PageSize="8"
                                        AllowPaging="false"
                                        CssClass="table table-bordered invoice">
                                        <PagerStyle HorizontalAlign = "Right" CssClass="table table-bordered "  />
                                        <RowStyle  BackColor="#F0F0F0" />
                                        <alternatingrowstyle  BackColor="#FFFFFF" />
                           
                                        <Columns>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <%# Container.DataItemIndex + 1 %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="id" HeaderText="ID" Visible="false"  HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>
                                            <asp:BoundField DataField="referencia" HeaderText="REFERENCIA"  HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>
                                            <asp:BoundField DataField="container" HeaderText="CONTENEDOR" ItemStyle-Font-Size="14px"  HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>
                                            <asp:BoundField DataField="sello_CGSA" HeaderText="SELLO CGSA"  ItemStyle-Font-Size="14px" HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>
                                            <asp:BoundField DataField="sello1" HeaderText="SELLO1" ItemStyle-Font-Size="14px" HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>                                                                
                                            <asp:BoundField DataField="sello2" HeaderText="SELLO2" ItemStyle-Font-Size="14px" HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>                                                                
                                            <asp:BoundField DataField="sello3" HeaderText="SELLO3" ItemStyle-Font-Size="14px" HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>                                                                
                                            <asp:BoundField DataField="sello4" HeaderText="SELLO4" ItemStyle-Font-Size="14px" HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>                                                                
                                            <asp:BoundField DataField="diferencia" HeaderText="DIFERENCIA"  Visible="false" HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>
                                            <asp:BoundField DataField="revisado" HeaderText="VERIFICADO"  Visible="false" HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>
                                            <asp:BoundField DataField="mensaje" HeaderText="RESULTADO" ItemStyle-Font-Size="14px"  HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>
                                            <asp:BoundField DataField="usuarioCrea" HeaderText="USUARIO" ItemStyle-Font-Size="14px"  HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>
                                            <asp:BoundField DataField="fechaCreacion" HeaderText="FECHA/HORA" ItemStyle-Font-Size="14px"  HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>

                                               <asp:TemplateField HeaderText="DIFERENCIA" ItemStyle-CssClass="center hidden-phone" >
                                                <ItemTemplate>
                                                    <asp:UpdatePanel ID="UPDIF" runat="server" ChildrenAsTriggers="true">
                                                        <ContentTemplate>
                                                            <asp:CheckBox ID="CHKDIF" runat="server" Checked="false" CssClass="center hidden-phone" Enabled="false"/>
                                                        </ContentTemplate>
                                                        <%-- <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="CHKPRO" />
                                                        </Triggers>--%>
                                                    </asp:UpdatePanel>
                                                </ItemTemplate>
                                            </asp:TemplateField>

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
                                                    <asp:Button runat="server" ID="btnFotoRecepcion" Height="30px" CommandName="Foto" Text="Fotos"  CommandArgument='<%# Bind("id") %>' class="btn btn-primary"  data-toggle="modal" data-target="#myModal"  />                                                                                                                           
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                           <asp:TemplateField HeaderText="" ItemStyle-CssClass="center hidden-phone">
                                                <ItemTemplate>
                                                    <asp:Button runat="server" ID="btnEditar" Height="30px" CommandName="Editar" Text="Editar"  CommandArgument='<%# Bind("id") %>' class="btn btn-primary"  data-toggle="modal" data-target="#myModal2"  />                                                                                                                           
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
                        asegurese que ha escrito correctamente el contenedor o la nave
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
                
            </ContentTemplate>
        </asp:UpdatePanel> 

        <div id="myModal" class="modal fade" tabindex="-1" role="dialog">

            <div class="modal-dialog" role="document" style="max-width: 820px"> <!-- Este tag style controla el ancho del modal -->
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title">FOTOS DE SELLOS</h5>
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                        </button>
                    </div>
                    <div class="modal-body">

                        <asp:UpdatePanel ID="UPMODAL" runat="server" UpdateMode="Conditional" >  
                            <ContentTemplate>
                              
                                <section class="wrapper2">
                                    <div class="row mb"> 
                                        <div class="content-panel">
                                            <div class="adv-table">
                                                <div class="bokindetalle" style="width:100%; overflow:auto">    
                                                                    
                                                    <script type="text/javascript">
                                                        Sys.Application.add_load(BindFunctions);
                                                    </script>
          
                                                    <div id="xfinde2" runat="server" visible="false" >

                                                        <!-- page start-->
                                                        <div class="chat-room mt">
                                                            <aside class="mid-side">
                                                                                
                                                                <div class="catawrap" >
                                                                    <div class="room-desk" id="htmlImagenes" runat="server">
                                                                    </div>
                                                                </div>
                                                            </aside>
                                                                <br />
                                                        </div>
                                                        <!-- page end-->
                                                    </div>

                                                    <div id="sinresultadoFotos" runat="server" class=" alert  alert-warning" visible ="false" >
                                                        No se encontraron resultados, 
                                                        asegurese que ha exista fotos de esta transacción
                                                    </div>
                                            </div>
                                        </div><!--content-panel-->
                                    </div><!--row mb-->
                                </section>

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

            <div class="modal-dialog" role="document" style="max-width: 500px"> <!-- Este tag style controla el ancho del modal -->
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title">EDICIÓN DE DATOS DE SELLOS</h5>
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                        </button>
                    </div>
                    <div class="modal-body">

                        <asp:UpdatePanel ID="UPEDIT" runat="server" UpdateMode="Conditional" >  
                                    <ContentTemplate>
                                        <asp:Panel ID="Panel2" runat="server">         
                                                <div id="div_Codigos" style="visibility:hidden;">
                                                    <asp:HiddenField ID="hdf_CodigoDet" runat="server" />
                                                </div>

                                            <div class="form-row">

                                                <asp:TextBox ID="txtIdSelloMuelle" Visible="false"  runat="server" class="form-control"   placeholder="MRN" Font-Bold="false" disabled></asp:TextBox>

                                                <div class="form-group col-md-12">
                                                    <label for="inputAddress">Numero Contenedor<span style="color: #FF0000; font-weight: bold;"></span></label>
                                                    <div class="d-flex">
                                                        <asp:TextBox ID="txtContenedorEdit"  disabled runat="server" class="form-control"   placeholder="Containers" Font-Bold="false" ></asp:TextBox>
                                                    </div>
                                                </div>
                                           

                                                 <div class="form-group   col-md-12"> 
                                                        <label for="inputAddress">Sello 1:<span style="color: #FF0000; font-weight: bold;">*</span></label>

                                                        <div class="d-flex">
                                                            <asp:TextBox ID="txtSello1" runat="server" MaxLength="100" 
                                                                placeholder="Seal 1"
                                                                class="form-control"
                                                                style="text-transform:uppercase;"
                                                                ClientIDMode="Static">
                                                            </asp:TextBox>
                        
                                                        </div>
                                                    </div>

                                                <div class="form-group   col-md-12"> 
                                                        <label for="inputAddress">Sello 2:<span style="color: #FF0000; font-weight: bold;"></span></label>

                                                        <div class="d-flex">
                                                            <asp:TextBox ID="txtSello2" runat="server" MaxLength="100" 
                                                                placeholder="Seal 2"
                                                                class="form-control"
                                                                style="text-transform:uppercase;"
                                                                ClientIDMode="Static">
                                                            </asp:TextBox>
                        
                                                        </div>
                                                    </div>

                                                <div class="form-group   col-md-12"> 
                                                        <label for="inputAddress">Sello 3:<span style="color: #FF0000; font-weight: bold;"></span></label>

                                                        <div class="d-flex">
                                                            <asp:TextBox ID="txtSello3" runat="server" MaxLength="100" 
                                                                placeholder="Seal 3"
                                                                class="form-control"
                                                                style="text-transform:uppercase;"
                                                                ClientIDMode="Static">
                                                            </asp:TextBox>
                        
                                                        </div>
                                                    </div>

                                                <div class="form-group   col-md-12"> 
                                                        <label for="inputAddress">Sello 4:<span style="color: #FF0000; font-weight: bold;"></span></label>

                                                        <div class="d-flex">
                                                            <asp:TextBox ID="txtSello4" runat="server" MaxLength="100" 
                                                                placeholder="Seal 4"
                                                                class="form-control"
                                                                style="text-transform:uppercase;"
                                                                ClientIDMode="Static">
                                                            </asp:TextBox>
                        
                                                        </div>
                                                    </div>

                                               
                                            </div>
                                            
                                        </asp:Panel>
                      
                                        <section class="wrapper2">
                                            <%--<div class="row mb"> --%>
                                                <div class="content-panel">
                                                  

                                                    <div class="row">
                                                        <div class="col-md-12 d-flex justify-content-center">
                                                            <asp:Button ID="btnActualizar" runat="server" class="btn btn-primary"  Text="ACTUALIZAR" OnClick="btnActualizar_Click"  OnClientClick="return mostrarloader('1')"   />
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

          function prepareObjectRuc() {
               <%-- try {
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
                }--%>
        }

        function popupCallback(catalogo) {
                if (catalogo == null || catalogo == undefined) {
                    alert('Hubo un problema al setaar un objeto de catalogo');
                    return;
                }

            this.document.getElementById('<%= txtNave.ClientID %>').value = catalogo.codigo;
            this.document.getElementById('<%= txtDescripcionNave.ClientID %>').value = catalogo.descripcion;
            }

    </script>

    <script type="text/javascript">
        $(document).ready(function () {
                $('.datetimepicker').datetimepicker().datetimepicker({ lang: 'es', timepicker: false, step: 30, format: 'm/d/Y' });
            });    
    </script>

<%--    <asp:updateprogress  id="updateProgress" runat="server">
        <progresstemplate>
            <div id="progressBackgroundFilter"></div>
            <div id="processMessage"> Estamos procesando la tarea que solicitó, por favor  espere...<br />
             <img alt="Loading" src="../shared/imgs/loader.gif" style="margin:0 auto;" />
            </div>
        </progresstemplate>
    </asp:updateprogress>--%>
</asp:Content>
