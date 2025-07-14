<%@ Page Title="" Language="C#" MasterPageFile="~/site.Master" AutoEventWireup="true" CodeBehind="proformaapertura.aspx.cs" Inherits="CSLSite.proformaapertura" %>
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


 <%-- <link rel="stylesheet" type="text/css" href="../lib/bootstrap-datepicker/css/datepicker.css" />
  <link rel="stylesheet" type="text/css" href="../lib/bootstrap-daterangepicker/daterangepicker.css" />
  <link rel="stylesheet" type="text/css" href="../lib/bootstrap-timepicker/compiled/timepicker.css" />
  <link rel="stylesheet" type="text/css" href="../lib/bootstrap-datetimepicker/datertimepicker.css" />

   <!-- Custom styles for this template -->
   <link href="../css/style.css" rel="stylesheet"/>
  <link href="../css/style-responsive.css" rel="stylesheet"/>--%>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="placebody" runat="server">

    <asp:ToolkitScriptManager ID="tkscata" runat="server"></asp:ToolkitScriptManager>

     <div class="mt-4">
        <nav class="mt-4" aria-label="breadcrumb">
          <ol class="breadcrumb">
            <li class="breadcrumb-item" id="opcion_principal" runat="server"><a href="#">FREIGHT FORWARDER</a></li>
            <li class="breadcrumb-item active" aria-current="page" id="sub_opcion" runat="server">PROFORMA CONTENEDOR IMPORTACION</li>
          </ol>
        </nav>
    </div>
  <div class="dashboard-container p-4" id="cuerpo" runat="server">
      <div class="form-row">
     
      
        <div class="form-group col-md-12"> <!--col-lg-12 mt-->

            <div class="col-lg-12 col-lg-offset-1"> <!--col-lg-10 col-lg-offset-1-->
                 
                <asp:UpdatePanel ID="UPDETALLE" runat="server"  UpdateMode="Conditional" ChildrenAsTriggers="true">
                <ContentTemplate>
                <div class="invoice-body"><!--proforma-body-->
                <div class="pull-left">
                  <%--<h1>EMPRESA</h1>--%>
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
                  <h2>PROFORMA</h2>
                </div>
                <!-- /pull-right -->
                <div class="clearfix"></div>
                <br/>
        
                <div class="row">
                  <div class="col-md-9">

                  

                   <strong><span id="agente" runat="server">.... </span></strong>
                   <asp:HiddenField ID="hf_idagente" runat="server" />
                   <asp:HiddenField ID="hf_BrowserWindowName" runat="server" />
                     <%-- </h4>--%>
                  <address>
                  <strong><span id="cliente" runat="server">.... </span></strong><br/>
                 <asp:HiddenField ID="hf_idcliente" runat="server" />
                 <strong><span id="facturado" runat="server">.... </span></strong><br/><br/>
                 <asp:HiddenField ID="hf_idfacturado" runat="server" />
                  <strong><span id="observacion" runat="server">.... </span></strong><br/>
                <strong><span id="carga" runat="server">.... </span></strong><br/>
                <asp:HiddenField ID="hf_idcarga" runat="server" />
                <span id="contenedor" runat="server">.... </span><br/>
                </address>
                  </div>
                  <!-- /col-md-9 -->
                  <div class="col-md-3">
                    <br/>
                    <div>
                      <div class="pull-left"><h4> PROFORMA #: </h4></div>
                      <div class="pull-right"><h4> <span id="numero" runat="server">.... </span> </h4></div>
                      <div class="clearfix"></div>
                    </div>
                    <div>
                      <!-- /col-md-3 -->
                      <div class="pull-left"><strong>FECHA GENERACION: </strong></div>
                      <div class="pull-right"><strong><span id="fecha" runat="server">.... </span> </strong></div>
                      <div class="clearfix"></div>
                   </div>
                    <div>
                      <!-- /col-md-3 -->
                      <div class="pull-left"><strong>  </strong></div>
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
                      <%--<th style="width:60px" class="text-center">CARGA</th>--%>
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
                        <p>Gracias por realizar la proforma. Esperamos el pago dentro de los 21 días, así que procese esta factura dentro de ese tiempo. Habrá un cargo de interés del 1.5% por mes en las facturas atrasadas.</p>
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

                 <div class="d-flex justify-content-end mt-4">
                        
                          <asp:Button ID="BtnNuevo" runat="server" class="btn btn-primary"  Text="REGRESAR" OnClick="BtnNuevo_Click"  />
                            &nbsp;&nbsp;&nbsp;&nbsp;
                         
                            <asp:Button ID="BtnFacturar" runat="server" class="btn btn-primary" Text="IMPRIMIR PROFORMA" OnClick="BtnFacturar_Click" />
                          
                     </div><!--btn-group-justified-->
                         
               </div> <!-- /invoice-body -->
                   </ContentTemplate>
                         <Triggers>
                          <asp:AsyncPostBackTrigger ControlID="BtnFacturar" />
                        </Triggers>
                        </asp:UpdatePanel>
              
            </div><!--col-lg-10 col-lg-offset-1-->
     
        </div><!--col-lg-12 mt-->
    
     </div>
   </div>
    
    <!-- /MAIN CONTENT -->
 <%-- <script type="text/javascript" src="../lib/jquery/jquery.min.js"></script>
  <script type="text/javascript" language="javascript" src="../lib/advanced-datatable/js/jquery.js"></script>

  <script type="text/javascript" src="../lib/bootstrap/js/bootstrap.min.js"></script>
  <script  type="text/javascript" src="../lib/jquery.dcjqaccordion.2.7.js"></script>
  <script type="text/javascript" src="../lib/jquery.scrollTo.min.js"></script>
  <script type="text/javascript" src="../lib/jquery.nicescroll.js" ></script>
  <script type="text/javascript" src="../lib/jquery.sparkline.js"></script>

  <script type="text/javascript" language="javascript" src="../lib/advanced-datatable/js/jquery.dataTables.js"></script>
  <script type="text/javascript" src="../lib/advanced-datatable/js/DT_bootstrap.js"></script>

  <!--common script for all pages-->
  <script type="text/javascript" src="../lib/common-scripts.js"></script>
  <script type="text/javascript" src="../lib/gritter/js/jquery.gritter.js"></script>
  <script type="text/javascript" src="../lib/gritter-conf.js"></script>
  <!--script for this page-->--%>
 
  <script type="text/javascript" src="../lib/pages.js" ></script>
 
<%-- <script type="text/javascript" src="../lib/advanced-form-components.js"></script>--%>

 <%--<script type="text/javascript">

    function openPopReporte(opcion) {
    window.open('../reportes/wbreporteproforma.aspx?id_comprobante='+opcion, 'name', 'width=700,height=700');
    return true;
}
</script>--%>

</asp:Content>
