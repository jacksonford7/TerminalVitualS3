<%@ Page Title="AISV Carga Suelta" EnableEventValidation="true" EnableViewState="false"  Language="C#" MasterPageFile="~/site.Master" AutoEventWireup="true" CodeBehind="cargasuelta.aspx.cs" Inherits="CSLSite.cargasuelta" %>
<%@ MasterType VirtualPath="~/site.Master" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="placehead" runat="server">
    <link href="../shared/estilo/aisv-general.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/work.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="placebody" runat="server">
    <input id="zonaid" type="hidden" value="202" />
    <noscript><meta http-equiv="refresh" content="0; url=sinsoporte.htm" /> </noscript>
     <div>
         <i class="ico-titulo-1"></i><h2>Autorización de Ingreso y Salida de Vehículos </h2>  <br /> 
         <i class="ico-titulo-2"></i><h1>Nuevo AISV para Exportación de Carga Suelta</h1><br />
    </div>
     <div class="msg-alerta">
    <span id="dtlo" runat="server">Estimado usuario:</span> 
    <br /> 
          Para este tipo de AISV, en el campo “Documento de Aduana” debe registrar una <strong>  DAE de carga suelta</strong>.  
          Si la carga ingresará en un contenedor de apoyo, deberá llenar de manera obligatoria el campo de
          “Datos del contenedor”.La transmisión al sistema de Aduana se efectuará confirmando la cantidad de 
           bultos indicados en este formulario. Aceptado el registro no se realizarán modificaciones, ni correcciones. 
    </div>
         <div class="seccion">
      <div class="informativo">
      <table id="BUSCAR">
      <tr><td rowspan="2" class="inum"> <div class="number">1</div></td><td class="level1" >Ingreso o selección de Booking </td></tr>
      <tr><td class="level2">Ingrese o escoja el Booking a relacionar con este AISV.</td></tr>
      </table>
     </div>
     <div class="colapser colapsa"></div>
     <div class="accion">
     <table class="controles" cellspacing="0" cellpadding="1">
        <tr><th class="bt-bottom bt-right bt-top bt-left" colspan="4"> Búsqueda del Booking</th></tr>
         <tr>
         <td class="bt-bottom  bt-right bt-left" >1. Nombre del Exportador:</td>
         <td class="bt-bottom bt-right" colspan="3">
         <span id="numexpo" class="caja" runat="server" clientidmode="Static"  enableviewstate="False">...</span>
         <span id="nomexpo" class="caja cajafull" style="width:300px;" runat="server" clientidmode="Static"  enableviewstate="False">...</span>
         <input id="numexport" type="hidden" runat="server" clientidmode="Static"  enableviewstate="False" />
         </td>       
         </tr>
         <tr><td class="bt-bottom  bt-right bt-left">2. Línea de Naviera:</td>
         <td class="bt-bottom">
          <span id="linea" class="caja">...</span>
          <input id="lineaCI" type="hidden" />
         </td>
         <td class="bt-bottom finder">
           <a  class="topopup" target="popup" onclick="window.open('../catalogo/lineas','name','width=850,height=480')" >
          <i class="ico-find" ></i> Buscar </a>
         </td>
         <td class="bt-bottom bt-right validacion"><span class="validacion" id="xplinea" > * obligatorio</span></td>
         </tr>
         <tr><td class="bt-bottom  bt-right bt-left">3. Número de <i>Booking</i>:</td>
         <td class="bt-bottom">
          <span id="numbook" class="caja">...</span>
         </td>
              <td class="bt-bottom finder">
            <a class="topopup" target="popup" onclick="linkbokin('lineaCI','numexport','BBK');" >
          <i class="ico-find" ></i> Buscar   </a>
         </td>
         <td class="bt-bottom bt-right validacion"><span class="validacion"  id="xpbok"> * obligatorio</span></td>
         </tr>
     </table>
     </div>
    </div>
     <div class="seccion">
      <div class="informativo">
      <table>
      <tr><td rowspan="2" class="inum"> <div class="number">2</div></td><td class="level1" >Verficación de datos del Booking asociado</td></tr>
      <tr><td class="level2">
         Confirme que los datos sean correctos. En caso de error, favor comuníquese con el Departamento de Planificación a los teléfonos: +593 (04) 6006300, 3901700  ext. 4039, 4040, 4060, 4040, 4039.
      </td></tr>
      </table>
     </div>
    <div class="colapser colapsa"></div>
      <div class="accion">
     <table class="controles" cellspacing="0" cellpadding="1">
        <tr><th class="bt-bottom bt-right bt-top bt-left" colspan="2"> Datos del <i>Booking</i> </th></tr>
         <tr>
         <td class="bt-bottom  bt-right bt-left" >4. Referencia <i>CONTECON</i> </td>
         <td class="bt-bottom bt-right">
           <span id="referencia" class="caja cajafull">... </span>
         </td>
         </tr>
           <tr><td class="bt-bottom  bt-right bt-left">5. Nombre de la nave:</td>
         <td class="bt-bottom bt-right">
           <span id="buque" class="caja cajafull">...</span>
         </td>
         </tr>
         <tr><td class="bt-bottom  bt-right bt-left">6. Fecha estimada de arribo [ETA]:</td>
         <td class="bt-bottom bt-right">
           <span id="eta" class="caja cajafull">...</span>
         </td>
         </tr>
         <tr><td class="bt-bottom  bt-right bt-left">7. Fecha límite [CutOff]:</td>
         <td class="bt-bottom bt-right">
           <span id="cutof" class="caja cajafull">...</span>
         </td>
         </tr>
         <tr><td class="bt-bottom  bt-right bt-left">8. Último Ingreso sugerido [UIS]:</td>
         <td class="bt-bottom bt-right">
           <span id="uis" class="caja cajafull">...</span>
         </td>
         </tr>
         <tr><td class="bt-bottom  bt-right bt-left">9. Nombre de la agencia Naviera:</td>
         <td class="bt-bottom bt-right">
          <span id="agencia" class="caja cajafull">...</span>
         </td>
         </tr>
         <tr><td class="bt-bottom  bt-right bt-left">10. Puerto de descarga:</td>
         <td class="bt-bottom bt-right">
           <span id="descarga" class="caja cajafull">...</span>
         </td>
         </tr>
         <tr><td class="bt-bottom  bt-right bt-left">11. Puerto de descarga Final:</td>
         <td class="bt-bottom bt-right">
         <span id="final" class="caja cajafull">...</span>
         </td>
         </tr>
         <tr><td class="bt-bottom  bt-right bt-left">12. Producto declarado en Booking:</td>
         <td class="bt-bottom bt-right">
          <span id="producto" class="caja cajafull">...</span>
         </td>
         </tr>
         <tr><td class="bt-bottom  bt-right bt-left">13. Tamaño de contenedor:</td>
         <td class="bt-bottom bt-right">
         <span id="tamano" class="caja cajafull">NO APLICA EN ESTE DOCUMENTO</span>
         </td>
         </tr>
         <tr><td class="bt-bottom  bt-right bt-left">14. Tipo de contenedor:</td>
         <td class="bt-bottom bt-right">
           <span id="tipo" class="caja cajafull">NO APLICA EN ESTE DOCUMENTO</span>
         </td>
         </tr>
         <tr><td class="bt-bottom  bt-right bt-left">15. Características de la unidad:</td>
         <td class="bt-bottom  bt-right">
             <span>IMO</span>
             <input id="imo" type="checkbox"  disabled="disabled" />
             <span>Refeer</span>
             <input  type="checkbox" id="refer" disabled="disabled"   />
             <span>Sobredimensionado</span>
             <input id="over" type="checkbox"  onclick="radio(this,ckdimen);"/>
                   <span>Diplomático</span>
               <input id="ckdip" type="checkbox" />   
             <a class="tooltip" >
			<span class="classic">Contenedor diplomático, unidad para entrega a unidades gubernamentales</span>
            <img alt='' src='../shared/imgs/info.gif' class='datainfo'/>
            </a>
         </td>
         </tr>
         <tr><td class="bt-bottom  bt-right bt-left">16. Estiba bajo cubierta:</td>
         <td class="bt-bottom bt-right">
          <a class="tooltip" ><span class="classic">Indica si la carga declarada debe ser colocada bajo cubierta</span>
              Si[<input id="rbsi"   type="radio" name="deck" />]
              No[ <input id="rbno" checked="checked"   type="radio" name="deck" />]
             </a>&nbsp;(Sujeto a disponibilidad de espacio)</td>
         </tr>
         <tr><td class="bt-bottom  bt-right bt-left">17. Notas del Booking:</td>
         <td class="bt-bottom  bt-right">
             <span class="notas caja cajafull" id="remar">
             
             </span>
             </td>
         </tr>
     </table>
     </div>
    </div>
     <div class="seccion">
      <div class="informativo">
      <table>
      <tr><td rowspan="2" class="inum"> <div class="number">3</div></td><td class="level1" >Ingreso de Datos</td></tr>
      <tr>
      <td class="level2">
         Ingrese los datos requeridos en cada una de las siguientes secciones.
      </td>
      </tr>
      </table>
     </div>
      <div class="colapser colapsa"></div>
     <div class="accion">
      <table class="controles" cellspacing="0" cellpadding="1">
        <tr><th class="bt-bottom bt-right bt-top bt-left" colspan="5" id="ADU"> Datos de Aduana (Senae) </th></tr>
         <tr>
         <td class="bt-bottom  bt-right bt-left" >18. Nombre de agente afianzado:</td>
         <td class="bt-bottom" colspan="2">
          <span id="agente" class="caja cajafull">...</span>
         </td>
         <td class="bt-bottom finder">
          <input id="caeid" type="hidden" />
          <a  class="topopup" target="popup" onclick="window.open('../catalogo/agentes','name','width=850,height=480')" >
           <i class="ico-find" ></i> Buscar   </a>
         </td>
        <td class="bt-bottom bt-right validacion "><span class="opcional" id="valexpor" > * opcional</span></td>
         </tr>
         <tr><td class="bt-bottom  bt-right bt-left">19. Documento de Aduana No:</td>
         <td class="bt-bottom" colspan="3">
             <select id="dpdoc" onchange="adudoc(this,'txtdae1','txtdae2','txtdae3','xxxms');" style='width:60px'>
                 <option selected="selected"  value="DAE" >DAE</option>
                 <option value='DAS' >DAS</option>
                 <option value='DJT' >DJT</option>
                 <option value='TRS' >TRS</option>
             </select>
            
              <input id="txtdae1" type="text" maxlength="3" value='028' placeholder="00" onkeypress="return soloLetras(event,'1234567890',false)"  style='width:30px'/>
              <input id="txtdae2" type="text" maxlength="4" placeholder="0000" onkeypress="return soloLetras(event,'1234567890',false)"  style='width:40px'/>
                 <input id="txtdae3" type="text" maxlength="20" value='40'  onkeypress="return soloLetras(event,'ps1234567890',false)"  
                onblur="cadenareqerida(this,8,20,'valadu');"
                placeholder="Documento"
             
                style='width:180px'/>
              <a class="tooltip" ><span class="classic" id='xxxms'>D.A.E: Declaración Aduanera de Exportación</span>
                        <img alt='' src='../shared/imgs/info.gif' class='datainfo'/>
              </a>
             </td>
        <td class="bt-bottom bt-right validacion "><span class="validacion" id="valadu"  > * obligatorio</span></td>
         </tr>
         <tr><td class="bt-bottom  bt-right bt-left">20. Mercadería suceptible a cupo:</td>
         <td class="bt-bottom bt-right" colspan="4">
          <a class="tooltip" ><span class="classic">Indicar si la carga declarada, se controla bajo cupos como por ejemplo, banano.</span>
           <input id="ckcupo" type="checkbox" onclick="ckcupos(this);" />
                  
           </a>
           <div id="rules" class="disable">
              &nbsp;
               <asp:DropDownList 
               ID="dpins" 
                runat="server" 
               Width="100px" 
               Enabled="False"  
               EnableViewState="false"
               onchange="invoke(this,'regla',loadrule);"
               ClientIDMode="Static"
               >
               </asp:DropDownList>
               &nbsp;
               <select id="dprule"  style="width:200px;" onchange="rulevalidate(this,mrule);">
                   <option value="0">Regla</option>
               </select>
               &nbsp;
               <span id="mrule" class="validacion" > </span>
           </div>
         </td>
         </tr>
                <tr><td class="bt-bottom  bt-right bt-left">21. Consignatario:</td>
         <td class="bt-bottom bt-right" colspan="3">
               <asp:TextBox ID="consignee" runat="server" MaxLength="250" Width="350px"
               onblur="cadenareqerida(this,8,250,'valconsig');"
               placeholder="Consignatario"
               
             ></asp:TextBox>
                        <a class="tooltip" ><span class="classic" >
                        Consignatario o propietario final de la carga o contenido de la unidad
                        </span>
                        <img alt='' src='../shared/imgs/info.gif' class='datainfo'/>
              </a>

         </td>
         <td class="bt-bottom bt-right validacion "><span class="validacion" id="valconsig"  > * obligatorio</span></td>
         </tr>

     </table>
     <table class="controles" cellspacing="0" cellpadding="1">
        <tr><th class="bt-bottom bt-right  bt-left" colspan="3"> Datos de la carga suelta</th></tr>
         <tr>
         <td class="bt-bottom  bt-right bt-left" >23. Cantidad de unidades:</td>
         <td class="bt-bottom">
               <a class="tooltip" >
          <span class="classic">Sean Bultos, Cajas, Paquetes..  </span>
             <asp:TextBox ID="txtbultos" runat="server" Width="200px" MaxLength="6" CssClass="mayusc"
             onkeypress="return soloLetras(event,'1234567890')" onkeyup="valdata(this);"
               onBlur="valrange(this,1,100000,'valcantx',true);" EnableViewState="False"
               placeholder="Unidades"
             ></asp:TextBox>
             </a>
             <span id="bulcount" class="bulk"></span>
             </td>
         <td class="bt-bottom bt-right validacion "><span id="valcantx" class="validacion"> * obligatorio</span></td>
         </tr>
         <tr><td class="bt-bottom  bt-right bt-left">24. Peso bruto de unidades&nbsp;[KG]:</td>
         <td class="bt-bottom">
             <asp:TextBox ID="txtpeso" runat="server"  Width="200px" 
             MaxLength="5"
             onBlur="valrange(this,1,100000,'valpeso',true);"
             placeholder="Peso"
             onkeypress="return soloLetras(event,'1234567890')" EnableViewState="False"   onkeyup="conver(this,convierte);"
              ></asp:TextBox>
              <span id="convierte" class="">...</span>
             </td>
         <td class="bt-bottom bt-right validacion "><span id="valpeso" class="validacion"> * obligatorio</span></td>
         </tr>

        <tr><td class="bt-bottom  bt-right bt-left">25. Tipo de embalaje:</td>
         <td class="bt-bottom">
         <asp:DropDownList ID="dpembala" runat="server" Width="95%" 
                 onchange="valdpme(this,0,vaembala);" EnableViewState="False">
                 <asp:ListItem Selected="True" Value="0">* Tipo de embalaje*</asp:ListItem>
             </asp:DropDownList>
             </td>
         <td class="bt-bottom bt-right validacion "><span id="vaembala" class="validacion"> * obligatorio</span></td>
         </tr>

        <tr><td class="bt-bottom  bt-right bt-left">27. Código de peligrosidad (IMO):</td>
         <td class="bt-bottom">
           <asp:DropDownList ID="dpimo" runat="server" Width="95%" EnableViewState="False" >
                 <asp:ListItem Selected="True" Value="0">* Código de peligrosidad *</asp:ListItem>
             </asp:DropDownList>
             </td>
         <td class="bt-bottom bt-right validacion "><span id="valsimo" class="validacion"> </span></td>
         </tr>

     </table>
   <div id="opcional" class="colapser-option  colapsa">Carga para Acopio?, Click aqui y añada datos del contenedor</div>
       <div id="describir" class="accion">
      <table class="controles" cellspacing="0" cellpadding="1" id="CNTR">
        <tr><th class="bt-bottom bt-right  bt-left" colspan="3"> Datos del contenedor</th></tr>
         <tr>
         <td class="bt-bottom  bt-right bt-left" >28. Número del contenedor:</td>
         <td class="bt-bottom">
             <asp:TextBox ID="txtcontenedor" runat="server" Width="200px" MaxLength="11" CssClass="mayusc"
              onkeypress="return soloLetras(event,'abcdefghijklmnñopqrstuvwxyz1234567890')"
              onBlur="checkDC(this,'valcont',true);" EnableViewState="False"
              placeholder="Contenedor"
            
             ></asp:TextBox>
             </td>
         <td class="bt-bottom bt-right validacion "><span id="valcont" class="validacion"> * obligatorio</span></td>
         </tr>
         <tr><td class="bt-bottom  bt-right bt-left">29. Tara del contenedor [TON]:</td>
         <td class="bt-bottom">
                <span id="tara" class="caja cajafull">00.00</span>
             </td>
         <td class="bt-bottom bt-right validacion ">
         <span id="valtara"  class="validacion" > </span>
         
         </td>
         </tr>
         <tr><td class="bt-bottom  bt-right bt-left">30. Max.&nbsp;<i>Payload</i>&nbsp;contenedor[TON]:</td>
         <td class="bt-bottom">
             <asp:TextBox ID="txtpay" runat="server" Width="200px" 
              ClientIDMode="Static"
              placeholder="MaxPayLoad Ton."
                 MaxLength="5" 
                onBlur="valrange(this,tara.textContent,60,'valpayload',true);"
                onkeypress="return soloLetras(event,'1234567890.')" EnableViewState="False">00.00</asp:TextBox>
             </td>
         <td class="bt-bottom bt-right validacion "><span id="valpayload" class="validacion" > * obligatorio</span></td>
         </tr>
           <tr><td class="bt-bottom  bt-right bt-left">31. Origen del contenedor <i>(Depot)</i></td>
         <td class="bt-bottom" colspan="1">
          <a class="tooltip" >
          <span class="classic">Seleccione el depósito origen del contenedor y la fecha de entrega del mismo,
          esta información es obligatoria   </span>
            <asp:DropDownList ID="dporigen" runat="server" Width="160px"  >
                 <asp:ListItem Value="0">* Seleccione origen *</asp:ListItem>
             </asp:DropDownList>
              </a>
             retirado el día:

            <a class="tooltip" >
          <span class="classic">Fecha y Hora del retiro de la unidad <br /> [año/mes/dia hh:mm]</span>
             <input type="text"
             id="txtorigen"  maxlength="16"  style="width:120px!important;"   
             class="datetimepicker"
             onkeypress="return soloLetras(event,'1234567890/:')"
             placeholder="dd/mm/aaaa hh:mm"
             onblur="valDate(this,true,fechax);"
             />
             </a>
             </td>
         <td class="bt-bottom bt-right validacion "><span id="fechax" class="opcional" > * opcional</span></td>
         </tr>
     </table>
      <table class="controles" cellspacing="0" cellpadding="1" >
        <tr><th class="bt-bottom bt-right  bt-left" colspan="3"> Datos sobre refrigeración del contenedor</th></tr>
         <tr>
         <td class="bt-bottom  bt-right bt-left" >32. Tipo de refrigeración:</td>
             <td class="bt-bottom">
               <asp:DropDownList ID="dprefrigera" runat="server" Width="80%" onchange="valdpmeref(this,0,valrefri,tipor)"
                ClientIDMode="Static"
               >
                 <asp:ListItem Selected="True" Value="0">* Tipo de refrigeración *</asp:ListItem>
             </asp:DropDownList>
             <a class="tooltip" ><span class="classic" id='tipor'>Escoja el tipo para ver su descripción</span>
                        <img alt='' src='../shared/imgs/info.gif' class='datainfo'/>
              </a>
           </td>
         <td class="bt-bottom bt-right validacion "><span  class="opcional" id="valrefri" > * opcional</span></td>
         </tr>
         <tr><td class="bt-bottom  bt-right bt-left">33. Temperatura [°C]:</td>
         <td class="bt-bottom">
                     <asp:TextBox ID="txttemp" runat="server" Width="200px" MaxLength="5"
               onkeypress="return soloLetras(event,'1234567890-.')" ClientIDMode="Static"
               placeholder="Temperatura °C"
              
                 onblur="valrange(this,-25,25,'valtemp');" EnableViewState="False"    
             >0.0</asp:TextBox>
             </td>
        <td class="bt-bottom bt-right validacion "><span id='valtemp' class="opcional" > * opcional</span></td>
         </tr>
          <tr><td class="bt-bottom  bt-right bt-left">34. Humedad &nbsp;[CBM]:</td>
         <td class="bt-bottom">
          <asp:DropDownList ID="dphumedad" runat="server" Width="90%" >
                 <asp:ListItem Selected="True" Value="0">* Humedad *</asp:ListItem>
          </asp:DropDownList>
             </td>
         <td class="bt-bottom bt-right validacion "><span id='valhum' class="opcional"> * opcional</span></td>
         </tr>
         <tr>
         <td class="bt-bottom  bt-right bt-left">35. Ventilación&nbsp;[%]:</td>
         <td class="bt-bottom">
         <asp:DropDownList ID="dpventila" runat="server" Width="90%" EnableViewState="False">
                 <asp:ListItem Selected="True" Value="0">* Tipo de ventilación *</asp:ListItem>
          </asp:DropDownList>
         </td>
        <td class="bt-bottom bt-right"><span  class="opcional" id="valventi" >* opcional </span> </td>
         </tr>
     </table>
      <table class="controles" cellspacing="0" cellpadding="1">
        <tr><th class="bt-bottom bt-right  bt-left" colspan="3"> Detalle de los sellos del contenedor</th></tr>
         <tr>
         <td class="bt-bottom  bt-right bt-left" >36. Sello de agencia:</td>
         <td class="bt-bottom">
             <asp:TextBox ID="tseal1" runat="server" CssClass="mayusc" 
             onkeypress="return soloLetras(event,'abcdefghijklmnñopqrstuvwxyz1234567890',true)" 
             MaxLength="14"
             onblur="cadenareqerida(this,1,20,'valsel1');"
             placeholder="Sello agencia"
             onpaste="return false;" 
             Width="200px"></asp:TextBox>
             </td>
          <td class="bt-bottom bt-right validacion "><span id="valsel1" class="validacion" > * obligatorio</span></td>
         </tr>
         <tr>
         <td class="bt-bottom  bt-right bt-left" >37. Sello de ventilación [Reefer]:</td>
         <td class="bt-bottom">
             <asp:TextBox ID="tseal2" runat="server" CssClass="mayusc"
                 onkeypress="return soloLetras(event,'abcdefghijklmnñopqrstuvwxyz1234567890',true)" 
                 placeholder="Sello refrigeración"
                 MaxLength="14" Width="200px" EnableViewState="False"
                 onpaste="return false;" 
             ></asp:TextBox>
             </td>
         <td class="bt-bottom bt-right validacion"><span id="valselvent" class="opcional" > * opcional</span></td>
         </tr>
         <tr>
         <td class="bt-bottom  bt-right bt-left" >38. Sello adicional 1:</td>
         <td class="bt-bottom">
             <asp:TextBox ID="tseal3" runat="server" CssClass="mayusc"
                 onkeypress="return soloLetras(event,'abcdefghijklmnñopqrstuvwxyz1234567890',true)" 
                 MaxLength="14" Width="200px" EnableViewState="False"
                 placeholder="Sello opcional"
                 onpaste="return false;" 
             ></asp:TextBox>
             </td>
         <td class="bt-bottom bt-right ">
         <span class="opcional" > * opcional</span>
         </td>
         </tr>
         <tr>
         <td class="bt-bottom  bt-right bt-left" >39. Sello adicional 2:</td>
         <td class="bt-bottom">
             <asp:TextBox ID="tseal4" runat="server" CssClass="mayusc"
                 onkeypress="return soloLetras(event,'abcdefghijklmnñopqrstuvwxyz1234567890',true)" 
                 MaxLength="14" Width="200px" EnableViewState="False"
                 placeholder="Sello opcional"
                 onpaste="return false;" 
             ></asp:TextBox>
             </td>
         <td class="bt-bottom bt-right ">
         <span class="opcional" > * opcional</span>
         </td>
         </tr>
     </table>
      <table class="controles" cellspacing="0" cellpadding="1">
        <tr><th class="bt-bottom bt-right  bt-left" colspan="3"> Detalle del exceso de 
            dimensiones del contenedor
                 <a class="tooltip" ><span class="classic" id='exdim'>Aplica a contenedores descubiertos (OPEN TOP), Plataformas (FLAT RACK), entre otros.</span>
                <img alt='' src='../shared/imgs/info.gif' class='datainfo'/>
             </a>
            </th></tr>
         <tr>
         <td class="bt-bottom  bt-right bt-left" >40. Excede dimesiones nominales:</td>
         <td class="bt-bottom">
          <a class="tooltip" ><span class="classic">
          Marque para activar las dimensiones de exceso
          </span>
           <input id="ckdimen" type="checkbox" onclick="radio(this)"  />   
           </a>
             </td>
         <td class="bt-bottom bt-right validacion "></td>
         </tr>
         <tr><td class="bt-bottom  bt-right bt-left">41. Excesos izquierda [cm]:
          <i class="ileft"></i>
         </td>
         <td class="bt-bottom">
             <asp:TextBox ID="txtexizq" runat="server" MaxLength="4" Width="200px" 
              onkeypress="return soloLetras(event,'1234567890.')"
              placeholder="Izquierda"
               onblur="valrange(this,0,0,'xizq',true);"  
                 CssClass="xact" Enabled="False">0</asp:TextBox>
             </td>
         <td class="bt-bottom bt-right validacion "><span id="xizq" 
                 class="xalter validacion" > &nbsp;</span></td>
         </tr>
         <tr><td class="bt-bottom  bt-right bt-left">42. Excesos derecha [cm]:&nbsp;&nbsp;
          <i class="iright"></i>
         </td>
         <td class="bt-bottom">
             <asp:TextBox ID="txtexder" runat="server" MaxLength="4" Width="200px" 
              onkeypress="return soloLetras(event,'1234567890.')"  
              placeholder="Derecha"
               onBlur="valrange(this,0,0,'xder',true);"  
                 CssClass="xact" Enabled="False">0</asp:TextBox>
             </td>
         <td class="bt-bottom bt-right validacion "><span id="xder" 
                 class="xalter validacion" >  &nbsp;</span></td>
         </tr>
         <tr>
         <td class="bt-bottom  bt-right bt-left">43. Excesos frontal&nbsp;[cm]: 
         &nbsp;&nbsp;&nbsp;  <i class="ifront"></i>
         </td>
         <td class="bt-bottom">
             <asp:TextBox ID="txtexfro" runat="server" MaxLength="4" Width="200px" 
             placeholder="Frontal"
              onkeypress="return soloLetras(event,'1234567890.')"
               onBlur="valrange(this,0,0,'xfro',true);"   
                 CssClass="xact" Enabled="False">0</asp:TextBox>
             </td>
         <td class="bt-bottom bt-right validacion "><span id="xfro" 
                 class="xalter validacion" > &nbsp;</span></td>
         </tr>
         <tr><td class="bt-bottom  bt-right bt-left">44. Excesos atrás&nbsp;[cm]:&nbsp;&nbsp;
         &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<i class="ibotom"></i></td>
          
         <td class="bt-bottom">
             <asp:TextBox ID="txtexatra" runat="server" MaxLength="4" Width="200px" 
             placeholder="Posterior"
              onkeypress="return soloLetras(event,'1234567890.')"  
                onBlur="valrange(this,0,0,'xatr',true);" 
                 CssClass="xact" Enabled="False">0</asp:TextBox>
             </td>
         <td class="bt-bottom bt-right validacion "><span id="xatr" 
                 class="xalter validacion" > &nbsp;</span></td>
         </tr>
         <tr><td class="bt-bottom  bt-right bt-left">45. Excesos superior&nbsp;[cm]:
         &nbsp;
          <i class="itop"></i></td>
         <td class="bt-bottom">
             <asp:TextBox ID="txtexenc" runat="server" MaxLength="4" Width="200px" 
             placeholder="Superior"
              onkeypress="return soloLetras(event,'1234567890.')"  
               onBlur="valrange(this,0,0,'xsup',true);" 
                 CssClass="xact" Enabled="False">0</asp:TextBox>
             </td>
         <td class="bt-bottom bt-right validacion "><span id="xsup" 
                 class="xalter validacion" >  &nbsp;</span></td>
         </tr>
     </table>
       </div>
      <table class="controles" cellspacing="0" cellpadding="1" id="TRSP">
        <tr><th class="bt-bottom bt-right  bt-left" colspan="4"> Datos del transporte</th></tr>
           <tr>
         <td class="bt-bottom  bt-right bt-left" >46. Ubicación de la planta:</td>
        
         <td class="bt-bottom">
             <asp:DropDownList ID="dprovincia" runat="server" Width="150px" 
                 onchange="invoke(this,'canton',loadcanton); valdpme(this,0,valprovincia);" 
                 EnableViewState="False">
                 <asp:ListItem Selected="True" Value="0">Provincia</asp:ListItem>
             </asp:DropDownList>
             </td>
         <td class="bt-bottom">
             <select id="dcanton" style="width:190px;">
                <option selected="selected" value="0" >* Cantón *</option>
             </select>
         </td>
         <td class="bt-bottom bt-right validacion ">
            <span class="validacion" id="valprovincia" > * obligatorio</span>
            </td>
         </tr>
         <tr><td class="bt-bottom  bt-right bt-left">47. Fecha y hora de salida de planta:</td>
         <td class="bt-bottom" colspan="2">
           <asp:TextBox ID="txtSalida" runat="server" MaxLength="16" Width="250px" CssClass="datetimepicker"
               onkeypress="return soloLetras(event,'1234567890/:')"
               placeholder="dd/mm/aaaa hh:mm"
               onBlur="valDate(this,  true,valdatem);" EnableViewState="False"></asp:TextBox>
             </td>
                  <td class="bt-bottom bt-right validacion ">
            <span class="validacion" id="valdatem" > * obligatorio</span>
            </td>
         </tr>
            <tr><td class="bt-bottom  bt-right bt-left">48. Tiempo estimado de viaje [Hrs]:</td>
             <td class="bt-bottom" colspan="2">
             <asp:TextBox ID="txthoras" runat="server" MaxLength="3" Width="250px"
             onkeypress="return soloLetras(event,'1234567890',true)"
             placeholder="Horas viaje"
              onBlur="valrange(this,1,2000,'valestima',true);" EnableViewState="False"
             ></asp:TextBox>
             </td>
         <td class="bt-bottom bt-right validacion "><span class="validacion" id="valestima" > * obligatorio</span></td>
         </tr>

          <tr><td class="bt-bottom  bt-right bt-left">49. Nombre del conductor:</td>
         <td class="bt-bottom" colspan="2">
             <span id="txtconductor" class="caja cajafull"    runat="server" clientidmode="Static"  enableviewstate="False">...</span>

         
             </td>
         <td class="bt-bottom bt-right validacion "><span class="validacion" id="valnombre" ></span></td>
         </tr>
         		         <tr><td class="bt-bottom  bt-right bt-left">50. Documento de identidad:</td>
         <td class="bt-bottom" >
         <input id="driID" type="hidden" runat="server" clientidmode="Static"  enableviewstate="False" />
         <span id="txtidentidad" class="caja cajafull "   
         runat="server" clientidmode="Static"  enableviewstate="False">...</span>
      

             </td>
              <td class="bt-bottom finder"  >
             <a  class="topopup" target="popup" onclick="window.open('../catalogo/chofer','name','width=850,height=480')" >
           <i class="ico-find" ></i> Buscar</a>
         </td>

         <td class="bt-bottom bt-right validacion "><span class="validacion" id="valced" > * obligatorio</span></td>
         </tr>
     
         <tr><td class="bt-bottom  bt-right bt-left">51. Placa del vehículo:</td>
         <td class="bt-bottom" colspan="2">
             <asp:TextBox ID="txtplaca" runat="server" MaxLength="10" CssClass="mayusc"
             onkeypress="return soloLetras(event,'abcdefghijklmnopqrstuvwxyz0123456789',true)" Width="250px"
             onblur="validarPlaca(this,'valplaca');"
             placeholder="AAA0000"
             EnableViewState="False"
              ></asp:TextBox>
             </td>
         <td class="bt-bottom bt-right validacion "><span class="validacion" id="valplaca" > * obligatorio</span></td>
         </tr>
         <tr><td class="bt-bottom  bt-right bt-left">52. Certificado de Cabezal:</td>
               <td class="bt-bottom" colspan="2">
          <a class="tooltip" ><span class="classic">Si no agrega esta información, la misma será solicitada en la garita de ingreso</span>
             <asp:TextBox ID="certcabezal" runat="server" MaxLength="8" CssClass="mayusc"
             placeholder="Certificado"
            onkeypress="return soloLetras(event,'abcdefghijklmnopqrstuvwxyz0123456789',true)" EnableViewState="False"
              ></asp:TextBox> </a>
              Expira el:
              <input 
              id="txtcerCab" 
              type="text" 
              maxlength="16" 
              style=" width:120px!important;"  
              class="datepicker" 
              onkeypress="return soloLetras(event,'1234567890/:')"
               onblur="valDate(this,false,fechacab);"
               placeholder="dd/mm/aaaa"
                   />

             </td>
         <td class="bt-bottom bt-right validacion "><span class="opcional" id="fechacab" > * opcional</span></td>
         </tr>
         <tr><td class="bt-bottom  bt-right bt-left">53. Certificado de Chasis:</td>
               <td class="bt-bottom" colspan="2">
         <a class="tooltip" >
             <span class="classic">Si no agrega está información la misma será solicitada, en la garita de ingreso</span>
             <asp:TextBox ID="certchasis" runat="server" MaxLength="8" CssClass="mayusc"
             placeholder="Certificado"
                onkeypress="return soloLetras(event,'abcdefghijklmnopqrstuvwxyz0123456789',true)" EnableViewState="False"
              ></asp:TextBox>
              </a>
            Expira el:
              <input type="text" id="txtcerCha"  
               maxlength="16"  style=" width:120px!important;"   class="datepicker"
               onkeypress="return soloLetras(event,'1234567890/:')"
               onblur="valDate(this,false,fechach);" 
               placeholder="dd/mm/aaaa"
              />

             </td>
         <td class="bt-bottom bt-right validacion "><span class="opcional" id="fechach" > * opcional</span></td>
         </tr>
         <tr><td class="bt-bottom  bt-right bt-left">54. Certificado especial:</td>
         <td class="bt-bottom" colspan="2">
             <asp:TextBox ID="certespecial" runat="server" MaxLength="8" CssClass="mayusc"
               onkeypress="return soloLetras(event,'abcdefghijklmnopqrstuvwxyz0123456789')" EnableViewState="False"
               placeholder="Certificado"
              ></asp:TextBox>
             </td>
         <td class="bt-bottom bt-right validacion ">
         <span class="opcional" > * opcional</span>
         </td>
         </tr>
                  <tr><td class="bt-bottom  bt-right bt-left">55. Compañía de Transporte:</td>
         <td class="bt-bottom" colspan="2">
             <asp:TextBox ID="tcompania" runat="server" MaxLength="100" Width="90%" CssClass="mayusc"
             onkeypress="return soloLetras(event,'abcdefghijklmnopqrstuvwxyzñ')"
              onBlur="cadenareqerida(this,1,200,'valtc');" placeholder="Compania de transporte"
             ></asp:TextBox>
                  <a class="tooltip" ><span class="classic" >
                  Nombre de la compañia transportista que realiza la entrega de la unidad de carga en CGSA.
                  
                  </span>
                        <img alt='' src='../shared/imgs/info.gif' class='datainfo'/>
              </a>

             </td>
         <td class="bt-bottom bt-right validacion "><span class="validacion" id="valtc" > * obligatorio</span></td>
         </tr>
         
         <tr><td class="bt-bottom  bt-right bt-left">56. RUC:</td>
         <td class="bt-bottom" colspan="2">
             <asp:TextBox ID="truc" runat="server" MaxLength="15" Width="250px"
              onkeypress="return soloLetras(event,'0123456789',true)"
               placeholder="RUC"
               onBlur="cadenareqerida(this,1,20,'valruc');"
             ></asp:TextBox>
                        <a class="tooltip" ><span class="classic" >
                        Número del Registro Único de Contribuyente de la compañia transportista registrada anteriormente.
                        
                        </span>
                        <img alt='' src='../shared/imgs/info.gif' class='datainfo'/>
              </a>
             </td>
         <td class="bt-bottom bt-right validacion "><span class="validacion" id="valruc" > * obligatorio</span></td>
         </tr>
     </table>
      <table class="controles" cellspacing="0" cellpadding="1">
        <tr><th class="bt-bottom bt-right  bt-left" colspan="3"> Datos para notificación</th></tr>
         <tr>
         <td class="bt-bottom  bt-right bt-left" >57. Correo(s) para notificación: 
         <i><small>(Estos correos se utilizarán para notificar en caso que sea necesaria una inspección)</small></i>
         </td>
         <td class="bt-bottom">
         <div id='TextBoxesGroup'>
           <div id="TextBoxDiv1" class="cntmail">
               <span>mail #1:</span><input type='text' id='textbox1' name='textbox1' 
               enableviewstate="false" clientidmode="Static" runat="server"
               onkeypress="return soloLetras(event,'abcdefghijklmnñopqrstuvwxyz1234567890_$@.',true)"  
                onblur="cadenareqerida(this,1,100,'valmail');"
                placeholder="mail@mail.com"
               />
           </div>
         </div>
             <input type='button' value='Agregar' id='addButton' />
             <input type='button' value='Remover' id='removeButton' />
          </td>
          <td class="bt-bottom bt-right validacion "><span id="valmail" class="validacion"> * obligatorio</span></td>
         </tr>
         </table>
     <div class="informativo">
      <table>
      <tr><td rowspan="2" class="inum"> <div class="number">4</div></td><td class="level1" >Responsabilidad de la información</td></tr>
      <tr>
      <td class="level2">
             <div class=" msg-critico">
       Los datos proporcionados son de entera responsabilidad de quien los consigna, por lo que CONTECON GUAYAQUIL S.A. no se responsabiliza por cualquier error o falsedad que los mismos pudieren tener, siendo de cuenta del cliente todos los gastos y perjuicios que por dicho error se ocasionen a la carga.
       <br />CGSA otorgará la información proporcionada en este documento a las autoridades competentes a la operación de exportación cuando sean solicitadas.
      </div>
      </td>
      </tr>
      </table>
     </div>
     <div class="botonera">
         <img alt="loading.." src="../shared/imgs/loader.gif" id="loader" class="nover"  />
         <input id="btsalvar" type="button" value="Generar AISV" onclick="imprimir();" /> &nbsp;
         <input id="btclear"   type="reset"     value="Borrar formulario" 
             onclick="fullreset(this.form,true);"/>
     </div>
     <div id="secciones">
              Secciones:&nbsp;
             <a href="#BUSCAR">Datos del Booking</a>
             <a href="#ADU" >Datos de Aduana</a> 
             <a href="#CNTR">Datos del Contenedor</a>
             <a href="#TRSP">Datos del Transporte</a>
        </div>
     </div>
    </div>

    <script src="../Scripts/pages.js" type="text/javascript"></script>
    <script type="text/javascript">
        var jAisv = {};
        $(window).load(function () {
            //objeto a transportar.
            $(document).ready(function () {
                //init pickers
                $('.datetimepicker').datetimepicker().datetimepicker({ lang: 'es', step: 30, format: 'd/m/Y H:i' });
                //inicia los fecha
                $('.datepicker').datetimepicker().datetimepicker({ lang: 'es', timepicker: false, format: 'd/m/Y', closeOnDateSelect: true, minDate: '0' });
                //init excesos
                $('.xalter').text('');
                //colapsar y expandir
                $("div.colapser").toggle(function () { $(this).removeClass("colapsa").addClass("expande"); $(this).next().hide(); }
                                  , function () { $(this).removeClass("expande").addClass("colapsa"); $(this).next().show(); });

                $("#opcional").toggle(function () { $(this).text('Click aqui para añadir los datos del contenedor de acopio'); $(this).removeClass("colapsa").addClass("expande"); $(this).next().hide(); }
                                  , function () { $(this).text('Clic aquí para añadir ocultar información del contenedor'); $(this).removeClass("expande").addClass("colapsa"); $(this).next().show(); });

                $('#opcional').click();
                //poner valor en campo
                var da = new Date();
                document.getElementById('txtdae2').value = da.getFullYear();
                //controlador de mails
                var counter = 2;
                $("#addButton").click(function () {
                    if (counter > 5) {
                        alert("Solo se permiten 5 mails");
                        return false;
                    }
                    $('<div/>', { 'id': 'TextBoxDiv' + counter }).html($('<span/>').html('mail #' + counter + ':')).append($('<input type="text" placeholder="mail@mail.com ">').attr({ 'id': 'textbox' + counter, 'name': 'textbox' + counter, onkeypress: 'return soloLetras(event,"abcdefghijklmnñopqrstuvwxyz1234567890_$@.",true);' })).appendTo('#TextBoxesGroup')
                    counter++;
                });
                $("#removeButton").click(function () {
                    if (counter == 2) {
                        alert("Un mail es obligatorio");
                        return false;
                    }
                    counter--;
                    $("#TextBoxDiv" + counter).remove();
                });
            });
        });
        //Esta funcion va a validar que cuando presionen booking debe poner los 3 parametros
        function validateBook(objeto) {
            /*Recordar vaciar los span*/
            var expnum = document.getElementById('numexpo');
            var linebook = document.getElementById('linea');
            if (expnum.textContent.length > 0 && linebook.textContent.length > 0 && objeto.numero.length > 0) {
                document.getElementById('xplinea').textContent = '';
                document.getElementById('xpbok').textContent = '';
                document.getElementById('numbook').textContent = objeto.numero;
                document.getElementById('referencia').textContent = objeto.referencia;
                document.getElementById('buque').textContent = objeto.nave;
                document.getElementById('eta').textContent = objeto.eta;
                document.getElementById('cutof').textContent = objeto.cutoff;
                document.getElementById('uis').textContent = objeto.uis; ;
                document.getElementById('agencia').textContent = linebook.textContent;
                document.getElementById('descarga').textContent = objeto.pod;
                document.getElementById('final').textContent = objeto.pod1;
                document.getElementById('producto').textContent = objeto.comoditi;
                document.getElementById('tamano').textContent = objeto.longitud;
                document.getElementById('tipo').textContent = objeto.iso;
                document.getElementById('tara').textContent = objeto.tara;
                document.getElementById('remar').textContent = objeto.remark;
                this.document.getElementById('refer').checked = objeto.refer;
                this.document.getElementById('imo').checked = objeto.bimo;
                this.jAisv.bnumber = objeto.numero; //numero de booking
                this.jAisv.breferencia = objeto.referencia; //referencia de nave
                this.jAisv.bfk = objeto.fk; // freightkind
                this.jAisv.bnave = objeto.nave; // nombre de nave
                this.jAisv.beta = objeto.eta; //fecha eta
                this.jAisv.bcutOff = objeto.cutoff; //fecha cutoff
                this.jAisv.buis = objeto.uis; //ultimo ingreso sugerido
                this.jAisv.bagencia = linebook.textContent; // nombre de la agencia/linea
                this.jAisv.bpod = objeto.pod; //pto desc1
                this.jAisv.bpod1 = objeto.pod1; //pto desc2
                this.jAisv.bcomodity = objeto.comoditi; // notas del booking
                this.jAisv.bsizeu = objeto.longitud; //longitud de unit booking
                this.jAisv.btipou = objeto.iso; //iso del boking
                this.jAisv.breefer = objeto.refer; //es reefer booking
                this.jAisv.gkey = objeto.gkey;
                this.jAisv.bitem = objeto.item; //id de item de booking
                this.jAisv.breserva = objeto.reserva; // cant reserva
                this.jAisv.busa = objeto.usa; //cant usa
                this.jAisv.bimo = objeto.bimo; //Imo del booking
                this.jAisv.bdispone = objeto.dispone; //dispone booking
                this.jAisv.utara = objeto.tara; //tara de la unidad
                this.jAisv.cimo = objeto.imo; // este imo es por cada unidad, debe reemplar cs
                this.jAisv.remark = objeto.remark;
                this.jAisv.utemp = objeto.temp;
                this.jAisv.shipid = objeto.shipid;
                this.jAisv.shipname = objeto.shipname;
                this.jAisv.hzkey = objeto.hzkey;
                this.jAisv.vent_pc = objeto.vent_pc;
                this.jAisv.ventu = objeto.ventu;
                this.jAisv.uhumedad = objeto.hume;
                
                this.document.getElementById('txttemp').value = objeto.temp;
                return true;
            }
            else {
                alert('Por favor use los botones de búsqueda para los 3 parametros');
                return false;
            }
        }
        function imprimir() {
            var cntr = document.getElementById('<%=txtcontenedor.ClientID %>').value;
            if (setWarningContainer(cntr)) {
                getPrint(this.jAisv, 'cargasuelta.aspx/ValidateJSON');
            }
        }

        function prepareObject() {
            this.jAisv.secuencia = '0'; //asiv secuencia
            this.jAisv.idexport = document.getElementById('numexport').value; //id export
            this.jAisv.idline = document.getElementById('lineaCI').value; // linea
            this.jAisv.tipo = 'ES'; //tipo aisv
            this.jAisv.aidagente = document.getElementById('caeid').value; //id agente
            this.jAisv.adocnumero = document.getElementById('txtdae1').value + document.getElementById('txtdae2').value + document.getElementById('txtdae3').value
            this.jAisv.adoctipo = document.getElementById('dpdoc').value; //tipo dae,djt etc..
            this.jAisv.unumber = document.getElementById('<%=txtcontenedor.ClientID %>').value; //cntr number
            this.jAisv.umaxpay = this.document.getElementById('txtpay').value; //cntr maxpayload
            this.jAisv.upeso = this.document.getElementById('<%= txtpeso.ClientID %>').value; //cntr,cargas peso
            this.jAisv.uidrefri = this.document.getElementById('<%= dprefrigera.ClientID %>').value; //cntr refrigeración
            this.jAisv.utemp = this.document.getElementById('txttemp').value;
           
           // this.jAisv.uhumedad = this.document.getElementById('<%= dphumedad.ClientID %>').value;
           
            this.jAisv.uidventila = this.document.getElementById('<%= dpventila.ClientID %>').value;
            this.jAisv.seal1 = this.document.getElementById('<%= tseal1.ClientID %>').value;
            this.jAisv.seal2 = this.document.getElementById('<%= tseal2.ClientID %>').value;
            this.jAisv.seal3 = this.document.getElementById('<%= tseal3.ClientID %>').value;
            this.jAisv.seal4 = this.document.getElementById('<%= tseal4.ClientID %>').value;
            this.jAisv.eHas = this.document.getElementById('ckdimen').checked; // cntr tiene exceso
            this.jAisv.eleft = this.document.getElementById('<%= txtexizq.ClientID %>').value;
            this.jAisv.eright = this.document.getElementById('<%= txtexder.ClientID %>').value;
            this.jAisv.efront = this.document.getElementById('<%= txtexfro.ClientID %>').value;
            this.jAisv.etop = this.document.getElementById('<%= txtexenc.ClientID %>').value;
            this.jAisv.eback = this.document.getElementById('<%= txtexatra.ClientID %>').value;
            this.jAisv.tidubica = this.document.getElementById('<%= dprovincia.ClientID %>').value;
            this.jAisv.tidcanton = this.document.getElementById('dcanton').value;
            this.jAisv.tfechadoc = this.document.getElementById('<%= txtSalida.ClientID %>').value;
            this.jAisv.tviaje = this.document.getElementById('<%= txthoras.ClientID %>').value;

            this.jAisv.tconductor = this.document.getElementById('txtconductor').textContent; //nombre del chofer
            this.jAisv.tdocument = this.document.getElementById('<%= driID.ClientID %>').value; // id del chofer

            this.jAisv.tplaca = this.document.getElementById('<%= txtplaca.ClientID %>').value;
            this.jAisv.tcabcert = this.document.getElementById('<%= certcabezal.ClientID %>').value;
            this.jAisv.tcabcertfecha = this.document.getElementById('txtcerCab').value;
            this.jAisv.tchacert = this.document.getElementById('<%= certchasis.ClientID %>').value;
            this.jAisv.tcabchafecha = this.document.getElementById('txtcerCha').value;
            this.jAisv.tespcert = this.document.getElementById('<%= certespecial.ClientID %>').value;
            this.jAisv.autor = 'token';
            this.jAisv.acupo = this.document.getElementById('ckcupo').checked;
            this.jAisv.aidinst = this.document.getElementById('<%= dpins.ClientID %>').value;
            this.jAisv.aidrule = this.document.getElementById('dprule').value;
            this.jAisv.bdeck = this.document.getElementById('rbsi').checked ? true : false; //bajo cubierta
            this.jAisv.ubultos = this.document.getElementById('<%= txtbultos.ClientID %>').value;
            this.jAisv.thoras = this.document.getElementById('<%= txthoras.ClientID %>').value;
            this.jAisv.cimo = this.document.getElementById('<%= dpimo.ClientID %>').value;
            this.jAisv.cembalaje = this.document.getElementById('<%= dpembala.ClientID %>').value;
            //deposito
            this.jAisv.udepo = this.document.getElementById('<%= dporigen.ClientID %>').value; //id del deposito
            this.jAisv.udepofecha = this.document.getElementById('txtorigen').value; // fecha hora salida
            this.jAisv.nomexpo = this.document.getElementById('nomexpo').textContent;

            //todos
            this.jAisv.trancia = this.document.getElementById('<%= tcompania.ClientID %>').value;
            this.jAisv.tranruc = this.document.getElementById('<%= truc.ClientID %>').value;

            //2017->Consignatario-diplomatico
            this.jAisv.diplomatico = this.document.getElementById('ckdip').checked; //usa cupos
            this.jAisv.consignatario = this.document.getElementById('<%= consignee.ClientID %>').value;
        }
        function popupCallback(data, control) {
            this.document.getElementById(control).value = data;
        }
        function valdata(control) {
            var bitem = this.jAisv.gkey;
            var texto = control.value;
            if (bitem != undefined && bitem != null && bitem.trim().length > 0 && texto.trim().length > 0 && !isNaN(texto)) {
                setBulk(bitem);
            }
        }

        function driverCallback(data) {
            this.document.getElementById('driID').value = data.codigo;
            this.document.getElementById('txtconductor').textContent = data.descripcion;
            this.document.getElementById('txtidentidad').textContent = data.codigo;
        }
    </script>
</asp:Content>

