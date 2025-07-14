<%@ Page Title="" Language="C#" MasterPageFile="~/site.Master" AutoEventWireup="true" CodeBehind="UNIT_Consulta_Carga.aspx.cs" Inherits="CSLSite.unit.UNIT_Consulta_Carga" %>

<%@ MasterType VirtualPath="~/site.Master" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content3" ContentPlaceHolderID="placehead" runat="server">

    <link href="../img/favicon2.png" rel="icon" />
    <link href="../img/icono.png" rel="apple-touch-icon" />
    <!-- Bootstrap core CSS -->
    <link href="../css/bootstrap.min.css" rel="stylesheet" />
    <link href="../css/dashboard.css" rel="stylesheet" />
    <link href="../css/icons.css" rel="stylesheet" />
    <link href="../css/style.css" rel="stylesheet" />

    <!--external css-->
    <link href="../shared/estilo/jquery.datetimepicker.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/jquery.datetimepicker.js" type="text/javascript"></script>
    <script src="../js/sweetAlert.js" type="text/javascript"></script>
    <%--    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11" type="text/javascript"></script>--%>
    <link rel="stylesheet" href="https://cdn.datatables.net/1.13.6/css/jquery.dataTables.min.css" />

    <link href="../css/sweetalert2.min.css" rel="stylesheet" />
    <link href="../lib/advanced-datatable/css/demo_page.css" rel="stylesheet" />
    <link href="../lib/advanced-datatable/css/demo_table.css" rel="stylesheet" />
    <link rel="stylesheet" href="../lib/advanced-datatable/css/DT_bootstrap2.css" />


    <script type="text/javascript" src="../lib/datatables/datatables.min.js"></script>
    <script type="text/javascript" src="../lib/common-scripts.js"></script>
    <script type="text/javascript" src="../lib/pages.js"></script>
    <script type="text/javascript" src="../lib/advanced-form-components.js"></script>

    <script src="../Scripts/pages.js" type="text/javascript"></script>
    <script src="../Scripts/credenciales.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.tablesorter.min.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.tablesorter.pager.js" type="text/javascript"></script>


    <script type="text/javascript" src="../js/bootstrap.bundle.min.js"></script>
    <script type="text/javascript" src="../js/datatables.js"></script>
    <script type="text/javascript" src="../lib/common-scripts.js"></script>
    <script type="text/javascript" src="../lib/pages.js"></script>
    <script type="text/javascript" src="../lib/advanced-form-components.js"></script>
    <link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.5.0/css/all.min.css" rel="stylesheet" />


    <script type="text/javascript">

        function toggleFields() {
            var ddl = document.getElementById('<%= ddlOpciones.ClientID %>');
            if (!ddl) return;

            var selectedValue = ddl.value;

            var mrnField = document.getElementById('<%= TXTMRN.ClientID %>')?.parentNode;
            var msnField = document.getElementById('<%= TXTMSN.ClientID %>')?.parentNode;
            var hsnField = document.getElementById('<%= TXTHSN.ClientID %>')?.parentNode;
            var bookingField = document.getElementById('<%= TXTBooking.ClientID %>')?.parentNode;
            var containerField = document.getElementById('<%= txtcontainer.ClientID %>')?.parentNode;
            var fecdesde = document.getElementById('<%= fecdesde.ClientID %>')?.parentNode;
            var fechasta = document.getElementById('<%= fechasta.ClientID %>')?.parentNode;
            var btnBuscar = document.getElementById('divBtnBuscar');

            if (mrnField && msnField && hsnField && bookingField) {
                if (selectedValue === "Book") {
                    mrnField.style.display = "none";
                    msnField.style.display = "none";
                    hsnField.style.display = "none";
                    fecdesde.style.display = "none";
                    fechasta.style.display = "none";
                    bookingField.style.display = "block";

                    containerField.parentNode.insertBefore(btnBuscar, containerField.nextSibling);

                } else if (selectedValue === "NC") {
                    mrnField.style.display = "block";
                    msnField.style.display = "block";
                    hsnField.style.display = "block";
                    fecdesde.style.display = "none";
                    fechasta.style.display = "none";
                    bookingField.style.display = "none";
                    divFiltroGdvCargaSuelta.style.display = "none";
                    btnExportarGdvCargaSuelta.style.display = "none";

                    var secondRow = document.querySelector(".form-row:nth-child(2)");
                    secondRow.appendChild(btnBuscar);
                } else {
                    mrnField.style.display = "block";
                    msnField.style.display = "block";
                    hsnField.style.display = "block";
                    fecdesde.style.display = "block";
                    fechasta.style.display = "block";
                    bookingField.style.display = "none";

                    var secondRow = document.querySelector(".form-row:nth-child(2)");
                    secondRow.appendChild(btnBuscar);
                }
            }

            if (containerField) {
                containerField.value = "";
            }
        }




        function filtrarTabla(gridId, filtroTexto) {
            const tabla = document.getElementById(gridId);
            if (!tabla) return;

            const texto = filtroTexto.toLowerCase();
            const filas = tabla.getElementsByTagName("tr");

            for (let i = 1; i < filas.length; i++) {
                const celdas = filas[i].getElementsByTagName("td");
                let coincide = false;

                for (let j = 0; j < celdas.length; j++) {
                    const celdaTexto = celdas[j].innerText.toLowerCase();
                    if (celdaTexto.includes(texto)) {
                        coincide = true;
                        break;
                    }
                }

                filas[i].style.display = coincide ? "" : "none";
            }
        }


        $(document).ready(function () {
            $('#gbContainer').DataTable({
                paging: true,
                searching: true,
                ordering: true,
                info: true,
                language: {
                    url: '//cdn.datatables.net/plug-ins/1.13.6/i18n/es-ES.json'
                }
            });
        });

        document.addEventListener("DOMContentLoaded", function () {
            toggleFields();
        });

        function mostrarError(mensaje) {
            Swal.close();
            setTimeout(() => {
                Swal.fire({
                    title: "Error",
                    text: mensaje,
                    icon: "error",
                    iconColor: "#E23B1B",
                    confirmButtonText: "Aceptar",
                    confirmButtonColor: "#E23B1B"
                });
            }, 300); // 🔹 Espera 300ms para asegurar que el Loader desaparezca antes del error
        }
        function mostrarLoaderSwal() {
            Swal.fire({
                title: 'Cargando...',
                text: 'Por favor, espera un momento',
                allowOutsideClick: false,
                showConfirmButton: false,
                didOpen: () => {
                    Swal.showLoading();
                }
            });
        }

        function ocultarLoaderSwal() {
            Swal.close();
        }


    </script>
    <style type="text/css">
        #divContainer {
            width: 100%;
        }

        #txtcontainer {
            width: 100%;
        }

        .uppercase {
            text-transform: uppercase;
        }

        table.aspNetPager td span {
            color: white !important;
            background-color: #e23b1b !important;
        }

        /* Solo aplica el estilo en clases específicas si lo necesitas */
        .alert-danger span {
            color: white !important;
            background-color: #e23b1b !important;
        }

        .aspNetPager td span {
            background-color: #e23b1b !important;
            color: white !important;
            padding: 4px 8px;
            border-radius: 3px;
            font-weight: bold;
        }
    </style>



