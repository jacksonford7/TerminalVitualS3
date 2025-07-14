<%@ Page Title="" Language="C#" MasterPageFile="~/site.Master" AutoEventWireup="true" CodeBehind="confirmareversapago.aspx.cs" Inherits="CgsaMaster.facturacion.bloqueo.confirmareversapago" %>
<%@ MasterType VirtualPath="~/Site.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="../../Styles/sna.css" rel="stylesheet" type="text/css" />
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
    <script src="../../Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery.tablesorter.min.js" type="text/javascript"></script>
    <script src="../../Scripts/panels.js" type="text/javascript"></script>
    <link href="../../Styles/ecuapass.css" rel="stylesheet" type="text/css" />
    <script src="../../Scripts/jquery.reveal.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery.tablePagination.0.1.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-ui.min.js" type="text/javascript"></script>
    <style type="text/css">
    .alinear
    {
        vertical-align:middle;
    }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
  <div class='setcontenido' style=" width:100%">
  <table class='controles' >
        <tr>
            <td style="  background-color:White">Número de Liquidación:</td>
            <td>
                <asp:TextBox ID="txtnumliq" runat="server" MaxLength="25" onkeypress="return soloLetras(event,'abcdefghijklmnopqrstuvwxyz0123456789')"></asp:TextBox>
                <span style="font-size:small; font-family:Consolas; font-weight:normal; font-style:italic; color:Red" id="mailopcional"> * Requerido</span>
            </td>
            <td></td>
        </tr>
        <tr>
            <td style="  background-color:White"></td>
            <td colspan="3" style="text-align:left;">
                <%--<asp:Button ID="btsave" runat="server" Text="Buscar" OnClientClick="return fValida();" onclick="btsave_Click" />--%>
                <asp:ImageButton ImageUrl="~/Imagenes/cmdBoton/b-buscar.gif" ID="btsave" runat="server" ToolTip="Buscar datos." Text="Buscar" OnClientClick="return fValida();" onclick="btsave_Click" />
            </td>
        </tr>
    </table>
  <div class="border_cuple despliegue-al " style=" width:100%;">  
      <asp:UpdatePanel ID="up_arriba" runat="server" ChildrenAsTriggers="true" RenderMode="Inline" UpdateMode="Conditional" >
         <ContentTemplate>
           <script type="text/javascript">               Sys.Application.add_load(BindFunctions);</script>
           <p class="opt-title" style=" display:none">DATOS encontradOS</p>
             <asp:Repeater id="rpDetalleVGM" runat="server" onitemcommand="rpDetalleVGM_ItemCommand" >
                 <HeaderTemplate>
                     <table id="tbfinder" cellpadding="0" cellspacing="1" class="t_repeat">
                         <thead>
                             <tr>
                                 <th class="alinear" style=" border-color:Gray">Número Factura</th>
                                 <th class="alinear" style=" border-color:Gray">Número Identificación</th>
                                 <th class="alinear" style=" border-color:Gray">Razon Social</th>
                                 <th class="alinear" style=" border-color:Gray">Fecha Liquidación</th>
                                 <th class="alinear" style=" border-color:Gray">Número Liquidación</th>
                                 <th class="alinear" style=" border-color:Gray">Fecha Registro</th>
                                 <th class="alinear" style=" border-color:Gray">Usuario Registro</th>
                                 <th class="alinear" style=" border-color:Gray">Valor Factura</th>
                                 <th class="alinear" style=" border-color:Gray">Valor Pagado</th>
                                 <th class="alinear" style=" border-color:Gray">Valor Pendiente</th>
                                 <th class="alinear" style=" border-color:Gray">Confirmar</th>
                                 <th class="alinear" style=" border-color:Gray; display:none">Reversar</th>
                                 <%--<th class="alinear" style=" border-color:Gray">Exportar</th>--%>
                             </tr>
                         </thead>
                         <tbody>
                 </HeaderTemplate>
                 <ItemTemplate>
                     <tr class="point">
                         <td class="alinear"><%#Eval("NUMERO_FACTURA")%></td>
                         <td class="alinear"><%#Eval("NUMERO_IDENTIFICACION")%></td>
                         <td class="alinear"><%#Eval("RAZON_SOCIAL")%></td>
                         <td class="alinear"><%#Eval("FECHA_LIQUIDACION")%></td>
                         <td class="alinear" id="NumLiquida"><%#Eval("NUMERO_LIQUIDACION")%></td>
                         <td class="alinear"><%#Eval("FECHA_REGISTRO")%></td>
                         <td class="alinear"><%#Eval("USUARIO_REGISTRO")%></td>
                         <td class="alinear"><%#Eval("VALOR_FACTURA")%></td>
                         <td class="alinear"><%#Eval("VALOR_PAGADO")%></td>
                         <td class="alinear"><%#Eval("VALOR_PENDIENTE")%></td>
                         <td class="alinear">
                         <asp:ImageButton ID="btconfirmar" runat="server" CommandName="False" CommandArgument='<%#Eval("NUMERO_LIQUIDACION") %>' OnClientClick="return fConfirma('¿Esta seguro de realizar la confirmación?');"  ImageUrl="~/Imagenes/cmdBoton/aceptar.png" ToolTip="Confirmar."/>
                         </td>
                         <td class="alinear">
                         <asp:ImageButton ID="btreversar" runat="server" CommandName="True" CommandArgument='<%#Eval("NUMERO_LIQUIDACION") %>' OnClientClick="return fConfirma('¿Esta seguro de realizar el reverso?');"  ImageUrl="~/Imagenes/cmdBoton/reversar.png" ToolTip="Reversar." />
                         </td>
                         <%--<td class="alinear" style="width: 100px">
                         <asp:ImageButton ImageUrl="~/Imagenes/cmdBoton/b-excell.gif" CommandArgument="Excel" ID="expoToExcel" ToolTip="Exportar a Excel." runat="server"
                                          OnClientClick='return fnExcelReport(NumLiquida);'/>
                         </td>--%>
                         <%--<div class="row">
                            <div class="btn-group pull-right" style=" padding: 20px;">
                            <div class="dropdown">
                            <button class="btn btn-default dropdown-toggle" type="button" id="dropdownMenu1" data-toggle="dropdown" aria-haspopup="true" aria-expanded="true">
                            <span class="glyphicon glyphicon-th-list"></span>Seleccione
                            </button>
                            <ul class="dropdown-menu" aria-labelledby="dropdownMenu1" style=" width:50px">
                                <li><a href="#" onclick="$('#tbfinder').tableExport({type:'json',escape:'false'});"> <img src="images/json.jpg" width="24px"> JSON</a></li>
                                <li><a href="#" onclick="$('#tbfinder').tableExport({type:'json',escape:'false',ignoreColumn:'[2,3]'});"><img src="images/json.jpg" width="24px">JSON (ignoreColumn)</a></li>
                                <li><a href="#" onclick="$('#tbfinder').tableExport({type:'json',escape:'true'});"> <img src="images/json.jpg" width="24px"> JSON (with Escape)</a></li>
                                <li class="divider"></li>
                                <li><a href="#" onclick="$('#tbfinder').tableExport({type:'xml',escape:'false'});"> <img src="images/xml.png" width="24px"> XML</a></li>
                                <li class="divider"></li>
                                <li><a href="#" onclick="$('#tbfinder').tableExport({type:'csv',escape:'false'});"> <img src="../../Imagenes/icons/csv.png" width="20px"> CSV</a></li>
                                <li><a href="#" onclick="$('#tbfinder').tableExport({type:'txt',escape:'false'});"> <img src="../../Imagenes/icons/txt.png" width="20px"> TXT</li>
                                <li class="divider"></li>									
                                <li><a href="#" onclick="$('#tbfinder').tableExport({type:'excel',escape:'false'});"> <img src="../../Imagenes/icons/excel.png" width="20px"> EXCEL</a></li>
                                <li><a href="#" onclick="$('#tbfinder').tableExport({type:'doc',escape:'false'});"> <img src="images/word.png" width="24px"> Word</a></li>
                                <li><a href="#" onclick="$('#tbfinder').tableExport({type:'powerpoint',escape:'false'});"> <img src="images/ppt.png" width="24px"> PowerPoint</a></li>
                                <li class="divider"></li>
                                <li><a href="#" onclick="$('#tbfinder').tableExport({type:'png',escape:'false'});"> <img src="images/png.png" width="24px"> PNG</a></li>
                                <li><a href="#" onclick="$('#tbfinder').tableExport({type:'pdf',pdfFontSize:'7',escape:'false'});"> <img src="../../Imagenes/icons/pdf.png" width="20px"> PDF</a></li>								
                            </ul>
                            </div>
                            </div>
                            </div>	--%>
                     </tr>
                 </ItemTemplate>
                 <FooterTemplate>
                     </tbody>
                     </table>
                 </FooterTemplate>
             </asp:Repeater>
        </ContentTemplate>
        <Triggers>
         <asp:AsyncPostBackTrigger ControlID="btsave" EventName="Click"  />
        </Triggers>
        </asp:UpdatePanel>
         <table style=" width:100%">
   <tr>
    <td align="center">
        <asp:Label Text="[Error]" ForeColor="Red" ID="lblError" runat="server" />
    </td>
   </tr>
   </table>
    </div>
    </div>
