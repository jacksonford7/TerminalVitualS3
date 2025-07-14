<%@ Page Title="AISV Contenedores"  Language="C#" MasterPageFile="~/site.Master" AutoEventWireup="true" CodeBehind="consolidadora.aspx.cs" Inherits="CSLSite.consolidadora" %>
<%@ MasterType VirtualPath="~/site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="placehead" runat="server">
    <link href="../img/favicon2.png" rel="icon"/>
  <link href="../img/icono.png" rel="apple-touch-icon"/>
  <link href="../css/bootstrap.min.css" rel="stylesheet"/>
  <link href="../css/dashboard.css" rel="stylesheet"/>
  <link href="../css/icons.css" rel="stylesheet"/>
  <link href="../css/style.css" rel="stylesheet"/>
  <link href="https://fonts.googleapis.com/css2?family=Roboto:wght@300;400;500;700&display=swap" rel="stylesheet"/>
  
       <!--mensajes-->
        <link href="https://cdn.jsdelivr.net/npm/alertifyjs@1.11.0/build/css/alertify.min.css" rel="stylesheet"/>
        <script type="text/javascript" src="https://cdn.jsdelivr.net/npm/alertifyjs@1.11.0/build/alertify.min.js"></script>

    <link href="../shared/estilo/jquery.datetimepicker.css" rel="stylesheet" type="text/css" />
   <script src="../Scripts/jquery.datetimepicker.js" type="text/javascript"></script>

 </asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="placebody" runat="server">

    <input id="ruc_dae" type="hidden" value="" runat="server"  clientidmode="Static" />

    <div class="mt-4">         
        <nav class="mt-4" aria-label="breadcrumb">
          <ol class="breadcrumb">
            <li class="breadcrumb-item" id="opcion_principal" runat="server"><a href="#">AISV</a></li>
            <li class="breadcrumb-item active" aria-current="page" id="sub_opcion" runat="server">Exportación para Contenedores Consolidados</li>
          </ol>
        </nav>
      </div>


 <!-- White Container -->
<div class="dashboard-container p-4" id="cuerpo" runat="server">

        <div class="alert alert-warning">
            <span id="dtlo" runat="server">Estimado usuario:</span> 
           <br /> Para este tipo de AISV, en el campo “Documento de Aduana” se debe registrar dos o más <strong> DAE de Carga Suelta.</strong> La transmisión al sistema de Aduana se efectuará por cada DAE registrada en este formulario, confirmando la cantidad de bultos indicados. Aceptado el registro no se realizarán modificaciones ni correcciones.

    </div>
         <div class="form-title barra_colapser barra_cerrar "  data-toggle="collapse" data-target="#colexpo"  aria-controls="colexpo"  role="button"  aria-expanded="true"   >
            Datos del Booking 
         </div>
           <div  id="colexpo" class="show">
           <div class="form-row">
             <div class="form-group col-md-12">
                  <label for="inputEmail4">1. Nombre del Exportador</label>
                 <div class="d-flex">
                  <span id="numexpo" class="form-control col-md-6"   runat="server" clientidmode="Static"  enableviewstate="False">...</span>&nbsp;
                  <span id="nomexpo" class="form-control col-md-6"  runat="server" clientidmode="Static"  enableviewstate="False">...</span>
                  <input id="numexport" type="hidden" runat="server" clientidmode="Static"  enableviewstate="False" />
                 </div>
              </div>  
          </div>
          <div class="form-row">
               <div class="form-group col-md-6">
                         <label for="inputAddress">2. Línea de Naviera <span style="color: #FF0000; font-weight: bold;"> *</span></label>
                         <div class="d-flex">
                                <span id="linea" class="form-control col-md-11">...</span>
                                <input id="lineaCI" type="hidden" />
                                <a  class="btn btn-outline-primary mr-4" target="popup" onclick="window.open('../catalogo/lineas.aspx','name','width=850,height=880')" >
                                <span class='fa fa-search' style='font-size:24px'></span> </a>
                             <span class="validacion" id="xplinea" > </span>
                         </div>
               </div> 
               <div class="form-group col-md-6">
                    <label for="inputAddress">3. Booking<span style="color: #FF0000; font-weight: bold;"> *</span></label>
                    <div class="d-flex">
                         <span id="numbook" class="form-control col-md-11">...</span>
                         <a  class="btn btn-outline-primary mr-4" target="popup" onclick="linkbokin('lineaCI','numexport','FCL');" >
                                <span class='fa fa-search' style='font-size:24px'></span> </a>
                        <span class="validacion"  id="xpbok"></span>
                    </div>
               </div> 
          
              </div>
            </div>


      <div class="form-title barra_colapser barra_cerrar" data-toggle="collapse" data-target="#colbook" aria-controls="colbook" role="button" aria-expanded="true">
               Verificación Datos
          </div>
           <div class="show" id="colbook">
           <div class="">Confirme que los datos sean correctos. En caso de error, favor comuníquese con el Departamento de Planificación a los teléfonos: +593 (04) 6006300, 3901700 ext.  ext. 4039, 4040, 4060, 4040, 4039.</div>
          
            <div class="form-row">
	           <div class="form-group col-md-4">
                    <label for="inputEmail4">4. Referencia <i>CONTECON</i></label>
                     <span id="referencia" class="form-control col-md-12">... </span>
                </div>

                   <div class="form-group col-md-4">
                     <label for="inputEmail4">5. Nave</label>
                       <span id="buque" class="form-control col-md-12">...</span>
                </div> 

                              <div class="form-group col-md-4">
                     <label for="inputEmail4">9. Agencia Naviera</label>
                       <span id="agencia" class="form-control col-md-12">...</span>
                </div> 
	       </div>

              <div class="form-row">
	                  <div class="form-group col-md-4">
                     <label for="inputEmail4">6. Fecha estimada de arribo [ETA]</label>
                      <span id="eta" class="form-control col-md-12">...</span>
                </div> 
              <div class="form-group col-md-4">
                     <label for="inputEmail4">7. Fecha límite [CutOff]</label>
                       <span id="cutof" class="form-control col-md-12">...</span>
                </div> 
               <div class="form-group col-md-4">
                     <label for="inputEmail4">8. Último Ingreso [UIS]</label>
                      <span id="uis" class="form-control col-md-12">...</span>
                </div> 
	       </div>


               <div class="form-row"> 
                              <div class="form-group col-md-3">
                     <label for="inputEmail4">10. Puerto destino</label>
                     <span id="descarga" class="form-control col-md-12">...</span>
                </div> 
              <div class="form-group col-md-3">
                     <label for="inputEmail4">11. Puerto Final</label>
                     <span id="final" class="form-control col-md-12">...</span>
                </div> 
              <div class="form-group col-md-6">
                     <label for="inputEmail4">12. Producto declarado</label>
                    <span id="producto" class="form-control col-md-12">...</span>
                </div> 

               </div>


               <div class="form-row"> 
                         <div class="form-group col-md-6">
                     <label for="inputEmail4">13. Tamaño de contenedor</label>
                      <span id="tamano" class="form-control col-md-12">...</span>
                </div> 
              <div class="form-group col-md-6">
                     <label for="inputEmail4">14. Tipo de contenedor</label>
                       <span id="tipo" class="form-control col-md-12">...</span>
                </div> 


               </div>


               <div class="form-row"> 
                 <div class="form-group col-md-12"> 
                      <label for="inputEmail4">15. Características</label>
                     
                   <div class="form-check form-check-inline">
             <input id="imo" class="form-check-input" type="checkbox"  value="imo"  disabled/>
             <label class="form-check-label" for="inlineCheckbox1">IMO</label>
               </div>

                   
