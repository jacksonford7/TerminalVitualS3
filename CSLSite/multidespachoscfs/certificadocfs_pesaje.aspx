<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="certificadocfs_pesaje.aspx.cs" 
    Inherits="CSLSite.certificadocfs_pesaje" Title="Certificado" %>
<!DOCTYPE HTML>
<html>
    <head>
        <meta http-equiv="Content-Type" content="text/html; charset=utf-8">
        <title>Certificado de Pesos y Dimensiones</title>
        <link href="https://fonts.googleapis.com/css?family=Open+Sans:400,700&display=swap" rel="stylesheet">
       
        <link href="../shared/estilo/carbono/style_pesos.css" rel="stylesheet" type="text/css">
        
         
    </head>
     
    <body>
	  <form id="form1" runat="server">
          <div runat="server" id="pagina">
         <%--  <section class="certificado">
            <header>
               
                <h1 style="text-align:center;"><strong><span runat="server" id="xtitulo" >Certificado de Peso y Dimensiones</span></strong> 
                    <strong><span runat="server" id="cert_tipo" >Verified Gross Mass </span></strong> 
                </h1>
            </header>
            <div>              
                <br/>
                <ul>
                    <li><strong><span runat="server" id="XNUM">Número Carga:</span></strong> <span runat="server" id="numero_carga"></span></li>
                     <li><strong>  <span runat="server" id="XCID">Agente de Aduana:</span>  </strong> <span runat="server" id="agente"></span></li>
                    <li><strong> <span runat="server" id="XIN"> Cliente:</span> </strong> <span runat="server" id="cliente"></span>    </li>
                </ul>
                <br/>
                
                 <div  runat="server" id="detalle" clientidmode="Static">   
                 <table width="722" border="1" cellpadding="0" cellspacing="0">
                  <tr>
                    <td width="187" height="22" valign="top" bgcolor="#F3F3F3"><div align="center"><strong># CERTIFICADO </strong></div></td>
                    <td width="110" valign="top" bgcolor="#F3F3F3"><div align="center"><strong>ALTO</strong></div></td>
                    <td width="113" valign="top" bgcolor="#F3F3F3"><div align="center"><strong>ANCHO</strong></div></td>
                    <td width="116" valign="top" bgcolor="#F3F3F3"><div align="center"><strong>LARGO</strong></div></td>
                    <td width="111" valign="top" bgcolor="#F3F3F3"><div align="center"><strong>PESO</strong></div></td>
                    <td width="120" valign="top" bgcolor="#F3F3F3"><div align="center"><strong>VOLUMEN</strong></div></td>
                  </tr>
                  <tr>
                    <td height="23" valign="top"><div align="center">9025-CFS-10121</div></td>
                    <td valign="top"><div align="center">2.00</div></td>
                    <td valign="top"><div align="center">1.50</div></td>
                    <td valign="top"><div align="center">1.50</div></td>
                    <td valign="top"><div align="center">415.00</div></td>
                    <td valign="top"><div align="center">4.50</div></td>
                  </tr>
                  <tr>
                    <td height="21">&nbsp;</td>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                  </tr>
                </table>
                </div>

                <div class="signature">
                   <span runat="server" id="XGER">Javier Lancha de Micheo</span>
                    <br>
                    <strong>  <span runat="server" id="XCEO">C.E.O.</span>  </strong>
                </div>
                <div class="logos">
                    <img src="../shared/imgs/carbono_img/logoContecon.jpg" width="230">
                   
                </div>
            </div>
         
            <footer>
            
            </footer>
        </section>--%>

          </div>
   
      
  
		</form>
    </body>
</html>