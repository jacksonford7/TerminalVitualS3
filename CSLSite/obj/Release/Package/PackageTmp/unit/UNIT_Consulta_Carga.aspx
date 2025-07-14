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
    
    <link href="../css/sweetalert2.min.css" rel="stylesheet" />
    <link href="../lib/advanced-datatable/css/demo_page.css" rel="stylesheet" />
    <link href="../lib/advanced-datatable/css/demo_table.css" rel="stylesheet" />
    <link rel="stylesheet" href="../lib/advanced-datatable/css/DT_bootstrap2.css" />
    
    <script type="text/javascript" src="../js/Confirmaciones.js""></script>
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
    </style>



</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="placebody" runat="server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></asp:ToolkitScriptManager>
    <div id="div_BrowserWindowName" style="visibility: hidden;">
        <asp:HiddenField ID="HiddenField1" runat="server" />
        <asp:HiddenField ID="hf_BrowserWindowName" runat="server" />
    </div>

    <div class="form-title">
        CONSULTA DE CARGA
    </div>

  <asp:UpdatePanel ID="UPCARGA" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
    <ContentTemplate>
        <div class="form-row">
            <!-- Primera Fila -->
            <div class="form-group col-md-2">
                <label for="inputZip">Acciones</label>
                <asp:DropDownList ID="ddlOpciones" runat="server" CssClass="form-control" AutoPostBack="True"
                    OnSelectedIndexChanged="ddlOpciones_SelectedIndexChanged" onchange="toggleFields()">
                    <asp:ListItem Value="Ctrn">Contenedor</asp:ListItem>
                    <asp:ListItem Value="Book">Booking</asp:ListItem>
                    <asp:ListItem Value="BL">No. B/L</asp:ListItem>
                    <asp:ListItem Value="NC">No. Carga</asp:ListItem>
                </asp:DropDownList>
            </div>

            <div class="form-group col-md-2" id="divMRN" runat="server">
                <label for="inputZip">MRN<span style="color: #FF0000; font-weight: bold;"></span></label>
                <asp:TextBox ID="TXTMRN" runat="server" CssClass="form-control uppercase"></asp:TextBox>
            </div>

            <div class="form-group col-md-1" id="divMSN" runat="server">
                <label for="inputZip">MSN<span style="color: #FF0000; font-weight: bold;"></span></label>
                <asp:TextBox ID="TXTMSN" runat="server" CssClass="form-control uppercase"></asp:TextBox>
            </div>

            <div class="form-group col-md-1" id="divHSN" runat="server">
                <label for="inputZip">HSN<span style="color: #FF0000; font-weight: bold;"></span></label>
                <asp:TextBox ID="TXTHSN" runat="server" CssClass="form-control uppercase"></asp:TextBox>
            </div>

            <div class="form-group col-md-2" id="divBooking" runat="server" visible="false">
                <label for="inputZip">Booking<span style="color: #FF0000; font-weight: bold;">*</span></label>
                <asp:TextBox ID="TXTBooking" runat="server" CssClass="form-control uppercase"></asp:TextBox>
            </div>

                    <div id="divContainer" runat="server" class="form-group col-md-2">
               <label id="lblCntr" runat="server" for="txtcontainer">CNTR<span style="color: #FF0000; font-weight: bold;">*</span></label>
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
    <div class="form-group col-md-2" id="divFecIngreso" runat="server" style="display:none;">
        <label for="inputAddress">Fecha Desde (Ingreso)<span style="color: #FF0000; font-weight: bold;"></span></label>
        <div class="d-flex">
            <asp:TextBox CssClass="form-control" runat="server" ID="fecdesde" AutoPostBack="false" MaxLength="10"></asp:TextBox>
            <asp:CalendarExtender ID="FEECHADESDE" runat="server"
                CssClass="cal_Theme1" Format="dd/MM/yyyy" TargetControlID="fecdesde">
            </asp:CalendarExtender>
        </div>
    </div>

    <div class="form-group col-md-2" id="divFecHasta" runat="server" style="display:none;">
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
                    <!-- GridView Principal -->
                    <div id="dvGbContainer" runat="server">
                        <asp:GridView ID="gbContainer" runat="server" AutoGenerateColumns="False" DataKeyNames="CONTENEDOR"
                            OnRowDataBound="gbContainer_RowDataBound" AllowPaging="True"
                            CellPadding="4" ForeColor="#333333" CssClass="table table-bordered invoice" Font-Size="Smaller">
                            <AlternatingRowStyle BackColor="White" />
                            <Columns>
                                <asp:BoundField DataField="CONTENEDOR" HeaderText="contenedor" Visible="False" />
                                <asp:BoundField DataField="CAS" HeaderText="Cas Move" />
                                <asp:TemplateField HeaderText="Contenedor">
                                 <ItemTemplate>
                                    <asp:HyperLink ID="lnkContenedor" runat="server"
                                        NavigateUrl='<%# Eval("CONTENEDOR") %>'
                                        Text='<%# Eval("CONTENEDOR") %>'
                                        Style="color: blue; font-weight: bold; text-decoration: underline;">
                                    </asp:HyperLink>
                                 </ItemTemplate>

                                </asp:TemplateField>

                                <asp:BoundField DataField="TIPO_IZO" HeaderText="Type ISO" />
                                <asp:BoundField DataField="Category" HeaderText="Category" />
                                <asp:BoundField DataField="VSTATE" HeaderText="V-State" />
                                <asp:BoundField DataField="TIPO_STATE" HeaderText="T-State" />
                                <asp:BoundField DataField="LINE_OP" HeaderText="Line Op" />
                                <asp:BoundField DataField="IB_ACTUAL_VISIT" HeaderText="I/B Actual Visit" />
                                  <asp:BoundField DataField="NDOCUMENTO" HeaderText="NDOCUMENTO" />
                                <asp:BoundField DataField="FRGHT_KIND" HeaderText="Frght Kind" />
                            </Columns>
                            <HeaderStyle BackColor="#B4B4B4" Font-Bold="True" ForeColor="White" BorderStyle="Groove" BorderWidth="1px" />
                            <RowStyle Font-Size="Small" HorizontalAlign="Left" VerticalAlign="Top" />
                        </asp:GridView>
                    </div>

                    <!-- Nuevo GridView gdvBooking -->
                    <div id="dvGBooking" runat="server">
                        <asp:GridView ID="gdvBooking" runat="server" AutoGenerateColumns="False" DataKeyNames="CNTR_CONTAINER"
                            OnRowDataBound="gdvBooking_RowDataBound" AllowPaging="True"
                            CellPadding="4" ForeColor="#333333" CssClass="table table-bordered invoice" Font-Size="Smaller">
                            <AlternatingRowStyle BackColor="White" />
                            <Columns>


                                <asp:TemplateField HeaderText="Unit Nbr">
                                    <ItemTemplate>
                                        <asp:HyperLink ID="lnkCntrBooking" runat="server"
                                            NavigateUrl='<%# Eval("CNTR_CONTAINER") %>'
                                            Text='<%# Eval("CNTR_CONTAINER") %>'
                                            Style="color: blue; font-weight: bold; text-decoration: underline;">

                                        </asp:HyperLink>

                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:BoundField DataField="CNTR_TYSZ_ISO" HeaderText="Type ISO" />

                                <asp:BoundField DataField="CNTR_TYPE" HeaderText="Category" />
                                <asp:BoundField DataField="CNTR_YARD_STATUS" HeaderText="V-State" />
                                <asp:BoundField DataField="CNTR_CLNT_CUSTOMER_LINE" HeaderText="Line Op" />
                                <asp:BoundField DataField="CNTR_BKNG_BOOKING" HeaderText="Booking" />
                                <asp:BoundField DataField="CNTR_AISV" HeaderText="AISV" />
                                <asp:TemplateField HeaderText="TEMPERATURE">
                                    <ItemTemplate>
                                        <%# Eval("TemperaturaTexto") %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="CNTR_LCL_FCL" HeaderText="Frght Kind" />
                                <asp:BoundField DataField="WEIGHT" HeaderText="WEIGHT" />
                            </Columns>
                            <HeaderStyle BackColor="#B4B4B4" Font-Bold="True" ForeColor="White" BorderStyle="Groove" BorderWidth="1px" />
                            <RowStyle Font-Size="Small" HorizontalAlign="Left" VerticalAlign="Top" />
                        </asp:GridView>
                    </div>

                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>


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
    <script type="text/javascript" src="../js/buttons/1.6.4/dataTables.buttons.min.js"></script>
    <script type="text/javascript" src="../js/buttons/1.6.4/jszip.min.js"></script>
    <script type="text/javascript" src="../js/buttons/1.6.4/pdfmake.min.js"></script>
    <script type="text/javascript" src="../js/buttons/1.6.4/vfs_fonts.js"></script>
    <script type="text/javascript" src="../js/buttons/1.6.4/buttons.print.min.js"></script>
    <script type="text/javascript" src="../js/buttons/1.6.4/buttons.html5.min.js"></script>
    <script type="text/javascript" src="../js/buttons/1.6.4/buttons.colVis.min.js"></script>


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
