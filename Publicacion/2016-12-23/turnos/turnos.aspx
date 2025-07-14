<%@ Page Title="Asignar turnos" Language="C#" MasterPageFile="~/site.Master" AutoEventWireup="true" CodeBehind="turnos.aspx.cs" Inherits="CSLSite.turnos" %>
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
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="placebody" runat="server">
    <input id="zonaid" type="hidden" value="1203" />
    <asp:ToolkitScriptManager ID="tkcal" EnableScriptGlobalization="true" runat="server"> </asp:ToolkitScriptManager>
    <div>
   <i class="ico-titulo-1"></i><h2>Carga suelta</h2>  <br /> 
   <i class="ico-titulo-2"></i><h1>Asignación de turnos</h1><br />
 </div><div class="seccion">
       <div class="accion">
           <input id="agencia" type="hidden" runat="server"  clientidmode="Static"/>
            <table class="xcontroles" cellspacing="0" cellpadding="1">
        <tr><th class="bt-bottom bt-right  bt-left bt-top" colspan="3">Datos para la programación</th></tr>
         <tr>
         <td class="bt-bottom bt-left bt-right" >Booking No. :</td>
         <td class="bt-bottom bt-right" >
                   <span id="numbook" class="caja" onclick="clear();">...</span>
                   <input id="nbrboo" type="hidden" value="" runat="server" clientidmode="Static"/>
          </td>
         <td class="bt-bottom bt-right validacion ">
        <a  class="topopup" target="popup" onclick="window.open('../catalogo/book','name','width=850,height=480')" >
          <i class="ico-find" ></i> Buscar </a>
         </td>
         </tr>
          <tr>
         <td class="bt-bottom  bt-right bt-left">Fecha de programación:</td>
          <td class="bt-bottom bt-right " >
                   <span id="txtfecha" class="caja" style=" width:280px!important;" onclick="clear2();">...</span>
                   <input id="xfecha" type="hidden" value="" runat="server" clientidmode="Static" />
          </td>
             <td class="bt-bottom  bt-right ">  <a  class="topopup" target="popup" onclick="openPop();" >
             <i class="ico-find" ></i> Buscar </a>
          </td>
         </tr>
         <tr>
         <td class="bt-bottom  bt-right bt-left">Mail informativo:</td>
         <td class="bt-bottom bt-right">
               <input 
               placeholder="Para enviar a varios mails separelos con (;)"
               type='text' id='tmail' name='tmail'  runat="server" style= 'width:400px' class="date"
               enableviewstate="false" clientidmode="Static"
               onkeypress="return soloLetras(event,'abcdefghijklmnopqrstuvwxyz1234567890_@.;',true)"  
               onblur="maildatavarios(this,'valmailz');" maxlength="250"
               />
             </td>
         <td class="bt-bottom bt-right validacion ">
         <span id="valmailz" class="validacion" > * obligatorio</span>
         </td>
         </tr>
         </table>
         <div class="botonera">
          <span id="imagen"></span>
             <asp:Button ID="btbuscar" runat="server" Text="Consultar disponibilidad"   
                 onclick="btbuscar_Click"  OnClientClick="return checkDate('xfecha');"
                  />
         </div>
             <div class="cataresult" >
               <asp:UpdatePanel ID="upresult" runat="server" ChildrenAsTriggers="true">
                     <ContentTemplate>
                       <input id="idlin" type="hidden" runat="server" clientidmode="Static" />
                       <input id="diponible" type="hidden" runat="server" clientidmode="Static" />

                  <div id="xfinder" runat="server" visible="false" >
                 <div class="msg-alerta" id="alerta" runat="server" ></div>
             <div class="findresult" >
             <div class="booking" >
                  <div class="separator">Turnos disponibles</div>
                 
                 <div class="bokindetalle">
                 <asp:Repeater ID="tablePagination" runat="server"  >
                 <HeaderTemplate>
                 <table id="tabla"  cellspacing="1" cellpadding="1" class="tabRepeat">
                 <thead>
                 <tr>
                 <th>#</th>
                 <th>Desde</th>
                 <th>Hasta</th>
                 <th class="nover">Reservado</th>
                 <th >Disponible</th>
                 <th class="nover"></th>
                 <th class="nover"></th>
                 <th class="nover"></th>
                 <th>Reserva</th>
                 </tr>
                 </thead> 
                 <tbody>
                 </HeaderTemplate>
                 <ItemTemplate>
                 <tr class="point" >
                  <td><%#Eval("ROW")%></td>
                  <td><%#Eval("DESDE")%></td>
                  <td><%#Eval("HASTA")%></td>
                  <td class="nover"><%#Eval("RESERVADO")%></td>
                  <td><%#Eval("DISPONIBLE")%></td>
                  <td class="nover"><%#Eval("ID_HORARIO")%></td>
                  <td class="nover"><%#Eval("ID_HORARIO_DET")%></td>
                  <td class="nover"><%#Eval("TOTAL")%></td>
                  <td>
                      <asp:TextBox 
                      style="text-align: center" 
                      Text="0" ID="caja" CssClass="suma" xval='<%#Eval("DISPONIBLE")%>' onblur="cajaControl(this);" runat="server" MaxLength="2" onkeypress="return soloLetras(event,'01234567890',true)" ></asp:TextBox>
                   </td>
                 </tr>
                  </ItemTemplate>
                 <FooterTemplate>
                 </tbody>
                 </table>
                 </FooterTemplate>
         </asp:Repeater>
         <fieldset>
          <div id="ttd" class="botonera"   >Total reservado: 0</div>
         </fieldset>
                </div>
           <div class="botonera" runat="server" id="btnera">
              <img alt="loading.." src="../shared/imgs/loader.gif" id="loader" class="nover"  />
              <input id="btsalvar" type="button" value="Proceder y Asignar"  onclick="prepareObject();" /> &nbsp;
           </div>
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

        function popupCallback(objeto, catalogo) {
            if (objeto == null || objeto==undefined) {
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
            window.open('../catalogo/Calendario.aspx?bk='+bo, 'name', 'width=850,height=480')
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
