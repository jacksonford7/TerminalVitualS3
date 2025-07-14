<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="VBS_BAN_Reporte_Detalle_Inv.aspx.cs" Inherits="CSLSite.VBS_BAN_Reporte_Detalle_Inv" %>

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
                <li class="breadcrumb-item" id="opcion_principal" runat="server"><a href="#">VBS Banano</a></li>
                <li class="breadcrumb-item active" aria-current="page" id="sub_opcion" runat="server">GENERACIÓN DE REPORTE DE INVENTARIO DETALLADO</li>
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
              FILTRO DE REPORTE
        </div>

        <div class="row">

            <div class="form-group col-md-4"> 
                <label for="inputAddress">Bodega :<span style="color: #FF0000; font-weight: bold;"></span></label>
                <div class="d-flex">
                    <asp:DropDownList ID="cmbBodega" class="form-control" runat="server" Font-Size="Medium"  Font-Bold="true" ></asp:DropDownList>
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
            var txtLinea = document.getElementById('<%= Txtruc.ClientID %>').value;
            var cboBodega = document.getElementById("<%= cmbBodega.ClientID %>");
            var tipoCargaId = cboBodega.value;

          

            var xhr = new XMLHttpRequest();
            xhr.open('POST', 'VBS_BAN_Reporte_Detalle_Inv.aspx/ExportarExcel', true);
            xhr.setRequestHeader('Content-Type', 'application/json; charset=utf-8');
            xhr.responseType = 'blob';

            xhr.onload = function () {
                var fileName = "Reporte_Det_Inv_" + txtLinea +"_"+ tipoCargaId + ".xlsx";
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
                } else if (xhr.status === 204) {
                    alert("No hay datos disponibles para exportar.");
                }
                else {
                    console.log('Error al exportar el archivo Excel. Código de estado: ' + xhr.status);
                }
            };

            xhr.onerror = function () {
                console.log('Error al enviar la solicitud de exportación del archivo Excel.');
            };

            xhr.send(JSON.stringify({ fechaDesde: txtLinea, fechaHasta: tipoCargaId }));
        }
    </script>



</asp:Content>
