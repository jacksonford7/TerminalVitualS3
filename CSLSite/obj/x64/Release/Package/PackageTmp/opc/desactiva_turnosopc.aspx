<%@ Page  Title="Finalizar grúa"  Language="C#" MasterPageFile="~/site.Master" AutoEventWireup="true" CodeBehind="desactiva_turnosopc.aspx.cs" Inherits="CSLSite.desactiva_turnosopc" MaintainScrollPositionOnPostback="True" %>
<%@ MasterType VirtualPath="~/site.Master" %>


<asp:Content ID="Content1" ContentPlaceHolderID="placehead" runat="server">
    
 
     <link href="../shared/estilo/tables.css" rel="stylesheet" type="text/css" />
     <link href="../shared/estilo/catalogos.css" rel="stylesheet" type="text/css" />
    	   <link href="../shared/estilo/jquery.datetimepicker.css" rel="stylesheet" type="text/css" />

 </asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="placebody" runat="server" >

    <div class="mt-4">         
        <nav class="mt-4" aria-label="breadcrumb">
          <ol class="breadcrumb">
            <li class="breadcrumb-item" id="opcion_principal" runat="server"><a href="#">CPF</a></li>
             <li class="breadcrumb-item active" aria-current="page" id="sub_opcion" runat="server">Transacción Para Finalizar Turnos De Una Grúa en Estado (Abierto)</li>
          </ol>
        </nav>
      </div>

      <div class="dashboard-container p-4" id="cuerpo" runat="server">

         <div class="form-title">Datos del documento buscado</div>
		 
		  <div class="form-row">
               <div class="form-group col-md-12"> 
                   <div class=" alert alert-warning">
    <span id="dtlo" runat="server"  >Estimado usuario:</span> 
   
    </div>
                   </div>
		  
		  </div>

          <div class="form-row">
		  
		   <div class="form-group col-md-12"> 
		   	  <label for="inputAddress">Referencia:<span style="color: #FF0000; font-weight: bold;"></span></label>
			  <div class="d-flex">
                       <asp:TextBox ID="TxtReferencia" runat="server" CssClass=" form-control" MaxLength="10" 
          onkeypress="return soloLetras(event,'abcdefghijklmnopqrstuvwxyz1234567890',true)" 
            
            placeholder="Nave"></asp:TextBox>
             <span class="validacion" id="xplinea" > * </span>
         <asp:Button ID="BtnBuscar" runat="server"   CssClass="btn btn-primary"
             OnClick="BtnBuscar_Click" Text="Consultar" />

			  </div>
		   </div>
		  </div>


          <div class="form-row">
		  
		   <div class="form-group col-md-12"> 
		   	  <label for="inputAddress">Nombre<span style="color: #FF0000; font-weight: bold;"></span></label>
			  <div class="d-flex">
                  <asp:Label ID="LblNombre" runat="server" Text="LblNombre" CssClass="form-control col-md-12"  ></asp:Label>
                 <asp:Label ID="LblVoyageIn" runat="server" Text="LblVoyageIn"  Visible="false" CssClass="form-control col-md-4" ></asp:Label>
                 <asp:Label ID="LblVoyageOut" runat="server" Text="LblVoyageOut"  Visible="false" CssClass="form-control col-md-4" ></asp:Label>
			  </div>
		   </div>
		  </div>

          <div class="form-row">
		  
		   <div class="form-group col-md-6"> 
		   	  <label for="inputAddress">ETA<span style="color: #FF0000; font-weight: bold;"></span></label>
                 <asp:Label ID="LblETA" runat="server" Text="LblETA" CssClass="form-control col-md-12"  ></asp:Label>
		   </div>

                 <div class="form-group col-md-6"> 
		   	  <label for="inputAddress">ETD<span style="color: #FF0000; font-weight: bold;"></span></label>
		                    <asp:Label ID="LblETD" runat="server" Text="LblETD" CssClass="form-control col-md-12"></asp:Label>

                 </div>
		  </div>


          <div class="form-row">
		  
		   <div class="form-group col-md-6"> 
		   	  <label for="inputAddress">Viaje<span style="color: #FF0000; font-weight: bold;"></span></label>
			        <asp:Label ID="LblViaje" runat="server" Text="LblViaje" CssClass="form-control col-md-12"></asp:Label>
		   </div>

                 <div class="form-group col-md-6"> 
		   	  <label for="inputAddress">Trabajo (HR)<span style="color: #FF0000; font-weight: bold;"></span></label>
			  <asp:Label ID="LblHoras" runat="server" Text="LblHoras" CssClass="form-control col-md-12"></asp:Label>
		   </div>
		  </div>

           <div class="form-title">Datos del turno a finalizar/cancelar</div>
	
              <div class="bokindetalle">

          <asp:Repeater ID="TableTurnos" runat="server"  onitemcommand="RemoverTurno_ItemCommand" OnItemDataBound="Opciones_ItemDataBound">
                <HeaderTemplate>
                <table id="tablaTurno" cellspacing="0" cellpadding="1" class="table table-bordered invoice">
                <thead>
                <tr>
                    <th style='width:50px' class ="nover">ID TURNO</th>
                    <th style='width:50px'>O/T #</th>
                    <th style='width:100px'>GRUA</th>
                    <th style='width:50px'>TURNO</th>
                    <th style='width:50px' class ="nover">IDGRUDA</th>
                    <th style='width:80px'>HORA INICIO</th>
                    <th style='width:80px'>HORA FIN</th>
                    <th style='width:150px'>OPC</th>
                    <th style='width:50px'>ESTADO</th>
                    
                  </tr>
                </thead> 
                <tbody>
                </HeaderTemplate>
                <ItemTemplate>
                <tr class="point">
                <td class ="nover"><asp:Label Text='<%#Eval("id")%>' ID="lblidTurno" runat="server" Visible="false"/></td>
                <td ><asp:Label Text='<%#Eval("vessel_visit_id")%>' ID="LblIdOT" runat="server" Visible="true"/></td>
                <td >
                    <asp:LinkButton CssClass="btn btn-link" CommandName="RefButton" CommandArgument='<%#Eval("id") %>' ID="MostrarDetalle" runat="server" Text='<%#Eval("crane_name") %>'
                       ></asp:LinkButton>
                </td>
                     
                <td ><asp:Label Text='<%#Eval("turno_number")%>' ID="lblturno_nomber" runat="server" Visible="true"/></td>
                <td class ="nover"><asp:Label Text='<%#Eval("crane_id")%>' ID="lblcrane_id" runat="server" Visible="false" /></td>
                <td ><asp:Label Text='<%#Eval("turn_time_start")%>' ID="LblStart" runat="server" Visible="true"/></td>
                <td ><asp:Label Text='<%#Eval("turn_time_end")%>' ID="Lblend" runat="server" Visible="true"/></td>
                <td ><asp:Label Text='<%#Eval("opc_name")%>' ID="Lblopcname" runat="server" Visible="true"/></td>
                <td id="status"><asp:Label Text='<%#Eval("status")%>' ID="LblStatus" runat="server" Visible="true"/></td>               
               </tr>
                </ItemTemplate>
                <FooterTemplate>
                </tbody>
                </table>
                </FooterTemplate>
        </asp:Repeater>
                   </div>
      <div class="form-title">Datos para cancelación/finalización</div>
