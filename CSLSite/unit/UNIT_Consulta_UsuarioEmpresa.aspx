<%@ Page Title="" Language="C#" MasterPageFile="~/site.Master" AutoEventWireup="true" CodeBehind="UNIT_Consulta_UsuarioEmpresa.aspx.cs" Inherits="CSLSite.unit.UNIT_Consulta_UsuarioEmpresa" %>

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

    <script type="text/javascript">

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
        function mostrarExito(mensaje, callback) {
            Swal.fire({
                title: "Éxito",
                text: mensaje,
                icon: "success",
                iconColor: "#E23B1B",
                confirmButtonText: "Aceptar",
                confirmButtonColor: "#E23B1B"
            }).then((result) => {
                if (result.isConfirmed) {
                    callback();
                }
            });
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

        .form-container {
            width: 100%;
            padding: 20px;
        }

        .form-box {
            border: 1px solid #ddd;
            padding: 20px;
            margin-bottom: 20px;
            border-radius: 6px;
            background-color: #fff;
        }

        .form-title h4 {
            color: #d00000;
            font-weight: bold;
            margin-bottom: 20px;
        }

        .badge-section {
            background-color: #d00000;
            color: white;
            padding: 5px 12px;
            font-weight: bold;
            border-radius: 50%;
            margin-right: 10px;
        }

        label {
            font-size: 14px;
        }

        .form-control {
            font-size: 14px;
            height: 35px;
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
        CONSULTA DE EMPRESA
    </div>

    <br />

    <div class="row">
        <div class="col-md-12">
            <asp:UpdatePanel ID="UPDETALLE" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="container">

                        <!-- Sección 1: Tipo de Cliente -->
                        <div class="border p-3 mb-3">
                            <h5><span class="badge bg-danger text-white">1</span> Tipo de Cliente - Empresa</h5>
                            <label><strong>1. Tipo de Cliente: <span class="text-danger">*</span></strong></label><br />
                            <div class="form-check form-check-inline">
                                <asp:CheckBox ID="chkExportador" runat="server" CssClass="form-check-input" />
                                <label class="form-check-label" for="chkExportador">EXPORTADOR</label>
                            </div>
                            <div class="form-check form-check-inline">
                                <asp:CheckBox ID="chkImportador" runat="server" CssClass="form-check-input" />
                                <label class="form-check-label" for="chkImportador">IMPORTADOR</label>
                            </div>
                            <div class="form-check form-check-inline">
                                <asp:CheckBox ID="chkOperador" runat="server" CssClass="form-check-input" />
                                <label class="form-check-label" for="chkOperador">OPERADOR PORTUARIO</label>
                            </div>
                            <div class="form-check form-check-inline">
                                <asp:CheckBox ID="chkProveedor" runat="server" CssClass="form-check-input" />
                                <label class="form-check-label" for="chkProveedor">PROVEEDOR CGSA</label>
                            </div>
                        </div>

                        <!-- Sección 2: Información del Cliente -->
                        <div class="border p-3 mb-3">
                            <h5><span class="badge bg-danger text-white">2</span> Información del Cliente</h5>
                            <div class="row mb-2">
                                <div class="col-md-6">
                                    <label>2. Nombre/Razón Social: <span class="text-danger">*</span></label>
                                    <asp:TextBox ID="txtRazonSocial" runat="server" CssClass="form-control" />
                                </div>
                                <div class="col-md-6">
                                    <label>3. RUC / Cédula / Pasaporte: <span class="text-danger">*</span></label>
                                    <div class="d-flex flex-wrap align-items-center gap-3 mb-2 mt-1">
                                        <div class="form-check form-check-inline">
                                            <asp:CheckBox ID="chkRuc" runat="server" CssClass="form-check-input" />
                                            <label class="form-check-label" for="chkRuc">RUC</label>
                                        </div>
                                        <div class="form-check form-check-inline">
                                            <asp:CheckBox ID="chkCedula" runat="server" CssClass="form-check-input" />
                                            <label class="form-check-label" for="chkCedula">Cédula</label>
                                        </div>
                                        <div class="form-check form-check-inline">
                                            <asp:CheckBox ID="chkPasaporte" runat="server" CssClass="form-check-input" />
                                            <label class="form-check-label" for="chkPasaporte">Pasaporte</label>
                                        </div>
                                    </div>
                                    <asp:TextBox ID="txtIdentificacion" runat="server" CssClass="form-control" />
                                </div>
                            </div>
                            <div class="row mb-2">
                                <div class="col-md-6">
                                    <label>4. Actividad Comercial: <span class="text-danger">*</span></label>
                                    <asp:TextBox ID="txtActividadComercial" runat="server" CssClass="form-control" />
                                </div>
                                <div class="col-md-6">
                                    <label>5. Dirección Oficina: <span class="text-danger">*</span></label>
                                    <asp:TextBox ID="txtDireccionOficina" runat="server" CssClass="form-control" />
                                </div>
                            </div>
                            <div class="row mb-2">
                                <div class="col-md-6">
                                    <label>6. Teléfono Oficina:</label>
                                    <asp:TextBox ID="txtTelefonoOficina" runat="server" CssClass="form-control" />
                                </div>
                                <div class="col-md-6">
                                    <label>7. Persona Contacto: <span class="text-danger">*</span></label>
                                    <asp:TextBox ID="txtPersonaContacto" runat="server" CssClass="form-control" />
                                </div>
                            </div>
                            <div class="row mb-2">
                                <div class="col-md-6">
                                    <label>8. Celular Contacto: <span class="text-danger">*</span></label>
                                    <asp:TextBox ID="txtCelularContacto" runat="server" CssClass="form-control" />
                                </div>
                                <div class="col-md-6">
                                    <label>9. Mail Contacto: <span class="text-danger">*</span></label>
                                    <asp:TextBox ID="txtMailContacto" runat="server" CssClass="form-control" TextMode="Email" />
                                </div>
                            </div>
                            <div class="row mb-2">
                                <div class="col-md-6">
                                    <label>10. Mail Ebilling:</label>
                                    <asp:TextBox ID="txtMailEbilling" runat="server" CssClass="form-control" TextMode="Email" />
                                </div>
                                <div class="col-md-6">
                                    <label>11. Certificaciones:</label>
                                    <asp:TextBox ID="txtCertificaciones" runat="server" CssClass="form-control" placeholder="Sepárelas con punto y coma." />
                                </div>
                            </div>
                            <div class="row mb-2">
                                <div class="col-md-6">
                                    <label>12. Sitio Web:</label>
                                    <asp:TextBox ID="txtSitioWeb" runat="server" CssClass="form-control" placeholder="http://www.dominio.com" />
                                </div>
                                <div class="col-md-6">
                                    <label>13. Afiliación a Gremios:</label>
                                    <asp:TextBox ID="txtGremios" runat="server" CssClass="form-control" placeholder="Sepárelos con punto y coma." />
                                </div>
                            </div>
                            <div class="row mb-2">
                                <div class="col-md-12">
                                    <label>14. Referencia Comercial:</label>
                                    <asp:TextBox ID="txtReferenciaComercial" runat="server" CssClass="form-control" placeholder="Sepárelas con punto y coma." />
                                </div>
                            </div>
                        </div>

                        <!-- Sección 3: Representante Legal -->
                        <div class="border p-3 mb-3">
                            <h5><span class="badge bg-danger text-white">3</span> Información del Representante Legal</h5>
                            <div class="row mb-2">
                                <div class="col-md-6">
                                    <label>15. Representante Legal: <span class="text-danger">*</span></label>
                                    <asp:TextBox ID="txtRepresentanteLegal" runat="server" CssClass="form-control" />
                                </div>
                                <div class="col-md-6">
                                    <label>16. Teléfono Domicilio: <span class="text-danger">*</span></label>
                                    <asp:TextBox ID="txtTelefonoDomicilio" runat="server" CssClass="form-control" />
                                </div>
                            </div>
                            <div class="row mb-2">
                                <div class="col-md-6">
                                    <label>17. Dirección Domiciliaria: <span class="text-danger">*</span></label>
                                    <asp:TextBox ID="txtDireccionDomiciliaria" runat="server" CssClass="form-control" />
                                </div>
                                <div class="col-md-6">
                                    <label>18. Cédula/Pasaporte:</label>
                                    <div class="d-flex flex-wrap align-items-center gap-3 mb-2 mt-1">
                                        <div class="form-check form-check-inline">
                                            <asp:CheckBox ID="chkCedulaRep" runat="server" CssClass="form-check-input" />
                                            <label class="form-check-label" for="chkCedulaRep">Cédula</label>
                                        </div>
                                        <div class="form-check form-check-inline">
                                            <asp:CheckBox ID="chkPasaporteRep" runat="server" CssClass="form-check-input" />
                                            <label class="form-check-label" for="chkPasaporteRep">Pasaporte</label>
                                        </div>
                                    </div>
                                    <asp:TextBox ID="txtIdentificacionRep" runat="server" CssClass="form-control" />
                                </div>
                            </div>
                            <div class="row mb-2">
                                <div class="col-md-12">
                                    <label>19. Mail: <span class="text-danger">*</span></label>
                                    <asp:TextBox ID="txtMailRepresentante" runat="server" CssClass="form-control" TextMode="Email" />
                                </div>
                            </div>
                        </div>


                        <!-- Aceptación y CAPTCHA -->
                        <div class="form-group">
                            <div class="alert alert-danger" role="alert">
                                Declaro que los datos consignados y suministrados en el presente documento son correctos y de procedencia lícita. Autorizo a CONTECON GUAYAQUIL S.A. a solicitar confirmación de los mismos...
                            </div>

                        </div>

                        <div class="text-center mt-3">
                            <asp:Button ID="btnEnviarSolicitud" runat="server" OnClick="btnEnviarSolicitud_Click" Text="Enviar Solicitud" CssClass="btn btn-danger" />
                        </div>

                    </div>
                    <div class="row">
                        <div class="col-md-12 d-flex justify-content-center">
                            <div class="alert alert-danger" id="banmsg" runat="server" clientidmode="Static">
                                <b>Error!</b> Debe ingresar el número de la carga MRN...
                            </div>
                        </div>
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
