<%@ Page Language="C#" Title="Carga de Vacíos" AutoEventWireup="true" 
         CodeBehind="RptEmptyCntLoad.aspx.cs" Inherits="CSLSite.rpt_econt_load" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Carga de Vacíos</title>
    <link href="../shared/estilo/catalogos.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/work.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/aisv-general.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/tables.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/print.css" rel="stylesheet" type="text/css" />

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

    <style type="text/css">
    p{ margin:0; padding:0;}
    td span { font-weight:bold; display:block; margin:0; padding:0; background-color:#CCC; text-align:right; }
   
    
    
        #bookingfrm
        {
            width: 100%;
        }
   
    
    
        .style1
        {
            width: 92%;
        }
   
    
    
    </style>

        <script language="javascript" type="text/javascript">
// <![CDATA[
            function btclear_onclick() {
                window.print()
            }
// ]]>
    </script>

    <script type="text/javascript" src="../lib/jquery/jquery-3.5.1.slim.min.js" integrity="sha384-DfXdz2htPH0lsSSs5nCTpuj/zy4C+OGpamoFVy38MVBnE+IbbVYUew+OrCXaRkfj" crossorigin="anonymous"></script>
    <script src="../Scripts/pages.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.tablesorter.min.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.tablesorter.pager.js" type="text/javascript"></script>

     <script src="../Scripts/jquery.datetimepicker.js" type="text/javascript"></script>

</head>
<body>
    <form id="bookingfrm" runat="server">
    <%--<div class="catabody" style="width:1800px; height:500px; overflow:auto">--%>
    <div class="dashboard-container p-4" id="cuerpo" runat="server">
       <div  >
   
   <table class="tabRepeat" style="width:96%; margin:0 auto;">
        <tr>
            <td colspan="2" class="style1">
                <%--<h1 style="width: 1216px">S3: Sistema de Solicitud de Servicio - Contecon Guayaquil S.A.</h1>
                <h1 style="width: 1216px">Reporte de Carga de Contenedores Vacíos</h1>--%>
                <div class="form-group col-md-12"> 
                    <div class="form-title">
                        TV: Terminal Virtual - Contecon Guayaquil S.A.
                    </div>
                    <h6>Reporte de Carga de Contenedores Vacíos</h6>
                </div>
            
            </td>
            <td rowspan="4">
                <img alt="logo"  src="../shared/imgs/logoContecon.png" width="150px" height="50px" class="float-right" />
           </td>

        </tr>
   </table>
    <div class="botonera" >
        <input id="btclear" class="btn btn-outline-primary mr-4"  type="reset"    value="Imprimir" onclick="return btclear_onclick()" />
    </div>
         <div class="cataresult-" >
             <div class="booking" >
                 <div class="form-group col-md-12"> 
                    <div class="form-title">EMPTY CONTAINERS LOAD</div>
                </div>
                <div class="bokindetalle" style=" width:100%; overflow:auto">
                     <asp:Repeater ID="tablePagination" runat="server">
                         <HeaderTemplate>
                             <table id="tabla" style=" font-size:medium"  cellspacing="1" cellpadding="1" class="table table-bordered invoice">
                             <thead>
                                 <tr>
                                    <th style="width:250px;">Vessel</th>
                                    <th style="width:60px">Voyage</th>
                                    <th style="width:100px">Reference</th>
                                    <%--<th style="width:100px">Line Vessel</th>--%>
                                    <th style="width:100px">Container</th>
                                    <th style="width:100px">Traffic</th>
                                    <th style="width:50px">Seal_1</th>
                                    <th style="width:50px">Seal_2</th>
                                    <th style="width:50px">Seal_3</th>
                                    <th style="width:50px">Line</th>
                                    <th style="width:80px">Size</th>
                                    <th style="width:30px">Type</th>
                                    <th style="width:30px">POD</th>
                                    <%--<th style="width:30px">POL</th>--%>
                                    <th style="width:30px">POL Name</th>
                                    <%--<th style="width:30px">ISO</th>--%>
                                    <th style="width:70px">FPOD</th>
                                    <th style="width:250px">Shipper</th>
                                    <th style="width:60px">Booking</th>
                                    <th style="width:120px">Gate In</th>
                                    <th style="width:120px">Load Date</th>
                                    <%--<th style="width:30px">Cargo</th>--%>
                                 </tr>
                             </thead> 
                             <tbody>
                         </HeaderTemplate>
                         <ItemTemplate>
                            <tr class="point" >
                                <td><%#Eval("VESSEL")%></td>
                                <td><%#Eval("VOYAGE")%></td>
                                <td><%#Eval("REFERENCE")%></td>
                                <%--<td><%#Eval("LINE VESSEL")%></td>--%>
                                <td><%#Eval("CONTAINER")%></td>
                                <td><%#Eval("TRAFFIC")%></td>
                                <td><%#Eval("SEAL_1")%></td>
                                <td><%#Eval("SEAL_2")%></td>
                                <td><%#Eval("SEAL_3")%></td>
                                <td><%#Eval("LINE")%></td>
                                <td><%#Eval("SIZE")%></td>
                                <td><%#Eval("TYPE")%></td>
                                <td><%#Eval("POD")%></td>
                                <%--<td><%#Eval("POL")%></td>--%>
                                <td><%#Eval("POL NAME")%></td>
                                <%--<td><%#Eval("ISO")%></td>--%>
                                <td><%#Eval("FPOD")%></td>
                                <td><%#Eval("SHIPPER")%></td>
                                <td><%#Eval("BOOKING")%></td>
                                <td><%#Eval("GATE IN")%></td>
                                <td><%#Eval("LOAD DATE")%></td>
                                <%--<td><%#Eval("CARGO")%></td>--%>
                            </tr>
                         </ItemTemplate>
                         <FooterTemplate>
                         </tbody>
                         </table>
                         </FooterTemplate>
                    </asp:Repeater>
                <</div>
            </div>
       </div>
           <%-- <div class="botonera" >
            <input id="btclear"   type="reset"    value="Imprimir" onclick="return btclear_onclick()" />
            </div>--%>
      </div>
      </div>
    </form>
</body>
</html>
