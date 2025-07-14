<%@ Page Title="Compensación" Language="C#" MasterPageFile="~/site.Master" AutoEventWireup="true" CodeBehind="~/Pago en Linea/Compensacion.aspx.cs" Inherits="CSLSite.Pago_en_Linea.Compensacion" %>
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
            function BindFunctions1() {
                $(document).ready(function () {
                    document.getElementById('imagen').innerHTML = '';
                    $('#tablasort1').tablesorter({ cancelSelection: true, cssAsc: "ascendente", cssDesc: "descendente", headers: { 8: { sorter: false }, 9: { sorter: false}} }).tablesorterPager({ container: $('#Div2'), positionFixed: false, pagesize: 10 });
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
        <input id="idCliente" type="hidden" runat="server" clientidmode="Static"/>
         <input id="idAnticipo" type="hidden" runat="server" clientidmode="Static"/>
         <input id="saldoAnticipoSeleccionado" type="hidden" runat="server" clientidmode="Static"/>
         <input id="facturas" type="hidden" runat="server" clientidmode="Static"/>
    <div>
   <i class="ico-titulo-1"></i><h2>Compensación de Anticipos</h2>  <br /> 
   <i class="ico-titulo-2"></i><h1>Compensación de Anticipos</h1><br />
 </div>
  <div class="seccion">
              <div class=" msg-alerta">
    <span id="dtlo" runat="server">Estimado usuario:</span> 
    <br /> Asegúrese que toda la información que agrega a este documento es correcta antes de proceder a su respectivo proceso, si desea confirmar alguna información antes de proceder comuníquese con nuestro departamento finaciero a los teléfonos: +593 (04) 6006300, 
                 ext. 8016, 8017, 8019, 8044, 8045. 
    </div>
       <div class="accion">
         <table class="xcontroles" cellspacing="0" cellpadding="1">
            <tr><th class="bt-bottom bt-right  bt-left bt-top"> Datos del documento buscado</th></tr>
         <div class="botonera">
          <span id="imagen"></span>
             <asp:Button ID="btbuscar" runat="server" Text="Iniciar la búsqueda" onclick="Buscar" />
         </div>
         </table>
          <asp:TextBox ID="TextBox2"  runat="server" Width="250px" MaxLength="19"  ClientIDMode="Static" onkeypress="return soloLetras(event,'01234567890/')"></asp:TextBox>      
            <asp:TextBox ID="desded"  runat="server" Width="90px" MaxLength="10" CssClass="datetimepicker"
             onkeypress="return soloLetras(event,'01234567890/')" 
                  ClientIDMode="Static"></asp:TextBox>
             <asp:TextBox ID="hastad"  runat="server" ClientIDMode="Static" Width="90px" MaxLength="15" CssClass="datetimepicker"
             onkeypress="return soloLetras(event,'01234567890/')" 
                 ></asp:TextBox>

         <div class="cataresult" >
               <asp:UpdatePanel ID="upresult" runat="server" ChildrenAsTriggers="true">
                     <ContentTemplate>
                                 <script type="text/javascript">
                                     Sys.Application.add_load(BindFunctions); 
                      </script>
             <div id="xfinder" runat="server" visible="false" >
             <div class="findresult" >
             <div class="booking" >
                  <div class="separator">Anticipos encontrados</div>
                 <div class="bokindetalle">
                 <asp:Repeater ID="tablePagination" runat="server" 
                         onitemcommand="tablePagination_ItemCommand1" >
                 <HeaderTemplate>
                 <table id="tablasort"  cellspacing="1" cellpadding="1" class="tabRepeat" >
                 <thead>
                 <tr>
                 <th>Número Liquidación</th>
                 <th>Fecha Registro</th>
                 <th>Monto</th>
                 <th>Saldo</th>
                 <th>Selec.</th>
                 </tr>
                 </thead> 
                 <tbody>
                 </HeaderTemplate>
                 <ItemTemplate>
                 <tr class="point" >
                  <td style="width: 200px"><%#Eval("NUMERO_LIQUIDACION")%></td>
                  <td style="width: 200px"><%#DataBinder.Eval(Container.DataItem, "FECHA_REGISTRO", "{0:dd-MM-yyyy HH:mm}")%></td>
                  <td style="width: 60px"><%#Eval("MONTO_TOTAL")%></td>
                  <td style="width: 60px"><%#Eval("SALDO")%></td>
                  <td style="width: 80px">
                   <div class="tcomand">
                       <input type="radio" name="gender" onchange="EstablecerColorFila(this.parentElement.parentElement.parentElement)"/>
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
                    Disponible a Compensar $
                 <input id="disponibleACompensar" type="text" disabled="disabled"/>
            </div>
              </div>
               <div id="sinresultado" runat="server" class="msg-info"></div>
                  </ContentTemplate>
                     <Triggers>
                     <asp:AsyncPostBackTrigger ControlID="btbuscar" />
                     </Triggers>
                 </asp:UpdatePanel>
             </div>
         <div class="cataresult" >
               <asp:UpdatePanel ID="UpdatePanel1" runat="server" ChildrenAsTriggers="true">
                  <ContentTemplate>
<%--                     <script type="text/javascript">
                                     Sys.Application.add_load(BindFunctions1); 
                      </script>--%>
                     <div id="xfinderFacturas" runat="server" visible="false" >
             <div class="findresult" >
             <div class="booking" >
                  <div class="separator">Facturas(Pendientes de Pago)</div>
                 <div class="bokindetalle">
                 <asp:Repeater ID="tableFacturas" runat="server" 
                         onitemcommand="tablePagination_ItemCommand" >
                 <HeaderTemplate>
                 <table id="tablasort1" cellspacing="1" cellpadding="1" class="tabRepeat" >
                 <thead>
                 <tr>
                 <th>Número Liquidación</th>
                 <th>Fecha Registro</th>
                 <th>Monto</th>
                 <th>Saldo</th>
                 <th>Monto a Pagar</th>
                 <th>Selec.</th>
                 </tr>
                 </thead> 
                 <tbody>
                 </HeaderTemplate>
                 <ItemTemplate>
                 <tr class="claseFactura" >
                  <td style="width: 200px"><%#Eval("NUMERO_LIQUIDACION")%></td>
                  <td style="width: 200px"><%#DataBinder.Eval(Container.DataItem, "FECHA_REGISTRO", "{0:dd-MM-yyyy HH:mm}")%></td>
                  <td style="width: 60px"><%#Eval("MONTO_TOTAL")%></td>
                  <td style="width: 60px"><%#Eval("SALDO")%></td>
                  <td style="width: 80px">
                   <div class="tcomand">
                       <input type="text" id="montoPagar" disabled="disabled" onkeypress="return soloLetras(event,'01234567890./')" onblur="return ActualizarSaldoDisponible()" />
                   </div>
                  </td>
                  <td style="width: 80px">
                   <div class="tcomand">
                       <input type="checkbox" name="checkSeleccion" onchange ="CambiarEstadoMontoAIngresar(this.parentElement.parentElement.parentElement)"/>
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
<%--             <div id="Div2">
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
                 
            </div>--%>
              </div>
                  </ContentTemplate>
                     <Triggers>
                     <asp:AsyncPostBackTrigger ControlID="btbuscar" />
                     </Triggers>
                 </asp:UpdatePanel>
         </div>
              <asp:Button Hidden = "true" ID="BtnCompensar" runat="server" Text="Compensar" onclick="Compensar" OnClientClick="return RegistrarFacturasSeleccionadas()"/>
      </div>
  </div>
    <script src="../Scripts/pages.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.tablesorter.min.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.tablesorter.pager.js" type="text/javascript"></script>
    <script type="text/javascript">
        var clics = 0;
        $(document).ready(function () {
            $('.datetimepicker').datetimepicker().datetimepicker({ lang: 'es', timepicker: false, closeOnDateSelect: true, format: 'd/m/Y' });
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

        function clear() {
            document.getElementById('numbook').textContent = '...';
            document.getElementById('nbrboo').value = '';
        }
        function RegistrarFacturasSeleccionadas() {
            clics = clics + 1;
            if (clics > 1)
                return false;
            var facturas = '';
            document.getElementById('facturas').textContent = '';
            var table = document.getElementById("tablasort1");
            for (var i = 1, row; row = table.rows[i]; i++) {
                //iterate through rows
                //rows would be accessed using the "row" variable assigned in the for loop
                if (row.cells[5].children['0'].children.checkSeleccion.checked) {
                    facturas = facturas + row.cells[0].innerText + '-' + row.cells[4].children['0'].children.montoPagar.value + ';';
                }
            }
            document.getElementById('facturas').value = facturas;
            return true;
        }
        function EstablecerColorFila(element) {
//            $('#tablasort tr').each(function() {
//                $(this).find('td').each(function() {
//                    $(this).bgColor = "blue";
//                });
//            });

            //element.bgColor = "yellow";
            document.getElementById('idAnticipo').value = element.children['0'].innerText;// element.firstChild.nextSibling.outerText;
            document.getElementById('disponibleACompensar').value = element.children['3'].innerText;
            document.getElementById('saldoAnticipoSeleccionado').value = element.children['3'].innerText;
            $('#<%= BtnCompensar.ClientID %>')[0].hidden = true;
            BorrarDatos();
        }
        function BorrarDatos() {
            var table = document.getElementById("tablasort1");
            for (var i = 1, row; row = table.rows[i]; i++) {
                if (row.cells[5].children['0'].children.checkSeleccion.checked) {
                    row.cells[5].children['0'].children.checkSeleccion.checked = false;
                    row.cells[4].children['0'].children.montoPagar.disabled = true;
                    row.cells[4].children['0'].children.montoPagar.value = "";
                }
            }
        }
        function ActualizarSaldoDisponible() {
            $('#<%= BtnCompensar.ClientID %>')[0].hidden = true;
            var montoTotalAPagar = parseFloat('0.00').toFixed(2);
            var montoDisponibleActual = document.getElementById('saldoAnticipoSeleccionado').value;
            var table = document.getElementById("tablasort1");
            for (var i = 1, row; row = table.rows[i]; i++) {
                if (row.cells[5].children['0'].children.checkSeleccion.checked) {
                    if (isNaN(row.cells[4].children['0'].children.montoPagar.value)) {
                        alert('Formato de monto incorrecto.');
                        return;
                    }
                    else {
                        if (row.cells[4].children['0'].children.montoPagar.value > parseFloat(row.cells[3].textContent.replace(',', '.'))) {
                            alert('El monto a pagar ingresado sobresa el saldo de la factura.');
                            return;
                        }
                        else
                            montoTotalAPagar = parseFloat(montoTotalAPagar - 1 + 1 + (row.cells[4].children['0'].children.montoPagar.value - 1 + 1)).toFixed(2);
                    }
                }
             }
            if (montoTotalAPagar > parseFloat(montoDisponibleActual.replace(',', '.'))) {
                alert('El monto total a pagar es mayor al disponible del anticipo seleccionado.');
                return;
            }
            else
                document.getElementById('disponibleACompensar').value = parseFloat(parseFloat(montoDisponibleActual.replace(',', '.')).toFixed(2) - parseFloat(montoTotalAPagar).toFixed(2)).toFixed(2);
            if (parseFloat(montoTotalAPagar).toFixed(2) > 0)
                $('#<%= BtnCompensar.ClientID %>')[0].hidden = false;
        }
        function CambiarEstadoMontoAIngresar(element) {
            if (document.getElementById('disponibleACompensar').value == "") {
                alert('Primero debe seleccionar un anticipo antes de seleccionar una factura pendiente.');
                element.children['5'].children['0'].children.checkSeleccion.checked = false;
                return;
            }
            element.children['4'].children['0'].children.montoPagar.disabled = !element.children['5'].children['0'].children.checkSeleccion.checked;
            if (element.children['4'].children['0'].children.montoPagar.disabled) {
//                var monto = parseFloat('0.00').toFixed(2);
//                monto = parseFloat(monto - 1 + 1 + (document.getElementById('disponibleACompensar').value - 1 + 1)).toFixed(2);
//                monto = parseFloat(monto - 1 + 1 + (element.children['4'].children['0'].children.montoPagar.value - 1 + 1)).toFixed(2);
//                document.getElementById('disponibleACompensar').value = monto;
                element.children['4'].children['0'].children.montoPagar.value = "";
//                if (monto.toString() == document.getElementById('saldoAnticipoSeleccionado').value.replace(',', '.'))
//                    $('#<%= BtnCompensar.ClientID %>')[0].hidden = true;
                document.getElementById('disponibleACompensar').value = document.getElementById('saldoAnticipoSeleccionado').value;
                ActualizarSaldoDisponible();
            }
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
  <asp:updateprogress associatedupdatepanelid="UpdatePanel1"
    id="updateProgress1" runat="server">
    <progresstemplate>
        <div id="progressBackgroundFilter"></div>
        <div id="processMessage"> Estamos procesando la tarea que solicitó, por favor espere...<br />
             <img alt="Loading" src="../shared/imgs/loader.gif" style="margin:0 auto;" />
        </div>
    </progresstemplate>
</asp:updateprogress>
  </asp:Content>
