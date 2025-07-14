<%@ Page Title="Autorización de Bookings" Language="C#" MasterPageFile="~/site.Master" AutoEventWireup="true" CodeBehind="booking_autoriza.aspx.cs" Inherits="CSLSite.booking_autoriza" %>
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
      <!--mensajes-->
        <link href="https://cdn.jsdelivr.net/npm/alertifyjs@1.11.0/build/css/alertify.min.css" rel="stylesheet"/>
        <script type="text/javascript" src="https://cdn.jsdelivr.net/npm/alertifyjs@1.11.0/build/alertify.min.js"></script>


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
    <asp:ToolkitScriptManager ID="tkcal" EnableScriptGlobalization="true" runat="server"> </asp:ToolkitScriptManager>
            <input id="agencia" type="hidden" runat="server"  clientidmode="Static"/>     
    <div class="mt-4">         
        <nav class="mt-4" aria-label="breadcrumb">
          <ol class="breadcrumb">
            <li class="breadcrumb-item" id="opcion_principal" runat="server"><a href="#">Gestión financiera</a></li>
             <li class="breadcrumb-item active" aria-current="page" id="sub_opcion" runat="server">Despacho de Contenedores con sobretiempo</li>
          </ol>
        </nav>
      </div>

      <div class="dashboard-container p-4" id="cuerpo" runat="server">
        
		  <div class="form-row">
		  
		   <div class="form-group col-md-12"> 
                       <div class="   alert alert-warning" >
        Estimado Cliente;
        Al procesar los bookings se habilitarán parsa que se puedan despachar fuera del 
            horario normal.
        </div>
		   </div>
		  </div>
		  <div class="form-title">Datos para el registro de sobretiempo</div>
          <div class="form-row">
		  
		   <div class="form-group col-md-4"> 
		   	  <label for="inputAddress">Booking<span style="color: #FF0000; font-weight: bold;"></span></label>
			  <div class="d-flex">
                                     <span id="numbook" class=" form-control col-md-10" onclick="clear();">...</span>
                   <input id="nbrboo" type="hidden" value="" runat="server" clientidmode="Static"/>
                <a  class="btn btn-outline-primary mr-4" target="popup" onclick="openPopup()" >
                                <span class='fa fa-search' style='font-size:24px'></span> 

        </a>
			  </div>
		   </div>

              	   <div class="form-group col-md-4"> 
		   	  <label for="inputAddress">Referencia<span style="color: #FF0000; font-weight: bold;"></span></label>
			  <div class="d-flex">
                                     <span id="referencia" runat="server" clientidmode="Static" class=" form-control col-md-12" >...</span>
                   <input id="xreferencia" type="hidden" value="" runat="server" clientidmode="Static"/>


			  </div>
		   </div>

              		   <div class="form-group col-md-4"> 
		   	  <label for="inputAddress">Línea<span style="color: #FF0000; font-weight: bold;"></span></label>
			  <div class="d-flex">
                                     <span id="linea" runat="server" clientidmode="Static" class=" form-control col-md-12" >...</span>
                   <input id="xlinea" type="hidden" value="" runat="server" clientidmode="Static"/>


			  </div>
		   </div>
		  </div>



          	   <div class="form-row">
		   <div class="col-md-12 d-flex justify-content-center" runat="server" id="div3"> 
		     <div class="d-flex">
                                  <img alt="loading.." src="../shared/imgs/loader.gif" id="Img1" class="nover"  />
                 <asp:Button ID="btnAsumirBook"  CssClass="btn btn-primary"
                     runat="server" Text="Agregar Booking"
                   ToolTip="Agrega el Booking seleccionado a la lista para luego autorizar dicha lista." OnClick="btnAsumirBook_Click"/>


		     </div>
		   </div> 
		   </div>

            <div class="form-row">
		  
		   <div class="form-group col-md-12"> 
                        <div class="bokindetalle" style=" overflow:scroll;  max-height:150px">
         <asp:UpdatePanel ID="UpdatePanel3" runat="server" ChildrenAsTriggers="true">
                 <ContentTemplate> 
                 <asp:Repeater ID="RepeaterBooking" runat="server" 
                     onitemcommand="RepeaterBooking_ItemCommand" >
                 <HeaderTemplate>
                 <table id="tablasortbook" cellspacing="1" cellpadding="1" class="table table-bordered invoice">
                 <thead>
                 <tr>
               
                 <th >Booking</th>
                 <th >Referencia</th>
                 <th >Linea</th>
                 <th>Mensaje</th>
                 <th></th>
            
                 </tr>
                 </thead> 
                 <tbody>
                 </HeaderTemplate>
                 <ItemTemplate>

                  <td ><%#Eval("BOOKING")%></td>
                  <td ><%#Eval("REFERENCIA")%></td>
                  <td ><%#Eval("LINEA")%></td>
                  <td ><%#Eval("MENSAJE")%></td>
                  <td ><asp:Button Text="Remover" CssClass="btn btn-secondary"
                      ID="btnRemoveBook" OnClick="btnRemove_Click" 
                      CommandName="Delete" runat="server"/></td>
                  <td></td>
                 </tr>
                 </ItemTemplate>
                 <FooterTemplate>
                 </tbody>
                 </table>
                 </FooterTemplate>
                 </asp:Repeater>
                 </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnAsumirBook" />
                </Triggers>
         </asp:UpdatePanel>
         </div>
		   </div>
		  </div>

          	   <div class="form-row">
		   <div class="col-md-12 d-flex justify-content-center" runat="server" id="btnera"> 
		     <div class="d-flex">
                               <img alt="loading.." src="../shared/imgs/loader.gif" id="loader" class="nover"  />
              <asp:Button ID="btsalvar"   CssClass=" btn btn-primary"
                  runat="server" Text="Autorizar sobretiempo"  onclick="btsalvar_Click"
                   OnClientClick="return prepareObject('¿Estimado Cliente, está seguro de procesar los Bookings?')"
                   ToolTip="Procesar Bookings."/>


		     </div>
		   </div> 
		   </div>

          	   <div class="form-row">
		   <div class="col-md-12 d-flex justify-content-center"> 
		          <div class="   alert  alert-primary" >
        Estimado Cliente<br />
        Al procesar los bookings de la lista, se habilita que los mismos puedan despacharse en horarios después de las 17h30.
        </div>
		   </div> 
		   </div>
     </div>




