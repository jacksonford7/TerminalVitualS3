<%@ Page  Title="Consultar Plan"  Language="C#" MasterPageFile="~/site.Master" AutoEventWireup="true" CodeBehind="consulta_plan.aspx.cs" Inherits="CSLSite.opc.consulta_plan" %>
<%@ MasterType VirtualPath="~/site.Master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxtoolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="placehead" runat="server">

         <!--Datatable  s-->
       <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/5.15.1/css/all.min.css" integrity="sha512-+4zCK9k+qNFUR5X+cKL9EIR+ZOhtIloNl9GIKS57V1MyNsYpYcUrUeQc9vNfzsWfV28IaLL3i96P9sdNyeRssA==" crossorigin="anonymous" />
       <link href="../css/datatables.min.css" rel="stylesheet" />
       <link rel="stylesheet" type="text/css" href="..js/buttons/css1.6.4/buttons.dataTables.min.css"/>
        <script src="../lib/jquery/jquery.min.js" type="text/javascript"></script>
       <script type="text/javascript"  src="../js/datatables.js"></script>
       <script src="../Scripts/table_catalog.js" type="text/javascript"></script>  
       <script type="text/javascript" src="../js/bootstrap.bundle.min.js"></script>
    <!--   Fin  -->

    <script type="text/javascript">
        BindFunctions();

    </script>

    </asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="placebody" runat="server">


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
		                 <div class="nover">
    <span id="dtlo" runat="server" class="alert alert-warning" >Estimado usuario:</span> 
    <br /> ...
    </div>
		   </div>
		  </div>



	 <div class="form-row">
	 <div class="form-group col-md-12"> 
             <div class="form-title">Turnos Planificados</div>
               

          <asp:Repeater ID="TableTurnos" runat="server" OnItemCommand="TableTurnos_ItemCommand"  >
                <HeaderTemplate>
                <table id="tablasort" cellspacing="0" cellpadding="1" class="table table-bordered table-sm table-contecon">
                <thead>
                <tr>
                    <th style='width:50px'>ID</th>
                    <th style='width:350px'>DESCRIPCION</th>
                    <th style='width:150px'>REFERENCIA</th>
                    <th style='width:50px'>DESDE</th>
                    <th style='width:50px'>HASTA</th>
                    <th >SELECCIONAR</th>
                  </tr>
                </thead> 
                <tbody>
                </HeaderTemplate>
                <ItemTemplate>
                <tr class="point">
                <td ><asp:Label Text='<%#Eval("ID")%>' ID="lblId" runat="server"  /></td>
                <td ><asp:Label Text='<%#Eval("NAME")%>' ID="lblName" runat="server" /></td>
                <td ><asp:Label Text='<%#Eval("REFERENCE")%>' ID="lblReference" runat="server" /></td>
                <td ><asp:Label Text='<%#Eval("START_WORK")%>' ID="lblStarWork" runat="server" /></td>
                <td ><asp:Label Text='<%#Eval("END_WORK")%>' ID="lblEndWork" runat="server" /></td>
                <td >
                   <a  class="btn-link" href="javascript:void popOpen('plan_preview.aspx?sid=<%# securetext(Eval("ID")) %>')" >Previsualizar</a>
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
       
		 

		 
    
 
                  
     </div>
    <script src="../Scripts/pages.js" type="text/javascript"></script>
     <script src="../Scripts/opc_control.js" type="text/javascript"></script>
    
</asp:Content>
