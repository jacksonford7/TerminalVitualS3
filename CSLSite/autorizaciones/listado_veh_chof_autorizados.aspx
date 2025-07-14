<%@ Page  Title="Reporte de vehículos y Choferes Autorizados"  Language="C#" MasterPageFile="~/site.Master" AutoEventWireup="true" CodeBehind="listado_veh_chof_autorizados.aspx.cs" Inherits="CSLSite.autorizaciones.listado_veh_chof_autorizados" %>
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
      <link href="../shared/estilo/jquery.datetimepicker.css" rel="stylesheet" type="text/css" />

 

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
            <li class="breadcrumb-item" id="opcion_principal" runat="server"><a href="#">Reportes</a></li>
            <li class="breadcrumb-item active" aria-current="page" id="sub_opcion" runat="server">Reporte de Vehículos y Choferes Autorizados</li>
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
               <asp:DropDownList runat="server" ID="CboEmpresas"  class="form-control"  AutoPostBack="false"   ></asp:DropDownList>  
            </div> 
           <div class="form-group col-md-6">
                <label for="inputEmail4">Tipo:</label> 
                 <asp:DropDownList runat="server" ID="CboTipo"  class="form-control" AutoPostBack="false"    ></asp:DropDownList>
            </div> 
            <div class="form-group col-md-6">
                <label for="inputEmail4">Estado:</label> 
                  <asp:DropDownList runat="server" ID="CboFiltro"  class="form-control"  AutoPostBack="false"  ></asp:DropDownList>
            </div> 

      </div>
        <div class="row">
            <div class="col-md-12 d-flex justify-content-center">
                 <asp:Button ID="BtnBuscar"  runat="server"    class="btn btn-primary"  Text="Buscar" onclick="BtnBuscar_Click" 
                 OnClientClick="return mostrarloader()"/>
                &nbsp;&nbsp;
                  <img alt="loading.." src="lib/imgs/loading.gif" height="32px" width="32px"  id="ImgCarga"  class="nover"  />
         </div>
      </div>
      <div class="form-row">
        <div class="form-group col-md-12">
        <asp:UpdatePanel ID="upresult" runat="server" ChildrenAsTriggers="true">
        <ContentTemplate>

        <input id="idlin" type="hidden" runat="server" clientidmode="Static" />
        <input id="diponible" type="hidden" runat="server" clientidmode="Static" />
        <div class="alert alert-warning" id="alerta" runat="server" style=" display:none" ></div>
       
              <asp:UpdatePanel ID="UPVEHICULOS" runat="server" ChildrenAsTriggers="true">
              <ContentTemplate>
               <%-- <div class="form-group col-md-12">--%>
                      <div id="sinresultado" runat="server" class="alert alert-warning"></div>
                      <div class="alert alert-warning" id="banmsg_det" runat="server" clientidmode="Static"><b>Estimado Cliente,</b> si desea regularizar la información del conductor o del vehículo por favor contactarse con el área de Permisos y Credenciales a través del correo <a href="mailto:permisosycredenciales@cgsa.com.ec">permisosycredenciales@cgsa.com.ec</a>  o a los teléfonos (04) 6006300 – 3901700 opción #1</div></div>
                    <div  class="form-group col-md-12" runat="server" id="vehiculo" visible="true">
                                <rsweb:reportviewer ID="ReportViewer1" runat="server" Font-Names="Verdana" 
                                    Font-Size="8pt" InteractiveDeviceInfos="(Colección)" 
                                    WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt" 
                                    Visible="true"   >
                                    <LocalReport ReportPath="autorizaciones\Rpt_Vehiculos_Autorizados.rdlc">
                                        <DataSources>
                                            <rsweb:ReportDataSource DataSourceId="SqlDataSource_Detalle" 
                                                Name="RVA_REPORTE_CHOFERES_VEHICULO_AUTORIZADOS" />
                                        </DataSources>
                                    </LocalReport>
                                </rsweb:reportviewer>
                                        <asp:SqlDataSource ID="SqlDataSource_Detalle" runat="server" ConnectionString="<%$ ConnectionStrings:N5 %>" SelectCommand="RVA_REPORTE_CHOFERES_VEHICULO_AUTORIZADOS" SelectCommandType="StoredProcedure">        
                                            <SelectParameters>  
                                            <asp:SessionParameter Type="String"  Name="XMLEmpresas"   SessionField="XMLEmpresas" />
                                            <asp:SessionParameter  Type="String" Name="XMLVehiculo"  SessionField="XMLVehiculo" />
                                            <asp:SessionParameter  Type="String" Name="XMLChofer"  SessionField="XMLChofer" />
                                            <asp:SessionParameter  Type="String" Name="XMLMensaje"  SessionField="XMLMensaje"  />
                                            <asp:SessionParameter  Name="TIPO" SessionField="TIPO" DbType="Int32"   />  
                                            <asp:SessionParameter  Name="AUTORIZADOS" SessionField="AUTORIZADOS" DbType="Int32"   /> 
                                        </SelectParameters>
                                        </asp:SqlDataSource>          

                    </div><!--content-panel-->
                    <div  class="form-group col-md-12" runat="server" id="choferes" visible="false">
                                <rsweb:reportviewer ID="ReportViewer2" runat="server" Font-Names="Verdana" 
                                    Font-Size="8pt" InteractiveDeviceInfos="(Colección)" 
                                    WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt" 
                                    Visible="true"   >
                                    <LocalReport ReportPath="autorizaciones\Rpt_Choferes_Autorizados.rdlc">
                                        <DataSources>
                                            <rsweb:ReportDataSource DataSourceId="SqlDataSource_Detalle2" 
                                                Name="RVA_REPORTE_CHOFERES_VEHICULO_AUTORIZADOS" />
                                        </DataSources>
                                    </LocalReport>
                                </rsweb:reportviewer>
                                        <asp:SqlDataSource ID="SqlDataSource_Detalle2" runat="server" ConnectionString="<%$ ConnectionStrings:N5 %>" SelectCommand="RVA_REPORTE_CHOFERES_VEHICULO_AUTORIZADOS" SelectCommandType="StoredProcedure">        
                                            <SelectParameters>  
                                            <asp:SessionParameter Type="String"  Name="XMLEmpresas"   SessionField="XMLEmpresas" />
                                            <asp:SessionParameter  Type="String" Name="XMLVehiculo"  SessionField="XMLVehiculo" />
                                            <asp:SessionParameter  Type="String" Name="XMLChofer"  SessionField="XMLChofer" />
                                            <asp:SessionParameter  Type="String" Name="XMLMensaje"  SessionField="XMLMensaje"  />
                                            <asp:SessionParameter  Name="TIPO" SessionField="TIPO" DbType="Int32"   />  
                                                <asp:SessionParameter  Name="AUTORIZADOS" SessionField="AUTORIZADOS" DbType="Int32"   /> 
                                    </SelectParameters>
                                        </asp:SqlDataSource>          

                    </div><!--content-panel-->
              <%--  </div>--%>
              </ContentTemplate>
                <Triggers>
                <asp:AsyncPostBackTrigger ControlID="BtnBuscar" />
                </Triggers>
            </asp:UpdatePanel>
           
           
        

        </ContentTemplate>
                     <Triggers>
                         <asp:AsyncPostBackTrigger ControlID="BtnBuscar" />
                     </Triggers>
        </asp:UpdatePanel>
        
        </div>
      </div>

 
</div> 

      <script src="../Scripts/pages.js" type="text/javascript"></script>
     <script type="text/javascript" src="../lib/jquery/jquery.min.js"></script>
     <script type="text/javascript" src="../lib/bootstrap/js/bootstrap.min.js"></script>

 
   
 

<script type="text/javascript">

    function mostrarloader() {

        try {
           
                document.getElementById("ImgCarga").className = 'ver';
                
            } catch (e) {
                alertify.alert('Advertencia','Lamentamos comunicarle que ha ocurrido el siguiente problema:\n' + e.Message).set('label', 'Aceptar');
            }
        }

    function ocultarloader() {
        try {

           
                document.getElementById("ImgCarga").className = 'nover';
           
             } catch (e) {
                alertify.alert('Advertencia','Lamentamos comunicarle que ha ocurrido el siguiente problema:\n' + e.Message).set('label', 'Aceptar');
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

