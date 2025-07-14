<%@ Page Title="Pago a Terceros" Language="C#" MasterPageFile="~/site.Master" AutoEventWireup="true" CodeBehind="pago_terceros.aspx.cs" Inherits="CSLSite.pago_terceros" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="placehead" runat="server">
    <link href="../shared/estilo/tables.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/aisv-general.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/work.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/catalogos.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/info.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/w3-progressbar.css" rel="stylesheet" type="text/css" />
    <link href="../shared/avisos/ppt.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/turnos.js" type="text/javascript"></script>
    <script type="text/javascript">
//        $(document).ready(function () {
//            $('#tablasort').tablesorter({ cancelSelection: true, cssAsc: "ascendente", cssDesc: "descendente", headers: { 5: { sorter: false }, 6: { sorter: false}} }).tablesorterPager({ container: $('#pager'), positionFixed: false, pagesize: 10 });
//        });
  
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
* input[type=text]
    {
        text-align:left!important;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="placebody" runat="server">
    <input id="zonaid" type="hidden" value="1203" />
    <asp:ToolkitScriptManager ID="tkcal" EnableScriptGlobalization="true" runat="server"> </asp:ToolkitScriptManager>
    <div>
   <i class="ico-titulo-1"></i><h2>Mantenimiento</h2>  <br /> 
   <i class="ico-titulo-2"></i><h1>Pago a Terceros</h1><br />
 </div><div class="seccion">
       <div class="accion">
         <input id="agencia" type="hidden" runat="server"  clientidmode="Static"/>
         <table class="xcontroles" cellspacing="0" cellpadding="1">
         <tr>
            <th class="bt-bottom bt-right  bt-left bt-top" colspan="3">DATOS PARA REGISTRO DE PAGO A TERCEROS</th>
         </tr>
         </table>
         <div class="cataresult" >
               <asp:UpdatePanel ID="upresult" runat="server" ChildrenAsTriggers="true">
                     <ContentTemplate>
                       <input id="idlin" type="hidden" runat="server" clientidmode="Static" />
                       <input id="diponible" type="hidden" runat="server" clientidmode="Static" />

                  <div id="xfinder" runat="server" >
             <div class="findresult" >
             <div class="booking" >
                 <div class="bokindetalle" style=" overflow-y: scroll;">
                 <table id="tabla" cellspacing="1" cellpadding="1" class="tabRepeat">
                 <thead>
                 <tr>
                 <th style=" width:150px">Referencia</th>
                 <th style=" width:150px">Nave</th>
                 <th style=" width:150px">Fase</th>
                 <th style=" width:100px">Acciones</th>
                 </tr>
                 <tr>
                  <td colspan="4" style=" width:99%">
                    <input id="buscarref"  style=" width:99%" type="text" class="form-control" placeholder="Escriba algo para filtrar" />
                  </td>
                 </tr>
                 </thead> 
                 </table>
                 </div>
                 <div class="bokindetalle" style=" overflow:scroll; height:100px">
                 <asp:Repeater ID="tablePaginationReferencias" runat="server" >
                 <HeaderTemplate>
                 <table id="tablasort" cellspacing="1" cellpadding="1" class="tabRepeat">
                 <thead>
                 </thead> 
                 <tbody>
                 </HeaderTemplate>
                 <ItemTemplate>
                 <tr class="point" onclick="setObjectRef(this);">
                  <td style=" width:150px"><%#Eval("id")%></td>
                  <td style=" width:150px"><%#Eval("name")%></td>
                  <td style=" width:150px"><%#Eval("fase")%></td>
                  <td style=" width:100px">
                     <a href="#" >Elegir</a>
                  </td>
                 </tr>
                  </ItemTemplate>
                 <FooterTemplate>
                 </tbody>
                 </table>
                 </FooterTemplate>
         </asp:Repeater>
         <%--<div id="pager">
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
             </div>
             </div>
              </div>
                  </ContentTemplate>
                     <Triggers>
                         <%--<asp:AsyncPostBackTrigger ControlID="btsalvar" />--%>
                     </Triggers>
                 </asp:UpdatePanel>
             </div>
         <table class="xcontroles" cellspacing="0" cellpadding="1">
         <tr>
         <td class="bt-bottom bt-left bt-right" >Referencia:</td>
         <td class="bt-bottom " >
                   <span id="referencia" runat="server" clientidmode="Static" class="caja" style=" width:280px!important;">...</span>
                   <input id="xreferencia" type="hidden" value="" runat="server" clientidmode="Static"/>
          </td>
         <td class="bt-bottom bt-right validacion">
         </td>
         </tr>
         </table>
         <div class="cataresult" >
               <asp:UpdatePanel ID="UpdatePanel1" runat="server" ChildrenAsTriggers="true">
                     <ContentTemplate>
                       <input id="Hidden1" type="hidden" runat="server" clientidmode="Static" />
                       <input id="Hidden2" type="hidden" runat="server" clientidmode="Static" />

                  <div id="Div1" runat="server" >
             <div class="findresult" >
             <div class="booking" >
                 <div class="bokindetalle" style=" overflow-y: scroll;">
                 <table id="Table1" cellspacing="1" cellpadding="1" class="tabRepeat">
                 <thead>
                 <tr>
                 <th style=" width:50px">Código SAP</th>
                 <th style=" width:50px">Ruc</th>
                 <th style=" width:300px">Razon Social</th>
                 <th style=" width:100px">Acciones</th>
                 </tr>
                 <tr>
                  <td colspan="4" style=" width:99%">
                    <input id="buscarcli"  style=" width:99%" type="text" class="form-control" placeholder="Escriba algo para filtrar" />
                  </td>
                 </tr>
                 </thead> 
                 </table>
                 </div>
                 <div class="bokindetalle" style=" overflow:scroll; height:100px">
                 <asp:Repeater ID="tablePaginationClientes" runat="server" >
                 <HeaderTemplate>
                 <table id="tablasortcli" cellspacing="1" cellpadding="1" class="tabRepeat">
                 <thead>
                 </thead> 
                 <tbody>
                 </HeaderTemplate>
                 <ItemTemplate>
                 <tr class="point" onclick="setObjectCliente(this);">
                 <td style=" width:50px"><%#Eval("CODIGO_SAP")%></td>
                  <td style=" width:50px"><%#Eval("CLNT_CUSTOMER")%></td>
                  <td style=" width:300px"><%#Eval("CLNT_NAME")%></td>
                  <td style=" width:100px">
                     <a href="#" >Elegir</a>
                  </td>
                 </tr>
                  </ItemTemplate>
                 <FooterTemplate>
                 </tbody>
                 </table>
                 </FooterTemplate>
         </asp:Repeater>
         <%--<div id="pager">
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
             </div>
             </div>
              </div>
                  </ContentTemplate>
                     <Triggers>
                         <%--<asp:AsyncPostBackTrigger ControlID="btsalvar" />--%>
                     </Triggers>
                 </asp:UpdatePanel>
             </div>
             <table class="xcontroles" cellspacing="0" cellpadding="1">
             <tr>
         <td class="bt-bottom bt-left bt-right" >Código SAP:</td>
         <td class="bt-bottom " >
                   <span id="codigosap" runat="server" clientidmode="Static" class="caja" style=" width:280px!important;">...</span>
                   <input id="xcodigosap" type="hidden" value="" runat="server" clientidmode="Static"/>
          </td>
         <td class="bt-bottom bt-right validacion">
         </td>
         </tr>
         <tr>
         <td class="bt-bottom bt-left bt-right" >Ruc:</td>
         <td class="bt-bottom " >
                   <span id="ruc" runat="server" clientidmode="Static" class="caja" style=" width:280px!important;">...</span>
                   <input id="xruc" type="hidden" value="" runat="server" clientidmode="Static"/>
          </td>
         <td class="bt-bottom bt-right validacion">
         </td>
         </tr>
         <tr>
         <td class="bt-bottom bt-left bt-right" >Razon Social:</td>
         <td class="bt-bottom " >
                   <span id="razonsocial" runat="server" clientidmode="Static" class="caja" style=" width:280px!important;">...</span>
                   <input id="xrazonsocial" type="hidden" value="" runat="server" clientidmode="Static"/>
          </td>
         <td class="bt-bottom bt-right validacion">
         </td>
         </tr>
         </table>
         <div class="botonera" runat="server" id="btnera">
              <img alt="loading.." src="../shared/imgs/loader.gif" id="loader" class="nover"  />
              <asp:Button ID="btsalvar" runat="server" Text="Procesar Pago a Terceros"  onclick="btsalvar_Click" 
                   OnClientClick="return prepareObject('¿Esta seguro de procesar el Pago a Terceros?')"
                   ToolTip="Procesar Pago a Terceros."/>
         </div>
      </div>
  </div>
    <script src="../Scripts/pages.js" type="text/javascript"></script>
    <script src="../Scripts/credenciales.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.tablesorter.min.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.tablesorter.pager.js" type="text/javascript"></script>

    <script type="text/javascript">

        document.querySelector("#buscarref").onkeyup = function () {
            $TableFilter("#tablasort", this.value);
        }

        document.querySelector("#buscarcli").onkeyup = function () {
            $TableFilter("#tablasortcli", this.value);
        }

        $TableFilter = function (id, value) {
            var rows = document.querySelectorAll(id + ' tbody tr');

            for (var i = 0; i < rows.length; i++) {
                var showRow = false;

                var row = rows[i];
                row.style.display = 'none';

                for (var x = 0; x < row.childElementCount; x++) {
                    if (row.children[x].textContent.toLowerCase().indexOf(value.toLowerCase().trim()) > -1) {
                        showRow = true;
                        break;
                    }
                }

                if (showRow) {
                    row.style.display = null;
                }
            }
        }

        var ced_count = 0;
        var jAisv = {};
        $(document).ready(function () {
            $('.datetimepicker').datetimepicker().datetimepicker({ lang: 'es', timepicker: false, closeOnDateSelect: true, format: 'd/m/Y' });
            $('.datepicker').datetimepicker().datetimepicker({ lang: 'es', timepicker: false, format: 'd/m/Y', closeOnDateSelect: true, minDate: '0' });
        });
        function add(button) {
            var row = button.parentNode.parentNode;
            var cells = row.querySelectorAll('td:not(:last-of-type)');
            addToCartTable(cells);
        }

        function remove() {
            var row = this.parentNode.parentNode;
            document.querySelector('#target tbody')
            .removeChild(row);
        }

        function addToCartTable(cells) {
            var code = cells[1].innerText;
            var name = cells[2].innerText;
            var price = cells[3].innerText;

            var newRow = document.createElement('tr');

            newRow.appendChild(createCell(code));
            newRow.appendChild(createCell(name));
            newRow.appendChild(createCell(price));
            var cellInputQty = createCell();
            cellInputQty.appendChild(createInputQty());
            newRow.appendChild(cellInputQty);
            var cellRemoveBtn = createCell();
            cellRemoveBtn.appendChild(createRemoveBtn())
            newRow.appendChild(cellRemoveBtn);

            document.querySelector('#target tbody').appendChild(newRow);
        }

        function createInputQty() {
            var inputQty = document.createElement('input');
            inputQty.type = 'number';
            inputQty.required = 'true';
            inputQty.min = 1;
            inputQty.className = 'form-control'
            return inputQty;
        }

        function createRemoveBtn() {
            var btnRemove = document.createElement('button');
            btnRemove.className = 'btn btn-xs btn-danger';
            btnRemove.onclick = remove;
            btnRemove.innerText = 'Descartar';
            return btnRemove;
        }

        function createCell(text) {
            var td = document.createElement('td');
            if (text) {
                td.innerText = text;
            }
            return td;
        }

        function popupCallbackRef(objeto) {
            if (objeto == null || objeto == undefined) {
                alert('¡ Hubo un problema al setaar un objeto de catalogo.');
                return;
            }
            document.getElementById('referencia').textContent = objeto.referencia;
            document.getElementById('xreferencia').textContent = objeto.referencia;
            return;
        }

        function popupCallbackCliente(objeto) {
            if (objeto == null || objeto == undefined) {
                alert('¡ Hubo un problema al setaar un objeto de catalogo.');
                return;
            }
            document.getElementById('codigosap').textContent = objeto.codigosap;
            document.getElementById('xcodigosap').textContent = objeto.codigosap;
            document.getElementById('ruc').textContent = objeto.ruc;
            document.getElementById('xruc').textContent = objeto.ruc;
            document.getElementById('razonsocial').textContent = objeto.razonsocial;
            document.getElementById('xrazonsocial').textContent = objeto.razonsocial;
            return;
        }

        function clear() {
            document.getElementById('').textContent = '...';
            document.getElementById('').value = '';
        }

        var programacion = {};
        var lista = [];
        function prepareObject(mensaje) {

            try {
                if (confirm(mensaje) == false) {
                    return false;
                }
                document.getElementById("loader").className = '';
                
                return true;
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
            window.open('../catalogo/Calendario.aspx?bk='+bo, 'name', 'width=850,height=480')
        }

        function setObjectRef(row) {
            var celColect = row.getElementsByTagName('td');
            var catalogo = {
                referencia: celColect[0].textContent
            };
            popupCallbackRef(catalogo);
        }

        function setObjectCliente(row) {
            var celColect = row.getElementsByTagName('td');
            var catalogo = {
                codigosap: celColect[0].textContent,
                ruc: celColect[1].textContent,
                razonsocial: celColect[2].textContent
            };
            popupCallbackCliente(catalogo);
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