</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="placebody" runat="server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></asp:ToolkitScriptManager>
    <div id="div_BrowserWindowName" style="visibility: hidden;">
        <asp:HiddenField ID="HiddenField1" runat="server" />
        <asp:HiddenField ID="hf_BrowserWindowName" runat="server" />
    </div>




    <asp:UpdatePanel ID="UPCARGA" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
        <ContentTemplate>
            <div id="divTituloConsulta" runat="server" class="form-title">
                CONSULTA DE CARGA
            </div>
            <div class="form-row">
                <!-- Primera Fila -->
                <div class="form-group col-md-2">
                    <label for="inputZip">Acciones</label>
                    <asp:DropDownList ID="ddlOpciones" runat="server" CssClass="form-control" AutoPostBack="True"
                        OnSelectedIndexChanged="ddlOpciones_SelectedIndexChanged" onchange="toggleFields()">
                        <asp:ListItem Value="Cntr">Contenedor</asp:ListItem>
                        <asp:ListItem Value="Book">Booking</asp:ListItem>
                        <asp:ListItem Value="BL">No. B/L</asp:ListItem>
                        <asp:ListItem Value="NC">No. Carga</asp:ListItem>
                    </asp:DropDownList>
                </div>

                <div class="form-group col-md-2" id="divMRN" runat="server">
                    <label for="TXTMRN" title="# Manifiesto Carga">MRN<span style="color: #FF0000; font-weight: bold;"></span></label>
                    <asp:TextBox ID="TXTMRN" runat="server" CssClass="form-control uppercase"></asp:TextBox>
                </div>

                <div class="form-group col-md-1" id="divMSN" runat="server">
                    <label for="TXTMSN" title="# Secuencial Máster">MSN<span style="color: #FF0000; font-weight: bold;"></span></label>
                    <asp:TextBox ID="TXTMSN" runat="server" CssClass="form-control uppercase"></asp:TextBox>
                </div>

                <div class="form-group col-md-1" id="divHSN" runat="server">
                    <label for="TXTHSN" title="# Secuencial House">HSN<span style="color: #FF0000; font-weight: bold;"></span></label>
                    <asp:TextBox ID="TXTHSN" runat="server" CssClass="form-control uppercase"></asp:TextBox>
                </div>


                <div class="form-group col-md-2" id="divBooking" runat="server" visible="false">
                    <label for="inputZip">Booking</label>
                    <asp:TextBox ID="TXTBooking" runat="server" CssClass="form-control uppercase"></asp:TextBox>
                </div>

                <div id="divContainer" runat="server" class="form-group col-md-2">
                    <label id="lblCntr" runat="server" for="txtcontainer">CNTR</label>
                    <asp:TextBox ID="txtcontainer" runat="server" CssClass="form-control uppercase" MaxLength="20"></asp:TextBox>
                </div>


                <div id="divBtnBuscar" runat="server" class="form-group col-md-2">
                    <label for="inputZip">&nbsp;</label>
                    <div class="d-flex">
                        <asp:Button ID="BtnBuscar" runat="server" CssClass="btn btn-primary" Text="BUSCAR"
                            OnClientClick="mostrarLoaderSwal();" OnClick="BtnBuscar_Click" />

                    </div>
                </div>
            </div>

            <!-- Segunda Fila -->
            <div class="form-row" id="divSecondRow">
                <div class="form-group col-md-2" id="divFecIngreso" runat="server" style="display: none;">
                    <label for="inputAddress">Fecha Desde (Ingreso)<span style="color: #FF0000; font-weight: bold;"></span></label>
                    <div class="d-flex">
                        <asp:TextBox CssClass="form-control" runat="server" ID="fecdesde" AutoPostBack="false" MaxLength="10"></asp:TextBox>
                        <asp:CalendarExtender ID="FEECHADESDE" runat="server"
                            CssClass="cal_Theme1" Format="dd/MM/yyyy" TargetControlID="fecdesde">
                        </asp:CalendarExtender>
                    </div>
                </div>

                <div class="form-group col-md-2" id="divFecHasta" runat="server" style="display: none;">
                    <label for="inputAddress">Fecha Hasta (Ingreso)<span style="color: #FF0000; font-weight: bold;"></span></label>
                    <div class="d-flex">
                        <asp:TextBox CssClass="form-control" runat="server" ID="fechasta" AutoPostBack="false" MaxLength="10"></asp:TextBox>
                        <asp:CalendarExtender ID="CalendarExtender1" runat="server"
                            CssClass="cal_Theme1" Format="dd/MM/yyyy" TargetControlID="fechasta">
                        </asp:CalendarExtender>
                    </div>
                </div>
            </div>


            <br />
            <div class="row">
                <div class="col-md-12 d-flex justify-content-center">
                    <div class="alert alert-danger" id="banmsg" runat="server" clientidmode="Static">
                        <b>Error!</b> Debe ingresar el número de la carga MRN...
                    </div>
                </div>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="BtnBuscar" />
        </Triggers>
    </asp:UpdatePanel>



    <br />

    <div class="row">
        <div class="col-md-12">
            <asp:UpdatePanel ID="UPDETALLE" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <!-- Filtro para gbContainer -->
                    <div id="divFiltroGbContainer" runat="server" visible="false" class="form-group mb-2">
                        <div class="d-flex justify-content-between align-items-center">
                            <div class="d-flex w-75">
                                <input type="text" id="txtGvContainer" class="form-control me-2"
                                    placeholder="Buscar..." onkeyup="filtrarTabla('gbContainer', this.value)" />
                                <asp:DropDownList ID="ddlPageSizeCntr" runat="server" AutoPostBack="true"
                                    OnSelectedIndexChanged="ddlPageSizeCntr_SelectedIndexChanged"
                                    CssClass="form-control" Width="100px">
                                    <asp:ListItem Text="10" Value="10" />
                                    <asp:ListItem Text="25" Value="25" />
                                    <asp:ListItem Text="50" Value="50" />
                                    <asp:ListItem Text="100" Value="100" />
                                </asp:DropDownList>
                            </div>
                            <asp:Label ID="lblResumenCntr" runat="server" CssClass="ml-2" Font-Size="Small" />
                            <div class="d-flex gap-2">
                                <asp:LinkButton ID="btnExportarGbContainerExcel" runat="server" CssClass="btn btn-outline-success btn-sm"
                                    OnClick="btnExportarGbContainer_Click" ToolTip="Exportar a Excel">
        <i class="fas fa-file-excel"></i>
                                </asp:LinkButton>
                                <asp:LinkButton ID="btnExportarGbContainerPDF" runat="server" CssClass="btn btn-outline-danger btn-sm"
                                    OnClick="btnExportarGbContainerPDF_Click" ToolTip="Exportar a PDF">
        <i class="fas fa-file-pdf"></i>
                                </asp:LinkButton>
                                <asp:LinkButton ID="btnExportarGbContainerXML" runat="server" CssClass="btn btn-outline-primary btn-sm"
                                    OnClick="btnExportarGbContainerXML_Click" ToolTip="Exportar a XML">
        <i class="fas fa-file-code"></i>
                                </asp:LinkButton>
                            </div>

                        </div>
                    </div>

                    <div id="dvGbContainer" runat="server">
                        <asp:GridView ID="gbContainer" runat="server"
                            ClientIDMode="Static"
                            AutoGenerateColumns="False"
                            DataKeyNames="CONTENEDOR"
                            OnRowDataBound="gbContainer_RowDataBound"
                            OnPageIndexChanging="gbContainer_PageIndexChanging"
                            AllowPaging="True"
                            PageSize="10"
                            CellPadding="4"
                            ForeColor="#333333"
                            CssClass="table table-bordered invoice"
                            Font-Size="Smaller"
                            AllowSorting="True"
                            PagerStyle-CssClass="aspNetPager"
                            OnSorting="GridView_SortingCntr"
                            OnDataBound="GridView_DataBound">
                            <AlternatingRowStyle BackColor="White" />
                            <Columns>
                                <asp:BoundField DataField="CONTENEDOR" Visible="False" />
                                <asp:TemplateField HeaderText="Contenedor" SortExpression="CONTENEDOR">
                                    <ItemTemplate>
                                        <asp:HyperLink ID="lnkContenedor" runat="server"
                                            NavigateUrl='<%# Eval("CONTENEDOR") %>'
                                            Text='<%# Eval("CONTENEDOR") %>'
                                            Style="color: blue; font-weight: bold; text-decoration: underline;" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="CAS" HeaderText="Vigencia De Cas" SortExpression="CAS" />

                                <asp:BoundField DataField="Category" HeaderText="Categoría" SortExpression="Category" />
                                <asp:TemplateField HeaderText="Estado" SortExpression="TIPO_STATE">
                                    <ItemTemplate>
                                        <asp:Label ID="lblVSTATE" runat="server" Text='<%# Eval("TIPO_STATE") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="LINE_OP" HeaderText="Naviera" SortExpression="LINE_OP" />
                                <asp:BoundField DataField="IB_ACTUAL_VISIT" HeaderText="Referencia Nave" SortExpression="IB_ACTUAL_VISIT" />
                                <asp:BoundField DataField="NDOCUMENTO" HeaderText="N. Autorización de Aduana" SortExpression="NDOCUMENTO" />
                                <asp:BoundField DataField="FRGHT_KIND" HeaderText="Tipo De Carga" SortExpression="FRGHT_KIND" />
                            </Columns>
                            <HeaderStyle BackColor="#B4B4B4" Font-Bold="True" ForeColor="White" BorderStyle="Groove" BorderWidth="1px" />
                            <RowStyle Font-Size="Small" HorizontalAlign="Left" VerticalAlign="Top" />
                        </asp:GridView>
                    </div>

                    <!-- Filtro para gdvBooking -->
                    <div id="divFiltroGdvBooking" runat="server" visible="false" class="form-group mb-2">
                        <div class="d-flex justify-content-between align-items-center">
                            <div class="d-flex w-75">
                                <input type="text" id="txtGvBooking" class="form-control me-2"
                                    placeholder="Buscar..." onkeyup="filtrarTabla('gdvBooking', this.value)" />
                                <asp:DropDownList ID="ddlPageSizeBooking" runat="server" AutoPostBack="true"
                                    OnSelectedIndexChanged="ddlPageSizeBooking_SelectedIndexChanged"
                                    CssClass="form-control" Width="100px">
                                    <asp:ListItem Text="10" Value="10" />
                                    <asp:ListItem Text="25" Value="25" />
                                    <asp:ListItem Text="50" Value="50" />
                                    <asp:ListItem Text="100" Value="100" />
                                </asp:DropDownList>
                            </div>
                            <asp:Label ID="lblResumenBooking" runat="server" CssClass="ml-2" Font-Size="Small" />

                            <div class="d-flex gap-2">
                                <asp:LinkButton ID="btnExportarGdvBooking" runat="server" CssClass="btn btn-outline-success btn-sm"
                                    OnClick="btnExportarGdvBooking_Click" ToolTip="Exportar a Excel">
        <i class="fas fa-file-excel"></i>
                                </asp:LinkButton>
                                <asp:LinkButton ID="btnExportarGdvBookingPDF" runat="server" CssClass="btn btn-outline-danger btn-sm"
                                    OnClick="btnExportarGdvBookingPDF_Click" ToolTip="Exportar a PDF">
        <i class="fas fa-file-pdf"></i>
                                </asp:LinkButton>
                                <asp:LinkButton ID="btnExportarGbBookingXML" runat="server" CssClass="btn btn-outline-primary btn-sm"
                                    OnClick="btnExportarGbBookingXML_Click" ToolTip="Exportar a XML">
        <i class="fas fa-file-code"></i>
                                </asp:LinkButton>
                            </div>



                        </div>
                    </div>

                    <div id="dvGBooking" runat="server">
                        <asp:GridView ID="gdvBooking" runat="server"
                            ClientIDMode="Static"
                            AutoGenerateColumns="False"
                            DataKeyNames="CNTR_CONTAINER"
                            AllowPaging="True"
                            PageSize="10"
                            Font-Size="Smaller"
                            CellPadding="4"
                            ForeColor="#333333"
                            CssClass="table table-bordered invoice"
                            OnRowDataBound="gdvBooking_RowDataBound"
                            OnPageIndexChanging="gdvBooking_PageIndexChanging"
                            AllowSorting="True"
                            PagerStyle-CssClass="aspNetPager"
                            OnSorting="GridView_Sorting"
                            OnDataBound="GridView_DataBound">
                            <AlternatingRowStyle BackColor="White" />
                            <Columns>
                                <asp:TemplateField HeaderText="Contenedor" SortExpression="CNTR_CONTAINER">
                                    <ItemTemplate>
                                        <asp:HyperLink ID="lnkCntrBooking" runat="server"
                                            NavigateUrl='<%# Eval("CNTR_CONTAINER") %>'
                                            Text='<%# Eval("CNTR_CONTAINER") %>'
                                            Style="color: blue; font-weight: bold; text-decoration: underline;" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="CNTR_TYPE" HeaderText="Tráfico" SortExpression="CNTR_TYPE" />
                                <asp:TemplateField HeaderText="Estado" SortExpression="CNTR_YARD_STATUS">
                                    <ItemTemplate>
                                        <asp:Label ID="lblEstadoBooking" runat="server" Text='<%# Eval("CNTR_YARD_STATUS") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="CNTR_CLNT_CUSTOMER_LINE" HeaderText="Naviera" SortExpression="CNTR_CLNT_CUSTOMER_LINE" />
                                <asp:BoundField DataField="CNTR_BKNG_BOOKING" HeaderText="Booking" SortExpression="CNTR_BKNG_BOOKING" />
                                <asp:BoundField DataField="CNTR_AISV" HeaderText="# AISV" SortExpression="CNTR_AISV" />
                                <asp:TemplateField HeaderText="Temperatura" SortExpression="TemperaturaTexto">
                                    <ItemTemplate>
                                        <%# Eval("TemperaturaTexto") %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="CNTR_LCL_FCL" HeaderText="Tipo de Carga" SortExpression="CNTR_LCL_FCL" />
                                <asp:BoundField DataField="WEIGHT" HeaderText="Peso" SortExpression="WEIGHT" />
                                <asp:BoundField DataField="CNTR_PERMANENCIA" HeaderText="Tiempo de permanencia" SortExpression="WEIGHT" />
                            </Columns>
                            <HeaderStyle BackColor="#B4B4B4" Font-Bold="True" ForeColor="White" BorderStyle="Groove" BorderWidth="1px" />
                            <RowStyle Font-Size="Small" HorizontalAlign="Left" VerticalAlign="Top" />
                        </asp:GridView>
                    </div>


                    <!-- Filtro para gdvCargaSuelta -->
                    <div id="divFiltroGdvCargaSuelta" runat="server" visible="false" class="form-group mb-2">
                        <div class="d-flex justify-content-between align-items-center">
                            <div class="d-flex w-75">
                                <input type="text" id="txtGvCargaSuelta" class="form-control me-2"
                                    placeholder="Buscar..." onkeyup="filtrarTabla('gdvCargaSuelta', this.value)" />
                                <asp:Button ID="btnGenerarProforma" runat="server" Text="Generar Proforma"
                                    CssClass="btn btn-primary ms-2"
                                    OnClick="btnGenerarProforma_Click" />

                                <asp:DropDownList ID="ddlPageSizeCargaSuelta" runat="server" AutoPostBack="true"
                                    OnSelectedIndexChanged="ddlPageSizeCargaSuelta_SelectedIndexChanged"
                                    CssClass="form-control" Width="100px">
                                    <asp:ListItem Text="10" Value="10" />
                                    <asp:ListItem Text="25" Value="25" />
                                    <asp:ListItem Text="50" Value="50" />
                                    <asp:ListItem Text="100" Value="100" />
                                </asp:DropDownList>
                            </div>
                            <asp:Label ID="lblResumenCargaSuelta" runat="server" CssClass="ml-2" Font-Size="Small" />

                            <div class="d-flex gap-2">
                                <asp:LinkButton ID="btnExportarGdvCargaSuelta" runat="server" CssClass="btn btn-outline-success btn-sm"
                                    OnClick="btnExportarGdvCargaSuelta_Click" ToolTip="Exportar a Excel">
        <i class="fas fa-file-excel"></i>
                                </asp:LinkButton>
                                <asp:LinkButton ID="btnExportarGdvCargaSueltaPDF" runat="server" CssClass="btn btn-outline-danger btn-sm"
                                    OnClick="btnExportarGdvCargaSueltaPDF_Click" ToolTip="Exportar a PDF">
        <i class="fas fa-file-pdf"></i>
                                </asp:LinkButton>
                                <asp:LinkButton ID="btnExportarGdvCargaSueltaXML" runat="server" CssClass="btn btn-outline-primary btn-sm"
                                    OnClick="btnExportarGdvCargaSueltaXML_Click" ToolTip="Exportar a XML">
        <i class="fas fa-file-code"></i>
                                </asp:LinkButton>
                            </div>


                        </div>
                    </div>
                    <div id="dvGCargaSuelta" runat="server">
                        <asp:GridView ID="gdvCargaSuelta" runat="server"
                            ClientIDMode="Static"
                            AutoGenerateColumns="False"
                            DataKeyNames="DAI"
                            AllowPaging="True"
                            PageSize="10"
                            Font-Size="Smaller"
                            CellPadding="4"
                            ForeColor="#333333"
                            CssClass="table table-bordered invoice"
                            OnRowDataBound="gdvCargaSuelta_RowDataBound"
                            OnPageIndexChanging="gdvCargaSuelta_PageIndexChanging"
                            AllowSorting="True"
                            PagerStyle-CssClass="aspNetPager"
                            OnSorting="GridView_SortingCargaSuelta"
                            OnDataBound="GridView_DataBound">
                            <AlternatingRowStyle BackColor="White" />
                            <Columns>
                                <asp:BoundField DataField="LINEA" HeaderText="*LÍNEA" SortExpression="LINEA" />
                                <asp:BoundField DataField="NAVE" HeaderText="NAVE" SortExpression="NAVE" />
                                <asp:TemplateField HeaderText="DAI" SortExpression="DAI">
                                    <ItemTemplate>
                                        <asp:HyperLink ID="lnkDai" runat="server"
                                            NavigateUrl='<%# Eval("DAI", "UNIT_Consulta_CargaSuelta.aspx?dai={0}") %>'
                                            Text='<%# Eval("DAI") %>'
                                            Style="color: blue; font-weight: bold; text-decoration: underline;" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="BULTOS" HeaderText="# BULTOS O ITEMS" SortExpression="BULTOS" />
                                <asp:BoundField DataField="ESTADO" HeaderText="ESTADO" SortExpression="ESTADO" />
                                <asp:BoundField DataField="FECHAINGRESO" HeaderText="Fecha de Ingreso"
                                    DataFormatString="{0:dd/MM/yyyy}" HtmlEncode="False" SortExpression="FECHAINGRESO" />
                                <asp:BoundField DataField="FECHADESCONSOLIDA" HeaderText="Fecha de Desconsolidación"
                                    DataFormatString="{0:dd/MM/yyyy}" HtmlEncode="False" SortExpression="FECHADESCONSOLIDA" />
                                <asp:BoundField DataField="FECHADESPACHO" HeaderText="Fecha de Despacho"
                                    DataFormatString="{0:dd/MM/yyyy}" HtmlEncode="False" SortExpression="FECHADESPACHO" />
                            </Columns>

                            <HeaderStyle BackColor="#B4B4B4" Font-Bold="True" ForeColor="White" BorderStyle="Groove" BorderWidth="1px" />
                            <RowStyle Font-Size="Small" HorizontalAlign="Left" VerticalAlign="Top" />
                        </asp:GridView>
                    </div>

                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="btnExportarGbContainerExcel" />
                    <asp:PostBackTrigger ControlID="btnExportarGbContainerPDF" />
                    <asp:PostBackTrigger ControlID="btnExportarGbContainerXML" />
                    <asp:PostBackTrigger ControlID="btnExportarGdvBooking" />
                    <asp:PostBackTrigger ControlID="btnExportarGdvBookingPDF" />
                    <asp:PostBackTrigger ControlID="btnExportarGbBookingXML" />
                    <asp:PostBackTrigger ControlID="btnExportarGdvCargaSuelta" />
                    <asp:PostBackTrigger ControlID="btnExportarGdvCargaSueltaPDF" />
                    <asp:PostBackTrigger ControlID="btnExportarGdvCargaSueltaXML" />
                    <asp:AsyncPostBackTrigger ControlID="ddlPageSizeBooking" EventName="SelectedIndexChanged" />
                    <asp:AsyncPostBackTrigger ControlID="ddlPageSizeCntr" EventName="SelectedIndexChanged" />
                    <asp:AsyncPostBackTrigger ControlID="ddlPageSizeCargaSuelta" EventName="SelectedIndexChanged" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>



    <script type="text/javascript">

        function prepareObjectRuc() {
            try {
                document.getElementById("loader3").className = '';

                return true;
            } catch (e) {
                alert('Lamentamos comunicarle que ha ocurrido el siguiente problema:\n' + e.Message);
            }
        }

        function mostrarloader(Valor) {

            try {
                if (Valor == "1") {
                    document.getElementById("ImgCarga").className = 'ver';
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
