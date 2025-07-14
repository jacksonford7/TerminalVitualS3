<%@ Page  Title="Excluir vehículos y choferes"  Language="C#" MasterPageFile="~/site.Master" AutoEventWireup="true" CodeBehind="excluir_veh_chof.aspx.cs" Inherits="CSLSite.autorizaciones.excluir_veh_chof" %>
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
        $(document).ready(function () {
            $('#<%= tablePagination.ClientID %>').dataTable();
        });
       
    </script>

     <script type="text/javascript">
        $(document).ready(function () {
            $('#<%= tablePagination2.ClientID %>').dataTable();
        });
       
    </script>

 </asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="placebody" runat="server">
 
   
    <input id="zonaid" type="hidden" value="7" />
    <input id="IdEmpresa" type="hidden" value="" runat="server" clientidmode="Static" />
    <input id="RucEmpresa" type="hidden" value="" runat="server" clientidmode="Static" />
    <input id="NombreEmpresa" type="hidden" value="" runat="server" clientidmode="Static" />
    
    <input id="IdVehiculo" type="hidden" value="" runat="server" clientidmode="Static" />
    <input id="Placa" type="hidden" value="" runat="server" clientidmode="Static" />

    <input id="IdChofer" type="hidden" value="" runat="server" clientidmode="Static" />
    <input id="Licencia" type="hidden" value="" runat="server" clientidmode="Static" />
    <input id="NombreChofer" type="hidden" value="" runat="server" clientidmode="Static" />

    <input id="AuxLinea_Naviera" type="hidden" value="" runat="server" clientidmode="Static" />

    <asp:ToolkitScriptManager ID="tkscata" runat="server"></asp:ToolkitScriptManager>
    
    <div class="mt-4">         
        <nav class="mt-4" aria-label="breadcrumb">
          <ol class="breadcrumb">
            <li class="breadcrumb-item" id="opcion_principal" runat="server"><a href="#">Exclusiones</a></li>
            <li class="breadcrumb-item active" aria-current="page" id="sub_opcion" runat="server">Excluir Vehículos y choferes</li>
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
         <div class="form-group col-md-6">
             <label for="inputEmail4">Empresa de Transporte:</label>
             <div class="d-flex">
                 <asp:TextBox ID="Txtempresa" runat="server"  MaxLength="150"  class="form-control"
                 autocomplete="off"  
                  onkeypress="return soloLetras(event,'abcdefghijklmnopqrstuvwxyz01234567890/')" Font-Bold="false" disabled></asp:TextBox>  
                 <a class="btn btn-outline-primary mr-4" target="popup" onclick="Buscar_Empresas();"  id="llave1" runat="server" >
                                <span class='fa fa-search' style='font-size:24px' id="BtnBuscarEmpresa" clientidmode="Static"></span> </a>   
             </div> 
         </div>
         <div id="sinresultado" runat="server" class="alert alert-warning"></div>
    </div>




 <asp:UpdatePanel ID="upresult" runat="server" ChildrenAsTriggers="true">
