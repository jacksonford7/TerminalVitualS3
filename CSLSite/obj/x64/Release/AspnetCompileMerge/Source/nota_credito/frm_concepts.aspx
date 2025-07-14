<%@ Page  Title="Mantenimiento De Conceptos"  Language="C#" MasterPageFile="~/site.Master" AutoEventWireup="true" CodeBehind="frm_concepts.aspx.cs" Inherits="CSLSite.frm_concepts" MaintainScrollPositionOnPostback="True" %>
<%@ MasterType VirtualPath="~/site.Master" %>


<asp:Content ID="Content1" ContentPlaceHolderID="placehead" runat="server">
    <link href="../shared/estilo/aisv-general.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/work.css" rel="stylesheet" type="text/css" />
     <link href="../shared/estilo/tables.css" rel="stylesheet" type="text/css" />
     <link href="../shared/estilo/catalogos.css" rel="stylesheet" type="text/css" />
     <link href="../shared/estilo/info.css" rel="stylesheet" type="text/css" />

    </asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="placebody" runat="server" >
    <input id="zonaid" type="hidden" value="205" />
     <noscript><meta http-equiv="refresh" content="0; url=sinsoporte.htm" /> </noscript>
     <div>
        
         <i class="ico-titulo-2"></i><h1>Mantenimiento de Conceptos de Cartera</h1><br />
    </div>
     <div class=" msg-alerta">
    <span id="dtlo" runat="server">Estimado usuario:</span> 
    <br /> ...
    </div>
     <div class="seccion" id="BUSCAR">
      <div class="informativo">
      <table>
      <tr><td rowspan="2" class="inum"> <div class="number">1</div></td><td class="level1" >Datos del Concepto</td></tr>
      <tr><td class="level2"></td></tr>
      </table>
     </div>
     <div class="colapser colapsa" ></div>
     <div class="accion">
     <table class="controles" cellspacing="0" cellpadding="1">
        <tr><th class="bt-bottom bt-right bt-top bt-left" colspan="4"></th></tr>
         <tr><td class="bt-bottom bt-right">ID:</td>
         <td class="bt-bottom bt-right" colspan="3"> <asp:Label ID="LblID" runat="server" Text="LblID" Width="200px"></asp:Label>
            
         </td>  
         </tr>  
        <tr><td class="bt-bottom  bt-right bt-left">DESCRIPCION:</td>
         <td class="bt-bottom bt-right" colspan="3"> 
              <asp:TextBox ID="TxtDescripcion" runat="server" CssClass="inputText" MaxLength="100"  ClientIDMode="Static"
         Width="338px" onkeypress="return soloLetras(event,'abcdefghijklmnopqrstuvwxyz1234567890',true)" placeholder="Descripción"></asp:TextBox>
          
         </td>  
        </tr>  

         <tr>
         <td class="bt-bottom  bt-right bt-left" >ESTADO:</td>
         <td class="bt-bottom bt-right" colspan="3"> <asp:DropDownList ID="CboEstado" runat="server" Width="150px" AutoPostBack="False"
                                            Height="28px"  DataTextField='name' DataValueField='id_depot'  
                                            Font-Size="Small" 
                                            >
                                        </asp:DropDownList>
     
         </td>       
         </tr>
         <tr>
         <td class="bt-bottom  bt-right bt-left" ></td>
         <td class="bt-bottom bt-right" colspan="3"> 
             <span class="validacion" id="xplinea" > </span>
         <asp:Button ID="BtnAgregar" runat="server"   OnClick="BtnGrabar_Click" Text="Grabar" Width="100px"/>
         &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
         <asp:Button ID="BtnNuevo" runat="server"   OnClick="BtnNuevo_Click" Text="Nuevo" Width="100px"/>
         </td>       
         </tr>

        <%-- </tr>--%>



     </table>
     </div>
    </div>
     <div class="seccion">
      <div class="informativo">
      <table>
      <tr><td rowspan="2" class="inum"> <div class="number">2</div></td><td class="level1" >DETALLE DE CONCEPTOS</td></tr>
      <tr><td class="level2">
      
      </td></tr>
      </table>
     </div>
    <div class="colapser colapsa"></div>
      <div class="accion">
     <table class="controles" cellspacing="0" cellpadding="1">
        <tr><th class="bt-bottom bt-right bt-top bt-left" colspan="2"> </th></tr>
        

          <tr><th colspan="2">
                <div class="findresult" >
              <div class="booking" >
               <div class="separator">CONCEPTOS</div>
              <div class="bokindetalle">

          <asp:Repeater ID="Tableconceptos" runat="server"  onitemcommand="Seleccionar_ItemCommand" OnItemDataBound="Opciones_ItemDataBound">
                <HeaderTemplate>
                <table id="tablasort" cellspacing="0" cellpadding="1" class="tabRepeat">
                <thead>
                <tr>
                    <th style='width:50px'>ID</th>
                    <th style='width:250px'>DESCRIPCION</th>
                    <th style='width:50px'>CREADO</th>
                    <th style='width:50px'>MODIFICADO</th>
                    <th style='width:50px'>ESTADO</th>
                    <th >ACTUALIZAR</th>
                  </tr>
                </thead> 
                <tbody>
                </HeaderTemplate>
                <ItemTemplate>
                <tr class="point">
                <td ><asp:Label Text='<%#Eval("id_concept")%>' ID="lbl_id_concept" runat="server"  /></td>
                <td ><asp:Label Text='<%#Eval("description")%>' ID="lbl_description" runat="server" /></td>
                <td ><asp:Label Text='<%#Eval("create_user")%>' ID="lbl_usuario_crea" runat="server" /></td>
                <td ><asp:Label Text='<%#Eval("mod_user")%>' ID="lbl_usuario_mod" runat="server" /></td>
                <td ><asp:Label Text='<%#Eval("estado")%>' ID="lbl_estado" runat="server" /></td>   
                <td class="alinear" style=" width:50px">
                    <asp:linkbutton  ID="BtnConfirmar"  runat="server" Text="Seleccionar" CssClass="Anular" ToolTip="Permite modificar un concepto" CommandArgument='<%#Eval("id_concept")%>' />    
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
            </th>
          </tr>


     </table>
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


    <script src="../Scripts/pages.js" type="text/javascript"></script>
   <script src="../Scripts/opc_control.js" type="text/javascript"></script>

    <script type="text/javascript">
    
        $(window).load(function () {
            //objeto a transportar.
            $(document).ready(function () {
                //inicia los fecha-hora
                $('.datetimepicker').datetimepicker().datetimepicker({ lang: 'es', step: 30, format: 'd/m/Y H:i' });
                //inicia los fecha
                $('.datetimepickerAlt').datetimepicker().datetimepicker({ lang: 'es', step: 30, format: 'd/m/Y' });

                $('.datepicker').datetimepicker().datetimepicker({ lang: 'es', timepicker: false, format: 'd/m/Y', closeOnDateSelect: true, minDate: '0' });
                //init reefer-> lo pone a false.

            });
        });
   
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

</asp:Content>