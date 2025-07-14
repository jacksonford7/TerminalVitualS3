<%@ Page Language="C#" Title="Detalle Actualización o Cancelación Pase Puerta" AutoEventWireup="true" CodeBehind="detalle_can_act_pasepuerta.aspx.cs" Inherits="CSLSite.detalle_can_act_pasepuerta" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Detalle Actualización o Cancelación Pase Puerta</title>
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
     <noscript><meta http-equiv="refresh" content="0; url=sinsoporte.htm" /> </noscript>
    <form id="bookingfrm" runat="server">
    <asp:ToolkitScriptManager ID="tkscata" runat="server"></asp:ToolkitScriptManager>
        <div class="catabody">
            <div class="catawrap" >
                <div class="catabuscar" align="center">
                    <div class="catacapa">
                        
             <span id="imagen"></span>
          </div>
         <p class="catavalida">Detalle de Consulta para Actualización o Cancelación de Pase de Puerta:</p>
         </div>
         <div class="cataresult" >
             <asp:UpdatePanel ID="upresult" runat="server" >
             <ContentTemplate>
             <script type="text/javascript">Sys.Application.add_load(BindFunctions); </script>
             <div id="xfinder" runat="server">
             <%-- catalogo de bookings--%>
             <div class="findresult" >
             <div class="booking" >
                 <div class="bokindetalle">
                 <asp:Repeater ID="tablePaginationPPWeb" runat="server" >
                 <HeaderTemplate>
                 <table id="tablasort" cellspacing="1" cellpadding="1" class="tabRepeat" style=" font-size:Small">
                 <thead>
                     <th style=" width:100px">Mrn-Msn-Hsn</th>
                     <th style=" width:50px">Contenedor</th>
                     <th style=" width:70px">Fec. Salida</th>
                     <th style=" width:70px">Turno</th>
                     <th style=" width:200px;">Cia. Trans</th>
                     <th style=" width:190px;">Chofer</th>
                     <th style=" width:50px;">Placa</th>
                     <th style=" width:80px">Actualizar Turno</th>
                     <th style=" width:200px;">Actualizar Cia. Trans</th>
                     <th style=" width:200px;">Actualizar Chofer</th>
                     <th style=" width:50px;">Actualizar Placa</th>
                     <th style=" width:50px;">Act. Pase</th>
                     <th style=" width:50px;">Can. Pase</th>
                 </thead> 
                 <tbody>
                 </HeaderTemplate>
                 <ItemTemplate>
                 <tr class="point">
                  <td style=" width:100px"><asp:Label Text='<%#Eval("CARGA")%>' ID="lblCarga" runat="server" /></td>
                  <td style=" width:50px"><asp:Label Text='<%#Eval("CONTENEDOR")%>' ID="lblCntr" runat="server" /></td>
                  <td style=" width:70px"><asp:Label Text='<%#Eval("FECHA_AUT_PPWEB")%>' ID="lblFecAutPPWeb" runat="server" /></td>
                  <td style=" width:70px"><%#Eval("TURNO")%></td>
                  <td style=" width:200px"><%#Eval("CIATRANS")%></td>
                  <td style=" width:190px"><%#Eval("CHOFER")%></td>
                  <td style=" width:50px"><%#Eval("PLACA")%></td>
                  <td>
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" ChildrenAsTriggers="true">
                    <ContentTemplate>
                        <asp:DropDownList runat="server" ID="ddlTurno" Font-Size="X-Small" Width="80px" AutoPostBack="true" onselectedindexchanged="ddlTurno_SelectedIndexChanged">
                        </asp:DropDownList>
                    </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="ddlTurno" />
                        </Triggers>
                    </asp:UpdatePanel>
                  </td>
                  <td>
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server" ChildrenAsTriggers="true">
                    <ContentTemplate>
                        <asp:DropDownList ID="ddlEmpresa" runat="server" Font-Size="X-Small"  Width="200px" AutoPostBack="true" onselectedindexchanged="ddlEmpresa_SelectedIndexChanged">
                        </asp:DropDownList>
                    </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="ddlEmpresa" />
                        </Triggers>
                    </asp:UpdatePanel>
                  </td>
                  <td >
                    <asp:UpdatePanel ID="UpdatePanel3" runat="server" ChildrenAsTriggers="true">
                    <ContentTemplate>
                        <asp:DropDownList runat="server" ID="ddlChofer" AutoPostBack="True" Font-Size="X-Small" Width="200px" onselectedindexchanged="ddlChofer_SelectedIndexChanged">
                        </asp:DropDownList>
                    </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="ddlChofer" />
                        </Triggers>
                    </asp:UpdatePanel>
                  </td>
                  <td style=" width:50px"><asp:TextBox runat="server" Font-Size="X-Small" ID="txtPlaca" Width="50px" /></td>
                  <td style=" width:50px">
                        <asp:CheckBox id="chkPase" runat="server" AutoPostBack="True" oncheckedchanged="chkPase_CheckedChanged" />
                 </td>
                 <td style=" width:50px">
                    <asp:UpdatePanel ID="UpdatePanel5" runat="server" ChildrenAsTriggers="true">
                    <ContentTemplate>
                        <asp:CheckBox id="chkCanPase" runat="server" AutoPostBack="True"  />
                    </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="chkCanPase" />
                        </Triggers>
                    </asp:UpdatePanel>
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
                  <option  value="20">20</option>
                  <option value="30">30</option>
                  <option value="40">40</option>
                  <option value="50">50</option>
                  </select>
                 <img alt="" src="../shared/imgs/first.gif" class="first"/>
                 <img alt="" src="../shared/imgs/prev.gif" class="prev"/>
                 <input  type="text" class="pagedisplay" size="5px"/>
                 <img alt="" src="../shared/imgs/next.gif" class="next"/>
                 <img alt="" src="../shared/imgs/last.gif" class="last"/>
            </div>
              <div id="sinresultado" runat="server" class="msg-info" visible="false" ></div>
              </div>
             </ContentTemplate>
             <Triggers>      
                
             </Triggers>
             </asp:UpdatePanel>
             
       </div>
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


      
   </script>
    <script src="../Scripts/jquery-1.7.1.min.js" type="text/javascript"></script>
    <script src="../Scripts/pages.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.tablesorter.min.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.tablesorter.pager.js" type="text/javascript"></script>
</body>
</html>
