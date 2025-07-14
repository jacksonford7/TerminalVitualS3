<%@ Page Title="Aisv Consolidar" EnableEventValidation="true" EnableViewState="false"  Language="C#" MasterPageFile="~/site.Master" AutoEventWireup="true" CodeBehind="cargaconsolidar.aspx.cs" Inherits="CSLSite.cargaconsolidar" %>
<%@ MasterType VirtualPath="~/site.Master" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>


<asp:Content ID="Content1" ContentPlaceHolderID="placehead" runat="server">

    <link href="../img/favicon2.png" rel="icon"/>
  <link href="../img/icono.png" rel="apple-touch-icon"/>
  <link href="../css/bootstrap.min.css" rel="stylesheet"/>
  <link href="../css/dashboard.css" rel="stylesheet"/>
  <link href="../css/icons.css" rel="stylesheet"/>
  <link href="../css/style.css" rel="stylesheet"/>
  <link href="https://fonts.googleapis.com/css2?family=Roboto:wght@300;400;500;700&display=swap" rel="stylesheet"/>

    <link href="../shared/avisos/popup_simple.css" rel="stylesheet" />
      <link href="../shared/estilo/jquery.datetimepicker.css" rel="stylesheet" type="text/css" />

     <link href="../shared/estilo/warningBubbles.css" rel="stylesheet" />
    <link href="../shared/avisos/popup_simple.css" rel="stylesheet" />


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="placebody" runat="server">

    
     <div class="mt-4">         
        <nav class="mt-4" aria-label="breadcrumb">
          <ol class="breadcrumb">
            <li class="breadcrumb-item" id="opcion_principal" runat="server"><a href="#">AISV</a></li>
            <li class="breadcrumb-item active" aria-current="page" id="sub_opcion" runat="server">Exportación de Carga para Consolidación</li>
          </ol>
        </nav>
      </div>


 

     <!-- White Container -->
      <div class="dashboard-container p-4" id="cuerpo" runat="server">
             <div class="alert alert-warning">
        <span id="dtlo" runat="server">Estimado usuario:</span> 
        <br /> 
              Para este tipo de AISV, en el campo “Documento de Aduana” debe registrar una <strong>  DAE de carga suelta</strong>.  
              Si la carga ingresará en un contenedor de apoyo, deberá llenar de manera obligatoria el campo de
              “Datos del contenedor”.La transmisión al sistema de Aduana se efectuará confirmando la cantidad de 
               bultos indicados en este formulario. Aceptado el registro no se realizarán modificaciones, ni correcciones. 
    </div>


     <div class="form-title barra_colapser barra_cerrar "  data-toggle="collapse" data-target="#colexpo"  aria-controls="colexpo"  role="button"  aria-expanded="true"   >
            Datos del Booking 
         </div>
     <div class="show" id="colexpo">
          <div class="form-row " >
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
                    <label for="inputAddress">3. Número de Booking<span style="color: #FF0000; font-weight: bold;"> *</span></label>
                    <div class="d-flex">
                         <span id="numbook" class="form-control col-md-11">...</span>
                         <a  class="btn btn-outline-primary mr-4" target="popup" onclick="linkbokin('lineaCI','numexport','LCL');" >
                                <span class='fa fa-search' style='font-size:24px'></span> </a>
                        <span class="validacion"  id="xpbok"></span>
                    </div>
               </div> 

         </div>
      </div>

       <div class="form-title barra_colapser barra_cerrar "  data-toggle="collapse" data-target="#colbook"  aria-controls="colbook"  role="button"  aria-expanded="true"
           >
             Verficación de datos
       </div>
      <div class="show" id="colbook">
      <div class="form-row">
               <div class=" alert alett alert-light">Confirme que los datos ingresados sean correctos.  En caso de error, por favor notifíquelo a las casilla ec.sac@contecon.com.ec o comuníquese a los teléfonos (04) 6006300 – 3901700 opción 4	</div>

                <div class="form-group col-md-4">
                    <label for="inputEmail4">4. Referencia <i>CONTECON</i></label>
                     <span id="referencia" class="form-control">... </span>
                </div> 
                <div class="form-group col-md-4">
                     <label for="inputEmail4">5. Nave</label>
                       <span id="buque" class="form-control">...</span>
                </div>     
          <div class="form-group col-md-4">
                     <label for="inputEmail4">9. A Naviera</label>
                       <span id="agencia" class="form-control">...</span>
                </div> 

          </div>
           <div class="form-row">

                <div class="form-group col-md-4">
                     <label for="inputEmail4">6. Fecha estimada de arribo [ETA]</label>
                      <span id="eta" class="form-control">...</span>
                </div> 
              <div class="form-group col-md-4">
                     <label for="inputEmail4">7. Fecha límite [CutOff]</label>
                       <span id="cutof" class="form-control">...</span>
                </div> 
               <div class="form-group col-md-4">
                     <label for="inputEmail4">8. Último Ingreso sugerido [UIS]</label>
                      <span id="uis" class="form-control">...</span>
                </div> 
          
               </div>
          <div class="form-row">


               <div class="form-group col-md-3">
                     <label for="inputEmail4">10. Puerto de descarga</label>
                     <span id="descarga" class="form-control">...</span>
                </div> 
              <div class="form-group col-md-3">
                     <label for="inputEmail4">11. Puerto de descarga Final</label>
                     <span id="final" class="form-control">...</span>
                </div> 
              <div class="form-group col-md-6">
                     <label for="inputEmail4">12. Producto declarado en Booking</label>
                    <span id="producto" class="form-control">...</span>
                </div> 
              </div>
           <div class="form-row">


              <div class="form-group col-md-6">
                     <label for="inputEmail4">13. Tamaño de contenedor</label>
                      <span id="tamano" class="form-control">NO APLICA EN ESTE DOCUMENTO</span>
                </div> 
              <div class="form-group col-md-6">
                     <label for="inputEmail4">14. Tipo de contenedor</label>
                       <span id="tipo" class="form-control">NO APLICA EN ESTE DOCUMENTO</span>
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
  <input class="form-check-input" type="checkbox"  value="Sobredimensionado" id="over" onclick="radio(this,ckdimen);"/>
  <label class="form-check-label" for="inlineCheckbox1">Sobredimensionado</label>
