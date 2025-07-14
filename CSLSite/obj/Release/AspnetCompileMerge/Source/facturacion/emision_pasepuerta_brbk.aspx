<%@ Page Title="Emisión Pase Puerta BreakBulk" Language="C#" MasterPageFile="~/site.Master" AutoEventWireup="true"CodeBehind="emision_pasepuerta_brbk.aspx.cs" Inherits="CSLSite.facturacion.emision_pasepuerta_brbk" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

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
    <%--<asp:UpdatePanel ID="UPPrincipal" runat="server"  UpdateMode="Conditional"  ChildrenAsTriggers="true">
    <ContentTemplate>--%>
     <input id="zonaid" type="hidden" value="1203" />
    <div>
    <i class="ico-titulo-2"></i><h1>Emisión e-Pass Carga General BreakBulK</h1>
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
                    <asp:Button ID="btclean" runat="server" CssClass="BotonNavegacion" 
                                  Text="Limpiar" Width="100px" onclick="btclean_Click" OnClientClick="return fReload()"/>    
          </td>
             <td class=" bt-bottom bt-right validacion "><%--<span id="valcntr" class="validacion"> * obligatorio</span>--%></td>
         </tr>
          </table>
        <div class="cataresult" >
               <asp:UpdatePanel ID="UpdatePanel2" runat="server" ChildrenAsTriggers="true">
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
                ToolTip="Buscar..." OnClick="btnBuscar_Click" />
         </div>
    <div align="center">
    <asp:Panel ID="PNMODETALLEBREAKBULK" runat="server" Style="display: none" Width="100%">
                                <asp:UpdatePanel ID="UPMODETALLEBREAK" runat="server">
                                    <ContentTemplate>
                                        <table cellpadding="0" cellspacing="0" class="estilo" style="width: 100%;">
                                            <tr>
                                                <td bgcolor="#ccccff" class="titTabla">
                                                    &nbsp; PASE A PUERTA BREAKBULK
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: right" class="lblTabla">
                                                    <table style="width: 100%">
                                                    <tr>
                                                                <td class="lblTabla">
                                                                    <asp:Label runat="server" Font-Size="X-Small" ID="lblNumCarga" Text="Mrn-Msn-Hsn: " Width="250px"></asp:Label>
                                                                </td>
                                                                <td align="left" >
                                                                    <asp:TextBox runat="server" Enabled="false" Font-Size="X-Small" id="txtmrnmsnhsnppbrbk" Width="180px" Font-Bold="true"></asp:TextBox>
                                                                </td>
                                                    </tr>
                                                    <tr>
                                                                <td class="lblTabla">
                                                                    <asp:Label runat="server" ID="lblfechasalidaPP" Font-Size="X-Small" Text="Fecha de Salida para Emisión de Pase de Puerta: " Width="250px"></asp:Label>
                                                                </td>
                                                                <td align="left" >
                                                                    <asp:TextBox runat="server" ID="txtfecsalppbrbrk" Font-Size="X-Small"  ontextchanged="txtfecsalppbrbrk_TextChanged" AutoPostBack="true" onkeypress="return soloLetras(event,'1234567890/-')" MaxLength="10" Enabled="true" Width="80px" Font-Bold="true"></asp:TextBox>
                                                                     <asp:CalendarExtender ID="CAGTFECHAFASA" runat="server" Enabled="True"
                                                                        Format="dd/MM/yyyy" TargetControlID="txtfecsalppbrbrk">
                                                                     </asp:CalendarExtender>
                                                                </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="lblTabla">
                                                                
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: right" class="lblTabla">
                                                    <table style="width: 100%">
                                                        <tr>
                                                            <td class="lblTabla"  width="80px">
                                                                <asp:Label ID="Label6" Font-Size="X-Small" runat="server" Text="Cant. Pases" Width="80px"></asp:Label>
                                                            </td>
                                                            <td  width="40px">
                                                                <asp:TextBox ID="TXtCANTPASES" runat="server" Font-Size="X-Small" CssClass="inputTextgv" 
                                                                    MaxLength="10" Width="40px" ValidationGroup="Breakbulk" onkeypress="return soloLetras(event,'1234567890')"/>
                                                            </td>
                                                            <td  width="40px">
                                                                <asp:RegularExpressionValidator ID="rqvPases" runat="server" 
                                                                    ControlToValidate="TXtCANTPASES" ErrorMessage="*" SetFocusOnError="True" 
                                                                    ToolTip="Ingrese solo Numeros" ValidationExpression="^[0-9]+$" 
                                                                    ValidationGroup="Breakbulk"></asp:RegularExpressionValidator>
                                                            </td>
                                                            <td class="lblTabla">
                                                                <asp:Label ID="Label7" Font-Size="X-Small" runat="server" Text="Cantidad"></asp:Label>
                                                            </td>
                                                            <td width="40px">
                                                                <asp:TextBox ID="TXTCANTIDAD" runat="server" CssClass="inputTextgv" Font-Size="X-Small"  MaxLength="10" 
                                                                    Width="40px" ValidationGroup="Breakbulk" onkeypress="return soloLetras(event,'1234567890')"/>
                                                            </td>
                                                            <td  width="40px">
                                                                <asp:RegularExpressionValidator ID="rqvCantidad" runat="server" 
                                                                    ControlToValidate="TXTCANTIDAD" ErrorMessage="*" SetFocusOnError="True" 
                                                                    ToolTip="Ingrese solo Numeros" ValidationExpression="^[0-9]+$" 
                                                                    ValidationGroup="Breakbulk"></asp:RegularExpressionValidator>
                                                            </td>
                                                            <td class="lblTabla">
                                                                <asp:Label ID="Label3" Font-Size="X-Small" runat="server" Text="Cia. Trans." Width="80px"></asp:Label>
                                                            </td>
                                                            <td  width="170px">
                                                            <asp:TextBox ID="TXTADDCIABREAKBULK" onkeyup="searchCiaTrans(this)" Width="350PX" AutoPostBack="false" Font-Size="X-Small" runat="server"></asp:TextBox>
                                                                <%--<asp:TextBox ID="TXTADDCIABREAKBULK" runat="server" CssClass="inputTextgv" 
                                                                    Width="170px" Font-Size="X-Small" onkeypress="return soloLetras(event,'abcdefghijklmnñopqrstuvwxyz1234567890_-')"></asp:TextBox>
                                                                <asp:AutoCompleteExtender runat="server" ID="AutoCompleteExtender1" TargetControlID="TXTADDCIABREAKBULK"
                                                                    ServiceMethod="GetEmpresaList" MinimumPrefixLength="4" CompletionInterval="0"
                                                                    EnableCaching="true" CompletionSetCount="13" FirstRowSelected="false" CompletionListCssClass="AutoExtender"
                                                                    CompletionListItemCssClass="AutoExtenderList" CompletionListHighlightedItemCssClass="AutoExtenderHighlight"
                                                                    CompletionListElementID="divwidth" OnClientPopulated="acePopulated" DelimiterCharacters=" " />--%>
                                                            </td>
                                                            <td class="lblTabla" width="40px">
                                                                <asp:Label ID="Label5" runat="server" Font-Size="X-Small" Text="Chofer:"></asp:Label>
                                                            </td>
                                                            <td width="170px">
                                                            <asp:TextBox ID="TxtAddChoferbreakbulk" onkeyup="searchChofer(this)" Width="200PX" Font-Size="X-Small" runat="server"></asp:TextBox>
                                                                <%--<asp:TextBox ID="TxtAddChoferbreakbulk" runat="server" MaxLength="14" CssClass="inputTextgv" Width="170px" Font-Size="X-Small" onkeypress="return soloLetras(event,'abcdefghijklmnñopqrstuvwxyz1234567890_-')"/>
                                                                <asp:AutoCompleteExtender runat="server" ID="AutoCompleteExtender2" TargetControlID="TxtAddChoferbreakbulk"
                                                                    ServiceMethod="GetChoferList" MinimumPrefixLength="4" CompletionInterval="0"
                                                                    EnableCaching="true" CompletionSetCount="13" FirstRowSelected="false" CompletionListCssClass="AutoExtender"
                                                                    CompletionListItemCssClass="AutoExtenderList" CompletionListHighlightedItemCssClass="AutoExtenderHighlight"
                                                                    CompletionListElementID="divwidth" OnClientPopulated="acePopulated" DelimiterCharacters=" " />--%>
                                                            </td>
                                                            <td class="lblTabla" width="40px">
                                                                <asp:Label ID="Label4" runat="server" Font-Size="X-Small" Text="Placa:"></asp:Label>
                                                            </td>
                                                            <td  width="60px">
                                                                <asp:TextBox ID="TXTADDPLACABREAKBULK" runat="server" MaxLength="10" CssClass="inputTextgv" Width="60px"  Font-Size="X-Small" onkeypress="return soloLetras(event,'abcdefghijklmnñopqrstuvwxyz1234567890_-')"/>
                                                            </td>
                                                            <td width="30px" valign="middle"> 
                                                            
                                                                <asp:Button ID="CMDADDBBREAKBULK" runat="server" CssClass="BotonNavegacion2" 
                                                                    Text="Añadir" onclick="CMDADDBBREAKBULK_Click" ValidationGroup="Breakbulk" Width="100px" />
                                                                    
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="lblTabla" colspan="13" width="500px" valign="middle" align="center">
                                                                <asp:GridView ID="GVPASEAPUERTA" runat="server" AutoGenerateColumns="False" 
                                                                    DataKeyNames="ID" Font-Size="Smaller" GridLines="None" 
                                                                    onrowcommand="GVPASEAPUERTA_RowCommand" PageSize="5" ShowFooter="True" 
                                                                    ShowHeaderWhenEmpty="True" Style="text-align: left" Width="95%">
                                                                    <AlternatingRowStyle CssClass="altrowstyle1" />
                                                                    <HeaderStyle CssClass="headerstyle" />
                                                                    <RowStyle CssClass="rowstyle1" />
                                                                    <EmptyDataTemplate>
                                                                    </EmptyDataTemplate>
                                                                    <FooterStyle CssClass="FooterStyle" />
                                                                    <Columns>
                                                                        <asp:BoundField DataField="CONSECUTIVO" HeaderText="Consecutivo" 
                                                                            Visible="false" />
                                                                        <asp:TemplateField HeaderText="Cant. Pases">
                                                                          
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="LBLGCANTPASES" runat="server" Font-Size="XX-Small" 
                                                                                    Text='<%# Bind("CANTPASES") %>' Width="40px"></asp:Label>
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
                                                                        <asp:TemplateField HeaderText="Cia. Trans.">
                                                                            
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="Label1" runat="server" Text='<%# Bind("EMPRESA") %>'  Font-Size="XX-Small" ></asp:Label>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Placa">
                                                                            
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="LBLPLACA" runat="server" Text='<%# Bind("PLACA") %>'  Font-Size="XX-Small" ></asp:Label>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Chofer">
                                                                            
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="Label2"  runat="server" Text='<%# Bind("CHOFER") %>' Font-Size="XX-Small" ></asp:Label>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField ShowHeader="False">
                                                                            <ItemTemplate>
                                                                                <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="false"   Font-Size="XX-Small" 
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
                                                <td style="text-align: center" class="lblTabla">
                                                    <table style="width: 100%">
                                                        <tr>
                                                            <td class="lblTabla" colspan="13" width="500px" valign="middle" align="center">
                                                            <asp:GridView ID="gvHorariosBRBK" runat="server" AutoGenerateColumns="False" 
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
                                                                <%--<asp:TemplateField HeaderText="Total" SortExpression="TOTBULTOS">
                                                                    <EditItemTemplate>
                                                                        <asp:TextBox ID="txtgvTb" runat="server" Text='<%# Bind("TOTBULTOS") %>'></asp:TextBox>
                                                                    </EditItemTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblgvTb" runat="server" Text='<%# Bind("TOTBULTOS") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ControlStyle Width="50px"  CssClass="displaynone"/>
                                                                    <HeaderStyle Width="50px"  CssClass="displaynone"/>
                                                                    <ItemStyle Width="50px"  CssClass="displaynone"/>
                                                                </asp:TemplateField>--%>
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
                                                                <asp:TemplateField HeaderText="Pases Disponibles" SortExpression="BULTOS">
                                                                    <EditItemTemplate>
                                                                        <asp:TextBox ID="txtgvBultos" runat="server" Text='<%# Bind("BULTOS") %>'></asp:TextBox>
                                                                    </EditItemTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblgvBultos" runat="server" Text='<%# Bind("BULTOS") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ControlStyle Width="100px" CssClass=""/>
                                                                    <HeaderStyle Width="100px" CssClass=""/>
                                                                    <ItemStyle Width="100px"  CssClass=""/>
                                                                </asp:TemplateField> 
                                                                
                                                                <asp:TemplateField HeaderText="Seleccione Horario">
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="CHKHORARIOBRBK" runat="server" Checked='<%# Bind("CHECKED") %>'
                                                                                  OnCheckedChanged="CHKHORARIOBRBK_CheckedChanged"
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
                                                <td bgcolor="#CCCCCC" class="titTabla" valign="middle">
                                                <div class="botonera" runat="server" id="div1">
                                                    <asp:Button ID="btConsultarBRBK" runat="server" Font-Size="Small" CausesValidation="False" 
                                                        CssClass="BotonNavegacion" OnClick="btConsultarBRBK_Click" Text="Consulta Turnos" 
                                                        ValidationGroup="noP" Width="150px" />
                                                        </div>
                                                </td>
                                                
                                            </tr>
                                            <tr>
                                            <td bgcolor="#CCCCCC" class="estilo"  valign="middle"><br /></td>
                                            </tr>
                                            <tr>
                                             <td bgcolor="#CCCCCC" class="estilo"  valign="middle" >
                                                <table>
                                                <tr>
                                                <td bgcolor="#CCCCCC" class="estilo"  valign="middle">
                                                    <asp:Button ID="CMDCANCELARPASEBREAKBULK" runat="server" CausesValidation="False"
                                                        CssClass="BotonNavegacion" OnClick="CMDCANCELARPASEBREAKBULK_Click" Text="Salir" ValidationGroup="noP" Width="100px" />
                                                 </td>
                                                    <td>
                                                    <asp:Button ID="btsalvar" runat="server" Text="Generar Pase de Puerta"  onclick="btsalvar_Click" Width="215px"
                                                         OnClientClick="return prepareObject('¿Está seguro de Generar el Pase de Puerta?')"  CssClass="BotonNavegacion"
                                                         ToolTip="Generar Pase de Puerta"/>
                                                </td>
                                                </tr>
                                                </table>
                                                </td>
                                            </tr>
                                            <tr>
                                            <td>
                                                 <asp:ModalPopupExtender ID="MODALBREAKBULK" runat="server" BackgroundCssClass="panel-reveal-modal-bg"
                                                DropShadow="true" PopupControlID="PNMODETALLEBREAKBULK" PopupDragHandleControlID="PNMODETALLEBREAKBULK"
                                                TargetControlID="HPHIDFE" />
                                                <asp:HyperLink ID="HPHIDFE" runat="server" Style="visibility: hidden"></asp:HyperLink>
                                            </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                      </asp:UpdatePanel>
                            </asp:Panel>
    <asp:Panel ID="Panel1" runat="server" style=" display:none" CssClass="esquinas">
                <table cellpadding="0" cellspacing="1" class="mitabla" >
                                <tr>
                                    <td class="lblTabla">
                                        <asp:Label ID="Label2" runat="server" Text="Cia. Trans."></asp:Label>
                                    </td>
                                    <td bgcolor="White" >
         <asp:TextBox ID="TxtCiaTrans"  runat="server" AutoPostBack="true"  Font-Size="X-Small"  Width="700px" onkeypress="return soloLetras(event,'abcdefghijklmnñopqrstuvwxyz1234567890_-')"></asp:TextBox>
                                                            <asp:AutoCompleteExtender runat="server" ID="autocomEmpresa" TargetControlID="TxtCiaTrans"
                                                                ServiceMethod="GetEmpresaList" MinimumPrefixLength="4" CompletionInterval="0"
                                                                EnableCaching="true" CompletionSetCount="13" FirstRowSelected="false" CompletionListCssClass="AutoExtender"
                                                                CompletionListItemCssClass="AutoExtenderList" CompletionListHighlightedItemCssClass="AutoExtenderHighlight"
                                                                CompletionListElementID="divwidth" OnClientPopulated="acePopulated" DelimiterCharacters=" - " /> 
    </td>
    <td class="lblTabla;aline" bgcolor="White" >
         
                                
    </td>
    </tr>
    </table>
    </asp:Panel>
    <div class="border_cuple despliegue-al " style=" width:100%; display:none">    
    
    <asp:Panel ID="Panel2" runat="server" CssClass="esquinas">
            <table cellpadding="0" cellspacing="1" class="mitabla" >
                                <tr style=" display:none">
                                    <td class="lblTabla">
                                    
                <asp:Label Text="[TotCntr]" style=" display:none" id="lblTotCntr" runat="server" />
            </td>
            </tr>
            <tr>    
                <td colspan="2">
                    <asp:HyperLink ID="HPFACCLI" runat="server" Style="visibility: hidden"></asp:HyperLink>
                </td>
                </tr>
                <tr>
                <td class="lblTabla" colspan="2">
                        <div align="center">
                            <asp:Panel ID="pnlFacturasCliente" runat="server" Width="710px" ScrollBars="Auto" Height="500px" Style="display: none">
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
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
                                                            <asp:BoundField DataField="MENSAJE" HeaderText="Estado Factura" />
                                                            <asp:BoundField DataField="FECHA_SALIDA" HeaderText="Fecha Salida PP CFS" />
                                                            <%--<asp:BoundField DataField="ESTADO" HeaderText="Estado PP CFS" />--%>
                                                            <asp:TemplateField HeaderText="Estado PP CFS">
                                                                <ItemTemplate>
                                                                    <asp:Label Text='<%# Bind("ESTADO") %>' ID="lblEstadoPP" runat="server" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <%--<asp:TemplateField HeaderText="">
                                                                <ItemTemplate>
                                                                    
                                                                </ItemTemplate>
                                                            </asp:TemplateField>--%>
                                                            
                                                        </Columns>
                                                    </asp:GridView>
                                                </td>
                                            </tr>
                                            <tr>
                                                
                                                <td bgcolor="#CCCCCC" class="estilo"  valign="middle" >
                                                
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>

                                                                      <table cellpadding="0" cellspacing="0" class="estilo" style="width: 710px">
                                                                        <tr>
                                                <td bgcolor="#CCCCCC" class="estilo"  valign="middle" >
                                                <table>
                                                <tr>
                                                    <td >
                                                   
                                                        
                                                    </td>
                                                    <td>
                                                                                                                
                                                    
                                                        
                                                      <%--  <asp:Button Text="Continuar" runat="server" ID="btnContinuarPP" AutoPostBack="True" Width="100px" OnClick="btnContinuarPP_Click" />--%>
                                                    </td>
                                                </tr>
                                                </table>
                                                </td>
                                                </tr>
                                                </table>
                            </asp:Panel>

                        </div>
                    </td>
                </tr>
                <tr>
                <td>
                    <asp:ModalPopupExtender ID="modalFacturasCliente" runat="server" BackgroundCssClass="modalBackground"
                            DropShadow="False" PopupControlID="pnlFacturasCliente" PopupDragHandleControlID="pnlFacturasCliente"
                            TargetControlID="HPFACCLI" />
                </td>
                </tr>
            </table>
            
    </asp:Panel>
    </div>
    </div>
    </div>
    </div>
    
    <asp:HiddenField ID="hfPanel" runat="server" />   
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
                        url: "emision_pasepuerta_brbk.aspx/GetEmpresaList",
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
                        url: "emision_pasepuerta_brbk.aspx/GetChoferList",
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
                function acePopulated(sender, e) {
                    sender._popupBehavior._x = 30;
                    sender._popupBehavior._y = 20;
                }
                function autocompleteClientShown(source, args) {
                    source._popupBehavior._element.style.height = "130px";
                }
                function fvalclientebill(control, validador) {
                    if (control.value != 0) {
                        control.style.cssText = "background-color:none;color:none;width:500px;"
                        validador.innerHTML = '';
                        return;
                    }
                    control.style.cssText = "background-color:White;color:Red;width:500px;";
                    validador.innerHTML = '<span class="obligado"> * Requerido</span>';
                    return;
                }
                function fValBotonSalir(mensaje) {
                    if (confirm(mensaje) == false) {
                        return false;
                    }
                    return true;
                }
                function prepareObject(mensaje) {
                    try {
                        if (confirm(mensaje) == false) {
                            return false;
                        }
                        $find('<%= MODALBREAKBULK.ClientID%>').hide();
                        return true;
                    } catch (e) {
                        alert('Lamentamos comunicarle que ha ocurrido el siguiente problema:\n' + e.Message);
                    }
                }
                function getGif() {
                    document.getElementById('loading').style.display = 'block';
                    return true;
                }
                function getGifOculta() {
                    document.getElementById('loading').style.display = 'none';
                    return true;
                }
                function prepareObjectTable() {
                    try {

                        var mrn = document.getElementById('<%=txtmrn.ClientID %>').value;
                        var msn = document.getElementById('<%=txtmsn.ClientID %>').value;
                        var hsn = document.getElementById('<%=txthsn.ClientID %>').value;
                        var contenedor = document.getElementById('<%=txtcntr.ClientID %>').value;
                        if (mrn == '' && msn == '' && hsn == '' && contenedor == '') {
                            alert('¡ Escriba al menos un criterio de consulta. ');
                            return false;
                        }
                        if (mrn != '' && msn == '') {
                            alert('¡ Escriba el MSN. ');
                            //msn.focus();
                            return false;
                        }
                        if (mrn != '' && msn != '' && hsn == '') {
                            alert('¡ Escriba el HSN. ');
                            //hsn.focus();
                            return false;
                        }
                        if (mrn == '' && msn != '') {
                            alert('¡ Escriba el MRN. ');
                            // mrn.focus();
                            return false;
                        }
                        if (mrn != '' && msn != '' && hsn == '') {
                            alert('¡ Escriba el HSN. ');
                            //hsn.focus();
                            return false;
                        }
                        if (mrn == '' && hsn != '') {
                            alert('¡ Escriba el MRN. ');
                            // mrn.focus();
                            return false;
                        }
                        if (mrn != '' && msn == '' && hsn != '') {
                            alert('¡ Escriba el MSN. ');
                            // msn.focus();
                            return false;
                        }
                        //getGif();
                        return true;
                    } catch (e) {
                        alert('Lamentamos comunicarle que ha ocurrido el siguiente problema:\n' + e.Message);
                    }
                }
                function openPop(carga, pase) {
                    window.open('../facturacion/impresion-pase-de-puerta-carga-breakbull?opcion=PasePuerta', 'name', 'width=700,height=700');
                    //window.location = '../facturacion/emision-pase-de-puerta';
                    return true;
                }
                function fReload() {
                    location.reload();
                    return true;
                }
    </script>
    <%--</ContentTemplate>
  </asp:UpdatePanel>--%>
  <asp:updateprogress  id="updateProgress" runat="server">
    <progresstemplate>
        <div id="progressBackgroundFilter"></div>
        <div id="processMessage"> Estamos procesando la tarea que solicitó, por favor  espere...<br />
         <img alt="Loading" src="../shared/imgs/loader.gif" style="margin:0 auto;" />
        </div>
    </progresstemplate>
  </asp:updateprogress>
  </asp:Content>
