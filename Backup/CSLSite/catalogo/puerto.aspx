﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="puerto.aspx.cs" Inherits="CSLSite.puerto" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Catálogo de Puertos</title>
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
             <p class="catalabel">Nombre de la ciudad destino:</p>
             <asp:TextBox ID="txtfinder" 
                 runat="server"  
                 CssClass="catamayusc" 
                 onkeypress="return soloLetras(event,'abcdefghijklmnñopqrstuvwxyz1234567890-*.')" 
                 MaxLength="30" Width="40%"
                 onkeyup="msgfinder(this,'valintro');"
                  ></asp:TextBox>  
             <asp:Button ID="find" runat="server" Text="Buscar" onclick="find_Click" OnClientClick="return initFinder();" />
             <span id="imagen"></span>
            
             
          </div>
         <p class="catavalida" id="valintro">Escriba una o varias letras del nombre y pulse buscar</p>
         </div>
         <div class="cataresult" >
             <asp:UpdatePanel ID="upresult" runat="server"  >
             <ContentTemplate>
             <script type="text/javascript">Sys.Application.add_load(BindFunctions); </script>
               <div id="xfinder" runat="server" visible="false" >
             <div class="msg-alerta" id="alerta" >
               Confirme que los datos sean correctos. En caso de error, favor comuníquese 
               con el Departamento de Planificación a los teléfonos: +593 
               (04) 6006300, 3901700 ext. 1132 y 1133 . 
             </div>
             <%-- catalogo de bookings--%>
             <div class="findresult" >
             <div class="booking" >
                  <div class="separator">Puertos encontrados</div>
                 <div class="bokindetalle">
                 <asp:Repeater ID="tablePagination" runat="server" >
                 <HeaderTemplate>
                 <table id="tablasort" cellspacing="1" cellpadding="1" class="tabRepeat">
                 <thead>
                 <tr>
                 <th>No.</th>
                 <th>Código</th>
                 <th>Descripción</th>
                 <th>Acciones</th></tr>
                 </thead> 
                 <tbody>
                 </HeaderTemplate>
                 <ItemTemplate>
                 <tr class="point" onclick="setBooking(this);">
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
            No se encontraron resultados, asegúrese que haya escrito correctamente el nombre de la ciudad del puerto destino
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
       function setBooking(row) {

           var valor = window.opener.sw;    
           var celColect = row.getElementsByTagName('td');
              var puerto = {
                  item: celColect[0].textContent,
                  codigo: celColect[1].textContent,
                  descripcion: celColect[2].textContent,
                  opcion: valor
              };
              if (window.opener != null) {
                  window.opener.popupCallback(puerto, 'puerto');
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