</div>

                   
<div class="form-check form-check-inline">
  <input onchange="popupWarn(this);" id="cklate" class="form-check-input" type="checkbox"  value="A. Tardio" disabled />
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
  <input class="form-check-input" type="radio" name="deck" id="rbsi" value="SI"  />
  <label class="form-check-label" for="rbsi">
    SI
  </label>
</div>
<div class="form-check-inline">
  <input class="form-check-input" type="radio" name="deck" id="rbno" value="NO" checked/>
  <label class="form-check-label" for="rbno">
    NO
  </label>
</div>
	       </div>
	       </div>
          
          </div>
     
    
    
    
        <div class="form-title barra_colapser barra_cerrar" data-toggle="collapse" data-target="#colcarga" aria-controls="colcarga" role="button" aria-expanded="true"> 
             Datos de la Carga 
          </div>
          <div class="show" id="colcarga">
           <div class="form-row">
             <div class="form-group col-md-12">
                        <label for="inputAddress">18. Nombre de agente afianzado</label>
                         <div class="d-flex">
                                <span id="agente" class="form-control col-md-11">...</span>
                                 <input id="caeid" type="hidden" />
                                <a  class="btn btn-outline-primary mr-4" target="popup" onclick="window.open('../catalogo/agente.aspx','name','width=850,height=880')"  >
                                <span class='fa fa-search' style='font-size:24px'></span> </a>
                             <span class="opcional" id="valexpor" ></span>
                         </div>
               </div> 

            </div>
           <div class="form-row">
                 <div class="form-group col-md-12">
                   <label for="inputEmail4">19. Documento de Aduana No <span style="color: #FF0000; font-weight: bold;"> *</span></label>
                   <div class="d-flex">
                        <select id="dpdoc" onchange="adudoc(this,'txtdae1','txtdae2','txtdae3','xxxms');" class="form-control">
                         <option selected="selected"  value="DAE" >DAE</option>
                         <option value='DAS' >DAS</option>
                         <option value='DJT' >DJT</option>
                         <option value='TRS' >TRS</option>
                             <option value='DTAI' >DTAI</option>
                     </select>
                        
                       <label for="inputEmail4">&nbsp;&nbsp;</label>
                        <input id="txtdae1"  type="text" maxlength="3" value='028'  onkeypress="return soloLetras(event,'1234567890',false)"  class="form-control" />
                       <label for="inputEmail4">&nbsp;&nbsp;</label>
                        <input id="txtdae2"  type="text" maxlength="4"  onkeypress="return soloLetras(event,'1234567890',false)"   class="form-control"/>
                       <label for="inputEmail4">&nbsp;&nbsp;</label>
                         <input id="txtdae3" type="text" maxlength="21" value='40'  onkeypress="return soloLetras(event,'ps1234567890',false)"   class="form-control"
                                    onblur="cadenareqerida(this,8,20,'valadu');"  /> 
                       <span class="classic" id='xxxms'></span>
                        <span class="validacion" id="valadu"  ></span>
                       
                  </div> 
                </div> 
              </div>
           <div class="form-row">
                 <div class="form-group col-md-12">
                     <label for="inputEmail4">20. Mercadería suceptible a cupo</label>
                      <div class="d-flex">
                         
                          <label for="inputPassword4">Indicar si la carga declarada, se controla bajo cupos por ejemplo, banano.</label>
                          <label class="checkbox-container">
                           <input id="ckcupo" type="checkbox" onclick="ckcupos(this);" />   
                          <span class="checkmark"></span>
                          </label>
                          
                          <label for="inputPassword4">&nbsp;&nbsp;</label>
                           <asp:DropDownList ID="dpins" runat="server"   disabled  class="form-control"
                           onchange="invoke(this,'regla',loadrule);" ClientIDMode="Static"
                           ></asp:DropDownList>

                          <label for="inputPassword4">&nbsp;&nbsp;</label>
                           <select id="dprule"  onchange="rulevalidate(this,mrule);"  class="form-control">
                               <option value="0">Regla</option>
                           </select>
                           <span id="mrule" class="validacion" > </span>
                          
                      </div> 
                 </div> 
               </div>
           <div class="form-row">

                   <div class="form-group col-md-12">
                         <label for="inputEmail4">21. Consignatario <span style="color: #FF0000; font-weight: bold;"> *</span></label>
                          <asp:TextBox ID="consignee" runat="server"  class="form-control"
                   onblur="cadenareqerida(this,8,250,'valconsig');"
                  ></asp:TextBox>
                       <span class="validacion" id="valconsig"  ></span>
                 </div> 
              
          </div>
         </div>



    <div   class="form-title barra_colapser barra_cerrar "  data-toggle="collapse" data-target="#colcons"  aria-controls="colcons"  role="button"  aria-expanded="true"
        >Datos de la carga para consolidación</div>
          <div class="show" id="colcons">
