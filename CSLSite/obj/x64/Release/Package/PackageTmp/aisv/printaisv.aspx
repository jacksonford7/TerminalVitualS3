<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="printaisv.aspx.cs" Inherits="CSLSite.printaisv" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Imprimir AISV</title>
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
     <tr><td colspan="2"><h2>AUTORIZACIÓN DE INGRESO Y SALIDA DE VEHÍCULOS </h2>  </td></tr>
     </table>
     </div>
     <div class="seccion">
     <div class="accion">
     <div class="membrete">
     <table cellpadding="0" cellspacing="0">
     <tr><td class="bt-top" rowspan="4"><span id="servicio">Servicio #</span></td>
     <td class="bt-top " rowspan="4"><span class="service" runat="server" id="anumber">0123456789</span></td>
     <td class="fondo bt-top bt-left bt-right">&nbsp;&nbsp;Contenedor lleno</td>
     <td class="bt-top bt-right"><span runat="server" id="full" class="equis"  >(&nbsp;&nbsp;&nbsp;)</span></td></tr>
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
        <tr><th class="bt-bottom bt-right bt-top bt-left" colspan="6"> Datos del <i>Booking</i> 
        <span id="propietario" runat="server" class="propietario">*</span>
        </th>
        </tr>
         <tr>
         <td class="bt-bottom  bt-right bt-left" >Referencia <i>CONTECON</i> </td>
         <td class="bt-bottom ">
           <span id="referencia" class="labelprint" runat="server">CGS2014002 </span>
         </td>
         <td class="bt-bottom  bt-right bt-left">Nombre de la nave</td>
         <td class="bt-bottom bt-right" colspan="3">
           <span id="buque" runat="server" class="labelprint">NAVE DE PRUEBAS DE CONTECON</span>
         </td>
         </tr>

         <tr><td class="bt-bottom  bt-right bt-left">Fecha estimada de arribo [ETA]</td>
         <td class="bt-bottom" >
           <span id="eta" runat="server" class="labelprint">2014/00/00 00:00</span>
         </td>
                 <td class="bt-bottom  bt-right bt-left">Fecha límite [CutOff]</td>
         <td class="bt-bottom ">
           <span id="cutof" runat="server" class="labelprint">2014/00/00 00:00</span>
         </td>
         <td class="bt-bottom  bt-right bt-left">Último Ingreso sugerido [UIS]</td>
         <td class="bt-bottom">
           <span id="uis" runat="server" class="labelprint">2014/00/00 00:00</span>
         </td>
         </tr>
         <tr>
         <td class="bt-bottom  bt-right bt-left">Nombre de la agencia Naviera</td>
         <td class="bt-bottom" colspan="5">
          <span id="agencia" runat="server" class="labelprint"> </span>
         </td>
         </tr>
         <tr>
         <td class="bt-bottom  bt-right bt-left">Tamaño de contenedor</td>
         <td class="bt-bottom">
         <span id="tamano" runat="server" class="labelprint">20'</span>
         </td>
         <td class="bt-bottom  bt-right bt-left">Tipo de contenedor (ISO)</td>
         <td class="bt-bottom ">
           <span runat="server" id="tipo" class="labelprint">-</span>
         </td>
         <td class="bt-bottom" colspan="2">
             <span>IMO</span>
             <span id="imos" runat="server">(&nbsp;&nbsp;&nbsp;)</span>
             <span>Refeer</span>
              <span id="refers" runat="server">(&nbsp;&nbsp;&nbsp;)</span>
             <span>Sobredimensionado</span>
             <span id="sobredimension" runat="server">(&nbsp;&nbsp;&nbsp;)</span>
             <span>L.A.</span>
             <span id="tardio" runat="server">(&nbsp;&nbsp;&nbsp;)</span>
         </td>
         </tr>
         <tr>
             <td class="bt-bottom  bt-right bt-left">Producto declarado en Booking</td>
             <td class="bt-bottom bt-right" colspan="2">
              <span id="producto" runat="server" class="labelprint">&nbsp;</span>
             </td>
        
             <td class="bt-bottom  bt-right bt-left">Colocar en Zona Especial Contenedor Seco.</td>
             <td class="bt-bottom bt-right" colspan="2">
              <span id="zonaespecial" runat="server" class="labelprint">( &nbsp;&nbsp;&nbsp;)</span>
             </td>
         </tr>
         </table>
     </div>
    </div>
     <div class="seccion">
     <div class="accion">
          <table class="xcontroles tabladata" cellspacing="0" cellpadding="0" >
        <tr><th colspan="3" class="bt-bottom bt-right  bt-left" > DATOS DEL CONTENEDOR / CARGAS</th></tr> 
         <tr>
         <td class="bt-bottom  bt-left"  >Número del contenedor</td>
         <td class="bt-left bt-bottom bt-right  "><span id="container" runat="server" class="labelprint">1234567899</span> </td>
         <td rowspan="8" style="background-color:transparent!important; border-left-color:transparent!important";>
             <img  alt="" src="../shared/imgs/modified.jpg" height="200px" width="350px" />
         </td>
         </tr>
         <tr>
         <td class="bt-bottom   bt-left">Max. Payload (ton)</td>
         <td class="bt-left bt-bottom"> <span id="payload" runat="server" class="labelprint">00</span> 
         </td>
         </tr>
         <tr>
        <td class=" bt-left bt-bottom ">Peso declarado</td>
        <td class="bt-left bt-bottom"> <span id="peso" runat="server" class="labelprint">00</span></td>
         </tr>
        <tr>
            <td class="bt-bottom   bt-left">Cant. Bultos (U)</td>
            <td class="bt-left bt-bottom"><span id="bultos" runat="server" class="labelprint">00</span></td>
         </tr>
        <tr>
        <td class="bt-bottom   bt-left">Embalaje</td>
         <td class="bt-left bt-bottom" ><span id="embalar" runat="server" class="labelprint">No aplica</span></td>
         </tr>
         <tr>
         <td class=" bt-left bt-bottom ">Peligrosidad</td>
         <td class="bt-left bt-bottom"> <span id="peligro" runat="server" class="labelprint">No aplica</span>
        </td>
         </tr>
         <tr><td class="bt-bottom  bt-right bt-left">Código Certificado (TECA)</td>
         <td class="bt-bottom">
            <span id="tecacert" runat="server"  class="labelprint">certificado teca</span>
             </td>
             </tr>
             <tr>
              <td class="bt-bottom bt-right">Fecha Fumigación (TECA)</td>
         <td class="bt-bottom bt-right validacion ">
          <span id="tecafecha" runat="server"  class="labelprint">12/12/12</span>
         </td>
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
        <tr><th class=" bt-top bt-bottom bt-right  bt-left" colspan="4"> Detalle de los sellos de 
            LA CARGA / CONTENEDOR</th></tr>
         <tr>

         <tr>
            <td class="bt-bottom bt-right bt-left">Documento de exportación No.</td>
             <td class="bt-bottom" colspan="3">
             <span id="dae" runat="server"  class="labelprint">000000000000000000000</span>
             </td>
         </tr>

         <td class="bt-bottom  bt-right bt-left" >Sello de agencia</td>
         <td class="bt-bottom">
              <span id="seal1" runat="server" class="labelprint xant">0000000000</span></td>
               <td class="bt-bottom bt-right ">Sello de ventilación</td>
          <td class="bt-bottom bt-right validacion ">
           <span id="seal2" runat="server" class="labelprint xant">00000000</span>
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
