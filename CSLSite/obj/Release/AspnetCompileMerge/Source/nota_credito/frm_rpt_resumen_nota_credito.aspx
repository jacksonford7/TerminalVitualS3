<%@ Page  Language="C#" AutoEventWireup="true" Title="Consulta Resumida de Nota de Crédito" CodeBehind="frm_rpt_resumen_nota_credito.aspx.cs" Inherits="CSLSite.zal.frm_rpt_resumen_nota_credito" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Consulta Resumida de Nota de Crédito</title>
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

</head>
<body>
  <noscript><meta http-equiv="refresh" content="0; url=sinsoporte.htm" /> </noscript>
   <form id="wfconestsol" runat="server">
   <input id="zonaid" type="hidden" value="7" />
    <asp:ToolkitScriptManager ID="tkscata" runat="server"></asp:ToolkitScriptManager>
    <div class="catabody" style=" height:100%">
     <div class="catawrap" >
 <div>
 <table>
 <tr>
 <td>   
 <div class="envolve">
      <a href="../cuenta/menu.aspx"><i class="element-menu"></i>Menú principal</a>
 </div>
 </td>
 <td>
  <i class="ico-titulo-1"></i><h2>Transacciones de Nota de Crédito</h2><h2>&nbsp;</h2><br />
   <i class="ico-titulo-2"></i><h1>Consulta de Movimientos</h1><br />
 </td>
 </tr>
 </table>
 </div>
 <div class="seccion">
       <input id="numbook" type="hidden" value="" runat="server" clientidmode="Static"/>
       <input id="nbrboo" type="hidden" value="" runat="server" clientidmode="Static"/>
      <span id="nomexpo" class="nover" type="hidden" runat="server" clientidmode="Static" enableviewstate="true" >...</span>
      <span id="numexpo" class="nover" type="hidden" runat="server" clientidmode="Static" enableviewstate="true">...</span>
      <%--<input id="nomexpo" type="hidden" value="" runat="server" clientidmode="Static"/>--%>
       <div class="accion">
       <table class="xcontroles" cellspacing="0" cellpadding="1">
         <!--DWLayoutTable-->
       <tr><th height="27" colspan="5" class="bt-bottom bt-right bt-left bt-top"> Criterios de consulta:</th></tr>
      
        <tr>
        <td height="37" class="bt-bottom bt-right bt-left" >Estado</td>
        <td width="452" valign="top" class="bt-bottom bt-right">
             <asp:DropDownList ID="CboEstado" runat="server" Width="150px" AutoPostBack="False"
                                            Height="28px"  DataTextField='name' DataValueField='id'  
                                            Font-Size="Small" 
                                            >
                                        </asp:DropDownList>        </td>
       
       <td width="91" valign="top" class="bt-bottom bt-right"> </td>
        <td colspan="2" valign="top" class="bt-bottom bt-right"><!--DWLayoutEmptyCell-->&nbsp;</td>
        </tr>
       <tr >
          <td height="34" class="bt-bottom bt-right bt-left">Motivo/Concepto:</td>
          <td valign="top" class="bt-bottom bt-right">
                <asp:DropDownList ID="CboConcepto" runat="server" Width="250px" AutoPostBack="False"
                                            Height="28px"  DataTextField='description' DataValueField='id_concept'  
                                            Font-Size="Small" > </asp:DropDownList>   </td>
          <td valign="top" class="bt-bottom bt-right"></td>
          <td colspan="2" valign="top" class="bt-bottom bt-right"><!--DWLayoutEmptyCell-->&nbsp;</td>
          </tr>
      
       <tr>
         <td height="54" class="bt-bottom bt-right bt-left">Generados desde / hasta:</td>
         <td colspan="3" class="bt-bottom bt-right">
            <%--<a class="tooltip" >
            <span class="classic">Fecha desde.</span>--%>
            <asp:TextBox ID="tfechaini" runat="server" Width="120px" MaxLength="10" CssClass="datetimepicker"
             onkeypress="return soloLetras(event,'01234567890/')" style="text-align: center" 
                  ClientIDMode="Static"></asp:TextBox><span id="valfechaini" class="validacion" > * obligatorio</span>
            <%--</a>
            <a class="tooltip" >
            <span class="classic">Fecha hasta.</span>--%>
            <asp:TextBox ID="tfechafin" runat="server" ClientIDMode="Static" Width="120px" MaxLength="15" CssClass="datetimepicker"
             onkeypress="return soloLetras(event,'01234567890/')"  style="text-align: center" 
                  ></asp:TextBox><span id="valfechafin" class="validacion" > * obligatorio</span>
            <%--</a>--%>          </td>
         <td width="845" class="bt-bottom bt-right">&nbsp;</td>
         </tr>
      
       
       <tr>
         <td height="5"></td>
         <td></td>
         <td></td>
         <td width="329"></td>
         <td></td>
       </tr>
       </table>
       <div class="botonera">
            <span id="imagen"></span>
             <asp:Button ID="btbuscar" runat="server" Text="Iniciar la búsqueda" OnClientClick="return getGif();"
               onclick="btbuscar_Click"/>
       </div>
       </div>
       </div>
        <div class="cataresult" >
        <asp:UpdatePanel ID="upresult" runat="server" ChildrenAsTriggers="true">
        <ContentTemplate>
