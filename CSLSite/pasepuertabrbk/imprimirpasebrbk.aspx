<%@ Page Title="" Language="C#" MasterPageFile="~/site.Master" AutoEventWireup="true" CodeBehind="imprimirpasebrbk.aspx.cs" Inherits="CSLSite.imprimirpasebrbk" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>


<asp:Content ID="Content1" ContentPlaceHolderID="placehead" runat="server">

<link href="../img/favicon2.png" rel="icon"/>
  <link href="../img/icono.png" rel="apple-touch-icon"/>
 <!-- Bootstrap core CSS -->
  <link href="../css/bootstrap.min.css" rel="stylesheet"/>
  <link href="../css/dashboard.css" rel="stylesheet"/>
  <link href="../css/icons.css" rel="stylesheet"/>
  <link href="../css/style.css" rel="stylesheet"/>

  <!--external css-->
  <link href="../shared/estilo/jquery.datetimepicker.css" rel="stylesheet" type="text/css" />
  <script src="../Scripts/jquery.datetimepicker.js" type="text/javascript"></script>

 <link href="../css/calendario_ajax.css" rel="stylesheet"/>


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

      $('#hidden-table-info thead tr').each(function() {

      });

      $('#hidden-table-info tbody tr').each(function() {
       
      });

      /*
       * Initialse DataTables, with no sorting on the 'details' column
       */
      var oTable = $('#hidden-table-info').dataTable({
        "aoColumnDefs": [{
          "bSortable": false,
          "aTargets": [0]
        }],
        "aaSorting": [
          [1, 'asc']
        ]
      });

     
        });
    }


</script>
 

</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="placebody" runat="server" >
      <asp:ToolkitScriptManager ID="tkscata" runat="server"></asp:ToolkitScriptManager>
     <script type="text/javascript">
              Sys.Application.add_load(Calendario); 
            </script>  
  <div id="div_BrowserWindowName" style="visibility:hidden;">
    <asp:HiddenField ID="hf_BrowserWindowName" runat="server" />
    
  </div>

     <div class="mt-4">         
        <nav class="mt-4" aria-label="breadcrumb">
          <ol class="breadcrumb">
            <li class="breadcrumb-item" id="opcion_principal" runat="server"><a href="#">BREAK BULK</a></li>
            <li class="breadcrumb-item active" aria-current="page" id="sub_opcion" runat="server">REIMPRIMIR E-PASS E-PASS CARGA BREAK BULK (BRBK)</li>
          </ol>
        </nav>
   </div>

