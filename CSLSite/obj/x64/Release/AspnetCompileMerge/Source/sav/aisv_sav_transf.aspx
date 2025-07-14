<%@ Page Title="Transferencia de Turnos" Language="C#" MasterPageFile="~/site.Master" AutoEventWireup="true" CodeBehind="aisv_sav_transf.aspx.cs" Inherits="CSLSite.aisv_sav_transf" %>
<%@ MasterType VirtualPath="~/site.Master" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="placehead" runat="server">
    <link href="../shared/estilo/tables.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/aisv-general.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/work.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/catalogos.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/info.css" rel="stylesheet" type="text/css" />

    <link href="../img/favicon2.png" rel="icon"/>
    <link href="../img/icono.png" rel="apple-touch-icon"/>
    <link href="../css/bootstrap.min.css" rel="stylesheet"/>
    <link href="../css/dashboard.css" rel="stylesheet"/>
    <link href="../css/icons.css" rel="stylesheet"/>
    <link href="../css/style.css" rel="stylesheet"/>
    <link href="https://fonts.googleapis.com/css2?family=Roboto:wght@300;400;500;700&display=swap" rel="stylesheet"/>

    <link href="../shared/estilo/Reset.css" rel="stylesheet" />
    <link href="../shared/estilo/jquery.datetimepicker.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/tables.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/info.css" rel="stylesheet" type="text/css" />

     <link href="../shared/estilo/proforma.css" rel="stylesheet" type="text/css" />
     
      <style type="text/css">
    .cal_Theme1 .ajax__calendar_container   {
    background-color: #DEF1F4;
    border:solid 1px #77D5F7;
    }

    .cal_Theme1 .ajax__calendar_header  {
background-color: #ffffff;
margin-bottom: 4px;
}

.cal_Theme1 .ajax__calendar_title,
.cal_Theme1 .ajax__calendar_next,
.cal_Theme1 .ajax__calendar_prev    {
color: #004080;
padding-top: 3px;
}

.cal_Theme1 .ajax__calendar_body    {
background-color: #ffffff;
border: solid 1px #77D5F7;
}

.cal_Theme1 .ajax__calendar_dayname {
text-align:center;
font-weight:bold;
margin-bottom: 4px;
margin-top: 2px;
color: #004080;
}

.cal_Theme1 .ajax__calendar_day {
color: #004080;
text-align:center;
}

.cal_Theme1 .ajax__calendar_hover .ajax__calendar_day,
.cal_Theme1 .ajax__calendar_hover .ajax__calendar_month,
.cal_Theme1 .ajax__calendar_hover .ajax__calendar_year,
.cal_Theme1 .ajax__calendar_active  {
color: #004080;
font-weight: bold;
background-color: #DEF1F4;
}


.cal_Theme1 .ajax__calendar_today   {
font-weight:bold;
}

