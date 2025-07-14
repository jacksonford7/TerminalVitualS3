<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="damageconsultaimportador.aspx.cs" Inherits="CSLSite.damageconsultaimportador" %>
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
  <link rel="stylesheet" type="text/css" href="../lib/gritter/css/jquery.gritter.css" />




 
   
   <link href="https://fonts.googleapis.com/css2?family=Roboto:wght@300;400;500;700&display=swap" rel="stylesheet"/>
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/5.15.1/css/all.min.css" integrity="sha512-+4zCK9k+qNFUR5X+cKL9EIR+ZOhtIloNl9GIKS57V1MyNsYpYcUrUeQc9vNfzsWfV28IaLL3i96P9sdNyeRssA==" crossorigin="anonymous" />


  <link href="../css/datatables.min.css" rel="stylesheet" />
  <link rel="stylesheet" type="text/css" href="..js/buttons/css1.6.4/buttons.dataTables.min.css"/>





    <style>
        .accordion-container {
            width: 100%;
            background: #fff;
            border-radius: 8px;
            box-shadow: 0 4px 8px rgba(0, 0, 0, 0.2);
            padding: 20px;
        }

        /* Estilo de los botones del acordeón */
        .accordion {
            background-color: #E23B1B;
            color: white;
            cursor: pointer;
            padding: 15px;
            width: 100%;
            border: none;
            text-align: left;
            outline: none;
            font-size: 18px;
            transition: background-color 0.3s ease-in-out;
            display: flex;
            justify-content: space-between;
            align-items: center;
            border-radius: 5px;
            margin-bottom: 5px;
        }

        .accordion:hover {
            background-color: #E23B1B;
        }

        /* Icono de más/menos */
        .accordion::after {
            content: "\25B6"; /* Flecha derecha */
            font-size: 16px;
            transition: transform 0.3s ease-in-out;
        }

        .accordion.active::after {
            transform: rotate(90deg);
        }

        /* Panel de contenido */
        .panel {
            padding: 15px;
            background-color: #F9F9F9;
            border-radius: 5px;
            border-left: 3px solid #E23B1B;
            margin-bottom: 5px;
            display: block;
        }

        .image-container
        {
            padding-top : 20px;
            padding-bottom : 20px;
            display: flex;
            flex-wrap: wrap;
            justify-content: center;
            gap: 10px;
            margin-top: 20px;
            border-radius: 5px;
            border-left: 3px solid #E23B1B;
            background-color: #ffffff;
        }

         .thumbnail {
           
            width: 100px;
            height: 100px;
            object-fit: cover;
            cursor: pointer;
            border: 2px solid transparent;
            transition: 0.3s;
            border-color: #E23B1B;
        }

        .thumbnail:hover {
            border-color: #000000;
        }
       

       
         /* Estilos del Pop-up */
        .modal {
            display: none;
            position: fixed;
            z-index: 1000;
            left: 0;
            top: 0;
            width: 100%;
            height: 100%;
            background-color: rgba(0, 0, 0, 0.7);
        }

        .modal-content {
            margin: 10% auto;
            display: block;
            width: 100%;
            height: 50%;
            max-width: 900px;
            height: 680px;
            border-radius: 10px;
            background: white;
            padding: 10px;
            position: relative;
        }

        .close {
            position: absolute;
            top: 10px;
            right: 15px;
            font-size: 50px;
            cursor: pointer;
            color: red;
        }

        .close:hover {
            color: red;
        }

        .large-image {
            width: 100%;
            height: 100%;
            /*height: auto;*/
            border-radius: 5px;
        }


        </style>

