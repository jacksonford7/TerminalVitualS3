<%@ Page Language="C#" AutoEventWireup="true" Title="Solicitud Registro de Empresa"
CodeBehind="consultasolicitudempresa.aspx.cs" Inherits="CSLSite.cliente.consultasolicitudempresa" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Catálogo de agentes</title>
    <link href="../shared/estilo/catalogosolicitudempresa.css" rel="stylesheet" type="text/css" />
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
          <tr><td rowspan="2" class="inum"> <div class="number">1</div></td><td class="level1" >Tipo de Cliente - Empresa.</td></tr>
          <tr><td class="level2">Tipo de Cliente - Empresa.</td></tr>
          </table>
         </div>
       <div class="colapser colapsa"></div>
       <div class="accion">
         <table class="controles" style=" font-size:small" cellspacing="0" cellpadding="1">
            <tr>
            <td class="bt-bottom bt-right bt-left" style=" width:155px;" >1. Tipo de Cliente:</td>
            <td class="bt-bottom bt-right">
            <asp:TextBox ID="txttipcli" runat="server" Width="99%" MaxLength="500" Enabled="false"
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
      <tr><td rowspan="2" class="inum"> <div class="number">2</div></td><td class="level1" >Información del Cliente.</td></tr>
      <tr><td class="level2">
         Confirme que los datos sean correctos.
      </td></tr>
      </table>
     </div>
       <div class="colapser colapsa"></div>
       <div class="accion">
      <table class="controles" style=" font-size:small" cellspacing="0" cellpadding="1">
         <tr>
         <td class="bt-bottom  bt-right bt-left" style=" width:155px" >2. Nombre/Razón Social:</td>
         <td class="bt-bottom bt-right">
           <asp:TextBox ID="txtrazonsocial" runat="server" Width="99%" MaxLength="500" Enabled="false"
             style="text-align: center;" 
             onkeypress="return soloLetras(event,'abcdefghijklnmñopqrstuvwxyz01234567890 ',true)"></asp:TextBox>
         </td>
         </tr>
         <tr>
         <td class="bt-bottom  bt-right bt-left" style=" width:155px" >
         3. RUC, C.I o Pasaporte
         </td>
         <td class="bt-bottom bt-right">
            <asp:TextBox ID="txtruccipas" runat="server" Width="99%" MaxLength="25" Enabled="false"
             style="text-align: center"
             
             onkeypress="return soloLetras(event,'01234567890abcdefghijklnmñopqrstuvwxyz',true)"></asp:TextBox>
         </td>
         </tr>
         <tr><td class="bt-bottom  bt-right bt-left" style=" width:155px">4. Actividad Comercial:</td>
         <td class="bt-bottom bt-right">
            <asp:TextBox ID="txtactividadcomercial" runat="server" Width="99%" MaxLength="500" Enabled="false"
             style="text-align: center" 
             onkeypress="return soloLetras(event,'abcdefghijklnmñopqrstuvwxyz01234567890 ',true)"></asp:TextBox>
         </td>
         </tr>
         <tr><td class="bt-bottom  bt-right bt-left" style=" width:155px">5. Dirección Oficina:</td>
         <td class="bt-bottom bt-right">
            <asp:TextBox ID="txtdireccion" runat="server" Width="99%" MaxLength="500" Enabled="false"
             style="text-align: center" 
             onkeypress="return soloLetras(event,'abcdefghijklnmñopqrstuvwxyz01234567890/.-_ ',true)"></asp:TextBox>
         </td>
         </tr>
         <tr>
         <td class="bt-bottom  bt-right bt-left" style=" width:155px" >6. Teléfono Oficina:</td>
         <td class="bt-bottom bt-right">
            <asp:TextBox ID="txttelofi" runat="server" Width="99%" MaxLength="9" Enabled="false"
             style="text-align: center"
             
             onkeypress="return soloLetras(event,'01234567890',true)"></asp:TextBox>
         </td>
         </tr>
         <tr>
         <td class="bt-bottom  bt-right bt-left" style=" width:155px" >8. Persona Contacto:</td>
         <td class="bt-bottom bt-right">
           <asp:TextBox ID="txtcontacto" runat="server" Width="99%" MaxLength="500" Enabled="false"
             style="text-align: center" 
             onkeypress="return soloLetras(event,'abcdefghijklnmñopqrstuvwxyz01234567890 ',true)"></asp:TextBox>
         </td>
         </tr>
         <tr>
         <td class="bt-bottom  bt-right bt-left" style=" width:155px" >9. Celular Contacto:</td>
         <td class="bt-bottom bt-right">
            <asp:TextBox ID="txttelcelcon" runat="server" Width="99%" MaxLength="10" Enabled="false"
             style="text-align: center"
             
             onkeypress="return soloLetras(event,'01234567890',true)"></asp:TextBox>
         </td>
         </tr>
         <tr>
         <td class="bt-bottom  bt-right bt-left" style=" width:155px;" >10. Mail Contacto:</td>
         <td class="bt-bottom bt-right">
             <asp:TextBox runat="server" id="txtmailinfocli" Width="99%" Enabled="false"></asp:TextBox>
          </td>
         </tr>
         <tr>
         <td class="bt-bottom  bt-right bt-left" style=" width:155px;" >11. Mail EBilling:</td>
         <td class="bt-bottom bt-right">
             <asp:TextBox runat="server" id="txtmailebilling" Width="99%" Enabled="false"></asp:TextBox>
          </td>
         </tr>
         <tr>
         <td class="bt-bottom  bt-right bt-left" style=" width:155px" >12. Certificaciones:</td>
         <td class="bt-bottom bt-right">
           <asp:TextBox ID="txtcertificaciones" runat="server" Width="99%" MaxLength="500" Enabled="false"
             style="text-align: center"
             
                 onkeypress="return soloLetras(event,'abcdefghijklnmñopqrstuvwxyz01234567890;./-_ ',true)"></asp:TextBox>
         </td>
         </tr>
         <tr>
         <td class="bt-bottom  bt-right bt-left" style=" width:155px" >13. Sitio Web:</td>
         <td class="bt-bottom bt-right">
             <asp:TextBox
              id='turl' runat="server" style= 'width:99%; font-size:small'
              enableviewstate="false" clientidmode="Static" Enabled="false"
              onkeypress="return soloLetras(event,'abcdefghijklmnopqrstuvwxyz1234567890_:.-/;',true)"  
              maxlength="250"> </asp:TextBox>
         </td>
         </tr>
         <tr><td class="bt-bottom bt-left bt-right" style=" width:155px">14. Afiliación a Gremios:</td>
         <td class="bt-bottom bt-right">
            <asp:TextBox ID="txtafigremios" runat="server" Width="99%" MaxLength="1000" Enabled="false"
             style="text-align: center"
             onkeypress="return soloLetras(event,'abcdefghijklnmñopqrstuvwxyz01234567890;./-_ ',true)"></asp:TextBox>
         </td>
         </tr>
         <tr><td class="bt-bottom  bt-right bt-left" style=" width:155px">15. Referencia Comercial:</td>
         <td class="bt-bottom bt-right">
            <asp:TextBox ID="txtrefcom" runat="server" Width="99%" MaxLength="500" Enabled="false"
             style="text-align: center" 
                 onkeypress="return soloLetras(event,'abcdefghijklnmñopqrstuvwxyz01234567890;./-_ ',true)" 
                 ></asp:TextBox>
         </td>
         </tr>
      </table>
      </div>
       </div>
       <div class="seccion">
      <div class="informativo">
      <table>
      <tr><td rowspan="2" class="inum"> <div class="number">3</div></td><td class="level1" >Información del Representante Legal.</td></tr>
      </table>
     </div>
    <div class="colapser colapsa"></div>
      <div class="accion">
      <table class="controles" cellspacing="0" cellpadding="1" style=" font-size:small" >
         <tr>
         <td class="bt-bottom  bt-right bt-left" style=" width:155px" >16. Representante Legal:</td>
         <td class="bt-bottom bt-right">
           <asp:TextBox ID="txtreplegal" runat="server" Width="99%" MaxLength="500"
             style="text-align: center" Enabled="false"
                 onkeypress="return soloLetras(event,'abcdefghijklnmñopqrstuvwxyz01234567890 ',true)" 
                 ></asp:TextBox>
         </td>
         </tr>
         <tr>
         <td class="bt-bottom  bt-right bt-left" style=" width:155px" >17. Teléfono Domicilio:</td>
         <td class="bt-bottom bt-right">
            <asp:TextBox ID="txttelreplegal" runat="server" Width="99%" MaxLength="9"
             style="text-align: center"
             Enabled="false"
             onkeypress="return soloLetras(event,'01234567890',true)"></asp:TextBox>
         </td>
         </tr>
         <tr><td class="bt-bottom  bt-right bt-left" style=" width:155px">18. Dirección Domiciliaria:</td>
         <td class="bt-bottom bt-right">
            <asp:TextBox ID="txtdirdomreplegal" runat="server" Width="99%" MaxLength="500"
             style="text-align: center" Enabled="false"
             onkeypress="return soloLetras(event,'abcdefghijklnmñopqrstuvwxyz01234567890 ./_-',true)"></asp:TextBox>
         </td>
         </tr>
         <tr>
         <td class="bt-bottom  bt-right bt-left" style=" width:155px" >19. Cédula de Identidad:</td>
         <td class="bt-bottom bt-right">
            <asp:TextBox ID="txtci" runat="server" Width="99%" MaxLength="10"
             style="text-align: center"
             Enabled="false"
             onkeypress="return soloLetras(event,'01234567890',true)"></asp:TextBox>
         </td>
         </tr>
         <tr>
         <td class="bt-bottom  bt-right bt-left" style=" width:155px;" >20. Mail:</td>
         <td class="bt-bottom bt-right">
             <asp:TextBox runat="server" id="tmailRepLegal" Width="99%" Enabled="false"/>
          </td>
         </tr>
      </table>
     </div>
    </div>
       <div class="seccion">
       <div class="informativo">
       <table>
       <tr><td rowspan="2" class="inum"> <div class="number">4</div></td><td class="level1" >Observación en caso de rechazo.</td></tr>
       </table>
       <div class="colapser colapsa"></div>
       <table class="controles" cellspacing="0" cellpadding="1" style=" font-size:small" >
       <tr>
         <%--<td class="bt-bottom  bt-right bt-left" style=" width:155px" >Motivo de rechazo:</td>--%>
         <td class="bt-bottom bt-left bt-right">
           <asp:TextBox ID="txtmotivorechazo" ForeColor="Red" runat="server" Width="99%" MaxLength="500" Enabled="false"
             style="text-align: center"  onkeypress="return soloLetras(event,'abcdefghijklnmñopqrstuvwxyz01234567890-_/ ',true)"></asp:TextBox>
       </td>
       </tr>
       </table>
       </div>
       </div>
       <div class="cataresult" >
       <div id="xfinder" runat="server">
       <div class="findresult" >
        <div class="booking">
       <%--<div class="separator">Revisar los documentos solicitados:</div>--%>
        <div class="bokindetalle" style=" overflow:auto; height:100%; width:100%; font-size:inherit">
         <div class="bokindetalle">
            <table id="tablerp" cellpadding="1" cellspacing="0">
            <asp:Repeater ID="tablePaginationDocumentos" runat="server">
            <HeaderTemplate>
            <table id="tablar"  cellspacing="1" cellpadding="1" class="tabRepeat">
            <thead>
            <tr>
            <th>Tipo de Empresa</th>
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
            <td style=" width:200px"><%#Eval("TIPOEMPRESA")%></td>
            <td style=" width:400px; font-size:inherit"><%#Eval("DOCUMENTO")%></td>
            <td style=" width:80px">
                <%--<asp:Button Text="Ver Documento" Width="100px" runat="server" onclick="verDocumento(5);"/>--%>
                <%--<asp:LinkButton Text="Ver Documento" Width="100px" ID="lbVerDoc" runat="server" CommandName="ViewPDF" CommandArgument='<%#Eval("RUTADOCUMENTO") %>'/>--%>
                <a href='<%#Eval("RUTADOCUMENTO") %>' style=" width:80px" class="topopup" target="_blank">
                    <i></i> Ver Documento </a>
            </td>
            <td style=" width:70px"><asp:CheckBox runat="server" Checked='<%#Eval("ESTADOCOL")%>' Enabled="false" id="chkRevisado"/></td>
            <td><asp:TextBox ID="tcomentario" ForeColor="Red" runat="server" Width="250px" Enabled="false" Text='<%#Eval("COMENTARIO")%>' onkeypress="return soloLetras(event,'abcdefghijklnmñopqrstuvwxyz01234567890-_/ ',true)"></asp:TextBox></td>
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
        <%--<div class="seccion" runat="server" id="factura">
      <div class="accion">
       <table runat="server" class="controles" id="tablefac" style=" font-size:small">
       <tr><td class="bt-bottom bt-top  bt-right bt-left" style=" width:155px">Adjuntar factura:</td>
         <td class="bt-bottom bt-left bt-top bt-right">
         <asp:FileUpload runat="server" ID="fuAdjuntarFactura" Width="100%" />
         </td>
         </tr>
       </table>
       </div>
       </div>--%>
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
                   <div id="sinresultado" runat="server" class="msg-info" >
              No se encontraron resultados, 
              asegurese que ha seleccionado correctamente un tipo de solicitud.
              </div>
        </div>
        </div>
       <div class="botonera" runat="server" id="salir">
        <asp:Button ID="btnSalir" runat="server" Text="Salir" Width="125px"
                onclick="btnSalir_Click" 
                ToolTip="Regresa a la Pantalla Consultar Solicitud."/>
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
        function prepareObject(valor) {
            try {
//                document.getElementById("loader").className = '';
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
//              alert('Por favor escriba una o varias \nletras del número');
              return false;
          }
          document.getElementById('imagen').innerHTML = '<img alt="" src="../shared/imgs/loader.gif">';
      }
      function Quit() {
//          event.returnValue = '[ ! ] Se perdera el trabajo realizado [ ! ]';
//          return;
      }
      function soloLetras(e, caracteres, espacios) {
          key = e.keyCode || e.which;
          tecla = String.fromCharCode(key).toLowerCase();
          if (caracteres) {
              letras = caracteres;
          }
          else {
              letras = " áéíóúabcdefghijklmnñopqrstuvwxyz1234567890-_/ ";
          }
          if (espacios == undefined || espacios == null) {
              especiales = [8, 13, 32, 9, 16, 20];
          }
          else {
              especiales = [8, 13, 9, 16, 20];
          }
          tecla_especial = false
          for (var i in especiales) {
              if (key == especiales[i]) {
                  tecla_especial = true;
                  break;
              }
          }
          if (letras.indexOf(tecla) == -1 && !tecla_especial) {
              return false;
          }
      }
      function verDocumento(val) {
          var caja = val;
          window.open('../credenciales/solicituddocumentos/?placa=' + caja, 'name', 'width=850,height=480')
      }
   </script>
</body>
</html>
