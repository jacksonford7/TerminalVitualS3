<%@ Page  Title=""  Language="C#" MasterPageFile="~/site.Master" 
    AutoEventWireup="true" CodeBehind="asignar_op_original.aspx.cs" Inherits="CSLSite.asignar_op_original" %>
<%@ MasterType VirtualPath="~/site.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="placehead" runat="server">
    <link href="../shared/estilo/aisv-general.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/work.css" rel="stylesheet" type="text/css" />
     <link href="../shared/estilo/tables.css" rel="stylesheet" type="text/css" />

 </asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="placebody" runat="server">

    <input id="zonaid" type="hidden" value="1188" />

    <input id="identificacion" type="hidden" value="" runat="server" clientidmode="Static" />
     <input id="nombres" type="hidden" value="" runat="server" clientidmode="Static" />
    <input id="apellidos" type="hidden" value="" runat="server" clientidmode="Static" />
     <noscript><meta http-equiv="refresh" content="0; url=sinsoporte.htm" /> </noscript>
     <div>
         <i class="ico-titulo-1"></i><h2>Planificación de trabajo de estiba </h2>  <br /> 
         <i class="ico-titulo-2"></i><h1>Selección de operarios para Turno.</h1><br />
    </div>
     <div class=" alert alert-warning">
    <span id="dtlo" runat="server">Estimado usuario:</span> 
    <br /> Por favor asegúrese de selecionar a las personas correctas de la lista anres de guardar la cuadrilla
    </div>
     
     <div class="seccion">
      <div class="informativo">
      <table>
      <tr><td rowspan="2" class="inum"> <div class="number">1</div></td><td class="level1" >Datos del Turno</td></tr>

      </table>
     </div>
    <div class="colapser colapsa"></div>
      <div class="accion">
     <table class="controles" cellspacing="0" cellpadding="1">

         <tr>
         <td class="bt-bottom  bt-right bt-left" >Operadora Actual:</td>
         <td class="bt-bottom bt-right" colspan="3">
           <span runat="server" id="operator_no" class="caja cajafull">... </span>
         </td>
         </tr>
           <tr><td class="bt-bottom  bt-right bt-left">Referencia:</td>
         <td class="bt-bottom bt-right">
           <span  runat="server" id="referencia" class="caja cajafull">...</span>
         </td>
               <td class="bt-bottom  bt-right bt-left">Grúa:</td>
         <td class="bt-bottom bt-right">
           <span runat="server" id="grua" class="caja cajafull">...</span>
         </td>
         </tr>
  

         <tr><td class="bt-bottom  bt-right bt-left">Turno:</td>
         <td class="bt-bottom bt-right" colspan="3">
           <span runat="server" id="turno" class="caja cajafull">...</span>
         </td>
         </tr>
         <tr>
             <td class="bt-bottom  bt-right bt-left" ><strong>Operadora de Cuadrilla</strong> </td>
             
             <td class="bt-bottom bt-right" >
                 <asp:DropDownList ID="dpopc" runat="server"  CssClass="form-control" ClientIDMode="Static"></asp:DropDownList>
             </td>
              <td class="bt-bottom  bt-right bt-left" colspan="2">
                  <strong>Amarre</strong>
                  <asp:CheckBox ID="vlock" runat="server" /> &nbsp;&nbsp;
                 &nbsp;&nbsp;
                  <strong>Desamarre</strong>
                  <asp:CheckBox ID="vunlock" runat="server"  />
                  </td>
         </tr>
     </table>
     </div>
    </div>
        <div class="seccion">
      <div class="informativo">
      <table>
      <tr><td class="inum"> <div class="number">2</div></td><td class="level1" >Selección de Cuadrilla</td></tr>
      </table>
     </div>
      <div class="colapser colapsa"></div>
     <div class="accion" >
      <table class="controles" cellspacing="0" cellpadding="1">
         <tr>
         <td class="bt-bottom  bt-right bt-left" >Nombres y Apellidos:</td>
         <td class="bt-bottom bt-right">
           <span clientidmode="Static" onclick="Golink();" runat="server" id="operario_name" class="caja cajafull" style="cursor:pointer;">Click aqui para buscar </span>
         </td>
             <td  class="bt-bottom  bt-right">
                 <asp:Button CssClass="btn btn-primary" ID="btAdd" runat="server"  Text="Agregar" OnClick="btAdd_Click" OnClientClick="return check_info();" />
             </td>
         </tr>
     </table>
    </div>
    </div>
     <div class="seccion">
      <div class="informativo">
      <table>
      <tr><td class="inum"> <div class="number">3</div></td><td class="level1" >Personal Seleccionado</td></tr>
      </table>
     </div>
      <div class="colapser colapsa"></div>
     <div class="accion" id="ADU">
       
     <div class="bokindetalle">
                 <asp:Repeater ID="rp_turno" runat="server" OnItemCommand="rp_turno_ItemCommand" >
                 <HeaderTemplate>
                 <table id="tablasort" cellspacing="1" cellpadding="1" class="tabRepeat">
                 <thead>
                 <tr>
                 <th>Identificación</th>
                 <th>Nombres y Apellidos</th>
                  <th>Acciones</th>
                 </tr>
                 </thead> 
                 <tbody>
                 </HeaderTemplate>
                 <ItemTemplate>
                 <tr class="point" >
                  <td><%#Eval("operator_id")%></td>
                  <td><%#join( Eval("operator_names"), Eval("operator_apellidos")) %>,</td>
                  <td>
                   <div class="tcomand">
                       <asp:Button CommandArgument='<%# Eval("operator_id")%>'  ID="bt_quitar" runat="server" Text="Remover de la lista"   CssClass="btn btn-secondary" />
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
          <div class="botonera">
              <asp:Button OnClientClick="return confirm('Está seguro de guardar la cuadrilla, este proceso es irreversible?');"   ID="btsalvar" runat="server" Text="Guardar cuadrilla" OnClick="btsalvar_Click" />
     </div>

    <script type="text/javascript">
        $(window).load(function () {
                            $("div.colapser").toggle(function () { $(this).removeClass("colapsa").addClass("expande"); $(this).next().hide(); }
                                  , function () { $(this).removeClass("expande").addClass("colapsa"); $(this).next().show(); });
        });
    </script>
    <script src="../Scripts/opc_control.js" type="text/javascript"></script>
</asp:Content>