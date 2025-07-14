<%@ Page Title="" Language="C#" MasterPageFile="~/site.Master" AutoEventWireup="true" CodeBehind="facturacioncfs.aspx.cs" Inherits="CSLSite.facturacioncfs" %>
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

     <link href="../css/jquery.notify.css" type="text/css" rel="stylesheet" />
	
	  <link href="../css/animate.min.css" type="text/css" rel="stylesheet" />

    <script type="text/javascript" src="../js/bootstrap-notify.min.js"></script>

<script type="text/javascript" src='https://maps.google.com/maps/api/js?key=AIzaSyA0f3IQRMX1fmn-35UxyLJSDvKv3BbKBhI&sensor=false&libraries=places'></script>
<script type="text/javascript" src="../maps/locationpicker.jquery.js"></script>

  <script type="text/javascript">

$(document).ready(function(){
  $('[data-toggle="tooltip"]').tooltip();   
});
</script>



    
     <script type="text/javascript">

         function mensaje()
         {
             
              try
              {
                 
                  $.notify({
	                // options
	                title: '<div class="text-center m-5"><strong>Disfruta los servicios que ofrecemos</strong></div><div class="text-center m-5"><img src="../img/p2d.jpg" style="justify-content: center"/> </div><br/>',
                      message: "<div class='text-justify'>El servicio puerta a puerta ofrece un servicio completo desde la recogida de mercancía en una ubicación exacta hasta la entrega final en un punto específico a través de cualquiera de los medios de transporte (marítimo, terrestre o aéreo) y abarcando, por tanto, sus respectivas etapas.<br/> <br/>El servicio puerta a puerta ofrece un servicio completo desde la recogida de mercancía en una ubicación exacta hasta la entrega final en un punto específico a través de cualquiera de los medios de transporte (marítimo, terrestre o aéreo) y abarcando, por tanto, sus respectivas etapas.</div>",
                  icon: 'glyphicon glyphicon-ok-circle',
                },{
	                // settings
	                element: 'body',
	                position: null,
	                type: "warning",
	                allow_dismiss: true,
	                newest_on_top: false,
	                showProgressbar: false,
	                placement: {
		                from: "top",
		                align: "center"
	                },
	                offset: 20,
	                spacing: 10,
	                z_index: 1031,
	                delay: 3300,
	                timer: 1000,
	                url_target: '_blank',
	                mouse_over: null,
	                animate: {
		                enter: 'animated rollIn',
		                exit: 'animated rollOut'
	                },
	                onShow: null,
	                onShown: null,
	                onClose: null,
	                onClosed: null,
	                icon_type: 'class',
                });

                
              }
              catch (e)
              {
                        alert('Lamentamos comunicarle que ha ocurrido el siguiente problema:\n' + e.Message);
              }
        }


     </script>


    <style type="text/css">
        
        /*.modal
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
        */
        .modalBackground
        {
            background-color: Black;
            filter: alpha(opacity=60);
            opacity: 0.6;
        }
        .modalPopup
        {
            background-color: #FFFFFF;
            width: 726px;
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
            line-height: 25px;
            text-align: center;
           
            margin-bottom: 5px;
        }
    </style>
 


<script type="text/javascript">

    

 function BindFunctions() {

     $(document).ready(function() {
      
      var nCloneTh = document.createElement('th');
      var nCloneTd = document.createElement('td');
      nCloneTd.innerHTML = '<img src="../lib/advanced-datatable/images/details_open.png">';
      nCloneTd.className = "center";

      $('#<%= tablePagination.ClientID %> thead tr').each(function() {
      
      });

      $('#<%= tablePagination.ClientID %> tbody tr').each(function() {
     
      });

     
      var oTable = $('#<%= tablePagination.ClientID %>').dataTable({
        "aoColumnDefs": [{
          "bSortable": false,
          "aTargets": [0]
        }],
        "aaSorting": [
          [1, 'asc']
        ]
      });

      
      $('#<%= tablePagination.ClientID %> tbody td img').live('click', function() {
        var nTr = $(this).parents('tr')[0];
        if (oTable.fnIsOpen(nTr)) {
       
          this.src = "../lib/advanced-datatable/media/images/details_open.png";
          oTable.fnClose(nTr);
        } else {
        
          this.src = "../lib/advanced-datatable/images/details_close.png";
          oTable.fnOpen(nTr, fnFormatDetails(oTable, nTr), 'details');
        }
      });
        });
    }
