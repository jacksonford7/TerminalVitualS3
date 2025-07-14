<%@ Page Language="C#" MasterPageFile="~/site.Master" Title="Solicitud de Permisos a Credenciales Activas OPC" MaintainScrollPositionOnPostback="true"
         AutoEventWireup="true" CodeBehind="solicitudCredencialesActivasOPC.aspx.cs" Inherits="CSLSite.solicitudCredencialesActivasOPC" %>
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

     <!--external css-->
    <link href="../lib/font-awesome/css/font-awesome.css" rel="stylesheet" />
    <link rel="stylesheet" type="text/css" href="../lib/gritter/css/jquery.gritter.css" />

    <!--mensajes-->
    <link href="https://cdn.jsdelivr.net/npm/alertifyjs@1.11.0/build/css/alertify.min.css" rel="stylesheet"/>
    
    <link href="../shared/estilo/jquery.datetimepicker.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/jquery.datetimepicker.js" type="text/javascript"></script>

    <script type="text/javascript">

        function BindFunctions()
        {
            $(document).ready(function () {
                $('.datetimepicker').datetimepicker().datetimepicker({ lang: 'es', timepicker: false, step: 30, format: 'd/m/Y' });
            });    
        }

    </script>

   

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
            <li class="breadcrumb-item active" aria-current="page" id="sub_opcion" runat="server">Renovación de Permisos de Credenciales OPC</li>
          </ol>
        </nav>
    </div>

    <div class="dashboard-container p-4" id="cuerpo" runat="server">
        <asp:UpdatePanel ID="UPCAB" runat="server" UpdateMode="Conditional" >  
            <ContentTemplate>
                <div class="form-row">
                    <div class="form-group col-md-12">
                        <div class="alert alert-danger" id="banmsg" runat="server" visible="false" clientidmode="Static"><b>Error!</b> Debe ingresar el codigo......</div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>   

        <div class="form-row">
            <div class="form-title col-md-12">
                <a class="btn btn-outline-primary mr-4" href="#" target="_blank" runat="server" id="aprint" clientidmode="Static" >1</a>
                <a class="level1" target="_blank" runat="server" id="a1" clientidmode="Static" >Datos de Compañia</a>
            </div>

            <div class="form-group col-md-12 d-flex">
                <label for="inputAddress"><span style="color: #FF0000; font-weight: bold;"> *</span></label>
                
                <asp:TextBox diseable ReadOnly="true" ID="TxtEmpresa" placeholder="INGRESE NOMBRE DE EMPRESA"   runat="server" class="form-control"   autocomplete="off"  oncopy="return false;" onpaste="return false;"  
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
                        <asp:HiddenField ID="txtColaborador" runat="server" ClientIDMode="Static"/>   
                    
            </div>

            <div class="col-md-12 d-flex justify-content-center">
                        <asp:Button ID="btnAgregar" runat="server" Text="Agregar" ClientIDMode="Static"
                        CssClass="btn btn-primary" OnClick="btnAgregar_Click"
                        ToolTip="Agrega Colaborador a la lista"/>
            </div>
        </div>

        <div class="form-row">
            <div class="form-title col-md-12">
                <a class="btn btn-outline-primary mr-4" href="#" target="_blank" runat="server" id="a4" clientidmode="Static" >3</a>
                <a class="level1" target="_blank" runat="server" id="a5" clientidmode="Static" >Listado del Colaborador Asignados</a>
            </div>
        </div>

        <div class="form-group col-md-12"> 
   
        
            <div class="cataresult">
       
               <%--<script type="text/javascript">Sys.Application.add_load(BindFunctions); </script>--%>

              <%--  <asp:UpdatePanel ID="upresult" runat="server" ChildrenAsTriggers="true">
                <ContentTemplate>--%>
                <input id="idlin" type="hidden" runat="server" clientidmode="Static" />
                <input id="diponible" type="hidden" runat="server" clientidmode="Static" />
                <div class="msg-alerta" id="alerta" runat="server" style=" display:none" ></div>

                <asp:UpdatePanel ID="uptabla" runat="server" ChildrenAsTriggers="true" >
                    <ContentTemplate>

                        <div id="xfinder" runat="server" visible="true" >
                            <div class="findresult" >
                                <div class="informativo" style=" height:100%;">
                                    <div class="bokindetalle" style="width:100%;overflow:auto" >
                                        <asp:Repeater ID="tablePagination" runat="server" OnItemCommand="tablePagination_ItemCommand" >
                                            <HeaderTemplate>
                                                <table   cellspacing="1"  border="solid" cellpadding="1" class="table table-bordered table-sm table-contecon" id="tablePagination">
                                                <thead>
                                                <tr>
                                                    <th>Código</th>
                                                    <th>Identificación</th>
                                                    <th>Apellidos</th>
                                                    <th>Nombre</th>
                                                    <th class="nover">Tipo</th>
                                                    <th class="nover">Empresa</th>
                                                    <th class="nover">Area</th>
                                                    <th class="nover">Cargo</th>
                                                    <th></th>
                                                </tr>
                                                </thead> 
                                                <tbody>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <tr class="point" >
                                                    <td title="# Solicitud"><%#Eval("ID")%></td>
                                                    <td title="Tipo de Empresa"><%#Eval("CEDULA")%></td>
                                                    <td title="Apellidos"><%#Eval("APELLIDOS")%></td>
                                                    <td title="Nombresr"><%#Eval("NOMBRES")%></td>
                                                    <td class="nover" title="Tipo"><%#Eval("TIPO")%></td>
                                                    <td class="nover" title="Empresa"><%#Eval("EMPRESA")%></td>
                                                    <td class="nover" title="Area"><%#Eval("AREA")%></td>
                                                    <td class="nover" title="Cargo"><%#Eval("CARGO")%></td>
                                                    <%--<td title="Quitar" >
                                                        <a id="btnQuitar" class="btn btn-outline-primary mr-4" >
                                                        <i class="fa fa-close" ></i> Quitar 
                                                        </a>
                   
                                                    </td>--%>
                                                    <td scope="row" title="Quitar" > 
                                                        <asp:Button runat="server" ID="btnQuitar" Height="55px" CommandName="Quitar" Text="Quitar" 
                                                            CommandArgument='<%# Eval("ID") + "," + Eval("CEDULA")     %>'  
                                                            class="btn btn-primary"   />
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
                        <div class="form-row">
	                        <div class="form-group col-md-12"> 
                                <div id="sinresultado" runat="server" visible="false" class=" alert alert-info"></div>
	                        </div>
	                    </div>
                    </ContentTemplate>
                    <Triggers>
                        <%--<asp:AsyncPostBackTrigger  ControlID="btbuscar"/>--%>
                       <%-- <asp:AsyncPostBackTrigger ControlID="btcancel" />--%>
 
             
                     </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>

        <div class="form-row">
            <div class="form-title col-md-12">
                <a class="btn btn-outline-primary mr-4" href="#" target="_blank" runat="server" id="a6" clientidmode="Static" >4</a>
                <a class="level1" target="_blank" runat="server" id="a7" clientidmode="Static" >Horarios de Accesos</a>
            </div>

