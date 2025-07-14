<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="facturacioncarganoexportada.aspx.cs" Inherits="CSLSite.contenedorexpo.facturacioncarganoexportada" %>
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


    <link href="../lib/advanced-datatable/css/demo_page.css" rel="stylesheet" />
    <link href="../lib/advanced-datatable/css/demo_table.css" rel="stylesheet" />
    <link rel="stylesheet" href="../lib/advanced-datatable/css/DT_bootstrap2.css" />

   <%-- <link href="../shared/estilo/tables.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/aisv-general.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/work.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/catalogos.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/info.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/proforma.css" rel="stylesheet" type="text/css" />

    <link href="../img/favicon2.png" rel="icon"/>
    <link href="../img/icono.png" rel="apple-touch-icon"/>
    <link href="../css/bootstrap.min.css" rel="stylesheet"/>
    <link href="../css/dashboard.css" rel="stylesheet"/>
    <link href="../css/icons.css" rel="stylesheet"/>
    <link href="../css/style.css" rel="stylesheet"/>
    <link href="https://fonts.googleapis.com/css2?family=Roboto:wght@300;400;500;700&display=swap" rel="stylesheet"/>

    <link href="../shared/estilo/Reset.css" rel="stylesheet" />
    <link href="../shared/estilo/jquery.datetimepicker.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/tables.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/info.css" rel="stylesheet" type="text/css" />
      
    <meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no"/>
    <link rel="canonical" href="https://getbootstrap.com/docs/4.5/examples/dashboard/"/>
    <!-- Custom styles for this template -->
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/5.15.1/css/all.min.css" integrity="sha512-+4zCK9k+qNFUR5X+cKL9EIR+ZOhtIloNl9GIKS57V1MyNsYpYcUrUeQc9vNfzsWfV28IaLL3i96P9sdNyeRssA==" crossorigin="anonymous" />
    <link href="../css/datatables.min.css" rel="stylesheet"/>--%>



<%--   

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
    <link href="../css/pagination.css" rel="stylesheet"/>--%>
 
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

            $('#<%= tablePagination.ClientID %> thead tr').each(function() {
          //this.insertBefore(nCloneTh, this.childNodes[0]);
      });

      $('#<%= tablePagination.ClientID %> tbody tr').each(function() {
          //this.insertBefore(nCloneTd.cloneNode(true), this.childNodes[0]);
      });

      /*
       * Initialse DataTables, with no sorting on the 'details' column
       */
      var oTable = $('#<%= tablePagination.ClientID %>').dataTable({
          "aoColumnDefs": [{
              "bSortable": false,
              "aTargets": [0]
          }],
          "aaSorting": [
              [1, 'asc']
          ]
      });

      /* Add event listener for opening and closing details
       * Note that the indicator for showing which row is open is not controlled by DataTables,
       * rather it is done here
       */
      $('#<%= tablePagination.ClientID %> tbody td img').live('click', function() {
          var nTr = $(this).parents('tr')[0];
          if (oTable.fnIsOpen(nTr)) {
              /* This row is already open - close it */
              this.src = "../lib/advanced-datatable/media/images/details_open.png";
              oTable.fnClose(nTr);
          } else {
              /* Open this row */
              this.src = "../lib/advanced-datatable/images/details_close.png";
              oTable.fnOpen(nTr, fnFormatDetails(oTable, nTr), 'details');
          }
      });
        });
    }