<div class="form-check form-check-inline">
  <input  id="refer"class="form-check-input" type="checkbox"  value="Reefer" disabled />
  <label class="form-check-label" for="inlineCheckbox1">Reefer</label>
</div>

                   
<div class="form-check form-check-inline">
  <input class="form-check-input" type="checkbox"  value="Sobredimensionado" id="over" onclick="radio(this,ckdimen);" disabled/>
  <label class="form-check-label" for="inlineCheckbox1">Sobredimensionado</label>
</div>

                   
<div class="form-check form-check-inline">
  <input onchange="popupWarn(this);" id="cklate" class="form-check-input" type="checkbox"  value="A. Tardio" />
  <label class="form-check-label" for="inlineCheckbox1">Arribo tardío</label>
</div>

<div class="form-check form-check-inline">
  <input id="ckdip" class="form-check-input" type="checkbox"  value="option1" />
  <label class="form-check-label" for="inlineCheckbox1">Diplomático</label>
</div>
	              </div>

               </div>


                            <div class="form-row">

                      <div class="form-group col-md-8">
                     <label for="inputEmail4">17. Notas del Booking</label>
                     <span class="form-control col-md-12" id="remar"> </span>       
                </div> 

	        <div class="form-group col-md-4"> 
                 <label for="inputEmail4">16. Estiba bajo cubierta</label><br />

                <div class="form-check-inline">
  <input class="form-check-input" type="radio" name="deck" id="rbsi" value="SI"  disabled />
  <label class="form-check-label" for="rbsi">
    SI
  </label>
</div>
<div class="form-check-inline">
  <input class="form-check-input" type="radio" name="deck" id="rbno" value="NO"  disabled/>
  <label class="form-check-label" for="rbno">
    NO
  </label>
</div>
	       </div>
	       </div>







           </div>

   <div class="form-title barra_colapser barra_cerrar" data-toggle="collapse" data-target="#colcond" aria-controls="colcond" role="button" aria-expanded="true">
              Datos del Contenedor
           </div>
          <div id="colcond" class="show">
             

    <div class="form-row">
        <div class="form-group col-md-3">
                     <label for="inputEmail4">18. Número del contenedor <span style="color: #FF0000; font-weight: bold;"> *</span></label>
                        <div class="d-flex">
  <asp:TextBox ID="txtcontenedor" runat="server"  MaxLength="11"  class="form-control"
                             onkeypress="return soloLetras(event,'abcdefghijklmnñopqrstuvwxyz1234567890')" 
                              onBlur="checkDC(this,'valcont',true);"
                              
                             ></asp:TextBox>
                            <span id="valcont" class="validacion"></span>
                  </div>
            
          
                </div> 
              <div class="form-group col-md-3">
                     <label for="inputEmail4">19. Tara del contenedor [TON]</label>
                      <span id="tara" class="form-control">00.00</span>
              </div> 
              <div class="form-group col-md-3">
                     <label for="inputEmail4">20. Max.&nbsp;<i>Payload</i>&nbsp; [TON] <span style="color: #FF0000; font-weight: bold;"> *</span></label>
                    
                     <div class="d-flex">
  <asp:TextBox ID="txtpay" runat="server" class="form-control" 
                 MaxLength="5"  ClientIDMode="Static"
                onBlur="valrange(this,1,60,'valpayload',true);"
                onkeypress="return soloLetras(event,'1234567890.')"
                
                >00.00</asp:TextBox>
                  <span id="valpayload" 
                 class="validacion" ></span>
                  </div>
                
             </div> 
          <div class="form-group col-md-3">
                     <label for="inputEmail4">21. Peso bruto &nbsp;[TON]
                       
                         <span style="color: #FF0000; font-weight: bold;"> *</span></label>

                  <div class="d-flex">
