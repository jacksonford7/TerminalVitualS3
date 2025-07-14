<%@ Page Language="C#" Title="Registro de Empresa" MaintainScrollPositionOnPostback="true"  AutoEventWireup="true" CodeBehind="solicitudempresa.aspx.cs" Inherits="CSLSite.cliente.solicitudempresa" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<%@ Register Assembly="MSCaptcha" Namespace="MSCaptcha" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">

    <link href="../img/favicon2.png" rel="icon"/>
  <link href="../img/icono.png" rel="apple-touch-icon"/>
  <link href="../css/bootstrap.min.css" rel="stylesheet"/>
  <link href="../css/dashboard.css" rel="stylesheet"/>
  <link href="../css/icons.css" rel="stylesheet"/>
  <link href="../css/style.css" rel="stylesheet"/>
  <link href="https://fonts.googleapis.com/css2?family=Roboto:wght@300;400;500;700&display=swap" rel="stylesheet"/>

     <!--mensajes-->
        <link href="https://cdn.jsdelivr.net/npm/alertifyjs@1.11.0/build/css/alertify.min.css" rel="stylesheet"/>
        <script type="text/javascript" src="https://cdn.jsdelivr.net/npm/alertifyjs@1.11.0/build/alertify.min.js"></script>
        <script src="../lib/jquery/jquery.min.js" type="text/javascript"></script>

   <%--   <link href="../shared/estilo/Reset.css" rel="stylesheet" />
 
     <link href="../shared/estilo/tables.css" rel="stylesheet" type="text/css" />
     <link href="../shared/estilo/info.css" rel="stylesheet" type="text/css" />

    <link href="../shared/estilo/jquery.datetimepicker.css" rel="stylesheet" type="text/css" />
   <script src="../Scripts/jquery.datetimepicker.js" type="text/javascript"></script>--%>


       <style type="text/css">
        body2
        {
            font-family: Arial;
            font-size: 10pt;
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
            width: 506px;
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
            line-height: 30px;
            text-align: center;
            font-weight: bold;
            margin-bottom: 5px;
        }
    </style>

