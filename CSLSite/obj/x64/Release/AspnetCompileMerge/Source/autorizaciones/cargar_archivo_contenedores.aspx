<%@ Page  Title="Envió de Archivo de Vehículo y Choferes"  Language="C#" MasterPageFile="~/site.Master" AutoEventWireup="true" CodeBehind="cargar_archivo_contenedores.aspx.cs" Inherits="CSLSite.autorizaciones.cargar_archivo_contenedores" %>
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

      <style type="text/css">
        body
        {
            /*font-family: Arial;
            font-size: 10pt;*/
        }
        .modal
        {
            position: fixed;
            z-index: 999;
            height: 100%;
            width: 100%;
            top: 0;
            background-color: Black;
            filter: alpha(opacity=60);
            opacity: 0.6;
            -moz-opacity: 0.8;
        }

        .modalBackground
        {
            background-color: Black;
            filter: alpha(opacity=60);
            opacity: 0.6;
        }
        .modalPopup
        {
            background-color: #FFFFFF;
            width: 500px;
            border: 3px solid #FF3720;
            padding: 0;
        }
        .modalPopup .header
        {
            background-color: #2FBDF1;
            height: 30px;
            color: White;
            line-height: 30px;
            text-align: center;
            font-weight: bold;
        }
        .modalPopup .body
        {
            min-height: 50px;
            line-height: 30px;
            text-align: center;
            font-weight: bold;
            margin-bottom: 5px;
        }
          </style>
    
 <script type="text/javascript">
        $(document).ready(function () {
            $('#<%= tablePagination.ClientID %>').dataTable();
        });
       
    </script>

 
 </asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="placebody" runat="server">


   <input id="zonaid" type="hidden" value="7" />
    <input id="Referencia" type="hidden" value="" runat="server" clientidmode="Static" />
         <input id="ruta" type="hidden" value="" runat="server" clientidmode="Static" />
    <asp:ToolkitScriptManager ID="tkscata" runat="server"></asp:ToolkitScriptManager>

    <div class="mt-4">         
        <nav class="mt-4" aria-label="breadcrumb">
          <ol class="breadcrumb">
            <li class="breadcrumb-item" id="opcion_principal" runat="server"><a href="#">Autorizaciones</a></li>
            <li class="breadcrumb-item active" aria-current="page" id="sub_opcion" runat="server">Orden de Retiro de Vacíos</li>
          </ol>
        </nav>
   </div>
 
