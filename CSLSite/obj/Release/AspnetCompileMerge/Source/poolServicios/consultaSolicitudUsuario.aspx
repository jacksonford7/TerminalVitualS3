<%@ Page Title="" Language="C#" MasterPageFile="~/site.Master" AutoEventWireup="true" 
         CodeBehind="consultaSolicitudUsuario.aspx.cs" Inherits="CSLSite.consultaSolicitudUsuario" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="placehead" runat="server">
    <link href="../shared/estilo/centroSolicitud.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/jquery.datetimepicker.css" rel="stylesheet" type="text/css" />
     
    <!--Datatables-->
       <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/5.15.1/css/all.min.css" integrity="sha512-+4zCK9k+qNFUR5X+cKL9EIR+ZOhtIloNl9GIKS57V1MyNsYpYcUrUeQc9vNfzsWfV28IaLL3i96P9sdNyeRssA==" crossorigin="anonymous" /><link href="../css/datatables.min.css" rel="stylesheet" /><link rel="stylesheet" type="text/css" href="..js/buttons/css1.6.4/buttons.dataTables.min.css" />
        <script src="../lib/jquery/jquery.min.js" type="text/javascript"></script>
       <script type="text/javascript"  src="../js/datatables.js"></script>
       <script src="../Scripts/table_catalog.js" type="text/javascript"></script>  
       <script type="text/javascript" src="../js/bootstrap.bundle.min.js"></script>
    <!--Fin-->


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
 <input id="zonaid" type="hidden" value="701" />
 <asp:ToolkitScriptManager ID="tkcal" EnableScriptGlobalization="true" runat="server"> </asp:ToolkitScriptManager>

            <div class="mt-4">         
        <nav class="mt-4" aria-label="breadcrumb">
          <ol class="breadcrumb">
            <li class="breadcrumb-item" id="opcion_principal" runat="server"><a href="#">Centro de Servicios</a></li>
             <li class="breadcrumb-item active" aria-current="page" id="sub_opcion" runat="server">Consulta de Solicitudes</li>
          </ol>
        </nav>
      </div>


        <div class="dashboard-container p-4" id="cuerpo" runat="server">

         <div class="form-title">Criterios de consulta</div>
		 
		  <div class="form-row">
		  
		   <div class="form-group col-md-6"> 
		   	  <label for="inputAddress">Num. Solicitud<span style="color: #FF0000; font-weight: bold;"></span></label>
			   <asp:TextBox ID="txtNumSolicitud" runat="server"  MaxLength="30"
             CssClass="form-control" ></asp:TextBox>
             <input id="usuariologin" runat="server" type="hidden" />
		   </div>
	
		  
		   <div class="form-group col-md-6"> 
		   	  <label for="inputAddress">Contenedor<span style="color: #FF0000; font-weight: bold;"></span></label>
			  <div class="d-flex">
                  <asp:TextBox ID="contenedor1" runat="server"  
              CssClass="form-control col-md-11" Text="..."  Enabled="true"></asp:TextBox>
             <input id="idContenedorU" runat="server" type="hidden" />
                  <a class="btn btn-outline-primary mr-4" target="popup" 
                      onclick="linkconsultacontenedor('<%=usuariologin.ClientID %>');" >
             <span class='fa fa-search' style='font-size:24px'></span> 

                  </a>
			  </div>
		   </div>
		  </div>

                 	  <div class="form-row">
		  
		   <div class="form-group col-md-6"> 
		   	  <label for="inputAddress">Tipo de Servicio<span style="color: #FF0000; font-weight: bold;"></span></label>
			  <asp:DropDownList ID="dptiposervicios" runat="server" CssClass="form-control" >
                     <asp:ListItem Value="0">* Seleccione servicios *</asp:ListItem>
              </asp:DropDownList>
		   </div>

                           	   <div class="form-group col-md-6"> 
		   	  <label for="inputAddress">Estado<span style="color: #FF0000; font-weight: bold;"></span></label>
			 <asp:DropDownList ID="dpestados" runat="server"  CssClass="form-control"  >
                     <asp:ListItem Value="0">* Seleccione estados *</asp:ListItem>
              </asp:DropDownList>
		   </div>
		  </div>

            	 <div class="form-row">
		  
		   
 <div class="form-group col-md-6"> 
     		   	  <label for="inputAddress">Desde<span style="color: #FF0000; font-weight: bold;"></span></label>
                              <asp:TextBox ID="desded" runat="server" MaxLength="10" CssClass="datetimepicker form-control"
             onkeypress="return soloLetras(event,'01234567890/')" 
                 onblur="valDate(this,false,valdate);" ClientIDMode="Static"></asp:TextBox>
	 </div>

                <div class="form-group col-md-6"> 
                    		   	  <label for="inputAddress">Hasta<span style="color: #FF0000; font-weight: bold;"></span></label>
	  <div class="d-flex">
    
         
            <asp:TextBox ID="hastad" runat="server"
                ClientIDMode="Static"  MaxLength="15" CssClass="datetimepicker form-control"
             onkeypress="return soloLetras(event,'01234567890/')" 
                  onblur="valDate(this,false,valdate);"></asp:TextBox>

             <span id="valdate" class="opcional"> * opcional</span>

			  </div>
	 </div>