</head>
<body>
<div class="dashboard-container p-4" id="cuerpo" runat="server">
     
    <div class="mt-4">         
        <nav class="mt-4" aria-label="breadcrumb">
          <ol class="breadcrumb">
            <li class="breadcrumb-item" id="opcion_principal" runat="server"><a href="#">MCA - Módulo de Control de Acceso</a></li>
            <li class="breadcrumb-item active" aria-current="page" id="sub_opcion" runat="server">Formulario de Registro de Empresa</li>
          </ol>
        </nav>
    </div>

    <form id="bookingfrm" runat="server">
        
    <input id="zonaid" type="hidden" value="7" />
    <asp:ToolkitScriptManager ID="tkscata" runat="server"></asp:ToolkitScriptManager>

     <asp:HiddenField ID="manualHide" runat="server" />


     <div class="form-row">

              <div class="form-group col-md-12">
             <a class="btn btn-outline-primary mr-4"  runat="server" id="aprint" clientidmode="Static" >1</a>
             <a class="level1"  runat="server" id="a1" clientidmode="Static" >Tipo de Cliente - Empresa</a>
             </div>

             <div class="form-group col-md-12" >
                 <label for="inputAddress">1. Tipo de Cliente:<span style="color: #FF0000; font-weight: bold;"> *</span></label>
            </div>
            <div class="form-group col-md-12" style=" height:150px; overflow-y:scroll;">  <%--overflow:auto--%>
                
                    <asp:CheckBoxList ID="cbltipousuario" runat="server"  onchange="valcbl();"></asp:CheckBoxList>
                     
            </div>
     </div>
    
    <div class="form-row">

            <div class="form-group col-md-12">
                <a class="btn btn-outline-primary mr-4"  runat="server" id="a4" clientidmode="Static" >2</a>
                <a class="level1"  runat="server" id="a5" clientidmode="Static" >Información del Cliente</a>
            </div>

            <div class="form-group col-md-6"> 
                <label for="inputZip">2. Nombre/Razón Social:<span style="color: #FF0000; font-weight: bold;">*</span></label>
                <asp:TextBox ID="txtrazonsocial" runat="server" MaxLength="500" onblur="checkcajalarge(this,'valrazsocial',true);"  class="form-control"
                 onkeypress="return soloLetras(event,'abcdefghijklnmñopqrstuvwxyz01234567890.-_/& ',true)" Style="text-transform:uppercase"></asp:TextBox>
                <span id="valrazsocial" class="validacion"></span>
            </div> 

            <div class="form-group col-md-6"> 
                <label for="inputZip">&nbsp;<span style="color: #FF0000; font-weight: bold;"></span></label>
                <div class="d-flex">
                <label class="radiobutton-container" >
                    3. RUC<input id="rbruc" runat="server" checked="true" type="radio" name="deck" value="ced"/>
                    <span class="checkmark"></span></label>
                <label for="inputEmail4">&nbsp;&nbsp;&nbsp;&nbsp;</label>
                <label class="radiobutton-container" >
                    Cédula<input id="rbci" runat="server" type="radio" name="deck" value="ci"/>
                    <span class="checkmark"></span></label>
                <label for="inputEmail4">&nbsp;&nbsp;&nbsp;&nbsp;</label>
                <label class="radiobutton-container" > 
                    Pasaporte<input id="rbpasaporte" runat="server" type="radio" name="deck" value="pas"/>
                    <span class="checkmark"></span> <span style="color: #FF0000; font-weight: bold;"> *</span>

                </label> 
                <asp:TextBox ID="txtruccipas" runat="server" MaxLength="25" onpaste="return false" class="form-control" style="text-align: center;text-transform:uppercase" 
                     onBlur="valruccipas(this,'valruccipas','rbruc','rbci','rbpasaporte');" onkeypress="return soloLetras(event,'01234567890abcdefghijklnmñopqrstuvwxyz_-',true)"></asp:TextBox> 
                 <span class="validacion" id="valruccipas" ></span>
                </div> 
            </div> 
          

        <div class="form-group col-md-6"> 
             <label for="inputAddress">4. Actividad Comercial:<span style="color: #FF0000; font-weight: bold;">*</span></label>
                <asp:TextBox ID="txtactividadcomercial" runat="server" MaxLength="500" style="text-align: center" onblur="checkcajalarge(this,'valactcom',true);"
             onkeypress="return soloLetras(event,'abcdefghijklnmñopqrstuvwxyz01234567890 ',true)" class="form-control"></asp:TextBox>
                <span id="valactcom" class="validacion"></span>
            </div> 

         <div class="form-group col-md-6"> 
              <label for="inputAddress">5. Dirección Oficina:<span style="color: #FF0000; font-weight: bold;">*</span></label>
                <asp:TextBox ID="txtdireccion" runat="server" MaxLength="60" style="text-align: center" onblur="checkcajalarge(this,'valdir',true);"
             onkeypress="return soloLetras(event,'abcdefghijklnmñopqrstuvwxyz01234567890/.-_ ',true)" class="form-control"></asp:TextBox>
                <span id="valdir" class="validacion"></span>
            </div> 

         <div class="form-group col-md-6"> 
              <label for="inputAddress">6. Teléfono Oficina<span style="color: #FF0000; font-weight: bold;">*</span></label>
                <asp:TextBox ID="txttelofi" runat="server" MaxLength="9" style="text-align: center" onBlur="telconvencional(this,'valtelofi',true);"
             onkeypress="return soloLetras(event,'01234567890',true)" class="form-control"></asp:TextBox>
                <span id="valtelofi" class="validacion"></span>
            </div> 

        <div class="form-group col-md-6"> 
            <label for="inputAddress">7. Persona Contacto:<span style="color: #FF0000; font-weight: bold;">*</span></label>
                <asp:TextBox ID="txtcontacto" runat="server" MaxLength="500" style="text-align: center" onblur="checkcaja(this,'valcontacto',true);"
             onkeypress="return soloLetras(event,'abcdefghijklnmñopqrstuvwxyz01234567890 ',true)" class="form-control"></asp:TextBox>
                <span id="valcontacto" class="validacion"></span>
            </div> 

        <div class="form-group col-md-6"> 
             <label for="inputAddress">8. Celular Contacto:<span style="color: #FF0000; font-weight: bold;">*</span></label>
                <asp:TextBox ID="txttelcelcon" runat="server" MaxLength="10" style="text-align: center" onBlur="telcelular(this,'valtelcel',true);"
             onkeypress="return soloLetras(event,'01234567890',true)" class="form-control"></asp:TextBox>
                <span id="valtelcel" class="validacion"></span>
            </div> 

        <div class="form-group col-md-6"> 
            <label for="inputAddress">9. Mail Contacto:<span style="color: #FF0000; font-weight: bold;">*</span></label>
                <asp:TextBox id='tmailinfocli' runat="server" name='textboxmailinfocli' class="form-control" placeholder="mail@mail.com" 
                onkeypress="return soloLetras(event,'abcdefghijklmnñopqrstuvwxyz1234567890_$@.;',true)"  MaxLength="200"/>
                <span id="valmail" class="validacion"></span>
            </div> 

         <div class="form-group col-md-6"> 
              <label for="inputAddress">10. Mail EBilling:<span style="color: #FF0000; font-weight: bold;">*</span></label>
                <asp:TextBox id='tmailebilling' runat="server" name='textboxmailinfocli' class="form-control" placeholder="mail@mail.com" 
                onkeypress="return soloLetras(event,'abcdefghijklmnñopqrstuvwxyz1234567890_$@.;',true)"  MaxLength="200"/>
                <span id="valmaileb" class="validacion"></span>
            </div> 

         <div class="form-group col-md-6"> 
             <label for="inputAddress">11. Certificaciones:<span style="color: #FF0000; font-weight: bold;"></span></label>
                <asp:TextBox ID="txtcertificaciones" runat="server" MaxLength="500" style="text-align: center; text-transform: uppercase" placeholder="Para varias Certificaciones separelos con punto y coma."
                 onkeypress="return soloLetras(event,'abcdefghijklnmñopqrstuvwxyz01234567890;./-_ ',true)" class="form-control"></asp:TextBox>
            </div> 

        <div class="form-group col-md-6"> 
            <label for="inputAddress">12. Sitio Web:<span style="color: #FF0000; font-weight: bold;"></span></label>
                <asp:TextBox placeholder="http https://www.dominio.com .ec .es .org etc." id='turl' runat="server" 
              enableviewstate="false" clientidmode="Static" onkeypress="return soloLetras(event,'abcdefghijklmnopqrstuvwxyz1234567890_:.-/;',true)" maxlength="250" class="form-control"> </asp:TextBox>
            </div> 

        <div class="form-group col-md-6"> 
            <label for="inputAddress">13. Afiliación a Gremios:<span style="color: #FF0000; font-weight: bold;"></span></label>
                <asp:TextBox ID="txtafigremios" runat="server" MaxLength="1000" style="text-align: center;text-transform: uppercase" placeholder="Para varios Gremios separelos con punto y coma."
             onkeypress="return soloLetras(event,'abcdefghijklnmñopqrstuvwxyz01234567890;./-_ ',true)" class="form-control"></asp:TextBox>
            </div> 

       <div class="form-group col-md-6"> 
            <label for="inputAddress">14. Referencia Comercial:<span style="color: #FF0000; font-weight: bold;"></span></label>
                <asp:TextBox ID="txtrefcom" runat="server" MaxLength="500" style="text-align: center;text-transform: uppercase" placeholder="Para varias referencias separelos con punto y coma."
             onkeypress="return soloLetras(event,'abcdefghijklnmñopqrstuvwxyz01234567890;./-_ ',true)" class="form-control"></asp:TextBox>
            </div> 

    </div>

        <div class="form-row">
            <div class="form-group col-md-12">
                <a class="btn btn-outline-primary mr-4"   runat="server" id="a2" clientidmode="Static" >3</a>
                <a class="level1"  runat="server" id="a3" clientidmode="Static" >Información del Representante Legal</a>
            </div>

            <div class="form-group col-md-6"> 
                 <label for="inputAddress">15. Representante Legal:<span style="color: #FF0000; font-weight: bold;">*</span></label>
                <asp:TextBox ID="txtreplegal" runat="server" MaxLength="500" style="text-align: center" onblur="checkcajalarge(this,'valreplegal',true);"
                 onkeypress="return soloLetras(event,'abcdefghijklnmñopqrstuvwxyz01234567890 ',true)" CssClass="form-control"></asp:TextBox>
                <span id="valreplegal" class="validacion"></span>
            </div> 

            <div class="form-group col-md-6"> 
                <label for="inputAddress">16. Teléfono Domicilio:<span style="color: #FF0000; font-weight: bold;">*</span></label>
                <asp:TextBox ID="txttelreplegal" runat="server" MaxLength="9" style="text-align: center" onBlur="telconvencionalcelular(this,'valtelreplegal',true);"
             onkeypress="return soloLetras(event,'01234567890',true)" CssClass="form-control"></asp:TextBox>
                <span id="valtelreplegal" class="validacion"></span>
            </div> 

            <div class="form-group col-md-6"> 
                 <label for="inputAddress">17. Dirección Domiciliaria:<span style="color: #FF0000; font-weight: bold;">*</span></label>
                <asp:TextBox ID="txtdirdomreplegal" runat="server" MaxLength="500" style="text-align: center" onblur="checkcajalarge(this,'valdirdomreplegal',true);"
             onkeypress="return soloLetras(event,'abcdefghijklnmñopqrstuvwxyz01234567890 ./_-',true)" CssClass="form-control"></asp:TextBox>
                <span id="valdirdomreplegal" class="validacion"></span>
            </div> 

             <div class="form-group col-md-6"> 
                   <label for="inputAddress">18. Cédula/Pasaporte<span style="color: #FF0000; font-weight: bold;"></span></label>
                 <div class="d-flex">

                <label class="radiobutton-container" >
                    Cédula<input id="rbcireplegal" runat="server" checked="true" type="radio" name="deck" value="ced"/>
                    <span class="checkmark"></span></label>
                <label for="inputEmail4">&nbsp;&nbsp;&nbsp;&nbsp;</label>
                <label class="radiobutton-container" >
                    Pasaporte<input id="rbpasreplegal" runat="server" type="radio" name="deckrp" value="ci"/>
                    <span class="checkmark"></span></label>
                      <asp:TextBox ID="txtci" runat="server" MaxLength="25" onpaste="return false" class="form-control"
                    style="text-align: center" onBlur="valccipasrep(this,'valcipasreplegal','rbcireplegal','rbpasreplegal');"
                    onkeypress="return soloLetras(event,'01234567890abcdefghijklnmñopqrstuvwxyz_-',true)"></asp:TextBox> 
                <span class="validacion" id="valcipasreplegal" ></span>
                 </div> 
            </div> 
           
        <div class="form-group col-md-12">
              <label for="inputAddress">19. Mail:<span style="color: #FF0000; font-weight: bold;">*</span></label>

                <asp:TextBox id='tmailRepLegal' runat="server" name='textboxRepLegal' class="form-control" placeholder="mail@mail.com" 
                onkeypress="return soloLetras(event,'abcdefghijklmnñopqrstuvwxyz1234567890_$@.;',true)"  MaxLength="200"/>
                <span id="valmailrl" class="validacion"></span>
            </div> 

    </div>


    <div class="row">
        <div class="form-group col-md-12">
                <a class="btn btn-outline-primary mr-4"  runat="server" id="a6" clientidmode="Static" >4</a>
                <a class="level1"  runat="server" id="ls6" clientidmode="Static" >Subir Documentos</a>
            </div>

        <div class="form-group col-md-12">
            <asp:Button ID="btnbuscardoc" runat="server" Text="Consultar Documentos" OnClientClick="mostrarsecexp();"
            onclick="btnbuscardoc_Click" class="btn btn-outline-primary mr-4"/>
        </div> 

        <div class="form-group col-md-12">
            <div class="alert alert-danger" style=" font-weight:bold">
                Declaro que los datos consignados y suministrados en el presente documento son correctos y de procedencia lícita.
                Autorizo a CONTECON GUAYAQUIL S.A. a solicitar confirmación de los mismos, en cualquier fuente de información y a
                compartir esta información con las Autoridades que lo soliciten.
            </div>
        </div>

      <div class="form-group col-md-12">
      
        <table id="tablerp" cellpadding="1" cellspacing="0">
       <asp:Repeater ID="tablePaginationDocumentos" runat="server">
            <HeaderTemplate>
            <table id="tablar"  cellspacing="1" cellpadding="1" class="table table-bordered invoice">
            <thead>
            <tr>
            <th class="nover"></th>
            <th>Documentos</th>
            <th>Escoja el archivo con formato indicado.</th>
            <th>Formato</th>
            </tr>
            </thead> 
            <tbody>
            </HeaderTemplate>
            <ItemTemplate>
            <tr class="point">
            <td class="nover"><asp:TextBox ID="txtiddocemp" runat="server" Text='<%#Eval("IDDOCEMP")%>' /></td>
            <td style=" font-size:inherit"><%#Eval("DOCUMENTO")%></td>
            <td >
                <asp:FileUpload extension='<%#Eval("EXTENSION")%>' class="btn btn-outline-primary mr-4" id="fsupload" title="Escoja el archivo con formato indicado en la siguiente columna."
                       onchange="validaextension(this)" style=" font-size:small" runat="server"/>
            </td>
            <td ><%#Eval("EXTENSION")%></td>
            </tr>
            </ItemTemplate>
            <FooterTemplate>
            </tbody>
            </table>
            </FooterTemplate>
         </asp:Repeater>
      </table>
     </div>

     </div>


   <div id="seccionexp" style=" display:none" runat="server" class="seccion">
    <div class="informativo">
      <table>
      <tr><td rowspan="2" class="inum"><div class="number"><asp:Label runat="server" ID="ls4" Text="4"></asp:Label></div></td><td class="level1" >Información Adicional - Exportadores</td></tr>
      <tr><td class="level2">
         Confirme que los datos sean correctos.
      </td></tr>
      </table>
    </div>
   
       <div class="colapser colapsa"></div>

      <div class="accion">
      <table class="controles" cellspacing="0" cellpadding="1">
      <tr><th class="bt-bottom bt-right  bt-left" colspan="4">Datos de Personas Naturales o Jurídicas que intervienen en la exportación de su Producto.</th></tr>
      </table>
      <div class="informativo" id="colector2">
      <table id="datexp" class="controles" cellpadding="1" cellspacing="0">
       <asp:Repeater ID="tablePaginationDatosExp" runat="server"  >
            <HeaderTemplate>
            <table id="tabla"  cellspacing="1" cellpadding="1" class="tabRepeat">
            <thead>
            <tr>
            <th></th>
            <th>Nombre</th>
            <th>Ejecutivo de Cuenta</th>
            <th>Mail</th>
            <th>Tèlefono</th>
            </tr>
            </thead> 
            <tbody>
            </HeaderTemplate>
            <ItemTemplate>
            <tr class="point" >
            <td><%#Eval("DESCRIPCION")%></td>
            <td>
                <asp:TextBox 
                style="text-align: center" 
                Text="" ID="txtnombreexp"  xval='<%#Eval("NOMBRE")%>' 
                runat="server" MaxLength="500" onkeypress="return soloLetras(event,'abcdefghijklmnñopqrstuvwxyz1234567890_- ',true)" ></asp:TextBox>
            </td>
            <td>
                <asp:TextBox 
                style="text-align: center" 
                Text="" ID="txtejecta" xval='<%#Eval("EJECUTIVO")%>' 
                runat="server" MaxLength="500" onkeypress="return soloLetras(event,'abcdefghijklmnñopqrstuvwxyz1234567890_- ',true)" ></asp:TextBox>
            </td>
            <td>
                <asp:TextBox
                placeholder="mail@mail.com" 
                id='tmail' name='tmail' runat="server" style= 'width:150px' class="date"
                onkeypress="return soloLetras(event,'abcdefghijklmnopqrstuvwxyz1234567890_@.;',true)"
                enableviewstate="false" clientidmode="Static"
                onblur="validamail(this,'valmailz');" maxlength="250"/>
               <span id="valmailz" class="validacion"></span>
            </td>
            <td>
                <asp:TextBox 
                style="text-align: center; width:100px"
                Text="" ID="txttelexp" xval='<%#Eval("TELEFONO")%>' 
                onBlur="telconvencionalcelularnulo(this,'valteleexp',true);"
                runat="server" MaxLength="10" onkeypress="return soloLetras(event,'01234567890',true)" ></asp:TextBox>
                <span id="valteleexp" class="validacion"></span>
            </td>
            </tr>
            </ItemTemplate>
            <FooterTemplate>
            </tbody>
            </table>
            </FooterTemplate>
         </asp:Repeater>
      </table>
      </div>
      <table class="controles" cellspacing="0" cellpadding="1">
       <tr><th class="bt-bottom bt-top bt-right  bt-left" colspan="4">Datos relativos a la logística de la exportación (deposito / planta o finca):</th></tr>
       </table>
     
          <table class="controles" cellspacing="0" cellpadding="1">
       <tr>
         <td class="bt-bottom  bt-right bt-left" style=" width:155px">20. Nombre:</td>
         <td class="bt-bottom">
         
         <input 
         id="txtnombreexp" type="text" style="Width:400px"  maxlength="500"  onblur="checkcajalarge(this,'valnomexp',true);"
                onkeypress="return soloLetras(event,'abcdefghijklnmñopqrstuvwxyz01234567890 ',false)"/>
         </td>
         <td class="bt-bottom bt-right validacion "><span id="valnomexp" class="validacion"> * obligatorio</span></td>
       </tr>
       <tr>
         <td class="bt-bottom  bt-right bt-left" style=" width:155px">21. Tipo:</td>
         <td class="bt-bottom">
             
             <input 
             id="txttipoexp" type="text" style="Width:400px"  maxlength="500"  onblur="checkcajalarge(this,'valtipoexp',true);"
                onkeypress="return soloLetras(event,'abcdefghijklnmñopqrstuvwxyz01234567890 ',false)"/>
         </td>
         <td class="bt-bottom bt-right validacion "><span id="valtipoexp" class="validacion"> * obligatorio</span></td>
       </tr>
       <tr>
         <td class="bt-bottom  bt-right bt-left" style=" width:155px">22. Dirección:</td>
         <td class="bt-bottom">
             <input 
             id="txtdirexp" type="text" style="Width:400px"  maxlength="500"  onblur="checkcajalarge(this,'valdirexp',true);"
                onkeypress="return soloLetras(event,'abcdefghijklnmñopqrstuvwxyz01234567890 /_-',false)"/>
         </td>
         <td class="bt-bottom bt-right validacion "><span id="valdirexp" class="validacion"> * obligatorio</span></td>
       </tr>
       <tr>
         <td class="bt-bottom  bt-right bt-left" style=" width:155px">23. Teléfono:</td>
         <td class="bt-bottom">
             <input 
             id="txttelexp" type="text" style="Width:200px"  maxlength="9"  onblur="telconvencionalcelular(this,'valtelexp',true);"
                onkeypress="return soloLetras(event,'1234567890 ',false)"/>
         </td>
         <td class="bt-bottom bt-right validacion "><span id="valtelexp" class="validacion"> * obligatorio</span></td>
         </tr>
       <tr>
         <td class="bt-bottom  bt-left" style=" width:155px">&nbsp;</td>
         <td class="bt-bottom  bt-left">
         <input id="btnAgregar"  type="button" value="Agregar" onclick="addrowlogexp();"/>
         <input  type="button" value="Remover" onclick="deleterowlogexp();" />
         </td>
         <td class="opcional bt-bottom bt-right" style="height:40px;" >
            agregar/quitar documentos.
         </td>
         </tr>
      </table>
  
          <div class="informativo" id="colector">
      <table runat="server" id="tableexpo">
      <tr><td>
       <table id="datlogexp" cellpadding="1" cellspacing="0" class="tabRepeat">
            <thead>
            <tr>
            <th>Nombre</th>
            <th>Tipo</th>
            <th>Dirección</th>
            <th>Télefono</th>
            </tr>
            </thead>
            <tbody>
            </tbody>
       </table>
       </td></tr>
       </table>
       </div>
      </div>

    </div>

   <div class="seccion" style=" display:none">
  
       <div class="informativo">
      <table>
      <tr><td rowspan="2" class="inum"><div class="number"><asp:Label Text="5" runat="server" ID="ls5"/></div></td><td class="level1" >Datos relativos a la seguridad para traslado al puerto.</td></tr>
      <tr><td class="level2">
         Confirme que los datos sean correctos.
      </td></tr>
      </table>
     </div>
  
       <div class="colapser colapsa"></div>
    <div class="accion">
    <table class="controles" cellspacing="0" cellpadding="1">
         <tr>
         <td class="bt-bottom bt-top bt-right bt-left" style=" width:155px" >24. Su producto es transportado con custodio o GPS?:</td>
         </tr>
    </table>
    <div class="bt-bottom  bt-right bt-left">
    <table cellspacing="0" cellpadding="1" style=" width:100%">
    <tr>
         <td>
           <asp:TextBox ID="txtgps" runat="server" Width="570px" MaxLength="500"
             style="text-align: center" 
             onkeypress="return soloLetras(event,'abcdefghijklnmñopqrstuvwxyz01234567890 ',true)"></asp:TextBox>
         </td>
    </tr>
    </table>
    </div>
    </div>
   
       
   </div>

    <div class="form-row" style=" display:none">
        <div class="form-group col-md-12">
                <a class="btn btn-outline-primary mr-4"  runat="server" id="a7" clientidmode="Static" >5</a>
                <a class="level1"  runat="server" id="A8" clientidmode="Static" >SISTEMA DE TRAZABILIDAD DE CARGA STC.</a>
       </div>
        <div class="form-group col-md-12">
             Estimado Cliente, suscríbase a STC y reciba notificaciones en tiempo real de la trazabilidad de sus contenedores de importación.
        </div>
         <div class="form-group col-md-12">
              Toda la información de su carga, al alcance de su mano a través de mail, app o nuestro portal de clientes.
        </div>
      <div class="form-group col-md-6"> 
           <%--<label for="inputAddress">Deseo el Servicio?:</label>--%>
          <div class="d-flex"> 
                <label class="radiobutton-container" >
                   Servicio de trazabilidad de carga de importación <input  id="rbpSiAcepto" runat="server"  type="checkbox"  checked="true"  clientidmode="Static" />
                    <span class="checkmark"></span>

                </label>
                <label for="inputEmail4">&nbsp;&nbsp;&nbsp;&nbsp;</label>
                <%--<label class="radiobutton-container" >
                    NO ACPEPTO<input id="rbpNoAcepto" runat="server" type="radio" checked="false"  onclick="checkNo(this);" clientidmode="Static"/>
                    <span class="checkmark"></span>

                </label>--%>
                     <label for="inputEmail4">&nbsp;&nbsp;&nbsp;&nbsp;</label>
               <%--  <asp:Button ID="BtnInformacion" runat="server" Text="Click para más información"  CssClass="btn btn-primary"
              OnClientClick="mostrarinformacion();" />--%>

               <input id="btclear"   type="reset" value="Click para más información" onClick="return mostrarinformacion()"  class="btn btn-primary" />

          </div> 
      </div>
        <div class="form-group col-md-6"> 
                
           
        </div> 

       
  <%--  <div class="seccion"  >
        
    
         <div class="accion">
               <table class="controles" cellspacing="0" cellpadding="1">
         <tr>
         <td class="bt-bottom  bt-right bt-left" style=" width:185px" ></td>
        
         <td class="bt-bottom bt-right validacion "><span id="valreplegal2" class="validacion"> </span></td>
         </tr>
         <tr>
         <td class="bt-bottom  bt-right bt-left" style=" width:185px" >Deseo el Servicio?:</td>
         <td class="bt-bottom">
           Si Acepto[<asp:RadioButton ID="rbcAcepto" runat="server" Checked="false" Text=""  />] No Acepto[<asp:RadioButton ID="rbpNoAcepto" runat="server" Checked ="true" Text=""/>]
   
         </td>
         <td class="bt-bottom bt-right validacion "><span id="valtelreplegal2" class="validacion"> * obligatorio</span></td>
         </tr>
         <tr><td class="bt-bottom  bt-right bt-left" style=" width:185px"></td>
         <td class="bt-bottom">
           </td>
         </tr>
        
      </table>

         </div>
    </div>--%>
     </div> 

          <div class="form-row">  
                <div class="form-group col-md-6">
                    <div data-theme="light" class="g-recaptcha" data-sitekey="6LfibkEUAAAAAIK-pu90AlJAbjvMSoKVIGkrov__" data-callback="recaptchaCallback"></div>
                             <input type="hidden" class="hiddenRecaptcha required" name="hiddenRecaptcha" id="hiddenRecaptcha">
                             <span id="msgCaptcha" style="color:red; font-size:small;"></span>
                             <asp:HiddenField runat="server" ID="imagencaptcha" />
                 </div> 

        </div>
       
   <div class="seccion">

   
    <div>
   <%-- <table class="controles" cellspacing="0" cellpadding="1">
    <tr>
    <td>
        <div id="dvCaptcha"></div>
        <asp:TextBox ID="txtCaptcha" runat="server" style=" display:none" />
        <asp:RequiredFieldValidator ID = "rfvCaptcha" style=" display:none"  ErrorMessage="Captcha validation is required." ControlToValidate="txtCaptcha" runat="server" ForeColor = "Red" Display = "Dynamic" />
    </td>
    </tr>
    <tr>
    <td style=" display:none">
        <asp:Image ID="imgCaptcha" runat="server" BorderColor="Silver" Width="300px" Height="73px" BorderStyle="Solid"/>
    </td>
    </tr>
    <tr>
    <td style=" display:none">
        <asp:TextBox ID="txtTextCaptcha" runat="server" Width="200px" MaxLength="6" 
        style="text-align: center" onblur="checkcaja(this,'valcaptcha',true)"
        onkeypress="return soloLetras(event,'abcdefghijklnmñopqrstuvwxyz01234567890 ',true)"></asp:TextBox></td>
    <td class="" style=" display:none"><span id="valcaptcha" class="validacion"> * obligatorio</span></td>
    </tr>
    </table>--%>
    </div>
  <br/> <br/>
    <div class="col-md-12 d-flex justify-content-center">
         <img alt="loading.." src="../shared/imgs/loader.gif" id="loader" style=" display:none" />
         <span id="imagen"></span>
         <asp:Button ID="btsalvar" runat="server" Text="Enviar Solicitud"  onclick="btsalvar_Click" OnClientClick="return prepareObject('¿Esta seguro de enviar la solicitud?')"
                   ToolTip="Confirma la información y genera el envio de la solicitud." CssClass="btn btn-primary"/>
     </div>

    <asp:HiddenField runat="server" ID="secexportador" Value="EXP"/>
    <asp:HiddenField runat="server" ID="emailsec2" />
    <asp:HiddenField runat="server" ID="emailsec3" />
    <asp:HiddenField runat="server" ID="tableexportador"/>
   <%-- <asp:HiddenField runat="server" ID="imagencaptcha" />--%>
  
   </div>
   
         


  <!-- Modal -->
   <asp:ModalPopupExtender  
      ID="mpedit" runat="server" 
       BehaviorID="idmpeLoading"
      PopupControlID="myModal"
      BackgroundCssClass="modalBackground"  
      TargetControlID="manualHide"
       CancelControlID="btclose"  />
       

    <asp:Panel ID="myModal" runat="server" CssClass="modalPopup" align="center" Style="display: none" >
        
        <div class="body2">
             <asp:UpdatePanel ID="msgload" runat="server" UpdateMode="Conditional" >
               <ContentTemplate>
            
 
  <table width="501" border="0" cellpadding="0" cellspacing="0">
  <!--DWLayoutTable-->
  <tr>
    <td height="36" colspan="3" valign="middle" bgcolor="#FF3720"><span style="color:#f2f2f2; font-weight:bold">&nbsp; Sistema de Trazabilidad de Carga STC</span></td>
  </tr>
  <tr>
    <td width="17" rowspan="2" valign="top"><!--DWLayoutEmptyCell-->&nbsp;</td>
    <td width="469" height="95" valign="top"><div align="justify">Estimado Cliente, suscríbase a STC y reciba notificaciones en tiempo real de la trazabilidad de sus contenedores de importación.<br/>
    Toda la información de su carga, al alcance de su mano a través de Whatsapp, Mail, App o nuestro portal de clientes.</div></td>
  <td width="15" rowspan="2" valign="top"><!--DWLayoutEmptyCell-->&nbsp;</td>
  </tr>
  <tr>
    <td height="190" valign="top"><div align="left"><strong>¿Qué información recibirá?</strong><br/>
        - Registro fotográfico del acto de aforo de su carga.<br/>
        - Registro fotográfico de sellos luego del acto de aforo.<br/>
        - Información de ubicación de contenedor en área de aforo.<br/>
        - Arribo de tu mercancía.<br/>
        - Correcciones que se realicen por peso o sello de descarga vs. manifestado.<br/>
        - Liberación aduanera.<br/>
        - Emisión de factura.<br/>
        - Confirmación de pago realizado.<br/>
        - Emisión de e-PASS y reasignación de turno.<br/>
        - Salida de tu mercancía.<br/>
        - Otros eventos.</div></td>
  </tr>
  <tr>
    <td height="44" colspan="3" valign="top"><div align="center"><strong>¡Fácil! ¡Rápido! ¡Seguro!</strong><br/>
        <strong>
    Precio del servicio $13,63 más IVA por contenedor</strong></div></td>
  </tr>
  <tr>
    <td height="25" colspan="3" valign="top"><div class="modal-footer">
                                            <input  type="button" id="btclose" class="btn btn-primary"  value="Salir"  />
                                            </div></td>
  </tr>
