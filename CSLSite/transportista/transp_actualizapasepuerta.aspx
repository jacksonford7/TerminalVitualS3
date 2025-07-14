<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="transp_actualizapasepuerta.aspx.cs" Inherits="CSLSite.contenedorexpo.transp_actualizapasepuerta" %>
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

      <!--external css-->
      <link href="../shared/estilo/jquery.datetimepicker.css" rel="stylesheet" type="text/css" />
      <script src="../Scripts/jquery.datetimepicker.js" type="text/javascript"></script>


     <link href="https://fonts.googleapis.com/css2?family=Roboto:wght@300;400;500;700&display=swap" rel="stylesheet"/>
        <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/5.15.1/css/all.min.css" integrity="sha512-+4zCK9k+qNFUR5X+cKL9EIR+ZOhtIloNl9GIKS57V1MyNsYpYcUrUeQc9vNfzsWfV28IaLL3i96P9sdNyeRssA==" crossorigin="anonymous" />


      <link href="../css/datatables.min.css" rel="stylesheet" />
      <link rel="stylesheet" type="text/css" href="..js/buttons/css1.6.4/buttons.dataTables.min.css"/>


   
<script type="text/javascript">

    function BindFunctions_Impo()
       {
       $(document).ready(function() {    
        $('#grilla_importacion').DataTable({        
            language: {
                    "lengthMenu": "_MENU_ REGISTROS POR PAGINA",
                    "zeroRecords": "No se encontraron resultados",
                    "info": "Mostrando registros del _START_ al _END_ de un total de _TOTAL_ registros",
                    "infoEmpty": "Mostrando registros del 0 al 0 de un total de 0 registros",
                    "infoFiltered": "(filtrado de un total de _MAX_ registros)",
                    "sSearch": "Buscar:",
			         "sProcessing":"Procesando...",
                },
            //para usar los botones   
            responsive: "true",
            dom: 'Bfrtilp',    
            buttons: [  
                {
				    extend:    'excel',
				    text:      '<i class="fa fa-file-excel-o"></i> ',
				    titleAttr: 'Exportar a Excel',
				    className: 'btn btn-primary'
			    },
			    {
				    extend:    'pdf',
				    text:      '<i class="fa fa-file-pdf-o"></i> ',
				    titleAttr: 'Exportar a PDF',
				    className: 'btn btn-primary'
			    },
			    {
				    extend:    'print',
				    text:      '<i class="fa fa-print"></i> ',
				    titleAttr: 'Imprimir',
				    className: 'btn btn-primary'
			    },
            ]	 
        });     
    });

        }

</script>