<span id="txtpeso" class=" form-control col-md-12"  >00.00</span>
                 <span id="valpes" class="validacion"></span>
                  </div>

                  
            </div>
        
    </div>

        <div class="form-row">
               <div class="form-group col-md-6">
                 <label for="inputEmail4">22. Origen del contenedor <i>(Depot)</i> <span style="color: #FF0000; font-weight: bold;"> <span style="color: #FF0000; font-weight: bold;"> *</span></span></label>
                 <div class="d-flex">
                      <asp:DropDownList ID="dporigen" runat="server" class="form-control"  onchange="valdpme(this,0,fechax);" >
                         <asp:ListItem Value="0">* Seleccione *</asp:ListItem>
                     </asp:DropDownList>
               
                     
                 </div>
            </div>
                     <div class="form-group col-md-6"> 
		   	  <label for="inputAddress">Fecha/Hora del retiro<span style="color: #FF0000; font-weight: bold;"></span></label>
			  <div class="d-flex">
                    <input type="text"
                             id="txtorigen"  maxlength="16" class="datetimepicker form-control" 
                               onkeypress="return soloLetras(event,'1234567890/:')"
                               onblur="valDate(this,true,fechax);"  
                             /><span id="fechax" class="validacion" ></span>

			  </div>
		   </div>

               </div> 

    <div class="form-row">

          <div class="form-group col-md-6">
               <label for="inputEmail4">23. Código de peligrosidad (IMO): <span style="color: #FF0000; font-weight: bold;"> </span></label>
               <asp:DropDownList ID="dpimo" runat="server" class="form-control" EnableViewState="False" 
                 ClientIDMode="Static" >
                 <asp:ListItem Selected="True" Value="0">* Código de peligrosidad *</asp:ListItem>
             </asp:DropDownList>
             <span id="valsimo" class="validacion"> </span>
         </div>
         <div class="form-group col-md-6">
               <label for="inputEmail4">24. Consignatario: <span style="color: #FF0000; font-weight: bold;">* </span></label>
              <asp:TextBox ID="consignee" runat="server" MaxLength="250" class="form-control"
               onblur="cadenareqerida(this,8,250,'valconsig');"
               ></asp:TextBox>
             <span class="validacion" id="valconsig"  ></span>
         </div>
    </div>
 </div>


            <div class="form-title barra_colapser barra_cerrar" data-toggle="collapse" data-target="#colref" aria-controls="colref" role="button" aria-expanded="true">
               Datos sobre refrigeración
           </div>
    <div id="colref" class=" show">
          <div class="form-row">
                <div class="form-group col-md-6">      
                       <label for="inputEmail4">25. Tipo de refrigeración <span style="color: #FF0000; font-weight: bold;"> *</span></label>
                         <div class="d-flex">
  <asp:DropDownList ID="dprefrigera" runat="server" class="form-control" onchange="valdpmeref(this,0,valrefri,tipor,1)"
                        ClientIDMode="Static" disabled>
                         <asp:ListItem Selected="True" Value="0">* Tipo de refrigeración *</asp:ListItem>
                     </asp:DropDownList>
                    <span id='valrefri' class="validacion" ></span>
                  </div>
                    
                  
                </div> 
                <div class="form-group col-md-6">
                     <label for="inputEmail4">26. Temperatura [°C] <span style="color: #FF0000; font-weight: bold;"> *</span></label>
                      <div class="d-flex">
    <asp:TextBox ID="txttemp" runat="server"  MaxLength="5" class="form-control"
                       onkeypress="return soloLetras(event,'1234567890.-')" ClientIDMode="Static"
                         onblur="valrange(this,-60,60,'valtemp');" 
                           disabled
                          data-toggle="tooltip" data-placement="top" title=" Asegúrese que esta temperatura coincida con la carta de temperatura."
                     >0.0</asp:TextBox>
                    <span id='valtemp' class="validacion" ></span>
                  </div>
                    
                
                </div> 
           </div>

           <div class="form-row">

               <div class="form-group col-md-4">          
                <label for="inputEmail4">27. Humedad &nbsp;[CBM] </label>
                    <div class="d-flex">
                      <asp:DropDownList ID="dphumedad" runat="server" class="form-control" ClientIDMode="Static" disabled>
                         <asp:ListItem Selected="True" Value="0">* Humedad *</asp:ListItem>
                  </asp:DropDownList>
                   <span id='valhum' class="validacion"></span>

                    </div>
               </div> 
               <div class="form-group col-md-4">          
                <label for="inputEmail4">28. Ventilación&nbsp;[%] </label>
                    <div class="d-flex">
                    <asp:DropDownList ID="dpventila" runat="server" class="form-control"  ClientIDMode="Static" disabled>
                         <asp:ListItem Selected="True" Value="0">* Tipo de ventilación *</asp:ListItem>
                        
                  </asp:DropDownList>
                   <span id='valventi' class="validacion" > </span>

                    </div>
                </div> 
                <div class="form-group col-md-4">          
                    <label for="inputEmail4">29. Refrigeración para inspección&nbsp; </label>
                     <div class="d-flex">
                         <asp:DropDownList ID="dprefservice" runat="server" class="form-control" ClientIDMode="Static" disabled 
                             data-toggle="tooltip" data-placement="top" title="El tipo de refrigeración de apoyo que será usado durante la inspección (PNA) ">
                         <asp:ListItem Selected="True" Value="0">* No programar *</asp:ListItem>
                  </asp:DropDownList>
                   <span id="valtiporef" class="opcional" > </span>

                     </div>
                 </div> 
          </div> 
