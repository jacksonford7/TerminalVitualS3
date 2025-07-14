<%@ Page Language="C#" MasterPageFile="~/site.Master" AutoEventWireup="true" Title="Consulta de Colaboradores" MaintainScrollPositionOnPostback="true"
         CodeBehind="solicitudpermisodeaccesovehiculo.aspx.cs" Inherits="CSLSite.cliente.solicitudpermisodeaccesovehiculo" %>

<%@ MasterType VirtualPath="~/site.Master" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="placehead" runat="server">
    <link href="../shared/estilo/tables.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/aisv-general.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/work.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/catalogos.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/info.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/turnos.js" type="text/javascript"></script>
    <style type="text/css">
        .warning
        {
            background-color: Yellow;
            color: Red;
        }
        
        #progressBackgroundFilter
        {
            position: fixed;
            bottom: 0px;
            right: 0px;
            overflow: hidden;
            z-index: 1000;
            top: 0;
            left: 0;
            background-color: #CCC;
            opacity: 0.8;
            filter: alpha(opacity=80);
            text-align: center;
        }
        #processMessage
        {
            text-align: center;
            position: fixed;
            top: 30%;
            left: 43%;
            z-index: 1001;
            border: 5px solid #67CFF5;
            width: 200px;
            height: 100px;
            background-color: White;
            padding: 0;
        }
        #aprint
        {
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
            background-position: left center;
            text-decoration: none;
            padding: 5px 2px 5px 30px;
        }
        * input[type=text]
        {
            text-align: left !important;
        }
        </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="placebody" runat="server">
    <input id="zonaid" type="hidden" value="2" />
    <asp:ToolkitScriptManager ID="tkcal" EnableScriptGlobalization="true" runat="server">
    </asp:ToolkitScriptManager>
    <noscript>
        <meta http-equiv="refresh" content="0; url=sinsoporte.htm" />
    </noscript>
    <div>
        <i class="ico-titulo-1"></i>
        <h2>
            MCA - Módulo de Control de Acceso</h2>
        <h2>
            &nbsp;</h2>
        <br />
        <i class="ico-titulo-2"></i>
        <h1>
            Solicitar Acceso de Vehículos Livianos previamente registrados
         </h1>
        <br />
    </div>
    <div class="seccion" style=" display:none">
      <div class="informativo">
      <table class="controles" cellspacing="0" cellpadding="1">
      <tr><td rowspan="2" class="inum"> <div class="number">1</div></td><td class="level1" >
          Tipo de permiso.</td></tr>
      <tr><td class="level2">Tipo de Solicitud.</td></tr>
      </table>
     </div>
     <div class="colapser colapsa"></div>
     <%--<div class="accion">--%>
     <div class="bt-bottom  bt-right bt-left">
     <table cellspacing="0" cellpadding="1" style=" width:100%">
     <tr>
     <td class="bt-bottom ">
           <asp:DropDownList ID="dptipoevento" runat="server" Width="400px"
                onchange="valdltipsol(this, valtipeve);" AutoPostBack="false"
               onselectedindexchanged="dptipoevento_SelectedIndexChanged">
                 <asp:ListItem Value="0">* Seleccione permiso *</asp:ListItem>
          </asp:DropDownList>
        </td>
        <td class="bt-bottom bt-right validacion "><span id='valtipeve' class="validacion" > * obligatorio</span></td>
     </tr>
     </table>
     </div>
    <%--</div>--%>
    </div>
    <div class="seccion">
        <div class="accion">
            <div id="Div2" class="accion" runat="server">
                <div class="informativo">
                    <table class="controles" cellspacing="0" cellpadding="1">
                        <tr>
                            <td rowspan="2" class="inum">
                                <div class="number">
                                    1</div>
                            </td>
                            <td class="level1">
                                    Datos del permisos.</td>
                        </tr>
                        <tr>
                            <td class="level2">
                                Confirme 
                                que los datos sean los correctos.
                            </td>
                        </tr>
                    </table>
                </div>
                <div class="colapser colapsa">
                </div>
                <%--<table class="xcontroles" cellspacing="0" cellpadding="1">
       <tr><th class="bt-bottom bt-right bt-left bt-top" colspan="4"> Criterios de consulta:</th></tr>
       <tr>
        <td class="bt-bottom bt-right bt-left" >Cedula:</td>
        <td class="bt-bottom bt-right">
             <asp:TextBox ID="txtColaborador" runat="server" Width="120px" MaxLength="10"
             style="text-align: center" onblur="cajaControl(this);"
             onkeypress="return soloLetras(event,'01234567890',true)"></asp:TextBox>
        </td>
       </tr>
       </table>--%>
                <table class="xcontroles" cellspacing="0" cellpadding="1">
                    <tr>
                        <td class="bt-bottom bt-right bt-left">
                            Vehículo Liviano:
                        </td>
                        <td class="bt-bottom">
                            <%--<a class="tooltip" >
          <span class="classic">Esta informacion contiene los tipos de solicitudes disponibles.</span>--%>
                            <asp:DropDownList ID="ddlNominaOnlyControl" runat="server" Width="400px" 
                                onchange="valdltipsol(this, valcolaborador);"> 
                                <asp:ListItem Value="0">* Seleccione origen *</asp:ListItem>
                            </asp:DropDownList>
                            <%--</a>--%>
                        </td>
                        <td class="bt-bottom bt-right validacion "><span id='valcolaborador' class="validacion" > * obligatorio</span></td>
                    </tr>
        <%--<tr>
                            <td class="bt-bottom bt-right bt-left">
                                Área Destino/Actividad:
                            </td>
                            <td class="bt-bottom ">
                                <asp:TextBox ID="txtarea" runat="server" Width="200px" MaxLength="30 " Style="text-align: center"
                                    onblur="checkcaja(this,'valnumfac',true);" onkeypress="return soloLetras(event,'abcdefghijklmnñopqrstuvwxyz01234567890 -_/',true)"></asp:TextBox>
                            </td>
                            <td class="bt-bottom bt-right validacion ">
                                <span id="valnumfac" class="validacion">* obligatorio</span>
                            </td>
                        </tr>--%>
                        <tr>
                        <th class=" bt-right bt-left" colspan="4">
                            <asp:Label Text="Criterios de Consulta para Conductor Designado:" style="text-align: left;text-transform:none" runat="server" />
                        </th>
                        </tr>
                        <tr>
                        <th class="bt-bottom bt-right bt-left" colspan="4">
                            <asp:Label Text="Cedula" style="text-transform:none; font-weight:normal" runat="server" />[<input id="rbcedula" runat="server" type="radio" name="deck" checked="true" value="ced"/>]
                            <asp:Label Text="Nombre(s)" style="text-transform:none; font-weight:normal" runat="server" />[<input id="rbnombres" runat="server" type="radio" name="deck" value="nom"/>]
                            <asp:Label Text="Apellido(s)" style="   text-transform:none; font-weight:normal" runat="server" />[<input id="rbapellidos" runat="server" type="radio" name="deck" value="ape"/>]
                            <asp:TextBox runat="server" id="txtcriterioconsulta" Width="200px"/>
                            <asp:Button ID="btnBuscar" runat="server" Width="100px" Text="Buscar" 
                                OnClientClick="return fvalidaCriterios();" onclick="btnBuscar_Click"/>
                        </th>
                        </tr>
                        <tr>
         <td class="bt-bottom  bt-right bt-left" style=" width:155px" >Cédula Conductor Designado:</td>
         <td class="bt-bottom">
            <asp:TextBox ID="txtci" runat="server" Width="200px" MaxLength="10" Enabled="False" BackColor="Gray"
             style="text-align: center"
             onBlur="validalacedula(this,'valci',true);"
             onkeypress="return soloLetras(event,'01234567890',true)"></asp:TextBox>
         </td>
         <td class="bt-bottom bt-right validacion "><%--<span id="valci" runat="server" class="validacion"> * obligatorio</span>--%></td>
         </tr>
                        <tr><td class="bt-bottom  bt-right bt-left" style=" width:155px;">
                            Nombres Conductor Designado:</td>
         <td class="bt-bottom">
             <asp:TextBox 
             style="text-align: center" 
             ID="txtNombres" runat="server" ClientIDMode="Static" width="400px" MaxLength="500" Enabled="False" BackColor="Gray"
             onkeypress="return soloLetras(event,'abcdefghijklnmñopqrstuvwxyz01234567890 .')" 
             onblur="checkcajalarge(this,'valnombres',true);"></asp:TextBox>
         </td>
         <td class="bt-bottom bt-right validacion"><%--<span id="valnombres" runat="server" class="validacion"> * obligatorio</span>--%></td>
         </tr>
         
         <tr><td class="bt-bottom  bt-right bt-left" style=" width:155px;">
                            Apellidos Conductor Designado:</td>
         <td class="bt-bottom">
             <asp:TextBox 
             style="text-align: center" 
             ID="txtApellidos" runat="server" ClientIDMode="Static" width="400px" MaxLength="500" Enabled="False" BackColor="Gray"
             onkeypress="return soloLetras(event,'abcdefghijklnmñopqrstuvwxyz01234567890 .')" 
             onblur="checkcajalarge(this,'valapellidos',true);"></asp:TextBox>
         </td>
         <td class="bt-bottom bt-right validacion"><%--<span id="valapellidos" runat="server" class="validacion"> * obligatorio</span>--%></td>
         </tr>
         <tr style=" display:none"><td class="bt-bottom  bt-right bt-left" style=" width:155px;">Cargo del Empleado:</td>
         <td class="bt-bottom">
             <asp:TextBox 
             style="text-align: center" 
             ID="txtcargo" runat="server" ClientIDMode="Static" width="200px" MaxLength="500"
             onkeypress="return soloLetras(event,'abcdefghijklnmñopqrstuvwxyz01234567890 /_-.')" 
             onblur="checkcaja(this,'valcargo',true);"></asp:TextBox>
         </td>
         <td class="bt-bottom bt-right validacion"><span id="valcargo" class="validacion"> * obligatorio</span></td>
         </tr>
                  <tr>
        <td class="bt-bottom bt-right bt-left" style=" width:155px">Area:</td>
        <td class="bt-bottom ">
           <asp:DropDownList runat="server" Width="400px" ID="ddlAreaOnlyControl" onchange="validadropdownlist(this, valareaoc);">
                 <asp:ListItem Value="0">* Elija *</asp:ListItem>
          </asp:DropDownList>
        </td>
        <td class="bt-bottom bt-right validacion "><span id='valareaoc' class="validacion" > * obligatorio</span></td>
        </tr>
      <tr>
        <td class="bt-bottom bt-right bt-left" style=" width:155px">Actividad: Permitida:</td>
        <td class="bt-bottom ">
           <asp:DropDownList runat="server" Width="400px"  ID="ddlActividadOnlyControl" onchange="validadropdownlist(this, valactividad);">
                 <asp:ListItem Value="0">* Elija *</asp:ListItem>
           </asp:DropDownList>
        </td>
        <td class="bt-bottom bt-right validacion "><span id='valactividad' class="validacion" > * obligatorio</span></td>
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
         <tr><td class="bt-bottom  bt-right bt-left" style=" width:155px">Nota:</td>
         <td class="bt-bottom">
                 <asp:TextBox ID="txtNota" runat="server" MaxLength="3000" TextMode="MultiLine" Width="400px" style="overflow:auto;resize:none"
                              onkeypress="return soloLetras(event,'abcdefghijklnmñopqrstuvwxyz01234567890 /_-.',true)"/>
         </td>
         <td class="bt-bottom bt-right"><span style="font-size:small; font-family:Consolas; font-weight:normal; font-style:italic; color:Green" id="Span1"> * opcional</span></td>
         </tr>
         <tr>
      <td class="bt-left" style=" width:155px; background-color:White"></td>
      <td>
      <asp:Button ID="btnAgregar" runat="server" Width="100px" Text="Agregar" OnClientClick="return validaCabecera();"
                onclick="btnAgregar_Click" />
        </td>
       <td class="bt-right"></td>
       </tr>
                </table>
                <div class="botonera" style=" display:none">
                    <img alt="loading.." src="../shared/imgs/loader.gif" id="loader" class="nover" />
                    <asp:Button ID="btbuscar" runat="server" Text="Iniciar la búsqueda" OnClick="btbuscar_Click" />
                </div>
            </div>
             <asp:UpdatePanel ID="upresult2" runat="server"  >
      <ContentTemplate>
      <script type="text/javascript">          Sys.Application.add_load(BindFunctions);</script>
                    <div class="informativo" id="colector">
      <table runat="server" id="tableexpo">
      <tr><td>
              <asp:GridView runat="server" id="gvColaboradores" class="tabRepeat" AutoGenerateColumns="False" Width="100%" 
                       onrowcommand="gvColaboradores_RowCommand"  onrowdeleting="gvColaboradores_RowDeleting">
              <Columns>
                  <asp:TemplateField HeaderText="Placa">
                      <ItemTemplate>
                          <asp:Label ID="lplaca" runat="server" Width="40px" Text='<%# Eval("[Placa]") %>'
                          ></asp:Label>
                      </ItemTemplate>
                  </asp:TemplateField>
                  <asp:TemplateField HeaderText="Cedula Conductor">
                      <ItemTemplate>
                          <asp:Label ID="lcedula" runat="server" Width="50px" Text='<%# Eval("[Cedula]") %>'
                          ></asp:Label>
                      </ItemTemplate>
                  </asp:TemplateField>
                  <asp:TemplateField HeaderText="Nombres Conductor">
                      <ItemTemplate>
                          <asp:Label ID="lnombres" runat="server" Width="60px" Text='<%# Eval("[Nombres]") %>'
                          ></asp:Label>
                      </ItemTemplate>
                  </asp:TemplateField>
                  <asp:TemplateField HeaderText="Apellidos Conductor">
                      <ItemTemplate>
                          <asp:Label ID="lapellidos" runat="server" Width="60px" Text='<%# Eval("[Apellidos]") %>'
                          ></asp:Label>
                      </ItemTemplate>
                  </asp:TemplateField>
                  <asp:TemplateField HeaderText="Área">
                      <ItemTemplate>
                          <asp:Label ID="larea" runat="server"  Text='<%# Eval("[Area]") %>' ></asp:Label>
                      </ItemTemplate>
                  </asp:TemplateField>
                  <asp:TemplateField HeaderText="Actividad">
                      <ItemTemplate>
                          <asp:Label ID="lactividad" runat="server" Text='<%# Eval("[Actividad]") %>' ></asp:Label>
                      </ItemTemplate>
                  </asp:TemplateField>
                  <asp:TemplateField HeaderText="Cargo">
                      <ItemTemplate>
                          <asp:Label ID="lcargo" runat="server"  Text='<%# Eval("[Cargo]") %>' ></asp:Label>
                      </ItemTemplate>
                  </asp:TemplateField>
                  <asp:TemplateField HeaderText="Fecha Ingreso">
                      <ItemTemplate>
                          <asp:Label ID="lfecini" runat="server" Width="50px" Text='<%# Eval("[FechaIng]") %>'></asp:Label>
                      </ItemTemplate>
                  </asp:TemplateField>
                  <asp:TemplateField HeaderText="Fecha Caducidad">
                      <ItemTemplate>
                          <asp:Label ID="lfeccad" runat="server" Width="50px" Text='<%# Eval("[FechaSal]") %>'></asp:Label>
                      </ItemTemplate>
                  </asp:TemplateField>
                  <asp:TemplateField HeaderText="Nota">
                      <ItemTemplate>
                      <div style="overflow-y:scroll; overflow-x: hidden; height:30px; text-align:center">
                          <asp:Label ID="tnota"  style="text-transform :uppercase" runat="server" ToolTip='<%# Eval("[Nota]") %>' Text='<%# Eval("[Nota]") %>'></asp:Label>
                      </div>
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
            <div class="cataresult" id="resultado" runat="server" style=" display:none">
                <%--<asp:UpdatePanel ID="upresult" runat="server" ChildrenAsTriggers="true">
        <ContentTemplate>--%>
                <div>
                <div class="msg-alerta" id="alerta" runat="server">
                </div>
                <input id="idlin" type="hidden" runat="server" clientidmode="Static" />
                <input id="diponible" type="hidden" runat="server" clientidmode="Static" />
                <div id="xfinder">
                    <div class="findresult">
                        <%--<div class="booking" >--%>
                        <div class="informativo" style="height: 100%; overflow: auto">
                            <div class="separator">
                                Información disponible:</div>
                            <div class="bokindetalle">
                                <asp:Repeater ID="tablePagination" runat="server" >
                                    <HeaderTemplate>
                                        <table id="tablar2" cellspacing="1" cellpadding="1" class="tabRepeat">
                                            <thead>
                                                <tr>
                                                    <th>
                                                        # Solicitud
                                                    </th>
                                                    <th>
                                                        Tipo Solicitud
                                                    </th>
                                                    <th>
                                                        Estado Solicitud
                                                    </th>
                                                    <th>
                                                        Empresa
                                                    </th>
                                                    <th>
                                                        Cedula
                                                    </th>
                                                    <th>
                                                        Nombres y Apellidos
                                                    </th>
                                                    <%--<th>Estado Colaborador</th>--%>
                                                    <th style="display: none">
                                                        Ruc
                                                    </th>
                                                    <th style="display: none">
                                                        Tipo
                                                    </th>
                                                    <%--<th>Usuario</th>
                 <th>Fecha Registro</th>--%>
                                                    <th style="display: none">
                                                    </th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <tr class="point">
                                            <td>
                                                <%#Eval("NUMSOLICITUD")%>
                                            </td>
                                            <td style="width: 150px">
                                                <%#Eval("TIPOSOLICITUD")%>
                                            </td>
                                            <td>
                                                <%#Eval("ESTADO_SOLICITUD")%>
                                            </td>
                                            <td>
                                                <asp:Label ID="lempresa" Style="text-transform: uppercase" runat="server" Width="150px"
                                                    Text='<%# Eval("EMPRESA") %>'></asp:Label>
                                            </td>
                                            <td>
                                                <%#Eval("CEDULA")%>
                                            </td>
                                            <td style="width: 250px">
                                                <%#Eval("NOMBRE")%>
                                            </td>
                                            <%--<td><%#Eval("ESTADO_COLABORADOR")%></td>--%>
                                            <td style="display: none">
                                                <asp:Label ID="lruccipas" Style="text-transform: uppercase" runat="server" Width="70px"
                                                    Text='<%# Eval("RUCCIPAS") %>'></asp:Label>
                                            </td>
                                            <td style="display: none">
                                                <%#Eval("TIPO")%>
                                            </td>
                                            <%--<td style=" width:80px"><%#Eval("USUARIOING")%></td>
                  <td style=" width:80px"><%#Eval("FECHAING")%></td>--%>
                                            <td style="display: none">
                                                <a style="width: 90px" id="adjDoc" class="topopup" onclick="redirectsol('<%# Eval("NUMSOLICITUD") %>', '<%# Eval("IDSOLCOL") %>', '<%#Eval("CEDULA")%>');">
                                                    <i class="ico-find"></i>Ver Documentos </a>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        </tbody> </table>
                                    </FooterTemplate>
                                </asp:Repeater>
                            </div>
                            <%--<div class="botonera" runat="server" id="btnera">
        <img alt="loading.." src="../shared/imgs/loader.gif" id="loader" class="nover"  />
        <input id="btsalvar" type="button" value="Proceder y Asignar"  onclick="prepareObject();encerar();" /> &nbsp;
        </div>--%>
                        </div>
                    </div>
                </div>
                <div id="sinresultado" runat="server" class="msg-info">
                </div>
                </div>
                <%--</ContentTemplate>
                     <Triggers>
                         <asp:AsyncPostBackTrigger ControlID="btbuscar" />
                     </Triggers>
        </asp:UpdatePanel>--%>
                <div class="accion" style=" display:none">
                    <div class="informativo">
                        <table class="controles" cellspacing="0" cellpadding="1">
                            <tr>
                                <td rowspan="2" class="inum">
                                    <div class="number">
                                        2</div>
                                </td>
                                <td class="level1">
                                    Datos del permisos.
                                </td>
                            </tr>
                            <tr>
                                <td class="level2">
                                    Datos del permisos.
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div class="colapser colapsa">
                    </div>
                    
                </div>
                <div class="msg-alerta" id="infootros" runat="server">
                    Usted deberá adjuntar documentos que demuestren el motivo por el cual está solicitando
                    los permisos de acceso al terminal, si son mas de un documento adjuntarlos en el
                    mismo .pdf: Ejemplo: BL, copia de factura cancelada, requerimiento de servicios,
                    carta de la Subsecretaría, fitosanitario, etc.</div>
                <div class="accion" id="factura" runat="server">
                    <div class="accion">
                        <table runat="server" class="controles" id="tablefac" style="font-size: small">
                            <tr>
                                <td class="bt-bottom bt-top  bt-right bt-left" style="width: 155px">
                                    Adjuntar factura:
                                </td>
                                <td class="bt-bottom bt-left bt-top bt-right">
                                    <asp:FileUpload runat="server" ID="fuAdjuntarFactura" Width="100%" extension='.pdf'
                                        class="uploader" title="Adjunte el archivo en formato PDF." onchange="validaextension(this)" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </div>
        <%--<asp:HiddenField ID="hfCustomerId" runat="server" />--%>
        <%--<asp:HiddenField ID="hfpermisopermanente" runat="server" />--%>
    </div>
    <div class="seccion" style=" display:none">
    <div class="informativo">
      <table>
      <tr><td rowspan="2" class="inum"> <div class="number"><asp:Label runat="server" ID="ls6" Text="2"></asp:Label></div></td><td class="level1" >Subir documentos.</td></tr>
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
      <table class="controles" cellspacing="0" cellpadding="1" style=" display:none">
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
            <th class="nover">Tipo de Solicitud</th>
            <th class="nover"></th>
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
            <td style=" width:125px; display:none"><%#Eval("DESCRIPCION")%></td>
            <td class="nover"><asp:TextBox ID="txtidsolicitud" runat="server" Text='<%#Eval("IDTIPSOL")%>' Width="5px"/></td>
            <td class="nover"><asp:TextBox ID="txtiddocemp" runat="server" Text='<%#Eval("IDDOCEMP")%>' Width="5px"/></td>
            <td style=" width:300px; font-size:inherit"><%#Eval("DOCUMENTO")%></td>
            <td style=" width:200px">
                <%--<input extension='<%#Eval("EXTENSION")%>' class="uploader" id="fsupload" title="Escoja el archivo con formato indicado en la siguiente columna."
                       onchange="validaextension(this)" type="file"  runat="server" clientidmode="Static" />--%>
                <asp:FileUpload extension='<%#Eval("EXTENSION")%>' class="uploader" id="fsupload" title="Escoja el archivo con formato indicado en la siguiente columna."
                       onchange="validaextension(this)" style=" font-size:small" runat="server"/>
                <%--<input class="uploader" id="File1" title="Escoja el archivo CSV (Excel separado por comas)" accept="csv" type="file"  runat="server" clientidmode="Static" />--%>
            </td>
            <td style=" width:30px"><%#Eval("EXTENSION")%></td>
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
    </div>
    <%--<div class="g-recaptcha" data-sitekey="6Le7xBgTAAAAAEvVoppLLsqPNgmr7gDEDhGpUuDp" data-sitekey="my_key"></div>--%>
    </div>
            </div>
               
                       <div class="botonera">
                    <img alt="loading.." src="../shared/imgs/loader.gif" id="Img1" class="nover" />
                    <asp:Button ID="btsalvar" runat="server" Text="Solicitar Permiso de Acceso" OnClick="btsalvar_Click"
                        OnClientClick="return prepareObject('¿Esta seguro de crear la solicitud de Permiso de Acceso?');"
                        ToolTip="Confirma la información y crea la solicitud de Permiso de Acceso." Width="200px" />
                </div>
     <asp:HiddenField runat="server" ID="hftablecol" Value="0" />
    <script src="../Scripts/pages.js" type="text/javascript"></script>
    <script src="../Scripts/credenciales.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.tablesorter.min.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.tablesorter.pager.js" type="text/javascript"></script>
    <%--<script src="http://ajax.aspnetcdn.com/ajax/jQuery/jquery-1.10.0.min.js" type="text/javascript"></script>
    <script src="http://ajax.aspnetcdn.com/ajax/jquery.ui/1.9.2/jquery-ui.min.js" type="text/javascript"></script>
    <link href="http://ajax.aspnetcdn.com/ajax/jquery.ui/1.9.2/themes/blitzer/jquery-ui.css" />--%>
    <script type="text/javascript">
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
        function validaCabecera() {
        try {
                var vals = document.getElementById('<%=ddlNominaOnlyControl.ClientID %>').value;
                if (vals == 0) {
                    alert('* Elija un Vehículo *');
                    document.getElementById('<%=ddlNominaOnlyControl.ClientID %>').focus();
                    document.getElementById('<%=ddlNominaOnlyControl.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:400px;";
                    document.getElementById("loader").className = 'nover';
                    return false;
                }
                var vals = document.getElementById('<%=txtci.ClientID %>').value;
                var vals2 = document.getElementById('<%=txtNombres.ClientID %>').value;
                var vals3 = document.getElementById('<%=txtApellidos.ClientID %>').value;
                if (vals == 0) {
                    alert('* Consulta los datos del Conductor Designado *');
                    document.getElementById('<%=txtcriterioconsulta.ClientID %>').focus();
                    document.getElementById('<%=txtcriterioconsulta.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:200px;";
                    document.getElementById("loader").className = 'nover';
                    return false;
                }
                
//                var vals = document.getElementById('<%=dptipoevento.ClientID %>').value;
//                if (vals == 0) {
//                    alert('* Seleccione un Permiso *');
//                    document.getElementById('<%=dptipoevento.ClientID %>').focus();
//                    document.getElementById('<%=dptipoevento.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:400px;";
//                    document.getElementById("loader").className = 'nover';
//                    return false;
////                }
//                var vals = document.getElementById('<%=txtcargo.ClientID %>').value;
//                if (vals == '' || vals == null || vals == undefined) {
//                    alert('* Escriba el Cargo del Empleado *');
//                    document.getElementById('<%=txtcargo.ClientID %>').focus();
//                    document.getElementById('<%=txtcargo.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:200px;";
//                    document.getElementById("loader").className = 'nover';
//                    return false;
//                }
                var vals = document.getElementById('<%=txtfecing.ClientID %>').value;
                if (vals == '' || vals == null || vals == undefined) {
                    alert('* Escriba la Fecha de Ingreso *');
                    document.getElementById('<%=txtfecing.ClientID %>').focus();
                    document.getElementById('<%=txtfecing.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:200px;";
                    document.getElementById("loader").className = 'nover';
                    return false;
                }
                var vals = document.getElementById('<%=txtfecsal.ClientID %>').value;
                if (vals == '' || vals == null || vals == undefined) {
                    alert('* Escriba la Fecha de Salida *');
                    document.getElementById('<%=txtfecsal.ClientID %>').focus();
                    document.getElementById('<%=txtfecsal.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:200px;";
                    document.getElementById("loader").className = 'nover';
                    return false;
                }
                document.getElementById('<%=txtci.ClientID %>').disabled = false;
                document.getElementById('<%=txtNombres.ClientID %>').disabled = false;
                document.getElementById('<%=txtApellidos.ClientID %>').disabled = false;
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
            document.getElementById('<%=txtcriterioconsulta.ClientID %>').value = "";
            document.getElementById('<%=txtNombres.ClientID %>').value = objeto.nombres;
            document.getElementById('<%=txtApellidos.ClientID %>').value = objeto.apellidos;
            document.getElementById('<%=txtci.ClientID %>').value = objeto.cedula;
            document.getElementById('<%=txtNombres.ClientID %>').style.cssText = "background-color:White;width:400px;";
            document.getElementById('<%=txtApellidos.ClientID %>').style.cssText = "background-color:White;width:400px;";
            document.getElementById('<%=txtci.ClientID %>').style.cssText = "background-color:White;width:200px;";
        }
        function myDivCab() {
            document.getElementById('divcabecera').style.display = "block";
        }
        function clear() {
            document.getElementById('numbook').textContent = '...';
            document.getElementById('nbrboo').value = '';
        }
        function clear2() {
            document.getElementById('txtfecha').textContent = '...';
            document.getElementById('xfecha').value = '';
        }
        var programacion = {};
        var lista = [];
        function prepareObject() {
            try {
                if (confirm('¿Esta seguro de solicitar el acceso a la terminal?') == false) {
                    return false;
                }
                var vals = document.getElementById('<%=hftablecol.ClientID %>').value;
                if (vals == '0') {
                    alert('* Agregue al menos un Vehículo *');
                    document.getElementById("loader").className = 'nover';
                    return false;
                }
                //Valida Documentos
//                lista = [];
//                var tbl = document.getElementById('tablar');
//                for (var f = 0; f < tbl.rows.length; f++) {
//                    var celColect = tbl.rows[f].getElementsByTagName('td');
//                    if (celColect != undefined && celColect != null && celColect.length > 0) {
//                        var tdetalle = {
//                            documento: celColect[4].getElementsByTagName('input')[0].value
//                        };
//                        this.lista.push(tdetalle);
//                    }
//                }
//                var nomdoc = null;
//                for (var n = 0; n < this.lista.length; n++) {
//                    if (lista[n].documento == '' || lista[n].documento == null || lista[n].documento == undefined) {
//                        alert('* Seleccione todos los documentos requeridos *');
//                        document.getElementById("loader").className = 'nover';
//                        return false;
//                    }
//                    if (nomdoc == lista[n].documento) {
//                        alert('* Existen archivos repetidos, revise por favor *');
//                        document.getElementById("loader").className = 'nover';
//                        return false;
//                    }
//                    nomdoc = lista[n].documento;
//                } 
                document.getElementById("loader").className = '';
                return true;
            } catch (e) {
                alert('Lamentamos comunicarle que ha ocurrido el siguiente problema:\n' + e.Message);
            }
        }
        function valdltippermiso(control, validador) {
            if (control.value != 0) {
                control.style.cssText = "background-color:none;color:none;width:400px;"
                validador.innerHTML = '';
            }
            control.style.cssText = "background-color:White;color:Red;width:400px;";
            validador.innerHTML = '<span class="obligado">OBLIGATORIO!</span>';
            var vals = document.getElementById('<%=ddlNominaOnlyControl.ClientID %>').value;
            if (vals == "PER") {
                alert(vals);
            }
        }
        function valPermiso() {
            
        }
        function openPop() {
            var bo = document.getElementById('nbrboo').value;
            if (bo == undefined || bo == '' || bo == null) {
                alert('Por favor seleccione el booking primero');
                return;
            }
            window.open('../catalogo/calendarioConsolidacion.aspx?bk=' + bo, 'name', 'width=850,height=480')
        }
        function redirectsol(val, val2, val3) {
            var caja = val;
            var caja2 = val2;
            var caja3 = val3;
            window.open('../credenciales/consulta/documentos-solicitud-colaborador/?numsolicitud=' + caja + '&idsolcol=' + caja2 + '&cedula=' + caja3)
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
                    alert('Escriba la Cedula del Conductor Designado.');
                    return false;
                }
                if (nombres) {
                    alert('Escriba el Nombre(s) del Conductor Designado.');
                    return false;
                }
                if (apellidos) {
                    alert('Escriba el Apellido(s) del Conductor Designado.');
                    return false;
                }
            }
            return true;
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
