<%@ Page Title="Consulta de Carga" Language="C#" MasterPageFile="~/site.Master" AutoEventWireup="true" CodeBehind="UNIT_Consulta_Booking.aspx.cs" Inherits="CSLSite.unit.UNIT_Consulta_Booking" %>

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
    <script type="text/javascript" src="../js/Confirmaciones.js""></script>
    <style type="text/css">
        .treeview-menu {
            background: #f8f9fa;
            padding: 10px;
            border-radius: 10px;
            width: 100%;
        }

            .treeview-menu td.main-node {
                font-size: 22px;
                font-weight: bold;
                color: #000; /* Negro por defecto */
                padding: 12px 18px;
                position: relative;
                display: flex;
                align-items: center;
                gap: 10px;
                cursor: pointer;
            }

                .treeview-menu td.main-node:hover::after {
                    content: "▶";
                    font-size: 18px;
                    color: #E23B1B;
                    margin-left: auto;
                    transition: transform 0.3s ease;
                }

                .treeview-menu td.main-node.expanded:hover::after {
                    transform: rotate(90deg);
                }

            .treeview-menu td.sub-node {
                font-size: 18px;
                font-weight: normal;
                color: #333;
                padding-left: 40px;
                cursor: pointer;
                position: relative;
            }

            .treeview-menu td:hover::before {
                content: "";
                position: absolute;
                left: 0;
                top: 50%;
                transform: translateY(-50%);
                height: 80%;
                width: 3px;
                background-color: #E23B1B;
                border-radius: 2px;
            }

            .treeview-menu td.selected {
                font-weight: bold;
                color: #E23B1B;
            }

            .treeview-menu td:hover {
                background: rgba(226, 59, 27, 0.1);
                font-weight: bold;
                color: #E23B1B;
            }


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
        document.addEventListener("DOMContentLoaded", function () {
            let row = document.getElementById("cardsRow");
            let icon = document.getElementById("toggleIcon");
            if (!row || !icon) return;
        });

        function descargarCertificadoEIR(gkey) {
            if (!gkey) {
                alert("No se encontró el GKEY para descargar el certificado.");
                return;
            }

            fetch("UNIT_Consulta_Booking.aspx/ObtenerPdfBase64", {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify({ gkeyValue: gkey })
            })
                .then(response => response.json())
                .then(data => {
                    console.log("data:",data);
                    if (data.d.startsWith("ERROR")) {
                        alert(data.d);
                    } else {
                        let pdfBase64 = data.d;
                        let pdfBlob = base64ToBlob(pdfBase64, "application/pdf");
                        let pdfUrl = URL.createObjectURL(pdfBlob);

                        document.getElementById("pdfViewer").src = pdfUrl;

                        let modal = new bootstrap.Modal(document.getElementById("pdfModalIER"));
                        modal.show();
                    }
                })
                .catch(error => {

                    console.error("Error en la descarga:", error);
                    ocultarLoaderSwal();
                    mostrarError("Ocurrió un problema al descargar la EIR");
                });

        }

        function descargarPdfFact(gkey) {
            if (!gkey) {
                alert("No se encontró el GKEY para descargar el certificado.");
                return;
            }

            fetch("UNIT_Consulta_Booking.aspx/ObtenerPdfFact",

                {
                    method: "POST",
                    headers: { "Content-Type": "application/json" },
                    body: JSON.stringify({ gkeyValue: gkey })
                }).then(response => response.json()).then(data => {
                    if (data.d.startsWith("ERROR")) {
                        alert(data.d);
                    }
                    else {
                        let pdfBase64 = data.d;
                        let pdfBlob = base64ToBlob(pdfBase64, "application/pdf");
                        let pdfUrl = URL.createObjectURL(pdfBlob);

                        document.getElementById("pdfFact").src = pdfUrl;

                        let modal = new bootstrap.Modal(document.getElementById("pdfModalFact"));
                        modal.show();
                    }
                }).catch(error => {

                    console.error("Error en la descarga:", error);
                    ocultarLoaderSwal();
                    mostrarError("Ocurrió un problema al descargar la EIR");
                });
        }
        function descargarPdfVgm(vgmData) {

            console.log(vgmData, "sss")
            if (!vgmData || !vgmData.cntr) {
                alert("No se encontró la información para descargar el VGM.");
                return;
            }

            fetch("UNIT_Consulta_Booking.aspx/ObtenerPdfVgm", {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify({ vgmDato: vgmData }),
            })
                .then(response => response.json())
                .then(data => {

                    console.log("data", data)
                    if (data.d.startsWith("ERROR")) {
                        alert(data.d);
                    } else {
                        let pdfBase64 = data.d;
                        let pdfBlob = base64ToBlob(pdfBase64, "application/pdf");
                        let pdfUrl = URL.createObjectURL(pdfBlob);


                        document.getElementById("pdfVgm").src = pdfUrl;

                        let modal = new bootstrap.Modal(document.getElementById("pdfModalVgm"));
                        modal.show();

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
                Swal.fire("Error", "No se encontró la información para descargar el certificado de peso.", "error");
                return;
            }

            console.time("TiempoFetch");  // ⏳ Mide el tiempo de la solicitud

            fetch("UNIT_Consulta_Booking.aspx/ObtenerPdfPeso", {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify({ vgmDato: vgmData }),
            })
                .then(response => {
                    return response.json();
                })
                .then(data => {
                    console.timeEnd("TiempoFetch"); // 🕒 Fin del tiempo de carga
                    console.log("Respuesta del servidor:", data);

                    if (!data || !data.d) {
                        Swal.fire("Error", "Respuesta inválida del servidor.", "error");
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

                    console.error("Error en la descarga:", error);
                    ocultarLoaderSwal();
                    mostrarError("Ocurrió un problema al descargar la Peso");
                });
        }



        window.onload = function () {
            var imgCarga = document.getElementById("ImgCarga");
            if (imgCarga) {
                imgCarga.style.display = "none";
            }
        };
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
            console.log('Cerrando modal:', idModal);

            $(idModal).removeClass('show');  // Quita la clase 'show'
            $(idModal).attr('aria-hidden', 'true');  // Marca el modal como oculto
            $(idModal).css('display', 'none');  // Oculta manualmente
            $('.modal-backdrop').remove();  // Elimina el fondo oscuro de Bootstrap
        }

        function mostrarError(mensaje) {
            Swal.close(); // 🔹 Cierra cualquier alerta previa (como el loader)
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

        function exportar() {
            var xhr = new XMLHttpRequest();
            xhr.open('POST', 'UNIT_Consulta_Booking.aspx/ExportarExcel', true);
            xhr.setRequestHeader('Content-Type', 'application/json; charset=utf-8');
            xhr.responseType = 'blob';

            xhr.onload = function () {
                if (xhr.status === 200) {
                    var blob = xhr.response;

                    // Obtener la fecha actual en formato yyyyMMdd_HHmmss
                    var fechaActual = new Date();
                    var formatoFecha = fechaActual.getFullYear().toString()
                        + ("0" + (fechaActual.getMonth() + 1)).slice(-2)
                        + ("0" + fechaActual.getDate()).slice(-2)
                        + "_"
                        + ("0" + fechaActual.getHours()).slice(-2)
                        + ("0" + fechaActual.getMinutes()).slice(-2)
                        + ("0" + fechaActual.getSeconds()).slice(-2);

                    var fileName = "Reporte_de_horas_reefer" + formatoFecha + ".xlsx";

                    console.log("fileName", fileName);

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

    </script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="placebody" runat="server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></asp:ToolkitScriptManager>
    <div id="div_BrowserWindowName" style="visibility: hidden;">
        <asp:HiddenField ID="HiddenField1" runat="server" />
        <asp:HiddenField ID="hf_BrowserWindowName" runat="server" />
    </div>

    <div class="row">
        <div class="col-md-12">
            <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
        </div>
    </div>


    <br />
    <div class="d-flex align-items-center">
        <h5 class="form-title">Consulta de Contenedor <span id="toggleIcon" class="toggle-icon" onclick="toggleCards()">&#9660;</span></h5>
    </div>
    <div id="cardsRow" class="row mt-2">


    <!-- Card: Información Básica -->
    <div class="col-md-3">
        <div class="card">
            <div class="card-body">
                <h3 class="form-title">Basic Information</h3>
                
                <p class="small-text" style="display:none;"><strong># de Contenedor:</strong> <span id="lblidContenedor" runat="server" style="display:none;"></span></p>
                <p class="small-text"><strong>Book No.:</strong> <span id="lblBookingNo" runat="server"></span></p>
                <p class="small-text"><strong>Vessel Name:</strong> <span id="lblVesselName" runat="server"></span></p>
                <p class="small-text"><strong>Voyage:</strong> <span id="lblVoyage" runat="server"></span></p>
                <p class="small-text"><strong>Category:</strong> <span id="lblCategory" runat="server">Exportación</span></p>
            </div>
        </div>
    </div>

    <!-- Card: Lugares y Destinos -->
    <div class="col-md-3">
        <div class="card">
            <div class="card-body">
                <h3 class="form-title">Location & Destination</h3>
                <p class="small-text"><strong>Place of Receipt:</strong> <span id="lblPlaceReceipt" runat="server"></span></p>
                <p class="small-text"><strong>Port of Loading:</strong> <span id="lblPortLoading" runat="server"></span></p>
                <p class="small-text"><strong>Port of Discharging:</strong> <span id="lblPortDischarging" runat="server"></span></p>
            </div>
        </div>
    </div>

    <!-- Card: Fechas Estimadas -->
    <div class="col-md-3">
        <div class="card">
            <div class="card-body">
                <h3 class="form-title">Estimated Dates</h3>
                <div class="row">
                    <div class="col-md-6">
                        <p class="small-text"><strong>Estimated Arrival Date:</strong> <span id="lblEstDepArribDate" runat="server"></span></p>
                        <p class="small-text"><strong>Estimated Departure Date :</strong> <span id="lblEstDepDate" runat="server"></span></p>
                    </div>
                   
                </div>
            </div>
        </div>
    </div>

           <!-- Card: Bloqueos -->
        <div class="col-md-3">
            <div class="card">
                <div class="card-body">
                    <h3 class="form-title">Bloqueos</h3>
                    <p class="small-text"><strong>Tipos de Bloqueos:</strong> <span id="lblTiposBloqueos" runat="server"></span></p>
                </div>
            </div>
        </div>
    </div>
         <div class="mt-3">
        <asp:Button ID="btnVolver" runat="server" Text="← Volver a la Consulta" CssClass="btn btn-secondary"
            OnClick="btnVolver_Click" />
    </div>

    <br />
    <hr />

    <div class="row">
        <div class="col-md-3">
            <asp:UpdatePanel ID="UPTREEVIEW" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
                <ContentTemplate>
                <asp:TreeView ID="tvopciones" runat="server" CssClass="treeview-menu"
                    ExpandDepth="0"
                    EnableClientScript="true"
                    PopulateNodesFromClient="true"
                    OnTreeNodeDataBound="tvopciones_TreeNodeDataBound"
                    OnSelectedNodeChanged="tvopciones_SelectedNodeChanged">

                    <HoverNodeStyle Font-Underline="True" ForeColor="#5555DD" />
                    <Nodes>
                        <asp:TreeNode Text="Consulta de Sellos" Value="Sellos"></asp:TreeNode>
                        <asp:TreeNode Text="Facturas generadas clientes" Value="Facturas"></asp:TreeNode>
                        <asp:TreeNode Text="Descarga Certificado peso VGM" Value="VGM"></asp:TreeNode>
                        <asp:TreeNode Text="Certificado peso" Value="Peso"></asp:TreeNode>
                        <asp:TreeNode Text="Descarga Certificado EIR" Value="EIR (descarga)"></asp:TreeNode>
                        <asp:TreeNode Text="Exportar Reporte de Facturas" Value="ExportarFacturas"></asp:TreeNode>

                    </Nodes>
                </asp:TreeView>


                </ContentTemplate>
            </asp:UpdatePanel>


        </div>
        <div class="col-md-9">
            <div>
                <asp:UpdatePanel ID="UPWUSR" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>

                        <!-- GridView de Sellos -->
                        <asp:GridView ID="gvSellos" runat="server" AutoGenerateColumns="False" CssClass="table table-striped" GridLines="None">
                            <Columns>
                                <asp:BoundField DataField="SEAL_1" HeaderText="Sello 1" />
                                <asp:BoundField DataField="SEAL_2" HeaderText="Sello 2" />
                                <asp:BoundField DataField="SEAL_3" HeaderText="Sello 3" />
                                <asp:BoundField DataField="SEAL_4" HeaderText="Sello 4" />
                                <asp:BoundField DataField="FECHA" HeaderText="FECHA" />
                            </Columns>
                        </asp:GridView>

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

                        <asp:GridView ID="gvVGM" runat="server" AutoGenerateColumns="False" CssClass="table table-striped" GridLines="None"
                            OnRowCommand="gvVGM_RowCommand" DataKeyNames="cntr" EnableViewState="true">

                            <Columns>
                                <asp:BoundField DataField="cntr" HeaderText="Contenedor" />
                                <asp:BoundField DataField="export" HeaderText="Exportador" />
                                <asp:BoundField DataField="NOMBRE_BUQUE" HeaderText="Nombre Buque" />
                                <asp:BoundField DataField="VIAJE" HeaderText="Viaje" />
                                <asp:BoundField DataField="fecha" HeaderText="Fecha" DataFormatString="{0:dd/MM/yyyy}" />


                                <asp:TemplateField HeaderText="Acciones">
                                    <ItemTemplate>
                                        <asp:Button ID="btnDescargarVGM" runat="server" CssClass="btn btn-primary btn-sm"
                                            CommandName="DescargarVGM" CommandArgument="<%# Container.DataItemIndex %>"
                                            Text="Descargar VGM" />
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

    </div>
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

    <script type="text/javascript">



</script>
</asp:Content>
