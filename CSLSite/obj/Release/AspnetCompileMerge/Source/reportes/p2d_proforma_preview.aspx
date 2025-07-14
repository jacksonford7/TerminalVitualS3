<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="p2d_proforma_preview.aspx.cs" Inherits="CSLSite.reportes.p2d_proforma_preview" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>Imprimir Proforma</title>
     <link href="../css/print-style-billion.css" rel="stylesheet" type="text/css" />
    <link href="../css/print-billion.css" rel="stylesheet" type="text/css"  />
  
    <script language="javascript" type="text/javascript">
        function btclear_onclick() {
            window.print()
        }
    </script>
</head>
<body>
  <div id="hoja" class="agua-cgsa">
     <div>
     <table class="imagenes" cellpadding="0" cellspacing="0">
     <tr>
     <td>
         <img src="../img/logo_01.jpg" alt="logo" />
     </td>
     <td class="aright">
         &nbsp;&nbsp;
     </td>
     </tr>
     </table>
     </div>
     
      <div>
     <table width="399" border="0" cellpadding="0" cellspacing="0">
  <tr>
    <td width="399" height="33" valign="top"><span class="Estilo1">Contecon Guayaquil S.A.</span></td>
  </tr>
  <tr>
    <td height="23" valign="top"><span class="Estilo2">R.U.C.:   0992506717001</span></td>
  </tr>
  <tr>
    <td height="24" valign="top"><span class="Estilo3">Matriz VIA AL PUERTO MARITIMO AV. DE LA MARINA S/N</span></td>
  </tr>
  <tr>
    <td height="22" valign="top"><span class="Estilo3">PBX:(593)46006300 (593)43901700</span></td>
  </tr>
  <tr>
    <td height="23" valign="top"><span class="Estilo3">Guayaquil - Ecuador</span></td>
  </tr>
</table>
     </div>
       <div>
     <table class="imagenes" cellpadding="0" cellspacing="0">
     <tr><td  colspan="2"><span class="service"  runat="server" id="numero_proforma">PROFORMA # 1</span></td></tr>
     </table>
    </div>
     <div class="seccion">
     <div class="accion">
     <div >
     <table class="division" cellpadding="0" cellspacing="0">
     <tr>
     <td  class="division division-td1">&nbsp;EMISION:</td>
     <td  class="division division-td2"><span  runat="server" id="fechaemision">05/12/2019 12:00</span></td>  
     </tr>
    </table>
     </div>
     </div>
     </div>
     <div class="seccion">
      <div class="accion">
       <table class="bartitulos" cellspacing="0" cellpadding="0">
        <tr>
            <th class="btx-bottom btx-right btx-top btx-left" colspan="6">NOMBRE/RAZÓN SOCIAL</th>
        </tr>
         <tr>
         <td class="bt-bottom  bt-right bt-left" colspan="3"><span id="cliente" class="labelprint" runat="server">.... </span></td>

         </tr>
         <tr>
            <th class="btx-bottom btx-right btx-top btx-left" colspan="6">TIPO SERVIIO</th>
        </tr>
         <tr>
         <td class="bt-bottom  bt-right bt-left" colspan="3"><span id="tiposervicio" class="labelprint" runat="server">.... </span></td>
         </tr>
        <tr><th class="bt-bottom bt-right bt-top bt-left" colspan="6">DIRECCIÓN ENTREGA</th>
        </tr>
         <tr><td class="bt-bottom  bt-right bt-left" colspan="3"><span id="ciudad" runat="server" class="labelprint">...</span></td></tr>
         <tr><td class="bt-bottom  bt-right bt-left" colspan="3"><span id="zona" runat="server" class="labelprint">...</span></td></tr>
         <tr><td class="bt-bottom  bt-right bt-left" colspan="3"><span id="direccion" runat="server" class="labelprint">...</span></td></tr>


  <tr><th class="bt-bottom bt-right bt-top bt-left" colspan="6">NÚMERO DE CARGA</th>
        </tr>
         <tr><td class="bt-bottom  bt-right bt-left" colspan="3"> <span id="numero_carga" runat="server" class="labelprint">...</span></td>
         </tr>
        
         </table>
     </div>
    </div>
     <div class="seccion">
     <div class="accion">
    <table class="xcontroles tabladata" cellspacing="0" cellpadding="0" >
        <tr><th colspan="3" class="bt-bottom bt-right  bt-left" > DETALLE DE SERVICIOS</th></tr> 
                 </table>
       <div class="pdiv bt-left bt-right" runat="server" id="detalle_data" clientidmode="Static">
           <table class='print_table'>
               <thead>
               <tr>
                   <td><strong>Código</strong></td> 
                   <td><strong>Descripción</strong></td> 
                   <td align='right'><strong>Cantidad</strong></td>
                   <td align='right'><strong>V.Unit.</strong></td>
                   <td align='right'><strong>V.Total</strong></td>
               </tr>
               </thead>
               <tbody>
                   <tr>
                   <td>...</td>
                   <td>...</td>
                   <td align="right">...</td>
                   <td align="right">...</td>
                   <td align="right">...</td>
                   </tr>
               </tbody>
               <tfoot>
              
               <tr >
                   <td colspan="5"><div class="lineadiv"></div></td>
               </tr>
                <tr >
                   <td></td>
                   <td></td>
                   <td></td>
                   <td></td>
                   <td></td>
               </tr>
               <tr>
                   <td></td>
                   <td>Generado por: </td>
                   <td></td>
                   <td align="right"><strong>SUBTOTAL $</strong></td>
                   <td align="right"><strong><span id="Subtotal" runat="server" class="labelprint2">0.00</span></strong></td>
               </tr>
                 <tr>
                   <td></td>
                   <td></td>
                   <td></td>
                   <td align="right"><strong>IVA 0%</strong></td>
                   <td align="right"><strong><span id="IvaCero" runat="server" class="labelprint">0.00</span></strong></td>
               </tr>
               <tr>
                   <td></td>
                   <td></td>
                   <td></td>
                   <td align="right"><strong>IVA 12%</strong></td>
                   <td align="right"><strong><span id="Iva" runat="server" class="labelprint">0.00</span></strong></td>
               </tr>
                   <tr>                 
                   <td></td>
                   <td></td>
                   <td></td>
                   <td align="right"><strong>TOTAL $</strong></td>
                   <td align="right"><strong><span id="Total" runat="server" class="labelprint">0.00</span></strong></td>
               </tr>
                <tr>
                   <td></td>
                   <td></td>
                   <td></td>
                   <td></td>
                   <td></td>
               </tr>

               </tfoot>
           </table>

       </div>

     <div id="generacion" >
       <table class="poster" cellspacing="1" cellpadding="1">
         <tr>
         <td>Fecha de generación:</td>
         <td><span id="fechagenera" runat="server">...</span></td>
         <td>Fecha de impresión:</td>
         <td><span id="fechaimprime" runat="server">...</span></td>
         </tr>
       </table>
     </div>
     <div class="botonera" >
         <input id="btclear"   type="reset"    value="Imprimir" onclick="return btclear_onclick()" />
     </div>
     </div>
    </div>
</div>

</body>
</html>
