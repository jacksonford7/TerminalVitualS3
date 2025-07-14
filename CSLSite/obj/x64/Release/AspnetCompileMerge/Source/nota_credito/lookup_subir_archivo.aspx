<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="lookup_subir_archivo.aspx.cs" Inherits="CSLSite.lookup_subir_archivo"  Title="Buscar Archivo a Cargar" %>
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
    </style>
</head>
<body >
 <noscript><meta http-equiv="refresh" content="0; url=sinsoporte.htm" /> </noscript>
    <form id="bookingfrm" runat="server">
    <asp:ToolkitScriptManager ID="tkscata" runat="server"></asp:ToolkitScriptManager>
   <input id="ruta_completa" type="hidden" value="" runat="server" clientidmode="Static" />
    <div class="catabody">
       <div class="catawrap" >
         <div class="catabuscar">
         <div class="catacapa">
            <table  cellspacing="0" cellpadding="1">
              <tr>
                <td class="auto-style2">Archivo:</td>
               
            </tr>
            <tr>
               <td colspan="1" class="auto-style2" ><asp:AsyncFileUpload ID="fsuploadarchivo" runat="server"  
                 Width="80%" title="Escoja el archivo con formato indicado .pdf"  style=" font-size:small" visible="true" /> </td>  
            </tr>
                 <tr>
  <td colspan="1" class="auto-style2" ><asp:Button ID="find" runat="server" Text="Cargar" onclick="find_Click"  /> &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
      <input type="button" value="Cerrar" onclick="setArchivo()"> 
     <%-- <asp:Button ID="BtnCerrar" runat="server" Text="Cargar" OnClientClick="setArchivo();"  />--%>

  </td>  

                 </tr>
           </table>
             <span id="imagen"></span>
          </div>
        
         </div>
         <div class="cataresult" >

             <div id="xfinder" runat="server" visible="false" >

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

       //window.onbeforeunload = setArchivo;

       window.onbeforeunload = function (e) { 
            var e = e || window.event; 
            if (e) { 
           
            } 
           var celColect =  this.document.getElementById("ruta_completa").value;
              var lookup_archivo = {
                  sel_Ruta: this.document.getElementById("ruta_completa").value  
              };
           if (window.opener != null) {

               window.opener.popupCallback_Archivo(lookup_archivo);
           }
       } 

       function setArchivo()
       { 
            var celColect =  this.document.getElementById("ruta_completa").value;
              var lookup_archivo = {
                  sel_Ruta: this.document.getElementById("ruta_completa").value  
              };
           if (window.opener != null) {

               window.opener.popupCallback_Archivo(lookup_archivo);
           }
         
           self.close();

       }

      function msgfinder(control, expresa) {
          if (control.value.trim().length <= 0) {
              this.document.getElementById(expresa).textContent = 'Escriba una o varias letras del nombre/código y pulse buscar';
              return;
          }
          this.document.getElementById(expresa).textContent = 'Se buscará [' + control.value.toUpperCase() + '], presione el botón';
       }

      function initFinder() {
         
          document.getElementById('imagen').innerHTML = '<img alt="" src="../shared/imgs/loader.gif">';
      }
   </script>
</body>

</html>