<div class="form-row">
		  
		   <div class="form-group col-md-6"> 
		   	  <label for="inputAddress">Grúa/Turno<span style="color: #FF0000; font-weight: bold;"></span></label>
			  <div class="d-flex">
                       <asp:DropDownList ID="CboGruas" runat="server" AutoPostBack="False"
                                             DataTextField='crane_name' DataValueField='Id'  
                                             CssClass="form-control"
                                            onselectedindexchanged="CboGruas_SelectedIndexChanged">
                                        </asp:DropDownList>
                                <span class="validacion" id="valcbogrua"  > * </span>

			  </div>
		   </div>


		 
		  
		   <div class="form-group col-md-6"> 
		   	  <label for="inputAddress">Fecha Finalización del Trabajo:<span style="color: #FF0000; font-weight: bold;"></span></label>
			  <div class="d-flex">
                             <asp:TextBox ID="TxtFechaDesde" runat="server" MaxLength="16" 
                                 CssClass="datetimepicker form-control"
               onkeypress="return soloLetras(event,'1234567890/:')"
               onBlur="valDate(this,  true,valdatem);"
               placeholder="dd/mm/aaaa hh:mm"
                ClientIDMode="Static"
               ></asp:TextBox>
             <span class="validacion" id="valFechaDesde"  > *</span>


			  </div>
		   </div>
		  </div>

          	   <div class="form-row">
		   <div class="col-md-12 d-flex justify-content-center"> 
                        <asp:Button ID="BtnAgregar"  CssClass="btn  btn-primary mr-2"
              runat="server"  OnClick="BtnAgregar_Click" Text="Procesar" 
               OnClientClick="return confirm('Esta seguro de que desea desactivar el turno?');" 
              />
	
                     
       
          <asp:Button ID="BtnNuevo"  CssClass="btn  btn-secondary"
              runat="server"  OnClick="BtnNuevo_Click" Text="Nuevo" /> 
                 <img alt="loading.." src="../shared/imgs/loader.gif" id="loader" class="nover"  />
		   </div> 
		   </div>

     </div>



    <script src="../Scripts/pages.js" type="text/javascript"></script>
   <script src="../Scripts/opc_control.js" type="text/javascript"></script>
     <script src="../Scripts/jquery.datetimepicker.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            //inicia los fecha-hora
            $('.datetimepicker').datetimepicker().datetimepicker({ lang: 'es', step: 30, format: 'd/m/Y H:i' });
            //inicia los fecha
            $('.datetimepickerAlt').datetimepicker().datetimepicker({ lang: 'es', step: 30, format: 'd/m/Y' });

            $('.datepicker').datetimepicker().datetimepicker({ lang: 'es', timepicker: false, format: 'd/m/Y', closeOnDateSelect: true, minDate: '0' });
            //init reefer-> lo pone a false.

        });
   
        function defaultDate(control) {
            if (control.value) {
                var cj = document.getElementById("TxtFechaDesde");
                if (cj != null && cj != undefined) {
                    cj.value = control.value;
                }
            }
        }
    </script>

</asp:Content>