</script>

 <%--<script type="text/javascript">
        $(document).ready(function () {
            $('#<%= tablePagination.ClientID %>').dataTable();
        });
       
    </script>--%>

 
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="placebody" runat="server" >
    <asp:ToolkitScriptManager ID="tkscata" runat="server"></asp:ToolkitScriptManager>

  <div id="div_BrowserWindowName" style="visibility:hidden;">
    <asp:HiddenField ID="hf_BrowserWindowName" runat="server" />
     
  </div>
    <asp:HiddenField ID="manualHide" runat="server" />
      <asp:HiddenField ID="manualHideCarbono" runat="server" />

      <div class="mt-4">         
        <nav class="mt-4" aria-label="breadcrumb">
          <ol class="breadcrumb">
            <li class="breadcrumb-item" id="opcion_principal" runat="server"><a href="#">Impo Carga Suelta</a></li>
            <li class="breadcrumb-item active" aria-current="page" id="sub_opcion" runat="server">FACTURACIÓN CARGA SUELTA (CFS)</li>
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
           <div class="form-group col-md-4">
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
                        
        <h4 class="mb">DATOS DE LA FACTURA</h4>
        <asp:UpdatePanel ID="UPDATOSCLIENTE" runat="server"  UpdateMode="Conditional" ChildrenAsTriggers="true">
        <ContentTemplate>
        <div class="form-row">
             <div class="form-group col-md-6"> 
                  <label for="inputAddress">AGENTE DE ADUANA:<span style="color: #FF0000; font-weight: bold;"></span></label>
                  <asp:HiddenField ID="hf_idagente" runat="server" />
                    <asp:HiddenField ID="hf_descagente" runat="server" />
                <asp:HiddenField ID="hf_rucagente" runat="server" />
                  <asp:TextBox ID="TXTAGENCIA" runat="server" class="form-control"    placeholder=""  Font-Bold="false" disabled ></asp:TextBox>
            </div>
             <div class="form-group col-md-6"> 
                 <label for="inputAddress">CLIENTE:<span style="color: #FF0000; font-weight: bold;"></span></label>
                        <asp:HiddenField ID="hf_idcliente" runat="server" />
                        <asp:HiddenField ID="hf_desccliente" runat="server" />
					    <asp:TextBox ID="TXTCLIENTE" runat="server" class="form-control"    placeholder=""  Font-Bold="false" disabled></asp:TextBox>                
             </div>
             <div class="form-group col-md-12"> 
                 <label for="inputAddress">FACTURADO A:<span style="color: #FF0000; font-weight: bold;">*</span></label>
                  <asp:TextBox ID="TXTASUMEFACTURA" runat="server" class="form-control"  placeholder=""  Font-Bold="false" disabled Visible="false"></asp:TextBox>
                    <asp:HiddenField ID="hf_idasume" runat="server" />
                    <asp:HiddenField ID="hf_descasume" runat="server" />
                               
                    <asp:DropDownList runat="server" ID="CboAsumeFactura"    AutoPostBack="false"  class="form-control"  >
                        </asp:DropDownList>
             </div> 

       </div>
                        
        </ContentTemplate>
            <Triggers>
            <asp:AsyncPostBackTrigger ControlID="BtnBuscar" />
        </Triggers>
    </asp:UpdatePanel>
   <h4 class="mb">DETALLE DE LA CARGA</h4>

    <div class="form-row">  
        <div class="form-group col-md-3">
                     
               <label for="inputEmail4">FECHA SALIDA<span style="color: #FF0000; font-weight: bold;">*</span></label>
               <div class="d-flex">
                    <asp:TextBox runat="server" ID="TxtFechaHasta"   MaxLength="16" 
                                    onkeypress="return soloLetras(event,'0123456789-')"    class="datetimepicker form-control" ></asp:TextBox>
                    &nbsp;&nbsp;
                    <asp:UpdatePanel ID="UPAGREGAR" runat="server"  UpdateMode="Conditional" ChildrenAsTriggers="true">
                    <ContentTemplate>
                    <asp:Button ID="BtnAgregar" runat="server" class="btn btn-primary"  Text="AGREGAR FECHA"  OnClick="BtnAgregar_Click" />      
                    </ContentTemplate> 
                        <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="BtnAgregar" />
                    </Triggers>
                    </asp:UpdatePanel> 
               
                       <asp:TextBox ID="Txtcomentario" runat="server" class="form-control" MaxLength="150" Width="330px" onkeypress="return soloLetras(event,'abcdefghijklmnopqrstuvwxyz0123456789-_')"  
                       placeholder="Comentario" Visible="false"></asp:TextBox>
               </div>
                         
       </div>

         <div class="form-group col-md-4">
              <label for="inputEmail4"> &nbsp;<span style="color: #FF0000; font-weight: bold;"> &nbsp;</span></label>
                             <asp:UpdatePanel ID="UPCGSAAPP" runat="server"  UpdateMode="Conditional" ChildrenAsTriggers="true">
                        <ContentTemplate>
                            <label class="checkbox-container" runat="server" id="Servicio" >
                             <input  id="ChkAppCgsa" class="form-check-input" type="checkbox"  runat="server" value="false"   disabled />
                                 <span class="checkmark"></span>
                                <label class="form-check-label" for="inlineCheckbox1">Servicio de Notificaciones de Trazabilidad de Carga mediante aplicación móvil CGSApp y Terminal Virtual</label>  
                             </label>
                            </ContentTemplate> 
                        <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="BtnBuscar" />
                       </Triggers>
                      </asp:UpdatePanel> 
          </div>
         
         <div class="form-group col-md-1">
              <label for="inputEmail4"> &nbsp;<span style="color: #FF0000; font-weight: bold;"> &nbsp;</span></label>
              <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
              <ContentTemplate>
                        <asp:Button ID="BtnVerMas" runat="server" class="btn btn-primary" 
                                                Text="Ver más..." OnClick="BtnVerMas_Click"  
                                             /> 
              </ContentTemplate> 
                   <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="BtnBuscar" />
                       </Triggers>
            </asp:UpdatePanel> 
        </div>


        <div class="form-group col-md-4">
            <label for="inputEmail4"> &nbsp;<span style="color: #FF0000; font-weight: bold;"> &nbsp;</span></label>
            <asp:UpdatePanel ID="UPCGSAAPPAG" runat="server"  UpdateMode="Conditional" ChildrenAsTriggers="true">
                <ContentTemplate>
                    <label class="checkbox-container" runat="server" id="ServicioAG" >
                        <input  id="ChkAppCgsaAG" class="form-check-input" type="checkbox"  runat="server" value="false"  data-toggle="modal" data-target="#myModalAG"    />
                        <span class="checkmark"></span>
                        <label class="form-check-label" for="inlineCheckbox1">Servicio de Notificaciones de Trazabilidad de Carga mediante aplicación móvil CGSApp y Terminal Virtual</label>  
                    </label>
                </ContentTemplate> 
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="BtnBuscar" />
                </Triggers>
            </asp:UpdatePanel> 
        </div>


         <div class="form-group col-md-4">
              <label for="inputEmail4"> &nbsp;<span style="color: #FF0000; font-weight: bold;"> &nbsp;</span></label>
                   <asp:UpdatePanel ID="UPCARBONO" runat="server"  UpdateMode="Conditional" ChildrenAsTriggers="true">
                        <ContentTemplate>
                            <label class="checkbox-container" runat="server" id="LblCarbono" visible="true" >
                             <asp:CheckBox id="ChkCarbono"  runat="server"  OnCheckedChanged="ChkCarbono_CheckedChanged" AutoPostBack="True"  />
                                 <span class="checkmark"></span>
                                <label class="form-check-label" for="inlineCheckbox1" runat="server" id="LblTituloCarbono">Certificado de Carbono Neutro</label>  
                             </label>
                            </ContentTemplate> 
                        <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="BtnBuscar" />
                       </Triggers>
                      </asp:UpdatePanel> 
              

          </div>
    </div>
    
    <div class="form-row">  

        <div class="form-group col-md-12"> 
             <asp:UpdatePanel ID="UPPROFORMA" runat="server" UpdateMode="Conditional" >  
            <ContentTemplate>
                  <div class="alert alert-success" id="mensaje_proforma" runat="server" clientidmode="Static" visible="false"></div>
           </ContentTemplate>
             </asp:UpdatePanel>   
        </div>

         <div class="form-group col-md-12"> 
             <asp:UpdatePanel ID="UPACEPTA" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">  
            <ContentTemplate>

                  <div class="alert alert-success" id="acepta_servicio" runat="server" clientidmode="Static" visible="false">
                      
                     <div class="form-group col-md-12" id="texto_servicio" runat="server"> 
                      ..
                     </div>

                     <div class="form-row"> 
                      
                       <div class="form-group col-md-2">   
                           <label for="inputZip">CIUDAD<span style="color: #FF0000; font-weight: bold;">*</span></label>
                            <asp:DropDownList runat="server" ID="CboCiudad"    AutoPostBack="false"  class="form-control"  >
                             </asp:DropDownList>
                        </div> 
                          <div class="form-group col-md-2">   
                           <label for="inputZip">ZONA<span style="color: #FF0000; font-weight: bold;">*</span></label>
                            <asp:DropDownList runat="server" ID="CboZonas"    AutoPostBack="false"  class="form-control"  >
                             </asp:DropDownList>
                        </div> 
                        <div class="form-group col-md-5">   
                           <label for="inputZip">DIRECCIÓN DE ENTREGA<span style="color: #FF0000; font-weight: bold;">*</span></label>
                            <asp:TextBox ID="Txtdireccion" runat="server" class="form-control" MaxLength="200"  onkeypress="return soloLetras(event,'abcdefghijklmnopqrstuvwxyz0123456789-_#,')"  
                                         placeholder="DIRECCIÓN DE ENTREGA"></asp:TextBox>
                        </div> 

                         <%--<div class="form-group col-md-2">
                                <label for="inputZip">&nbsp;<span style="color: #FF0000; font-weight: bold;"></span></label>
                                <asp:UpdatePanel ID="UPTODOS" runat="server"  UpdateMode="Conditional" ChildrenAsTriggers="true">
                                <ContentTemplate>
                                    <label class="checkbox-container">
                                    &nbsp;&nbsp;<asp:CheckBox ID="ChkTodos"  runat="server"  Text="Carga No Apilable"  OnCheckedChanged="ChkTodos_CheckedChanged"   AutoPostBack="true"   />
                                    <span class="checkmark"></span>
                                    </label>
                              </ContentTemplate> 
                                <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="ChkTodos" />
                            </Triggers>
                          </asp:UpdatePanel> 
                            </div>--%>
                          <div class="form-group col-md-1">
                          <label for="inputEmail4"> &nbsp;<span style="color: #FF0000; font-weight: bold;"> &nbsp;</span></label>
                               <asp:UpdatePanel ID="UPEXPRESS" runat="server"  UpdateMode="Conditional" ChildrenAsTriggers="true">
                                    <ContentTemplate>
                                        <label class="checkbox-container" runat="server" id="LblExpress" visible="true" >
                                         <asp:CheckBox id="ChkExpress"  runat="server" OnCheckedChanged="ChkExpress_CheckedChanged"  AutoPostBack="True"  />
                                             <span class="checkmark"></span>
                                            <label class="form-check-label" for="inlineCheckbox1" runat="server" id="Label1">P2D Xpress </label>  
                                         </label>
                                        </ContentTemplate> 
                                    <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="BtnBuscar" />
                                   </Triggers>
                                  </asp:UpdatePanel> 
                        </div>


                         <div class="form-group col-md-1">
                               <label for="inputZip">&nbsp;<span style="color: #FF0000; font-weight: bold;"></span></label>
                             <asp:UpdatePanel ID="UPAPILABLE" runat="server" UpdateMode="Conditional" >  
                           <ContentTemplate>
                             
                             <asp:Button ID="BtnApilable" runat="server" class="btn btn-primary mr-4"  Text="CARGA NO APILABLE" OnClick="BtnApilable_Click" />
                           </ContentTemplate>
                         </asp:UpdatePanel>   
                         </div>

                          <div class="form-group col-md-2">                  
                      <label for="inputZip">UBICACIÓN GOOGLE MAPS<span style="color: #FF0000; font-weight: bold;">*</span></label><br/>
                      <button type="button" class="btn btn-primary" data-toggle="modal" data-target="#ModalMap"><span class="fa fa-map-marker"></span><span id="ubicacion"> Seleccionar Ubicación</span></button>
                       <div class="container">
                        <style>
                            .pac-container {
                                z-index:99999;
                            }
                        </style>
                        <div class="modal fade" id="ModalMap" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true"> 
                            <div class="modal-dialog" role="document" style="max-width: 1250px; max-height:850px;">
                                 <div class="modal-content" >
                                     <div class="modal-header">
                                          <h5 class="modal-title">BUSCAR DIRECCIÓN Y COORDENADAS</h5>
                                         <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                          <span aria-hidden="true">&times;</span>
                                          </button>
                                    </div>
                                     <div class="modal-body" >
                                         <div class="form-horizontal" >
                                             
                                             <div class="form-row">
                                                  <div class="form-group col-md-12">   
                                                       <label for="inputZip">UBICACIÓN DE ENTREGA<span style="color: #FF0000; font-weight: bold;">*</span></label>
                                                        <asp:TextBox ID="TxtUbicacionEntrega" runat="server" class="form-control" MaxLength="200"  onkeypress="return soloLetras(event,'abcdefghijklmnopqrstuvwxyz0123456789-_#,')"  
                                                                    ></asp:TextBox>
                                                  </div> 
                                             </div>
                                             <div id="ModalMapPreview" style="width:1200px; height:700px;"> </div>
                                             <div class="clearfix"> &nbsp;</div>
                                              <div class="form-row">
                                                <label class="p-r-small col-sm-1 control-label">Lat.:</label>
                                                <div class="col-sm-3">
                                                     <asp:TextBox ID="TxtLat" runat="server" class="form-control" MaxLength="200"  onkeypress="return soloLetras(event,'0123456789-,')" ></asp:TextBox>
                                                </div>
                                                <label class="p-r-small col-sm-2 control-label">Long.:</label>

                                                <div class="col-sm-3">
                                                     <asp:TextBox ID="TxtLon" runat="server" class="form-control" MaxLength="200"  onkeypress="return soloLetras(event,'0123456789-,')" ></asp:TextBox>
                                                </div>
                                                <div class="col-sm-3">
                                                      <asp:Button ID="BtnGrabarCoordenadas" runat="server" class="btn btn-primary btn-block" data-dismiss="modal" Text="Aceptar" OnClientClick="asignar();" />
                                                </div>
                                                 
                                            </div>
                                             <div class="form-row"><div class="form-group col-md-12"><h5 class="modal-title">   Por favor, debe arrastrar el icono del globo de color rojo, para capturar las coordenadas.</h5></div> </div>
                                             <%-- <div class="row" >&nbsp;</div>--%>
                                             <div class="clearfix"></div>
                                             <script type="text/javascript">
                                                 $('#ModalMapPreview').locationpicker({
                                                     radius: 300,
                                                     location: {
                                                         latitude:-2.2804353,
                                                         longitude:-79.9107017
                                                     },
                                                     enableAutocomplete: true,
                                                     inputBinding: {
                                                         latitudeInput: $('#<%=TxtLat.ClientID%>'),
                                                         longitudeInput: $('#<%=TxtLon.ClientID%>'),
                                                         locationNameInput:$('#<%=TxtUbicacionEntrega.ClientID%>')
                                                     },
                                                     onchanged: function (currenLocation, radius, isMarkerDropped) {
                                                         $('#ubicacion').html($('#<%=TxtUbicacionEntrega.ClientID%>').val());
                                                     }
                                                 });
                                                 $('#ModalMap').on('shown.bs.modal', function () {
                                                     $('#ModalMapPreview').locationpicker('autosize');
                                                 });
                                             </script>
                                        </div>

                                    </div>
                                 </div>
                            </div>
                        </div>
                     </div>
                </div> 
               
               
                           <div class="form-group col-md-2">   
                               <label for="inputZip">LATITUD<span style="color: #FF0000; font-weight: bold;">*</span></label>
                                <asp:TextBox ID="TxtLatitud" runat="server" class="form-control" MaxLength="200"  disabled  
                                             ></asp:TextBox>
                            </div> 
                           <div class="form-group col-md-2">   
                               <label for="inputZip">LONGITUD<span style="color: #FF0000; font-weight: bold;">*</span></label>
                                <asp:TextBox ID="TxtLongitud" runat="server" class="form-control" MaxLength="200"  disabled  
                                             ></asp:TextBox>
                            </div> 

                    </div>

                    <div class="row">
                        <div class="col-md-12 d-flex justify-content-center">
                         <asp:UpdatePanel ID="UPCALCULAR" runat="server" UpdateMode="Conditional" >  
                           <ContentTemplate>
                               <asp:Button ID="BtnConfirmar" runat="server" class="btn btn-primary mr-4"   Text="ACEPTAR SERVICIO"  OnClick="BtnConfirmar_Click"  />    
                                 <asp:Button ID="BtnCancelar" runat="server" class="btn btn-primary mr-4"  Text="CANCELAR SERVICIO" OnClick="BtnCancelar_Click" />
                           </ContentTemplate>
                         </asp:UpdatePanel>   
                        </div>
                    </div>

                  </div>
           </ContentTemplate>
                 <Triggers>
                      <asp:AsyncPostBackTrigger ControlID="BtnBuscar" />
                   <asp:AsyncPostBackTrigger ControlID="BtnConfirmar" />
                      <asp:AsyncPostBackTrigger ControlID="BtnCancelar" />
                        <asp:AsyncPostBackTrigger ControlID="BtnApilable" />
                </Triggers>
             </asp:UpdatePanel>   
        </div>

        <div class="form-group col-md-12">
     <asp:UpdatePanel ID="UPDETALLE" runat="server" UpdateMode="Conditional" >  
       <ContentTemplate>
         
              <div class="nover" id="myModal"  tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true" clientidmode="Static" runat="server" >
                        <div class="modal-dialog" id="ventana_content-popup"  >
                            <div class="modal-content">
		    
                            <div class="modal-header">
                               <asp:Button ID="BtnCerrar2" runat="server" class="btn btn-outline-primary mr-4"  data-dismiss="modal" aria-hidden="true"  Text="&times;" OnClick="BtnCerrar_Click" />  
                              <h4 class="modal-title" id="myModalLabel">Alerta por cartera vencida!</h4>
                            </div>
                            <div class="modal-body">
			                 Estimado Cliente: <span id="fac_cliente" runat="server" clientidmode="Static">..</span>
                             <br /> 
                             Al momento usted presenta facturas vencidas.
                             <table border="1" cellspacing="1" cellpadding="1" style=" width:100%;">
                                 <tr>
                                  <td>Facturas por Vencer</td>
                                  <td><span id="fac_pend" runat="server" clientidmode="Static">0</span></td>
                                  </tr>
                                  <tr>
                                  <td>Facturas Vencidas</td>
                                  <td><span id="fac_ven" runat="server" clientidmode="Static">0</span></td>
                                  </tr>
                                  <tr>
                                  <td>Monto Total</td>
                                  <td><span id="monto_fac" runat="server" clientidmode="Static">$0.00</span></td>
                                  </tr>
                                 <tr>
                                 <td>Cupo Crédito</td>
                                 <td><span id="fac_cupo" runat="server" clientidmode="Static">0</span></td>
                                 </tr>
                             </table>
                                Favor proceder con el pago de las facturas detalladas y compensar las mismas. En caso de requerir revisión con el departamento Tesorería, contactar a <a href="mailto:tesoreria@cgsa.com.ec?Subject=Aviso falta de pago" >tesoreria@cgsa.com.ec</a>
			                   <br />y teléfono +593 4 6006300 - Opción 3 en horario lunes a viernes de 8am a 5:30pm.
                               <br />
                                <span id='cliente_ruc'></span>
                             </div>
                            <div class="modal-footer">
                                 <asp:Button ID="BtnCerrar" runat="server" class="btn btn-outline-primary mr-4"   Text="cerrar" OnClick="BtnCerrar_Click" />  
                            </div>
                        </div>
                    </div>
                 </div>
            
           <h3 id="LabelTotal" runat="server">DETALLE DE ITEMS</h3>
 
            <asp:GridView ID="tablePagination" runat="server" AutoGenerateColumns="False"  DataKeyNames="CONTENEDOR"
                                    GridLines="None" OnPageIndexChanging="tablePagination_PageIndexChanging" OnPreRender="tablePagination_PreRender"   OnRowDataBound="tablePagination_RowDataBound" 
                                    PageSize="10"
                                    AllowPaging="True"
                                   CssClass="table table-bordered invoice">
                    <PagerStyle HorizontalAlign = "Right" CssClass="table table-bordered invoice"   />
                    <RowStyle  BackColor="#F0F0F0" />
                    <alternatingrowstyle  BackColor="#FFFFFF" />
                           
                                    <Columns>
                                                 
                                            <asp:BoundField DataField="SECUENCIA" HeaderText="#"  HeaderStyle-HorizontalAlign="Center" />
                                        <asp:TemplateField HeaderText="FA" ItemStyle-CssClass="center hidden-phone" Visible="false">
                                            <ItemTemplate>
                                                <asp:UpdatePanel ID="UPSELECCIONAR" runat="server" ChildrenAsTriggers="true">
                                                <ContentTemplate>
                                                    <label class="checkbox-container">
                                                    <asp:CheckBox ID="CHKFA" runat="server" Checked='<%# Bind("VISTO") %>'  />
                                                    <span class="checkmark"></span>
                                                    </label>
                                                </ContentTemplate>
                                               <Triggers>
                                               <asp:AsyncPostBackTrigger ControlID="CHKFA" />
                                               </Triggers>
                                            </asp:UpdatePanel>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="CONTENEDOR" HeaderText="CONTENEDOR" SortExpression="CONTENEDOR"  HeaderStyle-HorizontalAlign="Center" Visible="false"/>
                                        <asp:BoundField DataField="FECHA_HASTA" DataFormatString="{0:yyyy/MM/dd HH:mm}" HeaderText="FECHA HASTA"  HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"/>
                                                  
                                        <asp:BoundField DataField="IN_OUT" HeaderText="ESTADO"  HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                        <asp:BoundField DataField="DOCUMENTO" HeaderText="DOCUMENTO"   HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                        <asp:BoundField DataField="FECHA_ULTIMA" HeaderText="ULTIMA FACTURA"  DataFormatString="{0:yyyy/MM/dd}" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                        <asp:BoundField DataField="NUMERO_FACTURA" HeaderText="NUMERO FACTURA"  HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                        <asp:BoundField DataField="CAS" HeaderText="FECHA CAS"  DataFormatString="{0:yyyy/MM/dd}" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                        <asp:BoundField DataField="AUTORIZADO" HeaderText="AUTORIZADO"  HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                        <asp:BoundField DataField="DES_BLOQUEO" HeaderText="BLOQUEADO"  HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                        <asp:BoundField DataField="DESCRIPCION"   HeaderText="DESCRIPCION"  HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                        <asp:TemplateField HeaderText="CERTIFICADO" HeaderStyle-HorizontalAlign="Center"  ItemStyle-Width="30px"   >
                                            <ItemTemplate>
                                                <asp:UpdatePanel ID="UPCERTIFICADO" runat="server" ChildrenAsTriggers="true">
                                                <ContentTemplate>
                                                
                                                        <label class="checkbox-container">   
                                                        <asp:CheckBox ID="CHKCERTIFICADO"  runat="server" Checked='<%# Bind("CERTIFICADO") %>' OnCheckedChanged="CHKCERTIFICADO_CheckedChanged"   AutoPostBack="True"  />
                                                            <span class="checkmark"></span> 
                                                    </label>
                                                        </ContentTemplate>
                                                    <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="CHKCERTIFICADO" />
                                                    </Triggers>
                                            </asp:UpdatePanel>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="CANTIDAD" HeaderText="CANTIDAD"  HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"  Visible="true"/>
                                        <asp:BoundField DataField="IMDT" HeaderText="IMDT"  HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"  Visible="false"/>
                                        <asp:BoundField DataField="ESTADO_RDIT" HeaderText="ESTADO RDIT"  HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"  Visible="false"/>
                                        <asp:BoundField DataField="GKEY" HeaderText="GKEY"  HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"  Visible="false"/>
                                        <asp:BoundField DataField="IDPLAN" HeaderText="IDPLAN"  HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"  Visible="false"/>
                                        <asp:BoundField DataField="NUMERO_PASE_N4" HeaderText="# PASE"  HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"  Visible="true"/>
                                        <asp:BoundField DataField="TIENE_CERTIFICADO" HeaderText="TIENE CERTIFICADO"   Visible="false"/>
                                    </Columns>
                                </asp:GridView>
                  
 
           </ContentTemplate>
     </asp:UpdatePanel>   
     </div><!--content-panel-->
     
    </div><!--row mb-->
   
    
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
                      <div class="alert alert-danger" id="banmsg_Pase" runat="server" clientidmode="Static"><b>Error!</b>.</div>
                 </div>
             </div>
             <br/>

             <div class="row">
                <div class="col-md-12 d-flex justify-content-center">
                             <img alt="loading.." src="../lib/file-uploader/img/loading.gif" height="32px" width="32px"  id="ImgCargaDet" class="nover"  />
                 </div>
            </div>    
            <br/>


             <div class="row">
             <div class="col-md-12 d-flex justify-content-center">
                             
                    <asp:Button ID="BtnNuevo" runat="server" class="btn btn-outline-primary mr-4"  Text="NUEVA TRANSACCION"  OnClick="BtnNuevo_Click"  />

                   <asp:Button ID="BtnCotizar" runat="server" class="btn btn-outline-primary mr-4" Text="GENERAR PROFORMA" OnClientClick="return mostrarloader('2')" OnClick="BtnCotizar_Click"/>

                    
                   <asp:Button ID="BtnFacturar" runat="server" class="btn btn-primary" Text="GENERAR FACTURA" OnClientClick="return mostrarloader('2')" OnClick="BtnFacturar_Click" />
               </div> 
             </div>
            </ContentTemplate>
             </asp:UpdatePanel>   
   
     <asp:UpdatePanel ID="UPMENSAJE" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">  
        <ContentTemplate>

            <div class="modal fade" id="myModalAG" tabindex="-1" role="dialog" aria-labelledby="myModalLabelAG" aria-hidden="true">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <h4 class="modal-title" id="myModalLabelAG">Confirmar Generación</h4>
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                          
                    </div>
                    <div class="modal-body">
                        <br>
                        </br/>
                        Con CGSApp podrá acceder a la información completa de todos sus registros de cargas notificaciones, documentos y registros fotográficos  &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                        <br>
                        </br/>
                    </div>
                    <div class="modal-footer">
                        <asp:Button ID="btnSiDeseo" runat="server"  OnClick="btnSiDeseo_Click" class="btn btn-primary"  Text="SI DESEO"   UseSubmitBehavior="false" data-dismiss="modal" />
                        <asp:Button ID="btnNoDeseo" runat="server"  OnClick="btnNoDeseo_Click" class="btn btn-default"  Text="NO DESEO"   UseSubmitBehavior="false" data-dismiss="modal" />
                        <%--<button type="button" class="btn btn-default" data-dismiss="modal">NO DESEO</button>--%>
                    </div>
                </div>
            </div>
            </div>
        </ContentTemplate>
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="btnSiDeseo" />
    </Triggers>
    </asp:UpdatePanel>   
       
    
