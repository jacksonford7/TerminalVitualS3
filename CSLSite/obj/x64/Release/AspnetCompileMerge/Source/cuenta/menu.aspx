<%@ Page Title="" Language="C#" MasterPageFile="~/site.Master" AutoEventWireup="true" CodeBehind="menu.aspx.cs" Inherits="CSLSite.cuenta.menu" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="placehead" runat="server">
     <!-- Favicons -->
 <%-- <link rel="canonical" href="https://getbootstrap.com/docs/4.5/examples/dashboard/">--%>
  <link href="../img/favicon2.png" rel="icon"/>
  <link href="../img/icono.png" rel="apple-touch-icon"/>
  <link href="../css/bootstrap.min.css" rel="stylesheet"/>
  <link href="../css/dashboard.css" rel="stylesheet"/>
  <link href="../css/icons.css" rel="stylesheet"/>
  <link href="../css/style.css" rel="stylesheet"/>
  <link href="https://fonts.googleapis.com/css2?family=Roboto:wght@300;400;500;700&display=swap" rel="stylesheet"/>


       <style type="text/css">
        body
        {
            /*font-family: Arial;
            font-size: 10pt;*/
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
            width: 726px;
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
            line-height: 25px;
            text-align: center;
            /*font-weight: bold;*/
            margin-bottom: 5px;
        }
    </style>
 

<script type="text/javascript">
$(document).ready(function(){
  $('[data-toggle="tooltip"]').tooltip();   
});
</script>


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="placebody" runat="server">
        <asp:ToolkitScriptManager ID="tkscata" runat="server"></asp:ToolkitScriptManager>
    <br/>
       <asp:HiddenField ID="manualHide" runat="server" />
