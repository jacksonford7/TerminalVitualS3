<%@ Page  Title="Asignar Depositos"  Language="C#" MasterPageFile="~/site.Master" AutoEventWireup="true" CodeBehind="line_depots.aspx.cs" Inherits="CSLSite.line_depots" MaintainScrollPositionOnPostback="True" %>
<%@ MasterType VirtualPath="~/site.Master" %>


<asp:Content ID="Content1" ContentPlaceHolderID="placehead" runat="server">
    <link href="../shared/estilo/aisv-general.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/work.css" rel="stylesheet" type="text/css" />
     <link href="../shared/estilo/tables.css" rel="stylesheet" type="text/css" />
     <link href="../shared/estilo/catalogos.css" rel="stylesheet" type="text/css" />
     <link href="../shared/estilo/info.css" rel="stylesheet" type="text/css" />

    <link href="../img/favicon2.png" rel="icon"/>
    <link href="../img/icono.png" rel="apple-touch-icon"/>
    <link href="../css/bootstrap.min.css" rel="stylesheet"/>
    <link href="../css/dashboard.css" rel="stylesheet"/>
    <link href="../css/icons.css" rel="stylesheet"/>
    <link href="../css/style.css" rel="stylesheet"/>
    <link href="https://fonts.googleapis.com/css2?family=Roboto:wght@300;400;500;700&display=swap" rel="stylesheet"/>

    <link href="../shared/estilo/Reset.css" rel="stylesheet" />
    <link href="../shared/estilo/jquery.datetimepicker.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/tables.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/info.css" rel="stylesheet" type="text/css" />



    </asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="placebody" runat="server" >
    <input id="zonaid" type="hidden" value="205" />
     <noscript><meta http-equiv="refresh" content="0; url=sinsoporte.htm" /> </noscript>
