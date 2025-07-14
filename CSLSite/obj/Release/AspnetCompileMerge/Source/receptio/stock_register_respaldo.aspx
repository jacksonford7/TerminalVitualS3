<%@ Page  Title="Distribución de Inventario para Depósitos"  Language="C#" MasterPageFile="~/site.Master"
    AutoEventWireup="true" CodeBehind="stock_register_respaldo.aspx.cs" Inherits="CSLSite.stock_register_respaldo"  %>



<asp:Content ID="Content1" ContentPlaceHolderID="placehead" runat="server">
    
       
     <link href="../shared/estilo/tables.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/aisv-general.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/work.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/catalogos.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/info.css" rel="stylesheet" type="text/css" />
   <link href="../shared/estilo/opc_tables.css" rel="stylesheet" />
    <script type="text/javascript">
           
                $(document).ready(function () {
                    document.getElementById('imagen').innerHTML = '';
                    $('#tablasort').tablesorter({ cancelSelection: true, cssAsc: "ascendente", cssDesc: "descendente", headers: { 8: { sorter: false }, 9: { sorter: false}} }).tablesorterPager({ container: $('#pager'), positionFixed: false, pagesize: 10 });
                });
        
    </script>

    </asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="placebody" runat="server" >
    <asp:ScriptManager ID="Validacion1" runat="server" />
 

    <input id="zonaid" type="hidden" value="205" />
     <noscript><meta http-equiv="refresh" content="0; url=sinsoporte.htm" /> </noscript>
   
     <div>
        
         <i class="ico-titulo-2"></i><h1>Distribución de Inventario para Depósitos</h1><br />
    </div>
     <div class=" msg-alerta">
    <span id="dtlo" runat="server">Estimado usuario:</span> 
    <br /> ...
    </div>
     
     <div class="seccion" id="BUSCAR">
      <div class="informativo">
      <table>
      <tr><td rowspan="2" class="inum"> <div class="number">1</div></td><td class="level1" >Datos de la Transacción</td></tr>
      <tr><td class="level2">Debe ingresar la fecha, la cantidad, seleccionar el depósito, seleccionar el tipo de operación (+/-)</td></tr>
      </table>
     </div>


  
   
     <div class="colapser colapsa" ></div>
     <div class="accion">
   
     <table class="controles" cellspacing="0" cellpadding="1">
        <tr><th class="bt-bottom bt-right bt-top bt-left" colspan="4"></th></tr>
          <tr><td class="bt-bottom  bt-right bt-left">LINEA NAVIERA:</td>
             <td class="bt-bottom bt-right"> <span id="nombre" class="caja cajafull"><asp:Label ID="LblNombre" runat="server" Text="LblNombre" Width="379px"></asp:Label></span></td>   

          </tr>
          <tr>
         <td class="bt-bottom  bt-right bt-left" >1. Fecha Transacción:</td>
         <td class="bt-bottom bt-right" colspan="3">
         <asp:TextBox ID="TxtFechaTransaccion" runat="server" MaxLength="16" Width="250px" CssClass="datetimepicker"
               onkeypress="return soloLetras(event,'1234567890/:')"
               onBlur="valDate(this,  true,valdatem);"
               placeholder="dd/mm/aaaa hh:mm "
               
             onchange="defaultDate(this);" Enabled ="false" 
               ></asp:TextBox>
             <span class="validacion" id="valfechacita"  > * obligatorio</span>
         </td>       
         </tr>
         <tr>
         <td class="bt-bottom  bt-right bt-left" >2. Seleccione un Depósito:</td>
         <td class="bt-bottom bt-right" colspan="3">
         
             <asp:DropDownList ID="CboDeposito" runat="server" Width="250px" AutoPostBack="True"
                                            Height="28px"  DataTextField='name' DataValueField='id_depot'  
                                            Font-Size="Small" OnSelectedIndexChanged="CboDeposito_SelectedIndexChanged" 
                                            >
                                        </asp:DropDownList>
                        
             <span class="validacion" id="xplinea" > * obligatorio</span>
        
         </td>       
         </tr>
         <tr>
         <td class="auto-style6" >3. Cantidad</td>
          <td class="bt-bottom bt-right">
          <asp:TextBox ID="TxtCantidad" runat="server" CssClass="inputText" MaxLength="5"
         Width="120px" onkeypress="return soloLetras(event,'1234567890',false)" onpaste="return false;"></asp:TextBox>
          <span class="validacion" id="valhoras"  > * obligatorio</span></td>            
         </tr>
          <tr>
         <td class="bt-bottom  bt-right bt-left" >4. Seleccione la operación:</td>
         <td class="bt-bottom bt-right" colspan="3"> <asp:DropDownList ID="CboOperacion" runat="server" Width="280px" AutoPostBack="False"
                                            Height="28px"  DataTextField='notes' DataValueField='id_operation'  
                                            Font-Size="Small" 
                                            >
                                        </asp:DropDownList>

             <span class="validacion" id="xplinea" > * obligatorio</span>
      
         </td>       
         </tr>
        

     </table>
         <div class="botonera" style="align-content:center">
          <asp:Button ID="BtnAgregar" runat="server"   OnClick="BtnAgregar_Click" Text="Agregar" Width="100px"/>
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        </div>
   
     </div>
    </div>
     <div class="seccion">
      <div class="informativo">
      <table>
      <tr><td rowspan="2" class="inum"> <div class="number">2</div></td><td class="level1" >DETALLE DE MOVIMIENTOS</td></tr>
      <tr><td class="level2">
       Podrá eliminar un movimiento, dando click en el botón REMOVER.
      </td></tr>
      </table>
     </div>
    <div class="colapser colapsa"></div>
      <div class="accion">
     <table class="controles" cellspacing="0" cellpadding="1">
        <tr><th class="bt-bottom bt-right bt-top bt-left" colspan="2"> &nbsp;</th></tr>
        

          <tr>
            <th colspan="2">
             <div class="findresult" >
              <div class="booking" >
               <div class="separator">DETALLE DEPOSITOS</div>
              <div class="bokindetalle">

          <asp:Repeater ID="TableStock" runat="server"   OnItemDataBound="Opciones_ItemDataBound">
                <HeaderTemplate>
                <table id="tablasort" cellspacing="0" cellpadding="1" class="tabRepeat">
                     
                <thead>
                <tr>
                    <th style='width:20px'>SEMANA</th>
                    <th style='width:110px'>FECHA</th>
                    <th style='width:150px'>DEPOSITO</th>
                    <th style='width:250px'>TIPO TRANSACCION</th>
                    <th style='width:70px'>(+)</th>
                    <th style='width:70px'>(-)</th>
                    <th style='width:70px'>STOCK</th>
                   
                  </tr>
                </thead> 
                <tbody>
                </HeaderTemplate>
                <ItemTemplate>
                <tr class="point">
                <td  style='width:40px'><asp:Label Text='<%#Eval("CCREATE_WEEK")%>' ID="Label1" runat="server"   /></td>
                <td style='width:100px'><asp:Label Text='<%#Eval("CCREATE_DATE")%>' ID="lblFecha" runat="server"  /></td>
                <td ><asp:Label Text='<%#Eval("NAME")%>' ID="lblname" runat="server" /></td>
                <td ><asp:Label Text='<%#Eval("NOTES")%>' ID="lblNotes" runat="server" /></td>
                <td ><asp:Label Text='<%#Eval("CCANT_ING")%>' ID="lblIngreso" runat="server" /></td>
                <td ><asp:Label Text='<%#Eval("CCANT_EGR")%>' ID="lblEgreso" runat="server" /></td>
                <td ><asp:Label Text='<%#Eval("CTOTAL")%>' ID="lbltotal" runat="server" /></td>
               
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


            </th>
          </tr>


     </table>
     </div>
    </div>
 
     <div class="seccion">
      <div class="informativo">
      <table>
     
      
      </table>
     </div>

     <div class="accion" id="ADU">
  

 
      <div class="botonera">
         <img alt="loading.." src="../shared/imgs/loader.gif" id="loader" class="nover"  />
          &nbsp;
          &nbsp;

     </div>
    
     </div>
    </div>

   
    



    <script type="text/javascript">
    
        $(window).load(function () {
            //objeto a transportar.
            $(document).ready(function () {
                //inicia los fecha-hora
                $('.datetimepicker').datetimepicker().datetimepicker({ lang: 'es', step: 30, format: 'd/m/Y H:i' });
                //inicia los fecha
                $('.datetimepickerAlt').datetimepicker().datetimepicker({ lang: 'es', step: 30, format: 'd/m/Y' });

                $('.datepicker').datetimepicker().datetimepicker({ lang: 'es', timepicker: false, format: 'd/m/Y', closeOnDateSelect: true, minDate: '0' });
                //init reefer-> lo pone a false.

            });
        });
   
    </script>

    <script type="text/javascript">

        $(window).load(function () {
                            $("div.colapser").toggle(function () { $(this).removeClass("colapsa").addClass("expande"); $(this).next().hide(); }
                                  , function () { $(this).removeClass("expande").addClass("colapsa"); $(this).next().show(); });
        });


        function cancelEmpty() {
            var txt = document.getElementById("TxtReferencia");
            if (txt != null && txt != undefined) {
                if (txt.value) {
                    return true;
                }
            }
            //detenga el post
            alert("Por favor escriba la referencia");
            return false;
        }


        function defaultDate(control) {
            if (control.value) {
                var cj = document.getElementById("TxtFechaDesde");
                if (cj != null && cj != undefined) {
                    cj.value = control.value;
                }
            }
        }
    </script>
      <script src="../Scripts/pages.js" type="text/javascript"></script>
    <script src="../Scripts/opc_control.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.tablesorter.min.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.tablesorter.pager.js" type="text/javascript"></script>
</asp:Content>