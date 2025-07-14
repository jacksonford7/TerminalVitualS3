<%@ Page Title="Consultar Booking" Language="C#" MasterPageFile="~/site.Master"
    AutoEventWireup="true" CodeBehind="consulta_booking.aspx.cs" Inherits="CSLSite.consultabook" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="placehead" runat="server">
    
    
    <link href="../shared/estilo/tables.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/work.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/jquery.datetimepicker.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/info.css" rel="stylesheet" type="text/css" />
   
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
           <div class="mt-4">         
        <nav class="mt-4" aria-label="breadcrumb">
          <ol class="breadcrumb">
            <li class="breadcrumb-item" id="opcion_principal" runat="server"><a href="#">Exportación</a></li>
             <li class="breadcrumb-item active" aria-current="page" id="sub_opcion" runat="server">Consulta de reservas bookings (reserva)</li>
          </ol>
        </nav>
      </div>

         <div class="dashboard-container p-4" id="cuerpo" runat="server">

         <div class="form-title">Datos del documento buscado</div>
		              <div class="form-row">
		   <div class="form-group col-md-6"> 
		   	  <label for="inputAddress">Numero<span style="color: #FF0000; font-weight: bold;"></span></label>
			               <asp:TextBox ID="bonum" runat="server"  MaxLength="15"  ClientIDMode="Static"
                                CssClass="form-control"
             onkeypress="return soloLetras(event,'01234567890abcdefghijklmnopqrstuvwxyz_-',true)"></asp:TextBox>
		   </div>
           <div class="form-group col-md-6"> 
		   	  <label for="inputAddress">Tipo<span style="color: #FF0000; font-weight: bold;"></span></label>
			  <div class="d-flex">
                     <asp:DropDownList ClientIDMode="Static" ID="dptipo"  CssClass="form-control"
                         runat="server" 
                 onchange="valdpme(this,0,valran);" >
                  <asp:ListItem Value="0" Selected="True">Selecione Tipo</asp:ListItem>
                  <asp:ListItem Value="FCL">Lleno (FCL)</asp:ListItem>
                  <asp:ListItem Value="MTY">Vacio (MTY)</asp:ListItem>
                  <asp:ListItem Value="LCL">Consolidacion (LCL)</asp:ListItem>
                  <asp:ListItem Value="BBK">Carga General (BBK)</asp:ListItem>
              </asp:DropDownList>
                  <span id="valran" class="validacion">*</span>
			  </div>
		   </div>
		  </div>
             		  <div class="form-row">
		  
		   <div class="form-group col-md-6"> 
		   	  <label for="inputAddress">Desde<span style="color: #FF0000; font-weight: bold;"></span></label>
			 
                  <asp:TextBox ID="desded" runat="server"  MaxLength="10" CssClass="datetimepicker form-control"
             onkeypress="return soloLetras(event,'01234567890/')" 
                 onblur="valDate(this,true,valdate);" ClientIDMode="Static"></asp:TextBox>

		   </div>

                           	   <div class="form-group col-md-6"> 
		   	  <label for="inputAddress">Hasta<span style="color: #FF0000; font-weight: bold;"></span></label>
			  <div class="d-flex">

                               <asp:TextBox ID="hastad" runat="server" 
                                   ClientIDMode="Static"  MaxLength="15" CssClass="datetimepicker form-control"
             onkeypress="return soloLetras(event,'01234567890/')" 
                  onblur="valDate(this,true,valdate);"></asp:TextBox>

   <span id="valdate" class="validacion">*</span>
			  </div>
		   </div>
		  </div>
               <div class="row">
		   <div class="col-md-12 d-flex justify-content-center">
                <span id="imagen"></span>
             <asp:Button ID="btbuscar" runat="server" Text="Iniciar la búsqueda"  
                 onclick="btbuscar_Click" CssClass="btn btn-primary"
                 OnClientClick="return getprocesa();" />
		   </div> 
		   </div>

              <div class="cataresult" >
               <asp:UpdatePanel ID="upresult" runat="server" ChildrenAsTriggers="true">
                     <ContentTemplate>
                        <script type="text/javascript">
                            Sys.Application.add_load(BindFunctions); 
                      </script>
             <div id="xfinder" runat="server" visible="false" >
             <div class=" alert alert-warning" id="alerta" runat="server" >
