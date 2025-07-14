<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="emisionpasecfs.aspx.cs" Inherits="CSLSite.emisionpasecfs" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>


<asp:Content ID="Content1" ContentPlaceHolderID="placehead" runat="server">

  <link href="../img/favicon2.png" rel="icon"/>
  <link href="../img/icono.png" rel="apple-touch-icon"/>
 <!-- Bootstrap core CSS -->
  <link href="../css/bootstrap.min.css" rel="stylesheet"/>
  <link href="../css/dashboard.css" rel="stylesheet"/>
  <link href="../css/icons.css" rel="stylesheet"/>
  <link href="../css/style.css" rel="stylesheet"/>

  <!--external css-->
  <link href="../shared/estilo/jquery.datetimepicker.css" rel="stylesheet" type="text/css" />
  <script src="../Scripts/jquery.datetimepicker.js" type="text/javascript"></script>


<%-- <link href="../lib/advanced-datatable/css/demo_page.css" rel="stylesheet" />
  <link href="../lib/advanced-datatable/css/demo_table.css" rel="stylesheet" />
  <link rel="stylesheet" href="../lib/advanced-datatable/css/DT_bootstrap2.css" />--%>


 <link href="../css/calendario_ajax.css" rel="stylesheet"/>



<script type="text/javascript">

 function BindFunctions() {
     $(document).ready(function() {
      /*
       * Insert a 'details' column to the table
       */
      var nCloneTh = document.createElement('th');
      var nCloneTd = document.createElement('td');
      nCloneTd.innerHTML = '<img src="../lib/advanced-datatable/images/details_open.png">';
      nCloneTd.className = "center";

      $('#hidden-table-info thead tr').each(function() {

      });

      $('#hidden-table-info tbody tr').each(function() {
       
      });

      /*
       * Initialse DataTables, with no sorting on the 'details' column
       */
      var oTable = $('#hidden-table-info').dataTable({
        "aoColumnDefs": [{
          "bSortable": false,
          "aTargets": [0]
        }],
        "aaSorting": [
          [1, 'asc']
        ]
      });     
        });
    }
</script>
 

<script type="text/javascript">

 function BindFunctions_Tarja() {
     $(document).ready(function() {
      /*
       * Insert a 'details' column to the table
       */
      var nCloneTh = document.createElement('th');
      var nCloneTd = document.createElement('td');
      nCloneTd.innerHTML = '<img src="../lib/advanced-datatable/images/details_open.png">';
      nCloneTd.className = "center";

      $('#hidden-table-info-tarja thead tr').each(function() {

      });

      $('#hidden-table-info-tarja tbody tr').each(function() {
       
      });

      /*
       * Initialse DataTables, with no sorting on the 'details' column
       */
      var oTable = $('#hidden-table-info-tarja').dataTable({
        "aoColumnDefs": [{
          "bSortable": false,
          "aTargets": [0]
        }],
        "aaSorting": [
          [1, 'asc']
        ]
      });     
        });
    }

</script>

</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="placebody" runat="server" >
    
    <asp:ToolkitScriptManager ID="tkscata" runat="server"></asp:ToolkitScriptManager>

     <script type="text/javascript">
              Sys.Application.add_load(Calendario); 
            </script>  
  <div id="div_BrowserWindowName" style="visibility:hidden;">
    <asp:HiddenField ID="hf_BrowserWindowName" runat="server" />
    
  </div>

      <div class="mt-4">         
        <nav class="mt-4" aria-label="breadcrumb">
          <ol class="breadcrumb">
            <li class="breadcrumb-item" id="opcion_principal" runat="server"><a href="#">Impo Carga Suelta</a></li>
            <li class="breadcrumb-item active" aria-current="page" id="sub_opcion" runat="server">EMISIÓN E-PASS CARGA SUELTA (CFS)</li>
          </ol>
        </nav>
      </div>

