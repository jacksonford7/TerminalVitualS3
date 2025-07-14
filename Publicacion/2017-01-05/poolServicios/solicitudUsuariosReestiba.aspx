<%@ Page Title="" Language="C#" MasterPageFile="~/site.Master" AutoEventWireup="true" 
         CodeBehind="solicitudUsuariosReestiba.aspx.cs" Inherits="CSLSite.solicitudUsuariosReestiba" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="placehead" runat="server">
    <link href="../shared/estilo/tables.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/aisv-general.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/work.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/catalogos.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/info.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/centroServicios.js" type="text/javascript"></script>
    <link href="../shared/estilo/centroSolicitud.css" rel="stylesheet" type="text/css" />

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
    <input id="zonaid" type="hidden" value="713" />
 <asp:ToolkitScriptManager ID="tkcal" EnableScriptGlobalization="true" runat="server"> </asp:ToolkitScriptManager>
 <div>
 <i class="ico-titulo-1"></i><h2>Ingreso de Solicitud (Reestiba de Carga)</h2>  
     <br />
     <br />
 <i class="ico-titulo-2"></i><h1>Solicitud</h1>
   <div class="msg-alerta" id="alerta" runat="server" >Nota: Si la solicitud es generada posterior a las 13H00 la misma será planificada para el siguiente día, en la hora disponible informada por nuestra área de CFS.  </div>
 </div>
 <asp:UpdatePanel ID="upresult" runat="server">
    <ContentTemplate>
       <div class="seccion">
       <div class="accion">
            <table class="xcontroles" cellspacing="0" cellpadding="1">
       <tr><th class="bt-bottom bt-right bt-left bt-top" colspan="4"> Criterios de consulta:</th></tr>
       <tr>
          <td class="bt-bottom bt-right bt-left">Servicios:</td>
          <td class="bt-bottom bt-right" colspan="3">
          <%--<a class="tooltip" >
          <span class="classic">Esta informacion contiene los tipos de servicios disponibles.</span>--%>
          <asp:DropDownList ID="dptiposervicios" runat="server" Width="300px" >
                 <asp:ListItem Value="0">* Seleccione servicios *</asp:ListItem>
          </asp:DropDownList>
          <%--</a>--%>
        </td>
       </tr> 
       <tr>
          <td class="bt-bottom bt-right bt-left">Tráfico:</td>
          <td class="bt-bottom bt-right" colspan="3">
          <%--<a class="tooltip" >
          <span class="classic">Esta informacion contiene los tipos de servicios disponibles.</span>--%>
          <asp:DropDownList ID="dptipotrafico" runat="server" Width="300px" 
          onchange="selectTrafico($('[id*=dptipotrafico]').val(),$('[id*=bookingContenedor]').val())" >
                 <asp:ListItem Value="0">* Seleccione tráfico *</asp:ListItem>
          </asp:DropDownList>
          <%--</a>--%>
        </td>        
       </tr>       
       <tr>
        <td class="bt-bottom bt-right bt-left" >Contenedor Lleno:</td>
        <td class="bt-bottom">
             <asp:TextBox ID="contenedor1" runat="server"  
             style="text-align:center;" Text="..." Width="90%" Enabled="false"></asp:TextBox>
        <%--</td>
        <td class="bt-bottom">
          <span id="contenedor1" class="caja">...</span>--%>
          <input id="contenedorNombreUno" runat="server" type="hidden" />
          <input id="contenedor1HF" runat="server" type="hidden" />
         </td>
         <td class="bt-bottom finder">
            <a class="topopup" target="popup" onclick="linkcontenedor1('<%=dptipotrafico.ClientID %>','FCL');" >
            <i class="ico-find" ></i> Buscar </a>
         </td>     
        <td class="bt-bottom bt-right validacion "><span id="valcont2" class="validacion"> * obligatorio</span></td>
       </tr>
       <tr>
            <td class="bt-bottom bt-right bt-left">Booking Contenedor:</td>
            <td class="bt-bottom">
                <asp:TextBox ID="bookingContenedor" runat="server"  
                style="text-align:center;" Text="..." Width="90%" Enabled="false"></asp:TextBox>
              <%--<span id="bookingContenedor" class="caja">...</span>--%>
              <input id="bookingContenedorHF"  runat="server" type="hidden" />
            </td>
            <td class="bt-bottom bt-right" colspan="2"></td>
       </tr>
       <tr>
            <td class="bt-bottom bt-right bt-left" >Contenedor Vacio:</td>
            <td class="bt-bottom">
                <asp:TextBox ID="contenedor2" runat="server"  
                style="text-align:center;" Width="90%" Text="..." Enabled="false"></asp:TextBox>
                <%--<span id="contenedor2" class="caja">...</span>--%>
                <input id="contenedor2HF" runat="server" type="hidden" />
                <input id="contenedorNombreDos" runat="server" type="hidden" />
            </td>
            <td class="bt-bottom finder">
                <a class="topopup" target="popup" onclick="linkcontenedor2('<%=dptipotrafico.ClientID %>','<%=contenedor1HF.ClientID %>','PTY');" >
                <i class="ico-find" ></i> Buscar </a>
            </td> 
            <td class="bt-bottom bt-right validacion "><span id="valcont" class="validacion"> * obligatorio</span></td>
       </tr>   
       <tr>
            <td class="bt-bottom bt-right bt-left" >Fecha/Hora propuesta para la operación:</td>
            <td class="bt-bottom bt-right" colspan="2">
                <asp:TextBox ID="txtFechaPropuesta" runat="server" Width="80%" MaxLength="10" CssClass="datetimepicker"
                onkeypress="return soloLetras(event,'01234567890/')" 
                onblur="valDate(this,false,valdate);" ClientIDMode="Static"></asp:TextBox>           
            </td>
            <td class="bt-bottom bt-right validacion "><span id="valdate" class="validacion"> * obligatorio</span></td>
       </tr>  
       <tr>
            <td class="bt-bottom bt-right bt-left" >Tipo de producto cantidad y embalaje:</td>
            <td class="bt-bottom bt-right" colspan="2">
                 <asp:TextBox ID="txtTipoProductoEmbalaje" runat="server"  
                 style="text-align:center;" Text="" 
                 CssClass="mayusc" placeholder="Ejem: 1014 Cajas de Banano/2 Motores 1TON" MaxLength="30" Width="80%"></asp:TextBox>                 
            </td>            
            <td class="bt-bottom bt-right validacion "><span id="valtipopro" class="validacion"> * obligatorio</span></td>
       </tr>
       <tr>
            <td class="bt-bottom bt-right bt-left" >Comentario:</td>
            <td class="bt-bottom bt-right" colspan="2">
                 <asp:TextBox ID="txtComentario" runat="server"  
                 style="text-align:center;" CssClass="mayusc"  MaxLength="30" Width="80%"></asp:TextBox>                 
            </td>            
            <td class="bt-bottom bt-right validacion "><span id="Span3" class="validacion"> * obligatorio</span></td>
       </tr>   
       <tr>
            <td class="bt-bottom bt-right bt-left" >Número de Solicitud de Reestiba:</td>
            <td class="bt-bottom bt-right" colspan="2">
                 <asp:TextBox ID="txtNumDocAduana" runat="server"  
                 style="text-align:center;" CssClass="mayusc" placeholder="123456782016RE000001P"
              onkeypress="return soloLetras(event,'abcdefghijklmnñopqrstuvwxyz1234567890')"
              Text="" MaxLength="21" Width="80%"></asp:TextBox>                 
            </td>            
            <td class="bt-bottom bt-right validacion"><span id="Span1" class="validacion"> * obligatorio</span></td>
       </tr>
       <tr> 
            <td class="bt-bottom bt-right bt-left" >Documento PDF (Aduana):</td>
            <td class="bt-bottom bt-right" colspan="2">                                            
               
                <input class="uploader" id="archivoAduana" title="Escoja el archivo PDF" accept="pdf" type="file"  runat="server" clientidmode="Static" />              
            </td>                        
            <td class="bt-bottom bt-right validacion"><span id="valar" class="validacion"> * obligatorio</span></td>
       </tr>    
       </table>
       <div class="botonera">
            <span id="imagen" runat="server"></span>
            <input type="button" id="btgenerar" value="Generar solicitud" onclick="confirmGenerarSolicitud()"/>
            <asp:Button ID="btgenerarServer" runat="server" Text="Generar solicitud" OnClientClick="showGif('placebody_imagen')"
            OnClick="btgenerarServer_Click"  style="display:none;" />
          <%--<asp:Button ID="btgenerar" runat="server" Text="Generar solicitud" 
               onclick="btgenerar_Click" OnClientClick="showGif('placebody_imagen')"/>--%>
       </div>
       
       <div class="cataresult" >        
        
        <input id="idlin" type="hidden" runat="server" clientidmode="Static" />
        <input id="diponible" type="hidden" runat="server" clientidmode="Static" />        
        <div id="sinresultado" runat="server" class="msg-info"></div>
        
        </div>
       </div>
 </div>
    </ContentTemplate>
    <Triggers>
        <asp:PostBackTrigger ControlID="btgenerarServer"/>
    </Triggers>
