<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="booking.aspx.cs" Inherits="CSLSite.booking" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Catálogo de bookings</title>
    <link href="../shared/estilo/catalogos.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/work.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/pages.js" type="text/javascript"></script>
    <link href="../shared/estilo/aisv-general.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/tables.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        function BindFunctions() {
            document.getElementById('imagen').innerHTML = '';
           $(document).ready(function () {
               $('#tablasort').tablesorter({ cancelSelection: true, cssAsc: "ascendente", cssDesc: "descendente", headers: { 5: { sorter: false }, 6: { sorter: false}} }).tablesorterPager({ container: $('#pager'), positionFixed: false, pagesize: 4 });
           });
        }
    </script>
</head>
<body>
 <noscript><meta http-equiv="refresh" content="0; url=sinsoporte.htm" /></noscript>
    <form id="bookingfrm" runat="server">
    <asp:ToolkitScriptManager ID="tkscata" runat="server"></asp:ToolkitScriptManager>
    <div class="catabody">
       <div class="catawrap" >
         <div class="catabuscar">
         <div class="catacapa">
             <p class="catalabel">Numero de booking:</p>
             <asp:TextBox ID="txtfinder" runat="server"  CssClass="catamayusc" 
                 onkeypress="return soloLetras(event,'abcdefghijklmnñopqrstuvwxyz1234567890-_')" 
                 MaxLength="30" Width="40%" onkeyup="msgfinder(this,'valintro','Escriba un número de booking y pulse buscar');" ></asp:TextBox>  
             <asp:Button ID="find" runat="server" Text="Buscar" onclick="find_Click" OnClientClick="return initFinder();" />
             <span id="imagen"></span>
          </div>
         <p class="catavalida" id="valintro" runat="server">Escriba un número de booking y pulse buscar</p>
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
              <div class="separator">Booking</div>
             <div class="booking" >
                 <div class="bokincab">
                   <table class="catable" cellpadding="0" cellspacing="0">
                    <tr>
                    <td ><p class=" labelp bt-bottom bt-right">Número:</p></td>
                    <td class="bt-bottom bt-right" ><span id="numero" runat="server" class="datasee">REM00006689</span></td>
                    <td ><p class=" labelp  bt-bottom bt-right">FreightKind:</p></td>
                    <td class="bt-bottom " ><span id="fk" runat="server" class="datasee">FCL</span></td>
                    </tr>
                    <tr>
                    <td ><p class=" labelp bt-bottom bt-right">Referencia:</p></td>
                    <td colspan="1" class="bt-bottom bt-right">
                    <span class="datasee" id="referencia" runat="server"></span>
                    </td>
                    <td  ><p class=" labelp  bt-bottom bt-right">IMO:</p></td>
                    <td class="bt-bottom"><span class="datasee" id="imo" runat="server"></span></td>
                    </tr>
                    <tr>
                    <td ><p class=" labelp bt-bottom bt-right">Nave:</p></td>
                    <td colspan="1" class="bt-bottom bt-right " >
                    <span class="datasee" id="nave" runat="server"></span>
                    </td>
                    <td  ><p class=" labelp  bt-bottom bt-right">Viaje:</p></td>
                    <td class="bt-bottom">
                      <span class="datasee" id="viaje" runat="server"></span>
                    </td>
                    </tr>
                    <tr>
                    <td colspan="1"><p class=" labelp bt-bottom bt-right">Fecha estimada de atraque [ETA]:</p></td>
                    <td colspan="1" class="bt-bottom bt-right"><span id="eta" runat="server" class="datasee">
                        </span></td>
                     <td  ><p class=" labelp bt-right bt-bottom">Reefer:</p></td>
                    <td class="bt-bottom"><span class="datasee" id="refer" runat="server">NO</span></td>
                    </tr>
                    <tr>
                    <td colspan="1"><p class="labelp bt-bottom bt-right">Fecha límite [CutOff]:</p></td>
                    <td colspan="1" class="bt-bottom bt-right"><span id="cutoff" runat="server" class="datasee">
                        01/01/2014 10:00</span></td>
                    <td  ><p class=" labelp bt-right bt-bottom">Puerto de descarga:</p></td>
                    <td class="bt-bottom"><span class="datasee" id="pod" runat="server">BEANR</span></td>
                    </tr>
                    <tr>
                    <td ><p class=" labelp bt-bottom bt-right">Producto declarado:</p></td>
                    <td colspan="1" class="bt-bottom bt-right" >
                    <span class="datasee" id="comoditi" runat="server">Producto DUMMY</span>
                    </td>
                    <td><p class="labelp bt-bottom bt-right">Puerto descarga final:</p></td>
                    <td><span class="datasee" id="pod1" runat="server">BEANR2</span></td>
                    </tr>
                   </table>
                      <input id="oversize" type="hidden" runat="server" />
                      <input id="notas" type="hidden"  runat="server"/>
                 </div>
                  <div class="separator">Booking Item</div>
                 <div class="bokindetalle">
                 <asp:Repeater ID="tablePagination" runat="server" >
                 <HeaderTemplate>
                 <table id="tablasort" cellspacing="1" cellpadding="1" class="tabRepeat">
                 <thead>
                 <tr>
                 <th>No.</th>
                 <th>ISO</th>
                 <th>Temp°C</th>
                 <th>Reservado</th>
                 <th>Ingresados</th>
                 <th>Disponible</th>
                 <th>&nbsp;</th>
                 <th class="xax">&nbsp;</th>
                 <th class="xax">&nbsp;</th>
                 <th class="xax">&nbsp;</th>
                 <th class="xax">&nbsp;</th>
                 <th class="xax">&nbsp;</th>
                 <th class="xax">&nbsp;</th>
                 <th class="xax">&nbsp;</th>
                 <th class="xax">&nbsp;</th>
                 <th class="xax">&nbsp;</th>
                 <th class="xax">&nbsp;</th>
                 <th class="xax">&nbsp;</th>
                 <th class="xax">&nbsp;</th>
                 <th class="xax">&nbsp;</th>
                 <th class="xax">&nbsp;</th>
                 <th class="xax">&nbsp;</th>
                 </tr>
                 </thead> 
                 <tbody>
                 </HeaderTemplate>
                 <ItemTemplate>
                 <tr class= '<%# getClass( Eval("dispone"),Eval("bfk"),Request.QueryString["V"]) %>' onclick="setBooking(this);">
                  <td><%#Eval("bitem")%></td>
                  <td><a class="tooltip" ><span class="classic"><%#Eval("iso_descrip")%></span> <%#Eval("biso")%></span></td>
                  <td><%#Eval("btemp")!=null?Eval("btemp").ToString().Replace(',','.'):"0.0"%></td>
                  <td><%#Eval("reserva")%></td>
                  <td><%#Eval("ingresa")%></td>
                  <td><%#Eval("dispone")%></td>
                  <td><a href="#" >Elegir</a></td>
                  <td class="xax"><%#Eval("gkey")%></td>
                  <td class="xax"><%#Eval("usize")%></td>
                  <td class="xax"><%#Eval("biso")%></td>
                  <td class="xax"><%#Eval("utara")%></td>
                  <td class="xax"><%#Eval("bimo")%></td>
                  <td class="xax"><%#Eval("breefer")%></td>
                  <td class="xax"><%#Eval("shiperid")%></td>
                  <td class="xax"><%#Eval("shipname")%></td>
                  <td class="xax"><%#Eval("bline")%></td>
                  <td class="xax"><%#Eval("hzkey")%></td>
                  <td class="xax"><%#Eval("aqt")%></td>
                  <td class="xax"><%#Eval("vent_pc")%></td>
                  <td class="xax"><%#Eval("ventu")%></td>
                  <td class="xax"><%#Eval("hume")%></td>
                  <td class="xax"><%#Eval("temp")%></td>
                 </tr>
                  </ItemTemplate>
                 <FooterTemplate>
                 </tbody>
                 </table>
                 </FooterTemplate>
         </asp:Repeater>
                </div>
             </div>
             <div class="propiedad" id="propiedad" runat="server" >Mensaje</div>
             </div>
             <div id="pager">
                  Registros por página
                 <select class="pagesize">
                  <option selected="selected" value="5">5</option>
                  <option value="10">10</option>
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
             asegurese que el booking que busca sea correspondiente al AISV, AISV Contenedores-> Booking FCL, 
             AISV Carga suelta-> Booking BBULK,AISV Consolidación-> Booking LCL.
             y pertenecientes a la línea seleccionada  </div>
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
       function setBooking(row) {
           var str = row.getElementsByTagName('td');
           var strda = document.getElementById('cutoff').textContent;
           if(strda == null || strda == undefined  || strda.trim().length<=0 ) 
           {
             alert('El booking no tiene fecha de cutoff!');
             return;
           }
            var datePart = strda.split('/');
            var fecha = new Date(datePart[1] + '/' + datePart[0] + '/' + datePart[2]);
           fecha.setHours(fecha.getHours() - 12);
           var bkitem = {
               numero: document.getElementById('numero').textContent,
               referencia: document.getElementById('referencia').textContent,
               fk: document.getElementById('fk').textContent,
               nave: document.getElementById('nave').textContent,
               eta: document.getElementById('eta').textContent,
               cutoff: document.getElementById('cutoff').textContent,
               pod: document.getElementById('pod').textContent,
               pod1: document.getElementById('pod1').textContent,
               comoditi: document.getElementById('comoditi').textContent,
               imo: document.getElementById('imo').textContent,
               remark: document.getElementById('notas').value,
               uis : formattedDate(fecha,true),
               //Cabecera ok.
               item: str[0].textContent,
               temp: str[2].textContent ,
               reserva: str[3].textContent ,
               ingresa: str[4].textContent,
               dispone: str[5].textContent ,
               gkey: str[7].textContent,
               longitud: str[8].textContent,
               iso:  str[9].textContent,
               tara:  str[10].textContent.length > 0? (parseFloat(str[10].textContent)/1000).toString():'0',
               bimo: str[11].textContent.length > 0 ? true:false,
               refer: str[12].textContent.length >= 0 && str[12].textContent.trim() == '1' ? true : false,
               shipid: str[13].textContent,
               shipname:str[14].textContent,
               bline:str[15].textContent,
               hzkey:str[16].textContent,
               aqt:str[17].textContent,
             
               vent_pc:str[18].textContent,
               ventu:str[19].textContent,
               hume: str[20].textContent
           };
           if (bkitem.fk !='LCL' && bkitem.dispone <= 0) {
               alert('El item elegido ya no tiene disponibilidad');
               return;
           }
           if (window.opener != null) {
                window.opener.validateBook(bkitem)
            }
            self.close();
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