<script type="text/javascript">


 function fechas()
   {
    $(document).ready(function () {
        $('.datetimepicker').datetimepicker().datetimepicker({ lang: 'es', timepicker: false, step: 30, format: 'm/d/Y' });
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
            <li class="breadcrumb-item" id="opcion_principal" runat="server"><a href="#">DAMAGE CONTROL</a></li>
            <li class="breadcrumb-item active" aria-current="page" id="sub_opcion" runat="server">CONSULTAR IMAGENES DE UN CONTENEDOR (IMPORTADOR)</li>
          </ol>
        </nav>
      </div>

 <div class="dashboard-container p-4" id="cuerpo" runat="server">

      <section id="main-content">
           <section class="wrapper">

                <div class="row mt">
                        <asp:UpdatePanel ID="UPMENSAJE" runat="server"  UpdateMode="Conditional" ChildrenAsTriggers="true">
                       <ContentTemplate>
                          <div class="row">
                            <div class="col-md-12 d-flex justify-content-center">
                                 <div class="alert alert-warning" id="banmsg" runat="server" clientidmode="Static"><b>Error!</b> >......</div>
                            </div>
                          </div>

                        </ContentTemplate>
                        <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="tablePagination" />
                    </Triggers>
                        </asp:UpdatePanel>
                </div>
                <div class="row mt">
                     


                     <div class="col-sm-3" >
                         <div class="dashboard-container p-4" id="Div_Contenedores" runat="server" >
                              
                             <div class="form-group col-md-12"> 
                                    <label for="inputZip" style="color:#E23B1B">DETALLE DE UNIDADES</label>  
                              </div>
                             
                               <asp:UpdatePanel ID="UPBUSCADOR_UNIDADES" runat="server" UpdateMode="Conditional" >  
                                <ContentTemplate>
                                      <div class="form-group col-md-12 d-flex"> 
                                         <asp:TextBox ID="txtFiltro" runat="server" class="form-control"  width="100%"  MaxLength="20" onkeypress="return soloLetras(event,'abcdefghijklmnopqrstuvwxyz0123456789-_')" placeholder="FILTRO CONTENEDOR"></asp:TextBox>
			                     &nbsp; 
                                        <asp:LinkButton runat="server" ID="BtnFiltrarUnidades" Text="<span class='fa fa-search' style='font-size:24px'></span>"   class="btn btn-primary"  OnClick="BtnFiltrarUnidades_Click"/>
                                         &nbsp;    <img alt="loading.." src="../lib/file-uploader/img/loading.gif" height="32px" width="32px"  id="ImgCarga" class="nover"   />     
                                     </div>
                                </ContentTemplate>
                                </asp:UpdatePanel>
                                 
                                  <asp:UpdatePanel ID="UPMUESTRA_UNIDADES"  runat="server"  UpdateMode="Conditional">
                                    <ContentTemplate>   
                                       
                                     <div class="col-md-12 d-flex justify-content-center">
                                     
                                            <asp:GridView ID="tablePagination" runat="server" AutoGenerateColumns="False"  
                                                        DataKeyNames="GKEY"
                                                        GridLines="None" 
                                                        OnPageIndexChanging="tablePagination_PageIndexChanging" 
                                                        OnPreRender="tablePagination_PreRender"     
                                                        OnRowCommand="tablePagination_RowCommand"   
                                                        OnRowDataBound="tablePagination_RowDataBound"
                                                        PageSize="15"
                                                        AllowPaging="True" Font-Size="11px"
                                                 CssClass="display table table-bordered">
                                                      
                                                    <PagerStyle HorizontalAlign = "Right" CssClass="display table table-bordered"  />
                                                    <RowStyle  BackColor="#F0F0F0" />
                                                    <alternatingrowstyle  BackColor="#FFFFFF" />
                           
                                                    <Columns>
                                                        <asp:BoundField DataField="CONTENEDOR" HeaderText="CONTENEDOR" Visible="true"  HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone">
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="FECHA_REGISTRO" HeaderText="FECHA" Visible="true"  HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"> 
                                                        </asp:BoundField>
                                                        <asp:TemplateField HeaderText="" ItemStyle-CssClass="center hidden-phone" >
                                                            <ItemTemplate>
                                                                <asp:Button runat="server" ID="btnFactura" Height="30px" CommandName="Actualizar" Text="VER" ToolTip="Ver Imagenes" 
                                                                     OnClientClick="return mostrarloader('1')" 
                                                                    CommandArgument='<%# Bind("GKEY") %>' class="btn btn-primary" 
                                                                  />                                                                       
                                                            </ItemTemplate>
                                                        </asp:TemplateField>

                                                    </Columns>
                                            </asp:GridView>
                                        </div>

                                
                                    </ContentTemplate>
                                                     
                                </asp:UpdatePanel>

                         </div>
                     </div>

                     <div class="col-sm-9"> 
                          <div class="dashboard-container p-4" id="Div_Imagenes" runat="server" > 


                                <div class="form-row">
                                    <div class="form-group col-md-12"> 
                
                                        <div class="accordion-container">

                                            <h2>Información del contenedor</h2> 
               
                                            <asp:UpdatePanel ID="UPINFOCONTENEDOR" runat="server" UpdateMode="Conditional" >  
                                             <ContentTemplate>
                                                 
                                               <asp:Button ID="btnAccordion1" CssClass="accordion" runat="server" Text=""  />
                                                <div class="panel">

                                                    <div class="form-row">
                                                        <div class="form-group col-md-3"> 
                                                            <label style="color:#000000;font-weight:bold;">Size:</label>
                                                              <asp:TextBox ID="TxtSize" runat="server"  class="form-control" disabled></asp:TextBox>
                                                        </div>
                                                        <div class="form-group col-md-3"> 
                                                            <label style="color:#000000;font-weight:bold;">Type ISO:</label>
                                                              <asp:TextBox ID="TxtTipoIso" runat="server"  class="form-control" disabled></asp:TextBox>
                                                        </div>
                                                         <div class="form-group col-md-3"> 
                                                            <label style="color:#000000;font-weight:bold;">Line OP:</label>
                                                              <asp:TextBox ID="TxtLinea" runat="server"  class="form-control" disabled></asp:TextBox>
                                                        </div>
                                                        <div class="form-group col-md-3"> 
                                                            <label style="color:#000000;font-weight:bold;">Category:</label>
                                                              <asp:TextBox ID="TxtCategoria" runat="server"  class="form-control" disabled></asp:TextBox>
                                                        </div>


                                                        <div class="form-group col-md-3"> 
                                                            <label style="color:#000000;font-weight:bold;">Freight Kind:</label>
                                                              <asp:TextBox ID="TxtFreightKind" runat="server"  class="form-control" disabled></asp:TextBox>
                                                        </div>
                                                        <div class="form-group col-md-3"> 
                                                            <label style="color:#000000;font-weight:bold;">Weight (kg):</label>
                                                              <asp:TextBox ID="TxtPeso" runat="server"  class="form-control" disabled></asp:TextBox>
                                                        </div>
                                                         <div class="form-group col-md-3"> 
                                                            <label style="color:#000000;font-weight:bold;">Document:</label>
                                                              <asp:TextBox ID="TxtDocumento" runat="server"  class="form-control" disabled></asp:TextBox>
                                                        </div>
                                                           <div class="form-group col-md-3"> 
                                                            <label style="color:#000000;font-weight:bold;">Reference:</label>
                                                              <asp:TextBox ID="TxtReferencia" runat="server"  class="form-control" disabled></asp:TextBox>
                                                        </div>
                                                         <div class="form-group col-md-3"> 
                                                            <label style="color:#000000;font-weight:bold;">CAS:</label>
                                                              <asp:TextBox ID="TxtFechaCas" runat="server"  class="form-control" disabled></asp:TextBox>
                                                        </div>
                                                        <div class="form-group col-md-3"> 
                                                            <label style="color:#000000;font-weight:bold;">&nbsp;</label>
                                                            <div class="d-flex">    
                                                              <asp:Button ID="BtnDescargar" runat="server" class="btn btn-primary"   Text="DESCARGAR IMAGENES"  OnClick="BtnDescargar_Click"  OnClientClick="return mostrarloader('2')" />  
                                                                  &nbsp;    <img alt="loading.." src="../lib/file-uploader/img/loading.gif" height="32px" width="32px"  id="ImgCargaZip" class="nover"   /> 
                                                               </div>
                                                        </div>
                                                         <div class="form-group col-md-3"> 
                                                            <label style="color:#000000;font-weight:bold;">&nbsp;</label>
                                                             <div class="d-flex">    
                                                             <asp:HyperLink ID="LinkDescargarZip" runat="server"  Visible="false" style="color:#F9F9F9;"   >Ver archivo ZIP</asp:HyperLink>
                                                             
                                                             </div>
                                                        </div>
                                                    </div>
                      

                                                    <div class="image-container">
                         
                                                           <asp:Repeater ID="rptImagenes" runat="server" >
                                                           <ItemTemplate>


                                                                <asp:Image ID="imgPicture" runat="server" ImageUrl='<%# Eval("Url") %>'   
                                                                    class="thumbnail"  onclick='<%# "mostrarImagenGrande(\"" + Eval("Url_Large") + "\")" %>'
                                                                  
                                                                    />
                                                            </ItemTemplate>
                                                        </asp:Repeater>
                          
                                                    </div>
                                                </div>
                                            </ContentTemplate>   
                                            </asp:UpdatePanel>
                                        </div>

                                    </div>
                                </div>

                          </div>
                     </div>
                </div><!-- row mt-->
           </section><!--wrapper site-min-height-->
      </section><!--main-content-->
 


    

  </div>


  <div id="myModal" class="modal modal" >
     
        <div class="modal-content">
            <span class="close" onclick="cerrarModal()">&times;</span>
           
            <img id="imgLarge" class="large-image" src="" alt="Imagen grande" />
        </div>

    </div>

  


<script type="text/javascript" src="../lib/datatables/datatables.min.js"></script>


   <script type="text/javascript" src="../lib/common-scripts.js"></script>
 
   <script type="text/javascript" src="../lib/pages.js" ></script>

   <script type="text/javascript" src="../lib/advanced-form-components.js"></script>

 <script type="text/javascript" src='../lib/autocompletar/bootstrap3-typeahead.min.js'></script>

     <script type="text/javascript" src="../js/buttons/1.6.4/dataTables.buttons.min.js"></script>  
     <script type="text/javascript" src="../js/buttons/1.6.4/jszip.min.js"></script>  
     <script type="text/javascript" src="../js/buttons/1.6.4/pdfmake.min.js"></script>  
    <script type="text/javascript" src="../js/buttons/1.6.4/vfs_fonts.js"></script>  
    


<script type="text/javascript">

    function confirmacion() {
        var mensaje;
        var opcion = confirm("¿Estimado cliente, está seguro de que desea aceptar el servicio de visualización de fotos de contenedores?");
        if (opcion == true) {
            loader();
            return true;
        } else {
            return false;
        }

    }

</script>
    

<script type="text/javascript">

  $(document).ready(function () {
      $('.datetimepicker').datetimepicker().datetimepicker({ lang: 'es', timepicker: false, step: 30, format: 'm/d/Y' });
        });
      
       


    function mostrarloader(Valor) {

        try {
            if (Valor == "1") {
                document.getElementById("ImgCarga").className = 'ver';
            }
            else {
                document.getElementById("ImgCargaZip").className = 'ver';
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
                document.getElementById("ImgCargaZip").className = 'nover';
            }

             } catch (e) {
                alert('Lamentamos comunicarle que ha ocurrido el siguiente problema:\n' + e.Message);
            }
        }

</script>



    <script type="text/javascript">

        function mostrarImagenGrande(ruta)
        {
            document.getElementById("imgLarge").src = ruta;
            document.getElementById("myModal").style.display = "block";
        }

        function cerrarModal() {
            document.getElementById("myModal").style.display = "none";
        }

        // Cerrar modal si se hace clic fuera de la imagen
        window.onclick = function (event) {
            var modal = document.getElementById("myModal");
            if (event.target === modal) {
                cerrarModal();
            }
        }
    </script>


    <script  type="text/javascript">
        function autoDownload() {
            var link = document.getElementById('<%= LinkDescargarZip.ClientID %>');
            if (link) {
                link.click(); // Simula el clic en el enlace de descarga
            }
        }
</script>


</asp:Content>