<%--</div>
<script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.10.2/jquery.min.js"></script>
<link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.4/css/bootstrap.min.css"/>
<script type="text/javascript" src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.5/js/bootstrap.min.js"></script>--%>
<script type="text/javascript">
    function fnExcelReport(valor) {
        var tab_text = '<table border="1px" style="font-size:20px" ">';
        var textRange;
        var j = 0;
        var tab = document.getElementById('tbfinder'); // id of table
        var lines = tab.rows.length;

        // the first headline of the table
        if (lines > 0) {
            tab_text = tab_text + '<tr bgcolor="#DFDFDF">' + tab.rows[0].innerHTML + '</tr>';
        }

        // table data lines, loop starting from 1
        for (j = 1; j < lines; j++) {
            tab_text = tab_text + "<tr>" + tab.rows[j].innerHTML + "</tr>";
        }

        tab_text = tab_text + "</table>";
        tab_text = tab_text.replace(/<A[^>]*>|<\/A>/g, "");             //remove if u want links in your table
        tab_text = tab_text.replace(/<img[^>]*>/gi, "");                 // remove if u want images in your table
        tab_text = tab_text.replace(/<input[^>]*>|<\/input>/gi, "");    // reomves input params
        // console.log(tab_text); // aktivate so see the result (press F12 in browser)

        var ua = window.navigator.userAgent;
        var msie = ua.indexOf("MSIE ");

        // if Internet Explorer
        if (msie > 0 || !!navigator.userAgent.match(/Trident.*rv\:11\./)) {
            txtArea1.document.open("txt/html", "replace");
            txtArea1.document.write(tab_text);
            txtArea1.document.close();
            txtArea1.focus();
            sa = txtArea1.document.execCommand("SaveAs", true, valor.innerHTML + ".xls");
        }
        else // other browser not tested on IE 11
            sa = window.open('data:application/vnd.ms-excel,' + encodeURIComponent(tab_text));

        return false;
    }
    function fValida() {
        var cntr = document.getElementById('<%=txtnumliq.ClientID %>').value;
        if (cntr == '' || cntr == null || cntr == undefined) {
            alert('* Escriba el Número de Liquidación. *');
            document.getElementById('<%=txtnumliq.ClientID %>').focus();
            return false;
        }
        return true;
    }
    function fConfirma(valor) {
        if (confirm(valor) == false) {
            return false;
        }
        return true;
    }
</script>
</asp:Content>
