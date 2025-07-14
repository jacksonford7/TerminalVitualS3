<%@ Page Title="Notificaciones Consulta" Language="C#" MasterPageFile="~/site.Master" AutoEventWireup="true" CodeBehind="notificacionesConsulta.aspx.cs" Inherits="CSLSite.notificacionesConsulta" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="placehead" runat="server">

 <link href="../img/favicon2.png" rel="icon"/>
  <link href="../img/icono.png" rel="apple-touch-icon"/>
  <!-- Bootstrap core CSS -->
  <link href="../css/bootstrap.min.css" rel="stylesheet"/>
  <link href="../css/dashboard.css" rel="stylesheet"/>
  <link href="../css/icons.css" rel="stylesheet"/>
  <link href="../css/style.css" rel="stylesheet"/>
  <link href="../css/stc_final.css" rel="stylesheet"/>

  <!--external css-->
  <link href="../lib/font-awesome/css/font-awesome.css" rel="stylesheet" />
  <link href="../shared/estilo/jquery.datetimepicker.css" rel="stylesheet" type="text/css" />
  <script src="../Scripts/jquery.datetimepicker.js" type="text/javascript"></script>


      <script src="../js/new/popper.min.js"></script>
      <script src="../js/new/bootstrap.min.js"></script>
        <style>
            .bs-example{
                margin: 20px;
            }
            .accordion .fa{
                margin-right: 0.5rem;
            }
            .auto-style5 {
                height: 46px;
            }
        </style>
        <script>

            
            $(document).ready(function () {
                // Add minus icon for collapse element which is open by default
                $(".collapse.show").each(function () {
                    $(this).prev(".card-header").find(".fa").addClass("fa-chevron-down").removeClass("fa-chevron-right");
                });

                // Toggle plus minus icon on show hide of collapse element
                $(".collapse").on('show.bs.collapse', function () {
                    $(this).prev(".card-header").find(".fa").removeClass("fa-chevron-right").addClass("fa-chevron-down");
                }).on('hide.bs.collapse', function () {
                    $(this).prev(".card-header").find(".fa").removeClass("fa-chevron-down").addClass("fa-chevron-right");
                });
            });
        </script>

        <script type="text/javascript">

            function BindFunctions() {

                 $(document).ready(function () {
                // Add minus icon for collapse element which is open by default
                $(".collapse.show").each(function () {
                    $(this).prev(".card-header").find(".fa").addClass("fa-chevron-down").removeClass("fa-chevron-right");
                });

                // Toggle plus minus icon on show hide of collapse element
                $(".collapse").on('show.bs.collapse', function () {
                    $(this).prev(".card-header").find(".fa").removeClass("fa-chevron-right").addClass("fa-chevron-down");
                }).on('hide.bs.collapse', function () {
                    $(this).prev(".card-header").find(".fa").removeClass("fa-chevron-down").addClass("fa-chevron-right");
                });
            });
            }

    
        </script>


 </asp:Content>

  <asp:Content ID="Content2" ContentPlaceHolderID="placebody" runat="server" >
         <asp:ToolkitScriptManager ID="tkscata" runat="server"></asp:ToolkitScriptManager>

       <input id="Idnot" type="hidden" value="" runat="server" clientidmode="Static" />

      <div class="mt-4">         
        <nav class="mt-4" aria-label="breadcrumb">
          <ol class="breadcrumb">
            <li class="breadcrumb-item" id="opcion_principal" runat="server"><a href="#">CGSApp</a></li>
            <li class="breadcrumb-item active" aria-current="page" id="sub_opcion" runat="server">CONSULTA DE TRAZABILIDAD DE SU CARGA</li>
          </ol>
        </nav>
      </div>
           
          <%--  <div style="text-align:center">        
             <img class="mb-4" src="../img/logo-contecon-white.png" alt="" width="194" height="52"/>
           </div>--%>
            <%--<div>       
                <h5 class="font-weight-bold text-white">&nbsp;&nbsp;&nbsp;SERVICIO DE TRAZABILIDAD DE LA CARGA</h5>
            </div>--%>
            <%--<h1 class="text-white my-4">STC: TRAZABILIDAD DE LA CARGA</h1>
            <h1 class="h3 mb-3 font-weight-normal">Iniciar Sesión</h1>--%>
            <%--<div class="mt-4">         
                <nav class="mt-4" aria-label="breadcrumb">
                    <ol class="breadcrumb">
                    <li class="breadcrumb-item text-white" id="opcion_principal" runat="server">&nbsp;&nbsp;&nbsp;STC: TRAZABILIDAD DE LA CARGA</li>
                    <li class="breadcrumb-item text-white" aria-current="page" id="sub_opcion" runat="server">TRAZABILIDAD DE LA CARGA</li>
                    </ol>
                </nav>
            </div>--%>


            <div class="dashboard-container p-4" id="cuerpo" runat="server">

              <%--   <div class="form-title">
                     CRITERIOS DE BÚSQUEDAS
                </div>--%>

                 <div class='bs-example'>
                     <div class='accordion' id='accordionExample2'>
                         <div class='card'>
                             <div class='card-header' id='heading-busq'>
                                
                                   <h2 class='mb-0'>
                                       <button type = 'button' class='btn btn-link font-weight-bold text-red' data-toggle='collapse' data-target='#collapse_busq'><i class='fa fa-chevron-right'></i> CRITERIOS DE BÚSQUEDAS</button>
                                   </h2>
                                
                             </div>

                             <div id = 'collapse_busq' class='collapse' aria-labelledby='heading-busq' data-parent='#accordionExample2'>
                                 <div class='card-body'>
                                     <div class="form-row"> 

                                      

                                          <div class="form-group col-md-6">
                                             <label for="inputEmail4">NÚMERO DEL CONTENEDOR/CARGA:<span style="color: #FF0000; font-weight: bold;"> </span></label>
                                              <asp:TextBox ID="txtcontenedor" runat="server"  MaxLength="30"  class="form-control"
                                                     onkeypress="return soloLetras(event,'abcdefghijklmnñopqrstuvwxyz1234567890')"></asp:TextBox>
                                
                                        </div> 
                  
                                   

                                          <div class="form-group col-md-3"> 
                                                <label for="inputEmail4">DESDE:<span style="color: #FF0000; font-weight: bold;">  </span></label>
                                                <div class="d-flex">
                                                    <asp:TextBox ID="TxtFechaDesde" runat="server"  class="datetimepicker form-control"  ClientIDMode="Static" MaxLength="15"  placeholder="dd/MM/yyyy" onkeypress="return soloLetras(event,'1234567890/:')"></asp:TextBox>
                                                    <span style="color: #FF0000; font-weight: bold;"> * </span>
                                                 </div> 
                                          </div> 

                                         <div class="form-group col-md-3"> 
                                                <label for="inputEmail4">HASTA: <span style="color: #FF0000; font-weight: bold;">  </span></label>
                                                <div class="d-flex">
                                                    <asp:TextBox ID="txtFechaHasta" runat="server"  class="datetimepicker form-control"  ClientIDMode="Static" MaxLength="15"  placeholder="dd/MM/yyyy" onkeypress="return soloLetras(event,'1234567890/:')"></asp:TextBox>
                                                    <span style="color: #FF0000; font-weight: bold;"> * </span>
                                                 </div> 
                                          </div> 

                                      
                                    </div> 
                                     <div class="row">
                                         <div class="col-md-12 d-flex justify-content-center">
                                                  <asp:UpdatePanel ID="UPACCION" runat="server"  UpdateMode="Conditional" ChildrenAsTriggers="true">
                                                    <ContentTemplate>
                                                          <asp:Button ID="BtnBuscar" runat="server" class="btn btn-primary"  Text="BUSCAR"  OnClientClick="return mostrarloader('1')" OnClick="BtnBuscar_Click"  />                             
                                                           <img alt="loading.." src="../lib/file-uploader/img/loading.gif" height="32px" width="32px"  id="ImgCarga" class="nover"   />
                                                    </ContentTemplate>
                                                     <Triggers>
                                                      <asp:AsyncPostBackTrigger ControlID="BtnBuscar" />
                                                    </Triggers>
                                                    </asp:UpdatePanel>
                                          </div>
                                     </div>    
                                    <br/>
                                    <div class="row">
                                        <div class="col-md-12 d-flex justify-content-center">
                                            <asp:UpdatePanel ID="UPCARGA" runat="server"  UpdateMode="Conditional" ChildrenAsTriggers="true">
                                            <ContentTemplate>
                          
                                                        <div class="alert alert-warning" id="banmsg" runat="server" clientidmode="Static"><b>Error!</b>......</div>
    
                                                    </ContentTemplate>
                                                        <Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID="BtnBuscar" />
                                                </Triggers>
                                                </asp:UpdatePanel>
                                        </div>
                                    </div>
                                </div>
                             </div>
                         </div>
                    </div>
                </div>

              <%--  <div class='bs-example'>
                    <div class='accordion' id='accordionExample3'> 
                        <div class='card'>
                            d
                        </div>
                    </div>
               </div>--%>

                <div class="form-row">
                    <div class="form-group col-md-12"> 
                        <div class="cataresult" >

                            <asp:UpdatePanel ID="UPDETALLE" runat="server" UpdateMode="Conditional" >  
                                <ContentTemplate>
                                    <script type="text/javascript">
                                        Sys.Application.add_load(BindFunctions);
                                    </script>
          
                                    <div id="xfinder" runat="server" visible="false" >

                                        <!-- page start-->
                                        <div class="chat-room mt">
                                            <aside class="mid-side">
                                             
                                                <div class="catawrap" >
                                                    <div class="room-desk" id="htmlcasos" runat="server">
                                                    </div>
                                                </div>
                                            </aside>
         
                                        </div>
                                        <!-- page end-->
                                    </div>
                                    <%--<div id="sinresultado" runat="server" class=" alert  alert-warning" >
                                        No se encontraron resultados, 
                                        asegurese que ha escrito correctamente el número/nombre
                                        buscado  
                                    </div>--%>

                                </ContentTemplate>
                            </asp:UpdatePanel>

                        </div>
                    </div>

                         


                </div>


                    
            </div>

          


            <div id="myModal2" class="modal fade" tabindex="-1" role="dialog">
                <div class="modal-dialog" role="document" style="max-width: 1000px"> <!-- Este tag style controla el ancho del modal -->
                    <div class="modal-content">
                        <div class="modal-header">
                            <h5 class="modal-title">GALERIA DE IMAGENES</h5>
                            <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span>
                            </button>
                        </div>
                        <div class="modal-body">

                             <asp:UpdatePanel ID="UPMODAL" runat="server" UpdateMode="Conditional" >  
                                            <ContentTemplate>
                                                <asp:Panel ID="Panel1"  runat="server">         
                                                 

                                                    <asp:Button runat="server" ID="newButton" Text="" style="display:none;" OnClick="BtnCargarImagenes_Click" />
                                                    
                                                </asp:Panel>
                      
                                                <section class="wrapper2">
                                                    <div class="row mb"> 
                                                        <div class="content-panel">
                                                            <div class="adv-table">
                                                                <div class="bokindetalle" style="width:100%; overflow:auto">    
                                                                    
                                                                    <script type="text/javascript">
                                                                        Sys.Application.add_load(BindFunctions);
                                                                    </script>
          
                                                                    <div id="xfinde2" runat="server" visible="false" >

                                                                        <!-- page start-->
                                                                        <div class="chat-room mt">
                                                                            <aside class="mid-side">
                                                                                
                                                                                <div class="catawrap" >
                                                                                    <div class="room-desk" id="htmlImagenes" runat="server">

                                                                                      <%--  <div class='mb-5'>
                                                                                                <div id='carouselExampleCaptions' class='carousel slide' data-ride='carousel'>
                                                                                                <ol class='carousel-indicators'>
                                                                                                    <li data-target='#carouselExampleCaptions' data-slide-to='0' class='active'></li>
                                                                                                    <li data-target='#carouselExampleCaptions' data-slide-to='1'></li>
                                                                                                    <li data-target='#carouselExampleCaptions' data-slide-to='2'></li>
                                                                                                    <li data-target='#carouselExampleCaptions' data-slide-to='3'></li>
                                                                                                </ol>
                                                                                                <div class='carousel-inner'>
                                           
                                                                                                    <div class='carousel-item active'>
                                                                                                            <img src = '../img/1.png' class='d-block w-100' style='height:750px; overflow:auto' alt='...'/>
                                                                                                        <div class='carousel-caption d-none d-md-block'>
                                                                                                            <!-- <h5>Second slide label</h5>
                                                                                                            <p>Lorem ipsum dolor sit amet, consectetur adipiscing elit.</p>-->
                                                                                                        </div>
                                                                                                    </div> 
											                                                        <div class='carousel-item'>
                                                                                                        <img src = '../img/2.png' class='d-block w-100' style='height:750px; overflow:auto' alt='...'/>
                                                                                                        <div class='carousel-caption d-none d-md-block'>
                                                                                                            <!-- <h5>Second slide label</h5>
                                                                                                            <p>Lorem ipsum dolor sit amet, consectetur adipiscing elit.</p>-->
                                                                                                        </div>
                                                                                                    </div> 
											                                                        <div class='carousel-item'>
                                                                                                         <img src = '../img/3.png' class='d-block w-100' style='height:750px; overflow:auto' alt='...'/>
                                                                                                        <div class='carousel-caption d-none d-md-block'>
                                                                                                            <!-- <h5>Second slide label</h5>
                                                                                                            <p>Lorem ipsum dolor sit amet, consectetur adipiscing elit.</p>-->
                                                                                                        </div>
                                                                                                    </div> 
                                                                                                    <div class='carousel-item'>
                                                                                                          <img src = '../img/4.png' class='d-block w-100' style='height:750px; overflow:auto' alt='...'/>
                                                                                                        <div class='carousel-caption d-none d-md-block'>
                                                                                                            <!-- <h5>Second slide label</h5>
                                                                                                            <p>Lorem ipsum dolor sit amet, consectetur adipiscing elit.</p>-->
                                                                                                        </div>
                                                                                                    </div> 
                                                                                                </div>
                                                                                                <a class='carousel-control-prev' href='#carouselExampleCaptions' role='button' data-slide='prev'>
                                                                                                    <span class='carousel-control-prev-icon' aria-hidden='true'></span>
                                                                                                    <span class='sr-only'>Previous</span>
                                                                                                </a>
                                                                                                <a class='carousel-control-next' href='#carouselExampleCaptions' role='button' data-slide='next'>
                                                                                                    <span class='carousel-control-next-icon' aria-hidden='true'></span>
                                                                                                    <span class='sr-only'>Next</span>
                                                                                                </a>
                                                                                            </div>
                                                                                         </div>--%>


                                                                                    </div>
                                                                                </div>
                                                                            </aside>
                                                                                <br />
                                                                        </div>
                                                                        <!-- page end-->
                                                                    </div>
  
                                                            </div>
                                                        </div><!--content-panel-->
                                                        </div>
                                                    </div><!--row mb-->
                                                </section>

                                            </ContentTemplate>   
                                        </asp:UpdatePanel>

                        </div>
                        <div class="modal-footer d-flex justify-content-center">
                            <button type="button" class="btn btn-outline-primary " data-dismiss="modal">Cancelar</button>
                        </div>
                    </div>
                </div>
            </div>


   

    <script src="../Scripts/jquery.datetimepicker.js" type="text/javascript"></script>
    
    <script type="text/javascript">
        $(document).ready(function () {
            $('.datetimepicker').datetimepicker().datetimepicker({ lang: 'es', timepicker: false, closeOnDateSelect: true, format: 'd/m/Y' });
        });
    </script>

    <script type="text/javascript">
        $(document).ready(function () {
            $('.datetimepicker').datetimepicker().datetimepicker({ lang: 'es', timepicker: false, closeOnDateSelect: true, format: 'd/m/Y' });
        });
   </script>

  
        
    <script type="text/javascript">

        function clickaction(b)
        {
            try {
    
              
                this.document.getElementById("Idnot").value = b;

                var x = this.document.getElementById("Idnot").value;

            document.getElementById("<%=newButton.ClientID%>").click();

           

            }
            catch (e) {
               alert(e);
              
            }

           

        }

         function mostrar(id, secuencia) {
            var lz = "S";
            var url = '../carbono_neutro/certificado.aspx?sid=';
           
            var cc = secuencia;
            url = url + id + '&lg=' + lz;
            
            var w = window.open(url, 'Vista preliminar', 'width=850,height=900');
            w.focus();
        }

        function mostrar_impo(id, secuencia) {

            var lz = "S";
            var url = '../carbono_neutro/certificado_impo.aspx?sid=';

            var cc = secuencia;
            url = url + id + '&lg=' + lz;

            var w = window.open(url, 'Vista preliminar', 'width=850,height=900');
            w.focus();
        }

        function descarga(id, secuencia) {

            var lz = "S";
            var url = '../handler/certificado.ashx?sid=';

            var cc = secuencia;
            url = url + id + '&lg=' + lz;
            
            var iframe = document.createElement("iframe");
            iframe.src = url;
            iframe.style.display = "none";
            document.body.appendChild(iframe);
        }

        function descarga_impo(id, secuencia) {

            var lz = "S";
            var url = '../handler/certificadoimpo.ashx?sid=';

            var cc = secuencia;
            url = url + id + '&lg=' + lz;

            var iframe = document.createElement("iframe");
            iframe.src = url;
            iframe.style.display = "none";
            document.body.appendChild(iframe);

           
        }
        
    </script>


 </asp:Content>