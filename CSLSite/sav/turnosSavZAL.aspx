<%@ Page Title="Administrar Turnos SAV" Language="C#" MasterPageFile="~/site.Master" AutoEventWireup="true" CodeBehind="turnosSavZAL.aspx.cs" Inherits="CSLSite.sav.turnosSavZAL" %>
<%@ MasterType VirtualPath="~/site.Master" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="placehead" runat="server">
    <link href="../shared/estilo/tables.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/aisv-general.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/work.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/catalogos.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/info.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/proforma.css" rel="stylesheet" type="text/css" />

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

<asp:Content ID="Content2" ContentPlaceHolderID="placebody" runat="server">
    <asp:ToolkitScriptManager ID="tkcal" EnableScriptGlobalization="true" runat="server"> </asp:ToolkitScriptManager>

     <div class="mt-4">         
        <nav class="mt-4" aria-label="breadcrumb">
            <ol class="breadcrumb">
            <li class="breadcrumb-item" id="opcion_principal" runat="server"><a href="#">Servicio de Administración de Vacíos</a></li>
                <li class="breadcrumb-item active" aria-current="page" id="sub_opcion" runat="server">Administración de turnos y horarios</li>
            </ol>
        </nav>
    </div>

    <div class="dashboard-container p-4" id="cuerpo" runat="server">

        <div class="form-title">
           1.- Datos de Busqueda
        </div>
        <h6>Debe seleccionar un depósito de la lista para filtar los turnos de la misma.</h6>

        <div >
            <div class="form-row" >
                <asp:TextBox Visible="false" ID ="txtUsuario" runat="server"></asp:TextBox>
                <div class="form-group   col-md-3"> 
                    <label for="inputAddress">1. Seleccione el Depósito :<span style="color: #FF0000; font-weight: bold;"></span></label>

                    <div class="d-flex">
                        <asp:DropDownList class="form-control" ID="cmbDepositoList" runat="server" Enabled="false" Font-Size="Medium" AutoPostBack="True" Font-Bold="true" OnSelectedIndexChanged="cmbDeposito_SelectedIndexChanged"></asp:DropDownList>
                    </div>
                </div>

            </div>
        </div>

    
        <div class="form-title">
           2.- Datos de Turnos SAV
        </div>
        <h6>En esta sección puede agregar, modificar y visualizar los turnos del deposito seleccionado.</h6>


        <div >
            <div class="form-row" >

                <div class="form-group   col-md-3"> 
                    <label for="inputAddress">Depósito:<span style="color: #FF0000; font-weight: bold;"></span></label>

                    <div class="d-flex">
                        <asp:DropDownList class="form-control" ID="cmbDeposito" runat="server" Enabled="false" Font-Size="Medium" AutoPostBack="true" Font-Bold="true"></asp:DropDownList>
                    </div>
                </div>

                <div class="form-group   col-md-3"> 
                    <label for="inputAddress">Dia:<span style="color: #FF0000; font-weight: bold;"></span></label>

                    <div class="d-flex">
                        <asp:DropDownList class="form-control" ID="ddlDiaCab" runat="server" 
                            SelectedValue='<%# Bind("DIA") %>' >
                            <asp:ListItem Value="1">DOMINGO</asp:ListItem>
                            <asp:ListItem Value="2">LUNES</asp:ListItem>
                            <asp:ListItem Value="3">MARTES</asp:ListItem>
                            <asp:ListItem Value="4">MIERCOLES</asp:ListItem>
                            <asp:ListItem Value="5">JUEVES</asp:ListItem>
                            <asp:ListItem Value="6">VIERNES</asp:ListItem>
                            <asp:ListItem Value="7">SABADO</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>

                <div class="form-group   col-md-3"> 
                    <label for="inputAddress">Hora/Minuto:<span style="color: #FF0000; font-weight: bold;"></span></label>

                    <div class="d-flex">
                        <asp:DropDownList class="form-control" ID="ddlHora" runat="server" 
                                SelectedValue='<%# Bind("DIA") %>'>
                                <asp:ListItem Value="00">00</asp:ListItem>
                                <asp:ListItem Value="01">01</asp:ListItem>
                                <asp:ListItem Value="02">02</asp:ListItem>
                                <asp:ListItem Value="03">03</asp:ListItem>
                                <asp:ListItem Value="04">04</asp:ListItem>
                                <asp:ListItem Value="05">05</asp:ListItem>
                                <asp:ListItem Value="06">06</asp:ListItem>
                                <asp:ListItem Value="07">07</asp:ListItem>
                                <asp:ListItem Value="08">08</asp:ListItem>
                                <asp:ListItem Value="09">09</asp:ListItem>
                                <asp:ListItem Value="10">10</asp:ListItem>
                                <asp:ListItem Value="11">11</asp:ListItem>
                                <asp:ListItem Value="12">12</asp:ListItem>
                                <asp:ListItem Value="13">13</asp:ListItem>
                                <asp:ListItem Value="14">14</asp:ListItem>
                                <asp:ListItem Value="15">15</asp:ListItem>
                                <asp:ListItem Value="16">16</asp:ListItem>
                                <asp:ListItem Value="17">17</asp:ListItem>
                                <asp:ListItem Value="18">18</asp:ListItem>
                                <asp:ListItem Value="19">19</asp:ListItem>
                                <asp:ListItem Value="20">20</asp:ListItem>
                                <asp:ListItem Value="21">21</asp:ListItem>
                                <asp:ListItem Value="22">22</asp:ListItem>
                                <asp:ListItem Value="23">23</asp:ListItem>
                                <asp:ListItem Value="24">24</asp:ListItem>
                            </asp:DropDownList>
                        <asp:DropDownList class="form-control" ID="ddlMinuto" runat="server" 
                            SelectedValue='<%# Bind("DIA") %>'>
                            <asp:ListItem Value="00">00</asp:ListItem>
                            <asp:ListItem Value="01">01</asp:ListItem>
                            <asp:ListItem Value="02">02</asp:ListItem>
                            <asp:ListItem Value="03">03</asp:ListItem>
                            <asp:ListItem Value="04">04</asp:ListItem>
                            <asp:ListItem Value="05">05</asp:ListItem>
                            <asp:ListItem Value="06">06</asp:ListItem>
                            <asp:ListItem Value="07">07</asp:ListItem>
                            <asp:ListItem Value="08">08</asp:ListItem>
                            <asp:ListItem Value="09">09</asp:ListItem>
                            <asp:ListItem Value="10">10</asp:ListItem>
                            <asp:ListItem Value="11">11</asp:ListItem>
                            <asp:ListItem Value="12">12</asp:ListItem>
                            <asp:ListItem Value="13">13</asp:ListItem>
                            <asp:ListItem Value="14">14</asp:ListItem>
                            <asp:ListItem Value="15">15</asp:ListItem>
                            <asp:ListItem Value="16">16</asp:ListItem>
                            <asp:ListItem Value="17">17</asp:ListItem>
                            <asp:ListItem Value="18">18</asp:ListItem>
                            <asp:ListItem Value="19">19</asp:ListItem>
                            <asp:ListItem Value="20">20</asp:ListItem>
                            <asp:ListItem Value="21">21</asp:ListItem>
                            <asp:ListItem Value="22">22</asp:ListItem>
                            <asp:ListItem Value="23">23</asp:ListItem>
                            <asp:ListItem Value="24">24</asp:ListItem>
                            <asp:ListItem Value="25">25</asp:ListItem>
                            <asp:ListItem Value="26">26</asp:ListItem>
                            <asp:ListItem Value="27">27</asp:ListItem>
                            <asp:ListItem Value="28">28</asp:ListItem>
                            <asp:ListItem Value="29">29</asp:ListItem>
                            <asp:ListItem Value="30">30</asp:ListItem>
                            <asp:ListItem Value="31">31</asp:ListItem>
                            <asp:ListItem Value="32">32</asp:ListItem>
                            <asp:ListItem Value="33">33</asp:ListItem>
                            <asp:ListItem Value="34">34</asp:ListItem>
                            <asp:ListItem Value="35">35</asp:ListItem>
                            <asp:ListItem Value="36">36</asp:ListItem>
                            <asp:ListItem Value="36">37</asp:ListItem>
                            <asp:ListItem Value="38">38</asp:ListItem>
                            <asp:ListItem Value="39">39</asp:ListItem>
                            <asp:ListItem Value="40">40</asp:ListItem>
                            <asp:ListItem Value="41">41</asp:ListItem>
                            <asp:ListItem Value="42">42</asp:ListItem>
                            <asp:ListItem Value="43">43</asp:ListItem>
                            <asp:ListItem Value="44">44</asp:ListItem>
                            <asp:ListItem Value="45">45</asp:ListItem>
                            <asp:ListItem Value="46">46</asp:ListItem>
                            <asp:ListItem Value="47">47</asp:ListItem>
                            <asp:ListItem Value="48">48</asp:ListItem>
                            <asp:ListItem Value="49">49</asp:ListItem>
                            <asp:ListItem Value="50">50</asp:ListItem>                    
                            <asp:ListItem Value="51">51</asp:ListItem>
                            <asp:ListItem Value="52">52</asp:ListItem>
                            <asp:ListItem Value="53">53</asp:ListItem>
                            <asp:ListItem Value="54">54</asp:ListItem>
                            <asp:ListItem Value="55">55</asp:ListItem>
                            <asp:ListItem Value="56">56</asp:ListItem>
                            <asp:ListItem Value="57">57</asp:ListItem>
                            <asp:ListItem Value="58">58</asp:ListItem>
                            <asp:ListItem Value="59">59</asp:ListItem>
                            <asp:ListItem Value="60">60</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>

                <div class="form-group   col-md-3"> 
                    <label for="inputAddress">Cantidad disponible:<span style="color: #FF0000; font-weight: bold;"></span></label>

                    <div class="d-flex">
                        <asp:TextBox class="form-control" ID="txtCantDisp" runat="server"  MaxLength="2" onkeypress="return soloLetras(event,'1234567890:')">
                        </asp:TextBox>
                    </div>
                </div>

            </div>
        </div>
        
        <div class="form-group col-md-12"> 
            <div class="cataresult" >
                <asp:UpdatePanel ID="UPPrincipal" runat="server"  UpdateMode="Conditional"  ChildrenAsTriggers="true">
                    <ContentTemplate>
                       <table style=" display:none" cellpadding="2" cellspacing="2" class="table table-bordered invoice" width="100%">
                            <tr>
                                <td align="left">
                                <fieldset>  
                                <asp:DetailsView ID="DetailsView1" runat="server" AutoGenerateRows="False" 
                                DataKeyNames="ID_HORARIO" DataSourceID="SqlDataSource1" DefaultMode="Insert" 
                                CellPadding="4" ForeColor="#333333" 
                                GridLines="None" oniteminserting="DetailsView1_ItemInserting">
                                <AlternatingRowStyle BackColor="White" />
                                <CommandRowStyle BackColor="#D1DDF1" Font-Bold="True" />
                                <EditRowStyle BackColor="#2461BF" />
                                <FieldHeaderStyle BackColor="#DEE8F5" Font-Bold="True" />
                                <Fields>
                                    <asp:TemplateField HeaderText="DIA:" SortExpression="DIA">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="TextBox1" runat="server" class="form-control" ></asp:TextBox>
                                        </EditItemTemplate>
                                        <InsertItemTemplate>
                                            <asp:DropDownList class="form-control" ID="DropDownList1" runat="server" 
                                                SelectedValue='<%# Bind("DIA") %>'  >
                                                <asp:ListItem Value="1">DOMINGO</asp:ListItem>
                                                <asp:ListItem Value="2">LUNES</asp:ListItem>
                                                <asp:ListItem Value="3">MARTES</asp:ListItem>
                                                <asp:ListItem Value="4">MIERCOLES</asp:ListItem>
                                                <asp:ListItem Value="5">JUEVES</asp:ListItem>
                                                <asp:ListItem Value="6">VIERNES</asp:ListItem>
                                                <asp:ListItem Value="7">SABADO</asp:ListItem>
                                            </asp:DropDownList>
                                        </InsertItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label class="form-control" ID="Label1" runat="server" ></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="HORA:" SortExpression="HORA">
                                        <EditItemTemplate>
                                            <asp:TextBox class="form-control" ID="TextBox2" runat="server" Text='<%# Bind("HORA") %>'></asp:TextBox>
                                        </EditItemTemplate>
                                        <InsertItemTemplate>
                                            <asp:TextBox ID="TextBox1" runat="server" MaxLength="5"  onkeypress="return soloLetras(event,'1234567890:')"
                                                Text='<%# Bind("HORA") %>' class="form-control"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rq01" runat="server" 
                                                ControlToValidate="TextBox1" Display="Dynamic" ErrorMessage="* Requerido" 
                                                CssClass="validacion" ValidationGroup="cabecera"></asp:RequiredFieldValidator>
                                        </InsertItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label class="form-control" ID ="Label2" runat="server" Text='<%# Bind("HORA") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="UNIDADES DISPONIBLES:" SortExpression="CNTR_DISP">
                                        <EditItemTemplate>
                                            <asp:TextBox class="form-control" ID="TextBox3" runat="server" Text='<%# Bind("CNTR_DISP") %>' onkeypress="return soloLetras(event,'1234567890:')"></asp:TextBox>
                                        </EditItemTemplate>
                                        <InsertItemTemplate>
                                            <asp:TextBox ID="TextBox2" runat="server" class="form-control" MaxLength="2" onkeypress="return soloLetras(event,'1234567890:')"
                                                Text='<%# Bind("CNTR_DISP") %>'></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rq2" runat="server" 
                                                ControlToValidate="TextBox2" Display="Dynamic" ErrorMessage="* Requerido" 
                                                 CssClass="validacion" ValidationGroup="cabecera"></asp:RequiredFieldValidator>
                                        </InsertItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label class="form-control" ID="Label3" runat="server" Text='<%# Bind("CNTR_DISP") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="ID_HORARIO" HeaderText="ID_HORARIO" 
                                        SortExpression="ID_HORARIO" InsertVisible="False" ReadOnly="True" />
                                    <asp:TemplateField ShowHeader="False">
                                        <ItemTemplate>
                                            <asp:Button  class="btn btn-primary" ID="btnNuevo" runat="server" CausesValidation="False" 
                                                Text="Nuevo" onclick="btnNuevo_Click" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Fields>
                                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                <RowStyle BackColor="#EFF3FB" />
                            </asp:DetailsView>
                                </fieldset>    
                                </td>
                            </tr>
                       </table>
              
                        <div><br /></div>

                        <table cellpadding="2" cellspacing="2" class="tabla_controles" width="100%">
                            <tr>
                                <td align="center">
                                    <asp:Button  class="btn btn-primary" Text="Guardar" runat="server" id="btnIngresarCab" 
                                        onclick="btnIngresarCab_Click"/>
                                    <asp:Button  class="btn btn-outline-primary mr-4" Text="Cancelar" runat="server" id="btnCancelarCab" 
                                        onclick="btnCancelarCab_Click"/>
                                </td>
                            </tr>
                        </table>
                        
                        <div><br /></div>

                        <%--<tr><th colspan="2">--%>
                                <div class="findresult" >
                                    <div class="booking" >
                                    
                                        <div class="form-group col-md-12"> 
                                            <div class="form-title">DETALLE HORARIOS</div>
                                        </div>

                                        <div class="bokindetalle" style=" width:100%; overflow:auto">

                                            <table cellpadding="2" cellspacing="2" class="table table-bordered invoice"  width="100%">
                                            <tr>
                                                <td align="center">
                                                <asp:Panel ID="Panel1" runat="server" CssClass="center">
                                                <fieldset>
                                                    <asp:GridView ID="GridView1"  runat="server" AllowPaging="True" 
                                                    AllowSorting="True" AutoGenerateColumns="False" 
                                                    DataSourceID="SqlDataSource1" DataKeyNames="ID_HORARIO" CellPadding="4" 
                                                    EmptyDataText="No hay datos" ForeColor="#333333" GridLines="None" 
                                                    onrowcommand="GridView1_RowCommand" 
                                                        onrowupdating="GridView1_RowUpdating"
                                                         CssClass="table table-bordered invoice">

                                                    <PagerStyle HorizontalAlign = "Right" CssClass="pagination-ys"  />
                                                    <RowStyle  BackColor="#F0F0F0" Font-Size="Small" />
                                                    <%--<AlternatingRowStyle BackColor="White" />--%>
                                                        <alternatingrowstyle  BackColor="#FFFFFF" Font-Size="Small" />
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="DIA" SortExpression="DIA" HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone"   >
                                                            <EditItemTemplate>
                                                                
                                                                    <asp:DropDownList  class="form-control" ID="DropDownList1" runat="server" 
                                                                        SelectedValue='<%# Bind("DIA") %>' >
                                                                        <asp:ListItem Value="1">DOMINGO</asp:ListItem>
                                                                        <asp:ListItem Value="2">LUNES</asp:ListItem>
                                                                        <asp:ListItem Value="3">MARTES</asp:ListItem>
                                                                        <asp:ListItem Value="4">MIERCOLES</asp:ListItem>
                                                                        <asp:ListItem Value="5">JUEVES</asp:ListItem>
                                                                        <asp:ListItem Value="6">VIERNES</asp:ListItem>
                                                                        <asp:ListItem Value="7">SABADO</asp:ListItem>
                                                                    </asp:DropDownList>
                                                                  
                                                            </EditItemTemplate>
                                                            <ItemTemplate>
                                                                
                                                                    <asp:DropDownList  class="form-control" ID="DropDownList1" runat="server" 
                                                                        SelectedValue='<%# Bind("DIA") %>'  Enabled="False">
                                                                        <asp:ListItem Value="1">DOMINGO</asp:ListItem>
                                                                        <asp:ListItem Value="2">LUNES</asp:ListItem>
                                                                        <asp:ListItem Value="3">MARTES</asp:ListItem>
                                                                        <asp:ListItem Value="4">MIERCOLES</asp:ListItem>
                                                                        <asp:ListItem Value="5">JUEVES</asp:ListItem>
                                                                        <asp:ListItem Value="6">VIERNES</asp:ListItem>
                                                                        <asp:ListItem Value="7">SABADO</asp:ListItem>
                                                                    </asp:DropDownList>
                                                                
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="HORA" SortExpression="HORA" HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone" >
                                                            <ItemTemplate>
                                                                <asp:Label class="form-control" ID="Label1" runat="server" Text='<%# Bind("HORA") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <EditItemTemplate>
                                                                <asp:TextBox  class="form-control" ID="TextBox1" onkeypress="return soloLetras(event,'1234567890:')" runat="server" Text='<%# Bind("HORA") %>'></asp:TextBox>
                                                            </EditItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="UNIDADES" SortExpression="CNTR_DISP" HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone" >
                                                            <ItemTemplate>
                                                                <asp:Label class="form-control" ID="Label2" runat="server" Text='<%# Bind("CNTR_DISP") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <EditItemTemplate>
                                                                <asp:TextBox  class="form-control" ID="TextBox2" onkeypress="return soloLetras(event,'1234567890')" runat="server" Text='<%# Bind("CNTR_DISP") %>'></asp:TextBox>
                                                            </EditItemTemplate>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderText="DEPOSITO" SortExpression="DEPOSITO" HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone" >
                                                            <ItemTemplate>
                                                                <asp:Label class="form-control" ID="Label6" runat="server" Text='<%# Bind("DEPOSITO_DESC") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderText="ID_DEPOSITO" SortExpression="ID_DEPOSITO" InsertVisible="False" ShowHeader="False" Visible="False" HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone" ItemStyle-Width="100px">
                                                            <ItemTemplate>
                                                                <asp:Label class="form-control" ID="Label7" runat="server" Text='<%# Bind("DEPOSITO") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>

                                                        <asp:BoundField DataField="ID_HORARIO" HeaderText="ID_HORARIO" 
                                                            SortExpression="ID_HORARIO" InsertVisible="False" ReadOnly="True" 
                                                            ShowHeader="False" Visible="False" HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone" ItemStyle-Width="100px"/>
                                                        <asp:TemplateField ShowHeader="False" HeaderStyle-CssClass="center hidden-phone" ItemStyle-CssClass="gradeC center hidden-phone" ItemStyle-Width="100px">
                                                            <EditItemTemplate>
                                                                <div class="d-flex">
                                                                    <asp:Button class="btn btn-primary" ID="Button1" runat="server" CausesValidation="True"  CommandName="Update" Text="Guardar"   />
                                                                    &nbsp;&nbsp;&nbsp;
                                                                    <asp:Button class="btn btn-outline-primary mr-4" ID="Button2" runat="server" CausesValidation="False" CommandName="Cancel" Text="Cancelar"   />
                                                                </div>
                                                            </EditItemTemplate>
                                                            <ItemTemplate>
                                                                <asp:Button class="btn btn-primary" ID="Button3" runat="server" CausesValidation="False"  
                                                                    CommandName="Edit" Text="Editar" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:CommandField ShowSelectButton="true" ButtonType="Button" ControlStyle-CssClass="btn btn-outline-primary mr-4"  SelectText="Inactivar">
                                                        <ControlStyle  />
                                                        </asp:CommandField>
                                                    </Columns>
                                                    <EditRowStyle BackColor="#8C8C8C" /><%--7C6F57--%>
                                                    <EmptyDataTemplate>
                                                        No hay datos
                                                    </EmptyDataTemplate>
                                           
                                                </asp:GridView>
                                                <hr />
                                                <p id="pError" class="pError" runat="server"> </p>
                                                </fieldset>  
                                                <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
                                                    ConnectionString="<%$ ConnectionStrings:midle %>"
                                                    SelectCommand="SELECT A.[HORA], A.[CNTR_DISP], A.[DIA], A.[ID_HORARIO], A.ESTADO,B.descripcion  AS DEPOSITO_DESC , B.id AS DEPOSITO FROM [SAV_HORARIOS] A JOIN ZAL_DEPOT B ON (A.DEPOSITO = B.ID ) WHERE (A.ESTADO = 'A') AND B.id = @ParameterDeposito ORDER BY A.DIA, A.HORA, A.DEPOSITO"
                                                    ConflictDetection="CompareAllValues"
                                                    DeleteCommand=""
                                                    OldValuesParameterFormatString="original_{0}"
                                                    UpdateCommand="UPDATE [SAV_HORARIOS]  
                                                                        SET HORA = @HORA
                                                                        , CNTR_DISP = @CNTR_DISP
                                                                        , DIA = @DIA
                                                                        , fechaModifica = getdate()
                                                                        , usuarioModifica = @ParameterUsuario
                                                                        , DESCRIPCION = CASE @DIA  
                                                                                            WHEN 1 THEN 'DOMINGO'
                                                                                            WHEN 2 THEN 'LUNES'
                                                                                            WHEN 3 THEN 'MARTES'
                                                                                            WHEN 4 THEN 'MIERCOLES'
                                                                                            WHEN 5 THEN 'JUEVES'
                                                                                            WHEN 6 THEN 'VIERNES'
                                                                                        ELSE
                                                                                            'SABADO'
                                                                                        END
                                                                    WHERE [ID_HORARIO] = @original_ID_HORARIO " 
                                                    onupdated="SqlDataSource1_Updated" 
                                                    oninserted="SqlDataSource1_Inserted" OnSelecting="SqlDataSource1_Selecting">
                                                    <DeleteParameters>
                                                        <asp:Parameter Name="original_ID_HORARIO" Type="Int64" />
                                                        <asp:Parameter Name="original_HORA" Type="String" />
                                                        <asp:Parameter Name="original_CNTR_DISP" Type="Byte" />
                                                        <asp:Parameter Name="original_DIA" Type="String" />
                                                    </DeleteParameters>
                                                    <UpdateParameters>
                                                        <asp:Parameter Name="HORA" Type="String" />
                                                        <asp:Parameter Name="CNTR_DISP" Type="Byte" />
                                                        <asp:Parameter Name="DIA" Type="String" />
                                                        <asp:Parameter Name="original_ID_HORARIO" Type="Int64" />
                                                        <asp:Parameter Name="original_HORA" Type="String" />
                                                        <asp:Parameter Name="original_CNTR_DISP" Type="Byte" />
                                                        <asp:Parameter Name="original_DIA" Type="String" />
                                                        <asp:Parameter Name="ParameterUsuario" Type="String" />
                                                    </UpdateParameters>
                                                    <SelectParameters>
                                                        <asp:Parameter DefaultValue="5" Name="ParameterDeposito" Type="String" />
                                                    </SelectParameters>
                                                </asp:SqlDataSource>
                                                </asp:Panel>
                                                </td>
                                            </tr>
                                            <tr>
                                            <td align="center">
                                                <asp:Button class="btn btn-outline-primary mr-4"  Text="Refrescar" ID="btnConsultar" runat="server" 
                                                                 onclick="btnConsultar_Click"/>
                                            </td>
                                            </tr>
                                            </table>

                                        </div>
                                    </div>
                                </div>
                            <%--</th>
                        </tr>--%>

                        <table cellpadding="2" cellspacing="2" class="tabla_controles" width="100%">
                            <tr>
                                <td class="" align="center" style=" width:350px">
                                    <asp:HyperLink ID="hplHorario" runat="server" Style="visibility: hidden">HyperLink</asp:HyperLink>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Panel ID="plnHorario" style=" display:none" runat="server"  Width="350px">
                                        <asp:UpdatePanel ID="UPHORARIO"  runat="server">
                                            <ContentTemplate>
                                                <span class="t3" style=" font-weight:bold; width:350px">Ya existe un Horario activo, desea Activarlo?</span>
                                                <table cellpadding="0" cellspacing="0" class="tabla_controles" style="border:2px solid Black; width:350px; background-color:White">
                                                    <tr>
                                                        <td align="center">
                                                            <asp:Button class="btn btn-primary" Text="Aceptar" ID="btnAceptar_" runat="server" 
                                                                 onclick="btnAceptar__Click"/>
                                                        </td>
                                                        <td align="center">
                                                            <asp:Button class="btn btn-outline-primary mr-4" Text="Cancelar" ID="btnCancelar_" runat="server"
                                                                 onclick="btnCancelar__Click"/>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </asp:Panel>
                                </td>
                            </tr>
                        </table>

                        <script type="text/javascript">
                            function soloLetras(e, caracteres) {
                                key = e.keyCode || e.which;
                                tecla = String.fromCharCode(key).toLowerCase();
                                if (caracteres) {
                                    letras = caracteres;
                                }
                                else {
                                    letras = " áéíóúabcdefghijklmnñopqrstuvwxyz";
                                }


                                especiales = [8, 37, 39, 46, 13, 9];

                                tecla_especial = false
                                for (var i in especiales) {
                                    if (key == especiales[i]) {
                                        tecla_especial = true;
                                        break;
                                    }
                                }

                                if (letras.indexOf(tecla) == -1 && !tecla_especial) {
                                    return false;
                                }
                            }

                        </script>

                        <asp:ModalPopupExtender ID="HORARIO" runat="server" 
                        BackgroundCssClass="modalBackground" DropShadow="true" 
                        PopupControlID="plnHorario" PopupDragHandleControlID="plnHorario" 
                        TargetControlID="hplHorario" 
                            />
                     </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>

</asp:Content>
