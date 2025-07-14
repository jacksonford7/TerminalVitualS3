<%@ Page  Title="Reporte de operaciones" Language="C#" MasterPageFile="~/site.Master" AutoEventWireup="true" CodeBehind="preimpresion.aspx.cs" Inherits="CSLSite.preimpresion" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<%@ MasterType VirtualPath="~/site.Master" %>
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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="placebody" runat="server">
 <input id="zonaid" type="hidden" value="103" />
 <input id="usuario" type="hidden" runat="server" clientidmode="Static" />
    <asp:ToolkitScriptManager ID="tkcal" EnableScriptGlobalization="true" runat="server"> </asp:ToolkitScriptManager>
    <div>
   <i class="ico-titulo-1"></i><h2>Servicios a clientes de CGSA </h2>  <br /> 
   <i class="ico-titulo-2"></i><h1>Consulta e impresión de avisos de contenedores exportación/vacíos</h1><br />
 </div>
  <div class="seccion">
       <div class="accion">
            <table class="xcontroles" cellspacing="0" cellpadding="1">
        <tr><th class="bt-bottom bt-right  bt-left bt-top" colspan="5"> Datos de las 
            unidades buscadas</th></tr>
         <tr>
         <td  class="bt-bottom bt-left bt-right" >Referencia :</td>
         <td class="bt-bottom" >
             <asp:TextBox ID="refer" runat="server" Width="120px" MaxLength="15" 
             onkeypress="return soloLetras(event,'01234567890abcdefghijklmnopqrstuvwxyz',true)"></asp:TextBox>
          </td>
           <td class="bt-bottom" >Contenedor No. :</td>
          <td class="bt-bottom" >
             <asp:TextBox ID="cntrn" runat="server" Width="120px" MaxLength="15"  
                  onkeypress="return soloLetras(event,'01234567890abcdefghijklmnopqrstuvwxyz',true)">
                  </asp:TextBox>
          </td>
         <td class="bt-bottom bt-right validacion "><span id="valran" class="opcional"> * opcionales</span></td>
         </tr>
          <tr>
         <td class="bt-bottom  bt-right bt-left">Número de booking:</td>
          <td class="bt-bottom bt-right " colspan="2" >
               <asp:TextBox ID="docnum" runat="server" Width="160px" MaxLength="20"  
                   onkeypress="return soloLetras(event,'01234567890abcdefghijklmnopqrstuvwxyz_-',true)"></asp:TextBox>
          </td>
            <td class="bt-bottom"></td>
             <td class="bt-bottom  bt-right "><span id="valdae" class="opcional"> * opcionales</span></td>
         </tr>
         <tr>
         <td class="bt-bottom  bt-right bt-left">Preaviso generados desde el día:</td>
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
         <span id="imagen"> </span>
             <asp:Button ID="btbuscar" runat="server" Text="Generar reporte"  
                 OnClientClick="return validateDatesRange('desded','hastad','imagen')" 
                 onclick="btbuscar_Click" />
         </div>
             <div class="cataresult" >
               <asp:UpdatePanel ID="upresult" runat="server" ChildrenAsTriggers="true"  >
                     <ContentTemplate>
                        <script type="text/javascript">
                            Sys.Application.add_load(BindFunctions); 
                      </script>
                
                <div id="xfinder" runat="server" visible="false" clientidmode="Static" >
                <div id="postable" runat="server">

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
                  <input id="btprinter" type="button" runat="server" value="Imprimir" onclick="window.open('preaviso/print','Impresion','width=850,height=500,scrollbars=yes');" />
            </div>
              </div>
                <span id="htmlstring"  runat="server" clientidmode="Static" style=" width:1px; height:1px; max-width:1px; max-height:1px; overflow:hidden; visibility:hidden; display:inline; z-index:4000; float:right;"   ></span>
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

        function loader() {
            document.getElementById('htmlstring').value = '';
            document.getElementById('imagen').innerHTML='<img alt="" src="../shared/imgs/loader.gif">'
            return true;
        }
  </script>
</asp:Content>