</script>

 <script type="text/javascript">
     $(document).ready(function () {
         $('#<%= tablePagination.ClientID %>').dataTable();
     });

    </script>

 
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="placebody" runat="server" >
    <asp:ToolkitScriptManager ID="tkscata" runat="server"></asp:ToolkitScriptManager>

    <div id="div_BrowserWindowName" style="visibility:hidden;">
        <asp:HiddenField ID="hf_BrowserWindowName" runat="server" />
     
    </div>

     <div class="mt-4">         
        <nav class="mt-4" aria-label="breadcrumb">
            <ol class="breadcrumb">
                <li class="breadcrumb-item" id="opcion_principal" runat="server"><a href="#">Expo Contenedores</a></li>
                <li class="breadcrumb-item active" aria-current="page" id="sub_opcion" runat="server">FACTURACIÓN DE CONTENEDORES DE EXPORTACIÓN</li>
            </ol>
        </nav>
    </div>

    <div class="dashboard-container p-4" id="cuerpo" runat="server">
        
        <div class="form-title">
            DATOS DEL BOOKING
        </div>

        
        <asp:UpdatePanel ID="UPCARGA" runat="server"  UpdateMode="Conditional" ChildrenAsTriggers="true">
            <ContentTemplate>
               

                    <div >
                        <div class="form-row" >
                                     
                            <div class="form-group col-md-4">
                                <br />
                                <div class="d-flex">
                                    &nbsp;
                                    <asp:TextBox ID="TXTMRN" runat="server" class="form-control" MaxLength="16"  onkeypress="return soloLetras(event,'abcdefghijklmnopqrstuvwxyz0123456789-_')"  
                                     placeholder="BOOKING"    type="text" >
                                    </asp:TextBox>
                                    &nbsp;
                                    <asp:Button ID="BtnBuscar" runat="server" class="btn btn-primary"  Text="BUSCAR"  OnClientClick="return mostrarloader('1')" OnClick="BtnBuscar_Click" />                             
                                    &nbsp;
                                    <img alt="loading.." src="../lib/file-uploader/img/loading.gif" height="32px" width="32px"  id="ImgCarga" class="nover"   />
                                    
                                </div>
                            </div>
                        </div>
                    </div>
                    <div><br /></div>
                    
                    <div class="alert alert-danger" id="banmsg" runat="server" clientidmode="Static"><b>Error!</b> >Debe ingresar el número de BOOKING......</div>
                    
           
                             
            </ContentTemplate>
                                                                                                                                                                                                                                
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="BtnBuscar" />
            </Triggers>
        </asp:UpdatePanel>

        <div class="form-title">
            DATOS DE LA FACTURA
        </div>
        
       
       

                <div >
                    <div class="form-row" >
              
                        <div class="form-group col-md-4">
                            
                            <div class ="d-flex">
                                &nbsp;
                                <label for="inputZip">FECHA SALIDA<span style="color: #FF0000; font-weight: bold;"></span></label>
                            </div>
                            <div class ="d-flex">
                                 <asp:TextBox runat="server" ID="TxtFechaHasta"   MaxLength="16" 
                                         onkeypress="return soloLetras(event,'0123456789-')"    class="datetimepicker form-control" ></asp:TextBox>
                                &nbsp;
                                <%--<asp:UpdatePanel ID="UPINVOICETYPE" runat="server"  UpdateMode="Conditional"  >
                                    <ContentTemplate>
                                        <asp:DropDownList runat="server" ID="CboInvoiceType"   AutoPostBack="false"  class="form-control"  ></asp:DropDownList>
                                    </ContentTemplate>
                                </asp:UpdatePanel> --%>
                            </div>                            
                        </div>

                        <div class="form-group col-md-5">
                            <label for="inputZip">EXPORTADOR:<span style="color: #FF0000; font-weight: bold;"></span></label>
                            <asp:TextBox ID="Txtempresa"  runat="server" class="form-control"  autocomplete="off"  onkeypress="return soloLetras(event,'abcdefghijklmnñopqrstuvwxyz0123456789_')"></asp:TextBox>                      
                            <asp:HiddenField ID="IdTxtempresa" runat="server" ClientIDMode="Static"/> 
                        </div>

                       
      
                        <div class="form-group col-md-3">
                            <span class="help-block">&nbsp;</span>
                          
                            <asp:UpdatePanel ID="UPFECHA" runat="server"  UpdateMode="Conditional" ChildrenAsTriggers="true">
                                    <ContentTemplate>
                                     &nbsp;&nbsp;<asp:CheckBox ID="ChkTodos" runat="server" class="list-child" Text="Facturar Todos"  OnCheckedChanged="ChkTodos_CheckedChanged"    AutoPostBack="True" />
                                    </ContentTemplate> 
                                    <Triggers>
                                 <asp:AsyncPostBackTrigger ControlID="ChkTodos" />
                                </Triggers>
                            </asp:UpdatePanel> 
                                  
		                </div>

                    </div>
                </div>   
   

        <div class="form-title">
             <strong><h5 id="LabelTotal" runat="server"> DETALLE DE CONTENEDORES</h5></strong> 
           
        </div>
                  
        <asp:UpdatePanel ID="UPDETALLE" runat="server" UpdateMode="Conditional" >  
           <ContentTemplate>
         
                 
                <asp:GridView ID="tablePagination" runat="server" AutoGenerateColumns="False"  DataKeyNames="CONTENEDOR"
                                        GridLines="None" OnPageIndexChanging="tablePagination_PageIndexChanging" OnPreRender="tablePagination_PreRender"   OnRowDataBound="tablePagination_RowDataBound" 
                                        PageSize="50"
                                        AllowPaging="True"
                                        CssClass="table table-bordered invoice">
                        <PagerStyle HorizontalAlign = "Right" CssClass="pagination-ys"  />
                        <RowStyle  BackColor="#F0F0F0" />
                        <alternatingrowstyle  BackColor="#FFFFFF" />
                           
                        <Columns>
                                                 
                                <asp:BoundField DataField="SECUENCIA" HeaderText="#"  HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>
                            <asp:TemplateField HeaderText="FA" ItemStyle-CssClass="center hidden-phone">
                                <ItemTemplate>
                                    <asp:UpdatePanel ID="UPSELECCIONAR" runat="server" ChildrenAsTriggers="true">
                                    <ContentTemplate>
                                    <asp:CheckBox ID="CHKFA" runat="server" Checked='<%# Bind("VISTO") %>' OnCheckedChanged="CHKFA_CheckedChanged"    AutoPostBack="True" CssClass="center hidden-phone" />
                                            </ContentTemplate>
                                        <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="CHKFA" />
                                        </Triggers>
                                </asp:UpdatePanel>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="CONTENEDOR" HeaderText="CONTENEDOR" SortExpression="CONTENEDOR"  HeaderStyle-CssClass="gradeC center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone" HeaderStyle-HorizontalAlign="Center"/>
                                                     
                            <asp:BoundField DataField="IN_OUT" HeaderText="ESTADO"  HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>
                            <asp:BoundField DataField="DOCUMENTO" HeaderText="DOCUMENTO"  HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>
                                                   
                            <asp:BoundField DataField="FECHA_ARRIBO" HeaderText="FECHA ARRIBO"   DataFormatString="{0:yyyy/MM/dd HH:mm}" HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>
                            <asp:BoundField DataField="CNTR_DEPARTED" HeaderText="FECHA SALIDA"   DataFormatString="{0:yyyy/MM/dd HH:mm}" HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>
                            <asp:BoundField DataField="FECHA_HASTA" HeaderText="FACTURADO HASTA"   DataFormatString="{0:yyyy/MM/dd HH:mm}" HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>
                            <asp:BoundField DataField="NUMERO_FACTURA" HeaderText="NUMERO FACTURA" HeaderStyle-HorizontalAlign="Center"  ItemStyle-HorizontalAlign="Center" />
                            <asp:BoundField DataField="DES_BLOQUEO" HeaderText="BLOQUEOS"  HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>
                            <asp:BoundField DataField="REEFER"   HeaderText="TIPO"  HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>
                            <asp:BoundField DataField="CONECTADO" HeaderText="CONECTADO"  HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone" Visible="true"/>
                            <asp:BoundField DataField="TAMANO" HeaderText="TAMAÑO CONT"  HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>
                            <asp:BoundField DataField="NUMERO_PASE_N4" HeaderText="# PASE" HeaderStyle-HorizontalAlign="Center"  ItemStyle-HorizontalAlign="Center" Visible="true"/>
                            <asp:BoundField DataField="IMDT" HeaderText="IMDT"  HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone" Visible="false"/>
                            <asp:BoundField DataField="ESTADO_RDIT" HeaderText="ESTADO RDIT"  HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone" Visible="false"/>
                            <asp:BoundField DataField="GKEY" HeaderText="GKEY"  HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone" Visible="false"/>
                            <asp:BoundField DataField="ESTADO_RDIT" HeaderText="GKEY"  HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone" Visible="false"/>
                        </Columns>
                    </asp:GridView>

     
            </ContentTemplate>
        </asp:UpdatePanel>      

        <asp:UpdatePanel ID="UPBOTONES" runat="server" UpdateMode="Conditional" >  
             <ContentTemplate>
                
                

                 <div class="form-group">
                         <div class="alert alert-danger" id="banmsg_det" runat="server" clientidmode="Static"><b>Error!</b>Debe ingresar el número de la carga MRN......</div>
                      <div class="alert alert-danger" id="banmsg_Pase" runat="server" clientidmode="Static"><b>Error!</b>.</div>
                 </div>
                 <div class="white-panel mt">
                      <div class="panel-body">
                            <div align="center">    
                                <img alt="loading.." src="../lib/file-uploader/img/loading.gif" height="32px" width="32px"  id="ImgCargaDet" class="nover"  />
                            </div>
                    </div>    
                 </div>  
                 

                 <div class="form-group col-md-12"> 
                    <div class="row">
                        <div class="col-md-12 d-flex justify-content-center">
                            <div class="d-flex">
                                <asp:Button ID="BtnNuevo" runat="server" class="btn btn-primary"  Text="NUEVA TRANSACCION"  OnClick="BtnNuevo_Click"  />
                                &nbsp;&nbsp;&nbsp;
                                <asp:Button ID="BtnFacturar" runat="server" class="btn btn-primary" Text="GENERAR FACTURA" OnClientClick="return mostrarloader('2')" OnClick="BtnFacturar_Click" />
                            </div>
                        </div>
                    </div><!--btn-group-justified-->
                </div><!--showback-->
            </ContentTemplate>
             </asp:UpdatePanel>   
   
    </div>


