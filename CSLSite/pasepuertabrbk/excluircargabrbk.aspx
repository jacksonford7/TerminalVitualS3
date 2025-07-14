<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="excluircargabrbk.aspx.cs" Inherits="CSLSite.excluircargabrbk" %>
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


 <link href="../css/calendario_ajax.css" rel="stylesheet"/>


 <script src="../js/locationpicker.jquery.js" type="text/javascript"></script>
 <script type="text/javascript" src="https://maps.google.com/maps/api/js?key=AIzaSyAuC0-LIk1uTDZPg9pz7ctQQmR-4IPP0zY"></script>


 
  <style type="text/css">
        body2
        {
            font-family: Arial;
            font-size: 10pt;
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

</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="placebody" runat="server" >
    
    <asp:ToolkitScriptManager ID="tkscata" runat="server"></asp:ToolkitScriptManager>

     <script type="text/javascript">
              Sys.Application.add_load(Calendario); 
            </script>  
  <div id="div_BrowserWindowName" style="visibility:hidden;">
    <asp:HiddenField ID="hf_BrowserWindowName" runat="server" />
    <asp:HiddenField ID="hf_idsolicitudgenerada" runat="server" />
  </div>
     <asp:HiddenField ID="manualHide" runat="server" />

      <div class="mt-4">         
        <nav class="mt-4" aria-label="breadcrumb">
          <ol class="breadcrumb">
            <li class="breadcrumb-item" id="opcion_principal" runat="server"><a href="#">BREAK BULK</a></li>
            <li class="breadcrumb-item active" aria-current="page" id="sub_opcion" runat="server">EXCLUIR CARGA/TURNOS ESPECIALES (BRBK)</li>
          </ol>
        </nav>
      </div>

<div class="dashboard-container p-4" id="cuerpo" runat="server"> 
     <div class="form-title">
           DATOS DE LA CARGA
     </div>

     <asp:UpdatePanel ID="UPCARGA" runat="server"  UpdateMode="Conditional" ChildrenAsTriggers="true">
        <ContentTemplate>
			<div class="form-row">
                <div class="form-group col-md-2">
                <label for="inputZip">MRN<span style="color: #FF0000; font-weight: bold;">*</span></label>
                <asp:TextBox ID="TXTMRN" runat="server" class="form-control" MaxLength="16"  onkeypress="return soloLetras(event,'abcdefghijklmnopqrstuvwxyz0123456789-_')"  
                                    placeholder="MRN"></asp:TextBox>
            </div>
                <div class="form-group col-md-2">
                    <label for="inputZip">MSN<span style="color: #FF0000; font-weight: bold;">*</span></label>
                    <asp:TextBox ID="TXTMSN" runat="server" class="form-control"  MaxLength="4"  onkeypress="return soloLetras(event,'abcdefghijklmnopqrstuvwxyz0123456789-_')" 
                                    placeholder="MSN"></asp:TextBox>
                </div>
            <div class="form-group col-md-2">
                    <label for="inputZip">HSN<span style="color: #FF0000; font-weight: bold;">*</span></label>
                    <asp:TextBox ID="TXTHSN" runat="server" class="form-control"  MaxLength="4"  onkeypress="return soloLetras(event,'abcdefghijklmnopqrstuvwxyz0123456789-_')" 
                                    placeholder="HSN"></asp:TextBox>
            </div>
           <div class="form-group col-md-3">   
                   <label for="inputZip">OBSERVACIONES<span style="color: #FF0000; font-weight: bold;">*</span></label>
                    <asp:TextBox ID="TxtObservaciones" runat="server" class="form-control" MaxLength="200"  onkeypress="return soloLetras(event,'abcdefghijklmnopqrstuvwxyz0123456789-_#,')"  
                                 placeholder="OBSERVACIONES"></asp:TextBox>
           </div> 
            <div class="form-group col-md-2">
                    <label for="inputZip">&nbsp;</label>
                    <div class="d-flex">
                        <asp:Button ID="BtnBuscar" runat="server" class="btn btn-primary"   Text="BUSCAR"  OnClientClick="return mostrarloader('1')" OnClick="BtnBuscar_Click" />   
                            <img alt="loading.." src="../lib/file-uploader/img/loading.gif" height="32px" width="32px"  id="ImgCarga" class="nover"   />
                         
                    </div>
            </div>
     
		</div>
			<br/>
           <div class="row">
            <div class="col-md-12 d-flex justify-content-center">
                    <div class="alert alert-danger" id="banmsg" runat="server" clientidmode="Static"><b>Error!</b> >Debe ingresar el número de la carga MRN......</div>
            </div>
            </div>

                          
                     
        </ContentTemplate>
            <Triggers>
            <asp:AsyncPostBackTrigger ControlID="BtnBuscar" />
        </Triggers>
        </asp:UpdatePanel>
     
    
   <%--  <h4 class="mb">COMPAÑÍA DE TRANSPORTE</h4>    --%>
     <div class="form-row"> 
        
           <div class="form-group col-md-1">
             <label for="inputZip">Pagado:<span style="color: #FF0000; font-weight: bold;"></span></label>
               <asp:UpdatePanel ID="UPPAGADO" runat="server"  UpdateMode="Conditional" >
                           <ContentTemplate>
                            <asp:TextBox ID="TxtPagado" runat="server" class="form-control"    
                                placeholder=""  Font-Bold="true" style="text-align:center" disabled ></asp:TextBox>
                                </ContentTemplate>
                            </asp:UpdatePanel>
         </div>

          <div class="form-group col-md-1">
             <label for="inputZip">Facturado Hasta:<span style="color: #FF0000; font-weight: bold;"></span></label>
               <asp:UpdatePanel ID="UPCAS" runat="server"  UpdateMode="Conditional" >
                           <ContentTemplate>
                            <asp:TextBox ID="TxtFechaCas" runat="server" class="form-control"    
                                placeholder=""  Font-Bold="true" style="text-align:center" disabled ></asp:TextBox>
                                </ContentTemplate>
                            </asp:UpdatePanel>
         </div>
         <div class="form-group col-md-2">
             <label for="inputZip">Total Bultos:<span style="color: #FF0000; font-weight: bold;"></span></label>
               <asp:UpdatePanel ID="UPCONTENEDOR" runat="server"  UpdateMode="Conditional" >
                           <ContentTemplate>
                            <asp:TextBox ID="TxtContenedorSeleccionado" runat="server" class="form-control"    
                                placeholder=""  Font-Bold="true" style="text-align:center" disabled ></asp:TextBox>
                                </ContentTemplate>
                            </asp:UpdatePanel>
         </div>
          <div class="form-group col-md-2">
             <label for="inputZip">Retirados:<span style="color: #FF0000; font-weight: bold;"></span></label>
               <asp:UpdatePanel ID="UPRETIRADOS" runat="server"  UpdateMode="Conditional" >
                           <ContentTemplate>
                            <asp:TextBox ID="TxtRetirados" runat="server" class="form-control"    
                                placeholder=""  Font-Bold="true" style="text-align:center" disabled ></asp:TextBox>
                                </ContentTemplate>
                            </asp:UpdatePanel>
         </div>
          <div class="form-group col-md-2">
             <label for="inputZip">Saldo:<span style="color: #FF0000; font-weight: bold;"></span></label>
               <asp:UpdatePanel ID="UPSALDO" runat="server"  UpdateMode="Conditional" >
                           <ContentTemplate>
                            <asp:TextBox ID="TxtSaldo" runat="server" class="form-control"    
                                placeholder=""  Font-Bold="true" style="text-align:center" disabled ></asp:TextBox>
                                </ContentTemplate>
                            </asp:UpdatePanel>
         </div>
         <div class="form-group col-md-2">
             <label for="inputZip">Ubicación:<span style="color: #FF0000; font-weight: bold;"></span></label>
               <asp:UpdatePanel ID="UPBODEGA" runat="server"  UpdateMode="Conditional" >
                           <ContentTemplate>
                            <asp:TextBox ID="TxtBodega" runat="server" class="form-control"    
                                placeholder=""  Font-Bold="true" style="text-align:center" disabled ></asp:TextBox>
                                </ContentTemplate>
                            </asp:UpdatePanel>
         </div>
          <div class="form-group col-md-2">
             <label for="inputZip">Producto:<span style="color: #FF0000; font-weight: bold;"></span></label>
               <asp:UpdatePanel ID="UPPRODUCTO" runat="server"  UpdateMode="Conditional" >
                           <ContentTemplate>
                            <asp:TextBox ID="TxtTipoProducto" runat="server" class="form-control"    
                                placeholder=""  Font-Bold="true" style="text-align:center" disabled ></asp:TextBox>
                                </ContentTemplate>
                            </asp:UpdatePanel>
         </div>
     </div>
    
     

 
 </div>

   


      <asp:UpdatePanel ID="UPBOTONES" runat="server" UpdateMode="Conditional" >  
             <ContentTemplate>
                  <div class="row">
                    <div class="col-md-12 d-flex justify-content-center">
                             <div class="alert alert-danger" id="banmsg_det" runat="server" clientidmode="Static"><b>Error!</b>Debe ingresar el número de la carga MRN......</div>
                    </div>
                </div>

                 <br/>
                <div class="row">
                    <div class="col-md-12 d-flex justify-content-center">
                                 <img alt="loading.." src="../lib/file-uploader/img/loading.gif" height="32px" width="32px"  id="ImgCargaDet" class="nover"  />
                     </div>
                </div>
                  <div class="row">
                     <div class="col-md-12 d-flex justify-content-center">
                                      
                        <asp:Button ID="BtnGrabar" runat="server"  class="btn btn-primary"  Text="Enviar a Turnos Especiales"   OnClientClick="return confirmacion()"  OnClick="BtnGrabar_Click" />
                       
                  </div>
                </div>
            </ContentTemplate>
             </asp:UpdatePanel>   
   
    <asp:ModalPopupExtender ID="mpeLoading" runat="server" BehaviorID="idmpeLoading"
        PopupControlID="pnlLoading" TargetControlID="btnLoading" EnableViewState="false"
        DropShadow="true" BackgroundCssClass="modalBackground" />

    <asp:Panel ID="pnlLoading" runat="server"  HorizontalAlign="Center"
        CssClass="modalPopup" align="center"  EnableViewState="false" Style="display: none">
        <br />Procesando...
         <div class="body2">
             <asp:UpdatePanel ID="msgload" runat="server" UpdateMode="Conditional"  >
               <ContentTemplate>
           <div align="center">   
              
               <asp:Image ID="loading" runat="server" ImageUrl="../lib/file-uploader/img/loading.gif"  Visible="true" Width="40" Height="40" />
             
            </div>
                  
            <br/>
            Estimado Cliente, se está generando su solicitud... <br/>
        
           <br />
                     </ContentTemplate>
                 
            </asp:UpdatePanel>
        </div>


    </asp:Panel>
    <asp:Button ID="btnLoading" runat="server" Style="display: none" />
    <!-- Modal -->

	    


   <script type="text/javascript" src="../lib/datatables/datatables.min.js"></script>


   <script type="text/javascript" src="../lib/common-scripts.js"></script>
 
   <script type="text/javascript" src="../lib/pages.js" ></script>

   <script type="text/javascript" src="../lib/advanced-form-components.js"></script>

 <script type="text/javascript" src='../lib/autocompletar/bootstrap3-typeahead.min.js'></script>


    

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
        var opcion = confirm("Estimado usuario, está seguro que desea agregar la carga al esquema de turnos especiales bajo solicitud ?");
        if (opcion == true)
        {
            //loader();
            return true;
        } else
        {
            //loader();
	         return false;
        }

       
    }

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