<div class="form-row">
                <div class="form-group col-md-6">
                    <label for="inputEmail4">22. Cantidad de unidades: <span style="color: #FF0000; font-weight: bold;"> *</span></label>
                    <div class="d-flex">
     <asp:TextBox ID="txtbultos" runat="server"  class="form-control" MaxLength="6"  
                 onkeypress="return soloLetras(event,'1234567890')"
                   onBlur="valrange(this,1,100000,'valcantx',true);" EnableViewState="False" placeholder="Unidades"
                 ></asp:TextBox>
                       <span id="valcantx" class="validacion"></span>
                  </div>
                    
               
                </div> 
                <div class="form-group col-md-6">
                    <label for="inputEmail4">23. Peso unitario&nbsp;[KG]: <span style="color: #FF0000; font-weight: bold;"> *</span></label>
                     <div class="d-flex">
    <asp:TextBox ID="txtpeso" runat="server"  class="form-control"
                     MaxLength="5"
                     onBlur="valrange(this,1,100000,'valpeso',true);"
                     placeholder="Peso"
                     onkeypress="return soloLetras(event,'1234567890')" EnableViewState="False"   onkeyup="conver(this,convierte);"
                      ></asp:TextBox>
                      <span id="convierte" class=""></span>
                     <span id="valpeso" class="validacion"> * </span>
                  </div>
                    
                
               </div> 

 </div> 

               <div class="form-row"> 
                <div class="form-group col-md-6">
                    <label for="inputEmail4">24. Tipo de embalaje:<span style="color: #FF0000; font-weight: bold;"> *</span></label>
                      <div class="d-flex">
  <asp:DropDownList ID="dpembala" runat="server" class="form-control"
                         onchange="valdpme(this,0,vaembala);" EnableViewState="False">
                         <asp:ListItem Selected="True" Value="0">* Tipo de embalaje*</asp:ListItem>
                     </asp:DropDownList>
                     <span id="vaembala" class="validacion"> </span>
                  </div>
                    
                    
                  
                </div> 
                <div class="form-group col-md-6">
                    <label for="inputEmail4">25. Código de peligrosidad (IMO):<span style="color: #FF0000; font-weight: bold;"></span></label> 
                            
                     <div class="d-flex">
  <asp:DropDownList ID="dpimo" runat="server" class="form-control" EnableViewState="False" >
                         <asp:ListItem Selected="True" Value="0">* Código de peligrosidad *</asp:ListItem>
                     </asp:DropDownList>
                     <span id="valsimo" class="validacion"> </span>
                  </div>
                    
                  
                </div> 
  </div>

          </div>

         <div class="form-title barra_colapser barra_cerrar" data-toggle="collapse" data-target="#colcond" aria-controls="colcond" role="button" aria-expanded="true">
              Datos del Contenedor
           </div>
          <div id="colcond" class="show">
          <div class="form-row">
                             <div class="form-group col-md-4">
                     <label for="inputEmail4">22. Número del contenedor <span style="color: #FF0000; font-weight: bold;"> *</span></label>
                        <div class="d-flex">
   <asp:TextBox ID="txtcontenedor" runat="server"  MaxLength="11"  class="form-control"
                             onkeypress="return soloLetras(event,'abcdefghijklmnñopqrstuvwxyz1234567890')" 
                              onBlur="checkDC(this,'valcont',true);"
                            
                             ></asp:TextBox>
                            <span id="valcont" class="validacion"></span>
                  </div>
                                 
                              
                </div> 

                             <div class="form-group col-md-4">
                     <label for="inputEmail4">23. Tara del contenedor [TON]</label>
                      <span id="tara" class="form-control col-md-12">00.00</span>
              </div> 

                                           <div class="form-group col-md-4">
                     <label for="inputEmail4">24. Max.&nbsp;<i>Payload</i>&nbsp;contenedor [TON] <span style="color: #FF0000; font-weight: bold;"> *</span></label>
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

          </div> 

          <div class="form-row">
               <div class="form-group col-md-6">
                 <label for="inputEmail4">26. Origen del contenedor <i>(Depot)</i> <span style="color: #FF0000; font-weight: bold;"> <span style="color: #FF0000; font-weight: bold;"> *</span></span></label>
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
              </div>

      
          
          
          
          
     <div class="form-title barra_colapser barra_cerrar" data-toggle="collapse" data-target="#colref" aria-controls="colref" role="button" aria-expanded="true">
               Datos sobre refrigeración (OPCIONAL)
           </div>
    <div id="colref" class=" show">
        <div class="form-row">
                <div class="form-group col-md-6">      
                       <label for="inputEmail4">30. Tipo de refrigeración <span style="color: #FF0000; font-weight: bold;"> *</span></label>
                         <div class="d-flex">
  <asp:DropDownList ID="dprefrigera" runat="server" class="form-control" onchange="valdpmeref(this,0,valrefri,tipor,1)"
                        ClientIDMode="Static" >
                         <asp:ListItem Selected="True" Value="0">* Tipo de refrigeración *</asp:ListItem>
                     </asp:DropDownList>
                    <span id='valrefri' class="validacion" ></span>
                  </div>
                </div> 
                <div class="form-group col-md-6">
                     <label for="inputEmail4">31. Temperatura [°C] <span style="color: #FF0000; font-weight: bold;"> </span></label>
                      <div class="d-flex">
 <asp:TextBox ID="txttemp" runat="server"  MaxLength="5" class="form-control"
                       onkeypress="return soloLetras(event,'1234567890.-')" ClientIDMode="Static"
                         onblur="valrange(this,-60,60,'valtemp');" 
                         placeholder="Temperatura °C"   
                          data-toggle="tooltip" data-placement="top" title=" Asegúrese que esta temperatura coincida con la carta de temperatura."
                     >0.0</asp:TextBox>
                    <span id='valtemp' class="validacion" ></span>
                  </div>
                    
                    
                   
                </div> 
       </div>
         <div class="form-row"> 
               <div class="form-group col-md-6">          
                <label for="inputEmail4">32. Humedad &nbsp;[CBM] </label>

                      <div class="d-flex">
        <asp:DropDownList ID="dphumedad" runat="server" class="form-control" ClientIDMode="Static" >
                         <asp:ListItem Selected="True" Value="0">* Humedad *</asp:ListItem>
                  </asp:DropDownList>
                   <span id='valhum' class="validacion"></span>
                  </div>
              
               </div> 
               <div class="form-group col-md-6">          
                <label for="inputEmail4">33. Ventilación&nbsp;[%] </label>

                      <div class="d-flex">
          <asp:DropDownList ID="dpventila" runat="server" class="form-control"  ClientIDMode="Static" >
                         <asp:ListItem Selected="True" Value="0">* Tipo de ventilación *</asp:ListItem>
                        
                  </asp:DropDownList>
                   <span id='valventi' class="validacion" > </span>
                  </div>
          
                </div> 
          </div>
        </div>


  <div class="form-title barra_colapser barra_cerrar"  data-toggle="collapse" data-target="#colsel" aria-controls="colsel" role="button" aria-expanded="true">
              Datos sobre los sellos (OPCIONAL)

          </div>
      <div class="show" id="colsel">
          <div class="form-row">
	          <div class="form-group col-md-3">          
                    <label for="inputEmail4">34. Sello de agencia <span style="color: #FF0000; font-weight: bold;"> *</span></label>
                      <div class="d-flex">
           <asp:TextBox ID="tseal1" runat="server" class="form-control"
                             onkeypress="return soloLetras(event,'abcdefghijklmnñopqrstuvwxyz1234567890',true)" 
                             MaxLength="14" onpaste="return false;" 
                             onblur="cadenareqerida(this,1,20,'valsel1');"
                             ></asp:TextBox>
                      <span id="valsel1" class="validacion" ></span>
                      <span id="msc_sello"></span>
                  </div>
                  
       
                      
                </div> 
                <div class="form-group col-md-3">          
                    <label for="inputEmail4">35. Sello de ventilación [Reefer] </label>
                    
                      <div class="d-flex">
      <asp:TextBox ID="tseal2" runat="server"  class="form-control" ClientIDMode="Static" disabled
                             onkeypress="return soloLetras(event,'abcdefghijklmnñopqrstuvwxyz1234567890',true)" 
                             MaxLength="14"   onpaste="return false;" 
                            
                         ></asp:TextBox>
                         <span id="valselvent" class="opcional" ></span>
                  </div>
                    
              
                </div> 
                <div class="form-group col-md-3">          
                    <label for="inputEmail4">36. Sello adicional 1</label>



                      <asp:TextBox ID="tseal3" runat="server"  class="form-control"
                         onkeypress="return soloLetras(event,'abcdefghijklmnñopqrstuvwxyz1234567890',true)" 
                         MaxLength="14"  onpaste="return false;" 
                         ></asp:TextBox>
                </div> 
                <div class="form-group col-md-3">          
                    <label for="inputEmail4">37. Sello adicional 2</label>
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
          </div>


    <div class="form-title barra_colapser barra_cerrar"  data-toggle="collapse" data-target="#coldim" aria-controls="coldim" role="button" aria-expanded="true">
        Detalle del exceso de dimensiones del contenedor (OPCIONAL)</div>
          
    <div class=" show" id="coldim">
    
    <div class="form-row">
                 <div class="form-group col-md-9">
                     <label for="inputEmail4">40. Excede dimesiones nominales</label>
                       <div class="d-flex">
                        <label for="inputPassword4"> &nbsp;</label>
                            <label class="checkbox-container">
                             <input id="ckdimen" type="checkbox" onclick="radio(this)" />   
                                <span class="classic" id='exdim'>Aplica a contenedores descubiertos (OPEN TOP), Plataformas (FLAT RACK), entre otros.</span>
                             <span class="checkmark"></span>
                            </label>  
                       </div> 
                  </div>

        	        <div class="form-group col-md-3">          
                    <label for="inputEmail4">41. Lateral (IZ)&nbsp; [cm]</label>
                            <div class="d-flex">
      <asp:TextBox ID="txtexizq" runat="server" MaxLength="4" class="form-control xact"
                      onkeypress="return soloLetras(event,'1234567890')" onblur="valrange(this,0,0,'xizq',true);"  
                          disabled >0</asp:TextBox>
                     <span id="xizq" class="xalter validacion" > &nbsp;</span>
                  </div>

                
                 </div> 
                 
          </div>

       <div class="form-row">

                  <div class="form-group col-md-3">          
                    <label for="inputEmail4">42. Lateral (DER)&nbsp; [cm]</label>
                          <div class="d-flex">
 <asp:TextBox ID="txtexder" runat="server" MaxLength="4"  class="form-control xact"
                          onkeypress="return soloLetras(event,'1234567890')"   onBlur="valrange(this,0,0,'xder',true);"  
                              disabled
                             >0</asp:TextBox>
                         <span id="xder"  class="xalter validacion" >  &nbsp;</span>
                  </div>
                      
                     
                 </div> 
                  <div class="form-group col-md-3">          
                    <label for="inputEmail4">43. Frente&nbsp;[cm]</label>
                          <div class="d-flex">
        <asp:TextBox ID="txtexfro" runat="server" MaxLength="4" class="form-control xact" disabled
                          onkeypress="return soloLetras(event,'1234567890')"
                           onBlur="valrange(this,0,0,'xfro',true);"   
                           >0</asp:TextBox>
                           <span id="xfro" class="xalter validacion" > &nbsp;</span>
                  </div>
              
                 </div> 
                 <div class="form-group col-md-3">          
                    <label for="inputEmail4">44. Posterior&nbsp;[cm]:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</label>
                        <div class="d-flex">
   <asp:TextBox ID="txtexatra" runat="server" MaxLength="4" class="form-control xact" disabled
                      onkeypress="return soloLetras(event,'1234567890')"  
                        onBlur="valrange(this,0,0,'xatr',true);" 
                         >0</asp:TextBox>
                     <span id="xatr" class="xalter validacion" > &nbsp;</span>
                  </div>
                     
                  
                 </div> 
                 <div class="form-group col-md-3">          
                    <label for="inputEmail4">45. Superior&nbsp;[cm]:&nbsp;</label>
                         <div class="d-flex">
          <asp:TextBox ID="txtexenc" runat="server" MaxLength="4" class="form-control xact" disabled
                      onkeypress="return soloLetras(event,'1234567890')"  
                       onBlur="valrange(this,0,0,'xsup',true);" 
                        >0</asp:TextBox>
                     <span id="xsup" class="xalter validacion" >  &nbsp;</span>
                  </div>
                     
           
                 </div> 
	       </div>


