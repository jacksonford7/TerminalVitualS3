<%@ Page Title="" Language="C#" MasterPageFile="~/site.Master" AutoEventWireup="true" CodeBehind="ReportePagos.aspx.cs" Inherits="CSLSite.bloqueo.ReportePagos" %>
 <%@ MasterType VirtualPath="~/site.master" %>
<%--<%@ Register assembly="DevExpress.Web.ASPxEditors.v12.1, Version=12.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxEditors" tagprefix="dx" %>--%>
<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>
<%--<%@ Register src="../../controles/Loading.ascx" tagname="Loading" tagprefix="uc1" %>--%>
<%--<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxtoolkit" %>--%>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxtoolkit" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="placehead" runat="server">
    <%--<link href="../../Styles/sna.css" rel="stylesheet" type="text/css" />
    <link href="../../Styles/paneles.css" rel="stylesheet" type="text/css" />
    <script src="../../Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
    <script src="../../Scripts/panels.js" type="text/javascript"></script>
    <link href="../../Styles/ecuapass.css" rel="stylesheet" type="text/css" />
    <link href="../../Styles/controls.css" rel="stylesheet" type="text/css" />
    <link href="../../Styles/tablas.css" rel="stylesheet" type="text/css" />
    <link href="../../Styles/ui-auto.css" rel="stylesheet" type="text/css" />
    <link href="../../Styles/modal.css" rel="stylesheet" type="text/css" />
    <link href="../../Styles/paneles.css" rel="stylesheet" type="text/css" />
    <link href="../../Styles/tablas.css" rel="stylesheet" type="text/css" />
    <link href="../../Styles/cuenta.css" rel="stylesheet" type="text/css" />
    <script src="../../Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery.tablesorter.min.js" type="text/javascript"></script>
    <script src="../../Scripts/panels.js" type="text/javascript"></script>
    <link href="../../Styles/ecuapass.css" rel="stylesheet" type="text/css" />
    <script src="../../Scripts/jquery.reveal.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery.tablePagination.0.1.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-ui.min.js" type="text/javascript"></script>--%>

    <link href="../img/favicon2.png" rel="icon"/>
    <link href="../img/icono.png" rel="apple-touch-icon"/>
    <link href="../css/bootstrap.min.css" rel="stylesheet"/>
    <link href="../css/dashboard.css" rel="stylesheet"/>
    <link href="../css/icons.css" rel="stylesheet"/>
    <link href="../css/style.css" rel="stylesheet"/>
    <link href="https://fonts.googleapis.com/css2?family=Roboto:wght@300;400;500;700&display=swap" rel="stylesheet"/>
    <link href="../shared/estilo/jquery.datetimepicker.css" rel="stylesheet" type="text/css" />

    <link href="../shared/estilo/catalogos.css" rel="stylesheet" type="text/css" />

  </asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="placebody" runat="server">
    <asp:ToolkitScriptManager ID="tkcal" EnableScriptGlobalization="true" runat="server"> </asp:ToolkitScriptManager>

    <div class="mt-4">         
        <nav class="mt-4" aria-label="breadcrumb">
            <ol class="breadcrumb">
                <li class="breadcrumb-item" id="opcion_principal" runat="server"><a href="#">Gestión Financiera</a></li>
                <li class="breadcrumb-item active" aria-current="page" id="sub_opcion" runat="server">REPORTE DE BLOQUEO DE CLIENTE</li>
            </ol>
        </nav>
    </div>

  <asp:UpdatePanel ID="UPPrincipal" runat="server"  UpdateMode="Conditional"  ChildrenAsTriggers="true">
    <ContentTemplate>
    <div class="dashboard-container p-4" id="cuerpo" runat="server">

        <div class="form-title">
            REPORTE PAGO CLIENTE
        </div>

        
        <div class="form-row">
            <div class="form-group col-md-3"> 
                <label for="inputAddress">Fecha Desde:<span style="color: #FF0000; font-weight: bold;"></span></label>
                <div class="d-flex"> 
                    <%--<asp:TextBox 
                        style="text-align: center" 
                        ID="ASPxDateEdit_desde" runat="server" ClientIDMode="Static" MaxLength="15" CssClass="datetimepicker form-control"
                        onkeypress="return soloLetras(event,'01234567890/')" 
                        onblur="valDate(this,true,valfechaini);">
                    </asp:TextBox>
                    <span id="valfechaini" class="validacion" ></span>                --%>
                    <asp:TextBox ID="ASPxDateEdit_desde" runat="server" MaxLength="10"  onkeypress="return soloLetras(event,'1234567890/')"  CssClass="form-control" ></asp:TextBox>

                    <ajaxtoolkit:CalendarExtender ID="CalendarExtender2" runat="server" Enabled="True" Format="MM/dd/yyyy" TargetControlID="ASPxDateEdit_desde" TodaysDateFormat="MMMM d,yyyy" ></ajaxtoolkit:CalendarExtender>
                </div>
            </div>

            <div class="form-group col-md-3">   
                <label for="inputAddress">Fecha Hasta:<span style="color: #FF0000; font-weight: bold;"></span></label>
                <div class="d-flex"> 
                    <%--<asp:TextBox 
                        style="text-align: center" 
                        ID="ASPxDateEdit_hasta" runat="server" ClientIDMode="Static" MaxLength="15" CssClass="datetimepicker form-control"
                        onkeypress="return soloLetras(event,'01234567890/')" 
                        onblur="valDate(this,true,valfechaini);">
                    </asp:TextBox>    --%>

                     <asp:TextBox ID="ASPxDateEdit_hasta" runat="server" MaxLength="10"  onkeypress="return soloLetras(event,'1234567890/')"  CssClass="form-control"></asp:TextBox>
                     <ajaxtoolkit:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="True" Format="MM/dd/yyyy" TargetControlID="ASPxDateEdit_hasta" TodaysDateFormat="MMMM d,yyyy" ></ajaxtoolkit:CalendarExtender>

                     &nbsp;
                     &nbsp;
                    <asp:Button Text="Consultar" CssClass="btn btn-primary" runat="server" id="btn_RPT" Height="36" onclick="btn_RPT_Click" />
                </div>
            </div>
        </div>
        <div class="cataresult" >
            <div class="bokindetalle" style=" width:100%; overflow:auto">
                <rsweb:ReportViewer ID="ReportViewer1" runat="server" Font-Names="Verdana" style=" width:100%"
                    Font-Size="8pt" InteractiveDeviceInfos="(Colección)" 
                    WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt" 
                     Width="407px">
                    <localreport reportpath="bloqueo\ReportePagos.rdlc">
                        <datasources>
                            <rsweb:ReportDataSource DataSourceId="dsReportePagos" Name="dsReportePagos" />
                        </datasources>
                    </localreport>
                </rsweb:ReportViewer>
                <asp:SqlDataSource ID="dsReportePagos" runat="server" 
                    ConnectionString="<%$ ConnectionStrings:midle %>" 
                    SelectCommand="INV_ECU_LIQUIDACION_PAGO_SENAE" 
                    SelectCommandType="StoredProcedure" onselecting="DataSet_Daily_Selecting">
                    <SelectParameters>
                        <asp:SessionParameter Name="FECHA_INI" SessionField="FECHA_INI" 
                            Type="DateTime" DefaultValue="" />
                        <asp:SessionParameter Name="FECHA_FIN" SessionField="FECHA_FIN" 
                            Type="DateTime" DefaultValue="" />
                        <asp:Parameter DefaultValue="1" Name="TIPO" Type="Int32" />
                    </SelectParameters>
                </asp:SqlDataSource>
                <%--<uc1:Loading ID="Loading1" runat="server" />--%>
            </div>
        </div>
    </div>                
 
     </ContentTemplate>
  </asp:UpdatePanel>

     <script src="../Scripts/jquery.datetimepicker.js" type="text/javascript"></script>
    
    <script type="text/javascript">
        $(document).ready(function () {
            $('.datetimepicker').datetimepicker().datetimepicker({ lang: 'es', timepicker: false, closeOnDateSelect: true, format: 'd/m/Y' });
        });
    </script>
</asp:Content>
