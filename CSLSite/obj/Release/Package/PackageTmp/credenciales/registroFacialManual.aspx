<%@ Page Language="C#" MasterPageFile="~/site.Master" Title="Registro Facial Manual" MaintainScrollPositionOnPostback="true"
         AutoEventWireup="true" CodeBehind="registroFacialManual.aspx.cs" Inherits="CSLSite.registroFacialManual" %>
         <%@ MasterType VirtualPath="~/site.Master" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="placehead" runat="server">
    <link href="../img/favicon2.png" rel="icon"/>
    <link href="../img/icono.png" rel="apple-touch-icon"/>
    <link href="../css/bootstrap.min.css" rel="stylesheet"/>
    <link href="../css/dashboard.css" rel="stylesheet"/>
    <link href="../css/icons.css" rel="stylesheet"/>
    <link href="../css/style.css" rel="stylesheet"/>
    <link href="https://fonts.googleapis.com/css2?family=Roboto:wght@300;400;500;700&display=swap" rel="stylesheet"/>
    
    <link href="../shared/estilo/jquery.datetimepicker.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/jquery.datetimepicker.js" type="text/javascript"></script>

    <!--external css-->
    <link href="../lib/font-awesome/css/font-awesome.css" rel="stylesheet" />
    <link rel="stylesheet" type="text/css" href="../lib/gritter/css/jquery.gritter.css" />

    <!--mensajes-->
    <link href="https://cdn.jsdelivr.net/npm/alertifyjs@1.11.0/build/css/alertify.min.css" rel="stylesheet"/>

    <style type="text/css">
        body {
          font-family: Arial, Helvetica, sans-serif;
        }

        /* Dashed border */
        hr.dashed {
          border-top: 3px dashed #bbb;
        }

        /* Dotted border */
        hr.dotted {
          border-top: 3px dotted #bbb;
        }

        /* Solid border */
        hr.solid {
          border-top: 3px solid #bbb;
        }

        /* Rounded border */
        hr.rounded {
          border-top: 8px solid #bbb;
          border-radius: 5px;
        }
    </style>
