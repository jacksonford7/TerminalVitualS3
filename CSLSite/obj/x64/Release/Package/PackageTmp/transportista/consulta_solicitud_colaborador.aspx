<%@ Page  Language="C#" AutoEventWireup="true" Title="Registra Huella y Foto/Creaciòn de Permiso"
         CodeBehind="consulta_solicitud_colaborador.aspx.cs" Inherits="CSLSite.consulta_solicitud_colaborador" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Registra Huella y Foto/Creación de Permiso</title>
        <link href="../shared/estilo/jquery.datetimepicker.css" rel="stylesheet" type="text/css" />
    <script src="../../Scripts/jquery-1.7.1.min.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery.datetimepicker.js" type="text/javascript"></script>
    <link href="../shared/estilo/catcomprobantedepago.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/work.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/aisv-general.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/tables.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/info.css" rel="stylesheet" type="text/css" />
        <link href="../Inicio/css/font-awesome.min.css" rel="stylesheet" type="text/css" />
    <link href="../Inicio/base-site.css" rel="stylesheet" type="text/css" />
        <link rel="../shortcut icon" type="image/x-icon" href="../favicon.ico" />
    <link href="../Inicio/base-site.css" rel="stylesheet" type="text/css" />
    <link href="../Inicio/css/font-awesome.min.css" rel="stylesheet" type="text/css" />
    <link rel="../shortcut icon" type="image/x-icon" href="../favicon.ico" />
    <link href="../shared/estilo/work.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
     * input[type=text]
        {
            text-align:left!important;
        }
        #tablerp
        {
            width: 726px;
        }
        .aling-check
        {
             text-align:center
        }
    </style>
    <script type="text/javascript">
        function BindFunctions() {
            document.getElementById('imagen').innerHTML = '';
            $(document).ready(function () {
                $('#tablasort').tablesorter({ cancelSelection: true, cssAsc: "ascendente", cssDesc: "descendente", headers: { 5: { sorter: false }, 6: { sorter: false}} }).tablesorterPager({ container: $('#pager'), positionFixed: false, pagesize: 10 });
            });
        }       
    </script>
</head>
<body>
     <noscript><meta http-equiv="refresh" content="0; url=sinsoporte.htm" /> </noscript>
    <form id="bookingfrm2" runat="server">
    <input id="zonaid" type="hidden" value="7" />
    <asp:ToolkitScriptManager ID="tkscata" runat="server"></asp:ToolkitScriptManager>
        <div class="catabody">
    <div class="catawrap" >
