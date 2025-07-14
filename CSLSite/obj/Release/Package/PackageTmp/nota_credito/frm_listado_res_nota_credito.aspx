<%@ Page Title="Listado Resumido de Nota de Crédito" Language="C#" MasterPageFile="~/site.Master" AutoEventWireup="true" 
    CodeBehind="frm_listado_res_nota_credito.aspx.cs" Inherits="CSLSite.frm_listado_res_nota_credito" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="placehead" runat="server">
    <link href="../shared/estilo/tables.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/aisv-general.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/work.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/catalogos.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/info.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/opc_tables.css" rel="stylesheet" />

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
    <asp:ToolkitScriptManager ID="tkcal" EnableScriptGlobalization="true" runat="server"> </asp:ToolkitScriptManager>
     
    <div>
   <i class="ico-titulo-1"></i><h2>Listado Resumido de Nota de Crédito</h2>  <br /> 
 <%--  <i class="ico-titulo-2"></i><h1>Podra visualizar </h1><br />--%>
 </div>
  <div class="seccion">
       <div class="accion">

         <table class="xcontroles" cellspacing="0" cellpadding="1">

        <tr><th class="bt-bottom bt-right  bt-left bt-top" colspan="5">CRITERIOS DE BÚSQUEDA</th></tr>
         <tr>
         <td  class="bt-bottom bt-left bt-right" >Estado</td>
         <td class="bt-bottom" >
             <asp:DropDownList ID="CboEstado" runat="server" Width="150px" AutoPostBack="False"
                                            Height="28px"  DataTextField='name' DataValueField='id'  
                                            Font-Size="Small" 
                                            >
                                        </asp:DropDownList>
          </td>
          
         
         <td class="bt-bottom bt-right validacion " colspan="3"><span id="valran" class="validacion"> * obligatorio</span></td>
         </tr>
           <tr>
         <td  class="bt-bottom bt-left bt-right" >Motivo/Concepto:</td>
         <td class="bt-bottom" >
           <asp:DropDownList ID="CboConcepto" runat="server" Width="250px" AutoPostBack="False"
                                            Height="28px"  DataTextField='description' DataValueField='id_concept'  
                                            Font-Size="Small" > </asp:DropDownList> 
          </td>
          
         
         <td class="bt-bottom bt-right validacion " colspan="3"><span id="valran2" class="validacion"> * obligatorio</span></td>
         </tr>

         <tr>
         <td class="bt-bottom  bt-right bt-left">Desde:</td>
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
         <td class="bt-bottom bt-right validacion " colspan ="2">
         <span id="valdate" class="validacion"> * obligatorio</span>
         </td>
         </tr>

         </table>
            
         <div class="botonera">
          <span id="imagen"></span>
             <asp:Button ID="btnPrueba" runat="server" Text="Bajar"   onclick="btnprueba_Click" />
             <asp:Button ID="btbuscar" runat="server" Text="Iniciar la búsqueda"   onclick="btbuscar_Click" OnClientClick="return validateDatesRange('desded','hastad','imagen');" />
         </div>
         
               <div class="cataresult" >



               <asp:UpdatePanel ID="upresult" runat="server" ChildrenAsTriggers="true" >
                     <ContentTemplate>
                        <script type="text/javascript">
                            Sys.Application.add_load(BindFunctions); 
                      </script>
             <div id="xfinder" runat="server" visible="false" >
             
             <div class="findresult" >
             <div class="booking" >
                  <div class="separator">Documentos encontrados</div>
                 <div class="bokindetalle">
                 <asp:Repeater ID="tablePagination" runat="server" 
                         onitemcommand="tablePagination_ItemCommand" >
                 <HeaderTemplate>
                 <table id="tablasort" cellspacing="1" cellpadding="1" class="tabRepeat">
                 <thead>
                 <tr>
                 <th>No.</th>
                 <th>Fecha</th>
                 <th>Concepto<br>Archivo</th>
                 <th>Factura</th>
                 <th>Cliente</th>
                 <th>Total $</th>
                 <th>Aprobados</th>
                 <th>Pendientes</th>
                 <th>Nivel Actual</th>
                 <th>Estado</th>
                
                 <th colspan="3">Acciones</th>
                 </tr>
                 </thead> 
                 <tbody>
                 </HeaderTemplate>
                 <ItemTemplate>
                 <tr class="point" >
                  <td><%#Eval("nc_id")%></td>
                  <td><%#Eval("nc_date_text")%></td>
                  <td><%#Eval("description")%><br>
                      <a href="../nota_credito/lookup_download_archivo.aspx?nc_id=<%#securetext(Eval("nc_id")) %>"  target="_blank" >Descargar Archivos</a>
                     </td>
                  <td><%#Eval("num_factura")%></td>
                  <td><%#Eval("nombre_cliente")%></td>
                  <td><%#Eval("nc_total")%></td>
                  <td><%#Eval("usuarios_aprobados")%></td>
                  <td><%#Eval("usuarios_pendientes")%></td>
                  <td><%#Eval("level_text")%></td>
                  <td><%#Eval("estado")%></td>          
                  <td> <a href="../nota_credito/nota_credito_preview.aspx?nc_id=<%#securetext(Eval("nc_id")) %>" class="imprimir" target="_blank">Imprimir</a></td>
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
                     <%--<asp:AsyncPostBackTrigger ControlID="BtnProcesar" />--%>
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

   <script type="text/javascript" >

           function GolinkArchivo(pnc_id) {

             window.open('../nota_credito/lookup_download_archivo.aspx?nc_id="'+pnc_id+'"', 'name', 'width=850,height=480,menubar=NO,scrollbars=NO,resizable=NO,toolbars=NO,Titlebar=NO,status=no,help=no,minimize=no,unadorned=on,maximize=no');
          
        }
 </script>
  <%--<asp:updateprogress associatedupdatepanelid="upresult"
    id="updateProgress" runat="server">
    <progresstemplate>
        <div id="progressBackgroundFilter"></div>
        <div id="processMessage"> Estamos procesando la tarea que solicitó, por favor 
            espere...<br />
             <img alt="Loading" src="../shared/imgs/loader.gif" style="margin:0 auto;" />
        </div>
    </progresstemplate>
</asp:updateprogress>--%>
  </asp:Content>
