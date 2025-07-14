<%@ Page Title="Actualización, Cancelación, Reimpresión e-Pass ZAL" Language="C#" 
    MasterPageFile="~/site.Master" AutoEventWireup="true" 
    CodeBehind="pases_zal.aspx.cs" Inherits="CSLSite.zal.pases_zal" %>
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
    <script src="../Scripts/turnos.js" type="text/javascript"></script>
    <script src="../Scripts/credenciales.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.min.js" type="text/javascript"></script>
    <script src="../Scripts/jquery-ui.min.js" type="text/javascript"></script>

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
                <li class="breadcrumb-item active" aria-current="page" id="sub_opcion" runat="server">Actualización, Cancelación, Reimpresión e-Pass ZAL</li>
            </ol>
        </nav>
    </div>

    <table class="controles" style=" display:none" cellspacing="0" cellpadding="0">
    <tr>
        <td "bt-bottom  bt-right bt-left">
        <div class=" msg-critico" style=" font-weight:bold">
        Estimado Cliente;
        Al registrar la Proforma se generarán los pases correspondientes.
        </div>
        </td>
        </tr>
    </table>
   <%-- <div class="alert alert-warning">
        <span id="dtlo" runat="server">Estimado Cliente:</span> 
        <br /> Al registrar la Proforma se generarán los pases correspondientes.
    </div>--%>

    <div class="dashboard-container p-4" id="cuerpo" runat="server">
   
         <input id="agencia" type="hidden" runat="server"  clientidmode="Static"/>

        <div class="form-title">
            DATOS DE CONSULTA PARA CANCELACIÓN, REIMPRESION E-PASS: 
        </div>


        <div >
            <div class="form-row">
            
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

                <div class="form-group   col-md-2"> 
                    <label for="inputAddress">Referencia :<span style="color: #FF0000; font-weight: bold;"></span></label>

                    <div class="d-flex">
                       <span id="referencia" runat="server" clientidmode="Static" class="form-control" >...</span>
                       <input id="xreferencia" type="hidden" value="" runat="server" clientidmode="Static"/>
                    </div>
                </div>

                <div class="form-group   col-md-2"> 
                    <label for="inputAddress">Linea :<span style="color: #FF0000; font-weight: bold;"></span></label>

                    <div class="d-flex">
                       <span id="linea" runat="server" clientidmode="Static" class="form-control" >...</span>
                       <input id="xlinea" type="hidden" value="" runat="server" clientidmode="Static"/>
                    </div>
                </div>

                <div class="form-group   col-md-2"> 
                    <label for="inputAddress">Fecha Salida :<span style="color: #FF0000; font-weight: bold;"></span></label>

                    <div class="d-flex">
                       <asp:TextBox class="form-control"  runat="server" ID="fecsalida" AutoPostBack="false" MaxLength="10"
                         onkeypress="return soloLetras(event,'0123456789/')"></asp:TextBox>

                        <asp:CalendarExtender ID="CAGTFECHAFASA"  runat="server"
                            CssClass="cal_Theme1" Format="dd/MM/yyyy" TargetControlID="fecsalida">
                        </asp:CalendarExtender>
                    </div>
                </div>
            
            </div>

     
     <%--   <div class="botonera" runat="server" id="div3">
                     <img alt="loading.." src="../shared/imgs/loader.gif" id="loadersearch" class="nover"  />
                     <asp:Button ID="btnAsumirBook" runat="server" Text="Consultar Pases" OnClientClick="return prepareObject();"
                       ToolTip="Agrega los turnos seleccionados a la tabla para luego poderlos reservar." OnClick="btnAsumirBook_Click"/>
             </div>--%>

            <div class="row">
                <img alt="loading.." src="../shared/imgs/loader.gif" id="loader" class="nover"  />

                <div class="col-md-12 d-flex justify-content-center">
                     <img alt="loading.." src="../shared/imgs/loader.gif" id="loadersearch" class="nover"  />
                     <asp:Button class="btn btn-primary"  ID="btnAsumirBook" runat="server" Text="Consultar Pases" OnClientClick="return prepareObject();"
                       ToolTip="Agrega los horarios seleccionados a la tabla para luego poderlos reservar." OnClick="btnAsumirBook_Click"/>                   

                    <span id="imagen"></span>
                </div>

            </div>

        </div>

        <div><br /></div>

         <asp:UpdatePanel ID="UdSaldo" runat="server" ChildrenAsTriggers="true">
         <ContentTemplate>   
          <div class="alert alert-warning" runat="server" id="mensaje" style=" font-weight:bold">
                    <span id="saldo" runat="server">Estimado Cliente,</span>
           </div>  

         </ContentTemplate>
            <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnAsumirBook" />
                </Triggers>
         </asp:UpdatePanel>

        <div >
            
   
            <div class="form-row">
                <div class="form-group col-md-12"> 
                    <%--<div class="bokindetalle" style=" width:100%; overflow:auto">--%>
                        <div class="cataresult" >
                            <asp:UpdatePanel ID="UpdatePanel3" runat="server" ChildrenAsTriggers="true">
                 
                                <ContentTemplate> 

                                   
                                        <div class="findresult" >
                                            <div class="booking" >
                                                <div class="alert alert-warning" runat="server" id="Div1" style=" font-weight:bold">
                                                    <span>Estimado Cliente,</span><br />
                                                    <span>Si desea actualizar los datos del Chofer o la Placa asociada a un Pase, ubíquese en la caja de texto modifique los datos y luego de clic en el botón [ Actualizar Pase </span>
                                                    <asp:ImageButton ImageUrl="~/shared/imgs/edita.png" ID="ImageButton1" runat="server" /><span>]</span>
                                                </div>

                                                <div class="bokindetalle" style=" width:100%; overflow:auto">
                                                    <asp:Repeater ID="RepeaterBooking" runat="server" 
                                                        onitemcommand="RepeaterBooking_ItemCommand"
                                                        OnItemDataBound="RepeaterBooking_ItemDataBound">
                                                        <HeaderTemplate>
                 
                                                            <table id="tablasortbook" cellspacing="1" cellpadding="1" class="table table-bordered invoice">
                  
                                                                <thead>
                                                                    <tr>
                                                                    <th id="pase" style=" width:100px"># Pase</th>
                                                                        <th id="deposito"><asp:Label Text='Deposito' ID="Label1" runat="server" visible="true"/></th>
                                                                    <th id="fecha" style=" width:80px">Fecha de Salida</th>
                                                                    <th id="turno" style=" width:70px">Horario</th>
                                                                    <th id="boocking" style=" width:150px; display:none">Booking</th>
                                                                    <th id="referencia" style=" width:150px; display:none">Referencia</th>
                                                                    <th id="linea" style=" width:50px; display:none">Linea</th>
                                                                    <th id="chofer">Chofer</th>
                                                                    <th id="placa">Placa</th>
                                                                    <th id="liquidacion"><asp:Label Text='Liquidación' ID="lbl_tit_liquidacion" runat="server" visible="true" /></th>
                                                                    <th id="estado"><asp:Label Text='Estado' ID="lbl_tit_estado" runat="server" visible="true" /></th>
                                                                    <th id="actualizar" >Actualizar Pase</th>
                                                                    <th id="cancelar">Cancelar Pase</th>
                                                                    <th id="imprimir">Imprimir Pase</th>
                                                                    <th id="imprimir_prof"><asp:Label Text='Imprimir Liquidación' ID="lbl_tit_imprimir" runat="server" visible="true"/></th>
                                                                    <th id="idproforma" style=" width:50px; display:none">Proforma</th>
                                                                        <th id="paseLinea" style=" width:50px; display:none">PaseLine</th>
                                                                    </tr>
                                                                </thead> 
                                                            <tbody>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <td style=" width:100px;"><asp:Label Text='<%#Eval("IDPZAL")%>' ID="lblidppzal" runat="server" /></td>
                                                                <td style=" width:50px" ><%#Eval("Deposito")%></td>
                                                            <td style=" width:80px"><asp:Label Text='<%#Eval("FECHA_SALIDA")%>' Width="80PX" id="lblFechaSalida" runat="server" /></td>
                                                            <td style=" width:70px"><asp:Label Text='<%#Eval("TURNO")%>' Width="70PX" id="lblTurno" runat="server" /></td>
                                                            <td style=" width:150px; display:none"><%#Eval("BOOKING")%></td>
                                                            <td style=" width:150px; display:none"><%#Eval("REFERENCIA")%></td>
                                                            <td style=" width:50px; display:none"><%#Eval("LINEA")%></td>
                                                            <td>
                                                            <asp:TextBox ID="txtchofer" Text='<%#Eval("CHOFER")%>' runat="server" class="form-control" Width="300px"  onkeyup="searchChofer(this)" onblur="fTittleChofer(this)"
                                                                onkeypress="return soloLetras(event,'abcdefghijklmnopqrstuvwxyz1234567890 -',true)" placeholder="DIGITE EL NOMBRE O LA LICENCIA"></asp:TextBox>
                                                            </td>
                                                            <td>
                                                            <asp:TextBox ID="txtplaca" Text='<%#Eval("PLACA")%>' runat="server" Width="90px" class="form-control"
                                                            onkeypress="return soloLetras(event,'abcdefghijklmnopqrstuvwxyz1234567890',true)" placeholder="DIGITE LA PLACA"></asp:TextBox>
                                                            </td>
                                                            <td id="datliquidacion"> <asp:Label Text='<%#Eval("LIQUIDACION")%>' ID="lbl_liquidacion" runat="server" visible="true"/></td>
                                                            <td id="datestado" ><asp:Label Text='<%#Eval("ESTADO_PAGO")%>' ID="lbl_estado" runat="server" visible="true"/></td>
                                                            <%--<td style=" width:250px"><%#Eval("MENSAJE")%></td>--%>
                                                            <td style=" width:50px">
                                                                <asp:ImageButton class="form-control"  ImageUrl="~/shared/imgs/edita.png"  ID="btnActualizar" ToolTip="Actualizar Pase"  OnClick='<%# String.Format("return prepareObjectTableAct(\"{0}\", \"{1}\", \"{2}\", \"{3}\", \"{4}\", \"{5}\", \"{6}\");", Eval("IDPZAL"), Eval("LINEA"), Eval("FECHA_SALIDA"), Eval("CHOFER"), Eval("PLACA"), Eval("BOOKING"), Eval("TURNO")) %>' CommandName='Update' runat="server" />
                                                            </td>
                                                            <td style=" width:50px">
                                                                <asp:ImageButton class="form-control" ImageUrl="~/shared/imgs/action_stop.gif"  ID="btnCancelaPase" ToolTip="Eliminar Pase" OnClick='<%# "return prepareObjectTable(" +Eval("IDPZAL") + " );" %>'  CommandName='Delete;<%#Eval("IDPZAL")%>'  runat="server" />
                                                            </td>
                                                            <td style=" width:50px">
                                                                <asp:ImageButton  class="form-control" ImageUrl="~/shared/imgs/action_print.gif" ID="btnReImprimirPase" ToolTip="Imprimir Pase" CommandName="Imprimir" runat="server" />
                                                            </td>
                                                            <td style=" width:90px">
                                                                <asp:ImageButton class="form-control" ImageUrl="~/shared/imgs/action_print.gif" ID="btnImprimirProforma" ToolTip="Imprimir Liquidación" CommandName="Imprimir_proforma" runat="server" visible="true"/>
                                                            </td>
                                                                <td style=" width:50px; display:none"><%#Eval("IdProforma")%></td>
                                                                <td style=" width:50px; display:none"><%#Eval("ID_PASE_LINEA")%></td>
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
                               
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="btnAsumirBook" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </div>
                    <%--</div>--%>
                </div>
            </div>
        </div>

        <div><br /></div>
         
       <%--  <div class="botonera" runat="server" id="btnera" style=" display:none">
              <img alt="loading.." src="../shared/imgs/loader.gif" id="loader1" class="nover"  />
              <asp:Button ID="btsalvar" runat="server" Text="Generar Proforma"  onclick="btsalvar_Click"
                   OnClientClick="return prepareObjectGenerar('¿Estimado Cliente, está seguro de reservar los Turnos?')"
                   ToolTip="Reservar Turnos."/>
         </div>--%>

        <div class="row">
            

            <div class="col-md-12 d-flex justify-content-center">
                 <img alt="loading.." src="../shared/imgs/loader.gif" id="loader1" class="nover"  />
                <asp:Button class="btn btn-primary" ID="btsalvar" runat="server" Text="Generar Proforma"  onclick="btsalvar_Click"
                   OnClientClick="return prepareObjectGenerar('¿Estimado Cliente, está seguro de reservar los Horarios?')"
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
        };

