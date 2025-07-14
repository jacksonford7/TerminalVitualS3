
<%@ Page  Title="Consola de Solicitudes"  Language="C#" MasterPageFile="~/site.Master" AutoEventWireup="true" CodeBehind="consultaestadosolicitud.aspx.cs" Inherits="CSLSite.cliente.consultaestadosolicitud" %>
<%@ MasterType VirtualPath="~/site.Master" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="placehead" runat="server">

    <link href="../img/favicon2.png" rel="icon"/>
    <link href="../img/icono.png" rel="apple-touch-icon"/>
         <!-- Bootstrap core CSS -->
    <link href="../css/bootstrap.min.css" rel="stylesheet"/>
    <link href="../css/dashboard.css" rel="stylesheet"/>
    <link href="../css/icons.css" rel="stylesheet"/>
    <link href="../css/style.css" rel="stylesheet"/>
    <link href="https://fonts.googleapis.com/css2?family=Roboto:wght@300;400;500;700&display=swap" rel="stylesheet"/>

    <link href="../shared/estilo/jquery.datetimepicker.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/jquery.datetimepicker.js" type="text/javascript"></script>

     <!--mensajes-->
    <link href="https://cdn.jsdelivr.net/npm/alertifyjs@1.11.0/build/css/alertify.min.css" rel="stylesheet"/>
    <script type="text/javascript" src="https://cdn.jsdelivr.net/npm/alertifyjs@1.11.0/build/alertify.min.js"></script>
    <script src="../lib/jquery/jquery.min.js" type="text/javascript"></script>
    <!--external css-->
    <link href="../lib/font-awesome/css/font-awesome.css" rel="stylesheet" />
    <link rel="stylesheet" type="text/css" href="../lib/gritter/css/jquery.gritter.css" />





