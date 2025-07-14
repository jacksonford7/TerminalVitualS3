<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="transp_actualizadocumentos.aspx.cs" Inherits="CSLSite.contenedorexpo.transp_actualizadocumentos" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>


<asp:Content ID="Content1" ContentPlaceHolderID="placehead" runat="server">

    <link href="../shared/estilo/catalogos.css" rel="stylesheet" type="text/css" />
   
    <link href="../img/favicon2.png" rel="icon"/>
    <link href="../img/icono.png" rel="apple-touch-icon"/>
    <link href="../css/bootstrap.min.css" rel="stylesheet"/>
    <link href="../css/dashboard.css" rel="stylesheet"/>
    <link href="../css/icons.css" rel="stylesheet"/>
    <link href="../css/style.css" rel="stylesheet"/>
    <link href="https://fonts.googleapis.com/css2?family=Roboto:wght@300;400;500;700&display=swap" rel="stylesheet"/>

 <%-- <link href="../css/datatables.min.css" rel="stylesheet" />
  <link rel="stylesheet" type="text/css" href="..js/buttons/css1.6.4/buttons.dataTables.min.css"/>--%>





     <script type="text/javascript">
         $(document).ready(function () {
             $('#<%= tablePagination.ClientID %>').dataTable();
         });

    </script>

    <script type="text/javascript">
         $(document).ready(function () {
             $('#<%= tableVehiculos.ClientID %>').dataTable();
         });

    </script>


    

     <script type="text/javascript">
         $(document).ready(function () {
             $('#<%= GrillaDetalle.ClientID %>').dataTable();
         });

    </script>
   

  


