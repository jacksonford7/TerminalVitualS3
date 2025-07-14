<%@ Page Language="C#" AutoEventWireup="true" Title="Solicitud de Permiso de Acceso"
CodeBehind="revisasolicitudpermisodeacceso.aspx.cs" Inherits="CSLSite.revisasolicitudpermisodeacceso" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Solicitud de Credencial/Permiso Provisional</title>
    <script src="../../Scripts/jquery-1.7.1.min.js" type="text/javascript"></script>
    <link href="../shared/estilo/jquery.datetimepicker.css" rel="stylesheet" type="text/css" />
    <script src="../../Scripts/jquery.datetimepicker.js" type="text/javascript"></script>
    <link href="../shared/estilo/catalogosolicitudcolaborador.css" rel="stylesheet" type="text/css" />
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
    <link rel="../shortcut icon" type="image/x-icon" href="../favicon.ico" />
    <link href="../shared/estilo/work.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
     * input[type=text]
        {
            text-align:left!important;
        }
    </style>
</head>
<body>
    <noscript><meta http-equiv="refresh" content="0; url=sinsoporte.htm" /> </noscript>
    <form id="bookingfrm"  runat="server">
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
      <tr><td rowspan="2" class="inum"> <div class="number">2</div></td><td class="level1" >
          Datos Generales de los Permisos de Acceso.</td></tr>
      <tr><td class="level2">
         Confirme que los datos sean correctos.
      </td></tr>
      </table>
     </div>
     <div class="colapser colapsa"></div>
     <div class="msg-alerta" id="alerta" runat="server" ></div>
     <div class="accion">
     <div style=" height:100%; overflow:auto">
    <asp:Repeater ID="tablePagination" runat="server"  >
                <HeaderTemplate>
                <table id="tableRpt"  cellspacing="1" cellpadding="1" class="tabRepeat" style=" font-size:small;">
