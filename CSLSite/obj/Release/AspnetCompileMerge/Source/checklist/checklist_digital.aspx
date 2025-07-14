<%@ Page Title="" Language="C#" MasterPageFile="~/site.Master" AutoEventWireup="true" CodeBehind="checklist_digital.aspx.cs" Inherits="CSLSite.checklist_digital" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="placehead" runat="server">

   <link href="../img/favicon2.png" rel="icon"/>
  <link href="../img/icono.png" rel="apple-touch-icon"/>
 <!-- Bootstrap core CSS -->
  <link href="../css/bootstrap.min.css" rel="stylesheet"/>
  <link href="../css/dashboard.css" rel="stylesheet"/>
  <link href="../css/icons.css" rel="stylesheet"/>
  <link href="../css/style.css" rel="stylesheet"/>

   <link href="https://fonts.googleapis.com/css2?family=Roboto:wght@300;400;500;700&display=swap" rel="stylesheet"/>

  <!--external css-->
  <link href="../shared/estilo/jquery.datetimepicker.css" rel="stylesheet" type="text/css" />
  <script src="../Scripts/jquery.datetimepicker.js" type="text/javascript"></script>


 <link href="../lib/advanced-datatable/css/demo_page.css" rel="stylesheet" />
  <link href="../lib/advanced-datatable/css/demo_table.css" rel="stylesheet" />
  <link rel="stylesheet" href="../lib/advanced-datatable/css/DT_bootstrap2.css" />

 

       
 
<script type="text/javascript">


 function BindFunctions() {

     $(document).ready(function() {
      /*
       * Insert a 'details' column to the table
       */
      var nCloneTh = document.createElement('th');
      var nCloneTd = document.createElement('td');
      nCloneTd.innerHTML = '<img src="../lib/advanced-datatable/images/details_open.png">';
      nCloneTd.className = "center";

      $('#<%= tablePagination.ClientID %> thead tr').each(function() {
        //this.insertBefore(nCloneTh, this.childNodes[0]);
      });

      $('#<%= tablePagination.ClientID %> tbody tr').each(function() {
        //this.insertBefore(nCloneTd.cloneNode(true), this.childNodes[0]);
      });

      /*
       * Initialse DataTables, with no sorting on the 'details' column
       */
      var oTable = $('#<%= tablePagination.ClientID %>').dataTable({
        "aoColumnDefs": [{
          "bSortable": false,
          "aTargets": [0]
        }],
        "aaSorting": [
          [1, 'asc']
        ]
      });

      /* Add event listener for opening and closing details
       * Note that the indicator for showing which row is open is not controlled by DataTables,
       * rather it is done here
       */
      $('#<%= tablePagination.ClientID %> tbody td img').live('click', function() {
        var nTr = $(this).parents('tr')[0];
        if (oTable.fnIsOpen(nTr)) {
          /* This row is already open - close it */
          this.src = "../lib/advanced-datatable/media/images/details_open.png";
          oTable.fnClose(nTr);
        } else {
          /* Open this row */
          this.src = "../lib/advanced-datatable/images/details_close.png";
          oTable.fnOpen(nTr, fnFormatDetails(oTable, nTr), 'details');
        }
      });
        });
    }
</script>

 <script type="text/javascript">
        $(document).ready(function () {
            $('#<%= tablePagination.ClientID %>').dataTable();
        });
       
    </script>

 
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="placebody" runat="server" >

   <asp:ToolkitScriptManager ID="tkscata" runat="server"></asp:ToolkitScriptManager>

  <div id="div_BrowserWindowName" style="visibility:hidden;">
    <asp:HiddenField ID="hf_BrowserWindowName" runat="server" />
     
  </div>
    <asp:HiddenField ID="manualHide" runat="server" />
     <input id="ID_NOVEDAD" type="hidden" value="" runat="server" clientidmode="Static" />
    <input id="NOVEDAD" type="hidden" value="" runat="server" clientidmode="Static" />

    <div class="mt-4">         
        <nav class="mt-4" aria-label="breadcrumb">
          <ol class="breadcrumb">
            <li class="breadcrumb-item" id="opcion_principal" runat="server"><a href="#">Otros</a></li>
            <li class="breadcrumb-item active" aria-current="page" id="sub_opcion" runat="server">CHECK LIST</li>
          </ol>
        </nav>
      </div>
