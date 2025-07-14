<%@ Page Title="Reporte Check List Digital" Language="C#" MasterPageFile="~/site.Master" AutoEventWireup="true" CodeBehind="checklist_reporte.aspx.cs" Inherits="CSLSite.checklist_reporte" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="placehead" runat="server">

 <link href="../img/favicon2.png" rel="icon"/>
  <link href="../img/icono.png" rel="apple-touch-icon"/>
 <!-- Bootstrap core CSS -->
  <link href="../css/bootstrap.min.css" rel="stylesheet"/>
  <link href="../css/dashboard.css" rel="stylesheet"/>
  <link href="../css/icons.css" rel="stylesheet"/>
  <link href="../css/style.css" rel="stylesheet"/>

 
  <link href="../shared/estilo/jquery.datetimepicker.css" rel="stylesheet" type="text/css" />
  <script src="../Scripts/jquery.datetimepicker.js" type="text/javascript"></script>

     <link href="../lib/font-awesome/css/font-awesome.css" rel="stylesheet" />
   
  


 
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="placebody" runat="server" >

<asp:ToolkitScriptManager ID="tkscata" runat="server"></asp:ToolkitScriptManager>

      <div class="mt-4">         
        <nav class="mt-4" aria-label="breadcrumb">
          <ol class="breadcrumb">
            <li class="breadcrumb-item" id="opcion_principal" runat="server"><a href="#">Otros</a></li>
            <li class="breadcrumb-item active" aria-current="page" id="sub_opcion" runat="server">REPORTE CHECK LIST</li>
          </ol>
        </nav>
      </div>

     <asp:HiddenField ID="manualHide" runat="server" />


<div class="dashboard-container p-4" id="cuerpo" runat="server">
     <div class="form-title">
             CRITERIOS DE BÚSQUEDAS
     </div>
     <asp:UpdatePanel ID="UpdatePanel1" runat="server"  UpdateMode="Conditional" ChildrenAsTriggers="true">
       <ContentTemplate>
    <div class="form-row">  
        <div class="form-group col-md-4"> 
               <label for="inputEmail4">TIPO EQUIPO:</label>
             <asp:UpdatePanel ID="UpTipoEquipo" runat="server"  UpdateMode="Conditional" ChildrenAsTriggers="true">
                    <ContentTemplate>
                 <asp:DropDownList runat="server" ID="CboTipoEquipo"    AutoPostBack="true"  class="form-control" OnSelectedIndexChanged="CboTipoEquipo_SelectedIndexChanged" >
                        </asp:DropDownList>
                        </ContentTemplate> 
                        <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="CboTipoEquipo" />
                    </Triggers>
                    </asp:UpdatePanel> 

                     
          </div>
        <div class="form-group col-md-4"> 
               <label for="inputEmail4">EQUIPO:</label>
                       <asp:DropDownList runat="server"  ID="CboEquipo" class="form-control">
                                      
                                        </asp:DropDownList>
          </div>
         <div class="form-group col-md-"> 
              <label for="inputEmail4">TURNO:</label>
                       <asp:DropDownList runat="server" ID="CboTurno" class="form-control">
                                      
                                          
                                        </asp:DropDownList>
          </div>
    </div>
      </ContentTemplate>
            <Triggers>
            <asp:AsyncPostBackTrigger ControlID="CboTipoEquipo" />
        </Triggers>
        </asp:UpdatePanel>
     <div class="form-row"> 
        <div class="form-group col-md-6"> 
                 <label for="inputEmail4">FECHA DESDE:</label>
                <asp:TextBox ID="TxtFechaDesde" runat="server"  class="datetimepicker form-control"  MaxLength="10"  placeholder="dd/MM/yyyy" onkeypress="return soloLetras(event,'1234567890/:')"></asp:TextBox>
        </div>
        <div class="form-group col-md-6">  
                <label for="inputEmail4">FECHA HASTA:</label>
                <asp:TextBox ID="TxtFechaHasta" runat="server"  class="datetimepicker form-control" MaxLength="10"  placeholder="dd/MM/yyyy" onkeypress="return soloLetras(event,'1234567890/:')"></asp:TextBox>
                          
        </div>
     </div> 
    <asp:UpdatePanel ID="UPCARGA" runat="server"  UpdateMode="Conditional">
     <ContentTemplate>

     <div class="row">
            <div class="col-md-12 d-flex justify-content-center">
                 <%-- <asp:Button ID="BtnBuscar" runat="server" class="btn btn-primary"   Text="GENERAR REPORTE"  OnClientClick="return mostrarloader('1')" OnClick="BtnBuscar_Click" />     --%>                        
                  <img alt="loading.." src="../lib/file-uploader/img/loading.gif" width="32px" height="32px"   id="ImgCarga" class="nover" />
            </div>    
     </div>    
           <br/>
      <div class="row">
        <div class="col-md-12 d-flex justify-content-center">
              <div class="alert alert-warning" id="banmsg" runat="server" clientidmode="Static"><b>Error!</b> >......</div>
        </div>
      </div>

    </ContentTemplate>
  <%--  <Triggers>
    <asp:AsyncPostBackTrigger ControlID="BtnBuscar" />
</Triggers>--%>
</asp:UpdatePanel>

 <div class="row">
      <div class="col-md-12 d-flex justify-content-center">
       <asp:Button ID="BtnBuscar" runat="server" class="btn btn-primary"   Text="GENERAR REPORTE"    OnClick="BtnBuscar_Click" />                                   
      </div>    
 </div>      
     
			
   </div>

   


    <script type="text/javascript" src="../lib/common-scripts.js"></script>
 
  <script type="text/javascript" src="../lib/pages.js" ></script>

 <script type="text/javascript" src="../lib/advanced-form-components.js"></script>





<script type="text/javascript">

      function confirmacion()
    {
        var mensaje;
        var opcion = confirm("Está seguro que desea generar el reporte a Excel. ?");
        if (opcion == true)
        {
            mostrarloader();
            
            return true;
        } else
        {
            ocultarloader();
	         return false;
        }

       
    }

    function mostrarloader() {

        try {
            
                document.getElementById("ImgCarga").className = 'ver';
            
            
            } catch (e) {
                alert('Lamentamos comunicarle que ha ocurrido el siguiente problema:\n' + e.Message);
            }
        }


    function ocultarloader() {
        try {

                document.getElementById("ImgCarga").className = 'nover';
           
            
             } catch (e) {
                alert('Lamentamos comunicarle que ha ocurrido el siguiente problema:\n' + e.Message);
            }
        }


  </script>

      <script type="text/javascript">
            $(document).ready(function () {
                 $('.datetimepicker').datetimepicker().datetimepicker({ lang: 'es', timepicker: false, step: 30, format: 'm/d/Y' });
              });    
      </script>

   

</asp:Content>