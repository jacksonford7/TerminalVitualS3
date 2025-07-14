<%@ Page Title="" Language="C#" MasterPageFile="~/site.Master" AutoEventWireup="true" 
         CodeBehind="mesaServicio.aspx.cs" Inherits="CSLSite.mesaServicio" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="placehead" runat="server">
    <link href="../shared/estilo/tables.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/aisv-general.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/work.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/catalogos.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/info.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/turnos.js" type="text/javascript"></script>
    <link href="../shared/estilo/centroSolicitud.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/centroServicios.js" type="text/javascript"></script>
    <script src="../Scripts/pages.js" type="text/javascript"></script>
    <script type="text/javascript">
        function BindFunctions() {
            $(document).ready(function () {
                document.getElementById('imagen').innerHTML = '';
                $('#tablaAnalistas').tablesorter({ cancelSelection: true, cssAsc: "ascendente", cssDesc: "descendente", headers: { 8: { sorter: false }, 9: { sorter: false}} }).tablesorterPager({ container: $('#pager'), positionFixed: false, pagesize: 10 });
            });
        }
    </script>
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
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="placebody" runat="server">
    <input id="zonaid" type="hidden" value="706" />
 <asp:ToolkitScriptManager ID="tkcal" EnableScriptGlobalization="true" runat="server"> </asp:ToolkitScriptManager>
 <div>
 <i class="ico-titulo-1"></i><h2>Administración de Servicios</h2>  <br />
 <i class="ico-titulo-2"></i><h1>Pool de Servicios</h1><br />
 </div>
 <div class="seccion">
       <div class="accion">
       <table class="xcontroles" cellspacing="0" cellpadding="1">
       <tr><th class="bt-bottom bt-right bt-left bt-top" colspan="4"> Criterios de consulta:</th></tr>
       <tr>
            <td class="bt-bottom bt-right bt-left" >No. Solicitud:</td>
            <td class="bt-bottom bt-right">
                 <asp:TextBox ID="txtNumSolicitud" runat="server" Width="250px" MaxLength="30"
                 style="text-align: center"></asp:TextBox>
                 <%--onkeypress="return soloLetras(event,'01234567890',true)"--%>
            </td>
            <td class="bt-bottom bt-right validacion "></td>
       </tr>       
       <tr>
          <td class="bt-bottom bt-right bt-left">Tipo de Servicio:</td>
          <td class="bt-bottom bt-right">
              <%--<a class="tooltip" >
              <span class="classic">Esta informacion contiene los tipos de servicios disponibles.</span>--%>
              <asp:DropDownList ID="dptiposervicios" runat="server" Width="200px" >
                     <asp:ListItem Value="0">* Seleccione servicios *</asp:ListItem>
              </asp:DropDownList>
              <%--</a>--%>
           </td>
           <td class="bt-bottom bt-right validacion "></td>
       </tr>
       <tr class="nover">
            <td class="bt-bottom bt-right bt-left">Tipo de Usuario:</td>
            <td class="bt-bottom bt-right">
              <%--<a class="tooltip" >
              <span class="classic">Esta informacion contiene los tipos de servicios disponibles.</span>--%>
              <asp:DropDownList ID="dptipousuario" runat="server" Width="200px" AppendDataBoundItems="true">                     
              </asp:DropDownList>
              <%--</a>--%>          
            </td>
            <td class="bt-bottom bt-right validacion "></td>
       </tr>       
       <tr>
         <td class="bt-bottom bt-right bt-left">Generados desde / hasta:</td>
         <td class="bt-bottom bt-right">
            <%--<a class="tooltip" >
            <span class="classic">Fecha desde.</span>--%>
            <asp:TextBox ID="desded" runat="server" Width="120px" MaxLength="10" CssClass="datetimepicker"
             onkeypress="return soloLetras(event,'01234567890/')" 
                 onblur="valDate(this,false,valdate);" ClientIDMode="Static"></asp:TextBox>
            <%--</a>
            <a class="tooltip" >
            <span class="classic">Fecha hasta.</span>--%>
            <asp:TextBox ID="hastad" runat="server" ClientIDMode="Static" Width="120px" MaxLength="15" CssClass="datetimepicker"
             onkeypress="return soloLetras(event,'01234567890/')" 
                  onblur="valDate(this,false,valdate);"></asp:TextBox>
            <%--</a>--%>
          </td>
          <td class="bt-bottom bt-right validacion "><span id="valdate" class="opcional"> * opcional</span></td>
        </tr>
       <tr>
            <td class="bt-bottom  bt-right bt-left">Estado:</td>
            <td class="bt-bottom bt-right">
              <%--<a class="tooltip" >
              <span class="classic">Esta informacion contiene los estados disponibles.</span>--%>
              <asp:DropDownList ID="dpestados" runat="server" Width="250px"  >
                     <asp:ListItem Value="0">* Seleccione estados *</asp:ListItem>
              </asp:DropDownList>
              <%--</a>--%>
            </td>
            <td class="bt-bottom bt-right validacion "></td>
       </tr>
       <tr class="nover">
         <td class="bt-bottom bt-right bt-left">Todas las solicitudes.</td>
         <td class="bt-bottom bt-right">
             <asp:CheckBox Text="" ID="chkTodos" runat="server" />
         </td>
         <td class="bt-bottom bt-right validacion "></td>
       </tr>
       </table>
       <div class="botonera">
            <input clientidmode="Static" id="dataexport" onclick="getdata('Analista_REP');"
            type="button" value="Exportar" runat="server" />    
            
            
                                
            <span id="imagen"></span>
            <asp:Button ID="btbuscar" runat="server" Text="Iniciar la búsqueda" 
            onclick="btbuscar_Click" OnClientClick="showGif('imagen')"/>
       </div>
       <div class="cataresult" >
        <asp:UpdatePanel ID="upresult" runat="server" ChildrenAsTriggers="true">
        <ContentTemplate>
                <script type="text/javascript">
                    Sys.Application.add_load(BindFunctions); 
                </script>
        <input id="idlin" type="hidden" runat="server" clientidmode="Static" />
        <input id="diponible" type="hidden" runat="server" clientidmode="Static" />
        <div id="xfinder" runat="server" visible="false" >
        <div class="msg-alerta" id="alerta" runat="server" ></div>        
        <div class="findresult" >
        <div class="booking" >
                 <div class="separator">Contenedor(es):</div>
                 <div class="bokindetalle">
                 <asp:Repeater ID="tbPaginationGeneral" runat="server"  >
                 <HeaderTemplate>
                 <table id="tablaAnalistas"  cellspacing="1" cellpadding="1" class="tabRepeat">
                 <thead>
                 <tr>                 
                     <th>No.Sol</th>
                     <th>Servicio</th>                 
                     <th>Usuario</th>
                     <th>Empresa</th>
                     <th>Fecha de Solicitud</th>
                     <th>Estado</th>
                     <th></th>
                     <th></th>
                 </tr>
                 </thead> 
                 <tbody>
                 </HeaderTemplate>
                 <ItemTemplate>
                 <tr class="point" >                  
                  <td><%#Eval("numSolicitud")%></td>                  
                  <td><%#Eval("servicio")%></td>
                  <td><%#Eval("usuario")%></td>
                  <td><%#Eval("Exportador")%></td>
                  <td><%#Eval("fechaSolicitud")%></td>
                  <td><%#Eval("estado")%></td>
                  <td>
                   <%--<div class="tcomand">
                       <asp:Button ID="btverdocumento"
                       runat="server" Text="Ver" CssClass="Anular" ToolTip="Permite ver el detalle del contenedor." />
                   </div>--%>
                    <a class="topopup" target="popup" onclick="linkContenedoresBySolicitud('<%#Eval("idSolicitud")%>');" >
                    <i class="ico-find" ></i> Ver </a>
                  </td>
                  <td>
                    <%--<a href="impresion.aspx?sid=<%# securetext(Eval("nombreDocumento")) %>" class="imprimir" target="_blank">Imprimir</a>|--%>
                    <a href="../handler/getFile.ashx?id=<%#Eval("nombreDocumento")%>" class="imprimir" target="_blank" style='<%#Eval("nombreDocumento") == null ? "display:none;": "display:block;"%>'>Imprimir</a>
                  </td>
                 </tr>
                 </ItemTemplate>
                 <FooterTemplate>
                 </tbody>
                 </table>                 
                 </FooterTemplate>
         </asp:Repeater>
                </div>
        <%--<div class="botonera" runat="server" id="btnera">
        <img alt="loading.." src="../shared/imgs/loader.gif" id="loader" class="nover"  />
        <input id="btsalvar" type="button" value="Proceder y Asignar"  onclick="prepareObject();encerar();" /> &nbsp;
        </div>--%>
        </div>
            <div id="pager">
               Registros por página
                  <select class="pagesize">
                  <option selected="selected" value="10">10</option>
                  <option value="20">20</option>
                  </select>
                 <img alt="" src="../shared/imgs/first.gif" class="first"/>
                 <img alt="" src="../shared/imgs/prev.gif" class="prev"/>
                 <input  type="text" class="pagedisplay" size="5px"/>
                 <img alt="" src="../shared/imgs/next.gif" class="next"/>
                 <img alt="" src="../shared/imgs/last.gif" class="last"/>                 
            </div>
        </div>
        </div>
        <div id="sinresultado" runat="server" class="msg-info"></div>
        </ContentTemplate>
                     <Triggers>
                         <asp:AsyncPostBackTrigger ControlID="btbuscar" />
                     </Triggers>
        </asp:UpdatePanel>
        </div>
       </div>
 </div>
 <script src="../Scripts/pages.js" type="text/javascript"></script>
    <script src="../Scripts/exportar.js" type="text/javascript"></script>
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

        function popupCallback(objeto, catalogo) {
            if (objeto == null || objeto == undefined) {
                alert('Hubo un problema al setear un objeto de catalogo');
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
<asp:updateprogress  id="updateProgress" runat="server">
    <progresstemplate>
        <div id="progressBackgroundFilter"></div>
        <div id="processMessage"> Estamos procesando la tarea que solicitó, por favor  espere...<br />
         <img alt="Loading" src="../shared/imgs/loader.gif" style="margin:0 auto;" />
        </div>
    </progresstemplate>
</asp:updateprogress>
</asp:Content>
