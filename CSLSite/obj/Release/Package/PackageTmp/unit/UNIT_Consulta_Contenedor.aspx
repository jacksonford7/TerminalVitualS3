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

            if (!vgmData || !vgmData.CONTENEDOR) {
                Swal.fire("Error", "No se encontró la información para descargar el certificado de peso.", "error");
                return;
            }

            console.time("TiempoFetch");  // ⏳ Mide el tiempo de la solicitud

            fetch("UNIT_Consulta_Contenedor.aspx/ObtenerPdfPesoImpo", {
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

        function descargarPdfFact(gkey) {
            if (!gkey) {
                alert("No se encontró el GKEY para descargar el certificado.");
                return;
            }

            fetch("UNIT_Consulta_Contenedor.aspx/ObtenerPdfFact",

                {
                    method: "POST",
                    headers: { "Content-Type": "application/json" },
                    body: JSON.stringify({ gkeyValue: gkey })
                }).then(response => response.json()).then(data => {
                    if (data.d.startsWith("ERROR")) {
                        mostrarError(data.d)
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
                    mostrarError("Ocurrió un problema al descargar la factura");
                });
        }
        function descargarPdfVgm(vgmData) {
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
            }, 300); // 🔹 Espera 300ms para asegurar que el Loader desaparezca antes del error
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

        <!-- Card: Información del Contenedor -->
        <div class="col-md-4">
            <div class="card">
                <div class="card-body">
                    <h3 class="form-title">Información del Contenedor</h3>
                    <p class="small-text"><strong># de Contenedor:</strong> <span id="lblidContenedor" runat="server"></span></p>
                    <p class="small-text"><strong>Categoria :</strong> IMPORTACIÓN </p>
                    <p class="small-text"><strong>Tamaño del Contenedor:</strong> <span id="lblCNTR_Size" runat="server"></span></p>
                    <p class="small-text"><strong>Nave:</strong> <span id="lblVessel" runat="server"></span></p>
                    <p class="small-text"><strong>Estatus:</strong> <span id="lbltstate" runat="server"></span></p>
                    <p class="small-text"><strong>Nombre Importador:</strong> <span id="lblExportador" runat="server"></span></p>
                    <p class="small-text"><strong>MRN:</strong> <span id="lblMrn" runat="server"></span></p>
                 <%--   <p class="small-text"><strong>Tara:</strong> <span id="lblTara" runat="server"></span></p>--%>
                    <p class="small-text"><strong>Weight:</strong> <span id="lblweight" runat="server"></span></p>
                    <p class="small-text"><strong>No. Doc:</strong> <span id="lblnumberdoc" runat="server"></span></p>
                    <p class="small-text"><strong>Fecha de CAS:</strong> <span id="lblFCAS" runat="server"></span></p>
                    <p class="small-text"><strong>Fecha de Salida</strong> <span id="lblSalida" runat="server"></span></p>
                </div>
            </div>
        </div>

        <!-- Card: Inspección -->
        <div class="col-md-4">
            <div class="card">
                <div class="card-body">
                    <h3 class="form-title">Aforo </h3>
                    <p class="small-text"><strong>AFORO:</strong> <span id="lblAforo" runat="server"></span></p>

                </div>
            </div>
        </div>

        <!-- Card: Bloqueos -->
        <div class="col-md-4">
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
                        PopulateNodesFromClient="false"
                       
                        OnSelectedNodeChanged="tvopciones_SelectedNodeChanged"
                     >

                        <HoverNodeStyle Font-Underline="True" ForeColor="#5555DD" />
                        <Nodes>
                            <asp:TreeNode Text="Imagenes/Consultas" Value="img" Expanded="False">
<%--                                <asp:TreeNode Text="Informe Imagenes" Value="Informe Imagenes"></asp:TreeNode>--%>
                                <asp:TreeNode Text="Consulta de Sellos" Value="Consulta de Sellos"></asp:TreeNode>
<%--                                <asp:TreeNode Text="Descarga Certificado peso VGM" Value="VGM"></asp:TreeNode>--%>
                            </asp:TreeNode>
                            <asp:TreeNode Text="Reimpresion" Value="Reimpresion">
                                <asp:TreeNode Text="Descarga Certificado EIR" Value="EIR (descarga)"></asp:TreeNode>
<%--                                <asp:TreeNode Text="Consulta Unid. Vacias sin NAAA" Value="Consulta Unid. Vacias sin NAAA"></asp:TreeNode>--%>
<%--                                <asp:TreeNode Text="Formulario registro empresa" Value="Formulario registro empresa"></asp:TreeNode>--%>
                            </asp:TreeNode>
                            <asp:TreeNode Text="Reporte" Value="Reporte" Expanded="False">
                            <asp:TreeNode Text=" Certificado de Peso " Value="PesoIMPO"></asp:TreeNode>
<%--                                <asp:TreeNode Text="Monitoreo de temperatura" Value="Monitoreo de temperatura"></asp:TreeNode>--%>
                                <asp:TreeNode Text="Facturas generadas clientes" Value="Facturas"></asp:TreeNode>
                            </asp:TreeNode>
                            <asp:TreeNode Text="Otros" Value="Otros" Expanded="False">
                              <asp:TreeNode Text="Visualizar Pase Puerta" Value="PasePuerta"></asp:TreeNode>
             <%--                     <asp:TreeNode Text="Generacion proforma para carga suelta" Value="Generacion proforma para carga suelta"></asp:TreeNode>--%>
                            </asp:TreeNode>
                        </Nodes>
                    </asp:TreeView>

                </ContentTemplate>
                 <Triggers>
        <asp:AsyncPostBackTrigger ControlID="tvopciones" EventName="SelectedNodeChanged" />
    </Triggers>
            </asp:UpdatePanel>


        </div>
        <div class="col-md-9">
            <div>
                <asp:UpdatePanel ID="UPWUSR" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>

                <div id="loaderTreeView" class="nover text-center">
                    <img src="../lib/file-uploader/img/loading.gif" height="50px" width="50px" alt="Cargando..." />
                    <p>Cargando información...</p>
                </div>

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
                                        <asp:HiddenField ID="MRNHidden" runat="server" Value='<%# Bind("mrn") %>' />
                                        <asp:HiddenField ID="MSNHidden" runat="server" Value='<%# Bind("msn") %>' />
                                        <asp:HiddenField ID="HSNHidden" runat="server" Value='<%# Bind("hsn") %>' />
                                        <asp:HiddenField ID="BLHidden" runat="server" Value='<%# Bind("bl") %>' />
                                        <asp:HiddenField ID="DOCUMENTOHidden" runat="server" Value='<%# Bind("documento") %>' />
                                        <asp:HiddenField ID="PASEHidden" runat="server" Value='<%# Bind("pase") %>' />
                                        <asp:HiddenField ID="TTURNOHidden" runat="server" Value='<%# Bind("tturno") %>' />
                                        <asp:HiddenField ID="TINICIOHidden" runat="server" Value='<%# Bind("tinicio") %>' />
                                        <asp:HiddenField ID="TFINHidden" runat="server" Value='<%# Bind("tfin") %>' />
                                        <asp:HiddenField ID="IMPORTADORHidden" runat="server" Value='<%# Bind("importador") %>' />
                                        <asp:HiddenField ID="SNHidden" runat="server" Value='<%# Bind("sn") %>' />
                                        <asp:HiddenField ID="RUCHidden" runat="server" Value='<%# Bind("ruc") %>' />
                                        <asp:HiddenField ID="EMPRESAHidden" runat="server" Value='<%# Bind("empresa") %>' />
                                        <asp:HiddenField ID="PLACAHidden" runat="server" Value='<%# Bind("placa") %>' />
                                        <asp:HiddenField ID="LICENCIAHidden" runat="server" Value='<%# Bind("licencia") %>' />
                                        <asp:HiddenField ID="CONDUCTORHidden" runat="server" Value='<%# Bind("conductor") %>' />
                                        <asp:HiddenField ID="ITEMHidden" runat="server" Value='<%# Bind("item") %>' />
                                        <asp:HiddenField ID="PROVINCIAHidden" runat="server" Value='<%# Bind("provincia") %>' />
                                        <asp:HiddenField ID="NETOHidden" runat="server" Value='<%# Bind("neto") %>' />
                                        <asp:HiddenField ID="BRUTOHidden" runat="server" Value='<%# Bind("bruto") %>' />
                                        <asp:HiddenField ID="GKEYHidden" runat="server" Value='<%# Bind("gkey") %>' />
                                        <asp:HiddenField ID="TELEFONOHidden" runat="server" Value='<%# Bind("telefono") %>' />
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

    <script type="text/javascript">



</script>
</asp:Content>
