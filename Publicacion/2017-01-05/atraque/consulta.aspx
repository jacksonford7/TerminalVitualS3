<%@ Page Title="Consulta y Reimpresion" Language="C#" MasterPageFile="~/site.Master" AutoEventWireup="true" CodeBehind="consulta.aspx.cs" Inherits="CSLSite.atraque.consulta" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="placehead" runat="server">
    <link href="../shared/estilo/tables.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/aisv-general.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/work.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/catalogos.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/info.css" rel="stylesheet" type="text/css" />
        <script type="text/javascript">
            function BindFunctions() {
                $(document).ready(function () {
                    document.getElementById('imagen').innerHTML = '';
                    $('#tablasort').tablesorter({ cancelSelection: true, cssAsc: "ascendente", cssDesc: "descendente", headers: { 8: { sorter: false }, 9: { sorter: false}} }).tablesorterPager({ container: $('#pager'), positionFixed: false, pagesize: 10 });
                });
            }
    </script>
    <style type="text/css">
        
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
    
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="placebody" runat="server">
<input id="zonaid" type="hidden" value="301" />
    <asp:ToolkitScriptManager ID="tkcal" EnableScriptGlobalization="true" runat="server"> </asp:ToolkitScriptManager>
    <div>
   <i class="ico-titulo-1"></i><h2>Solicitud de Atraque</h2>  <br /> 
   <i class="ico-titulo-2"></i><h1>Consulta y reimpresión de solicitudes</h1><br />
 </div>
  <div class="seccion">
       <div class="accion">
            <table class="xcontroles" cellspacing="0" cellpadding="1">
        <tr><th class="bt-bottom bt-right  bt-left bt-top" colspan="5"> Datos del documento buscado</th></tr>
         <tr>
         <td  class="bt-bottom bt-left bt-right" >Referencia :</td>
         <td class="bt-bottom" >
             <asp:TextBox ID="treferencia" runat="server" Width="120px" MaxLength="10" 
             
                 onkeypress="return soloLetras(event,'01234567890abcdefghijklmnopqrstuvwxyz',true)"></asp:TextBox>
          </td>
           <td class="bt-bottom" >IMO:</td>
          <td class="bt-bottom" >
             <asp:TextBox ID="timo" runat="server" Width="120px" MaxLength="10"  
             
                  
                  onkeypress="return soloLetras(event,'01234567890abcdefghijklmnopqrstuvwxyz -_')"></asp:TextBox>
          </td>
         <td class="bt-bottom bt-right validacion "><span id="valran" class="opcional"> * opcional</span></td>
         </tr>
          <tr>
         <td class="bt-bottom  bt-right bt-left">Nave:</td>
          <td class="bt-bottom bt-right " colspan="1" >
               <asp:TextBox ID="tnave" runat="server" Width="160px" MaxLength="50"  
                   
                   onkeypress="return soloLetras(event,'01234567890abcdefghijklmnopqrstuvwxyz_/ ')"></asp:TextBox>
          </td>
            <td class="bt-bottom">&nbsp;Manifiesto:</td>
            <td class="bt-bottom">
            
            <asp:TextBox ID="tmani" runat="server" ClientIDMode="Static" Width="120px" MaxLength="16" 
             onkeypress="return soloLetras(event,'abcdefghijklmnopqrstuvwxyz1234567890-_')" 
                 ></asp:TextBox>
            
            </td>
             <td class="bt-bottom  bt-right "><span id="valdae" class="opcional"> * opcional</span></td>
         </tr>
         <tr>
         <td class="bt-bottom  bt-right bt-left">Generadas desde el día:</td>
         <td class="bt-bottom">
            <asp:TextBox ID="desded" runat="server" Width="120px" MaxLength="10" CssClass="datetimepicker"
             onkeypress="return soloLetras(event,'01234567890/')" 
                 onblur="valDate(this,true,valdate);" ClientIDMode="Static"></asp:TextBox>

             </td>
          <td class="bt-bottom" >Hasta:</td>
          <td class="bt-bottom " >
             <asp:TextBox ID="hastad" runat="server" ClientIDMode="Static" Width="120px" MaxLength="15" CssClass="datetimepicker"
             onkeypress="return soloLetras(event,'01234567890/')" 
                  onblur="valDate(this,true,valdate);"></asp:TextBox>
   
          </td>
         <td class="bt-bottom bt-right validacion ">
         <span id="valdate" class="validacion"> * obligatorio</span>
         </td>
         </tr>

         </table>
         <div class="botonera">
          <span id="imagen"></span>
             <asp:Button ID="btbuscar" runat="server" Text="Iniciar la búsqueda"   onclick="btbuscar_Click" OnClientClick="return validateDatesRange('desded','hastad','imagen');" />
         </div>
             <div class="cataresult" >



               <asp:UpdatePanel ID="upresult" runat="server" ChildrenAsTriggers="true">
                     <ContentTemplate>
                        <script type="text/javascript">
                            Sys.Application.add_load(BindFunctions); 
                      </script>
             <div id="xfinder" runat="server" visible="false" >
             <div class="msg-alerta" id="alerta" runat="server" >
               Confirme que los datos sean correctos. En caso de error, favor comuníquese 
               con el Departamento de Planificación a los teléfonos: +593 
               (04) 6006300, 3901700 
             </div>
             <div class="findresult" >
             <div class="booking" >
                  <div class="separator">Documentos encontrados</div>
                 <div class="bokindetalle">
                 <asp:Repeater ID="tablePagination" runat="server" >
                 <HeaderTemplate>
                 <table id="tablasort" cellspacing="1" cellpadding="1" class="tabRepeat">
                 <thead>
                 <tr>
                 <th>#</th>
                 <th>Referencia</th>
                 <th>Nave</th>
                 <th>Viaje In</th>
                 <th>Viaje Out</th>
                 <th>Registrada</th>
                 <th>Acciones</th>
                 </tr>
                 </thead> 
                 <tbody>
                 </HeaderTemplate>
                 <ItemTemplate>
                 <tr class="point" >
                  <td><%#Eval("item")%></td>
                  <td><%#Eval("referencia")%></td>
                  <td><%#Eval("nave")%></td>
                  <td><%#Eval("viajeIn")%></td>
                  <td><%#Eval("ViajeOut")%></td>
                   <td><%#Eval("fecha")%></td>
                  <td>
                   <div class="tcomand">
                       <a href="../atraque/printer.aspx?sid=<%# securetext(Eval("referencia")) %>" class="imprimir" target="_blank">Imprimir</a>
  
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
             </div>
             <div id="pager">
               Registros por página
                  <select class="pagesize">
                  <option selected="selected" value="10">10</option>
                  <option value="20">20</option>
                  </select>
                 <img alt="" src="../shared/imgs/first.gif" class="first"/>
                 <img alt="" src="../shared/imgs/prev.gif" class="prev"/>
                 <input  type="text" class="pagedisplay" size="5px"/>
                 <img alt="" src="../shared/imgs/next.gif" class="next"/>
                 <img alt="" src="../shared/imgs/last.gif" class="last"/>
                 
            </div>
              </div>
               <div id="sinresultado" runat="server" class="msg-info"></div>
                  </ContentTemplate>
                     <Triggers>
                     <asp:AsyncPostBackTrigger ControlID="btbuscar" />
                     </Triggers>
                 </asp:UpdatePanel>
             </div>
      </div>

  </div>
    <script src="../Scripts/pages.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.tablesorter.min.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.tablesorter.pager.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
              $('.datetimepicker').datetimepicker().datetimepicker({ lang: 'es', timepicker: false, closeOnDateSelect: true, format: 'd/m/Y' });
          });
  </script>

<%#Eval("item")%>
</asp:updateprogress>
  </asp:Content>
