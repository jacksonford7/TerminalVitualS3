<%@ Page Title="Reporte de PreDescarga" Language="C#" MasterPageFile="~/site.Master" AutoEventWireup="true" CodeBehind="brbkRptPreDescargaFinal.aspx.cs" Inherits="CSLSite.brbk.brbkRptPreDescargaFinal" %>
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
    
       <link href="https://fonts.googleapis.com/css2?family=Roboto:wght@300;400;500;700&display=swap" rel="stylesheet"/>
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/5.15.1/css/all.min.css" integrity="sha512-+4zCK9k+qNFUR5X+cKL9EIR+ZOhtIloNl9GIKS57V1MyNsYpYcUrUeQc9vNfzsWfV28IaLL3i96P9sdNyeRssA==" crossorigin="anonymous" />


  <link href="../css/datatables.min.css" rel="stylesheet" />
  <link rel="stylesheet" type="text/css" href="..js/buttons/css1.6.4/buttons.dataTables.min.css"/>
 <%--   <link href="../lib/advanced-datatable/css/demo_page.css" rel="stylesheet" />
    <link href="../lib/advanced-datatable/css/demo_table.css" rel="stylesheet" />
    <link rel="stylesheet" href="../lib/advanced-datatable/css/DT_bootstrap2.css" />--%>
 
    <%--<script type="text/javascript">

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
    </script>--%>

    
