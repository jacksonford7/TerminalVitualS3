<%@ Page Title="" Language="C#" MasterPageFile="~/site.Master" AutoEventWireup="true" CodeBehind="backoffice_rditmanual.aspx.cs" Inherits="CSLSite.backoffice.backoffice_rditmanual" %>
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
     
    </div>

     <div class="mt-4">
        <nav class="mt-4" aria-label="breadcrumb">
          <ol class="breadcrumb">
            <li class="breadcrumb-item" id="opcion_principal" runat="server"><a href="#">BackOffice</a></li>
            <li class="breadcrumb-item active" aria-current="page" id="sub_opcion" runat="server">REGISTRO DE RIDT MANUAL - PARA DESADUANAMIENTO DIRECTO</li>
          </ol>
        </nav>
    </div>
<div class="dashboard-container p-4" id="cuerpo" runat="server">
      <div class="form-title">
            DATOS DEL USUARIO
     </div>
     
    <div class="form-row">
            <div class="form-group col-md-6"> 
                <label for="inputAddress">ESTIMADO CLIENTE:<span style="color: #FF0000; font-weight: bold;"></span></label>
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
    <div class="form-title">
            DATOS DE LA CARGA
    </div>
    <asp:UpdatePanel ID="UPCARGA" runat="server"  UpdateMode="Conditional" ChildrenAsTriggers="true">
    <ContentTemplate>
    <div class="form-row">
        <div class="form-group col-md-4">
              <label for="inputZip">MRN<span style="color: #FF0000; font-weight: bold;">*</span></label>
                <asp:TextBox ID="TXTMRN" runat="server" class="form-control" MaxLength="16"  onkeypress="return soloLetras(event,'abcdefghijklmnopqrstuvwxyz0123456789-_')"  
                                 placeholder="mrn"></asp:TextBox>
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
              <asp:Button ID="BtnBuscar" runat="server" class="btn btn-primary"  Text="REGISTRAR RIDT"  OnClientClick="return mostrarloader('1')" OnClick="BtnAsumir_Click" /> 
              <img alt="loading.." src="../lib/file-uploader/img/loading.gif" height="32px" width="32px"  id="ImgCarga" class="nover"   />
              </div>
         </div>
        <div class="form-group col-md-2">
         </div>
    </div>
     <br/>
    <div class="row">
        <div class="col-md-12 d-flex justify-content-center">
             <div class="alert alert-warning" id="banmsg" runat="server" clientidmode="Static"><b>Error!</b> >......</div>
        </div>
    </div>
         </ContentTemplate>
        <Triggers>
       <asp:AsyncPostBackTrigger ControlID="BtnBuscar" />
        </Triggers>
        </asp:UpdatePanel>
        
      <h4 class="mb">DATOS DEL AGENTE/IMPORTADOR</h4>
     <asp:UpdatePanel ID="UPDATOSCLIENTE" runat="server"  UpdateMode="Conditional" ChildrenAsTriggers="true">
                        <ContentTemplate>
                        
     <div class="form-row">
            <div class="form-group col-md-6"> 
              <label for="inputAddress">ID AGENTE DE ADUANA:<span style="color: #FF0000; font-weight: bold;">*</span></label>
                <asp:UpdatePanel ID="UPAGENTE" runat="server"  UpdateMode="Conditional" ChildrenAsTriggers="true">
                                    <ContentTemplate>
                                     <asp:TextBox ID="TxtIdAgente" runat="server" class="form-control"    
                                        placeholder=""  MaxLength="13" Font-Bold="false" onkeypress="return soloLetras(event,'abcdefghijklmnñopqrstuvwxyz0123456789_')" 
                                        ClientIDMode="Static" ></asp:TextBox>
                                    </ContentTemplate> 
                                    <Triggers>
                                 <asp:AsyncPostBackTrigger ControlID="TxtIdAgente" />
                                </Triggers>
                                </asp:UpdatePanel> 
           </div>
           <div class="form-group col-md-6"> 
               <label for="inputAddress">&nbsp;<span style="color: #FF0000; font-weight: bold;"></span></label>
                <asp:TextBox ID="TXTAGENCIA" runat="server" class="form-control"    
                                placeholder=""  Font-Bold="false" disabled  ClientIDMode="Static"></asp:TextBox>
          </div>
         <div class="form-group col-md-6"> 
              <label for="inputAddress">ID IMPORTADOR/RUC:<span style="color: #FF0000; font-weight: bold;">*</span></label> 
                 <asp:TextBox ID="TxtIdImportador" runat="server" class="form-control"    
                                placeholder="" MaxLength="13" Font-Bold="false" onkeypress="return soloLetras(event,'abcdefghijklmnñopqrstuvwxyz0123456789_')"
                                    ClientIDMode="Static"></asp:TextBox>
         </div>
          <div class="form-group col-md-6"> 
               <label for="inputAddress">&nbsp;<span style="color: #FF0000; font-weight: bold;"></span></label> 
                <asp:TextBox ID="TxtDescImportador" runat="server" class="form-control"    
                                placeholder=""  Font-Bold="false" disabled  ClientIDMode="Static"></asp:TextBox>
          </div>

           <div class="form-group col-md-6"> 
              <label for="inputAddress"># DECLARACION:<span style="color: #FF0000; font-weight: bold;">*</span></label>  
                <asp:TextBox ID="TxtNumeroDeclaracion" runat="server" class="form-control"    
                                placeholder=""  MaxLength="30" Font-Bold="false" onkeypress="return soloLetras(event,'abcdefghijklmnñopqrstuvwxyz0123456789_')"
                                    ClientIDMode="Static"></asp:TextBox>
           </div>
            <div class="form-group col-md-6"> 
              <label for="inputAddress">COMENTARIO:<span style="color: #FF0000; font-weight: bold;"></span></label> 
                <asp:TextBox ID="Txtcomentario" runat="server" class="form-control"    
                                placeholder=""  MaxLength="100" Font-Bold="false" onkeypress="return soloLetras(event,'abcdefghijklmnñopqrstuvwxyz0123456789_')"
                                    ClientIDMode="Static"></asp:TextBox>
            </div> 
     </div>
    </ContentTemplate>
    <Triggers>
    <asp:AsyncPostBackTrigger ControlID="BtnBuscar" />
    </Triggers>
    </asp:UpdatePanel>

      

</div>
 

   <script type="text/javascript" src="lib/jquery/jquery.min.js"></script>
   <script type="text/javascript" src="lib/bootstrap/js/bootstrap.min.js"></script>
  <script type="text/javascript" src='lib/autocompletar/bootstrap3-typeahead.min.js'></script>
 

  <script type="text/javascript" src="../lib/pages.js"></script>


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