<%--            <div class="form-group col-md-12 d-flex">
                <label for="inputAddress"><span style="color: #FF0000; font-weight: bold;"> *</span></label>
                
                

            </div>
--%>
            <div class="form-group col-md-2">
                <label for="inputEmail4">Fecha de Ingreso:<span style="color: #FF0000; font-weight: bold;"> *</span></label>
                <asp:TextBox ID="txtfecing" runat="server" MaxLength="10" CssClass="datetimepicker form-control" onkeypress="return soloLetras(event,'01234567890/')" style="text-align: center"  ClientIDMode="Static"></asp:TextBox> 
                <span id="valfecing" class="validacion"></span>
            </div> 
            <div class="form-group col-md-2">
                <label for="inputEmail4">Fecha de Caducidad:<span style="color: #FF0000; font-weight: bold;"> *</span></label>
                <asp:TextBox ID="txtfecsal" runat="server" MaxLength="10" CssClass="datetimepicker form-control" onkeypress="return soloLetras(event,'01234567890/')" style="text-align: center"  ClientIDMode="Static"></asp:TextBox> 
                <span id="valfecsal" class="validacion"> </span>
            </div> 
        </div>


        <asp:UpdatePanel ID="UPDET" runat="server" UpdateMode="Conditional" >  
            <ContentTemplate>
                <div class="form-row">
                    <div class="form-group col-md-12">
                        <div class="alert alert-danger" id="banmsg_det" visible="false" runat="server" clientidmode="Static"><b>Error!</b><br/>Debe ingresar el codigo......</div>
                    </div>
                </div>

                <div class="col-md-12 d-flex justify-content-center">
                    <asp:Button ID="btnNuevo" runat="server" Text="Nueva Solicitud" ClientIDMode="Static" CssClass="btn btn-outline-primary2" OnClick="btnNuevo_Click" ToolTip="Agrega Colaborador a la lista"/>
                    &nbsp;&nbsp;
                    <asp:Button ID="btnSalvar" runat="server" Text="Enviar Solicitud" ClientIDMode="Static"  CssClass="btn btn-primary" OnClick="btnSalvar_Click"/>
                    
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>

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
    $('[id*=TxtChofer]').typeahead({
        hint: true,
        highlight: true,
        minLength: 5,
        source: function (request, response) {
            $.ajax({
                url: '<%=ResolveUrl("solicitudCredencialesActivasOPC.aspx/GetColaborador") %>',
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
            $('[id*=txtColaborador]').val(map[item].name);
            //alert(map[item].id);
            //alert(map[item].name);
            //alert($("#IdTxtChofer").val());
            return item;
        }
    });
    });
        
    </script>

    <script type="text/javascript">
    var ced_count = 0;
    var jAisv = {};
    //$(document).ready(function () {
    //    $('.datetimepicker').datetimepicker().datetimepicker({ lang: 'es', timepicker: false, closeOnDateSelect: true, format: 'd/m/Y' });
    //});
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

    <script type="text/javascript">
    $(document).ready(function () {
            $('.datetimepicker').datetimepicker().datetimepicker({ lang: 'es', timepicker: false, closeOnDateSelect: true, format: 'd/m/Y' });
        });    
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
