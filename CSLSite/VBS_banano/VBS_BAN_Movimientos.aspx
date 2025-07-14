
<%@ Page Title="Stowage Movimiento" Language="C#" MasterPageFile="~/site.Master" AutoEventWireup="true" CodeBehind="VBS_BAN_Movimientos.aspx.cs" Inherits="CSLSite.VBS_BAN_Movimientos" %>
<%@ MasterType VirtualPath="~/site.Master" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="placehead" runat="server">

    <link href="../img/favicon2.png" rel="icon"/>
    <link href="../img/icono.png" rel="apple-touch-icon"/>
    <!-- Bootstrap core CSS -->
    <link href="../css/bootstrap.min.css" rel="stylesheet"/>
    <link href="../css/dashboard.css" rel="stylesheet"/>
    <link href="../css/icons.css" rel="stylesheet"/>
    <link href="../css/style.css" rel="stylesheet"/>

    <!--external css-->
    <link href="../shared/estilo/jquery.datetimepicker.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/jquery.datetimepicker.js" type="text/javascript"></script>
    
    <link href="../lib/advanced-datatable/css/demo_page.css" rel="stylesheet" />
    <link href="../lib/advanced-datatable/css/demo_table.css" rel="stylesheet" />
    <link rel="stylesheet" href="../lib/advanced-datatable/css/DT_bootstrap2.css" />

    <script type="text/javascript">
        function fechas() {
            $(document).ready(function () {
                $('.datetimepicker').datetimepicker().datetimepicker({ lang: 'es', step: 30, format: 'm/d/Y' });
            });

            $(document).ready(function () {
                $('.datetimepickerAlt').datetimepicker().datetimepicker({ lang: 'es', closeOnDateSelect: true, timepicker: false, step: 30, format: 'm/d/Y' });
            });

            $(document).ready(function () {
                $('.datepicker').datetimepicker().datetimepicker({ lang: 'es', timepicker: false, format: 'm/d/Y', closeOnDateSelect: true, minDate: '0' });

            });

              $(document).ready(function () {
                $('.datetimepickerDate').datetimepicker().datetimepicker({ lang: 'es', step: 30, format: 'm/d/Y H:i' });
            });
        }
    </script>

    <script type="text/javascript">

        function BindFunctions()
        {
            $(document).ready(function () {

                 if (!$.fn.DataTable.isDataTable('#tablePagination')) {
                    $('#tablePagination').DataTable(
                    {

                        language: {
                            "lengthMenu": "_MENU_ REGISTROS POR PAGINA",
                            "zeroRecords": "No se encontraron resultados",
                            "info": "Mostrando registros del _START_ al _END_ de un total de _TOTAL_ registros",
                            "infoEmpty": "Mostrando registros del 0 al 0 de un total de 0 registros",
                            "infoFiltered": "(filtrado de un total de _MAX_ registros)",
                            "sSearch": "Filtrar:",
                            "sProcessing": "Procesando...",

                        },
                        //para usar los botones   
                        responsive: "true",
                        dom: 'Bfrtilp',
                        buttons: [
                            //{
                            //    extend: 'excel',
                            //    text: '<i class="fa fa-file-excel-o"></i> ',
                            //    titleAttr: 'Exportar a Excel',
                            //    className: 'btn btn-primary'
                            //},
                            //{
                            //    extend: 'pdf',
                            //    text: '<i class="fa fa-file-pdf-o"></i> ',
                            //    titleAttr: 'Exportar a PDF',
                            //    className: 'btn btn-primary',
                            //    orientation: 'landscape',
                            //    pageSize: 'LEGAL'
                            //},
                            {
                                extend: 'print',
                                text: '<i class="fa fa-print"></i> ',
                                titleAttr: 'Imprimir',
                                className: 'btn btn-primary',
                                size: 'landscape'
                            },
                        ],
                        pageLength: 25,
                        initComplete: function () {
                         
                        }


                    });
                }
            });

        }

    </script>

     
    
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="placebody" runat="server" >
 
    <asp:ToolkitScriptManager ID="tkscata" runat="server"></asp:ToolkitScriptManager>
    
    <div id="div_BrowserWindowName" style="visibility:hidden;">
        <asp:HiddenField ID="hf_BrowserWindowName" runat="server" />
    </div>

    <div class="mt-4">         
        <nav class="mt-4" aria-label="breadcrumb">
            <ol class="breadcrumb">
                <li class="breadcrumb-item" id="opcion_principal" runat="server"><a href="#">VBS Banano</a></li>
                <li class="breadcrumb-item active" aria-current="page" id="sub_opcion" runat="server">STOWAGE MOVIMIENTOS</li>
            </ol>
        </nav>
    </div>

    <div class="dashboard-container p-4" id="cuerpo" runat="server">
        <div class="form-title">
            DATOS DEL USUARIO
        </div>
        <div class="form-row" >
            <div class="form-group col-md-6"> 
                <label for="inputAddress">ESTIMADO CLIENTE:<span style="color: #FF0000; font-weight: bold;"></span></label>
                <asp:TextBox ID="Txtcliente" runat="server" class="form-control" placeholder=""  Font-Bold="true" disabled ></asp:TextBox>
            </div>

            <div class="form-group col-md-2">
                <label for="inputAddress">RUC:<span style="color: #FF0000; font-weight: bold;"></span></label>
				<asp:TextBox ID="Txtruc" runat="server" class="form-control" placeholder=""  Font-Bold="true" disabled></asp:TextBox>
            </div>

            <div class="form-group col-md-4">
                <label  for="inputAddress">EMPRESA:<span style="color: #FF0000; font-weight: bold;"></span></label>
                <asp:TextBox ID="Txtempresa" runat="server" class="form-control" placeholder=""  Font-Bold="true" disabled></asp:TextBox>
            </div>
        </div>

        <div class="form-title">
              DATOS DE NAVE
        </div>
        <asp:UpdatePanel ID="UPCAB" runat="server"  UpdateMode="Conditional" ChildrenAsTriggers="true">
            <ContentTemplate>
                 <script type="text/javascript">
                    Sys.Application.add_load(fechas); 
                </script>
                 <div id="div_Codigos" style="visibility:hidden;">
                    <asp:HiddenField ID="hdf_txtNave" runat="server" />
                    <asp:HiddenField ID="hdf_txtNaveDesc" runat="server" />
                </div>
                <div class="form-row">

                    <div class="form-group col-md-2"> 
                        <label for="inputAddress"> Referencia<span style="color: #FF0000; font-weight: bold;">*</span><span style="color: #FF0000; font-weight: bold;"></span></label>
                        <div class="d-flex"> 
                            <asp:TextBox  class="form-control" disabled  ID="txtNave" AutoPostBack="true" runat="server" MaxLength="30" onkeypress="return soloLetras(event,'abcdefghijklmnopqrstuvwxyz01234567890/')" ></asp:TextBox>
                            
                        </div>
                    </div>

                    <div class="form-group col-md-4"> 
                        <label for="inputAddress">Nombre de Nave<span style="color: #FF0000; font-weight: bold;">*</span><span style="color: #FF0000; font-weight: bold;"></span></label>
                        <div class="d-flex"> 
                            <asp:TextBox  class="form-control"  ID="txtDescripcionNave" disabled runat="server" MaxLength="50" onkeypress="return soloLetras(event,'abcdefghijklmnopqrstuvwxyz01234567890/')"></asp:TextBox>
                            &nbsp;
                            
                            <a class="btn btn-outline-primary mr-4" style='font-size:24px' target="popup" onclick="document.getElementById('<%= btnLimpiar.ClientID %>').click(),window.open('../catalogo/naves.aspx','name','width=900,height=880')">
                                    <span class='fa fa-search' style='font-size:24px'></span> </a>
                        </div>
                    </div>

                    <div class="form-group col-md-2" style="display: none;">
                        <label for="inputZip">MRN<span style="color: #FF0000; font-weight: bold;">*</span></label>
                        <asp:TextBox ID="TXTMRN" Visible ="false" runat="server" class="form-control" disabled MaxLength="16"  onkeypress="return soloLetras(event,'abcdefghijklmnopqrstuvwxyz0123456789-_')" placeholder="MRN"></asp:TextBox>
                    </div>

                    <div class="form-group   col-md-2" style="display: none;"> 
                        <label for="inputAddress"> ETA:<span style="color: #FF0000; font-weight: bold;"></span></label>

                        <div class="d-flex">
                            <asp:TextBox class="form-control" Visible ="false"  runat="server" ID="fecETA" disabled   AutoPostBack="false" MaxLength="10"  onkeypress="return soloLetras(event,'0123456789/')" ></asp:TextBox>

                            <asp:CalendarExtender ID="CAGTFECHAFASA"  runat="server"
                                CssClass="cal_Theme1" Format="dd/MM/yyyy" TargetControlID="fecETA">
                            </asp:CalendarExtender>      
                        </div>
                    </div>

                    <div class="form-group col-md-4"> 
                        <label for="inputAddress">Linea :<span style="color: #FF0000; font-weight: bold;"></span></label>
                        <div class="d-flex">
                            <asp:DropDownList ID="cmbLinea" class="form-control"  runat="server" Font-Size="Medium" AutoPostBack="true" Font-Bold="true" OnSelectedIndexChanged="cmbLinea_SelectedIndexChanged">
                            </asp:DropDownList>
                            <a class="tooltip" ><span class="classic" >Nombre de Linea</span><img alt='' src='../shared/imgs/info.gif' class='datainfo'/></a>
                        </div>
                    </div>
             
                </div>

                <div></div>

                <div class="row">
                    <div class="col-md-12 d-flex justify-content-center">
                        <asp:Button ID="btnLimpiar" runat="server" class="btn btn-outline-primary mr-2" Text="Limpiar" OnClick="btnLimpiar_Click"   />
                        <asp:Button ID="btnBuscar" runat="server" class="btn btn-primary mr-2"  Text="Buscar" OnClientClick="return mostrarloader('1')" OnClick="btnBuscar_Click"  />
                        <%--<asp:Button ID="btnExportador" runat="server" class="btn btn-primary" Text="EXPORTAR" OnClick="btnExportador_Click"  />--%>
                    </div>
                    <div class="col-md-12 d-flex justify-content-center">
                        <img alt="loading.." src="../lib/file-uploader/img/loading.gif" height="32px" width="32px"  id="ImgCarga" class="nover"   />
                        <span id="imagen"></span>
                    </div>
                </div>
            <%--   <div class="row">

                        <div class="form-group col-md-4">
                            <label for="inputZip">&nbsp;</label>
                            <div class="d-flex">
                                &nbsp;&nbsp;
                                <img alt="loading.." src="../lib/file-uploader/img/loading.gif" height="32px" width="32px" id="ImgCargas" class="nover" />
                                <asp:Button ID="btnExportador" runat="server" class="btn btn-primary" Text="EXPORTAR" OnClientClick="exportar()" />

                            </div>
                        </div>
                    </div>--%>

                 <br/>
                <div class="row">
                    <div class="col-md-12 d-flex justify-content-center">
                        <div class="alert alert-warning" id="banmsg" runat="server" clientidmode="Static"><b>Error!</b> >......</div>
                    </div>
                </div>
             </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnBuscar" />
            </Triggers>
        </asp:UpdatePanel>

         <div class="row">
              <div class="col-md-12 d-flex justify-content-center">
               <asp:Button ID="btnExportar" runat="server" class="btn btn-primary"   Text="EXPORTAR"    OnClick="btnExportador_Click" />                                   
              </div>    
         </div>      

        <asp:UpdatePanel ID="UPDETALLE" runat="server"  UpdateMode="Conditional"  >  
            <ContentTemplate>
                <script type="text/javascript">
                    Sys.Application.add_load(BindFunctions); 
                </script>


                <div class="form-group col-md-12"> 
                    <div class="form-title">DETALLE DE MOVIMIENTOS EN BODEGA</div>
                </div>

                <div class="form-group">
                    <div class="alert alert-warning" id="banmsg_det" runat="server" clientidmode="Static"><b>Error!</b> >Debe ingresar el número de la carga MRN......</div>
                </div>

        
                <section class="wrapper2">
                
                    <div id="xfinder" runat="server" visible="true" >
                        <div class="findresult" >
                            <div class="booking" >                                
                                      
                                <div class="bokindetalle" style=" width:100%; overflow:auto">

                                    <asp:Repeater ID="tablePagination" runat="server" onitemcommand ="tablePagination_ItemCommand" OnItemDataBound="tablePagination_ItemDataBound">
                                        <HeaderTemplate>
                                        <table cellpadding="0" cellspacing="0" border="0" class="table table-bordered table-sm table-contecon" id="tablePagination" width="100%">
                                            <thead style="background: #B4B4B4; color: white">
                                                <tr>
                                                    <th class="th-sm"></th>
                                                    <th class="nover">ID</th>
                                                    <th class="nover">HOLD_ID</th>
                                                    <th class="th-sm">DATE</th>
                                                    <th class="th-sm">BARCODE</th>
                                                    <th class="th-sm">SHIPPER</th>
                                                    <th class="th-sm">BRAND</th>
                                                    <th class="th-sm">TYPE</th>
                                                    <th class="th-sm">BOX</th>
                                                    <th class="th-sm">HOLD</th>
                                                    <th class="th-sm">WAREHOUSE</th>
                                                    <th class="th-sm">ESTADO</th>
                                                    <th class="th-sm">CREADO</th>
                                                    <th class="th-sm">MODIFICADO</th>
                                                    <th class="th-sm">FECHA MODIFICA</th>
                                                    <th class="th-sm">PRE-DESPACHO</th>
                                                    <th class="th-sm">FECHA PRE-DESPACHO</th>
                                                    <th class="th-sm">DESPACHADO</th>
                                                    <th class="th-sm">FECHA DESPACHO</th>
                                                    <th class="nover">IDORDEN</th>
                                                   <%-- <th class="center hidden-phone"></th>
                                                    <th class="center hidden-phone"></th>
                                                    <th class="center hidden-phone"></th>
                                                    <th class="center hidden-phone"></th>--%>
                                                </tr>
                                            </thead>
                                        <tbody>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <tr>
                                                <td class="center hidden-phone"><asp:Label Text='<%# Container.ItemIndex + 1 %>' ID="lblSecuencia" runat="server"  /></td>
                                                <td class="nover"><asp:Label Text='<%#Eval("id")%>' ID="lblId" runat="server"  /></td>
                                                <td class="nover"> <%#Eval("idHold")%></td> 
                                                <td class="center hidden-phone"><asp:Label Text='<%#Eval("fecha")%>' ID="lblFecha" runat="server"  /></td>           
                                                <td class="center hidden-phone"><asp:Label Text='<%#Eval("barcode")%>' ID="lbldeck" runat="server"  /></td>            
                                                <td class="center hidden-phone"><asp:Label Text='<%#Eval("exportador")%>' ID="lblexportador" runat="server"  /></td>      
                                                <td class="center hidden-phone"><asp:Label Text='<%#Eval("marca")%>' ID="lblmarca" runat="server"  /></td>           
                                                <td class="center hidden-phone"><asp:Label Text='<%#Eval("tipo")%>' ID="lblTipo" runat="server"  /></td>   
                                                <td class="center hidden-phone"><asp:Label Text='<%#Eval("box")%>' ID="lblbox" runat="server"  /></td>             
                                                <td class="center hidden-phone"><asp:Label Text='<%#Eval("bloque")%>' ID="lblHold" runat="server"  /></td>            
                                                <td class="center hidden-phone"><asp:Label Text='<%#Eval("bodega")%>' ID="lblBodega" runat="server"  /></td>            
                                                <td class="center hidden-phone"><asp:Label Text='<%#Eval("estado")%>' ID="lblAisv" runat="server"  /></td>   
                                                <td class="center hidden-phone"><asp:Label Text='<%#Eval("usuarioCrea")%>' ID="lblUsuarioCrea" runat="server"  /></td>   
                                                <td class="center hidden-phone"><asp:Label Text='<%#Eval("usuarioModifica")%>' ID="Label1" runat="server"  /></td>   
                                                <td class="center hidden-phone"><asp:Label Text='<%#Eval("fechaModifica")%>' ID="Label2" runat="server"  /></td>   
                                                 <td class="center hidden-phone"><asp:Label Text='<%#Eval("usuarioPreDespacho")%>' ID="Label5" runat="server"  /></td>   
                                                <td class="center hidden-phone"><asp:Label Text='<%#Eval("fechaPreDespacho")%>' ID="Label6" runat="server"  /></td>  
                                                <td class="center hidden-phone"><asp:Label Text='<%#Eval("usuarioDespacho")%>' ID="Label3" runat="server"  /></td>   
                                                <td class="center hidden-phone"><asp:Label Text='<%#Eval("fechaDespacho")%>' ID="Label7" runat="server"  /></td>   
                                                <td class="nover"><asp:Label Text='<%#Eval("idOrdenDespacho")%>' ID="Label4" runat="server"  /></td>   
                                               
                                              <%--  <td class="center hidden-phone">  
                                                    <asp:Button runat="server" ID="btnEditar" Height="30px" CommandName="Editar" Text="Editar"  CommandArgument='<%# Bind("id") %>' class="btn btn-outline-primary mr-2"  data-toggle="modal" data-target="#myModal"  />
                                                </td>
                                                <td class="center hidden-phone"> 
                                                    <asp:Button runat="server" ID="btnDetalle" Height="30px" CommandName="Aisv" Text="Turnos"  CommandArgument='<%# Bind("id") %>' class="btn btn-primary"  data-toggle="modal" data-target="#myModal1"  />
                                                </td> 
                                                <td class="center hidden-phone"> 
                                                    <asp:Button runat="server" ID="btnServicios" Height="30px" CommandName="Servicios" Text="Servicios"  CommandArgument='<%# Bind("id") %>' class="btn btn-primary"  data-toggle="modal" data-target="#myModal2"  />
                                                </td> 
                                                <td class="center hidden-phone"> 
                                                    <asp:Button runat="server" ID="btnPublicar" Height="30px" CommandName="Publicar" Text="Publicar"  CommandArgument='<%# Bind("id") %>' class="btn btn-primary"  data-toggle="modal" data-target="#myModal3"  />
                                                </td> --%>
                                            </tr>    
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            
                                        </tbody>
                                         <%--   <tr>
                                                <td colspan="2">Total:</td>
                                                    <td>
                                                        <asp:Label ID="lblTotalBoxes" Text="0" Font-Bold="true" runat="server"></asp:Label>

                                                    </td>
                                                <td colspan="6"></td>
                                            </tr>--%>
                                        </table>
                                            
                                        </FooterTemplate>
                                    </asp:Repeater>

                                 
                                </div><!--adv-table-->
                            <%--   </section>--%>
                            </div><!--content-panel-->
                        </div><!--col-lg-12-->

                         
                    </div><!--row mt-->
                     <div id="sinresultado" runat="server" class="alert alert-info">
                        No se encontraron resultados, por favor verifique los criterios de búsqueda
                    </div>

                </section><!--wrapper2-->
     
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnBuscar" />
            </Triggers>
        </asp:UpdatePanel>




    </div>

    <script type="text/javascript" src="../lib/datatables/datatables.min.js"></script>
    <script type="text/javascript" src="../lib/common-scripts.js"></script>
    <script type="text/javascript" src="../lib/pages.js" ></script>
    <script type="text/javascript" src="../lib/advanced-form-components.js"></script>

    <script src="../Scripts/pages.js" type="text/javascript"></script>
    <script src="../Scripts/credenciales.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.tablesorter.min.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.tablesorter.pager.js" type="text/javascript"></script>

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