<div class="dashboard-container p-4" id="cuerpo" runat="server"> 
     <div class="form-title">
           DATOS DE LA CARGA
     </div>

     <asp:UpdatePanel ID="UPCARGA" runat="server"  UpdateMode="Conditional" ChildrenAsTriggers="true">
        <ContentTemplate>
			<div class="form-row">
                <div class="form-group col-md-4">
                <label for="inputZip">MRN<span style="color: #FF0000; font-weight: bold;">*</span></label>
                <asp:TextBox ID="TXTMRN" runat="server" class="form-control" MaxLength="16"  onkeypress="return soloLetras(event,'abcdefghijklmnopqrstuvwxyz0123456789-_')"  
                                    placeholder="MRN"></asp:TextBox>
            </div>
                <div class="form-group col-md-2">
                    <label for="inputZip">MSN<span style="color: #FF0000; font-weight: bold;">*</span></label>
                    <asp:TextBox ID="TXTMSN" runat="server" class="form-control"  MaxLength="4"  onkeypress="return soloLetras(event,'abcdefghijklmnopqrstuvwxyz0123456789-_')" 
                                    placeholder="MSN"></asp:TextBox>
                </div>
            <div class="form-group col-md-2">
                    <label for="inputZip">HSN<span style="color: #FF0000; font-weight: bold;">*</span></label>
                    <asp:TextBox ID="TXTHSN" runat="server" class="form-control"  MaxLength="4"  onkeypress="return soloLetras(event,'abcdefghijklmnopqrstuvwxyz0123456789-_')" 
                                    placeholder="HSN"></asp:TextBox>
            </div>
            <div class="form-group col-md-2">
                    <label for="inputZip">&nbsp;</label>
                    <div class="d-flex">
                        <asp:Button ID="BtnBuscar" runat="server" class="btn btn-primary"   Text="BUSCAR"  OnClientClick="return mostrarloader('1')" OnClick="BtnBuscar_Click" />   
                            <img alt="loading.." src="../lib/file-uploader/img/loading.gif" height="32px" width="32px"  id="ImgCarga" class="nover"   />
                         
                    </div>
            </div>
     
		</div>
			<br/>
           <div class="row">
            <div class="col-md-12 d-flex justify-content-center">
                    <div class="alert alert-danger" id="banmsg" runat="server" clientidmode="Static"><b>Error!</b> >Debe ingresar el número de la carga MRN......</div>
            </div>
            </div>

                          
                     
        </ContentTemplate>
            <Triggers>
            <asp:AsyncPostBackTrigger ControlID="BtnBuscar" />
        </Triggers>
        </asp:UpdatePanel>

     <h4 class="mb">COMPAÑÍA DE TRANSPORTE</h4>    
     <div class="form-row"> 
         <div class="form-group col-md-4">
             <label for="inputZip">Desaduanamiento Directo:<span style="color: #FF0000; font-weight: bold;"></span></label>
             <asp:UpdatePanel ID="UPDESADUANAMIENTO" runat="server"  UpdateMode="Conditional" >
                           <ContentTemplate>
                            <asp:TextBox ID="TxtDesaduanamiento" runat="server" class="form-control"     
                                placeholder=""  Font-Bold="true" style="text-align:center" disabled ></asp:TextBox>
                                </ContentTemplate>
                            </asp:UpdatePanel>
         </div>
          <div class="form-group col-md-4">
             <label for="inputZip">Facturado Hasta:<span style="color: #FF0000; font-weight: bold;"></span></label>
               <asp:UpdatePanel ID="UPCAS" runat="server"  UpdateMode="Conditional" >
                           <ContentTemplate>
                            <asp:TextBox ID="TxtFechaCas" runat="server" class="form-control"    
                                placeholder=""  Font-Bold="true" style="text-align:center" disabled ></asp:TextBox>
                                </ContentTemplate>
                            </asp:UpdatePanel>
         </div>
         <div class="form-group col-md-4">
             <label for="inputZip">Total Bultos:<span style="color: #FF0000; font-weight: bold;"></span></label>
               <asp:UpdatePanel ID="UPCONTENEDOR" runat="server"  UpdateMode="Conditional" >
                           <ContentTemplate>
                            <asp:TextBox ID="TxtContenedorSeleccionado" runat="server" class="form-control"    
                                placeholder=""  Font-Bold="true" style="text-align:center" disabled ></asp:TextBox>
                                </ContentTemplate>
                            </asp:UpdatePanel>
         </div>
     </div>
     <div class="form-row"> 
         <div class="form-group col-md-2">
                 <label for="inputZip">Cia. Trans:<span style="color: #FF0000; font-weight: bold;">*</span></label>
                    <asp:TextBox ID="Txtempresa"  runat="server" class="form-control"  autocomplete="off" onkeypress="return soloLetras(event,'abcdefghijklmnñopqrstuvwxyz0123456789_')"
                        ></asp:TextBox>                      
                    <asp:HiddenField ID="IdTxtempresa" runat="server" ClientIDMode="Static"/> 
             </div>
            <div class="form-group col-md-2">
                 <label for="inputZip">Chofer: (Opcional)<span style="color: #FF0000; font-weight: bold;"></span></label>
                <asp:TextBox ID="TxtChofer"  runat="server" class="form-control"   autocomplete="off"  oncopy="return false;" onpaste="return false;"  
                                onkeypress="return soloLetras(event,'abcdefghijklmnñopqrstuvwxyz0123456789_')"></asp:TextBox>
                            <asp:HiddenField ID="IdTxtChofer" runat="server" ClientIDMode="Static"/>        
             </div>
            <div class="form-group col-md-2">
                 <label for="inputZip">Placa: (opcional)<span style="color: #FF0000; font-weight: bold;"></span></label>
                   <asp:TextBox ID="TxtPlaca"  runat="server" class="form-control"   autocomplete="off"  oncopy="return false;" onpaste="return false;"  
                        onkeypress="return soloLetras(event,'abcdefghijklmnñopqrstuvwxyz0123456789_')"></asp:TextBox>
                    <asp:HiddenField ID="IdTxtPlaca" runat="server" ClientIDMode="Static"/>       
             </div>
             <div class="form-group col-md-2">
                 <label for="inputZip">&nbsp;<span style="color: #FF0000; font-weight: bold;"></span></label>
                  <asp:UpdatePanel ID="UPDATOSCLIENTE" runat="server"  UpdateMode="Conditional" ChildrenAsTriggers="true">
                    <ContentTemplate>
                            <asp:Button ID="BtnAgregar" runat="server" class="btn btn-primary"   Text="Añadir" onclick="BtnAgregar_Click"/>      
                        </ContentTemplate>
                        <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="BtnAgregar" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
            <div class="form-group col-md-2">
                <label for="inputZip">&nbsp;<span style="color: #FF0000; font-weight: bold;"></span></label>
                <asp:UpdatePanel ID="UPTODOS" runat="server"  UpdateMode="Conditional" ChildrenAsTriggers="true">
                <ContentTemplate>
                    <label class="checkbox-container">
                    &nbsp;&nbsp;<asp:CheckBox ID="ChkTodos" runat="server"  Text="Seleccionar Todos"  OnCheckedChanged="ChkTodos_CheckedChanged"    AutoPostBack="True" />
                    <span class="checkmark"></span>
                    </label>
                </ContentTemplate> 
                <Triggers>
                <asp:AsyncPostBackTrigger ControlID="ChkTodos" />
            </Triggers>
            </asp:UpdatePanel> 
            </div>
     </div>


   <asp:UpdatePanel ID="UPTARJA" runat="server"  >  
       <ContentTemplate>

             <h3 id="LabelTotal" runat="server">DETALLE DE SUB. ÍTEMS: </h3>
            
              <div class="form-row">
                  <div class="form-group col-md-12">
                       <asp:Repeater ID="tablePagination_Tarja" runat="server"  onitemcommand="tablePagination_Tarja_ItemCommand" onitemdatabound="tablePagination_Tarja_ItemDataBound">
                       <HeaderTemplate>
                       <table cellpadding="0" cellspacing="0" border="0" class="table table-bordered invoice" id="hidden-table-info-tarja">
                           <thead>
                          <tr>
                            
                            <th class="center hidden-phone">NUMERO DE CARGA</th>
                            <th class="center hidden-phone">CÓDIGO SUB. ÍTEMS</th>
                            <th class="center hidden-phone">CANTIDAD</th>
                            <th class="center hidden-phone">CIA. TRANSPORTE</th>
                            <th class="center hidden-phone">CONDUCTOR</th>
                            <th class="center hidden-phone">PLACA</th>
                            <th class="center hidden-phone">PAGADO</th>
                            <th class="center hidden-phone">SELECCIONAR</th>
                           
                          </tr>
                        </thead>
                        <tbody>
                        </HeaderTemplate>
                        <ItemTemplate>
                           
 
                                <td class="center hidden-phone"><asp:Label Text='<%#Eval("CARGA")%>' ID="LblCarga" runat="server"  /></td>
                                <td class="center hidden-phone"><asp:Label Text='<%#Eval("CONSECUTIVO")%>' ID="LblConsecutivo" runat="server"  /></td>
                                <td class="center hidden-phone"><asp:Label Text='<%#Eval("CANTIDAD")%>' ID="LblCantidad" runat="server"  /></td>
                                <td class="center hidden-phone"><asp:Label Text='<%#Eval("CIATRANS")%>' ID="LblEmpresa" runat="server"  /> </td>
                                <td class="center hidden-phone"><asp:Label Text='<%#Eval("CHOFER")%>' ID="LblChofer" runat="server"  />  </td> 
                                <td class="center hidden-phone"><asp:Label Text='<%#Eval("PLACA")%>' ID="LblPlaca" runat="server"  /> </td> 
                                <td class="center hidden-phone"><asp:Label Text='<%#Eval("ESTADO_PAGO")%>' ID="LblEstado" runat="server"  /> </td> 
                                <td class="center hidden-phone"> 
                                    <asp:UpdatePanel ID="UPSELECCIONARTARJA" runat="server" ChildrenAsTriggers="true">
                                        <ContentTemplate>
                                             <script type="text/javascript">
                                                 Sys.Application.add_load(BindFunctions_Tarja); 
                                            </script>
                                             <label class="checkbox-container">
                                            <asp:CheckBox id="chkPaseTarja" runat="server"  Checked='<%#Eval("VISTO")%>' AutoPostBack="True" OnCheckedChanged="chkPaseTarja_CheckedChanged" />
                                             <span class="checkmark"></span>
                                             </label>
                                        </ContentTemplate>
                                         <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="chkPaseTarja" />
                                            </Triggers>
                                        </asp:UpdatePanel>
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
                    <asp:AsyncPostBackTrigger ControlID="BtnGrabar" />
                </Triggers>
            </asp:UpdatePanel>

        <h4 class="mb">DETALLE DEL TURNO</h4>

         <div class="form-row">
                <div class="form-group col-md-2">
                    <label for="inputZip">Fecha Salida:<span style="color: #FF0000; font-weight: bold;">*</span></label>         
                    <asp:UpdatePanel ID="UPFECHASALIDA" runat="server"  UpdateMode="Conditional" ChildrenAsTriggers="true" >
                    <ContentTemplate>
                            
                        <asp:TextBox runat="server" ID="TxtFechaHasta"  AutoPostBack="true" MaxLength="10" 
                                onkeypress="return soloLetras(event,'0123456789-')"    class="form-control"  
                            ontextchanged="TxtFechaHasta_TextChanged"></asp:TextBox>

                            <asp:CalendarExtender ID="CAGTFECHAFASA"  runat="server"
                                CssClass="red" Format="MM/dd/yyyy" TargetControlID="TxtFechaHasta">
                            </asp:CalendarExtender>

                    </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="TxtFechaHasta" />
                        </Triggers>
                    </asp:UpdatePanel> 
                 </div>      
                <div class="form-group col-md-2">
                    <label for="inputZip">Turnos:<span style="color: #FF0000; font-weight: bold;">*</span></label>                                 
                    <asp:UpdatePanel ID="UPTURNO" runat="server"  UpdateMode="Conditional"  >
                    <ContentTemplate>
                    <asp:DropDownList runat="server" ID="CboTurnos"  Width="150px"  AutoPostBack="false"  class="form-control"  ></asp:DropDownList>
                    </ContentTemplate>
                    </asp:UpdatePanel> 
                </div>
                 <div class="form-group col-md-2">
                      <label for="inputZip">&nbsp;<span style="color: #FF0000; font-weight: bold;">*</span></label>     
                           <asp:UpdatePanel ID="UPAGREGATURNO" runat="server"  UpdateMode="Conditional" ChildrenAsTriggers="true">
                            <ContentTemplate>
                                    <asp:Button ID="BtnAgregaTruno" runat="server" class="btn btn-primary"   Text="Agregar Turno" onclick="BtnAgregaTruno_Click"/>      
                                </ContentTemplate>
                             <Triggers>
                              <asp:AsyncPostBackTrigger ControlID="BtnAgregaTruno" />
                            </Triggers>
                        </asp:UpdatePanel>
                 </div>
        </div>
         
     
    <h3>DETALLE DE PASES A EMITIR</h3>

     <asp:UpdatePanel ID="UPDETALLE" runat="server"  >  
       <ContentTemplate>

              <div class="form-row">
                 <div class="form-group col-md-12">
                       <asp:Repeater ID="tablePagination" runat="server"  onitemcommand="tablePagination_ItemCommand" onitemdatabound="tablePagination_ItemDataBound">
                       <HeaderTemplate>
                       <table cellpadding="0" cellspacing="0" border="0" class="table table-bordered invoice" id="hidden-table-info">
                           <thead>
                          <tr>
                            <th class="center hidden-phone">NUMERO DE CARGA</th>
                            <th class="center hidden-phone">CANTIDAD</th>
                            <th class="center hidden-phone">FACTURADO <br/> HASTA</th>
                            <th class="center hidden-phone">FECHA<br/>TURNO</th>
                            <th class="center hidden-phone">H/TURNO</th>
                            <th class="center hidden-phone">CIA. TRANSPORTE</th>
                            <th class="center hidden-phone">CONDUCTOR</th>
                            <th class="center hidden-phone">PLACA</th>
                            <th class="center hidden-phone">PAGADO</th>
                            <th >ACCIONES</th>
                          </tr>
                        </thead>
                        <tbody>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr class="gradeC"> 
                                <td class="center hidden-phone"><asp:Label Text='<%#Eval("CARGA")%>' ID="LblCarga" runat="server"  /></td>
                                <td class="center hidden-phone"><asp:Label Text='<%#Eval("CANTIDAD")%>' ID="LblContenedor" runat="server"  /></td>
                                <td class="center hidden-phone"><asp:Label Text='<%#Eval("FECHA_SALIDA")%>' ID="LblFechaSalida" runat="server"  /></td>
                                <td class="center hidden-phone"><asp:Label Text='<%#DataBinder.Eval(Container.DataItem, "FECHA_SALIDA_PASE", "{0:yyyy/MM/dd}")%>' ID="LblFechaturno" runat="server"  /></td>
                                <td class="center hidden-phone"><asp:Label Text='<%#Eval("D_TURNO")%>' ID="LblTurno" runat="server"  /></td><%--D_TURNO--%>
                                <td class="center hidden-phone"><asp:Label Text='<%#Eval("CIATRANS")%>' ID="LblEmpresa" runat="server"  /> </td>
                                <td class="center hidden-phone"><asp:Label Text='<%#Eval("CHOFER")%>' ID="LblChofer" runat="server"  />  </td> 
                                <td class="center hidden-phone"><asp:Label Text='<%#Eval("PLACA")%>' ID="LblPlaca" runat="server"  /> </td> 
                                <td class="center hidden-phone"><asp:Label Text='<%#Eval("ESTADO_PAGO")%>' ID="LblEstado" runat="server"  /> </td> 
                               <td class="center hidden-phone">  
                                <asp:Button ID="BtnActualizar" CommandArgument= '<%#Eval("LLAVE")%>' runat="server" Text="Eliminar" class="btn btn-primary" ToolTip="Quitar Pase" CommandName="Eliminar" />
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
                    <asp:AsyncPostBackTrigger ControlID="BtnGrabar" />
                </Triggers>
            </asp:UpdatePanel>


      <asp:UpdatePanel ID="UPBOTONES" runat="server" UpdateMode="Conditional" >  
             <ContentTemplate>
                  <div class="row">
                    <div class="col-md-12 d-flex justify-content-center">
                             <div class="alert alert-danger" id="banmsg_det" runat="server" clientidmode="Static"><b>Error!</b>Debe ingresar el número de la carga MRN......</div>
                    </div>
                </div>

                 <br/>
                <div class="row">
                    <div class="col-md-12 d-flex justify-content-center">
                                 <img alt="loading.." src="../lib/file-uploader/img/loading.gif" height="32px" width="32px"  id="ImgCargaDet" class="nover"  />
                     </div>
                </div>
                  <div class="row">
                     <div class="col-md-12 d-flex justify-content-center">
                                      
                        <asp:Button ID="BtnGrabar" runat="server"  class="btn btn-primary"  Text="Generar e-Pass"  OnClientClick="return mostrarloader('2')" OnClick="BtnGrabar_Click" />
                    
                  </div>
                </div>
            </ContentTemplate>
             </asp:UpdatePanel>   
   

     
	    
