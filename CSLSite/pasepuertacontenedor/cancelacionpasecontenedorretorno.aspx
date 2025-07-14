<%@ Page Title="" Language="C#" MasterPageFile="~/site.Master" AutoEventWireup="true" CodeBehind="cancelacionpasecontenedorretorno.aspx.cs" Inherits="CSLSite.cancelacionpasecontenedorretorno" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>


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


<%-- <link href="../lib/advanced-datatable/css/demo_page.css" rel="stylesheet" />
  <link href="../lib/advanced-datatable/css/demo_table.css" rel="stylesheet" />
  <link rel="stylesheet" href="../lib/advanced-datatable/css/DT_bootstrap2.css" />--%>


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
     
  <div id="div_BrowserWindowName" style="visibility:hidden;">
    <asp:HiddenField ID="hf_BrowserWindowName" runat="server" />
    
  </div>
   <div class="mt-4">         
        <nav class="mt-4" aria-label="breadcrumb">
          <ol class="breadcrumb">
            <li class="breadcrumb-item" id="opcion_principal" runat="server"><a href="#">Impo Contenedores</a></li>
            <li class="breadcrumb-item active" aria-current="page" id="sub_opcion" runat="server">CANCELACIÓN E-PASS CONTENEDOR POR RETORNO DE SELLOS</li>
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
                <div class="alert alert-danger" id="banmsg" runat="server" clientidmode="Static"><b>Error!</b> >Debe ingresar el número de la carga MRN......</div>
                </div>
                </div>
      
            </ContentTemplate>
                <Triggers>
                <asp:AsyncPostBackTrigger ControlID="BtnBuscar" />
            </Triggers>
            </asp:UpdatePanel>
                        

     <h3>DETALLE DE PASES A CANCELAR</h3>   
     <asp:UpdatePanel ID="UPDETALLE" runat="server"  >  
       <ContentTemplate>
            <div class="form-row">
                <div class="form-group col-md-12">
                    <asp:Repeater ID="tablePagination" runat="server"  onitemcommand="tablePagination_ItemCommand" onitemdatabound="tablePagination_ItemDataBound" >
                    <HeaderTemplate>
                    <table cellpadding="0" cellspacing="0" border="0" class="table table-bordered invoice" id="hidden-table-info">
                        <thead>
                        <tr>
       
                        <th class="center hidden-phone">NUMERO DE CARGA</th>
                        <th class="center hidden-phone">CONTENEDOR</th>
                        <th class="center hidden-phone">FACTURADO <br/> HASTA</th>
                        <th class="center hidden-phone">TIPO</th>
                        <th class="center hidden-phone">FECHA<br/>TURNO</th>
                        <th class="center hidden-phone">HORA<br/>TURNO</th>
                        <th class="center hidden-phone">CIA. <br/>TRANSPORTE</th>
                        <th class="center hidden-phone">CONDUCTOR</th>
                        <th class="center hidden-phone">PLACA</th>
                            <th class="center hidden-phone">ESTADO</th>
                        <th class="center hidden-phone">GENERAR  <br/>E-PASS</th> 
                        </tr>
                    </thead>
                    <tbody>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr class="gradeC">
                            <td class="center hidden-phone"><asp:Label Text='<%#Eval("CARGA")%>' ID="LblCarga" runat="server"  /></td>
                            <td class="center hidden-phone"><asp:Label Text='<%#Eval("CONTENEDOR")%>' ID="LblContenedor" runat="server"  /></td>
                            <td class="center hidden-phone"><asp:Label Text='<%#Eval("FACTURADO_HASTA")%>' ID="LblFechaSalida" runat="server"  /></td>
                            <td class="center hidden-phone"><asp:Label Text='<%#Eval("TIPO")%>' ID="LblTipo" runat="server"  /></td>
                            <td class="center hidden-phone"><asp:Label Text='<%#DataBinder.Eval(Container.DataItem, "FECHA_SALIDA_PASE", "{0:yyyy/MM/dd}")%>' ID="LblFechaturno" runat="server"  /></td>
                            <td class="center hidden-phone"><asp:Label Text='<%#Eval("D_TURNO")%>' ID="LblTurno" runat="server"  /></td><%--D_TURNO--%>
                            <td class="center hidden-phone"><asp:Label Text='<%#Eval("CIATRASNSP")%>' ID="LblEmpresa" runat="server"  /> </td>
                            <td class="center hidden-phone"><asp:Label Text='<%#Eval("CONDUCTOR")%>' ID="LblChofer" runat="server"  />  </td> 
                            <td class="center hidden-phone"><asp:Label Text='<%#Eval("PLACA")%>' ID="LblPlaca" runat="server"  /> </td> 
                            
                                <td class="center hidden-phone"><asp:Label Text='<%#Eval("MOSTRAR_MENSAJE")%>' ID="LblMensaje" runat="server"  />
                                    <asp:Label Text='<%#Eval("ESTADO")%>' ID="LblEstado" runat="server" Visible="false"  />
                                    <asp:Label Text='<%#Eval("IN_OUT")%>' ID="LblIn_Out" runat="server" Visible="false"  />
                                    <br/> 
                               

                            </td> 
                            <td class="center hidden-phone"> 
                                <asp:UpdatePanel ID="UPSELECCIONAR" runat="server" ChildrenAsTriggers="true">
                                    <ContentTemplate>
                                          
                                            <label class="checkbox-container">
                                            <asp:CheckBox id="chkPase" runat="server"  Checked='<%#Eval("VISTO")%>' AutoPostBack="True" OnCheckedChanged="chkPase_CheckedChanged" />
                                        <span class="checkmark"></span>
                                            </label>
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
                </div>
            </div>
    
       </ContentTemplate>   
       </asp:UpdatePanel>

           <asp:UpdatePanel ID="UPMENSAJE" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">  
             <ContentTemplate>

                  <div class="modal fade" id="myModal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
                    <div class="modal-dialog">
                      <div class="modal-content">
                        <div class="modal-header">
                          <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                          <h4 class="modal-title" id="myModalLabel">Confirmar Cancelación</h4>
                        </div>
                        <div class="modal-body">
                          Si usted da click en SI, se procederá a cancelar los pases seleccionados
                        </div>
                        <div class="modal-footer">
                             <asp:Button ID="BtnSi" runat="server" class="btn btn-default"  Text="SI" OnClick="BtnSi_Click"   UseSubmitBehavior="false" data-dismiss="modal" />
                          <button type="button" class="btn btn-primary" data-dismiss="modal">NO</button>
                        </div>
                      </div>
                    </div>
                  </div>
             </ContentTemplate>
               <Triggers>
                 <asp:AsyncPostBackTrigger ControlID="BtnSi" />
               </Triggers>
         </asp:UpdatePanel>   

             <asp:UpdatePanel ID="UPBOTONES" runat="server" UpdateMode="Conditional" >  
             <ContentTemplate>
                
                <div class="row">
                    <div class="col-md-12 d-flex justify-content-center">
                             <div class="alert alert-danger" id="banmsg_det" runat="server" clientidmode="Static"><b>Error!</b>Debe ingresar el número de la carga MRN......</div>
                    </div>
                </div>

                  <br/>
                <div class="row">
                    <div class="col-md-12 d-flex justify-content-center">
                                 <img alt="loading.." src="../lib/file-uploader/img/loading.gif" height="32px" width="32px"  id="ImgCargaDet" class="nover"  />
                     </div>
                </div>

                 <div class="row">
                  <div class="col-md-12 d-flex justify-content-center">

                        <asp:Button ID="BtnGrabar" runat="server" class="btn btn-primary"   Text="Proceder Con Cancelación e-Pass"
                            OnClick="BtnGrabar_Click" 
                             OnClientClick="return confirm('Estimado cliente, está seguro que desea Proceder Con La Cancelación de e-Pass ?');" />

              
                  
                  </div><!--btn-group-justified-->
                </div><!--showback-->
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

     function mostrarloader2()
    {

             document.getElementById("ImgCargaDet").className = 'ver';
      
    }


    function ocultarloader2()
    {
        
       document.getElementById("ImgCargaDet").className = 'nover';
      
 
</script>



</asp:Content>