<script type="text/javascript">



 function BindFunctions()
   {
   $(document).ready(function() {    
    $('#grilla').DataTable({        
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
   //             titleAttr: 'Exportar a PDF',
   //             disabled: true,
   //             display: none,
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
                <li class="breadcrumb-item" id="opcion_principal" runat="server"><a href="#">Cargas Break Bulk</a></li>
                <li class="breadcrumb-item active" aria-current="page" id="sub_opcion" runat="server">REPORTE DE PRE-DESCARGA FINAL</li>
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
              FILTROS DEL REPORTE
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


                    <div class="form-group col-md-4">
                        <label for="inputZip">MRN<span style="color: #FF0000; font-weight: bold;">*</span></label>
                        <asp:TextBox ID="TXTMRN" runat="server" class="form-control" disabled MaxLength="16"  onkeypress="return soloLetras(event,'abcdefghijklmnopqrstuvwxyz0123456789-_')" placeholder="MRN"></asp:TextBox>
                    </div>

                   <%-- <div class="form-group col-md-2">
                        <label for="inputZip">MSN<span style="color: #FF0000; font-weight: bold;">*</span></label>
                        <div class="d-flex"> 
                            <asp:TextBox ID="txtFiltroMSN" runat="server" class="form-control"  MaxLength="16"  onkeypress="return soloLetras(event,'abcdefghijklmnopqrstuvwxyz0123456789-_')" placeholder="MRN"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group col-md-2">
                        <label for="inputZip">HSN<span style="color: #FF0000; font-weight: bold;">*</span></label>
                        <div class="d-flex"> 
                            <asp:TextBox ID="txtFiltroHSN" runat="server" class="form-control"  MaxLength="16"  onkeypress="return soloLetras(event,'abcdefghijklmnopqrstuvwxyz0123456789-_')" placeholder="MRN"></asp:TextBox>
                        </div>
                    </div>--%>

                    
             
                </div>

                <div></div>
                <br />

                <div class="row">
                    <div class="col-md-12 d-flex justify-content-center">
                        <asp:Button ID="btnLimpiar" runat="server" class="btn btn-primary"  Text="Limpiar"  OnClick="btnLimpiar_Click"  /><%----%>
                         &nbsp;
                        <asp:Button ID="btnBuscar" runat="server" class="btn btn-primary"  Text="Consultar"  OnClientClick="return mostrarloader('1')"  OnClick="btnconsultar_Click" /><%--OnClick="btnBuscar_Click" --%>
                        <img alt="loading.." src="../lib/file-uploader/img/loading.gif" height="32px" width="32px"  id="ImgCarga" class="nover"   />
                        <span id="imagen"></span>
                       
                    </div>
                </div>

                 <br/>
                <div class="row">
                    <div class="col-md-12 d-flex justify-content-center">
                        <%--<div class="alert alert-warning" id="banmsg" runat="server" clientidmode="Static"><b>Error!</b> >......</div>--%>
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
                                      <asp:Repeater ID="grilla" runat="server"  >
                                       <HeaderTemplate>
                                       <table cellpadding="0" cellspacing="0" border="0" class="table table-bordered table-sm table-contecon" id="grilla" width="100%">
                                           <thead>
                                          <tr>
                                            <th class="th-sm">REFERENCIA</th>
                                            <th class="th-sm">NAVE</th>
                                            <th class="th-sm">CONSIGNATARIO RUC</th>
                                            <th class="th-sm">CONSIGNATARIO</th>
                                            <th class="th-sm">CARGA</th>
                                            <th class="th-sm">BL</th>
                                            <th class="th-sm">CONDICION</th>
                                            <th class="th-sm">MANIOBRA</th>
                                            <th class="th-sm">CANTIDAD</th>
                                            <th class="th-sm">KILOS</th>
                                            <th class="th-sm">TONELAJE</th>
                                            <th class="th-sm">CUBICAJE</th>
                                            <th class="th-sm">TIPO</th>
                                            <th class="th-sm">ITEM</th>
                                            <th class="th-sm">DESCRIPCION</th>
                                            <th class="th-sm">UBICACION</th>
                                            <th class="th-sm">OBSERVACION</th>
                                          </tr>
                                        </thead>
                                        <tbody>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <tr class="gradeC">
                                                <td class="center hidden-phone"> <%#Eval("idNave")%></td>
                                                <td class="center hidden-phone"> <%#Eval("nave")%></td>
                                                <td class="center hidden-phone"> <%#Eval("idConsignatario")%></td>
                                                <td class="center hidden-phone"> <%#Eval("Consignatario")%></td>
                                                <td class="center hidden-phone"> <%#Eval("carga")%></td>
                                                <td class="center hidden-phone"> <%#Eval("bl")%></td>
                                                <td class="center hidden-phone"> <%#Eval("condicion")%></td> 
                                                <td class="center hidden-phone"> <%#Eval("maniobra")%></td> 
                                                <td class="center hidden-phone"> <%#Eval("cantidad")%></td>
                                                <td class="center hidden-phone"> <%#Eval("kilos")%></td>
                                                <td class="center hidden-phone"> <%#Eval("tonelaje")%></td>
                                                <td class="center hidden-phone"> <%#Eval("cubicaje")%></td>
                                                <td class="center hidden-phone"> <%#Eval("TipoProducto")%></td>
                                                <td class="center hidden-phone"> <%#Eval("Item")%></td>
                                                <td class="center hidden-phone"> <%#Eval("productoEcuapass")%></td>
                                                <td class="center hidden-phone"> <%#Eval("Ubicacion")%></td>
                                                <td class="center hidden-phone"> <%#Eval("observacion")%></td>
                                             </tr>    
                                       </ItemTemplate>
                                       <FooterTemplate>
                                        </tbody>
                                      </table>
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
                        asegurese que ha escrito correctamente el MRN y la NAVE
                        buscada 
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


        <asp:UpdatePanel ID="UPMENSAJE" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">  
             <ContentTemplate>

                  <div class="modal fade" id="myModal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
                    <div class="modal-dialog">
                      <div class="modal-content">
                        <div class="modal-header">
                            <h4 class="modal-title" id="myModalLabel">Confirmar Generación</h4>
                            <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                          
                        </div>
                        <div class="modal-body">
                            <br>
                            </br/>
                            Si usted da click en SI, se procederá a generar el IMDT  &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                            <br>
                            </br/>
                        </div>
                        <div class="modal-footer">
                             <asp:Button ID="btnGenerarIMDT" runat="server" class="btn btn-default"  Text="SI"   UseSubmitBehavior="false" data-dismiss="modal" /><%--OnClick="btnGenerarIMDT_Click"--%>
                          <button type="button" class="btn btn-primary" data-dismiss="modal">NO</button>
                        </div>
                      </div>
                    </div>
                  </div>
             </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnGenerarIMDT" />
            </Triggers>
         </asp:UpdatePanel>   

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

    <script type="text/javascript"  src="../js/datatables.js"></script>
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


