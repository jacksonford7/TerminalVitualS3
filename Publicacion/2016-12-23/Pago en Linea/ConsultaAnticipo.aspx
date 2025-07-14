<%@ Page Title="Consultar Anticipos" Language="C#" MasterPageFile="~/site.Master" AutoEventWireup="true" CodeBehind="~/Pago en Linea/ConsultaAnticipo.aspx.cs" Inherits="CSLSite.Pago_en_Linea.ConsultaAnticipo" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="placehead" runat="server">
    <link href="../shared/estilo/tables.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/aisv-general.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/work.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/catalogos.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/info.css" rel="stylesheet" type="text/css" />
        <script type="text/javascript">
            function BindFunctions() {
                $(document).ready(function () {
                    document.getElementById('imagen').innerHTML = '';
                    $('#tablasort').tablesorter({ cancelSelection: true, cssAsc: "ascendente", cssDesc: "descendente", headers: { 8: { sorter: false }, 9: { sorter: false}} }).tablesorterPager({ container: $('#pager'), positionFixed: false, pagesize: 10 });
                });
            }
    </script>
    <style type="text/css">
        
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
    <asp:ToolkitScriptManager ID="tkcal" EnableScriptGlobalization="true" runat="server"> </asp:ToolkitScriptManager>
      <input id="zonaid" type="hidden" value="8" />
        <input id="nombreCliente" type="hidden" runat="server" clientidmode="Static"/>
        <input id="idCliente" type="hidden" runat="server" clientidmode="Static"/>
        <input id="rolCliente" type="hidden" runat="server" clientidmode="Static"/>
        <input id="userName" type="hidden" runat="server" clientidmode="Static"/>
        <input id="codigoAnticipo" type="hidden" runat="server" clientidmode="Static"/>
        <asp:Button ID="BtnAnularOculto" runat="server"  OnClick="AnularAnticipo" style="visibility: hidden; display: none;" />
    <div>
   <i class="ico-titulo-1"></i><h2>Generación y consulta de anticipos </h2>  <br /> 
   <i class="ico-titulo-2"></i><h1>Registro</h1><br />
 </div>
  <div class="seccion">
       <div class="accion">
            <div class=" msg-alerta">
    <span id="dtlo" runat="server">Estimado usuario:</span> 
    <br /> Asegúrese que toda la información que agrega a este documento es correcta antes de proceder a su respectiva generación, si desea confirmar alguna información antes de proceder comuníquese con nuestro departamento de planificación a los teléfonos: +593 (04) 6006300, 3901700 
                 ext. 4039, 4040, 4060. 
    </div>
             <div>
                <asp:Label ID="Label1" runat="server" Text="Monto Anticipo $:"></asp:Label>
                <asp:TextBox ID="TextBox1" ValidationGroup="Ingreso" runat="server" onkeypress="return soloLetras(event,'01234567890./')"></asp:TextBox>
                <asp:Label ID="Label2" runat="server" Text="Número de Booking:"></asp:Label>
                <asp:TextBox ID="TextBox4" runat="server"></asp:TextBox>
                <asp:Button ID="Button1" runat="server" Text="Registrar" OnClick="GrabarAnticipo" ValidationGroup="Ingreso"/>
                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" ControlToValidate="TextBox1" ValidationGroup="Ingreso"
                                                  runat="server" ErrorMessage="El valor ingresado está incorrecto." ForeColor="Red" Display="Dynamic" 
                                                  ValidationExpression="(?!^0*$)(?!^0*\.0*$)^\d{1,18}(\.\d{1,2})?$" ValidateEmptyText="false">
                                                  </asp:RegularExpressionValidator>
                <asp:RequiredFieldValidator 
                    ControlToValidate="TextBox1" Display="Dynamic" 
                    Text="Por favor ingrese el anticipo."
                    ErrorMessage="Por favor ingrese el anticipo." 
                    runat="Server"  ForeColor="Red" ValidationGroup="Ingreso"
                    ID="TextRequiredFieldValidator" />
    </div>

         <table class="xcontroles" cellspacing="0" cellpadding="1">
            <tr><th class="bt-bottom bt-right  bt-left bt-top"> Datos del documento buscado</th></tr>
                     <div class="botonera">
          <span id="imagen"></span>
             <asp:Button ID="btbuscar" runat="server" Text="Iniciar la búsqueda" onclick="Buscar" UseSubmitBehavior="false"/>
         </div>
         </table>
            <asp:TextBox ID="TextBox3" runat="server" Width="80px" MaxLength="19"  ClientIDMode="Static" BorderColor="Transparent" Enabled="false" ReadOnly="true"></asp:TextBox>      
          <asp:TextBox ID="TextBox2" runat="server" Width="190px" MaxLength="19"  ClientIDMode="Static" onkeypress="return soloLetras(event,'01234567890/')"></asp:TextBox>      
            <asp:TextBox ID="desded" runat="server" Width="90px" MaxLength="10" CssClass="datetimepicker"
             onkeypress="return soloLetras(event,'01234567890/')" 
                  ClientIDMode="Static"></asp:TextBox>
             <asp:TextBox ID="hastad" runat="server" ClientIDMode="Static" Width="90px" MaxLength="15" CssClass="datetimepicker"
             onkeypress="return soloLetras(event,'01234567890/')" 
                 ></asp:TextBox>
            <asp:ComboBox ID="ComboBox1" runat="server" Width="100px" DropDownStyle ="Simple">
            <asp:ListItem Text="Todos" Value="0" />
            <asp:ListItem Text="Pendiente" Value="1" />
            <asp:ListItem Text="Confirmado" Value="2" />
            <asp:ListItem Text="Aplicado" Value="2" />
        </asp:ComboBox>

         <div class="cataresult" >
               <asp:UpdatePanel ID="upresult" runat="server" ChildrenAsTriggers="true">
                     <ContentTemplate>
                                 <script type="text/javascript">
                                     Sys.Application.add_load(BindFunctions); 
                      </script>
             <div id="xfinder" runat="server" visible="false" >
             <div class="findresult" >
             <div class="booking" >
                  <div class="separator">Documentos encontrados</div>
                 <div class="bokindetalle">
                 <asp:Repeater ID="tablePagination" runat="server" 
                         onitemcommand="tablePagination_ItemCommand" >
                 <HeaderTemplate>
                 <table id="tablasort" cellspacing="1" cellpadding="1" class="tabRepeat">
                 <thead>
                 <tr>
                 <th>#</th>
                 <th>Monto</th>
                 <th>Numero Liquidación</th>
                 <th>Fecha Registro</th>
                 <th>Estado</th>
                 <th>Acciones</th>
                 <th> </th>
                 </tr>
                 </thead> 
                 <tbody>
                 </HeaderTemplate>
                 <ItemTemplate>
                 <tr class="point" >
                  <td style="width: 10px"><%#Eval("ITEM")%></td>
                  <td style="width: 60px"><%#Eval("MONTO_TOTAL")%></td>
                  <td style="width: 200px"><%#Eval("NUMERO_LIQUIDACION")%></td>
                  <td style="width: 0px" hidden="true"><%#Eval("CODIGO_ANTICIPO")%></td>
                  <td style="width: 200px"><%#DataBinder.Eval(Container.DataItem, "FECHA_REGISTRO", "{0:dd-MM-yyyy HH:mm}")%></td>
                  <td><%#Eval("ESTADO")%></td>
                  <td style="width: 80px">
                   <div class="tcomand">
                       <a href="../Pago En Linea/ImprimirAnticipo.aspx?sid=<%# securetext(Eval("NUMERO_LIQUIDACION")) %>" class="imprimir" target="_blank">Detalle</a>
                       <a href="../Pago En Linea/ImprimirFacturasPagadas.aspx?sid=<%# securetext(Eval("CODIGO_ANTICIPO")) %>" class="imprimir" target="_blank">Pagos</a>
                   </div>
                  </td>
                  <td style="width: 20px">
                   <div>
                        <input id="BtnAnular" type="button" value="Anular" onclick="AnularAnticipo(<%#Eval("CODIGO_ANTICIPO")%>)" />
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
                 <input clientidmode="Static" id="dataexport" onclick="getTable('resultado');" type="button" value="Exportar" runat="server" />
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
        $(document).ready(function () {
            $('.datetimepicker').datetimepicker().datetimepicker({ lang: 'es', timepicker: false, closeOnDateSelect: true, format: 'd/m/Y' });


        });
        $("#BtnAnular").click(function () {
            //document.getElementById('codigoAnticipo').value = facturas;
        });
        $(window).load(function () {
            var table = document.getElementById("tablasort");
            for (var i = 1, row; row = table.rows[i]; i++) {
                if (row.cells[5].innerText != "PENDIENTE") {
                    row.cells[7].children['0'].children.BtnAnular.disabled = true;
                }
            }
        });
        function popupCallback(objeto, catalogo) {
            if (objeto == null || objeto == undefined) {
                alert('Hubo un problema al setear un objeto de catalogo');
                return;
            }
            //si catalogos es booking
            if (catalogo == 'bk') {
                document.getElementById('numbook').textContent = objeto.boking;
                document.getElementById('nbrboo').value = objeto.boking;
                return;
            }
        }
        function AnularAnticipo(codigo) {
            document.getElementById("codigoAnticipo").value = codigo;
           // alert(document.getElementById("BtnAnularOculto").textContent);
            $("#<%= BtnAnularOculto.ClientID %>").click();
        }
        function clear() {
            document.getElementById('numbook').textContent = '...';
            document.getElementById('nbrboo').value = '';
        }

        function soloLetras(e, caracteres, espacios) {

            key = e.keyCode || e.which;
            tecla = String.fromCharCode(key).toLowerCase();
            if (caracteres) {
                letras = caracteres;
            }
            else {
                letras = " áéíóúabcdefghijklmnñopqrstuvwxyz";
            }
            if (espacios == undefined || espacios == null) {
                especiales = [8, 13, 32, 9, 16, 20];
            }
            else {
                especiales = [8, 13, 9, 16, 20];
            }
            tecla_especial = false
            for (var i in especiales) {
                if (key == especiales[i]) {
                    tecla_especial = true;
                    break;
                }
            }
            if (letras.indexOf(tecla) == -1 && !tecla_especial) {
                return false;
            }
        }
  </script>

  <asp:updateprogress associatedupdatepanelid="upresult"
    id="updateProgress" runat="server">
    <progresstemplate>
        <div id="progressBackgroundFilter"></div>
        <div id="processMessage"> Estamos procesando la tarea que solicitó, por favor espere...<br />
             <img alt="Loading" src="../shared/imgs/loader.gif" style="margin:0 auto;" />
        </div>
    </progresstemplate>
</asp:updateprogress>
  </asp:Content>

