<%@ Page Title="" Language="C#" MasterPageFile="~/site.Master" AutoEventWireup="true" CodeBehind="DespachoVehiculos.aspx.cs" Inherits="CSLSite.DespachoVehiculos" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="placehead" runat="server">
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

    <link href="../lib/advanced-datatable/css/demo_page.css" rel="stylesheet" />
    <link href="../lib/advanced-datatable/css/demo_table.css" rel="stylesheet" />
    <link rel="stylesheet" href="../lib/advanced-datatable/css/DT_bootstrap2.css" />

    <style type="text/css">
        body
        {
            /*font-family: Arial;*/
            /*font-size: 10pt;*/
        }
        .modalBackground
        {
            background-color: Black;
            filter: alpha(opacity=60);
            opacity: 0.6;
        }
        .modalPopup
        {
            background-color: #FFFFFF;
            width: 726px;
            border: 3px solid #FF3720;
            padding: 0;
        }
        .modalPopup .header
        {
            background-color: #2FBDF1;
            height: 30px;
            color: White;
            line-height: 30px;
            text-align: center;
            font-weight: bold;
        }
        .modalPopup .body
        {
            min-height: 50px;
            line-height: 25px;
            text-align: center;
            /*font-weight: bold;*/
            margin-bottom: 5px;
        }
        .form-title
        {
            color: #d14124 !important;
        }
        .section-title
        {
            color: #d14124;
            font-weight: bold;
            text-transform: uppercase;
            width: 100%;
            border-bottom: 1px solid rgba(74, 74, 70, 0.1);
            padding-bottom: 14px;
            margin: 50px 0 25px 0;
        }

        .btn-buscar
        {
            background-color: #EF6C00;
            border-color: #EF6C00;
            color: #fff;
        }
    </style>

    <script type="text/javascript">
        $(document).ready(function () {
            $('[data-toggle="tooltip"]').tooltip();
        });
    </script>

    <script type="text/javascript">
        $(document).ready(function () {
            $('.datetimepicker').datetimepicker().datetimepicker({ lang: 'es', timepicker: true, step: 30, format: 'm/d/Y H:i' });
        });
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="placebody" runat="server">
    <asp:ToolkitScriptManager ID="tkscata" runat="server" EnablePageMethods="True" ScriptMode="Release" />

    <div class="mt-4">
        <nav class="mt-4" aria-label="breadcrumb">
            <ol class="breadcrumb">
                <li class="breadcrumb-item" id="opcion_principal" runat="server"><a href="#">Despacho Vehículos</a></li>
                <li class="breadcrumb-item active" aria-current="page" id="sub_opcion" runat="server">DESPACHO DE VEHÍCULOS</li>
            </ol>
        </nav>
    </div>

    <div class="dashboard-container p-4">
        <div class="form-title">DESPACHO DE VEHÍCULOS</div>

        <asp:UpdatePanel ID="UPMENSAJE" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:Label ID="lblMensaje" runat="server" CssClass="alert alert-info" Visible="false" />
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="BtnBuscar" />
                <asp:AsyncPostBackTrigger ControlID="BtnGrabar" />
                <asp:AsyncPostBackTrigger ControlID="gvContenedoresVHS" EventName="RowCommand" />
            </Triggers>
        </asp:UpdatePanel>

        <div class="section-title">DATOS DE LA CARGA</div>

        <asp:UpdatePanel ID="UPBUSCAR" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="form-row">
                    <div class="form-group col-md-4">
                        <label for="TXTMRN">MRN<span style="color: #FF0000; font-weight: bold;">*</span></label>
                        <asp:TextBox ID="TXTMRN" runat="server" CssClass="form-control" placeholder="MRN" />
                    </div>
                    <div class="form-group col-md-2">
                        <label for="TXTMSN">MSN<span style="color: #FF0000; font-weight: bold;">*</span></label>
                        <asp:TextBox ID="TXTMSN" runat="server" CssClass="form-control" placeholder="MSN" />
                    </div>
                    <div class="form-group col-md-2">
                        <label for="TXTHSN">HSN<span style="color: #FF0000; font-weight: bold;">*</span></label>
                        <asp:TextBox ID="TXTHSN" runat="server" CssClass="form-control" placeholder="HSN" />
                    </div>
                    <div class="form-group col-md-2">
                        <label for="BtnBuscar">&nbsp;</label>
                        <div class="d-flex justify-content-end">
                            <asp:Button ID="BtnBuscar" runat="server" CssClass="btn btn-buscar" Text="BUSCAR" OnClick="BtnBuscar_Click" />
                        </div>
                    </div>
                </div>
                <br />
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="BtnBuscar" />
            </Triggers>
        </asp:UpdatePanel>

        <div class="section-title">DATOS DE LA FACTURA</div>

        <asp:UpdatePanel ID="UPDETALLE" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="section-title">DETALLE DE LA CARGA</div>

                <asp:GridView ID="gvContenedores" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered invoice" GridLines="None">
                    <RowStyle BackColor="#F0F0F0" />
                    <AlternatingRowStyle BackColor="#FFFFFF" />
                    <Columns>
                        <asp:BoundField DataField="SECUENCIA" HeaderText="#" HeaderStyle-HorizontalAlign="Center" />
                        <asp:TemplateField HeaderText="FA">
                            <ItemTemplate><asp:CheckBox ID="CHKFA" runat="server" Checked='<%# Bind("VISTO") %>' /></ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="CONTENEDOR" HeaderText="CONTENEDOR" />
                        <asp:BoundField DataField="FECHA_HASTA" HeaderText="FECHA HASTA" DataFormatString="{0:yyyy/MM/dd HH:mm}" />
                        <asp:BoundField DataField="DOCUMENTO" HeaderText="DOCUMENTO" />
                        <asp:BoundField DataField="DESCRIPCION" HeaderText="MANIFIESTO" />
                        <asp:TemplateField Visible="false"><ItemTemplate><asp:Label ID="lblMRN" runat="server" Text='<%# Bind("MRN") %>' /></ItemTemplate></asp:TemplateField>
                        <asp:TemplateField Visible="false"><ItemTemplate><asp:Label ID="lblMSN" runat="server" Text='<%# Bind("MSN") %>' /></ItemTemplate></asp:TemplateField>
                        <asp:TemplateField Visible="false"><ItemTemplate><asp:Label ID="lblHSN" runat="server" Text='<%# Bind("HSN") %>' /></ItemTemplate></asp:TemplateField>
                        <asp:TemplateField Visible="false"><ItemTemplate><asp:Label ID="lblIdentificadorUnico" runat="server" Text='<%# Bind("GKEY") %>' /></ItemTemplate></asp:TemplateField>
                        <asp:TemplateField Visible="false"><ItemTemplate><asp:Label ID="lblNombreNave" runat="server" Text='<%# Bind("CNTR_VEPR_VSSL_NAME") %>' /></ItemTemplate></asp:TemplateField>
                        <asp:TemplateField Visible="false"><ItemTemplate><asp:Label ID="lblClienteID" runat="server" Text='<%# Bind("ClienteID") %>' /></ItemTemplate></asp:TemplateField>
                        <asp:TemplateField Visible="false"><ItemTemplate><asp:Label ID="lblLinea" runat="server" Text='<%# Bind("LINEA") %>' /></ItemTemplate></asp:TemplateField>
                        <asp:TemplateField Visible="false"><ItemTemplate><asp:Label ID="lblTamano" runat="server" Text='<%# Bind("TAMANO") %>' /></ItemTemplate></asp:TemplateField>
                        <asp:TemplateField Visible="false"><ItemTemplate><asp:Label ID="lblImportadorID" runat="server" Text='<%# Bind("ImportadorID") %>' /></ItemTemplate></asp:TemplateField>
                        <asp:TemplateField Visible="false"><ItemTemplate><asp:Label ID="lblImportadorNombre" runat="server" Text='<%# Bind("ImportadorNombre") %>' /></ItemTemplate></asp:TemplateField>
                        <asp:TemplateField Visible="false"><ItemTemplate><asp:Label ID="lblTipoContenedor" runat="server" Text='<%# Bind("TIPO") %>' /></ItemTemplate></asp:TemplateField>
                        <asp:TemplateField Visible="false"><ItemTemplate><asp:Label ID="lblBultos" runat="server" Text='<%# Bind("CANTIDAD") %>' /></ItemTemplate></asp:TemplateField>
                        <asp:TemplateField Visible="false"><ItemTemplate><asp:Label ID="lblRIDT" runat="server" Text='<%# Bind("RIDT") %>' /></ItemTemplate></asp:TemplateField>
                        <asp:TemplateField Visible="false"><ItemTemplate><asp:Label ID="lblManifiesto" runat="server" Text='<%# Bind("DESCRIPCION") %>' /></ItemTemplate></asp:TemplateField>
                        <asp:TemplateField Visible="false"><ItemTemplate><asp:Label ID="lblBL" runat="server" Text='<%# Bind("DOCUMENTO") %>' /></ItemTemplate></asp:TemplateField>
                        <asp:TemplateField Visible="false"><ItemTemplate><asp:Label ID="lblBuque" runat="server" Text='<%# Bind("CNTR_VEPR_VOYAGE") %>' /></ItemTemplate></asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <asp:Panel ID="panelContenedoresVHS" runat="server" Visible="false">
                    <div class="section-title">DETALLE DE CONTENEDORES</div>

                    <asp:GridView ID="gvContenedoresVHS" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered invoice" GridLines="None"
                        OnRowCommand="gvContenedoresVHS_RowCommand">
                        <RowStyle BackColor="#F0F0F0" />
                        <AlternatingRowStyle BackColor="#FFFFFF" />
                        <Columns>
                            <asp:BoundField DataField="ContenedorID" HeaderText="ID" />
                            <asp:BoundField DataField="NumeroContenedor" HeaderText="Contenedor" />
                            <asp:BoundField DataField="manifiesto" HeaderText="Manifiesto" />
                            <asp:TemplateField HeaderText="Acciones">
                                <ItemTemplate>
                                    <asp:Button ID="btnEliminar" runat="server" Text="Eliminar" CommandName="Eliminar" CommandArgument='<%# Eval("ContenedorID") %>' CssClass="btn btn-primary btn-sm" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </asp:Panel>

                <asp:Button ID="BtnGrabar" runat="server" CssClass="btn btn-primary" Text="GRABAR" OnClick="BtnGrabar_Click" />
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="BtnBuscar" />
                <asp:AsyncPostBackTrigger ControlID="BtnGrabar" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
</asp:Content>
