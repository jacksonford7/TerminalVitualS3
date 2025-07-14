<%@ Page Language="C#" Title="Subir Archivos - Imagenes Contenedor" MasterPageFile="~/site.Master"  AutoEventWireup="true" CodeBehind="stc_archivos.aspx.cs" Inherits="CSLSite.stc_archivos" %>

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
            width: 500px;
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

</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="placebody" runat="server" >
     
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></asp:ToolkitScriptManager>

     <input id="mrn" type="hidden" value="" runat="server" clientidmode="Static" />
     <input id="msn" type="hidden" value="" runat="server" clientidmode="Static" />
     <input id="hsn" type="hidden" value="" runat="server" clientidmode="Static" />
     <input id="unidad" type="hidden" value="" runat="server" clientidmode="Static" />
     <input id="gkey" type="hidden" value="" runat="server" clientidmode="Static" />
     <input id="id_importador" type="hidden" value="" runat="server" clientidmode="Static" />

     

    <div class="mt-4">         
        <nav class="mt-4" aria-label="breadcrumb">
          <ol class="breadcrumb">
            <li class="breadcrumb-item" id="opcion_principal" runat="server"><a href="#">STC</a></li>
            <li class="breadcrumb-item active" aria-current="page" id="sub_opcion" runat="server">Subir imagenes de contenedores</li>
          </ol>
        </nav>
    </div>

   <div class="dashboard-container p-4" id="cuerpo" runat="server">

  
    <div class="form-row">

            <div class="form-group col-md-3"> 
                <label for="inputZip">Contenedor:<span style="color: #FF0000; font-weight: bold;">*</span></label>
                <asp:TextBox ID="TxtContenedor" runat="server" MaxLength="20"   class="form-control"
                 onkeypress="return soloLetras(event,'abcdefghijklnmñopqrstuvwxyz01234567890',true)" Style="text-transform:uppercase"></asp:TextBox>
                <span id="valrazsocial" class="validacion"></span>
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
                 <div class="alert alert-danger" id="banmsg" runat="server" clientidmode="Static"><b>Error!</b> </div>
            </div>
         </div>

    <div class="row">
        <div class="form-group col-md-12">              
              <a class="level1"  runat="server" id="ls6" clientidmode="Static" >Detalle de imagenes a Cargar</a>
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
            <th>Imagen</th>
            <th>Escoja el archivo con formato indicado.</th>
            <th>Formato</th>
            </tr>
            </thead> 
            <tbody>
            </HeaderTemplate>
            <ItemTemplate>
            <tr class="point">
            <td class="nover"><asp:TextBox ID="txtiddocemp" runat="server" Text='<%#Eval("id")%>' /></td>
            <td style=" font-size:inherit"><%#Eval("nombre")%></td>
            <td >
                <asp:FileUpload extension='<%#Eval("extension")%>' class="btn btn-outline-primary mr-4" id="fsupload" title="Escoja el archivo con formato indicado en la siguiente columna."
                       onchange="validaextension(this)" style=" font-size:small" runat="server"/>
            </td>
            <td ><%#Eval("extension")%></td>
            </tr>
            </ItemTemplate>
            <FooterTemplate>
            </tbody>
            </table>
            </FooterTemplate>
         </asp:Repeater>
      </table>


     </div>

  <div class="form-row">
            <div class="form-group col-md-12">               
                <div class="cataresult" >

                 <%--   <asp:UpdatePanel ID="upresult" runat="server" ChildrenAsTriggers="true" >
                        <ContentTemplate>--%>
                           
                            <div><br /></div>
                            <div id="xfinder" runat="server" visible="false" >
                                <div ><br /></div>
                                 <div class="findresult" >
                                     <div class="booking" >
                                          
                                         <div class="form-group col-md-12"> 
                                            <div class="form-title">Detalle de Imagenes</div>
                                        </div>

                                         <asp:Repeater ID="tablePagination" runat="server" 
                                                  >
                                             <HeaderTemplate>
                                                 <table id="tablasort" cellspacing="1" cellpadding="1" class="table table-bordered invoice">
                                                     <thead>
                                                         <tr>
                                                             <th id="pase" ># Carga</th>
                                                             <th id="fecha" >Contenedor</th>
                                                             <th id="turno" >Usuario</th>
                                                             <th id="contenedor" >Fecha</th>
                                                             <th id="archivo">Archivo</th>

                                                         </tr>
                                                     </thead> 
                                                 <tbody>
                                             </HeaderTemplate>
                                             <ItemTemplate>
                                                 <tr class="point" >
                                                      <td ><%#Eval("carga")%> </td>
                                                      <td ><%#Eval("contenedor")%> </td>
                                                      <td ><%#Eval("usuario")%> </td>
                                                      <td ><%#Eval("fecha")%></td>
                                                      <td  > <%#Eval("imagen")%></td>
                                                    <%-- <td  > <a href='<%#Eval("imagen") %>'  class="topopup" target="_blank">
                                                    <i class="fa fa-search"></i> Ver Imagen </a></td>--%>
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
                            
                     <%-- </ContentTemplate>
                         <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="BtnBuscar" />
                           
                         </Triggers>
                    </asp:UpdatePanel>--%>
                </div>
            </div>
        </div>


   
    <div class="col-md-12 d-flex justify-content-center">

               <asp:Button ID="btsalvar" runat="server" Text="Grabar Información"  
                   ToolTip="Confirma la información de imagenes a cargar" CssClass="btn btn-primary" OnClientClick="return prepareObject('¿Esta seguro de subir las imagenes?')" OnClick="btsalvar_Click"/>
                <img alt="loading.." src="../lib/file-uploader/img/loading.gif" height="32px" width="32px"  id="ImgCargaDet" class="nover"   />

     </div>

 

  </div>

   


   

    <script src="../Scripts/pages.js" type="text/javascript"></script>
    <script src="../Scripts/credenciales.js" type="text/javascript"></script>

  

    <script type="text/javascript">
        var registroempresa = {};
        var lista = [];
        var cblarray = [];
        var carray = [];
        function prepareObject(valor) {
            try {

                if (confirm(valor) == false) {
                    return false;
                }

                mostrarloader('2');

                var vals = document.getElementById('<%=TxtContenedor.ClientID %>').value;
                if (vals == '' || vals == null || vals == undefined) {
                    alertify.alert('* Debe ingresar el contenedor *').set('label', 'Aceptar');
                    document.getElementById('<%=TxtContenedor.ClientID %>').focus();
                    document.getElementById("ImgCargaDet").className = 'nover';
                    return false;
                }
                
                var vals = document.getElementById('<%=id_importador.ClientID %>').value;
                if (vals == '' || vals == null || vals == undefined) {
                    alertify.alert('* No existen datos del importador con el con el número de contenedor ingresado *').set('label', 'Aceptar');
                    document.getElementById('<%=TxtContenedor.ClientID %>').focus();
                    document.getElementById("ImgCargaDet").className = 'nover';
                    return false;
                }

                var vals = document.getElementById('<%=unidad.ClientID %>').value;
                if (vals == '' || vals == null || vals == undefined) {
                    alertify.alert('* Debe ingresar el contenedor *').set('label', 'Aceptar');
                    document.getElementById('<%=TxtContenedor.ClientID %>').focus();
                    document.getElementById("ImgCargaDet").className = 'nover';
                    return false;
                }

                var vals = document.getElementById('<%=mrn.ClientID %>').value;
                if (vals == '' || vals == null || vals == undefined) {
                    alertify.alert('* Debe ingresar el contenedor o generar la consulta (MRN) Vacío *').set('label', 'Aceptar');
                    document.getElementById('<%=TxtContenedor.ClientID %>').focus();
                    document.getElementById("ImgCargaDet").className = 'nover';
                    return false;
                }

                var vals = document.getElementById('<%=msn.ClientID %>').value;
                if (vals == '' || vals == null || vals == undefined) {
                    alertify.alert('* Debe ingresar el contenedor o generar la consulta (MSN) Vacío *').set('label', 'Aceptar');
                    document.getElementById('<%=TxtContenedor.ClientID %>').focus();
                    document.getElementById("ImgCargaDet").className = 'nover';
                    return false;
                }

                var vals = document.getElementById('<%=hsn.ClientID %>').value;
                if (vals == '' || vals == null || vals == undefined) {
                    alertify.alert('* Debe ingresar el contenedor o generar la consulta (HSN) Vacío *').set('label', 'Aceptar');
                    document.getElementById('<%=TxtContenedor.ClientID %>').focus();
                    document.getElementById("ImgCargaDet").className = 'nover';
                    return false;
                }

                 var vals = document.getElementById('<%=gkey.ClientID %>').value;
                if (vals == '0' || vals == null || vals == undefined) {
                    alertify.alert('* Debe ingresar el contenedor o generar la consulta (GKEY) Vacío *').set('label', 'Aceptar');
                    document.getElementById('<%=TxtContenedor.ClientID %>').focus();
                    document.getElementById("ImgCargaDet").className = 'nover';
                    return false;
                }

                
                lista = [];
                var vals = document.getElementById('tablar');
                if (vals == '' || vals == null || vals == undefined) {
                    alertify.alert('debe registrar las imagenes a cargar').set('label', 'Aceptar');
                    
                    document.getElementById("ImgCargaDet").className = 'nover';
                    return false;
                }
                if (vals != null) {
                    var tbl = document.getElementById('tablar');
                    if (tbl.rows.length == 1) {
                        alertify.alert('Debe subir 4 imagenes').set('label', 'Aceptar');
                        document.getElementById("ImgCargaDet").className = 'nover';
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
                            alertify.alert('Debe cargar las 4 imagenes requeridas').set('label', 'Aceptar');
                            document.getElementById("ImgCargaDet").className = 'nover';
                            return false;
                        }
                        if (nomdoc == lista[n].documento) {
                            alertify.alert('Existen imagenes repetidas..Debe volver a cargar una imagen').set('label', 'Aceptar');
                            document.getElementById("ImgCargaDet").className = 'nover';
                            return false;
                        }
                        nomdoc = lista[n].documento;
                    }
                }
                
                return true;
            } catch (e) {
                alertify.alert('Lamentamos comunicarle que ha ocurrido el siguiente problema:\n' + e.Message).set('label', 'Aceptar');
            }
        }


        
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

      
       
        

        function getGif() {
            document.getElementById('imagen').innerHTML = '<img alt="" src="../shared/imgs/loader.gif"/>';
            return true;
        }
        function getGifOculta() {
            document.getElementById('imagen').innerHTML = '<img alt="" src=""/>';
            return true;
        }

        
    </script>



</asp:Content>
