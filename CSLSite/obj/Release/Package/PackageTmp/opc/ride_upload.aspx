<%@ Page  Title="Subir RIDE"  Language="C#" MasterPageFile="~/site.Master" 
    AutoEventWireup="true" CodeBehind="ride_upload.aspx.cs" Inherits="CSLSite.ride_upload" %>
<%@ MasterType VirtualPath="~/site.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="placehead" runat="server">
 

        <link href="../shared/estilo/tables.css" rel="stylesheet" type="text/css" />
         <!--mensajes-->
        <link href="https://cdn.jsdelivr.net/npm/alertifyjs@1.11.0/build/css/alertify.min.css" rel="stylesheet"/>
        <script type="text/javascript" src="https://cdn.jsdelivr.net/npm/alertifyjs@1.11.0/build/alertify.min.js"></script>
 </asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="placebody" runat="server">

    <input id="zonaid" type="hidden" value="1188" />

    <input id="identificacion" type="hidden" value="" runat="server" clientidmode="Static" />
     <input id="nombres" type="hidden" value="" runat="server" clientidmode="Static" />
    <input id="apellidos" type="hidden" value="" runat="server" clientidmode="Static" />
           <div class="mt-4">         
        <nav class="mt-4" aria-label="breadcrumb">
          <ol class="breadcrumb">
            <li class="breadcrumb-item" id="opcion_principal" runat="server"><a href="#">CPF</a></li>
             <li class="breadcrumb-item active" aria-current="page" id="sub_opcion" runat="server">Subir factura electrónica (RIDE)</li>
          </ol>
        </nav>
      </div>

       <div class="dashboard-container p-4" id="cuerpo" runat="server">
		  <div class="form-row">
		  
		   <div class="form-group col-md-12"> 
		   	   <div class=" alert alert-warning">
    <span id="dtlo" runat="server">Estimado usuario:</span> 
    <br /> Por favor verifique los datos antes de guardarlos, este proceso es muy importante</div>
		   </div>
		  </div>
           
           <div class="form-title">Datos de la proforma</div>
		           <div class="form-row">
		  
				   <div class="col-md-12 "> 
   <span  class="form-control col-md-12" id="pro_referencia" runat="server">REFERENCIA: ... </span> 
		   </div> 
		   </div>
        

              <div class="form-row">
                     <div class="form-group col-md-3"> 
		   	  <label for="inputAddress">Número<span style="color: #FF0000; font-weight: bold;"></span></label>
		           <span  runat="server" id="pro_id" class=" form-control col-md-12 ">...</span>

		   </div>

                               <div class="form-group col-md-3"> 
		   	  <label for="inputAddress">Fecha emisión<span style="color: #FF0000; font-weight: bold;"></span></label>
		           <span runat="server" id="pro_fecha" class="form-control col-md-12 ">...</span>

		   </div>

                  		   <div class="form-group col-md-3"> 
		   	  <label for="inputAddress">Subtotal<span style="color: #FF0000; font-weight: bold;"></span></label>
		           <span  runat="server" id="pro_subt" class="form-control col-md-12 ">...</span>

		   </div>

                <div class="form-group col-md-3"> 
		   	  <label for="inputAddress">Total<span style="color: #FF0000; font-weight: bold;"></span></label>
		           <span runat="server" id="pro_total" class="form-control col-md-12 ">...</span>

		   </div>



           


   

		  </div>
          <div class="form-row">
		  
		   <div class="form-group col-md-6"> 
		   	  <label for="inputAddress">Ruc<span style="color: #FF0000; font-weight: bold;"></span></label>
		           <span  runat="server" id="pro_ruc" class=" form-control col-md-12 ">...</span>

		   </div>

                <div class="form-group col-md-6"> 
		   	  <label for="inputAddress">Razón social<span style="color: #FF0000; font-weight: bold;"></span></label>
		           <span runat="server" id="pro_razon" class="form-control col-md-12 ">...</span>
		   </div>

		  </div>
 
           <div class="form-title">Datos de la factura (RIDE)</div>

            <div class="form-row">
		  
		   <div class="form-group col-md-12"> 
		   	  <label for="inputAddress">Archivo<span style="color: #FF0000; font-weight: bold;"></span></label>
			  <div class="d-flex">
                   <asp:FileUpload ID="ffile" runat="server" CssClass=" fa-upload form-control" />
       <asp:Button ID="btsubir" runat="server" CssClass=" btn btn-secondary" Text="Subir archivo" OnClick="btsubir_Click" />

			  </div>
		   </div>
		  </div>

            <div class="form-row">
		  
		   <div class="form-group col-md-3"> 
		   	  <label for="inputAddress">Número<span style="color: #FF0000; font-weight: bold;"></span></label>
		              <span  runat="server" id="fac_num" class=" form-control col-md-12  " >...</span>
           </div>

                   <div class="form-group col-md-3"> 
		   	  <label for="inputAddress">Fecha emisión<span style="color: #FF0000; font-weight: bold;"></span></label>
		              <span runat="server" id="fac_fecha" class=" form-control col-md-12  ">...</span>

                   </div>

                	   <div class="form-group col-md-3"> 
		   	  <label for="inputAddress">Subtotal<span style="color: #FF0000; font-weight: bold;"></span></label>
		             <span  runat="server" id="fac_subtotal" class=" form-control col-md-12  ">...</span>

               </div>

                   <div class="form-group col-md-3"> 
		   	  <label for="inputAddress">Total<span style="color: #FF0000; font-weight: bold;"></span></label>
		             <span runat="server" id="fac_total" class=" form-control col-md-12 ">...</span>

                       </div>
		  </div>

                <div class="form-row">
		  
		   <div class="form-group col-md-6"> 
		   	  <label for="inputAddress">Ruc<span style="color: #FF0000; font-weight: bold;"></span></label>
		             <span  runat="server" id="fac_ruc" class=" form-control col-md-12  ">...</span>

               </div>

                   <div class="form-group col-md-6"> 
		   	  <label for="inputAddress">Razón social<span style="color: #FF0000; font-weight: bold;"></span></label>
		             <span runat="server" id="fac_razon" class=" form-control col-md-12  ">...</span>

                       </div>
		  </div>

                <div class="form-row">
		  
	
		  </div>

           	   <div class="form-row">
		   <div class="col-md-12 d-flex justify-content-center"> 
                             <asp:Button CssClass="btn btn-primary" OnClientClick="return confirm('Está seguro de guardar los datos de la factura, este proceso es irreversible?');"   ID="btsalvar" runat="server" Text="Guardar Información" OnClick="btsalvar_Click" />

		   </div> 
		   </div>


     </div>


   
     	  

    <script type="text/javascript">
        function check_info() {
            var str = this.document.getElementById("identificacion").value;
            if (!str || 0 === str.length) {
              alertify.alert("Por favor selecione el operario de la lista");
                //cancel postback
                return false;
            }
            return true;
        }

    

    </script>

</asp:Content>