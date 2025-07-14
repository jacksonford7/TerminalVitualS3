<%@ Page Language="C#" Title="Lista de Conductores" AutoEventWireup="true" CodeBehind="consultaCoductorDesignado.aspx.cs" Inherits="CSLSite.conductor" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Lista de Conductores</title>
    <script src="../Scripts/pages.js" type="text/javascript"></script>
        <link href="../img/favicon2.png" rel="icon"/>
  <link href="../img/icono.png" rel="apple-touch-icon"/>
  <link href="../css/bootstrap.min.css" rel="stylesheet"/>
  <link href="../css/dashboard.css" rel="stylesheet"/>
  <link href="../css/icons.css" rel="stylesheet"/>
  <link href="../css/style.css" rel="stylesheet"/>
  <link href="https://fonts.googleapis.com/css2?family=Roboto:wght@300;400;500;700&display=swap" rel="stylesheet"/>

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
<body >
    
    <form id="bookingfrm" runat="server">
       <div class="dashboard-container p-4" id="cuerpo" runat="server">
		
           
     
		 
    
             <div id="xfinder" runat="server" visible="false" >
     

                       <div class="form-row">
		   <div class="form-group col-md-12"> 
		          <div class=" alert alert-warning" id="alerta" runat="server"> </div>
		   </div>
		  </div>
   
            
           
                 <div class="bokindetalle">
                 <asp:Repeater ID="tablePagination" runat="server" >
                 <HeaderTemplate>
                 <table id="tablasort" cellspacing="1" cellpadding="1" class="table table-bordered table-sm table-contecon">
                 <thead>
                 <tr>
                 <th>Cedula</th>
                 <th>Nombres</th>
                 <th>Apellidos</th>
                 <th>Empresa</th>
                 <th>Acciones</th>
                 </tr></thead> 
                 <tbody>
                 </HeaderTemplate>
                 <ItemTemplate>
                 <tr class="point" onclick="setObject(this);">
                  <td><%#Eval("NOMINA_CED")%></td>
                  <td><%#Eval("NOMINA_NOM")%></td>
                  <td><%#Eval("NOMINA_APE")%></td>
                  <td><%#Eval("EMPE_NOM")%></td>
                  <td>
                     <a href="#" class=" btn btn-link" >Elegir</a>
                  </td>
                 </tr>
                  </ItemTemplate>
                 <FooterTemplate>
                 </tbody>
                 </table>
                 </FooterTemplate>
         </asp:Repeater>
                </div>
         
        

              </div>
              <div id="sinresultado" runat="server" class=" alert  alert-primary" >
                  No se encontraron resultados, asegurese que ha escrito correctamente los criterios de consulta.</div>

       </div>
    
    </form>

   <script type="text/javascript" >
       function setObject(row) {
           var celColect = row.getElementsByTagName('td');
           var conductor = {
              cedula: celColect[0].textContent,
              nombres: celColect[1].textContent,
              apellidos: celColect[2].textContent
              };
            if (window.opener != null) {
                window.opener.popupCallback(conductor, '');
            }
            self.close();
      }
   </script>
   
</body>
</html>