.cal_Theme1 .ajax__calendar_other,
.cal_Theme1 .ajax__calendar_hover .ajax__calendar_today,
.cal_Theme1 .ajax__calendar_hover .ajax__calendar_title {
color: #bbbbbb;
}
        .warning { background-color:Yellow;  color:Red;}

  #progressBackgroundFilter {
    position:fixed;
    bottom:0px;
    right:0px;
    overflow:hidden;
    z-index:1000;
    top: 0;
    left: 0;
    background-color: #CCC;
    opacity: 0.8;
    filter: alpha(opacity=80);
    text-align:center;
}
#processMessage 
{
    text-align:center;
    position:fixed;
    top:30%;
    left:43%;
    z-index:1001;
    border: 5px solid #67CFF5;
    width: 200px;
    height: 100px;
    background-color: White;
    padding:0;
}
* input[type=text]
    {
        text-align:left!important;
        }
        
        .autocomplete_completionListElement
        {
            margin: 0px !important;
            background-color: inherit;
            color: windowtext;
            border: buttonshadow;
            border-width: 1px;
            border-style: solid;
            cursor: 'default';
            overflow: auto;
            height: auto;
            text-align: left;
            list-style-type: none;
        }
        
        /* AutoComplete highlighted item */
        .autocomplete_highlightedListItem
        {
            background-color: #ffff99;
            color: black;
            padding: 1px;
        }
        
        /* AutoComplete item */
        .autocomplete_listItem
        {
            background-color: window;
            color: windowtext;
            padding: 1px;
             z-index:2000 !important;
        }
        .AutoExtender
        {
            font-family: Verdana, Helvetica, sans-serif;
            font-size: xx-small;
            font-weight: normal;
            border: solid 1px #006699;
            line-height: 20px;
            padding: 10px;
            background-color: White;
            margin-left: 10px;
            z-index: 200000 !important;
        }
        .AutoExtenderList
        {
            border-bottom: dotted 1px #006699;
            cursor: pointer;
            /*color: Maroon;*/
            color:Black;
            font-style: normal;
            font-size: xx-small;
            font-family:Arial!important;
            z-index: 200000 !important;
        }
        .AutoExtenderHighlight
        {
            color: White;
            background-color: #006699;
            cursor: pointer;
            z-index: 200000 !important;
        }
    </style>  
     
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="placebody" runat="server">
              <input id="zonaid" type="hidden" value="105" />

    <input id="dv_nombre"     type="hidden"   runat="server" clientidmode="Static"  />
    <input id="dv_licencia"   type="hidden"   runat="server" clientidmode="Static"  />
    <input id="bandera"     type="hidden"   runat="server" clientidmode="Static"  />
    <asp:ToolkitScriptManager ID="tkcal" EnableScriptGlobalization="true" runat="server"> </asp:ToolkitScriptManager>
             

    <div class="mt-4">         
        <nav class="mt-4" aria-label="breadcrumb">
            <ol class="breadcrumb">
            <li class="breadcrumb-item" id="opcion_principal" runat="server"><a href="#">Servicio de Administración de Vacíos</a></li>
                <li class="breadcrumb-item active" aria-current="page" id="sub_opcion" runat="server">Transferencia de turnos para recepción</li>
            </ol>
        </nav>
    </div>

    <div class="dashboard-container p-4" id="cuerpo" runat="server">

        <div class="form-title">
            DATOS PARA EL PROCESAMIENTO
        </div>

        <div >
            <div class="form-row" >

                <div class="form-group   col-md-6"> 
                    <label for="inputAddress">1: Seleccione Depósito:<span style="color: #FF0000; font-weight: bold;"></span></label>

                    <div class="d-flex">
                        <asp:DropDownList ClientIDMode="Static" ID="cmbDeposito" class="form-control" runat="server" onchange="valdpme(this,0,valcbo);"></asp:DropDownList>
                    </div>
                </div>

                <div class="form-group   col-md-6"> 
                    <label for="inputAddress">2: Operador o Línea:<span style="color: #FF0000; font-weight: bold;"></span></label>

                    <div class="d-flex">
                        <asp:DropDownList ClientIDMode="Static" ID="dplinea" class="form-control" runat="server" onchange="valdpme(this,0,valopera);">
                            <asp:ListItem Value="0" Selected="True">Selecione Operador</asp:ListItem>
                            <asp:ListItem Value="MSK">(MSK) MOLLER MAERSK</asp:ListItem>
                            <asp:ListItem Value="HSD">(HSD) HAMBURG SUDAMERIKAN</asp:ListItem>
                            <asp:ListItem Value="SEA">(SEA) MAERSK SEALAND</asp:ListItem>
                        </asp:DropDownList>
                        <span id="valopera" class="validacion"> * </span>
                    </div>
                </div>

                <div class="form-group   col-md-6"> 
                    <label for="inputAddress">3: Numero de contenedor:<span style="color: #FF0000; font-weight: bold;"></span></label>

                    <div class="d-flex">
                        <asp:TextBox ID="txtcontenedor" runat="server" MaxLength="11" 
                            onkeypress="return soloLetras(event,'abcdefghijklmnñopqrstuvwxyz1234567890')" 
                            onBlur="checkDC(this,'valcont',true);"
                            placeholder="Contenedor"
                            class="form-control"
                            ClientIDMode="Static">
                        </asp:TextBox>
                        <span id="valcont" class="validacion"> * </span>
                    </div>
                </div>

                

                <div class="form-group   col-md-6"> 
                    <label for="inputAddress">4: Seleccione Fecha de Ingreso:<span style="color: #FF0000; font-weight: bold;"></span></label>

                    <div class="d-flex">
                        <asp:TextBox class="form-control" runat="server" ID="fecsalida" AutoPostBack="false" MaxLength="10"  onkeypress="return soloLetras(event,'0123456789/')" ></asp:TextBox>

                        <asp:CalendarExtender ID="CAGTFECHAFASA"  runat="server"
                            CssClass="cal_Theme1" Format="dd/MM/yyyy" TargetControlID="fecsalida">
                        </asp:CalendarExtender>      

                        <asp:Button class="btn btn-primary" Text="Consultar Horarios"  OnClientClick="return fchange()" OnClick="btnconsultarturnos_Click" runat="server" id="btnconsultarturnos"/>
                        <img alt="loading.." src="../shared/imgs/loader.gif" id="imgfecha" class="nover"  />
                        <span class="validacion" id="fecsalida2" > * </span>
                    </div>
                </div>

                <div class="form-group   col-md-6"> 
                    <label for="inputAddress">5: Seleccione Horario:<span style="color: #FF0000; font-weight: bold;"></span></label>

                    <div class="d-flex">
                        <asp:DropDownList ClientIDMode="Static" ID="dpturno"  class="form-control" runat="server" onchange="valdpme(this,0,valcbo);" >
                            <%--<asp:ListItem Value="0" Selected="True">Selecione horario</asp:ListItem>
                            <asp:ListItem Value="19:00">07:00-19:00</asp:ListItem>
                            <asp:ListItem Value="07:00">19:00-07:00</asp:ListItem>--%>
                        </asp:DropDownList>

                        <span id="valcbo" class="validacion"> * </span>
                    </div>
                </div>

                <div class="form-group   col-md-6"> 
                    <label for="inputAddress">6: Tamaño de contenedor:<span style="color: #FF0000; font-weight: bold;"></span></label>

                    <div class="d-flex">
                        <asp:DropDownList ClientIDMode="Static" ID="dptamano"  class="form-control" runat="server" onchange="valdpme(this,0,valsize);"  >
                            <asp:ListItem Value="0" Selected="True">Selecione Tamaño</asp:ListItem>
                            <asp:ListItem Value="20">20 ST</asp:ListItem>
                            <asp:ListItem Value="40">40 ST</asp:ListItem>
                            <asp:ListItem Value="45">40 HC</asp:ListItem>
                        </asp:DropDownList>
                        <span id="valsize" class="validacion"> * </span>
                    </div>
                </div>

                <div class="form-group   col-md-6"> 
                    <label for="inputAddress">7. Nombre del conductor:<span style="color: #FF0000; font-weight: bold;"></span></label>

                    <div class="d-flex">
                        <span id="txtconductor"  class="form-control" runat="server" clientidmode="Static"  enableviewstate="False">...</span>
                        <span class="validacion" id="valnombre" ></span>
                    </div>
                </div>

                <div class="form-group   col-md-6"> 
                    <label for="inputAddress">8. Documento de identidad:<span style="color: #FF0000; font-weight: bold;"></span></label>

                    <div class="d-flex">
                        <input id="driID" type="hidden" runat="server" clientidmode="Static"  enableviewstate="False" />
                        <span id="txtidentidad"  class="form-control" runat="server" clientidmode="Static"  enableviewstate="False">...</span>
                        <span class="validacion" id="valced" > * </span>
                        <a  class="btn btn-outline-primary mr-4" target="popup" onclick="window.open('../catalogo/chofer.aspx','name','width=850,height=880')"> <span class='fa fa-search' style='font-size:24px'></span>  </a>
                    </div>
                </div>

                <div class="form-group   col-md-6"> 
                    <label for="inputAddress">9. Placa del vehículo:<span style="color: #FF0000; font-weight: bold;"></span></label>

                    <div class="d-flex">
                        <asp:TextBox ID="txtplaca" runat="server" MaxLength="10"  class="form-control" onkeypress="return soloLetras(event,'abcdefghijklmnopqrstuvwxyz0123456789')" onBlur="validarPlaca(this,'valplaca');buble_warning();" placeholder="AAA0000" ClientIDMode="Static"></asp:TextBox>
                        <span class="popuptext" id="verPop"></span>
                        <span class="validacion" id="valplaca" > * </span>
                    </div>
                </div>

                <div class="form-group   col-md-6"> 
                    <label for="inputAddress">10. Mail notificación adicional<span style="color: #FF0000; font-weight: bold;"></span></label>

                    <div class="d-flex">
                        <input  class="form-control" type='text' id='txtMailAdicional' name='textbox1'  runat="server" style= ' margin:0'
                            enableviewstate="false" clientidmode="Static"
                            onkeypress="return soloLetras(event,'abcdefghijklmnñopqrstuvwxyz1234567890_$@.',true)"  
                            onblur="maildata(this,'valmailz');" maxlength="50"/>
                        <span id="valmailz" class="validacion"></span>
                        <span class="opcional"> * </span>

                    </div>
                </div>

                <div class="form-group   col-md-6"> 
                    <label for="inputAddress">11: Numero de DOCUMENTO:<span style="color: #FF0000; font-weight: bold;"></span></label>

                    <div class="d-flex">
                        <asp:TextBox ID="txtDAE" runat="server" MaxLength="50"  class="form-control"
                            onkeypress="return soloLetras(event,'abcdefghijklmnñopqrstuvwxyz1234567890')" 
                            onBlur=""
                            placeholder="No DOCUMENTO"
                                ClientIDMode="Static">
                        </asp:TextBox>
                        <span class="opcional"> * </span>
                    </div>
                </div>

            </div>

            <%--<table class="xcontroles" cellspacing="0" cellpadding="1">
        <tr><th  colspan="4"> Datos PARA EL 
            PROCESAMIENTO

        <tr>
	        <td class="bt-bottom  bt-right bt-left">1: Seleccione Depósito:</td>
	        <td class="bt-bottom"  colspan="2">
	            <asp:DropDownList ClientIDMode="Static" ID="cmbDeposito" Width="200px" runat="server" onchange="valdpme(this,0,valcbo);"></asp:DropDownList>
	        </td>
        </tr>

       <tr>
         <td class="bt-bottom  bt-right bt-left">2: Numero de contenedor:</td>
          <td class="bt-bottom " >
              <asp:TextBox ID="txtcontenedor" runat="server" Width="200px" MaxLength="11" CssClass="mayusc"
             onkeypress="return soloLetras(event,'abcdefghijklmnñopqrstuvwxyz1234567890')" 
              onBlur="checkDC(this,'valcont',true);"
              placeholder="Contenedor"
                  ClientIDMode="Static"
             ></asp:TextBox>
              </td>
            <td class="bt-bottom">
                &nbsp;</td>
             <td class="bt-bottom  bt-right "><span id="valcont" class="validacion"> * obligatorio</span></td>
         </tr>

         <tr>
            <td class="bt-bottom  bt-right bt-left">3: Seleccione Fecha de Ingreso:</td>
            <td class="bt-bottom bt-top  "  colspan="2">
                <asp:TextBox runat="server" ID="fecsalida" AutoPostBack="false" MaxLength="10"  onkeypress="return soloLetras(event,'0123456789/')" Width="80px"></asp:TextBox>

                    <asp:CalendarExtender ID="CAGTFECHAFASA"  runat="server"
                        CssClass="cal_Theme1" Format="dd/MM/yyyy" TargetControlID="fecsalida">
                    </asp:CalendarExtender>      

                <asp:Button Text="Consultar Turnos"  OnClientClick="return fchange()" OnClick="btnconsultarturnos_Click" runat="server" id="btnconsultarturnos"/>
                <img alt="loading.." src="../shared/imgs/loader.gif" id="imgfecha" class="nover"  />
                
            </td>
            <td class="bt-bottom bt-right validacion "><span class="validacion" id="fecsalida2" > * obligatorio</span></td>
        </tr>

          <tr>
         <td class="bt-bottom  bt-right bt-left bt-top">4: Seleccione Turno:</td>
          <td class="bt-bottom bt-top  "  colspan="2">
             
              <asp:DropDownList ClientIDMode="Static" ID="dpturno" Width="200px" runat="server" onchange="valdpme(this,0,valcbo);" >
            
              </asp:DropDownList>
           </td>
             <td class="bt-bottom  bt-right bt-top"><span id="valcbo" class="validacion"> * obligatorio</span></td>
         </tr>
                                 <tr>
         <td class="bt-bottom  bt-right bt-left ">5: Tamaño de contenedor:</td>
          <td class="bt-bottom   "  colspan="2">
             
              <asp:DropDownList ClientIDMode="Static" ID="dptamano" Width="200px" runat="server" onchange="valdpme(this,0,valsize);"  >
                  <asp:ListItem Value="0" Selected="True">Selecione Tamaño</asp:ListItem>
                  <asp:ListItem Value="20">20 ST</asp:ListItem>
                  <asp:ListItem Value="40">40 ST</asp:ListItem>
                  <asp:ListItem Value="45">40 HC</asp:ListItem>
              </asp:DropDownList>
           </td>
             <td class="bt-bottom  bt-right "><span id="valsize" class="validacion"> * obligatorio</span></td>
         </tr>

                          <tr>
         <td class="bt-bottom  bt-right bt-left">6: Operador o Línea:</td>
          <td class="bt-bottom   "  colspan="2">
             
              <asp:DropDownList ClientIDMode="Static" ID="dplinea" Width="200px" runat="server" onchange="valdpme(this,0,valopera);">
                  <asp:ListItem Value="0" Selected="True">Selecione Operador</asp:ListItem>
                  <asp:ListItem Value="MSK">(MSK) MOLLER MAERSK</asp:ListItem>
                  <asp:ListItem Value="HSD">(HSD) HAMBURG SUDAMERIKAN</asp:ListItem>
                  <asp:ListItem Value="SEA">(SEA) MAERSK SEALAND</asp:ListItem>
              </asp:DropDownList>
           </td>
             <td class="bt-bottom  bt-right"><span id="valopera" class="validacion"> * obligatorio</span></td>
         </tr>

         <tr>

             <td class="bt-bottom  bt-right bt-left">7. Nombre del conductor:</td>
         <td class="bt-bottom" colspan="2">
             <span id="txtconductor" class="caja cajafull"    runat="server" clientidmode="Static"  enableviewstate="False">...</span>

         
             </td>
         <td class="bt-bottom bt-right validacion "><span class="validacion" id="valnombre" ></span></td>
         </tr>
         
         <tr><td class="bt-bottom  bt-right bt-left">8. Documento de identidad:</td>
         <td class="bt-bottom" >
         <input id="driID" type="hidden" runat="server" clientidmode="Static"  enableviewstate="False" />
         <span id="txtidentidad" class="caja cajafull "   
         runat="server" clientidmode="Static"  enableviewstate="False">...</span>

             </td>
              <td class="bt-bottom finder"  >
             <a  class="topopup" target="popup" onclick="window.open('../catalogo/chofer','name','width=850,height=480')" >
           <i class="ico-find" ></i> Buscar</a>
         </td>

         <td class="bt-bottom bt-right validacion "><span class="validacion" id="valced" > * obligatorio</span></td>
         </tr>

         <tr><td class="bt-bottom  bt-right bt-left">9. Placa del vehículo:</td>
         <td class="bt-bottom" colspan="2">

             <div class="popup" >
              <asp:TextBox ID="txtplaca" runat="server" MaxLength="10" CssClass="mayusc"
            onkeypress="return soloLetras(event,'abcdefghijklmnopqrstuvwxyz0123456789')" Width="250px"
             onBlur="validarPlaca(this,'valplaca');buble_warning();" placeholder="AAA0000" ClientIDMode="Static"
              ></asp:TextBox>
                 <span class="popuptext" id="verPop"></span>
                </div>

             </td>
         <td class="bt-bottom bt-right validacion "><span class="validacion" id="valplaca" > * obligatorio</span></td>
         </tr>
 



