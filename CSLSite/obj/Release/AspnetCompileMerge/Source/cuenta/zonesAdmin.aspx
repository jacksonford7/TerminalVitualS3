
<%@ Page Title="Menu de opciones" Language="C#" MasterPageFile="~/site.Master" AutoEventWireup="true" CodeBehind="zonesAdmin.aspx.cs" Inherits="CSLSite.cuenta.zonesAdmin" %>
<asp:Content ID="Content1" ContentPlaceHolderID="placehead" runat="server">
    <link href="../shared/estilo/menu-old.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="placebody" runat="server">
<input id="zonaid" type="hidden" value="504" />
 <noscript><meta http-equiv="refresh" content="0; url=sinsoporte.htm" /> </noscript>
    <div id="capadivs" runat="server" class="clean">
     <div class="contorno">
         <div class="cleft">
         <table cellspacing ="2" cellpadding="1">
         <tr><td><img class="icono" src="../shared/imgs/aisv.png" alt="NoIcon" /></td>
         <td class="arriba" ><p class="t-left">Aqui viene el titulo</p></td>
         </tr>
         </table>
         </div>
         <div class="content-opciones">
             <a href="#" xtitle="Hola mundo nuevo"   onmouseover="displaytext(this,descripciones);">Texto de la opcion 1</a>
             <a href="#" xtitle="Hola mundo nuevo 2" onmouseover="displaytext(this,descripciones);">Texto de la opcion 2</a>
             <a href="#" xtitle="Hola mundo nuevo 3" onmouseover="displaytext(this,descripciones);">Texto de la opcion 3</a>
             <a href="#" xtitle="Hola mundo nuevo 4" onmouseover="displaytext(this,descripciones);">Texto de la opcion 4</a>
             <a href="#" xtitle="Hola mundo nuevo 5" onmouseover="displaytext(this,descripciones);">Texto de la opcion 5</a>
             <a href="#" xtitle="Hola mundo nuevo 6" onmouseover="displaytext(this,descripciones);">Texto de la opcion 6</a>
             <a href="#" xtitle="Hola mundo nuevo 7" onmouseover="displaytext(this,descripciones);">Texto de la opcion 7</a>
             <a href="#" xtitle="Hola mundo nuevo 8" onmouseover="displaytext(this,descripciones);">Texto de la opcion 8</a>
          </div>
         <div class="content-descripcion">
              Descripción
              <p class="p-descrip" id="descripciones">
                 Ponga el mouse sobre una de las opciones para ver su descripción
              </p>
          </div>
     </div>
 </div>
  <script type="text/javascript" >
      function displaytext(elemento, destino) {
          destino.textContent = elemento.getAttribute('xtitle');
      }
 </script>
</asp:Content>
