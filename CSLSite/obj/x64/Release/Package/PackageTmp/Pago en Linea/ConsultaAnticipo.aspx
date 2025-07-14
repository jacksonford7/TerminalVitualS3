<%@ Page Title="Consultar Anticipos" Language="C#" MasterPageFile="~/site.Master" AutoEventWireup="true" CodeBehind="~/Pago en Linea/ConsultaAnticipo.aspx.cs" Inherits="CSLSite.Pago_en_Linea.ConsultaAnticipo" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="placehead" runat="server">
     <link href="../shared/estilo/jquery.datetimepicker.css" rel="stylesheet" type="text/css" />
         <!--mensajes-->
        <link href="https://cdn.jsdelivr.net/npm/alertifyjs@1.11.0/build/css/alertify.min.css" rel="stylesheet"/>
        <script type="text/javascript" src="https://cdn.jsdelivr.net/npm/alertifyjs@1.11.0/build/alertify.min.js"></script>

     <!--Datatables-->
       <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/5.15.1/css/all.min.css" integrity="sha512-+4zCK9k+qNFUR5X+cKL9EIR+ZOhtIloNl9GIKS57V1MyNsYpYcUrUeQc9vNfzsWfV28IaLL3i96P9sdNyeRssA==" crossorigin="anonymous" />
       <link href="../css/datatables.min.css" rel="stylesheet" />
       <link rel="stylesheet" type="text/css" href="..js/buttons/css1.6.4/buttons.dataTables.min.css"/>
        <script src="../lib/jquery/jquery.min.js" type="text/javascript"></script>
       <script type="text/javascript"  src="../js/datatables.js"></script>
       <script src="../Scripts/table_catalog.js" type="text/javascript"></script>  
       <script type="text/javascript" src="../js/bootstrap.bundle.min.js"></script>
    <!--Fin-->
    <style type="text/css">
        
  #progressBackgroundFilter {
    position:fixed;
    bottom:0px;
    right:0px;
    overflow:hidden;
    z-index:1000;
    top: 0;
    left: 0;
    background-color: #CCC;
    opacity: 0.8;
    filter: alpha(opacity=80);
    text-align:center;
    
}
#processMessage 
{
    text-align:center;
    position:fixed;
    top:30%;
    left:43%;
    z-index:1001;
    border: 5px solid #67CFF5;
    width: 200px;
    height: 100px;
    background-color: White;
    padding:0;
}
    
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="placebody" runat="server">
    <asp:ToolkitScriptManager ID="tkcal" EnableScriptGlobalization="true" runat="server"> </asp:ToolkitScriptManager>
      <input id="zonaid" type="hidden" value="8" />
        <input id="nombreCliente" type="hidden" runat="server" clientidmode="Static"/>
        <input id="idCliente" type="hidden" runat="server" clientidmode="Static"/>
        <input id="rolCliente" type="hidden" runat="server" clientidmode="Static"/>
        <input id="userName" type="hidden" runat="server" clientidmode="Static"/>
        <input id="codigoAnticipo" type="hidden" runat="server" clientidmode="Static"/>
        <asp:Button ID="BtnAnularOculto" runat="server"  OnClick="AnularAnticipo" style="visibility: hidden; display: none;" />

     <div class="mt-4"> 
    <nav class="mt-4" aria-label="breadcrumb">
          <ol class="breadcrumb">
            <li class="breadcrumb-item" id="opcion_principal" runat="server"><a href="#">Gestión financiera</a></li>
             <li class="breadcrumb-item active" aria-current="page" id="sub_opcion" runat="server">Generación y consulta de anticipos</li>
          </ol>
        </nav>
      </div>
       <div class="dashboard-container p-4" id="cuerpo" runat="server">

             <div class="form-row">
		  
		   <div class="form-group col-md-12"> 
                           <div class=" alert alert-warning">
    <span id="dtlo" runat="server">Estimado usuario:</span> 
    <br /> Asegúrese que toda la información que agrega a este documento es correcta antes de proceder a su respectivo proceso, si desea confirmar alguna información antes de proceder comuníquese con nuestro departamento finaciero a los teléfonos: +593 (04) 6006300, 
                 ext. 8016, 8017, 8019, 8044, 8045. 
    </div>
		   </div>
		  </div>


         <div class="form-title">Datos del documento buscado</div>
		 
		  <div class="form-row">
		  
		   <div class="form-group col-md-4"> 
                     <asp:Label for="inputAddress" ID="Label1" runat="server">Monto </asp:Label><span style="color: #FF0000; font-weight: bold;"></span>
               <div class="d-flex">
                                   <asp:TextBox ID="TextBox1" CssClass="form-control" ValidationGroup="Ingreso" runat="server" onkeypress="return soloLetras(event,'01234567890./')"></asp:TextBox>
                      <asp:RegularExpressionValidator ID="RegularExpressionValidator1" ControlToValidate="TextBox1" ValidationGroup="Ingreso"
                                                  runat="server" ErrorMessage="*" ForeColor="Red" Display="Dynamic" 
                                                  ValidationExpression="(?!^0*$)(?!^0*\.0*$)^\d{1,18}(\.\d{1,2})?$" ValidateEmptyText="false">
                                                  </asp:RegularExpressionValidator>
                   <asp:RequiredFieldValidator 
                    ControlToValidate="TextBox1" Display="Dynamic" 
                    Text="*"
                    ErrorMessage="*" 
                    runat="Server"  ForeColor="Red" ValidationGroup="Ingreso"
                    ID="TextRequiredFieldValidator" />
               </div>
		   </div>

                 <div class="form-group col-md-4"> 
                     <asp:Label for="inputAddress" ID="Label2" runat="server" >Booking <span style="color: #FF0000; font-weight: bold;"></span></asp:Label>
                   <asp:TextBox ID="TextBox4" CssClass="form-control" runat="server"></asp:TextBox>
		   </div>
		  
              	   <div class="col-md-4"> 
                                <asp:Button ID="Button1"  CssClass="btn btn-primary"
                                    runat="server" Text="Registrar" OnClick="GrabarAnticipo" ValidationGroup="Ingreso"/>

		   </div> 
          </div>


           <div class="form-row">
                  <div class="form-group col-md-2"> 
                                                  <asp:TextBox ID="TextBox3" CssClass="form-control" runat="server"  MaxLength="19"  ClientIDMode="Static" BorderColor="Transparent" Enabled="false" ReadOnly="true"></asp:TextBox>      

                 </div>
                  <div class="form-group col-md-2"> 
                                <asp:TextBox ID="TextBox2" CssClass="form-control" runat="server"  MaxLength="19"  ClientIDMode="Static" onkeypress="return soloLetras(event,'01234567890/')"></asp:TextBox>      

                 </div>
                  <div class="form-group col-md-2"> 
                                        <asp:TextBox ID="desded" runat="server"  MaxLength="10" CssClass="datetimepicker form-control"
             onkeypress="return soloLetras(event,'01234567890/')" 
                  ClientIDMode="Static"></asp:TextBox>

                 </div>
                  <div class="form-group col-md-2"> 
                                  
                  <asp:TextBox ID="hastad" runat="server" ClientIDMode="Static" 
                 MaxLength="15" CssClass="datetimepicker form-control"
             onkeypress="return soloLetras(event,'01234567890/')" 
                 ></asp:TextBox>
                 </div>
                  <div class="form-group col-md-3"> 
                      <asp:ComboBox ID="ComboBox1" runat="server" DropDownStyle ="DropDownList" CssClass="">
                            <asp:ListItem Text="Todos" Value="0" />
                            <asp:ListItem Text="Pendiente" Value="1" />
                            <asp:ListItem Text="Confirmado" Value="2" />
                            <asp:ListItem Text="Aplicado" Value="2" />
                      </asp:ComboBox>


  
                 </div>
                  <div class="form-group col-md-1">
                      <div class="d-flex">
                                <asp:Button ID="btbuscar" runat="server"
                 Text="Buscar" CssClass="btn btn-primary"
                 onclick="Buscar" UseSubmitBehavior="false"/>

                            <span id="imagen"></span>
                          </div>
                 </div>
		  </div>

            <div class="form-row">
                <div class="form-group col-md-12"> 
                             <div class="cataresult" >
               <asp:UpdatePanel ID="upresult" runat="server" ChildrenAsTriggers="true">
                     <ContentTemplate>
                                 <script type="text/javascript">
                                     Sys.Application.add_load(BindFunctions); 
                      </script>
             <div id="xfinder" runat="server" visible="false" >
             <div class="findresult" >
             <div class="booking" >
                  <div class=" card-title">Documentos encontrados</div>
                 <div class="bokindetalle">
                 <asp:Repeater ID="tablePagination" runat="server" 
                         onitemcommand="tablePagination_ItemCommand" >
                 <HeaderTemplate>
                 <table id="tablasort" cellspacing="1" cellpadding="1" class=" table table-bordered table-sm table-contecon">
                 <thead>
                 <tr>
                 <th>#</th>
                 <th>Monto</th>
                 <th>Numero Liquidación</th>
                 <th>Fecha Registro</th>
                 <th>Estado</th>
                 <th colspan="3">Acciones</th>
                 
                 </tr>
                 </thead> 
                 <tbody>
                 </HeaderTemplate>
                 <ItemTemplate>
                 <tr class="point" >
                  <td ><%#Eval("ITEM")%></td>
                  <td ><%#Eval("MONTO_TOTAL")%></td>
                  <td><%#Eval("NUMERO_LIQUIDACION")%></td>
                  <td  class="nover"><%#Eval("CODIGO_ANTICIPO")%></td>
                  <td ><%#DataBinder.Eval(Container.DataItem, "FECHA_REGISTRO", "{0:dd-MM-yyyy HH:mm}")%></td>
                  <td><%#Eval("ESTADO")%></td>
                  <td >
                   <div class="tcomand">
                       <a href="../Pago En Linea/ImprimirAnticipo.aspx?sid=<%# securetext(Eval("NUMERO_LIQUIDACION")) %>" class=" btn btn-link" target="_blank">Detalle</a>
                       <a href="../Pago En Linea/ImprimirFacturasPagadas.aspx?sid=<%# securetext(Eval("CODIGO_ANTICIPO")) %>" class="btn btn-link" target="_blank">Pagos</a>
                   </div>
                  </td>
                  <td >
                   <div>
                        <input id="BtnAnular"  class="btn  btn-secondary"
                            type="button" value="Anular" 
                            onclick="AnularAnticipo(<%#Eval("CODIGO_ANTICIPO")%>)" />
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
             </div>
             </div>

              </div>
               <div id="sinresultado" runat="server" class="  alert alert-primary"></div>
                  </ContentTemplate>
                     <Triggers>
                     <asp:AsyncPostBackTrigger ControlID="btbuscar" />
                     </Triggers>
                 </asp:UpdatePanel>
             </div>

                </div>
                </div>
		 
     </div>



       

    
     

     
  
    <script src="../Scripts/pages.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.datetimepicker.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.tablesorter.min.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.tablesorter.pager.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $('.datetimepicker').datetimepicker().datetimepicker({ lang: 'es', timepicker: false, closeOnDateSelect: true, format: 'd/m/Y' });


        });
        $("#BtnAnular").click(function () {
            //document.getElementById('codigoAnticipo').value = facturas;
        });
        $(window).load(function () {
            var table = document.getElementById("tablasort");
            for (var i = 1, row; row = table.rows[i]; i++) {
                if (row.cells[5].innerText != "PENDIENTE") {
                    row.cells[7].children['0'].children.BtnAnular.disabled = true;
                }
            }
        });
        function popupCallback(objeto, catalogo) {
            if (objeto == null || objeto == undefined) {
             alertify.alert('Hubo un problema al setear un objeto de catalogo');
                return;
            }
            //si catalogos es booking
            if (catalogo == 'bk') {
                document.getElementById('numbook').textContent = objeto.boking;
                document.getElementById('nbrboo').value = objeto.boking;
                return;
            }
        }
        function AnularAnticipo(codigo) {
            document.getElementById("codigoAnticipo").value = codigo;
           // alert(document.getElementById("BtnAnularOculto").textContent);
            $("#<%= BtnAnularOculto.ClientID %>").click();
        }
        function clear() {
            document.getElementById('numbook').textContent = '...';
            document.getElementById('nbrboo').value = '';
        }

        function soloLetras(e, caracteres, espacios) {

            key = e.keyCode || e.which;
            tecla = String.fromCharCode(key).toLowerCase();
            if (caracteres) {
                letras = caracteres;
            }
            else {
                letras = " áéíóúabcdefghijklmnñopqrstuvwxyz";
            }
            if (espacios == undefined || espacios == null) {
                especiales = [8, 13, 32, 9, 16, 20];
            }
            else {
                especiales = [8, 13, 9, 16, 20];
            }
            tecla_especial = false
            for (var i in especiales) {
                if (key == especiales[i]) {
                    tecla_especial = true;
                    break;
                }
            }
            if (letras.indexOf(tecla) == -1 && !tecla_especial) {
                return false;
            }
        }
  </script>

  <asp:updateprogress associatedupdatepanelid="upresult"
    id="updateProgress" runat="server">
    <progresstemplate>
        <div id="progressBackgroundFilter"></div>
        <div id="processMessage"> Estamos procesando la tarea que solicitó, por favor espere...<br />
             <img alt="Loading" src="../shared/imgs/loader.gif" style="margin:0 auto;" />
        </div>
    </progresstemplate>
</asp:updateprogress>
  </asp:Content>

