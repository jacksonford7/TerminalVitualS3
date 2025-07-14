<%@ Page Language="C#" Title="Catálogo de agentes" AutoEventWireup="true" CodeBehind="rptreservas.aspx.cs" Inherits="CSLSite.rptreservas" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Catálogo de agentes</title>
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
    


    <style type="text/css">
    p{ margin:0; padding:0;}
    td span { font-weight:bold; display:block; margin:0; padding:0; background-color:#CCC; text-align:right; }
   
    
    
    </style>

        <script language="javascript" type="text/javascript">
// <![CDATA[
            function btclear_onclick() {
                window.print()
            }
// ]]>
    </script>

</head>
<body>
    <form id="bookingfrm" runat="server">
        <div class="dashboard-container p-4" id="cuerpo" runat="server">
		  <div class="form-row">
		  
		   <div class="form-group col-md-12"> 
		   	 <table class="tabRepeat" style="width:96%; margin:0 auto;">
   <tr><td colspan="2">

       <div class="form-title">Reporte de reservas para consolidación</div>

       </td>
       <td rowspan="4">
       <img alt="logo"  src="../shared/imgs/logoContecon.png" width="150px" height="50px" /></td></tr>
   </table>
		   </div>
		  </div>
		    <div class="cataresult" >
             <div class="booking" >
                  <div class="form-title">Horarios y Reservas</div>
                 <asp:Repeater ID="tablePagination" runat="server"  >
                 <HeaderTemplate>
                 <table id="tabla"  cellspacing="1" cellpadding="1" class="table table-bordered invoice">
                 <thead>
                 <tr>
                 <th>#</th>
                 <th>Linea</th>
                 <th>Fecha</th>
                 <th>Booking</th>
                 <th>Exportador</th>
                 <th>Desde</th>
                 <th>Hasta</th>
                 <th>Total</th>
                 <th>Reservado</th>
                 <th>Disponible</th>
                 <th>Cancelado</th>
                 </tr>
                 </thead> 
                 <tbody>
                 </HeaderTemplate>
                 <ItemTemplate>
                 <tr class="point" >
                  <td><%#Eval("ROW")%></td>
                  <td><%#Eval("LINEA")%></td>
                  <td><%#Eval("FECHA_PRG")%></td>
                  <td><%#Eval("BOOKING")%></td>
                  <td><%#Eval("EXPORTADOR")%></td>
                  <td><%#Eval("DESDE")%></td>
                  <td><%#Eval("HASTA")%></td>
                  <td><%#Eval("TOTAL")%></td>
                  <td><%#Eval("RESERVADO")%></td>
                  <td><%#Eval("DISPONIBLE")%></td>
                  <td><%#Eval("CANCELADO")%></td>
                 </tr>
                  </ItemTemplate>
                 <FooterTemplate>
                 </tbody>
                 </table>
                 </FooterTemplate>
         </asp:Repeater>
             </div>
     

       </div>
            <div class="botonera" >
         <input id="btclear"   type="reset"    class="btn btn-primary"
             value="Imprimir" onclick="return btclear_onclick()" />
     </div>
     </div>





    </form>
</body>
</html>
