<%@ Page Title="Refeers" Language="C#" MasterPageFile="~/site.Master" 
         AutoEventWireup="true" CodeBehind="registro_inspeccion.aspx.cs" Inherits="CSLSite.registro_inspeccion" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="placehead" runat="server">
    <link href="../shared/estilo/tables.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/aisv-general.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/work.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/catalogos.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/info.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/turnos.js" type="text/javascript"></script>

    <link href="../img/favicon2.png" rel="icon"/>
    <link href="../img/icono.png" rel="apple-touch-icon"/>
    <link href="../css/bootstrap.min.css" rel="stylesheet"/>
    <link href="../css/dashboard.css" rel="stylesheet"/>
    <link href="../css/icons.css" rel="stylesheet"/>
    <link href="../css/style.css" rel="stylesheet"/>
    <link href="https://fonts.googleapis.com/css2?family=Roboto:wght@300;400;500;700&display=swap" rel="stylesheet"/>

    <link href="../shared/estilo/Reset.css" rel="stylesheet" />
    <link href="../shared/estilo/jquery.datetimepicker.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/tables.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/info.css" rel="stylesheet" type="text/css" />


     <link href="../shared/estilo/jquery.datetimepicker.css" rel="stylesheet" type="text/css" />
   <script src="../Scripts/jquery.datetimepicker.js" type="text/javascript"></script>

    
<script type="text/javascript">


 function fechas()
   {
    $(document).ready(function () {
            $('.datetimepicker').datetimepicker().datetimepicker({ lang: 'es', step: 30, format: 'd/m/Y H:i' });
        });
      
        $(document).ready(function () {
            $('.datetimepickerAlt').datetimepicker().datetimepicker({ lang: 'es', closeOnDateSelect: true, timepicker: false, step: 30, format: 'd/m/Y' });
        });
     
        $(document).ready(function () {
            $('.datepicker').datetimepicker().datetimepicker({ lang: 'es', timepicker: false, format: 'd/m/Y', closeOnDateSelect: true, minDate: '0' });

        });

    }


