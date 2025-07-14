<%@ Page Title="Carga de Contenedor" Language="C#" MasterPageFile="~/site.Master" AutoEventWireup="true"
    CodeBehind="FullCntLoad.aspx.cs" Inherits="CSLSite.fcont_load" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="placehead" runat="server">
    <link href="../shared/estilo/tables.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/aisv-general.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/work.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/catalogos.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/info.css" rel="stylesheet" type="text/css" />

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
    <style type="text/css">
        .warning
        {
            background-color: Yellow;
            color: Red;
        }
        
        #progressBackgroundFilter
        {
            position: fixed;
            bottom: 0px;
            right: 0px;
            overflow: hidden;
            z-index: 1000;
            top: 0;
            left: 0;
            background-color: #CCC;
            opacity: 0.8;
            filter: alpha(opacity=80);
            text-align: center;
        }
        #processMessage
        {
            text-align: center;
            position: fixed;
            top: 30%;
            left: 43%;
            z-index: 1001;
            border: 5px solid #67CFF5;
            width: 200px;
            height: 100px;
            background-color: White;
            padding: 0;
        }
        #aprint
        {
            color: #666;
            border: 1px solid #ccc;
            -moz-border-radius: 3px;
            -webkit-border-radius: 3px;
            background-color: #f6f6f6;
            padding: 0.3125em 1em;
            cursor: pointer;
            white-space: nowrap;
            overflow: visible;
            font-size: 1em;
            outline: 0 none /* removes focus outline in IE */;
            margin: 0px;
            line-height: 1.6em;
            background-image: url(../shared/imgs/action_print.gif);
            background-repeat: no-repeat;
            background-position: left center;
            text-decoration: none;
            padding: 5px 2px 5px 30px;
        }
        .auto-style1 {
            left: 0px;
            top: 0px;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="placebody" runat="server">
    <input id="zonaid" type="hidden" value="607" />
    <asp:ToolkitScriptManager ID="tkcal" EnableScriptGlobalization="true" runat="server">
    </asp:ToolkitScriptManager>

    <div class="mt-4">         
        <nav class="mt-4" aria-label="breadcrumb">
            <ol class="breadcrumb">
            <li class="breadcrumb-item" id="opcion_principal" runat="server"><a href="#">Mis Naves</a></li>
                <li class="breadcrumb-item active" aria-current="page" id="sub_opcion" runat="server">Full Containers Load Report</li>
            </ol>
        </nav>
    </div>
    <div class="dashboard-container p-4" id="cuerpo" runat="server">
        
        <input id="agencia" type="hidden" runat="server" clientidmode="Static" />
                    
        <div class="form-title">
            Filtros para el reporte
        </div>
            
        <div class="form-row">
            <div class="form-group col-md-12"> 
                <label for="inputAddress">Referencia de Nave:<span style="color: #FF0000; font-weight: bold;"></span></label>
                <div class="d-flex"> 
                    <asp:TextBox  class="form-control"  ID="tbooking" runat="server" MaxLength="15" onkeypress="return soloLetras(event,'abcdefghijklmnopqrstuvwxyz01234567890/')"></asp:TextBox>
                      
                    <a class="btn btn-outline-primary mr-4" style='font-size:24px' target="popup" onclick="window.open('../catalogo/naves.aspx','name','width=900,height=880')">
                            <span class='fa fa-search' style='font-size:24px'></span> </a>
                </div>
            </div>
        </div>

        <asp:UpdatePanel ID="UpdatePanel1" runat="server" >
            <ContentTemplate>
                    <div class="row">
                        <div class="col-md-12 d-flex justify-content-center">
                            <asp:Button ID="btbuscar" runat="server" Text="Descargar" 
                                class="btn btn-primary" 
                                onclick="btbuscar_Click" />
                            <span id="imagen"></span>
                        </div>
                    
                    </div>
                <br />
                 <div class="form-row">
                     <div class="form-group col-md-12"> 
                         <div id="sinresultado" visible="false" runat="server" class=" alert  alert-warning" ></div>
                     </div>
                 </div>
                </ContentTemplate>
             <Triggers>
              <asp:AsyncPostBackTrigger ControlID="btbuscar" />
        </Triggers>
        </asp:UpdatePanel>
           
        <div class="form-row">
            <div class="auto-style1"> 
                <div class="cataresult">
                    <asp:UpdatePanel ID="upresult" runat="server" ChildrenAsTriggers="true">
                        <ContentTemplate>
                            <input id="idlin" type="hidden" runat="server" clientidmode="Static" />
                            <input id="diponible" type="hidden" runat="server" clientidmode="Static" />

                            <div><br /></div>

                            <div id="xfinder" runat="server" visible="false" title="PRUEBA">
                                <div class="alert alert-warning" id="alerta" runat="server">
                                </div>
                                <div class="findresult">
                                    <div class="booking">
                                        <table>
                                             <tr>
                                                <td>
                                                    <a href="#" class="btn btn-primary" target="_blank" runat="server" id="aprint" clientidmode="Static" >Vista Preliminar / Imprimir</a>
                                                </td>
                                                <td>
                                                    <input class="btn btn-primary" clientidmode="Static" id="dataexport" onclick="getTable('reportCargFul');" type="button" value="Exportar" runat="server" />
                                                </td>
                                            </tr>
                                         </table>
                                        <br />

                                        <div class="form-group col-md-12"> 
                                            <div class="form-title">
                                                Full Container Load
                                            </div>
                                        </div>
                                        
                                        <div class="bokindetalle" style=" width:100%; overflow:auto">
                                            <div style=" width : 2000px; overflow : auto; height : 300px ">
                                                <asp:Repeater ID="tablePagination" runat="server">
                                                    <HeaderTemplate>
                                                        <table id="tabla" cellspacing="1" cellpadding="1" class="table table-bordered invoice">
                                                            <thead>
                                                                <tr>
                                                                    <%--<th class="nover">#</th>--%>
                                                                    <th style="width:250px;">Vessel</th>
                                                                     <th style="width:60px">Voyage</th>
                                                                     <th style="width:90px">Reference</th>
                                                                     <th style="width:150px">Line Vessel</th>
                                                                     <th style="width:90px">Container</th>
                                                                     <th style="width:100px">Traffic</th>
                                                                     <th style="width:50px">Line</th>
                                                                     <th style="width:80px">Size</th>
                                                                     <th style="width:30px">Type</th>
                                                                     <th style="width:30px">ISO</th>
                                                                     <th style="width:30px">Cargo</th>
                                                                     <th style="width:30px">Temperature</th>
                                                                     <th style="width:30px">Weight</th>
                                                                     <th style="width:30px">POL</th>
                                                                     <th style="width:30px">POL Name</th>
                                                                     <th style="width:30px">POD</th>
                                                                     <th style="width:30px">FPOD</th>
                                                                     <th style="width:250px">Shipper</th>
                                                                     <th style="width:50px">Seal_1</th>
                                                                     <th style="width:50px">Seal_2</th>
                                                                     <th style="width:50px">Seal_3</th>
                                                                     <th style="width:60px">Booking</th>
                                                                     <th style="width:120px">Gate In</th>
                                                                     <th style="width:120px">Load Date</th>
                                                                </tr>
                                                            </thead>
                                                            <tbody>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <tr class="point">
                                                            <%--<td class="nover"><%#Eval("ROW")%></td>--%>
                                                            <td><%#Eval("VESSEL")%></td>
                                                            <td><%#Eval("VOYAGE")%></td>
                                                            <td><%#Eval("REFERENCE")%></td>
                                                            <td><%#Eval("LINE VESSEL")%></td>
                                                            <td><%#Eval("CONTAINER")%></td>
                                                            <td><%#Eval("TRAFFIC")%></td>
                                                            <td><%#Eval("LINE")%></td>
                                                            <td><%#Eval("SIZE")%></td>
                                                            <td><%#Eval("TYPE")%></td>
                                                            <td><%#Eval("ISO")%></td>
                                                            <td><%#Eval("CARGO")%></td>
                                                            <td><%#Eval("TEMPERATURE")%></td>
                                                            <td><%#Eval("WEIGHT")%></td>
                                                            <td><%#Eval("POL")%></td>
                                                            <td><%#Eval("POL NAME")%></td>
                                                            <td><%#Eval("POD")%></td>
                                                            <td><%#Eval("FPOD")%></td>
                                                            <td><%#Eval("SHIPPER")%></td>
                                                            <td><%#Eval("SEAL_1")%></td>
                                                            <td><%#Eval("SEAL_2")%></td>
                                                            <td><%#Eval("SEAL_3")%></td>
                                                            <td><%#Eval("BOOKING")%></td>
                                                            <td><%#Eval("GATE IN")%></td>
                                                            <td><%#Eval("LOAD DATE")%></td>
                                                        </tr>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        </tbody> </table>
                                                    </FooterTemplate>
                                                </asp:Repeater>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <%--<div id="sinresultado" visible="false" runat="server" class="alert alert-info">--%>
                            </div>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btbuscar" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>

    <script type="text/javascript" src="../lib/jquery/jquery-3.5.1.slim.min.js" integrity="sha384-DfXdz2htPH0lsSSs5nCTpuj/zy4C+OGpamoFVy38MVBnE+IbbVYUew+OrCXaRkfj" crossorigin="anonymous"></script>
    <script src="../Scripts/pages.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.tablesorter.min.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.tablesorter.pager.js" type="text/javascript"></script>

    <script src="../Scripts/jquery.datetimepicker.js" type="text/javascript"></script>
    
    <script type="text/javascript">
        $(document).ready(function () {
            $('.datetimepicker').datetimepicker().datetimepicker({ lang: 'es', timepicker: false, closeOnDateSelect: true, format: 'd/m/Y' });
        });
    </script>


    <script type="text/javascript">
        $(document).ready(function () {
            $('.datetimepicker').datetimepicker().datetimepicker({ lang: 'es', timepicker: false, closeOnDateSelect: true, format: 'd/m/Y' });
        });

        function popupCallback(catalogo) {
            if (catalogo == null || catalogo == undefined) {
                alert('Hubo un problema al setear un objeto de catalogo');
                return;
            }

            this.document.getElementById('<%= tbooking.ClientID %>').value = catalogo.codigo;

        }

        function clear() {
            document.getElementById('numbook').textContent = '...';
            document.getElementById('nbrboo').value = '';
        }
        function clear2() {
            document.getElementById('txtfecha').textContent = '...';
            document.getElementById('xfecha').value = '';
        }

        var programacion = {};
        var lista = [];
        function prepareObject() {

            try {
                document.getElementById("loader").className = '';
                lista = [];
                //validaciones básicas
                var vals = document.getElementById('nbrboo');
                if (vals == null || vals == undefined || vals.value.trim().length <= 2) {
                    alert('* Datos de programación *\n *Escriba el numero de Booking*');
                    document.getElementById("loader").className = 'nover';
                    return;
                }
                vals = document.getElementById('xfecha');
                if (vals == null || vals == undefined || vals.value.trim().length <= 5) {
                    alert('* Datos de programación *\n Escriba la fecha de programación');
                    document.getElementById("loader").className = 'nover';
                    return;
                }

                this.programacion.booking = document.getElementById('nbrboo').value;
                this.programacion.fecha_pro = document.getElementById('xfecha').value;
                this.programacion.idlinea = document.getElementById('idlin').value;
                this.programacion.linea = document.getElementById('agencia').value;


            } catch (e) {
                alert('Lamentamos comunicarle que ha ocurrido el siguiente problema:\n' + e.Message);
            }
        }

        function openPop() {
            var bo = document.getElementById('nbrboo').value;
            if (bo == undefined || bo == '' || bo == null) {
                alert('Por favor seleccione el booking primero');
                return;
            }
            window.open('../catalogo/calendarioConsolidacion.aspx?bk=' + bo, 'name', 'width=850,height=880')
        }

    </script>
  <%--  <asp:updateprogress  id="updateProgress" runat="server">
    <progresstemplate>
        <div id="progressBackgroundFilter"></div>
        <div id="processMessage"> Estamos procesando la tarea que solicitó, por favor  espere...<br />
         <img alt="Loading" src="../shared/imgs/loader.gif" style="margin:0 auto;" />
        </div>
    </progresstemplate>
</asp:updateprogress>--%>

    


    <script type="text/javascript">
        function descarga(fname, hname, tbname) {
            var iframe = document.createElement("iframe");
            iframe.src = "../handler/fileExcel.ashx?name="+fname+"&page="+hname+"&obj="+tbname
            iframe.style.display = "none";
            document.body.appendChild(iframe);
        }
    </script>




    <asp:updateprogress  associatedupdatepanelid="upresult"   id="updateProgress" runat="server">
    <progresstemplate>
        <div id="progressBackgroundFilter"></div>
        <div id="processMessage"> Estamos procesando la tarea que solicitó, por favor 
            espere...<br />
             <img alt="Loading" src="../shared/imgs/loader.gif" style="margin:0 auto;" />
        </div>
    </progresstemplate>
</asp:updateprogress>
</asp:Content>
