﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="wbareportecfs.aspx.cs" Inherits="CSLSite.facturacion.wbareportecfs" %>

<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
     <table>
    <tr>
    <td height="450" width="780px" valign="top" >
        
        <rsweb:ReportViewer ID="rwReporte" runat="server" Width="780px" Height="550px"  
            DocumentMapCollapsed="True" ShowDocumentMapButton="False" 
        ShowParameterPrompts="False" ShowPromptAreaButton="False">
         <LocalReport EnableExternalImages="True" ReportPath="RptPasePuertaBreakBulkSubRpt.rdlc" >
        </LocalReport>
        </rsweb:ReportViewer>
        
</td>
</tr><tr><td>
    <asp:ObjectDataSource runat="server" id="odsdsHorariosPasePuerta" 
                 OldValuesParameterFormatString="original_{0}" SelectMethod="GetData" 
                 TypeName="CSLSite.dsHorariosPPTableAdapters.CFS_CONSULTA_HORARIOS_PASEPUERTATableAdapter">
        <SelectParameters>
            <asp:SessionParameter Name="PASE" SessionField="IDPP" Type="Int64" />
        </SelectParameters>
             </asp:ObjectDataSource>
</td></tr>
</table>
    </div>
    </form>
</body>
</html>
