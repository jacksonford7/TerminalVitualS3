<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="certificado_sellos_impo.aspx.cs" Inherits="CSLSite.certificado_sellos_impo" Title="Certificado" %>
<!DOCTYPE HTML>
<html>
    <head>
        <meta http-equiv="Content-Type" content="text/html; charset=utf-8">
        <title>Certificate of Import Seals</title>
        <link href="https://fonts.googleapis.com/css?family=Open+Sans:400,700&display=swap" rel="stylesheet">
        <link href="../shared/estilo/carbono/style.css" rel="stylesheet" type="text/css">
    </head>
     
    <body>
	  <form id="form1" runat="server">
        <section class="certificado">
            <header>
                <h5 runat="server" id="cert_generado"></h5>
                <h1><strong>
                    <span runat="server" id="xtitulo">Certificate of Import Seals</span>
                    

                    </strong> 
                    <span runat="server" id="cert_tipo">&nbsp;</span>
                </h1>
            </header>
            <div>
                <br/>
                <p id="basic_line" runat="server">Contecon Guayaquil S.A. Ensure the safety of your import unit with the seals. to:</p>
                <h2 runat="server" id="nombres_exportador"></h2>
               
                <p runat="server" id="siguiente"></p>
                <ul>
                    <li><strong><span runat="server" id="FECHA">Date:</span></strong> <span runat="server" id="fecha_certificado"></span></li>
                    <li><strong><span runat="server" id="NOMBRE">Importer Name:</span></strong> <span runat="server" id="nombre_importador"></span></li>
                    <li><strong><span runat="server" id="NUMERO">Container Number:</span></strong> <span runat="server" id="numero_contenedor"></span></li>
                    <li><strong><span runat="server" id="CARGA">Load Number:</span></strong> <span runat="server" id="numero_carga"></span></li>
                    <li><strong><span runat="server" id="CERTIFICADO">Certificate Number:</span>  </strong> <span runat="server" id="cert_secuencia"></span></li>
                   
                    <li><strong><span runat="server" id="SELLO_UNO">Seal 1:</span> </strong> <span runat="server" id="sello_uno_data"></span>    </li>
                    <li><strong><span runat="server" id="SELLO_DOS">Seal 2:</span>  </strong> <span runat="server" id="sello_dos_data"></span>  </li>
                    <li><strong><span runat="server" id="SELLO_TRES">Seal 3:</span>  </strong> <span runat="server" id="sello_tres_data"></span> </li>
                 
                </ul>

                <p runat="server" id="salto">&nbsp;</p>
                 <p runat="server" id="comp"></p>

              <%--  <div class="signature">
                    <span runat="server" id="XGER">Jesús Cáceres</span>
                    
                    <br>
                    <strong>  <span runat="server" id="XCEO">C.E.O.</span>  </strong>
                </div>--%>
                <div class="logos">
                    <img src="../shared/imgs/carbono_img/logoContecon.jpg" width="230">
                  <%--  <img src="../shared/imgs/carbono_img/logoSeal.png" width="110">--%>
                </div>
            </div>
            <footer>
            
            </footer>
        </section>
		</form>
    </body>
</html>