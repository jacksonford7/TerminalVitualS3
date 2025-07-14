<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="~/Pago en Linea/ImprimirAnticipo.aspx.cs" Inherits="CSLSite.Pago_en_Linea.ImprimirAnticipo" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Imprimir Anticipo</title>
    <link href="../shared/estilo/aisv-general.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/print.css" rel="stylesheet" type="text/css"  />


    <script language="javascript" type="text/javascript">
// <![CDATA[
        function btclear_onclick() {
            window.print()
        }
// ]]>

    </script>

    <style type="text/css">

     .tabladata 
     {
                height:100%;  
                  width:100%!important; 
                  vertical-align:top;  
   
             } 
    
 table.costo{width:100%; font-size:small;}
 table.costo td { text-align:center; border:1px solid #CCC;  font-size:x-small;}
 table.costo th {text-align:center; border:1px solid #CCC; background-color:#F0f0f0;}


 
 .totales { width:100%; font-size:small;}
 .totales td { border-bottom:1px solid black; background-color:#F7F2E0;  font-weight:bold; font-size:small;}
 .totales .filat { text-align:right;}
 .estotal { width:60px; border:1px solid #CCC; background-color:White;  text-align:right; padding-left:4px;}
 .resaltar { color:Black!important;  font-family:Consolas;}  
 .dvtotal { margin:0; padding:0; border-left:1px solid #CCC; border-right:1px solid #CCC; font-size:x-small;}
 
    
    </style>

</head>
<body>
<div id="hoja">
     <div>
     <table class="imagenes" cellpadding="0" cellspacing="0">
     <tr>
     <td style="text-align:right">
         <img src="../shared/imgs/logoContecon.png" alt="logo" />
     </td>
     </tr>
     <tr><td style="text-align:center" colspan="2"><h2>REGISTRO DE ANTICIPO</h2></td></tr>
     </table>
     </div>
     <div class="seccion">
     <div class="accion">
     <div class="membrete">
     <table cellpadding="0" cellspacing="0">
     <tr><td class="bt-top" rowspan="4"><span id="servicio">Liquidación #</span></td>
     <td class="bt-top " rowspan="4"><span class="service" runat="server" id="anumber">0123456789</span></td>
        </table>
     </div>
     </div>
     </div>
     <div class="seccion">
      <div class="accion">

        <table class="xcontroles" cellspacing="0" cellpadding="0">
        <tr><th class="bt-bottom bt-right bt-top bt-left" colspan="2"> <i>DATOS RELACIONADOS 
            AL CLIENTE</i> 
        </th>
        </tr>
         <tr>
         <td class="bt-bottom  bt-right bt-left" >Registro Único de ContrIbuyente (RUC) </td>
         <td class="bt-bottom ">
           <span id="cliruc" class="labelprint" runat="server">000000</span>
         </td>
         </tr>

         <tr><td class="bt-bottom  bt-right bt-left">Nombre/Razón Social</td>
         <td class="bt-bottom" >
           <span id="clinombre" runat="server" class="labelprint">NOMBRE DE PRUEBAS</span>
        
         </td>
         </tr>
         <tr><td class="bt-bottom  bt-right bt-left">Número de Booking</td>
         <td class="bt-bottom" >
           <span id="cliNumeroBooking" runat="server" class="labelprint">NOMBRE DE PRUEBAS</span>
        
         </td>
         </tr>
         </table>
     </div>
    </div>
     <div class="seccion">
     <div class="accion">
         
      <table class="xcontroles" cellspacing="0" cellpadding="0">
        <tr><th class=" bt-top bt-bottom bt-right  bt-left"> Detalle del Anticipo</th></tr>
     

         </table>
         <div>
                  <asp:Repeater ID="tablaNueva" runat="server"  >
                 <HeaderTemplate>
                    <table class="costo"  cellpadding="1" cellspacing="1">
                    <thead>
                       <tr><th>Fecha Registro</th><th>Monto</th></tr>
                    </thead>
                 </HeaderTemplate>
                 <ItemTemplate>
                 <tr class="point" >
                  <td><%#Eval("FECHA_REGISTRO")%></td>
                  <td><%#Eval("MONTO_TOTAL")%></td>
                 </tr>
                  </ItemTemplate>
                 <FooterTemplate>
                 </tbody>
                 </table>
                 </FooterTemplate>
                </asp:Repeater>
           <div class="dvtotal">
         <table class="totales" cellpadding="0" cellspacing="0">
                <tr><td  class='filat'>TOTAL</td><td class="estotal"><span id='sttal' runat="server" clientidmode="Static" >$0000.00</span></td></tr>
          </table>
          </div>
         </div>
     <div class="informativo bt-left bt-right bt-bottom">
      <table cellpadding="0" cellspacing="0">
      <tr><td class="level1" >NOTA IMPORTANTE!</td></tr>
      <tr>
      <td class="level2 resaltar">
         Los valores detallados en la PROFORMA No. <span runat="server" id="numprofpie">00000000000</span>, son los que hasta la presente 
         fecha han sido generados por nuestro sistema. En consecuencia, los mismos pueden variar al momento de emitir la factura
         definitiva por los servicios prestados.
      </td>
      </tr>
      </table>
     </div>
     <div id="generacion" >
       <table class="poster" cellspacing="1" cellpadding="1">
         <tr>
         <td>Fecha de generación:</td>
         <td><span id="fechagenera" runat="server">19/05/2014 09:00</span></td>
         <td>Fecha de impresión:</td>
         <td><span id="fechaimprime" runat="server">19/05/2014 09:00</span></td>
         </tr>
       </table>
     </div>
     <div class="botonera" >
         <input id="btclear"   type="reset"  value="Imprimir" onclick="return btclear_onclick()" />
     </div>
     </div>
    </div>
</div>

</body>
</html>
