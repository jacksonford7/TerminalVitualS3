<%@ Page  Title="OPC Asignar turno"  Language="C#" MasterPageFile="~/site.Master" 
    AutoEventWireup="true" CodeBehind="asignar_op.aspx.cs" Inherits="CSLSite.asignar_op" %>
<%@ MasterType VirtualPath="~/site.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="placehead" runat="server">
   
    <link href="../shared/estilo/catalogos.css" rel="stylesheet" />
     <link href="../shared/estilo/tables.css" rel="stylesheet" type="text/css" />
    	   <link href="../shared/estilo/jquery.datetimepicker.css" rel="stylesheet" type="text/css" />
 </asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="placebody" runat="server">

    <input id="zonaid" type="hidden" value="1188" />

    <input id="identificacion" type="hidden" value="" runat="server" clientidmode="Static" />
     <input id="nombres" type="hidden" value="" runat="server" clientidmode="Static" />
    <input id="apellidos" type="hidden" value="" runat="server" clientidmode="Static" />
      <div class="mt-4">         
        <nav class="mt-4" aria-label="breadcrumb">
          <ol class="breadcrumb">
            <li class="breadcrumb-item" id="opcion_principal" runat="server"><a href="#">PCF</a></li>
             <li class="breadcrumb-item active" aria-current="page" id="sub_opcion" runat="server">Selección de operarios para Turno</li>
          </ol>
        </nav>
      </div>
        <div class="dashboard-container p-4" id="cuerpo" runat="server">
          <div class="form-row">
		  
		   <div class="form-group col-md-12"> 
		   	               <div class="   alert alert-warning">
    <span id="dtlo" runat="server">Estimado usuario:</span> 
    <br /> Por favor asegúrese de selecionar a las personas correctas de la lista anres de guardar la cuadrilla
    </div>
		   </div>
		  </div>
		 <div class="form-title">Datos de la Nave</div>
		  <div class="form-row">
		        <div class="form-group col-md-6"> 
		   	  <label for="inputAddress">Referencia<span style="color: #FF0000; font-weight: bold;"></span></label>
			           <span runat="server" id="idreferencia" class=" form-control col-md-12 ">... </span>
		   </div>

                 <div class="form-group col-md-6"> 
		   	  <label for="inputAddress">Nombre de la nave<span style="color: #FF0000; font-weight: bold;"></span></label>
			           <span  runat="server" id="buque" class="form-control col-md-12">...</span>

		   </div>
  
		  </div>
          <div class="form-row">
		                <div class="form-group col-md-6"> 
		   	  <label for="inputAddress">Viaje IN/OUT<span style="color: #FF0000; font-weight: bold;"></span></label>
			           <span runat="server"  id="vio" class="form-control col-md-12 ">...</span>

		   </div>
		
                <div class="form-group col-md-6"> 
		   	  <label for="inputAddress">Muelle<span style="color: #FF0000; font-weight: bold;"></span></label>
			           <span runat="server" id="muelle" class="form-control col-md-12 ">...</span>
		   </div>
		  </div>
          <div class="form-row">
		  
		   <div class="form-group col-md-3"> 
		   	  <label for="inputAddress">ETA<span style="color: #FF0000; font-weight: bold;"></span></label>
			           <span runat="server" id="eta" class="form-control col-md-12 ">...</span>

		   </div>
                <div class="form-group col-md-3"> 
		   	  <label for="inputAddress">ETD<span style="color: #FF0000; font-weight: bold;"></span></label>
           <span runat="server" id="etd" class="form-control col-md-12 ">...</span>
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
			           <span runat="server" id="wini" class="form-control col-md-12 ">...</span>

		   </div>
                <div class="form-group col-md-4"> 
		   	  <label for="inputAddress">E.OP<span style="color: #FF0000; font-weight: bold;"></span></label>
           <span runat="server" id="wend" class="form-control col-md-12">...</span>
		   </div>

               <div class="form-group col-md-4"> 
                                 <label for="inputAddress">Fecha Hora de Citación<span style="color: #FF0000; font-weight: bold;"></span></label>
		
                          <span runat="server" id="fcita" class="form-control c col-md-12 ">...</span>
		 

                   </div>
		  </div>
  
          <div class="form-title">Datos de la Grúa</div>
          <div class="form-row">
		  
		   <div class="form-group col-md-6"> 
		   	  <label for="inputAddress">Operadora Actual<span style="color: #FF0000; font-weight: bold;"></span></label>
			           <span runat="server" id="operator_no" class="form-control col-md-12 ">... </span>

		   </div>
                <div class="form-group col-md-6"> 
		   	  <label for="inputAddress">Máquina<span style="color: #FF0000; font-weight: bold;"></span></label>
			           <span  runat="server" id="grua" class="form-control  cl-md-12 ">...</span>

		   </div>
		  </div>
          <div class="form-title">Selección de Turnos</div>
   		  <div class="form-row">
		  
		   <div class="form-group col-md-6"> 
		   	  <label for="inputAddress">Operadora<span style="color: #FF0000; font-weight: bold;"></span></label>
			                 <asp:DropDownList ID="dpopc" runat="server"  CssClass="form-control" ClientIDMode="Static" AutoPostBack="True" OnSelectedIndexChanged="dpopc_SelectedIndexChanged"></asp:DropDownList>

		   </div>
                <div class="form-group col-md-6"> 
		   	  <label for="inputAddress">Grupo<span style="color: #FF0000; font-weight: bold;"></span></label>
			           <asp:DropDownList ID="CboGrupos" CssClass="form-control" runat="server"  ClientIDMode="Static"></asp:DropDownList>

		   </div>
		  </div>
   		  <div class="form-row">
           <div class="form-group col-md-6"> 
		   	  <label for="inputAddress">Desde<span style="color: #FF0000; font-weight: bold;"></span></label>
			<div class="d-flex">
                              <asp:TextBox ID="TxtFechaDesde" runat="server" MaxLength="16" 
                  CssClass="datetimepicker form-control"
               onkeypress="return soloLetras(event,'1234567890/:')"
               onBlur="valDate(this,  true,valdatem);"
               placeholder="dd/mm/aaaa hh:mm"
                ClientIDMode="Static" 
               ></asp:TextBox>
             <span class="validacion" id="valFechaDesde"  > * </span>
                <asp:TextBox ID="TxtIdGrua" runat="server" 
                    CssClass=" form-control" MaxLength="10"  ClientIDMode="Static"  Visible="false"></asp:TextBox>


			</div>
		   </div>
            <div class="form-group col-md-6"> 
		   	  <label for="inputAddress">Hasta<span style="color: #FF0000; font-weight: bold;"></span></label>
			<div class=" d-flex">
                  <asp:TextBox ID="TxtFechaHasta" 
                  runat="server" MaxLength="16"  CssClass="datetimepicker form-control"
               onkeypress="return soloLetras(event,'1234567890/:')"
               onBlur="valDate(this,  true,valdatem);"
               placeholder="dd/mm/aaaa hh:mm"
                ClientIDMode="Static" 
               ></asp:TextBox>
             <span class="validacion" id="valFechaHasta"  > * </span>

			</div>
		   </div>
		  </div>
   		  <div class="form-row">
		  
		   <div class="form-group col-md-6"> 
		   	  <label for="inputAddress">Trabajo Realizado<span style="color: #FF0000; font-weight: bold;"></span></label>
			<div class="d-flex">
                  <strong>Amarre</strong>
                  <asp:CheckBox ID="vlock" runat="server"  CssClass="form-check"/> 

                <strong>Desamarre</strong>
                  <asp:CheckBox ID="vunlock" runat="server" CssClass="form-check" />
			</div>
		   </div>
 
		  </div>

               		  <div class="form-row">
		  
		   <div class="form-group col-md-6"> 
		   	  <label for="inputAddress">Acciones:<span style="color: #FF0000; font-weight: bold;"></span></label>
			<div class="d-flex">
                    <asp:Button  CssClass="btn btn-primary" ID="btAdd"
               runat="server" Text="Agregar Turno" OnClick="btAdd_Click" /> &nbsp;
                                    <asp:Button  CssClass=" btn btn-primary" ID="BtnDetele" runat="server" Text="Quitar Turno" OnClick="btnDelete_Click"    OnClientClick="return confirm('Esta seguro de que desea desactivar el turno?');" />

			</div>
		   </div>

		  </div>