</asp:Content>
<asp:Content ID="formcolaborador" ContentPlaceHolderID="placebody" runat="server">
    
    <input id="zonaid" type="hidden" value="7" />
    <asp:ScriptManager ID="sMan" EnableScriptGlobalization="true" runat="server"></asp:ScriptManager>
    <noscript><meta http-equiv="refresh" content="0; url=sinsoporte.htm" /> </noscript>

    <div class="mt-4">         
        <nav class="mt-4" aria-label="breadcrumb">
          <ol class="breadcrumb">
            <li class="breadcrumb-item" id="opcion_principal" runat="server"><a href="#">MCA - Módulo de Control de Acceso</a></li>
            <li class="breadcrumb-item active" aria-current="page" id="sub_opcion" runat="server">Registro Facial Manual</li>
          </ol>
        </nav>
    </div>

    <div class="dashboard-container p-4" id="cuerpo" runat="server">

        <div class="form-row">
            <div class="form-group col-md-12">
                <div class="alert alert-danger" id="banmsg" runat="server" visible="false" clientidmode="Static"><b>Error!</b> Debe ingresar el codigo......</div>
            </div>
        </div>
    
        <div class="form-row">
            <div class="form-title col-md-12">
                <a class="btn btn-outline-primary mr-4" href="#" target="_blank" runat="server" id="aprint" clientidmode="Static" >1</a>
                <a class="level1" target="_blank" runat="server" id="a1" clientidmode="Static" >Lista de Empresas</a>
            </div>

            <div class="form-group col-md-12 d-flex">
                <label for="inputAddress"><span style="color: #FF0000; font-weight: bold;"> *</span></label>
                <%--<asp:DropDownList runat="server" ID="ddlTipoEmpresa" class="form-control" onchange="fEmpresa();valdltipsol(this, valtipempresa);" >
                        <asp:ListItem Value="0">* Seleccione origen *</asp:ListItem>
                </asp:DropDownList>--%>
                <asp:TextBox ID="TxtEmpresa" placeholder="INGRESE NOMBRE DE EMPRESA"   runat="server" class="form-control"   autocomplete="off"  oncopy="return false;" onpaste="return false;"  
                                onkeypress="return soloLetras(event,'abcdefghijklmnñopqrstuvwxyz0123456789_')"></asp:TextBox>
                <asp:HiddenField ID="IdTxtempresa" runat="server" ClientIDMode="Static"/> 
                <span id='valtipempresa' class="validacion" ></span>
            </div>
        </div>

        <div class="form-row">
            <div class="form-title col-md-12">
                <a class="btn btn-outline-primary mr-4" href="#" target="_blank" runat="server" id="a2" clientidmode="Static" >2</a>
                <a class="level1" target="_blank" runat="server" id="a3" clientidmode="Static" >Busqueda de Colaborador</a>
            </div>
        
            <div class="form-group col-md-12">
                <asp:TextBox ID="TxtChofer" placeholder="INGRESE APELLIDO DEL COLABORADOR"   runat="server" class="form-control"   autocomplete="off"  oncopy="return false;" onpaste="return false;"  
                                onkeypress="return soloLetras(event,'abcdefghijklmnñopqrstuvwxyz0123456789_')"></asp:TextBox>
                <asp:HiddenField ID="IdTxtChofer" runat="server" ClientIDMode="Static"/>        
            </div>
        </div>
   
        <div class="form-row">
            <div class="form-title col-md-12">
                <a class="btn btn-outline-primary mr-4" href="#" target="_blank" runat="server" id="a4" clientidmode="Static" >3</a>
                <a class="level1" target="_blank" runat="server" id="a5" clientidmode="Static" >Datos Generales del Colaborador</a>
            </div>
        </div>

        <div class="form-group col-md-12"> 
   
            <div class="informativo" id="colector">
   
                <div class="bokindetalle" style=" width:100%; overflow:auto">
                    <asp:GridView runat="server" id="gvColaboradores" class="table table-bordered invoice" OnRowCommand="gvColaboradores_RowCommand" AutoGenerateColumns="False" Width="100%">
                        <Columns>

                            <asp:TemplateField ShowHeader="False"  HeaderText="Imagen 1" FooterText="Imagen 1" HeaderImageUrl="~/images/facial1.jpg">
                                <ItemTemplate>
                                    <asp:FileUpload extension=".jpg"  class="btn btn-outline-primary mr-4"
                                        id="fsupload" title="Escoja el archivo con formato indicado en la siguiente columna." 
                                        onchange="validaextensionJG(this)" style=" font-size:small" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:templatefield showheader="false"  headertext="imagen 2" HeaderImageUrl="~/images/facial2.jpg">
                                <itemtemplate>
                                    <asp:fileupload extension=".jpg" class="btn btn-outline-primary mr-4" 
                                        id="fsupload1" title="escoja el archivo con formato indicado en la siguiente columna." 
                                        onchange="validaextensionjg(this)" style=" font-size:small" runat="server"/>
                                </itemtemplate>
                            </asp:templatefield>
                            <asp:templatefield showheader="false"  headertext="imagen 3" HeaderImageUrl="~/images/facial3.jpg">
                                <itemtemplate>
                                    <asp:fileupload extension=".jpg"  class="btn btn-outline-primary mr-4" 
                                        id="fsupload2" title="escoja el archivo con formato indicado en la siguiente columna." 
                                        onchange="validaextensionjg(this)" style=" font-size:small" runat="server"/>
                                </itemtemplate>
                            </asp:templatefield>
                                       
                        </Columns>
                    </asp:GridView>
                </div>    
            </div>
    
        </div>

        <div class="form-row">
            <div class="form-group col-md-12">
                        <div class="alert alert-danger" id="banmsg_det" visible="false" runat="server" clientidmode="Static"><b>Error!</b><br/>Debe ingresar el codigo......</div>
            </div>
        </div>

        <div class="col-md-12 d-flex justify-content-center">
            <asp:UpdatePanel ID="UPDET" runat="server" UpdateMode="Conditional" >  
                <ContentTemplate>
                    <asp:Button ID="btnVerRegistroFacial" runat="server" Text="Ver Registro Facial" ClientIDMode="Static" OnClick="btnVerRegistroFacial_Click" CssClass="btn btn-outline-primary2" data-toggle="modal" data-target="#myModal"/>
                </ContentTemplate>
            </asp:UpdatePanel>
            &nbsp;&nbsp;
            <asp:Button ID="btsalvar" runat="server" Text="Registro Facial" ClientIDMode="Static"
                        OnClientClick="return prepareObject('¿Esta seguro de enviar la solicitud?');" CssClass="btn btn-primary" OnClick="btsalvar_Click"
                        ToolTip="Confirma la información y genera el REGISTRO FACIAL."/>
        </div>


   </div>

    <div id="myModal" class="modal fade" tabindex="-1" role="dialog">
        <div class="modal-dialog" role="document" style="max-width: 980px"> <!-- Este tag style controla el ancho del modal -->
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title">REGISTRO FOTOGRÁFICO</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                    <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <asp:UpdatePanel ID="UPFotos" runat="server" UpdateMode="Conditional" >  
                    <ContentTemplate>
                        <div class="modal-body">
          
                            <div id="xfinderDes" runat="server" visible="true" >
            <br />
                                <table border="1">
                                    
                                    <tr>
                                        <th>
                                           <img alt="loading.." src="../images/facial1.jpg" runat="server" height="400" width="300"  id="ImgFoto1"/>
                                           <%-- <img id="img" src="#" runat="server" alt=""/>--%>
                                        </th>
                                        <th>
                                            <img alt="loading.." src="../images/facial2.jpg" runat="server" height="400" width="300" id="ImgFoto2"/>
                                        </th>

                                        <th>
                                           <img alt="loading.." src="../images/facial3.jpg" runat="server" height="400" width="300" id="ImgFoto3"/>
                                        </th>
                                    </tr>
                                    
                                </table>
                      <br />        
                            </div>
                            <div id="sinresultadoDespacho" runat="server" class=" alert  alert-warning" visible="false" >
                                No se encontraron resultados, 
                                asegurese que exista los registros faciales.
                            </div>
                           
                   
                        </div>
                        <div class="modal-footer d-flex justify-content-center">
                            <button type="button" class="btn btn-outline-primary " data-dismiss="modal">Salir</button>
                        </div>
                    </ContentTemplate>   
                </asp:UpdatePanel>
            </div>
        </div>
    </div>

   <asp:HiddenField runat="server" ID="emailsec2" />
    <script src="../Scripts/pages.js" type="text/javascript"></script>
    <script src="../Scripts/credenciales.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.tablesorter.min.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.tablesorter.pager.js" type="text/javascript"></script>
    <script src='../shared/avisos/popup_script.js' type='text/javascript' ></script> 
    <script type="text/javascript" src='../lib/autocompletar/bootstrap3-typeahead.min.js'></script>

    
