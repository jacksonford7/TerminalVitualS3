<%@ Page Language="C#" MasterPageFile="~/site.Master" Title="Emisión/Renovación de Credencial" MaintainScrollPositionOnPostback="true"
         AutoEventWireup="true" CodeBehind="solicitudcolaborador.aspx.cs" Inherits="CSLSite.cliente.solicitudcolaborador" %>
         <%@ MasterType VirtualPath="~/site.Master" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="placehead" runat="server">
    <link href="../shared/estilo/tables.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/aisv-general.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/work.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/catalogos.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/info.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/w3-progressbar.css" rel="stylesheet" type="text/css" />
    <link href="../shared/avisos/ppt.css" rel="stylesheet" type="text/css" />
<style type="text/css">
    .div {
    border: 2px solid;
    padding: 20px;
    width: 300px;
    resize: none;
    overflow: auto;
}
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
</style>
</asp:Content>
<asp:Content ID="formcolaborador" ContentPlaceHolderID="placebody" runat="server">
  <a name="MOVEHERE"></a>
  <input id="zonaid" type="hidden" value="7" />
<%--  <asp:ToolkitScriptManager ID="tkcal2" EnableScriptGlobalization="true" runat="server"> </asp:ToolkitScriptManager>--%>
  <asp:ScriptManager ID="sMan" EnableScriptGlobalization="true" runat="server"></asp:ScriptManager>
  <div>
        <asp:Timer ID="TimerPb" OnTick="TimerPb_Tick" Enabled="false" runat="server" Interval="10"></asp:Timer>
  </div>
  <noscript><meta http-equiv="refresh" content="0; url=sinsoporte.htm" /> </noscript>
   <div>
         <i class="ico-titulo-1"></i><h2>MCA - Módulo de Control de Acceso</h2><br />
         <i class="ico-titulo-2"></i><h1>Emisión/Renovación de Credencial</h1><br />
   </div>

   <div class="seccion">
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
     <table cellspacing="0" cellpadding="1" style=" width:100%">
           <tr>
     <td class="bt-bottom bt-left">
     <asp:DropDownList ID="ddlTipoEmpresa" runat="server" Width="405px"
             onchange="fEmpresa();valdltipsol(this, valtipempresa);">
             <asp:ListItem Value="0">* Seleccione origen *</asp:ListItem>
     </asp:DropDownList>
     </td>
     <td class="bt-bottom bt-right validacion "><span id='valtipempresa' class="validacion" > * obligatorio</span></td>
     </tr>
     </table>
     </div>
    </div>
    </div>
   <div class="seccion">
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
        <table cellspacing="0" cellpadding="1" style=" width:100%">
    <tr>
        <td "bt-bottom  bt-right bt-left">
        <div class=" msg-critico" runat="server" id="infotipcre" style=" font-weight:bold">
        </div>
        </td>
    </tr>
    </table>
     <table cellspacing="0" cellpadding="1" style=" width:100%">
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
     <div style=" height:80px; overflow-y:scroll" class="bt-right">--%>
     <%--<fieldset>--%>
     <%--<legend style="font-size:smaller; font-family:Consolas; font-weight:normal; font-style:italic; color:Red"></legend>--%>
     
     <%--</fieldset>--%>
     <%--</div>
     </td>--%>
     </tr>
     </table>
     </div>
    </div>
    </div>
   <div class="seccion">
      <div class="informativo">
      <table>
      <tr><td rowspan="2" class="inum"> <div class="number">3</div></td><td class="level1" >Datos Generales del Colaborador.</td></tr>
      <tr><td class="level2">
         Confirme que los datos sean correctos.
      </td></tr>
      </table>
     </div>
    <div class="colapser colapsa"></div>
      <div class="accion">
      <table class="controles" cellspacing="0" cellpadding="1">
        <tr>
        <td class="bt-bottom  bt-left" style=" width:280px; background-color:White" >
        Registro de Credencial [<input id="rbemision" onclick="fValidaEmision();" checked="true" runat="server"  type="radio" name="deck2" value="ci" />]
            <%--<asp:RadioButton ID="rbNuevo" runat="server" Text="Registro Nuevo Vehículo" 
                Width="250px" />--%>
        </td>
        <td class="bt-bottom">
        Renovación de Credencial [<input id="rbrenovacion" onclick="fValidaRenovacion();" runat="server" type="radio" name="deck2" value="pas"/>]
            <%--<asp:RadioButton ID="rbRenovacion" runat="server" 
                Text="Renovación Registro Vehículo" />--%>
        </td>
        <td class="bt-bottom bt-right validacion ">&nbsp;</td>
        </tr>
      </table>
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
         <td class="bt-bottom  bt-right bt-left" style=" width:155px" >
         C.I [ <input id="rbci" checked="true"  type="radio" name="deck" value="ci"/>] Pasaporte [<input id="rbpasaporte"  type="radio" name="deck" value="pas"/>]
         </td>
         <td class="bt-bottom">
            <asp:TextBox ID="txtcipas" runat="server" Width="200px" MaxLength="25"
             style="text-align: center"
             onBlur="valcipas(this,'valcipas','rbci','rbpasaporte');"
             onkeypress="return soloLetras(event,'01234567890abcdefghijklnmñopqrstuvwxyz',true)"></asp:TextBox>
         </td>
         <td align="right" class="bt-bottom bt-right validacion "  style="width:200px"><span class="validacion" id="valcipas" > * obligatorio</span></td>
         </tr>
      </table>
      <table class="controles" cellspacing="0" cellpadding="1">
         <tr>
         <td class="bt-bottom  bt-right bt-left" style=" width:155px" >Nombres:</td>
         <td class="bt-bottom">
           <asp:TextBox ID="txtnombres" runat="server" Width="400px" MaxLength="500"
             style="text-align: center;" onblur="checkcajalarge(this,'valnombre',true);"
             onkeypress="return soloLetras(event,'abcdefghijklnmñopqrstuvwxyz01234567890 ',true)"></asp:TextBox>
         </td>
         <td class="bt-bottom bt-right validacion "><span id="valnombre" class="validacion"> * obligatorio</span></td>
         </tr>
         <tr>
         <td class="bt-bottom  bt-right bt-left" style=" width:155px" >Apellidos:</td>
         <td class="bt-bottom">
           <asp:TextBox ID="txtapellidos" runat="server" Width="400px" MaxLength="500"
             style="text-align: center;" onblur="checkcajalarge(this,'valapellido',true);"
             onkeypress="return soloLetras(event,'abcdefghijklnmñopqrstuvwxyz01234567890 ',true)"></asp:TextBox>
         </td>
         <td class="bt-bottom bt-right validacion "><span id="valapellido" class="validacion"> * obligatorio</span></td>
         </tr>
      </table>
      <table class="controles" cellspacing="0" cellpadding="1">
         <tr>
         <td class="bt-bottom  bt-right bt-left" style=" width:155px" >Tipo de Sangre:</td>
         <td class="bt-bottom">
           <asp:TextBox ID="txttiposangre" runat="server" Width="200px" MaxLength="500"
             style="text-align: center" onblur="checkcaja(this,'valtipsangre',true);"
             onkeypress="return soloLetras(event,'abcdefghijklnmñopqrstuvwxyz+- ',true)"></asp:TextBox>
         </td>
         <td class="bt-bottom bt-right validacion "><span id="valtipsangre" class="validacion"> * obligatorio</span></td>
         </tr>
         <tr><td class="bt-bottom  bt-right bt-left" style=" width:155px">Dirección de Domicilio:</td>
         <td class="bt-bottom">
            <asp:TextBox ID="txtdirdom" runat="server" Width="400px" MaxLength="500"
             style="text-align: center" onblur="checkcajalarge(this,'valdirdom',true);"
             onkeypress="return soloLetras(event,'abcdefghijklnmñopqrstuvwxyz01234567890 /_-.',true)"></asp:TextBox>
         </td>
         <td class="bt-bottom bt-right validacion "><span id="valdirdom" class="validacion"> * obligatorio</span></td>
         </tr>
         <tr>
         <td class="bt-bottom  bt-right bt-left" style=" width:155px" >Teléfono de Domicilio:</td>
         <td class="bt-bottom">
            <asp:TextBox ID="txtteldom" runat="server" Width="200px" MaxLength="9"
             style="text-align: center"
             onBlur="telconvencional(this,'valtelofi',true);"
             onkeypress="return soloLetras(event,'01234567890',true)"></asp:TextBox>
         </td>
         <td class="bt-bottom bt-right validacion "><span id="valtelofi" class="validacion"> * obligatorio</span></td>
         </tr>
         <tr>
         <td class="bt-bottom  bt-right bt-left" style=" width:155px;" >Correo Electronico:</td>
         <td class="bt-bottom">
         <%--<div id='TextBoxesGroup'>--%>
           <%--<div id="TextBoxDiv1" class="cntmail">--%>
               <%--<span>mail #1:</span>--%>
               <a class="tooltip"><span class="classic">
                Para mas de un Mail separelos con punto y coma.</span>
               <input type='text' id='tmailinfocli' name='textboxmailinfocli' runat="server" style=" width:400px;"
                enableviewstate="false" clientidmode="Static"
                onkeypress="return soloLetras(event,'abcdefghijklmnñopqrstuvwxyz1234567890_$@.;',true)"
                onblur="mailopcional(this,'mailopcional');"
                placeholder="mail@mail.com"
                />
                </a>
           <%--</div>--%>
         <%--</div>--%>
             <%--    <input type='button' value='Agregar' id='addButton' />
                 <input type='button' value='Remover' id='removeButton' />--%>
          </td>
          <%--<td class="bt-bottom bt-right validacion "><span id="valmailrl" class="validacion"> * opcional</span></td>--%>
          <td class="bt-bottom bt-right"><span style="font-size:small; font-family:Consolas; font-weight:normal; font-style:italic; color:Green" id="mailopcional"> * opcional</span></td>
         </tr>
         <tr><td class="bt-bottom  bt-right bt-left" style=" width:155px">Lugar de Nacimiento:</td>
         <td class="bt-bottom">
             <asp:TextBox 
             style="text-align: center" 
             ID="txtlugarnacimiento" runat="server" width="200px" MaxLength="500"
             onkeypress="return soloLetras(event,'abcdefghijklnmñopqrstuvwxyz01234567890 /_-.')" 
             onblur="checkcaja(this,'vallugarnac',true);"></asp:TextBox>
         </td>
         <td class="bt-bottom bt-right validacion "><span id="vallugarnac" class="validacion"> * obligatorio</span></td>
         </tr>
         <tr><td class="bt-bottom  bt-right bt-left" style=" width:155px">Fecha de Nacimiento:</td>
         <td class="bt-bottom">
             <asp:TextBox 
             style="text-align: center" 
             ID="txtfechanacimiento" runat="server"  width="200px" MaxLength="15" CssClass="datetimepicker"
             onkeypress="return soloLetras(event,'0123456789/')" 
             onblur="checkcaja(this,'valfecnac',true);"></asp:TextBox>
         </td>
         <td class="bt-bottom bt-right validacion "><span id="valfecnac" class="validacion" > * obligatorio</span></td>
         </tr>
         <tr><td class="bt-bottom  bt-right bt-left" style=" width:155px">Tipo de Licencia:</td>
         <td class="bt-bottom">
             <%--<asp:TextBox  Enabled="false" BackColor="Gray"
             style="text-align: center; text-transform: uppercase" onblur="checkcaja(this,'valtipolic',true);"
             ID="txttiplic2" runat="server" ClientIDMode="Static" width="200px" MaxLength="500" 
             onkeypress="return soloLetras(event,'abcdefghijklnmñopqrstuvwxyz01234567890 /_-.')" 
             ></asp:TextBox>--%>
            <asp:DropDownList ID="ddlTipoLicencia" runat="server" Width="205px"
                 onchange="fValidaTipLic();valdlcatveh(this, valtipolic);" 
                    style="margin-top: 0px">
                 <asp:ListItem Value="0">* Seleccione origen *</asp:ListItem>
            </asp:DropDownList>
         </td>
         <%--<td class="bt-bottom bt-right validacion"><span id="valtipolic" class="validacion"></span></td>--%>
         <td class="bt-bottom bt-right validacion "><span id="valtipolic" class="validacion">* obligatorio</span></td>
         </tr>
         <tr><td class="bt-bottom  bt-right bt-left" style=" width:155px">Fecha Exp. Licencia:</td>
         <td class="bt-bottom">
             <asp:TextBox 
             style="text-align: center"  onblur="checkcaja(this,'valfecexplic',true);"
             ID="txtfecexplic" runat="server"  width="200px" MaxLength="15" CssClass="datetimepicker"
             onkeypress="return soloLetras(event,'0123456789/')" 
             ></asp:TextBox>
         </td>
         <%--<td class="bt-bottom bt-right validacion"></td>--%>
         <td class="bt-bottom bt-right validacion "><span id="valfecexplic" class="validacion" >* obligatorio</span></td>
         </tr>
         <tr><td class="bt-bottom  bt-right bt-left" style=" width:155px">Cargo del Empleado:</td>
         <td class="bt-bottom">
             <asp:TextBox 
             style="text-align: center"
             ID="txtcargo" runat="server" ClientIDMode="Static" width="400px" MaxLength="500"
             onkeypress="return soloLetras(event,'abcdefghijklnmñopqrstuvwxyz01234567890 /_-.')" 
             onblur="checkcaja(this,'valcargo',true);"></asp:TextBox>
         </td>
         <td class="bt-bottom bt-right validacion"><span id="valcargo" class="validacion"> * obligatorio</span></td>
         </tr>
         
         <tr><td class="bt-bottom  bt-right bt-left" style=" width:155px">Nota:</td>
         <td class="bt-bottom">
                 <asp:TextBox runat="server" ID="txtNota" MaxLength="3000" TextMode="MultiLine" Width="400px" style="overflow:auto;resize:none"
                              onkeypress="return soloLetras(event,'abcdefghijklnmñopqrstuvwxyz01234567890 /_-.',true)"/>
         </td>
         <td class="bt-bottom bt-right"><span style="font-size:small; font-family:Consolas; font-weight:normal; font-style:italic; color:Green" id="Span1"> * opcional</span></td>
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
         <%--<td class="bt-bottom bt-right validacion "><span id="valaredes" class="validacion"> * obligatorio</span></td>--%>
         <%--<td class="bt-bottom bt-right" style=" width:200PX"><span style="font-size:small; font-family:Consolas; font-weight:normal; font-style:italic; color:Green" id="Span4"> * opcional</span></td>--%>
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
         <%--<td class="bt-bottom bt-right validacion "><span id="valTiempo" class="validacion"> * obligatorio</span></td>--%>
         <%--<td class="bt-bottom bt-right" style=" width:200PX"><span style="font-size:small; font-family:Consolas; font-weight:normal; font-style:italic; color:Green;" id="Span1"> * opcional</span></td>--%>
         </tr>
         </table>
      <table class="controles" cellspacing="0" cellpadding="1">
         <tr>
      <td class="bt-left" style=" width:155px; background-color:White"></td>
      <td>
      <div>
      <asp:Button ID="btnAgregar" runat="server" Width="100px" Text="Agregar" OnClientClick="return validaCabecera();"
                onclick="btnAgregar_Click" />
      <img alt="loading.." src="../shared/imgs/loader.gif" id="loader" class="nover"  />
     </div>
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
                          <asp:Label ID="tnombres" runat="server" Enabled="false" Width="100px" ToolTip='<%# Eval("[Nombres]") %>' Text='<%# Eval("[Nombres]") %>'></asp:Label>
                      </ItemTemplate>
                  </asp:TemplateField>
                  <asp:TemplateField HeaderText="Apellidos">
                      <ItemTemplate>
                          <asp:Label ID="tapellidos" runat="server" Enabled="false" Width="100px" ToolTip='<%# Eval("[Apellidos]") %>' Text='<%# Eval("[Apellidos]") %>'
                          ></asp:Label>
                      </ItemTemplate>
                  </asp:TemplateField>
                  <asp:TemplateField HeaderText="C.I./Pasaporte">
                      <ItemTemplate>
                          <asp:Label ID="tcipas" runat="server" Width="50px" ToolTip='<%# Eval("[CIPasaporte]") %>' Text='<%# Eval("[CIPasaporte]") %>' ></asp:Label>
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
                  <asp:TemplateField HeaderText="TipoLicencia" Visible="false">
                      <ItemTemplate>
                         <asp:Label ID="tareades"  style="text-transform :uppercase" runat="server" Width="70px" Text='<%# Eval("[TipoLicencia]") %>'></asp:Label>
                      </ItemTemplate>
                  </asp:TemplateField>
                  <asp:TemplateField HeaderText="Cargo">
                      <ItemTemplate>
                          <asp:Label ID="tcargo"  style="text-transform :uppercase" runat="server" Width="70px" ToolTip='<%# Eval("[Cargo]") %>' Text='<%# Eval("[Cargo]") %>'></asp:Label>
                      </ItemTemplate>
                  </asp:TemplateField>
                   <asp:TemplateField HeaderText="Nota">
                      <ItemTemplate>
                      <div style="overflow-y:scroll; overflow-x: hidden; height:30px; text-align:center">
                          <asp:Label ID="tnota"  style="text-transform :uppercase" runat="server" ToolTip='<%# Eval("[Nota]") %>' Text='<%# Eval("[Nota]") %>'></asp:Label>
                      </div>
                      </ItemTemplate>
                  </asp:TemplateField>
                  <asp:TemplateField HeaderText="FechaExpLicencia" Visible="false">
                      <ItemTemplate>
                          <asp:Label ID="ttiempoest"  style="text-transform :uppercase" runat="server" Width="70px" Text='<%# Eval("[FecExpLicencia]") %>'></asp:Label>
                      </ItemTemplate>
                  </asp:TemplateField>
                   <asp:TemplateField HeaderText="NO Cargados" Visible="false">
                      <ItemTemplate>
                          <%--<asp:TextBox ID="tplaca" runat="server" Width="70px" Text='<%# Eval("Placa") %>' onkeypress="return soloLetras(event,'abcdefghijklnmñopqrstuvwxyz01234567890',true)"></asp:TextBox>--%>
                          <asp:CheckBox ID="chkEstadoDocumentos" runat="server" Width="50px" Enabled="false"/>
                      </ItemTemplate>
                  </asp:TemplateField>
                  <asp:TemplateField ShowHeader="False" HeaderStyle-Width="70px" HeaderText="Adjuntar">
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
        <%--<asp:AsyncPostBackTrigger ControlID="ddlTipoEmpresa" />--%>
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
    <asp:UpdatePanel ID="UpdatePanelpb" UpdateMode="Conditional" runat="server">
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="TimerPb" EventName="Tick" />
        </Triggers>
        <ContentTemplate>
            <div class="botonera">
            <asp:Button ID="btsalvar" runat="server" Text="Enviar Solicitud"  onclick="btsalvar_Click" ClientIDMode="Static"
                            OnClientClick="return prepareObject('¿Esta seguro de enviar la solicitud?');"
                            ToolTip="Confirma la información y genera el envio de la solicitud."/>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </div>
   <asp:HiddenField runat="server" ID="emailsec2" />
    <%--<script src='http://www.w3schools.com/lib/w3.css'></script>--%>
    <script src="../Scripts/pages.js" type="text/javascript"></script>
    <script src="../Scripts/credenciales.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.tablesorter.min.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.tablesorter.pager.js" type="text/javascript"></script>
    <script src='../shared/avisos/popup_script.js' type='text/javascript' ></script> 
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
                var vals = document.getElementById('<%=ddlTipoEmpresa.ClientID %>').value;
                if (vals == 0) {
                    alert('* Seleccione el Tipo de Empresa *');
                    document.getElementById('<%=ddlTipoEmpresa.ClientID %>').focus();
                    document.getElementById('<%=ddlTipoEmpresa.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:400px;";
                    document.getElementById("loader").className = 'nover';
                    return false;
                }
                var vals = document.getElementById('<%=cbltiposolicitud.ClientID %>').value;
                if (vals == 0) {
                    alert('* Seleccione al menos un Tipo de Solicitud *');
                    document.getElementById('<%=cbltiposolicitud.ClientID %>').focus();
                    document.getElementById('<%=cbltiposolicitud.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:400px;";
                    document.getElementById("loader").className = 'nover';
                    return false;
                }
                document.getElementById('<%=ddlTipoEmpresa.ClientID %>').focus();
                return true;
            } catch (e) {
                alert('Lamentamos comunicarle que ha ocurrido el siguiente problema:\n' + e.Message);
            }
        }
        function Panel() {
            $(document).ready(function () {
                $('#ventana_popup').fadeIn('slow');
                $('#popup-overlay').fadeIn('slow');
                $('#popup-overlay').height($(window).height());
            });
        }
        function validaCabecera() {
            try {
                //Valida los datos de las secciones
                var vals = document.getElementById('<%=ddlTipoEmpresa.ClientID %>').value;
                if (vals == 0) {
                    alert('* Seleccione el Tipo de Empresa *');
                    document.getElementById('<%=ddlTipoEmpresa.ClientID %>').focus();
                    document.getElementById('<%=ddlTipoEmpresa.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:400px;";
                    document.getElementById("loader").className = 'nover';
                    return false;
                }
                var vals = document.getElementById('<%=cbltiposolicitud.ClientID %>').value;
                if (vals == 0) {
                    alert('* Seleccione el Tipo de Solicitud *');
                    document.getElementById('<%=cbltiposolicitud.ClientID %>').focus();
                    document.getElementById('<%=cbltiposolicitud.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:400px;";
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
                    alert('* Escriba la Dirección de Domicilio *');
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
                var control = document.getElementById('tmailinfocli').value.trim();
                if (control.length > 0) {
                    if (!validarVariosEmail(control)) {
                        alert('Mail no parece correcto');
                        document.getElementById('mailopcional').innerHTML = '<span class="obligado">Mail no parece correcto</span>';
                        document.getElementById('tmailinfocli').focus();
                        return false;
                    }
                }
                document.getElementById('mailopcional').innerHTML = ' * opcional';

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
//                    alert('* Escriba el Area de Destino *');
//                    document.getElementById('<%=txtaredes.ClientID %>').focus();
//                    document.getElementById('<%=txtaredes.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:200px;";
//                    document.getElementById("loader").className = 'nover';
//                    return false;
//                }
//                document.getElementById('<%=tmailinfocli.ClientID %>').disabled = false;
//                var vals = document.getElementById('<%=txttiempoestadia.ClientID %>').value;
//                if (vals == '' || vals == null || vals == undefined) {
//                    alert('* Escriba el Tiempo de Estadia *');
//                    document.getElementById('<%=txttiempoestadia.ClientID %>').focus();
//                    document.getElementById('<%=txttiempoestadia.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:200px;";
//                    document.getElementById("loader").className = 'nover';
//                    return false;
                //                }
                var vals = document.getElementById('<%=ddlTipoEmpresa.ClientID %>').value;
                if (vals == 12) {
                    var vals = document.getElementById('<%=ddlTipoLicencia.ClientID %>').value;
                    if (vals == 0) {
                        alert('* Seleccione el Tipo de Licencia *');
                        document.getElementById('<%=ddlTipoLicencia.ClientID %>').focus();
                        document.getElementById('<%=ddlTipoLicencia.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:200px;";
                        document.getElementById("loader").className = 'nover';
                        return false;
                    }
                    var vals = document.getElementById('<%=txtfecexplic.ClientID %>').value;
                    if (vals == '' || vals == null || vals == undefined) {
                        alert('* Escriba la Fecha de Expiración de la Licencia *');
                        document.getElementById('<%=txtfecexplic.ClientID %>').focus();
                        document.getElementById('<%=txtfecexplic.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:200px;";
                        document.getElementById("loader").className = 'nover';
                        return false;
                    }
                }
                var vals = document.getElementById('<%=txtcargo.ClientID %>').value;
                if (vals == '' || vals == null || vals == undefined) {
                    alert('* Escriba el Cargo *');
                    document.getElementById('<%=txtcargo.ClientID %>').focus();
                    document.getElementById('<%=txtcargo.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:200px;";
                    document.getElementById("loader").className = 'nover';
                    return false;
                }
                document.getElementById('<%=txtcargo.ClientID %>').disabled = false;
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
                alert('* Escoja la Fecha de Nacimiento *');
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
                        alert('* Solicitud de Credencial/Permiso Provisional: *\n * No. C.I. no válido! *');
                        document.getElementById('<%=txtcipas.ClientID %>').focus();
                        document.getElementById('<%=txtcipas.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:200;";
                        document.getElementById("loader").className = 'nover';
                        return false;
                    }
                }
                else {
                    if (final != 10) {
                        alert('* Solicitud de Credencial/Permiso Provisional: *\n * No. C.I. no válido! *');
                        document.getElementById('<%=txtcipas.ClientID %>').focus();
                        document.getElementById('<%=txtcipas.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:200;";
                        document.getElementById("loader").className = 'nover';
                        return false;
                    }
                }
            }
            if (vpas == true) {
                if (valruccipas == '' || valruccipas == null || valruccipas == undefined) {
                    alert('* Solicitud de Credencial/Permiso Provisional: *\n * Escriba el No. de Pasaporte *');
                    document.getElementById('<%=txtcipas.ClientID %>').focus();
                    document.getElementById('<%=txtcipas.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:200;";
                    document.getElementById("loader").className = 'nover';
                    return false;
                }
            }
            return true;
        }
        function redireccionar() {
            window.locationf = "~/cuenta/zones.aspx";
        }
        function redirectcatdoc(val) {
            var caja = val;
            //window.open('../credenciales/documentos/?placa=' + caja, 'name', 'width=850,height=480')
            window.open('../cliente/solicitudcolaboradordocumentos.aspx?CIPAS=' + caja, 'name', 'width=950,height=480')
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
            document.getElementById('<%=txtcargo.ClientID %>').value = objeto.cargo;
            document.getElementById('<%=ddlTipoLicencia.ClientID %>').value = objeto.tiplic;
            document.getElementById('<%=txtfecexplic.ClientID %>').value = objeto.fecexplic;
            document.getElementById('valnombre').textContent = "";
            document.getElementById('valapellido').textContent = "";
            document.getElementById('valcipas').textContent = "";
            document.getElementById('valtipsangre').textContent = "";
            document.getElementById('valdirdom').textContent = "";
            document.getElementById('valtelofi').textContent = "";
            //                document.getElementById('mailopcional').textContent = "";
            document.getElementById('vallugarnac').textContent = "";
            document.getElementById('valfecnac').textContent = "";
            document.getElementById('valfecexplic').textContent = "";
            document.getElementById('valtipolic').textContent = "";
            document.getElementById('valcargo').textContent = "";

            document.getElementById('<%=txtnombres.ClientID %>').style.cssText = "background-color:White;width:400px;";
            document.getElementById('<%=txtapellidos.ClientID %>').style.cssText = "background-color:White;width:400px;";
            document.getElementById('<%=txtcipas.ClientID %>').style.cssText = "background-color:White;width:200px;";

            document.getElementById('<%=txttiposangre.ClientID %>').style.cssText = "background-color:White;width:200px;";
            document.getElementById('<%=txtdirdom.ClientID %>').style.cssText = "background-color:White;width:400px;";
            document.getElementById('<%=txtteldom.ClientID %>').style.cssText = "background-color:White;width:200px;";
            document.getElementById('<%=txtlugarnacimiento.ClientID %>').style.cssText = "background-color:White;width:200px;";
            document.getElementById('<%=txtfechanacimiento.ClientID %>').style.cssText = "background-color:White;width:200px;";
//            document.getElementById('<%=ddlTipoLicencia.ClientID %>').value = "0";
//            document.getElementById('<%=ddlTipoLicencia.ClientID %>').disabled = false;
//            document.getElementById('<%=ddlTipoLicencia.ClientID %>').style.cssText = "background-color:White;width:200px;";        
            fEmpresa();
//            document.getElementById('<%=txtcargo.ClientID %>').value = "";
//            document.getElementById('<%=txtcargo.ClientID %>').disabled = false;
//            document.getElementById('<%=txtcargo.ClientID %>').style.cssText = "background-color:White;width:200px;";
//            document.getElementById('<%=txtfecexplic.ClientID %>').value = "";
//            document.getElementById('<%=txtfecexplic.ClientID %>').disabled = false;
//            document.getElementById('<%=txtfecexplic.ClientID %>').style.cssText = "background-color:White;width:200px;";
        }
        function fvalidaCriterios() {
            var renovacion = document.getElementById('<%=rbrenovacion.ClientID %>').checked;
            if (renovacion) {
                var vals = document.getElementById('<%=ddlTipoEmpresa.ClientID %>').value;
                if (vals == 0) {
                    alert('Seleccione el Tipo de Empresa');
                    document.getElementById('<%=ddlTipoEmpresa.ClientID %>').focus();
                    document.getElementById('<%=ddlTipoEmpresa.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:400px;";
                    document.getElementById("loader").className = 'nover';
                    return false;
                }
                var vals = document.getElementById('<%=cbltiposolicitud.ClientID %>').value;
                if (vals == 0) {
                    alert('Seleccione al menos un Tipo de Solicitud');
                    document.getElementById('<%=cbltiposolicitud.ClientID %>').focus();
                    document.getElementById('<%=cbltiposolicitud.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:400px;";
                    document.getElementById("loader").className = 'nover';
                    return false;
                }
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
                document.getElementById('<%=txtcriterioconsulta.ClientID %>').value = document.getElementById('<%=txtcriterioconsulta.ClientID %>').value.toUpperCase();
            }
            else {
                alert('Solo puede buscar cuando es una Renovación de Credencial.');
                document.getElementById('<%=txtcriterioconsulta.ClientID %>').focus();
                document.getElementById('<%=txtcriterioconsulta.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:200px;";                
                return false;
            }
            return true;
        }
        function fValidaRenovacion() {
            var renovacion = document.getElementById('<%=rbrenovacion.ClientID %>').checked;
            if (renovacion) {
                document.getElementById('<%=txtcriterioconsulta.ClientID %>').focus();
                document.getElementById('<%=txtcriterioconsulta.ClientID %>').value = "";
                document.getElementById('<%=txtcipas.ClientID %>').disabled = true;
                document.getElementById('<%=txtcipas.ClientID %>').style.cssText = "background-color:Gray;width:200px;";
                document.getElementById('<%=txtnombres.ClientID %>').disabled = true;
                document.getElementById('<%=txtnombres.ClientID %>').style.cssText = "background-color:Gray;width:400px;";
                document.getElementById('<%=txtapellidos.ClientID %>').disabled = true;
                document.getElementById('<%=txtapellidos.ClientID %>').style.cssText = "background-color:Gray;width:400px;";

                document.getElementById('<%=txtNota.ClientID %>').disabled = true;
                document.getElementById('<%=txtNota.ClientID %>').style.cssText = "background-color:Gray;width:400px;";

                document.getElementById('<%=txttiposangre.ClientID %>').disabled = true;
                document.getElementById('<%=txttiposangre.ClientID %>').style.cssText = "background-color:Gray;width:200px;";
                document.getElementById('<%=txtdirdom.ClientID %>').disabled = true;
                document.getElementById('<%=txtdirdom.ClientID %>').style.cssText = "background-color:Gray;width:400px;";
                document.getElementById('<%=txtteldom.ClientID %>').disabled = true;
                document.getElementById('<%=txtteldom.ClientID %>').style.cssText = "background-color:Gray;width:200px;";
                document.getElementById('<%=tmailinfocli.ClientID %>').disabled = true;
                document.getElementById('<%=tmailinfocli.ClientID %>').style.cssText = "background-color:Gray;width:400px;";
                document.getElementById('<%=txtlugarnacimiento.ClientID %>').disabled = true;
                document.getElementById('<%=txtlugarnacimiento.ClientID %>').style.cssText = "background-color:Gray;width:200px;";
                document.getElementById('<%=txtfechanacimiento.ClientID %>').disabled = true;
                document.getElementById('<%=txtfechanacimiento.ClientID %>').style.cssText = "background-color:Gray;width:200px;";
                document.getElementById('<%=txtcargo.ClientID %>').disabled = true;
                document.getElementById('<%=txtcargo.ClientID %>').style.cssText = "background-color:Gray;width:200px;";
                document.getElementById('<%=ddlTipoLicencia.ClientID %>').disabled = true;
                document.getElementById('<%=ddlTipoLicencia.ClientID %>').style.cssText = "background-color:Gray;width:200px;";
                document.getElementById('<%=txtfecexplic.ClientID %>').disabled = true;
                document.getElementById('<%=txtfecexplic.ClientID %>').style.cssText = "background-color:Gray;width:200px;";
                document.getElementById('<%=ddlTipoLicencia.ClientID %>').value = "0";
//                document.getElementById('<%=txtcargo.ClientID %>').disabled = true;
//                document.getElementById('<%=txtcargo.ClientID %>').style.cssText = "background-color:Gray;width:200px;";
                document.getElementById('<%=txtcipas.ClientID %>').value = "";
                document.getElementById('<%=txtnombres.ClientID %>').value = "";
                document.getElementById('<%=txtapellidos.ClientID %>').value = "";
                document.getElementById('<%=txttiposangre.ClientID %>').value = "";
                document.getElementById('<%=txtdirdom.ClientID %>').value = "";
                document.getElementById('<%=txtteldom.ClientID %>').value = "";
                document.getElementById('<%=tmailinfocli.ClientID %>').value = "";
                document.getElementById('<%=txtlugarnacimiento.ClientID %>').value = "";
                document.getElementById('<%=txtfechanacimiento.ClientID %>').value = "";
                document.getElementById('<%=txtcargo.ClientID %>').value = "";
                document.getElementById('<%=ddlTipoLicencia.ClientID %>').value = "";
                document.getElementById('<%=txtfecexplic.ClientID %>').value = "";
                document.getElementById('valnombre').textContent = "";
                document.getElementById('valapellido').textContent = "";
                document.getElementById('valcipas').textContent = "";
                document.getElementById('valtipsangre').textContent = "";
                document.getElementById('valdirdom').textContent = "";
                document.getElementById('valtelofi').textContent = "";
//                document.getElementById('mailopcional').textContent = "";
                document.getElementById('vallugarnac').textContent = "";
                document.getElementById('valfecnac').textContent = "";

                document.getElementById('valfecexplic').textContent = "";
                document.getElementById('valtipolic').textContent = "";
                document.getElementById('valcargo').textContent = "";

                document.getElementById('<%=ddlTipoLicencia.ClientID %>').value = "0";
                document.getElementById('<%=ddlTipoLicencia.ClientID %>').disabled = true;
                document.getElementById('<%=ddlTipoLicencia.ClientID %>').style.cssText = "background-color:Gray;width:200px;";
                document.getElementById('<%=txtcargo.ClientID %>').value = "";
                document.getElementById('<%=txtcargo.ClientID %>').disabled = true;
                document.getElementById('<%=txtcargo.ClientID %>').style.cssText = "background-color:Gray;width:200px;";
//                fEmpresa();
            }
        }
        function fValidaEmision() {
            var emision = document.getElementById('<%=rbemision.ClientID %>').checked;
            if (emision) {
                
                document.getElementById('<%=txtcriterioconsulta.ClientID %>').value = "";
                document.getElementById('<%=txtcipas.ClientID %>').disabled = false;
                document.getElementById('<%=txtcipas.ClientID %>').style.cssText = "background-color:White;width:200px;";
                document.getElementById('<%=txtnombres.ClientID %>').disabled = false;
                document.getElementById('<%=txtnombres.ClientID %>').style.cssText = "background-color:White;width:400px;";
                document.getElementById('<%=txtapellidos.ClientID %>').disabled = false;
                document.getElementById('<%=txtapellidos.ClientID %>').style.cssText = "background-color:White;width:400px;";

                document.getElementById('<%=txtNota.ClientID %>').disabled = false;
                document.getElementById('<%=txtNota.ClientID %>').style.cssText = "background-color:White;width:400px;";

                document.getElementById('<%=txttiposangre.ClientID %>').disabled = false;
                document.getElementById('<%=txttiposangre.ClientID %>').style.cssText = "background-color:White;width:200px;";
                document.getElementById('<%=txtdirdom.ClientID %>').disabled = false;
                document.getElementById('<%=txtdirdom.ClientID %>').style.cssText = "background-color:White;width:400px;";
                document.getElementById('<%=txtteldom.ClientID %>').disabled = false;
                document.getElementById('<%=txtteldom.ClientID %>').style.cssText = "background-color:White;width:200px;";
                document.getElementById('<%=tmailinfocli.ClientID %>').disabled = false;
                document.getElementById('<%=tmailinfocli.ClientID %>').style.cssText = "background-color:White;width:400px;";
                document.getElementById('<%=txtlugarnacimiento.ClientID %>').disabled = false;
                document.getElementById('<%=txtlugarnacimiento.ClientID %>').style.cssText = "background-color:White;width:200px;";
                document.getElementById('<%=txtfechanacimiento.ClientID %>').disabled = false;
                document.getElementById('<%=txtfechanacimiento.ClientID %>').style.cssText = "background-color:White;width:200px;";
                document.getElementById('<%=txtcargo.ClientID %>').disabled = true;
                document.getElementById('<%=txtcargo.ClientID %>').style.cssText = "background-color:Gray;width:200px;";
                document.getElementById('<%=txtfecexplic.ClientID %>').disabled = false;
                document.getElementById('<%=txtfecexplic.ClientID %>').style.cssText = "background-color:White;width:200px;";
                document.getElementById('<%=txtcipas.ClientID %>').value = "";
                document.getElementById('<%=txtnombres.ClientID %>').value = "";
                document.getElementById('<%=txtapellidos.ClientID %>').value = "";
                document.getElementById('<%=txttiposangre.ClientID %>').value = "";
                document.getElementById('<%=txtdirdom.ClientID %>').value = "";
                document.getElementById('<%=txtteldom.ClientID %>').value = "";
                document.getElementById('<%=tmailinfocli.ClientID %>').value = "";
                document.getElementById('<%=txtlugarnacimiento.ClientID %>').value = "";
                document.getElementById('<%=txtfechanacimiento.ClientID %>').value = "";
                document.getElementById('<%=txtcargo.ClientID %>').value = "";
                document.getElementById('<%=ddlTipoLicencia.ClientID %>').value = "";
                document.getElementById('<%=txtfecexplic.ClientID %>').value = "";
                document.getElementById('<%=txtcipas.ClientID %>').focus();
                document.getElementById('valnombre').textContent = " * obligatorio";
                document.getElementById('valapellido').textContent = " * obligatorio";
                document.getElementById('valcipas').textContent = " * obligatorio";
                document.getElementById('valtipsangre').textContent = " * obligatorio";
                document.getElementById('valdirdom').textContent = " * obligatorio";
                document.getElementById('valtelofi').textContent = " * obligatorio";
//                document.getElementById('mailopcional').textContent = " * obligatorio";
                document.getElementById('vallugarnac').textContent = " * obligatorio";
                document.getElementById('valfecnac').textContent = " * obligatorio";
                document.getElementById('valfecexplic').textContent = " * obligatorio";
                document.getElementById('valtipolic').textContent = " * obligatorio";
                document.getElementById('valcargo').textContent = " * obligatorio";

                document.getElementById('<%=ddlTipoLicencia.ClientID %>').value = "0";
                document.getElementById('<%=ddlTipoLicencia.ClientID %>').disabled = true;
                document.getElementById('<%=ddlTipoLicencia.ClientID %>').style.cssText = "background-color:Gray;width:200px;";
//                document.getElementById('<%=txtcargo.ClientID %>').value = "";
//                document.getElementById('<%=txtcargo.ClientID %>').disabled = true;
                //                document.getElementById('<%=txtcargo.ClientID %>').style.cssText = "background-color:Gray;width:200px;";
                fEmpresa();
            }
        }
        function fIrAlCampoCriterioDeBusqueda() {
            document.getElementById('<%=txtcriterioconsulta.ClientID %>').focus();
//            document.getElementById('<%=txtcriterioconsulta.ClientID %>').value = "";
        } 
        function rbemision_onclick() {

        }
        function fValidaTipLic() {
//            var vals = document.getElementById('<%=ddlTipoLicencia.ClientID %>').value;
//            if (vals == 'E' || vals == 'G') {
                document.getElementById('<%=txtcargo.ClientID %>').value = "CONDUCTOR";
                document.getElementById('<%=txtcargo.ClientID %>').style.cssText = "background-color:White;width:200px;";
                document.getElementById('<%=txtcargo.ClientID %>').disabled = true;
                document.getElementById('valtipolic').textContent = "";
//            }
//            else {
//                document.getElementById('<%=txtcargo.ClientID %>').value = "";
//                document.getElementById('<%=txtcargo.ClientID %>').disabled = false;
//                document.getElementById('<%=txtcargo.ClientID %>').style.cssText = "background-color:White;width:200px;";
//                document.getElementById('valtipolic').textContent = " * obligatorio";
//            }
        }
        function fEmpresa() {
            var vals = document.getElementById('<%=ddlTipoEmpresa.ClientID %>').value;
            if (vals == 12) {
                document.getElementById('<%=ddlTipoLicencia.ClientID %>').disabled = false;
                document.getElementById('<%=txtfecexplic.ClientID %>').disabled = false;
                document.getElementById('<%=txtcargo.ClientID %>').disabled = true;
//                document.getElementById('<%=txtcargo.ClientID %>').style.cssText = "background-color:White;width:200px;";
                document.getElementById('<%=ddlTipoLicencia.ClientID %>').style.cssText = "background-color:White;width:200px;";
                document.getElementById('<%=txtfecexplic.ClientID %>').style.cssText = "background-color:White;width:200px;";
                document.getElementById('valtipolic').textContent = " * obligatorio";
                document.getElementById('valfecexplic').textContent = " * obligatorio";
                document.getElementById('<%=ddlTipoLicencia.ClientID %>').value = "0";
                document.getElementById('<%=txtfecexplic.ClientID %>').value = "";
            }
            else {
                document.getElementById('<%=ddlTipoLicencia.ClientID %>').disabled = true;
                document.getElementById('<%=txtfecexplic.ClientID %>').disabled = true;
                document.getElementById('<%=txtcargo.ClientID %>').disabled = true;
//                document.getElementById('<%=txtcargo.ClientID %>').style.cssText = "background-color:Gray;width:200px;";
                document.getElementById('<%=ddlTipoLicencia.ClientID %>').style.cssText = "background-color:Gray;width:200px;";
                document.getElementById('<%=txtfecexplic.ClientID %>').style.cssText = "background-color:Gray;width:200px;";
                document.getElementById('valtipolic').textContent = "";
                document.getElementById('valfecexplic').textContent = "";
                document.getElementById('<%=ddlTipoLicencia.ClientID %>').value = "0";
                document.getElementById('<%=txtfecexplic.ClientID %>').value = "";
            }
        }
        function modalPanel() {
            document.getElementById('close').style.display = "block";
            window.location = '../csl/menu';
        }
        function move(incio, intervalo) {
            document.getElementById('divpb').style.display = "block";
            var elem = document.getElementById("myBar");
            var width = incio;
            var id = setInterval(frame, 15);
            function frame() {
                if (width >= intervalo) {
                    clearInterval(id);
                } else {
                    width++;
                    elem.style.width = width + '%';
                    document.getElementById("demo").innerHTML = width * 1 + '%';
                }
            }
            return true;
        }
    </script>
    <div id="ventana_popup" style=" display:none;">
    <div id="ventana_content-popup">
            <div>
             <table style=" width:100%">
             <tr><td align="center"><p id='manage_ventana_popup'><asp:Label Text="Procesando!" id="lblProgreso" runat="server" /></p></td></tr>
             </table>
		        <div id='borde_ventana_popup'> 
                <br /><br /><br /><br />
			    <div  style='text-align:center;' >
                    <div id="divpb">
                    <div class="w3-progress-container">
                        <div id="myBar" class="w3-progressbar w3-green" style="width:0%">
                        </div>
                    </div>
                    <p id="demo">0%</p>
                    </div>
			    </div>
			    <br/>
			     <div  id="close" style=" display:none" >SALIR</div>
  			    </div>
            </div>
        </div>
    </div>
    <div id="popup-overlay"></div>
</asp:Content>
