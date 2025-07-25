﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="VBS_Duplicar_Turnos_Expo_Vacios.aspx.cs" Inherits="CSLSite.VBS_Duplicar_Turnos_Expo_Vacios" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="placehead" runat="server">



    <script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/moment.js/2.29.1/moment.min.js"></script>
 <link href="../shared/estilo/jquery.datetimepicker.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/jquery.datetimepicker.js" type="text/javascript"></script>
    <link href="https://cdn.jsdelivr.net/npm/sweetalert2@11.0.18/dist/sweetalert2.min.css" rel="stylesheet" />


</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="placebody" runat="server">
    <script type="text/javascript" src="https://cdn.jsdelivr.net/npm/sweetalert2@11.0.18/dist/sweetalert2.all.min.js"></script>
    <div class="mt-4">
        <nav class="mt-4" aria-label="breadcrumb">
            <ol class="breadcrumb">
                <li class="breadcrumb-item" id="opcion_principal" runat="server"><a href="#">Copiar Turnos de Expo. Vacíos</a></li>
                <li class="breadcrumb-item active" aria-current="page" id="sub_opcion" runat="server">COPIAR CITAS POR HORA</li>
            </ol>
        </nav>
    </div>

    <div class="dashboard-container p-4" id="cuerpo" runat="server">


        <div class="row"></div>
        <div class="row">
            <div class="form-group col-md-2">
                <label for="inputEmail4">Fecha Origen :</label>
                <div class="d-flex">
                    <asp:TextBox ID="TxtFechaDesdeInactivar" runat="server" class="datetimepicker form-control" MaxLength="10" placeholder="dd/MM/yyyy HH:mm" onkeypress="return soloLetras(event,'1234567890/:')"></asp:TextBox>
                </div>
            </div>
            <div class="form-group col-md-2">
                <label for="inputEmail4">Fecha Destino :</label>
                <div class="d-flex">
                    <asp:TextBox ID="TxtFechaHastaInactivar" runat="server" class="datetimepicker form-control" MaxLength="10" placeholder="dd/MM/yyyy HH:mm" onkeypress="return soloLetras(event,'1234567890/:')"></asp:TextBox>
                </div>
            </div>
            <div class="form-group col-md-3">
                <label for="inputZip">LÍNEA NAVIERA</label>
                <asp:DropDownList runat="server" ID="cboBloque" AutoPostBack="false" class="form-control">
                </asp:DropDownList>

            </div>


            <div class="form-group col-md-4">
                <label for="inputZip">&nbsp;</label>
                <div class="d-flex">
                    &nbsp;&nbsp;
                            <img alt="loading.." src="../lib/file-uploader/img/loading.gif" height="32px" width="32px" id="ImgCarga" class="nover" />

                       <button class="btn btn-primary" id="BtnInactivar" <%--onclick="enviarDatosAlServidor()"--%> type="button">Guardar</button>

                </div>
            </div>
        </div>

    </div>


    <script type="text/javascript">

        $(document).ready(function () {
            $('.datetimepicker').datetimepicker().datetimepicker({ lang: 'es', step: 60, format: 'd/m/Y  H:i' });
        });

        $(document).ready(function () {
            // Manejar el evento de clic en el botón "Guardar"
            $("#BtnInactivar").click(function () {
                InactivarHoras();
            });
        });

    </script>

    <script type="text/javascript">

        function InactivarHoras() {

            var fechaDesde = document.getElementById('<%= TxtFechaDesdeInactivar.ClientID %>').value;

            var fechaHasta = document.getElementById('<%= TxtFechaHastaInactivar.ClientID %>').value;
            var cboBloque = document.getElementById("<%= cboBloque.ClientID %>");
            var tipoCargas = cboBloque.options[cboBloque.selectedIndex].text;
            var cboBloqueId = cboBloque.value;

            var cboBloqueTexto = cboBloque.options[cboBloque.selectedIndex].text;

            const formatoEntrada = "DD/MM/YYYY HH:mm";

            // Convierte las fechas a objetos Moment.js
            const fechaDesdeMoment = moment(fechaDesde, formatoEntrada);
            const fechaHastaMoment = moment(fechaHasta, formatoEntrada);

            const fechaDesdeFormat = fechaDesdeMoment.format("YYYY-MM-DD HH:mm:ss");
            const fechaHastaFormat = fechaHastaMoment.format("YYYY-MM-DD HH:mm:ss");
            const mensaje = `¿Deseas Copiar las horas del día ${fechaDesdeFormat} Hasta: ${fechaHastaFormat} Para la Línea Naviera: ${cboBloqueTexto}?`;


            mostrarConfirmacion(mensaje,
                function () {

                    $.ajax({
                        url: "VBS_Duplicar_Turnos_Expo_Vacios.aspx/DuplicarHorasExpoLineas",
                        data: JSON.stringify({ fechaDesde: fechaDesdeMoment.format("YYYY-MM-DD HH:mm:ss"), fechaHasta: fechaHastaMoment.format("YYYY-MM-DD HH:mm:ss"), cboBloqueId: cboBloqueId }),
                        contentType: "application/json; charset=utf-8",
                        type: 'POST',
                        success: function (response) {

                            if (response.d == "Succes") {

                                document.getElementById('<%= TxtFechaDesdeInactivar.ClientID %>').value = ''
                                document.getElementById('<%= TxtFechaHastaInactivar.ClientID %>').value = ''
                                mostrarExito("Horas Clonadas Exitosamente")
                            }

                        },

                        error: function (error) {

                        }
                    });

                }
            );







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
        function mostrarConfirmacion(mensaje, callback) {
            Swal.fire({
                title: "Confirmación",
                text: mensaje,
                icon: "question",
                iconColor: "#E23B1B",
                showCancelButton: true,
                confirmButtonText: "Sí",
                confirmButtonColor: "#E23B1B",
                cancelButtonText: "No",
                cancelButtonColor: "#E23B1B"
            }).then((result) => {
                if (result.isConfirmed) {
                    callback();
                }
            });
        }

    </script>


</asp:Content>
