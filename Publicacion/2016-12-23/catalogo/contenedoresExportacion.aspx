<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="contenedoresExportacion.aspx.cs" Inherits="CSLSite.contenedoresExportacion" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Contenedores</title>
    <link href="../shared/estilo/catalogos.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/work.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/pages.js" type="text/javascript"></script>
    <link href="../shared/estilo/aisv-general.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/tables.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        function BindFunctions() {
            document.getElementById('imagen').innerHTML = '';
            $(document).ready(function () {
                $('#tablasort').tablesorter({ cancelSelection: true, cssAsc: "ascendente", cssDesc: "descendente", headers: { 5: { sorter: false }, 6: { sorter: false}} }).tablesorterPager({ container: $('#pager'), positionFixed: false, pagesize: 10 });
            });
        }
    </script>

</head>
<body>
 <noscript><meta http-equiv="refresh" content="0; url=sinsoporte.htm" /> </noscript>
    <form id="bookingfrm" runat="server">
    <asp:ToolkitScriptManager ID="tkscata" runat="server"></asp:ToolkitScriptManager>
    <div class="catabody">
       <div class="catawrap" >
         <div class="catabuscar">
         <div class="catacapa">
             <p class="catalabel">Contenedor:</p>
             <asp:TextBox ID="txtfinder" runat="server" Width="40%" MaxLength="11" CssClass="mayusc"
              onkeypress="return soloLetras(event,'abcdefghijklmnñopqrstuvwxyz1234567890')"
              onBlur="checkDC(this,'valintro',true);" EnableViewState="False"
              placeholder="Contenedor"
            
             ></asp:TextBox>

             <%--<asp:TextBox ID="txtfinder" 
                 runat="server"  
                 CssClass="catamayusc" 
                 onkeypress="return soloLetras(event,'abcdefghijklmnñopqrstuvwxyz1234567890')" 
                 MaxLength="30" Width="40%"
                 onkeyup="msgfinder(this,'valintro');"
                  ></asp:TextBox>  --%>
             <asp:Button ID="find" runat="server" Text="Buscar" onclick="find_Click" OnClientClick="return initFinder();" />
             <span id="imagen"></span>
          </div>
         <p class="catavalida" id="valintro">Escriba una o varias letras del código y pulse buscar</p>
         </div>
         <div class="cataresult" >
             <asp:UpdatePanel ID="upresult" runat="server"  >
             <ContentTemplate>
             <script type="text/javascript">                 Sys.Application.add_load(BindFunctions); </script>
               <div id="xfinder" runat="server" visible="false" >
             <div class="msg-alerta" id="alerta"  >
                Esta(s) transmisión(es) tienen un costo, el cual será facturado a su representada y deberá ser cancelado antes del embarque de su contenedor.
             </div>
             <%-- catalogo de bookings--%>
             <div class="findresult" >
             <div class="booking" >
                  <div class="separator">Contenedores</div>
                 <div class="bokindetalle">
                 <asp:Repeater ID="tablePagination" runat="server" >
                 <HeaderTemplate>
                 <table id="tablasort" cellspacing="1" cellpadding="1" class="tabRepeat">
                 <thead>
                 <tr>
                 <th align="center">No.</th>
                 <th align="center">Código</th>
                 <th align="center">Booking</th>
                 <th align="center" style="display: none">Tipo Contenedor</th>
                 <th align="center">DAE</th>
                 <th align="center" style="display: none">S1</th>
                 <th align="center" style="display: none">S2</th>
                 <th align="center" style="display: none">S3</th>
                 <th align="center" style="display: none">S4</th>
                 <th align="center" style="display: none">Carga</th>
                 <th align="center" style="display: none">Peso</th>
                 <th align="center" style="display: none">AISV</th>
                 <th align="center" style="display: none">iso</th>
                 <th align="center">Acciones</th></tr>
                 </thead> 
                 <tbody>
                 </HeaderTemplate>
                 <ItemTemplate>
                 <tr class="point" onclick="setContenedor(this);">
                  <td><%#Eval("idConsecutivo")%></td>
                  <td><%#Eval("nombrecont")%></td>
                  <td><%#Eval("booking")%></td>
                  <td style="display:none"><%#Eval("tipoContenedor")%></td>
                  <td><%#Eval("dae")%></td>
                   <td style="display:none"><%#Eval("sello1")%></td>
                    <td style="display:none"><%#Eval("sello2")%></td>
                     <td style="display:none"><%#Eval("sello3")%></td>
                      <td style="display:none"><%#Eval("sello4")%></td>
                       <td style="display:none"><%#Eval("codigo_carga")%></td>
                        <td style="display:none"><%#Eval("pesoContenedor")%></td>
                        <td style="display:none"><%#Eval("aisv")%></td>
                        <td style="display:none"><%#Eval("iso")%></td>
                  <td>
                     <a href="#" >Elegir</a>
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
               <div id="sinresultado" runat="server" class="msg-info">
              No se encontraron resultados,  asegurese que ha escrito correctamente el nombre/referencia del contenedor
              </div>
             </ContentTemplate>
             <Triggers>
             <asp:AsyncPostBackTrigger ControlID="find" />
             </Triggers>
             </asp:UpdatePanel>
       </div>
      </div>
      </div>
    <input id="json_object" type="hidden" />
    </form>
     <script src="../Scripts/jquery-1.7.1.min.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.tablesorter.min.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.tablesorter.pager.js" type="text/javascript"></script>
   <script type="text/javascript" >
       function setContenedor(row) {
           var celColect = row.getElementsByTagName('td');
           var contenedor = {
               item: celColect[0].textContent,
               codigo: celColect[1].textContent,
               booking: celColect[2].textContent,
               tipoContenedor: celColect[3].textContent,
               dae: celColect[4].textContent,
               sello1: celColect[5].textContent,
               sello2: celColect[6].textContent,
               sello3: celColect[7].textContent,
               sello4: celColect[8].textContent,
               codigoCarga: celColect[9].textContent,
               peso: celColect[10].textContent,
               aisv: celColect[11].textContent,
               iso: celColect[12].textContent
           };

           if (window.opener != null) {
               window.opener.popupCallback(contenedor);
           }
           self.close();
       }
       function msgfinder(control, expresa) {
           if (control.value.trim().length <= 0) {
               this.document.getElementById(expresa).textContent = 'Escriba una o varias letras del nombre/código y pulse buscar';
               return;
           }
           this.document.getElementById(expresa).textContent = 'Se buscará [' + control.value.toUpperCase() + '], presione el botón';
       }
       function initFinder() {
           if (document.getElementById('txtfinder').value.trim().length <= 0) {
               alert('Escriba una o varias letras para iniciar la búsqueda');
               return false;
           }
           document.getElementById('imagen').innerHTML = '<img alt="" src="../shared/imgs/loader.gif">';
       }
   </script>
</body>

</html>