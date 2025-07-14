<%@ Page  Title="LISTADO DE ORDENES DE RETIRO."  Language="C#" MasterPageFile="~/site.Master" AutoEventWireup="true" CodeBehind="listado_orden_retiro.aspx.cs" Inherits="CSLSite.autorizaciones.listado_orden_retiro" %>
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
            <li class="breadcrumb-item active" aria-current="page" id="sub_opcion" runat="server">Listado de Ordenes de Retiro</li>
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
            <label for="inputEmail4">Referencia:</label> 
              <asp:TextBox ID="TxtReferencia" runat="server"  MaxLength="50"  class="form-control" autocomplete="off"  onkeypress="return soloLetras(event,'abcdefghijklmnopqrstuvwxyz01234567890/-')" ></asp:TextBox>   
        </div> 
         <div class="form-group col-md-6"> 
           <label for="inputAddress">Fecha Desde:<span style="color: #FF0000; font-weight: bold;"></span></label>
               <asp:TextBox ID="desded" runat="server"  CssClass="datetimepicker form-control" 
             onkeypress="return soloLetras(event,'01234567890/')" 
                 onblur="valDate(this,true,valdate);" ClientIDMode="Static"></asp:TextBox>
          </div>
          <div class="form-group col-md-6"> 
           <label for="inputAddress">Fecha Hasta:<span style="color: #FF0000; font-weight: bold;"></span></label>
               <asp:TextBox ID="hastad" runat="server" ClientIDMode="Static"  MaxLength="15" CssClass="datetimepicker form-control" 
             onkeypress="return soloLetras(event,'01234567890/')" 
                  onblur="valDate(this,true,valdate);"></asp:TextBox>
               <span id="valdate" class="validacion"></span>
          </div>

     </div>
     <div class="row">
            <div class="col-md-12 d-flex justify-content-center">
                 <asp:Button ID="BtnBuscar" runat="server" Text="Buscar"  
                     class="btn btn-primary" 
                    onclick="BtnBuscar_Click"  OnClientClick="return mostrarloader()" />
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
       
                       
                   
                     <div class="bokindetalle" style=" width:100%; overflow:auto">       
                          <asp:UpdatePanel ID="UPVEHICULOS" runat="server" ChildrenAsTriggers="true">
                                     <ContentTemplate>

                                                        <div id="sinresultado" runat="server" class="alert alert-warning"></div>
                                                       <div class="table table-bordered invoice" runat="server" id="ordenes" visible="true">
                                                                   <asp:GridView ID="tablePagination" runat="server" AutoGenerateColumns="False"  DataKeyNames="ID"
                                                                                        GridLines="None" OnPageIndexChanging="tablePagination_PageIndexChanging" 
                                                                                         OnRowCommand="tablePagination_RowCommand"
                                                                                       PageSize="10"
                                                                                       AllowPaging="True"
                                                                                        CssClass="table table-bordered invoice">
                                                                      <PagerStyle HorizontalAlign = "Center" CssClass="table table-bordered invoice"  />
                                                                       <RowStyle  BackColor="#F0F0F0"  />
                                                                       <alternatingrowstyle  BackColor="#FFFFFF"  />
                           
                                                                                        <Columns>
                                                                                             <asp:BoundField DataField="ID" HeaderText="#ORDEN"/>
                                                                                            <asp:BoundField DataField="FECHA" DataFormatString="{0:yyyy/MM/dd HH:mm}" HeaderText="FECHA"   HeaderStyle-HorizontalAlign="Center"/>
                                                                                            <asp:BoundField DataField="AUTORIZACION" HeaderText="AUTORIZACION (NAA)"  />
                                                                                            <asp:BoundField DataField="REFERENCIA" HeaderText="REFERENCIA"  />
                                                                                             <asp:BoundField DataField="TOTAL_CONTENEDORES"  HeaderText="# CONTENEDORES A RETIRAR" HeaderStyle-HorizontalAlign="Center"   ItemStyle-HorizontalAlign="Center"/>
                                                                                            <asp:BoundField DataField="TOTAL_PENDIENTES"  HeaderText="# CONTENEDORES POR RETIRAR"  />
                                                                                             <asp:BoundField DataField="TOTAL_PROCESADOS"  HeaderText="# CONTENEDORES RETIRADOS"  />
                                                                                            <asp:BoundField DataField="ESTADO_TRANSACCION"  HeaderText="ESTADO"  />
                                                                                            <asp:BoundField DataField="USUARIO_CRE" HeaderText="USUARIO/CREA"  />
                                                                                            <asp:TemplateField HeaderText="VISUALIZAR" >
                                                                                        <ItemTemplate>
                                                                                            <a href="../autorizaciones/listado_orden_retiro_lookup.aspx?id=<%#securetext(Eval("ID")) %>"  target="_blank"><button type="button" class="btn btn-outline-primary mr-4"><i class="fa fa-print"></i> Visualizar</button></a>
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>
                                                                                        </Columns>
                                                                                    </asp:GridView>

                                                       </div><!--content-panel-->
                                           

                              </ContentTemplate>
                                     <Triggers>
                                     <asp:AsyncPostBackTrigger ControlID="BtnBuscar" />
                                     </Triggers>
                                 </asp:UpdatePanel>
                      </div>
                    
                 

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


     <script src="../Scripts/jquery.datetimepicker.js" type="text/javascript"></script>
    
     <script type="text/javascript">
        $(document).ready(function () {
              $('.datetimepicker').datetimepicker().datetimepicker({ lang: 'es', timepicker: false, closeOnDateSelect: true, format: 'd/m/Y' });
          });
     </script>
   
 

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


 </asp:Content>