<%--<style type="text/css">
        .warning { background-color:Yellow;  color:Red;}
        .panel-reveal-modal-bg { background: #000; background: rgba(0,0,0,.8);cursor:progress;	}
    </style>

     <style type="text/css">
        body2
        {
            font-family: Arial;
            font-size: 10pt;
        }
        .modal
        {
            position: fixed;
            z-index: 999;
            height: 100%;
            width: 100%;
            top: 0;
            background-color: Black;
            filter: alpha(opacity=60);
            opacity: 0.6;
            -moz-opacity: 0.8;
        }

        .modalBackground
        {
            background-color: Black;
            filter: alpha(opacity=60);
            opacity: 0.6;
        }
        .modalPopup
        {
            background-color: #FFFFFF;
            width: 500px;
            border: 3px solid #FF3720;
            padding: 0;
        }
        .modalPopup .header
        {
            background-color: #2FBDF1;
            height: 30px;
            color: White;
            line-height: 30px;
            text-align: center;
            font-weight: bold;
        }
        .modalPopup .body
        {
            min-height: 50px;
            line-height: 30px;
            text-align: center;
            font-weight: bold;
            margin-bottom: 5px;
        }
    </style>--%>
 </asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="placebody" runat="server">

  <noscript><meta http-equiv="refresh" content="0; url=sinsoporte.htm" /> </noscript>

   <input id="zonaid" type="hidden" value="7" />
    <asp:ToolkitScriptManager ID="tkscata" runat="server"></asp:ToolkitScriptManager>
    <asp:HiddenField ID="manualHide" runat="server" />
     <div class="mt-4">         
        <nav class="mt-4" aria-label="breadcrumb">
          <ol class="breadcrumb">
            <li class="breadcrumb-item" id="opcion_principal" runat="server"><a href="#">MCA - Módulo de Control de Acceso</a></li>
            <li class="breadcrumb-item active" aria-current="page" id="sub_opcion" runat="server">Consola de Solicitudes</li>
          </ol>
        </nav>
      </div>

        <div class="dashboard-container p-4" id="cuerpo" runat="server">
            <div class="form-title">
                Criterios de consulta:
            </div>
            <div class="form-row">
                <div class="form-group col-md-12" style=" display:none">
                    <label for="inputPassword4"> </label>
                        <label class="checkbox-container">
                        <asp:CheckBox Text="Asociación de Transportistas" ID="chkAsoTrans" Checked="false"  runat="server" />
                            <span class="checkmark"></span>
                        </label>
                </div>
                <div class="form-group col-md-6">
                        <label for="inputAddress">Número de Solicitud:<span style="color: #FF0000; font-weight: bold;"> *</span></label>
                                <asp:TextBox ID="txtsolicitud" runat="server" class="form-control" MaxLength="11"
                            style="text-align: center" onblur="cajaControl(this);"
                            onkeypress="return soloLetras(event,'01234567890',true)"></asp:TextBox>
             
                </div>
                <div class="form-group col-md-6"> </div>
                <div class="form-group col-md-6" style=" display:none">
                    <label for="inputAddress">Tipo de Solicitud:<span style="color: #FF0000; font-weight: bold;"> </span></label>
                            <asp:DropDownList ID="dptiposolicitud" runat="server" class="form-control" >
                            <asp:ListItem Value="0">* Seleccione origen *</asp:ListItem>
                    </asp:DropDownList>
             
            </div>
                <div class="form-group col-md-6" style=" display:none"> </div>
                <div class="form-group col-md-6" style=" display:none">
                    <label for="inputAddress">Tipo de Usuario:<span style="color: #FF0000; font-weight: bold;"> </span></label>
                    <asp:DropDownList ID="dptipousuario" runat="server" class="form-control" >
                        <asp:ListItem Value="0">* Seleccione origen *</asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="form-group col-md-6" style=" display:none"> </div>
                <div class="form-group col-md-6">
                    <label for="inputAddress">Generados desde / hasta:<span style="color: #FF0000; font-weight: bold;"> </span></label>
                    <div class="d-flex">
                    <asp:TextBox ID="tfechaini" runat="server" MaxLength="10" CssClass="datetimepicker form-control"
                    onkeypress="return soloLetras(event,'01234567890/')" style="text-align: center" 
                        onblur="valDate(this,true,valdate);" ClientIDMode="Static"></asp:TextBox> 
                        &nbsp;&nbsp;
                        <asp:TextBox ID="tfechafin" runat="server" ClientIDMode="Static"  MaxLength="15" CssClass="datetimepicker form-control"
                    onkeypress="return soloLetras(event,'01234567890/')"  style="text-align: center" 
                    onblur="valDate(this,true,valdate);"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group col-md-6"> </div>
                <div class="form-group col-md-6">
                    <label for="inputAddress">Estado:<span style="color: #FF0000; font-weight: bold;"> </span></label>
                            <asp:DropDownList ID="dpestados" runat="server" class="form-control"  >
                            <asp:ListItem Value="0">* Seleccione origen *</asp:ListItem>
                    </asp:DropDownList>
             
                </div>
                <div class="form-group col-md-6" style=" display:none"> </div>
                <div class="form-group col-md-6" style=" display:none">
                    <label for="inputAddress">Todas las solicitudes.<span style="color: #FF0000; font-weight: bold;"> </span></label>
                            <label for="inputPassword4"> </label>
                            <label class="checkbox-container">
                            <asp:CheckBox Text="" ID="chkTodos" runat="server" AutoPostBack="false"
                                oncheckedchanged="chkTodos_CheckedChanged" />
                                <span class="checkmark"></span>
                            </label>
             
                </div>
                <div class="form-group col-md-6"> </div>


            </div>

            <div class="row">
                <div class="col-md-12 d-flex justify-content-center">
                        <asp:Button ID="btbuscar" runat="server" Text="Iniciar la búsqueda" OnClientClick="return getGif();" class="btn btn-primary" 
                    onclick="btbuscar_Click"/>
                </div>
            </div>

            <%-- <div class="catabody" style=" height:100%">
            <div class="catawrap" >--%>

            <div class="cataresult" >
            <asp:UpdatePanel ID="upresult" runat="server" ChildrenAsTriggers="true">
                <ContentTemplate>

                    <input id="idlin" type="hidden" runat="server" clientidmode="Static" />
                    <input id="diponible" type="hidden" runat="server" clientidmode="Static" />
                    <div class="msg-alerta" id="alerta" runat="server" style=" display:none" ></div>
                    <div id="xfinder" runat="server" visible="false" >
                        <div class="findresult" >
     
                            <div class="informativo" style=" height:100%;">
          
                                <div class="bokindetalle" style="width:100%;overflow:auto">
                                    <asp:Repeater ID="tablePagination" runat="server" OnItemCommand="tablePagination_ItemCommand" OnItemDataBound="tablePagination_ItemDataBound">
                                        <HeaderTemplate>
                                            <table id="tablasort"  cellspacing="1" cellpadding="1"   class="table table-bordered invoice"  >
                                            <thead>
                                            <tr>
                                            <th># Solicitud..</th>
                                            <th class="nover">Solicitado por</th>
                                            <th style=" display:none">Ruc</th>
                                            <th style=" display:none">Tipo</th>
                                            <th>Empresa</th>
                                            <th>Tipo de Empresa</th>
                                            <th>Tipo de Solicitud</th>
                                            <th>Fecha Registro</th>
                                            <th>Usuario Atiende</th>
                                            <th>Estado</th>
                                            <th>N° Liquidación</th>
                                            <th>Valor</th>
                                            <th></th>
                                            <th></th>
                                            </tr>
                                            </thead> 
                                            <tbody>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                        <tr class="point" >
                                        <td scope="row" title="# Solicitud"><%#Eval("NUMSOLICITUD")%></td>
                                        <td class="nover" title="Solicitado por" scope="row"><%#Eval("ASO_TRANSPORTISTA")%></td>
                                        <td style=" display:none"><asp:Label ID="lruccipas"  style="text-transform :uppercase" runat="server" Text='<%# Eval("RUCCIPAS") %>'></asp:Label></td>
                                        <td style=" display:none"><%#Eval("TIPO")%></td>
                                        <td scope="row" title="Empresa"><asp:Label ID="lempresa"  style="text-transform :uppercase" runat="server" Text='<%# Eval("EMPRESA") %>'></asp:Label></td>
                                        <td scope="row" title="Tipo de Empresa"><%#Eval("TIPOEMPRESA")%></td>
                                        <td scope="row" title="Tipo de Solicitud"><%#Eval("TIPOSOLICITUD")%></td>
                                        <td scope="row" title="Fecha de Registro"><%#Eval("FECHAING")%></td>
                                        <td scope="row" title="Usuario Atiende"><%#Eval("USUARIOMOD")%></td>
                                        <td scope="row" title="Estado"><%#Eval("ESTADO")%></td>
                                        <td scope="row" title="N° Liquidación"><%#Eval("NUMERO_FACTURA")%></td>
                                        <td scope="row" title="Valor"><%#Eval("MONTO_FACTURA")%></td>
                                        <td scope="row" title="Ver detalle de la Solicitud" >
                                        <a  id="adjDoc" class="btn btn-outline-primary mr-4" onclick="redirectsol('<%# Eval("NUMSOLICITUD") %>', '<%# Eval("TIPO") %>', '<%#Eval("TIPOSOLICITUD")%>', '<%#Eval("ESTADO")%>', '<%# Eval("RUCCIPAS") %>', '<%#Eval("ASO_TRANSPORTISTA")%>', '<%#Eval("CODIGO_TIPO_SOLICITUD")%>');">
                                        <i class="fa fa-search" ></i> Ver 
                                        </a>
                                        </td>

                                        <td scope="row" title="Ver liquidación" > 
                                            <%-- <a  id="btnImprimir" class="btn btn-outline-primary mr-4" >
                                                <i class="fa fa-print" ></i> Factura 
                                            </a>--%>
                       
                                            <asp:Button runat="server" ID="btnImprimir" Height="55px" CommandName="FACTURA" Text="Factura" 
                                                CommandArgument='<%# Eval("NUMERO_FACTURA") + "," + Eval("MONTO_FACTURA") %>'  class="btn btn-primary"  data-toggle="modal" data-target="#myModal"  />

                                    <%--        <asp:Button runat="server" ID="btnImprimir" Height="30px" CommandName="Editar" Text="Ver"  CommandArgument='<%# Eval("NUMERO_FACTURA") + "," + Eval("MONTO_FACTURA") %>'  class="btn btn-primary"  data-toggle="modal" data-target="#myModal3"  />
                                            --%>
                        
                                        </td>

                                        </tr>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                        </tbody>
                                        </table>
                                        </FooterTemplate>
                                    </asp:Repeater>
                                </div>

                            </div>
                        </div>
                    </div>
                    <div id="sinresultado" runat="server" class="alert alert-warning"></div>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btbuscar" />
                       
                </Triggers>
            </asp:UpdatePanel>
            </div>

         

            <div class="modal fade" id="myModal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
            <div class="modal-dialog">
                <div class="modal-content">
                <div class="modal-header">
                    <h4 class="modal-title" id="myModalLabel">Datos de Liquidación</h4>
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                          
                </div>
                <div class="modal-body">

                    <asp:UpdatePanel ID="UPMENSAJE" runat="server" UpdateMode="Conditional" >  
                        <ContentTemplate>

                            <br>
                            </br/>
                            El N°Liquidación generado es :&nbsp; <asp:Label runat="server" ID="lblNumLiq" Font-Bold="true"></asp:Label>  <%--&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;--%>
                            <br />
                            El monto total a cancelar es : &nbsp; <asp:Label runat="server" ID="lblMonto" Font-Bold="true"></asp:Label>
                            <br>
                            </br/>
                        </ContentTemplate>
                    <%-- <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnCerrar" />
               
                    </Triggers>--%>
                    </asp:UpdatePanel>
                </div>
                <div class="modal-footer">
                        <asp:Button ID="btnCerrar" runat="server" class="btn btn-default"  Text="Cerrar"  UseSubmitBehavior="false" data-dismiss="modal" />
            <%--                          <button type="button" class="btn btn-primary" data-dismiss="modal">NO</button>--%>
                </div>
                </div>
            </div>
            </div>
         
   

<%--    <asp:updateprogress associatedupdatepanelid="uptabla"
        id="updateProgress" runat="server">
        <progresstemplate>
            <div id="progressBackgroundFilter"></div>
            <div id="processMessage"> Estamos procesando la tarea que solicitó, por favor 
                espere...<br />
                    <img alt="Loading" src="../shared/imgs/loader.gif" style="margin:0 auto;" />
            </div>
        </progresstemplate>
    </asp:updateprogress>
    
    <asp:ModalPopupExtender ID="mpeLoading" runat="server" BehaviorID="idmpeLoading" PopupControlID="pnlLoading" TargetControlID="btnLoading" EnableViewState="false" DropShadow="true" BackgroundCssClass="modalBackground" />

    <asp:Panel ID="pnlLoading" runat="server"  HorizontalAlign="Center" CssClass="modalPopup" align="center"  EnableViewState="false" Style="display: none">
        <br />Procesando información....
        <div class="body2">
            <asp:UpdatePanel ID="msgload" runat="server" UpdateMode="Conditional"  >
                <ContentTemplate>
                    <div align="center">   
                        <asp:Image ID="loading" runat="server" ImageUrl="../lib/file-uploader/img/loading.gif"  Visible="true" Width="40" Height="40" />
                    </div>
                  
                    <br/>
                        Estimado Cliente, se está procesando su solicitud....<br/>Por favor espere.... 
                    <br />
                    <br />
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </asp:Panel>
    <asp:Button ID="btnLoading" runat="server" Style="display: none" />--%>

 </div>
<%-- </form>--%>
<script src="../Scripts/pages.js" type="text/javascript"></script>
<script src="../Scripts/credenciales.js" type="text/javascript"></script>
<script src="../Scripts/jquery.datetimepicker.js" type="text/javascript"></script>

   <%-- <script src="../Scripts/jquery.tablesorter.min.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.tablesorter.pager.js" type="text/javascript"></script>--%>
    <script type="text/javascript">
        (function ($) {
            $("#header").ready(function () { $("#cargando").stop().animate({ width: "25%" }, 1500) });
            $("#footer").ready(function () { $("#cargando").stop().animate({ width: "75%" }, 1500) });
            $(window).load(function () {
                $("#cargando").stop().animate({ width: "100%" }, 600, function () {
                    $("#cargando").fadeOut("fast", function () { $(this).remove(); });
                });
            });
        })($);
        //$(document).ready(function () {
        //    $('.datetimepicker').datetimepicker().datetimepicker({ lang: 'es', timepicker: false, closeOnDateSelect: true, format: 'd/m/Y' });
        //});
        function popupCallback(objeto, catalogo) {
            if (objeto == null || objeto == undefined) {
                 alertify.alert('Hubo un problema al setaar un objeto de catalogo').set('label', 'Aceptar');
                return;
            }
            //si catalogos es booking
            if (catalogo == 'bk') {
                document.getElementById('numbook').textContent = objeto.nbr;
                document.getElementById('nbrboo').value = objeto.nbr;
                return;
            }

            //si catalogos es booking
            if (catalogo == 'cc') {
                document.getElementById('txtfecha').textContent = objeto.fecha;
                document.getElementById('xfecha').value = objeto.fecha;
                return;
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
                    alertify.alert('* Datos de programación *\n *Escriba el numero de Booking*').set('label', 'Aceptar');
                    document.getElementById("loader").className = 'nover';
                    return;
                }
                vals = document.getElementById('xfecha');
                if (vals == null || vals == undefined || vals.value.trim().length <= 5) {
                    alertify.alert('* Datos de programación *\n Escriba la fecha de programación').set('label', 'Aceptar');
                    document.getElementById("loader").className = 'nover';
                    return;
                }
                vals = document.getElementById('tmail');
                if (vals == null || vals == undefined || vals.value.trim().length <= 5) {
                    alertify.alert('* Datos de programación *\n *Escriba el correo electrónico para la notificación').set('label', 'Aceptar');
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
                            alertify.alert('El Horario ' + lista[n].desde + '-' + lista[n].hasta + ' excede su disponibilidad, favor verifique').set('label', 'Aceptar');
                            return;
                        }
                        total += parseInt(lista[n].reserva);
                    }
                }
                if (total > qtlimite) {
                    alertify.alert('* Reserva *\n La cantidad de reserva excede el cupo disponible \n Cupo: ' + qtlimite + '\n Reserva: ' + total).set('label', 'Aceptar');
                    return;
                }
                if (total <= 0) {
                    alertify.alert('* Reserva *\n La cantidad de reservas debe ser mayor que 0').set('label', 'Aceptar');
                    return;
                }
                tansporteServer(this.programacion, 'turnos.aspx/ValidateJSON');

            } catch (e) {
                alertify.alert('Lamentamos comunicarle que ha ocurrido el siguiente problema:\n' + e.Message).set('label', 'Aceptar');
            }
        }

        function openPop() {
            var bo = document.getElementById('nbrboo').value;
            if (bo == undefined || bo == '' || bo == null) {
                alertify.alert('Por favor seleccione el booking primero').set('label', 'Aceptar');
                return;
            }
            window.open('../catalogo/calendarioConsolidacion.aspx?bk=' + bo, 'name', 'width=850,height=880')
        }
        function redirectsol(val, tipo, tiposolicitud, estado, idemp, asotrans, codigo_documento) {
//            alert(tipo);
//            return;
            var caja = val;
            //window.open('../credenciales/documentos/?placa=' + caja, 'name', 'width=850,height=480')
            if (tipo == 'EMPRESA') {
                //                window.open('../credenciales/revision-solicitud-empresa/?numsolicitud=' + caja)
                window.open('../cliente/consultasolicitudempresa.aspx?numsolicitud=' + caja)
            }
            if (tipo == "PERMANENTE" || tipo == "TEMPORAL") {
                window.open('../cliente/consultasolicitudpermisodeacceso.aspx?numsolicitud=' + caja + '&ruc=' + idemp)
            }
            if (tipo == "PERMISO VEHICULAR PERMANENTE") {
                window.open('../cliente/consultasolicitudpermisodeaccesovehiculo.aspx?numsolicitud=' + caja + '&ruc=' + idemp)
            }
            if (tipo == 'COLABORADOR' && tiposolicitud == 'PERMISO PROVISIONAL')
            {
                window.open('../cliente/consultasolicitudpermisoprovisional.aspx?numsolicitud=' + caja)
                return;
            }
            if (tipo == 'COLABORADOR' && tiposolicitud == 'RENUEVA PERMISO OPC')
            {
                window.open('../cliente/solicitudCredencialesActivasOPCConsulta.aspx?numsolicitud=' + caja)
                return;
            }
            if ((asotrans != '' || asotrans != null) && estado == 'PENDIENTE' && codigo_documento != 'ATE')
            {
                window.open('../transportista/bloquea_solicitud_colaborador.aspx?numsolicitud=' + caja);
            }
            else if (tipo == 'COLABORADOR' && estado == 'FACTURADA')
            {
               
                window.open('../cliente/consultafacturacolaborador.aspx?sid1=' + caja)
            }
            else if (tipo == 'COLABORADOR' && estado != 'FACTURADA' && codigo_documento != 'ATE')
            {
                window.open('../cliente/consultasolicitudcolaborador.aspx?numsolicitud=' + caja)
            }
            else if (tipo == 'COLABORADOR' && estado != 'FACTURADA' && codigo_documento == 'ATE')
            {
                window.open('../cliente/consultasolicitudcolaborador_new.aspx?numsolicitud=' + caja)
            }
            if (tipo == 'VEHICULO' && estado == 'FACTURADA' && codigo_documento != 'ATV')
            {
                window.open('../cliente/consultafacturavehiculo.aspx?sid1=' + caja)
            }
            else if (tipo == 'VEHICULO' && estado != 'FACTURADA' && codigo_documento != 'ATV')
            {
                window.open('../cliente/consultasolicitudvehiculo.aspx?numsolicitud=' + caja)
            }
            else if (tipo == 'VEHICULO' && estado != 'FACTURADA' && codigo_documento == 'ATV')
            {
                window.open('../cliente/consultasolicitudvehiculo_new.aspx?numsolicitud=' + caja)
            }
        }
        function getGif() {document.getElementById('imagen').innerHTML = '<img alt="" src="../shared/imgs/loader.gif"/>';
            return true;
        }
        function getGifOculta() {
            document.getElementById('imagen').innerHTML = '<img alt="" src=""/>';
            return true;
        }
    </script>

     <script type="text/javascript">
            $(document).ready(function () {
                 $('.datetimepicker').datetimepicker().datetimepicker({ lang: 'es', timepicker: false, closeOnDateSelect: true, format: 'd/m/Y' });
              });
      
           
      </script>

<%--<script type="text/javascript">

    var mpeLoading;
    function initializeRequest(sender, args){
        mpeLoading = $find('idmpeLoading');
        mpeLoading.show();
        mpeLoading._backgroundElement.style.zIndex += 10;
        mpeLoading._foregroundElement.style.zIndex += 10;
    }
        function endRequest(sender, args) {
             $find('idmpeLoading').hide();

        }

    Sys.WebForms.PageRequestManager.getInstance().add_initializeRequest(initializeRequest);
    Sys.WebForms.PageRequestManager.getInstance().add_endRequest(endRequest); 

    if (typeof(Sys) !== 'undefined') Sys.Application.notifyScriptLoaded();
        $find('idmpeLoading').hide();

</script>--%>

</asp:Content>
