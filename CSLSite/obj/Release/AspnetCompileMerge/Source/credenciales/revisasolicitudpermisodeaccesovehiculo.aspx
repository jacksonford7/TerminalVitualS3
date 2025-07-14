<%@ Page Language="C#" AutoEventWireup="true" Title="Solicitud de Permiso de Acceso"
CodeBehind="revisasolicitudpermisodeaccesovehiculo.aspx.cs" Inherits="CSLSite.revisasolicitudpermisodeaccesovehiculo" %>

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

<%--      <link href="../shared/estilo/Reset.css" rel="stylesheet" />
 
     <link href="../shared/estilo/tables.css" rel="stylesheet" type="text/css" />
     <link href="../shared/estilo/info.css" rel="stylesheet" type="text/css" />--%>

    <link href="../shared/estilo/jquery.datetimepicker.css" rel="stylesheet" type="text/css" />
   <script src="../Scripts/jquery.datetimepicker.js" type="text/javascript"></script>

    <!--external css-->
  <link href="../lib/font-awesome/css/font-awesome.css" rel="stylesheet" />
  <link rel="stylesheet" type="text/css" href="../lib/gritter/css/jquery.gritter.css" />

    <!--mensajes-->
  <link href="https://cdn.jsdelivr.net/npm/alertifyjs@1.11.0/build/css/alertify.min.css" rel="stylesheet"/>

