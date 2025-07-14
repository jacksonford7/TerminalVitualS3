<%@ Page Title="" Language="C#" MasterPageFile="~/site.Master" AutoEventWireup="true" CodeBehind="consultacertificadocfs_pesaje.aspx.cs" Inherits="CSLSite.consultacertificadocfs_pesaje" %>
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

     <link href="../css/jquery.notify.css" type="text/css" rel="stylesheet" />
	
	  <link href="../css/animate.min.css" type="text/css" rel="stylesheet" />

    <script type="text/javascript" src="../js/bootstrap-notify.min.js"></script>

<script type="text/javascript" src='https://maps.google.com/maps/api/js?key=AIzaSyA0f3IQRMX1fmn-35UxyLJSDvKv3BbKBhI&sensor=false&libraries=places'></script>
<script type="text/javascript" src="../maps/locationpicker.jquery.js"></script>

  <script type="text/javascript">

$(document).ready(function(){
  $('[data-toggle="tooltip"]').tooltip();   
});
</script>



    
  


    <style type="text/css">
      
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
           
            margin-bottom: 5px;
        }
    </style>
 


 
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="placebody" runat="server" >
    <asp:ToolkitScriptManager ID="tkscata" runat="server"></asp:ToolkitScriptManager>

  <div id="div_BrowserWindowName" style="visibility:hidden;">
    <asp:HiddenField ID="hf_BrowserWindowName" runat="server" />
     
  </div>
    <asp:HiddenField ID="manualHide" runat="server" />
      <asp:HiddenField ID="manualHideCarbono" runat="server" />

      <div class="mt-4">         
        <nav class="mt-4" aria-label="breadcrumb">
          <ol class="breadcrumb">
            <li class="breadcrumb-item" id="opcion_principal" runat="server"><a href="#">MULTIDESPACHO</a></li>
            <li class="breadcrumb-item active" aria-current="page" id="sub_opcion" runat="server">CERTIFICADO DE PESOS - CARGA SUELTA (CFS)</li>
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
           

       </div>
                        
        </ContentTemplate>
            <Triggers>
            <asp:AsyncPostBackTrigger ControlID="BtnBuscar" />
        </Triggers>
    </asp:UpdatePanel>
  
    
    <div class="form-row">  

        <div class="form-group col-md-12"> 
             <asp:UpdatePanel ID="UPPROFORMA" runat="server" UpdateMode="Conditional" >  
            <ContentTemplate>
                  <div class="alert alert-success" id="mensaje_proforma" runat="server" clientidmode="Static" visible="false"></div>
           </ContentTemplate>
             </asp:UpdatePanel>   
        </div>

     

     <div class="form-group col-md-12">
           

           <asp:UpdatePanel ID="UPTARJA" runat="server" UpdateMode="Conditional" >  
       <ContentTemplate>

             <h3 id="LabelTotal" runat="server">DETALLE DE ITEMS</h3>
          
            
              <div class="form-row">
                  <div class="form-group col-md-12">
                       <asp:Repeater ID="tablePagination_Tarja" runat="server"  >
                       <HeaderTemplate>
                       <table cellpadding="0" cellspacing="0" border="0" class="table table-bordered invoice" id="hidden-table-info-tarja">
                           <thead>
                          <tr>
                            
                            <th class="center hidden-phone"># CERTIFICADO</th>
                            <th class="center hidden-phone">CANTIDAD</th>
                            <th class="center hidden-phone">ALTO</th>
                            <th class="center hidden-phone">ANCHO</th>
                            <th class="center hidden-phone">LARGO</th>
                            <th class="center hidden-phone">PESO</th>
                            <th class="center hidden-phone">VOLUMEN</th>
                           
                          </tr>
                        </thead>
                        <tbody>
                        </HeaderTemplate>
                        <ItemTemplate>
                          
                                <td class="center hidden-phone"><asp:Label Text='<%#Eval("NUMERO_CERTIFICADO")%>' ID="LblConsecutivo" runat="server"  /></td>
                                <td class="center hidden-phone"><asp:Label Text='<%#Eval("CANTIDAD")%>' ID="LblCantidad" runat="server"  /></td>
                                <td class="center hidden-phone"><asp:Label Text='<%#Eval("P2D_ALTO")%>' ID="LblEmpresa" runat="server"  /> </td>
                                <td class="center hidden-phone"><asp:Label Text='<%#Eval("P2D_ANCHO")%>' ID="LblChofer" runat="server"  />  </td> 
                                <td class="center hidden-phone"><asp:Label Text='<%#Eval("P2D_LARGO")%>' ID="LblPlaca" runat="server"  /> </td> 
                                <td class="center hidden-phone"><asp:Label Text='<%#Eval("PESO")%>' ID="LblEstado" runat="server"  /> </td> 
                                <td class="center hidden-phone"><asp:Label Text='<%#Eval("P2D_VOLUMEN")%>' ID="Label1" runat="server"  /> </td> 
                              
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
                   <asp:Button ID="BtnVer" runat="server" class="btn btn-primary" Text="VISUALIZAR CERTIFICADO" OnClientClick="mostrar();"  /> &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                  <asp:Button ID="BtnDescargar" runat="server" class="btn btn-primary" Text="DESCARGAR CERTIFICADO" OnClientClick="descarga();"   />
               </div> 
             </div>
            </ContentTemplate>
             </asp:UpdatePanel>   
   
    
    
</div>



     

 


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


        function mostrar()
        {
            try {

                var url = '../multidespachoscfs/certificadocfs_pesaje.aspx?mrn=';

                var mrn = this.document.getElementById('<%= TXTMRN.ClientID %>').value;
                var msn = this.document.getElementById('<%= TXTMSN.ClientID %>').value;
                var hsn = this.document.getElementById('<%= TXTHSN.ClientID %>').value;

                url = url + mrn + '&msn=' + msn + '&hsn=' + hsn;


                var w = window.open(url, 'Vista preliminar', 'width=850,height=900');
                w.focus();
            }
            catch (e) {
                alert('Lamentamos comunicarle que ha ocurrido el siguiente problema:\n' + e.Message);
            }
        }


        function descarga()
        {
           try {
            var url = '../handler/pesoscfs.ashx?mrn=';

            var mrn = this.document.getElementById('<%= TXTMRN.ClientID %>').value;
            var msn = this.document.getElementById('<%= TXTMSN.ClientID %>').value;
            var hsn = this.document.getElementById('<%= TXTHSN.ClientID %>').value;

            url = url + mrn + '&msn=' + msn + '&hsn=' + hsn;

            var iframe = document.createElement("iframe");
            iframe.src = url;
            iframe.style.display = "none";
            document.body.appendChild(iframe);
           }
            catch (e) {
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