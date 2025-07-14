<%@ Page Title="" Language="C#" MasterPageFile="~/site.Master" AutoEventWireup="true" CodeBehind="p2d_facturafreightforwarder.aspx.cs" Inherits="CSLSite.p2d_facturafreightforwarder" %>
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



      <style type="text/css">
        body2
        {
            font-family: Arial;
            font-size: 10pt;
        }
        .modal2
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
$(document).ready(function(){
  $('[data-toggle="tooltip"]').tooltip();   
});
</script>

<script type="text/javascript">


 function BindFunctions() {

     $(document).ready(function() {
      /*
       * Insert a 'details' column to the table
       */
      var nCloneTh = document.createElement('th');
      var nCloneTd = document.createElement('td');
      nCloneTd.innerHTML = '<img src="../lib/advanced-datatable/images/details_open.png">';
      nCloneTd.className = "center";

      $('#<%= tablePagination.ClientID %> thead tr').each(function() {
        //this.insertBefore(nCloneTh, this.childNodes[0]);
      });

      $('#<%= tablePagination.ClientID %> tbody tr').each(function() {
        //this.insertBefore(nCloneTd.cloneNode(true), this.childNodes[0]);
      });

      /*
       * Initialse DataTables, with no sorting on the 'details' column
       */
      var oTable = $('#<%= tablePagination.ClientID %>').dataTable({
        "aoColumnDefs": [{
          "bSortable": false,
          "aTargets": [0]
        }],
        "aaSorting": [
          [1, 'asc']
        ]
      });

      /* Add event listener for opening and closing details
       * Note that the indicator for showing which row is open is not controlled by DataTables,
       * rather it is done here
       */
      $('#<%= tablePagination.ClientID %> tbody td img').live('click', function() {
        var nTr = $(this).parents('tr')[0];
        if (oTable.fnIsOpen(nTr)) {
          /* This row is already open - close it */
          this.src = "../lib/advanced-datatable/media/images/details_open.png";
          oTable.fnClose(nTr);
        } else {
          /* Open this row */
          this.src = "../lib/advanced-datatable/images/details_close.png";
          oTable.fnOpen(nTr, fnFormatDetails(oTable, nTr), 'details');
        }
      });
        });
    }
</script>

 <script type="text/javascript">
        $(document).ready(function () {
            $('#<%= tablePagination.ClientID %>').dataTable();
        });
       
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
            <li class="breadcrumb-item active" aria-current="page" id="sub_opcion" runat="server">FACTURACIÓN CARGA SUELTA (FREIGHT FORWARDER)</li>
          </ol>
        </nav>
      </div>

