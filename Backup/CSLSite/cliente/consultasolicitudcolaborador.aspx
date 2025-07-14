<%@ Page Language="C#" AutoEventWireup="true" Title="Solicitud de Credencial/Permiso Provisional"
CodeBehind="consultasolicitudcolaborador.aspx.cs" Inherits="CSLSite.cliente.consultasolicitudcolaborador" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Solicitud de Credencial/Permiso Provisional</title>
    <link href="../shared/estilo/catalogosolicitudcolaborador.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/work.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/aisv-general.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/tables.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/info.css" rel="stylesheet" type="text/css" />
     <script src="Scripts/jquery-1.7.1.min.js" type="text/javascript"></script>
    <script src="../Scripts/jquery-1.7.1.min.js" type="text/javascript"></script>
    <link href="Inicio/base-site.css" rel="stylesheet" type="text/css" />
    <link href="Inicio/css/font-awesome.min.css" rel="stylesheet" type="text/css" />
    <link rel="shortcut icon" type="image/x-icon" href="favicon.ico" />
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
          <tr><td rowspan="2" class="inum"> <div class="number">1</div></td><td class="level1" >
              Tipo de Solicitud.</td></tr>
          <tr><td class="level2">Tipo de Solicitud.</td></tr>
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
      <tr><td rowspan="2" class="inum"> <div class="number">2</div></td><td class="level1" >Datos Generales del Colaborador(es).</td></tr>
      <tr><td class="level2">Datos Generales del Colaborador(es).</td></tr>
      </table>
     </div>
     <div class="colapser colapsa"></div>
     <div class="accion">
      <div class="informativo" id="colector" style=" height:100%; width:100%; overflow:auto">
      <asp:Repeater ID="tablePagination" runat="server"  >
                 <HeaderTemplate>
                 <table id="tabla"  cellspacing="1" cellpadding="1" class="tabRepeat" style=" font-size:small">
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
                 <th>Licencia</th>
                 <th>Fecha Exp. Licencia</th>
                 <th>Colaborador Rechazado</th>
                 <th>Comentario</th>
                 <th>Tipo</th>
                 <th></th>
                 </tr>
                 </thead> 
                 <tbody>
                 </HeaderTemplate>
                 <ItemTemplate>
                 <tr class="point" >
                  <td style=" display:none; width:1px"><%#Eval("NUMSOLICITUD")%></td>
                  <td style=" display:none; width:1px"><%#Eval("IDSOLCOL")%></td>
                  <td><asp:Label Text='<%#Eval("CIPAS")%>' runat="server" id="lblcipas"/></td>
                  <td><%#Eval("NOMBRE")%></td>
                  <td><%#Eval("TIPOSANGRE")%></td>
                  <td><%#Eval("DIRECCIONDOM")%></td>
                  <td><%#Eval("TELFDOM")%></td>
                  <td><%#Eval("EMAIL")%></td>
                  <td style=" width:80px"><%#Eval("LUGARNAC")%></td>
                  <td style=" width:60px"><%#Eval("FECHANAC")%></td>
                  <td><%#Eval("CARGO")%></td>
                  <td><%#Eval("TIPOLICENCIA")%></td>
                  <td><%#Eval("FECHAEXPLICENCIA")%></td>
                  <td style=" width:60px"><asp:CheckBox runat="server" Enabled="FALSE" ForeColor="Red" id="chkRevisado" Checked='<%#Eval("ESTADOCOL")%>'/></td>
                  <td style=" width:200px" ><asp:TextBox ID="tcomentario" Enabled="FALSE" runat="server" ForeColor="Red" Width="200px" ToolTip='<%#Eval("COMENTARIO")%>' Text='<%#Eval("COMENTARIO")%>' onkeypress="return soloLetras(event,'abcdefghijklnmñopqrstuvwxyz01234567890-_/ ',true)"></asp:TextBox></td>
                  <td style=" width:50px"><%#Eval("TIPO")%></td>
                  <td style=" width:80px" >
                    <a style=" width:80px" id="adjDoc" class="topopup"  onclick="redirectsol('<%# Eval("NUMSOLICITUD") %>', '<%# Eval("IDSOLCOL") %>', '<%#Eval("CIPAS")%>');">
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
     <div class="botonera" runat="server" id="salir">
        <asp:Button ID="btnSalir" runat="server" Text="Salir" Width="125px"
                onclick="btnSalir_Click" 
                ToolTip="Regresa a la Pantalla Consultar Solicitud."/>
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
       function setObject(row) {
            self.close();
        }
        function prepareObject() {
            try {
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
          window.open('../consulta-documentos-colaborador/?numsolicitud=' + caja + '&idsolcol=' + caja2 + '&cedula=' + caja3)
      }
   </script>
</body>
</html>
