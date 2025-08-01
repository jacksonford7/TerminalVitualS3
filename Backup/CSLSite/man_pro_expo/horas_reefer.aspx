﻿<%@ Page Title="Horas Reefer" Language="C#" MasterPageFile="~/site.Master" AutoEventWireup="true" CodeBehind="horas_reefer.aspx.cs" Inherits="CSLSite.horas_reefer" %>
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
   <i class="ico-titulo-2"></i><h1>Horas Reefer</h1><br />
 </div><div class="seccion">
       <div class="accion">
         <input id="agencia" type="hidden" runat="server"  clientidmode="Static"/>
         <table class="xcontroles" cellspacing="0" cellpadding="1">
         <tr>
            <th class="bt-bottom bt-right  bt-left bt-top" colspan="3">Datos PARA REGISTRO DE HORAS REEFER QUE ASUME LA LINEA</th>
         </tr>
         </table>
         <div class="cataresult" >
               <asp:UpdatePanel ID="upresult" runat="server" ChildrenAsTriggers="true">
                     <ContentTemplate>
                       <input id="idlin" type="hidden" runat="server" clientidmode="Static" />
                       <input id="diponible" type="hidden" runat="server" clientidmode="Static" />

                  <div id="xfinder" runat="server" >
                 <%--<div class="msg-alerta" id="alerta" runat="server" ></div>--%>
             <div class="findresult" >
             <div class="booking" >
                  <%--<div class="separator">Lineas disponibles:</div>--%>
                 <div class="bokindetalle" style=" overflow-y: scroll;">
                 <table id="tabla" cellspacing="1" cellpadding="1" class="tabRepeat">
                 <thead>
                 <tr>
                 <th style=" width:150px">Linea</th>
                 <th style=" width:150px">Cantidad de Horas</th>
                 <th style=" width:150px">Fecha de Vigencia</th>
                 <th style=" width:100px">Acciones</th>
                 </tr>
                 <tr>
                  <td colspan="4" style=" width:99%">
                    <input id="buscar"  style=" width:99%" type="text" class="form-control" placeholder="Escriba algo para filtrar" />
                  </td>
                 </tr>
                 </thead>
                 </table>
                 </div>
                 <div class="bokindetalle" style=" overflow:scroll; height:150px">
                 <asp:Repeater ID="tablePagination" runat="server" >
                 <HeaderTemplate>
                 <table id="tablasort" cellspacing="1" cellpadding="1" class="tabRepeat">
                 <thead>
                 </thead> 
                 <tbody>
                 </HeaderTemplate>
                 <ItemTemplate>
                 <tr class="point" onclick="setObject(this);">
                  <td style=" width:150px"><%#Eval("SAHR_LINEA")%></td>
                  <td style=" width:150px"><%#Eval("SERE_HORAS")%></td>
                  <td style=" width:150px"><%#Eval("SERE_FECHAVIGENCIA")%></td>
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
        <%-- <div id="pager">
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
               <div id="sinresultado" runat="server" class="msg-info"></div>
                  </ContentTemplate>
                     <Triggers>
                         <%--<asp:AsyncPostBackTrigger ControlID="btsalvar" />--%>
                     </Triggers>
                 </asp:UpdatePanel>
             </div>
         <table class="xcontroles" cellspacing="0" cellpadding="1">
         <tr>
         <td class="bt-bottom bt-left bt-right" >Linea:</td>
         <td class="bt-bottom " >
                   <span id="lineanav" runat="server" clientidmode="Static" class="caja" style=" width:280px!important;">...</span>
                   <input id="xlineanav" type="hidden" value="" runat="server" clientidmode="Static"/>
          </td>
         <td class="bt-bottom bt-right validacion">
        <%--<a  class="topopup" target="popup" onclick="window.open('../mantenimientos_proforma_expo/lineas-asume-horas-reefer','name','width=850,height=480')" >
          <i class="ico-find" ></i> Buscar </a>--%>
         </td>
         </tr>
         <tr>
         <td class="bt-bottom bt-left bt-right">Cantidad de Horas Reefer:</td>
          <td class="bt-bottom  " >
                   <span id="ncantidadhoras" runat="server" clientidmode="Static" class="caja" style=" width:280px!important;">...</span>
                   <input id="xcantidadhoras" type="hidden" value="" runat="server" clientidmode="Static" />
          </td>
          <td class="bt-bottom bt-right"></td>
         </tr>
         <tr>
         <td class="bt-bottom bt-left bt-right">Fecha de Vigencia:</td>
          <td class="bt-bottom  " >
                   <span id="fechavigencia" runat="server" clientidmode="Static" class="caja" style=" width:280px!important;">...</span>
                   <input id="xfechavigencia" type="hidden" value="" runat="server" clientidmode="Static" />
          </td>
          <td class="bt-bottom bt-right"></td>
         </tr>
         <tr>
         <td class="bt-bottom bt-right bt-left">Nueva Cantidad de Horas Reefer:</td>
         <td class="bt-bottom ">
              <asp:TextBox 
             style="text-align: center"  onblur="checkcaja(this,'valcanthoras',true);"
             ID="txtcanthoras" runat="server"  width="200px" MaxLength="3"
             onkeypress="return soloLetras(event,'0123456789')" 
             ></asp:TextBox>
             <asp:CheckBox Text="Asume todo"  ID="chkAsumeTodo" runat="server" onchange="fValidaAsumeTodo()"/>
         </td>
         <td class="bt-bottom bt-right validacion ">
         <span id="valcanthoras" class="validacion" > * obligatorio</span>
         </td>
         </tr>
         <tr>
         <td class="bt-bottom bt-right bt-left">Nueva Fecha de Vigencia:</td>
         <td class="bt-bottom ">  
             <asp:TextBox 
             style="text-align: center"  onblur="checkcaja(this,'valfecvig',true);"
             ID="txtfecvigencia" runat="server"  width="200px" MaxLength="15" CssClass="datepicker"
             onkeypress="return soloLetras(event,'0123456789/')" 
             ></asp:TextBox>
             </td>
         <td class="bt-bottom bt-right validacion ">
         <span id="valfecvig" class="validacion" > * obligatorio</span>
         </td>
         </tr>
         </table>
         <div class="botonera" runat="server" id="btnera">
              <img alt="loading.." src="../shared/imgs/loader.gif" id="loader" class="nover"  />
              <asp:Button ID="btsalvar" runat="server" Text="Procesar Horas Reefer"  onclick="btsalvar_Click" 
                   OnClientClick="return prepareObject('¿Esta seguro de procesar las Horas Reefer?')"
                   ToolTip="Procesar Horas Reefer."/>
         </div>
            <%--<link href="https://cdnjs.cloudflare.com/ajax/libs/twitter-bootstrap/3.3.6/css/bootstrap.min.css" rel="stylesheet"/>--%>
            <%--