Confirme que los datos ingresados sean correctos.  En caso de error, por favor notifíquelo a las casilla ec.sac@contecon.com.ec o comuníquese a los teléfonos (04) 6006300 – 3901700 opción 4	 
             </div>
          
        
                  <div class="form-title">Documentos encontrados</div>
                 
                 <asp:Repeater ID="tablePagination" runat="server" 
                        >
                 <HeaderTemplate>
                 <table id="tablasort" cellspacing="1" cellpadding="1" class="table table-bordered table-sm table-contecon">
                 <thead>
                 <tr>
                
                 <th>ID</th>
                 <th>Numero</th>
                 <th>Tipo</th>
                 <th>Reefer</th>
                 <th>Cantidad</th>
                 <th>Linea</th>
                
                 <th>Buque</th>
             
                 <th>Estado (ATA)</th>
                
                 <th>Acciones</th>
                 </tr>
                 </thead> 
                 <tbody>
                 </HeaderTemplate>
                 <ItemTemplate>
                 <tr class="point" >
                  
                  <td><%#Eval("id")%></td>
                  <td><%#Eval("numero")%></td>
                  <td><%#Eval("tipo")%></td>
                   <td><%# TipoB(Eval("frio")) %></td>
                  <td><%#Eval("cantidad")%></td>
                  <td><%#Eval("linea")%></td>
                  <td><%#Eval("buque")%></td>
                  <td  ><%# NaveEstado(Eval("ata"))%></td>
                  <td>
                   <div class="tcomand">
                       <a href="#" onclick="ClickUrl('<%# securetext(Eval("numero")) %>');" class=" btn btn-link" >Detalles</a>
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
               <div id="sinresultado" runat="server" class="msg-info" clientidmode="Static"></div>
                  </ContentTemplate>
                     <Triggers>
                     <asp:AsyncPostBackTrigger ControlID="btbuscar" />
                     </Triggers>
                 </asp:UpdatePanel>
             </div>

		  </div>


    <script src="../Scripts/pages.js" type="text/javascript"></script>
     <script src="../Scripts/jquery.datetimepicker.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
              $('.datetimepicker').datetimepicker().datetimepicker({ lang: 'es', timepicker: false, closeOnDateSelect: true, format: 'd/m/Y' });
        });
        function getprocesa() {
            var i = document.getElementById('bonum');
            if (i == null || i.value == '0' || i.value == '') {
               setMsg('Escriba una o varias letras del número de reserva');
                return false;
            }
            i = document.getElementById('dptipo');
            if (i == null || i.value == '0' || i.value == '') {
                
                 setMsg('Seleccione el tipo de reserva');
                return false;
            }
            i = document.getElementById('desded');
            if (i == null || i.value == '0' || i.value == '') {
                setMsg('Escriba la fecha desde');
                return false;
            }
            i = document.getElementById('hastad');
            if (i == null || i.value == '0' || i.value == '') {
                setMsg('Escriba la fecha hasta');
                return false;
            }
            return true;
        }
        function setMsg(mens) {
            var m = document.getElementById('sinresultado');
            if (m != null) {

                m.setAttribute("class", "");
                m.textContent = '';
            }
            var i = document.getElementById('sinresultado');
          //  alert(i);
            if (i != null) {
                i.setAttribute("class", "");
                i.setAttribute("class", "msg-critico");
               
                i.textContent = mens;
            }
        }
        function ClickUrl(ca) {
            var url = 'bookingdetalle.aspx?sid=' + ca;
            var leftPos = 200;// screen.width -50;
            window.open(url, "Booking", "width=850, height=600, top=40, left=" + leftPos);
        }
  </script>

  <asp:updateprogress associatedupdatepanelid="upresult"
    id="updateProgress" runat="server">
    <progresstemplate>
        <div id="progressBackgroundFilter"></div>
        <div id="processMessage"> Estamos procesando la tarea que solicitó, por favor 
            espere...<br />
             <img alt="Loading" src="../shared/imgs/loader.gif" style="margin:0 auto;" />
        </div>
    </progresstemplate>
</asp:updateprogress>


 
  </asp:Content>
