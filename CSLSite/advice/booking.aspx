<%@ Page Title="BOOKING exportacion, consolidacion" Language="C#" MasterPageFile="~/site.Master" AutoEventWireup="true" CodeBehind="booking.aspx.cs" Inherits="CSLSite.aisv.booking" %>
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

      <!--Datatables-->
       <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/5.15.1/css/all.min.css" integrity="sha512-+4zCK9k+qNFUR5X+cKL9EIR+ZOhtIloNl9GIKS57V1MyNsYpYcUrUeQc9vNfzsWfV28IaLL3i96P9sdNyeRssA==" crossorigin="anonymous" />
       <link href="../css/datatables.min.css" rel="stylesheet" />
       <link rel="stylesheet" type="text/css" href="..js/buttons/css1.6.4/buttons.dataTables.min.css"/>
        <script src="../lib/jquery/jquery.min.js" type="text/javascript"></script>
       <script type="text/javascript"  src="../js/datatables.js"></script>
       <script src="../Scripts/table_catalog.js" type="text/javascript"></script>  
       <script type="text/javascript" src="../js/bootstrap.bundle.min.js"></script>
    <!--Fin-->

     <script type="text/javascript">

        function BindFunctions()
        {
            $(document).ready(function ()
            {
                $('.datetimepicker').datetimepicker().datetimepicker({ lang: 'es', timepicker: false, step: 30, format: 'm/d/Y' });

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
<asp:Content ID="Content2" ContentPlaceHolderID="placebody" runat="server">
    <asp:ToolkitScriptManager ID="tkscata" runat="server"></asp:ToolkitScriptManager>

     <div id="div_BrowserWindowName" style="visibility:hidden;">
        <asp:HiddenField ID="hf_BrowserWindowName" runat="server" />
    </div>

    <input id="zonaid" type="hidden" value="105" />
    <input id="bandera"     type="hidden"   runat="server" clientidmode="Static"  />
    <input id="procesar"    type="hidden"   runat="server" clientidmode="Static"  />
    <input id="itemT4"      type="hidden"   runat="server" clientidmode="Static"  />
    <input id="linea_validar"     type="hidden"   runat="server" clientidmode="Static"  />
    
    <div class="mt-4">         
        <nav class="mt-4" aria-label="breadcrumb">
            <ol class="breadcrumb">
            <li class="breadcrumb-item" id="opcion_principal" runat="server"><a href="#">Mis Naves</a></li>
            <li class="breadcrumb-item active" aria-current="page" id="sub_opcion" runat="server">BOOKING MANUAL (COPARN MASIVO)</li>
            </ol>
        </nav>
    </div>

    <!-- White Container -->
    <div class="dashboard-container p-4" id="cuerpo" runat="server">
        <div class="form-title">Datos para procesar</div>

        <div class="form-row">

            <div class="form-group col-md-3"> 
                <label for="inputAddress">Referencia :<span style="color: #FF0000; font-weight: bold;"></span></label>
                <div class="d-flex">
                    <asp:DropDownList ID="cmbNave" class="form-control"  runat="server" Font-Size="Medium" AutoPostBack="false" Font-Bold="true" >
                    </asp:DropDownList>
                    <a class="tooltip" ><span class="classic" >Nombre de nave</span><img alt='' src='../shared/imgs/info.gif' class='datainfo'/></a>
                </div>
            </div>
     
            <div class="form-group col-md-9">
                    <label for="inputAddress">Archivo CSV:<span style="color: #FF0000; font-weight: bold;"> *</span></label>
                    <div class="d-flex">
                        <input class="uploader form-control " id="fsupload" title="Escoja el archivo CSV (Excel separado por comas)" accept="csv" type="file"  runat="server" clientidmode="Static" />
                        &nbsp;&nbsp;&nbsp;
                        <asp:Button ID="btnCargar" runat="server" Text="Cargar" onclick="btnCargar_Click" 
                            ToolTip="Carga el archivo y valida la información" class="btn  btn-primary"/>
                        <span id="valdae" class="validacion"></span>
                    </div>
            </div> 
        </div>

        <div class="form-row">
            <div class="form-group col-md-12">
                <label for="inputAddress">Mail notificación adicional<span style="color: #FF0000; font-weight: bold;"></span></label>
                <input type='text' id='txtEmail' name='txtEmail'  runat="server" class="form-control "
                enableviewstate="false" clientidmode="Static"
                onkeypress="return soloLetras(event,'abcdefghijklmnñopqrstuvwxyz1234567890_$@.',true)"  
                onblur="maildata(this,'valmailz');" maxlength="50"/>
                <span id="valmailz" class="validacion"></span>
            </div> 
        </div>
  
        <div class="form-title">Resultados de la verificación del archivo </div>
        <div class="form-row" id="xfinder" runat="server" visible="true">
            <div id="unit_ok" class="form-group col-md-4" runat="server" clientidmode="Static" >
            </div>
            <div id="unit_error" class="form-group col-md-8" runat="server" clientidmode="Static">
            </div>
        </div>

        <div class="nover">
            <div class="col-md-12 d-flex justify-content-center"> 
	            <input clientidmode="Static" id="dataexport" onclick="getfile('resultado');" type="button" value="Exportar" runat="server" class="btn btn-secondary"/>
            </div> 
        </div>


        <div id="sinresultado" runat="server" class="alert alert-danger">
            No se encontraron resultados.
        </div>

        <asp:UpdatePanel ID="UPDETALLE" runat="server"  UpdateMode="Conditional"  >  
            <ContentTemplate>
                <script type="text/javascript">
                    Sys.Application.add_load(BindFunctions); 
                </script>
        
                <section class="wrapper2">
                
                    <div id="xfinderResult" runat="server" visible="true" >
                        <div class="findresult" >
                            <div class="booking" >                                
                                      
                                <div class="bokindetalle" style=" width:100%; overflow:auto">

                                    <asp:Repeater ID="tablePagination" runat="server" OnItemDataBound="tablePagination_ItemDataBound">
                                        <HeaderTemplate>
                                        <table cellpadding="0" cellspacing="0" border="0" class="table table-bordered table-sm table-contecon" id="tablePagination" width="100%">
                                            <thead style="background: #B4B4B4; color: white">
                                                <tr>
                                                    <th class="th-sm"></th>
                                                    <th class="th-sm">NUMERO</th>
                                                    <th class="th-sm">VISITA</th>
                                                    <th class="th-sm">ESTADO</th>
                                                    <th class="th-sm">RESULTADO</th>
                                                    <th class="th-sm">SECUENCIA</th>
                                                </tr>
                                            </thead>
                                        <tbody>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <tr id="row">
                                                <td class="center hidden-phone"><asp:Label Text='<%# Container.ItemIndex + 1 %>' ID="lblCount" runat="server"  /></td>
                                                <td class="center hidden-phone"><asp:Label Text='<%#Eval("numero")%>' ID="lblNumero" runat="server"  /></td>
                                                <td class="center hidden-phone"><asp:Label Text='<%#Eval("visita")%>' ID="lblVisita" runat="server"  /></td>
                                                <td class="nover"><asp:Label Text='<%#Eval("estado")%>' ID="lblEstado" runat="server"  /></td>
                                                <td class="center hidden-phone"> <asp:CheckBox ID="chkEstado" runat="server" CssClass="center hidden-phone" Enabled="false"/></td>
                                                <td class="center hidden-phone"><asp:Label Text='<%#Eval("resultado")%>' ID="lblResultado" runat="server"  /></td>            
                                                <td class="center hidden-phone"><asp:Label Text='<%#Eval("secuencia")%>' ID="lblSecuencia" runat="server"  /></td>             
                                                
                                            </tr>    
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            
                                        </tbody>
                                            <tr>
                                                <td colspan="3">TOTAL ITEMS SIN ERRORES:</td>
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
                    <div id="sinResult" runat="server" class="alert alert-info">
                        No se encontraron resultados.
                    </div>

                </section><!--wrapper2-->
     
            </ContentTemplate>
            <Triggers>
                
            </Triggers>
        </asp:UpdatePanel>

        <div class="form-row">
            <div class="col-md-12 d-flex justify-content-center">
            <asp:Button ID="btnProceasr" runat="server" Text="Generar Booking"  onclick="btnProceasr_Click" ToolTip="Confirma la información y genera los booking"  class="btn  btn-primary"/>
            </div>
        </div>
    
    </div>

    <script src="../Scripts/pages.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            BindFunctions();

            var t = document.getElementById('bandera');
            if (t.value != undefined && t.value != null && t.value.trim().length > 0) {
                $('span.validacion').text('');
            }
            
        });

        function getprocesa() {
            var i = document.getElementById('procesar');
            if (i == undefined || i == null) {
                alertify.alert('Informativo','No se encontró el control asociado!!').set('label', 'Aceptar');
                return false;
            }
            i = i.value;
            if (i == '0') {
                alertify.alert('Informativo','Por favor realice todos los pasos antes de proceder:\n\t 1.Booking \n\t2.Operación\n\t3.Archivo Csv').set('label', 'Aceptar');
                return false;
            }
            if (confirm('Está seguro de generar los preavisos para las unidades en la lista?')) {
                      alertify.alert('Informativo','En algunos minutos recibirá un mail confirmando la lista de unidades preavisadas y si existe algún contenedor con error').set('label', 'Aceptar');
                return true;
            }
            return false;
        }
        function onBook() {
            tipo = 'MTY';
            if (document.getElementById('consolida').checked) {
                tipo = 'LCL';
            }
          var w =  window.open('../catalogo/bookingmas.aspx?tipo=' + tipo + '&v=1', 'Bookings', 'width=850,height=880');
          w.focus();
        }
        function validateBook(objeto) {
            //stringnifiobjeto
            var bokIt = {
                number    :objeto.numero,
                linea     :objeto.bline,
                referencia:objeto.referencia,
                gkey      :objeto.gkey,
                pod       :objeto.pod,
                pod1      :objeto.pod1,
                shiperID  :objeto.shipid,
                temp      :objeto.temp,
                fkind     :objeto.fk,
                imo       :objeto.imo,
                refer     :objeto.refer,
                dispone   :objeto.dispone,
                iso       :objeto.aqt,
                cutOff    :objeto.cutoff,
                temp      :objeto.temp,
                hume      :objeto.hume,
                vent_pc   :objeto.vent_pc,
                ventu     :objeto.ventu,
                gkey      :objeto.gkey
            };
            document.getElementById('refnumber').textContent = objeto.numero + '/' + objeto.referencia;
            document.getElementById('itemT4').value = JSON.stringify(bokIt);
            document.getElementById('bandera').value = objeto.fk;
            document.getElementById('linea_validar').value = objeto.bline;
            alertify.alert('Informativo','Se le comunica que la disponibilidad máxima unidades del item de booking elegido es: ' + objeto.dispone).set('label', 'Aceptar');
        }
     $('form').live("submit", function () { ShowProgress();});
    </script>
     <div class="loading" align="center">
        Estamos verificando toda la información 
        que nos facilitó,por favor espere unos segundos<br />
        <img src="../shared/imgs/loader.gif" alt="x" />
    </div>
</asp:Content>