<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="bookingdetalle.aspx.cs" Inherits="CSLSite.bookingdetalle" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Catálogo de bookings</title>

    <link href="../img/favicon2.png" rel="icon"/>
  <link href="../img/icono.png" rel="apple-touch-icon"/>
  <link href="../css/bootstrap.min.css" rel="stylesheet"/>
  <link href="../css/dashboard.css" rel="stylesheet"/>
  <link href="../css/icons.css" rel="stylesheet"/>
  <link href="../css/style.css" rel="stylesheet"/>
  <link href="https://fonts.googleapis.com/css2?family=Roboto:wght@300;400;500;700&display=swap" rel="stylesheet"/>

    <link href="../shared/estilo/work.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/pages.js" type="text/javascript"></script>
    <link href="../shared/estilo/aisv-general.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/tables.css" rel="stylesheet" type="text/css" />
   
       <!--Datatables-->
       <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/5.15.1/css/all.min.css" integrity="sha512-+4zCK9k+qNFUR5X+cKL9EIR+ZOhtIloNl9GIKS57V1MyNsYpYcUrUeQc9vNfzsWfV28IaLL3i96P9sdNyeRssA==" crossorigin="anonymous" />
       <link href="../css/datatables.min.css" rel="stylesheet" />
       <link rel="stylesheet" type="text/css" href="..js/buttons/css1.6.4/buttons.dataTables.min.css"/>
        <script src="../lib/jquery/jquery.min.js" type="text/javascript"></script>
       <script type="text/javascript"  src="../js/datatables.js"></script>
       <script src="../Scripts/table_catalog.js" type="text/javascript"></script>  
       <script type="text/javascript" src="../js/bootstrap.bundle.min.js"></script>
    <!--Fin-->

</head>
<body>

    <form id="bookingfrm" runat="server">
    <asp:ToolkitScriptManager ID="tkscata" runat="server"></asp:ToolkitScriptManager>
             <div id="xfinder" runat="server" visible="false" >
          <div class="dashboard-container p-4" id="cuerpo" runat="server">
         <div class="form-title">Booking</div>
		  <div class="form-row">
		  
		   <div class="form-group col-md-3"> 
		   	  <label for="inputAddress">Número<span style="color: #FF0000; font-weight: bold;"></span></label>
			  <span id="numero" runat="server" class="form-control">REM00006689</span>
		   </div>

               <div class="form-group col-md-3"> 
		   	  <label for="inputAddress">FreightKind<span style="color: #FF0000; font-weight: bold;"></span></label>
			  <span id="fk" runat="server" class="form-control">FCL</span>
		   </div>

               <div class="form-group col-md-3"> 
		   	  <label for="inputAddress">Referencia<span style="color: #FF0000; font-weight: bold;"></span></label>
			  <span class="form-control" id="referencia" runat="server">REF</span>
		   </div>

               <div class="form-group col-md-3"> 
		   	  <label for="inputAddress">IMO<span style="color: #FF0000; font-weight: bold;"></span></label>
			  <span class="form-control" id="imo" runat="server"></span>
		   </div>
		  </div>

            <div class="form-row">
		  
		   <div class="form-group col-md-3"> 
		   	  <label for="inputAddress">Nave<span style="color: #FF0000; font-weight: bold;"></span></label>
			  <span class="form-control" id="nave" runat="server"></span>
		   </div>
                 <div class="form-group col-md-3"> 
		   	  <label for="inputAddress">Viaje<span style="color: #FF0000; font-weight: bold;"></span></label>
			   <span class="form-control" id="viaje" runat="server"></span>
		   </div>
 <div class="form-group col-md-3"> 
		   	  <label for="inputAddress">Fecha de atraque [ETA]<span style="color: #FF0000; font-weight: bold;"></span></label>
			  <span id="eta" runat="server" class="form-control"> </span>
		   </div>
 <div class="form-group col-md-3"> 
		   	  <label for="inputAddress">Reefer<span style="color: #FF0000; font-weight: bold;"></span></label>
			   <span class="form-control" id="refer" runat="server">NO</span>
     
    
		   </div>
		  </div>

              <div class="form-row">
		  
		   <div class="form-group col-md-3"> 
		   	  <label for="inputAddress">Fecha límite [CutOff]<span style="color: #FF0000; font-weight: bold;"></span></label>
			  <span id="cutoff" runat="server" class="form-control">01/01/2014 10:00</span>
		   </div>
                  <div class="form-group col-md-3"> 
		   	  <label for="inputAddress">Puerto de descarga<span style="color: #FF0000; font-weight: bold;"></span></label>
			   <span class="form-control" id="pod" runat="server">BEANR</span>
		   </div>
                <div class="form-group col-md-3"> 
		   	  <label for="inputAddress">Producto declarado<span style="color: #FF0000; font-weight: bold;"></span></label>
			 <span class="form-control" id="comoditi" runat="server">Producto DUMMY</span>
		   </div>
                <div class="form-group col-md-3"> 
		   	  <label for="inputAddress">Puerto descarga final:<span style="color: #FF0000; font-weight: bold;"></span></label>
			  <span class="form-control" id="pod1" runat="server">BEANR2</span>
		   </div>    
		  </div>

             <div class="findresult" >
             <div class="booking" >
                      <input id="oversize" type="hidden" runat="server" />
                      <input id="notas" type="hidden"  runat="server"/>
                  <div class="form-title">Booking Item</div>
                 <asp:Repeater ID="tablePagination" runat="server" >
                 <HeaderTemplate>
                 <table id="tablasort" cellspacing="1" cellpadding="1" class="table table-bordered table-sm table-contecon">
                 <thead>
                 <tr>
                 <th>No.</th>
                 <th>ISO</th>
                 <th>Temp°C</th>
                 <th>Reservado</th>
                 <th>Ingresados</th>
                 <th>Disponible</th>

             
                 </tr>
                 </thead> 
                 <tbody>
                 </HeaderTemplate>
                 <ItemTemplate>
                 <tr class= '<%# getClass( Eval("dispone"),Eval("bfk")) %>' ">
                  <td><%#Eval("bitem")%></td>
                  <td><a class="tooltip" ><span class="classic"><%#Eval("iso_descrip")%></span> <%#Eval("biso")%></td>
                  <td><%#Eval("btemp")!=null?Eval("btemp").ToString().Replace(',','.'):"0.0"%></td>
                  <td><%#Eval("reserva")%></td>
                  <td><%#Eval("ingresa")%></td>
                  <td><%#Eval("dispone")%></td>
                 

                 </tr>
                  </ItemTemplate>
                 <FooterTemplate>
                 </tbody>
                 </table>
                 </FooterTemplate>
         </asp:Repeater>
             </div>
             <div class="propiedad" id="propiedad" runat="server" >Mensaje</div>
             </div>
              </div>
             <div id="sinresultado" runat="server" class=" alert alert-info" clientidmode="Static">
              </div>
       </div>
    </form>
</body>
</html>