</div>

           <div class="form-title barra_colapser barra_cerrar"  data-toggle="collapse" data-target="#coltax" aria-controls="coltax" role="button" aria-expanded="true">
               Datos del transporte / origen</div>
          
    <div class="show" id="coltax">
          <div class="form-row"> 

                 <div class="form-group col-md-3">
                   <label for="inputEmail4">46. Provincia (Planta)<span style="color: #FF0000; font-weight: bold;"> *</span></label>
                    <asp:DropDownList ID="dprovincia" runat="server" class="form-control"  onchange="invoke(this,'canton',loadcanton); valdpme(this,0,valprovincia);">
                         <asp:ListItem Selected="True" Value="0">Provincia</asp:ListItem>
                     </asp:DropDownList>

                </div> 
              <div class="form-group col-md-3">
                   <label for="inputEmail4">46. Canton (Planta)<span style="color: #FF0000; font-weight: bold;"> *</span></label>
                    		     <div class="d-flex">
    <select id="dcanton" class="form-control" >
                         <option selected="selected" value="0" >* Cantón *</option>
                     </select>
                     <span class="validacion" id="valprovincia" ></span>
                  </div>
                  
                  
               
               </div> 
              <div class="form-group col-md-3">
                     <label for="inputEmail4">47. Fecha/hora de salida <span style="color: #FF0000; font-weight: bold;"> *</span></label>
                       <div class="d-flex">
             <asp:TextBox ID="txtSalida" runat="server" MaxLength="16"  CssClass="datetimepicker form-control"
                           onkeypress="return soloLetras(event,'1234567890/:')"
                           onBlur="valDate(this,  true,valdatem);"
                         
                           ></asp:TextBox>
                          <span class="validacion" id="valdatem" ></span>   
                  </div>
                  
            
             </div>
               <div class="form-group col-md-3">          
                    <label for="inputEmail4">48. Horas viaje [Estimado] <span style="color: #FF0000; font-weight: bold;"> *</span></label>
                    
                      <div class="d-flex">
          <asp:TextBox ID="txthoras" runat="server" MaxLength="3" class="form-control"
                         onkeypress="return soloLetras(event,'1234567890',true)"
                          onBlur="valrange(this,1,2000,'valestima',true);" 
                         ></asp:TextBox>
                         <span class="validacion" id="valestima" ></span>
                  </div>
         


               </div> 
