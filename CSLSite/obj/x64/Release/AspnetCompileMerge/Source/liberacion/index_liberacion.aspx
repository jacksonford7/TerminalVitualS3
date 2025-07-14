<%@ Page Title="" Language="C#" MasterPageFile="~/site.Master" AutoEventWireup="true" CodeBehind="index_liberacion.aspx.cs" Inherits="CSLSite.liberacion.index_liberacion" %>
<%@ MasterType VirtualPath="~/Site.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="placehead" Runat="Server">
    <link href="../Styles/tablas.css" rel="stylesheet" type="text/css" />
    <link href="../Styles/paneles.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.tablesorter.min.js" type="text/javascript"></script>
    <script src="../Scripts/panels.js" type="text/javascript"></script>


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="placebody" Runat="Server">

<script type="text/javascript">
    $(document).ready(function () {
        $(".t_repeat").tablesorter({
            cancelSelection: true,
            cssAsc: "ascendente",
            cssDesc: "descendente"
        });
    }
);
</script>
</asp:Content>
