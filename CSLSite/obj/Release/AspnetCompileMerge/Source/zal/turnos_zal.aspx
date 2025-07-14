<%@ Page Language="C#" Title="Actualizar Turno e-Pass ZAL" AutoEventWireup="true" CodeBehind="turnos_zal.aspx.cs" Inherits="CSLSite.turnos_zal" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Actualizar Turno e-Pass ZAL</title>
        <link href="../shared/estilo/tables.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/aisv-general.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/work.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/catalogos.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/info.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/w3-progressbar.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/jquery-ui.css" rel="stylesheet" type="text/css" />
    <link href="../shared/avisos/ppt.css" rel="stylesheet" type="text/css" />

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

    <script type="text/javascript" src="../lib/jquery/jquery-3.5.1.slim.min.js" integrity="sha384-DfXdz2htPH0lsSSs5nCTpuj/zy4C+OGpamoFVy38MVBnE+IbbVYUew+OrCXaRkfj" crossorigin="anonymous"></script>
    <script src="../Scripts/pages.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.tablesorter.min.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.tablesorter.pager.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.datetimepicker.js" type="text/javascript"></script>

    <script type="text/javascript">
//        function BindFunctions() {
//            document.getElementById('imagen').innerHTML = '';
//            $(document).ready(function () {
//                $('#tablasort').tablesorter({ cancelSelection: true, cssAsc: "ascendente", cssDesc: "descendente", headers: { 5: { sorter: false }, 6: { sorter: false}} }).tablesorterPager({ container: $('#pager'), positionFixed: false, pagesize: 10 });
//            });
        //        }
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
</head>
<body>
     <noscript><meta http-equiv="refresh" content="0; url=sinsoporte.htm" /> </noscript>
    <form id="bookingfrm" runat="server">
         <asp:HiddenField ID="hfChoferId" runat="server" />
    <%--<asp:ToolkitScriptManager ID="tkscata" runat="server"></asp:ToolkitScriptManager>--%>
        <asp:ToolkitScriptManager ID="tkscata" EnableScriptGlobalization="true" runat="server"> </asp:ToolkitScriptManager>

        <div><br /></div>

        <div class="form-group col-md-12">  
            <asp:TextBox class="form-control" Font-Bold="false"  id="txtChofer" onkeyup="searchChofer(this)" onkeypress="return soloLetras(event,'abcdefghijklmnopqrstuvwxyz1234567890',true)" placeholder="DIGITE EL NOMBRE O LA LICENCIA" runat="server" />
        </div>

        <div class="dashboard-container p-4" id="cuerpo" runat="server">
            <div class="catabody">
                <div  >
                    <div  >
                        <div >

                            <div class="form-row">
                                <%--<div class="form-group col-md-12"> --%>
                                    <div class="form-group col-md-6"> 
                                        <label id="Label1" for="inputAddress"># Pase:<span style="color: #FF0000; font-weight: bold;"></span></label>
                                        <asp:Label class="form-control" Font-Bold="false" id="lblPase" Text="[]" runat="server" />
                                    </div>
                                
                                    <div class="form-group col-md-6"> 
                                        <label id="Label2" for="inputAddress">Fecha de Salida:<span style="color: #FF0000; font-weight: bold;"></span></label>
                                        <%--<asp:Label class="catalabel" Width="125px" id="Label2" Text="Fecha de Salida:" runat="server" />--%>
                                        <asp:Label class="form-control" Font-Bold="false" id="lblFechaSalida" Text="[]" runat="server" />
                                    </div>

                                    <div class="form-group col-md-6"> 
                                        <label id="Label3" for="inputAddress">Turno:<span style="color: #FF0000; font-weight: bold;"></span></label>
                                        <%--<asp:Label class="catalabel" Width="125px" id="Label3" Text="Turno:" runat="server" />--%>
                                        <asp:Label  class="form-control" Font-Bold="false" id="lblTurnoPase" Text="[]" runat="server" />
                                    </div>
                                
                                    <div class="form-group col-md-6"> 
                                        <label id="Label6" for="inputAddress">Booking:<span style="color: #FF0000; font-weight: bold;"></span></label>
                                        <%--<asp:Label class="catalabel" Width="125px" id="Label6" Text="Booking:" runat="server" />--%>
                                        <asp:Label  class="form-control" Font-Bold="false" id="lblBooking"  runat="server" />
                                    </div>

                                    <div class="form-group col-md-6"> 
                                        <label id="Label4" for="inputAddress">Chofer:<span style="color: #FF0000; font-weight: bold;"></span></label>
                                        <%--<asp:Label class="catalabel" Width="125px" id="Label4" Text="Chofer:" runat="server" />--%>
                                        <asp:Label  class="form-control" Font-Bold="false" id="Label7"  runat="server" />
                                    </div>

                                    <div class="form-group col-md-6"> 
                                        <label id="Label5" for="inputAddress">Placa:<span style="color: #FF0000; font-weight: bold;"></span></label>
                                        <%--<asp:Label class="catalabel" Width="125px" id="Label5" Text="Placa:" runat="server" />--%>
                                        <asp:TextBox class="form-control" Font-Bold="false" id="txtPlaca" onkeypress="return soloLetras(event,'abcdefghijklmnopqrstuvwxyz1234567890',true)" placeholder="DIGITE LA PLACA" runat="server" />
                                    </div>
                                <%--</div>--%>

                            </div>
                     

                        </div>
                        <p class="catavalida"></p>
                    </div>
                    
                    <div><br /></div>

                    <div class="row">
                        <div class="col-md-12 d-flex justify-content-center">
                            <span id="imagen"></span>
                            <asp:Button ID="find" class="btn btn-primary"  runat="server" Text="Actualizar" OnClientClick="return initFinder();"/>
                        </div>
                    </div>    
                    
                    <div class="form-row">
                        <div class="form-group col-md-12"> 
                            <div class="cataresult" >
                                <asp:UpdatePanel ID="upresult" runat="server"  >
                                    <ContentTemplate>
                                        <script type="text/javascript">Sys.Application.add_load(BindFunctions); </script>

                                        <div><br /></div>

                                        <div id="xfinder" runat="server" visible="false" >
                                            <div class="alert alert-warning" runat="server" id="alerta" >
                                                    Confirme que los datos sean correctos. En caso de error, favor comuníquese con 
                                                    el Departamento de Planificación a los teléfonos: +593 (04) 6006300, 3901700 
                                                    ext. 4039, 4040, 4060. 
                                            </div>
                                            <%-- catalogo de bookings--%>
                                            <div class="findresult" >
                                                <div class="booking" >
                                                    
                                                    <div class="form-group col-md-12"> 
                                                        <div class="form-title">Agentes / Personas naturales</div>
                                                    </div>

                                                    <div class="bokindetalle" style=" width:100%; overflow:auto">
                                                        <asp:Repeater ID="tablePagination" runat="server" >
                                                            <HeaderTemplate>
                                                                <table id="tablasort" cellspacing="1" cellpadding="1" class="table table-bordered invoice">
                                                                <thead>
                                                                    <tr>
                                                                        <th>No.</th>
                                                                        <th>Booking</th>
                                                                        <th class="nover">Ruc</th>
                                                                        <th>Referencia</th>
                                                                        <th>Cantidad</th>
                                                                        <th>Reservado</th>
                                                                        <th>Despachado</th>
                                                                        <th>Nave</th>
                                                                        <th>Acciones</th>
                                                                    </tr>
                                                                </thead> 
                                                                <tbody>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <tr class="point" onclick="setObject(this);">
                                                                    <td><%#Eval("row")%></td>
                                                                    <td><%#Eval("nbr")%></td>
                                                                    <td class="nover"><%#Eval("ruc")%></td>
                                                                    <td><%#Eval("line")%></td>
                                                                    <td><%#Eval("cant_bkg")%></td>
                                                                    <td><%#Eval("reservado")%></td>
                                                                    <td><%#Eval("despachado")%></td>
                                                                    <td><%#Eval("name")%></td>
                                                                    <td>
                                                                        <a href="#" >Elegir</a>
                                                                    </td>
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
                                            </div>
                                        </div>
                                        <div id="sinresultado" runat="server" class="alert alert-info">
                                            No se encontraron resultados, 
                                            asegurese que ha escrito correctamente el número/nombre
                                            buscado  
                                        </div>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="find" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </form>
    <script src="../Scripts/pages.js" type="text/javascript"></script>
    <script src="../Scripts/credenciales.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.tablesorter.min.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.tablesorter.pager.js" type="text/javascript"></script>

    <script type="text/javascript" >


        function autocompleteClientShown(source, args) {
            source._popupBehavior._element.style.height = "130px";
        }

        function searchChofer(idval) {
            var pageUrl = '<%=ResolveUrl("turnos_zal.aspx")%>'
            var validametodo = false;
            $.ajax({
                type: "POST",
                url: pageUrl + "/GetChoferList",
                data: '{prefix: "' + idval.value + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: function OnSuccess(response) {
                    if (response.d == "1") {
                        validametodo = true;
                    }
                    else {
                        if (response.d != "0") {
                            alert(response.d);
                        }
                        validametodo = false;
                    }
                },
                failure: function (response) {
                    alert(response.d);
                    validametodo = false;
                }
            });
//            $(idval).autocomplete({
//                source: function (request, response) {
//                    $.ajax({
//                        type: "POST",
//                        url: pageUrl + "/GetChoferList",
//                        data: "{ 'prefix': '" + idval.value + "'}",
//                        contentType: "application/json; charset=utf-8",
//                        dataType: "json",
//                        async: false,
//                        success: function (data) {
//                            response($.map(data.d, function (item) {
//                                return {
//                                    label: item.split('-')[0].trim() + ' - ' + item.split('-')[1].trim(),
//                                    val: item.split('-')[0].trim()
//                                }
//                            }))
//                        },
//                        error: function (response) {
//                            alert(response.responseText);
//                        },
//                        failure: function (response) {
//                            alert(response.responseText);
//                        }
//                    });
//                },
//                select: function (e, i) {
//                    $("[id$=hfChoferId]").val(i.item.val);
//                },
//                minLength: 1
//            });
        }

       function setObject(row) {
           var celColect = row.getElementsByTagName('td');
           var bookin = {
               row: celColect[0].textContent,
               nbr: celColect[1].textContent,
               ruc: celColect[2].textContent,
               line: celColect[3].textContent,
               cant_bkg: celColect[4].textContent,
               reservado: celColect[5].textContent,
               despachado: celColect[6].textContent,
               nave: celColect[7].textContent
           };
           if (window.opener != null) {

               window.opener.popupCallback(bookin, 'bk');
           }
           self.close();
       }
       function initFinder() {
           if (document.getElementById('txtname').value.trim().length <= 0) {
               alert('Por favor escriba una o varias \nletras del número');
               return false;
           }
           document.getElementById('imagen').innerHTML = '<img alt="" src="../shared/imgs/loader.gif">';
       }
       
   </script>
</body>
</html>
