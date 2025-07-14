<%@ Page  Title="Seleccion de Turno" MaintainScrollPositionOnPostback="true"  EnableEventValidation="true" Language="C#" MasterPageFile="~/site.Master" AutoEventWireup="true" CodeBehind="turno_opc_original.aspx.cs" Inherits="CSLSite.turno_opc_original" %>
<%@ MasterType VirtualPath="~/site.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="placehead" runat="server">
    <link href="../shared/estilo/aisv-general.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/work.css" rel="stylesheet" type="text/css" />
        <link href="../shared/estilo/tables.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/opc_tables.css" rel="stylesheet" />
 </asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="placebody" runat="server">
   
    <input id="zonaid" type="hidden" value="1188" />
     <noscript><meta http-equiv="refresh" content="0; url=sinsoporte.htm" /> </noscript>
     <div>
         <i class="ico-titulo-1"></i><h2>Planificación de trabajo de estiba </h2>  <br /> 
         <i class="ico-titulo-2"></i><h1>Selección de Turno para OPC</h1><br />
    </div>
     <div class=" msg-alerta">
    <span id="dtlo" runat="server">Estimado usuario:</span> 
    <br /> Asegurese de seleccionar el turno apropiado antes de realizar la asignación, recuerde elegir siempre la OPC correcta en la lista.
    </div>
     
     <div class="seccion">
      <div class="informativo">
      <table>
      <tr><td rowspan="2" class="inum"> <div class="number">1</div></td><td class="level1" >Datos de la Nave:</td></tr>

      </table>
     </div>
    <div class="colapser colapsa"></div>
      <div class="accion">
     <table class="controles" cellspacing="0" cellpadding="1">

         <tr>
         <td class="bt-bottom  bt-right bt-left" >Referencia:  </td>
         <td class="bt-bottom bt-right">
           <span runat="server" id="referencia" class="caja cajafull">... </span>
         </td>
         <td class="bt-bottom  bt-right ">Viaje IN/OUT:</td>
         <td class="bt-bottom bt-right">
           <span runat="server"  id="vio" class="caja cajafull">...</span>
         </td>
         </tr>
           <tr><td class="bt-left bt-bottom  bt-right ">Nombre de la nave:</td>
         <td class="bt-bottom bt-right">
           <span  runat="server" id="buque" class="caja cajafull">...</span>
         </td>
          <td class="bt-bottom  bt-right bt-left">Muelle:</td>
         <td class="bt-bottom bt-right">
           <span runat="server" id="muelle" class="caja cajafull">...</span>
         </td>
         </tr>

         <tr><td class="bt-bottom  bt-right bt-left">Fecha estimada de arribo:</td>
         <td class="bt-bottom bt-right">
           <span runat="server" id="eta" class="caja cajafull">...</span>
         </td>
             <td class="bt-bottom  bt-right bt-left">Fecha estimada de Zarpe:</td>
         <td class="bt-bottom bt-right">
           <span runat="server" id="etd" class="caja cajafull">...</span>
         </td>
         </tr>



     
        <tr><td class="bt-bottom  bt-right bt-left">Fecha de arribo:</td>
         <td class="bt-bottom bt-right">
           <span runat="server" id="ata" class="caja cajafull">...</span>
         </td>
                        <td class="bt-bottom  bt-right bt-left">Fecha de Zarpe:</td>
         <td class="bt-bottom bt-right">
           <span runat="server" id="atd" class="caja cajafull">...</span>
         </td>
         </tr>
       
        <tr><td class="bt-bottom  bt-right bt-left">Inicio operaciones:</td>
         <td class="bt-bottom bt-right">
           <span runat="server" id="wini" class="caja cajafull">...</span>
         </td>
          <td class="bt-bottom  bt-right bt-left">Fin operaciones:</td>
         <td class="bt-bottom bt-right">
           <span runat="server" id="wend" class="caja cajafull">...</span>
         </td>
         </tr>


             <tr><td class="bt-bottom  bt-right bt-left">Fecha Hora de Citación:</td>
         <td class="bt-bottom bt-right" colspan="3">
           <span runat="server" id="fcita" class="caja cajafull">...</span>
         </td>
         </tr>




    


     </table>
     </div>
    </div>

        <div class="seccion">
      <div class="informativo">
      <table>
      <tr><td class="inum"> <div class="number">2</div></td><td class="level1" >Detalle de Grúas</td></tr>
      </table>
     </div>
      <div class="colapser colapsa"></div>
     <div class="accion" >
      <div class="bokindetalle" >
          <div id="sin_gruas" runat="server" class="msg-info">Este documento no tiene grúas asignadas</div>
       <asp:Repeater ID="rp_grua" runat="server" >
                 <HeaderTemplate>
                 <table id="tbgru" cellspacing="1" cellpadding="1" class="tabRepeat">
                 <thead>
                 <tr>
                 <th>Nombre</th>
                 <th>Trabajo (Hrs)</th>
                 <th>Inicio Trabajos</th>
                 </tr>
                 </thead> 
                 <tbody>
                 </HeaderTemplate>
                 <ItemTemplate>
                 <tr class="point" >
                  <td><%#Eval("Crane_name")%></td>
                  <td><%#Eval("Crane_time_qty")%></td>
                  <td><%#Eval("DateWork")%></td>
  
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
    


     <div class="seccion">
      <div class="informativo">
      <table>
      <tr><td rowspan="2" class="inum"> <div class="number">3</div></td><td class="level1" >Selección del Turno</td></tr>
      <tr>
      <td class="level2">
         De click en EL LINK para asignar el personal para cada turno
      </td>
      </tr>
      </table>
     </div>
      <div class="colapser colapsa"></div>
     <div class="accion" id="ADU">
     <div class="bokindetalle" >
         <div id="sin_turno" runat="server" class="msg-info">Este documento no tiene turnos creados o disponibles</div>
                 <asp:Repeater ID="rp_turno" runat="server"  OnItemCommand="rp_turno_ItemCommand" OnItemDataBound="rp_turno_ItemDataBound"    >
                 <HeaderTemplate>
                 <table id="tablasort" cellspacing="1" cellpadding="1" class="tabRepeat">
                 <thead>
                 <tr>
                 <th>Cuadrilla #</th>
                 <th>Grúa</th>
                 <th>Inicio </th>
                 <th>Final</th>
                 <th>OPC</th>
                 
                 <th>Acciones</th>
                 </tr>
                 </thead> 
                 <tbody>
                 </HeaderTemplate>
                 <ItemTemplate>
                 <tr class="point" >
                  <td><%#Eval("turno_number")%></td>
                  <td><%#Eval("crane_name")%></td>
                  
                  <td><%#Eval("turn_time_start")%></td>
                  <td><%#Eval("turn_time_end")%></td>
                  <td><%#testado(Eval("opc_name"))%></td>
                  <td>
                   <div class="tcomand top" id="div_control" runat="server"  >
                       <a class='<%#toSet(Eval("opc_id"))%>'  href="javascript:void popOpen('asignar_op.aspx?sid=<%# securetext(Eval("id")) %>')" >ASIGNAR</a>
                       <div class='<%#controles(Eval("t_status"))%>' id="botonera" >
                        <a href="javascript:void popOpen('asignar_op.aspx?op=e&sid=<%# securetext(Eval("id")) %>')" >EDITAR</a>
                       &nbsp;|&nbsp;
                       <asp:Button ID="btremover"  
                       OnClientClick="return confirm('Esta seguro de remover la asignación de cuadrilla?');" 
                        runat="server" 
                        Text="Liberar" 
                        CssClass="Anular" 
                        CommandArgument='<%#Eval("id")%>' 
                        ToolTip="Permite liberar la cuadrilla y turno" />
                       </div>
                   </div>
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
         <br />
    </div>
    <script type="text/javascript">
        $(window).load(function () {
                            $("div.colapser").toggle(function () { $(this).removeClass("colapsa").addClass("expande"); $(this).next().hide(); }
                                  , function () { $(this).removeClass("expande").addClass("colapsa"); $(this).next().show(); });
        });
        function popOpen(URL) {
            window.open(URL, '', 'width=1000, height=1200, top=0, left=100, scrollbar=no, resize=no, menus=no');
       
        }
    </script>
</asp:Content>