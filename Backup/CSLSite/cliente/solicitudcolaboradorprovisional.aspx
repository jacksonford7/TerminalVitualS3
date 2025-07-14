<%@ Page Language="C#" MasterPageFile="~/site.Master" Title="Emisión Pase Provisional" MaintainScrollPositionOnPostback="true"
         AutoEventWireup="true" CodeBehind="solicitudcolaboradorprovisional.aspx.cs" Inherits="CSLSite.cliente.solicitudcolaboradorprovisional" %>
         <%@ MasterType VirtualPath="~/site.Master" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="placehead" runat="server">
    <link href="../shared/estilo/tables.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/aisv-general.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/work.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/catalogos.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/info.css" rel="stylesheet" type="text/css" />
<style type="text/css">
      .warning { background-color:Yellow;  color:Red;}
      #progressBackgroundFilter {
        position:fixed;
        bottom:0px;
        right:0px;
        overflow:hidden;
        z-index:1000;
        top: 0;
        left: 0;
        background-color: #CCC;
        opacity: 0.8;
        filter: alpha(opacity=80);
        text-align:center;
    }
    #processMessage 
    {
        text-align:center;
        position:fixed;
        top:30%;
        left:43%;
        z-index:1001;
        border: 5px solid #67CFF5;
        width: 200px;
        height: 100px;
        background-color: White;
        padding:0;
    }
     #aprint {
 	         color: #666;    
	         border: 1px solid #ccc;    
	         -moz-border-radius: 3px;    
	         -webkit-border-radius: 3px;    
	         background-color: #f6f6f6;    
	         padding: 0.3125em 1em;    
	         cursor: pointer;   
	         white-space: nowrap;   
	         overflow: visible;   
	         font-size: 1em;    
	         outline: 0 none /* removes focus outline in IE */;    
	         margin: 0px;    
	         line-height: 1.6em;    
	         background-image: url(../shared/imgs/action_print.gif);
	         background-repeat: no-repeat;
	         background-position:left center;
	         text-decoration:none;
	         padding:5px 2px 5px 30px;
	  
    }
    * input[type=text]
    {
        text-align:left!important;
        }
    .style1
    {
        width: 250px;
        height: 29px;
    }
    .style2
    {
        border-bottom: 1px solid #CCC;
        height: 29px;
    }
    </style>
