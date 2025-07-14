<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="VBS_Exportar_Reporte_Expo.aspx.cs" Inherits="CSLSite.VBS_Exportar_Reporte_Expo" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="placehead" runat="server">


    <!-- Bootstrap core CSS -->


    <link href="../shared/estilo/jquery.datetimepicker.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/jquery.datetimepicker.js" type="text/javascript"></script>


    <script type="text/javascript">


        function fechas() {
            $(document).ready(function () {
                $('.datetimepicker').datetimepicker().datetimepicker({ lang: 'es', step: 30, format: 'd/m/Y H:i' });
            });

            $(document).ready(function () {
                $('.datetimepickerAlt').datetimepicker().datetimepicker({ lang: 'es', closeOnDateSelect: true, timepicker: false, step: 30, format: 'd/m/Y' });
            });

            $(document).ready(function () {
                $('.datepicker').datetimepicker().datetimepicker({ lang: 'es', timepicker: false, format: 'd/m/Y', closeOnDateSelect: true, minDate: '0' });

            });
        }


    </script>

</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="placebody" runat="server">

    <div class="mt-4">
        <nav class="mt-4" aria-label="breadcrumb">
            <ol class="breadcrumb">
                <li class="breadcrumb-item" id="opcion_principal" runat="server"><a href="#">Reporte de citas Expo</a></li>
                <li class="breadcrumb-item active" aria-current="page" id="sub_opcion" runat="server">GENERACIÓN DE REPORTE</li>
            </ol>
        </nav>
    </div>

    <div class="dashboard-container p-4" id="cuerpo" runat="server">


        <div class="row"></div>
        <div class="row">

            <div class="form-group col-md-2">
                <label for="inputEmail4">Fecha Desde:</label>
                <div class="d-flex">
                    <asp:TextBox ID="TxtFechaDesde" runat="server" class="datetimepickerAlt form-control" MaxLength="10" placeholder="dd/MM/yyyy " onkeypress="return soloLetras(event,'1234567890/:')"></asp:TextBox>
                </div>
            </div>

            <div class="form-group col-md-2">
                <label for="inputEmail4">Fecha Hasta:</label>
                <div class="d-flex">
                    <asp:TextBox ID="TxtFechaHasta" runat="server" class="datetimepickerAlt form-control" MaxLength="10" placeholder="dd/MM/yyyy " onkeypress="return soloLetras(event,'1234567890/:')"></asp:TextBox>
                </div>

            </div>

            <div class="form-group col-md-4">
                <label for="inputZip">&nbsp;</label>
                <div class="d-flex">
                    &nbsp;&nbsp;
                            <img alt="loading.." src="../lib/file-uploader/img/loading.gif" height="32px" width="32px" id="ImgCarga" class="nover" />
                    <asp:Button ID="BtnExportar" runat="server" class="btn btn-primary" Text="EXPORTAR" OnClientClick="exportar()" />

                </div>
            </div>
        </div>

    </div>


    <script type="text/javascript"></script>

    <!--common script for all pages-->

    <!--script for this page-->


    <script type="text/javascript">
        $(document).ready(function () {
            $('.datetimepicker').datetimepicker().datetimepicker({ lang: 'es', step: 30, format: 'd/m/Y H:i' });
        });
        $(document).ready(function () {
            $('.datetimepickerAlt').datetimepicker().datetimepicker({ lang: 'es', closeOnDateSelect: true, timepicker: false, step: 30, format: 'd/m/Y' });
        });

        $(document).ready(function () {
            $('.datepicker').datetimepicker().datetimepicker({ lang: 'es', timepicker: false, format: 'd/m/Y', closeOnDateSelect: true, minDate: '0' });

        });

    </script>


    <script type="text/javascript">

        function exportar() {
            var fechaDesde = document.getElementById('<%= TxtFechaDesde.ClientID %>').value;
            var fechaHasta = document.getElementById('<%= TxtFechaHasta.ClientID %>').value;

           <%-- var cboTipoCarga = document.getElementById("<%= cboTipoCarga.ClientID %>");
            var tipoCargaId = cboTipoCarga.value;--%>

          

            var xhr = new XMLHttpRequest();
            xhr.open('POST', 'VBS_Exportar_Reporte_Expo.aspx/ExportarExcel', true);
            xhr.setRequestHeader('Content-Type', 'application/json; charset=utf-8');
            xhr.responseType = 'blob';

            xhr.onload = function () {
                var fileName = "Reporte_" + fechaDesde + ".xlsx";
                if (xhr.status === 200) {
                    var blob = xhr.response;

                    // Configura la respuesta HTTP para la descarga del archivo
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

            xhr.send(JSON.stringify({ fechaDesde: fechaDesde, fechaHasta: fechaHasta }));
        }
    </script>



</asp:Content>
