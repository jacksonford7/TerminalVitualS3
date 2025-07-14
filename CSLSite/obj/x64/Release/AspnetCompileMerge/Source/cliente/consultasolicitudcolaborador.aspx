<%@ Page Language="C#" AutoEventWireup="true" Title="Solicitud de Credencial/Permiso Provisional"
CodeBehind="consultasolicitudcolaborador.aspx.cs" Inherits="CSLSite.cliente.consultasolicitudcolaborador" %>
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
    </style>--%>
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
    <div class="dashboard-container p-4" id="cuerpo" runat="server">
     <noscript><meta http-equiv="refresh" content="0; url=sinsoporte.htm" /> </noscript>
    <form id="bookingfrm" runat="server">
    <input id="zonaid" type="hidden" value="7" />
    <asp:ToolkitScriptManager ID="tkscata" runat="server"></asp:ToolkitScriptManager>

     <div class="form-row">
        <div class="form-title col-md-12">
            <a class="btn btn-outline-primary mr-4" href="#" runat="server" id="aprint" clientidmode="Static" >1</a>
            <a class="level1" runat="server" id="a1" clientidmode="Static" >Tipo de Solicitud</a>
        </div>
        <div class="form-group col-md-12 d-flex">
                <label for="inputAddress">Tipo de Solicitud:</label>
                <asp:TextBox ID="txttipcli" runat="server" MaxLength="500" Enabled="false" style="text-align: center;"
                onkeypress="return soloLetras(event,'abcdefghijklnmñopqrstuvwxyz01234567890 ',true)" CssClass="form-control"></asp:TextBox>
         </div>
    </div>

      <div class="form-row">
            <div class="form-title col-md-12">
                <a class="btn btn-outline-primary mr-4" href="#" runat="server" id="a4" clientidmode="Static" >2</a>
                <a class="level1" runat="server" id="a5" clientidmode="Static" >Datos Generales del Colaborador(es)</a>
            </div>

     
      <div class="informativo" id="colector" style=" width:100%; overflow:auto">
      <asp:Repeater ID="tablePagination" runat="server"  >
                 <HeaderTemplate>
                 <table id="tabla"  cellspacing="1" cellpadding="1" class="table table-bordered invoice" style=" font-size:small">
                 <thead>
                 <tr>
                 <th style=" display:none"></th>
                 <th style=" display:none"></th>
                 <th class="nover"></th>
                 <th>Estado</th>
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
                 <th></th>
                 </tr>
                 </thead> 
                 <tbody>
                 </HeaderTemplate>
                 <ItemTemplate>
                 <tr class="point" >
                  <td style=" display:none; width:1px"><%#Eval("NUMSOLICITUD")%></td>
                  <td style=" display:none; width:1px"><%#Eval("IDSOLCOL")%></td>
                  <td class="nover" ><%#Eval("ESTADO")%></td>
                  <td style="font-size:x-small"><%#Eval("ESTADODESC")%></td>
                  <td><asp:Label Text='<%#Eval("CIPAS")%>' runat="server" id="lblcipas"/></td>
                  <td><%#Eval("NOMBRE")%></td>
                  <td><%#Eval("TIPOSANGRE")%></td>
                  <td><%#Eval("DIRECCIONDOM")%></td>
                  <td><%#Eval("TELFDOM")%></td>
                  <td><%#Eval("EMAIL")%></td>
                  <td ><%#Eval("LUGARNAC")%></td>
                  <td ><%#Eval("FECHANAC")%></td>
                  <td><%#Eval("CARGO")%></td>
                  <td><%#Eval("TIPOLICENCIA")%></td>
                  <td><%#Eval("FECHAEXPLICENCIA")%></td>
                  <td ><asp:CheckBox runat="server" Enabled="FALSE" ForeColor="Red" id="chkRevisado" Checked='<%#Eval("ESTADOCOL")%>'/></td>
                  <td ><asp:TextBox ID="tcomentario" Enabled="FALSE" runat="server" ForeColor="Red" ToolTip='<%#Eval("COMENTARIO")%>' Text='<%#Eval("COMENTARIO")%>' onkeypress="return soloLetras(event,'abcdefghijklnmñopqrstuvwxyz01234567890-_/ ',true)"></asp:TextBox></td>
                  <td ><%#Eval("TIPO")%></td>
                  <td>
                    <a  id="adjDoc" class="btn btn-outline-primary mr-4"  onclick="redirectsol('<%# Eval("NUMSOLICITUD") %>', '<%# Eval("IDSOLCOL") %>', '<%#Eval("CIPAS")%>');">
                    <i class="fa fa-search"></i> Ver Documentos </a>
                  </td>

                  <td>
                    <a  id="adjFotosRFacial" class="btn btn-outline-primary mr-4"  onclick="redirectAddFotos('<%# Eval("NUMSOLICITUD") %>', '<%# Eval("IDSOLCOL") %>', '<%#Eval("CIPAS")%>');">
                    <i class="fa fa-photo"></i> Añadir Fotos </a>
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

     <div class="col-md-12 d-flex justify-content-rigth" runat="server" id="salir">
        <asp:Button ID="btnSalir" runat="server" Text="Regresar" onclick="btnSalir_Click" 
                ToolTip="Regresa a la Pantalla Consultar Solicitud." CssClass="btn btn-outline-primary mr-4"/>
    </div>

    </form>
    </div>
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
          window.open('../cliente/consultasolicitudcolaboradordocumentos.aspx?numsolicitud=' + caja + '&idsolcol=' + caja2 + '&cedula=' + caja3)
       }

        function redirectAddFotos(val, val2, val3) {
          var caja = val;
          var caja2 = val2;
          var caja3 = val3;
          window.open('../cliente/consultasolicitudcolaboradorfacial.aspx?numsolicitud=' + caja + '&idsolcol=' + caja2 + '&cedula=' + caja3)
      }
   </script>
</body>
</html>
