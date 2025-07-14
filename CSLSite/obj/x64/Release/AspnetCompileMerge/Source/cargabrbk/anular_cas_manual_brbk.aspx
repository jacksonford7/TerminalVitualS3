<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="anular_cas_manual_brbk.aspx.cs" Inherits="CSLSite.anular_cas_manual_brbk" %>
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
    <asp:ToolkitScriptManager ID="tkscata" runat="server"></asp:ToolkitScriptManager>   
    
  <div id="div_BrowserWindowName" style="visibility:hidden;">
    <asp:HiddenField ID="hf_BrowserWindowName" runat="server" />
     
  </div>
     <div class="mt-4">         
        <nav class="mt-4" aria-label="breadcrumb">
          <ol class="breadcrumb">
            <li class="breadcrumb-item" id="opcion_principal" runat="server"><a href="#">Impo Carga Suelta</a></li>
            <li class="breadcrumb-item active" aria-current="page" id="sub_opcion" runat="server">ANULACIÓN DE CARTA DE AUTORIZACIÓN DE SALIDA (BREAK BULK)</li>
          </ol>
        </nav>
      </div>

 <div class="dashboard-container p-4" id="cuerpo" runat="server">
        <div class="form-title">
           DATOS DEL USUARIO
     </div>
    <div class="form-row">
            <div class="form-group col-md-6"> 
                <label for="inputAddress">ESTIMADO CLIENTE:<span style="color: #FF0000; font-weight: bold;"></span></label>
                   <asp:TextBox ID="Txtcliente" runat="server" class="form-control" 
                                placeholder=""  Font-Bold="true" disabled ></asp:TextBox>
            </div>
            <div class="form-group col-md-2">
                <label for="inputAddress">RUC:<span style="color: #FF0000; font-weight: bold;"></span></label>
                
						        <asp:TextBox ID="Txtruc" runat="server" class="form-control" 
                                placeholder=""  Font-Bold="true" disabled></asp:TextBox>
            </div>
            <div class="form-group col-md-4">
                <label for="inputAddress">EMPRESA:<span style="color: #FF0000; font-weight: bold;"></span></label>
                  <asp:TextBox ID="Txtempresa" runat="server" class="form-control"  
                                placeholder=""  Font-Bold="true" disabled></asp:TextBox>
            </div>
    </div>

    <h4 class="mb">BUSCAR CARGAS AUTORIZADAS</h4>   
      <asp:UpdatePanel ID="UPCARGA" runat="server"  UpdateMode="Conditional" ChildrenAsTriggers="true">
        <ContentTemplate>
			<div class="form-row">
                 <div class="form-group col-md-6">
                    <label for="inputZip">MRN<span style="color: #FF0000; font-weight: bold;">*</span></label>
                    <div class="d-flex">
                        <asp:TextBox ID="TXTMRN" runat="server" class="form-control" MaxLength="16"  onkeypress="return soloLetras(event,'abcdefghijklmnopqrstuvwxyz0123456789-_')"  
                            placeholder="mrn"></asp:TextBox>&nbsp;
                        <asp:Button ID="BtnBuscar" runat="server" class="btn btn-primary"  Text="BUSCAR"  OnClientClick="return mostrarloader('1')" OnClick="BtnBuscar_Click" /> &nbsp;     
                         <img alt="loading.." src="../lib/file-uploader/img/loading.gif" height="32px" width="32px"  id="ImgCarga" class="nover"   />
                    </div>
                </div>
			</div>
			<br/>
           <div class="row">
            <div class="col-md-12 d-flex justify-content-center">			
                <div class="alert alert-warning" id="banmsg" runat="server" clientidmode="Static"><b>Error!</b> >Debe ingresar el número de la carga MRN......</div>
           </div>
          </div>      
        </ContentTemplate>
            <Triggers>
            <asp:AsyncPostBackTrigger ControlID="BtnBuscar" />
        </Triggers>
        </asp:UpdatePanel>

      <h4 class="mb">ANULAR AUTORIZACIÓN DE SALIDA (CAS)</h4>        
     <div class="form-row"> 
        <div class="form-group col-md-6">    
             <label for="inputZip">&nbsp;<span style="color: #FF0000; font-weight: bold;"></span></label>
            <div class="d-flex"> 
                <asp:UpdatePanel ID="UPTODOS" runat="server"  UpdateMode="Conditional" ChildrenAsTriggers="true">
                <ContentTemplate>
                    <label class="checkbox-container">
                    <asp:CheckBox ID="ChkTodos" runat="server" Text="  Anular Todos"  OnCheckedChanged="ChkTodos_CheckedChanged"    AutoPostBack="True" />
                    <span class="checkmark"></span>
                    </label>
                </ContentTemplate> 
                <Triggers>
                <asp:AsyncPostBackTrigger ControlID="ChkTodos" />
            </Triggers>
            </asp:UpdatePanel> 
                   &nbsp;&nbsp;&nbsp;
                  <asp:UpdatePanel ID="UPGRABAR" runat="server"  UpdateMode="Conditional" ChildrenAsTriggers="true">
                    <ContentTemplate>
                    <asp:Button ID="BtnGrabar" runat="server" class="btn btn-primary"   Text="PROCEDER CON LA ANULACIÓN (CAS)"  OnClientClick="return mostrarloader('2')" OnClick="BtnGrabar_Click" />  
                        
                         &nbsp;
                 <img alt="loading.." src="../lib/file-uploader/img/loading.gif" height="32px" width="32px"  id="ImgCargaDet" class="nover"   />

                    </ContentTemplate> 
                        <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="BtnGrabar" />
                    </Triggers>
                    </asp:UpdatePanel> 
                
            </div>  
        </div>    
     </div>     

  
     <asp:UpdatePanel ID="UPDETALLE" runat="server"  >  
       <ContentTemplate>
        
        
             <h3>DETALLE DE CARGAS AUTORIZADAS (CAS)</h3>
          <div class="form-row">
                 <div class="form-group col-md-6"> 
                     <label for="inputZip">BUSCAR CARGA<span style="color: #FF0000; font-weight: bold;"></span></label>
                        <asp:UpdatePanel ID="UPFILTRAR" runat="server"  UpdateMode="Conditional" ChildrenAsTriggers="true">
                        <ContentTemplate>
                            <div class="d-flex">
                            <asp:TextBox ID="TxtFiltro" runat="server" class="form-control" MaxLength="25"  onkeypress="return soloLetras(event,'abcdefghijklmnopqrstuvwxyz0123456789-_')"  
                                placeholder="MRN-MSN-HSN"></asp:TextBox>&nbsp;&nbsp;
                            <asp:Button ID="BtnFiltrar" runat="server" class="btn btn-primary"   Text="FILTRAR"   OnClick="BtnFiltrar_Click" />     
                                </div>	
                        </ContentTemplate>
                         <Triggers>
                          <asp:AsyncPostBackTrigger ControlID="BtnFiltrar" />
                        </Triggers>
                        </asp:UpdatePanel>
				 </div>		
             </div>
             <div class="form-row">
                <div class="form-group col-md-12">
                    <div class="content-panel">

                       <asp:Repeater ID="tablePagination" runat="server" onitemdatabound="tablePagination_ItemDataBound" >
                       <HeaderTemplate>
                       <table cellpadding="0" cellspacing="0" border="0" class="table table-bordered invoice" id="hidden-table-info">
                           <thead>
                          <tr>
                            <th class="center hidden-phone">CARGA</th>
                            <th class="center hidden-phone">CANTIDAD/ITEMS</th>
                            <th class="center hidden-phone">FECHA MANIFIESTO</th>
                            <th class="center hidden-phone">CONSIGNATARIO</th>
                            <th class="center hidden-phone">CONTENEDOR</th>
                            <th class="center hidden-phone">DESCRIPCIÓN</th>
                            <th class="center hidden-phone">DESCONSOLIDADORA </th>
                            <th class="center hidden-phone">FACTURAS</th>
                            <th class="center hidden-phone">USUARIO/AUTORIZA</th>
                            <th class="center hidden-phone">F/AUTORIZACIÓN</th>
                            <th class="nover">ID</th>
                            <th class="center hidden-phone">ANULAR</th>
                          
                          </tr>
                        </thead>
                        <tbody>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr class="gradeC">
                                <td class="center hidden-phone"><asp:Label Text='<%#Eval("carga")%>' ID="carga" runat="server"  /></td>
                                <td class="center hidden-phone"><asp:Label Text='<%#Eval("total_items_manifiesto")%>' ID="total_items_manifiesto" runat="server"  /></td>
                                <td class="center hidden-phone"><asp:Label Text='<%#DataBinder.Eval(Container.DataItem, "fecha_manifiesto", "{0:yyyy/MM/dd HH:mm}")%>' ID="fecha_manifiesto" runat="server"  /></td>
                                <td class="center hidden-phone"><asp:Label Text='<%#Eval("consignatario_manifiesto")%>' ID="consignatario_manifiesto" runat="server"  /></td>
                                <td class="center hidden-phone"><asp:Label Text='<%#Eval("contenedor_manifiesto")%>' ID="contenedor_manifiesto" runat="server"  /></td> 
                                <td class="center hidden-phone"><asp:Label Text='<%#Eval("descripcion_manifiesto")%>' ID="descripcion_manifiesto" runat="server"  /></td> 
                                <td class="center hidden-phone"><asp:Label Text='<%#Eval("desconsolidador_asigna_nombre")%>' ID="desconsolidador_asigna_nombre" runat="server"  /></td> 
                                <td class="center hidden-phone"><asp:Label Text='<%#Eval("facturas")%>' ID="facturas" runat="server"  /></td> 
                                <td class="center hidden-phone"><asp:Label Text='<%#Eval("usuario_libera")%>' ID="usuario_libera" runat="server"  /></td> 
                                <td class="center hidden-phone"><asp:Label Text='<%#Eval("fecha_libera")%>' ID="fecha_libera" runat="server"  /></td> 
                                <td class="nover"><asp:Label Text='<%#Eval("id")%>' ID="id" runat="server"   /></td> 
                                 <td class="center hidden-phone"> 
                                      <asp:UpdatePanel ID="UPSELECCIONAR" runat="server" ChildrenAsTriggers="true">
                                        <ContentTemplate>
                                             <script type="text/javascript">
                                                 Sys.Application.add_load(BindFunctions); 
                                            </script>
                                              <label class="checkbox-container">
                                            <asp:CheckBox id="chkMarcar" runat="server"  Checked='<%#Eval("visto")%>' AutoPostBack="True" OnCheckedChanged="chkMarcar_CheckedChanged" />
                                             <span class="checkmark"></span>
                                             </label>
                                        </ContentTemplate>
                                         <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="chkMarcar" />
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
             </div>

            </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="BtnBuscar" />
                </Triggers>
            </asp:UpdatePanel>


     </div>

 

   <script type="text/javascript" src="../lib/datatables/datatables.min.js"></script>
  <script type="text/javascript" src="../lib/advanced-datatable/js/DT_bootstrap.js"></script>

   <script type="text/javascript" src="../lib/common-scripts.js"></script>
 
   <script type="text/javascript" src="../lib/pages.js" ></script>

   <script type="text/javascript" src="../lib/advanced-form-components.js"></script>

 <script type="text/javascript" src='../lib/autocompletar/bootstrap3-typeahead.min.js'></script>


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