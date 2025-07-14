<%@ Page Title="Consultar Archivos de EIR Pases Zal" Language="C#" MasterPageFile="~/site.Master" AutoEventWireup="true" 
    CodeBehind="visualizararchivo.aspx.cs" Inherits="CSLSite.visualizararchivo" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="placehead" runat="server">
    <link href="../shared/estilo/tables.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/aisv-general.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/work.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/catalogos.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/info.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/opc_tables.css" rel="stylesheet" />

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
            function BindFunctions() {
                $(document).ready(function () {
                    document.getElementById('imagen').innerHTML = '';
                    $('#tablasort').tablesorter({ cancelSelection: true, cssAsc: "ascendente", cssDesc: "descendente", headers: { 8: { sorter: false }, 9: { sorter: false}} }).tablesorterPager({ container: $('#pager'), positionFixed: false, pagesize: 10 });
                });
            }
    </script>
    <style type="text/css">
        
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
    
        .auto-style2 {
            border-bottom: 1px solid #CCC;
            width: 697px;
        }
    
        </style>
    <script type="text/javascript" src="../lib/jquery/jquery-3.5.1.slim.min.js" integrity="sha384-DfXdz2htPH0lsSSs5nCTpuj/zy4C+OGpamoFVy38MVBnE+IbbVYUew+OrCXaRkfj" crossorigin="anonymous"></script>
    <script src="../Scripts/pages.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.tablesorter.min.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.tablesorter.pager.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.datetimepicker.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="placebody" runat="server">
    <asp:ToolkitScriptManager ID="tkcal" EnableScriptGlobalization="true" runat="server"> </asp:ToolkitScriptManager>
  
    <div class="mt-4">         
        <nav class="mt-4" aria-label="breadcrumb">
            <ol class="breadcrumb">
            <li class="breadcrumb-item" id="opcion_principal" runat="server"><a href="#">Depósito de Vacíos</a></li>
                <li class="breadcrumb-item active" aria-current="page" id="sub_opcion" runat="server">Consultar Archivos EIR - e-Pass Zal/CISE/ZEA</li>
            </ol>
        </nav>
    </div>

    <div class="dashboard-container p-4" id="cuerpo" runat="server">

        <div class="form-title">
            Filtros para el reporte
        </div>

        <div class="form-row">
            
            <div class="form-group col-md-6"> 
                <label for="inputAddress">Número De Pase Zal:<span style="color: #FF0000; font-weight: bold;"></span></label>
                <div class="d-flex"> 
                    
                    <asp:TextBox ID="TxtNumero" class="form-control" runat="server" MaxLength="10" onkeypress="return soloLetras(event,'01234567890',true)"></asp:TextBox>
                    <span id="valran" class="opcional"> * </span>
                </div>
            </div>

            <div class="form-group col-md-3"> 
                <label for="inputAddress">Fecha Desde:<span style="color: #FF0000; font-weight: bold;"></span></label>
                <div class="d-flex"> 
                    <asp:TextBox ID="desded" runat="server" MaxLength="10" CssClass="datetimepicker form-control" onkeypress="return soloLetras(event,'01234567890/')" ClientIDMode="Static"></asp:TextBox>
                </div>
            </div>

            <div class="form-group col-md-3"> 
                <label for="inputAddress">Fecha Hasta:<span style="color: #FF0000; font-weight: bold;"></span></label>
                <div class="d-flex"> 
                    <asp:TextBox ID="hastad" runat="server" ClientIDMode="Static" MaxLength="15" CssClass="datetimepicker form-control" onkeypress="return soloLetras(event,'01234567890/')"  ></asp:TextBox>
                    <span id="valdate" class="validacion"> * </span>
   
                </div>
            </div>
        </div>
       
        <div class="row">
            <div class="col-md-12 d-flex justify-content-center">
                <asp:Button ID="btbuscar" runat="server" Text="Iniciar la búsqueda" 
                        class="btn btn-primary" 
                        onclick="btbuscar_Click" />
                    <span id="imagen"></span>
            </div>
        </div>    
            
     
        <div class="form-row">
            <div class="form-group col-md-12">               
                <div class="cataresult" >

                    <asp:UpdatePanel ID="upresult" runat="server" ChildrenAsTriggers="true" >
                        <ContentTemplate>
                            <script type="text/javascript">
                                Sys.Application.add_load(BindFunctions); 
                            </script>
                            <div><br /></div>
                            <div id="xfinder" runat="server" visible="false" >
                                <div ><br /></div>
                                 <div class="findresult" >
                                     <div class="booking" >
                                          
                                         <div class="form-group col-md-12"> 
                                            <div class="form-title">Detalle de Pases Zal</div>
                                        </div>

                                         <div class="bokindetalle" style=" width:100%; overflow:auto">
                                         <asp:Repeater ID="tablePagination" runat="server" 
                                                 onitemcommand="tablePagination_ItemCommand" >
                                             <HeaderTemplate>
                                                 <table id="tablasort" cellspacing="1" cellpadding="1" class="table table-bordered invoice">
                                                     <thead>
                                                         <tr>
                                                             <th id="pase" ># Pase</th>
                                                             <th id="fecha" >Fecha de Salida</th>
                                                             <th id="turno" >Turno</th>
                                                             <th id="contenedor" >Contenedor</th>
                                                             <th id="boocking" >Booking</th>
                                                             <th id="referencia" >Referencia</th>
                                                             <th id="chofer">Chofer</th>
                                                             <th id="placa">Placa</th>
                                                             <th id="liquidacion">Liquidación</th>
                                                             <th id="estado">Estado Pago</th>
                                                             <th id="archivo">Archivo</th>

                                                         </tr>
                                                     </thead> 
                                                 <tbody>
                                             </HeaderTemplate>
                                             <ItemTemplate>
                                                 <tr class="point" >
                                                      <td ><%#Eval("ID_PPZAL")%> </td>
                                                      <td ><%#Eval("FECHA_SALIDA")%> </td>
                                                      <td ><%#Eval("TURNO")%> </td>
                                                      <td ><%#Eval("CONTENEDOR")%></td>
                                                      <td ><%#Eval("BOOKING")%></td>
                                                      <td ><%#Eval("REFERENCIA")%></td>
                                                      <td ><%#Eval("CHOFER")%></td>
                                                      <td ><%#Eval("PLACA")%></td>
                                                      <td > <%#Eval("LIQUIDACION")%></td>
                                                      <td  > <%#Eval("ESTADO_PAGO")%></td>
                                                      <td  > <%#Eval("ruta_documento")%></td>
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
                                     <input clientidmode="Static" id="dataexport" onclick="getTable('resultado');" type="button" value="Exportar" runat="server" />
                                </div>
                            </div>
                            <div id="sinresultado" runat="server" class="msg-info"></div>
                      </ContentTemplate>
                         <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btbuscar" />
                            <%--<asp:AsyncPostBackTrigger ControlID="BtnProcesar" />--%>
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
    <script type="text/javascript">
        $(document).ready(function () {
              $('.datetimepicker').datetimepicker().datetimepicker({ lang: 'es', timepicker: false, closeOnDateSelect: true, format: 'd/m/Y' });
        });
    </script>

    <script src="../Scripts/jquery.datetimepicker.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $('.datetimepicker').datetimepicker().datetimepicker({ lang: 'es', timepicker: false, closeOnDateSelect: true, format: 'd/m/Y' });
        });
    </script>

<%-- <script src="../Scripts/nota_credito.js" type="text/javascript"></script>--%>
  <%--<asp:updateprogress associatedupdatepanelid="upresult"
    id="updateProgress" runat="server">
    <progresstemplate>
        <div id="progressBackgroundFilter"></div>
        <div id="processMessage"> Estamos procesando la tarea que solicitó, por favor 
            espere...<br />
             <img alt="Loading" src="../shared/imgs/loader.gif" style="margin:0 auto;" />
        </div>
    </progresstemplate>
</asp:updateprogress>--%>
  </asp:Content>
