<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="facturaagencia_print_preview.aspx.cs" Inherits="CSLSite.reportes.facturaagencia_print_preview" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>Imprimir Factura</title>
     <link href="../css/print-style-billion.css" rel="stylesheet" type="text/css" />
    <link href="../css/print-billion.css" rel="stylesheet" type="text/css"  />
  
    <script language="javascript" type="text/javascript">
        function btclear_onclick() {
            window.print()
        }
    </script>
    <style type="text/css">
        .auto-style1 {
            font-size: 11px;
        }
    </style>
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
     <table width="100%" border="0" cellpadding="0" cellspacing="0">
       <!--DWLayoutTable-->
     <tr>
      <td width="700" height="150" valign="top">
        <table width="100%" border="0" cellpadding="0" cellspacing="0">
          <!--DWLayoutTable-->
          <tr>
            <td width="450" height="33" valign="top"><span class="Estilo1">Contecon Guayaquil S.A.</span></td>
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
          <tr>
            <td height="25"></td>
            </tr>
        </table></td>
    <td width="500" valign="top">
        <table width="100%"  border="0" cellpadding="0" cellspacing="0" >
          <!--DWLayoutTable-->
          <tr>
            <td width="564" height="34" valign="middle"><span class="Estilo10">Comprobante para pago No. </span></td>
              <td width="486" valign="middle"><div align="right" class="Estilo5"><span id="numero_factura" runat="server">...</span></div></td>
            </tr>
          <tr>
            <td height="25" valign="top"><span class="Estilo10">NUM LIQUIDACIÓN</span></td>
              <td valign="top"><div align="right" class="Estilo5"><span id="numero_liquidacion" runat="server">...</span></div></td>
            </tr>
          <tr>
            <td height="25" valign="top" class="Estilo10">EMISIÓN</td>
              <td valign="top"><div align="right" class="Estilo5"><span id="fecha_factura" runat="server">...</span></div></td>
            </tr>
          <tr>
            <td height="25" valign="top" class="Estilo10">VENCIMIENTO</td>
              <td valign="top"><div align="right" class="Estilo5"><span id="fecha_vencimiento" runat="server">...</span></div></td>
            </tr>
          
          <tr>
            <td height="25" colspan="2" valign="top"><div align="right" class="Estilo8">CONTRIBUYENTE ESPECIAL Nro. 870 </div></td>
            </tr>
             <tr>
            <td height="25" colspan="2" valign="top"><div align="right" class="Estilo8">OBLIGADO A LLEVAR CONTABILIDAD: SI</div></td>
            </tr>
             <tr>
            <td height="25" colspan="2" valign="top"><div align="right" class="Estilo8">EXPORTADOR HABITUAL DE SERVICIOS</div></td>
            </tr>
          <tr>
            <td height="16"></td>
            <td></td>
          </tr>
                                        </table></td>
    <td width="20">&nbsp;</td>
     </tr>
    </table>
</div>

 <%--<div>
     <table class="imagenes" cellpadding="0" cellspacing="0">
     <tr><td  colspan="2">&nbsp;</td></tr>
     </table>
    </div>--%>
    <%-- <div class="seccion">
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
     </div>--%>
     <div class="seccion">
      <div class="accion">
       <table class="xcontroles" cellspacing="0" cellpadding="0">
        <tr><th class="btx-bottom btx-right btx-top btx-left" colspan="6">CLIENTE</th>
        </tr>
         <tr>
            <td class="bt-bottom  bt-right bt-left">NOMBRE/RAZÓN SOCIAL: </td>
            <td class="bt-bottom" colspan="5"><span id="cliente" class="labelprint" runat="server">.... </span></td>
         </tr>
         <tr>
            <td class="bt-bottom  bt-right bt-left">DIRECCIÓN: </td>
            <td class="bt-bottom" colspan="5"><span id="direccion" class="labelprint" runat="server">.... </span></td>
         </tr>
         <tr>
            <td class="bt-bottom  bt-right bt-left">CIUDAD: </td>
            <td class="bt-bottom" colspan="3"> <span id="ciudad" class="labelprint" runat="server">.... </span></td>
            <td class="bt-bottom  bt-right bt-left">PROVINCIA: </td>
             <td class="bt-bottom"><span id="provincia" class="labelprint" runat="server">.... </span></td>
         </tr>
         <tr>
            <td class="bt-bottom  bt-right bt-left">RUC: </td>
            <td class="bt-bottom" colspan="5"><span id="ruc" class="labelprint" runat="server">.... </span></td>
         </tr>
        <tr><th class="bt-bottom bt-right bt-top bt-left" colspan="6">OBSERVACIONES</th>
        </tr>
         <tr>
            <td class="bt-bottom  bt-right bt-left">TOTAL CAJAS BODEGA FRIA/SECA: </td>
            <td class="bt-bottom" colspan="5"><span id="cajas_bodega" class="labelprint" runat="server">.... </span></td>
         </tr>
         <tr>
            <td class="bt-bottom  bt-right bt-left">TOTAL CAJAS MUELLE</td>
            <td class="bt-bottom" colspan="5"> <span id="cajas_muelle" class="labelprint" runat="server">.... </span></td>
          
          </tr>
         <tr>
             <td class="bt-bottom  bt-right bt-left" >REFERENCIA: </td>
            <td class="bt-bottom" colspan="5"><span id="referencia" class="labelprint" runat="server">.... </span></td>
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
                   <td colspan="3" rowspan="5" class="auto-style1">Este documento hace referencia a la FACTURA No 001-018-000396450 y no tiene validez legal alguna. <br/>
                       La factura autorizada por el SRI la podrá visualizar y descargar en nuestro portal e-billing ingresando al <br/>
                       siguiente link: http://contecongye.e-custodia.com.ec <br/><br/>
                       Debo y pagaré incondicionalmente a la orden de Contecon Guayaquil S.A. en el lugar y fecha que se me reconvenga,<br/>
                       el valor  total expresado  en este documento, más los  impuestos legales respectivos en  Dólares de <br/>
                       los Estados Unidos de América, por los bienes y/o servicios que he recibido a mi entera satisfacción.
                   </td>
                   <td align="right"><strong>SUBTOTAL</strong></td>
                   <td align="right"><strong><span id="Subtotal" runat="server" class="labelprint2">0.00</span></strong></td>
               </tr>
                 <tr>
                   <td align="right"><strong>IVA 0%</strong></td>
                   <td align="right"><strong><span id="IvaCero" runat="server" class="labelprint">0.00</span></strong></td>
               </tr>
               <tr>
                   <td align="right"><strong>IVA 12%</strong></td>
                   <td align="right"><strong><span id="Iva" runat="server" class="labelprint">0.00</span></strong></td>
               </tr>
                   <tr>                 
                   <td align="right"><strong>TOTAL</strong></td>
                   <td align="right"><strong><span id="Total" runat="server" class="labelprint">0.00</span></strong></td>
               </tr>
                <tr>
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