<%--        <script type="text/javascript">
            Sys.Application.add_load(BindFunctions); 
                      </script>--%>
        <input id="idlin" type="hidden" runat="server" clientidmode="Static" />
        <input id="diponible" type="hidden" runat="server" clientidmode="Static" />
        <div class="msg-alerta" id="alerta" runat="server" style=" display:none" ></div>
        <div id="xfinder" runat="server" visible="false" >
        <div class="findresult" >
        <%--<div class="booking" >--%>
        <div class="informativo" style=" height:100%;">
             <%--    <div class="separator">Solicitudes disponibles:</div>--%>
                 <div class="bokindetalle">
                <rsweb:reportviewer ID="ReportViewer1" runat="server" Font-Names="Verdana" 
                    Font-Size="8pt" InteractiveDeviceInfos="(Colección)" 
                    WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt" 
                    >
                    <LocalReport ReportPath="nota_credito\Rpt_Resumen_Nota_Credito.rdlc">
                        <DataSources>
                            <rsweb:ReportDataSource DataSourceId="SqlDataSource_Detalle" 
                                Name="nc_c_list_credit_rpt_services" />
                        </DataSources>
                    </LocalReport>
                </rsweb:reportviewer>
                      <asp:SqlDataSource ID="SqlDataSource_Detalle" runat="server" ConnectionString="<%$ ConnectionStrings:NOTA_CREDITO %>" SelectCommand="nc_c_list_credit_rpt_services" SelectCommandType="StoredProcedure">        
                         <SelectParameters>  
                          <asp:SessionParameter DbType="Int32" Name="i_Estado" SessionField="estado" />
                          <asp:SessionParameter  Type="String"  Name="i_Desde"   SessionField="fecha_desde" />
                          <asp:SessionParameter   Type="String"  Name="i_Hasta" SessionField="fecha_hasta" />      
                          <asp:SessionParameter DbType="Int32" Name="i_id_concept" SessionField="id_concept" />

                    </SelectParameters>
                      </asp:SqlDataSource>
                </div>

        </div>
        </div>
        </div>
            <div id="sinresultado" runat="server" class="msg-info"></div>
        </ContentTemplate>
                     <Triggers>
                         <asp:AsyncPostBackTrigger ControlID="btbuscar" />
                     </Triggers>
        </asp:UpdatePanel>
        </div>
 </div>
 </form>
  <script src="../Scripts/pages.js" type="text/javascript"></script>
 <script src="../Scripts/credenciales.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.tablesorter.min.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.tablesorter.pager.js" type="text/javascript"></script>
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
            window.open('../catalogo/bookZAL', 'name', 'width=850,height=880');       
            return ;
        }
         function openPopupExp() {
            window.open('../catalogo/exportadores', 'name', 'width=850,height=880');       
            return ;
        }
        function redirectsol(val, tipo, tiposolicitud, estado, idemp, asotrans) {
//            alert(tipo);
//            return;
            var caja = val;
            //window.open('../credenciales/documentos/?placa=' + caja, 'name', 'width=850,height=480')
            if (tipo == 'EMPRESA') {
                //                window.open('../credenciales/revision-solicitud-empresa/?numsolicitud=' + caja)
                window.open('../cliente/consulta-empresa/?numsolicitud=' + caja)
            }
            if (tipo == "PERMANENTE" || tipo == "TEMPORAL") {
                window.open('../cliente/consulta-permiso-de-acceso/?numsolicitud=' + caja + '&ruc=' + idemp)
            }
            if (tipo == "PERMISO VEHICULAR PERMANENTE") {
                window.open('../cliente/consulta-permiso-de-acceso-vehicular/?numsolicitud=' + caja + '&ruc=' + idemp)
            }
            if (tipo == 'COLABORADOR' && tiposolicitud == 'PERMISO PROVISIONAL') {
                window.open('../cliente/consulta-permiso-provisional/?numsolicitud=' + caja)
            }
            if ((asotrans != '' || asotrans != null) && estado == 'PENDIENTE') {
                window.open('../transportista/bloquea-permiso-de-acceso/?numsolicitud=' + caja);
            }
            else if (tipo == 'COLABORADOR' && estado == 'FACTURADA') {
                //            if ((tipo == 'COLABORADOR' && tiposolicitud == 'CREDENCIAL' && estado == 'FACTURADA') || tipo == "PERMANENTE" || tipo == "TEMPORAL") {
                //                window.open('../credenciales/revision-solicitud-colaborador/?numsolicitud=' + caja)
                window.open('../cliente/consultar-factura-colaborador/?sid1=' + caja)
            }
            else if (tipo == 'COLABORADOR' && estado != 'FACTURADA') {
                window.open('../cliente/consulta-colaborador/?numsolicitud=' + caja)
            }
            if (tipo == 'VEHICULO' && estado == 'FACTURADA') {
                window.open('../cliente/consultar-factura-vehiculo/?sid1=' + caja)
            }
            else if (tipo == 'VEHICULO' && estado != 'FACTURADA') {
                //                window.open('../credenciales/revision-solicitud-vehiculo/?numsolicitud=' + caja)
                window.open('../cliente/consulta-vehiculo/?numsolicitud=' + caja)
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
    </body>
</html>

