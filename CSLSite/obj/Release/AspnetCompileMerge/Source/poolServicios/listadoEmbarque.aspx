<%@ Page Title="" Language="C#" MasterPageFile="~/site.Master" AutoEventWireup="true"
    CodeBehind="listadoEmbarque.aspx.cs" Inherits="CSLSite.listadoEmbarque" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="placehead" runat="server">
    <link href="../shared/estilo/tables.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/work.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/info.css" rel="stylesheet" type="text/css" />
   <link href="../shared/estilo/jquery.datetimepicker.css" rel="stylesheet" type="text/css" />
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
           
            width:100%;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="placebody" runat="server">
    <input id="zonaid" type="hidden" value="703" />
    <asp:ToolkitScriptManager ID="tkcal" EnableScriptGlobalization="true" runat="server"
        AsyncPostBackTimeout="1200">
    </asp:ToolkitScriptManager>
           <div class="mt-4">         
        <nav class="mt-4" aria-label="breadcrumb">
          <ol class="breadcrumb">
            <li class="breadcrumb-item" id="opcion_principal" runat="server"><a href="#">Servicios</a></li>
             <li class="breadcrumb-item active" aria-current="page" id="sub_opcion" runat="server">Listado de Embarque (LE)</li>
          </ol>
        </nav>
      </div>

    <asp:UpdatePanel ID="updSolicitud" runat="server">
        <ContentTemplate>
 <div class="dashboard-container p-4" id="cuerpo" runat="server">

           <div class="form-title">Datos del documento buscado</div>
		   <div class="form-row">
		  
		   <div class="form-group col-md-3"> 
		   	  <label for="inputAddress">Referencia<span style="color: #FF0000; font-weight: bold;"></span></label>
			  <div class="d-flex">
                      <asp:TextBox ID="txtReferencia" runat="server" CssClass="form-control" Enabled="false" ></asp:TextBox>
                                <asp:HiddenField ID="hdfReferencia" runat="server" />
                   <a class="btn btn-outline-primary mr-4" target="popup" onclick="linkReferencia();">
                       <span class='fa fa-search' style='font-size:24px'></span>
                                  </a>
                                <asp:Button ID="btnBuscar" runat="server" 
                                    Style="display: none;" OnClick="btnBuscar_Click"
                                    OnClientClick="showGif('placebody_imagenb')" />

			  </div>
		   </div>

                             	   <div class="form-group  col-md-5"> 
		   	  <label for="inputAddress">Nave<span style="color: #FF0000; font-weight: bold;"></span></label>
			  <div class="d-flex">
                   <asp:TextBox ID="txtNave" runat="server" CssClass="form-control" Enabled="false" ></asp:TextBox>
                                <asp:HiddenField ID="hdfNave" runat="server" />

			  </div>
		   </div>

               	   <div class="form-group  col-md-2"> 
		   	  <label for="inputAddress">Fecha límite<span style="color: #FF0000; font-weight: bold;"></span></label>
			  <div class="d-flex">
                     <asp:TextBox ID="txtCutOff" runat="server" CssClass="form-control" 
                                    Enabled="false" Style="text-align: center;"></asp:TextBox>
                                <asp:HiddenField ID="hdfCutOff" runat="server" />
                   <span id="imagenb" runat="server"></span>

			  </div>
		   </div>

              	   <div class="form-group  col-md-2"> 
		   	  <label for="inputAddress">Reporte<span style="color: #FF0000; font-weight: bold;"></span></label>
			  <div class="d-flex">
                   <asp:TextBox ID="txtReporte" runat="server" Enabled="false" CssClass="form-control" ></asp:TextBox>
                                <asp:HiddenField ID="hdfReporte" runat="server" />

			  </div>
		   </div>



		  </div>

          
       <div id="xfinder" runat="server" visible="false">
            <div class="form-title">Consulta de contenedor</div>
           <div class="form-row">
		  
		   <div class="form-group col-md-6"> 
		   	  <label for="inputAddress">Exportador<span style="color: #FF0000; font-weight: bold;"></span></label>
			  <div class="d-flex">
                         <asp:TextBox ID="txtExportador" runat="server" CssClass="form-control" ></asp:TextBox>
                                    <asp:HiddenField ID="hdfExportador" runat="server" />

			  </div>
		   </div>

                <div class="form-group col-md-2"> 
		   	  <label for="inputAddress">Booking<span style="color: #FF0000; font-weight: bold;"></span></label>
			  <div class="d-flex">
                  <asp:TextBox ID="txtBooking" runat="server" CssClass="form-control" ></asp:TextBox>
                                    <asp:HiddenField ID="hdfBooking" runat="server" />

			  </div>
		   </div>
               	  
		   <div class="form-group col-md-2"> 
		   	  <label for="inputAddress">Unidad<span style="color: #FF0000; font-weight: bold;"></span></label>
			  <div class="d-flex">
                   <asp:TextBox ID="txtContenedor" runat="server"  CssClass="form-control" ></asp:TextBox>
                                    <asp:HiddenField ID="hdfContenedor" runat="server" />

			  </div>
		   </div>

             <div class="form-group col-md-2"> 
		   	  <label for="inputAddress">Falta de Pago para embarque<span style="color: #FF0000; font-weight: bold;"></span></label>
			  <div class="d-flex">
                             <asp:DropDownList ID="ddlFaltaPago" runat="server" CssClass="form-control">
                                        <asp:ListItem Selected="True" Text="TODOS" Value="T"></asp:ListItem>
                                        <asp:ListItem Text="SI" Value="S"></asp:ListItem>
                                        <asp:ListItem Text="NO" Value="N"></asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:HiddenField ID="hdfFaltaPago" runat="server" />

			  </div>
		   </div>
		  </div>

           <div class="form-row">
		   <div class="col-md-12 d-flex justify-content-center"> 
               <div class="d-flex">
                       <asp:Button ID="btnBuscarContenedor" 
           runat="server" Text="Buscar" 
           OnClientClick="showGif('placebody_imagenBus')"
                                OnClick="btnBuscarContenedor_Click"
           CssClass="btn btn-primary" />
                                <asp:Button ID="btnBuscarHF" runat="server" Text="Buscar" onclick="btnBuscarHF_Click" style="display:none;"
                                 />
                            <span id="imagenBus" runat="server"></span>

               </div>
  
		   </div> 
        </div>
           <div class="form-title">Contenedores</div>
           <asp:GridView ID="grvContenedores" runat="server" CssClass="table table-bordered invoice" AutoGenerateColumns="False"
                            AllowPaging="True" OnPageIndexChanging="grvContenedores_PageIndexChanging" HeaderStyle-HorizontalAlign="Center"
                            HeaderStyle-VerticalAlign="Middle" 
                            onrowdatabound="grvContenedores_RowDataBound">
                            <Columns>
                                 <asp:TemplateField HeaderText="gkey" ItemStyle-Width="20%" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblBloqueoBool" runat="server" Text='<%#Eval("asumoCosto")%>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="25%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="gkey" ItemStyle-Width="20%" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblHabilitar" runat="server" Text='<%#Eval("habilitar")%>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="25%" />
                                </asp:TemplateField>
                                
                                <asp:TemplateField HeaderText="gkey" ItemStyle-Width="20%" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblGkey" runat="server" Text='<%#Eval("gkey")%>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="25%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Linea" ItemStyle-Width="20%" >
                                    <ItemTemplate>
                                        <asp:Label ID="lblLinea" runat="server" Text='<%#Eval("linea")%>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="25%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Exportador" ItemStyle-Width="20%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblTrafico" runat="server" Text='<%# Eval("exportador") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="25%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Unidad" ItemStyle-Width="20%">
                                    <ItemTemplate>
                                        <a class="xinfo"><span class="xclass" style="width: 250px;">
                                            <h3>
                                                Tipo</h3>
                                            <%#Eval("tipo")%>
                                            <br />
                                            <h3>
                                                DAE.:</h3>
                                            <%#Eval("dae")%>
                                            <br />
                                            <h3>
                                                Stuff:</h3>
                                            <%#Eval("stuff")%>
                                            <br />
                                            <h3>
                                                IN:</h3>
                                            <%#Eval("ingreso")%>
                                        </span>
                                            <%#Eval("unidad")%>
                                        </a>
                                    </ItemTemplate>
                                    <ItemStyle Width="25%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Booking" ItemStyle-Width="20%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblBooking" runat="server" Text='<%# Eval("booking") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="25%" />
                                </asp:TemplateField>
                                
                                <%--<asp:TemplateField HeaderText="D.A.E." ItemStyle-Width="20%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDAE" runat="server" Text='<%# Eval("dae") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="25%" />
                                </asp:TemplateField>--%>
                                <%-- <asp:TemplateField HeaderText="Exportador" ItemStyle-Width="20%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblExportador" runat="server" Text='<%# Eval("exportador") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="25%" />
                                </asp:TemplateField>--%>
                                <asp:TemplateField HeaderText="Cliente AISV" ItemStyle-Width="20%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCliente" runat="server" Text='<%# Eval("cliente_aisv") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="25%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Tipo de Cliente" ItemStyle-Width="20%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblTipoCliente" runat="server" Text='<%# Eval("tipoCliente") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="20%" />
                                </asp:TemplateField>
                                <%--<asp:TemplateField HeaderText="Tipo" ItemStyle-Width="20%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblTipo" runat="server" Text='<%# Eval("tipo") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="25%" >
                                </asp:TemplateField>--%>
                                <%-- <asp:TemplateField HeaderText="Stuff" ItemStyle-Width="20%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblStuff" runat="server" Text='<%# Eval("stuff") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="25%" />
                                </asp:TemplateField>--%>
                                <asp:TemplateField HeaderText="Tipo Roleo" ItemStyle-Width="20%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblTipoRoleo" runat="server" Text='<%# Eval("tipo_roleo") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="25%" />
                                </asp:TemplateField>
                                <%-- <asp:TemplateField HeaderText="Roleo" ItemStyle-Width="20%">
                                     <ItemTemplate>
                                        <asp:CheckBox ID="chkRow_rol" runat="server" Checked='<%#Convert.ToBoolean(Eval("roleo"))%>' Enabled="false" />
                                    </ItemTemplate>
                                    <ItemStyle Width="25%" />
                                </asp:TemplateField>--%>
                                <asp:TemplateField HeaderText="Tipo Bloqueo" ItemStyle-Width="20%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblTipoBloqueo" runat="server" Text='<%# Eval("tipo_bloqueo") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="25%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Falta de pago" ItemStyle-Width="25%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblBloqueo" runat="server" Text='<%# Convert.ToBoolean(Eval("bloqueo"))?"Si":"No" %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="25%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Garantiza Costos" ItemStyle-Width="25%">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkRow" runat="server" Visible='<%#Convert.ToBoolean(Eval("bloqueo"))%>'
                                            Checked='<%#Convert.ToBoolean(Eval("asumoCosto"))%>' onclick='<%# "confirmCheck(this,\"" +Eval("unidad") + "\");" %>'
                                            Enabled='<%#Convert.ToBoolean(Eval("habilitar"))%>' />
                                    </ItemTemplate>
                                    <ItemStyle Width="25%" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Tercero" ItemStyle-Width="20%">
                                    <ItemTemplate>
                                        <asp:Label ID="lbltercerorep" runat="server" Text='<%# Eval("terceros") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="25%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Estado" ItemStyle-Width="20%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblestadofac" runat="server" Text='<%# Eval("estado_fac") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="25%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Liquidación" ItemStyle-Width="20%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblliquidacion" runat="server" Text='<%# Eval("liquidacion") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="25%" />
                                </asp:TemplateField>
                            </Columns>
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                            <PagerSettings FirstPageImageUrl="~/shared/imgs/first.gif" LastPageImageUrl="~/shared/imgs/last.gif"
                                Mode="NextPreviousFirstLast" NextPageImageUrl="~/shared/imgs/next.gif" PreviousPageImageUrl="~/shared/imgs/prev.gif" />
                            <RowStyle CssClass="columnasGrid" />
                        </asp:GridView>
           <div class="form-row">
             
		   <div runat="server" id="divBotonera" class="col-md-12 d-flex justify-content-center" > 
                <div class="form-group col-md-6 justify-content-center">
                    <input clientidmode="Static"  class="btn btn-secondary"
                            id="dataexport" onclick="getTable('resultadoLista');"
                            type="button" value="Exportar" runat="server" />
		   </div>
      <div class="form-group col-md-6 justify-content-center"> 
          <div class="d-flex">
                   <asp:Button ID="btgenerar" 
                            runat="server" Text="Guardar" 
                            OnClientClick="showGif('placebody_imagen')"
                            CssClass="btn btn-primary"
                            OnClick="btgenerar_Click" />
                    <span id="imagen" runat="server"></span>

          </div>
		   </div>

		   </div> 
                  
        </div>
           <div class="cataresult">
                        <asp:UpdatePanel ID="upresult" runat="server" ChildrenAsTriggers="true">
                            <ContentTemplate>
                                <div class=" alert-modal" id="alerta" runat="server">
                                </div>
                                <input id="idlin" type="hidden" runat="server" clientidmode="Static" />
                                <input id="diponible" type="hidden" runat="server" clientidmode="Static" />
                                <div id="sinresultado" runat="server" class="  alert-primary">
                                </div>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btgenerar" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
           </div>
 </div>
        </ContentTemplate>
    </asp:UpdatePanel>
   
    
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
            if (objeto == null || objeto == undefined) {
                alertify.alert('Hubo un problema al setear un objeto de catalogo');
                return;
            }
            //si contenedor está lleno (FCL) o vacio (MTY)
            document.getElementById('<%=txtReferencia.ClientID %>').value = objeto.referencia;
            document.getElementById('<%=hdfReferencia.ClientID %>').value = objeto.referencia;
            document.getElementById('<%=txtCutOff.ClientID %>').value = objeto.cutoff;
            document.getElementById('<%=hdfCutOff.ClientID %>').value = objeto.cutoff;
            document.getElementById('<%=txtNave.ClientID %>').value = objeto.nave;
            document.getElementById('<%=hdfNave.ClientID %>').value = objeto.nave;
            document.getElementById('<%=txtReporte.ClientID %>').value = objeto.tipoReporte;
            document.getElementById('<%=hdfReporte.ClientID %>').value = objeto.tipoReporte;
            $("#<%=btnBuscar.ClientID%>").click(); //set value
            return;
        }

        function confirmCheck(ctrl, contenedor) {
            if (ctrl.checked == true) {
                alertify.confirm("¿Está seguro que desea asumir/garantizar los costos de embarque (recepción/porteo, roleos, inspección, aforo y/o late arrival) del contenedor " + contenedor + "?"
                    , function () { ctrl.checked = true;   $("#<%=btnBuscarHF.ClientID%>").click(); }, function () {                    ctrl.checked = false; }
                );
            }
            else {
                $("#<%=btnBuscarHF.ClientID%>").click();
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
