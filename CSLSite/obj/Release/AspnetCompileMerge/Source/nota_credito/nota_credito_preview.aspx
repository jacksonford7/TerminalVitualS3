<%@ Page Title="Vista preliminar Nota Crédito" Language="C#" AutoEventWireup="true" CodeBehind="nota_credito_preview.aspx.cs" Inherits="CSLSite.nota_credito_preview" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Imprimir Nota Crédito</title>
    <link href="../shared/estilo/aisv-general.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/print.css" rel="stylesheet" type="text/css"  />
    <link href="../shared/avisos/marcas.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/opc_tables.css" rel="stylesheet" />
    <script language="javascript" type="text/javascript">
// <![CDATA[
        function btclear_onclick() {
            window.print()
        }
// ]]>

    </script>



    <style type="text/css">
        .auto-style4 {
            width: 379px;
        }
        .auto-style5 {
            width: 258px;
        }
        .auto-style6 {
            width: 475px;
        }
        .auto-style7 {
            width: 305px;
        }
    </style>



</head>
<body>
<div id="hoja" class="agua-cgsa">
     <div>
     <table class="imagenes" cellpadding="0" cellspacing="0">
     <tr>
     <td>
         <img src="../shared/imgs/logoContecon.png" alt="logo" />
     </td>
     <td class="aright">
         <asp:Image ID="barras" runat="server" Height="60px" Width="200px" AlternateText="Servicio de código de barras no disponible" />&nbsp;&nbsp;
     </td>
     </tr>
     <tr><td  colspan="2"><span class="service" runat="server" id="num_nota_credito">NOTA CREDITO # 1</span></td></tr>
     </table>
     </div>
     <div class="seccion">
     <div class="accion">
     <div class="membrete">
     <table cellpadding="0" cellspacing="0">
     <tr><td class="bt-top" rowspan="4"><span id="servicio">APLICA FACTURA #</span></td>
     <td class="bt-top " rowspan="4"><span class="service" runat="server" id="numero_factura">1019000245146</span></td>
     <td class="fondo bt-top bt-left bt-right" ></td>
     </tr>
      <tr><td class="fondo bt-top">&nbsp;</td></tr>
      
        </table>
     </div>
     </div>
     </div>
     <div class="seccion">
      <div class="accion">
       <table class="xcontroles" cellspacing="0" cellpadding="0">
        <tr><th class="bt-bottom bt-right bt-top bt-left" colspan="6"> DATOS DEL CLIENTE</th>
        </tr>
         <tr>
         <td class="bt-bottom  bt-right bt-left" >Cliente</td>
         <td class="bt-bottom " colspan="3" >
           <span id="cliente" class="labelprint" runat="server">.... </span>
         </td>
       
         </tr>
        <tr><th class="bt-bottom bt-right bt-top bt-left" colspan="6"> DATOS NOTA CREDITO</th>
        </tr>
         <tr><td class="bt-bottom  bt-right bt-left">Fecha emisión</td>
         <td class="bt-bottom" colspan="3" >
           <span id="fecha_emision" runat="server" class="labelprint">...</span>
         </td>
  
         </tr>



         <tr><td class="bt-bottom  bt-right bt-left">Concepto</td>
         <td class="bt-bottom" colspan="3" >
           <span id="concepto" runat="server" class="labelprint">...</span>
         </td>
         </tr>

  
         <tr><td class="bt-bottom  bt-right bt-left">Glosa</td>
         <td class="bt-bottom" colspan="3" >
           <span id="glosa" runat="server" class="labelprint">...</span>
         </td>
         </tr>
          <tr><td class="bt-bottom  bt-right bt-left">Generado por</td>
         <td class="bt-bottom" colspan="3" >
           <span id="generado_por" runat="server" class="labelprint">...</span>
         </td>
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
                   <th>#</th> 
                   <th>Cód. Servicio</th> 
                   <th>Servicio</th>
                   <th>Cantidad</th>
                   <th>Precio</th>
                   <th>Subtotal</th>
                   <th>Iva</th>
                   <th>Carga</th>
               </tr>
               </thead>
               <tbody>
                   <tr>
                   <td>...</td>
                   <td>...</td>
                   <td>...</td>
                   <td>...</td>
                   <td>...</td>
                   <td>...</td>
                   <td>...</td>
                   <td>...</td>
                   </tr>
               </tbody>
               <tfoot>
               <tr>
                   <th></th> 
                   <th></th> 
                   <th></th>
                   <th></th>
                   <th></th>
                   <th></th>
                   <th>SUBTOTAL $</th>
                   <th><span id="Subtotal" runat="server" class="labelprint">0.00</span></th>
               </tr>
                   <tr>
                   <th></th> 
                   <th></th> 
                   <th></th>
                   <th></th>
                   <th></th>
                   <th></th>
                   <th>IVA $</th>
                   <th><span id="Iva" runat="server" class="labelprint">0.00</span></th>
               </tr>
               
                   <tr>
                   <th></th> 
                   <th></th> 
                   <th></th>
                   <th></th>
                   <th></th>
                   <th></th>
                   <th>TOTAL $</th>
                   <th><span id="Total" runat="server" class="labelprint">0.00</span></th>
               </tr>
               </tfoot>
           </table>

       </div>
 

     


  
   
      

      


     <div id="generacion" >
       <table class="poster" cellspacing="1" cellpadding="1">
         <tr>
         <td colspan="1">
           <asp:Image ID="barcode2" runat="server" Height="50px" Width="200px" AlternateText="Servicio de código de barras no disponible" />
           <span id="numcontenedor" runat="server">...</span>
           </td>
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
