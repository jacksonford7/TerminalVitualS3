<%@ Page Title="Cancelar operaciones" Language="C#" MasterPageFile="~/site.Master" AutoEventWireup="true" CodeBehind="cancel.aspx.cs" Inherits="CSLSite.cancel" %>
<%@ MasterType VirtualPath="~/site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="placehead" runat="server">
     <!--mensajes-->
        <link href="https://cdn.jsdelivr.net/npm/alertifyjs@1.11.0/build/css/alertify.min.css" rel="stylesheet"/>
        <script type="text/javascript" src="https://cdn.jsdelivr.net/npm/alertifyjs@1.11.0/build/alertify.min.js"></script>
          <!--Datatables-->
       <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/5.15.1/css/all.min.css" integrity="sha512-+4zCK9k+qNFUR5X+cKL9EIR+ZOhtIloNl9GIKS57V1MyNsYpYcUrUeQc9vNfzsWfV28IaLL3i96P9sdNyeRssA==" crossorigin="anonymous" />
       <link href="../css/datatables.min.css" rel="stylesheet" />
       <link rel="stylesheet" type="text/css" href="..js/buttons/css1.6.4/buttons.dataTables.min.css"/>
        <script src="../lib/jquery/jquery.min.js" type="text/javascript"></script>
       <script type="text/javascript"  src="../js/datatables.js"></script>
       <script src="../Scripts/table_catalog.js" type="text/javascript"></script>  
       <script type="text/javascript" src="../js/bootstrap.bundle.min.js"></script>
    <!--Fin-->

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="placebody" runat="server">
              <input id="zonaid" type="hidden" value="101" />
              <input id="lineaCI"    type="hidden"   runat="server" clientidmode="Static"   />
              <input id="numexport"  type="hidden"   runat="server" clientidmode="Static"  />
              <input id="referencia" type="hidden"   runat="server" clientidmode="Static"    />
              <input id="boking"     type="hidden"   runat="server" clientidmode="Static"    />
              <input id="bkitem"     type="hidden"   runat="server" clientidmode="Static"    />
              <input id="nave"       type="hidden"   runat="server" clientidmode="Static"   />
              <input id="procesar"   type="hidden"   runat="server" clientidmode="Static"   />
              <input id="unitiso"    type="hidden"   runat="server" clientidmode="Static"   />
              <input id="lineabok"   type="hidden"   runat="server" clientidmode="Static"   />

        <div class="mt-4">         
        <nav class="mt-4" aria-label="breadcrumb">
          <ol class="breadcrumb">
            <li class="breadcrumb-item" id="opcion_principal" runat="server"><a href="#">Exortacion</a></li>
             <li class="breadcrumb-item active" aria-current="page" id="sub_opcion" runat="server">Cancelar avisos de contenedores vacios</li>
          </ol>
        </nav>
      </div>

      <div class="dashboard-container p-4" id="cuerpo" runat="server">

         <div class="form-title">Datos para procesamiento</div>
		 
		  <div class="form-row">
		  
		   <div class="form-group col-md-12"> 
		   	  <label for="inputAddress">Referencia/Nave<span style="color: #FF0000; font-weight: bold;"></span></label>
			  <div class="d-flex">

                               <span id="refnumber" class=" form-control col-md-11 " runat="server" clientidmode="Static" enableviewstate="true">...</span>
                                   <a class="btn btn-outline-primary mr-4" target="popup" onclick="window.open('../catalogo/naves.aspx','name','width=850,height=880')" >