<%--     <div>
        
         <i class="ico-titulo-2"></i><h1>Transacción de Asignación de Depósitos a un Línea Naviera</h1><br />
    </div>--%>

    <div class="mt-4">         
        <nav class="mt-4" aria-label="breadcrumb">
            <ol class="breadcrumb">
            <li class="breadcrumb-item" id="opcion_principal" runat="server"><a href="#">Depósito de Vacíos</a></li>
                <li class="breadcrumb-item active" aria-current="page" id="sub_opcion" runat="server">Transacción de Asignación de Depósitos a un Línea Naviera</li>
            </ol>
        </nav>
    </div>


    <div class="alert alert-warning">
        <span id="dtlo" runat="server">Estimado usuario:</span> 
        <br /> ...
    </div>

    <div class="dashboard-container p-4" id="cuerpo" runat="server">

         <div class="form-title">
           1.- Datos de Depósito
        </div>

        <h6>Debe seleccionar el depósito, para realizar la asignación a la línea naviera</h6>

        <div >
            <div class="form-row">
                <%--<div class="seccion" id="BUSCAR">--%>
                    <%--<div class="informativo">
                        <table>
                            <tr><td rowspan="2" class="inum"> <div class="number">1</div></td><td class="level1" >Datos de Depósito</td></tr>
                            <tr><td class="level2">Debe seleccionar el depósito, para realizar la asignación a la línea naviera</td></tr>
                        </table>
                    </div>--%>

                   <%-- <div class="colapser colapsa" ></div>
                    <div class="accion">--%>


                      <%--  <table class="controles" cellspacing="0" cellpadding="1">
                            <tr><th class="bt-bottom bt-right bt-top bt-left" colspan="4"></th></tr>
                            <tr>
                                <td class="bt-bottom  bt-right bt-left">LINEA NAVIERA:</td>
                                <td class="bt-bottom bt-right"> 
                                    <span id="nombre" class="caja cajafull">
                                        <asp:Label ID="LblNombre" runat="server" Text="LblNombre" Width="379px"></asp:Label>
                                        <asp:Label ID="LblVoyageIn" runat="server" Text="LblVoyageIn" Width="379px" Visible="false"></asp:Label>
                                        <asp:Label ID="LblVoyageOut" runat="server" Text="LblVoyageOut" Width="379px" Visible="false"></asp:Label>
                                    </span>
          
                                </td>    
                                <tr>
                                    <td class="bt-bottom  bt-right bt-left" >1. Seleccione un Depósito:</td>
                                    <td class="bt-bottom bt-right" colspan="3"> <asp:DropDownList ID="CboDeposito" runat="server" Width="250px" AutoPostBack="False"
                                                                    Height="28px"  DataTextField='name' DataValueField='id_depot'  
                                                                    Font-Size="Small" 
                                                                    >
                                                                </asp:DropDownList>

        
                                        <span class="validacion" id="xplinea" > * obligatorio</span>
                                    <asp:Button ID="BtnAgregar" runat="server"   OnClick="BtnAgregar_Click" Text="Agregar" Width="100px"/>
                                    </td>       
                                </tr>
                            </tr>
                
                        </table>--%>
                   <%-- </div>--%>
                <%--</div>--%>

                <div class="form-group   col-md-12"> 
                    <label for="inputAddress">Linea Naviera :<span style="color: #FF0000; font-weight: bold;"></span></label>

                    <div class="d-flex">
                        <asp:Label class="form-control" ID="LblNombre" runat="server" Text="LblNombre" ></asp:Label>
                        <asp:Label class="form-control" ID="LblVoyageIn" runat="server" Text="LblVoyageIn"  Visible="false"></asp:Label>
                        <asp:Label class="form-control" ID="LblVoyageOut" runat="server" Text="LblVoyageOut"  Visible="false"></asp:Label>
                    </div>
                     
                </div>

                <div class="form-group col-md-6"> 
                    <label for="inputAddress">Seleccione un Depósito:<span style="color: #FF0000; font-weight: bold;"></span></label>
                    <div class="d-flex">
                        <asp:DropDownList ID="CboDeposito" class="form-control" runat="server" Font-Size="Medium"  AutoPostBack="False" DataTextField='name' DataValueField='id_depot'></asp:DropDownList>
                        <span class="validacion" id="xplinea" > * </span>
                    </div>
                    
                </div>
            </div>

            <div class="row">

                <div class="col-md-12 d-flex justify-content-center">
                   <%-- <asp:Button class="btn btn-primary" Text="Consultar Turnos"  OnClientClick="return fchange()" OnClick="btnconsultarturnos_Click" runat="server" id="btnconsultarturnos"/>
                    <img alt="loading.." src="../shared/imgs/loader.gif" id="imgfecha" class="nover"  />--%>
                    <asp:Button ID="BtnAgregar" class="btn btn-primary" runat="server"   OnClick="BtnAgregar_Click" Text="Agregar" Width="100px"/>
                </div>

            </div>
        </div>

       <%-- <div class="seccion">
            <div class="informativo">
                <table>
                    <tr>
                        <td rowspan="2" class="inum"> 
                            <div class="number">2</div>
                        </td>
                        <td class="level1" >BODEGAS ASIGNADAS</td>
                    </tr>
                    <tr>
                        <td class="level2">
                            Podrá dar de baja a un Depósito, que no tenga movimientos.
                        </td>

                    </tr>
                </table>
            </div>
            <div class="colapser colapsa"></div>
            <div class="accion">
                <table class="controles" cellspacing="0" cellpadding="1">
                    <tr>
                        <th class="bt-bottom bt-right bt-top bt-left" colspan="2"> </th>
                    </tr>

                    <tr>
                        <th colspan="2">
                            <div class="findresult" >
                                <div class="booking" >
                                    <div class="separator">DETALLE DEPOSITOS</div>
                                    <div class="bokindetalle">

                                        <asp:Repeater ID="TableBodegas" runat="server"  onitemcommand="RemoverBodegas_ItemCommand">
                                            <HeaderTemplate>
                                            <table id="tablasort" cellspacing="0" cellpadding="1" class="tabRepeat">
                                            <thead>
                                            <tr>
                                                <th style='width:50px'>ID</th>
                                                <th style='width:250px'>NOMBRE</th>
                                                <th style='width:150px'>ESTADO</th>
                                                <th class ="nover">ID BODEGA</th>
                                                <th >REMOVER</th>
                                                </tr>
                                            </thead> 
                                            <tbody>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                            <tr class="point">
                                            <td ><asp:Label Text='<%#Eval("ID_LINE_DEPO")%>' ID="lbllinea" runat="server"  /></td>
                                            <td ><asp:Label Text='<%#Eval("NAME")%>' ID="lblname" runat="server" /></td>
                                            <td ><asp:Label Text='<%#Eval("estado")%>' ID="lblestado" runat="server" /></td>
                                            <td class ="nover"><asp:Label Text='<%#Eval("ID_DEPOT")%>' ID="lblBodega" runat="server" /></td>
                                            <td class="alinear" style=" width:50px">
                                                <asp:Button ID="BtnConfirmar"  
                                                    OnClientClick="return confirm('Está seguro de que desea remover la bodega?');" 
                                                    runat="server" Text="Remover" CssClass="Anular" ToolTip="Permite remover una bodega" CommandArgument='<%#Eval("ID_LINE_DEPO")%>' />    
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
        </div>--%>

        <div class="form-title">
           2.- Bodegas Asignadas
        </div>

        <h6>Debe seleccionar el depósito, para realizar la asignación a la línea naviera</h6>

        <div >
            <div class="form-row">
                <div class="form-group   col-md-12"> 

                            <div class="findresult" >
                                <div class="booking" >
                                    <%--<div class="separator">DETALLE DEPOSITOS</div>--%>

                                    <div class="form-group col-md-12"> 
                                        <div class="form-title">Detalle Depositos</div>
                                    </div>

                                    <div class="bokindetalle" style=" width:100%; overflow:auto">

                                        <asp:Repeater ID="TableBodegas" runat="server"  onitemcommand="RemoverBodegas_ItemCommand">
                                            <HeaderTemplate>
                                            <table id="tablasort" cellspacing="0" cellpadding="1" class="table table-bordered invoice">
                                            <thead>
                                            <tr>
                                                <th style='width:50px'>ID</th>
                                                <th style='width:250px'>NOMBRE</th>
                                                <th style='width:150px'>ESTADO</th>
                                                <th class ="nover">ID BODEGA</th>
                                                <th >REMOVER</th>
                                                </tr>
                                            </thead> 
                                            <tbody>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                            <tr class="point">
                                            <td ><asp:Label Text='<%#Eval("ID_LINE_DEPO")%>' ID="lbllinea" runat="server"  /></td>
                                            <td ><asp:Label Text='<%#Eval("NAME")%>' ID="lblname" runat="server" /></td>
                                            <td ><asp:Label Text='<%#Eval("estado")%>' ID="lblestado" runat="server" /></td>
                                            <td class ="nover"><asp:Label Text='<%#Eval("ID_DEPOT")%>' ID="lblBodega" runat="server" /></td>
                                            <td class="alinear" style=" width:50px">
                                                <asp:Button ID="BtnConfirmar"  
                                                    OnClientClick="return confirm('Está seguro de que desea remover la bodega?');" 
                                                    runat="server" Text="Remover" CssClass="Anular" ToolTip="Permite remover una bodega" CommandArgument='<%#Eval("ID_LINE_DEPO")%>' />    
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
            </div>
        </div>

       <div class="form-row">
        <%--<div class="form-group   col-md-12"> --%>
            <%--<div class="informativo">
                <table>
     
      
                </table>
            </div>--%>

             <div class="row">

                <div class="col-md-12 d-flex justify-content-center" id ="ADU">
                    <img alt="loading.." src="../shared/imgs/loader.gif" id="loader" class="nover"  />
                    &nbsp;
                    &nbsp;
                </div>
    
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