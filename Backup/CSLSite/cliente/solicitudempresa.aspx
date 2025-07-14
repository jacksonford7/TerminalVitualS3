<%@ Page Language="C#" Title="Registro de Empresa" MaintainScrollPositionOnPostback="true"
         AutoEventWireup="true" CodeBehind="solicitudempresa.aspx.cs" Inherits="CSLSite.cliente.solicitudempresa" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<%@ Register Assembly="MSCaptcha" Namespace="MSCaptcha" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Registro de Empresa</title>
    <link href="../shared/estilo/aisv-general.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/work.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/tables.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/solicitudempresa.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/info.css" rel="stylesheet" type="text/css" />
    <script src="Scripts/jquery-1.7.1.min.js" type="text/javascript"></script>
    <script src="../Scripts/jquery-1.7.1.min.js" type="text/javascript"></script>
    <link href="Inicio/base-site.css" rel="stylesheet" type="text/css" />
    <link href="Inicio/css/font-awesome.min.css" rel="stylesheet" type="text/css" />
    <link rel="shortcut icon" type="image/x-icon" href="favicon.ico" />
    <link href="shared/estilo/work.css" rel="stylesheet" type="text/css" />
    <link href="../Inicio/base-site.css" rel="stylesheet" type="text/css" />
    <link href="../Inicio/css/font-awesome.min.css" rel="stylesheet" type="text/css" />
    <link rel="../shortcut icon" type="image/x-icon" href="../favicon.ico" />
    <link href="../shared/estilo/work.css" rel="stylesheet" type="text/css" />
     <script src="Scripts/jquery-1.7.1.min.js" type="text/javascript"></script>
    <script src="../Scripts/jquery-1.7.1.min.js" type="text/javascript"></script>
    <link href="Inicio/base-site.css" rel="stylesheet" type="text/css" />
    <link href="Inicio/css/font-awesome.min.css" rel="stylesheet" type="text/css" />
    <link rel="shortcut icon" type="image/x-icon" href="favicon.ico" />
    <link href="shared/estilo/work.css" rel="stylesheet" type="text/css" />
    <link href="../Inicio/base-site.css" rel="stylesheet" type="text/css" />
    <link href="../Inicio/css/font-awesome.min.css" rel="stylesheet" type="text/css" />
    <link rel="../shortcut icon" type="image/x-icon" href="../favicon.ico" />
    <link href="../shared/estilo/work.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
     * input[type=text]
        {
            text-align:left!important;
        }
        .style1
        {
            border-bottom: 1px solid #CCC;
            width: 530px;
        }
    </style>
   <%-- <script type="text/javascript">
        function BindFunctions() {
            document.getElementById('imagen').innerHTML = '';
            $(document).ready(function () {
                $('#tablasort').tablesorter({ cancelSelection: true, cssAsc: "ascendente", cssDesc: "descendente", headers: { 5: { sorter: false }, 6: { sorter: false}} }).tablesorterPager({ container: $('#pager'), positionFixed: false, pagesize: 10 });
            });
        }
    </script>--%>
