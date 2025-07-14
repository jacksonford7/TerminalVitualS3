<%@ Page Language="C#" AutoEventWireup="true" Title="Documentos Solicitud de Sticker/Provisional Vehícular"
CodeBehind="consultasolicitudvehiculodocumentos.aspx.cs" Inherits="CSLSite.cliente.consultasolicitudvehiculodocumentos" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

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

    <%--<title>Catálogo de agentes</title>
    <link href="../shared/estilo/catalogodocumentos.css" rel="stylesheet" type="text/css" />
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
    <asp:ToolkitScriptManager ID="tkscata" runat="server"></asp:ToolkitScriptManager>

    <div class="catabody">
       <div class="catawrap" >
         <div class="cataresult" >
       <div id="xfinder" runat="server">
       <div class="findresult" >
             <div class="alert alert-warning" runat="server" id="alerta" >
                            Confirme que los documentos sean los correctos.
             </div>
        <%--<div class="booking">--%>
       <%--<div class="separator">Revisar los documentos solicitados:</div>--%>
        <div class="informativo" style=" width:100%; overflow:auto">
         <table id="tablerp" cellpadding="1" cellspacing="0">
            <asp:Repeater ID="tablePaginationDocumentos" runat="server">
            <HeaderTemplate>
            <table id="tablar"  cellspacing="1" cellpadding="1" class="table table-bordered invoice">
            <thead>
            <tr>
            <th>Tipo Cliente</th>
            <th>Documentos</th>
            <th></th>
            <th>Documento Rechazado</th>
            <th>Comentario</th>
            </tr>
            </thead> 
            <tbody>
            </HeaderTemplate>
            <ItemTemplate>
            <tr class="point">
            <td ><%#Eval("TIPOSOLICITUD")%></td>
            <td style=" font-size:inherit"><%#Eval("DOCUMENTO")%></td>
            <td >
                <%--<asp:Button Text="Ver Documento" Width="100px" runat="server" onclick="verDocumento(5);"/>--%>
                <%--<asp:LinkButton Text="Ver Documento" Width="100px" ID="lbVerDoc" runat="server" CommandName="ViewPDF" CommandArgument='<%#Eval("RUTADOCUMENTO") %>'/>--%>
                <a href='<%#Eval("RUTADOCUMENTO") %>' class="btn btn-outline-primary mr-4" target="_blank">
                    <i class="fa fa-search"></i> Ver Documento </a>
            </td>
            <td ><asp:CheckBox Enabled="false" Checked='<%#Eval("ESTADOCOL")%>' ForeColor="Red" runat="server" id="chkRevisado"/></td>
            <td><asp:TextBox ID="tcomentario" Enabled="false" Text='<%#Eval("COMENTARIO")%>' ForeColor="Red" runat="server" onkeypress="return soloLetras(event,'abcdefghijklnmñopqrstuvwxyz01234567890-_/ ',true)"></asp:TextBox></td>
            </tr>
            </ItemTemplate>
            <FooterTemplate>
            </tbody>
            </table>
            </FooterTemplate>
            </asp:Repeater>
        </table>
        </div>
        <%--</div>--%>
        </div>
              <div class="col-md-12 d-flex justify-content-rigth" runat="server" id="salir">
        <asp:Button ID="btnSalir" runat="server" Text="Regresar" onclick="btnSalir_Click" 
                ToolTip="Regresa a la Pantalla Consultar Solicitud." CssClass="btn btn-outline-primary mr-4"/>
    </div>        
        </div>
      </div>
      </div>
    </form>
    </div>

   <script src="../Scripts/pages.js" type="text/javascript"></script>
   <script src="../Scripts/credenciales.js" type="text/javascript"></script>
   <script src="../Scripts/jquery-1.7.1.min.js" type="text/javascript"></script>
   <script src="../Scripts/jquery.tablesorter.min.js" type="text/javascript"></script>
   <script src="../Scripts/jquery.tablesorter.pager.js" type="text/javascript"></script>
   <script type="text/javascript" >
       function setObject(row) {
            self.close();
        }
        function prepareObject() {
            try {
                lista = [];
                var tbl = document.getElementById('tablar');
                for (var f = 0; f < tbl.rows.length; f++) {
                    var celColect = tbl.rows[f].getElementsByTagName('td');
                    if (celColect != undefined && celColect != null && celColect.length > 0) {
                        var tdetalle = {
                            documento: celColect[3].getElementsByTagName('input')[0].checked,
                            comentario: celColect[4].getElementsByTagName('input')[0].value
                        };
                        this.lista.push(tdetalle);
                    }
                }                
                for (var n = 0; n < this.lista.length; n++) {
                    if (lista[n].documento == true) {
                        if (lista[n].comentario == '' || lista[n].comentario == null || lista[n].comentario == undefined) {
                            alert('* Documentos Solicitud de Sticker/Provisional Vehícular: *\n * Escriba el Comentario del documento rechazado.*');
                            return false;
                        }
                    }
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
          //event.returnValue = '[ ! ] Se perdera el trabajo realizado [ ! ]';
          //return;
      }
      function verDocumento(val) {
          var caja = val;
          window.open('../credenciales/solicituddocumentos/?placa=' + caja, 'name', 'width=850,height=480')
      }
      function setComentario(control) {
          var ext = control.getAttribute("extension");
          alert(ext)
      }
   </script>
</body>
</html>
