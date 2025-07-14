<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="certificado.aspx.cs" 
    Inherits="CSLSite.carbono_neutro.certificado_carbono" Title="Certificado" %>
<!DOCTYPE HTML>
<html>
    <head>
        <meta http-equiv="Content-Type" content="text/html; charset=utf-8">
        <title>Certificado de Carbono Neutro</title>
        <link href="https://fonts.googleapis.com/css?family=Open+Sans:400,700&display=swap" rel="stylesheet">
        <link href="../shared/estilo/carbono/style.css" rel="stylesheet" type="text/css">
    </head>
     
    <body>
	  <form id="form1" runat="server">
        <section class="certificado">
            <header>
                <h5 runat="server" id="cert_generado"></h5>
                <h1><strong>
                    <span runat="server" id="xtitulo">Carbon Neutral Certificate</span>
                    

                    </strong> 
                    <span runat="server" id="cert_tipo"> </span>
                </h1>
            </header>
            <div>
                <p id="basic_line" runat="server">Contecon Guayaquil S.A. and Soluciones Ambientales Totales Sambito S.A., endorse the inland cargo transportation of Carbon Neutral to:</p>
                <h2 runat="server" id="nombres_exportador"></h2>
              
                 <p runat="server" id="siguiente"></p>

                <ul>
                    <li><strong><span runat="server" id="XNUM">Number</span></strong> <span runat="server" id="aisv_contenedor"></span></li>
                     <li><strong>  <span runat="server" id="XCID">Certificate Number:</span>  </strong> <span runat="server" id="cert_secuencia"></span></li>
                    <li><strong> <span runat="server" id="XIN"> Gate In:</span> </strong> <span runat="server" id="unidad_fecha_ingreso"></span>    </li>
                    <li><strong><span runat="server" id="XLOAD">Load Data:</span>  </strong> <span runat="server" id="unidad_fecha_embarque"></span>  </li>
                    <li><strong> <span runat="server" id="XTRIP">Vessel/Trip:</span>  </strong> <span runat="server" id="buque_unidad_viaje"></span> </li>
                    <li><strong> <span runat="server" id="XCOM">Commodity:</span>   </strong><span runat="server" id="aisv_producto"></span></li>
                </ul>
                  <p runat="server" id="salto">&nbsp;</p>
                  <p runat="server" id="comp"></p>

                <div class="signature">
                    <span runat="server" id="XGER">Javier Lancha de Micheo</span>
                    
                    <br>
                    <strong>  <span runat="server" id="XCEO">C.E.O.</span>  </strong>
                </div>
                <div class="logos">
                    <img src="../shared/imgs/carbono_img/logoContecon.jpg" width="230">
                    <img src="../shared/imgs/carbono_img/logoSeal.png" width="110">
                </div>
            </div>
            <footer>
               <%-- <span runat="server" id="XCERTI">Carbon Neutral Port, Certified by:</span>
                <img src="../shared/imgs/carbono_img/logoTUV.png" width="150">
                <img src="../shared/imgs/carbono_img/logoSambito.png" width="120">--%>
            </footer>
        </section>
		</form>
    </body>
</html>