</table>
                     </ContentTemplate>
                   
            </asp:UpdatePanel>
        </div>
    </asp:Panel>

  
  </form>

     </div>
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.6.2/jquery.min.js"></script>
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
   <%-- <script type="text/javascript" src="https://www.google.com/recaptcha/api.js?hl=es&onload=onloadCallback&render=explicit" async defer></script>--%>

    <script src="../Scripts/pages.js" type="text/javascript"></script>
    <script src="../Scripts/credenciales.js" type="text/javascript"></script>

   <script src='https://www.google.com/recaptcha/api.js'></script>

   <%-- <script src="../Scripts/jquery-1.7.1.min.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.tablesorter.min.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.tablesorter.pager.js" type="text/javascript"></script>--%>

<%--<script type="text/javascript">
 
    function checkNo(element) {

        var valor1 = document.getElementById('<%=rbpNoAcepto.ClientID %>').checked;
     
        if (valor1) {

            document.getElementById('<%=rbpSiAcepto.ClientID %>').checked = false;
        }

    }

       function checksi(element) {

         var valor2 = document.getElementById('<%=rbpSiAcepto.ClientID %>').checked;

       if (valor2) {
           document.getElementById('<%=rbpNoAcepto.ClientID %>').checked = false;

        }
    }
  
   

