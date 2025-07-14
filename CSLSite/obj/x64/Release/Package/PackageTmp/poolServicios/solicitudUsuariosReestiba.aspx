<%--<Page Title="" Language="C#" MasterPageFile="~/site.Master" AutoEventWireup="true"  CodeBehind="solicitudUsuariosReestiba.aspx.cs" Inherits="CSLSite.solicitudUsuariosReestiba" %>--%>
 <%@ Page Title="" Language="C#" MasterPageFile="~/site.Master" AutoEventWireup="true" 
    CodeBehind="solicitudUsuariosReestiba.aspx.cs" Inherits="CSLSite.solicitudUsuariosReestiba" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="placehead" runat="server">
   
   <link href="../shared/estilo/jquery.datetimepicker.css" rel="stylesheet" type="text/css" />
   <link href="../shared/estilo/tables.css" rel="stylesheet" type="text/css" />
    
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
 
    


             <div class="mt-4">         
        <nav class="mt-4" aria-label="breadcrumb">
          <ol class="breadcrumb">
            <li class="breadcrumb-item" id="opcion_principal" runat="server"><a href="#">Servicios</a></li>
             <li class="breadcrumb-item active" aria-current="page" id="sub_opcion" runat="server">Ingreso de Solicitud (Reestiba de Carga)</li>
          </ol>
        </nav>
                  <div class="alert alert-warning" id="alerta" runat="server" >Nota: Si la solicitud es generada posterior a las 13H00 la misma será planificada para el siguiente día, en la hora disponible informada por nuestra área de CFS.  </div>
      </div>

 <asp:UpdatePanel ID="upresult" runat="server">
    <ContentTemplate>
             <div class="dashboard-container p-4" id="cuerpo" runat="server">

         <div class="form-title">Criterios de consulta</div>
                 <div class="form-row" >
                                <div class="col-md-6">
                       <label for="inputAddress">Servicio<span style="color: #FF0000; font-weight: bold;"></span></label>
                               <asp:DropDownList ID="dptiposervicios" runat="server"  CssClass="form-control" >
                 <asp:ListItem Value="0">* Seleccione *</asp:ListItem>
          </asp:DropDownList>
                                
                                </div>

                                <div class="col-md-6">
  <label for="inputAddress">Tráfico<span style="color: #FF0000; font-weight: bold;"></span></label>
                                        <asp:DropDownList ID="dptipotrafico" runat="server"  CssClass="form-control"
          onchange="selectTrafico($('[id*=dptipotrafico]').val(),$('[id*=bookingContenedor]').val())" >
                 <asp:ListItem Value="0">* Seleccione *</asp:ListItem>
          </asp:DropDownList>
                                    
                                </div>


	 </div>
                 <div class="form-row">

 <div class="form-group col-md-6">
 <label for="inputAddress">Contenedor Lleno<span style="color: #FF0000; font-weight: bold;"></span></label>

   <div class="d-flex">
   <asp:TextBox ID="contenedor1" runat="server"   
             CssClass="form-control" Text="..."  Enabled="false"></asp:TextBox>

          <input id="contenedorNombreUno" runat="server" type="hidden" />
          <input id="contenedor1HF" runat="server" type="hidden" />


       <asp:TextBox ID="bookingContenedor" runat="server"  
                CssClass="form-control" Text="..."  Enabled="false"></asp:TextBox>
       
              <input id="bookingContenedorHF"  runat="server" type="hidden" />

                <a class="btn btn-outline-primary mr-4" target="popup" onclick="linkcontenedor1('<%=dptipotrafico.ClientID %>','FCL');" >
           <span class='fa fa-search' style='font-size:24px'></span>

            </a>
       <span id="valcont2" class="validacion">*</span>
   </div>
                         </div>
           
                 <div class="form-group col-md-6">
                      <label for="inputAddress">Contenedor Vacío<span style="color: #FF0000; font-weight: bold;"></span></label>
                     <div class="d-flex">
                           <asp:TextBox ID="contenedor2" runat="server"   CssClass="form-control"
                Text="..." Enabled="false"></asp:TextBox>
                       
                <input id="contenedor2HF" runat="server" type="hidden" />
                <input id="contenedorNombreDos" runat="server" type="hidden" />

                         <a class="btn btn-outline-primary mr-4" target="popup" onclick="linkcontenedor2('<%=dptipotrafico.ClientID %>','<%=contenedor1HF.ClientID %>','PTY');" >
                <span class='fa fa-search' style='font-size:24px'></span>
                    </a>
  <span id="valcont" class="validacion">*</span>
                     </div>

                     </div>
                     </div>
                 <div class="form-row">
                       <div class="form-group col-md-12">
                           	  <label for="inputAddress">Fecha/Hora propuesta para la operación<span style="color: #FF0000; font-weight: bold;"></span></label>
                             <div class="d-flex">
                                  <asp:TextBox ID="txtFechaPropuesta" runat="server"  MaxLength="10" 
                                      CssClass="datetimepicker form-control"
                onkeypress="return soloLetras(event,'01234567890/')" 
                onblur="valDate(this,false,valdate);" ClientIDMode="Static"></asp:TextBox>  
                    <span id="valdate" class="validacion">*</span>
                             </div>
                           </div>
                     </div>
                 <div class="form-row">
		  
		   <div class="form-group col-md-12"> 
               	  <label for="inputAddress">Tipo de producto cantidad y embalaje<span style="color: #FF0000; font-weight: bold;"></span></label>
               <div class="d-flex">
                       <asp:TextBox ID="txtTipoProductoEmbalaje" runat="server"  
                 
                 CssClass="form-control" placeholder="Ejem: 1014 Cajas de Banano/2 Motores 1TON" 
                     MaxLength="300" ></asp:TextBox>  
                   <span id="valtipopro" class="validacion">*</span>
               </div>
		   </div>
		  </div>
                 <div class="form-row">
		  
		   <div class="form-group col-md-12"> 
               	  <label for="inputAddress">Comentario<span style="color: #FF0000; font-weight: bold;"></span></label>
               <div class="d-flex">
                       <asp:TextBox ID="txtComentario" runat="server"  
                  CssClass="form-control"  MaxLength="30" ></asp:TextBox> 
                   <span id="Span3" class="validacion">*</span>
               </div>

		   </div>
		  </div>
                 <div class="form-row">
		  
		   <div class="form-group col-md-6"> 
		   	  <label for="inputAddress">Solicitud de Reestiba No.<span style="color: #FF0000; font-weight: bold;"></span></label>
		       <div class="d-flex">
                      <asp:TextBox ID="txtNumDocAduana" runat="server"  
                 CssClass="form-control" placeholder="123456782016RE000001P"
              onkeypress="return soloLetras(event,'abcdefghijklmnñopqrstuvwxyz1234567890')"
            MaxLength="30" ></asp:TextBox> 

		       </div>
           
           </div>
		  
		  
		   <div class="form-group col-md-6"> 
		   	  <label for="inputAddress">Documento de Aduana (PDF)<span style="color: #FF0000; font-weight: bold;"></span></label>
			  <div class="d-flex">
                   <input class=" form-control" id="archivoAduana" title="Escoja el archivo PDF" accept="pdf" type="file"  runat="server" clientidmode="Static" />        
                  <span id="valar" class="validacion">*</span>
			  </div>
		   </div>
		  </div>
                 <div class="form-row">
		  
		   <div class="col-md-12 d-flex justify-content-center"> 
                <span id="imagen" runat="server"></span>
            <input class="btn btn-primary" type="button" id="btgenerar" value="Generar solicitud" onclick="confirmGenerarSolicitud()"/>
            <asp:Button ID="btgenerarServer" runat="server" Text="Generar solicitud" OnClientClick="showGif('placebody_imagen')"
            OnClick="btgenerarServer_Click"  style="display:none;" />
               </div>
                                 </div>

                          
        
        <input id="idlin" type="hidden" runat="server" clientidmode="Static" />
        <input id="diponible" type="hidden" runat="server" clientidmode="Static" />  
                 
                    <div class="form-row">
		   <div class="col-md-12 d-flex justify-content-center"> 
		      <div id="sinresultado" runat="server" class="alert-info"></div>
		   </div> 
		   </div>
      
        
       
		 
     </div>


    </ContentTemplate>
    <Triggers>
        <asp:PostBackTrigger ControlID="btgenerarServer"/>
    </Triggers>
