<%@ Page Title="Asignar turnos" Language="C#" MasterPageFile="~/site.Master" AutoEventWireup="true" CodeBehind="turnos.aspx.cs" Inherits="CSLSite.turnos" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="placehead" runat="server">
    <link href="../shared/estilo/tables.css" rel="stylesheet" type="text/css" />

    <link href="../shared/estilo/catalogos.css" rel="stylesheet" type="text/css" />
  
    <link href="../shared/estilo/jquery.datetimepicker.css" rel="stylesheet" type="text/css" />
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

      
     <!--mensajes-->
        <link href="https://cdn.jsdelivr.net/npm/alertifyjs@1.11.0/build/css/alertify.min.css" rel="stylesheet"/>
        <script type="text/javascript" src="https://cdn.jsdelivr.net/npm/alertifyjs@1.11.0/build/alertify.min.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="placebody" runat="server">
    <input id="zonaid" type="hidden" value="1203" />
    <asp:ToolkitScriptManager ID="tkcal" EnableScriptGlobalization="true" runat="server"> </asp:ToolkitScriptManager>
    <div class="mt-4">         
        <nav class="mt-4" aria-label="breadcrumb">
          <ol class="breadcrumb">
            <li class="breadcrumb-item" id="opcion_principal" runat="server"><a href="#">Exportación</a></li>
             <li class="breadcrumb-item active" aria-current="page" id="sub_opcion" runat="server">Asignación de horarios (CFS)</li>
          </ol>
        </nav>
      </div>

      <div class="dashboard-container p-4" id="cuerpo" runat="server">
               <input id="agencia" type="hidden" runat="server"  clientidmode="Static"/>
         <div class="form-title">Datos para la programación</div>
		  <div class="form-row">
		  
		   <div class="form-group col-md-6"> 
		   	  <label for="inputAddress">Booking No.<span style="color: #FF0000; font-weight: bold;"></span></label>
			 
               <div class="d-flex">
               <span id="numbook" class=" form-control col-md-10" onclick="clear();">...</span>
                   <input id="nbrboo" type="hidden" value="" runat="server" clientidmode="Static"/>
                      <a  class="btn btn-outline-primary mr-4" target="popup" 
            onclick="window.open('../catalogo/bookinListConsolidacion.aspx','name','width=850,height=880')" >
          <span class='fa fa-search' style='font-size:24px'></span> 

        </a>

               </div>
		   </div>
		
		  
		   <div class="form-group col-md-6"> 
		   	  <label for="inputAddress">Fecha de programación<span style="color: #FF0000; font-weight: bold;"></span></label>
			  <div class="d-flex">
                                     <span id="txtfecha" class=" form-control col-md-10"  onclick="clear2();">...</span>
                   <input id="xfecha" type="hidden" value="" runat="server" clientidmode="Static" />
                                 <a  class="btn btn-outline-primary mr-4" target="popup" onclick="openPop();" >
             <span class='fa fa-search' style='font-size:24px'></span> 

                 </a>

			  </div>
		   </div>
		  </div>
          <div class="form-row">
		  
		   <div class="form-group col-md-12"> 
		   	  <label for="inputAddress">Mail informativo<span style="color: #FF0000; font-weight: bold;"></span></label>
			  <div class="d-flex">
                    <input 
               placeholder="Para enviar a varios mails separelos con (;)"
               type='text' id='tmail' name='tmail'  runat="server" class="date form-control"
               enableviewstate="false" clientidmode="Static"
               onkeypress="return soloLetras(event,'abcdefghijklmnopqrstuvwxyz1234567890_@.;',true)"  
               onblur="maildatavarios(this,'valmailz');" maxlength="250"
               />
                           <span id="valmailz" class="validacion" > * </span>

			  </div>
		   </div>
		  </div>
          <div class="form-row">
		   <div class="col-md-12 d-flex justify-content-center"> 
               <div class="d-flex">
                            <asp:Button ID="btbuscar"  CssClass="btn btn-primary"
                 runat="server" Text="Consultar disponibilidad"   
                 onclick="btbuscar_Click"  OnClientClick="return checkDate('xfecha');"
                  />
                         <span id="imagen"></span></div>

		   </div> 
		   </div>

           <div class="cataresult" >
               <asp:UpdatePanel ID="upresult" runat="server" ChildrenAsTriggers="true">
                     <ContentTemplate>
                       <input id="idlin" type="hidden" runat="server" clientidmode="Static" />
                       <input id="diponible" type="hidden" runat="server" clientidmode="Static" />

                  <div id="xfinder" runat="server" visible="false" >
                 <div class=" alert-modal" id="alerta" runat="server" ></div>
             <div class="findresult" >
             <div class="booking" >
                  <div class="form-title">Turnos disponibles</div>
                 
                 <div class="bokindetalle">
                 <asp:Repeater ID="tablePagination" runat="server"  >
                 <HeaderTemplate>
                 <table id="tabla"  cellspacing="1" cellpadding="1" class="table table-bordered invoice">
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
                 
                      Text="0" ID="caja" CssClass=" form-control" xval='<%#Eval("DISPONIBLE")%>' onblur="cajaControl(this);" runat="server" MaxLength="2" onkeypress="return soloLetras(event,'01234567890',true)" ></asp:TextBox>
                   </td>
                 </tr>
                  </ItemTemplate>
                 <FooterTemplate>
                 </tbody>
                 </table>
                 </FooterTemplate>
         </asp:Repeater>
                     <div class="d-flex flex-row-reverse border border-primary" >
                          <div id="ttd"   >Total reservado: 0</div>
                     </div>

       
         
       
                </div>

                 	   <div class="form-row">
		   <div class="col-md-12 d-flex justify-content-center" runat="server" id="btnera"> 
		     <div class="d-flex">
                 <img alt="loading.." src="../shared/imgs/loader.gif" id="loader" class="nover"  />
              <input id="btsalvar"  class="btn btn-primary"
                  type="button" value="Proceder y Asignar"  onclick="prepareObject();" /> 

		     </div>
		   </div> 
		   </div>

      
             </div>
             </div>
              </div>
               <div id="sinresultado" runat="server" class=" alert-secondary"></div>
                  </ContentTemplate>
                     <Triggers>
                         <asp:AsyncPostBackTrigger ControlID="btbuscar" />
                     </Triggers>
                 </asp:UpdatePanel>
             </div>
      
        </div>

    <script src="../Scripts/pages.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.tablesorter.min.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.tablesorter.pager.js" type="text/javascript"></script>
     <script src="../Scripts/jquery.datetimepicker.js" type="text/javascript"></script>
    <script type="text/javascript">

        function popupCallback(objeto, catalogo) {
            if (objeto == null || objeto==undefined) {
                 alertify.alert('Hubo un problema al setear un objeto de catalogo');
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
                     alertify.alert('* Datos de programación *\n *Escriba el numero de Booking*');
                    document.getElementById("loader").className = 'nover';
                    return;
                }
                vals = document.getElementById('xfecha');
                if (vals == null || vals == undefined || vals.value.trim().length <= 5) {
                     alertify.alert('* Datos de programación *\n Escriba la fecha de programación');
                    document.getElementById("loader").className = 'nover';
                    return;
                }
                vals = document.getElementById('tmail');
                if (vals == null || vals == undefined || vals.value.trim().length <= 5) {
                     alertify.alert('* Datos de programación *\n *Escriba el correo electrónico para la notificación');
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
                             alertify.alert('El Horario ' + lista[n].desde + '-' + lista[n].hasta + ' excede su disponibilidad, favor verifique');
                            return;
                        }
                        total += parseInt(lista[n].reserva);
                    }
                }
                if (total > qtlimite) {
                     alertify.alert('* Reserva *\n La cantidad de reserva excede el cupo disponible \n Cupo: ' + qtlimite + '\n Reserva: ' + total);
                    return;
                }
                if (total <= 0) {
                     alertify.alert('* Reserva *\n La cantidad de reservas debe ser mayor que 0');
                    return;
                }
                tansporteServer(this.programacion, 'turnos.aspx/ValidateJSON');
               
            } catch (e) {
                 alertify.alert('Lamentamos comunicarle que ha ocurrido el siguiente problema:\n' + e.Message);
            }
        }

        function openPop() {
            var bo = document.getElementById('nbrboo').value;
            if (bo == undefined || bo == '' || bo == null) {
            alertify.alert('Por favor seleccione el booking primero');
                return;
            }
            window.open('../catalogo/Calendario.aspx?bk='+bo, 'name', 'width=850,height=880')
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
