<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="booking.aspx.cs" Inherits="CSLSite.booking" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">  
    <title>Catálogo de bookings</title>
    <link href="../shared/estilo/info.css" rel="stylesheet" />
   <link href="../img/favicon2.png" rel="icon"/>
  <link href="../img/icono.png" rel="apple-touch-icon"/>
  <link href="../css/bootstrap.min.css" rel="stylesheet"/>
  <link href="../css/dashboard.css" rel="stylesheet"/>
  <link href="../css/icons.css" rel="stylesheet"/>
  <link href="../shared/estilo/aisv-general.css" rel="stylesheet" type="text/css" />
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

    <script src="../Scripts/pages.js" type="text/javascript"></script>
   <script  type="text/javascript">
      function BindFunctions(){
          document.getElementById('imagen').innerHTML = "";
       }
   </script>

</head>
<body>
 
    <form id="bookingfrm" runat="server">
    <asp:ToolkitScriptManager ID="tkscata" runat="server"></asp:ToolkitScriptManager>
      <div class="dashboard-container p-4" id="cuerpo" runat="server">
         <div class="form-title">Datos del documento buscado</div>
		  <div class="form-row">
		   <div class="form-group col-md-12"> 
		   	  <label for="inputAddress">Numero de booking<span style="color: #FF0000; font-weight: bold;"></span></label>
			  <div class="d-flex">
                      <asp:TextBox ID="txtfinder" runat="server"  CssClass="form-control" 
                 onkeypress="return soloLetras(event,'abcdefghijklmnñopqrstuvwxyz1234567890-_')" 
                 MaxLength="50"  onkeyup="msgfinder(this,'valintro','Escriba un número de booking y pulse buscar');" ></asp:TextBox>  
             <asp:Button ID="find" runat="server"  CssClass="btn btn-primary"
                 Text="Buscar" onclick="find_Click" OnClientClick="return initFinder();" />
             <span id="imagen" class="xax"></span>

			  </div>
		   </div>
		  </div>
            <div class="form-row">
		   <div class="form-group col-md-12"> 
		   	  <p class="  alert alert-light" id="valintro" runat="server">Escriba un número de booking y pulse buscar</p>
		   </div>
		  </div>

         <div class="cataresult" >
             <asp:UpdatePanel ID="upresult" runat="server"  >
             <ContentTemplate>
             <script type="text/javascript">Sys.Application.add_load(BindFunctions); </script>

             <div id="xfinder" runat="server" visible="false" >
                <div class="form-row">
		  
		   <div class="form-group col-md-12"> 
			       <div class="alert alert-warning" id="alerta" >
                 Confirme que los datos ingresados sean correctos.  En caso de error, por favor notifíquelo a las casilla ec.sac@contecon.com.ec o comuníquese a los teléfonos (04) 6006300 – 3901700 opción 4	
                </div>
		   </div>
		  </div>
               <div class="form-title">Booking</div>
                 
                   <div class="form-row">
		  
		   <div class="form-group col-md-3"> 
		   	  <label for="inputAddress">Número<span style="color: #FF0000; font-weight: bold;"></span></label>
			  <span id="numero" runat="server" class=" form-control col-md-12">REM00006689</span>
		   </div>
		    <div class="form-group col-md-3"> 
		   	  <label for="inputAddress">FreightKind<span style="color: #FF0000; font-weight: bold;"></span></label>
			  <span id="fk" runat="server" class=" form-control col-md-12">FCL</span>
		   </div>
                          <div class="form-group col-md-3"> 
		   	  <label for="inputAddress">Referencia<span style="color: #FF0000; font-weight: bold;"></span></label>
			  <span class=" form-control col-md-12" id="referencia" runat="server"></span>
		   </div>
                          <div class="form-group col-md-3"> 
		   	  <label for="inputAddress">IMO<span style="color: #FF0000; font-weight: bold;"></span></label>
			  <span class=" form-control col-md-12" id="imo" runat="server"></span>
		   </div>
                       
                   </div>

                   <div class="form-row">
		  
		   <div class="form-group col-md-3"> 
		   	  <label for="inputAddress">Nave<span style="color: #FF0000; font-weight: bold;"></span></label>
			 <span class="  form-control col-md-12" id="nave" runat="server"></span>
		   </div>
                         <div class="form-group col-md-3"> 
		   	  <label for="inputAddress">Viaje<span style="color: #FF0000; font-weight: bold;"></span></label>
			  <span class=" form-control col-md-12" id="viaje" runat="server"></span>
		   </div>
                         <div class="form-group col-md-3"> 
		   	  <label for="inputAddress">Fecha de atraque [ETA]<span style="color: #FF0000; font-weight: bold;"></span></label>
			 <span id="eta" runat="server" class="  form-control col-md-12"> </span>
		   </div>
  <div class="form-group col-md-3"> 
		   	  <label for="inputAddress">Reefer<span style="color: #FF0000; font-weight: bold;"></span></label>
			    <span class="form-control col-md-12" id="refer" runat="server">NO</span>
		   </div>
		  </div>

                   <div class="form-row">
		  
		   <div class="form-group col-md-3"> 
		   	  <label for="inputAddress">Fecha límite [CutOff]<span style="color: #FF0000; font-weight: bold;"></span></label>
			     <span id="cutoff" runat="server" class=" form-control col-md-12">         01/01/2014 10:00</span>
		   </div>
                         <div class="form-group col-md-3"> 
		   	  <label for="inputAddress">Puerto de descarga<span style="color: #FF0000; font-weight: bold;"></span></label>
			  <span class=" form-control col-md-12" id="pod" runat="server">BEANR</span>
		   </div>
                         <div class="form-group col-md-3"> 
		   	  <label for="inputAddress">Producto declarado<span style="color: #FF0000; font-weight: bold;"></span></label>
			   <span class="form-control col-md-12" id="comoditi" runat="server">Producto DUMMY</span>
		   </div>
                         <div class="form-group col-md-3"> 
		   	  <label for="inputAddress">Puerto descarga final<span style="color: #FF0000; font-weight: bold;"></span></label>
			   <span class="form-control col-md-12" id="pod1" runat="server">BEANR2</span>
		   </div>
		  </div>
                  <div class="form-title">Item de Booking</div>
             <div class="booking" >
                        <input id="oversize" type="hidden" runat="server" />
                      <input id="notas" type="hidden"  runat="server"/>
                 <asp:Repeater ID="tablePagination" runat="server" >
                 <HeaderTemplate>
                 <table id="tablasort" cellspacing="1" cellpadding="1" class="table table-bordered invoice">
                 <thead>
                 <tr>
                 <th>No.</th>
                 <th>ISO</th>
                 <th>Temp°C</th>
                 <th>Reservado</th>
                 <th>Ingresados</th>
                 <th>Disponible</th>
                 <th>&nbsp;</th>
                 <th class="xax">&nbsp;</th>
                 <th class="xax">&nbsp;</th>
                 <th class="xax">&nbsp;</th>
                 <th class="xax">&nbsp;</th>
                 <th class="xax">&nbsp;</th>
                 <th class="xax">&nbsp;</th>
                 <th class="xax">&nbsp;</th>
                 <th class="xax">&nbsp;</th>
                 <th class="xax">&nbsp;</th>
                 <th class="xax">&nbsp;</th>
                 <th class="xax">&nbsp;</th>
                 <th class="xax">&nbsp;</th>
                 <th class="xax">&nbsp;</th>
                 <th class="xax">&nbsp;</th>
                 <th class="xax">&nbsp;</th>
                 </tr>
                 </thead> 
                 <tbody>
                 </HeaderTemplate>
                 <ItemTemplate>
                 <tr class= '<%# getClass( Eval("dispone"),Eval("bfk"),Request.QueryString["V"]) %>' onclick="setBooking(this);">
                  <td><%#Eval("bitem")%></td>
           <td>
               <a class="xinfo" >
                   <span class="xclass"><%#Eval("iso_descrip")%>  </span>
                    <%#Eval("biso")%>
                    </a>
                   </td>
            
            
                     <td><%#Eval("btemp")!=null?Eval("btemp").ToString().Replace(',','.'):"0.0"%></td>
                  <td><%#Eval("reserva")%></td>
                  <td><%#Eval("ingresa")%></td>
                  <td><%#Eval("dispone")%></td>
                  <td><a href="#" class=" btn btn-link" >Elegir</a></td>
                  <td class="xax"><%#Eval("gkey")%></td>
                  <td class="xax"><%#Eval("usize")%></td>
                  <td class="xax"><%#Eval("biso")%></td>
                  <td class="xax"><%#Eval("utara")%></td>
                  <td class="xax"><%#Eval("bimo")%></td>
                  <td class="xax"><%#Eval("breefer")%></td>
                  <td class="xax"><%#Eval("shiperid")%></td>
                  <td class="xax"><%#Eval("shipname")%></td>
                  <td class="xax"><%#Eval("bline")%></td>
                  <td class="xax"><%#Eval("hzkey")%></td>
                  <td class="xax"><%#Eval("aqt")%></td>
                  <td class="xax"><%#Eval("vent_pc")%></td>
                  <td class="xax"><%#Eval("ventu")%></td>
                  <td class="xax"><%#Eval("hume")%></td>
                  <td class="xax"><%#Eval("temp")%></td>
                 </tr>
                  </ItemTemplate>
                 <FooterTemplate>
                 </tbody>
                 </table>
                 </FooterTemplate>
         </asp:Repeater>
                
             </div>
             <div class=" alert  alert-secondary" id="propiedad" runat="server" >Mensaje</div>

              </div>
             <div id="sinresultado" runat="server" class=" alert  alert-primary">
             No se encontraron resultados, 
             asegurese que el booking que busca sea correspondiente al AISV, AISV Contenedores-> Booking FCL, 
             AISV Carga suelta-> Booking BBULK,AISV Consolidación-> Booking LCL.
             y pertenecientes a la línea seleccionada  </div>
             </ContentTemplate>
             <Triggers>
             <asp:AsyncPostBackTrigger ControlID="find" />
             </Triggers>
             </asp:UpdatePanel>
       </div>
  </div>
    </form>
       <script src="../lib/jquery/jquery.min.js" type="text/javascript"></script>
   <script type="text/javascript" >
       function setBooking(row) {
           var str = row.getElementsByTagName('td');
           var strda = document.getElementById('cutoff').textContent;
           if(strda == null || strda == undefined  || strda.trim().length<=0 ) 
           {
             alertify.alert('El booking no tiene fecha de cutoff!');
             return;
           }
            var datePart = strda.split('/');
            var fecha = new Date(datePart[1] + '/' + datePart[0] + '/' + datePart[2]);
           fecha.setHours(fecha.getHours() - 12);
           var bkitem = {
               numero: document.getElementById('numero').textContent,
               referencia: document.getElementById('referencia').textContent,
               fk: document.getElementById('fk').textContent,
               nave: document.getElementById('nave').textContent,
               eta: document.getElementById('eta').textContent,
               cutoff: document.getElementById('cutoff').textContent,
               pod: document.getElementById('pod').textContent,
               pod1: document.getElementById('pod1').textContent,
               comoditi: document.getElementById('comoditi').textContent,
               imo: document.getElementById('imo').textContent,
               remark: document.getElementById('notas').value,
               uis : formattedDate(fecha,true),
               //Cabecera ok.
               item: str[0].textContent,
               temp: str[2].textContent ,
               reserva: str[3].textContent ,
               ingresa: str[4].textContent,
               dispone: str[5].textContent ,
               gkey: str[7].textContent,
               longitud: str[8].textContent,
               iso:  str[9].textContent,
               tara:  str[10].textContent.length > 0? (parseFloat(str[10].textContent)/1000).toString():'0',
               bimo: str[11].textContent.length > 0 ? true:false,
               refer: str[12].textContent.length >= 0 && str[12].textContent.trim() == '1' ? true : false,
               shipid: str[13].textContent,
               shipname:str[14].textContent,
               bline:str[15].textContent,
               hzkey:str[16].textContent,
               aqt:str[17].textContent,
             
               vent_pc:str[18].textContent,
               ventu:str[19].textContent,
               hume: str[20].textContent,
               ata: str[21].textContent
           };
           if (bkitem.fk !='LCL' && bkitem.dispone <= 0) {
               alertify.alert('El Item elegido ya no tiene disponibilidad, favor consultar con su agencia naviera');
               return;
           }
           if (window.opener != null) {
                window.opener.validateBook(bkitem)
            }
            self.close();
        }
       function initFinder() {
           if (document.getElementById('txtfinder').value.trim().length <= 0) {
               alertify.alert('Escriba una o varias letras para iniciar la búsqueda');
               return false;
           }
           document.getElementById('imagen').innerHTML = '<img alt="" src="../shared/imgs/loader.gif">';
       }
   </script>
</body>
</html>