<script type="text/javascript">

         $(function () {
        $('[id*=TxtEmpresa]').typeahead({
            hint: true,
            highlight: true,
            minLength: 5,
            source: function (request, response) {
                $.ajax({
                    url: '<%=ResolveUrl("registroFacialManual.aspx/GetEmpresas") %>',
                    data: "{ 'prefix': '" + request + "'}",
                    dataType: "json",
                    dataType: "json",
                    type: "POST",
                    maxJsonLength: 1,
                    contentType: "application/json; charset=utf-8",
                    success: function (data) {
                        items = [];
                        map = {};
                        $.each(data.d, function (i, item) {
                            var id = item.split('+')[0];
                            var name = item.split('+')[1];
                            map[name] = { id: id, name: name };
                            items.push(name);
                        });
                        response(items);
                        $(".dropdown-menu").css("height", "auto");
                    },
                    error: function (response) {
                        alert(response.responseText);
                    },
                    failure: function (response) {
                        alert(response.responseText);
                    }
                });
            },
            updater: function (item) {
                $('[id*=IdTxtempresa]').val(map[item].id);
                //$('[id*=Txtempresa]').val(map[item].name);
                //alert(map[item].id);
                //alert(map[item].name);
                //alert($("#IdTxtempresa").val());
                //alert($("#Txtempresa").val());
                return item;
            }
        });
     });



     $(function () {
        $('[id*=TxtChofer]').typeahead({
            hint: true,
            highlight: true,
            minLength: 5,
            source: function (request, response) {
                $.ajax({
                    url: '<%=ResolveUrl("registroFacialManual.aspx/GetColaborador") %>',
                    data: "{ 'prefix': '" + request + "', 'empresa' : '" + $("#IdTxtempresa").val() + "' }",
                    dataType: "json",
                    type: "POST",
                    maxJsonLength: 1,
                    contentType: "application/json; charset=utf-8",
                    success: function (data) {
                        items = [];
                        map = {};
                        $.each(data.d, function (i, item) {
                            var id = item.split('+')[0];
                            var name = item.split('+')[1];
                            map[name] = { id: id, name: name };
                            items.push(name);
                        });
                        response(items);
                        $(".dropdown-menu").css("height", "auto");
                    },
                    error: function (response) {
                        alert(response.responseText);
                    },
                    failure: function (response) {
                        alert(response.responseText);
                    }
                });
            },
            updater: function (item) {
                $('[id*=IdTxtChofer]').val(map[item].id);
                //alert(map[item].id);
                //alert(map[item].name);
                //alert($("#IdTxtChofer").val());
                return item;
            }
        });
     });
        

     function validaextensionJG(control) {
        var ext = control.getAttribute("extension");

        extensiones_permitidas = new Array(ext);
        archivo = control.value;
        mierror = "";
        if (!archivo) {
            //Si no tengo archivo, es que no se ha seleccionado un archivo en el formulario 
            mierror = "No has seleccionado ningún archivo";
        } else {
            //recupero la extensión de este nombre de archivo 
            var extension = (archivo.substring(archivo.lastIndexOf("."))).toLowerCase();

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
                            alert(
                              "Archivo demasiado grande, seleccione un archivo de menos de 301 KB. Resolución máxima de la foto 1280 x 720");
                        } else if (file < 12) {
                            alert(
                              "Archivo demasiado pequeño, seleccione un archivo de más de 12 KB. Resolución de la foto mínima 640 x 480");
                        } else {
                           control.tooltip = control.value;
                            return 1;
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



    <script type="text/javascript">
        var ced_count = 0;
        var jAisv = {};
        $(document).ready(function () {
            $('.datetimepicker').datetimepicker().datetimepicker({ lang: 'es', timepicker: false, closeOnDateSelect: true, format: 'd/m/Y' });
        });
        $(window).load(function () {
            $(document).ready(function () {
                //colapsar y expandir
                $("div.colapser").toggle(function () { $(this).removeClass("colapsa").addClass("expande"); $(this).next().hide(); }
                                  , function () { $(this).removeClass("expande").addClass("colapsa"); $(this).next().show(); });
            });
        });
        
        var registroempresa = {};
        var lista = [];
        var cblarray = [];
        var carray = [];
        function prepareObject(valor) {
            try {
                if (confirm(valor) == false) {
                    return false;
                }
                var vals = document.getElementById('<%=TxtEmpresa.ClientID %>').value;
                if (vals == 0) {
                    alert('* Escriba el nombre de la empresa *');
                    document.getElementById('<%=TxtEmpresa.ClientID %>').focus();
                    <%--document.getElementById('<%=ddlTipoEmpresa.ClientID %>').style.cssText = "background-color:#ffffc6;color:Red;width:400px;";--%>
                    document.getElementById("loader").className = 'nover';
                    return false;
                }
                 var vals = document.getElementById('<%=TxtChofer.ClientID %>').value;
                if (vals == '' || vals == null || vals == undefined) {
                    alert('* Escriba los datos del colaborador *');
                    document.getElementById('<%=TxtChofer.ClientID %>').focus();
                    document.getElementById("loader").className = 'nover';
                    return false;
                }
                var vals = document.getElementById('<%=IdTxtChofer.ClientID %>').value;
                if (vals == '' || vals == null || vals == undefined) {
                    alert('* Elija un colaborador *');
                    document.getElementById('<%=IdTxtChofer.ClientID %>').focus();
                   
                    document.getElementById("loader").className = 'nover';
                    return false;
                }

                document.getElementById('<%=TxtChofer.ClientID %>').focus();
                return true;
            } catch (e) {
                alert('Lamentamos comunicarle que ha ocurrido el siguiente problema:\n' + e.Message);
            }
        }
        function Panel() {
            $(document).ready(function () {
                $('#ventana_popup').fadeIn('slow');
                $('#popup-overlay').fadeIn('slow');
                $('#popup-overlay').height($(window).height());
            });
        }
        function validaCabecera() {
            try {
                //Valida los datos de las secciones
                var vals = document.getElementById('<%=TxtEmpresa.ClientID %>').value;
                if (vals == 0) {
                    alert('* Seleccione la Empresa *');
                    document.getElementById('<%=TxtEmpresa.ClientID %>').focus();
                   
                    document.getElementById("loader").className = 'nover';
                    return false;
                }
             
                var vals = document.getElementById('<%=TxtChofer.ClientID %>').value;
                if (vals == '' || vals == null || vals == undefined) {
                    alert('* Escriba los datos del colaborador *');
                    document.getElementById('<%=TxtChofer.ClientID %>').focus();
                   
                    document.getElementById("loader").className = 'nover';
                    return false;
                }
                var vals = document.getElementById('<%=IdTxtChofer.ClientID %>').value;
                if (vals == '' || vals == null || vals == undefined) {
                    alert('* Elija un colaborador *');
                    document.getElementById('<%=IdTxtChofer.ClientID %>').focus();
                   
                    document.getElementById("loader").className = 'nover';
                    return false;
                }

                if (!valedad()) {
                    return false;
                };
               
               
                document.getElementById("loader").className = '';
                return true;
            } catch (e) {
                alert('Lamentamos comunicarle que ha ocurrido el siguiente problema..:\n' + e.Message);
            }
        }

        function validNumericos(inputtxt) {
            var valor = inputtxt.value;
                 for(i=0;i<valor.length;i++){
                     var code=valor.charCodeAt(i);
                         if(code<=48 || code>=57){          
                           return;
                         }    
                   }
        }

        function redireccionar() {
            window.locationf = "~/cuenta/menu.aspx";
        }
        
        }
        function modalPanel() {
            document.getElementById('close').style.display = "block";
            window.location = '../cuenta/menu.aspx';
        }
        function move(incio, intervalo) {
            document.getElementById('divpb').style.display = "block";
            var elem = document.getElementById("myBar");
            var width = incio;
            var id = setInterval(frame, 15);
            function frame() {
                if (width >= intervalo) {
                    clearInterval(id);
                } else {
                    width++;
                    elem.style.width = width + '%';
                    document.getElementById("demo").innerHTML = width * 1 + '%';
                }
            }
            return true;
        }
    </script>
    <div id="ventana_popup" style=" display:none;">
    <div id="ventana_content-popup">
            <div>
             <table style=" width:100%">
             <tr><td align="center"><p id='manage_ventana_popup'><asp:Label Text="Procesando!" id="lblProgreso" runat="server" /></p></td></tr>
             </table>
		        <div id='borde_ventana_popup' style=" height:267px"> 
                <br /><br /><br /><br />
			    <div  style='text-align:center;' >
                    <div id="divpb">
                    <div class="w3-progress-container">
                        <div id="myBar" class="w3-progressbar w3-green" style="width:0%">
                        </div>
                    </div>
                    <p id="demo">0%</p>
                    </div>
			    </div>
			    <br/>
			     <div  id="close" style=" display:none" >SALIR</div>
  			    </div>
            </div>
        </div>
    </div>
    <div id="popup-overlay"></div>
</asp:Content>
