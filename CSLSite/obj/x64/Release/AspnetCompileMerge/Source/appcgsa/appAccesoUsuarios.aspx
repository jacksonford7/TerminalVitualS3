

<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="appAccesoUsuarios.aspx.cs" Inherits="CSLSite.appAccesoUsuarios" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>


<asp:Content ID="Content1" ContentPlaceHolderID="placehead" runat="server">

    <link href="../shared/estilo/catalogos.css" rel="stylesheet" type="text/css" />
    <link href="../img/favicon2.png" rel="icon"/>
    <link href="../img/icono.png" rel="apple-touch-icon"/>
    <link href="../css/bootstrap.min.css" rel="stylesheet"/>
    <link href="../css/dashboard.css" rel="stylesheet"/>
    <link href="../css/icons.css" rel="stylesheet"/>
    <link href="../css/style.css" rel="stylesheet"/>
    <link href="https://fonts.googleapis.com/css2?family=Roboto:wght@300;400;500;700&display=swap" rel="stylesheet"/>

 
 

<script type="text/javascript">
 function BindFunctions() {
         $(document).ready(function ()
         {
              /*
               * Insert a 'details' column to the table
               */
              var nCloneTh = document.createElement('th');
              var nCloneTd = document.createElement('td');
              nCloneTd.innerHTML = '<img src="../lib/advanced-datatable/images/details_open.png">';
              nCloneTd.className = "center";

              $('#hidden-table-info thead tr').each(function() {});

              $('#hidden-table-info tbody tr').each(function() {});

              /*
               * Initialse DataTables, with no sorting on the 'details' column
               */
              var oTable = $('#hidden-table-info').dataTable(
                    {
                          "aoColumnDefs":
                          [
                            {
                                "bSortable": false,
                                "aTargets": [0]
                            }
                          ],
                          "aaSorting":
                          [
                            [1, 'asc']
                          ]
                    }
                );
        });
    }
</script>


     <script type="text/javascript">
         $(document).ready(function () {
             $('#<%= tablePagination.ClientID %>').dataTable();
         });

    </script>


    

     <script type="text/javascript">
         $(document).ready(function () {
             $('#<%= GrillaDetalle.ClientID %>').dataTable();
         });

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
                <li class="breadcrumb-item" id="opcion_principal" runat="server"><a href="#">App CGSA Usuarios Con Paquetes Adquiridos
