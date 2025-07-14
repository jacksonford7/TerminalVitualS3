<%@ Page Language="C#" Title="Lista de Placas" AutoEventWireup="true" CodeBehind="consultaPlacas.aspx.cs" Inherits="CSLSite.placas" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Lista de Placas</title>
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



    <script src="../Scripts/pages.js" type="text/javascript"></script>
</head>
<body >
    <form id="bookingfrm" runat="server">
          <div class="dashboard-container p-4" id="cuerpo" runat="server">

                 <div class="form-row">
		   <div class="col-md-12"> 
                   <div class="catabody">
      
             <div id="xfinder" runat="server" visible="false" >
             <div class=" alert  alert-warning" id="alerta" runat="server">
                 
             </div>
    
  
                
                
                 <asp:Repeater ID="tablePagination" runat="server" >
                 <HeaderTemplate>
                 <table id="tablasort" cellspacing="1" cellpadding="1" class="table table-bordered table-sm table-contecon">
                 <thead>
                 <tr>
                 <th>Placa / No. Serie</th>
                 <th>Clase / Tipo</th>
                 <th>Marca</th>
                 <th>Modelo</th>
                 <th>Color</th>
                 <th style=" display:none">TipoCertificado</th>
                 <th style=" display:none">NumCertificado</th>
                 <th style=" display:none">Categoria</th>
                 <th style=" display:none">FechaPoliza</th>
                 <th style=" display:none">FechaMTOP</th>
                 <th style=" display:none">TipoCategoria</th>
                 <th>Empresa</th>
                 <th>Acciones</th>
                 </tr></thead> 
                 <tbody>
                 </HeaderTemplate>
                 <ItemTemplate>
                 <tr class="point" onclick="setObject(this);">
                  <td><%#Eval("VE_PLACA")%></td>
                  <td><%#Eval("VE_TIPO")%></td>
                  <td><%#Eval("VE_MARCA")%></td>
                  <td><%#Eval("VE_MODELO")%></td>
                  <td><%#Eval("VE_COLOR")%></td>
                  <td style=" display:none"><%#Eval("VE_PMTIPO")%></td>
                  <td style=" display:none"><%#Eval("VE_PMCERTIFICADO")%></td>
                  <td style=" display:none"><%#Eval("DESCRIPCIONCATEGORIA")%></td>
                  <td style=" display:none"><%#Eval("VE_POLIZA")%></td>
                  <td style=" display:none"><%#Eval("VE_MTOP")%></td>
                  <td style=" display:none"><%#Eval("CODCATEGORIA")%></td>
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
          
         
       
              <div id="sinresultado" runat="server" class=" alert  alert-primary " >
                  No se encontraron resultados, asegurese que ha escrito correctamente la Placa / 
                  No. Serie (para Montacargas).</div>


      </div>
		 
     </div>

		   </div> 
		   </div>




</div>
    </form>

   <script type="text/javascript" >
       function setObject(row) {
           var celColect = row.getElementsByTagName('td');
           var vehiculos = {
              placa: celColect[0].textContent,
              clasetipo: celColect[1].textContent,
              marca: celColect[2].textContent,
              modelo: celColect[3].textContent,
              color: celColect[4].textContent,
              tipocertificado: celColect[5].textContent,
              certificado: celColect[6].textContent,
              categoria: celColect[7].textContent,
              fechapoliza: celColect[8].textContent,
              fechamtop: celColect[9].textContent,
              tipocategoria: celColect[10].textContent
              };
            if (window.opener != null) {
                window.opener.popupCallback(vehiculos, '');
            }
            self.close();
      }
   </script>
   
</body>
</html>
