<%@ Page Title="Usuarios STC" Language="C#" MasterPageFile="~/site.Master" AutoEventWireup="true" CodeBehind="plus_usuario.aspx.cs" Inherits="CSLSite.nuevo_sna" %>
<%@ MasterType VirtualPath="~/site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="placehead" runat="server">
    <link href="../shared/estilo/tables.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/aisv-general.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/work.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/catalogos.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/info.css" rel="stylesheet" type="text/css" />
     <!--mensajes-->
        <link href="https://cdn.jsdelivr.net/npm/alertifyjs@1.11.0/build/css/alertify.min.css" rel="stylesheet"/>
        <script type="text/javascript" src="https://cdn.jsdelivr.net/npm/alertifyjs@1.11.0/build/alertify.min.js"></script>
    
     
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="placebody" runat="server">
            


     <input id="bandera"     type="hidden"   runat="server" clientidmode="Static"  />

    <div class="mt-4">         
        <nav class="mt-4" aria-label="breadcrumb">
          <ol class="breadcrumb">
            <li class="breadcrumb-item" id="opcion_principal" runat="server"><a href="#">STC</a></li>
             <li class="breadcrumb-item active" aria-current="page" id="sub_opcion" runat="server">
                 
                <span runat="server" id="etiquetaprint">Nuevo Cliente</span> 


             </li>
          </ol>
        </nav>
      </div>

 <div class="dashboard-container p-4" id="cuerpo" runat="server">

         <div class="form-title">Datos para el procesamiento</div>
		 
		  <div class="form-row">
		  
		   <div class="form-group col-md-6"> 
		   	  <label for="inputAddress">Categoría<span style="color: #FF0000; font-weight: bold;"></span></label>
			  <div class="d-flex">
                    <asp:DropDownList ClientIDMode="Static" ID="dpcategoria" 
                  CssClass="form-control" runat="server" 
                  onchange="valdpme(this,0,valcat);" >
                  <asp:ListItem Value="0" Selected="True">Selecione </asp:ListItem>
                  <asp:ListItem Value="Impo">Importación</asp:ListItem>
                  <asp:ListItem Value="Expo">Exportación</asp:ListItem>
              </asp:DropDownList>
                               <span id="valcat" class="validacion"> * </span>

			  </div>
		   </div>
		  </div>

      <div class="form-row">
		  
		   <div class="form-group col-md-6"> 
		   	  <label for="inputAddress">RUC<span style="color: #FF0000; font-weight: bold;"></span></label>
			  <div class="d-flex">
  <asp:TextBox ID="txtruc" runat="server"  MaxLength="15" CssClass=" form-control"
             onkeypress="return soloLetras(event,'1234567890')" 
              onblur="cadenareqerida(this,1,15,'valcont');"
              placeholder="RUC / CI"
                  ClientIDMode="Static"
             ></asp:TextBox>


                <asp:Button ID="btrevisa" runat="server" Text="Buscar"  
                    OnClientClick="return ValidaData();" CssClass="btn btn-primary"
                     ToolTip="Busca y carga datos en N4" OnClick="btrevisa_Click"  />
                 <span id="valcont" class="validacion"> * </span>

			  </div>
		   </div>
		  </div>

      <div class="form-row">
		  
		   <div class="form-group col-md-6"> 
		   	  <label for="inputAddress">Nombres / Descripción<span style="color: #FF0000; font-weight: bold;"></span></label>
			  <div class="d-flex">
                       <asp:TextBox ID="txtnombre" runat="server"  MaxLength="200" CssClass=" form-control"
             onkeypress="return soloLetras(event,'abcdefghijklmnñopqrstuvwxyz1234567890')" 
             onblur="cadenareqerida(this,1,200,'valnom');"
              placeholder="Descripción"
                  ClientIDMode="Static"
             ></asp:TextBox>

              <span id="valnom" class="validacion"> * </span>

			  </div>
		   </div>
		  </div>

      <div class="form-row">
		  
		   <div class="form-group col-md-6"> 
		   	  <label for="inputAddress">Teléfono (Whastapp)<span style="color: #FF0000; font-weight: bold;"></span></label>
			  <div class="d-flex">
                        <asp:TextBox ID="txtfono" runat="server"  MaxLength="12" CssClass="mayusc  form-control"
             onkeypress="return soloLetras(event,'1234567890')" 
             
              placeholder="Telefono"
                  ClientIDMode="Static"
             ></asp:TextBox> 
                <span id="valcel" class="validacion"> * </span>

			  </div>
		   </div>
		  </div>

      <div class="form-row">
		  
		   <div class="form-group col-md-6"> 
		   	  <label for="inputAddress">Email<span style="color: #FF0000; font-weight: bold;"></span></label>
			  <div class="d-flex">
                    <asp:TextBox ID="txtmail" runat="server"   CssClass=" form-control"
               onkeypress="return soloLetras(event,'abcdefghijklmnñopqrstuvwxyz1234567890_$@.',true)"  
                onblur="maildata(this,'valmailz');" maxlength="50"
             
              placeholder="Telefono"
                  ClientIDMode="Static"
             ></asp:TextBox>
              <span id="valmailz" class="validacion"> * </span>

			  </div>
		   </div>
		  </div>

      <div class="form-row">
		  
		   <div class="form-group col-md-6"> 
		   	  <label for="inputAddress">Estado de suscripción<span style="color: #FF0000; font-weight: bold;"></span></label>
			  <div class="d-flex">
                    <asp:DropDownList ClientIDMode="Static" ID="dpsuscrip"  runat="server"  CssClass="form-control" >
                  <asp:ListItem Value="1">Activo</asp:ListItem>
                  <asp:ListItem Value="0">Inactivo</asp:ListItem>
                
              </asp:DropDownList>
              <span id="valestado" class="validacion"> </span>

			  </div>
		   </div>
		  </div>

      <div class="form-row" >
		  
		   <div class="form-group col-md-6" > 
               <div class="nover" runat="server" id="trw" >
               <label for="inputAddress"> Modificaciones<span style="color: #FF0000; font-weight: bold;"></span></label>
			  <div class="d-flex">
                   <span runat ="server" id="modificaciones">-</span>

			  </div>

               </div>
		   </div>
		  </div>

       <div class="form-row">
		   <div class="col-md-12 d-flex justify-content-center"> 
		             <asp:Button ID="btbuscar" runat="server" Text="Guardar"  onclick="btbuscar_Click" 
                    OnClientClick="return getprocesa()" CssClass="btn  btn-primary"
                     ToolTip="Generar AISV"  />
		   </div> 
		   </div>

       <div class="form-row">
		   <div class="col-md-12 d-flex justify-content-center"> 
		      <div id="sinresultado" runat="server" class=" alert-info" clientidmode="Static"> </div>
		   </div> 
		   </div>
		 
     </div>






    <script src="../Scripts/pages.js" type="text/javascript"></script>

    <script type="text/javascript">
        $(document).ready(function () {
            var t = document.getElementById('bandera');
            if (t.value != undefined && t.value != null && t.value.trim().length > 0) {
                $('span.validacion').text('');
            }

        });

        function setMsg(mens) {
            var m = document.getElementById('sinresultado');
            if (m != null) {
                m.setAttribute("class", "");
                m.textContent = '';
            }
            var i = document.getElementById('sinresultado');
            if (i != null) {
                i.setAttribute("class", "");
                i.setAttribute("class", "msg-critico");
                i.textContent = mens;
            }

        }

        function ValidaData() {
            var i = document.getElementById('txtruc');
            //1.Turno horario
            if (i == null || i.value == '0' || i.value == '') {
                setMsg('Por favor escriba el numero de RUC a buscar');
                return false;
            }
            return true;
        }





        function getprocesa() {
            var i = document.getElementById('dpcategoria');
            //1.Turno horario
            if (i == null || i.value == '0' || i.value == '') {
                 setMsg('Seleccione la categoría');
                return false;
            }
            i = document.getElementById('txtruc');
            //2.Contenedor
            if (i == null || i.value == '0' || i.value == '') {
                setMsg('Por favor escriba el numero de RUC');
                return false;
            }
            i = document.getElementById('txtnombre');
            //tamaño
            if (i == null || i.value == '0' || i.value == '') {
                setMsg('Por favor escriba el nombre o descripción');
               // alert('Por favor seleccione el tamaño de contenedor');
                return false;
            }

            i = document.getElementById('txtmail');
            //tamaño
            if (i == null || i.value == '0' || i.value == '') {
                setMsg('Por favor escriba el correo electrónico');
                return false;
            }
       
            return true;
        }

     $('form').live("submit", function () { ShowProgress();});
    </script>
 <div class="loading" align="center">
    Estamos verificando toda la información 
    que nos facilitó,por favor espere unos segundos<br />
    <img src="../shared/imgs/loader.gif" alt="x" />
</div>
</asp:Content>