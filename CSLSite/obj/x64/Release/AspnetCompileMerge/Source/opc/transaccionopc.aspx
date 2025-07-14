<%@ Page  Title="Nuevo Plan"  Language="C#" MasterPageFile="~/site.Master" AutoEventWireup="true" CodeBehind="transaccionopc.aspx.cs" Inherits="CSLSite.transaccionopc" MaintainScrollPositionOnPostback="True" %>
<%@ MasterType VirtualPath="~/site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="placehead" runat="server">
     <link href="../shared/estilo/jquery.datetimepicker.css" rel="stylesheet" type="text/css" />
      <link href="../shared/estilo/catalogos.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/aisv-general.css" rel="stylesheet" type="text/css" />
  
     <!--mensajes-->
        <link href="https://cdn.jsdelivr.net/npm/alertifyjs@1.11.0/build/css/alertify.min.css" rel="stylesheet"/>
        <script type="text/javascript" src="https://cdn.jsdelivr.net/npm/alertifyjs@1.11.0/build/alertify.min.js"></script>
 </asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="placebody" runat="server" >

           <div class="mt-4">         
        <nav class="mt-4" aria-label="breadcrumb">
          <ol class="breadcrumb">
            <li class="breadcrumb-item" id="opcion_principal" runat="server"><a href="#">CPF</a></li>
             <li class="breadcrumb-item active" aria-current="page" id="sub_opcion" runat="server">Transacción de Trabajo por Buque y Grúas</li>
          </ol>
        </nav>
      </div>
       <div class="dashboard-container p-4" id="cuerpo" runat="server">
           <div class="form-row">
		   <div class="form-group col-md-12"> 
                   <div class=" alert alert-warning">
    <span id="dtlo" runat="server">Estimado usuario</span> 
    <br />
    </div>
		   </div>
		  </div>
           <div class="form-title">Datos de la nave</div>
		  <div class="form-row">
		  
		   <div class="form-group col-md-12"> 
		   	  <label for="inputAddress">Referencia<span style="color: #FF0000; font-weight: bold;"></span></label>
			  <div class="d-flex">
  <asp:TextBox ID="TxtReferencia" runat="server" CssClass=" form-control" 
            MaxLength="10"  ClientIDMode="Static"
         onkeypress="return soloLetras(event,'abcdefghijklmnopqrstuvwxyz1234567890',true)" 
            
            placeholder="Referencia"></asp:TextBox>
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
                             <asp:Label ID="LblNombre" CssClass=" form-control  col-md-12" runat="server" Text="LblNombre" ></asp:Label>
             <asp:Label ID="LblVoyageIn"  runat="server" Text="LblVoyageIn"  Visible="false"></asp:Label>
              <asp:Label ID="LblVoyageOut" runat="server" Text="LblVoyageOut"  Visible="false"></asp:Label>


			  </div>
		   </div>
		  </div>

           <div class="form-row">
		  
		   <div class="form-group col-md-6"> 
		   	  <label for="inputAddress">ETA<span style="color: #FF0000; font-weight: bold;"></span></label>
			                   <asp:Label ID="LblETA"
                                    CssClass="form-control  col-md-6"
                                   runat="server" Text="LblETA" ></asp:Label>

		   </div>

                  <div class="form-group col-md-6"> 
		   	  <label for="inputAddress">ETD<span style="color: #FF0000; font-weight: bold;"></span></label>
		                <asp:Label ID="LblETD"  CssClass="form-control  col-md-6" 
                            runat="server" Text="LblETD" ></asp:Label>

                  </div>
		  </div>

           <div class="form-row">
		  
		   <div class="form-group col-md-6"> 
		   	  <label for="inputAddress">Viaje<span style="color: #FF0000; font-weight: bold;"></span></label>
			               <asp:Label ID="LblViaje" runat="server"   CssClass="form-control  col-md-6"
                               Text="LblViaje" ></asp:Label>


		   </div>

                  <div class="form-group col-md-6"> 
		   	  <label for="inputAddress">Trabajo (Hrs)<span style="color: #FF0000; font-weight: bold;"></span></label>
			              <asp:Label ID="LblHoras" runat="server"  CssClass="form-control  col-md-6"
                              Text="LblHoras" ></asp:Label>

		   </div>
		  </div>

           <div class="form-row">
		  
		   <div class="form-group col-md-12"> 
		   	  <label for="inputAddress">Fecha y Hora de la Cita<span style="color: #FF0000; font-weight: bold;"></span></label>
			  <div class="d-flex">
                           <asp:TextBox ID="TxtFechacita" runat="server" MaxLength="16"
             CssClass="datetimepicker form-control"
               onkeypress="return soloLetras(event,'1234567890/:')"
               onBlur="valDate(this,  true,valdatem);"
               placeholder="dd/mm/aaaa hh:mm"
             onchange="defaultDate(this);"
               ></asp:TextBox>
             <span class="validacion" id="valfechacita"  > * </span>
			  </div>
		   </div>
		  </div>
		    <div class="form-title">Datos de la Máquina</div>
           <div class="form-row">
		  
		   <div class="form-group col-md-4"> 
		   	  <label for="inputAddress">Máquina<span style="color: #FF0000; font-weight: bold;"></span></label>
			 <div class="d-flex">
                    <asp:DropDownList ID="CboGruas" runat="server" Width="250px" AutoPostBack="False"
                                              DataTextField='Id' DataValueField='GKey'  
                                            CssClass="form-control"
                                            onselectedindexchanged="CboGruas_SelectedIndexChanged">
                                        </asp:DropDownList>
              <span class="validacion" id="valcbogrua"  > * </span>
			 </div>
		   </div>
               		   <div class="form-group col-md-4"> 
		   	  <label for="inputAddress">Desde<span style="color: #FF0000; font-weight: bold;"></span></label>
			  <div class="d-flex">
                    <asp:TextBox ID="TxtFechaDesde" runat="server" MaxLength="16" 
               CssClass="datetimepicker  form-control"
               onkeypress="return soloLetras(event,'1234567890/:')"
               onBlur="valDate(this,  true,valdatem);"
               placeholder="dd/mm/aaaa hh:mm"
                ClientIDMode="Static"
               ></asp:TextBox>
             <span class="validacion" id="valFechaDesde"  > * </span>

			  </div>
		   </div>

                <div class="form-group col-md-4"> 
		   	  <label for="inputAddress">Trabajo (Hrs)<span style="color: #FF0000; font-weight: bold;"></span></label>
			  <div class="d-flex">
                             <asp:TextBox ID="TxtHorasTrabajo" runat="server" CssClass=" form-control" MaxLength="15"
          onkeypress="return soloLetras(event,'1234567890',false)" onpaste="return false;"></asp:TextBox>
          <span class="validacion" id="valhoras"  > * </span>

			  </div>
		   </div>
		  </div>

           <div class="form-row">
		   <div class="col-md-12 d-flex justify-content-center">
                 <asp:Button ID="BtnAgregar"  CssClass="btn btn-secondary"
              runat="server"  OnClick="BtnAgregar_Click" Text="Agregar" />
		   </div> 
		   </div>

                        <div class="bokindetalle">

          <asp:Repeater ID="TableGruas" runat="server"  onitemcommand="RemoverGruas_ItemCommand">
                <HeaderTemplate>
                <table id="tablasort" cellspacing="0" cellpadding="1" class="table table-bordered invoice">
                <thead>
                <tr>
                    <th >ID</th>
                    <th >GRUA</th>
                    <th >DESDE</th>
                    <th >HORAS</th>
                    <th class ="nover">NOTA</th>
                    <th >REMOVER</th>
                  </tr>
                </thead> 
                <tbody>
                </HeaderTemplate>
                <ItemTemplate>
                <tr class="point">
                <td ><asp:Label Text='<%#Eval("Crane_Gkey")%>' ID="lblCarga" runat="server"  /></td>
                <td ><asp:Label Text='<%#Eval("Crane_name")%>' ID="lblCntr" runat="server" /></td>
                <td ><asp:Label Text='<%#Eval("DateWork")%>' ID="lblTipo" runat="server" /></td>
                <td ><asp:Label Text='<%#Eval("Crane_time_qty")%>' ID="lblFacA" runat="server" /></td>
                <td class ="nover">
                     <asp:TextBox ID="TxtNota" 
                         runat="server" AutoPostBack="False"   CssClass="form-control"
                         onkeypress="return soloLetras(event,'abcdefghijklmnñopqrstuvwxyz1234567890_-')" Visible="false"></asp:TextBox>
                </td>                  
                <td >
                  
                    <asp:Button ID="BtnConfirmar"  
                       OnClientClick="return confirm('Está seguro de que desea remover la grúa?......Al realizar esta acción se eliminarán todos los turnos de la grúa.');" 
                       runat="server" Text="Remover" CssClass=" btn-secondary" ToolTip="Permite remover una grúa" CommandArgument='<%#Eval("Crane_Gkey")%>' />
                    
                </td>
                </tr>
                </ItemTemplate>
                <FooterTemplate>
                </tbody>
                </table>
                </FooterTemplate>
        </asp:Repeater>
                   </div>

        	  
	   <div class="form-row">
		   <div class="col-md-12 d-flex justify-content-center     p-1"> 
               <div class="d-flex">
    
          
          <asp:Button ID="BtnGrabar" runat="server"  OnClick="BtnGrabar_Click" Text="Grabar" 
               CssClass="btn btn-primary mr-2"/> 


          <asp:Button ID="BtnNuevo" runat="server"  OnClick="BtnNuevo_Click" Text="Nuevo" 
             CssClass="btn btn-primary ml-2 "/> 

                                               <img alt="loading.." src="../shared/imgs/loader.gif" id="loader" class="nover"  />
       

               </div>
		   </div> 
		   </div>
    </div>

    <script src="../Scripts/jquery.datetimepicker.js" type="text/javascript"></script>
    <script src="../Scripts/pages.js" type="text/javascript"></script>
   <script src="../Scripts/opc_control.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $('.datetimepicker').datetimepicker().datetimepicker({ lang: 'es', step: 30, format: 'd/m/Y H:i' });
            $('.datetimepickerAlt').datetimepicker().datetimepicker({ lang: 'es', step: 30, format: 'd/m/Y' });
            $('.datepicker').datetimepicker().datetimepicker({ lang: 'es', timepicker: false, format: 'd/m/Y', closeOnDateSelect: true, minDate: '0' });
        });
      
        function cancelEmpty() {
            var txt = document.getElementById("TxtReferencia");
            if (txt != null && txt != undefined) {
                if (txt.value) {
                    return true;
                }
            }
            alertify.alert("Por favor escriba la referencia");
            return false;
        }
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