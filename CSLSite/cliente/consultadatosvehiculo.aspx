﻿<%@ Page  Language="C#" MasterPageFile="~/site.Master" AutoEventWireup="true" Title="Consulta de Vehiculos"
         CodeBehind="consultadatosvehiculo.aspx.cs" Inherits="CSLSite.consultadatosvehiculo" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="placehead" runat="server">

    <link href="../img/favicon2.png" rel="icon"/>
  <link href="../img/icono.png" rel="apple-touch-icon"/>
  <link href="../css/bootstrap.min.css" rel="stylesheet"/>
  <link href="../css/dashboard.css" rel="stylesheet"/>
  <link href="../css/icons.css" rel="stylesheet"/>
  <link href="../css/style.css" rel="stylesheet"/>
  <link href="https://fonts.googleapis.com/css2?family=Roboto:wght@300;400;500;700&display=swap" rel="stylesheet"/>

      <%--<link href="../shared/estilo/Reset.css" rel="stylesheet" />
 
     <link href="../shared/estilo/tables.css" rel="stylesheet" type="text/css" />
     <link href="../shared/estilo/info.css" rel="stylesheet" type="text/css" />--%>

        <%--<link href="../shared/estilo/catalogos.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/work.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/aisv-general.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/tables.css" rel="stylesheet" type="text/css" />
     <script src="../Scripts/jquery-1.7.1.min.js" type="text/javascript"></script>
    <script src="../Scripts/pages.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.tablesorter.min.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.tablesorter.pager.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $('#tablasort').tablesorter({ cancelSelection: true, cssAsc: "ascendente", cssDesc: "descendente", headers: { 5: { sorter: false }, 6: { sorter: false}} }).tablesorterPager({ container: $('#pager'), positionFixed: false, pagesize: 10 });
        });
        function imprSelec(muestra) {
            var ficha = document.getElementById(muestra); var ventimp = window.open(' ', 'popimpr'); ventimp.document.write(ficha.innerHTML); ventimp.document.close(); ventimp.print(); ventimp.close();
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
    .input_type_text
    {
        text-align:left!important;
        }
</style>--%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="placebody" runat="server">
    <input id="zonaid" type="hidden" value="2" />
 <asp:ToolkitScriptManager ID="tkcal" EnableScriptGlobalization="true" runat="server"> </asp:ToolkitScriptManager>
 
    <div class="mt-4">         
        <nav class="mt-4" aria-label="breadcrumb">
          <ol class="breadcrumb">
            <li class="breadcrumb-item" id="opcion_principal" runat="server"><a href="#">MCA - Módulo de Control de Acceso</a></li>
            <li class="breadcrumb-item active" aria-current="page" id="sub_opcion" runat="server">Consulta de Vehículos</li>
          </ol>
        </nav>
      </div>


      <div class="dashboard-container p-4" id="cuerpo" runat="server">
         <div class="form-title">
            Criterios de consulta:
        </div>
      <div class="form-row">

        <div class="form-group col-md-12">
                 <label for="inputAddress">Placa o Placas separadas por coma(,):  <%--<span style="color: #FF0000; font-weight: bold;"> *</span>--%></label> 
                           <asp:TextBox ID="txtPlaca" runat="server" class="form-control"  placeholder="Ejemplo: ABC1234,ABC5678"
             style="text-align: center" onblur="cajaControl(this);"
             onkeypress="return soloLetras(event,'abcdefghijklmnñopqrstuvwxyz01234567890-,',true)"></asp:TextBox>
             
        </div>

        <div class="form-group col-md-12" >
            <label for="inputAddress">Todos los vehículos.<span style="color: #FF0000; font-weight: bold;"> </span></label>
                    <label for="inputPassword4"> </label>
                    <label class="checkbox-container">
                    <asp:CheckBox Text="" ID="chkTodos" runat="server" AutoPostBack="false" Checked="true"
                        oncheckedchanged="chkTodos_CheckedChanged" />
                        <span class="checkmark"></span>
                    </label>
             
        </div>
    </div>

    <div class="row">
    <div class="col-md-12 d-flex justify-content-center">
            <asp:Button ID="btbuscar" runat="server" Text="Iniciar la búsqueda" OnClientClick="return getGif();" class="btn btn-primary" 
        onclick="btbuscar_Click"/>
    </div>
    </div>
       
       <div class="cataresult" >
        <%--<asp:UpdatePanel ID="upresult" runat="server" ChildrenAsTriggers="true">
        <ContentTemplate>--%>
        <input id="idlin" type="hidden" runat="server" clientidmode="Static" />
        <input id="diponible" type="hidden" runat="server" clientidmode="Static" />
        <div id="xfinder" runat="server" visible="false" >
        <div class="msg-alerta" id="alerta" runat="server" ></div>
        <div class="findresult" >
        <%--<div class="booking" >--%>
        <div class="informativo" style=" height:100%; overflow:auto">
                 <div class="separator">Vehículos disponibles:</div>
                 <div class="bokindetalle" style="width:100%;overflow:auto">
                 <asp:Repeater ID="tablePagination" runat="server"  >
                 <HeaderTemplate>
                 <table id="tablasort"  cellspacing="1" cellpadding="1" class="table table-bordered invoice">
                 <thead>
                 <tr>
                 <th>Empresa</th>
                 <th>Placa</th>
                 <th>Fecha de Poliza</th>
                 <th>Estado</th>
                 <th>Novedad</th>
                 <th></th>
                 </tr>
                 </thead> 
                 <tbody>
                 </HeaderTemplate>
                 <ItemTemplate>
                 <tr class="point" >
                  <td><%#Eval("EMPE_NOM")%></td>
                  <td><%#Eval("VE_PLACA")%></td>
                  <td><%#Eval("VE_POLIZA")%></td>
                  <td><%#Eval("ESTADO")%></td>
                  <td><%#Eval("NOVEDAD")%></td>
                 </tr>
                 </ItemTemplate>
                 <FooterTemplate>
                 </tbody>
                 </table>
                 </FooterTemplate>
         </asp:Repeater>
                </div>
                <div id="pager">
             Registros por página
                     <select class="pagesize">
                  <option selected="selected" value="10">10</option>
                  <option value="20">20</option>
                  <option value="30">30</option>
                  <option value="40">40</option>
                  <option value="50">50</option>
                  </select>
                 <img alt="" src="../shared/imgs/first.gif" class="first"/>
                 <img alt="" src="../shared/imgs/prev.gif" class="prev"/>
                 <input  type="text" class="pagedisplay" size="5px"/>
                 <img alt="" src="../shared/imgs/next.gif" class="next"/>
                 <img alt="" src="../shared/imgs/last.gif" class="last"/>
            </div>
        <%--<div class="botonera" runat="server" id="btnera">
        <img alt="loading.." src="../shared/imgs/loader.gif" id="loader" class="nover"  />
        <input id="btsalvar" type="button" value="Proceder y Asignar"  onclick="prepareObject();encerar();" /> &nbsp;
        </div>--%>
        </div>
        </div>
        </div>
                    <fieldset>
        <br />
        <a class="btn btn-outline-primary mr-4" href="#" target="_blank" runat="server" id="aprint" clientidmode="Static" >
            <span class='fa fa-print' style='font-size:24px' id="BtnBuscarLinea" clientidmode="Static">Imprimir</span>
        </a>

        </fieldset>
        <div id="sinresultado" runat="server" class="msg-info"></div>
        <%--</ContentTemplate>
                     <Triggers>
                         <asp:AsyncPostBackTrigger ControlID="btbuscar" />
                     </Triggers>
        </asp:UpdatePanel>--%>
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
            window.open('../catalogo/calendarioConsolidacion.aspx?bk=' + bo, 'name', 'width=850,height=880')
        }
        function redirectsol(val, val2, val3) {
            var caja = val;
            var caja2 = val2;
            var caja3 = val3;
            //window.open('../credenciales/documentos/?placa=' + caja, 'name', 'width=850,height=480')
            window.open('../credenciales/consulta/documentos-solicitud-vehiculo/?numsolicitud=' + caja + '&idsolveh=' + caja2 + '&placa=' + caja3)
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