<div class="dashboard-container p-4" id="cuerpo" runat="server">
    <div class="form-title">
           CHECK LIST
     </div>
       
      <asp:UpdatePanel ID="UPCARGA" runat="server"  UpdateMode="Conditional" ChildrenAsTriggers="true">
       <ContentTemplate>
        <div class="form-row"> 
           <div class="form-group col-md-6">
              <label for="inputZip">TIPO EQUIPO<span style="color: #FF0000; font-weight: bold;">*</span></label>
                <asp:UpdatePanel ID="UpTipoEquipo" runat="server"  UpdateMode="Conditional" ChildrenAsTriggers="true">
                    <ContentTemplate>
                 <asp:DropDownList runat="server" ID="CboTipoEquipo"    AutoPostBack="true"  class="form-control" OnSelectedIndexChanged="CboTipoEquipo_SelectedIndexChanged" >
                        </asp:DropDownList>
                        </ContentTemplate> 
                        <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="CboTipoEquipo" />
                    </Triggers>
                    </asp:UpdatePanel> 
              
            </div>
             <div class="form-group col-md-6">
                  <label for="inputZip">EQUIPO<span style="color: #FF0000; font-weight: bold;">*</span></label>
                  <asp:DropDownList runat="server" ID="CboEquipo"    AutoPostBack="false"  class="form-control"  >
                        </asp:DropDownList>         
             </div>
           
       </div>

        </ContentTemplate>
            <Triggers>
            <asp:AsyncPostBackTrigger ControlID="CboTipoEquipo" />
        </Triggers>
        </asp:UpdatePanel>

     <div class="form-row"> 
            <div class="form-group col-md-6">
                <label for="inputZip">OPERADOR<span style="color: #FF0000; font-weight: bold;">*</span></label>
                 <asp:TextBox ID="TxtOperador" runat="server" class="form-control" 
                    onkeypress="return soloLetras(event,'abcdefghijklmnopqrstuñvwxyz1234567890 -',true)" placeholder="DIGITE EL NOMBRE DEL OPERADOR" MaxLength="150"></asp:TextBox>
            </div>

            <div class="form-group col-md-2">
                 <label for="inputZip">FECHA<span style="color: #FF0000; font-weight: bold;">*</span></label>
                <asp:TextBox runat="server" ID="TxtFechaHasta"   MaxLength="16" 
                                    onkeypress="return soloLetras(event,'0123456789-')"    class="datetimepicker form-control" ></asp:TextBox>
            </div>
             <div class="form-group col-md-2">
                 <label for="inputZip">TURNO<span style="color: #FF0000; font-weight: bold;">*</span></label>
                 <asp:DropDownList runat="server" ID="CboTurno"    AutoPostBack="false"  class="form-control"  >
                        </asp:DropDownList>
            </div>
        </div>
      

     <asp:UpdatePanel ID="UPMENSAJE" runat="server"  UpdateMode="Conditional" ChildrenAsTriggers="true">
       <ContentTemplate>
             <div class="row">
                <div class="col-md-12 d-flex justify-content-center">
                     <div class="alert alert-danger" id="banmsg" runat="server" clientidmode="Static"><b>Error!</b> >Debe ingresar información......</div>
                </div>
             </div>		
            </ContentTemplate>
            <Triggers>
            <asp:AsyncPostBackTrigger ControlID="BtnBuscar" />
        </Triggers>
        </asp:UpdatePanel>

     <div class="form-title">
          NOVEDADES
     </div>

     <div class="form-row"> 
         <div class="form-group col-md-2">
                <label for="inputEmail4">NOVEDADES:</label>
                 <div class="d-flex">
                  <asp:TextBox ID="TxtCodNovedad" runat="server"  MaxLength="10"  class="form-control"   disabled ></asp:TextBox>  
                   <a class="btn btn-outline-primary mr-4" target="popup" onclick="Buscar_Novedades();"  id="buscar_novedad" runat="server" >
                   <span class='fa fa-search' style='font-size:24px' id="BtnBuscarNovedad" clientidmode="Static"></span> </a>   
                   
              </div> 
          </div>
          <div class="form-group col-md-3"> 
              <label for="inputEmail4">&nbsp;</label>
              <asp:TextBox ID="TxtDescNovedad" runat="server"  MaxLength="10"  class="form-control"   disabled ></asp:TextBox>  
          </div>

           <div class="form-group col-md-6">
                <label for="inputZip">MOTIVO DE LA NOVEDAD<span style="color: #FF0000; font-weight: bold;">*</span></label>
                 <asp:TextBox ID="TxtMotivo" runat="server" class="form-control" 
                    onkeypress="return soloLetras(event,'abcdefghijklmnopqrstuvwxyzñ1234567890 -',true)"  MaxLength="350"></asp:TextBox>
            </div>
          <div class="form-group col-md-1"> 
              <label for="inputZip">&nbsp;</label>
               <div class="d-flex">
                        <asp:Button ID="BtnBuscar" runat="server" class="btn btn-primary"   Text="AGREGAR"   OnClick="BtnBuscar_Click" />   
                         <img alt="loading.." src="../lib/file-uploader/img/loading.gif" height="32px" width="32px"  id="ImgCarga" class="nover"   />
                         
                  </div>
              
          </div>
     </div>


  
  
   

 
    
     <div class="form-row">  
    <div class="form-group col-md-12">

     <asp:UpdatePanel ID="UPDETALLE" runat="server" UpdateMode="Conditional" >  
       <ContentTemplate>
         
            
       <div class="form-row">
                 <div class="form-group col-md-12">
            <h3 id="LabelTotal" runat="server">DETALLE DE NOVEDADES</h3>
                <asp:Repeater ID="tablePagination" runat="server"  onitemcommand="tablePagination_ItemCommand">
                       <HeaderTemplate>
                       <table cellpadding="0" cellspacing="0" border="0" class="table table-bordered invoice" id="hidden-table-info-tarja">
                           <thead>
                          <tr>
                            
                            <th class="center hidden-phone">#</th>
                            <th class="center hidden-phone">TIPO EQUIPO</th>
                            <th class="center hidden-phone">EQUIPO</th>
                            <th class="center hidden-phone">NOVEDAD</th>
                            <th class="center hidden-phone">MOTIVO</th>
                            <th class="center hidden-phone">QUITAR</th>
                           
                          </tr>
                        </thead>
                        <tbody>
                        </HeaderTemplate>
                        <ItemTemplate>
                           <tr class="gradeC"> 
                                <td class="center hidden-phone"><asp:Label Text='<%#Eval("SECUENCIA")%>' ID="LblCarga" runat="server"  /></td>
                                <td class="center hidden-phone"><asp:Label Text='<%#Eval("NOMBRE_TIPO_EQUIPO")%>' ID="LblConsecutivo" runat="server"  /></td>
                                <td class="center hidden-phone"><asp:Label Text='<%#Eval("NOMBRE_EQUIPO")%>' ID="LblCantidad" runat="server"  /></td>
                                <td class="center hidden-phone"><asp:Label Text='<%#Eval("NOVEDAD")%>' ID="LblEmpresa" runat="server"  /> </td>
                                <td class="center hidden-phone"><asp:Label Text='<%#Eval("MOTIVO")%>' ID="LblChofer" runat="server"  />  </td> 
                              <td class="center hidden-phone">  
                                <asp:Button ID="BtnActualizar" CommandArgument= '<%#Eval("SECUENCIA")%>' runat="server" Text="QUITAR" class="btn btn-primary" ToolTip="Quitar" CommandName="Eliminar" />
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
    </div>

     <asp:UpdatePanel ID="UPBOTONES" runat="server" UpdateMode="Conditional" >  
             <ContentTemplate>
                
            
             <div class="row">
                <div class="col-md-12 d-flex justify-content-center">
                      <div class="alert alert-danger" id="banmsg_pie" runat="server" clientidmode="Static"><b>Error!</b>.</div>
                 </div>
             </div>
             <br/>

             <div class="row">
                <div class="col-md-12 d-flex justify-content-center">
                             <img alt="loading.." src="../lib/file-uploader/img/loading.gif" height="32px" width="32px"  id="ImgCargaDet" class="nover"  />
                 </div>
            </div>    
            <br/>

               
             <div class="row">
             <div class="col-md-12 d-flex justify-content-center">
                                   
                        <asp:Button ID="BtnNuevo" runat="server" class="btn btn-outline-primary mr-4"  Text="NUEVA TRANSACCION"  OnClick="BtnNuevo_Click"  />
                    
                    
                        <asp:Button ID="BtnGrabar" runat="server" class="btn btn-primary"  Text="GRABAR TRANSACCION"  OnClientClick="return confirmacion()"  OnClick="BtnGrabar_Click"/>
                   
                  
                  </div><!--btn-group-justified-->
                </div><!--showback-->
            </ContentTemplate>
             </asp:UpdatePanel>   
   

    
 
