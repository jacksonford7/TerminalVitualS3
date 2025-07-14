<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="buque.aspx.cs" Inherits="CSLSite.buque" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Catálogo Buques</title>
    <link href="../shared/estilo/catalogos.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/work.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/pages.js" type="text/javascript"></script>
    <link href="../shared/estilo/aisv-general.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/tables.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
       function BindFunctions(){
           $(document).ready(function () {
               document.getElementById('imagen').innerHTML = '';
               $('#tablasort').tablesorter({ cancelSelection: true, cssAsc: "ascendente", cssDesc: "descendente", headers: { 5: { sorter: false }, 6: { sorter: false}} }).tablesorterPager({ container: $('#pager'), positionFixed: false, pagesize: 10 });
           });
        }
    </script>
</head>
<body>
 <noscript><meta http-equiv="refresh" content="0; url=sinsoporte.htm" /> </noscript>
    <form id="bookingfrm" runat="server">
    <asp:ToolkitScriptManager ID="tkscata" runat="server"></asp:ToolkitScriptManager>
    <div class="catabody">
       <div class="catawrap" >
         <div class="catabuscar">
         <div class="catacapa">
             <p class="catalabel">Número IMO / IMO number:</p>
             <asp:TextBox ID="txtfinder" 
                 runat="server"  
                 CssClass="catamayusc" 
                 onkeypress="return soloLetras(event,'abcdefghijklmnñopqrstuvwxyz1234567890-*.')" 
                 MaxLength="30" Width="40%"
                 onkeyup="msgfinder(this,'valintro');"
                  ></asp:TextBox>  
             <asp:Button ID="find" runat="server" Text="Buscar" onclick="find_Click" OnClientClick="return initFinder();" />
             <span id="imagen"></span>
          </div>
         <p class="catavalida" id="valintro">Escriba una o varias letras del nombre y pulse buscar</p>
         </div>
         <div class="cataresult" >
             <asp:UpdatePanel ID="upresult" runat="server"  >
             <ContentTemplate>
             <script type="text/javascript">Sys.Application.add_load(BindFunctions); </script>
               <div id="xfinder" runat="server" visible="false" >
             <div class="msg-alerta" id="alerta" >
                 Confirme que los datos sean correctos. En caso de error, favor comuníquese con 
                 el Departamento de Planificación a los teléfonos: +593 (04) 6006300, 3901700 
                 ext. 4039, 4040, 4060. 
             </div>
             <%-- catalogo de bookings--%>
             <div class="findresult" >
             <div class="booking" >
                  <div class="separator">Buques / Ships</div>
                 <div class="bokindetalle">
                 <asp:Repeater ID="tablePagination" runat="server" >
                 <HeaderTemplate>
                 <table id="tablasort" cellspacing="1" cellpadding="1" class="tabRepeat">
                 <thead>
                 <tr>
                 <th>No.</th>
                 <th class="xax">bkey</th>
                 <th>IMO</th>
                 <th>Nombre</th>
                 <th>Flag</th>
                 <th>Linea</th>
                 <th class="xax">blargo</th>
                 <th class="xax">bancho</th>
                 <th class="xax">bservicio</th>
                 <th class="xax">bgros</th>
                  <th class="xax">bnet</th>
                  <th class="xax">bradio</th>
                   <th class="xax">btipo</th>
                 <th>Acciones</th></tr>
                 </thead> 
                 <tbody>
                 </HeaderTemplate>
                 <ItemTemplate>
                 <tr class="point" onclick="setBooking(this);">
                  <td><%#Eval("bnumber")%></td>
                  <td class="xax"><%#Eval("bkey")%></td>
                  <td><%#Eval("bid")%></td>
                  <td><%#Eval("bnombre")%></td>
                  <td><%#Eval("bpais")%></td>
                  <td><%#Eval("bline")%></td>
                  <td class="xax"><%#Eval("blargo")%></td>
                  <td class="xax"><%#Eval("bancho")%></td>
                  <td class="xax"><%#Eval("bservicio")%></td>
                  <td class="xax"><%#Eval("bgros")%></td>
                  <td class="xax"><%#Eval("bnet")%></td>
                  <td class="xax"><%#Eval("bradio")%></td>
                  <td class="xax"><%#Eval("btipo")%></td>
                  <td>
                     <a href="#" >Elegir</a>
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
             </div>
             <div id="pager">
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
            </div>
              </div>
               <div id="sinresultado" runat="server" class="msg-info">
            No se encontraron resultados, asegúrese que haya escrito correctamente el nombre de la línea buscada
              </div>
             </ContentTemplate>
             <Triggers>
             <asp:AsyncPostBackTrigger ControlID="find" />
             </Triggers>
             </asp:UpdatePanel>
       </div>
      </div>
      </div>
    <input id="json_object" type="hidden" />
    </form>
     <script src="../Scripts/jquery-1.7.1.min.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.tablesorter.min.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.tablesorter.pager.js" type="text/javascript"></script>
   <script type="text/javascript" >
       function setBooking(row) {
            var celColect = row.getElementsByTagName('td');
              var buque = {
                  bnumber: celColect[0].textContent,
                  bkey: celColect[1].textContent,
                  bid: celColect[2].textContent,
                  bnombre: celColect[3].textContent,
                  bpais: celColect[4].textContent,
                  bline: celColect[5].textContent,
                  blargo: celColect[6].textContent,
                  bancho: celColect[7].textContent,
                  bservicio: celColect[8].textContent,
                  bgros: celColect[9].textContent,
                  bnet: celColect[10].textContent,
                  bradio: celColect[11].textContent,
                  btipo: celColect[12].textContent
              };
             if (window.opener != null) {
                 window.opener.popupCallback(buque, 'Buque');
            }
            self.close();
      }
      function msgfinder(control, expresa) {
          if (control.value.trim().length <= 0) {
              this.document.getElementById(expresa).textContent = 'Escriba una o varias letras del nombre/IMO y pulse buscar';
              return;
          }
          this.document.getElementById(expresa).textContent = 'Se buscará [' + control.value.toUpperCase() + '], presione el botón';
      }
      function initFinder() {
          if (document.getElementById('txtfinder').value.trim().length <= 0) {
              alert('Escriba una o varias letras para iniciar la búsqueda');
              return false;
          }
          document.getElementById('imagen').innerHTML = '<img alt="" src="../shared/imgs/loader.gif">';
      }
   </script>
</body>

</html>