</div>




         <div class="form-title barra_colapser barra_cerrar"  data-toggle="collapse" data-target="#colsel" aria-controls="colsel" role="button" aria-expanded="true">
              Datos sobre los sellos

          </div>
      <div class="show" id="colsel">
          <div class="form-row">
	          <div class="form-group col-md-3">          
                    <label for="inputEmail4">30. Sello de agencia <span style="color: #FF0000; font-weight: bold;"> *</span></label>
                      <div class="d-flex">
           <asp:TextBox ID="tseal1" runat="server" class="form-control"
                             onkeypress="return soloLetras(event,'abcdefghijklmnñopqrstuvwxyz1234567890',true)" 
                             MaxLength="14" onpaste="return false;" 
                             onblur="cadenareqerida(this,1,20,'valsel1');"
                             ></asp:TextBox>
                      <span id="valsel1" class="validacion" ></span>
                    
                  </div>
                  
       
                      
                </div> 
                <div class="form-group col-md-3">          
                    <label for="inputEmail4">31. Sello de ventilación [Reefer] </label>
                    
                      <div class="d-flex">
      <asp:TextBox ID="tseal2" runat="server"  class="form-control" ClientIDMode="Static" disabled
                             onkeypress="return soloLetras(event,'abcdefghijklmnñopqrstuvwxyz1234567890',true)" 
                             MaxLength="14"   onpaste="return false;" 
                            
                         ></asp:TextBox>
                         <span id="valselvent" class="opcional" ></span>
                  </div>
                    
              
                </div> 
                <div class="form-group col-md-3">          
                    <label for="inputEmail4" id="msc_sello">32. Sello adicional 1</label>
                    


                      <asp:TextBox ID="tseal3" runat="server"  class="form-control"
                         onkeypress="return soloLetras(event,'abcdefghijklmnñopqrstuvwxyz1234567890',true)" 
                         MaxLength="14"  onpaste="return false;" 
                         ></asp:TextBox>
                </div> 
                <div class="form-group col-md-3">          
                    <label for="inputEmail4">33. Sello adicional 2</label>
                      <div class="d-flex">
     <asp:TextBox ID="tseal4" runat="server"  class="form-control"
                             onkeypress="return soloLetras(event,'abcdefghijklmnñopqrstuvwxyz1234567890',true)" 
                             MaxLength="14" 
                              onpaste="return false;" 
                         ></asp:TextBox>
                         <span class="opcional" id="sel_cau" ></span>
                  </div>

                
                </div> 
	       </div>
          <div class="form-row">

                <div class="form-group col-md-8">          
                    <label for="inputEmail4">34. Responsable del sellado <span style="color: #FF0000; font-weight: bold;"> *</span></label>
                     <div class="d-flex">
      <asp:TextBox ID="tsresponsable" runat="server" class="form-control"
                             onkeypress="return soloLetras(event,'abcdefghijklmnñopqrstuvwxyz')" 
                             MaxLength="120" 
                             onblur="cadenareqerida(this,1,200,'valselper');"
                            
                                 data-toggle="tooltip" data-placement="top" title="Nombres y apellidos de la persona que estuvo a cargo del llenado y posterior cierre de la unidad, incluyendo la colocación de sellos."
                             ></asp:TextBox>
                             <span id="valselper" class="validacion" ></span>
                  </div>
                    
                    
              
                </div> 
                 <div class="form-group col-md-4">          
                    <label for="inputEmail4">35. Documento de identidad <span style="color: #FF0000; font-weight: bold;"> *</span></label>
                  
                       <div class="d-flex">
    <asp:TextBox ID="tsCedula" runat="server" MaxLength="14" class="form-control"
                      onkeypress="return soloLetras(event,'0123456789',true)"
                      onBlur="oCedulaValida(this,'valsced');" 
                      data-toggle="tooltip" data-placement="top" title="Número de identificación de la persona registrada en el campo anterior."></asp:TextBox>
                     <span class="validacion" id="valsced" ></span>
                  </div>
                     
                 
                </div>   
          </div> 
       </div>

               <div class="form-title barra_colapser barra_cerrar"  data-toggle="collapse" data-target="#coltax" aria-controls="coltax" role="button" aria-expanded="true">
               Datos del transporte / origen</div>
          
    <div class="show" id="coltax">
          <div class="form-row"> 

                 <div class="form-group col-md-3">
                   <label for="inputEmail4">36. Provincia (Planta)<span style="color: #FF0000; font-weight: bold;"> *</span></label>
                    <asp:DropDownList ID="dprovincia" runat="server" class="form-control"  onchange="invoke(this,'canton',loadcanton); valdpme(this,0,valprovincia);">
                         <asp:ListItem Selected="True" Value="0">Provincia</asp:ListItem>
                     </asp:DropDownList>

                </div> 
              <div class="form-group col-md-3">
                   <label for="inputEmail4">37. Canton (Planta)<span style="color: #FF0000; font-weight: bold;"> *</span></label>
                    		     <div class="d-flex">
    <select id="dcanton" class="form-control" >
                         <option selected="selected" value="0" >* Cantón *</option>
                     </select>
                     <span class="validacion" id="valprovincia" ></span>
                  </div>
                  
                  
              
               </div> 
              <div class="form-group col-md-3">
                     <label for="inputEmail4">38. Fecha/hora de salida <span style="color: #FF0000; font-weight: bold;"> *</span></label>
                       <div class="d-flex">
             <asp:TextBox ID="txtSalida" runat="server" MaxLength="16"  CssClass="datetimepicker form-control"
                           onkeypress="return soloLetras(event,'1234567890/:')"
                           onBlur="valDate(this,  true,valdatem);"
                         
                           ></asp:TextBox>
                          <span class="validacion" id="valdatem" ></span>   
                  </div>
                  
            
             </div>
               <div class="form-group col-md-3">          
                    <label for="inputEmail4">39. Horas viaje [Estimado] <span style="color: #FF0000; font-weight: bold;"> *</span></label>
                    
                      <div class="d-flex">
          <asp:TextBox ID="txthoras" runat="server" MaxLength="3" class="form-control"
                         onkeypress="return soloLetras(event,'1234567890',true)"
                          onBlur="valrange(this,1,2000,'valestima',true);" 
                         ></asp:TextBox>
                         <span class="validacion" id="valestima" ></span>
                  </div>
         


               </div> 
</div>

 <div class="form-row"> 
  <div class="form-group col-md-12">
                  <label for="inputEmail4">40. Ubicación de la planta Dirección<span style="color: #FF0000; font-weight: bold;"> *</span></label>
                    <asp:TextBox 
                            ID="txtdirec" runat="server" class="form-control"
                         onkeypress="return soloLetras(event,'abcdefghijklmnñopqrstuvwxyz1234567890')" 
                          onblur="cadenareqerida(this,1,2000,'valprovincia');"
                         MaxLength="150" ClientIDMode="Static"
                         placeholder="Direccion del area de consolidacion"></asp:TextBox>
              </div> 
