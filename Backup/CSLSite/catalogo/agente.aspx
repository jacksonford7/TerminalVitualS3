<%@ Page Language="C#" Title="Catálogo de agentes" AutoEventWireup="true" CodeBehind="agente.aspx.cs" Inherits="CSLSite.agente" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Catálogo de agentes</title>
    <link href="../shared/estilo/catalogos.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/work.css" rel="stylesheet" type="text/css" />
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
             <p class="catalabel" id="senae">Código Senae (Aduana):</p>
             <asp:TextBox ID="txtci" runat="server"  CssClass="catamayusc" 
                 onkeypress="return soloLetras(event,'0123456789abcdefghijklmnopqrstuvwxyzñ',true)" 
                 MaxLength="15" Width="30%" ></asp:TextBox> 
                 
                 
                   <a class="tooltip ctrlxax"  >
             <span class="classic">Marque esta casilla si desea buscar solo personas naturales</span>
             <asp:CheckBox ID="cknatural" runat="server" Text="Persona natural" onclick="changeLabel(this);" /> 
             </a>
              <br />
                         <p class="catalabel" id="agente">Nombres y/o apellidos:</p>
             <asp:TextBox ID="txtname" runat="server"  CssClass="catamayusc" 
                 onkeypress="return soloLetras(event,'abcdefghijklmnñopqrstuvwxyz1234567890')" 
                 MaxLength="50" Width="30%" ></asp:TextBox>  
             <asp:Button ID="find" runat="server" Text="Buscar" onclick="find_Click"  OnClientClick="return initFinder();"/>
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
               Confirme que los datos sean correctos. En caso de error, favor comuníquese 
               con el Departamento de servicio al cliente a los teléfonos: +593 (04) 6006300, 3901700 ext. 4040, 
               en sus horarios hábiles Lunes a Viernes de 8:30 a 19:30
             </div>
             <%-- catalogo de bookings--%>
             <div class="findresult" >
             <div class="booking" >
                  <div class="separator">Agentes / Personas naturales</div>
                 <div class="bokindetalle">
                 <asp:Repeater ID="tablePagination" runat="server" >
                 <HeaderTemplate>
                 <table id="tablasort" cellspacing="1" cellpadding="1" class="tabRepeat">
                 <thead>
                 <tr>
                 <th>No.</th>
                 <th>Documento No.</th>
                 <th>Descripción</th>
                 <th>Acciones</th>
                 </tr>
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
              <div id="sinresultado" runat="server" class="msg-info" >
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

   <script type="text/javascript" >
       function setObject(row) {
           var celColect = row.getElementsByTagName('td');
          var agente = {
              item: celColect[0].textContent,
              codigo: celColect[1].textContent,
              descripcion: celColect[2].textContent
              };
            if (window.opener != null) {
                   window.opener.document.getElementById('valexpor').innerHTML = '';
                   window.opener.document.getElementById('agente').textContent = agente.descripcion;
                   window.opener.popupCallback(agente.codigo, 'caeid');
            }
            self.close();
      }

      function initFinder() {
          if (document.getElementById('txtci').value.trim().length <= 0 && document.getElementById('txtname').value.trim().length <= 0) {
              alert('Por favor escriba una o varias \nletras del nombre y|o documento');
              return false;
          }
          document.getElementById('imagen').innerHTML = '<img alt="" src="../shared/imgs/loader.gif">';
      }

      function changeLabel(control) {
          if (control.checked) {
              document.getElementById('senae').textContent = 'Número CI | Pasaporte:';
               return;
          }
           document.getElementById('senae').textContent = 'Código Senae (Aduana):';
          return;
      }
   </script>
    <script src="../Scripts/jquery-1.7.1.min.js" type="text/javascript"></script>
    <script src="../Scripts/pages.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.tablesorter.min.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.tablesorter.pager.js" type="text/javascript"></script>
</body>
</html>
