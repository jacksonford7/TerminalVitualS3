<%@ Page  Title="Solicitud de Atraque"  Language="C#" MasterPageFile="~/site.Master" AutoEventWireup="true" CodeBehind="solicitud.aspx.cs" Inherits="CSLSite.solicitud" %>
<%@ MasterType VirtualPath="~/site.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="placehead" runat="server">
    <link href="../shared/estilo/jquery.datetimepicker.css" rel="stylesheet" type="text/css" />
   
    
     <link href="../shared/estilo/atraque.css" rel="stylesheet" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="placebody" runat="server">
    <div class="mt-4">         
        <nav class="mt-4" aria-label="breadcrumb">
            <ol class="breadcrumb">
            <li class="breadcrumb-item" id="opcion_principal" runat="server"><a href="#">Mis Naves</a></li>
                <li class="breadcrumb-item active" aria-current="page" id="sub_opcion" runat="server">Nueva Solicitud de atraque</li>
            </ol>
        </nav>
    </div>
    <div class="dashboard-container p-4" id="cuerpo" runat="server">
        <div class="alert alert-warning">
        <span id="dtlo" runat="server">Estimado Cliente:</span> 
        <br /> Asegúrese que toda la información que agrega a este documento es correcta antes de proceder a su respectiva generación, si desea confirmar alguna información antes de proceder comuníquese con nuestro departamento de planificación email:AfterDock@cgsa.com.ec o a los teléfonos: +593 (04) 6006300, 3901700 
                 ext. 4043. 
    </div>
	    <div class="form-title barra_colapser barra_cerrar "  data-toggle="collapse" data-target="#row_general"  aria-controls="row_general"  role="button" aria-expanded="true" >Información General 
		     </div>
	    <div class="form-row show" id="row_general">
                <div class=" alert alert-light"> En esta sección agregue TODAS las lineas asociadas, en caso de fallar alguna solo producirá correcciones y retrasos en los datos relacionados a la carga.</div>
                <div class="form-group col-md-6"> 
                    <label for="inputAddress">1. Línea Naviera<span style="color: #FF0000; font-weight: bold;"></span></label>
                    <span id="nomline" class="form-control" runat="server" clientidmode="Static"  enableviewstate="False">...</span>
                    <input type="hidden" runat="server" clientidmode="Static" id="agencia" />
                </div>

                <div class="form-group col-md-6"> 
                    <label for="inputAddress">2. Servicio de transporte<span style="color: #FF0000; font-weight: bold;"></span></label>
                    <asp:DropDownList ID="dpservicio" class="form-control" runat="server" ClientIDMode="Static" enableviewstate="false"  onchange="populateLines(this);"></asp:DropDownList>
                    <a class="tooltip" ><span class="classic" >Ruta asociada al servicio</span><img alt='' src='../shared/imgs/info.gif' class='datainfo'/></a>
                </div>
            </div>



       <div 
	  class="form-title barra_colapser barra_cerrar "  data-toggle="collapse" data-target="#row_asoc"  aria-controls="row_asoc"  role="button"  aria-expanded="true"   
	  >
            Información de Líneas Asociadas

           </div>
        <div class="form-row show" id="row_asoc">
		   <div class="form-group col-md-12"> 
                               <table class="table table-bordered" cellspacing="0" cellpadding="1">
                    <tr>
                        <td class="bt-bottom  bt-right bt-left" colspan="3" >
                            <div id="clineas">
                                <table  class="olineas" id="tblineas" cellpadding="1" cellspacing="1" >
                                    <thead>
                                        <tr>
                                            <th>Operador</th>
                                            <th> Viaje entrante</th>
                                            <th>Viaje saliente</th>
                                            <th><span id="smas" class="add"  onclick="window.open('../catalogo/linea.aspx','name','width=850,height=880')">&nbsp;</span></th>
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
       
        
         <div 
	  class="form-title barra_colapser barra_cerrar "  data-toggle="collapse" data-target="#row_viaje"  aria-controls="row_viaje"  role="button"  aria-expanded="true"   
	  >
            Información sobre el viaje
       </div>
        <div class="show" id="row_viaje"> 
         <div class="form-row">
		  <div class="form-group col-md-6"> 
		   	  <label for="inputAddress">2. Nombre<span style="color: #FF0000; font-weight: bold;"></span></label>
			  <span id="bqname" class="form-control col-md-12"  runat="server" clientidmode="Static"  enableviewstate="False">...</span>

		   </div>
              <div class="form-group col-md-6"> 
		   	  <label for="inputAddress">3. Número IMO<span style="color: #FF0000; font-weight: bold;"></span></label>
			 <div class="d-flex">
              <span id="bqimo" class="form-control col-md-10"  runat="server" clientidmode="Static"  enableviewstate="False">...</span>
                   <a  class="btn btn-outline-primary mr-4" target="popup" onclick="window.open('../catalogo/buque.aspx','name','width=850,height=880')" >
                                <span class='fa fa-search' style='font-size:24px'></span> 
                            </a>

			 </div>
		   </div>

		   </div> 
        <div class="form-row">
		  
		   <div class="form-group col-md-6"> 
		   	  <label for="inputAddress">5. Bandera<span style="color: #FF0000; font-weight: bold;"></span></label>
			    <span id="bqflag" class="form-control col-md-12"  runat="server" clientidmode="Static"  enableviewstate="False">...</span>

		   </div>

              <div class="form-group col-md-6"> 
		   	  <label for="inputAddress">6. Eslora<span style="color: #FF0000; font-weight: bold;"></span></label>
			    <span id="bqloa" class="form-control col-md-12" runat="server" clientidmode="Static"  enableviewstate="False">...</span>

		   </div>
		  </div>
            	  <div class="form-row">
		  
		   <div class="form-group col-md-6"> 
		   	  <label for="inputAddress">7. Tonelaje Neto<span style="color: #FF0000; font-weight: bold;"></span></label>
			     <span id="bqtone" class="form-control col-md-12"  runat="server" clientidmode="Static"  enableviewstate="False">...</span>

		   </div>

                      <div class="form-group col-md-6"> 
		   	  <label for="inputAddress">7. Tonelaje Neto<span style="color: #FF0000; font-weight: bold;"></span></label>
			      <span id="bpnet" class="form-control col-md-12"  runat="server" clientidmode="Static"  enableviewstate="False">...</span>

		   </div>
		  </div>

        	  <div class="form-row">
		  
		   <div class="form-group col-md-6"> 
		   	  <label for="inputAddress">9. Call (Señal)<span style="color: #FF0000; font-weight: bold;"></span></label>
			     <span id="bpcall" class="form-control col-md-12"  runat="server" clientidmode="Static"  enableviewstate="False">...</span>

		   </div>

                  <div class="form-group col-md-6"> 
		   	  <label for="inputAddress">10. Tipo de Buque<span style="color: #FF0000; font-weight: bold;"></span></label>
			                             <span id="bqship" class="form-control col-md-12"  runat="server" clientidmode="Static"  enableviewstate="False">...</span>

		   </div>
		  </div>
  

        	  	  <div class="form-row">
		  
		   <div class="form-group col-md-6"> 
		   	  <label for="inputAddress">11. Viaje entrante<span style="color: #FF0000; font-weight: bold;"></span></label>
			  <div class="d-flex">
                      <asp:TextBox  class="form-control" ID="tvIn" runat="server" MaxLength="20" ClientIDMode="Static"
                                    onkeypress="return soloLetras(event,'abcdefghijklmnopqrstuvwxyz1234567890-/._ ')" 
                                    onblur="cadenareqerida(this,1,20,'tvInv');"
                                    placeholder="INBOUND">
                            </asp:TextBox>
                            <span id="tvInv" class="validacion"> * </span>

			  </div>
		   </div>
		   
		     <div class="form-group col-md-6"> 
		   	  <label for="inputAddress">12. Número saliente<span style="color: #FF0000; font-weight: bold;"></span></label>
			  <div class="d-flex">
                       <asp:TextBox  class="form-control" ID="tvOu" runat="server" MaxLength="20"  ClientIDMode="Static"
                                onkeypress="return soloLetras(event,'abcdefghijklmnopqrstuvwxyz1234567890-/._ ')" 
                                onblur="cadenareqerida(this,1,20,'tvOuv');"
                                placeholder="OUTBOUND">
                            </asp:TextBox>
                            <span id="tvOuv" class="validacion"> * </span>

			  </div>
		   </div>
		  </div>

