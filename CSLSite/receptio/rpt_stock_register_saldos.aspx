<%@ Page Title="Detalle de saldos" Language="C#" MasterPageFile="~/site.Master" AutoEventWireup="true" 
    CodeBehind="rpt_stock_register_saldos.aspx.cs" Inherits="CSLSite.rpt_stock_register_saldos" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>

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


    <link href="../shared/estilo/opc_tables.css" rel="stylesheet" />
        <script type="text/javascript">
            function BindFunctions() {
                $(document).ready(function () {
                    document.getElementById('imagen').innerHTML = '';
                    $('#tablasort').tablesorter({ cancelSelection: true, cssAsc: "ascendente", cssDesc: "descendente", headers: { 8: { sorter: false }, 9: { sorter: false}} }).tablesorterPager({ container: $('#pager'), positionFixed: false, pagesize: 10 });
                });
            }

       function validateDatesRange2(desde, hasta,ctrl) {
        var desde = document.getElementById(desde);
        var hasta = document.getElementById(hasta);
        if (desde == undefined || desde == null || hasta == undefined || hasta == null) {
            alert('Por favor agregue o revise las fechas desde/hasta');
            return false;
        }

        var datePart = desde.value.split('/');
        var date = new Date(datePart[1] + '/' + datePart[0] + '/' + datePart[2]);
        if (!date instanceof Date || isNaN(date.valueOf())) {
            alert('Por favor revise/escriba la fecha desde');
            return false;
        }
        var datePart2 = hasta.value.split('/');
        var date2 = new Date(datePart2[1] + '/' + datePart2[0] + '/' + datePart2[2]);
        if (!date2 instanceof Date || isNaN(date2.valueOf())) {
            alert('Por favor revise/escriba la fecha hasta');
            return false;
        }
        if (date > date2) {
            alert('El rango de fechas está incorrecto, por favor revise');
            return false;

        }
        if ((date.getFullYear() - date2.getFullYear()) != 0) {
            alert('El rango máximo de registros es de 1 año');
            return false;
        }

       var dias = date2 - date
       var diff_ = dias / (1000 * 60 * 60 * 24);
       if (diff_ > 14) {
             alert('Solo puede solicitar información en rango menor o igual a 2 semanas');
            return false;
        }
           

          

        //var mini = parseInt(datePart[1]);  var mfin = parseInt(datePart2[1]);
        //if (mini != mfin) {
        //    alert('Solo puede solicitar documentos en rango de 1 mes, gracias por entender' + datePart[0]);
        //    return false;
        //}

        if (ctrl != null && ctrl != undefined && ctrl.trim().length > 0) {
            document.getElementById(ctrl).innerHTML = '<img alt="" src="../shared/imgs/loader.gif">'
        }
        return true;
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
            width: 220px;
        }
    
        .auto-style3 {
            width: 192px;
        }
    
        .auto-style4 {
            border-bottom: 1px solid #CCC;
            width: 94px;
        }
    
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="placebody" runat="server">
    <asp:ToolkitScriptManager ID="tkcal" EnableScriptGlobalization="true" runat="server"> </asp:ToolkitScriptManager>
