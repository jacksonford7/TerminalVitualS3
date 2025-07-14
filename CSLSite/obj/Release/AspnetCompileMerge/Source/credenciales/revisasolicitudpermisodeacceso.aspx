<%@ Page Language="C#" AutoEventWireup="true" Title="Solicitud de Permiso de Acceso"
CodeBehind="revisasolicitudpermisodeacceso.aspx.cs" Inherits="CSLSite.revisasolicitudpermisodeacceso" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="../img/favicon2.png" rel="icon"/>
  <link href="../img/icono.png" rel="apple-touch-icon"/>
  <link href="../css/bootstrap.min.css" rel="stylesheet"/>
  <link href="../css/dashboard.css" rel="stylesheet"/>
  <link href="../css/icons.css" rel="stylesheet"/>
  <link href="../css/style.css" rel="stylesheet"/>
  <link href="https://fonts.googleapis.com/css2?family=Roboto:wght@300;400;500;700&display=swap" rel="stylesheet"/>
    <link href="../shared/estilo/jquery.datetimepicker.css" rel="stylesheet" type="text/css" />
   <script src="../Scripts/jquery.datetimepicker.js" type="text/javascript"></script>

    <!--external css-->
  <link href="../lib/font-awesome/css/font-awesome.css" rel="stylesheet" />
  <link rel="stylesheet" type="text/css" href="../lib/gritter/css/jquery.gritter.css" />

    <!--mensajes-->
  <link href="https://cdn.jsdelivr.net/npm/alertifyjs@1.11.0/build/css/alertify.min.css" rel="stylesheet"/>

    <%--<title>Solicitud de Credencial/Permiso Provisional</title>
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
    </style>--%>
