<%@ Page Language="C#" Title="Lista de Conductores" AutoEventWireup="true" CodeBehind="consultaColaboradorSCA.aspx.cs" Inherits="CSLSite.colaboradorsca" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Lista de Conductores</title>
    <link href="../shared/estilo/catalogos.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/work.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/aisv-general.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/tables.css" rel="stylesheet" type="text/css" />
     <script src="../Scripts/jquery-1.7.1.min.js" type="text/javascript"></script>
    <script src="../Scripts/pages.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.tablesorter.min.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.tablesorter.pager.js" type="text/javascript"></script>
    <script type="text/javascript">
            $(document).ready(function () {
                $('#tablasort').tablesorter({ cancelSelection: true, cssAsc: "ascendente", cssDesc: "descendente", headers: { 5: { sorter: false }, 6: { sorter: false}} }).tablesorterPager({ container: $('#pager'), positionFixed: false, pagesize: 10 });
            });
  
    </script>
</head>
<body >
     <noscript><meta http-equiv="refresh" content="0; url=sinsoporte.htm" /> </noscript>
    <form id="bookingfrm" runat="server">
    <div class="catabody">
       <div class="catawrap" >
         <div class="cataresult" >
             <div id="xfinder" runat="server" visible="false" >
             <div class="msg-alerta" id="alerta" runat="server">
                 
             </div>
             <%-- catalogo de bookings--%>
             <div class="findresult" >
             <div class="booking" >
                  <div class="separator"></div>
                 <div class="bokindetalle">
                 <asp:Repeater ID="tablePagination" runat="server" >
                 <HeaderTemplate>
                 <table id="tablasort" cellspacing="1" cellpadding="1" class="tabRepeat">
                 <thead>
                 <tr>
                 <th>Cedula</th>
                 <th>Nombres</th>
                 <th>Apellidos</th>
                 <th>Cargo</th>
                 <th>Empresa</th>
                 <th style=" display:none">TipoSangre</th>
                 <th style=" display:none">DirDomicilio</th>
                 <th style=" display:none">TelDomicilio</th>
                 <th style=" display:none">Email</th>
                 <th style=" display:none">LugNacimiento</th>
                 <th style=" display:none">FecNacimiento</th>
                 <th style=" display:none">TipLicencia</th>
                 <th style=" display:none">FecExpLicencia</th>
                 <th>Acciones</th>
                 </tr></thead> 
                 <tbody>
                 </HeaderTemplate>
                 <ItemTemplate>
                 <tr class="point" onclick="setObject(this);">
                  <td><%#Eval("CEDULA")%></td>
                  <td><%#Eval("NOMBRES")%></td>
                  <td><%#Eval("APELLIDOS")%></td>
                  <td><%#Eval("CARGO")%></td>
                  <td><%#Eval("EMPRESA")%></td>
                  <td style=" display:none"><%#Eval("TIPOSANGRE")%></td>
                  <td style=" display:none"><%#Eval("DIRECCIONDOM")%></td>
                  <td style=" display:none"><%#Eval("TELFDOM")%></td>
                  <td style=" display:none"><%#Eval("EMAIL")%></td>
                  <td style=" display:none"><%#Eval("LUGARNAC")%></td>
                  <td style=" display:none"><%#Eval("FECHANAC")%></td>
                  <td style=" display:none"><%#Eval("TIPOLICENCIA")%></td>
                  <td style=" display:none"><%#Eval("FECHAEXPLICENCIA")%></td>
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
              <div id="sinresultado" runat="server" class="msg-info" >
                  No se encontraron resultados, asegurese que ha escrito correctamente los criterios de consulta.</div>

       </div>
      </div>
      </div>
    </form>

   <script type="text/javascript" >
       function setObject(row) {
           var celColect = row.getElementsByTagName('td');
           var colaborador = {
              cedula: celColect[0].textContent,
              nombres: celColect[1].textContent,
              apellidos: celColect[2].textContent,
              cargo: celColect[3].textContent,
              tiposangre: celColect[5].textContent,
              dirdom: celColect[6].textContent,
              telfdom: celColect[7].textContent,
              email: celColect[8].textContent,
              lugnac: celColect[9].textContent,
              fecnac: celColect[10].textContent,
              tiplic: celColect[11].textContent,
              fecexplic: celColect[12].textContent
              };
            if (window.opener != null) {
                window.opener.popupCallback(colaborador, '');
            }
            self.close();
      }
   </script>
   
</body>
</html>