<div class="dashboard-container p-4" id="cuerpo" runat="server">

    <div class="form-title">
         DATOS DEL ARCHIVO:
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
              <label for="inputEmail4"></label>
         </div>
         <div class="form-group col-md-6">
              <label for="inputEmail4">Orden de Retiro #:</label>
              <asp:UpdatePanel ID="UpdatePanel3" runat="server"   >  
                 <ContentTemplate>
                    <asp:TextBox ID="TxtNumeroOrden" runat="server"  MaxLength="150"  class="form-control" autocomplete="off"  disabled ></asp:TextBox>
                    </ContentTemplate>
                     <Triggers>
                     <asp:AsyncPostBackTrigger ControlID="BtnGrabar" />    
                    </Triggers>
                  </asp:UpdatePanel>  
         </div> 
         <div class="form-group col-md-6">
             <label for="inputEmail4">Fecha Proceso:</label>
              <asp:TextBox ID="TxtFechaProceso" runat="server"  MaxLength="150"  class="form-control" autocomplete="off"  disabled ></asp:TextBox>   
         </div>
         <div class="form-group col-md-6">
              <label for="inputEmail4"># Autorización Aduana (NAA):</label>
               <asp:TextBox ID="Txtautorizacion" runat="server"  MaxLength="110"  class="form-control" autocomplete="off"
                 onkeypress="return soloLetras(event,'abcdefghijklmnopqrstuvwxyz01234567890')"></asp:TextBox>
         </div>
         <div class="form-group col-md-6">
              <label for="inputEmail4">Cantidad Contenedores Autorizados en el (NAA):</label>
              <asp:TextBox ID="TxtCantidad" runat="server"  MaxLength="4"  class="form-control" autocomplete="off"
                 onkeypress="return soloLetras(event,'01234567890')"></asp:TextBox>
         </div>
         <div class="form-group col-md-6">
              <label for="inputEmail4">Referencia:</label>
              <div class="d-flex"> 
                   <asp:TextBox ID="TxtReferencia" runat="server"  MaxLength="150"  class="form-control"
                     autocomplete="off"  
                      onkeypress="return soloLetras(event,'abcdefghijklmnopqrstuvwxyz01234567890')" disabled></asp:TextBox>    
                    <a class="btn btn-outline-primary mr-4" target="popup" onclick="window.open('../autorizaciones/lookup_naves.aspx','name','width=1024,height=800')"  id="llave1" runat="server" >
                                <span class='fa fa-search' style='font-size:24px' id="BtnBuscarNave" clientidmode="Static"></span> </a>   
                    
              </div>
         </div>
        <div class="form-group col-md-6">
              <label for="inputEmail4">Seleccionar Archivo CSV:</label>
              <div class="d-flex"> 
                   <asp:AsyncFileUpload ID="fsuploadarchivo" class="uploader form-control" runat="server"   title="Escoja el archivo con formato indicado .CSV"  style=" font-size:small" visible="true" />
                    <asp:AsyncFileUpload ID="AsyncFileUpload1" class="uploader form-control" runat="server"   title="Escoja el archivo con formato indicado .pdf"  style=" font-size:small" visible="false" /> 
    
         
                    <asp:Label ID="LblRuta" runat="server" Text="" Font-Bold="true" ForeColor="Red" class="form-control"></asp:Label>
              </div> 
         </div>

    </div>
    <div class="alert alert-danger" id="Div1" runat="server" clientidmode="Static" ><b>Estimado Cliente, revise que la información ingresada sea correcta antes de continuar.</b><br/>
                             <b>NOTA:</b> Los datos / información ingresados son de exclusiva responsabilidad del usuario que lo realiza, por lo que es importante verificar antes de completar el proceso.

    </div>

     <div class="row">
            <div class="col-md-12 d-flex justify-content-center">
                     <img alt="loading.." src="../shared/imgs/loading.gif" height="32px" width="32px" id="loader" class="nover"  />
                   <asp:UpdatePanel ID="UpdateRuta" runat="server"   >  
                    <ContentTemplate>
                    <asp:Button ID="BtnAgregar" runat="server"  class="btn btn-primary"  
                        Text="Validar Archivo" onclick="BtnAgregar_Click"  OnClientClick="return prepareObject()"/>
                         <asp:Button ID="BtnGrabar" runat="server" class="btn btn-outline-primary mr-4"  
                        Text="Grabar Archivo" onclick="BtnGrabar_Click"  OnClientClick="return confirmacion()" />
                     </ContentTemplate>
                   <Triggers>
                     <asp:AsyncPostBackTrigger ControlID="BtnAgregar" />   
                        <asp:AsyncPostBackTrigger ControlID="BtnGrabar" />
                    </Triggers>
                  </asp:UpdatePanel>  
            </div>
    </div>

    <div class="form-row"> 
          <div class="form-group col-md-12">
            <asp:UpdatePanel ID="UpdatePanel2" runat="server"   ChildrenAsTriggers="true" UpdateMode="Conditional">  
                             <ContentTemplate>

                    <input id="idlin" type="hidden" runat="server" clientidmode="Static" />
                    <input id="diponible" type="hidden" runat="server" clientidmode="Static" />
                    <div class="msg-alerta" id="alerta" runat="server" style=" display:none" ></div>
       
                      <div class="seccion">                
                       <div class="accion">
                         <div class="cataresult" >
             
                        
                        <div id="xfinder1" runat="server" visible="true" >

                            <div class="findresult" >
                              <div class="booking" >
                                  
                                 <div id="sinresultado" runat="server" class="alert alert-danger"></div>

                                                    <div class="bokindetalle">
                                                       <h3 runat="server" id="mensaje_proceso" visible="false">DETALLE DE CONTENEDORES: <asp:Label ID="lblTotContenedores" runat="server" BorderStyle="None" 
                                                            Font-Bold="True" Font-Names="Arial"  ForeColor="Black" Width="400px"
                                                            Text="[tot]"></asp:Label></h3>
                                                        <h3 runat="server" id="mensaje_errores" visible="false">DETALLE DE ERRORES:</h3>
                        <%--  <div class="content-panel">--%>
                              <div class="table table-bordered invoice">              
                                <div align="center" runat="server" id="div_error_n4" style="height:210px;" visible="false">
                                    <textarea style="height:200px; overflow:auto; width: 882px; font-size:small" 
                                    rows="20" cols="20" id="text_cont" runat="server" visible="true" class="tinymce form-control" > 
                                    </textarea>
                                </div> 
                                <br/>
                                   <asp:GridView ID="tablePagination" runat="server" AutoGenerateColumns="False"  
                                                                                        GridLines="None" OnPageIndexChanging="tablePagination_PageIndexChanging" 
                                                                            
                                                                                       PageSize="10"
                                                                                       AllowPaging="True"
                                                                                        CssClass="table table-bordered invoice">
                                                                                        <PagerStyle HorizontalAlign = "Center"  />
                                                                       <RowStyle  BackColor="#F0F0F0" />
                                                                    <alternatingrowstyle  BackColor="#FFFFFF" />
                                                                                        <Columns>
                                                 
                                                                                             <asp:BoundField DataField="CNTR_CONTAINER" HeaderText="Contenedor"    />
                                                                                            <asp:BoundField DataField="CNTR_NUMAUTSENAE" HeaderText="N° Aut. Senae"   />                   
                                                                                            <asp:BoundField DataField="CNTR_LINE" HeaderText="Linea Naviera"   />
                                                                                            <asp:BoundField DataField="CNTR_VEPR_REFERENCE" HeaderText="Referencia"  />
                                                                                             <asp:BoundField DataField="CNTR_CONSECUTIVO" HeaderText="Consecutivo"   Visible="false"/>
                                                                                           <asp:BoundField DataField="CNTR_GROUP" HeaderText="Grupo"   Visible="false"/>
                                                                                             <asp:BoundField DataField="REF_FINAL" HeaderText="Referencia Final"  Visible="false"/>

                                                                                        </Columns>
                                                                                    </asp:GridView>
                              </div><!--adv-table-->
             
                       <%--  </div><!--content-panel-->--%>

                              </div>
                             </div>
                             </div> 
                  
                          </div>
                        </div>       
                       </div>

                   </ContentTemplate>
                              <Triggers>
                                 <asp:AsyncPostBackTrigger ControlID="BtnAgregar" />    
                                </Triggers>
                              </asp:UpdatePanel>  

        </div>

    </div>


         <asp:ModalPopupExtender ID="mpeLoading" runat="server" BehaviorID="idmpeLoading"
        PopupControlID="pnlLoading" TargetControlID="btnLoading" EnableViewState="false"
        DropShadow="true" BackgroundCssClass="modalBackground" />

    <asp:Panel ID="pnlLoading" runat="server"  HorizontalAlign="Center"
        CssClass="modalPopup" align="center"  EnableViewState="false" Style="display: none">
        <br />Procesando...
         <div class="body">
             <asp:UpdatePanel ID="msgload" runat="server" UpdateMode="Conditional"  >
               <ContentTemplate>
           <div align="center">   
              
               <asp:Image ID="loading" runat="server" ImageUrl="lib/imgs/loading.gif"  Visible="true" Width="40" Height="40" />
             
            </div>
                  
            <br/>
            Estimado Cliente, por favor esperar unos segundos.... <br/>
  
           <br />
                     </ContentTemplate>
                 
            </asp:UpdatePanel>
        </div>


    </asp:Panel>
    <asp:Button ID="btnLoading" runat="server" Style="display: none"  />

