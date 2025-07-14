<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="VBS_Cancelacion_Turnos_Expo.aspx.cs" Inherits="CSLSite.VBS_Cancelacion_Turnos_Expo" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="placehead" runat="server">


    
    <link href="../img/favicon2.png" rel="icon" />
    <link href="../img/icono.png" rel="apple-touch-icon" />
    <!-- Bootstrap core CSS -->


    <!--external css-->
    <link href="../lib/font-awesome/css/font-awesome.css" rel="stylesheet" />
    <link rel="stylesheet" type="text/css" href="../lib/gritter/css/jquery.gritter.css" />


    <!-- Incluye los estilos CSS -->
    <link href="../css/icons.css" rel="stylesheet" />
    <link href="../css/style.css" rel="stylesheet" />
    <link href="https://cdnjs.cloudflare.com/ajax/libs/fullcalendar/3.10.2/fullcalendar.min.css" rel="stylesheet" />

    <!-- Incluye los scripts JavaScript -->
    <script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/jquery/3.6.0/jquery.min.js"></script>
    <script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/moment.js/2.29.1/moment.min.js"></script>
    <script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/fullcalendar/3.10.2/fullcalendar.min.js"></script>
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/sweetalert2@11.0.19/dist/sweetalert2.min.css" />


    <link href="../shared/estilo/jquery.datetimepicker.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/jquery.datetimepicker.js" type="text/javascript"></script>

 
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/5.15.3/css/all.min.css"/>


</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="placebody" runat="server">
    <script type="text/javascript" src="https://cdn.jsdelivr.net/npm/sweetalert2@11.0.18/dist/sweetalert2.all.min.js"></script>
    <div class="mt-4">
        <nav class="mt-4" aria-label="breadcrumb">
            <ol class="breadcrumb">
                <li class="breadcrumb-item" id="opcion_principal" runat="server"><a href="#">Cancelacion de citas Exportación</a></li>
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
                        <asp:TextBox ID="TxtFechaDesdeInactivarExpo" runat="server" class="datetimepicker form-control" MaxLength="10" placeholder="dd/MM/yyyy HH:mm " onkeypress="return soloLetras(event,'1234567890/:')"></asp:TextBox>
                    </div>
                </div>  
                <div class="form-group col-md-2">
                    <label for="inputEmail4">Fecha Hasta :</label>
                    <div class="d-flex">
                        <asp:TextBox ID="TxtFechaHastaInactivarExpo" runat="server" class="datetimepicker form-control" MaxLength="10" placeholder="dd/MM/yyyy HH:mm " onkeypress="return soloLetras(event,'1234567890/:')"></asp:TextBox>
                    </div>
                </div>
                  <div class="form-group col-md-3">
                            <label for="inputZip">TIPO DE CARGAS</label>
                            <asp:DropDownList runat="server" ID="cboTipoCargaExpo" AutoPostBack="false" class="form-control" >
                            </asp:DropDownList>

                        </div>
                

                <div class="form-group col-md-4">
                    <label for="inputZip">&nbsp;</label>
                    <div class="d-flex">
                        &nbsp;&nbsp;
                            <img alt="loading.." src="../lib/file-uploader/img/loading.gif" height="32px" width="32px" id="ImgCarga" class="nover" />
                            <button class="btn btn-primary" id="BtnInactivarExpo" <%--onclick="enviarDatosAlServidor()"--%> type="button">Guardar</button>

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
            $("#BtnInactivarExpo").click(function () {
                InactivarHorasExpo();
            });
        });

    </script>
   
    <script type="text/javascript">
        function InactivarHorasExpo() {
         
            var fechaDesde = document.getElementById('<%= TxtFechaDesdeInactivarExpo.ClientID %>').value;

            var fechaHasta = document.getElementById('<%= TxtFechaHastaInactivarExpo.ClientID %>').value;
            var cboTipoCarga = document.getElementById("<%= cboTipoCargaExpo.ClientID %>");
            var cboTipoCargaID = cboTipoCarga.value;
            var cboTipoCargaTexto = cboTipoCarga.options[cboTipoCarga.selectedIndex].text;



            const formatoEntrada = "DD/MM/YYYY HH:mm";

            // Convierte las fechas a objetos Moment.js
            const fechaDesdeMoment = moment(fechaDesde, formatoEntrada);
            const fechaHastaMoment = moment(fechaHasta, formatoEntrada);

            const fechaDesdeFormat = fechaDesdeMoment.format("YYYY-MM-DD HH:mm:ss");
            const fechaHastaFormat = fechaHastaMoment.format("YYYY-MM-DD HH:mm:ss");
            const mensaje = `¿Deseas Inactivar las horas del día ${fechaDesdeFormat} Hasta: ${fechaHastaFormat} Para el Tipo De cargas: ${cboTipoCargaTexto}?`;

            mostrarConfirmacion(mensaje,
                function ()
                {
                    $.ajax({
                        url: "VBS_Cancelacion_Turnos_Expo.aspx/InactivarHorasExpo",
                        data: JSON.stringify({ fechaDesde: fechaDesdeMoment.format("YYYY-MM-DD HH:mm:ss"), fechaHasta: fechaHastaMoment.format("YYYY-MM-DD HH:mm:ss"), cboTipoCargaID: cboTipoCargaID }),
                        contentType: "application/json; charset=utf-8",
                        type: 'POST',
                        success: function (response) {

                            if (response.d == "Succes") {

                                document.getElementById('<%= TxtFechaDesdeInactivarExpo.ClientID %>').value = ''
                                          document.getElementById('<%= TxtFechaHastaInactivarExpo.ClientID %>').value = ''
                                          mostrarExito("Horas inactivadas Exitosamente")
                                      }

                                  },

                                  error: function (error) {

                                  }
                              });

                }
            );
  
        }
        function ejecutarAccion() {
            // Aquí puedes realizar la acción que deseas ejecutar después de la confirmación
            console.log("Acción ejecutada después de confirmación.");
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
