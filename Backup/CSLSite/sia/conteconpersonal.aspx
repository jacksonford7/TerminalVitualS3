<%@ Page Title="Reporte de Personas en Terminal" Language="C#" MasterPageFile="~/site.Master" AutoEventWireup="true"
 CodeBehind="conteconpersonal.aspx.cs" Inherits="CSLSite.conteconpersonal" %>
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
<input id="zonaid" type="hidden" value="1103" />
    <asp:ToolkitScriptManager ID="tkcal" EnableScriptGlobalization="true" runat="server"> </asp:ToolkitScriptManager>
    <div>
   <i class="ico-titulo-1"></i><h2>Seguridad Industrial </h2>  <br /> 
   <i class="ico-titulo-2"></i><h1>Reporte de Personas dentro de la Terminal</h1><br />
 </div>
  <div class="seccion">
       <div class="accion">
            <table class="xcontroles" cellspacing="0" cellpadding="1">
        <tr><th class="bt-bottom bt-right  bt-left bt-top" colspan="5"> Datos para el corte</th></tr>
         <tr class="nover">
         <td class="bt-bottom  bt-right bt-left">Corte desde:</td>
         <td class="bt-bottom">
            <asp:TextBox ID="desded" runat="server" Width="120px" MaxLength="10" Enabled="false"
             
                  ClientIDMode="Static"></asp:TextBox>

             </td>
          <td class="bt-bottom" >Hasta:</td>
          <td class="bt-bottom " >
             <asp:TextBox ID="hastad" runat="server" ClientIDMode="Static" 
             Width="120px" MaxLength="15"  Enabled="false"
            
                  ></asp:TextBox>
   
          </td>
         <td class="bt-bottom bt-right validacion ">
         <span id="valdate" class="validacion"> </span>
         </td>
         </tr>


                  <tr>
         <td class="bt-bottom  bt-right bt-left">Apellidos:</td>
         <td class="bt-bottom">
            <asp:TextBox ID="txapellidos" runat="server" Width="120px" MaxLength="200" 
             onkeypress="return soloLetras(event,'abcdefghijklmnopqrstuvwxyz ')" 
                  ClientIDMode="Static"></asp:TextBox>

             </td>
          <td class="bt-bottom" >Cedula:</td>
          <td class="bt-bottom " >
             <asp:TextBox ID="tcedula" runat="server" ClientIDMode="Static" Width="120px" MaxLength="12" 
             
             onkeypress="return soloLetras(event,'01234567890AASSCC-')"   ></asp:TextBox>
   
          </td>
         <td class="bt-bottom bt-right validacion ">
         <span id="Span1" class="validacion"> </span>
         </td>
         </tr>




         </table>
         <div class="botonera">
          <span id="imagen"></span>
             <asp:Button ID="btbuscar" runat="server" Text="Iniciar la búsqueda"   
             onclick="btbuscar_Click"  />
         </div>
             <div class="cataresult" >



               <asp:UpdatePanel ID="upresult" runat="server" >
                     <ContentTemplate>



                        <script type="text/javascript">
                            Sys.Application.add_load(BindFunctions); 
                      </script>
             <div id="xfinder" runat="server" visible="false" >
             <div class="msg-info" id="alerta" runat="server" >
              Para ver todos los detalles en una lista exporte y verifique la información en excel.
             </div>
             <div class="findresult" >
             <div class="booking" >
                  <div class="separator">Documentos encontrados</div>
                 <div class="bokindetalle">
                 <asp:Repeater ID="tablePagination" runat="server" 
                          >
                 <HeaderTemplate>
                 <table id="tablasort" cellspacing="1" cellpadding="1" class="tabRepeat">
                 <thead>
                 <tr>
                 <th>#</th>
                 <th>CEDULA</th>
                 <th>EMPLEADO</th>
                 <th>CORREOS</th>
                 <th>TELEFONO</th>
                 </tr>
                 </thead> 
                 <tbody>
                 </HeaderTemplate>
                 <ItemTemplate>
                 <tr class="point" >
                  <td><%#Eval("item")%></td>
                  <td><%#Eval("cedula")%></td>
                  <td><%#Eval("empleado")%></td>
                  <td><%#Eval("correos")%></td>
                  <td>
                    <a class="xinfo" >
                    <span class="xclass">
                      <%#Eval("direccion")%>
                     </span>
                        <%#Eval("telefonos")%>
                    </a>
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
                 <input clientidmode="Static" id="dataexport" onclick="getTable('resultado');" type="button" value="Exportar" runat="server" />
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

 
<%--    <asp:updateprogress    id="updateProgress" runat="server">
    <progresstemplate>
        <div id="progressBackgroundFilter"></div>
        <div id="processMessage"> Por favor espere, este proceso puede tardar algunos minutos..
            espere...<br />
             <img alt="Loading" src="../shared/imgs/loader.gif" style="margin:0 auto;" />
        </div>
    </progresstemplate>
</asp:updateprogress>--%>

  </asp:Content>
