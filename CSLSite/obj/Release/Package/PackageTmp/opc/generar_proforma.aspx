<%@ Page  Title="Generar Proformas"  Language="C#" MasterPageFile="~/site.Master" AutoEventWireup="true" CodeBehind="generar_proforma.aspx.cs" Inherits="CSLSite.opc.generar_proforma" MaintainScrollPositionOnPostback="True"   %>
<%@ MasterType VirtualPath="~/site.Master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxtoolkit" %>

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
    <asp:ScriptManager ID="Validacion1" runat="server" />
              <div class="mt-4">         
        <nav class="mt-4" aria-label="breadcrumb">
          <ol class="breadcrumb">
            <li class="breadcrumb-item" id="opcion_principal" runat="server"><a href="#">CPF</a></li>
             <li class="breadcrumb-item active" aria-current="page" id="sub_opcion" runat="server">Generación de Proformas</li>
          </ol>
        </nav>
      </div>
    
        <div class="dashboard-container p-4" id="cuerpo" runat="server">
    <asp:UpdatePanel runat="server" id="UpdatePanel1" updatemode="Conditional">
  <ContentTemplate>

        	  <div class="form-row">
		   <div class="form-group col-md-12"> 
                  <div class="  alert alert-warning">
    <span id="dtlo" runat="server">Estimado usuario:</span> 
    <br /> Mediante esta opción podrá generar una o varias proformas para las OPC, en base a un Buque.
    </div>
		   </div>
		  </div

        
             
        
			
      <div class="form-row">
                   <div class="form-title">Consulta de Ordenes de Trabajo Cerradas y Pendientes de Emitir Proformas</div>

		   <div class="form-group col-md-12"> 
                        

          <asp:Repeater ID="TablePendientes" runat="server"  onitemcommand="GenerarProforma_ItemCommand" >
                <HeaderTemplate>
                <table id="tablasort" cellspacing="0" cellpadding="1" class="table table-bordered table-sm table-contecon">
                <thead>
                <tr>
                    <th >Estado</th>
                    <th >Id.</th>
                    <th >Referencia</th>
                    <th >Nave</th>
                    <th >ETA</th>
                     <th >ETD</th>
                    <th >Desde</th>
                    <th >Hasta</th>
                    <th >Proforma</th>
                    <th >Ver</th>
                  </tr>
                </thead> 
                <tbody>
                </HeaderTemplate>
                <ItemTemplate>
                <tr class="point">
                <td ><asp:Label Text='<%#Eval("ESTADOS")%>' ID="LblEstados" runat="server" /></td>
                <td ><asp:Label Text='<%#Eval("ID")%>' ID="lblId" runat="server"  /></td>
                <td >
                    
                    <asp:LinkButton CssClass="btn btn-link" CommandName="RefButton" CommandArgument='<%#Eval("REFERENCE") %>' ID="MostrarDetalle" runat="server" Text='<%#Eval("REFERENCE") %>'>f</asp:LinkButton>

                </td>
                <td ><asp:Label Text='<%#Eval("NAME")%>' ID="lblName" runat="server" /></td>
               <td ><asp:Label Text='<%#Eval("ETA")%>' ID="LblEta" runat="server" /></td>
                <td ><asp:Label Text='<%#Eval("ETD")%>' ID="LblEtd" runat="server" /></td>
                <td ><asp:Label Text='<%#Eval("START_WORK")%>' ID="lblStarWork" runat="server" /></td>
                <td ><asp:Label Text='<%#Eval("END_WORK")%>' ID="lblEndWork" runat="server" /></td>
                 <td class="alinear" style=" width:50px">
                    <asp:Button ID="BtnGenerarProforma"   
                        CommandName="GenButton"   OnClientClick="return confirm('Esta seguro de que desea generar las proforma?');" 
                       runat="server" Text="Generar" CssClass=" btn btn-secondary" ToolTip="Permite generar una proforma" CommandArgument='<%#Eval("REFERENCE")%>' />   
                </td>    
                <td >
                    <a class="btn btn-link" href="javascript:void popOpen('plan_preview.aspx?sid=<%# securetext(Eval("ID")) %>')" >Previsualizar</a>
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


      	 <div class="form-row">
	 <div class="form-group col-md-12"> 
      <div class="form-title">Consulta de Proformas Generadas</div>
          <asp:Repeater ID="TableProformas" runat="server"  onitemcommand="AgregarProforma_ItemCommand">
                <HeaderTemplate>
                <table id="tablasort1" cellspacing="0" cellpadding="1" class="table table-bordered table-sm table-contecon">
                <thead>
                <tr>
                    <th >No.</th>
                    <th >Fecha</th>
                    <th >Creada por</th>
                    <th >Nave</th>
                    <th >RUC</th>
                    <th >Proveedor</th>
                    <th >T/Hrs</th>
                    <th >Total USD$</th>
                   
                    <th >Ver</th>
                  </tr>
                </thead> 
                <tbody>
                </HeaderTemplate>
                <ItemTemplate>
                <tr class="point">
                <td ><asp:Label Text='<%#Eval("Id")%>' ID="LblIdProf" runat="server" /></td>
                <td ><asp:Label Text='<%#Eval("Create_date")%>' ID="LblFechaProf" runat="server"  /></td>
                <td ><asp:Label Text='<%#Eval("Create_user")%>' ID="LblUsuarioProf" runat="server" /></td>
                <td ><asp:Label Text='<%#Eval("Vessel_visit_reference")%>' ID="LblBuque" runat="server" /></td>
               <td ><asp:Label Text='<%#Eval("Opc_id")%>' ID="LblRuc" runat="server" /></td>
                <td ><asp:Label Text='<%#Eval("opc_name")%>' ID="LblProveedor" runat="server" /></td>
                <td ><asp:Label Text='<%#Eval("Total_horas")%>' ID="LblHoras" runat="server" /></td>
                <td ><asp:Label Text='<%#Eval("Total")%>' ID="LblTotal" runat="server" /></td>
                
             
                 <td class="alinear" >
                     <a class="btn btn-link" href="javascript:void popOpen('proforma_preview.aspx?sid=<%# securetext(Eval("ID")) %>')" >Visualizar</a>
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


           




</ContentTemplate>
</asp:UpdatePanel>
            </div>
   <script src="../Scripts/pages.js" type="text/javascript"></script>
   <script src="../Scripts/opc_control.js" type="text/javascript"></script>
</asp:Content>
