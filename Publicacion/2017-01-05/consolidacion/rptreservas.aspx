<%@ Page Language="C#" Title="Catálogo de agentes" AutoEventWireup="true" 
         CodeBehind="rptreservas.aspx.cs" Inherits="CSLSite.rptreservas_consolidacion" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Catálogo de agentes</title>
    <link href="../shared/estilo/catalogos.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/work.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/aisv-general.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/tables.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/print.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
    p{ margin:0; padding:0;}
    td span { font-weight:bold; display:block; margin:0; padding:0; background-color:#CCC; text-align:right; }
   
    
    
    </style>

        <script language="javascript" type="text/javascript">
// <![CDATA[
            function btclear_onclick() {
                window.print()
            }
// ]]>
    </script>

</head>
<body>
    <form id="bookingfrm" runat="server">
    <div class="catabody">
       <div class="catawrap" >
   
   <table class="tabRepeat" style="width:96%; margin:0 auto;">
   <tr><td colspan="2"><h1>Reporte de reservas para consolidación</h1></td><td rowspan="4">
       <img alt="logo"  src="../shared/imgs/logoContecon.png" width="150px" height="50px" /></td></tr>
   <%--<tr><td><span> Fecha de consolidación:</span> </td><td><p id="cfecha" runat="server">1900/01/01</p></td></tr>
   <tr><td><span> Booking:</span> </td><td><p id="cbook" runat="server">ABC00123</p></td></tr>
   <tr><td><span> Linea:</span> </td><td><p id="clinea" runat="server">LINEA DE PRUEBAS CONTECON</p></td></tr>--%>
   </table>
         <div class="cataresult" >
             <div class="booking" >
                  <div class="separator">HORARIOS Y RESERVAS</div>
                 <asp:Repeater ID="tablePagination" runat="server">
                 <HeaderTemplate>
                 <table id="tabla" style=" font-size:medium"  cellspacing="1" cellpadding="1" class="tabRepeat">
                 <thead>
                 <tr>
                 <%--<th class="nover">#</th>--%>
                 <th style="width:80px">Ruc</th>
                 <th style="width:60px">Booking</th>
                 <th style="width:120px">Fecha</th>
                 <th style="width:50px">Cantidad</th>
                 <th style="width:50px">Estado</th>
                 <th>Exportador</th>
                 <th style="width:30px">Desde</th>
                 <th style="width:30px">Hasta</th>
                 <th style="width:110px">Contenedores</th>
                 <%--<th>Total</th>
                 <th>Reservado</th>
                 <th>Disponible</th>
                 <th>Cancelado</th>--%>
                 </tr>
                 </thead> 
                 <tbody>
                 </HeaderTemplate>
                 <ItemTemplate>
                  <tr class="point" >
                  <%--<td class="nover"><%#Eval("ROW")%></td>--%>
                  <td><%#Eval("RUC")%></td>
                  <td><%#Eval("BOOKING")%></td>
                  <td><%#Eval("FECHA_PRG")%></td>
                  <td><%#Eval("CANTIDAD")%></td>
                  <td><%#Eval("ESTADO")%></td>
                  <td><%#Eval("EXPORTADOR")%></td>
                  <td><%#Eval("DESDE")%></td>
                  <td><%#Eval("HASTA")%></td>
                  <td><%#Eval("CNTR")%>
                  <%--p><%# Eval("CNTR") %></p>--%>
                  <%--<asp:TextBox ID="lblCONTENEDOR" Text='<%# Eval("CNTR") %>' Enabled="false" runat="server" Width="150px" Height="50px" TextMode="MultiLine"/>--%>
                  </td>
                  <%--<td><%#Eval("TOTAL")%></td>
                  <td><%#Eval("RESERVADO")%></td>
                  <td><%#Eval("DISPONIBLE")%></td>
                  <td><%#Eval("CANCELADO")%></td>--%>
                 </tr>
                 </ItemTemplate>
                 <FooterTemplate>
                 </tbody>
                 </table>
                 </FooterTemplate>
         </asp:Repeater>
         </div>
       </div>
            <div class="botonera" >
            <input id="btclear"   type="reset"    value="Imprimir" onclick="return btclear_onclick()" />
            </div>
      </div>
      </div>
    </form>
</body>
</html>
