<%@ Page Title="Servicios" Language="C#" MasterPageFile="~/site.Master" AutoEventWireup="true"
    CodeBehind="datosservicio.aspx.cs" Inherits="CSLSite.datosservicio" %>

<%@ MasterType VirtualPath="~/site.Master" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="placehead" runat="server">
    <link href="../shared/estilo/tables.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/aisv-general.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/work.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/catalogos.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/info.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        function BindFunctions() {
            $(document).ready(function () {
                $('#tablasort').tablesorter({ cancelSelection: true, cssAsc: "ascendente", cssDesc: "descendente", headers: { 5: { sorter: false }, 6: { sorter: false}} }).tablesorterPager({ container: $('#pager'), positionFixed: false, pagesize: 10 });
            });


            $(document).ready(function () {
                //inicia los fecha-hora

                //colapsar y expandir
                $("div.colapser").toggle(function () { $(this).removeClass("colapsa").addClass("expande"); $(this).next().hide(); }
                                  , function () { $(this).removeClass("expande").addClass("colapsa"); $(this).next().show(); });
                //poner valor en campo

            });
        }

        
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="placebody" runat="server">
    <asp:ToolkitScriptManager ID="tkcal" EnableScriptGlobalization="true" runat="server">
    </asp:ToolkitScriptManager>
    <input id="zonaid" type="hidden" value="909" />
    <noscript>
        <meta http-equiv="refresh" content="0; url=sinsoporte.htm" />
    </noscript>
    <asp:UpdatePanel ID="updServicio" runat="server">
        <ContentTemplate>
            <script type="text/javascript">
                $(window).load(function () {
                    //objeto a transportar.
                    $(document).ready(function () {
                        //inicia los fecha-hora

                        //colapsar y expandir
                        $("div.colapser").toggle(function () { $(this).removeClass("colapsa").addClass("expande"); $(this).next().hide(); }
                                  , function () { $(this).removeClass("expande").addClass("colapsa"); $(this).next().show(); });
                        //poner valor en campo

                    });
                });
            </script>
            <div>
                <i class="ico-titulo-1"></i>
                <h2>
                    Servicios del sistema
                </h2>
                <br />
                <i class="ico-titulo-2"></i>
                <h1>
                    Ingreso/Modificación de Servicios
                </h1>
                <br />
            </div>
            <%--<div class=" msg-alerta">
                <span id="dtlo" runat="server">Estimado usuario:</span>
                <br />
                ???? <strong>??? </strong>???
            </div>--%>
            <div class="seccion" id="SESION">
                <div class="informativo">
                    <table>
                        <tr>
                            <td rowspan="2" class="inum">
                                <div class="number">
                                    1</div>
                            </td>
                            <td class="level1">
                                Ingreso/Modificación de Datos del Servicio
                            </td>
                        </tr>
                        <tr>
                            <td class="level2">
                                Ingrese o modifique los datos del servicio.
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
                                Datos del Servicio
                            </th>
                        </tr>
                        <tr>
                            <td class="bt-bottom  bt-right bt-left">
                                1. Código del servicio:
                            </td>
                            <td class="bt-bottom bt-right" colspan="3">
                                <asp:TextBox ID="txtIdServicio" runat="server"  Enabled="false"
                                    onkeypress="return soloLetras(event,'1234567890',true)" onpaste="return false;"
                                    placeholder="CODIGO" Width="200px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="bt-bottom  bt-right bt-left">
                                2. Descripción:
                            </td>
                            <td class="bt-bottom">
                                <asp:TextBox ID="txtDescripcion" runat="server"  onkeypress="return soloLetras(event,'abcdefghijklmnñopqrstuvwxyz1234567890,;áéíóú:. -/()',true)"
                                    MaxLength="100" onpaste="return false;" onblur="cadenareqerida(this,1,100,'valsel1');"
                                    placeholder="DESCRIPCION" Width="200px"></asp:TextBox>
                            </td>
                            <td class="bt-bottom bt-right validacion">
                                <span class="validacion" id="valsel1">* obligatorio</span>
                            </td>
                        </tr>
                        <tr>
                            <td class="bt-bottom  bt-right bt-left">
                                3. Estado:
                            </td>
                            <td class="bt-bottom bt-right" colspan="3">
                                <asp:DropDownList ID="ddlEstado" runat="server" Width="205px">
                                    <asp:ListItem Text="ACTIVO" Value="A"></asp:ListItem>
                                    <asp:ListItem Text="INACTIVO" Value="I"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                    <div class="botonera">
                        <asp:UpdateProgress AssociatedUpdatePanelID="updServicio" ID="updateProgress1"
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
                        <asp:Button ID="btnLimpiar" Text="Limpiar" runat="server" OnClick="btnLimpiar_Click" />
                        &nbsp;
                    </div>
                    <div class="msg-alerta" id="alerta" runat="server">
                    </div>
                    <div id="error" runat="server" class="msg-critico" visible="false">
                    </div>
                </div>
            </div>
            <div class="seccion" id="PERSONAL">
                <div class="informativo">
                    <table>
                        <tr>
                            <td rowspan="2" class="inum">
                                <div class="number">
                                    2</div>
                            </td>
                            <td class="level1">
                                Consulta de servicios
                            </td>
                        </tr>
                        <tr>
                            <td class="level2">
                                Consulta de servicios
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
                                Criterios de Búsqueda
                            </th>
                        </tr>
                        <tr>
                            <td class="bt-bottom  bt-right bt-left">
                                1. Descripción:
                            </td>
                            <td class="bt-bottom bt-right" colspan="3">
                                <asp:TextBox ID="txtDescripcionBuscar" runat="server"  onkeypress="return soloLetras(event,'abcdefghijklmnñopqrstuvwxyz1234567890,;áéíóú:. -/()',true)"
                                    MaxLength="50" onpaste="return false;" placeholder="DESCRIPCION" Width="200px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="bt-bottom  bt-right bt-left">
                                2. Estado:
                            </td>
                            <td class="bt-bottom bt-right" colspan="3">
                                <asp:DropDownList ID="ddlEstadoBuscar" runat="server" Width="205px">
                                    <asp:ListItem Text="TODOS" Value="0"></asp:ListItem>
                                    <asp:ListItem Text="ACTIVO" Value="A"></asp:ListItem>
                                    <asp:ListItem Text="INACTIVO" Value="I"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                    <div class="botonera">
                        <asp:UpdateProgress AssociatedUpdatePanelID="updServicio" ID="updateProgress"
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
                    <div class="cataresult">
                        <asp:UpdatePanel ID="upresult" runat="server">
                            <ContentTemplate>
                                <script type="text/javascript">                                    Sys.Application.add_load(BindFunctions);  </script>
                                <input id="idlin" type="hidden" runat="server" clientidmode="Static" />
                                <input id="diponible" type="hidden" runat="server" clientidmode="Static" />
                                <div id="xfinder" runat="server" visible="false">
                                    <div class="findresult">
                                        <div class="booking">
                                            <div class="separator">
                                                Listado de Servicios:</div>
                                            <div class="bokindetalle">
                                                <asp:Repeater ID="tablePagination" runat="server">
                                                    <HeaderTemplate>
                                                        <table id="tablasort" cellspacing="1" cellpadding="1" class="tabRepeat">
                                                            <thead>
                                                                <tr>
                                                                    <th>
                                                                        Código de Servicio.
                                                                    </th>
                                                                    <th>
                                                                        Descripción
                                                                    </th>
                                                                    <th>
                                                                        Estado
                                                                    </th>
                                                                    <th>
                                                                        Acciones
                                                                    </th>
                                                                </tr>
                                                            </thead>
                                                            <tbody>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <tr class="point" onclick="setService(this);">
                                                            <td>
                                                                <%#Eval("codigo")%>
                                                            </td>
                                                            <td>
                                                                <%#Eval("descripcion")%>
                                                            </td>
                                                            <td>
                                                                <%#Eval("estado")%>
                                                            </td>
                                                            <td>
                                                                <asp:Button ID="btnModificar" runat="server" Text="Modificar" CssClass="Anular" ToolTip="Carga la información del servicio para modificarla" />
                                                            </td>
                                                        </tr>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        </tbody> </table>
                                                    </FooterTemplate>
                                                </asp:Repeater>
                                            </div>
                                        </div>
                                    </div>
                                    <div id="pager">
                                        Registros por página
                                        <select class="pagesize">
                                            <option selected="selected" value="10">10</option>
                                            <option value="20">20</option>
                                        </select>
                                        <img alt="" src="../shared/imgs/first.gif" class="first" />
                                        <img alt="" src="../shared/imgs/prev.gif" class="prev" />
                                        <input type="text" class="pagedisplay" size="5px" />
                                        <img alt="" src="../shared/imgs/next.gif" class="next" />
                                        <img alt="" src="../shared/imgs/last.gif" class="last" />
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
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <script src="../Scripts/pages.js" type="text/javascript"></script>
    <script src="../Scripts/jquery-1.7.1.min.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.tablesorter.min.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.tablesorter.pager.js" type="text/javascript"></script>
    <script type="text/javascript">
        var ced_count = 0;
        var jAisv = {};



        //Esta funcion cargara los datos del servicio seleccionado para modificar
        function setService(row) {
            var celColect = row.getElementsByTagName('td');
            var servicio = {
                codigo: celColect[0].textContent.trim(),
                descripcion: celColect[1].textContent.trim(),
                estado: celColect[2].textContent.trim() == 'ACTIVO' ? 'A' : 'I'
            };

            var estado = celColect[2].textContent.trim();
            $("#<%=txtIdServicio.ClientID%>").val(servicio.codigo); //set value
            $("#<%=txtDescripcion.ClientID%>").val(servicio.descripcion); //set value
            var ddl = document.getElementById('<%=ddlEstado.ClientID%>');
            var opts = ddl.options.length;
            for (var i = 0; i < opts; i++) {
                if (ddl.options[i].value == servicio.estado) {
                    ddl.options[i].selected = true;
                    break;
                }
            }
        }



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
