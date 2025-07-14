<%@ Page  Title="Emisión/Renovación de Permiso Vehicular" Language="C#" MasterPageFile="~/site.Master" MaintainScrollPositionOnPostback="true"
         AutoEventWireup="true" CodeBehind="solicitudvehiculo.aspx.cs" 
         Inherits="CSLSite.cliente.solicitudvehiculo" %>
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
    <input id="zonaid" type="hidden" value="7" />
 <%-- <asp:ToolkitScriptManager ID="tkcal" EnableScriptGlobalization="true" runat="server"> </asp:ToolkitScriptManager>--%>
  <asp:ScriptManager ID="sMan" EnableScriptGlobalization="true" runat="server"></asp:ScriptManager>
  <div>
        <asp:Timer ID="TimerPb" OnTick="TimerPb_Tick" Enabled="false" runat="server" Interval="10"></asp:Timer>
  </div>
  <noscript><meta http-equiv="refresh" content="0; url=sinsoporte.htm" /> </noscript>
   <div>
         <i class="ico-titulo-1"></i><h2>MCA - Módulo de Control de Acceso</h2><br />
         <i class="ico-titulo-2"></i><h1>Solicitar Emisión/Renovación de Registro de Vehículo (Pesado o Liviano)</h1><br />
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
   <div class="seccion">
      <div class="informativo">
      <table class="controles" cellspacing="0" cellpadding="1">
      <tr><td rowspan="2" class="inum"> <div class="number">2</div></td><td class="level1" >
          Tipo de Solicitud.</td></tr>
      <tr><td class="level2">Tipo de Solicitud.</td></tr>
      </table>
     </div>
     <div class="colapser colapsa"></div>
       <%--<div class="accion">--%>
     <div class="bt-bottom  bt-right bt-left">
     <table cellspacing="0" cellpadding="1" style=" width:100%">
     <tr>
     <td class="bt-bottom bt-left">
     <asp:DropDownList ID="cbltiposolicitud" runat="server" Width="400px"
             onchange="valdltipsol(this, valtipsol);">
             <asp:ListItem Value="0">* Seleccione origen *</asp:ListItem>
     </asp:DropDownList>
     </td>
     <td class="bt-bottom bt-right validacion "><span id='valtipsol' class="validacion" > * obligatorio</span></td>
     </tr>
     </table>
     </div>
       <%--</div>--%>
    </div>
   <div class="seccion">
      <div class="informativo">
      <table>
      <tr><td rowspan="2" class="inum"> <div class="number">3</div></td><td class="level1" >Datos del Vehículo(S).</td></tr>
      <tr><td class="level2">
         Confirme que los datos sean correctos.
      </td></tr>
      </table>
     </div>
     <table class="controles" cellspacing="0" cellpadding="0">
        <tr>
            <td "bt-bottom  bt-right bt-left">
            <div class=" msg-critico" style=" font-weight:bold">
            <span>Consideraciones para vehículos PESADOS/MONTACARGAS:</span>
                <br />
                <span style=" font-weight:normal">* Pintar el número de placa en los techos de los vehículos, de acuerdo a las normativas vigentes.</span>
                <br />
                <span style=" font-weight:normal">* Tener correctamente colocadas las placas otorgadas por la autoridad competente en la parte frontal y posterior del vehículo.</span>
                <br />
                <span style=" font-weight:normal">* El vehículo debe tener el Logotipo de la Organización de transporte a la que pertenece, pintado en lugar visible.</span>
                <br />
                <span style=" font-weight:normal">* La fecha de vigencia de la póliza determina la fecha hasta la cual el vehículo podrá ingresar a la Terminal.<br /></span>
               <span> Consideraciones para vehículos LIVIANOS:</span><br />
               <span style=" font-weight:normal"> * Luego de aceptada la solicitud de registro del vehículo, realizar la solicitud 
                de ingreso temporal a la Terminal desde la opción: Control de Acceso - Vehículos / Solicitar Acceso Temporal de Vehículos Livianos previamente registrados.</span></div>
            </td>
            </tr>
        </table>
    <div class="colapser colapsa"></div>
      <div class="accion">
      <div class="accion">
      <table class="controles" cellspacing="0" cellpadding="1">
        <tr>
        <td class="bt-bottom  bt-left" style=" width:280px; background-color:White" >
        Registro Nuevo Vehículo [<input id="rbemision" onclick="fValidaEmision();" checked="true" runat="server" type="radio" name="deck" value="ci"/>]
            <%--<asp:RadioButton ID="rbNuevo" runat="server" Text="Registro Nuevo Vehículo" 
                Width="250px" />--%>
        </td>
        <td class="bt-bottom">
        Renovación Registro Vehículo [<input id="rbrenovacion" onclick="fValidaRenovacion();" runat="server"  type="radio" name="deck" value="pas"/>]
            <%--<asp:RadioButton ID="rbRenovacion" runat="server" 
                Text="Renovación Registro Vehículo" />--%>
        </td>
        <td class="bt-bottom bt-right validacion ">&nbsp;</td>
        </tr>
        <tr>
        <td class="bt-bottom  bt-right bt-left" style=" width:280px" >Categoría del Vehículo:</td>
        <td class="bt-bottom">
        <asp:DropDownList ID="ddlCategoria" runat="server" Width="205px"
             onchange="fCategoria();valdlcatveh(this, valcategoriavehiculo);" AutoPostBack="false"
                style="margin-top: 0px" 
                onselectedindexchanged="ddlCategoria_SelectedIndexChanged">
             <asp:ListItem Value="0">* Seleccione origen *</asp:ListItem>
        </asp:DropDownList>
        </td>
        <td class="bt-bottom bt-right validacion "><span id="valcategoriavehiculo" class="validacion"> 
            * obligatorio</span></td>
        </tr>
        <tr>
                            <td class="bt-bottom bt-right bt-left">
                                Área Destino/Actividad:
                            </td>
                            <td class="bt-bottom ">
                                <%--<asp:TextBox ID="txtarea" runat="server" Width="200px" MaxLength="30 " Style="text-align: center"
                                    onblur="checkcaja(this,'valnumfac',true);" onkeypress="return soloLetras(event,'abcdefghijklmnñopqrstuvwxyz01234567890 -_/',true)"></asp:TextBox>--%>
                                <asp:DropDownList runat="server" Width="300px" ID="ddlActividadOnlyControl" BackColor="Gray" Enabled="False"
                                    onchange="validadropdownlistveh(this, valareaoc);">
                                    <asp:ListItem Value="0">* Elija *</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td class="bt-bottom bt-right validacion ">
                                <%--<span id="valnumfac" class="validacion">* obligatorio</span>--%>
                                <span id='valareaoc' class="validacion" > </span>
                            </td>
                        </tr>
        <tr>
        <td class="bt-bottom  bt-right bt-left" style=" width:280px" >Placa / No. Serie (para Montacargas):</td>
        <td class="bt-bottom ">
        <asp:TextBox runat="server" id="txtPlaca" Width="200px" MaxLength="10" onkeypress="return soloLetras(event,'abcdefghijklnmñopqrstuvwxyz01234567890',true)" onblur="checkcaja(this,'valPlaca',true);"/>
        <%--<a  class="topopup" target="popup" onclick="fvalidaPlaca();"  ><i class="ico-find" ></i> Buscar </a>--%>
            <asp:Button Text="Buscar" Width="80px" ID="btnBuscar" runat="server"
                OnClientClick="return fvalidaPlaca();" onclick="btnBuscar_Click" />
        </td>
        <td class="bt-bottom bt-right validacion "><span id="valPlaca" class="validacion"> * obligatorio</span></td>
        </tr>
        <tr>
        <td class="bt-bottom bt-right bt-left" style=" width:280px" >Clase / Tipo:</td>
        <td class="bt-bottom"><asp:TextBox runat="server" id="txtClaseTipo" Width="200px" onkeypress="return soloLetras(event,'abcdefghijklnmñopqrstuvwxyz01234567890/-_ ',true)" onblur="checkcaja(this,'valClaseTipo',true);"/></td>
        <td class="bt-bottom bt-right validacion "><span id="valClaseTipo" class="validacion"> * obligatorio</span></td>
        </tr>
        <tr>
        <td class="bt-bottom  bt-right bt-left" style=" width:280px" >Marca:</td>
        <td class="bt-bottom"><asp:TextBox runat="server" id="txtMarca" MaxLength="25" Width="200px" onkeypress="return soloLetras(event,'abcdefghijklnmñopqrstuvwxyz01234567890/-_ ',true)" onblur="checkcaja(this,'valMarca',true);"/></td>
        <td class="bt-bottom bt-right validacion "><span id="valMarca" class="validacion"> * obligatorio</span></td>
        </tr>
        <tr>
        <td class="bt-bottom  bt-right bt-left" style=" width:280px" >Modelo:</td>
        <td class="bt-bottom"><asp:TextBox runat="server" id="txtModelo" Width="200px" MaxLength="25" onkeypress="return soloLetras(event,'abcdefghijklnmñopqrstuvwxyz01234567890/-_ ',true)" onblur="checkcaja(this,'valModelo',true);"/></td>
        <td class="bt-bottom bt-right validacion "><span id="valModelo" class="validacion"> * obligatorio</span></td>
        </tr>
        <tr>
        <td class="bt-bottom  bt-right bt-left" style=" width:280px" >Color:</td>
        <td class="bt-bottom"><asp:TextBox runat="server" id="txtColor" Width="200px" onkeypress="return soloLetras(event,'abcdefghijklnmñopqrstuvwxyz01234567890/-_ ',true)" onblur="checkcaja(this,'valColor',true);"/></td>
        <td class="bt-bottom bt-right validacion "><span id="valColor" class="validacion"> * obligatorio</span></td>
        </tr>
        
        <tr>
        <td class="bt-bottom  bt-right bt-left" style=" width:280px" >Tipo Certificado de Pesos y Medidas:</td>
        <td class="bt-bottom"><asp:TextBox runat="server" id="txttipocertificado" Width="200px" BackColor="Gray"
        onblur="checkcaja(this,'valtipcer',true);" Enabled="false"
        onkeypress="return soloLetras(event,'abcdefghijklnmñopqrstuvwxyz01234567890/-_ ',true)" MaxLength="10"/></td>
        <td class="bt-bottom bt-right validacion"><span id="valtipcer" class="validacion"></span></td>
        </tr>
        <tr>
        <td class="bt-bottom bt-right bt-left" style=" width:280px" >Nùmero de Certificado de Pesos y Medidas:</td>
        <td class="bt-bottom"><asp:TextBox runat="server" id="txtnumcertificado" Width="200px" MaxLength="10" BackColor="Gray"
        onblur="checkcaja(this,'valnumcer',true);" Enabled="false"
        onkeypress="return soloLetras(event,'0123456789',true)"></asp:TextBox></td>
        <td class="bt-bottom bt-right validacion"><span id="valnumcer" class="validacion"></span></td>
        </tr>
        <tr><td class="bt-bottom  bt-right bt-left" style=" width:280px">Fecha de Vigencia Permiso MTOP:</td>
         <td class="bt-bottom">
             <asp:TextBox  BackColor="Gray" 
             onblur="checkcaja(this,'valfecmtop',true);" Enabled="false"
             style="text-align: center; text-decoration: underline;"
             ID="txtfechamtop" runat="server" width="200px" MaxLength="10" CssClass="datetimepicker"
             onkeypress="return soloLetras(event,'0123456789/')" 
             ></asp:TextBox>
         </td>
         <td class="bt-bottom bt-right validacion"><span id="valfecmtop" class="validacion" </span></td>
         </tr>
        <tr><td class="bt-bottom  bt-right bt-left" style=" width:280px">Fecha de Vigencia de Póliza:</td>
         <td class="bt-bottom">
             <asp:TextBox  BackColor="Gray" Enabled="false"
             style="text-align: center" onblur="checkcaja(this,'valfecpol',true);"
             ID="txtfechapoliza" runat="server"  width="200px" MaxLength="10" CssClass="datetimepicker"
             onkeypress="return soloLetras(event,'0123456789/')" 
             ></asp:TextBox>
         </td>
         <td class="bt-bottom bt-right validacion"><span id="valfecpol" class="validacion" ></span></td>
        </tr>
        <tr><td class="bt-bottom  bt-right bt-left" style=" width:155px">Nota:</td>
         <td class="bt-bottom">
                 <asp:TextBox ID="txtNota" runat="server" MaxLength="3000" TextMode="MultiLine" Width="300px" style="overflow:auto;resize:none"
                              onkeypress="return soloLetras(event,'abcdefghijklnmñopqrstuvwxyz01234567890 /_-.',true)"/>
         </td>
         <td class="bt-bottom bt-right"><span style="font-size:small; font-family:Consolas; font-weight:normal; font-style:italic; color:Green" id="Span1"> * opcional</span></td>
         </tr>
        <tr>
      <td class="bt-left" style=" width:280px; background-color:White"></td>
      <td>
      <div>
      <asp:Button ID="btnAgregar" runat="server" Width="100px" Text="Agregar" OnClientClick="return validaCabecera();"
                onclick="btnAgregar_Click" />
       <img alt="loading.." src="../shared/imgs/loader.gif" id="loader" class="nover"/>
       </div>
        </td>
       <td class="bt-right"></td>
       </tr>
        </table>
      </div>
            <asp:UpdatePanel ID="upresult2" runat="server"  >
      <ContentTemplate>
      <script type="text/javascript">          Sys.Application.add_load(BindFunctions);</script>
      <div class="informativo" id="colector">
      <table runat="server" id="tableexpo">
      <tr><td>
              <asp:GridView runat="server" id="gvVehiculos" class="tabRepeat" AutoGenerateColumns="False" Width="100%"
                        onrowcommand="gvVehiculos_RowCommand" onrowdeleting="gvVehiculos_RowDeleting">
              <Columns>
              <asp:TemplateField HeaderText="IdTipoEmpresa" Visible="False">
                      <ItemTemplate>
                          <%--<asp:TextBox ID="tplaca" runat="server" Width="70px" Text='<%# Eval("Placa") %>' onkeypress="return soloLetras(event,'abcdefghijklnmñopqrstuvwxyz01234567890',true)"></asp:TextBox>--%>
                          <asp:Label ID="tidtipemp"  style="text-transform :uppercase"  Width="70px" runat="server" Text='<%# Eval("IdTipoEmpresa") %>' onkeypress="return soloLetras(event,'abcdefghijklnmñopqrstuvwxyz01234567890',true)"></asp:Label>
                      </ItemTemplate>
                  </asp:TemplateField>
                  <asp:TemplateField HeaderText="IdCategoria" Visible="False">
                      <ItemTemplate>
                          <%--<asp:TextBox ID="tplaca" runat="server" Width="70px" Text='<%# Eval("Placa") %>' onkeypress="return soloLetras(event,'abcdefghijklnmñopqrstuvwxyz01234567890',true)"></asp:TextBox>--%>
                          <asp:Label ID="tidcategoria"  style="text-transform :uppercase"  Width="70px" runat="server" Text='<%# Eval("IdCategoria") %>' onkeypress="return soloLetras(event,'abcdefghijklnmñopqrstuvwxyz01234567890',true)"></asp:Label>
                      </ItemTemplate>
                  </asp:TemplateField>
                 <asp:TemplateField HeaderText="Placa">
                      <ItemTemplate>
                          <%--<asp:TextBox ID="tplaca" runat="server" Width="70px" Text='<%# Eval("Placa") %>' onkeypress="return soloLetras(event,'abcdefghijklnmñopqrstuvwxyz01234567890',true)"></asp:TextBox>--%>
                          <asp:Label ID="tplaca"  style="text-transform :uppercase" Width="50px" ToolTip='<%# Eval("Placa") %>' runat="server" Text='<%# Eval("Placa") %>' onkeypress="return soloLetras(event,'abcdefghijklnmñopqrstuvwxyz01234567890',true)"></asp:Label>
                      </ItemTemplate>
                  </asp:TemplateField>
                  <asp:TemplateField HeaderText="Categoría">
                      <ItemTemplate>
                          <%--<asp:TextBox ID="tplaca" runat="server" Width="70px" Text='<%# Eval("Placa") %>' onkeypress="return soloLetras(event,'abcdefghijklnmñopqrstuvwxyz01234567890',true)"></asp:TextBox>--%>
                          <asp:Label ID="tcategoria" ToolTip='<%# Eval("DesCategoria") %>'  style="text-transform :uppercase"  Width="60px" runat="server" Text='<%# Eval("DesCategoria") %>' onkeypress="return soloLetras(event,'abcdefghijklnmñopqrstuvwxyz01234567890',true)"></asp:Label>
                      </ItemTemplate>
                  </asp:TemplateField>
                  <asp:TemplateField HeaderText="Area">
                      <ItemTemplate>
                          <%--<asp:TextBox ID="tplaca" runat="server" Width="70px" Text='<%# Eval("Placa") %>' onkeypress="return soloLetras(event,'abcdefghijklnmñopqrstuvwxyz01234567890',true)"></asp:TextBox>--%>
                          <asp:Label ID="tareaactv" ToolTip='<%# Eval("Area") %>'  style="text-transform :uppercase"  Width="60px" runat="server" Text='<%# Eval("Area") %>' onkeypress="return soloLetras(event,'abcdefghijklnmñopqrstuvwxyz01234567890',true)"></asp:Label>
                      </ItemTemplate>
                  </asp:TemplateField>
                  <asp:TemplateField HeaderText="Clase/Tipo">
                      <ItemTemplate>
                          <asp:Label ID="tclasetipo" ToolTip='<%# Eval("ClaseTipo") %>'  runat="server" Width="60px" Text='<%# Eval("[ClaseTipo]") %>' 
                          onkeypress="return soloLetras(event,'abcdefghijklnmñopqrstuvwxyz01234567890 -/_.',true)"></asp:Label>
                      </ItemTemplate>
                  </asp:TemplateField>
                  <asp:TemplateField HeaderText="Marca">
                      <ItemTemplate>
                          <asp:Label ID="tmarca" runat="server"  ToolTip='<%# Eval("Marca") %>' Width="60px" Text='<%# Eval("[Marca]") %>' onkeypress="return soloLetras(event,'abcdefghijklnmñopqrstuvwxyz01234567890 -/_.',true)"></asp:Label>
                      </ItemTemplate>
                  </asp:TemplateField>
                  <asp:TemplateField HeaderText="Modelo">
                      <ItemTemplate>
                          <asp:Label ID="tmodelo" runat="server"  ToolTip='<%# Eval("Modelo") %>' Width="60px" Text='<%# Eval("[Modelo]") %>' onkeypress="return soloLetras(event,'abcdefghijklnmñopqrstuvwxyz01234567890 -/_.',true)"></asp:Label>
                      </ItemTemplate>
                  </asp:TemplateField>
                  <asp:TemplateField HeaderText="Nota">
                      <ItemTemplate>
                      <div style="overflow-y:scroll; overflow-x: hidden; height:30px; text-align:center">
                          <asp:Label ID="tnota"  style="text-transform :uppercase" runat="server" ToolTip='<%# Eval("[Nota]") %>' Text='<%# Eval("[Nota]") %>'></asp:Label>
                      </div>
                      </ItemTemplate>
                  </asp:TemplateField>
                  <asp:TemplateField HeaderText="Color" Visible="False">
                      <ItemTemplate>
                          <asp:Label ID="tcolor" runat="server" Width="70px" Text='<%# Eval("[Color]") %>' onkeypress="return soloLetras(event,'abcdefghijklnmñopqrstuvwxyz01234567890',true)"></asp:Label>
                      </ItemTemplate>
                  </asp:TemplateField>
                  <asp:TemplateField HeaderText="NO Cargados" Visible="false">
                      <ItemTemplate>
                          <%--<asp:TextBox ID="tplaca" runat="server" Width="70px" Text='<%# Eval("Placa") %>' onkeypress="return soloLetras(event,'abcdefghijklnmñopqrstuvwxyz01234567890',true)"></asp:TextBox>--%>
                          <asp:CheckBox ID="chkEstadoDocumentos" runat="server" Enabled="false"/>
                      </ItemTemplate>
                  </asp:TemplateField>
                  <asp:TemplateField ShowHeader="False" HeaderText="Adjuntar">
                        <ItemTemplate>
                            <a style=" width:80px" id="adjDoc" class="topopup" onclick="redirectcatdoc('<%# Eval("Placa") %>', '<%# Eval("IdTipoEmpresa") %>', '<%# Eval("IdCategoria") %>');" >
                            <i class="ico-find" ></i> Documentos </a>
                             <%--<asp:Button ID="btnAdjDoc" runat="server" CausesValidation="False" Width="125px"
                                OnClientClick='redirectcatdoc(tplaca);'
                                CommandName="Add" Text="Adjuntar Documentos" />--%>
                        </ItemTemplate>
                  </asp:TemplateField>
                  <%--<asp:TemplateField HeaderText="TipoCertificado" Visible="false">
                      <ItemTemplate>
                          <asp:TextBox ID="ttipocertificado" runat="server" Width="70px" Text='<%# Eval("[TipoCertificado]") %>' onkeypress="return soloLetras(event,'abcdefghijklnmñopqrstuvwxyz01234567890',true)"></asp:TextBox>
                      </ItemTemplate>
                  </asp:TemplateField>
                  <asp:TemplateField HeaderText="Certificado" Visible="false">
                      <ItemTemplate>
                          <asp:TextBox ID="tcertificado" runat="server" Width="70px" Text='<%# Eval("[Certificado]") %>' onkeypress="return soloLetras(event,'abcdefghijklnmñopqrstuvwxyz01234567890',true)"></asp:TextBox>
                      </ItemTemplate>
                  </asp:TemplateField>
                  <asp:TemplateField HeaderText="Categoria" Visible="false">
                      <ItemTemplate>
                          <asp:TextBox ID="tcategoria" runat="server" Width="70px" Text='<%# Eval("[Categoria]") %>' onkeypress="return soloLetras(event,'abcdefghijklnmñopqrstuvwxyz01234567890',true)"></asp:TextBox>
                      </ItemTemplate>
                  </asp:TemplateField>
                  <asp:TemplateField HeaderText="FechaPoliza" Visible="false">
                      <ItemTemplate>
                          <asp:TextBox ID="tfechapoliza" runat="server" Width="70px" Text='<%# Eval("[FechaPoliza]") %>' onkeypress="return soloLetras(event,'abcdefghijklnmñopqrstuvwxyz01234567890',true)"></asp:TextBox>
                      </ItemTemplate>
                  </asp:TemplateField>
                  <asp:TemplateField HeaderText="FechaMtop" Visible="false">
                      <ItemTemplate>
                          <asp:TextBox ID="tfechapoliza" runat="server" Width="70px" Text='<%# Eval("[FechaMtop]") %>' onkeypress="return soloLetras(event,'abcdefghijklnmñopqrstuvwxyz01234567890',true)"></asp:TextBox>
                      </ItemTemplate>
                  </asp:TemplateField>--%>
                  <asp:TemplateField ShowHeader="False" Visible="false">
                        <ItemTemplate>
                            <asp:Button ID="btnAdd" runat="server" CausesValidation="False" Width="60px" CommandArgument="Add"
                                CommandName="Add" Text="Agregar" />
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
        <asp:AsyncPostBackTrigger ControlID="hfupdatepanel" />
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
   <asp:HiddenField runat="server" ID="hfupdatepanel" />
   <asp:HiddenField runat="server" ID="hfcategoria" />
   <asp:HiddenField runat="server" ID="categoriaveh" />
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
        function validadropdownlistveh(control, validador) {
            if (control.value != 0) {
                control.style.cssText = "background-color:none;color:none;width:300px;"
                validador.innerHTML = '';
                return;
            }
            control.style.cssText = "background-color:White;color:Red;width:350px;";
            validador.innerHTML = '<span class="obligado">OBLIGATORIO!</span>';
            return;
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
        function validaCabecera() {
            try {
                var vals = document.getElementById('<%=ddlTipoEmpresa.ClientID %>').value;
                if (vals == 0) {
                    alert('Seleccione al menos un Tipo de Empresa.');
                    document.getElementById('<%=ddlTipoEmpresa.ClientID %>').focus();
                    document.getElementById('<%=ddlTipoEmpresa.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:400px;";
                    document.getElementById("loader").className = 'nover';
                    return false;
                }
                var vals = document.getElementById('<%=cbltiposolicitud.ClientID %>').value;
                if (vals == 0) {
                    alert('Seleccione al menos un Tipo de Solicitud.');
                    document.getElementById('<%=cbltiposolicitud.ClientID %>').focus();
                    document.getElementById('<%=cbltiposolicitud.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:400px;";
                    document.getElementById("loader").className = 'nover';
                    return false;
                }
                var emision = document.getElementById('<%=rbemision.ClientID %>').checked;
                var renovacion = document.getElementById('<%=rbrenovacion.ClientID %>').checked;
                if (emision == false && renovacion == false) {
                    alert('Indique si es un Registro Nuevo Vehículo o Renovación Registro Vehículo.');
                    document.getElementById("loader").className = 'nover';
                    return false;
                }
                var vals = document.getElementById('<%=txtPlaca.ClientID %>').value;
                if (vals == '' || vals == null || vals == undefined) {
                    alert('Escriba la Placa o No. Serie (para Montacargas).');
                    document.getElementById('<%=txtPlaca.ClientID %>').focus();
                    document.getElementById('<%=txtPlaca.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:200px;";
                    document.getElementById("loader").className = 'nover';
                    return false;
                }
                var vals = document.getElementById('<%=txtMarca.ClientID %>').value;
                if (vals == '' || vals == null || vals == undefined) {
                    alert('Escriba la Marca del vehiculo.');
                    document.getElementById('<%=txtMarca.ClientID %>').focus();
                    document.getElementById('<%=txtMarca.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:200px;";
                    document.getElementById("loader").className = 'nover';
                    return false;
                }
                var vals = document.getElementById('<%=txtColor.ClientID %>').value;
                if (vals == '' || vals == null || vals == undefined) {
                    alert('Escriba el Color del vehiculo.');
                    document.getElementById('<%=txtColor.ClientID %>').focus();
                    document.getElementById('<%=txtColor.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:200px;";
                    document.getElementById("loader").className = 'nover';
                    return false;
                }
                var vals = document.getElementById('<%=ddlCategoria.ClientID %>').value;
                if (vals == 0) {
                    alert('Seleccione la Categoria del Vehiculo.');
                    document.getElementById('<%=ddlCategoria.ClientID %>').focus();
                    document.getElementById('<%=ddlCategoria.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:200px;";
                    document.getElementById("loader").className = 'nover';
                    return false;
                }
                if (vals == 'CAT') {
                    var vals = document.getElementById('<%=txttipocertificado.ClientID %>').value;
                    if (vals == '' || vals == null || vals == undefined) {
                        alert('Escriba el Tipo de Certificado.');
                        document.getElementById('<%=txttipocertificado.ClientID %>').focus();
                        document.getElementById('<%=txttipocertificado.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:200px;";
                        document.getElementById("loader").className = 'nover';
                        return false;
                    }
                    var vals = document.getElementById('<%=txtnumcertificado.ClientID %>').value;
                    if (vals == '' || vals == null || vals == undefined) {
                        alert('Escriba el Nùmero de Certificado.');
                        document.getElementById('<%=txtnumcertificado.ClientID %>').focus();
                        document.getElementById('<%=txtnumcertificado.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:200px;";
                        document.getElementById("loader").className = 'nover';
                        return false;
                    }
                    vals = document.getElementById('<%=txtfechapoliza.ClientID %>').value;
                    if (/*!validarFecha(vals)*/vals == '' || vals == null || vals == undefined) {
                        alert('Seleccione la Fecha de Poliza.');
                        document.getElementById('<%=txtfechapoliza.ClientID %>').focus();
                        document.getElementById('<%=txtfechapoliza.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:200px;";
                        document.getElementById("loader").className = 'nover';
                        return false;
                    };
                    vals = document.getElementById('<%=txtfechamtop.ClientID %>').value;
                    if (/*!validarFecha(vals)*/vals == '' || vals == null || vals == undefined) {
                        alert('Seleccione la Fecha de MTOP.');
                        document.getElementById('<%=txtfechamtop.ClientID %>').focus();
                        document.getElementById('<%=txtfechamtop.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:200px;";
                        document.getElementById("loader").className = 'nover';
                        return false;
                    };
                }
                else if (vals == 'MON') {
                    var vals = document.getElementById('<%=txtPlaca.ClientID %>').value;
                    if (vals == '' || vals == null || vals == undefined) {
                        alert('Escriba la Placa o No. Serie (para Montacargas).');
                        document.getElementById('<%=txtPlaca.ClientID %>').focus();
                        document.getElementById('<%=txtPlaca.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:200px;";
                        document.getElementById("loader").className = 'nover';
                        return false;
                    }
                    var vals = document.getElementById('<%=txtMarca.ClientID %>').value;
                    if (vals == '' || vals == null || vals == undefined) {
                        alert('Escriba la Marca del vehiculo.');
                        document.getElementById('<%=txtMarca.ClientID %>').focus();
                        document.getElementById('<%=txtMarca.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:200px;";
                        document.getElementById("loader").className = 'nover';
                        return false;
                    }
                    var vals = document.getElementById('<%=txtColor.ClientID %>').value;
                    if (vals == '' || vals == null || vals == undefined) {
                        alert('Escriba el Color del vehiculo.');
                        document.getElementById('<%=txtColor.ClientID %>').focus();
                        document.getElementById('<%=txtColor.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:200px;";
                        document.getElementById("loader").className = 'nover';
                        return false;
                    }
                }
                else {
                    var vals = document.getElementById('<%=txtClaseTipo.ClientID %>').value;
                    if (vals == '' || vals == null || vals == undefined) {
                        alert('Escriba la Clase/Tipo del vehiculo.');
                        document.getElementById('<%=txtClaseTipo.ClientID %>').focus();
                        document.getElementById('<%=txtClaseTipo.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:200px;";
                        document.getElementById("loader").className = 'nover';
                        return false;
                    }
                    var vals = document.getElementById('<%=txtModelo.ClientID %>').value;
                    if (vals == '' || vals == null || vals == undefined) {
                        alert('Escriba el Modelo del vehiculo.');
                        document.getElementById('<%=txtModelo.ClientID %>').focus();
                        document.getElementById('<%=txtModelo.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:200px;";
                        document.getElementById("loader").className = 'nover';
                        return false;
                    }
                    var vals = document.getElementById('<%=ddlActividadOnlyControl.ClientID %>').value;
                    if (vals == 0) {
                        alert('Seleccione el Área Destino/Actividad.');
                        document.getElementById('<%=ddlActividadOnlyControl.ClientID %>').focus();
                        document.getElementById('<%=ddlActividadOnlyControl.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:200px;";
                        document.getElementById("loader").className = 'nover';
                        return false;
                    }
                }
                if (document.getElementById('<%=ddlCategoria.ClientID %>').value == 'CAT') {
                    document.getElementById('<%=txttipocertificado.ClientID %>').disabled = false;
                    document.getElementById('<%=txtnumcertificado.ClientID %>').disabled = false;
                    document.getElementById('<%=txtfechamtop.ClientID %>').disabled = false;
                    document.getElementById('<%=txtfechapoliza.ClientID %>').disabled = false;
                }
                document.getElementById('<%=ddlCategoria.ClientID %>').disabled = false;
                document.getElementById('<%=txtMarca.ClientID %>').disabled = false;
                document.getElementById('<%=txtClaseTipo.ClientID %>').disabled = false;
                document.getElementById('<%=txtModelo.ClientID %>').disabled = false;
                document.getElementById('<%=txtColor.ClientID %>').disabled = false;
                document.getElementById("loader").className = '';
                return true;
            } catch (e) {
                alert('Lamentamos comunicarle que ha ocurrido el siguiente problema:\n' + e.Message);
            }
        }
        function popupCallback(objeto, catalogo) {
            if (objeto == null || objeto == undefined) {
                alert('Hubo un problema al setaar un objeto de catalogo');
                return;
            }
            document.getElementById('<%=txtPlaca.ClientID %>').value = objeto.placa;
            document.getElementById('<%=txtMarca.ClientID %>').value = objeto.marca;
            document.getElementById('<%=txtClaseTipo.ClientID %>').value = objeto.clasetipo;
            document.getElementById('<%=txtModelo.ClientID %>').value = objeto.modelo;
            document.getElementById('<%=txtColor.ClientID %>').value = objeto.color;
            document.getElementById('<%=txttipocertificado.ClientID %>').value = objeto.tipocertificado;
            document.getElementById('<%=txtnumcertificado.ClientID %>').value = objeto.certificado;
            document.getElementById('<%=txtfechamtop.ClientID %>').value = objeto.fechamtop.substr(0, 10);
            document.getElementById('<%=txtfechapoliza.ClientID %>').value = objeto.fechapoliza.substr(0, 10);
            document.getElementById('<%=ddlCategoria.ClientID %>').value = objeto.tipocategoria;
            document.getElementById('<%=ddlCategoria.ClientID %>').style.cssText = "background-color:White;width:200px;";
            document.getElementById('<%=txtMarca.ClientID %>').style.cssText = "background-color:White;width:200px;";
            document.getElementById('<%=txtClaseTipo.ClientID %>').style.cssText = "background-color:White;width:200px;";
            document.getElementById('<%=txtModelo.ClientID %>').style.cssText = "background-color:White;width:200px;";
            document.getElementById('<%=txtColor.ClientID %>').style.cssText = "background-color:White;width:200px;";
            document.getElementById('<%=ddlActividadOnlyControl.ClientID %>').value = "0";
            if (objeto.tipocategoria == 'LIV') {
                document.getElementById('<%=txttipocertificado.ClientID %>').style.cssText = "background-color:Gray;width:200px;";
                document.getElementById('<%=txtnumcertificado.ClientID %>').style.cssText = "background-color:Gray;width:200px;";
                document.getElementById('<%=txtfechamtop.ClientID %>').style.cssText = "background-color:Gray;width:200px;";
                document.getElementById('<%=txtfechapoliza.ClientID %>').style.cssText = "background-color:Gray;width:200px;";
                document.getElementById('<%=hfcategoria.ClientID %>').value = objeto.tipocategoria;
                document.getElementById('<%=ddlActividadOnlyControl.ClientID %>').disabled = false;
                document.getElementById('<%=ddlActividadOnlyControl.ClientID %>').style.cssText = "background-color:White;width:300px;";
                document.getElementById('<%=txttipocertificado.ClientID %>').value   = "";
                document.getElementById('<%=txtnumcertificado.ClientID %>').value = "";
                document.getElementById('<%=txtfechamtop.ClientID %>').value = "";
                document.getElementById('<%=txtfechapoliza.ClientID %>').value = "";
                document.getElementById('<%=txttipocertificado.ClientID %>').disabled = true;
                document.getElementById('<%=txtnumcertificado.ClientID %>').disabled = true;
                document.getElementById('<%=txtfechamtop.ClientID %>').disabled = true;
                document.getElementById('<%=txtfechapoliza.ClientID %>').disabled = true;
            }
            if (objeto.tipocategoria == 'CAT' || objeto.tipocategoria == 'MON') {
                document.getElementById('<%=txttipocertificado.ClientID %>').style.cssText = "background-color:White;width:200px;";
                document.getElementById('<%=txtnumcertificado.ClientID %>').style.cssText = "background-color:White;width:200px;";
                document.getElementById('<%=txtfechamtop.ClientID %>').style.cssText = "background-color:White;width:200px;";
                document.getElementById('<%=txtfechapoliza.ClientID %>').style.cssText = "background-color:White;width:200px;";
                document.getElementById('<%=hfcategoria.ClientID %>').value = objeto.tipocategoria;
                document.getElementById('<%=ddlActividadOnlyControl.ClientID %>').disabled = true;
                document.getElementById('<%=ddlActividadOnlyControl.ClientID %>').style.cssText = "background-color:Gray;width:300px;";
                document.getElementById('<%=txttipocertificado.ClientID %>').disabled = false;
                document.getElementById('<%=txtnumcertificado.ClientID %>').disabled = false;
                document.getElementById('<%=txtfechamtop.ClientID %>').disabled = false;
                document.getElementById('<%=txtfechapoliza.ClientID %>').disabled = false;
            }
            document.getElementById('valcategoriavehiculo').textContent = "";
            document.getElementById('valClaseTipo').textContent = "";
            document.getElementById('valMarca').textContent = "";
            document.getElementById('valModelo').textContent = "";
            document.getElementById('valColor').textContent = "";
            document.getElementById('valfecmtop').textContent = "";
            document.getElementById('valfecpol').textContent = "";
            document.getElementById('valtipcer').textContent = "";
            document.getElementById('valnumcer').textContent = "";
            document.getElementById('<%=txtNota.ClientID %>').disabled = false;
            document.getElementById('<%=txtNota.ClientID %>').style.cssText = "background-color:White;width:300px;";
        }
        function redireccionar() {
            window.locationf = "~/cuenta/zones.aspx";
        }
        function redirectcatdoc(val, val2, val3) {
            var caja = val;
            var tipoempresa = val2;
            var categoria = val3;
            //window.open('../credenciales/documentos/?placa=' + caja, 'name', 'width=850,height=480')
            window.open('../cliente/solicitudvehiculodocumentos.aspx?PLACA=' + caja + '&CATEGORIA=' + categoria + '&SID=' + tipoempresa, 'name', 'width=950,height=550')
        }
        function isValidDate(day, month, year) {
            var dteDate;
            // En javascript, el mes empieza en la posicion 0 y termina en la 11 
            //   siendo 0 el mes de enero
            // Por esta razon, tenemos que restar 1 al mes
            month = month - 1;
            // Establecemos un objeto Data con los valore recibidos
            // Los parametros son: año, mes, dia, hora, minuto y segundos
            // getDate(); devuelve el dia como un entero entre 1 y 31
            // getDay(); devuelve un num del 0 al 6 indicando siel dia es lunes,
            //   martes, miercoles ...
            // getHours(); Devuelve la hora
            // getMinutes(); Devuelve los minutos
            // getMonth(); devuelve el mes como un numero de 0 a 11
            // getTime(); Devuelve el tiempo transcurrido en milisegundos desde el 1
            //   de enero de 1970 hasta el momento definido en el objeto date
            // setTime(); Establece una fecha pasandole en milisegundos el valor de esta.
            // getYear(); devuelve el año
            // getFullYear(); devuelve el año
            dteDate = new Date(year, month, day);
            alert(dteDate);
            return ((day == dteDate.getDate()) && (month == dteDate.getMonth()) && (year == dteDate.getFullYear()));
        }
        function validate_fecha(fecha) {
            var patron = new RegExp("^(19|20)+([0-9]{2})([-])([0-9]{1,2})([-])([0-9]{1,2})$");
            if (fecha.search(patron) == 0) {
                var values = fecha.split("-");
                if (isValidDate(values[2], values[1], values[0])) {
                    return true;
                }
            }
            return false;
        }
        function validarFecha(fecha) {
            if (validate_fecha(fecha) == false) {
                var val = "El formato de fecha " + fecha + " es incorrecta, debe ser dia/mes/anio";
                alert(val);
                return false;
            }
            return true;
        }
        function fCategoria() {
            var vals = document.getElementById('<%=ddlCategoria.ClientID %>').value;
            document.getElementById('<%=categoriaveh.ClientID %>').value = vals;
            if (vals == 'CAT') {
                document.getElementById('<%=txttipocertificado.ClientID %>').disabled = false;
                document.getElementById('<%=txtnumcertificado.ClientID %>').disabled = false;
                document.getElementById('<%=txtfechamtop.ClientID %>').disabled = false;
                document.getElementById('<%=txtfechapoliza.ClientID %>').disabled = false;
                document.getElementById('<%=ddlActividadOnlyControl.ClientID %>').disabled = true;
                document.getElementById('<%=txttipocertificado.ClientID %>').style.cssText = "background-color:White;width:200px;";
                document.getElementById('<%=txtnumcertificado.ClientID %>').style.cssText = "background-color:White;width:200px;";
                document.getElementById('<%=txtfechamtop.ClientID %>').style.cssText = "background-color:White;width:200px;";
                document.getElementById('<%=txtfechapoliza.ClientID %>').style.cssText = "background-color:White;width:200px;";
                document.getElementById('<%=ddlActividadOnlyControl.ClientID %>').style.cssText = "background-color:Gray;width:300px;";
                document.getElementById('<%=txtClaseTipo.ClientID %>').disabled = false;
                document.getElementById('<%=txtModelo.ClientID %>').disabled = false;
                document.getElementById('<%=txtClaseTipo.ClientID %>').style.cssText = "background-color:White;width:200px;";
                document.getElementById('<%=txtModelo.ClientID %>').style.cssText = "background-color:White;width:200px;";        
                document.getElementById('valfecmtop').textContent = " * obligatorio";
                document.getElementById('valfecpol').textContent = " * obligatorio";
                document.getElementById('valtipcer').textContent = " * obligatorio";
                document.getElementById('valnumcer').textContent = " * obligatorio";
                document.getElementById('valClaseTipo').textContent = " * obligatorio";
                document.getElementById('valModelo').textContent = " * obligatorio";
                document.getElementById('valareaoc').textContent = "";
                document.getElementById('<%=ddlActividadOnlyControl.ClientID %>').value = "0";
            }
            else if (vals == 'MON') {
                document.getElementById('<%=txttipocertificado.ClientID %>').disabled = false;
                document.getElementById('<%=txtnumcertificado.ClientID %>').disabled = false;
                document.getElementById('<%=txtfechamtop.ClientID %>').disabled = false;
                document.getElementById('<%=txtfechapoliza.ClientID %>').disabled = false;
                document.getElementById('<%=ddlActividadOnlyControl.ClientID %>').disabled = true;
                document.getElementById('<%=txtClaseTipo.ClientID %>').disabled = true;
                document.getElementById('<%=txtModelo.ClientID %>').disabled = true;
                document.getElementById('<%=txttipocertificado.ClientID %>').disabled = true;
                document.getElementById('<%=txtnumcertificado.ClientID %>').disabled = true;
                document.getElementById('<%=txtfechamtop.ClientID %>').disabled = true;
                document.getElementById('<%=txtfechapoliza.ClientID %>').disabled = true;
                document.getElementById('<%=txtClaseTipo.ClientID %>').style.cssText = "background-color:Gray;width:200px;";
                document.getElementById('<%=txtModelo.ClientID %>').style.cssText = "background-color:Gray;width:200px;";
                document.getElementById('<%=txttipocertificado.ClientID %>').style.cssText = "background-color:Gray;width:200px;";
                document.getElementById('<%=txtnumcertificado.ClientID %>').style.cssText = "background-color:Gray;width:200px;";
                document.getElementById('<%=txtfechamtop.ClientID %>').style.cssText = "background-color:Gray;width:200px;";
                document.getElementById('<%=txtfechapoliza.ClientID %>').style.cssText = "background-color:Gray;width:200px;";
                document.getElementById('<%=ddlActividadOnlyControl.ClientID %>').style.cssText = "background-color:Gray;width:300px;";
                document.getElementById('valfecmtop').textContent = "";
                document.getElementById('valfecpol').textContent = "";
                document.getElementById('valtipcer').textContent = "";
                document.getElementById('valnumcer').textContent = "";
                document.getElementById('valClaseTipo').textContent = "";
                document.getElementById('valModelo').textContent = "";
                document.getElementById('valareaoc').textContent = "";
                document.getElementById('<%=txtClaseTipo.ClientID %>').value = "";
                document.getElementById('<%=txtModelo.ClientID %>').value = "";
                document.getElementById('<%=ddlActividadOnlyControl.ClientID %>').value = "0";
            }
            else if (vals == 'LIV') {
//                var vals = document.getElementById('<%=cbltiposolicitud.ClientID %>').value;
//                if (vals == 0) {
//                    alert('* Seleccione un Tipo de Solicitud *');
//                    document.getElementById('<%=cbltiposolicitud.ClientID %>').focus();
//                    document.getElementById('<%=cbltiposolicitud.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:400px;";
//                    document.getElementById('<%=ddlCategoria.ClientID %>').value = 0;
//                    return false;
//                }
                document.getElementById('<%=txttipocertificado.ClientID %>').disabled = true;
                document.getElementById('<%=txtnumcertificado.ClientID %>').disabled = true;
                document.getElementById('<%=txtfechamtop.ClientID %>').disabled = true;
                document.getElementById('<%=txtfechapoliza.ClientID %>').disabled = true;
                document.getElementById('<%=ddlActividadOnlyControl.ClientID %>').disabled = false;
                document.getElementById('<%=txttipocertificado.ClientID %>').style.cssText = "background-color:Gray;width:200px;";
                document.getElementById('<%=txtnumcertificado.ClientID %>').style.cssText = "background-color:Gray;width:200px;";
                document.getElementById('<%=txtfechamtop.ClientID %>').style.cssText = "background-color:Gray;width:200px;";
                document.getElementById('<%=txtfechapoliza.ClientID %>').style.cssText = "background-color:Gray;width:200px;";
                document.getElementById('<%=ddlActividadOnlyControl.ClientID %>').style.cssText = "background-color:White;width:300px;";
                document.getElementById('<%=txtClaseTipo.ClientID %>').disabled = false;
                document.getElementById('<%=txtModelo.ClientID %>').disabled = false;
                document.getElementById('<%=txtClaseTipo.ClientID %>').style.cssText = "background-color:White;width:200px;";
                document.getElementById('<%=txtModelo.ClientID %>').style.cssText = "background-color:White;width:200px;";  
                document.getElementById('valfecmtop').textContent = "";
                document.getElementById('valfecpol').textContent = "";
                document.getElementById('valtipcer').textContent = "";
                document.getElementById('valnumcer').textContent = "";
                document.getElementById('valClaseTipo').textContent = " * obligatorio";
                document.getElementById('valModelo').textContent = " * obligatorio";
                document.getElementById('valareaoc').textContent = " * obligatorio";
            }
        }
        function fvalidaPlaca() {
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
                var vals = document.getElementById('<%=txtPlaca.ClientID %>').value;
                if (vals == '' || vals == null || vals == undefined) {
                    alert('Escriba la Placa o No. Serie (para Montacargas).');
                    document.getElementById('<%=txtPlaca.ClientID %>').focus();
                    document.getElementById('<%=txtPlaca.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:200px;";
                    document.getElementById("loader").className = 'nover';
                    return false;
                }
                window.open('../catalogo/listado-de-placas' + '?sidpons=' + vals, 'name', 'width=850,height=480');
                return false;
            }
            else {
                alert('Solo puede buscar cuando es una Renovación Registro Vehículo.');
                document.getElementById('<%=txtPlaca.ClientID %>').focus();
                document.getElementById('<%=txtPlaca.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:200px;";
                document.getElementById("loader").className = 'nover';
                return false;
            }
        }
        function fValidaRenovacion() {
            var renovacion = document.getElementById('<%=rbrenovacion.ClientID %>').checked;
            if (renovacion) {
//                document.getElementById('<%=btnBuscar.ClientID %>').disabled = false;
                document.getElementById('<%=ddlCategoria.ClientID %>').disabled = true;
                document.getElementById('<%=ddlActividadOnlyControl.ClientID %>').disabled = true;
                document.getElementById('<%=txtMarca.ClientID %>').disabled = true;
                document.getElementById('<%=txtClaseTipo.ClientID %>').disabled = true;
                document.getElementById('<%=txtPlaca.ClientID %>').focus();
                document.getElementById('<%=txtModelo.ClientID %>').disabled = true;
                document.getElementById('<%=txtColor.ClientID %>').disabled = true;
                document.getElementById('<%=txttipocertificado.ClientID %>').disabled = true;
                document.getElementById('<%=txtnumcertificado.ClientID %>').disabled = true;
                document.getElementById('<%=txtfechamtop.ClientID %>').disabled = true;
                document.getElementById('<%=txtfechapoliza.ClientID %>').disabled = true;
                document.getElementById('<%=ddlCategoria.ClientID %>').style.cssText = "background-color:Gray;width:200px;";
                document.getElementById('<%=ddlActividadOnlyControl.ClientID %>').style.cssText = "background-color:Gray;width:300px;";
                document.getElementById('<%=txtMarca.ClientID %>').style.cssText = "background-color:Gray;width:200px;";
                document.getElementById('<%=txtClaseTipo.ClientID %>').style.cssText = "background-color:Gray;width:200px;";
                document.getElementById('<%=txtModelo.ClientID %>').style.cssText = "background-color:Gray;width:200px;";
                document.getElementById('<%=txtColor.ClientID %>').style.cssText = "background-color:Gray;width:200px;";
                document.getElementById('<%=txttipocertificado.ClientID %>').style.cssText = "background-color:Gray;width:200px;";
                document.getElementById('<%=txtnumcertificado.ClientID %>').style.cssText = "background-color:Gray;width:200px;";
                document.getElementById('<%=txtfechamtop.ClientID %>').style.cssText = "background-color:Gray;width:200px;";
                document.getElementById('<%=txtfechapoliza.ClientID %>').style.cssText = "background-color:Gray;width:200px;";
                document.getElementById('<%=txtNota.ClientID %>').disabled = true;
                document.getElementById('<%=txtNota.ClientID %>').style.cssText = "background-color:Gray;width:300px;";
                document.getElementById('valcategoriavehiculo').textContent = "";
                document.getElementById('valClaseTipo').textContent = "";
                document.getElementById('valMarca').textContent = "";
                document.getElementById('valModelo').textContent = "";
                document.getElementById('valColor').textContent = "";
                document.getElementById('valfecmtop').textContent = "";
                document.getElementById('valfecpol').textContent = "";
                document.getElementById('valtipcer').textContent = "";
                document.getElementById('valnumcer').textContent = "";
                document.getElementById('<%=ddlCategoria.ClientID %>').value = "0";
                document.getElementById('<%=ddlActividadOnlyControl.ClientID %>').value = "0";             
                document.getElementById('<%=hfcategoria.ClientID %>').value = "";
                document.getElementById('<%=txtMarca.ClientID %>').value = "";
                document.getElementById('<%=txtClaseTipo.ClientID %>').value = "";
                document.getElementById('<%=txtModelo.ClientID %>').value = "";
                document.getElementById('<%=txtColor.ClientID %>').value = "";
                document.getElementById('<%=txttipocertificado.ClientID %>').value = "";
                document.getElementById('<%=txtnumcertificado.ClientID %>').value = "";
                document.getElementById('<%=txtfechamtop.ClientID %>').value = "";
                document.getElementById('<%=txtfechapoliza.ClientID %>').value = "";
                document.getElementById('<%=txtNota.ClientID %>').disabled = true;
                document.getElementById('<%=txtNota.ClientID %>').style.cssText = "background-color:Gray;width:300px;";
            }
        }
        function fValidaEmision() {
            var emision = document.getElementById('<%=rbemision.ClientID %>').checked;
            if (emision) {
//                document.getElementById('<%=btnBuscar.ClientID %>').disabled = true;
                document.getElementById('<%=ddlCategoria.ClientID %>').disabled = false;
                document.getElementById('<%=ddlActividadOnlyControl.ClientID %>').disabled = true;
                document.getElementById('<%=txtMarca.ClientID %>').disabled = false;
                document.getElementById('<%=txtClaseTipo.ClientID %>').disabled = false;
                document.getElementById('<%=txtPlaca.ClientID %>').focus();
                document.getElementById('<%=txtModelo.ClientID %>').disabled = false;
                document.getElementById('<%=txtColor.ClientID %>').disabled = false;
                document.getElementById('<%=txttipocertificado.ClientID %>').disabled = true;
                document.getElementById('<%=txtnumcertificado.ClientID %>').disabled = true;
                document.getElementById('<%=txtfechamtop.ClientID %>').disabled = true;
                document.getElementById('<%=txtfechapoliza.ClientID %>').disabled = true;
                document.getElementById('<%=ddlCategoria.ClientID %>').style.cssText = "background-color:White;width:200px;";
                document.getElementById('<%=ddlActividadOnlyControl.ClientID %>').style.cssText = "background-color:Gray;width:300px;";
                document.getElementById('<%=txtMarca.ClientID %>').style.cssText = "background-color:White;width:200px;";
                document.getElementById('<%=txtClaseTipo.ClientID %>').style.cssText = "background-color:White;width:200px;";
                document.getElementById('<%=txtModelo.ClientID %>').style.cssText = "background-color:White;width:200px;";
                document.getElementById('<%=txtColor.ClientID %>').style.cssText = "background-color:White;width:200px;";
                document.getElementById('<%=txttipocertificado.ClientID %>').style.cssText = "background-color:Gray;width:200px;";
                document.getElementById('<%=txtnumcertificado.ClientID %>').style.cssText = "background-color:Gray;width:200px;";
                document.getElementById('<%=txtfechamtop.ClientID %>').style.cssText = "background-color:Gray;width:200px;";
                document.getElementById('<%=txtfechapoliza.ClientID %>').style.cssText = "background-color:Gray;width:200px;";
                document.getElementById('<%=txtNota.ClientID %>').disabled = false;
                document.getElementById('<%=txtNota.ClientID %>').style.cssText = "background-color:White;width:300px;";
                document.getElementById('valcategoriavehiculo').textContent = " * obligatorio";
                document.getElementById('valClaseTipo').textContent = " * obligatorio";
                document.getElementById('valMarca').textContent = " * obligatorio";
                document.getElementById('valModelo').textContent = " * obligatorio";
                document.getElementById('valColor').textContent = " * obligatorio";
                document.getElementById('valfecmtop').textContent = "";
                document.getElementById('valfecpol').textContent = "";
                document.getElementById('valtipcer').textContent = "";
                document.getElementById('valnumcer').textContent = "";
                document.getElementById('<%=ddlCategoria.ClientID %>').value = "0";
                document.getElementById('<%=ddlActividadOnlyControl.ClientID %>').value = "0";
                document.getElementById('<%=hfcategoria.ClientID %>').value = "";
                document.getElementById('<%=txtMarca.ClientID %>').value = "";
                document.getElementById('<%=txtClaseTipo.ClientID %>').value = "";
                document.getElementById('<%=txtModelo.ClientID %>').value = "";
                document.getElementById('<%=txtColor.ClientID %>').value = "";
                document.getElementById('<%=txttipocertificado.ClientID %>').value = "";
                document.getElementById('<%=txtnumcertificado.ClientID %>').value = "";
                document.getElementById('<%=txtfechamtop.ClientID %>').value = "";
                document.getElementById('<%=txtfechapoliza.ClientID %>').value = "";
                document.getElementById('<%=txtNota.ClientID %>').disabled = false;
                document.getElementById('<%=txtNota.ClientID %>').style.cssText = "background-color:White;width:300px;";
            }
        }
        function fValidaEmisionf() {
            document.getElementById('<%=rbemision.ClientID %>').checked = true;
            document.getElementById('<%=rbrenovacion.ClientID %>').checked = false;
            var emision = document.getElementById('<%=rbemision.ClientID %>').checked;
            if (emision) {
                document.getElementById('<%=btnBuscar.ClientID %>').disabled = true;
                document.getElementById('<%=ddlCategoria.ClientID %>').disabled = false;
                document.getElementById('<%=ddlActividadOnlyControl.ClientID %>').disabled = true;
                document.getElementById('<%=txtMarca.ClientID %>').disabled = false;
                document.getElementById('<%=txtClaseTipo.ClientID %>').disabled = false;
                document.getElementById('<%=txtPlaca.ClientID %>').focus();
                document.getElementById('<%=txtModelo.ClientID %>').disabled = false;
                document.getElementById('<%=txtColor.ClientID %>').disabled = false;
                document.getElementById('<%=txttipocertificado.ClientID %>').disabled = true;
                document.getElementById('<%=txtnumcertificado.ClientID %>').disabled = true;
                document.getElementById('<%=txtfechamtop.ClientID %>').disabled = true;
                document.getElementById('<%=txtfechapoliza.ClientID %>').disabled = true;
                document.getElementById('<%=ddlCategoria.ClientID %>').style.cssText = "background-color:White;width:200px;";
                document.getElementById('<%=ddlActividadOnlyControl.ClientID %>').style.cssText = "background-color:Gray;width:300px;";
                document.getElementById('<%=txtMarca.ClientID %>').style.cssText = "background-color:White;width:200px;";
                document.getElementById('<%=txtClaseTipo.ClientID %>').style.cssText = "background-color:White;width:200px;";
                document.getElementById('<%=txtModelo.ClientID %>').style.cssText = "background-color:White;width:200px;";
                document.getElementById('<%=txtColor.ClientID %>').style.cssText = "background-color:White;width:200px;";
                document.getElementById('<%=txttipocertificado.ClientID %>').style.cssText = "background-color:Gray;width:200px;";
                document.getElementById('<%=txtnumcertificado.ClientID %>').style.cssText = "background-color:Gray;width:200px;";
                document.getElementById('<%=txtfechamtop.ClientID %>').style.cssText = "background-color:Gray;width:200px;";
                document.getElementById('<%=txtfechapoliza.ClientID %>').style.cssText = "background-color:Gray;width:200px;";
                document.getElementById('<%=txtNota.ClientID %>').disabled = false;
                document.getElementById('<%=txtNota.ClientID %>').style.cssText = "background-color:White;width:300px;";
                document.getElementById('valcategoriavehiculo').textContent = " * obligatorio";
                document.getElementById('valClaseTipo').textContent = " * obligatorio";
                document.getElementById('valMarca').textContent = " * obligatorio";
                document.getElementById('valModelo').textContent = " * obligatorio";
                document.getElementById('valColor').textContent = " * obligatorio";
                document.getElementById('valfecmtop').textContent = "";
                document.getElementById('valfecpol').textContent = "";
                document.getElementById('valtipcer').textContent = "";
                document.getElementById('valnumcer').textContent = "";
                document.getElementById('<%=ddlCategoria.ClientID %>').value = "0";
                document.getElementById('<%=ddlActividadOnlyControl.ClientID %>').value = "0";
                document.getElementById('<%=hfcategoria.ClientID %>').value = "";
                document.getElementById('<%=txtMarca.ClientID %>').value = "";
                document.getElementById('<%=txtClaseTipo.ClientID %>').value = "";
                document.getElementById('<%=txtModelo.ClientID %>').value = "";
                document.getElementById('<%=txtColor.ClientID %>').value = "";
                document.getElementById('<%=txttipocertificado.ClientID %>').value = "";
                document.getElementById('<%=txtnumcertificado.ClientID %>').value = "";
                document.getElementById('<%=txtfechamtop.ClientID %>').value = "";
                document.getElementById('<%=txtfechapoliza.ClientID %>').value = "";
                document.getElementById('<%=txtNota.ClientID %>').disabled = false;
                document.getElementById('<%=txtNota.ClientID %>').style.cssText = "background-color:White;width:300px;";
            }
        }
        function Panel() {
            $(document).ready(function () {
                $('#ventana_popup').fadeIn('slow');
                $('#popup-overlay').fadeIn('slow');
                $('#popup-overlay').height($(window).height());
            });
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