</script>
 

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
 #aprint {
 	     color: #666;    
	     border: 1px solid #ccc;    
	     -moz-border-radius: 3px;    
	     -webkit-border-radius: 3px;    
	     background-color: #f6f6f6;    
	     padding: 0.3125em 1em;    
	     cursor: pointer;   
	     white-space: nowrap;   
	     overflow: visible;   
	     font-size: 1em;    
	     outline: 0 none /* removes focus outline in IE */;    
	     margin: 0px;    
	     line-height: 1.6em;    
	     background-image: url(../shared/imgs/action_print.gif);
	     background-repeat: no-repeat;
	     background-position:left center;
	     text-decoration:none;
	     padding:5px 2px 5px 30px;
	  
}
    </style>

    <script type="text/javascript" src="../lib/jquery/jquery-3.5.1.slim.min.js" integrity="sha384-DfXdz2htPH0lsSSs5nCTpuj/zy4C+OGpamoFVy38MVBnE+IbbVYUew+OrCXaRkfj" crossorigin="anonymous"></script>
    <script src="../Scripts/pages.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.tablesorter.min.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.tablesorter.pager.js" type="text/javascript"></script>

    <script src="../Scripts/jquery.datetimepicker.js" type="text/javascript"></script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="placebody" runat="server">
    <input id="zonaid" type="hidden" value="610" />
     <input id="ruta_completa" type="hidden" value="" runat="server" clientidmode="Static" />
     <input id="id_linea" type="hidden" value="" runat="server" clientidmode="Static" />

    <asp:ToolkitScriptManager ID="tkcal" EnableScriptGlobalization="true" runat="server"> </asp:ToolkitScriptManager>

    <div class="mt-4">         
        <nav class="mt-4" aria-label="breadcrumb">
            <ol class="breadcrumb">
            <li class="breadcrumb-item" id="opcion_principal" runat="server"><a href="#">Inspección</a></li>
                <li class="breadcrumb-item active" aria-current="page" id="sub_opcion" runat="server">Registro de Inspección de Buques</li>
            </ol>
        </nav>
    </div>
  <div class="dashboard-container p-4" id="cuerpo" runat="server">
      
           <input id="agencia" type="hidden" runat="server"  clientidmode="Static"/>
        <div class="form-title">
            Filtros para el reporte
        </div>
        
        <div class="form-row">
            <div class="form-group col-md-3"> 
                <label for="inputAddress">Referencia de Nave:<span style="color: #FF0000; font-weight: bold;"></span></label>
                <div class="d-flex"> 
                    <asp:TextBox  class="form-control"  ID="tbooking" runat="server" disabled></asp:TextBox>&nbsp;
                      
                </div>
            </div>
            <div class="form-group col-md-5"> 
                <label for="inputAddress">&nbsp;<span style="color: #FF0000; font-weight: bold;"></span></label>
                 <div class="d-flex">
                      <asp:TextBox  class="form-control"  ID="txtNaveDescrip" runat="server"  disabled></asp:TextBox>
                    <a class="btn btn-outline-primary mr-4" style='font-size:24px' target="popup" onclick="window.open('../inspeccion_buques/inspeccion_naves.aspx','name','width=950,height=850')">
                            <span class='fa fa-search' style='font-size:24px'></span> 
                    </a>
                 </div>
            </div>
            <div class="form-group col-md-4">  </div>
            
            <div class="form-group col-md-6"> 
                <label for="inputZip">LÍNEA NAVIERA</label>
               <asp:TextBox ID="TxtLineaNaviera" runat="server" class="form-control"  disabled 
                                placeholder=""></asp:TextBox>
                  
            </div>
             <div class="form-group col-md-6"> 
                <label for="inputZip">REFERENCIA</label>
               <asp:TextBox ID="TxtReferencia" runat="server" class="form-control"  disabled 
                                placeholder=""></asp:TextBox>
            </div>
             <div class="form-group col-md-6"> 
                <label for="inputZip">BANDERA</label>
               <asp:TextBox ID="TxtBandera" runat="server" class="form-control"  disabled 
                                placeholder=""></asp:TextBox>
            </div>
            <div class="form-group col-md-6"> 
                <label for="inputAddress">FECHA Y HORA (INSPECCIÓN)<span style="color: #FF0000; font-weight: bold;"></span></label>
                <div class="d-flex"> 
                    <asp:TextBox 
                       
                        ID="tfechaini" runat="server" ClientIDMode="Static" MaxLength="15" class="datetimepicker form-control"
                        placeholder="dd/MM/yyyy HH:mm"  onkeypress="return soloLetras(event,'1234567890/:')"
                        >
                    </asp:TextBox>
                    <span id="valfechaini" class="validacion" ></span>                   
                </div>
            </div>

            <div class="form-group col-md-6"> 
                <label for="inputAddress">OBSERVACIONES:<span style="color: #FF0000; font-weight: bold;"></span></label>
                <div class="d-flex"> 
                    <asp:TextBox 
                                
                                ID="TxtObservacion" runat="server" ClientIDMode="Static" MaxLength="250" CssClass="form-control" TextMode="MultiLine" Height="150px"
                             >
                    </asp:TextBox>
                    <span id="valfechafin" class="validacion" ></span>
                </div>
            </div>

            <div class="form-group col-md-6" > 
                          <label for="inputEmail4">ARCHIVO PDF DE INSPECCIÓN:</label>
                          <div class="d-flex">
                          <asp:TextBox ID="TxtRuta1" runat="server"   class="form-control" ClientIDMode="Static" disabled ></asp:TextBox>
                                 <a  class="btn btn-outline-primary mr-4" runat="server" id="BtnArchivos" 
                                     target ="popup"  onclick="subirpdf();"   >
                                <span class='fa fa-search' style='font-size:24px' ></span></a>
                        </div>
		           </div>
            

           
        </div>
         
        <div ><br /></div>
        <div class="row">
            <div class="col-md-12 d-flex justify-content-center">

                <asp:Button ID="BtnNuevo" runat="server" class="btn btn-outline-primary mr-4"   Text="NUEVA TRANSACCION"  OnClick="BtnNuevo_Click"  />
                <asp:Button ID="btbuscar" runat="server" Text="Registrar Inspección"  class="btn btn-primary"   onclick="btbuscar_Click" />
               
                <span id="imagen"></span>
            </div>
        </div>
      <br/>
        <div class="form-row">
            <div class="form-group col-md-12"> 
             <div class="cataresult" >
               <asp:UpdatePanel ID="upresult" runat="server" ChildrenAsTriggers="true">
                     <ContentTemplate>
                     
             <div class="alert alert-warning" id="alerta" runat="server" ></div>
                  
               <div id="sinresultado" runat="server" class="alert alert-info"></div>
                  </ContentTemplate>
                     <Triggers>
                         <asp:AsyncPostBackTrigger ControlID="btbuscar" />
                     </Triggers>
                 </asp:UpdatePanel>
             </div>
            </div>
        </div>
  </div>

    <script type="text/javascript" src="../lib/jquery/jquery-3.5.1.slim.min.js" integrity="sha384-DfXdz2htPH0lsSSs5nCTpuj/zy4C+OGpamoFVy38MVBnE+IbbVYUew+OrCXaRkfj" crossorigin="anonymous"></script>
    <script src="../Scripts/pages.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.tablesorter.min.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.tablesorter.pager.js" type="text/javascript"></script>

   
  
    <script type="text/javascript">
      

        function popupCallback(catalogo) {
            if (catalogo == null || catalogo == undefined) {
                alert('Hubo un problema al setaar un objeto de catalogo');
                return;
            }
            //alert(catalogo);
            this.document.getElementById('<%= tbooking.ClientID %>').value = catalogo.codigo;
            this.document.getElementById('<%= txtNaveDescrip.ClientID %>').value = catalogo.descripcion;
            this.document.getElementById('<%= TxtReferencia.ClientID %>').value = catalogo.codigo;
            this.document.getElementById('<%= TxtBandera.ClientID %>').value = catalogo.bandera;
            this.document.getElementById('<%= TxtLineaNaviera.ClientID %>').value = catalogo.line;
          
             this.document.getElementById("id_linea").value = catalogo.id_line; 
        }

       function limpiar() {
          
            this.document.getElementById('<%= tbooking.ClientID %>').value = "";
            this.document.getElementById('<%= txtNaveDescrip.ClientID %>').value = "";
            this.document.getElementById('<%= TxtReferencia.ClientID %>').value = "";
            this.document.getElementById('<%= TxtBandera.ClientID %>').value = "";
           this.document.getElementById('<%= TxtLineaNaviera.ClientID %>').value = "";
           this.document.getElementById('<%= tfechaini.ClientID %>').value = "";
           this.document.getElementById('<%= TxtObservacion.ClientID %>').value = "";
           this.document.getElementById('<%= TxtRuta1.ClientID %>').value = "";
         
        }



    </script>

    
