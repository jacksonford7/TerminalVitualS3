<%@ Page Title="Pago a Terceros" Language="C#" MasterPageFile="~/site.Master" AutoEventWireup="true" CodeBehind="pago_terceros.aspx.cs" Inherits="CSLSite.pago_terceros" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="placehead" runat="server">
    <link href="../shared/estilo/tables.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/catalogos.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/w3-progressbar.css" rel="stylesheet" type="text/css" />
    <link href="../shared/avisos/ppt.css" rel="stylesheet" type="text/css" />
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
        .auto-style1 {
            height: 35px;
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
             <li class="breadcrumb-item active" aria-current="page" id="sub_opcion" runat="server">Mantenimiento Pago a Terceros</li>
          </ol>
        </nav>
      </div>
     <div class="dashboard-container p-4" id="cuerpo" runat="server">
         <input id="agencia" type="hidden" runat="server"  clientidmode="Static"/>
		  <div class="form-row">
		  
		   <div class="form-group col-md-12"> 
		        <div class=" alert-primary" >
        Estimado Cliente;
        Al asumir valores de un tercero, asume todos los valores de la carga de un cliente en particular, incluyendo los servicios de Inspecciones, roleos, almacenajes, late arrival y valores adicionales que pudiesen
        generarse antes del embarque de la carga; considerar que no podrá asumir valores parciales.
        </div>
		   </div>
		  </div>

           <div class="form-title">Datos para el registro</div>
		   <div class="form-row">
		   <div class="form-group col-md-12"> 
                <div class="cataresult" >
               <asp:UpdatePanel ID="UpdatePanel0" runat="server" ChildrenAsTriggers="true">
                     <ContentTemplate>
                       <input id="idlin" type="hidden" runat="server" clientidmode="Static" />
                       <input id="diponible" type="hidden" runat="server" clientidmode="Static" />

                  <div id="xfinder" runat="server" >
           
           
                 <div class="bokindetalle" style=" overflow-y: scroll;">
                 <table id="tabla" cellspacing="1" cellpadding="1" class="table table-bordered invoice">
                 <thead>
                 <tr>
                 <th >Referencia</th>
                 <th >Nave</th>
                 <th >Acciones</th>
                 </tr>
             
                 </thead> 
                     <tbody>
                             <tr>
                  <td colspan="4" style=" width:99%">
                    <input id="buscarref"  style=" width:99%" type="text" class="form-control" placeholder="Nave o Referencia" />
                  </td>
                 </tr>
                     </tbody>
                 </table>
                 </div>
                 <div class="bokindetalle" style=" overflow:scroll; height:100px">
                 <asp:Repeater ID="tablePaginationReferencias" runat="server" >
                 <HeaderTemplate>
                 <table id="tablasort" cellspacing="1" cellpadding="1" class="table table-bordered invoice">
                 <thead>
                 </thead> 
                 <tbody>
                 </HeaderTemplate>
                 <ItemTemplate>
                 <tr class="point" onclick="setObjectRef(this);">
                  <td ><%#Eval("id")%></td>
                  <td ><%#Eval("name")%></td>
             
                  <td >
                     <a href="#"  class="btn btn-link" >Elegir</a>
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
                
                     </Triggers>
                 </asp:UpdatePanel>
             </div>
		   </div>
		  </div>

           <div class="form-row">
		  
		   <div class="form-group col-md-6"> 
		   	  <label for="inputAddress">Referencia<span style="color: #FF0000; font-weight: bold;"></span></label>
			         <div class="d-flex">         
               <span id="referencia" runat="server" clientidmode="Static" class=" form-control col-md-12">...</span>
                   <input id="xreferencia" type="hidden" value="" runat="server" clientidmode="Static"/>
                <span id='valreferencia' class="validacion" > * </span>

			         </div>
		   </div>

                <div class="form-group col-md-6"> 
		   	  <label for="inputAddress">Nave<span style="color: #FF0000; font-weight: bold;"></span></label>
			  <div class="d-flex">
                                     <span id="nave" runat="server" clientidmode="Static" class="form-control col-md-12" >...</span>
                   <input id="xnave" type="hidden" value="" runat="server" clientidmode="Static"/>
          <span id='valnave' class="validacion" > * </span>


			  </div>
		   </div>
		  </div>

           <div class="form-row">
		  
		   <div class="form-group col-md-12"> 
		   	  <label for="inputAddress">Buscar Cliente<span style="color: #FF0000; font-weight: bold;"></span></label>
			  <div class="d-flex">
                        <asp:TextBox 
                    style="text-align: center"
                    ID="txtrucbuscar" runat="server"  
                       MaxLength="13" placeholder="RUC" CssClass= " form-control"
                    onkeypress="return soloLetras(event,'abcdefghijklmnñopqrstuvwxyz0123456789')" 
                    ></asp:TextBox>
                               <img alt="loading.." src="../shared/imgs/loader.gif" id="loader3" class="nover"  />
                  <asp:Button ID="btnBuscar" runat="server"  CssClass="btn btn-primary"
                     Text="Buscar" OnClientClick="return prepareObjectRuc()"
                   ToolTip="Busca al Cliente por el RUC." OnClick="btnBuscar_Click"/>

			  </div>
		   </div>
		  </div>


            <div class="form-row">
		  
		   <div class="form-group col-md-12"> 
		   	  
                              <asp:UpdatePanel ID="UpdatePanel1" runat="server" ChildrenAsTriggers="true">
                     <ContentTemplate>
                       <input id="Hidden1" type="hidden" runat="server" clientidmode="Static" />
                       <input id="Hidden2" type="hidden" runat="server" clientidmode="Static" />

                  <div id="Div1" runat="server" >
            
             <div class="booking" >

                 <div class="bokindetalle" style=" overflow:scroll;  max-height:150px">
                 <asp:Repeater ID="tablePaginationClientes" runat="server" >
                 <HeaderTemplate>
                 <table id="tablasortcli" cellspacing="1" cellpadding="1" class="table table-bordered invoice">
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
                  <td ><%#Eval("CLNT_CUSTOMER")%></td>
                  <td><%#Eval("CLNT_NAME")%></td>
                  <td class="xax" ><%#Eval("EMAIL")%></td>
                  <td >
                     <a href="#" class="btn btn-link" >Elegir</a>
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
                  </ContentTemplate>
                     <Triggers>
                         <asp:AsyncPostBackTrigger ControlID="btnBuscar" />
                     </Triggers>
                 </asp:UpdatePanel>

             
		   </div>
		  </div>

           <div class="form-row">
		  
		   <div class="form-group col-md-6"> 
		   	  <label for="inputAddress">Código SAP<span style="color: #FF0000; font-weight: bold;"></span></label>
	                   
               <div class="d-flex">
               <span id="codigosap" runat="server" clientidmode="Static" class="form-control col-md-12" style=" width:280px!important;">...</span>
                   <input id="xcodigosap" type="hidden" value="" runat="server" clientidmode="Static"/>
<span id='valcodsap' class="validacion" > * </span>
		   </div>
               </div>
                 <div class="form-group col-md-6"> 
		   	  <label for="inputAddress">Ruc<span style="color: #FF0000; font-weight: bold;"></span></label>
		           <div class="d-flex">   
                     
                     <span id="ruc" runat="server" clientidmode="Static" class="form-control col-md-12" style=" width:280px!important;">...</span>
                   <input id="xruc" type="hidden" value="" runat="server" clientidmode="Static"/>
          <span id='valrucasume' class="validacion" > * </span>
		   </div>

                 </div>
		  </div>

          <div class="form-row">
		   <div class="form-group col-md-6">
            

		   	  <label for="inputAddress">Razón social<span style="color: #FF0000; font-weight: bold;"></span></label>
                      <div class="d-flex">              
                   
                   <span id="razonsocial" runat="server" clientidmode="Static" class="form-control col-md-12" >...</span>
                   <input id="xrazonsocial" type="hidden" value="" runat="server" clientidmode="Static"/>
         <span id='valrazsoc' class="validacion" > * </span>
	</div>
		   </div>
                 
               <div class="form-group col-md-6"> 
		   	  <label for="inputAddress">Email<span style="color: #FF0000; font-weight: bold;"></span></label>
		                 
                      <div class="d-flex">
                     <span id="email" runat="server" clientidmode="Static" class="form-control col-md-12" >...</span>
                   <input id="xemail" type="hidden" value="" runat="server" clientidmode="Static"/>
         <span id='valemail' class="validacion" > * </span>
                          </div>
		   </div>
              </div>
           <div class="form-row">
		   <div class="form-group col-md-12"> 
		   	  
               
               <label for="inputAddress">Booking<span style="color: #FF0000; font-weight: bold;"></span></label>

<div class="d-flex">
			  
                  <span id="numbook" class="form-control col-md-11" onclick="clear();">...</span>
                   <input id="nbrboo" type="hidden" value="" runat="server" clientidmode="Static"/>
                <a  class="btn btn-outline-primary mr-4" target="popup" onclick="openPopup()" >
        
          <span class='fa fa-search' style='font-size:24px'></span> 

        </a>

			  </div>
		   </div>
 
               </div>

	

         	   <div class="form-row">
		   <div class="col-md-12 d-flex justify-content-center" runat="server" id="div3"> 
                <div class="d-flex">

                     <img alt="loading.." src="../shared/imgs/loader.gif" id="Img1" class="nover"  />
                 <asp:Button ID="btnAsumirBook" runat="server" Text="Agregar Booking" CssClass="btn btn-primary"
                   ToolTip="Agrega el Booking seleccionado y lo agrega a una lista para luego asumir toda la lista." 
                     OnClick="btnAsumirBook_Click"/>
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
                 <%--<th class="xax" >Booking</th>--%>
                 <th style=" width:120px">Booking</th>
                 <%--<th>Razon Social</th>--%>
                 <th></th>
                 <%--<th></th>--%>
                 </tr>
                 </thead> 
                 <tbody>
                 </HeaderTemplate>
                 <ItemTemplate>
                  <tr class="point" onclick="setObjectCliente(this);">
                  <%--<td  class="xax" ><%#Eval("CODIGO_SAP")%></td>--%>
                  <td style=" width:120px"><%#Eval("BOOKING")%></td>
                  <%--<td><%#Eval("CLNT_NAME")%></td>--%>
                  <td style=" width:50px">
                      <asp:Button CssClass="btn btn-primary" Text="Remover" ID="btnRemoveBook" OnClick="btnRemove_Click" CommandName="Delete" runat="server"/>

                  </td>
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
		   <div class="col-md-12 d-flex justify-content-center" runat="server" id="divAsumirCliente"> 
		    <div class="d-flex">
               <img alt="loading.." src="../shared/imgs/loader.gif" id="loader2" class="nover"  />
                 <asp:Button ID="btnAsumir" runat="server" Text="Asumir Cliente" CssClass="btn btn-primary"
                   OnClientClick="return prepareObjectTable('¿Esta seguro de asumir el Pago del Cliente.?')"
                   ToolTip="Asume al Cliente seleccionado y lo agrega a una lista para luego procesar el Pago a Terceros." OnClick="btnAsumir_Click"/>
           </div> 
               </div>
		   </div>

 <div class="form-row">
		  
		   <div class="form-group col-md-12"> 
		         <div class="cataresult" >
               <asp:UpdatePanel ID="UpdatePanel2" runat="server" ChildrenAsTriggers="true">
                 <ContentTemplate> 
                 <div id="Div2" runat="server" >
                 <div class="findresult" >
                 <div class="booking" >
                 <div class="bokindetalle" style=" overflow-y: scroll;">                 
                 <asp:Repeater ID="RepeaterAsume" runat="server" OnItemCommand="RepeaterAsume_ItemCommand" >
                 <HeaderTemplate>
                 <table id="tabLeasume" cellspacing="1" cellpadding="1" class="table table-bordered invoice">
                 <thead>
                    <th>Referencia</th>
                     <th>Nave</th>
                     <th>Booking</th>
                    <th class="xax" >Código SAP</th>
                    <th>Ruc</th>
                    <th>Razon Social</th>
                    <th style=" display:none">Tipo</th>
                    <th></th>
                 </thead> 
                 <tbody>
                 </HeaderTemplate>
                 <ItemTemplate>
                 <tr class="point">
                  <td ><asp:Label Text='<%#Eval("REFERENCIA")%>' ID="lbReferencia" runat="server" /></td>
                     <td ><asp:Label Text='<%#Eval("NAVE")%>' ID="lblNave" runat="server" /></td>
                     <td ><asp:Label Text='<%#Eval("BOOKING")%>' ID="Label1" runat="server" /></td>
                  <td class="xax" ><asp:Label Text='<%#Eval("CODIGOSAP")%>' ID="lbCodigoSap" runat="server" /></td>
                  <td ><asp:Label Text='<%#Eval("RUC")%>' ID="lbRuc" runat="server" /></td>
                  <td ><asp:Label Text='<%#Eval("RAZSOCIAL")%>' ID="lbRazSocial" runat="server" /></td>
                  <td style=" display:none"><%#Eval("TIPO")%></td>
                  <td ><asp:Button Text="Remover" CssClass="btn btn-secondary" ID="btnRemove" OnClick="btnRemove_Click" OnClientClick="return prepareObjectTableRemove('¿Esta seguro de remover el Cliente que Asumio?')" CommandName="Delete" runat="server"/></td>
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
              <asp:Button ID="btsalvar" CssClass="btn btn-primary" runat="server" Text="Procesar Pago a Terceros"  onclick="btsalvar_Click"
                   OnClientClick="return prepareObject('¿Estimado Cliente, está seguro de procesar el pago a Terceros?')"
                   ToolTip="Procesar Pago a Terceros."/>

               </div>
		   </div> 
		   </div>

          <div class="form-row">
		   <div class="col-md-12 d-flex justify-content-center"> 
                    <div class="alert-warning" >
        Estimado Cliente;
        Al asumir valores de un tercero, asume todos los valores de la carga de un cliente en particular, incluyendo los servicios de Inspecciones, roleos, almacenajes, late arrival y valores adicionales que pudiesen
        generarse antes del embarque de la carga; considerar que no podrá asumir valores parciales.
        </div>
		   </div> 
		   </div>
     </div>




    <asp:HiddenField runat="server" ID="hfRucBuscar" />
    <script src="../Scripts/pages.js" type="text/javascript"></script>
    <script src="../Scripts/credenciales.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.tablesorter.min.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.tablesorter.pager.js" type="text/javascript"></script>

    <script type="text/javascript">

        document.querySelector("#buscarref").onkeyup = function () {
            $TableFilter("#tablasort", this.value);
        }

        //document.querySelector("#buscarcli").onkeyup = function () {
        //    $TableFilter("#tablasortcli", this.value);
        //}

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
            document.getElementById('loader3').className = 'nover';

            document.querySelector("#buscarref").onkeyup = function () {
                $TableFilter("#tablasort", this.value);
            }

            document.getElementById('buscarref').value = document.getElementById('<%=hfRucBuscar.ClientID %>').value;
            $TableFilter("#tablasort", document.getElementById('buscarref').value);

            return true;
        }
        function getGifOculta() {
            document.getElementById('loader2').className = 'nover';
            
            //document.querySelector("#buscarcli").onkeyup = function () {
            //    $TableFilter("#tablasortcli", this.value);
            //}
            
            document.getElementById('referencia').textContent = "";
            document.getElementById('xreferencia').value = "";
            document.getElementById('codigosap').textContent = "";
            document.getElementById('xcodigosap').value = "";
            document.getElementById('ruc').textContent = "";
            document.getElementById('xruc').value = "";
            document.getElementById('razonsocial').textContent = "";
            document.getElementById('xrazonsocial').value = "";
            document.getElementById('nave').textContent = "";
            document.getElementById('xnave').value = "";
            document.getElementById('email').textContent = "";
            document.getElementById('xemail').value = "";
            document.getElementById('numbook').textContent = "";
            document.getElementById('nbrboo').value = "";

            document.getElementById('valreferencia').textContent = " * obligatorio";
            document.getElementById('valnave').textContent = " * obligatorio";
            document.getElementById('valcodsap').textContent = " * obligatorio";
            document.getElementById('valrucasume').textContent = " * obligatorio";
            document.getElementById('valrazsoc').textContent = " * obligatorio";
            document.getElementById('valemail').textContent = " * obligatorio";

            document.getElementById('<%=txtrucbuscar.ClientID %>').value = "";

            document.querySelector("#buscarref").onkeyup = function () {
                $TableFilter("#tablasort", this.value);
            }

            return true;
        }

        function getGifOcultaEnviar(mensaje) {
            document.getElementById('loader').className = 'nover';
            alert(mensaje);
            return true;
        }
        function getBookOculta() {
            document.getElementById('loader2').className = 'nover';
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
                    alert('¡ Seleccione la Referencia.');
                    document.getElementById("loader2").className = 'nover';
                    document.getElementById("buscarref").focus();
                    return false;
                }
                var vals = document.getElementById('xnave');
                if (vals == null || vals == undefined || vals.value == '') {
                    alert('¡ Seleccione la Referencia.');
                    document.getElementById("loader2").className = 'nover';
                    document.getElementById("buscarref").focus();
                    return false;
                }
                var vals = document.getElementById('xcodigosap');
                if (vals == null || vals == undefined || vals.value == '') {
                    alert('¡ Seleccione el Cliente que desea Asumir el Pago.');
                    document.getElementById("loader2").className = 'nover';
                    document.getElementById('<%=txtrucbuscar.ClientID %>').focus();
                    return false;
                }
                var vals = document.getElementById('xruc');
                if (vals == null || vals == undefined || vals.value == '') {
                    alert('¡ Seleccione el Cliente que desea Asumir el Pago.');
                    document.getElementById('<%=txtrucbuscar.ClientID %>').className = 'nover';
                    return false;
                }
                var vals = document.getElementById('xrazonsocial');
                if (vals == null || vals == undefined || vals.value == '') {
                    alert('¡ Seleccione el Cliente que desea Asumir el Pago.');
                    document.getElementById("loader2").className = 'nover';
                    document.getElementById('<%=txtrucbuscar.ClientID %>').focus();
                    return false;
                }
                var vals = document.getElementById('xemail');
                if (vals == null || vals == undefined || vals.value == '') {
                    var mensajeerror = '¡ El Cliente: ' + document.getElementById('xruc').value + ' - ' + document.getElementById('xrazonsocial').value + ', no tiene un Email registrado.'
                    alert(mensajeerror);
                    document.getElementById("loader2").className = 'nover';
                    document.getElementById('<%=txtrucbuscar.ClientID %>').focus();
                    return false;
                }
                var vals = document.getElementById('nbrboo');
                if (vals == null || vals == undefined || vals.value.trim().length <= 2) {
                    alert('¡ Seleccione el numero de Booking que pertenece al Cliente.');
                    document.getElementById("loader2").className = 'nover';
                    return false;
                }
                return true;
            } catch (e) {
                alert('Lamentamos comunicarle que ha ocurrido el siguiente problema:\n' + e.Message);
            }
        }

        function prepareObjectTableRemove(mensaje) {

            try {
                if (confirm(mensaje) == false) {
                    return false;
                }
                return true;
            } catch (e) {
                alert('Lamentamos comunicarle que ha ocurrido el siguiente problema:\n' + e.Message);
            }
        }

        function prepareObjectRuc() {
            try {
                document.getElementById("loader3").className = '';
                var vals = document.getElementById('<%=txtrucbuscar.ClientID %>').value;
                if (vals == null || vals == undefined || vals == '') {
                    alert('¡ Escriba el Ruc del Cliente a buscar.');
                    document.getElementById("loader3").className = 'nover';
                    document.getElementById('<%=txtrucbuscar.ClientID %>').focus();
                    return false;
                }
                return true;
            } catch (e) {
                alert('Lamentamos comunicarle que ha ocurrido el siguiente problema:\n' + e.Message);
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
                alert('¡ Hubo un problema al setear un objeto de catalogo.');
                return;
            }
            document.getElementById('referencia').textContent = objeto.referencia;
            document.getElementById('xreferencia').value = objeto.referencia;
            document.getElementById('nave').textContent = objeto.nave;
            document.getElementById('xnave').value = objeto.nave;
            document.getElementById('valreferencia').textContent = "";
            document.getElementById('valnave').textContent = "";
            document.getElementById('<%=hfRucBuscar.ClientID %>').value = document.getElementById('buscarref').value ;
            return;
        }

        function popupCallbackCliente(objeto) {
            if (objeto == null || objeto == undefined) {
                alert('¡ Hubo un problema al setear un objeto de catalogo.');
                return;
            }
            document.getElementById('codigosap').textContent = objeto.codigosap;
            document.getElementById('xcodigosap').value = objeto.codigosap;
            document.getElementById('ruc').textContent = objeto.ruc;
            document.getElementById('xruc').value = objeto.ruc;
            document.getElementById('razonsocial').textContent = objeto.razonsocial;
            document.getElementById('xrazonsocial').value = objeto.razonsocial;
            document.getElementById('email').textContent = objeto.email;
            document.getElementById('xemail').value = objeto.email;
            document.getElementById('valcodsap').textContent = "";
            document.getElementById('valrucasume').textContent = "";
            document.getElementById('valrazsoc').textContent = "";
            document.getElementById('valemail').textContent = "";
            return;
        }

        function popupCallbackBook(objeto) {
            if (objeto == null || objeto == undefined) {
                alert('¡ Hubo un problema al setear un objeto de catalogo.');
                return;
            }
            document.getElementById('numbook').textContent = objeto.nbr;
            document.getElementById('nbrboo').value = objeto.nbr;
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

        function setObjectCliente(row) {
            var celColect = row.getElementsByTagName('td');
            var catalogo = {
                codigosap: celColect[0].textContent,
                ruc: celColect[1].textContent,
                razonsocial: celColect[2].textContent,
                email: celColect[3].textContent
            };
            popupCallbackCliente(catalogo);
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
                return;
        }
        function openPopup() {
        var ref = document.getElementById('xreferencia').value;
        var ruc = document.getElementById('xruc').value;
            window.open('../catalogo/bookinListRef.aspx?' + 'ref=' + ref + '&ruc=' + ruc, 'name', 'width=850,height=880');
            //window.location = '../facturacion/emision-pase-de-puerta';
            return true;
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
