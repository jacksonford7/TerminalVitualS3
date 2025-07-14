<%@ Page Title="AISV exportacion, consolidacion" Language="C#" MasterPageFile="~/site.Master" AutoEventWireup="true" CodeBehind="casvacios.aspx.cs" 
Inherits="CSLSite.aisv.casvacios" %>
<%@ MasterType VirtualPath="~/site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="placehead" runat="server">
    <link href="../shared/estilo/tables.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/aisv-general.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/work.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/catalogos.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/info.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="placebody" runat="server">
              <input id="zonaid" type="hidden" value="102" />
              <input id="bandera"     type="hidden"   runat="server" clientidmode="Static"  />
              <input id="procesar"    type="hidden"   runat="server" clientidmode="Static"  />
             
    <div>
   <i class="ico-titulo-1"></i><h2>Servicios a clientes de CGSA </h2>  <br /> 
   <i class="ico-titulo-2"></i><h1>Marcar contenedores para retorno (IMPO CAS)</h1><br />
 </div>
  <div class="seccion">
       <div class="accion">
            <table class="xcontroles" cellspacing="0" cellpadding="1">
        <tr><th class="bt-bottom bt-left bt-right bt-top" colspan="4"> Datos PARA EL  PROCESAMIENTO</th></tr>
         <tr>
         <td  class="bt-bottom bt-left bt-right" >Paso 1: Código de Agencia</td>
         <td class="bt-bottom bt-left bt-right" colspan="3" >
             <span id="refnumber" class="caja cajafull" runat="server" clientidmode="Static" enableviewstate="true">...</span>
          </td>
    
         </tr>

         <tr>
         <td class="bt-bottom  bt-right bt-left">Paso 2: Archivo CSV:</td>
          <td class="bt-bottom " >
               <input class="uploader" id="fsupload" title="Escoja el archivo CSV (Excel separado por comas)" accept="csv" type="file"  runat="server" clientidmode="Static" />
              </td>
            <td class="bt-bottom">
                <asp:Button ID="btup" runat="server" Text="Cargar" onclick="btup_Click"  OnClientClick="return sendform();"
                    ToolTip="Carga el archivo y valida la información" />
                </td>
             <td class="bt-bottom  bt-right "><span id="valdae" class="validacion"> * obligatorio</span></td>
         </tr>

         <tr>
         <td class="bt-bottom  bt-right bt-left">Opcional: Mail notificación adicional</td>
          <td class="bt-bottom " >
          <input type='text' id='textbox1' name='textbox1'  runat="server" style= ' width:200px'
                enableviewstate="false" clientidmode="Static"
               onkeypress="return soloLetras(event,'abcdefghijklmnñopqrstuvwxyz1234567890_$@.',true)"  
                onblur="maildata(this,'valmailz');" maxlength="50"
               />
             
              </td>
            <td class="bt-bottom">
             <span id="valmailz" class="validacion" style=" width:120px;"></span>
                </td>
             <td class="bt-bottom  bt-right "><span class="opcional"> * opcional</span></td>
         </tr>
         </table>
           <div class="botonera" id="bkresult"></div>
             <div class="cataresult" >
             <div id="xfinder" runat="server" visible="true" >
             <div class="findresult" >
             <div class="booking" >
                  <div class="separator">&nbsp;&nbsp; Resultados de la verificación de los 
                      contenedores en archivo </div>
                 <div class="bokindetalle">
                 <div id="unit_ok" class="resultados" runat="server" clientidmode="Static">
                 </div>
                 <div id="unit_error" class="resultados" runat="server" clientidmode="Static">
                 </div>
                </div>
                  <input clientidmode="Static" id="dataexport" onclick="getfile('resultado');" type="button" value="Exportar" runat="server" />
             </div>
             </div>
              </div>
             </div>
              <div id="sinresultado" runat="server" class="msg-info" clientidmode="Static"></div>
             <div class="botonera">
             <asp:Button ID="btbuscar" runat="server" Text="Generar documentos"  onclick="btbuscar_Click" 
                   OnClientClick="return getprocesa();"  
                     ToolTip="Confirma la información y genera los preavisos" Enabled="False" />
           </div>
      </div>
  </div>
    <script src="../Scripts/pages.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.tablesorter.min.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.tablesorter.pager.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            var t = document.getElementById('bandera');
            if (t.value != undefined && t.value != null && t.value.trim().length > 0) {
                $('span.validacion').text('');
            }
            $('#tablaok')
                        .tablesorter({ cancelSelection: true, cssAsc: "ascendente", cssDesc: "descendente" })
                        .tablesorterPager({ container: $('#pageok'), positionFixed: false, pagesize: 10 })
            $('#tablamal')
                        .tablesorter({ cancelSelection: true, cssAsc: "ascendente", cssDesc: "descendente" })
                        .tablesorterPager({ container: $('#pagefail'), positionFixed: false, pagesize: 10 })
        });

        function sendform() {

             if (!ValidateFile('fsupload')) {
                return false;
            }
           
            return true;
        }

        function getprocesa() {
            var i = document.getElementById('procesar');
            if (i == undefined || i == null) {
                alert('No se encontró el control asociado!!');
                return false;
            }
            i = i.value;
            if (i == '0') {
                alert('Por favor realice todos los pasos antes de proceder:\n\t1.Archivo Csv');
                return false;
            }
            if (confirm('Está seguro de marcar y preparar los preavisos para las unidades en la lista?')) {
                      //alert('En algunos minutos recibirá un mail confirmando la lista de unidades preavisadas y si existe algún contenedor con error');
                return true;
            }
            return false;
        }

     $('form').live("submit", function () { ShowProgress();});
    </script>
 <div class="loading" align="center">
    Estamos verificando toda la información 
    que nos facilitó,por favor espere unos segundos<br />
    <img src="../shared/imgs/loader.gif" alt="x" />
</div>
</asp:Content>