<div class="accion" id="Turno">
            <div class="bokindetalle">
                   <asp:Repeater ID="TableTurnos" runat="server"  onitemcommand="RemoverTurno_ItemCommand">
                <HeaderTemplate>
                <table id="tablaTurno" cellspacing="0" cellpadding="1" class="table table-bordered invoice">
                <thead>
                <tr>
                    <th style='width:50px' class ="nover">ID</th>
                    <th style='width:100px'>GRUA</th>
                    <th style='width:50px'>TURNO</th>
                    <th style='width:150px' class ="nover">IDGRUDA</th>
                    <th style='width:100px'>INICIO</th>
                     <th style='width:100px'>FIN</th>
                     <th style='width:50px' class ="nover">GRUPO ID</th>
                     <th style='width:60px'>GRUPO</th>
                     <th style='width:70px'>AMARRE</th>
                     <th style='width:70px'>DESAMARRE</th>
                  </tr>
                </thead> 
                <tbody>
                </HeaderTemplate>
                <ItemTemplate>
                <tr class="point">
                <td class ="nover"><asp:Label Text='<%#Eval("id")%>' ID="lblid" runat="server" Visible="false"/></td>
                <td ><asp:Label Text='<%#Eval("crane_name")%>' ID="lblcrane_name" runat="server"  /></td>
                <td ><asp:Label Text='<%#Eval("turno_number")%>' ID="lblturno_nomber" runat="server" /></td>
                <td class ="nover"><asp:Label Text='<%#Eval("crane_id")%>' ID="lblcrane_id" runat="server" Visible="false" /></td>
                <td ><asp:Label Text='<%#Eval("turn_time_start")%>' ID="lblstart" runat="server" /></td>
                 <td ><asp:Label Text='<%#Eval("turn_time_end")%>' ID="lblend" runat="server" /></td>               
                <td class ="nover"><asp:Label Text='<%#Eval("grupo_id")%>' ID="lblgrupoid" runat="server" Visible="false" /></td> 
                <td ><asp:Label Text='<%#Eval("grupo_name")%>' ID="lblgruponame" runat="server" /></td> 
                 <td ><asp:Label Text='<%#Eval("vlock_text")%>' ID="lblvlock" runat="server" /></td> 
                <td ><asp:Label Text='<%#Eval("vunlock_text")%>' ID="lblvunlock" runat="server" /></td> 
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
     
          function Validaturno() {
              var txtDesde = document.getElementById("TxtFechaDesde");
              var txtHasta = document.getElementById("TxtFechaHasta");
            if (txtDesde.value != null && txtDesde.value != undefined && txtHasta.value != null && txtHasta.value != undefined ) {
                if (txtDesde.value > txtHasta.value) {
                    alert("La fecha del turno inicial, no puede ser mayor que la final");
                    txtDesde.value = null;
                    return false;
                }
            }
         }
    </script>
    <script src="../Scripts/opc_control.js" type="text/javascript"></script>
</asp:Content>