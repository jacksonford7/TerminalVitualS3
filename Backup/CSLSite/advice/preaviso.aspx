<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="preaviso.aspx.cs" Inherits="CSLSite.preaviso" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Reporte de contenedores que fueron preavisados</title>
    <link href="../shared/estilo/aisv-general.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/print.css" rel="stylesheet" type="text/css"  />
     <link href="../Inicio/css/font-awesome.min.css" rel="stylesheet" type="text/css" />
      <link href="../shared/estilo/tables.css" rel="stylesheet" type="text/css" />
       <script language="javascript" type="text/javascript">
// <![CDATA[
            function btclear_onclick() {
                window.print()
            }
// ]]>
    </script>
</head>
<body onload="loadData();">
   <div id="hoja">
      <div >
     <table class="imagenes" cellpadding="0" cellspacing="0">
     <tr>
     <td>
      <div id="logoCGSA"></div>
     </td>
     <td > 
     <p class="rptitulo"> <i class="xprint"></i>&nbsp;&nbsp;Lista de contenedores procesados</p>
      <br />
     <p class="rplabel" id="fechero"></p> 
     <p class="rplabel" id="esteuser">Usuario:usuario</p>
     </td>
     </tr>
     <tr>
     <td colspan="2">
     <div id="idxxp">
         <img src="../shared/imgs/loader.gif"  alt=''/> Obteniendo los datos..
     </div>
     </td>
     </tr>
     </table>
     </div>
     <div class="botonera" >
         <input id="btclear"  type="reset"    value="Imprimir" onclick="return btclear_onclick()" />
     </div>
    </div>

    <script type="text/javascript">
        function loadData() {
            if (window.opener != null) {
                document.getElementById('idxxp').innerHTML = window.opener.document.getElementById('htmlstring').textContent;
                document.getElementById('esteuser').innerHTML = '<strong>Usuario:</strong>&nbsp;' + window.opener.document.getElementById('usuario').value;
            }
            var d = new Date();
            document.getElementById('fechero').innerHTML = '<strong>Fecha:</strong>&nbsp;'+ d.toLocaleDateString();
        }
    </script>
</body>


</html>
