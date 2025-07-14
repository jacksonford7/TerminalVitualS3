<%@ Page Title="AISV exportacion, consolidacion" Language="C#" MasterPageFile="~/site.Master" AutoEventWireup="true" CodeBehind="vacios.aspx.cs" Inherits="CSLSite.aisv.vacios" %>
<%@ MasterType VirtualPath="~/site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="placehead" runat="server">
  <link href="../img/favicon2.png" rel="icon"/>
  <link href="../img/icono.png" rel="apple-touch-icon"/>
  <link href="../css/bootstrap.min.css" rel="stylesheet"/>
  <link href="../css/dashboard.css" rel="stylesheet"/>
  <link href="../css/icons.css" rel="stylesheet"/>
  <link href="../css/style.css" rel="stylesheet"/>
  <link href="https://fonts.googleapis.com/css2?family=Roboto:wght@300;400;500;700&display=swap" rel="stylesheet"/>

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
              <input id="zonaid" type="hidden" value="105" />
              <input id="bandera"     type="hidden"   runat="server" clientidmode="Static"  />
              <input id="procesar"    type="hidden"   runat="server" clientidmode="Static"  />
              <input id="itemT4"      type="hidden"   runat="server" clientidmode="Static"  />
              <input id="linea_validar"     type="hidden"   runat="server" clientidmode="Static"  />
    
    <div class="mt-4">         
        <nav class="mt-4" aria-label="breadcrumb">
          <ol class="breadcrumb">
            <li class="breadcrumb-item" id="opcion_principal" runat="server"><a href="#">Servicios a clientes de CGSA</a></li>
            <li class="breadcrumb-item active" aria-current="page" id="sub_opcion" runat="server">AISV para exportación o consolidación de contenedores</li>
          </ol>
        </nav>
      </div>

    <!-- White Container -->
<div class="dashboard-container p-4" id="cuerpo" runat="server">
      <div class="form-title">Datos para procesar</div>
           
     
     <div class="form-row">
          <div class="form-group col-md-6">
                          <div class="form-group col-md-12">
                    <label for="inputAddress">Booking:<span style="color: #FF0000; font-weight: bold;"> *</span></label>
                    <div class="d-flex">
                         <span id="refnumber" class="form-control col-md-11 " runat="server" clientidmode="Static" enableviewstate="true">...</span>
                         <a  class="btn btn-outline-primary mr-4" target="popup" onclick="onBook();"  >
                                <span class='fa fa-search' style='font-size:24px'></span> </a>
                        <span id="valnave" class="validacion"></span>
                    </div>
         </div>   
              
              

         </div> 

           <div class="col-md-6"> 
                  <label for="inputEmail4">Operación</label>
		     <div class="d-flex">
                     
                     <div class="d-flex">
                         <label class="checkbox-container">
                               <input runat="server" clientidmode="Static" enableviewstate="true" id="vacio" name="fk" type="radio" value="MTY" checked="true" class="xradio" />&nbsp;Exportación
                                <span class="checkmark"></span>
                        </label><label for="inputEmail4">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</label>
                         <label class="checkbox-container"> 
                               <input runat="server" clientidmode="Static" enableviewstate="true" id="consolida" name="fk" type="radio" value="LCL"  class="xradio"/>&nbsp;Consolidación
                                <span class="checkmark"></span>
                        </label>
                         <span id="valope" class="validacion"> </span>
                     </div> 
		     </div>
		   </div> 

                
        </div>
     <div class="form-row">


         <div class="form-group col-md-12">
                    <label for="inputAddress">Archivo CSV:<span style="color: #FF0000; font-weight: bold;"> *</span></label>
                    <div class="d-flex">
                           <input class="uploader form-control " id="fsupload" title="Escoja el archivo CSV (Excel separado por comas)" accept="csv" type="file"  runat="server" clientidmode="Static" />
                          <asp:Button ID="btup" runat="server" Text="Cargar" onclick="btup_Click"  OnClientClick="return sendform();"
                             ToolTip="Carga el archivo y valida la información" class="btn  btn-primary"/>
                       <span id="valdae" class="validacion"></span>
                    </div>
         </div> 
 </div>
      <div class="form-row">
         <div class="form-group col-md-12">
                   <label for="inputAddress">Mail notificación adicional<span style="color: #FF0000; font-weight: bold;"></span></label>
               <input type='text' id='textbox1' name='textbox1'  runat="server" class="form-control "
                enableviewstate="false" clientidmode="Static"
               onkeypress="return soloLetras(event,'abcdefghijklmnñopqrstuvwxyz1234567890_$@.',true)"  
                onblur="maildata(this,'valmailz');" maxlength="50"/>
             <span id="valmailz" class="validacion"></span>
         </div> 

    </div>
 
  
      <div class="form-title">Resultados de la verificación del archivo </div>
          <div class="form-row" id="xfinder" runat="server" visible="true">
   
     <div id="unit_ok" class="form-group col-md-4" runat="server" clientidmode="Static" >
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
                     <asp:Button ID="btbuscar" runat="server" Text="Generar documentos"  onclick="btbuscar_Click" 
                           OnClientClick="return getprocesa();"  
                             ToolTip="Confirma la información y genera los preavisos"  class="btn  btn-primary"/>
                    </div>
               </div>
   
