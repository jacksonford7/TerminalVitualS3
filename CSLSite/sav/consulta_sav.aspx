<%@ Page Title="Consulta de turnos SAV" Language="C#" MasterPageFile="~/site.Master" AutoEventWireup="true" CodeBehind="consulta_sav.aspx.cs" Inherits="CSLSite.consultasav" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="placehead" runat="server">
    <link href="../shared/estilo/tables.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/aisv-general.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/work.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/catalogos.css" rel="stylesheet" type="text/css" />

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
    
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="placebody" runat="server">
    <asp:ToolkitScriptManager ID="tkcal" EnableScriptGlobalization="true" runat="server"> </asp:ToolkitScriptManager>
<%--    <div>
   <i class="ico-titulo-1"></i><h2>Servicio de Administración de Vacíos </h2>  <br /> 
   <i class="ico-titulo-2"></i><h1>Consulta, anulación y reimpresión de turnos</h1><br />
 </div>--%>

    <div class="mt-4">         
        <nav class="mt-4" aria-label="breadcrumb">
            <ol class="breadcrumb">
            <li class="breadcrumb-item" id="opcion_principal" runat="server"><a href="#">Servicio de Administración de Vacíos</a></li>
                <li class="breadcrumb-item active" aria-current="page" id="sub_opcion" runat="server">Consulta, anulación y reimpresión de turnos</li>
            </ol>
        </nav>
    </div>

    <div class="dashboard-container p-4" id="cuerpo" runat="server">

        <div class="form-title">
           DATOS DEL DOCUMENTO BUSCADO
        </div>

        <div >
            <div class="form-row" >
                 <div class="form-group   col-md-4"> 
                    <label for="inputAddress">Depósito :<span style="color: #FF0000; font-weight: bold;"></span></label>

                    
                         <asp:UpdatePanel ID="UPDEPOSITO" runat="server"  UpdateMode="Conditional">
                             <ContentTemplate>
                        <asp:DropDownList ClientIDMode="Static" ID="cmbDeposito" class="form-control"  AutoPostBack="true"  runat="server" 
                            OnSelectedIndexChanged="cmbDeposito_SelectedIndexChanged"></asp:DropDownList>
                                  </ContentTemplate>
                         </asp:UpdatePanel>
                    
                </div>

                <div class="form-group   col-md-4"> 
                    <label for="inputAddress">AISV No. :<span style="color: #FF0000; font-weight: bold;"></span></label>

                    <div class="d-flex">
                        <asp:TextBox class="form-control" ID="aisvn" runat="server" MaxLength="15" onkeypress="return soloLetras(event,'01234567890',true)"></asp:TextBox>
                    </div>
                </div>

                <div class="form-group   col-md-4"> 
                    <label for="inputAddress">Contenedor:<span style="color: #FF0000; font-weight: bold;"></span></label>

                    <div class="d-flex">
                        <asp:TextBox class="form-control" ID="cntrn" runat="server"  MaxLength="15" onkeypress="return soloLetras(event,'01234567890abcdefghijklmnopqrstuvwxyz',true)"></asp:TextBox>  
                        <span id="valran" class="opcional"> * </span>
                    </div>
                </div>

                <div class="form-group   col-md-6"> 
                    <label for="inputAddress">Generados desde el día:<span style="color: #FF0000; font-weight: bold;"></span></label>

                    <div class="d-flex">
                        <asp:TextBox class="form-control" ID="desded" runat="server" MaxLength="10" CssClass="datetimepicker form-control" onkeypress="return soloLetras(event,'01234567890/')" onblur="valDate(this,true,valdate);" ClientIDMode="Static"></asp:TextBox>
                    </div>
                </div>

                <div class="form-group   col-md-6"> 
                    <label for="inputAddress">Hasta:<span style="color: #FF0000; font-weight: bold;"></span></label>

                    <div class="d-flex">
                        <asp:TextBox CssClass="datetimepicker form-control" ID="hastad" runat="server" ClientIDMode="Static" MaxLength="15" onkeypress="return soloLetras(event,'01234567890/')" onblur="valDate(this,true,valdate);"></asp:TextBox>
                        <span id="valdate" class="validacion"> * </span>
                    </div>
                </div>
            </div>

            <div class="form-group col-md-12"> 
                <div class="row">
                    <div class="col-md-12 d-flex justify-content-center">
                        <asp:Button class="btn btn-primary"  ID="btbuscar" runat="server" Text="Iniciar la búsqueda"  onclick="btbuscar_Click"  OnClientClick="return validateDatesRange('desded','hastad','imagen');" />
                    </div>
                </div>
            </div>

        </div>

        <div><br /></div>

        <div class="form-group col-md-12"> 
            <div class="cataresult" >
                <asp:UpdatePanel ID="upresult" runat="server" ChildrenAsTriggers="true">
                    <ContentTemplate>
                        <script type="text/javascript">
                            Sys.Application.add_load(BindFunctions); 
                        </script>
                        <div class="alert alert-warning" runat="server" id="mensaje" style=" font-weight:bold">
                            <span id="saldo" runat="server">Estimado Cliente,</span>
                        </div>  
                        <div id="xfinder" runat="server" visible="false" >
                            <div class="alert alert-warning" id="alerta" runat="server" >
                            Confirme que los datos sean correctos. En caso de error, favor comuníquese 
                            con el Departamento de Planificación a los teléfonos: +593 
                            (04) 6006300, 3901700 
                            </div>
                            <div class="findresult" >
                                <div class="booking" >
                    
                                    <div class="form-group col-md-12"> 
                                        <div class="form-title">Documentos encontrados</div>
                                    </div>

                                    <div class="bokindetalle" style=" width:100%; overflow:auto">
                                        <asp:Repeater ID="tablePagination" runat="server" 
                                                onitemcommand="tablePagination_ItemCommand" 
                                                OnItemDataBound="tablePagination_ItemDataBound">
                                                <HeaderTemplate>
                                                <table id="tablasort" cellspacing="1" cellpadding="1" class="table table-bordered invoice" >
                                                <thead>
                                                <tr>
                
                                                <th>AISV #</th>
                                                <th>Contenedor</th>
                                                <th>Fecha Turno</th>
                                                <th>Hora Turno</th>
                
                                                <th>Registrado</th>
             
                                                <th>Estado</th>
                
                                                <th>Acciones</th>
                                                <th id="imprimir_prof" runat="server" ><asp:Label Text='Imprimir Liquidación' ID="lbl_tit_imprimir" runat="server" visible="true"/></th>
                                                <th id="idproforma" style=" width:50px; display:none">Proforma</th>
                                              
                                                </tr>
                                                </thead> 
                                                <tbody>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                <tr class="point" >
                  
                                                <td><%#Eval("turno_id")%></td>
                                                <td><%#Eval("unidad_id")%></td>
                                                <td><%#Eval("turno_fecha")%></td>
                                                <td><%#Eval("turno_hora")%></td>
                                                <td><%#Eval("creado_fecha")%></td>
                                                <td><asp:Label Text='<%#anulado(Eval("active"))%>' Width="70px" id="lblEstado" runat="server" /></td>
                  

                                                <td>
                                                <div class="tcomand">
                                                    <a href="../sav/printaisvsav.aspx?sid=<%# securetext(Eval("turno_id")) %>" class="imprimir" target="_blank">Imprimir</a>|
                                                    <div class='<%# boton(Eval("active"))%>' >
                                                    <asp:Button ID="btanula"  
                                                    OnClientClick="return confirm('Esta seguro que desea eliminar este documento?');" 
                                                    CommandArgument=   '<%# jsarguments( Eval("turno_id"),Eval("unidad_id"),Eval("ID_PASE_LINEA") )%>' CommandName="eliminar" runat="server" Text="Anular" CssClass="Anular" ToolTip="Permite anular este documento" />
                                                    </div>
                                                </div>
                                                </td>
                                                <td id="imprimir_prof_det" runat="server" style=" width:90px">
                                                    <asp:Label Text='<%#Eval("estado_pago")%>' Width="70px" id="LblEstadoPago" runat="server" />
                                                    <br></br>
                                                    <asp:ImageButton 
                                                        CommandArgument='<%# jsarguments( Eval("turno_id"), Eval("documento_id"), Eval("ID_PASE_LINEA"))%>'
                                                        ImageUrl="~/shared/imgs/action_print.gif" ID="btnImprimirProforma" ToolTip="Imprimir Liquidación" CommandName="Imprimir_proforma" runat="server" visible="true"/>
                                                </td>
                                                <td style=" width:50px; display:none"><%#Eval("documento_id")%></td>
                                              
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
                                    <input  type="text" class="pagedisplay"  size="10"/>
                                    <img alt="" src="../shared/imgs/next.gif" class="next"/>
                                    <img alt="" src="../shared/imgs/last.gif" class="last"/>
                                    &nbsp;
                            </div>
                        </div>
                    <div id="sinresultado" runat="server" class="alert alert-info"></div>
                    </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btbuscar" />
                        </Triggers>
                </asp:UpdatePanel>
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
  </script>

<%--  <asp:updateprogress associatedupdatepanelid="upresult"
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
