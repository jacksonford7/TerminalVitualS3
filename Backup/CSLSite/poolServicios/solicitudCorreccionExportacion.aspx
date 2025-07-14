<%@ Page Title="" Language="C#" MasterPageFile="~/site.Master" AutoEventWireup="true"
    CodeBehind="solicitudCorreccionExportacion.aspx.cs" Inherits="CSLSite.solicitudCorreccionExportacion" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="placehead" runat="server">
    <link href="../shared/estilo/tables.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/aisv-general.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/work.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/catalogos.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/info.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/centroServicios.js" type="text/javascript"></script>
    <link href="../shared/estilo/centroSolicitud.css" rel="stylesheet" type="text/css" />
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
        .style1
        {
            height: 65px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="placebody" runat="server">
    <input id="zonaid" type="hidden" value="710" />
    <asp:ToolkitScriptManager ID="tkcal" EnableScriptGlobalization="true" runat="server"
        AsyncPostBackTimeout="1200">
    </asp:ToolkitScriptManager>
    <div>
        <i class="ico-titulo-1"></i>
        <h2>
            Ingreso de Solicitud (Corrección de Ingreso de Exportación)</h2>
        <br />
        <br />
        <i class="ico-titulo-2"></i>
        <h1>
            Solicitud</h1>
        <br />
    </div>
    <asp:UpdatePanel ID="updSolicitud" runat="server">
        <ContentTemplate>
            <div class="seccion">
                <div class="accion">
                    <table class="xcontroles" cellspacing="0" cellpadding="1">
                        <tr>
                            <th class="bt-bottom bt-right bt-left bt-top" colspan="4">
                                Criterios de consulta:
                            </th>
                        </tr>
                        <tr>
                            <td class="bt-bottom bt-right bt-left">
                                Servicios:
                            </td>
                            <td class="bt-bottom bt-right" colspan="4">
                                <asp:DropDownList ID="ddlTipoServicios" runat="server" Width="300px">
                                    <asp:ListItem Value="0">* Seleccione servicios *</asp:ListItem>
                                </asp:DropDownList>
                                <asp:HiddenField ID="hdfUsuario" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="bt-bottom bt-right bt-left">
                                Tráfico:
                            </td>
                            <td class="bt-bottom bt-right" colspan="4">
                                <asp:DropDownList ID="ddlTipoTrafico" runat="server" Width="300px">
                                    <asp:ListItem Value="0">* Seleccione tráfico *</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td class="bt-bottom bt-right bt-left">
                                Tipo de Corrección:
                            </td>
                            <td class="bt-bottom bt-right" colspan="2">
                                <asp:DropDownList ID="ddlTipoCorreccion" runat="server" Width="300px" OnSelectedIndexChanged="ddlTipoCorreccion_SelectedIndexChanged"
                                    AutoPostBack="true" onblur="opcionrequerida(this,0,'valTipoCorreccion');">
                                    <asp:ListItem Value="0">* Seleccione tipo de corrección *</asp:ListItem>
                                </asp:DropDownList>
                              
                            </td>
                            <td class="bt-bottom bt-right validacion">
                                <span class="validacion" id="valTipoCorreccion">* obligatorio</span>
                            </td>
                        </tr>
                        <tr class='nover'>
                            <td class="bt-bottom bt-right bt-left">
                                Teléfono:
                            </td>
                            <td class="bt-bottom bt-right" colspan="2">
                                <asp:TextBox ID="txtTelefono" runat="server" Width="290px" onkeypress="return soloLetras(event,'abcdefghijklmnñopqrstuvwxyz1234567890 .',true)"
                                    MaxLength="20" onpaste="return false;" onblur="cadenareqerida(this,1,20,'valtelefono');" />
                            </td>
                            <td class="bt-bottom bt-right validacion">
                                <span class="validacion" id="valtelefono">* obligatorio</span>
                            </td>
                        </tr>
                        <tr>
                            <td class="bt-bottom bt-right bt-left">
                                Mail:
                            </td>
                            <td class="bt-bottom bt-right" colspan="2">
                                <script language='javascript' type="text/javascript">

                                    var g_correoUsuarioID = '<%=hdfCorreoUsuario.ClientID%>'

                                </script>
                                <asp:HiddenField ID="hdfCorreoUsuario" runat="server" />
                                <asp:TextBox ID="txtMail" runat="server" Width="290px" onkeypress="return soloLetras(event,'abcdefghijklmnñopqrstuvwxyz1234567890_$@.',true)"
                                    MaxLength="150" onpaste="return false;" onblur="validarEmail(this,'valemailusu', document.getElementById(g_correoUsuarioID));"
                                    placeholder="MAIL@MAIL.COM" />
                            </td>
                            <td class="bt-bottom bt-right validacion">
                                <span class="validacion" id="valemailusu">* obligatorio</span>
                            </td>
                        </tr>
                        <tr>
                            <th class="bt-bottom bt-right bt-left bt-top" colspan="4">
                                Datos del Contenedor:
                            </th>
                        </tr>
                        <tr>
                            <td class="bt-bottom bt-right bt-left">
                                Contenedor:
                            </td>
                            <td class="bt-bottom">
                                <asp:TextBox ID="contenedor1" runat="server" Style="text-align: center;" Text="..."
                                    Width="90%" Enabled="false"></asp:TextBox>
                                <input id="contenedor1HF" runat="server" type="hidden" />
                            </td>
                            <td class="bt-bottom finder">
                                <a class="topopup" target="popup" onclick="linkcontenedorCorrecionDAEExportacion('<%=ddlTipoTrafico.ClientID %>','FCL', '<%=ddlTipoCorreccion.ClientID %>' ,'<%=txtAISV.ClientID %>','<%=hdfUsuario.ClientID %>');">
                                    <i class="ico-find"></i>Buscar </a>
                            </td>
                            <td class="bt-bottom bt-right validacion ">
                                <span id="valcont2" class="validacion">* obligatorio</span>
                            </td>
                        </tr>

                        <tr id="trAisv" runat="server">
                            <td class="bt-bottom bt-right bt-left">
                                AISV:
                            </td>
                            <td class="bt-bottom bt-right" colspan="2">
                                <asp:TextBox ID="txtAISV" runat="server" Style="text-align: center;" Width="90%"
                                     MaxLength="30"></asp:TextBox>
                            </td>
                            <td class="bt-bottom bt-right validacion">
                                <asp:Panel ID="pnlAisv" runat="server">
                                    <span class="validacion" id="valAisv"></span></asp:Panel>
                            </td>
                        </tr>

                        <tr>
                            <td class="bt-bottom bt-right bt-left">
                                Booking Contenedor:
                            </td>
                            <td class="bt-bottom">
                                <asp:TextBox ID="bookingContenedor" runat="server" Style="text-align: center;" Text="..."
                                    Width="90%" Enabled="false"></asp:TextBox>
                                <input id="bookingContenedorHF" runat="server" type="hidden" />
                            </td>
                            <td class="bt-bottom bt-right" colspan="2">
                            </td>
                        </tr>
                        <tr>
                            <td class="bt-bottom bt-right bt-left">
                                Código de Carga:
                            </td>
                            <td class="bt-bottom">
                                <asp:TextBox ID="txtNumeroCarga" runat="server" Style="text-align: center;" Text="..."
                                    Width="90%" Enabled="false"></asp:TextBox>
                                <asp:HiddenField ID="hdfIso" runat="server" />
                            </td>
                            <td class="bt-bottom bt-right" colspan="2">
                            </td>
                        </tr>
                        <tr>
                            <td class="bt-bottom bt-right bt-left">
                                Peso:
                            </td>
                            <td class="bt-bottom">
                                <asp:TextBox ID="txtPeso" runat="server" Style="text-align: center;" Text="..." Width="90%"
                                    Enabled="false"></asp:TextBox>
                            </td>
                            <td class="bt-bottom bt-right" colspan="2">
                            </td>
                        </tr>
                        <tr>
                            <td class="bt-bottom bt-right bt-left">
                                Sello de agencia:
                            </td>
                            <td class="bt-bottom">
                                <asp:TextBox ID="txtSello1" runat="server" Style="text-align: center;" Text="..."
                                    Width="90%" Enabled="false"></asp:TextBox>
                            </td>
                            <td class="bt-bottom bt-right" colspan="2">
                            </td>
                        </tr>
                        <tr>
                            <td class="bt-bottom bt-right bt-left">
                                Sello de ventilación [Reefer]:
                            </td>
                            <td class="bt-bottom">
                                <asp:TextBox ID="txtSello2" runat="server" Style="text-align: center;" Text="..."
                                    Width="90%" Enabled="false"></asp:TextBox>
                            </td>
                            <td class="bt-bottom bt-right" colspan="2">
                            </td>
                        </tr>
                        <tr>
                            <td class="bt-bottom bt-right bt-left">
                                Sello adicional 1:
                            </td>
                            <td class="bt-bottom">
                                <asp:TextBox ID="txtSello3" runat="server" Style="text-align: center;" Text="..."
                                    Width="90%" Enabled="false"></asp:TextBox>
                            </td>
                            <td class="bt-bottom bt-right" colspan="2">
                            </td>
                        </tr>
                        <tr>
                            <td class="bt-bottom bt-right bt-left">
                                Sello adicional 2:
                            </td>
                            <td class="bt-bottom">
                                <asp:TextBox ID="txtSello4" runat="server" Style="text-align: center;" Text="..."
                                    Width="90%" Enabled="false"></asp:TextBox>
                            </td>
                            <td class="bt-bottom bt-right" colspan="2">
                            </td>
                        </tr>
                        <tr>
                            <th class="bt-bottom bt-right bt-left bt-top" colspan="4">
                                DATOS DE DAE :
                            </th>
                        </tr>
                        <tr>
                            <td class="bt-bottom bt-right bt-left">
                                DAE errada :
                            </td>
                            <td class="bt-bottom bt-right" colspan="2">
                                <asp:TextBox ID="txtActualDae" runat="server" Style="text-align: center;" Width="90%"
                                    Enabled="false" onblur="cadenareqerida(this,1,200,'valDaeActual');"></asp:TextBox>
                                <%--<span id="bookingContenedor" class="caja">...</span>--%>
                                <input id="hdfDaeActual" runat="server" type="hidden" />
                            </td>
                            <td class="bt-bottom bt-right validacion">
                                <asp:Panel ID="pnlDaeActual" runat="server">
                                    <span class="validacion" id="valDaeActual">* obligatorio</span>
                                </asp:Panel>
                            </td>
                        </tr>
                        <tr>
                            <td class="bt-bottom bt-right bt-left">
                                DAE correcta :
                            </td>
                            <td class="bt-bottom bt-right" colspan="2">
                                <asp:TextBox ID="txtNuevoDae" runat="server" Style="text-align: center;" Width="90%"
                                    onblur="cadenareqerida(this,1,17,'valDaeNueva');" MaxLength="17" onkeypress="return soloLetras(event,'abcdefghijklmnñopqrstuvwxyz1234567890',true)"></asp:TextBox>
                                <%--<span id="bookingContenedor" class="caja">...</span>--%>
                                <input id="hdfDaeNueva" runat="server" type="hidden" />
                            </td>
                            <td class="bt-bottom bt-right validacion">
                                <asp:Panel ID="pnlDaeNueva" runat="server">
                                    <span class="validacion" id="valDaeNueva">* obligatorio</span>
                                </asp:Panel>
                            </td>
                        </tr>
                    </table>
                    <div class="botonera">
                        <span id="imagen" runat="server"></span>
                        <input type="button" id="btgenerar" value="Generar solicitud" onclick="confirmSolicitud()"
                             />
                        <asp:Button ID="btgenerarServer" runat="server" Text="Generar solicitud" OnClientClick="showGif('placebody_imagen')"
                            OnClick="btgenerar_Click"  style="display:none;" />
                    </div>
                    <div class="cataresult">
                        <asp:UpdatePanel ID="upresult" runat="server" ChildrenAsTriggers="true">
                            <ContentTemplate>
                                <div class="msg-alerta" id="alerta" runat="server">
                                </div>
                                <input id="idlin" type="hidden" runat="server" clientidmode="Static" />
                                <input id="diponible" type="hidden" runat="server" clientidmode="Static" />
                                <div id="sinresultado" runat="server" class="msg-info">
                                </div>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btgenerarServer" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <script src="../Scripts/pages.js" type="text/javascript"></script>
    <script src="../Scripts/pages_mod.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.tablesorter.min.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.tablesorter.pager.js" type="text/javascript"></script>
    <script type="text/javascript">
        function showGif(ctrl) {
            if (ctrl != null && ctrl != undefined && ctrl.trim().length > 0) {
                document.getElementById(ctrl).innerHTML = '<img alt="" src="../shared/imgs/loader.gif">'
            }
        }

        $(document).ready(function () {
            $('.datetimepicker').datetimepicker().datetimepicker({ lang: 'es', timepicker: false, closeOnDateSelect: true, format: 'd/m/Y' });
        });

        function popupCallback(objeto) {
            if (objeto == null || objeto == undefined) {
                alert('Hubo un problema al setaar un objeto de catalogo');
                return;
            }
            //si contenedor está lleno (FCL) o vacio (MTY)

            document.getElementById('<%=contenedor1.ClientID %>').value = objeto.codigo;
            document.getElementById('<%=contenedor1HF.ClientID %>').value = objeto.item;
            document.getElementById('<%=bookingContenedor.ClientID %>').value = objeto.booking;
            document.getElementById('<%=bookingContenedorHF.ClientID %>').value = objeto.booking;
            document.getElementById('<%=txtActualDae.ClientID %>').value = objeto.dae;
            document.getElementById('<%=hdfDaeActual.ClientID %>').value = objeto.dae;
            document.getElementById('<%=txtSello1.ClientID %>').value = objeto.sello1;
            document.getElementById('<%=txtSello2.ClientID %>').value = objeto.sello2;
            document.getElementById('<%=txtSello3.ClientID %>').value = objeto.sello3;
            document.getElementById('<%=txtSello4.ClientID %>').value = objeto.sello4;
            document.getElementById('<%=txtPeso.ClientID %>').value = objeto.peso;
            document.getElementById('<%=txtNumeroCarga.ClientID %>').value = objeto.codigoCarga;
            document.getElementById('<%=hdfIso.ClientID %>').value = objeto.iso;

            if(objeto.aisv !='')
            document.getElementById('<%=txtAISV.ClientID %>').value = objeto.aisv;
            document.getElementById('<%=txtAISV.ClientID %>').setAttribute("disabled", "disabled");
            return;

        }

        function confirmSolicitud() {

         var trafico = document.getElementById('<%=ddlTipoCorreccion.ClientID %>').value;
         if (trafico == 'DA') {
             var r = confirm("Se generará una eliminación de ingreso de exportación y un nuevo ingreso de exportación ¿Está seguro de realizar la solicitud?");
             if (r == true) {
                 $("#<%=btgenerarServer.ClientID%>").click();
             }
             else {
                 return;
             }
         }
         else {
             if (trafico == 'DE') {
                 var r = confirm("Se generará un nuevo ingreso de exportación ¿Está seguro de realizar la solicitud?");
                 if (r == true) {
                     $("#<%=btgenerarServer.ClientID%>").click();
                 }
                 else {
                     return;
                 }
             }
             else if (trafico == 'DI') {
                 var r = confirm("Se generará una eliminación de ingreso de exportación y un nuevo ingreso de exportación ¿Está seguro de realizar la solicitud?");
                 if (r == true) {
                     $("#<%=btgenerarServer.ClientID%>").click();
                 }
                 else {
                     return;
                 }
             }
             else {
                 var r = confirm("¿Está seguro de realizar la solicitud?");
                 if (r == true) {
                     $("#<%=btgenerarServer.ClientID%>").click();
                 }
                 else {
                     return;
                 }
             }
         }

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
                document.getElementById("loader").className = '';
                lista = [];
                //validaciones básicas
                var vals = document.getElementById('nbrboo');
                if (vals == null || vals == undefined || vals.value.trim().length <= 2) {
                    alert('* Datos de programación *\n *Escriba el numero de Booking*');
                    document.getElementById("loader").className = 'nover';
                    return;
                }
                vals = document.getElementById('xfecha');
                if (vals == null || vals == undefined || vals.value.trim().length <= 5) {
                    alert('* Datos de programación *\n Escriba la fecha de programación');
                    document.getElementById("loader").className = 'nover';
                    return;
                }
                vals = document.getElementById('tmail');
                if (vals == null || vals == undefined || vals.value.trim().length <= 5) {
                    alert('* Datos de programación *\n *Escriba el correo electrónico para la notificación');
                    document.getElementById("loader").className = 'nover';
                    return;
                }
                this.programacion.booking = document.getElementById('nbrboo').value;
                this.programacion.fecha_pro = document.getElementById('xfecha').value;
                this.programacion.mail = document.getElementById('tmail').value;
                this.programacion.idlinea = document.getElementById('idlin').value;
                this.programacion.linea = document.getElementById('agencia').value;
                this.programacion.total = document.getElementById('diponible').value;

                //recorrer tabla->
                var tbl = document.getElementById('tabla');
                for (var f = 0; f < tbl.rows.length; f++) {
                    var celColect = tbl.rows[f].getElementsByTagName('td');
                    if (celColect != undefined && celColect != null && celColect.length > 0) {

                        var tdetalle = {
                            num: celColect[0].textContent,
                            desde: celColect[1].textContent,
                            hasta: celColect[2].textContent,
                            dispone: celColect[4].textContent,
                            idh: celColect[5].textContent,
                            idd: celColect[6].textContent,
                            total: celColect[7].textContent
                        };
                        tdetalle.reserva = celColect[8].getElementsByTagName('input')[0].value;
                        this.lista.push(tdetalle);
                    }
                }
                this.programacion.detalles = this.lista;
                var qtlimite = parseInt(document.getElementById('diponible').value);
                var total = 0;
                for (var n = 0; n < this.lista.length; n++) {
                    if (lista[n].reserva != '') {
                        if (parseInt(lista[n].dispone) < parseInt(lista[n].reserva)) {
                            alert('El Horario ' + lista[n].desde + '-' + lista[n].hasta + ' excede su disponibilidad, favor verifique');
                            return;
                        }
                        total += parseInt(lista[n].reserva);
                    }
                }
                if (total > qtlimite) {
                    alert('* Reserva *\n La cantidad de reserva excede el cupo disponible \n Cupo: ' + qtlimite + '\n Reserva: ' + total);
                    return;
                }
                if (total <= 0) {
                    alert('* Reserva *\n La cantidad de reservas debe ser mayor que 0');
                    return;
                }
                tansporteServer(this.programacion, 'turnos.aspx/ValidateJSON');

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
            window.open('../catalogo/calendarioConsolidacion.aspx?bk=' + bo, 'name', 'width=850,height=480')
        }

    </script>
    <%--<asp:updateprogress  id="updateProgress" runat="server">
    <progresstemplate>
        <div id="progressBackgroundFilter"></div>
        <div id="processMessage"> Estamos procesando la tarea que solicitó, por favor  espere...<br />
         <img alt="Loading" src="../shared/imgs/loader.gif" style="margin:0 auto;" />
        </div>
    </progresstemplate>
</asp:updateprogress>--%>
</asp:Content>