<script type="text/javascript">

  $(document).ready(function () {
            $('.datetimepicker').datetimepicker().datetimepicker({ lang: 'es', step: 30, format: 'd/m/Y H:i' });
        });
      
        $(document).ready(function () {
            $('.datetimepickerAlt').datetimepicker().datetimepicker({ lang: 'es', closeOnDateSelect: true, timepicker: false, step: 30, format: 'd/m/Y' });
        });
     
        $(document).ready(function () {
            $('.datepicker').datetimepicker().datetimepicker({ lang: 'es', timepicker: false, format: 'd/m/Y', closeOnDateSelect: true, minDate: '0' });

        });


</script>

<script type="text/javascript">

            function subirpdf()
            {
               try {
                    var w = window.open('../inspeccion_buques/archivo_inspeccion.aspx', 'Archivos', 'width=850,height=400');
                     w.focus();     
               }
               catch (e) {
                    alertify.alert('ERROR',e.Message  ).set('label', 'Reportar');
                }
            }

          function popupCallback_Archivo(lookup_archivo)
          {
     
               if (lookup_archivo.sel_Ruta != null )
               {
                    this.document.getElementById('<%= TxtRuta1.ClientID %>').value = lookup_archivo.sel_Nombre_Archivo1;
                    this.document.getElementById("ruta_completa").value = lookup_archivo.sel_Ruta; 
                }
          
          } 
 </script>



<asp:updateprogress  id="updateProgress" runat="server">
    <progresstemplate>
        <div id="progressBackgroundFilter"></div>
        <div id="processMessage"> Estamos procesando la tarea que solicitó, por favor  espere...<br />
         <img alt="Loading" src="../shared/imgs/loader.gif" style="margin:0 auto;" />
        </div>
    </progresstemplate>
</asp:updateprogress>
  </asp:Content>