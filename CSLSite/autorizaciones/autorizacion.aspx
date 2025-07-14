<%@ Page  Title="Autorización de Compañías de Transportes"  Language="C#" MasterPageFile="~/site.Master" AutoEventWireup="true" CodeBehind="autorizacion.aspx.cs" Inherits="CSLSite.autorizaciones.autorizacion" %>
<%@ MasterType VirtualPath="~/site.Master" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>

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
   <input id="AuxLinea_Naviera" type="hidden" value="" runat="server" clientidmode="Static" />
    <asp:ToolkitScriptManager ID="tkscata" runat="server"></asp:ToolkitScriptManager>

     <div class="mt-4">         
        <nav class="mt-4" aria-label="breadcrumb">
          <ol class="breadcrumb">
            <li class="breadcrumb-item" id="opcion_principal" runat="server"><a href="#">Autorizaciones</a></li>
            <li class="breadcrumb-item active" aria-current="page" id="sub_opcion" runat="server">Autorización de Compañías de Transportes</li>
          </ol>
        </nav>
      </div>

   <div class="dashboard-container p-4" id="cuerpo" runat="server">
        <div class="form-title">
             Criterios de consulta:
        </div>
         <div class="form-row">
              <div class="form-group col-md-6">
                     <label for="inputEmail4">Línea Naviera:</label>
                     <div class="d-flex">
                           <asp:TextBox ID="TxtLineaNaviera" runat="server"  MaxLength="150"  class="form-control"   disabled ></asp:TextBox>  
                            <a class="btn btn-outline-primary mr-4" target="popup" onclick="window.open('../autorizaciones/lookup_linea_naviera.aspx','name','width=1024,height=800')"  id="buscar_linea" runat="server" visible="false">
                                <span class='fa fa-search' style='font-size:24px' id="BtnBuscarLinea" clientidmode="Static"></span> </a>   
                     </div> 
                      
              </div> 
              <div class="form-group col-md-12">
                <label for="inputEmail4">Empresa de Transporte:</label>
                       <asp:TextBox ID="Txtempresa" runat="server"  MaxLength="150"  class="form-control"
                     autocomplete="off"  
                      onkeypress="return soloLetras(event,'abcdefghijklmnopqrstuvwxyz01234567890/')" ></asp:TextBox> 
                    <asp:HiddenField ID="IdTxtempresa" runat="server" ClientIDMode="Static"/> 
             </div> 

         </div> 
        
         <div class="row">
                <div class="col-md-12 d-flex justify-content-center">
                      <asp:UpdatePanel ID="UPEMPRESA" runat="server"  UpdateMode="Conditional" ChildrenAsTriggers="true">
                            <ContentTemplate>
                                    <asp:Button ID="BtnAgregar" runat="server"   class="btn btn-primary" Text="Añadir" onclick="BtnAgregar_Click" OnClientClick="return getGif();"/> &nbsp;&nbsp;
                                          <asp:Button ID="BtnLimpiar" runat="server" class="btn btn-outline-primary mr-4"  Text="Limpiar" onclick="BtnLimpiar_Click" OnClientClick="clearTextBox();" />  
                                </ContentTemplate>
                             <Triggers>
                              <asp:AsyncPostBackTrigger ControlID="BtnAgregar" />
                            </Triggers>
                        </asp:UpdatePanel>
               </div>
         </div>    
         
        <div class="form-title">
            2.- DETALLE DE EMPRESAS DE TRANSPORTE
        </div>
       
       <div class="form-row"> 
          <div class="form-group col-md-12">
             <asp:UpdatePanel ID="upresult" runat="server" ChildrenAsTriggers="true">
            <ContentTemplate>

            <input id="idlin" type="hidden" runat="server" clientidmode="Static" />
            <input id="diponible" type="hidden" runat="server" clientidmode="Static" />
            <div class="alert alert-warning" id="alerta" runat="server" style=" display:none" ></div>
            <div id="xfinder" runat="server" visible="false" >
            <div class="findresult" >
       
            <div class="informativo" style=" height:100%;">
             
              <div class="bokindetalle">
         
               <asp:UpdatePanel ID="UPDETALLE" runat="server"  UpdateMode="Conditional" >  
               <ContentTemplate>
               <%--    <h3>DETALLE DE EMPRESAS DE TRANSPORTE</h3>--%>

                           <asp:Repeater ID="tablePagination" runat="server"  onitemcommand="tablePagination_ItemCommand">
                           <HeaderTemplate>
                           <table cellpadding="0" cellspacing="0" border="0" class="table table-bordered invoice" id="hidden-table-info">
                               <thead>
                              <tr >
                                <th class="center hidden-phone">ID</th>
                                <th class="center hidden-phone">RUC</th>
                                <th class="center hidden-phone">RAZON SOCIAL</th>
                                <th class="center hidden-phone">LÍNEA NAVIERA</th>
                                <th class="center hidden-phone">CREADO POR</th>
                                <th class="center hidden-phone">FECHA CREACIÓN</th>
                                <th class="center hidden-phone">ACCIÓN</th>
                          
                              </tr>
                            </thead>
                            <tbody>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <tr class="gradeC">
                                    <td class="center hidden-phone"><asp:Label Text='<%#Eval("ID")%>' ID="ID" runat="server"  /></td>
                                    <td class="center hidden-phone"><asp:Label Text='<%#Eval("ID_EMPRESA")%>' ID="ID_EMPRESA" runat="server"  /></td>
                                    <td class="center hidden-phone"><asp:Label Text='<%#Eval("RAZON_SOCIAL")%>' ID="RAZON_SOCIAL" runat="server" />  </td>
                                    <td class="center hidden-phone"><%#Eval("LINEA_NAVIERA")%></td>
                                    <td class="center hidden-phone"><%#Eval("USUARIO_CRE")%></td>
                                    <td class="center hidden-phone"><%#Eval("FECHA_CREA")%></td> 
                                    <td class="center hidden-phone">  <asp:Button ID="BtnAprobar"    CommandArgument= '<%#Eval("ID")%>' runat="server" Text="Remover" 
                           class="btn btn-outline-primary mr-4" ToolTip="Permite remover una empresa de transporte" CommandName="Remover"   OnClientClick="return confirm('Esta seguro de que desea inactivar la empresa de transporte?');"  /></td> 
 
                                 </tr>    
                           </ItemTemplate>
                           <FooterTemplate>
                            </tbody>
                          </table>
                         </FooterTemplate>
                        </asp:Repeater>
  
                </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="BtnAgregar" />
                    </Triggers>
                </asp:UpdatePanel>

                    </div>

            </div>
            </div>
            </div>
            <div id="sinresultado" runat="server" class="alert alert-warning"></div>
            </ContentTemplate>
                         <Triggers>
                             <asp:AsyncPostBackTrigger ControlID="BtnAgregar" />
                         </Triggers>
            </asp:UpdatePanel>
              </div>
           </div>


  </div> 

    <script type="text/javascript" src="../lib/jquery/jquery.min.js"></script>
     <script type="text/javascript" src="../lib/bootstrap/js/bootstrap.min.js"></script>
  <script type="text/javascript" src='../lib/autocompletar/bootstrap3-typeahead.min.js'></script>

 <script type="text/javascript">

     $(function () {
        $('[id*=Txtempresa]').typeahead({
            hint: true,
            highlight: true,
            minLength: 5,
            source: function (request, response) {
                $.ajax({
                    url: '<%=ResolveUrl("autorizacion.aspx/GetEmpresas") %>',
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
    function clearTextBox()
    {
    
         document.getElementById('Txtempresa').value = '';
    }

    function Buscar_Lineas()
    {
  
        window.open('../autorizaciones/lookup_linea_naviera.aspx', 'name', 'width=850,height=480');

    }

    function popupCallback_Lineas(lookup_get_linea)
    {
            if (lookup_get_linea.sel_g_id_linea != null)
            {
                this.document.getElementById('<%= TxtLineaNaviera.ClientID %>').value = lookup_get_linea.sel_g_id_linea;
                //this.document.getElementById('TxtLineaNaviera').value = lookup_get_linea.sel_g_id_linea;
                this.document.getElementById('AuxLinea_Naviera').value = lookup_get_linea.sel_g_id_linea;

            }
            else
            {

                 this.document.getElementById('<%= TxtLineaNaviera.ClientID %>').value = "";
            }

        __doPostBack();
    }

</script>

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

