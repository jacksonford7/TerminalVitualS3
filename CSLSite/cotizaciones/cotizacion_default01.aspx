<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="cotizacion_default01.aspx.cs" Inherits="CSLSite.cotizacion_default01"  Title="Cotizaciones de contenedores de importación" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>


<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
  <title>Cotizaciones de contenedores de importación</title>
  <link href="../img/favicon2.png" rel="icon"/>
  <link href="../img/icono.png" rel="apple-touch-icon"/>
 <!-- Bootstrap core CSS -->
  <link href="../css/bootstrap.min.css" rel="stylesheet"/>
  <link href="../css/dashboard.css" rel="stylesheet"/>
  <link href="../css/icons.css" rel="stylesheet"/>
  <link href="../css/style.css" rel="stylesheet"/>

     <!--external css-->
  <link href="../lib/font-awesome/css/font-awesome.css" rel="stylesheet" />
 

    <!--mensajes-->
 <link href="https://cdn.jsdelivr.net/npm/alertifyjs@1.11.0/build/css/alertify.min.css" rel="stylesheet"/>
 <script type="text/javascript" src="https://cdn.jsdelivr.net/npm/alertifyjs@1.11.0/build/alertify.min.js"></script>
<script src="../lib/jquery/jquery.min.js" type="text/javascript"></script>

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

 
   


</head>

<body>