<%-- <div>
 <i class="ico-titulo-1"></i><h2>MCA - Módulo de Control de Acceso</h2><h2>&nbsp;</h2>
     <br />
 <i class="ico-titulo-2"></i><h1>Confirmación de Pago/Creaciòn de Permiso</h1><br />
 </div>--%>
  <div class="seccion">
      <div class="informativo">
      <table>
      <tr><td rowspan="2" class="inum"> <div class="number">1</div></td><td class="level1" >Datos Generales del Colaborador(es).</td></tr>
      <tr><td class="level2">
         Confirme que los datos sean correctos.
      </td></tr>
      </table>
     </div>
     <div class="colapser colapsa"></div>
     <div class=" bt-bottom bt-top bt-left bt-right"   style=" overflow-x: scroll; height:300px">
     <asp:Repeater id="rpDetalle" runat="server">
                 <HeaderTemplate>
                 <table id="tabla" cellspacing="1" cellpadding="1" class="tabRepeat" style=" width:2100px">
                 <thead>
                 <tr>
                 <th>Tipo - Permiso</th>
                 <th>Estado Actual</th>
                 <th id="HeaderColDeleteHuella" runat="server">Capturar</th>
                 <th id="HeaderColDeleteFoto" runat="server">Capturar</th>
                 <th class="nover"></th>
                 <th class="nover"></th>
                 <th class="nover"></th>
                 <th>CI/Pasaporte</th>
                 <th>Nombre</th>
                 <th>Tipo Sangre</th>
                 <th>Dirección Domiciliaria</th>
                 <th>Telefono</th>
                 <th>Email</th>
                 <th>Lugar Nacimiento</th>
                 <th>Fecha Nacimiento</th>
                 <th>Cargo</th>
                 <th>Licencia</th>
                 <th>Fecha Exp. Licencia</th>
                 <th>Colaborador Rechazado</th>
                 <th>Comentario</th>
                 <th></th>
                 <th class="nover"></th>
                 <th class="nover"></th>
                 </tr>
                 </thead> 
                 <tbody>
                 </HeaderTemplate>
                 <ItemTemplate>
                 <tr class="point" >
                 <td style=" width:225px"><asp:Label runat="server"  Width="225px" ID="lblTipo" Text='<%#Eval("TIPOD")%>'></asp:Label></td>
                  <td  style=" width:175px">
                     <table ><td>
                     <asp:CheckBox ID="chkHuellaEstado" Text="HUELLA" TextAlign="Left" Font-Bold="true" Font-Size="X-Small"  Width="100px"  Enabled="false" runat="server" />
                     </td>
                     <td>
                     <asp:CheckBox ID="chkFotoEstado" Text="FOTO" TextAlign="Left" Font-Bold="true" Font-Size="X-Small"  Width="75px"  Enabled="false" runat="server" />
                     </td>
                     </table>
                  </td>
                  <td id="DataColDeleteHuella" runat="server" style=" width:100px">
                  <table><tr>
                  <td >
                    <a style=" width:60px; font-weight:bold;"  id="a1" class="topopup"  onclick="return prepareObjectHuella('<%#Eval("NOMINA_ID")%>', '<%#Eval("NUMSOLICITUD")%>', '<%#Eval("CIPAS")%>', '<%#Eval("NOMBRE")%>', '<%#Eval("RUCCIPAS")%>', '<%#Eval("VAL_CNN")%>');">
                    <i></i>HUELLA</a> 
                  </td>
                  <td>  
                    <asp:CheckBox ID="chkValHuella" Font-Bold="true"  TextAlign="Left"  Font-Size="X-Small" Text="[]" Width="50px"  Enabled="false" runat="server" />
                  </td>
                  </tr>
                  </table>
                  </td>
                  <td id="DataColDeleteFoto" runat="server" style=" width:100px">
                  <table><tr>
                  <td >
                    <a style=" width:60px; font-weight:bold;"  id="a2" class="topopup"  onclick="return prepareObjectFoto('<%#Eval("NOMINA_ID")%>', '<%#Eval("NUMSOLICITUD")%>', '<%#Eval("CIPAS")%>', '<%#Eval("NOMBRE")%>', '<%#Eval("VAL_CNN")%>');">
                    <i></i>FOTO</a> 
                  </td>
                  <td>  
                    <asp:CheckBox ID="chkValFoto" Font-Bold="true" TextAlign="Left"  Font-Size="X-Small" Text="[]" Width="50px"  Enabled="false" runat="server" />
                  </td>
                  </tr></table>
                  </td>
                  <td class="nover"><%#Eval("NUMSOLICITUD")%></td>
                  <td class="nover"><%#Eval("IDSOLCOL")%></td>
                  <td class="nover"><%#Eval("NOMINA_ID")%></td>
                  <td><asp:Label Text='<%#Eval("CIPAS")%>' runat="server" id="lblcipas"/></td>
                  <td style=" width:300px"><%#Eval("NOMBRE")%></td>
                  <td><%#Eval("TIPOSANGRE")%></td>
                  <td style=" width:250px"><%#Eval("DIRECCIONDOM")%></td>
                  <td><%#Eval("TELFDOM")%></td>
                  <td style=" width:350px"><%#Eval("EMAIL")%></td>
                  <td style=" width:150px"><%#Eval("LUGARNAC")%></td>
                  <td style=" width:60px"><%#Eval("FECHANAC")%></td>
                  <td><%#Eval("CARGO")%></td>
                  <td><%#Eval("TIPOLICENCIA")%></td>
                  <td style=" width:80px"><%#Eval("FECHAEXPLICENCIA")%></td>
                  <td style=" width:60px"><asp:CheckBox runat="server" ForeColor="Red" Enabled="false" id="chkRevisado" Checked='<%#Eval("ESTADOCOL")%>'/></td>
                  <td style=" width:150px">
                  <asp:TextBox ID="tcomentario" runat="server" ForeColor="Red" Enabled="false"  Width="150px" ToolTip='<%#Eval("COMENTARIO")%>' Text='<%#Eval("COMENTARIO")%>' onkeypress="return soloLetras(event,'abcdefghijklnmñopqrstuvwxyz01234567890-_/ ',true)"></asp:TextBox>
                  </td>
                  <td style=" width:80px">
                    <a style=" width:80px" id="adjDoc" class="topopup"  onclick="redirectsol('<%# Eval("NUMSOLICITUD") %>', '<%# Eval("IDSOLCOL") %>', '<%#Eval("CIPAS")%>');">
                    <i></i> Ver Documentos </a>
                  </td>
                  <td class="nover"><%#Eval("RUCCIPAS")%></td>
                  <td class="nover"><%#Eval("VAL_CNN")%></td>
                 </tr>
                 </ItemTemplate>
                 <FooterTemplate>
                 </tbody>
                 </table>
                 </FooterTemplate>
         </asp:Repeater>
     </div>
     <div class="accion" >
      <div class="informativo" id="colector">
      <%--repeater aqui entes--%>
       </div>
     </div>
     </div>
 <div class="seccion">
       <div class="accion">
       <div class="informativo" style=" display:none">

      <table class="controles" cellspacing="0" cellpadding="1">
      <tr><td rowspan="2" class="inum"> <div class="number">2</div></td><td class="level1" > Documentación.</td></tr>
      <tr><td class="level2">Criterios de consulta.</td></tr>
      <tr><td><asp:TextBox Text="0" ID="lblCodigoUsuario" runat="server"></asp:TextBox></td></tr>
      </table>
     </div>
          <div class="colapser colapsa" style=" display:none"></div>
          <div style=" display:none">
       <table class="xcontroles" cellspacing="0" cellpadding="1">
       <%--<tr><th class="bt-bottom bt-right bt-left bt-top" colspan="4"> Criterios de consulta:</th></tr>--%>
       <tr>
        <td class="bt-bottom bt-right bt-left" >Número de Solicitud:</td>
        <td class="bt-bottom bt-right ">
             <asp:TextBox ID="txtsolicitud" runat="server" Width="120px" MaxLength="11"
             style="text-align: center"
             onkeypress="return soloLetras(event,'01234567890',true)"></asp:TextBox>
        </td>
       </tr>
        <tr style=" display:none">
         <td class="bt-bottom bt-right bt-left">Generados desde / hasta:</td>
         <td class="bt-bottom bt-right">
            <%--<a class="tooltip" >
            <span class="classic">Fecha desde.</span>--%>
            <asp:TextBox ID="tfechaini" runat="server" Width="120px" MaxLength="10" CssClass="datetimepicker"
             onkeypress="return soloLetras(event,'01234567890/')" style="text-align: center" 
                 onblur="valDate(this,true,valdate);" ClientIDMode="Static"></asp:TextBox>
            <%--</a>
            <a class="tooltip" >
            <span class="classic">Fecha hasta.</span>--%>
            <asp:TextBox ID="tfechafin" runat="server" ClientIDMode="Static" Width="120px" MaxLength="15" CssClass="datetimepicker"
             onkeypress="return soloLetras(event,'01234567890/')"  style="text-align: center" 
                  onblur="valDate(this,true,valdate);"></asp:TextBox>
            <%--</a>--%>
          </td>
        </tr>
       <tr style=" display:none">
         <td class="bt-bottom bt-right bt-left">Todas las facturas.</td>
         <td class="bt-bottom bt-right">
             <asp:CheckBox Text="" ID="chkTodos" runat="server" />
         </td>
       </tr>
       </table>
       <div class=" botonera">
            <img alt="loading.." src="../shared/imgs/loader.gif" id="Img1" class="nover"/>
            <asp:Button ID="btbuscar" runat="server" Text="Iniciar la búsqueda" 
            onclick="btbuscar_Click"/>
       </div>
       </div>
       <div runat="server" id="div2"  style=" display:none">
        <div class="botonera" style=" height:100%; overflow:auto">
       <div class="msg-alerta" id="alertapagado" runat="server" style=" display:none" ></div>
                 <div class="bokindetalle">
                 <table id="table1" class="controles" cellpadding="1" cellspacing="0">
                 <asp:GridView runat="server" id="gvComprobantes" class="tabRepeat" AutoGenerateColumns="False" Width="100%">
                 <Columns>
                 <asp:TemplateField HeaderText="# Solicitud" ItemStyle-Width="80px">
                      <ItemTemplate>
                          <asp:Label runat="server" Text='<%#Eval("NUMSOLICITUD")%>'  Width="50px" ID="lblIdSolicitud" />
                      </ItemTemplate>
                  </asp:TemplateField>
                  <asp:TemplateField HeaderText="Tipo de Empresa" Visible="False">
                      <ItemTemplate>
                          <asp:Label runat="server" Text='<%#Eval("TIPOEMPRESA")%>' ID="lblTipoEmpresa" />
                      </ItemTemplate>
                  </asp:TemplateField>
                  <asp:TemplateField HeaderText="Tipo de Solicitud">
                      <ItemTemplate>
                          <asp:Label runat="server" Text='<%#Eval("TIPOSOLICITUD")%>' ID="lblTipoSolicitud" />
                      </ItemTemplate>
                  </asp:TemplateField>
                  <asp:TemplateField HeaderText="Documento/Estado">
                      <ItemTemplate>
                          <asp:Label runat="server" Text='<%#Eval("ESTADO")%>' ID="lblCodEstado" />
                      </ItemTemplate>
                  </asp:TemplateField>
                  <asp:TemplateField HeaderText="">
                      <ItemTemplate>
                          <a href='<%#Eval("RUTADOCUMENTO") %>' style=" width:80px" class="topopup" target="_blank">
                          <i></i> Ver Documento </a>
                      </ItemTemplate>
                  </asp:TemplateField>
                 </Columns>
                 </asp:GridView>
                 </table>
                </div>
        </div>
        </div>
        <div class="cataresult" >
        <%--<asp:UpdatePanel ID="upresult" runat="server" ChildrenAsTriggers="true">
        <ContentTemplate>--%>
        <%--script type="text/javascript">            Sys.Application.add_load(BindFunctions);</script>--%>
        <input id="idlin" type="hidden" runat="server" clientidmode="Static" />
        <input id="diponible" type="hidden" runat="server" clientidmode="Static" />
        <div id="xfinderpagado" runat="server">
        <%--<div class="separator">Información disponible:</div>--%>
        <div class="findresult" >
        <div runat="server" id="divseccion2" style=" display:none">
        <div class="informativo">
      <table class="controles" cellspacing="0" cellpadding="1">
      <tr><td rowspan="2" class="inum"> <div class="number">3</div></td><td class="level1" > 
          Datos del Permiso de Acceso.</td></tr>
      <tr><td class="level2">Dcocumentos a revisar.</td></tr>
      </table>
     </div>
        <div class="colapser colapsa"></div>
        <div runat="server" id="divcabecera" >
       <div class="accion" runat="server" id="divnumfactura">
        <table class="xcontroles" cellspacing="0" cellpadding="1">
       <tr>
        <td class="bt-bottom bt-right bt-left" >Número de Factura:</td>
        <td class="bt-bottom ">
             <asp:TextBox ID="txtnumfactura" runat="server" Width="200px" MaxLength="30"
             style="text-align: center" onblur="checkcaja(this,'valnumfac',true);"
             onkeypress="return soloLetras(event,'01234567890-',true)"></asp:TextBox>
        </td>
        <td class="bt-bottom bt-right validacion "><span id="valnumfac" class="validacion"> * obligatorio</span></td>
       </tr>
       <tr style=" display:none">
        <td class="bt-bottom bt-right bt-left" >Permiso:</td>
        <td class="bt-bottom ">
           <asp:DropDownList ID="dptipoevento" runat="server" Width="400px" AutoPostBack="false" OnSelectedIndexChanged="dptipoevento_SelectedIndexChanged"
                onchange="valdltipsol(this, valtipeve);">
                 <asp:ListItem Value="0">* Seleccione permiso *</asp:ListItem>
          </asp:DropDownList>
        </td>
        <td class="bt-bottom bt-right validacion "><span id='valtipeve' class="validacion" > * obligatorio</span></td>
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
        <tr>
        <td class="bt-bottom bt-right bt-left" >Area:</td>
        <td class="bt-bottom ">
           <asp:DropDownList runat="server" Width="400px" ID="ddlAreaOnlyControl" onchange="valdltipsol(this, valareaoc);">
                 <asp:ListItem Value="0">* Elija *</asp:ListItem>
          </asp:DropDownList>
        </td>
        <td class="bt-bottom bt-right validacion "><span id='valareaoc' class="validacion" > * obligatorio</span></td>
        </tr>
        <tr style=' display:none'>
        <td class="bt-bottom bt-right bt-left" >Departamento:</td>
        <td class="bt-bottom ">
           <asp:DropDownList runat="server" Width="400px" ID="ddlDepartamentoOnlyControl" onchange="valdltipsol(this, valdepoc);">
                 <asp:ListItem Value="0">* Elija *</asp:ListItem>
           </asp:DropDownList>
        </td>
        <td class="bt-bottom bt-right validacion "><span id='valdepoc' class="validacion" > * obligatorio</span></td>
        </tr>
        <tr style=" display:none">
        <td class="bt-bottom bt-right bt-left" >Cargo:</td>
        <td class="bt-bottom ">
           <asp:DropDownList runat="server" Width="350px" ID="ddlCargoOnlyControl" onchange="valdltipsol(this, valcargoac);">
                 <asp:ListItem Value="0">* Elija *</asp:ListItem>
           </asp:DropDownList>
        </td>
        <td class="bt-bottom bt-right validacion "><span id='valcargoac' class="validacion" > * obligatorio</span></td>
        </tr>
        <tr>
        <td class="bt-bottom bt-right bt-left" >Turno:</td>
        <td class="bt-bottom ">
           <asp:DropDownList runat="server" Width="400px" ID="ddlTurnoOnlyControl">
                 <asp:ListItem Value="0">* Elija *</asp:ListItem>
           </asp:DropDownList>
        </td>
        <td class="bt-bottom bt-right validacion "></td>
        </tr>
       </table>
        </div>
        </div>
        </div>
         <div runat="server" id="divseccion3" style=" display:none">
       <%--<div class="informativo">--%>
       <table>
       <tr><td rowspan="2" class="inum"> <div class="number">4</div></td><td class="level1" >Observación en caso de rechazo.</td></tr>
       <tr><td class="level2">
         Confirme que los datos sean correctos.
       </td></tr>
       </table>
       <div class="colapser colapsa"></div>
       <div class="accion" runat="server" id="div3">
       <table class="xcontroles" cellspacing="0" cellpadding="1" style=" font-size:small" >
       <tr style=" background-color:White">
         <%--<td class="bt-bottom  bt-right bt-left" style=" width:155px" >Motivo de rechazo:</td>--%>
         <td class="bt-bottom bt-left bt-right" style=" background-color:White">
           <asp:TextBox ID="txtmotivorechazo" ForeColor="Red" runat="server" Width="99%" MaxLength="500"
             style="text-align: center"  onkeypress="return soloLetras(event,'abcdefghijklnmñopqrstuvwxyz01234567890-_/ ',true)"></asp:TextBox>
       </td>
       </tr>
       </table>
       </div>
       <%--</div>--%>
        </div>
                
        <div id="sinresultado" runat="server" class="msg-info"></div>
        <div class="botonera" runat="server" id="botonera">
        <img alt="loading.." src="" id="Img2" class="nover" />
        <asp:Button Text="Rechazar" ID="btnRechazar" Visible="false" runat="server" Width="125px" 
                onclick="btnRechazar_Click" OnClientClick="return prepareObjRechazo();"
                ToolTip="Rechaza la solicitud."/>
        <asp:Button ID="btsalvar" runat="server" Text="Crear Permiso(s)" Width="160px"
                OnClientClick="return prepareObject();" onclick="btsalvar_Click" 
                ToolTip="Crea el Permiso de Acceso."/>
        </div>
        
            <%--</ContentTemplate>
                         <Triggers>
                             <asp:AsyncPostBackTrigger ControlID="btbuscar" />
                         </Triggers>
            </asp:UpdatePanel>--%>
        </div>
       </div>
 </div>
 </div>
 </div>
 </div>
 </div> 