</asp:UpdatePanel>
 <script src="../Scripts/pages.js" type="text/javascript"></script>
      <script src="../Scripts/jquery.datetimepicker.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $('.datetimepicker').datetimepicker().datetimepicker({ lang: 'es', timepicker: true, closeOnDateSelect: true, format: 'd/m/Y H:i' });
        });

        function confirmGenerarSolicitud() {
            if (!ValidateFile('archivoAduana', valar)) {
                return false;
            }
              var r = alertify.confirm("Se generará una nueva solicitud, ¿Desea continuar?"
                , function () { $("#<%=btgenerarServer.ClientID%>").click(); alertify.success('Procesando') },
                function () {return;}
            );
        }
        function showGif(ctrl) {
            if (ctrl != null && ctrl != undefined && ctrl.trim().length > 0) {
                document.getElementById(ctrl).innerHTML = '<img alt="" src="../shared/imgs/loader.gif">'
            }
        }


        function popupCallback(objeto) {
            if (objeto == null || objeto == undefined) {
                alertify.alert('Hubo un problema al setear un objeto de catalogo');
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
<%--<asp:updateprogress  id="updateProgress" runat="server">
    <progresstemplate>
        <div id="progressBackgroundFilter"></div>
        <div id="processMessage"> Estamos procesando la tarea que solicitó, por favor  espere...<br />
         <img alt="Loading" src="../shared/imgs/loader.gif" style="margin:0 auto;" />
        </div>
    </progresstemplate>
</asp:updateprogress>--%>
</asp:Content>