<tr>
         <td class="bt-bottom  bt-right bt-left">10. Mail notificación adicional</td>
          <td class="bt-bottom " >
          <input type='text' id='textbox1' name='textbox1'  runat="server" style= ' margin:0; width:200px'
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
         </table>--%>
        </div>

        <div><br /></div>

        <div class="form-row" >
            <div class="form-group col-md-12"> 
                 <div class="cataresult" >    
                        
                 <div id="xfinder2" runat="server" visible="false" >
                 <div class="findresult" >
                  <div class="booking" >
                        
                        <div class="form-group col-md-12"> 
                            <div class="form-title">DETALLE DE SALDO A FAVOR</div>
                        </div>
                        <div class="bokindetalle" style=" width:100%; overflow:auto">

                            <asp:Repeater ID="tablePagination" runat="server" >
                                 <HeaderTemplate>
                                     <table id="tablasort" cellspacing="1" cellpadding="1" class="table table-bordered invoice">
                                     <thead>
                                     <tr>
 
                                     <th># Pase</th>
                                     <th>Fecha de Salida</th>
                                     <th>Horario</th>
                                     <th>Booking</th>
                                     <th>Referencia</th>
                                     <th>Liquidación</th>
                                     <th>Estado Pase</th>
                                     <th>Estado Pago</th>
                                     <th>Valor Pase</th>
                                     <th>Saldo</th>
                                     </tr>
                                     </thead> 
                                     <tbody>
                                 </HeaderTemplate>
                                 <ItemTemplate>
                                     <tr class="point" >
                                      <td ><asp:Label Text='<%#Eval("turno_id")%>' ID="lbl_id_pase" runat="server"  /></td>
                                      <td ><asp:Label Text='<%#Eval("turno_fecha_dos")%>' ID="lbl_fecha_salida" runat="server" /></td>
                                      <td ><asp:Label Text='<%#Eval("turno_hora")%>' ID="lbl_turno" runat="server" /></td>
                                      <td ><asp:Label Text='<%#Eval("unidad_booking")%>' ID="lbl_booking" runat="server" /></td>
                                      <td ><asp:Label Text='<%#Eval("unidad_referencia")%>' ID="lbl_referencia" runat="server" /></td>
                                      <td ><asp:Label Text='<%#Eval("liquidacion")%>' ID="lbl_liquidacion" runat="server" /></td>
                                      <td ><asp:Label Text='<%#Eval("estado_pase")%>' ID="lbl_estado_pase" runat="server" /></td>
                                      <td ><asp:Label Text='<%#Eval("estado_pago")%>' ID="lbl_estado_pago" runat="server" /></td>
                                      <td ><asp:Label Text='<%#Eval("valor_pase")%>' ID="lbl_valor_pase" runat="server" /></td>   
                                     <td ><asp:Label Text='<%#Eval("saldo_pase")%>' ID="lbl_saldo_pase" runat="server" /></td>   

                      
                                     </tr>
                                  </ItemTemplate>
                             <FooterTemplate>
                             </tbody>
                             </table>
                             </FooterTemplate>
                        </asp:Repeater>
                            <table class="table table-bordered invoice" cellpadding="0" cellspacing="0">
                                <tr><td colspan="3"  ></td></tr>
                                <tr><td  class='border-none text-right font-weight-bold'><strong>SALDO A FAVOR:</strong></td><td class="estotal"><span id='tot_saldo' runat="server" clientidmode="Static" ><strong>$0000.00</strong></span></td></tr>
                            </table>
                        </div>
                  </div>
                 </div>
            
                </div>
             
                 </div>
            </div>

          <%--  <div><br /></div>--%>

            <div class="form-group col-md-12"> 
                 <div class="cataresult" >            
                      <div id="xfinder" runat="server" visible="false" >
                          <div class="findresult" >
                             <div class="booking" >
                                 <%--<div class="separator">Detalle De Valores a Cancelar [Proforma]</div>--%>

                                <div class="form-group col-md-12"> 
                                    <div class="form-title">Detalle De Valores a Cancelar [Proforma]</div>
                                </div>

                                 <div class="bokindetalle" style=" width:100%; overflow:auto">
                                 <fieldset>
                                         <asp:Repeater ID="tablaNueva" runat="server" >
                                             <HeaderTemplate>
                                                <table id="miProforma" class="table table-bordered invoice"  cellpadding="1" cellspacing="1">
                                                <thead>
                                                   <tr><th>Código</th><th>Descripcion</th><th>Cant.</th><th>P.Unit</th><th>P.Total</th></tr>
                                                </thead>
                                             </HeaderTemplate>
                                             <ItemTemplate>
                                             <tr class="point" >
                                              <td><%#Eval("codigo")%></td>
                                              <td><%#Eval("contenido")%></td>
                                              <td><%#Eval("cantidad")%></td>
                                              <td><%#DataBinder.Eval(Container.DataItem, "costo", "{0:C}")%></td>
                                              <td><%#DataBinder.Eval(Container.DataItem, "vtotal", "{0:C}")%></td>
                                             </tr>
                                              </ItemTemplate>
                                             <FooterTemplate>
                                             </tbody>
                                             </table>
                                             </FooterTemplate>
                                        </asp:Repeater>
                                       <%-- <table class="totales" cellpadding="0" cellspacing="0">
                                       <tr><td colspan="3"  ></td></tr>
                                       <tr><td  class='filat'><strong>SUBTOTAL:</strong></td><td class="estotal"><span id='stsubtotal' runat="server" clientidmode="Static" ><strong>$0000.00</strong></span></td></tr>
                                        <tr><td  class='filat' style="color:blue;"><strong>IVA:</strong></td><td class="estotal"><span id='stiva' style="color:blue;" runat="server" clientidmode="Static" ><strong>$0000.00</strong></span></td></tr>
                                        <tr><td  class='filat'><strong>NETO A PAGAR:</strong></td><td class="estotal"><span id='sttal' runat="server" clientidmode="Static" ><strong>$0000.00</strong></span></td></tr>
                                        </table>--%>
                                        <table class="totales" cellpadding="0" cellspacing="0">
                                        <tr><td  class='border-none text-right font-weight-bold'>Total:</td><td class="estotal"><span id='sttsubtotal' runat="server" clientidmode="Static"  >$0000.00</span></td></tr>
                                        <tr><td  class='border-none text-right text-info font-weight-bold'> <span runat="server" clientidmode="Static" id="st_saldo_anterior">Saldo Anterior:</span> </td><td class="estotal"><span id='sttsaldo_anterior' runat="server" clientidmode="Static"  >$0000.00</span></td></tr>
                                        <tr><td  class='border-none text-right font-weight-bold'> <span runat="server" clientidmode="Static" id="st_saldo_actual">Saldo Actual:</span> </td><td class="estotal"><span id='sttsaldo_actual' runat="server" clientidmode="Static"  >$0000.00</span></td></tr>
              
                                       <tr><td colspan="3"></td></tr>
                                        <tr><td  class='border-none text-right font-weight-bold'><strong>NETO A PAGAR:</strong></td><td class="estotal"><span id='sttal' runat="server" clientidmode="Static" ><strong>$0000.00</strong></span></td></tr>
                                        </table>
                                    </fieldset>
                                </div>
               
                             </div>
                         </div>
                      </div>
                 </div>
            </div>

            <div><br /></div>

            <div class="form-group col-md-12"> 
                <div class="row">
                    <div class="col-md-12 d-flex justify-content-center">
                        <asp:Button class="btn btn-primary"  ID="btbuscar" runat="server" Text="Generar AISV"  onclick="btbuscar_Click" 
                            OnClientClick="return getprocesa()" ToolTip="Generar AISV"/>
                    </div>
                </div>
            </div>

            <div class="form-group col-md-12"> 
                <div class="cataresult" >

                    <div id="sinresultado" runat="server" class="alert alert-danger" clientidmode="Static">
                    
                    </div>      

                </div>
            </div>

      </div>
    </div>
    <script src="../Scripts/pages.js" type="text/javascript"></script>

    <script type="text/javascript">
        $(document).ready(function () {
            var t = document.getElementById('bandera');
            if (t.value != undefined && t.value != null && t.value.trim().length > 0) {
                $('span.validacion').text('');
            }

        });

        function setMsg(mens) {
            var m = document.getElementById('sinresultado');
            if (m != null) {
                m.setAttribute("class", "");
                m.textContent = '';
            }
            var i = document.getElementById('sinresultado');
            if (i != null) {
                i.setAttribute("class", "");
                i.setAttribute("class", "msg-critico");
                i.textContent = mens;
            }

        }

        function getprocesa() {
            var i = document.getElementById('dpturno');
            //1.Turno horario
            if (i == null || i.value == '0' || i.value == '') {
                 setMsg('Seleccione el horario de ingreso');
               //alert('Por favor seleccione el horario');
                return false;
            }
            i = document.getElementById('txtcontenedor');
            //2.Contenedor
            if (i == null || i.value == '0' || i.value == '') {
               // alert('Por favor escriba el numero del contenedor');
                setMsg('Por favor escriba el numero del contenedor');
                return false;
            }
            i = document.getElementById('dptamano');
            //tamaño
            if (i == null || i.value == '0' || i.value == '') {
                setMsg('Seleccione el tamaño del contenedor');
               // alert('Por favor seleccione el tamaño de contenedor');
                return false;
            }
            i = document.getElementById('dplinea');
            //operador y linea
            if (i == null || i.value == '0' || i.value == '') {
                setMsg('Seleccione el operador del contenedor');
                //alert('Por favor seleccione el operador');
                return false;
            }
            i = document.getElementById('dv_licencia');
            //condutor
            if (i == null || i.value == '0' || i.value == '') {
                 setMsg('Por favor seleccione el conductor');
               // alert('Por favor seleccione el conductor');
                return false;
            }
            i = document.getElementById('txtplaca');
            //placa
            if (i == null || i.value == '0' || i.value == '') {
                // alert('Por favor escriba el numero de placa');
                setMsg('Por favor escriba el numero de placa');
                return false;
            }
     
            return true;
        }
        function onBook() {
            tipo = 'MTY';
            var w =  window.open('../catalogo/booking.aspx?tipo=' + tipo + '&v=1', 'Bookings', 'width=850,height=880');
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

            //para pantalla
            document.getElementById('refnumber').textContent = objeto.numero + '/' + objeto.referencia;
            //item para servidor
            document.getElementById('bk_num').value =objeto.numero;
            document.getElementById('bk_linea').value = objeto.linea;
            document.getElementById('bk_iso').value = objeto.iso;

        }

        function driverCallback(data) {
            this.document.getElementById('dv_licencia').value = data.codigo;
            this.document.getElementById('dv_nombre').value = data.descripcion;
            this.document.getElementById('txtconductor').textContent = data.descripcion;
            this.document.getElementById('txtidentidad').textContent = data.codigo;

            this.document.getElementById('valced').textContent = '';
            var m = document.getElementById('sinresultado');
            if (m != null) {
                m.setAttribute("class", "");
                m.textContent = '';
            }
            
           //limpiar obligatorio
        }

     $('form').live("submit", function () { ShowProgress();});
    </script>
 <div class="loading" align="center">
    Estamos verificando toda la información 
    que nos facilitó,por favor espere unos segundos<br />
    <img src="../shared/imgs/loader.gif" alt="x" />
</div>
</asp:Content>