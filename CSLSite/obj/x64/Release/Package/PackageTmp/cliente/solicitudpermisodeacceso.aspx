    <%@ Page Language="C#" MasterPageFile="~/site.Master" AutoEventWireup="true" Title="Consulta de Colaboradores" MaintainScrollPositionOnPostback="true"
    CodeBehind="solicitudpermisodeacceso.aspx.cs" Inherits="CSLSite.cliente.solicitudpermisodeacceso" %>
<%@ MasterType VirtualPath="~/site.Master" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="placehead" runat="server">

    <link href="../img/favicon2.png" rel="icon"/>
  <link href="../img/icono.png" rel="apple-touch-icon"/>
  <link href="../css/bootstrap.min.css" rel="stylesheet"/>
  <link href="../css/dashboard.css" rel="stylesheet"/>
  <link href="../css/icons.css" rel="stylesheet"/>
  <link href="../css/style.css" rel="stylesheet"/>
  <link href="https://fonts.googleapis.com/css2?family=Roboto:wght@300;400;500;700&display=swap" rel="stylesheet"/>

<%--      <link href="../shared/estilo/Reset.css" rel="stylesheet" />
 
     <link href="../shared/estilo/tables.css" rel="stylesheet" type="text/css" />
     <link href="../shared/estilo/info.css" rel="stylesheet" type="text/css" />--%>

    <link href="../shared/estilo/jquery.datetimepicker.css" rel="stylesheet" type="text/css" />
   <script src="../Scripts/jquery.datetimepicker.js" type="text/javascript"></script>

    <!--external css-->
  <link href="../lib/font-awesome/css/font-awesome.css" rel="stylesheet" />
  <link rel="stylesheet" type="text/css" href="../lib/gritter/css/jquery.gritter.css" />

    <!--mensajes-->
  <link href="https://cdn.jsdelivr.net/npm/alertifyjs@1.11.0/build/css/alertify.min.css" rel="stylesheet"/>

    <%-- <link href="../shared/estilo/tables.css" rel="stylesheet" type="text/css" />
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
        </style>--%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="placebody" runat="server">
    <input id="zonaid" type="hidden" value="2" />
    <asp:ToolkitScriptManager ID="tkcal" EnableScriptGlobalization="true" runat="server">
    </asp:ToolkitScriptManager>
    <noscript>
        <meta http-equiv="refresh" content="0; url=sinsoporte.htm" />
    </noscript>

    <div class="mt-4">         
        <nav class="mt-4" aria-label="breadcrumb">
          <ol class="breadcrumb">
            <li class="breadcrumb-item" id="opcion_principal" runat="server"><a href="#">MCA - Módulo de Control de Acceso</a></li>
            <li class="breadcrumb-item active" aria-current="page" id="sub_opcion" runat="server"> Solicitar acceso a la terminal</li>
          </ol>
        </nav>
      </div>

 <div class="dashboard-container p-4" id="cuerpo" runat="server">

         <div class="form-row">

             <div class="form-title col-md-12">
             <a class="btn btn-outline-primary mr-4" href="#" target="_blank" runat="server" id="aprint" clientidmode="Static" >1</a>
             <a class="level1" target="_blank" runat="server" id="a1" clientidmode="Static" >Datos del permisos</a>
                <div class="colapser colapsa">
                </div>
             </div>

                    <div class="form-title col-md-12 d-flex">
                     <label for="inputEmail4">Criterios de consulta para Colaboradores:</label>
                        </div> 

                     <div class="form-group col-md-6 d-flex">
                         <label class="radiobutton-container" >
                               Cedula<input id="rbcedula" runat="server"  checked="true" type="radio" name="deck" value="ced" clientidmode="Static"/>
                                <span class="checkmark"></span></label>
                        <label for="inputEmail4">&nbsp;&nbsp;&nbsp;&nbsp;</label>
                         <label class="radiobutton-container" > 
                               Nombre(s)<input id="rbnombres" runat="server" type="radio" name="deck" value="nom" clientidmode="Static"/>
                                <span class="checkmark"></span></label>
                        <label for="inputEmail4">&nbsp;&nbsp;&nbsp;&nbsp;</label>
                         <label class="radiobutton-container" > 
                               Apellido(s)<input id="rbapellidos" runat="server" type="radio" name="deck" value="ape" clientidmode="Static"/>
                                <span class="checkmark"></span></label>
                        <%--<label for="inputEmail4">&nbsp;&nbsp;&nbsp;&nbsp;</label>--%>
                         </div> 
                         <div class="form-group col-md-6 d-flex">
                         <asp:TextBox id="txtcriterioconsulta" runat="server" class="form-control" Style="text-transform :uppercase"/> 
                             <label for="inputEmail4">&nbsp;&nbsp;</label>
                            <asp:Button ID="btnBuscar" runat="server" class="btn btn-outline-primary mr-4"
                                OnClientClick="return fvalidaCriterios();" onclick="btnBuscar_Click" Text="Buscar" clientidmode="Static"></asp:Button> 

                     </div> 
                
             <div class="form-group col-md-6">
                 <label for="inputAddress">Cédula del Colaborador:<span style="color: #FF0000; font-weight: bold;"> </span></label>
             </div>
             <div class="form-group col-md-6">
                           <asp:TextBox ID="txtci" runat="server" class="form-control"
             style="text-align: center" onblur="cajaControl(this);"
             onkeypress="return soloLetras(event,'01234567890',true)"></asp:TextBox>

            </div>

             <div class="form-group col-md-6">
                 <label for="inputAddress">Nombres del Colaborador:<span style="color: #FF0000; font-weight: bold;"> </span></label>
            </div>
             <div class="form-group col-md-6">
                           <asp:TextBox ID="txtNombres" runat="server" class="form-control"
             style="text-align: center; text-transform :uppercase" onblur="checkcajalarge(this,'valnombres',true);"
             onkeypress="return soloLetras(event,'abcdefghijklnmñopqrstuvwxyz01234567890 .',true)" ></asp:TextBox>
             <span id="valnombres" class="validacion"></span>
            </div>

             <div class="form-group col-md-6">
                 <label for="inputAddress">Apellidos del Colaborador:<span style="color: #FF0000; font-weight: bold;"> </span></label>
             </div>
             <div class="form-group col-md-6">
                 <asp:TextBox ID="txtApellidos" runat="server" CssClass="form-control"
                     style="text-align: center; text-transform :uppercase" onblur="checkcajalarge(this,'valapellidos',true);"
                     onkeypress="return soloLetras(event,'abcdefghijklnmñopqrstuvwxyz01234567890 .',true)" ></asp:TextBox>
             <span id="valapellidos" class="validacion"></span>
            </div>

             <div class="form-group col-md-6" style=" display:none">
                 <label for="inputAddress">Área Destino/Actividad:<span style="color: #FF0000; font-weight: bold;"> *</span></label>

                <asp:DropDownList runat="server" ID="ddlActividadOnlyControl" CssClass="form-control" 
                        onchange="valdropdownlist(this, valareaoc);" AutoPostBack="true"
                        onselectedindexchanged="ddlActividadOnlyControl_SelectedIndexChanged">
                        <asp:ListItem Value="0">* Elija *</asp:ListItem>
                    </asp:DropDownList>
             <span id="valareaoc" class="validacion"></span>
            </div>

             <div class="form-group col-md-6">
                 <label for="inputAddress">Fecha de Ingreso:<span style="color: #FF0000; font-weight: bold;"> *</span></label>
            </div>
             <div class="form-group col-md-6">
                 <asp:TextBox ID="txtfecing" runat="server" MaxLength="10" CssClass="datetimepicker form-control"
                 onkeypress="return soloLetras(event,'01234567890/')" style="text-align: center" 
                     onblur="checkcaja(this,'valfecing',true);" ClientIDMode="Static"></asp:TextBox> 
                 <span id="valfecing" class="validacion"></span>
            </div>

             <div class="form-group col-md-6">
                 <label for="inputAddress">Fecha de Caducidad:<span style="color: #FF0000; font-weight: bold;"> *</span></label>
            </div>
             <div class="form-group col-md-6">
                  <asp:TextBox ID="txtfecsal" runat="server" ClientIDMode="Static"  MaxLength="15" CssClass="datetimepicker form-control"
                    onkeypress="return soloLetras(event,'01234567890/')"  style="text-align: center" 
                    onblur="checkcaja(this,'valfecsal',true);"></asp:TextBox>
                 <span id="valfecsal" class="validacion"></span>
            </div>

             <div class="form-group col-md-12 d-flex">
                 <label for="inputAddress">Nota:<span style="color: #FF0000; font-weight: bold;"> </span></label>
                           <asp:TextBox ID="txtNota" runat="server" MaxLength="3000" TextMode="MultiLine" Heigth="50px" CssClass="form-control" style="overflow:auto;resize:none"
                              onkeypress="return soloLetras(event,'abcdefghijklnmñopqrstuvwxyz01234567890 /_-.',true)"/>
            </div>

             <div class="form-group col-md-12">
                            <asp:Button ID="btnAgregar" runat="server" Text="Agregar" class="btn btn-outline-primary mr-4"
                                OnClientClick="return validaCabecera();" onclick="btnAgregar_Click"/>
                     <img alt="loading.." src="../shared/imgs/loader.gif" id="loader" class="nover"  />
                 <span id="imgagrega" class="fa fa-plus-square-0"></span>
             </div> 

                    
    <%--     <tr style=" display:none"><td class="bt-bottom  bt-right bt-left" style=" width:155px;">Cargo del Empleado:</td>
         <td class="bt-bottom">
             <asp:TextBox 
             style="text-align: center" 
             ID="txtcargo" runat="server" ClientIDMode="Static" width="200px" MaxLength="500"
             onkeypress="return soloLetras(event,'abcdefghijklnmñopqrstuvwxyz01234567890 /_-.')" 
             onblur="checkcaja(this,'valcargo',true);"></asp:TextBox>
         </td>
         <td class="bt-bottom bt-right validacion"><span id="valcargo" class="validacion"> * obligatorio</span></td>
         </tr>--%>

            </div>
     <div class="form-row">
                    <div class="informativo" id="colector">

              <asp:GridView runat="server" id="gvColaboradores" class="table table-bordered invoice" AutoGenerateColumns="False" Width="100%" 
                       onrowcommand="gvColaboradores_RowCommand"  onrowdeleting="gvColaboradores_RowDeleting">
              <Columns>
                  <asp:TemplateField HeaderText="Cedula">
                      <ItemTemplate>
                          <asp:Label ID="lcedula"  ToolTip='<%# Eval("Cedula") %>' runat="server" Text='<%# Eval("[Cedula]") %>'
                          ></asp:Label>
                      </ItemTemplate>
                  </asp:TemplateField>
                  <asp:TemplateField HeaderText="Nombres y Apellidos">
                      <ItemTemplate>
                          <asp:Label ID="lnombres"  ToolTip='<%# Eval("NombresApellidos") %>' runat="server" Text='<%# Eval("[NombresApellidos]") %>'
                          ></asp:Label>
                      </ItemTemplate>
                  </asp:TemplateField>
                  <asp:TemplateField HeaderText="Área Destino/Actividad">
                      <ItemTemplate>
                          <asp:Label ID="larea" ToolTip='<%# Eval("Area") %>' runat="server" Text='<%# Eval("[Area]") %>' ></asp:Label>
                      </ItemTemplate>
                  </asp:TemplateField>
                  <asp:TemplateField HeaderText="Fecha Ingreso">
                      <ItemTemplate>
                          <asp:Label ID="lfecini" runat="server" ToolTip='<%# Eval("FechaInicio") %>' Text='<%# Eval("[FechaInicio]") %>'></asp:Label>
                      </ItemTemplate>
                  </asp:TemplateField>
                  <asp:TemplateField HeaderText="Fecha Caducidad">
                      <ItemTemplate>
                          <asp:Label ID="lfeccad" runat="server" ToolTip='<%# Eval("FechaCaducidad") %>' Text='<%# Eval("[FechaCaducidad]") %>'></asp:Label>
                      </ItemTemplate>
                  </asp:TemplateField>
                  <asp:TemplateField HeaderText="Nota">
                      <ItemTemplate>
                      <div style="overflow-y:scroll; overflow-x: hidden; height:30px; text-align:center">
                          <asp:Label ID="tnota"  style="text-transform :uppercase" runat="server" ToolTip='<%# Eval("[Nota]") %>' Text='<%# Eval("[Nota]") %>'></asp:Label>
                      </div>
                      </ItemTemplate>
                  </asp:TemplateField>
                  <asp:TemplateField HeaderText="IdPermiso" Visible="false">
                      <ItemTemplate>
                          <asp:Label ID="lidpermiso" runat="server" Text='<%# Eval("[IdPermiso]") %>'></asp:Label>
                      </ItemTemplate>
                  </asp:TemplateField>
                  <asp:TemplateField HeaderText="Permiso" Visible="false">
                      <ItemTemplate>
                          <asp:Label ID="lpermiso" runat="server" Text='<%# Eval("[Permiso]") %>'></asp:Label>
                      </ItemTemplate>
                  </asp:TemplateField>
                  <asp:TemplateField ShowHeader="False">
                        <ItemTemplate>
                            <asp:Button ID="btnDelete" runat="server" CausesValidation="False" class="btn btn-outline-primary mr-4"
                                CommandName="Delete" Text="Eliminar" />
                        </ItemTemplate>
                  </asp:TemplateField>
              </Columns>
          </asp:GridView>

       </div>
        <asp:HiddenField ID="hfCustomerId" runat="server" />
        <asp:HiddenField ID="hfpermisopermanente" runat="server" />
     

             <div class="form-group col-md-12">
             <a class="btn btn-outline-primary mr-4" href="#" target="_blank" runat="server" id="a3" clientidmode="Static" >2</a>
             <a class="level1" target="_blank" runat="server" id="a4" clientidmode="Static" >Subir documentos</a>
             </div>

    <div class="accion">
    <div class="informativo">
      <div class="cataresult" >
      <asp:UpdatePanel ID="upresult" runat="server" ChildrenAsTriggers="true">
        <ContentTemplate>
       <asp:Repeater ID="tablePaginationDocumentos" runat="server">
            <HeaderTemplate>
            <table id="tablar"  cellspacing="1" cellpadding="1" class="table table-bordered invoice">
            <thead>
            <tr>
            <th>Documentos</th>
            <th>Escoja el archivo con formato indicado en formato PDF.</th>
            </tr>
            </thead> 
            <tbody>
            </HeaderTemplate>
            <ItemTemplate>
            <tr class="point">
            <td class="nover"><%#Eval("DESCRIPCION")%></td>
            <td class="nover"><asp:TextBox ID="txtidsolicitud" runat="server" Text='<%#Eval("IDTIPSOL")%>' Width="5px"/></td>
            <td class="nover"><asp:TextBox ID="txtiddocemp" runat="server" Text='<%#Eval("IDDOCEMP")%>' Width="5px"/></td>
            <td style=" font-size:inherit"><%#Eval("DOCUMENTO")%></td>
            <td>
                <%--<input extension='<%#Eval("EXTENSION")%>' class="uploader" id="fsupload" title="Escoja el archivo con formato indicado en la siguiente columna."
                       onchange="validaextension(this)" type="file"  runat="server" clientidmode="Static" />--%>
                <asp:FileUpload extension='<%#Eval("EXTENSION")%>' class="btn btn-outline-primary mr-4" id="fsupload" title="Escoja el archivo con formato indicado en la siguiente columna."
                       onchange="validaextension(this)" style=" font-size:small" runat="server"/>
                <%--<input class="uploader" id="File1" title="Escoja el archivo CSV (Excel separado por comas)" accept="csv" type="file"  runat="server" clientidmode="Static" />--%>
            </td>
            <td class="nover"><%#Eval("EXTENSION")%></td>
            </tr>
            </ItemTemplate>
            <FooterTemplate>
            </tbody>
            </table>
            </FooterTemplate>
         </asp:Repeater>
      </ContentTemplate>
                     <Triggers>
                         <asp:AsyncPostBackTrigger ControlID="ddlActividadOnlyControl" />
                     </Triggers>
        </asp:UpdatePanel>
        </div>
      </div>
    </div>

    <%--<div class="g-recaptcha" data-sitekey="6Le7xBgTAAAAAEvVoppLLsqPNgmr7gDEDhGpUuDp" data-sitekey="my_key"></div>--%>
    </div>
            
        <div class="row">
            <div class="col-md-12 d-flex justify-content-center">
                   <asp:Button ID="btsalvar" runat="server" Text="Solicitar Permiso de Acceso" OnClientClick="return prepareObject();" class="btn btn-primary" 
               onclick="btsalvar_Click" ToolTip="Confirma la información y crea la solicitud de Permiso de Acceso."/>
            </div>
       </div>
     
     <asp:HiddenField runat="server" ID="hftablecol" Value="0" />
     </div>
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
        function valdropdownlist(control, validador) {
            if (control.value != 0) {
                //control.style.cssText = "background-color:none;color:none;width:350px;"
                validador.innerHTML = '';
                document.getElementById('imgagrega').innerHTML = '<img alt="" src="../shared/imgs/loader.gif"/>';
                return;
            }
            //control.style.cssText = "background-color:White;color:Red;width:350px;";
            validador.innerHTML = '<span class="obligado">OBLIGATORIO!</span>';
            document.getElementById('imgagrega').innerHTML = '<img alt="" src="../shared/imgs/loader.gif"/>';
            return;
        }
        function getGifOculta(control) {
            document.getElementById(control).innerHTML = '<img alt="" src=""/>';
            return true;
        }
        function validaCabecera() {
        try {
            var vals = document.getElementById('<%=ddlActividadOnlyControl.ClientID %>').value;
            if (vals == 0) {
                alert('* Seleccione el Área Destino/Actividad *');
                document.getElementById('<%=ddlActividadOnlyControl.ClientID %>').focus();
                <%--document.getElementById('<%=ddlActividadOnlyControl.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:400px;";--%>
                document.getElementById("loader").className = 'nover';
                return false;
            }
            var vals = document.getElementById('<%=txtci.ClientID %>').value;
            if (vals == '' || vals == null || vals == undefined) {
                alert('* La Cédula del Colaborador no debe ser nulo  *');
                document.getElementById('<%=txtci.ClientID %>').focus();
                <%--document.getElementById('<%=txtci.ClientID %>').style.cssText = "background-color:Gray;width:200px;";--%>
                document.getElementById("loader").className = 'nover';
                return false;
            }

            var vals = document.getElementById('<%=txtNombres.ClientID %>').value;
            if (vals == '' || vals == null || vals == undefined) {
                alert('* Los Nombres del Colaborador no deben ser nulo *');
                document.getElementById('<%=txtNombres.ClientID %>').focus();
                <%--document.getElementById('<%=txtNombres.ClientID %>').style.cssText = "background-color:Gray;width:400px;";--%>
                document.getElementById("loader").className = 'nover';
                return false;
            }

            var vals = document.getElementById('<%=txtApellidos.ClientID %>').value;
            if (vals == '' || vals == null || vals == undefined) {
                alert('* Los Nombres del Colaborador no deben ser nulo  *');
                document.getElementById('<%=txtApellidos.ClientID %>').focus();
                <%--document.getElementById('<%=txtApellidos.ClientID %>').style.cssText = "background-color:Gray;width:400px;";--%>
                document.getElementById("loader").className = 'nover';
                return false;
            }
                <%--var vals = document.getElementById('<%=txtcargo.ClientID %>').value;
                if (vals == '' || vals == null || vals == undefined) {
                    alert('* Escriba el Cargo del Empleado *');
                    document.getElementById('<%=txtcargo.ClientID %>').focus();
                    document.getElementById('<%=txtcargo.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:200px;";
                    document.getElementById("loader").className = 'nover';
                    return false;
                }--%>
                var vals = document.getElementById('<%=txtfecing.ClientID %>').value;
                if (vals == '' || vals == null || vals == undefined) {
                    alert('* Escriba la Fecha de Ingreso *');
                    document.getElementById('<%=txtfecing.ClientID %>').focus();
                    <%--document.getElementById('<%=txtfecing.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:200px;";--%>
                    document.getElementById("loader").className = 'nover';
                    return false;
                }
                var vals = document.getElementById('<%=txtfecsal.ClientID %>').value;
                if (vals == '' || vals == null || vals == undefined) {
                    alert('* Escriba la Fecha de Salida *');
                    document.getElementById('<%=txtfecsal.ClientID %>').focus();
                    <%--document.getElementById('<%=txtfecsal.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:200px;";--%>
                    document.getElementById("loader").className = 'nover';
                    return false;
                }
                document.getElementById('<%=txtNombres.ClientID %>').disabled = false;
                document.getElementById('<%=txtApellidos.ClientID %>').disabled = false;
                document.getElementById('<%=txtci.ClientID %>').disabled = false;
                document.getElementById("loader").className = '';
                document.getElementById('imgagrega').innerHTML = '<img alt="" src="../shared/imgs/loader.gif"/>';
                return true;
            } catch (e) {
                alert('Lamentamos comunicarle que ha ocurrido el siguiente problema:\n' + e.Message);
            }
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
                if (confirm('¿Esta seguro de crear la solicitud de Permiso de Acceso?') == false) {
                    return false;
                }
                var vals = document.getElementById('<%=hftablecol.ClientID %>').value;
                if (vals == '0') {
                    alert('* Agregue al menos un Colaborador *');
                    document.getElementById("loader").className = 'nover';
                    return false;
                }
                //Valida Documentos
                lista = [];
                var tbl = document.getElementById('tablar');
                for (var f = 0; f < tbl.rows.length; f++) {
                    var celColect = tbl.rows[f].getElementsByTagName('td');
                    if (celColect != undefined && celColect != null && celColect.length > 0) {
                        var tdetalle = {
                            documento: celColect[4].getElementsByTagName('input')[0].value
                        };
                        this.lista.push(tdetalle);
                    }
                }
                var nomdoc = null;
                for (var n = 0; n < this.lista.length; n++) {
                    if (lista[n].documento == '' || lista[n].documento == null || lista[n].documento == undefined) {
                        alert('* Seleccione todos los documentos requeridos *');
                        document.getElementById("loader").className = 'nover';
                        return false;
                    }
                }
//                    if (nomdoc == lista[n].documento) {
//                        alert('* Existen archivos repetidos, revise por favor *');
//                        document.getElementById("loader").className = 'nover';
//                        return false;
//                    }
//                    nomdoc = lista[n].documento;
//                }
                document.getElementById("loader").className = '';
                document.getElementById('imgenvia').innerHTML = '<img alt="" src="../shared/imgs/loader.gif"/>';
                return true;
            } catch (e) {
                alert('Lamentamos comunicarle que ha ocurrido el siguiente problema:\n' + e.Message);
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
            window.open('../catalogo/calendarioConsolidacion.aspx?bk=' + bo, 'name', 'width=850,height=880')
        }
        function redirectsol(val, val2, val3) {
            var caja = val;
            var caja2 = val2;
            var caja3 = val3;
            window.open('../credenciales/revisasolicitudcolaboradordocumentos.aspx?numsolicitud=' + caja + '&idsolcol=' + caja2 + '&cedula=' + caja3)
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
            <%--document.getElementById('<%=txtNombres.ClientID %>').style.cssText = "background-color:White;width:400px;";
            document.getElementById('<%=txtApellidos.ClientID %>').style.cssText = "background-color:White;width:400px;";
            document.getElementById('<%=txtci.ClientID %>').style.cssText = "background-color:White;width:200px;";--%>
        }
        function fvalidaCriterios() {
<%--            var cedula = document.getElementById('<%=rbcedula.ClientID %>').checked;
            var nombres = document.getElementById('<%=rbnombres.ClientID %>').checked;
            var apellidos = document.getElementById('<%=rbapellidos.ClientID %>').checked;--%>
            var vals = document.getElementById('<%=txtcriterioconsulta.ClientID %>').value;
            if (vals == '' || vals == null || vals == undefined) {
                document.getElementById('<%=txtcriterioconsulta.ClientID %>').focus();
                <%--document.getElementById('<%=txtcriterioconsulta.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:200px;";--%>
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