</a></li>
                <li class="breadcrumb-item active" aria-current="page" id="sub_opcion" runat="server">ADMINISTRACIÓN DE PERMISOS DE USUARIOS</li>
            </ol>
        </nav>
    </div>

    <div class="dashboard-container p-4" id="cuerpo" runat="server">
    
         
            
  <%--           <asp:UpdatePanel ID="UPCARGA" runat="server"  UpdateMode="Conditional" ChildrenAsTriggers="true">
                <ContentTemplate>
                    <asp:Panel ID="PanelCualquiera" runat="server" DefaultButton ="BtnBusca">
                      --%>
                            <div class="form-row" >
                                     
                                <div class="form-group col-md-4">
                                    <label for="inputAddress">Usuario:<span style="color: #FF0000; font-weight: bold;"></span></label>
                                    <div class="d-flex">
                                        <asp:HiddenField ID="hdfIdUsuario" runat="server" />
                                        &nbsp;
                                        <asp:TextBox ID="txtUsuaroName" runat="server" disabled class="form-control" MaxLength="16" onkeypress="return soloLetras(event,'abcdefghijklmnopqrstuvwxyz0123456789-_')"  placeholder="NAVE REFERENCIA"></asp:TextBox>
                                        <asp:HiddenField ID="hdfIdUsuarioLogeado" runat="server" />

                                    </div>
                                </div>

                                <div class="form-group  col-md-3"> 
                                    <label for="inputAddress">Tipo de Usuario:<span style="color: #FF0000; font-weight: bold;"></span></label>

                                    <div class="d-flex">
                                        <asp:DropDownList ID="cmbTipoUsuario" disabled CssClass="form-control" runat="server" AppendDataBoundItems="true"></asp:DropDownList>
                                        <asp:HiddenField ID="hdIdTipoUsuarioLogeado" runat="server" />
                                    </div>
                                </div>

                                <div class="form-group   col-md-3"> 
                                    <label for="inputAddress">Categoría de Usuario:<span style="color: #FF0000; font-weight: bold;"></span></label>

                                    <div class="d-flex">
                                        <asp:DropDownList ID="ddlTipoUsuario" disabled CssClass="form-control" runat="server" AppendDataBoundItems="true"></asp:DropDownList>
                                        <asp:HiddenField ID="hdTipoUsuariologeado" runat="server" />
                                    </div>
                                </div>

                                <div class="form-group   col-md-2"> 
                                    <br />
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                    <asp:Button ID="btnRegresar" class="btn btn-outline-primary mr-4" runat="server" Text="Regresar" OnClick="btnRegresar_Click" />
                                </div>
                            </div>
                     
                        <div><br /></div>
                    
                        <div class="alert alert-warning" id="banmsg" runat="server" clientidmode="Static"><b>Error!</b> >Debe ingresar el número de la carga MRN......</div>
                    
                   <%--   </asp:Panel>            
               </ContentTemplate>
                   <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="BtnBusca" />
                </Triggers>
            </asp:UpdatePanel>     --%>
        
        

    </div>

    <br />

    <section id="main-content">
        <section class="wrapper">


           <div class="row mt">
               <div class="col-sm-4" >
                

                     <div class="dashboard-container p-4" id="Div1" runat="server" style="height:990px" >

                            <asp:UpdatePanel ID="UPBUSCARREPORTE" runat="server" UpdateMode="Conditional" >  
                                <ContentTemplate>
                            

                                    <div class="form-group   col-md-12"> 
                                        <label for="inputAddress">Filtros de opciones:<span style="color: #FF0000; font-weight: bold;"></span></label>

                                        <div class="d-flex">
                                            <asp:DropDownList ID="cmbFiltro" CssClass="form-control" runat="server" AppendDataBoundItems="true" AutoPostBack="true" OnSelectedIndexChanged="cmbFiltro_SelectedIndexChanged"></asp:DropDownList>
                                            <img alt="loading.." src="../lib/file-uploader/img/loading.gif" height="32px" width="32px"  id="ImgCarga" class="nover"  />
                                        </div>
                                    </div>

                                   
                                       
			                    </ContentTemplate>
                            </asp:UpdatePanel>
                      
                           
		                                              
                                <div class="form-title ">
                                    Detalle de Opciones Disponibles
                                </div>
                             

                                <asp:UpdatePanel ID="UPCAB"  runat="server"  UpdateMode="Conditional">
                                    <ContentTemplate>        
                                        <div class="bokindetalle" style="height:750px; width:100%; overflow:auto">    
                                            <asp:GridView ID="tablePagination" runat="server" AutoGenerateColumns="False"  
                                                        DataKeyNames="opcion"
                                                        GridLines="None" 

                                                        OnPageIndexChanging="tablePagination_PageIndexChanging" 
                                                        OnPreRender="tablePagination_PreRender"     
                                                        OnRowCommand="tablePagination_RowCommand"   
                                                        OnRowDataBound="tablePagination_RowDataBound"
                                                        
                                                        PageSize="8000"
                                                        AllowPaging="false"
                                                 CssClass="display table table-bordered">
                                                       
                                                    <PagerStyle HorizontalAlign = "Right" CssClass="display table table-bordered"  />
                                                    <RowStyle  BackColor="#F0F0F0" />
                                                    <alternatingrowstyle  BackColor="#FFFFFF" />
                           
                                                    <Columns>
                                                        <asp:BoundField DataField="servicio" HeaderText="SERVICIO" Visible="false"  HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>
                                                        <asp:BoundField DataField="opcion" HeaderText="ID"  HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>
                                                        <asp:BoundField DataField="descripcion" HeaderText="OPCIÓN"   SortExpression="descripcion" HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>                                                                
                                                        <asp:BoundField DataField="idFiltro" HeaderText="FILTRO" Visible="false" ItemStyle-Font-Size="9px" SortExpression="idFiltro" HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>                                                                                                       
                                                        
                                                   

                                                        <asp:TemplateField HeaderText="" ItemStyle-CssClass="center hidden-phone">
                                                            <ItemTemplate>
                                                                <asp:Button runat="server" ID="btnAdd"  Height="30px" Text="+"  CommandName="Seleccionar" CommandArgument='<%#Eval("opcion")%>' class="btn btn-primary"/>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>

                                                      

                                                    </Columns>
                                            </asp:GridView>
                                        </div>

                                
                                    </ContentTemplate>
                                                     
                                </asp:UpdatePanel>
                                        
                            
                    </div>

               
              </div> 

            
              <div class="col-sm-8">
                   
                        <div class="dashboard-container p-4" id="Div2" runat="server" style="height:990px">
                                        
                            
                                        <asp:UpdatePanel ID="UPDET" runat="server" UpdateMode="Conditional" >  
                                            <ContentTemplate>

                                                      <div class="form-title ">
                                                        Detalle de Opciones Asignadas
                                                    </div>

                                                    <div class="bokindetalle" style="height:890px; width:100%; overflow:auto">       
                                                        <asp:GridView ID="GrillaDetalle" runat="server" AutoGenerateColumns="False"  DataKeyNames="opcion"
                                                                                GridLines="None" 
                                                                                PageSize="800"
                                                                                AllowPaging="false"
                                                                                
                                                                                OnRowDataBound="GrillaDetalle_RowDataBound"
                                                                                OnRowCommand="GrillaDetalle_RowCommand" 
                                                                                OnPageIndexChanging="GrillaDetalle_PageIndexChanging" 
                                                                                OnPreRender="GrillaDetalle_PreRender"


                                                                                CssClass="table table-bordered invoice">
                                                                <PagerStyle HorizontalAlign = "Right" CssClass="table table-bordered invoice"  />
                                                                <RowStyle  BackColor="#F0F0F0" />
                                                                <alternatingrowstyle  BackColor="#FFFFFF" />
                           
                                                            <Columns>       
                                                              

                                                            
                                                                <asp:BoundField DataField="servicio" HeaderText="SERVICIO" Visible="false"  HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>
                                                                <asp:BoundField DataField="opcion" HeaderText="ID"  HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>
                                                                <asp:BoundField DataField="descripcion" HeaderText="OPCIÓN"   SortExpression="descripcion" HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>                                                                
                                                                <asp:BoundField DataField="idFiltro" HeaderText="FILTRO" Visible="false" ItemStyle-Font-Size="9px" SortExpression="idFiltro" HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"/>                                                                                                       
                                                        
                                                                <asp:TemplateField HeaderText="" ItemStyle-CssClass="center hidden-phone">
                                                                    <ItemTemplate>
                                                                        <asp:Button runat="server" ID="btnDel"  Height="30px" Text="-"  CommandName="Seleccionar" CommandArgument='<%#Eval("opcion")%>' class="btn btn-primary"/>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>

                                                            </Columns>
                                                        </asp:GridView>
                                                    </div>
                                                   
                                            </ContentTemplate>   
                                        </asp:UpdatePanel>   
                     
                        </div>
                   
              </div>
          </div>
        

            <asp:UpdatePanel ID="UPBOTONES" runat="server" UpdateMode="Conditional" >  
                <ContentTemplate>
                
                    <div class="form-group">
                            <div class="alert alert-warning" id="banmsg_det" runat="server" clientidmode="Static"><b>Error!</b> >Debe ingresar el número de la carga MRN......</div>
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
                          <h4 class="modal-title" id="myModalLabel">Confirmar Generación</h4>
                        </div>
                        <div class="modal-body">
                          Si usted da click en SI, se procederá a generar la factura
                        </div>
                        <div class="modal-footer">
                             <asp:Button ID="BtnProcesar" runat="server" class="btn btn-default"  Text="SI"  UseSubmitBehavior="false" data-dismiss="modal" />
                          <button type="button" class="btn btn-primary" data-dismiss="modal">NO</button>
                        </div>
                      </div>
                    </div>
                  </div>
             </ContentTemplate>
                    <Triggers>
                          <asp:AsyncPostBackTrigger ControlID="BtnProcesar" />
                        </Triggers>
         </asp:UpdatePanel>   

        </section>
    </section>

  
  <script type="text/javascript" src="../lib/pages.js" ></script>
 
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
