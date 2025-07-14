<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="plan_preview.aspx.cs" Inherits="CSLSite.plan_preview" Title="Vista preliminar" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Imprimir AISV</title>
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
     <tr><td colspan="2"><h2>PLANIFICACIÓN DE TRABAJO</h2>  </td></tr>
     </table>
     </div>
     <div class="seccion">
     <div class="accion">
     <div class="membrete">
     <table cellpadding="0" cellspacing="0">
     <tr><td class="bt-top" rowspan="4"><span id="servicio">Referencia #</span></td>
     <td class="bt-top " rowspan="4"><span class="service" runat="server" id="anumber">0123456789</span></td>
     <td class="fondo bt-top bt-left bt-right">&nbsp;&nbsp;Contenedores</td>
     <td class="bt-top bt-right"><span runat="server" id="full" class="equis"  >(&nbsp;&nbsp;&nbsp;)</span></td></tr>
      <tr><td class="fondo bt-top">Carga general</td><td class="bt-right"><span runat="server" id="csuelta" class="equis"  >( &nbsp;&nbsp;&nbsp;)</span></td></tr>
      
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

         <tr><td class="bt-bottom  bt-right bt-left">Fecha estimada de arribo [ETA]</td>
         <td class="bt-bottom" >
           <span id="eta" runat="server" class="labelprint">...</span>
         </td>
                 <td class="bt-bottom  bt-right bt-left">Fecha estimada de zarpe [ETD]</td>
         <td class="bt-bottom ">
           <span id="etd" runat="server" class="labelprint">...</span>
         </td>
 
         </tr>



         <tr><td class="bt-bottom  bt-right bt-left">Fecha actual de arribo [ATA]</td>
         <td class="bt-bottom" >
           <span id="ata" runat="server" class="labelprint">...</span>
         </td>
                 <td class="bt-bottom  bt-right bt-left">Fecha actual de zarpe [ATD]</td>
         <td class="bt-bottom ">
           <span id="atd" runat="server" class="labelprint">...</span>
         </td>
 
         </tr>

  
         <tr><td class="bt-bottom  bt-right bt-left">Fecha inicio de trabajos</td>
         <td class="bt-bottom" >
           <span id="wini" runat="server" class="labelprint">...</span>
         </td>
                 <td class="bt-bottom  bt-right bt-left">Fecha fin de trabajos</td>
         <td class="bt-bottom ">
           <span id="wend" runat="server" class="labelprint">...</span>
         </td>
 
         </tr>

         <tr><td class="bt-bottom  bt-right bt-left">Muelle</td>
         <td class="bt-bottom" >
           <span id="muelle" runat="server" class="labelprint">...</span>
         </td>
                 <td class="bt-bottom  bt-right bt-left">Fecha citación</td>
         <td class="bt-bottom ">
           <span id="citacion" runat="server" class="labelprint">...</span>
         </td>
 
         </tr>


         </table>
     </div>
    </div>
     <div class="seccion">
     <div class="accion">
          <table class="xcontroles tabladata" cellspacing="0" cellpadding="0" >
        <tr><th colspan="3" class="bt-bottom bt-right  bt-left" > DETALLE DE LAS GRÚAS</th></tr> 
                 </table>
       <div class="pdiv bt-left bt-right" runat="server" id="grua_data" clientidmode="Static">
           <table class='print_table'>
               <thead>
               <tr>
                   <th>Grúa</th> <th>Trabajo (Hrs)</th> <th>Inicio</th><th>Fin</th>
               </tr>
               </thead>
               <tbody>
                   <tr>
                   <td>...</td>
                   <td>...</td>
                   <td>...</td>
                   <td>...</td>
                   </tr>
               </tbody>
           </table>

       </div>
 

     


  
   
      

      <table class="xcontroles" cellspacing="0" cellpadding="0">
        <tr><th class=" bt-top bt-bottom bt-right  bt-left" colspan="4"> Detalle de LOS TURNOS</th></tr>
         </table>

                <div class="pdiv bt-left bt-right" runat="server" id="turno_data" clientidmode="Static" >
           <table class='print_table'>
               <thead>
               <tr>
                   <th>Grúa</th> <th>Trabajo (Hrs)</th> <th>Inicio</th><th>Fin</th>
               </tr>
               </thead>
               <tbody>
                   <tr>
                   <td>...</td>
                   <td>...</td>
                   <td>...</td>
                   <td>...</td>
                   </tr>
               </tbody>
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
