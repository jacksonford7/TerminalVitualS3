<%@ Page  Language="C#" MasterPageFile="~/site.Master" AutoEventWireup="true" Title="Consola de Solicitudes"
         CodeBehind="consultasolicitud.aspx.cs" Inherits="CSLSite.consultasolicitud" %>
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
<%--    <link href="https://fonts.googleapis.com/css2?family=Roboto:wght@300;400;500;700&display=swap" rel="stylesheet"/>

    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/5.15.1/css/all.min.css" integrity="sha512-+4zCK9k+qNFUR5X+cKL9EIR+ZOhtIloNl9GIKS57V1MyNsYpYcUrUeQc9vNfzsWfV28IaLL3i96P9sdNyeRssA==" crossorigin="anonymous" />--%>
    <link href="../css/datatables.min.css" rel="stylesheet" />
    <link rel="stylesheet" type="text/css" href="..js/buttons/css1.6.4/buttons.dataTables.min.css"/>

    <link href="../shared/estilo/jquery.datetimepicker.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/jquery.datetimepicker.js" type="text/javascript"></script>


    <link href="../shared/estilo/modal.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/work.css" rel="stylesheet" type="text/css" />

     <!--formatos de tabla y controles de tabla con pagineo css-->
    <link href="../lib/advanced-datatable/css/demo_page.css" rel="stylesheet" />
    <link href="../lib/advanced-datatable/css/demo_table.css" rel="stylesheet" />
    <link rel="stylesheet" href="../lib/advanced-datatable/css/DT_bootstrap.css" />
   
       <!--external css-->
    <link href="../lib/font-awesome/css/font-awesome.css" rel="stylesheet" />
   

    <script type="text/javascript">

         function BindFunctions()
         {
               $(document).ready(function () {
                         $('.datetimepicker').datetimepicker().datetimepicker({ lang: 'es', timepicker: false, step: 30, format: 'd/m/Y' });
               });    

              $(document).ready(function () {
                        // Add minus icon for collapse element which is open by default
                        $(".collapse.show").each(function () {
                            $(this).prev(".card-header").find(".fa").addClass("fa-chevron-down").removeClass("fa-chevron-right");
                        });

                        // Toggle plus minus icon on show hide of collapse element
                        $(".collapse").on('show.bs.collapse', function () {
                            $(this).prev(".card-header").find(".fa").removeClass("fa-chevron-right").addClass("fa-chevron-down");
                        }).on('hide.bs.collapse', function () {
                            $(this).prev(".card-header").find(".fa").removeClass("fa-chevron-down").addClass("fa-chevron-right");
                        });
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
			                {
				                extend:    'pdf',
				                text:      '<i class="fa fa-file-pdf-o"></i> ',
				                titleAttr: 'Exportar a PDF',
				                className: 'btn btn-primary'
			                },
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
        .warning { background-color:Yellow;  color:Red;}
        .panel-reveal-modal-bg { background: #000; background: rgba(0,0,0,.8);cursor:progress;	}
    </style>

     <style type="text/css">
        body2
        {
            font-family: Arial;
            font-size: 10pt;
        }
        .modal
        {
            position: fixed;
            z-index: 999;
            height: 100%;
            width: 100%;
            top: 0;
            background-color: Black;
            filter: alpha(opacity=60);
            opacity: 0.6;
            -moz-opacity: 0.8;
        }

        .modalBackground
        {
            background-color: Black;
            filter: alpha(opacity=60);
            opacity: 0.6;
        }
        .modalPopup
        {
            background-color: #FFFFFF;
            width: 500px;
            border: 3px solid #FF3720;
            padding: 0;
        }
        .modalPopup .header
        {
            background-color: #2FBDF1;
            height: 30px;
            color: White;
            line-height: 30px;
            text-align: center;
            font-weight: bold;
        }
        .modalPopup .body
        {
            min-height: 50px;
            line-height: 30px;
            text-align: center;
            font-weight: bold;
            margin-bottom: 5px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="placebody" runat="server">

    <noscript><meta http-equiv="refresh" content="0; url=sinsoporte.htm" /> </noscript>
   <input id="zonaid" type="hidden" value="7" />
    <asp:ToolkitScriptManager ID="tkscata" runat="server"></asp:ToolkitScriptManager>
     <asp:HiddenField ID="manualHide" runat="server" />
        <div class="mt-4">         
        <nav class="mt-4" aria-label="breadcrumb">
          <ol class="breadcrumb">
            <li class="breadcrumb-item" id="opcion_principal" runat="server"><a href="#">MCA - Módulo de Control de Acceso</a></li>
            <li class="breadcrumb-item active" aria-current="page" id="sub_opcion" runat="server">Consola de Solicitudes</li>
          </ol>
        </nav>
      </div>

    <div class="dashboard-container p-4" id="cuerpo" runat="server">
         <div class="form-title">
            Criterios de consulta:
        </div>
        <div class="form-row">
      
        <div class="form-group col-md-3">
                 <label for="inputAddress">Número de Solicitud:<span style="color: #FF0000; font-weight: bold;"> </span></label>
                           <asp:TextBox ID="txtsolicitud" runat="server" class="form-control" MaxLength="11"
                     style="text-align: center" onblur="cajaControl(this);"
                     onkeypress="return soloLetras(event,'01234567890',true)"></asp:TextBox>
             
            </div>

            <div class="form-group col-md-3">
                 <label for="inputAddress">RUC/CI/PAS:<span style="color: #FF0000; font-weight: bold;"> </span></label>
                           <asp:TextBox ID="txtruccipas" runat="server" class="form-control" MaxLength="25"
             style="text-align: center" 
             onkeypress="return soloLetras(event,'01234567890abcdefghijklnmñopqrstuvwxyz',true)"></asp:TextBox>
            </div>
          
            <div class="form-group col-md-6">
                 <label for="inputAddress">Tipo de Solicitud:<span style="color: #FF0000; font-weight: bold;"> </span></label>
                          <asp:DropDownList ID="dptiposolicitud" runat="server" class="form-control" >
                         <asp:ListItem Value="0">* Seleccione origen *</asp:ListItem>
                  </asp:DropDownList>
            </div>

          <div class="form-group col-md-3" style="display:none">
                <label for="inputPassword4"> </label>
                 <label class="checkbox-container">
                   <asp:CheckBox Text="Asociación de Transportistas" ID="chkAsoTrans" Checked="false"  runat="server" />
                     <span class="checkmark"></span>
                 </label>
            </div>

            <div class="form-group col-md-6">
                 <label for="inputAddress">Generados desde / hasta:<span style="color: #FF0000; font-weight: bold;"> </span></label>
                  <div class="d-flex">
                 <asp:TextBox ID="tfechaini" runat="server" MaxLength="10" CssClass="datetimepicker form-control"
                 onkeypress="return soloLetras(event,'01234567890/')" style="text-align: center" 
                      ClientIDMode="Static"></asp:TextBox> 
                      &nbsp;&nbsp;
                      <asp:TextBox ID="tfechafin" runat="server" ClientIDMode="Static"  MaxLength="15" CssClass="datetimepicker form-control"
                    onkeypress="return soloLetras(event,'01234567890/')"  style="text-align: center" 
                   ></asp:TextBox>
                  </div>
            </div>

             <div class="form-group col-md-6 d-flex" style="border:outset ; height:70px;overflow:auto" >
                <label for="inputAddress">Estado:<span style="color: #FF0000; font-weight: bold;"> &nbsp;&nbsp;</span></label>
                <asp:CheckBoxList runat="server" ID="cblestados" Width="600px"   onchange="fValidaTodos(this)">
                    <asp:ListItem Value="0">* Seleccione origen *</asp:ListItem>
                </asp:CheckBoxList>
              </div>
        </div>
        <br />
        <div class="row">
            <div class="col-md-12 d-flex justify-content-center">
                   <asp:Button ID="btbuscar" runat="server" Text="Iniciar la búsqueda" OnClientClick="return getGif();" class="btn btn-primary" 
               onclick="btbuscar_Click"/>
            </div>
       </div>


           <div class="cataresult">
       
               <script type="text/javascript">Sys.Application.add_load(BindFunctions); </script>

              <%--  <asp:UpdatePanel ID="upresult" runat="server" ChildrenAsTriggers="true">
                <ContentTemplate>--%>
                <input id="idlin" type="hidden" runat="server" clientidmode="Static" />
                <input id="diponible" type="hidden" runat="server" clientidmode="Static" />
                <div class="msg-alerta" id="alerta" runat="server" style=" display:none" ></div>

                <asp:UpdatePanel ID="uptabla" runat="server" ChildrenAsTriggers="true" >
                    <ContentTemplate>

                        <div id="xfinder" runat="server" visible="false" >
                            <div class="findresult" >
                            <%--<div class="booking" >--%>
                                <div class="informativo" style=" height:100%;">
                                            <%--<div class="separator">Solicitudes disponibles:</div>--%>
                                            <div class="bokindetalle" style="width:100%;overflow:auto" >
                                            <asp:Repeater ID="tablePagination" runat="server"  >
                                            <HeaderTemplate>
                                            <table   cellspacing="1"  border="solid" cellpadding="1" class="table table-bordered table-sm table-contecon" id="tablePagination">
                                            <thead>
                                            <tr>
                                            <th># Solicitud</th>
                                            <th class="nover">Solicitado por</th>
                                            <th style=" display:none">Ruc</th>
                                            <th style=" display:none">Tipo</th>
                                            <th>Empresa</th>
                                            <th>Tipo de Empresa</th>
                                            <th>Tipo de Solicitud</th>
                                            <th>Usuario Solicitante</th>
                                            <th>Usuario Atiende</th>
                                            <th>Fecha Registro</th>
                                            <th>Estado</th>
                                                <th>N° Liquidación</th>
                                                <th>Valor</th>
                                                <th>Notas</th>
                                            <th></th>
                                            </tr>
                                            </thead> 
                                            <tbody>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                            <tr class="point" >
                                            <td title="# Solicitud"><%#Eval("NUMSOLICITUD")%></td>
                                            <td class="nover" title="Solicitado por"><%#Eval("ASO_TRANSPORTISTA")%></td>
                                            <td style=" display:none"><asp:Label ID="lruccipas"  style="text-transform :uppercase" runat="server" Text='<%# Eval("RUCCIPAS") %>'></asp:Label></td>
                                            <td style=" display:none"><%#Eval("TIPO")%></td>
                                            <td title="Empresa"><asp:Label ID="lempresa"  style="text-transform :uppercase" runat="server" Text='<%# Eval("EMPRESA") %>'></asp:Label></td>
                                            <td title="Tipo de Empresa"><%#Eval("TIPOEMPRESA")%></td>
                                            <td title="Tipo de Solicitud"><%#Eval("TIPOSOLICITUD")%></td>
                                            <td title="Usuario Solicita" style=" text-transform :uppercase"><%#Eval("USUARIOING")%></td>
                                            <td title="Usuario Atiende" style="text-transform :uppercase"><%#Eval("USUARIOMOD")%></td>
                                            <td title="Fecha Registro" ><%#Eval("FECHAING")%></td>
                                            <td title="Estado"><%#Eval("ESTADO")%></td>
                                                <td title="N° Liquidación"><%#Eval("NUMERO_FACTURA")%></td>
                                                <td title="Valor"><%#Eval("MONTO_FACTURA")%></td>
                                                <td title="Valor"><%#Eval("NOTA")%></td>
                                            <td title="Ver detalle de la Solicitud" >
                                            <a id="adjDoc" class="btn btn-outline-primary mr-4" onclick="redirectsol('<%#Eval("NUMSOLICITUD")%>', '<%#Eval("TIPO")%>', '<%#Eval("TIPOSOLICITUD")%>', '<%#Eval("RUCCIPAS")%>', '<%#Eval("EMPRESA")%>',  '<%#Eval("ESTADO")%>')">
                                            <i class="fa fa-search" ></i> Ver 
                                            </a>
                   
                                            </td>
                                            </tr>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                            </tbody>
                                            </table>
                                            </FooterTemplate>
                                    </asp:Repeater>
                                        </div>
                                                <%--   <div id="pager">
                                        Registros por página
                                                <select class="pagesize">
                                            <option selected="selected" value="10">10</option>
                                            <option  value="20">20</option>
                                            <option value="30">30</option>
                                            <option value="40">40</option>
                                            <option value="50">50</option>
                                            </select>
                                            <img alt="" src="../shared/imgs/first.gif" class="first"/>
                                            <img alt="" src="../shared/imgs/prev.gif" class="prev"/>
                                            <input  type="text" class="pagedisplay" size="5px"/>
                                            <img alt="" src="../shared/imgs/next.gif" class="next"/>
                                            <img alt="" src="../shared/imgs/last.gif" class="last"/>
                                    </div>--%>

     
                                </div>
                            </div>
                        </div>
                        <div class="form-row">
	                        <div class="form-group col-md-12"> 
                                <div id="sinresultado" runat="server" class=" alert alert-info"></div>
	                        </div>
	                    </div>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger  ControlID="btbuscar"/>
                       <%-- <asp:AsyncPostBackTrigger ControlID="btcancel" />--%>
 
             
                     </Triggers>
                </asp:UpdatePanel>
            </div>
    </div>

      



    <asp:updateprogress associatedupdatepanelid="uptabla"
        id="updateProgress" runat="server">
        <progresstemplate>
            <div id="progressBackgroundFilter"></div>
            <div id="processMessage"> Estamos procesando la tarea que solicitó, por favor 
                espere...<br />
                 <img alt="Loading" src="../shared/imgs/loader.gif" style="margin:0 auto;" />
            </div>
        </progresstemplate>
    </asp:updateprogress>
    
    <asp:ModalPopupExtender ID="mpeLoading" runat="server" BehaviorID="idmpeLoading" PopupControlID="pnlLoading" TargetControlID="btnLoading" EnableViewState="false" DropShadow="true" BackgroundCssClass="modalBackground" />

    <asp:Panel ID="pnlLoading" runat="server"  HorizontalAlign="Center" CssClass="modalPopup" align="center"  EnableViewState="false" Style="display: none">
        <br />Procesando información....
        <div class="body2">
            <asp:UpdatePanel ID="msgload" runat="server" UpdateMode="Conditional"  >
                <ContentTemplate>
                    <div align="center">   
                        <asp:Image ID="loading" runat="server" ImageUrl="../lib/file-uploader/img/loading.gif"  Visible="true" Width="40" Height="40" />
                    </div>
                  
                    <br/>
                        Estimado Cliente, se está procesando su solicitud....<br/>Por favor espere.... 
                    <br />
                    <br />
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </asp:Panel>
    <asp:Button ID="btnLoading" runat="server" Style="display: none" />
    
<script type="text/javascript">

var mpeLoading;
function initializeRequest(sender, args){
    mpeLoading = $find('idmpeLoading');
    mpeLoading.show();
    mpeLoading._backgroundElement.style.zIndex += 10;
    mpeLoading._foregroundElement.style.zIndex += 10;
}
    function endRequest(sender, args) {
         $find('idmpeLoading').hide();

    }

Sys.WebForms.PageRequestManager.getInstance().add_initializeRequest(initializeRequest);
Sys.WebForms.PageRequestManager.getInstance().add_endRequest(endRequest); 

if (typeof(Sys) !== 'undefined') Sys.Application.notifyScriptLoaded();
    $find('idmpeLoading').hide();

</script>

 <script src="../Scripts/credenciales.js" type="text/javascript"></script>
    
    <script type="text/javascript">
       $(document).ready(function () {
            $('.datetimepicker').datetimepicker().datetimepicker({ lang: 'es', timepicker: false, closeOnDateSelect: true, format: 'd/m/Y' });
        });
        function popupCallback(objeto, catalogo) {
            if (objeto == null || objeto == undefined) {
                alert('Hubo un problema al setaar un objeto de catalogo');
                return;
            }
            //si catalogos es booking
            if (catalogo == 'bk') {
                document.getElementById('numbook').textContent = objeto.nbr;
                document.getElementById('nbrboo').value = objeto.nbr;
                return;
            }

            //si catalogos es booking
            if (catalogo == 'cc') {
                document.getElementById('txtfecha').textContent = objeto.fecha;
                document.getElementById('xfecha').value = objeto.fecha;
                return;
            }

        }

        function clear() {
            document.getElementById('numbook').textContent = '...';
            document.getElementById('nbrboo').value = '';
        }
        function clear2() {
            document.getElementById('txtfecha').textContent = '...';
            document.getElementById('xfecha').value = '';
        }

        var programacion = {};
        var lista = [];
       
         

      
        function redirectsol(val, tipo, tiposol, rucempresa, razonsocial, estado) {

            var caja = val;
            //window.open('../credenciales/documentos/?placa=' + caja, 'name', 'width=850,height=480')
            if (tipo == 'EMPRESA') {
                
                window.open('../credenciales/revisasolicitudempresa.aspx?numsolicitud=' + caja)
            }
            if (tipo == 'COLABORADOR' && estado == 'CONFIRMACIÓN DE PAGO') {
                //            if ((tipo == 'COLABORADOR' && tiposolicitud == 'CREDENCIAL' && estado == 'FACTURADA') || tipo == "PERMANENTE" || tipo == "TEMPORAL") {
                //                window.open('../credenciales/revision-solicitud-colaborador/?numsolicitud=' + caja)
                window.open('../credenciales/consultacomprobantedepagocolaborador.aspx?sid1=' + caja)
            }
            else if (tipo == 'COLABORADOR') {
                //                window.open('../credenciales/revision-solicitud-colaborador/?numsolicitud=' + caja)
                if (tiposol == 'RENUEVA PERMISO CREDENCIAL OPC') {
                    window.open('../credenciales/revisasolicitudpermisoOPC.aspx?numsolicitud=' + caja)
                }
                else {
                    if (tiposol == 'PERMISO PROVISIONAL') {
                        window.open('../credenciales/revisasolicitudpermisoprovisional.aspx?numsolicitud=' + caja)
                    }
                    else {
                        window.open('../credenciales/revisasolicitudcolaborador.aspx?numsolicitud=' + caja + '&ruc=' + rucempresa + '&razonsocial=' + razonsocial)
                    }
                }
            }
            if (tipo == 'VEHICULO' && estado == 'CONFIRMACIÓN DE PAGO') {
                window.open('../credenciales/consultacomprobantedepagovehiculos.aspx/?sid1=' + caja)
            }
            else if (tipo == 'VEHICULO') {
                //                window.open('../credenciales/revision-solicitud-vehiculo/?numsolicitud=' + caja)
                window.open('../credenciales/revisasolicitudvehiculo.aspx?numsolicitud=' + caja + '&ruc=' + rucempresa + '&razonsocial=' + razonsocial)
            }
            if (tipo == 'PERMISO VEHICULAR PERMANENTE') {
                //                window.open('../credenciales/revision-solicitud-vehiculo/?numsolicitud=' + caja)
                window.open('../credenciales/revisasolicitudpermisodeaccesovehiculo.aspx?numsolicitud=' + caja + '&ruc=' + rucempresa + '&razonsocial=' + razonsocial)
            }
            if (tipo == "PERMANENTE" || tipo == "TEMPORAL") {
                window.open('../credenciales/revisasolicitudpermisodeacceso.aspx?numsolicitud=' + caja + '&ruc=' + rucempresa)
            }
        }
        function fValidaTodos(valor) {
        }
        function getGif() {
            //document.getElementById('imagen').innerHTML = '<img alt="" src="../shared/imgs/loader.gif"/>';
            return true;
        }
        function getGifOculta() {
            //document.getElementById('imagen').innerHTML = '<img alt="" src=""/>';
            return true;
        }
    </script>


    <!--referecnia para que cargue paginacion y botones de control table para descarga-->
     <script type="text/javascript" src="../js/bootstrap.bundle.min.js"></script>
    <script type="text/javascript"  src="../js/datatables.js"></script>

    <script type="text/javascript" src="../js/buttons/1.6.4/dataTables.buttons.min.js"></script>  
     <script type="text/javascript" src="../js/buttons/1.6.4/jszip.min.js"></script>  
     <script type="text/javascript" src="../js/buttons/1.6.4/pdfmake.min.js"></script>  
    <script type="text/javascript" src="../js/buttons/1.6.4/vfs_fonts.js"></script>  

    <script type="text/javascript" src="../js/buttons/1.6.4/buttons.print.min.js"></script>  
     <script type="text/javascript" src="../js/buttons/1.6.4/buttons.html5.min.js"></script>  
     <script type="text/javascript" src="../js/buttons/1.6.4/buttons.colVis.min.js"></script>  

<%-- 

   <asp:updateprogress  id="updateProgress" runat="server">
        <progresstemplate>
            <div id="progressBackgroundFilter"></div>
            <div id="processMessage"> Estamos procesando la tarea que solicitó, por favor  espere...<br />
             <img alt="Loading" src="../shared/imgs/loader.gif" style="margin:0 auto;" />
            </div>
        </progresstemplate>
    </asp:updateprogress>--%>
</asp:Content>

