<%@ Page Title="" Language="C#" MasterPageFile="~/site.Master" AutoEventWireup="true"
    CodeBehind="reporteLateArrival.aspx.cs" Inherits="CSLSite.reporteLateArrival" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="placehead" runat="server">
    <link href="../shared/estilo/tables.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/aisv-general.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/work.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/catalogos.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/info.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/jquery.datetimepicker.css" rel="stylesheet" type="text/css" />
      <!--mensajes-->
        <link href="https://cdn.jsdelivr.net/npm/alertifyjs@1.11.0/build/css/alertify.min.css" rel="stylesheet"/>
        <script type="text/javascript" src="https://cdn.jsdelivr.net/npm/alertifyjs@1.11.0/build/alertify.min.js"></script>

    <script src="../Scripts/centroServicios.js" type="text/javascript"></script>
    <link href="../shared/estilo/centroSolicitud.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .warning
        {
            background-color: Yellow;
            color: Red;
        }
        
        #progressBackgroundFilter
        {
            position: fixed;
            bottom: 0px;
            right: 0px;
            overflow: hidden;
            z-index: 1000;
            top: 0;
            left: 0;
            background-color: #CCC;
            opacity: 0.8;
            filter: alpha(opacity=80);
            text-align: center;
        }
        #processMessage
        {
            text-align: center;
            position: fixed;
            top: 30%;
            left: 43%;
            z-index: 1001;
            border: 5px solid #67CFF5;
            width: 200px;
            height: 100px;
            background-color: White;
            padding: 0;
        }
        #aprint
        {
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
            background-position: left center;
            text-decoration: none;
            padding: 5px 2px 5px 30px;
        }
        .xcontroles
        {
            width: 705px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="placebody" runat="server">
 
    <asp:ToolkitScriptManager ID="tkcal" EnableScriptGlobalization="true" runat="server"
        AsyncPostBackTimeout="1200">
    </asp:ToolkitScriptManager>

          <div class="mt-4">         
        <nav class="mt-4" aria-label="breadcrumb">
          <ol class="breadcrumb">
            <li class="breadcrumb-item" id="opcion_principal" runat="server"><a href="#">Servicios</a></li>
             <li class="breadcrumb-item active" aria-current="page" id="sub_opcion" runat="server">Reporte de Late Arrival</li>
          </ol>
        </nav>
      </div>

         <div class="dashboard-container p-4" id="cuerpo" runat="server">
    <asp:UpdatePanel ID="updSolicitud" runat="server">
        <ContentTemplate>
           <div class="form-title">Datos del documento buscado</div>
             <div class="form-row">
		  
		   <div class="form-group col-md-4"> 
		   	  <label for="inputAddress">Referencia<span style="color: #FF0000; font-weight: bold;"></span></label>
			  <div class="d-flex">
                                <asp:TextBox ID="txtReferencia" runat="server"  CssClass="form-control" onkeypress="return soloLetras(event,'abcdefghijklmnñopqrstuvwxyz1234567890',true)"></asp:TextBox>
                                <asp:HiddenField ID="hdfReferencia" runat="server" />


			  </div>
		   </div>
		 
		  
		   <div class="form-group col-md-4"> 
		   	  <label for="inputAddress">Estado<span style="color: #FF0000; font-weight: bold;"></span></label>
			  <div class="d-flex">
                          <asp:DropDownList ID="ddlTransito" runat="server"  CssClass=" form-control">
                                    <asp:ListItem Selected="True" Text="TODOS" Value="T"></asp:ListItem>
                                    <asp:ListItem Text="INGRESADOS" Value="Y"></asp:ListItem>
                                    <asp:ListItem Text="NO INGRESADOS" Value="I"></asp:ListItem>
                                </asp:DropDownList>
                                <asp:HiddenField ID="hdfTransito" runat="server" />
                   <span id="imagenb" runat="server"></span>

			  </div>
		   </div>

                 		   <div class="col-md-4 "> 
		     <div class="d-flex">
                       <asp:Button ID="btnBuscarContenedores"  CssClass="btn btn-primary"
                            runat="server" Text="Buscar" 
                            OnClientClick="showGif('placebody_imagenBusCont')" onclick="btnBuscarContenedores_Click"
                             />
                        
                        <span id="imagenBusCont" runat="server"></span>

		     </div>
		   </div> 
		  </div>

              <div class="form-row">

		   </div>

              <div class="form-row">
		   <div class="col-md-12 d-flex justify-content-center"> 
		                  <div id="xfinder" runat="server" visible="false">
                        
                        <asp:GridView ID="grvContenedores" runat="server" 
                            CssClass="table table-bordered invoice" 
                            AutoGenerateColumns="False"
                            AllowPaging="True" OnPageIndexChanging="grvContenedores_PageIndexChanging" HeaderStyle-HorizontalAlign="Center"
                            HeaderStyle-VerticalAlign="Middle" OnRowDataBound="grvContenedores_RowDataBound">
                            <Columns>
                                
                                <asp:TemplateField HeaderText="gkey" ItemStyle-Width="20%" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblGkey" runat="server" Text='<%#Eval("gkey")%>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="25%" />
                                </asp:TemplateField>
                                
                                <asp:TemplateField HeaderText="Unidad" ItemStyle-Width="20%">
                                    <ItemTemplate>
                                        <a class="xinfo">
                                            <%#Eval("contenedor")%>
                                        </a>
                                    </ItemTemplate>
                                    <ItemStyle Width="25%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Línea" ItemStyle-Width="20%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblLinea" runat="server" Text='<%# Eval("linea") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="25%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Booking" ItemStyle-Width="20%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblBooking" runat="server" Text='<%# Eval("booking") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="25%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="DAE" ItemStyle-Width="20%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDAE" runat="server" Text='<%# Eval("dae") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="25%" />
                                </asp:TemplateField>
                                 <asp:TemplateField HeaderText="Exportador" ItemStyle-Width="20%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblExportador" runat="server" Text='<%# Eval("exportador") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="25%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Cliente" ItemStyle-Width="20%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCliente" runat="server" Text='<%# Eval("cliente") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="25%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Fecha de Ingreso" ItemStyle-Width="20%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblIngreso" runat="server" Text='<%# Eval("ingreso") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="20%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Fecha de Cutoff" ItemStyle-Width="20%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCutoff" runat="server" Text='<%# Eval("cutoff") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="20%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Fecha de Cutoff Máximo" ItemStyle-Width="20%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCutoffMaximo" runat="server" Text='<%# Eval("cutoffMaximo") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="20%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Estado Late Arrival" ItemStyle-Width="20%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblLate" runat="server" Text='<%# Eval("lateArrival") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="20%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Estado Contenedor" ItemStyle-Width="20%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblEstado" runat="server" Text='<%# Eval("estado") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="20%" />
                                </asp:TemplateField>
                            </Columns>
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                            <PagerSettings FirstPageImageUrl="~/shared/imgs/first.gif" LastPageImageUrl="~/shared/imgs/last.gif"
                                Mode="NextPreviousFirstLast" NextPageImageUrl="~/shared/imgs/next.gif" PreviousPageImageUrl="~/shared/imgs/prev.gif" />
                            <RowStyle CssClass="columnasGrid" />
                        </asp:GridView>
                    </div>
		   </div> 
		   </div>

              <div class="form-row">
		   <div class="col-md-12 d-flex justify-content-center" runat="server" id="divBotonera"> 
		     <div class="d-flex">
                      <input clientidmode="Static" id="dataexport" 
                            onclick="getTable('resultadoListaLate');" class="btn btn-primary"
                            type="button" value="Exportar" runat="server" />
                        <span id="imagen" runat="server"></span>

		     </div>
		   </div> 
		   </div>

              <div class="form-row">
		   <div class="col-md-12 d-flex justify-content-center"> 
		     <div class="d-flex">
                         <asp:UpdatePanel ID="upresult" runat="server" ChildrenAsTriggers="true">
                            <ContentTemplate>
                                <div class=" alert-primary" id="alerta" runat="server">
                                </div>
                                <input id="idlin" type="hidden" runat="server" clientidmode="Static" />
                                <input id="diponible" type="hidden" runat="server" clientidmode="Static" />
                                <div id="sinresultado" runat="server" class=" alert-secondary">
                                </div>
                            </ContentTemplate>
                            
                        </asp:UpdatePanel>

		     </div>
		   </div> 
		   </div>

        </ContentTemplate>
    </asp:UpdatePanel>
   
   </div> 
    
    
    
    <script src="../Scripts/pages.js" type="text/javascript"></script>
    <script src="../Scripts/pages_mod.js" type="text/javascript"></script>
      <script src="../Scripts/jquery.datetimepicker.js" type="text/javascript"></script>
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

        function popupCallback(objeto) {
            

        }
        function confirmCheck(ctrl, contenedor) {

            

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
              alertify.alert('Por favor seleccione el booking primero.');
                return;
            }
            window.open('../catalogo/calendarioConsolidacion.aspx?bk=' + bo, 'name', 'width=850,height=880')
        }

    </script>
<%--    <asp:updateprogress  id="updateProgress" runat="server">
    <progresstemplate>
        <div id="progressBackgroundFilter"></div>
        <div id="processMessage"> Estamos procesando la tarea que solicitó, por favor  espere...<br />
         <img alt="Loading" src="../shared/imgs/loader.gif" style="margin:0 auto;" />
        </div>
    </progresstemplate>
</asp:updateprogress>--%>
</asp:Content>
