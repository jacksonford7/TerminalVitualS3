<%@ Page  Title="Transacción Nota de Crédito"  Language="C#" MasterPageFile="~/site.Master" AutoEventWireup="true" CodeBehind="frm_nota_credito.aspx.cs" Inherits="CSLSite.frm_nota_credito" MaintainScrollPositionOnPostback="True" %>
<%@ MasterType VirtualPath="~/site.Master" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="placehead" runat="server">

    <link href="../shared/estilo/aisv-general.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/work.css" rel="stylesheet" type="text/css" />
     <link href="../shared/estilo/tables.css" rel="stylesheet" type="text/css" />
     <link href="../shared/estilo/catalogos.css" rel="stylesheet" type="text/css" />
     <link href="../shared/estilo/info.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript">
            function BindFunctions() {
                $(document).ready(function () {
                    document.getElementById('imagen').innerHTML = '';
                    //$('#tablasort').tablesorter({ cancelSelection: true, cssAsc: "ascendente", cssDesc: "descendente", headers: { 8: { sorter: false }, 9: { sorter: false}} }).tablesorterPager({ container: $('#pager'), positionFixed: false, pagesize: 15000 });
                });
        }

   
    </script>

    <style type="text/css">
        
        .seccion2 { margin:0;padding: 0 0 0 4px;}
        .seccion2 .accion2 > table { width:100%;}

        .seccion2 .accion2 > table th   
        {
            vertical-align:middle; 
            height:25px; 
            padding: 0px 0px 0px 5px;  
            margin:0; 
            background-color:#F0f0f0; 
            text-align:left;  
            font-weight:bolder; 
            text-transform:uppercase;
        }

      
        .seccion2 .accion2 > table tr td:first-child { background-color:#F0f0f0; text-align:left; padding:5px; width:130px;
       
        }


        table.controles2 tr td > span input[type="checkbox"] {margin:0px 4px; }
        table.controles2 tr td > span label{ color:Gray; margin:0px 4px; width:150px; }
                

        .auto-style1 {
           width: 218px;
           border-bottom:1px solid #CCC;
           border-right:1px solid #CCC;
        }
        .auto-style2 {
            width: 127px;
            background-color:#F0f0f0; 
            text-align:left; 
            padding:5px; 
            border-bottom:1px solid #CCC;
            border-right:1px solid #CCC;
           
        }
        .auto-style3 {
            width: 164px;
            border-bottom:1px solid #CCC;
           border-right:1px solid #CCC;
            border-left:1px solid #CCC;
        }
        .auto-style4 {
            width: 144px;
            border-bottom:1px solid #CCC;
           border-right:1px solid #CCC;
            border-left:1px solid #CCC;
        }
        </style>

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
      
     <input id="id_factura" type="hidden" value="" runat="server" clientidmode="Static" />
     <input id="fec_factura" type="hidden" value="" runat="server" clientidmode="Static" />
     <input id="id_cliente" type="hidden" value="" runat="server" clientidmode="Static" />
     <input id="ruc_cliente" type="hidden" value="" runat="server" clientidmode="Static" />
     <input id="nombre_cliente" type="hidden" value="" runat="server" clientidmode="Static" />
     <input id="email_cliente" type="hidden" value="" runat="server" clientidmode="Static" />
     <input id="total_factura" type="hidden" value="" runat="server" clientidmode="Static" />
     <input id="iva_factura" type="hidden" value="" runat="server" clientidmode="Static" />
     <input id="porc_iva" type="hidden" value="" runat="server" clientidmode="Static" />
     <input id="ruta" type="hidden" value="" runat="server" clientidmode="Static" />

     <noscript><meta http-equiv="refresh" content="0; url=sinsoporte.htm" /> </noscript>
     <div>
        
         <i class="ico-titulo-2"></i><h1>Emisión de Nota de Crédito</h1><br />
    </div>
     <div class=" msg-alerta">
    <span id="dtlo" runat="server">Estimado usuario:</span> 
    <br /> ...
    </div>
     <div class="seccion2" id="BUSCAR">
      <div class="informativo">
      <table>
      <tr><td rowspan="2" class="inum"> <div class="number">1</div></td><td class="level1" >Datos de Factura</td></tr>
      <tr><td class="level2"></td></tr>
      </table>
     </div>
     <div class="colapser colapsa" ></div>
     <div class="accion3">
    <div class="separator">DATOS DE LA TRANSACCION</div>
      <asp:UpdatePanel ID="UpdateCabecera" runat="server"  ChildrenAsTriggers="true" UpdateMode="Conditional">
       <ContentTemplate>
     <table class="controles2" cellspacing="0" cellpadding="1">

       <tr><td  class="auto-style3">APLICA FACTURA</td>
         <td colspan="3" class="bt-bottom bt-right" >  
            
              <asp:TextBox ID="TxtNumeroFactura" runat="server" CssClass="inputText"  ClientIDMode="Static"
         Width="140px"></asp:TextBox>
              <span id="imagen"></span>
               <asp:Button ID="BtnBuscar" runat="server" OnClick="BtnBuscar_Click"   Text="Buscar" Width="80px" Height="26px" />  
                     <img alt="loading.." src="../shared/imgs/loader.gif" id="loader2" class="nover"  />     
              
         </td>   

         </tr> 
           <tr>
              <td  class="auto-style3">FECHA FACTURA</td>
              <td  colspan="3" class="bt-bottom bt-right">
                 
                  <asp:TextBox ID="TxtFechaFactura" runat="server" CssClass="inputText" ClientIDMode="Static" 
         Width="200px" ReadOnly="true" ></asp:TextBox>        
              </td>
          </tr> 
          <tr>
              <td  class="auto-style3">RUC CLIENTE</td>
              <td  colspan="3" class="bt-bottom bt-right"><asp:TextBox ID="TxtRucFactura" runat="server" CssClass="inputText" ClientIDMode="Static" 
         Width="200px" ReadOnly="true" ></asp:TextBox></td>
          </tr> 
          <tr>
              <td  class="auto-style3">NOMBRES</td>
              <td  colspan="3" class="bt-bottom bt-right"><asp:TextBox ID="TxtNombreFactura" runat="server" CssClass="inputText" ClientIDMode="Static" 
         Width="550px" ReadOnly="true" ></asp:TextBox></td>
          </tr> 
          <tr>
              <td  class="auto-style3">EMAIL</td>
              <td  colspan="3" class="bt-bottom bt-right"><asp:TextBox ID="TxtemailFactura" runat="server" CssClass="inputText" ClientIDMode="Static" 
         Width="550px" ReadOnly="true" ></asp:TextBox></td>
          </tr> 

         <tr><td  class="auto-style3">SUBTOTAL</td>
         <td colspan="3" class="bt-bottom bt-right" ><asp:TextBox ID="TxtSubtotalFactura" runat="server" CssClass="inputText" ClientIDMode="Static" 
         Width="300px" ReadOnly="true" ></asp:TextBox></td>  
             
         </tr> 
          <tr><td  class="auto-style3">IVA</td>
         <td colspan="3" class="bt-bottom bt-right"  ><asp:TextBox ID="TxtIvaFactura" runat="server" CssClass="inputText" ClientIDMode="Static" 
         Width="300px" ReadOnly="true" ></asp:TextBox></td>  
             
         </tr> 
         <tr>
         <td   class="auto-style3">TOTAL</td>
         <td colspan="3" class="bt-bottom bt-right" ><asp:TextBox ID="TxtTotalFactura" runat="server" CssClass="inputText" ClientIDMode="Static" 
         Width="300px" ReadOnly="true" ></asp:TextBox> </td>  
            
         </tr>
     </table>
 

     </ContentTemplate>
             <Triggers>
            <asp:AsyncPostBackTrigger ControlID="BtnBuscar" />    
              </Triggers>
         </asp:UpdatePanel>   

    <div class="separator">DATOS DE LA NOTA DE CREDITO</div>
    <table class="controles2" cellspacing="0" cellpadding="1">
      
        <tr><td  class="auto-style4">FECHA EMISION</td>
         <td class="bt-bottom bt-right">
            
             <asp:TextBox ID="TxtFechaEmision" runat="server" Width="120px" MaxLength="10" CssClass="datetimepicker"
             onkeypress="return soloLetras(event,'01234567890/')" 
                 onblur="valDate(this,true,valdate);" ClientIDMode="Static"></asp:TextBox>

         </td>  
             <td class="auto-style2"  >CONCEPTO</td><td class="bt-bottom bt-right">
                 <asp:UpdatePanel ID="UpdateCboConcepto" runat="server"  ChildrenAsTriggers="true" UpdateMode="Conditional">
                 <ContentTemplate>
                 <asp:DropDownList ID="CboConcepto" runat="server" Width="150px" AutoPostBack="False"
                                            Height="28px"  DataTextField='description' DataValueField='id_concept'  
                                            Font-Size="Small" > </asp:DropDownList>
                       </ContentTemplate>
                     <Triggers>
                     <asp:AsyncPostBackTrigger ControlID="BtnBuscar" />    
                    </Triggers>
                 </asp:UpdatePanel>   
                     </td>
         </tr>
           
         <tr><td  class="auto-style4">GLOSA</td>
         <td colspan="3" class="bt-bottom bt-right" >
             <asp:UpdatePanel ID="UpdateTxtGlosa" runat="server"  ChildrenAsTriggers="true" UpdateMode="Conditional">
                 <ContentTemplate>
             <asp:TextBox ID="TxtGlosa" runat="server" CssClass="inputText"  ClientIDMode="Static" MaxLength="200"
         Width="550px"></asp:TextBox>
                      </ContentTemplate>
                     <Triggers>
                     <asp:AsyncPostBackTrigger ControlID="BtnBuscar" />    
                    </Triggers>
                 </asp:UpdatePanel>   
         </td>  
            
         </tr>  
         <tr><td  class="auto-style4">SUBTOTAL $</td>
         <td class="auto-style1"  colspan="3" >
                <asp:UpdatePanel ID="UpdateTxtSubtotal" runat="server"  ChildrenAsTriggers="true" UpdateMode="Conditional">
                 <ContentTemplate>
             <asp:TextBox ID="TxtSubtotal" runat="server" Width="200px"  MaxLength="10" 
                onkeypress="return soloLetras(event,'1234567890.')" EnableViewState="False" Readonly="true"   Font-Bold ="true" >00.00</asp:TextBox>  
                      </ContentTemplate>
                     <Triggers>
                     <asp:AsyncPostBackTrigger ControlID="BtnBuscar" />    
                    </Triggers>
                 </asp:UpdatePanel>  
                  </td>  
             
         </tr> 
          <tr><td  class="auto-style4">IVA $</td>
         <td class="auto-style1"  colspan="3" >
               <asp:UpdatePanel ID="UpdateTxtIva" runat="server"  ChildrenAsTriggers="true" UpdateMode="Conditional">
                 <ContentTemplate>
             <asp:TextBox ID="TxtIva" runat="server" Width="200px"  MaxLength="10"  
                onkeypress="return soloLetras(event,'1234567890.')" EnableViewState="False" Readonly="true"  Font-Bold ="true" >00.00</asp:TextBox>
                          </ContentTemplate>
                     <Triggers>
                     <asp:AsyncPostBackTrigger ControlID="BtnBuscar" />    
                    </Triggers>
                 </asp:UpdatePanel>     
                       </td>  
               
         </tr> 
         <tr>
         <td   class="auto-style4">TOTAL $</td>

         <td class="auto-style1" colspan="3" >
             <asp:UpdatePanel ID="UpdateTxtTotal" runat="server"  ChildrenAsTriggers="true" UpdateMode="Conditional">
                 <ContentTemplate>
             <asp:TextBox ID="TxtTotal" runat="server" Width="200px"  MaxLength="10"  
                onkeypress="return soloLetras(event,'1234567890.')" EnableViewState="False" Font-Bold ="true" Readonly="true" >00.00</asp:TextBox>     
                </ContentTemplate>
                     <Triggers>
                     <asp:AsyncPostBackTrigger ControlID="BtnBuscar" />    
                    </Triggers>
                 </asp:UpdatePanel>    
         </td>  
             
         </tr>   
         <tr>
         <td   class="auto-style4">CARGAR DOCUMENTO</td>

         <td class="auto-style1" colspan="3" >
        
              <asp:AsyncFileUpload ID="fsuploadarchivo" class="uploader" runat="server" onChange="uploadImage();"  title="Escoja el archivo con formato indicado .pdf"  style=" font-size:small" visible="true" />
                 <asp:AsyncFileUpload ID="AsyncFileUpload1" class="uploader" runat="server"   title="Escoja el archivo con formato indicado .pdf"  style=" font-size:small" visible="false" /> 
             <asp:UpdatePanel ID="UpdateRuta" runat="server"   ChildrenAsTriggers="true" UpdateMode="Conditional">  
                 <ContentTemplate>
             <asp:Label ID="LblRuta" runat="server" Text=".." Font-Bold="true" ForeColor="Red"></asp:Label><br>
                   </ContentTemplate>
                  <Triggers>
                     <asp:AsyncPostBackTrigger ControlID="BtnBuscar" />    
                    </Triggers>
                  </asp:UpdatePanel>    
               <asp:Button ID="btsubir" runat="server" Text="Subir archivo" OnClick="btsubir_Click" />
           
           
         </td>  
             
         </tr>   
     </table>
        
          

          <div class="cataresult" >
             <asp:UpdatePanel ID="UpdatePanel3" runat="server" ChildrenAsTriggers="true">
                     <ContentTemplate>
                       <input id="Identificado1" type="hidden" runat="server" clientidmode="Static" />
                       <input id="Identificado2" type="hidden" runat="server" clientidmode="Static" />
                  </ContentTemplate>
                     <Triggers>
                         <asp:AsyncPostBackTrigger ControlID="BtnNuevo" />
                         <asp:AsyncPostBackTrigger ControlID="BtnGrabar" />
                         <asp:AsyncPostBackTrigger ControlID="ChkTodos" />
                         <%--  <asp:AsyncPostBackTrigger ControlID="btsubir" />--%>
                     </Triggers>
                 </asp:UpdatePanel>
             </div>     

          <div class="botonera" >    
              <asp:CheckBox ID="ChkTodos" runat="server"  AutoPostBack="True" oncheckedchanged="ChkTodos_CheckedChanged" BorderStyle="Outset" Text="Devolver Todos  "  />
           &nbsp;
           <asp:Button ID="BtnNuevo" runat="server"   OnClick="BtnNuevo_Click" Text="Nuevo" Width="100px" ForeColor="Blue"/>
              &nbsp;<asp:Button ID="BtnGrabar" runat="server"   OnClick="BtnGrabar_Click" Text="Grabar" Width="100px" />
                 </div>
        
     </div>
    </div>
     <div class="seccion">
      <div class="informativo">
      <table>
      <tr><td rowspan="2" class="inum"> <div class="number">2</div></td><td class="level1" >DETALLE DE ITEMS</td></tr>
      <tr><td class="level2">
      Digite la cantidad a devolver de la nota de crédito
      </td></tr>
      </table>
     </div>
    <div class="colapser colapsa"></div>
     <div class="accion">
         <div class="cataresult" >

          <asp:UpdatePanel ID="upresult" runat="server" >
                     <ContentTemplate>
                       <%-- <script type="text/javascript">
                            Sys.Application.add_load(BindFunctions); 
                      </script>--%>
             <div id="xfinder" runat="server" visible="false" >

            <div class="findresult" >
              <div class="booking" >
                    <div class="separator">DETALLE DE GRUPOS</div>
                    <div class="bokindetalle">

                <asp:Repeater ID="tablePagination" runat="server" >
                 <HeaderTemplate>
                 <table id="tablasort" cellspacing="1" cellpadding="1" class="tabRepeat">
                 <thead>
                 <tr>
 
                 <th class="nover">Cód.Item</th>
                 <th class="nover">BL</th>        
                 <th>Cód. Servicio</th>
                 <th>Servicio</th>
                 <th>Cantidad</th>
                 <th>Precio</th>
                 <th>Subtotal</th>
                 <th>Iva</th>
                 <th>Carga</th>
                 <th>Devolver</th>  
                 <th class="nover">IVA</th> 
                 <th>N/C Subtotal</th>
                 <th>N/C IVA</th>
                 </tr>
                 </thead> 
                 <tbody>
                 </HeaderTemplate>
                 <ItemTemplate>
                 <tr class="point" >
                  <td class="nover"><asp:Label Text='<%#Eval("codigo_item")%>' ID="lbl_codigo_item" runat="server"  /></td>
                  <td class="nover"><asp:Label Text='<%#Eval("unidad_bl")%>' ID="lbl_unidad_bl" runat="server"  /></td>
                  <td ><asp:Label Text='<%#Eval("codigo_servicio")%>' ID="lbl_codigo_servicio" runat="server" /></td>
                  <td ><asp:Label Text='<%#Eval("desc_servicio")%>' ID="lbl_desc_servicio" runat="server" /></td>
                  <td ><asp:Label Text='<%#Eval("cantidad")%>' ID="lbl_cantidad" runat="server" /></td>
                  <td ><asp:Label Text='<%#Eval("precio")%>' ID="lbl_precio" runat="server" /></td>
                  <td ><asp:Label Text='<%#Eval("subtotal")%>' ID="lbl_subtotal" runat="server" /></td>
                  <td ><asp:Label Text='<%#Eval("iva")%>' ID="lbl_iva" runat="server" /></td> 
                  <td style="width:100px;"><asp:Label Text='<%#Eval("numero_carga")%>' ID="lbl_numero_carga" runat="server" /></td>   
                  <td>
                      <asp:UpdatePanel ID="UpdatePanel2" runat="server" ChildrenAsTriggers="true">
                     <ContentTemplate>
                       <asp:TextBox ID="TxtNewCantidad"   OnTextChanged="TxtNewCantidad_TextChanged"  Width="50px"
                           onkeypress="return soloLetras(event,'1234567890.')" AutoPostBack="true"   Font-Size="X-Small" runat="server"></asp:TextBox>
                          </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="TxtNewCantidad" />
                        </Triggers>
                    </asp:UpdatePanel>
                  </td>
                 <td class="nover"><asp:Label Text='<%#Eval("iva_porcentaje")%>' ID="lbl_porc_iva" runat="server" /></td>   
                 <td ><asp:Label Text='<%#Eval("nc_subtotal")%>' ID="lbl_nc_subtotal" runat="server" /></td> 
                 <td ><asp:Label Text='<%#Eval("nc_iva")%>' ID="lbl_nc_iva" runat="server" /></td>
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
             </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="BtnGrabar" />
                </Triggers>
                 
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
  
    
    
     </div>
    </div>
     <script src="../Scripts/pages.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.tablesorter.min.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.tablesorter.pager.js" type="text/javascript"></script>
    <script src="../Scripts/nota_credito.js" type="text/javascript"></script>

    <%-- <script type="text/javascript">
                            Sys.Application.add_load(BindFunctions); 
                      </script>--%>

  <script type="text/javascript">
        $(document).ready(function () {
              $('.datetimepicker').datetimepicker().datetimepicker({ lang: 'es', timepicker: false, closeOnDateSelect: true, format: 'd/m/Y' });
        });
  </script>


    <script type="text/javascript">

          $(window).load(function () {
                            $("div.colapser").toggle(function () { $(this).removeClass("colapsa").addClass("expande"); $(this).next().hide(); }
                                  , function () { $(this).removeClass("expande").addClass("colapsa"); $(this).next().show(); });
        });


        function defaultDate(control) {
            if (control.value) {
                var cj = document.getElementById("TxtFechaEmision");
                if (cj != null && cj != undefined) {
                    cj.value = control.value;
                }
            }
        }
       
        function uploadImage() {
            this.document.getElementById("ruta").value = "";
            var txt = document.getElementById("<%=fsuploadarchivo.ClientID %>");
            if (txt != null && txt != undefined) {
                this.document.getElementById("ruta").value = txt.value;
            }
             
            document.getElementById("<%=LblRuta.ClientID %>").value = "";
         
        }
      <%--  function success() {         
            var fu = document.getElementById("<%=fsuploadarchivo.ClientID %>"); 
            document.getElementById("<%=fsuploadarchivo.ClientID %>").innerHTML = fu.innerHTML; 
        } --%>
    
    </script>

 
    
</asp:Content>