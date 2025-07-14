<%@ Page Title="Asignación de Grupos" Language="C#" MasterPageFile="~/site.Master"
    AutoEventWireup="true" CodeBehind="datosgruposoperadores.aspx.cs" Inherits="CSLSite.datosgruposoperadores" %>

<%@ MasterType VirtualPath="~/site.Master" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="placehead" runat="server">
    <link href="../shared/estilo/tables.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/catalogos.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/info.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/aisv-general.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/work.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        function BindFunctions() {

            $(document).ready(function () {
                //inicia los fecha-hora

                //colapsar y expandir
                $("div.colapser").toggle(function () { $(this).removeClass("colapsa").addClass("expande"); $(this).next().hide(); }
                                  , function () { $(this).removeClass("expande").addClass("colapsa"); $(this).next().show(); });
                //poner valor en campo

            });

            $(document).ready(function () {
                $("#tblCustomers [id*=chkHeader]").click(function () {
                    if ($(this).is(":checked")) {
                        $("#tblCustomers [id*=chkRow]").attr("checked", "checked");
                    } else {
                        $("#tblCustomers [id*=chkRow]").removeAttr("checked");
                    }
                });

                $("#tblCustomers [id*=chkRow]").click(function () {
                    if ($("#tblCustomers [id*=chkRow]").length == $("#tblCustomers [id*=chkRow]:checked").length) {
                        $("#tblCustomers [id*=chkHeader]").attr("checked", "checked");
                    } else {
                        $("#tblCustomers [id*=chkHeader]").removeAttr("checked");
                    }
                });
            });
        }

        
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="placebody" runat="server">
    <asp:ToolkitScriptManager ID="tkcal" EnableScriptGlobalization="true" runat="server">
    </asp:ToolkitScriptManager>
    <asp:UpdatePanel ID="udpRolesUsuario" runat="server">
        <ContentTemplate>
            <input id="zonaid" type="hidden" value="905" />
            <noscript>
                <meta http-equiv="refresh" content="0; url=sinsoporte.htm" />
            </noscript>
            <div>
                <i class="ico-titulo-1"></i>
                <h2>
                    Asignación de roles a usuarios
                </h2>
                <br />
                <i class="ico-titulo-2"></i>
                <h1>
                    Ingreso/Modificación de roles a usuarios
                </h1>
                <br />
            </div>
            
            <div class="seccion" id="PERSONAL">
                <div class="informativo">
                    <table>
                        <tr>
                            <td rowspan="2" class="inum">
                                <div class="number">
                                    1</div>
                            </td>
                            <td class="level1">
                                Asignación de roles
                            </td>
                        </tr>
                        <tr>
                            <td class="level2">
                                Asigna los usuarios que tendrán asociados el siguiente rol
                            </td>
                        </tr>
                    </table>
                </div>
                <div class="colapser colapsa">
                </div>
                <div class="accion">
                    <table class="controles" cellspacing="0" cellpadding="1">
                        <tr>
                            <th class="bt-bottom bt-right bt-top bt-left" colspan="4">
                                Datos de asignación.
                            </th>
                        </tr>
                        <tr>
                            <td class="bt-bottom  bt-right bt-left">
                                1. Rol a asignar:
                            </td>
                            <td class="bt-bottom">
                                <asp:DropDownList ID="ddlGrupo" runat="server" Width="205px" AppendDataBoundItems="true"
                                    onblur="opcionrequerida(this,0,'valddlGrupo');">
                                </asp:DropDownList>
                            </td>
                            <td class="bt-bottom bt-right validacion">
                                <span class="validacion" id="valddlGrupo">* obligatorio</span>
                            </td>
                        </tr>
                        <tr>
                            <th class="bt-bottom bt-right bt-top bt-left" colspan="4">
                                Criterios de Búsqueda
                            </th>
                        </tr>
                        <tr>
                            <td class="bt-bottom  bt-right bt-left">
                                1. Usuario:
                            </td>
                            <td class="bt-bottom bt-right" colspan="3">
                                <asp:TextBox ID="txtUsuario" runat="server"  onkeypress="return soloLetras(event,'abcdefghijklmnñopqrstuvwxyz1234567890 _',true)"
                                    MaxLength="50" onpaste="return false;" placeholder="Usuario" Width="200px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="bt-bottom  bt-right bt-left">
                                2. Nombre:
                            </td>
                            <td class="bt-bottom bt-right" colspan="3">
                                <asp:TextBox ID="txtNombreUsuario" runat="server"  onkeypress="return soloLetras(event,'abcdefghijklmnñopqrstuvwxyz1234567890áéíóú ',true)"
                                    MaxLength="50" onpaste="return false;" placeholder="Nombre" Width="200px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="bt-bottom  bt-right bt-left">
                                3. Identificación:
                            </td>
                            <td class="bt-bottom bt-right" colspan="3">
                                <asp:TextBox ID="txtIdentificacion" runat="server"  onkeypress="return soloLetras(event,'abcdefghijklmnñopqrstuvwxyz1234567890 ',true)"
                                    MaxLength="50" onpaste="return false;" placeholder="Identificacion" Width="200px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="bt-bottom  bt-right bt-left">
                                4. Empresa:
                            </td>
                            <td class="bt-bottom bt-right" colspan="3">
                                <asp:TextBox ID="txtEmpresa" runat="server" onkeypress="return soloLetras(event,'abcdefghijklmnñopqrstuvwxyz1234567890 ',true)"
                                    MaxLength="50" onpaste="return false;" placeholder="Empresa" Width="200px"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                    <div class="botonera">
                        <asp:UpdateProgress AssociatedUpdatePanelID="udpRolesUsuario" ID="updateProgress1"
                            runat="server">
                            <ProgressTemplate>
                                <div id="progressBackgroundFilter">
                                </div>
                                <div id="processMessage">
                                    Estamos procesando la tarea que solicitó, por favor espere...
                                    <img alt="Loading" src="../shared/imgs/loader.gif" style="margin: 0 auto;" />
                                </div>
                            </ProgressTemplate>
                        </asp:UpdateProgress>
                        <asp:Button ID="btbuscar" Text="Iniciar búsqueda" runat="server" OnClick="btbuscar_Click" />
                        &nbsp;
                    </div>
                    <div class="msg-alerta" id="alerta" runat="server">
                    </div>
                    <div id="error" runat="server" class="msg-critico" visible="false">
                    </div>
                    <div class="cataresult">
                        <asp:UpdatePanel ID="upresult" runat="server" ChildrenAsTriggers="true">
                            <ContentTemplate>
                                <script type="text/javascript">                                    Sys.Application.add_load(BindFunctions);  </script>
                                <input id="idlin" type="hidden" runat="server" clientidmode="Static" />
                                <input id="diponible" type="hidden" runat="server" clientidmode="Static" />
                                <div id="xfinder" runat="server" visible="false">
                                    <div class="findresult">
                                        <div class="booking">
                                            <div class="separator">
                                                Listado de Usuarios:</div>
                                            <div class="bokindetalle">
                                                <asp:Repeater ID="tablePagination" runat="server">
                                                    <HeaderTemplate>
                                                        <table id="tblCustomers" cellspacing="1" cellpadding="1" class="tabRepeat">
                                                            <thead>
                                                                <tr>
                                                                    <th style="display: none;">
                                                                        idUsuario
                                                                    </th>
                                                                    <th style="display: none;">
                                                                        idGrupo
                                                                    </th>
                                                                    <th>
                                                                        Usuario
                                                                    </th>
                                                                    <th>
                                                                        Nombres
                                                                    </th>
                                                                    <th>
                                                                        Identificación
                                                                    </th>
                                                                    <th>
                                                                        Empresa
                                                                    </th>
                                                                    <th>
                                                                        Roles
                                                                    </th>
                                                                    <th>
                                                                        <asp:CheckBox ID="chkHeader" runat="server" />
                                                                    </th>
                                                                </tr>
                                                            </thead>
                                                            <tbody>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <tr class="point" id="trUsuario">
                                                            <td style="display: none;">
                                                                <asp:Label ID="lblIdUsuario" runat="server" Text='<%#Eval("idUsuario")%>'></asp:Label>
                                                            </td>
                                                            <td style="display: none;">
                                                                <asp:Label ID="lblIdGrupo" runat="server" Text='<%#Eval("idGrupo")%>'></asp:Label>
                                                            </td>
                                                            <td>
                                                                <%#Eval("usuario")%>
                                                            </td>
                                                            <td>
                                                                <%#Eval("nombres")%>
                                                            </td>
                                                            <td>
                                                                <%#Eval("usuarioIdentificacion")%>
                                                            </td>
                                                            <td>
                                                                <%#Eval("nombreEmpresa")%>
                                                            </td>
                                                            <td>
                                                                <%#Eval("roles")%>
                                                            </td>
                                                            <td>
                                                                <div class="tcomand">
                                                                    <asp:CheckBox ID="chkRow" runat="server" Checked='<%#Convert.ToBoolean(Eval("rolGrupo"))%>' />
                                                                </div>
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
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btbuscar" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                    <div class="botonera" runat="server" id="divBotonGuardar">
                        <asp:UpdateProgress AssociatedUpdatePanelID="udpRolesUsuario" ID="updateProgress"
                            runat="server">
                            <ProgressTemplate>
                                <div id="progressBackgroundFilter">
                                </div>
                                <div id="processMessage">
                                    Estamos procesando la tarea que solicitó, por favor espere...
                                    <img alt="Loading" src="../shared/imgs/loader.gif" style="margin: 0 auto;" />
                                </div>
                            </ProgressTemplate>
                        </asp:UpdateProgress>
                        <asp:Button ID="btnGuardar" Text="Guardar" runat="server" OnClick="btnGuardar_Click" />
                        &nbsp;
                        <asp:Button ID="btnRegresar" Text="Regresar" runat="server" OnClick="btnRegresar_Click" />
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <script src="../Scripts/pages.js" type="text/javascript"></script>
    <script src="../Scripts/pages_mod.js" type="text/javascript"></script>
    <script type="text/javascript">
        var ced_count = 0;
        var jAisv = {};
        $(window).load(function () {
            //objeto a transportar.
            $(document).ready(function () {
                //inicia los fecha-hora

                //colapsar y expandir
                $("div.colapser").toggle(function () { $(this).removeClass("colapsa").addClass("expande"); $(this).next().hide(); }
                                  , function () { $(this).removeClass("expande").addClass("colapsa"); $(this).next().show(); });


            });
        });
        //Esta funcion va a validar que cuando presionen booking debe poner los 3 parametros
        function validateBook(objeto) {
            /*Recordar vaciar los span*/
            var expnum = document.getElementById('numexpo');
            var linebook = document.getElementById('linea');
            //nuevo > 2015-09-08
            if (objeto.bline != null) {
                if (objeto.bline.toLowerCase() === "msc") {
                    document.getElementById('msc_sello').innerHTML = '32. Sello de sticker [Obligatorio]: <a class="tooltip" ><span class="classic" >Sello adhesivo que fue otorgado por MSC.</span><img alt="" src="../shared/imgs/info.gif" class="datainfo"/></a>';
                }
                else {

                    document.getElementById('msc_sello').innerHTML = "32. Sello adicional 1: ";
                }
            }
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
                this.setRefer(objeto.refer);
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
                this.jAisv.shipid = objeto.shipid;
                this.jAisv.shipname = objeto.shipname;
                this.jAisv.hzkey = objeto.hzkey;
                this.jAisv.utemp = objeto.temp;
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

        //Imprimir.......................
        function imprimir() {

            //Si es contenedor validar cedula

        }

        //esta futura funcion va a preparar el objeto a transportar.
        function prepareObject() {






        }
        function popupCallback(data, control) {
            this.document.getElementById(control).value = data;
        }

    </script>
</asp:Content>
