<%@ Page Language="C#" Title="Salida de Contenedores" AutoEventWireup="true" 
         CodeBehind="RptReqService.aspx.cs" Inherits="CSLSite.rpt_req_serv" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Salida de Contenedores</title>
    <link href="../shared/estilo/catalogos.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/work.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/aisv-general.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/tables.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/print.css" rel="stylesheet" type="text/css" />
    <link href="../shared/estilo/reqserv.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
    p{ margin:0; padding:0;}
    td span { font-weight:bold; display:block; margin:0; padding:0; background-color:#CCC; text-align:right; }
   
    
    
        #bookingfrm
        {
            width: 100%;
        }
   
    
    
        .style1
        {
            width: 117px;
        }
        .alinear
        {
             text-align:center!important;
        }
    
    
        .auto-style1 {
            width: 90px;
        }
        .auto-style2 {
            width: 1000px;
            height: 1291px;
            overflow: auto;
            background-color: White;
            margin: 0 auto;
            border-radius: 15px 15px 15px 15px;
            padding: 10px;
        }
    
    
    </style>

        <script language="javascript" type="text/javascript">
// <![CDATA[
            function btclear_onclick() {
                window.print()
            }
// ]]>
    </script>

</head>
<body>
    <form id="bookingfrm" runat="server">
    <div id="printDiv" class="rs_print" onClick="return btclear_onclick()">Imprimir</div>
    <div class="auto-style2">
       <div  >
   
   <table class="tabRepeat" style="width:96%; margin:0 auto;">
   <tr><td rowspan="4" class="style1">
       <img alt="logo"  src="../shared/imgs/logoContecon.png" width="150px" height="50px" /></td></tr>
   <tr><td colspan="1"><h1>Requerimiento de Servicios</h1></td></tr>
   <tr align="center"><td colspan="1" align="center"><h1><asp:Label CssClass="alinear" runat="server" ID="lbReq"></asp:Label></h1></td></tr>
   <tr><td colspan="1"><h1>Inspección Antinarcóticos</h1></td></tr>
   <%--<tr><td><span> Fecha de consolidación:</span> </td><td><p id="cfecha" runat="server">1900/01/01</p></td></tr>
   <tr><td><span> Booking:</span> </td><td><p id="cbook" runat="server">ABC00123</p></td></tr>
   <tr><td><span> Linea:</span> </td><td><p id="clinea" runat="server">LINEA DE PRUEBAS CONTECON</p></td></tr>--%>
   </table>
         <div class="cataresult" >
             <div class="booking" >
                 <table cellspacing='0' class="rs_tbl0" border="0" >
	<tbody>
		<%--<tr>
			<td colspan='2' align='center'><b class="rs_big">RES No <%=reqserv_arrData1(11,0)%></b></td>
	    </tr>--%>
		<tr>
			<td colspan='6' class="rs_cellRight" valign="top">
				<table class="rs_tbl1">
					<tr>
						<td class="rs_label" >Cliente:</td>
						<td><asp:Label runat="server" ID="lbClnt"></asp:Label></td>
					</tr>
					<tr>
						<td class="rs_label" >Nav&iacute;o:</td>
						<td><asp:Label runat="server" id="lbNave"></asp:Label></td>
					</tr>
					<tr>
						<td class="rs_label" >Viaje:</td>
						<td><asp:Label runat="server" id="lbViaj"></asp:Label></td>
					</tr>
					<tr>
						<td  class="rs_label">Contenedor:</td>			
						<td><asp:Label runat="server" id="lbCont"></asp:Label></td>
					</tr>
					<tr>
						<td  class="rs_label">Tamaño:</td>			
						<td><asp:Label runat="server" id="lbSize"></asp:Label></td>
					</tr>																						
					<tr>						
					    <td  class="rs_label">Tipo de carga:</td>
						<td><asp:Label runat="server" id="lbTipoC"></asp:Label></td>
					</tr>
					
				</table>
			</td>
			<td colspan='2' valign="top">
				<table class="rs_tbl1">
					<tr>
						<td class="rs_label">Fecha de Impresi&oacute;n:</td>
						<td><asp:Label runat="server" id="lbFech"></asp:Label></td>
					</tr>
					<tr>
						<td  class="rs_label">Documento de Aduana:</td>
						<td><asp:Label runat="server" id="lbDoc"></asp:Label></td>
					</tr>
					<tr>
						<td class="rs_label">Documento AISV:</td>
						<td><asp:Label runat="server" id="lbAisv"></asp:Label></td>
					</tr>
					
					<tr>
						<td valign="top"  class="rs_label">Sellos:</td>
                        <td><asp:Label runat="server" id="lbSeal1"></asp:Label></td>
                    </tr>
                    <tr>			
						<td valign="top"  class="rs_label"></td>
                        <td><asp:Label runat="server" id="lbSeal2"></asp:Label></td>
					</tr>
                    <tr>
						<td valign="top"  class="rs_label"></td>
                        <td><asp:Label runat="server" id="lbSeal3"></asp:Label></td>
                    </tr>
				</table>
			</td>
		</tr>
						
		<tr>
			<td colspan='6' class="rs_cellRightUnder" valign="top">
				<table class="rs_tbl1">
					<tr>
						<td class="rs_Seccion1">Servicios programados:</td>
					</tr>

                    <tr>
                    <td>
                    <asp:Repeater id="repeat" runat="server">
                    <ItemTemplate>

						    <div class="rs_Servicios"> <%#Eval("Codigo")%> - <%#Eval("Descripcion")%></div>

                    </ItemTemplate>
                    </asp:Repeater>
                    </td>

                    </tr>
                    <tr>
						<td class="rs_Seccion1">Contenido:</td>
					</tr>
					<tr>
						<td >
						    <div class="rs_txtArea" >&nbsp;</div>
						    <div class="rs_txtArea" >&nbsp;</div>
						    <div class="rs_txtArea" >&nbsp;</div>
						    <div class="rs_txtArea" >&nbsp;</div>
						    <div class="rs_txtArea" >&nbsp;</div>
						</td>
					</tr>
					<tr>
						<td class="rs_Seccion1">Novedades:</td>
					</tr>
					<tr>
						<td >
						    <div class="rs_txtArea" >&nbsp;</div>
						    <div class="rs_txtArea" >&nbsp;</div>
						    <div class="rs_txtArea" >&nbsp;</div>
						    <div class="rs_txtArea" >&nbsp;</div>
						    <div class="rs_txtArea" >&nbsp;</div>
						</td>
					</tr>
				</table>				
			</td>
			<td colspan='2' valign="top" class="rs_rowUnder">
				<table class="rs_tbl1">
					<tr>
						<td class="rs_Seccion1" colspan="2">Servicios Adicionales Brindados:</td>
					</tr>
					<tr>
			            <td>Pesaje:</td>
			            <td><div class="lchkbox" /></td>
			        </tr>
			        <tr>
			            <td>Extra Handling:</td>
			            <td><div class="lchkbox" /></td>
			        </tr>
			        <tr>		
			            <td>Contenedor de Apoyo:</td>
			            <td><div class="lchkbox" /></td>
		            </tr>
		            <tr>
			            <td>Hora de conexión de cont. alq:</td>
			            <td><div class="lchkbox" /></td>
		            </tr>
		            <tr>
			            <td>Sello Adicional:</td>
			            <td><div class="lchkbox" /></td>
		            </tr>
		            <tr>
			            <td>Cerrojo:</td>
			            <td><div class="lchkbox" /></td>
		            </tr>
		            <tr>
			            <td>Persona Adicional:</td>
			            <td><div class="lchkbox" /></td>
		            </tr>
		            <tr>
			            <td>Porteo:</td>
			            <td><div class="lchkbox" /></td>
		            </tr>
		            <tr>
			            <td>Provisi&oacute;n Equipos Especiales:</td>
			            <td><div class="lchkbox" /></td>
		            </tr>
		            <tr>
						<td class="rs_Seccion1" colspan="2">Nuevos sellos:</td>
					</tr>
					<tr><td>Sello 1:</td><td>&nbsp;</td></tr>
			        <tr><td>Sello 2:</td><td>&nbsp;</td></tr>
			        <tr><td>Sello 3:</td><td>&nbsp;</td></tr>
			        <tr>
						<td class="rs_Seccion1" colspan="2">Tiempo del Servicio:</td>
					</tr>				
		            <tr>
		                <td>Hora Inicial:</td>
			            <td>&nbsp;</td>
			        </tr>
			        <tr>
		                <td>Hora Final:</td>
			            <td>&nbsp;</td>
		            </tr>
					
					<tr>
						<td class="rs_Seccion1" colspan="2">Tipo de Carga:</td>
					</tr>
					<tr>
			            <td>Granel:</td>
			            <td><div class="lchkbox" /></td>
			        </tr>
			        <tr>
			            <td>Paletizado:</td>
			            <td><div class="lchkbox" /></td>
			        </tr>
				
				</table>

				<table class="rs_tbl1">
					<tr>
						<td class="rs_Seccion1" colspan="2">Porcentaje de Extracción:</td>
					</tr>
					 
					<tr>
			            <td>
							<table class="rs_tbl1">
								<tr>
									<td>10%:</td>
									<td class="auto-style1"><div class="lchkbox" /></td>
								</tr>
									<tr>
									<td>20%:</td>
									<td class="auto-style1"><div class="lchkbox" /></td>
								</tr>
								<tr>
									<td>30%:</td>
									<td class="auto-style1"><div class="lchkbox" /></td>
								</tr>
								<tr>
									<td>40%:</td>
									<td class="auto-style1"><div class="lchkbox" /></td>
								</tr>
								<tr>
									<td>50%:</td>
									<td class="auto-style1"><div class="lchkbox" /></td>
								</tr>
							</table>
			            </td>
			            
						 <td>
							<table class="rs_tbl1">
								<tr>
									<td>60%:</td>
									<td><div class="lchkbox" /></td>
								</tr>
									<tr>
									<td>70%:</td>
									<td><div class="lchkbox" /></td>
								</tr>
								<tr>
									<td>80%:</td>
									<td><div class="lchkbox" /></td>
								</tr>
								<tr>
									<td>90%:</td>
									<td><div class="lchkbox" /></td>
								</tr>
								<tr>
									<td>100%:</td>
									<td><div class="lchkbox" /></td>
								</tr>
							</table>
			            </td>
						 
			        </tr>
				</table>
			</td>
										
		</tr>		
		 <tr>
            <td align="left" colspan="4" valign="top" class="rs_cellRightUnder">
                <table  class="rs_tbl1">
                    <tr><td colspan="2" class="rs_Seccion1">Exportador/Representante</td></tr>                
                    <tr>
                        <td>Nombre:</td>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td>CI:</td>
                        <td>&nbsp;</td>
                    </tr>
                </table>
            </td>
            <td align="left" colspan="4" valign="top" class="rs_rowUnder">
                <table  class="rs_tbl1">
                    <tr><td colspan="2" class="rs_Seccion1">L&iacute;der de CFS-CGSA</td></tr>                
                    <tr>
                        <td>Nombre:</td>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td>CI:</td>
                        <td>&nbsp;</td>
                    </tr>
                </table>
            </td>
		</tr>		 
        <tr>
            <td align="middle" colspan="8" class="rs_Seccion1">
                VERIFICACIÓN DE PROTOCOLOS TLS
            </td>
		</tr>
        <tr>
        <td colspan='8' valign="top" class="rs_cellRightUnder">
				<table class="rs_tbl1">
                <tr>
						<td></td>
                        <td>SI</td>
                        <td>NO</td>
                        <td></td>
                        <td>SI</td>
                        <td>NO</td>
					</tr>
					<tr>
			            <td>La Inspección inició a la hora programada</td>
			            <td><div class="lchkbox" /></td>
                        <td><div class="lchkbox" /></td>
                        <td>La inspección se realizó bajo techo</td>
			            <td><div class="lchkbox" /></td>
                        <td><div class="lchkbox" /></td>
			        </tr>
			        <tr>
			            <td>El personal de estiba utilizó los EPP</td>
			            <td><div class="lchkbox" /></td>
                        <td><div class="lchkbox" /></td>
                        <td>Se utilizó cámara fría o cont. de apoyo</td>
			            <td><div class="lchkbox" /></td>
                        <td><div class="lchkbox" /></td>
			        </tr>
			        <tr>		
			            <td>El personal de estiba tiene ropa apropiada</td>
			            <td><div class="lchkbox" /></td>
                        <td><div class="lchkbox" /></td>
                        <td>El área de inspección estaba limpia</td>
			            <td><div class="lchkbox" /></td>
                        <td><div class="lchkbox" /></td>
		            </tr>
		            <tr>
			            <td>Uso de Herramientas adecuadas</td>
			            <td><div class="lchkbox" /></td>
                        <td><div class="lchkbox" /></td>
                        <td>Se utilizó mesas de acero inoxidable</td>
			            <td><div class="lchkbox" /></td>
                        <td><div class="lchkbox" /></td>
		            </tr>
		            <tr>
			            <td>Se realizó toma fotográfica de la insp.</td>
			            <td><div class="lchkbox" /></td>
                        <td><div class="lchkbox" /></td>
                        <td>El suelo se encuentra en buen estado</td>
			            <td><div class="lchkbox" /></td>
                        <td><div class="lchkbox" /></td>
		            </tr>
		            <tr>
			            <td>Correcta manipulación de la mercadería</td>
			            <td><div class="lchkbox" /></td>
                        <td><div class="lchkbox" /></td>
                        <td>La mercadería tuvo contacto con el suelo</td>
			            <td><div class="lchkbox" /></td>
                        <td><div class="lchkbox" /></td>
		            </tr>
		            <tr>
			            <td>La mercadería fue manipulada con guantes</td>
			            <td><div class="lchkbox" /></td>
                        <td><div class="lchkbox" /></td>
                        <td>Se toman muestras de la mercadería</td>
			            <td><div class="lchkbox" /></td>
                        <td><div class="lchkbox" /></td>
		            </tr>
		            <tr>
			            <td>Se reacondicionó la mercadería en el interior del embalaje</td>
			            <td><div class="lchkbox" /></td>
                        <td><div class="lchkbox" /></td>
                        <td>Se dañó mercadería producto de la insp.</td>
			            <td><div class="lchkbox" /></td>
                        <td><div class="lchkbox" /></td>
		            </tr>
				</table>
			</td>
        </tr>
        <tr>
            <td align="left" colspan="8" class="rs_Seccion1">
                Información a considerar:
            </td>
		</tr>
		<tr>
			<td colspan="8">1. Debe presentar 2 juegos de este formulario para realizar la inspección</td>
	    </tr>
	    <tr>
			<td colspan="8">2. El contenedor se encuentra bloqueado por dispocion de la PAN.  La fecha indicada en este documento para la inspección deberá cumplirse, caso contrario el servicio no podrá brindarse </td>
	    </tr>
	    <tr>
			<td colspan="8">3. El contenedor no será embarcado si esta operación no se cumple o si los servicios no se encuentran cancelados.</td>
	    </tr>
	    <tr>
			<td colspan="8">4. La PAN desbloquerá la unidad concluida la inspección.</td>
	    </tr>
	</tbody>
</table>
       </div>
            <div class="botonera" >
            <input id="btclear"   type="reset"    value="Imprimir" onclick="return btclear_onclick()" />
            </div>
      </div>
      </div>
      </div>
    </form>
</body>
</html>
