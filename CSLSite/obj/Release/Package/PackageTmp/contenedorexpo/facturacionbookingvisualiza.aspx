


<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="facturacionbookingvisualiza.aspx.cs" Inherits="CSLSite.contenedorexpo.facturacionbookingvisualiza" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="placehead" runat="server">
    <link href="../img/favicon2.png" rel="icon"/>
  <link href="../img/icono.png" rel="apple-touch-icon"/>
  <!-- Bootstrap core CSS -->
  <link href="../lib/bootstrap/css/bootstrap.min.css" rel="stylesheet"/>
  <!--external css-->
  <link href="../lib/font-awesome/css/font-awesome.css" rel="stylesheet" />
  <link rel="stylesheet" type="text/css" href="../lib/gritter/css/jquery.gritter.css" />



   <!-- Custom styles for this template -->
   <link href="../css/style.css" rel="stylesheet"/>
  <link href="../css/style-responsive.css" rel="stylesheet"/>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="placebody" runat="server">
  <div id="div_BrowserWindowName" style="visibility:hidden;">
    <asp:HiddenField ID="hf_BrowserWindowName" runat="server" />
   
  </div>
    <section id="main-content"> <!--main content start-->
      <section class="wrapper"> <!--wrapper-->
      <h3>PREVISUALIZACIÓN DE FACTURA EXPORTACIONES</h3>
        <div class="col-lg-12 mt"> <!--col-lg-12 mt-->
          <div class="row content-panel"> <!--row content-panel-->
            <div class="col-lg-10 col-lg-offset-1"> <!--col-lg-10 col-lg-offset-1-->
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
                </div>
                <!-- /pull-left -->
                <div class="pull-right">
                  <h2>PREVISUALIZACIÓN DE FACTURA EXPORTACIONES</h2>
                </div>
                <!-- /pull-right -->
                <div class="clearfix"></div>
                <br/>
                <br/>
                <br/>
                <div class="row">
                  <div class="col-md-9">

                   <div class="form-group">
                        <div class="alert alert-warning" id="banmsg" runat="server" clientidmode="Static"><b>Error!</b> >......</div>
                   </div>

                 <%--  <strong><span id="agente" runat="server">.... </span></strong>--%>
                     <%-- </h4>--%>
                  <address>
                  <%--<strong><span id="cliente" runat="server">.... </span></strong><br/>--%>
                 <strong><span id="facturado" runat="server">.... </span></strong><br/><br/>
                  <strong><span id="observacion" runat="server">.... </span></strong><br/>
                <strong><span id="carga" runat="server">.... </span></strong><br/>
                <div class="form-group">
                <strong><span id="contenedor" runat="server" style="height:auto; width:auto;">.... </span></strong>
                </div>
                <br/>
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
                    <%--<div>
                      <!-- /col-md-3 -->
                      <div class="pull-left"><strong></strong></div>
                      <div class="pull-right"><strong> <span id="fecha_hasta" runat="server">.... </span> </strong></div>
                      <div class="clearfix"></div>
                   </div>--%>
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
                             <img alt="loading.." src="../lib/file-uploader/img/loading.gif"   id="ImgCarga" class="nover" runat="server" />
                                </div>
                      </div>    
                  </div>  

                    <div class="btn-group btn-group-justified">
                        <div class="btn-group">
                          <asp:Button ID="BtnNuevo" runat="server" class="btn btn-theme07"  Text="REGRESAR" OnClick="BtnNuevo_Click"   />
                          </div>
                          <div class="btn-group">
                           <%-- <asp:Button ID="BtnFacturar" runat="server" class="btn btn-theme07" Text="GENERAR FACTURA"   data-toggle="modal" data-target="#myModal"  OnClick="BtnFacturar_Click"  />
                          --%>
                               <asp:Button ID="BtnFacturar" runat="server" class="btn btn-theme07" Text="GENERAR FACTURA"   
                                  onclick="BtnFacturar_Click"  OnClientClick="return confirm('Estimado cliente, está seguro que desea registrar la pre-factura. ?');"   />
                          
                          </div>
                        <%--  <div class="btn-group">
                            <asp:Button ID="BtnVisualizar" runat="server" class="btn btn-theme07" Text="IMPRIMIR FACTURA"    OnClick="BtnVisualizar_Click" />
                          </div>--%>
                  </div><!--btn-group-justified-->                  
               </div> <!-- /invoice-body -->
                   </ContentTemplate>
                         <Triggers>
                          <asp:AsyncPostBackTrigger ControlID="BtnFacturar" />
                        </Triggers>
                        </asp:UpdatePanel>

              
 
            </div><!--col-lg-10 col-lg-offset-1-->
         </div><!--row content-panel-->
        </div><!--col-lg-12 mt-->
     </section>
      <!-- /wrapper -->
   </section>
      <!-- Modal -->
  
                <!-- Modal -->

   <!-- /MAIN CONTENT -->
  <script type="text/javascript" src="../lib/jquery/jquery.min.js"></script>

  <script type="text/javascript" src="../lib/bootstrap/js/bootstrap.min.js"></script>
  <script  type="text/javascript" src="../lib/jquery.dcjqaccordion.2.7.js"></script>
  <script type="text/javascript" src="../lib/jquery.scrollTo.min.js"></script>
  <script type="text/javascript" src="../lib/jquery.nicescroll.js" ></script>


  <!--common script for all pages-->
  <script type="text/javascript" src="../lib/common-scripts.js"></script>
  <script type="text/javascript" src="../lib/gritter/js/jquery.gritter.js"></script>
  <script type="text/javascript" src="../lib/gritter-conf.js"></script>
  <!--script for this page-->
 
 <script type="text/javascript" src="../lib/pages.js" ></script>
 <script type="text/javascript" src="../lib/advanced-form-components.js"></script>


<script type="text/javascript">

   

    function confirmacion()
    {
        var mensaje;
        var opcion = confirm("Estimado cliente, está seguro que desea generar la factura. ?");
        if (opcion == true)
        {
            mostrarloader();
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


    function ocultarloader()
    {
        
       document.getElementById("ImgCarga").className = 'nover';
      
    }


  </script>

</asp:Content>