</div>

         <%--nuevo orden - empresa de transporte--%>
         <div class="form-row"> 
               <div class="form-group col-md-9">
                     <label for="inputAddress">55. Compañia de Transporte <span style="color: #FF0000; font-weight: bold;"> *</span></label>
                    <div class="d-flex">
                    <asp:TextBox ID="tcompania" runat="server" MaxLength="100"  class="form-control"    
                                     onkeypress="return soloLetras(event,'abcdefghijklmnopqrstuvwxyzñ')"
                                      onBlur="cadenareqerida(this,1,200,'valtc');" 
                                         data-toggle="tooltip" data-placement="top" title="Nombre de la compañía transportista que realiza la entrega de la unidad de carga en CGSA." disabled
                                     ></asp:TextBox>
                       <span class="validacion" id="valtc" ></span>
                  </div>
               </div>

               <div class="form-group col-md-3">
                  <label for="inputAddress">56. RUC <span style="color: #FF0000; font-weight: bold;"> *</span></label>
                  <div class="d-flex">
                        <input id="IdRucCia" type="hidden" runat="server" clientidmode="Static" enableviewstate="False" />
                      <asp:TextBox ID="truc" runat="server" MaxLength="15" class="form-control" 
                                          onkeypress="return soloLetras(event,'0123456789',true)" onBlur="cadenareqerida(this,1,20,'valruc');"
                         data-toggle="tooltip" data-placement="top" title="Número del Registro Único de Contribuyente de la compañia transportista registrada anteriormente." disabled></asp:TextBox>
                      <a class="btn btn-outline-primary mr-4" target="popup" onclick="window.open('../catalogo/aisv_empresa_transporte.aspx','name','width=850,height=880')">
                                <span class='fa fa-search' style='font-size: 24px'></span></a>
                       <span class="validacion" id="valruc" > </span>
                  </div>  
             
               </div>
         </div>

         <div class="form-row"> 
              <div class="form-group col-md-9">
                     <label for="inputEmail4">49. Nombre del conductor <span style="color: #FF0000; font-weight: bold;"> *</span></label>
                       <span id="txtconductor" class="form-control"   runat="server" clientidmode="Static"  enableviewstate="False">...</span>
                        <span class="validacion" id="valnombre" ></span>
               </div> 
               <div class="form-group col-md-3">
                     <label for="inputAddress">50. Documento de identidad <span style="color: #FF0000; font-weight: bold;"> *</span></label>
                    <div class="d-flex">
                              <input id="driID" type="hidden" runat="server" clientidmode="Static"  enableviewstate="False" />
                             <span id="txtidentidad" class="form-control"
                             runat="server" clientidmode="Static"  enableviewstate="False">...</span>
                                 <a  class="btn btn-outline-primary mr-4" target="popup"  onclick="BuscarChofer();" >
                                <span class='fa fa-search' style='font-size:24px'></span></a>
                            <span class="validacion" id="valced" ></span>
                        
                    </div>
               </div>
         </div>

          <div class="form-row"> 

               <div class="form-group col-md-2">          
                    <label for="inputEmail4">51. Placa del vehículo <span style="color: #FF0000; font-weight: bold;"> *</span></label>
                    <div class="d-flex">
                       <asp:TextBox ID="txtplaca" runat="server" MaxLength="10"  class="form-control"
                                            onkeypress="return soloLetras(event,'abcdefghijklmnopqrstuvwxyz0123456789')" 
                                             onBlur="validarPlaca(this,'valplaca');buble_warning();"  ClientIDMode="Static"
                                              ></asp:TextBox>
                                             <span class="validacion" id="valplaca" ></span>
                    </div>
               </div> 

               <div class="form-group col-md-4">
                     <label for="inputAddress">52. Certificado de Cabezal</label>
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

               <div class="form-group col-md-4">
                     <label for="inputAddress">53. Certificado de Chasis</label>
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

               <div class="form-group col-md-2">
                     <label for="inputAddress">54. Certificado especial</label>
                       <asp:TextBox ID="certespecial" runat="server" MaxLength="8" class="form-control"    
                           onkeypress="return soloLetras(event,'abcdefghijklmnopqrstuvwxyz0123456789',true)"
                          ></asp:TextBox>
               </div>

          </div>

       
 
    </div>


 <div class="form-title barra_colapser barra_cerrar"  data-toggle="collapse" data-target="#colnot" aria-controls="colnot" role="button" aria-expanded="true">Datos para notificación</div>
        <div id="colnot" class="show">
    
    
    <div class="form-row " >
         <div class="form-group col-md-12">
                     <label for="inputEmail4">57. Teléfono/Correo(s) para notificación<i><small>(Estos correos se utilizarán para alertas y notificaciones.)</small></i> <span style="color: #FF0000; font-weight: bold;"> *</span></label>
                </div>
                 <div class="form-group col-md-2">
                     <label for="inputEmail4">Número de Celular<span style="color: #FF0000; font-weight: bold;"> *</span></label>
                      <asp:TextBox ID="TxtCelular" runat="server"  MaxLength="10"  class="form-control"
                             onkeypress="return soloLetras(event,'1234567890')" 
                             
                             ></asp:TextBox>
                           
                </div> 

           <div class="form-group col-md-12">
            
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

        

       <h4 class="mb">Responsabilidad de la información</h4>
          <div class="alert alert-danger">
             Los datos proporcionados son de entera responsabilidad de quien los consigna, por lo que <strong> CONTECON GUAYAQUIL S.A.</strong>  no se responsabiliza por cualquier error o falsedad que los mismos pudieren tener, siendo de cuenta del cliente todos los gastos y perjuicios que por dicho error se ocasionen a la carga.  
         </div>

           <div class="row">
            <div class="col-md-12 d-flex justify-content-center">
                     <img alt="loading.." src="../shared/imgs/loading.gif" height="32px" width="32px" id="loader" class="nover"  />
                     <input id="btsalvar" type="button" value="Generar AISV" onclick="imprimir();"  class="btn btn-primary" /> &nbsp;
                     <input id="btclear"   type="reset"    value="Borrar formulario" 
                          onclick="fullreset(this.form,true);" class="btn btn-outline-primary mr-4"   />
            </div>
          </div>
    

      
      </div>

         <script src="../Scripts/pages.js" type="text/javascript"></script>
       	<script src="../Scripts/Optional_validations.js" type="text/javascript"></script>
       <script src="../shared/avisos/popup_simple.js" type="text/javascript"></script>
     <script src="../Scripts/jquery.datetimepicker.js" type="text/javascript"></script>
   
    <script type="text/javascript">
        var jAisv = {};
            //objeto a transportar.
        $(document).ready(function () {
            $('.datetimepickerAlt').datetimepicker().datetimepicker({ lang: 'es', closeOnDateSelect: true, timepicker: false, step: 30, format: 'd/m/Y' });
            //init pickers
            $('.datetimepicker').datetimepicker().datetimepicker({ lang: 'es', step: 30, format: 'd/m/Y H:i' });
            //inicia los fecha
            $('.datepicker').datetimepicker().datetimepicker({ lang: 'es', timepicker: false, format: 'd/m/Y', closeOnDateSelect: true, minDate: '0' });
            //init excesos
            $('.xalter').text('');
            //poner valor en campo
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
   

        function xclic() { $('#opcional').click();}
        //Esta funcion va a validar que cuando presionen booking debe poner los 3 parametros
        function validateBook(objeto) {
            /*Recordar vaciar los span*/
            var expnum = document.getElementById('numexpo');
            var linebook = document.getElementById('linea');
            if (expnum.textContent.length > 0 && linebook.textContent.length > 0 && objeto.numero.length > 0) {
                document.getElementById('xplinea').textContent = '';
                document.getElementById('xpbok').textContent = '';
                document.getElementById('numbook').textContent = objeto.numero;
                document.getElementById('referencia').textContent = objeto.referencia;
                document.getElementById('buque').textContent = objeto.nave;
                document.getElementById('eta').textContent = objeto.eta;
                document.getElementById('cutof').textContent = objeto.cutoff;
                document.getElementById('uis').textContent = objeto.uis;;
                document.getElementById('agencia').textContent = linebook.textContent;
                document.getElementById('descarga').textContent = objeto.pod;
                document.getElementById('final').textContent = objeto.pod1;
                document.getElementById('producto').textContent = objeto.comoditi;
                document.getElementById('tamano').textContent = objeto.longitud;
                document.getElementById('tipo').textContent = objeto.iso;
                document.getElementById('tara').textContent = objeto.tara;
                document.getElementById('remar').textContent = objeto.remark;
                this.document.getElementById('refer').checked = objeto.refer;
                this.document.getElementById('imo').checked = objeto.bimo;
                this.jAisv.bnumber = objeto.numero; //numero de booking
                this.jAisv.breferencia = objeto.referencia; //referencia de nave
                this.jAisv.bfk = objeto.fk; // freightkind
                this.jAisv.bnave = objeto.nave; // nombre de nave
                this.jAisv.beta = objeto.eta; //fecha eta
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
                this.jAisv.cimo = objeto.imo; // este imo es por cada unidad, debe reemplar cs
                this.jAisv.remark = objeto.remark;
                this.jAisv.utemp = objeto.temp;
                this.jAisv.shipid = objeto.shipid;
                this.jAisv.shipname = objeto.shipname;
                this.jAisv.hzkey = objeto.hzkey;
                this.jAisv.vent_pc = objeto.vent_pc;
                this.jAisv.ventu = objeto.ventu;
                this.jAisv.uhumedad = objeto.hume;
                this.document.getElementById('txttemp').value = objeto.temp;

                  //celular     
                this.document.getElementById('TxtCelular').value = objeto.celular;

                return true;
            }
            else {
                alertify.alert('Por favor use los botones de búsqueda para los 3 parametros').set('label', 'Aceptar');
                return false;
            }
        }
        function imprimir() {
            getPrint(this.jAisv, 'cargaconsolidar.aspx/ValidateJSON');
        }
        //esta futura funcion va a preparar el objeto a transportar.
        function prepareObject() {
            this.jAisv.secuencia = '0'; //asiv secuencia
            this.jAisv.idexport = document.getElementById('numexport').value; //id export
            this.jAisv.idline = document.getElementById('lineaCI').value; // linea
            this.jAisv.tipo = 'CS'; //tipo aisv
            this.jAisv.aidagente = document.getElementById('caeid').value; //id agente
            this.jAisv.adocnumero = document.getElementById('txtdae1').value + document.getElementById('txtdae2').value + document.getElementById('txtdae3').value
            this.jAisv.adoctipo = document.getElementById('dpdoc').value; //tipo dae,djt etc..
            this.jAisv.unumber = document.getElementById('<%=txtcontenedor.ClientID %>').value; //cntr number
            this.jAisv.umaxpay = this.document.getElementById('<%= txtpay.ClientID %>').value; //cntr maxpayload
            this.jAisv.upeso = this.document.getElementById('<%= txtpeso.ClientID %>').value; //cntr,cargas peso
            this.jAisv.uidrefri = this.document.getElementById('<%= dprefrigera.ClientID %>').value; //cntr refrigeración
            this.jAisv.utemp = this.document.getElementById('txttemp').value;
           // this.jAisv.uhumedad = this.document.getElementById('<%= dphumedad.ClientID %>').value;
            this.jAisv.uidventila = this.document.getElementById('<%= dpventila.ClientID %>').value;
            this.jAisv.seal1 = this.document.getElementById('<%= tseal1.ClientID %>').value;
            this.jAisv.seal2 = this.document.getElementById('<%= tseal2.ClientID %>').value;
            this.jAisv.seal3 = this.document.getElementById('<%= tseal3.ClientID %>').value;
            this.jAisv.seal4 = this.document.getElementById('<%= tseal4.ClientID %>').value;
            this.jAisv.eleft = this.document.getElementById('<%= txtexizq.ClientID %>').value;
            this.jAisv.eright = this.document.getElementById('<%= txtexder.ClientID %>').value;
            this.jAisv.efront = this.document.getElementById('<%= txtexfro.ClientID %>').value;
            this.jAisv.etop = this.document.getElementById('<%= txtexenc.ClientID %>').value;
            this.jAisv.eback = this.document.getElementById('<%= txtexatra.ClientID %>').value;
            this.jAisv.tidubica = this.document.getElementById('<%= dprovincia.ClientID %>').value;
            this.jAisv.tidcanton = this.document.getElementById('dcanton').value;
            this.jAisv.tfechadoc = this.document.getElementById('<%= txtSalida.ClientID %>').value;
            this.jAisv.tviaje = this.document.getElementById('<%= txthoras.ClientID %>').value;
            this.jAisv.tconductor = this.document.getElementById('txtconductor').textContent; //nombre del chofer
            this.jAisv.tdocument = this.document.getElementById('<%= driID.ClientID %>').value; // id del chofer
            this.jAisv.tplaca = this.document.getElementById('<%= txtplaca.ClientID %>').value;
            this.jAisv.tcabcert = this.document.getElementById('<%= certcabezal.ClientID %>').value;
            this.jAisv.tcabcertfecha = this.document.getElementById('txtcerCab').value;
            this.jAisv.tchacert = this.document.getElementById('<%= certchasis.ClientID %>').value;
            this.jAisv.tcabchafecha = this.document.getElementById('txtcerCha').value;
            this.jAisv.tespcert = this.document.getElementById('<%= certespecial.ClientID %>').value;
            this.jAisv.autor = 'token';
            this.jAisv.acupo = this.document.getElementById('ckcupo').checked;
            this.jAisv.aidinst = this.document.getElementById('<%= dpins.ClientID %>').value;
            this.jAisv.aidrule = this.document.getElementById('dprule').value;
            this.jAisv.bdeck = this.document.getElementById('rbsi').checked ? true : false; //bajo cubierta
            this.jAisv.ubultos = this.document.getElementById('<%= txtbultos.ClientID %>').value;
            this.jAisv.thoras = this.document.getElementById('<%= txthoras.ClientID %>').value;
            //imo del combo
            this.jAisv.cimo = this.document.getElementById('<%= dpimo.ClientID %>').value;
            //embalaje combo
            this.jAisv.cembalaje = this.document.getElementById('<%= dpembala.ClientID %>').value;
            this.jAisv.udepo = this.document.getElementById('<%= dporigen.ClientID %>').value; //id del deposito
            this.jAisv.udepofecha = this.document.getElementById('txtorigen').value; // fecha hora salida
            this.jAisv.nomexpo = this.document.getElementById('nomexpo').textContent;
            //todos
            this.jAisv.trancia = this.document.getElementById('<%= tcompania.ClientID %>').value;
            this.jAisv.tranruc = this.document.getElementById('<%= truc.ClientID %>').value;
            //2017->Consignatario-diplomatico
            this.jAisv.diplomatico = this.document.getElementById('ckdip').checked; //usa cupos
            this.jAisv.consignatario = this.document.getElementById('<%= consignee.ClientID %>').value;
            this.jAisv.carga = "X";
              //celular     
            this.jAisv.celular = this.document.getElementById('<%= TxtCelular.ClientID %>').value;

        }
        function popupCallback(data, control) {
            this.document.getElementById(control).value = data;
        }
        function driverCallback(data) {
            this.document.getElementById('driID').value = data.codigo;
            this.document.getElementById('txtconductor').textContent = data.descripcion;
            this.document.getElementById('txtidentidad').textContent = data.codigo;
        }


        function ciaCallback(data)
        {
            this.document.getElementById('IdRucCia').value = data.codigo;
            this.document.getElementById('<%= tcompania.ClientID %>').value = data.descripcion;
             this.document.getElementById('<%= truc.ClientID %>').value = data.codigo;
           
        }

        function BuscarChofer()
        {

            var id_empresa = document.getElementById('<%=truc.ClientID%>').value;
            if (id_empresa == '' || id_empresa == null || id_empresa == undefined)
            {


                alertify.alert('Advertencia', 'Debe seleccionar una empresa de transporte').set('label', 'Aceptar');
                return false;
            }

            window.open('../catalogo/aisv_chofer.aspx?IdRucCia=' + id_empresa, 'name', 'width=850,height=880');

        }

    </script>

<script type="text/javascript">

        $(function () {
            $('[id*=txtplaca]').typeahead({
                hint: true,
                highlight: true,
                minLength: 3,
                source: function (request, response) {

                    $.ajax({
                        url: '<%=ResolveUrl("cargaconsolidar.aspx/GetPlaca") %>',
                    data: "{ 'prefix': '" + request + "', 'idempresa' : '" + $("#IdRucCia").val() + "' }",
                    dataType: "json",
                    type: "POST",
                    maxJsonLength: 1,
                    contentType: "application/json; charset=utf-8",
                    success: function (data) {
                        items = [];
                        map = {};
                        $.each(data.d, function (i, item) {
                            var id = item.split('+')[0];
                            var name = item.split('+')[1];
                            map[name] = { id: id, name: name };
                            items.push(name);
                        });
                        response(items);
                        $(".dropdown-menu").css("height", "auto");
                    },
                    error: function (response) {
                        alert(response.responseText);
                    },
                    failure: function (response) {
                        alert(response.responseText);
                    }
                });
            },
            updater: function (item) {
                $('[id*=IdTxtPlaca]').val(map[item].id);
                return item;
            }
        });
    });

</script>

    <script src="../Scripts/hide.js" type="text/javascript"></script>
    <script type="text/javascript" src='../lib/autocompletar/bootstrap3-typeahead.min.js'></script>



</asp:Content>

