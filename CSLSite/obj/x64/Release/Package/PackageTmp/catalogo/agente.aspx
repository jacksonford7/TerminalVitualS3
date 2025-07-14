<%@ Page Language="C#" Title="Catálogo de agentes" AutoEventWireup="true" CodeBehind="agente.aspx.cs" Inherits="CSLSite.agente" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Catálogo de agentes</title>
    
    <link href="../img/favicon2.png" rel="icon"/>
  <link href="../img/icono.png" rel="apple-touch-icon"/>
  <link href="../css/bootstrap.min.css" rel="stylesheet"/>
  <link href="../css/dashboard.css" rel="stylesheet"/>
  <link href="../css/icons.css" rel="stylesheet"/>
  <link href="../css/style.css" rel="stylesheet"/>
  <link href="https://fonts.googleapis.com/css2?family=Roboto:wght@300;400;500;700&display=swap" rel="stylesheet"/>
    
    
    
     <!--mensajes-->
        <link href="https://cdn.jsdelivr.net/npm/alertifyjs@1.11.0/build/css/alertify.min.css" rel="stylesheet"/>
        <script type="text/javascript" src="https://cdn.jsdelivr.net/npm/alertifyjs@1.11.0/build/alertify.min.js"></script>
    

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
   
       <div class="dashboard-container p-4" id="cuerpo" runat="server">

         <div class="form-title">Datos del Agente</div>
		 
		  <div class="form-row">
		      <div class="form-group col-md-5"> 
		   	  <label for="inputAddress">Código Senae (Aduana)<span style="color: #FF0000; font-weight: bold;"></span></label>
			  <div class="d-flex">
                      <asp:TextBox ID="txtci" runat="server"  CssClass="form-control" 
                 onkeypress="return soloLetras(event,'0123456789abcdefghijklmnopqrstuvwxyzñ',true)" 
                 MaxLength="15"  ></asp:TextBox> 

                  
			  </div>
		   </div>
              <div class="form-group col-md-5"> 
		   	  <label for="inputAddress">Nombres y/o apellidos<span style="color: #FF0000; font-weight: bold;"></span></label>
			             <asp:TextBox ID="txtname" runat="server"  CssClass="form-control" 
                 onkeypress="return soloLetras(event,'abcdefghijklmnñopqrstuvwxyz1234567890')" 
                 MaxLength="50"  ></asp:TextBox>  
		   </div>

               <div class="form-group col-md-2"> 
		   	  <label for="inputAddress">Persona natural<span style="color: #FF0000; font-weight: bold;"></span></label>
			   <asp:CheckBox ID="cknatural" runat="server" CssClass="form-check-inline" onclick="changeLabel(this);" /> 
		   </div>
		  </div>

             <div class="form-row">
		  
		   <div class="form-group col-md-12"> 
		   	 <p class="  alert  alert-light">Escriba una o varias letras del nombre y/o documento pulse buscar</p>
		   </div>
		  </div>

           <div class="row">
		         <div class="col-md-12 d-flex justify-content-center"> 
                     <div class="d-flex">
                         <asp:Button ID="find" 
                             CssClass="btn btn-primary" 
                             runat="server" Text="Buscar" 
                             onclick="find_Click"  
                             OnClientClick="return initFinder();"/>
                          <span id="imagen"></span>
                     </div>
                 
		       </div> 
		   </div>

            <div class="cataresult" >
             <asp:UpdatePanel ID="upresult" runat="server"  >
             <ContentTemplate>
             <script type="text/javascript">Sys.Application.add_load(BindFunctions); </script>
             <div id="xfinder" runat="server" visible="false" >
             <div class=" alert alert-warning" id="alerta" >
               Confirme que los datos sean correctos. En caso de error, favor comuníquese 
               con el Departamento de servicio al cliente a los teléfonos: +593 (04) 6006300, 3901700 ext. 4040, 
               en sus horarios hábiles Lunes a Viernes de 8:30 a 19:30
             </div>
             <%-- catalogo de bookings--%>
            
                  <div class=" form-title">Agentes / Personas naturales</div>
            
                 <asp:Repeater ID="tablePagination" runat="server" >
                 <HeaderTemplate>
                 <table id="tablasort" cellspacing="1" cellpadding="1" class="table table-bordered table-sm table-contecon">
                 <thead>
                 <tr>
                 <th>No.</th>
                 <th>Documento No.</th>
                 <th>Descripción</th>
                 <th>Acciones</th>
                 </tr>
                 </thead> 
                 <tbody>
                 </HeaderTemplate>
                 <ItemTemplate>
                 <tr class="point" onclick="setObject(this);">
                  <td><%#Eval("number")%></td>
                  <td><%#Eval("codigo")%></td>
                  <td><%#Eval("descripcion")%></td>
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
              <div id="sinresultado" runat="server" class=" alert  alert-primary" >
              No se encontraron resultados, 
              asegurese que ha escrito correctamente el número/nombre
              buscado  
              </div>
             </ContentTemplate>
             <Triggers>
             <asp:AsyncPostBackTrigger ControlID="find" />
             </Triggers>
             </asp:UpdatePanel>
       </div>
		 
     </div>   
        
        
        
        
        

    </form>

   <script type="text/javascript" >
       function setObject(row) {
           var celColect = row.getElementsByTagName('td');
          var agente = {
              item: celColect[0].textContent,
              codigo: celColect[1].textContent,
              descripcion: celColect[2].textContent
              };
            if (window.opener != null) {
                   window.opener.document.getElementById('valexpor').innerHTML = '';
                   window.opener.document.getElementById('agente').textContent = agente.descripcion;
                   window.opener.popupCallback(agente.codigo, 'caeid');
            }
            self.close();
      }

      function initFinder() {
          if (document.getElementById('txtci').value.trim().length <= 0 && document.getElementById('txtname').value.trim().length <= 0) {
           alertify.alert('Por favor escriba una o varias letras del nombre o documento');
              return false;
          }
          document.getElementById('imagen').innerHTML = '<img alt="" src="../shared/imgs/loader.gif">';
      }

      function changeLabel(control) {
          if (control.checked) {
              document.getElementById('senae').textContent = 'Número CI | Pasaporte:';
               return;
          }
           document.getElementById('senae').textContent = 'Código Senae (Aduana):';
          return;
      }
   </script>
    <script src="../Scripts/pages.js" type="text/javascript"></script>
</body>
</html>
