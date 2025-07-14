<%@ Page Title="Consulta de Carga" Language="C#" MasterPageFile="~/site.Master" AutoEventWireup="true" CodeBehind="UNIT_Consulta_Contenedor.aspx.cs" Inherits="CSLSite.unit.UNIT_Consulta_Contenedor" %>

<%@ MasterType VirtualPath="~/site.Master" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>


<asp:Content ID="Content1" ContentPlaceHolderID="placehead" runat="server">
    <link href="../css/bootstrap.min.css" rel="stylesheet" />
    <link href="../css/dashboard.css" rel="stylesheet" />
    <link href="../css/icons.css" rel="stylesheet" />
    <link href="../css/style.css" rel="stylesheet" />

    <!--external css-->
    <link href="../shared/estilo/jquery.datetimepicker.css" rel="stylesheet" type="text/css" />
    <script src="../js/sweetAlert.js" type="text/javascript"></script>

    <script src="../Scripts/jquery.datetimepicker.js" type="text/javascript"></script>
    <script src="../js/bootstrap.bundle.min.js" type="text/javascript"></script>
    <script src="../js/dashboard.js" type="text/javascript"></script>
    <link href="../css/sweetalert2.min.css" rel="stylesheet" />
    <script type="text/javascript" src="../lib/jquery/jquery-3.6.0.min.js"></script>
    <link href="../lib/advanced-datatable/css/demo_page.css" rel="stylesheet" />
    <link href="../lib/advanced-datatable/css/demo_table.css" rel="stylesheet" />
    <link rel="stylesheet" href="../lib/advanced-datatable/css/DT_bootstrap2.css" />

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

        .ver {
            display: inline-block; /* Muestra el loader */
        }

        .nover {
            display: none; /* Oculta el loader */
        }

        .modal-header .btn-close {
            position: relative;
            width: 40px; /* Hace la X más grande */
            height: 40px;
            font-size: 24px; /* Ajusta el tamaño de la X */
            padding: 8px; /* Agrega un poco de espacio */
            margin-right: 10px; /* Separa la X del borde derecho */
            margin-top: 5px; /* Baja un poco la X */
            filter: invert(0); /* Asegura que se vea en fondos oscuros */
            opacity: 1; /* Hace que siempre sea visible */
        }

        .status-label {
            padding: 3px 8px;
            border-radius: 4px;
            font-weight: bold;
            color: white;
            font-size: 0.85rem;
        }

        .carousel-item img {
            max-height: 700px;
            object-fit: contain;
            margin: auto;
        }

        .carousel-control-prev-icon,
        .carousel-control-next-icon {
            background-color: rgba(0, 0, 0, 0.5);
            border-radius: 50%;
            padding: 20px;
            background-size: 100% 100%;
        }
    </style>

    <script type="text/javascript">
        function initFotosDanios() {
            var idContenedor = $('#placebody_lblidContenedor').text().trim();
            if (!idContenedor) {
                mostrarError("Parámetro inválido: idContenedor es requerido.");
                return;
            }

            mostrarLoaderSwal();
            $('#modalFotosContenedor').modal('hide'); // por si quedó abierto
            $('#carouselExampleCaptions').carousel('dispose'); // limpia instancia previa del carrusel
            $('#placebody_htmlImagenesContenedor').html(''); // limpia HTML previo

            fetch("UNIT_Consulta_Contenedor.aspx/ObtenerFotosContenedor", {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify({ idContenedor: idContenedor })
            })
                .then(response => response.json())
                .then(data => {
                    ocultarLoaderSwal();

                    if (typeof data.d === "string") {
                        data.d = JSON.parse(data.d);
                    }

                    console.log("Fotos recibidas:", data.d);

                    if (data.d.error) {
                        mostrarError(data.d.error);
                        return;
                    }

                    if (data.d.length === 0) {
                        document.getElementById('sinResultadoFotosContenedor').style.display = "block";
                        return;
                    }

                    let divImagenes = '';
                    data.d.forEach((item, index) => {
                        divImagenes += `
                <div class="carousel-item ${index === 0 ? 'active' : ''}">
                    <img src="${item.UrlWeb}" class="d-block w-100" style="max-height:110vh; max-width: 100vw; object-fit:contain;" alt="Foto de daño" />
                </div>`;
                    });

                    document.getElementById('placebody_htmlImagenesContenedor').innerHTML = `
            <div class="mb-5">
                <div id="carouselExampleCaptions" class="carousel slide" data-ride="carousel">
                    <div class="carousel-inner">
                        ${divImagenes}
                    </div>
                    <a class="carousel-control-prev" href="#carouselExampleCaptions" role="button" data-slide="prev">
                        <span class="carousel-control-prev-icon" aria-hidden="true"></span>
                        <span class="sr-only">Anterior</span>
                    </a>
                    <a class="carousel-control-next" href="#carouselExampleCaptions" role="button" data-slide="next">
                        <span class="carousel-control-next-icon" aria-hidden="true"></span>
                        <span class="sr-only">Siguiente</span>
                    </a>
                </div>
            </div>`;

                    // Forzar inicio del carrusel tras render
                    setTimeout(() => {
                        $('#modalFotosContenedor').modal('show');
                        $('#carouselExampleCaptions').carousel(); // Bootstrap 4
                    }, 100);
                })
                .catch(error => {
                    console.error("Error al obtener imágenes:", error);
                    ocultarLoaderSwal();
                    mostrarError("No se pudieron cargar las imágenes del contenedor.");
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

        function ocultarLoaderSwal() {
            Swal.close();
        }


        document.addEventListener("DOMContentLoaded", function () {
            let row = document.getElementById("cardsRow");
            let icon = document.getElementById("toggleIcon");
            if (!row || !icon) return;
        });

        function descargarCertificadoEIR(gkey) {
            mostrarLoaderSwal();
            if (!gkey) {

                mostrarError("Parámetro inválido: gkey es requerido.");
                return;
            }

            fetch("UNIT_Consulta_Contenedor.aspx/ObtenerPdfBase64", {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify({ gkeyValue: gkey })
            })
                .then(response => {
                    if (!response.ok) {
                        return response.text().then(text => {
                            try {
                                let errorData = JSON.parse(text);
                                throw new Error(errorData.error || "Error desconocido al generar el certificado.");
                            } catch (e) {
                                throw new Error("Erro: no se encontro datos para el certificado.");
                            }
                        });
                    }
                    ocultarLoaderSwal();
                    return response.blob();
                })
                .then(pdfBlob => {
                    let pdfUrl = URL.createObjectURL(pdfBlob);
                    document.getElementById("pdfViewer").src = pdfUrl;

                    let modal = new bootstrap.Modal(document.getElementById("pdfModalIER"));
                    modal.show();
                })
                .catch(error => {

                    console.error("Error en la descarga:", error);
                    ocultarLoaderSwal();
                    mostrarError("Ocurrió un problema al descargar la EIR");
                });
        }

        function descargarPdfPeso(vgmData) {
            mostrarLoaderSwal();
        
            console.log("vgmdata:",vgmData)
            fetch("UNIT_Consulta_Contenedor.aspx/ObtenerPdfPesoImpo", {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify({ vgmDato: vgmData }),
            })
                .then(response => {
                    return response.json();
                })
                .then(data => {
                    console.timeEnd("TiempoFetch");


                    if (!data || !data.d) {
                        Swal.fire("Error", "Respuesta inválida del servidor.", "error");
                        ocultarLoaderSwal();
                        return;
                    }

                    if (data.d.startsWith("ERROR")) {
                        Swal.fire("Error", data.d, "error");
                        ocultarLoaderSwal();

                    } else {
                        let pdfBase64 = data.d;
                        let pdfBlob = base64ToBlob(pdfBase64, "application/pdf");
                        let pdfUrl = URL.createObjectURL(pdfBlob);

                        document.getElementById("pdfPeso").src = pdfUrl;

                        let modal = new bootstrap.Modal(document.getElementById("pdfModalPeso"));

                        ocultarLoaderSwal();
                        modal.show();
                    }
                })
                .catch(error => {

                    console.error("Error en la descarga:", error);
                    ocultarLoaderSwal();
                    mostrarError("Ocurrió un problema al descargar la Peso");
                });
        }

        function descargarPdfFact(gkey) {
            console.log("gkey", gkey)
            if (!gkey) {
                mostrarError("Ocurrió un problema al descargar la factura.");
                return;
            }
            mostrarLoaderSwal();
            
            fetch("UNIT_Consulta_Contenedor.aspx/ObtenerPdfFact", {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify({ gkeyValue: gkey }),
            })
                .then(response => response.json())
                .then(data => {
                    if (data.d.startsWith("ERROR")) {
                        mostrarError("No se encontró la información para descargar el VGM.")
                    } else {
                        let pdfBase64 = data.d;
                        let pdfBlob = base64ToBlob(pdfBase64, "application/pdf");
                        let pdfUrl = URL.createObjectURL(pdfBlob);

                        ocultarLoaderSwal();
                        document.getElementById("pdfFact").src = pdfUrl;

                        let modal = new bootstrap.Modal(document.getElementById("pdfModalFact"));
                        modal.show();

                    }
                })
                .catch(error => {

                    console.error("Error en la descarga:", error);
                    ocultarLoaderSwal();
                    mostrarError("Ocurrió un problema al descargar Facturas.");
                });

        }
        function descargarPdfVgm(vgmData) {

            mostrarLoaderSwal();
            if (!vgmData || !vgmData.gkeyValue) {
                mostrarError("No se encontró la información para descargar el VGM.")

                return;
            }

            fetch("UNIT_Consulta_Contenedor.aspx/ObtenerPdfVgm", {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify({ vgmDato: vgmData }),
            })
                .then(response => response.json())
                .then(data => {
                    if (data.d.startsWith("ERROR")) {
                        mostrarError("No se encontró la información para descargar el VGM.")
                    } else {
                        let pdfBase64 = data.d;
                        let pdfBlob = base64ToBlob(pdfBase64, "application/pdf");
                        let pdfUrl = URL.createObjectURL(pdfBlob);

                        ocultarLoaderSwal();
                        document.getElementById("pdfVgm").src = pdfUrl;

                        let modal = new bootstrap.Modal(document.getElementById("pdfModalVgm"));
                        modal.show();

                    }
                })
                .catch(error => {

                    console.error("Error en la descarga:", error);
                    ocultarLoaderSwal();
                    mostrarError("Ocurrió un problema al descargar VGM.");
                });
        }

        function descargarCertificadoPasePuerta(vgmData) {
            mostrarLoaderSwal();
            fetch("UNIT_Consulta_Contenedor.aspx/ObtenerPdfPasePuerta", {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify({ idPase: vgmData }),
            })
                .then(response => response.json())
                .then(data => {
                    console.log("Respuesta del servidor:", data); // 📌 Verifica si es una cadena Base64 válida

                    if (data.d.startsWith("ERROR")) {
                        ocultarLoaderSwal();
                        mostrarError("No se encontró la información para descargar el Pase Puerta.");
                    } else {
                        let pdfBase64 = data.d;
                        let pdfBlob = base64ToBlob(pdfBase64, "application/pdf");
                        let pdfUrl = URL.createObjectURL(pdfBlob);

                        document.getElementById("pdfPase").src = pdfUrl;

                        let modal = new bootstrap.Modal(document.getElementById("pdfModalPase"));
                        ocultarLoaderSwal();
                        modal.show();
                    }
                })
                .catch(error => {
                    console.error("Error en la descarga:", error);
                    ocultarLoaderSwal();
                    mostrarError("Ocurrió un problema al descargar el Pase Puerta.");
                });
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
        function base64ToBlob(base64, mimeType) {
            let byteCharacters = atob(base64);
            let byteNumbers = new Array(byteCharacters.length).fill(0).map((_, i) => byteCharacters.charCodeAt(i));
            let byteArray = new Uint8Array(byteNumbers);
            return new Blob([byteArray], { type: mimeType });
        }


        function cerrarModal(modalId) {
            let idModal = "#" + modalId;
            console.log('Cerrando modal:', idModal);

            $(idModal).removeClass('show');
            $(idModal).attr('aria-hidden', 'true');
            $(idModal).css('display', 'none');
            $('.modal-backdrop').remove();
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
            }, 500); // 🔹 Espera 300ms para asegurar que el Loader desaparezca antes del error
        }
        function dispararDescargaEIR() {
            const gkey = '<%= Session["Gkey"] %>';
            if (gkey) {
                descargarCertificadoEIR(gkey);
            } else {
                mostrarError("No se encontró el dato para descargar el certificado.");
            }
        }

    </script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="placebody" runat="server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></asp:ToolkitScriptManager>
    <div id="div_BrowserWindowName" style="visibility: hidden;">
        <asp:HiddenField ID="HiddenField1" runat="server" />
        <asp:HiddenField ID="hf_BrowserWindowName" runat="server" />
        <asp:HiddenField ID="hfJsonVgm" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hfContenedor" runat="server" Value="<%= lblidContenedor.Text %>" />

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

        <!-- Card: Información del Contenedor -->
        <div class="col-md-4">
            <div class="card">
                <div class="card-body">
                    <h3 class="form-title">Información del Contenedor</h3>

                    <!-- Datos del contenedor -->
                    <p class="small-text"><strong># de Contenedor:</strong> <span id="lblidContenedor" runat="server"></span></p>
                    <p class="small-text"><strong>Categoria :</strong> IMPORTACIÓN </p>
                    <p class="small-text"><strong>Tamaño del Contenedor:</strong> <span id="lblCNTR_Size" runat="server"></span></p>
                    <p class="small-text"><strong>Nave:</strong> <span id="lblVessel" runat="server"></span></p>

                    <p class="small-text">
                        <strong>Estado:</strong>
                        <asp:Label ID="lbltstate" runat="server" CssClass="fw-bold" />
                    </p>

                    <p class="small-text"><strong>Nombre Importador:</strong> <span id="lblExportador" runat="server"></span></p>
                    <p class="small-text"><strong title="# Manifiesto Carga">MRN:</strong> <span id="lblMrn" title="# Manifiesto Carga" runat="server"></span></p>

                    <p class="small-text">
                        <strong>Peso:</strong>
                        <span id="lblweight" runat="server" clientidmode="Static"
                            title="Descargar Certificado de Peso"
                            style="cursor: pointer; color: #0d6efd; text-decoration: underline;"
                            onclick="descargarPdfPeso(JSON.parse(document.getElementById('hfJsonVgm').value))"></span>
                    </p>

                    <p class="small-text"><strong>No. Doc:</strong> <span id="lblnumberdoc" runat="server"></span></p>
                    <p class="small-text"><strong>Fecha Vigencia Cas:</strong> <span id="lblFCAS" runat="server"></span></p>
                    <p class="small-text">
                        <strong>Fecha de Salida:</strong>
                        <span id="lblSalida" runat="server" clientidmode="Static"
                            title="Descargar Certificado EIR"
                            style="cursor: pointer; color: #0d6efd; text-decoration: underline;"
                            onclick="dispararDescargaEIR()"></span>
                    </p>

                    <!-- Botones de acciones -->
                    <div class="mb-2 d-flex justify-content-between align-items-center">
                        <strong>Consulta de Sellos:</strong>
                        <asp:LinkButton ID="btnSellos" runat="server" CssClass="btn btn-outline-primary btn-sm" OnClick="btnSellos_Click">Ver</asp:LinkButton>
                    </div>
                    <div class="mb-2 d-flex justify-content-between align-items-center">
                        <strong>Facturas Emitidas:</strong>
                        <asp:LinkButton ID="btnFacturas" runat="server" CssClass="btn btn-outline-primary btn-sm" OnClick="btnFacturas_Click">Ver</asp:LinkButton>
                    </div>
                    <div class="mb-2 d-flex justify-content-between align-items-center">
                        <strong>Pase Puerta:</strong>
                        <asp:LinkButton ID="btnPasePuerta" runat="server" CssClass="btn btn-outline-primary btn-sm" OnClick="btnPasePuerta_Click">Ver</asp:LinkButton>
                    </div>
                    <div class="mb-4 d-flex justify-content-between align-items-center">
                        <strong>Control de Daños:</strong>
                        <asp:LinkButton ID="btnFotosDanios" CssClass="btn btn-outline-primary btn-sm" runat="server" OnClientClick="initFotosDanios(); return false;">Ver</asp:LinkButton>
                    </div>

                    <hr />
                </div>
            </div>
        </div>

        <!-- Card: Novedades -->
        <div class="col-md-6">
            <div class="card">
                <div class="card-body">
                    <h3 class="form-title">Novedades</h3>
                    <p class="small-text">
                        <strong>Aforo:</strong> <span id="lblAforo" runat="server"></span>
                    </p>
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

                    <div id="loaderTreeView" class="nover text-center">
                        <img src="../lib/file-uploader/img/loading.gif" height="50px" width="50px" alt="Cargando..." />
                        <p>Cargando información...</p>
                    </div>

                    <!-- GridView de Sellos -->
                    <asp:GridView ID="gvSellos" runat="server" AutoGenerateColumns="False" OnRowCommand="gvSellos_RowCommand" CssClass="table table-striped" GridLines="None">
                        <Columns>
                            <asp:BoundField DataField="SEAL_1" HeaderText="Sello 1" />
                            <asp:BoundField DataField="SEAL_2" HeaderText="Sello 2" />
                            <asp:BoundField DataField="SEAL_3" HeaderText="Sello 3" />
                            <asp:BoundField DataField="SEAL_4" HeaderText="Sello 4" />
                            <asp:BoundField DataField="FECHA" HeaderText="FECHA" />
                            <asp:TemplateField HeaderText="">
                                <ItemTemplate>
                                    <asp:Button ID="btnFotoSellos" runat="server" Text="Fotos"
                                        CssClass="btn btn-primary btn-sm"
                                        CommandName="Foto"
                                        CommandArgument="GLOBAL"
                                        data-toggle="modal" data-target="#myModal" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>

                    <!-- GridView de Facturas -->
                    <asp:GridView ID="gvFacturas" runat="server" AutoGenerateColumns="False" CssClass="table table-striped" GridLines="None"
                        OnRowCommand="gvFacturas_RowCommand" DataKeyNames="IV_FACTURA">
                        <Columns>
                            <asp:BoundField DataField="IV_FACTURA" HeaderText="Número Factura" />
                            <asp:BoundField DataField="IV_DESC_CLIENTE" HeaderText="Cliente" />
                            <asp:BoundField DataField="IV_TOTAL" HeaderText="Monto Total" DataFormatString="{0:C}" />
                            <asp:BoundField DataField="IV_NUMERO_CARGA" HeaderText="Número de Carga" />
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

                    <!-- GridView VGM -->
                    <asp:GridView ID="gvVGM" runat="server" AutoGenerateColumns="False" CssClass="table table-striped" GridLines="None"
                        OnRowCommand="gvVGM_RowCommand" DataKeyNames="cntr" EnableViewState="true">
                        <Columns>
                            <asp:BoundField DataField="cntr" HeaderText="Contenedor" />
                            <asp:BoundField DataField="export" HeaderText="Exportador" />
                            <asp:BoundField DataField="NOMBRE_BUQUE" HeaderText="Nombre Buque" />
                            <asp:BoundField DataField="VIAJE" HeaderText="Viaje" />
                            <asp:BoundField DataField="fecha" HeaderText="Fecha" DataFormatString="{0:dd/MM/yyyy}" />
                            <asp:TemplateField HeaderText="Datos Ocultos" Visible="False">
                                <ItemTemplate>
                                    <asp:HiddenField ID="taraHidden" runat="server" Value='<%# Bind("tara") %>' />
                                    <asp:HiddenField ID="pesoHidden" runat="server" Value='<%# Bind("peso") %>' />
                                    <asp:HiddenField ID="payloadHidden" runat="server" Value='<%# Bind("payload") %>' />
                                    <asp:HiddenField ID="equipoHidden" runat="server" Value='<%# Bind("equipo") %>' />
                                    <asp:HiddenField ID="rucHidden" runat="server" Value='<%# Bind("ruc") %>' />
                                    <asp:HiddenField ID="fechaRegHidden" runat="server" Value='<%# Bind("fecha_reg") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Acciones">
                                <ItemTemplate>
                                    <asp:Button ID="btnDescargarVGM" runat="server" CssClass="btn btn-primary btn-sm"
                                        CommandName="DescargarVGM" CommandArgument="<%# Container.DataItemIndex %>"
                                        Text="Descargar VGM" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>

                    <!-- GridView Pase Puerta -->
                    <asp:GridView ID="gvPasePuerta" runat="server" AutoGenerateColumns="False" CssClass="table table-striped" GridLines="None"
                        OnRowCommand="gvPasePuerta_RowCommand" DataKeyNames="sn" EnableViewState="true">
                        <Columns>
                            <asp:BoundField DataField="sn" HeaderText="Id" />
                            <asp:BoundField DataField="contenedor" HeaderText="Cntr" />
                            <asp:BoundField DataField="documento" HeaderText="Nombre Buque" />
                            <asp:BoundField DataField="PASE" HeaderText="Viaje" />
                            <asp:BoundField DataField="tturno" HeaderText="Fecha" DataFormatString="{0:dd/MM/yyyy}" />

                            <asp:TemplateField HeaderText="Datos Ocultos" Visible="False">
                                <ItemTemplate>
                                    <asp:HiddenField ID="GKEYHidden" runat="server" Value='<%# Bind("gkey") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Acciones">
                                <ItemTemplate>
                                    <asp:Button ID="btnDescargarPase" runat="server" CssClass="btn btn-primary btn-sm"
                                        CommandName="DescargarPase" CommandArgument="<%# Container.DataItemIndex %>"
                                        Text="Descargar Pase Puerta" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>

                </ContentTemplate>
            </asp:UpdatePanel>


        </div>


    </div>

    <div class="mt-3">
        <asp:Button ID="btnVolver" runat="server" Text="← Volver a la Consulta" CssClass="btn btn-secondary btn-sm ms-6" OnClick="btnVolver_Click" />

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

    <div class="modal fade" id="pdfModalPase" tabindex="-1" aria-labelledby="pdfModalLabel" aria-hidden="true">
        <div class="modal-dialog modal-xl">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="pdfModalLabelPase">Visualizar Pase Puerta</h5>
                    <button type="button" class="btn-close" onclick="cerrarModal('pdfModalPase')" data-bs-dismiss="modal" aria-label="Close">X</button>
                </div>
                <div class="modal-body">
                    <iframe id="pdfPase" width="100%" height="500px"></iframe>
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


    <div class="modal fade" id="modalFotosContenedor" tabindex="-1" role="dialog">
        <div class="modal-dialog" role="document" style="max-width: 820px">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title">FOTOS DEL CONTENEDOR</h5>
                    <button type="button" class="btn-close" onclick="cerrarModal('modalFotosContenedor')" data-bs-dismiss="modal" aria-label="Close">
                        X
                    </button>
                </div>
                <div class="modal-body">
                    <div id="placebody_htmlImagenesContenedor">
                    </div>
                    <div id="sinResultadoFotosContenedor" class="alert alert-warning" style="display: none;">
                        No se encontraron fotos del contenedor.
                    </div>
                </div>
            </div>
        </div>
    </div>

    <script type="text/javascript">
        $('#modalFotosContenedor').on('shown.bs.modal', function () {
            $('#carouselExampleCaptions').carousel();
        });
    </script>


</asp:Content>
