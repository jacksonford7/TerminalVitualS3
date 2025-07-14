<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="backoffice_descon_manual.aspx.cs" Inherits="CSLSite.backoffice_descon_manual" %>
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
  <link href="../lib/font-awesome/css/font-awesome.css" rel="stylesheet" />
  <link rel="stylesheet" type="text/css" href="../lib/gritter/css/jquery.gritter.css" />


  <link href="../lib/advanced-datatable/css/demo_page.css" rel="stylesheet" />
  <link href="../lib/advanced-datatable/css/demo_table.css" rel="stylesheet" />
  <link rel="stylesheet" href="../lib/advanced-datatable/css/DT_bootstrap.css" />
   
 


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
            <li class="breadcrumb-item" id="opcion_principal" runat="server"><a href="#">BackOffice</a></li>
            <li class="breadcrumb-item active" aria-current="page" id="sub_opcion" runat="server">REGISTRO MANUAL DESCONSOLIDADORA - CARGA SUELTA</li>
          </ol>
        </nav>
      </div>

 <div class="dashboard-container p-4" id="cuerpo" runat="server">

      <div class="form-title">
          DATOS DEL USUARIO
     </div>
     
     <div class="form-row">
          <div class="form-group col-md-6"> 
              <label for="inputAddress">RUC:<span style="color: #FF0000; font-weight: bold;"></span></label>
                <asp:TextBox ID="Txtcliente" runat="server" class="form-control"  size="50" 
                                placeholder=""  Font-Bold="true" disabled  Visible="false"></asp:TextBox>
                <asp:TextBox ID="Txtruc" runat="server" class="form-control"  size="25" 
                                placeholder=""  Font-Bold="true" disabled></asp:TextBox>
          </div>
          <div class="form-group col-md-6"> 
              <label for="inputAddress">EMPRESA:<span style="color: #FF0000; font-weight: bold;"></span></label>
                 <asp:TextBox ID="Txtempresa" runat="server" class="form-control"  size="30" 
                                placeholder=""  Font-Bold="true" disabled></asp:TextBox>
          </div>

     </div>
     <div class="form-title">
          DATOS DE LA CARGA
     </div>
      <asp:UpdatePanel ID="UPCARGA" runat="server"  UpdateMode="Conditional" ChildrenAsTriggers="true">
      <ContentTemplate>
     <div class="form-row">
      
             <div class="form-group col-md-4">
              <label for="inputZip">MRN</label>
              <asp:TextBox ID="TXTMRN" runat="server" class="form-control" MaxLength="16"  onkeypress="return soloLetras(event,'abcdefghijklmnopqrstuvwxyz0123456789-_')"  
                                 placeholder="mrn"></asp:TextBox>
            </div>
             <div class="form-group col-md-2">
              <label for="inputZip">MSN</label>
               <asp:TextBox ID="TXTMSN" runat="server" class="form-control"  MaxLength="4"  onkeypress="return soloLetras(event,'abcdefghijklmnopqrstuvwxyz0123456789-_')" 
                                placeholder="MSN"></asp:TextBox>
            </div> 
             <div class="form-group col-md-2">
              <label for="inputZip">MSN</label>
              <div class="d-flex">
                <asp:TextBox ID="TXTHSN" runat="server" class="form-control"  MaxLength="4"  onkeypress="return soloLetras(event,'abcdefghijklmnopqrstuvwxyz0123456789-_')" 
                                placeholder="HSN"></asp:TextBox>
                &nbsp;&nbsp;
                            <asp:Button ID="BtnBuscar" runat="server" class="btn btn-primary"  Text="BUSCAR"  OnClientClick="return mostrarloader('1')" OnClick="BtnBuscar_Click" />  
                   &nbsp;&nbsp;
                 <img alt="loading.." src="../lib/file-uploader/img/loading.gif" height="32px" width="32px"  id="ImgCarga" class="nover"   />
                          </div> 
            </div> 
           <br/>
              <div class="form-group col-md-12">
                <div class="alert alert-warning" id="banmsg" runat="server" clientidmode="Static"><b>Error!</b> >Debe ingresar el número de la carga MRN......</div>
            </div> 
     </div>
       </ContentTemplate>
        <Triggers>
        <asp:AsyncPostBackTrigger ControlID="BtnBuscar" />
    </Triggers>
    </asp:UpdatePanel>
   
      <div class="form-row">
          <div class="form-group col-md-12">
            <asp:UpdatePanel ID="UPDETALLE" runat="server"  >  
            <ContentTemplate>

                       <asp:Repeater ID="tablePagination" runat="server"  >
                       <HeaderTemplate>
                       <table cellpadding="0" cellspacing="0" border="0" class="table table-bordered invoice" id="hidden-table-info">
                           <thead>
                          <tr >
                            <th class="center hidden-phone">CARGA</th>
                            <th class="center hidden-phone">CANTIDAD/ITEMS</th>
                            <th class="center hidden-phone">FECHA MANIFIESTO</th>
                            <th class="center hidden-phone">CONSIGNATARIO</th>
                            <th class="center hidden-phone">CONTENEDOR</th>
                            <th class="center hidden-phone">DESCRIPCIÓN</th>
                            <th class="center hidden-phone">BL MANIFIESTO</th>
                              <th class="center hidden-phone">PESO BL</th>
                            <th class="nover">CONSIGNATARIO</th>
                            <th class="nover">DESCONSOLIDADOR</th>
                            <th class="nover">ID MANIFIESTO</th>
                            <th class="nover">ID MANIFIESTO DETALLE</th>
                            <th class="nover">LLAVE</th>
                           <th class="center hidden-phone">ACTUALIZAR</th>
                          
                          </tr>
                        </thead>
                        <tbody>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr >
                                <td class="center hidden-phone"><asp:Label Text='<%#Eval("carga")%>' ID="carga" runat="server"  /></td>
                                <td class="center hidden-phone"><asp:Label Text='<%#Eval("total_items_manifiesto")%>' ID="total_items_manifiesto" runat="server"  /></td>
                                <td class="center hidden-phone"><asp:Label Text='<%#DataBinder.Eval(Container.DataItem, "fecha_manifiesto", "{0:yyyy/MM/dd HH:mm}")%>' ID="fecha_manifiesto" runat="server"  /></td>
                                <td class="center hidden-phone"><asp:Label Text='<%#Eval("consignatario_manifiesto")%>' ID="consignatario_manifiesto" runat="server"  /></td>
                                <td class="center hidden-phone"><asp:Label Text='<%#Eval("contenedor_manifiesto")%>' ID="contenedor_manifiesto" runat="server"  /></td> 
                                <td class="center hidden-phone"><asp:Label Text='<%#Eval("descripcion_manifiesto")%>' ID="descripcion_manifiesto" runat="server"  /></td> 
                                <td class="center hidden-phone"><asp:Label Text='<%#Eval("bl_manifiesto")%>' ID="bl_manifiesto" runat="server"  /></td> 
                                <td class="center hidden-phone"><asp:Label Text='<%#Eval("peso_total")%>' ID="peso_total" runat="server"  /></td> 
                                <td class="nover"><asp:Label Text='<%#Eval("consignatario_manifiesto_id")%>' ID="consignatario_manifiesto_id" runat="server"   /></td> 
                                <td class="nover"><asp:Label Text='<%#Eval("desconsolidador_manifiesto")%>' ID="desconsolidador_manifiesto" runat="server"   /></td> 
                                <td class="nover"><asp:Label Text='<%#Eval("id_manifiesto")%>' ID="id_manifiesto" runat="server"   /></td> 
                                <td class="nover"><asp:Label Text='<%#Eval("id_manifiesto_detalle")%>' ID="id_manifiesto_detalle" runat="server"   /></td> 
                                <td class="nover"><asp:Label Text='<%#Eval("llave")%>' ID="llave" runat="server"   /></td> 
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

            </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="BtnBuscar" />
                </Triggers>
            </asp:UpdatePanel>       
         </div>
     </div>

      <div class="form-title">
         DATOS DE LA DESCONSOLIDADORA
     </div>

      <asp:UpdatePanel ID="UPDATOSCLIENTE" runat="server"  UpdateMode="Conditional" ChildrenAsTriggers="true">
      <ContentTemplate>
     <div class="form-row">
          <div class="form-group col-md-6">
              <label for="inputZip">ID DESCONSOLIDADORA</label>
               

                    <asp:UpdatePanel ID="UPAGENTE" runat="server"  UpdateMode="Conditional" ChildrenAsTriggers="true">
                    <ContentTemplate>
                        <asp:TextBox ID="TxtIdAgente" runat="server" class="form-control"    
                        placeholder="" size="10" MaxLength="13" Font-Bold="false" onkeypress="return soloLetras(event,'abcdefghijklmnñopqrstuvwxyz0123456789_')" 
                        ClientIDMode="Static" ></asp:TextBox>
                    </ContentTemplate> 
                    <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="TxtIdAgente" />
                </Triggers>
                </asp:UpdatePanel> 

                              
            
         </div>

          <div class="form-group col-md-6">
               <label for="inputZip">&nbsp; </label>
              <div class="d-flex">
                     <asp:TextBox ID="TXTAGENCIA" runat="server" class="form-control"    
                        placeholder="" size="16" Font-Bold="false" disabled  ClientIDMode="Static"></asp:TextBox>
                            
                         &nbsp;&nbsp;             
                    <asp:Button ID="BtnActualizar" runat="server"  class="btn btn-primary"  Text="ACTUALIZAR"  OnClientClick="return mostrarloader('1')" OnClick="BtnActualizar_Click" />      
            
              </div>
          </div>

    </div>
     </ContentTemplate>
                         <Triggers>
                          <asp:AsyncPostBackTrigger ControlID="BtnBuscar" />
                        </Triggers>
                    </asp:UpdatePanel>
      
	    
   
   

  </div>

 <%-- <script type="text/javascript" src="../lib/jquery/jquery.min.js"></script>

  <script type="text/javascript" src="../lib/bootstrap/js/bootstrap.min.js"></script>
  <script  type="text/javascript" src="../lib/jquery.dcjqaccordion.2.7.js"></script>
  <script type="text/javascript" src="../lib/jquery.scrollTo.min.js"></script>
  <script type="text/javascript" src="../lib/jquery.nicescroll.js" ></script>--%>

  <!--common script for all pages-->
  <script type="text/javascript" src="../lib/common-scripts.js"></script>
  <script type="text/javascript" src="../lib/gritter/js/jquery.gritter.js"></script>
  <script type="text/javascript" src="../lib/gritter-conf.js"></script>
  <!--script for this page-->
 
  <script type="text/javascript" src="../lib/pages.js" ></script>
  

 <script type="text/javascript" src="../lib/advanced-form-components.js"></script>
 <script type="text/javascript" src="../lib/popup_script_cta.js" ></script>

<script type="text/javascript">

    function mostrarloader(Valor) {

        try {
            if (Valor == "1") {
                document.getElementById("ImgCarga").className = 'ver';
            }
            else {
                
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
                
            }

             } catch (e) {
                alert('Lamentamos comunicarle que ha ocurrido el siguiente problema:\n' + e.Message);
            }
        }

</script>



</asp:Content>