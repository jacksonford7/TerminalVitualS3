<%@ Page Language="C#" Title="Reporte de Salida" AutoEventWireup="true" 
         CodeBehind="RptRefCont.aspx.cs" Inherits="CSLSite.rpt_ref_cont" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Reporte de Salida</title>
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
            width: 32px;
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
    <div class="catabody" style="width:2000px;">
       <div class="catawrap" >
   
   <table class="tabRepeat" style="width:96%; margin:0 auto;">
   <tr><td colspan="2" class="style1"><h1>S3: Sistema de Solicitud de Servicio - Contecon Guayaquil S.A.</h1>
   <h1>Reporte de Contenedores Reefer</h1></td><td rowspan="4">
       <img alt="logo"  src="../shared/imgs/logoContecon.png" height="50px" 
           style="width: 241px; margin-left: 0px" /></td></tr>
   <%--<tr><td><span> Fecha de consolidación:</span> </td><td><p id="cfecha" runat="server">1900/01/01</p></td></tr>
   <tr><td><span> Booking:</span> </td><td><p id="cbook" runat="server">ABC00123</p></td></tr>
   <tr><td><span> Linea:</span> </td><td><p id="clinea" runat="server">LINEA DE PRUEBAS CONTECON</p></td></tr>--%>
   </table>
         <div class="cataresult" >
             <div class="booking" >
                  <div class="separator">CONTAINERS REEFER</div>
                 <asp:Repeater ID="tablePagination" runat="server">
                 <HeaderTemplate>
                 <table id="tabla" style=" font-size:medium"  cellspacing="1" cellpadding="1" class="tabRepeat">
                 <thead>
                 <tr>
                 <th style="width:250px;">Vessel</th>
                 <th style="width:60px">Voyage</th>
                 <th style="width:70px">Reference</th>
                 <th style="width:30px">Container</th>
                 <th style="width:30px">Size</th>
                 <th style="width:30px">Type</th>
                 <th style="width:120px">Bill of Lading</th>
                 <th style="width:60px">Booking</th>
                 <th style="width:50px">Document</th>
                 <th style="width:80px">Type Doc</th>
                 <th style="width:70px">FPOD</th>
                 <th style="width:30px">Time</th>
                 <%--<th>Exportador</th>--%>
                 <th style="width:100px">Event</th>
                 <th style="width:30px">Temp</th>
                 <th style="width:30px">O2</th>
                 <th style="width:110px">CO2</th>
                 <th style="width:110px">Unit</th>
                 <th style="width:110px">ReadMode</th>
                 </tr>
                 </thead> 
                 <tbody>
                 </HeaderTemplate>
                 <ItemTemplate>
                  <tr class="point" >
                  <td><%#Eval("VESSEL")%></td>
                  <td><%#Eval("VOYAGE")%></td>
                  <td><%#Eval("REFERENCE")%></td>
                  <td><%#Eval("CONTAINER")%></td>
                  <td><%#Eval("SIZE")%></td>
                  <td><%#Eval("TYPE")%></td>
                  <td><%#Eval("BILL OF LADING")%></td>
                  <td><%#Eval("BOOKING")%></td>
                  <td><%#Eval("DOCUMENT")%></td>
                  <td><%#Eval("TYPE DOC")%></td>
                  <td><%#Eval("FPOD")%></td>
                  <td><%#Eval("TIME")%></td>
                  <td><%#Eval("EVENT")%></td>
                  <td><%#Eval("TEMP")%></td>
                  <td><%#Eval("O2")%></td>
                  <td><%#Eval("CO2")%></td>
                  <td><%#Eval("UNIT")%></td>
                  <td><%#Eval("READMODE")%></td>
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
