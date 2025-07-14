<%@ Page Language="C#" Title="Lista de Conductores" AutoEventWireup="true" CodeBehind="consultaCoductorDesignado.aspx.cs" Inherits="CSLSite.conductor" %>
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
                 <th>Empresa</th>
                 <th>Acciones</th>
                 </tr></thead> 
                 <tbody>
                 </HeaderTemplate>
                 <ItemTemplate>
                 <tr class="point" onclick="setObject(this);">
                  <td><%#Eval("NOMINA_CED")%></td>
                  <td><%#Eval("NOMINA_NOM")%></td>
                  <td><%#Eval("NOMINA_APE")%></td>
                  <td><%#Eval("EMPE_NOM")%></td>
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
           var conductor = {
              cedula: celColect[0].textContent,
              nombres: celColect[1].textContent,
              apellidos: celColect[2].textContent
              };
            if (window.opener != null) {
                window.opener.popupCallback(conductor, '');
            }
            self.close();
      }
   </script>
   
</body>
</html>