</form>
    <script src="../Scripts/pages.js" type="text/javascript"></script>
    <script src="../../Scripts/credenciales.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.tablesorter.min.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.tablesorter.pager.js" type="text/javascript"></script>
    
    <script type="text/javascript">
        var codeuserac = "0";
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
        function popupCallback(objeto, catalogo) {

        }

        function clear() {
            document.getElementById('numbook').textContent = '...';
            document.getElementById('nbrboo').value = '';
        }
        function clear2() {
            document.getElementById('txtfecha').textContent = '...';
            document.getElementById('xfecha').value = '';
        }
        function prepareObjRechazo() {
            lista = [];
            if (confirm('¿Esta seguro de rechazar la confirmaciòn de pago?') == false) {
                return false;
            }
            var vals = document.getElementById('<%=txtmotivorechazo.ClientID %>').value;
            if (vals == '' || vals == null || vals == undefined) {
                alert('* Escriba la observación de rechazo. *');
                document.getElementById('<%=txtmotivorechazo.ClientID %>').focus();
                document.getElementById('<%=txtmotivorechazo.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:98%;";
                //document.getElementById("loader").className = 'nover';
                return false;
            }
            return true;
        }
        function prepareObject() {
            try {
                lista = [];
                if (confirm('¿Esta seguro de crear el permiso de acceso temporal?') == false) {
                    return false;
                }
                var vals = document.getElementById('<%=txtsolicitud.ClientID %>').value;
                if (vals == '' || vals == null || vals == undefined) {
                    alert('*Consultar Comprobante de Pago: *\n *Escriba el numero de solicitud*');
                    document.getElementById('<%=txtsolicitud.ClientID %>').focus();
                    document.getElementById('<%=txtsolicitud.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:200px;";
                    //document.getElementById("loader").className = 'nover';
                    return false;
                }
                var vals = document.getElementById('<%=txtnumfactura.ClientID %>').value;
                if (vals == '' || vals == null || vals == undefined) {
                    alert('*Escriba el Número de factura *');
                    document.getElementById('<%=txtnumfactura.ClientID %>').focus();
                    document.getElementById('<%=txtnumfactura.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:200px;";
                    //document.getElementById("loader").className = 'nover';
                    return false;
                }
                var vals = document.getElementById('<%=ddlAreaOnlyControl.ClientID %>').options[document.getElementById("<%=ddlAreaOnlyControl.ClientID%>").selectedIndex].text;
                if (vals == '* Elija *') {
                    alert('*Seleccione el Area *');
                    document.getElementById('<%=ddlAreaOnlyControl.ClientID %>').focus();
                    document.getElementById('<%=ddlAreaOnlyControl.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:400px;";
                    //document.getElementById("loader").className = 'nover';
                    return false;
                }
//                var vals = document.getElementById('<%=ddlDepartamentoOnlyControl.ClientID %>').options[document.getElementById("<%=ddlDepartamentoOnlyControl.ClientID%>").selectedIndex].text;
//                if (vals == '* Elija *') {
//                    alert('*Seleccione el Departamento *');
//                    document.getElementById('<%=ddlDepartamentoOnlyControl.ClientID %>').focus();
//                    document.getElementById('<%=ddlDepartamentoOnlyControl.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:400px;";
//                    //document.getElementById("loader").className = 'nover';
//                    return false;
//                }
//                var vals = document.getElementById('<%=ddlTurnoOnlyControl.ClientID %>').value;
//                if (vals == 0) {
//                    alert('*Seleccione el Turno *');
//                    document.getElementById('<%=ddlTurnoOnlyControl.ClientID %>').focus();
//                    document.getElementById('<%=ddlTurnoOnlyControl.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:400px;";
//                    //document.getElementById("loader").className = 'nover';
//                    return false;
//                }
                var vals = document.getElementById('<%=txtfecing.ClientID %>').value;
                if (vals == '' || vals == null || vals == undefined) {
                    alert('*Seleccione la Fecha de Ingreso *');
                    document.getElementById('<%=txtfecing.ClientID %>').focus();
                    document.getElementById('<%=txtfecing.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:200px;";
                    //document.getElementById("loader").className = 'nover';
                    return false;
                }
                var vals = document.getElementById('<%=txtfecsal.ClientID %>').value;
                if (vals == '' || vals == null || vals == undefined) {
                    alert('*Seleccione la Fecha de Caducidad *');
                    document.getElementById('<%=txtfecsal.ClientID %>').focus();
                    document.getElementById('<%=txtfecsal.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:200px;";
                    //document.getElementById("loader").className = 'nover';
                    return false;
                }

                var tbl = document.getElementById('tabla');
                if (tbl.rows.length == 1) {
                    return false;
                }
                /*
                for (var i = 1; i < tbl.rows.length; i++) {
                    alert(tbl.rows[i].cells[8].textContent);
                }
                return false;
                */
                var banderaconductor = false;
                for (var f = 0; f < tbl.rows.length; f++) {
                    var celColect = tbl.rows[f].getElementsByTagName('td');
                    if (celColect != undefined && celColect != null && celColect.length > 0) {
                        
                        if (celColect[6].getElementsByTagName('input')[0].checked == false && celColect[7].getElementsByTagName('input')[0].checked == true) {
                            alert('*Se necesita que se capture la huella del conductor:\n' + tbl.rows[f].cells[7].textContent + ' - ' + tbl.rows[f].cells[8].textContent);
                            return false;
                        }
                        if (celColect[6].getElementsByTagName('input')[0].checked == true && celColect[7].getElementsByTagName('input')[0].checked == false) {
                            alert('*Se necesita que se capture la foto del conductor:\n' + tbl.rows[f].cells[7].textContent + ' - ' + tbl.rows[f].cells[8].textContent);
                            return false;
                        }
                        /*
                        var tdetalle = {
                        huella: celColect[6].getElementsByTagName('input')[0].checked
                        };
                        this.lista.push(tdetalle);
                        */
                    }
                }
                return true;
            } catch (e) {
                alert('Lamentamos comunicarle que ha ocurrido el siguiente problema:\n' + e.Message);
            }
        }

        function openPop() {
            var bo = document.getElementById('nbrboo').value;
            if (bo == undefined || bo == '' || bo == null) {
                alert('Por favor seleccione el booking primero');
                return;
            }
            window.open('../catalogo/calendarioConsolidacion.aspx?bk=' + bo, 'name', 'width=850,height=880')
        }
        function redirectsol(val, val2, val3) {
            var caja = val;
            var caja2 = val2;
            var caja3 = val3;
            window.open('../consulta/documentos-solicitud-colaborador/?numsolicitud=' + caja + '&idsolcol=' + caja2 + '&cedula=' + caja3)
        }

        function prepareObjectHuella(user, numsolicitud, cedula, nombres, ruc, cnn) {
            if (confirm('¿Esta seguro de capturar la huella?') == false) {
                return false;
            }
            if (!validaConductor(user, numsolicitud, cedula, nombres, ruc)) {
                return false;
            }            
            if (user == '0') {
                CallHuella(cedula, cnn);
            }
            else {
                CallHuella(cedula, cnn);
            }          

            return true;
        }

        function prepareObjectFoto(user, numsolicitud, cedula, nombres, cnn) {
            if (confirm('¿Esta seguro de capturar la foto?') == false) {
                return false;
            }
            if (!validaFotoConductor(user, numsolicitud, cedula, nombres)) {
                return false;
            }
            if (user == '0') {
                CallCamara(cedula, cnn);
            }
            else {
                CallCamara(cedula, cnn);
            }    
            return true;
        }

        function CallHuella(user, cnn) {
            var err;
            try // Exception handling
            {
                var myobj = new ActiveXObject("Z_OnlyControl_2018.UC_Biometric_2018");
                var result = myobj.Only_RegistraHuella(user, true, false);
                lista = [];
                var tbl = document.getElementById('tabla');
                if (tbl.rows.length == 1) {
                    return false;
                }
                var separador = ":";
                var arregloDeSubCadenas = result.split(separador);
                if (arregloDeSubCadenas[0] == 'OK') {
                    for (var f = 0; f < tbl.rows.length; f++) {
                        var celColect = tbl.rows[f].getElementsByTagName('td');
                        if (celColect != undefined && celColect != null && celColect.length > 0) {
                            if (tbl.rows[f].cells[6].textContent == '0') {
                                if (!validaHuella(user)) {
                                    return false;
                                }
                                else {
                                    celColect[6].getElementsByTagName('label')[0].innerHTML = '[OK]';
                                    celColect[6].getElementsByTagName('input')[0].checked = true;
                                }
                            }
                            else {
                                if (tbl.rows[f].cells[7].textContent == user) {
                                    if (!validaHuella(user)) {
                                        return false;
                                    }
                                    else {
                                        celColect[6].getElementsByTagName('label')[0].innerHTML = '[OK]';
                                        celColect[6].getElementsByTagName('input')[0].checked = true;
                                    }
                                }
                            }
                            /*
                            var tdetalle = {
                            huella: celColect[6].getElementsByTagName('input')[0].checked
                            };
                            this.lista.push(tdetalle);
                            */
                        }
                    }
                }
                else {
                    alert(result);
                }
                /*
                for (var n = 0; n < this.lista.length; n++) {
                alert(lista[n].huella);
                }
                */
                return (true);
            }
            catch (e) {
                alert(e.message);
                return (false);
            }
        }

        function CallCamara(user, cnn) {
            var err;
            try // Exception handling
            {
                var myobj = new ActiveXObject("Z_OnlyControl_2018.UC_Biometric_2018");
                var result = myobj.Only_CapturaFoto(user);
                lista = [];
                var tbl = document.getElementById('tabla');
                if (tbl.rows.length == 1) {
                    return false;
                }
                var separador = ":";
                var arregloDeSubCadenas = result.split(separador);
                if (arregloDeSubCadenas[0] == 'OK') {
                    for (var f = 0; f < tbl.rows.length; f++) {
                        var celColect = tbl.rows[f].getElementsByTagName('td');
                        if (celColect != undefined && celColect != null && celColect.length > 0) {
                            if (tbl.rows[f].cells[6].textContent == '0') {
                                if (!validaFoto(user)) {
                                    return false;
                                }
                                else {
                                    celColect[7].getElementsByTagName('label')[0].innerHTML = '[OK]';
                                    celColect[7].getElementsByTagName('input')[0].checked = true;
                                }
                            }
                            else {
                                if (tbl.rows[f].cells[7].textContent == user) {
                                    if (!validaFoto(user)) {
                                        return false;
                                    }
                                    else {
                                        celColect[7].getElementsByTagName('label')[0].innerHTML = '[OK]';
                                        celColect[7].getElementsByTagName('input')[0].checked = true;
                                    }
                                }
                            }
                            /*
                            var tdetalle = {
                            huella: celColect[6].getElementsByTagName('input')[0].checked
                            };
                            this.lista.push(tdetalle);
                            */
                        }
                    }
                }
                else {
                    alert(result);
                }
                /*
                for (var n = 0; n < this.lista.length; n++) {
                alert(lista[n].huella);
                }
                */
                return (true);
            }
            catch (e) {
                alert(e.message);
                return (false);
            }
        }

        function validaHuella(user) {
            var pageUrl = '<%=ResolveUrl("consulta_solicitud_colaborador.aspx")%>'
            var validametodo = false;
            $.ajax({
                type: "POST",
                url: pageUrl + "/IsAvailableHuella",
                data: '{user: "' + user + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: function OnSuccess(response) {
                    if (response.d == "1") {
                        validametodo = true;
                    }
                    else {
                        if (response.d != "0") {
                            alert(response.d);
                        }
                        validametodo = false;
                    }
                },
                failure: function (response) {
                    alert(response.d);
                    validametodo = false;
                }
            });
            return validametodo;
        }

        function validaFoto(user) {
            var pageUrl = '<%=ResolveUrl("consulta_solicitud_colaborador.aspx")%>'
            var validametodo = false;
            $.ajax({
                type: "POST",
                url: pageUrl + "/IsAvailableFoto",
                data: '{user: "' + user + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: function OnSuccess(response) {
                    if (response.d == "1") {
                        validametodo = true;
                    }
                    else {
                        if (response.d != "0") {
                            alert(response.d);
                        }
                        validametodo = false;
                    }
                },
                failure: function (response) {
                    alert(response.d);
                    validametodo = false;
                }
            });
            return validametodo;
        }

        function validaConductor(user, numsolicitud, cedula, nombres, ruc) {
            var fechaingreso = document.getElementById('<%=txtfecing.ClientID %>').value;
            var fechasalida = document.getElementById('<%=txtfecsal.ClientID %>').value;
            var ddlAreaOnlyControl = document.getElementById('<%=ddlAreaOnlyControl.ClientID %>').options[document.getElementById("<%=ddlAreaOnlyControl.ClientID%>").selectedIndex].value;
            //alert(fechaingreso + '-' + fechasalida + '-' + ddlAreaOnlyControl);
            //return false;
            var pageUrl = '<%=ResolveUrl("consulta_solicitud_colaborador.aspx")%>'
            var validametodo = false;
            var scodeuserac = "";
            $.ajax({
                type: "POST",
                url: pageUrl + "/IsAvailableRAC",
                data: '{user: "' + user + '", numsolicitud: "' + numsolicitud + '", fechaingreso: "' + fechaingreso + '", fechasalida: "' + fechasalida + '", ddlAreaOnlyControl: "' + ddlAreaOnlyControl + '", cedula: "' + cedula + '", conductor: "' + cedula + ' - ' + nombres + '", ruc: "' + ruc + '" }',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: function OnSuccess(response) {
                    var separador = ",";
                    var arregloDeSubCadenas = response.d.split(separador);
                    if (arregloDeSubCadenas.length == 1) {
                        if (response.d == "0") {
                            validametodo = true;
                        }
                        else {
                            alert(response.d);
                            validametodo = false;
                        }
                    }
                    else {
                        if (arregloDeSubCadenas[0] == "0") {
                            document.getElementById('<%=lblCodigoUsuario.ClientID %>').value = arregloDeSubCadenas[1];
                            validametodo = true;
                        }
                        else {
                            alert(response.d);
                            validametodo = false;
                        }
                    }
                },
                failure: function (response) {
                    alert(response.d);
                    validametodo = false;
                }
            });
            codeuserac = scodeuserac;
            return validametodo;
        }

        function validaFotoConductor(user, numsolicitud, cedula, nombres) {
            var fechaingreso = document.getElementById('<%=txtfecing.ClientID %>').value;
            var fechasalida = document.getElementById('<%=txtfecsal.ClientID %>').value;
            var ddlAreaOnlyControl = document.getElementById('<%=ddlAreaOnlyControl.ClientID %>').options[document.getElementById("<%=ddlAreaOnlyControl.ClientID%>").selectedIndex].value;
            //alert(fechaingreso + '-' + fechasalida + '-' + ddlAreaOnlyControl);
            //return false;
            var pageUrl = '<%=ResolveUrl("consulta_solicitud_colaborador.aspx")%>'
            var validametodo = false;
            $.ajax({
                type: "POST",
                url: pageUrl + "/IsAvailableFotoRAC",
                data: '{user: "' + user + '", numsolicitud: "' + numsolicitud + '", fechaingreso: "' + fechaingreso + '", fechasalida: "' + fechasalida + '", ddlAreaOnlyControl: "' + ddlAreaOnlyControl + '", cedula: "' + cedula + '", conductor: "' + cedula + ' - ' + nombres + '" }',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: function OnSuccess(response) {
                    if (response.d == "0") {
                        validametodo = true;
                    }
                    else {
                        alert(response.d);
                        validametodo = false;
                    }
                },
                failure: function (response) {
                    alert(response.d);
                    validametodo = false;
                }
            });
            return validametodo;
        }
    </script>
    </body>
    </html>
    