</div>



                         <div class="form-row">
		  
		   <div class="col-md-12 d-flex justify-content-center">
              
            
            
                                
            <span id="imagen"></span>
            <asp:Button ID="btbuscar" runat="server" Text="Iniciar la búsqueda" 
                CssClass="btn btn-primary" 
            onclick="btbuscar_Click" OnClientClick="showGif('imagen')"/>
		   </div> </div>

		 
     </div>

     <div class="form-row">
	 <div class="form-group col-md-12"> 
                    
        <asp:UpdatePanel ID="upresult" runat="server" ChildrenAsTriggers="true">
            <ContentTemplate>
                <script type="text/javascript">
                    Sys.Application.add_load(BindFunctions); 
                </script>
        <input id="idlin" type="hidden" runat="server" clientidmode="Static" />
        <input id="diponible" type="hidden" runat="server" clientidmode="Static" />
        <div id="xfinder" runat="server" visible="false" >
        <div class=" alert alert-primary" id="alerta" runat="server" ></div>
     
       
                 <div class=" form-title">Contenedor(es):</div>
            
                 <asp:Repeater ID="tablePagination" runat="server"  >
                 <HeaderTemplate>
                 <table id="tablasort"  cellspacing="1" cellpadding="1" class="table table-bordered table-sm table-contecon" >
                 <thead>
                 <tr>                                
                 <th>No. Solicitud</th>  
                 <th>Tráfico</th>  
                 <th>Servicio</th>                                                 
                 <th>No. Booking</th>
                 <th>No. Carga</th>
                 <th>Fecha de Solicitud</th>
                 <th>Estado</th>
                 <th  class="nover">CodigoServicio</th>
                 <th></th>                 
                 </tr>
                 </thead> 
                 <tbody>
                 </HeaderTemplate>
                 <ItemTemplate>
                 <tr class="point" >                                    
                  <td><%#Eval("idSolicitud")%></td>    
                  <td><%#Eval("trafico")%></td>                  
                  <td><%#Eval("servicio")%></td>
                  <td><%#Eval("noBooking")%></td>
                  <td><%#Eval("noCarga")%></td>
                  <td><%#Eval("fechaSolicitud")%></td>
                  <td><%#Eval("estado")%></td>
                  <td  class="nover"><%#Eval("codigoServicio")%></td>
                  <td>
                      <div class="tcomand">
                           <a class=" btn btn-link" target="popup" onclick="linkconsultadetalle('<%#Eval("idSolicitud")%>', '<%#Eval("codigoServicio")%>');" >
                           <i class="ico-find" ></i> Detalles </a>
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
        <div id="sinresultado" runat="server" class=" alert alert-secondary"></div>
        </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btbuscar" />
            </Triggers>
        </asp:UpdatePanel>
       

	 </div>
	 
	 </div>



    <script src="../Scripts/centroServicios.js" type="text/javascript"></script>
 <script src="../Scripts/pages.js" type="text/javascript"></script>
      <script src="../Scripts/jquery.datetimepicker.js" type="text/javascript"></script>
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

            document.getElementById('<%=contenedor1.ClientID %>').value = objeto.codigo;
            document.getElementById('<%=idContenedorU.ClientID %>').value = objeto.item;
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
