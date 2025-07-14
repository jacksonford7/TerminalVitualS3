<%@ Page  Language="C#" MasterPageFile="~/site.Master"  AutoEventWireup="true"  Title="Consulta de Movimientos de Turnos Zal" CodeBehind="reporte_zal.aspx.cs" Inherits="CSLSite.zal.reporte_zal" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>


<%--<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">--%>
<asp:Content ID="Content1" ContentPlaceHolderID="placehead" runat="server">
    <title>Consulta de Movimientos de Turnos Zal</title>
    <link href="../shared/estilo/base-site.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/jquery.datetimepicker.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/jquery-1.7.1.min.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.datetimepicker.js" type="text/javascript"></script>
     <link href="../Inicio/css/font-awesome.min.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/aisv-general.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/work.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/tables.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/catalogoestadosolicitud.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/info.css" rel="stylesheet" type="text/css" />
    <link href="../Inicio/css/font-awesome.min.css" rel="stylesheet" type="text/css" />
    <link href="../Inicio/base-site.css" rel="stylesheet" type="text/css" />
    <link rel="../shortcut icon" type="image/x-icon" href="../favicon.ico" />
    <link href="../shared/estilo/aisv-general.css" rel="stylesheet" type="text/css" />

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

    <style type="text/css">
     * input[type=text]
        {
            text-align:left!important;
        }
        .style1
        {
            border-bottom: 1px solid #CCC;
            width: 530px;
        }
    </style>
    <script type="text/javascript">
        function BindFunctions() {
            document.getElementById('imagen').innerHTML = '';
            $(document).ready(function () {
                $('#tablasort').tablesorter({ cancelSelection: true, cssAsc: "ascendente", cssDesc: "descendente", headers: { 5: { sorter: false }, 6: { sorter: false}} }).tablesorterPager({ container: $('#pager'), positionFixed: false, pagesize: 10 });
            });
        }
    </script>


    <script type="text/javascript" src="../lib/jquery/jquery-3.5.1.slim.min.js" integrity="sha384-DfXdz2htPH0lsSSs5nCTpuj/zy4C+OGpamoFVy38MVBnE+IbbVYUew+OrCXaRkfj" crossorigin="anonymous"></script>
    <script src="../Scripts/pages.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.tablesorter.min.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.tablesorter.pager.js" type="text/javascript"></script>

    <script src="../Scripts/jquery.datetimepicker.js" type="text/javascript"></script>

    </asp:Content>

