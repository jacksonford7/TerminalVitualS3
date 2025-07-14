<%@ Page Language="C#" MasterPageFile="~/site.Master" AutoEventWireup="true" Title="Actualizar datos de Empresa"
         CodeBehind="consultasolicitud.aspx.cs" Inherits="CSLSite.cliente.consultasolicitud" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="placehead" runat="server">
    <link href="../shared/estilo/tables.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/aisv-general.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/work.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/catalogos.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/info.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/turnos.js" type="text/javascript"></script>
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
<asp:Content ID="Content2" ContentPlaceHolderID="placebody" runat="server">
    <input id="zonaid" type="hidden" value="2" />
 <asp:ToolkitScriptManager ID="tkcal" EnableScriptGlobalization="true" runat="server"> </asp:ToolkitScriptManager>
 <div>
 <i class="ico-titulo-1"></i><h2>MCA - Módulo de Control de Acceso</h2><h2>&nbsp;</h2>  <br />
 <i class="ico-titulo-2"></i><h1>Actualizar datos de Empresa</h1><h1>&nbsp;</h1>
     <br />
 </div>
 <div class="seccion">
       <div class="accion">
       <table class="xcontroles" cellspacing="0" cellpadding="1">
       <tr><th class="bt-bottom bt-right bt-left bt-top" colspan="4"> Criterios de consulta:</th></tr>
       <tr>
        <td class="bt-bottom bt-right bt-left">Número de Solicitud:</td>
        <td class="bt-bottom bt-right">
             <asp:TextBox ID="txtsolicitud" runat="server" Width="120px" MaxLength="11"
             style="text-align: center" onblur="cajaControl(this);"
             onkeypress="return soloLetras(event,'01234567890',true)"></asp:TextBox>
        </td>
       </tr>
       <tr style=" display:none">
        <td class="bt-bottom bt-right bt-left" >RUC/CI/PAS:</td>
        <td class="bt-bottom bt-right">
             <asp:TextBox ID="txtruccipas" runat="server" Width="200px" MaxLength="25"
             style="text-align: center" onblur="cajaControl(this);"
             onkeypress="return soloLetras(event,'01234567890abcdefghijklnmñopqrstuvwxyz',true)"></asp:TextBox>
        </td>
       </tr>
       <tr style=" display:none">
          <td class="bt-bottom bt-right bt-left" Tipo de Solicitud:</td>
          <td class="bt-bottom bt-right">
          <%--<a class="tooltip" >
          <span class="classic">Esta informacion contiene los tipos de solicitudes disponibles.</span>--%>
          <asp:DropDownList ID="dptiposolicitud" runat="server" Width="200px" >
                 <asp:ListItem Value="0">* Seleccione origen *</asp:ListItem>
          </asp:DropDownList>
          <%--</a>--%>
        </td>
        </tr>
       <tr style=" display:none">
        <td class="bt-bottom bt-left bt-right">Tipo de Usuario:</td>
        <td class="bt-bottom bt-right">
          <%--<a class="tooltip" >
          <span class="classic">Esta informacion contiene los tipos de usuarios disponibles.</span>--%>
          <asp:DropDownList ID="dptipousuario" runat="server" Width="300px" >
                 <asp:ListItem Value="0">* Seleccione origen *</asp:ListItem>
          </asp:DropDownList>
          <%--</a>--%>
        </td>
        </tr>
       <tr>
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
       <tr style=" display:none"><td class="bt-bottom  bt-right bt-left">Estado:</td>
       <td class="bt-bottom bt-right">
          <%--<a class="tooltip" >
          <span class="classic">Esta informacion contiene los estados disponibles.</span>--%>
          <asp:DropDownList ID="dpestados" runat="server" Width="250px"  >
                 <asp:ListItem Value="0">* Seleccione origen *</asp:ListItem>
          </asp:DropDownList>
          <%--</a>--%>
       </td>
       </tr>
       <tr>
         <td class="bt-bottom bt-right bt-left">Todas las solicitudes.</td>
         <td class="bt-bottom bt-right">
             <asp:CheckBox Text="" ID="chkTodos" runat="server" />
         </td>
       </tr>
       </table>
       <div class="botonera">
          <img alt="loading.." src="../shared/imgs/loader.gif" id="loader" class="nover"  />
             <asp:Button ID="btbuscar" runat="server" Text="Iniciar la búsqueda" 
               onclick="btbuscar_Click"/>
       </div>
       <div class="cataresult" >
        <asp:UpdatePanel ID="upresult" runat="server" ChildrenAsTriggers="true">
        <ContentTemplate>
        <input id="idlin" type="hidden" runat="server" clientidmode="Static" />
        <input id="diponible" type="hidden" runat="server" clientidmode="Static" />
                <div class="msg-alerta" id="alerta" runat="server" ></div>
        <div id="xfinder" runat="server" visible="false" >
        <div class="findresult" >
        <%--<div class="booking" >--%>
        <div class="informativo" style=" height:100%; overflow:auto">
                 <div class="separator">Solicitudes disponibles:</div>
                 <div class="bokindetalle">
                 <asp:Repeater ID="tablePagination" runat="server"  >
                 <HeaderTemplate>
                 <table id="tabla"  cellspacing="1" cellpadding="1" class="tabRepeat">
                 <thead>
                 <tr>
                 <th># Solicitud</th>
                 <th style=" display:none"># Solicitud Encrypt</th>
                 <th style=" display:none">Ruc</th>
                 <th style=" display:none">Tipo</th>
                 <th>Empresa</th>
                 <th>Tipo de Empresa</th>
                 <th>Tipo de Solicitud</th>
                 <th>Usuario Solicita</th>
                 <th>Usuario Atiende</th>
                 <th>Fecha Registro</th>
                 <th>Estado</th>
                 <th></th>
                 </tr>
                 </thead> 
                 <tbody>
                 </HeaderTemplate>
                 <ItemTemplate>
                 <tr class="point" >
                  <td style=" width:60px"><%#Eval("NUMSOLICITUD")%></td>
                  <td style=" display:none"><%#Eval("ENCRYPTNUMSOLICITUD")%></td>
                  <td style=" display:none"><asp:Label ID="lruccipas"  style="text-transform :uppercase" runat="server" Width="70px" Text='<%# Eval("RUCCIPAS") %>'></asp:Label></td>
                  <td style=" display:none"><%#Eval("TIPO")%></td>
                  <td style=" width:125px"><asp:Label ID="lempresa"  style="text-transform :uppercase" runat="server" Width="125px" Text='<%# Eval("EMPRESA") %>'></asp:Label></td>
                  <td style=" width:225px"><%#Eval("TIPOEMPRESA")%></td>
                  <td style=" width:200px"><%#Eval("TIPOSOLICITUD")%></td>
                  <td style=" width:80px"><%#Eval("USUARIOING")%></td>
                  <td style=" width:80px"><%#Eval("USUARIOMOD")%></td>
                  <td style=" width:80px"><%#Eval("FECHAING")%></td>
                  <td style=" width:80px"><%#Eval("ESTADO")%></td>
                  <td>
                    <a style=" width:50px" id="adjDoc" class="topopup" onclick="redirectsol('<%# Eval("ENCRYPTNUMSOLICITUD") %>', '<%# Eval("TIPO") %>');">
                    <i class="ico-find" ></i> Ver 
                    </a>
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
    <script src="../Scripts/jquery.tablesorter.min.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.tablesorter.pager.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $('.datetimepicker').datetimepicker().datetimepicker({ lang: 'es', timepicker: false, closeOnDateSelect: true, format: 'd/m/Y' });
        });

        function popupCallback(objeto, catalogo) {
            if (objeto == null || objeto == undefined) {
                alert('Hubo un problema al setaar un objeto de catalogo');
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
        function redirectsol(val, tipo) {
            var caja = val;
            //window.open('../credenciales/documentos/?placa=' + caja, 'name', 'width=850,height=480')
            window.open('../cliente/actualizar-datos-empresa/?sid=' + caja)
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