</div>

  <!-- Modal -->
   <asp:ModalPopupExtender  
      ID="mpedit" runat="server" 
      PopupControlID="myModal2"
      BackgroundCssClass="modalBackground"  
      TargetControlID="manualHide"
       CancelControlID="btclose"  
      >
    </asp:ModalPopupExtender>   
 

       <asp:Panel ID="myModal2" runat="server" CssClass="modalPopup" align="center" Style="display: none" >
        
        <div class="body">
             <asp:UpdatePanel ID="msgload" runat="server" UpdateMode="Conditional"  ChildrenAsTriggers="true">
               <ContentTemplate>
            
 <table width="724" border="0" cellpadding="0" cellspacing="0">
  <!--DWLayoutTable-->
  <tr>
    <td height="36" colspan="3" valign="middle" bgcolor="#FF3720"><span style="color:#f2f2f2; font-weight:bold">&nbsp; Sistema de Trazabilidad de Carga CGSApp</span></td>
  </tr>
  <tr>
    <td width="17" rowspan="4" valign="top"><!--DWLayoutEmptyCell-->&nbsp;</td>
    <td width="692" height="76" valign="top"><div align="justify">Estimado Cliente, suscríbase a CGSApp y reciba notificaciones en tiempo real de la trazabilidad de sus contenedores de importación.<br/>
    Toda la información de su carga, al alcance de su mano a través de Mail, App o nuestro portal de clientes.</div></td>
  <td width="15" rowspan="4" valign="top"><!--DWLayoutEmptyCell-->&nbsp;</td>
  </tr>
  <tr>
    <td height="228" valign="top"><div align="left"><strong>¿Qué información recibirá?</strong><br/>
        - Registro fotográfico del acto de aforo de su carga.<br/>
        - Registro fotográfico de sellos luego del acto de aforo.<br/>
        - Información de ubicación de contenedor en área de aforo.<br/>
        - Arribo de tu mercancía.<br/>
        - Correcciones que se realicen por peso o sello de descarga vs. manifestado.<br/>
        - Liberación aduanera.<br/>
        - Emisión de factura.<br/>
        - Confirmación de pago realizado.<br/>
        - Emisión de e-PASS y reasignación de turno.<br/>
        - Salida de tu mercancía.<br/>
    - Otros eventos.</div></td>
  </tr>
  <tr>
    <td height="38" valign="top"><div align="center"><strong>¡Fácil! ¡Rápido! ¡Seguro!</strong><br/>
        <strong>
    Precio del servicio $5.00 más IVA por contenedor</strong></div></td>
  </tr>
  <tr>
    <td height="104" valign="top">
	
	    <table width="100%" border="0" cellpadding="0" cellspacing="0">
	    
            </tr>  <td><br/></td></tr>
	  
	      <tr>
	        <td height="24" colspan="4" valign="top">
                 <div class="d-flex justify-content-end mt-4">
                <asp:Button ID="BtnProcesar" runat="server" class="btn btn-primary"  
                                                Text="Acepto El Servicio" OnClick="BtnProcesar_Click"   OnClientClick="this.disabled=true"
                                            UseSubmitBehavior="false" Visible="false"  />    &nbsp;&nbsp;                        
              <input  type="button" id="btclose" class="btn btn-primary" value="Salir"  />
                     </div>
                     </td>
          </tr>
	      <tr>
	        <td height="1"></td>
	        <td></td>
	        <td width="31"></td>
	        <td></td>
          </tr>
            </table>
     </div></td>
  </tr>
