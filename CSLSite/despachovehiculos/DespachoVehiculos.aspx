<%@ Page Title="" Language="C#" MasterPageFile="~/site.Master" AutoEventWireup="true" CodeBehind="DespachoVehiculos.aspx.cs" Inherits="CSLSite.DespachoVehiculos" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="placehead" runat="server">
    <link href="../css/bootstrap.min.css" rel="stylesheet" />
    <link href="../css/dashboard.css" rel="stylesheet" />
    <link href="../css/icons.css" rel="stylesheet" />
    <link href="../css/style.css" rel="stylesheet" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="cuerpo" runat="server">
    <asp:ToolkitScriptManager ID="tkscata" runat="server" EnablePageMethods="True" ScriptMode="Release" />
    <asp:UpdatePanel ID="UPBUSCAR" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="row">
                <div class="col-md-2">
                    <asp:TextBox ID="TXTMRN" runat="server" CssClass="form-control" placeholder="MRN" />
                </div>
                <div class="col-md-2">
                    <asp:TextBox ID="TXTMSN" runat="server" CssClass="form-control" placeholder="MSN" />
                </div>
                <div class="col-md-2">
                    <asp:TextBox ID="TXTHSN" runat="server" CssClass="form-control" placeholder="HSN" />
                </div>
                <div class="col-md-2">
                    <asp:Button ID="BtnBuscar" runat="server" CssClass="btn btn-primary" Text="BUSCAR" OnClick="BtnBuscar_Click" />
                </div>
            </div>
            <br />
            <div class="row">
                <div class="col-md-12">
                    <div class="alert alert-danger" id="banmsg" runat="server" visible="false"></div>
                </div>
            </div>
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
                    <asp:TemplateField HeaderText="FA" HeaderStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:CheckBox ID="CHKFA" runat="server" Checked='<%# Bind("VISTO") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="CONTENEDOR" HeaderText="CONTENEDOR" HeaderStyle-HorizontalAlign="Center" />
                    <asp:BoundField DataField="FECHA_HASTA" HeaderText="FECHA HASTA" DataFormatString="{0:yyyy/MM/dd HH:mm}" HeaderStyle-HorizontalAlign="Center" />
                    <asp:TemplateField HeaderText="TURNO REFERENCIAL" HeaderStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:DropDownList ID="CboTurno" runat="server" SelectedValue='<%# Bind("IDPLAN") %>'>
                                <asp:ListItem Value="0">No</asp:ListItem>
                                <asp:ListItem Value="1">Sí</asp:ListItem>
                            </asp:DropDownList>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="IN_OUT" HeaderText="ESTADO" HeaderStyle-HorizontalAlign="Center" />
                    <asp:BoundField DataField="DOCUMENTO" HeaderText="DOCUMENTO" HeaderStyle-HorizontalAlign="Center" />
                    <asp:BoundField DataField="FECHA_ULTIMA" HeaderText="ULTIMA FACTURA" DataFormatString="{0:yyyy/MM/dd}" HeaderStyle-HorizontalAlign="Center" />
                    <asp:BoundField DataField="NUMERO_FACTURA" HeaderText="NUMERO FACTURA" HeaderStyle-HorizontalAlign="Center" />
                    <asp:BoundField DataField="CAS" HeaderText="FECHA CAS" DataFormatString="{0:yyyy/MM/dd}" HeaderStyle-HorizontalAlign="Center" />
                </Columns>
            </asp:GridView>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="BtnBuscar" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