<form id="contenedor" runat="server">
  <asp:ToolkitScriptManager ID="tkscata" runat="server"></asp:ToolkitScriptManager>

  <div id="div_BrowserWindowName" style="visibility:hidden;">
    <asp:HiddenField ID="hf_BrowserWindowName" runat="server" />
     
  </div>

      <div class="mt-4">         
        <nav class="mt-4" aria-label="breadcrumb">
          <ol class="breadcrumb">
            <li class="breadcrumb-item" id="opcion_principal" runat="server"><a href="#">Impo Contenedores</a></li>
            <li class="breadcrumb-item active" aria-current="page" id="sub_opcion" runat="server">COTIZACIÓN CONTENEDOR IMPORTACIÓN</li>
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
                        <asp:Button ID="BtnBuscar" runat="server" class="btn btn-primary"   Text="BUSCAR"   OnClientClick="return DetenerPost();"  OnClick="BtnBuscar_Click" />   
                         <img alt="loading.." src="../lib/file-uploader/img/loading.gif" height="32px" width="32px"  id="ImgCarga" class="nover"   />
                          
                  </div>
            </div>	
          <%--  <div class="form-group col-md-2"> 
                 <label for="inputZip">&nbsp;</label>
               
            </div>	--%>
       </div>
        <br/>
        <div class="row">
            <div class="col-md-12 d-flex justify-content-center">
                    <div data-theme="light" class="g-recaptcha" data-sitekey="6LfibkEUAAAAAIK-pu90AlJAbjvMSoKVIGkrov__" data-callback="recaptchaCallback"></div>
                             <input type="hidden" class="hiddenRecaptcha required" name="hiddenRecaptcha" id="hiddenRecaptcha">
                             <span id="msgCaptcha" style="color:red; font-size:small;"></span>
                             <asp:HiddenField runat="server" ID="imagencaptcha" />
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

     <h4 class="mb">DATOS DE LA COTIZACIÓN</h4>
     <asp:UpdatePanel ID="UPDATOSCLIENTE" runat="server"  UpdateMode="Conditional" ChildrenAsTriggers="true">
            <ContentTemplate>
            <div class="form-row">
                 <div class="form-group col-md-6"> 
                      <label for="inputAddress">AGENTE DE ADUANA:<span style="color: #FF0000; font-weight: bold;"></span></label>
                      <asp:HiddenField ID="hf_idagente" runat="server" />
                        <asp:HiddenField ID="hf_descagente" runat="server" />
                    <asp:HiddenField ID="hf_rucagente" runat="server" />
                      <asp:TextBox ID="TXTAGENCIA" runat="server" class="form-control"    placeholder=""  Font-Bold="false" disabled ></asp:TextBox>
                </div>
                 <div class="form-group col-md-6"> 
                     <label for="inputAddress">CLIENTE:<span style="color: #FF0000; font-weight: bold;"></span></label>
                            <asp:HiddenField ID="hf_idcliente" runat="server" />
                            <asp:HiddenField ID="hf_desccliente" runat="server" />
					        <asp:TextBox ID="TXTCLIENTE" runat="server" class="form-control"    placeholder=""  Font-Bold="false" disabled></asp:TextBox>                
                 </div>
                 <div class="form-group col-md-12" style=" display:none"> 
                     <label for="inputAddress">CLIENTE A COTIZAR:<span style="color: #FF0000; font-weight: bold;">*</span></label>
                      <asp:TextBox ID="TXTASUMEFACTURA" runat="server" class="form-control"  placeholder=""  Font-Bold="false" disabled Visible="false"></asp:TextBox>
                        <asp:HiddenField ID="hf_idasume" runat="server" />
                        <asp:HiddenField ID="hf_descasume" runat="server" />
                               
                        <asp:DropDownList runat="server" ID="CboAsumeFactura"    AutoPostBack="false"  class="form-control"  >
                            </asp:DropDownList>
                 </div> 

             </div>
            </ContentTemplate>
                <Triggers>
                <asp:AsyncPostBackTrigger ControlID="BtnBuscar" />
            </Triggers>
        </asp:UpdatePanel>
              
       
        <h4 class="mb">DETALLE DE LA CARGA</h4>
	    <div class="form-row">  
         <div class="form-group col-md-4">
                <label for="inputEmail4">FECHA SALIDA<span style="color: #FF0000; font-weight: bold;">*</span></label>
              <div class="d-flex">
                 <asp:TextBox runat="server" ID="TxtFechaHasta"   MaxLength="16" 
                                         onkeypress="return soloLetras(event,'0123456789-')"    class="datetimepicker form-control" ></asp:TextBox>
                  &nbsp;&nbsp;
                   
                        <asp:UpdatePanel ID="UPFECHA" runat="server"  UpdateMode="Conditional" ChildrenAsTriggers="true">
                        <ContentTemplate>
                            <label class="checkbox-container">
                            <asp:CheckBox ID="ChkTodos" runat="server"  Text="Cotizar Todos"  OnCheckedChanged="ChkTodos_CheckedChanged" Width="200px"    AutoPostBack="True" />
                                 <span class="checkmark"></span>
                             </label>
                        </ContentTemplate> 
                        <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="ChkTodos" />
                       </Triggers>
                      </asp:UpdatePanel> 
                   <asp:TextBox ID="Txtcomentario" runat="server" class="form-control" MaxLength="150" Width="330px" onkeypress="return soloLetras(event,'abcdefghijklmnopqrstuvwxyz0123456789-_')"  
                                 placeholder="Comentario" Visible="false"></asp:TextBox>
     
              </div>
         </div>
      </div>

		
       
     <asp:UpdatePanel ID="UPDETALLE" runat="server" UpdateMode="Conditional" >  
       <ContentTemplate>
         
              <div class="nover" id="myModal"  tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true" clientidmode="Static" runat="server" >
                        <div class="modal-dialog" id="ventana_content-popup"  >
                            <div class="modal-content">
		    
                            <div class="modal-header">
                               <asp:Button ID="BtnCerrar2" runat="server" class="btn btn-outline-primary mr-4" data-dismiss="modal" aria-hidden="true"  Text="&times;" OnClick="BtnCerrar_Click" />  
                              <h4 class="modal-title" id="myModalLabel">Alerta por cartera vencida!</h4>
                            </div>
                            <div class="modal-body">
			                 Estimado Cliente: <span id="fac_cliente" runat="server" clientidmode="Static">..</span>
                             <br /> 
                             Al momento usted presenta facturas vencidas.
                             <table border="1" cellspacing="1" cellpadding="1" style=" width:100%;">
                                 <tr>
                                  <td>Facturas por Vencer</td>
                                  <td><span id="fac_pend" runat="server" clientidmode="Static">0</span></td>
                                  </tr>
                                  <tr>
                                  <td>Facturas Vencidas</td>
                                  <td><span id="fac_ven" runat="server" clientidmode="Static">0</span></td>
                                  </tr>
                                  <tr>
                                  <td>Monto Total</td>
                                  <td><span id="monto_fac" runat="server" clientidmode="Static">$0.00</span></td>
                                  </tr>
                             </table>
                               Favor proceder con el pago de las facturas detalladas y compensar las mismas, o acercarse a nuestro departamento de Cobranzas (<a href="mailto:tesoreria@cgsa.com.ec?Subject=Aviso falta de pago" >tesoreria@cgsa.com.ec</a>)
			                   para regularizar valores.
                               <br />
                                <span id='cliente_ruc'></span>
                             </div>
                            <div class="modal-footer">
                                 <asp:Button ID="BtnCerrar" runat="server" class="btn btn-outline-primary mr-4"   Text="cerrar" OnClick="BtnCerrar_Click" />  
                            </div>
                        </div>
                    </div>
                 </div>
        
            <h3 id="LabelTotal" runat="server">DETALLE DE CONTENEDORES</h3>

            <asp:GridView ID="tablePagination" runat="server" AutoGenerateColumns="False"  DataKeyNames="CONTENEDOR"
                                    GridLines="None" OnPageIndexChanging="tablePagination_PageIndexChanging" OnPreRender="tablePagination_PreRender"   OnRowDataBound="tablePagination_RowDataBound" 
                                    PageSize="10"
                                    AllowPaging="True"
                                    CssClass="table table-bordered invoice">
                    <PagerStyle HorizontalAlign = "Right" CssClass="table table-bordered invoice"/>
                    <RowStyle  BackColor="#F0F0F0" />
                    <alternatingrowstyle  BackColor="#FFFFFF" />
                           
                                    <Columns>
                                                 
                                            <asp:BoundField DataField="SECUENCIA" HeaderText="#"  HeaderStyle-HorizontalAlign="Center"/>
                                        <asp:TemplateField HeaderText="FA" ItemStyle-CssClass="center hidden-phone">
                                            <ItemTemplate>
                                                <asp:UpdatePanel ID="UPSELECCIONAR" runat="server" ChildrenAsTriggers="true">
                                                <ContentTemplate>
                                                    <label class="checkbox-container">
                                                    <asp:CheckBox ID="CHKFA" runat="server" Checked='<%# Bind("VISTO") %>' OnCheckedChanged="CHKFA_CheckedChanged"    AutoPostBack="True"  />
                                                    <span class="checkmark"></span>
                                                    </label>
                                                </ContentTemplate>
                                                <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="CHKFA" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="CONTENEDOR" HeaderText="CONTENEDOR" SortExpression="CONTENEDOR"   HeaderStyle-HorizontalAlign="Center"/>
                                        <asp:BoundField DataField="FECHA_HASTA" DataFormatString="{0:yyyy/MM/dd HH:mm}" HeaderText="FECHA HASTA"  HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                            <asp:TemplateField HeaderText="TURNO" HeaderStyle-HorizontalAlign="Center" >
                                            <ItemTemplate>
                                                <asp:UpdatePanel ID="UPTURNO" runat="server" ChildrenAsTriggers="true">
                                                <ContentTemplate>
                                                <asp:DropDownList ID="CboTurno" class="form-control"
                                                runat="server"  DataTextField='TURNO' DataValueField='IDPLAN' AutoPostBack="true"  OnSelectedIndexChanged="CboTurno_SelectedIndexChanged"  >
                                                </asp:DropDownList>   
                                                </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="CboTurno" />
                                                    </Triggers>
                                            </asp:UpdatePanel>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="IN_OUT" HeaderText="ESTADO"  HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                        <asp:BoundField DataField="DOCUMENTO" HeaderText="DOCUMENTO"  HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                        <asp:BoundField DataField="FECHA_ULTIMA" HeaderText="ULTIMA FACTURA"  DataFormatString="{0:yyyy/MM/dd}" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                        <asp:BoundField DataField="NUMERO_FACTURA" HeaderText="NUMERO FACTURA"  HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                        <asp:BoundField DataField="CAS" HeaderText="FECHA CAS"  DataFormatString="{0:yyyy/MM/dd}" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                        <asp:BoundField DataField="DES_BLOQUEO" HeaderText="BLOQUEOS"  HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                        <asp:BoundField DataField="REEFER"   HeaderText="TIPO"  HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                        <asp:BoundField DataField="CONECTADO" HeaderText="CONECTADO"  HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"  Visible="false"/>
                                        <asp:BoundField DataField="TAMANO" HeaderText="TAMAÑO CONT"  HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                        <asp:BoundField DataField="IMDT" HeaderText="IMDT"  HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"  Visible="false"/>
                                        <asp:BoundField DataField="ESTADO_RDIT" HeaderText="ESTADO RDIT"  HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"  Visible="false"/>
                                        <asp:BoundField DataField="GKEY" HeaderText="GKEY"  HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"  Visible="false"/>
                                        <asp:BoundField DataField="IDPLAN" HeaderText="IDPLAN"  HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"  Visible="false"/>
                                    </Columns>
                                </asp:GridView>
                  

           </ContentTemplate>
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
                         <asp:Button ID="BtnNuevo" runat="server" class="btn btn-outline-primary mr-4"   Text="INICIAR"  OnClick="BtnNuevo_Click"  />
                        <asp:Button ID="BtnCotizar" runat="server" class="btn btn-primary"  Text="COTIZAR" OnClientClick="return mostrarloader('2')" OnClick="BtnCotizar_Click"/>
                </div>
            </div>
                 
                
                 
            </ContentTemplate>
             </asp:UpdatePanel>   
   

      
   

