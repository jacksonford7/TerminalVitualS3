<%@ Page  Title="Generar Proforma Adicional"  Language="C#" MasterPageFile="~/site.Master" AutoEventWireup="true" CodeBehind="generar_proforma_adicional.aspx.cs" Inherits="CSLSite.opc.generar_proforma_adicional" MaintainScrollPositionOnPostback="True"  %>
<%@ MasterType VirtualPath="~/site.Master" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxtoolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="placehead" runat="server">
    <link href="../shared/estilo/aisv-general.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/work.css" rel="stylesheet" type="text/css" />
     <link href="../shared/estilo/tables.css" rel="stylesheet" type="text/css" />
     <link href="../shared/estilo/catalogos.css" rel="stylesheet" type="text/css" />
     <link href="../shared/estilo/info.css" rel="stylesheet" type="text/css" />
       <!--mensajes-->
        <link href="https://cdn.jsdelivr.net/npm/alertifyjs@1.11.0/build/css/alertify.min.css" rel="stylesheet"/>
        <script type="text/javascript" src="https://cdn.jsdelivr.net/npm/alertifyjs@1.11.0/build/alertify.min.js"></script>

    </asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="placebody" runat="server">
      <div class="mt-4">         
        <nav class="mt-4" aria-label="breadcrumb">
          <ol class="breadcrumb">
            <li class="breadcrumb-item" id="opcion_principal" runat="server"><a href="#">CPF</a></li>
             <li class="breadcrumb-item active" aria-current="page" id="sub_opcion" runat="server">Generación de Proformas Adicionales a un Buque Específico, para las OPC</li>
          </ol>
        </nav>
      </div>

    <div class="dashboard-container p-4" id="cuerpo" runat="server">
		   <div class="form-row">
		       <div class="form-group col-md-12"> 
		   	    <div class="  alert alert-warning">
                    <span id="dtlo" runat="server" class="alert alert-warning">Estimado usuario:</span> 
                    <br /> Mediante esta opción podrá generar una proforma adicional para las OPC 
                    </div>
		       </div>
		  </div>
           <div class="form-title">Detalle de  proformas</div>
		   <div class="form-row">
		   <div class="form-group col-md-12"> 
		   	  <label for="inputAddress">Referencia<span style="color: #FF0000; font-weight: bold;"></span></label>
			  <div class="d-flex">
                     <asp:TextBox ID="TxtReferencia" runat="server" CssClass=" form-control " MaxLength="15"
          onkeypress="return soloLetras(event,'abcdefghijklmnñopqrstuvwxyz1234567890-',true)"
            placeholder="Nave"></asp:TextBox>
             
         <asp:Button ID="BtnBuscar" runat="server"  CssClass="btn btn-primary"
             OnClientClick="return sendformReferencia();" 
             OnClick="BtnBuscar_Click" Text="Consultar" />
                  <span class="validacion" id="xplinea" > * </span>
			  </div>
		   </div>
		  </div>
           <div class="form-row">
		  
		   <div class="form-group col-md-12"> 
		   	  <label for="inputAddress">Nombre<span style="color: #FF0000; font-weight: bold;"></span></label>
			  <asp:Label ID="LblNombre" runat="server" Text="LblNombre" CssClass=" form-control col-12" ></asp:Label>
		   </div>

      
		  </div>
           <div class="form-row">
		  
		   <div class="form-group col-md-6"> 
		   	  <label for="inputAddress">ETA<span style="color: #FF0000; font-weight: bold;"></span></label>
			     <asp:Label ID="LblETA" runat="server" Text="LblETA" CssClass=" form-control col-6" ></asp:Label>

		   </div>

                <div class="form-group col-md-6"> 
		   	  <label for="inputAddress">ETD<span style="color: #FF0000; font-weight: bold;"></span></label>
			               <asp:Label ID="LblETD" runat="server" Text="LblETD" CssClass=" form-control col-6"  ></asp:Label>

		   </div>
		  </div>
           <div class="form-row">
		  
		   <div class="form-group col-md-6"> 
		   	  <label for="inputAddress">Viaje<span style="color: #FF0000; font-weight: bold;"></span></label>
			              <asp:Label ID="LblViaje" runat="server" Text="LblViaje" CssClass=" form-control col-6" ></asp:Label>
		   </div>

               <div class="form-group col-md-6"> 
		   	  <label for="inputAddress">Trabajo Hrs<span style="color: #FF0000; font-weight: bold;"></span></label>
			   <asp:Label ID="LblHoras" runat="server" Text="LblHoras" CssClass=" form-control col-6" ></asp:Label>

		   </div>
		  </div>

         <div class="form-row">
		   <div class="col-md-12 d-flex "> 
		                  <div class="bokindetalle">

          <asp:Repeater ID="TablePendientes" runat="server"  onitemcommand="AgregarProforma_ItemCommand"  OnItemDataBound="Opciones_ItemDataBound">
                <HeaderTemplate>
                <table id="tablasort" cellspacing="0" cellpadding="1" class="table table-bordered invoice">
                <thead>
                <tr>
                    <th >No.</th>
                    <th class=" nover" >FECHA</th>
                    <th >CREADA POR</th>
                    <th class=" nover"  >BUQUE</th>
                    <th>RUC</th>
                    <th >PROVEEDOR</th>
                    <th >T/HORAS</th>
                    <th >TOTAL $</th>
                    <th >TIPO</th>
                    <th >IMPRIMIR</th>
                    <th >ADICIONAR</th>
                  </tr>
                </thead> 
                <tbody>
                </HeaderTemplate>
                <ItemTemplate>
                <tr class="point">
                <td ><asp:Label Text='<%#Eval("Id") %>' ID="LblIdProf" runat="server" /></td>
                <td class=" nover" ><asp:Label Text='<%#Eval("Create_date")%>' ID="LblFechaProf" runat="server"  /></td>
                <td ><asp:Label Text='<%#Eval("Create_user")%>' ID="LblUsuarioProf" runat="server" /></td>
                <td class=" nover"  ><asp:Label Text='<%#Eval("Vessel_visit_reference")%>' ID="LblBuque" runat="server" /></td>
               <td ><asp:Label Text='<%#Eval("Opc_id")%>' ID="LblRuc" runat="server" /></td>
                <td ><asp:Label Text='<%#Eval("opc_name")%>' ID="LblProveedor" runat="server" /></td>
                <td ><asp:Label Text='<%#Eval("Total_horas")%>' ID="LblHoras" runat="server" /></td>
                <td ><asp:Label Text='<%#Eval("Total")%>' ID="LblTotal" runat="server" /></td>
               <td ><asp:Label Text='<%#Eval("Adicional")%>' ID="LblTipo" runat="server" /></td>
                <td ><a class="btn btn-link" href="javascript:void popOpen('proforma_preview.aspx?sid=<%# securetext(Eval("Id")) %>')">Imprimir</a></td>
                 <td class="alinear">
                    <asp:Button ID="BtnAdicionar"    
                       runat="server" Text="Adicionar" CssClass=" btn  btn-secondary" ToolTip="Permite generar una proforma adicional" CommandArgument='<%#Eval("Id")%>' />   
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



 
           <div class="form-title">Generación de Proformas</div>
             <div class="form-row">
		  
		           <div class="form-group col-4"> 
		   	  <label for="inputAddress">Fecha<span style="color: #FF0000; font-weight: bold;"></span></label>
			               <asp:Label ID="LblFechaProforma" runat="server" Text="LblFechaProforma" CssClass=" form-control col-10" > </asp:Label>

		   </div>
                   <div class="form-group col-4"> 
		   	  <label for="inputAddress">RUC<span style="color: #FF0000; font-weight: bold;"></span></label>
		             <asp:Label ID="LblRucProv" runat="server" Text="LblRucProv"  CssClass=" form-control col-10" ></asp:Label>

		   </div>
                   <div class="form-group col-4"> 
		   	  <label for="inputAddress">Proveedor<span style="color: #FF0000; font-weight: bold;"></span></label>
			   <asp:Label ID="LblProveedor" runat="server" Text="LblProveedor"  CssClass=" form-control col-10" ></asp:Label>
		   </div>
		  </div>
         <div class="form-row">
		  
		   <div class="form-group col-md-12"> 
		   	  <label for="inputAddress">Concepto<span style="color: #FF0000; font-weight: bold;"></span></label>
			            <asp:DropDownList ID="CboConcepto" runat="server"  AutoPostBack="True"
                                             DataTextField='name' DataValueField='id'  
                                            CssClass="form-control" 
                                           OnSelectedIndexChanged="CboConcepto_SelectedIndexChanged" >
                                        </asp:DropDownList>
		   </div>
		  </div>
         <div class="form-row">
		   <div class="form-group col-md-12"> 
		   	  <label for="inputAddress">Glosa<span style="color: #FF0000; font-weight: bold;"></span></label>
			    <asp:TextBox ID="TxtGlosa" runat="server" MaxLength="200"  CssClass="form-control"
               placeholder="Glosa"
             ></asp:TextBox>
		   </div>
		  </div>
         <div class="form-row">
		  
		   <div class="form-group col-md-6"> 
		   	  <label for="inputAddress">Cantidad<span style="color: #FF0000; font-weight: bold;"></span></label>
			  <div class="d-flex">
                             <asp:TextBox ID="TxtCantidad" runat="server"  MaxLength="10"
          CssClass="form-control" onkeypress="return soloLetras(event,'1234567890.')" ></asp:TextBox>
          <span class="validacion" id="valcantidad"  > * </span>

			  </div>
		   </div>

               <div class="form-group col-md-6"> 
		   	  <label for="inputAddress">Costo<span style="color: #FF0000; font-weight: bold;"></span></label>
			  <div class="d-flex">
    <asp:TextBox ID="TxtPrecio" runat="server"  placeholder="Valor"
                 MaxLength="10" 
               CssClass="form-control"
                onkeypress="return soloLetras(event,'1234567890.')" EnableViewState="False">00.00</asp:TextBox>
              <span class="validacion" id="valprecio"  > * </span>

			  </div>
		   </div>
		  </div>
         <div class="form-row">
		   <div class="col-md-12 d-flex justify-content-center"> 
                  <asp:Button ID="BtnAgregarItem" runat="server"  OnClientClick="return sendformItem();" OnClick="BtnAgregarItem_Click" Text="Agregar" 
                  CssClass="btn  btn-primary"
                 />
		   </div> 
		   </div>

                      <div class="form-row">
		  
		   <div class="form-group col-md-12"> 

          <asp:Repeater ID="TableProformas" runat="server"   onitemcommand="RemoverConceptos_ItemCommand">
                <HeaderTemplate>
                <table id="tablasort" cellspacing="0" cellpadding="1" class="table table-bordered invoice">
                <thead>
                <tr>
                    <th >#</th>
                    <th >Id.</th>
                    <th >Concepto</th>
                    <th >Cantidad</th>
                    <th >Valor $</th>
                    <th >Subtotal $</th>
                    <th >Remover</th>
                  </tr>
                </thead> 
                <tbody>
                </HeaderTemplate>
                <ItemTemplate>
                <tr class="point">
                <td ><asp:Label Text='<%#Eval("Line")%>' ID="LblEstados" runat="server" /></td>
                <td ><asp:Label Text='<%#Eval("Concepto_id")%>' ID="lblId" runat="server"  /></td>
                <td ><asp:Label Text='<%#Eval("Concepto_name")%>' ID="lblReference" runat="server" /></td>
                <td ><asp:Label Text='<%#Eval("Total_horas")%>' ID="lblName" runat="server" /></td>
               <td ><asp:Label Text='<%#Eval("Precio_hora")%>' ID="LblEta" runat="server" /></td>
               <td ><asp:Label Text='<%#Eval("Total")%>' ID="LblEtd" runat="server" /></td>
                  <td class="alinear" >
                  
                    <asp:Button ID="BtnRemover"  
                       CommandArgument='<%#Eval("Line")%>'
                       runat="server" Text="Remover" CssClass=" btn btn-secondary" ToolTip="Permite remover un concepto" />
                    
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
		   <div class="col-md-12 d-flex flex-row-reverse border border-primary"> 
               		      <asp:Label ID="LblTotalProforma" runat="server" Text="LblTotalProforma" Width="80px"></asp:Label>

  <label for="inputAddress">Total:&nbsp;<span style="color: #FF0000; font-weight: bold;"></span></label>

		   </div> 
		   </div>




        	   <div class="form-row">
		   <div class="col-md-12 d-flex justify-content-center"> 
                <div class="d-flex">
         <img alt="loading.." src="../shared/imgs/loader.gif" id="loader" class="nover"  />
         <asp:Button ID="BtnGrabar" runat="server" CssClass=" btn btn-primary  " OnClientClick="return sendform();"  OnClick="BtnGrabar_Click"  Text="Generar Proforma" /> &nbsp;
          <asp:Button ID="BtnNuevo" runat="server"   CssClass="btn btn-primary " OnClick="BtnNuevo_Click" Text="Nuevo" /> &nbsp;


                </div>
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
                    document.getElementById('<%=TxtReferencia.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:163px;";
                    document.getElementById("loader").className = 'nover';
                    return false;
              }
              var vals = document.getElementById('<%=TxtCantidad.ClientID %>').value;
                if (vals == '0' || vals == null || vals == undefined) {
                    alertify.alert('* Debe ingresar la cantidad *');
                    document.getElementById('<%=TxtCantidad.ClientID %>').focus();
                    document.getElementById('<%=TxtCantidad.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:120px;";
                    document.getElementById("loader").className = 'nover';
                    return false;
              }
                var vals = document.getElementById('<%=TxtPrecio.ClientID %>').value;
                if (vals == '0' || vals == null || vals == undefined) {
                    alertify.alert('* Debe ingresar el costo *');
                    document.getElementById('<%=TxtPrecio.ClientID %>').focus();
                    document.getElementById('<%=TxtPrecio.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:200px;";
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
                    document.getElementById('<%=TxtReferencia.ClientID %>').style.cssText = "background-color:#ffffc6;";
                    document.getElementById("loader").className = 'nover';
                    return false;
              }
            return true;
          }

          function sendformItem() {

           
              var vals = document.getElementById('<%=TxtCantidad.ClientID %>').value;
                if (vals == '0' || vals == null || vals == undefined || vals == '') {
                    alertify.alert('* Debe ingresar la cantidad *');
                    document.getElementById('<%=TxtCantidad.ClientID %>').focus();
                    document.getElementById('<%=TxtCantidad.ClientID %>').style.cssText = "background-color:#ffffc6;";
                    document.getElementById("loader").className = 'nover';
                    return false;
              }
                var vals = document.getElementById('<%=TxtPrecio.ClientID %>').value;
                if (vals == '0' || vals == null || vals == undefined || vals == '') {
                    alertify.alert('* Debe ingresar el costo *');
                    document.getElementById('<%=TxtPrecio.ClientID %>').focus();
                    document.getElementById('<%=TxtPrecio.ClientID %>').style.cssText = "background-color:#ffffc6;";
                    document.getElementById("loader").className = 'nover';
                    return false;
              }
               var vals = document.getElementById('<%=CboConcepto.ClientID %>').value;
              if (vals == null || vals == undefined) {
                alertify.alert('* Seleccionar el concepto *');
                   return false;        
                }

                return true;
          }

    </script>


</asp:Content>