</div>

         <script src="../Scripts/pages.js" type="text/javascript"></script>
   <%-- <script type="text/javascript" src="lib/jquery/jquery.min.js"></script>
   <script type="text/javascript" src="lib/bootstrap/js/bootstrap.min.js"></script>--%>

 
   
 

<script type="text/javascript">

var mpeLoading;
function initializeRequest(sender, args){
    mpeLoading = $find('idmpeLoading');
    mpeLoading.show();
    mpeLoading._backgroundElement.style.zIndex += 10;
    mpeLoading._foregroundElement.style.zIndex += 10;
}
    function endRequest(sender, args) {
         $find('idmpeLoading').hide();

    }

Sys.WebForms.PageRequestManager.getInstance().add_initializeRequest(initializeRequest);
Sys.WebForms.PageRequestManager.getInstance().add_endRequest(endRequest); 

if (typeof(Sys) !== 'undefined') Sys.Application.notifyScriptLoaded();
    $find('idmpeLoading').hide();

</script>

<script type="text/javascript">


     function confirmacion()
    {
        var mensaje;
        var opcion = confirm("Estimado cliente, está seguro que desea grabar la transacción. ?");
        if (opcion == true)
        {
           
            return true;
        } else
        {
           
	         return false;
        }

       
    }

function mostrarloader()
    {
       document.getElementById("ImgCarga").className = 'ver';  
    }


    function clearTextBox() {
    
         document.getElementById('TxtAsunto').value = '';
    }

     function Buscar_Naves() {

        var id_linea = document.getElementById('<%=TxtLineaNaviera.ClientID %>').value;
        
         if (id_linea == '' || id_linea == null || id_linea == undefined)
         {
              alert('Debe seleccionar una líneas naviera');
              return false;
         }

        window.open('../autorizaciones/lookup_naves.aspx', 'name', 'width=850,height=480');

    }

      function Buscar_Lineas() {
  
        window.open('../autorizaciones/lookup_linea_naviera.aspx', 'name', 'width=850,height=480');

    }

     function GolinkArchivo() {

             window.open('../autorizaciones/lookup_subir_archivo.aspx', 'name', 'width=850,height=480,menubar=NO,scrollbars=NO,resizable=NO,toolbars=NO,Titlebar=NO,status=no,help=no,minimize=no,unadorned=on,maximize=no');
          
    }

    function popupCallback_Naves(lookup_get_naves)
    {
     
        if (lookup_get_naves.sel_g_id_numero != null) {
                this.document.getElementById('<%= TxtReferencia.ClientID %>').value = lookup_get_naves.sel_g_nave;
                //this.document.getElementById("TxtReferencia").value = lookup_get_naves.sel_g_nave;
                 this.document.getElementById("Referencia").value = lookup_get_naves.sel_g_nave;
            }
            else {
               
                
                 this.document.getElementById('<%= TxtReferencia.ClientID %>').value = "";
                this.document.getElementById("Referencia").value = "";
            }
    
    }

     function popupCallback_Lineas(lookup_get_linea)
    {
     
         if (lookup_get_linea.sel_g_id_linea != null) {
                 this.document.getElementById('<%= TxtLineaNaviera.ClientID %>').value = lookup_get_linea.sel_g_id_linea;
                //this.document.getElementById("TxtLineaNaviera").value = lookup_get_linea.sel_g_id_linea;
               
            }
            else {
                
                 this.document.getElementById('<%= TxtLineaNaviera.ClientID %>').value = "";
            }

        

    }

    function popupCallback_Archivo(lookup_archivo) {
     
            if (lookup_archivo.sel_Ruta != null) {
             
               <%-- this.document.getElementById('<%= TxtArchivo.ClientID %>').value = lookup_archivo.sel_Ruta;   --%> 
            }
            else {
               
               <%-- this.document.getElementById('<%= TxtArchivo.ClientID %>').value = "";   --%> 
            }
    
    }

    function prepareObject() {

        var vals = document.getElementById('<%=TxtLineaNaviera.ClientID %>').value;
                if (vals == '' || vals == null || vals == undefined) {
                    alert('* Datos de Líneas Naviera: *\n * Seleccione Líneas Naviera *');
                    document.getElementById('<%=Txtautorizacion.ClientID %>').focus();
                    
                    return false;
        }

        var vals = document.getElementById('<%=Txtautorizacion.ClientID %>').value;
                if (vals == '' || vals == null || vals == undefined) {
                    alert('* Datos de Autorización: *\n * Escriba la Autorización *');
                    document.getElementById('<%=Txtautorizacion.ClientID %>').focus();
                    
                    return false;
        }
        var vals = document.getElementById('<%=TxtReferencia.ClientID %>').value;
                if (vals == '' || vals == null || vals == undefined) {
                    alert('* Datos de Referencia: *\n * Seleccione la Referencia *');
                    document.getElementById('<%=TxtReferencia.ClientID %>').focus();
                    
                    return false;
        }
           
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

