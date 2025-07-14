<%@ Page Title="Liberación de Pase ZAL/CISE/ZEA" Language="C#" MasterPageFile="~/site.Master" AutoEventWireup="true" CodeBehind="liberacion_pase_zal.aspx.cs" Inherits="CSLSite.liberacion_pase_zal" %>

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
                    <li class="breadcrumb-item active" aria-current="page" id="sub_opcion" runat="server">Liberación de Pase ZAL/CISE/ZEA</li>
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
                    
                        <asp:TextBox  class="form-control" ID="TxtNumero" runat="server" MaxLength="10" onkeypress="return soloLetras(event,'01234567890',true)"></asp:TextBox>
                        <span id="valopc" class="opcional"></span>
                    </div>
                </div>

                <div class="form-group col-md-6"> 
                    <label for="inputAddress">Número Liquidación:<span style="color: #FF0000; font-weight: bold;"></span></label>
                    <div class="d-flex"> 
                        <asp:TextBox class="form-control" ID="TxtContenedor" runat="server" MaxLength="25" onkeypress="return soloLetras(event,'abcdefghijklmnopqrstuvwxyz01234567890/')"></asp:TextBox>
                        <span id="valran2" class="validacion"></span>
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
            
   
            <asp:UpdatePanel ID="UdMotivo" runat="server" ChildrenAsTriggers="true" >
                <ContentTemplate>   
                    <div class="form-row" id="motivo" runat="server" visible="false"> 
                        <div class="form-group col-md-12"> 
                        <label for="inputAddress">Motivo:<span style="color: #FF0000; font-weight: bold;">*</span></label>
                        <asp:TextBox  class="form-control" ID="TxtMotivo" runat="server" MaxLength="100" ></asp:TextBox>   
                        </div>
                    </div>
                  </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btbuscar" />
                </Triggers>
             </asp:UpdatePanel>  
           
            <asp:UpdatePanel ID="UdBotones" runat="server" ChildrenAsTriggers="true"  UpdateMode="Conditional">
                <ContentTemplate>   
                     <div class="col-md-12 d-flex justify-content-center" id="botones" runat="server" visible="false">
                   
                        <span id="imagen2" ></span>
                            
                         <asp:Button class="btn btn-primary" ID="BtnProcesar" runat="server" Text="Liberar Pases"   onclick="btnprocesar_Click"  OnClientClick="return confirm('Esta seguro de que desea liberar los pases?');"  />
                    </div>
                </ContentTemplate>
                <Triggers>
                  <asp:AsyncPostBackTrigger ControlID="BtnProcesar" />
                </Triggers>
             </asp:UpdatePanel>  

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
                                                                <th id="boocking" >Booking</th>
                                                                <th id="referencia" >Referencia</th>
                                                                <th id="chofer">Chofer</th>
                                                                <th id="placa">Placa</th>
                                                                <th id="liquidacion">Liquidación</th>
                                                                <th id="estado">Estado Pago</th>
                                                                <th id="bodega">Bodega</th>
                                                                 <th id="valor">Valor</th>
                                                            </tr>
                                                            </thead> 
                                                            <tbody>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <tr class="point" >
                                                                <td ><asp:Label ID="txtpase" runat="server" Text='<%#Eval("ID_PPZAL")%>' /> </td>
                                                                <td ><%#Eval("FECHA_SALIDA")%> </td>
                                                                <td ><%#Eval("TURNO")%> </td>
                                                                <td ><%#Eval("BOOKING")%></td>
                                                                <td ><%#Eval("REFERENCIA")%></td>
                                                                <td ><%#Eval("CHOFER")%></td>
                                                                <td ><%#Eval("PLACA")%></td>
                                                                <td > <%#Eval("LIQUIDACION")%></td>
                                                                <td  > <%#Eval("ESTADO_PAGO")%></td>
                                                                <td  > <%#Eval("DEPOSITO")%></td>
                                                                <td  > <%#Eval("valor")%></td>
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
                                <div id="sinresultado" runat="server" class="alert alert-info">></div>
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
    
    

  </script>

 <%-- <asp:updateprogress associatedupdatepanelid="upresult"
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