<div class="dashboard-container p-4" id="cuerpo" runat="server">
    <div class="form-title">
           DATOS DE LA CARGA
     </div>

     <asp:UpdatePanel ID="UPCARGA" runat="server"  UpdateMode="Conditional" ChildrenAsTriggers="true">
        <ContentTemplate>
		<div class="form-row">
                <div class="form-group col-md-4">
                <label for="inputZip">MRN<span style="color: #FF0000; font-weight: bold;">*</span></label>
                <asp:TextBox ID="TXTMRN" runat="server" class="form-control" MaxLength="16"  onkeypress="return soloLetras(event,'abcdefghijklmnopqrstuvwxyz0123456789-_')"  
                                    placeholder="MRN"></asp:TextBox>
            </div>
                <div class="form-group col-md-2">
                    <label for="inputZip">MSN<span style="color: #FF0000; font-weight: bold;">*</span></label>
                    <asp:TextBox ID="TXTMSN" runat="server" class="form-control"  MaxLength="4"  onkeypress="return soloLetras(event,'abcdefghijklmnopqrstuvwxyz0123456789-_')" 
                                    placeholder="MSN"></asp:TextBox>
                </div>
            <div class="form-group col-md-2">
                    <label for="inputZip">HSN<span style="color: #FF0000; font-weight: bold;">*</span></label>
                    <asp:TextBox ID="TXTHSN" runat="server" class="form-control"  MaxLength="4"  onkeypress="return soloLetras(event,'abcdefghijklmnopqrstuvwxyz0123456789-_')" 
                                    placeholder="HSN"></asp:TextBox>
            </div>
            <div class="form-group col-md-2">
                    <label for="inputZip">&nbsp;</label>
                    <div class="d-flex">
                        <asp:Button ID="BtnBuscar" runat="server" class="btn btn-primary"   Text="BUSCAR"  OnClientClick="return mostrarloader('1')" OnClick="BtnBuscar_Click" />   
                            <img alt="loading.." src="../lib/file-uploader/img/loading.gif" height="32px" width="32px"  id="ImgCarga" class="nover"   />
                         
                    </div>
            </div>
     
		</div>
			<br/>
        <div class="row">
            <div class="col-md-12 d-flex justify-content-center">
                    <div class="alert alert-warning" id="banmsg" runat="server" clientidmode="Static"><b>Error!</b> >Debe ingresar el número de la carga MRN......</div>
            </div>
            </div>
                     
        </ContentTemplate>
            <Triggers>
            <asp:AsyncPostBackTrigger ControlID="BtnBuscar" />
        </Triggers>
        </asp:UpdatePanel>


   <div class="form-row">
           <div class="col-sm-4">
             <div class="d-flex">
                   <asp:UpdatePanel ID="UPBUSCARREPORTE" runat="server" UpdateMode="Conditional" >  
                    <ContentTemplate>
                    <div class="d-flex">
                         <asp:Button ID="BtnCargar" runat="server" class="btn btn-primary"  Text="CARGAR INFORMACION DEL PASE"  OnClientClick="mostrarloader('2')" OnClick="BtnCargar_Click" /> &nbsp;&nbsp;
  
                         <img alt="loading.." src="../lib/file-uploader/img/loading.gif" height="32px" width="32px"  id="ImgCargaDet" class="nover"  />
 
                     </div> 
			       </ContentTemplate>
                 </asp:UpdatePanel>  
                   &nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp;   
                     <asp:UpdatePanel ID="UPFECHA" runat="server"  UpdateMode="Conditional" ChildrenAsTriggers="true">
                     <ContentTemplate>
                    
                             <label class="checkbox-container">
                              <asp:CheckBox ID="ChkTodos" runat="server"  Text="Marcar Todos"  OnCheckedChanged="ChkTodos_CheckedChanged" AutoPostBack="True" />
                              <span class="checkmark"></span>
                              </label>
                      

                    </ContentTemplate> 
                    <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="ChkTodos" />
                    </Triggers>
                    </asp:UpdatePanel>        
             </div>        
            <h4 class="mb">
              Detalle de Pases
            </h4>
              <div class="bokindetalle" style=" width:100%; overflow:auto">               
                    <asp:UpdatePanel ID="UPDETALLE" runat="server"  >  
                            <ContentTemplate>        
                             
                                <asp:Repeater ID="tablePagination" runat="server" onitemdatabound="tablePagination_ItemDataBound" >
                                <HeaderTemplate>
                                    <table cellpadding="0" cellspacing="0" border="0" class="table table-bordered invoice">
                                    <thead>
                                    <tr >
                                    <th class="nover"># PASE</th>
                                    <th class="center hidden-phone"># PASE</th>
                                    <th class="center hidden-phone">TURNO</th>
                                    <th class="center hidden-phone">BULTOS</th>
                                    <th class="center hidden-phone">ESTADO</th>
                                    <th class="center hidden-phone">EMITIR</th>
                                    </tr>
                                </thead>
                                <tbody>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr >
                                        <td class="nover"><asp:Label Text='<%#Eval("NUMERO_PASE_N4")%>' ID="LblContenedor" runat="server"  /></td>
                                        <td class="center hidden-phone"><asp:Label Text='<%#Eval("ID_PASE")%>' ID="Label1" runat="server"  /></td>
                                        <td class="center hidden-phone"><asp:Label Text='<%#Eval("D_TURNO")%>' ID="Lblturno" runat="server"   /></td>
                                        <td class="center hidden-phone"  align="center"><asp:Label Text='<%#Eval("DOCUMENTO")%>' ID="LblCantidad" runat="server"    /></td>
                                        <td class="center hidden-phone"><asp:Label Text='<%#Eval("TIPO_CNTR")%>' ID="LblEstado" runat="server"  /></td>           
                                        <td class="center hidden-phone" align="center">  
                                            <asp:UpdatePanel ID="UPSELECCIONAR" runat="server" ChildrenAsTriggers="true">
                                                <ContentTemplate>
                                                        <script type="text/javascript">
                                                            Sys.Application.add_load(BindFunctions); 
                                                    </script>
                                                        <label class="checkbox-container">
                                                    <asp:CheckBox id="chkPase" runat="server"  Checked='<%#Eval("VISTO")%>' AutoPostBack="True" OnCheckedChanged="chkPase_CheckedChanged" />
                                                    <span class="checkmark"></span>
                                                    </label>
                                                    <br/> <asp:Label Text='<%#Eval("USUARIO_ING")%>' ID="LblUsuario" runat="server"  />
                                                </ContentTemplate>
                                                    <Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID="chkPase" />
                                                    </Triggers>
                                                </asp:UpdatePanel>
                                        </td>
                                        </tr>    
                                </ItemTemplate>
                                <FooterTemplate>
                                </tbody>
                                </table>
                                </FooterTemplate>
                            </asp:Repeater>
                                
                            </ContentTemplate>
                             <Triggers>
                                 <asp:AsyncPostBackTrigger ControlID="BtnCargar" />
                             </Triggers>
                           </asp:UpdatePanel>
              </div>             
          </div> <!-- col-sm-3-->
          <div class="col-sm-8">
              <br/>
               <div class="form-title">
                     Pase a Imprimir
               </div>

                 <div class="panel-body minimal">
                      
                     <div class="table-inbox-wrap">
                           <asp:UpdatePanel ID="UPPASEPUERTA" runat="server" UpdateMode="Conditional" >  
                            <ContentTemplate>
                            <div id="imagen" style="align-content:center;" runat="server" clientidmode="Static">
                                 <p align="center">
                                <img src="../img/print_pase.png" width="300" height="450" alt ="" /></p>
                           </div>
                          <rsweb:ReportViewer ID="rwReporte" runat="server" Width="100%" Height="890px"
                            DocumentMapCollapsed="True" ShowDocumentMapButton="False" 
                        ShowParameterPrompts="False" ShowPromptAreaButton="False">
                        <LocalReport EnableExternalImages="True" ReportPath="rptpasepuertacfs.rdlc" >
                        </LocalReport>
                        </rsweb:ReportViewer>

                        </ContentTemplate>
                        </asp:UpdatePanel>   
					</div><!-- table-inbox-wrap-->
                 </div>
               
          </div> <!-- col-sm-9-->
      </div><!-- row mt-->
        

     <asp:UpdatePanel ID="UPBOTONES" runat="server" UpdateMode="Conditional" >  
      <ContentTemplate>
                
           <div class="row">
                        <div class="col-md-12 d-flex justify-content-center">
                                 <div class="alert alert-warning" id="banmsg_det" runat="server" clientidmode="Static"><b>Error!</b> >Debe ingresar el número de la carga MRN......</div>
                          </div>
                     </div>

    </ContentTemplate>
        </asp:UpdatePanel>   
   


 </div>

 
  <script type="text/javascript" src="../lib/datatables/datatables.min.js"></script>
  <script type="text/javascript" src="../lib/advanced-datatable/js/DT_bootstrap.js"></script>

   <script type="text/javascript" src="../lib/common-scripts.js"></script>
 
   <script type="text/javascript" src="../lib/pages.js" ></script>

   <script type="text/javascript" src="../lib/advanced-form-components.js"></script>

  <script type="text/javascript" src='../lib/autocompletar/bootstrap3-typeahead.min.js'></script>



<script type="text/javascript">

 
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

</script>




</asp:Content>