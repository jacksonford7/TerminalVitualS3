<%@ Page Language="C#" AutoEventWireup="true" Title="Documentos Solicitud de Credencial/Permiso Provisional"
CodeBehind="solicitudcolaboradordocumentos.aspx.cs" Inherits="CSLSite.cliente.solicitudcolaboradordocumentos" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Catálogo de agentes</title>
    <link href="../img/favicon2.png" rel="icon"/>
  <link href="../img/icono.png" rel="apple-touch-icon"/>
  <link href="../css/bootstrap.min.css" rel="stylesheet"/>
  <link href="../css/dashboard.css" rel="stylesheet"/>
  <link href="../css/icons.css" rel="stylesheet"/>
  <link href="../css/style.css" rel="stylesheet"/>
  <link href="https://fonts.googleapis.com/css2?family=Roboto:wght@300;400;500;700&display=swap" rel="stylesheet"/>

      <link href="../shared/estilo/Reset.css" rel="stylesheet" />
 
     <link href="../shared/estilo/tables.css" rel="stylesheet" type="text/css" />
     <link href="../shared/estilo/info.css" rel="stylesheet" type="text/css" />

    <%--<link href="../shared/estilo/catalogodocumentos.css" rel="stylesheet" type="text/css" />
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
<body onbeforeunload="Quit()";>
     <noscript><meta http-equiv="refresh" content="0; url=sinsoporte.htm" /> </noscript>
    <form id="bookingfrm" runat="server">
    <asp:ToolkitScriptManager ID="tkscata" runat="server"></asp:ToolkitScriptManager>
    <div class="catabody">
       <div class="catawrap" >
         <div class="cataresult" >
             <div id="xfinder" runat="server" visible="false" >
             <div class="alert alert-warning" runat="server" id="alerta" >
                            Confirme que los documentos sean los correctos.
             </div>
             <div class="findresult" >
             <div >
                  <div class="separator">Adjuntar los documentos solicitados:</div>
                 <div class="bokindetalle" style=" overflow:auto; height:450px;">
         <table id="tablerp" cellpadding="1" cellspacing="0">
      <asp:Repeater ID="tablePaginationDocumentos" runat="server">
            <HeaderTemplate>
            <table id="tablar"  cellspacing="1" cellpadding="1" class="tabRepeat">
            <thead>
            <tr>
            <th>Tipo Cliente</th>
            <th class="nover"></th>
            <th class="nover"></th>
            <th>Documentos</th>
            <th>Escoja el archivo con formato indicado.</th>
            <th></th>
            <th>Formato</th>
            </tr>
            </thead>
            <tbody>
            </HeaderTemplate>
            <ItemTemplate>
            <tr class="point">
            <td style=" width:150px"><%#Eval("DESCRIPCION")%></td>
            <td class="nover"><asp:TextBox ID="txtidsolicitud" runat="server" Text='<%#Eval("IDTIPSOL")%>' Width="5px"/></td>
            <td class="nover"><asp:TextBox ID="txtiddocemp" runat="server" Text='<%#Eval("IDDOCEMP")%>' Width="5px"/></td>
            <td style=" font-size:inherit"><%#Eval("DOCUMENTO")%></td>
            <td>
                <asp:FileUpload extension='<%#Eval("EXTENSION")%>' id="fsupload" class="btn btn-outline-primary mr-4" title="Escoja el archivo con formato indicado en la siguiente columna."
                       onchange="validaextension(this)" style=" font-size:small;" runat="server"/>
                       </td>
            </td>
            <td>
                <a  class="btn btn-outline-primary mr-4"  onclick="abrirVentana('<%#Eval("RUTADOCUMENTO") %>')" style=" target="_blank">
                <i class="fa fa-search"></i> Ver Documento </a>
            </td>
            <td><%#Eval("EXTENSION")%></td>
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
              <div id="sinresultado" runat="server" class="alert alert-warning" >
              No se encontraron resultados, 
              asegurese que ha seleccionado correctamente un tipo de solicitud.
              </div>
              <div class="btn btn-outline-primary mr-4">
              <img alt="loading.." src="../shared/imgs/loader.gif" id="loader" class="nover"  />
                <asp:Button ID="btsalvar" runat="server" Text="Regresar" 
                               OnClientClick="return prepareObject();" onclick="btsalvar_Click" 
                               ToolTip="Regresa a la pantalla Emisión/Renovación de Credencial." Width="122px"/>
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
       function setObject(row) {
//           var celColect = row.getElementsByTagName('td');
//          var bookin = {
//              fila: celColect[0].textContent,
//              gkey: celColect[1].textContent,
//              nbr: celColect[2].textContent,
//              linea: celColect[3].textContent,
//              fk: celColect[4].textContent
//              };
//            if (window.opener != null) {
//                window.opener.popupCallback(bookin, 'bk');
//            }
            self.close();
        }
        function abrirVentana(url) {
            var newurl = "../cliente/" + url;
            if (url == '' || url == null || url == undefined) {
                window.open("", "nuevo", "directories=no, location=no, menubar=no, scrollbars=yes, statusbar=no, tittlebar=no, width=400, height=400");
            }
            else {
                window.open(newurl, "nuevo", "directories=no, location=no, menubar=no, scrollbars=yes, statusbar=no, tittlebar=no, width=400, height=400");
            }
        }
        function prepareObject() {
            try {
                //Valida Documentos
                lista = [];
                var vals = document.getElementById('tablar');
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
                    var nomdoc = null;
                    for (var n = 0; n < this.lista.length; n++) {
                        if (lista[n].documento == '' || lista[n].documento == null || lista[n].documento == undefined) {
                            alert('* Seleccione todos los documentos requeridos *');
                            document.getElementById("loader").className = 'nover';
                            return false;
                        }
                        if (nomdoc == lista[n].documento) {
                            alert('* Existen archivos repetidos, revise por favor *');
                            document.getElementById("loader").className = 'nover';
                            return false;
                        }
                        nomdoc = lista[n].documento;
                    }
                }
                document.getElementById("loader").className = '';
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
//          window.open('../credenciales/solicituddocumentos/?placa=' + caja, 'name', 'width=950,height=480')
      }
   </script>
    <script src="../Scripts/jquery-1.7.1.min.js" type="text/javascript"></script>
    <script src="../Scripts/pages.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.tablesorter.min.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.tablesorter.pager.js" type="text/javascript"></script>
</body>
</html>
