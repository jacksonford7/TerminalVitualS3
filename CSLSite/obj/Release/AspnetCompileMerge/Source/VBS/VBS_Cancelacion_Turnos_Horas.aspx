<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="VBS_Cancelacion_Turnos_Horas.aspx.cs" Inherits="CSLSite.VBS_Cancelacion_Turnos_Horas" %>

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
                <li class="breadcrumb-item" id="opcion_principal" runat="server"><a href="#">Cancelacion de citas Importación</a></li>
                <li class="breadcrumb-item active" aria-current="page" id="sub_opcion" runat="server">CANCELACION DE CITAS POR HORA</li>
            </ol>
        </nav>
    </div>

    <div class="dashboard-container p-4" id="cuerpo" runat="server">


            <div class="row"></div>
            <div class="row">
                <div class="form-group col-md-2">
                    <label for="inputEmail4">Fecha Desde :</label>
                    <div class="d-flex">
                        <asp:TextBox ID="TxtFechaDesdeInactivar" runat="server" class="datetimepicker form-control" MaxLength="10" placeholder="dd/MM/yyyy HH:mm " onkeypress="return soloLetras(event,'1234567890/:')"></asp:TextBox>
                    </div>
                </div>  
                <div class="form-group col-md-2">
                    <label for="inputEmail4">Fecha Hasta :</label>
                    <div class="d-flex">
                        <asp:TextBox ID="TxtFechaHastaInactivar" runat="server" class="datetimepicker form-control" MaxLength="10" placeholder="dd/MM/yyyy HH:mm " onkeypress="return soloLetras(event,'1234567890/:')"></asp:TextBox>
                    </div>
                </div>
                  <div class="form-group col-md-3">
                            <label for="inputZip">TIPO DE BLOQUES</label>
                            <asp:DropDownList runat="server" ID="cboBloque" AutoPostBack="false" class="form-control" >
                            </asp:DropDownList>

                        </div>
                

                <div class="form-group col-md-4">
                    <label for="inputZip">&nbsp;</label>
                    <div class="d-flex">
                        &nbsp;&nbsp;
                            <img alt="loading.." src="../lib/file-uploader/img/loading.gif" height="32px" width="32px" id="ImgCarga" class="nover" />
                        <asp:Button ID="BtnInactivar" runat="server" class="btn btn-primary" Text="Inactivar Horas" OnClientClick="InactivarHoras()" />

                    </div>
                </div>
            </div>

    </div>


    <!--common script for all pages-->
   

  

    <script type="text/javascript">

        $(document).ready(function () {
            $('.datetimepicker').datetimepicker().datetimepicker({ lang: 'es', step: 30, format: 'd/m/Y  H:i' });
        });

     

    </script>
   
    <script type="text/javascript">
        function InactivarHoras() {

            var fechaDesde = document.getElementById('<%= TxtFechaDesdeInactivar.ClientID %>').value;

            var fechaHasta = document.getElementById('<%= TxtFechaHastaInactivar.ClientID %>').value;
            var cboBloque = document.getElementById("<%= cboBloque.ClientID %>");
            var tipoCargas = cboBloque.options[cboBloque.selectedIndex].text;
            var cboBloqueId = cboBloque.value;

         

            const formatoEntrada = "DD/MM/YYYY HH:mm";

            // Convierte las fechas a objetos Moment.js
            const fechaDesdeMoment = moment(fechaDesde, formatoEntrada);
            const fechaHastaMoment = moment(fechaHasta, formatoEntrada);

            // Make the Ajax request to send the updated value
            $.ajax({
                url: "VBS_Cancelacion_Turnos_Horas.aspx/InactivarHorasImport",
                data: JSON.stringify({ fechaDesde: fechaDesdeMoment.format("YYYY-MM-DD HH:mm:ss"), fechaHasta: fechaHastaMoment.format("YYYY-MM-DD HH:mm:ss"), cboBloqueId: cboBloqueId }),
                contentType: "application/json; charset=utf-8",
                type: 'POST',
                success: function (response) {
                
                    if (response.d == "Succes") {
       
                        document.getElementById('<%= TxtFechaDesdeInactivar.ClientID %>').value = ''
                        document.getElementById('<%= TxtFechaHastaInactivar.ClientID %>').value = ''
                        mostrarExito("Horas inactivadas Exitosamente")
                    }

                },

                error: function (error) {
                    // Manejar el error
                }
            });
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


</asp:Content>
