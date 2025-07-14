<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="actualizacionpasecfs.aspx.cs" Inherits="SiteBillion.actualizacionpasecfs" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>


<asp:Content ID="Content1" ContentPlaceHolderID="placehead" runat="server">

  <link href="../img/favicon2.png" rel="icon"/>
  <link href="../img/icono.png" rel="apple-touch-icon"/>
  
  <link href="../lib/bootstrap/css/bootstrap.min.css" rel="stylesheet"/>

  <link href="../lib/font-awesome/css/font-awesome.css" rel="stylesheet" />
  <link rel="stylesheet" type="text/css" href="../lib/gritter/css/jquery.gritter.css" />


  <link href="../lib/advanced-datatable/css/demo_page.css" rel="stylesheet" />
  <link href="../lib/advanced-datatable/css/demo_table.css" rel="stylesheet" />
  <link rel="stylesheet" href="../lib/advanced-datatable/css/DT_bootstrap.css" />
   
 
  <link rel="stylesheet" href="../lib/file-uploader/css/jquery.fileupload.css"/>
  <link rel="stylesheet" href="../lib/file-uploader/css/jquery.fileupload-ui.css"/>


  <link href="../css/style.css" rel="stylesheet"/>
  <link href="../css/style-responsive.css" rel="stylesheet"/>

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
 