<%--<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
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
<body>--%>
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

             <div class="form-group col-md-6" >
                 <label for="inputAddress" Class="form-control">Tipo de Solicitud:<span style="color: #FF0000; font-weight: bold;"> *</span></label>
            </div>
            <div class="form-group col-md-6">  
                    <asp:TextBox ID="txttipcli" runat="server" MaxLength="500" Enabled="false" style="text-align: center;"
             onkeypress="return soloLetras(event,'abcdefghijklnmñopqrstuvwxyz01234567890 ',true)" CssClass="form-control"></asp:TextBox>
            </div>

        </div>


          <div class="form-row">
        <div class="form-group col-md-12">
             <a class="btn btn-outline-primary mr-4" href="#" target="_blank" runat="server" id="a2" clientidmode="Static" >2</a>
             <a class="level1" runat="server" id="a3" clientidmode="Static" >Datos Generales de los Permisos de Acceso</a>
         </div>

     <div class="alert alert-danger" id="alerta" runat="server" ></div>
     <div class="accion">
     <div style="overflow:auto">
    <asp:Repeater ID="tablePagination" runat="server"  >
                <HeaderTemplate>
                <table id="tableRpt"  cellspacing="1" cellpadding="1" class="table table-bordered invoice" style=" font-size:small;">
                <tr style="position:static">
                 <th style=" display:none"></th>
                 <th>Placa</th>
                 <th>Cedula Conductor Designado</th>
                 <th>Nombres Conductor Designado</th>
                 <th>Apellidos Conductor Designado</th>
                 <th>Área</th>
                 <th>Actividad</th>
                 <th>Cargo</th>
                 <th>Fecha Ingreso</th>
                 <th>Fecha Caducidad</th>
                 <th>Cambiar Permiso</th>
                 <th>Fecha Ingreso</th>
                 <th>Fecha Caducidad</th>
                 <th>Rechazado</th>
                 <th>Comentario</th>
                 </tr>
                 </HeaderTemplate>
                 <ItemTemplate>
                 <tr >
                  <td style=" display:none;"><%#Eval("IDSOLICITUD")%></td>
                  <td ><asp:Label Text='<%#Eval("PLACA")%>' runat="server" id="lblPlaca"/></td>
                  <td ><asp:Label Text='<%#Eval("CICONDES")%>' runat="server" id="lblcipas"/></td>
                  <td ><asp:Label Text='<%#Eval("NOMBRESCONDES")%>' runat="server" id="lblnombres"/></td>
                  <td ><asp:Label Text='<%#Eval("APELLIDOSCONDES")%>' runat="server" id="lblapellidos"/></td>
                  <td ><%#Eval("AREA")%></td>
                  <td ><%#Eval("ACTIVIDAD")%></td>
                  <td ><%#Eval("CARGO")%></td>
                  <td ><asp:Label Text='<%#Eval("FECHAINGRESO")%>'  runat="server" id="lblfecing"/></td>
                  <td ><asp:Label Text='<%#Eval("FECHACADUCIDAD")%>'  runat="server" id="lblfeccad"/></td>
                  <td ><asp:CheckBox runat="server"  Width="50px" id="chkPermiso" onchange="validaPermiso();"/></td>
                  <td >
                                    <asp:TextBox Style="text-align: center" ID="txtfecing" runat="server" Enabled="false" BackColor="Gray" 
                                    MaxLength="15" CssClass="datetimepicker" ClientIDMode="Static" onkeypress="return soloLetras(event,'0123456789/')"
                                    ></asp:TextBox>
                  </td>
                  <td>
                                <asp:TextBox Style="text-align: center" ID="txtfecsal" runat="server" Enabled="false" BackColor="Gray" 
                                    MaxLength="15" CssClass="datetimepicker" ClientIDMode="Static" onkeypress="return soloLetras(event,'0123456789/')"
                                ></asp:TextBox>
                  </td>  
                  <td ><asp:CheckBox runat="server"  Checked='<%#Eval("ESTADO")%>' onchange="valRechazado()" id="chkRevisado" ForeColor="Red"/></td>
                  <td ><asp:TextBox ID="tcomentario" ForeColor="Red" runat="server" Text='<%#Eval("COMENTARIO")%>' onkeypress="return soloLetras(event,'abcdefghijklnmñopqrstuvwxyz01234567890-_/ ',true)"></asp:TextBox></td>
                 </tr>
                 </ItemTemplate>
                 <FooterTemplate>
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

       <div class="col-md-12 d-flex justify-content-rigth" id="divfac" runat="server">
      <div class="alert alert-warning">Adjunte el archivo en formato PDF</div>
           </div>
           <div class="col-md-12 d-flex justify-content-rigth" id="div1" runat="server">
         <span class="bt-bottom bt-top  bt-right bt-left" >Adjuntar permiso:</span>

         <asp:FileUpload runat="server" ID="fuPDF" Width="100%" extension='.pdf' class="btn btn-outline-primary mr-4" title="Escoja el archivo en formato PDF." onchange="validaextension(this)"/>

       </div>

     <div class="col-md-12 d-flex justify-content-rigth" runat="server" id="salir" visible="false">
        <asp:Button ID="btnSalir" runat="server" Text="Salir" onclick="btnSalir_Click" 
                ToolTip="Regresa a la Pantalla Consultar Solicitud." CssClass="btn btn-outline-primary mr-4"/>
    </div>
     <div class="col-md-12 d-flex justify-content-rigth" runat="server" id="botonera">

        <asp:Button Text="Rechazar" ID="btnRechazar" runat="server" onclick="btnRechazar_Click" CssClass="btn btn-primary mr-4"
            OnClientClick="return prepareObjectRechazar('¿Esta seguro de rechazar la solicitud?');" ToolTip="Rechaza la solicitud."/>

        <asp:Button ID="btsalvar" runat="server" Text="Crear Permiso(s)" onclick="btsalvar_Click" CssClass="btn btn-primary mr-4"
                OnClientClick="return prepareObject();" ToolTip="Crear el Permiso de Acceso."/>
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
                vals = document.getElementById('fuPDF').files.length;
                if (vals == 0) {
                    alert('Adjunte el permiso');
                    document.getElementById('<%=fuPDF.ClientID %>').focus();
                    document.getElementById('<%=fuPDF.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:100%;";
                    return false;
                }
                lista = [];
                var tbl = document.getElementById('tableRpt');
                for (var f = 0; f < tbl.rows.length; f++) {
                    var celColect = tbl.rows[f].getElementsByTagName('td');
                    if (celColect != undefined && celColect != null && celColect.length > 0) {
                        var vals = celColect[10].getElementsByTagName('input')[0].checked;
                        if (vals == true) {
                            var vals = celColect[11].getElementsByTagName('input')[0].value;
                            if (vals == '' || vals == null || vals == undefined) {
                                alert('Seleccione la Fecha de Ingreso');
                                celColect[11].getElementsByTagName('input')[0].focus();
                                celColect[11].getElementsByTagName('input')[0].style.cssText = "background-color:#ffffc6;color:Red;width:80px;";
                                return false;
                            }
                            var vals = celColect[12].getElementsByTagName('input')[0].value;
                            if (vals == '' || vals == null || vals == undefined) {
                                alert('Seleccione la Fecha de Caducidad');
                                celColect[12].getElementsByTagName('input')[0].focus();
                                celColect[12].getElementsByTagName('input')[0].style.cssText = "background-color:#ffffc6;color:Red;width:80px;";
                                return false;
                            }
                        }
                        var vals = celColect[13].getElementsByTagName('input')[0].checked;
                        if (vals == true) {
                            var vals = celColect[14].getElementsByTagName('input')[0].value;
                            if (vals == '' || vals == null || vals == undefined) {
                                alert('Escriba el comentario de rechazo');
                                celColect[18].getElementsByTagName('input')[0].focus();
                                celColect[18].getElementsByTagName('input')[0].style.cssText = "background-color:#ffffc6;color:Red;width:80px;";
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
        function validaPermiso() {
            lista = [];
            var tbl = document.getElementById('tableRpt');
            for (var f = 0; f < tbl.rows.length; f++) {
                var celColect = tbl.rows[f].getElementsByTagName('td');
                if (celColect != undefined && celColect != null && celColect.length > 0) {
                    if (celColect[10].getElementsByTagName('input')[0].checked == true) {
                        celColect[13].getElementsByTagName('input')[0].checked = false;
                        celColect[11].getElementsByTagName('input')[0].style.cssText = "background-color:White;width:80px;"; //fecing
                        celColect[12].getElementsByTagName('input')[0].style.cssText = "background-color:White;width:80px;"; //fecsal
                        celColect[11].getElementsByTagName('input')[0].disabled = false;
                        celColect[12].getElementsByTagName('input')[0].disabled = false;
                    }
                    else {
                        celColect[11].getElementsByTagName('input')[0].style.cssText = "background-color:Gray;width:80px;"; //fecing
                        celColect[12].getElementsByTagName('input')[0].style.cssText = "background-color:Gray;width:80px;"; //fecsal
                        celColect[11].getElementsByTagName('input')[0].disabled = true;
                        celColect[12].getElementsByTagName('input')[0].disabled = true;
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
                    if (celColect[13].getElementsByTagName('input')[0].checked == true) {
                        celColect[10].getElementsByTagName('input')[0].checked = false
                        celColect[11].getElementsByTagName('input')[0].style.cssText = "background-color:Gray;width:80px;"; //fecing
                        celColect[12].getElementsByTagName('input')[0].style.cssText = "background-color:Gray;width:80px;"; //fecsal
                        celColect[11].getElementsByTagName('input')[0].disabled = true;
                        celColect[12].getElementsByTagName('input')[0].disabled = true;
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
            window.open('../credenciales/revisasolicitudcolaboradordocumentos.aspx?numsolicitud=' + caja + '&idsolcol=' + caja2 + '&cedula=' + caja3)
        }
   </script>
</body>
</html>
