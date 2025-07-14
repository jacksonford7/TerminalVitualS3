<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="certificado.aspx.cs" 
    Inherits="CSLSite.escaner.certificado_scan" Title="Certificado" %>
<!DOCTYPE HTML>
<html>
    <head>
        <meta http-equiv="Content-Type" content="text/html; charset=utf-8">
        <title>Certificado de Carbono Neutro</title>
        <link href="https://fonts.googleapis.com/css?family=Open+Sans:400,700&display=swap" rel="stylesheet">
        <link href="../shared/estilo/carbono/style.css" rel="stylesheet" type="text/css">


        <style>
            table {
                border-collapse: collapse;
            }

            td {
                border: none;
                padding: 5px;
            }
        </style>
    </head>
     
    <body>
	  <form id="form1" runat="server">
        <section class="certificado">
            <header>
                <h5 runat="server" id="cert_generado"></h5>
                <h1><strong>
                    <span runat="server" id="xtitulo">SCAN CERTIFICATE</span>
                    <br />
                    <span runat="server" id="XSUB_P"></span>

                    </strong> 
               
                    <span runat="server" id="cert_tipo"> </span>
                </h1>
            </header>
            <div>
                <%--<p id="basic_line" runat="server">Contecon Guayaquil S.A. and Soluciones Ambientales Totales Sambito S.A., endorse the inland cargo transportation of Carbon Neutral to:</p>
                <h2 runat="server" id="nombres_exportador"></h2>--%>
              
                 <%--<p runat="server" id="siguiente"></p>--%>

                <table>
                    <tbody>
                        
                        <tr>
                            <td><strong><span runat="server" id="XDATO01">Certificate Code:</span></strong></td>
                            <td><span runat="server" id="cert_secuencia"></span></td>
                        </tr>
                        <tr>
                            <td><strong>  <span runat="server" id="XDATO02">BL / Booking Number:</span>  </strong></td>
                            <td><span runat="server" id="aisv_booking"></span></td>
                        </tr>
                        <tr>
                            <td><strong><span runat="server" id="XDATO03">Container ID:</span></strong></td>
                            <td><span runat="server" id="aisv_contenedor"></span></td>
                        </tr>
                        <tr>
                            <td><strong>  <span runat="server" id="XDATO04">Vessel / Trip:</span>  </strong></td>
                            <td><span runat="server" id="aisv_vessel"></span></td>
                        </tr>
                        <tr>
                            <td><strong><span runat="server" id="XDATO05">Customs Reference:</span></strong></td>
                            <td><span runat="server" id="aisv_dae"></span></td>
                        </tr>
                        <tr>
                            <td><strong>  <span runat="server" id="XDATO06">Seal 1:</span>  </strong></td>
                            <td><span runat="server" id="aisv_seal1"></span></td>
                        </tr>
                        <tr>
                            <td><strong><span runat="server" id="XDATO07">Seal 2:</span></strong></td>
                            <td><span runat="server" id="aisv_seal2"></span></td>
                        </tr>
                        <tr>
                            <td><strong>  <span runat="server" id="XDATO08">Seal 3:</span>  </strong></td>
                            <td><span runat="server" id="aisv_seal3"></span></td>
                        </tr>
                        <tr>
                            <td><strong><span runat="server" id="XDATO09">Seal 4:</span></strong></td>
                            <td><span runat="server" id="aisv_seal4"></span></td>
                        </tr>
                        <tr>
                            <td><strong>  <span runat="server" id="XDATO10">Transportation Company:</span>  </strong></td>
                            <td><span runat="server" id="aisv_transportista"></span></td>
                        </tr>
                        <tr>
                            <td><strong><span runat="server" id="XDATO11">Driver's Name:</span></strong></td>
                            <td><span runat="server" id="aisv_chofer"></span></td>
                        </tr>
                        <tr>
                            <td><strong>  <span runat="server" id="XDATO12">Driver's Id:</span>  </strong></td>
                            <td><span runat="server" id="aisv_licencia"></span></td>
                        </tr><tr>
                            <td><strong><span runat="server" id="XDATO13">Plate: </span></strong></td>
                            <td><span runat="server" id="aisv_placa"></span></td>
                        </tr>
                        <tr>
                            <td><strong>  <span runat="server" id="XDATO14">*Scanning Process Status:</span>  </strong></td>
                            <td><span runat="server" id="scan_status"></span></td>
                        </tr>
                        <tr>
                            <td><strong><span runat="server" id="XDATO15">Scanning Date/Time: </span></strong></td>
                            <td><span runat="server" id="scan_date"></span></td>
                        </tr>
                        

                    </tbody>
                </table>

                <br>
                <br>

                    <span runat="server"  id="XDATO16"></span>
                    
                <br>
                <br>
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