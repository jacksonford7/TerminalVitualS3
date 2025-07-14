<%@ Page Language="C#" AutoEventWireup="true" Title="Emisión/Renovación de Credencial"
CodeBehind="revisasolicitudpermisoprovisional.aspx.cs" Inherits="CSLSite.revisasolicitudpermisoprovisional" %>
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
     <style>
     * input[type=text]
        {
            text-align:left!important;
        }
         .style1
         {
             width: 155px;
             height: 254px;
         }
         .style2
         {
             height: 254px;
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
    <div class="cataresult" >
     <div class="seccion">
       <div class="informativo">
          <table class="controles" cellspacing="0" cellpadding="1">
          <tr><td rowspan="2" class="inum"> <div class="number">1</div></td><td class="level1" >
              Datos del Permiso Peatonal Provisional.</td></tr>
          <tr><td class="level2">Tipo de Credencial.</td></tr>
          </table>
         </div>
       <div class="colapser colapsa"></div>
       <div class="accion">
         <table class="controles" style=" font-size:small" cellspacing="0" cellpadding="1">
            <tr>
            <td class="bt-bottom bt-right bt-left" style=" width:200px;" >Empresa que solicita el permiso:</td>
            <td class="bt-bottom bt-right">
            <asp:TextBox ID="txttipcli" runat="server" Width="99%" MaxLength="500" Enabled="false"
             style="text-align: center;"
             onkeypress="return soloLetras(event,'abcdefghijklnmñopqrstuvwxyz01234567890 ',true)"></asp:TextBox>
             </td>
            </tr>
            <tr>
            <td class="bt-bottom bt-right bt-left" style=" width:200px;" >Area Destino:</td>
            <td class="bt-bottom bt-right">
            <asp:TextBox ID="txtarea" runat="server" Width="99%" MaxLength="500" Enabled="false"
             style="text-align: center;"
             onkeypress="return soloLetras(event,'abcdefghijklnmñopqrstuvwxyz01234567890 ',true)"></asp:TextBox>
             </td>
            </tr>
            <tr>
            <td class="bt-bottom bt-right bt-left" style=" width:200px;" >Usuario que solicita el permiso:</td>
            <td class="bt-bottom bt-right">
            <asp:TextBox ID="txtusuariosolper" runat="server" Width="99%" MaxLength="500" Enabled="false"
             style="text-align: center;"
             onkeypress="return soloLetras(event,'abcdefghijklnmñopqrstuvwxyz01234567890 ',true)"></asp:TextBox>
             </td>
            </tr>
            <tr>
            <td class="bt-bottom bt-right bt-left" style=" width:200px;" >Actividad permitida:</td>
            <td class="bt-bottom bt-right">
            <asp:TextBox ID="txtactper" runat="server" Width="99%" MaxLength="500" Enabled="false"
             style="text-align: center;"
             onkeypress="return soloLetras(event,'abcdefghijklnmñopqrstuvwxyz01234567890 ',true)"></asp:TextBox>
             </td>
            </tr>
            <tr ><td class="bt-bottom  bt-right bt-left" style=" width:155px">Fecha de Ingreso:</td>
         <td class="bt-bottom bt-right">
             <asp:TextBox 
             style="text-align: center" 
             ID="txtfecing" runat="server"  width="200px" MaxLength="10" CssClass="datetimepicker" ClientIDMode="Static"
             onkeypress="return soloLetras(event,'0123456789/')"  Enabled="true"
             ></asp:TextBox>
         </td>
         </tr>
      <tr ><td class="bt-bottom  bt-right bt-left" style=" width:155px">Fecha de Caducidad:</td>
         <td class="bt-bottom bt-right">
             <asp:TextBox 
             style="text-align: center" 
             ID="txtfecsal" runat="server"  width="200px" MaxLength="10" CssClass="datetimepicker" ClientIDMode="Static"
             onkeypress="return soloLetras(event,'0123456789/')"  Enabled="true"
             ></asp:TextBox>
         </td>
         </tr>
         <tr>
        <td class="bt-bottom bt-right bt-left" >Turno:</td>
        <td class="bt-bottom bt-right ">
           <asp:DropDownList runat="server" Width="500px" ID="ddlTurnoOnlyControl" onchange="valdltipsolhorario(this, valturno);">
                 <asp:ListItem Value="0">* Elija *</asp:ListItem>
           </asp:DropDownList>
           <span id='valturno' class="validacion" > * obligatorio</span>
        </td>
        <%--<td class="bt-bottom bt-right validacion "></td>--%>
        </tr>
         </table>
       </div>
       </div>
     <div class="seccion">
      <div class="informativo">
      <table>
      <tr><td rowspan="2" class="inum"> <div class="number">2</div></td><td class="level1" >
          Personal resgistrado en permiso provisional.</td></tr>
      <tr><td class="level2">
         Confirme que los datos sean correctos.
      </td></tr>
      </table>
     </div>
     <div class="colapser colapsa"></div>
     <div class="accion">
      <div class="informativo" id="colector" style=" height:100%; overflow:auto">
      <div class="msg-alerta" id="alerta" runat="server"></div>
      <asp:Repeater ID="tablePagination" runat="server"  >
                 <HeaderTemplate>
                 <table id="tablar2"  cellspacing="1" cellpadding="1" class="tabRepeat" style=" font-size:small">
                 <thead>
                 <tr>
                 <th style=" display:none"></th>
                 <th style=" display:none"></th>
                 <th>CI/Pasaporte</th>
                 <th>Nombre</th>
                 <th>Tipo Sangre</th>
                 <th>Dirección Domiciliaria</th>
                 <th>Telefono</th>
                 <th>Email</th>
                 <th>Lugar Nacimiento</th>
                 <th>Fecha Nacimiento</th>
                 <th>Cargo</th>
                 <th style=" display:none;">Area</th>
                 <th>Colaborador Rechazado</th>
                 <th>Comentario</th>
                 <th></th>
                 <th style=" display:none"></th>
                 </tr>
                 </thead> 
                 <tbody>
                 </HeaderTemplate>
                 <ItemTemplate>
                 <tr class="point" >
                  <td style=" display:none; width:1px"><%#Eval("NUMSOLICITUD")%></td>
                  <td style=" display:none; width:1px"><%#Eval("IDSOLCOL")%></td>
                  <td><asp:Label Text='<%#Eval("CIPAS")%>' runat="server" id="lblcipas"/></td>
                  <td><asp:Label Text='<%#Eval("NOMBRE")%>' runat="server" id="lblNombres"/></td>
                  <td><%#Eval("TIPOSANGRE")%></td>
                  <td><%#Eval("DIRECCIONDOM")%></td>
                  <td><%#Eval("TELFDOM")%></td>
                  <td><%#Eval("EMAIL")%></td>
                  <td style=" width:80px"><%#Eval("LUGARNAC")%></td>
                  <td style=" width:60px"><%#Eval("FECHANAC")%></td>
                  <td><%#Eval("CARGO")%></td>
                  <td style=" display:none;"><%#Eval("AREA")%></td>
                  <td style=" width:60px"><asp:CheckBox runat="server" id="chkRevisado" Enabled="false"/></td>
                  <td style=" width:200px"><asp:TextBox ID="tcomentario" runat="server" Width="200px" Text='' onkeypress="return soloLetras(event,'abcdefghijklnmñopqrstuvwxyz01234567890-_/ ',true)"></asp:TextBox></td>
                  <td style=" width:80px">
                    <a style=" width:80px" id="adjDoc" class="topopup"  onclick="redirectsol('<%# Eval("NUMSOLICITUD") %>', '<%#Eval("CIPAS")%>', '<%#Eval("IDSOLCOL")%>');">
                    <i></i> Ver Documentos </a>
                  </td>
                  <td style=" display:none"><asp:TextBox ID="txtcedula" runat="server" Text='<%#Eval("CIPAS")%>' onkeypress="return soloLetras(event,'abcdefghijklnmñopqrstuvwxyz01234567890-_/ ',true)"></asp:TextBox></td>
                 </tr>
                 </ItemTemplate>
                 <FooterTemplate>
                 </tbody>
                 </table>
                 </FooterTemplate>
         </asp:Repeater>
       </div>
     </div>
     <%--<table rules='all'>
     <tr><th align='center' style='width:500px'>Colaborador</th><th align='center' style='width:200px'>Codigo de Acceso</th></tr>
	 <tr><th align='center' style='width:500px'>A</th><th align='center'>B</th></tr>
	 </table>--%>
     <div class="accion" id="factura" runat="server" style=" display:none" >
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
     </div>
        <div class="botonera" runat="server" id="botonera">
                <asp:Button Text="Rechazar" ID="btnRechazar" runat="server" Width="150px" 
                onclick="btnRechazar_Click" OnClientClick="return prepareObjectRechazar('¿Esta seguro de rechazar la solicitud?');"
                ToolTip="Rechaza la solicitud."/>
                <img alt="loading.." src="../shared/imgs/loader.gif" id="loader" class="nover"  />
                <asp:Button ID="btsalvar" runat="server" Text="Crear Permiso" Width="150px"
                OnClientClick="return prepareObject('¿Esta seguro de procesar la solicitud?');" onclick="btsalvar_Click" 
                ToolTip="Crea el permiso provisional."/>
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
    <script type="text/javascript" >
        var ced_count = 0;
        var jAisv = {};
        $(document).ready(function () {
            $('.datetimepicker').datetimepicker().datetimepicker({ lang: 'es', timepicker: false, closeOnDateSelect: true, format: 'd/m/Y' });
        });
        $(window).load(function () {
            $(document).ready(function () {
                //colapsar y expandir
                $("div.colapser").toggle(function () { $(this).removeClass("colapsa").addClass("expande"); $(this).next().hide(); }
                                  , function () { $(this).removeClass("expande").addClass("colapsa"); $(this).next().show(); });
            });
        });
       function setObject(row) {
            self.close();
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
                        if (celColect2[15].getElementsByTagName('input')[0].value == objeto.cedula) {
                            celColect2[12].getElementsByTagName('input')[0].checked = true;
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
                        if (celColect2[15].getElementsByTagName('input')[0].value == objeto.cedula) {
                            celColect2[12].getElementsByTagName('input')[0].checked = false;
                        }
                    }
                }
            }
        }
        function prepareObject(valor) {
            try {
                if (confirm(valor) == false) {
                    return false;
                }
                var vals = document.getElementById('<%=ddlTurnoOnlyControl.ClientID %>').value;
                if (vals == 0) {
                    alert('*Seleccione el Turno *');
                    document.getElementById('<%=ddlTurnoOnlyControl.ClientID %>').focus();
                    document.getElementById('<%=ddlTurnoOnlyControl.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:400px;";
                    //                    document.getElementById("loader").className = 'nover';
                    var tbl2 = document.getElementById('tablar2');
                    for (var r = 0; r < tbl2.rows.length; r++) {
                        var celColect2 = tbl2.rows[r].getElementsByTagName('td');
                        if (celColect2 != undefined && celColect2 != null && celColect2.length > 0) {
                            celColect2[12].getElementsByTagName('input')[0].disabled = false;
                        }
                    }
                    return false;
                }
                return true;
            } catch (e) {
                alert('Lamentamos comunicarle que ha ocurrido el siguiente problema:\n' + e.Message);
            }
        }
        function prepareObjectRechazar(valor) {
            try {
                if (confirm(valor) == false) {
                    return false;
                }
                return true;
            } catch (e) {
                alert('Lamentamos comunicarle que ha ocurrido el siguiente problema:\n' + e.Message);
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
          window.open('../consulta/documentos-solicitud-permiso-provisional/?numsolicitud=' + caja + '&idsolcol=' + caja3 + '&cedula=' + caja2)
      }
   </script>
    <script src="../Scripts/jquery-1.7.1.min.js" type="text/javascript"></script>
    <script src="../Scripts/pages.js" type="text/javascript"></script>
    <script src="../../Scripts/credenciales.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.tablesorter.min.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.tablesorter.pager.js" type="text/javascript"></script>
</body>
</html>
