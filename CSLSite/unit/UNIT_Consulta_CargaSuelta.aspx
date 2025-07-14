<%@ Page Title="Consulta de Carga" Language="C#" MasterPageFile="~/site.Master" AutoEventWireup="true" CodeBehind="UNIT_Consulta_CargaSuelta.aspx.cs" Inherits="CSLSite.unit.UNIT_Consulta_CargaSuelta" %>

<%@ MasterType VirtualPath="~/site.Master" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>


<asp:Content ID="Content1" ContentPlaceHolderID="placehead" runat="server">
    <link href="../css/bootstrap.min.css" rel="stylesheet" />
    <link href="../css/dashboard.css" rel="stylesheet" />
    <link href="../css/icons.css" rel="stylesheet" />
    <link href="../css/style.css" rel="stylesheet" />

    <!--external css-->
    <link href="../shared/estilo/jquery.datetimepicker.css" rel="stylesheet" type="text/css" />

    <script src="../Scripts/jquery.datetimepicker.js" type="text/javascript"></script>
    <script src="../js/bootstrap.bundle.min.js" type="text/javascript"></script>
    <script src="../js/dashboard.js" type="text/javascript"></script>
    <script src="../js/sweetAlert.js" type="text/javascript"></script>

    <script type="text/javascript" src="../lib/jquery/jquery-3.6.0.min.js"></script>
    <link href="../lib/advanced-datatable/css/demo_page.css" rel="stylesheet" />
    <link href="../lib/advanced-datatable/css/demo_table.css" rel="stylesheet" />
    <link rel="stylesheet" href="../lib/advanced-datatable/css/DT_bootstrap2.css" />
    <link href="../css/sweetalert2.min.css" rel="stylesheet" />
    <style type="text/css">
        body {
            font-size: 1rem;
            margin: 0;
        }

        .form-title {
            font-size: 1.5rem;
            font-weight: bold;
            display: flex;
            align-items: center;
            margin: 0;
        }

        .small-text {
            font-size: 0.9rem;
        }

        .toggle-icon {
            cursor: pointer;
            font-size: 1.5rem;
            margin-left: 10px;
        }

        .modal-header .btn-close {
            position: relative; /* Mantiene el botón dentro del modal */
            width: 40px; /* Hace la X más grande */
            height: 40px;
            font-size: 24px; /* Ajusta el tamaño de la X */
            padding: 8px; /* Agrega un poco de espacio */
            margin-right: 10px; /* Separa la X del borde derecho */
            margin-top: 5px; /* Baja un poco la X */
            filter: invert(0); /* Asegura que se vea en fondos oscuros */
            opacity: 1; /* Hace que siempre sea visible */
        }
    </style>

    <script type="text/javascript">
        let loaderTimeout;

        function mostrarLoaderSwal() {
            Swal.fire({
                title: 'Cargando...',
                text: 'Por favor, espera un momento',
                allowOutsideClick: false,
                showConfirmButton: false,
                didOpen: () => {
                    Swal.showLoading();
                    // Auto cerrar luego de 5 segundos (5000 ms)
                    loaderTimeout = setTimeout(() => {
                        Swal.close();
                    }, 5000);
                }
            });
        }


        function ocultarLoaderSwal() {
            clearTimeout(loaderTimeout); // Evita que se cierre dos veces
            Swal.close();
        }


        document.addEventListener("DOMContentLoaded", function () {
            let row = document.getElementById("cardsRow");
            let icon = document.getElementById("toggleIcon");
            if (!row || !icon) return;
        });

        function descargarCertificadoEIR(gkey) {
            if (!gkey) {
                mostrarError("Ocurrio un error en la consulta.");
                return;
            }
            mostrarLoaderSwal();
            fetch("UNIT_Consulta_Booking.aspx/ObtenerPdfBase64", {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify({ gkeyValue: gkey })
            })
                .then(response => response.json())
                .then(data => {
                    if (data.d.startsWith("ERROR")) {
                        mostrarError(data.d);
                    } else {
                        let pdfBase64 = data.d;
                        let pdfBlob = base64ToBlob(pdfBase64, "application/pdf");
                        let pdfUrl = URL.createObjectURL(pdfBlob);

                        document.getElementById("pdfViewer").src = pdfUrl;

                        let modal = new bootstrap.Modal(document.getElementById("pdfModalIER"));
                        modal.show();
                        ocultarLoaderSwal();
                    }
                })
                .catch(error => {

                    console.error("Error en la descarga:", error);
                    ocultarLoaderSwal();
                    mostrarError("Ocurrió un problema al descargar la EIR");
                });

        }

        function abrirAISV() {
            var url = document.getElementById('AISV').value;
            console.log("url ", url);

            if (!url || url.trim() === "") {
                mostrarError("No hay AISV disponible para imprimir.");
                return false;
            }

            mostrarLoaderSwal();

            var iframe = document.getElementById("iframeAISV");
            iframe.src = url;

            iframe.onload = function () {
                ocultarLoaderSwal();
                document.getElementById("modalAISV").style.display = "block";
            };

            return false;
        }


        function descargarPdfFact(gkey) {
            if (!gkey) {
                mostrarError("No se encontró el GKEY para descargar el certificado.");
                return;
            }
            mostrarLoaderSwal();
            fetch("UNIT_Consulta_Booking.aspx/ObtenerPdfFact",

                {
                    method: "POST",
                    headers: { "Content-Type": "application/json" },
                    body: JSON.stringify({ gkeyValue: gkey })
                }).then(response => response.json()).then(data => {
                    if (data.d.startsWith("ERROR")) {
                        mostrarError(data.d);
                    }
                    else {
                        let pdfBase64 = data.d;
                        let pdfBlob = base64ToBlob(pdfBase64, "application/pdf");
                        let pdfUrl = URL.createObjectURL(pdfBlob);

                        document.getElementById("pdfFact").src = pdfUrl;

                        let modal = new bootstrap.Modal(document.getElementById("pdfModalFact"));
                        modal.show();
                        ocultarLoaderSwal();
                    }
                }).catch(error => {

                    console.error("Error en la descarga:", error);
                    ocultarLoaderSwal();
                    mostrarError("Ocurrió un problema al descargar la EIR");
                });
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

        function descargarPdfVgm(vgmData) {


            if (!vgmData || !vgmData.cntr) {
                mostrarError("No se encontró la información para descargar el VGM.");
                return;
            }
            mostrarLoaderSwal();
            fetch("UNIT_Consulta_Booking.aspx/ObtenerPdfVgm", {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify({ vgmDato: vgmData }),
            })
                .then(response => response.json())
                .then(data => {

                    console.log("data", data)
                    if (data.d.startsWith("ERROR")) {
                        mostrarError(data.d);
                    } else {
                        let pdfBase64 = data.d;
                        let pdfBlob = base64ToBlob(pdfBase64, "application/pdf");
                        let pdfUrl = URL.createObjectURL(pdfBlob);


                        document.getElementById("pdfVgm").src = pdfUrl;

                        let modal = new bootstrap.Modal(document.getElementById("pdfModalVgm"));
                        modal.show();
                        ocultarLoaderSwal();

                    }
                })
                .catch(error => {

                    console.error("Error en la descarga:", error);
                    ocultarLoaderSwal();
                    mostrarError("Ocurrió un problema al descargar la VGM");
                });
        }

        function descargarPdfPeso(vgmData) {

            if (!vgmData || !vgmData.CONTENEDOR) {
                mostrarError("No se encontró la información para descargar el certificado de peso.");
                return;
            }
            mostrarLoaderSwal();

            fetch("UNIT_Consulta_Booking.aspx/ObtenerPdfPeso", {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify({ vgmDato: vgmData }),
            })
                .then(response => {
                    return response.json();
                })
                .then(data => {

                    if (!data || !data.d) {
                        mostrarError("No se encontró la información para descargar el certificado de peso.");
                        return;
                    }

                    if (data.d.startsWith("ERROR")) {
                        Swal.fire("Error", data.d, "error");
                    } else {
                        let pdfBase64 = data.d;
                        let pdfBlob = base64ToBlob(pdfBase64, "application/pdf");
                        let pdfUrl = URL.createObjectURL(pdfBlob);

                        document.getElementById("pdfPeso").src = pdfUrl;

                        let modal = new bootstrap.Modal(document.getElementById("pdfModalPeso"));
                        modal.show();
                    }
                })
                .catch(error => {
                    mostrarError("Ocurrió un problema al descargar la Peso");
                });
        }

        function base64ToBlob(base64, mimeType) {
            let byteCharacters = atob(base64);
            let byteArrays = [];
            for (let i = 0; i < byteCharacters.length; i++) {
                byteArrays.push(byteCharacters.charCodeAt(i));
            }
            return new Blob([new Uint8Array(byteArrays)], { type: mimeType });
        }

        function toggleCards() {
            let row = document.getElementById("cardsRow");
            let icon = document.getElementById("toggleIcon");
            if (row.style.display === "none") {
                row.style.display = "flex";
                icon.innerHTML = "&#9660;";
            } else {
                row.style.display = "none";
                icon.innerHTML = "&#9654;";
            }
        }


        function cerrarModal(modalId) {
            let idModal = "#" + modalId;

            $(idModal).removeClass('show');
            $(idModal).attr('aria-hidden', 'true');
            $(idModal).css('display', 'none');
            $('.modal-backdrop').remove();
        }

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
            }, 500);
        }

        function exportar() {
            var xhr = new XMLHttpRequest();
            xhr.open('POST', 'UNIT_Consulta_Booking.aspx/ExportarExcel', true);
            xhr.setRequestHeader('Content-Type', 'application/json; charset=utf-8');
            xhr.responseType = 'blob';

            xhr.onload = function () {
                if (xhr.status === 200) {
                    var blob = xhr.response;

                    var fechaActual = new Date();
                    var formatoFecha = fechaActual.getFullYear().toString()
                        + ("0" + (fechaActual.getMonth() + 1)).slice(-2)
                        + ("0" + fechaActual.getDate()).slice(-2)
                        + "_"
                        + ("0" + fechaActual.getHours()).slice(-2)
                        + ("0" + fechaActual.getMinutes()).slice(-2)
                        + ("0" + fechaActual.getSeconds()).slice(-2);

                    var fileName = "Reporte_de_horas_reefer" + formatoFecha + ".xlsx";

                    var link = document.createElement('a');
                    link.href = window.URL.createObjectURL(blob);
                    link.download = fileName;
                    link.style.display = 'none';
                    document.body.appendChild(link);
                    link.click();
                    document.body.removeChild(link);
                } else {
                    console.log('Error al exportar el archivo Excel. Código de estado: ' + xhr.status);
                }
            };

            xhr.onerror = function () {
                console.log('Error al enviar la solicitud de exportación del archivo Excel.');
            };

            xhr.send(JSON.stringify({}));
        }



        function dispararDescargaEIR() {
            const gkey = '<%= Session["GkeyBooking"] %>';
            if (gkey) {
                descargarCertificadoEIR(gkey);
            } else {
                mostrarError("No se encontró el dato para descargar el certificado.");
            }
        }
        function dispararDescargaVGM() {
            const json = document.getElementById('hfJsonVgm').value;


            descargarPdfVgm(JSON.parse(json));
        }



    </script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="placebody" runat="server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></asp:ToolkitScriptManager>
    <div id="div_BrowserWindowName" style="visibility: hidden;">
        <asp:HiddenField ID="HiddenField1" runat="server" />
        <asp:HiddenField ID="hf_BrowserWindowName" runat="server" />
        <asp:HiddenField ID="hfJsonVgm" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hfJsonPeso" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="AISV" runat="server" ClientIDMode="Static" />
    </div>

    <div class="row">
        <div class="col-md-12">
            <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
        </div>
    </div>


    <br />


    <div class="d-flex align-items-center justify-content-between flex-wrap">
        <div class="d-flex align-items-center">
            <h5 class="form-title mb-0">Consulta de Contenedor
            <span id="toggleIcon" class="toggle-icon ms-2" onclick="toggleCards()">&#9660;</span>
            </h5>
        </div>
    </div>

    <div id="cardsRow" class="row mt-2">
        <!-- Card Unificada (Basic Info, Location, Dates) -->
        <div class="col-md-4">
            <div class="card">
                <div class="card-body">
                    <!-- Sección: Basic Information -->
                    <h3 class="form-title">Información Básica</h3>
                    <hr />
                    <p class="small-text" style="display: none;"><strong># de Contenedor:</strong> <span id="lblidContenedor" runat="server" style="display: none;"></span></p>
                    <p class="small-text"><strong title="Reserva">Booking</strong> <span id="lblBookingNo" runat="server"></span></p>
                    <p class="small-text"><strong>Nombre Nave:</strong> <span id="lblVesselName" runat="server"></span></p>
                    <p class="small-text"><strong>Viaje:</strong> <span id="lblVoyage" runat="server"></span></p>
                    <p class="small-text"><strong>Categoria:</strong> <span id="lblCategory" runat="server">Exportación</span></p>
                    <p class="small-text">
                        <strong>Peso:</strong>
                        <span id="lblweight" runat="server" clientidmode="Static"
                            title="Descargar Certificado de Peso"
                            style="cursor: pointer; color: #0d6efd; text-decoration: underline;"
                            onclick="descargarPdfPeso(JSON.parse(document.getElementById('hfJsonPeso').value))"></span>
                    </p>
                    <!-- Sección: Location & Destination -->
                    <h3 class="form-title mt-3">Ubicación y Destino</h3>
                    <hr />
                    <p class="small-text"><strong>Lugar de Recepción:</strong> <span id="lblPlaceReceipt" runat="server"></span></p>
                    <p class="small-text"><strong>Puerto de Carga:</strong> <span id="lblPortLoading" runat="server"></span></p>
                    <p class="small-text"><strong>Puerto de Descarga:</strong> <span id="lblPortDischarging" runat="server"></span></p>

                    <!-- Sección: Estimated Dates -->
                    <h3 class="form-title mt-3">Fechas Estimadas</h3>
                    <hr />
                    <p class="small-text"><strong>Fecha estimada de llegada:</strong> <span id="lblEstDepArribDate" runat="server"></span></p>
                    <p class="small-text">
                        <strong>Fecha de Salida:</strong>
                        <span id="lblEstDepDate" runat="server" clientidmode="Static"
                            title="Descargar Certificado EIR"
                            style="cursor: pointer; color: #0d6efd; text-decoration: underline;"
                            onclick="dispararDescargaEIR()"></span>
                    </p>
                   

                    <div class="mb-4 d-flex justify-content-between align-items-center">
                        <strong>Peso VGM:</strong>
                        <asp:Button ID="btnDescargarVGM" runat="server" Text="Ver" CssClass="btn btn-outline-primary btn-sm" OnClick="btnDescargarVGM_Click" />
                    </div>

                    <div class="mb-2 d-flex justify-content-between align-items-center">
                        <strong>Facturas Emitidas:</strong>
                        <asp:LinkButton ID="btnFacturas" runat="server" CssClass="btn btn-outline-primary btn-sm" OnClick="btnFacturas_Click">Ver</asp:LinkButton>
                    </div>

                    <div class="mb-4 d-flex justify-content-between align-items-center">
                        <strong>Exportar Reporte de Facturas:</strong>
                        <asp:LinkButton ID="btnFactExcel" CssClass="btn btn-outline-primary btn-sm" runat="server" OnClick="btnFactExcel_Click">Ver</asp:LinkButton>
                    </div>
                <%--    <div class="mb-4 d-flex justify-content-between align-items-center">
                        <strong>Descargar AISV:</strong>
                        <asp:LinkButton ID="btnAisv" runat="server" CssClass="btn btn-outline-primary btn-sm" OnClientClick="return abrirAISV();">Ver</asp:LinkButton>

                    </div>--%>

                    <hr />

                </div>
            </div>
        </div>

        <!-- Card: Bloqueos -->
        <div class="col-md-6">
            <div class="card">
                <div class="card-body">
                    <h3 class="form-title">Novedades</h3>
                    <hr />
                    <p class="small-text"><strong>Tipos de Bloqueos:</strong> <span id="lblTiposBloqueos" runat="server"></span></p>
                    <p class="small-text">
                        <strong>Historial de Bloqueos:</strong><br />
                        <asp:Repeater ID="rptHistorialBloqueos" runat="server">
                            <ItemTemplate>
                                <div style="margin-bottom: 5px;">
                                    <span><strong>Fecha Bloqueo:</strong> <%# Eval("FECHA", "{0:dd/MM/yyyy HH:mm}") %></span><br />
                                    <span><strong>Fecha Desbloqueo:</strong>
                                        <%# Eval("FECHA_CAMBIO") == DBNull.Value || Eval("FECHA_CAMBIO") == null 
                    ? "Pendiente" 
                    : String.Format("{0:dd/MM/yyyy HH:mm}", Eval("FECHA_CAMBIO")) %>
                                    </span>
                                    <br />
                                    <span><strong>Acción:</strong> <%# Eval("Id") %></span>
                                </div>
                            </ItemTemplate>
                        </asp:Repeater>

                    </p>

                </div>
            </div>

            <asp:UpdatePanel ID="UPWUSR" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
             

                    <!-- GridView de Facturas -->
                    <asp:GridView ID="gvFacturas" runat="server" AutoGenerateColumns="False" CssClass="table table-striped" GridLines="None"
                        OnRowCommand="gvFacturas_RowCommand" DataKeyNames="IV_FACTURA">
                        <Columns>
                            <asp:BoundField DataField="IV_FACTURA" HeaderText="Número Factura" />
                            <asp:BoundField DataField="IV_DESC_CLIENTE" HeaderText="Cliente" />
                            <asp:BoundField DataField="IV_TOTAL" HeaderText="Monto Total" DataFormatString="{0:C}" />
                            <%--  <asp:BoundField DataField="IV_NUMERO_CARGA" HeaderText="Número de Carga" />--%>
                            <asp:BoundField DataField="IV_FECHA" HeaderText="Fecha" DataFormatString="{0:dd/MM/yyyy}" />
                            <asp:TemplateField HeaderText="Acciones">
                                <ItemTemplate>
                                    <asp:Button ID="btnDescargar" runat="server" CssClass="btn btn-primary btn-sm"
                                        CommandName="DescargarPDF" CommandArgument="<%# Container.DataItemIndex %>"
                                        Text="Descargar PDF" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>


                    <asp:GridView ID="gvPeso" runat="server" AutoGenerateColumns="False" CssClass="table table-striped" GridLines="None"
                        OnRowCommand="gvPeso_RowCommand" DataKeyNames="CODIGO_CERTIFICADO" EnableViewState="true">

                        <Columns>

                            <asp:BoundField DataField="CONTENEDOR" HeaderText="Contenedor" />
                            <asp:BoundField DataField="PLACA" HeaderText="Placa" />
                            <asp:BoundField DataField="REFERENCIA" HeaderText="Referencia" />
                            <asp:BoundField DataField="NAVE" HeaderText="Nave" />
                            <asp:BoundField DataField="PESO_BALANZA" HeaderText="Peso Balanza" DataFormatString="{0:N2}" />
                            <asp:BoundField DataField="PESO_BRUTO" HeaderText="Peso Bruto" DataFormatString="{0:N2}" />
                            <asp:BoundField DataField="TARA" HeaderText="Tara" DataFormatString="{0:N2}" />
                            <asp:BoundField DataField="PESO_NETO" HeaderText="Peso Neto" DataFormatString="{0:N2}" />
                            <asp:BoundField DataField="FECHA" HeaderText="Fecha" DataFormatString="{0:dd/MM/yyyy HH:mm:ss}" />


                            <asp:TemplateField HeaderText="Acciones">
                                <ItemTemplate>
                                    <asp:Button ID="btnDescargarPeso" runat="server" CssClass="btn btn-primary btn-sm"
                                        CommandName="DescargarPeso" CommandArgument="<%# Container.DataItemIndex %>"
                                        Text="Descargar Certificado Peso" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>

                </ContentTemplate>
            </asp:UpdatePanel>
        </div>

    </div>
    <div class="mt-3">
        <asp:Button ID="btnVolver" runat="server" Text="← Volver a la Consulta" CssClass="btn btn-secondary btn-sm ms-3" OnClick="btnVolver_Click" />

    </div>
    <br />
    <hr />


    <!-- Modal para mostrar el PDF en pantalla completa -->
    <div class="modal fade" id="pdfModalIER" tabindex="-1" aria-labelledby="pdfModalLabel" aria-hidden="true">
        <div class="modal-dialog modal-xl">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="pdfModalLabel">Vista Previa del Certificado EIR</h5>
                    <button type="button" class="btn-close" onclick="cerrarModal('pdfModalIER')" data-bs-dismiss="modal" aria-label="Close">X</button>

                </div>
                <div class="modal-body">
                    <iframe id="pdfViewer" style="width: 100%; height: 90vh; border: none;"></iframe>
                </div>
            </div>
        </div>
    </div>


    <!-- Modal para mostrar el PDF Factura en pantalla completa -->
    <div class="modal fade" id="pdfModalFact" tabindex="-1" aria-labelledby="pdfModalLabel" aria-hidden="true">
        <div class="modal-dialog modal-xl">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="pdfModalLabelFact">Vista Previa de la factura</h5>

                    <button type="button" class="btn-close" onclick="cerrarModal('pdfModalFact')" data-bs-dismiss="modal" aria-label="Close">X</button>
                </div>
                <div class="modal-body">
                    <iframe id="pdfFact" style="width: 100%; height: 90vh; border: none;"></iframe>
                </div>
            </div>
        </div>
    </div>



    <div class="modal fade" id="pdfModalVgm" tabindex="-1" aria-labelledby="pdfModalLabel" aria-hidden="true">
        <div class="modal-dialog modal-xl">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="pdfModalLabelVGM">Visualizar VGM</h5>
                    <button type="button" class="btn-close" onclick="cerrarModal('pdfModalVgm')" data-bs-dismiss="modal" aria-label="Close">X</button>
                </div>
                <div class="modal-body">
                    <iframe id="pdfVgm" width="100%" height="500px"></iframe>
                </div>
            </div>
        </div>
    </div>



    <div class="modal fade" id="pdfModalPeso" tabindex="-1" aria-labelledby="pdfModalLabel" aria-hidden="true">
        <div class="modal-dialog modal-xl">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="pdfModalLabelPeso">Visualizar Peso</h5>
                    <button type="button" class="btn-close" onclick="cerrarModal('pdfModalPeso')" data-bs-dismiss="modal" aria-label="Close">X</button>
                </div>
                <div class="modal-body">
                    <iframe id="pdfPeso" width="100%" height="500px"></iframe>
                </div>
            </div>
        </div>
    </div>


    <div id="myModal" class="modal fade" tabindex="-1" role="dialog">

        <div class="modal-dialog" role="document" style="max-width: 820px">
            <!-- Este tag style controla el ancho del modal -->
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title">FOTOS DE SELLOS</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">

                    <asp:UpdatePanel ID="UPMODAL" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>

                            <section class="wrapper2">
                                <div class="row mb">
                                    <div class="content-panel">
                                        <div class="adv-table">
                                            <div class="bokindetalle" style="width: 100%; overflow: auto">

                                                <script type="text/javascript">
                                                    Sys.Application.add_load(BindFunctions);
                                                </script>

                                                <div id="xfinde2" runat="server" visible="false">

                                                    <!-- page start-->
                                                    <div class="chat-room mt">
                                                        <aside class="mid-side">

                                                            <div class="catawrap">
                                                                <div class="room-desk" id="htmlImagenes" runat="server">
                                                                </div>
                                                            </div>
                                                        </aside>
                                                        <br />
                                                    </div>
                                                    <!-- page end-->
                                                </div>

                                                <div id="sinresultadoFotos" runat="server" class=" alert  alert-warning" visible="false">
                                                    No se encontraron resultados, 
                                                        asegurese que ha exista fotos de esta transacción
                                                </div>
                                            </div>
                                        </div>
                                        <!--content-panel-->
                                    </div>
                                    <!--row mb-->
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

    <div id="modalAISV" class="modal" tabindex="-1" role="dialog" style="display: none;">
        <div class="modal-dialog modal-xl" role="document">
            <div class="modal-content" style="height: 90vh;">
                <div class="modal-header">
                    <h5 class="modal-title">Visualización AISV</h5>
                    <button type="button" class="btn-close" onclick="cerrarModal('modalAISV')" data-bs-dismiss="modal" aria-label="Close">X</button>
                </div>
                <div class="modal-body" style="height: calc(100% - 56px); padding: 0;">
                    <iframe id="iframeAISV" style="width: 100%; height: 100%; border: none;"></iframe>
                </div>
            </div>
        </div>
    </div>

</asp:Content>
