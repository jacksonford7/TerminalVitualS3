<%@ Page Title="Cargas Pendientes de Facturar" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="cargas_pendientes.aspx.cs" Inherits="CSLSite.cargas_pendientes" %>
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
  <link href="../lib/font-awesome/css/font-awesome.css" rel="stylesheet" />
  <link rel="stylesheet" type="text/css" href="../lib/gritter/css/jquery.gritter.css" />


  <link href="../css/datatables.min.css" rel="stylesheet" />


   

</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="placebody" runat="server" >

     <asp:ToolkitScriptManager ID="tkscata" runat="server"></asp:ToolkitScriptManager>

     <asp:HiddenField ID="manualHide" runat="server" />

  <div id="div_BrowserWindowName" style="visibility:hidden;">
    <asp:HiddenField ID="hf_BrowserWindowName" runat="server" />
     
  </div>

     <div class="mt-4">         
        <nav class="mt-4" aria-label="breadcrumb">
          <ol class="breadcrumb">
            <li class="breadcrumb-item" id="opcion_principal" runat="server"><a href="#">MULTIDESPACHOS</a></li>
            <li class="breadcrumb-item active" aria-current="page" id="sub_opcion" runat="server">CARGAS CFS</li>
          </ol>
        </nav>
      </div>

 <div class="dashboard-container p-4" id="cuerpo" runat="server">

      <h4 class="mb">DATOS DE LA FACTURA</h4>
     <div class="form-row">
          <div class="form-group col-md-12"> 
                 <label for="inputAddress">FACTURADO A:<span style="color: #FF0000; font-weight: bold;">*</span></label>
                
                    <asp:HiddenField ID="hf_idasume" runat="server" />
                    <asp:HiddenField ID="hf_descasume" runat="server" />
                               
                    <asp:DropDownList runat="server" ID="CboAsumeFactura"    AutoPostBack="false"  class="form-control"  >
                        </asp:DropDownList>
             </div> 
     </div>
     <div class="form-title">
          CARGAS PENDIENTES DE FACTURAR
     </div>
      <div class="form-group">
          <div class="alert alert-success" id="Div1" runat="server" clientidmode="Static"><b>Para el servicio de Multidespacho, debe seleccionar más de una carga.</b> </div>
       </div>

      <div class="row">
       <div class="col-md-12 d-flex justify-content-center">
           <div class="alert alert-warning" id="banmsg" runat="server" clientidmode="Static"><b>Error!</b> >......</div>
         
       </div>
     </div>


      <div class="form-row">
          <div class="form-group col-md-12">
         <asp:UpdatePanel ID="UPDETALLE" runat="server"  >  
            <ContentTemplate>

          
                       <asp:Repeater ID="grilla" runat="server" onitemcommand="grilla_ItemCommand" >
                       <HeaderTemplate>
                       <table cellpadding="0" cellspacing="0" border="0" class="table table-bordered table-sm table-contecon" id="grilla" width="100%" >
                        <thead>
                          <tr >
                            <th class="center hidden-phone" style="width:240px">CARGA</th>
                            <th class="center hidden-phone">RUC</th>
                            <th class="center hidden-phone">IMPORTADOR</th>
                            <th class="center hidden-phone">DESCARGA</th>
                            <th class="left hidden-phone" >DESCRIPCION</th>
                            <th class="center hidden-phone">BULTOS</th>
                            <th class="center hidden-phone">VOLUMEN</th>
                            <th class="center hidden-phone">PESO</th>
                            <th class="center hidden-phone" style="align-items:center">APLICAR</th>
                          </tr>
                        </thead>
                        <tbody>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr >
                                <td class="center hidden-phone"  style="width:240px"><asp:Label Text='<%#Eval("numero_carga")%>' ID="numero_carga" runat="server"  style="align-content:center"/> </td>
                                <td class="center hidden-phone"><asp:Label Text='<%#Eval("importador_id")%>' ID="importador_id" runat="server"  style="align-content:center"   /></td>
                                <td class="center hidden-phone"><asp:Label Text='<%#Eval("importador_name")%>' ID="importador_name" runat="server"  style="align-content:center"   /></td>
                                <td class="center hidden-phone"><asp:Label Text='<%#DataBinder.Eval(Container.DataItem, "descarga", "{0:yyyy/MM/dd HH:mm}")%>' ID="descarga" runat="server"  style="text-align:center"  /></td>
                                <td class="left hidden-phone"><asp:Label Text='<%#Eval("descripcion")%>' ID="descripcion" runat="server"  style="text-align:left"  /></td>
                                <td class="center hidden-phone"><asp:Label Text='<%#Eval("total_partida")%>' ID="total_partida" runat="server"  style="text-align:center"  /></td> 
                                <td class="center hidden-phone"><asp:Label Text='<%#Eval("volumen")%>' ID="volumen" runat="server"  style="text-align:center"  /></td> 
                                <td class="center hidden-phone"><asp:Label Text='<%#Eval("peso")%>' ID="peso" runat="server"  style="text-align:center"  /></td> 
                                <td class="center hidden-phone" style="align-items:center"> 
                                 <asp:UpdatePanel ID="UPSELECCIONARTARJA" runat="server" ChildrenAsTriggers="true">
                                        <ContentTemplate>
                                           
                                             <label class="checkbox-container">
                                            <asp:CheckBox id="chkSeleccionar" runat="server"  Checked='<%#Eval("visto")%>' AutoPostBack="True" OnCheckedChanged="chkSeleccionar_CheckedChanged" />
                                             <span class="checkmark"></span>
                                             </label>
                                        </ContentTemplate>
                                         <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="chkSeleccionar" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                 </td>

                                
                             </tr>    
                       </ItemTemplate>
                       <FooterTemplate>
                        </tbody>
                      </table>
                     </FooterTemplate>
                    </asp:Repeater>

          </ContentTemplate>
       
           </asp:UpdatePanel>

               <asp:UpdatePanel ID="UPBOTONES" runat="server"  UpdateMode="Conditional" ChildrenAsTriggers="true">
              <ContentTemplate>
                  <table class="table table-bordered">
                  <tbody>
                    <tr>
                      <td>
                          <label for="inputAddress">Total de Bultos: <span style="color: #FF0000; font-weight: bold;"></span></label>
                       <asp:TextBox ID="TxtTotalBultos" runat="server" class="form-control"      placeholder=""  Font-Bold="false" disabled  ></asp:TextBox>

                      </td>
                      <td> <label for="inputAddress">Total Volumen: <span style="color: #FF0000; font-weight: bold;"></span></label> 
                           <asp:TextBox ID="TxtTotalVolumen" runat="server" class="form-control"    placeholder=""  Font-Bold="false" disabled ></asp:TextBox>
                      </td>
                      <td><label for="inputAddress">Total Peso: <span style="color: #FF0000; font-weight: bold;"></span></label>  
                           <asp:TextBox ID="TxtTotalPeso" runat="server" class="form-control"    placeholder=""  Font-Bold="false" disabled ></asp:TextBox>
                      </td>
                    </tr>
                   
                  </tbody>
                </table>
                 <div class="form-row">
                        <div class="col-md-12 d-flex justify-content-center">
                           
                            <asp:Button ID="BtnFacturar" runat="server"  class="btn btn-primary"  Text="GENERAR FACTURA" data-toggle="modal" data-target="#exampleModalToggle"  onclick="BtnFacturar_Click"  />    
                           
                      </div>
                </div>
             </ContentTemplate>
            </asp:UpdatePanel>
         </div>
     </div>


 <div class="modal fade" id="exampleModalToggle" tabindex="-1" role="dialog"  >
  <div class="modal-dialog modal-dialog-scrollable" style="max-width: 1200px">
    <div class="modal-content">
       <div class="modal-header">
           <asp:UpdatePanel ID="UPTITULO" runat="server"  UpdateMode="Conditional" ChildrenAsTriggers="true">
           <ContentTemplate>
                 <h5 class="modal-title" id="Titulo" runat="server">FACTURA DE MULTIDESPACHOS</h5>       
           </ContentTemplate>
         </asp:UpdatePanel>
            <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                        </button>
       </div>
      <div class="modal-body">

          <asp:UpdatePanel ID="UPCARGA" runat="server"  UpdateMode="Conditional" ChildrenAsTriggers="true">
          <ContentTemplate>
             

                 <div class="form-row">
                     <div class="form-group col-md-12"> 
      
                        <div class="col-lg-12 col-lg-offset-1"> 
                         
                            <div class="invoice-body">

                            <div class="pull-left">
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
                              <h2>PREVISUALIZACIÓN DE FACTURA</h2>
                            </div>

                           
                            <!-- /pull-right -->
                            <div class="clearfix">
                                
                            </div>
                            <br/>

                             <div class="row">
                                 <div class="form-group">
                                     <div class="alert alert-warning" id="banmsg_cab" runat="server" clientidmode="Static"><b>Error!</b> >Debe ingresar el número de la carga MRN......</div>
                                 </div>
                             </div>

                            <div class="row">
                              <div class="col-md-9">

                              <strong><span id="lbl_agente" runat="server" clientidmode="Static">.... </span></strong>
                              <address>
                             <strong><span id="lbl_facturado" runat="server">.... </span></strong><br/><br/>
                              <strong><span id="lbl_observacion" runat="server">.... </span></strong><br/>
                            <strong><span id="lbl_carga" runat="server">.... </span></strong><br/>
               
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
                                  <div class="pull-left"><strong>FECHA: </strong></div>
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

                           </div> <!-- /invoice-body -->
                              
                        </div><!--col-lg-12 col-lg-offset-1-->

                     </div>
                 </div>
 
     
        


       </ContentTemplate>
         <Triggers>
                                      <asp:AsyncPostBackTrigger ControlID="BtnFacturar" />
                                     <asp:AsyncPostBackTrigger ControlID="BtnConfirmar" />
                                      <asp:AsyncPostBackTrigger ControlID="BtnVisualizar" />
                                    </Triggers>
    </asp:UpdatePanel>


     
        

      </div>
      <div class="modal-footer">
         <asp:UpdatePanel ID="UPBOTONESFACTURA" runat="server"  UpdateMode="Conditional" ChildrenAsTriggers="true">
          <ContentTemplate>     
      <asp:Button ID="BtnConfirmar" runat="server" class="btn btn-primary"  Text="GENERAR FACTURA"   
                                         onclick="BtnConfirmar_Click"        OnClientClick="return confirmacion()"    />
         &nbsp;&nbsp;
         <asp:Button ID="BtnVisualizar" runat="server" class="btn btn-primary" Text="IMPRIMIR FACTURA"   OnClick="BtnVisualizar_Click"  />
           &nbsp;&nbsp;            
    
        <button type="button" class="btn btn-outline-primary " data-dismiss="modal">Cancelar</button>
         </ContentTemplate>
        
    </asp:UpdatePanel>
      </div>
    </div>
  </div>
</div>

</div>





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

</script>

  <script type="text/javascript" src="../js/bootstrap.bundle.min.js"></script>

    <script type="text/javascript"  src="../js/datatables.js"></script>

   <script type="text/javascript" src="../lib/common-scripts.js"></script>
 
   <script type="text/javascript" src="../lib/pages.js" ></script>

   <script type="text/javascript" src="../lib/advanced-form-components.js"></script>

 

 

<script type="text/javascript">

  

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