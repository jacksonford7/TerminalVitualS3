<%@ Page  Title="Inventario"  Language="C#" MasterPageFile="~/site.Master"
    AutoEventWireup="true" CodeBehind="stock_register.aspx.cs" Inherits="CSLSite.stock_register"  %>



<asp:Content ID="Content1" ContentPlaceHolderID="placehead" runat="server">
    
       
    <link href="../shared/estilo/tables.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/aisv-general.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/work.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/catalogos.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/info.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/opc_tables.css" rel="stylesheet" />

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
    <asp:ScriptManager ID="Validacion1" runat="server" />

    <input id="zonaid" type="hidden" value="205" />
    <noscript><meta http-equiv="refresh" content="0; url=sinsoporte.htm" /> </noscript>
   
<%--     <div>
        
         <i class="ico-titulo-2"></i><h1>Distribución de Inventario para Depósitos</h1><br />
    </div>--%>

    <div class="mt-4">         
        <nav class="mt-4" aria-label="breadcrumb">
            <ol class="breadcrumb">
            <li class="breadcrumb-item" id="opcion_principal" runat="server"><a href="#">Depósito de Vacíos</a></li>
                <li class="breadcrumb-item active" aria-current="page" id="sub_opcion" runat="server">Distribución de Inventario para Depósitos</li>
            </ol>
        </nav>
    </div>


    <div class="alert alert-warning">
        <span id="dtlo" runat="server">Estimado usuario:</span> 
        <br /> ...
    </div>
     
    <div class="dashboard-container p-4" id="cuerpo" runat="server">

        <div class="form-title">
            1.- Datos de la Transacción
        </div>

        <h6>Debe ingresar la fecha, la cantidad, seleccionar el depósito, seleccionar el tipo de operación (+/-)</h6>

        <div >
            <div class="form-row" >

                 <div class="form-group   col-md-6"> 
                    <label for="inputAddress">Linea Naviera:<span style="color: #FF0000; font-weight: bold;"></span></label>

                    <div class="d-flex">
                        <asp:Label ID="LblNombre" class="form-control" runat="server" Text="LblNombre"></asp:Label></span>
                    </div>
                </div>

                 <div class="form-group   col-md-6"> 
                    <label for="inputAddress">1. Fecha Transacción:<span style="color: #FF0000; font-weight: bold;"></span></label>

                    <div class="d-flex">
                        <asp:TextBox class="form-control"  ID="TxtFechaTransaccion" runat="server" MaxLength="16" CssClass="datetimepicker form-control"
                        onkeypress="return soloLetras(event,'1234567890/:')"
                        onBlur="valDate(this,  true,valdatem);"
                        placeholder="dd/mm/aaaa hh:mm "               
                        onchange="defaultDate(this);"></asp:TextBox>
                        <span class="validacion" id="valfechacita"  > * </span>
                    </div>
                </div>

                 <div class="form-group   col-md-6"> 
                    <label for="inputAddress">2. Seleccione un Depósito:<span style="color: #FF0000; font-weight: bold;"></span></label>

                    <div class="d-flex">
                        <asp:DropDownList ID="CboDeposito" class="form-control" runat="server" AutoPostBack="True"
                                                    DataTextField='name' DataValueField='id_depot'  
                                                    Font-Size="Medium" OnSelectedIndexChanged="CboDeposito_SelectedIndexChanged">
                        </asp:DropDownList>
                        
                        <span class="validacion" id="xplinea" > * </span>
                    </div>
                </div>

                 <div class="form-group   col-md-3"> 
                    <label for="inputAddress">3. Cantidad:<span style="color: #FF0000; font-weight: bold;"></span></label>

                    <div class="d-flex">
                        <asp:TextBox class="form-control" ID="TxtCantidad" runat="server"  MaxLength="5"
                         onkeypress="return soloLetras(event,'1234567890',false)" onpaste="return false;"></asp:TextBox>
                        <span class="validacion" id="valhoras"  > * </span>
                    </div>
                </div>

                 <div class="form-group   col-md-3"> 
                    <label for="inputAddress">4. Seleccione la operación:<span style="color: #FF0000; font-weight: bold;"></span></label>

                    <div class="d-flex">
                        <asp:DropDownList class="form-control" ID="CboOperacion" runat="server" AutoPostBack="False"
                            DataTextField='notes' DataValueField='id_operation'  
                            Font-Size="Small">
                        </asp:DropDownList>

                        <span class="validacion" id="xplinea1" > * </span>
                    </div>
                </div>
            <%--    <table class="controles" cellspacing="0" cellpadding="1">
                <tr><th class="bt-bottom bt-right bt-top bt-left" colspan="4"></th></tr>
                    <tr><td class="bt-bottom  bt-right bt-left">LINEA NAVIERA:</td>
                        <td class="bt-bottom bt-right"> <span id="nombre" class="caja cajafull">
                            
                        </td>   

                    </tr>
                    <tr>
                    <td class="bt-bottom  bt-right bt-left" >1. Fecha Transacción:</td>
                    <td class="bt-bottom bt-right" colspan="3">
                    
                    </td>       
                    </tr>
                    <tr>
                    <td class="bt-bottom  bt-right bt-left" >2. Seleccione un Depósito:</td>
                    <td class="bt-bottom bt-right" colspan="3">
         
                        
        
                    </td>       
                    </tr>
                    <tr>
                    <td class="auto-style6" >3. Cantidad</td>
                    <td class="bt-bottom bt-right">
                    </td>            
                    </tr>
                    <tr>
                    <td class="bt-bottom  bt-right bt-left" >4. Seleccione la operación:</td>
                    <td class="bt-bottom bt-right" colspan="3"> 
                            
      
                    </td>       
                    </tr>
        

                </table>
                <div class="botonera" style="align-content:center">
                    <asp:Button ID="BtnAgregar" runat="server"   OnClick="BtnAgregar_Click" Text="Agregar" Width="100px"/>
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                </div>--%>
            </div>
   
            <div class="row">

                <div class="col-md-12 d-flex justify-content-center">
                    <%--<asp:Button class="btn btn-primary" Text="Consultar Turnos"  OnClientClick="return fchange()" OnClick="btnconsultarturnos_Click" runat="server" id="btnconsultarturnos"/>--%>
                    <%--<img alt="loading.." src="../shared/imgs/loader.gif" id="imgfecha" class="nover"  />--%>
                    <asp:Button class="btn btn-primary" ID="BtnAgregar" runat="server"   OnClick="BtnAgregar_Click" Text="Agregar" />
                </div>

            </div>
             
        </div>
   
        <div class="form-title">
            2.- Detalle de Movimientos
        </div>

        <h6>Podrá eliminar un movimiento, dando click en el botón REMOVER.</h6>




        <div >

            <div class="form-row" >
                <div class="form-group col-md-12"> 
                    <div class="cataresult" >
          

                        <div class="findresult" >
                            <div class="booking" >
        
                                    <div class="form-group col-md-12"> 
                                        <div class="form-title">DETALLE DEPOSITOS</div>
                                    </div>
                                   <div class="bokindetalle" style=" width:100%; overflow:auto">

                                        <asp:Repeater ID="TableStock" runat="server"   OnItemDataBound="Opciones_ItemDataBound">
                                            <HeaderTemplate>
                                            <table id="tablasort" cellspacing="0" cellpadding="1" class="table table-bordered invoice">
                     
                                            <thead>
                                            <tr>
                                            <th style='width:20px'>SEMANA</th>
                                            <th style='width:110px'>FECHA</th>
                                            <th style='width:150px'>DEPOSITO</th>
                                            <th style='width:250px'>TIPO TRANSACCION</th>
                                            <th style='width:70px'>(+)</th>
                                            <th style='width:70px'>(-)</th>
                                            <th style='width:70px'>STOCK</th>
                   
                                            </tr>
                                            </thead> 
                                            <tbody>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                            <tr class="point">
                                            <td  style='width:40px'><asp:Label Text='<%#Eval("CCREATE_WEEK")%>' ID="Label1" runat="server"   /></td>
                                            <td style='width:100px'><asp:Label Text='<%#Eval("CCREATE_DATE")%>' ID="lblFecha" runat="server"  /></td>
                                            <td ><asp:Label Text='<%#Eval("NAME")%>' ID="lblname" runat="server" /></td>
                                            <td ><asp:Label Text='<%#Eval("NOTES")%>' ID="lblNotes" runat="server" /></td>
                                            <td ><asp:Label Text='<%#Eval("CCANT_ING")%>' ID="lblIngreso" runat="server" /></td>
                                            <td ><asp:Label Text='<%#Eval("CCANT_EGR")%>' ID="lblEgreso" runat="server" /></td>
                                            <td ><asp:Label Text='<%#Eval("CTOTAL")%>' ID="lbltotal" runat="server" /></td>
               
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


        </div>
 
      

