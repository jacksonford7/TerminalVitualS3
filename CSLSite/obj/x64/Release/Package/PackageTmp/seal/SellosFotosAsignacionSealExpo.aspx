<%@ Page Title="Asignación Sellos Expo" Language="C#" MasterPageFile="~/site.Master" AutoEventWireup="true" CodeBehind="SellosFotosAsignacionSealExpo.aspx.cs" Inherits="CSLSite.SellosFotosAsignacionSealExpo" %>
<%@ MasterType VirtualPath="~/site.Master" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="placehead" runat="server">

    <link href="../img/favicon2.png" rel="icon"/>
    <link href="../img/icono.png" rel="apple-touch-icon"/>
    <link href="../css/bootstrap.min.css" rel="stylesheet"/>
    <link href="../css/dashboard.css" rel="stylesheet"/>
    <link href="../css/icons.css" rel="stylesheet"/>
    <link href="../css/style.css" rel="stylesheet"/>
    <link href="https://fonts.googleapis.com/css2?family=Roboto:wght@300;400;500;700&display=swap" rel="stylesheet"/>
    
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/5.15.1/css/all.min.css" integrity="sha512-+4zCK9k+qNFUR5X+cKL9EIR+ZOhtIloNl9GIKS57V1MyNsYpYcUrUeQc9vNfzsWfV28IaLL3i96P9sdNyeRssA==" crossorigin="anonymous" />
    <link href="../css/datatables.min.css" rel="stylesheet" />
    <link rel="stylesheet" type="text/css" href="../js/buttons/css1.6.4/buttons.dataTables.min.css"/>
    <link href="../css/stc_final.css" rel="stylesheet"/>

    <link href="../shared/estilo/jquery.datetimepicker.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/jquery.datetimepicker.js" type="text/javascript"></script>
  
    

    
