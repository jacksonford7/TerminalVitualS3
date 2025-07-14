<%@ Page Title="Emisión e-Pass ZAL" Language="C#" MasterPageFile="~/site.Master" AutoEventWireup="true" CodeBehind="proforma_zal.aspx.cs" Inherits="CSLSite.zal.proforma_zal" %>
<%@ MasterType VirtualPath="~/site.Master" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="placehead" runat="server">
    <link href="../shared/estilo/tables.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/aisv-general.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/work.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/catalogos.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/info.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/w3-progressbar.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/jquery-ui.css" rel="stylesheet" type="text/css" />
    <link href="../shared/avisos/ppt.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/proforma.css" rel="stylesheet" type="text/css" />

    <link href="../img/favicon2.png" rel="icon"/>
    <link href="../img/icono.png" rel="apple-touch-icon"/>
    <link href="../css/bootstrap.min.css" rel="stylesheet"/>
    <link href="../css/dashboard.css" rel="stylesheet"/>
    <link href="../css/icons.css" rel="stylesheet"/>
    <link href="../css/style.css" rel="stylesheet"/>
    <link href="https://fonts.googleapis.com/css2?family=Roboto:wght@300;400;500;700&display=swap" rel="stylesheet"/>

    <link href="../shared/estilo/Reset.css" rel="stylesheet" />
    <link href="../shared/estilo/jquery.datetimepicker.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/tables.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/info.css" rel="stylesheet" type="text/css" />


    <script src="../Scripts/turnos.js" type="text/javascript"></script>
    <script src="../Scripts/credenciales.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.min.js" type="text/javascript"></script>
    <script src="../Scripts/jquery-ui.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        //        $(document).ready(function () {
        //            $('#tablasort').tablesorter({ cancelSelection: true, cssAsc: "ascendente", cssDesc: "descendente", headers: { 5: { sorter: false }, 6: { sorter: false}} }).tablesorterPager({ container: $('#pager'), positionFixed: false, pagesize: 10 });
        //        });
  
    </script>
    <style type="text/css">
.cal_Theme1 .ajax__calendar_container   {
background-color: #DEF1F4;
border:solid 1px #77D5F7;
}

.cal_Theme1 .ajax__calendar_header  {
background-color: #ffffff;
margin-bottom: 4px;
}

.cal_Theme1 .ajax__calendar_title,
.cal_Theme1 .ajax__calendar_next,
.cal_Theme1 .ajax__calendar_prev    {
color: #004080;
padding-top: 3px;
}

.cal_Theme1 .ajax__calendar_body    {
background-color: #ffffff;
border: solid 1px #77D5F7;
}

.cal_Theme1 .ajax__calendar_dayname {
text-align:center;
font-weight:bold;
margin-bottom: 4px;
margin-top: 2px;
color: #004080;
}

.cal_Theme1 .ajax__calendar_day {
color: #004080;
text-align:center;
}

.cal_Theme1 .ajax__calendar_hover .ajax__calendar_day,
.cal_Theme1 .ajax__calendar_hover .ajax__calendar_month,
.cal_Theme1 .ajax__calendar_hover .ajax__calendar_year,
.cal_Theme1 .ajax__calendar_active  {
color: #004080;
font-weight: bold;
background-color: #DEF1F4;
}


.cal_Theme1 .ajax__calendar_today   {
font-weight:bold;
}

