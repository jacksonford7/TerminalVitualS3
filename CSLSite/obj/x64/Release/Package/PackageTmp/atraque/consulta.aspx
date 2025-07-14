<%@ Page Title="Consulta y Reimpresion" Language="C#" MasterPageFile="~/site.Master" AutoEventWireup="true" CodeBehind="consulta.aspx.cs" Inherits="CSLSite.atraque.consulta" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="placehead" runat="server">
  <link href="../img/favicon2.png" rel="icon"/>
  <link href="../img/icono.png" rel="apple-touch-icon"/>
  <link href="../css/bootstrap.min.css" rel="stylesheet"/>
  <link href="../css/dashboard.css" rel="stylesheet"/>
  <link href="../css/icons.css" rel="stylesheet"/>
  <link href="../css/style.css" rel="stylesheet"/>
  <link href="https://fonts.googleapis.com/css2?family=Roboto:wght@300;400;500;700&display=swap" rel="stylesheet"/>

  <link href="../shared/estilo/Reset.css" rel="stylesheet" />
  <link href="../shared/estilo/jquery.datetimepicker.css" rel="stylesheet" type="text/css" />

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
    <input id="zonaid" type="hidden" value="10" />
    <asp:ToolkitScriptManager ID="tkcal" EnableScriptGlobalization="true" runat="server"> </asp:ToolkitScriptManager>
  
    <div class="mt-4">         
        <nav class="mt-4" aria-label="breadcrumb">
            <ol class="breadcrumb">
            <li class="breadcrumb-item" id="opcion_principal" runat="server"><a href="#">Mis Naves</a></li>
                <li class="breadcrumb-item active" aria-current="page" id="sub_opcion" runat="server">Consulta y reimpresión de solicitudes</li>
            </ol>
        </nav>
    </div>

    <div class="dashboard-container p-4" id="cuerpo" runat="server">
      
        <div class="form-title">
           Datos del documento buscado
        </div>

        <div class="form-row">

            <div class="form-group col-md-6"> 
                <label for="inputAddress">Referencia<span style="color: #FF0000; font-weight: bold;"></span></label>
                <asp:TextBox  class="form-control"  ID="treferencia" runat="server" MaxLength="10" onkeypress="return soloLetras(event,'01234567890abcdefghijklmnopqrstuvwxyz',true)"></asp:TextBox>
            </div>

            <div class="form-group col-md-6"> 
                <label for="inputAddress">IMO<span style="color: #FF0000; font-weight: bold;"></span></label>
                <asp:TextBox  class="form-control"  ID="timo" runat="server" MaxLength="10" onkeypress="return soloLetras(event,'01234567890abcdefghijklmnopqrstuvwxyz -_')"></asp:TextBox>
                <span id="valran" class="opcional"> </span>
            </div>

            <div class="form-group col-md-6"> 
                <label for="inputAddress">Nave<span style="color: #FF0000; font-weight: bold;"></span></label>
                <asp:TextBox  class="form-control"  ID="tnave" runat="server" MaxLength="50" onkeypress="return soloLetras(event,'01234567890abcdefghijklmnopqrstuvwxyz_/ ')"></asp:TextBox>
            </div>

            <div class="form-group col-md-6"> 
                <label for="inputAddress">Manifiesto<span style="color: #FF0000; font-weight: bold;"></span></label>
                <asp:TextBox  class="form-control"  ID="tmani" runat="server" ClientIDMode="Static" MaxLength="16" onkeypress="return soloLetras(event,'abcdefghijklmnopqrstuvwxyz1234567890-_')"></asp:TextBox>
                <span id="valdae" class="opcional"></span>
            </div>
          
            <div class="form-group col-md-6"> 
                <label for="inputAddress">Desde<span style="color: #FF0000; font-weight: bold;"></span></label>
                <asp:TextBox ID="desded" runat="server"  CssClass="datepicker form-control" onkeypress="return soloLetras(event,'01234567890/')" onblur="valDate(this,true,valdate);" ClientIDMode="Static"></asp:TextBox>
            </div>

            <div class="form-group col-md-6"> 
                <label for="inputAddress">Hasta<span style="color: #FF0000; font-weight: bold;"></span></label>
               <div class="d-flex">
                <asp:TextBox ID="hastad" runat="server" ClientIDMode="Static"  MaxLength="15" CssClass="datepicker form-control" onkeypress="return soloLetras(event,'01234567890/')" onblur="valDate(this,true,valdate);"></asp:TextBox>
                <span id="valdate" class="validacion"></span>
                   </div>
            </div>
        </div>

        <div class="row">
            <div class="col-md-12 d-flex justify-content-center">
                <asp:Button ID="btbuscar" runat="server" Text="Iniciar la búsqueda" 
                    class="btn btn-primary" 
                    onclick="btbuscar_Click" 
                    OnClientClick="return validateDatesRange('desded','hastad','imagen');" />
                <span id="imagen"></span>
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
                                    <div ><br /></div>
                                    <div class="alert alert-warning" id="alerta" runat="server" >
                                    Confirme que los datos sean correctos. En caso de error, favor comuníquese 
                                    con el Departamento de Planificación a los teléfonos: +593 
                                    (04) 6006300, 3901700 
                                    </div>
                    
                                        <div class="form-title" >Documentos encontrados</div>
                                                <asp:Repeater ID="tablePagination" runat="server" onitemcommand="tablePagination_ItemCommand" >
                                                    <HeaderTemplate>
                                                    <table id="tablasort" cellspacing="1" cellpadding="1" class="table table-bordered table-sm table-contecon">
                                                        <thead>
                                                        <tr>
                                                        <th>#</th>
                                                        <th>Referencia</th>
                                                        <th>Nave</th>
                                                        <th>Viaje In</th>
                                                        <th>Viaje Out</th>
                                                        <th>Registrada</th>
                                                        <th>Imprimir</th>
                                                        <th>Actualizar</th>
                                                        </tr>
                                                        </thead> 
                                                        <tbody>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <tr class="point" >
                                                            <td><%#Eval("item")%></td>
                                                            <td><%#Eval("referencia")%></td>
                                                            <td><%#Eval("nave")%></td>
                                                            <td><%#Eval("viajeIn")%></td>
                                                            <td><%#Eval("ViajeOut")%></td>
                                                            <td><%#Eval("fecha")%></td>
                                                            <td>
                                                            <div class="tcomand">
                                                                <a href="../atraque/printer.aspx?sid=<%# securetext(Eval("referencia")) %>" class=" btn btn-link" target="_blank">Imprimir</a>
                                                            </div>
                                                            </td>
                                                              <td class="center hidden-phone"> 
                                                                <asp:Button ID="BtnDetalle"
                                                                CommandArgument= '<%#Eval("referencia")%>' runat="server" Text="ACTUALIZAR"  class="btn btn-primary" data-toggle="modal" data-target="#exampleModalToggle" ToolTip="ACTUALIZAR SOLICITUD DE ATRAQUE" CommandName="Ver" />
                                                            </td>
                                                        </tr>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        </tbody>
                                                        </table>
                                                    </FooterTemplate>
                                                </asp:Repeater>
                                          
                                
                        
                            </div>
                        <div id="sinresultado" runat="server" class=" alert  alert-primary"></div>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btbuscar" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>

     
  </div>