</head>
<body>
    <div class="dashboard-container p-4" id="cuerpo" runat="server">
    <noscript><meta http-equiv="refresh" content="0; url=sinsoporte.htm" /> </noscript>
    <form id="bookingfrm"  runat="server">
    <input id="zonaid" type="hidden" value="7" />
    <asp:ToolkitScriptManager ID="tkscata" runat="server"></asp:ToolkitScriptManager>
     
    <div class="form-row">
            <div class="form-title col-md-12">
             <a class="btn btn-outline-primary mr-4" href="#" runat="server" id="aprint" clientidmode="Static" >1</a>
             <a class="level1" runat="server" id="a1" clientidmode="Static" >Tipo de Solicitud</a>
            </div>
             <div class="form-group col-md-6">
                 <label for="inputAddress">Tipo de Solicitud:<span style="color: #FF0000; font-weight: bold;"> *</span></label>
                 </div>
        <div class="form-group col-md-6">
                 <asp:TextBox ID="txttipcli" runat="server" MaxLength="500" Enabled="false" style="text-align: center;"
                    onkeypress="return soloLetras(event,'abcdefghijklnmñopqrstuvwxyz01234567890 ',true)" CssClass="form-control"></asp:TextBox>
                 </div>
    </div>

    <div class="form-row">
        <div class="form-title col-md-12">
             <a class="btn btn-outline-primary mr-4" href="#" runat="server" id="a2" clientidmode="Static" >2</a>
             <a class="level1" runat="server" id="a3" clientidmode="Static" >Datos Generales de los Permisos de Acceso</a>
         </div>

     <div class="alert alert-danger" id="alerta" runat="server" ></div>
     <div style=" overflow:auto">
    <asp:Repeater ID="tablePagination" runat="server"  >
                <HeaderTemplate>
                <table id="tableRpt" cellspacing="1" cellpadding="1" class="table table-bordered invoice">
                    <thead>
                <tr>
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
                 </thead>
                <tbody>
                </HeaderTemplate>
                
                 <ItemTemplate>
                 <tr >
                  <td style=" display:none; width:1px"><%#Eval("NUMSOLICITUD")%></td>
                  <td style=" display:none; width:1px"><%#Eval("IDSOLPER")%></td>
                  <td ><asp:Label Text='<%#Eval("CEDULA")%>' runat="server" id="lblcipas"/></td>
                  <td ><asp:Label Text='<%#Eval("NOMBRES")%>' runat="server" id="lblnombres"/></td>
                  <td ><asp:Label Text='<%#Eval("APELLIDOS")%>' runat="server" id="lblapellidos"/></td>
                  <td ><%#Eval("AREADESTINO")%></td>
                  <td ><%#Eval("CARGO")%></td>
                  <td style=" display:none"><%#Eval("TIPO")%></td>
                  <td ><asp:Label Text='<%#Eval("FECHAINGRESO")%>'  runat="server" id="lblfecing"/></td>
                  <td ><asp:Label Text='<%#Eval("FECHACADUCIDAD")%>'  runat="server" id="lblfeccad"/></td>
                  <td ><asp:CheckBox runat="server"  id="chkPermiso" onchange="validaPermiso();"/></td>
                  <td style=" display:none"><asp:DropDownList runat="server" Width="80px" ID="ddlPermiso"></asp:DropDownList></td>
                  <td >
                                    <asp:TextBox Style="text-align: center" ID="txtfecing" runat="server" Enabled="false" BackColor="Gray" 
                                    MaxLength="15" CssClass="datetimepicker" ClientIDMode="Static" onkeypress="return soloLetras(event,'0123456789/')"
                                    ></asp:TextBox>
                  </td>
                  <td >
                                <asp:TextBox Style="text-align: center" ID="txtfecsal" runat="server" Enabled="false" BackColor="Gray" 
                                    MaxLength="15" CssClass="datetimepicker" ClientIDMode="Static" onkeypress="return soloLetras(event,'0123456789/')"
                                ></asp:TextBox>
                  </td>
                  <td ><asp:DropDownList runat="server" ID="ddlTurnoOnlyControl"></asp:DropDownList></td>
                  <td style=" display:none"><asp:DropDownList runat="server"  ID="ddlAreaOnlyControl"></asp:DropDownList></td>
                  <td style=" display:none"><asp:DropDownList runat="server"  ID="ddlDepartamentoOnlyControl"></asp:DropDownList></td>
                  <td style=" display:none"><asp:DropDownList runat="server"  ID="ddlCargoOnlyControl"></asp:DropDownList></td>      
                  <td ><asp:CheckBox runat="server" id="chkRevisado" ForeColor="Red" Checked='<%#Eval("ESTADOSPA")%>' onchange="valRechazado()"/></td>
                  <td ><asp:TextBox ID="tcomentario" runat="server" ForeColor="Red" Text='<%#Eval("COMENTARIO")%>' ToolTip='<%#Eval("COMENTARIO")%>' onkeypress="return soloLetras(event,'abcdefghijklnmñopqrstuvwxyz01234567890-_/ ',true)"></asp:TextBox></td>
                  <td >
                    <a href='<%#Eval("RUTADOCUMENTO") %>' style=" class="btn btn-outline-primary mr-4" target="_blank">
                    <i class="fa fa-search"></i>Documento(s)</a>    
                  </td>
                 </tr>
                 </ItemTemplate>
                 <FooterTemplate>
                     </tbody>
                    </table>
                 </FooterTemplate>
    </asp:Repeater>
    </div>

     <div class="col-md-12 d-flex justify-content-rigth" id="factura" runat="server" style=" display:NONE" >
       <div class="alert alert-danger" id="alertafu" runat="server"></div>
        <span class="bt-bottom bt-top  bt-right bt-left" >Adjuntar factura:</span>
         <asp:FileUpload runat="server" ID="fuAdjuntarFactura" Width="100%" extension='.pdf' class="btn btn-outline-primary mr-4" title="Adjunte el archivo en formato PDF." onchange="validaextension(this)"/>
     </div>

     <div class="col-md-12 d-flex justify-content-rigth" runat="server" id="salir" visible="false">
        <asp:Button ID="btnSalir" runat="server" Text="Salir" onclick="btnSalir_Click" 
                ToolTip="Regresa a la Pantalla Consultar Solicitud." CssClass="btn btn-outline-primary mr-4"/>
    </div>

     <div class="col-md-12 d-flex justify-content-rigth" runat="server" id="botonera">
        <asp:Button Text="Rechazar" ID="btnRechazar" runat="server" onclick="btnRechazar_Click" CssClass="btn btn-primary mr-4"
            OnClientClick="return prepareObjectRechazar('¿Esta seguro de rechazar la solicitud?');" ToolTip="Rechaza la solicitud."/>
         <span>&nbsp;</span>
        <asp:Button ID="btsalvar" runat="server" Text="Crear Permiso(s)" onclick="btsalvar_Click" 
                OnClientClick="return prepareObject();" ToolTip="Crear el Permiso de Acceso." CssClass="btn btn-primary mr-4"/>
    </div>
    </div>

    </form>
    </div>
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
          window.open('../credenciales/revisasolicitudcolaboradordocumentos.aspx?numsolicitud=' + caja + '&idsolcol=' + caja2 + '&cedula=' + caja3)
      }
   </script>
</body>
</html>
