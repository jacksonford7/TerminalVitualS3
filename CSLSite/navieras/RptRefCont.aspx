<%@ Page Language="C#" Title="Reporte de Salida" AutoEventWireup="true" 
         CodeBehind="RptRefCont.aspx.cs" Inherits="CSLSite.rpt_ref_cont" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Reporte de Salida</title>
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
   
    
    
        .style1
        {
            width: 32px;
        }
        #bookingfrm
        {
            width: 100%;
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
    <div class="dashboard-container p-4" id="cuerpo" runat="server">
       <div  >
           <table class="tabRepeat" style="width:96%; margin:0 auto;">
                <tr>
                    <td colspan="2" class="style1">
            
                        <div class="form-group col-md-12"> 
                            <div class="form-title">
                                TV: Terminal Virtual - Contecon Guayaquil S.A.
                            </div>
                            <h6>Reporte de Contenedores Reefer</h6>
                        </div>

                    </td>
                    <td rowspan="4">
                        <img alt="logo"  src="../shared/imgs/logoContecon.png" width="150px" height="50px" class="float-right" />
                    </td></tr>
            </table>
            <div class="botonera" >
                <input id="btclear" class="btn btn-outline-primary mr-4"  type="reset"    value="Imprimir" onclick="return btclear_onclick()" />
            </div>   

         <div class="cataresult" >
             <div class="booking" >
                 <div class="form-group col-md-12"> 
                       <div class="form-title">CONTAINERS REEFER</div>
                  </div>
                 <div class="bokindetalle" style=" width:100%; overflow:auto">
                     <asp:Repeater ID="tablePagination" runat="server">
                     <HeaderTemplate>
                     <table id="tabla" style=" font-size:medium"  cellspacing="1" cellpadding="1" class="table table-bordered invoice">
                     <thead>
                     <tr>
                     <th style="width:250px;">Vessel</th>
                     <th style="width:60px">Voyage</th>
                     <th style="width:70px">Reference</th>
                     <th style="width:30px">Container</th>
                     <th style="width:30px">Size</th>
                     <th style="width:30px">Type</th>
                     <th style="width:120px">Bill of Lading</th>
                     <th style="width:60px">Booking</th>
                     <th style="width:50px">Document</th>
                     <th style="width:80px">Type Doc</th>
                     <th style="width:70px">FPOD</th>
                     <th style="width:30px">Time</th>
                     <%--<th>Exportador</th>--%>
                     <th style="width:100px">Event</th>
                     <th style="width:30px">Temp</th>
                     <th style="width:30px">O2</th>
                     <th style="width:110px">CO2</th>
                     <th style="width:110px">Unit</th>
                     <th style="width:110px">ReadMode</th>
                     </tr>
                     </thead> 
                     <tbody>
                     </HeaderTemplate>
                     <ItemTemplate>
                      <tr class="point" >
                      <td><%#Eval("VESSEL")%></td>
                      <td><%#Eval("VOYAGE")%></td>
                      <td><%#Eval("REFERENCE")%></td>
                      <td><%#Eval("CONTAINER")%></td>
                      <td><%#Eval("SIZE")%></td>
                      <td><%#Eval("TYPE")%></td>
                      <td><%#Eval("BILL OF LADING")%></td>
                      <td><%#Eval("BOOKING")%></td>
                      <td><%#Eval("DOCUMENT")%></td>
                      <td><%#Eval("TYPE DOC")%></td>
                      <td><%#Eval("FPOD")%></td>
                      <td><%#Eval("TIME")%></td>
                      <td><%#Eval("EVENT")%></td>
                      <td><%#Eval("TEMP")%></td>
                      <td><%#Eval("O2")%></td>
                      <td><%#Eval("CO2")%></td>
                      <td><%#Eval("UNIT")%></td>
                      <td><%#Eval("READMODE")%></td>
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
            <%--<div class="botonera" >
            <input id="btclear"   type="reset"    value="Imprimir" onclick="return btclear_onclick()" />
            </div>--%>
      </div>
      </div>
    </form>
</body>
</html>
