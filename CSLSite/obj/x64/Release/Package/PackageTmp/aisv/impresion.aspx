<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="impresion.aspx.cs" Inherits="CSLSite.impresion" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="../shared/avisos/marcas.css" rel="stylesheet" type="text/css" />
    <title>Imprimir AISV</title>
    <link href="../shared/estilo/aisv-general.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/print.css" rel="stylesheet" type="text/css"  />
    <script language="javascript" type="text/javascript">
// <![CDATA[
        function btclear_onclick() {
            window.print()
        }
// ]]>

    </script>

</head>
<body>
<div id="hoja" runat="server" clientidmode="Static">
     <div >
     <table class="imagenes" cellpadding="0" cellspacing="0">
     <tr>
     <td>
      <img src="../shared/imgs/logoContecon.png"  alt="logo"/>
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
      <div runat="server" id="xestado" class="bt-right bt-top bt-left" >ESTE DOCUMENTO NO ES VÁLIDO FUE ANULADO</div>
     <table class="xcontroles" cellspacing="0" cellpadding="0">
        <tr><th class="bt-bottom bt-right bt-top bt-left" colspan="6"> Datos del <i>Booking</i> 
         <span id="propietario" runat="server" class="propietario">*</span>
        </th></tr>
         <tr>
         <td class="bt-bottom  bt-right bt-left" >Referencia <i>CONTECON</i> </td>
         <td class="bt-bottom ">
           <span id="referencia" class="labelprint" runat="server">CGS2014002 </span>
         </td>
         <td class="bt-bottom  bt-right bt-left">Nombre de la nave</td>
         <td class="bt-bottom bt-right" colspan="4">
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
         <tr><td class="bt-bottom  bt-right bt-left">Nombre de la agencia Naviera</td>
         <td class="bt-bottom bt-right" colspan="6">
          <span id="agencia" runat="server" class="labelprint">Agencia de pruebas de Contecon Guayaquil </span>
         </td>
         </tr>
         <tr>
         <td class="bt-bottom  bt-right bt-left">Puerto de descarga</td>
         <td class="bt-bottom">
           <span id="descarga" runat="server" class="labelprint">ECUCGS</span>
         </td>
         <td class="bt-bottom   bt-left">Puerto de descarga Final</td>
         <td class="bt-bottom  ">
         <span id="final" runat="server" class="labelprint">ECUCGS</span>
         </td>
         <td class="bt-bottom  bt-right ">Bajo Cubierta</td>
         <td class="bt-bottom bt-right">
             <span id="cubierta" runat="server" class="labelprint">SI</span>
             </td>
         </tr>
         <tr><td class="bt-bottom  bt-right bt-left">Producto declarado en Booking</td>
         <td class="bt-bottom bt-right" colspan="6">
          <span id="producto" runat="server" class="labelprint">&nbsp;</span>
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
         <td class="bt-bottom" colspan="3">
             <span>IMO</span>
             <span id="imos" runat="server">(&nbsp;&nbsp;&nbsp;)</span>
             <span>Refeer</span>
              <span id="refers" runat="server">(&nbsp;&nbsp;&nbsp;)</span>
             <span>Sobredimen.</span>
             <span id="sobredimension" runat="server">(&nbsp;&nbsp;&nbsp;)</span>&nbsp; 
             <span>L.A.</span>
             <span id="tardio" runat="server">(&nbsp;&nbsp;&nbsp;)</span>&nbsp; 
            <span>DIP</span>
             <span id="diplo" runat="server">(&nbsp;&nbsp;&nbsp;)</span>&nbsp; 
            </td>
         </tr>
         <tr>
             <td class=" bt-right bt-left">Notas del Booking</td>
             <td class=" bt-right" colspan="2">
                  <span id="notas" runat="server" class="labelprint">NOTAS AGREGADAS</span>
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
     <table class="xcontroles" cellspacing="0" cellpadding="0">
        <tr><th class="bt-bottom bt-right bt-top bt-left" colspan="4"> Datos de Aduana (Senae) </th></tr>
         <tr>
         <td class="bt-bottom  bt-right bt-left" >Nombre de agente afianzado</td>
         <td class="bt-bottom bt-right" colspan="3">
          <span id="agente" runat="server" class="labelprint">Agente de pruebas de Contecon Guayaquil</span>
         </td>