</div>
       
        
         <div 
	  class="form-title barra_colapser barra_cerrar "  data-toggle="collapse" data-target="#row_pib"  aria-controls="row_pib"  role="button"  aria-expanded="true"   
	  >
            PBIP y Seguridad
        </div>
        <div id="row_pib" class="show">
        	<div class="form-row">
		  
		   <div class="form-group col-md-4"> 
		   	  <label for="inputAddress">13. PBIP Número<span style="color: #FF0000; font-weight: bold;"></span></label>
			  <div class="d-flex">
                    <asp:TextBox  class="form-control" ID="tbipnum" runat="server" MaxLength="11" 
                                onkeypress="return soloLetras(event,'abcdefghijklmnopqrstuvwxyz1234567890')" 
                                onblur="cadenareqerida(this,1,40,'valpbi');"
                                placeholder="PBPIP" ClientIDMode="Static">
                            </asp:TextBox>
                            <span id="valpbi" class="validacion"> * </span>

			  </div>
		   </div>
		   
		     <div class="form-group col-md-4"> 
		   	  <label for="inputAddress">14. Válido hasta<span style="color: #FF0000; font-weight: bold;"></span></label>
			  <div class="d-flex">
                                              <asp:TextBox ID="pbhasta" runat="server"  CssClass="datepicker form-control" placeholder="Fecha" onkeypress="return soloLetras(event,'01234567890/')" onblur="valDate(this,true,valhasta);" ClientIDMode="Static"></asp:TextBox>
                            <span id="valhasta" class="validacion"> * </span>


			  </div>
		   </div>

                 <div class="form-group col-md-4"> 
		   	  <label for="inputAddress">15. Provisional<span style="color: #FF0000; font-weight: bold;"></span></label>
			  <div class="d-flex">
                        <label class="checkbox-container">
                                    <asp:radiobutton ID="rbsi" ClientIDMode="Static" runat="server" text="Si"  AutoPostBack="false" GroupName="gender"/>
                                    <span class="checkmark"></span>
                                </label>
                                <label for="inputEmail4">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</label>
                                <label class="checkbox-container"> 
                                    <asp:radiobutton ID="rbno" ClientIDMode="Static" runat="server" text="No" Checked="true"  AutoPostBack="false" GroupName="gender"/>
                                    <span class="checkmark"></span>
                                </label>
                                <label for="inputEmail4">&nbsp;&nbsp;&nbsp; </label>

			  </div>
		   </div>
		  </div>
       
            		   
			   	  	  <div class="form-row">
		  
		   <div class="form-group col-md-6"> 
		   	  <label for="inputAddress">16. Seguridad<span style="color: #FF0000; font-weight: bold;"></span></label>
			  <div class="d-flex">
                   <select id='dpseguro' class="form-control" onchange="segVal(this);">
                                    <option value="0">*Nivel*&nbsp;</option>
                                    <option value="1">1</option>
                                    <option value="2">2</option>
                                    <option value="3">3</option>
                            </select>
                            <span id="va_nivel" class="validacion"> * </span>

			  </div>
		   </div>
		   
		     <div class="form-group col-md-6"> 
		   	  <label for="inputAddress">17. Compañía de Seguridad<span style="color: #FF0000; font-weight: bold;"></span></label>
			  <div class="d-flex">
                       <asp:DropDownList ID="dpseguros_cia" class="form-control" runat="server"  onchange="segSEGCIA(this);" ClientIDMode="Static" enableviewstate="false"></asp:DropDownList>
                            <a class="tooltip" ><span class="classic" >Compañía de Seguridad, durante la permanencia del Buque en la terminal</span>
                                <img alt='' src='../shared/imgs/info.gif' class='datainfo'/>
                            </a>
                            <span id="va_nivel_cia" class="validacion"> * </span>

			  </div>
		   </div>
		  </div>
        
        </div>
      
         <div 
	  class="form-title barra_colapser barra_cerrar "  data-toggle="collapse" data-target="#row_pto"  aria-controls="row_pto"  role="button"  aria-expanded="true"   
	  >
            Puertos itinerario
        </div>
		<div class="form-row show" id="row_pto">
		  
		   <div class="form-group col-md-6"> 
		   	  <label for="inputAddress">18. Último Puerto<span style="color: #FF0000; font-weight: bold;"></span></label>
			  <div class="d-flex">
                                              <span id="lpuerto" class="form-control" runat="server" clientidmode="Static"  enableviewstate="False">...</span>
                            <input type="hidden" id="hlpuerto" value=""/>
                            <a  class="btn btn-outline-primary mr-4" target="popup" onclick="setPort(1);" ><span class='fa fa-search' style='font-size:24px'></span></a>


			  </div>
		   </div>
		   
		     <div class="form-group col-md-6"> 
		   	  <label for="inputAddress">19. Próximo Puerto<span style="color: #FF0000; font-weight: bold;"></span></label>
			  <div class="d-flex">
                                              <span id="npuerto" class="form-control" runat="server" clientidmode="Static"  enableviewstate="False">...</span>
                            <input type="hidden" id="hnpuerto" value=""/>
                            <a class="btn btn-outline-primary mr-4" target="popup" onclick="setPort(2)" ><span class='fa fa-search' style='font-size:24px'></span></a>


			  </div>
		   </div>
		  </div>
      
        
         <div 
	  class="form-title barra_colapser barra_cerrar "  data-toggle="collapse" data-target="#row_adu"  aria-controls="row_adu"  role="button"  aria-expanded="true"   
	  >
            Información de Senae
        </div>
   	    <div class="form-row show" id="row_adu">
		  
		   <div class="form-group col-md-6"> 
		   	  <label for="inputAddress">20. Manifiesto de Importación<span style="color: #FF0000; font-weight: bold;"></span></label>
			  <div class="d-flex">
                                              <asp:TextBox  class="form-control" ID="tmrn" runat="server" MaxLength="16" 
                             onkeypress="return soloLetras(event,'abcdefghijklmnopqrstuvwxyz1234567890')" 
                             onblur="cadenareqerida(this,1,16,'valmrn');"
                             placeholder="CEC0000XXXX0000" ClientIDMode="Static" Text="CEC">
                            </asp:TextBox>
                            <span id="valmrn" class="validacion"> * </span>


			  </div>
		   </div>
		   
		     <div class="form-group col-md-6"> 
		   	  <label for="inputAddress">21. Manifiesto de Exportación<span style="color: #FF0000; font-weight: bold;"></span></label>
			  <div class="d-flex">
                                              <asp:TextBox  class="form-control" ID="tdae" runat="server" MaxLength="16" 
                             onkeypress="return soloLetras(event,'abcdefghijklmnopqrstuvwxyz1234567890',false)" Text="CEC"
                              onblur="cadenareqerida(this,1,16,'sdae');"
                             placeholder="CEC0000XXXX0000" ClientIDMode="Static">
                            </asp:TextBox>
                            <span id="sdae" class="validacion"> * </span>


			  </div>
		   </div>
		  </div>
     
         <div 
	  class="form-title barra_colapser barra_cerrar "  data-toggle="collapse" data-target="#row_apg"  aria-controls="row_apg"  role="button"  aria-expanded="true"   
	  >
            Información APG

        </div>
    	<div class="form-row show" id="row_apg">
		  
		   <div class="form-group col-md-6"> 
		   	  <label for="inputAddress">22. Año de registro<span style="color: #FF0000; font-weight: bold;"></span></label>
			  <div class="d-flex">
                              <asp:TextBox  class="form-control" ID="tyear" runat="server" MaxLength="4" 
                                onkeypress="return soloLetras(event,'1234567890')" 
                                onblur="cadenareqerida(this,1,4,'sanio');"
                                placeholder="Numero" ClientIDMode="Static">
                            </asp:TextBox>
                            <a class="tooltip" ><span class="classic" >Año del registro portuario</span>
                              <img alt='' src='../shared/imgs/info.gif' class='datainfo'/>
                            </a>
                            <span id="sanio" class="validacion"> * </span>

			  </div>
		   </div>
		   
		     <div class="form-group col-md-6"> 
		   	  <label for="inputAddress">23. Número de registro<span style="color: #FF0000; font-weight: bold;"></span></label>
			  <div class="d-flex">
                                  <asp:TextBox  class="form-control" ID="tregistro" runat="server" MaxLength="6" 
                                onkeypress="return soloLetras(event,'1234567890',false)" 
                                onblur="cadenareqerida(this,1,8,'sreg');"
                                placeholder="Registro" ClientIDMode="Static">
                            </asp:TextBox>
                            <a class="tooltip" ><span class="classic" >Número de registro otorgado por APG</span>
                                <img alt='' src='../shared/imgs/info.gif' class='datainfo'/>
                            </a>
                            <span id="sreg" class="validacion"> * </span>

			  </div>
		   </div>
		  </div>  

		 <div 
	  class="form-title barra_colapser barra_cerrar "  data-toggle="collapse" data-target="#row_fec"  aria-controls="row_fec"  role="button"  aria-expanded="true"   
	  >
            Fechas de Operación (Estimadas)
        </div>
		<div class="show" id="row_fec">
			   	  	  <div class="form-row">
		  
		   <div class="form-group col-md-6"> 
                        <label for="inputEmail4">24. <strong>ETA:</strong>  Fecha estimada de arribo a boya de Data Posorja<span style="color: #FF0000; font-weight: bold;"> *</span></label>
			  <div class="d-flex">
                        <asp:TextBox ID="teta" runat="server" MaxLength="16"  class="form-control" 
                                onkeypress="return soloLetras(event,'1234567890/ ')" 
                                onblur="valDate(this,true,seta);"
                                CssClass="datetimepicker form-control"
                                placeholder="ETA" 
                                ClientIDMode="Static">
                            </asp:TextBox>
                            <span id="seta" class="validacion"> * </span>

			  </div>
		   </div>
		   
		                <div class="form-group col-md-6">
                        <label for="inputEmail4">25.<strong> ETB:</strong> Fecha estimada de Atraque muelle CGSA <span style="color: #FF0000; font-weight: bold;"> *</span></label>
                        <div class="d-flex">
                            <asp:TextBox ID="tetb" runat="server" MaxLength="16" 
                            onkeypress="return soloLetras(event,'1234567890/ ')" 
                            onblur="valDate(this,true,setb);sumarHorasFecha();"
                            CssClass="datetimepicker form-control"
                            placeholder="ETB" ClientIDMode="Static">
                            </asp:TextBox>
                            <span id="setb" class="validacion"> * </span>
                        </div>
                    </div>
		  </div>

                    	  <div class="form-row">
		  
		      <div class="form-group col-md-6">
                        <label for="inputEmail4">26. Número de horas uso de muelle <span style="color: #FF0000; font-weight: bold;"> *</span></label>
                        <div class="d-flex">
                            <asp:TextBox  class="form-control" ID="thoras" runat="server" MaxLength="3" ClientIDMode="Static"
                             onkeypress="return soloLetras(event,'1234567890 /',false)" 
            
                             onblur=" cadenareqerida(this,1,8,'shora');sumarHorasFecha();"
                             placeholder="Horas uso">
                            </asp:TextBox>
                            <span id="shora" class="validacion"> * </span>
                        </div>
                    </div>
		   
		         <div class="form-group col-md-6">
                        <label for="inputEmail4">27.<strong> ETS:</strong>  Fecha estimada de zarpe <span style="color: #FF0000; font-weight: bold;"> *</span></label>
                        <div class="d-flex">
                            <asp:TextBox ID="tets" runat="server" CssClass="datetimepicker form-control"  MaxLength="16" ClientIDMode="Static" ReadOnly="true"></asp:TextBox>
                            <a class="tooltip" ><span class="classic" >Se calcula a partir de el ETB + uso</span>
                                <img alt='' src='../shared/imgs/info.gif' class='datainfo'/>
                            </a>
                        </div>
                    </div>
		  </div>


             </div>

         <div 
	  class="form-title barra_colapser barra_cerrar "  data-toggle="collapse" data-target="#row_nto"  aria-controls="row_nto"  role="button"  aria-expanded="true"   
	  >
            Datos para notificación

        </div>
		<div class="show" id="row_nto">
		  
	                <div class="form-row">
                    <div class="form-group col-md-12">
                        <label for="inputEmail4">
                            28. Correo(s) para notificación: 
                            <i>
                                <small>
                                    (Estos correos se utilizarán para notificar en caso que sea necesaria una inspección)
                                </small>
                            </i> 
                            <span style="color: #FF0000; font-weight: bold;"> *</span>
                        </label>
                         <div id='TextBoxesGroup'>
                       <div id="TextBoxDiv1" class=" form-inline">
                           <span>mail #1:</span><input type='text' id='textbox1' class=" form-control" name='textbox1'  runat="server"
                            enableviewstate="false" clientidmode="Static"
                           onkeypress="return soloLetras(event,'abcdefghijklmnñopqrstuvwxyz1234567890_$@.',true)"  
                            onblur="cadenareqerida(this,1,100,'valmail');"
                             placeholder="mail@mail.com"
                           />
                       </div>
                     </div>
                        <span id="valmail" class="validacion"></span>
                    </div>
                </div>

		    <div class="form-row">
		   <div class="form-group col-md-12 "> 
		     <div class="d-flex">
                <input id="addButton" type='button' class="btn btn-outline-primary"  title="Agregar Mail" value="Agregar" />
                <input id="removeButton" type='button' class="btn btn-outline-primary ml-2"  title="Borrar Mail" value="Remover" />

		     </div>
		   </div> 
		   </div>
	
		  </div>
        <div class="form-title">4.- Responsabilidad de la información </div>
        <div class="form-row">
		  
		   <div class="form-group col-md-12"> 
		         <div class="alert alert-danger">
                    Los datos proporcionados son de entera responsabilidad de quien los consigna, por lo que CONTECON GUAYAQUIL S.A. no se responsabiliza por cualquier error o falsedad que los mismos pudieren tener, siendo de cuenta del cliente todos los gastos y perjuicios que por dicho error se ocasionen a la carga.
                    <br />Esta solicitud de atraque, no implica la aceptación de la nave o asignación de muelle.<br />
                    </div>
		   </div>
		   
	
		  </div>
        <div class="form-row">
		   <div class="col-md-12 d-flex justify-content-center"> 
		                      
          

                        <input id="btsalvar" type="button" class="btn btn-primary" value="Generar Solicitud" onclick="imprimir();" /> &nbsp;

                        <span id="imagen"></span>
                   
                                        <img alt="loading.." src="../shared/imgs/loader.gif" id="loader" class="nover"  />


		   </div> 
		   </div>
    </div>
    <script  type="text/javascript" src="../Scripts/pages.js"></script>
    <script src="../Scripts/atraque.js" type="text/javascript"></script>
        <script src="../Scripts/jquery.datetimepicker.js" type="text/javascript"></script>

    <script type="text/javascript">
       //variables;
        var jSolicitud = {};
        var sw = 0;
        var lineasAsociadas = [];
                //Creacion de Variable incremental
        var incremento = 1;

            //objeto a transportar.
        $(document).ready(function () {
            //inicia los fecha-hora
            $('.datetimepicker').datetimepicker().datetimepicker({ lang: 'es', format: 'd/m/Y H:i', step: 30 });
            //inicia los fecha
            $('.datepicker').datetimepicker().datetimepicker({ lang: 'es', timepicker: false, format: 'd/m/Y', closeOnDateSelect: true, minDate: '0' });
            //colapsar y expandir
            var counter = 2;
            $("#addButton").click(function () {
                if (counter > 5) {
                    alertify.alert("Solo se permiten 5 mails");
                    return false;
                }
                $('<div/>', { 'id': 'TextBoxDiv' + counter ,'class':'form-inline'}).html($('<span/>').html('mail #' + counter + ':')).append($('<input type="text" class="form-control"  placeholder="mail@mail.com" >').attr({ 'id': 'textbox' + counter, 'name': 'textbox' + counter, onkeypress: 'return soloLetras(event,"abcdefghijklmnñopqrstuvwxyz1234567890_@.",true);' })).appendTo('#TextBoxesGroup')
                counter++;
            });
            $("#removeButton").click(function () {
                if (counter == 2) {
                  alertify.alert("Un mail es obligatorio");
                    return false;
                }
                counter--;
                $("#TextBoxDiv" + counter).remove();
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
        function imprimir()
        {
            try
            {
                if (!confirm('Está seguro de que desea generar la solicitud, este proceso es IRREVERSIBLE?'))
                {
                    return;
                }
                    document.getElementById("loader").className = '';
                    //poblar Objeto
                    populateObject(jSolicitud, lineasAsociadas);

                     //validarlo objeto
                    if (!validaciones(jSolicitud))
                    {
                        document.getElementById("loader").className = 'nover';
                        return ;
                    }
                    invokeJsonTransportAT(jSolicitud, '../atraque/solicitud.aspx/ValidateJSON');

                }
            catch (e)
            {
                alert(e.Message);
                return false;
            }

           
        }

      function populateObject(objeto, arreglo) {
          //variable = (condition) ? true-value : false-value;
          objeto.service = (document.getElementById('dpservicio').value != undefined) ? document.getElementById('dpservicio').value : '';
          objeto.imo = (objeto.imo != undefined) ? objeto.imo : '';
       //  alert('P1');
          objeto.vIn = (document.getElementById('tvIn').value != undefined) ? document.getElementById('tvIn').value : '';
          objeto.vOut = (document.getElementById('tvOu').value != undefined) ? document.getElementById('tvOu').value : '';
          objeto.imrn = (document.getElementById('tmrn').value != undefined) ? document.getElementById('tmrn').value : '';
       //   alert('P2');
          objeto.emrn = (document.getElementById('tdae').value != undefined) ? document.getElementById('tdae').value : '';
          objeto.anio = (document.getElementById('tyear').value != undefined) ? document.getElementById('tyear').value : '';
        //  alert('P3');
          objeto.regis = (document.getElementById('tregistro').value != undefined) ? document.getElementById('tregistro').value : '';
        //  objeto.eta = (document.getElementById('teta').value != undefined) ? document.getElementById('teta').value : ''

          objeto.eta = (document.getElementById('tetb').value != undefined) ? document.getElementById('tetb').value : '';

          //  alert('P4');
        //  objeto.etb = (document.getElementById('tetb').value != undefined) ? document.getElementById('tetb').value : ''

          objeto.etb = (document.getElementById('teta').value != undefined) ? document.getElementById('teta').value : '';

          objeto.uso = (document.getElementById('thoras').value != undefined) ? document.getElementById('thoras').value : '';
       //   alert('P5');
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
          objecLines(objeto, arreglo, 'tblineas');
          objeto.seguro = (document.getElementById('dpseguros_cia').value != undefined) ? document.getElementById('dpseguros_cia').value : '';
      }
        $(document).ready(function () {
            $('.datetimepicker').datetimepicker().datetimepicker({ lang: 'es', timepicker: false, closeOnDateSelect: true, format: 'd/m/Y' });
            $('.datepicker').datetimepicker().datetimepicker({ lang: 'es', timepicker: false, format: 'd/m/Y', closeOnDateSelect: true, minDate: '0' });

        });
    </script>
     <script src="../Scripts/hide.js" type="text/javascript"></script>
</asp:Content>