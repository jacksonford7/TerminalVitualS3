<%@ Page Language="C#" Title="Catálogo de agentes" AutoEventWireup="true" 
         CodeBehind="rptreservas.aspx.cs" Inherits="CSLSite.rptreservas_consolidacion" %>
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
		     <table  class="table">
   <tr><td colspan="2"><h1 class="form-title">Reporte de reservas para consolidación</h1></td><td rowspan="4">
       <img alt="logo"  src="../shared/imgs/logoContecon.png" width="150px" height="50px" /></td></tr>
   </table>
		   </div>
		  </div>
       	  <div class="form-row">
		  
		   <div class="form-group col-md-12"> 
		   	           <div class="cataresult" >
             <div class="booking" >
                  <div class=" modal-title">Horarios y reservas</div>
                 <asp:Repeater ID="tablePagination" runat="server">
                 <HeaderTemplate>
                 <table id="tabla" style=" font-size:medium"  cellspacing="1" cellpadding="1" class="table table-bordered invoice">
                 <thead>
                 <tr>
      
                 <th style="width:80px">Ruc</th>
                 <th style="width:60px">Booking</th>
                 <th style="width:120px">Fecha</th>
                 <th style="width:50px">Cantidad</th>
                 <th style="width:50px">Estado</th>
                 <th>Exportador</th>
                 <th style="width:30px">Desde</th>
                 <th style="width:30px">Hasta</th>
                 <th style="width:110px">Contenedores</th>

                 </tr>
                 </thead> 
                 <tbody>
                 </HeaderTemplate>
                 <ItemTemplate>
                  <tr class="point" >
                  <td><%#Eval("RUC")%></td>
                  <td><%#Eval("BOOKING")%></td>
                  <td><%#Eval("FECHA_PRG")%></td>
                  <td><%#Eval("CANTIDAD")%></td>
                  <td><%#Eval("ESTADO")%></td>
                  <td><%#Eval("EXPORTADOR")%></td>
                  <td><%#Eval("DESDE")%></td>
                  <td><%#Eval("HASTA")%></td>
                  <td><%#Eval("CNTR")%>
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
		  </div>

          <div class="form-row">
		   <div class="col-md-12 d-flex justify-content-center"> 
                           <input id="btclear" class="btn btn-primary"   type="reset"    value="Imprimir" onclick="return btclear_onclick()" />

		   </div> 
		   </div>
     </div>

    </form>
</body>
</html>
