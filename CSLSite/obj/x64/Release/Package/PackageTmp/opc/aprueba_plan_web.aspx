<%@ Page  Title="OPC Aprobar"  Language="C#" MasterPageFile="~/site.Master" AutoEventWireup="true" CodeBehind="aprueba_plan_web.aspx.cs" Inherits="CSLSite.opc.aprueba_plan_web" %>
<%@ MasterType VirtualPath="~/site.Master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxtoolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="placehead" runat="server">
      <link href="https://cdn.jsdelivr.net/npm/alertifyjs@1.11.0/build/css/alertify.min.css" rel="stylesheet"/>
        <script type="text/javascript" src="https://cdn.jsdelivr.net/npm/alertifyjs@1.11.0/build/alertify.min.js"></script>

       <!--Datatables-->
       <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/5.15.1/css/all.min.css" integrity="sha512-+4zCK9k+qNFUR5X+cKL9EIR+ZOhtIloNl9GIKS57V1MyNsYpYcUrUeQc9vNfzsWfV28IaLL3i96P9sdNyeRssA==" crossorigin="anonymous" /><link href="../css/datatables.min.css" rel="stylesheet" /><link rel="stylesheet" type="text/css" href="..js/buttons/css1.6.4/buttons.dataTables.min.css" />
        <script src="../lib/jquery/jquery.min.js" type="text/javascript"></script>
       <script type="text/javascript"  src="../js/datatables.js"></script>
       <script src="../Scripts/table_catalog.js" type="text/javascript"></script>  
       <script type="text/javascript" src="../js/bootstrap.bundle.min.js"></script>
    <!--Fin-->



    </asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="placebody" runat="server">
    <input id="zonaid" type="hidden" value="205" />

    <div class="mt-4">         
        <nav class="mt-4" aria-label="breadcrumb">
          <ol class="breadcrumb">
            <li class="breadcrumb-item" id="opcion_principal" runat="server"><a href="#">PCF</a></li>
             <li class="breadcrumb-item active" aria-current="page" id="sub_opcion" runat="server">Aprobación de Ordenes de Trabajos para ser visualizadas por las OPC</li>
          </ol>
        </nav>
      </div>

     <div class="dashboard-container p-4" id="cuerpo" runat="server">

		  <div class="form-row">
		   <div class="form-group col-md-12"> 
		   	         <div class=" alert alert-warning">
    <span id="dtlo" runat="server">Estimado usuario:</span> 
    <br /> Cada orden de trabajo que usted aprueba mediante esta opción será visualizada por las OPC, las mismas serán encargadas de seleccionar los turnos y asignar a su personal para realizar los trabajos.
    </div>
		   </div>
		  </div>
		 

                       <div class="booking" >
               <div class="form-title">Seleccionar O/T</div>
              <div class="bokindetalle">

          <asp:Repeater ID="TableTurnos" runat="server" OnItemCommand="AprobarPlam_Web_ItemCommand"  >
                <HeaderTemplate>
                <table id="tablasort" cellspacing="0" cellpadding="1" class="table table-bordered table-sm table-contecon">
                <thead>
                <tr>
                    <th >ESTADO</th>
                    <th >ID</th>
                    <th >REFERENCIA</th>
                    <th>NAVE</th>
                    <th >ETA</th>
                     <th >ETD</th>
                    <th >DESDE</th>
                    <th >HASTA</th>
                    <th >VER</th>
                    <th >APROBAR</th>
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
                   <a class="btn  btn-link" href="javascript:void popOpen('plan_preview.aspx?sid=<%# securetext(Eval("ID")) %>')" >Previsualizar</a>
                </td>
               
                    
                    
                    <td class="alinear" >
                    <asp:Button ID="BtnConfirmar"  
                       OnClientClick="return confirm('Esta seguro de que desea aprobar la O/T?');" 
                      CommandArgument='<%#Eval("ID")%>' 
                       runat="server" Text="Aprobar" CssClass="  btn btn-secondary" ToolTip="Permite aprobar un O/T" />
                    
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