</div>
    <script src="../Scripts/pages.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            BindFunctions();

            var t = document.getElementById('bandera');
            if (t.value != undefined && t.value != null && t.value.trim().length > 0) {
                $('span.validacion').text('');
            }
            
        });

        function sendform() {

            var t = document.getElementById('bandera').value;
            var lin = document.getElementById('linea_validar').value;


            if (t == undefined || t == null || t.trim().length <= 0) {
                alertify.alert('Informativo','Por favor seleccione el booking..!').set('label', 'Aceptar');
                return false;
            }
            var xvac = document.getElementById('vacio').checked;

            if (xvac) {
                if (t != 'MTY') {
                    alertify.alert('Informativo','El tipo de booking [LCL] no va acorde a la operación [Evacuación]').set('label', 'Aceptar');
                    return false;
                }
            }
            else {
                if (t != 'LCL' && lin != 'MSC') {
                    alertify.alert('Informativo','El tipo de booking [MTY] no va acorde a la operación [Consolidación]').set('label', 'Aceptar');
                    return false;
                }
            }
            if (!ValidateFile('fsupload')) {
                return false;
            }
           
            return true;
        }

        function getprocesa() {
            var i = document.getElementById('procesar');
            if (i == undefined || i == null) {
                alertify.alert('Informativo','No se encontró el control asociado!!').set('label', 'Aceptar');
                return false;
            }
            i = i.value;
            if (i == '0') {
                alertify.alert('Informativo','Por favor realice todos los pasos antes de proceder:\n\t 1.Booking \n\t2.Operación\n\t3.Archivo Csv').set('label', 'Aceptar');
                return false;
            }
            if (confirm('Está seguro de generar los preavisos para las unidades en la lista?')) {
                      alertify.alert('Informativo','En algunos minutos recibirá un mail confirmando la lista de unidades preavisadas y si existe algún contenedor con error').set('label', 'Aceptar');
                return true;
            }
            return false;
        }
        function onBook() {
            tipo = 'MTY';
            if (document.getElementById('consolida').checked) {
                tipo = 'LCL';
            }
          var w =  window.open('../catalogo/bookingmas.aspx?tipo=' + tipo + '&v=1', 'Bookings', 'width=850,height=880');
          w.focus();
        }
        function validateBook(objeto) {
            //stringnifiobjeto
            var bokIt = {
                number    :objeto.numero,
                linea     :objeto.bline,
                referencia:objeto.referencia,
                gkey      :objeto.gkey,
                pod       :objeto.pod,
                pod1      :objeto.pod1,
                shiperID  :objeto.shipid,
                temp      :objeto.temp,
                fkind     :objeto.fk,
                imo       :objeto.imo,
                refer     :objeto.refer,
                dispone   :objeto.dispone,
                iso       :objeto.aqt,
                cutOff    :objeto.cutoff,
                temp      :objeto.temp,
                hume      :objeto.hume,
                vent_pc   :objeto.vent_pc,
                ventu     :objeto.ventu,
                gkey      :objeto.gkey
            };
            document.getElementById('refnumber').textContent = objeto.numero + '/' + objeto.referencia;
            document.getElementById('itemT4').value = JSON.stringify(bokIt);
            document.getElementById('bandera').value = objeto.fk;
            document.getElementById('linea_validar').value = objeto.bline;
            alertify.alert('Informativo','Se le comunica que la disponibilidad máxima unidades del item de booking elegido es: ' + objeto.dispone).set('label', 'Aceptar');
        }
     $('form').live("submit", function () { ShowProgress();});
    </script>
 <div class="loading" align="center">
    Estamos verificando toda la información 
    que nos facilitó,por favor espere unos segundos<br />
    <img src="../shared/imgs/loader.gif" alt="x" />
</div>
</asp:Content>