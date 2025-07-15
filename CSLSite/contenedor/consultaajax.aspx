<%@ Page Title="Consulta Contenedores" Language="C#" MasterPageFile="~/site.Master" AutoEventWireup="true" CodeBehind="consultaajax.aspx.cs" Inherits="CSLSite.consultaajax" %>
<asp:Content ID="Content1" ContentPlaceHolderID="placehead" runat="server">
    <script src="../Scripts/jquery-3.7.1.min.js"></script>
    <script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/moment.js/2.29.1/moment.min.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="placebody" runat="server">
    <div class="container mt-4">
        <div class="form-row">
            <div class="form-group col-md-3">
                <label>MRN</label>
                <input type="text" id="txtMrn" class="form-control" maxlength="16" />
            </div>
            <div class="form-group col-md-3">
                <label>MSN</label>
                <input type="text" id="txtMsn" class="form-control" maxlength="4" />
            </div>
            <div class="form-group col-md-3">
                <label>HSN</label>
                <input type="text" id="txtHsn" class="form-control" maxlength="4" />
            </div>
            <div class="form-group col-md-3 align-self-end">
                <button type="button" class="btn btn-primary" id="btnBuscar">BUSCAR</button>
            </div>
        </div>
        <table class="table table-bordered" id="tablaContenedores">
            <thead>
                <tr>
                    <th>#</th>
                    <th>FA</th>
                    <th>CONTENEDOR</th>
                    <th>FECHA HASTA</th>
                    <th>TURNO REFERENCIAL</th>
                    <th>ESTADO</th>
                    <th>DOCUMENTO</th>
                    <th>ULTIMA FACTURA</th>
                    <th>NUMERO FACTURA</th>
                    <th>FECHA CAS</th>
                </tr>
            </thead>
            <tbody></tbody>
        </table>
    </div>
    <script type="text/javascript">
        $(function () {
            $('#btnBuscar').on('click', function () {
                var mrn = $('#txtMrn').val();
                var msn = $('#txtMsn').val();
                var hsn = $('#txtHsn').val();
                $.getJSON('../api/contenedor/detalle', { mrn: mrn, msn: msn, hsn: hsn })
                    .done(function (data) {
                        var tbody = $('#tablaContenedores tbody');
                        tbody.empty();
                        $.each(data, function (i, item) {
                            var row = $('<tr>');
                            row.append($('<td>').text(i + 1));
                            row.append($('<td>').html('<input type="checkbox" />'));
                            row.append($('<td>').text(item.Contenedor));
                            row.append($('<td>').text(item.FechaHasta ? moment(item.FechaHasta).format('YYYY/MM/DD') : ''));
                            var select = $('<select class="form-control"><option value="No">No</option><option value="Si">Si</option></select>');
                            row.append($('<td>').append(select));
                            row.append($('<td>').text(item.Estado));
                            row.append($('<td>').text(item.Documento));
                            row.append($('<td>').text(item.UltimaFactura ? moment(item.UltimaFactura).format('YYYY/MM/DD') : ''));
                            row.append($('<td>').text(item.NumeroFactura));
                            row.append($('<td>').text(item.FechaCas ? moment(item.FechaCas).format('YYYY/MM/DD') : ''));
                            tbody.append(row);
                        });
                    });
            });
        });
    </script>
</asp:Content>