</div>

          <div class="form-row"> 
 <div class="form-group col-md-3">          
                    <label for="inputEmail4">41. Placa del vehículo <span style="color: #FF0000; font-weight: bold;"> *</span></label>
                        <div class="d-flex">
   <asp:TextBox ID="txtplaca" runat="server" MaxLength="10"  class="form-control"
                        onkeypress="return soloLetras(event,'abcdefghijklmnopqrstuvwxyz0123456789')" 
                         onBlur="validarPlaca(this,'valplaca');buble_warning();"  ClientIDMode="Static"
                          ></asp:TextBox>
                         <span class="validacion" id="valplaca" ></span>
                  </div>
     
  
               </div> 

                       <div class="form-group col-md-6">
                     <label for="inputEmail4">42. Nombre del conductor <span style="color: #FF0000; font-weight: bold;"> *</span></label>
                       <span id="txtconductor" class="form-control"   runat="server" clientidmode="Static"  enableviewstate="False">...</span>
                        <span class="validacion" id="valnombre" ></span>
                </div> 
               <div class="form-group col-md-3">
                     <label for="inputAddress">43. Documento de identidad <span style="color: #FF0000; font-weight: bold;"> *</span></label>
                    <div class="d-flex">
                              <input id="driID" type="hidden" runat="server" clientidmode="Static"  enableviewstate="False" />
                             <span id="txtidentidad" class="form-control"
                             runat="server" clientidmode="Static"  enableviewstate="False">...</span>
                                 <a  class="btn btn-outline-primary mr-4" target="popup" onclick="window.open('../catalogo/chofer.aspx','name','width=850,height=880')" >
                                <span class='fa fa-search' style='font-size:24px'></span></a>
                            <span class="validacion" id="valced" ></span>
                        
                    </div>
               </div>

          </div>

         <div class="form-row"> 

  <div class="form-group col-md-6">
                     <label for="inputAddress">44. Certificado de Cabezal</label>
                    <div class="d-flex">
                             <asp:TextBox ID="certcabezal" runat="server" MaxLength="8" class="form-control"    
                            onkeypress="return soloLetras(event,'abcdefghijklmnopqrstuvwxyz0123456789',true)"
                           
                                  data-toggle="tooltip" data-placement="top" title="Si no agrega esta información, la misma será solicitada en la garita de ingreso"
                              ></asp:TextBox>
                            
                            <label for="inputEmail4" class="ml-1">Expira </label>
                         <input type="text" id="txtcerCab" maxlength="10"   class="datepicker form-control"
                           onkeypress="return soloLetras(event,'1234567890/',true)"
                           onblur="valDate(this,false,fechacab);"
                            />
                           <span class="opcional" id="fechacab" ></span>      
                    </div>
               </div>
                <div class="form-group col-md-6">
                     <label for="inputAddress">45. Certificado de Chasis</label>
                    <div class="d-flex">
                             <asp:TextBox ID="certchasis" runat="server" MaxLength="8" class="form-control mr-1"    
                            onkeypress="return soloLetras(event,'abcdefghijklmnopqrstuvwxyz0123456789',true)"
                            
                              data-toggle="tooltip" data-placement="top" title="Si no agrega está información la misma será solicitada, en la garita de ingreso"
                          ></asp:TextBox>
                             <label for="inputEmail4" class="ml-1">Expira </label>
                             <input type="text" id="txtcerCha" 
                               maxlength="10"  class="datepicker form-control"
                               onkeypress="return soloLetras(event,'1234567890/',true)"
                               onblur="valDate(this,false,fechach);"
                             
                              />   
                        <span class="opcional" id="fechach" ></span>
                    </div>
               </div>

         </div>

    <div class="form-row">
       
      
              
             
               <div class="form-group col-md-3">
                     <label for="inputAddress">46. Certificado especial</label>
                             <asp:TextBox ID="certespecial" runat="server" MaxLength="8" class="form-control"    
                           onkeypress="return soloLetras(event,'abcdefghijklmnopqrstuvwxyz0123456789',true)"
                           
                          ></asp:TextBox>

               </div>


               <div class="form-group col-md-6">
                     <label for="inputAddress">47. Compañia de Transporte <span style="color: #FF0000; font-weight: bold;"> *</span></label>
                             <div class="d-flex">
        <asp:TextBox ID="tcompania" runat="server" MaxLength="100"  class="form-control"    
                         onkeypress="return soloLetras(event,'abcdefghijklmnopqrstuvwxyzñ')"
                          onBlur="cadenareqerida(this,1,200,'valtc');" 
                             data-toggle="tooltip" data-placement="top" title="Nombre de la compañía transportista que realiza la entrega de la unidad de carga en CGSA."
                         ></asp:TextBox>
                       <span class="validacion" id="valtc" ></span>
                  </div>
                   
           
               </div>

               <div class="form-group col-md-3">
                     <label for="inputAddress">48. RUC <span style="color: #FF0000; font-weight: bold;"> *</span></label>
                              <div class="d-flex">
      <asp:TextBox ID="truc" runat="server" MaxLength="15" class="form-control" 
                          onkeypress="return soloLetras(event,'0123456789',true)"
                          
                           onBlur="cadenareqerida(this,1,20,'valruc');"
                             data-toggle="tooltip" data-placement="top" title="Número del Registro Único de Contribuyente de la compañia transportista registrada anteriormente."
                         ></asp:TextBox>
                         <span class="validacion" id="valruc" > </span>
                  </div>  
                   
             
               </div>

           </div>

    </div>



        <div class="form-title barra_colapser barra_cerrar"  data-toggle="collapse" data-target="#colnot" aria-controls="colnot" role="button" aria-expanded="true">Datos para notificación</div>
        <div id="colnot" class="show">
    
    
    <div class="form-row " >
           <div class="form-group col-md-12">
              <label for="inputEmail4">57. Correo(s) para notificación<i><small>(Estos correos se utilizarán para notificar en caso que sea necesaria una inspección)</small></i> <span style="color: #FF0000; font-weight: bold;"> *</span></label>
           
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




     <div  class="form-title barra_colapser barra_cerrar "  data-toggle="collapse" data-target="#colsip"  aria-controls="colsip"  role="button"
         >Datos de la consolidación</div>
   <div  class="show" id="colsip">

   </div>
    
    
    
    <div class="form-row">
          <div class="form-group col-md-12">
                   <label for="inputEmail4">50. Documento de Aduana No <span style="color: #FF0000; font-weight: bold;"> *</span></label>
                   <div class="d-flex">
                        <select id="dpdoc" onchange="adudoc(this,'txtdae1','txtdae2','txtdae3','xxxms');" class="form-control">
                         <option selected="selected"  value="DAE" >DAE</option>
                         <option value='DAS' >DAS</option>
                         <option value='DJT' >DJT</option>
                         <option value='TRS' >TRS</option>
                             <option value='DTAI' >DTAI</option>
                     </select>
                        
                       <label for="inputEmail4">&nbsp;&nbsp;</label>
                        <input id="txtdae1" placeholder="00" type="text" maxlength="3" value='028'  onkeypress="return soloLetras(event,'1234567890',false)"  class="form-control" />
                       <label for="inputEmail4">&nbsp;&nbsp;</label>
                        <input id="txtdae2" placeholder="0000" type="text" maxlength="4"  onkeypress="return soloLetras(event,'1234567890',false)"   class="form-control"/>
                       <label for="inputEmail4">&nbsp;&nbsp;</label>
                         <input id="txtdae3" type="text" maxlength="21" value='40'  onkeypress="return soloLetras(event,'ps1234567890',false)"   class="form-control"
                                    onblur="cadenareqerida(this,8,20,'valadu');" placeholder="Documento" /> 
                       <span class="classic" id='xxxms'></span>
                        <span class="validacion" id="valadu"  ></span>
                       
                  </div> 
          </div> 
    </div>


     <div class="form-row"> 

         <div class="form-group col-md-4">
                 <label for="inputEmail4">51. Cantidad de unidades:<span style="color: #FF0000; font-weight: bold;"> *</span></label>
               <div class="d-flex">
  <input type="text" id="txtbultos" runat="server" class="form-control" maxlength="6" 
             onkeypress="return soloLetras(event,'1234567890')" clientidmode="Static"
               onblur="valrange(this,1,100000,'valcantx',true);"  placeholder="Unidades" />
              <span id="bulcount" class="bulk"></span>
             <span id="valcantx" class="validacion"></span>
                  </div>
             
           
         </div> 
         <div class="form-group col-md-4">
            <label for="inputEmail4">52. Peso total&nbsp;[KG]:<span style="color: #FF0000; font-weight: bold;"> *</span></label>
           
              <div class="d-flex">
    <input type="text" id="pesoc" runat="server"    class="form-control"  clientidmode="Static"
             maxlength="5"
             onblur="valrange(this,1,100000,'valpeso',true);"
             onkeypress="return soloLetras(event,'1234567890')" 
             onkeyup="conver(this,convierte);"
              placeholder="Peso"/>
              <span id="convierte" class=""></span>
             <span id="valpeso" class="validacion"></span>
                  </div>
             
         
         </div> 
         <div class="form-group col-md-4">
            <label for="inputEmail4">53. Tipo de embalaje<span style="color: #FF0000; font-weight: bold;"> *</span></label>
             
              <div class="d-flex">
 <asp:DropDownList ID="dpembala" runat="server"  class="form-control"
                 onchange="valdpme(this,0,vaembala);" EnableViewState="False" 
                 ClientIDMode="Static">
                 <asp:ListItem Selected="True" Value="0">* Tipo de embalaje*</asp:ListItem>
             </asp:DropDownList>
             <span id="vaembala" class="validacion"> * </span>
                  </div>
             
            

         </div> 

