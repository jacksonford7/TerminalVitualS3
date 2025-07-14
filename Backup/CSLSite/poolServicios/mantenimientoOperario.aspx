<%@ Page Title="" Language="C#" MasterPageFile="~/site.Master" AutoEventWireup="true" EnableEventValidation="false"
         CodeBehind="mantenimientoOperario.aspx.cs" Inherits="CSLSite.mantenimientoOperario" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="placehead" runat="server">
    <link href="../shared/estilo/tables.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/aisv-general.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/work.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/catalogos.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/info.css" rel="stylesheet" type="text/css" />
    <%--<script src="../Scripts/turnos.js" type="text/javascript"></script>--%>
    <link href="../shared/estilo/centroSolicitud.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/centroServicios.js" type="text/javascript"></script>
    <script type="text/javascript">
        function BindFunctions() {
            $(document).ready(function () {
                document.getElementById('imagen').innerHTML = '';
                $('#tablasort').tablesorter({ cancelSelection: true, cssAsc: "ascendente", cssDesc: "descendente", headers: { 8: { sorter: false }, 9: { sorter: false}} }).tablesorterPager({ container: $('#pager'), positionFixed: false, pagesize: 10 });
            });
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
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="placebody" runat="server">
    <input id="zonaid" type="hidden" value="705" />
 <asp:ToolkitScriptManager ID="tkcal" EnableScriptGlobalization="true" runat="server"> </asp:ToolkitScriptManager>
 <div>
 <i class="ico-titulo-1"></i><h2>Administración de Servicios</h2>  <br />
 <i class="ico-titulo-2"></i><h1>Administración de Operarios</h1><br />
 </div>
 <div class="seccion">
       <div class="accion">
       <table class="xcontroles" cellspacing="0" cellpadding="1">
       <tr><th class="bt-bottom bt-right bt-left bt-top" colspan="4"> Criterios de consulta:</th></tr>
       <tr>
        <td class="bt-bottom bt-right bt-left" >Contenedor:</td>
        <td class="bt-bottom bt-right">
             <asp:TextBox ID="contenedor1" runat="server"  
             style="text-align:center;" placeholder="..." Width="90%"></asp:TextBox>
             <input id="idContenedorU" runat="server" type="hidden" />
        </td>
        <td class="bt-bottom bt-right finder">
            <a class="topopup" target="popup" onclick="linkconsultacontenedorOperario();" >
            <i class="ico-find" ></i> Buscar </a>
         </td>
       </tr>       
       <tr>
          <td class="bt-bottom bt-right bt-left">Tipo de Servicio:</td>
          <td class="bt-bottom bt-right">
          <%--<a class="tooltip" >
          <span class="classic">Esta informacion contiene los tipos de servicios disponibles.</span>--%>
          <asp:DropDownList ID="dptiposervicios" runat="server" Width="200px" >
                 <asp:ListItem Value="0">* Seleccione servicios *</asp:ListItem>
          </asp:DropDownList>
          <%--</a>--%>
        </td>
        <td class="bt-bottom bt-right validacion"></td>
       </tr>              
       <tr>
         <td class="bt-bottom bt-right bt-left">Generados desde / hasta:</td>
         <td class="bt-bottom bt-right">
            <%--<a class="tooltip" >
            <span class="classic">Fecha desde.</span>--%>
            <asp:TextBox ID="desded" runat="server" Width="120px" MaxLength="10" CssClass="datetimepicker"
             onkeypress="return soloLetras(event,'01234567890/')" 
                 onblur="valDate(this,false,valdate);" ClientIDMode="Static"></asp:TextBox>
            <%--</a>
            <a class="tooltip" >
            <span class="classic">Fecha hasta.</span>--%>
            <asp:TextBox ID="hastad" runat="server" ClientIDMode="Static" Width="120px" MaxLength="15" CssClass="datetimepicker"
             onkeypress="return soloLetras(event,'01234567890/')" 
                  onblur="valDate(this,false,valdate);"></asp:TextBox>
            <%--</a>--%>
          </td>
          <td class="bt-bottom bt-right validacion "><span id="valdate" class="opcional"> * opcional</span></td>
        </tr>
       <tr><td class="bt-bottom  bt-right bt-left">Estado:</td>
       <td class="bt-bottom bt-right">
          <%--<a class="tooltip" >
          <span class="classic">Esta informacion contiene los estados disponibles.</span>--%>
          <asp:DropDownList ID="dpestados" runat="server" Width="200px"  >
                 <asp:ListItem Value="0">* Seleccione estados *</asp:ListItem>
          </asp:DropDownList>
          <%--</a>--%>
       </td>
       <td class="bt-bottom bt-right validacion "></td>
       </tr>
       <tr>
         <td class="bt-bottom bt-right bt-left">Todas las solicitudes.</td>
         <td class="bt-bottom bt-right">
             <asp:CheckBox Text="" ID="chkTodos" runat="server" />
         </td>
         <td class="bt-bottom bt-right validacion "></td>
       </tr>
       </table>
       <div class="botonera">
          <span id="imagen"></span>
             <asp:Button ID="btbuscar" runat="server" Text="Iniciar la búsqueda" 
               onclick="btbuscar_Click" OnClientClick="showGif('imagen')" />           
       </div>
       <div class="cataresult" >
        <asp:UpdatePanel ID="upresult" runat="server" ChildrenAsTriggers="true">
            <ContentTemplate>
                <script type="text/javascript">
                    Sys.Application.add_load(BindFunctions); 
                </script>
                <input id="idlin" type="hidden" runat="server" clientidmode="Static" />
                <input id="diponible" type="hidden" runat="server" clientidmode="Static" />
       <%-- <div id="xfinder" runat="server" visible="false" >
        <div class="msg-alerta" id="alerta" runat="server" ></div>
        <div class="findresult" >
        <div class="booking" >
                 <div class="separator">Contenedor(es):</div>
                 <div class="bokindetalle">
                 <asp:Repeater ID="tablePagination" runat="server"  >
                 <HeaderTemplate>
                 <table id="tabla"  cellspacing="1" cellpadding="1" class="tabRepeat">
                 <thead>
                 <tr>               
                 <th style="display:none"></th>                   
                 <th>N°.</th>
                 <th>N°. Solicitud</th>
                 <th>Contenedor</th>
                 <th>Tipo Contenedor</th>
                 <th>Servicio</th>                                  
                 <th>Observación</th>
                 <th>Confirmar</th>
                 </tr>
                 </thead> 
                 <tbody>
                 </HeaderTemplate>
                 <ItemTemplate>
                 <tr class="point" >                                    
                  <td style="display:none"><asp:Label id='lblIdDetalleS' runat="server" Visible="true" Text='<%#Eval("idDetalleSolicitud")%>'/></td>
                  <td><%#Eval("noFila")%></td>
                  <td><%#Eval("numSolicitud")%></td>
                  <td><%#Eval("contenedor")%></td>
                  <td><%#Eval("tipoContenedor")%></td>
                  <td><%#Eval("servicio")%></td>                 
                  <td><asp:TextBox TextMode="MultiLine" id='txtArea' runat="server" Text='<%#Eval("observacion")%>' Rows="1" Columns="20" Height="30px" Width="200px"/> </td>
                  <td><asp:DropDownList ID="confirmacionddl"  runat="server" SelectedValue='<%#Eval("confirmacion")%>' onChange="return setRequired(this);">
                            <asp:ListItem Text="Pendiente" Value="PE"></asp:ListItem>
                            <asp:ListItem Text="Si" Value="SI"></asp:ListItem>
                            <asp:ListItem Text="No" Value="NO"></asp:ListItem>
                      </asp:DropDownList>
                  </td>                  
                 </tr>
                 </ItemTemplate>
                 <FooterTemplate>
                 </tbody>
                 </table>                
                 </FooterTemplate>
         </asp:Repeater>
                </div>
        <div class="botonera">
            <span id="imagenConfirmacion" runat="server"></span>            
                <asp:Button ID="btconfirmarServicio" runat="server" Text="Confirmar Servicio" 
                onclick="btconfirmar_Click" OnClientClick="showGif('placebody_imagenConfirmacion')"/>                            
        </div>
        </div>
            <div id="pager">
               Registros por página
                  <select class="pagesize">
                  <option selected="selected" value="10">10</option>
                  <option value="20">20</option>
                  </select>
                 <img alt="" src="../shared/imgs/first.gif" class="first"/>
                 <img alt="" src="../shared/imgs/prev.gif" class="prev"/>
                 <input  type="text" class="pagedisplay" size="5px"/>
                 <img alt="" src="../shared/imgs/next.gif" class="next"/>
                 <img alt="" src="../shared/imgs/last.gif" class="last"/>                 
            </div>
        </div>        
        </div>--%>
        <div id="xfinder" runat="server" visible="false">
            <div class="msg-alerta" id="alerta" runat="server" ></div>
            <div class="separator">Contenedor(es):</div>
            <div style="overflow-x:scroll; height:100%; width:100%" >
                <asp:GridView ID="grvContenedores" runat="server" 
                CssClass="mGrid" AutoGenerateColumns="false" 
                AllowPaging="True" OnRowDataBound="grvContenedores_RowDataBound" onpageindexchanging="grvContenedores_PageIndexChanging">
                <Columns>
                    <asp:TemplateField HeaderText="idDetalleSolicitud" Visible="false">
                        <ItemTemplate>
                            <asp:Label id='lblIdDetalleS' runat="server" Visible="true" Text='<%#Eval("idDetalleSolicitud")%>'/>
                        </ItemTemplate>                        
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="N°."  Visible="false
                    " ItemStyle-Width="3%">
                        <ItemTemplate>
                            <asp:Label ID="lblNoFila" runat="server" Text='<%#Eval("noFila")%>'></asp:Label>
                        </ItemTemplate>                        
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Solicitud" ItemStyle-Width="10%">
                        <ItemTemplate>
                            <asp:Label ID="lblNoSolicitud" runat="server" Text='<%# Eval("numSolicitud") %>'></asp:Label>
                        </ItemTemplate>                        
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Contenedor" ItemStyle-Width="15%">
                        <ItemTemplate>
                            <asp:Label ID="lblContenedor" runat="server" Text='<%# Eval("contenedor") %>'></asp:Label>
                        </ItemTemplate>                        
                    </asp:TemplateField>
                    <%--<asp:TemplateField HeaderText="Tráfico" ItemStyle-Width="10%">
                        <ItemTemplate>
                            <asp:Label ID="lblTipoContenedor" runat="server" Text='<%# Eval("tipoContenedor") %>'></asp:Label>
                        </ItemTemplate>                        
                    </asp:TemplateField>--%>
                    <asp:TemplateField HeaderText="Servicio" ItemStyle-Width="15%">
                        <ItemTemplate>
                            <asp:Label ID="lblServicio" runat="server" Text='<%# Eval("servicio") %>'></asp:Label>
                        </ItemTemplate>                        
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Peso">
                        <ItemTemplate>
                            <asp:Label ID="lblPeso" runat="server" Text='<%# Eval("peso") %>'></asp:Label>
                        </ItemTemplate>                        
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Observación" ItemStyle-Width="12%">
                        <ItemTemplate>
                            <asp:TextBox id='txtArea' MaxLength="150" runat="server" Text='<%#Eval("observacion")%>' Rows="1" Columns="20" Height="30px" Width="130px"/>
                        </ItemTemplate>                        
                    </asp:TemplateField>                    
                    <asp:TemplateField HeaderText="Procesado" ItemStyle-Width="8%">
                        <ItemTemplate>
                            <asp:DropDownList ID="confirmacionddl"  runat="server" SelectedValue='<%#Eval("confirmacion")%>' onChange="return setRequired(this);">
                                <asp:ListItem Text="Pend" Value="PE"></asp:ListItem>
                                <asp:ListItem Text="Si" Value="SI"></asp:ListItem>
                                <asp:ListItem Text="No" Value="NO"></asp:ListItem>
                            </asp:DropDownList>
                        </ItemTemplate>                        
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Ecuapass" ItemStyle-Width="8%">
                        <ItemTemplate>
                            <asp:DropDownList ID="tipoDDL" runat="server" SelectedValue='<%#Eval("tipoVerificacion")%>'>
                                <asp:ListItem Text="Selec." Value="SELE"></asp:ListItem>
                                <asp:ListItem Text="CIIS" Value="CIIS"></asp:ListItem>
                                <asp:ListItem Text="IMPDT" Value="IMPDT"></asp:ListItem>
                            </asp:DropDownList>
                        </ItemTemplate>                        
                    </asp:TemplateField>
                </Columns>                
                <PagerSettings FirstPageImageUrl="~/shared/imgs/first.gif" 
                    LastPageImageUrl="~/shared/imgs/last.gif" Mode="NextPreviousFirstLast" 
                    NextPageImageUrl="~/shared/imgs/next.gif" 
                    PreviousPageImageUrl="~/shared/imgs/prev.gif" />
                <RowStyle CssClass="columnasGrid" />
            </asp:GridView>
            </div>
            <div class="botonera">
                    <span id="imagenConfirmacion" runat="server"></span>            
                    <asp:Button ID="btconfirmarServicio" runat="server" Text="Confirmar Servicio" 
                    onclick="btconfirmar_Click" OnClientClick="showGif('placebody_imagenConfirmacion')"/>                            
            </div>                  
        </div>
        <!--<asp:TemplateField HeaderText="Sello #1" ItemStyle-Width="10%">
                        <ItemTemplate>
                            <asp:Label ID="lblSelloUno" runat="server" Text='<%# Eval("sello1") %>'></asp:Label>
                        </ItemTemplate>                        
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Sello #2" ItemStyle-Width="10%">
                        <ItemTemplate>
                            <asp:Label ID="lblSelloDos" runat="server" Text='<%# Eval("sello2") %>'></asp:Label>
                        </ItemTemplate>                        
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Sello #3" ItemStyle-Width="10%">
                        <ItemTemplate>
                            <asp:Label ID="lblSelloTres" runat="server" Text='<%# Eval("sello3") %>'></asp:Label>
                        </ItemTemplate>                        
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Sello #4" ItemStyle-Width="10%">
                        <ItemTemplate>
                            <asp:Label ID="lblSelloCuatro" runat="server" Text='<%# Eval("sello4") %>'></asp:Label>
                        </ItemTemplate>                        
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="#/Carga" ItemStyle-Width="15%">
                        <ItemTemplate>
                            <asp:Label ID="lblNumeroCargo" runat="server" Text='<%# Eval("numeroCarga") %>'></asp:Label>
                        </ItemTemplate>                        
                    </asp:TemplateField>-->
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

        function setRequired(obj) {

            var ddlObjeto = obj.name; //Obtengo el nombre completo del ddl que seleccione
            var nameDDL = obj.id.substring(0, (obj.name.length) - 7); //Solo necesito el nombre, sin el indice del mismo
            var objDDL = document.getElementById(nameDDL); //Obtengo el objeto y sus datos.
            var txtObjeto = ddlObjeto.replace("confirmacionddl", "txtArea"); //Reemplazo el nombre del combo con el de la caja de texto
            var nameTXT = nameDDL.replace("confirmacionddl", "txtArea"); //Reemplazo el nombre del combo con el de la caja de texto, no me preocupo por el indice porque están en la misma fila
            var objTXT = document.getElementById(nameTXT); //Obtengo el objeto y sus datos.
            var nameLblServicio = nameDDL.replace("confirmacionddl", "lblServicio");
            var nameLblPeso = nameDDL.replace("confirmacionddl", "lblPeso");
            var nameDDLTipo = nameDDL.replace("confirmacionddl", "tipoDDL");
            
            var valorActual = objDDL.value; //Valor del ddl
            var valorActualOb = objTXT.value; //Valor del txt

            var objLBLS = document.getElementById(nameLblServicio); //Obtengo el objeto del servicio y sus datos.                
            var objLBLP = document.getElementById(nameLblPeso); //Obtengo el objeto peso y sus datos.                
            var objDDLT = document.getElementById(nameDDLTipo); //Obtengo el objeto tipo y sus datos.

            if (valorActual === 'NO') {
                switch (objLBLS.textContent) {
                    case 'Repesaje':
                        objTXT.value = "";
                        break;
                    case 'Verificación de Sellos':
                        objDDLT.setAttribute("style", "display:none!important");
                        //$('#' + nameDDLTipo + '').html("");
                        break;
                }

                if (valorActualOb === '') {
                    alert('La observación es obligatoria cuando no se ha trabajado el container.');
                }
            } else if (valorActual === 'SI') {
                switch (objLBLS.textContent) {
                    case 'Repesaje':
                        objTXT.value = objLBLP.textContent;
                        break;
                    case 'Verificación de Sellos':
                        objDDLT.setAttribute("style", "display:inline!important");
                       //$('#' + nameDDLTipo + '').html("<option value='SELE'>Selec</option><option value='CISS'>CIIS</option><option value='IMPDT'>IMPDT</option>");                        
                        break;
                }
            } else if (valorActual === 'PE') {
                switch (objLBLS.textContent) {
                    case 'Repesaje':
                        objTXT.value = "";
                        break;
                    case 'Verificación de Sellos':
                        objDDLT.setAttribute("style", "display:none!important");
                        //$('#' + nameDDLTipo + '').html("");
                        break;
                }
            }
        }

        function showGif(ctrl) {
            if (ctrl != null && ctrl != undefined && ctrl.trim().length > 0) {
                document.getElementById(ctrl).innerHTML = '<img alt="" src="../shared/imgs/loader.gif">'
            }
        }

        function popupCallback(objeto) {
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
            window.open('../catalogo/calendarioConsolidacion.aspx?bk=' + bo, 'name', 'width=850,height=480')
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
