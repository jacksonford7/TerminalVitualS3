<%@ Page  Title="Anular proforma"  Language="C#" MasterPageFile="~/site.Master" AutoEventWireup="true" CodeBehind="anulacion_proformas.aspx.cs" Inherits="CSLSite.opc.anulacion_proformas" MaintainScrollPositionOnPostback="True"  %>
<%@ MasterType VirtualPath="~/site.Master" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxtoolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="placehead" runat="server">
       
    <!--mensajes-->
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
       <asp:ScriptManager ID="Validacion1" runat="server" />
   
    
           <div class="mt-4">         
        <nav class="mt-4" aria-label="breadcrumb">
          <ol class="breadcrumb">
            <li class="breadcrumb-item" id="opcion_principal" runat="server"><a href="#">PCF</a></li>
             <li class="breadcrumb-item active" aria-current="page" id="sub_opcion" runat="server">Anulación de Proformas</li>
          </ol>
        </nav>
      </div>
    
     <div class="dashboard-container p-4" id="cuerpo" runat="server">
	 
      <asp:UpdatePanel runat="server" id="UpdatePanel1" updatemode="Conditional">
         <ContentTemplate>
         
         <div class="form-row">
		   <div class="form-group col-md-12"> 
                  <div class=" alert alert-warning">
    <span id="dtlo" runat="server">Estimado usuario:</span> 
    <br /> Mediante esta opción podrá anular todas las proformas que se generaron durante el proceso de operaciones de un buque.
    </div>
		   	
		   </div>
		  </div>

           <div class="form-row">
		      <div class="form-title">Buscar Proformas</div>
		   <div class="form-group col-md-6"> 
		   	  		   	  <label for="inputAddress">Referencia<span style="color: #FF0000; font-weight: bold;"></span></label>
               <div class="d-flex">
                     <asp:TextBox ID="TxtReferencia" runat="server" CssClass="form-control"
            MaxLength="15"
          onkeypress="return soloLetras(event,'abcdefghijklmnñopqrstuvwxyz1234567890-',true)"  placeholder="Nave"></asp:TextBox>
             <span class="validacion" id="xplinea" > *</span>
               </div>

		   </div>
		
		  
		   <div class="form-group col-md-6"> 
		   	  <label for="inputAddress">Tipo de Consulta<span style="color: #FF0000; font-weight: bold;"></span></label>
			  <div class="d-flex">
                               <asp:RadioButton ID="RdbProceso" runat="server" Checked="True" Text="Por Proceso" GroupName="opt" CssClass=" form-check"/>
             <asp:RadioButton ID="RdbAdicionales" runat="server" Text="Adicionales" GroupName="opt" CssClass="form-check" />  


			  </div>
		   </div>
		  </div>

         	   <div class="form-row">
		   <div class="col-md-12 d-flex justify-content-center"> 
                        <asp:Button ID="BtnBuscar"  CssClass="btn btn-primary"
             runat="server" OnClientClick="return sendformReferencia();"   OnClick="BtnBuscar_Click" Text="Consultar" Width="163px"/>

		   </div> 
		   </div>
		 
              <div class="form-row">
	 <div class="form-group col-md-12"> 
               <div class="form-title">Detalle de proformas</div>
          <asp:Repeater ID="TablePendientes" runat="server"  >
                <HeaderTemplate>
                <table id="tablasort" cellspacing="0" cellpadding="1" class="table table-bordered table-sm table-contecon">
                <thead>
                <tr>
                    <th ># PROFORMA</th>
                    <th >FECHA</th>
                    <th >CREADA POR</th>
                    <th >BUQUE</th>
                    <th>RUC</th>
                    <th >PROVEEDOR</th>
                    <th>T/HORAS</th>
                    <th >TOTAL $</th>
                    <th >IMPRIMIR</th>
                  
                  </tr>
                </thead> 
                <tbody>
                </HeaderTemplate>
                <ItemTemplate>
                <tr class="point">
                <td ><asp:Label Text='<%#Eval("Id") %>' ID="LblIdProf" runat="server" /></td>
                <td ><asp:Label Text='<%#Eval("Create_date")%>' ID="LblFechaProf" runat="server"  /></td>
                <td ><asp:Label Text='<%#Eval("Create_user")%>' ID="LblUsuarioProf" runat="server" /></td>
                <td ><asp:Label Text='<%#Eval("Vessel_visit_reference")%>' ID="LblBuque" runat="server" /></td>
               <td ><asp:Label Text='<%#Eval("Opc_id")%>' ID="LblRuc" runat="server" /></td>
                <td ><asp:Label Text='<%#Eval("opc_name")%>' ID="LblProveedor" runat="server" /></td>
                <td ><asp:Label Text='<%#Eval("Total_horas")%>' ID="LblHoras" runat="server" /></td>
                <td ><asp:Label Text='<%#Eval("Total")%>' ID="LblTotal" runat="server" /></td>
                
                <td ><a href="javascript:void popOpen('proforma_preview.aspx?sid=<%# securetext(Eval("Id")) %>')">Visualizar</a></td>
                   
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

          <div class="form-row">
		  
		   <div class="form-group  justify-content-center col-md-12  d-flex"> 
		   	 
			 
                           <asp:UpdatePanel runat="server" id="UpdatePanel2" updatemode="Conditional">
         <ContentTemplate>
         <img alt="loading.." src="../shared/imgs/loader.gif" id="loader" class="nover"  />
         <asp:Button ID="BtnGrabar" CssClass="btn btn-primary mr-3" runat="server" OnClientClick="return confirm('Esta seguro de que desea anular las proformas?');"   OnClick="BtnGrabar_Click"  Text="Anular" /> 
          <asp:Button ID="BtnNuevo" CssClass="btn btn-primary"  runat="server"  OnClick="BtnNuevo_Click" Text="Nuevo" /> 
          </ContentTemplate>
        </asp:UpdatePanel>
			  
		   </div>
		  </div>
     </div>

    <script src="../Scripts/pages.js" type="text/javascript"></script>
     <script src="../Scripts/opc_control.js" type="text/javascript"></script>
      <script type="text/javascript">

          function sendform() {
              var vals = document.getElementById('<%=TxtReferencia.ClientID %>').value;
                if (vals == '' || vals == null || vals == undefined) {
                    alertify.alert('* Debe ingresar la referencia de la nave *');
                    document.getElementById('<%=TxtReferencia.ClientID %>').focus();
                    document.getElementById('<%=TxtReferencia.ClientID %>').style.cssText = "background-color:#FDFD96;";
                    document.getElementById("loader").className = 'nover';
                    return false;
              }
            return true;
          }
           function sendformReferencia() {
              var vals = document.getElementById('<%=TxtReferencia.ClientID %>').value;
                if (vals == '' || vals == null || vals == undefined) {
                 alertify.alert('* Debe ingresar la referencia de la nave *');
                    document.getElementById('<%=TxtReferencia.ClientID %>').focus();
                    document.getElementById('<%=TxtReferencia.ClientID %>').style.cssText = "background-color:#FDFD96;";
                    document.getElementById("loader").className = 'nover';
                    return false;
              }
            return true;
          }
    </script>
</asp:Content>