</asp:UpdatePanel>
 <script src="../Scripts/pages.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.tablesorter.min.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.tablesorter.pager.js" type="text/javascript"></script>
    <script type="text/javascript">
        function confirmGenerarSolicitud() {

            if (!ValidateFile('archivoAduana', valar)) {
                return false;
            }


            var r = confirm("Se generará una nueva solicitud, ¿Está seguro de realizar la operación?");
            if (r == true) {
                $("#<%=btgenerarServer.ClientID%>").click();
            }
            else {
                return;
            }
        }

        function showGif(ctrl) {
            if (ctrl != null && ctrl != undefined && ctrl.trim().length > 0) {
                document.getElementById(ctrl).innerHTML = '<img alt="" src="../shared/imgs/loader.gif">'
            }
        }

        $(document).ready(function () {
            $('.datetimepicker').datetimepicker().datetimepicker({ lang: 'es', timepicker: true, closeOnDateSelect: true, format: 'd/m/Y H:i' });
        });

        function popupCallback(objeto) {
  

            if (objeto == null || objeto == undefined) {
                alert('Hubo un problema al setear un objeto de catalogo');
                return;
            }
           
            //si contenedor está lleno (FCL) o vacio (MTY)
              if (objeto.tipoContenedor == 'FCL') {
                document.getElementById('<%=contenedor1.ClientID %>').value = objeto.codigo;
                document.getElementById('<%=contenedorNombreUno.ClientID %>').value = objeto.codigo;
                document.getElementById('<%=contenedor1HF.ClientID %>').value = objeto.item;
                document.getElementById('<%=bookingContenedor.ClientID %>').value = objeto.booking;
                document.getElementById('<%=bookingContenedorHF.ClientID %>').value = objeto.booking;
                return;
            } else if (objeto.tipoContenedor == 'MTY' || objeto.tipoContenedor == 'LCL' ) {
                document.getElementById('<%=contenedor2.ClientID %>').value = objeto.codigo;
                document.getElementById('<%=contenedorNombreDos.ClientID %>').value = objeto.codigo;
                document.getElementById('<%=contenedor2HF.ClientID %>').value = objeto.item;
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