<script type="text/javascript">

    function BindFunctions_Expo()
       {
       $(document).ready(function() {    
        $('#grilla_exportacion').DataTable({        
            language: {
                    "lengthMenu": "_MENU_ REGISTROS POR PAGINA",
                    "zeroRecords": "No se encontraron resultados",
                    "info": "Mostrando registros del _START_ al _END_ de un total de _TOTAL_ registros",
                    "infoEmpty": "Mostrando registros del 0 al 0 de un total de 0 registros",
                    "infoFiltered": "(filtrado de un total de _MAX_ registros)",
                    "sSearch": "Buscar:",
			         "sProcessing":"Procesando...",
                },
            //para usar los botones   
            responsive: "true",
            dom: 'Bfrtilp',    
            buttons: [  
                {
				    extend:    'excel',
				    text:      '<i class="fa fa-file-excel-o"></i> ',
				    titleAttr: 'Exportar a Excel',
				    className: 'btn btn-primary'
			    },
			    {
				    extend:    'pdf',
				    text:      '<i class="fa fa-file-pdf-o"></i> ',
				    titleAttr: 'Exportar a PDF',
				    className: 'btn btn-primary'
			    },
			    {
				    extend:    'print',
				    text:      '<i class="fa fa-print"></i> ',
				    titleAttr: 'Imprimir',
				    className: 'btn btn-primary'
			    },
            ]	 
        });     
    });

        }

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
                <li class="breadcrumb-item active" aria-current="page" id="sub_opcion" runat="server">ACTUALIZACIÓN DE EMPRESA DE TRANSPORTE</li>
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
                    
                        
                        
                     </div>
                    </asp:Panel>            
                </ContentTemplate>   
            </asp:UpdatePanel>     
        
        

    </div>

    <br />

    <div class="dashboard-container p-4" id="expo" runat="server">
        <div class="form-title">
             PASE DE PUERTA DE EXPORTACIÓN
        </div>
        
         <asp:UpdatePanel ID="UPDETALLE_EXPO" runat="server" UpdateMode="Conditional" >  
           <ContentTemplate>
                 <script type="text/javascript">
                                Sys.Application.add_load(BindFunctions_Expo); 
                </script>

                <div class="form-group">
                     <div class="alert alert-warning" id="banmsg_det" runat="server" clientidmode="Static"><b>Error!</b> >......</div>
                </div>

                     <asp:Repeater ID="grilla_exportacion" runat="server"  onitemcommand="grilla_exportacion_ItemCommand" >
                               <HeaderTemplate>
                               <table cellpadding="0" cellspacing="0" border="0"  class="table table-bordered table-sm table-contecon" id="grilla_exportacion" >
                                   <thead>
                                  <tr>
                                      <th class="nover" id="COL_TO_HIDE" runat="server">aisv</th>
                                      <th class="center hidden-phone">#</th>
                                      <th class="center hidden-phone">AISV #</th>
                                      <th class="center hidden-phone">TIPO</th>
                                      <th class="center hidden-phone">BOOKING</th>
                                      <th class="center hidden-phone">CARGA</th>
                                      <th class="center hidden-phone">ESTADO</th>
                                      <th class="center hidden-phone">CHOFER</th>
                                      <th class="center hidden-phone">PLACA</th>
                                      <th class="center hidden-phone">TURNO</th>
                                      <th class="center hidden-phone">ACCION</th>
                              
                                  </tr>
                                </thead>
                                <tbody>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr class="gradeC">
                                           <td class="nover" id="COL_TO_HIDE" runat="server"> <asp:Label Text='<%#Eval("aisv")%>' ID="LblGkey" runat="server" /> </td>
                                            <td class="center hidden-phone"> <%#Eval("item")%></td>
                                            <td class="center hidden-phone"> <%#Eval("aisv")%></td>
                                            <td class="center hidden-phone"> <%#tipos(Eval("tipo"), Eval("movi"))%></td>
                                            <td class="center hidden-phone"> <%#Eval("boking")%></td>
                                            <td class="center hidden-phone"> <%#Eval("carga") %></td>
                                            <td class="center hidden-phone"> <%#anulado(Eval("estado"))%></td>
                                            <td class="center hidden-phone"> <%#Eval("chofer")%></td>
                                            <td class="center hidden-phone"> <%#Eval("aisv_placa_vehi")%></td>
                                            <td class="center hidden-phone"> <%#Eval("turno")%></td> 
                                            <td class="center hidden-phone">  
                                                 <asp:Button ID="BtnDetalle" CommandArgument= '<%#Eval("aisv")%>' runat="server" Text="Actualizar" 
                                                    class="btn btn-primary" data-toggle="modal" data-target="#exampleModalToggleExpo" CommandName="Detalle" OnClientClick="limpiarExpo();" />
                                            </td> 
                                             
                                               
                                     </tr>    
                               </ItemTemplate>
                               <FooterTemplate>
                                </tbody>
                              </table>
                             </FooterTemplate>
                         </asp:Repeater>

          </ContentTemplate>
           </asp:UpdatePanel>   
   </div>
    <br />

      <div class="dashboard-container p-4" id="impo" runat="server">
            <div class="form-title">
             PASE DE PUERTA DE IMPORTACIÓN
            </div>
            <div class="form-group col-md-12">
           <asp:UpdatePanel ID="UPDETALLE_IMPO" runat="server" UpdateMode="Conditional" >  
           <ContentTemplate>

                 <script type="text/javascript">
                                Sys.Application.add_load(BindFunctions_Impo); 
                </script>

                 <div class="form-group col-md-12">                                                                                    
                    <div class="alert alert-warning" id="banmsg" runat="server" clientidmode="Static"><b>Error!</b> ......</div>
                </div>

              
                 <asp:Repeater ID="grilla_importacion" runat="server"  onitemcommand="grilla_importacion_ItemCommand" >
                               <HeaderTemplate>
                               <table cellpadding="0" cellspacing="0" border="0"  class="table table-bordered table-sm table-contecon" id="grilla_importacion" >
                                   <thead>
                                  <tr>
                                      <th class="nover" id="COL_TO_HIDE" runat="server">ID_PASE</th>
                                      <th class="center hidden-phone">TIPO</th>
                                      <th class="center hidden-phone"># PASE</th>
                                      <th class="center hidden-phone">CARGA</th>
                                      <th class="center hidden-phone">CONTENEDOR</th>
                                      <th class="center hidden-phone">EMPRESA TRANSP.</th>
                                      <th class="center hidden-phone">CHOFER</th>
                                      <th class="center hidden-phone">PLACA</th>
                                      <th class="center hidden-phone">TURNO</th>
                                      <th class="center hidden-phone">ACCION</th>
                              
                                  </tr>
                                </thead>
                                <tbody>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr class="gradeC">
                                           <td class="nover" id="COL_TO_HIDE" runat="server"> <asp:Label Text='<%#Eval("ID_PASE")%>' ID="LblGkey" runat="server" /> </td>
                                            <td class="center hidden-phone"> <%#Eval("TIPO_CARGA")%></td>
                                            <td class="center hidden-phone"> <%#Eval("ID_PASE")%></td>
                                            <td class="center hidden-phone"> <%#Eval("NUMERO_CARGA")%></td>
                                            <td class="center hidden-phone"> <%#Eval("CONTENEDOR")%></td>
                                            <td class="center hidden-phone"> <%#Eval("CIATRASNSP")%></td>
                                            <td class="center hidden-phone"> <%#Eval("CONDUCTOR")%></td>
                                            <td class="center hidden-phone"> <%#Eval("PLACA")%></td>
                                            <td class="center hidden-phone"> <%#Eval("TURNO")%></td> 
                                            <td class="center hidden-phone">  
                                                 <asp:Button ID="BtnDetalle" CommandArgument= '<%#Eval("ID_PASE")%>' runat="server" Text="Actualizar" 
                                                    class="btn btn-primary" data-toggle="modal" data-target="#exampleModalToggleImpo" CommandName="Detalle" OnClientClick="limpiar();" />
                                            </td> 
                                             
                                               
                                     </tr>    
                               </ItemTemplate>
                               <FooterTemplate>
                                </tbody>
                              </table>
                             </FooterTemplate>
                         </asp:Repeater>
                        

               
            </ContentTemplate>
            </asp:UpdatePanel>  
           </div>
     </div>
 

         


    <div class="modal fade" id="exampleModalToggleImpo" tabindex="-1" role="dialog" aria-hidden="true">
         <div class="modal-dialog" role="document" style="max-width: 1200px"> 
             <div class="modal-content">
                 <div class="dashboard-container p-4" id="DivImpo" runat="server">
                     
                      <div class="modal-header">
                       <asp:UpdatePanel ID="UPTITULO_TRANSP_IMPO" runat="server"  UpdateMode="Conditional" ChildrenAsTriggers="true">
                       <ContentTemplate>
                                    <h5 class="modal-title" id="H1" runat="server">ACTUALIZAR DATOS DEL TRANSPORTISTA</h5>
                           </ContentTemplate>
                        </asp:UpdatePanel>
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span> </button>
                    </div>

                     <div class="modal-body">
                        
                            <div class="form-row"> 
                                  <div class="form-group col-md-12">
                                     <label for="inputZip">Cia. Trans:<span style="color: #FF0000; font-weight: bold;"></span></label>
                                       <asp:UpdatePanel ID="EMPRESA_TRANSPORTE_IMPO" runat="server"  UpdateMode="Conditional">
                                        <ContentTemplate>
                                        <asp:TextBox ID="TxtEmpresaTransporte"  runat="server" class="form-control"  autocomplete="off" onkeypress="return soloLetras(event,'abcdefghijklmnñopqrstuvwxyz0123456789_')"
                                        disabled    ></asp:TextBox>                      
                                        <asp:HiddenField ID="IdTxtempresa" runat="server" ClientIDMode="Static"/> 
                                        </ContentTemplate> 
                                      </asp:UpdatePanel> 
                                 </div>
                                 <div class="form-group col-md-6">
                                       <label for="inputZip">Chofer:<span style="color: #FF0000; font-weight: bold;"></span></label>
                                       <asp:UpdatePanel ID="UPCHOFER_ANTERIOR" runat="server"  UpdateMode="Conditional">
                                        <ContentTemplate>
                                        <asp:TextBox ID="TxtChoferAnterior"  runat="server" class="form-control"  autocomplete="off" disabled    ></asp:TextBox>       
                                              <asp:TextBox ID="TxtPase"  runat="server" class="form-control"   autocomplete="off"  oncopy="return false;" onpaste="return false;"  
                                             Visible="false"></asp:TextBox>
                                        </ContentTemplate> 
                                      </asp:UpdatePanel> 
                                  </div>
                                <div class="form-group col-md-6">
                                       <label for="inputZip">Placa:<span style="color: #FF0000; font-weight: bold;"></span></label>
                                       <asp:UpdatePanel ID="UPPLACA_ANTERIOR" runat="server"  UpdateMode="Conditional">
                                        <ContentTemplate>
                                        <asp:TextBox ID="TxtPlacaAnterior"  runat="server" class="form-control"  autocomplete="off" disabled    ></asp:TextBox>                      
                                        </ContentTemplate> 
                                      </asp:UpdatePanel> 
                                  </div>
                                  <div class="form-group col-md-3">
                                       <label for="inputZip">Tipo Carga:<span style="color: #FF0000; font-weight: bold;">*</span></label>
                                       <asp:UpdatePanel ID="UPTIPOCARGA" runat="server"  UpdateMode="Conditional">
                                        <ContentTemplate>
                                        <asp:TextBox ID="TxtTipoCarga"  runat="server" class="form-control"  autocomplete="off" disabled    ></asp:TextBox>                      
                                        </ContentTemplate> 
                                      </asp:UpdatePanel> 
                                  </div>
                                 <div class="form-group col-md-3">
                                       <label for="inputZip"># Pase:<span style="color: #FF0000; font-weight: bold;">*</span></label>
                                       <asp:UpdatePanel ID="UPNUMEROPASE" runat="server"  UpdateMode="Conditional">
                                        <ContentTemplate>
                                        <asp:TextBox ID="TxtNumeroPase"  runat="server" class="form-control"  autocomplete="off" disabled    ></asp:TextBox>                      
                                        </ContentTemplate> 
                                      </asp:UpdatePanel> 
                                  </div>
                                 <div class="form-group col-md-3">
                                       <label for="inputZip">Contenedor:<span style="color: #FF0000; font-weight: bold;">*</span></label>
                                       <asp:UpdatePanel ID="UPCONTENEDOR" runat="server"  UpdateMode="Conditional">
                                        <ContentTemplate>
                                        <asp:TextBox ID="TxtNumeroContenedor"  runat="server" class="form-control"  autocomplete="off" disabled    ></asp:TextBox>                      
                                        </ContentTemplate> 
                                      </asp:UpdatePanel> 
                                  </div>
                                <div class="form-group col-md-3">
                                       <label for="inputZip"># Carga:<span style="color: #FF0000; font-weight: bold;">*</span></label>
                                       <asp:UpdatePanel ID="UPNUMEROCARGA" runat="server"  UpdateMode="Conditional">
                                        <ContentTemplate>
                                        <asp:TextBox ID="TxtNumeroCarga"  runat="server" class="form-control"  autocomplete="off" disabled    ></asp:TextBox>                      
                                        </ContentTemplate> 
                                      </asp:UpdatePanel> 
                                  </div>

                                 <div class="form-group col-md-12">
                                         <label for="inputZip">Chofer: (Nuevo)<span style="color: #FF0000; font-weight: bold;"></span></label>
                                        <asp:TextBox ID="TxtChofer"  runat="server" class="form-control"   autocomplete="off"  oncopy="return false;" onpaste="return false;"  
                                                        onkeypress="return soloLetras(event,'abcdefghijklmnñopqrstuvwxyz0123456789_')"></asp:TextBox>
                                                    <asp:HiddenField ID="IdTxtChofer" runat="server" ClientIDMode="Static"/>        
                                 </div>
                                <div class="form-group col-md-12">
                                        <label for="inputZip">Placa: (Nuevo)<span style="color: #FF0000; font-weight: bold;"></span></label>
                                        <asp:TextBox ID="TxtPlaca"  runat="server" class="form-control"   autocomplete="off"  oncopy="return false;" onpaste="return false;"  
                                            onkeypress="return soloLetras(event,'abcdefghijklmnñopqrstuvwxyz0123456789_')"></asp:TextBox>
                                        <asp:HiddenField ID="IdTxtPlaca" runat="server" ClientIDMode="Static"/>     
                                   
                                    </div>

                                 <div class="form-group col-md-12">
                                     
                                      <asp:UpdatePanel ID="UPDATOS_TRANSP_IMPO" runat="server"  UpdateMode="Conditional" >
                                        <ContentTemplate>
                                              
                                                <div class="row">
                                                <div class="col-md-12 d-flex justify-content-center">
                                                    <img alt="loading.." src="../lib/file-uploader/img/loading.gif" height="32px" width="32px"  id="ImgCarga" class="nover"   />
                                                </div>
                                                </div>
                                            </ContentTemplate>
                                           
                                    </asp:UpdatePanel>
                                </div>
           
                         </div>

                           <asp:UpdatePanel ID="UPDETALLE_TRANSP_IMPO" runat="server" UpdateMode="Conditional" > 
                            <ContentTemplate>
                                 <div class="row">
                                    <div class="col-md-12 d-flex justify-content-center">
                                        <p class="alert alert-light" id="sinresultado" runat="server" visible="false"></p>
                                    </div>
                                </div>
                                  </ContentTemplate>
                            </asp:UpdatePanel>
                     </div>

                      <div class="modal-footer">
                         <asp:UpdatePanel ID="UPBOTONES_TRANSP_IMPO" runat="server" UpdateMode="Conditional" >  
                         <ContentTemplate>
                             
                              <asp:Button ID="BtnAgregar" runat="server" class="btn btn-primary"   Text="Actualizar" onclick="BtnAgregar_Click"/>      
                           
                             
                        </ContentTemplate>
                         </asp:UpdatePanel>   
                           <button type="button" class="btn btn-outline-primary " data-dismiss="modal">Cerrar</button>
                      </div>
                 </div> 
             </div>
         </div>
    </div>


      <div class="modal fade" id="exampleModalToggleExpo" tabindex="-1" role="dialog" aria-hidden="true">
         <div class="modal-dialog" role="document" style="max-width: 1200px"> 
             <div class="modal-content">
                 <div class="dashboard-container p-4" id="DivExpo" runat="server">
                     
                      <div class="modal-header">
                       <asp:UpdatePanel ID="UPTITULO_TRANSP_EXPO" runat="server"  UpdateMode="Conditional" ChildrenAsTriggers="true">
                       <ContentTemplate>
                                    <h5 class="modal-title" id="H3" runat="server">ACTUALIZAR DATOS DEL TRANSPORTISTA</h5>
                           </ContentTemplate>
                        </asp:UpdatePanel>
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span> </button>
                    </div>

                     <div class="modal-body">
                        
                            <div class="form-row"> 
                                  <div class="form-group col-md-12">
                                     <label for="inputZip">Cia. Trans:<span style="color: #FF0000; font-weight: bold;"></span></label>
                                       <asp:UpdatePanel ID="EMPRESA_TRANSPORTE_EXPO" runat="server"  UpdateMode="Conditional">
                                        <ContentTemplate>
                                        <asp:TextBox ID="TxtEmpresaTransporteExpo"  runat="server" class="form-control"  autocomplete="off" 
                                        disabled    ></asp:TextBox>                      
                                        <asp:HiddenField ID="IdTxtempresaExpo" runat="server" ClientIDMode="Static"/> 
                                        </ContentTemplate> 
                                      </asp:UpdatePanel> 
                                 </div>
                                 <div class="form-group col-md-6">
                                       <label for="inputZip">Chofer:<span style="color: #FF0000; font-weight: bold;"></span></label>
                                       <asp:UpdatePanel ID="UPCHOFER_ANTERIOR_EXPO" runat="server"  UpdateMode="Conditional">
                                        <ContentTemplate>
                                        <asp:TextBox ID="TxtChoferAnteriorExpo"  runat="server" class="form-control"  autocomplete="off" disabled    ></asp:TextBox>       
                                              <asp:TextBox ID="TxtPaseExpo"  runat="server" class="form-control"   autocomplete="off"  oncopy="return false;" onpaste="return false;"  
                                             Visible="false"></asp:TextBox>
                                        </ContentTemplate> 
                                      </asp:UpdatePanel> 
                                  </div>
                                <div class="form-group col-md-6">
                                       <label for="inputZip">Placa:<span style="color: #FF0000; font-weight: bold;"></span></label>
                                       <asp:UpdatePanel ID="UPPLACA_ANTERIOR_EXPO" runat="server"  UpdateMode="Conditional">
                                        <ContentTemplate>
                                        <asp:TextBox ID="TxtPlacaAnteriorExpo"  runat="server" class="form-control"  autocomplete="off" disabled    ></asp:TextBox>                      
                                        </ContentTemplate> 
                                      </asp:UpdatePanel> 
                                  </div>
                                  <div class="form-group col-md-3">
                                       <label for="inputZip">Tipo Carga:<span style="color: #FF0000; font-weight: bold;">*</span></label>
                                       <asp:UpdatePanel ID="UPTIPOCARGA_EXPO" runat="server"  UpdateMode="Conditional">
                                        <ContentTemplate>
                                        <asp:TextBox ID="TxtTipoCargaExpo"  runat="server" class="form-control"  autocomplete="off" disabled    ></asp:TextBox>                      
                                        </ContentTemplate> 
                                      </asp:UpdatePanel> 
                                  </div>
                                 <div class="form-group col-md-3">
                                       <label for="inputZip"># Aisv:<span style="color: #FF0000; font-weight: bold;">*</span></label>
                                       <asp:UpdatePanel ID="UPNUMEROPASE_EXPO" runat="server"  UpdateMode="Conditional">
                                        <ContentTemplate>
                                        <asp:TextBox ID="TxtNumeroPaseExpo"  runat="server" class="form-control"  autocomplete="off" disabled    ></asp:TextBox>                      
                                        </ContentTemplate> 
                                      </asp:UpdatePanel> 
                                  </div>
                                 <div class="form-group col-md-3">
                                       <label for="inputZip">Booking:<span style="color: #FF0000; font-weight: bold;">*</span></label>
                                       <asp:UpdatePanel ID="UPCONTENEDOR_EXPO" runat="server"  UpdateMode="Conditional">
                                        <ContentTemplate>
                                        <asp:TextBox ID="TxtNumeroContenedorExpo"  runat="server" class="form-control"  autocomplete="off" disabled    ></asp:TextBox>                      
                                        </ContentTemplate> 
                                      </asp:UpdatePanel> 
                                  </div>
                                <div class="form-group col-md-3">
                                       <label for="inputZip"># Carga:<span style="color: #FF0000; font-weight: bold;">*</span></label>
                                       <asp:UpdatePanel ID="UPNUMEROCARGA_EXPO" runat="server"  UpdateMode="Conditional">
                                        <ContentTemplate>
                                        <asp:TextBox ID="TxtNumeroCargaExpo"  runat="server" class="form-control"  autocomplete="off" disabled    ></asp:TextBox>                      
                                        </ContentTemplate> 
                                      </asp:UpdatePanel> 
                                  </div>

                                 <div class="form-group col-md-12">
                                         <label for="inputZip">Chofer: (Nuevo)<span style="color: #FF0000; font-weight: bold;"></span></label>
                                        <asp:TextBox ID="TxtChoferExpo"  runat="server" class="form-control"   autocomplete="off"  oncopy="return false;" onpaste="return false;"  
                                                        onkeypress="return soloLetras(event,'abcdefghijklmnñopqrstuvwxyz0123456789_')"></asp:TextBox>
                                                    <asp:HiddenField ID="IdTxtChoferExpo" runat="server" ClientIDMode="Static"/>        
                                 </div>
                                <div class="form-group col-md-12">
                                        <label for="inputZip">Placa: (Nuevo)<span style="color: #FF0000; font-weight: bold;"></span></label>
                                        <asp:TextBox ID="TxtPlacaExpo"  runat="server" class="form-control"   autocomplete="off"  oncopy="return false;" onpaste="return false;"  
                                            onkeypress="return soloLetras(event,'abcdefghijklmnñopqrstuvwxyz0123456789_')"></asp:TextBox>
                                        <asp:HiddenField ID="IdTxtPlacaExpo" runat="server" ClientIDMode="Static"/>     
                                   
                                    </div>

                                 <div class="form-group col-md-12">
                                     
                                      <asp:UpdatePanel ID="UPDATOS_TRANSP_EXPO" runat="server"  UpdateMode="Conditional" >
                                        <ContentTemplate>
                                              
                                                <div class="row">
                                                <div class="col-md-12 d-flex justify-content-center">
                                                    <img alt="loading.." src="../lib/file-uploader/img/loading.gif" height="32px" width="32px"  id="ImgCargaExpo" class="nover"   />
                                                </div>
                                                </div>
                                            </ContentTemplate>
                                           
                                    </asp:UpdatePanel>
                                </div>
           
                         </div>

                           <asp:UpdatePanel ID="UPDETALLE_TRANSP_EXPO" runat="server" UpdateMode="Conditional" > 
                            <ContentTemplate>
                                 <div class="row">
                                    <div class="col-md-12 d-flex justify-content-center">
                                        <p class="alert alert-light" id="sinresultado_expo" runat="server" visible="false"></p>
                                    </div>
                                </div>
                                  </ContentTemplate>
                            </asp:UpdatePanel>
                     </div>

                      <div class="modal-footer">
                         <asp:UpdatePanel ID="UPBOTONES_TRANSP_EXPO" runat="server" UpdateMode="Conditional" >  
                         <ContentTemplate>
                             
                              <asp:Button ID="BtnAgregarExpo" runat="server" class="btn btn-primary"   Text="Actualizar" onclick="BtnAgregarExpo_Click"/>      
                           
                             
                        </ContentTemplate>
                         </asp:UpdatePanel>   
                           <button type="button" class="btn btn-outline-primary " data-dismiss="modal">Cerrar</button>
                      </div>
                 </div> 
             </div>
         </div>
    </div>
  <!--script for this page--> 



  <script type="text/javascript" src="../js/bootstrap.bundle.min.js"></script>
  <script type="text/javascript"  src="../js/datatables.js"></script>
  <script type="text/javascript" src="../lib/common-scripts.js"></script>
  <script type="text/javascript" src="../lib/pages.js" ></script>
  <script type="text/javascript" src="../lib/advanced-form-components.js"></script>

   <script type="text/javascript" src="../js/buttons/1.6.4/dataTables.buttons.min.js"></script>  
     <script type="text/javascript" src="../js/buttons/1.6.4/jszip.min.js"></script>  
     <script type="text/javascript" src="../js/buttons/1.6.4/pdfmake.min.js"></script>  
    <script type="text/javascript" src="../js/buttons/1.6.4/vfs_fonts.js"></script>  

    <script type="text/javascript" src="../js/buttons/1.6.4/buttons.print.min.js"></script>  
     <script type="text/javascript" src="../js/buttons/1.6.4/buttons.html5.min.js"></script>  
     <script type="text/javascript" src="../js/buttons/1.6.4/buttons.colVis.min.js"></script>  

     <script type="text/javascript" src='../lib/autocompletar/bootstrap3-typeahead.min.js'></script>

