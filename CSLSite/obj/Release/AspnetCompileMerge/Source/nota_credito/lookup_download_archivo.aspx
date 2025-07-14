<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="lookup_download_archivo.aspx.cs" Inherits="CSLSite.lookup_download_archivo"  Title="Descargar Archivos" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>buscador de archivos..</title>
    <link href="../shared/estilo/catalogos.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/work.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/pages.js" type="text/javascript"></script>
    <link href="../shared/estilo/aisv-general.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/tables.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
       function BindFunctions(){
           $(document).ready(function () {
               document.getElementById('imagen').innerHTML = '';
               $('#tablasort').tablesorter({ cancelSelection: true, cssAsc: "ascendente", cssDesc: "descendente", headers: { 5: { sorter: false }, 6: { sorter: false}} }).tablesorterPager({ container: $('#pager'), positionFixed: false, pagesize: 10 });
           });
        }
    </script>
    <style type="text/css">
        .auto-style2 {
            width: 784px;
        }
        .auto-style3 {
            width: 800px;
            height: 632px;
            overflow: hidden;
            background-color: White;
            margin: 0 auto;
            border-radius: 15px 15px 15px 15px;
            padding: 10px;
        }
        .auto-style4 {
            height: 494px;
        }
        .auto-style5 {
            height: 21px;
        }
    </style>
</head>
<body >
 <noscript><meta http-equiv="refresh" content="0; url=sinsoporte.htm" /> </noscript>
    <form id="bookingfrm" runat="server">
    <asp:ToolkitScriptManager ID="tkscata" runat="server"></asp:ToolkitScriptManager>
   <input id="ruta_completa" type="hidden" value="" runat="server" clientidmode="Static" />
    <div class="auto-style3">
       <div class="catawrap" >
         <div class="catabuscar">
         <div class="catacapa">
            <table  cellspacing="0" cellpadding="1">
              <tr>
                <td class="auto-style2">Archivo:</td>
               
            </tr>
            <tr>
               
            </tr>
                 <tr>
  <td colspan="1" class="auto-style2" >
      <input type="button" value="Cerrar" onclick="setArchivo()"> 


  </td>  

                 </tr>
           </table>
             <span id="imagen"></span>
          </div>
        
         </div>
         <div class="cataresult" >

             <div id="xfinder" runat="server" visible="false" >
                  <div class="findresult" >
             <div class="booking" >
                  <div class="auto-style5">Archivos Encontrados</div>
                 <div class="auto-style4">
                 <asp:Repeater ID="tablePagination" runat="server" 
                         onitemcommand="tablePagination_ItemCommand" >
                 <HeaderTemplate>
                 <table id="tablasort" cellspacing="1" cellpadding="1" class="tabRepeat">
                 <thead>
                 <tr>
                 <th>USUARIO</th>
                 <th>NOMBRES</th>
                 <th>NIVEL APROBACION</th>
               <%--  <th>ARCHIVO</th>--%>
                 <th>ACCION</th>
                 </tr>
                 </thead> 
                 <tbody>
                 </HeaderTemplate>
                 <ItemTemplate>
                 <tr class="point" >
                  <td><%#Eval("usuario")%></td>
                  <td><%#Eval("descripcion")%></td>
                  <td><%#Eval("estado")%> </td>
                 <%-- <td><asp:Label Text='<%#Eval("direccion_archivo")%>' ID="lbl_archivo" runat="server" Width="100px" /></td>--%>
                  <td><asp:Button ID="BtnDescargar" CommandArgument= '<%#Eval("direccion_archivo")%>' runat="server" Text='Descargar'
                       CssClass="Anular"  CommandName="Descargar"  Height="22px" /></td>
                  
                  
                 </tr>
                  </ItemTemplate>
                 <FooterTemplate>
                 </tbody>
                 </table>
                 </FooterTemplate>
         </asp:Repeater>
                </div>
             </div>
             </div>
              </div>
               <div id="sinresultado" runat="server" class="msg-info">
            Selecciones el archivo a cargar...
              </div>
         
       </div>
      </div>
      </div>
    <input id="json_object" type="hidden" />
    </form>
     <script src="../Scripts/jquery-1.7.1.min.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.tablesorter.min.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.tablesorter.pager.js" type="text/javascript"></script>
   <script type="text/javascript" >


       function setArchivo()
       { 
           
           self.close();

       }

      function initFinder() {
         
          document.getElementById('imagen').innerHTML = '<img alt="" src="../shared/imgs/loader.gif">';
      }
   </script>
</body>

</html>
