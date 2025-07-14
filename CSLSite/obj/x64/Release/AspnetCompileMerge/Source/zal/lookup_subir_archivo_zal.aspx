<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="lookup_subir_archivo_zal.aspx.cs" Inherits="CSLSite.lookup_subir_archivo_zal"  Title="Buscar Archivo a Cargar" %>
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

  <link href="../img/favicon2.png" rel="icon"/>
  <link href="../img/icono.png" rel="apple-touch-icon"/>
  <link href="../css/bootstrap.min.css" rel="stylesheet"/>
  <link href="../css/dashboard.css" rel="stylesheet"/>
  <link href="../css/icons.css" rel="stylesheet"/>
  <link href="../css/style.css" rel="stylesheet"/>
  <link href="https://fonts.googleapis.com/css2?family=Roboto:wght@300;400;500;700&display=swap" rel="stylesheet"/>

  <link href="../shared/estilo/Reset.css" rel="stylesheet" />
  <link href="../shared/estilo/jquery.datetimepicker.css" rel="stylesheet" type="text/css" />


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

        

            <input class="form-control"  id="ruta_completa" type="hidden" value="" runat="server" clientidmode="Static" />
            <div class="dashboard-container p-4" id="cuerpo" runat="server">
                <div  >
                    <div class="catabuscar">
                        <div  >
                                <div class="catacapa">

                                   <%-- <div class="form-row">
                                        <div class="form-group col-md-12"> 
                                            <div class="d-flex">
                                                <label for="inputAddress">Archivo:<span style="color: #FF0000; font-weight: bold;"></span></label>
                                
                                                <asp:AsyncFileUpload ID="fsuploadarchivo" runat="server"  
                                                class="form-control" title="Escoja el archivo con formato indicado .pdf"  style=" font-size:small" visible="true" /> 
                                            </div>
                                        
                                        </div>
                                    </div>--%>
                                    <div class="d-flex">
                                        <p class="catalabel">Archivo:</p>
                                        <asp:AsyncFileUpload ID="fsuploadarchivo" runat="server"  
                                                    class="form-control" title="Escoja el archivo con formato indicado .pdf"  style=" font-size:small" visible="true" /> 
                                    </div>
                                
                                </div>
                                <div><br /></div>
                                <div class="row">
                                    <div class="col-md-12 d-flex justify-content-center">
                                        <asp:Button class="btn btn-primary"  ID="find" runat="server" Text="Cargar" onclick="find_Click"  /> &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
                                        <input  class="btn btn-outline-primary mr-4" type="button" value="Cerrar" onclick="setArchivo()"/> 
                                        <span id="imagen"></span>
                                    </div>
                                </div>
                        </div>
                    </div>
                    
                    <div class="form-row">
                        <div class="form-group col-md-12"> 
                            <div class="cataresult" >

                                <div id="xfinder" runat="server" visible="false" >

                                </div>
                                <div><br /></div>
                                <div id="sinresultado" runat="server" class="alert alert-info">
                                    Selecciones el archivo a cargar...
                                </div>
         
                            </div>
                        </div>
                    </div>

                </div>
            </div>

            <input class="form-control"  id="json_object" type="hidden" />

       
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
