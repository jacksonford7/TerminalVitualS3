<%@ Page  Title="Mantenimiento De Usuarios"  Language="C#" MasterPageFile="~/site.Master" AutoEventWireup="true" CodeBehind="frm_usuarios.aspx.cs" Inherits="CSLSite.frm_usuarios" MaintainScrollPositionOnPostback="True" %>
<%@ MasterType VirtualPath="~/site.Master" %>


<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="placehead" runat="server">
    <link href="../shared/estilo/aisv-general.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/work.css" rel="stylesheet" type="text/css" />
     <link href="../shared/estilo/tables.css" rel="stylesheet" type="text/css" />
     <link href="../shared/estilo/catalogos.css" rel="stylesheet" type="text/css" />
     <link href="../shared/estilo/info.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/pages.js" type="text/javascript"></script>


     <script type="text/javascript">
            function BindFunctions() {
                $(document).ready(function () {
                    document.getElementById('imagen').innerHTML = '';
                    $('#tablasort').tablesorter({ cancelSelection: true, cssAsc: "ascendente", cssDesc: "descendente", headers: { 8: { sorter: false }, 9: { sorter: false}} }).tablesorterPager({ container: $('#pager'), positionFixed: false, pagesize: 10 });
                });
            }
    </script>

    </asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="placebody" runat="server" >
      <asp:ToolkitScriptManager ID="tkcal" EnableScriptGlobalization="true" runat="server"> </asp:ToolkitScriptManager>
    <input id="zonaid" type="hidden" value="205" />
    <input id="IdUsuario" type="hidden" value="" runat="server" clientidmode="Static" />
     <input id="Usuario" type="hidden" value="" runat="server" clientidmode="Static" />
    <input id="Nombres" type="hidden" value="" runat="server" clientidmode="Static" />
    <input id="Nombre" type="hidden" value="" runat="server" clientidmode="Static" />
    <input id="Apellido" type="hidden" value="" runat="server" clientidmode="Static" />
    <input id="Accion" type="hidden" value="" runat="server" clientidmode="Static" />

     <noscript><meta http-equiv="refresh" content="0; url=sinsoporte.htm" /> </noscript>
     <div>
        
         <i class="ico-titulo-2"></i><h1>Mantenimiento de Usuarios</h1><br />
    </div>
     <div class=" msg-alerta">
    <span id="dtlo" runat="server">Estimado usuario:</span> 
    <br /> ...
    </div>
     <div class="seccion" id="BUSCAR">
      <div class="informativo">
      <table>
      <tr><td rowspan="2" class="inum"> <div class="number">1</div></td><td class="level1" >Datos del Usuario</td></tr>
      <tr><td class="level2"></td></tr>
      </table>
     </div>
     <div class="colapser colapsa" ></div>
     <div class="accion">
     <table class="controles" cellspacing="0" cellpadding="1">
        <tr><th class="bt-bottom bt-right bt-top bt-left" colspan="4"></th></tr>
         <tr><td class="bt-bottom bt-right">ID USUARIO:</td>
         <td class="bt-bottom bt-right" colspan="3"><asp:TextBox ID="TxtID" runat="server" CssClass="inputText" ClientIDMode="Static" MaxLength="100" 
         Width="100px" ></asp:TextBox></td>  
         </tr>  
         <tr><td class="bt-bottom bt-right">USUARIO:</td>
         <td class="bt-bottom bt-right" colspan="3"><asp:TextBox ID="TxtUsuario" runat="server" CssClass="inputText"  ClientIDMode="Static"
         Width="200px"></asp:TextBox></td>  
         </tr>  
            <tr><td class="bt-bottom bt-right">NOMBRES:</td>
         <td class="bt-bottom bt-right" colspan="3"> <asp:TextBox ID="TxtNombres" runat="server" CssClass="inputText"  ClientIDMode="Static"
         Width="250px" ></asp:TextBox></td>  
         </tr> 
         <tr><td class="bt-bottom bt-right">APELLIDOS:</td>
         <td class="bt-bottom bt-right" colspan="3">  <asp:TextBox ID="TxtApellidos" runat="server" CssClass="inputText"  ClientIDMode="Static"
         Width="250px"  ></asp:TextBox></td>  
         </tr>  
        <tr><td class="bt-bottom  bt-right bt-left">EMAIL:</td>
         <td class="bt-bottom bt-right" colspan="3"> 
              <asp:TextBox ID="TxtDescripcion" runat="server" CssClass="inputText" MaxLength="100" ClientIDMode="Static"
         Width="410px"  placeholder="Email"></asp:TextBox>
          
         </td>  
        </tr>  

         <tr>
         <td class="bt-bottom  bt-right bt-left" >ESTADO:</td>
         <td class="bt-bottom bt-right" colspan="3"> <asp:DropDownList ID="CboEstado" runat="server" Width="150px" AutoPostBack="False"
                                            Height="28px"  DataTextField='name' DataValueField='id_depot'  
                                            Font-Size="Small" > </asp:DropDownList>     </td>       
         </tr>
          <tr>
         <td class="bt-bottom  bt-right bt-left" ></td>
         <td class="bt-bottom bt-right" colspan="3"> 
             <span clientidmode="Static" onclick="Golink();" runat="server" id="llave1" class="caja cajafull" style="cursor:pointer;color:black" >Click aqui para Agregar Usuarios</span>
         </td>       
         </tr>
         <tr>
         <td class="bt-bottom  bt-right bt-left" ></td>
         <td class="bt-bottom bt-right" colspan="3"> 
             <span class="validacion" id="xplinea" > </span>
                    <span id="imagen"></span>
         <asp:Button ID="BtnAgregar" runat="server"   OnClick="BtnGrabar_Click" Text="Grabar" Width="100px"/>
         &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
         <asp:Button ID="BtnNuevo" runat="server"   OnClick="BtnNuevo_Click" Text="Nuevo" Width="100px"/>
         &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
         </td>       
         </tr>

        <%-- </tr>--%>



     </table>
     </div>
    </div>
     <div class="seccion">
      <div class="informativo">
      <table>
      <tr><td rowspan="2" class="inum"> <div class="number">2</div></td><td class="level1" >DETALLE DE USUARIOS</td></tr>
      <tr><td class="level2">
      Usuarios que conformaran los grupos para las aprobaciones de notas de créditos.
      </td></tr>
      </table>
     </div>
    <div class="colapser colapsa"></div>
      <div class="accion">
         <div class="cataresult" >
            <asp:UpdatePanel ID="upresult" runat="server" ChildrenAsTriggers="true">
                <ContentTemplate>
                  <script type="text/javascript"> Sys.Application.add_load(BindFunctions);  </script>
              <div id="xfinder" runat="server" visible="false" >
               
                <div class="findresult" >
              <div class="booking" >
               <div class="separator">USUARIOS</div>
              <div class="bokindetalle">
                         <asp:Repeater ID="TableUsuarios" runat="server"  onitemcommand="Seleccionar_ItemCommand" OnItemDataBound="Opciones_ItemDataBound">
                <HeaderTemplate>
                <table id="tablasort" cellspacing="0" cellpadding="1" class="tabRepeat">
                <thead>
                <tr>
                    <th style='width:10px'>ID</th>
                    <th style='width:50px'>USUARIO</th>
                    <th style='width:150px'>NOMBRES</th>
                    <th style='width:150px'>EMAIL</th>
                    <th style='width:50px'>ESTADO</th>
                    <th >ACTUALIZAR</th>
                  </tr>
                </thead> 
                <tbody>
                </HeaderTemplate>
                <ItemTemplate>
                <tr class="point">
                <td ><asp:Label Text='<%#Eval("IdUsuario")%>' ID="lbl_id_usuario" runat="server"  /></td>
                <td ><asp:Label Text='<%#Eval("Usuario")%>' ID="lbl_usuario" runat="server" /></td>
                <td ><asp:Label Text='<%#Eval("Nombres")%>' ID="lbl_nombres" runat="server" /></td>
                <td ><asp:Label Text='<%#Eval("email")%>' ID="lbl_email" runat="server" /></td>
                <td ><asp:Label Text='<%#Eval("estado")%>' ID="lbl_estado" runat="server" /></td>   
                <td class="alinear" style=" width:50px">
                    <asp:linkbutton  ID="BtnConfirmar"  runat="server" Text="Seleccionar" CssClass="Anular" ToolTip="Permite modificar un concepto" CommandArgument='<%#Eval("IdUsuario")%>' />    
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
              <div id="pager">
                         Registros por página
                      <select class="pagesize">
                      <option selected="selected" value="10">10</option>
                      <option value="20">20</option>
                      </select>
                     <img alt="" src="../shared/imgs/first.gif" class="first"/>
                     <img alt="" src="../shared/imgs/prev.gif" class="prev"/>
                     <input  type="text" class="pagedisplay" size="5px"/>
                     <img alt="" src="../shared/imgs/next.gif" class="next"/>
                     <img alt="" src="../shared/imgs/last.gif" class="last"/>
                 </div>
              </div>
                    </ContentTemplate>
                   <%--  <Triggers>
                     <asp:AsyncPostBackTrigger ControlID="BtnGrabar" />
                     </Triggers>--%>
           </asp:UpdatePanel>

         </div>
      </div>


    </div>
     <div class="seccion">
      <div class="informativo">
      <table>
     
      
      </table>
     </div>

     <div class="accion" id="ADU">
  

 
      <div class="botonera">
         <img alt="loading.." src="../shared/imgs/loader.gif" id="loader" class="nover"  />
          &nbsp;
          &nbsp;

     </div>
    
     </div>
    </div>

    <script src="../Scripts/jquery-1.7.1.min.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.tablesorter.min.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.tablesorter.pager.js" type="text/javascript"></script>

    <script src="../Scripts/pages.js" type="text/javascript"></script>

    <script type="text/javascript">
    
        //$(window).load(function () {
        //    //objeto a transportar.
        //    $(document).ready(function () {
        //        //inicia los fecha-hora
        //        $('.datetimepicker').datetimepicker().datetimepicker({ lang: 'es', step: 30, format: 'd/m/Y H:i' });
        //        //inicia los fecha
        //        $('.datetimepickerAlt').datetimepicker().datetimepicker({ lang: 'es', step: 30, format: 'd/m/Y' });

        //        $('.datepicker').datetimepicker().datetimepicker({ lang: 'es', timepicker: false, format: 'd/m/Y', closeOnDateSelect: true, minDate: '0' });
        //        //init reefer-> lo pone a false.

        //    });
        //});

        function popupCallback2(lookup_usuario) {
     
            if (lookup_usuario.sel_IdUsuario != null) {
                this.document.getElementById("IdUsuario").value = lookup_usuario.sel_IdUsuario;
                this.document.getElementById("Usuario").value = lookup_usuario.sel_Usuario;
                this.document.getElementById("Nombres").value = lookup_usuario.sel_Nombre + " " + lookup_usuario.sel_Apellido;
                this.document.getElementById("Nombre").value = lookup_usuario.sel_Nombre;
                this.document.getElementById("Apellido").value = lookup_usuario.sel_Apellido;
                this.document.getElementById("llave1").textContent = lookup_usuario.sel_Nombre + " " + lookup_usuario.sel_Apellido;
                this.document.getElementById("Accion").value = "I";

                this.document.getElementById('<%= TxtDescripcion.ClientID %>').value = lookup_usuario.sel_email;
                this.document.getElementById('<%= TxtID.ClientID %>').value = lookup_usuario.sel_IdUsuario;
                this.document.getElementById('<%= TxtUsuario.ClientID %>').value = lookup_usuario.sel_Usuario;
                this.document.getElementById('<%= TxtNombres.ClientID %>').value = lookup_usuario.sel_Nombre;
                this.document.getElementById('<%= TxtApellidos.ClientID %>').value = lookup_usuario.sel_Apellido;

               this.document.getElementById("llave1").style.color = 'black';
               
            }
            else {
                this.document.getElementById("IdUsuario").value = "0";
            this.document.getElementById("Usuario").value = "";
            this.document.getElementById("Nombres").value = "";
            this.document.getElementById("Nombre").value = "";
            this.document.getElementById("Apellido").value = "";
            this.document.getElementById("llave1").textContent =  "";
            this.document.getElementById("Accion").value = "I";

            this.document.getElementById('<%= TxtDescripcion.ClientID %>').value = "";
            this.document.getElementById('<%= TxtID.ClientID %>').value = "";
            this.document.getElementById('<%= TxtUsuario.ClientID %>').value = "";
            this.document.getElementById('<%= TxtNombres.ClientID %>').value = "";
            this.document.getElementById('<%= TxtApellidos.ClientID %>').value = "";
            }
    
        }
   
    </script>

    <script type="text/javascript">

        $(window).load(function () {
                            $("div.colapser").toggle(function () { $(this).removeClass("colapsa").addClass("expande"); $(this).next().hide(); }
                                  , function () { $(this).removeClass("expande").addClass("colapsa"); $(this).next().show(); });
        });


        function cancelEmpty() {
            var txt = document.getElementById("TxtReferencia");
            if (txt != null && txt != undefined) {
                if (txt.value) {
                    return true;
                }
            }
            //detenga el post
            alert("Por favor escriba la referencia");
            return false;
        }


        function defaultDate(control) {
            if (control.value) {
                var cj = document.getElementById("TxtFechaDesde");
                if (cj != null && cj != undefined) {
                    cj.value = control.value;
                }
            }
        }
    </script>

    <script src="../Scripts/nota_credito.js" type="text/javascript"></script>
</asp:Content>