</div>

      <div class="form-row">
          <div class="form-group col-md-12 justify-content-center d-flex">
                           <input  type="button" value="Agregar" onclick="DaeCkecker(exito,problema);" class="btn btn-outline-primary mr-4" />
                    <input  type="button" value="Remover" onclick="deleterow();" class="btn btn-outline-primary mr-4"  />

          </div> 
        </div>
        
         <div class="form-row"> 
   <div class="form-group col-md-12">
              <div id="describir" class="accion">
     
                   <div class="informativo" id="colector">
                   <table id="daes" cellpadding="1" cellspacing="0" class="table table-bordered invoice">
                      <thead>
                      <tr>
                        <th>#</th>
                        <th class="documento">Documento</th>
                        <th>Descripción</th>
                        <th class="peso">Peso (Kg)</th>
                      </tr>
                      </thead>
                      <tbody>
                      </tbody>
                   </table>
                  </div>
    
                  <div class=" alert alert-info">
                   <p id="conteo">Total de documentos de exportación ingresados:0</p>
                   </div>

                </div>
         </div> 
         </div> 
    
    
    
 








      <h4 class="mb">Responsabilidad de la información</h4>
          <div class="alert alert-danger">
             Los datos proporcionados son de entera responsabilidad de quien los consigna, por lo que <strong> CONTECON GUAYAQUIL S.A.</strong>  no se responsabiliza por cualquier error o falsedad que los mismos pudieren tener, siendo de cuenta del cliente todos los gastos y perjuicios que por dicho error se ocasionen a la carga.  
         </div>



     <div class="form-row">
            <div class="col-md-12 d-flex justify-content-center">
                    
                      <input id="btsalvar" type="button" value="Generar AISV" onclick="imprimir();" class="btn btn-primary"  />

                 <img alt="loading.." src="../shared/imgs/loading.gif" height="32px" width="32px" id="loader" class="nover"  />
             
            </div>
     </div>


          
 </div>
 
    <script type="text/javascript">
        var ced_count = 0;
        var jAisv = {};
        var xrowcounter = 0;
        var lista = [];

        $(document).ready(function () {
            $('.datetimepickerAlt').datetimepicker().datetimepicker({ lang: 'es', closeOnDateSelect: true, timepicker: false, step: 30, format: 'd/m/Y' });
            //inicia los fecha-hora
            $('.datetimepicker').datetimepicker().datetimepicker({ lang: 'es', step: 30, format: 'd/m/Y H:i' });
            //inicia los fecha
            $('.datepicker').datetimepicker().datetimepicker({ lang: 'es', timepicker: false, format: 'd/m/Y', closeOnDateSelect: true, minDate: '0' });
            //init reefer-> lo pone a false.
            setRefer(false);
            var da = new Date();
            document.getElementById('txtdae2').value = da.getFullYear();
            //controlador de mails
            var counter = 2;
            $("#addButton").click(function () {
                if (counter > 5) {
                    alertify.alert("Solo se permiten 5 mails");
                    return false;
                }
                $('<div/>', { 'id': 'TextBoxDiv' + counter, 'class': 'form-inline' }).html($('<span/>').html('mail #' + counter + ':')).append($('<input type="text" class="form-control"  placeholder="mail@mail.com" >').attr({ 'id': 'textbox' + counter, 'name': 'textbox' + counter, onkeypress: 'return soloLetras(event,"abcdefghijklmnñopqrstuvwxyz1234567890_@.",true);' })).appendTo('#TextBoxesGroup')
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
        
      //Esta funcion va a validar que cuando presionen booking debe poner los 3 parametros
      function validateBook(objeto) {
         /*Recordar vaciar los span*/
          var expnum = document.getElementById('numexpo');
          var linebook = document.getElementById('linea');
          //nuevo para MSC
          if (objeto.bline != null) {
              if (objeto.bline.toLowerCase() === "msc") {
                  document.getElementById('msc_sello').innerHTML = '31. Sello de sticker [Obligatorio] <a class="tooltip" ><span class="classic" >Sello adhesivo que fue otorgado por MSC.</span><img alt="" src="../shared/imgs/info.gif" class="datainfo"/></a>';
              }
              else {
                  document.getElementById('msc_sello').innerHTML = "31. Sello adicional 1 ";
              }
          }
          if (expnum.textContent.length > 0 && linebook.textContent.length > 0 && objeto.numero.length > 0) {
              document.getElementById('xplinea').textContent = '';
              document.getElementById('xpbok').textContent = '';
              document.getElementById('numbook').textContent = objeto.numero;
              document.getElementById('referencia').textContent = objeto.referencia;
              document.getElementById('buque').textContent = objeto.nave;
              document.getElementById('eta').textContent = objeto.eta;
              document.getElementById('cutof').textContent = objeto.cutoff;
              document.getElementById('uis').textContent = objeto.uis; ;
              document.getElementById('agencia').textContent = linebook.textContent;
              document.getElementById('descarga').textContent = objeto.pod;
              document.getElementById('final').textContent = objeto.pod1;
              document.getElementById('producto').textContent = objeto.comoditi;
              document.getElementById('tamano').textContent = objeto.longitud;
              document.getElementById('tipo').textContent = objeto.iso;
              document.getElementById('tara').textContent = objeto.tara;
              document.getElementById('remar').textContent = objeto.remark;
              this.setRefer(objeto.refer);
              this.document.getElementById('refer').checked = objeto.refer;
              this.document.getElementById('imo').checked = objeto.bimo;
              this.jAisv.bnumber = objeto.numero; //numero de booking
              this.jAisv.breferencia = objeto.referencia; //referencia de nave
              this.jAisv.bfk = objeto.fk; // freightkind
              this.jAisv.bnave = objeto.nave; // nombre de nave
              this.jAisv.beta = objeto.eta;//fecha eta
              this.jAisv.bcutOff = objeto.cutoff; //fecha cutoff
              this.jAisv.buis = objeto.uis; //ultimo ingreso sugerido
              this.jAisv.bagencia = linebook.textContent; // nombre de la agencia/linea
              this.jAisv.bpod = objeto.pod; //pto desc1
              this.jAisv.bpod1 = objeto.pod1; //pto desc2
              this.jAisv.bcomodity = objeto.comoditi; // notas del booking
              this.jAisv.bsizeu = objeto.longitud; //longitud de unit booking
              this.jAisv.btipou = objeto.iso; //iso del boking
              this.jAisv.breefer = objeto.refer; //es reefer booking
              this.jAisv.gkey = objeto.gkey;
              this.jAisv.bitem = objeto.item; //id de item de booking
              this.jAisv.breserva = objeto.reserva; // cant reserva
              this.jAisv.busa = objeto.usa; //cant usa
              this.jAisv.bimo = objeto.bimo; //Imo del booking
              this.jAisv.bdispone = objeto.dispone; //dispone booking
              this.jAisv.utara = objeto.tara; //tara de la unidad
              this.jAisv.remark = objeto.remark;
              this.jAisv.utemp = objeto.temp;
              this.jAisv.shipid = objeto.shipid;
              this.jAisv.shipname = objeto.shipname;
              this.jAisv.hzkey = objeto.hzkey;
              this.jAisv.vent_pc = objeto.vent_pc;
              this.jAisv.ventu = objeto.ventu;
              this.jAisv.uhumedad = objeto.hume;
           

              this.document.getElementById('txttemp').value = objeto.temp;
              return true;
          }
          else {
               alertify.alertalert('Por favor use los botones de búsqueda para los 2 parametros').set('label', 'Aceptar');
              return false;
          }
      }
        function imprimir() {
            if (lista == undefined || lista == null || lista.length <= 1 || lista.length > 70) {
                alertify.alert('Debe registrar mas de una DAE en el AISV. Caso contrario debe realizar AISV de contenedor lleno').set('label', 'Aceptar');
                return;
            }
            //uncomment for client validations
            var tpr = this.document.getElementById('<%= dprefservice.ClientID %>').value;
            // if (tpr != null && tpr === '**') {
            //     alert('Por favor seleccione el tipo de refrigeración para inspección (29)');
            //   return;
            //  }

           // alert(tpr);
            var xcedu = document.getElementById('<%=tsCedula.ClientID %>').value;
            var cntr = document.getElementById('<%=txtcontenedor.ClientID %>').value;

             //  alert(this.jAisv);
            if (!cedulaWarning(xcedu)) {
                this.ced_count++;
                if (this.ced_count < 3) {
                    alertify.alert('Asegúrese que el número de cédula de la persona que aplicó los sellos esta correcto\n [ Advertencia ' + this.ced_count + ' de 3 ]').set('label', 'Aceptar');
                    return;
                }
                else {
                    if (confirm('El número de cédula de la persona que aplicó los sellos no parece ser válido esta información será reportada a la Policía Antinarcóticos\n Está seguro que desea continuar?')) {
                        if (setWarningContainer(cntr)) {
                            getPrint(this.jAisv, 'consolidadora.aspx/ValidateJSON');
                            this.ced_count = 0;
                        }
                    }
                }
            }
            else {
                if (setWarningContainer(cntr)) {
                    getPrint(this.jAisv, 'consolidadora.aspx/ValidateJSON');
                    this.ced_count = 0;
                }
            }
        }

      //esta futura funcion va a preparar el objeto a transportar.
        function prepareObject() {
            this.jAisv.secuencia = '0';//numero de aisv
            this.jAisv.idexport = document.getElementById('numexport').value;//id exporter
            this.jAisv.idline = document.getElementById('lineaCI').value; // id line selected
            this.jAisv.tipo = 'EC'; //tipo aisv
            this.jAisv.aidagente = -1 //numero de agente
            this.jAisv.adocnumero = 'DOCCON(' + xrowcounter.toString() + ')';
            this.jAisv.adoctipo = document.getElementById('dpdoc').value; //dae,das,otro
            this.jAisv.unumber = document.getElementById('<%=txtcontenedor.ClientID %>').value;//cntr numero
            this.jAisv.umaxpay = this.document.getElementById('<%= txtpay.ClientID %>').value;//cntr maxmapayload

            this.jAisv.upeso = this.document.getElementById('txtpeso').textContent;//cntr,carga peso

            this.jAisv.uidrefri = this.document.getElementById('<%= dprefrigera.ClientID %>').value; // id refrigeración
            this.jAisv.utemp = this.document.getElementById('<%= txttemp.ClientID %>').value; // cntr temperatura
            this.jAisv.uidventila = this.document.getElementById('<%= dpventila.ClientID %>').value; // cntr id ventilación
            this.jAisv.seal1 = this.document.getElementById('<%= tseal1.ClientID %>').value;//cntr seal1
            this.jAisv.seal2 = this.document.getElementById('<%= tseal2.ClientID %>').value; //cntr seal2
            this.jAisv.seal3 = this.document.getElementById('<%= tseal3.ClientID %>').value; //cntr seal3
            this.jAisv.seal4 = this.document.getElementById('<%= tseal4.ClientID %>').value; //cntr seal4
            this.jAisv.tidubica = this.document.getElementById('<%= dprovincia.ClientID %>').value; //id provincia
            this.jAisv.tidcanton = this.document.getElementById('dcanton').value; //id canton
            this.jAisv.tfechadoc = this.document.getElementById('<%= txtSalida.ClientID %>').value; // fecha hora salida
            this.jAisv.tconductor = this.document.getElementById('txtconductor').textContent; //nombre del chofer
            this.jAisv.tdocument = this.document.getElementById('<%= driID.ClientID %>').value; // id del chofer
            this.jAisv.tplaca = this.document.getElementById('<%= txtplaca.ClientID %>').value; //placa del carro
            this.jAisv.tcabcert = this.document.getElementById('<%= certcabezal.ClientID %>').value; // certificado cabezal
            this.jAisv.tcabcertfecha = this.document.getElementById('txtcerCab').value; //fecha cabezal
            this.jAisv.tchacert = this.document.getElementById('<%= certchasis.ClientID %>').value; //certificado chasis
            this.jAisv.tcabchafecha = this.document.getElementById('txtcerCha').value; // fecha chazis
            this.jAisv.tespcert = this.document.getElementById('<%= certespecial.ClientID %>').value; //certificado especial
            this.jAisv.autor = 'x'; //autor
            this.jAisv.acupo = 'false' //usa cupos
            this.jAisv.aidinst = 0; //id de institución
            this.jAisv.aidrule = 0;  //id de regla
            this.jAisv.bdeck = this.document.getElementById('rbsi').checked ? true : false; //bajo cubierta
            this.jAisv.ubultos = 0; //cs bultos
            this.jAisv.thoras = this.document.getElementById('<%= txthoras.ClientID %>').value;
            this.jAisv.cembalaje = '0'; //cs embajale
            this.jAisv.udepo = this.document.getElementById('<%= dporigen.ClientID %>').value; //id del deposito
            this.jAisv.udepofecha = this.document.getElementById('txtorigen').value; // fecha hora salida
            this.jAisv.nomexpo = this.document.getElementById('nomexpo').textContent;
            this.jAisv.cimo = this.document.getElementById('dpimo').value;
            this.jAisv.direccion = this.document.getElementById('txtdirec').value;
            this.eHas = false;
            this.jAisv.detalles = this.lista;
            this.jAisv.sellor = this.document.getElementById('<%= tsresponsable.ClientID %>').value;
            this.jAisv.selloid = this.document.getElementById('<%= tsCedula.ClientID %>').value;
            this.jAisv.trancia = this.document.getElementById('<%= tcompania.ClientID %>').value;
            this.jAisv.tranruc = this.document.getElementById('<%= truc.ClientID %>').value;
            this.jAisv.diplomatico = this.document.getElementById('ckdip').checked; //usa cupos
            this.jAisv.consignatario = this.document.getElementById('<%= consignee.ClientID %>').value;
            this.jAisv.carga = "M"
            //2019--->Esto es si escoge alguna refrigeracion para inspeccion
            this.jAisv.refservicio = this.document.getElementById('<%= dprefservice.ClientID %>').value;
            this.jAisv.late = this.document.getElementById('cklate').checked; //usa cupos

        }
      function popupCallback(data, control) {
          this.document.getElementById(control).value = data;
      }
      function driverCallback(data) {
          this.document.getElementById('driID').value = data.codigo;
          this.document.getElementById('txtconductor').textContent = data.descripcion;
          this.document.getElementById('txtidentidad').textContent = data.codigo;
      }
    </script>
  <script src="../Scripts/Optional_validations.js" type="text/javascript"></script>
      <script src="../Scripts/pages.js" type="text/javascript"></script>
  <script src="../shared/avisos/popup_simple.js" type="text/javascript"></script>

      <script src="../Scripts/hide.js" type="text/javascript"></script>
</asp:Content>