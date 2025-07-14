<%@ Page  Title="Iniciar Trabajos"  Language="C#" MasterPageFile="~/site.Master" AutoEventWireup="true" CodeBehind="iniciar_trabajos.aspx.cs" Inherits="CSLSite.opc.iniciar_trabajos" %>
<%@ MasterType VirtualPath="~/site.Master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxtoolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="placehead" runat="server">

     <link href="../shared/estilo/tables.css" rel="stylesheet" type="text/css" />
     <link href="../shared/estilo/catalogos.css" rel="stylesheet" type="text/css" />
    

    </asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="placebody" runat="server">
           <div class="mt-4">         
        <nav class="mt-4" aria-label="breadcrumb">
          <ol class="breadcrumb">
            <li class="breadcrumb-item" id="opcion_principal" runat="server"><a href="#">CPF</a></li>
             <li class="breadcrumb-item active" aria-current="page" id="sub_opcion" runat="server">Iniciar/Aprobar una órden de trabajo</li>
          </ol>
        </nav>
      </div>
    <div class="dashboard-container p-4" id="cuerpo" runat="server">

         <div class="form-title">Datos del documento buscado</div>
		 
        <div class="form-row">
            <div  class=" form-group col-md-12">
                    <div class="  alert alert-warning">
    <span id="dtlo" runat="server">Estimado usuario:</span> 
    <br /> Mediante esta opción, podrá aprobar e iniciar los trabajos de cada transacción.</div>
            </div>

        </div>
		  <div class="form-row">
		  
		   <div class="form-group col-md-12"> 
                  <div class="bokindetalle">

          <asp:Repeater ID="TableTurnos" runat="server" OnItemCommand="IniciarTrabajo_ItemCommand"  >
                <HeaderTemplate>
                <table id="tablasort" cellspacing="0" cellpadding="1" class="table table-bordered invoice">
                <thead>
                <tr>
                    <th >Estado</th>
                    <th >ID</th>
                    <th >Referencia</th>
                    <th>Nave</th>
                    <th >ETA</th>
                     <th >ETD</th>
                    <th >Desde</th>
                    <th >Hasta</th>
                    <th >Ver</th>
                    <th >Iniciar</th>
                  </tr>
                </thead> 
                <tbody>
                </HeaderTemplate>
                <ItemTemplate>
                <tr class="point">
                <td ><asp:Label Text='<%#Eval("ESTADOS")%>' ID="LblEstados" runat="server" /></td>
                <td ><asp:Label Text='<%#Eval("ID")%>' ID="lblId" runat="server"  /></td>
                <td ><asp:Label Text='<%#Eval("REFERENCE")%>' ID="lblReference" runat="server" /></td>
                <td ><asp:Label Text='<%#Eval("NAME")%>' ID="lblName" runat="server" /></td>
               <td ><asp:Label Text='<%#Eval("ETA")%>' ID="LblEta" runat="server" /></td>
                <td ><asp:Label Text='<%#Eval("ETD")%>' ID="LblEtd" runat="server" /></td>
                <td ><asp:Label Text='<%#Eval("START_WORK")%>' ID="lblStarWork" runat="server" /></td>
                <td ><asp:Label Text='<%#Eval("END_WORK")%>' ID="lblEndWork" runat="server" /></td>
                
                 <td >
                   <a class="btn btn-link" href="javascript:void popOpen('plan_preview.aspx?sid=<%# securetext(Eval("ID")) %>')" >Previsualizar</a>
                </td>
                 <td class="alinear">
                   
                     
                     <asp:Button ID="BtnConfirmar"  
                       OnClientClick="return confirm('Esta seguro de que desea iniciar la O/T?');" 
                      CommandArgument='<%#Eval("ID")%>' 
                       runat="server" Text="APROBAR" CssClass=" btn  btn-secondary" ToolTip="Iniciar una O/T" />
                    
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
		 
     </div>

    <script src="../Scripts/pages.js" type="text/javascript"></script>
     <script src="../Scripts/opc_control.js" type="text/javascript"></script>
    
</asp:Content>
