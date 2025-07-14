<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="printaisvsav.aspx.cs" Inherits="CSLSite.printaisvsav" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Imprimir Turno</title>
    <link href="../shared/estilo/aisv-general.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/print.css" rel="stylesheet" type="text/css"  />
    <link href="../shared/avisos/marcas.css" rel="stylesheet" type="text/css" />
    <script language="javascript" type="text/javascript">
// <![CDATA[
        function btclear_onclick() {
            window.print()
        }
// ]]>

    </script>

    <style type="text/css">

     .tabladata{ height:100%;  
                  width:100%!important; 
                  vertical-align:top;  
   
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
         <asp:Image ID="barras" runat="server" Height="60px" Width="400px" AlternateText="Servicio de código de barras no disponible" />&nbsp;&nbsp;
     </td>
     </tr>
     <tr><td colspan="2"><h2>Servicio de Administración de Vacíos</h2>  </td></tr>
     </table>
     </div>
     <div class="seccion">
     <div class="accion">
     <div class="membrete">
     <table cellpadding="0" cellspacing="0">
     <tr><td class="bt-top" rowspan="4"><span id="servicio">Secuencia #</span></td>
     <td class="bt-top " rowspan="4"><span class="service" runat="server" id="anumber">0123456789</span></td>
     <td class="fondo bt-top bt-left bt-right">&nbsp;&nbsp;Contenedor vacío (retorno)</td>
     <td class="bt-top bt-right"><span runat="server" id="full" class="equis"  >(&nbsp;X&nbsp;)</span></td></tr>
      <tr><td class="fondo bt-top">Carga suelta</td><td class="bt-right"><span runat="server" id="csuelta" class="equis"  >( &nbsp;&nbsp;&nbsp;)</span></td></tr>
      <tr><td class="fondo bt-top">Carga a consolidar</td><td class="bt-right"><span runat="server" id="consolidado" class="equis"  >( &nbsp;&nbsp;&nbsp;)</span></td></tr>
      <tr><td class="fondo bt-top">Contenedor lleno (Multiple)</td><td class="bt-right"><span runat="server" id="multiple" class="equis"  >( &nbsp;&nbsp;&nbsp;)</span></td></tr>
        </table>
     </div>
     </div>
     </div>
     <div class="seccion">
      <div class="accion">
       <table class="xcontroles" cellspacing="0" cellpadding="0">
        <tr><th class="bt-bottom bt-right bt-top bt-left" colspan="6"> Datos del <i>Turno Asignado</i> 
        
        </th>
        </tr>
         <tr>
         <td class="bt-bottom  bt-right bt-left" >Referencia <i>CONTECON</i> </td>
         <td class="bt-bottom ">
           <span id="referencia" class="labelprint" runat="server">CGS2014002 </span>
         </td>
         <td class="bt-bottom  bt-right bt-left">Booking</td>
         <td class="bt-bottom bt-right" colspan="3">
           <span id="booking" runat="server" class="labelprint">NUMERO BOOKING</span>
         </td>
         </tr>

         <tr><td class="bt-bottom  bt-right bt-left">Fecha de llegada (C.A.S)</td>
         <td class="bt-bottom" >
           <span id="fecha" runat="server" class="labelprint">2014/00/00</span>
         </td>
                 <td class="bt-bottom  bt-right bt-left" >Hora de llegada (Maxima)</td>
         <td class="bt-bottom " colspan="3">
           <span id="hora" runat="server" class="labelprint">00:00</span>
         </td>

         </tr>



         </table>
     </div>
    </div>
     <div class="seccion">
     <div class="accion">
          <table class="xcontroles tabladata" cellspacing="0" cellpadding="0" >
        <tr><th colspan="3" class="bt-bottom bt-right  bt-left" > DATOS DEL CONTENEDOR </th></tr> 
         <tr>
         <td class="bt-bottom  bt-left"  >Número del contenedor</td>
         <td class="bt-left bt-bottom bt-right  "><span id="container" runat="server" class="labelprint">1234567899</span> </td>
         <td rowspan="8" style="background-color:transparent!important; border-left-color:transparent!important";>
             <img  alt="" src="../shared/imgs/modified.jpg" height="200px" width="350px" />
         </td>
         </tr>
         <tr>
         <td class="bt-bottom   bt-left">Tamaño</td>
         <td class="bt-left bt-bottom"> <span id="tamano" runat="server" class="labelprint">00</span> 
         </td>
         </tr>
         <tr>
        <td class=" bt-left bt-bottom ">Operador</td>
        <td class="bt-left bt-bottom"> <span id="opera" runat="server" class="labelprint">00</span></td>
         </tr>
         <tr>
        <td class=" bt-left bt-bottom ">Depósito</td>
        <td class="bt-left bt-bottom"> <span id="depot" runat="server" class="labelprint">..</span></td>
         </tr>




         <tr>
         <td class="bt-left " colspan="2">
         IMPORTANTE! Gates:<br />
         Marque con una X las zonas del contenedor que presentan daños.
         Explique en la parte posterior del documento.
         </td>
         </tr>
     </table>
   



     <table class="xcontroles" cellspacing="0" cellpadding="0">
        <tr><th class="bt-bottom bt-right  bt-left" colspan="4"> Datos del transporte </th></tr>
            <tr>
            <td class="bt-bottom bt-right bt-left">Nombre del conductor</td>
             <td class="bt-bottom" colspan="3">
             <span id="conductor" runat="server"  class="labelprint">DATOS DE PRUEBA</span>
             </td>
         </tr>
         <tr>
         <td class="bt-bottom  bt-right bt-left">Documento de identidad</td>
         <td class="bt-bottom">
            <span id="cedula" runat="server"  class="labelprint">000000000000</span>
             </td>
              <td class="bt-bottom ">Placa de camión</td>
         <td class="bt-left bt-bottom bt-right validacion ">
          <span id="placa" runat="server"  class="labelprint">XXX000</span>
         </td>
         </tr>
    
         </table>
     <div class="informativo bt-left bt-right bt-bottom">
      <table cellpadding="0" cellspacing="0">
      <tr><td class="level1" >Responsabilidad de la información</td></tr>
      <tr>
      <td class="level2">
         Los datos proporcionados son de entera responsabilidad de quien los consigna, por lo que CONTECON GUAYAQUIL S.A. no se responsabiliza por cualquier error o falsedad que los mismos pudieren tener, siendo de cuenta del cliente todos los gastos y perjuicios que por dicho error se ocasionen a la carga.
      </td>
      </tr>
      </table>
     </div>
     <div id="generacion" >
       <table class="poster" cellspacing="1" cellpadding="1">
         <tr>
         <td colspan="1">
           <asp:Image ID="barcode2" runat="server" Height="50px" Width="400px" AlternateText="Servicio de código de barras no disponible" />
           <span id="numcontenedor" runat="server">CRXU1234568</span>
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