<script type="text/javascript">

    function mostrarloader(Valor) {

        try {
            if (Valor == "1") {
                document.getElementById("ImgCarga").className = 'ver';
                //document.getElementById("ImgCargas").className = 'ver';
            }
            else {
                  document.getElementById("ImgCarga").className='ver';            
            }
                
            } catch (e) {
                alert('Lamentamos comunicarle que ha ocurrido el siguiente problema:\n' + e.Message);
            }
    }

    function ocultarloader(Valor) {
        try {

            if (Valor == "1") {
                document.getElementById("ImgCarga").className = 'nover';
               // document.getElementById("ImgCargas").className = 'nover';
            }
            else {
                document.getElementById("ImgCarga").className='nover';
            }

             } catch (e) {
                alert('Lamentamos comunicarle que ha ocurrido el siguiente problema:\n' + e.Message);
            }
    }

    function prepareObjectRuc() {
            try {
                document.getElementById("loader3").className = '';
                var vals = document.getElementById('<%=TXTMRN.ClientID %>').value;
                if (vals == null || vals == undefined || vals == '') {
                    alert('¡ Escriba el MRN.');
                    document.getElementById("loader3").className = 'nover';
                    document.getElementById('<%=TXTMRN.ClientID %>').focus();
                    return false;
                }
                return true;
            } catch (e) {
                alert('Lamentamos comunicarle que ha ocurrido el siguiente problema:\n' + e.Message);
            }
    }

    

    function popupCallback(catalogo)
    {
        if (catalogo == null || catalogo == undefined) {
            alert('Hubo un problema al setaar un objeto de catalogo');
            return;
        }

        this.document.getElementById('<%= txtNave.ClientID %>').value = catalogo.codigo;
        this.document.getElementById('<%= txtDescripcionNave.ClientID %>').value = catalogo.descripcion;

        this.document.getElementById('<%= hdf_txtNave.ClientID %>').value = catalogo.codigo; 
        this.document.getElementById('<%= hdf_txtNaveDesc.ClientID %>').value = catalogo.descripcion;
    }

</script>

    <script type="text/javascript">
   
        $(document).ready(function () {
            $('.datetimepicker').datetimepicker().datetimepicker({ lang: 'es', step: 30, format: 'm/d/Y H:i' });
        });
        $(document).ready(function () {
            $('.datetimepickerAlt').datetimepicker().datetimepicker({ lang: 'es', closeOnDateSelect: true, timepicker: false, step: 30, format: 'm/d/Y' });
        });

        $(document).ready(function () {
            $('.datepicker').datetimepicker().datetimepicker({ lang: 'es', timepicker: false, format: 'm/d/Y', closeOnDateSelect: true, minDate: '0' });

        });
    </script>
</asp:Content>