//        $(function () {
//            $('[id*=lstFruits]').multiselect({
//                includeSelectAllOption: true
//            });
//        });
        function autocompleteClientShown(source, args) {
            source._popupBehavior._element.style.height = "130px";
        }
        function searchChofer(idval) {
            $(idval).autocomplete({
                source: function (request, response) {
                    $.ajax({
                        type: "POST",
                        url: "pases_zal.aspx/GetChoferList",
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
    var contador = 0;
    var pos = 0;

    function checkAll(e,p) {
        document.getElementById("textcantidad").value;
        newState = e.checked;
        tope = document.getElementById("textcantidad").value;
        auxFor = 0;

        if (tope > parseInt(document.getElementById("gv").rows.length)) {
            auxFor = parseInt(document.getElementById("gv").rows.length);
        } else {
            auxFor = tope;
        }
        
        

        if (newState == true) {
            if (p == 2) {
                contador += 1;
                for (var i = 1; i < auxFor; i++) {
                    if (document.getElementById("gv").rows[i].cells[0].firstElementChild.checked == true) {
                        pos = i;
                    }                    
                }
            } else {
                if (contador == 1){
                    for (var i = pos+1; i < pos + auxFor - 1; i++) {
                        document.getElementById("gv").rows[i].cells[0].firstElementChild.checked = newState;
                    }
                }
            }
        }
        else {
            if (p == 1) {
                for (var i = 1; i < document.getElementById("gv").rows.length; i++) {
                    document.getElementById("gv").rows[i].cells[0].firstElementChild.checked = newState;
                }
                contador=0;
            } else {
                contador -= 1;
            }
        }
    }

//        function getGifOcultaBuscar() {
//            //document.getElementById('loader3').className = 'nover';

//            return true;
//        };
//        function getGifOculta() {
//            //document.getElementById('loader2').className = 'nover';

//            
//            document.getElementById('referencia').textContent = "";
//            document.getElementById('xreferencia').value = "";
//            
//            document.getElementById('numbook').textContent = "";
//            document.getElementById('nbrboo').value = "";

//            document.getElementById('linea').textContent = "";
//            document.getElementById('xlinea').value = "";

//            return true;
//        };

//        function getGifOcultaEnviar(mensaje) {
//            document.getElementById('loader').className = 'nover';
//            alert(mensaje);
//            return true;
    //        }
        var programacion = {};
        var lista = [];
        function prepareObjectTable(mensaje) {
            try {
                mensaje = "Esta seguro de Cancelar el Pase: " + mensaje;
                if (confirm(mensaje) == false) {
                    return false;
                }
                return true;
            } catch (e) {
                alert('Lamentamos comunicarle que ha ocurrido el siguiente problema:\n' + e.Message);
            }
        }

        function prepareObjectTableAct(mensaje, line, fechasalida, chofer, placa, bkg, turno) {
            try {
                var idpase = mensaje;
                mensaje = "Esta seguro de Actualizar el Pase: " + mensaje;
                if (confirm(mensaje) == false) {
                    return false;
                }
                //window.open('../zal/actualizar-turno-pase-de-puerta-zal/?pase=' + idpase + '&line=' + line + '&fsal=' + fechasalida + '&chofer=' + chofer + '&placa=' + placa + '&bkg=' + bkg + '&turno=' + turno, '_blank', 'width=850,height=480');
                return true;
            } catch (e) {
                alert('Lamentamos comunicarle que ha ocurrido el siguiente problema:\n' + e.Message);
            }
        }

        function prepareObject() {
            try {
                document.getElementById('loadersearch').className = '';
                return true;
            } catch (e) {
                alert('Lamentamos comunicarle que ha ocurrido el siguiente problema:\n' + e.Message);
            }
        }

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

        function getBookOculta() {
            document.getElementById('loadersearch').className = 'nover';
            return true;
        }

        function add(button) {
            var row = button.parentNode.parentNode;
            var cells = row.querySelectorAll('td:not(:last-of-type)');
            addToCartTable(cells);
        }

        function remove() {
            var row = this.parentNode.parentNode;demofab
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

        function popupCallbackBook(objeto) {
            if (objeto == null || objeto == undefined) {
                alert('¡ Hubo un problema al setear un objeto de catalogo.');
                return;
            }
            document.getElementById('numbook').textContent = objeto.nbr;
            document.getElementById('nbrboo').value = objeto.nbr;
            document.getElementById('referencia').textContent = objeto.id;
            document.getElementById('xreferencia').value = objeto.id;
            document.getElementById('linea').textContent = objeto.id;
            document.getElementById('xlinea').value = objeto.id;
            return;
        }

        function clear() {
            document.getElementById('').textContent = '...';
            document.getElementById('').value = '';
        }

        var programacion = {};
        var lista = [];
        function prepareObjectGenerar(mensaje) {

            try {
                if (confirm(mensaje) == false) {
                    return false;
                }
                document.getElementById("loader").className = '';
                
                return true;
            } catch (e) {
                alert('Lamentamos comunicarle que ha ocurrido el siguiente problema:\n' + e.Message);
            }
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

//        function setObjectBook(row) {
//           var celColect = row.getElementsByTagName('td');
//          var catalogo = {
//              fila: celColect[0].textContent,
//              gkey: celColect[1].textContent,
//              nbr: celColect[2].textContent,
//              linea: celColect[3].textContent,
//              fk: celColect[4].textContent
//              };
//            popupCallbackBooking(catalogo);
//      }

//      function popupCallback(objeto, catalogo) {
//            
//                document.getElementById('numbook').textContent = objeto.nbr;
//                document.getElementById('nbrboo').value = objeto.nbr;
//                document.getElementById('referencia').textContent = objeto.id;
//                document.getElementById('xreferencia').value = objeto.id;
//                document.getElementById('linea').textContent = objeto.linea;
//                document.getElementById('xlinea').value = objeto.linea;
//                return;
        //        };

        function popupCallback(objeto, catalogo) {

            document.getElementById('numbook').textContent = objeto.nbr;
            document.getElementById('nbrboo').value = objeto.nbr;
            var a = objeto.line.split("-");
            document.getElementById('referencia').textContent = a[0].toString();
            document.getElementById('xreferencia').value = a[0].toString(); ;
            document.getElementById('linea').textContent = a[1].toString();
            document.getElementById('xlinea').value = a[1].toString();
            

            return;

        }

        function openPopReporte(opcion) {
            window.open('../zal/wbareportezal.aspx', 'name', 'width=700,height=700');
            //window.location = '../facturacion/emision-pase-de-puerta';
            return true;
        }

        function openPopup() {
            //var ref = document.getElementById('xreferencia').value;
            //var ruc = document.getElementById('xruc').value;
            //window.open('../mantenimientos_proforma_expo/autoriza-booking', 'name', 'width=1000,height=480');
            //window.open('../catalogo/bookaut','name','width=850,height=480')
            //window.open('../catalogo/reserva', 'name', 'width=850,height=480');
            window.open('../catalogo/bookinZAL.aspx', 'name', 'width=850,height=880');
            //window.location = '../facturacion/emision-pase-de-puerta';
            return ;
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
