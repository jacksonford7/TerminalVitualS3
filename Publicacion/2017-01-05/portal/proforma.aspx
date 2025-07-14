<%@ Page Title="Proformas" Language="C#" MasterPageFile="~/site.Master" AutoEventWireup="true" CodeBehind="proforma.aspx.cs" Inherits="CSLSite.proforma" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="placehead" runat="server">
    <link href="../shared/estilo/tables.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/aisv-general.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/work.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/catalogos.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/info.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/proforma.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/modal.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .warning { background-color:Yellow;  color:Red;}
        .panel-reveal-modal-bg { background: #000; background: rgba(0,0,0,.8);cursor:progress;	}
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="placebody" runat="server">
    <asp:ToolkitScriptManager ID="tksq"  runat="server" EnablePartialRendering="true"></asp:ToolkitScriptManager>
      <asp:HiddenField ID="manualHide" runat="server" />
    <input id="zonaid" type="hidden" value="802" />
    <div>
   <i class="ico-titulo-1"></i><h2>Servicios a clientes CGSA</h2>  <br /> 
   <i class="ico-titulo-2"></i><h1>Generación de Proformas para Exportación</h1><br />
 </div><div class="seccion">
       <div class="accion">
                          <div class=" msg-alerta">
    <span id="dtlo" runat="server">Estimado usuario:</span> 
    <br /> Asegúrese que toda la información que agrega a este documento es correcta antes de proceder a su respectiva generación, si desea confirmar alguna información antes de proceder comuníquese con nuestro departamento de planificación a los teléfonos: +593 (04) 6006300, 3901700 
                 ext. 4039, 4040, 4060. 
                         <asp:HyperLink 
            ID="HyperLink1" 
            runat="server"
            Text="Genere la Liquidación para realizar el pago de su Proforma aquí"
            NavigateUrl="~/PagoenLinea/ConsultaAnticipo"
            >
        </asp:HyperLink>
    </div>
            <table class="xcontroles" cellspacing="0" cellpadding="1">
        <tr><th class="bt-bottom bt-right  bt-left bt-top" colspan="3">Datos para la generación</th></tr>
         <tr>
         <td class="bt-bottom bt-left bt-right" >Booking No. :</td>
         <td class="bt-bottom bt-right" >
                   <span id="numbook" runat="server" clientidmode="Static" class="caja" onclick="quitar();" >...</span>
                   <span id="nbqty" class="caja" style="width:80px;" runat="server" clientidmode="Static"  >...</span>
                   <input id="nbrboo" type="hidden" value="" runat="server" clientidmode="Static"/>
                   <input id="bkey" type="hidden" value="" runat="server" clientidmode="Static"/>
                   <input id="bkqty" type="hidden" value="" runat="server" clientidmode="Static"/>
                   <input id="bknna" type="hidden" value="" runat="server" clientidmode="Static"/>
                   <input id="bkrefe" type="hidden" value="" runat="server" clientidmode="Static"/>
                    
                   <input id="bketd" type="hidden" value="" runat="server" clientidmode="Static"/>
                   <input id="bkrefer" type="hidden" value="" runat="server" clientidmode="Static"/>
                   <input id="bkcutof" type="hidden" value="" runat="server" clientidmode="Static"/>
                   <input id="bksize" type="hidden" value="" runat="server" clientidmode="Static"/>
                   
                     
          </td>
         <td class="bt-bottom bt-right validacion ">
        <a  class="topopup" target="popup" onclick="window.open('../catalogo/reserva','name','width=850,height=480')" >
          <i class="ico-find" ></i> Buscar </a>
         </td>
         </tr>

         <tr>
         <td class="bt-bottom  bt-right bt-left">Cantidad:</td>
         <td class="bt-bottom bt-right">
               <asp:TextBox ID="txtqty" runat="server" Width="200px" MaxLength="3" CssClass="mayusc"
              onkeypress="return soloLetras(event,'1234567890',true)"  ClientIDMode="Static"
              placeholder="CNTR QTY"
             ></asp:TextBox>
             </td>
         <td class="bt-bottom bt-right validacion ">
         <span id="valmailz" class="opcional" > * opcional</span>
         </td>
         </tr>
         <tr  id="fefila" class="nover" >
         <td class="bt-bottom  bt-right bt-left" >Fecha estimada de retiro:</td>
         <td class="bt-bottom bt-right">
                   <asp:TextBox ID="txdate" runat="server" Width="200px" MaxLength="3" CssClass="datepicker"
              onkeypress="return soloLetras(event,'1234567890/',true)" 
               onblur="valDate(this,false,esfecha);"
               ClientIDMode="Static"
              placeholder="dd/mm/yyyy"
             ></asp:TextBox>
             </td>
         <td class="bt-bottom bt-right validacion ">
         <span id="esfecha" class="opcional" > </span>
         </td>
         </tr>
         </table>
         <div class="botonera">
          <span id="imagen"></span>
             <asp:Button ID="btbuscar" runat="server" Text="Generar proforma"  onclick="btbuscar_Click" OnClientClick="return checkData();"  />
         </div>
             <div class="cataresult" >
               <asp:UpdatePanel ID="uptabla" runat="server" ChildrenAsTriggers="true" >
               <ContentTemplate>


                  <div id="xfinder" runat="server" visible="false" >
                  <div class="msg-alerta" id="alerta" runat="server"  >Notas y descripciones sobre la  proforma</div> 
                  <div class="findresult" >
                 <div class="booking" >
                 <div class="separator">Vista preliminar</div>
                 <div class="bokindetalle">
                 <fieldset>
                 <asp:Repeater ID="tablaNueva" runat="server" OnItemCreated="repeat_ItemCreated"   >
                 <HeaderTemplate>
                    <table id="miProforma" class="costo"  cellpadding="1" cellspacing="1">
                    <thead>
                       <tr><th></th><th></th><th>Código</th><th>Descripcion</th><th>Cant.</th><th>V.Unit</th><th>V.Total</th></tr>
                    </thead>
                 </HeaderTemplate>
                 <ItemTemplate>
                 <tr class="point" >
                  <td><%#Eval("mensaje")%></td>  
                  <td><asp:CheckBox id='cbox' AutoPostBack="true" runat="server" Checked='<%#Eval("aplica")%>'  Enabled='<%#Eval("opcional")%>'  OnCheckedChanged="chkCheckedChanged"   argumento='<%#Eval("codigo")%>' evento='<%#Eval("html")%>' /> </td>
                  <td><%#Eval("codigo")%></td>
                  <td><%#Eval("contenido")%></td>
                  <td><%#Eval("cantidad")%></td>
                  <td><%#DataBinder.Eval(Container.DataItem, "costo", "{0:C}")%></td>
                  <td><%#DataBinder.Eval(Container.DataItem, "vtotal", "{0:C}")%></td>
                 </tr>
                  </ItemTemplate>
                 <FooterTemplate>
                 </tbody>
                 </table>
                 </FooterTemplate>
                </asp:Repeater>
                <table class="totales" cellpadding="0" cellspacing="0">
                <tr><td  class='filat'>Total por unidad</td><td class="estotal"><span id='stunit' runat="server" clientidmode="Static"  >$0000.00</span></td></tr>
                <tr><td  class='filat'> <span runat="server" clientidmode="Static" id="etiIva">IVA %(+)</span> </td><td class="estotal"><span id='siva' runat="server" clientidmode="Static"  >$0000.00</span></td></tr>
                <tr><td  class='filat'>TOTAL</td><td class="estotal"><span id='sttal' runat="server" clientidmode="Static" >$0000.00</span></td></tr>
                </table>
            </fieldset>
                </div>
                <div class="botonera" runat="server" id="btnera">
                <img alt="loading.." src="../shared/imgs/loader.gif" id="loader" class="nover"  />
                    &nbsp;<asp:Button ID="btprint" runat="server" Text="Confirmar e Imprimir" 
                        OnClientClick="return confirm('Esta seguro de generar la proforma?\nEste proceso es irreversible');" 
                        onclick="btprint_Click"   />&nbsp;
               </div>
             </div>
             </div>
              </div>
               <div id="sinresultado" runat="server" class="msg-info"></div>
         </ContentTemplate>
         <Triggers>
           <asp:AsyncPostBackTrigger  ControlID="btprint"/>
            <asp:AsyncPostBackTrigger  ControlID="btbuscar"/>
            <asp:AsyncPostBackTrigger ControlID="btcancel" />
         </Triggers>
         </asp:UpdatePanel>
             </div>
      </div>
  </div>


      <asp:ModalPopupExtender  
      ID="mpedit" runat="server" 
      PopupControlID="myModal"
      CancelControlID="btclose"    
      BackgroundCssClass="panel-reveal-modal-bg"  
      TargetControlID="manualHide"
      >
    </asp:ModalPopupExtender>

    <asp:Panel ID="myModal" runat="server" CssClass="panel-reveal-modal " >
        <div class="inner-modal ">
            <asp:UpdatePanel ID="upinercontent" runat="server" ChildrenAsTriggers="true" >
               <ContentTemplate>
                   <p class="sumary " runat="server" id="notario">&nbsp;</p>
               </ContentTemplate>
            </asp:UpdatePanel>
       </div>
    <div class="modal-menu">
         
          <asp:Button  runat="server" id="btcancel"  Text="Incluir"  
              class="close-reveal-modal" onclick="btcancel_Click"  />
               <input  type="button" id="btclose"  value="No Incluir"  
               class="close-reveal-modal"  />
    </div>
    </asp:Panel>

    <script src="../Scripts/pages.js" type="text/javascript"></script>
    <script src="../Scripts/AddIn.js" type="text/javascript"></script>
    <script type="text/javascript">

        function popupCallback(objeto, catalogo) {
            if (objeto == null || objeto==undefined) {
                alert('Hubo un problema al setear un objeto de catalogo');
                return;
            }
            //si catalogos es booking
            if (catalogo == 'bk') {
                document.getElementById('numbook').textContent = objeto.boking;
                document.getElementById('nbqty').textContent = '' + objeto.qty + ' U';
                document.getElementById('txtqty').value = objeto.qty;
                document.getElementById('bkqty').value = objeto.qty;
                document.getElementById('nbrboo').value = objeto.boking;
                document.getElementById('bkey').value = objeto.gkey;
                document.getElementById('bknna').value = objeto.nave;
                document.getElementById('bkrefe').value = objeto.referencia;
                /*nuevo*/
                document.getElementById('bketd').value = objeto.etd;
                document.getElementById('bkrefer').value = objeto.reefer;
                document.getElementById('bkcutof').value = objeto.cutof;
                document.getElementById('bksize').value = objeto.long;
               
                return;
            }
        }
        function quitar() {
            document.getElementById('numbook').textContent = '...';
            document.getElementById('nbqty').textContent = '';
            document.getElementById('bkqty').value = '';
            document.getElementById('nbrboo').value = '';
            document.getElementById('bkey').value = '';
            document.getElementById('bknna').value = '';
            document.getElementById('bkrefe').value = '';
          
            /*Nuevo*/
            document.getElementById('bketd').value = '';
            document.getElementById('bkrefer').value = '';
            document.getElementById('bkcutof').value = '';
            document.getElementById('bksize').value = '';

            return;
        }
        function checkData() {
            if (document.getElementById('nbrboo').value == '...' || document.getElementById('nbrboo').value == '') {
                alert('Seleccione el número de booking');
                return false;
            }
            if (document.getElementById('txtqty').value == '' || isNaN(document.getElementById('txtqty').value) || document.getElementById('txtqty').value == '0' ) {
                alert('Por favor escriba la cantidad para la proforma');
                return false;
            }
            if (parseInt(document.getElementById('txtqty').value) > parseInt(document.getElementById('bkqty').value)) {
                alert('La cantidad de la proforma no debe superar las reservas del booking');
                return false;
            }

            /*Nuevo si es reefer verificar: valor en fecha, que sea fecha y cutoff */
            if (document.getElementById('bkrefer').value == '1') {
                //verficar que tenga valor el campo fecha. ok
                var etd = document.getElementById('bketd').value;
            
                var cut = document.getElementById('bkcutof').value;
                var cli = document.getElementById('txdate').value; //
                if (etd == null || etd == '') {
                    alert('La fecha estimada de zarpe de la nave no ha sido establecida, comuníquese urgente con planeamiento');
                    return false;
                }
                if (cut == null || cut == '') {
                    alert('La fecha de CutOff de la nave no ha sido establecida, comuníquese urgente con planeamiento');
                    return false;
                }
                /*
                if (cli == null || cli == '') {
                    alert('La fecha estimada de retiro es un campo obligatorio');
                    return false;
                }

                var res = comprobarFecha(cli, cut);
                if (!res) {
                    alert('La fecha estimada de retiro [' + cli + '] no puede ser mayor a fecha de corte [CutOff: ' + cut + ']');
                    return false;
                }
                */
            }
            return true;
        }
        $(document).ready(function () {
            //inicia los fecha
            $('.datepicker').datetimepicker().datetimepicker({ lang: 'es', timepicker: false, format: 'd/m/Y', closeOnDateSelect: true, minDate: '0' });
        });


    </script>
  </asp:Content>
