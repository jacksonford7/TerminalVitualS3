<%@ Page MaintainScrollPositionOnPostback="true" Title="Pago a Terceros" Language="C#" MasterPageFile="~/site.Master" AutoEventWireup="true" CodeBehind="mant_pago_terceros.aspx.cs" Inherits="CSLSite.mant_pago_terceros" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="placehead" runat="server">
    <link href="../shared/estilo/tables.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/catalogos.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/w3-progressbar.css" rel="stylesheet" type="text/css" />
    <link href="../shared/avisos/ppt.css" rel="stylesheet" type="text/css" />

         <!--mensajes-->
        <link href="https://cdn.jsdelivr.net/npm/alertifyjs@1.11.0/build/css/alertify.min.css" rel="stylesheet"/>
        <script type="text/javascript" src="https://cdn.jsdelivr.net/npm/alertifyjs@1.11.0/build/alertify.min.js"></script>

    <script src="../Scripts/turnos.js" type="text/javascript"></script>

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

    <div class="mt-4">         
        <nav class="mt-4" aria-label="breadcrumb">
          <ol class="breadcrumb">
            <li class="breadcrumb-item" id="opcion_principal" runat="server"><a href="#">Gestión financiera</a></li>
             <li class="breadcrumb-item active" aria-current="page" id="sub_opcion" runat="server">Mantenimiento de Pago a Terceros</li>
          </ol>
        </nav>
      </div>

          <input id="agencia" type="hidden" runat="server"  clientidmode="Static"/>
    <div class="dashboard-container p-4" id="cuerpo" runat="server">
         <div class="form-title">Datos del cliente que asume el pago</div>
		  <div class="form-row">


		       <div class="form-group col-md-12 "> 
		   	  <label for="inputAddress"><span style="color: #FF0000; font-weight: bold;">RUC</span></label>
			 <div class="d-flex">
                   <asp:TextBox 
                     CssClass="form-control"
                    ID="txtrucbuscarcli" runat="server" 
                    MaxLength="13" placeholder="ESCRIBA EL RUC"
                    onkeypress="return soloLetras(event,'abcdefghijklmnñopqrstuvwxyz0123456789')" 
                    ></asp:TextBox>

                                      		       <asp:Button ID="btBuscarCli"  CssClass="btn btn-primary"
                     runat="server" Text="Buscar" OnClientClick="return prepareObjectRuc()"
                   ToolTip="Busca al Cliente por el RUC." OnClick="btBuscarCli_Click"/>
                   <img alt="loading.." src="../shared/imgs/loader.gif" id="loader1" class="nover"  />
                   </div>
		   </div>
           
		

              

		
		  
		  </div>
         <div class="form-row">
		   <div class="form-group col-md-12"> 
          <asp:UpdatePanel ID="UpdatePanel3" runat="server" ChildrenAsTriggers="true">
                     <ContentTemplate>
                       <input id="Hidden3" type="hidden" runat="server" clientidmode="Static" />
                       <input id="Hidden4" type="hidden" runat="server" clientidmode="Static" />

                  <div id="Div3" runat="server" >
          
                 <div class="bokindetalle" style=" overflow:scroll;  max-height:200px">
                 <asp:Repeater ID="tablePagination" runat="server" >
                 <HeaderTemplate>
                 <table id="tablasortcli" cellspacing="1" cellpadding="1" class="table table-bordered invoice">
                 <thead>
                 <tr>
                 <th class="xax" >Código SAP</th>
                 <th style=" width:75px">Ruc</th>
                 <th>Razon Social</th>
                 <th class="xax">Email</th>
                 <th style=" width:50px"></th>
                 </tr>
                 </thead> 
                 <tbody>
                 </HeaderTemplate>
                 <ItemTemplate>
                  <tr class="point" onclick="setObjectClienteAsume(this);">
                  <td  class="xax" ><%#Eval("CODIGO_SAP")%></td>
                  <td style=" width:75px"><%#Eval("CLNT_CUSTOMER")%></td>
                  <td><%#Eval("CLNT_NAME")%></td>
                  <td class="xax" ><%#Eval("EMAIL")%></td>
                  <td style=" width:50px">
                     <a href="#" class="btn btn-secondary" >Elegir</a>
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
                  </ContentTemplate>
                     <Triggers>
                         <asp:AsyncPostBackTrigger ControlID="btnBuscar" />
                         <asp:AsyncPostBackTrigger ControlID="btBuscarCli" />
                     </Triggers>
                 </asp:UpdatePanel>
		   </div>
         </div>
         <div class="form-row">
		   <div class="form-group col-md-6"> 
		   	  <label for="inputAddress">Ruc<span style="color: #FF0000; font-weight: bold;"></span></label>
			  <div class="d-flex">
                   <span id="ruca" runat="server" clientidmode="Static" class=" form-control col-md-12">...</span>
                   <input id="xruca" type="hidden" value="" runat="server" clientidmode="Static"/>
                   <span id='valrucasumecli' class="validacion" > * </span>
			  </div>
		   </div>
		 
		  
		   <div class="form-group col-md-6"> 
		   	  <label for="inputAddress">Razón social<span style="color: #FF0000; font-weight: bold;"></span></label>
			  <div class="d-flex">
                                     <span id="razsoca" runat="server" clientidmode="Static" class=" form-control clo-md-12" >...</span>
                   <input id="xrazsoca" type="hidden" value="" runat="server" clientidmode="Static"/>
             <span id='valrazsoccli' class="validacion" > * </span>


			  </div>
		   </div>
		  </div>
        
        
        
        
        <div class="form-title">Datos del tercero que desea asumir el pago</div>
        <div class="form-row">
		  
		   <div class="form-group col-md-6"> 
  <label for="inputAddress"><span style="color: #FF0000; font-weight: bold;">RUC</span></label>		   

		   </div>
             <div class="form-group col-md-12"> 
			  <div class="d-flex">
                     <asp:TextBox 
                    CssClass="form-control"
                    ID="txtrucbuscar" runat="server"   MaxLength="13" placeholder="ESCRIBA EL RUC"
                    onkeypress="return soloLetras(event,'abcdefghijklmnñopqrstuvwxyz0123456789')" 
                    ></asp:TextBox>

                   <asp:Button ID="btnBuscar"  CssClass="btn btn-primary"
                     runat="server" Text="Buscar" OnClientClick="return prepareObjectRucAsume()"
                   ToolTip="Busca al Cliente por el RUC." OnClick="btnBuscar_Click"/>

                               <img alt="loading.." src="../shared/imgs/loader.gif" id="loader3" class="nover"  />

			  </div>
		   </div>
		  </div>
        <div class="form-row">
		  
		   <div class="form-group col-md-12"> 
                                <div class="cataresult" >
               <asp:UpdatePanel ID="UpdatePanel1" runat="server" ChildrenAsTriggers="true">
                     <ContentTemplate>
                       <input id="Hidden1" type="hidden" runat="server" clientidmode="Static" />
                       <input id="Hidden2" type="hidden" runat="server" clientidmode="Static" />

                  <div id="Div1" runat="server" >
                 <div class="bokindetalle" style=" overflow:scroll;   max-height:150px">
                 <asp:Repeater ID="tablePaginationClientes" runat="server" >
                 <HeaderTemplate>
                 <table id="tablasortcliasume" cellspacing="1" cellpadding="1" class="table table-bordered invoice">
                 <thead>
                 <tr>
                 <th class="xax" >Código SAP</th>
                 <th >Ruc</th>
                 <th>Razon Social</th>
                 <th class="xax">Email</th>
                 <th >Acciones</th>
                 </tr>
                 </thead> 
                 <tbody>
                 </HeaderTemplate>
                 <ItemTemplate>
                  <tr class="point" onclick="setObjectCliente(this);">
                  <td  class="xax" ><%#Eval("CODIGO_SAP")%></td>
                  <td><%#Eval("CLNT_CUSTOMER")%></td>
                  <td><%#Eval("CLNT_NAME")%></td>
                  <td class="xax" ><%#Eval("EMAIL")%></td>
                  <td >
                     <a href="#"  class="btn btn-secondary" >Elegir</a>
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
                  </ContentTemplate>
                     <Triggers>
                         <asp:AsyncPostBackTrigger ControlID="btnBuscar" />
                     </Triggers>
                 </asp:UpdatePanel>
             </div>

               </div>
            </div>
        <div class="form-row">
		  
		   <div class="form-group col-md-6"> 
		   	  <label for="inputAddress">Ruc<span style="color: #FF0000; font-weight: bold;"></span></label>
			  <div class="d-flex">
                        <span id="ruc" runat="server" clientidmode="Static" class=" form-control col-md-12" style=" width:280px!important;">...</span>
                   <input id="xruc" type="hidden" value="" runat="server" clientidmode="Static"/>
                      <span id='valrucasume' class="validacion" > * </span>
			  </div>
		   </div>
	
		  
		   <div class="form-group col-md-6"> 
		   	  <label for="inputAddress">Razón social<span style="color: #FF0000; font-weight: bold;"></span></label>
			  <div class="d-flex">
                                     <span id="razonsocial" runat="server" clientidmode="Static" class=" form-control col-md-12" style=" width:280px!important;">...</span>
                   <input id="xrazonsocial" type="hidden" value="" runat="server" clientidmode="Static"/>
         <span id='valrazsoc' class="validacion" > * </span>


			  </div>
		   </div>
		  </div>
        <div class="form-row">
		   <div class="col-md-12 d-flex justify-content-center"  runat="server" id="divAsumirCliente"> 
		     <div class="d-flex">
       <asp:Button ID="Button1"  CssClass="btn  btn-dark mr-2"
                         runat="server" Text="Buscar y Modificar Tercero"
                       OnClientClick="return openPop()"
                       ToolTip="Busca y Modifica el Catalogo de Terceros."/>

                                      <asp:Button ID="btnAsumir"  CssClass="btn  btn-dark"
                         runat="server" Text="Asumir y Agregar Tercero"
                       OnClientClick="return prepareObjectTable('¿Esta seguro de asumir el Pago del Cliente.?')"
                       ToolTip="Asume al Cliente seleccionado y lo agrega a una lista para luego procesar el Pago a Terceros." OnClick="btnAsumir_Click"/>

                                                           <img alt="loading.." src="../shared/imgs/loader.gif" id="loader2" class="nover"  />

		     </div>
		   </div> 
		   </div>
        	   <div class="form-row">
		   <div class="col-md-12"> 
		       <div class="cataresult" >
               <asp:UpdatePanel ID="UpdatePanel2" runat="server" ChildrenAsTriggers="true">
                 <ContentTemplate> 
                 <div id="Div2" runat="server" >
                
             
                           
                 <asp:Repeater ID="RepeaterAsume" runat="server" OnItemCommand="RepeaterAsume_ItemCommand" >
                 <HeaderTemplate>
                 <table id="tabLeasume" cellspacing="1" cellpadding="1" class="table table-bordered invoice">
                 <thead>
                    <th>Ruc Cliente</th>
                    <th>Razon Social</th>
                    <th>Ruc Tercero</th>
                    <th>Razon Social Tercero</th>
                    <th></th>
                 </thead> 
                 <tbody>
                 </HeaderTemplate>
                 <ItemTemplate>
                 <tr class="point">
                  <td ><%#Eval("RUCCLI")%></td>
                  <td ><%#Eval("RAZSOCIALCLI")%></td>
                  <td ><%#Eval("RUCASUME")%></td>
                  <td ><%#Eval("RAZSOCIALASUME")%></td>
                  <td ><asp:Button Text="Remover"
                      ID="btnRemove" OnClick="btnRemove_Click"  CssClass="btn btn-secondary"
                      OnClientClick="return prepareObjectTableRemove('¿Esta seguro de remover el Cliente que Asumio?')" CommandName="Delete" runat="server"/></td>
                 </tr>
                  </ItemTemplate>
                 <FooterTemplate>
                 </tbody>
                 </table>
                 </FooterTemplate>
                 </asp:Repeater>
              
                 
               
                 </div>
                 </ContentTemplate>
                 <Triggers>
                         <asp:AsyncPostBackTrigger ControlID="btnAsumir" />
                 </Triggers>
                 </asp:UpdatePanel>
         </div>
		   </div> 
		   </div>

        	   <div class="form-row">
		   <div class="col-md-12 d-flex justify-content-center" runat="server" id="btnera"> 
		     <div class="d-flex">
                               <img alt="loading.." src="../shared/imgs/loader.gif" id="loader" class="nover"  />
              <asp:Button ID="btsalvar"  CssClass="btn  btn-success"
                  runat="server" Text="Procesar Mantenimiento"  onclick="btsalvar_Click"
                   OnClientClick="return prepareObject('¿Estimado Cliente, está seguro de procesar el pago a Terceros?')"
                   ToolTip="Procesar Pago a Terceros."/>


		     </div>
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
            document.getElementById('loader1').className = 'nover';
            document.getElementById('loader3').className = 'nover';
            return true;
        }
        function getGifOculta() {
            document.getElementById('loader2').className = 'nover';

            document.getElementById('ruc').textContent = "";
            document.getElementById('xruc').value = "";
            document.getElementById('razonsocial').textContent = "";
            document.getElementById('xrazonsocial').value = "";


            document.getElementById('valrucasume').textContent = " * obligatorio";
            document.getElementById('valrazsoc').textContent = " * obligatorio";

            document.getElementById('<%=txtrucbuscar.ClientID %>').value = "";
            //document.getElementById('<%=txtrucbuscarcli.ClientID %>').value = "";
            
            return true;
        }

        function getGifOcultaEnviar(mensaje) {
            document.getElementById('loader').className = 'nover';
            alertify.alert(mensaje);
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
                var vals = document.getElementById('xruca');
                if (vals == null || vals == undefined || vals.value == '') {
                    alertify.alert('¡ Seleccione el Ruc del que asume el pago a un tercero(s)');
                    document.getElementById("loader2").className = 'nover';
                    return false;
                }
                var vals = document.getElementById('xruc');
                if (vals == null || vals == undefined || vals.value == '') {
                    alertify.alert('¡ Seleccione el Ruc del tercero que desea asumir pago');
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

        function prepareObjectRuc() {
            try {
                document.getElementById("loader1").className = '';
                var vals = document.getElementById('<%=txtrucbuscarcli.ClientID %>').value;
                if (vals == null || vals == undefined || vals == '') {
                    alertify.alert('¡ Escriba el Ruc del que asume el pago a un tercero(s)');
                    document.getElementById("loader1").className = 'nover';
                    document.getElementById('<%=txtrucbuscarcli.ClientID %>').focus();
                    return false;
                }
                return true;
            } catch (e) {
                alertify.alert('Lamentamos comunicarle que ha ocurrido el siguiente problema:\n' + e.Message);
            }
        }

        function prepareObjectRucAsume() {
            try {
                document.getElementById("loader3").className = '';
                var vals = document.getElementById('<%=txtrucbuscar.ClientID %>').value;
                if (vals == null || vals == undefined || vals == '') {
                    alertify.alert('¡ Escriba el Ruc del tercero que desea asumir pago');
                    document.getElementById("loader3").className = 'nover';
                    document.getElementById('<%=txtrucbuscar.ClientID %>').focus();
                    return false;
                }
                return true;
            } catch (e) {
                alertify.alert('Lamentamos comunicarle que ha ocurrido el siguiente problema:\n' + e.Message);
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

        function popupCallbackRef(objeto) {
            if (objeto == null || objeto == undefined) {
                alertify.alert('¡ Hubo un problema al setear un objeto de catalogo.');
                return;
            }
            document.getElementById('referencia').textContent = objeto.referencia;
            document.getElementById('xreferencia').value = objeto.referencia;
            document.getElementById('nave').textContent = objeto.nave;
            document.getElementById('xnave').value = objeto.nave;
            document.getElementById('valreferencia').textContent = "";
            document.getElementById('valnave').textContent = "";
            return;
        }

        function popupCallbackCliente(objeto) {
            if (objeto == null || objeto == undefined) {
                alertify.alert('¡ Hubo un problema al setaar un objeto de catalogo.');
                return;
            }
            document.getElementById('ruc').textContent = objeto.ruc;
            document.getElementById('xruc').value = objeto.ruc;
            document.getElementById('razonsocial').textContent = objeto.razonsocial;
            document.getElementById('xrazonsocial').value = objeto.razonsocial;
            document.getElementById('valrucasume').textContent = "";
            document.getElementById('valrazsoc').textContent = "";
            return;
        }

        function popupCallbackClienteAsume(objeto) {
            if (objeto == null || objeto == undefined) {
                alertify.alert('¡ Hubo un problema al setear un objeto de catalogo.');
                return;
            }
            document.getElementById('ruca').textContent = objeto.ruc;
            document.getElementById('xruca').value = objeto.ruc;
            document.getElementById('razsoca').textContent = objeto.razonsocial;
            document.getElementById('xrazsoca').value = objeto.razonsocial;
            document.getElementById('valrucasumecli').textContent = "";
            document.getElementById('valrazsoccli').textContent = "";
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

        function setObjectRef(row) {
            var celColect = row.getElementsByTagName('td');
            var catalogo = {
                referencia: celColect[0].textContent,
                nave: celColect[1].textContent,
            };
            popupCallbackRef(catalogo);
        }

        function setObjectCliente(row) {
            var celColect = row.getElementsByTagName('td');
            var catalogo = {
                ruc: celColect[1].textContent,
                razonsocial: celColect[2].textContent
            };
            popupCallbackCliente(catalogo);
        }

        function setObjectClienteAsume(row) {
            var celColect = row.getElementsByTagName('td');
            var catalogo = {
                ruc: celColect[1].textContent,
                razonsocial: celColect[2].textContent
            };
            popupCallbackClienteAsume(catalogo);
        }

        function openPop() {
            window.open('../catalogo/catalogo_pago_terceros.aspx', 'name', 'width=1300,height=880');
            return false;
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