<div class="table-responsive">
<button onclick="add(this)" class="btn btn-primary btn-xs">
          Agregar
        </button>
<div class="table-responsive">
<table id="target" class="table table-bordered table-hover">
  <thead>
    <tr>
      <th>Código</th>
      <th>Nombre</th>
      <th>Precio</th>
      <th>Cantidad</th>
      <th>Acciones</th>
    </tr>
  </thead>
  <tbody>
  </tbody>
</table>
</div>
            </div>
            --%>
      </div>
  </div>
    <asp:HiddenField runat="server" ID="hfBusqueda" runat="server" /> 
    <script src="../Scripts/pages.js" type="text/javascript"></script>
    <script src="../Scripts/credenciales.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.tablesorter.min.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.tablesorter.pager.js" type="text/javascript"></script>

    <script type="text/javascript">

        document.querySelector("#buscar").onkeyup = function () {
            $TableFilter("#tablasort", this.value);
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

        function getGifOculta(mensaje) {
            document.getElementById('buscar').value = document.getElementById('<%=hfBusqueda.ClientID %>').value;

            var id = "#tablasort";
            var value = document.getElementById('<%=hfBusqueda.ClientID %>').value;

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
            alert(mensaje);
            //document.getElementById("loader").className = 'nover';
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
        function popupCallback(objeto) {
            if (objeto == null || objeto == undefined) {
                alert('¡ Hubo un problema al setaar un objeto de catalogo.');
                return;
            }
            document.getElementById('lineanav').textContent = objeto.linea;
            document.getElementById('ncantidadhoras').textContent = objeto.cant_horas;
            document.getElementById('fechavigencia').textContent = objeto.fecha_vigencia;
            document.getElementById('xlineanav').value = objeto.linea;
            document.getElementById('xcantidadhoras').value = objeto.cant_horas;
            document.getElementById('xfechavigencia').value = objeto.fecha_vigencia;
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
                lista = [];
                //validaciones básicas
                var vals = document.getElementById('xlineanav');
                if (vals == null || vals == undefined || vals.value == '') {
                    alert('¡ Consulte la Linea Naviera para Asumir la Nueva Cantidad de Horas Reefer y Fecha de Vegencia.');
                    document.getElementById("loader").className = 'nover';
                    return false;
                }

                var asume = document.getElementById('<%=chkAsumeTodo.ClientID %>').checked;
                if (!asume) {
                    var vals = document.getElementById('<%=txtcanthoras.ClientID %>');
                    if (vals == null || vals == undefined || vals.value == '') {
                        alert('¡ Digite la Nueva Cantidad de Horas Reefer que Asume la Linea.');
                        document.getElementById('<%=txtcanthoras.ClientID %>').focus();
                        document.getElementById("loader").className = 'nover';
                        return false;
                    }
                }
                var vals = document.getElementById('<%=txtfecvigencia.ClientID %>');
                if (vals == null || vals == undefined || vals.value == '') {
                    alert('¡ Seleccione la Nueva Fecha de Vigencia que Asume la Linea.');
                    document.getElementById('<%=txtfecvigencia.ClientID %>').focus();
                    document.getElementById("loader").className = 'nover';
                    return false;
                }
                document.getElementById('<%=hfBusqueda.ClientID %>').value = document.getElementById('buscar').value;
                return true;
            } catch (e) {
                alert('Lamentamos comunicarle que ha ocurrido el siguiente problema:\n' + e.Message);
            }
        }

        function fValidaAsumeTodo() {
            var asume = document.getElementById('<%=chkAsumeTodo.ClientID %>').checked;
            if (asume) {
                document.getElementById('<%=txtcanthoras.ClientID %>').value = "";
                document.getElementById('<%=txtcanthoras.ClientID %>').disabled = true;
                document.getElementById('<%=txtcanthoras.ClientID %>').style.cssText = "background-color:Gray;width:200px;";
//                document.getElementById('<%=txtfecvigencia.ClientID %>').focus();
            }
            else {
                document.getElementById('<%=txtcanthoras.ClientID %>').disabled = false;
                document.getElementById('<%=txtcanthoras.ClientID %>').style.cssText = "background-color:White;width:200px;";
                document.getElementById('<%=txtcanthoras.ClientID %>').focus();
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

        function setObject(row) {
            var celColect = row.getElementsByTagName('td');
            var catalogo = {
                linea: celColect[0].textContent,
                cant_horas: celColect[1].textContent,
                fecha_vigencia: celColect[2].textContent
            };
            popupCallback(catalogo);
            /*
            if (window.opener != null) {
                window.opener.popupCallback(catalogo);
            }
            self.close();
            */
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
