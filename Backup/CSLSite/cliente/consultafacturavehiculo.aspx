<%@ Page  Language="C#"  AutoEventWireup="true" Title="Consultar Factura"
         CodeBehind="consultafacturavehiculo.aspx.cs" Inherits="CSLSite.cliente.consultafacturavehiculo" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Consultar Factura</title>
     <link href="../shared/estilo/comprobantedepago.css" rel="stylesheet" type="text/css" />
   <link href="../shared/estilo/work.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/aisv-general.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/tables.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/info.css" rel="stylesheet" type="text/css" />
     <script src="Scripts/jquery-1.7.1.min.js" type="text/javascript"></script>
    <script src="../Scripts/jquery-1.7.1.min.js" type="text/javascript"></script>
    <link href="Inicio/base-site.css" rel="stylesheet" type="text/css" />
    <link href="Inicio/css/font-awesome.min.css" rel="stylesheet" type="text/css" />
    <link rel="shortcut icon" type="image/x-icon" href="../favicon.ico" />
    <link href="shared/estilo/work.css" rel="stylesheet" type="text/css" />
    <link href="../Inicio/base-site.css" rel="stylesheet" type="text/css" />
    <link href="../Inicio/css/font-awesome.min.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/work.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
     * input[type=text]
        {
            text-align:left!important;
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
    <form id="bookingfrm" runat="server">
    <input id="zonaid" type="hidden" value="7" />
    <asp:ToolkitScriptManager ID="tkscata" runat="server"></asp:ToolkitScriptManager>
     <div class="catabody">
     <div class="catawrap" >
 <div class="seccion">
 <div class="informativo">
          <table class="controles" cellspacing="0" cellpadding="1">
          <tr><td rowspan="2" class="inum"> <div class="number">1</div></td><td class="level1" >Tipo de Solicitud.</td></tr>
          <tr><td class="level2">Tipo de Solicitud.</td></tr>
          </table>
         </div>
       <div class="colapser colapsa"></div>
       <div class="accion">
         <table class="controles" style=" font-size:small" cellspacing="0" cellpadding="1">
            <tr>
            <td class="bt-bottom bt-right bt-left" style=" width:155px;" >Tipo de Solicitud:</td>
            <td class="bt-bottom bt-right">
            <asp:TextBox ID="txttipcli" runat="server" Width="99%" MaxLength="500" Enabled="false"
             style="text-align: center;"
             onkeypress="return soloLetras(event,'abcdefghijklnmñopqrstuvwxyz01234567890 ',true)"></asp:TextBox>
             </td>
            </tr>
         </table>
       </div>
       <div class="accion">
       <table class="xcontroles" cellspacing="0" cellpadding="1" style=" display:none">
       <tr><th class="bt-bottom bt-right bt-left bt-top" colspan="4"> Criterios de consulta:</th></tr>
       <tr>
        <td class="bt-bottom bt-right bt-left" >Número de Solicitud:</td>
        <td class="bt-bottom bt-right">
             <asp:TextBox ID="txtsolicitud" runat="server" Width="120px" MaxLength="11"
             style="text-align: center" onblur="cajaControl(this);"
             onkeypress="return soloLetras(event,'01234567890',true)"></asp:TextBox>
        </td>
       </tr>
       <tr style=" display:none">
         <td class="bt-bottom bt-right bt-left">Generados desde / hasta:</td>
         <td class="bt-bottom bt-right">
            <%--<a class="tooltip" >
            <span class="classic">Fecha desde.</span>--%>
            <asp:TextBox ID="tfechaini" runat="server" Width="120px" MaxLength="10" CssClass="datetimepicker"
             onkeypress="return soloLetras(event,'01234567890/')" style="text-align: center" 
                 onblur="valDate(this,true,valdate);" ClientIDMode="Static"></asp:TextBox>
            <%--</a>
            <a class="tooltip" >
            <span class="classic">Fecha hasta.</span>--%>
            <asp:TextBox ID="tfechafin" runat="server" ClientIDMode="Static" Width="120px" MaxLength="15" CssClass="datetimepicker"
             onkeypress="return soloLetras(event,'01234567890/')"  style="text-align: center" 
                  onblur="valDate(this,true,valdate);"></asp:TextBox>
            <%--</a>--%>
          </td>
        </tr>
       <tr style=" display:none">
         <td class="bt-bottom bt-right bt-left">Todas las facturas.</td>
         <td class="bt-bottom bt-right">
             <asp:CheckBox Text="" ID="chkTodos" runat="server" />
         </td>
       </tr>
       </table>
       <div class="botonera" style=" display:none">
          <img alt="loading.." src="../shared/imgs/loader.gif" id="loader" class="nover"  />
             <asp:Button ID="btbuscar" runat="server" Text="Iniciar la búsqueda" 
               onclick="btbuscar_Click"/>

       </div>
       </div>
 </div>
 <div class="seccion">
      <div class="informativo">
      <table>
      <tr><td rowspan="2" class="inum"> <div class="number">2</div></td><td class="level1" >Datos del Vehículo(s).</td></tr>
      <tr><td class="level2">
         Confirme que los datos sean correctos.
      </td></tr>
      </table>
     </div>
     <div class="colapser colapsa"></div>
     <div class="accion" >
      <div class="informativo" id="colector" >
      <asp:Repeater ID="rpVehiculos" runat="server" >
                 <HeaderTemplate>
                 <table id="tabla"  cellspacing="1" cellpadding="1" class="tabRepeat" style=" font-size:small">
                 <thead>
                 <tr>
                 <%--<th># Solicitud</th>--%>
                 <th class="nover" >IdSolVeh</th>
                 <th class="nover" >Tipo Solicitud</th>
                 <th>Placa</th>
                 <th>ClaseTipo</th>
                 <th>Marca</th>
                 <th>Modelo</th>
                 <th>Color</th>
                 <th>Categoria</th>
                 <th>Tipo Certificado</th>
                 <th>Nº Certificado</th>
                 <th>Fecha Poliza</th>
                 <th>Fecha MTOP</th>
                 <th>Vehiculo Rechazado</th>
                 <th>Comentario</th>
                 <th></th>
                 </tr>
                 </thead> 
                 <tbody>
                 </HeaderTemplate>
                 <ItemTemplate>
                 <tr class="point" >
                  <%--<td style=" width:65px"><%#Eval("NUMSOLICITUD")%></td>--%>
                  <td class="nover" ><%#Eval("IDSOLVEH")%></td>
                  <td class="nover" ><%#Eval("TIPOSOLICITUD")%></td>
                  <td><asp:Label Text='<%#Eval("PLACA")%>' runat="server" ID="lblplaca"></asp:Label></td>
                  <td><%#Eval("CLASETIPO")%></td>
                  <td><%#Eval("MARCA")%></td>
                  <td><%#Eval("MODELO")%></td>
                  <td><%#Eval("COLOR")%></td>
                  <td><%#Eval("DESCRIPCIONCATEGORIA")%></td>
                  <td><%#Eval("TIPOCERTIFICADO")%></td>
                  <td><%#Eval("CERTIFICADO")%></td>
                  <td style=" width:60px"><%#Eval("FECHAPOLIZA")%></td>
                  <td style=" width:65px"><%#Eval("FECHAMTOP")%></td>
                  <td style=" width:60px"><asp:CheckBox runat="server" ForeColor="Red" Checked='<%#Eval("ESTADOCOL")%>' Enabled="false" id="chkRevisado"/></td>
                  <td><asp:TextBox ID="tcomentario" Enabled="false" ForeColor="Red" Text='<%#Eval("COMENTARIO")%>' runat="server" Width="200px"  onkeypress="return soloLetras(event,'abcdefghijklnmñopqrstuvwxyz01234567890-_/ ',true)"></asp:TextBox></td>
                  <td>
                    <a style=" width:80px" id="adjDoc" class="topopup" onclick="redirectsol('<%# Eval("NUMSOLICITUD") %>', '<%# Eval("IDSOLVEH") %>', '<%#Eval("PLACA")%>');">
                    <i></i> Ver Documentos </a>
                  </td>
                 </tr>
                 </ItemTemplate>
                 <FooterTemplate>
                 </tbody>
                 </table>
                 </FooterTemplate>
         </asp:Repeater>
       </div>
     </div>
     </div>
 <div class="seccion">
  <div class="informativo">
          <table class="controles" cellspacing="0" cellpadding="1">
          <tr><td rowspan="2" class="inum"> <div class="number">3</div></td><td class="level1" >Datos de la factura.</td></tr>
          <tr><td class="level2">Revise la factura y una vez realizado el pago o deposito por favor adjunte el comprobante o retención.</td></tr>
          </table>
         </div>
       <div class="colapser colapsa"></div>
              <div class="cataresult" >
        <%--<asp:UpdatePanel ID="upresult" runat="server" ChildrenAsTriggers="true">
        <ContentTemplate>
        <script type="text/javascript">            Sys.Application.add_load(BindFunctions);</script>--%>
        <input id="idlin" type="hidden" runat="server" clientidmode="Static" />
        <input id="diponible" type="hidden" runat="server" clientidmode="Static" />
        <div id="xfinder" runat="server" visible="false">
        <div class="msg-alerta" id="alerta" runat="server" ></div>
        <div class="findresult" >
        <div class="separator">Información disponibles:</div>
        <div class="botonera" style=" height:100%; overflow:auto">
                 <div class="bokindetalle">
                 <table id="tablerp" class="controles" cellpadding="1" cellspacing="0">
                 <asp:Repeater ID="tablePagination" runat="server">
                 <HeaderTemplate>
                 <table id="tablar"  cellspacing="1" cellpadding="1" class="tabRepeat" width="100%">
                 <thead>
                 <tr>
                 <th># Solicitud</th>
                 <th>Tipo Solicitud</th>
                 <th>Factura</th>
                 <th>Estado</th>
                 <th>Adjunte el comprobante de pago/Retención.</th>
                 <th style=" display:none"></th>
                 <th>Observación de rechazo.</th>
                 <th>Comprobante de pago/Retención rechazado.</th>
                 </tr>
                 </thead> 
                 <tbody>
                 </HeaderTemplate>
                 <ItemTemplate>
                 <tr class="point" >
                  <td style=" width:60px"><asp:Label runat="server" Text='<%#Eval("NUMSOLICITUD")%>' ID="lblIdSolicitud" /></td>
                  <td><%#Eval("TIPOSOLICITUD")%></td>
                  <td style=" width:80px">
                  <a href='<%#Eval("RUTADOCUMENTO") %>' style=" width:80px" class="topopup" target="_blank">
                    <i></i> Ver Documento </a>
                 </td>
                 <td style=" width:100px"><%#Eval("ESTADO")%></td>
                 <td style=" width:300px">
                    <asp:FileUpload extension='<%#Eval("EXTENSION") %>' class="uploader" id="fsupload" title="Escoja el archivo en formato PDF."
                           onchange="validaextension(this)" style=" font-size:small" runat="server"/>
                </td>
                <td style=" display:none"><asp:Label runat="server" Text='<%#Eval("CODESTADO")%>' ID="lblEstado" /></td>
                <td style=" width:250px"><%#Eval("COMRECHAZO")%></td>
                <td style=" width:250px">
                  <a href='<%#Eval("RUTADOCRECHAZO") %>' style=" width:80px" class="topopup" target="_blank">
                    <i></i> Ver Documento </a>
                 </td>
                 </tr>
                 </ItemTemplate>
                 <FooterTemplate>
                 </tbody>
                 </table>
                 </FooterTemplate>
         </asp:Repeater>
                 </table>
                </div>
        <%--<div class="botonera" runat="server" id="btnera">
        <img alt="loading.." src="../shared/imgs/loader.gif" id="loader" class="nover"  />
        <input id="btsalvar" type="button" value="Proceder y Asignar"  onclick="prepareObject();encerar();" /> &nbsp;
        </div>--%>
        </div>
        <%--<div  runat="server" id="divsalir">
        <img alt="loading.." src="../shared/imgs/loader.gif" id="Img2" class="nover"  />
                <asp:Button ID="btnSalir" runat="server" Text="Salir" Width="125px"
                onclick="btnSalir_Click"
                ToolTip="Regresa a la Pantalla Consultar Solicitud."/>
        </div>--%>
        <div class="botonera" runat="server" id="botonera">
        <img alt="loading.." src="../shared/imgs/loader.gif" id="Img1" class="nover"  />
        <asp:Button ID="btsalvar" runat="server" Text="Enviar Comprobante" Width="160px"
                OnClientClick="return prepareObject('¿Esta seguro de enviar el comprobante de pago?');" onclick="btsalvar_Click" 
                ToolTip="Envia el comprobante de pago."/>
        </div>
        </div>
        </div>
        <div id="xfinderpagado" runat="server" style=" display:none">
        <div class="msg-alerta" id="alertapagado" runat="server" ></div>
        <div class="separator">Facturas disponibles:</div>
        <div class="findresult" >
        <div class="botonera" style=" height:100%; overflow:auto">
                 <div class="bokindetalle">
                 <table id="table1" class="controles" cellpadding="1" cellspacing="0">
                 <asp:Repeater ID="tablePagado" runat="server">
                 <HeaderTemplate>
                 <table id="tablar"  cellspacing="1" cellpadding="1" class="tabRepeat" width="100%">
                 <thead>
                 <tr>
                 <th># Solicitud</th>
                 <th>Tipo Solicitud</th>
                 <th>Estado</th>
                 <th>Comprobante de Pago/Retención</th>
                 </tr>
                 </thead> 
                 <tbody>
                 </HeaderTemplate>
                 <ItemTemplate>
                 <tr class="point" >
                  <td><asp:Label runat="server" Text='<%#Eval("NUMSOLICITUD")%>' ID="lblIdSolicitud" /></td>
                  <td style=" width:200px"><%#Eval("TIPOSOLICITUD")%></td>
                                   <td><%#Eval("ESTADO")%></td>
                  <td style=" width:150px">
                  <a href='<%#Eval("RUTADOCUMENTO") %>' style=" width:90px" class="topopup" target="_blank">
                    <i></i> Ver Comprobante </a>
                 </td>
                 </tr>
                 </ItemTemplate>
                 <FooterTemplate>
                 </tbody>
                 </table>
                 </FooterTemplate>
         </asp:Repeater>
                 </table>
                </div>
        </div>
        </div>
        </div>
        <div id="sinresultado" runat="server" class="msg-info"></div>
        <%--</ContentTemplate>
                     <Triggers>
                         <asp:AsyncPostBackTrigger ControlID="btbuscar" />
                     </Triggers>
        </asp:UpdatePanel>--%>
        </div>
 </div>
 </div>
 </div>
 </form>
   <script src="../Scripts/pages.js" type="text/javascript"></script>
    <script src="../Scripts/credenciales.js" type="text/javascript"></script>
    <script src="../Scripts/jquery-1.7.1.min.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.tablesorter.min.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.tablesorter.pager.js" type="text/javascript"></script>
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
        function prepareObject() {
            try {
                if (confirm('¿Esta seguro de enviar el comprobante de pago/Retención.?') == false) {
                    return false;
                }
                lista = [];
                var vals = document.getElementById('tablar');
                if (vals == '' || vals == null || vals == undefined) {
                    alert('* Consultar Solicitud de Factura: *\n *No se encontraron Documentos*');
                    document.getElementById("loader").className = 'nover';
                    return false;
                }
                if (vals != null) {
                    var tbl = document.getElementById('tablar');
                    for (var f = 0; f < tbl.rows.length; f++) {
                        var celColect = tbl.rows[f].getElementsByTagName('td');
                        if (celColect != undefined && celColect != null && celColect.length > 0) {
                            var tdetalle = {
                                documento: celColect[4].getElementsByTagName('input')[0].value
                            };
                            this.lista.push(tdetalle);
                        }
                    }
                    //                var nomdoc = null;
                    for (var n = 0; n < this.lista.length; n++) {
                        if (lista[n].documento == '' || lista[n].documento == null || lista[n].documento == undefined) {
                            alert('* Consultar Solicitud de Factura: *\n * Adjunte el recibo de pago. *');
                            document.getElementById("loader").className = 'nover';
                            return false;
                        }
                        //                    if (nomdoc == lista[n].documento) {
                        //                        alert('* Datos de Registro de Empresa: *\n * Existen archivos repetidos, revise por favor *');
                        //                        document.getElementById("loader").className = 'nover';
                        //                        return false;
                        //                    }
                        //                    nomdoc = lista[n].documento;
                    }
                }
                document.getElementById("loader").className = '';
                return true;
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
            window.open('../catalogo/calendarioConsolidacion.aspx?bk=' + bo, 'name', 'width=850,height=480')
        }

        function redirectsol(val, val2, val3) {
            var caja = val;
            var caja2 = val2;
            var caja3 = val3;
            window.open('../consulta-documentos-vehiculo/?numsolicitud=' + caja + '&idsolveh=' + caja2 + '&placa=' + caja3)
        }
    </script>
</body>
</html>

