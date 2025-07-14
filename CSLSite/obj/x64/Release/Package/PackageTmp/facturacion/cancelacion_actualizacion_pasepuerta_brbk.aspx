<%@ Page Title="Cancelación, Actualización o Reimpresion e-Pass" Language="C#" MasterPageFile="~/site.Master" AutoEventWireup="true" CodeBehind="cancelacion_actualizacion_pasepuerta_brbk.aspx.cs" Inherits="CSLSite.facturacion.cancelacion_actualizacion_pasepuerta_brbk" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="placehead" runat="server">
    <link href="../shared/estilo/tables.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/aisv-general.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/work.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/catalogos.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/info.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/w3-progressbar.css" rel="stylesheet" type="text/css" />
    <link href="../shared/avisos/ppt.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/jquery-ui.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/turnos.js" type="text/javascript"></script>
    <script src="../Scripts/credenciales.js" type="text/javascript"></script>
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
    <script src="../Scripts/jquery.searchabledropdown-1.0.8.min.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.min.js" type="text/javascript"></script>
    <script src="../Scripts/jquery-ui.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $("select").searchable({
                maxListSize: 200, // if list size are less than maxListSize, show them all
                maxMultiMatch: 300, // how many matching entries should be displayed
                exactMatch: false, // Exact matching on search
                wildcards: true, // Support for wildcard characters (*, ?)
                ignoreCase: true, // Ignore case sensitivity
                latency: 200, // how many millis to wait until starting search
                warnMultiMatch: 'top {0} matches ...',
                warnNoMatch: 'no matches ...',
                zIndex: 'auto'
            });
        });
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
            .button-link
        {
            display: inline-block;
            height: 20px;
            background-image: url("../shared/imgs/action_print.gif");
            background-position: left center;
            background-repeat: no-repeat;
            padding-left: 20px;
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
    </style>
    <script type="text/javascript">
        function BindFunctions() {
            document.getElementById('imagen').innerHTML = '';
            $(document).ready(function () {
                $('#tablasort').tablesorter({ cancelSelection: true, cssAsc: "ascendente", cssDesc: "descendente", headers: { 5: { sorter: false }, 6: { sorter: false}} }).tablesorterPager({ container: $('#pager'), positionFixed: false, pagesize: 10 });
            });
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="placebody" runat="server">
<%--<div>
    <asp:DropDownList ID="ddlItems" runat="server" >
        <asp:ListItem Text="Mango" Value="1"></asp:ListItem>
        <asp:ListItem Text="Orange" Value="2"></asp:ListItem>
        <asp:ListItem Text="Apple" Value="3"></asp:ListItem>
        <asp:ListItem Text="Banana" Value="4"></asp:ListItem>
        <asp:ListItem Text="Water Melon" Value="5"></asp:ListItem>
        <asp:ListItem Text="Lemon" Value="6"></asp:ListItem>
        <asp:ListItem Text="Pineapple" Value="7"></asp:ListItem>
        <asp:ListItem Text="Papaya" Value="8"></asp:ListItem>
        <asp:ListItem Text="Chickoo" Value="9"></asp:ListItem>
        <asp:ListItem Text="Apricot" Value="10"></asp:ListItem>
        <asp:ListItem Text="Grapes" Value="11"></asp:ListItem>
        <asp:ListItem Text="Olive" Value="12"></asp:ListItem>
        <asp:ListItem Text="Guava" Value="13"></asp:ListItem>
        <asp:ListItem Text="Sweet Lime" Value="14"></asp:ListItem>
    </asp:DropDownList>
</div>--%>
    <input id="zonaid" type="hidden" value="1203" />
    <asp:ToolkitScriptManager ID="tkcal" EnableScriptGlobalization="true" runat="server"> </asp:ToolkitScriptManager>
    <div>
    <i class="ico-titulo-2"></i><h1>Actualización, Reimpresion e-Pass BreakBulk</h1>
        <br />
    </div>

    <div class="seccion">
       <div class="accion">
         <input id="agencia" type="hidden" runat="server"  clientidmode="Static"/>
         <table class="xcontroles" cellspacing="0" cellpadding="1">
         <tr>
            <th class="bt-right  bt-left bt-top" colspan="3">DATOS DE CONSULTA PARA Actualización, Reimpresion e-Pass BreakBull:</th>
         </tr>
         </table>
         <table class="xcontroles" cellspacing="0" cellpadding="1">
         <tr>
         <td class="bt-bottom  bt-top bt-left bt-right" >Mrn - Msn - Hsn:</td>
         <td class="bt-bottom bt-top  " >
                   <asp:TextBox 
                    style="text-align: center"  
                    ID="txtmrn" runat="server"  width="150px" MaxLength="16" placeholder="MRN"
                    onkeypress="return soloLetras(event,'abcdefghijklmnopqrstuvwxyz0123456789-_')" />
                   <asp:TextBox 
                    style="text-align: center" 
                    ID="txtmsn" runat="server"  width="50px" MaxLength="4" placeholder="MSN"
                    onkeypress="return soloLetras(event,'abcdefghijklmnopqrstuvwxyz0123456789-_')" />
                   <asp:TextBox 
                    style="text-align: center" 
                    ID="txthsn" runat="server"  width="50px" MaxLength="4" placeholder="HSN"
                    onkeypress="return soloLetras(event,'abcdefghijklmnopqrstuvwxyz0123456789-_')" />
          </td>
             <td class="bt-top bt-bottom bt-right validacion "><%--<span id="valcarga" class="validacion"> * obligatorio</span>--%></td>
         </tr>
         <tr style=" display:none">
         <td class="bt-bottom   bt-left bt-right" >Contenedor:</td>
         <td class="bt-bottom   " >
                   <asp:TextBox 
                    style="text-align: center"  
                    ID="txtcntr" runat="server"  width="150px" MaxLength="50" placeholder="CONTENEDOR"
                    onkeypress="return soloLetras(event,'abcdefghijklmnopqrstuvwxyz0123456789-_/')" 
                    ></asp:TextBox>
          </td>
             <td class=" bt-bottom bt-right validacion "><%--<span id="valcntr" class="validacion"> * obligatorio</span>--%></td>
         </tr>
         <tr style=" display:none">
         <td class="bt-bottom  bt-left bt-right" >Fecha de Salida:</td>
         <td class="bt-bottom   " >
             <asp:TextBox 
             style="text-align: center" 
             ID="txtfecsal" runat="server"  width="150px" MaxLength="15" CssClass="datetimepicker" placeholder="FECHA DE SALIDA"
             onkeypress="return soloLetras(event,'0123456789/')" 
             ></asp:TextBox>
          </td>
             <td class=" bt-bottom bt-right validacion "><%--<span id="valfecsal" class="validacion"> * obligatorio</span>--%></td>
         </tr>
         </table>
         <div class="cataresult" >
               <asp:UpdatePanel ID="UpdatePanel1" runat="server" ChildrenAsTriggers="true">
                     <ContentTemplate>
                       <input id="Hidden3" type="hidden" runat="server" clientidmode="Static" />
                       <input id="Hidden4" type="hidden" runat="server" clientidmode="Static" />
                  </ContentTemplate>
                     <Triggers>
                      <asp:AsyncPostBackTrigger ControlID="btnBuscar" />
                      <asp:AsyncPostBackTrigger ControlID="btsalvar" />
                     </Triggers>
                 </asp:UpdatePanel>
             </div>
         <div class="botonera" runat="server" id="divAsumirCliente">
                <img alt="loading.." src="../shared/imgs/loader.gif" id="loader2" class="nover"  />
                <asp:Button ID="btnBuscar" runat="server" Text="Buscar..."
                OnClientClick="return prepareObjectTable();"
                ToolTip="Buscar..." OnClick="btnBuscar_Click"/>
         </div>
         <br />
         <div class="cataresult" >
            <asp:UpdatePanel ID="UpdatePanel0" runat="server">
            <ContentTemplate>
             <%--<input id="idlin" type="hidden" runat="server" clientidmode="Static" />
             <input id="diponible" type="hidden" runat="server" clientidmode="Static" />--%>
             <%--<script type="text/javascript">                           Sys.Application.add_load(BindFunctions); </script>--%>
             <div id="xfinder" runat="server" >
             <div class="findresult" >
             <div class="booking" >
                 <div class="bokindetalle" style=" overflow:scroll; height:250px;">
                 <asp:Label Text="" ID="lblTotCntr" Font-Bold="true" runat="server" />
                 <asp:Repeater ID="tablePaginationPPWeb" runat="server" >
                 <HeaderTemplate>
                 <table id="tablasort" cellspacing="1" cellpadding="1" class="tabRepeat">
                 <thead>
                  <div class=" bt-top">
                     <th style=" width:50px;display:none">Cancelar ePass</th>
                     <th style=" width:50px;">Actualizar ePass</th>
                     <th style=" width:60px;">Reimprimir ePass</th>
                     <th style=" width:80px">Pase</th>
                     <th style=" width:150px">Mrn-Msn-Hsn</th>
                     <th style=" width:50px">Facturado hasta</th>
                     <th style=" width:100px">Turno</th>
                     <th style=" width:200px;">Cia. Trans</th>
                     <th style=" width:200px;">Conductor</th>
                     <th >Placa</th>
                     <th style=" width:80px">Actualizar Turno</th>
                     <th style=" width:200px;">Actualizar Cia. Trans</th>
                     <th style=" width:200px;">Actualizar Conductor</th>
                     <th style=" width:50px;">Actualizar Placa</th>
                     </div>
                 </thead> 
                 <tbody>
                 </HeaderTemplate>
                 <ItemTemplate>
                 <tr class="point">
                  <td style=" width:50px;display:none">
                    <asp:UpdatePanel ID="UpdatePanel5" runat="server" ChildrenAsTriggers="true">
                    <ContentTemplate>
                        <asp:CheckBox id="chkCanPase" runat="server" AutoPostBack="True" oncheckedchanged="chkCanPase_CheckedChanged" />
                    </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="chkCanPase" />
                        </Triggers>
                    </asp:UpdatePanel>
                 </td>
                 <td style=" width:50px">
                    <asp:UpdatePanel ID="UpdatePanel4" runat="server" ChildrenAsTriggers="true">
                    <ContentTemplate>
                        <asp:CheckBox id="chkPase" runat="server" AutoPostBack="True" oncheckedchanged="chkPase_CheckedChanged" />
                    </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="chkPase" />
                        </Triggers>
                    </asp:UpdatePanel>
                 </td>
                 <td style=" width:60px"><asp:LinkButton ID="lnkImprimir" runat="server" Text = "" CssClass="button-link"></asp:LinkButton></td>
                 <td style=" width:80px"><asp:Label Text='<%#Eval("NUMERO_PASE_N4")%>' ID="lblPase" Width="80px" runat="server" /></td>
                  <td style=" width:150px"><asp:Label Text='<%#Eval("CARGA")%>' ID="lblCarga" Width="150px" runat="server" /></td>
                  <td style=" width:50px">
                  <%--<asp:Label Text='<%#Eval("FECHA_AUT_PPWEB")%>' Width="50px" ID="lblFecAutPPWeb" runat="server" />--%>
                  <asp:UpdatePanel ID="UpdatePanel2" runat="server" ChildrenAsTriggers="true">
                    <ContentTemplate>
                    <asp:TextBox runat="server" ID="lblFecAutPPWeb" Text='<%#Eval("FECHA_AUT_PPWEB")%>' Font-Size="X-Small" AutoPostBack="true" MaxLength="10" 
                     onkeypress="return soloLetras(event,'0123456789/')" Width="60px" ontextchanged="lblFecAutPPWeb_TextChanged"></asp:TextBox>

                    <asp:CalendarExtender ID="CAGTFECHAFASA"  runat="server"
                        CssClass="cal_Theme1" Format="dd/MM/yyyy" TargetControlID="lblFecAutPPWeb">
                    </asp:CalendarExtender>

                     </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="lblFecAutPPWeb" />
                        </Triggers>
                    </asp:UpdatePanel>

                  </td>
                  <td style=" width:100px"><asp:Label Text='<%#Eval("D_TURNO")%>' Width="100px" ID="lbldturno" runat="server" /></td>
                  <td style=" width:200px"><asp:Label Text='<%#Eval("CIATRANSDES")%>'  Width="200px" ID="lblciatrans" runat="server" /></td>
                  <td style=" width:200px;"><asp:Label Text='<%#Eval("CHOFERDES")%>'   Width="200px" ID="lblchofer" runat="server" /></td>
                  <td ><%#Eval("PLACADES")%></td>
                  <td>
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" ChildrenAsTriggers="TRUE">
                    <ContentTemplate>
                        <asp:DropDownList runat="server" ID="ddlTurno" Font-Size="X-Small" Width="80px" AutoPostBack="false" onselectedindexchanged="ddlTurno_SelectedIndexChanged">
                        </asp:DropDownList>
                    </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="ddlTurno" />
                        </Triggers>
                    </asp:UpdatePanel>
                  </td>
                  <td>
                  <asp:TextBox ID="TxtGEmpresa" onkeyup="searchCiaTrans(this)"  OnTextChanged="TxtGEmpresa_TextChanged" AutoPostBack="false" onblur="searchCiaTrans(this)"  Font-Size="X-Small" runat="server"></asp:TextBox>
                   <%-- <asp:UpdatePanel ID="UpdatePanel2" runat="server" ChildrenAsTriggers="true">
                    <ContentTemplate>
                        <asp:DropDownList ID="ddlEmpresa" runat="server" Font-Size="X-Small"  Width="200px" AutoPostBack="true" onselectedindexchanged="ddlEmpresa_SelectedIndexChanged">
                        </asp:DropDownList>
                    </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="ddlEmpresa" />
                        </Triggers>
                    </asp:UpdatePanel>--%>
                  </td>
                  <td >
                   <asp:TextBox ID="TxtGChofer" onkeyup="searchChofer(this)" onblur="fTittleChofer(this)" Font-Size="X-Small" runat="server"></asp:TextBox>
                   <%-- <asp:UpdatePanel ID="UpdatePanel3" runat="server" ChildrenAsTriggers="true">
                    <ContentTemplate>
                        <asp:DropDownList runat="server" ID="ddlChofer" AutoPostBack="True" Font-Size="X-Small" Width="200px" onselectedindexchanged="ddlChofer_SelectedIndexChanged">
                        </asp:DropDownList>
                    </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="ddlChofer" />
                        </Triggers>
                    </asp:UpdatePanel>--%>
                  </td>
                  <td style=" width:50px"><asp:TextBox runat="server" Font-Size="X-Small" ID="txtPlaca" Width="50px" /></td>
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
             <%--<div id="pager">
                 Registros por página
                 <select class="pagesize">
                  <option selected="selected" value="10">10</option>
                  <option value="20">20</option>
                  <option value="30">30</option>
                  <option value="40">40</option>
                  <option value="50">50</option>
                  </select>
                 <img alt="" src="../shared/imgs/first.gif" class="first"/>
                 <img alt="" src="../shared/imgs/prev.gif" class="prev"/>
                 <input  type="text" class="pagedisplay" size="5px"/>
                 <img alt="" src="../shared/imgs/next.gif" class="next"/>
                 <img alt="" src="../shared/imgs/last.gif" class="last"/>
            </div>--%>
             </div>
                  </ContentTemplate>
                     <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btsalvar" />
                     </Triggers>
                 </asp:UpdatePanel>
             </div>
         <div class="botonera" runat="server" id="btnera">
                   <img alt="loading.." src="../shared/imgs/loader.gif" id="loader" class="nover"  />
                   <asp:Button ID="btsalvar" runat="server" Text="Procesar..."  onclick="btsalvar_Click"
                   OnClientClick="return prepareObject('¿Estimado Cliente está seguro de continuar con el Proceso?')"
                   ToolTip="Procesar..."/>
         </div>
      </div>
  </div>
    <asp:HiddenField runat="server" id="hfRucUser" />
    <script src="../Scripts/pages.js" type="text/javascript"></script>
    <script src="../Scripts/credenciales.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.tablesorter.min.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.tablesorter.pager.js" type="text/javascript"></script>

    <script type="text/javascript">

        function searchCiaTrans(idval) {
            $(idval).autocomplete({
                source: function (request, response) {
                    $.ajax({
                        type: "POST",
                        url: "cancelacion_actualizacion_pasepuerta_brbk.aspx/GetEmpresaList",
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
                    $("[id$=hfCustomerId]").val(i.item.label);
                },
                minLength: 1
            });
        }

        function searchChofer(idval) {
            $(idval).autocomplete({
                source: function (request, response) {
                    $.ajax({
                        type: "POST",
                        url: "cancelacion_actualizacion_pasepuerta_brbk.aspx/GetChoferList",
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
                    //document.getElementById('<%=txtmrn.ClientID %>').value = i.item.label;
                },
                minLength: 1
            });
        }

        function fConsultaListaChoferes(idval) {
            idval.title = idval.value;
            var validametodo = false;
            $.ajax({
                type: "POST",
                url: "cancelacion_actualizacion_pasepuerta_brbk.aspx/GetFilterChoferList",
                data: "{ 'prefix': '" + idval.value + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: function OnSuccess(response) {
                    document.getElementById("loader2").className = 'nover';
                    if (response.d != "1") {
                        if (!confirm("Mensaje Informativo:\n¿El Booking no le pertenece esta seguro de continuar?")) {
                            validametodo = false;
                        }
                        else {
                            validametodo = true;
                        }
                    }
                    else {
                        validametodo = true;
                    }
                },
                failure: function (response) {
                    alert(response.d);
                    validametodo = false;
                }
            });
            return validametodo;
        }

        function fTittleChofer(idval) {
            idval.title = idval.value;
        }

        var ced_count = 0;
        var jAisv = {};
        $(document).ready(function () {
            $('.datetimepicker').datetimepicker().datetimepicker({ lang: 'es', timepicker: false, closeOnDateSelect: true, format: 'd/m/Y' });
        });

        function checkcaja50(control, validador, opcional) {
            try {
                control.style.cssText = "background-color:White;color:Red;width:50px;";
                var codigo;
                codigo = control.value.trim().toUpperCase();
                //Opcional no vino, opñcional es nulo opcional es falso
                if (opcional == undefined || opcional == null || opcional == false) {
                    if (codigo.length <= 0) {
                        document.getElementById(validador).innerHTML = '<span >* opcional</span>';
                        return true;
                    }
                }
                if (codigo.length <= 0) {
                    document.getElementById(validador).innerHTML = '<span class="obligado">OBLIGATORIO!</span>';
                    return false;
                }
                control.style.cssText = "background-color:none;color:none;width:50px;"
                document.getElementById(validador).innerHTML = '';
                control.value = control.value.trim().toUpperCase();
                return true;
            } catch (e) {
                alert(e.Message);
                return false;
            }
        }

        document.querySelector("#buscarref").onkeyup = function () {
            $TableFilter("#tablasort", this.value);
        }

        //document.querySelector("#buscarcli").onkeyup = function () {
        //    $TableFilter("#tablasortcli", this.value);
        //}

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

        function datosReplace() {
            var tableReg = document.getElementById('tablasort');
            var cellsOfRow = "";
            // Recorremos todas las filas con contenido de la tabla
            for (var i = 0; i < tableReg.rows.length; i++) {
                cellsOfRow = tableReg.rows[i].getElementsByTagName('td');
                var x = cellsOfRow[4].getElementsByTagName('label')[0];
                x.checked = false;
            }
        }

        function datosReplaceBkg() {
            var tableReg = document.getElementById('tablasort');
            var cellsOfRow = "";
            // Recorremos todas las filas con contenido de la tabla
            for (var i = 0; i < tableReg.rows.length; i++) {
                cellsOfRow = tableReg.rows[i].getElementsByTagName('td');
                var x = cellsOfRow[1];
                
            }
        }

        function getGifOculta() {
            document.getElementById("loader2").className = 'nover';
  
            return true;
        }

        var programacion = {};
        var lista = [];
        function prepareObjectTable() {
            try {
                
                document.getElementById("loader2").className = '';

                var mrn = document.getElementById('<%=txtmrn.ClientID %>').value;
                var msn = document.getElementById('<%=txtmsn.ClientID %>').value;
                var hsn = document.getElementById('<%=txthsn.ClientID %>').value;
                var contenedor = document.getElementById('<%=txtcntr.ClientID %>').value;
                var fechasalida = document.getElementById('<%=txtfecsal.ClientID %>').value;
                if (mrn == '' && msn == '' && hsn == '' && contenedor == '' && fechasalida == '') {
                    alert('¡ Escriba al menos un criterio de consulta. ');
                    document.getElementById("loader2").className = 'nover';
                    return false;
                }
                if (mrn != '' && msn == '') {
                    alert('¡ Escriba el MSN. ');
                    document.getElementById("loader2").className = 'nover';
                    //msn.focus();
                    return false;
                }
                if (mrn != '' && msn != '' && hsn == '') {
                    alert('¡ Escriba el HSN. ');
                    document.getElementById("loader2").className = 'nover';
                    //hsn.focus();
                    return false;
                }
                if (mrn == '' && msn != '') {
                    alert('¡ Escriba el MRN. ');
                    document.getElementById("loader2").className = 'nover';
                   // mrn.focus();
                    return false;
                }
                if (mrn != '' && msn != '' && hsn == '') {
                    alert('¡ Escriba el HSN. ');
                    document.getElementById("loader2").className = 'nover';
                    //hsn.focus();
                    return false;
                }
                if (mrn == '' && hsn != '') {
                    alert('¡ Escriba el MRN. ');
                    document.getElementById("loader2").className = 'nover';
                   // mrn.focus();
                    return false;
                }
                if (mrn != '' && msn == '' && hsn != '') {
                    alert('¡ Escriba el MSN. ');
                    document.getElementById("loader2").className = 'nover';
                   // msn.focus();
                    return false;
                }
                return true;
            } catch (e) {
                alert('Lamentamos comunicarle que ha ocurrido el siguiente problema:\n' + e.Message);
            }
        }

        var ced_count = 0;
        var jAisv = {};
        $(document).ready(function () {
            $('.datetimepicker').datetimepicker().datetimepicker({ lang: 'es', timepicker: false, closeOnDateSelect: true, format: 'd/m/Y' });
            $('.datepicker').datetimepicker().datetimepicker({ lang: 'es', timepicker: false, format: 'd/m/Y', closeOnDateSelect: true, minDate: '0' });
        });

        var programacion = {};
        var lista = [];
        function prepareObject(mensaje) {
            try {
                if (confirm(mensaje) == false) {
                    return false;
                }
                document.getElementById("loader").className = 'nover';
                //lista = [];
                //validaciones básicas

                var tableReg = document.getElementById('tablasort');
                if (tableReg.rows.length == 1) {
                    alert('¡ No tiene datos para Procesar.');
                    return false;
                }
                /*
                var valida = "0";
                //var tableReg = document.getElementById('tablasort');
                var cellsOfRow = "";
                // Recorremos todas las filas con contenido de la tabla
                for (var i = 1; i < tableReg.rows.length; i++) {
                    cellsOfRow = tableReg.rows[i].getElementsByTagName('td');
                    var x = cellsOfRow[7].getElementsByTagName('input')[0];
                    if (x.checked) {
                        valida = "1";
                    }
                }
                if (valida == "0") {
                    alert('¡ Marque con un check la casilla Gen. Pase, para Generar e-Pass.');
                    return false;
                }
                */

                return true;
            } catch (e) {
                alert('Lamentamos comunicarle que ha ocurrido el siguiente problema:\n' + e.Message);
            }
        }

        function myLoad(DAE) {
            window.location = '../cuenta/menu.aspx';
            alert('DAE: ' + DAE + ', Asignada exitosamente.');
            return;
        }

        function openPop() {
            window.open('../facturacion/impresion-pase-de-puerta', 'name', 'width=700,height=700');
            return true;
        }

        function getGifOcultaBuscar() {
            document.getElementById('loader2').className = 'nover';
            document.getElementById("loader").className = 'nover';
        }
    </script>
<asp:updateprogress  id="updateProgress" runat="server">
    <progresstemplate>
        <div id="progressBackgroundFilter"></div>
        <div id="processMessage"> Estamos procesando la tarea que solicitó, por favor  espere...<br />
         <img alt="Loading" src="../shared/imgs/loader.gif" style="margin:0 auto;" />
        </div>
    </progresstemplate>
</asp:updateprogress>
  </asp:Content>