<%--  </div>--%>
    <asp:HiddenField runat="server" ID="hfRucBuscar" />
    <script src="../Scripts/pages.js" type="text/javascript"></script>
    <script src="../Scripts/credenciales.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.tablesorter.min.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.tablesorter.pager.js" type="text/javascript"></script>

    <script type="text/javascript">

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

        function getGifOcultaBuscar() {
            //document.getElementById('loader3').className = 'nover';

            return true;
        }
        function getGifOculta() {
            //document.getElementById('loader2').className = 'nover';

            
            document.getElementById('referencia').textContent = "";
            document.getElementById('xreferencia').value = "";
            
            document.getElementById('numbook').textContent = "";
            document.getElementById('nbrboo').value = "";

            document.getElementById('linea').textContent = "";
            document.getElementById('xlinea').value = "";

            return true;
        }

        function getGifOcultaEnviar(mensaje) {
            document.getElementById('loader').className = 'nover';
            alertify.alert(mensaje);
            return true;
        }
        function getBookOculta() {
            //document.getElementById('loader2').className = 'nover';
            return true;
        }
        var programacion = {};
        var lista = [];
        function prepareObjectTable(mensaje) {

            try {
                if (confirm(mensaje) == false) {
                    return false;
                }
                document.getElementById("loader2").className = '';
                lista = [];
                //validaciones básicas
                var vals = document.getElementById('xreferencia');
                if (vals == null || vals == undefined || vals.value == '') {
                    alertify.alert('¡ Seleccione la Referencia.');
                    document.getElementById("loader2").className = 'nover';
                    document.getElementById("buscarref").focus();
                    return false;
                }
                
                var vals = document.getElementById('nbrboo');
                if (vals == null || vals == undefined || vals.value.trim().length <= 2) {
                    alertify.alert('¡ Seleccione el numero de Booking que pertenece al Cliente.');
                    document.getElementById("loader2").className = 'nover';
                    return false;
                }
                return true;
            } catch (e) {
                alertify.alert('Lamentamos comunicarle que ha ocurrido el siguiente problema:\n' + e.Message);
            }
        }

        function prepareObjectTableRemove(mensaje) {

            try {
                if (confirm(mensaje) == false) {
                    return false;
                }
                return true;
            } catch (e) {
                alertify.alert('Lamentamos comunicarle que ha ocurrido el siguiente problema:\n' + e.Message);
            }
        }

