<%@ Page Title="Menu de opciones" Language="C#" MasterPageFile="~/site.Master" AutoEventWireup="true" CodeBehind="zones.aspx.cs" Inherits="CSLSite.cuenta.zones" %>


<asp:Content ID="Content1" ContentPlaceHolderID="placehead" runat="server">
 <link href="../shared/avisos/ppt.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/menu-old.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="placebody" runat="server">
<input id="zonaid" type="hidden" value="503" />
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

 <input type="hidden"  id="ruc_cliente" runat="server" clientidmode="Static"/>
<script type='text/javascript'>
    function viewModal() {
        $(document).ready(function () {
            $('#ventana_popup').fadeIn('slow');
            $('#popup-overlay').fadeIn('slow');
            $('#popup-overlay').height($(window).height());
             document.getElementById('cliente_ruc').textContent = document.getElementById('ruc_cliente').value;
        });
    } 
</script>

     <div id="ventana_popup" style="display: none;">
    <div id="ventana_content-popup">
        <div>
		 <p id='manage_ventana_popup'> Aviso de Bloqueo!</p>
		    <div id='borde_ventana_popup'> 
			 Estimado Cliente:
             <br /> 
            Usted mantiene valores vencidos en Contecon Guayaquil S.A., por lo que es necesario que regularice los pagos de manera inmediata.  Los accesos al S3 estarán restringidos,  se le permitirá realizar consultas, elaborar liquidaciones anticipadas o realizar compensaciones de facturas.
             
              <br />
             Favor contactar a la brevedad posible con nuestro Dpto. Financiero, a la dirección electrónica : 
               <a href="mailto:CGSA-Tesoreria@cgsa.com.ec?Subject=Bloqueo por pago" >
              CGSA-Tesoreria@cgsa.com.ec 
              </a> 
			o al PBX 593 (04) 6006300 ext. 8016 – 8017 – 8019 – 8044 – 8045 
            <br/>
             RUC:  <span id='cliente_ruc'></span>
			 <div  id="close" >SALIR</div>
  			</div>
        </div>
    </div>
   </div>
   <div id="popup-overlay" style="display: none;"></div>

<script src="../shared/avisos/popup_script_home.js" type="text/javascript"></script>
</asp:Content>

