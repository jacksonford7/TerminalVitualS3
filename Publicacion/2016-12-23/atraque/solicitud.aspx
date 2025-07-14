<%@ Page  Title="Solicitud de Atraque"  Language="C#" MasterPageFile="~/site.Master" AutoEventWireup="true" CodeBehind="solicitud.aspx.cs" Inherits="CSLSite.solicitud" %>
<%@ MasterType VirtualPath="~/site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="placehead" runat="server">
    <link href="../shared/estilo/aisv-general.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/work.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/atraque.css" rel="stylesheet" type="text/css" />
 </asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="placebody" runat="server">
     <input id="zonaid" type="hidden" value="302" />
     <noscript><meta http-equiv="refresh" content="0; url=sinsoporte.htm" /> </noscript>
     <div>
         <i class="ico-titulo-1"></i><h2>Solicitud de Atraque</h2>  <br /> 
         <i class="ico-titulo-2"></i><h1>Nueva Solicitud de atraque</h1>
         
         <br />
    </div>
     <div class=" msg-alerta">
    <span id="dtlo" runat="server">Estimado usuario:</span> 
    <br /> Asegúrese que toda la información que agrega a este documento es correcta antes de proceder a su respectiva generación, si desea confirmar alguna información antes de proceder comuníquese con nuestro departamento de planificación a los teléfonos: +593 (04) 6006300, 3901700 
                 ext. 4039, 4040, 4060. 
    </div>
     <div class="seccion" id="BUSCAR">
      <div class="informativo">
      <table>
      <tr><td rowspan="2" class="inum"> <div class="number">1</div></td><td class="level1" >Información General</td></tr>
      <tr><td class="level2">Seleccione la línea y servicio</td></tr>
      </table>
     </div>
     <div class="colapser colapsa" ></div>
     <div class="accion" id="SERVICIO">
     <table class="controles" cellspacing="0" cellpadding="1">
        <tr><th class="bt-bottom bt-right bt-top bt-left" colspan="4"> Línea y Servicio</th></tr>
         <tr>
         <td class="bt-bottom  bt-right bt-left" >1. Línea Naviera</td>
         <td class="bt-bottom bt-right" colspan="3">
         <span id="nomline" class="caja cajafull" style="width:450px;" runat="server" clientidmode="Static"  enableviewstate="False">...</span>
         <input type="hidden" runat="server" clientidmode="Static" id="agencia" />
         </td>       
         </tr>
         <tr><td class="bt-bottom  bt-right bt-left">2. Servicio de transporte</td>
         <td class="bt-bottom" colspan="2">
             <asp:DropDownList ID="dpservicio" runat="server"  Width="300px" 
                ClientIDMode="Static" enableviewstate="false"  onchange="populateLines(this);"
               ></asp:DropDownList>
              <a class="tooltip" ><span class="classic" >Ruta asociada al servicio</span>
                        <img alt='' src='../shared/imgs/info.gif' class='datainfo'/>
              </a>
         </td>
         <td class="bt-bottom bt-right validacion"><span class="validacion" id="xplinea" ></span></td>
         </tr>

     </table>
     </div>
    </div>
     <div class="seccion">
      <div class="informativo">
      <table>
      <tr><td rowspan="2" class="inum"> <div class="number">2</div></td><td class="level1" >Información de Líneas Asociadas</td></tr>
      <tr><td class="level2">
         En esta sección agregue TODAS las lineas asociadas, en caso de fallar alguna solo producirá correcciones y retrasos en los datos relacionados a la carga
      </td></tr>
      </table>
     </div>
    <div class="colapser colapsa"></div>
      <div class="accion" id="LINEAS">
     <table class="controles" cellspacing="0" cellpadding="1">
        <tr><th class="bt-bottom bt-right bt-top bt-left" colspan="2"> Líneas asociadas al servicio</th></tr>
         <tr>
         <td class="bt-bottom  bt-right bt-left" colspan="3" >
         <div id="clineas">
          <table  class="olineas" id="tblineas" cellpadding="1" cellspacing="1" >
          <thead>
            <tr>
                <th>Linea naviera</th>
                <th> Viaje entrante</th>
                <th>Viaje saliente</th>
                <th><span id="smas" class="add"  onclick="window.open('../catalogo/linea','name','width=850,height=480')">&nbsp;</span></th>
                <th><span id="smenos" class="quitar"  onclick="rem_line_row()" >&nbsp;</span></th>
            </tr>
            </thead>
            <tbody></tbody>
            </table>
         </div>


         </td>
         </tr>
           </table>
     </div>
    </div>
     <div class="seccion">
      <div class="informativo">
      <table>
      <tr><td rowspan="2" class="inum"> <div class="number">3</div></td><td class="level1" >Información sobre el viaje</td></tr>
      <tr>
      <td class="level2">
         Ingrese los datos requeridos en cada una de las siguientes secciones.
      </td>
      </tr>
      </table>
     </div>
      <div class="colapser colapsa"></div>
     <div class="accion" id="NAVE">
      <table class="controles" cellspacing="0" cellpadding="1">
        <tr><th class="bt-bottom bt-right bt-top bt-left" colspan="3"> Información de la Nave</th></tr>
        
                 <tr>
         <td class="bt-bottom  bt-right bt-left" >3. Número IMO</td>
         <td class="bt-bottom">
             <span id="bqimo" class="caja cajafull"  runat="server" clientidmode="Static"  enableviewstate="False">...</span>
           
             </td>
         <td class="bt-bottom bt-right ">  <a  class="topopup" target="popup" onclick="window.open('../catalogo/buque','name','width=850,height=480')" >Buscar</a></td>
         </tr>
        
        
                         <tr>
         <td class="bt-bottom  bt-right bt-left" >4. Nombre</td>
         <td class="bt-bottom">
              <span id="bqname" class="caja cajafull"  runat="server" clientidmode="Static"  enableviewstate="False">...</span>
           
             </td>
         <td class="bt-bottom bt-right validacion "></td>
         </tr>

          <tr>
         <td class="bt-bottom  bt-right bt-left" >5. Bandera</td>
         <td class="bt-bottom">
              <span id="bqflag" class="caja cajafull"  runat="server" clientidmode="Static"  enableviewstate="False">...</span>
           
             </td>
         <td class="bt-bottom bt-right validacion "></td>
         </tr>

                          <tr>
         <td class="bt-bottom  bt-right bt-left" >6. Eslora</td>
         <td class="bt-bottom">
              <span id="bqloa" class="caja cajafull" runat="server" clientidmode="Static"  enableviewstate="False">...</span>
           
             </td>
         <td class="bt-bottom bt-right validacion "></td>
         </tr>

                   <tr>
         <td class="bt-bottom  bt-right bt-left" >7. Tonelaje Neto</td>
         <td class="bt-bottom">
              <span id="bpnet" class="caja cajafull"  runat="server" clientidmode="Static"  enableviewstate="False">...</span>
           
             </td>
         <td class="bt-bottom bt-right validacion "></td>
         </tr>

                            <tr>
         <td class="bt-bottom  bt-right bt-left" >8. Tonelaje Bruto</td>
         <td class="bt-bottom">
              <span id="bqtone" class="caja cajafull"  runat="server" clientidmode="Static"  enableviewstate="False">...</span>
           
             </td>
         <td class="bt-bottom bt-right validacion "></td>
         </tr>
        

         <tr>
         <td class="bt-bottom  bt-right bt-left" >9. Call</td>
         <td class="bt-bottom">
              <span id="bpcall" class="caja cajafull"  runat="server" clientidmode="Static"  enableviewstate="False">...</span>
           
             </td>
         <td class="bt-bottom bt-right validacion "></td>
         </tr>


          <tr>
         <td class="bt-bottom  bt-right bt-left" >10. Tipo de Buque</td>
         <td class="bt-bottom">
        <span id="bqship" class="caja cajafull"  runat="server" clientidmode="Static"  enableviewstate="False">...</span>
           
             </td>
         <td class="bt-bottom bt-right validacion "></td>
         </tr>
         <tr>
         <td class="bt-bottom  bt-right bt-left" >11. Número de Viaje (ENTRANTE)</td>
         <td class="bt-bottom">
             <asp:TextBox ID="tvIn" runat="server" Width="250px" MaxLength="20" CssClass="mayusc" ClientIDMode="Static"
             onkeypress="return soloLetras(event,'abcdefghijklmnopqrstuvwxyz1234567890-/._ ')" 
             onblur="cadenareqerida(this,1,20,'tvInv');"
              placeholder="INBOUND"
             ></asp:TextBox>

             </td>
         <td class="bt-bottom bt-right validacion "><span id="tvInv" class="validacion"> * obligatorio</span></td>
         </tr>
          <tr>
         <td class="bt-bottom  bt-right bt-left" >12. Número de Viaje (SALIENTE)</td>
         <td class="bt-bottom">
             <asp:TextBox ID="tvOu" runat="server" Width="250px" MaxLength="20" CssClass="mayusc" ClientIDMode="Static"
             onkeypress="return soloLetras(event,'abcdefghijklmnopqrstuvwxyz1234567890-/._ ')" 
             onblur="cadenareqerida(this,1,20,'tvOuv');"
              placeholder="OUTBOUND"
             ></asp:TextBox>

             </td>
         <td class="bt-bottom bt-right validacion "><span id="tvOuv" class="validacion"> * obligatorio</span></td>
         </tr>
         </table>

         <table class="controles" cellspacing="0" cellpadding="1" id="CNTR">
        <tr><th class="bt-bottom bt-right  bt-left" colspan="3">Información PBIP (proteccion 
            de buques e instalaciones portuarias)</th></tr>
         <tr>
         <td class="bt-bottom  bt-right bt-left" >13. PBIP Número</td>
         <td class="bt-bottom">
             <asp:TextBox ID="tbipnum" runat="server" Width="250px" MaxLength="11" CssClass="mayusc"
             onkeypress="return soloLetras(event,'abcdefghijklmnopqrstuvwxyz1234567890')" 
              onblur="cadenareqerida(this,1,40,'valpbi');"
              placeholder="PBPIP" ClientIDMode="Static"
             ></asp:TextBox>

             </td>
         <td class="bt-bottom bt-right "><span id="valpbi" class="validacion"> * obligatorio</span></td>
         </tr>

                  <tr>
         <td class="bt-bottom  bt-right bt-left" >14. Válido hasta</td>
         <td class="bt-bottom">
             <asp:TextBox ID="pbhasta" runat="server" Width="250px" MaxLength="11" CssClass="datepicker"
             onkeypress="return soloLetras(event,'/1234567890',false)" 
              onblur="cadenareqerida(this,1,12,'valhasta');"
              placeholder="Fecha" ClientIDMode="Static"
             ></asp:TextBox>

             </td>
         <td class="bt-bottom bt-right "><span id="valhasta" class="validacion"> * obligatorio</span></td>
         </tr>

         <tr>
         <td class="bt-bottom  bt-right bt-left" >15. Provisional</td>
         <td class="bt-bottom">
             
              Si[<input id="rbsi"   type="radio" name="deck"  checked="checked" />]
              No[ <input id="rbno"   type="radio" name="deck" />]
            

         
         </td>
         <td class="bt-bottom bt-right "></td>
         </tr>
          <tr>
         <td class="bt-bottom  bt-right bt-left" >16. Seguridad</td>
         <td class="bt-bottom">
           <select id='dpseguro' onchange="segVal(this);">
                 <option value="0">*Nivel*&nbsp;</option>
                 <option value="1">1</option>
                 <option value="2">2</option>
                 <option value="3">3</option>
           </select>

         
         </td>
         <td class="bt-bottom bt-right "><span id="va_nivel" class="validacion"> * obligatorio</span></td>
         </tr>

		 </table>

      <table class="controles" cellspacing="0" cellpadding="1">
      <tr><th class="bt-bottom bt-right  bt-left" colspan="3">Información de Puertos</th> </tr>
      <tr>
       <td class="bt-bottom  bt-right bt-left" >17. Ultimo Puerto</td>
       <td class="bt-bottom">
       <span id="lpuerto" class="caja cajafull"  style="width:400px;" runat="server" clientidmode="Static"  enableviewstate="False">...</span>
       <input type="hidden" id="hlpuerto"  value=""/>
       </td>
       <td class="bt-bottom bt-right ">  
       <a  class="topopup" target="popup" onclick="setPort(1);" >Buscar</a>
       </td>
       </tr>

       <tr>
       <td class="bt-bottom  bt-right bt-left" >18. Próximo Puerto</td>
       <td class="bt-bottom">
       <span id="npuerto" class="caja cajafull"  style="width:400px;" runat="server" clientidmode="Static"  enableviewstate="False">...</span>
       <input type="hidden" id="hnpuerto"  value=""/>
       </td>
       <td class="bt-bottom bt-right ">  
       <a  class="topopup" target="popup" onclick="setPort(2)" >Buscar</a>
       </td>
       </tr>
      </table>


               <table class="controles" cellspacing="0" cellpadding="1" id="tbsenae">
        <tr><th class="bt-bottom bt-right  bt-left" colspan="3">Información de SENAE</th></tr>
         <tr>
         <td class="bt-bottom  bt-right bt-left" >19. Manifiesto de Importación</td>
         <td class="bt-bottom">
             <asp:TextBox ID="tmrn" runat="server" Width="350px" MaxLength="16" CssClass="mayusc"
             onkeypress="return soloLetras(event,'abcdefghijklmnopqrstuvwxyz1234567890')" 
              onblur="cadenareqerida(this,1,16,'valmrn');"
              placeholder="CEC0000XXXX0000" ClientIDMode="Static" Text="CEC"
             ></asp:TextBox>

             </td>
         <td class="bt-bottom bt-right "><span id="valmrn" class="validacion"> * obligatorio</span></td>
         </tr>

                  <tr>
         <td class="bt-bottom  bt-right bt-left" >20. Manifiesto de Exportación</td>
         <td class="bt-bottom">
             <asp:TextBox ID="tdae" runat="server" Width="350px" MaxLength="16" CssClass="mayusc"
             onkeypress="return soloLetras(event,'abcdefghijklmnopqrstuvwxyz1234567890',false)" Text="CEC"
              onblur="cadenareqerida(this,1,16,'sdae');"
             placeholder="CEC0000XXXX0000" ClientIDMode="Static"
             ></asp:TextBox>

             </td>
         <td class="bt-bottom bt-right "><span id="sdae" class="validacion"> * obligatorio</span></td>
         </tr>
		 </table>

       <table class="controles" cellspacing="0" cellpadding="1" id="tbapg">
        <tr><th class="bt-bottom bt-right  bt-left" colspan="3">Información de autoridad 
            portuaria de guayaquil</th></tr>
         <tr>
         <td class="bt-bottom  bt-right bt-left" >21. Año de registro</td>
         <td class="bt-bottom">
             <asp:TextBox ID="tyear" runat="server" Width="150px" MaxLength="4" CssClass="mayusc"
             onkeypress="return soloLetras(event,'1234567890')" 
              onblur="cadenareqerida(this,1,4,'sanio');"
              placeholder="Numero" ClientIDMode="Static"
             ></asp:TextBox>
             <a class="tooltip" ><span class="classic" >Año del registro portuario</span>
              <img alt='' src='../shared/imgs/info.gif' class='datainfo'/>
              </a>
             </td>
         <td class="bt-bottom bt-right "><span id="sanio" class="validacion"> * obligatorio</span></td>
         </tr>

                  <tr>
         <td class="bt-bottom  bt-right bt-left" >22. Número de registro</td>
         <td class="bt-bottom">
             <asp:TextBox ID="tregistro" runat="server" Width="150px" MaxLength="6" CssClass="mayusc"
             onkeypress="return soloLetras(event,'1234567890',false)" 
              onblur="cadenareqerida(this,1,8,'sreg');"
              placeholder="Registro" ClientIDMode="Static"
             ></asp:TextBox>
                                   <a class="tooltip" ><span class="classic" >Número de registro otorgado por APG</span>
                        <img alt='' src='../shared/imgs/info.gif' class='datainfo'/>
              </a>
             </td>
         <td class="bt-bottom bt-right "><span id="sreg" class="validacion"> * obligatorio</span></td>
         </tr>
		 </table>

       

         <table class="controles" cellspacing="0" cellpadding="1" id="tbeta">
        <tr><th class="bt-bottom bt-right  bt-left" colspan="3">Información de fechas estimadas de operación</th></tr>
         <tr>
         <td class="bt-bottom  bt-right bt-left" >23. <strong>ETA:</strong>  Fecha estimada de arribo a boya de Data Posorja</td>
         <td class="bt-bottom">
             <asp:TextBox ID="teta" runat="server" Width="250px" MaxLength="16" 
                onkeypress="return soloLetras(event,'1234567890/ ')" 
                onblur="valDate(this,true,seta);"
                CssClass="datetimepicker"
                placeholder="ETA" 
                ClientIDMode="Static"
             ></asp:TextBox>

             </td>
         <td class="bt-bottom bt-right "><span id="seta" class="validacion"> * obligatorio</span></td>
         </tr>

                 <tr>
         <td class="bt-bottom  bt-right bt-left" >24.<strong> ETB:</strong> Fecha estimada de Atraque muelle CGSA</td>
         <td class="bt-bottom">
             <asp:TextBox ID="tetb" runat="server" Width="250px" MaxLength="16" 
               onkeypress="return soloLetras(event,'1234567890/ ')" 
               onblur="valDate(this,true,setb);sumarHorasFecha();"
               CssClass="datetimepicker"
             
              placeholder="ETB" ClientIDMode="Static"
             ></asp:TextBox>

             </td>
         <td class="bt-bottom bt-right "><span id="setb" class="validacion"> * obligatorio</span></td>
         </tr>




                  <tr>
         <td class="bt-bottom  bt-right bt-left" >25. Número de horas uso de muelle</td>
         <td class="bt-bottom">
             <asp:TextBox ID="thoras" runat="server" Width="100px" MaxLength="3" ClientIDMode="Static"
             onkeypress="return soloLetras(event,'1234567890 /',false)" 
            
             onblur=" cadenareqerida(this,1,8,'shora');sumarHorasFecha();"
              placeholder="Horas uso"
             ></asp:TextBox>

             </td>
         <td class="bt-bottom bt-right "><span id="shora" class="validacion"> * obligatorio</span></td>
         </tr>

                 <tr>
         <td class="bt-bottom  bt-right bt-left" >26.<strong> ETS:</strong>  Fecha estimada de zarpe</td>
         <td class="bt-bottom">
             <asp:TextBox ID="tets" runat="server" Width="250px" MaxLength="16" 
           ClientIDMode="Static"
              ReadOnly="true"
            
             ></asp:TextBox>
             <a class="tooltip" ><span class="classic" >Se calcula a partir de el ETB + uso</span>
              <img alt='' src='../shared/imgs/info.gif' class='datainfo'/>
              </a>
             </td>
         <td class="bt-bottom bt-right "></td>
         </tr>


		 </table>




      <table class="controles" cellspacing="0" cellpadding="1">
        <tr><th class="bt-bottom bt-right  bt-left" colspan="3"> Datos para notificación</th></tr>
         <tr>
         <td class="bt-bottom  bt-right bt-left" >27. Correo(s) para notificación: 
         <i><small>(Estos correos se utilizarán para notificar en caso que sea necesaria una inspección)</small></i>
         </td>
         <td class="bt-bottom">
         <div id='TextBoxesGroup'>
           <div id="TextBoxDiv1" class="cntmail">
               <span>mail #1:</span><input type='text' id='textbox1' name='textbox1'  runat="server"
                enableviewstate="false" clientidmode="Static"
               onkeypress="return soloLetras(event,'abcdefghijklmnñopqrstuvwxyz1234567890_$@.',true)"  
                onblur="cadenareqerida(this,1,100,'valmail');"
                 placeholder="mail@mail.com"
               />
           </div>
         </div>
             <input type='button' value='Agregar' id='addButton' />
             <input type='button' value='Remover' id='removeButton' />
          </td>
          <td class="bt-bottom bt-right validacion "><span id="valmail" class="validacion"> </span></td>
         </tr>
         </table>
      <div class="informativo">
      <table>
      <tr><td rowspan="2" class="inum"> <div class="number">4</div></td><td class="level1" >Responsabilidad de la información</td></tr>
      <tr>
      <td class="level2">
      <div class=" msg-critico">
       Los datos proporcionados son de entera responsabilidad de quien los consigna, por lo que CONTECON GUAYAQUIL S.A. no se responsabiliza por cualquier error o falsedad que los mismos pudieren tener, siendo de cuenta del cliente todos los gastos y perjuicios que por dicho error se ocasionen a la carga.
       <br />CGSA otorgará la información proporcionada en este documento a las autoridades competentes a la operación de exportación cuando sean solicitadas.
      </div>
      </td>
      </tr>
      </table>
     </div>




      <div class="botonera">
         <img alt="loading.." src="../shared/imgs/loader.gif" id="loader" class="nover"  />
         <input id="btsalvar" type="button" value="Generar Solicitud" onclick="imprimir();" /> &nbsp;
         

     </div>
      <div id="secciones">
         Secciones:&nbsp;
         <a href="#SERVICIO">Servicio</a>
         <a href="#LINEAS" >Líneas Asociadas</a> 
         <a href="#NAVE">Datos de la Nave</a>
        
   </div>
     </div>
    </div>




    <script src="../Scripts/pages.js" type="text/javascript"></script>
    <script src="../Scripts/atraque.js" type="text/javascript"></script>
    <script type="text/javascript">
       //variables;
        var jSolicitud = {};
        var sw = 0;
        var lineasAsociadas = [];
        $(window).load(function () {
            //objeto a transportar.
            $(document).ready(function () {
                //inicia los fecha-hora
                $('.datetimepicker').datetimepicker().datetimepicker({ lang: 'es', format: 'd/m/Y H:i', step: 30 });
                //inicia los fecha
                $('.datepicker').datetimepicker().datetimepicker({ lang: 'es', timepicker: false, format: 'd/m/Y', closeOnDateSelect: true, minDate: '0' });

                //colapsar y expandir
                $("div.colapser").toggle(function () { $(this).removeClass("colapsa").addClass("expande"); $(this).next().hide(); }
                                  , function () { $(this).removeClass("expande").addClass("colapsa"); $(this).next().show(); });

                //controlador de mails
                var counter = 2;
                $("#addButton").click(function () {
                    if (counter > 5) {
                        alert("Solo se permiten 5 mails");
                        return false;
                    }
                    $('<div/>', { 'id': 'TextBoxDiv' + counter }).html($('<span/>').html('mail #' + counter + ':')).append($('<input type="text"  placeholder="mail@mail.com" >').attr({ 'id': 'textbox' + counter, 'name': 'textbox' + counter, onkeypress: 'return soloLetras(event,"abcdefghijklmnñopqrstuvwxyz1234567890_@.",true);' })).appendTo('#TextBoxesGroup')
                    counter++;
                });
                $("#removeButton").click(function () {
                    if (counter == 2) {
                        alert("Un mail es obligatorio");
                        return false;
                    }
                    counter--;
                    $("#TextBoxDiv" + counter).remove();
                });
            });
        });

      function popupCallback(objeto, catname) {
          if (catname == 'lineaCI') {
              var it = {};
              it.codigo = objeto.codigo;
              it.valor = objeto.descripcion;
              add_line_row('tblineas', lineasAsociadas, it);
              return;
          }
          if (catname == 'puerto') {
              var opt = objeto.opcion;
              if (opt == null || opt == undefined) {
                  return;
              }
              if (opt == 1) {
                  document.getElementById('lpuerto').textContent = objeto.descripcion;
                  document.getElementById('hlpuerto').value = objeto.codigo;

              }
              if (opt == 2) {
                  document.getElementById('npuerto').textContent = objeto.descripcion;
                  document.getElementById('hnpuerto').value = objeto.codigo;
              }
              return;
          }
          if (catname == 'Buque') {
              document.getElementById('bqimo').textContent = objeto.bid;
              document.getElementById('bqname').textContent = objeto.bnombre;
              document.getElementById('bqflag').textContent = objeto.bpais;
              document.getElementById('bqloa').textContent = objeto.blargo;
              document.getElementById('bpnet').textContent = objeto.bnet;
              document.getElementById('bqtone').textContent = objeto.bgros;
              document.getElementById('bpcall').textContent = objeto.bradio;
              document.getElementById('bqship').textContent = objeto.btipo;
              jSolicitud.imo = objeto.bid;
              jSolicitud.sign = objeto.bradio;
              jSolicitud.pebruto = objeto.bgros;
              jSolicitud.peneto = objeto.bnet;
              jSolicitud.qgkey = objeto.bkey;
              jSolicitud.nombre = objeto.bnombre;
              jSolicitud.tipo = objeto.btipo;
              jSolicitud.qline = objeto.bline;
              jSolicitud.flag = objeto.bpais;
              jSolicitud.eslora = objeto.blargo;
              return;
          }
      }


      function imprimir() {

          if (!confirm('Está seguro de que desea generar la solicitud, este proceso es IRREVERSIBLE?')) {
              return;
          }

          document.getElementById("loader").className = '';
          //poblar Objeto
          populateObject(jSolicitud, lineasAsociadas);
          //validarlo objeto
          if (!validaciones(jSolicitud)) {
              document.getElementById("loader").className = 'nover';
              return;
          }
          invokeJsonTransport(jSolicitud, '../atraque/solicitud.aspx/ValidateJSON');
      }

      function populateObject(objeto, arreglo) {
          //variable = (condition) ? true-value : false-value;
          objeto.service = (document.getElementById('dpservicio').value != undefined) ? document.getElementById('dpservicio').value : '';
          objeto.imo = (objeto.imo != undefined) ? objeto.imo : '';
          objeto.vIn = (document.getElementById('tvIn').value != undefined) ? document.getElementById('tvIn').value : '';
          objeto.vOut = (document.getElementById('tvOu').value != undefined) ? document.getElementById('tvOu').value : '';
          objeto.imrn = (document.getElementById('tmrn').value != undefined) ? document.getElementById('tmrn').value : '';
          objeto.emrn = (document.getElementById('tdae').value != undefined) ? document.getElementById('tdae').value : '';
          objeto.anio = (document.getElementById('tyear').value != undefined) ? document.getElementById('tyear').value : '';
          objeto.regis=(document.getElementById('tregistro').value != undefined) ? document.getElementById('tregistro').value : '';
          objeto.eta = (document.getElementById('teta').value != undefined) ? document.getElementById('teta').value : '';
          objeto.etb = (document.getElementById('tetb').value != undefined) ? document.getElementById('tetb').value : '';
          objeto.uso = (document.getElementById('thoras').value != undefined) ? document.getElementById('thoras').value : '';
          objeto.ets = (document.getElementById('tets').value != undefined) ? document.getElementById('tets').value : '';
          objeto.lport =(document.getElementById('hlpuerto').value != undefined) ? document.getElementById('hlpuerto').value : '';
          objeto.nport =(document.getElementById('hnpuerto').value != undefined) ? document.getElementById('hnpuerto').value : '';
          objeto.pebruto = (objeto.pebruto != undefined) ? objeto.pebruto : '';
          objeto.sign = (objeto.sign != undefined) ? objeto.sign : '';
          objeto.pnum = (document.getElementById('tbipnum').value != undefined) ? document.getElementById('tbipnum').value : '';
          objeto.phasta = (document.getElementById('pbhasta').value != undefined) ? document.getElementById('pbhasta').value : '';
          objeto.pprov = (document.getElementById('rbsi').value != undefined) ? document.getElementById('rbsi').checked : '';
          objeto.pseg = (document.getElementById('dpseguro').value != undefined) ? document.getElementById('dpseguro').value : '';
          objeto.agencia = (document.getElementById('agencia').value != undefined) ? document.getElementById('agencia').value : '';
          objeto.nservicio = document.getElementById('dpservicio').options[document.getElementById('dpservicio').selectedIndex].text;
          objecLines(objeto, arreglo, 'tblineas')
      }




    </script>
</asp:Content>