//        function prepareObjectRuc() {
//            try {
//                document.getElementById("loader3").className = '';
//                
//                return true;
//            } catch (e) {
//                alert('Lamentamos comunicarle que ha ocurrido el siguiente problema:\n' + e.Message);
//            }
//        }

//        var ced_count = 0;
//        var jAisv = {};
//        $(document).ready(function () {
//            $('.datetimepicker').datetimepicker().datetimepicker({ lang: 'es', timepicker: false, closeOnDateSelect: true, format: 'd/m/Y' });
//            $('.datepicker').datetimepicker().datetimepicker({ lang: 'es', timepicker: false, format: 'd/m/Y', closeOnDateSelect: true, minDate: '0' });
//        });
        function add(button) {
            var row = button.parentNode.parentNode;
            var cells = row.querySelectorAll('td:not(:last-of-type)');
            addToCartTable(cells);
        }

        function remove() {
            var row = this.parentNode.parentNode;demofab
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

        function popupCallbackBook(objeto) {
            if (objeto == null || objeto == undefined) {
                alertify.alert('¡ Hubo un problema al setear un objeto de catalogo.');
                return;
            }
            document.getElementById('numbook').textContent = objeto.nbr;
            document.getElementById('nbrboo').value = objeto.nbr;
            document.getElementById('referencia').textContent = objeto.id;
            document.getElementById('xreferencia').value = objeto.id;
            document.getElementById('linea').textContent = objeto.id;
            document.getElementById('xlinea').value = objeto.id;
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
                alertify.alert('Lamentamos comunicarle que ha ocurrido el siguiente problema:\n' + e.Message);
            }
        }

        function openPop() {
            var bo = document.getElementById('nbrboo').value;
            if (bo == undefined || bo == '' || bo == null) {
              alertify.alert('Por favor seleccione el booking primero');
                return;
            }
            window.open('../catalogo/Calendario.aspx?bk='+bo, 'name', 'width=850,height=880')
        }

        function setObjectRef(row) {
            var celColect = row.getElementsByTagName('td');
            var catalogo = {
                referencia: celColect[0].textContent,
                nave: celColect[1].textContent,
            };
            popupCallbackRef(catalogo);
        }

        function setObjectBook(row) {
           var celColect = row.getElementsByTagName('td');
          var catalogo = {
              fila: celColect[0].textContent,
              gkey: celColect[1].textContent,
              nbr: celColect[2].textContent,
              linea: celColect[3].textContent,
              fk: celColect[4].textContent
              };
            popupCallbackBooking(catalogo);
      }
      function popupCallback(objeto, catalogo) {
            
                document.getElementById('numbook').textContent = objeto.nbr;
                document.getElementById('nbrboo').value = objeto.nbr;
                document.getElementById('referencia').textContent = objeto.id;
                document.getElementById('xreferencia').value = objeto.id;
                document.getElementById('linea').textContent = objeto.linea;
                document.getElementById('xlinea').value = objeto.linea;
                return;
        }
        function openPopup() {
            //var ref = document.getElementById('xreferencia').value;
            //var ruc = document.getElementById('xruc').value;
            //window.open('../mantenimientos_proforma_expo/autoriza-booking', 'name', 'width=1000,height=480');
            window.open('../catalogo/bookaut','name','width=850,height=880')
            //window.location = '../facturacion/emision-pase-de-puerta';
            return true;
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