<%--         <td class="bt-bottom  bt-right bt-left">Número de entrega:</td>
         <td class="bt-bottom bt-right">
          <span id="entregassss" runat="server" class="labelprint">0590520149011336544 (R)</span>
         </td>--%>
         </tr>
         <tr>
         <td class="bt-bottom  bt-right bt-left">Documento de Aduana No</td>
         <td class="bt-bottom">
           <span id="documento" runat="server" class="labelprint">20140280123456987 (DAE)</span>
         </td>
         <td class="bt-bottom  bt-right bt-left">Mercadería suceptible a cupo</td>
         <td class="bt-bottom bt-right">
          <span id="regla" runat="server" class="labelprint">MAGAP, Banano 8%</span>
         </td>
         </tr>

          <tr>
         <td class="bt-bottom  bt-right bt-left" >Consignatario</td>
         <td class="bt-bottom bt-right" colspan="3">
          <span id="consignata" runat="server" class="labelprint">
          CONSIGNATAIORIO DE LA CARG
          </span>
         </td>

         </tr>
     </table>
     <div class="divtx">
     <table cellpadding="0" cellspacing="0">
     <tr>
     <td class="tablix">
          <table class="xcontroles" cellspacing="0" cellpadding="0">
        <tr><th class="bt-bottom bt-right  bt-left" colspan="4"> Datos del contenedor 
            / CARGAS</th></tr>
              <tr>
         <td class="bt-bottom  bt-right bt-left" >Número del contenedor</td>
         <td class="bt-bottom xant">
           <span id="container" runat="server" class="labelprint xant">1234567899</span>
         </td>
         <td class=" bt-left bt-bottom bt-right">Tara del contenedor (ton)</td>
         <td class="bt-bottom bt-right">
          <span id="tara" runat="server" class="labelprint xant">00</span>
         </td>
         </tr>
         <tr><td class="bt-bottom  bt-right bt-left">Max. Payload (ton)</td>
         <td class="bt-bottom">
              <span id="payload" runat="server" class="labelprint xant">00</span></td>
              <td class=" bt-left bt-bottom bt-right">Peso declarado</td>
        <td class="bt-bottom bt-right validacion ">
         <span id="peso" runat="server" class="labelprint">00</span>
        </td>
         </tr>

            <tr><td class="bt-bottom  bt-right bt-left">Cant. Bultos (U)</td>
         <td class="bt-bottom">
              <span id="bultos" runat="server" class="labelprint">00</span></td>
              <td class=" bt-left bt-bottom bt-right">Peligrosidad</td>
        <td class="bt-bottom bt-right validacion ">
         <span id="peligro" runat="server" class="labelprint">No aplica</span>
        </td>
         </tr>
        <tr><td class="bt-bottom  bt-right bt-left">Embalaje</td>
         <td class="bt-bottom bt-right" colspan="3">
              <span id="embalar" runat="server" class="labelprint">No aplica</span></td>

         </tr>

         <tr><td class="bt-bottom  bt-right bt-left">Código Certificado (TECA)</td>
            <td class="bt-bottom">
             <span id="tecacert" runat="server"  class="labelprint">certificado teca</span>
             </td>
         <td class="bt-bottom bt-right">Fecha Fumigación (TECA)</td>
           <td class="bt-bottom bt-right validacion ">
            <span id="tecafecha" runat="server"  class="labelprint">12/12/12</span>
           </td>
         </tr>
     </table>
     <table class="xcontroles" cellspacing="0" cellpadding="0" id="refrigeracion">
        <tr><th class="bt-bottom bt-right  bt-left" colspan="4"> Datos sobre refrigeración del contenedor 
            (SI APLICA)</th></tr>
         <tr>
         <td class="bt-bottom  bt-right bt-left" >Tipo de refrigeración</td>
         <td class="bt-bottom xant">
           <span id="refrigera" class="labelprint xant" runat="server">No refrigerado</span>
         </td>
         <td class="bt-bottom bt-right bt-left">Humedad &nbsp;[CBM]</td>
         <td class="bt-bottom bt-right">
          <span id="humedad" runat="server" class="labelprint xant">00</span>
         </td>
         </tr>
         <tr>
         <td class="bt-bottom  bt-right bt-left">Temperatura [°C]</td>
         <td class="bt-bottom">
          <span id="temp" runat="server" class="labelprint xant">00</span></td>
         <td class="bt-bottom bt-right  bt-left">Ventilación&nbsp;[%]</td>
        <td class="bt-bottom bt-right validacion ">
         <span id="ventilacion" runat="server" class="labelprint xant">00%</span>
        </td>
         </tr>
         <tr>
         <td class="bt-bottom  bt-right bt-left">Depósito:</td>
         <td class="bt-bottom">
          <span id="depot" runat="server" class="labelprint">&nbsp;</span></td>
         <td class="bt-bottom bt-right bt-left">Fecha entrega:</td>
        <td class="bt-bottom bt-right validacion ">
         <span id="fechadepot" runat="server" class="labelprint">2014/02/01 00:00</span>
        </td>
         </tr>
         </table>
     </td>
     <td class="danio bt-right  bt-bottom">
         <img alt="" src="../shared/imgs/modified.jpg" height="150px" width="300px" />
         (Marque con una [X], las zonas de la unidad que presenten daños)
      </td>
     </tr>
     </table>
     
     </div>
     <table class="xcontroles" cellspacing="0" cellpadding="0">
        <tr><th class="bt-bottom bt-right  bt-left" colspan="4"> Detalle de los sellos del contenedor 
            (SI APLICA)</th></tr>
         <tr>
         <td class="bt-bottom  bt-right bt-left" >Sello de agencia</td>
         <td class="bt-bottom ancho">
              <span id="seal1" runat="server" class="labelprint xant">0000000000</span></td>
               <td class="bt-bottom bt-right">Sello de ventilación</td>
          <td class="bt-bottom bt-right validacion ">
           <span id="seal2" runat="server" class="labelprint xant">00000000</span>
          </td>
         </tr>
         <tr>
         <td class="bt-bottom  bt-right bt-left" >Sello adicional 1</td>
         <td class="bt-bottom ancho">
         <span id="seal3"  runat="server" class="labelprint xant">00000</span>
         </td>
         <td class="bt-bottom bt-right">Sello adicional 2</td>
         <td class="bt-bottom bt-right validacion">
           <span id="seal4" runat="server" class="labelprint xant">00000</span>
         </td>
         </tr>

         <tr>
         <td class="bt-bottom  bt-right bt-left" >Responsable del sellado:</td>
         <td colspan="3" class="bt-bottom ancho">
         <span id="sresponde"  runat="server" class="labelprint xant">091917595013 - PERSONA RESPONSABLE POR SELLOS</span>
         </td>

         </tr>

         </table>
     <table class="xcontroles" cellspacing="0" cellpadding="0">
        <tr>
         <th class="bt-bottom bt-right  bt-left" colspan="10"> Detalle del exceso de dimensiones del contenedor 
             (SI APLICA)</th>
         </tr>
         <tr>
             <td class="bt-bottom  bt-right bt-left">Excesos izquierda(cm)</td>
             <td class="bt-bottom"><span id="izquierda" runat="server" class="labelprint">0</span> </td>
             <td class="bt-bottom bt-right">Frontal (cm)</td>
             <td class="bt-bottom"><span id="frontal" runat="server" class="labelprint">0</span>   </td>
             <td class="bt-bottom  bt-right bt-left">Derecha (cm)</td>
             <td class="bt-bottom"><span id="derecha" runat="server" class="labelprint">0</span></td>
             <td class="bt-bottom bt-right"> Posterior (cm)</td>
             <td class="bt-bottom "><span id="atras" runat="server"  class="labelprint">0</span></td>
             <td class="bt-bottom  bt-right bt-left">Superior&nbsp; (cm)</td>
             <td class="bt-bottom" ><span id="superior" runat="server"  class="labelprint">0</span></td>   
         </tr>
  </table>
     <table class="xcontroles" cellspacing="0" cellpadding="0">
        <tr><th class="bt-bottom bt-right  bt-left" colspan="4"> Datos del transporte 
            (AISV)</th></tr>
         <tr>
         <td class="bt-bottom  bt-right bt-left" >Ubicación de la planta</td>
         <td class="bt-bottom" colspan="3"> 
          <span id="ubicacion" runat="server"  class="labelprint">Guayas, Guayaquil</span>
          </td>
         </tr>
         <tr><td class="bt-bottom  bt-right bt-left">Fecha y hora de salida de planta</td>
         <td class="bt-bottom">
           <span id="salida" runat="server"  class="labelprint">2014/05/01 18:00</span>
             </td>
              <td class="bt-bottom bt-right">Tiempo estimado de viaje (horas)</td>
         <td class="bt-bottom bt-right validacion ">
          <span id="tviaje" runat="server"  class="labelprint">0</span>
         </td>
         </tr>
            <tr><td class="bt-bottom  bt-right bt-left">Nombre del conductor</td>
         <td class="bt-bottom" colspan="3">
             <span id="conductor" runat="server"  class="labelprint">DATOS DE PRUEBA</span>
             </td>
         </tr>
         <tr><td class="bt-bottom  bt-right bt-left">Documento de identidad</td>
         <td class="bt-bottom">
            <span id="cedula" runat="server"  class="labelprint">000000000000</span>
             </td>
              <td class="bt-bottom bt-right">Placa de camión</td>
         <td class="bt-bottom bt-right validacion ">
          <span id="placa" runat="server"  class="labelprint">XXX000</span>
         </td>
         </tr>

         <tr><td class="bt-bottom  bt-right bt-left">Certificado de cabezal</td>
         <td class="bt-bottom">
             <span id="cabezal" runat="server"  class="labelprint">adnm123654</span>
             </td>
               <td class="bt-bottom bt-right">Certificado de chasis</td>
         <td class="bt-bottom bt-right validacion ">
          <span id="chasis" runat="server"  class="labelprint">abc456</span>
         </td>
         </tr>
         <tr><td class=" bt-right bt-left bt-bottom">Certificado especial</td>
           <td class="bt-right bt-bottom" colspan="3">
            <span id="especial" runat="server"  class="labelprint">No aplica</span>
           </td>
         </tr>
                                           

                  <tr><td class=" bt-right bt-left bt-bottom">Empresa de transporte:</td>
           <td class="bt-right" colspan="3">
            <span id="trasnport" runat="server"  class="labelprint">....</span>
           </td>
         </tr>

     </table>
       <table id="tbsellos" class="xcontroles" cellspacing="0" cellpadding="0" runat="server" clientidmode="Static">
        <tr><th class="bt-bottom bt-right bt-top bt-left" colspan="4"> Detalles de los sellos al Ingreso a la terminal (FISICOS)</th></tr>
         <tr>
         <td class="bt-bottom  bt-right bt-left" >Sello CGSA:</td>
         <td class="bt-bottom" colspan="3">
        <span id="sellocgsa" runat="server" class="labelprint">00000000000000000</span>
         </td>
         </tr>
         <tr>
         <td class="bt-bottom  bt-right bt-left">Sello Físico #1:</td>
         <td class="bt-bottom">
           <span id="cgsas1" runat="server" class="labelprint">00000000000000000</span>
         </td>
         <td class="bt-bottom  bt-right bt-left">Sello Físico #2:</td>
         <td class="bt-bottom bt-right">
          <span id="cgsas2" runat="server" class="labelprint">0000000000000000</span>
         </td>
         </tr>
        <tr>
         <td class="bt-bottom  bt-right bt-left" >Sello Físico #3:</td>
         <td class="bt-bottom" colspan="1">
          <span id="cgsas3" runat="server" class="labelprint">0000000000000000</span>
         </td>
         <td class="bt-bottom  bt-right bt-left">Sello Físico #4:</td>
         <td class="bt-bottom bt-right">
          <span id="cgsas4" runat="server" class="labelprint">000000000000000</span>
         </td>
         </tr>
     </table>
       <table id="tbaduana" class="xcontroles" cellspacing="0" cellpadding="0" runat="server" clientidmode="Static">
        <tr><th class="bt-bottom bt-right bt-top bt-left" colspan="2"> Datos de aduana generados en la Terminal (CGSA)</th></tr>
         <tr>
         <td class="bt-bottom  bt-right bt-left" >No. Entrega (Ecuapass):</td>
         <td class="bt-bottom">
          <span id="cgsaentrega" runat="server" class="labelprint">000000000000000000</span>
         </td>
         </tr>
         </table>
       <table id="tbchofer" class="xcontroles" cellspacing="0" cellpadding="0" runat="server" clientidmode="Static">
        <tr><th class="bt-bottom bt-right bt-top bt-left" colspan="4"> Datos del transporte al ingreso (CGSA)</th></tr>
         <tr>
         <td class="bt-bottom  bt-right bt-left" >Nombre del conductor:</td>
         <td class="bt-bottom" colspan="3"> <span id="cgsachofer" runat="server" class="labelprint">...</span></td>
         
         </tr>
         <tr>
         <td class="bt-bottom  bt-right bt-left">Documento de Indetidad:</td>
         <td class="bt-bottom">
           <span id="cgsalicencia" runat="server" class="labelprint">00000000000000000</span>
         </td>
         <td class="bt-bottom  bt-right bt-left">Placa de camión (FÍSICO):</td>
         <td class="bt-bottom bt-right">
          <span id="cgsaplaca" runat="server" class="labelprint">0000000000000000</span>
         </td>
         </tr>

        </table>
       <table class="xcontroles" id="tbpeso" cellspacing="0" cellpadding="0" runat="server" clientidmode="Static">
        <tr><th class="bt-bottom bt-right bt-top bt-left" colspan="4"> Datos DE INGRESO Y 
            PESO DE LA UNIDAD (CGSA)</th></tr>
         <tr>
         <td class="bt-bottom  bt-right bt-left" >Fecha y Hora Entrada Puerto:</td>
         <td class="bt-bottom" colspan="1">
          <span id="fechaptoin" runat="server" class="labelprint">01-02-2014 00:00</span>
         </td>
         <td class="bt-bottom  bt-right bt-left" >Peso Neto (Ton):</td>
         <td class="bt-bottom bt-right" >
            <span id="pesoneto" runat="server" class="labelprint">9</span>
         </td>
         </tr>
         <tr>
         <td class="bt-bottom  bt-right bt-left">Fecha y Hora Salida Puerto:</td>
         <td class="bt-bottom">
           <span id="fechaptoout" runat="server" class="labelprint">01-02-2014 00:00</span>
         </td>
         <td class="bt-bottom  bt-right bt-left" >Peso bruto (Ton):</td>
         <td class="bt-bottom bt-right" >
           <span id="pesobruto" runat="server" class="labelprint">9</span>
         </td>
         </tr>
        <tr>
         <td class="bt-bottom  bt-right bt-left" >Máximo peso bruto (TON):</td>
         <td class="bt-bottom" colspan="1">
         <span id="pesobrutomax" runat="server" class="labelprint">5</span>
         </td>
         <td class="bt-bottom  bt-right bt-left">Diferencia peso bruto (TON):</td>
         <td class="bt-bottom bt-right">
           <span id="diferencia" runat="server" class="labelprint">5</span>
         </td>
         </tr>

         <tr style=" display:none; visibility:hidden;">
         <td class="bt-bottom  bt-right bt-left" >Peso al ingreso (TON):</td>
         <td class="bt-bottom" colspan="1">
          <span id="pesoin" runat="server" class="labelprint">5</span>
         </td>
         <td class="bt-bottom  bt-right bt-left">Peso a la salida (TON):</td>
         <td class="bt-bottom bt-right">
          <span id="pesoout" runat="server" class="labelprint">10</span>
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
