<%@ Page Title="Vista preliminar" Language="C#" AutoEventWireup="true" CodeBehind="proforma_preview.aspx.cs" Inherits="CSLSite.proforma_preview" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Imprimir Proforma</title>
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
        //ok
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
     <tr><td  colspan="2"><span class="service" runat="server" id="num_proforma">PROFORMA # 1</span></td></tr>
     </table>
     </div>
     <div class="seccion">
     <div class="accion">
     <div class="membrete">
     <table cellpadding="0" cellspacing="0">
     <tr><td class="bt-top" rowspan="4"><span id="servicio">Referencia #</span></td>
     <td class="bt-top " rowspan="4"><span class="service" runat="server" id="anumber">0123456789</span></td>
     <td class="fondo bt-top bt-left bt-right" >&nbsp;&nbsp;&nbsp;</td>
     <td class="bt-top bt-right"><span runat="server" id="full" class="equis"  >(&nbsp;&nbsp;&nbsp;)</span></td></tr>
      <tr><td class="fondo bt-top">&nbsp;</td><td class="bt-right"><span runat="server" id="csuelta" class="equis"  >( &nbsp;&nbsp;&nbsp;)</span></td></tr>
      
        </table>
     </div>
     </div>
     </div>
     <div class="seccion">
      <div class="accion">
       <table class="xcontroles" cellspacing="0" cellpadding="0">
        <tr><th class="bt-bottom bt-right bt-top bt-left" colspan="6"> DETALLES DE LA NAVE</th>
        </tr>
         <tr>
         <td class="bt-bottom  bt-right bt-left" >Referencia  </td>
         <td class="bt-bottom ">
           <span id="referencia" class="labelprint" runat="server">.... </span>
         </td>
         <td class="bt-bottom  bt-right bt-left">Nombre de la nave</td>
         <td class="bt-bottom bt-right" colspan="1">
           <span id="buque" runat="server" class="labelprint">...</span>
         </td>
         </tr>
        <tr><th class="bt-bottom bt-right bt-top bt-left" colspan="6"> DATOS DE LA PROFORMA</th>
        </tr>
         <tr><td class="bt-bottom  bt-right bt-left">Fecha Proforma</td>
         <td class="bt-bottom" >
           <span id="FecProforma" runat="server" class="labelprint">...</span>
         </td>
                 <td class="bt-bottom  bt-right bt-left">Estado Proforma</td>
         <td class="bt-bottom ">
           <span id="EstProforma" runat="server" class="labelprint">...</span>
         </td>
 
         </tr>



         <tr><td class="bt-bottom  bt-right bt-left">Ruc Proveedor</td>
         <td class="bt-bottom" >
           <span id="ruc" runat="server" class="labelprint">...</span>
         </td>
                 <td class="bt-bottom  bt-right bt-left">Proveedor</td>
         <td class="bt-bottom ">
           <span id="proveedor" runat="server" class="labelprint">...</span>
         </td>
 
         </tr>

  
         <tr><td class="bt-bottom  bt-right bt-left">Generado por</td>
         <td class="bt-bottom" >
           <span id="generado" runat="server" class="labelprint">...</span>
         </td>
                 <td class="bt-bottom  bt-right bt-left">Observaciones</td>
         <td class="bt-bottom ">
           <span id="observacion" runat="server" class="labelprint">...</span>
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
                   <th>Grúa</th> 
                   <th>Concpeto</th>
                   <th>Inicio</th>
                   <th>Fin</th>
                   <th>Cantidad/Horas</th>
                   <th>Valor/Horas</th>
                   <th>Subtotal</th>
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
