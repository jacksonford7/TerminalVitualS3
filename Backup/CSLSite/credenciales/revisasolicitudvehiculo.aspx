﻿<%@ Page Language="C#" AutoEventWireup="true" Title="Emisión Permiso Vehicular"
CodeBehind="revisasolicitudvehiculo.aspx.cs" Inherits="CSLSite.revisasolicitudvehiculo" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Solicitud de Sticker/Provisional Vehícular</title>
    <link href="../shared/estilo/catalogosolicitudvehiculo.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/work.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/aisv-general.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/tables.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/info.css" rel="stylesheet" type="text/css" />
      <link href="Inicio/base-site.css" rel="stylesheet" type="text/css" />
    <link href="Inicio/css/font-awesome.min.css" rel="stylesheet" type="text/css" />
    <link rel="shortcut icon" type="image/x-icon" href="../favicon.ico" />
    <link href="shared/estilo/work.css" rel="stylesheet" type="text/css" />
    <link href="../Inicio/base-site.css" rel="stylesheet" type="text/css" />
    <link href="../Inicio/css/font-awesome.min.css" rel="stylesheet" type="text/css" />
    <link rel="../shortcut icon" type="image/x-icon" href="../favicon.ico" />
    <link href="../shared/estilo/work.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
     * input[type=text]
        {
            text-align:left!important;
        }
        #tablerp
        {
            width: 726px;
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
          <tr><td class="level2">Tipo de Solcitud.</td></tr>
          </table>
         </div>
       <div class="colapser colapsa"></div>
       <div class="accion">
         <table class="controles" style=" font-size:small" cellspacing="0" cellpadding="1">
            <tr>
            <td class="bt-bottom bt-right bt-left" style=" width:155px;" >Tipo de Solicitud:</td>
            <td class="bt-bottom bt-right">
            <asp:TextBox ID="txttipcli" runat="server" Width="400px" MaxLength="500" Enabled="false"
             style="text-align: center;"
             onkeypress="return soloLetras(event,'abcdefghijklnmñopqrstuvwxyz01234567890 ',true)"></asp:TextBox>
             </td>
            </tr>
         </table>
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
      <div class="accion">
      <div class="informativo" id="colector" style=" height:100%; overflow:auto">
      <div class="msg-alerta" id="alerta" runat="server"></div>
      <asp:Repeater ID="tablePagination" runat="server" >
                 <HeaderTemplate>
                 <table id="tablar2"  cellspacing="1" cellpadding="1" class="tabRepeat" style=" font-size:small">
                 <thead>
                 <tr>
                 <%--<th># Solicitud</th>--%>
                 <th class="nover">IdSolVeh</th>
                 <%--<th class="nover">Tipo Solicitud</th>--%>
                 <th>Placa</th>
                 <th>ClaseTipo</th>
                 <th>Marca</th>
                 <th>Modelo</th>
                 <th>Color</th>
                 <th>Tipo</th>
                 <th>Categoria</th>
                 <th>Área Destino/Actividad</th>
                 <th>Tipo Certificado</th>
                 <th>Nº Certificado</th>
                 <th>Fecha Poliza</th>
                 <th>Fecha MTOP</th>
                 <th>Vehiculo Rechazado</th>
                 <th>Comentario</th>
                 <th></th>
                 <th style=" display:none"></th>
                 </tr>
                 </thead> 
                 <tbody>
                 </HeaderTemplate>
                 <ItemTemplate>
                 <tr class="point" >
                  <%--<td style=" width:65px"><%#Eval("NUMSOLICITUD")%></td>--%>
                  <td class="nover"><%#Eval("IDSOLVEH")%></td>
                  <%--<td class="nover"><%#Eval("TIPOSOLICITUD")%></td>--%>
                  <td><asp:Label Text='<%#Eval("PLACA")%>' runat="server" ID="lblplaca"></asp:Label></td>
                  <td><%#Eval("CLASETIPO")%></td>
                  <td><%#Eval("MARCA")%></td>
                  <td><%#Eval("MODELO")%></td>
                  <td><%#Eval("COLOR")%></td>
                  <td style=" width:50px"><%#Eval("TIPO")%></td>
                  <td><%#Eval("DESCRIPCIONCATEGORIA")%></td>
                  <td style=" width:100px"><%#Eval("AREA")%></td>
                  <td><%#Eval("TIPOCERTIFICADO")%></td>
                  <td><%#Eval("CERTIFICADO")%></td>
                  <td style=" width:60px"><%#Eval("FECHAPOLIZA")%></td>
                  <td style=" width:65px"><%#Eval("FECHAMTOP")%></td>
                  <td style=" width:60px"><asp:CheckBox runat="server" ForeColor="Red" Enabled="false" id="chkRevisado"/></td>
                  <td style=" width:60px"><asp:TextBox ID="tcomentario" ForeColor="Red" runat="server" Width="200px" Text='' onkeypress="return soloLetras(event,'abcdefghijklnmñopqrstuvwxyz01234567890-_/ ',true)"></asp:TextBox></td>
                  <td style=" width:80px">
                    <a style=" width:80px" id="adjDoc" class="topopup" onclick="redirectsol('<%# Eval("NUMSOLICITUD") %>', '<%# Eval("IDSOLVEH") %>', '<%#Eval("PLACA")%>');">
                    <i></i> Ver Documentos </a>
                  </td>
                  <td style=" display:none"><asp:TextBox ID="tced" runat="server" Text='<%#Eval("PLACA")%>'></asp:TextBox></td>
                 </tr>
                 </ItemTemplate>
                 <FooterTemplate>
                 </tbody>
                 </table>
                 </FooterTemplate>
         </asp:Repeater>
       </div>
       </div>
      <div class="accion" id="factura" runat="server" >
      <div class="accion">
      <div class="msg-alerta" id="alertafu" runat="server"></div>
       <table runat="server" class="controles" id="tablefac" style=" font-size:small">
       <tr><td class="bt-bottom bt-top  bt-right bt-left" style=" width:155px">Adjuntar factura:</td>
         <td class="bt-bottom bt-left bt-top bt-right">
         <asp:FileUpload runat="server" ID="fuAdjuntarFactura" Width="100%" extension='.pdf' class="uploader" title="Escoja el archivo en formato PDF." onchange="validaextension(this)"/>
         </td>
         </tr>
       </table>
       </div>
       </div>
      <div class="botonera" runat="server" id="salir" visible="false">
        <asp:Button ID="btnSalir" runat="server" Text="Salir" Width="125px"
                onclick="btnSalir_Click" 
                ToolTip="Regresa a la Pantalla Consultar Solicitud."/>
    </div>
      <div class="botonera" runat="server" id="botonera">
        <img alt="loading.." src="../shared/imgs/loader.gif" id="Img1" class="nover"  />
        <asp:Button Text="Rechazar" ID="btnRechazar" runat="server" Width="125px" 
                onclick="btnRechazar_Click" OnClientClick="return prepareObjectR('¿Esta seguro de rechazar la solicitud?');"
                ToolTip="Rechaza la solicitud."/>
        <img alt="loading.." src="../shared/imgs/loader.gif" id="Img2" class="nover"  />
        <asp:Button ID="btsalvar" runat="server" Text="Enviar Factura" Width="135px"
                OnClientClick="return prepareObject('¿Esta seguro de procesar la solicitud?');" onclick="btsalvar_Click" 
                ToolTip="Aprueba la solicitud."/>
        </div>
     </div>
      </div>
      </div>
    </form>
    <script src="../Scripts/pages.js" type="text/javascript"></script>
    <script src="../../Scripts/credenciales.js" type="text/javascript"></script>
    <script src="../Scripts/jquery-1.7.1.min.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.tablesorter.min.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.tablesorter.pager.js" type="text/javascript"></script>
   <script type="text/javascript" >
       var ced_count = 0;
       var jAisv = {};
       function setObject(row) {
            self.close();
        }
        function prepareObject(valor) {
            try {
                if (confirm(valor) == false) {
                    return false;
                }
//                var vals = document.getElementById('<%=fuAdjuntarFactura.ClientID %>').value;
//                if (vals == '' || vals == null || vals == undefined) {
//                    alert('* Adjunte la factura.  *');
//                    document.getElementById('<%=fuAdjuntarFactura.ClientID %>').focus();
//                    document.getElementById('<%=fuAdjuntarFactura.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:400;";
//                    return false;
//                }
                var lista2 = [];
                var tbl2 = document.getElementById('tablar2');
                for (var r = 0; r < tbl2.rows.length; r++) {
                    var celColect2 = tbl2.rows[r].getElementsByTagName('td');
                    if (celColect2 != undefined && celColect2 != null && celColect2.length > 0) {
                        celColect2[13].getElementsByTagName('input')[0].disabled = false;
                    }
                }
            } catch (e) {
                alert('Lamentamos comunicarle que ha ocurrido el siguiente problema:\n' + e.Message);
            }
        }
        function prepareObjectR(valor) {
            try {
                if (confirm(valor) == false) {
                    return false;
                }
                var lista2 = [];
                var tbl2 = document.getElementById('tablar2');
                for (var r = 0; r < tbl2.rows.length; r++) {
                    var celColect2 = tbl2.rows[r].getElementsByTagName('td');
                    if (celColect2 != undefined && celColect2 != null && celColect2.length > 0) {
                        celColect2[13].getElementsByTagName('input')[0].disabled = false;
                    }
                }
                return true;
            } catch (e) {
                alert('Lamentamos comunicarle que ha ocurrido el siguiente problema:\n' + e.Message);
            }
        }
        function popupCallback(objeto, catalogo) {
            if (objeto == null || objeto == undefined) {
                alert('Hubo un problema al setaar un objeto de catalogo');
                return;
            }
            if (objeto.valor == "1") {
                var lista2 = [];
                var tbl2 = document.getElementById('tablar2');
                for (var r = 0; r < tbl2.rows.length; r++) {
                    var celColect2 = tbl2.rows[r].getElementsByTagName('td');
                    if (celColect2 != undefined && celColect2 != null && celColect2.length > 0) {
                        if (celColect2[16].getElementsByTagName('input')[0].value == objeto.placa) {
                            celColect2[13].getElementsByTagName('input')[0].checked = true;
                        }
                    }
                }
            }
            else {
                var lista2 = [];
                var tbl2 = document.getElementById('tablar2');
                for (var r = 0; r < tbl2.rows.length; r++) {
                    var celColect2 = tbl2.rows[r].getElementsByTagName('td');
                    if (celColect2 != undefined && celColect2 != null && celColect2.length > 0) {
                        if (celColect2[16].getElementsByTagName('input')[0].value == objeto.placa) {
                            celColect2[13].getElementsByTagName('input')[0].checked = false;
                        }
                    }
                }
            }
        }
        function initFinder() {
            if (document.getElementById('txtname').value.trim().length <= 0) {
                alert('Por favor escriba una o varias \nletras del número');
                return false;
            }
            document.getElementById('imagen').innerHTML = '<img alt="" src="../shared/imgs/loader.gif">';
        }
        function Quit() {
    //          event.returnValue = '[ ! ] Se perdera el trabajo realizado [ ! ]';
    //          return;
        }
        function redirectsol(val, val2, val3) {
            var caja = val;
            var caja2 = val2;
            var caja3 = val3;
            //window.open('../credenciales/documentos/?placa=' + caja, 'name', 'width=850,height=480')
            window.open('../consulta/documentos-solicitud-vehiculo/?numsolicitud=' + caja + '&idsolveh=' + caja2 + '&placa=' + caja3)
        }
   </script>
</body>
</html>

