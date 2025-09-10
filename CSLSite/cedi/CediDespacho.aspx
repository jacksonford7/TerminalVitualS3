<%@ Page Title="" Language="C#" MasterPageFile="~/site.Master" AutoEventWireup="true" CodeBehind="CediDespacho.aspx.cs" Inherits="CSLSite.Cedi.CediDespacho" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="placehead" runat="server">
    <!-- Estilos y scripts compartidos replicados del módulo de Vehículos -->
    <link href="../css/bootstrap.min.css" rel="stylesheet" />
    <link href="../css/dashboard.css" rel="stylesheet" />
    <script src="../Scripts/jquery.datetimepicker.js" type="text/javascript"></script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="placebody" runat="server">
    <asp:ToolkitScriptManager ID="CediScript" runat="server" EnablePageMethods="True" ScriptMode="Release" />

    <div class="mt-4">
        <nav class="mt-4" aria-label="breadcrumb">
            <ol class="breadcrumb">
                <li class="breadcrumb-item"><a href="#">Despacho CEDI</a></li>
                <li class="breadcrumb-item active" aria-current="page">DESPACHO CEDI</li>
            </ol>
        </nav>
    </div>

    <div class="dashboard-container p-4">
        <div class="form-title">DESPACHO CEDI</div>

        <asp:UpdatePanel ID="UPCediMensaje" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:Label ID="lblCediMensaje" runat="server" CssClass="alert alert-info" Visible="false" />
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="BtnCediBuscar" />
                <asp:AsyncPostBackTrigger ControlID="BtnCediExportar" />
                <asp:AsyncPostBackTrigger ControlID="gvCediContenedores" EventName="RowCommand" />
            </Triggers>
        </asp:UpdatePanel>

        <!-- Filtros de búsqueda -->
        <asp:UpdatePanel ID="UPCediBuscar" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="form-row">
                    <div class="form-group col-md-4">
                        <label for="TXTCediMRN">MRN</label>
                        <asp:TextBox ID="TXTCediMRN" runat="server" CssClass="form-control" />
                    </div>
                    <div class="form-group col-md-2">
                        <label for="TXTCediMSN">MSN</label>
                        <asp:TextBox ID="TXTCediMSN" runat="server" CssClass="form-control" />
                    </div>
                    <div class="form-group col-md-2">
                        <label for="TXTCediHSN">HSN</label>
                        <asp:TextBox ID="TXTCediHSN" runat="server" CssClass="form-control" />
                    </div>
                    <div class="form-group col-md-2">
                        <label for="BtnCediBuscar">&nbsp;</label>
                        <div class="d-flex justify-content-end">
                            <asp:Button ID="BtnCediBuscar" runat="server" CssClass="btn btn-buscar" Text="BUSCAR" OnClick="BtnCediBuscar_Click" />
                        </div>
                    </div>
                    <div class="form-group col-md-2">
                        <label for="BtnCediLimpiar">&nbsp;</label>
                        <div class="d-flex justify-content-end">
                            <asp:Button ID="BtnCediLimpiar" runat="server" CssClass="btn btn-secondary" Text="LIMPIAR" OnClick="BtnCediLimpiar_Click" />
                        </div>
                    </div>
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="BtnCediBuscar" />
            </Triggers>
        </asp:UpdatePanel>

        <!-- Detalle de resultados -->
        <div class="section-title">DETALLE DE LA CARGA</div>
        <asp:UpdatePanel ID="UPCediDetalle" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:GridView ID="gvCediContenedores" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered invoice" GridLines="None" OnRowCommand="gvCediContenedores_RowCommand">
                    <Columns>
                        <asp:BoundField DataField="Secuencia" HeaderText="#" />
                        <asp:BoundField DataField="Contenedor" HeaderText="CONTENEDOR" />
                        <asp:TemplateField HeaderText="Acciones">
                            <ItemTemplate>
                                <asp:Button ID="btnCediAsignar" runat="server" Text="Asignar" CommandName="Asignar" CommandArgument='<%# Eval("Id") %>' CssClass="btn btn-primary btn-sm" />
                                <asp:Button ID="btnCediDespachar" runat="server" Text="Despachar" CommandName="Despachar" CommandArgument='<%# Eval("Id") %>' CssClass="btn btn-success btn-sm" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <asp:Button ID="BtnCediExportar" runat="server" CssClass="btn btn-primary" Text="EXPORTAR" OnClick="BtnCediExportar_Click" />
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="BtnCediBuscar" />
                <asp:AsyncPostBackTrigger ControlID="BtnCediExportar" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
</asp:Content>
