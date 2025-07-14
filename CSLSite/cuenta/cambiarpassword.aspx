<%@ Page Title="Cambio de Contraseña" Language="C#" MasterPageFile="~/site.Master"
    AutoEventWireup="true" CodeBehind="cambiarpassword.aspx.cs" Inherits="CSLSite.cuenta.cambiarpassword" %>

<%@ MasterType VirtualPath="~/site.Master" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="placehead" runat="server">

   <link href="../img/favicon2.png" rel="icon"/>
   <link href="../img/icono.png" rel="apple-touch-icon"/>
   <!-- Bootstrap core CSS -->
  <link href="../css/bootstrap.min.css" rel="stylesheet"/>
  <link href="../css/dashboard.css" rel="stylesheet"/>
  <link href="../css/icons.css" rel="stylesheet"/>
  <link href="../css/style.css" rel="stylesheet"/>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="placebody" runat="server">
    <asp:ToolkitScriptManager ID="tkcal" EnableScriptGlobalization="true" runat="server">
    </asp:ToolkitScriptManager>
    <input id="zonaid" type="hidden" value="501" />
  
     <div class="mt-4">         
        <nav class="mt-4" aria-label="breadcrumb">
          <ol class="breadcrumb">
            <li class="breadcrumb-item" id="opcion_principal" runat="server"><a href="#">CLAVE</a></li>
            <li class="breadcrumb-item active" aria-current="page" id="sub_opcion" runat="server">CAMBIO DE CLAVE</li>
          </ol>
        </nav>
      </div>
<div class="dashboard-container p-4" id="cuerpo" runat="server">
     <div class="form-title">
           DATOS DE CONTASEÑA
     </div>
    <asp:UpdatePanel ID="updConsultaUsuarios" runat="server">
        <ContentTemplate>
           <div class="form-row">
                <div class="form-group col-md-6">
                    <label for="inputAddress">1. Contraseña anterior:<span style="color: #FF0000; font-weight: bold;">*</span></label>
                    <asp:TextBox ID="txtPasswordAnterior" runat="server" class="form-control"  onpaste="return false;"
                                    MaxLength="30" placeholder="CONTRASEÑA ANTERIOR"  TextMode="Password"
                                    onblur="cadenareqerida(this,1,30,'valPas');"></asp:TextBox>
                    <span class="validacion" id="valPas"></span>
               </div>
                <div class="form-group col-md-6">
                    <label for="inputAddress">2. Nueva contraseña:<span style="color: #FF0000; font-weight: bold;">*</span></label>
                      <asp:TextBox ID="txtNuevoPassword" runat="server" class="form-control" onpaste="return false;"
                                    MaxLength="30" placeholder="NUEVA CONTRASEÑA" TextMode="Password"
                                    onblur="cadenareqerida(this,1,30,'valNuePas');"></asp:TextBox>
                    <span class="validacion" id="valNuePas"></span>
               </div>
                 <div class="form-group col-md-6">
                    <label for="inputAddress">3. Confirmar contraseña:<span style="color: #FF0000; font-weight: bold;">*</span></label>
                      <asp:TextBox ID="txtPasswordConfirmado" runat="server"  class="form-control" onpaste="return false;"
                                    placeholder="CONFIRMAR CONTRASEÑA" MaxLength="30"  TextMode="Password"
                                    onblur="cadenareqerida(this,1,30,'valConfPas');"></asp:TextBox>
                    <span class="validacion" id="valConfPas"></span>
               </div>
           </div>
          
             <br/>
            <div class="row">
                <div class="col-md-12 d-flex justify-content-center">
                      <div class="alert alert-danger" id="alerta" runat="server" clientidmode="Static"><b></b>.</div>
                 </div>
             </div>
            <div class="row">
                <div class="col-md-12 d-flex justify-content-center">
                      <div class="alert alert-danger" id="error" runat="server" clientidmode="Static" visible="false"><b></b>.</div>
                 </div>
             </div>
             <br/>
             <div class="row">
                <div class="col-md-12 d-flex justify-content-center">
                         <asp:UpdateProgress AssociatedUpdatePanelID="updConsultaUsuarios" ID="updateProgress"
                        runat="server">
                        <ProgressTemplate>
                            <div id="progressBackgroundFilter">
                            </div>
                            <div id="processMessage">
                                Estamos procesando la tarea que solicitó, por favor espere...
                                <img alt="Loading" src="../shared/imgs/loader.gif" style="margin: 0 auto;" />
                            </div>
                        </ProgressTemplate>
                    </asp:UpdateProgress>
                </div>
              </div>
             <br/>

             <div class="row">
                <div class="col-md-12 d-flex justify-content-center">
                       <asp:Button ID="btbuscar" Text="Cambiar Contraseña" runat="server" OnClick="btbuscar_Click" class="btn btn-primary" />
                </div>
            </div>

            <div class="seccion" id="PERSONAL">
              
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
  </div>
     <script type="text/javascript" src="../lib/common-scripts.js"></script>
 
   <script type="text/javascript" src="../lib/pages.js" ></script>

   <script type="text/javascript" src="../lib/advanced-form-components.js"></script>


  <%--  <script src="../Scripts/pages.js" type="text/javascript"></script>
    <script src="../Scripts/pages_mod.js" type="text/javascript"></script>
    <script src="../Scripts/jquery-1.7.1.min.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.tablesorter.min.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.tablesorter.pager.js" type="text/javascript"></script>
    <script src="../Scripts/chosen.jquery.js" type="text/javascript"></script>--%>

    <script type="text/javascript">
        var ced_count = 0;
        var jAisv = {};
        $(window).load(function () {
            //objeto a transportar.
            $(document).ready(function () {
                //inicia los fecha-hora

                //colapsar y expandir
                $("div.colapser").toggle(function () { $(this).removeClass("colapsa").addClass("expande"); $(this).next().hide(); }
                                  , function () { $(this).removeClass("expande").addClass("colapsa"); $(this).next().show(); });
                //poner valor en campo

            });
        });






        //Esta funcion va a validar que cuando presionen booking debe poner los 3 parametros
        function validateBook(objeto) {

        }

        //Imprimir.......................
        function imprimir() {

            //Si es contenedor validar cedula

        }

        //esta futura funcion va a preparar el objeto a transportar.
        function prepareObject() {






        }
        function popupCallback(data, control) {
            this.document.getElementById(control).value = data;
        }

    </script>
</asp:Content>
