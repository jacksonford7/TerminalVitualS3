<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="solicitud_preview.aspx.cs" Inherits="CSLSite.pasepuertabrbk.solicitud_preview" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>Imprimir Factura</title>
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
        .auto-style2 {
            height: 22px;
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
            <td width="564" height="34" valign="middle"><span class="Estilo10">Solicitud de Turnos No. </span></td>
              <td width="486" valign="middle"><div align="right" class="Estilo5"><span id="numero_factura" runat="server">...</span></div></td>
            </tr>
          <tr>
            <td height="25" valign="top"><span class="Estilo10">Fecha:</span></td>
              <td valign="top"><div align="right" class="Estilo5"><span id="fecha_factura" runat="server">...</span></div></td>
            </tr>
          <tr>
            <td height="25" valign="top" class="Estilo10"></td>
              <td valign="top"><div align="right" class="Estilo5"></div></td>
            </tr>
          <tr>
            <td height="25" valign="top" class="Estilo10"></td>
              <td valign="top"><div align="right" class="Estilo5"></div></td>
            </tr>
          
          <tr>
            <td height="25" colspan="2" valign="top"><div align="right" class="Estilo8"></div></td>
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
        <tr><th class="btx-bottom btx-right btx-top btx-left" colspan="6">SOLICITUD</th>
        </tr>
         <tr>
            <td class="auto-style2">NUMERO CARGA: </td>
            <td class="auto-style2" colspan="5"><span id="numero_carga" class="labelprint" runat="server">.... </span></td>
         </tr>
         <tr>
            <td class="bt-bottom  bt-right bt-left">AGENTE: </td>
            <td class="bt-bottom" colspan="5"><span id="agente" class="labelprint" runat="server">.... </span></td>
         </tr>
         <tr>
            <td class="bt-bottom  bt-right bt-left">IMPORTADOR: </td>
            <td class="bt-bottom" colspan="5"> <span id="importador" class="labelprint" runat="server">.... </span></td>
            
         </tr>
         <tr>
            <td class="bt-bottom  bt-right bt-left">ESTADO SOLICITUD: </td>
            <td class="bt-bottom" colspan="5"><span id="estado" class="labelprint" runat="server">.... </span></td>
         </tr>
            <tr>
            <td class="bt-bottom  bt-right bt-left">TIPO TURNO: </td>
            <td class="bt-bottom" colspan="5"><span id="tipo_turno" class="labelprint" runat="server">.... </span></td>
         </tr>
        <tr><th class="bt-bottom bt-right bt-top bt-left" colspan="6"># PASES</th>
        </tr>
         <tr>
            <td class="bt-bottom  bt-right bt-left">VEHICULOS: </td>
            <td class="bt-bottom" colspan="5"><span id="total_pases" class="labelprint" runat="server">.... </span></td>
         </tr>
         <tr>
            <td class="bt-bottom  bt-right bt-left">BULTOS: </td>
            <td class="bt-bottom" colspan="5"> <span id="total_bultos" class="labelprint" runat="server">.... </span></td>
            
          </tr>
         <tr><td class="bt-bottom  bt-right bt-left" colspan="6"> <span id="agente_aduana" runat="server" class="labelprint"></span></td>
         </tr>   
         </table>
     </div>
    </div>
     <div class="seccion">
     <div class="accion">
    <table class="xcontroles tabladata" cellspacing="0" cellpadding="0" >
        <tr><th colspan="3" class="bt-bottom bt-right  bt-left" > DETALLE DE TURNOS</th></tr> 
                 </table>
       <div class="pdiv bt-left bt-right" runat="server" id="detalle_data" clientidmode="Static">
           <table class='print_table'>
               <thead>
               <tr>
                   <td align='center'><strong>FECHA</strong></td> 
                   <td align='center'><strong>TURNO</strong></td> 
                   <td align='center'><strong># VEHICULOS</strong></td>
                   <td align='center'><strong>CANT. X VEH</strong></td>
                   <td align='center'><strong>TOTAL BULTOS</strong></td>
                   <td align='center'><strong>TRANSPORTISTA</strong></td>
               </tr>
               </thead>
               <tbody>
                   <tr>
                   <td align='center'>...</td>
                   <td align='center'>...</td>
                   <td align="center">...</td>
                   <td align="center">...</td>
                   <td align="center">...</td>
                       <td align="center">...</td>
                   </tr>
               </tbody>
               <tfoot>
              
               <tr >
                   <td colspan="6"><div class="lineadiv"></div></td>
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
