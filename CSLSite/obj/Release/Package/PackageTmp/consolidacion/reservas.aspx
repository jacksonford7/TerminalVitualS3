<%@ Page Title="Asignar turnos" Language="C#" MasterPageFile="~/site.Master" 
         AutoEventWireup="true" CodeBehind="reservas.aspx.cs" Inherits="CSLSite.reservas_consolidacion" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="placehead" runat="server">
    <link href="../shared/estilo/tables.css" rel="stylesheet" type="text/css" />

    <link href="../shared/estilo/work.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/catalogos.css" rel="stylesheet" type="text/css" />

           <link href="../shared/estilo/jquery.datetimepicker.css" rel="stylesheet" type="text/css" />
         <!--mensajes-->
        <link href="https://cdn.jsdelivr.net/npm/alertifyjs@1.11.0/build/css/alertify.min.css" rel="stylesheet"/>
        <script type="text/javascript" src="https://cdn.jsdelivr.net/npm/alertifyjs@1.11.0/build/alertify.min.js"></script>

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
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="placebody" runat="server">
<input id="Hidden1" type="hidden" value="402" />

    <input id="zonaid" type="hidden" value="2" />
    <asp:ToolkitScriptManager ID="tkcal" EnableScriptGlobalization="true" runat="server"> </asp:ToolkitScriptManager>
    
           <div class="mt-4">         
        <nav class="mt-4" aria-label="breadcrumb">
          <ol class="breadcrumb">
            <li class="breadcrumb-item" id="opcion_principal" runat="server"><a href="#">Exportación</a></li>
             <li class="breadcrumb-item active" aria-current="page" id="sub_opcion" runat="server">Reporte de Reservas</li>
          </ol>
        </nav>
      </div>
       <input id="agencia" type="hidden" runat="server"  clientidmode="Static"/>
     <div class="dashboard-container p-4" id="cuerpo" runat="server">

         <div class="form-title">Filtros para el reporte</div>
		 
		  <div class="form-row">
		  
		   <div class="form-group col-md-3"> 
		   	  <label for="inputAddress">Booking<span style="color: #FF0000; font-weight: bold;"></span></label>
			               <asp:TextBox
             CssClass="form-control"
             ID="tbooking" runat="server" ClientIDMode="Static"  MaxLength="15"
             onkeypress="return soloLetras(event,'abcdefghijklmnopqrstuvwxyz01234567890/')" >

			               </asp:TextBox>

		   </div>

       
		   <div class="form-group col-md-3"> 
		   	  <label for="inputAddress">Desde<span style="color: #FF0000; font-weight: bold;"></span></label>
			      <asp:TextBox 
             ID="tfechaini" runat="server" ClientIDMode="Static" 
                 MaxLength="15" CssClass="datetimepicker form-control"
             onkeypress="return soloLetras(event,'01234567890/')" 
             onblur="valDate(this,true,valfechaini);"></asp:TextBox>
		   </div>
		  
                <div class="form-group col-md-3"> 
		   	  <label for="inputAddress">Hasta<span style="color: #FF0000; font-weight: bold;"></span></label>
			 <asp:TextBox 

             ID="tfechafin" runat="server" ClientIDMode="Static"
                 MaxLength="15" CssClass="datetimepicker form-control"
             onkeypress="return soloLetras(event,'01234567890/')" 
             onblur="valDate(this,true,valfechafin);"></asp:TextBox>
		   </div>


                <div class="form-group col-md-3">
                     <label for="inputAddress">Ver detalle reserva<span style="color: #FF0000; font-weight: bold;"></span></label>
		             <asp:CheckBox Text="" ID="chkDetalle" runat="server"  CssClass="form-check"/></td>

		   </div>

		  </div>

            <div class="form-row">
		   <div class="col-md-12 d-flex justify-content-center"> 
		     <div class="d-flex">
                          
             <asp:Button ID="btbuscar" runat="server" Text="Consultar reservas"   
                  CssClass="btn btn-primary"
                 onclick="btbuscar_Click"
                  />

                  <span id="imagen"></span>
		     </div>
		   </div> 
		   </div>

            <div class="form-row">
		   <div class="col-md-12 "> 
		                 <asp:UpdatePanel ID="upresult" runat="server" ChildrenAsTriggers="true">
                     <ContentTemplate>
                       <input id="idlin" type="hidden" runat="server" clientidmode="Static" />
                       <input id="diponible" type="hidden" runat="server" clientidmode="Static" />

             <div id="xfinder" runat="server" visible="false" title="PRUEBA" >
             <div class=" alert-primary" id="alerta" runat="server" ></div>
        
             <div class="booking" >
                 <br />
                 <div>
                 <a href="#" target="_blank" runat="server" id="aprint" clientidmode="Static" class="btn btn-link" >Vista Preliminar / Imprimir</a>
                 </div>
                 <br />
                 <div class="form-title">Reservas</div>
                 <div class="bokindetalle">
                 <asp:Repeater ID="tablePagination" runat="server"  >
                 <HeaderTemplate>
                 <table id="tabla" cellspacing="1" cellpadding="1" class="table table-bordered invoice">
                 <thead>
                 <tr>
                 <th style="width:80px;">Ruc</th>
                 <th style="width:60px">Booking</th>
                 <th style="width:100px">Fecha</th>
                 <th style="width:50px">Reservado</th>
                 <th style="width:50px">Estado</th>
                 <th>Exportador</th>
                 <th style="width:30px">Desde</th>
                 <th style="width:30px">Hasta</th>
                 <th style="width:110px">Contenedores</th>
         
                 </tr>
                 </thead> 
                 <tbody>
                 </HeaderTemplate>
                 <ItemTemplate>
                 <tr class="point" >
                  <td><%#Eval("RUC")%></td>
                  <td><%#Eval("BOOKING")%></td>
                  <td><%#Eval("FECHA_PRG")%></td>
                  <td><%#Eval("CANTIDAD")%></td>
                  <td><%#Eval("ESTADO")%></td>
                  <td><%#Eval("EXPORTADOR")%></td>
                  <td><%#Eval("DESDE")%></td>
                  <td><%#Eval("HASTA")%></td>
                  <td align="center">
                  <div style=" overflow-y:scroll; max-height:50px; width:110px">
                  <%#Eval("CNTR")%>
                  </div>
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
               <div id="sinresultado" runat="server" class=" alert-secondary"></div>
                  </ContentTemplate>
                     <Triggers>
                         <asp:AsyncPostBackTrigger ControlID="btbuscar" />
                     </Triggers>
                 </asp:UpdatePanel>

		   </div> 
		   </div>
		 
     </div>
    
    
    
  

        

  
    <script src="../Scripts/pages.js" type="text/javascript"></script>
         <script src="../Scripts/jquery.datetimepicker.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.tablesorter.min.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.tablesorter.pager.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $('.datetimepicker').datetimepicker().datetimepicker({ lang: 'es', timepicker: false, closeOnDateSelect: true, format: 'd/m/Y' });
        });

        function popupCallback(objeto, catalogo) {
            if (objeto == null || objeto==undefined) {
                alertify.alert('Hubo un problema al setaar un objeto de catalogo');
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
            window.open('../catalogo/calendarioConsolidacion.aspx?bk=' + bo, 'name', 'width=850,height=880')
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
