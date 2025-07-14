<%@ Page Language="C#" Title="Registro de Fotos" MaintainScrollPositionOnPostback="true"  AutoEventWireup="true" CodeBehind="consultasolicitudcolaboradorfacial.aspx.cs" Inherits="CSLSite.cliente.consultasolicitudcolaboradorfacial" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">

<%--    <link href="../img/favicon2.png" rel="icon"/>
    <link href="../img/icono.png" rel="apple-touch-icon"/>
    <link href="../css/bootstrap.min.css" rel="stylesheet"/>
    <link href="../css/dashboard.css" rel="stylesheet"/>
    <link href="../css/icons.css" rel="stylesheet"/>
    <link href="../css/style.css" rel="stylesheet"/>
    <link href="https://fonts.googleapis.com/css2?family=Roboto:wght@300;400;500;700&display=swap" rel="stylesheet"/>

     <!--mensajes-->
        <link href="https://cdn.jsdelivr.net/npm/alertifyjs@1.11.0/build/css/alertify.min.css" rel="stylesheet"/>
        <script type="text/javascript" src="https://cdn.jsdelivr.net/npm/alertifyjs@1.11.0/build/alertify.min.js"></script>
        <script src="../lib/jquery/jquery.min.js" type="text/javascript"></script>--%>
      <link href="../img/favicon2.png" rel="icon"/>
  <link href="../img/icono.png" rel="apple-touch-icon"/>
  <link href="../css/bootstrap.min.css" rel="stylesheet"/>
  <link href="../css/dashboard.css" rel="stylesheet"/>
  <link href="../css/icons.css" rel="stylesheet"/>
  <link href="../css/style.css" rel="stylesheet"/>
  <link href="https://fonts.googleapis.com/css2?family=Roboto:wght@300;400;500;700&display=swap" rel="stylesheet"/>

      <!--external css-->
  <link href="../lib/font-awesome/css/font-awesome.css" rel="stylesheet" />
  <link rel="stylesheet" type="text/css" href="../lib/gritter/css/jquery.gritter.css" />

    <!--mensajes-->
  <link href="https://cdn.jsdelivr.net/npm/alertifyjs@1.11.0/build/css/alertify.min.css" rel="stylesheet"/>
  <script type="text/javascript" src="https://cdn.jsdelivr.net/npm/alertifyjs@1.11.0/build/alertify.min.js"></script>
  <script src="../lib/jquery/jquery.min.js" type="text/javascript"></script>



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
                <li class="breadcrumb-item active" aria-current="page" id="sub_opcion" runat="server">Formulario de Registro Facial</li>
              </ol>
            </nav>
        </div>

        <form id="bookingfrm" runat="server">
        
            <input id="zonaid" type="hidden" value="7" />
            <asp:ToolkitScriptManager ID="tkscata" runat="server"></asp:ToolkitScriptManager>

            <asp:HiddenField ID="manualHide" runat="server" />

            <div class="form-row">

                <div class="form-group col-md-12">
                    <a class="btn btn-outline-primary mr-4"  runat="server" id="a01" clientidmode="Static" >1</a>
                    <a class="level1"  runat="server" id="a1" clientidmode="Static" ><b>Instrucciones - Registro Facial</b></a>
                </div>

                <div class="form-group col-md-6">
                    <asp:TextBox ID="txtrazonsocial" Visible="false" runat="server" MaxLength="500" Enabled="false" style="text-align: center;" 
                    onkeypress="return soloLetras(event,'abcdefghijklnmñopqrstuvwxyz01234567890 ',true)"></asp:TextBox>
                </div> 
                 <div class="form-group col-md-12">
                        <div class="alert alert-info" style=" font-weight:bold">
                            Alinear el rostro con las líneas de guía y mantenga la mirada en la guía central mostranda en la pantalla
                        </div>
                    </div>

              <%--  <div id="imagenRpt" style="align-content:center;" runat="server" clientidmode="Static">
                        <p align="center">
                    <img src="../images/facial.jpg" alt ="" /></p>
                </div>--%>

            </div>
    
            <div id="xfinder" runat="server" visible="false">
                <div class="row">
                    <div class="form-group col-md-12">
                        <a class="btn btn-outline-primary mr-4"  runat="server" id="a6" clientidmode="Static" >2</a>
                        <a class="level1"  runat="server" id="ls6" clientidmode="Static" ><b>Subir Fotos</b></a>
                    </div>

                    <div class="form-group col-md-12">
                        <asp:Button ID="btnbuscardoc" runat="server" Visible="false" onclick="btnbuscardoc_Click" Text="Consultar Documentos" OnClientClick="mostrarsecexp();"
                            class="btn btn-outline-primary mr-4"/>
                    </div> 

                    <div class="form-group col-md-12">
                        <div class="alert alert-danger" style=" font-weight:bold">
                            Declaro que los datos consignados y suministrados en el presente documento son correctos y de procedencia lícita.
                            Autorizo a CONTECON GUAYAQUIL S.A. a solicitar confirmación de los mismos, en cualquier fuente de información y a
                            compartir esta información con las Autoridades que lo soliciten.
                        </div>
                    </div>
                    <div class="form-group col-md-12">
                         <asp:UpdatePanel ID="UPNotificacion" runat="server" UpdateMode="Conditional" >  
                            <ContentTemplate>
                                 <div id="msjNotificaciones" runat="server" class=" alert  alert-warning" visible="false" >
                                            No se encontraron resultados, 
                                            asegurese que exista los registros faciales.
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
                    <div class="bokindetalle" style="width:100%;overflow:auto">
      
                        <table id="tablerp" cellpadding="1" cellspacing="0">
                            <asp:Repeater ID="tablePaginationDocumentos" runat="server">
                                <HeaderTemplate>
                                <table id="tablar"  cellspacing="1" cellpadding="1" class="table table-bordered invoice">
                                <thead>
                                <tr>
                                    <th class="nover"></th>
                                    <th class="nover"></th>
                                    <th class="nover"></th>
                                    <th class="nover"></th>
                                    <th>Forma</th>
                                    <th>Documentos</th>
                                    <th>Escoja el archivo con formato indicado.</th>
                                    <th>Formato</th>
                                    <th>Observación</th>
                                </tr>
                                </thead> 
                                <tbody>
                                </HeaderTemplate>
                                <ItemTemplate>
                                <tr class="point">
                                    <td class="nover"><asp:TextBox ID="txtidSolicitud" runat="server" Text='<%#Eval("idSolicitud")%>' /></td>
                                    <td class="nover"><asp:TextBox ID="txtidSolcol" runat="server" Text='<%#Eval("idSolcol")%>' /></td>
                                    <td class="nover"><asp:TextBox ID="txtidentificacion" runat="server" Text='<%#Eval("identificacion")%>' /></td>
                                    <td class="nover"><asp:TextBox ID="txtsec" runat="server" Text='<%#Eval("secuencia")%>' /></td>
                                    <td class="nover"><asp:TextBox ID="txtdoc" runat="server" Text='<%#Eval("documento")%>' /></td>
                                    <td class="nover"><asp:TextBox ID="txtext" runat="server" Text='<%#Eval("extension")%>' /></td>
                                    <td style=" font-size:inherit" align="center"> <img src='<%#Eval("forma")%>' alt ="" /> </td>
                                    <td style=" font-size:inherit"><%#Eval("DOCUMENTO")%></td>
                                    <td >
                                        <asp:FileUpload extension='<%#Eval("EXTENSION")%>'  class="btn btn-outline-primary mr-4" id="fsupload" title="Escoja el archivo con formato indicado en la siguiente columna."
                                          onchange="validaextensionJG(this)" style=" font-size:small" runat="server"/>
                                    </td>
                                    <td ><%#Eval("EXTENSION")%></td>
                                    <td ><%#Eval("comentarios")%></td>
                                </tr>
                                </ItemTemplate>
                                <FooterTemplate>
                                </tbody>
                                </table>
                                </FooterTemplate>
                                </asp:Repeater>
                        </table>
                    </div>

               
       
                <div class="seccion">
                    <div></div>
                    <br/> <br/>
                    <div class="col-md-12 d-flex justify-content-center">
                        <img alt="loading.." src="../shared/imgs/loader.gif" id="loader" style=" display:none" />
                        <span id="imagen"></span>
                        <asp:Button ID="btsalvar" onclick="btsalvar_Click" runat="server" Text="Enviar Imagenes"   OnClientClick="return prepareObject('¿Esta seguro de enviar la solicitud?')"
                                ToolTip="Confirma la información y genera el envio del registro facial." CssClass="btn btn-primary"/>
                    </div>
                </div>
            </div>

            <div id="xResult" runat="server" visible="false">
                    <div class="bokindetalle" style="width:100%;overflow:auto">
      
                        <table id="tblResultado" cellpadding="1" cellspacing="0">
                            <asp:Repeater ID="tableResultado" runat="server">
                                <HeaderTemplate>
                                <table id="tablar"  cellspacing="1" cellpadding="1" class="table table-bordered invoice">
                                    <thead>
                                    <tr>
                                        <th class="nover"></th>
                                        <th class="nover"></th>
                                        <th class="nover"></th>
                                        <th class="nover"></th>
                                        <th class="nover"></th>
                                        <th class="nover"></th>
                                        <th class="nover"></th>
                                        <th class="nover"></th>
                                        <th>Forma</th>
                                        <th>Documentos</th>
                                        <th></th>
                                        <th>Formato</th>
                                        <th>Estado</th>
                                        <th>Comentario</th>
                                    </tr>
                                    </thead> 
                                    <tbody>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                    <tr class="point">
                                        <td class="nover"><asp:TextBox ID="txtID" runat="server" Text='<%#Eval("id")%>' /></td>
                                        <td class="nover"><asp:TextBox ID="txtidSolicitud" runat="server" Text='<%#Eval("idSolicitud")%>' /></td>
                                        <td class="nover"><asp:TextBox ID="txtidSolcol" runat="server" Text='<%#Eval("idSolcol")%>' /></td>
                                        <td class="nover"><asp:TextBox ID="txtidentificacion" runat="server" Text='<%#Eval("identificacion")%>' /></td>
                                        <td class="nover"><asp:TextBox ID="txtsec" runat="server" Text='<%#Eval("secuencia")%>' /></td>
                                        <td class="nover"><asp:TextBox ID="txtdoc" runat="server" Text='<%#Eval("documento")%>' /></td>
                                        <td class="nover"><asp:TextBox ID="txtext" runat="server" Text='<%#Eval("extension")%>' /></td>
                                        <td class="nover"><asp:TextBox ID="txtRuta" runat="server" Text='<%#Eval("ruta")%>' /></td>
                                        <td style=" font-size:inherit" align="center"> <img src='<%#Eval("forma")%>' alt ="" /> </td>
                                        <td style=" font-size:inherit"><%#Eval("DOCUMENTO")%></td>
                                        <%--<td >
                                            <asp:FileUpload extension='<%#Eval("EXTENSION")%>'  class="btn btn-outline-primary mr-4" id="fsupload" title="Escoja el archivo con formato indicado en la siguiente columna."
                                                    onchange="validaextension(this)" style=" font-size:small" runat="server"/>
                                        </td>--%>
                                         <td >
                                            <a href='<%#Eval("ruta") %>'  class="topopup" target="_blank">
                                                <i class="fa fa-search"></i> Ver Imagen </a>
                                        </td>
                                        <td ><%#Eval("EXTENSION")%></td>
                                        <td ><%#Eval("EstadoDesc")%></td>
                                        <td ><%#Eval("comentarios")%></td>
                                    </tr>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                    </tbody>
                                </table>
                                </FooterTemplate>
                                </asp:Repeater>
                        </table>
                    </div>
                    <div class="col-md-12 d-flex justify-content-rigth" runat="server" id="salir">
                        <asp:Button ID="btnSalir" runat="server" Text="Regresar" onclick="btnSalir_Click" ToolTip="Regresa a la Pantalla Consultar Solicitud." CssClass="btn btn-outline-primary mr-4"/>
                       
                        <asp:Button Visible="false" ID="btnReenviar" runat="server" Text="Corregir" onclick="btnReenviar_Click" ToolTip="Habilita opción de reenviar." CssClass="btn btn-primary"/>
                    </div>

                    
            </div>


           
        </form>

     </div>
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.6.2/jquery.min.js"></script>
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
   

    <script src="../Scripts/pages.js" type="text/javascript"></script>
    <script src="../Scripts/credenciales.js" type="text/javascript"></script>


    <script type="text/javascript">


        var ced_count = 0;
        var jAisv = {};
        var valida = "";



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
                <%--var chk = document.getElementById('<%=cbltipousuario.ClientID %>');
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
                    
                    document.getElementById("loader").className = 'nover';
                    return false;
                }--%>

              
              
                //getGif();
                //document.getElementById("loader").className = '';
                return true;
            } catch (e) {
                alertify.alert('Lamentamos comunicarle que ha ocurrido el siguiente problema:\n' + e.Message).set('label', 'Aceptar');
            }
        }
        function popupCallback(data, control) {
            this.document.getElementById(control).value = data;
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



    <script type="text/javascript">
    //Filevalidation = () => {
    //    const fi =  document.getElementById('fsupload');
        
    //    // Check if any file is selected.
    //    if (fi.files.length > 0) {
    //        for (const i = 0; i <= fi.files.length - 1; i++) {
  
    //            const fsize = fi.files.item(i).size;
    //            const file = Math.round((fsize / 1024));
    //            // The size of the file.
    //            if (file >= 2048) {
    //                alert(
    //                  "Archivo demasiado grande, seleccione un archivo de menos de 2 MB");
    //            } else if (file < 100) {
    //                alert(
    //                  "Archivo demasiado pequeño, seleccione un archivo de más de 10 kb");
    //            } else {
    //                document.getElementById('size').innerHTML = '<b>'
    //                + file + '</b> KB';
    //            }
    //        }
    //    }
    //    }

    function validaextensionJG(control) {
        var ext = control.getAttribute("extension");
        //alert(extension);
        //return false;
        extensiones_permitidas = new Array(ext);
        archivo = control.value;
        mierror = "";
        if (!archivo) {
            //Si no tengo archivo, es que no se ha seleccionado un archivo en el formulario 
            mierror = "No has seleccionado ningún archivo";
        } else {
            //recupero la extensión de este nombre de archivo 
            var extension = (archivo.substring(archivo.lastIndexOf("."))).toLowerCase();

       
            //alert (extension); 
            //compruebo si la extensión está entre las permitidas 
            permitida = false;
            for (var i = 0; i < extensiones_permitidas.length; i++) {

           
                if (extensiones_permitidas[i] == extension) {
                    permitida = true;
                    break;
                }
            }
            if (!permitida) {
                mierror = "Comprueba la extensión de los archivos a subir. \nSólo se pueden subir archivos con extensiones: " + extensiones_permitidas.join();
                control.value = null;
            } else {
                //submito! 
               // alert("Todo correcto. Voy a submitir el formulario.");

                //////////////////////////
                //validacion de tamaño
                //////////////////////////
                const fi = control;
      
                // Check if any file is selected.
                if (fi.files.length > 0) {
                    for (const i = 0; i <= fi.files.length - 1; i++) {
  
                        const fsize = fi.files.item(i).size;
                        const file = Math.round((fsize / 1024));

                        // alert(file);
                        // The size of the file.
                        if (file > 300) {
                            alert("Archivo demasiado grande, seleccione un archivo de menos de 301 KB. Resolución máxima de la foto 1280 x 720");
                            mierror = "Archivo demasiado grande, seleccione un archivo de menos de 301 KB. Resolución máxima de la foto 1280 x 720";
                            control.value = null;
                        }
                        else
                        {
                            if (file < 12)
                            {
                                alert("Archivo demasiado pequeño, seleccione un archivo de más de 12 KB. Resolución de la foto mínima 640 x 480");
                                mierror = "Archivo demasiado pequeño, seleccione un archivo de más de 12 KB. Resolución de la foto mínima 640 x 480";
                                control.value = null;
                            } else
                            {
                               control.tooltip = control.value;
                                return 1;
                            }
                        }
                    }
                }
            }
        }
        //si estoy aqui es que no se ha podido submitir 
        alert(mierror);
        return 0;
    }
</script>

</body>
</html>