<div class="modal fade" id="exampleModalToggle" tabindex="-1" role="dialog" >

    <div class="modal-dialog modal-dialog-scrollable" style="max-width: 1500px">

        <div class="modal-content"> 

             <div class="dashboard-container p-4" id="Div1" runat="server">   
                  <div class="modal-header">
                      <asp:UpdatePanel ID="UPTITULO" runat="server"  UpdateMode="Conditional" ChildrenAsTriggers="true">
                       <ContentTemplate>
                                    <h5 class="modal-title" id="Titulo" runat="server">ACTUALIZAR SOLICITUD DE ATRAQUE</h5>
                       <asp:HiddenField ID="REFERENCIA" runat="server" />
                           </ContentTemplate>
                     </asp:UpdatePanel>
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                    <span aria-hidden="true">&times;</span>
                        </button>
                  </div>

                  
                
                 <div class="modal-body">
                       <div class="form-title">
                              Información sobre el viaje
                         </div>

                     <asp:UpdatePanel ID="UPVIAJE" runat="server"  UpdateMode="Conditional" >
                    <ContentTemplate>

                       

                         
  
        	  	          <div class="form-row">
		  
		                   <div class="form-group col-md-6"> 
		   	                  <label for="inputAddress">11. Viaje entrante<span style="color: #FF0000; font-weight: bold;"></span></label>
			                  <div class="d-flex">
                                      <asp:TextBox  class="form-control" ID="tvIn" runat="server" MaxLength="20" ClientIDMode="Static"
                                                    onkeypress="return soloLetras(event,'abcdefghijklmnopqrstuvwxyz1234567890-/._ ')" 
                                                    onblur="cadenareqerida(this,1,20,'tvInv');"
                                                    placeholder="INBOUND">
                                            </asp:TextBox>
                                            <span id="tvInv" class="validacion"> * </span>

			                  </div>
		                   </div>
		   
		                     <div class="form-group col-md-6"> 
		   	                  <label for="inputAddress">12. Número saliente<span style="color: #FF0000; font-weight: bold;"></span></label>
			                  <div class="d-flex">
                                       <asp:TextBox  class="form-control" ID="tvOu" runat="server" MaxLength="20"  ClientIDMode="Static"
                                                onkeypress="return soloLetras(event,'abcdefghijklmnopqrstuvwxyz1234567890-/._ ')" 
                                                onblur="cadenareqerida(this,1,20,'tvOuv');"
                                                placeholder="OUTBOUND">
                                            </asp:TextBox>
                                            <span id="tvOuv" class="validacion"> * </span>

			                  </div>
		                   </div>
		                 </div>

                   </ContentTemplate> 
                    </asp:UpdatePanel>

                       <div class="form-title">
                             Información de Senae
                         </div>
                         <asp:UpdatePanel ID="UPSENAE" runat="server"  UpdateMode="Conditional" >
                        <ContentTemplate>
                            <div class="form-row">
                                  <div class="form-group col-md-6"> 
		   	                      <label for="inputAddress">20. Manifiesto de Importación<span style="color: #FF0000; font-weight: bold;"></span></label>
			                      <div class="d-flex">
                                                                  <asp:TextBox  class="form-control" ID="tmrn" runat="server" MaxLength="16" 
                                                 onkeypress="return soloLetras(event,'abcdefghijklmnopqrstuvwxyz1234567890')" 
                                                 onblur="cadenareqerida(this,1,16,'valmrn');"
                                                 placeholder="CEC0000XXXX0000" ClientIDMode="Static" Text="CEC">
                                                </asp:TextBox>
                                                <span id="valmrn" class="validacion"> * </span>


			                      </div>
		                       </div>
		   
		                         <div class="form-group col-md-6"> 
		   	                      <label for="inputAddress">21. Manifiesto de Exportación<span style="color: #FF0000; font-weight: bold;"></span></label>
			                      <div class="d-flex">
                                                                  <asp:TextBox  class="form-control" ID="tdae" runat="server" MaxLength="16" 
                                                 onkeypress="return soloLetras(event,'abcdefghijklmnopqrstuvwxyz1234567890',false)" Text="CEC"
                                                  onblur="cadenareqerida(this,1,16,'sdae');"
                                                 placeholder="CEC0000XXXX0000" ClientIDMode="Static">
                                                </asp:TextBox>
                                                <span id="sdae" class="validacion"> * </span>


			                      </div>
		                       </div>
                            </div>
                        </ContentTemplate> 
                        </asp:UpdatePanel>

                          <div class="form-title">
                             Fechas de Operación (Estimadas)
                         </div>
                       <%-- <asp:UpdatePanel ID="UPFECHAS" runat="server"  UpdateMode="Conditional" >
                        <ContentTemplate>
                            --%>

                             <div class="form-row">
		                        
		                       <div class="form-group col-md-6"> 
                             <label for="inputEmail4">24. <strong>ETA:</strong>  Fecha estimada de arribo a boya de Data Posorja<span style="color: #FF0000; font-weight: bold;"> *</span></label> 
                           <%--        <label for="inputEmail4">24. <strong>ETA:</strong>  Fecha estimada de Atraque muelle CGSA <span style="color: #FF0000; font-weight: bold;"> *</span></label> --%>
                              <div class="d-flex">
                                    <label for="inputZip" style="width:200px;">ETA:</label>
                                       <asp:UpdatePanel ID="UPFECHAS" runat="server"  UpdateMode="Conditional"  >
                                       <ContentTemplate>
                                         
                                         <asp:TextBox ID="teta_ant" runat="server" MaxLength="16"  class="form-control" 
                                                       placeholder="ETA"  ClientIDMode="Static" disabled Width="200px" >
                                        </asp:TextBox>
                                        </ContentTemplate> 
                                        </asp:UpdatePanel>
                                    
                                            <label for="inputZip">&nbsp;</label>
                                               <asp:TextBox ID="teta" runat="server" MaxLength="16"  class="form-control" 
                                                        onkeypress="return soloLetras(event,'1234567890/ ')" 
                                                        onblur="valDate(this,true,seta);"
                                                        CssClass="datetimepicker form-control"
                                                        placeholder="ETA (Nuevo)" 
                                                        ClientIDMode="Static"  >
                                                    </asp:TextBox>
                                                    <span id="seta" class="validacion"> * </span>
    
                               
                                  </div>
		                       </div>
		                        
		                        <div class="form-group col-md-6">
                                    <label for="inputEmail4">25.<strong> ETB:</strong> Fecha estimada de Atraque muelle CGSA <span style="color: #FF0000; font-weight: bold;"> *</span></label> 
                                <%--    <label for="inputEmail4">25.<strong> ETB:</strong>  Fecha estimada de arribo a boya de Data Posorja <span style="color: #FF0000; font-weight: bold;"> *</span></label>--%>
                                    <div class="d-flex">
                                        <label for="inputZip" style="width:200px;">ETB:</label>
                                         <asp:UpdatePanel ID="UPFECHAS2" runat="server"  UpdateMode="Conditional"  >
                                       <ContentTemplate>
                                         
                                         <asp:TextBox ID="tetb_ant" runat="server" MaxLength="16"  class="form-control" 
                                                       placeholder="ETA"  ClientIDMode="Static" disabled Width="200px" >
                                        </asp:TextBox>
                                        </ContentTemplate> 
                                        </asp:UpdatePanel>

                                        <label for="inputZip">&nbsp;</label>

                                        <asp:TextBox ID="tetb" runat="server" MaxLength="16" 
                                        onkeypress="return soloLetras(event,'1234567890/ ')" 
                                        onblur="valDate(this,true,setb);sumarHorasFecha();"
                                        CssClass="datetimepicker form-control"
                                        placeholder="ETB (Nuevo)" ClientIDMode="Static">
                                        </asp:TextBox>
                                        <span id="setb" class="validacion"> * </span>
                                    </div>
                                </div>

		                    </div>

                    	     <div class="form-row">
		  
		                      <div class="form-group col-md-6">
                                     <asp:UpdatePanel ID="UPHORAS" runat="server"  UpdateMode="Conditional"  >
                                       <ContentTemplate>
                                        <label for="inputEmail4">26. Número de horas uso de muelle <span style="color: #FF0000; font-weight: bold;"> *</span></label>
                                        <div class="d-flex">
                                            <asp:TextBox  class="form-control" ID="thoras" runat="server" MaxLength="3" ClientIDMode="Static"
                                             onkeypress="return soloLetras(event,'1234567890 /',false)" 
            
                                             onblur=" cadenareqerida(this,1,8,'shora');sumarHorasFecha();"
                                             placeholder="Horas uso">
                                            </asp:TextBox>
                                            <span id="shora" class="validacion"> * </span>
                                        </div>
                                            </ContentTemplate> 
                                        </asp:UpdatePanel>

                                    </div>
		   
		                  <%--       <div class="form-group col-md-6">
                                        <label for="inputEmail4">27.<strong> ETS:</strong>  Fecha estimada de zarpe <span style="color: #FF0000; font-weight: bold;"> *</span></label>
                                        <div class="d-flex">
                                            <label for="inputZip" style="width:200px;">ETS:</label>
                                             <asp:UpdatePanel ID="UPFECHAS3" runat="server"  UpdateMode="Conditional"  >
                                           <ContentTemplate>
                                         
                                             <asp:TextBox ID="tets_ant" runat="server" MaxLength="16"  class="form-control" 
                                                           placeholder="ETS"  ClientIDMode="Static" disabled Width="200px" >
                                            </asp:TextBox>
                                            </ContentTemplate> 
                                            </asp:UpdatePanel>

                                             <label for="inputZip">&nbsp;</label>
                                            <asp:TextBox ID="tets" runat="server" CssClass="datetimepicker form-control"  MaxLength="16" ClientIDMode="Static" ReadOnly="true" placeholder="ETS (Nuevo)"></asp:TextBox>
                                            <a class="tooltip" ><span class="classic" >Se calcula a partir de el ETB + uso</span>
                                                <img alt='' src='../shared/imgs/info.gif' class='datainfo'/>
                                            </a>
                                        </div>
                                    </div>--%>
		                    </div>

                       
                       <div class="row">
                                <div class="col-md-12 d-flex justify-content-center">
                                        <asp:UpdatePanel ID="UPMENSAJE" runat="server"  UpdateMode="Conditional" >
                                        <ContentTemplate>
                                         <div class="alert alert-danger" id="banmsg_det" runat="server" clientidmode="Static"><b>Error!</b>Debe ingresar el número de la carga MRN......</div>
                                             </ContentTemplate>
                                         </asp:UpdatePanel>   
                                </div>
                            </div>

                 </div>


                  <div class="modal-footer">
                   <asp:UpdatePanel ID="UPBOTONES" runat="server" UpdateMode="Conditional" >  
                         <ContentTemplate>
                             
                             <br/>
                            <div class="row">
                                <div class="col-md-12 d-flex justify-content-center">
                                            
                                 </div>
                            </div>
                              <div class="row">
                                 <div class="col-md-12 d-flex justify-content-center">
                                      
                                     <img alt="loading.." src="../lib/file-uploader/img/loading.gif" height="32px" width="32px"  id="ImgCargaDet" class="nover"  />
                                     &nbsp;&nbsp;
                                    <asp:Button ID="BtnGrabar" runat="server"  class="btn btn-primary"  Text="Actualizar" OnClick="BtnGrabar_Click"  OnClientClick="return confirmacion()"   />
                                       &nbsp;&nbsp;
                                     <button type="button" class="btn btn-outline-primary " data-dismiss="modal">Cerrar</button>
                              </div>
                            </div>
                        </ContentTemplate>
                         </asp:UpdatePanel>   
            </div>

             </div>
        </div>

    </div>

