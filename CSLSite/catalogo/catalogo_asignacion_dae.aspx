<%@ Page Language="C#" Title="Asignacion DAE" AutoEventWireup="true" CodeBehind="catalogo_asignacion_dae.aspx.cs" Inherits="CSLSite.catalogo_asignacion_dae" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Catálogo asignación DAE</title>
         <link href="../img/favicon2.png" rel="icon"/>
  <link href="../img/icono.png" rel="apple-touch-icon"/>
  <link href="../css/bootstrap.min.css" rel="stylesheet"/>
  <link href="../css/dashboard.css" rel="stylesheet"/>
  <link href="../css/icons.css" rel="stylesheet"/>
  <link href="../css/style.css" rel="stylesheet"/>
  <link href="https://fonts.googleapis.com/css2?family=Roboto:wght@300;400;500;700&display=swap" rel="stylesheet"/>
    
    <link href="../shared/estilo/catalogo_pago_tercero.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/work.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/aisv-general.css" rel="stylesheet" type="text/css" />
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
		  <div class="form-row">
		       <div class="form-group col-md-3"> 
		   	  <label for="inputAddress">Referencia<span style="color: #FF0000; font-weight: bold;"></span></label>
			      <asp:TextBox 
                                   
                                    ID="txtRef" runat="server" CssClass=" form-control"  
                     
                      MaxLength="13"
                      placeholder="REFERENCIA"
                                    onkeypress="return soloLetras(event,'abcdefghijklmnñopqrstuvwxyz0123456789_-')" 
                                    ></asp:TextBox>  
		   </div>
               <div class="form-group col-md-3"> 
		   	  <label for="inputAddress">Booking:<span style="color: #FF0000; font-weight: bold;"></span></label>
			     <asp:TextBox 
                                   
                                    ID="txtbkg" runat="server" CssClass=" form-control" 
                     MaxLength="50" placeholder="BOOKING"
                                    onkeypress="return soloLetras(event,'abcdefghijklmnñopqrstuvwxyz0123456789_-')" 
                                    ></asp:TextBox>  
		   </div>
               <div class="form-group col-md-3"> 
		   	  <label for="inputAddress">Contenedor<span style="color: #FF0000; font-weight: bold;"></span></label>
			<asp:TextBox 
                                    
                                    ID="txtContenedor" runat="server"
                CssClass=" form-control"  
                MaxLength="13" placeholder="CONTENEDOR"
                                    onkeypress="return soloLetras(event,'abcdefghijklmnñopqrstuvwxyz0123456789_-')" 
                                    ></asp:TextBox> 
		   </div>
               <div class="form-group col-md-3"> 
		   	  <label for="inputAddress">D.A.E<span style="color: #FF0000; font-weight: bold;"></span></label>
			  <asp:TextBox 
                                    
                                    ID="txtDAE" runat="server" CssClass=" form-control"
                                     MaxLength="20" placeholder="D.A.E"
                                    onkeypress="return soloLetras(event,'abcdefghijklmnñopqrstuvwxyz0123456789_-')" 
                                    ></asp:TextBox> 
		   </div>

		  </div>
          <div class="form-row">
		   <div class="col-md-12 d-flex justify-content-center"> 
               <div class="d-flex">
              <asp:Button ID="find" runat="server" Text="Buscar" onclick="find_Click" CssClass="btn btn-primary" />
               <span id="imagen"></span></div>
		   </div> 
		   </div>
          <div class="form-row">
		   <div class="col-md-12 d-flex justify-content-center"> 
		       <p class=" alert alert-light">Escriba el Booking, Contenedor o D.A.E y pulse buscar</p>
           </div> 
		   </div>
       </div>
        
        
        
 

         <div class="cataresult" >
             <asp:UpdatePanel ID="upresult" runat="server"  >
             <ContentTemplate>
             <script type="text/javascript">Sys.Application.add_load(BindFunctions); </script>
             <div id="xfinder" runat="server">
             <div class="booking" >
                 <div class="bokindetalle">
                 <asp:GridView runat="server" class="table table-bordered invoice" Font-Size="Medium" AutoGenerateColumns="False" DataSourceID="SqlDSAsignacionDAE" PageSize="20" AllowPaging="True" AllowSorting="True">
                         <PagerStyle HorizontalAlign = "Center" Font-Size="Large" CssClass = "GridPager" />
                         <Columns>
                         
                             <asp:BoundField DataField="BKG" HeaderStyle-Font-Size="Small"  HeaderText="BKG" SortExpression="BKG">
                             </asp:BoundField>
                             <asp:BoundField DataField="CNTR" HeaderStyle-Font-Size="Small" HeaderText="CNTR" HeaderStyle-Width="80px" SortExpression="CNTR" >
                             </asp:BoundField>
                             <asp:BoundField DataField="DAE" HeaderStyle-Font-Size="Small" HeaderText="DAE" HeaderStyle-Width="200px"  SortExpression="DAE" >
                             </asp:BoundField>
                             <asp:BoundField DataField="IIE" HeaderStyle-Font-Size="Small" HeaderText="CÓDIGO ECUAPASS (ACEPTACIÓN IIE)" HeaderStyle-Width="200px" SortExpression="IIE" >
                             </asp:BoundField>
                             <asp:BoundField DataField="USUARIOING" HeaderStyle-Font-Size="Small" HeaderStyle-Width="80px" HeaderText="USUARIO" SortExpression="USUARIOING" >
                             </asp:BoundField>
                             <asp:BoundField DataField="FECHAING" HeaderStyle-Font-Size="Small" HeaderStyle-Width="80px" HeaderText="FECHA ASIGNACION DAE" SortExpression="FECHAING" >
                             </asp:BoundField>
                         </Columns>
                     </asp:GridView>
                     <asp:SqlDataSource ID="SqlDSAsignacionDAE" runat="server" ConnectionString="<%$ ConnectionStrings:ecuapass %>" SelectCommand="SP_CONSULTA_ASIGNACION_DAE" SelectCommandType="StoredProcedure" >
                         <SelectParameters>
                             <asp:SessionParameter Name="RUC" SessionField="RUC_ASIGNACION_DAE" Type="String" />
                             <asp:ControlParameter ControlID="txtbkg" DefaultValue="%" Name="BOOKING" PropertyName="Text" Type="String" />
                             <asp:ControlParameter ControlID="txtContenedor" DefaultValue="%" Name="CNTR" PropertyName="Text" Type="String" />
                             <asp:ControlParameter ControlID="txtDAE" DefaultValue="%" Name="DAE" PropertyName="Text" Type="String" />
                             <asp:ControlParameter ControlID="txtRef" DefaultValue="0" Name="REFERENCIA" 
                                 PropertyName="Text" Type="String" />
                         </SelectParameters>
                     </asp:SqlDataSource>
                </div>
             </div>
              <div id="sinresultado" runat="server" class=" alert alert-primary" visible="false" ></div>
              </div>
             </ContentTemplate>
             <Triggers>
             <asp:AsyncPostBackTrigger ControlID="find" />
             </Triggers>
             </asp:UpdatePanel>
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


      
   </script>
     <script src="../lib/jquery/jquery.min.js" type="text/javascript"></script>
    <script src="../Scripts/pages.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.tablesorter.min.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.tablesorter.pager.js" type="text/javascript"></script>
</body>
</html>