<%--    <div>
  
   <i class="ico-titulo-2"></i><h1>Consulta Detallada De Saldo de Contenedores Vacíos Por Depósitos</h1><br />
 </div>--%>

     <div class="mt-4">         
        <nav class="mt-4" aria-label="breadcrumb">
            <ol class="breadcrumb">
            <li class="breadcrumb-item" id="opcion_principal" runat="server"><a href="#">Depósito de Vacíos</a></li>
                <li class="breadcrumb-item active" aria-current="page" id="sub_opcion" runat="server">Consulta Detallada De Saldo de Contenedores Vacíos Por Depósitos</li>
            </ol>
        </nav>
    </div>

    <div class="dashboard-container p-4" id="cuerpo" runat="server">

        <div class="form-title">
            Criterios de búsquedas
        </div>

        <div >

            <div class="form-row" >
                 <div class="form-group col-md-6"> 
                    <label for="inputAddress">Deposito :<span style="color: #FF0000; font-weight: bold;"></span></label>
                    <asp:DropDownList ID="CboDeposito" class="form-control" runat="server" Font-Size="Medium"  AutoPostBack="False" DataTextField='name' DataValueField='id_depot'  ></asp:DropDownList>
                    <a class="tooltip" ><span class="classic" >Ruta asociada al servicio</span><img alt='' src='../shared/imgs/info.gif' class='datainfo'/></a>
                </div>

                <div class="form-group   col-md-6"> 
                    <label for="inputAddress">Línea Naviera:<span style="color: #FF0000; font-weight: bold;"></span></label>

                    <div class="d-flex">
                        <asp:Label class="form-control" ID="LblNombre" runat="server" Text="LblNombre" ></asp:Label>
                        <span id="valran" class="opcional"></span>
                    </div>
                </div>

                <div class="form-group   col-md-6"> 
                    <label for="inputAddress">Generados desde el día:<span style="color: #FF0000; font-weight: bold;"></span></label>

                    <div class="d-flex">
                        <asp:TextBox ID="desded" runat="server" MaxLength="10"  CssClass="datetimepicker form-control"
                         onkeypress="return soloLetras(event,'01234567890/')" 
                             onblur="valDate(this,true,valdate);" ClientIDMode="Static"></asp:TextBox>
                    </div>

                </div>

                <div class="form-group   col-md-6"> 
                    <label for="inputAddress">Hasta:<span style="color: #FF0000; font-weight: bold;"></span></label>

                    <div class="d-flex">
                        <asp:TextBox ID="hastad" runat="server" ClientIDMode="Static" MaxLength="15"  CssClass="datetimepicker form-control"
                            onkeypress="return soloLetras(event,'01234567890/')" 
                            onblur="valDate(this,true,valdate);"></asp:TextBox>
                        <span id="valdate" class="validacion"> * </span>
                    </div>

                </div>

            </div>

            <div class="row">
                <div class="col-md-12 d-flex justify-content-center">
                    <asp:Button ID="btbuscar" runat="server" Text="Generar Consulta" 
                        class="btn btn-primary" 
                        onclick="btbuscar_Click" 
                        OnClientClick="return validateDatesRange2('desded','hastad','imagen');" />
                    <span id="imagen"></span>
                </div>
            </div>

        </div>

        <div><br /></div>

         <div >
            <div class="form-row" >
                <div class="form-group col-md-12"> 
                    <div class="cataresult" >

                        <div id="xfinder" runat="server" visible="true" >
             
                            <div class="findresult" >
                                <div class="booking" >
                                    <%--<div class="separator">Documentos encontrados</div>--%>

                                    <div class="form-group col-md-12"> 
                                        <div class="form-title">Documentos encontrados</div>
                                    </div>

                                    <div class="bokindetalle" style=" width:100%; overflow:auto">
                                        <rsweb:reportviewer ID="ReportViewer1" runat="server" Font-Names="Verdana" 
                                        Font-Size="8pt" InteractiveDeviceInfos="(Colección)" 
                                        WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt" 
                                        >
                                        <LocalReport ReportPath="receptio\Rpt_Stock_Register_Detallado.rdlc">
                                            <DataSources>
                                                <rsweb:ReportDataSource DataSourceId="SqlDataSource_Stock" 
                                                    Name="pc_rpt_stock_register_saldos" />
                                            </DataSources>
                                        </LocalReport>
                                        </rsweb:reportviewer>
                                        <asp:SqlDataSource ID="SqlDataSource_Stock" runat="server" ConnectionString="<%$ ConnectionStrings:receptio %>" SelectCommand="pc_rpt_stock_register_saldos" SelectCommandType="StoredProcedure">
                                        <SelectParameters>  
                                            <asp:SessionParameter DbType="Int32" Name="i_id_line" SessionField="id_line" />
                                                <asp:SessionParameter   Type="Int32" Name="i_id_depot"  SessionField="id_depot"/>
                                                <asp:SessionParameter  Type="String"  Name="i_fecha_desde"   SessionField="fecha_desde" />
                                                <asp:SessionParameter   Type="String"  Name="i_fecha_hasta" SessionField="fecha_hasta" />                      
                                        </SelectParameters>
                                        </asp:SqlDataSource>



                                    </div>
                                </div>
                            </div>

            
                        </div>

                        <div id="sinresultado" runat="server" class="alert alert-info"></div>
                
                    </div>
                </div>
            </div>
        </div>


       <%--<div class="accion">--%>
            <%--<table class="xcontroles" cellspacing="0" cellpadding="1">
                <tr><th class="bt-bottom bt-right  bt-left bt-top" colspan="5">Criterios de búsquedas</th></tr>
                 <tr>
                 <td  class="auto-style3" >Depósito:</td>
                 <td class="bt-bottom" >
            
                     <asp:DropDownList ID="CboDeposito" runat="server" Width="180px" AutoPostBack="False"
                                                    Height="28px"  DataTextField='name' DataValueField='id_depot'  
                                                    Font-Size="Small"  >
                                                </asp:DropDownList>
                  </td>
                   <td class="auto-style4" >Línea Naviera:</td>
                  <td class="bt-bottom" >
                    <asp:Label ID="LblNombre" runat="server" Text="LblNombre" Width="150px"></asp:Label>
                  </td>
                 <td class="bt-bottom bt-right validacion "><span id="valran" class="opcional"></span></td>
                 </tr>

                 <tr>
                 <td class="auto-style3">Generados desde el día:</td>
                 <td class="bt-bottom">
                    <asp:TextBox ID="desded" runat="server" Width="120px" MaxLength="10" CssClass="datetimepicker"
                     onkeypress="return soloLetras(event,'01234567890/')" 
                         onblur="valDate(this,true,valdate);" ClientIDMode="Static"></asp:TextBox>

                     </td>
                  <td class="auto-style4" >Hasta:</td>
                  <td class="bt-bottom " >
                     <asp:TextBox ID="hastad" runat="server" ClientIDMode="Static" Width="120px" MaxLength="15" CssClass="datetimepicker"
                     onkeypress="return soloLetras(event,'01234567890/')" 
                          onblur="valDate(this,true,valdate);"></asp:TextBox>
   
                  </td>
                 <td class="bt-bottom bt-right validacion ">
                 <span id="valdate" class="validacion"> * obligatorio</span>
                 </td>
                 </tr>

            </table>--%>

    <%--<table>
        <tr>
            <td class="auto-style2">
               
            </td>
           
        </tr>
    </table>--%>

         <%--<div class="botonera">
          <span id="imagen"></span>
             <asp:Button ID="btbuscar" runat="server" Text="Generar Consulta"   onclick="btbuscar_Click" OnClientClick="return validateDatesRange2('desded','hastad','imagen');" />
         </div>--%>



             
      <%--</div>--%>

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

  </asp:Content>