</div>

    <script src="../Scripts/pages.js" type="text/javascript"></script>
     <script src="../Scripts/jquery.datetimepicker.js" type="text/javascript"></script>

     <script type="text/javascript" src="../lib/common-scripts.js"></script>
     <script type="text/javascript" src="../lib/advanced-form-components.js"></script>

    <script type="text/javascript">

                        $(document).ready(function () {
                            $('.datepicker').datetimepicker().datetimepicker({ lang: 'es', timepicker: false, closeOnDateSelect: true, format: 'd/m/Y' });
                             $('.datetimepicker').datetimepicker().datetimepicker({ lang: 'es', format: 'd/m/Y H:i', step: 30 });
                           // $('.datepicker').datetimepicker().datetimepicker({ lang: 'es', timepicker: false, format: 'd/m/Y', closeOnDateSelect: true, minDate: '0' });

                        });
  </script>
  

    <script type="text/javascript">
    function confirmacion()
    {
        var mensaje;
        var opcion = confirm("Estimado cliente, está seguro que desea actualizar la solicitud de atraque ?");
        if (opcion == true)
        {
            document.getElementById("ImgCargaDet").className = 'ver';
            return true;
        } else
        {
            //loader();
	         return false;
        }

       
        }

        function ocultarloader(Valor) {
        try {

            if (Valor == "1") {
               document.getElementById("ImgCargaDet").className='nover';
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

   function fechas()
    {
        
       try
       {

           $(document).ready(function ()
           {

             $('.datetimepicker').datetimepicker().datetimepicker({ lang: 'es', format: 'd/m/Y H:i', step: 30 });

           });

        }
        catch (e)
        {
                alert('Lamentamos comunicarle que ha ocurrido el siguiente problema:\n' + e.Message);
        }
   }
    

</script>
       <script src="../Scripts/hide.js" type="text/javascript"></script>

  </asp:Content>