</head>
<body>
    <noscript><meta http-equiv="refresh" content="0; url=sinsoporte.htm" /> </noscript>
    <form id="bookingfrm" runat="server">
    <input id="zonaid" type="hidden" value="7" />
    <asp:ToolkitScriptManager ID="tkscata" runat="server"></asp:ToolkitScriptManager>
     <div class="catabody" >
     <div class="catawrap" >
   <div>
         <i class="ico-titulo-1"></i><h2>MCA - Módulo de Control de Acceso</h2><br />
         <i class="ico-titulo-2"></i><h1>Formulario de Registro de Empresa</h1><br />
   </div>
   <div class="seccion">
      <div class="informativo">
      <table class="controles" cellspacing="0" cellpadding="1">
      <tr><td rowspan="2" class="inum"> <div class="number">1</div></td><td class="level1" >Tipo de Cliente - Empresa.</td></tr>
      <tr><td class="level2">Tipo de Cliente - Empresa.</td></tr>
      </table>
     </div>
     <div class="colapser colapsa"></div>
     <div class="accion">
     <table class="controles" cellspacing="0" cellpadding="1">
        <tr>
        <td class="bt-bottom  bt-right bt-left" style=" width:150px">1. Tipo de Cliente:</td>
        </tr>
     </table>
     <div class="bt-bottom  bt-right bt-left">
     <table cellspacing="0" cellpadding="1" style=" width:100%">
     <tr>
     <td class=""><span style="font-size:smaller; font-family:Consolas; font-weight:normal; font-style:italic; color:Red">* obligatorio / seleccione al menos un valor</span></td>
     </tr>
     <tr>
     <td class="">
     <div style=" height:150px; overflow-y:scroll" class="bt-right">
     <%--<fieldset>--%>
     <%--<legend style="font-size:smaller; font-family:Consolas; font-weight:normal; font-style:italic; color:Red"></legend>--%>
     <asp:CheckBoxList ID="cbltipousuario" runat="server" Width="400px"
             onchange="valcbl();">
            </asp:CheckBoxList>
     <%--</fieldset>--%>
     </div>
     </td>
     </tr>
     </table>
     </div>
    </div>
    </div>
   <div class="seccion">
      <div class="informativo">
      <table>
      <tr><td rowspan="2" class="inum"> <div class="number">2</div></td><td class="level1" >Información del Cliente.</td></tr>
      <tr><td class="level2">
         Confirme que los datos sean correctos.
      </td></tr>
      </table>
     </div>
    <div class="colapser colapsa"></div>
      <div class="accion">
      <table class="controles" cellspacing="0" cellpadding="1">
         <tr>
         <td class="bt-bottom  bt-right bt-left" style=" width:155px" >2. Nombre/Razón Social:</td>
         <td class="bt-bottom">
           <asp:TextBox ID="txtrazonsocial" runat="server" Width="400px" MaxLength="500" onblur="checkcajalarge(this,'valrazsocial',true);"
             style="text-align: center;" 
             onkeypress="return soloLetras(event,'abcdefghijklnmñopqrstuvwxyz01234567890.-_/& ',true)"></asp:TextBox>
         </td>
         <td class="bt-bottom bt-right validacion ">
         <span id="valrazsocial" class="validacion"> * obligatorio</span>
         </td>
         </tr>
      </table>
      <table class="controles" cellspacing="0" cellpadding="1">
         <tr>
         <td class="bt-bottom  bt-right bt-left" style=" width:250px" >
         3. RUC [<input id="rbruc"  checked="checked"  type="radio" name="deck" value="ruc" />] C.I [ <input id="rbci"  type="radio" name="deck" value="ci"/>] Pasaporte [<input id="rbpasaporte"  type="radio" name="deck" value="pas"/>]
         </td>
         <td class="bt-bottom">
            <asp:TextBox ID="txtruccipas" runat="server" Width="200px" MaxLength="25"
             style="text-align: center"
             onBlur="valruccipas(this,'valruccipas','rbruc','rbci','rbpasaporte');"
             onkeypress="return soloLetras(event,'01234567890abcdefghijklnmñopqrstuvwxyz_-',true)"></asp:TextBox>
         </td>
         <td align="right" class="bt-bottom bt-right validacion "  style="width:200px"><span class="validacion" id="valruccipas" > * obligatorio</span></td>
         </tr>
      </table>
      <table class="controles" cellspacing="0" cellpadding="1">
         <tr><td class="bt-bottom  bt-right bt-left" style=" width:155px">4. Actividad Comercial:</td>
         <td class="bt-bottom">
            <asp:TextBox ID="txtactividadcomercial" runat="server" Width="400px" MaxLength="500"
             style="text-align: center" onblur="checkcajalarge(this,'valactcom',true);"
             onkeypress="return soloLetras(event,'abcdefghijklnmñopqrstuvwxyz01234567890 ',true)"></asp:TextBox>
         </td>
         <td class="bt-bottom bt-right validacion "><span id="valactcom" class="validacion"> * obligatorio</span></td>
         </tr>
         <tr><td class="bt-bottom  bt-right bt-left" style=" width:155px">5. Dirección Oficina:</td>
         <td class="bt-bottom">
            <asp:TextBox ID="txtdireccion" runat="server" Width="400px" MaxLength="60"
             style="text-align: center" onblur="checkcajalarge(this,'valdir',true);"
             onkeypress="return soloLetras(event,'abcdefghijklnmñopqrstuvwxyz01234567890/.-_ ',true)"></asp:TextBox>
         </td>
         <td class="bt-bottom bt-right validacion "><span id="valdir" class="validacion"> * obligatorio</span></td>
         </tr>
         <tr>
         <td class="bt-bottom  bt-right bt-left" style=" width:155px" >6. Teléfono Oficina:</td>
         <td class="bt-bottom">
            <asp:TextBox ID="txttelofi" runat="server" Width="200px" MaxLength="9"
             style="text-align: center"
             onBlur="telconvencional(this,'valtelofi',true);"
             onkeypress="return soloLetras(event,'01234567890',true)"></asp:TextBox>
         </td>
         <td class="bt-bottom bt-right validacion "><span id="valtelofi" class="validacion"> * obligatorio</span></td>
         </tr>
      </table>
      <table class="controles" cellspacing="0" cellpadding="1">
         <tr>
         <td class="bt-bottom  bt-right bt-left" style=" width:155px" >7. Persona Contacto:</td>
         <td class="bt-bottom">
           <asp:TextBox ID="txtcontacto" runat="server" Width="200px" MaxLength="500"
             style="text-align: center" onblur="checkcaja(this,'valcontacto',true);"
             onkeypress="return soloLetras(event,'abcdefghijklnmñopqrstuvwxyz01234567890 ',true)"></asp:TextBox>
         </td>
         <td class="bt-bottom bt-right validacion "><span id="valcontacto" class="validacion"> * obligatorio</span></td>
         </tr>
         <tr>
         <td class="bt-bottom  bt-right bt-left" style=" width:155px" >8. Celular Contacto:</td>
         <td class="bt-bottom">
            <asp:TextBox ID="txttelcelcon" runat="server" Width="200px" MaxLength="10"
             style="text-align: center"
             onBlur="telcelular(this,'valtelcel',true);"
             onkeypress="return soloLetras(event,'01234567890',true)"></asp:TextBox>
         </td>
         <td class="bt-bottom bt-right validacion " ><span id="valtelcel" class="validacion"> * obligatorio</span></td>
         </tr>
     </table>
      <table id="tbmail1" class="controles" cellspacing="0" cellpadding="1">
         <tr>
         <td class="bt-bottom  bt-right bt-left" style=" width:155px;" >9. Mail Contacto:</td>
         <td class="bt-bottom">
         <%--<div id='TextBoxesGroup'>
           <div id="TextBoxDiv1" class="cntmail">--%>
               <%--<span>mail:</span>--%>
               <a class="tooltip"><span class="classic">
                Para mas de un Mail separelos con punto y coma.</span>
               <input type='text' id='tmailinfocli' runat="server" name='textboxmailinfocli' style=" width:400px"
                enableviewstate="false" clientidmode="Static"
                onkeypress="return soloLetras(event,'abcdefghijklmnñopqrstuvwxyz1234567890_$@;.',true)"
                onblur="mailone(this,'valmail');"
                placeholder="mail@mail.com"
                />
                </a>
           <%--</div>
         </div>--%>
             <%--<input type='button' value='Agregar' id='addButton' />
             <input type='button' value='Remover' id='removeButton' />--%>
          </td>
          <td class="bt-bottom bt-right validacion "><span id="valmail" class="validacion"> * obligatorio</span></td>
         </tr>
         </table>
    <table id="Table1" class="controles" cellspacing="0" cellpadding="1">
             <tr>
         <td class="bt-bottom  bt-right bt-left" style=" width:155px;" >10. Mail EBilling:</td>
         <td class="bt-bottom">
         <%--<div id='TextBoxesGroup'>
           <div id="TextBoxDiv1" class="cntmail">--%>
               <%--<span>mail:</span>--%>
               <a class="tooltip"><span class="classic">
                Para mas de un Mail separelos con punto y coma.</span>
               <input type='text' id='tmailebilling' runat="server" name='textboxmailinfocli' style=" width:400px"
                enableviewstate="false" clientidmode="Static"
                onkeypress="return soloLetras(event,'abcdefghijklmnñopqrstuvwxyz1234567890_$@;.',true)"
                onblur="mailone(this,'valmaileb');"
                placeholder="mail@mail.com"
                />
                </a>
           <%--</div>
         </div>--%>
             <%--<input type='button' value='Agregar' id='addButton' />
             <input type='button' value='Remover' id='removeButton' />--%>
          </td>
          <td class="bt-bottom bt-right validacion "><span id="valmaileb" class="validacion"> * obligatorio</span></td>
         </tr>
      </table>
      <table class="controles" cellspacing="0" cellpadding="1">
         <tr>
         <td class="bt-bottom  bt-right bt-left" style=" width:155px" >11. Certificaciones:</td>
         <td class="style1">
           <asp:TextBox ID="txtcertificaciones" runat="server" Width="400px" MaxLength="500"
             style="text-align: center; text-transform: uppercase" placeholder="Para varias Certificaciones separelos con punto y coma."
             
                 onkeypress="return soloLetras(event,'abcdefghijklnmñopqrstuvwxyz01234567890;./-_ ',true)"></asp:TextBox>
         </td>
        <td class="bt-bottom bt-right"><span style="font-size:small; font-family:Consolas; font-weight:normal; font-style:italic; color:Green" id="Span1"> * opcional</span></td>
         </tr>
      </table>
      <table class="controles" cellspacing="0" cellpadding="1">
         <tr>
         <td class="bt-bottom  bt-right bt-left" style=" width:155px" >12. Sitio Web:</td>
         <td class="style1">
             <asp:TextBox
              placeholder="http https://www.dominio.com .ec .es .org etc."
              id='turl' runat="server" style= 'width:400px; font-size:small;text-transform: uppercase'
              enableviewstate="false" clientidmode="Static"
              onkeypress="return soloLetras(event,'abcdefghijklmnopqrstuvwxyz1234567890_:.-/;',true)"  
              maxlength="250"> </asp:TextBox>
         </td>
         <%--<td class="bt-bottom bt-right validacion "><span id="valsitioweb" class="validacion"> * obligatorio</span></td>--%>
         <td class="bt-bottom bt-right"><span style="font-size:small; font-family:Consolas; font-weight:normal; font-style:italic; color:Green" id="Span4"> * opcional</span></td>
         </tr>
      </table>
      <table class="controles" cellspacing="0" cellpadding="1">
         <tr><td class="bt-bottom bt-left bt-right" style=" width:155px">13. Afiliación a Gremios:</td>
         <td class="style1">
            <asp:TextBox ID="txtafigremios" runat="server" Width="400px" MaxLength="1000"
             style="text-align: center;text-transform: uppercase" placeholder="Para varios Gremios separelos con punto y coma."
             onkeypress="return soloLetras(event,'abcdefghijklnmñopqrstuvwxyz01234567890;./-_ ',true)"></asp:TextBox>
         </td>
         <td class="bt-bottom bt-right "><span style="font-size:small; font-family:Consolas; font-weight:normal; font-style:italic; color:Green"  id="Span3"> * opcional</span></td>
         </tr>
         <tr><td class="bt-bottom  bt-right bt-left" style=" width:155px">14. Referencia Comercial:</td>
         <td class="style1">
            <asp:TextBox ID="txtrefcom" runat="server" Width="400px" MaxLength="500"
             style="text-align: center;text-transform: uppercase" placeholder="Para varias referencias separelos con punto y coma."
             onkeypress="return soloLetras(event,'abcdefghijklnmñopqrstuvwxyz01234567890;./-_ ',true)"></asp:TextBox>
         </td>
         <td class="bt-bottom bt-right"><span style="font-size:small; font-family:Consolas; font-weight:normal; font-style:italic; color:Green" id="Span2"> * opcional</span></td>
         </tr>
      </table>
     </div>
    </div>
   <div class="seccion">
      <div class="informativo">
      <table>
      <tr><td rowspan="2" class="inum"> <div class="number">3</div></td><td class="level1" >Información del Representante Legal.</td></tr>
      <tr><td class="level2">
         Confirme que los datos sean correctos.
      </td></tr>
      </table>
     </div>
    <div class="colapser colapsa"></div>
      <div class="accion">
      <table class="controles" cellspacing="0" cellpadding="1">
         <tr>
         <td class="bt-bottom  bt-right bt-left" style=" width:185px" >15. Representante Legal:</td>
         <td class="bt-bottom">
           <asp:TextBox ID="txtreplegal" runat="server" Width="400px" MaxLength="500"
             style="text-align: center" onblur="checkcajalarge(this,'valreplegal',true);"
                 onkeypress="return soloLetras(event,'abcdefghijklnmñopqrstuvwxyz01234567890 ',true)" 
                 ></asp:TextBox>
         </td>
         <td class="bt-bottom bt-right validacion "><span id="valreplegal" class="validacion"> * obligatorio</span></td>
         </tr>
         <tr>
         <td class="bt-bottom  bt-right bt-left" style=" width:185px" >16. Teléfono Domicilio:</td>
         <td class="bt-bottom">
            <asp:TextBox ID="txttelreplegal" runat="server" Width="200px" MaxLength="9"
             style="text-align: center"
             onBlur="telconvencionalcelular(this,'valtelreplegal',true);"
             onkeypress="return soloLetras(event,'01234567890',true)"></asp:TextBox>
         </td>
         <td class="bt-bottom bt-right validacion "><span id="valtelreplegal" class="validacion"> * obligatorio</span></td>
         </tr>
         <tr><td class="bt-bottom  bt-right bt-left" style=" width:185px">17. Dirección Domiciliaria:</td>
         <td class="bt-bottom">
            <asp:TextBox ID="txtdirdomreplegal" runat="server" Width="400px" MaxLength="500"
             style="text-align: center" onblur="checkcajalarge(this,'valdirdomreplegal',true);"
             onkeypress="return soloLetras(event,'abcdefghijklnmñopqrstuvwxyz01234567890 ./_-',true)"></asp:TextBox>
         </td>
         <td class="bt-bottom bt-right validacion "><span id="valdirdomreplegal" class="validacion"> * obligatorio</span></td>
         </tr>
         <tr>    
         <td class="bt-bottom  bt-right bt-left" style=" width:185px" >
         18. C.I [ <input id="rbcireplegal" checked="checked" type="radio" name="deckrp" value="ci"/>] Pasaporte [<input id="rbpasreplegal"  type="radio" name="deckrp" value="pas"/>]
         </td>
         <td class="bt-bottom">
            <asp:TextBox ID="txtci" runat="server" Width="200px" MaxLength="25"
             style="text-align: center"
             onBlur="valccipasrep(this,'valcipasreplegal','rbcireplegal','rbpasreplegal');"
             onkeypress="return soloLetras(event,'01234567890abcdefghijklnmñopqrstuvwxyz_-',true)"></asp:TextBox>
         </td>
         <td align="right" class="bt-bottom bt-right validacion "><span class="validacion" id="valcipasreplegal" > * obligatorio</span></td>
         </tr>
      </table>
      <table id="tbmail2" class="controles" cellspacing="0" cellpadding="1">
         <tr>
         <td class="bt-bottom  bt-right bt-left" style=" width:185px;" >19. Mail:</td>
         <td class="bt-bottom">
         <%--<div id='TextBoxesGroupRepLegal'>
           <div id="TextBoxDivRepLegal" class="cntmail">--%>
               <%--<span style=" color:Black;">mail:</span>--%>
               <a class="tooltip"><span class="classic">
                Para mas de un Mail separelos con punto y coma.</span>
               <input type='text' id='tmailRepLegal' name='textboxRepLegal' runat="server" style=" width:400px"
                enableviewstate="false" clientidmode="Static"
                onkeypress="return soloLetras(event,'abcdefghijklmnñopqrstuvwxyz1234567890_$@.;',true)"
                onblur="mailone(this,'valmailrl');"
                placeholder="mail@mail.com"
               />
               </a>
           <%--</div>
         </div>--%>
            <%-- <input type='button' value='Agregar' id='addButtonRepLegal' />
             <input type='button' value='Remover' id='removeButtonRepLegal' />--%>
          </td>
          <td class="bt-bottom bt-right validacion "><span id="valmailrl" class="validacion"> * obligatorio</span></td>
         </tr>
      </table>
     </div>
    </div>
   <div id="seccionexp" style=" display:none" runat="server" class="seccion">
    <div class="informativo">
      <table>
      <tr><td rowspan="2" class="inum"><div class="number"><asp:Label runat="server" ID="ls4" Text="4"></asp:Label></div></td><td class="level1" >Información Adicional - Exportadores</td></tr>
      <tr><td class="level2">
         Confirme que los datos sean correctos.
      </td></tr>
      </table>
    </div>
    <div class="colapser colapsa"></div>
      <div class="accion">
      <table class="controles" cellspacing="0" cellpadding="1">
      <tr><th class="bt-bottom bt-right  bt-left" colspan="4">Datos de Personas Naturales o Jurídicas que intervienen en la exportación de su Producto.</th></tr>
      </table>
      <div class="informativo" id="colector2">
      <table id="datexp" class="controles" cellpadding="1" cellspacing="0">
       <asp:Repeater ID="tablePaginationDatosExp" runat="server"  >
            <HeaderTemplate>
            <table id="tabla"  cellspacing="1" cellpadding="1" class="tabRepeat">
            <thead>
            <tr>
            <th></th>
            <th>Nombre</th>
            <th>Ejecutivo de Cuenta</th>
            <th>Mail</th>
            <th>Tèlefono</th>
            </tr>
            </thead> 
            <tbody>
            </HeaderTemplate>
            <ItemTemplate>
            <tr class="point" >
            <td><%#Eval("DESCRIPCION")%></td>
            <td>
                <asp:TextBox 
                style="text-align: center" 
                Text="" ID="txtnombreexp"  xval='<%#Eval("NOMBRE")%>' 
                runat="server" MaxLength="500" onkeypress="return soloLetras(event,'abcdefghijklmnñopqrstuvwxyz1234567890_- ',true)" ></asp:TextBox>
            </td>
            <td>
                <asp:TextBox 
                style="text-align: center" 
                Text="" ID="txtejecta" xval='<%#Eval("EJECUTIVO")%>' 
                runat="server" MaxLength="500" onkeypress="return soloLetras(event,'abcdefghijklmnñopqrstuvwxyz1234567890_- ',true)" ></asp:TextBox>
            </td>
            <td>
                <asp:TextBox
                placeholder="mail@mail.com" 
                id='tmail' name='tmail' runat="server" style= 'width:150px' class="date"
                onkeypress="return soloLetras(event,'abcdefghijklmnopqrstuvwxyz1234567890_@.;',true)"
                enableviewstate="false" clientidmode="Static"
                onblur="validamail(this,'valmailz');" maxlength="250"/>
               <span id="valmailz" class="validacion"></span>
            </td>
            <td>
                <asp:TextBox 
                style="text-align: center; width:100px"
                Text="" ID="txttelexp" xval='<%#Eval("TELEFONO")%>' 
                onBlur="telconvencionalcelularnulo(this,'valteleexp',true);"
                runat="server" MaxLength="10" onkeypress="return soloLetras(event,'01234567890',true)" ></asp:TextBox>
                <span id="valteleexp" class="validacion"></span>
            </td>
            </tr>
            </ItemTemplate>
            <FooterTemplate>
            </tbody>
            </table>
            </FooterTemplate>
         </asp:Repeater>
      </table>
      </div>
      <table class="controles" cellspacing="0" cellpadding="1">
       <tr><th class="bt-bottom bt-top bt-right  bt-left" colspan="4">Datos relativos a la logística de la exportación (deposito / planta o finca):</th></tr>
       </table>
       <table class="controles" cellspacing="0" cellpadding="1">
       <tr>
         <td class="bt-bottom  bt-right bt-left" style=" width:155px">20. Nombre:</td>
         <td class="bt-bottom">
         
         <%--<asp:TextBox runat="server"--%>
         <input 
         id="txtnombreexp" type="text" style="Width:400px"  maxlength="500"  onblur="checkcajalarge(this,'valnomexp',true);"
                onkeypress="return soloLetras(event,'abcdefghijklnmñopqrstuvwxyz01234567890 ',false)"/>
         </td>
         <td class="bt-bottom bt-right validacion "><span id="valnomexp" class="validacion"> * obligatorio</span></td>
       </tr>
       <tr>
         <td class="bt-bottom  bt-right bt-left" style=" width:155px">21. Tipo:</td>
         <td class="bt-bottom">
             
             <%--<asp:TextBox runat="server"--%>
             <input 
             id="txttipoexp" type="text" style="Width:400px"  maxlength="500"  onblur="checkcajalarge(this,'valtipoexp',true);"
                onkeypress="return soloLetras(event,'abcdefghijklnmñopqrstuvwxyz01234567890 ',false)"/>
         </td>
         <td class="bt-bottom bt-right validacion "><span id="valtipoexp" class="validacion"> * obligatorio</span></td>
       </tr>
       <tr>
         <td class="bt-bottom  bt-right bt-left" style=" width:155px">22. Dirección:</td>
         <td class="bt-bottom">
             
             <%--<asp:TextBox runat="server"--%>
             <input 
             id="txtdirexp" type="text" style="Width:400px"  maxlength="500"  onblur="checkcajalarge(this,'valdirexp',true);"
                onkeypress="return soloLetras(event,'abcdefghijklnmñopqrstuvwxyz01234567890 /_-',false)"/>
         </td>
         <td class="bt-bottom bt-right validacion "><span id="valdirexp" class="validacion"> * obligatorio</span></td>
       </tr>
       <tr>
         <td class="bt-bottom  bt-right bt-left" style=" width:155px">23. Teléfono:</td>
         <td class="bt-bottom">
             
             <%--<asp:TextBox runat="server"--%>
             <input 
             id="txttelexp" type="text" style="Width:200px"  maxlength="9"  onblur="telconvencionalcelular(this,'valtelexp',true);"
                onkeypress="return soloLetras(event,'1234567890 ',false)"/>
         </td>
         <td class="bt-bottom bt-right validacion "><span id="valtelexp" class="validacion"> * obligatorio</span></td>
         </tr>
       <tr>
         <td class="bt-bottom  bt-left" style=" width:155px">&nbsp;</td>
         <td class="bt-bottom  bt-left">
         <input id="btnAgregar"  type="button" value="Agregar" onclick="addrowlogexp();"/>
         <input  type="button" value="Remover" onclick="deleterowlogexp();" />
         </td>
         <td class="opcional bt-bottom bt-right" style="height:40px;" >
            agregar/quitar documentos.
         </td>
         </tr>
      </table>
      <div class="informativo" id="colector">
      <table runat="server" id="tableexpo">
      <tr><td>
       <table id="datlogexp" cellpadding="1" cellspacing="0" class="tabRepeat">
            <thead>
            <tr>
            <th>Nombre</th>
            <th>Tipo</th>
            <th>Dirección</th>
            <th>Télefono</th>
            </tr>
            </thead>
            <tbody>
            </tbody>
       </table>
       </td></tr>
       </table>
       </div>
      </div>
    </div>
   <div class="seccion" style=" display:none">
    <div class="informativo">
      <table>
      <tr><td rowspan="2" class="inum"><div class="number"><asp:Label Text="5" runat="server" ID="ls5"/></div></td><td class="level1" >Datos relativos a la seguridad para traslado al puerto.</td></tr>
      <tr><td class="level2">
         Confirme que los datos sean correctos.
      </td></tr>
      </table>
     </div>
    <div class="colapser colapsa"></div>
    <div class="accion">
    <table class="controles" cellspacing="0" cellpadding="1">
         <tr>
         <td class="bt-bottom bt-top bt-right bt-left" style=" width:155px" >24. Su producto es transportado con custodio o GPS?:</td>
         </tr>
    </table>
    <div class="bt-bottom  bt-right bt-left">
    <table cellspacing="0" cellpadding="1" style=" width:100%">
    <tr>
         <td>
           <asp:TextBox ID="txtgps" runat="server" Width="570px" MaxLength="500"
             style="text-align: center" 
             onkeypress="return soloLetras(event,'abcdefghijklnmñopqrstuvwxyz01234567890 ',true)"></asp:TextBox>
         </td>
         <%--onblur="checkcajagps(this,'valgps',true);" <td align="right" class="validacion"><span id="valgps" class="validacion"> * obligatorio</span></td>--%>
    </tr>
    </table>
    </div>
    <%--
    <table class="controles" cellspacing="0" cellpadding="0">
    <tr>
      <td "bt-bottom  bt-right bt-left">
      <div class=" msg-critico" style=" font-weight:bold">
       Autorizo a CONTECON GUAYAQUIL S.A. a entregar mi usuario y clave a la empresa detallada a continuación,
       quién será la encargada de generar los AISV para el ingreso de la carga al Terminal:
      </div>
      </td>
      </tr>
    </table>
    <table class="controles" cellspacing="0" cellpadding="1">
         <tr>
         <td class="bt-bottom bt-top bt-right bt-left" style=" width:155px" >24. Nombre/Razón Social:</td>
         <td class="bt-bottom bt-top">
           <asp:TextBox ID="txtrazsocauter" runat="server" Width="400px" MaxLength="500"
             style="text-align: center" onblur="checkcajalarge(this,'valrazsocauter',true);"
             onkeypress="return soloLetras(event,'abcdefghijklnmñopqrstuvwxyz01234567890 ',true)"></asp:TextBox>
         </td>
         <td class="bt-bottom bt-right bt-top validacion "><span id="valrazsocauter" class="validacion"> * obligatorio</span></td>
         </tr>
         <tr>
         <td class="bt-bottom  bt-right bt-left" style=" width:155px" >25. RUC/C.I:</td>
         <td class="bt-bottom">
            <asp:TextBox ID="txtrucciauter" runat="server" Width="200px" MaxLength="13"
             style="text-align: center"
             onBlur="checkrucci(this,'valrucciauter',true);"
             onkeypress="return soloLetras(event,'01234567890',true)"></asp:TextBox>
         </td>
         <td class="bt-bottom bt-right validacion "><span id="valrucciauter" class="validacion"> * obligatorio</span></td>
         </tr>
         <tr><td class="bt-bottom  bt-right bt-left" style=" width:155px">26. Actividad Comercial:</td>
         <td class="bt-bottom">
            <asp:TextBox ID="txtactcomauter" runat="server" Width="400px" MaxLength="500"
             style="text-align: center" onblur="checkcajalarge(this,'valactcomauter',true);"
             onkeypress="return soloLetras(event,'abcdefghijklnmñopqrstuvwxyz01234567890 ',true)"></asp:TextBox>
         </td>
         <td class="bt-bottom bt-right validacion "><span id="valactcomauter" class="validacion"> * obligatorio</span></td>
         </tr>
         <tr>
         <td class="bt-bottom  bt-right bt-left" style=" width:155px" >27. Teléfono:</td>
         <td class="bt-bottom">
            <asp:TextBox ID="txttelauter" runat="server" Width="200px" MaxLength="9"
             style="text-align: center"
             onBlur="telconvencionalcelular(this,'valtxttelauter',true);"
             onkeypress="return soloLetras(event,'01234567890',true)"></asp:TextBox>
         </td>
         <td class="bt-bottom bt-right validacion "><span id="valtxttelauter" class="validacion"> * obligatorio</span></td>
         </tr>
         <tr><td class="bt-bottom  bt-right bt-left" style=" width:155px">28. Dirección de Oficinas:</td>
         <td class="bt-bottom">
            <asp:TextBox ID="txtdirofiauter" runat="server" Width="400px" MaxLength="500"
             style="text-align: center" onblur="checkcajalarge(this,'valdirofiauter',true);"
             onkeypress="return soloLetras(event,'abcdefghijklnmñopqrstuvwxyz01234567890 /_-',true)"></asp:TextBox>
         </td>
         <td class="bt-bottom bt-right validacion "><span id="valdirofiauter" class="validacion"> * obligatorio</span></td>
         </tr>
         <tr>
         <td class="bt-bottom bt-top bt-right bt-left" style=" width:155px" >29. Contacto:</td>
         <td class="bt-bottom bt-top">
           <asp:TextBox ID="txtcontacntoauter" runat="server" Width="400px" MaxLength="500"
             style="text-align: center" onblur="checkcajalarge(this,'valcontacntoaute',true);"
             onkeypress="return soloLetras(event,'abcdefghijklnmñopqrstuvwxyz01234567890 ',true)"></asp:TextBox>
         </td>
         <td class="bt-bottom bt-right bt-top validacion "><span id="valcontacntoaute" class="validacion"> * obligatorio</span></td>
         </tr>
         <tr>
         <td class="bt-bottom  bt-right bt-left" style=" width:155px" >30. Teléfono Contacto:</td>
         <td class="bt-bottom">
            <asp:TextBox ID="txttelconauter" runat="server" Width="200px" MaxLength="9"
             style="text-align: center"
             onBlur="telconvencionalcelular(this,'valtelconauter',true);"
             onkeypress="return soloLetras(event,'01234567890',true)"></asp:TextBox>
         </td>
         <td class="bt-bottom bt-right validacion "><span id="valtelconauter" class="validacion"> * obligatorio</span></td>
         </tr>
    </table>
    <table class="controles" cellspacing="0" cellpadding="1">
         <tr>
         <td class="bt-bottom  bt-right bt-left" style=" width:155px;" >31. Mail:</td>
         <td class="bt-bottom">
         <div id='TextBoxesGroupAuTer'>
           <div id="TextBoxDivAuTer" class="cntmail">
               <span style=" color:Blue">mail #1:</span><input type='text' id='tmailauter' name='textboxmailauter'  runat="server"
                enableviewstate="false" clientidmode="Static" style=" font-size:small"
               onkeypress="return soloLetras(event,'abcdefghijklmnñopqrstuvwxyz1234567890_$@.',true)"  
                onblur="cadenareqerida(this,1,100,'valmail3');"
                placeholder="mail@mail.com"
               />
           </div>
         </div>
             <input type='button' value='Agregar' id='addButtonMailAuTer' />
             <input type='button' value='Remover' id='removeButtonMailAuTer' />
          </td>
          <td class="bt-bottom bt-right validacion "><span id="valmail3" class="validacion"> * obligatorio</span></td>
         </tr>
      </table>
    <table class="controles" cellspacing="0" cellpadding="0">
    <tr>
      <td "bt-bottom  bt-right bt-left">
      <div class=" msg-critico" style=" font-weight:bold">
       Declaro que los datos consignados y suministrados en el presente documento son correctos y de procedencia lícita.
       Autorizo a CONTECON GUAYAQUIL S.A. a solicitar confirmación de los mismos, en cualquier fuente de información y a
       compartir esta información con las Autoridades que lo soliciten.
      </div>
      </td>
      </tr>
    </table>
    --%>
    </div>
    </div>
   <div class="seccion">
    <div class="informativo">
      <table>
      <tr><td rowspan="2" class="inum"> <div class="number"><asp:Label runat="server" ID="ls6" Text="4"></asp:Label></div></td><td class="level1" >Subir documentos.</td></tr>
      <tr><td class="level2">
         Confirme que los documentos sean los correctos.
      </td></tr>
      </table>
     </div>
    <div class="colapser colapsa"></div>
    <div class="accion">
    <table class="controles" cellspacing="0" cellpadding="1">
    </table>
    <div class="informativo">
      <table class="controles" cellspacing="0" cellpadding="1">
      <tr>
         <td class="bt-bottom bt-top bt-right bt-left">
         <%--<input id="btncondoc" type="button" value="Consultar Documentos" onclick="btncondoc_Click" /></td>--%>
         <asp:Button ID="btnbuscardoc" runat="server" Text="Consultar Documentos" OnClientClick="mostrarsecexp();"
                 onclick="btnbuscardoc_Click"/>
         </td>
      </tr>
      </table>
      <table id="tablerp" class="controles" cellpadding="1" cellspacing="0">
       <asp:Repeater ID="tablePaginationDocumentos" runat="server">
            <HeaderTemplate>
            <table id="tablar"  cellspacing="1" cellpadding="1" class="tabRepeat">
            <thead>
            <tr>
            <%--<th>Tipo de Empresa</th>--%>
            <%--<th class="nover"></th>--%>
            <th class="nover"></th>
            <th>Documentos</th>
            <th>Escoja el archivo con formato indicado.</th>
            <th>Formato</th>
            </tr>
            </thead> 
            <tbody>
            </HeaderTemplate>
            <ItemTemplate>
            <tr class="point">
            <%--<td style=" width:125px"><%#Eval("DESCRIPCION")%></td>--%>
            <%--<td class="nover"><asp:TextBox ID="txtidsolicitud" runat="server" Text='<%#Eval("IDTIPSOL")%>' Width="5px"/></td>--%>
            <td class="nover"><asp:TextBox ID="txtiddocemp" runat="server" Text='<%#Eval("IDDOCEMP")%>' Width="5px"/></td>
            <td style=" width:300px; font-size:inherit"><%#Eval("DOCUMENTO")%></td>
            <td style=" width:300px">
                <%--<input extension='<%#Eval("EXTENSION")%>' class="uploader" id="fsupload" title="Escoja el archivo con formato indicado en la siguiente columna."
                       onchange="validaextension(this)" type="file"  runat="server" clientidmode="Static" />--%>
                <asp:FileUpload extension='<%#Eval("EXTENSION")%>' class="uploader" id="fsupload" title="Escoja el archivo con formato indicado en la siguiente columna."
                       onchange="validaextension(this)" style=" font-size:small" runat="server"/>
                <%--<input class="uploader" id="File1" title="Escoja el archivo CSV (Excel separado por comas)" accept="csv" type="file"  runat="server" clientidmode="Static" />--%>
            </td>
            <td style=" width:40px"><%#Eval("EXTENSION")%></td>
            </tr>
            </ItemTemplate>
            <FooterTemplate>
            </tbody>
            </table>
            </FooterTemplate>
         </asp:Repeater>
      </table>
      </div>
    </div>
    <div>
    <table class="controles" cellspacing="0" cellpadding="0">
    <tr>
        <td "bt-bottom  bt-right bt-left">
        <div class=" msg-critico" style=" font-weight:bold">
        Declaro que los datos consignados y suministrados en el presente documento son correctos y de procedencia lícita.
        Autorizo a CONTECON GUAYAQUIL S.A. a solicitar confirmación de los mismos, en cualquier fuente de información y a
        compartir esta información con las Autoridades que lo soliciten.
        </div>
        </td>
        </tr>
    </table>
    </div>
    <%--<div class="g-recaptcha" data-sitekey="6Le7xBgTAAAAAEvVoppLLsqPNgmr7gDEDhGpUuDp" data-sitekey="my_key"></div>--%>
    <div>
    <table class="controles" cellspacing="0" cellpadding="1">
    <tr>
    <td><asp:Image ID="imgCaptcha" runat="server" BorderColor="Silver" Width="300px" Height="73px" BorderStyle="Solid"/></td>
    <%--<td style=" display:none">
      <cc1:CaptchaControl ID="captchaEmpresa" runat="server" 
            CaptchaBackgroundNoise="Low" CaptchaLength="5" CaptchaHeight="60" CaptchaFont="Comic Sans MS"
            CaptchaWidth="260" BorderColor="Silver" BorderStyle="Solid"
            CaptchaLineNoise="None" CaptchaMinTimeout="6" 
            CaptchaMaxTimeout="240" FontColor="OrangeRed" CustomValidatorErrorMessage="*Comuníquese con el Departamento de Sistemas." />         
    </td>--%>
    </tr>
    <tr>
    <td>Escribe los caracteres que veas:</td>
    </tr>
    <tr>
    <td><asp:TextBox ID="txtTextCaptcha" runat="server" Width="200px" MaxLength="6" 
             style="text-align: center" onblur="checkcaja(this,'valcaptcha',true)"
             onkeypress="return soloLetras(event,'abcdefghijklnmñopqrstuvwxyz01234567890 ',true)"></asp:TextBox></td>
    <td class=""><span id="valcaptcha" class="validacion"> * obligatorio</span></td>
    </tr>

    </table>
    </div>
    <div class="botonera">
         <img alt="loading.." src="../shared/imgs/loader.gif" id="loader" style=" display:none" />
         <span id="imagen"></span>
         <asp:Button ID="btsalvar" runat="server" Text="Enviar Solicitud"  onclick="btsalvar_Click" 
                   OnClientClick="return prepareObject('¿Esta seguro de enviar la solicitud?')"
                   ToolTip="Confirma la información y genera el envio de la solicitud."/>
     </div>
    <asp:HiddenField runat="server" ID="secexportador" Value="EXP"/>
    <asp:HiddenField runat="server" ID="emailsec2" />
    <asp:HiddenField runat="server" ID="emailsec3" />
    <asp:HiddenField runat="server" ID="tableexportador"/>
    <asp:HiddenField runat="server" ID="imagencaptcha" />
    </div>
            </div>
      </div>
    </form>
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.6.2/jquery.min.js"></script>
    <script src='https://www.google.com/recaptcha/api.js?hl=es'></script>
    <script src="../Scripts/pages.js" type="text/javascript"></script>
    <script src="../Scripts/credenciales.js" type="text/javascript"></script>
    <script src="../Scripts/jquery-1.7.1.min.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.tablesorter.min.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.tablesorter.pager.js" type="text/javascript"></script>
    <script type="text/javascript">
        var ced_count = 0;
        var jAisv = {};
        var valida = "";

        $(window).load(function () {
            $(document).ready(function () {
                //colapsar y expandir
                $("div.colapser").toggle(function () { $(this).removeClass("colapsa").addClass("expande"); $(this).next().hide(); }
                                  , function () { $(this).removeClass("expande").addClass("colapsa"); $(this).next().show(); });
            });
        });
        //controlador de mails
        var counter = 2;
        $("#addButton").click(function () {
            if (counter > 5) {
                alert("Solo se permiten 5 mails");
                return false;
            }
            $('<div/>', { 'id': 'TextBoxDiv' + counter }).html($('<span/>').html('mail #' + counter + ':')).append($('<input type="text" placeholder="mail@mail.com">').attr({ 'id': 'textbox' + counter, 'name': 'textbox' + counter, onkeypress: 'return soloLetras(event,"abcdefghijklmnñopqrstuvwxyz1234567890_@.",true);' })).appendTo('#TextBoxesGroup')
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
        //controlador de mails representante legal
        var counter2 = 2;
        $("#addButtonRepLegal").click(function () {
            if (counter2 > 5) {
                alert("Solo se permiten 5 mails");
                return false;
            }
            $('<div/>', { 'id': 'TextBoxDivRepLegal' + counter2 }).html($('<span/>').html('mail #' + counter2 + ':')).append($('<input type="text" placeholder="mail@mail.com">').attr({ 'id': 'textboxreplegal' + counter2, 'name': 'textboxreplegal' + counter2, onkeypress: 'return soloLetras(event,"abcdefghijklmnñopqrstuvwxyz1234567890_@.",true);' })).appendTo('#TextBoxesGroupRepLegal')
            counter2++;
        });
        $("#removeButtonRepLegal").click(function () {
            if (counter2 == 2) {
                alert("Un mail es obligatorio");
                return false;
            }
            counter2--;
            $("#TextBoxDivRepLegal" + counter2).remove();
        });
        //controlador de mails autorizacion terceros
        var counter3 = 2;
        $("#addButtonMailAuTer").click(function () {
            if (counter3 > 5) {
                alert("Solo se permiten 5 mails");
                return false;
            }
            $('<div/>', { 'id': 'textboxmailauter' + counter3 }).html($('<span/>').html('mail #' + counter3 + ':')).append($('<input type="text" placeholder="mail@mail.com">').attr({ 'id': 'textboxmailauter' + counter3, 'name': 'textboxmailauter' + counter3, onkeypress: 'return soloLetras(event,"abcdefghijklmnñopqrstuvwxyz1234567890_@.",true);' })).appendTo('#TextBoxesGroupAuTer')
            counter3++;
        });
        $("#removeButtonMailAuTer").click(function () {
            if (counter3 == 2) {
                alert("Un mail es obligatorio");
                return false;
            }
            counter3--;
            $("#textboxmailauter" + counter3).remove();
        });

        function validaMSCaptcha() {
            $.ajax({
                type: "POST",
                url: "solicitudempresa.aspx/IsCaptchaAvailable",
                data: '{valorcaptchat: "' + $("#<%=imagencaptcha.ClientID%>")[0].value + '", valorcaja: "' + $("#<%=txtTextCaptcha.ClientID%>")[0].value + '" }',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function OnSuccess(response) {
                    if (response.d != "1") {
                        alert('* Datos de Registro de Empresa: *\n * Los caracteres no coinciden, inténtalo de nuevo *');
                        document.getElementById('<%=txtTextCaptcha.ClientID %>').focus();
                        document.getElementById('<%=txtTextCaptcha.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:200;";
                        document.getElementById("loader").className = 'nover';
                        valida = response.d;
                    }
                    else {
                        valida = response.d;
                    }
                },
                failure: function (response) {
                    alert(response.d);
                }
            });
        }
        //esta futura funcion va a preparar el objeto a transportar.
        var registroempresa = {};
        var lista = [];
        var cblarray = [];
        var carray = [];
        function prepareObject(valor) {
            try {

                if (confirm(valor) == false) {
                    return false;
                }
                //Valida los datos de las secciones
                var chk = document.getElementById('<%=cbltipousuario.ClientID %>');
                var checkboxes = chk.getElementsByTagName("input");
                var cont = 0;
                for (var x = 0; x < checkboxes.length; x++) {
                    if (checkboxes[x].checked) {
                        cblarray[x] = x;
                        cont++;
                    }
                }
                if (cont == 0) {
                    alert('* Datos de Registro de Empresa: *\n * Seleccione al menos un Tipo de Cliente *');
                    document.getElementById('<%=cbltipousuario.ClientID %>').focus();
                    //document.getElementById('<%=cbltipousuario.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:400px;";
                    document.getElementById("loader").className = 'nover';
                    return false;
                }
                var vals = document.getElementById('<%=txtrazonsocial.ClientID %>').value;
                if (vals == '' || vals == null || vals == undefined) {
                    alert('* Datos de Registro de Empresa: *\n * Escriba la Razon Social *');
                    document.getElementById('<%=txtrazonsocial.ClientID %>').focus();
                    document.getElementById('<%=txtrazonsocial.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:400px;";
                    document.getElementById("loader").className = 'nover';
                    return false;
                }
                if (!valruccipasservidor()) {
                    return false;
                };
                var vals = document.getElementById('<%=txtactividadcomercial.ClientID %>').value;
                if (vals == '' || vals == null || vals == undefined) {
                    alert('* Datos de Registro de Empresa: *\n * Escriba la Actividad Comercial *');
                    document.getElementById('<%=txtactividadcomercial.ClientID %>').focus();
                    document.getElementById('<%=txtactividadcomercial.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:400px;";
                    document.getElementById("loader").className = 'nover';
                    return false;
                }
                var vals = document.getElementById('<%=txtdireccion.ClientID %>').value;
                if (vals == '' || vals == null || vals == undefined) {
                    alert('* Datos de Registro de Empresa: *\n * Escriba la Dirección de Oficina *');
                    document.getElementById('<%=txtdireccion.ClientID %>').focus();
                    document.getElementById('<%=txtdireccion.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:400px;";
                    document.getElementById("loader").className = 'nover';
                    return false;
                }
                var vals = document.getElementById('<%=txttelofi.ClientID %>').value;
                if (!/^([0-9])*$/.test(vals)) {
                    alert('* Datos de Registro de Empresa: *\n * Teléfono de Oficina No es un Numero *');
                    document.getElementById('<%=txttelofi.ClientID %>').focus();
                    document.getElementById('<%=txttelofi.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:200;";
                    document.getElementById("loader").className = 'nover';
                    return false;
                }
                if (vals.length < 9) {
                    alert('* Datos de Registro de Empresa: *\n * Teléfono de Oficina Incompleto *');
                    document.getElementById('<%=txttelofi.ClientID %>').focus();
                    document.getElementById('<%=txttelofi.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:200;";
                    document.getElementById("loader").className = 'nover';
                    return false;
                }
                if (vals == '' || vals == null || vals == undefined) {
                    alert('* Datos de Registro de Empresa: *\n * Escriba el Teléfono de Oficina *');
                    document.getElementById('<%=txttelofi.ClientID %>').focus();
                    document.getElementById('<%=txttelofi.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:200;";
                    document.getElementById("loader").className = 'nover';
                    return false;
                }
                var vals = document.getElementById('<%=txtcontacto.ClientID %>').value;
                if (vals == '' || vals == null || vals == undefined) {
                    alert('* Datos de Registro de Empresa: *\n * Escriba la Persona de Contacto *');
                    document.getElementById('<%=txtcontacto.ClientID %>').focus();
                    document.getElementById('<%=txtcontacto.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:200;";
                    document.getElementById("loader").className = 'nover';
                    return false;
                }
                var vals = document.getElementById('<%=txttelcelcon.ClientID %>').value;
                if (!/^([0-9])*$/.test(vals)) {
                    alert('* Datos de Registro de Empresa: *\n * No. de Celular de Contacto No es un Numero *');
                    document.getElementById('<%=txttelcelcon.ClientID %>').focus();
                    document.getElementById('<%=txttelcelcon.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:200;";
                    document.getElementById("loader").className = 'nover';
                    return false;
                }
                if (vals.length < 10) {
                    alert('* Datos de Registro de Empresa: *\n * No. de Celular de Contacto Incompleto *');
                    document.getElementById('<%=txttelcelcon.ClientID %>').focus();
                    document.getElementById('<%=txttelcelcon.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:200;";
                    document.getElementById("loader").className = 'nover';
                    return false;
                }
                if (vals == '' || vals == null || vals == undefined) {
                    alert('* Datos de Registro de Empresa: *\n * Escriba el No. de Celular de Contacto *');
                    document.getElementById('<%=txttelcelcon.ClientID %>').focus();
                    document.getElementById('<%=txttelcelcon.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:200;";
                    document.getElementById("loader").className = 'nover';
                    return false;
                }
                var mail1 = document.getElementById('<%=tmailinfocli.ClientID %>').value;
                if (mail1 == null || mail1 == undefined || mail1 == '') {
                    alert('* Datos de Registro de Empresa: *\n * Escriba al menos un mail *');
                    document.getElementById('<%=tmailinfocli.ClientID %>').focus();
                    document.getElementById('<%=tmailinfocli.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:200;";
                    document.getElementById("loader").className = 'nover';
                    return false;
                }
                if (mail1.trim().length > 0) {
                    if (!validarEmail(mail1)) {
                        //alert('* Datos de Registro de Empresa: *\n * Mail no parece correcto *')
                        document.getElementById('<%=tmailinfocli.ClientID %>').focus();
                        document.getElementById('<%=tmailinfocli.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:200;";
                        document.getElementById("loader").className = 'nover';
                        return false;
                    }
                }
                var mail1 = document.getElementById('<%=tmailebilling.ClientID %>').value;
                if (mail1 == null || mail1 == undefined || mail1 == '') {
                    alert('* Datos de Registro de Empresa: *\n * Escriba al menos un mail *');
                    document.getElementById('<%=tmailebilling.ClientID %>').focus();
                    document.getElementById('<%=tmailebilling.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:200;";
                    document.getElementById("loader").className = 'nover';
                    return false;
                }
                if (mail1.trim().length > 0) {
                    if (!validarEmail(mail1)) {
                        alert('* Datos de Registro de Empresa: *\n * Mail no parece correcto *')
                        document.getElementById('<%=tmailebilling.ClientID %>').focus();
                        document.getElementById('<%=tmailebilling.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:200;";
                        document.getElementById("loader").className = 'nover';
                        return false;
                    }
                }
//                }
//                var vals = document.getElementById('<%=turl.ClientID %>').value;
//                if (vals == '' || vals == null || vals == undefined) {
//                    alert('* Datos de Registro de Empresa: *\n * Escriba el Sitio Web *');
//                    document.getElementById('<%=turl.ClientID %>').focus();
//                    document.getElementById('<%=turl.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:400;";
//                    document.getElementById("loader").className = 'nover';
//                    return false;
                //                }
                var vals = document.getElementById('<%=txtreplegal.ClientID %>').value;
                if (vals == '' || vals == null || vals == undefined) {
                    alert('* Datos de Registro de Empresa: *\n * Escriba el Representante Legal *');
                    document.getElementById('<%=txtreplegal.ClientID %>').focus();
                    document.getElementById('<%=txtreplegal.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:400;";
                    document.getElementById("loader").className = 'nover';
                    return false;
                }
                var vals = document.getElementById('<%=txttelreplegal.ClientID %>').value;
                if (!/^([0-9])*$/.test(vals)) {
                    alert('* Datos de Registro de Empresa: *\n * Teléfono de Domicilio No es un Numero *');
                    document.getElementById('<%=txttelreplegal.ClientID %>').focus();
                    document.getElementById('<%=txttelreplegal.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:200;";
                    document.getElementById("loader").className = 'nover';
                    return false;
                }
                if (vals.length < 9) {
                    alert('* Datos de Registro de Empresa: *\n * Teléfono de Domicilio Incompleto *');
                    document.getElementById('<%=txttelreplegal.ClientID %>').focus();
                    document.getElementById('<%=txttelreplegal.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:200;";
                    document.getElementById("loader").className = 'nover';
                    return false;
                }
                if (vals == '' || vals == null || vals == undefined) {
                    alert('* Datos de Registro de Empresa: *\n * Escriba el Teléfono de Domicilio *');
                    document.getElementById('<%=txttelreplegal.ClientID %>').focus();
                    document.getElementById('<%=txttelreplegal.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:200;";
                    document.getElementById("loader").className = 'nover';
                    return false;
                }
                var vals = document.getElementById('<%=txtdirdomreplegal.ClientID %>').value;
                if (vals == '' || vals == null || vals == undefined) {
                    alert('* Datos de Registro de Empresa: *\n * Escriba la Dirección Domiciliaria *');
                    document.getElementById('<%=txtdirdomreplegal.ClientID %>').focus();
                    document.getElementById('<%=txtdirdomreplegal.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:400;";
                    document.getElementById("loader").className = 'nover';
                    return false;
                }
//                var vals = document.getElementById('<%=txtci.ClientID %>').value;
//                if (valruccipas == '' || valruccipas == null || valruccipas == undefined) {
//                    alert('* Datos de Registro de Empresa: *\n * Escriba el No. de CI *');
//                    document.getElementById('<%=txtci.ClientID %>').focus();
//                    document.getElementById('<%=txtci.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:200;";
//                    document.getElementById("loader").className = 'nover';
//                    return false;
//                }
                var mail1 = document.getElementById('<%=tmailRepLegal.ClientID %>').value;
                if (mail1 == null || mail1 == undefined || mail1 == '') {
                    alert('* Datos de Registro de Empresa: *\n * Escriba al menos un mail *');
                    document.getElementById('<%=tmailRepLegal.ClientID %>').focus();
                    document.getElementById('<%=tmailRepLegal.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:200;";
                    document.getElementById("loader").className = 'nover';
                    return false;
                }
                if (mail1.trim().length > 0) {
                    if (!validarEmail(mail1)) {
                        alert('* Datos de Registro de Empresa: *\n * Mail de Representante Legal no parece correcto *')
                        document.getElementById('<%=tmailRepLegal.ClientID %>').focus();
                        document.getElementById('<%=tmailRepLegal.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:200;";
                        document.getElementById("loader").className = 'nover';
                        return false;
                    }
                }
                if (!validaciservidor()) {
                    return false;
                };
                //Valida datos del Exportador si es que lo requieren
//                var chk = document.getElementById('<%=cbltipousuario.ClientID %>');
//                var checkboxes = chk.getElementsByTagName("input");
//                var cont = 0;
//                for (var x = 0; x < checkboxes.length; x++) {
//                    if (checkboxes[x].checked) {
//                        cont++;
//                        //Si es Tipo EXP = EXPORTADOR se habilita lsa seccion de datos y se validan
//                        var secexpo = document.getElementById('<%=secexportador.ClientID %>').value;
//                        if (checkboxes[x].value == secexpo) {
//                            var tbl = document.getElementById('datlogexp');
//                            if (tbl.rows.length == 1) {
//                                if (document.getElementById('txttelexp').value != '' && document.getElementById('txtdirexp').value != '' && document.getElementById('txttipoexp').value != '' && document.getElementById('txtnombreexp').value != '') {
//                                    alert('* Datos de Registro de Empresa: *\n * Por favor de un click sobre el boton Agregar *');
//                                    document.getElementById('btnAgregar').focus();
//                                    document.getElementById("loader").className = 'nover';
//                                    return false;
//                                }
//                            }
//                            lista = [];
//                            document.getElementById('<%=tableexportador.ClientID %>').value = '';
//                            for (var f = 0; f < tbl.rows.length; f++) {
//                                var celColect = tbl.rows[f].getElementsByTagName('td');
//                                if (celColect != undefined && celColect != null && celColect.length > 0) {
//                                    var tdetalle = {
//                                        nombre: celColect[0].textContent,
//                                        tipo: celColect[1].textContent,
//                                        direccion: celColect[2].textContent,
//                                        telefono: celColect[3].textContent
//                                    };
//                                    this.lista.push(tdetalle);
//                                }
//                                for (var n = 0; n < this.lista.length; n++) {
//                                    //alert(lista[n].nombre);
//                                    vals = document.getElementById('<%=tableexportador.ClientID %>').value;
//                                    if (vals == '' || vals == null || vals == undefined) {
//                                        document.getElementById('<%=tableexportador.ClientID %>').value = lista[n].nombre + ',' + lista[n].tipo + ',' + lista[n].direccion + ',' + lista[n].telefono;
//                                    }
//                                    else {
//                                        document.getElementById('<%=tableexportador.ClientID %>').value = document.getElementById('<%=tableexportador.ClientID %>').value + ',' + lista[n].nombre + ',' + lista[n].tipo + ',' + lista[n].direccion + ',' + lista[n].telefono;
//                                    }
//                                }
//                                lista = [];
//                            }
//                        }
//                    }
//                }
                //Valida Documentos
                lista = [];
                var vals = document.getElementById('tablar');
                if (vals == '' || vals == null || vals == undefined) {
                    alert('* Datos de Registro de Empresa: *\n *No se encontraron Documentos*');
                    document.getElementById('<%=btnbuscardoc.ClientID %>').focus();
                    document.getElementById("loader").className = 'nover';
                    return false;
                }
                if (vals != null) {
                    var tbl = document.getElementById('tablar');
                    if (tbl.rows.length == 1) {
                        alert('* Datos de Registro de Empresa: *\n *Por favor de un click sobre el boton Consultar Documentos*');
                        document.getElementById('<%=btnbuscardoc.ClientID %>').focus();
                        document.getElementById("loader").className = 'nover';
                        return false;
                    }
                    for (var f = 0; f < tbl.rows.length; f++) {
                        var celColect = tbl.rows[f].getElementsByTagName('td');
                        if (celColect != undefined && celColect != null && celColect.length > 0) {
                            var tdetalle = {
                                documento: celColect[2].getElementsByTagName('input')[0].value
                            };
                            this.lista.push(tdetalle);
                        }
                    }
                    var nomdoc = null;
                    for (var n = 0; n < this.lista.length; n++) {
                        if (lista[n].documento == '' || lista[n].documento == null || lista[n].documento == undefined) {
                            alert('* Datos de Registro de Empresa: *\n * Seleccione todos los documentos requeridos *');
                            document.getElementById("loader").className = 'nover';
                            return false;
                        }
                        if (nomdoc == lista[n].documento) {
                            alert('* Datos de Registro de Empresa: *\n * Existen archivos repetidos, revise por favor *');
                            document.getElementById("loader").className = 'nover';
                            return false;
                        }
                        nomdoc = lista[n].documento;
                    }
                }
                //                if (get_action(this) == false) {
                //                    return false;
                //                }

                var vals = document.getElementById('<%=txtTextCaptcha.ClientID %>').value;
                if (vals == '' || vals == null || vals == undefined) {
                    alert('* Datos de Registro de Empresa: *\n * Escribe los caracteres que veas *');
                    document.getElementById('<%=txtTextCaptcha.ClientID %>').focus();
                    document.getElementById('<%=txtTextCaptcha.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:200;";
                    document.getElementById("loader").className = 'nover';
                    return false;
                }
                if (document.getElementById('<%=imagencaptcha.ClientID %>').value != document.getElementById('<%=txtTextCaptcha.ClientID %>').value) {
                    alert('* Datos de Registro de Empresa: *\n * Los caracteres no coinciden, inténtalo de nuevo *');
                    document.getElementById('<%=txtTextCaptcha.ClientID %>').focus();
                    document.getElementById('<%=txtTextCaptcha.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:200;";
                    document.getElementById("loader").className = 'nover';
                    return false;
                }
                getGif();
                document.getElementById("loader").className = '';
                return true;
            } catch (e) {
                alert('Lamentamos comunicarle que ha ocurrido el siguiente problema:\n' + e.Message);
            }
        }
        function popupCallback(data, control) {
            this.document.getElementById(control).value = data;
        }
        function valcbl() {
            var chk = document.getElementById('<%=cbltipousuario.ClientID %>');
            var checkboxes = chk.getElementsByTagName("input");
            var cont = 0;
            for (var x = 0; x < checkboxes.length; x++) {
                if (checkboxes[x].checked) {
                    cont++;
                    if (checkboxes[x].value == 'EXP') {
                        document.getElementById('<%=seccionexp.ClientID %>').style.display = 'none';
//                        document.getElementById('<%=ls5.ClientID %>').innerHTML = '5';
//                        document.getElementById('<%=ls6.ClientID %>').innerHTML = '6';
                    }
                }
            }
            for (var x = 0; x < checkboxes.length; x++) {
                if (checkboxes[x].checked == false) {
                    cont++;
                    if (checkboxes[x].value == 'EXP') {
                        document.getElementById('<%=seccionexp.ClientID %>').style.display = 'none';
////                        document.getElementById('<%=ls5.ClientID %>').innerHTML = '4';
////                        document.getElementById('<%=ls6.ClientID %>').innerHTML = '5';
                    }
                }
            }
            //validador.innerHTML = '<span class="obligado">OBLIGATORIO!</span>';
            return;
        }
        function ocultarsecexp() {
            document.getElementById('<%=tablePaginationDocumentos.ClientID %>').style.display = 'none';
            
//            document.getElementById('<%=ls5.ClientID %>').innerHTML = '4';
//            document.getElementById('<%=ls6.ClientID %>').innerHTML = '5';
        }
        function mostrarsecexp() {
            document.getElementById('<%=tablePaginationDocumentos.ClientID %>').style.display = 'block';
            //            document.getElementById('<%=ls5.ClientID %>').innerHTML = '4';
            //            document.getElementById('<%=ls6.ClientID %>').innerHTML = '5';
        }
        function validaciservidor() {
            var vcireplegal = document.getElementById('rbcireplegal').checked;
            if (vcireplegal == true) {
                var valruccipas = document.getElementById('<%=txtci.ClientID %>').value;
                if (!/^([0-9])*$/.test(valruccipas)) {
                    alert('* Datos de Registro de Empresa: *\n * Cédula de Identidad No es un Numero. *');
                    document.getElementById('<%=txtci.ClientID %>').focus();
                    document.getElementById('<%=txtci.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:200;";
                    document.getElementById("loader").className = 'nover';
                    return false;
                }
                else if (valruccipas == '' || valruccipas == null || valruccipas == undefined) {
                    alert('* Datos de Registro de Empresa: *\n * Escriba la Cédula de Identidad. *');
                    document.getElementById('<%=txtci.ClientID %>').focus();
                    document.getElementById('<%=txtci.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:200;";
                    document.getElementById("loader").className = 'nover';
                    return false;
                }
                if (valruccipas.length = 0) {
                    alert('* Datos de Registro de Empresa: *\n * Escriba la Cédula de Identidad. *');
                    document.getElementById('<%=txtci.ClientID %>').focus();
                    document.getElementById('<%=txtci.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:200;";
                    document.getElementById("loader").className = 'nover';
                    return false;
                }
                if (valruccipas.length < 10) {
                    alert('* Datos de Registro de Empresa: *\n * Cédula de Identidad imcompleto. *');
                    document.getElementById('<%=txtci.ClientID %>').focus();
                    document.getElementById('<%=txtci.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:200;";
                    document.getElementById("loader").className = 'nover';
                    return false;
                }
                var array = valruccipas.split("");
                var num = array.length;
                var total = 0;
                var digito = (array[9] * 1);
                for (i = 0; i < (num - 1); i++) {
                    var mult = 0;
                    if ((i % 2) != 0) {
                        total = total + (array[i] * 1);
                    }
                    else {
                        mult = array[i] * 2;
                        if (mult > 9)
                            total = total + (mult - 9);
                        else
                            total = total + mult;
                    }
                }
                var decena = total / 10;
                decena = Math.floor(decena);
                decena = (decena + 1) * 10;
                var final = (decena - total);

                if (digito != 0) {
                    if (final != digito) {
                        alert('* Datos de Registro de Empresa: *\n * Cédula de Identidad no válido. *');
                        document.getElementById('<%=txtci.ClientID %>').focus();
                        document.getElementById('<%=txtci.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:200;";
                        document.getElementById("loader").className = 'nover';
                        return false;
                    }
                }
                else {
                    if (final != 10) {
                        alert('* Datos de Registro de Empresa: *\n * Cédula de Identidad no válido. *');
                        document.getElementById('<%=txtci.ClientID %>').focus();
                        document.getElementById('<%=txtci.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:200;";
                        document.getElementById("loader").className = 'nover';
                        return false;
                    }
                }
            }
            else {
                var pasreplegal = document.getElementById('<%=txtci.ClientID %>').value;
                if (pasreplegal == '' || pasreplegal == null || pasreplegal == undefined) {
                    alert('* Datos de Registro de Empresa: *\n * Escriba el No. Pasaporte del Representante Legal *');
                    document.getElementById('<%=txtci.ClientID %>').focus();
                    document.getElementById('<%=txtci.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:200;";
                    document.getElementById("loader").className = 'nover';
                    return false;
                }
            }
            return true;
        }
        function valruccipasservidor() {
            //codigo = control.value.trim().toUpperCase();
            var valruccipas = document.getElementById('<%=txtruccipas.ClientID %>').value;
            var vruc = document.getElementById('rbruc').checked;
            var vci = document.getElementById('rbci').checked;
            var vpas = document.getElementById('rbpasaporte').checked;
            if (vruc == true) {
                if (!/^([0-9])*$/.test(valruccipas)) {
                    alert('* Datos de Registro de Empresa: *\n * No. RUC No es un Numero. *');
                    document.getElementById('<%=txtruccipas.ClientID %>').focus();
                    document.getElementById('<%=txtruccipas.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:200;";
                    document.getElementById("loader").className = 'nover';
                    return false;
                }
                else if (valruccipas == '' || valruccipas == null || valruccipas == undefined) {
                    alert('* Datos de Registro de Empresa: *\n * Escriba el No. de RUC. *');
                    document.getElementById('<%=txtruccipas.ClientID %>').focus();
                    document.getElementById('<%=txtruccipas.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:200;";
                    document.getElementById("loader").className = 'nover';
                    return false;
                }
                var numeroProvincias = 24;
                var numprov = valruccipas.substr(0, 2);
                if (numprov > numeroProvincias) {
                    alert('* Datos de Registro de Empresa: *\n * El código de la provincia (dos primeros dígitos) es inválido! *');
                    document.getElementById('<%=txtruccipas.ClientID %>').focus();
                    document.getElementById('<%=txtruccipas.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:200;";
                    document.getElementById("loader").className = 'nover';
                    return false;
                }
                validadocrucservidor(valruccipas);
            }
            if (vci == true) {
                if (!/^([0-9])*$/.test(valruccipas)) {
                    alert('* Datos de Registro de Empresa: *\n * No. C.I. No es un Numero. *');
                    document.getElementById('<%=txtruccipas.ClientID %>').focus();
                    document.getElementById('<%=txtruccipas.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:200;";
                    document.getElementById("loader").className = 'nover';
                    return false;
                }
                else if (valruccipas == '' || valruccipas == null || valruccipas == undefined) {
                    alert('* Datos de Registro de Empresa: *\n * Escriba el No. de C.I. *');
                    document.getElementById('<%=txtruccipas.ClientID %>').focus();
                    document.getElementById('<%=txtruccipas.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:200;";
                    document.getElementById("loader").className = 'nover';
                    return false;
                }
                if (valruccipas.length = 0) {
                    alert('* Datos de Registro de Empresa: *\n * Escriba el No. de C.I. *');
                    document.getElementById('<%=txtruccipas.ClientID %>').focus();
                    document.getElementById('<%=txtruccipas.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:200;";
                    document.getElementById("loader").className = 'nover';
                    return false;
                }
                if (valruccipas.length < 10) {
                    alert('* Datos de Registro de Empresa: *\n * No. C.I. INCOMPLETO. *');
                    document.getElementById('<%=txtruccipas.ClientID %>').focus();
                    document.getElementById('<%=txtruccipas.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:200;";
                    document.getElementById("loader").className = 'nover';
                    return false;
                }
                var array = valruccipas.split("");
                var num = array.length;
                var total = 0;
                var digito = (array[9] * 1);
                for (i = 0; i < (num - 1); i++) {
                    var mult = 0;
                    if ((i % 2) != 0) {
                        total = total + (array[i] * 1);
                    }
                    else {
                        mult = array[i] * 2;
                        if (mult > 9)
                            total = total + (mult - 9);
                        else
                            total = total + mult;
                    }
                }
                var decena = total / 10;
                decena = Math.floor(decena);
                decena = (decena + 1) * 10;
                var final = (decena - total);

                if (digito != 0) {
                    if (final != digito) {
                    alert('* Datos de Registro de Empresa: *\n * No. C.I. no válido. *');
                    document.getElementById('<%=txtruccipas.ClientID %>').focus();
                    document.getElementById('<%=txtruccipas.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:200;";
                    document.getElementById("loader").className = 'nover';
                    return false;
                    }
                }
                else {
                    if (final != 10) {
                    alert('* Datos de Registro de Empresa: *\n * No. C.I. no válido. *');
                    document.getElementById('<%=txtruccipas.ClientID %>').focus();
                    document.getElementById('<%=txtruccipas.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:200;";
                    document.getElementById("loader").className = 'nover';
                    return false;
                    }
                }
            }
            if (vpas == true) {
                if (valruccipas == '' || valruccipas == null || valruccipas == undefined) {
                    alert('* Datos de Registro de Empresa: *\n * Escriba el No. Pasaporte. *');
                    document.getElementById('<%=txtruccipas.ClientID %>').focus();
                    document.getElementById('<%=txtruccipas.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:200;";
                    document.getElementById("loader").className = 'nover';
                    return false;
                }
           }
            return true;
        }
        function validadocrucservidor(campo) {

            var numero = campo;
            var suma = 0;
            var residuo = 0;
            var pri = false;
            var pub = false;
            var nat = false;
            var numeroProvincias = 24;
            var modulo = 11;

            if (campo.length < 13) {
                alert('* Datos de Registro de Empresa: *\n * No. RUC. INCOMPLETO. *');
                document.getElementById('<%=txtruccipas.ClientID %>').focus();
                document.getElementById('<%=txtruccipas.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:200;";
                document.getElementById("loader").className = 'nover';
                return false;
            }
            if (campo.length > 13) {
                alert('* Datos de Registro de Empresa: *\n * El valor no corresponde a un No. de RUC. *');
                document.getElementById('<%=txtruccipas.ClientID %>').focus();
                document.getElementById('<%=txtruccipas.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:200;";
                document.getElementById("loader").className = 'nover';
                return false;
            }
            /* Verifico que el campo no contenga letras */

            /* Aqui almacenamos los digitos de la cedula en variables. */
            d1 = numero.substr(0, 1);
            d2 = numero.substr(1, 1);
            d3 = numero.substr(2, 1);
            d4 = numero.substr(3, 1);
            d5 = numero.substr(4, 1);
            d6 = numero.substr(5, 1);
            d7 = numero.substr(6, 1);
            d8 = numero.substr(7, 1);
            d9 = numero.substr(8, 1);
            d10 = numero.substr(9, 1);

            /* El tercer digito es: */
            /* 9 para sociedades privadas y extranjeros */
            /* 6 para sociedades publicas */
            /* menor que 6 (0,1,2,3,4,5) para personas naturales */

            if (d3 == 7 || d3 == 8) {
                //alert('El tercer dígito ingresado es inválido');
                alert('* Datos de Registro de Empresa: *\n * El tercer dígito ingresado es inválido. *');
                document.getElementById('<%=txtruccipas.ClientID %>').focus();
                document.getElementById('<%=txtruccipas.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:200;";
                document.getElementById("loader").className = 'nover';
                return false;
            }

            /* Solo para personas naturales (modulo 10) */
            if (d3 < 6) {
                nat = true;
                p1 = d1 * 2; if (p1 >= 10) p1 -= 9;
                p2 = d2 * 1; if (p2 >= 10) p2 -= 9;
                p3 = d3 * 2; if (p3 >= 10) p3 -= 9;
                p4 = d4 * 1; if (p4 >= 10) p4 -= 9;
                p5 = d5 * 2; if (p5 >= 10) p5 -= 9;
                p6 = d6 * 1; if (p6 >= 10) p6 -= 9;
                p7 = d7 * 2; if (p7 >= 10) p7 -= 9;
                p8 = d8 * 1; if (p8 >= 10) p8 -= 9;
                p9 = d9 * 2; if (p9 >= 10) p9 -= 9;
                modulo = 10;
            }

            /* Solo para sociedades publicas (modulo 11) */
            /* Aqui el digito verficador esta en la posicion 9, en las otras 2 en la pos. 10 */
            else if (d3 == 6) {
                pub = true;
                p1 = d1 * 3;
                p2 = d2 * 2;
                p3 = d3 * 7;
                p4 = d4 * 6;
                p5 = d5 * 5;
                p6 = d6 * 4;
                p7 = d7 * 3;
                p8 = d8 * 2;
                p9 = 0;
            }

            /* Solo para entidades privadas (modulo 11) */
            else if (d3 == 9) {
                pri = true;
                p1 = d1 * 4;
                p2 = d2 * 3;
                p3 = d3 * 2;
                p4 = d4 * 7;
                p5 = d5 * 6;
                p6 = d6 * 5;
                p7 = d7 * 4;
                p8 = d8 * 3;
                p9 = d9 * 2;
            }

            suma = p1 + p2 + p3 + p4 + p5 + p6 + p7 + p8 + p9;
            residuo = suma % modulo;

            /* Si residuo=0, dig.ver.=0, caso contrario 10 - residuo*/
            digitoVerificador = residuo == 0 ? 0 : modulo - residuo;

            /* ahora comparamos el elemento de la posicion 10 con el dig. ver.*/
            if (pub == true) {
                if (digitoVerificador != d9) {
                    alert('* Datos de Registro de Empresa: *\n * El RUC de la empresa del sector público es incorrecto. *');
                    document.getElementById('<%=txtruccipas.ClientID %>').focus();
                    document.getElementById('<%=txtruccipas.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:200;";
                    document.getElementById("loader").className = 'nover';
                    return false;
                }
                /* El ruc de las empresas del sector publico terminan con 0001*/
                if (numero.substr(9, 4) != '0001') {
                    alert('* Datos de Registro de Empresa: *\n * El RUC de la empresa del sector público debe terminar con 0001. *');
                    document.getElementById('<%=txtruccipas.ClientID %>').focus();
                    document.getElementById('<%=txtruccipas.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:200;";
                    document.getElementById("loader").className = 'nover';
                    return false;
                }
            }
            else if (pri == true) {
                if (digitoVerificador != d10) {
                    alert('* Datos de Registro de Empresa: *\n * El RUC de la empresa del sector privado es incorrecto. *');
                    document.getElementById('<%=txtruccipas.ClientID %>').focus();
                    document.getElementById('<%=txtruccipas.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:200;";
                    document.getElementById("loader").className = 'nover';
                    return false;
                }
                if (numero.substr(10, 3) != '001') {
                    alert('* Datos de Registro de Empresa: *\n * El RUC de la empresa del sector privado debe terminar con 001. *');
                    document.getElementById('<%=txtruccipas.ClientID %>').focus();
                    document.getElementById('<%=txtruccipas.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:200;";
                    document.getElementById("loader").className = 'nover';
                    return false;
                }
            }
            else if (nat == true) {
                if (digitoVerificador != d10) {
                    alert('* Datos de Registro de Empresa: *\n * El número de cédula de la persona natural es incorrecto. *');
                    document.getElementById('<%=txtruccipas.ClientID %>').focus();
                    document.getElementById('<%=txtruccipas.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:200;";
                    document.getElementById("loader").className = 'nover';
                    return false;
                }
                if (numero.length > 10 && numero.substr(10, 3) != '001') {
                    alert('* Datos de Registro de Empresa: *\n * El RUC de la persona natural debe terminar con 001. *');
                    document.getElementById('<%=txtruccipas.ClientID %>').focus();
                    document.getElementById('<%=txtruccipas.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:200;";
                    document.getElementById("loader").className = 'nover';
                    return false;
                }
            }
            return true;
        }
        function validarEmail(email) {
            expr = /^([a-zA-Z0-9@;_\.\-])+\@(([a-zA-Z0-9@;\-])+\.)+([a-zA-Z0-9@;]{2,4})+$/; ;
            if (!expr.test(email)) {
                return false;
            }
            return true;
        }
        function get_action(form) {
            var v = grecaptcha.getResponse();
            if (v.length == 0) {
                alert("No se puede dejar vacío Código Captcha.");
                return false;
            }
            if (v.length != 0) {
                //alert("Captcha completed.");
                return true;
            }
        }
        function getGif() {
            document.getElementById('imagen').innerHTML = '<img alt="" src="../shared/imgs/loader.gif"/>';
            return true;
        }
        function getGifOculta() {
            document.getElementById('imagen').innerHTML = '<img alt="" src=""/>';
            return true;
        }
    </script>
    </body>
</html>