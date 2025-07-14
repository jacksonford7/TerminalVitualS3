<%@ Page Language="C#" AutoEventWireup="true"  CodeFile="Default.aspx.cs" Inherits="_Default" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="Ajax" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="Themes/black.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
<Ajax:ToolkitScriptManager ID="ToolkitScriptManager1" 
                           runat="server"/>
       
Pick a date: <asp:TextBox ID="txtCal" runat="server"/>

<Ajax:CalendarExtender ID="CalendarExtender1" 
                       runat="server" 
                       TargetControlID="txtCal" 
                       CssClass="black">
</Ajax:CalendarExtender>
  
        
    </div>
    </form>
</body>
</html>
