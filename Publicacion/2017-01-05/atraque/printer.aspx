<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="printer.aspx.cs" Inherits="CSLSite.atraque.printer" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Imprimir Solicitud</title>
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
<div id="hoja">
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
     <tr><td colspan="2"><h2>SOLICITUD DE ATRAQUE</h2>  </td></tr>
     </table>
     </div>
     <div class="seccion">
     <div class="accion">
     <div class="membrete">
     <table cellpadding="0" cellspacing="0">
     <tr>
     <td class="bt-top" ><span id="servicio">Referencia #</span></td>
     <td class="bt-top " ><span class="service" runat="server" id="anumber">CGS20160000</span></td>
     <td class="fondo bt-top bt-left bt-right"><br /></td>
     </tr>
     </table>
     </div>
     </div>
     </div>
     <div class="seccion">
      <div class="accion">
      <div runat="server" id="xestado">ESTE DOCUMENTO NO ES VÁLIDO FUE ANULADO</div>
     <table class="xcontroles" cellspacing="0" cellpadding="0">
        <tr><th class="bt-bottom bt-right bt-top bt-left" colspan="4"> Datos de Agencia
        </th></tr>
         <tr>
         <td class="bt-bottom  bt-right bt-left" >Servicio </td>
         <td class="bt-bottom ">
           <span id="sservicio" class="labelprint" runat="server">- </span>
         </td>
         <td class="bt-bottom  bt-right bt-left">Agencia</td>
         <td class="bt-bottom bt-right">
           <span id="agencia" runat="server" class="labelprint">CONTECON CGSA</span>
         </td>
         </tr>

         </table>
     </div>
    </div>
     <div class="seccion">
     <div class="accion">
     <table class="xcontroles" cellspacing="0" cellpadding="0">
        <tr><th class="bt-bottom bt-right bt-top bt-left" colspan="4"> Datos de NAVE</th></tr>
         <tr>
         <td class="bt-bottom  bt-right bt-left" >Nombre de la nave</td>
         <td class="bt-bottom" colspan="1">
          <span id="nave_nombre" runat="server" class="labelprint">Agente de pruebas de Contecon Guayaquil</span>
         </td>
         <td class="bt-bottom  bt-right bt-left">Número IMO:</td>
         <td class="bt-bottom bt-right">
          <span id="num_imo" runat="server" class="labelprint">00000000</span>
         </td>
         </tr>
         <tr>
         <td class="bt-bottom  bt-right bt-left">Bandera</td>
         <td class="bt-bottom">
           <span id="nave_flag" runat="server" class="labelprint">Guayaquil</span>
         </td>
         <td class="bt-bottom  bt-right bt-left">Eslora</td>
         <td class="bt-bottom bt-right">
          <span id="nave_eslora" runat="server" class="labelprint">000,00</span>
         </td>
         </tr>

          <tr>
         <td class="bt-bottom  bt-right bt-left">Peso Neto (Ton)</td>
         <td class="bt-bottom"><span id="nave_neto" runat="server" class="labelprint">Guayaquil</span></td>
         <td class="bt-bottom  bt-right bt-left">Peso Bruto (Ton)</td>
         <td class="bt-bottom bt-right"><span id="nave_bruto" runat="server" class="labelprint">000,00</span> </td>
         </tr>

          <tr>
         <td class="bt-bottom  bt-right bt-left">Señal</td>
         <td class="bt-bottom"><span id="nave_sign" runat="server" class="labelprint">ZZZ000</span></td>
         <td class="bt-bottom  bt-right bt-left">Tipo</td>
         <td class="bt-bottom bt-right"><span id="nave_tipo" runat="server" class="labelprint">CELL</span> </td>
         </tr>

          <tr>
         <td class="bt-bottom  bt-right bt-left">Número de viaje (IN)</td>
         <td class="bt-bottom"><span id="nave_in" runat="server" class="labelprint">ZZZ000</span></td>
         <td class="bt-bottom  bt-right bt-left">Número de viaje (OUT)</td>
         <td class="bt-bottom bt-right"><span id="nave_out" runat="server" class="labelprint">XXX000</span> </td>
         </tr>

     </table>


      <table class="xcontroles" cellspacing="0" cellpadding="0">
        <tr><th class="bt-bottom bt-right bt-top bt-left" colspan="4">Datos de Certificado Internacional PBIP</th></tr>
         <tr>
         <td class="bt-bottom  bt-right bt-left" >Número PBIP</td>
         <td class="bt-bottom" colspan="1">
          <span id="pbip_num" runat="server" class="labelprint">000000</span>
         </td>
         <td class="bt-bottom  bt-right bt-left">Válido hasta</td>
         <td class="bt-bottom bt-right">
          <span id="pbip_hasta" runat="server" class="labelprint">2016-01-01</span>
         </td>
         </tr>

         <tr>
         <td class="bt-bottom  bt-right bt-left" >Provisional</td>
         <td class="bt-bottom" colspan="1">
          <span id="pbip_pro" runat="server" class="labelprint">NO</span>
         </td>
         <td class="bt-bottom  bt-right bt-left">Nivel de seguridad</td>
         <td class="bt-bottom bt-right">
          <span id="pbip_seguridad" runat="server" class="labelprint">3</span>
         </td>
         </tr>

     </table>



     <table class="xcontroles" cellspacing="0" cellpadding="0">
        <tr><th class="bt-bottom bt-right bt-top bt-left" colspan="4">Datos sobre Puertos</th></tr>
         <tr>
         <td class="bt-bottom  bt-right bt-left" >Último Puerto</td>
         <td class="bt-bottom" colspan="1">
          <span id="pto_ultimo" runat="server" class="labelprint">GYE</span>
         </td>
         <td class="bt-bottom  bt-right bt-left">Próximo Puerto</td>
         <td class="bt-bottom bt-right">
          <span id="pto_proximo" runat="server" class="labelprint">GYE</span>
         </td>
         </tr>
     </table>


          <table class="xcontroles" cellspacing="0" cellpadding="0">
        <tr><th class="bt-bottom bt-right bt-top bt-left" colspan="4">DATOS DE SENAE</th></tr>
         <tr>
         <td class="bt-bottom  bt-right bt-left" >Manifiesto de Importación</td>
         <td class="bt-bottom" colspan="1">
          <span id="adu_impo" runat="server" class="labelprint">GYE</span>
         </td>
         <td class="bt-bottom  bt-right bt-left">Manifiesto de Exportación</td>
         <td class="bt-bottom bt-right">
          <span id="adu_expo" runat="server" class="labelprint">GYE</span>
         </td>
         </tr>
     </table>

         <table class="xcontroles" cellspacing="0" cellpadding="0">
        <tr><th class="bt-bottom bt-right bt-top bt-left" colspan="4">DATOS DE AUTORIDAD PORTUARIA</th></tr>
         <tr>
         <td class="bt-bottom  bt-right bt-left" >Año</td>
         <td class="bt-bottom" colspan="1">
          <span id="apg_anio" runat="server" class="labelprint">2016</span>
         </td>
         <td class="bt-bottom  bt-right bt-left">Registro</td>
         <td class="bt-bottom bt-right">
          <span id="apg_registro" runat="server" class="labelprint">000000</span>
         </td>
         </tr>
     </table>



       <table class="xcontroles" cellspacing="0" cellpadding="0">
        <tr><th class="bt-bottom bt-right bt-top bt-left" colspan="4">DATOS DE FECHAS Y OPERACIÓN</th></tr>
         <tr>
         <td class="bt-bottom  bt-right bt-left" >Fecha estimada de arribo a boya Data-Posorja (ETA)</td>
         <td class="bt-bottom" colspan="1">
          <span id="ope_eta" runat="server" class="labelprint">0000/00/000 00:00</span>
         </td>
         <td class="bt-bottom  bt-right bt-left">Fecha estimada de atraque en muelle CGSA (ETB)</td>
         <td class="bt-bottom bt-right">
          <span id="ope_etb" runat="server" class="labelprint">0000/00/000 00:00</span>
         </td>
         </tr>


          <tr>
         <td class="bt-bottom  bt-right bt-left" >Número de Horas uso de muelle</td>
         <td class="bt-bottom" colspan="1">
          <span id="ope_uso" runat="server" class="labelprint">0000/00/000 00:00</span>
         </td>
         <td class="bt-bottom  bt-right bt-left">Fecha estimada de Zarpe (ETD)</td>
         <td class="bt-bottom bt-right">
          <span id="ope_etd" runat="server" class="labelprint">0000/00/000 00:00</span>
         </td>
         </tr>


          <tr>
         <td class="bt-bottom  bt-right bt-left" >Fecha de Atraque (ATA)</td>
         <td class="bt-bottom" colspan="1">
          <span id="ope_ata" runat="server" class="labelprint">0000/00/000 00:00</span>
         </td>
         <td class="bt-bottom  bt-right bt-left">Fecha de Zarpe (ATD)</td>
         <td class="bt-bottom bt-right">
          <span id="ope_atd" runat="server" class="labelprint">0000/00/000 00:00</span>
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
         <td colspan="1"> &nbsp;</td>
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
