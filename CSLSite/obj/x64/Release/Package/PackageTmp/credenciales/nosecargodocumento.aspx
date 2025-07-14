<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="nosecargodocumento.aspx.cs" Inherits="CSLSite.nosecargodumento" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>No se encontro documento</title>
    <link href="../shared/estilo/catalogosolicitudempresa.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/work.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/aisv-general.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/tables.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/info.css" rel="stylesheet" type="text/css" />
    <script src="Scripts/jquery-1.7.1.min.js" type="text/javascript"></script>
    <script src="../Scripts/jquery-1.7.1.min.js" type="text/javascript"></script>
    <link href="Inicio/base-site.css" rel="stylesheet" type="text/css" />
    <link href="Inicio/css/font-awesome.min.css" rel="stylesheet" type="text/css" />
    <link rel="shortcut icon" type="image/x-icon" href="favicon.ico" />
    <link href="shared/estilo/work.css" rel="stylesheet" type="text/css" />
    <link href="../Inicio/base-site.css" rel="stylesheet" type="text/css" />
    <link href="../Inicio/css/font-awesome.min.css" rel="stylesheet" type="text/css" />
    <link rel="../shortcut icon" type="image/x-icon" href="../favicon.ico" />
    <link href="../shared/estilo/work.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
     * input[type=text]
        {
            text-align:left!important;
        }
    </style>
    <script type="text/javascript">
        function BindFunctions() {
            document.getElementById('imagen').innerHTML = '';
            $(document).ready(function () {
                $('#tablasort').tablesorter({ cancelSelection: true, cssAsc: "ascendente", cssDesc: "descendente", headers: { 5: { sorter: false }, 6: { sorter: false}} }).tablesorterPager({ container: $('#pager'), positionFixed: false, pagesize: 10 });
            });
        }
    </script>
</head>
<body>
    <noscript><meta http-equiv="refresh" content="0; url=sinsoporte.htm" /> </noscript>
    <form id="bookingfrm" runat="server">
    <div id="wall-panel">
    <div id="wraper-panel">
    <div id="logoCGSA"> 
    </div>
      <div id="menu-panel">
       <%--<p class="calendar" > <span id="dia">00</span><em id="mesx">Mes</em></p>--%>
      <br />
     Terminal Virtual
      
      <hr />
      </div>
      <div>
       <div class="left borde-all" style=" width:98%" >
        <i class="element"></i> <span class="icon-text">Estimado Usuario:</span>
        <h3></h3>
        <p>En esta solicitud no se cargo ningún documento, revise el detalle de la solicitud para más información.</p>

        <fieldset style=" border:white">
        <br />
        <ul style="display:none">
         <li>Contenedores Llenos <a href="http://www.cgsa.com.ec/files/ZonaDescarga/CSL/videos/AISV-CONTENEDOR-LLENO.zip" target="_blank">VIDEO</a> (3516Kb) <a href="http://www.cgsa.com.ec/Files/ZonaDescarga/CSL/GUIA-DE-AISV-DE-CONTENEDOR-LLENO.pdf" target="_blank">PDF</a> (913 Kb)</li>
         <li>Contenedores Vacíos <a href="http://www.cgsa.com.ec/files/ZonaDescarga/CSL/videos/AISV-DE-CONTENEDOR-VACIO.zip" target="_blank">VIDEO</a> (1752Kb) </li>
         <li>Carga Suelta (AISV de carga suelta <a href="http://www.cgsa.com.ec/files/ZonaDescarga/CSL/videos/AISV-DE-CARGA-SUELTA.zip" target="_blank">VIDEO</a> (2908Kb) y AISV carga suelta contenedor de acopio <a href="http://www.cgsa.com.ec/files/ZonaDescarga/CSL/videos/AISV-DE-CARGA-SUELTA-(CONT-DE-ACOPIO).zip" target="_blank">VIDEO</a> (3371Kb) ) <a href="http://www.cgsa.com.ec/Files/ZonaDescarga/CSL/GUIA-DE-AISV-DE-CARGA-SUELTA.pdf" target="_blank">PDF</a> (973 Kb) </li>
         <li>Carga a Consolidar (AISV de carga a consolidar) <a href="http://www.cgsa.com.ec/files/ZonaDescarga/CSL/videos/AISV-DE-CARGA-A-CONSOLIDAR.zip" target="_blank">VIDEO</a> (2940Kb) y AISV de carga a consolidar contenedores de acopio <a href="http://www.cgsa.com.ec/files/ZonaDescarga/CSL/videos/AISV-CARGA-A-CONSOLIDAR-(ACOPIO).zip" target="_blank">VIDEO</a> (2821Kb) ) <a href="http://www.cgsa.com.ec/Files/ZonaDescarga/CSL/GUIA-DE-AISV-DE-CARGA-A-CONSOLIDAR.pdf" target="_blank">PDF</a> (973 Kb)</li>
         <li>AISV Consolidadoras <a href="http://www.cgsa.com.ec/files/ZonaDescarga/CSL/videos/AISV_CONSOLIDADORAS.zip" target="_blank">VIDEO</a>(4443Kb)</li>
        </ul>
        </fieldset>

      </div>
      </div>
      <div id="foot-panel">
        <table class="tabla" cellpadding="1" cellspacing ="1">
        <tr>
        <td class="nota-pie">
        <p class="al-left ">
                  
        </p>
        </td>
        <td class="nota-pie">
           <p class="al-right">
           © 2014 CONTECON S.A
           </p>
        </td>
        </tr>
        </table>
      <br />
      </div>
    </div>
    </div>
    </form>
    <script src="../../Scripts/pages.js" type="text/javascript"></script>
    <script src="../../Scripts/credenciales.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.7.1.min.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery.tablesorter.min.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery.tablesorter.pager.js" type="text/javascript"></script>
</body>
</html>