<span class='fa fa-search' style='font-size:24px'></span> 
                                        

                 </a>
                  <span id="valnave" class="validacion"> * </span>
			  </div>
		   </div>
		  </div>
           <div class="form-row">
		  
		   <div class="form-group col-md-12"> 
		   	  <label for="inputAddress">Archivo CSV<span style="color: #FF0000; font-weight: bold;"></span></label>
			  <div class="d-flex">
                            <input  class=" form-control" id="fsupload" title="Escoja el archivo CSV (Excel separado por comas)" accept="csv" type="file"  runat="server" clientidmode="Static" />

                <asp:Button ID="btup" runat="server" Text="Cargar"   CssClass="btn btn-primary"
                onclick="btup_Click"  OnClientClick="return sendform();"
               ToolTip="Carga el archivo y valida la información" />
                 <span id="valdae" class="validacion"> * </span>

			  </div>
		   </div>
		  </div>
           <div class="form-row">
		  
		   <div class="form-group col-md-12"> 
		   	  <label for="inputAddress">Mail notificación adicional<span style="color: #FF0000; font-weight: bold;"></span></label>
			  <div class="d-flex">
                     <input type='text' id='textbox1' name='textbox1'  
                         runat="server"  class="form-control  "
                enableviewstate="false" clientidmode="Static"
               onkeypress="return soloLetras(event,'abcdefghijklmnñopqrstuvwxyz1234567890_$@.',true)"  
                onblur="maildata(this,'valmailz');" maxlength="50"
               />
                          <span id="valmailz" class="validacion" ></span>

			  </div>
		   </div>
		  </div>
        <div class=" form-title">Resultados de la verificación del archivo </div>
          <div class="form-row" id="xfinder" runat="server" visible="true" >
                 <div id="unit_ok" class="form-group col-md-4" runat="server" clientidmode="Static">
                 </div>
                 <div id="unit_error" class="form-group col-md-8" runat="server" clientidmode="Static">
                 </div>
		  </div>
          <div class="form-row">
		   <div class="col-md-12 d-flex justify-content-center"> 
		         <input clientidmode="Static" id="dataexport" onclick="getfile('resultado');" type="button" value="Exportar" runat="server" class="btn btn-secondary"/>
		   </div> 
		   </div>
          <div class="form-row">
		   <div class="col-md-12 d-flex justify-content-center"> 
		                    <div id="sinresultado" runat="server" class="alert alert-warning" clientidmode="Static"></div>

		   </div> 
		   </div>
          <div class="form-row">
		   <div class="col-md-12 d-flex justify-content-center"> 
		     <div class="d-flex">
                    <asp:Button ID="btbuscar"  CssClass="btn btn-primary"
             runat="server" Text="Cancelar"  
             onclick="btbuscar_Click" 
             OnClientClick="return getprocesa();"  
             ToolTip="Confirma la información y cancela los preavisos" 
             Enabled="False" />

		     </div>
		   </div> 
		   </div>

		 
     </div>


    <script src="../Scripts/pages.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.tablesorter.min.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.tablesorter.pager.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
              BindFunctions();
            var t = document.getElementById('nave');
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
        function popupCallback(nave) {
            document.getElementById('referencia').value = nave.codigo;
            document.getElementById('refnumber').textContent = nave.descripcion;
            document.getElementById('nave').value = nave.descripcion;
            document.getElementById('valnave').textContent = '';
        }
        function sendform() {
            var t = document.getElementById('referencia').value;
            if (t == undefined || t == null || t.trim().length <= 0) {
                alertify.alert('Por favor seleccione la referencia...!');
                return false;
            }
            if (!ValidateFile('fsupload')) {
                return false;
            }
            document.getElementById('unit_error').innerHTML = '<p><img alt="" src="../shared/imgs/loader.gif" />&nbsp;&nbsp;Procesando el archivo, espere..</p>';
            document.getElementById('unit_ok').innerHTML = '<p><img alt="" src="../shared/imgs/loader.gif" />&nbsp;&nbsp;Procesando el archivo, espere..</p>';
            return true;
        }
        function getprocesa() {
            var i = document.getElementById('procesar');
            if (i == undefined || i == null) {
                alertify.alert('No se encontró el control asociado!!');
                return false;
            }
            i = i.value;
            if (i == '0') {
                alertify.alert('Por favor realice todos los pasos antes de proceder:\n\t 1.Referencia \n\t2.Archivo Csv');
                return false;
            }
            if (confirm('Está seguro que desea cancelar TODOS los preavisos para las unidades en la lista?')) {
             alertify.alert('En algunos minutos recibirá un mail confirmando la lista de unidades que fueron canceladas correctamente.');
                return true;
            }
            return false;
        }
        $('form').live("submit", function () { ShowProgress(); });
    </script>

     <div class="loading" align="center">
    Estamos verificando toda la información 
    que nos facilitó,por favor espere unos segundos<br />
    <img src="../shared/imgs/loader.gif" alt="x" />
</div>
</asp:Content>