</table>

                     </ContentTemplate>
                   <Triggers>
                          <asp:AsyncPostBackTrigger ControlID="BtnProcesar" />
                        </Triggers>
            </asp:UpdatePanel>
        </div>
    </asp:Panel>
   <!-- Modal -->

 
      <!-- Modal -->
    <asp:ModalPopupExtender  
      ID="mpedit_carbono" runat="server" 
      PopupControlID="myModalCarbono"
      BackgroundCssClass="modalBackground"  
      TargetControlID="manualHideCarbono"
       CancelControlID="BtnAceptarCarbono"  
      >
    </asp:ModalPopupExtender>
    
       <asp:Panel ID="myModalCarbono" runat="server" CssClass="modalPopup" align="center" Style="display: none" >
        
        <div class="body">
             <asp:UpdatePanel ID="UpCheckCarbono" runat="server" UpdateMode="Conditional"  ChildrenAsTriggers="true">
               <ContentTemplate>
            
           <table width="722" border="0" cellpadding="0" cellspacing="0" background="../img/bg_carbono12.jpg">
  <!--DWLayoutTable-->
  <tr>
    <td height="36" colspan="3" valign="middle" bgcolor="#FF3720"><span style="color:#f2f2f2; font-weight:bold">&nbsp; Certificado de Carbo o Neutro</span></td>
  </tr>
  <tr>
    <td width="17" rowspan="4" valign="top"><!--DWLayoutEmptyCell-->&nbsp;</td>
    <td width="692" height="76" valign="top"><div align="justify">&nbsp;<br/>
   &nbsp;</div></td>
  <td width="15" rowspan="4" valign="top"><!--DWLayoutEmptyCell-->&nbsp;</td>
  </tr>
  <tr>
    <td height="228" valign="top"><div align="left"><strong></strong><br/>
      <br/>
        <br/>
       <br/>
        <br/>
        <br/>
        Marca tu huella de carbono, el mercado europeo lo valora.<br/>
        <br/>
       <br/>
         ¿Qué hay en el ultimo acuerdo provisional de ajuste fronterizo de carbono de la UE?<br/>
        <br/>
        </div></td>
  </tr>
  <tr>
    <td height="38" valign="top"><div align="left"><strong> <a href="https://www.niskanencenter.org/whats-in-the-latest-eu-carbon-border-adjustment-provisional-agreement/" target="_blank">https://www.niskanencenter.org/whats-in-the-latest-eu-carbon-border-adjustment-provisional-agreement/</a></strong><br/>
        <strong>
    &nbsp;</strong></div></td>
  </tr>
  <tr>
    <td height="104" valign="top">
	
	    <table width="100%" border="0" cellpadding="0" cellspacing="0">
	    
            <tr>  
                <td><br/></td>
            </tr>
	  
	      <tr>
	        <td height="24" colspan="4" valign="top">
                 <div class="d-flex justify-content-end mt-4">
                <asp:Button ID="BtnAceptarCarbono" runat="server" class="btn btn-primary"  
                                                Text="Aceptar" OnClick="BtnAceptarCarbono_Click"   
                                           Visible="true"  />    &nbsp;&nbsp;  
                      <asp:Button ID="btcloseCarbono" runat="server"  class="btn btn-primary"  
                                                Text="No Deseo" OnClick="btcloseCarbono_Click"   
                                             /> 
              <%--<input  type="button" id="btcloseCarbono" class="btn btn-primary" value="Salir"  />--%>
                     </div>
                     </td>
          </tr>
	      <tr>
	        <td height="1"></td>
	        <td></td>
	        <td width="31"></td>
	        <td></td>
          </tr>
       </table>
     </td>
  </tr>