<script type="text/javascript">

     

 function BindFunctions()
 {
       $(document).ready(function () {
                 $('.datetimepicker').datetimepicker().datetimepicker({ lang: 'es', timepicker: false, step: 30, format: 'd/m/Y' });
       });    

     


   $(document).ready(function() {    
    $('#tablePagination').DataTable({        
       
         language: {
                "lengthMenu": "_MENU_ REGISTROS POR PAGINA",
                "zeroRecords": "No se encontraron resultados",
                "info": "Mostrando registros del _START_ al _END_ de un total de _TOTAL_ registros",
                "infoEmpty": "Mostrando registros del 0 al 0 de un total de 0 registros",
                "infoFiltered": "(filtrado de un total de _MAX_ registros)",
                "sSearch": "Buscar:",
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
			//{
			//	extend:    'pdf',
			//	text:      '<i class="fa fa-file-pdf-o"></i> ',
			//	titleAttr: 'Exportar a PDF',
			//	className: 'btn btn-primary'
			//},
			{
				extend:    'print',
				text:      '<i class="fa fa-print"></i> ',
				titleAttr: 'Imprimir',
				className: 'btn btn-primary'
			},
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
                <li class="breadcrumb-item" id="opcion_principal" runat="server"><a href="#">SELLOS</a></li>
                <li class="breadcrumb-item active" aria-current="page" id="sub_opcion" runat="server">ASIGNACIÓN DE SELLOS EXPO (CONSOLIDACIÓN) </li>
            </ol>
        </nav>
    </div>

    <div class="dashboard-container p-4" id="cuerpo" runat="server">
        <!--<div class="form-title">
            DATOS DEL USUARIO
        </div>5t
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
              CONSULTA DE SELLOS ASIGNADOS(EXPO CONSOLIDACIÓN) POR NÚMERO DE CONTENEDOR
        </div>

        

        <asp:UpdatePanel ID="UPCAB" runat="server"  UpdateMode="Conditional" ChildrenAsTriggers="true">
            <ContentTemplate>
                <div class="form-row">
         
                    <div class="form-group   col-md-2"> 
                        <label for="inputAddress"> DESDE:<span style="color: #FF0000; font-weight: bold;">*</span></label>

                        <div class="d-flex">
                            <asp:TextBox class="form-control" runat="server" ID="dtpFechadesde" AutoPostBack="false" MaxLength="10"  onkeypress="return soloLetras(event,'0123456789/')" ></asp:TextBox>

                            <asp:CalendarExtender ID="CAGTFECHAFASA"  runat="server"
                                CssClass="cal_Theme1" Format="dd/MM/yyyy" TargetControlID="dtpFechadesde">
                            </asp:CalendarExtender>      
                        </div>
                    </div>

                    <div class="form-group   col-md-2"> 
                        <label for="inputAddress"> HASTA:<span style="color: #FF0000; font-weight: bold;">*</span></label>

                        <div class="d-flex">
                            <asp:TextBox class="form-control" runat="server" ID="txtFechaHasta" AutoPostBack="false" MaxLength="10"  onkeypress="return soloLetras(event,'0123456789/')" ></asp:TextBox>

                            <asp:CalendarExtender ID="CalendarExtender1"  runat="server"
                                CssClass="cal_Theme1" Format="dd/MM/yyyy" TargetControlID="txtFechaHasta">
                            </asp:CalendarExtender>      
                        </div>
                    </div>

                    <div class="form-group col-md-3">
                        <label for="inputZip">CONTENEDOR<span style="color: #FF0000; font-weight: bold;">*</span></label>
                        <asp:TextBox ID="txtContenedor" runat="server" class="form-control"  MaxLength="20"  onkeypress="return soloLetras(event,'abcdefghijklmnopqrstuvwxyz0123456789-_')" placeholder="CONTAINER"></asp:TextBox>
                    </div>


             
                </div>

                <div></div>
                <br />

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
                <script type="text/javascript">
                        Sys.Application.add_load(BindFunctions); 
                </script>
              <%--  <section class="wrapper2">
                
                    <div id="xfinder" runat="server" visible="true" >--%>
                       <%-- <div class="findresult" >
                            <div class="booking" >--%>
             
                                <div class="form-group col-md-12"> 
                                    <div class="form-title">RESULTADOS</div>
                                </div>

                                 <div class="bokindetalle" style=" width:100%; overflow:auto">  
                                      <asp:Repeater ID="tablePagination" runat="server"  onitemcommand="tablePagination_ItemCommand">
                                       <HeaderTemplate>
                                       <table  cellspacing="1" cellpadding="1" class="table table-bordered table-sm table-contecon" id="tablePagination" >
                                            <thead>
                                                <tr>
                                                    <th style="display: none;">ID</th>
                                                    <thstyle="display: none;">GKEY </th>
                                                    <%--<th>PRE GATE</th>
                                                    <th>CHOFER</th>
                                                    <th>PLACA</th>--%>
                                                    <th>CONTENEDOR</th>
                                                    <th>SELLO CGSA</th>
                                                    <th>SELLO 1</th>
                                                    <th>SELLO 2</th>
                                                    <th>SELLO 3</th>
                                                    <th>SELLO 4</th>
                                                    <th>ESTADO</th>
                                                    <th>MENSAJE</th>
                                                    <th>USUARIO</th>
                                                    <th>FECHA/HORA</th>
                                                   <%-- <th>FOTOS</th>--%>
                                                    <th>FOTOS</th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                        </HeaderTemplate>
                                            <ItemTemplate>
                                                <tr class="point"  id="trUsuario">
                                                    <td style="display: none;"><%#Eval("ROW_ID")%></td>
                                                    <td style="display: none;"><%#Eval("GKEY")%></td>
                                                   <%-- <td><%#Eval("PRE_GATE_ID")%></td>--%>
                                                    <%--<td><%#Eval("CHOFER")%></td>--%>
                                                    <%--<td><%#Eval("PLACA")%></td>--%>
                                                    <td><%#Eval("CONTENEDOR")%></td>
                                                    <td><%#Eval("SELLOCGSA")%></td>
                                                    <td><%#Eval("SELLO1")%></td>
                                                    <td><%#Eval("SELLO2")%></td>
                                                    <td><%#Eval("SELLO3")%></td>
                                                    <td><%#Eval("SELLO4")%></td>
                                                    <td><%#Eval("ESTADO")%></td>
                                                    <td><%#Eval("MENSAJE")%></td>
                                                    <td><%#Eval("USUARIO_CREA")%></td>
                                                    <td><%#Eval("DATE")%></td>
                                                                       
                                                    <%--<td class="center hidden-phone">  
                                                        <asp:Button runat="server" 
                                                                    ToolTip="Ver Fotos Pre-Embarque"  
                                                                    ID="btnFotoRecepcion" 
                                                                    Height="30px" 
                                                                    CommandName="FotosRecepcion" 
                                                                    Text="Recepción"  
                                                                    CommandArgument=' <%#Eval("ROW_ID")%>' 
                                                                    class="btn btn-primary"  
                                                                    data-toggle="modal" 
                                                                    data-target="#myModal"  />
                                                    </td> --%>
                                                    <td class="center hidden-phone">  
                                                        <asp:Button runat="server" 
                                                                    ID="btnFotoAsigna"
                                                                    Height="30px" 
                                                                    CommandName="FotosAsignacion" 
                                                                    Text="Foto Sello"  
                                                                    CommandArgument='<%# Eval("ROW_ID") %>' 
                                                                    class="btn btn-primary"  
                                                                    data-toggle="modal" data-target="#myModal"  />
                                                    </td>
                                                          
                                                </tr>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                        </tbody> </table>
                                     </FooterTemplate>
                                    </asp:Repeater>
                                  </div>

                                <!--adv-table-->
                            <%--   </section>--%>
                          <%--  </div><!--content-panel-->
                        </div><!--col-lg-12-->--%>
                         
                   <%-- </div><!--row mt-->--%>
                     <div id="sinresultado" runat="server" class="alert alert-info">
                        No se encontraron resultados, 
                        asegurese que ha escrito correctamente el contenedor a
                        buscar 
                    </div>

                <%--</section><!--wrapper2-->--%>
     
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

            <div class="modal-dialog" role="document" style="max-width: 600px"> <!-- Este tag style controla el ancho del modal -->
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
                                                                    
                                                  <%--  <script type="text/javascript">
                                                        Sys.Application.add_load(BindFunctions);
                                                    </script>--%>
          
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
        
       
        



    </div>

     <script type="text/javascript" src="../js/bootstrap.bundle.min.js"></script>
    <script type="text/javascript"  src="../js/datatables.js"></script>

       <script type="text/javascript" src="../js/buttons/1.6.4/dataTables.buttons.min.js"></script>  
     <script type="text/javascript" src="../js/buttons/1.6.4/jszip.min.js"></script>  
     <script type="text/javascript" src="../js/buttons/1.6.4/pdfmake.min.js"></script>  
    <script type="text/javascript" src="../js/buttons/1.6.4/vfs_fonts.js"></script>  

    <script type="text/javascript" src="../js/buttons/1.6.4/buttons.print.min.js"></script>  
     <script type="text/javascript" src="../js/buttons/1.6.4/buttons.html5.min.js"></script>  
     <script type="text/javascript" src="../js/buttons/1.6.4/buttons.colVis.min.js"></script>  

       <script src="../Scripts/pages.js" type="text/javascript"></script>


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

  <script type="text/javascript">
            $(document).ready(function () {
                 $('.datetimepicker').datetimepicker().datetimepicker({ lang: 'es', timepicker: false, step: 30, format: 'm/d/Y' });
              });    
      </script>
    <asp:updateprogress  id="updateProgress" runat="server">
        <progresstemplate>
            <div id="progressBackgroundFilter"></div>
            <div id="processMessage"> Estamos procesando la tarea que solicitó, por favor  espere...<br />
             <img alt="Loading" src="../shared/imgs/loader.gif" style="margin:0 auto;" />
            </div>
        </progresstemplate>
    </asp:updateprogress>
</asp:Content>



