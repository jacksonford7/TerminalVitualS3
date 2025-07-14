<%@ Page Language="C#" Title="Descarga de Vacíos" AutoEventWireup="true" 
         CodeBehind="RptEmptyCntDesc.aspx.cs" Inherits="CSLSite.rpt_econt_desc" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Descarga de Vacíos</title>
    <link href="../shared/estilo/catalogos.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/work.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/aisv-general.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/tables.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/print.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
    p{ margin:0; padding:0;}
    td span { font-weight:bold; display:block; margin:0; padding:0; background-color:#CCC; text-align:right; }
   
    
    
        .style1
        {
            width: 64px;
        }
        #bookingfrm
        {
            width: 100%;
        }
   
    
    
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
    <div class="catabody" style="width:1200px; height:500px; overflow:auto">
       <div class="catawrap" >
   
   <table class="tabRepeat" style="width:98%; margin:0 auto;">
   <tr><td colspan="2" class="style1"><h1>S3: Sistema de Solicitud de Servicio - Contecon Guayaquil S.A.</h1>
   <h1>Reporte de Descarga de Contenedores Vacíos</h1></td><td rowspan="4">
       <img alt="logo"  src="../shared/imgs/logoContecon.png" width="150px" height="50px" /></td></tr>
   <%--<tr><td><span> Fecha de consolidación:</span> </td><td><p id="cfecha" runat="server">1900/01/01</p></td></tr>
   <tr><td><span> Booking:</span> </td><td><p id="cbook" runat="server">ABC00123</p></td></tr>
   <tr><td><span> Linea:</span> </td><td><p id="clinea" runat="server">LINEA DE PRUEBAS CONTECON</p></td></tr>--%>
   </table>
         <div class="cataresult-" >
             <div class="booking" >
                  <div class="separator">EMPTY CONTAINERS DISCHARGE</div>
                 <asp:Repeater ID="tablePagination" runat="server">
                 <HeaderTemplate>
                 <table id="tabla" style=" font-size:medium"  cellspacing="1" cellpadding="1" class="tabRepeat">
                 <thead>
                 <tr>
                 <th style="width:250px;">Vessel</th>
                <th style="width:60px">Voyage</th>
                <th style="width:60px">Reference</th>
                <th style="width:50px">Flag</th>
                <th style="width:80px">Line Vessel</th>
                <th style="width:30px">Container</th>
                <th style="width:100px">Traffic</th>
                <th style="width:30px">Tare</th>
                <th style="width:50px">Seal_1</th>
                <th style="width:50px">Line</th>
                <th style="width:80px">Stowage</th>
                <th style="width:30px">POL</th>
                <th style="width:70px">POL Name</th>
                <th style="width:30px">POD</th>
                <th style="width:70px">POD Name</th>
                <th style="width:30px">Type</th>
                <th style="width:30px">ISO</th>
                 </tr>
                 </thead> 
                 <tbody>
                 </HeaderTemplate>
                 <ItemTemplate>
                  <tr class="point" >
                  <td><%#Eval("VESSEL")%></td>
                <td><%#Eval("VOYAGE")%></td>
                <td><%#Eval("REFERENCE")%></td>
                <td><%#Eval("FLAG")%></td>
                <td><%#Eval("LINE VESSEL")%></td>
                <td><%#Eval("CONTAINER")%></td>
                <td><%#Eval("TRAFFIC")%></td>
                <td><%#Eval("TARE")%></td>
                <td><%#Eval("SEAL_1")%></td>
                <td><%#Eval("LINE")%></td>
                <td><%#Eval("STOW POSITION")%></td>
                <td><%#Eval("POL")%></td>
                <td><%#Eval("POL NAME")%></td>
                <td><%#Eval("POD")%></td>
                <td><%#Eval("POD NAME")%></td>
                <td><%#Eval("TYPE")%></td>
                <td><%#Eval("ISO")%></td>
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
