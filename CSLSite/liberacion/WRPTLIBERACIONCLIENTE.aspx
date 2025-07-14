<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="WRPTLIBERACIONCLIENTE.aspx.cs" Inherits="CSLSite.liberacion.WRPTLIBERACIONCLIENTE" %>
 <%@ MasterType VirtualPath="~/Site.master" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxtoolkit" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<%--<%@ Register src="../../controles/Loading.ascx" tagname="Loading" tagprefix="uc1" %>--%>
<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="placehead" runat="server">

   
    <link href="../img/favicon2.png" rel="icon"/>
    <link href="../img/icono.png" rel="apple-touch-icon"/>
    <link href="../css/bootstrap.min.css" rel="stylesheet"/>
    <link href="../css/dashboard.css" rel="stylesheet"/>
    <link href="../css/icons.css" rel="stylesheet"/>
    <link href="../css/style.css" rel="stylesheet"/>
    <link href="https://fonts.googleapis.com/css2?family=Roboto:wght@300;400;500;700&display=swap" rel="stylesheet"/>
    
    <link href="../shared/estilo/catalogos.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/jquery.datetimepicker.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/atraque.css" rel="stylesheet" />
    <script  type="text/javascript" src="../Scripts/pages.js"></script>
    <script src="../Scripts/jquery.datetimepicker.js" type="text/javascript"></script>
    <link href="../shared/estilo/jquery.datetimepicker.css" rel="stylesheet" type="text/css" />

    <link href="../../styles/estilo.css" rel="stylesheet" type="text/css" />
    <link href="../../styles/aisv.css" rel="stylesheet" type="text/css" />
    <link href="../../Styles/paneles.css" rel="stylesheet" type="text/css" />
    <script src="../../Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
    <script src="../../Scripts/JScriptValidaTexto.js" type="text/javascript"></script>
    <script src="../../Scripts/panels.js" type="text/javascript"></script>
    <script type="text/javascript">
        function acePopulated(sender, e) {
            sender._popupBehavior._x = 30;
            sender._popupBehavior._y = 20;
        }
        function autocompleteClientShown(source, args) {
            source._popupBehavior._element.style.height = "130px";
        }
    </script>
    <style type="text/css">
        .fijo {position:fixed !important; 
               right:200px;
               top:200px;
                }
        
        .modalBackground
        {
            background-color: Gray;
            filter: alpha(opacity=70);
            opacity: 0.7;
        }
        
        .button-link
        {
            display: inline-block;
            height: 20px;
            background-image: url("../../Imagenes/cmdBoton/Imprimir2.png");
            background-position: left center;
            background-repeat: no-repeat;
            padding-left: 20px;
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
              
        div.ajax__calendar_body{padding: 0px!important;}
        .ajax__calendar {
             position: static;
             visibility: visible; display: block;
           }
       
       .ajax__calendar_container
        {
            border: 1px solid #646464;
            background-color:  White!important;
            color: black!important;
        }
        
        .ajax__calendar_active .ajax__calendar_day, .MyCalendar .ajax__calendar_active .ajax__calendar_month, .MyCalendar .ajax__calendar_active .ajax__calendar_year
        {
            color: black!important;
            font-weight: bold!important;
            height:10px!important
        }
        
        .ajax__calendar_other .ajax__calendar_day, .MyCalendar .ajax__calendar_other .ajax__calendar_year .ajax__calendar_month
        {
            color: black!important;
            padding-right:0px!important;
            padding: 0px!important;
            font-size:xx-small!important;
            background-color:White!important;
            border:inherit!important;
        }
        
        .ajax__calendar_hover .ajax__calendar_day, .MyCalendar .ajax__calendar_hover .ajax__calendar_month, .MyCalendar .ajax__calendar_hover .ajax__calendar_year
        {
            color: black!important;
        }
        
        
        .MyCalendar .ajax__calendar_container th
        {
            padding: 0px;
            height:100px!important
        }
        .MyCalendar .ajax__calendar_container td
        {
            background-color: White!important;
            padding: 0px;
            height:10px!important;
             }

   
        .MyCalendar .ajax__calendar_active .ajax__calendar_day, .MyCalendar .ajax__calendar_active .ajax__calendar_month, .MyCalendar .ajax__calendar_active .ajax__calendar_year
        {
            color: black;
            font-weight: bold;
            height:10px!important
        }
        .style1
        {
            height: 74px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="placebody" runat="server">
    <asp:ToolkitScriptManager ID="tkcal" EnableScriptGlobalization="true" runat="server"> </asp:ToolkitScriptManager>
    <div class="mt-4">         
        <nav class="mt-4" aria-label="breadcrumb">
            <ol class="breadcrumb">
                <li class="breadcrumb-item" id="opcion_principal" runat="server"><a href="#">Gestión Financiera</a></li>
                <li class="breadcrumb-item active" aria-current="page" id="sub_opcion" runat="server">REPORTE LIBERACION Cliente</li>
            </ol>
        </nav>
    </div>

    <div class="dashboard-container p-4" id="cuerpo" runat="server">

        <div class="form-title">
            INGRESO DE LA LIBERACIÓN 
        </div>

        <asp:UpdatePanel ID="UPRINCIPAL" runat="server" >
                <ContentTemplate>

                    <div class="form-row">
                        <div class="form-group col-md-12"> 
                            <label for="inputAddress">Cliente:<span style="color: #FF0000; font-weight: bold;"></span></label>
                            <div class="d-flex"> 
                                <asp:TextBox ID="TXTCLIENTE" runat="server" CssClass="form-control" onkeypress="return soloLetras(event,'abcdefghijklmnñopqrstuvwxyz1234567890_-')" ></asp:TextBox>
                                <ajaxToolkit:AutoCompleteExtender ID="autocomCliente" runat="server" 
                                        CompletionInterval="0" CompletionListCssClass="AutoExtender" 
                                        CompletionListElementID="divwidth" 
                                        CompletionListHighlightedItemCssClass="AutoExtenderHighlight" 
                                        CompletionListItemCssClass="AutoExtenderList" CompletionSetCount="13" 
                                        DelimiterCharacters=" " EnableCaching="true" FirstRowSelected="false" 
                                        MinimumPrefixLength="2" OnClientPopulated="acePopulated" 
                                        ServiceMethod="GetClienteList" TargetControlID="TXTCLIENTE" />
                            </div>
                        </div>

                        <div class="form-group col-md-6"> 
                            <label for="inputAddress">Fecha Inicio:<span style="color: #FF0000; font-weight: bold;"></span></label>
                            <div class="d-flex"> 
                                <asp:TextBox ID="TXTFECHAINICIO" runat="server" MaxLength="10"  onkeypress="return soloLetras(event,'1234567890/')"  CssClass="form-control" ></asp:TextBox>
                                <ajaxtoolkit:CalendarExtender ID="CalendarExtender2" runat="server" Enabled="True" Format="MM/dd/yyyy" TargetControlID="TXTFECHAINICIO" TodaysDateFormat="MMMM d,yyyy" ></ajaxtoolkit:CalendarExtender>
                            </div>
                        </div>

                        <div class="form-group col-md-6"> 
                            <label for="inputAddress">Fecha Fin:<span style="color: #FF0000; font-weight: bold;"></span></label>
                            <div class="d-flex"> 
                                <asp:TextBox ID="TXTFECHAFIN" runat="server" MaxLength="10"  onkeypress="return soloLetras(event,'1234567890/')"  CssClass="form-control"></asp:TextBox>
                                <ajaxtoolkit:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="True" Format="MM/dd/yyyy" TargetControlID="TXTFECHAFIN" TodaysDateFormat="MMMM d,yyyy" ></ajaxtoolkit:CalendarExtender>
                            </div>
                        </div>

                    </div>

                    <div class="row">
                        <div class="col-md-12 d-flex justify-content-center">
                            <asp:Button ID="CMDLIMPIAR" runat="server" class="btn btn-primary" OnClick="CMDLIMPIAR_Click" Text="Limpiar" />
                            &nbsp;
                            <asp:Button ID="CMDADD" runat="server" class="btn btn-primary" Text="Generar" onclick="CMDADD_Click" />
                        </div>
                    </div>


                    <div class="form-row">
                        <div class="form-title">
                            RESULTADO DE LA BUSQUEDAD
                        </div>

                        <div class="form-group col-md-12"> 
                            <div class="cataresult" >
                                <div class="bokindetalle" style=" width:100%; overflow:auto">
                                    <table cellpadding="0" cellspacing="0" class="mitabla" >
                                        <tr>
                                            <td><%--height="450" width="1000px" valign="top" --%>
                                                <rsweb:ReportViewer ID="rwReporte" runat="server" Width="1000px" Height="550px"  
                                                    DocumentMapCollapsed="True" ShowDocumentMapButton="False" 
                                                    ShowParameterPrompts="False" ShowPromptAreaButton="False">
                                                </rsweb:ReportViewer>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                        </div>
                    </div>

                </ContentTemplate>
            </asp:UpdatePanel>
    </div>
<%--<uc1:loading ID="Loading1" runat="server" />--%>
</asp:Content>
