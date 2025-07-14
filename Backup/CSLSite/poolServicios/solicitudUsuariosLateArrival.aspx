<%@ Page Title="" Language="C#" MasterPageFile="~/site.Master" AutoEventWireup="true" 
         CodeBehind="solicitudUsuariosLateArrival.aspx.cs" Inherits="CSLSite.solicitudUsuariosLateArrival" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="placehead" runat="server">
    <link href="../shared/estilo/tables.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/aisv-general.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/work.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/catalogos.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/info.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/centroServicios.js" type="text/javascript"></script>
    <link href="../shared/estilo/centroSolicitud.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        function BindFunctions() {
            $(document).ready(function () {
                document.getElementById('imagen').innerHTML = '';
                $('#tabla').tablesorter({ cancelSelection: true, cssAsc: "ascendente", cssDesc: "descendente", headers: { 8: { sorter: false }, 9: { sorter: false}} }).tablesorterPager({ container: $('#pager'), positionFixed: false, pagesize: 10 });
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
    <input id="zonaid" type="hidden" value="712" />
 <asp:ToolkitScriptManager ID="tkcal" EnableScriptGlobalization="true" runat="server"> </asp:ToolkitScriptManager>
 <div>
 <i class="ico-titulo-1"></i><h2>Ingreso de Solicitud (Late Arrival)</h2>  
     <br />
     <br />
 <i class="ico-titulo-2"></i><h1>Solicitud</h1><br />
 </div>
 <div class="seccion">
       <div class="accion">
     <%--  <asp:UpdatePanel ID="updBusquedaExcel" runat="server" ChildrenAsTriggers="true">
        <ContentTemplate> --%>

       <table class="xcontroles" cellspacing="0" cellpadding="1">
       <tr><th class="bt-bottom bt-right bt-left bt-top" colspan="4"> Criterios de consulta:</th></tr>
       <tr>
            <td class="bt-bottom bt-right bt-left">Servicios:</td>
            <td class="bt-bottom bt-right" colspan="4">
                <asp:DropDownList ID="dptiposervicios" runat="server" Width="300px" >
                    <asp:ListItem Value="0">* Seleccione servicios *</asp:ListItem>
                </asp:DropDownList>          
            </td>
       </tr> 
       <tr>
            <td class="bt-bottom bt-right bt-left" >Referencia:</td>
            <td class="bt-bottom bt-right">
                <asp:TextBox ID="txtReferencia" runat="server" Width="295px" MaxLength="11"
                style="text-align: center"
                onkeypress="return soloLetras(event,'01234567890',true)" ReadOnly="true"></asp:TextBox>
                <asp:HiddenField ID="hddFechaCutOff" Value=""  runat="server"/>
                <asp:HiddenField ID="hddReferencia" Value=""  runat="server"/>
            </td>
            <td class="bt-bottom finder">
                <a class="topopup" target="popup" onclick="linkBuque();" >
                <i class="ico-find" ></i> Buscar </a>
            </td>     
            <td class="bt-bottom bt-right validacion "><span id="valcont2" class="validacion"> * obligatorio</span></td>
       </tr>
       <tr>
            <td class="bt-bottom bt-right bt-left">Archivo CSV (separador coma)</td>
            <td class="bt-bottom bt-right" colspan="4">
                 <input style="width:50%;" class="uploader" id="fuContenedores" title="Escoja el archivo CSV (Excel separado por comas)" accept="csv" type="file"  runat="server" clientidmode="Static" />
                <span id="valdae" class="validacion"> * obligatorio</span>
            </td>                   
       </tr>             
    </table>
       <div class="botonera">
          <span id="imagen"></span>
             <asp:Button ID="btnSubirArchivoLateArrival" runat="server" Text="Subir"
               onclick="btnSubirArchivoLateArrival_Click" OnClientClick="return showGif('imagen')" />
       </div>
             <div class="msg-alerta" id="alerta" runat="server" >
       Verifique que los datos del contenedor sean correctos y el mismo se encuentre preavisado. Si el problema persiste comuníquese con Vessel Planners a las extensiones 4050/4054.
        </div>
       
      <%-- </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnSubirArchivoChoferes" />
            </Triggers>
        </asp:UpdatePanel>--%>

       <div class="cataresult" >
        <asp:UpdatePanel ID="upresult" runat="server" ChildrenAsTriggers="true">
        <ContentTemplate>
                <script type="text/javascript">
                    Sys.Application.add_load(BindFunctions); 
                </script>
                
        <input id="idlin" type="hidden" runat="server" clientidmode="Static" />
        <input id="diponible" type="hidden" runat="server" clientidmode="Static" />
        <div id="xfinder" runat="server" visible="false" >
  
        <div class="findresult" >
        <div class="booking" >
                 <div class="separator">Contenedor(es):</div>
                 <div class="bokindetalle">
                 <asp:Repeater ID="tablePagination" runat="server"  >
                 <HeaderTemplate>
                 <table id="tabla"  cellspacing="1" cellpadding="1" class="tabRepeat">
                 <thead>
                 <tr>
                 <th>No.</th>
                 <th style="display: none"></th>
                 <th>Código Contenedor</th>                                        
                 <th>Observación</th>                                        
                 </tr>
                 </thead> 
                 <tbody>
                 </HeaderTemplate>
                 <ItemTemplate>
                 <tr class="point" >
                  <td><asp:Label id='lblNoContenedor' runat="server" Visible="true" Text='<%#Eval("noContenedor")%>'/></td>
                  <td style="display: none"><asp:Label id='lblIDContenedor' runat="server" Text='<%#Eval("idCodigoContenedor")%>'/></td>
                  <td><asp:Label id='lblNombreContenedor' runat="server" Visible="true" Text='<%#Eval("descripcion")%>'/></td> 
                  <td><asp:Label id='lblObservacion' runat="server" Visible="true" Text='<%#Eval("observacion")%>'/></td>                  
                 </tr>
                 </ItemTemplate>
                 <FooterTemplate>
                 </tbody>
                 </table>                 
                 </FooterTemplate>
         </asp:Repeater>
                </div>
        <div class="botonera">
            <span id="imagenSolicitud" runat="server"></span>
            <input type="button" id="btgenerar" value="Generar solicitud" onclick="confirmGenerarSolicitud()"/>
            <asp:Button ID="btgenerarServer" runat="server" Text="Generar solicitud" OnClientClick="showGif('placebody_imagenSolicitud')"
            OnClick="btgenerarServer_Click"  style="display:none;" />
                         <%--<asp:Button ID="btgenerar" runat="server" Text="Generar Solicitud" 
                           onclick="btgenerar_Click" OnClientClick="showGif('placebody_imagenSolicitud')" />--%>
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
        </div>
        <div id="sinresultado" runat="server" class="msg-info"></div>
        </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="btnSubirArchivoLateArrival" />
            </Triggers>
        </asp:UpdatePanel>
       </div>
      </div>
 </div>
 <script src="../Scripts/pages.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.tablesorter.min.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.tablesorter.pager.js" type="text/javascript"></script>
    <script type="text/javascript">
        function confirmGenerarSolicitud() {

            var r = confirm("Se generará una nueva solicitud, ¿Está seguro de realizar la operación?");
            if (r == true) {
                $("#<%=btgenerarServer.ClientID%>").click();
            }
            else {
                return;
            }
        }

        function showGif(ctrl) {
            if (!ValidateFile('fuContenedores', valdae)) {
                return false;
            }
            if (ctrl != null && ctrl != undefined && ctrl.trim().length > 0) {
                document.getElementById(ctrl).innerHTML = '<img alt="" src="../shared/imgs/loader.gif">'
            }
            return true;
        }
        
        $(document).ready(function () {
            $('.datetimepicker').datetimepicker().datetimepicker({ lang: 'es', timepicker: false, closeOnDateSelect: true, format: 'd/m/Y' });
        });

        function popupCallback(objeto) {
            if (objeto == null || objeto == undefined) {
                alert('Hubo un problema al setear un objeto de catalogo');
                return;
            }

            document.getElementById('<%=txtReferencia.ClientID %>').value = objeto.referencia;
            document.getElementById('<%=hddFechaCutOff.ClientID %>').value = objeto.cutoff;
            document.getElementById('<%=hddReferencia.ClientID %>').value = objeto.referencia;
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
