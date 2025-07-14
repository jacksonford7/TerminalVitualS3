<%@ Page Title="Usuarios" Language="C#" MasterPageFile="~/site.Master" AutoEventWireup="true" CodeBehind="appPaquetesClientes.aspx.cs" Inherits="CSLSite.appPaquetesClientes" %>

<%@ MasterType VirtualPath="~/site.Master" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="placehead" runat="server">

  <%--  <link href="../shared/estilo/tables.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/aisv-general.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/work.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/catalogos.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/info.css" rel="stylesheet" type="text/css" />
    <link href="../../shared/estilo/chosen.css" rel="stylesheet" type="text/css" />

    <link href="../shared/estilo/proforma.css" rel="stylesheet" type="text/css" />--%>

    <link href="../img/favicon2.png" rel="icon"/>
    <link href="../img/icono.png" rel="apple-touch-icon"/>
    <link href="../css/bootstrap.min.css" rel="stylesheet"/>
    <link href="../css/dashboard.css" rel="stylesheet"/>
    <link href="../css/icons.css" rel="stylesheet"/>
    <link href="../css/style.css" rel="stylesheet"/>
    <link href="https://fonts.googleapis.com/css2?family=Roboto:wght@300;400;500;700&display=swap" rel="stylesheet"/>
    
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/5.15.1/css/all.min.css" integrity="sha512-+4zCK9k+qNFUR5X+cKL9EIR+ZOhtIloNl9GIKS57V1MyNsYpYcUrUeQc9vNfzsWfV28IaLL3i96P9sdNyeRssA==" crossorigin="anonymous" />
    <link href="../css/datatables.min.css" rel="stylesheet" />
  <link rel="stylesheet" type="text/css" href="..js/buttons/css1.6.4/buttons.dataTables.min.css"/>
      <link href="../css/stc_final.css" rel="stylesheet"/>

     <link href="../shared/estilo/jquery.datetimepicker.css" rel="stylesheet" type="text/css" />
  <script src="../Scripts/jquery.datetimepicker.js" type="text/javascript"></script>

   <%-- <link href="../shared/estilo/Reset.css" rel="stylesheet" />
    <link href="../shared/estilo/jquery.datetimepicker.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/tables.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/info.css" rel="stylesheet" type="text/css" />--%>

   <%-- <script type="text/javascript">
        function BindFunctions() {
            $(document).ready(function () {
                $('#tablasort').tablesorter({ cancelSelection: true, cssAsc: "ascendente", cssDesc: "descendente", headers: { 5: { sorter: false }, 6: { sorter: false}} }).tablesorterPager({ container: $('#pager'), positionFixed: false, pagesize: 10 });
            });

            $(document).ready(function () {
                //inicia los fecha-hora

                //colapsar y expandir
                $("div.colapser").toggle(function () { $(this).removeClass("colapsa").addClass("expande"); $(this).next().hide(); }
                                  , function () { $(this).removeClass("expande").addClass("colapsa"); $(this).next().show(); });
                //poner valor en campo

            });
        }
    </script>--%>


 <script type="text/javascript">
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

     

 function BindFunctions()
 {
       $(document).ready(function () {
                 $('.datetimepicker').datetimepicker().datetimepicker({ lang: 'es', timepicker: false, step: 30, format: 'd/m/Y' });
       });    

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


   $(document).ready(function() {    
    $('#tablePagination').DataTable({        
       
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
<asp:Content ID="Content2" ContentPlaceHolderID="placebody" runat="server">
    <asp:ToolkitScriptManager ID="tkcal" EnableScriptGlobalization="true" runat="server">
    </asp:ToolkitScriptManager>
    <input id="zonaid" type="hidden" value="901" />
     <input id="ruta_completa" type="hidden" value="" runat="server" clientidmode="Static" />
    <noscript>
        <meta http-equiv="refresh" content="0; url=sinsoporte.htm" />
    </noscript>
    <asp:UpdatePanel ID="updConsultaUsuarios" runat="server">
        <ContentTemplate>
        
            <div class="mt-4">         
                <nav class="mt-4" aria-label="breadcrumb">
                    <ol class="breadcrumb">
                    <li class="breadcrumb-item" id="opcion_principal" runat="server"><a href="#">App CGSA</a></li>
                        <li class="breadcrumb-item active" aria-current="page" id="sub_opcion" runat="server">PAQUETES/CLIENTES</li>
                    </ol>
                </nav>
            </div>
            
            <div class="dashboard-container p-4" id="cuerpo" runat="server">

                <div class="form-title">
                   	Consulta de usuarios
                </div>
                <h6>Criterios de Búsqueda.</h6>

             
                    <div class="form-row" >

                        <div class="form-group   col-md-2"> 
                            <label for="inputAddress">Usuario:<span style="color: #FF0000; font-weight: bold;"></span></label>

                            <div class="d-flex">
                                <asp:TextBox ID="txtUsuario" runat="server"  CssClass="form-control" MaxLength="20" placeholder="USUARIO" onkeypress="return soloLetras(event,'abcdefghijklmnñopqrstuvwxyz1234567890_',true)"></asp:TextBox>
                                <asp:HiddenField ID="hdfIdUsuario" runat="server" />
                                <asp:HiddenField ID="hdfUserName" runat="server" />
                            </div>
                        </div>

                        <div class="form-group   col-md-2"> 
                            <label for="inputAddress">Nombre del Usuario:<span style="color: #FF0000; font-weight: bold;"></span></label>

                            <div class="d-flex">
                                <asp:TextBox ID="txtNombres" CssClass="form-control" runat="server" onkeypress="return soloLetras(event,'abcdefghijklmnñopqrstuvwxyz1234567890')" MaxLength="100" placeholder="NOMBRE"></asp:TextBox>
                            </div>
                        </div>

                        <div class="form-group   col-md-2"> 
                            <label for="inputAddress">Identificación:<span style="color: #FF0000; font-weight: bold;"></span></label>

                            <div class="d-flex">
                                <asp:TextBox CssClass="form-control" ID="txtIdentificacion" runat="server" placeholder="IDENTIFICACION" MaxLength="15" ></asp:TextBox>
                            </div>
                        </div>

                        <div class="form-group   col-md-2"> 
                            <label for="inputAddress">Empresa:<span style="color: #FF0000; font-weight: bold;"></span></label>

                            <div class="d-flex">
                                <asp:TextBox ID="txtNombreEmpresa" CssClass="form-control" runat="server" onkeypress="return soloLetras(event,'abcdefghijklmnñopqrstuvwxyz1234567890',true)" MaxLength="255" placeholder="EMPRESA"></asp:TextBox>
                            </div>
                        </div>

                        <div class="form-group   col-md-3"> 
                            <label for="inputAddress">Tipo de Usuario:<span style="color: #FF0000; font-weight: bold;"></span></label>

                            <div class="d-flex">
                                <asp:DropDownList ID="ddlTipoUsuario" CssClass="form-control" runat="server" AppendDataBoundItems="true"></asp:DropDownList>
                            </div>
                        </div>

                        <div class="form-group   col-md-1"> 
                            <label for="inputAddress">Estado:<span style="color: #FF0000; font-weight: bold;"></span></label>

                            <div class="d-flex">
                                <asp:DropDownList ID="ddlEstado" runat="server" CssClass="form-control">   
                                    <asp:ListItem Text="ACTIVO" Value="A"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>

                    </div>
            
                


                <div><br /></div>

                <div class="form-group col-md-12"> 
                        <div class="row">
                            <div class="col-md-12 d-flex justify-content-center">
                                 
                               <%-- <asp:Button ID="Button1" class="btn btn-primary" Text="Crear Usuario" runat="server" OnClick="Button1_Click" />--%>
                                &nbsp;
                                <asp:Button ID="btbuscar" class="btn btn-outline-primary mr-4" Text="Iniciar búsqueda" runat="server" OnClick="btbuscar_Click" />
                                &nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:Button ID="btSetearId" Text="Iniciar búsqueda" runat="server" Style="display: none;"
                                    OnClick="btSetearId_Click" />
                                <asp:Button ID="btResetear" Text="Iniciar búsqueda" runat="server" Style="display: none;"
                                    OnClick="btResetear_Click" />
                            </div>

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
                </div>

                   <div class="form-row"> 
                       <div class="form-group col-md-6">
                        <label for="inputEmail4">EMPRESA:</label>
                         <asp:DropDownList runat="server" ID="CboEmpresa"  class="form-control"  AppendDataBoundItems="true"  AutoPostBack="true" OnSelectedIndexChanged="CboEmpresa_SelectedIndexChanged" ></asp:DropDownList>  
                       </div>   
                      
                        <div class="form-group col-md-6">   
                             <label for="inputEmail4">PAQUETE:</label>
                             <asp:DropDownList runat="server" ID="CboPaquete"  class="form-control"  AppendDataBoundItems="true"  ></asp:DropDownList>  

                        </div>   
                        <div class="form-group col-md-6" > 
                          <label for="inputEmail4">ARCHIVO PDF DE AUTORIZACIÓN:</label>
                          <div class="d-flex">
                          <asp:TextBox ID="TxtRuta1" runat="server"   class="form-control" ClientIDMode="Static" disabled ></asp:TextBox>
                                 <a  class="btn btn-outline-primary mr-4" runat="server" id="BtnArchivos" 
                                     target ="popup"  onclick="subirpdf();"   >
                                <span class='fa fa-search' style='font-size:24px' ></span></a>
                        </div>
		           </div>
                    

                   </div>

                 <div class="form-group col-md-12"> 
                     <div class="row">
                            <div class="col-md-12 d-flex justify-content-center"> 
                                 <asp:Button ID="BtnNuevo" class="btn btn-outline-primary mr-4" Text="Nuevo" runat="server" OnClick="BtnNuevo_Click" />
                                &nbsp;
                                <asp:Button ID="BtnGrabar" class="btn btn-primary" Text="Grabar" runat="server" OnClick="BtnGrabar_Click" />
                            </div>
                        </div>
                </div>

  <%--              <div class="col-md-12 d-flex justify-content-center">
                        <asp:Button ID="btsalvar" runat="server" Text="Generar Solicitud" 
                            class="btn btn-primary" 
                       
                            OnClientClick="imprimir();" />
                        <span id="imagen"></span>
                    </div>--%>
          
                <div class="accion">
                   
                    <%--<div class="botonera">
                       
                        &nbsp;
                        
                    </div>--%>
                    <div id="error" runat="server" class="alert alert-danger" visible="false">
                    </div>
                        <div class='bs-example'> 
                            <div class='accordion' id='accordionExample2'>
                                <div class='card'> 
                                    <div class='card-header' id='heading-busq'>
                                       <h2 class='mb-0'>
                                           <button type = 'button' class='btn btn-link font-weight-bold text-red' data-toggle='collapse' data-target='#collapse_busq'><i class='fa fa-chevron-right'></i> CRITERIOS DE BÚSQUEDAS DE CLIENTES</button>
                                       </h2>
                                    </div>

                                    <div id = 'collapse_busq' class='collapse' aria-labelledby='heading-busq' data-parent='#accordionExample2'>
                                        <div class='card-body'> 
                                            <div class="form-row">
                                                 <div class="form-group col-md-6"> 
                                                   <label for="inputAddress">Generados desde el día:<span style="color: #FF0000; font-weight: bold;"></span></label>
                                                      <asp:TextBox runat="server" ID="TxtFechaDesde"   MaxLength="16" 
                                                        onkeypress="return soloLetras(event,'0123456789-')"    class="datetimepicker form-control" ></asp:TextBox>
                                                 </div>
                                                 <div class="form-group col-md-6"> 
                                                   <label for="inputAddress">Hasta:<span style="color: #FF0000; font-weight: bold;"></span></label>
                                                       <asp:TextBox ID="TxtFechaHasta" runat="server" ClientIDMode="Static"  MaxLength="15" CssClass="datetimepicker form-control" 
                                                    onkeypress="return soloLetras(event,'0123456789-')" 
                                                          ></asp:TextBox>
                                                       <span id="valdate" class="validacion"></span>
                                                </div>
                                                 <div class="form-group col-md-6"> 
                                                <label for="inputAddress">Cliente:<span style="color: #FF0000; font-weight: bold;"></span></label>
                                                   <asp:TextBox ID="TxtBusCliente" runat="server"  MaxLength="100"  class="form-control"             
                                                      onkeypress="return soloLetras(event,'01234567890abcdefghijklmnopqrstuvwxyz.',true)"></asp:TextBox>
                                                     <span id="valran" class="opcional"></span>
                                              </div>
                                                
                                                  <div class="form-group col-md-2">
                                                    <label for="inputAddress">&nbsp;</label>
                                                      <asp:Button ID="BtnBuscarCliente" runat="server" class="btn btn-primary form-control" Text="BUSCAR CLIENTES"  OnClick="BtnBuscarCliente_Click"/>
                                                 

                                                </div>
                                                <div class="form-group col-md-2"></div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <br/>
                        <asp:UpdatePanel ID="upresult" runat="server" ChildrenAsTriggers="true">
                            <ContentTemplate>
                                <script type="text/javascript">Sys.Application.add_load(BindFunctions); </script>
                                <input id="idlin" type="hidden" runat="server" clientidmode="Static" />
                                <input id="diponible" type="hidden" runat="server" clientidmode="Static" />
                                <div class="alert alert-warning" id="alerta" runat="server">
                                </div>
                                <div id="xfinder" runat="server" visible="false">
                                   
                                            
                                            <%--<div class="form-group   col-md-2"> 
                                                <div class="form-title" >Listado de Clientes:</div>
                                            </div>--%>
                                          
                                                <asp:Repeater ID="tablePagination" runat="server" onitemcommand="tablePagination_ItemCommand" onitemdatabound="tablePagination_ItemDataBound"  >
                                                    <HeaderTemplate>
                                                        <table  cellspacing="1" cellpadding="1" class="table table-bordered table-sm table-contecon" id="tablePagination" >
                                                            <thead>
                                                                <tr>
                                                                    <th style="display: none;">
                                                                        id
                                                                    </th>
                                                                    <th>
                                                                        Paquete 
                                                                    </th>
                                                                    <th>
                                                                        Identificación
                                                                    </th>
                                                                    <th>
                                                                        Cliente
                                                                    </th>
                                                                    <th>
                                                                        Creado Por
                                                                    </th>
                                                                   
                                                                    <th>
                                                                        Fecha Creación
                                                                    </th>
                                                                   <th>
                                                                        Archivo
                                                                    </th>
                                                                    <th>
                                                                        Comentario
                                                                    </th>
                                                                     <th>
                                                                    </th>
                                                                    <%--<th>
                                                                    </th>
                                                                     <th>
                                                                    </th>--%>
                                                                </tr>
                                                            </thead>
                                                            <tbody>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <tr class="point"  id="trUsuario">
                                                            <td style="display: none;">
                                                                <%#Eval("Id")%>
                                                            </td>
                                                            <td>
                                                                <%#Eval("Name")%>
                                                            </td>
                                                            <td><asp:Label Text='<%#Eval("ClientId")%>' ID="LblClientId" runat="server"  />
     
                                                            </td>
                                                            <td>
                                                                <%#Eval("Client")%>
                                                            </td>
                                                            <td>
                                                                <%#Eval("Create_user")%>
                                                            </td>
                                                            <td>
                                                                <%#Eval("Create_user")%>
                                                            </td>
                                                            <td>
                                                                <%#Eval("file_pdf")%>
                                                            </td>

                                                            <td ><asp:TextBox ID="Txtcomentario" runat="server" class="form-control"  ForeColor="Red" ToolTip='<%#Eval("Comment")%>' Text='<%#Eval("Comment")%>' 
                                                                onkeypress="return soloLetras(event,'abcdefghijklnmñopqrstuvwxyz01234567890-_/ ',true)" MaxLength="120"></asp:TextBox>
                                                            </td>

                                                            <td>
                                                                <div class="tcomand">
                                                                      <asp:Button ID="BtnEvento" CommandArgument= '<%#Eval("Id")%>' runat="server" Text="Eliminar" 
                                                                            OnClientClick="return confirm('Está seguro de inactivar el paquete?');"
                                      
                                                                            class="btn btn-primary" ToolTip="Inactivar un paquete a empresa" CommandName="Eliminar"
                                                                              />

                                                                  
                                                                </div>
                                                            </td>

                                                        </tr>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        </tbody> </table>
                                                    </FooterTemplate>
                                                </asp:Repeater>
                                        
                                           
                                    
                                </div>
                                <div id="sinresultado" runat="server" class="alert alert-info">
                                </div>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btbuscar" />
                            </Triggers>
                        </asp:UpdatePanel>
                   
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

      <script type="text/javascript" src="../js/bootstrap.bundle.min.js"></script>
    <script type="text/javascript"  src="../js/datatables.js"></script>

       <script type="text/javascript" src="../js/buttons/1.6.4/dataTables.buttons.min.js"></script>  
     <script type="text/javascript" src="../js/buttons/1.6.4/jszip.min.js"></script>  
     <script type="text/javascript" src="../js/buttons/1.6.4/pdfmake.min.js"></script>  
    <script type="text/javascript" src="../js/buttons/1.6.4/vfs_fonts.js"></script>  

    <script type="text/javascript" src="../js/buttons/1.6.4/buttons.print.min.js"></script>  
     <script type="text/javascript" src="../js/buttons/1.6.4/buttons.html5.min.js"></script>  
     <script type="text/javascript" src="../js/buttons/1.6.4/buttons.colVis.min.js"></script>  

     <script src="../Scripts/pages.js" type="text/javascript"></script>
     
  

  
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


    </script>

    <script type="text/javascript">
            $(document).ready(function () {
                 $('.datetimepicker').datetimepicker().datetimepicker({ lang: 'es', timepicker: false, step: 30, format: 'd/m/Y' });
              });    
      </script>

     <script type="text/javascript">
            function subirpdf()
            {
               try {
                    var w = window.open('../appcgsa/appSubirArchivo.aspx', 'Archivos', 'width=850,height=400');
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
