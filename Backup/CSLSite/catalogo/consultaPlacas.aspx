<%@ Page Language="C#" Title="Lista de Placas" AutoEventWireup="true" CodeBehind="consultaPlacas.aspx.cs" Inherits="CSLSite.placas" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Lista de Placas</title>
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
                 <th>Placa / No. Serie</th>
                 <th>Clase / Tipo</th>
                 <th>Marca</th>
                 <th>Modelo</th>
                 <th>Color</th>
                 <th style=" display:none">TipoCertificado</th>
                 <th style=" display:none">NumCertificado</th>
                 <th style=" display:none">Categoria</th>
                 <th style=" display:none">FechaPoliza</th>
                 <th style=" display:none">FechaMTOP</th>
                 <th style=" display:none">TipoCategoria</th>
                 <th>Empresa</th>
                 <th>Acciones</th>
                 </tr></thead> 
                 <tbody>
                 </HeaderTemplate>
                 <ItemTemplate>
                 <tr class="point" onclick="setObject(this);">
                  <td><%#Eval("VE_PLACA")%></td>
                  <td><%#Eval("VE_TIPO")%></td>
                  <td><%#Eval("VE_MARCA")%></td>
                  <td><%#Eval("VE_MODELO")%></td>
                  <td><%#Eval("VE_COLOR")%></td>
                  <td style=" display:none"><%#Eval("VE_PMTIPO")%></td>
                  <td style=" display:none"><%#Eval("VE_PMCERTIFICADO")%></td>
                  <td style=" display:none"><%#Eval("DESCRIPCIONCATEGORIA")%></td>
                  <td style=" display:none"><%#Eval("VE_POLIZA")%></td>
                  <td style=" display:none"><%#Eval("VE_MTOP")%></td>
                  <td style=" display:none"><%#Eval("CODCATEGORIA")%></td>
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
                  No se encontraron resultados, asegurese que ha escrito correctamente la Placa / 
                  No. Serie (para Montacargas).</div>

       </div>
      </div>
      </div>
    </form>

   <script type="text/javascript" >
       function setObject(row) {
           var celColect = row.getElementsByTagName('td');
           var vehiculos = {
              placa: celColect[0].textContent,
              clasetipo: celColect[1].textContent,
              marca: celColect[2].textContent,
              modelo: celColect[3].textContent,
              color: celColect[4].textContent,
              tipocertificado: celColect[5].textContent,
              certificado: celColect[6].textContent,
              categoria: celColect[7].textContent,
              fechapoliza: celColect[8].textContent,
              fechamtop: celColect[9].textContent,
              tipocategoria: celColect[10].textContent
              };
            if (window.opener != null) {
                window.opener.popupCallback(vehiculos, '');
            }
            self.close();
      }
   </script>
   
</body>
</html>