</table>

                     </ContentTemplate>
                   <Triggers>
                          <asp:AsyncPostBackTrigger ControlID="BtnAceptarCarbono" />
                        </Triggers>
            </asp:UpdatePanel>
        </div>
    </asp:Panel>

    <!-- Modal -->


   <script type="text/javascript" src="../lib/common-scripts.js"></script>
 
   <script type="text/javascript" src="../lib/pages.js" ></script>

   <script type="text/javascript" src="../lib/advanced-form-components.js"></script>

 

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

   <script type="text/javascript">
       function asignar()
       {
           try
           {

                var Lat = document.getElementById("<%=TxtLat.ClientID%>").value ;
                var Long = document.getElementById("<%=TxtLon.ClientID%>").value;

                document.getElementById("<%=TxtLatitud.ClientID%>").value = Lat;
                 document.getElementById("<%=TxtLongitud.ClientID%>").value = Long;
            
         
           }
           catch (e)
           {
                alert('Lamentamos comunicarle que ha ocurrido el siguiente problema:\n' + e.Message);
            }
        }
   </script>


       <script type="text/javascript">
            $(document).ready(function () {
                 $('.datetimepicker').datetimepicker().datetimepicker({ lang: 'es', timepicker: false, step: 30, format: 'm/d/Y' });
              });    
      </script>

     
</asp:Content>