<%@ Page Title="Emisión Pase Puerta" Language="C#" MasterPageFile="~/site.Master" AutoEventWireup="true" CodeBehind="emision_pasepuerta_autocomplete.aspx.cs" Inherits="CSLSite.facturacion.emision_pasepuerta_autocomplete" %>
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

    
    <script type="text/javascript">
        //        $(document).ready(function () {
        //            $('#tablasort').tablesorter({ cancelSelection: true, cssAsc: "ascendente", cssDesc: "descendente", headers: { 5: { sorter: false }, 6: { sorter: false}} }).tablesorterPager({ container: $('#pager'), positionFixed: false, pagesize: 10 });
        //        });
  
    </script>
    <style type="text/css">


.completionListElement 
{  
    visibility : hidden; 
    margin : 0px! important; 
    background-color : inherit; 
    color : black; 
    border : solid 1px gray; 
    cursor : pointer; 
    text-align : left; 
    list-style-type : none; 
    font-family : Verdana; 
    font-size: 11px; 
    padding : 0; 
} 
.listItem 
{ 
    background-color: white; 
    padding : 1px; 
}      
.highlightedListItem 
{ 
    background-color: #c3ebf9; 
    padding : 1px; 
}

.completionList {

        border:solid 1px Gray;

        margin:0px;

        padding:3px;

        height: 120px;

        overflow:auto;

        background-color: #FFFFFF;     

        } 

        .listItem {

        color: #191919;

        } 

        .itemHighlighted {

        background-color: #ADD6FF;       

        }
        
        
        .CompletionListCssClass
        {
            font-size: xx-small;
            color: #000;
            padding: 3px 5px;
            border: 1px solid #999;
            background: #fff;
            width: 300px;
            float: left;
            z-index: 1;
            position: absolute;
            margin-left: 0px;
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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="placebody" runat="server">
 <%--   <asp:ToolkitScriptManager ID="tkcal" EnableScriptGlobalization="true" runat="server"> </asp:ToolkitScriptManager>--%>
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"/>
 
  
<%--<asp:TextBox ID="txtAutoComplete" runat="server"/>
                               
<asp:AutoCompleteExtender ID="AutoCompleteExtender1" 
                          runat="server" 
                          DelimiterCharacters="" 
                          Enabled="True" 
                          ServicePath="~/facturacion/AutoComplete.asmx" 
                          ServiceMethod="GetEmpresaList"
                          TargetControlID="txtAutoComplete"
                          MinimumPrefixLength="1" 
                          CompletionInterval="10" 
                          EnableCaching="true"
                          CompletionSetCount="12">
</asp:AutoCompleteExtender>--%>

<%--<asp:ScriptManager ID="ScriptManager1" runat="server"
EnablePageMethods = "true">
</asp:ScriptManager>--%>

    <input id="zonaid" type="hidden" value="1203" />
    <div>
    <i class="ico-titulo-2"></i><h1>Emisión e-Pass</h1>
        <br />
    </div>
    <div class="seccion">
       <div class="accion">
         <input id="agencia" type="hidden" runat="server"  clientidmode="Static"/>
           <table class="xcontroles" cellspacing="0" cellpadding="1">
         <tr>
            <th class="bt-right  bt-left bt-top" colspan="3">DATOS DE CONSULTA PARA EMISIÓN DE e-Pass:</th>

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
         <tr>
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
                        <%-- <asp:AsyncPostBackTrigger ControlID="btnBuscar" />--%>
                         <asp:AsyncPostBackTrigger ControlID="btsalvar" />
                        <%-- <asp:AsyncPostBackTrigger ControlID="btnAddCiatrans" />--%>
                     </Triggers>
                 </asp:UpdatePanel>
             </div>
           <div class="botonera" runat="server" id="divAsumirCliente">
                <img alt="loading.." src="../shared/imgs/loader.gif" id="loader2" class="nover"  />
                <asp:Button ID="btnBuscar" runat="server" Text="Buscar..."
                OnClientClick="return prepareObjectTable();"
                ToolTip="Buscar..." OnClick="btnBuscar_Click" />
                
         </div>
           <br />
                           <table class="xcontroles" cellspacing="0" cellpadding="1">
                 <tr>
                 <td class=" bt-left bt-top bt-right bt-bottom" >Cia. Trans:</td>
                 <td class=" bt-bottom bt-top" >
                     <%--<asp:TextBox ID="TxtGEmpresaAdd"  onblur="fCiTransTT(this)" onkeyup="searchCiaTrans(this)" placeholder="ESCRIBA PARA AÑADIR A SU CARGA" Width="325px"  runat="server"></asp:TextBox>--%>
                     <asp:TextBox ID="TxtGEmpresaAdd"  onblur="fCiTransTT(this)"  placeholder="ESCRIBA PARA AÑADIR A SU CARGA" Width="325px"  runat="server"></asp:TextBox>
                     <asp:AutoCompleteExtender ID="AutoCompleteExtender2" 
                          runat="server" 
                          CompletionListCssClass="AutoExtender"
                          CompletionListItemCssClass="AutoExtenderList"
                          CompletionListHighlightedItemCssClass="AutoExtenderHighlight"
                          CompletionListElementID="divwidth"
                          DelimiterCharacters=" - " 
                          Enabled="True" 
                          ServicePath="~/facturacion/AutoComplete.asmx" 
                          ServiceMethod="GetEmpresaList"
                          TargetControlID="TxtGEmpresaAdd"
                          MinimumPrefixLength="4" 
                          CompletionInterval="0" 
                          EnableCaching="true"
                          CompletionSetCount="13">
                     </asp:AutoCompleteExtender>
                 </td>
                 <td class=" bt-top bt-bottom bt-right" >
                     <asp:Button Text="Añadir" ID="btnAddCiatrans" runat="server" onclick="btnAddCiatrans_Click"  OnClientClick="return prepareObjectAdd()"/>
                 </td>
                 </tr>
                 </table>
           <br />
         <div class="cataresult" >
           <%-- <asp:UpdatePanel ID="UpdatePanel0" runat="server" >
            <ContentTemplate>--%>
            <input id="idlin" type="hidden" runat="server" clientidmode="Static" />
            <input id="diponible" type="hidden" runat="server" clientidmode="Static" />

            <div id="xfinder" runat="server" >
            
            <div class="findresult" >
            <div class="booking" >
                <div class="bokindetalle">
                <asp:Label Text="" ID="lblTotCntr" Font-Bold="true" runat="server" />
                <asp:Repeater ID="tablePaginationPPWeb" runat="server" >
                <HeaderTemplate>
                <table id="tablasort" cellspacing="1" cellpadding="1" class="tabRepeat">
                <thead>
                <div class=" bt-top">
                    <th style=" width:70px">Mrn-Msn-Hsn</th>
                    <th style=" width:50px">Contenedor</th>
                    <th style=" width:50px">Facturado hasta</th>
                    <th style=" width:70px">Turno</th>
                    <th style=" width:125px;">Cia. Trans</th>
                    <th style=" width:125px;">Conductor</th>
                    <th style=" width:50px;">Placa</th>
                    <th style=" width:50px;">Gen.<br />e-Pass</th>
                </div>
                </thead> 
                <tbody>
                </HeaderTemplate>
                <ItemTemplate>
                <tr class="point">
                <td style=" width:50px"><asp:Label Text='<%#Eval("CARGA")%>' ID="lblCarga" runat="server" /></td>
                <td style=" width:50px"><asp:Label Text='<%#Eval("CONTENEDOR")%>' ID="lblCntr" runat="server" /></td>
                <td style=" width:50px">
                <%--<asp:Label Text='<%#Eval("FECHA_AUT_PPWEB")%>' ID="lblFecAutPPWeb" runat="server" />--%>
               <%-- <asp:UpdatePanel ID="UpdatePanel2" runat="server" ChildrenAsTriggers="true">
                    <ContentTemplate>--%>
                    <asp:TextBox runat="server" ID="lblFecAutPPWeb" Text='<%#Eval("FECHA_AUT_PPWEB_")%>' Font-Size="X-Small" AutoPostBack="true" MaxLength="10" 
                     CssClass="datetimepicker"
                     onkeypress="return soloLetras(event,'0123456789/')" Width="60px" ontextchanged="lblFecAutPPWeb_TextChanged"></asp:TextBox>

                    <%--<asp:CalendarExtender ID="CAGTFECHAFASA"  runat="server"
                        CssClass="cal_Theme1" Format="dd/MM/yyyy" TargetControlID="lblFecAutPPWeb">
                    </asp:CalendarExtender>

                     </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="lblFecAutPPWeb" />
                        </Triggers>
                    </asp:UpdatePanel>--%>
                </td>
                <td>
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" ChildrenAsTriggers="true">
                    <ContentTemplate>
                        <asp:DropDownList runat="server" ID="ddlTurno" Font-Size="X-Small" Width="70px" AutoPostBack="true" onselectedindexchanged="ddlTurno_SelectedIndexChanged">
                        </asp:DropDownList>
                    </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="ddlTurno" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
                <td>
                    <%--<asp:UpdatePanel ID="UpdatePanel2" runat="server" ChildrenAsTriggers="true">
                    <ContentTemplate>--%>
                       <%-- <asp:DropDownList ID="ddlEmpresa" runat="server" Font-Size="X-Small"  Width="125px" AutoPostBack="true" onselectedindexchanged="ddlEmpresa_SelectedIndexChanged">
                        </asp:DropDownList>--%>
                        <%--<asp:TextBox ID="TxtGEmpresa" onkeyup="searchCiaTrans(this)"  OnTextChanged="TxtGEmpresa_TextChanged" AutoPostBack="false" onblur="fConsultaListaChoferes(this)"  Font-Size="X-Small" runat="server"></asp:TextBox>--%>
                        <asp:TextBox ID="TxtGEmpresa" OnTextChanged="TxtGEmpresa_TextChanged" AutoPostBack="false"  Font-Size="X-Small" runat="server"></asp:TextBox>
                        <asp:AutoCompleteExtender ID="AutoCompleteExtender3" 
                          runat="server"
                          CompletionListCssClass="AutoExtender"
                          CompletionListItemCssClass="AutoExtenderList"
                          CompletionListHighlightedItemCssClass="AutoExtenderHighlight"
                          CompletionListElementID="divwidth"
                          DelimiterCharacters=" - " 
                          Enabled="True" 
                          ServicePath="~/facturacion/AutoComplete.asmx" 
                          ServiceMethod="GetEmpresaList"
                          TargetControlID="TxtGEmpresa"
                          MinimumPrefixLength="4" 
                          CompletionInterval="0" 
                          EnableCaching="true"
                          CompletionSetCount="13">
                     </asp:AutoCompleteExtender>
                    <%--</ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="TxtGEmpresa" />
                        </Triggers>
                    </asp:UpdatePanel>--%>
                </td>
                <td>
                    <asp:TextBox ID="TxtGChofer" onkeyup="searchChofer(this)" Font-Size="X-Small" runat="server"></asp:TextBox>
                    <%--<asp:UpdatePanel ID="UpdatePanel3" runat="server" ChildrenAsTriggers="true">
                    <ContentTemplate>
                        <asp:DropDownList runat="server" ID="ddlChofer" AutoPostBack="True" Font-Size="X-Small" Width="125px" onselectedindexchanged="ddlChofer_SelectedIndexChanged">
                        </asp:DropDownList>
                    </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="ddlChofer" />
                        </Triggers>
                    </asp:UpdatePanel>--%>
                </td>
                <td style=" width:50px"><asp:TextBox runat="server" Font-Size="X-Small" ID="txtPlaca" Width="50px" /></td>
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
                </tr>
                </ItemTemplate>
                <FooterTemplate>
                </tbody>
                </table>
                </FooterTemplate>
        </asp:Repeater>
                <%--
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
            --%>
                    
            </div>
            <div runat="server" id="divnotificacion" style=" font-weight:bold"></div>
            </div>
            </div>
            </div>

            <%--</ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btsalvar" />
                </Triggers>
            </asp:UpdatePanel>--%>
         </div>
         <div class="botonera" runat="server" id="btnera">
                   <img alt="loading.." src="../shared/imgs/loader.gif" id="loader" class="nover"  />
                   <asp:Button ID="btsalvar" runat="server" Text="Generar e-Pass"  onclick="btsalvar_Click"
                   OnClientClick="return prepareObject('! Estimado Cliente, si está seguro de emitir el e-pass con la fecha seleccionada presione aceptar o continuar.')"
                   ToolTip="Generar e-Pass"/>
             
         </div>
      </div>
  </div>
  <asp:HiddenField ID="hfCustomerId" runat="server" />
  <asp:HiddenField ID="hfChoferId" runat="server" />
    <asp:HiddenField runat="server" id="hfRucUser" />
    <script src="../Scripts/pages.js" type="text/javascript"></script>
    <script src="../Scripts/credenciales.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.tablesorter.min.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.tablesorter.pager.js" type="text/javascript"></script>

<%--    <script src="http://ajax.googleapis.com/ajax/libs/jquery/1.4/jquery.min.js" type="text/javascript"></script>
    <script src="http://ajax.googleapis.com/ajax/libs/jqueryui/1.8/jquery-ui.min.js" type="text/javascript"></script>
    <link href="http://ajax.googleapis.com/ajax/libs/jqueryui/1.8/themes/base/jquery-ui.css" rel="Stylesheet" type="text/css" />--%>

    <script type="text/javascript">
        $(document).ready(function () {
            $('.datetimepicker').datetimepicker().datetimepicker({ lang: 'es', timepicker: false, closeOnDateSelect: true, format: 'd/m/Y' });
        });
        var valor_text = "";
        function searchCiaTrans(idval) {
            $(idval).autocomplete({
                source: function (request, response) {
                    $.ajax({
                        type: "POST",
                        url: "emision_pasepuerta.aspx/GetEmpresaListPase",
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
                        url: "emision_pasepuerta.aspx/GetChoferList",
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

        function fCiTransTT(idval) {
            idval.title = idval.value;
        }

        function fConsultaListaChoferes(idval) {
            idval.title = idval.value;
            var validametodo = false;
            $.ajax({
                type: "POST",
                url: "emision_pasepuerta.aspx/GetFilterChoferList",
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
        function acePopulated(sender, e) {
            sender._popupBehavior._x = 30;
            sender._popupBehavior._y = 20;
        }
        function autocompleteClientShown(source, args) {
            source._popupBehavior._element.style.height = "130px";
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
                    alert('¡ No tiene datos para Generar e-Pass.');
                    return false;
                }

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
                    alert('¡ Marque con un check la casilla Gen. e-Pass para continuar...');
                    return false;
                }


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

        function openPop(opcion) {
            window.open('../facturacion/impresion-pase-de-puerta?' + opcion, 'name', 'width=700,height=700');
            //window.location = '../facturacion/emision-pase-de-puerta';
            return true;
        }

        function getGifOcultaBuscar() {
            document.getElementById('loader2').className = 'nover';
            document.getElementById("loader").className = 'nover';
        }

        function prepareObjectAdd() {
            try {

                document.getElementById("loader").className = 'nover';
                //lista = [];
                //validaciones básicas

                var tableReg = document.getElementById('tablasort');
                if (tableReg.rows.length == 1) {
                    alert('¡ Primero busque la información del ePass.');
                    return false;
                }

                var ciatrans = document.getElementById('<%=TxtGEmpresaAdd.ClientID %>').value;
                if (ciatrans == '' || ciatrans == undefined || ciatrans == null) {
                    alert('¡ Escriba la Cia. Trans');
                    //document.getElementById('<%=TxtGEmpresaAdd.ClientID %>').focus();
                    return false;
                }
                var mensaje = '¿Estimado Cliente está seguro de añadir la Cia. Trans,\n' + ciatrans.trim() + '\na la carga o contenedor?';
                if (confirm(mensaje) == false) {
                    return false;
                }
                return true;
            } catch (e) {
                alert('Lamentamos comunicarle que ha ocurrido el siguiente problema:\n' + e.Message);
            }
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
