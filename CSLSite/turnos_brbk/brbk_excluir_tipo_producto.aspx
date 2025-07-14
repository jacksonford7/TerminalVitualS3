<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="brbk_excluir_tipo_producto.aspx.cs" Inherits="CSLSite.brbk_excluir_tipo_producto" %>
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


  <link href="../lib/advanced-datatable/css/demo_page.css" rel="stylesheet" />
  <link href="../lib/advanced-datatable/css/demo_table.css" rel="stylesheet" />
  <link rel="stylesheet" href="../lib/advanced-datatable/css/DT_bootstrap.css" />
   
 
   <link href="../shared/estilo/jquery.datetimepicker.css" rel="stylesheet" type="text/css" />
   <script src="../Scripts/jquery.datetimepicker.js" type="text/javascript"></script>

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


<script type="text/javascript">


 function fechas()
   {
    $(document).ready(function () {
            $('.datetimepicker').datetimepicker().datetimepicker({ lang: 'es', step: 30, format: 'd/m/Y H:i' });
        });
      
        $(document).ready(function () {
            $('.datetimepickerAlt').datetimepicker().datetimepicker({ lang: 'es', closeOnDateSelect: true, timepicker: false, step: 30, format: 'd/m/Y' });
        });
     
        $(document).ready(function () {
            $('.datepicker').datetimepicker().datetimepicker({ lang: 'es', timepicker: false, format: 'd/m/Y', closeOnDateSelect: true, minDate: '0' });

        });

    }

 function BindFunctions() {

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

     <asp:HiddenField ID="manualHide" runat="server" />

  <div id="div_BrowserWindowName" style="visibility:hidden;">
    <asp:HiddenField ID="hf_BrowserWindowName" runat="server" />
     
  </div>

     <div class="mt-4">         
        <nav class="mt-4" aria-label="breadcrumb">
          <ol class="breadcrumb">
            <li class="breadcrumb-item" id="opcion_principal" runat="server"><a href="#">BREAK BULK</a></li>
            <li class="breadcrumb-item active" aria-current="page" id="sub_opcion" runat="server">EXCLUIR TIPO DE PRODUCTO (PARA SOLICITUDES)</li>
          </ol>
        </nav>
      </div>

 <div class="dashboard-container p-4" id="cuerpo" runat="server">

     <div class="form-title">
          DATOS DEL TIPO DE PRODUCTO
     </div>

      <asp:UpdatePanel ID="UPCARGA" runat="server"  UpdateMode="Conditional" ChildrenAsTriggers="true">
      <ContentTemplate>
            <script type="text/javascript">
                            Sys.Application.add_load(fechas); 
            </script>
     <div class="form-row">
              
             <div class="form-group col-md-2">
              <label for="inputZip">TIPO PRODUCTO</label>
                  <asp:DropDownList runat="server" ID="CboTipoProducto"    AutoPostBack="false"  class="form-control"  >
                     </asp:DropDownList>
            </div>
             
         
           <div class="form-group col-md-2">
                <label for="inputZip"> &nbsp;</label>
                <div class="d-flex">
               <asp:Button ID="BtnBuscar" runat="server" class="btn btn-primary"  Text="EXCLUIR"  OnClientClick="return mostrarloader('1')" OnClick="BtnBuscar_Click" />  
                   &nbsp;&nbsp;
                 <img alt="loading.." src="../lib/file-uploader/img/loading.gif" height="32px" width="32px"  id="ImgCarga" class="nover"   />   </div> 
           </div> 

           <br/>
              <div class="form-group col-md-12">
                <div class="alert alert-warning" id="banmsg" runat="server" clientidmode="Static"><b>Error!</b> >Debe ingresar el número de la carga MRN......</div>
            </div> 
     </div>
       </ContentTemplate>
        <Triggers>
        <asp:AsyncPostBackTrigger ControlID="BtnBuscar" />
    </Triggers>
    </asp:UpdatePanel>
   
     

       <div class="form-row">
           <br/>
     </div>

      <div class="form-row">
          <div class="form-group col-md-12">
            <asp:UpdatePanel ID="UPDETALLE" runat="server"  >  
            <ContentTemplate>

                       <asp:Repeater ID="tablePagination" runat="server"  onitemcommand="tablePagination_ItemCommand">
                       <HeaderTemplate>
                       <table cellpadding="0" cellspacing="0" border="0" class="table table-bordered invoice" id="hidden-table-info">
                           <thead>
                          <tr >
                            <th class="center hidden-phone">#</th>
                            <th class="center hidden-phone">CODIGO</th>
                            <th class="center hidden-phone">TIPO PRODUCTO</th>
                            <th class="center hidden-phone">USUARIO</th>
                            <th class="center hidden-phone">FECHA</th>
                           <th class="center hidden-phone">INCLUIR</th>
                          </tr>
                        </thead>
                        <tbody>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr >
                                <td class="center hidden-phone"><asp:Label Text='<%#Eval("SECUENCIA")%>' ID="SECUENCIA" runat="server"  style="align-content:center"   /></td>
                                 <td class="center hidden-phone"><asp:Label Text='<%#Eval("ID")%>' ID="ID" runat="server"  style="text-align:center"  /></td> 
                                <td class="center hidden-phone"><asp:Label Text='<%#Eval("DESCRIPCION")%>' ID="DESCRIPCION" runat="server"  style="text-align:center"  /></td> 
                                <td class="center hidden-phone"><asp:Label Text='<%#Eval("USUARIO_CREA")%>' ID="USUARIO_CREA" runat="server"  style="text-align:center"  /></td> 
                                <td class="center hidden-phone"><asp:Label Text='<%#DataBinder.Eval(Container.DataItem, "FECHA_CREA", "{0:yyyy/MM/dd HH:mm}")%>' ID="FECHA_CREA" runat="server"  style="text-align:center"  /></td>
                               
                                 <td class="center hidden-phone"> 
                                     <asp:Button ID="BtnActualizar"
                                       OnClientClick="return confirm('Esta seguro que desea quitar el registro?');" 
                                    CommandArgument= '<%#Eval("ID")%>' runat="server" Text="INCLUIR" class="btn btn-primary" ToolTip="INCLUIR" CommandName="Eliminar" />
                                </td>
                             </tr>    
                       </ItemTemplate>
                       <FooterTemplate>
                        </tbody>
                      </table>
                     </FooterTemplate>
                    </asp:Repeater>

            </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="BtnBuscar" />
                </Triggers>
            </asp:UpdatePanel>       
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
        var opcion = confirm("Estimado cliente, está seguro que desea generar los Turnos. ?");
        if (opcion == true)
        {
            loader();
            return true;
        } else
        {
	         return false;
        }
 
    }

