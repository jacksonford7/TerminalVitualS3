<%@ Page Language="C#" Title="Lista de Bookings" AutoEventWireup="true" CodeBehind="catalogo_pago_terceros.aspx.cs" Inherits="CSLSite.catalogo_pago_terceros" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Catálogo de Pago a Terceros</title>
    
             <link href="../img/favicon2.png" rel="icon"/>
  <link href="../img/icono.png" rel="apple-touch-icon"/>
  <link href="../css/bootstrap.min.css" rel="stylesheet"/>
  <link href="../css/dashboard.css" rel="stylesheet"/>
  <link href="../css/icons.css" rel="stylesheet"/>
  <link href="../css/style.css" rel="stylesheet"/>
  <link href="https://fonts.googleapis.com/css2?family=Roboto:wght@300;400;500;700&display=swap" rel="stylesheet"/>
    
    
    
    <link href="../shared/estilo/catalogo_pago_tercero.css" rel="stylesheet" type="text/css" />

   <link href="../shared/estilo/tables.css" rel="stylesheet" type="text/css" />




    <style type="text/css">
        .GridPager a,
        .GridPager span {
            display: inline-block;
            padding: 0px 9px;
            margin-right: 4px;
            border-radius: 3px;
            border: solid 1px #c0c0c0;
            background: #e9e9e9;
            box-shadow: inset 0px 1px 0px rgba(255,255,255, .8), 0px 1px 3px rgba(0,0,0, .1);
            font-size: .875em;
            font-weight: bold;
            text-decoration: none;
            color: #717171;
            text-shadow: 0px 1px 0px rgba(255,255,255, 1);
        }

        .GridPager a {
            background-color: #f5f5f5;
            color: #969696;
            border: 1px solid #969696;
        }

        .GridPager span {

            background: #616161;
            box-shadow: inset 0px 0px 8px rgba(0,0,0, .5), 0px 1px 0px rgba(255,255,255, .8);
            color: #f0f0f0;
            text-shadow: 0px 0px 3px rgba(0,0,0, .5);
            border: 1px solid #3AC0F2;
        }
        .disnone{
            display:none
        }
        .mayusculas{text-transform:uppercase;} 
    </style>
    <script type="text/javascript">
        function BindFunctions() {
            document.getElementById('imagen').innerHTML = '';
            $(document).ready(function () {
                $('#tablasort').tablesorter({ cancelSelection: true, cssAsc: "ascendente", cssDesc: "descendente", headers: { 5: { sorter: false }, 6: { sorter: false}} }).tablesorterPager({ container: $('#pager'), positionFixed: false, pagesize: 10 });
            });
        }
    </script>
