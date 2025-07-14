<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="certificado_inspeccion.aspx.cs" 
    Inherits="CSLSite.certificado_inspeccion" Title="Certificado" %>
<!DOCTYPE HTML>
<html>
    <head>
        <meta http-equiv="Content-Type" content="text/html; charset=utf-8">
        <title>Certificado de Carbono Neutro</title>
        <link href="https://fonts.googleapis.com/css?family=Open+Sans:400,700&display=swap" rel="stylesheet">
        <link href="../shared/estilo/carbono/style_inspeccion.css" rel="stylesheet" type="text/css">
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
            <div><br/><br/>
                <p id="basic_line" runat="server">Contecon Guayaquil S.A. and Soluciones Ambientales Totales Sambito S.A., endorse the inland cargo transportation of Carbon Neutral to:</p>
                <h2 runat="server" id="nombres_exportador"></h2>
               
                <p runat="server" id="siguiente"></p>
                <ul>
                    <li><strong><span runat="server" id="XNUM">Number</span></strong> <span runat="server" id="aisv_contenedor"></span></li>
                     <li><strong>  <span runat="server" id="XCID">Certificate Number:</span>  </strong> <span runat="server" id="cert_secuencia"></span></li>
                    <li><strong> <span runat="server" id="XIN"> Discharge:</span> </strong> <span runat="server" id="unidad_fecha_ingreso"></span>    </li>
                    <li><strong><span runat="server" id="XLOAD">Gate Out:</span>  </strong> <span runat="server" id="unidad_fecha_embarque"></span>  </li>
                    <li><strong> <span runat="server" id="XTRIP">Vessel/Trip:</span>  </strong> <span runat="server" id="buque_unidad_viaje"></span> </li>
                  
                </ul>

                <p runat="server" id="salto">&nbsp;</p>
                 <p runat="server" id="comp">sss</p>



                <div class="signature">
                    <span runat="server" id="XGER"></span>
                    
                    <br>
                    <strong>  <span runat="server" id="XCEO"></span>  </strong>
                </div>
                <div class="logos">
                    <img src="../shared/imgs/carbono_img/logoContecon.jpg" width="230">
                   
                </div>
            </div>
            <footer>
            
            </footer>
        </section>
		</form>
    </body>
</html>