</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="placebody" runat="server" >
     <script type="text/javascript">
              Sys.Application.add_load(Calendario); 
            </script>  
  <div id="div_BrowserWindowName" style="visibility:hidden;">
    <asp:HiddenField ID="hf_BrowserWindowName" runat="server" />
    
  </div>

     <section id="main-content">
       <section class="wrapper site-min-height">
         <h3>ACTUALIZACIÓN E-PASS CARGA SUELTA (CFS)</h3>

       <div class="content-panel">
			<div class="form-panel">
				<h4 class="mb">DATOS DE LA CARGA</h4>   
					  <div class="form-inline">
                        <asp:UpdatePanel ID="UPCARGA" runat="server"  UpdateMode="Conditional" ChildrenAsTriggers="true">
                        <ContentTemplate>
						<div class="form-group">
                                <span class="help-block">MRN</span>
                                <asp:TextBox ID="TXTMRN" runat="server" class="form-control" MaxLength="16" Width="180px" onkeypress="return soloLetras(event,'abcdefghijklmnopqrstuvwxyz0123456789-_')"  
                                 placeholder="mrn"></asp:TextBox>
     
						</div>
						<div class="form-group">
                             <span class="help-block">MSN</span>
						    <asp:TextBox ID="TXTMSN" runat="server" class="form-control"  MaxLength="4" Width="100px" onkeypress="return soloLetras(event,'abcdefghijklmnopqrstuvwxyz0123456789-_')" 
                                placeholder="MSN"></asp:TextBox>
                            
						</div>
                          <div class="form-group">
                             <span class="help-block">HSN</span>
						    <asp:TextBox ID="TXTHSN" runat="server" class="form-control"  MaxLength="4" Width="100px" onkeypress="return soloLetras(event,'abcdefghijklmnopqrstuvwxyz0123456789-_')" 
                                placeholder="HSN"></asp:TextBox>
                            
						</div>
                         
                          <div class="form-group">
                            <span class="help-block">&nbsp;</span>
                              <asp:Button ID="BtnBuscar" runat="server" class="btn btn-theme"  Text="BUSCAR"  OnClientClick="return mostrarloader('1')" OnClick="BtnBuscar_Click" />                             
                          </div>

                        <div class="form-group">
                             <span class="help-block">&nbsp;</span>
                             <img alt="loading.." src="../lib/file-uploader/img/loading.gif" height="32px" width="32px"  id="ImgCarga" class="nover"   />
                             
                         </div>  

                          <br/>
                          <div class="form-group">
                            <div class="alert alert-danger" id="banmsg" runat="server" clientidmode="Static"><b>Error!</b> >Debe ingresar el número de la carga MRN......</div>
                           </div>
                     
                        </ContentTemplate>
                         <Triggers>
                          <asp:AsyncPostBackTrigger ControlID="BtnBuscar" />
                        </Triggers>
                        </asp:UpdatePanel>
                        
                             
					 </div><!-- form-inline-->
		    </div><!-- form-panel-->
           

         <div class="row mt">
         <div class="col-lg-12">   
		 <div class="form-panel">
			 <h4 class="mb">
                 COMPAÑÍA DE TRANSPORTE</h4>
              <div class="form-inline">
                       
                         <div class="form-group">
                           <span class="help-block">Cia. Trans:</span>
                               <asp:TextBox ID="Txtempresa"  runat="server" class="form-control" Width="250px"  autocomplete="off" onkeypress="return soloLetras(event,'abcdefghijklmnñopqrstuvwxyz0123456789_')"
                                 ></asp:TextBox>                      
                                <asp:HiddenField ID="IdTxtempresa" runat="server" ClientIDMode="Static"/> 
						</div>
                        <div class="form-group">
                             <span class="help-block">Chofer: (Opcional)</span>
						    <asp:TextBox ID="TxtChofer"  runat="server" class="form-control"   autocomplete="off"  oncopy="return false;" onpaste="return false;"  
                                    onkeypress="return soloLetras(event,'abcdefghijklmnñopqrstuvwxyz0123456789_')"></asp:TextBox>
                              <asp:HiddenField ID="IdTxtChofer" runat="server" ClientIDMode="Static"/>                           
						</div>
                        <div class="form-group">
                             <span class="help-block">Placa: (opcional)</span>
                              <asp:TextBox ID="TxtPlaca"  runat="server" class="form-control"   autocomplete="off"  oncopy="return false;" onpaste="return false;"  
                                    onkeypress="return soloLetras(event,'abcdefghijklmnñopqrstuvwxyz0123456789_')"></asp:TextBox>
                              <asp:HiddenField ID="IdTxtPlaca" runat="server" ClientIDMode="Static"/>        
                        </div>
                        
                     
                       <div class="form-group">
                            <span class="help-block">&nbsp;</span>
                           <asp:UpdatePanel ID="UPDATOSCLIENTE" runat="server"  UpdateMode="Conditional" ChildrenAsTriggers="true">
                            <ContentTemplate>
                                    <asp:Button ID="BtnAgregar" runat="server" class="btn btn-theme"  Text="Añadir" onclick="BtnAgregar_Click"/>      
                                </ContentTemplate>
                             <Triggers>
                              <asp:AsyncPostBackTrigger ControlID="BtnAgregar" />
                            </Triggers>
                        </asp:UpdatePanel>
                      </div>
                      <div class="form-group">
                           <span class="help-block">Desaduanamiento Directo :</span>
                           <asp:UpdatePanel ID="UPDESADUANAMIENTO" runat="server"  UpdateMode="Conditional" >
                           <ContentTemplate>
                            <asp:TextBox ID="TxtDesaduanamiento" runat="server" class="form-control"     
                                placeholder="" size="16" Font-Bold="true" style="text-align:center" disabled ></asp:TextBox>
                                </ContentTemplate>
                            </asp:UpdatePanel>
						</div>      
                       <div class="form-group">
                           <span class="help-block">Facturado Hasta :</span>
                           <asp:UpdatePanel ID="UPCAS" runat="server"  UpdateMode="Conditional" >
                           <ContentTemplate>
                            <asp:TextBox ID="TxtFechaCas" runat="server" class="form-control"    
                                placeholder="" size="10" Font-Bold="true" style="text-align:center" disabled ></asp:TextBox>
                                </ContentTemplate>
                            </asp:UpdatePanel>
						</div>   
                        <div class="form-group">
                           <span class="help-block">Total Bultos:</span>
                           <asp:UpdatePanel ID="UPCONTENEDOR" runat="server"  UpdateMode="Conditional" >
                           <ContentTemplate>
                            <asp:TextBox ID="TxtContenedorSeleccionado" runat="server" class="form-control"    
                                placeholder="" size="10" Font-Bold="true" style="text-align:center" disabled ></asp:TextBox>
                                </ContentTemplate>
                            </asp:UpdatePanel>
						</div>
                        <div class="form-group">
                                <span class="help-block">&nbsp;</span>
                                <ul class="task-list">
                                <li>
                                    <div class="task-checkbox">
                                 <asp:UpdatePanel ID="UPTODOS" runat="server"  UpdateMode="Conditional" ChildrenAsTriggers="true">
                                    <ContentTemplate>
                                     &nbsp;&nbsp;<asp:CheckBox ID="ChkTodos" runat="server" class="list-child" Text="Seleccionar Todos"  OnCheckedChanged="ChkTodos_CheckedChanged"    AutoPostBack="True" />
                                    </ContentTemplate> 
                                    <Triggers>
                                 <asp:AsyncPostBackTrigger ControlID="ChkTodos" />
                                </Triggers>
                                </asp:UpdatePanel> 
                                    </div> 
                              
                               </li></ul>
					</div>
                      
                </div> <!-- form-inline-->
        </div> <!-- form-panel-->
        </div> <!-- col-lg-12-->
        </div><!-- row mt-->

 
         <div class="row mt">
         <div class="col-lg-12">   
              <div class="form-panel">
			 <h4 class="mb">DETALLE DEL TURNO</h4>
                <div class="form-inline">
                  <div class="form-group">
                             <span class="help-block">Fecha Salida:</span>
                               <asp:UpdatePanel ID="UPFECHASALIDA" runat="server"  UpdateMode="Conditional" ChildrenAsTriggers="true" >
                                <ContentTemplate>
                            
                                 <asp:TextBox runat="server" ID="TxtFechaHasta"  AutoPostBack="true" MaxLength="10" 
                                         onkeypress="return soloLetras(event,'0123456789-')"    class="form-control"  
                                     ontextchanged="TxtFechaHasta_TextChanged"></asp:TextBox>

                                        <asp:CalendarExtender ID="CAGTFECHAFASA"  runat="server"
                                            CssClass="red" Format="MM-dd-yyyy" TargetControlID="TxtFechaHasta">
                                        </asp:CalendarExtender>

                              </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="TxtFechaHasta" />
                                    </Triggers>
                                </asp:UpdatePanel> 
                        </div>
                        <div class="form-group">
                             <span class="help-block">Turnos:</span>
                               <asp:UpdatePanel ID="UPTURNO" runat="server"  UpdateMode="Conditional"  >
                                <ContentTemplate>
                               <asp:DropDownList runat="server" ID="CboTurnos"  Width="150px"  AutoPostBack="false"  class="form-control"  ></asp:DropDownList>
                              </ContentTemplate>
                                </asp:UpdatePanel> 
                        </div>
                        <div class="form-group">
                            <span class="help-block">&nbsp;</span>
                           <asp:UpdatePanel ID="UPAGREGATURNO" runat="server"  UpdateMode="Conditional" ChildrenAsTriggers="true">
                            <ContentTemplate>
                                    <asp:Button ID="BtnAgregaTruno" runat="server" class="btn btn-theme"  Text="Agregar Turno" onclick="BtnAgregaTruno_Click"/>      
                                </ContentTemplate>
                             <Triggers>
                              <asp:AsyncPostBackTrigger ControlID="BtnAgregaTruno" />
                            </Triggers>
                        </asp:UpdatePanel>
                      </div>
                </div> <!-- form-inline-->
             </div> <!-- form-panel-->
        </div> <!-- col-lg-12-->
        </div><!-- row mt-->
         
     <asp:UpdatePanel ID="UPDETALLE" runat="server"  >  
       <ContentTemplate>
        
           <section class="wrapper2">
             <h3>DETALLE DE PASES A MODIFICAR</h3>
             <div class="row mb">
             <div class="col-lg-12">
              <div class="content-panel">
                  <div class="adv-table">
                  <%--<section id="unseen">--%>
                       <asp:Repeater ID="tablePagination" runat="server"  onitemcommand="tablePagination_ItemCommand" onitemdatabound="tablePagination_ItemDataBound">
                       <HeaderTemplate>
                       <table cellpadding="0" cellspacing="0" border="0" class="display table table-bordered" id="hidden-table-info">
                           <thead>
                          <tr style="background-color:#F4F4F4">
                            <th class="center hidden-phone"># PASE</th>
                            <th class="center hidden-phone">CANTIDAD</th>
                            <th class="center hidden-phone">FACTURADO <br/> HASTA</th>
                            <th class="center hidden-phone">FECHA<br/>TURNO</th>
                            <th class="center hidden-phone">H/TURNO</th>
                            <th class="center hidden-phone">CIA. TRANSPORTE</th>
                            <th class="center hidden-phone">CONDUCTOR</th>
                            <th class="center hidden-phone">PLACA</th>
                            <th class="center hidden-phone">ESTADO</th>
                            <th class="center hidden-phone">GENERAR  <br/>E-PASS</th> 
                          </tr>
                        </thead>
                        <tbody>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr class="gradeC"> 
                                <td class="center hidden-phone"><asp:Label Text='<%#Eval("ID_PASE")%>' ID="LblPase" runat="server"  /></td>
                                <td class="center hidden-phone"><asp:Label Text='<%#Eval("CANTIDAD_CARGA")%>' ID="LblCantidad" runat="server"  /></td>
                                <td class="center hidden-phone"><asp:Label Text='<%#Eval("FECHA_SALIDA")%>' ID="LblFechaSalida" runat="server"  /></td>
                                <td class="center hidden-phone"><asp:Label Text='<%#DataBinder.Eval(Container.DataItem, "FECHA_SALIDA_PASE", "{0:yyyy/MM/dd}")%>' ID="LblFechaturno" runat="server"  /></td>
                                <td class="center hidden-phone"><asp:Label Text='<%#Eval("D_TURNO")%>' ID="LblTurno" runat="server"  /></td><%--D_TURNO--%>
                                <td class="center hidden-phone"><asp:Label Text='<%#Eval("CIATRANS")%>' ID="LblEmpresa" runat="server"  /> </td>
                                <td class="center hidden-phone"><asp:Label Text='<%#Eval("CHOFER")%>' ID="LblChofer" runat="server"  />  </td> 
                                <td class="center hidden-phone"><asp:Label Text='<%#Eval("PLACA")%>' ID="LblPlaca" runat="server"  /> </td> 
                                <td class="center hidden-phone"><asp:Label Text='<%#Eval("MOSTRAR_MENSAJE")%>' ID="LblMensaje" runat="server"  />
                                     <asp:Label Text='<%#Eval("ESTADO")%>' ID="LblEstado" runat="server" Visible="false"  />
                                    <asp:Label Text='<%#Eval("IN_OUT")%>' ID="LblIn_Out" runat="server" Visible="false"  /> 
                                    <asp:Label Text='<%#Eval("CAMBIO_TURNO")%>' ID="LblCambioturno" runat="server" Visible="false"  />
                                    <asp:Label Text='<%#Eval("ESTADO_TRANSACCION")%>' ID="LblEstadoTransaccion" runat="server" Visible="false"  />
                                     <asp:Button ID="BtnEvento" CommandArgument= '<%#Eval("ID_PASE")%>' runat="server" Text="Facturar" 
                                         OnClientClick="this.disabled=true" UseSubmitBehavior="False"
                                        class="btn btn-theme08" ToolTip="Generar Cargos a nueva Factura...Debe ir a la opción de Carga Suelta CFS/Facturar E-Pass Vencido Cfs" CommandName="Facturar" />
                                </td> 
                               <td class="center hidden-phone">  
                               <asp:UpdatePanel ID="UPSELECCIONAR" runat="server" ChildrenAsTriggers="true">
                                        <ContentTemplate>
                                             <script type="text/javascript">
                                                 Sys.Application.add_load(BindFunctions); 
                                            </script>

                                            <asp:CheckBox id="chkPase" runat="server"  Checked='<%#Eval("VISTO")%>' AutoPostBack="True" OnCheckedChanged="chkPase_CheckedChanged" />
                                    
                                        </ContentTemplate>
                                         <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="chkPase" />
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
                </div><!--adv-table-->
               <%--   </section>--%>
               </div><!--content-panel-->
               </div><!--col-lg-12-->
             </div><!--row mt-->

        </section><!--wrapper2-->
     
            </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="BtnGrabar" />
                </Triggers>
            </asp:UpdatePanel>


             <asp:UpdatePanel ID="UPBOTONES" runat="server" UpdateMode="Conditional" >  
             <ContentTemplate>
                
                

                 <div class="form-group">
                         <div class="alert alert-danger" id="banmsg_det" runat="server" clientidmode="Static"><b>Error!</b> >Debe ingresar el número de la carga MRN......</div>
                 </div>
                 <div class="white-panel mt">
                      <div class="panel-body">
                            <div align="center">    
                             <img alt="loading.." src="../lib/file-uploader/img/loading.gif" height="32px" width="32px"  id="ImgCargaDet" class="nover"  />
                                </div>
                 </div>    
                 </div>  
                 <div class="showback">
                  <div class="btn-group btn-group-justified">
                    <div class="btn-group">                         
                        <asp:Button ID="BtnGrabar" runat="server" class="btn btn-theme07"  Text="Procesar Actualización e-Pass"  OnClientClick="return mostrarloader('2')" OnClick="BtnGrabar_Click" />
                    </div>
                  
                  </div><!--btn-group-justified-->
                </div><!--showback-->
            </ContentTemplate>
             </asp:UpdatePanel>   
   

       </div> <!--content-panel-->
	    
     </section><!--wrapper site-min-height-->
    </section><!--main-content-->
 <%-- </ContentTemplate>
     </asp:UpdatePanel>   --%>

 

  <script type="text/javascript" src="../lib/jquery/jquery.min.js"></script>

  <script type="text/javascript" src="../lib/bootstrap/js/bootstrap.min.js"></script>
  <script  type="text/javascript" src="../lib/jquery.dcjqaccordion.2.7.js"></script>
  <script type="text/javascript" src="../lib/jquery.scrollTo.min.js"></script>
  <script type="text/javascript" src="../lib/jquery.nicescroll.js" ></script>
 <script type="text/javascript" src="../lib/jquery.sparkline.js"></script>

  <!--common script for all pages-->
  <script type="text/javascript" src="../lib/common-scripts.js"></script>
  <script type="text/javascript" src="../lib/gritter/js/jquery.gritter.js"></script>
  <script type="text/javascript" src="../lib/gritter-conf.js"></script>
  <!--script for this page-->
 
  <script type="text/javascript" src="../lib/pages.js" ></script>
  
 <script type="text/javascript" src="../lib/bootstrap-datepicker/js/bootstrap-datepicker.js"></script> 
