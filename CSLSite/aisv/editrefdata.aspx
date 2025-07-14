<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="editrefdata.aspx.cs" Inherits="CSLSite.aisv.editrefdata" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">

   <link href="../img/favicon2.png" rel="icon"/>
  <link href="../img/icono.png" rel="apple-touch-icon"/>
  <link href="../css/bootstrap.min.css" rel="stylesheet"/>
  <link href="../css/dashboard.css" rel="stylesheet"/>
  <link href="../css/icons.css" rel="stylesheet"/>
  <link href="../css/style.css" rel="stylesheet"/>
  <link href="https://fonts.googleapis.com/css2?family=Roboto:wght@300;400;500;700&display=swap" rel="stylesheet"/>
    
    

    <title>Refrigeración apoyo</title>

    <script src="../Scripts/edit_refer.js"></script>
</head>
<body>
    <form id="frx_send" runat="server">

 <div class="dashboard-container p-4" id="cuerpo" runat="server">
         <div class="form-title">Editar Refrigeracion de Apoyo</div>
		  <div class="form-row">
		  
		   <div class="form-group col-md-4"> 
		   	  <label for="inputAddress">ID<span style="color: #FF0000; font-weight: bold;"></span></label>
			 <span runat="server" id="cntr_id" class="form-control col-md-12">ABCD1234567</span>
		   </div>

                 <div class="form-group col-md-4"> 
		   	  <label for="inputAddress">Booking<span style="color: #FF0000; font-weight: bold;"></span></label>
			   <span runat="server" class="form-control col-md-12"  id="book_id">200400500600</span>
		   </div>


                 <div class="form-group col-md-4"> 
		   	  <label for="inputAddress">AISV<span style="color: #FF0000; font-weight: bold;"></span></label>
			  <span runat="server" id="aisv_id" class="form-control col-md-12">32019123456789</span>
		   </div>
		  </div>

         <div class="form-row">
		  
		   <div class="form-group col-md-4"> 
		   	  <label for="inputAddress">Horario<span style="color: #FF0000; font-weight: bold;"></span></label>
			   <span runat="server" id="hro_id" class="form-control col-md-12">2019-05-04 20:30</span>
		   </div>
              <div class="form-group col-md-4"> 
		   	  <label for="inputAddress">Usuario<span style="color: #FF0000; font-weight: bold;"></span></label>
			                     <span class="form-control col-md-12" runat="server" id="user_id">DEMOFAB</span>

		   </div>
              <div class="form-group col-md-4"> 
		   	  <label for="inputAddress">Fecha<span style="color: #FF0000; font-weight: bold;"></span></label>
			                     <span class="form-control col-md-12" runat="server" id="fecha_id">2019-04-04 20:20</span>

		   </div>
		  </div>

      <div class="form-row">
		  
		   <div class="form-group col-md-4"> 
		   	  <label for="inputAddress">RUC<span style="color: #FF0000; font-weight: bold;"></span></label>
		                    <span runat="server" class="form-control col-md-12" id="expo_id">...</span>

		   </div>

            <div class="form-group col-md-8"> 
		   	  <label for="inputAddress">EXportador<span style="color: #FF0000; font-weight: bold;"></span></label>
		                    <span runat="server" class="form-control col-md-12" id="name_id">...</span>

		   </div>
		  </div>

      <div class="form-row">
		  
		   <div class="form-group col-md-6"> 
		   	  <label for="inputAddress">Estado<span style="color: #FF0000; font-weight: bold;"></span></label>
			  <div class="d-flex">
                       <input id="ckactivo" type="checkbox" runat="server"   onchange="cambio(this,'statusdoc');" />
            <span id="statusdoc" runat="server"  >...</span> 

			  </div>
		   </div>

             <div class="form-group col-md-6"> 
		   	  <label for="inputAddress">Tipo de refrigeración<span style="color: #FF0000; font-weight: bold;"></span></label>
			  <div class="d-flex">

                          <asp:DropDownList ID="dptipos" runat="server" CssClass=" form-control"></asp:DropDownList>

			  </div>
		   </div>
		  </div>

     <div class="form-row">
         	  <label for="inputAddress">Notas<span style="color: #FF0000; font-weight: bold;"></span></label>
		   <div class="col-md-12  justify-content-center"> 
		     <div class="d-flex">

                         <textarea name="field5" id="nota" class=" form-control" runat="server" ></textarea>

		     </div>
		   </div> 
		   </div>

      <div class="form-row">
		   <div class="col-md-12 justify-content-center"> 
		     <div class="d-flex">
                         <asp:Button ID="bt_send" runat="server" Text="Guardar" 
            OnClick="bt_send_Click" CssClass="btn btn-primary" /> &nbsp;
        <input id="btcancel" type="button" value="Cancelar" class=" ml-1 btn btn-primary" onclick="closeMe();" />

		     </div>
		   </div> 
		   </div>
		 
      <div class="form-row">
		   <div class="col-md-12  justify-content-center"> 
		      <p runat="server" id="problema" class="problema"></p>
		   </div> 
		   </div>
     </div>






</form>
</body>
    
</html>