</head>
<body>

    <form id="bookingfrm" runat="server">
    <asp:ToolkitScriptManager ID="tkscata" runat="server"></asp:ToolkitScriptManager>
   
           <div class="dashboard-container p-4" id="cuerpo" runat="server">

         <div class="form-title">Datos del documento buscado</div>
		 
		  <div class="form-row">
		  
		   <div class="form-group col-md-12"> 
		   	  <label for="inputAddress">Ruc que asume el pago a tercero(s)<span style="color: #FF0000; font-weight: bold;"></span></label>
			   <div class="d-flex" >
                    <asp:TextBox 
                    
                    ID="txtrucbuscarcli" runat="server" CssClass=" form-control"   MaxLength="13" placeholder="ESCRIBA EL RUC"
                    onkeypress="return soloLetras(event,'abcdefghijklmnñopqrstuvwxyz0123456789')" 
                    ></asp:TextBox> 
                    <asp:Button ID="find" runat="server" Text="Buscar" CssClass="btn btn-primary" onclick="find_Click"  OnClientClick="return initFinder();"/>
             <span id="imagen"></span>
			   </div>
               
              
		   </div>



		  </div>
		  
                  <div class="form-row">
		   <div class="col-md-12 d-flex justify-content-center"> 
                 <p class=" alert alert-light">Escriba el Ruc del Cliente y pulse buscar</p>
		   </div> 
		   </div>

                   <div class="cataresult" >
             <asp:UpdatePanel ID="upresult" runat="server"  >
             <ContentTemplate>
             <script type="text/javascript">Sys.Application.add_load(BindFunctions); </script>
             <div id="xfinder" runat="server">
                 <asp:GridView runat="server" class="table table-bordered invoice" PageSize="15" Font-Size="Medium" AutoGenerateColumns="False" DataSourceID="SqlDSCatPagoTerceros" AllowPaging="True" DataKeyNames="SAPT_CODIGO" AllowSorting="True">
                         <PagerStyle HorizontalAlign = "Center" Font-Size="Large" CssClass = "GridPager" />
                         <Columns>
                             <asp:BoundField DataField="SAPT_CODIGO" Visible="false" />
                             <asp:BoundField DataField="USUARIO_MOD" ItemStyle-CssClass="disnone" HeaderStyle-CssClass="disnone"/>
                             <asp:BoundField DataField="FECHA_MOD" ItemStyle-CssClass="disnone" HeaderStyle-CssClass="disnone"/>
                             <asp:BoundField DataField="SEPT_RUC_CLIENTE" HeaderStyle-Font-Size="Small" HeaderText="Ruc Ciente" ReadOnly="True" HeaderStyle-Width="80px" SortExpression="SEPT_RUC_CLIENTE" >
                             <HeaderStyle Width="80px" />
                             </asp:BoundField>
                             <asp:BoundField DataField="RAZSOCIALCLI" HeaderStyle-Font-Size="Small" HeaderText="Razon Social Cliente" ReadOnly="True" HeaderStyle-Width="200px"  SortExpression="RAZSOCIALCLI" >
                             <HeaderStyle Width="200px" />
                             </asp:BoundField>
                             <asp:BoundField DataField="SEPT_RUC_ASUME" HeaderStyle-Font-Size="Small" HeaderText="Ruc Asume" ControlStyle-Width="110px" HeaderStyle-Width="80px" SortExpression="SEPT_RUC_ASUME" >
                             <ControlStyle Width="110px" CssClass="mayusculas"/>
                             <HeaderStyle Width="80px" />
                             </asp:BoundField>
                             <asp:BoundField DataField="RAZSOCIALASUME" HeaderStyle-Font-Size="Small" HeaderText="Razon Social Asume" ReadOnly="True" HeaderStyle-Width="200px" SortExpression="RAZSOCIALASUME" >
                             <HeaderStyle Width="200px" />
                             </asp:BoundField>
                             <asp:CheckBoxField DataField="SEPT_ESTADO" HeaderStyle-Font-Size="Small" HeaderText="Estado"  HeaderStyle-Width="50px" SortExpression="SEPT_ESTADO" >
                             <HeaderStyle Width="50px" />
                             </asp:CheckBoxField>
                             <asp:BoundField DataField="SEPT_USUARIOINGRESO" HeaderStyle-Font-Size="Small" ReadOnly="True" HeaderStyle-Width="80px" HeaderText="Usuario Ing" SortExpression="SEPT_USUARIOINGRESO" >
                             <HeaderStyle Width="80px" />
                             </asp:BoundField>
                             <asp:BoundField DataField="SEPT_FECHAINGRESO" HeaderStyle-Font-Size="Small" ReadOnly="True" HeaderStyle-Width="80px" HeaderText="Fecha Ing" SortExpression="SEPT_FECHAINGRESO" >
                             <HeaderStyle Width="80px" />
                             </asp:BoundField>
                             <asp:BoundField DataField="SEPT_USUARIOMODIFICA" HeaderStyle-Font-Size="Small" ReadOnly="True" HeaderStyle-Width="80px" HeaderText="Usuario Mod" SortExpression="SEPT_USUARIOMODIFICA" >
                             <HeaderStyle Width="80px" />
                             </asp:BoundField>
                             <asp:BoundField DataField="SEPT_FECHAMODIFICA" HeaderStyle-Font-Size="Small" ReadOnly="True" HeaderStyle-Width="80px" HeaderText="Fecha Mod" SortExpression="SEPT_FECHAMODIFICA" >
                             <HeaderStyle Width="80px" />
                             </asp:BoundField>
                             <asp:CommandField HeaderText="Acciones"  ButtonType="Button" HeaderStyle-Width="150px" ShowEditButton="True" >
                             <HeaderStyle Width="150px" />
                             </asp:CommandField>
                         </Columns>
                     </asp:GridView>
                 <asp:SqlDataSource ID="SqlDSCatPagoTerceros" runat="server" ConnectionString="<%$ ConnectionStrings:validar %>"
                         SelectCommand="SP_CONSULTA_CAT_PAGO_TERCEROS" SelectCommandType="StoredProcedure"
                         UpdateCommand="UPDATE SIM_FILTRO_CLIENTES SET SEPT_ESTADO = @SEPT_ESTADO, SEPT_RUC_ASUME = @SEPT_RUC_ASUME, SEPT_USUARIOMODIFICA = @USUARIO_MOD, SEPT_FECHAMODIFICA = @FECHA_MOD WHERE (SAPT_CODIGO = @SAPT_CODIGO)">
                         <SelectParameters>
                             <asp:ControlParameter ControlID="txtrucbuscarcli" DefaultValue="%" Name="RUC_CLIENTE" PropertyName="Text" Type="String" />
                             <asp:SessionParameter Name="USUARIO_LOGIN" SessionField="MantPagoTercerosUsuaurioLogin" Type="String" />
                         </SelectParameters>
                         <UpdateParameters>
                             <asp:Parameter Name="SEPT_ESTADO"  />
                             <asp:Parameter Name="SEPT_RUC_ASUME" />
                             <asp:Parameter Name="USUARIO_MOD" />
                             <asp:Parameter Name="FECHA_MOD" DbType="DateTime" />
                             <asp:Parameter Name="SAPT_CODIGO" />
                         </UpdateParameters>
                     </asp:SqlDataSource>
             
            
        
              <div id="sinresultado" runat="server" class=" alert alert-primary" visible="false" ></div>
             </div>
             </ContentTemplate>
             <Triggers>
             <asp:AsyncPostBackTrigger ControlID="find" />
             </Triggers>
             </asp:UpdatePanel>
       </div>
     </div>
        
        
        
     
      

     
     
   
    </form>

   <script type="text/javascript" >
       function setObject(row) {
           var celColect = row.getElementsByTagName('td');
          var bookin = {
              fila: celColect[0].textContent,
              gkey: celColect[1].textContent,
              nbr: celColect[2].textContent,
              linea: celColect[3].textContent,
              fk: celColect[4].textContent
              };
            if (window.opener != null) {
                window.opener.popupCallback(bookin, 'bk');
            }
            self.close();
      }
      function initFinder() {
          if (document.getElementById('txtname').value.trim().length <= 0) {
              alert('Por favor escriba una o varias \nletras del número');
              return false;
          }
          document.getElementById('imagen').innerHTML = '<img alt="" src="../shared/imgs/loader.gif">';
      }


      
   </script>
    <script src="../Scripts/jquery-1.7.1.min.js" type="text/javascript"></script>
    <script src="../Scripts/pages.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.tablesorter.min.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.tablesorter.pager.js" type="text/javascript"></script>
</body>
</html>
