<%@ Page  Title="OPC Nuevo Grupo"  Language="C#" MasterPageFile="~/site.Master" 
    AutoEventWireup="true" CodeBehind="grupo.aspx.cs" Inherits="CSLSite.grupo" %>
<%@ MasterType VirtualPath="~/site.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="placehead" runat="server">
     <!--Datatables-->
       <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/5.15.1/css/all.min.css" integrity="sha512-+4zCK9k+qNFUR5X+cKL9EIR+ZOhtIloNl9GIKS57V1MyNsYpYcUrUeQc9vNfzsWfV28IaLL3i96P9sdNyeRssA==" crossorigin="anonymous" /><link href="../css/datatables.min.css" rel="stylesheet" /><link rel="stylesheet" type="text/css" href="..js/buttons/css1.6.4/buttons.dataTables.min.css" />
        <script src="../lib/jquery/jquery.min.js" type="text/javascript"></script>
       <script type="text/javascript"  src="../js/datatables.js"></script>
       <script src="../Scripts/table_catalog.js" type="text/javascript"></script>  
       <script type="text/javascript" src="../js/bootstrap.bundle.min.js"></script>
    <!--Fin-->

 

 </asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="placebody" runat="server">
    <input id="identificacion" type="hidden" value="" runat="server" clientidmode="Static" />
     <input id="nombres" type="hidden" value="" runat="server" clientidmode="Static" />
    <input id="apellidos" type="hidden" value="" runat="server" clientidmode="Static" />
       <div class="mt-4">         
        <nav class="mt-4" aria-label="breadcrumb">
          <ol class="breadcrumb">
            <li class="breadcrumb-item" id="opcion_principal" runat="server"><a href="#">CPF</a></li>
             <li class="breadcrumb-item active" aria-current="page" id="sub_opcion" runat="server">Crear un grupo de operarios</li>
          </ol>
        </nav>
      </div>
    <div class="dashboard-container p-4" id="cuerpo" runat="server">

         <div class="form-title">Datos para la cuadrilla</div>
		  <div class="form-row">
		  
		   <div class="form-group col-md-12"> 
                    <div class="alert alert-warning">
    <span id="dtlo" runat="server">Estimado usuario:</span> 
    <br /> Por favor asegúrese de selecionar a las personas correctas de la lista antes de guardar el grupo
    </div>

		   </div></div>
		  <div class="form-row">
		  
		   <div class="form-group col-md-12"> 
		   	  <label for="inputAddress">Operadora Actual<span style="color: #FF0000; font-weight: bold;"></span></label>
			 <span runat="server" id="operator_no" class=" form-control col-md-12 ">... </span>
		   </div>
		  </div>
		 	  <div class="form-row">
		  
		   <div class="form-group col-md-6"> 
		   	  <label for="inputAddress">Operadora Cuadrilla:<span style="color: #FF0000; font-weight: bold;"></span></label>
			                  <asp:DropDownList ID="dpopc" runat="server"  CssClass="form-control" ClientIDMode="Static"></asp:DropDownList>

		   </div>

                     <div class="form-group col-md-6"> 
		   	  <label for="inputAddress">Nombre:<span style="color: #FF0000; font-weight: bold;"></span></label>
			                   <asp:TextBox ID="txtnume" runat="server"  CssClass="form-control" ></asp:TextBox>

		   </div>
		  </div>

        	  <div class="form-row">
		  
		   <div class="form-group col-md-12"> 
		   	  <label for="inputAddress">Nombres y Apellidos:<span style="color: #FF0000; font-weight: bold;"></span></label>
			  <div class="d-flex">
                             
                  <span clientidmode="Static" onclick="Golink();" runat="server" id="operario_name" class=" form-control col-md-11"  style="cursor:pointer;">Click aqui para buscar </span>
               
               
                   <asp:Button   CssClass=" btn btn-primary" ID="btAdd" runat="server" Text="Agregar" OnClick="btAdd_Click" OnClientClick="return check_info();" />
                           

			  </div>
          


		   </div>
		  </div>
         <div class="form-title">Personal seleccionado</div>
            <div class="bokindetalle">
                 <asp:Repeater ID="rp_turno" runat="server" OnItemCommand="rp_turno_ItemCommand" >
                 <HeaderTemplate>
                 <table id="tablasort" cellspacing="1" cellpadding="1" class="table table-bordered table-sm table-contecon">
                 <thead>
                 <tr>
                 <th>Identificación</th>
                 <th>Nombres y Apellidos</th>
                  <th>Acciones</th>
                 </tr>
                 </thead> 
                 <tbody>
                 </HeaderTemplate>
                 <ItemTemplate>
                 <tr class="point" >
                  <td><%#Eval("ope_id")%></td>
                  <td><%# unir( Eval("ope_nombre"), Eval("ope_apellido")) %>,</td>
                  <td>
                   <div class="tcomand">
                       <asp:Button CssClass="btn btn-secondary" CommandArgument='<%# Eval("ope_id")%>'  ID="bt_quitar" runat="server" Text="Remover de la lista"   />
                   </div>
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
		   <div class="col-md-12 d-flex justify-content-center"> 
                             <asp:Button  CssClass="btn btn-primary"
                  OnClientClick="return confirm('Está seguro de guardar la cuadrilla, este proceso es irreversible?');"   ID="btsalvar" runat="server" Text="Guardar cuadrilla" OnClick="btsalvar_Click" />

		   </div> 
		   </div>
     </div>

    <script src="../Scripts/opc_control.js" type="text/javascript"></script>
</asp:Content>