<%@ Page Title="" Language="C#" MasterPageFile="~/site.Master" AutoEventWireup="true" CodeBehind="subopciones.aspx.cs" Inherits="CSLSite.cuenta.subopciones" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>


<asp:Content ID="Content1" ContentPlaceHolderID="placehead" runat="server">
     <!-- Favicons -->
  <link href="../img/favicon2.png" rel="icon"/>
  <link href="../img/icono.png" rel="apple-touch-icon"/>
  <link href="../css/bootstrap.min.css" rel="stylesheet"/>
  <link href="../css/dashboard.css" rel="stylesheet"/>
  <link href="../css/icons.css" rel="stylesheet"/>
  <link href="../css/style.css" rel="stylesheet"/>
  <link href="https://fonts.googleapis.com/css2?family=Roboto:wght@300;400;500;700&display=swap" rel="stylesheet"/>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="placebody" runat="server">
       <asp:ToolkitScriptManager ID="tkscata" runat="server"></asp:ToolkitScriptManager>

<%-- <div class="container-fluid">
    <div class="row">
    <main role="main" class="col-md-9 ml-sm-auto col-lg-10 px-md-4">--%>

      <!-- Breadcrumb section -->
      <div class="mt-4">         
        <nav class="mt-4" aria-label="breadcrumb">
          <ol class="breadcrumb">
            <li class="breadcrumb-item" id="opcion_principal" runat="server"><a href="#">AISV</a></li>
            <li class="breadcrumb-item active" aria-current="page" id="sub_opcion" runat="server">Asistente para contenedores llenos</li>
          </ol>
        </nav>
      </div>

      <!-- White Container -->
      <div class="dashboard-container p-4" id="sub_menu" runat="server">
      <asp:UpdatePanel ID="MENU" runat="server"  UpdateMode="Conditional" >
      <ContentTemplate>
        <div class="row mt-4 ">
          <div class="col-md-12 d-flex left-content-between">
            <div class="d-flex align-items-center">
              <span class="mr-2">Filtrar por: </span>
              <button type="button" class="btn btn-primary mr-2 py-1 px-4" data-dismiss="modal">Categoria 1</button>
              <button type="button" class="btn btn-outline-primary mr-2 py-1 px-4" data-dismiss="modal">Categoria 2</button>
              <button type="button" class="btn btn-outline-primary py-1 px-4" data-dismiss="modal">Categoria 3</button>
            </div>
          </div>

          <div class="col-md-3">
            <div class="card p-3 m-2" >
              <img src="../img/aisv.png" width="50">
              <div class="card-body p-0 mt-3">
                <h5 class="card-title m-0">AISV</h5>
                <p class="card-text">Lorem ipsum dolor sit amet, consectetur.</p>
              </div>
            </div>
          </div>
          <div class="col-md-3">
            <div class="card p-3 m-2" >
              <img src="../img/solicitud.png" width="30" >
              <div class="card-body p-0 mt-3">
                <h5 class="card-title m-0">Solicitud de Requerimientos</h5>
                <p class="card-text">Lorem ipsum dolor sit.</p>
              </div>
            </div>
          </div>
          <div class="col-md-3">
            <div class="card p-3 m-2" >
              <img src="../img/recaudacion.png" width="40" >
              <div class="card-body p-0 mt-3">
                <h5 class="card-title m-0">Recaudación en línea</h5>
                <p class="card-text">Lorem ipsum dolor sit.</p>
              </div>
            </div>
          </div>
          <div class="col-md-3">
            <div class="card p-3 m-2" >
              <img src="../img/turno.png" width="51" height="38">
              <div class="card-body p-0 mt-3">
                <h5 class="card-title m-0">Turnos</h5>
                <p class="card-text">Lorem ipsum dolor sit amet, consectetur.</p>
              </div>
            </div>
          </div>
          <div class="col-md-3">
            <div class="card p-3 m-2" >
              <img src="../img/aisv.png" width="50" >
              <div class="card-body p-0 mt-3">
                <h5 class="card-title m-0">AISV</h5>
                <p class="card-text">Lorem ipsum dolor sit amet, consectetur.</p>
              </div>
            </div>
          </div>
          <div class="col-md-3">
            <div class="card p-3 m-2" >
              <img src="../img/solicitud.png" width="30" >
              <div class="card-body p-0 mt-3">
                <h5 class="card-title m-0">Solicitud de Requerimientos</h5>
                <p class="card-text">Lorem ipsum dolor sit.</p>
              </div>
            </div>
          </div>
          <div class="col-md-3">
            <div class="card p-3 m-2" >
              <img src="../img/recaudacion.png" width="40" >
              <div class="card-body p-0 mt-3">
                <h5 class="card-title m-0">Recaudación en línea</h5>
                <p class="card-text">Lorem ipsum dolor sit.</p>
              </div>
            </div>
          </div>
          <div class="col-md-3">
            <div class="card p-3 m-2" >
              <img src="../img/turno.png" width="51" height="38">
              <div class="card-body p-0 mt-3">
                <h5 class="card-title m-0">Turnos</h5>
                <p class="card-text">Lorem ipsum dolor sit amet, consectetur.</p>
              </div>
            </div>
          </div>
          <div class="col-md-3">
            <div class="card p-3 m-2" >
              <img src="../img/aisv.png" width="50" >
              <div class="card-body p-0 mt-3">
                <h5 class="card-title m-0">AISV</h5>
                <p class="card-text">Lorem ipsum dolor sit amet, consectetur.</p>
              </div>
            </div>
          </div>
          <div class="col-md-3">
            <div class="card p-3 m-2" >
              <img src="../img/solicitud.png" width="30" >
              <div class="card-body p-0 mt-3">
                <h5 class="card-title m-0">Solicitud de Requerimientos</h5>
                <p class="card-text">Lorem ipsum dolor sit.</p>
              </div>
            </div>
          </div>
          <div class="col-md-3">
            <div class="card p-3 m-2" >
              <img src="../img/recaudacion.png" width="40" >
              <div class="card-body p-0 mt-3">
                <h5 class="card-title m-0">Recaudación en línea</h5>
                <p class="card-text">Lorem ipsum dolor sit.</p>
              </div>
            </div>
          </div>
          <div class="col-md-3">
            <div class="card p-3 m-2" >
              <img src="../img/turno.png" width="51" height="38">
              <div class="card-body p-0 mt-3">
                <h5 class="card-title m-0">Turnos</h5>
                <p class="card-text">Lorem ipsum dolor sit amet, consectetur.</p>
              </div>
            </div>
          </div>
          
        </div>
     </ContentTemplate>     
        </asp:UpdatePanel>
      </div>
    <%--</main>
 </div>
      </div>--%>
     <script type="text/javascript" src="..lib/jquery/jquery-3.5.1.slim.min.js" integrity="sha384-DfXdz2htPH0lsSSs5nCTpuj/zy4C+OGpamoFVy38MVBnE+IbbVYUew+OrCXaRkfj" crossorigin="anonymous"></script>
    <script type="text/javascript">window.jQuery || document.write('<script src="../assets/js/vendor/jquery.slim.min.js"><\/script>')</script>
    <script type="text/javascript" src="../js/bootstrap.bundle.min.js"></script>
    <script type="text/javascript" src="../lib/jquery/feather.min.js"></script>
    <script type="text/javascript" src="../lib/jquery/Chart.min.js"></script>
    <script type="text/javascript" src="../js/dashboard.js"></script>
   
</asp:Content>
