<%@ Page Title="" Language="C#" MasterPageFile="~/site.Master" AutoEventWireup="true" CodeBehind="backoffice_registra_dae.aspx.cs" Inherits="CSLSite.backoffice.backoffice_registra_dae" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="placehead" runat="server">

   <link href="../img/favicon2.png" rel="icon" />
    <link href="../img/icono.png" rel="apple-touch-icon" />
    <!-- Bootstrap core CSS -->
    <link href="../css/bootstrap.min.css" rel="stylesheet" />
    <link href="../css/dashboard.css" rel="stylesheet" />
    <link href="../css/icons.css" rel="stylesheet" />
    <link href="../css/style.css" rel="stylesheet" />

    <!--external css-->
    <link href="../lib/font-awesome/css/font-awesome.css" rel="stylesheet" />


</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="placebody" runat="server" >
  
  <asp:ToolkitScriptManager ID="tkscata" runat="server"></asp:ToolkitScriptManager>
 
    <div id="div_BrowserWindowName" style="visibility:hidden;">
    <asp:HiddenField ID="hf_BrowserWindowName" runat="server" />
      <input id="ruta_completa" type="hidden" value="" runat="server" clientidmode="Static" />
    </div>

     <div class="mt-4">
        <nav class="mt-4" aria-label="breadcrumb">
          <ol class="breadcrumb">
            <li class="breadcrumb-item" id="opcion_principal" runat="server"><a href="#">BackOffice</a></li>
            <li class="breadcrumb-item active" aria-current="page" id="sub_opcion" runat="server">REGISTRO DE DAE</li>
          </ol>
        </nav>
    </div>
<div class="dashboard-container p-4" id="cuerpo" runat="server">
      <div class="form-title">
            DATOS DEL USUARIO
     </div>
     
    <div class="form-row">
            <div class="form-group col-md-6"> 
                <label for="inputAddress">ESTIMADO USUARIO:<span style="color: #FF0000; font-weight: bold;"></span></label>
                   <asp:TextBox ID="Txtcliente" runat="server" class="form-control" 
                                placeholder=""  Font-Bold="true" disabled ></asp:TextBox>
            </div>
            <div class="form-group col-md-2">
                <label for="inputAddress">RUC:<span style="color: #FF0000; font-weight: bold;"></span></label>
                
						        <asp:TextBox ID="Txtruc" runat="server" class="form-control" 
                                placeholder=""  Font-Bold="true" disabled></asp:TextBox>
            </div>
            <div class="form-group col-md-4">
                <label for="inputAddress">EMPRESA:<span style="color: #FF0000; font-weight: bold;"></span></label>
                  <asp:TextBox ID="Txtempresa" runat="server" class="form-control"  
                                placeholder=""  Font-Bold="true" disabled></asp:TextBox>
            </div>
    </div>
   
        
     <h4 class="mb">DATOS DE LA DAE</h4>
     <asp:UpdatePanel ID="UPDATOSCLIENTE" runat="server"  UpdateMode="Conditional" ChildrenAsTriggers="true">
                        <ContentTemplate>
                        
     <div class="form-row">
            <div class="form-group col-md-6"> 
              <label for="inputAddress">DAE:<span style="color: #FF0000; font-weight: bold;">*</span></label>
                <asp:TextBox ID="TxtDae" runat="server" class="form-control"    
                placeholder=""  MaxLength="40" Font-Bold="false" onkeypress="return soloLetras(event,'0123456789')" 
                ClientIDMode="Static" ></asp:TextBox>                     
           </div>
           <div class="form-group col-md-6"> 
               <label for="inputAddress">RUC:<span style="color: #FF0000; font-weight: bold;">*</span></label>
                <asp:TextBox ID="TxtRucExportador" runat="server" class="form-control"   onkeypress="return soloLetras(event,'0123456789')" 
                                placeholder=""  Font-Bold="false" MaxLength="13"  ClientIDMode="Static"></asp:TextBox>
          </div>
         <div class="form-group col-md-6"> 
              <label for="inputAddress">NOMBRE EXPORTADOR:<span style="color: #FF0000; font-weight: bold;">*</span></label> 
                 <asp:TextBox ID="TxtDescExportador" runat="server" class="form-control"    
                                placeholder="" MaxLength="200" Font-Bold="false" onkeypress="return soloLetras(event,'abcdefghijklmnñopqrstuvwxyz0123456789_')"
                                    ClientIDMode="Static"></asp:TextBox>
         </div>
          <div class="form-group col-md-6"> 

               <label for="inputAddress">CANTIDAD<span style="color: #FF0000; font-weight: bold;">*</span></label> 
                <asp:TextBox ID="TxtCantidad" runat="server" class="form-control"    onkeypress="return soloLetras(event,'0123456789')"  
                                placeholder=""  Font-Bold="false" MaxLength="5"  ClientIDMode="Static"></asp:TextBox>
          </div>

           <div class="form-group col-md-6"> 
              <label for="inputAddress">TIPO:<span style="color: #FF0000; font-weight: bold;">*</span></label>  
                <asp:DropDownList ClientIDMode="Static" ID="CboTipo" class="form-control" runat="server" >
                            <asp:ListItem Value="CC" Selected="True">CARGA CONTENERIZADA</asp:ListItem>
                            <asp:ListItem Value="CS">CARGA SUELTA</asp:ListItem>
                 </asp:DropDownList>
           </div>
          <div class="form-group col-md-6" > 
                          <label for="inputEmail4">ARCHIVO PDF DE SOPORTE:</label>
                          <div class="d-flex">
                          <asp:TextBox ID="TxtRuta1" runat="server"   class="form-control" ClientIDMode="Static" disabled ></asp:TextBox>
                                 <a  class="btn btn-outline-primary mr-4" runat="server" id="BtnArchivos" 
                                     target ="popup"  onclick="subirpdf();"   >
                                <span class='fa fa-search' style='font-size:24px' ></span></a>
           </div>

           
        
       
          

     </div>
    </ContentTemplate>
    <Triggers>
    <asp:AsyncPostBackTrigger ControlID="BtnBuscar" />
    </Triggers>
    </asp:UpdatePanel>

      