<%--                 </thead> --%>
                <tr style="position:static">
                 <th style=" display:none"></th>
                 <th style=" display:none"></th>
                 <th>Cedula</th>
                 <th>Nombres</th>
                 <th>Apellidos</th>
                 <th>Área Destino/Actividad</th>
                 <th>Cargo</th>
                 <th style=" display:none">Permiso</th>
                 <th>Fecha Ingreso</th>
                 <th>Fecha Caducidad</th>
                 <th>Cambiar Permiso</th>
                 <th style=" display:none">Permiso OC</th>
                 <th>Fecha Ingreso OC</th>
                 <th>Fecha Caduc. OC</th>
                 <th>Turno. OC</th>
                 <th style=" display:none">Area. OC</th>
                 <th style=" display:none">Dpto. OC</th>
                 <th style=" display:none">Cargo. OC</th>
                 <th>Rechazado</th>
                 <th>Comentario</th>
                 <th>Ver</th>
                 </tr>
                 <%--<tbody>--%>
                 </HeaderTemplate>
                 <ItemTemplate>
                 <tr >
                  <td style=" display:none; width:1px"><%#Eval("NUMSOLICITUD")%></td>
                  <td style=" display:none; width:1px"><%#Eval("IDSOLPER")%></td>
                  <td style=" width:50px"><asp:Label Text='<%#Eval("CEDULA")%>' runat="server" id="lblcipas"/></td>
                  <td style=" width:80px"><asp:Label Text='<%#Eval("NOMBRES")%>' runat="server" id="lblnombres"/></td>
                  <td style=" width:80px"><asp:Label Text='<%#Eval("APELLIDOS")%>' runat="server" id="lblapellidos"/></td>
                  <td style=" width:80px"><%#Eval("AREADESTINO")%></td>
                  <td style=" width:80px"><%#Eval("CARGO")%></td>
                  <td style=" width:60px;display:none"><%#Eval("TIPO")%></td>
                  <td style=" width:70px"><asp:Label Text='<%#Eval("FECHAINGRESO")%>'  runat="server" id="lblfecing"/></td>
                  <td style=" width:70px"><asp:Label Text='<%#Eval("FECHACADUCIDAD")%>'  runat="server" id="lblfeccad"/></td>
                  <td style=" width:50px"><asp:CheckBox runat="server"  Width="50px" id="chkPermiso" onchange="validaPermiso();"/></td>
                  <td style=" width:80px;display:none"><asp:DropDownList runat="server" Width="80px" ID="ddlPermiso"></asp:DropDownList></td>
                  <td style=" width:80px">
                                    <asp:TextBox Style="text-align: center" ID="txtfecing" runat="server" Width="80px" Enabled="false" BackColor="Gray" 
                                    MaxLength="15" CssClass="datetimepicker" ClientIDMode="Static" onkeypress="return soloLetras(event,'0123456789/')"
                                    ></asp:TextBox>
                  </td>
                  <td style=" width:80px">
                                <asp:TextBox Style="text-align: center" ID="txtfecsal" runat="server" Width="80px" Enabled="false" BackColor="Gray" 
                                    MaxLength="15" CssClass="datetimepicker" ClientIDMode="Static" onkeypress="return soloLetras(event,'0123456789/')"
                                ></asp:TextBox>
                  </td>
                  <td style=" width:120px;"><asp:DropDownList runat="server" Width="120px" ID="ddlTurnoOnlyControl"></asp:DropDownList></td>
                  <td style=" width:120px; display:none"><asp:DropDownList runat="server" Width="120px" ID="ddlAreaOnlyControl"></asp:DropDownList></td>
                  <td style=" width:120px; display:none"><asp:DropDownList runat="server" Width="120px" ID="ddlDepartamentoOnlyControl"></asp:DropDownList></td>
                  <td style=" width:80px; display:none"><asp:DropDownList runat="server" Width="80px" ID="ddlCargoOnlyControl"></asp:DropDownList></td>      
                  <td style=" width:50px"><asp:CheckBox runat="server" Width="50px" id="chkRevisado" ForeColor="Red" Checked='<%#Eval("ESTADOSPA")%>' onchange="valRechazado()"/></td>
                  <td style=" width:150px"><asp:TextBox ID="tcomentario" runat="server" Width="150px" ForeColor="Red" Text='<%#Eval("COMENTARIO")%>' ToolTip='<%#Eval("COMENTARIO")%>' onkeypress="return soloLetras(event,'abcdefghijklnmñopqrstuvwxyz01234567890-_/ ',true)"></asp:TextBox></td>
                  <td style=" width:65px">
                    <a href='<%#Eval("RUTADOCUMENTO") %>' style=" width:65px" class="topopup" target="_blank">
                    <i></i>Documento(s)</a>    
                  </td>
                 </tr>
                 </ItemTemplate>
                 <FooterTemplate>
                 <%--</tbody>--%>
                 </table>
                 </FooterTemplate>
    </asp:Repeater>
    </div>
    </div>
     <div class="accion" id="factura" runat="server" style=" display:NONE" >
      <div class="accion">
       <div class="msg-alerta" id="alertafu" runat="server"></div>
       <table runat="server" class="controles" id="tablefac" style=" font-size:small">
       <tr><td class="bt-bottom bt-top  bt-right bt-left" style=" width:155px">Adjuntar factura:</td>
         <td class="bt-bottom bt-left bt-top bt-right">
         <asp:FileUpload runat="server" ID="fuAdjuntarFactura" Width="100%" extension='.pdf' class="uploader" title="Adjunte el archivo en formato PDF." onchange="validaextension(this)"/>
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
        <%--<img alt="loading.." src="../shared/imgs/loader.gif" id="Img1" class="nover"  />--%>
        <asp:Button Text="Rechazar" ID="btnRechazar" runat="server" Width="125px" 
                onclick="btnRechazar_Click" OnClientClick="return prepareObjectRechazar('¿Esta seguro de rechazar la solicitud?');"
                ToolTip="Rechaza la solicitud."/>
        <%--<img alt="loading.." src="../shared/imgs/loader.gif" id="Img2" class="nover"  />--%>
        <asp:Button ID="btsalvar" runat="server" Text="Crear Permiso(s)" Width="135px"
                OnClientClick="return prepareObject();" onclick="btsalvar_Click" 
                ToolTip="Crear el Permiso de Acceso."/>
        </div>
    </div>
     </div>
     </div>
    </form>
    <script src="../../Scripts/pages.js" type="text/javascript"></script>
    <script src="../../Scripts/credenciales.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery.tablesorter.min.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery.tablesorter.pager.js" type="text/javascript"></script>
   <script type="text/javascript" >
       $(document).ready(function () {
           //inicia los fecha
           $('.datetimepicker').datetimepicker().datetimepicker({ lang: 'es', timepicker: false, format: 'd/m/Y' });
       });
       function setObject(row) {
            self.close();
        }
        function prepareObjectRechazar() {
            var valor = '¿Esta seguro de rechazar la solicitud?';
            if (confirm(valor) == false) {
                return false;
            }
        }
        function AbrirVentana(url) {
            window.open(url, '_blank');
            return false;
        }
        function prepareObject() {
            try {
                var valor = '¿Esta seguro de procesar la solicitud?';
                if (confirm(valor) == false) {
                    return false;
                }
//                var vals = document.getElementById('<%=fuAdjuntarFactura.ClientID %>').value;
//                if (vals == '' || vals == null || vals == undefined) {
//                    alert('Adjunte la factura.');
//                    document.getElementById('<%=fuAdjuntarFactura.ClientID %>').focus();
//                    document.getElementById('<%=fuAdjuntarFactura.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:100%;";
//                    return false;
                //                }
                lista = [];
                var tbl = document.getElementById('tableRpt');
                for (var f = 0; f < tbl.rows.length; f++) {
                    var celColect = tbl.rows[f].getElementsByTagName('td');
                    if (celColect != undefined && celColect != null && celColect.length > 0) {
                        var vals = celColect[10].getElementsByTagName('input')[0].checked;
                        if (vals == true) {
//                            var vals = celColect[11].getElementsByTagName('select')[0].value;
//                            if (vals == '0' || vals == null || vals == undefined) {
//                                alert('Seleccione el tipo de Permiso OC');
//                                celColect[11].getElementsByTagName('select')[0].focus();
//                                celColect[11].getElementsByTagName('select')[0].style.cssText = "background-color:#ffffc6;color:Red;width:80px;";
//                                return false;
                            //                            }
                            var vals = celColect[12].getElementsByTagName('input')[0].value;
                            if (vals == '' || vals == null || vals == undefined) {
                                alert('Seleccione la Fecha de Ingreso OC');
                                celColect[12].getElementsByTagName('input')[0].focus();
                                celColect[12].getElementsByTagName('input')[0].style.cssText = "background-color:#ffffc6;color:Red;width:80px;";
                                return false;
                            }
                            var vals = celColect[13].getElementsByTagName('input')[0].value;
                            if (vals == '' || vals == null || vals == undefined) {
                                alert('Seleccione la Fecha de Caducidad OC');
                                celColect[13].getElementsByTagName('input')[0].focus();
                                celColect[13].getElementsByTagName('input')[0].style.cssText = "background-color:#ffffc6;color:Red;width:80px;";
                                return false;
                            }
                            var vals = celColect[14].getElementsByTagName('select')[0].value;
                            if (vals == '0' || vals == null || vals == undefined) {
                                alert('Seleccione el Turno OC');
                                celColect[14].getElementsByTagName('select')[0].focus();
                                celColect[14].getElementsByTagName('select')[0].style.cssText = "background-color:#ffffc6;color:Red;width:80px;";
                                return false;
                            }
//                            var vals = celColect[15].getElementsByTagName('select')[0].value;
//                            if (vals == '0' || vals == null || vals == undefined) {
//                                alert('Seleccione el Departamento OC');
//                                celColect[15].getElementsByTagName('select')[0].focus();
//                                celColect[15].getElementsByTagName('select')[0].style.cssText = "background-color:#ffffc6;color:Red;width:80px;";
//                                return false;
//                            }
//                            var vals = celColect[16].getElementsByTagName('select')[0].value;
//                            if (vals == '0' || vals == null || vals == undefined) {
//                                alert('Seleccione el Cargo OC');
//                                celColect[16].getElementsByTagName('select')[0].focus();
//                                celColect[16].getElementsByTagName('select')[0].style.cssText = "background-color:#ffffc6;color:Red;width:80px;";
//                                return false;
//                            }
                        }
                        var vals = celColect[18].getElementsByTagName('input')[0].checked;
                        if (vals == true) {
                            var vals = celColect[19].getElementsByTagName('input')[0].value;
                            if (vals == '' || vals == null || vals == undefined) {
                                alert('Escriba el comentario de rechazo');
                                celColect[18].getElementsByTagName('input')[0].focus();
                                celColect[18].getElementsByTagName('input')[0].style.cssText = "background-color:#ffffc6;color:Red;width:80px;";
                                return false;
                            }
                        }
                        else {
                            var vals = celColect[14].getElementsByTagName('select')[0].value;
                            var valst = celColect[14].getElementsByTagName('select')[0];
                            var valsturno = valst.options[valst.selectedIndex].text;
                            if (vals == '0' || vals == null || vals == undefined || valsturno == '* Elija *') {
                                alert('Seleccione el Turno OC');
                                celColect[14].getElementsByTagName('select')[0].focus();
                                celColect[14].getElementsByTagName('select')[0].style.cssText = "background-color:#ffffc6;color:Red;width:150px;";
                                return false;
                            }
                        }
                        var vals = celColect[10].getElementsByTagName('input')[0].checked;
                        if (vals == false) {
                            var vals1 = celColect[11].getElementsByTagName('select')[0].value;
                            var vals2 = celColect[12].getElementsByTagName('input')[0].value;
                            var vals3 = celColect[13].getElementsByTagName('input')[0].value;
                            var vals4 = celColect[14].getElementsByTagName('select')[0].value;
                            var vals5 = celColect[15].getElementsByTagName('select')[0].value;
                            var vals6 = celColect[16].getElementsByTagName('select')[0].value;
                            if (vals1 != '0' && vals2 != '' && vals3 != '' && vals4 != '0' && vals5 != '0' && vals6 != '0') {
                                alert('De check en la casilla Cambiar Permiso');
                                celColect[10].getElementsByTagName('input')[0].focus();
                                celColect[10].getElementsByTagName('input')[0].style.cssText = "background-color:Red;color:Red;width:50px;";
                                return false;
                            }
                        }
                    }
                }
                return true;
            } catch (e) {
                alert('Lamentamos comunicarle que ha ocurrido el siguiente problema:\n' + e.Message);
            }
        }