<%--    <div class="form-row">--%>
        <%--<div class="form-group   col-md-12"> --%>
            <%--<div class="informativo">
                <table>
     
      
                </table>
            </div>--%>

             <div class="row">

                   <div class="col-md-12 d-flex justify-content-center" id ="ADU">
                      <%--<div class="botonera">--%>
                         <img alt="loading.." src="../shared/imgs/loader.gif" id="loader" class="nover"  />
                          &nbsp;
                          &nbsp;
                     <%--</div>--%>
                 </div>
    
            </div>
   <%--     </div>--%>
    
    </div>


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


    <script type="text/javascript" src="../lib/jquery/jquery-3.5.1.slim.min.js" integrity="sha384-DfXdz2htPH0lsSSs5nCTpuj/zy4C+OGpamoFVy38MVBnE+IbbVYUew+OrCXaRkfj" crossorigin="anonymous"></script>
    <script src="../Scripts/jquery.datetimepicker.js" type="text/javascript"></script>    
    <script type="text/javascript">
        $(document).ready(function () {
            $('.datetimepicker').datetimepicker().datetimepicker({ lang: 'es', timepicker: false, closeOnDateSelect: true, format: 'd/m/Y' });
        });
    </script>


      <script src="../Scripts/pages.js" type="text/javascript"></script>
    <script src="../Scripts/opc_control.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.tablesorter.min.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.tablesorter.pager.js" type="text/javascript"></script>
</asp:Content>