<%--</head>--%>
<asp:Content ID="Content2" ContentPlaceHolderID="placebody" runat="server">
    <noscript><meta http-equiv="refresh" content="0; url=sinsoporte.htm" /> </noscript>
        <%--<form id="wfconestsol" runat="server">--%>
            <input id="zonaid" type="hidden" value="7" />
                <asp:ToolkitScriptManager ID="tkscata" runat="server"></asp:ToolkitScriptManager>

                <div class="mt-4">         
                    <nav class="mt-4" aria-label="breadcrumb">
                        <ol class="breadcrumb">
                        <li class="breadcrumb-item" id="opcion_principal" runat="server"><a href="#">Depósito de Vacíos</a></li>
                            <li class="breadcrumb-item active" aria-current="page" id="sub_opcion" runat="server">Consulta de Movimientos e-Pass Zal/CISE/ZEA</li>
                        </ol>
                    </nav>
                </div>

                <div class="dashboard-container p-4" id="cuerpo" runat="server">
                    <input id="numbook" type="hidden" value="" runat="server" clientidmode="Static"/>
                    <input id="nbrboo" type="hidden" value="" runat="server" clientidmode="Static"/>
                    <span id="nomexpo" class="nover" type="hidden" runat="server" clientidmode="Static" enableviewstate="true" >...</span>
                    <span id="numexpo" class="nover" type="hidden" runat="server" clientidmode="Static" enableviewstate="true">...</span>

                        <div class="form-title">
                            Filtros para el reporte
                        </div>

                        <div class="form-row">

                            <div class="form-group col-md-6">
                                <label for="inputAddress">Número de Booking:<span style="color: #FF0000; font-weight: bold;"></span></label>
                                <div class="d-flex">
                                    <asp:TextBox CssClass="form-control" ID="txtsolicitud" runat="server"  MaxLength="25"
                                            style="text-align: center" 
                                            onkeypress="return soloLetras(event,'abcdefghijklmnopqrstuvwxyz01234567890/')" >
                                    </asp:TextBox>
                                    <a class="btn btn-outline-primary mr-4" target="popup" onclick="openPopup()" > <span class='fa fa-search' style='font-size:24px'></span> </a>
                                </div>
                            </div>

                            <div class="form-group col-md-6">
                                <label for="inputAddress">Exportador:<span style="color: #FF0000; font-weight: bold;"></span></label>
                                <div class="d-flex">
                                     <asp:TextBox CssClass="form-control" ID="TxtExportador" runat="server" MaxLength="250"
                                            style="text-align: center" 
                                            onkeypress="return soloLetras(event,'abcdefghijklmnopqrstuvwxyz01234567890/')" >
                                     </asp:TextBox>
                                     <a  class="btn btn-outline-primary mr-4" target="popup" onclick="openPopupExp()" > <span class='fa fa-search' style='font-size:24px'></span> </a>
                                </div>
                            </div>

                            <div class="form-group col-md-6">
                                <label for="inputAddress">Fecha Desde:<span style="color: #FF0000; font-weight: bold;"></span></label>
                                <div class="d-flex">
                                    <asp:TextBox ID="tfechaini" runat="server" MaxLength="10"  CssClass="datetimepicker form-control"
                                            onkeypress="return soloLetras(event,'01234567890/')" style="text-align: center" 
                                                ClientIDMode="Static">
                                    </asp:TextBox>
                                    <span id="valfechaini" class="validacion" > * </span>
                                </div>
                            </div>

                            <div class="form-group col-md-6">
                                <label for="inputAddress">Fecha Hasta:<span style="color: #FF0000; font-weight: bold;"></span></label>
                                <div class="d-flex">
                                     <asp:TextBox ID="tfechafin" runat="server" ClientIDMode="Static" MaxLength="15"  CssClass="datetimepicker form-control"
                                            onkeypress="return soloLetras(event,'01234567890/')"  style="text-align: center">
                                     </asp:TextBox>
                                    <span id="valfechafin" class="validacion" > * </span>
                                </div>
                            </div>

                        </div>

                        <div class="row">
                            <div class="col-md-12 d-flex justify-content-center">
                                <asp:Button ID="btbuscar" runat="server" Text="Iniciar la búsqueda" 
                                    class="btn btn-primary" 
                                    OnClientClick="return getGif();"
                                    onclick="btbuscar_Click" />
                                <span id="imagen"></span>
                            </div>
                        </div>

                        <div class="form-row">
                            <div class="form-group col-md-12"> 

                                <div class="cataresult" >
                                    
                                    <asp:UpdatePanel ID="upresult" runat="server" ChildrenAsTriggers="true">
                                        <ContentTemplate>
                           
                                        <input id="idlin" type="hidden" runat="server" clientidmode="Static" />
                                        <input id="diponible" type="hidden" runat="server" clientidmode="Static" />
                                        <div class="msg-alerta" id="alerta" runat="server" style=" display:none" ></div>
                                        <div id="xfinder" runat="server" visible="false" >

                                        <div><br /></div>

                                        <div class="findresult" >
                                  
                                            <div class="informativo" style=" height:100%;">
                                                   
                                                   <div class="bokindetalle" style=" width:100%; overflow:auto">
                                                        <rsweb:reportviewer ID="ReportViewer1" runat="server" Font-Names="Verdana" 
                                                            Font-Size="8pt" InteractiveDeviceInfos="(Colección)" 
                                                            WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt" 
                                                            >
                                                            <LocalReport ReportPath="zal\Rpt_movimientos_Zal.rdlc">
                                                                <DataSources>
                                                                    <rsweb:ReportDataSource DataSourceId="SqlDataSource_Detalle" 
                                                                        Name="SP_MOVIMIENTOS_PROFORMAS_ZAL" />
                                                                </DataSources>
                                                            </LocalReport>
                                                        </rsweb:reportviewer>
                                                        <asp:SqlDataSource ID="SqlDataSource_Detalle" runat="server" ConnectionString="<%$ ConnectionStrings:midle %>" SelectCommand="SP_MOVIMIENTOS_PROFORMAS_ZAL" SelectCommandType="StoredProcedure">        
                                                            <SelectParameters>  
                                                                <asp:SessionParameter  Type="String"  Name="fecha_desde"   SessionField="fecha_desde" />
                                                                <asp:SessionParameter   Type="String"  Name="fecha_hasta" SessionField="fecha_hasta"  />      
                                                                <asp:SessionParameter DbType="String" Name="booking" SessionField="booking" DefaultValue=" " />
                                                                <asp:SessionParameter DbType="String" Name="exportador" SessionField="exportador" DefaultValue=" " />
                                                            </SelectParameters>
                                                        </asp:SqlDataSource>
                                                    </div>

                                            </div>
                                        </div>
                                        </div>
                                            <div id="sinresultado" runat="server" class="alert alert-info"></div>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="btbuscar" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                    
                                </div>
                            </div>
                       </div>
                </div>
 <%--</form>--%>
    <script type="text/javascript" src="../lib/jquery/jquery-3.5.1.slim.min.js" integrity="sha384-DfXdz2htPH0lsSSs5nCTpuj/zy4C+OGpamoFVy38MVBnE+IbbVYUew+OrCXaRkfj" crossorigin="anonymous"></script>
    <script src="../Scripts/pages.js" type="text/javascript"></script>
    <script src="../Scripts/credenciales.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.tablesorter.min.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.tablesorter.pager.js" type="text/javascript"></script>

    <script src="../Scripts/jquery.datetimepicker.js" type="text/javascript"></script>
    
    <script type="text/javascript">
        $(document).ready(function () {
            $('.datetimepicker').datetimepicker().datetimepicker({ lang: 'es', timepicker: false, closeOnDateSelect: true, format: 'd/m/Y' });
        });
    </script>

    <script type="text/javascript">
        (function ($) {
            $("#header").ready(function () { $("#cargando").stop().animate({ width: "25%" }, 1500) });
            $("#footer").ready(function () { $("#cargando").stop().animate({ width: "75%" }, 1500) });
            $(window).load(function () {
                $("#cargando").stop().animate({ width: "100%" }, 600, function () {
                    $("#cargando").fadeOut("fast", function () { $(this).remove(); });
                });
            });
        })($);
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
                 document.getElementById('txtsolicitud').value = objeto.nbr;
                return;
            }

            //si catalogos es booking
            if (catalogo == 'numexport') {
                //document.getElementById('txtfecha').textContent = objeto.fecha;
               // document.getElementById('xfecha').value = objeto.fecha;
                document.getElementById('TxtExportador').value = document.getElementById('nomexpo').textContent;
               // alert(exportador.codigo);
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
        function prepareObject() {

            try {
                document.getElementById("loader").className = '';
                lista = [];
                //validaciones básicas
                var vals = document.getElementById('nbrboo');
                if (vals == null || vals == undefined || vals.value.trim().length <= 2) {
                    alert('* Datos de programación *\n *Escriba el numero de Booking*');
                    document.getElementById("loader").className = 'nover';
                    return;
                }
                vals = document.getElementById('xfecha');
                if (vals == null || vals == undefined || vals.value.trim().length <= 5) {
                    alert('* Datos de programación *\n Escriba la fecha de programación');
                    document.getElementById("loader").className = 'nover';
                    return;
                }
                vals = document.getElementById('tmail');
                if (vals == null || vals == undefined || vals.value.trim().length <= 5) {
                    alert('* Datos de programación *\n *Escriba el correo electrónico para la notificación');
                    document.getElementById("loader").className = 'nover';
                    return;
                }
                this.programacion.booking = document.getElementById('nbrboo').value;
                this.programacion.fecha_pro = document.getElementById('xfecha').value;
                this.programacion.mail = document.getElementById('tmail').value;
                this.programacion.idlinea = document.getElementById('idlin').value;
                this.programacion.linea = document.getElementById('agencia').value;
                this.programacion.total = document.getElementById('diponible').value;

                //recorrer tabla->
                var tbl = document.getElementById('tabla');
                for (var f = 0; f < tbl.rows.length; f++) {
                    var celColect = tbl.rows[f].getElementsByTagName('td');
                    if (celColect != undefined && celColect != null && celColect.length > 0) {

                        var tdetalle = {
                            num: celColect[0].textContent,
                            desde: celColect[1].textContent,
                            hasta: celColect[2].textContent,
                            dispone: celColect[4].textContent,
                            idh: celColect[5].textContent,
                            idd: celColect[6].textContent,
                            total: celColect[7].textContent
                        };
                        tdetalle.reserva = celColect[8].getElementsByTagName('input')[0].value;
                        this.lista.push(tdetalle);
                    }
                }
                this.programacion.detalles = this.lista;
                var qtlimite = parseInt(document.getElementById('diponible').value);
                var total = 0;
                for (var n = 0; n < this.lista.length; n++) {
                    if (lista[n].reserva != '') {
                        if (parseInt(lista[n].dispone) < parseInt(lista[n].reserva)) {
                            alert('El Horario ' + lista[n].desde + '-' + lista[n].hasta + ' excede su disponibilidad, favor verifique');
                            return;
                        }
                        total += parseInt(lista[n].reserva);
                    }
                }
                if (total > qtlimite) {
                    alert('* Reserva *\n La cantidad de reserva excede el cupo disponible \n Cupo: ' + qtlimite + '\n Reserva: ' + total);
                    return;
                }
                if (total <= 0) {
                    alert('* Reserva *\n La cantidad de reservas debe ser mayor que 0');
                    return;
                }
                tansporteServer(this.programacion, 'turnos.aspx/ValidateJSON');

            } catch (e) {
                alert('Lamentamos comunicarle que ha ocurrido el siguiente problema:\n' + e.Message);
            }
        }

        function openPop() {
            var bo = document.getElementById('nbrboo').value;
            if (bo == undefined || bo == '' || bo == null) {
                alert('Por favor seleccione el booking primero');
                return;
            }
            window.open('../catalogo/calendarioConsolidacion.aspx?bk=' + bo, 'name', 'width=850,height=880')
        }

         function openPopup() {
             window.open('../catalogo/bookinZAL.aspx', 'name', 'width=850,height=880');       
            return ;
        }
         function openPopupExp() {
             window.open('../catalogo/exportador.aspx', 'name', 'width=850,height=880');       
            return ;
        }
        function redirectsol(val, tipo, tiposolicitud, estado, idemp, asotrans) {
//            alert(tipo);
//            return;
            var caja = val;
            //window.open('../credenciales/documentos/?placa=' + caja, 'name', 'width=850,height=480')
            if (tipo == 'EMPRESA') {
                //                window.open('../credenciales/revision-solicitud-empresa/?numsolicitud=' + caja)
                window.open('../cliente/consultasolicitudempresa.aspx/?numsolicitud=' + caja)
            }
            if (tipo == "PERMANENTE" || tipo == "TEMPORAL") {
                window.open('../cliente/consultasolicitudpermisodeacceso.aspx/?numsolicitud=' + caja + '&ruc=' + idemp)
            }
            if (tipo == "PERMISO VEHICULAR PERMANENTE") {
                window.open('../cliente/consultasolicitudpermisodeaccesovehiculo.aspx/?numsolicitud=' + caja + '&ruc=' + idemp)
            }
            if (tipo == 'COLABORADOR' && tiposolicitud == 'PERMISO PROVISIONAL') {
                window.open('../cliente/consultasolicitudpermisoprovisional.aspx/?numsolicitud=' + caja)
            }
            if ((asotrans != '' || asotrans != null) && estado == 'PENDIENTE') {
                window.open('../transportista/bloquea_solicitud_colaborador.aspx/?numsolicitud=' + caja);
            }
            else if (tipo == 'COLABORADOR' && estado == 'FACTURADA') {
                //            if ((tipo == 'COLABORADOR' && tiposolicitud == 'CREDENCIAL' && estado == 'FACTURADA') || tipo == "PERMANENTE" || tipo == "TEMPORAL") {
                //                window.open('../credenciales/revision-solicitud-colaborador/?numsolicitud=' + caja)
                window.open('../cliente/consultafacturacolaborador.aspx/?sid1=' + caja)
            }
            else if (tipo == 'COLABORADOR' && estado != 'FACTURADA') {
                window.open('../cliente/consultasolicitudcolaborador.aspx/?numsolicitud=' + caja)
            }
            if (tipo == 'VEHICULO' && estado == 'FACTURADA') {
                window.open('../cliente/consultafacturavehiculo.aspx/?sid1=' + caja)
            }
            else if (tipo == 'VEHICULO' && estado != 'FACTURADA') {
                //                window.open('../credenciales/revision-solicitud-vehiculo/?numsolicitud=' + caja)
                window.open('../cliente/consultasolicitudvehiculo.aspx/?numsolicitud=' + caja)
            }
        }
        function getGif() {document.getElementById('imagen').innerHTML = '<img alt="" src="../shared/imgs/loader.gif"/>';
            return true;
        }
        function getGifOculta() {
            document.getElementById('imagen').innerHTML = '<img alt="" src=""/>';
            return true;
        }
    </script>
  </asp:Content>
<%--</html>--%>