</asp:Content>
<asp:Content ID="formcolaborador" ContentPlaceHolderID="placebody" runat="server">
    <input id="zonaid" type="hidden" value="7" />
  <asp:ToolkitScriptManager ID="tkcal" EnableScriptGlobalization="true" runat="server"> </asp:ToolkitScriptManager>

  <noscript><meta http-equiv="refresh" content="0; url=sinsoporte.htm" /> </noscript>
   <div>
         <i class="ico-titulo-1"></i><h2>MCA - Módulo de Control de Acceso</h2><br />
         <i class="ico-titulo-2"></i><h1>Emisión Pase Provisional</h1>
         <br />
   </div>
   <div class="seccion" style=" display:none">
      <div class="informativo">
      <table class="controles" cellspacing="0" cellpadding="1">
      <tr><td rowspan="2" class="inum"> <div class="number">1</div></td><td class="level1" >
          Tipo de Empresa.</td></tr>
      <tr><td class="level2">Tipo de Empresa.</td></tr>
      </table>
     </div>
     <div class="colapser colapsa"></div>

     <div class="accion">
     <div class="bt-bottom  bt-right bt-left">
     <table cellspacing="0" cellpadding="1" >
           <tr>
     <td class="bt-bottom bt-left">
     <asp:DropDownList ID="ddlTipoEmpresa" runat="server" Width="405px"
             onchange="valdltipsol(this, valtipempresa);">
             <asp:ListItem Value="0">* Seleccione origen *</asp:ListItem>
     </asp:DropDownList>
     </td>
     <td class="bt-bottom bt-right validacion "><span id='valtipempresa' class="validacion" > * obligatorio</span></td>
     </tr>
     </table>
     </div>
    </div>
    </div>
   <div class="seccion"  style=" display:none">
      <div class="informativo">
      <table class="controles" cellspacing="0" cellpadding="1">
      <tr><td rowspan="2" class="inum"> <div class="number">2</div></td><td class="level1" >
          Tipo de Credencial.</td></tr>
      <tr><td class="level2">Tipo de Credencial.</td></tr>
      </table>
     </div>
     <div class="colapser colapsa"></div>

     <div class="accion">
         <%--<table class="controles" cellspacing="0" cellpadding="1">
        <tr>
        <td class="bt-bottom  bt-right bt-left" style=" width:150px">1. Tipo de Cliente:</td>
        </tr>
     </table>--%>
     <div class="bt-bottom  bt-right bt-left">
        <table cellspacing="0" cellpadding="1">
    <tr>
        <td "bt-bottom  bt-right bt-left">
        <div class=" msg-critico" runat="server" id="infotipcre" style=" font-weight:bold">
        </div>
        </td>
    </tr>
    </table>
     <table cellspacing="0" cellpadding="1" >
         <%--<tr>
     <td class=""><span style="font-size:smaller; font-family:Consolas; font-weight:normal; font-style:italic; color:Red">* obligatorio / seleccione al menos un valor</span></td>
     </tr>--%>
     <tr>
     <td class="bt-bottom bt-left">
     <asp:DropDownList ID="cbltiposolicitud" runat="server" Width="400px"
             onchange="valdltipsol(this, valtipsol);">
             <asp:ListItem Value="0">* Seleccione origen *</asp:ListItem>
     </asp:DropDownList>
     </td>
     <td class="bt-bottom bt-right validacion "><span id='valtipsol' class="validacion" > * obligatorio</span></td>
         <%--<td class="">
     <div style=" height:80px; overflow-y:scroll" class="bt-right">--%>     <%--<fieldset>--%>     <%--<legend style="font-size:smaller; font-family:Consolas; font-weight:normal; font-style:italic; color:Red"></legend>--%>     
     <%--</fieldset>--%>     <%--</div>
     </td>--%>
     </tr>
     </table>
     </div>
    </div>
    </div>
   <div class="seccion" style=" display:none">
      <div class="informativo">
      <table class="controles" cellspacing="0" cellpadding="1">
      <tr><td rowspan="2" class="inum"> <div class="number">0</div></td><td class="level1" >
          Usuario que solicita el permiso (Representante Legal).</td></tr>
      <tr><td class="level2">Confirme que los datos sean los correctos.</td></tr>
      </table>
     </div>
     <div class="colapser colapsa"></div>
     <div class="accion">
     <table class="controles" cellspacing="0" cellpadding="1">
      <tr>
         <td class="bt-bottom bt-right bt-left" style=" width:155px">Nombres y Apellidos:</td>
         <td class="bt-bottom">
           <asp:TextBox ID="txtusuariosolicita" runat="server" Width="400px" MaxLength="500"
             style="text-align: center;" onblur="checkcajalarge(this,'valusuariosolicita',true);"
             
                 onkeypress="return soloLetras(event,'abcdefghijklnmñopqrstuvwxyz01234567890 ',true)"></asp:TextBox>
         </td>
         <td class="bt-bottom bt-right validacion "><span id="valusuariosolicita" class="validacion"> * obligatorio</span></td>
         </tr>
     <tr>
         <td class="bt-bottom  bt-right bt-left" style=" width:155px">Cédula:</td>
         <td class="bt-bottom">
            <asp:TextBox ID="txtci" runat="server" Width="200px" MaxLength="10"
             style="text-align: center"
             onBlur="validalacedula(this,'valci',true);"
             onkeypress="return soloLetras(event,'01234567890',true)"></asp:TextBox>
         </td>
         <td class="bt-bottom bt-right validacion "><span id="valci" class="validacion"> * obligatorio</span></td>
      </tr>

     </table>
    </div>
    </div>
   <div class="seccion">
      <div class="informativo">
      <table class="controles" cellspacing="0" cellpadding="1">
      <tr><td rowspan="2" class="inum"> <div class="number">1</div></td><td class="level1" >
          Datos Generales del Permiso.</td></tr>
      <tr><td class="level2">Confirme que los datos sean los correctos.</td></tr>
      </table>
     </div>
     <div class="colapser colapsa"></div>
     <div class="accion">
     <table class="controles" cellspacing="0" cellpadding="1">
            <tr><td class="bt-bottom  bt-right bt-left" style=" width:155px">Fecha de Ingreso:</td>
         <td class="bt-bottom">
             <asp:TextBox 
             style="text-align: center" 
             ID="txtfecing" runat="server"  width="200px" MaxLength="15" CssClass="datetimepicker" ClientIDMode="Static"
             onkeypress="return soloLetras(event,'0123456789/')" onblur="checkcaja(this,'valfecing',true);"
             ></asp:TextBox>
         </td>
         <td class="bt-bottom bt-right validacion "><span id="valfecing" class="validacion"> * obligatorio</span></td>
         </tr>
      <tr ><td class="bt-bottom  bt-right bt-left" style=" width:155px">Fecha de Caducidad:</td>
         <td class="bt-bottom">
             <asp:TextBox 
             style="text-align: center" 
             ID="txtfecsal" runat="server"  width="200px" MaxLength="15" CssClass="datetimepicker" ClientIDMode="Static"
             onkeypress="return soloLetras(event,'0123456789/')" onblur="checkcaja(this,'valfecsal',true);"
             ></asp:TextBox>
         </td>
         <td class="bt-bottom bt-right validacion "><span id="valfecsal" class="validacion"> * obligatorio</span></td>
         </tr>
               <tr>
        <td class="bt-bottom bt-right bt-left" style=" width:155px">Area:</td>
        <td class="bt-bottom ">
           <asp:DropDownList runat="server" Width="350px" ID="ddlAreaOnlyControl" onchange="validadropdownlist(this, valareaoc);">
                 <asp:ListItem Value="0">* Elija *</asp:ListItem>
          </asp:DropDownList>
        </td>
        <td class="bt-bottom bt-right validacion "><span id='valareaoc' class="validacion" > * obligatorio</span></td>
        </tr>
      <tr>
        <td class="bt-bottom bt-right bt-left" style=" width:155px">Actividad Permitida:</td>
        <td class="bt-bottom ">
           <asp:DropDownList runat="server" Width="350px" ID="ddlActividadOnlyControl" onchange="validadropdownlist(this, valactividad);">
                 <asp:ListItem Value="0">* Elija *</asp:ListItem>
           </asp:DropDownList>
        </td>
        <td class="bt-bottom bt-right validacion "><span id='valactividad' class="validacion" > * obligatorio</span></td>
        </tr>
     </table>
    </div>
    </div>
   <div class="seccion">
    <div class="informativo">
       <table class="controles" cellspacing="0" cellpadding="1">
      <tr><td rowspan="2" class="inum"> <div class="number">2</div></td><td class="level1" >Datos Generales del Colaborador.</td></tr>
      <tr><td class="level2">Confirme que los datos sean correctos.</td></tr>
      </table>
     </div>
    <div class="colapser colapsa"></div>
     <div class="accion">
      <table class="controles" cellspacing="0" cellpadding="1">
      <tr>
                        <th class=" bt-right bt-left" colspan="4">
                            <asp:Label ID="Label1" Text="Criterios de Consulta para Colaboradores:" style="text-align: left;text-transform:none" runat="server" />
                        </th>
                        </tr>
        <tr>
        <th class="bt-bottom bt-right bt-left" colspan="4">
            <asp:Label ID="Label2" Text="C.I / Pasaporte" style="text-transform:none; font-weight:normal" runat="server" />[<input id="rbcedula" runat="server" type="radio" name="deck" checked="true" onclick="fIrAlCampoCriterioDeBusqueda();" value="ced"/>]
            <asp:Label ID="Label3" Text="Nombre(s)" style="text-transform:none; font-weight:normal" runat="server" />[<input id="rbnombres" runat="server" type="radio" name="deck" onclick="fIrAlCampoCriterioDeBusqueda();" value="nom"/>]
            <asp:Label ID="Label4" Text="Apellido(s)" style="   text-transform:none; font-weight:normal" runat="server" />[<input id="rbapellidos" runat="server" type="radio" name="deck" onclick="fIrAlCampoCriterioDeBusqueda();" value="ape"/>]
            <asp:TextBox runat="server" id="txtcriterioconsulta" Width="200px"/>
            <asp:Button ID="btnBuscar" runat="server" Width="100px" Text="Buscar" 
                OnClientClick="return fvalidaCriterios();" onclick="btnBuscar_Click" />
        </th>
        </tr>
      </table>
      <table class="controles" cellspacing="0" cellpadding="1">
            <tr>
         <td class="bt-bottom  bt-right bt-left" style=" width:250px" >
         C.I [<input id="rbci" checked="true"  type="radio" name="deck" value="ci"/>] Pasaporte[<input id="rbpasaporte"  type="radio" name="deck" value="pas"/>]
         </td>
         <td class="bt-bottom">
             <%--onBlur="valcipas(this,'valcipas','rbci','rbpasaporte');"--%>
            <asp:TextBox ID="txtcipas" runat="server" Width="200px" MaxLength="25" onBlur="valcipas(this,'valcipas','rbci','rbpasaporte');"
             style="text-align: center"
             onkeypress="return soloLetras(event,'01234567890abcdefghijklnmñopqrstuvwxyz',true)"></asp:TextBox>
             <%--             <asp:Button ID="btnConCiPas" runat="server" Width="90px" Text="Consultar" ToolTip="Puede consultar por nombres los datos del colaborador si es que existieran."
                 onclick="btnConCiPas_Click" />--%>
         </td>
         <td align="right" class="bt-bottom bt-right validacion "  style="width:200px"><span class="validacion" id="valcipas" > * obligatorio</span></td>
         </tr>
      <tr>
         <td class="bt-bottom  bt-right bt-left" >Nombres:</td>
         <td class="bt-bottom">
             <%--onblur="checkcajalarge(this,'valnombre',true);"--%>
           <asp:TextBox ID="txtnombres" runat="server" Width="400px" MaxLength="500"  onblur="checkcajalarge(this,'valnombre',true);"
             style="text-align: center;" 
             onkeypress="return soloLetras(event,'abcdefghijklnmñopqrstuvwxyz01234567890 ',true)"></asp:TextBox>
             <%--             <asp:Button ID="btnConNom" runat="server" Width="90px" Text="Consultar" ToolTipPuede consultar por nombres los datos del colaborador si es que existieran."
                 onclick="btnConNom_Click"/>--%>
         </td>
         <td class="bt-bottom bt-right validacion " style="height: 29px"><span id="valnombre" class="validacion"> * obligatorio</span></td>
         </tr>
      <tr>
         <td class="bt-bottom  bt-right bt-left" style=" width:250px" >Apellidos:</td>
         <td class="bt-bottom">
             <%--onblur="checkcajalarge(this,'valapellido',true);"--%>
           <asp:TextBox ID="txtapellidos" runat="server" Width="400px" MaxLength="500" onblur="checkcajalarge(this,'valapellido',true);"
             style="text-align: center;" 
             onkeypress="return soloLetras(event,'abcdefghijklnmñopqrstuvwxyz01234567890 ',true)"></asp:TextBox>
             <%--             <asp:Button ID="btnConApe" runat="server" Width="90px" Text="Consultar" ToolTip="Puede consultar por nombres los datos del colaborador si es que existieran."
                 onclick="btnConApe_Click"/>--%>
         </td>
         <td class="bt-bottom bt-right validacion "><span id="valapellido" class="validacion"> * obligatorio</span></td>
         </tr>
            <%--  </table>
      <table class="controles" cellspacing="0" cellpadding="1">--%>
      <%--</table>
      <table class="controles" cellspacing="0" cellpadding="1">--%>
      <tr>
         <td class="bt-bottom  bt-right bt-left" style=" width:250px" >Tipo de Sangre:</td>
         <td class="bt-bottom">
           <asp:TextBox ID="txttiposangre" runat="server" Width="200px" MaxLength="500"
             style="text-align: center" onblur="checkcaja(this,'valtipsangre',true);"
             onkeypress="return soloLetras(event,'abcdefghijklnmñopqrstuvwxyz+ ',true)"></asp:TextBox>
         </td>
         <td class="bt-bottom bt-right validacion "><span id="valtipsangre" class="validacion"> * obligatorio</span></td>
         </tr>
      <tr><td class="bt-bottom  bt-right bt-left" style=" width:250px">Dirección de Domicilio:</td>
         <td class="bt-bottom">
            <asp:TextBox ID="txtdirdom" runat="server" Width="400px" MaxLength="500"
             style="text-align: center" onblur="checkcajalarge(this,'valdirdom',true);"
             onkeypress="return soloLetras(event,'abcdefghijklnmñopqrstuvwxyz01234567890 /_-.',true)"></asp:TextBox>
         </td>
         <td class="bt-bottom bt-right validacion "><span id="valdirdom" class="validacion"> * obligatorio</span></td>
         </tr>
      <tr>
         <td class="bt-bottom  bt-right bt-left" style=" width:250px" >Teléfono de Domicilio:</td>
         <td class="bt-bottom">
            <asp:TextBox ID="txtteldom" runat="server" Width="200px" MaxLength="9"
             style="text-align: center"
             onBlur="telconvencional(this,'valtelofi',true);"
             onkeypress="return soloLetras(event,'01234567890',true)"></asp:TextBox>
         </td>
         <td class="bt-bottom bt-right validacion "><span id="valtelofi" class="validacion"> * obligatorio</span></td>
         </tr>
      <tr>
         <td class="bt-bottom  bt-right bt-left" style=" width:250px;" >Correo Electronico:</td>
         <td class="bt-bottom">
             <%--<div id='TextBoxesGroup'>--%>           <%--<div id="TextBoxDiv1" class="cntmail">--%>               <%--<span>mail #1:</span>--%>
               <a class="tooltip"><span class="classic">
                Para mas de un Mail separelos con punto y coma.</span>
               <input type='text' id='tmailinfocli' name='textboxmailinfocli'  runat="server" style=" width:400px"
                enableviewstate="false" clientidmode="Static"
                onkeypress="return soloLetras(event,'abcdefghijklmnñopqrstuvwxyz1234567890_$@.;',true)"
                onblur="mailone(this,'valmailrl');"
                placeholder="mail@mail.com"
                />
                </a>
             <%--</div>--%>         <%--</div>--%>             <%--    <input type='button' value='Agregar' id='addButton' />
                 <input type='button' value='Remover' id='removeButton' />--%>
          </td>
          <td class="bt-bottom bt-right validacion ">
              <%--<span id="valmailrl" class="validacion"> * obligatorio</span>--%>
          <span style="font-size:small; font-family:Consolas; font-weight:normal; font-style:italic; color:Green" id="Span1"> * opcional</span></td>
         </tr>
      <tr>
      <td class="bt-bottom  bt-right bt-left" style=" width:250px">Lugar de Nacimiento:</td>
         <td class="bt-bottom">
             <asp:TextBox 
             style="text-align: center" 
             ID="txtlugarnacimiento" runat="server" width="200px" MaxLength="500"
             onkeypress="return soloLetras(event,'abcdefghijklnmñopqrstuvwxyz01234567890 /_-.')" 
             onblur="checkcaja(this,'vallugarnac',true);"></asp:TextBox>
         </td>
         <td class="bt-bottom bt-right validacion "><span id="vallugarnac" class="validacion"> * obligatorio</span></td>
         </tr>
      <tr>
      <td class="bt-bottom  bt-right bt-left" style=" width:250px">Fecha de Nacimiento:</td>
         <td class="bt-bottom">
             <asp:TextBox 
             style="text-align: center" 
             ID="txtfechanacimiento" runat="server"  width="200px" MaxLength="15" CssClass="datetimepicker"
             onkeypress="return soloLetras(event,'0123456789/')" 
             onblur="checkcaja(this,'valfecnac',true);"></asp:TextBox>
         </td>
         <td class="bt-bottom bt-right validacion "><span id="valfecnac" class="validacion" > * obligatorio</span></td>
         </tr>
         <tr>
        <td class="bt-bottom bt-right bt-left" style=" width:155px">Cargo:</td>
        <td class="bt-bottom ">
           <asp:DropDownList runat="server" Width="350px" ID="ddlCargoOnlyControl" onchange="validadropdownlist(this, valcargoac);">
                 <asp:ListItem Value="0">* Elija *</asp:ListItem>
           </asp:DropDownList>
        </td>
        <td class="bt-bottom bt-right validacion "><span id='valcargoac' class="validacion" > * obligatorio</span></td>
        </tr>
      <tr style=" display:none"><td class="bt-bottom  bt-right bt-left" style=" width:250px">Tipo de Licencia:</td>
         <td class="bt-bottom">
         <a class="tooltip"><span class="classic">
                Campo obligatorio solo para Compañia de Transporte.</span>
             <asp:TextBox 
             style="text-align: center" 
             ID="txttiplic" runat="server" ClientIDMode="Static" width="200px" MaxLength="500"
             onkeypress="return soloLetras(event,'abcdefghijklnmñopqrstuvwxyz01234567890 /_-.')" 
             ></asp:TextBox>
             </a>
         </td>
          <%--<td class="bt-bottom bt-right validacion"><span id="valtipolic" class="validacion"> * obligatorio para compañia de transporte</span></td>--%>
         <td class="bt-bottom bt-right validacion"></td>
         </tr>
      <tr style=" display:none"><td class="bt-bottom  bt-right bt-left" style=" width:250px">Fecha Exp. Licencia:</td>
         <td class="bt-bottom">
         <a class="tooltip"><span class="classic">
                Campo obligatorio solo para Compañia de Transporte.</span>
             <asp:TextBox 
             style="text-align: center" 
             ID="txtfecexplic" runat="server"  width="200px" MaxLength="15" CssClass="datetimepicker"
             onkeypress="return soloLetras(event,'0123456789/')" 
             ></asp:TextBox>
             </a>
         </td>
         <td class="bt-bottom bt-right validacion"></td>
          <%--<td class="bt-bottom bt-right validacion "><span id="valfecexplic" class="validacion" > *  obligatorio para compañia de transporte</span></td>--%>
         </tr>
         <tr><td class="bt-bottom  bt-right bt-left" style=" width:155px">Nota:</td>
         <td class="bt-bottom">
                 <asp:TextBox ID="txtNota" runat="server" MaxLength="3000" TextMode="MultiLine" Width="400px" style="overflow:auto;resize:none"
                              onkeypress="return soloLetras(event,'abcdefghijklnmñopqrstuvwxyz01234567890 /_-.',true)"/>
         </td>
         <td class="bt-bottom bt-right"><span style="font-size:small; font-family:Consolas; font-weight:normal; font-style:italic; color:Green" id="Span2"> * opcional</span></td>
         </tr>
      </table>
      <table class="controles" cellspacing="0" cellpadding="1" style=" display:none">
      <tr><td class="bt-bottom  bt-right bt-left" style=" width:155px">Área Destino/Actividad:</td>
         <td class="bt-bottom bt-right">
             <asp:TextBox 
             style="text-align: center" 
             ID="txtaredes" runat="server" ClientIDMode="Static" width="200px" MaxLength="500"
             onkeypress="return soloLetras(event,'abcdefghijklnmñopqrstuvwxyz01234567890 /_-.')" 
             ></asp:TextBox>
         </td>
          <%--<td class="bt-bottom bt-right validacion "><span id="valaredes" class="validacion"> * obligatorio</span></td>--%>         <%--<td class="bt-bottom bt-right" style=" width:200PX"><span style="font-size:small; font-family:Consolas; font-weight:normal; font-style:italic; color:Green" id="Span4"> * opcional</span></td>--%>
         </tr>
      </table>
      <table class="controles" cellspacing="0" cellpadding="1" style=" display:none">
      <tr><td class="bt-bottom  bt-right bt-left" style=" width:155px">Tiempo de Estadia:</td>
         <td class="bt-bottom bt-right">
             <asp:TextBox 
             style="text-align: center" 
             ID="txttiempoestadia" runat="server" ClientIDMode="Static" width="200px" MaxLength="500"
             onkeypress="return soloLetras(event,'abcdefghijklnmñopqrstuvwxyz01234567890 /_-')" 
             ></asp:TextBox>
         </td>
          <%--<td class="bt-bottom bt-right validacion "><span id="valTiempo" class="validacion"> * obligatorio</span></td>--%>         <%--<td class="bt-bottom bt-right" style=" width:200PX"><span style="font-size:small; font-family:Consolas; font-weight:normal; font-style:italic; color:Green;" id="Span1"> * opcional</span></td>--%>
         </tr>
      </table>
      <table class="controles" cellspacing="0" cellpadding="1">
      <tr>
      <td class="bt-left" style=" width:155px; background-color:White"></td>
      <td>
      <asp:Button ID="btnAgregar" runat="server" Width="100px" Text="Agregar" OnClientClick="return validaCabecera();"
                onclick="btnAgregar_Click" />
        <img alt="loading.." src="../shared/imgs/loader.gif" id="loader" class="nover"  />
        </td>
       <td class="bt-right"></td>
       </tr>
      </table>
      <asp:UpdatePanel ID="upresult2" runat="server"  >
      <ContentTemplate>
      <script type="text/javascript">          Sys.Application.add_load(BindFunctions);</script>
      <div class="informativo" id="colector">
      <table runat="server" id="tableexpo">
      <tr><td>
              <asp:GridView runat="server" id="gvColaboradores" class="tabRepeat" AutoGenerateColumns="False" Width="100%"
                       onrowcommand="gvColaboradores_RowCommand"  onrowdeleting="gvColaboradores_RowDeleting">
              <Columns>
                  <asp:TemplateField HeaderText="Nombres">
                      <ItemTemplate>
                          <asp:Label ID="tnombres" runat="server" Width="80px" Text='<%# Eval("[Nombres]") %>'
                          ></asp:Label>
                      </ItemTemplate>
                  </asp:TemplateField>
                  <asp:TemplateField HeaderText="Apellidos">
                      <ItemTemplate>
                          <asp:Label ID="tapellidos" runat="server" Width="80px" Text='<%# Eval("[Apellidos]") %>'
                          ></asp:Label>
                      </ItemTemplate>
                  </asp:TemplateField>
                  <asp:TemplateField HeaderText="C.I./Pasaporte">
                      <ItemTemplate>
                          <asp:Label ID="tcipas" runat="server" Width="50px" Text='<%# Eval("[CIPasaporte]") %>' ></asp:Label>
                      </ItemTemplate>
                  </asp:TemplateField>
                  <asp:TemplateField HeaderText="TipoSangre" Visible="false">
                      <ItemTemplate>
                          <asp:TextBox ID="ttiposangre" runat="server" Width="70px" Text='<%# Eval("[TipoSangre]") %>'></asp:TextBox>
                      </ItemTemplate>
                  </asp:TemplateField>
                  <asp:TemplateField HeaderText="DireccionDomicilio" Visible="false">
                      <ItemTemplate>
                          <asp:TextBox ID="tdirdom" runat="server" Width="70px" Text='<%# Eval("[DireccionDomicilio]") %>'></asp:TextBox>
                      </ItemTemplate>
                  </asp:TemplateField>
                  <asp:TemplateField HeaderText="TelefonoDomicilio" Visible="false">
                      <ItemTemplate>
                          <asp:TextBox ID="ttelfdom" runat="server" Width="70px" Text='<%# Eval("[TelefonoDomicilio]") %>'></asp:TextBox>
                      </ItemTemplate>
                  </asp:TemplateField>
                  <asp:TemplateField HeaderText="Mail" Visible="false">
                      <ItemTemplate>
                          <asp:Label ID="tmail"  style="text-transform :uppercase" runat="server" Width="70px" Text='<%# Eval("[Mail]") %>'></asp:Label>
                      </ItemTemplate>
                  </asp:TemplateField>
                  <asp:TemplateField HeaderText="LugarNacimiento" Visible="false">
                      <ItemTemplate>
                          <asp:Label ID="tlugnac"  style="text-transform :uppercase" runat="server" Width="70px" Text='<%# Eval("[LugarNacimiento]") %>'></asp:Label>
                      </ItemTemplate>
                  </asp:TemplateField>
                  <asp:TemplateField HeaderText="FechaNacimiento" Visible="false">
                      <ItemTemplate>
                          <asp:Label ID="tfecnac"  style="text-transform :uppercase" runat="server" Width="70px" Text='<%# Eval("[FechaNacimiento]") %>'></asp:Label>
                      </ItemTemplate>
                  </asp:TemplateField>
                  <%--<asp:TemplateField HeaderText="TipoLicencia" Visible="false">
                      <ItemTemplate>
                         <asp:Label ID="tarea"  style="text-transform :uppercase" runat="server" Width="70px" Text='<%# Eval("[Area]") %>'></asp:Label>
                      </ItemTemplate>
                  </asp:TemplateField>--%>
                  <asp:TemplateField HeaderText="Cargo">
                      <ItemTemplate>
                          <asp:Label ID="tcargo"  style="text-transform :uppercase" runat="server" Width="80px" Text='<%# Eval("[Cargo]") %>'></asp:Label>
                      </ItemTemplate>
                  </asp:TemplateField>
                  <asp:TemplateField HeaderText="Nota">
                      <ItemTemplate>
                      <div style="overflow-y:scroll; overflow-x: hidden; height:30px; text-align:center">
                          <asp:Label ID="tnota"  style="text-transform :uppercase" runat="server" ToolTip='<%# Eval("[Nota]") %>' Text='<%# Eval("[Nota]") %>'></asp:Label>
                      </div>
                      </ItemTemplate>
                  </asp:TemplateField>
                  <%--<asp:TemplateField HeaderText="FechaExpLicencia" Visible="false">
                      <ItemTemplate>
                          <asp:Label ID="ttiempoini"  style="text-transform :uppercase" runat="server" Width="70px" Text='<%# Eval("[FechaInicio]") %>'></asp:Label>
                      </ItemTemplate>
                  </asp:TemplateField>
                  <asp:TemplateField HeaderText="FechaExpLicencia" Visible="false">
                      <ItemTemplate>
                          <asp:Label ID="ttiempocad"  style="text-transform :uppercase" runat="server" Width="70px" Text='<%# Eval("[FechaCaducidad]") %>'></asp:Label>
                      </ItemTemplate>
                  </asp:TemplateField>--%>
                   <asp:TemplateField HeaderText="NO Cargados" Visible="false">
                      <ItemTemplate>
                          <%--<asp:TextBox ID="tplaca" runat="server" Width="70px" Text='<%# Eval("Placa") %>' onkeypress="return soloLetras(event,'abcdefghijklnmñopqrstuvwxyz01234567890',true)"></asp:TextBox>--%>
                          <asp:CheckBox ID="chkEstadoDocumentos" runat="server" Width="50px" Enabled="false"/>
                      </ItemTemplate>
                  </asp:TemplateField>
                  <asp:TemplateField ShowHeader="False"  HeaderText="Adjuntar">
                        <ItemTemplate>
                            <a style=" width:80px" id="adjDoc" class="topopup" onclick="redirectcatdoc('<%# Eval("CIPasaporte") %>');" >
                            <i class="ico-find" ></i> Documentos </a>
                             <%--<asp:Button ID="btnAdjDoc" runat="server" CausesValidation="False" Width="125px"
                                OnClientClick='redirectcatdoc(tplaca);'
                                CommandName="Add" Text="Adjuntar Documentos" />--%>
                        </ItemTemplate>
                  </asp:TemplateField>
                  <asp:TemplateField ShowHeader="False">
                        <ItemTemplate>
                            <asp:Button ID="btnDelete" runat="server" CausesValidation="False" Width="55px"
                                CommandName="Delete" Text="Eliminar" />
                        </ItemTemplate>
                  </asp:TemplateField>
              </Columns>
          </asp:GridView>
       </td></tr>
      </table>
      </div>
             </ContentTemplate>
      <Triggers>
        <asp:AsyncPostBackTrigger ControlID="btsalvar" /> 
      </Triggers>
     </asp:UpdatePanel>
     </div>
     <table class="controles" cellspacing="0" cellpadding="0">
     <tr>
        <td "bt-bottom  bt-right bt-left">
        <div class=" msg-critico" style=" font-weight:bold">
         <span>Nota:</span>
         <br />
         <span>Certifico que la información aquí suministrada es verdadera y podrá ser verificada en cualquier momento por CONTECON Así mismo estoy dispuesto a brindar una ampliación de cualquier aspecto de los datos registrados.</span>
         <br />
         <span>Y me comprometo a no ingresar al TERMINAL MARITIMO en estado de embriaguez o bajo la acción de cualquier sustancia psicotrópicas así como cumplir con todas las normas, procedimientos  y disposiciones de CGSA.</span>
        </div>
        </td>
    </tr>
     </table>
     <div class="botonera">
     <asp:Button ID="btsalvar" runat="server" Text="Enviar Solicitud"  onclick="btsalvar_Click" 
                   OnClientClick="return prepareObject('¿Esta seguro de enviar la solicitud?');"
                   ToolTip="Confirma la información y genera el envio de la solicitud."/>
    </div>
    </div>   
    <asp:HiddenField runat="server" ID="emailsec2" />
    <script src="../Scripts/pages.js" type="text/javascript"></script>
    <script src="../Scripts/credenciales.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.tablesorter.min.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.tablesorter.pager.js" type="text/javascript"></script>
    <script type="text/javascript">
        var ced_count = 0;
        var jAisv = {};
        $(document).ready(function () {
            $('.datetimepicker').datetimepicker().datetimepicker({ lang: 'es', timepicker: false, closeOnDateSelect: true, format: 'd/m/Y' });
        });
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
                var vals = document.getElementById('<%=txtusuariosolicita.ClientID %>').value;
                if (vals == '' || vals == null || vals == undefined) {
                    alert('* Escriba los Nombres del Representante Legal*');
                    document.getElementById('<%=txtusuariosolicita.ClientID %>').focus();
                    document.getElementById('<%=txtusuariosolicita.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:400px;";
                    document.getElementById("loader").className = 'nover';
                    return false;
                }
                var vals = document.getElementById('<%=txtci.ClientID %>').value;
                if (vals == '' || vals == null || vals == undefined) {
                    alert('* Escriba la Cedula del Representante Legal*');
                    document.getElementById('<%=txtci.ClientID %>').focus();
                    document.getElementById('<%=txtci.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:200px;";
                    document.getElementById("loader").className = 'nover';
                    return false;
                }
                if (!valcisolicita()) {
                    return false;
                };
                var vals = document.getElementById('<%=txtfecing.ClientID %>').value;
                if (vals == '' || vals == null || vals == undefined) {
                    alert('*Seleccione la Fecha de Ingreso *');
                    document.getElementById('<%=txtfecing.ClientID %>').focus();
                    document.getElementById('<%=txtfecing.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:200px;";
                    document.getElementById("loader").className = 'nover';
                    return false;
                }
                var vals = document.getElementById('<%=txtfecsal.ClientID %>').value;
                if (vals == '' || vals == null || vals == undefined) {
                    alert('*Seleccione la Fecha de Caducidad *');
                    document.getElementById('<%=txtfecsal.ClientID %>').focus();
                    document.getElementById('<%=txtfecsal.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:200px;";
                    document.getElementById("loader").className = 'nover';
                    return false;
                }
                var vals = document.getElementById('<%=ddlAreaOnlyControl.ClientID %>').options[document.getElementById("<%=ddlAreaOnlyControl.ClientID%>").selectedIndex].text;
                if (vals == '* Elija *') {
                    alert('* Seleccione el Area *');
                    document.getElementById('<%=ddlAreaOnlyControl.ClientID %>').focus();
                    document.getElementById('<%=ddlAreaOnlyControl.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:350px;";
                    document.getElementById("loader").className = 'nover';
                    return false;
                }
                var vals = document.getElementById('<%=ddlActividadOnlyControl.ClientID %>').options[document.getElementById("<%=ddlActividadOnlyControl.ClientID%>").selectedIndex].text;
                if (vals == '* Elija *') {
                    alert('* Seleccione la Actividad Permitida *');
                    document.getElementById('<%=ddlActividadOnlyControl.ClientID %>').focus();
                    document.getElementById('<%=ddlActividadOnlyControl.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:350px;";
                    document.getElementById("loader").className = 'nover';
                    return false;
                }
                    return true;
            } catch (e) {
                alert('Lamentamos comunicarle que ha ocurrido el siguiente problema:\n' + e.Message);
            }
        }
        function validaCabecera() {
            try {
                //Valida los datos de las secciones
//                var vals = document.getElementById('<%=ddlTipoEmpresa.ClientID %>').value;
//                if (vals == 0) {
//                    alert('* Datos de Solicitud de Credencial: *\n * Seleccione el Tipo de Empresa *');
//                    document.getElementById('<%=ddlTipoEmpresa.ClientID %>').focus();
//                    document.getElementById('<%=ddlTipoEmpresa.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:400px;";
//                    document.getElementById("loader").className = 'nover';
//                    return false;
//                }
//                var vals = document.getElementById('<%=cbltiposolicitud.ClientID %>').value;
//                if (vals == 0) {
//                    alert('* Datos de Solicitud de Credencial: *\n * Seleccione el Tipo de Solicitud *');
//                    document.getElementById('<%=cbltiposolicitud.ClientID %>').focus();
//                    document.getElementById('<%=cbltiposolicitud.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:400px;";
//                    document.getElementById("loader").className = 'nover';
//                    return false;
                //                }
//                var vals = document.getElementById('<%=txtci.ClientID %>').value;
//                if (vals == '' || vals == null || vals == undefined) {
//                    alert('* Escriba la Cedula del Representante Legal*');
//                    document.getElementById('<%=txtci.ClientID %>').focus();
//                    document.getElementById('<%=txtci.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:200px;";
//                    document.getElementById("loader").className = 'nover';
//                    return false;
//                }
//                var vals = document.getElementById('<%=txtusuariosolicita.ClientID %>').value;
//                if (vals == '' || vals == null || vals == undefined) {
//                    alert('* Escriba los Nombres del Representante Legal*');
//                    document.getElementById('<%=txtusuariosolicita.ClientID %>').focus();
//                    document.getElementById('<%=txtusuariosolicita.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:400px;";
//                    document.getElementById("loader").className = 'nover';
//                    return false;
//                }
//                var vals = document.getElementById('<%=txtfecing.ClientID %>').value;
//                if (vals == '' || vals == null || vals == undefined) {
//                    alert('*Seleccione la Fecha de Ingreso *');
//                    document.getElementById('<%=txtfecing.ClientID %>').focus();
//                    document.getElementById('<%=txtfecing.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:200px;";
//                    document.getElementById("loader").className = 'nover';
//                    return false;
//                }
//                var vals = document.getElementById('<%=txtfecsal.ClientID %>').value;
//                if (vals == '' || vals == null || vals == undefined) {
//                    alert('*Seleccione la Fecha de Caducidad *');
//                    document.getElementById('<%=txtfecsal.ClientID %>').focus();
//                    document.getElementById('<%=txtfecsal.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:200px;";
//                    document.getElementById("loader").className = 'nover';
//                    return false;
//                }
//                var vals = document.getElementById('<%=ddlAreaOnlyControl.ClientID %>').value;
//                if (vals == 0) {
//                    alert('* Seleccione el Area *');
//                    document.getElementById('<%=ddlAreaOnlyControl.ClientID %>').focus();
//                    document.getElementById('<%=ddlAreaOnlyControl.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:350px;";
//                    document.getElementById("loader").className = 'nover';
//                    return false;
//                }
                var vals = document.getElementById('<%=ddlCargoOnlyControl.ClientID %>').options[document.getElementById("<%=ddlCargoOnlyControl.ClientID%>").selectedIndex].text;
                if (vals == '* Elija *') {
                    alert('* Seleccione el Cargo *');
                    document.getElementById('<%=ddlCargoOnlyControl.ClientID %>').focus();
                    document.getElementById('<%=ddlCargoOnlyControl.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:350px;";
                    document.getElementById("loader").className = 'nover';
                    return false;
                }
                var vals = document.getElementById('<%=txtnombres.ClientID %>').value;
                if (vals == '' || vals == null || vals == undefined) {
                    alert('* Escriba los Nombres *');
                    document.getElementById('<%=txtnombres.ClientID %>').focus();
                    document.getElementById('<%=txtnombres.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:400px;";
                    document.getElementById("loader").className = 'nover';
                    return false;
                }
                var vals = document.getElementById('<%=txtapellidos.ClientID %>').value;
                if (vals == '' || vals == null || vals == undefined) {
                    alert('* Escriba los Apellidos *');
                    document.getElementById('<%=txtapellidos.ClientID %>').focus();
                    document.getElementById('<%=txtapellidos.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:400px;";
                    document.getElementById("loader").className = 'nover';
                    return false;
                }
                if (!valcipasservidor()) {
                    return false;
                };
                var vals = document.getElementById('<%=txttiposangre.ClientID %>').value;
                if (vals == '' || vals == null || vals == undefined) {
                    alert('* Escriba el Tipo de Sangre *');
                    document.getElementById('<%=txttiposangre.ClientID %>').focus();
                    document.getElementById('<%=txttiposangre.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:200px;";
                    document.getElementById("loader").className = 'nover';
                    return false;
                }
                var vals = document.getElementById('<%=txtdirdom.ClientID %>').value;
                if (vals == '' || vals == null || vals == undefined) {
                    alert('*Escriba la Dirección de Domicilio *');
                    document.getElementById('<%=txtdirdom.ClientID %>').focus();
                    document.getElementById('<%=txtdirdom.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:400px;";
                    document.getElementById("loader").className = 'nover';
                    return false;
                }
                var vals = document.getElementById('<%=txtteldom.ClientID %>').value;
                if (!/^([0-9])*$/.test(vals)) {
                    alert('* Teléfono de Domicilio No es un Numero *');
                    document.getElementById('<%=txtteldom.ClientID %>').focus();
                    document.getElementById('<%=txtteldom.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:200;";
                    document.getElementById("loader").className = 'nover';
                    return false;
                }
                if (vals.length < 9) {
                    alert('* Teléfono de Domicilio Incompleto *');
                    document.getElementById('<%=txtteldom.ClientID %>').focus();
                    document.getElementById('<%=txtteldom.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:200;";
                    document.getElementById("loader").className = 'nover';
                    return false;
                }
                if (vals == '' || vals == null || vals == undefined) {
                    alert('* Escriba el Teléfono de Domicilio *');
                    document.getElementById('<%=txtteldom.ClientID %>').focus();
                    document.getElementById('<%=txtteldom.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:200;";
                    document.getElementById("loader").className = 'nover';
                    return false;
                }
                //Valida Mail
//                var mail1 = document.getElementById('tmailinfocli').value;
//                if (mail1 == null || mail1 == undefined || mail1 == '') {
//                    alert('* Escriba al menos un mail *');
//                    document.getElementById('tmailinfocli').focus();
//                    document.getElementById('tmailinfocli').style.cssText = "background-color:#ffffc6;color:Red;width:400;";
//                    document.getElementById("loader").className = 'nover';
//                    return false;
//                }
                var vals = document.getElementById('<%=txtlugarnacimiento.ClientID %>').value;
                if (vals == '' || vals == null || vals == undefined) {
                    alert('* Escriba el Lugar de Nacimiento *');
                    document.getElementById('<%=txtlugarnacimiento.ClientID %>').focus();
                    document.getElementById('<%=txtlugarnacimiento.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:200px;";
                    document.getElementById("loader").className = 'nover';
                    return false;
                }
                if (!valedad()) {
                    return false;
                };
//                var vals = document.getElementById('<%=txtaredes.ClientID %>').value;
//                if (vals == '' || vals == null || vals == undefined) {
//                    alert('* Datos de Solicitud de Credencial: *\n * Escriba el Area de Destino *');
//                    document.getElementById('<%=txtaredes.ClientID %>').focus();
//                    document.getElementById('<%=txtaredes.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:200px;";
//                    document.getElementById("loader").className = 'nover';
//                    return false;
//                }

//                var vals = document.getElementById('<%=txttiplic.ClientID %>').value;
//                if (vals == '' || vals == null || vals == undefined) {
//                    alert('* Datos de Solicitud de Credencial: *\n * Escriba el Tipo de Licencia *');
//                    document.getElementById('<%=txttiplic.ClientID %>').focus();
//                    document.getElementById('<%=txttiplic.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:200px;";
//                    document.getElementById("loader").className = 'nover';
//                    return false;
//                }
//                var vals = document.getElementById('<%=txtfecexplic.ClientID %>').value;
//                if (vals == '' || vals == null || vals == undefined) {
//                    alert('* Datos de Solicitud de Credencial: *\n * Escriba la Fecha de Expiración de la Licencia *');
//                    document.getElementById('<%=txtfecexplic.ClientID %>').focus();
//                    document.getElementById('<%=txtfecexplic.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:200px;";
//                    document.getElementById("loader").className = 'nover';
//                    return false;
//                }
//                var vals = document.getElementById('<%=txttiempoestadia.ClientID %>').value;
//                if (vals == '' || vals == null || vals == undefined) {
//                    alert('* Datos de Solicitud de Credencial: *\n * Escriba el Tiempo de Estadia *');
//                    document.getElementById('<%=txttiempoestadia.ClientID %>').focus();
//                    document.getElementById('<%=txttiempoestadia.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:200px;";
//                    document.getElementById("loader").className = 'nover';
//                    return false;
//                }
                document.getElementById("loader").className = '';
                return true;
            } catch (e) {
                alert('Lamentamos comunicarle que ha ocurrido el siguiente problema:\n' + e.Message);
            }
        }
        function valedad() {
            var codigo;
            codigo = document.getElementById('<%=txtfechanacimiento.ClientID %>').value;
            if (codigo.length <= 0) {
                alert('* Datos de Solicitud de Credencial: *\n * Escoja la Fecha de Nacimiento *');
                document.getElementById('<%=txtfechanacimiento.ClientID %>').focus();
                document.getElementById('<%=txtfechanacimiento.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:400;";
                document.getElementById("loader").className = 'nover';
                return false;
            }
//            var desdeDate = new Date();
//            var desde = desdeDate.format("d/mm/yy");
//            var hasta = codigo;
//            alert(desde);
//            alert(hasta);
//            return true;
//            if (desde == undefined || desde == null || hasta == undefined || hasta == null) {
//                alert('Por favor agregue o revise las fechas inicio/expiracion');
//                return false;
//            }
//            var datePart = codigo.value.split('/');
//            var date = new Date(datePart[1] + '/' + datePart[0] + '/' + datePart[2]);
//            if (!date instanceof Date || isNaN(date.valueOf())) {
//                alert('Por favor revise/escriba la fecha de inicio');
//                return false;
//            }
//            var datePart2 = hasta.value.split('/');
//            var date2 = new Date(datePart2[1] + '/' + datePart2[0] + '/' + datePart2[2]);
//            if (!date2 instanceof Date || isNaN(date2.valueOf())) {
//                alert('Por favor revise/escriba la fecha de caducidad');
//                return false;
//            }
//            if (date > date2) {
//                alert('El contrato no puede iniciar despues de su fecha fin');
//                return false;
//            }
            return true;
        }
        function valcisolicita() {
            //codigo = control.value.trim().toUpperCase();
            var valruccipas = document.getElementById('<%=txtci.ClientID %>').value;
            if (!/^([0-9])*$/.test(valruccipas)) {
                alert('* No. C.I. No es un Numero *');
                document.getElementById('<%=txtci.ClientID %>').focus();
                document.getElementById('<%=txtci.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:200;";
                document.getElementById("loader").className = 'nover';
                return false;
            }
            else if (valruccipas == '' || valruccipas == null || valruccipas == undefined) {
                alert('* Escriba el No. de C.I. *');
                document.getElementById('<%=txtci.ClientID %>').focus();
                document.getElementById('<%=txtci.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:200;";
                document.getElementById("loader").className = 'nover';
                return false;
            }
            if (valruccipas.length = 0) {
                alert('* Escriba el No. de C.I. *');
                document.getElementById('<%=txtci.ClientID %>').focus();
                document.getElementById('<%=txtci.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:200;";
                document.getElementById("loader").className = 'nover';
                return false;
            }
            if (valruccipas.length < 10) {
                alert('* No. C.I. INCOMPLETO! *');
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
                    alert('* No. C.I. no válido! *');
                    document.getElementById('<%=txtci.ClientID %>').focus();
                    document.getElementById('<%=txtci.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:200;";
                    document.getElementById("loader").className = 'nover';
                    return false;
                }
            }
            else {
                if (final != 10) {
                    alert('* No. C.I. no válido! *');
                    document.getElementById('<%=txtci.ClientID %>').focus();
                    document.getElementById('<%=txtci.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:200;";
                    document.getElementById("loader").className = 'nover';
                    return false;
                }
            }

            return true;
        }
        function valcipasservidor() {
            //codigo = control.value.trim().toUpperCase();
            var valruccipas = document.getElementById('<%=txtcipas.ClientID %>').value;
            var vci = document.getElementById('rbci').checked;
            var vpas = document.getElementById('rbpasaporte').checked;
            if (vci == true) {
                if (!/^([0-9])*$/.test(valruccipas)) {
                    alert('* No. C.I. No es un Numero *');
                    document.getElementById('<%=txtcipas.ClientID %>').focus();
                    document.getElementById('<%=txtcipas.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:200;";
                    document.getElementById("loader").className = 'nover';
                    return false;
                }
                else if (valruccipas == '' || valruccipas == null || valruccipas == undefined) {
                    alert('* Escriba el No. de C.I. *');
                    document.getElementById('<%=txtcipas.ClientID %>').focus();
                    document.getElementById('<%=txtcipas.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:200;";
                    document.getElementById("loader").className = 'nover';
                    return false;
                }
                if (valruccipas.length = 0) {
                    alert('* Escriba el No. de C.I. *');
                    document.getElementById('<%=txtcipas.ClientID %>').focus();
                    document.getElementById('<%=txtcipas.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:200;";
                    document.getElementById("loader").className = 'nover';
                    return false;
                }
                if (valruccipas.length < 10) {
                    alert('* No. C.I. INCOMPLETO! *');
                    document.getElementById('<%=txtcipas.ClientID %>').focus();
                    document.getElementById('<%=txtcipas.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:200;";
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
                        alert('* No. C.I. no válido! *');
                        document.getElementById('<%=txtcipas.ClientID %>').focus();
                        document.getElementById('<%=txtcipas.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:200;";
                        document.getElementById("loader").className = 'nover';
                        return false;
                    }
                }
                else {
                    if (final != 10) {
                        alert('* No. C.I. no válido! *');
                        document.getElementById('<%=txtcipas.ClientID %>').focus();
                        document.getElementById('<%=txtcipas.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:200;";
                        document.getElementById("loader").className = 'nover';
                        return false;
                    }
                }
            }
            if (vpas == true) {
                if (valruccipas == '' || valruccipas == null || valruccipas == undefined) {
                    alert('* Escriba el No. de Pasaporte *');
                    document.getElementById('<%=txtcipas.ClientID %>').focus();
                    document.getElementById('<%=txtcipas.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:200;";
                    document.getElementById("loader").className = 'nover';
                    return false;
                }
            }
            return true;
        }
        function popupCallback(data, control) {
            this.document.getElementById(control).value = data;
        }
        function redireccionar() {
            window.locationf = "~/cuenta/zones.aspx";
        }
        function redirectcatdoc(val) {
            var cajaced = val;
            var cajatip = document.getElementById('<%=ddlActividadOnlyControl.ClientID %>').value;
            //window.open('../credenciales/documentos/?placa=' + caja, 'name', 'width=850,height=480')
            window.open('../cliente/solicitudcolaboradordocumentosprovisional.aspx?CIPAS=' + cajaced + '&tipodoc=' + cajatip, 'name', 'width=950,height=480')
        }
        function fvalidaCriterios() {
                var cedula = document.getElementById('<%=rbcedula.ClientID %>').checked;
                var nombres = document.getElementById('<%=rbnombres.ClientID %>').checked;
                var apellidos = document.getElementById('<%=rbapellidos.ClientID %>').checked;
                var vals = document.getElementById('<%=txtcriterioconsulta.ClientID %>').value;
                if (vals == '' || vals == null || vals == undefined) {
                    document.getElementById('<%=txtcriterioconsulta.ClientID %>').focus();
                    document.getElementById('<%=txtcriterioconsulta.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:200px;";
                    if (cedula) {
                        alert('Escriba la Cedula del Colaborador.');
                        return false;
                    }
                    if (nombres) {
                        alert('Escriba el Nombre(s) del Colaborador.');
                        return false;
                    }
                    if (apellidos) {
                        alert('Escriba el Apellido(s) del Colaborador.');
                        return false;
                    }
                }
            return true;
        }
        function popupCallback(objeto, catalogo) {
            if (objeto == null || objeto == undefined) {
                alert('Hubo un problema al setaar un objeto de catalogo');
                return;
            }
            document.getElementById('<%=txtcriterioconsulta.ClientID %>').value = "";
            document.getElementById('<%=txtnombres.ClientID %>').value = objeto.nombres;
            document.getElementById('<%=txtapellidos.ClientID %>').value = objeto.apellidos;
            document.getElementById('<%=txtcipas.ClientID %>').value = objeto.cedula;
            document.getElementById('<%=txttiposangre.ClientID %>').value = objeto.tiposangre;
            document.getElementById('<%=txtdirdom.ClientID %>').value = objeto.dirdom;
            document.getElementById('<%=txtteldom.ClientID %>').value = objeto.telfdom;
            document.getElementById('tmailinfocli').value = objeto.email;
            document.getElementById('<%=txtlugarnacimiento.ClientID %>').value = objeto.lugnac;
            document.getElementById('<%=txtfechanacimiento.ClientID %>').value = objeto.fecnac;
            document.getElementById('<%=ddlCargoOnlyControl.ClientID %>').value = objeto.cargo;
//            document.getElementById('<%=txttiplic.ClientID %>').value = objeto.tiplic;
//            document.getElementById('<%=txtfecexplic.ClientID %>').value = objeto.fecexplic;
//            document.getElementById('valnombre').textContent = "";
//            document.getElementById('valapellido').textContent = "";
//            document.getElementById('valcipas').textContent = "";
//            document.getElementById('valtipsangre').textContent = "";
//            document.getElementById('valdirdom').textContent = "";
//            document.getElementById('valtelofi').textContent = "";
//            //                document.getElementById('mailopcional').textContent = "";
//            document.getElementById('vallugarnac').textContent = "";
//            document.getElementById('valfecnac').textContent = "";
//            document.getElementById('valcargoac').textContent = "";
            //            document.getElementById('<%=txtnombres.ClientID %>').style.cssText = "background-color:White;width:400px;";
            //            document.getElementById('<%=txtapellidos.ClientID %>').style.cssText = "background-color:White;width:400px;";
            //            document.getElementById('<%=txtcipas.ClientID %>').style.cssText = "background-color:White;width:200px;";
        }
    </script>
  <asp:updateprogress  id="updateProgress" runat="server">
    <progresstemplate>
        <div id="progressBackgroundFilter"></div>
        <div id="processMessage"> Estamos procesando la tarea que solicitó, por favor  espere...<br />
         <img alt="Loading" src="../shared/imgs/loader.gif" style="margin:0 auto;" />
        </div>
    </progresstemplate>
    </asp:updateprogress>
</asp:Content>
