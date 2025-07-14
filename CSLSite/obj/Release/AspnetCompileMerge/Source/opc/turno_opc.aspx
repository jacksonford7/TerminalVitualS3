<%@ Page  Title="Seleccion de grúa" MaintainScrollPositionOnPostback="true"  EnableEventValidation="true" Language="C#" MasterPageFile="~/site.Master" AutoEventWireup="true" CodeBehind="turno_opc.aspx.cs" Inherits="CSLSite.turno_opc" %>
<%@ MasterType VirtualPath="~/site.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="placehead" runat="server">

 
        
    <link href="../shared/estilo/opc_tables.css" rel="stylesheet" />
 </asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="placebody" runat="server">
   

     <div class="mt-4">         
        <nav class="mt-4" aria-label="breadcrumb">
          <ol class="breadcrumb">
            <li class="breadcrumb-item" id="opcion_principal" runat="server"><a href="#">CPF</a></li>
             <li class="breadcrumb-item active" aria-current="page" id="sub_opcion" runat="server">Selección de Turno para OPC</li>
          </ol>
        </nav>
      </div>

       <div class="dashboard-container p-4" id="cuerpo" runat="server">

           <div class="form-row">
		   <div class="form-group col-md-12"> 
		        <div class=" alert alert-warning">
    <span id="dtlo" runat="server">Estimado usuario:</span> 
    <br /> Asegurese de seleccionar el turno apropiado antes de realizar la asignación, recuerde elegir siempre la OPC correcta en la lista.
    </div>
		   </div>
		  </div>
           
           <div class="form-title">Datos de la nave</div>
		 
		  <div class="form-row">
		  
		   <div class="form-group col-md-6"> 
		   	  <label for="inputAddress">Referencia<span style="color: #FF0000; font-weight: bold;"></span></label>
           <span runat="server" id="referencia" class="form-control col-6">... </span>
		   </div>

                 <div class="form-group col-md-6"> 
		   	  <label for="inputAddress">Nombre<span style="color: #FF0000; font-weight: bold;"></span></label>
           <span  runat="server" id="buque" class=" form-control col-6">...</span>
		   </div>

     


		  </div>

           		  <div class="form-row">
		              <div class="form-group col-md-6"> 
		   	  <label for="inputAddress">Viaje<span style="color: #FF0000; font-weight: bold;"></span></label>
           <span runat="server"  id="vio" class="form-control col-md-12">...</span>
		   </div>
		

                 <div class="form-group col-md-6"> 
		   	  <label for="inputAddress">Muelle:<span style="color: #FF0000; font-weight: bold;"></span></label>
           <span runat="server" id="muelle" class="form-control col-md-12">...</span>
		   </div>
		  </div>

           		  <div class="form-row">
		  
		   <div class="form-group col-md-3"> 
		   	  <label for="inputAddress">ETA:<span style="color: #FF0000; font-weight: bold;"></span></label>
           <span runat="server" id="eta" class="form-control col-md-12">...</span>
		   </div>

                 <div class="form-group col-md-3"> 
		   	  <label for="inputAddress">ETD:<span style="color: #FF0000; font-weight: bold;"></span></label>
           <span runat="server" id="etd" class="form-control col-md-12">...</span>
		   </div>
	
		  
		   <div class="form-group col-md-3"> 
		   	  <label for="inputAddress">ATA<span style="color: #FF0000; font-weight: bold;"></span></label>
           <span runat="server" id="ata" class="form-control col-md-12">...</span>
		   </div>

                  <div class="form-group col-md-3"> 
		   	  <label for="inputAddress">ATD<span style="color: #FF0000; font-weight: bold;"></span></label>
           <span runat="server" id="atd" class="form-control col-md-12">...</span>
		   </div>
		  </div>

              <div class="form-row">
		  
		   <div class="form-group col-md-4"> 
		   	  <label for="inputAddress">S.OP<span style="color: #FF0000; font-weight: bold;"></span></label>
           <span runat="server" id="wini" class="form-control col-md-12">...</span>
		   </div>

                  <div class="form-group col-md-4"> 
		   	  <label for="inputAddress">E.OP<span style="color: #FF0000; font-weight: bold;"></span></label>
           <span runat="server" id="wend" class="form-control col-md-12">...</span>
		   </div>
		
		  
		   <div class="form-group col-md-4"> 
		   	  <label for="inputAddress">Fecha Cita<span style="color: #FF0000; font-weight: bold;"></span></label>
           <span runat="server" id="fcita" class="form-control col-md-12">...</span>
		   </div>
		  </div>
		  <div class="form-title">Detalles de máquinas</div>
                 <div class="bokindetalle" >
          <div id="sin_gruas" runat="server" class=" alert-info">Este documento no tiene grúas asignadas</div>
       <asp:Repeater ID="rp_grua" runat="server" >
                 <HeaderTemplate>
                 <table id="tbgru" cellspacing="1" cellpadding="1" class="table table-bordered invoice">
                 <thead>
                 <tr>
                 <th >Nombre</th>
                 <th >Trabajo (Hrs)</th>
                 <th >Inicio Trabajos</th>
                 <th >Turnos OPC</th>
                 </tr>
                 </thead> 
                 <tbody>
                 </HeaderTemplate>
                 <ItemTemplate>
                 <tr class="point" >
                  <td ><%#Eval("Crane_name")%></td>
                  <td ><%#Eval("Crane_time_qty")%></td>
                  <td ><%#Eval("DateWork")%></td>
                  <td ><a class="btn btn-link" href="javascript:void popOpen('asignar_op.aspx?sid=<%# securetext(Eval("id")) %>')">Turnos OPC</a></td>
                 </tr>
                  </ItemTemplate>
                 <FooterTemplate>
                 </tbody>
                 </table>
                 </FooterTemplate>
         </asp:Repeater>
         </div>

     </div>

    <script type="text/javascript">
        function popOpen(URL) {
            window.open(URL, '', 'width=1000, height=1200, top=0, left=100, scrollbar=no, resize=no, menus=no');
       
        }
    </script>
</asp:Content>