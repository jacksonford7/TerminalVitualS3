<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="backoffice_fecha_salidabrbk.aspx.cs" Inherits="CSLSite.backoffice_fecha_salidabrbk" %>
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


  
   
  <link href="../shared/estilo/jquery.datetimepicker.css" rel="stylesheet" type="text/css" />
  <script src="../Scripts/jquery.datetimepicker.js" type="text/javascript"></script>

  <link href="../css/calendario_ajax.css" rel="stylesheet"/>

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

     <div class="mt-4">         
        <nav class="mt-4" aria-label="breadcrumb">
          <ol class="breadcrumb">
            <li class="breadcrumb-item" id="opcion_principal" runat="server"><a href="#">BackOffice</a></li>
            <li class="breadcrumb-item active" aria-current="page" id="sub_opcion" runat="server">REGISTRA FECHA SALIDA - CARGA BREAKBULK</li>
          </ol>
        </nav>
      </div>

 <div class="dashboard-container p-4" id="cuerpo" runat="server">

      <div class="form-title">
          DATOS DEL USUARIO
     </div>
     
     <div class="form-row">
          <div class="form-group col-md-6"> 
              <label for="inputAddress">RUC:<span style="color: #FF0000; font-weight: bold;"></span></label>
                <asp:TextBox ID="Txtcliente" runat="server" class="form-control"  size="50" 
                                placeholder=""  Font-Bold="true" disabled  Visible="false"></asp:TextBox>
                <asp:TextBox ID="Txtruc" runat="server" class="form-control"  size="25" 
                                placeholder=""  Font-Bold="true" disabled></asp:TextBox>
          </div>
          <div class="form-group col-md-6"> 
              <label for="inputAddress">EMPRESA:<span style="color: #FF0000; font-weight: bold;"></span></label>
                 <asp:TextBox ID="Txtempresa" runat="server" class="form-control"  size="30" 
                                placeholder=""  Font-Bold="true" disabled></asp:TextBox>
          </div>

     </div>
     <div class="form-title">
          DATOS DE LA CARGA
     </div>
      <asp:UpdatePanel ID="UPCARGA" runat="server"  UpdateMode="Conditional" ChildrenAsTriggers="true">
      <ContentTemplate>
     <div class="form-row">
      
             <div class="form-group col-md-4">
              <label for="inputZip">MRN</label>
              <asp:TextBox ID="TXTMRN" runat="server" class="form-control" MaxLength="16"  onkeypress="return soloLetras(event,'abcdefghijklmnopqrstuvwxyz0123456789-_')"  
                                 placeholder="mrn"></asp:TextBox>
            </div>
             <div class="form-group col-md-2">
              <label for="inputZip">MSN</label>
               <asp:TextBox ID="TXTMSN" runat="server" class="form-control"  MaxLength="4"  onkeypress="return soloLetras(event,'abcdefghijklmnopqrstuvwxyz0123456789-_')" 
                                placeholder="MSN"></asp:TextBox>
            </div> 
             <div class="form-group col-md-2">
              <label for="inputZip">MSN</label>
              <div class="d-flex">
                <asp:TextBox ID="TXTHSN" runat="server" class="form-control"  MaxLength="4"  onkeypress="return soloLetras(event,'abcdefghijklmnopqrstuvwxyz0123456789-_')" 
                                placeholder="HSN"></asp:TextBox>
                &nbsp;&nbsp;
                            <asp:Button ID="BtnBuscar" runat="server" class="btn btn-primary"  Text="BUSCAR"  OnClientClick="return mostrarloader('1')" OnClick="BtnBuscar_Click" />  
                   &nbsp;&nbsp;
                 <img alt="loading.." src="../lib/file-uploader/img/loading.gif" height="32px" width="32px"  id="ImgCarga" class="nover"   />
                          </div> 
            </div> 
           <br/>
              <div class="form-group col-md-12">
                <div class="alert alert-warning" id="banmsg" runat="server" clientidmode="Static"><b>Error!</b> >Debe ingresar el número de la carga MRN......</div>
            </div> 
     </div>


        <div class="form-row">
             <div class="form-group col-md-4">
              <label for="inputZip">FACTURA</label>
              <asp:TextBox ID="TxtFactura" runat="server" class="form-control" MaxLength="20"  onkeypress="return soloLetras(event,'abcdefghijklmnopqrstuvwxyz0123456789-_')"  
                                 placeholder="# factura" ></asp:TextBox>
            </div>
             <div class="form-group col-md-2">
              <label for="inputZip">FECHA SALIDA</label>
               
                  <asp:UpdatePanel ID="UPCAS" runat="server"  UpdateMode="Conditional"  >
                    <ContentTemplate>
                            
                        <asp:TextBox runat="server" ID="TxtFechaCas"  AutoPostBack="true" MaxLength="10" 
                                onkeypress="return soloLetras(event,'0123456789-')"    class="form-control"  
                          ></asp:TextBox>

                            <asp:CalendarExtender ID="CAGTFECHAFASA"  runat="server"
                                CssClass="red" Format="MM/dd/yyyy" TargetControlID="TxtFechaCas">
                            </asp:CalendarExtender>

                    </ContentTemplate>
                    </asp:UpdatePanel> 

            </div> 
        </div>


       </ContentTemplate>
        <Triggers>
        <asp:AsyncPostBackTrigger ControlID="BtnBuscar" />
    </Triggers>
    </asp:UpdatePanel>
   

  
      <asp:UpdatePanel ID="UPDATOSCLIENTE" runat="server"  UpdateMode="Conditional" ChildrenAsTriggers="true">
      <ContentTemplate>
     <div class="form-row">
        

          <div class="form-group col-md-12">
              
              <div class="d-flex">
                   
                         &nbsp;&nbsp;             
                    <asp:Button ID="BtnActualizar" runat="server"  class="btn btn-primary"  Text="GRABAR"  OnClientClick="return mostrarloader('1')" OnClick="BtnActualizar_Click" />      
            
              </div>
          </div>

    </div>
     </ContentTemplate>
    <Triggers>
    <asp:AsyncPostBackTrigger ControlID="BtnBuscar" />
</Triggers>
</asp:UpdatePanel>
      
	    
   
   

  </div>

  <!--common script for all pages-->
  <script type="text/javascript" src="../lib/common-scripts.js"></script>
  <script type="text/javascript" src="../lib/gritter/js/jquery.gritter.js"></script>
  <script type="text/javascript" src="../lib/gritter-conf.js"></script>
  <!--script for this page-->
 
  <script type="text/javascript" src="../lib/pages.js" ></script>
  

 <script type="text/javascript" src="../lib/advanced-form-components.js"></script>
 <script type="text/javascript" src="../lib/popup_script_cta.js" ></script>

<script type="text/javascript">

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



</asp:Content>