//        function valToolTip(index) {
//            lista = [];
//            var tbl = document.getElementById('tableRpt');
//            for (var f = 0; f < tbl.rows.length; f++) {
//                var celColect = tbl.rows[f].getElementsByTagName('td');
//                if (celColect != undefined && celColect != null && celColect.length > 0) {
//                    var vals = celColect[index].getElementsByTagName('select')[0].value;
//                    if (vals != '0' || vals != null || vals != undefined) {
//                        alert(vals);
//                        celColect[index].getElementsByTagName('select')[0].tooltip = vals;
//                    }
//                }
//            }
//        }
        function validaPermiso() {
            lista = [];
            var tbl = document.getElementById('tableRpt');
            for (var f = 0; f < tbl.rows.length; f++) {
                var celColect = tbl.rows[f].getElementsByTagName('td');
                if (celColect != undefined && celColect != null && celColect.length > 0) {
                    if (celColect[10].getElementsByTagName('input')[0].checked == true) {
                        celColect[18].getElementsByTagName('input')[0].checked = false;
                        celColect[12].getElementsByTagName('input')[0].style.cssText = "background-color:White;width:80px;"; //fecing
                        celColect[13].getElementsByTagName('input')[0].style.cssText = "background-color:White;width:80px;"; //fecsal
                        celColect[12].getElementsByTagName('input')[0].disabled = false;
                        celColect[13].getElementsByTagName('input')[0].disabled = false;
                    }
                    else {
                        celColect[12].getElementsByTagName('input')[0].style.cssText = "background-color:Gray;width:80px;"; //fecing
                        celColect[13].getElementsByTagName('input')[0].style.cssText = "background-color:Gray;width:80px;"; //fecsal
                        celColect[12].getElementsByTagName('input')[0].disabled = true;
                        celColect[13].getElementsByTagName('input')[0].disabled = true;
                    }
                }
            }
        }
        function valRechazado() {
            lista = [];
            var tbl = document.getElementById('tableRpt');
            for (var f = 0; f < tbl.rows.length; f++) {
                var celColect = tbl.rows[f].getElementsByTagName('td');
                if (celColect != undefined && celColect != null && celColect.length > 0) {
                    if (celColect[18].getElementsByTagName('input')[0].checked == true) {
                        celColect[10].getElementsByTagName('input')[0].checked = false
                        celColect[12].getElementsByTagName('input')[0].style.cssText = "background-color:Gray;width:80px;"; //fecing
                        celColect[13].getElementsByTagName('input')[0].style.cssText = "background-color:Gray;width:80px;"; //fecsal
                        celColect[12].getElementsByTagName('input')[0].disabled = true;
                        celColect[13].getElementsByTagName('input')[0].disabled = true;
                    }
                }
            }
        }