</div>

</form>
  

</body>

 
  <script type="text/javascript">window.jQuery || document.write('<script src="../assets/js/vendor/jquery.slim.min.js"><\/script>')</script>
  <script type="text/javascript" src="../js/bootstrap.bundle.min.js"></script>
  <script type="text/javascript" src="../lib/jquery/feather.min.js"></script>
 <script type="text/javascript" src="../lib/jquery/Chart.min.js"></script>
     


<%-- <script type="text/javascript" src="../lib/datatables/datatables.min.js"></script>
  <script type="text/javascript" src="../lib/advanced-datatable/js/DT_bootstrap.js"></script>--%>

   <script type="text/javascript" src="../lib/common-scripts.js"></script>
 
   <script type="text/javascript" src="../lib/pages.js" ></script>

   <script type="text/javascript" src="../lib/advanced-form-components.js"></script>

    <script src='https://www.google.com/recaptcha/api.js'></script>

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
                 $('.datetimepicker').datetimepicker().datetimepicker({ lang: 'es', timepicker: true, step: 30, format: 'm/d/Y H:i' });
              });    
      </script>


    <script type="text/javascript">

        function recaptchaCallback() {
                   document.getElementById('hiddenRecaptcha').value= grecaptcha.getResponse();
                   document.getElementById('msgCaptcha').innerHTML ='';
                };

        function DetenerPost()
        {
            document.getElementById("ImgCarga").className = 'ver';
            var vals = document.getElementById('<%=TXTMRN.ClientID %>').value;

            if (vals == '' || vals == null || vals == undefined)
            {
                alertify.alert('* Consulta Carga: *\n * Escriba el MRN a consultar *').set('label', 'Aceptar');
                    document.getElementById('<%=TXTMRN.ClientID %>').focus();
               
                    document.getElementById("ImgCarga").className = 'nover';
                    return false;
            }

            var vals2 = document.getElementById('<%=TXTMSN.ClientID %>').value;

            if (vals2 == '' || vals2 == null || vals2 == undefined)
            {
                    alertify.alert('* Consulta Carga: *\n * Escriba el MSN a consultar *').set('label', 'Aceptar');;
                    document.getElementById('<%=TXTMSN.ClientID %>').focus();
               
                    document.getElementById("ImgCarga").className = 'nover';
                    return false;
            }
             var vals3 = document.getElementById('<%=TXTHSN.ClientID %>').value;
             if (vals3 == '' || vals3 == null || vals3 == undefined)
            {
                    alertify.alert('* Consulta Carga: *\n * Escriba el HSN a consultar *').set('label', 'Aceptar');;
                    document.getElementById('<%=TXTHSN.ClientID %>').focus();
               
                    document.getElementById("ImgCarga").className = 'nover';
                    return false;
            }


            var captura = document.getElementById("hiddenRecaptcha").value;
            if(captura == '')
            {
                document.getElementById('msgCaptcha').innerHTML = "<span>Por favor confirme que usted no es un robot</span>";
                document.getElementById("ImgCarga").className = 'nover';
                return false;
            }
            $.getScript("https://www.google.com/recaptcha/api.js");
            grecaptcha.reset(); 
            document.getElementById("ImgCarga").className = 'nover';
            return true;
        }

</script>


</html>
