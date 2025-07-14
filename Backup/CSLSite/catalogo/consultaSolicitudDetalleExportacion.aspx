
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="consultaSolicitudDetalleExportacion.aspx.cs" Inherits="CSLSite.consultaSolicitudDetalleExportacion" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Contenedores</title>
    <link href="../shared/estilo/catalogos.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/work.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/pages.js" type="text/javascript"></script>
    <link href="../shared/estilo/aisv-general.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/tables.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        function BindFunctions() {
            
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
                 
         </div>
         <div class="cataresult" >
             <asp:UpdatePanel ID="upresult" runat="server"  >
             <ContentTemplate>
             <script type="text/javascript">                 Sys.Application.add_load(BindFunctions); </script>
               <div id="xfinder" runat="server" visible="false" >
             <div class="msg-alerta" id="alerta" >
                 Confirme que los datos sean correctos. En caso de error, favor comuníquese con 
                 el Departamento de Planificación a los teléfonos: +593 (04) 6006300, 3901700 
                 ext. 4039, 4040, 4060. 
             </div>
             <%-- catalogo de bookings--%>
             <div class="findresult" >
             <div class="booking" >
                  <div class="separator">Contenedor</div>
                 <div class="bokindetalle">
                 <asp:Repeater ID="tablePagination" runat="server" >
                 <HeaderTemplate>
                 <table id="tablasort" cellspacing="1" cellpadding="1" class="tabRepeat">
                 <thead>
                 <tr>
                 <th align="center">No.</th>
                 <th align="center">Contenedor</th>
                 <th align="center">Número de Entrega</th>
                 <th align="center">Aisv</th>
                 <th align="center">DAE de solicitud</th>
                 </tr>
                 </thead> 
                 <tbody>
                 </HeaderTemplate>
                 <ItemTemplate>
                 <tr class="point">
                  <td><%#Eval("noFila")%></td>
                  <td><%#Eval("descripcionContenedor")%></td>
                  <td><%#Eval("numeroEntrega")%></td>  
                  <td><%#Eval("aisv")%></td>
                  <td><%#Eval("dae")%></td>                      
                  <%--<td><asp:TextBox TextMode="MultiLine" id='txtArea' runat="server" ReadOnly="true" Text='<%#Eval("observacion")%>'/> </td>--%>
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
            
              </div>
               <div id="sinresultado" runat="server" class="msg-info">
              No se encontraron resultados,  asegurese que ha escrito correctamente el nombre/referencia del contenedor
              </div>
             </ContentTemplate>
             <Triggers>
             <%--<asp:AsyncPostBackTrigger ControlID="find" />--%>
             </Triggers>
             </asp:UpdatePanel>
       </div>
     <%-- </div>--%>
      </div>
    <input id="json_object" type="hidden" />
    </form>
     <script src="../Scripts/jquery-1.7.1.min.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.tablesorter.min.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.tablesorter.pager.js" type="text/javascript"></script>
   <script type="text/javascript" >
       //       function setContenedor(row) {
       //            var celColect = row.getElementsByTagName('td');
       //              var contenedor = {
       //                  item: celColect[0].textContent,
       //                  codigo: celColect[1].textContent,
       //                  booking: celColect[2].textContent,
       //                  tipoContenedor: celColect[3].textContent
       //              };

       //             if (window.opener != null) {
       //                 window.opener.popupCallback(contenedor);
       //            }
       //            self.close();
       //      }
       function msgfinder(control, expresa) {
           if (control.value.trim().length <= 0) {
               this.document.getElementById(expresa).textContent = 'Escriba una o varias letras del nombre/código y pulse buscar';
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