<%--     <section id="main-content">
       <section class="wrapper site-min-height">
         <h3>FACTURACIÓN CONTENEDOR EXPORTACIÓN</h3>

       <div class="content-panel">
			          

          <div class="form-panel">
				<h4 class="mb">DATOS DE LA FACTURA</h4>
				 <div class="form-inline">	  

                      <div class="form-group">
                            <span class="help-block">INVOICE TYPE</span>
                           <asp:UpdatePanel ID="UPINVOICETYPE" runat="server"  UpdateMode="Conditional"  >
                                <ContentTemplate>
                               <asp:DropDownList runat="server" ID="CboInvoiceType"  Width="200px"  AutoPostBack="false"  class="form-control"  ></asp:DropDownList>
                              </ContentTemplate>
                          </asp:UpdatePanel> 
                     </div>
                 
                       <div class="form-group">
                           <span class="help-block">EXPORTADOR:</span>
                               <asp:TextBox ID="Txtempresa"  runat="server" class="form-control"  autocomplete="off" 
                                   Width="400px"
                                   onkeypress="return soloLetras(event,'abcdefghijklmnñopqrstuvwxyz0123456789_')"
                                 ></asp:TextBox>                      
                                <asp:HiddenField ID="IdTxtempresa" runat="server" ClientIDMode="Static"/> 
						</div>
                     
                    
                     <div class="form-group">
                                <span class="help-block">&nbsp;</span>
                                <ul class="task-list">
                                <li>
                                    <div class="task-checkbox">
                                 <asp:UpdatePanel ID="UPFECHA" runat="server"  UpdateMode="Conditional" ChildrenAsTriggers="true">
                                    <ContentTemplate>
                                     &nbsp;&nbsp;<asp:CheckBox ID="ChkTodos" runat="server" class="list-child" Text="Facturar Todos"  OnCheckedChanged="ChkTodos_CheckedChanged"    AutoPostBack="True" />
                                    </ContentTemplate> 
                                    <Triggers>
                                 <asp:AsyncPostBackTrigger ControlID="ChkTodos" />
                                </Triggers>
                                </asp:UpdatePanel> 
                                    </div> 
                              
                               </li></ul>
					</div>
                    
                </div>

		  </div><!-- form-panel-->
       
     

             

       </div> <!--content-panel-->
	    
     </section><!--wrapper site-min-height-->
    </section><!--main-content-->--%>


