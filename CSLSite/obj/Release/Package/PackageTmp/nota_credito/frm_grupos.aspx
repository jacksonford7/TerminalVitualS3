<%@ Page  Title="Mantenimiento De Grupos Usuarios"  Language="C#" MasterPageFile="~/site.Master" AutoEventWireup="true" CodeBehind="frm_grupos.aspx.cs" Inherits="CSLSite.frm_grupos" MaintainScrollPositionOnPostback="True" %>
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
    <input id="Nombre" type="hidden" value="" runat="server" clientidmode="Static" />
    <input id="Apellido" type="hidden" value="" runat="server" clientidmode="Static" />
    <input id="Email" type="hidden" value="" runat="server" clientidmode="Static" />
    <input id="Accion" type="hidden" value="" runat="server" clientidmode="Static" />
    <input id="Nombre_Grupo" type="hidden" value="" runat="server" clientidmode="Static" />

     <noscript><meta http-equiv="refresh" content="0; url=sinsoporte.htm" /> </noscript>
     <div>
        
         <i class="ico-titulo-2"></i><h1>Mantenimiento de Grupos Usuarios</h1><br />
    </div>
     <div class=" msg-alerta">
    <span id="dtlo" runat="server">Estimado usuario:</span> 
    <br /> ...
    </div>
     <div class="seccion" id="BUSCAR">
      <div class="informativo">
      <table>
      <tr><td rowspan="2" class="inum"> <div class="number">1</div></td><td class="level1" >Nuevo Grupo</td></tr>
      <tr><td class="level2"></td></tr>
      </table>
     </div>
     <div class="colapser colapsa" ></div>
     <div class="accion">
     <table class="controles" cellspacing="0" cellpadding="1">
       
         <tr><td class="bt-bottom bt-right">ID GRUPO:</td>
         <td class="bt-bottom bt-right" colspan="3"><asp:TextBox ID="TxtID" runat="server" CssClass="inputText" ClientIDMode="Static" MaxLength="100" 
         Width="100px" ></asp:TextBox></td>  
         </tr>  
         <tr><td class="bt-bottom bt-right">NOMBRE DEL GRUPO:</td>
         <td class="bt-bottom bt-right" colspan="3"><asp:TextBox ID="TxtNombreGrupo" runat="server" CssClass="inputText"  ClientIDMode="Static"
         Width="200px"></asp:TextBox></td>  
         </tr>  

         <tr>
         <td class="bt-bottom  bt-right bt-left" >ESTADO:</td>
         <td class="bt-bottom bt-right" colspan="3"> <asp:DropDownList ID="CboEstado" runat="server" Width="150px" AutoPostBack="False"
                                            Height="28px"  DataTextField='name' DataValueField='id_depot'  
                                            Font-Size="Small" > </asp:DropDownList>     </td>       
         </tr>
         <tr><th  colspan="4">
            
         </th></tr>
         
        

     </table>
          <div class="botonera">
               <span id="imagen"></span>
             Usuario: <asp:TextBox ID="TxtUsuario" runat="server" CssClass="inputText"  ClientIDMode="Static"
         Width="150px"></asp:TextBox>
            &nbsp;  <span clientidmode="Static" onclick="GolinkGroup();" runat="server" id="llave1" class="caja cajafull" style="cursor:pointer;color:black;width:100px;"  >Seleccionar</span>
            <asp:Button ID="BtnAgregar" runat="server"   OnClick="BtnAgregar_Click" Text="Agregar" Width="100px" ForeColor ="Blue"/>  
             &nbsp;<asp:Button ID="BtnNuevo" runat="server"   OnClick="BtnNuevo_Click" Text="Nuevo" Width="100px" ForeColor="Red"/>
              &nbsp;<asp:Button ID="BtnGrabar" runat="server"   OnClick="BtnGrabar_Click" Text="Grabar" Width="100px" />
               &nbsp;    </div>
         <div class="cataresult"  >
             <div class="booking"  >
                  <div class="separator">Detalle de Usuarios Agregados</div>
                  
                 <asp:Repeater ID="tablePagination2" runat="server" onitemcommand="RemoverUser_ItemCommand">
                 <HeaderTemplate>
                 <table id="tablasort2" cellspacing="1" cellpadding="1" class="tabRepeat">
                 <tr>
                 <th>#</th>
                 <th>ID</th>
                 <th>USUARIO</th>
                 <th>NOMBRE</th>
                 <th>APELLIDO</th>
                 <th>EMAIL</th>
                 <th>ESTADO</th>
                 <th>ACCION</th>
                 </tr>
                 </HeaderTemplate>
                 <ItemTemplate>
                  <tr  >
                  <td><%#Eval("sequence")%></td>
                  <td><%#Eval("IdUsuario")%></td>
                  <td><%#Eval("Usuario")%></td>
                  <td><%#Eval("Nombre")%></td>
                  <td><%#Eval("Apellido")%></td>
                  <td><%#Eval("Email")%></td>
                  <td><%#Eval("Estado")%></td>
                  <td> <asp:Button ID="BtnConfirmar"  
                       OnClientClick="return confirm('Está seguro de que desea remover el usuario?');" 
                       runat="server" Height="20px" Width="60px" Text="Remover" CssClass="Anular" ToolTip="Permite remover un usuario" CommandArgument='<%#Eval("IdUsuario")%>' /></td>
                 </tr>
                  </ItemTemplate>
                 <FooterTemplate>
                 </table>
                 </FooterTemplate>
         </asp:Repeater>
         
             </div>
     

       </div>
     </div>
    </div>
     <div class="seccion">
      <div class="informativo">
      <table>
      <tr><td rowspan="2" class="inum"> <div class="number">2</div></td><td class="level1" >DETALLE DE GRUPOS</td></tr>
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
                        <script type="text/javascript">
                            Sys.Application.add_load(BindFunctions); 
                      </script>
             <div id="xfinder" runat="server" visible="false" >

            <div class="findresult" >
              <div class="booking" >
                    <div class="separator">DETALLE DE GRUPOS</div>
                    <div class="bokindetalle">

                <asp:Repeater ID="tablePagination" runat="server" onitemcommand="Seleccionar_ItemCommand" OnItemDataBound="Opciones_ItemDataBound">
                 <HeaderTemplate>
                 <table id="tablasort" cellspacing="1" cellpadding="1" class="tabRepeat">
                 <thead>
                 <tr>
 
                 <th>Id</th>
                 <th>Nombre Grupo </th>
                 <th>Estado</th>
                 <th>Usuario Crea</th>
                 <th>Usuarios</th>
                 <th>Acción</th>

                 </tr>
                 </thead> 
                 <tbody>
                 </HeaderTemplate>
                 <ItemTemplate>
                 <tr class="point" >
                  <td ><asp:Label Text='<%#Eval("id_group")%>' ID="lbl_id_group" runat="server"  /></td>
                  <td ><asp:Label Text='<%#Eval("group_name")%>' ID="lbl_group_name" runat="server" /></td>
                  <td ><asp:Label Text='<%#Eval("estado")%>' ID="lbl_estado" runat="server" /></td>
                  <td ><asp:Label Text='<%#Eval("create_user")%>' ID="lbl_create_user" runat="server" /></td>
                  <td ><asp:Label Text='<%#Eval("usuarios")%>' ID="lbl_usuarios" runat="server" /></td>   
                  <td>
                      <asp:linkbutton  ID="BtnModificar"  runat="server" Text="Modifiar" CssClass="Anular" ToolTip="Permite modificar un grupo" CommandArgument='<%#Eval("id_group")%>' />  
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
       

        function popupCallback_Group(lookup_usuario) {
     
            if (lookup_usuario.sel_IdUsuario != null) {
                this.document.getElementById("IdUsuario").value = lookup_usuario.sel_IdUsuario;
                this.document.getElementById("Usuario").value = lookup_usuario.sel_Usuario;
                this.document.getElementById("Nombre").value = lookup_usuario.sel_Nombre;
                this.document.getElementById("Apellido").value = lookup_usuario.sel_Apellido;
                this.document.getElementById("Email").value = lookup_usuario.sel_email;
                this.document.getElementById("Accion").value = "I";
                this.document.getElementById('<%= TxtUsuario.ClientID %>').value = lookup_usuario.sel_Usuario;
              
                this.document.getElementById("llave1").style.color = 'black';
               
            }
            else {
                 this.document.getElementById("IdUsuario").value = "0";
                 this.document.getElementById("Usuario").value = "";
                 this.document.getElementById("Nombre").value = "";
                 this.document.getElementById("Apellido").value = "";
                 this.document.getElementById("Accion").value = "I";
                this.document.getElementById("Email").value = "";
                 this.document.getElementById("llave1").style.color = 'black';
                this.document.getElementById('<%= TxtUsuario.ClientID %>').value = "";
          
            }
    
        }
   
    </script>

 
    <script src="../Scripts/nota_credito.js" type="text/javascript"></script>
</asp:Content>