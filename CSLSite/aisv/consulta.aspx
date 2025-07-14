<%@ Page Title="Consultar AISV" Language="C#" MasterPageFile="~/site.Master" AutoEventWireup="true" CodeBehind="consulta.aspx.cs" Inherits="CSLSite.consulta" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="placehead" runat="server">
    <%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>


 <link href="../shared/estilo/tables.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/aisv-general.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/work.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/catalogos.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/info.css" rel="stylesheet" type="text/css" />

    <link href="../img/favicon2.png" rel="icon"/>
    <link href="../img/icono.png" rel="apple-touch-icon"/>
    <link href="../css/bootstrap.min.css" rel="stylesheet"/>
    <link href="../css/dashboard.css" rel="stylesheet"/>
    <link href="../css/icons.css" rel="stylesheet"/>
    <link href="../css/style.css" rel="stylesheet"/>
    <link href="https://fonts.googleapis.com/css2?family=Roboto:wght@300;400;500;700&display=swap" rel="stylesheet"/>

<%--    <link href="../shared/estilo/jquery.datetimepicker.css" rel="stylesheet" type="text/css" />--%>
    <!--Datatables-->
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/5.15.1/css/all.min.css" integrity="sha512-+4zCK9k+qNFUR5X+cKL9EIR+ZOhtIloNl9GIKS57V1MyNsYpYcUrUeQc9vNfzsWfV28IaLL3i96P9sdNyeRssA==" crossorigin="anonymous" />
    <link href="../css/datatables.min.css" rel="stylesheet" />
    <link rel="stylesheet" type="text/css" href="../js/buttons/css/buttons.dataTables.min.css" />
    <script src="../lib/jquery/jquery.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="../js/datatables.js"></script>
    <script src="../Scripts/table_catalog.js" type="text/javascript"></script>
    <script type="text/javascript" src="../js/bootstrap.bundle.min.js"></script>
    
      <script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/moment.js/2.29.1/moment.min.js"></script>
    <script type="text/javascript" src="https://cdn.jsdelivr.net/npm/sweetalert2@11.0.19/dist/sweetalert2.all.min.js"></script>
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/sweetalert2@11.0.19/dist/sweetalert2.min.css" />
    <script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/fullcalendar/3.10.2/fullcalendar.min.js"></script>
    <link href="https://cdnjs.cloudflare.com/ajax/libs/fullcalendar/3.10.2/fullcalendar.min.css" rel="stylesheet" />
  
     <link href="../shared/estilo/jquery.datetimepicker.css" rel="stylesheet" type="text/css" />
  <script src="../Scripts/jquery.datetimepicker.js" type="text/javascript"></script>

     <link href="../css/calendario_ajax.css" rel="stylesheet"/>
   
    <style type="text/css">
        #progressBackgroundFilter {
            position: fixed;
            bottom: 0px;
            right: 0px;
            overflow: hidden;
            z-index: 1000;
            top: 0;
            left: 0;
            background-color: #CCC;
            opacity: 0.8;
            filter: alpha(opacity=80);
            text-align: center;
        }

        #processMessage {
            text-align: center;
            position: fixed;
            top: 30%;
            left: 43%;
            z-index: 1001;
            border: 5px solid #67CFF5;
            width: 200px;
            height: 100px;
            background-color: White;
            padding: 0;
        }

        .modal-dialog.modal-xm {
            max-width: 950px; /* Ajusta el ancho máximo según tus necesidades */
            max-height: 650px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="placebody" runat="server">
    <asp:ToolkitScriptManager ID="tkcal" EnableScriptGlobalization="true" runat="server"> </asp:ToolkitScriptManager>
    <asp:HiddenField ID="manualHideReferencia" runat="server" />
     <div class="mt-4">         
        <nav class="mt-4" aria-label="breadcrumb">
          <ol class="breadcrumb">
            <li class="breadcrumb-item" id="opcion_principal" runat="server"><a href="#">Exportación</a></li>
             <li class="breadcrumb-item active" aria-current="page" id="sub_opcion" runat="server">Consulta, reimpresión y anulación de AISV</li>
          </ol>
        </nav>
      </div>
 
    <div class="dashboard-container p-4" id="cuerpo" runat="server">
      
        <div class="form-title">
           Datos del documento buscado
        </div>

      <div class="form-row">
          <div class="form-group col-md-6"> 
              <label for="inputAddress">AISV No. :<span style="color: #FF0000; font-weight: bold;"></span></label>
               <asp:TextBox ID="aisvn" runat="server"  MaxLength="15" class="form-control"
             onkeypress="return soloLetras(event,'01234567890',true)"></asp:TextBox>
          </div>
          <div class="form-group col-md-6"> 
            <label for="inputAddress">Contenedor:<span style="color: #FF0000; font-weight: bold;"></span></label>
               <asp:TextBox ID="cntrn" runat="server"  MaxLength="15"  class="form-control"             
                  onkeypress="return soloLetras(event,'01234567890abcdefghijklmnopqrstuvwxyz',true)"></asp:TextBox>
                 <span id="valran" class="opcional"></span>
          </div>
          <div class="form-group col-md-6"> 
           <label for="inputAddress">Documento No. (DAE,DAS etc):<span style="color: #FF0000; font-weight: bold;"></span></label>
               <asp:TextBox ID="docnum" runat="server"  MaxLength="15" class="form-control"     
                   onkeypress="return soloLetras(event,'01234567890abcdefghijklmnopqrstuvwxyz',true)"></asp:TextBox>
          </div>
          <div class="form-group col-md-6"> 
           <label for="inputAddress">Booking No.:<span style="color: #FF0000; font-weight: bold;"></span></label>
               <asp:TextBox ID="booking" runat="server" ClientIDMode="Static" MaxLength="15" class="form-control"   
             onkeypress="return soloLetras(event,'abcdefghijklmnñopqrstuvwxyz1234567890-_')" 
                 ></asp:TextBox>
              <span id="valdae" class="opcional"></span>
          </div>
          <div class="form-group col-md-6"> 
           <label for="inputAddress">Generados desde el día:<span style="color: #FF0000; font-weight: bold;"></span></label>
               <asp:TextBox ID="desded" runat="server"  CssClass="datetimepicker form-control" 
             onkeypress="return soloLetras(event,'01234567890/')" 
                 onblur="valDate(this,true,valdate);" ClientIDMode="Static"></asp:TextBox>
          </div>
          <div class="form-group col-md-6"> 
           <label for="inputAddress">Hasta:<span style="color: #FF0000; font-weight: bold;"></span></label>
               <asp:TextBox ID="hastad" runat="server" ClientIDMode="Static"  MaxLength="15" CssClass="datetimepicker form-control" 
             onkeypress="return soloLetras(event,'01234567890/')" 
                  onblur="valDate(this,true,valdate);"></asp:TextBox>
               <span id="valdate" class="validacion"></span>
          </div>
    </div>
      <div class="row">
            <div class="col-md-12 d-flex justify-content-center">
                 <asp:Button ID="btbuscar" runat="server" Text="Iniciar la búsqueda"  
                     class="btn btn-primary" 
                     onclick="btbuscar_Click" OnClientClick="return validateDatesRange('desded','hastad','imagen');" />
                 <span id="imagen"></span>
            
         </div>
             
            </div>

             <div class="cataresult" >
               <asp:UpdatePanel ID="upresult" runat="server" ChildrenAsTriggers="true">
                     <ContentTemplate>
                        <script type="text/javascript">
                            Sys.Application.add_load(BindFunctions);
                        </script>
             <div id="xfinder" runat="server" visible="false"  >
             <div class="alert alert-warning" id="alerta" runat="server" >
               Confirme que los datos sean correctos. En caso de error, favor comuníquese 
               con el Departamento de Planificación a los teléfonos: +593 
               (04) 6006300, 3901700 
             </div>
             <div >
             <div >
                  <div >Documentos encontrados</div>
                 <div class="bokindetalle">
                 <asp:Repeater ID="tablePagination" runat="server" 
                         onitemcommand="tablePagination_ItemCommand" >
                 <HeaderTemplate>
                 <table id="tablasort" cellspacing="1" cellpadding="1"  class="table table-bordered table-sm table-contecon"   >
                 <thead>
                 <tr>
                 <th scope="col">#</th>
                 <th scope="col">AISV #</th>
                 <th scope="col">Tipo</th>
                 <th scope="col">Booking</th>
                 <th scope="col">Registrado</th>
                 <th scope="col">Carga</th>
                 <th scope="col">Estado</th>
                 <th scope="col" >Ref. Apoyo</th>
                 <th>Acciones</th>
                 <th>Archivos</th>
                 <th>Turnos</th>
                 <th>Turnos</th>
                 </tr>
                 </thead> 
                 <tbody>
                 </HeaderTemplate>
                 <ItemTemplate>
                 <tr class="point" >
                  <td scope="row"><%#Eval("item")%></td>
                  <td scope="row"><%#Eval("aisv")%></td>
                  <td scope="row"><%# tipos(Eval("tipo"), Eval("movi"))%></td>
                  <td scope="row">
                    <a class="xinfo" >
                    <span class="xclass">
                    <h3>Referencia:</h3> <%#Eval("referencia")%>
                    <h3>FreightKind:</h3> <%#Eval("fk")%>
                    <h3>Puerto descarga:</h3> <%#Eval("pod")%>
                    <h3>Agencia:</h3> <%#Eval("agencia")%>
                    </span>
                        <%#Eval("boking")%>
                    </a>
                  </td>
                  <td scope="row"><%#Eval("fecha")%></td>
                  <td scope="row"> 
                        <%#Eval("tool")%>
                  </td>
                  
                  <td  scope="row"><%# anulado(Eval("estado"))%></td>
                     <td scope="row" >
                        <a href="editrefdata.aspx?sid=<%# securetext(Eval("aisv")) %>" target="_blank" onclick="popUpCal(this.href);  return false; "><%# refrigeracion(Eval("apoyo"), Eval("temperatura"))%></a>
                  </td>

                  <td scope="row">
                   <div class="tcomand" >
                       <a href="impresion.aspx?sid=<%# securetext(Eval("aisv")) %>"  target="_blank" class=" btn btn-link">Imprimir</a> | 
                       <div class='<%# boton( Eval("estado"))%>' >
                       <asp:Button ID="btanula"  
                       OnClientClick="return confirm('Esta seguro que desea eliminar este documento?');" 
                       CommandArgument=   '<%# jsarguments_cancela( Eval("aisv"), Eval("referencia"),Eval("cntr"),Eval("tipo"),Eval("movi"),Eval("estado"),Eval("vbs_id_hora_cita"), Eval("vbs_destino"),Eval("aisv_cant_bult") )%>' 
                        CommandName="Anular"   class="btn  btn-secondary" runat="server" Text="Anular" ToolTip="Permite anular este documento" />
                       </div>
                    
                          </div>
                  </td>
                   <td scope="row"> 
                       <%#Eval("archivo")%>  
                  </td>
                  <td scope="row"> 
                        <input type="button"   name="btnActualiza" onclick="actualizarTurno(this)" runat="server" value="Actualizar Cita"  class='btn btn-primary' style='<%#mostrarcontenedor(Eval("tipo"))%>' />  
                  </td>
                  <td scope="row">  
                          <asp:Button ID="BtnTurnoBanano" CommandArgument= '<%#Eval("aisv")%>' runat="server" Text="Actualizar Cita"  class='btn btn-primary' style='<%#mostrarbanano(Eval("tipo"))%>'
                        CommandName="actualizar_cita" data-toggle="modal" data-target="#exampleModalToggle"  />
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
     

        <div class="dashboard-container p-4" id="BodyModal" runat="server">

                   <div class="modal fade" id="modalActu" tabindex="-1" role="dialog" aria-labelledby="modal-event-label" aria-hidden="true" data-backdrop="static" data-keyboard="false">
        <div class="modal-dialog modal-xm" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="modal-Actu-label">Actualización de Citas creadas</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">

                    <div class=" form-row">
           
                    </div>

                    <div class="form-row">
                                     <div class="form-group col-md-3"> 
           <label for="inputAddress">Fecha Actual Cita:<span style="color: #FF0000; font-weight: bold;"></span></label>
               <asp:TextBox ID="txtFechaActualTurno" runat="server"  CssClass="form-control" 
            ClientIDMode="Static" disabled="disabled" ></asp:TextBox>
          </div>
                          <div class=" form-group col-md-2">
                        <label for="inputHorllgda">Hora Actual Cita:<span style="color: #FF0000; font-weight: bold;"></span></label>
               <asp:TextBox ID="txtHoraActualTurno" runat="server" 
                ClientIDMode="Static" CssClass="form-control" disabled="disabled" ></asp:TextBox>
                        </div>
                        <div class="form-group col-md-3"> 
           <label for="inputAddress">Fecha Nueva Cita:<span style="color: #FF0000; font-weight: bold;"></span></label>
               <asp:TextBox ID="txtFechaNuevoTurno" runat="server"  CssClass="datetimepicker form-control" 
             onkeypress="return soloLetras(event,'01234567890/')" 
                 onblur="valDate(this,true,valdate);" ClientIDMode="Static"></asp:TextBox>
          </div>
                          <div class=" form-group col-md-2">
                        <label for="inputHorllgda">Hora llegada Cita:<span style="color: #FF0000; font-weight: bold;"></span></label>
               <asp:TextBox ID="txtHorallegada" runat="server" 
                ClientIDMode="Static" CssClass="form-control"></asp:TextBox>
                        </div>
                       <div class="form-group col-md-2">
                               <label for="btnActualizar">Actualizar:<span style="color: #FF0000; font-weight: bold;"></span></label>
       
                             <button class="btn btn-primary" id="btnActualizar" <%--onclick="enviarDatosAlServidor()"--%> type="button">Actualizar</button>
                     
                       </div>
                        <div class=" form-group col-md-1">
                            <input runat="server" ID="txtAisv" hidden="hidden"/>

                        </div>
                         
                        <div class=" form-group col-md-1">
                            <input runat="server" ID="txtIdTurno" hidden="hidden"/>

                        </div>
                        <br />
                    </div>

                    <div class="row">
                           <div id="calendar"></div>
                    </div>
                         
                
              
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-dismiss="modal">Cerrar</button>
                </div>
            </div>
        </div>
    </div>
        </div>
     

     
  </div>


<div class="modal fade" id="exampleModalToggle" tabindex="-1" role="dialog" >
  <div class="modal-dialog modal-dialog-scrollable" style="max-width: 1200px">
    <div class="modal-content">
       <div class="modal-header">
           <asp:UpdatePanel ID="UPTITULO" runat="server"  UpdateMode="Conditional" ChildrenAsTriggers="true">
           <ContentTemplate>
                        <h5 class="modal-title" id="Titulo" runat="server">ACTUALIZAR TURNO</h5>                       
               </ContentTemplate>
         </asp:UpdatePanel>
            <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                        </button>
       </div>
      <div class="modal-body">

          <asp:UpdatePanel ID="UPACTUALIZAR" runat="server"  UpdateMode="Conditional" ChildrenAsTriggers="true">
          <ContentTemplate>
        
                 <div class="form-row">
                      <div class="form-group col-md-2"> 
                                <label for="inputEmail4">CANTIDAD UNIDADES</label>
                                 <div class="d-flex">
                                      <asp:TextBox ID="TxtUnidades" runat="server"  class="datetimepicker form-control"  MaxLength="10"  placeholder="HH:mm"  onkeypress="return soloLetras(event,'1234567890/:')" disabled></asp:TextBox>
                                     
                                 </div> 
                          </div> 
                      <div class="form-group col-md-2">

                          <label for="inputZip">DESTINO</label>
                              <asp:DropDownList runat="server" ID="CboDestino"    AutoPostBack="false"  class="form-control" disabled >
                                      <asp:ListItem Value="1">Muelle</asp:ListItem>
                                      <asp:ListItem Value="2">CGSA</asp:ListItem>
                                 </asp:DropDownList>
            
                        </div>
                          <div class="form-group col-md-2"> 
                                <label for="inputEmail4">FECHA TURNO</label>
                                 <div class="d-flex">
                                      <asp:TextBox ID="TxtFechaTurno" runat="server"  class="datetimepicker form-control"  MaxLength="10"  placeholder="dd/MM/yyyy"  onkeypress="return soloLetras(event,'1234567890/:')" disabled></asp:TextBox>
                                 </div> 
                          </div> 
                         <div class="form-group col-md-2"> 
                                <label for="inputEmail4">HORA INICIO </label>
                                 <div class="d-flex">
                                      <asp:TextBox ID="TxtHoraInicio" runat="server"  class="datetimepicker form-control"  MaxLength="10"  placeholder="HH:mm"  onkeypress="return soloLetras(event,'1234567890/:')" disabled></asp:TextBox>
                                 </div> 
                          </div> 
                         <div class="form-group col-md-2"> 
                                <label for="inputEmail4">HORA FIN </label>
                                 <div class="d-flex">
                                      <asp:TextBox ID="TxtHoraFin" runat="server"  class="datetimepicker form-control"  MaxLength="10"  placeholder="HH:mm"  onkeypress="return soloLetras(event,'1234567890/:')" disabled></asp:TextBox>
                                       <asp:TextBox ID="TxtReferencia" runat="server"  class="form-control" Visible="false"   disabled></asp:TextBox>
                                     <asp:TextBox ID="TxtNewAisv" runat="server"  class="form-control" Visible="false"   disabled></asp:TextBox>
                                     <asp:TextBox ID="TxtBox" runat="server"  class="form-control" Visible="false"   disabled></asp:TextBox>
                                 </div> 
                          </div> 
                 </div>
               
                <div class="form-title">
                      NUEVO TURNO
                 </div>
              <div class="form-row"> 
                 <div class="form-group col-md-2">
                    <label for="inputAddress">Fecha y hora de la cita <span style="color: #FF0000; font-weight: bold;">*</span></label>
                     <asp:UpdatePanel ID="UPFECHASALIDA" runat="server"  UpdateMode="Conditional" ChildrenAsTriggers="true" >
                    <ContentTemplate>
                     <script type="text/javascript">
                              Sys.Application.add_load(Calendario); 
                     </script>  
                      <asp:TextBox ID="TxtFechaHasta" runat="server"  class="form-control"  MaxLength="10" 
                         onkeypress="return soloLetras(event,'1234567890/:')"  AutoPostBack="true"  ontextchanged="TxtFechaHasta_TextChanged"></asp:TextBox>
                         <asp:CalendarExtender ID="CAGTFECHAFASA"  runat="server"
                                CssClass="red" Format="dd/MM/yyyy" TargetControlID="TxtFechaHasta">
                            </asp:CalendarExtender>
                        
                    </ContentTemplate>
                        <Triggers>
                             <asp:AsyncPostBackTrigger ControlID="TxtFechaHasta" />
                        </Triggers>
                    </asp:UpdatePanel> 
                </div>
                  <div class="form-group col-md-2">
                      <label for="inputAddress">&nbsp;</label>
                        <asp:UpdatePanel ID="UPHORA" runat="server"  UpdateMode="Conditional" ChildrenAsTriggers="true" >
                        <ContentTemplate>
                       <asp:DropDownList ID="Cbohora" runat="server" class="form-control"   AutoPostBack="true" OnSelectedIndexChanged="Cbohora_SelectedIndexChanged"  >
                       
                     </asp:DropDownList>
                              </ContentTemplate>
                        <Triggers>
                             <asp:AsyncPostBackTrigger ControlID="Cbohora" />
                        </Triggers>
                    </asp:UpdatePanel> 
                   </div>
                  <div class="form-group col-md-8">
                    <label for="inputZip">Nuevo turno disponible para la Cita:<span style="color: #FF0000; font-weight: bold;">*</span></label>       
                    <asp:UpdatePanel ID="UPTURNO" runat="server"  UpdateMode="Conditional"  >
                    <ContentTemplate>
                    <asp:DropDownList runat="server" ID="CboTurnos"   AutoPostBack="false"  class="form-control"  ></asp:DropDownList>
                    </ContentTemplate>
                    </asp:UpdatePanel> 
                     
                  

                </div>
               </div>
              <br/>
              <br/>
              <br/>
              <br/>
              <br/>
              <br/>

              <div class="row">  
                  <br/>
                   <div class="form-group col-md-12">
                            <div class="alert alert-warning" id="banmsg2" runat="server" clientidmode="Static"><b>Error!</b> >Debe ingresar el número de la carga MRN......</div>
                        </div> 
              </div>
       </ContentTemplate>
          <Triggers>
          <asp:AsyncPostBackTrigger ControlID="BtnActualizaTurno" />
        </Triggers>
    </asp:UpdatePanel>


        

      </div>
      <div class="modal-footer">
          <asp:UpdatePanel ID="UPBOTONES" runat="server" UpdateMode="Conditional" >  
             <ContentTemplate>
        <asp:Button ID="BtnActualizaTurno" runat="server"  class="btn btn-primary"  Text="ACTUALIZAR"  OnClientClick="return confirmacion()" OnClick="BtnAprobar_Click"  />    
                  </ContentTemplate>
             </asp:UpdatePanel>   
        <button type="button" class="btn btn-outline-primary " data-dismiss="modal">Cerrar</button>
        
      </div>
    </div>
  </div>
</div>

    <script type="text/javascript">
   
            function confirmacion()
            {
                var mensaje;
                var opcion = confirm("Estimado cliente, está seguro que desea actualizar el Turno. ?");
                if (opcion == true)
                {
                    loader();
                    return true;
                } else
                {
	                 return false;
                }
 
            }
    </script>

    <script type="text/javascript">



        function actualizarTurno(btn) {


            var row = btn.closest("tr"); // Obtener la fila padre del botón

            var estado = row.cells[6].textContent; // Obtener el texto del sexto elemento <td> en la fila (índice 6)
            var aisv = row.cells[1].textContent; // Obtener el texto del segundo elemento <td> en la fila (índice 1)
            if (estado == 'Anulado') {
                mostrarAdvertencia("No puede actualizar una Cita ya ANULADA")
            }
            else {

                mostrarModalFecha(aisv);

            }

        }



        function mostrarModalFecha(AISV) {
            // Crear el elemento del modal y el datepicker


            document.getElementById('<%=txtAisv.ClientID %>').value = AISV
            $('#txtHorallegada').val('');
            $('#txtIdTurno').val('');
            document.getElementById('<%=txtFechaNuevoTurno.ClientID %>').value = ''
            $('#calendar').fullCalendar('destroy');

            console.log("aisv", AISV)
            $.ajax({
                url: 'consulta.aspx/ConsultarFechaHoraAISV', // Reemplaza 'ruta_del_servidor' por la URL de tu servidor
                type: 'POST', // O 'GET' según tus necesidades
                data: JSON.stringify({ aisv: AISV }),// Datos que se enviarán al servidor
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                success: function (response) {
                    var responseData = JSON.parse(response.d);
                    var fechaLlegada = new Date(responseData.FechaLlegada);
                    var horaActualTurno = responseData.HoraLlegada;
                    var fechaFormateada = fechaLlegada.getDate() + '/' + (fechaLlegada.getMonth() + 1) + '/' + fechaLlegada.getFullYear();
                    console.log("fechallgeda",fechaLlegada)
                    console.log("hira", horaActualTurno)
                    console.log("formateada", fechaFormateada)
                    // Obtener la fecha y hora actual
                    var fechaActual = new Date();
                    var horaActual = fechaActual.getHours() + ':' + fechaActual.getMinutes();

                    // Comparar las fechas y horas
                    if (fechaLlegada > fechaActual || (fechaLlegada.getTime())) {
                        // Las fechas y horas son válidas, mostrar el modal y establecer los valores
                        $('#modalActu').modal('show');
                        document.getElementById('<%=txtFechaActualTurno.ClientID %>').value = fechaFormateada;
                        document.getElementById('<%=txtHoraActualTurno.ClientID %>').value = horaActualTurno;
                    } else {
                        // Las fechas y horas son inválidas, mostrar un mensaje de error o tomar alguna acción
                        mostrarAdvertencia('La fecha de llegada y hora de llegada del turno ya han pasado a la fecha actual.');
                    }
                },
                error: function (xhr, status, error) {

                }
            });

        }
        $('#txtFechaNuevoTurno').change(function () {
            var fechaSelecci = $(this).val();
            fechaSeleccionada(fechaSelecci);
        });

        function fechaSeleccionada(fecha) {

            if (fecha == '') {

            }
            else {
                var AISV = document.getElementById('<%=txtAisv.ClientID %>').value;


                // Obtener la fecha actual sin las horas
                var fechaActual = new Date();
                fechaActual.setHours(0, 0, 0, 0);

                // Convertir la fecha seleccionada en un objeto Date
                var partesFecha = fecha.split('/');
                var fechaSeleccionadaObj = new Date(partesFecha[2], partesFecha[1] - 1, partesFecha[0]);
                fechaSeleccionadaObj.setHours(0, 0, 0, 0);



                // Comparar las fechas
                if (fechaSeleccionadaObj < fechaActual) {
                    mostrarAdvertencia("La fecha seleccionada no debe ser menor a la actual");
                    // Realizar las acciones necesarias si la fecha es menor
                }

                else {
                    var anio = fechaSeleccionadaObj.getFullYear();
                    var mes = ('0' + (fechaSeleccionadaObj.getMonth() + 1)).slice(-2); // El mes comienza en 0, por lo que se suma 1
                    var dia = ('0' + fechaSeleccionadaObj.getDate()).slice(-2);

                    var fechaFormateada = anio + '-' + mes + '-' + dia;


                    // Realizar la consulta AJAX para obtener los eventos
                    $.ajax({
                        url: 'consulta.aspx/ConsultarEventosPorDiaAISV',
                        type: 'POST',
                        data: JSON.stringify({ start: fechaFormateada, varTodos: '', aisv: AISV }),
                        contentType: 'application/json; charset=utf-8',
                        dataType: 'json',
                        success: function (data) {
                            var jsonArray = JSON.parse(data.d);
                            var allTurnosZero = true;
                            var events = [];

                            for (var i = 0; i < jsonArray.length; i++) {
                                var evento = jsonArray[i];
                                var startDateTime = moment(fechaFormateada + ' ' + evento.horario, 'YYYY-MM-DD HH:mm:ss');
                                var endDateTime = startDateTime.clone().add(1, 'hour');
                                var color = evento.cantidad == 0 ? "#FF0000" : evento.color;
                                var event = {
                                    id: evento.idDetalle,
                                    //            title: evento.title,
                                    start: startDateTime,
                                    end: endDateTime,
                                    color: color,
                                    cantidad: evento.cantidad
                                };
                                events.push(event);

                                if (evento.cantidad !== 0) {
                                    allTurnosZero = false;
                     //   document.getElementById('<%=txtFechaNuevoTurno.ClientID %>').value = ''
                                }
                            }

                            // Verificar si todos los turnos son cero
                            if (allTurnosZero) {
                                mostrarConfirmacion('No tenemos Citas Disponibles para la fecha seleccionada, ¿Desea buscar nuevos datos?', function () {
                                    // Realizar una nueva consulta AJAX para obtener los nuevos eventos
                                    $.ajax({
                                        url: 'consulta.aspx/ConsultarEventosPorDiaAISV',
                                        type: 'POST',
                                        data: JSON.stringify({ start: fechaFormateada, varTodos: 'TODOS', aisv: AISV }),
                                        contentType: 'application/json; charset=utf-8',
                                        dataType: 'json',
                                        success: function (data) {
                                            var newEvents = [];
                                            var jsonArray = JSON.parse(data.d);
                                            var allTurnosZero = true;

                                            for (var i = 0; i < jsonArray.length; i++) {
                                                var evento = jsonArray[i];
                                                var startDateTime = moment(fechaFormateada + ' ' + evento.horario, 'YYYY-MM-DD HH:mm:ss');
                                                var endDateTime = startDateTime.clone().add(1, 'hour');
                                                var color = evento.cantidad === 0 ? "#FF0000" : evento.color;
                                                var event = {
                                                    id: evento.idDetalle,
                                                    //     title: evento.title,
                                                    start: startDateTime,
                                                    end: endDateTime,
                                                    color: color,
                                                    cantidad: evento.cantidad
                                                };
                                                newEvents.push(event);

                                                if (evento.cantidad !== 0) {
                                                    allTurnosZero = false;
                                //        document.getElementById('<%=txtFechaNuevoTurno.ClientID %>').value = ''
                                            }
                                        }

                                        // Actualizar el calendario con los nuevos eventos
                                        $('#calendar').fullCalendar('destroy');
                                        $('.fc-axis.fc-time.fc-widget-content').css('height', 60 + 'px');

                                        $('#calendar').fullCalendar({
                                            defaultView: 'agendaDay',
                                            defaultDate: fechaFormateada,
                                            header: {
                                                left: '',
                                                center: 'title',
                                                right: ''
                                            },
                                            editable: false,
                                            events: newEvents,
                                            displayEventTime: false,
                                            eventClick: function (calEvent, jsEvent, view) {
                                                var horaSeleccionada = calEvent.start.format('HH:mm');
                                                var idTurno = calEvent.id;
                                                var turnoDisponible = calEvent.cantidad;

                                                if (turnoDisponible == 0) {
                                                    mostrarAdvertencia("Turno que eligió ya no tiene disponibilidad, elija otra hora");
                                                } else {
                                                    mostrarConfirmacion('¿Está de acuerdo con seleccionar la hora ' + horaSeleccionada + '?', function () {
                                                        $('#txtHorallegada').val(horaSeleccionada);
                                                        $('#txtIdTurno').val(idTurno);
                                                        document.getElementById('<%=txtIdTurno.ClientID %>').value = idTurno;
                                                $('#txtAisv').val(AISV);
                                                $('#myModal').modal('hide');
                                            });
                                        }
                                    }
                                });

                                        // Realizar acciones adicionales si todos los turnos son cero
                                        if (allTurnosZero) {
                                            // ...
                                        }
                                    },
                                    error: function () {
                                        mostrarError('Error al realizar la consulta');
                                    }
                                });
                            }, function () {
                                $('#myModal').modal('hide'); // Cerrar el modal si el usuario selecciona "No"
                            });
                        } else {
                            // Crear el calendario con los eventos obtenidos
                            $('#calendar').fullCalendar('destroy');
                            $('.fc-axis.fc-time.fc-widget-content').css('height', 60 + 'px');

                            $('#calendar').fullCalendar({
                                defaultView: 'agendaDay',
                                defaultDate: fechaFormateada,
                                header: {
                                    left: '',
                                    center: 'title',
                                    right: ''
                                },
                                editable: false,
                                events: events,
                                displayEventTime: false,
                                eventClick: function (calEvent, jsEvent, view) {
                                    var horaSeleccionada = calEvent.start.format('HH:mm');
                                    var idTurno = calEvent.id;
                                    var turnoDisponible = calEvent.cantidad;

                                    if (turnoDisponible == 0) {
                                        mostrarAdvertencia("Cita que eligió ya no tiene disponibilidad, elija otra hora");
                                    } else {
                                        mostrarConfirmacion('¿Está de acuerdo con seleccionar la hora ' + horaSeleccionada + '?', function () {
                                            $('#txtHorallegada').val(horaSeleccionada);
                                            $('#txtIdTurno').val(idTurno);
                                            document.getElementById('<%=txtIdTurno.ClientID %>').value = idTurno;
                                    $('#txtAisv').val(AISV);
                                    $('#myModal').modal('hide');
                                });
                            }
                        }
                    });
                            }
                        },
                        error: function () {
                            mostrarError('Error al realizar la consulta');
                        }
                    });
                }
            }

        }

        var btnActualizar = document.getElementById('btnActualizar');
        btnActualizar.addEventListener('click', enviarSolicitudAJAX);

        function enviarSolicitudAJAX() {
            // Obtener los valores de los strings que deseas enviar

            var IdTurno = document.getElementById('<%=txtIdTurno.ClientID %>').value;
            var Fecha = document.getElementById('<%=txtFechaNuevoTurno.ClientID %>').value;
            var Horallegada = document.getElementById('<%=txtHorallegada.ClientID %>').value;
            var AISV = document.getElementById('<%=txtAisv.ClientID %>').value;

            // Crear el objeto de datos a enviar en la solicitud POST

        

            // Realizar la solicitud AJAX POST
            $.ajax({
                url: 'consulta.aspx/ActualizarTurnoPorAISV',
                type: 'POST',
                data: JSON.stringify({ idTurno: IdTurno, fecha: Fecha, horallegada: Horallegada, aisv: AISV }),
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                success: function (response) {
                    // Procesar la respuesta del servidor en caso de éxito
                    mostrarExito("Fecha y hora actualizados correctamente");
                    $('#modalActu').modal('hide');
                },
                error: function (error) {
                    // Procesar el error en caso de fallo en la solicitud AJAX
                    mostrarError("Ocurrio un error al Actualizar, vuelva a intentarlo");
                }
            });
        }





    </script>

    <script src="../Scripts/pages.js" type="text/javascript"></script>
     <script src="../Scripts/jquery.datetimepicker.js" type="text/javascript"></script>
    
    <script type="text/javascript">
        $(document).ready(function () {
            $('.datetimepicker').datetimepicker().datetimepicker({ lang: 'es', timepicker: false, closeOnDateSelect: true, format: 'd/m/Y' });
            //$('.datetimepicker').datetimepicker().datetimepicker({ lang: 'es', step: 30, format: 'd/m/Y H:i' });
        });

    </script>

    <script type="text/javascript">
       function Calendario() {
           $(document).ready(function () {
               $('.datetimepicker').datetimepicker().datetimepicker({ lang: 'es', timepicker: false, step: 30, format: 'd/m/Y' });
           });
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

    
       
    <script src="../Scripts/refrigerados.js" type="text/javascript"></script>
   
      <script type="text/javascript" src="../js/Confirmaciones.js""></script>
  </asp:Content>