<%--  <script type="text/javascript" src="../lib/jquery/jquery.min.js"></script>--%>

<%--  <script type="text/javascript" src="../lib/bootstrap/js/bootstrap.min.js"></script>--%>
  <script  type="text/javascript" src="../lib/jquery.dcjqaccordion.2.7.js"></script>
  <script type="text/javascript" src="../lib/jquery.scrollTo.min.js"></script>
  <script type="text/javascript" src="../lib/jquery.nicescroll.js" ></script>
 <script type="text/javascript" src="../lib/jquery.sparkline.js"></script>

  <!--common script for all pages-->
<%--  <script type="text/javascript" src="../lib/common-scripts.js"></script>
  <script type="text/javascript" src="../lib/gritter/js/jquery.gritter.js"></script>
  <script type="text/javascript" src="../lib/gritter-conf.js"></script>--%>
  <!--script for this page-->
 
  <script type="text/javascript" src="../lib/pages.js" ></script>
  
<%-- <script type="text/javascript" src="../lib/bootstrap-datepicker/js/bootstrap-datepicker.js"></script> --%>



 <script type="text/javascript" src="../lib/advanced-form-components.js"></script>
<%-- <script type="text/javascript" src="../lib/popup_script_cta.js" ></script>--%>
 
<script type="text/javascript" src='../lib/autocompletar/bootstrap3-typeahead.min.js'></script>


 <script type="text/javascript">

        $(function () {
            $('[id*=Txtempresa]').typeahead({
                hint: true,
                highlight: true,
                minLength: 5,
                source: function (request, response) {
                    $.ajax({
                        url: '<%=ResolveUrl("facturacioncarganoexportada.aspx/GetEmpresas") %>',
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
                    document.getElementById("ImgCargaDet").className = 'nover';
                }

            } catch (e) {
                alert('Lamentamos comunicarle que ha ocurrido el siguiente problema:\n' + e.Message);
            }
        }


  </script>

      <script type="text/javascript">
            $(document).ready(function () {
                 $('.datetimepicker').datetimepicker().datetimepicker({ lang: 'es', timepicker: true, step: 30, format: 'm/d/Y H:i' });
              });    
      </script>


</asp:Content>