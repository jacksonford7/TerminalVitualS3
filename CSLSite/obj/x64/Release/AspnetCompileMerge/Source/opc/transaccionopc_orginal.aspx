<%@ Page  Title="AISV Contenedores"  Language="C#" MasterPageFile="~/site.Master" AutoEventWireup="true" CodeBehind="transaccionopc_orginal.aspx.cs" Inherits="CSLSite.transaccionopc_orginal" MaintainScrollPositionOnPostback="True" %>
<%@ MasterType VirtualPath="~/site.Master" %>


<asp:Content ID="Content1" ContentPlaceHolderID="placehead" runat="server">
    <link href="../shared/estilo/aisv-general.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/work.css" rel="stylesheet" type="text/css" />
     <link href="../shared/estilo/tables.css" rel="stylesheet" type="text/css" />
     <link href="../shared/estilo/catalogos.css" rel="stylesheet" type="text/css" />
     <link href="../shared/estilo/info.css" rel="stylesheet" type="text/css" />

    <style type="text/css">
        .auto-style6 {
            width: 177px;
        }
    </style>
 </asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="placebody" runat="server" >
    <input id="zonaid" type="hidden" value="205" />
     <noscript><meta http-equiv="refresh" content="0; url=sinsoporte.htm" /> </noscript>
     <div>
         <i class="ico-titulo-1"></i><h2>Planificación y Control de Trabajos</h2>  <br /> 
         <i class="ico-titulo-2"></i><h1>Transacción de Trabajo por Buque y Grúas</h1><br />
    </div>
     <div class=" msg-alerta">
    <span id="dtlo" runat="server">Estimado usuario:</span> 
    <br /> ...
    </div>
     <div class="seccion" id="BUSCAR">
      <div class="informativo">
      <table>
      <tr><td rowspan="2" class="inum"> <div class="number">1</div></td><td class="level1" >Datos de la Nave</td></tr>
      <tr><td class="level2">Debe digitar la referencia de la nave, para luego dar click en el botón buscar, y visualizar los datos de la misma.</td></tr>
      </table>
     </div>
     <div class="colapser colapsa" ></div>
     <div class="accion">
     <table class="controles" cellspacing="0" cellpadding="1">
        <tr><th class="bt-bottom bt-right bt-top bt-left" colspan="4"> Búsqueda de Nave </th></tr>
         <tr>
         <td class="bt-bottom  bt-right bt-left" >1. Digite la Referencia de la Nave:</td>
         <td class="bt-bottom bt-right" colspan="3">
        <asp:TextBox ID="TxtReferencia" runat="server" CssClass="inputText" MaxLength="10"  ClientIDMode="Static"
         Width="120px" onkeypress="return soloLetras(event,'abcdefghijklmnopqrstuvwxyz1234567890',true)" 
            
            placeholder="Referencia"></asp:TextBox>
             <span class="validacion" id="xplinea" > * obligatorio</span>
         <asp:Button ID="BtnBuscar" runat="server"   OnClick="BtnBuscar_Click" Text="Consultar" Width="100px"/>
         </td>       
         </tr>

         <tr><td class="bt-bottom  bt-right bt-left">NOMBRE:</td>
         <td class="bt-bottom bt-right"> <span id="nombre" class="caja cajafull"><asp:Label ID="LblNombre" runat="server" Text="LblNombre" Width="379px"></asp:Label>
             <asp:Label ID="LblVoyageIn" runat="server" Text="LblVoyageIn" Width="379px" Visible="false"></asp:Label>
              <asp:Label ID="LblVoyageOut" runat="server" Text="LblVoyageOut" Width="379px" Visible="false"></asp:Label>

                                         </span>
          
         </td>    
         </tr>

         <tr><td class="bt-bottom  bt-right bt-left">ETA</td>
         <td class="bt-bottom bt-right"><span id="etas" class="caja cajafull"><asp:Label ID="LblETA" runat="server" Text="LblETA" Width="379px"></asp:Label></span>
         
         </td>       
         </tr>

         <tr><td class="bt-bottom  bt-right bt-left">ETD</td>
         <td class="bt-bottom bt-right"> <span id="etd" class="caja cajafull"><asp:Label ID="LblETD" runat="server" Text="LblETD" Width="379px"></asp:Label></span>
         
         </td>
         </tr>
          <tr><td class="bt-bottom  bt-right bt-left">VIAJE</td>
         <td class="bt-bottom bt-right"> <span id="viaje" class="caja cajafull"><asp:Label ID="LblViaje" runat="server" Text="LblViaje" Width="379px"></asp:Label></span>
           
         </td>
         </tr>
          <tr><td class="bt-bottom  bt-right bt-left">HORAS DE TRABAJO</td>
         <td class="bt-bottom bt-right"> <span id="Horas" class="caja cajafull"><asp:Label ID="LblHoras" runat="server" Text="LblHoras" Width="379px"></asp:Label></span>
           
         </td>
         </tr>
          <tr>
         <td class="bt-bottom  bt-right bt-left" >Fecha y Hora de la Cita:</td>
         <td class="bt-bottom bt-right" colspan="3">
         <asp:TextBox ID="TxtFechacita" runat="server" MaxLength="16" Width="250px" CssClass="datetimepicker"
               onkeypress="return soloLetras(event,'1234567890/:')"
               onBlur="valDate(this,  true,valdatem);"
               placeholder="dd/mm/aaaa hh:mm"
             onchange="defaultDate(this);"
               ></asp:TextBox>
             <span class="validacion" id="valfechacita"  > * obligatorio</span>
         </td>       
         </tr>

     </table>
     </div>
    </div>
     <div class="seccion">
      <div class="informativo">
      <table>
      <tr><td rowspan="2" class="inum"> <div class="number">2</div></td><td class="level1" >Datos de la Grúa</td></tr>
      <tr><td class="level2">
       Se debe seleccionar una grúa, ingresar la fecha de inicio del trabajo de la misma, debe digitar la cantidad de horas que va a trabajar, para que se generen los turnos de forma automática.
      </td></tr>
      </table>
     </div>
    <div class="colapser colapsa"></div>
      <div class="accion">
     <table class="controles" cellspacing="0" cellpadding="1">
        <tr><th class="bt-bottom bt-right bt-top bt-left" colspan="2"> SELECCIONAR GRÚA</th></tr>
         <tr>
         <td class="auto-style6" >Grúa</td>
          <td class="bt-bottom bt-right">
           <asp:DropDownList ID="CboGruas" runat="server" Width="100px" AutoPostBack="False"
                                            Height="20px"  DataTextField='Id' DataValueField='GKey'  
                                            Font-Size="XX-Small" 
                                            onselectedindexchanged="CboGruas_SelectedIndexChanged">
                                        </asp:DropDownList>
              <span class="validacion" id="valcbogrua"  > * obligatorio</span>
         </td>            
         </tr>
          <tr>
         <td class="auto-style6" >Desde</td>
          <td class="bt-bottom bt-right">
           <asp:TextBox ID="TxtFechaDesde" runat="server" MaxLength="16" Width="250px" CssClass="datetimepicker"
               onkeypress="return soloLetras(event,'1234567890/:')"
               onBlur="valDate(this,  true,valdatem);"
               placeholder="dd/mm/aaaa hh:mm"
                ClientIDMode="Static"
               ></asp:TextBox>
             <span class="validacion" id="valFechaDesde"  > * obligatorio</span>
         </td>            
         </tr>
          <tr>
         <td class="auto-style6" >Horas de Trabajo</td>
          <td class="bt-bottom bt-right">
          <asp:TextBox ID="TxtHorasTrabajo" runat="server" CssClass="inputText" MaxLength="15"
         Width="120px" onkeypress="return soloLetras(event,'1234567890',false)" onpaste="return false;"></asp:TextBox>
          <span class="validacion" id="valhoras"  > * obligatorio</span></td>            
         </tr>
          <tr>
         <td class="auto-style6" >Agregar Detalle</td>
          <td class="bt-bottom bt-right">
          <asp:Button ID="BtnAgregar" runat="server"  OnClick="BtnAgregar_Click" Text="Agregar" Width="100px"/>
          </td>            
         </tr>
         
          <tr><th colspan="2">
                <div class="findresult" >
              <div class="booking" >
               <div class="separator">DETALLE GRÚA</div>
              <div class="bokindetalle">

          <asp:Repeater ID="TableGruas" runat="server"  onitemcommand="RemoverGruas_ItemCommand">
                <HeaderTemplate>
                <table id="tablasort" cellspacing="0" cellpadding="1" class="tabRepeat">
                <thead>
                <tr>
                    <th style='width:50px'>ID</th>
                    <th style='width:250px'>GRUA</th>
                    <th style='width:150px'>DESDE</th>
                    <th style='width:50px'>HORAS</th>
                    <th class ="nover">NOTA</th>
                    <th >REMOVER</th>
                  </tr>
                </thead> 
                <tbody>
                </HeaderTemplate>
                <ItemTemplate>
                <tr class="point">
                <td ><asp:Label Text='<%#Eval("Crane_Gkey")%>' ID="lblCarga" runat="server"  /></td>
                <td ><asp:Label Text='<%#Eval("Crane_name")%>' ID="lblCntr" runat="server" /></td>
                <td ><asp:Label Text='<%#Eval("DateWork")%>' ID="lblTipo" runat="server" /></td>
                <td ><asp:Label Text='<%#Eval("Crane_time_qty")%>' ID="lblFacA" runat="server" /></td>
                <td class ="nover">
                     <asp:TextBox ID="TxtNota"  runat="server" AutoPostBack="False"  Font-Size="X-Small" Width="250px"  onkeypress="return soloLetras(event,'abcdefghijklmnñopqrstuvwxyz1234567890_-')" Visible="false"></asp:TextBox>
                </td>                  
                <td class="alinear" style=" width:50px">
                  
                    <asp:Button ID="BtnConfirmar"  
                       OnClientClick="return confirm('Está seguro de que desea remover la grúa?......Al realizar esta acción se eliminarán todos los turnos de la grúa.');" 
                       runat="server" Text="Remover" CssClass="Anular" ToolTip="Permite remover una grúa" CommandArgument='<%#Eval("Crane_Gkey")%>' />
                    
                </td>
                </tr>
                </ItemTemplate>
                <FooterTemplate>
                </tbody>
                </table>
                </FooterTemplate>
        </asp:Repeater>
                   </div>
              </div>
                     </div>
            </th>
          </tr>


     </table>
     </div>
    </div>
     <div class="seccion">
      <div class="informativo">
      <table>
      <tr><td rowspan="2" class="inum"> <div class="number">3</div></td><td class="level1" >Dato de los Turnos</td></tr>
      <tr>
      <td class="level2">
         Ingrese los datos requeridos en cada una de las siguientes secciones.
      </td>
      </tr>
      </table>
     </div>
      <div class="colapser colapsa"></div>
     <div class="accion" id="ADU">
  

   
      <table class="controles" cellspacing="0" cellpadding="1">
        <tr><th class="bt-bottom bt-right  bt-left" colspan="3"> SELECCIONAR TURNO</th></tr>
           <tr><th colspan="2">
                <div class="findresult" >
              <div class="booking" >
               <div class="separator">DETALLE DE TURNOS</div>
              <div class="bokindetalle">

          <asp:Repeater ID="TableTurnos" runat="server"  onitemcommand="RemoverTurno_ItemCommand">
                <HeaderTemplate>
                <table id="tablaTurno" cellspacing="0" cellpadding="1" class="tabRepeat">
                <thead>
                <tr>
                    <th style='width:50px' class ="nover">ID</th>
                    <th style='width:100px'>GRUA</th>
                    <th style='width:50px'>TURNO</th>
                    <th style='width:150px' class ="nover">IDGRUDA</th>
                    <th style='width:150px'>HORA</th>
                    <th >REMOVER</th>
                  </tr>
                </thead> 
                <tbody>
                </HeaderTemplate>
                <ItemTemplate>
                <tr class="point">
                <td class ="nover"><asp:Label Text='<%#Eval("id")%>' ID="lblid" runat="server" Visible="false"/></td>
                <td ><asp:Label Text='<%#Eval("crane_name")%>' ID="lblcrane_name" runat="server"  /></td>
                <td ><asp:Label Text='<%#Eval("turno_number")%>' ID="lblturno_nomber" runat="server" /></td>
                <td class ="nover"><asp:Label Text='<%#Eval("crane_id")%>' ID="lblcrane_id" runat="server" Visible="false" /></td>
                <td ><asp:Label Text='<%#Eval("turn_time_start")%>' ID="lblstart" runat="server" /></td>
                               
                <td class="alinear" style=" width:50px">
                  
                    <asp:Button ID="BtnConfirmar"  
                       OnClientClick="return confirm('Esta seguro de que desea remover el turno de la grúa?');" 
                       CommandArgument=   '<%# jsarguments( Eval("crane_id"),Eval("turno_number"))%>'
                       runat="server" Text="Remover" CssClass="Anular" ToolTip="Permite remover el turno de la grúa" />
                    
                </td>
                </tr>
                </ItemTemplate>
                <FooterTemplate>
                </tbody>
                </table>
                </FooterTemplate>
        </asp:Repeater>
                   </div>
              </div>
                     </div>
            </th>
          </tr>
         <tr>
         <td class="bt-bottom  bt-right bt-left" >&nbsp;</td>

         </tr>
         </table>
     
      <div class="botonera">
         <img alt="loading.." src="../shared/imgs/loader.gif" id="loader" class="nover"  />
         <asp:Button ID="BtnGrabar" runat="server"  OnClick="BtnGrabar_Click" Text="Grabar" Width="100px"/> &nbsp;
          <asp:Button ID="BtnNuevo" runat="server"  OnClick="BtnNuevo_Click" Text="Nuevo" Width="100px"/> &nbsp;

     </div>
    
     </div>
    </div>


    <script src="../Scripts/pages.js" type="text/javascript"></script>
   <script src="../Scripts/opc_control.js" type="text/javascript"></script>

    <script type="text/javascript">
    
        $(window).load(function () {
            //objeto a transportar.
            $(document).ready(function () {
                //inicia los fecha-hora
                $('.datetimepicker').datetimepicker().datetimepicker({ lang: 'es', step: 30, format: 'd/m/Y H:i' });
                //inicia los fecha
                $('.datetimepickerAlt').datetimepicker().datetimepicker({ lang: 'es', step: 30, format: 'd/m/Y' });

                $('.datepicker').datetimepicker().datetimepicker({ lang: 'es', timepicker: false, format: 'd/m/Y', closeOnDateSelect: true, minDate: '0' });
                //init reefer-> lo pone a false.

            });
        });
   
    </script>

    <script type="text/javascript">

        $(window).load(function () {
                            $("div.colapser").toggle(function () { $(this).removeClass("colapsa").addClass("expande"); $(this).next().hide(); }
                                  , function () { $(this).removeClass("expande").addClass("colapsa"); $(this).next().show(); });
        });


        function cancelEmpty() {
            var txt = document.getElementById("TxtReferencia");
            if (txt != null && txt != undefined) {
                if (txt.value) {
                    return true;
                }
            }
            //detenga el post
            alert("Por favor escriba la referencia");
            return false;
        }


        function defaultDate(control) {
            if (control.value) {
                var cj = document.getElementById("TxtFechaDesde");
                if (cj != null && cj != undefined) {
                    cj.value = control.value;
                }
            }
        }
    </script>

</asp:Content>