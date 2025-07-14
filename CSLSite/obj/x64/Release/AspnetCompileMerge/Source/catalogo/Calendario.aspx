<%@ Page Language="C#" Title="Calendario" AutoEventWireup="true" CodeBehind="Calendario.aspx.cs" Inherits="CSLSite.calendario" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    
         <link href="../img/favicon2.png" rel="icon"/>
  <link href="../img/icono.png" rel="apple-touch-icon"/>
  <link href="../css/bootstrap.min.css" rel="stylesheet"/>
  <link href="../css/dashboard.css" rel="stylesheet"/>
  <link href="../css/icons.css" rel="stylesheet"/>
  <link href="../css/style.css" rel="stylesheet"/>
  <link href="https://fonts.googleapis.com/css2?family=Roboto:wght@300;400;500;700&display=swap" rel="stylesheet"/>
    <link href="../shared/estilo/catalogos.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/tables.css" rel="stylesheet" type="text/css" />
    <script src="../lib/jquery/jquery.min.js" type="text/javascript"></script>
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
     
    <form id="bookingfrm" runat="server">
         <div class="dashboard-container p-4" id="cuerpo" runat="server">
               <div class="col-md-12 d-flex justify-content-center"> 
                  
      <div class="  alert alert-warning" id="alerta" runat="server">
          </div>
		   
     </div>
                <div id="xfinder" runat="server" visible="false" >
              <div class="booking" >
                  <div class="form-title">Fechas disponibles</div>
                 <div class="bokindetalle">
                 <asp:Repeater ID="tablePagination" runat="server" >
                 <HeaderTemplate>
                 <table id="tablasort" cellspacing="1" cellpadding="1" class="table table-bordered invoice">
                 <thead>
                 <tr>
                 <th>No.</th>
                 <th>Fecha</th>
                 <th>Cantidad</th>
                 <th>Acciones</th>
                 </tr>
                 </thead> 
                 <tbody>
                 </HeaderTemplate>
                 <ItemTemplate>
                 <tr class="point" onclick="setObject(this);">
                  <td><%#Eval("item1")%></td>
                  <td><%#Eval("item2")%></td>
                  <td><%#Eval("item3")%></td>
                  <td>
                     <a href="#" class="btn btn-link" >Elegir</a>
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
              <div id="pager">
             Registros por página
                  <select class="pagesize">
                  <option selected="selected" value="10">10</option>
                  <option value="20">20</option>
                  </select>
                 <img alt="" src="../shared/imgs/first.gif" class="first"/>
                 <img alt="" src="../shared/imgs/prev.gif" class="prev"/>
                 <input  type="text" class="pagedisplay" />
                 <img alt="" src="../shared/imgs/next.gif" class="next"/>
                 <img alt="" src="../shared/imgs/last.gif" class="last"/>
            </div>
              <div id="sinresultado" runat="server" class=" alert alert-primary" >
              No se encontraron resultados, 
              asegurese que ha escrito correctamente el número/nombre
              buscado  
              </div>
              </div>
         </div>
    </form>

   <script type="text/javascript" >
       function setObject(row) {
           var celColect = row.getElementsByTagName('td');
          var bookin = {
              fila: celColect[0].textContent,
              fecha: celColect[1].textContent,
              cantidad: celColect[2].textContent
              };
            if (window.opener != null) {
                window.opener.popupCallback(bookin, 'cc');
            }
            self.close();
      }
   </script>
   
</body>
</html>
