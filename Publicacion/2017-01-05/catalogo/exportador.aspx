<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="exportador.aspx.cs" Inherits="CSLSite.exportador" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Catálogo de exportadores</title>
    <link href="../shared/estilo/catalogos.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/work.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/pages.js" type="text/javascript"></script>
    <link href="../shared/estilo/aisv-general.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/tables.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
       function BindFunctions(){
           $(document).ready(function () {
               document.getElementById('imagen').innerHTML = '';
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
             <p class="catalabel">No. CI|RUC|Pasaporte:</p>
             <asp:TextBox ID="txtci" runat="server"  CssClass="catamayusc" 
                 onkeypress="return soloLetras(event,'0123456789')" 
                 MaxLength="15" Width="30%" ></asp:TextBox>  <br />
                         <p class="catalabel">Nombre del exportador:</p>
             <asp:TextBox ID="txtname" runat="server"  CssClass="catamayusc" 
                 onkeypress="return soloLetras(event,'abcdefghijklmnñopqrstuvwxyz1234567890')" 
                 MaxLength="50" Width="30%" ></asp:TextBox>  
             <asp:Button ID="find" runat="server" Text="Buscar" onclick="find_Click" OnClientClick="return initFinder();" />
             <span id="imagen"></span>
          </div>
         <p class="catavalida">Escriba una o varias letras del nombre y|o documento pulse buscar</p>
         </div>
         <div class="cataresult" >
             <asp:UpdatePanel ID="upresult" runat="server"  >
             <ContentTemplate>
             <script type="text/javascript">Sys.Application.add_load(BindFunctions); </script>
             <div id="xfinder" runat="server" visible="false" >
             <div class="msg-alerta" id="alerta" >
                  Confirme que los datos sean correctos. En caso de error, favor comuníquese con 
                 el Departamento de Planificación a los teléfonos: +593 (04) 6006300, 3901700 
                 ext. 4039, 4040, 4060. 
             </div>
             <div class="findresult" >
             <div class="booking" >
                  <div class="separator">Exportadores registrados</div>
                 <div class="bokindetalle">
                 <asp:Repeater ID="tablePagination" runat="server" >
                 <HeaderTemplate>
                 <table id="tablasort" cellspacing="1" cellpadding="1" class="tabRepeat">
                 <thead>
                 <tr>
                 <th>No.</th>
                 <th>Documento No.</th>
                 <th>Descripción</th>
                 <th>Acciones</th></tr>
                 </thead> 
                 <tbody>
                 </HeaderTemplate>
                 <ItemTemplate>
                 <tr class="point" onclick="setObject(this);">
                  <td><%#Eval("number")%></td>
                  <td><%#Eval("codigo")%></td>
                  <td><%#Eval("descripcion")%></td>
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
              No se encontraron resultados, 
              asegurese que ha escrito correctamente el número/nombre
              buscado  
              </div>
             </ContentTemplate>
             <Triggers>
             <asp:AsyncPostBackTrigger ControlID="find" />
             </Triggers>
             </asp:UpdatePanel>
       </div>
      </div>
      </div>
    </form>
     <script src="../Scripts/jquery-1.7.1.min.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.tablesorter.min.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.tablesorter.pager.js" type="text/javascript"></script>
   <script type="text/javascript" >
       function setObject(row) {
           var celColect = row.getElementsByTagName('td');
          var exportador = {
              item: celColect[0].textContent,
              codigo: celColect[1].textContent,
              descripcion: celColect[2].textContent
              };
          if (window.opener != null) {
              window.opener.document.getElementById('nomexpo').textContent = exportador.descripcion;
              window.opener.document.getElementById('numexpo').textContent = exportador.codigo;
              window.opener.popupCallback(exportador.codigo, 'numexport');
          }
           self.close();
        }
        function initFinder() {
            if (this.document.getElementById('txtci').value.trim().length <= 0 && document.getElementById('txtname').value.trim().length <= 0) {
                alert('Por favor escriba una o varias \nletras del nombre y|o documento');
                return false;
            }
            document.getElementById('imagen').innerHTML = '<img alt="" src="../shared/imgs/loader.gif">';
        }
   </script>
</body>
</html>