<ContentTemplate>

        <input id="idlin" type="hidden" runat="server" clientidmode="Static" />
        <input id="diponible" type="hidden" runat="server" clientidmode="Static" />
        <div class="msg-alerta" id="alerta" runat="server" style=" display:none" ></div>
       
        <div class="form-title">
                 1. Excluir Choferes
        </div>
        <div>Puede buscar los choferes a excluir</div>

         <asp:UpdatePanel ID="UpdatePanel2" runat="server" ChildrenAsTriggers="true">
         <ContentTemplate>
             <div class="form-row">    
                 <div class="form-group col-md-6">
                     <label for="inputEmail4">Chofer:</label>
                     <div class="d-flex">
                        <asp:TextBox ID="TxtChofer" runat="server"  MaxLength="150"  class="form-control"
                                             autocomplete="off"  
                            onkeypress="return soloLetras(event,'abcdefghijklmnopqrstuvwxyz01234567890/')" Font-Bold="false" disabled>
                        </asp:TextBox>
                         <a class="btn btn-outline-primary mr-4" target="popup" onclick="Buscar_Chofer();"  id="buscar_chofer" runat="server" visible="true">
                                        <span class='fa fa-search' style='font-size:24px' id="LookupChofer" clientidmode="Static"></span> </a>   
              
                     </div> 
                 </div> 
                 <div class="form-group col-md-6">
                     <label for="inputEmail4">Motivo:</label>
                     <div class="d-flex">
                         <asp:TextBox ID="TxtMotivoChofer" runat="server"  MaxLength="150"  class="form-control"
                                             autocomplete="off"  
                                              onkeypress="return soloLetras(event,'abcdefghijklmnopqrstuvwxyz01234567890/')" Font-Bold="false" ></asp:TextBox>
                         <asp:Button ID="BtnAgregarChofer" runat="server" class="btn btn-primary"  Text="Excluir" onclick="BtnAgregarChofer_Click" />
                     </div>
                 </div>
                 <div class="form-group col-md-12">
                    <label for="inputEmail4">Buscar:</label>
                     <div class="d-flex"> 
                         <asp:TextBox ID="TxtBuscarChofer" runat="server"  MaxLength="50"  class="form-control"
                                                         autocomplete="off"  
                                                          onkeypress="return soloLetras(event,'abcdefghijklmnopqrstuvwxyz01234567890/')" Font-Bold="false"  ></asp:TextBox>
                         <asp:Button ID="BtnBuscarChofer" runat="server"  class="btn btn-outline-primary mr-4" Text="Buscar" onclick="BtnBuscarChofer_Click" />
                     </div>
                 </div>

             </div> 
             <div class="">Detalle De Choferes</div>
            <div class="form-row">
              <div class="form-group col-md-12">
                <div class="table table-bordered invoice">   
                 
                                <asp:GridView ID="tablePagination2" runat="server" AutoGenerateColumns="False"  DataKeyNames="ID"
                                                        GridLines="None" OnPageIndexChanging="tablePagination2_PageIndexChanging" 
                                                            OnRowCommand="tablePagination2_RowCommand"
                                                        PageSize="10"
                                                        AllowPaging="True"
                                                        CssClass="table table-bordered invoice">
                                        <PagerStyle HorizontalAlign = "Right" CssClass="table table-bordered invoice" />
                                        <RowStyle  BackColor="#F0F0F0" />
                                    <alternatingrowstyle  BackColor="#FFFFFF" />
                           
                                                        <Columns>
                                                 
                                                    <asp:BoundField DataField="ID" HeaderText="ID"  HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone" Visible="false"/>
                                                                             
                                                <asp:BoundField DataField="LICENCIA" HeaderText="LICENCIA"  HeaderStyle-CssClass="gradeC center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone" HeaderStyle-HorizontalAlign="Center"/>
                                                <asp:BoundField DataField="NOMBRES" HeaderText="CHOFER"  HeaderStyle-CssClass="gradeC center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone" HeaderStyle-HorizontalAlign="Center"/>
                                                                            
                                                <asp:BoundField DataField="ID_EMPRESA" HeaderText="RUC"  HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>
                                                <asp:BoundField DataField="RAZON_SOCIAL" HeaderText="EMPRESA"  HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>
                                                    <asp:BoundField DataField="STATUS" HeaderText="ESTADO"  HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>
                                                    <asp:BoundField DataField="LICENCIA_EXPIRACION" DataFormatString="{0:yyyy/MM/dd HH:mm}" HeaderText="LICENCIA EXPIRACIÓN"  HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>
                                                <asp:BoundField DataField="CHOFER_SUSPENDIDO" DataFormatString="{0:yyyy/MM/dd HH:mm}" HeaderText="LICENCIA SUSPENDIDA"  HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>
                                                <asp:BoundField DataField="MOTIVO"  HeaderText="MOTIVO"  HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>
                                                    
                                                <asp:TemplateField HeaderText="INCLUIR" ItemStyle-CssClass="center hidden-phone">
                                            <ItemTemplate>
                                                <asp:Button runat="server" ID="IncreaseButton" Text="INCLUIR" CommandName="Seleccionar" CommandArgument='<%# Bind("ID") %>'  class="btn btn-outline-primary mr-4"  />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                  
                </div>
              </div>
            </div> 
          </ContentTemplate>               
           </asp:UpdatePanel>
          
        
        <div class="form-title">
                 2. Excluir Vehículos
        </div>
        <div> Puede buscar las placas de vehículos a excluir</div>
          <asp:UpdatePanel ID="UPVEHICULOS" runat="server" ChildrenAsTriggers="true">
          <ContentTemplate>
                <div class="form-row">    
                     <div class="form-group col-md-6">
                         <label for="inputEmail4">Chofer:</label>
                         <div class="d-flex">
                             <asp:TextBox ID="TxtVehiculo" runat="server"  MaxLength="150"  class="form-control"
                                 autocomplete="off"  
                                  onkeypress="return soloLetras(event,'abcdefghijklmnopqrstuvwxyz01234567890/')" Font-Bold="false" disabled ></asp:TextBox>
                              <a class="btn btn-outline-primary mr-4" target="popup" onclick="Buscar_Vehiculos();"  id="buscar_vehiculo" runat="server" visible="true">
                                        <span class='fa fa-search' style='font-size:24px' id="LookupVehiculo" clientidmode="Static"></span> </a>   
                         </div>
                     </div> 
                    <div class="form-group col-md-6">
                         <label for="inputEmail4">Motivo:</label>
                         <div class="d-flex">
                              <asp:TextBox ID="TxtMotivoVehiculo" runat="server" MaxLength="150"  class="form-control"
                                 autocomplete="off"  
                                  onkeypress="return soloLetras(event,'abcdefghijklmnopqrstuvwxyz01234567890/')" Font-Bold="false"  ></asp:TextBox>
                             <asp:Button ID="BtnAgregar" runat="server" class="btn btn-primary"  Text="Excluir" onclick="BtnAgregar_Click" />
                         </div>
                    </div>
                     <div class="form-group col-md-12">
                        <label for="inputEmail4">Buscar:</label>
                         <div class="d-flex"> 
                                <asp:TextBox ID="TxtBuscarVehiculo" runat="server"  MaxLength="50"  class="form-control"
                                 autocomplete="off"  
                                  onkeypress="return soloLetras(event,'abcdefghijklmnopqrstuvwxyz01234567890/')" Font-Bold="false"  ></asp:TextBox>
                                 <asp:Button ID="BtnBuscarVehiculo" runat="server"  class="btn btn-outline-primary mr-4" Text="Buscar" onclick="BtnBuscarVehiculo_Click" />
                         </div>
                     </div>

                </div> 
            
             <div class="">Detalle De Vehículos</div>
              <div class="form-row">
                <div class="form-group col-md-12">
                    <div class="table table-bordered invoice">   
                              <asp:GridView ID="tablePagination" runat="server" AutoGenerateColumns="False"  DataKeyNames="ID"
                                        GridLines="None" OnPageIndexChanging="tablePagination_PageIndexChanging" 
                                            OnRowCommand="tablePagination_RowCommand"
                                        PageSize="10"
                                        AllowPaging="True"
                                            CssClass="table table-bordered invoice">
                        <PagerStyle HorizontalAlign = "Right" CssClass="table table-bordered invoice"  />
                        <RowStyle  BackColor="#F0F0F0"  />
                        <alternatingrowstyle  BackColor="#FFFFFF"  />
                           
                                        <Columns>
                                                 
                                                <asp:BoundField DataField="ID" HeaderText="ID"  HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone" Visible="false"/>
                                                                             
                                            <asp:BoundField DataField="PLACA" HeaderText="VEHICULO"  HeaderStyle-CssClass="gradeC center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone" HeaderStyle-HorizontalAlign="Center"/>
                                                                            
                                            <asp:BoundField DataField="ID_EMPRESA" HeaderText="RUC"  HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>
                                            <asp:BoundField DataField="RAZON_SOCIAL" HeaderText="EMPRESA"  HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>
                                                <asp:BoundField DataField="TAG" HeaderText="TAG"  HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>
                                                <asp:BoundField DataField="STATUS" HeaderText="ESTADO"  HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>
                                                <asp:BoundField DataField="LICENCIA_EXPIRACION" DataFormatString="{0:yyyy/MM/dd HH:mm}" HeaderText="LICENCIA EXPIRACIÓN"  HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>
                                            <asp:BoundField DataField="CABEZAL_EXPIRACION" DataFormatString="{0:yyyy/MM/dd HH:mm}" HeaderText="EXPIRACIÓN PERMISO"  HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>
                                                <asp:BoundField DataField="MOTIVO"  HeaderText="MOTIVO"  HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>

                                            <asp:TemplateField HeaderText="INCLUIR" ItemStyle-CssClass="center hidden-phone">
                                        <ItemTemplate>
                                            <asp:Button runat="server" ID="IncreaseButton" Text="INCLUIR" CommandName="Seleccionar" CommandArgument='<%# Bind("ID") %>' class="btn btn-outline-primary mr-4"  />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                    </div> 
                </div>
              </div> 
             
              
            
            </ContentTemplate>
                <Triggers>
                <asp:AsyncPostBackTrigger ControlID="BtnAgregar" />
                </Triggers>
            </asp:UpdatePanel>
    

        
        </ContentTemplate>
                     <Triggers>
                         <asp:AsyncPostBackTrigger ControlID="BtnAgregar" />
                     </Triggers>
        </asp:UpdatePanel>



 