</script>

  <!--common script for all pages-->
  <script type="text/javascript" src="../lib/common-scripts.js"></script>
  <script type="text/javascript" src="../lib/gritter/js/jquery.gritter.js"></script>
  <script type="text/javascript" src="../lib/gritter-conf.js"></script>
  <!--script for this page-->
 
  <script type="text/javascript" src="../lib/pages.js" ></script>
  

 <script type="text/javascript" src="../lib/advanced-form-components.js"></script>
 <script type="text/javascript" src="../lib/popup_script_cta.js" ></script>

<script type="text/javascript">

  $(document).ready(function () {
            $('.datetimepicker').datetimepicker().datetimepicker({ lang: 'es', step: 30, format: 'd/m/Y H:i' });
        });
      
        $(document).ready(function () {
            $('.datetimepickerAlt').datetimepicker().datetimepicker({ lang: 'es', closeOnDateSelect: true, timepicker: false, step: 30, format: 'd/m/Y' });
        });
     
        $(document).ready(function () {
            $('.datepicker').datetimepicker().datetimepicker({ lang: 'es', timepicker: false, format: 'd/m/Y', closeOnDateSelect: true, minDate: '0' });

        });


    function mostrarloader(Valor) {

        try {
            if (Valor == "1") {
                document.getElementById("ImgCarga").className = 'ver';
            }
            else {
                
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
                
            }

             } catch (e) {
                alert('Lamentamos comunicarle que ha ocurrido el siguiente problema:\n' + e.Message);
            }
        }

</script>

 <script type="text/javascript">
        function descarga(fname, hname, tbname) {
            var iframe = document.createElement("iframe");
            iframe.src = "../handler/fileExcel.ashx?name="+fname+"&page="+hname+"&obj="+tbname
            iframe.style.display = "none";
            document.body.appendChild(iframe);
        }
    </script>



</asp:Content>