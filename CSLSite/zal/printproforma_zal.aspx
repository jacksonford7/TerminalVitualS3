<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="printproforma_zal.aspx.cs" Inherits="CSLSite.printproforma_zal" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Imprimir documento</title>
    <link href="../shared/estilo/aisv-general.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/print.css" rel="stylesheet" type="text/css"  />


    <script language="javascript" type="text/javascript">
// <![CDATA[
        function btclear_onclick() {
            window.print()
        }
// ]]>

    </script>
        <link rel="stylesheet" type="text/css" href="../shared/avisos/marcas.css" />
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
<div id="hoja" runat="server" clientidmode="Static">
     <div>
     <table class="imagenes" cellpadding="0" cellspacing="0">
     <tr>
     <td>
         <img src="../shared/imgs/logoContecon.png" alt="logo" />
     </td>
     <td class="aright">
         <asp:Image ID="barras" runat="server" Height="60px" Width="400px" AlternateText="Servicio de código de barras no disponible" />&nbsp;&nbsp;
     </td>
     </tr>
     <tr><td colspan="2"><h2>  <span id='pro_li' runat="server" clientidmode="Static">PROFORMA POR RESERVA DE TURNO</span>  </h2></td></tr>
     </table>
     </div>
     <div class="seccion">
     <div class="accion">
     <div class="membrete">
     <table cellpadding="0" cellspacing="0">
     <tr><td class="bt-top" rowspan="4"><span id="servicio" runat="server">Proforma Por Reserva De Turno #</span></td>
     <td class="bt-top " rowspan="4"><span class="service" runat="server" id="anumber">0123456789</span></td>
     <td class="fondo bt-top bt-left bt-right">&nbsp;&nbsp;Contenedor Vacío</td>
     <td class="bt-top bt-right"><span runat="server" id="full" class="equis"  >(&nbsp;&nbsp;&nbsp;)</span></td></tr>
      <tr><td class="fondo bt-top"></td><td class="bt-right"><span runat="server" id="x1" class="equis"  >&nbsp;&nbsp;&nbsp;</span></td></tr>
      <tr><td class="fondo bt-top"></td><td class="bt-right"><span runat="server" id="x2" class="equis"  > &nbsp;&nbsp;&nbsp;</span></td></tr>
      <tr><td class="fondo bt-top"></td><td class="bt-right"><span runat="server" id="x3" class="equis"  > &nbsp;&nbsp;&nbsp;</span></td></tr>
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
           <span id="cliruc" class="labelprint" runat="server">000000000000000000000</span>
         </td>
         </tr>

         <tr><td class="bt-bottom  bt-right bt-left">Nombre/Razón Social</td>
         <td class="bt-bottom" >
           <span id="clinombre" runat="server" class="labelprint">NOMBRE DE PRUEBAS</span>
        
         </td>
         </tr>
             <tr><td class="bt-bottom  bt-right bt-left">Fecha Emisión</td>
         <td class="bt-bottom" >
           <span id="sfecha_cliente" runat="server" class="labelprint">.</span>
        
         </td>
         </tr>
             <tr><td class="bt-bottom  bt-right bt-left">Depósito</td>
         <td class="bt-bottom" >
           <span id="depot" runat="server" class="labelprint">.</span>
        
         </td>
         </tr>

         </table>


       <table class="xcontroles" cellspacing="0" cellpadding="0">
        <tr><th class="bt-bottom bt-right bt-top bt-left" colspan="4"> Datos del <i>Booking</i> 
        </th>
        </tr>
         <tr>
         <td class="bt-bottom  bt-right bt-left" >Número</td>
         <td class="bt-bottom ">
           <span id="bonum" class="labelprint" runat="server">0000000000 </span>
         </td>
         <td class="bt-bottom  bt-right bt-left">Referencia <i>CONTECON</i></td>
         <td class="bt-bottom bt-right">
           <span id="boref" runat="server" class="labelprint">RRR0000</span>
         </td>
         </tr>

         <tr><td class="bt-bottom  bt-right bt-left">Nombre de la nave</td>
         <td class="bt-bottom" colspan="3" >
           <span id="bonom" runat="server" class="labelprint">NOMBRE DEMOSTRACIÓN</span>
  
         </td>
         </tr>
        <tr>
         <td class="bt-bottom  bt-right bt-left" >Reservas</td>
         <td class="bt-bottom ">
           <span id="bkres" class="labelprint" runat="server">10 </span>
         </td>
         <td class="bt-bottom  bt-right bt-left">Total de Contenedores:</td>
         <td class="bt-bottom bt-right">
           <span id="bkcntr" runat="server" class="labelprint">AAAA0000000</span>
         </td>
         </tr>

         <tr>
         <td class="bt-bottom  bt-right bt-left" ></td>
         <td class="bt-bottom ">
           <span id="sfecha_cliente2" class="labelprint" runat="server"></span>
         </td>
         <td class="bt-bottom  bt-right bt-left"></td>
         <td class="bt-bottom bt-right">
           <span id="sfecha_zarpe2" runat="server" class="labelprint"></span>
         </td>
         </tr>

         </table>
     </div>
    </div>
     <div class="seccion">
     <div class="accion">
         
   
      

      <table class="xcontroles" cellspacing="0" cellpadding="0">
        <tr><th class=" bt-top bt-bottom bt-right  bt-left"> Detalle de lOS SERVICIOS</th></tr>
     

         </table>
         <div>
                  <asp:Repeater ID="tablaNueva" runat="server"  >
                 <HeaderTemplate>
                    <table class="costo"  cellpadding="1" cellspacing="1">
                    <thead>
                       <tr><th>Código</th><th>Descripcion</th><th>Cant.</th><th>V.Unit</th><th>V.Total</th></tr>
                    </thead>
                 </HeaderTemplate>
                 <ItemTemplate>
                 <tr class="point" >
                  <td><%#Eval("codigoser")%></td>
                  <td><%#Eval("describe")%></td>
                  <td><%#Eval("cantidad")%></td>
                  <td><%#DataBinder.Eval(Container.DataItem, "unitario", "{0:C}")%></td>
                  <td><%#DataBinder.Eval(Container.DataItem, "totalfila", "{0:C}")%></td>
                 </tr>
                  </ItemTemplate>
                 <FooterTemplate>
                 </tbody>
                 </table>
                 </FooterTemplate>
                </asp:Repeater>
           <div class="dvtotal">
         <table class="totales" cellpadding="0" cellspacing="0">
               
                <tr><td  class='filat'>Subtotal:</td><td class="estotal"><span id='subtotal' runat="server" clientidmode="Static"  >$0000.00</span></td></tr>
                <tr><td  class='filat'> <span style="color:blue;" runat="server" clientidmode="Static" id="desiva">IVA %(+)</span> </td><td class="estotal"><span style="color:blue;" id='iva' runat="server" clientidmode="Static"  >$0000.00</span></td></tr>

                <tr><td  class='filat'>Neto a Pagar:</td><td class="estotal"><span id='sttal' runat="server" clientidmode="Static" >$0000.00</span></td></tr>
          </table>
          </div>
         </div>
     <div class="informativo bt-left bt-right bt-bottom">
      <table cellpadding="0" cellspacing="0">
      <tr><td class="level1" >NOTA IMPORTANTE!</td></tr>
      <tr>
      <td class="level2 resaltar">
         Los valores detallados en la PROFORMA/LIQUIDACIÓN No. <span runat="server" id="numprofpie">00000000000</span>, son los que hasta la presente 
         fecha han sido generados por nuestro sistema. En consecuencia, los mismos pueden variar al momento de emitir la factura
         definitiva por los servicios prestados.
         <p><strong>Importante: </strong>Recuerde que la liquidaci&oacute;n debe estar pagada  hasta 2 horas antes del turno seleccionado para que se active y pueda ser  utilizado.</p>
         <p id="fmensaje" runat="server" clientidmode="Static" >
        
         </p>

      </td>
      </tr>
      </table>
     </div>
     <div id="generacion" >
       <table class="poster" cellspacing="1" cellpadding="1">
         <tr>
         <td colspan="1">
           <asp:Image ID="barcode2" runat="server" Height="50px" Width="400px" AlternateText="Servicio de código de barras no disponible" />
           </td>
         <td>Fecha de generación:</td>
         <td><span id="fechagenera" runat="server">19/05/2014 09:00</span></td>
         <td>Fecha de impresión:</td>
         <td><span id="fechaimprime" runat="server">19/05/2014 09:00</span></td>
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