<script type="text/javascript"> 
    function mostrarloader(Valor) {

        try {
            if (Valor == "1") {
                document.getElementById("ImgCarga").className = 'ver';
            }
            else {
               // document.getElementById("ImgCargaDet").className='ver';
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
                document.getElementById("ImgCargaExpo").className='nover';
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


 <!--chofer impo--> 
<script type="text/javascript">

     $(function () {
        $('[id*=TxtChofer]').typeahead({
            hint: true,
            highlight: true,
            minLength: 3,
            source: function (request, response) {
               
                $.ajax({
                    url: '<%=ResolveUrl("transp_actualizapasepuerta.aspx/GetChofer") %>',
                    data: "{ 'prefix': '" + request + "', 'idempresa' : '" + $("#IdTxtempresa").val() + "' }",
                    dataType: "json",
                    type: "POST",
                    maxJsonLength: 1,
                    contentType: "application/json; charset=utf-8",
                    success: function (data) {
                        items = [];
                        map = {};
                        $.each(data.d, function (i, item) {
                            var id = item.split('+')[0];
                            var name = item.split('+')[1];
                            map[name] = { id: id, name: name };
                            items.push(name);
                        });
                        response(items);
                        $(".dropdown-menu").css("height", "auto");
                    },
                    error: function (response) {
                        alert(response.responseText);
                    },
                    failure: function (response) {
                        alert(response.responseText);
                    }
                });
            },
            updater: function (item) {
                $('[id*=IdTxtChofer]').val(map[item].id);
                return item;
            }
        });
     });

</script>
 <!--chofer expo--> 
<script type="text/javascript">

     $(function () {
        $('[id*=TxtChoferExpo]').typeahead({
            hint: true,
            highlight: true,
            minLength: 3,
            source: function (request, response) {
               
                $.ajax({
                    url: '<%=ResolveUrl("transp_actualizapasepuerta.aspx/GetChofer") %>',
                    data: "{ 'prefix': '" + request + "', 'idempresa' : '" + $("#IdTxtempresaExpo").val() + "' }",
                    dataType: "json",
                    type: "POST",
                    maxJsonLength: 1,
                    contentType: "application/json; charset=utf-8",
                    success: function (data) {
                        items = [];
                        map = {};
                        $.each(data.d, function (i, item) {
                            var id = item.split('+')[0];
                            var name = item.split('+')[1];
                            map[name] = { id: id, name: name };
                            items.push(name);
                        });
                        response(items);
                        $(".dropdown-menu").css("height", "auto");
                    },
                    error: function (response) {
                        alert(response.responseText);
                    },
                    failure: function (response) {
                        alert(response.responseText);
                    }
                });
            },
            updater: function (item) {
                $('[id*=IdTxtChoferExpo]').val(map[item].id);
                return item;
            }
        });
     });

</script>

 <!--placa impo--> 
<script type="text/javascript">

     $(function () {
        $('[id*=TxtPlaca]').typeahead({
            hint: true,
            highlight: true,
            minLength: 1,
            source: function (request, response) {
               
                $.ajax({
                    url: '<%=ResolveUrl("transp_actualizapasepuerta.aspx/GetPlaca") %>',
                    data: "{ 'prefix': '" + request + "', 'idempresa' : '" + $("#IdTxtempresa").val() + "' }",
                    dataType: "json",
                    type: "POST",
                    maxJsonLength: 1,
                    contentType: "application/json; charset=utf-8",
                    success: function (data) {
                        items = [];
                        map = {};
                        $.each(data.d, function (i, item) {
                            var id = item.split('+')[0];
                            var name = item.split('+')[1];
                            map[name] = { id: id, name: name };
                            items.push(name);
                        });
                        response(items);
                        $(".dropdown-menu").css("height", "auto");
                    },
                    error: function (response) {
                        alert(response.responseText);
                    },
                    failure: function (response) {
                        alert(response.responseText);
                    }
                });
            },
            updater: function (item) {
                $('[id*=IdTxtPlaca]').val(map[item].id);
                return item;
            }
        });
     });

</script>
 <!--placa expo--> 
<script type="text/javascript">

     $(function () {
        $('[id*=TxtPlacaExpo]').typeahead({
            hint: true,
            highlight: true,
            minLength: 1,
            source: function (request, response) {
               
                $.ajax({
                    url: '<%=ResolveUrl("transp_actualizapasepuerta.aspx/GetPlaca") %>',
                    data: "{ 'prefix': '" + request + "', 'idempresa' : '" + $("#IdTxtempresaExpo").val() + "' }",
                    dataType: "json",
                    type: "POST",
                    maxJsonLength: 1,
                    contentType: "application/json; charset=utf-8",
                    success: function (data) {
                        items = [];
                        map = {};
                        $.each(data.d, function (i, item) {
                            var id = item.split('+')[0];
                            var name = item.split('+')[1];
                            map[name] = { id: id, name: name };
                            items.push(name);
                        });
                        response(items);
                        $(".dropdown-menu").css("height", "auto");
                    },
                    error: function (response) {
                        alert(response.responseText);
                    },
                    failure: function (response) {
                        alert(response.responseText);
                    }
                });
            },
            updater: function (item) {
                $('[id*=IdTxtPlacaExpo]').val(map[item].id);
                return item;
            }
        });
     });

</script>
  
<script type="text/javascript">

     function limpiar()
    {
         document.getElementById("<%=TxtChofer.ClientID %>").value = "";
         document.getElementById("<%=TxtPlaca.ClientID %>").value = "";
       
    }

</script>

<script type="text/javascript">

     function limpiarExpo()
    {
         document.getElementById("<%=TxtChoferExpo.ClientID %>").value = "";
         document.getElementById("<%=TxtPlacaExpo.ClientID %>").value = "";
       
    }

</script>

</asp:Content>