</div> 
 

   <script type="text/javascript" src="../lib/datatables/datatables.min.js"></script>
 <%-- <script type="text/javascript" src="../lib/advanced-datatable/js/DT_bootstrap.js"></script>--%>

   <script type="text/javascript" src="../lib/common-scripts.js"></script>
 
   <script type="text/javascript" src="../lib/pages.js" ></script>

   <script type="text/javascript" src="../lib/advanced-form-components.js"></script>

 <script type="text/javascript" src='../lib/autocompletar/bootstrap3-typeahead.min.js'></script>



 
 <script type="text/javascript">

     $(function () {
        $('[id*=Txtempresa]').typeahead({
            hint: true,
            highlight: true,
            minLength: 5,
            source: function (request, response) {
                $.ajax({
                    url: '<%=ResolveUrl("emisionpasecfs.aspx/GetEmpresas") %>',
                    data: "{ 'prefix': '" + request + "'}",
                    dataType: "json",
                    type: "POST",
                    maxJsonLength: 1,
                    contentType: "application/json; charset=utf-8",
                    success: function (data) {
                        items = [];
                        map = {};
                        $.each(data.d, function (i, item) {
                            var id = item.split('+')[0];
                            var name = item.split('+')[1];
                            map[name] = { id: id, name: name };
                            items.push(name);
                        });
                        response(items);
                        $(".dropdown-menu").css("height", "auto");
                    },
                    error: function (response) {
                        alert(response.responseText);
                    },
                    failure: function (response) {
                        alert(response.responseText);
                    }
                });
            },
            updater: function (item) {
                $('[id*=IdTxtempresa]').val(map[item].id);
                //alert(map[item].id);
                //alert($("#IdTxtempresa").val());
                return item;
            }
        });
     });