<div class="dashboard-container p-4" id="cuerpo" runat="server">
    <div class="form-title">
           DATOS DE LA CARGA
     </div>
		
     
        <h4 class="mb">DATOS DE LA FACTURA</h4>

        <asp:UpdatePanel ID="UPDATOSCLIENTE" runat="server"  UpdateMode="Conditional" ChildrenAsTriggers="true">
        <ContentTemplate>
        <div class="form-row" >
             <div class="form-group col-md-6" runat="server" visible="false"> 
                  <label for="inputAddress">AGENTE DE ADUANA:<span style="color: #FF0000; font-weight: bold;"></span></label>
                  <asp:HiddenField ID="hf_idagente" runat="server" />
                    <asp:HiddenField ID="hf_descagente" runat="server" />
                <asp:HiddenField ID="hf_rucagente" runat="server" />
                  <asp:TextBox ID="TXTAGENCIA" runat="server" class="form-control"    placeholder=""  Font-Bold="false" disabled ></asp:TextBox>
            </div>
             <div class="form-group col-md-6" runat="server" visible="false"> 
                 <label for="inputAddress">CLIENTE:<span style="color: #FF0000; font-weight: bold;"></span></label>
                        <asp:HiddenField ID="hf_idcliente" runat="server" />
                        <asp:HiddenField ID="hf_desccliente" runat="server" />
					    <asp:TextBox ID="TXTCLIENTE" runat="server" class="form-control"    placeholder=""  Font-Bold="false" disabled></asp:TextBox>                
             </div>
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

            
       </div>
                        
        </ContentTemplate>
            <Triggers>
            <asp:AsyncPostBackTrigger ControlID="BtnBuscar" />
        </Triggers>
        </asp:UpdatePanel>
  
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
                 <label for="inputZip">&nbsp;</label>
                   <div class="d-flex">
                        <asp:Button ID="BtnBuscar" runat="server" class="btn btn-primary"   Text="BUSCAR"  OnClientClick="return mostrarloader('1')" OnClick="BtnBuscar_Click" />   
                         <img alt="loading.." src="../lib/file-uploader/img/loading.gif" height="32px" width="32px"  id="ImgCarga" class="nover"   />
                         
                  </div>
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

     <div class="form-row">
            <div class="form-group col-md-12">  
                 <asp:UpdatePanel ID="UPCARGAS" runat="server"  >  
                 <ContentTemplate>
                        <h3 id="H1" runat="server">DETALLE DE CARGAS A FACTURAR</h3>

                      <asp:Repeater ID="grillacargas" runat="server"  >
                       <HeaderTemplate>
                       <table cellpadding="0" cellspacing="0" border="0" class="table table-bordered invoice" id="grillacargas" width="100%" >
                        <thead>
                          <tr >
                            <th class="center hidden-phone" >CARGA</th>
                            <th class="center hidden-phone">RUC</th>
                            <th class="center hidden-phone">IMPORTADOR</th>
                            <th class="left hidden-phone" >DESCRIPCION</th>
                            <th class="center hidden-phone">BULTOS</th>
                         
                            <th class="center hidden-phone" style="align-items:center">APLICAR</th>
                          </tr>
                        </thead>
                        <tbody>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr >
                                <td class="center hidden-phone" ><asp:Label Text='<%#Eval("numero_carga")%>' ID="numero_carga" runat="server"  style="align-content:center"/> </td>
                                <td class="center hidden-phone"><asp:Label Text='<%#Eval("importador_id")%>' ID="importador_id" runat="server"  style="align-content:center"   /></td>
                                <td class="center hidden-phone"><asp:Label Text='<%#Eval("importador_name")%>' ID="importador_name" runat="server"  style="align-content:center"   /></td>
                                <td class="left hidden-phone"><asp:Label Text='<%#Eval("descripcion")%>' ID="descripcion" runat="server"  style="text-align:left"  /></td>
                                <td class="center hidden-phone"><asp:Label Text='<%#Eval("total_partida")%>' ID="total_partida" runat="server"  style="text-align:center"  /></td> 
                               
                                <td class="center hidden-phone" > 
                                 <asp:UpdatePanel ID="UPSELECCIONARTARJA" runat="server" ChildrenAsTriggers="true">
                                        <ContentTemplate>
                                           
                                             <label class="checkbox-container">
                                            <asp:CheckBox id="chkSeleccionar" runat="server"  Checked='<%#Eval("visto")%>' AutoPostBack="True"  OnCheckedChanged="chkSeleccionar_CheckedChanged" />
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
            </div>
     </div>

    <div class="form-row">  

        <div class="form-group col-md-12">
             <asp:UpdatePanel ID="UPDETALLE" runat="server" UpdateMode="Conditional" >  
               <ContentTemplate>
         
      
            
                   <h3 id="LabelTotal" runat="server">DETALLE DE CARGAS A FACTURAR</h3>
 
                    <asp:GridView ID="tablePagination" runat="server" AutoGenerateColumns="False"  DataKeyNames="NUMERO_CARGA"
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
                                                <asp:BoundField DataField="NUMERO_CARGA" HeaderText="CARGA"  HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"/>
                                                  
                                                <asp:BoundField DataField="IN_OUT" HeaderText="ESTADO"  HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                                <asp:BoundField DataField="DOCUMENTO" HeaderText="DOCUMENTO"   HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                                <asp:BoundField DataField="P2D_DESC_AGENTE" HeaderText="AGENTE"  DataFormatString="{0:yyyy/MM/dd}" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"  Visible="true" />
                                                <asp:BoundField DataField="P2D_DESC_CLIENTE" HeaderText="IMPORTADOR"  HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"  Visible="true"/>
                                                <asp:BoundField DataField="P2D_DIRECCION" HeaderText="DIRECCION ENTREGA"  HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"  Visible="true"/>
                                                <asp:BoundField DataField="AUTORIZADO" HeaderText="AUTORIZADO"  HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" Visible="false"/>
                                                <asp:BoundField DataField="DES_BLOQUEO" HeaderText="BLOQUEADO"  HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" Visible="false"/>
                                                <asp:BoundField DataField="DESCRIPCION"   HeaderText="DESCRIPCION"  HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                       
                                                <asp:BoundField DataField="CANTIDAD" HeaderText="CANTIDAD"  HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"  Visible="true"/>
                                                <asp:BoundField DataField="IMDT" HeaderText="IMDT"  HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"  Visible="false"/>
                                                <asp:BoundField DataField="ESTADO_RDIT" HeaderText="ESTADO RDIT"  HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"  Visible="false"/>
                                                <asp:BoundField DataField="GKEY" HeaderText="GKEY"  HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"  Visible="false"/>
                                                <asp:BoundField DataField="IDPLAN" HeaderText="IDPLAN"  HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"  Visible="false"/>
                                      
                                                <asp:BoundField DataField="TIENE_CERTIFICADO" HeaderText="TIENE CERTIFICADO"   Visible="false"/>

                                                <asp:TemplateField HeaderText="QUITAR" ItemStyle-CssClass="center hidden-phone" Visible="true">
                                                    <ItemTemplate>
                                                        <asp:UpdatePanel ID="UpdatePanel1" runat="server" ChildrenAsTriggers="true">
                                                        <ContentTemplate>
                                                    
                                                    
                                                            <asp:Button ID="BtnquitarReg" runat="server" OnClick="BtnquitarReg_Click" class="btn btn-primary" runat="server"  Text="QUITAR"/>
                                                   
                                                        </ContentTemplate>
                                                       <Triggers>
                                                       <asp:AsyncPostBackTrigger ControlID="CHKFA" />
                                                       </Triggers>
                                                    </asp:UpdatePanel>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
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

               
                    
                   <asp:Button ID="BtnFacturar" runat="server" class="btn btn-primary" Text="GENERAR FACTURA"  data-toggle="modal" data-target="#exampleModalToggle"  onclick="BtnFacturar_Click" />

               

               </div> 
             </div>
            </ContentTemplate>
           </asp:UpdatePanel>   
   


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

          <asp:UpdatePanel ID="UPCARGA2" runat="server"  UpdateMode="Conditional" ChildrenAsTriggers="true">
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
                                  <div class="pull-right"><strong><span id="fecha_factura" runat="server">.... </span> </strong></div>
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
                                  <div class="pull-left"> Total a Facturar : </div>
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
                                     <div class="alert alert-warning" id="banmsg_det2" runat="server" clientidmode="Static"><b>Error!</b> >Debe ingresar el número de la carga MRN......</div>
                              </div>
                              <div class="white-panel mt" runat="server" id="cargar" clientidmode="Static">
                                  <div class="panel-body">
                                        <div align="center">    
                                         <img alt="loading.." src="../lib/file-uploader/img/loading.gif"  class="nover"  id="ImgFactura"  runat="server" />
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
              
                <asp:Button ID="BtnConfirmar" runat="server" class="btn btn-primary"  Text="GENERAR FACTURA"    OnClientClick="return confirmacion()"    onclick="BtnConfirmar_Click"  />
                 &nbsp;&nbsp;
                 <asp:Button  ID="BtnVisualizar" runat="server" class="btn btn-primary" Text="IMPRIMIR FACTURA" OnClick="BtnVisualizar_Click"     />
                   &nbsp;&nbsp;            
    
                <button type="button" class="btn btn-outline-primary " data-dismiss="modal">Cancelar</button>
                 </ContentTemplate>
        
            </asp:UpdatePanel>
      </div>


    </div>
  </div>
</div>
       
    
</div>

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
            Estimado Usuario, se está generando su solicitud.....por favor espere.... <br/>
           
           <br />
                     </ContentTemplate>
                 
            </asp:UpdatePanel>
        </div>


    </asp:Panel>
    <asp:Button ID="btnLoading" runat="server" Style="display: none" />
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

       <script type="text/javascript">
            $(document).ready(function () {
                 $('.datetimepicker').datetimepicker().datetimepicker({ lang: 'es', timepicker: false, step: 30, format: 'm/d/Y' });
              });    
      </script>

     
    <script type="text/javascript">

  

      function mostrarloader_factura()
    {

             document.getElementById("ImgFactura").className = 'ver';
      
    }


    function ocultarloader_factura()
    {
        
       document.getElementById("ImgFactura").className = 'nover';
      
    }

</script>

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


</asp:Content>