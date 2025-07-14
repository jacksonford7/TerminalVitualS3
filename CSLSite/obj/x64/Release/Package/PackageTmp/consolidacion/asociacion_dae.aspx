<%@ Page Title="Asignación DAE" Language="C#" MasterPageFile="~/site.Master" AutoEventWireup="true" CodeBehind="asociacion_dae.aspx.cs" Inherits="CSLSite.asignacion_dae" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="placehead" runat="server">

     <!--Datatables-->
       <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/5.15.1/css/all.min.css" integrity="sha512-+4zCK9k+qNFUR5X+cKL9EIR+ZOhtIloNl9GIKS57V1MyNsYpYcUrUeQc9vNfzsWfV28IaLL3i96P9sdNyeRssA==" crossorigin="anonymous" />
       <link href="../css/datatables.min.css" rel="stylesheet" />
       <link rel="stylesheet" type="text/css" href="..js/buttons/css1.6.4/buttons.dataTables.min.css"/>
        <script src="../lib/jquery/jquery.min.js" type="text/javascript"></script>
       <script type="text/javascript"  src="../js/datatables.js"></script>
       <script src="../Scripts/table_catalog.js" type="text/javascript"></script>  
       <script type="text/javascript" src="../js/bootstrap.bundle.min.js"></script>
    <!--Fin-->


    <script src="../Scripts/turnos.js" type="text/javascript"></script>
    <script src="../Scripts/credenciales.js" type="text/javascript"></script>
    <link href="../shared/estilo/jquery.datetimepicker.css" rel="stylesheet" type="text/css" />
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

    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="placebody" runat="server">
    <asp:ToolkitScriptManager ID="tkcal" EnableScriptGlobalization="true" runat="server"> </asp:ToolkitScriptManager>

      <div class="mt-4">         
        <nav class="mt-4" aria-label="breadcrumb">
          <ol class="breadcrumb">
            <li class="breadcrumb-item" id="opcion_principal" runat="server"><a href="#">Exportación</a></li>
             <li class="breadcrumb-item active" aria-current="page" id="sub_opcion" runat="server">Asignación DAE</li>
          </ol>
        </nav>
      </div>


    <div class="dashboard-container p-4" id="cuerpo" runat="server">
        <input id="agencia" type="hidden" runat="server"  clientidmode="Static"/>

        <div class="form-title">Datos para asignación</div>
		  <div class="form-row">
		   <div class="col-md-12 d-flex justify-content-center"> 
                 <div class=" alert-danger" >
                Estimado Cliente;
                El Booking a consultar debe ser para consolidación (LCL) y la nave debe encontrarse activa, para cualquier consulta adicional favor contactar a la casilla:
                <a href="mailto:AfterDock@cgsa.com.ec">AfterDock@cgsa.com.ec</a> Ext:4043
                </div>
		   </div> 
		   </div>
		  <div class="form-row">
		   <div class="form-group col-md-5"> 
		   	  <label for="inputAddress">Dae:<span style="color: #FF0000; font-weight: bold;"></span></label>
			  <div class="d-flex">
                     <asp:TextBox 
                    onblur="checkcaja(this,'valdae',true)" CssClass="form-control"
                    ID="txtDAE" runat="server"   MaxLength="40" 
                         onkeyup="datosReplace()"
                    onkeypress="return soloLetras(event,'0123456789')" 
                    ></asp:TextBox>
                                   <span id="valdae" class="validacion"> * </span>

			  </div>
		   </div>
		   <div class="form-group col-md-5"> 
		   	  <label for="inputAddress">Booking<span style="color: #FF0000; font-weight: bold;"></span></label>
			  <div class="d-flex">
                    <asp:TextBox 
                    style="text-align: center"   CssClass="form-control"
                       onblur="checkcaja(this,'valbkg',true);"  
                       onkeyup="datosReplaceBkg()"
                    ID="txtbkg" runat="server"   MaxLength="50"
                    onkeypress="return soloLetras(event,'abcdefghijklmnñopqrstuvwxyz0123456789-_/')" 
                    ></asp:TextBox>
                                   <span id="valbkg" class="validacion"> * </span>


			  </div>
		   </div>
		   <div class="form-group col-md-2"> 
		   	  <label for="inputAddress">&nbsp;<span style="color: #FF0000; font-weight: bold;"></span></label>
			          <div class="d-flex">

                     <asp:Button ID="btnBuscar" 
                         runat="server" Text="Buscar Booking"
                       OnClientClick="return prepareObjectTable();" 
                         CssClass="btn btn-primary"
                       ToolTip="Busca los contenedores asociados al Booking." 
                         OnClick="btnBuscar_Click"/>

                      <img alt="loading.." src="../shared/imgs/loader.gif" id="loader2" class="nover"  />

                 </div>
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server" ChildrenAsTriggers="true">
                     <ContentTemplate>
                       <input id="Hidden1" type="hidden" runat="server" clientidmode="Static" />
                       <input id="Hidden2" type="hidden" runat="server" clientidmode="Static" />
                  </ContentTemplate>
                     <Triggers>
                         <asp:AsyncPostBackTrigger ControlID="btnBuscar" />
                     </Triggers>
                 </asp:UpdatePanel>
		   </div>
		


         

           </div>
          <div class="form-row">
              <div class="form-group col-md-4 ">  
                   <asp:UpdatePanel ID="UPFECHA" runat="server"  UpdateMode="Conditional" ChildrenAsTriggers="true">
                        <ContentTemplate>
                            <label class="checkbox-container">
                            <asp:CheckBox ID="ChkTodos" runat="server"  Text=""  OnCheckedChanged="ChkTodos_CheckedChanged"  AutoPostBack="True" />
                                <span class="checkmark"></span>
                               <label class="form-check-label" for="inlineCheckbox1">Marcar todas unidades, para el servicio de Carbono Neutro</label>  
                             </label>
                            </ContentTemplate> 
                        <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="ChkTodos" />
                       </Triggers>
                      </asp:UpdatePanel> 
              </div>
              <div class="form-group col-md-4 ">  

              </div>
              <div class="form-group col-md-4 ">  

              </div>
          </div>
          <div class="form-row">
		   <div class="form-group col-md-12 "> 
               <asp:UpdatePanel ID="UpdatePanel0" runat="server" ChildrenAsTriggers="true">
                     <ContentTemplate>
                            <script type="text/javascript">
                            Sys.Application.add_load(BindFunctions); 
                      </script>

                       <input id="idlin" type="hidden" runat="server" clientidmode="Static" />
                       <input id="diponible" type="hidden" runat="server" clientidmode="Static" />

                  <div id="xfinder" runat="server" >
              

               
                 <asp:Repeater ID="tablePaginationBkg" runat="server"   onitemdatabound="tablePaginationBkg_ItemDataBound" >
                 <HeaderTemplate>
                 <table id="tablasort" cellspacing="1" cellpadding="1" class="table table-bordered table-sm table-contecon">
                  <thead>
                 <tr>
                 <th style="display:none">Gkey</th>
                 <th>Booking</th>
                 <th >Contenedor</th>
                 <th style="display:none">Referencia</th>
                 <th >Acciones</th>
                 <th >	
                    <asp:Image ID="ImgCarbono" runat="server"  />
                    <asp:LinkButton ID="LinkCarbono" runat="server" />

                 </th>
                 <th style="display:none">tiene servicio</th>
                 </tr>
                 </thead>
                 <tbody>
                 </HeaderTemplate>
                 <ItemTemplate>
                 <tr class="point">
                  <td style="display:none"><asp:Label Text='<%#Eval("gkey")%>' ID="lblGkey" runat="server" /></td>
                  <td ><asp:Label Text='<%#Eval("BOOKING")%>' ID="lblBkg" runat="server" /></td>
                  <td ><asp:Label Text='<%#Eval("CONTENEDOR")%>' ID="lblCntr" runat="server" /></td>
                  <td style=" display:none" title='<%#Eval("NAVE")%>'><asp:Label Text='<%#Eval("REFERENCIA")%>' ID="lblReferencia" runat="server" /></td>
                  <td class="center hidden-phone">
                    <%-- <label class="checkbox-container">--%>
                     &nbsp;<asp:CheckBox  runat="server" Text="&nbsp;ASIGNAR CONTENEDOR A DAE" Checked='<%#Eval("ELEGIR")%>' ID="chkElegir" />
                  <%--   <span class="checkmark"></span>--%>
                  </td>
                   <td class="center hidden-phone">
                     <%--<label class="checkbox-container">--%>
                   &nbsp; <asp:CheckBox  runat="server" Text="&nbsp;CERTIFICADO" Checked="false" ID="CHKCERTIFICADO" />
                   <%--   <span class="checkmark"></span>--%>
                  </td>
                     <td style=" display:none"><asp:Label  ID="LblTiene" runat="server" /></td>
                 </tr>
                 </ItemTemplate>
                 <FooterTemplate>
                 </tbody>
                 </table>
                 </FooterTemplate>
         </asp:Repeater>
                
                 <div runat="server" id="divnotificacion" style=" font-weight:bold"></div>
           
             </div>
                  </ContentTemplate>
                     <Triggers>
                         <asp:AsyncPostBackTrigger ControlID="btsalvar" />
                     </Triggers>
                 </asp:UpdatePanel>
             
               </div>

             </div>
          <div class="form-row" runat="server" id="btnera">
         
              
             <div class="form-group col-md-12">
                 <label style="color:black;" >Correo Electrónico (Por favor debe ingresar por lo menos una dirección de correo, ya que las mismas serán utilizadas para las notificaciones de la policía antinarcóticos):<span style="color: #FF0000; font-weight: bold;">*</span></label>
                 <asp:TextBox runat="server" id='tmailinfocli' name='textboxmailinfocli' class="form-control" placeholder="mail@mail.com" 
                    onkeypress="return soloLetras(event,'abcdefghijklmnñopqrstuvwxyz1234567890_$@.-',true)"  MaxLength="100"  onblur="maildata(this,'valmailz');"/>
                 <span id="valmailz" class="validacion" ></span>
             </div>
              <div class="form-group col-md-12">
                   <asp:TextBox runat="server" id='Txtmail2'  class="form-control" placeholder="mail@mail.com" 
                    onkeypress="return soloLetras(event,'abcdefghijklmnñopqrstuvwxyz1234567890_$@.-',true)"  MaxLength="100"  onblur="maildata(this,'valmailz2');"/>
                 <span id="valmailz2" class="validacion" ></span>
               </div>
               <div class="form-group col-md-12">
                   <asp:TextBox runat="server" id='Txtmail3'  class="form-control" placeholder="mail@mail.com" 
                    onkeypress="return soloLetras(event,'abcdefghijklmnñopqrstuvwxyz1234567890_$@.-',true)"  MaxLength="100"  onblur="maildata(this,'valmailz3');"/>
                 <span id="valmailz3" class="validacion" ></span>
               </div>
               <div class="form-group col-md-12">
                   <asp:TextBox runat="server" id='Txtmail4'  class="form-control" placeholder="mail@mail.com" 
                    onkeypress="return soloLetras(event,'abcdefghijklmnñopqrstuvwxyz1234567890_$@.-',true)"  MaxLength="100"  onblur="maildata(this,'valmailz4');"/>
                 <span id="valmailz4" class="validacion" ></span>
               </div>
              <div class="form-group col-md-12">
                   <asp:TextBox runat="server" id='Txtmail5'  class="form-control" placeholder="mail@mail.com" 
                    onkeypress="return soloLetras(event,'abcdefghijklmnñopqrstuvwxyz1234567890_$@.-',true)"  MaxLength="100"  onblur="maildata(this,'valmailz5');"/>
                 <span id="valmailz5" class="validacion" ></span>
               </div>

               <div class="form-group col-md-6 d-flex justify-content-center"> 
			
                            <asp:Button ID="Button1" runat="server" 
                                Text="Reporte Asignaciones DAE"
                       OnClientClick="return openPop()" CssClass="btn btn-primary"
                       ToolTip="Buscar Asignaciones DAE."/>

		
		   </div>

                    <div class="form-group col-md-6 d-flex justify-content-center"> 
			
                                      <asp:Button ID="btsalvar"  
                                          CssClass="btn btn-primary"
                         runat="server" Text="Procesar Asignación DAE"  
                                          onclick="btsalvar_Click"
                   OnClientClick="return prepareObject('¿Estimado Cliente está seguro de asociar estos contenedor(es) a la DAE?')"
                   ToolTip="Procesar Asignación DAE."/>
                                                     <img alt="loading.." src="../shared/imgs/loader.gif" id="loader" class="nover" height="32px" width="32px"   />
		   </div>
         </div>
     </div>
    <asp:HiddenField runat="server" id="hfRucUser" ClientIDMode="Static" />
    
    
    <script src="../Scripts/pages.js" type="text/javascript"></script>
    <script src="../Scripts/credenciales.js" type="text/javascript"></script>
     <script src="../Scripts/jquery.datetimepicker.js" type="text/javascript"></script>
    <script type="text/javascript">
        var ced_count = 0;
        var jAisv = {};
         var programacion = {};
        //var lista = [];
        $(document).ready(function () {
            $('.datetimepicker').datetimepicker().datetimepicker({ lang: 'es', timepicker: false, closeOnDateSelect: true, format: 'd/m/Y' });
            $('.datepicker').datetimepicker().datetimepicker({ lang: 'es', timepicker: false, format: 'd/m/Y', closeOnDateSelect: true, minDate: '0' });
        });


        function datosReplace() {
           
            var tableReg = document.getElementById('tablasort');
            var cellsOfRow = "";
            // Recorremos todas las filas con contenido de la tabla
            for (var i = 0; i < tableReg.rows.length; i++) {
                cellsOfRow = tableReg.rows[i].getElementsByTagName('td');
                var x = cellsOfRow[4].getElementsByTagName('label')[0];
                x.checked = false;
                x.textContent = "Asignar Contenedor a DAE" + " [" + document.getElementById('<%=txtDAE.ClientID %>').value + "]";
            }
        }
        function datosReplaceBkg() {
           
            var tableReg = document.getElementById('tablasort');
            var cellsOfRow = "";
            // Recorremos todas las filas con contenido de la tabla
            for (var i = 0; i < tableReg.rows.length; i++) {
                cellsOfRow = tableReg.rows[i].getElementsByTagName('td');
                var x = cellsOfRow[1];
                x.textContent = document.getElementById('<%=txtbkg.ClientID %>').value;
            } 
        }
        function getGifOculta() {

            document.getElementById("loader2").className = 'nover';
            document.getElementById("loader").className = 'nover';

            document.querySelector("#buscarref").onkeyup = function () {
                $TableFilter("#tablasort", this.value);
            }
           
            datosReplace();
            datosReplaceBkg();
            return true;
        }
        function prepareObjectTable() {
           
            try {

                var rr = document.getElementById('hfRucUser');
               
                var sms = "El ruc del booking no coincide con el exportador'";
                 if (rr) {
                     sms = "El ruc del Exportador " +rr.value + " no coincide con el booking";
                }
                document.getElementById("loader2").className = '';
                document.getElementById("loader").className = 'nover';

                //alertify.alert(sms);

                //lista = [];
                var vals = document.getElementById('<%=txtbkg.ClientID %>');
                if (vals == null || vals == undefined || vals.value == '') {
                    alertify.alert('¡Escriba el Booking.');
                    document.getElementById("loader2").className = 'nover';
                    document.getElementById('<%=txtbkg.ClientID %>').focus();
                    return false;
                }
                if (!validaBooking()) {
                    alertify.alert(sms);
                    return false;
                }
                return true;
            } catch (e) {
                alertify.alert('Lamentamos comunicarle que ha ocurrido el siguiente problema:\n' + e.Message);
            }
        }
        function prepareObject(mensaje) {
            try {
                if (confirm(mensaje) == false) {
                    return false;
                }
                document.getElementById("loader").className = '';
                //lista = [];
                //validaciones básicas
                var vals = document.getElementById('<%=txtDAE.ClientID %>');
                if (vals == null || vals == undefined || vals.value == '') {
                    alertify.alert('¡ Escriba la DAE');
                    document.getElementById("loader").className = 'nover';
                    document.getElementById('<%=txtDAE.ClientID %>').focus();
                    return false;
                }
                var vals = document.getElementById('<%=txtbkg.ClientID %>');
                if (vals == null || vals == undefined || vals.value == '') {
                    alertify.alert('¡ Escriba el Booking.');
                    document.getElementById("loader").className = 'nover';
                    document.getElementById('<%=txtbkg.ClientID %>').focus();
                    return false;
                }
                var tableReg = document.getElementById('tablasort');
                if (tableReg == null || tableReg == undefined) {
                    document.getElementById("loader").className = 'nover';
                    alertify.alert('¡ No tiene Contenedores para Asignar a la DAE.');
                    return false;
                }
                <%--var valida = "0";
                var tableReg = document.getElementById('tablasort');
                var cellsOfRow = "";
               
                for (var i = 0; i < tableReg.rows.length; i++) {
                    cellsOfRow = tableReg.rows[i].getElementsByTagName('td');
                    var x = cellsOfRow[4].getElementsByTagName('input')[0];
                    if (x.checked) {
                        valida = "1";
                    }
                }
                if (valida == "0") {
                    document.getElementById("loader").className = 'nover';
                    alertify.alert('¡ No tiene ningun Contenedor para Asignar a la DAE:\n' + document.getElementById('<%=txtDAE.ClientID %>').value);
                    return false;
                }--%>
                return true;
            } catch (e) {
                alertify.alert('Lamentamos comunicarle que ha ocurrido el siguiente problema:\n' + e.Message);
            }
        }
        function myLoad(DAE) {

            alertify.alert('DAE: ' + DAE + ', Asignada exitosamente.');

            window.location = '../cuenta/menu.aspx';
            return;
        }
        function openPop() {
            window.open('../catalogo/catalogo_asignacion_dae.aspx', 'name', 'width=1300,height=880');
            return false;
        }
        function validaBooking() {
            var validametodo = false;
            $.ajax({
                type: "POST",
                url: "asociacion_dae.aspx/IsAvailableBooking",
                data: '{rucuser: "' + $("#<%=hfRucUser.ClientID%>")[0].value + '", booking: "' + $("#<%=txtbkg.ClientID%>")[0].value + '" }',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: function OnSuccess(response) {
                   
                    document.getElementById("loader2").className = 'nover';
                    if (response.d == "1") {
                        validametodo = true;
                    }
                },
                failure: function (response) {
                    alertify.alert(response.d);
                    validametodo = false;
                }
            });
            return validametodo;
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
