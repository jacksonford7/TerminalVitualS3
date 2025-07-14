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


<%--  <link href="../img/favicon2.png" rel="icon"/>
  <link href="../img/icono.png" rel="apple-touch-icon"/>
 
  <link href="../lib/bootstrap/css/bootstrap.min.css" rel="stylesheet"/>

  <link href="../lib/font-awesome/css/font-awesome.css" rel="stylesheet" />
  <link rel="stylesheet" type="text/css" href="../lib/gritter/css/jquery.gritter.css" />


  <link href="../lib/advanced-datatable/css/demo_page.css" rel="stylesheet" />
  <link href="../lib/advanced-datatable/css/demo_table.css" rel="stylesheet" />
  <link rel="stylesheet" href="../lib/advanced-datatable/css/DT_bootstrap.css" />
   

  <link rel="stylesheet" href="../lib/file-uploader/css/jquery.fileupload.css"/>
  <link rel="stylesheet" href="../lib/file-uploader/css/jquery.fileupload-ui.css"/>


  <link href="../css/style.css" rel="stylesheet"/>
  <link href="../css/style-responsive.css" rel="stylesheet"/>
  <link href="../css/pagination.css" rel="stylesheet"/>
 <link href="../css/calendario_ajax.css" rel="stylesheet"/>--%>

  
 

<script type="text/javascript">

$(document).ready(function(){
  $('[data-toggle="tooltip"]').tooltip();   
});
</script>




<script type="text/javascript">

 function BindVolumen() {

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
               <div class="form-group col-md-2">
                 <label for="inputZip">&nbsp;</label>
                   <div class="d-flex">
                        <asp:Button ID="BtnBuscarCarga" runat="server" class="btn btn-primary"   Text="BUSCAR"  OnClientClick="return mostrarloader('1')" OnClick="BtnBuscarCarga_Click" />                       
                        <img alt="loading.." src="../lib/file-uploader/img/loading.gif" height="32px" width="32px"  id="ImgCarga" class="nover"   />     
                  </div>
            </div>
             
            <div class="form-group col-md-2">
                 <label for="inputZip">&nbsp;</label>
                 <div class="d-flex"></div>
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
             <div class="form-group col-md-2">
               <label for="inputZip">VOLUMEN (MT)<span style="color: #FF0000; font-weight: bold;"></span></label>
                  <asp:TextBox ID="TxtTotalVolumen" runat="server" class="form-control text-center"  MaxLength="10"  onkeypress="return soloLetras(event,'0123456789.')" 
                                    placeholder="" disabled></asp:TextBox>
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
  
      
  </script>
    <script type="text/javascript">
            $(document).ready(function () {
                 $('.datetimepicker').datetimepicker().datetimepicker({ lang: 'es', timepicker: true, step: 30, format: 'm/d/Y H:i' });
              });    
      </script>

</asp:Content>