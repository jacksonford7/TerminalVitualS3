<%@ Page  Title="Detalle de Errores EDO"  Language="C#" MasterPageFile="~/site.Master" AutoEventWireup="true" CodeBehind="errores_contenedores.aspx.cs" Inherits="CSLSite.autorizaciones.errores_contenedores" %>
<%@ MasterType VirtualPath="~/site.Master" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="placehead" runat="server">

    <link href="../img/favicon2.png" rel="icon"/>
      <link href="../img/icono.png" rel="apple-touch-icon"/>
      <link href="../css/bootstrap.min.css" rel="stylesheet"/>
      <link href="../css/dashboard.css" rel="stylesheet"/>
      <link href="../css/icons.css" rel="stylesheet"/>
      <link href="../css/style.css" rel="stylesheet"/>
      <link href="https://fonts.googleapis.com/css2?family=Roboto:wght@300;400;500;700&display=swap" rel="stylesheet"/>



    <style type="text/css">
     * input[type=text]
        {
            text-align:left!important;
        }
        .style1
        {
            border-bottom: 1px solid #CCC;
            width: 530px;
        }
    </style>

     
<script type="text/javascript">

 function BindFunctions() {

     $(document).ready(function() {
      /*
       * Insert a 'details' column to the table
       */
      var nCloneTh = document.createElement('th');
      var nCloneTd = document.createElement('td');
      nCloneTd.innerHTML = '<img src="lib/advanced-datatable/images/details_open.png">';
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

<asp:Content ID="Content2" ContentPlaceHolderID="placebody" runat="server">
   <input id="zonaid" type="hidden" value="7" />

    <asp:ToolkitScriptManager ID="tkscata" runat="server"></asp:ToolkitScriptManager>

    <div class="mt-4">         
        <nav class="mt-4" aria-label="breadcrumb">
          <ol class="breadcrumb">
            <li class="breadcrumb-item" id="opcion_principal" runat="server"><a href="#">Errores</a></li>
            <li class="breadcrumb-item active" aria-current="page" id="sub_opcion" runat="server">Detalle de Errores al Procesar EDO Contenedores Vacíos.</li>
          </ol>
        </nav>
    </div>
<div class="dashboard-container p-4" id="cuerpo" runat="server">
        <div class="form-title">
            1.- Criterios de Procesamiento
        </div>
       
       <div class="form-row">
            <div class="form-group col-md-6">
                     <label for="inputEmail4">16. Estiba bajo cubierta</label>
                     <div class="d-flex">
                          
                           <asp:UpdatePanel ID="UPFECHA" runat="server"  UpdateMode="Conditional" ChildrenAsTriggers="true">
                           <ContentTemplate>
                          <label class="checkbox-container">
                           <asp:CheckBox ID="ChkTodos" runat="server"  Text=" Seleccionar Todos"   OnCheckedChanged="ChkTodos_CheckedChanged"   AutoPostBack="True" />
                               <span class="checkmark"></span>
                            </label>
                               </ContentTemplate> 
                            <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="ChkTodos" />
                           </Triggers>
                           </asp:UpdatePanel>
                            
                         &nbsp;&nbsp;
                         <asp:UpdatePanel ID="UpdatePanel1" runat="server"  UpdateMode="Conditional" ChildrenAsTriggers="true">
                         <ContentTemplate>
                         <asp:Button ID="BtnProcesarTodos" runat="server" class="btn btn-outline-primary mr-4"   Text="Procesar"   OnClick="BtnProcesarTodos_Click" OnClientClick="return confirm('Está seguro de que desea activar los contenedor seleccionados, para volver a procesarlo?');" />
                       </ContentTemplate> 
                            <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="BtnProcesarTodos" />
                        </Triggers>
                        </asp:UpdatePanel>
                         &nbsp;&nbsp;
                         <asp:UpdatePanel ID="UpdatePanel2" runat="server"  UpdateMode="Conditional" ChildrenAsTriggers="true">
                         <ContentTemplate>
                         <asp:Button ID="BtnEliminarTodos" runat="server" class="btn btn-outline-primary mr-4"  Text="Eliminar"   OnClick="BtnEliminarTodos_Click" OnClientClick="return confirm('Está seguro de que desea inactivar los contenedor seleccionados?, El registro desaparecerá de los errores.');"/> 
                         </ContentTemplate> 
                         <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="BtnEliminarTodos" />
                        </Triggers>
                        </asp:UpdatePanel>

                     </div>
            </div>
        </div>
       <div class="form-title">
           2. DETALLE DE MOVIMIENTOS
       </div>
       <div class="form-row"> 
          <div class="form-group col-md-12">
                <asp:UpdatePanel ID="upresult" runat="server" ChildrenAsTriggers="true">
        <ContentTemplate>

        <input id="idlin" type="hidden" runat="server" clientidmode="Static" />
        <input id="diponible" type="hidden" runat="server" clientidmode="Static" />
        <div class="msg-alerta" id="alerta" runat="server" style=" display:none" ></div>
        <div id="xfinder" runat="server" visible="false" >
        <div class="findresult" >
       
        <div class="informativo" style=" height:100%;">
             
          <div class="bokindetalle">
         
           <asp:UpdatePanel ID="UPDETALLE" runat="server"  UpdateMode="Conditional" >  
           <ContentTemplate>
             <%-- <h3>DETALLE DE MOVIMIENTOS</h3>--%>
              <div class="content-panel">
                  <div class="bokindetalle" style=" width:100%; overflow:auto">       
                
                       <asp:Repeater ID="tablePagination" runat="server"  onitemcommand="tablePagination_ItemCommand">
                       <HeaderTemplate>
                       <table cellpadding="0" cellspacing="0" border="0" class="table table-bordered invoice" id="hidden-table-info">
                           <thead>
                          <tr >
                            <th scope="col">MARCAR</th>
                            <th scope="col"># ORDEN</th>
                            <th scope="col">FECHA</th>
                            <th scope="col">CREADO POR</th>
                            <th scope="col">LÍNEA NAVIERA</th>
                            <th scope="col">AUTORIZACIÓN</th>
                            <th scope="col">REFERENCIA</th>
                            <th scope="col">CONTENEDOR</th>
                            <th scope="col">ERROR</th>
                            <th scope="col">PROCESAR</th>
                            <th scope="col">ELIMINAR</th>
                          </tr>
                        </thead>
                        <tbody>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr class="point">
                                  <td scope="row"> 
                                    <asp:UpdatePanel ID="UPSELECCIONAR" runat="server" ChildrenAsTriggers="true">
                                        <ContentTemplate>
                                             <script type="text/javascript">
                                                 Sys.Application.add_load(BindFunctions); 
                                            </script>
                                            <label class="checkbox-container">
                                            <asp:CheckBox id="chkMarcar" runat="server" AutoPostBack="True"   />
                                             <span class="checkmark"></span>
                                             </label>
                                        </ContentTemplate>
                                         <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="chkMarcar" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                </td>
                                <td scope="row"><asp:Label Text='<%#Eval("ID")%>' ID="ID" runat="server"  /><asp:Label Text='<%#Eval("SECUENCIA")%>' ID="SECUENCIA" runat="server" Visible="false"  /></td>
                                <td scope="row"><asp:Label Text='<%#Eval("FECHA")%>' ID="ID_EMPRESA" runat="server"  /></td>
                                <td scope="row"><asp:Label Text='<%#Eval("USUARIO_CRE")%>' ID="RAZON_SOCIAL" runat="server" />  </td>
                                <td scope="row"><asp:Label Text='<%#Eval("LINEA_NAVIERA")%>' ID="LINEA_NAVIERA" runat="server" /></td>
                                <td scope="row"><%#Eval("AUTORIZACION")%></td>
                                <td scope="row"><%#Eval("REFERENCIA")%></td> 
                                <td scope="row"><asp:Label Text='<%#Eval("CONTENEDOR")%>' ID="CONTENEDOR" runat="server" /></td> 
                                <td scope="row"><%#Eval("MENSAJE")%></td> 
                                <td scope="row">  <asp:Button ID="BtnAprobar"    CommandArgument= '<%#Eval("SECUENCIA")%>' runat="server" Text="Procesar" 
                      class="btn btn-outline-primary mr-4" ToolTip="Permite volver a procesar el contenedor" CommandName="Remover"   OnClientClick="return confirm('Está seguro de que desea activar el contenedor, para volver a procesarlo?');"  />

                                </td> 
                                <td scope="row">  <asp:Button ID="BtnEliminar"    CommandArgument= '<%#Eval("SECUENCIA")%>' runat="server" Text="Eliminar" 
                      class="btn btn-outline-primary mr-4" ToolTip="Permite inactivar un registro de error de contenedor" CommandName="Delete"   OnClientClick="return confirm('Está seguro de que desea inactivar el contenedor?, El registro desaparecerá de los errores.');"  />

                                </td> 
                             </tr>    
                       </ItemTemplate>
                       <FooterTemplate>
                        </tbody>
                      </table>
                     </FooterTemplate>
                    </asp:Repeater>
                </div><!--adv-table-->
             
               </div><!--content-panel-->
  
            </ContentTemplate>
            </asp:UpdatePanel>

                </div>

        </div>
        </div>
        </div>
        <div id="sinresultado" runat="server" class="alert alert-warning"></div>
        </ContentTemplate>
                 
        </asp:UpdatePanel>
          </div>

      </div>
 </div>



  <script type="text/javascript" src="lib/jquery/jquery.min.js"></script>
  <script type="text/javascript" src="lib/bootstrap/js/bootstrap.min.js"></script>
  <script type="text/javascript" src='lib/autocompletar/bootstrap3-typeahead.min.js'></script>

  <script src="../Scripts/pages.js" type="text/javascript"></script>
 <script src="../Scripts/credenciales.js" type="text/javascript"></script>

    <script type="text/javascript">

        function getGif() {document.getElementById('imagen').innerHTML = '<img alt="" src="../shared/imgs/loader.gif"/>';
            return true;
        }
        function getGifOculta() {
            document.getElementById('imagen').innerHTML = '<img alt="" src=""/>';
            return true;
        }
    </script>
    
</asp:Content>