<%--<script type="text/javascript" src="../lib/bootstrap-daterangepicker/date.js"></script>
 <script type="text/javascript" src="../lib/bootstrap-daterangepicker/daterangepicker.js"></script>
 <script type="text/javascript" src="../lib/bootstrap-datetimepicker/js/bootstrap-datetimepicker.js"></script>
 <script type="text/javascript" src="../lib/bootstrap-timepicker/js/bootstrap-timepicker.js"></script>--%>


 <script type="text/javascript" src="../lib/advanced-form-components.js"></script>
 <script type="text/javascript" src="../lib/popup_script_cta.js" ></script>
 
<%--<script type="text/javascript" src='../lib/autocompletar/jquery-1.8.3.min.js'></script>--%>
<script type="text/javascript" src='../lib/autocompletar/bootstrap3-typeahead.min.js'></script>

 
 <script type="text/javascript">

     $(function () {
        $('[id*=Txtempresa]').typeahead({
            hint: true,
            highlight: true,
            minLength: 5,
            source: function (request, response) {
                $.ajax({
                    url: '<%=ResolveUrl("actualizacionpasecfs.aspx/GetEmpresas") %>',
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
                    url: '<%=ResolveUrl("actualizacionpasecfs.aspx/GetChofer") %>',
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
                    url: '<%=ResolveUrl("actualizacionpasecfs.aspx/GetPlaca") %>',
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

 <%-- <script type="text/javascript">

        $(document).ready(function () {
            $('.datetimepicker').datetimepicker().datetimepicker({ lang: 'en-US', timepicker: false, closeOnDateSelect: true, format: 'm-d-Y' });
      });

  
        $(document).ready(function () {
            $('.datetimepicker').datetimepicker().datetimepicker({ lang: 'en-US', timepicker: false, closeOnDateSelect: true, format: 'm-d-Y' });
            $('.datepicker').datetimepicker().datetimepicker({ lang: 'en-US', timepicker: false, format: 'm-d-Y', closeOnDateSelect: true, minDate: '0' });
        });

          
    </script>--%>


</asp:Content>