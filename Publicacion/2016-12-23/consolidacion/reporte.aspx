<%@ Page Language="C#" Title="Catálogo de agentes" AutoEventWireup="true" ç
         CodeBehind="reporte.aspx.cs" Inherits="CSLSite.turno_reporte_consolidacion" %>
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
   <tr><td colspan="2"><h1>Reporte de asignación de turnos para consolidacion</h1></td><td rowspan="4">
       <img alt="logo"  src="../shared/imgs/logoContecon.png" width="150px" height="50px" /></td></tr>
   <tr><td><span> Fecha de consolidación:</span> </td><td><p id="cfecha" runat="server">1900/01/01</p></td></tr>
   <tr><td><span> Booking:</span> </td><td><p id="cbook" runat="server">ABC00123</p></td></tr>
   <tr><td><span> Linea:</span> </td><td><p id="clinea" runat="server">LINEA DE PRUEBAS CONTECON</p></td></tr>
   </table>
         <div class="cataresult" >
             <div class="booking" >
                  <div class="separator">HORARIOS Y RESERVAS</div>
                 <asp:Repeater ID="tablePagination" runat="server" >
                 <HeaderTemplate>
                 <table id="tablasort" cellspacing="1" cellpadding="1" class="tabRepeat">
                 <tr>
                 <th>#</th>
                 <th>Desde</th>
                 <th>Hasta</th>
                 <th>Reservado</th>
                 </tr>
                 </HeaderTemplate>
                 <ItemTemplate>
                  <tr>
                  <td><%#Eval("ROW")%></td>
                  <td><%#Eval("HORA_DESDE")%></td>
                  <td><%#Eval("HORA_HASTA")%></td>
                  <td><%#Eval("RESERVADO")%></td>
                 </tr>
                  </ItemTemplate>
                 <FooterTemplate>
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
