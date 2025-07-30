<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="imprimirpasecontenedordespacho.aspx.cs" Inherits="CSLSite.imprimirpasecontenedordespacho" MasterPageFile="~/site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="placehead" runat="server">
    <title>Imprimir Pase Despacho</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="placebody" runat="server">
    <asp:Label ID="LblMensaje" runat="server" CssClass="alert alert-warning" Visible="false"></asp:Label>
    <asp:GridView ID="GridPreview" runat="server" AutoGenerateColumns="false" CssClass="table table-bordered mt-3">
        <Columns>
            <asp:BoundField DataField="Chofer" HeaderText="Chofer" />
            <asp:BoundField DataField="Placa" HeaderText="Placa" />
            <asp:BoundField DataField="Producto" HeaderText="Producto" />
            <asp:BoundField DataField="FechaRegistro" HeaderText="Fecha Registro" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
            <asp:BoundField DataField="Estado" HeaderText="Estado" />
            <asp:BoundField DataField="Consignatario" HeaderText="Consignatario" />
        </Columns>
    </asp:GridView>
</asp:Content>
