<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="wbareportezal.aspx.cs" Inherits="CSLSite.facturacion.wbareportezal" %>

<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
      <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
     <table>
    <tr>
    <td height="800px" width="780px" valign="top" >
        
        <rsweb:ReportViewer ID="rwReporte" runat="server" Width="780px" Height="800px"  
            DocumentMapCollapsed="True" ShowDocumentMapButton="False" 
        ShowParameterPrompts="False" ShowPromptAreaButton="False" Font-Names="Verdana" 
            Font-Size="8pt" InteractiveDeviceInfos="(Colección)" 
            WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt">
        <LocalReport EnableExternalImages="True" 
                ReportPath="zal\rptpasepuertazal.rdlc" >
            <DataSources>
                <rsweb:ReportDataSource DataSourceId="odsdsHorariosPasePuerta" 
                    Name="dsPasePuerta" />
            </DataSources>
        </LocalReport>
        </rsweb:ReportViewer>
        
</td>
</tr><tr><td>
    <asp:ObjectDataSource runat="server" id="odsdsHorariosPasePuerta" 
                 OldValuesParameterFormatString="original_{0}" SelectMethod="GetData" 
                 
                 TypeName="CSLSite.CatalogosTableAdapters.VBS_P_RPT_PASE_PUERTA_ZALTableAdapter">
        <SelectParameters>
            <asp:SessionParameter Name="ID" SessionField="idpaseszal" Type="Object" />
        </SelectParameters>
             </asp:ObjectDataSource>
</td></tr>
</table>
    </div>
    </form>
</body>
</html>