</script>

<script type="text/javascript">

     $(function () {
        $('[id*=TxtChofer]').typeahead({
            hint: true,
            highlight: true,
            minLength: 5,
            source: function (request, response) {
               
                $.ajax({
                    url: '<%=ResolveUrl("emisionpasecfs.aspx/GetChofer") %>',
                    data: "{ 'prefix': '" + request + "', 'idempresa' : '" + $("#IdTxtempresa").val() + "' }",
                    dataType: "json",
                    type: "POST",
                    maxJsonLength: 1,
                    contentType: "application/json; charset=utf-8",
                    success: function (data) {
                        items = [];
                        map = {};
                        $.each(data.d, function (i, item) {
                            var id = item.split('+')[0];
                            var name = item.split('+')[1];
                            map[name] = { id: id, name: name };
                            items.push(name);
                        });
                        response(items);
                        $(".dropdown-menu").css("height", "auto");
                    },
                    error: function (response) {
                        alert(response.responseText);
                    },
                    failure: function (response) {
                        alert(response.responseText);
                    }
                });
            },
            updater: function (item) {
                $('[id*=IdTxtChofer]').val(map[item].id);
                //alert(map[item].id);
                //alert($("#IdTxtChofer").val());
                return item;
            }
        });
     });

</script>

<script type="text/javascript">

     $(function () {
        $('[id*=TxtPlaca]').typeahead({
            hint: true,
            highlight: true,
            minLength: 3,
            source: function (request, response) {
               
                $.ajax({
                    url: '<%=ResolveUrl("emisionpasecfs.aspx/GetPlaca") %>',
                    data: "{ 'prefix': '" + request + "', 'idempresa' : '" + $("#IdTxtempresa").val() + "' }",
                    dataType: "json",
                    type: "POST",
                    maxJsonLength: 1,
                    contentType: "application/json; charset=utf-8",
                    success: function (data) {
                        items = [];
                        map = {};
                        $.each(data.d, function (i, item) {
                            var id = item.split('+')[0];
                            var name = item.split('+')[1];
                            map[name] = { id: id, name: name };
                            items.push(name);
                        });
                        response(items);
                        $(".dropdown-menu").css("height", "auto");
                    },
                    error: function (response) {
                        alert(response.responseText);
                    },
                    failure: function (response) {
                        alert(response.responseText);
                    }
                });
            },
            updater: function (item) {
                $('[id*=IdTxtPlaca]').val(map[item].id);
                //alert(map[item].id);
                //alert($("#IdTxtChofer").val());
                return item;
            }
        });
     });

</script>


<script type="text/javascript">

 
    function mostrarloader(Valor) {

        try {
            if (Valor == "1") {
                document.getElementById("ImgCarga").className = 'ver';
            }
            else {
                document.getElementById("ImgCargaDet").className='ver';
            }
                
            } catch (e) {
                alert('Lamentamos comunicarle que ha ocurrido el siguiente problema:\n' + e.Message);
            }
        }

    function ocultarloader(Valor) {
        try {

            if (Valor == "1") {
                document.getElementById("ImgCarga").className = 'nover';
            }
            else {
                document.getElementById("ImgCargaDet").className='nover';
            }

             } catch (e) {
                alert('Lamentamos comunicarle que ha ocurrido el siguiente problema:\n' + e.Message);
            }
        }

</script>

 

</asp:Content>