<div class="dashboard-container p-4">
      <%--<div class="form-row">  
            <div class="form-group col-md-4">
                 <label for="inputEmail4">FECHA SALIDA<span style="color: #FF0000; font-weight: bold;">*</span></label>
                <asp:TextBox runat="server" ID="TxtTipo" class="form-control" disabled></asp:TextBox>
            </div>
     </div>--%>

              <div class="mb-5">
              <div id="carouselExampleCaptions" class="carousel slide" data-ride="carousel">
                <ol class="carousel-indicators">
                  <li data-target="#carouselExampleCaptions" data-slide-to="0" class="active"></li>
                  <li data-target="#carouselExampleCaptions" data-slide-to="1"></li>
                  <li data-target="#carouselExampleCaptions" data-slide-to="2"></li>
                </ol>
                <div class="carousel-inner">
                  <div class="carousel-item active">
                    <img src="../img/1.png" class="d-block w-100" alt="..."/>
                    <div class="carousel-caption d-none d-md-block">
                      <%--<h5>First slide label</h5>
                      <p>Nulla vitae elit libero, a pharetra augue mollis interdum.</p>--%>
                    </div>
                  </div>
                  <div class="carousel-item">
                    <img src="../img/2.png" class="d-block w-100" alt="..."/>
                    <div class="carousel-caption d-none d-md-block">
                     <%-- <h5>Second slide label</h5>
                      <p>Lorem ipsum dolor sit amet, consectetur adipiscing elit.</p>--%>
                    </div>
                  </div>
                  <div class="carousel-item">
                    <img src="../img/3.png" class="d-block w-100" alt="..."/>
                    <div class="carousel-caption d-none d-md-block">
                    <%--  <h5>Third slide label</h5>
                      <p>Praesent commodo cursus magna, vel scelerisque nisl consectetur.</p>--%>
                    </div>
                  </div>
                </div>
                <a class="carousel-control-prev" href="#carouselExampleCaptions" role="button" data-slide="prev">
                  <span class="carousel-control-prev-icon" aria-hidden="true"></span>
                  <span class="sr-only">Previous</span>
                </a>
                <a class="carousel-control-next" href="#carouselExampleCaptions" role="button" data-slide="next">
                  <span class="carousel-control-next-icon" aria-hidden="true"></span>
                  <span class="sr-only">Next</span>
                </a>
              </div>
            </div>

             <div id="error" runat="server" class="alert alert-danger" visible="false">
  </div>

     <!-- Modal -->
   <asp:ModalPopupExtender  
      ID="mpedit" runat="server" 
      PopupControlID="myModal2"
      BackgroundCssClass="modalBackground"  
      TargetControlID="manualHide"
       CancelControlID="btclose"  
      >
    </asp:ModalPopupExtender>
      <asp:Panel ID="myModal2" runat="server" CssClass="modalPopup" align="center" Style="display: none" >
        
        <div class="body">
             <asp:UpdatePanel ID="msgload" runat="server" UpdateMode="Conditional"  ChildrenAsTriggers="true">
               <ContentTemplate>
            
 <table width="724" border="0" cellpadding="0" cellspacing="0">
  <!--DWLayoutTable-->
  <tr>
    <td height="36" colspan="3" valign="middle" bgcolor="#FF3720"><span style="color:#f2f2f2; font-weight:bold">&nbsp; Alerta de Contenedores Bloqueados -  PAN</span></td>
  </tr>
  <tr>
    <td width="17" rowspan="4" valign="top"><!--DWLayoutEmptyCell-->&nbsp;</td>
    <td width="692" height="76" valign="top"><div align="justify"><br/>Estimado cliente, ud tiene unidades bloqueadas por la Unidad de Policía Antinarcóticos.<br/><br/>
            Para revisar las mismas debe dirigirse a la siguiente opción del de Terminal Virtual:<br/><br/>

  </div></td>
  <td width="15" rowspan="4" valign="top"><!--DWLayoutEmptyCell-->&nbsp;</td>
  </tr>
  <tr>
    <td height="150" valign="top"><div align="left"><a href="../aisv/pan_consulta.aspx"><strong>Ver Detalle </strong></a><br/>
       
    </div></td>
  </tr>
  
  <tr>
    <td height="104" valign="top">

	    <table width="100%" border="0" cellpadding="0" cellspacing="0">
	    
            </tr>  <td><br/></td></tr>
	 
	      <tr>
	        <td height="24" colspan="4" valign="top">
                 <div class="d-flex justify-content-end mt-4">
                <asp:Button ID="BtnProcesar" runat="server" class="btn btn-primary"  
                                                Text="Acepto El Servicio"  OnClientClick="this.disabled=true"
                                            UseSubmitBehavior="false"  Visible="false" />    &nbsp;&nbsp;                        
              <input  type="button" id="btclose" class="btn btn-primary" value="Salir"  />
                     </div>
                     </td>
          </tr>
	      <tr>
	        <td height="1"></td>
	        <td></td>
	        <td width="31"></td>
	        <td></td>
          </tr>
            </table>
     </div></td>
  </tr>
</table>

                     </ContentTemplate>
                   <Triggers>
                          <asp:AsyncPostBackTrigger ControlID="BtnProcesar" />
                        </Triggers>
            </asp:UpdatePanel>
        </div>
    </asp:Panel>
   <!-- Modal -->


    <script src="https://code.jquery.com/jquery-3.5.1.slim.min.js" integrity="sha384-DfXdz2htPH0lsSSs5nCTpuj/zy4C+OGpamoFVy38MVBnE+IbbVYUew+OrCXaRkfj" crossorigin="anonymous"></script>
    <script>window.jQuery || document.write('<script src="../assets/js/vendor/jquery.slim.min.js"><\/script>')</script>
    <script src="../js/bootstrap.bundle.min.js"></script>
  <%--  <script src="https://cdnjs.cloudflare.com/ajax/libs/feather-icons/4.9.0/feather.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/Chart.js/2.7.3/Chart.min.js"></script>--%>
    <script src="../js/dashboard.js"></script>
</asp:Content>
