<%@ Page Title="" Language="C#" MasterPageFile="~/site.Master" AutoEventWireup="true" CodeBehind="stc_servicios.aspx.cs" Inherits="CSLSite.stc_servicios" %>
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


 <link href="../lib/advanced-datatable/css/demo_page.css" rel="stylesheet" />
  <link href="../lib/advanced-datatable/css/demo_table.css" rel="stylesheet" />
  <link rel="stylesheet" href="../lib/advanced-datatable/css/DT_bootstrap2.css" />



        <style type="text/css">
        body
        {
            /*font-family: Arial;
            font-size: 10pt;*/
        }
        .modal
        {
            position: fixed;
            z-index: 999;
            height: 100%;
            width: 100%;
            top: 0;
            background-color: Black;
            filter: alpha(opacity=60);
            opacity: 0.6;
            -moz-opacity: 0.8;
        }

        .modalBackground
        {
            background-color: Black;
            filter: alpha(opacity=60);
            opacity: 0.6;
        }
        .modalPopup
        {
            background-color: #FFFFFF;
            width: 726px;
            border: 3px solid #FF3720;
            padding: 0;
        }
        .modalPopup .header
        {
            background-color: #2FBDF1;
            height: 30px;
            color: White;
            line-height: 30px;
            text-align: center;
            font-weight: bold;
        }
        .modalPopup .body
        {
            min-height: 50px;
            line-height: 25px;
            text-align: center;
            /*font-weight: bold;*/
            margin-bottom: 5px;
        }
    </style>
 
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

      <div class="mt-4">         
        <nav class="mt-4" aria-label="breadcrumb">
          <ol class="breadcrumb">
            <li class="breadcrumb-item" id="opcion_principal" runat="server"><a href="#">STC</a></li>
            <li class="breadcrumb-item active" aria-current="page" id="sub_opcion" runat="server">Notificación de arribo de carga contenerizada de importación</li>
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
           <div class="form-group col-md-2">
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
           <div class="form-group col-md-4">
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
                        
        <h4 class="mb">DATOS DEL CLIENTE</h4>
        <asp:UpdatePanel ID="UPDATOSCLIENTE" runat="server"  UpdateMode="Conditional" ChildrenAsTriggers="true">
        <ContentTemplate>
        <div class="form-row">
            <div class="form-group col-md-6"> 
                <label for="inputAddress">CONSIGNATARIO <span style="color: #FF0000; font-weight: bold;"></span></label>
                <div class="d-flex">
                    
                     <asp:TextBox ID="TxtIdConsignatario" runat="server" class="form-control" Width="30%"    placeholder=""  Font-Bold="false" disabled ></asp:TextBox> 
                     &nbsp;
                    
                     <asp:TextBox ID="TxtDescConsignatario" runat="server" class="form-control"   Width="70%"   placeholder=""  Font-Bold="false" disabled ></asp:TextBox>
                   
                 </div>
            </div>
             <div class="form-group col-md-3"> 
                  <label for="inputAddress">BUQUE <span style="color: #FF0000; font-weight: bold;"></span></label>
                  <asp:TextBox ID="TxtBuque" runat="server" class="form-control"  placeholder=""  Font-Bold="false" disabled ></asp:TextBox> 
            </div>
            <div class="form-group col-md-3"> 
                  <label for="inputAddress">NAVIERA <span style="color: #FF0000; font-weight: bold;"></span></label>
                <asp:TextBox ID="TxtNaviera" runat="server" class="form-control"  placeholder=""  Font-Bold="false" disabled ></asp:TextBox> 
            </div>
             <div class="form-group col-md-3"> 
                  <label for="inputAddress">DEPOSITO TEMPORAL <span style="color: #FF0000; font-weight: bold;"></span></label>
                  <asp:TextBox ID="TxtDeposito" runat="server" class="form-control"  placeholder=""  Font-Bold="false" disabled ></asp:TextBox> 
            </div>
             <div class="form-group col-md-3"> 
                  <label for="inputAddress">FECHA ESTIMADA ARRIBO<span style="color: #FF0000; font-weight: bold;"></span></label>
                  <asp:TextBox ID="TxtFechaEstimada" runat="server" class="form-control"  placeholder=""  Font-Bold="false" disabled ></asp:TextBox> 
            </div>
             <div class="form-group col-md-3"> 
                  <label for="inputAddress">PESO MANIFESTADO KG<span style="color: #FF0000; font-weight: bold;"></span></label>
                  <asp:TextBox ID="TxtPesoManifiesto" runat="server" class="form-control"  placeholder=""  Font-Bold="false" disabled ></asp:TextBox> 
            </div>
             <div class="form-group col-md-3"> 
                  <label for="inputAddress">CANTIDAD BULTOS<span style="color: #FF0000; font-weight: bold;"></span></label>
                  <asp:TextBox ID="TxtCantidadBultos" runat="server" class="form-control"  placeholder=""  Font-Bold="false" disabled ></asp:TextBox> 
            </div>
             <div class="form-group col-md-2"> 
                  <label for="inputAddress">TOT. CONTENEDORES<span style="color: #FF0000; font-weight: bold;"></span></label>
                  <asp:TextBox ID="TxtTotalContenedores" runat="server" class="form-control"  placeholder=""  Font-Bold="false" disabled ></asp:TextBox> 
            </div>
            <div class="form-group col-md-10"> 
                  <label for="inputAddress">DESCRIPCIÓN DE LA CARGA<span style="color: #FF0000; font-weight: bold;"></span></label>
                  <asp:TextBox ID="TxtDescriocionCarga" runat="server" class="form-control"  placeholder=""  Font-Bold="false" disabled ></asp:TextBox> 
            </div>
            

       </div>
                        
        </ContentTemplate>
            <Triggers>
            <asp:AsyncPostBackTrigger ControlID="BtnBuscar" />
        </Triggers>
    </asp:UpdatePanel>
   <h4 class="mb">DETALLE DE CONTENEDORES</h4>

    <div class="form-row">  
    <div class="form-group col-md-12">
     <asp:UpdatePanel ID="UPDETALLE" runat="server" UpdateMode="Conditional" >  
       <ContentTemplate>
         
           
            <asp:GridView ID="tablePagination" runat="server" AutoGenerateColumns="False"  DataKeyNames="CONTENEDOR"
                                    GridLines="None" OnPageIndexChanging="tablePagination_PageIndexChanging" OnPreRender="tablePagination_PreRender"   OnRowDataBound="tablePagination_RowDataBound" 
                                    PageSize="10"
                                    AllowPaging="True"
                                   CssClass="table table-bordered invoice">
                    <PagerStyle HorizontalAlign = "Right" CssClass="table table-bordered invoice"   />
                    <RowStyle  BackColor="#F0F0F0" />
                    <alternatingrowstyle  BackColor="#FFFFFF" />
                           
                                    <Columns>
                                                 
                                            <asp:BoundField DataField="SECUENCIA" HeaderText="#"  HeaderStyle-HorizontalAlign="Center" />
                                        <asp:TemplateField HeaderText="FA" ItemStyle-CssClass="center hidden-phone" Visible="false">
                                            <ItemTemplate>
                                                <asp:UpdatePanel ID="UPSELECCIONAR" runat="server" ChildrenAsTriggers="true">
                                                <ContentTemplate>
                                                    <label class="checkbox-container">
                                                    <asp:CheckBox ID="CHKFA" runat="server" Checked='<%# Bind("VISTO") %>'  />
                                                    <span class="checkmark"></span>
                                                    </label>
                                                </ContentTemplate>
                                               <Triggers>
                                               <asp:AsyncPostBackTrigger ControlID="CHKFA" />
                                               </Triggers>
                                            </asp:UpdatePanel>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="CONTENEDOR" HeaderText="CONTENEDOR" SortExpression="CONTENEDOR"  HeaderStyle-HorizontalAlign="Center" Visible="false"/>
                                        <asp:BoundField DataField="FECHA_HASTA" DataFormatString="{0:yyyy/MM/dd HH:mm}" HeaderText="FECHA HASTA"  HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"/>
                                                  
                                        <asp:BoundField DataField="IN_OUT" HeaderText="ESTADO"  HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                        <asp:BoundField DataField="DOCUMENTO" HeaderText="DOCUMENTO"   HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                        <asp:BoundField DataField="FECHA_ULTIMA" HeaderText="ULTIMA FACTURA"  DataFormatString="{0:yyyy/MM/dd}" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                        <asp:BoundField DataField="NUMERO_FACTURA" HeaderText="NUMERO FACTURA"  HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                        <asp:BoundField DataField="CAS" HeaderText="FECHA CAS"  DataFormatString="{0:yyyy/MM/dd}" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                        <asp:BoundField DataField="AUTORIZADO" HeaderText="AUTORIZADO"  HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                        <asp:BoundField DataField="DES_BLOQUEO" HeaderText="BLOQUEADO"  HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                        <asp:BoundField DataField="DESCRIPCION"   HeaderText="DESCRIPCION"  HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                        <asp:BoundField DataField="CANTIDAD" HeaderText="CANTIDAD"  HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"  Visible="true"/>
                                        <asp:BoundField DataField="IMDT" HeaderText="IMDT"  HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"  Visible="false"/>
                                        <asp:BoundField DataField="ESTADO_RDIT" HeaderText="ESTADO RDIT"  HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"  Visible="false"/>
                                        <asp:BoundField DataField="GKEY" HeaderText="GKEY"  HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"  Visible="false"/>
                                        <asp:BoundField DataField="IDPLAN" HeaderText="IDPLAN"  HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"  Visible="false"/>
                                        <asp:BoundField DataField="NUMERO_PASE_N4" HeaderText="# PASE"  HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"  Visible="true"/>
                                    </Columns>
                                </asp:GridView>
                  
                

       

     
           </ContentTemplate>
     </asp:UpdatePanel>   
     </div><!--content-panel-->
      </div><!--row mb-->
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
                      <div class="alert alert-danger" id="banmsg_Pase" runat="server" clientidmode="Static"><b>Error!</b>.</div>
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
                             
                    <asp:Button ID="BtnNuevo" runat="server" class="btn btn-outline-primary mr-4"  Text="NUEVA CONSULTA"  OnClick="BtnNuevo_Click"  />

                   <asp:Button ID="BtnCotizar" runat="server" class="btn btn-outline-primary mr-4" Text="GENERAR PROFORMA" OnClientClick="return mostrarloader('2')" OnClick="BtnCotizar_Click"/>

                    
                   <asp:Button ID="BtnFacturar" runat="server" class="btn btn-primary" Text="GENERAR FACTURA" OnClientClick="return mostrarloader('2')" OnClick="BtnFacturar_Click" Visible="false" />
               </div> 
             </div>
            </ContentTemplate>
             </asp:UpdatePanel>   
   

       
    
</div>

   
  <%--<script type="text/javascript" src="../lib/datatables/datatables.min.js"></script>
  <script type="text/javascript" src="../lib/advanced-datatable/js/DT_bootstrap.js"></script>--%>

   <script type="text/javascript" src="../lib/common-scripts.js"></script>
 
   <script type="text/javascript" src="../lib/pages.js" ></script>

   <script type="text/javascript" src="../lib/advanced-form-components.js"></script>

 

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
       <script type="text/javascript">
            $(document).ready(function () {
                 $('.datetimepicker').datetimepicker().datetimepicker({ lang: 'es', timepicker: false, step: 30, format: 'm/d/Y' });
              });    
      </script>

</asp:Content>