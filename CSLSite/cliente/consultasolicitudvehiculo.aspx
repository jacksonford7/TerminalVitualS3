<%@ Page Language="C#" AutoEventWireup="true" Title="Solicitud de Sticker/Provisional Vehícular"
CodeBehind="consultasolicitudvehiculo.aspx.cs" Inherits="CSLSite.cliente.consultasolicitudvehiculo" %>
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

    <%--<title>Solicitud de Sticker/Provisional Vehícular</title>
    <link href="../shared/estilo/catalogosolicitudvehiculo.css" rel="stylesheet" type="text/css" />
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
        #tablerp
        {
            width: 726px;
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
             <a class="btn btn-outline-primary mr-4" href="#" runat="server" id="a2" clientidmode="Static" >2</a>
             <a class="level1" runat="server" id="a3" clientidmode="Static" >Datos del Vehículo(s)</a>
         </div>

      <div class="informativo" id="colector" style=" overflow:auto">
      <asp:Repeater ID="tablePagination" runat="server" >
                 <HeaderTemplate>
                 <table id="tabla"  cellspacing="1" cellpadding="1" class="table table-bordered invoice" style=" font-size:small">
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
                 <th>Categoria</th>
                 <th>Área Destino/Actividad</th>
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
                  <td class="nover"><%#Eval("IDSOLVEH")%></td>
                  <%--<td class="nover"><%#Eval("TIPOSOLICITUD")%></td>--%>
                  <td><asp:Label Text='<%#Eval("PLACA")%>' runat="server" ID="lblplaca"></asp:Label></td>
                  <td><%#Eval("CLASETIPO")%></td>
                  <td><%#Eval("MARCA")%></td>
                  <td><%#Eval("MODELO")%></td>
                  <td><%#Eval("COLOR")%></td>                  
                  <td><%#Eval("DESCRIPCIONCATEGORIA")%></td>
                  <td><%#Eval("AREA")%></td>
                  <td><%#Eval("TIPOCERTIFICADO")%></td>
                  <td><%#Eval("CERTIFICADO")%></td>
                  <td ><%#Eval("FECHAPOLIZA")%></td>
                  <td ><%#Eval("FECHAMTOP")%></td>
                  <td ><asp:CheckBox runat="server" Checked='<%#Eval("ESTADOCOL")%>' Enabled="false" id="chkRevisado"/></td>
                  <td ><asp:TextBox ID="tcomentario" Enabled="false" Text='<%#Eval("COMENTARIO")%>' runat="server" onkeypress="return soloLetras(event,'abcdefghijklnmñopqrstuvwxyz01234567890-_/ ',true)"></asp:TextBox></td>
                  <td >
                    <a id="adjDoc" class="btn btn-outline-primary mr-4" onclick="redirectsol('<%# Eval("NUMSOLICITUD") %>', '<%# Eval("IDSOLVEH") %>', '<%#Eval("PLACA")%>');">
                    <i class="fa fa-search"></i> Ver Documentos </a>
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
        function prepareObject(valor) {
            try {
                if (confirm(valor) == true)
                    return true;
                else
                    return false;
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
            //window.open('../credenciales/documentos/?placa=' + caja, 'name', 'width=850,height=480')
            window.open('../cliente/consultasolicitudvehiculodocumentos.aspx?numsolicitud=' + caja + '&idsolveh=' + caja2 + '&placa=' + caja3)
        }
   </script>
</body>
</html>
