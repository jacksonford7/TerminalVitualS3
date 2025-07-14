<%@ Page  Title="Mantenimiento De Niveles de Aprobaciones"  Language="C#" MasterPageFile="~/site.Master" AutoEventWireup="true" CodeBehind="frm_nivel_aprobacion.aspx.cs" Inherits="CSLSite.frm_nivel_aprobacion" MaintainScrollPositionOnPostback="True" %>
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
    <input id="Nombre_Nivel" type="hidden" value="" runat="server" clientidmode="Static" />

     <input id="sel_g_id_group" type="hidden" value="" runat="server" clientidmode="Static" />
     <input id="sel_g_group_name" type="hidden" value="" runat="server" clientidmode="Static" />
     <input id="sel_g_estado" type="hidden" value="" runat="server" clientidmode="Static" />
     <input id="sel_g_create_user" type="hidden" value="" runat="server" clientidmode="Static" />
     <input id="sel_g_usuarios" type="hidden" value="" runat="server" clientidmode="Static" />


     <noscript><meta http-equiv="refresh" content="0; url=sinsoporte.htm" /> </noscript>
     <div>
        
         <i class="ico-titulo-2"></i><h1>Mantenimiento de Nivel de Aprobaciones</h1><br />
    </div>
     <div class=" msg-alerta">
    <span id="dtlo" runat="server">Estimado usuario:</span> 
    <br /> ...
    </div>
     <div class="seccion" id="BUSCAR">
      <div class="informativo">
      <table>
      <tr><td rowspan="2" class="inum"> <div class="number">1</div></td><td class="level1" >Nuevo Nivel</td></tr>
      <tr><td class="level2"></td></tr>
      </table>
     </div>
     <div class="colapser colapsa" ></div>
     <div class="accion">
     <table class="controles" cellspacing="0" cellpadding="1">
       
         <tr><td class="bt-bottom bt-right bt-left">ID NIVEL:</td>
         <td class="bt-bottom bt-right" colspan="3"><asp:TextBox ID="TxtID" runat="server" CssClass="inputText" ClientIDMode="Static" MaxLength="100" 
         Width="100px" ></asp:TextBox></td>  
         </tr>  
         <tr><td class="bt-bottom bt-right bt-left">NOMBRE DEL NIVEL:</td>
         <td class="bt-bottom bt-right" colspan="3"><asp:TextBox ID="TxtNombreNivel" runat="server" CssClass="inputText"  ClientIDMode="Static"
         Width="420px" MaxLength="150"></asp:TextBox></td>  
         </tr>  
         <tr><td class="bt-bottom bt-right bt-left">VALOR INICIAL:</td>
         <td class="bt-bottom bt-right" colspan="3"><asp:TextBox ID="TxtValorInicial" runat="server" Width="200px"  MaxLength="10"  ClientIDMode="Static"  
                onkeypress="return soloLetras(event,'1234567890.')" EnableViewState="False">00.00</asp:TextBox></td>  
         </tr> 
          <tr><td class="bt-bottom bt-right bt-left">VALOR FINAL:</td>
         <td class="bt-bottom bt-right" colspan="3"><asp:TextBox ID="TxtValorFinal" runat="server" Width="200px"  MaxLength="10"   ClientIDMode="Static" 
                onkeypress="return soloLetras(event,'1234567890.')" EnableViewState="False">00.00</asp:TextBox></td>  
         </tr> 
          <tr>
         <td class="bt-bottom  bt-right bt-left" >MOTIVO/CONCEPTO:</td>
         <td class="bt-bottom bt-right" colspan="3"> <asp:DropDownList ID="CboConcepto" runat="server" Width="250px" AutoPostBack="False"
                                            Height="28px"  DataTextField='description' DataValueField='id_concept'  
                                            Font-Size="Small" > </asp:DropDownList>     </td>       
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
             Grupos: <asp:TextBox ID="TxtUsuario" runat="server" CssClass="inputText"  ClientIDMode="Static"
         Width="100px"></asp:TextBox>

               Nivel: <%--<asp:TextBox ID="TxtCantidad" runat="server" CssClass="inputText" MaxLength="1" ClientIDMode="Static"
         Width="30px" onkeypress="return soloLetras(event,'1234567890',false)" ></asp:TextBox>--%>
                 <asp:DropDownList ID="CboCantidad" runat="server" Width="50px" AutoPostBack="False"
                                            Height="28px"  DataTextField='description' DataValueField='id_level'  
                                            Font-Size="Small" 
                                            >
                                        </asp:DropDownList>
               &nbsp;  
              <span clientidmode="Static" onclick="GolinkLevel();" runat="server" id="llave1" class="caja cajafull" style="cursor:pointer;color:black;width:100px;"  >Seleccionar</span>
            <asp:Button ID="BtnAgregar" runat="server"   OnClick="BtnAgregar_Click" Text="Agregar" Width="100px" ForeColor ="Blue"/>  
             &nbsp;<asp:Button ID="BtnNuevo" runat="server"   OnClick="BtnNuevo_Click" Text="Nuevo" Width="100px" ForeColor="Red"/>
              &nbsp;<asp:Button ID="BtnGrabar" runat="server"   OnClick="BtnGrabar_Click" Text="Grabar" Width="100px" />
               &nbsp;    </div>
         <div class="cataresult"  >
             <div class="booking"  >
                  <div class="separator">Detalle de Niveles Agregados</div>
                  
                 <asp:Repeater ID="tablePagination2" runat="server" onitemcommand="RemoverGrupos_ItemCommand">
                 <HeaderTemplate>
                 <table id="tablasort2" cellspacing="1" cellpadding="1" class="tabRepeat">
                 <tr>
                 <th>#</th>
                 <th>ID</th>
                 <th>GRUPO</th>
                 <th>USUARIOS</th>
                 <th>ESTADO</th>
                 <th>NIVEL</th>
                 <th>CREADO</th>
                 <th>ACCION</th>
                 </tr>
                 </HeaderTemplate>
                 <ItemTemplate>
                  <tr  >
                  <td><%#Eval("sequence")%></td>
                  <td><%#Eval("id_group")%></td>
                  <td><%#Eval("group_name")%></td>
                  <td><%#Eval("usuarios")%></td>
                  <td><%#Eval("estado")%></td>
                  <td><%#Eval("level")%></td>
                  <td><%#Eval("create_user")%></td>
                  <td> <asp:Button ID="BtnConfirmar"  
                       OnClientClick="return confirm('Está seguro de que desea remover el grupo?');" 
                       runat="server" Height="20px" Width="60px" Text="Remover" CssClass="Anular" ToolTip="Permite remover un grupo" CommandArgument='<%#Eval("id_group")%>' /></td>
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
      <tr><td rowspan="2" class="inum"> <div class="number">2</div></td><td class="level1" >DETALLE DE NIVELES</td></tr>
      <tr><td class="level2">
      Grupos que conformaran los niveles de aprobaciones de las notas de créditos.
      </td></tr>
      </table>
     </div>
    <div class="colapser colapsa"></div>
     <div class="accion">
         <div class="cataresult" >

          <asp:UpdatePanel ID="upresult" runat="server" >
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
                 <th>Motivo </th>
                 <th>Valor Ini</th>
                 <th>Valor Fin</th>
                 <th>Estado</th>
                 <th>Creado</th>
                 <th>Grupos</th>
                 <th>Acción</th>

                 </tr>
                 </thead> 
                 <tbody>
                 </HeaderTemplate>
                 <ItemTemplate>
                 <tr class="point" >
                  <td ><asp:Label Text='<%#Eval("id_level")%>' ID="lbl_id_level" runat="server"  /></td>
                  <td ><asp:Label Text='<%#Eval("level_name")%>' ID="lbl_level_name" runat="server" /></td>
                  <td ><asp:Label Text='<%#Eval("description")%>' ID="lbl_description" runat="server" /></td>
                  <td ><asp:Label Text='<%#Eval("init_value")%>' ID="lbl_init_value" runat="server" /></td>
                  <td ><asp:Label Text='<%#Eval("init_end")%>' ID="lbl_init_end" runat="server" /></td>
                  <td ><asp:Label Text='<%#Eval("estado")%>' ID="lbl_estado" runat="server" /></td>
                  <td ><asp:Label Text='<%#Eval("create_user")%>' ID="lbl_create_user" runat="server" /></td>
                  <td ><asp:Label Text='<%#Eval("grupos")%>' ID="lbl_grupos" runat="server" /></td>   
                  <td>
                      <asp:linkbutton  ID="BtnModificar"  runat="server" Text="Modifiar" CssClass="Anular" ToolTip="Permite modificar un nivel" CommandArgument='<%#Eval("id_level")%>' />  
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
    <script src="../Scripts/nota_credito.js" type="text/javascript"></script>
    <script src="../Scripts/pages.js" type="text/javascript"></script>
    <script type="text/javascript">

          $(window).load(function () {
                            $("div.colapser").toggle(function () { $(this).removeClass("colapsa").addClass("expande"); $(this).next().hide(); }
                                  , function () { $(this).removeClass("expande").addClass("colapsa"); $(this).next().show(); });
        });


        function GolinkLevel() {

            window.open('../nota_credito/lookup_grupos.aspx', 'name', 'width=850,height=480');

        }

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
       

        function popupCallback_Nivel(lookup_get_grupos) {
     
            if (lookup_get_grupos.sel_g_id_group != null) {
                this.document.getElementById("sel_g_id_group").value = lookup_get_grupos.sel_g_id_group;
                this.document.getElementById("sel_g_group_name").value = lookup_get_grupos.sel_g_group_name;
                this.document.getElementById("sel_g_estado").value = lookup_get_grupos.sel_g_estado;
                this.document.getElementById("sel_g_create_user").value = lookup_get_grupos.sel_g_create_user;
                this.document.getElementById("sel_g_usuarios").value = lookup_get_grupos.sel_g_usuarios;
                this.document.getElementById("Accion").value = "I";
                this.document.getElementById('<%= TxtUsuario.ClientID %>').value = lookup_get_grupos.sel_g_group_name;
              
                this.document.getElementById("llave1").style.color = 'black';
               
            }
            else {
                 this.document.getElementById("sel_g_id_group").value = "";
                 this.document.getElementById("sel_g_group_name").value = "";
                 this.document.getElementById("sel_g_estado").value = "";
                 this.document.getElementById("sel_g_create_user").value = "";
                 this.document.getElementById("Accion").value = "I";
                this.document.getElementById("sel_g_usuarios").value = "";
                 this.document.getElementById("llave1").style.color = 'black';
                this.document.getElementById('<%= TxtUsuario.ClientID %>').value = "";
          
            }
    
        }
   
    </script>

 
    
</asp:Content>