</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="placebody" runat="server" >
    <asp:ToolkitScriptManager ID="tkscata" runat="server"></asp:ToolkitScriptManager>
     <input id="CODIGO" type="hidden" value="" runat="server" clientidmode="Static" />
     <input id="TIPO" type="hidden" value="" runat="server" clientidmode="Static" />
     <input id="DOCUMENTO" type="hidden" value="0" runat="server" clientidmode="Static" />
   
    <div id="div_BrowserWindowName" style="visibility:hidden;">
        <asp:HiddenField ID="hf_BrowserWindowName" runat="server" />
    </div>


    <div class="mt-4">         
        <nav class="mt-4" aria-label="breadcrumb">
            <ol class="breadcrumb">
                <li class="breadcrumb-item" id="opcion_principal" runat="server"><a href="#">Transportistas</a></li>
                <li class="breadcrumb-item active" aria-current="page" id="sub_opcion" runat="server">ACTUALIZACIÓN DE DOCUMENTOS</li>
            </ol>
        </nav>
    </div>

     <%--PANEL DE CABECERA--%>
    <div class="dashboard-container p-4" id="cuerpo" runat="server">
       
             <asp:UpdatePanel ID="UPCARGA" runat="server"  UpdateMode="Conditional" >
                <ContentTemplate>
                    <asp:Panel ID="PanelCualquiera" runat="server" >
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
                    
                        <img alt="loading.." src="../lib/file-uploader/img/loading.gif" height="32px" width="32px"  id="ImgCarga" class="nover"/>
                        
                     </div>
                    </asp:Panel>            
                </ContentTemplate>   
            </asp:UpdatePanel>     
        
        

    </div>

    <br />

    <section id="main-content">
        <section class="wrapper">


           <div class="row mt">
               <div class="col-sm-4" >

                     <div class="dashboard-container p-4" id="Div1" runat="server" style="height:1150px" >
                   
                                <div class="form-group col-md-12">
                                    <label for="inputZip" style="color:#E23B1B">Detalle de Colaboradores</label>  
                                </div>
                            
                                <asp:UpdatePanel ID="UPBUSCACOLABORADOR" runat="server" UpdateMode="Conditional" >  
                                <ContentTemplate>
                                     <div class="d-flex">
                                         <asp:TextBox ID="txtFiltro" runat="server" class="form-control" MaxLength="20" onkeypress="return soloLetras(event,'abcdefghijklmnopqrstuvwxyz0123456789-_')" placeholder="FILTRO"></asp:TextBox>
						                 &nbsp;
                                        <asp:LinkButton runat="server" ID="BtnFiltrarColaborador" Text="<span class='fa fa-search' style='font-size:24px'></span>"    OnClick="BtnFiltrarColaborador_Click" class="btn btn-primary" />
                                     </div>
                                </ContentTemplate>
                                </asp:UpdatePanel>

                                <asp:UpdatePanel ID="UPCOLABORADOR"  runat="server"  UpdateMode="Conditional">
                                    <ContentTemplate>   
                                       
                                        <div class="bokindetalle" style="height:460px; width:100%; overflow:auto">    

                                            <asp:GridView ID="tablePagination" runat="server" AutoGenerateColumns="False"  
                                                        DataKeyNames="NOMINA_COD"
                                                        GridLines="None" 
                                                        OnPageIndexChanging="tablePagination_PageIndexChanging" 
                                                        OnPreRender="tablePagination_PreRender"     
                                                        OnRowCommand="tablePagination_RowCommand"   
                                                        OnRowDataBound="tablePagination_RowDataBound"
                                                        PageSize="6"
                                                        AllowPaging="True" Font-Size="10px"
                                                 CssClass="display table table-bordered">
                                                      
                                                    <PagerStyle HorizontalAlign = "Right" CssClass="display table table-bordered"  />
                                                    <RowStyle  BackColor="#F0F0F0" />
                                                    <alternatingrowstyle  BackColor="#FFFFFF" />
                           
                                                    <Columns>
                                                        <asp:BoundField DataField="COLABORADOR" HeaderText="COLABORADOR" Visible="true"  HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>
                                                        <asp:BoundField DataField="ESTADO" HeaderText="ESTADO" Visible="true"   HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>           
                                                        <asp:BoundField DataField="ESTADO2" HeaderText="ESTADO" Visible="false"   HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/> 
                                                        <asp:TemplateField HeaderText="" ItemStyle-CssClass="center hidden-phone">
                                                            <ItemTemplate>
                                                                  
                                                                <asp:Button runat="server" ID="btnFactura" Height="30px" CommandName="Actualizar" Text="AC" ToolTip="Actualizar Documentos"  CommandArgument='<%# Bind("NOMINA_COD") %>' class="btn btn-primary" 
                                                                  />                                                                       
                                                            </ItemTemplate>
                                                        </asp:TemplateField>

                                                    </Columns>
                                            </asp:GridView>
                                        </div>

                                
                                    </ContentTemplate>
                                                     
                                </asp:UpdatePanel>
                                        
                               <div class="form-group col-md-12">
                                    <label for="inputZip" style="color:#E23B1B">Detalle de Vehículos</label>  
                               </div>
                                 <asp:UpdatePanel ID="UPBUSCAVEHICULO" runat="server" UpdateMode="Conditional" >  
                                <ContentTemplate>
                                     <div class="d-flex">
                                         <asp:TextBox ID="TxtFiltrarVehiculo" runat="server" class="form-control" MaxLength="20" onkeypress="return soloLetras(event,'abcdefghijklmnopqrstuvwxyz0123456789-_')" placeholder="FILTRO"></asp:TextBox>
						                 &nbsp;
                                        <asp:LinkButton runat="server" ID="BtnFiltrarVehiculo" Text="<span class='fa fa-search' style='font-size:24px'></span>"    OnClick="BtnFiltrarVehiculo_Click" class="btn btn-primary" />
                                     </div>
                                </ContentTemplate>
                                </asp:UpdatePanel>
                                 <asp:UpdatePanel ID="UPVEHICULO"  runat="server"  UpdateMode="Conditional">
                                    <ContentTemplate>        
                                        <div class="bokindetalle" style="height:550px; width:100%; overflow:auto">    

                                            <asp:GridView ID="tableVehiculos" runat="server" AutoGenerateColumns="False"  
                                                        DataKeyNames="PLACA"
                                                        GridLines="None" 
                                                        OnPageIndexChanging="tableVehiculos_PageIndexChanging" 
                                                        OnPreRender="tableVehiculos_PreRender"     
                                                        OnRowCommand="tableVehiculos_RowCommand"   
                                                        OnRowDataBound="tableVehiculos_RowDataBound"
                                                        PageSize="7"
                                                        AllowPaging="True"
                                                        Font-Size="10px"
                                                 CssClass="display table table-bordered">
                                                      
                                                    <PagerStyle HorizontalAlign = "Right" CssClass="display table table-bordered"  />
                                                    <RowStyle  BackColor="#F0F0F0" />
                                                    <alternatingrowstyle  BackColor="#FFFFFF" />
                           
                                                    <Columns>
                                                        <asp:BoundField DataField="PLACA" HeaderText="PLACA" Visible="true"  HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>
                                                        <asp:BoundField DataField="VE_POLIZA" HeaderText="V/POLIZA" Visible="true"   HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/> 
                                                        <asp:BoundField DataField="FECHAMTOP" HeaderText="V/MTOP" Visible="true"   HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>          
                                                        <asp:BoundField DataField="ESTADO" HeaderText="ESTADO" Visible="false"   HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/> 
                                                        <asp:TemplateField HeaderText="" ItemStyle-CssClass="center hidden-phone">
                                                            <ItemTemplate>
                                                              
                                                                <asp:Button runat="server" ID="btnFactura" Height="30px" CommandName="Actualizar" Text="AC" ToolTip="Actualizar Documentos"  CommandArgument='<%# Bind("PLACA") %>' class="btn btn-primary"   />                                                             
                                                            </ItemTemplate>
                                                        </asp:TemplateField>

                                                    </Columns>
                                            </asp:GridView>
                                        </div>

                                
                                    </ContentTemplate>
                                                     
                                </asp:UpdatePanel>
                    </div>

               
              </div> 

              
            
              <div class="col-sm-8">                   
                        <div class="dashboard-container p-4" id="Div2" runat="server" style="height:1150px">

                         

                                        <asp:UpdatePanel ID="UPDET" runat="server" UpdateMode="Conditional" >  
                                            <ContentTemplate>
                                                     
                                                  <div class="form-group col-md-12">
                                                         <label for="inputZip">DOCUMENTOS:<span style="color: #FF0000; font-weight: bold;"></span></label>
                                                         <div class="d-flex">
                                                            <asp:TextBox ID="txtID"  runat="server" class="form-control"   placeholder="" size="16"  Font-Bold="false" disabled></asp:TextBox>
                                                             <a name="link" id="link"></a>
                                                        </div>
                                                      <br/>
                                                      <div class="alert alert-warning" id="banmsg" runat="server" clientidmode="Static"><b>Error!</b> ......</div>
                                                    </div>
                                        
                                                <br />

                                                             
                                                                    <asp:GridView ID="GrillaDetalle" runat="server" AutoGenerateColumns="False"  DataKeyNames="ID_DOCUMENTO"
                                                                                            GridLines="None" 
                                                                                            PageSize="10"
                                                                                            AllowPaging="True"
                                                                                         
                                                                                            CssClass="table table-bordered invoice"
                                                                                            OnRowDataBound="GrillaDetalle_RowDataBound"
                                                                                            OnRowCommand="GrillaDetalle_RowCommand" 
                                                                                            OnPageIndexChanging="GrillaDetalle_PageIndexChanging" 
                                                                                            OnPreRender="GrillaDetalle_PreRender">
                                                                            <PagerStyle HorizontalAlign = "Right" CssClass="table table-bordered invoice"  />
                                                                            <RowStyle  BackColor="#F0F0F0" />
                                                                            <alternatingrowstyle  BackColor="#FFFFFF" />
                           
                                                                        <Columns>       
                                                              

                                                                            <asp:BoundField DataField="DESC_DOCUMENTO"  HeaderText="DOCUMENTO" SortExpression="ID_DOCUMENTO" HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>
                                                                            <asp:BoundField DataField="FECHA_CADUCA"  HeaderText="FECHA CADUCIDAD"  HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>
                                                                            <asp:BoundField DataField="EXT_DOCUMENTO" HeaderText="EXTENSION" Visible="false" HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>
                                                                            <asp:BoundField DataField="ID_SOLICITUD" HeaderText="ID_SOLICITUD" Visible="false" HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>
                                                                            <asp:BoundField DataField="COD_SOLICITUD" Visible="false" HeaderText="COD_SOLICITUD"  HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>
                                                                            <asp:BoundField DataField="ID_DOCUMENTO" Visible="false" HeaderText="ID_DOCUMENTO" HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>
                                                                            <asp:BoundField DataField="COD_DOCUMENTO" Visible="false" HeaderText="COD_DOCUMENTO" HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>
                                                                             <asp:BoundField DataField="ESTADO" Visible="true" HeaderText="ESTADO" HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>
                                                                           <asp:BoundField DataField="RUTA" HeaderText="RUTA"  HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone" ItemStyle-Width="300px"/>

                                                                             <asp:TemplateField HeaderText="Seleccionar" ItemStyle-CssClass="center hidden-phone">
                                                                                <ItemTemplate>
                                                                                    <asp:Button runat="server" ID="IncreaseButton" Text="..." CommandName="Buscar" CommandArgument='<%#Eval("ID_DOCUMENTO")%>' class="btn btn-outline-primary" 
                                                                                        data-toggle="modal" data-target="#myModal2" ToolTip="Cargar documento" />
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                        </Columns>
                                                                    </asp:GridView>

                                                                      <br />
                                                                   <div class="row">
                                                                        <div class="col-md-12 d-flex justify-content-center">
                                                                                     <img alt="loading.." src="../lib/file-uploader/img/loading.gif" height="32px" width="32px"  id="ImgCargaDet" class="nover"  />
                                                                         </div>
                                                                    </div>    
                                                                     <div class="row"> 
                                                                      <div class="col-md-12 d-flex justify-content-center"> 
                                                                           <asp:Button ID="BtnGenerar" runat="server" class="btn btn-primary" Text="ENVIAR SOLICITUD"  OnClientClick="return confirmacion();" OnClick="BtnGenerar_Click" />
                                                                      </div>
                                                                 </div>

                                                             
                                               


                                            </ContentTemplate>   
                                        </asp:UpdatePanel>   

                                <asp:UpdatePanel ID="UPBOTONES" runat="server" UpdateMode="Conditional" >  
                <ContentTemplate>
                
                    <div class="form-group">
                        <br/>
                            <div class="alert alert-warning" id="banmsg_det" runat="server" clientidmode="Static"><b>Error!</b> >......</div>
                    </div>
                  
                </ContentTemplate>
            </asp:UpdatePanel>   
                      
                        </div>
                   
              </div> <!-- col-sm-9-->
          </div><!-- row mt-->
        

        
	    


         

        <div id="myModal2" class="modal fade" tabindex="-1" role="dialog">
            <div class="modal-dialog" role="document" style="max-width: 900px"> <!-- Este tag style controla el ancho del modal -->
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title">CARGA DE ARCHIVO</h5>
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                        </button>
                    </div>
                    <div class="modal-body">
                        <br/> <br/>
                        <input id="ruta_completa" type="hidden" value="" runat="server" clientidmode="Static" />
                        <input id="nombre_archivo1" type="hidden" value="" runat="server" clientidmode="Static" />

                         <asp:UpdatePanel ID="UPMODAL" runat="server" UpdateMode="Conditional" >  
                                        <ContentTemplate>
                                            <asp:Panel ID="Panel1" Visible="false"  runat="server">         
                                                <p>DOCUMENTO:<asp:TextBox ID="txtContainers"  runat="server" class="form-control"   placeholder="" size="16"  Width="200px" Font-Bold="false" disabled></asp:TextBox></p>
                                            </asp:Panel>
  
                                        </ContentTemplate>   
                                    </asp:UpdatePanel>

                       

                              <div class="form-row">
                                 <div class="form-group col-md-12"> 
		   	                      <label for="inputAddress">Archivo PDF<span style="color: #FF0000; font-weight: bold;"></span></label>
			                      <asp:AsyncFileUpload ID="fsuploadarchivo" runat="server"  
                                      title="Escoja el archivo con formato indicado .pdf"  style=" font-size:small" visible="true"  class="btn btn-primary"/>
		                       </div>
          
                             </div>
                          <br/> <br/>
                              <div class="form-row">
		                               <div class="form-group col-md-12"> 
		   	                              <p class="alert alert-light" id="sinresultado" runat="server" visible="false"></p>
		                               </div>
	                           </div>
                                

                                  <div class="form-row">
           
                                     <span id="imagen"></span>   
                                </div>
                             
                               <input id="json_object" type="hidden" />

                       

                    </div>
                    <div class="modal-footer d-flex justify-content-center">
                         <asp:Button ID="find" runat="server" Text="  Cargar  " onclick="find_Click" CssClass="btn btn-primary" />
                        <button type="button" class="btn btn-outline-primary "   data-dismiss="modal">Cancelar</button>
                    </div>
                </div>
            </div>
        </div>

      
         


   

        </section><!--wrapper site-min-height-->
    </section><!--main-content-->


  <!--script for this page--> 
  <script type="text/javascript" src="../lib/pages.js" ></script>
 <%--<script type="text/javascript"  src="../js/datatables.js"></script>--%>

    <%-- <script type="text/javascript" src="../js/buttons/1.6.4/dataTables.buttons.min.js"></script>  
     <script type="text/javascript" src="../js/buttons/1.6.4/jszip.min.js"></script>  
     <script type="text/javascript" src="../js/buttons/1.6.4/pdfmake.min.js"></script>  
    <script type="text/javascript" src="../js/buttons/1.6.4/vfs_fonts.js"></script>  

    <script type="text/javascript" src="../js/buttons/1.6.4/buttons.print.min.js"></script>  
     <script type="text/javascript" src="../js/buttons/1.6.4/buttons.html5.min.js"></script>  
     <script type="text/javascript" src="../js/buttons/1.6.4/buttons.colVis.min.js"></script>  --%>

<script type="text/javascript"> 
    function mostrarloader(Valor) {

        try {
            if (Valor == "1") {
                document.getElementById("ImgCarga").className = 'ver';
            }
            else {
                document.getElementById("ImgCargaDet").className='ver';
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
                document.getElementById("ImgCargaDet").className='nover';
            }

             } catch (e) {
                alert('Lamentamos comunicarle que ha ocurrido el siguiente problema:\n' + e.Message);
            }
        }
</script>

  
<script type="text/javascript">

     function confirmacion()
    {
        var mensaje;
        var opcion = confirm("Esta seguro que desea generar la solicitud?");
        if (opcion == true)
        {
            mostrarloader('2');
            return true;
        } else
        {
          
	         return false;
        }

       
    }

</script>

</asp:Content>
