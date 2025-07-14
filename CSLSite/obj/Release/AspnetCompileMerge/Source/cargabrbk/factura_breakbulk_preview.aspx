<%@ Page Title="" Language="C#" MasterPageFile="~/site.Master" AutoEventWireup="true" CodeBehind="factura_breakbulk_preview.aspx.cs" Inherits="CSLSite.factura_breakbulk_preview" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="placehead" runat="server">
    <link href="../img/favicon2.png" rel="icon" />
    <link href="../img/icono.png" rel="apple-touch-icon" />
    <!-- Bootstrap core CSS -->
    <link href="../css/bootstrap.min.css" rel="stylesheet" />
    <link href="../css/dashboard.css" rel="stylesheet" />
    <link href="../css/icons.css" rel="stylesheet" />
    <link href="../css/style.css" rel="stylesheet" />


  <link href="../lib/advanced-datatable/css/demo_page.css" rel="stylesheet" />
  <link href="../lib/advanced-datatable/css/demo_table.css" rel="stylesheet" />
  <link rel="stylesheet" href="../lib/advanced-datatable/css/DT_bootstrap.css" />
   
       <!--external css-->
    <link href="../lib/font-awesome/css/font-awesome.css" rel="stylesheet" />

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
<asp:Content ID="Content2" ContentPlaceHolderID="placebody" runat="server">

    <asp:ToolkitScriptManager ID="tkscata" runat="server"></asp:ToolkitScriptManager>

  <div id="div_BrowserWindowName" style="visibility:hidden;">
    <asp:HiddenField ID="hf_BrowserWindowName" runat="server" />
   
  </div>
    <asp:HiddenField ID="manualHide" runat="server" />

     <div class="mt-4">
        <nav class="mt-4" aria-label="breadcrumb">
          <ol class="breadcrumb">
            <li class="breadcrumb-item" id="opcion_principal" runat="server"><a href="#">Impo Break Bulk</a></li>
            <li class="breadcrumb-item active" aria-current="page" id="sub_opcion" runat="server">PREVISUALIZACIÓN DE FACTURA DE BREAK BULK</li>
          </ol>
        </nav>
    </div>

 <div class="dashboard-container p-4" id="cuerpo" runat="server">

     <div class="form-row">
         <div class="form-group col-md-12"> 
      
            <div class="col-lg-12 col-lg-offset-1"> <!--col-lg-10 col-lg-offset-1-->
             <asp:UpdatePanel ID="UPDETALLE" runat="server"  UpdateMode="Conditional" ChildrenAsTriggers="true">
                <ContentTemplate>
                <div class="invoice-body"><!--invoice-body-->
                <div class="pull-left">
                  <%--<h1>DASHIO</h1>--%>
                  <img src="../img/logo_02.jpg" width="175" height="44" alt =""/>
                   <h1> Contecon Guayaquil S.A. </h1>
                  <address>
                    <strong>R.U.C.:   0992506717001 </strong><br>
                     <strong>Matriz VIA AL PUERTO MARITIMO AV. DE LA MARINA S/N</strong><br/>
                        Guayaquil - Ecuador<br/>
                        <abbr title="Phone">PBX:</abbr> (593)46006300 (593)43901700

                </address>
                    <br/>
                    <div class="alert alert-warning" id="banmsg" runat="server" clientidmode="Static"><b>Error!</b> >......</div>
                </div>

                <!-- /pull-left -->
                <div class="pull-right">
                  <h2>PREVISUALIZACIÓN DE FACTURA</h2>
                </div>
                <!-- /pull-right -->
                <div class="clearfix"></div>
                <br/>

                <div class="row">
                  <div class="col-md-9">

                   <strong><span id="agente" runat="server">.... </span></strong>
                     <%-- </h4>--%>
                  <address>
                  <strong><span id="cliente" runat="server">.... </span></strong><br/>
                 <strong><span id="facturado" runat="server">.... </span></strong><br/><br/>
                  <strong><span id="observacion" runat="server">.... </span></strong><br/>
                <strong><span id="carga" runat="server">.... </span></strong><br/>
               
                <br/>
                      <address>
                      </address>
                      <address>
                      </address>
                      <address>
                      </address>
                      <address>
                      </address>
                      <address>
                      </address>
                      <address>
                      </address>
                      <address>
                      </address>
                </address>
                  </div>
                  <!-- /col-md-9 -->
                  <div class="col-md-3">
                    <br/>
                    <div>
                      <div class="pull-left"><h4></h4></div>
                      <div class="pull-right"><h4> <span id="numero" runat="server"></span> </h4></div>
                      <div class="clearfix"></div>
                    </div>
                    <div>
                      <!-- /col-md-3 -->
                      <div class="pull-left"><strong>FECHA GENERACION: </strong></div>
                      <div class="pull-right"><strong><span id="fecha" runat="server">.... </span> </strong></div>
                      <div class="clearfix"> <asp:HiddenField ID="hf_idfacturagenerada" runat="server" /></div>
                   </div>
                    <div>
                      <!-- /col-md-3 -->
                      <div class="pull-left"><strong></strong></div>
                      <div class="pull-right"><strong> <span id="fecha_hasta" runat="server">.... </span> </strong></div>
                      <div class="clearfix"></div>
                   </div>
                    <!-- /row -->
                    <br/>
                    <div class="well well-small rojo">
                      <div class="pull-left"> Total Proformado : </div>
                      <div class="pull-right"><span id="total" runat="server">.... </span> </div>
                      <div class="clearfix"></div>
                    </div>
                                   
                  </div> <!-- /col-md-9 -->
                 </div><!-- row -->

                 <div  runat="server" id="detalle" clientidmode="Static">   
                 <table class="table">
                  <thead>
                    <tr>
                     <%-- <th style="width:60px" class="text-center">CARGA</th>--%>
                      <th style="width:60px" class="text-center">CODIGO</th>
                      <th class="text-left">DESCRIPCION</th>
                      <th style="width:60px" class="text-center">CANTIDAD</th>
                      <th style="width:140px" class="text-right">V.UNITARIO</th>
                      <th style="width:90px" class="text-right">V.TOTAL</th>
                    </tr>
                  </thead>
                  <tbody>
                    <tr>
                      <td class="text-center">..</td>
                      <td class="text-center">..</td>
                      <td>..</td>
                       <td class="text-center">..</td>
                      <td class="text-right">..</td>
                      <td class="text-right">..</td>
                    </tr>
                    
                    <tr>
                      <td colspan="3" rowspan="4">
                        <h4>Términos y condiciones</h4>
                        <p>Este documento no tiene validez legal alguna. <br/>
                            Debo y pagaré incondicionalmente a la orden de Contecon Guayaquil S.A. en el lugar y fecha que se me reconvenga,
                             el valor  total expresado  en este documento, más los  impuestos legales respectivos en  Dólares de 
                             los Estados Unidos de América, por los bienes y/o servicios que he recibido a mi entera satisfacción.
                        </p>
                        <td class="text-right"><strong>Subtotal</strong></td>
                        <td class="text-right"><strong>$00.00</strong></td></td>

                    </tr>
                    <tr>
                      <td class="text-right no-border"><strong>Iva 12%</strong></td>
                      <td class="text-right"><strong>$0.00</strong></td>
                    </tr>
                   
                    <tr>
                      <td class="text-right no-border">
                        <div class="well well-small rojo"><strong>Total</strong></div>
                      </td>
                      <td class="text-right"><strong>$00.00</strong></td>
                    </tr>
                  </tbody>
                </table>
                </div>
                   <div class="form-group">
                         <div class="alert alert-warning" id="banmsg_det" runat="server" clientidmode="Static"><b>Error!</b> >Debe ingresar el número de la carga MRN......</div>
                  </div>
                  <div class="white-panel mt" runat="server" id="cargar" clientidmode="Static">
                      <div class="panel-body">
                            <div align="center">    
                             <img alt="loading.." src="../lib/file-uploader/img/loading.gif"  class="nover"  id="ImgCarga"  runat="server" />
                                </div>
                      </div>    
                  </div>  

                    <div class="d-flex justify-content-end mt-4">                      
                            <asp:Button ID="BtnFacturar" runat="server" class="btn btn-primary"  Text="GENERAR FACTURA"   
                                  onclick="BtnFacturar_Click"  OnClientClick="return confirmacion()"    />
                        &nbsp;&nbsp;
                            <asp:Button ID="BtnVisualizar" runat="server" class="btn btn-primary" Text="IMPRIMIR FACTURA"    OnClick="BtnVisualizar_Click" />
                          
                  </div><!--btn-group-justified-->                  
               </div> <!-- /invoice-body -->
                   </ContentTemplate>
                         <Triggers>
                          <asp:AsyncPostBackTrigger ControlID="BtnFacturar" />
                        </Triggers>
                        </asp:UpdatePanel>

            </div><!--col-lg-12 col-lg-offset-1-->

         </div>
     </div>
 
 </div>
  
    <asp:ModalPopupExtender ID="mpeLoading" runat="server" BehaviorID="idmpeLoading"
        PopupControlID="pnlLoading" TargetControlID="btnLoading" EnableViewState="false"
        DropShadow="true" BackgroundCssClass="modalBackground" />

    <asp:Panel ID="pnlLoading" runat="server"  HorizontalAlign="Center"
        CssClass="modalPopup" align="center"  EnableViewState="false" Style="display: none">
        <br />Generando Factura...
         <div class="body2">
             <asp:UpdatePanel ID="msgload" runat="server" UpdateMode="Conditional"  >
               <ContentTemplate>
           <div align="center">   
              
               <asp:Image ID="loading" runat="server" ImageUrl="../lib/file-uploader/img/loading.gif"  Visible="true" Width="40" Height="40" />
             
            </div>
                  
            <br/>
            Estimado Cliente, se está generando la factura..No deberá cerrar la ventana hasta que el proceso de 
                   generación de factura lo efectué de forma automática. <br/>
            Al cerrar la ventana incurrirá en problemas con la emisión del pase de puerta….Si el proceso tarda, por favor notificar a nuestro Dpto. De Facturación: <br />
            ec.fac@contecon.com.ec
           <br />
                     </ContentTemplate>
                 
            </asp:UpdatePanel>
        </div>


    </asp:Panel>
    <asp:Button ID="btnLoading" runat="server" Style="display: none" />
    <!-- Modal -->


 
  <script type="text/javascript" src="../lib/pages.js" ></script>


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
        var opcion = confirm("Estimado cliente, está seguro que desea generar la factura. ?");
        if (opcion == true)
        {
            loader();
            return true;
        } else
        {
            //loader();
	         return false;
        }

       
    }
    function mostrarloader()
    {

             document.getElementById("ImgCarga").className = 'ver';
      
    }


    function ocultarloader()
    {
        
       document.getElementById("ImgCarga").className = 'nover';
      
    }

    

  </script>



<script type="text/javascript">

    function loader() {
        $(document).ready(function () {
            var unique_id = $.gritter.add({
                // (string | mandatory) the heading of the notification
                title: 'PROCESANDO FACTURA ',
                // (string | mandatory) the text inside the notification
                text: 'Estimado Cliente, se está procesando la factura, por favor espere hasta que se culmine la generación de la misma. No deberá cerrar la ventana hasta que el proceso de generación de factura emita un número secuencial y habilite el botón para imprimir la factura. Al cerrar la ventana incurrirá en problemas con la emisión del pase de puerta….Si el proceso tarda, por favor notificar a nuestro Dpto. De Facturación:  ec.fac@contecon.com.ec',
                // (string | optional) the image to display on the left
                image: '../img/master.png',
                // (bool | optional) if you want it to fade out on its own or just sit there
                sticky: false,
                // (int | optional) the time you want it to be alive for before fading out
                time: 10000,
                // (string | optional) the class name you want to apply to that specific message
                class_name: 'my-sticky-class'
            });

            return false;
        });
    }
  </script>

</asp:Content>
