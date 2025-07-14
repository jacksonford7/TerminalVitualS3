<%@ Page Language="C#" AutoEventWireup="true" Title="Documentos Solicitud de Credencial/Permiso Provisional"
CodeBehind="solicitudcolaboradordocumentos.aspx.cs" Inherits="CSLSite.cliente.solicitudcolaboradordocumentos" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Catálogo de agentes</title>
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
<body onbeforeunload="Quit()";>
     <noscript><meta http-equiv="refresh" content="0; url=sinsoporte.htm" /> </noscript>
    <form id="bookingfrm" runat="server">
    <asp:ToolkitScriptManager ID="tkscata" runat="server"></asp:ToolkitScriptManager>
    <div class="catabody">
       <div class="catawrap" >
         <%--<div class="catabuscar">
         <div class="catacapa" style=" display:none"
                         <p class="catalabel" id="agente">Número de Booking:</p>
             <asp:TextBox ID="txtname" runat="server"  CssClass="catamayusc" 
                 onkeypress="return soloLetras(event,'abcdefghijklmnñopqrstuvwxyz1234567890')" 
                 MaxLength="50" Width="30%" ></asp:TextBox>  
             <asp:Button ID="find" runat="server" Text="Buscar" onclick="find_Click"  OnClientClick="return initFinder();"/>
             <span id="imagen"></span>
          </div>
         </div>--%>
         <div class="cataresult" >
            <%-- <asp:UpdatePanel ID="upresult" runat="server"  >
             <ContentTemplate>
             <script type="text/javascript">Sys.Application.add_load(BindFunctions); </script>--%>
             <div id="xfinder" runat="server" visible="false" >
             <div class="msg-alerta" runat="server" id="alerta" >
                            Confirme que los documentos sean los correctos.
             </div>
             <%-- catalogo de bookings--%>
             <div class="findresult" >
             <div >
                  <div class="separator">Adjuntar los documentos solicitados:</div>
                 <div class="bokindetalle" style=" overflow:auto; height:280px;">
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
            <td style=" width:350px; font-size:inherit"><%#Eval("DOCUMENTO")%></td>
            <td>
                <%--<input extension='<%#Eval("EXTENSION")%>' class="uploader" id="fsupload" title="Escoja el archivo con formato indicado en la siguiente columna."
                       onchange="validaextension(this)" type="file"  runat="server" clientidmode="Static" />--%>
                <asp:FileUpload extension='<%#Eval("EXTENSION")%>' id="fsupload"  title="Escoja el archivo con formato indicado en la siguiente columna."
                       onchange="validaextension(this)" style=" font-size:small; width:300px" runat="server"/>
                       </td>
                <%--<input class="uploader" id="File1" title="Escoja el archivo CSV (Excel separado por comas)" accept="csv" type="file"  runat="server" clientidmode="Static" />--%>
            </td>
            <td>
                <a onclick="abrirVentana('<%#Eval("RUTADOCUMENTO") %>')" style=" width:80px" class="topopup" target="_blank">
                <i></i> Ver Documento </a>
            </td>
            <td><%#Eval("EXTENSION")%></td>
            <%--<td id="COL_TO_HIDE" runat="server"><%#Eval("DOCUMENTOCARGADO")%></td> --%>
            <%--<td>--%>
            <%--<asp:Button Text="Ver Documento" Width="100px" runat="server" onclick="verDocumento(5);"/>--%>
                <%--<asp:LinkButton Text="Ver Documento" Width="100px" ID="lbVerDoc" runat="server" CommandName="ViewPDF" CommandArgument='<%#Eval("EXTENSION") %>'/>--%>
            <%--</td>--%>
            </tr>
            </ItemTemplate>
            <FooterTemplate>
            </tbody>
            </table>
            </FooterTemplate>
         </asp:Repeater>
      </table>
      <%--<table>
      <tr class="point">
      <td>
      <a  class="topopup" target="popup"  onclick="setObject(this);" href="#"  >
          <i class="ico-find" ></i> Regresar </a>
      </td>
      </tr>
      </table>--%>
                </div>
             </div>
             </div>
             <%--<div id="pager">
             Registros por página
                     <select class="pagesize">
                  <option selected="selected" value="10">10</option>
                  <option value="20">20</option>
                  </select>
                 <img alt="" src="../shared/imgs/first.gif" class="first"/>
                 <img alt="" src="../shared/imgs/prev.gif" class="prev"/>
                 <input  type="text" class="pagedisplay" size="5px"/>
                 <img alt="" src="../shared/imgs/next.gif" class="next"/>
                 <img alt="" src="../shared/imgs/last.gif" class="last"/>
            </div>--%>
              </div>
              <div id="sinresultado" runat="server" class="msg-info" >
              No se encontraron resultados, 
              asegurese que ha seleccionado correctamente un tipo de solicitud.
              </div>
              <div class="botonera">
              <img alt="loading.." src="../shared/imgs/loader.gif" id="loader" class="nover"  />
                <asp:Button ID="btsalvar" runat="server" Text="Regresar" Width="125px"
                               OnClientClick="return prepareObject();" onclick="btsalvar_Click" 
                               ToolTip="Regresa a la pantalla Emisión/Renovación de Credencial."/>
              </div>
              <%--</ContentTemplate>
              <Triggers>
              <asp:PostBackTrigger ControlID="find" />
              </Triggers>
              </asp:UpdatePanel>--%>
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
