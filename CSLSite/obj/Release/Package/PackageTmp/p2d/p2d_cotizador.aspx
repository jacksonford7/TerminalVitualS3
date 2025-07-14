<%@ Page Title="" Language="C#" MasterPageFile="~/site.Master" AutoEventWireup="true" CodeBehind="p2d_cotizador.aspx.cs" Inherits="CSLSite.p2d_cotizador" %>
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

    function BindVolumen()
    {

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
    <asp:HiddenField ID="manualHide" runat="server" />

     <div class="mt-4">         
        <nav class="mt-4" aria-label="breadcrumb">
          <ol class="breadcrumb">
            <li class="breadcrumb-item" id="opcion_principal" runat="server"><a href="#">P2D</a></li>
            <li class="breadcrumb-item active" aria-current="page" id="sub_opcion" runat="server">COTIZADOR</li>
          </ol>
        </nav>
      </div>
      
<div class="dashboard-container p-4" id="cuerpo" runat="server">
      
    <div class="alert alert-success" id="mensaje_proforma" runat="server" clientidmode="Static">...</div>

     <asp:UpdatePanel ID="UPCARGA" runat="server"  UpdateMode="Conditional" ChildrenAsTriggers="true">
    <ContentTemplate>

         <div class="form-title">
           DATOS PARA LA ENTREGA
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
             
                <div class="form-group col-md-8">   
                   <label for="inputZip">DIRECCIÓN DE ENTREGA<span style="color: #FF0000; font-weight: bold;">*</span></label>
                    <asp:TextBox ID="Txtdireccion" runat="server" class="form-control" MaxLength="200"  onkeypress="return soloLetras(event,'abcdefghijklmnopqrstuvwxyz0123456789-_#,')"  
                                 placeholder="DIRECCIÓN DE ENTREGA"></asp:TextBox>
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

        <div class="form-title">
           DATOS DE LA CARGA A COTIZAR
       </div>
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
           
           <%--   <div class="form-group col-md-2">
                    <label for="inputZip">&nbsp;<span style="color: #FF0000; font-weight: bold;"></span></label>
                    <asp:UpdatePanel ID="UPTODOS" runat="server"  UpdateMode="Conditional" ChildrenAsTriggers="true">
                    <ContentTemplate>
                        <label class="checkbox-container">
                        &nbsp;&nbsp;<asp:CheckBox ID="ChkTodos" runat="server"  Text="Carga No Apilable"   OnCheckedChanged="ChkTodos_CheckedChanged"   AutoPostBack="true" />
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
                                    <label class="form-check-label" for="inlineCheckbox1" runat="server" id="LblTituloCarbono">P2D Xpress </label>  
                                 </label>
                                </ContentTemplate> 
                            <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="BtnBuscarCarga" />
                           </Triggers>
                          </asp:UpdatePanel> 
                </div>

             <div class="form-group col-md-2">
                   <label for="inputZip">&nbsp;<span style="color: #FF0000; font-weight: bold;"></span></label>
                    <asp:UpdatePanel ID="UPAPILABLE" runat="server" UpdateMode="Conditional" >  
                    <ContentTemplate> 
                      <asp:Button ID="BtnApilable" runat="server" class="btn btn-primary mr-4"  Text="CARGA NO APILABLE" OnClick="BtnApilable_Click" />
                    </ContentTemplate>
                  </asp:UpdatePanel>   
              </div>

              

            <div class="form-group col-md-2">
                 <label for="inputZip">&nbsp;</label>
                 <div class="d-flex"></div>
            </div>          
        </div>

       <div class="row">
              <div class="col-md-6 d-flex justify-content-center">
                 <label for="inputZip">&nbsp;</label>
                   <div class="d-flex">
                        <asp:Button ID="BtnBuscarCarga" runat="server" class="btn btn-primary"   Text="BUSCAR CARGA"  OnClientClick="return mostrarloader('1')" OnClick="BtnBuscarCarga_Click" />                       
                        <img alt="loading.." src="../lib/file-uploader/img/loading.gif" height="32px" width="32px"  id="ImgCarga" class="nover"   />     
                  </div>
            </div>
       </div>


        <div class="form-title">
        VALORES DE LA PROFORMA   
        </div>
        <div class="form-row"> 
            <div class="form-group col-md-2">
               <label for="inputZip">CONTENEDOR<span style="color: #FF0000; font-weight: bold;"></span></label>
                  <asp:TextBox ID="TxtContenedor" runat="server" class="form-control text-center"  MaxLength="4"  onkeypress="return soloLetras(event,'0123456789')" 
                                    placeholder="" disabled></asp:TextBox>
            </div>

            <div class="form-group col-md-2">
               <label for="inputZip">TOTAL BULTOS<span style="color: #FF0000; font-weight: bold;"></span></label>
                  <asp:TextBox ID="TxtBultos" runat="server" class="form-control text-center"  MaxLength="4"  onkeypress="return soloLetras(event,'0123456789')" 
                                    placeholder="" disabled></asp:TextBox>
            </div>
           <div class="form-group col-md-3">
               <label for="inputZip">PESO CALCULADO (KG)<span style="color: #FF0000; font-weight: bold;"></span></label>
                  <asp:TextBox ID="TxtTotalPeso" runat="server" class="form-control text-center"  MaxLength="10"  onkeypress="return soloLetras(event,'0123456789.')" 
                                    placeholder="" disabled></asp:TextBox>
            </div>
             
             <div class="form-group col-md-2" runat="server" id="div_volumen">
                 <asp:UpdatePanel ID="UPVOLUMEN" runat="server" UpdateMode="Conditional" >  
                 <ContentTemplate>
                 <label for="inputZip" runat="server" id="lblvolumen">VOLUMEN (MT)<span style="color: #FF0000; font-weight: bold;"></span></label>
                  <asp:TextBox ID="TxtTotalVolumen" runat="server" class="form-control text-center"  MaxLength="10"  onkeypress="return soloLetras(event,'0123456789.')" 
                                    placeholder="" disabled></asp:TextBox>
                   </ContentTemplate>
                 </asp:UpdatePanel>   
            </div>
             
             <div class="form-group col-md-2">
                      <label for="inputZip">TOTAL A PAGAR<span style="color: #FF0000; font-weight: bold;"></span></label>
                        <asp:TextBox ID="TxtTotalPagar" runat="server" class="form-control text-right" MaxLength="16"  disabled 
                                     ></asp:TextBox>
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
    <asp:AsyncPostBackTrigger ControlID="BtnBuscarCarga" />
    </Triggers>
   </asp:UpdatePanel>

          <div class="row">
              <div class="col-md-12 d-flex justify-content-center">
                     <asp:UpdatePanel ID="UPCALCULAR" runat="server" UpdateMode="Conditional" >  
                       <ContentTemplate>
                           <asp:Button ID="BtnLimpiar" runat="server" class="btn btn-outline-primary mr-4"  Text="NUEVA PROFORMA"  OnClick="BtnLimpiar_Click"/>
                           <asp:Button ID="BtnCalcular" runat="server" class="btn btn-primary mr-4"   Text="CONFIRMAR PROFORMA"  OnClientClick="return confirmacion()"  OnClick="BtnCalcular_Click" disabled/>    
                           <asp:Button ID="BtnImprimir" runat="server" class="btn btn-primary mr-4"   Text="IMPRIMIR PROFORMA"   OnClick="BtnImprimir_Click"  disabled/>    
                       </ContentTemplate>
                        </asp:UpdatePanel>   
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

            </ContentTemplate>
             </asp:UpdatePanel>   
   


</div>

  <!-- Modal -->
<div class="modal fade" id="myModal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
<div class="modal-dialog" role="document" style="max-width: 828px; max-height:450px;">
    <div class="modal-content">
        <div class="modal-header">
             <h5 class="modal-title">DETALLE DE COTIZACIÓN</h5>
             <button type="button" class="close" data-dismiss="modal" aria-label="Close">
              <span aria-hidden="true">&times;</span>
              </button>
        </div>

         <img  src="../img/banner_cotizacion.png"  class="img-responsive"  />

         <asp:UpdatePanel ID="UPRESULTADO" runat="server" UpdateMode="Conditional" >  
         <ContentTemplate>
         <div class="modal-body">
             <br/>
            
                  <asp:Panel ID="Panel2" runat="server">         
                     <asp:HiddenField ID="hf_ID_TARIFA" runat="server" />
                    <asp:HiddenField ID="hf_SECUENCIA" runat="server" />
                  <asp:HiddenField ID="hf_PROFORMA" runat="server" />

                  <div class="row">
                        <div class="col-md-12 d-flex justify-content-center">
                             <div class="alert alert-danger" id="banmsg2" runat="server" clientidmode="Static"><b>Error!</b> >Debe ingresar el número de la carga MRN......</div>
                        </div>
                  </div>
                 <div class="form-title">
                       DETALLE DE PESOS CALCULADOS
                 </div>
                <%--<div class="form-row"> 
                    <div class="form-group col-md-6">
                      <label for="inputZip">PESO CALCULADO (KG)<span style="color: #FF0000; font-weight: bold;"></span></label>
                        <asp:TextBox ID="TxtTotalPeso" runat="server" class="form-control text-center" MaxLength="16"   disabled 
                                     ></asp:TextBox>
                    </div>
                    <div class="form-group col-md-6">
                      <label for="inputZip">PESO VOLÚMETRICO (MT)<span style="color: #FF0000; font-weight: bold;"></span></label>
                        <asp:TextBox ID="TxtTotalVolumen" runat="server" class="form-control text-center" MaxLength="16"  disabled 
                                     ></asp:TextBox>
                    </div>
                </div>--%>
                <div class="form-title">
                       DETALLE DE VALORES APROXIMADOS A PAGAR
                 </div>
                  <div class="form-row">
                       <div class="form-group col-md-6">
                       </div>
                     
                  </div>
                    <div class="row" >
                         <div class="col-md-12 d-flex justify-content-center">
                             <h5> Este es un Valor aproximado sujeto a verificacion.</h5>
                         </div>
                    </div>           
               </asp:Panel>
            
        </div>

        <div class="modal-footer">
            <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
            <%--<asp:Button ID="BtnVisualizar" runat="server" class="btn btn-primary" Text="IMPRIMIR PROFORMA"    OnClick="BtnVisualizar_Click" />--%>
        </div>
       </ContentTemplate>   
      </asp:UpdatePanel>
    </div>
</div>
</div>

<!-- /showback -->


   <script type="text/javascript" src="../lib/common-scripts.js"></script>
 
   <script type="text/javascript" src="../lib/pages.js" ></script>

   <script type="text/javascript" src="../lib/advanced-form-components.js"></script>

 


    <script type="text/javascript">
    

    function mostrarloader(Valor) {

        try {

            document.getElementById("ImgCarga").className = 'ver';

           
                
            } catch (e) {
                alert('Lamentamos comunicarle que ha ocurrido el siguiente problema:\n' + e.Message);
            }
        }


    function ocultarloader(Valor) {
        try {

            document.getElementById("ImgCarga").className = 'nover';

         
             } catch (e) {
                alert('Lamentamos comunicarle que ha ocurrido el siguiente problema:\n' + e.Message);
            }
        }

     function confirmacion()
    {
        var mensaje;
        var opcion = confirm("Estimado cliente, está seguro que desea generar la proforma. ?");
        if (opcion == true)
        {
            return true;
        } else
        {
	         return false;
        }

        }

        function asignar() {
        try {

            var Lat = document.getElementById("<%=TxtLat.ClientID%>").value ;
            var Long = document.getElementById("<%=TxtLon.ClientID%>").value;

            document.getElementById("<%=TxtLatitud.ClientID%>").value = Lat;
             document.getElementById("<%=TxtLongitud.ClientID%>").value = Long;
            
         
             } catch (e) {
                alert('Lamentamos comunicarle que ha ocurrido el siguiente problema:\n' + e.Message);
            }
        }

  
      
  </script>
    <script type="text/javascript">
            $(document).ready(function () {
                 $('.datetimepicker').datetimepicker().datetimepicker({ lang: 'es', timepicker: true, step: 30, format: 'm/d/Y H:i' });
              });    
      </script>

</asp:Content>