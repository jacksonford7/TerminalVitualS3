<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="subir_archivo_aisv.aspx.cs" Inherits="CSLSite.subir_archivo_aisv"  Title="Buscar Archivo a Cargar" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>buscador de archivos..</title>
  <link href="../img/favicon2.png" rel="icon"/>
  <link href="../img/icono.png" rel="apple-touch-icon"/>
  <link href="../css/bootstrap.min.css" rel="stylesheet"/>
  <link href="../css/dashboard.css" rel="stylesheet"/>
  <link href="../css/icons.css" rel="stylesheet"/>
  <link href="../css/style.css" rel="stylesheet"/>
  <link href="https://fonts.googleapis.com/css2?family=Roboto:wght@300;400;500;700&display=swap" rel="stylesheet"/>

   
    <script src="../Scripts/pages.js" type="text/javascript"></script>
   
     <!--Datatables-->
       <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/5.15.1/css/all.min.css" integrity="sha512-+4zCK9k+qNFUR5X+cKL9EIR+ZOhtIloNl9GIKS57V1MyNsYpYcUrUeQc9vNfzsWfV28IaLL3i96P9sdNyeRssA==" crossorigin="anonymous" />
       <link href="../css/datatables.min.css" rel="stylesheet" />
       <link rel="stylesheet" type="text/css" href="..js/buttons/css1.6.4/buttons.dataTables.min.css"/>
        <script src="../lib/jquery/jquery.min.js" type="text/javascript"></script>
       <script type="text/javascript"  src="../js/datatables.js"></script>
       <script src="../Scripts/table_catalog.js" type="text/javascript"></script>  
       <script type="text/javascript" src="../js/bootstrap.bundle.min.js"></script>
    <!--Fin-->

   
    <style type="text/css">
        .auto-style2 {
            width: 784px;
        }
    </style>
</head>

<body >
  
  <form id="bookingfrm" runat="server">
  <asp:ToolkitScriptManager ID="tkscata" runat="server"></asp:ToolkitScriptManager>
     <div class="dashboard-container p-4" id="cuerpo" runat="server">

        <input id="ruta_completa" type="hidden" value="" runat="server" clientidmode="Static" />
        <input id="ruta_completa2" type="hidden" value="" runat="server" clientidmode="Static" />
        <input id="nombre_archivo1" type="hidden" value="" runat="server" clientidmode="Static" />
        <input id="nombre_archivo2" type="hidden" value="" runat="server" clientidmode="Static" />

       <div class="form-title">Cargar archivos (Diplomático)</div>

       <div class="form-row">
             <div class="form-group col-md-12"> 
		   	  <label for="inputAddress">Archivo PDF<span style="color: #FF0000; font-weight: bold;"></span></label>
			  <asp:AsyncFileUpload ID="fsuploadarchivo" runat="server"  
                  title="Escoja el archivo con formato indicado .pdf"  style=" font-size:small" visible="true"  class="btn btn-primary"/>
		   </div>
            <div class="form-group col-md-12"> 
		   	  <label for="inputAddress">Archivo PDF<span style="color: #FF0000; font-weight: bold;"></span></label>
			  <asp:AsyncFileUpload ID="fsuploadarchivo2" runat="server"  
                  title="Escoja el archivo con formato indicado .pdf"  style=" font-size:small" visible="true"  class="btn btn-primary"/>
		   </div>
       </div>
       <div class="form-row">
		       <div class="form-group col-md-12"> 
		   	      <p class="alert alert-light" id="sinresultado" runat="server" visible="false"></p>
		       </div>
	   </div>
        <div class="form-row">
		        <div class="d-flex justify-content-end mt-4">
		   	    <asp:Button ID="find" runat="server" Text="Cargar" onclick="find_Click" CssClass="btn btn-primary" /> &nbsp;&nbsp;&nbsp;&nbsp;
                  <input type="button" value="Cerrar" onclick="setArchivo()"  class="btn btn-primary"> 
		       </div>
	   </div>
        <div class="form-row">
           
             <span id="imagen"></span>   
        </div>
      </div>
       <input id="json_object" type="hidden" />
    
    </form>
    
   <script type="text/javascript" >

     
       window.onbeforeunload = function (e) { 
            var e = e || window.event; 
            if (e) { 
           
           } 
           

           var celColect =  this.document.getElementById("ruta_completa").value;
              var lookup_archivo = {
                  sel_Ruta: this.document.getElementById("ruta_completa").value,
                  sel_Ruta2: this.document.getElementById("ruta_completa2").value  ,
                  sel_Nombre_Archivo1: this.document.getElementById("nombre_archivo1").value,
                  sel_Nombre_Archivo2: this.document.getElementById("nombre_archivo2").value  
              };
           if (window.opener != null) {

               window.opener.popupCallback_Archivo(lookup_archivo);
           }
       } 

       function setArchivo()
       { 
            var celColect =  this.document.getElementById("ruta_completa").value;
              var lookup_archivo = {
                  sel_Ruta: this.document.getElementById("ruta_completa").value,
                  sel_Ruta2: this.document.getElementById("ruta_completa2").value,
                  sel_Nombre_Archivo1: this.document.getElementById("nombre_archivo1").value,
                  sel_Nombre_Archivo2: this.document.getElementById("nombre_archivo2").value  
           };

           if (window.opener != null) {

               window.opener.popupCallback_Archivo(lookup_archivo);
           }
         
           self.close();

       }

    

      function initFinder() {
         
          document.getElementById('imagen').innerHTML = '<img alt="" src="../shared/imgs/loader.gif">';
      }
   </script>
</body>

</html>