.cal_Theme1 .ajax__calendar_other,
.cal_Theme1 .ajax__calendar_hover .ajax__calendar_today,
.cal_Theme1 .ajax__calendar_hover .ajax__calendar_title {
color: #bbbbbb;
}
        .warning { background-color:Yellow;  color:Red;}

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
* input[type=text]
    {
        text-align:left!important;
        }
        
        .autocomplete_completionListElement
        {
            margin: 0px !important;
            background-color: inherit;
            color: windowtext;
            border: buttonshadow;
            border-width: 1px;
            border-style: solid;
            cursor: 'default';
            overflow: auto;
            height: auto;
            text-align: left;
            list-style-type: none;
        }
        
        /* AutoComplete highlighted item */
        .autocomplete_highlightedListItem
        {
            background-color: #ffff99;
            color: black;
            padding: 1px;
        }
        
        /* AutoComplete item */
        .autocomplete_listItem
        {
            background-color: window;
            color: windowtext;
            padding: 1px;
             z-index:2000 !important;
        }
        .AutoExtender
        {
            font-family: Verdana, Helvetica, sans-serif;
            font-size: xx-small;
            font-weight: normal;
            border: solid 1px #006699;
            line-height: 20px;
            padding: 10px;
            background-color: White;
            margin-left: 10px;
            z-index: 200000 !important;
        }
        .AutoExtenderList
        {
            border-bottom: dotted 1px #006699;
            cursor: pointer;
            /*color: Maroon;*/
            color:Black;
            font-style: normal;
            font-size: xx-small;
            font-family:Arial!important;
            z-index: 200000 !important;
        }
        .AutoExtenderHighlight
        {
            color: White;
            background-color: #006699;
            cursor: pointer;
            z-index: 200000 !important;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="placebody" runat="server">
    <input id="zonaid" type="hidden" value="1203" />
    <asp:ToolkitScriptManager ID="tkcal" EnableScriptGlobalization="true" runat="server"> </asp:ToolkitScriptManager>

    <div class="mt-4">         
        <nav class="mt-4" aria-label="breadcrumb">
            <ol class="breadcrumb">
            <li class="breadcrumb-item" id="opcion_principal" runat="server"><a href="#">Depósito de Vacíos</a></li>
                <li class="breadcrumb-item active" aria-current="page" id="sub_opcion" runat="server">Emisión e-Pass Zal/CISE/ZEA</li>
            </ol>
        </nav>
    </div>

    <table class="controles" style=" display:none"  cellspacing="0" cellpadding="0">
    <tr>
        <td "bt-bottom  bt-right bt-left">
        <div class="msg-critico" style=" font-weight:bold">
        Estimado Cliente;
        Al registrar la Proforma se generarán los pases correspondientes.
        </div>
        </td>
        </tr>
    </table>

<%--    <div class="alert alert-warning">
        <span id="dtlo" runat="server">Estimado Cliente:</span> 
        <br /> Al registrar la Proforma se generarán los pases correspondientes.
    </div>--%>


    <div class="dashboard-container p-4" id="cuerpo" runat="server">
        <div class="form-row">
            <div class="form-group col-md-12"> 

                <div class="cataresult" >
                   <asp:UpdatePanel ID="UpdatePanel2" runat="server" ChildrenAsTriggers="true">
                         <ContentTemplate>
                           <input id="Hidden3" type="hidden" runat="server" clientidmode="Static" />
                           <input id="Hidden4" type="hidden" runat="server" clientidmode="Static" />
                      </ContentTemplate>
                         <Triggers>
                             <asp:AsyncPostBackTrigger ControlID="btnconsultarturnos" />
                         </Triggers>
                     </asp:UpdatePanel>
                 </div>
            </div>
        </div>

         <input id="agencia" type="hidden" runat="server"  clientidmode="Static"/>

        <div class="form-title">
            DATOS DE CONSULTA PARA EMISIÓN DE E-PASS: 
        </div>
        
        <div >

            <div class="form-row" >
                <div class="form-group col-md-3"> 
                    <label for="inputAddress">Deposito :<span style="color: #FF0000; font-weight: bold;"></span></label>
                    <asp:DropDownList ID="cmbDeposito" class="form-control" runat="server" Enabled="false" Font-Size="Medium"  AutoPostBack="true" Font-Bold="true" OnSelectedIndexChanged="cmbDeposito_SelectedIndexChanged"></asp:DropDownList>
                    <a class="tooltip" ><span class="classic" >Ruta asociada al servicio</span><img alt='' src='../shared/imgs/info.gif' class='datainfo'/></a>
                </div>

                <div class="form-group   col-md-3"> 
                    <label for="inputAddress">Booking No. :<span style="color: #FF0000; font-weight: bold;"></span></label>

                    <div class="d-flex">
                        <span id="numbook"  class="form-control" onclick="clear();">...</span> 
                        <a  class="btn btn-outline-primary mr-4" target="popup" onclick="openPopup()"> <span class='fa fa-search' style='font-size:24px'></span>  </a>
                    </div>
                     <input id="nbrboo" type="hidden" value="" runat="server" clientidmode="Static"/>               
                </div>

                <div class="form-group   col-md-3"> 
                    <label for="inputAddress">Referencia :<span style="color: #FF0000; font-weight: bold;"></span></label>

                    <div class="d-flex">
                       <span id="referencia" runat="server" clientidmode="Static" class="form-control" >...</span>
                       <input id="xreferencia" type="hidden" value="" runat="server" clientidmode="Static"/>
                    </div>
                </div>

                <div class="form-group   col-md-3"> 
                    <label for="inputAddress">Linea :<span style="color: #FF0000; font-weight: bold;"></span></label>

                    <div class="d-flex">
                       <span id="linea" runat="server" clientidmode="Static" class="form-control" style=" width:100%!important;" >...</span>
                       <input id="xlinea" type="hidden" value="" runat="server" clientidmode="Static"/>       
                    </div>
                </div>
                
                <div class="form-group   col-md-3"> 
                    <label for="inputAddress">Cantidad(QTY)<span style="color: #FF0000; font-weight: bold;"></span></label>
                    <span id="nbqty" runat="server" clientidmode="Static" class="form-control" style=" width:100%!important;">...</span>
                </div>

                <div class="form-group   col-md-3"> 
                    <label for="inputAddress">Reserva<span style="color: #FF0000; font-weight: bold;"></span></label>
                    <span id="cantr" runat="server" clientidmode="Static" class="form-control" style=" width:100%!important;">...</span>
                </div>

                <div class="form-group   col-md-3"> 
                    <label for="inputAddress">Despacho<span style="color: #FF0000; font-weight: bold;"></span></label>
                    <span id="cantd" runat="server" clientidmode="Static" class="form-control" style=" width:100%!important;">...</span>
                </div>

                <div class="form-group   col-md-3"> 
                    <label for="inputAddress">Disponible<span style="color: #FF0000; font-weight: bold;"></span></label>
                    <span id="cants" runat="server" clientidmode="Static" class="form-control" style=" width:100%!important;">...</span>
                </div>

                <input id="bkqty" type="hidden" value="" runat="server" clientidmode="Static"/>
                <input id="resqty" type="hidden" value="" runat="server" clientidmode="Static"/>
                <input id="desqty" type="hidden" value="" runat="server" clientidmode="Static"/>
                <input id="salqty" type="hidden" value="" runat="server" clientidmode="Static"/>
                <input id="ruc" type="hidden" value="" runat="server" clientidmode="Static"/>
                <input id="nave" type="hidden" value="" runat="server" clientidmode="Static"/>

            </div>
        </div>

        <div><br /></div>

        <div >

            <div class="form-row" >
                <div class="form-group   col-md-6"> 
                    <label for="inputAddress">Chofer<span style="color: #FF0000; font-weight: bold;"></span></label>
                    <asp:TextBox ID="txtchofer" runat="server" class="form-control" onkeyup="searchChofer(this)" onblur="fTittleChofer(this)"
                    onkeypress="return soloLetras(event,'abcdefghijklmnopqrstuvwxyz1234567890 -',true)" placeholder="DIGITE EL NOMBRE O LA LICENCIA"></asp:TextBox>
                </div>
                <div class="form-group   col-md-6"> 
                    <label for="inputAddress">Placa<span style="color: #FF0000; font-weight: bold;"></span></label>
                    <asp:TextBox ID="txtplaca" runat="server" class="form-control"
                    onkeypress="return soloLetras(event,'abcdefghijklmnopqrstuvwxyz1234567890',true)" placeholder="DIGITE LA PLACA"></asp:TextBox>
                </div>
                <div class="form-group   col-md-6"> 
                    <label for="inputAddress">Cantidad Solicitada<span style="color: #FF0000; font-weight: bold;"></span></label>
                    <asp:TextBox ID="txtqty" runat="server" MaxLength="3" class="form-control"
                      onkeypress="return soloLetras(event,'1234567890',true)" 
                      placeholder="CNTR QTY"
                     ></asp:TextBox>
                </div>

                <div class="form-group   col-md-6"> 
                    <label for="inputAddress">Fecha Salida<span style="color: #FF0000; font-weight: bold;"></span></label>
                    <asp:TextBox runat="server" ID="fecsalida" AutoPostBack="false" MaxLength="10"  
                         onkeypress="return soloLetras(event,'0123456789/')" class="form-control"></asp:TextBox>

                        <asp:CalendarExtender ID="CAGTFECHAFASA"  runat="server"
                            CssClass="cal_Theme1" Format="dd/MM/yyyy" TargetControlID="fecsalida">
                        </asp:CalendarExtender>
                </div>
            </div>


            <div class="row">

                <div class="col-md-12 d-flex justify-content-center">
                    <asp:Button class="btn btn-primary" Text="Consultar Horarios"  OnClientClick="return fchange()" OnClick="btnconsultarturnos_Click" runat="server" id="btnconsultarturnos"/>
                    <img alt="loading.." src="../lib/file-uploader/img/loading.gif" id="imgfecha" class="nover"   height="32px" width="32px"/>
                </div>

            </div>
             
        </div>

        

        <div> 
                <label for="inputAddress">Horarios<span style="color: #FF0000; font-weight: bold;"></span></label>
        </div>

        <div >
            <div class="form-row">
                <div class="form-group col-md-12"> 
                    <div class="bokindetalle" style=" width:100%; overflow:auto">
                        <div class="cataresult" >
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                <ContentTemplate>

                                    <div class="form-group col-md-12"> 
                                        <div style=" width:100%; height:250px; overflow-y:scroll"  id="divscroll">
                                            <asp:CheckBoxList BackColor="LightGray"  ID="cblturnos" class="form-control" runat="server"  OnSelectedIndexChanged="cblturnos_SelectedIndexChanged"   AutoPostBack="true"></asp:CheckBoxList>
                                        </div>
                                        <asp:Label Text="" ID="lbltotturnos" runat="server" Font-Bold="true" /> 
                                    </div>
           
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        

        <div class="form-row">
             <div class="form-group col-md-12"> 
                <div class="bokindetalle" style=" width:100%; overflow:auto">
                    <div class="cataresult" >
                        <asp:UpdatePanel ID="up3" runat="server">
                                <ContentTemplate>
                                                            
                                        <asp:Repeater ID="tableDetTurnos" runat="server" >
                                                <HeaderTemplate>
                                                <table id="tablasort" cellspacing="1" cellpadding="1" class="table table-bordered invoice">
                                                <thead>
                                                <div class=" bt-top">
                                                    <th style="  font-size:small">Horario</th>
                                                    <th style="font-size:small">Chofer</th>
                                                    <th style="  font-size:small">Placa</th>
                                                </div>
                                                </thead> 
                                                <tbody>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                <tr class="point">
                                                <td style="  font-size:small"><%#Eval("Inicio")%></td>
                                                <td>
                                                    <asp:Label Text='<%#Eval("CHOFER")%>' Font-Size="Small" ID="lblchof" Width="585px" runat="server" /> </td>
                                                <td style=" font-size:small"><%#Eval("PLACA")%></td>
                                                </tr>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                </tbody>
                                                </table>
                                                </FooterTemplate>
                                        </asp:Repeater>

                                                     
                                </ContentTemplate>
                            </asp:UpdatePanel>
                    </div>
                </div>
            </div>
        </div>

        <div class="form-row">
             <div class="form-group col-md-12"> 
                    <div class="cataresult" >
                        <asp:UpdatePanel ID="uptablaProforma" runat="server" ChildrenAsTriggers="true" >
                            <ContentTemplate>
                                <div id="xfinder" runat="server" visible="false" >
                                <div class="alert alert-warning" id="alerta" runat="server"  >Notas y descripciones sobre la  proforma</div> 
                                <div class="findresult" >
                                    <div class="booking" >
                                        
                                        
                                            <div class="form-group col-md-12"> 
                                                <div class="form-title">Detalle De Valores a Cancelar [Proforma]</div>
                                            </div>

                                           <div class="bokindetalle" style=" width:100%; overflow:auto">
                                                <fieldset>
                                                        <asp:Repeater ID="tablaNueva" runat="server" >
                                                            <HeaderTemplate>
                                                            <table id="miProforma"  cellpadding="1" cellspacing="1" class="table table-bordered invoice">
                                                            <thead>
                                                                <tr><th>Código</th><th>Descripcion</th><th>Cant.</th><th>P.Unit</th><th>P.Total</th></tr>
                                                            </thead>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                            <tr class="point" >
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
                                                    <table class="table table-bordered invoice" cellpadding="0" cellspacing="0">
                                                   <%-- <tr><td  class='filat'>Subtotal:</td><td class="estotal"><span id='stunit' runat="server" clientidmode="Static"  >$0000.00</span></td></tr>
                                                    <tr><td  class='filat'> <span runat="server" clientidmode="Static" id="etiIva">IVA %(+):</span> </td><td class="estotal"><span id='siva' runat="server" clientidmode="Static"  >$0000.00</span></td></tr>
                                                     <tr><td  class='filat'> <span runat="server" clientidmode="Static" id="etisrfte">Ret.Fte %(-):</span> </td><td class="estotal"><span id='srfte' runat="server" clientidmode="Static"  >$0000.00</span></td></tr>
                                                     <tr><td  class='filat'> <span runat="server" clientidmode="Static" id="stisriva">Ret.IVA %(-):</span> </td><td class="estotal"><span id='sriva' runat="server" clientidmode="Static"  >$0000.00</span></td></tr>--%>
                                                    <tr><td colspan="3"  ></td></tr>
                                                    <tr><td  class='border-none text-right font-weight-bold'><strong>SUBTOTAL:</strong></td><td class="estotal"><span id='stsubtotal' runat="server" clientidmode="Static" ><strong>$0000.00</strong></span></td></tr>
                                                    <tr><td  class='border-none text-right text-info font-weight-bold'> <strong>IVA:</strong></td><td class="estotal"><span id='stiva' style="color:blue;" runat="server" clientidmode="Static" ><strong>$0000.00</strong></span></td></tr>
                                                    <tr><td  class='border-none text-right font-weight-bold'><strong>NETO A PAGAR:</strong></td><td class="estotal"><span id='sttal' runat="server" clientidmode="Static" ><strong>$0000.00</strong></span></td></tr>
                                                    </table>
                                                </fieldset>
                                            </div>
               
                                     </div>
                                 </div>
                              </div>
                                <div id="sinresultado" runat="server" class="msg-info"></div>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger  ControlID="btnAsumirBook"/>
                                <asp:AsyncPostBackTrigger  ControlID="btnconsultarturnos"/>
                             </Triggers>
                         </asp:UpdatePanel>
                    </div>
                
            </div>
        </div>
         <%--<div class="botonera" runat="server" id="div3">
                 <img alt="loading.." src="../shared/imgs/loader.gif" id="imggenpase" class="nover"  />
                 <asp:Button ID="btnAsumirBook" runat="server" Text="Generar Pases"  OnClientClick="return prepareObject('¿Estimado Cliente, está seguro de generar el(los) pase(s).?')"
                   ToolTip="Agrega los turnos seleccionados a la tabla para luego poderlos reservar." OnClick="btnAsumirBook_Click"/>
         </div>--%>

        <div class="row">
            <div class="col-md-12 d-flex justify-content-center" runat="server" id="div3">
                <img alt="loading.." src="../shared/imgs/loader.gif" id="imggenpase" class="nover"  />
                 <asp:Button class="btn btn-primary" ID="btnAsumirBook" runat="server" Text="Generar Pases"  OnClientClick="return prepareObject('¿Estimado Cliente, está seguro de generar el(los) pase(s).?')"
                   ToolTip="Agrega los horarios seleccionados a la tabla para luego poderlos reservar." OnClick="btnAsumirBook_Click"/>
            </div>
        </div>

         

        <div class="form-row">
             <div class="form-group col-md-12"> 
                
                    <div class="cataresult" >
                         <asp:UpdatePanel ID="UpdatePanel3" runat="server" ChildrenAsTriggers="true">
                                 <ContentTemplate> 

                                     <div class="bokindetalle" style=" width:100%; overflow:auto">

                                         <asp:Repeater ID="RepeaterBooking" runat="server" 
                                             onitemcommand="RepeaterBooking_ItemCommand" >
                                         <HeaderTemplate>
                                         <table id="tablasortbook" cellspacing="1" cellpadding="1" class="table table-bordered invoice">
                                         <thead>
                                         <tr>
                                         <%--<th class="xax" >Booking</th>--%>
                                         <th style=" width:150px">Booking</th>
                                         <th style=" width:150px">Referencia</th>
                                         <th style=" width:50px">Linea</th>
                                         <th style=" width:250px">Mensaje</th>
                                         <th></th>
                 
                                         </tr>
                                         </thead> 
                                         <tbody>
                                         </HeaderTemplate>
                                         <ItemTemplate>
                                          <%--<tr class="point" onclick="setObjectCliente(this);">--%>
                                          <%--<td  class="xax" ><%#Eval("CODIGO_SAP")%></td>--%>
                                          <td style=" width:150px"><%#Eval("BOOKING")%></td>
                                          <td style=" width:150px"><%#Eval("REFERENCIA")%></td>
                                          <td style=" width:50px"><%#Eval("LINEA")%></td>
                                          <td style=" width:250px"><%#Eval("MENSAJE")%></td>
                                          <td style=" width:50px"><asp:Button Text="Remover" ID="btnRemoveBook" OnClick="btnRemove_Click" CommandName="Delete" runat="server"/></td>
                                          <td></td>
                                         </tr>
                                         </ItemTemplate>
                                         <FooterTemplate>
                                         </tbody>
                                         </table>
                                         </FooterTemplate>
                                         </asp:Repeater>

                                    </div>

                                 </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="btnAsumirBook" />
                                </Triggers>
                         </asp:UpdatePanel>
                    </div>
                
            </div>
        </div>
         

         
        <%-- <div class="botonera" runat="server" id="btnera" style=" display:none">
              <img alt="loading.." src="../shared/imgs/loader.gif" id="loader" class="nover"  />
              <asp:Button ID="btsalvar" runat="server" Text="Generar Proforma"  onclick="btsalvar_Click"
                   OnClientClick="return prepareObject('¿Estimado Cliente, está seguro de reservar los Turnos?')"
                   ToolTip="Reservar Turnos."/>
         </div>--%>
        
        <div class="row" runat="server" id="btnera" style=" display:none">
            <div class="col-md-12 d-flex justify-content-center">
                <img alt="loading.." src="../shared/imgs/loader.gif" id="loader" class="nover"  />
                <asp:Button class="btn btn-primary" ID="btsalvar" runat="server" Text="Generar Proforma"  onclick="btsalvar_Click"
                   OnClientClick="return prepareObject('¿Estimado Cliente, está seguro de reservar los Horarios?')"
                   ToolTip="Reservar Horarios."/>
            </div>
        </div>

    </div>

    <table class="controles" cellspacing="0" cellpadding="0" style=" display:none">
        <tr>
            <td "bt-bottom  bt-right bt-left">
                <div class="alert alert-warning" style=" font-weight:bold">
                Estimado Cliente;
                Al procesar la Proforma, se habilita que los mismos puedan despacharse en horarios después de las 17h30.
                </div>
            </td>
        </tr>
    </table>
<%--  </div>--%>
    <asp:HiddenField ID="hfChoferId" runat="server" />
    <asp:HiddenField runat="server" ID="hfRucBuscar" />
    <script src="../Scripts/pages.js" type="text/javascript"></script>
    <script src="../Scripts/credenciales.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.tablesorter.min.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.tablesorter.pager.js" type="text/javascript"></script>

    <script type="text/javascript">

        $(document).ready(function () {
            $('.datetimepicker').datetimepicker().datetimepicker({ lang: 'es', timepicker: false, closeOnDateSelect: true, format: 'd/m/Y' });
        });
        $TableFilter = function (id, value) {
            var rows = document.querySelectorAll(id + ' tbody tr');

            for (var i = 0; i < rows.length; i++) {
                var showRow = false;

                var row = rows[i];
                row.style.display = 'none';

                for (var x = 0; x < row.childElementCount; x++) {
                    if (row.children[x].textContent.toLowerCase().indexOf(value.toLowerCase().trim()) > -1) {
                        showRow = true;
                        break;
                    }
                }

                if (showRow) {
                    row.style.display = null;
                }
            }
        }
        function autocompleteClientShown(source, args) {
            source._popupBehavior._element.style.height = "130px";
        }
        function searchChofer(idval) {
            $(idval).autocomplete({
                source: function (request, response) {
                    $.ajax({
                        type: "POST",
                        url: "proforma_zal.aspx/GetChoferList",
                        data: "{ 'prefix': '" + idval.value + "'}",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        async: false,
                        success: function (data) {
                            response($.map(data.d, function (item) {
                                return {
                                    label: item.split('-')[0].trim() + ' - ' + item.split('-')[1].trim(),
                                    val: item.split('-')[0].trim()
                                }
                            }))
                        },
                        error: function (response) {
                            alert(response.responseText);
                        },
                        failure: function (response) {
                            alert(response.responseText);
                        }
                    });
                },
                select: function (e, i) {
                    $("[id$=hfChoferId]").val(i.item.val);
                },
                minLength: 1
            });
        }

        //        function getGifOcultaBuscar() {
        //            //document.getElementById('loader3').className = 'nover';

        //            return true;
        //        };
        function getGifOculta() {
            //document.getElementById('loader2').className = 'nover';


            document.getElementById('referencia').textContent = "";
            document.getElementById('xreferencia').value = "";

            document.getElementById('numbook').textContent = "";
            document.getElementById('nbrboo').value = "";

            document.getElementById('linea').textContent = "";
            document.getElementById('xlinea').value = "";

            return true;
        }

        //        function getGifOcultaEnviar(mensaje) {
        //            document.getElementById('loader').className = 'nover';
        //            alert(mensaje);
        //            return true;
        //        }
        //        function getBookOculta() {
        //            //document.getElementById('loader2').className = 'nover';
        //            return true;
        //        }
        var programacion = {};
        var lista = [];
        function prepareObjectTable(mensaje) {

            try {
                if (confirm(mensaje) == false) {
                    return false;
                }
                document.getElementById("loader2").className = '';
                lista = [];
                //validaciones básicas
                var vals = document.getElementById('xreferencia');
                if (vals == null || vals == undefined || vals.value == '') {
                    alert('¡ Seleccione la Referencia.');
                    document.getElementById("loader2").className = 'nover';
                    document.getElementById("buscarref").focus();
                    return false;
                }

                var vals = document.getElementById('nbrboo');
                if (vals == null || vals == undefined || vals.value.trim().length <= 2) {
                    alert('¡ Seleccione el numero de Booking que pertenece al Cliente.');
                    document.getElementById("loader2").className = 'nover';
                    return false;
                }
                return true;
            } catch (e) {
                alert('Lamentamos comunicarle que ha ocurrido el siguiente problema:\n' + e.Message);
            }
        };

        //        function prepareObjectTableRemove(mensaje) {

        //            try {
        //                if (confirm(mensaje) == false) {
        //                    return false;
        //                }
        //                return true;
        //            } catch (e) {
        //                alert('Lamentamos comunicarle que ha ocurrido el siguiente problema:\n' + e.Message);
        //            }
        //        };

        //        function prepareObjectRuc() {
        //            try {
        //                document.getElementById("loader3").className = '';
        //                
        //                return true;
        //            } catch (e) {
        //                alert('Lamentamos comunicarle que ha ocurrido el siguiente problema:\n' + e.Message);
        //            }
        //        }


        function add(button) {
            var row = button.parentNode.parentNode;
            var cells = row.querySelectorAll('td:not(:last-of-type)');
            addToCartTable(cells);
        }

        function remove() {
            var row = this.parentNode.parentNode; demofab
            document.querySelector('#target tbody')
            .removeChild(row);
        }

        function addToCartTable(cells) {
            var code = cells[1].innerText;
            var name = cells[2].innerText;
            var price = cells[3].innerText;

            var newRow = document.createElement('tr');

            newRow.appendChild(createCell(code));
            newRow.appendChild(createCell(name));
            newRow.appendChild(createCell(price));
            var cellInputQty = createCell();
            cellInputQty.appendChild(createInputQty());
            newRow.appendChild(cellInputQty);
            var cellRemoveBtn = createCell();
            cellRemoveBtn.appendChild(createRemoveBtn())
            newRow.appendChild(cellRemoveBtn);

            document.querySelector('#target tbody').appendChild(newRow);
        }

        function createInputQty() {
            var inputQty = document.createElement('input');
            inputQty.type = 'number';
            inputQty.required = 'true';
            inputQty.min = 1;
            inputQty.className = 'form-control'
            return inputQty;
        }

        function createRemoveBtn() {
            var btnRemove = document.createElement('button');
            btnRemove.className = 'btn btn-xs btn-danger';
            btnRemove.onclick = remove;
            btnRemove.innerText = 'Descartar';
            return btnRemove;
        }

        function createCell(text) {
            var td = document.createElement('td');
            if (text) {
                td.innerText = text;
            }
            return td;
        }

        function clear() {
            document.getElementById('').textContent = '...';
            document.getElementById('').value = '';
        }

        var programacion = {};
        var lista = [];
        function prepareObject(mensaje) {

            try {
                if (confirm(mensaje) == false) {
                    return false;
                }
                document.getElementById("imggenpase").className = '';

                return true;
            } catch (e) {
                alert('Lamentamos comunicarle que ha ocurrido el siguiente problema:\n' + e.Message);
            }
        }

        function fchange() {

            try {
                document.getElementById("imgfecha").className = '';
            } catch (e) {
                alert('Lamentamos comunicarle que ha ocurrido el siguiente problema:\n' + e.Message);
            }
        }

        function ocultagiffecha() {
            document.getElementById('imgfecha').className = 'nover';
        }

        function ocultagifloader() {
            document.getElementById('imggenpase').className = 'nover';
        }

        function reload() {
            location.reload(true);
        }
        //        function openPop() {
        //            var bo = document.getElementById('nbrboo').value;
        //            if (bo == undefined || bo == '' || bo == null) {
        //                alert('Por favor seleccione el booking primero');
        //                return;
        //            }
        //            window.open('../catalogo/Calendario.aspx?bk='+bo, 'name', 'width=850,height=480')
        //        }

        //        function setObjectRef(row) {
        //            var celColect = row.getElementsByTagName('td');
        //            var catalogo = {
        //                referencia: celColect[0].textContent,
        //                nave: celColect[1].textContent,
        //            };
        //            popupCallbackRef(catalogo);
        //        }

        function popupCallback(objeto, catalogo) {

            document.getElementById('numbook').textContent = objeto.nbr;
            document.getElementById('nbrboo').value = objeto.nbr;
            var a = objeto.line.split("-");
            document.getElementById('referencia').textContent = a[0].toString();
            document.getElementById('xreferencia').value = a[0].toString(); ;
            document.getElementById('linea').textContent = a[1].toString();
            document.getElementById('xlinea').value = a[1].toString();
            document.getElementById('ruc').value = objeto.ruc;
            document.getElementById('nbqty').textContent = objeto.cant_bkg;
            document.getElementById('bkqty').value = objeto.cant_bkg;
            document.getElementById('cantr').textContent = objeto.reservado;
            document.getElementById('resqty').value = objeto.reservado;
            document.getElementById('cantd').textContent = objeto.despachado;
            document.getElementById('desqty').value = objeto.despachado;
            document.getElementById('cants').textContent = objeto.cant_bkg - objeto.reservado - objeto.despachado;
            document.getElementById('salqty').value = objeto.cant_bkg - objeto.reservado - objeto.despachado
            document.getElementById('nave').value = objeto.nave;
            return;

        }
        function openPopup() {
            //var ref = document.getElementById('xreferencia').value;
            //var ruc = document.getElementById('xruc').value;
            //window.open('../mantenimientos_proforma_expo/autoriza-booking', 'name', 'width=1000,height=480');
            window.open('../catalogo/bookinZAL.aspx', 'name', 'width=850,height=880');
            //window.location = '../facturacion/emision-pase-de-puerta';
            return;
        }
        function openPopReporte(opcion) {
            window.open('../zal/wbareportezal.aspx', 'name', 'width=700,height=880');
            //window.location = '../facturacion/emision-pase-de-puerta';
            return true;
        }

         function openMnesaje(opcion) {
            window.open('mensaje.html', 'name', 'width=590,height=500');
            //window.location = '../facturacion/emision-pase-de-puerta';
            return true;
        }

        function mySetValue() {
            document.getElementById('numbook').textContent = document.getElementById('nbrboo').value;
            document.getElementById('referencia').textContent = document.getElementById('xreferencia').value;
            document.getElementById('linea').textContent = document.getElementById('xlinea').value;
            document.getElementById('nbqty').textContent = document.getElementById('bkqty').value;
            document.getElementById('cantr').textContent = document.getElementById('resqty').value;
            document.getElementById('cantd').textContent = document.getElementById('desqty').value;
            document.getElementById('cants').textContent = document.getElementById('salqty').value;
        }
        function fTittleChofer(idval) {
            idval.title = idval.value;
        }
    </script>

   

<%--<asp:updateprogress  id="updateProgress" runat="server">
    <progresstemplate>
        <div id="progressBackgroundFilter"></div>
        <div id="processMessage"> Estamos procesando la tarea que solicitó, por favor  espere...<br />
         <img alt="Loading" src="../shared/imgs/loader.gif" style="margin:0 auto;" />
        </div>
    </progresstemplate>
</asp:updateprogress>--%>
  </asp:Content>