//        function fCambiaPermiso() {
//            lista = [];
//            var tbl = document.getElementById('tableRpt');
//            for (var f = 0; f < tbl.rows.length; f++) {
//                var celColect = tbl.rows[f].getElementsByTagName('td');
//                if (celColect != undefined && celColect != null && celColect.length > 0) {
//                    if (celColect[10].getElementsByTagName('input')[0].checked == true) {
//                            celColect[17].getElementsByTagName('input')[0].checked = false;
//                            alert("true");
//                            celColect[12].getElementsByTagName('input')[0].style.cssText = "background-color:White;width:80px;"; //fecing
//                            celColect[13].getElementsByTagName('input')[0].style.cssText = "background-color:White;width:80px;"; //fecsal
//                            celColect[12].getElementsByTagName('input')[0].disabled = false;
//                            celColect[13].getElementsByTagName('input')[0].disabled = false;
//                        }
//                        else {
//                            alert("false");
//                            celColect[12].getElementsByTagName('input')[0].style.cssText = "background-color:Gray;width:80px;"; //fecing
//                            celColect[13].getElementsByTagName('input')[0].style.cssText = "background-color:Gray;width:80px;"; //fecsal
//                            celColect[12].getElementsByTagName('input')[0].disabled = true;
//                            celColect[13].getElementsByTagName('input')[0].disabled = true;
//                        }
//                    }
//                }
//            }
//        }
        
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
          window.open('../consulta/documentos-solicitud-colaborador/?numsolicitud=' + caja + '&idsolcol=' + caja2 + '&cedula=' + caja3)
      }
   </script>
</body>
</html>
