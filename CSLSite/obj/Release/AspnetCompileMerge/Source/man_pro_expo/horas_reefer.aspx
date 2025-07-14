<%@ Page Title="Horas Reefer" Language="C#" MasterPageFile="~/site.Master" AutoEventWireup="true" CodeBehind="horas_reefer.aspx.cs" Inherits="CSLSite.horas_reefer" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="placehead" runat="server">
       
      <link href="../shared/estilo/jquery.datetimepicker.css" rel="stylesheet" type="text/css" />
    <link href="../img/favicon2.png" rel="icon"/>
  <link href="../img/icono.png" rel="apple-touch-icon"/>
  <link href="../css/bootstrap.min.css" rel="stylesheet"/>
  <link href="../css/dashboard.css" rel="stylesheet"/>
  <link href="../css/icons.css" rel="stylesheet"/>
  <link href="../css/style.css" rel="stylesheet"/>
  <link href="https://fonts.googleapis.com/css2?family=Roboto:wght@300;400;500;700&display=swap" rel="stylesheet"/>
       
    
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

    <script src="../Scripts/turnos.js" type="text/javascript"></script>

    <style type="text/css">

        .warning { background-color:Yellow;  color:Red;}

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
            <li class="breadcrumb-item" id="opcion_principal" runat="server"><a href="#">Gestión financiera</a></li>
             <li class="breadcrumb-item active" aria-current="page" id="sub_opcion" runat="server">Mantenimiento horas reefer</li>
          </ol>
        </nav>
      </div>
     <input id="agencia" type="hidden" runat="server"  clientidmode="Static"/>
 <asp:HiddenField runat="server" ID="hfBusqueda" /> 
    <div class="dashboard-container p-4" id="cuerpo" runat="server">
         <div class="form-title">DATOS PARA REGISTRO DE HORAS REEFER QUE ASUME LA LINEA</div>
		  <div class="form-row">
		   <div class="form-group col-md-12">
               
               
               
               <asp:UpdatePanel ID="upresult" runat="server" ChildrenAsTriggers="true">
                     <ContentTemplate>
                       <input id="idlin" type="hidden" runat="server" clientidmode="Static" />
                       <input id="diponible" type="hidden" runat="server" clientidmode="Static" />

                 <asp:Repeater ID="tablePagination" runat="server" >
                 <HeaderTemplate>
                  <table id="tablasort" class="table table-bordered table-sm  table-contecon">
                     <thead>
                 <tr>
                 <th >Linea</th>
                 <th >Cantidad de Horas</th>
                 <th >Fecha de Vigencia</th>
                 <th >Acciones</th>
                 </tr>
              
                 </thead>
                 <tbody>
                 </HeaderTemplate>
                 <ItemTemplate>
                 <tr class="point" onclick="setObject(this);">
                  <td ><%#Eval("SAHR_LINEA")%></td>
                  <td ><%#Eval("SERE_HORAS")%></td>
                  <td ><%#Eval("SERE_FECHAVIGENCIA")%></td>
                  <td >
                     <a href="#"  class="btn btn-link" >Elegir</a>
                  </td>
                 </tr>
                  </ItemTemplate>
                 <FooterTemplate>
                 </tbody>
                 </table>
                 </FooterTemplate>
         </asp:Repeater>

         

            
            
            
               <div id="sinresultado" runat="server" class=" alert-primary"></div>
                  </ContentTemplate>
                     <Triggers>
               
                     </Triggers>
                 </asp:UpdatePanel>
            
		   </div>
		  </div>
          <div class="form-row">
		  
		   <div class="form-group col-md-4"> 
		   	  <label for="inputAddress">Linea<span style="color: #FF0000; font-weight: bold;"></span></label>
			  <div class="d-flex">
                                     <span id="lineanav" runat="server" clientidmode="Static" class=" form-control col-md-12" >...</span>
                   <input id="xlineanav" type="hidden" value="" runat="server" clientidmode="Static"/>
                       <span id="vallineanav" class="validacion" > * </span>

			  </div>
		   </div>
		   <div class="form-group col-md-4"> 
		   	  <label for="inputAddress">Horas (Cantidad)<span style="color: #FF0000; font-weight: bold;"></span></label>
			  <div class="d-flex">
                                <span id="ncantidadhoras" runat="server" clientidmode="Static" class="form-control col-md-12"    >...</span>
                   <input id="xcantidadhoras" type="hidden" value="" runat="server" clientidmode="Static" />
         <span id="valcanthorasreefer" class="validacion" > * </span>


			  </div>
		   </div>
		   <div class="form-group col-md-4"> 
		   	  <label for="inputAddress">Fecha de Vigencia<span style="color: #FF0000; font-weight: bold;"></span></label>
			  <div class="d-flex">
                     <span id="fechavigencia" runat="server" clientidmode="Static" class="form-control col-md-12" >...</span>
                   <input id="xfechavigencia" type="hidden" value="" runat="server" clientidmode="Static" />
                   <span id="valoldfecvigencia" class="validacion" > * </span>


			  </div>
		   </div>
		  </div>
          <div class="form-row">
		  
		   <div class="form-group col-md-4"> 
		   	  <label for="inputAddress">Nueva cantidad (Horas)<span style="color: #FF0000; font-weight: bold;"></span></label>
			  <div class="d-flex">
                            <asp:TextBox 
              onblur="checkcaja(this,'valcanthoras',true);"
             ID="txtcanthoras" runat="server"  MaxLength="3"
             onkeypress="return soloLetras(event,'0123456789')" 
                  CssClass=" form-control"
             ></asp:TextBox>
           <span id="valcanthoras" class="validacion" > * </span>

			  </div>
		   </div>
                 
	        <div class="form-group col-md-4"> 
                <label for="inputAddress">Asume todo?<span style="color: #FF0000; font-weight: bold;"></span></label>
                                          
                 <div class="d-flex">
                <asp:CheckBox  CssClass=" form-control"  Font-Bold="false"  ID="chkAsumeTodo" runat="server" onchange="fValidaAsumeTodo()"/>
                     </div>
	       </div>
	    


	
		  
		   <div class="form-group col-md-4"> 
		   	  <label for="inputAddress">Nueva Fecha de Vigencia<span style="color: #FF0000; font-weight: bold;"></span></label>
			  <div class="d-flex">
                               <asp:TextBox 
             style="text-align: center"  onblur="checkcaja(this,'valfecvig',true);"
             ID="txtfecvigencia" runat="server"  MaxLength="15" 
                 CssClass="datepicker form-control"
             onkeypress="return soloLetras(event,'0123456789/')" 
             ></asp:TextBox>
              <span id="valfecvig" class="validacion" > * </span>


			  </div>
		   </div>
		  </div>
          <div class="form-row">
		   <div class="col-md-12 d-flex justify-content-center" runat="server" id="btnera"> 
		     <div class="d-flex">
                           
              <asp:Button ID="btsalvar"  CssClass="btn btn-primary"
                  runat="server" Text="Procesar Horas Reefer"  onclick="btsalvar_Click" 
                   OnClientClick="return prepareObject('¿Esta seguro de procesar las Horas Reefer?')"
                   ToolTip="Procesar Horas Reefer."/>
                  <img alt="loading.." src="../shared/imgs/loader.gif" id="loader" class="nover"  />


		     </div>
		   </div> 
		   </div>
     </div>
    <script src="../Scripts/pages.js" type="text/javascript"></script>
    <script src="../Scripts/credenciales.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.datetimepicker.js" type="text/javascript"></script>



    <script type="text/javascript">

       
        $(document).ready(function () {
            $('.datepicker').datetimepicker().datetimepicker({ lang: 'es', timepicker: false, closeOnDateSelect: true, format: 'd/m/Y' });
            BindFunctions();
        });


        var programacion = {};
        var lista = [];
         function getGifOculta(mensaje) { alert(mensaje);}
        function popupCallback(objeto) {
            if (objeto == null || objeto == undefined) {
                alertify.alert('¡ Hubo un problema al setaar un objeto de catalogo.');
                return;
            }
            document.getElementById('lineanav').textContent = objeto.linea;
            document.getElementById('ncantidadhoras').textContent = objeto.cant_horas;
            document.getElementById('fechavigencia').textContent = objeto.fecha_vigencia;
            document.getElementById('xlineanav').value = objeto.linea;
            document.getElementById('xcantidadhoras').value = objeto.cant_horas;
            document.getElementById('xfechavigencia').value = objeto.fecha_vigencia;
            document.getElementById('vallineanav').textContent = "";
            document.getElementById('valcanthorasreefer').textContent = "";
            document.getElementById('valoldfecvigencia').textContent = "";
            return;
        }

        function prepareObject(mensaje) {
            try {
                if (confirm(mensaje) == false) {
                    return false;
                }

               
                document.getElementById("loader").className = '';
                lista = [];
                //validaciones básicas
                var vals = document.getElementById('xlineanav');
                if (vals == null || vals == undefined || vals.value == '') {
                    alertify.alert('¡ Consulte la Linea Naviera para Asumir la Nueva Cantidad de Horas Reefer y Fecha de Vegencia.');
                    document.getElementById("loader").className = 'nover';
                    return false;
                }
                 
                
              var asume = document.getElementById('<%=chkAsumeTodo.ClientID %>').checked;
                if (!asume) {
                    var vals = document.getElementById('<%=txtcanthoras.ClientID %>');
                    if (vals == null || vals == undefined || vals.value == '') {
                        alertify.alert('¡ Digite la Nueva Cantidad de Horas Reefer que Asume la Linea.');
                        document.getElementById('<%=txtcanthoras.ClientID %>').focus();
                        document.getElementById("loader").className = 'nover';
                        return false;
                    }
                }

               

               var vals = document.getElementById('<%=txtfecvigencia.ClientID %>');
                if (vals == null || vals == undefined || vals.value == '') {
                    alertify.alert('¡ Seleccione la Nueva Fecha de Vigencia que Asume la Linea.');
                   document.getElementById('<%=txtfecvigencia.ClientID %>').focus();
                    document.getElementById("loader").className = 'nover';
                    return false;
                }
                 

              
                return true;
            } catch (e) {
                alertify.alert('Lamentamos comunicarle que ha ocurrido el siguiente problema:\n' + e.Message);
            }
        }

        function fValidaAsumeTodo() {
            var asume = document.getElementById('<%=chkAsumeTodo.ClientID %>').checked;
            if (asume) {
                document.getElementById('<%=txtcanthoras.ClientID %>').value = "";
                document.getElementById('<%=txtcanthoras.ClientID %>').disabled = true;
                document.getElementById('valcanthoras').textContent = "";
            }
            else {
                document.getElementById('<%=txtcanthoras.ClientID %>').disabled = false;
                document.getElementById('<%=txtcanthoras.ClientID %>').focus();
                document.getElementById('valcanthoras').textContent = "*";
            }
        }
        function setObject(row) {
            var celColect = row.getElementsByTagName('td');
            var catalogo = {
                linea: celColect[0].textContent,
                cant_horas: celColect[1].textContent,
                fecha_vigencia: celColect[2].textContent
            };
            popupCallback(catalogo);
        }
    </script>
<%--<asp:updateprogress  id="updateProgress" runat="server">
    <progresstemplate>
        <div id="progressBackgroundFilter"></div>
        <div id="processMessage"> Estamos procesando la tarea que solicitó, por favor  espere...<br />
         <img alt="Loading" src="../shared/imgs/loader.gif" style="margin:0 auto;" />
        </div>
    </progresstemplate>
</asp:updateprogress>--%>
  </asp:Content>