</div>

   <script type="text/javascript" src="../lib/common-scripts.js"></script>
 
   <script type="text/javascript" src="../lib/pages.js" ></script>

   <script type="text/javascript" src="../lib/advanced-form-components.js"></script>

         <script type="text/javascript" src="../lib/bootstrap/js/bootstrap.min.js"></script>

    <script type="text/javascript">
   

    function confirmacion()
    {
        var mensaje;
        var opcion = confirm("Estimado usuario, está seguro que desea grabar la transación. ?");
        if (opcion == true)
        {
       
            return true;
        } else
        {
            //loader();
	         return false;
        }

       
    }

    function mostrarloader(Valor) {

        try {
            if (Valor == "1") {
                document.getElementById("ImgCarga").className = 'ver';
            }
            else {
                document.getElementById("ImgCargaDet").className='ver';
            }
                
            } catch (e) {
                alert('Lamentamos comunicarle que ha ocurrido el siguiente problema:\n' + e.Message);
            }
        }


    function ocultarloader(Valor) {
        try {

            if (Valor == "1") {
                document.getElementById("ImgCarga").className = 'nover';
            }
            else {
                document.getElementById("ImgCargaDet").className='nover';
            }

             } catch (e) {
                alert('Lamentamos comunicarle que ha ocurrido el siguiente problema:\n' + e.Message);
            }
        }


        function Buscar_Novedades()
        {

            try {

             
             
               var ID_TIPO_EQUIPO = document.getElementById('<%=CboTipoEquipo.ClientID %>').value;
              

              
                if (ID_TIPO_EQUIPO == '' || ID_TIPO_EQUIPO == null || ID_TIPO_EQUIPO == undefined)
                 {
                      alertify.alert('Advertencia', 'Debe seleccionar el tipo de equipó').set('label', 'Aceptar');
                      return false;
                }

              
                  window.open('../checklist/lookup_novedades.aspx?ID_TIPO_EQUIPO='+ID_TIPO_EQUIPO, 'name', 'width=1024,height=800');
              
            }
            catch (e)
            {
                alertify.alert('Error',e.Message).set('label', 'Aceptar');
                return false;
            }
        }

          function popupCallback_Novedades(lookup_get_novedad)
    {
     
        if (lookup_get_novedad.sel_g_id_novedad != null) {

                this.document.getElementById("ID_NOVEDAD").value = lookup_get_novedad.sel_g_id_novedad;
                this.document.getElementById("NOVEDAD").value = lookup_get_novedad.sel_g_nombre_novedad;
                this.document.getElementById('<%= TxtCodNovedad.ClientID %>').value = lookup_get_novedad.sel_g_id_novedad;
                 this.document.getElementById('<%= TxtDescNovedad.ClientID %>').value = lookup_get_novedad.sel_g_nombre_novedad;
            }
            else {
                this.document.getElementById("ID_NOVEDAD").value = "";
                this.document.getElementById("NOVEDAD").value = "";
                 this.document.getElementById('<%= TxtCodNovedad.ClientID %>').value = "";
               this.document.getElementById('<%= TxtDescNovedad.ClientID %>').value = "";
            }
    
        }


         function limpiar()
        {

            try {

             
             
                this.document.getElementById('<%= TxtCodNovedad.ClientID %>').value = "";
                this.document.getElementById('<%= TxtDescNovedad.ClientID %>').value = "";
                 this.document.getElementById('<%= TxtMotivo.ClientID %>').value = "";
              
            }
            catch (e)
            {
                alertify.alert('Error',e.Message).set('label', 'Aceptar');
                return false;
            }
        }

         function limpiar_todo()
        {

            try {

                this.document.getElementById('<%= TxtOperador.ClientID %>').value = "";
                 this.document.getElementById('<%= TxtFechaHasta.ClientID %>').value = "";
             
                this.document.getElementById('<%= TxtCodNovedad.ClientID %>').value = "";
                this.document.getElementById('<%= TxtDescNovedad.ClientID %>').value = "";
                 this.document.getElementById('<%= TxtMotivo.ClientID %>').value = "";
              
            }
            catch (e)
            {
                alertify.alert('Error',e.Message).set('label', 'Aceptar');
                return false;
            }
        }

  </script>
       <script type="text/javascript">
            $(document).ready(function () {
                 $('.datetimepicker').datetimepicker().datetimepicker({ lang: 'es', timepicker: false, step: 30, format: 'm/d/Y' });
              });    
      </script>

</asp:Content>