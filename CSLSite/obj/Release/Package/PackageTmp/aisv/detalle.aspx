<%@ Page Language="C#" Title="Catálogo de agentes" AutoEventWireup="true" CodeBehind="detalle.aspx.cs" Inherits="CSLSite.detalleaisv" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Catálogo de agentes</title>
<link href="../shared/estilo/catalogos.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/work.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/aisv-general.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/tables.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/print.css" rel="stylesheet" type="text/css" />
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
    <div class="catabody" style=" height:700px; overflow:auto; ">
       <div class="catawrap"   >
   
  
                <h2>Detalle de los documentos de exportación</h2><br />
                <span runat="server" id="aisv">Número de AISV:20140002333</span>
     
        
  
         <div class="cataresult"  >
             <div class="booking"  >
                  <div class="separator">Documentos de Exportación</div>
                  
                 <asp:Repeater ID="tablePagination" runat="server" >
                 <HeaderTemplate>
                 <table id="tablasort" cellspacing="1" cellpadding="1" class="tabRepeat">
                 <tr>
                 <th>Documento No.</th>
                 <th>Tipo</th>
                 <th>Bultos</th>
                 <th>Peso (KG)</th>
                 <th>Embalaje</th>
                 <th>Num. Entrega</th>
                 <th>Estado</th>
                 </tr>
                 </HeaderTemplate>
                 <ItemTemplate>
                  <tr  >
                  <td><%#Eval("adudoc")%></td>
                  <td><%#Eval("tipodoc")%></td>
                  <td><%#Eval("bultos")%></td>
                  <td><%#Eval("peso")%></td>
                  <td><%#Eval("embalaje")%></td>
                  <td><%#Eval("entrega")%></td>
                  <td><%#Eval("estado")%></td>
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
