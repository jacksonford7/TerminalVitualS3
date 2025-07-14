<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="p2d_resumen_preview_freightforwarder.aspx.cs" Inherits="CSLSite.reportes.p2d_resumen_preview_freightforwarder" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>Imprimir Factura - MultiDespacho</title>
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
            <td width="564" height="34" valign="middle">
                <span class="Estilo10">MULTIDESPACHO No. </span></td>
              <td width="486" valign="middle"><div align="right" class="Estilo5"><span id="numero_factura" runat="server">...</span></div></td>
            </tr>
          <tr>
            <td height="25" valign="top"><span class="Estilo10">EMISIÓN</span></td>
              <td valign="top"><div align="right" class="Estilo5"><span id="fecha_factura" runat="server">...</span></div></td>
            </tr>
            

          <tr>
            <td height="25" colspan="2" valign="top"><div align="right" class="Estilo8">CONTRIBUYENTE ESPECIAL </div></td>
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

 
     <div class="seccion">
      <div class="accion">
       
          <table class="xcontroles" cellspacing="0" cellpadding="0">
        <tr><th class="btx-bottom btx-right btx-top btx-left" colspan="6">FACTURA</th>
        </tr>
         <tr>
            <td class="bt-bottom  bt-right bt-left"># FACTURA: </td>
            <td class="bt-bottom" colspan="5"><span id="numero_factura_servicio" class="labelprint" runat="server">.... </span></td>
         </tr>
        
        
         </table>

       <table class="xcontroles" cellspacing="0" cellpadding="0">
        <tr><th class="btx-bottom btx-right btx-top btx-left" colspan="6">AGENTE</th>
        </tr>
         <tr>
            <td class="bt-bottom  bt-right bt-left">NOMBRE/RAZÓN SOCIAL: </td>
            <td class="bt-bottom" colspan="5"><span id="cliente" class="labelprint" runat="server">.... </span></td>
         </tr>
        
         <tr>
            <td class="bt-bottom  bt-right bt-left">RUC: </td>
            <td class="bt-bottom" colspan="5"><span id="ruc" class="labelprint" runat="server">.... </span></td>
         </tr>
         </table>

          <table class="xcontroles" cellspacing="0" cellpadding="0">
        <tr><th class="btx-bottom btx-right btx-top btx-left" colspan="6">TRANSPORTISTA</th>
        </tr>
         <tr>
            <td class="bt-bottom  bt-right bt-left">NOMBRE/RAZÓN SOCIAL: </td>
            <td class="bt-bottom" colspan="5"><span id="id_transportista" class="labelprint" runat="server">.... </span></td>
         </tr>
        
        
         <tr><td class="bt-bottom  bt-right bt-left" colspan="6"> <span id="Span3" runat="server" class="labelprint"></span></td>
         </tr>   
         </table>


     </div>
    </div>
     <div class="seccion">
     <div class="accion">
    <table class="xcontroles tabladata" cellspacing="0" cellpadding="0" >
        <tr><th colspan="3" class="bt-bottom bt-right  bt-left" > DETALLE DE PASES</th></tr> 
                 </table>
       <div class="pdiv bt-left bt-right" runat="server" id="detalle_data" clientidmode="Static">
           <table class='print_table'>
               <thead>
               <tr>
                   <td align='center'><strong># PASE</strong></td> 
                   <td align='center'><strong># CARGA</strong></td> 
                   <td align='center'><strong>TRANSPORTE</strong></td>
                   <td align='center'><strong>FECHA SALIDA</strong></td>
                   <td align='center'><strong>TURNO</strong></td>
                   <td align='center'><strong>DIRECCION</strong></td>
               </tr>
               </thead>
               <tbody>
                   <tr>
                   <td>...</td>
                   <td>...</td>
                   <td align="right">...</td>
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
                   <td></td>
               </tr>
              
   

               </tfoot>
           </table>

       </div>
          <div class="pdiv bt-left bt-right" runat="server" id="Div1" clientidmode="Static">
              <table class="xcontroles" cellspacing="0" cellpadding="0">
               <tr>
                     <td align='center'>  &nbsp;&nbsp;  &nbsp;&nbsp;  &nbsp;&nbsp;   &nbsp;&nbsp;  &nbsp;&nbsp;  &nbsp;&nbsp;  &nbsp;&nbsp;  &nbsp;&nbsp;  &nbsp;&nbsp;  &nbsp;&nbsp;  &nbsp;&nbsp;
                         &nbsp;&nbsp;  &nbsp;&nbsp;  &nbsp;&nbsp;   &nbsp;&nbsp;  &nbsp;&nbsp;  &nbsp;&nbsp;  &nbsp;&nbsp;  &nbsp;&nbsp;  &nbsp;&nbsp;  &nbsp;&nbsp;  &nbsp;&nbsp;
                     <asp:Image ID="barras" runat="server" Height="60px" Width="500px" AlternateText="Servicio de código de barras no disponible" />
                    </td>
             </tr>
                   <tr>
                     <td align='center'>
                     <span id="barra_id" class="Estilo1" runat="server">...</span>
                    </td>
             </tr>
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
