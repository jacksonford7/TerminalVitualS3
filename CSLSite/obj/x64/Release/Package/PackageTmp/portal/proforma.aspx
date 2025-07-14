<%@ Page Title="Proformas" Language="C#" MasterPageFile="~/site.Master" AutoEventWireup="true" CodeBehind="proforma.aspx.cs" Inherits="CSLSite.proforma" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="placehead" runat="server">
    <link href="../shared/estilo/tables.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/work.css" rel="stylesheet" type="text/css" />
    

    <link href="../shared/estilo/modal.css" rel="stylesheet" type="text/css" />
       <link href="../shared/estilo/jquery.datetimepicker.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .warning { background-color:Yellow;  color:Red;}
        .panel-reveal-modal-bg { background: #000; background: rgba(0,0,0,.8);cursor:progress;	}
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="placebody" runat="server">
    <asp:ToolkitScriptManager ID="tksq"  runat="server" EnablePartialRendering="true"></asp:ToolkitScriptManager>
      <asp:HiddenField ID="manualHide" runat="server" />
    <input id="zonaid" type="hidden" value="802" />
  
        <div class="mt-4">         
        <nav class="mt-4" aria-label="breadcrumb">
          <ol class="breadcrumb">
            <li class="breadcrumb-item" id="opcion_principal" runat="server"><a href="#">Servicios</a></li>
             <li class="breadcrumb-item active" aria-current="page" id="sub_opcion" runat="server">Generación de Proformas/Liquidación para Exportación</li>
          </ol>
        </nav>
      </div>
    
  <div class="dashboard-container p-4" id="cuerpo" runat="server">
                                <div class=" alert  alert-warning">
    <span id="dtlo" runat="server">Estimado Cliente:</span> 
    <br /> Asegúrese que toda la información que agregue a este documento es correcta antes de proceder a su respectiva generación, si desea confirmar alguna información antes de proceder comuníquese con nuestro departamento de Expo Navios email: ExpoNavios@cgsa.com.ec.

    </div>
         <div class="form-title">Datos para la generación</div>
		 
		  <div class="form-row">
		   <div class="form-group col-md-12"> 
		   	  <label for="inputAddress">Booking<span style="color: #FF0000; font-weight: bold;"></span></label>
			  <div class="d-flex">
                  <span id="numbook" runat="server" clientidmode="Static" class=" form-control col-md-8" onclick="quitar();" >...</span>
                   <span id="nbqty" class="  form-control col-md-3 ml-1"  runat="server" clientidmode="Static"  >...</span>

                     <a  class="btn btn-outline-primary mr-4" target="popup" onclick="window.open('../catalogo/bookingPro.aspx','name','width=850,height=880')" >
         <span class='fa fa-search' style='font-size:24px'></span> 

        </a>

			  </div>
		   </div>
		  </div>

        <div class="form-row">
		  
		   <div class="form-group col-md-6"> 
		   	  <label for="inputAddress">Cantidad<span style="color: #FF0000; font-weight: bold;"></span></label>
			  <div class="d-flex">
                     <asp:TextBox ID="txtqty" runat="server"  MaxLength="3" CssClass="mayusc form-control"
              onkeypress="return soloLetras(event,'1234567890',true)"  ClientIDMode="Static"
              onblur="cadenareqerida(this,1,5,'val_qty');"
              placeholder="CNTR QTY"
             ></asp:TextBox>
                  <span id="val_qty" class="validacion" > *</span>
			  </div>
		   </div>
            	   <div class="form-group col-md-6"> 
		   	  <label for="inputAddress">Fecha/Hora estimada de ingreso:<span style="color: #FF0000; font-weight: bold;"></span></label>
			  <div class="d-flex">
                        <asp:TextBox ID="txdate" runat="server"  
                            MaxLength="3" CssClass="datepicker form-control"
              onkeypress="return soloLetras(event,'1234567890/',true)" 
               onblur="valDate(this,false,esfecha);"
               ClientIDMode="Static"
              placeholder="dd/mm/yyyy HH:mm"
             ></asp:TextBox>
               <span id="esfecha" class="validacion" >*  </span>

			  </div>
		   </div>
		  </div>


		
      
      <div class="form-row">
		  
		   <div class="form-group col-md-6"> 
		   	  <label for="inputAddress">Retención FUENTE<span style="color: #FF0000; font-weight: bold;"></span></label>
			  <div class="d-flex">
                    <asp:DropDownList ID="dpfte" runat="server" ClientIDMode="Static" 
                 onchange="darValor(this,'valfte','val_fte')" CssClass="form-control" >
                 <asp:ListItem Value="0" Selected="True">0%</asp:ListItem>
                 <asp:ListItem Value="1" >1%</asp:ListItem>
                 <asp:ListItem Value="8" >8%</asp:ListItem>
                 <asp:ListItem Value="10" >10%</asp:ListItem>
             </asp:DropDownList>
              <span id="valfte" class="validacion" >* </span>

			  </div>
		   </div>

          	   <div class="form-group col-md-6"> 
		   	  <label for="inputAddress">Retención IVA<span style="color: #FF0000; font-weight: bold;"></span></label>
			  <div class="d-flex">
                       <asp:DropDownList ID="dpiva" runat="server" ClientIDMode="Static" 
                 onchange="darValor(this,'valiva','val_iva')" CssClass="form-control">
                 <asp:ListItem Value="0" Selected="True">0%</asp:ListItem>
                 <asp:ListItem Value="1" >1%</asp:ListItem>
                 <asp:ListItem Value="8" >8%</asp:ListItem>
                 <asp:ListItem Value="10" >10%</asp:ListItem>
             </asp:DropDownList>
              <span id="valiva" class="validacion" >*  </span>

			  </div>
		   </div>
		  </div>
		 
                 <input id="nbrboo" type="hidden" value="" runat="server" clientidmode="Static"/>
                   <input id="bkey" type="hidden" value="" runat="server" clientidmode="Static"/>
                   <input id="bkqty" type="hidden" value="" runat="server" clientidmode="Static"/>
                   <input id="bknna" type="hidden" value="" runat="server" clientidmode="Static"/>
                   <input id="bkrefe" type="hidden" value="" runat="server" clientidmode="Static"/>
                    
                   <input id="bketd" type="hidden" value="" runat="server" clientidmode="Static"/>
                   <input id="bkrefer" type="hidden" value="" runat="server" clientidmode="Static"/>
                   <input id="bkcutof" type="hidden" value="" runat="server" clientidmode="Static"/>
                   <input id="bksize" type="hidden" value="" runat="server" clientidmode="Static"/>
                   <input id="exporID" type="hidden" value="" runat="server" clientidmode="Static"/>

                    <input id="val_fte" type="hidden" value="" runat="server" clientidmode="Static"/>
                    <input id="val_iva" type="hidden" value="" runat="server" clientidmode="Static"/>


             <div class="form-row">
            <div class="col-md-12 d-flex justify-content-center"> 
                  <span id="imagen"></span>
             <asp:Button ID="btbuscar" runat="server" Text="Generar proforma" 
                 onclick="btbuscar_Click" OnClientClick="return checkData();" 
                 CssClass="btn btn-primary"  />
		   </div> </div>

 
		
		                 
               <asp:UpdatePanel ID="uptabla" runat="server" ChildrenAsTriggers="true" >
               <ContentTemplate>




                  <div id="xfinder" runat="server" visible="false" >


                         <div class="form-row">
		   <div class="col-md-12 d-flex justify-content-center"> 
		                      <div class=" alert   alert-primary" id="alerta" runat="server"  >Notas y descripciones sobre la  proforma</div> 

		   </div> 
		   </div>

                
                  <div class="form-row">
		   <div class="col-md-12"> 
		                     <asp:Repeater ID="tablaNueva" runat="server" OnItemCreated="repeat_ItemCreated"   >
                 <HeaderTemplate>
                    <table id="miProforma" class="table table-bordered invoice"  >
                    <thead>
                       <tr><th></th><th></th><th>Código</th><th>Descripcion</th><th>Cant.</th><th>P.Unit</th><th>P.Total</th></tr>
                    </thead>
                         <tbody>
                 </HeaderTemplate>
                 <ItemTemplate>
                 <tr class="point" >
                  <td><%#Eval("mensaje")%></td>  
                  <td><asp:CheckBox id='cbox' AutoPostBack="true" runat="server" Checked='<%#Eval("aplica")%>'  Enabled='<%#Eval("opcional")%>'  OnCheckedChanged="chkCheckedChanged"   argumento='<%#Eval("codigo")%>' evento='<%#Eval("html")%>' grupo='<%#Eval("condicion")%>' /> </td>
                  <td><%#Eval("codigo")%></td>
                  <td><%#Eval("contenido")%></td>
                  <td><%#Eval("cantidad")%></td>
                  <td><%#DataBinder.Eval(Container.DataItem, "costo", "{0:C}")%></td>
                  <td><%#DataBinder.Eval(Container.DataItem, "vtotal", "{0:C}")%></td>
                 <%-- <td><%#Eval("condicion")%></td>--%>
                 </tr>
                  </ItemTemplate>
                 <FooterTemplate>
                 </tbody>
       

                 </table>
                 </FooterTemplate>
                </asp:Repeater>

		   </div> 
		   </div>

                  <div class="form-row">
                       <div class="col-md-9 "> 
		    &nbsp;
		   </div> 
		   <div class="col-md-3 "> 
		                         <table class="table invoice table-bordered " >
                     <tbody>
                <tr><td  class='border-none text-right  '>Subtotal:</td><td class=" text-right"><span id='stunit' runat="server" clientidmode="Static"  >$0000.00</span></td></tr>
                <tr><td  class='border-none text-right text-success '> <span runat="server" clientidmode="Static" id="etiIva">IVA %(+):</span> </td><td class="text-right"><span id='siva' runat="server" clientidmode="Static"  >$0000.00</span></td></tr>
                 <tr><td  class='border-none text-right text-danger '> <span runat="server" clientidmode="Static" id="etisrfte">Ret.Fte %(-):</span> </td><td class="text-right"><span id='srfte' runat="server" clientidmode="Static"  >$0000.00</span></td></tr>
                 <tr><td  class='border-none text-right text-danger '> <span runat="server" clientidmode="Static" id="stisriva">Ret.IVA %(-):</span> </td><td class="text-right"><span id='sriva' runat="server" clientidmode="Static"  >$0000.00</span></td></tr>
                <tr><td  class='border-none text-right '>NETO A PAGAR:</td><td class="text-right"><span id='sttal' runat="server" clientidmode="Static" >$0000.00</span></td></tr>
                        </tbody>
                </table>

		   </div> 
		   </div>
               
                
         
              


                     <div class="row">
		  
		   <div class="col-md-12 d-flex justify-content-center" id="btnera" runat="server" clientidmode="Static"> 
                  <img alt="loading.." src="../shared/imgs/loader.gif" id="loader" class="nover"  />
                   <asp:Button ID="btprint" runat="server" Text="Confirmar e Imprimir" 
                        OnClientClick="return confirm('Esta completamente seguro que todos los datos están correctos?\nPara continuar con el proceso presione el botón aceptar.');" 
                        onclick="btprint_Click" CssClass="btn btn-primary"   />
		   </div> </div>


           
             
              </div>
               <div id="sinresultado" runat="server" class=" alert alert-info"></div>
         </ContentTemplate>
         <Triggers>
            <asp:AsyncPostBackTrigger  ControlID="btprint"/>
            <asp:AsyncPostBackTrigger  ControlID="btbuscar"/>
            <asp:AsyncPostBackTrigger ControlID="btcancel" />
         </Triggers>
         </asp:UpdatePanel>
        

		  
		  
      </div>
      <asp:ModalPopupExtender  
      ID="mpedit" runat="server" 
      PopupControlID="myModal"
      CancelControlID="btclose"    
      BackgroundCssClass="panel-reveal-modal-bg"  
      TargetControlID="manualHide"
      >
    </asp:ModalPopupExtender>

    <asp:Panel ID="myModal" runat="server" CssClass="panel-reveal-modal"  >
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
         <script src="../Scripts/jquery.datetimepicker.js" type="text/javascript"></script>
    <script type="text/javascript">

        function popupCallback(objeto, catalogo) {
            if (objeto == null || objeto==undefined) {
               alertify. alert('Hubo un problema al setear un objeto de catalogo');
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
                document.getElementById('exporID').value = objeto.shiper;
               
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
            document.getElementById('exporID').value = '';
            return;
        }
        function checkData() {
            if (document.getElementById('nbrboo').value == '...' || document.getElementById('nbrboo').value == '') {
                alertify.alert('Seleccione el número de booking');
                return false;
            }
            if (document.getElementById('txtqty').value == '' || isNaN(document.getElementById('txtqty').value) || document.getElementById('txtqty').value == '0' ) {
                alertify.alert('Por favor escriba la cantidad para la proforma');
                return false;
            }
            if (parseInt(document.getElementById('txtqty').value) > parseInt(document.getElementById('bkqty').value)) {
                alertify.alert('La cantidad de la proforma no debe superar las reservas del booking');
                return false;
            }

                //verficar que tenga valor el campo fecha. ok
                var etd = document.getElementById('bketd').value;
            
                var cut = document.getElementById('bkcutof').value;
                var cli = document.getElementById('txdate').value; //
                if (etd == null || etd == '') {
                    alertify.alert('La fecha estimada de zarpe de la nave no ha sido establecida, comuníquese urgente con planeamiento');
                    return false;
                }
                if (cut == null || cut == '') {
                    alertify.alert('La fecha de CutOff de la nave no ha sido establecida, comuníquese urgente con planeamiento');
                    return false;
                }
                
                if (cli == null || cli == '') {
                    alertify.alert('La fecha estimada de ingreso es un campo obligatorio');
                    return false;
                }
                var res = comprobarFecha(cli, cut);
                if (!res) {
                    alertify.alert('La fecha estimada de ingreso [' + cli + '] no puede ser mayor a fecha de corte [CutOff: ' + cut + ']');
                    return false;
                }
                var cbo = document.getElementById('dpiva').value;
                if (cbo == null || cbo == '' || cbo=='Z' || cbo == undefined) {
                    alertify.alert('Por favor seleccione el porcentaje de retención del IVA');
                    return false;
                }
                cbo = document.getElementById('dpfte').value;
                if (cbo == null || cbo == '' || cbo == 'Z' || cbo == undefined) {
                  alertify.alert('Por favor seleccione el porcentaje de retención de la fuente');
                    return false;
                }
         
            return true;
        }
        $(document).ready(function () {
            //inicia los fecha
            $('.datepicker').datetimepicker({ lang: 'es', timepicker: true, format: 'd/m/Y H:i', closeOnDateSelect: true, minDate: '0' });
        });

        function darValor(control,control2, destino) {
            document.getElementById(destino).value = control.value;
            document.getElementById(control2).textContent = '';
        }

    </script>
  </asp:Content>