</div>
    <script type="text/javascript" src="lib/jquery/jquery.min.js"></script>
   <script type="text/javascript" src="lib/bootstrap/js/bootstrap.min.js"></script>
  <script type="text/javascript" src='lib/autocompletar/bootstrap3-typeahead.min.js'></script>
 
   
    <%--<script src="../Scripts/pages.js" type="text/javascript"></script>--%>

<script type="text/javascript">
    
          $(window).load(function () {
                            $("div.colapser").toggle(function () { $(this).removeClass("colapsa").addClass("expande"); $(this).next().hide(); }
                                  , function () { $(this).removeClass("expande").addClass("colapsa"); $(this).next().show(); });
        });
    </script>

<script type="text/javascript">

    function clearTextBox() {

         this.document.getElementById("IdVehiculo").value = "";
                this.document.getElementById("Placa").value = "";
        this.document.getElementById('<%= TxtVehiculo.ClientID %>').value = "";

          this.document.getElementById("IdChofer").value = "0";
                this.document.getElementById("Licencia").value = "";
                this.document.getElementById("NombreChofer").value = "";
                 this.document.getElementById('<%= TxtChofer.ClientID %>').value = "";
    }

    function Buscar_Empresas() {

        try {

            var id_linea = document.getElementById('<%=TxtLineaNaviera.ClientID %>').value;

            

            if (id_linea == '' || id_linea == null || id_linea == undefined) {
                alertify.alert('Advertencia', 'Debe seleccionar una líneas naviera').set('label', 'Aceptar');
                return false;
            }

            document.getElementById('<%=Txtempresa.ClientID %>').value = '';
            document.getElementById('<%=TxtVehiculo.ClientID %>').value = '';
            document.getElementById('<%=TxtChofer.ClientID %>').value = '';
            document.getElementById('<%=TxtBuscarVehiculo.ClientID %>').value = '';
          
            document.getElementById('IdEmpresa').value = '';
            document.getElementById('RucEmpresa').value = '';
            document.getElementById('NombreEmpresa').value = '';
            document.getElementById('IdVehiculo').value = '';
            document.getElementById('Placa').value = '';
            document.getElementById('IdChofer').value = '';
            document.getElementById('Licencia').value = '';
            document.getElementById('NombreChofer').value = '';


            window.open('../autorizaciones/lookup_empresas.aspx', 'name', 'width=1024,height=800');
        }
        catch (e) {
            alertify.alert('Error',e.Message).set('label', 'Aceptar');
        return false;
        }
    }

    function Buscar_Vehiculos()
    {

         var id_empresa = document.getElementById('<%=RucEmpresa.ClientID %>').value;
         var Desc_Empresa = document.getElementById('<%=Txtempresa.ClientID %>').value;
         var id_linea = document.getElementById('<%=TxtLineaNaviera.ClientID %>').value;
        
         if (id_linea == '' || id_linea == null || id_linea == undefined)
         {
              alertify.alert('Advertencia', 'Debe seleccionar una líneas naviera').set('label', 'Aceptar');
              return false;
        }

         if (id_empresa == '' || id_empresa == null || id_empresa == undefined)
         {
              alertify.alert('Advertencia', 'Debe seleccionar una empresa').set('label', 'Aceptar');
              return false;
         }
         if (Desc_Empresa == '' || Desc_Empresa == null || Desc_Empresa == undefined)
         {
              alertify.alert('Advertencia', 'Debe seleccionar una empresa').set('label', 'Aceptar');
              return false;
        }

        window.open('../autorizaciones/lookup_vehiculos.aspx?ID_EMPRESA='+id_empresa, 'name', 'width=1024,height=800');

    }

    function Buscar_Chofer()
    {

         var id_empresa = document.getElementById('<%=RucEmpresa.ClientID %>').value;
        var Desc_Empresa = document.getElementById('<%=Txtempresa.ClientID %>').value;
         var id_linea = document.getElementById('<%=TxtLineaNaviera.ClientID %>').value;
        
         if (id_linea == '' || id_linea == null || id_linea == undefined)
         {
              alertify.alert('Advertencia','Debe seleccionar una líneas naviera').set('label', 'Aceptar');
              return false;
        }

         if (id_empresa == '' || id_empresa == null || id_empresa == undefined)
         {
              alertify.alert('Advertencia','Debe seleccionar una empresa').set('label', 'Aceptar');
              return false;
         }
         if (Desc_Empresa == '' || Desc_Empresa == null || Desc_Empresa == undefined)
         {
              alertify.alert('Advertencia','Debe seleccionar una empresa').set('label', 'Aceptar');
              return false;
        }

        window.open('../autorizaciones/lookup_choferes.aspx?ID_EMPRESA='+id_empresa, 'name', 'width=1024,height=800');

    }
    function popupCallback_Empresa(lookup_get_empresa)
    {
     
            if (lookup_get_empresa.sel_g_id_empresa != null) {
                this.document.getElementById("IdEmpresa").value = lookup_get_empresa.sel_g_id_empresa;
                this.document.getElementById("RucEmpresa").value = lookup_get_empresa.sel_g_ruc;
                this.document.getElementById("NombreEmpresa").value = lookup_get_empresa.sel_g_nombre;
               
                this.document.getElementById('<%= Txtempresa.ClientID %>').value = lookup_get_empresa.sel_g_ruc + " - " + lookup_get_empresa.sel_g_nombre;
                
            }
            else {
                 this.document.getElementById("IdEmpresa").value = "0";
                 this.document.getElementById("RucEmpresa").value = "";
                 this.document.getElementById("NombreEmpresa").value = "";
                 this.document.getElementById('<%= Txtempresa.ClientID %>').value = "";
          
            }
    
    }

    function popupCallback_Vehiculo(lookup_get_vehiculo)
    {
     
        if (lookup_get_vehiculo.sel_g_id_vehiculo != null) {

                this.document.getElementById("IdVehiculo").value = lookup_get_vehiculo.sel_g_id_vehiculo;
                this.document.getElementById("Placa").value = lookup_get_vehiculo.sel_g_placa;
                this.document.getElementById('<%= TxtVehiculo.ClientID %>').value = lookup_get_vehiculo.sel_g_placa;
                
            }
            else {
                this.document.getElementById("IdVehiculo").value = "";
                this.document.getElementById("Placa").value = "";
                 this.document.getElementById('<%= TxtVehiculo.ClientID %>').value = "";
          
            }
    
    } 

    function popupCallback_Choferes(lookup_get_choferes)
    {
     
            if (lookup_get_choferes.sel_g_id_chofer != null) {
                this.document.getElementById("IdChofer").value = lookup_get_choferes.sel_g_id_chofer;
                this.document.getElementById("Licencia").value = lookup_get_choferes.sel_g_licencia;
                this.document.getElementById("NombreChofer").value = lookup_get_choferes.sel_g_nombres;
                this.document.getElementById('<%= TxtChofer.ClientID %>').value = lookup_get_choferes.sel_g_licencia + " - " + lookup_get_choferes.sel_g_nombres;
                
            }
            else {
                this.document.getElementById("IdChofer").value = "0";
                this.document.getElementById("Licencia").value = "";
                this.document.getElementById("NombreChofer").value = "";
                 this.document.getElementById('<%= TxtChofer.ClientID %>').value = "";
          
        }

           
    
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
                this.document.getElementById("AuxLinea_Naviera").value = lookup_get_linea.sel_g_id_linea;
               
            }
            else
            {
                
                 this.document.getElementById('<%= TxtLineaNaviera.ClientID %>').value = "";
            }

        __doPostBack();
    }


</script>



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