</script>--%>

    <script type="text/javascript">


        var ced_count = 0;
        var jAisv = {};
        var valida = "";

        <%--var onloadCallback = function () {
            grecaptcha.render('dvCaptcha', {
                'sitekey': '<%=ReCaptcha_Key %>',
                'callback': function (response) {
                    $.ajax({
                        type: "POST",
                        url: "solicitudempresa.aspx/VerifyCaptcha",
                        data: "{response: '" + response + "'}",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (r) {
                            var captchaResponse = jQuery.parseJSON(r.d);
                            if (captchaResponse.success) {
                                $("[id*=txtCaptcha]").val(captchaResponse.success);
                                $("[id*=rfvCaptcha]").hide();
                            } else {
                                $("[id*=txtCaptcha]").val("");
                                $("[id*=rfvCaptcha]").show();
                                var error = captchaResponse["error-codes"][0];
                                $("[id*=rfvCaptcha]").html("RECaptcha error. " + error);
                            }
                        }
                    });
                }
            });
        };--%>

          function recaptchaCallback() {
                   document.getElementById('hiddenRecaptcha').value= grecaptcha.getResponse();
                   document.getElementById('msgCaptcha').innerHTML ='';
        };

       
        //controlador de mails
        var counter = 2;
        $("#addButton").click(function () {
            if (counter > 5) {
                alertify.alert("Solo se permiten 5 mails");
                return false;
            }
            $('<div/>', { 'id': 'TextBoxDiv' + counter }).html($('<span/>').html('mail #' + counter + ':')).append($('<input type="text" placeholder="mail@mail.com">').attr({ 'id': 'textbox' + counter, 'name': 'textbox' + counter, onkeypress: 'return soloLetras(event,"abcdefghijklmnñopqrstuvwxyz1234567890_@.",true);' })).appendTo('#TextBoxesGroup')
            counter++;
        });
        $("#removeButton").click(function () {
            if (counter == 2) {
                alertify.alert("Un mail es obligatorio");
                return false;
            }
            counter--;
            $("#TextBoxDiv" + counter).remove();
        });
        //controlador de mails representante legal
        var counter2 = 2;
        $("#addButtonRepLegal").click(function () {
            if (counter2 > 5) {
                alertify.alert("Solo se permiten 5 mails");
                return false;
            }
            $('<div/>', { 'id': 'TextBoxDivRepLegal' + counter2 }).html($('<span/>').html('mail #' + counter2 + ':')).append($('<input type="text" placeholder="mail@mail.com">').attr({ 'id': 'textboxreplegal' + counter2, 'name': 'textboxreplegal' + counter2, onkeypress: 'return soloLetras(event,"abcdefghijklmnñopqrstuvwxyz1234567890_@.",true);' })).appendTo('#TextBoxesGroupRepLegal')
            counter2++;
        });
        $("#removeButtonRepLegal").click(function () {
            if (counter2 == 2) {
                alertify.alert("Un mail es obligatorio");
                return false;
            }
            counter2--;
            $("#TextBoxDivRepLegal" + counter2).remove();
        });
        //controlador de mails autorizacion terceros
        var counter3 = 2;
        $("#addButtonMailAuTer").click(function () {
            if (counter3 > 5) {
                alertify.alert("Solo se permiten 5 mails");
                return false;
            }
            $('<div/>', { 'id': 'textboxmailauter' + counter3 }).html($('<span/>').html('mail #' + counter3 + ':')).append($('<input type="text" placeholder="mail@mail.com">').attr({ 'id': 'textboxmailauter' + counter3, 'name': 'textboxmailauter' + counter3, onkeypress: 'return soloLetras(event,"abcdefghijklmnñopqrstuvwxyz1234567890_@.",true);' })).appendTo('#TextBoxesGroupAuTer')
            counter3++;
        });
        $("#removeButtonMailAuTer").click(function () {
            if (counter3 == 2) {
                alertify.alert("Un mail es obligatorio");
                return false;
            }
            counter3--;
            $("#textboxmailauter" + counter3).remove();
        });

        <%--function validaMSCaptcha() {
            $.ajax({
                type: "POST",
                url: "solicitudempresa.aspx/IsCaptchaAvailable",
                data: '{valorcaptchat: "' + $("#<%=imagencaptcha.ClientID%>")[0].value + '", valorcaja: "' + $("#<%=txtTextCaptcha.ClientID%>")[0].value + '" }',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function OnSuccess(response) {
                    if (response.d != "1") {
                        alertify.alert('* Datos de Registro de Empresa: *\n * Los caracteres no coinciden, inténtalo de nuevo *').set('label', 'Aceptar');
                        document.getElementById('<%=txtTextCaptcha.ClientID %>').focus();
                      
                        document.getElementById("loader").className = 'nover';
                        valida = response.d;
                    }
                    else {
                        valida = response.d;
                    }
                },
                failure: function (response) {
                    alertify.alert(response.d);
                }
            });
        }--%>
        //esta futura funcion va a preparar el objeto a transportar.
        var registroempresa = {};
        var lista = [];
        var cblarray = [];
        var carray = [];
        function prepareObject(valor) {
            try {

                if (confirm(valor) == false) {
                    return false;
                }
                
                //Valida los datos de las secciones
                var chk = document.getElementById('<%=cbltipousuario.ClientID %>');
                var checkboxes = chk.getElementsByTagName("input");
                var cont = 0;
                for (var x = 0; x < checkboxes.length; x++) {
                    if (checkboxes[x].checked) {
                        cblarray[x] = x;
                        cont++;
                    }
                }
                if (cont == 0) {
                    alertify.alert('* Datos de Registro de Empresa: *\n * Seleccione al menos un Tipo de Cliente *').set('label', 'Aceptar');
                    document.getElementById('<%=cbltipousuario.ClientID %>').focus();
                    //document.getElementById('<%=cbltipousuario.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:400px;";
                    document.getElementById("loader").className = 'nover';
                    return false;
                }
                var vals = document.getElementById('<%=txtrazonsocial.ClientID %>').value;
                if (vals == '' || vals == null || vals == undefined) {
                    alertify.alert('* Datos de Registro de Empresa: *\n * Escriba la Razon Social *').set('label', 'Aceptar');
                    document.getElementById('<%=txtrazonsocial.ClientID %>').focus();
                    <%--document.getElementById('<%=txtrazonsocial.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:400px;";--%>
                    document.getElementById("loader").className = 'nover';
                    return false;
                }
                if (!valruccipasservidor()) {
                    return false;
                };
                var vals = document.getElementById('<%=txtactividadcomercial.ClientID %>').value;
                if (vals == '' || vals == null || vals == undefined) {
                    alertify.alert('* Datos de Registro de Empresa: *\n * Escriba la Actividad Comercial *').set('label', 'Aceptar');
                    document.getElementById('<%=txtactividadcomercial.ClientID %>').focus();
                   <%-- document.getElementById('<%=txtactividadcomercial.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:400px;";--%>
                    document.getElementById("loader").className = 'nover';
                    return false;
                }
                var vals = document.getElementById('<%=txtdireccion.ClientID %>').value;
                if (vals == '' || vals == null || vals == undefined) {
                    alertify.alert('* Datos de Registro de Empresa: *\n * Escriba la Dirección de Oficina *').set('label', 'Aceptar');
                    document.getElementById('<%=txtdireccion.ClientID %>').focus();
                    <%--document.getElementById('<%=txtdireccion.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:400px;";--%>
                    document.getElementById("loader").className = 'nover';
                    return false;
                }
                var vals = document.getElementById('<%=txttelofi.ClientID %>').value;
                if (!/^([0-9])*$/.test(vals)) {
                    alertify.alert('* Datos de Registro de Empresa: *\n * Teléfono de Oficina No es un Numero *').set('label', 'Aceptar');
                    document.getElementById('<%=txttelofi.ClientID %>').focus();
                  <%--  document.getElementById('<%=txttelofi.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:200;";--%>
                    document.getElementById("loader").className = 'nover';
                    return false;
                }
                if (vals.length < 9) {
                    alertify.alert('* Datos de Registro de Empresa: *\n * Teléfono de Oficina Incompleto *').set('label', 'Aceptar');
                    document.getElementById('<%=txttelofi.ClientID %>').focus();
                   <%-- document.getElementById('<%=txttelofi.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:200;";--%>
                    document.getElementById("loader").className = 'nover';
                    return false;
                }
                if (vals == '' || vals == null || vals == undefined) {
                    alertify.alert('* Datos de Registro de Empresa: *\n * Escriba el Teléfono de Oficina *').set('label', 'Aceptar');
                    document.getElementById('<%=txttelofi.ClientID %>').focus();
                  <%--  document.getElementById('<%=txttelofi.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:200;";--%>
                    document.getElementById("loader").className = 'nover';
                    return false;
                }
                var vals = document.getElementById('<%=txtcontacto.ClientID %>').value;
                if (vals == '' || vals == null || vals == undefined) {
                    alertify.alert('* Datos de Registro de Empresa: *\n * Escriba la Persona de Contacto *').set('label', 'Aceptar');
                    document.getElementById('<%=txtcontacto.ClientID %>').focus();
                  <%--  document.getElementById('<%=txtcontacto.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:200;";--%>
                    document.getElementById("loader").className = 'nover';
                    return false;
                }
                var vals = document.getElementById('<%=txttelcelcon.ClientID %>').value;
                if (!/^([0-9])*$/.test(vals)) {
                    alertify.alert('* Datos de Registro de Empresa: *\n * No. de Celular de Contacto No es un Numero *').set('label', 'Aceptar');
                    document.getElementById('<%=txttelcelcon.ClientID %>').focus();
                   <%-- document.getElementById('<%=txttelcelcon.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:200;";--%>
                    document.getElementById("loader").className = 'nover';
                    return false;
                }
                if (vals.length < 10) {
                    alertify.alert('* Datos de Registro de Empresa: *\n * No. de Celular de Contacto Incompleto *').set('label', 'Aceptar');
                    document.getElementById('<%=txttelcelcon.ClientID %>').focus();
                  <%--  document.getElementById('<%=txttelcelcon.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:200;";--%>
                    document.getElementById("loader").className = 'nover';
                    return false;
                }
                if (vals == '' || vals == null || vals == undefined) {
                    alertify.alert('* Datos de Registro de Empresa: *\n * Escriba el No. de Celular de Contacto *').set('label', 'Aceptar');
                    document.getElementById('<%=txttelcelcon.ClientID %>').focus();
                    <%--document.getElementById('<%=txttelcelcon.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:200;";--%>
                    document.getElementById("loader").className = 'nover';
                    return false;
                }
                var mail1 = document.getElementById('<%=tmailinfocli.ClientID %>').value;
                if (mail1 == null || mail1 == undefined || mail1 == '') {
                    alertify.alert('* Datos de Registro de Empresa: *\n * Escriba al menos un mail *').set('label', 'Aceptar');
                    document.getElementById('<%=tmailinfocli.ClientID %>').focus();
                   <%-- document.getElementById('<%=tmailinfocli.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:200;";--%>
                    document.getElementById("loader").className = 'nover';
                    return false;
                }
                if (mail1.trim().length > 0) {
                    if (!validarEmail(mail1)) {
                        //alertify.alert('* Datos de Registro de Empresa: *\n * Mail no parece correcto *')
                        document.getElementById('<%=tmailinfocli.ClientID %>').focus();
                        <%--document.getElementById('<%=tmailinfocli.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:200;";--%>
                        document.getElementById("loader").className = 'nover';
                        return false;
                    }
                }
                var mail1 = document.getElementById('<%=tmailebilling.ClientID %>').value;
                if (mail1 == null || mail1 == undefined || mail1 == '') {
                    alertify.alert('* Datos de Registro de Empresa: *\n * Escriba al menos un mail *').set('label', 'Aceptar');
                    document.getElementById('<%=tmailebilling.ClientID %>').focus();
                   <%-- document.getElementById('<%=tmailebilling.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:200;";--%>
                    document.getElementById("loader").className = 'nover';
                    return false;
                }
                if (mail1.trim().length > 0) {
                    if (!validarEmail(mail1)) {
                        alertify.alert('* Datos de Registro de Empresa: *\n * Mail no parece correcto *').set('label', 'Aceptar');
                        document.getElementById('<%=tmailebilling.ClientID %>').focus();
                       <%-- document.getElementById('<%=tmailebilling.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:200;";--%>
                        document.getElementById("loader").className = 'nover';
                        return false;
                    }
                }

                var vals = document.getElementById('<%=txtreplegal.ClientID %>').value;
                if (vals == '' || vals == null || vals == undefined) {
                    alertify.alert('* Datos de Registro de Empresa: *\n * Escriba el Representante Legal *').set('label', 'Aceptar');
                    document.getElementById('<%=txtreplegal.ClientID %>').focus();
                   <%-- document.getElementById('<%=txtreplegal.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:400;";--%>
                    document.getElementById("loader").className = 'nover';
                    return false;
                }
                var vals = document.getElementById('<%=txttelreplegal.ClientID %>').value;
                if (!/^([0-9])*$/.test(vals)) {
                    alertify.alert('* Datos de Registro de Empresa: *\n * Teléfono de Domicilio No es un Numero *').set('label', 'Aceptar');
                    document.getElementById('<%=txttelreplegal.ClientID %>').focus();
                   <%-- document.getElementById('<%=txttelreplegal.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:200;";--%>
                    document.getElementById("loader").className = 'nover';
                    return false;
                }
                if (vals.length < 9) {
                    alertify.alert('* Datos de Registro de Empresa: *\n * Teléfono de Domicilio Incompleto *').set('label', 'Aceptar');
                    document.getElementById('<%=txttelreplegal.ClientID %>').focus();
                   <%-- document.getElementById('<%=txttelreplegal.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:200;";--%>
                    document.getElementById("loader").className = 'nover';
                    return false;
                }
                if (vals == '' || vals == null || vals == undefined) {
                    alertify.alert('* Datos de Registro de Empresa: *\n * Escriba el Teléfono de Domicilio *').set('label', 'Aceptar');
                    document.getElementById('<%=txttelreplegal.ClientID %>').focus();
                   <%-- document.getElementById('<%=txttelreplegal.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:200;";--%>
                    document.getElementById("loader").className = 'nover';
                    return false;
                }
                var vals = document.getElementById('<%=txtdirdomreplegal.ClientID %>').value;
                if (vals == '' || vals == null || vals == undefined) {
                    alertify.alert('* Datos de Registro de Empresa: *\n * Escriba la Dirección Domiciliaria *').set('label', 'Aceptar');
                    document.getElementById('<%=txtdirdomreplegal.ClientID %>').focus();
                  <%--  document.getElementById('<%=txtdirdomreplegal.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:400;";--%>
                    document.getElementById("loader").className = 'nover';
                    return false;
                }

                var mail1 = document.getElementById('<%=tmailRepLegal.ClientID %>').value;
                if (mail1 == null || mail1 == undefined || mail1 == '') {
                    alertify.alert('* Datos de Registro de Empresa: *\n * Escriba al menos un mail *').set('label', 'Aceptar');
                    document.getElementById('<%=tmailRepLegal.ClientID %>').focus();
                   <%-- document.getElementById('<%=tmailRepLegal.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:200;";--%>
                    document.getElementById("loader").className = 'nover';
                    return false;
                }
                if (mail1.trim().length > 0) {
                    if (!validarEmail(mail1)) {
                        alertify.alert('* Datos de Registro de Empresa: *\n * Mail de Representante Legal no parece correcto *').set('label', 'Aceptar');
                        document.getElementById('<%=tmailRepLegal.ClientID %>').focus();
                      <%--  document.getElementById('<%=tmailRepLegal.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:200;";--%>
                        document.getElementById("loader").className = 'nover';
                        return false;
                    }
                }
                if (!validaciservidor()) {
                    return false;
                };

                //Valida Documentos
                lista = [];
                var vals = document.getElementById('tablar');
                if (vals == '' || vals == null || vals == undefined) {
                    alertify.alert('* Datos de Registro de Empresa: *\n *No se encontraron Documentos*').set('label', 'Aceptar');
                    document.getElementById('<%=btnbuscardoc.ClientID %>').focus();
                    document.getElementById("loader").className = 'nover';
                    return false;
                }
                if (vals != null) {
                    var tbl = document.getElementById('tablar');
                    if (tbl.rows.length == 1) {
                        alertify.alert('* Datos de Registro de Empresa: *\n *Por favor de un click sobre el boton Consultar Documentos*').set('label', 'Aceptar');
                        document.getElementById('<%=btnbuscardoc.ClientID %>').focus();
                        document.getElementById("loader").className = 'nover';
                        return false;
                    }
                    for (var f = 0; f < tbl.rows.length; f++) {
                        var celColect = tbl.rows[f].getElementsByTagName('td');
                        if (celColect != undefined && celColect != null && celColect.length > 0) {
                            var tdetalle = {
                                documento: celColect[2].getElementsByTagName('input')[0].value
                            };
                            this.lista.push(tdetalle);
                        }
                    }
                    var nomdoc = null;
                    for (var n = 0; n < this.lista.length; n++) {
                        if (lista[n].documento == '' || lista[n].documento == null || lista[n].documento == undefined) {
                            alertify.alert('* Datos de Registro de Empresa: *\n * Seleccione todos los documentos requeridos *').set('label', 'Aceptar');
                            document.getElementById("loader").className = 'nover';
                            return false;
                        }
                        if (nomdoc == lista[n].documento) {
                            alertify.alert('* Datos de Registro de Empresa: *\n * Existen archivos repetidos, revise por favor *').set('label', 'Aceptar');
                            document.getElementById("loader").className = 'nover';
                            return false;
                        }
                        nomdoc = lista[n].documento;
                    }
                }
                
               <%-- var vals = document.getElementById('<%=txtTextCaptcha.ClientID %>').value;
                if (vals == '' || vals == null || vals == undefined) {
                    alertify.alert('* Datos de Registro de Empresa: *\n * Escribe los caracteres que veas *').set('label', 'Aceptar');
                    document.getElementById('<%=txtTextCaptcha.ClientID %>').focus();
                    document.getElementById("loader").className = 'nover';
                    return false;
                }--%>
                
                
               <%-- var vals = document.getElementById('<%=txtCaptcha.ClientID %>').value;
                if (vals == '' || vals == null || vals == undefined) {
                    alertify.alert('* Se requiere validación de reCaptcha.').set('label', 'Aceptar').set('label', 'Aceptar');
                    return false;
                }--%>
                var captura = document.getElementById("hiddenRecaptcha").value;
                if(captura == '')
                {
                    document.getElementById('msgCaptcha').innerHTML = "<span>Por favor confirme que usted no es un robot</span>";
                    alertify.alert('* Se requiere validación de reCaptcha.').set('label', 'Aceptar').set('label', 'Aceptar');
                    return false;
                }
                $.getScript("https://www.google.com/recaptcha/api.js");
                grecaptcha.reset(); 



                getGif();
                document.getElementById("loader").className = '';
                return true;
            } catch (e) {
                alertify.alert('Lamentamos comunicarle que ha ocurrido el siguiente problema:\n' + e.Message).set('label', 'Aceptar');
            }
        }
        function popupCallback(data, control) {
            this.document.getElementById(control).value = data;
        }
        function valcbl() {
            var chk = document.getElementById('<%=cbltipousuario.ClientID %>');
            var checkboxes = chk.getElementsByTagName("input");
            var cont = 0;
            for (var x = 0; x < checkboxes.length; x++) {
                if (checkboxes[x].checked) {
                    cont++;
                    if (checkboxes[x].value == 'EXP') {
                        document.getElementById('<%=seccionexp.ClientID %>').style.display = 'none';
//                        document.getElementById('<%=ls5.ClientID %>').innerHTML = '5';
//                        document.getElementById('<%=ls6.ClientID %>').innerHTML = '6';
                    }
                }
            }
            for (var x = 0; x < checkboxes.length; x++) {
                if (checkboxes[x].checked == false) {
                    cont++;
                    if (checkboxes[x].value == 'EXP') {
                        document.getElementById('<%=seccionexp.ClientID %>').style.display = 'none';
////                        document.getElementById('<%=ls5.ClientID %>').innerHTML = '4';
////                        document.getElementById('<%=ls6.ClientID %>').innerHTML = '5';
                    }
                }
            }
            //validador.innerHTML = '<span class="obligado">OBLIGATORIO!</span>';
            return;
        }
        function ocultarsecexp() {
            document.getElementById('<%=tablePaginationDocumentos.ClientID %>').style.display = 'none';
            
//            document.getElementById('<%=ls5.ClientID %>').innerHTML = '4';
//            document.getElementById('<%=ls6.ClientID %>').innerHTML = '5';
        }
        function mostrarsecexp() {
            document.getElementById('<%=tablePaginationDocumentos.ClientID %>').style.display = 'block';
            //            document.getElementById('<%=ls5.ClientID %>').innerHTML = '4';
            //            document.getElementById('<%=ls6.ClientID %>').innerHTML = '5';
        }
        function validaciservidor() {
            var vcireplegal = document.getElementById('rbcireplegal').checked;
            if (vcireplegal == true) {
                var valruccipas = document.getElementById('<%=txtci.ClientID %>').value;
                if (!/^([0-9])*$/.test(valruccipas)) {
                    alertify.alert('* Datos de Registro de Empresa: *\n * Cédula de Identidad No es un Numero. *').set('label', 'Aceptar');
                    document.getElementById('<%=txtci.ClientID %>').focus();
                   <%-- document.getElementById('<%=txtci.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:200;";--%>
                    document.getElementById("loader").className = 'nover';
                    return false;
                }
                else if (valruccipas == '' || valruccipas == null || valruccipas == undefined) {
                    alertify.alert('* Datos de Registro de Empresa: *\n * Escriba la Cédula de Identidad. *').set('label', 'Aceptar');
                    document.getElementById('<%=txtci.ClientID %>').focus();
                   <%-- document.getElementById('<%=txtci.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:200;";--%>
                    document.getElementById("loader").className = 'nover';
                    return false;
                }
                if (valruccipas.length = 0) {
                    alertify.alert('* Datos de Registro de Empresa: *\n * Escriba la Cédula de Identidad. *').set('label', 'Aceptar');
                    document.getElementById('<%=txtci.ClientID %>').focus();
                   <%-- document.getElementById('<%=txtci.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:200;";--%>
                    document.getElementById("loader").className = 'nover';
                    return false;
                }
                if (valruccipas.length < 10) {
                    alertify.alert('* Datos de Registro de Empresa: *\n * Cédula de Identidad imcompleto. *').set('label', 'Aceptar');
                    document.getElementById('<%=txtci.ClientID %>').focus();
                    <%--document.getElementById('<%=txtci.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:200;";--%>
                    document.getElementById("loader").className = 'nover';
                    return false;
                }
                var array = valruccipas.split("");
                var num = array.length;
                var total = 0;
                var digito = (array[9] * 1);
                for (i = 0; i < (num - 1); i++) {
                    var mult = 0;
                    if ((i % 2) != 0) {
                        total = total + (array[i] * 1);
                    }
                    else {
                        mult = array[i] * 2;
                        if (mult > 9)
                            total = total + (mult - 9);
                        else
                            total = total + mult;
                    }
                }
                var decena = total / 10;
                decena = Math.floor(decena);
                decena = (decena + 1) * 10;
                var final = (decena - total);

                if (digito != 0) {
                    if (final != digito) {
                        alertify.alert('* Datos de Registro de Empresa: *\n * Cédula de Identidad no válido. *').set('label', 'Aceptar');
                        document.getElementById('<%=txtci.ClientID %>').focus();
                        <%--document.getElementById('<%=txtci.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:200;";--%>
                        document.getElementById("loader").className = 'nover';
                        return false;
                    }
                }
                else {
                    if (final != 10) {
                        alertify.alert('* Datos de Registro de Empresa: *\n * Cédula de Identidad no válido. *').set('label', 'Aceptar');
                        document.getElementById('<%=txtci.ClientID %>').focus();
                        <%--document.getElementById('<%=txtci.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:200;";--%>
                        document.getElementById("loader").className = 'nover';
                        return false;
                    }
                }
            }
            else {
                var pasreplegal = document.getElementById('<%=txtci.ClientID %>').value;
                if (pasreplegal == '' || pasreplegal == null || pasreplegal == undefined) {
                    alertify.alert('* Datos de Registro de Empresa: *\n * Escriba el No. Pasaporte del Representante Legal *').set('label', 'Aceptar');
                    document.getElementById('<%=txtci.ClientID %>').focus();
                    <%--document.getElementById('<%=txtci.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:200;";--%>
                    document.getElementById("loader").className = 'nover';
                    return false;
                }
            }
            return true;
        }
        function valruccipasservidor() {
            //codigo = control.value.trim().toUpperCase();
            var valruccipas = document.getElementById('<%=txtruccipas.ClientID %>').value;
            var vruc = document.getElementById('rbruc').checked;
            var vci = document.getElementById('rbci').checked;
            var vpas = document.getElementById('rbpasaporte').checked;
            if (vruc == true) {
                if (!/^([0-9])*$/.test(valruccipas)) {
                    alertify.alert('* Datos de Registro de Empresa: *\n * No. RUC No es un Numero. *').set('label', 'Aceptar');
                    document.getElementById('<%=txtruccipas.ClientID %>').focus();
                    <%--document.getElementById('<%=txtruccipas.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:200;";--%>
                    document.getElementById("loader").className = 'nover';
                    return false;
                }
                else if (valruccipas == '' || valruccipas == null || valruccipas == undefined) {
                    alertify.alert('* Datos de Registro de Empresa: *\n * Escriba el No. de RUC. *').set('label', 'Aceptar');
                    document.getElementById('<%=txtruccipas.ClientID %>').focus();
                   <%-- document.getElementById('<%=txtruccipas.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:200;";--%>
                    document.getElementById("loader").className = 'nover';
                    return false;
                }
                var numeroProvincias = 24;
                var numprov = valruccipas.substr(0, 2);
                if (numprov > numeroProvincias) {
                    alertify.alert('* Datos de Registro de Empresa: *\n * El código de la provincia (dos primeros dígitos) es inválido! *').set('label', 'Aceptar');
                    document.getElementById('<%=txtruccipas.ClientID %>').focus();
                   <%-- document.getElementById('<%=txtruccipas.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:200;";--%>
                    document.getElementById("loader").className = 'nover';
                    return false;
                }
                //validadocrucservidor(valruccipas);
                if (!validadocrucservidor(valruccipas)) {
                    return false;
                };
            }
            if (vci == true) {
                if (!/^([0-9])*$/.test(valruccipas)) {
                    alertify.alert('* Datos de Registro de Empresa: *\n * No. C.I. No es un Numero. *').set('label', 'Aceptar');
                    document.getElementById('<%=txtruccipas.ClientID %>').focus();
                   <%-- document.getElementById('<%=txtruccipas.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:200;";--%>
                    document.getElementById("loader").className = 'nover';
                    return false;
                }
                else if (valruccipas == '' || valruccipas == null || valruccipas == undefined) {
                    alertify.alert('* Datos de Registro de Empresa: *\n * Escriba el No. de C.I. *').set('label', 'Aceptar');
                    document.getElementById('<%=txtruccipas.ClientID %>').focus();
                    <%--document.getElementById('<%=txtruccipas.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:200;";--%>
                    document.getElementById("loader").className = 'nover';
                    return false;
                }
                if (valruccipas.length = 0) {
                    alertify.alert('* Datos de Registro de Empresa: *\n * Escriba el No. de C.I. *').set('label', 'Aceptar');
                    document.getElementById('<%=txtruccipas.ClientID %>').focus();
                  <%--  document.getElementById('<%=txtruccipas.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:200;";--%>
                    document.getElementById("loader").className = 'nover';
                    return false;
                }
                if (valruccipas.length < 10) {
                    alertify.alert('* Datos de Registro de Empresa: *\n * No. C.I. INCOMPLETO. *').set('label', 'Aceptar');
                    document.getElementById('<%=txtruccipas.ClientID %>').focus();
                   <%-- document.getElementById('<%=txtruccipas.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:200;";--%>
                    document.getElementById("loader").className = 'nover';
                    return false;
                }
                var array = valruccipas.split("");
                var num = array.length;
                var total = 0;
                var digito = (array[9] * 1);
                for (i = 0; i < (num - 1); i++) {
                    var mult = 0;
                    if ((i % 2) != 0) {
                        total = total + (array[i] * 1);
                    }
                    else {
                        mult = array[i] * 2;
                        if (mult > 9)
                            total = total + (mult - 9);
                        else
                            total = total + mult;
                    }
                }
                var decena = total / 10;
                decena = Math.floor(decena);
                decena = (decena + 1) * 10;
                var final = (decena - total);

                if (digito != 0) {
                    if (final != digito) {
                    alertify.alert('* Datos de Registro de Empresa: *\n * No. C.I. no válido. *').set('label', 'Aceptar');
                    document.getElementById('<%=txtruccipas.ClientID %>').focus();
                  <%--  document.getElementById('<%=txtruccipas.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:200;";--%>
                    document.getElementById("loader").className = 'nover';
                    return false;
                    }
                }
                else {
                    if (final != 10) {
                    alertify.alert('* Datos de Registro de Empresa: *\n * No. C.I. no válido. *').set('label', 'Aceptar');
                    document.getElementById('<%=txtruccipas.ClientID %>').focus();
                    <%--document.getElementById('<%=txtruccipas.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:200;";--%>
                    document.getElementById("loader").className = 'nover';
                    return false;
                    }
                }
            }
            if (vpas == true) {
                if (valruccipas == '' || valruccipas == null || valruccipas == undefined) {
                    alertify.alert('* Datos de Registro de Empresa: *\n * Escriba el No. Pasaporte. *').set('label', 'Aceptar');
                    document.getElementById('<%=txtruccipas.ClientID %>').focus();
                   <%-- document.getElementById('<%=txtruccipas.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:200;";--%>
                    document.getElementById("loader").className = 'nover';
                    return false;
                }
           }
            return true;
        }
        function validadocrucservidor(campo) {

            var numero = campo;
            var suma = 0;
            var residuo = 0;
            var pri = false;
            var pub = false;
            var nat = false;
            var numeroProvincias = 24;
            var modulo = 11;

            if (campo.length < 13) {
                alertify.alert('* Datos de Registro de Empresa: *\n * No. RUC. INCOMPLETO. *').set('label', 'Aceptar');
                document.getElementById('<%=txtruccipas.ClientID %>').focus();
                <%--document.getElementById('<%=txtruccipas.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:200;";--%>
                document.getElementById("loader").className = 'nover';
                return false;
            }
            if (campo.length > 13) {
                alertify.alert('* Datos de Registro de Empresa: *\n * El valor no corresponde a un No. de RUC. *').set('label', 'Aceptar');
                document.getElementById('<%=txtruccipas.ClientID %>').focus();
                <%--document.getElementById('<%=txtruccipas.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:200;";--%>
                document.getElementById("loader").className = 'nover';
                return false;
            }
            /* Verifico que el campo no contenga letras */

            /* Aqui almacenamos los digitos de la cedula en variables. */
            d1 = numero.substr(0, 1);
            d2 = numero.substr(1, 1);
            d3 = numero.substr(2, 1);
            d4 = numero.substr(3, 1);
            d5 = numero.substr(4, 1);
            d6 = numero.substr(5, 1);
            d7 = numero.substr(6, 1);
            d8 = numero.substr(7, 1);
            d9 = numero.substr(8, 1);
            d10 = numero.substr(9, 1);

            /* El tercer digito es: */
            /* 9 para sociedades privadas y extranjeros */
            /* 6 para sociedades publicas */
            /* menor que 6 (0,1,2,3,4,5) para personas naturales */

            if (d3 == 7 || d3 == 8) {
                //alertify.alert('El tercer dígito ingresado es inválido');
                alertify.alert('* Datos de Registro de Empresa: *\n * El tercer dígito ingresado es inválido. *').set('label', 'Aceptar');
                document.getElementById('<%=txtruccipas.ClientID %>').focus();
                <%--document.getElementById('<%=txtruccipas.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:200;";--%>
                document.getElementById("loader").className = 'nover';
                return false;
            }

            /* Solo para personas naturales (modulo 10) */
            if (d3 < 6) {
                nat = true;
                p1 = d1 * 2; if (p1 >= 10) p1 -= 9;
                p2 = d2 * 1; if (p2 >= 10) p2 -= 9;
                p3 = d3 * 2; if (p3 >= 10) p3 -= 9;
                p4 = d4 * 1; if (p4 >= 10) p4 -= 9;
                p5 = d5 * 2; if (p5 >= 10) p5 -= 9;
                p6 = d6 * 1; if (p6 >= 10) p6 -= 9;
                p7 = d7 * 2; if (p7 >= 10) p7 -= 9;
                p8 = d8 * 1; if (p8 >= 10) p8 -= 9;
                p9 = d9 * 2; if (p9 >= 10) p9 -= 9;
                modulo = 10;
            }

            /* Solo para sociedades publicas (modulo 11) */
            /* Aqui el digito verficador esta en la posicion 9, en las otras 2 en la pos. 10 */
            else if (d3 == 6) {
                pub = true;
                p1 = d1 * 3;
                p2 = d2 * 2;
                p3 = d3 * 7;
                p4 = d4 * 6;
                p5 = d5 * 5;
                p6 = d6 * 4;
                p7 = d7 * 3;
                p8 = d8 * 2;
                p9 = 0;
            }

            /* Solo para entidades privadas (modulo 11) */
            else if (d3 == 9) {
                pri = true;
                p1 = d1 * 4;
                p2 = d2 * 3;
                p3 = d3 * 2;
                p4 = d4 * 7;
                p5 = d5 * 6;
                p6 = d6 * 5;
                p7 = d7 * 4;
                p8 = d8 * 3;
                p9 = d9 * 2;
            }

            suma = p1 + p2 + p3 + p4 + p5 + p6 + p7 + p8 + p9;
            residuo = suma % modulo;

            /* Si residuo=0, dig.ver.=0, caso contrario 10 - residuo*/
            digitoVerificador = residuo == 0 ? 0 : modulo - residuo;

        /* ahora comparamos el elemento de la posicion 10 con el dig. ver.*/
            <%--
            if (pub == true) {
                if (digitoVerificador != d9) {
                    alertify.alert('* Datos de Registro de Empresa: *\n * El RUC de la empresa del sector público es incorrecto. *').set('label', 'Aceptar');
                    document.getElementById('<%=txtruccipas.ClientID %>').focus();
                 
                    document.getElementById("loader").className = 'nover';
                    return false;
                }
               
                if (numero.substr(9, 4) != '0001') {
                    alertify.alert('* Datos de Registro de Empresa: *\n * El RUC de la empresa del sector público debe terminar con 0001. *').set('label', 'Aceptar');
                    document.getElementById('<%=txtruccipas.ClientID %>').focus();
                
                    document.getElementById("loader").className = 'nover';
                    return false;
                }
            }
            else if (pri == true) {
                if (digitoVerificador != d10) {
                    alertify.alert('* Datos de Registro de Empresa: *\n * El RUC de la empresa del sector privado es incorrecto. *').set('label', 'Aceptar');
                    document.getElementById('<%=txtruccipas.ClientID %>').focus();
                   
                    document.getElementById("loader").className = 'nover';
                    return false;
                }
                if (numero.substr(10, 3) != '001') {
                    alertify.alert('* Datos de Registro de Empresa: *\n * El RUC de la empresa del sector privado debe terminar con 001. *').set('label', 'Aceptar');
                    document.getElementById('<%=txtruccipas.ClientID %>').focus();
                   
                    document.getElementById("loader").className = 'nover';
                    return false;
                }
            }
            else if (nat == true) {
                if (digitoVerificador != d10) {
                    alertify.alert('* Datos de Registro de Empresa: *\n * El número de cédula de la persona natural es incorrecto. *').set('label', 'Aceptar');
                    document.getElementById('<%=txtruccipas.ClientID %>').focus();
                  
                    document.getElementById("loader").className = 'nover';
                    return false;
                }
                if (numero.length > 10 && numero.substr(10, 3) != '001') {
                    alertify.alert('* Datos de Registro de Empresa: *\n * El RUC de la persona natural debe terminar con 001. *').set('label', 'Aceptar');
                    document.getElementById('<%=txtruccipas.ClientID %>').focus();
                
                    document.getElementById("loader").className = 'nover';
                    return false;
                }
            }--%>

            return true;
        }
        function validarEmail(email) {
            expr = /^([a-zA-Z0-9@;_\.\-])+\@(([a-zA-Z0-9@;\-])+\.)+([a-zA-Z0-9@;]{2,4})+$/; ;
            if (!expr.test(email)) {
                return false;
            }
            return true;
        }
        function get_action(form) {
            var v = grecaptcha.getResponse();
            if (v.length == 0) {
                alertify.alert("No se puede dejar vacío Código Captcha.").set('label', 'Aceptar');
                return false;
            }
            if (v.length != 0) {
                //alertify.alert("Captcha completed.");
                return true;
            }
        }
        function getGif() {
            document.getElementById('imagen').innerHTML = '<img alt="" src="../shared/imgs/loader.gif"/>';
            return true;
        }
        function getGifOculta() {
            document.getElementById('imagen').innerHTML = '<img alt="" src=""/>';
            return true;
        }

        function mostrarinformacion() {
             mpeLoading = $find('idmpeLoading');
    mpeLoading.show();
    mpeLoading._backgroundElement.style.zIndex += 10;
    mpeLoading._foregroundElement.style.zIndex += 10;
          
        }
    </script>
</body>
</html>
