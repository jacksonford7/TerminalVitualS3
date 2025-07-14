<%@ Page Language="C#" Title="Descarga de Contenedores" AutoEventWireup="true" 
         CodeBehind="RptFullCntDesc.aspx.cs" Inherits="CSLSite.rpt_fcont_desc" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Descarga de Contenedores</title>
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
            width: 100%;
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
   
   <table class="tabRepeat" style="width:96%; margin:0 auto;">
   <tr><td colspan="2" class="style1"><h1>S3: Sistema de Solicitud de Servicio - Contecon Guayaquil S.A.</h1>
   <h1>Reporte de Desarga de Contenedores Full</h1></td><td rowspan="4">
       <img alt="logo"  src="../shared/imgs/logoContecon.png" width="150px" height="50px" /></td></tr>
   <%--<tr><td><span> Fecha de consolidación:</span> </td><td><p id="cfecha" runat="server">1900/01/01</p></td></tr>
   <tr><td><span> Booking:</span> </td><td><p id="cbook" runat="server">ABC00123</p></td></tr>
   <tr><td><span> Linea:</span> </td><td><p id="clinea" runat="server">LINEA DE PRUEBAS CONTECON</p></td></tr>--%>
   </table>
         <div class="cataresult-" >
             <div class="booking" >
                  <div class="separator">FULL CONTAINERS DISCHARGE</div>
                 <asp:Repeater ID="tablePagination" runat="server">
                 <HeaderTemplate>
                 <table id="tabla" style=" font-size:medium"  cellspacing="1" cellpadding="1" class="tabRepeat">
                 <thead>
                 <tr>
                 <th style="width:250px;">Vessel</th>
                <th style="width:60px">Voyage</th>
                <th style="width:60px">Reference</th>
                <th style="width:50px">Flag</th>
                <th style="width:80px">Stow Position</th>
                <th style="width:60px">Line</th>
                <th style="width:30px">Container</th>
                <th style="width:30px">ISO</th>
                <th style="width:30px">Cargo Type</th>
                <th style="width:100px">Weight</th>
                <th style="width:30px">LCL/FCL</th>
                <th style="width:50px">Seal 1</th>
                <th style="width:50px">Seal 2</th>
                <th style="width:50px">Seal 3</th>
                <th style="width:30px">POL</th>
                <th style="width:30px">FPOD</th>
                <th style="width:120px">Unload Date</th>
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
                <td><%#Eval("STOW POSITION")%></td>
                <td><%#Eval("LINE")%></td>
                <td><%#Eval("CONTAINER")%></td>
                <td><%#Eval("ISO")%></td>
                <td><%#Eval("CARGO TYPE")%></td>
                <td><%#Eval("WEIGHT")%></td>
                <td><%#Eval("TYPE")%></td>
                <td><%#Eval("SEAL_1")%></td>
                <td><%#Eval("SEAL_2")%></td>
                <td><%#Eval("SEAL_3")%></td>
                <td><%#Eval("POL")%></td>
                <td><%#Eval("FPOD")%></td>
                <td><%#Eval("UNLOAD DATE")%></td>
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
