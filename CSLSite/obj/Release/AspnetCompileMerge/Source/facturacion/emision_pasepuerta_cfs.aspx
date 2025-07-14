<%@ Page Title="Emisión Pase Puerta CFS" Language="C#" MasterPageFile="~/site.Master" AutoEventWireup="true" CodeBehind="emision_pasepuerta_cfs.aspx.cs" Inherits="CSLSite.facturacion.emision_pasepuerta_cfs" %>
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
    
    <script type="text/javascript">
//        $(document).ready(function () {
//            $('#tablasort').tablesorter({ cancelSelection: true, cssAsc: "ascendente", cssDesc: "descendente", headers: { 5: { sorter: false }, 6: { sorter: false}} }).tablesorterPager({ container: $('#pager'), positionFixed: false, pagesize: 10 });
//        });
  
    </script>

    <style type="text/css">
         
         
        .displaynone
        {
            display:none;
        }
        .panel-reveal-modal-bg { background: #000; background: rgba(0,0,0,.8);cursor:progress;	}
        .estilo
{
     font-size: 11px;
    font-family: Tahoma;
    padding:0px,0px,0px,0px; 
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
        .titTabla
{    
	background:#f0f0f0 url(../Imagenes/layout/bg-tableth.jpg) repeat-x; 
    color:#333333; 
    font: bold 21px Arial, Helvetica, sans-serif; 
    margin:3px 0 10px 0;
	text-align: center;
	height: 32px;
}

.subTitTabla
{       
    margin:3px 0 10px 0;
    padding: 6px 8px;	
    border-color:#E1E1E1;   
    border-width: 1px 1px 0 0;  
    border-style: solid;  
    margin: 0; background:#f0f0f0 url(../data/images/layout/bg-tableth.jpg) repeat-x; 
    font-weight: bolder; color: #000000; text-align:left;
    
}
.subTitTabla2
{   
	font:Arial, Helvetica, sans-serif;  
	FONT-SIZE: 12px;
	font-weight: bolder;
    margin:3px 0 10px 0;
    padding: 6px 8px;	
    border-color:#E1E1E1;   
    border-width: 1px 1px 0 0;  
    border-style: solid;  
    margin: 0; background:#f0f0f0 url(../data/images/layout/bg-tableth.jpg) repeat-x;  
    color: #000000; 
    text-align:left;
}
.reqCampo
{
	FONT-SIZE: 10px;
    padding: 4px; 
    border-color:#E1E1E1;  
    border-width: 1px 0 0 0;  
    border-style: solid;  
    margin: 0; 	
    width: 10px;
}

.lblTabla
{   
    border-left: 0 solid #E1E1E1;
    border-right: 1px solid #E1E1E1;
    border-top: 1px solid #E1E1E1;
    border-bottom: 0 solid #E1E1E1;
    padding: 2px 3px;
    margin: 0;
    background: #f0f0f0 url('../Imagenes/layout/bg-tableth.jpg') repeat-x;
    font-weight: bold; 
    color: #000000; 
    text-align:left;
    font-size: 12px;
     vertical-align:middle;
      text-align:right;
      width:70px!important;
}


.lblTablaToTal
{   
    border-left: 0 solid #E1E1E1;
    border-right: 1px solid #E1E1E1;
    border-top: 1px solid #E1E1E1;
    border-bottom: 0 solid #E1E1E1;
    padding: 2px 3px;
    margin: 0;
    background: #f0f0f0 url('../Imagenes/layout/bg-tableth.jpg') repeat-x;
    font-weight: bold; 
    color: #000000; 
    text-align:left;
    font-size: 12px;
     vertical-align:middle;
      text-align:right;
      width:500px!important;
}
.tablestyle
{
    font-family: arial;
    font-size: small;
    border: solid 1px #7f7f7f;
    background-color: #F7F6F3;
}

.altrowstyle
{
    background-color: #E2DED6;
    color: #333333;
}

.FooterStyle
{
    background-color: #284775;
    color: White;
    font-weight: bold;
    border-color: #284775 #284775 #284775 #284775;
    border-style: solid solid solid none;
    border-width: 1px 1px 1px medium;
}

.headerstyle th 
{
    background:  #284775;
    border-color: #284775 #284775 #284775 #284775;
    border-style: solid solid solid none;
    border-width: 1px 1px 1px medium;
    color: White;
    padding: 4px 5px 4px 10px;
    text-align: center;
    vertical-align: bottom;
}  

.headerstyle th a
{
    font-weight: normal;
    text-decoration: none;
    text-align: center;
    color: White;
    display: block;
    padding-right: 10px;
}    

.rowstyle .sortaltrow, .altrowstyle .sortaltrow 
{
    background-color: #edf5ff;
}

.rowstyle .sortrow, .altrowstyle .sortrow 
{
    background-color: #dbeaff;
}

.rowstyle td, .altrowstyle td 
{
    padding: 4px 10px 4px 10px;
    border-right: solid 1px #cbcbcb;
}



.rowstyle1 .sortaltrow1, .altrowstyle1 .sortaltrow1 
{
    background-color: #edf5ff;
}

.rowstyle1 .sortrow1, .altrowstyle1 .sortrow1 
{
    background-color: #dbeaff;
}

.rowstyle1 td, .altrowstyle1 td 
{
    padding: 4px 4px 4px 4px;
    /*border-right: solid 0px #cbcbcb;*/
    border-bottom: solid 1px #cbcbcb;
    font-size:xx-small;
    vertical-align:middle;
    font-weight: bold;
}

.rowstyle1 td input[type=text], .altrowstyle1 td input[type=text]
{
     font-size:xx-small;
    vertical-align:middle;
}



.headerstyle .sortascheader 
{
    background: url(img/sprite.png) repeat-x 0px -100px;
}

.headerstyle .sortascheader a 
{
    background: url(img/dt-arrow-up.png) no-repeat right 50%;
} 

.headerstyle .sortdescheader 
{
    background: url(img/sprite.png) repeat-x 0px -100px;
}   

.headerstyle .sortdescheader a 
{
    background: url(img/dt-arrow-dn.png) no-repeat right 50%;
} 
td.fondo{ background-color:#CCCCCC; }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="placebody" runat="server">
    <asp:ToolkitScriptManager ID="tkcal" EnableScriptGlobalization="true" runat="server"> </asp:ToolkitScriptManager>
<%--<asp:ScriptManager ID="ScriptManager1" runat="server"
EnablePageMethods = "true">
</asp:ScriptManager>--%>

    <input id="zonaid" type="hidden" value="1203" />
    <div>
    <i class="ico-titulo-2"></i><h1>Emisión e-Pass Carga General CFS</h1>
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
                         <asp:AsyncPostBackTrigger ControlID="btnAddCiatrans" />
                     </Triggers>
                 </asp:UpdatePanel>
             </div>
         <div class="botonera" runat="server" id="divAsumirCliente">
                <img alt="loading.." src="../shared/imgs/loader.gif" id="loader2" class="nover"  />
                <asp:Button ID="btnBuscar" runat="server" Text="Buscar..."
                OnClientClick="return prepareObjectTable();"
                ToolTip="Buscar..." OnClick="btnBuscar_Click" />
                
         </div>
         <div align="center">
                            <asp:Panel ID="pnlFacturasCliente" runat="server" Width="710px" ScrollBars="Auto" Height="500px" Style="display: none;">
                                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                    <ContentTemplate>
                                        <table cellpadding="0" cellspacing="0" class="estilo" style="width: 99%;">
                                             <tr>
                                                <td bgcolor="#ccccff" class="titTabla" colspan="2">
                                                    &nbsp; <asp:Label runat="server" ID="lbl_facturadoa" Text=""></asp:Label> &nbsp;
                                                </td>
                                            </tr>
                                             <tr>
                                                <td class="lblTabla" style="text-align: right" colspan="2">
                                                      <asp:GridView ID="gvFacturas" runat="server" AutoGenerateColumns="False" Font-Size="Smaller" 
                                                        GridLines="None" PageSize="5" ShowFooter="True" ShowHeaderWhenEmpty="True" Style="text-align: left"
                                                        Width="700px">
                                                        <AlternatingRowStyle CssClass="altrowstyle1" />
                                                        <HeaderStyle CssClass="headerstyle" />
                                                        <RowStyle CssClass="rowstyle1" />
                                                        <EmptyDataTemplate>
                                                        </EmptyDataTemplate>
                                                        <FooterStyle CssClass="FooterStyle" />
                                                        <Columns>
                                                            <%--<asp:BoundField DataField="FACTURA" HeaderText="Factura" />--%>
                                                            <asp:TemplateField HeaderText="Factura">
                                                                <ItemTemplate>
                                                                    <asp:Label Text='<%# Bind("FACTURA") %>' ID="lblFacturaPP" runat="server" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <%--<asp:BoundField  DataField="MENSAJE" HeaderText="Estado Factura" />--%>
                                                            <asp:BoundField DataField="FECHA_SALIDA" HeaderText="Fecha Salida PP CFS" />
                                                            <%--<asp:BoundField DataField="ESTADO" HeaderText="Estado PP CFS" />--%>
                                                            <asp:TemplateField HeaderText="Estado PP CFS">
                                                                <ItemTemplate>
                                                                    <asp:Label Text='<%# Bind("ESTADO") %>' ID="lblEstadoPP" runat="server" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                             <asp:BoundField DataField="INFORMACION" HeaderText="Información" />
                                                            <%--<asp:TemplateField HeaderText="">
                                                                <ItemTemplate>
                                                                    
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            --%>
                                                        </Columns>
                                                    </asp:GridView>
                                                    
                                                     <div class="botonera" >
                                                     <table>
                                                     <tr>
                                                     <td>
                                                                                                        <asp:Button ID="btnSalirFac"  runat="server" CausesValidation="False" CssClass="BotonNavegacion"
                                                        OnClick="btnSalirFac_Click" Text="Salir " Width="80px"/>
                                                        </td>
                                                        <td>
                                                        <asp:Button Text="Continuar" runat="server" ID="btnContinuarPP" AutoPostBack="True" Width="100px" OnClick="btnContinuarPP_Click" />
                                                        </td>
                                                        </table>
                                                    </div>
                                                            
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="estilo fondo" colspan="2" align="right">

                                                    <asp:Button ID="btnSeguir" style=" display:none" runat="server" CausesValidation="False" CssClass="BotonNavegacion"
                                                        OnClick="btnSeguir_Click" Text="Continuar" Width="100px"/>

                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </asp:Panel>
                            <asp:ModalPopupExtender ID="modalFacturasCliente" runat="server" BackgroundCssClass="panel-reveal-modal-bg"
                            DropShadow="False" PopupControlID="pnlFacturasCliente" PopupDragHandleControlID="pnlFacturasCliente"
                            TargetControlID="HPFACCLI" />
                            <asp:HyperLink ID="HPFACCLI" runat="server" Style="visibility: hidden"></asp:HyperLink>
                            <asp:Panel ID="PNMODETALLECFS" runat="server"  Style="display: none" Width="100%">
                                <asp:UpdatePanel ID="UPMODETALLECFS" runat="server">
                                    <ContentTemplate>
                                        <table cellpadding="0" cellspacing="0" class="estilo" style="width: 95%;">
                                            <tr>
                                                <td bgcolor="#ccccff" class="titTabla">
                                                    &nbsp; PASE A PUERTA CFS
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: right" class="lblTabla">
                                                    <table style="width: 100%">
                                                    <tr>
                                                                <td class="lblTabla">
                                                                    <asp:Label runat="server" ID="lblNumCarga" Font-Size="X-Small"  Text="Mrn-Msn-Hsn: " Width="250px"></asp:Label>
                                                                </td>
                                                                <td align="left" >
                                                                    <asp:TextBox runat="server" Enabled="false"  Font-Size="X-Small"  id="txtmrnmsnhsnppcfs" Width="180px" Font-Bold="true"></asp:TextBox>
                                                                </td>
                                                    </tr>
                                                    <tr>
                                                                <td class="lblTabla">
                                                                    <asp:Label runat="server" ID="lblfechasalidaPP" Font-Size="X-Small" Text="Fecha de Salida para Emisión de Pase de Puerta: " Width="250px"></asp:Label>
                                                                </td>
                                                               <td align="left">
                                                                    <asp:TextBox runat="server" ID="txtfecsalppcfs" MaxLength="10"  onkeypress="return soloLetras(event,'0123456789/')" ontextchanged="txtfecsalppcfs_TextChanged" AutoPostBack="true" Font-Size="X-Small" Enabled="true" Width="60px" Font-Bold="true"></asp:TextBox>
                                                                    <asp:CalendarExtender ID="CAGTFECHAFASA" runat="server" Enabled="True" 
                                                                        Format="dd/MM/yyyy" TargetControlID="txtfecsalppcfs">
                                                                     </asp:CalendarExtender>
                                                                </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="lblTabla">
                                                                <asp:CheckBox ID="chkTodosSubItems" Font-Size="X-Small"  TextAlign="Left" Width="150px" runat="server" Text="Seleccionar todos: " AutoPostBack="true" OnCheckedChanged="chkTodosSubItems_CheckedChanged" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="lblTabla" colspan="13" width="500px" valign="middle" align="center">
                                                                <asp:GridView ID="GvCfsTemp" runat="server" AutoGenerateColumns="False" 
                                                                    DataKeyNames="CONSECUTIVO" Font-Size="Smaller" GridLines="None" 
                                                                     PageSize="5" ShowFooter="True" OnPageIndexChanging="GvCfsTemp_PageIndexChanging"
                                                                    ShowHeaderWhenEmpty="True" Style="text-align: left" Width="95%" AllowPaging="True">
                                                                    <AlternatingRowStyle CssClass="altrowstyle1" />
                                                                    <HeaderStyle CssClass="headerstyle" />
                                                                    <RowStyle CssClass="rowstyle1" />
                                                                     <PagerStyle HorizontalAlign = "Right" CssClass = "GridPager2" />
                                                                    <EmptyDataTemplate>
                                                                    </EmptyDataTemplate>
                                                                    <FooterStyle CssClass="FooterStyle" />
                                                                    <Columns>
                                                                        <asp:BoundField DataField="CONSECUTIVO" HeaderText="Consecutivo" 
                                                                            Visible="false" />
                                                                        
                                                                        <asp:TemplateField HeaderText="Codigo Sub. Item">
                                                                         <ItemTemplate>
                                                                                   <asp:Label ID="LBLGCONSECUTIVO" runat="server" Font-Size="XX-Small" 
                                                                                    Text='<%# Bind("CONSECUTIVO_VEH") %>' Width="40px"></asp:Label>
                                                                            </ItemTemplate>
                                                                            <ControlStyle Width="20px" />
                                                                        </asp:TemplateField>

                                                                        <asp:TemplateField HeaderText="Cantidad">
                                                                         <ItemTemplate>
                                                                                   <asp:Label ID="LBLGCANTIDAD" runat="server" Font-Size="XX-Small" 
                                                                                    Text='<%# Bind("CANTIDAD") %>' Width="40px"></asp:Label>
                                                                            </ItemTemplate>
                                                                            <ControlStyle Width="20px" />
                                                                        </asp:TemplateField>
                                                                
                                                               
                                                                    
                                                     <asp:TemplateField HeaderText="PIN">
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="CHKPNCFS" runat="server" Checked='<%# Bind("ASIGNADOPN") %>' OnCheckedChanged="CHKPNCFS_CheckedChanged"
                                                                AutoPostBack="True" Width="30px" />
                                                        </ItemTemplate>
                                                     </asp:TemplateField>
                                                                                                                             
                                                                    </Columns>
                                                                </asp:GridView>
                                                            </td>
                                                        </tr>
                                                        </table>
                                                         <table style="width: 100%">
                                                        <tr>
                                                                                                                      
                                                            <td class="lblTabla">
                                                                <asp:Label ID="Label18" runat="server" Font-Size="X-Small"  Text="Cia. Trans:" Width="80px"></asp:Label>
                                                            </td>
                                                            <td width="300px">

                                                                                     <%--<asp:TextBox ID="TxtGEmpresa" ontextchanged="TxtGEmpresa_TextChanged" runat="server" AutoPostBack="true"  Font-Size="X-Small"  Width="250PX" onkeypress="return soloLetras(event,'abcdefghijklmnñopqrstuvwxyz1234567890_-')"></asp:TextBox>
                                                           

                                                                <asp:AutoCompleteExtender runat="server" ID="AutoCompleteExtender3" TargetControlID="TxtGEmpresa"
                                                                    ServiceMethod="GetEmpresaList" MinimumPrefixLength="4" CompletionInterval="0"
                                                                    EnableCaching="true" CompletionSetCount="13" FirstRowSelected="false" CompletionListCssClass="AutoExtender"
                                                                    CompletionListItemCssClass="AutoExtenderList" CompletionListHighlightedItemCssClass="AutoExtenderHighlight"
                                                                    CompletionListElementID="divwidth" OnClientPopulated="acePopulated" DelimiterCharacters=" " />--%>
                                                                    
                                                                    <asp:TextBox ID="TxtGEmpresa" onkeyup="searchCiaTrans(this)" Width="350PX"  OnTextChanged="TxtGEmpresa_TextChanged" AutoPostBack="false" onblur="fConsultaListaChoferes(this)"  Font-Size="X-Small" runat="server"></asp:TextBox>
                                                            </td>
                                                            <td class="lblTabla" width="40px">
                                                                <asp:Label ID="Label20" runat="server" Font-Size="X-Small" Text="Chofer:"></asp:Label>
                                                            </td>
                                                            <td  width="170px">
                                                                <%--<asp:TextBox ID="TxtChoferCFS" runat="server" MaxLength="14" CssClass="inputTextgv" Width="170px" Font-Size="X-Small" onkeypress="return soloLetras(event,'abcdefghijklmnñopqrstuvwxyz1234567890_-')"/>
                                                                <asp:AutoCompleteExtender runat="server" ID="AutoCompleteExtender4" TargetControlID="TxtChoferCFS"
                                                                    ServiceMethod="GetChoferList" MinimumPrefixLength="4" CompletionInterval="0"
                                                                    EnableCaching="true" CompletionSetCount="13" FirstRowSelected="false" CompletionListCssClass="AutoExtender"
                                                                    CompletionListItemCssClass="AutoExtenderList" CompletionListHighlightedItemCssClass="AutoExtenderHighlight"
                                                                    CompletionListElementID="divwidth" OnClientPopulated="acePopulated" DelimiterCharacters=" " />--%>
                                                                    <asp:TextBox ID="TxtChoferCFS" onkeyup="searchChofer(this)" Width="200PX" Font-Size="X-Small" runat="server"></asp:TextBox>
                                                            </td>
                                                            <td class="lblTabla" width="40px">
                                                                <asp:Label ID="Label19" Font-Size="X-Small"  runat="server" Text="Placa:"></asp:Label>
                                                            </td>
                                                            <td  width="60px">
                                                                <asp:TextBox ID="TxtPlacaCFS"   runat="server" MaxLength="10" CssClass="inputTextgv" Width="60px"  Font-Size="X-Small" onkeypress="return soloLetras(event,'abcdefghijklmnñopqrstuvwxyz1234567890_-')"/>

                                                            </td>
                                                            <td width="30px" valign="middle"> 
                                                                <asp:Button ID="CMDADDCFS" runat="server" CssClass="BotonNavegacion2" 
                                                                    Text="Añadir" onclick="CMDADDCFS_Click" ValidationGroup="Breakbulk" Width="100px" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="lblTabla" colspan="13" width="500px" valign="middle" align="center">
                                                                <asp:GridView ID="gvCFS" runat="server" AutoGenerateColumns="False" 
                                                                    DataKeyNames="ID" Font-Size="Smaller" GridLines="None" 
                                                                    onrowcommand="gvCFS_RowCommand"
                                                                    OnPageIndexChanging="gvCFS_PageIndexChanging"
                                                                     PageSize="5" ShowFooter="True" 
                                                                    ShowHeaderWhenEmpty="True" Style="text-align: left" Width="95%" AllowPaging="True">
                                                                    <AlternatingRowStyle CssClass="altrowstyle1" />
                                                                    <HeaderStyle CssClass="headerstyle" />
                                                                    <RowStyle CssClass="rowstyle1" />
                                                                     <PagerStyle HorizontalAlign = "Right" CssClass = "GridPager2" />
                                                                    <EmptyDataTemplate>
                                                                    </EmptyDataTemplate>
                                                                    <FooterStyle CssClass="FooterStyle" />
                                                                    <Columns>
                                                                        <asp:BoundField DataField="CONSECUTIVO" HeaderText="Consecutivo" 
                                                                            Visible="false" />
                                                                        
                                                                        <asp:TemplateField HeaderText="Cantidad" >
                                                                         <ItemTemplate>
                                                                                   <asp:Label ID="LBLGCANTIDAD" runat="server" Font-Size="XX-Small" 
                                                                                    Text='<%# Bind("CANTIDAD") %>' Width="40px" ></asp:Label>
                                                                            </ItemTemplate>
                                                                            <ControlStyle Width="20px" />
                                                                        </asp:TemplateField>
                                                                       <asp:TemplateField HeaderText="Cia. Trans.">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="LBLGEMPRESA" runat="server" Font-Size="XX-Small" 
                                                                                    Text='<%# Bind("EMPRESA") %>' Width="250px"></asp:Label>

                                                                                                  </ItemTemplate>
                                                    </asp:TemplateField>
                                                                       
                                                                       <asp:TemplateField HeaderText="Chofer">
                                                        <ItemTemplate>
                                                          <asp:Label ID="LBLGCHOFER" runat="server" Font-Size="XX-Small" 
                                                                                    Text='<%# Bind("CHOFER") %>' Width="250px"></asp:Label>

                                                        
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Placa">
                                                        <ItemTemplate>
                                                                                                                              <asp:Label ID="LBLGPLACA" runat="server" Font-Size="XX-Small" 
                                                                                    Text='<%# Bind("PLACA") %>' Width="40px"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                     <asp:TemplateField HeaderText="">
                                                        <ItemTemplate>
                                                         <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="false"  Font-Size="XX-Small" 
                                                                                    CommandName="Eliminar" Text="Eliminar" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"></asp:LinkButton>
                                                        </ItemTemplate>
                                                     </asp:TemplateField>
                                                                                                                             
                                                                    </Columns>
                                                                </asp:GridView>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="lblTabla">
                                                <div class="botonera" >
                                                    <asp:Button ID="btnConsultar" runat="server"
                                                        CssClass="BotonNavegacion" OnClick="btnConsultar_Click" Text="Buscar Turno..." 
                                                         Width="120px" />
                                                </div>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: center" class="lblTabla">
                                                    <table style="width: 100%">
                                                        <tr>
                                                            <td class="lblTabla" colspan="13" width="500px" valign="middle" align="center">
                                                            <asp:GridView ID="gvHorarios" runat="server" AutoGenerateColumns="False" 
                                                            DataKeyNames="IDDISPONIBLEDET" Font-Size="Smaller" GridLines="None" 
                                                            Style="text-align: left" Width="95%">
                                                            <AlternatingRowStyle CssClass="altrowstyle1" />
                                                            <HeaderStyle CssClass="headerstyle" />
                                                            <RowStyle CssClass="rowstyle1" />
                                                            <PagerStyle HorizontalAlign = "Right" CssClass = "GridPager2" />
                                                            <FooterStyle CssClass="FooterStyle" />
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="Id" SortExpression="IDDISPONIBLEDET">
                                                                    <EditItemTemplate>
                                                                        <asp:TextBox ID="txtgvId" runat="server" Text='<%# Bind("IDDISPONIBLEDET") %>'></asp:TextBox>
                                                                    </EditItemTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblgvId" runat="server" Text='<%# Bind("IDDISPONIBLEDET") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ControlStyle Width="50px" CssClass="displaynone"/>
                                                                    <HeaderStyle Width="50px"  CssClass="displaynone"/>
                                                                    <ItemStyle Width="50px"  CssClass="displaynone"/>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Total" SortExpression="TOTBULTOS">
                                                                    <EditItemTemplate>
                                                                        <asp:TextBox ID="txtgvTb" runat="server" Text='<%# Bind("TOTBULTOS") %>'></asp:TextBox>
                                                                    </EditItemTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblgvTb" runat="server" Text='<%# Bind("TOTBULTOS") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ControlStyle Width="50px"  CssClass="displaynone"/>
                                                                    <HeaderStyle Width="50px"  CssClass="displaynone"/>
                                                                    <ItemStyle Width="50px"  CssClass="displaynone"/>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Turno" SortExpression="HORADESDE">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblgvDesde" runat="server" Text='<%# Bind("HORADESDE") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ControlStyle Width="100px" />
                                                                    <HeaderStyle Width="100px" />
                                                                    <ItemStyle Width="100px" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Hora Hasta" SortExpression="HORAHASTA">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblgvHasta" runat="server" Text='<%# Bind("HORAHASTA") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ControlStyle Width="100px" CssClass="displaynone"/>
                                                                    <HeaderStyle Width="100px" CssClass="displaynone"/>
                                                                    <ItemStyle Width="100px" CssClass="displaynone"/>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Bultos" SortExpression="BULTOS" Visible="false">
                                                                    <EditItemTemplate>
                                                                        <asp:TextBox ID="txtgvBultos" runat="server" Text='<%# Bind("BULTOS") %>' ></asp:TextBox>
                                                                    </EditItemTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblgvBultos" runat="server" Text='<%# Bind("BULTOS") %>' ></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ControlStyle Width="100px" CssClass=""/>
                                                                    <HeaderStyle Width="100px" CssClass=""/>
                                                                    <ItemStyle Width="100px"  CssClass=""/>
                                                                </asp:TemplateField> 
                                                                <%--<asp:TemplateField HeaderText="IDEMPRESA" SortExpression="IDEMPRESA">
                                                                    <EditItemTemplate>
                                                                        <asp:TextBox ID="txtgvIdEmpresa" runat="server" Text='<%# Bind("IDEMPRESA") %>'></asp:TextBox>
                                                                    </EditItemTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblgvIdEmpresa" runat="server" Text='<%# Bind("IDEMPRESA") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ControlStyle Width="100px" CssClass="displaynone"/>
                                                                    <HeaderStyle Width="100px" CssClass="displaynone"/>
                                                                    <ItemStyle Width="100px"  CssClass="displaynone"/>
                                                                </asp:TemplateField> --%>
                                                                <asp:TemplateField HeaderText="Seleccione Horario">
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="CHKHORARIO" runat="server" Checked='<%# Bind("CHECKED") %>'
                                                                                  OnCheckedChanged="CHKHORARIO_CheckedChanged"
                                                                                  AutoPostBack="True" Width="100px" />
                                                                </ItemTemplate>
                                                                </asp:TemplateField>
                                                            </Columns>
                                                            </asp:GridView>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="lblTabla">
                                                <div class="botonera" >
                                                    <asp:Button ID="CMDCANCELARPASECFS" runat="server" CausesValidation="False"
                                                        CssClass="BotonNavegacion" OnClick="CMDCANCELARPASECFS_Click" OnClientClick="return fValBotonSalir('¿Está seguro de Salir?')" Text="Salir" ValidationGroup="noP" Width="100px" />


                             <img alt="loading.." src="../shared/imgs/loader.gif" id="loader" class="nover"  />
                   <asp:Button ID="btsalvar" runat="server" Text="Generar e-Pass"  onclick="btsalvar_Click"
                   OnClientClick="return prepareObject('! Estimado Cliente, si está seguro de emitir el e-pass con la fecha seleccionada presione aceptar o continuar.')"
                   ToolTip="Generar e-Pass"/>
                                                </div>
                                                </td>
                                            </tr>
                                            <tr>
                                            <td>
                                                <asp:ModalPopupExtender ID="MODALCFS" runat="server" BackgroundCssClass="panel-reveal-modal-bg"
                                                DropShadow="true" PopupControlID="PNMODETALLECFS" PopupDragHandleControlID="PNMODETALLECFS"
                                                TargetControlID="HPHIDCFS" />
                                                <asp:HyperLink ID="HPHIDCFS" runat="server" Style="visibility: hidden"></asp:HyperLink>  
                                            </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </asp:Panel>
                            
         </div>


          <%-- <br />--%>
                           <table style=" display:none" class="xcontroles" cellspacing="0" cellpadding="1">
                 <tr>
                 <td class=" bt-left bt-top bt-right bt-bottom" >Cia. Trans:</td>
                 <td class=" bt-bottom bt-top" >
                     <asp:TextBox ID="TxtGEmpresaAdd"  onblur="fCiTransTT(this)" onkeyup="searchCiaTrans(this)" placeholder="ESCRIBA PARA AÑADIR A SU CARGA" Width="325px"  runat="server"></asp:TextBox>
                 </td>
                 <td class=" bt-top bt-bottom bt-right" >
                     <asp:Button Text="Añadir" ID="btnAddCiatrans" runat="server" onclick="btnAddCiatrans_Click"  OnClientClick="return prepareObjectAdd()"/>
                 </td>
                 </tr>
                 </table>
                 <br />
         <div class="cataresult" >
            <asp:UpdatePanel ID="UpdatePanel0" runat="server" >
            <ContentTemplate>
            <input id="idlin" type="hidden" runat="server" clientidmode="Static" />
            <input id="diponible" type="hidden" runat="server" clientidmode="Static" />

            <div id="xfinder" runat="server" >
            
            <div class="findresult" style=" display:none" >
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
                <td style=" width:50px"><asp:Label Text='<%#Eval("FECHA_AUT_PPWEB")%>' ID="lblFecAutPPWeb" runat="server" /></td>
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
                        
                    <%--</ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="TxtGEmpresa" />
                        </Triggers>
                    </asp:UpdatePanel>--%>
                </td>
                <td>
                    
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
                <td style=" width:50px"></td>
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

            </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btsalvar" />
                </Triggers>
            </asp:UpdatePanel>
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

    <script type="text/javascript">
        var valor_text = "";
        function searchCiaTrans(idval) {
            $(idval).autocomplete({
                source: function (request, response) {
                    $.ajax({
                        type: "POST",
                        url: "emision_pasepuerta_cfs.aspx/GetEmpresaList",
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
                        url: "emision_pasepuerta_cfs.aspx/GetChoferList",
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
                url: "emision_pasepuerta_cfs.aspx/GetFilterChoferList",
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
                
                //document.getElementById("loader2").className = '';

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
                $find('<%= MODALCFS.ClientID%>').hide();
                //document.getElementById("loader").className = 'nover';
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
            window.open('../facturacion/impresion-pase-de-puerta-carga-suelta-cfs?' + opcion, 'name', 'width=700,height=700');
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
        function openPop(carga, pase) {
            window.open('../facturacion/impresion-pase-de-puerta-carga-suelta-cfs?opcion=PasePuerta', 'name', 'width=700,height=700');
            //window.location = '../facturacion/emision-pase-de-puerta';
            return true;
        }
        function fValBotonSalir(mensaje) {
            if (confirm(mensaje) == false) {
                return false;
            }
            return true;
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
