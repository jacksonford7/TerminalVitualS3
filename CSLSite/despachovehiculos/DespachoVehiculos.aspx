<%@ Page Title="" Language="C#" MasterPageFile="~/site.Master" AutoEventWireup="true" CodeBehind="DespachoVehiculos.aspx.cs" Inherits="CSLSite.DespachoVehiculos" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="placehead" runat="server">
    <link href="../css/bootstrap.min.css" rel="stylesheet" />
    <link href="../css/dashboard.css" rel="stylesheet" />
    <link href="../css/icons.css" rel="stylesheet" />
    <link href="../css/style.css" rel="stylesheet" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="placebody" runat="server">
    <asp:ToolkitScriptManager ID="tkscata" runat="server" EnablePageMethods="True" ScriptMode="Release" />
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

    <asp:UpdatePanel ID="UPBUSCAR" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="row">
                <div class="col-md-2"><asp:TextBox ID="TXTMRN" runat="server" CssClass="form-control" placeholder="MRN" /></div>
                <div class="col-md-2"><asp:TextBox ID="TXTMSN" runat="server" CssClass="form-control" placeholder="MSN" /></div>
                <div class="col-md-2"><asp:TextBox ID="TXTHSN" runat="server" CssClass="form-control" placeholder="HSN" /></div>
                <div class="col-md-2"><asp:Button ID="BtnBuscar" runat="server" CssClass="btn btn-primary" Text="BUSCAR" OnClick="BtnBuscar_Click" /></div>
            </div>
            <br />
            
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="BtnBuscar" />
        </Triggers>
    </asp:UpdatePanel>

    <asp:UpdatePanel ID="UPDETALLE" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
          <asp:GridView ID="gvContenedores" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered invoice">
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
   <h4 class="mt-4">Contenedores Asignados</h4>


    <asp:GridView ID="gvContenedoresVHS" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered invoice"
        OnRowCommand="gvContenedoresVHS_RowCommand">
        <Columns>
            <asp:BoundField DataField="ContenedorID" HeaderText="ID" />
            <asp:BoundField DataField="NumeroContenedor" HeaderText="Contenedor" />
            <asp:BoundField DataField="manifiesto" HeaderText="Manifiesto" />
            <asp:TemplateField HeaderText="Acciones">
                <ItemTemplate>
                    <asp:Button ID="btnEliminar" runat="server" Text="Eliminar" CommandName="Eliminar" CommandArgument='<%# Eval("ContenedorID") %>' CssClass="btn btn-danger btn-sm" />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
</asp:Panel>


            <asp:Button ID="BtnGrabar" runat="server" CssClass="btn btn-success" Text="GRABAR" OnClick="BtnGrabar_Click" />
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="BtnBuscar" />
            <asp:AsyncPostBackTrigger ControlID="BtnGrabar" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
