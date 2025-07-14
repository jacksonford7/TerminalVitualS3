<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="consultaContenedoresAnalista.aspx.cs" Inherits="CSLSite.consultaContenedoresAnalista" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Contenedores</title>
    <link href="../shared/estilo/catalogos.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/work.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/pages.js" type="text/javascript"></script>
    <link href="../shared/estilo/aisv-general.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/centroSolicitud.css" rel="stylesheet" type="text/css" />
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
    <div class="catabody" style="width: 95%!important; height: 98%!important;">
       <div class="catawrap" >
         <div class="catabuscar">
         <div class="catacapa">
         <table style="width: 100%">
            <tr>
                <td>
                    <asp:Label ID="lblCodigoSol" Text="No. Solicitud:" runat="server"></asp:Label>
                </td>                    
                <td>
                    <asp:Label ID="lblCodigoSolicitud" Text="" runat="server"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="lblTrafico" Text="Tráfico:" runat="server"></asp:Label>
                </td>                    
                <td>
                    <asp:Label ID="lblTipoTrafico" Text="" runat="server"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="lblBooking" Text="Num. Booking:" runat="server"></asp:Label>
                </td>                    
                <td>
                    <asp:Label ID="lblNumBooking" Text="" runat="server"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="lblCarga" Text="Carga:" runat="server"></asp:Label>
                </td>                    
                <td>
                    <asp:Label ID="lblNumCarga" Text="" runat="server"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="lblEstado" Text="Estado:" runat="server"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="dpestados" runat="server" Width="200px"  >
                         <asp:ListItem Value="0">* Seleccione estados *</asp:ListItem>
                    </asp:DropDownList>
                </td>                
            </tr>
            <tr>
                <td colspan="10" align="center">
                    <asp:TextBox TextMode="MultiLine" MaxLength="300" id="txtobservacion" required="required" runat="server" Width="500px" Height="60px" Text=""/>
                </td>                
            </tr>
            <tr>
                <td colspan="10" align="center">
                    <asp:Button ID="save" runat="server" Text="Guardar" onclick="save_Click"/>
                    <span id="imagen"></span>
                </td>
            </tr>
         </table>
                
                
             <%--<p class="catalabel">Contenedor:</p>
             <asp:TextBox ID="txtfinder" runat="server" Width="40%" MaxLength="11" CssClass="mayusc"
              onkeypress="return soloLetras(event,'abcdefghijklmnñopqrstuvwxyz1234567890')"
              onBlur="checkDC(this,'valintro',true);" EnableViewState="False"
              placeholder="Contenedor"
            
             ></asp:TextBox>--%>

             <%--<asp:TextBox ID="txtfinder" 
                 runat="server"  
                 CssClass="catamayusc" 
                 onkeypress="return soloLetras(event,'abcdefghijklmnñopqrstuvwxyz1234567890')" 
                 MaxLength="30" Width="40%"
                 onkeyup="msgfinder(this,'valintro');"
                  ></asp:TextBox>  --%>
             
          </div>         
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
             <%-- catalogo de bookings--%>
             <div class="findresult" >
             <div class="booking" >
                  <div class="separator">Contenedores</div>
                 <div class="bokindetalle">
                 <asp:Repeater ID="tablePagination" runat="server" OnItemDataBound="tablePagination_ItemDataBound">
                 <HeaderTemplate>
                 <table id="tablasort" cellspacing="1" cellpadding="1" class="tabRepeat">
                 <thead>
                 <tr>
                 <th align="center">No.</th>
                 <th align="center">Contenedor</th>                 
                 <th align="center">Procesado</th>
                 <th align="center" id="tipoVerificacionTH" runat="server" visible="true">Tipo Verificación</th>
                 <th align="center">Observación</th>
                 <th align="center">Aplica servicio</th>
                 </tr>
                 </thead> 
                 <tbody>
                 </HeaderTemplate>
                 <ItemTemplate>
                 <tr class="point">
                  <td><asp:Label ID="idDetalleSolicitud" Text='<%#Eval("idDetalleSolicitud")%>' runat="server"></asp:Label></td>
                  <td><%#Eval("descripcionContenedor")%></td>
                  <td><%#Eval("confirmado")%></td>
                  <td id="tipoVerificacionTD" runat="server" visible="true"><%#Eval("tipoVerificacion")%></td>
                  <td><%#Eval("observacion")%></td>
                  <td>
                    <asp:DropDownList ID="servicioddl"  runat="server" Visible="false">
                        <asp:ListItem Text="Si" Value="SI"></asp:ListItem>
                        <asp:ListItem Text="No" Value="NO"></asp:ListItem>
                    </asp:DropDownList>
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
             <asp:AsyncPostBackTrigger ControlID="save" />
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
                  tipoContenedor: celColect[3].textContent
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