</div>
  <br/>
     <asp:UpdatePanel ID="UPMensaje" runat="server"  UpdateMode="Conditional" >
                        <ContentTemplate>
      <div class="row">
             <div class="col-md-12 d-flex justify-content-center">
                 <div class="alert alert-warning" id="banmsg" runat="server" clientidmode="Static"><b>Error!</b> >......</div>
            </div>
         </div>
  </ContentTemplate>

    </asp:UpdatePanel>
     <br/>

 <div class="row">
               <div class="col-md-12 d-flex justify-content-center">
                      <asp:Button ID="BtnBuscar" runat="server" class="btn btn-primary"  Text="REGISTRAR RIDT"  OnClientClick="return mostrarloader('1')" OnClick="BtnAsumir_Click" /> 
                      
                </div>
         </div>

   <script type="text/javascript" src="lib/jquery/jquery.min.js"></script>
   <script type="text/javascript" src="lib/bootstrap/js/bootstrap.min.js"></script>
  <script type="text/javascript" src='lib/autocompletar/bootstrap3-typeahead.min.js'></script>
 

  <script type="text/javascript" src="../lib/pages.js"></script>


<script type="text/javascript">

  
</script>


      <script type="text/javascript">
            function subirpdf()
            {
               try {
                    var w = window.open('../backoffice/backoffice_archivo_dae.aspx', 'Archivos', 'width=850,height=400');
                     w.focus();     
               }
               catch (e) {
                    alertify.alert('ERROR',e.Message  ).set('label', 'Reportar');
                }
            }

          function popupCallback_Archivo(lookup_archivo)
          {
     
               if (lookup_archivo.sel_Ruta != null )
               {
                    this.document.getElementById('<%= TxtRuta1.ClientID %>').value = lookup_archivo.sel_Nombre_Archivo1;
                    this.document.getElementById("ruta_completa").value = lookup_archivo.sel_Ruta; 
                }
          
          } 

     </script>


</asp:Content>