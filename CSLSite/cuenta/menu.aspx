<%@ Page Title="" Language="C#" MasterPageFile="~/site.Master" AutoEventWireup="true" CodeBehind="menu.aspx.cs" Inherits="CSLSite.cuenta.menu" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="placehead" runat="server">
     <!-- Favicons -->
 <%-- <link rel="canonical" href="https://getbootstrap.com/docs/4.5/examples/dashboard/">--%>
  <link href="../img/favicon2.png" rel="icon"/>
  <link href="../img/icono.png" rel="apple-touch-icon"/>
  <link href="../css/bootstrap.min.css" rel="stylesheet"/>
  <link href="../css/dashboard.css" rel="stylesheet"/>
  <link href="../css/icons.css" rel="stylesheet"/>
  <link href="../css/style.css" rel="stylesheet"/>
  <link href="https://fonts.googleapis.com/css2?family=Roboto:wght@300;400;500;700&display=swap" rel="stylesheet"/>


       <style type="text/css">
        body
        {
            /*font-family: Arial;
            font-size: 10pt;*/
        }
        .modal
        {
            position: fixed;
            z-index: 999;
            height: 100%;
            width: 100%;
            top: 0;
            background-color: Black;
            filter: alpha(opacity=60);
            opacity: 0.6;
            -moz-opacity: 0.8;
        }

        .modalBackground
        {
            background-color: Black;
            filter: alpha(opacity=60);
            opacity: 0.6;
        }
        .modalPopup
        {
            background-color: #FFFFFF;
            width: 726px;
            border: 3px solid #FF3720;
            padding: 0;
        }
        .modalPopup .header
        {
            background-color: #2FBDF1;
            height: 30px;
            color: White;
            line-height: 30px;
            text-align: center;
            font-weight: bold;
        }
        .modalPopup .body
        {
            min-height: 50px;
            line-height: 25px;
            text-align: center;
            /*font-weight: bold;*/
            margin-bottom: 5px;
        }
    </style>
 

<script type="text/javascript">
$(document).ready(function(){
  $('[data-toggle="tooltip"]').tooltip();   
});
</script>


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="placebody" runat="server">
        <asp:ToolkitScriptManager ID="tkscata" runat="server"></asp:ToolkitScriptManager>
    <br/>
       <asp:HiddenField ID="manualHide" runat="server" />
<div class="dashboard-container p-4">
      <%--<div class="form-row">  
            <div class="form-group col-md-4">
                 <label for="inputEmail4">FECHA SALIDA<span style="color: #FF0000; font-weight: bold;">*</span></label>
                <asp:TextBox runat="server" ID="TxtTipo" class="form-control" disabled></asp:TextBox>
            </div>
     </div>--%>

              <div class="mb-5">
              <div id="carouselExampleCaptions" class="carousel slide" data-ride="carousel">
                <ol class="carousel-indicators">
                  <li data-target="#carouselExampleCaptions" data-slide-to="0" class="active"></li>
                  <li data-target="#carouselExampleCaptions" data-slide-to="1"></li>
                  <li data-target="#carouselExampleCaptions" data-slide-to="2"></li>
                </ol>
                <div class="carousel-inner">
                  <div class="carousel-item active">
                    <img src="../img/1.png" class="d-block w-100" alt="..."/>
                    <div class="carousel-caption d-none d-md-block">
                      <%--<h5>First slide label</h5>
                      <p>Nulla vitae elit libero, a pharetra augue mollis interdum.</p>--%>
                    </div>
                  </div>
                  <div class="carousel-item">
                    <img src="../img/2.png" class="d-block w-100" alt="..."/>
                    <div class="carousel-caption d-none d-md-block">
                     <%-- <h5>Second slide label</h5>
                      <p>Lorem ipsum dolor sit amet, consectetur adipiscing elit.</p>--%>
                    </div>
                  </div>
                  <div class="carousel-item">
                    <img src="../img/3.png" class="d-block w-100" alt="..."/>
                    <div class="carousel-caption d-none d-md-block">
                    <%--  <h5>Third slide label</h5>
                      <p>Praesent commodo cursus magna, vel scelerisque nisl consectetur.</p>--%>
                    </div>
                  </div>
                </div>
                <a class="carousel-control-prev" href="#carouselExampleCaptions" role="button" data-slide="prev">
                  <span class="carousel-control-prev-icon" aria-hidden="true"></span>
                  <span class="sr-only">Previous</span>
                </a>
                <a class="carousel-control-next" href="#carouselExampleCaptions" role="button" data-slide="next">
                  <span class="carousel-control-next-icon" aria-hidden="true"></span>
                  <span class="sr-only">Next</span>
                </a>
              </div>
            </div>

             <div id="error" runat="server" class="alert alert-danger" visible="false">
  </div>

     <!-- Modal -->
   <asp:ModalPopupExtender  
      ID="mpedit" runat="server" 
      PopupControlID="myModal2"
      BackgroundCssClass="modalBackground"  
      TargetControlID="manualHide"
       CancelControlID="btclose"  
      >
    </asp:ModalPopupExtender>
      <asp:Panel ID="myModal2" runat="server" CssClass="modalPopup" align="center" Style="display: none" >
        
        <div class="body">
             <asp:UpdatePanel ID="msgload" runat="server" UpdateMode="Conditional"  ChildrenAsTriggers="true">
               <ContentTemplate>
            
 <table width="724" border="0" cellpadding="0" cellspacing="0">
  <!--DWLayoutTable-->
  <tr>
    <td height="36" colspan="3" valign="middle" bgcolor="#FF3720"><span style="color:#f2f2f2; font-weight:bold">&nbsp; Alerta de Contenedores Bloqueados -  PAN</span></td>
  </tr>
  <tr>
    <td width="17" rowspan="4" valign="top"><!--DWLayoutEmptyCell-->&nbsp;</td>
    <td width="692" height="76" valign="top"><div align="justify"><br/>Estimado cliente, ud tiene unidades bloqueadas por la Unidad de Policía Antinarcóticos.<br/><br/>
            Para revisar las mismas debe dirigirse a la siguiente opción del de Terminal Virtual:<br/><br/>

  </div></td>
  <td width="15" rowspan="4" valign="top"><!--DWLayoutEmptyCell-->&nbsp;</td>
  </tr>
  <tr>
    <td height="150" valign="top"><div align="left"><a href="../aisv/pan_consulta.aspx"><strong>Ver Detalle </strong></a><br/>
       
    </div></td>
  </tr>
  
  <tr>
    <td height="104" valign="top">

	    <table width="100%" border="0" cellpadding="0" cellspacing="0">
	    
            </tr>  <td><br/></td></tr>
	 
	      <tr>
	        <td height="24" colspan="4" valign="top">
                 <div class="d-flex justify-content-end mt-4">
                <asp:Button ID="BtnProcesar" runat="server" class="btn btn-primary"  
                                                Text="Acepto El Servicio"  OnClientClick="this.disabled=true"
                                            UseSubmitBehavior="false"  Visible="false" />    &nbsp;&nbsp;                        
              <input  type="button" id="btclose" class="btn btn-primary" value="Salir"  />
                     </div>
                     </td>
          </tr>
	      <tr>
	        <td height="1"></td>
	        <td></td>
	        <td width="31"></td>
	        <td></td>
          </tr>
            </table>
     </div></td>
  </tr>
</table>

                     </ContentTemplate>
                   <Triggers>
                          <asp:AsyncPostBackTrigger ControlID="BtnProcesar" />
                        </Triggers>
            </asp:UpdatePanel>
        </div>
    </asp:Panel>
   <!-- Modal -->
     <asp:ModalPopupExtender  
      ID="leydatos" runat="server" 
      PopupControlID="Panelleydatos"
      BackgroundCssClass="modalBackground"  
      TargetControlID="manualHide"
    
      >
    </asp:ModalPopupExtender>
    <asp:Panel ID="Panelleydatos" runat="server" CssClass="modal-dialog modal-dialog-scrollable" align="center" Style="display: none;"   TabIndex="-1"   >
         <div>
              <div class="modal-content">
                  <div class="dashboard-container p-4" id="Div1" runat="server">    
                      <div class="modal-header"><h5 class="modal-title" id="Titulo" runat="server">
                          Política de Protección y Tratamiento de Datos personales de CONTECON GUAYAQUIL S.A.
                          </h5>
                          </div>
                  </div>
                  <div class="modal-body">

                   <table width="650" border="0" cellpadding="0" cellspacing="0">
  <!--DWLayoutTable-->
  <tr>
    <td width="650" height="740" valign="top"><p align="justify"> <strong>Contecon Guayaquil S.A. </strong>(en adelante, &ldquo;Contecon&rdquo; o la &ldquo;Compa&ntilde;&iacute;a&rdquo;), empresa  filial ecuatoriana de la empresa filipina International Container Terminal  Services Inc, ICTSI, respeta la privacidad de las personas y est&aacute; totalmente  comprometida con la protecci&oacute;n de la informaci&oacute;n personal y confidencial de las  personas, de acuerdo con lo dispuesto en la Ley  Org&aacute;nica de Protecci&oacute;n de Datos Personales (en adelante, &ldquo;LOPDP&rdquo;) del Ecuador,  publicada en el Quinto Suplemento del Registro Oficial 459, del 26 de mayo del 2021,  sus reformas, y su Reglamento. </p>
      <p align="justify"><br />
        <strong>Declaraciones generales de la  pol&iacute;tica de privacidad:</strong><strong> </strong></p>
      <ol start="1" type="1">
        <li>
          <div align="justify">Contecon       se adhiere a los principios generales de juridicidad, lealtad,       transparencia, finalidad, pertinencia y       minimizaci&oacute;n de datos personales, proporcionalidad del tratamiento,       confidencialidad, calidad, exactitud, conservaci&oacute;n, seguridad de datos       personales, responsabilidad proactiva y demostrada, aplicaci&oacute;n favorable       al titular e independencia del control, estipulados en la LOPDP. </div>
        </li>
        <li>
          <div align="justify">Los       empleados, clientes, contratistas o terceros cuya informaci&oacute;n personal se       recopila se considerar&aacute;n sujetos de datos para los efectos de estas       pol&iacute;ticas. </div>
        </li>
        <li>
          <div align="justify">Se       informar&aacute; al Titular del motivo o finalidad de la recopilaci&oacute;n y       tratamiento de los datos personales. </div>
        </li>
        <li>
          <div align="justify">El Titular       de los datos tendr&aacute; derecho a corregir la informaci&oacute;n, especialmente en       casos de datos err&oacute;neos o desactualizados, y a oponerse a la recopilaci&oacute;n       de informaci&oacute;n personal dentro de los l&iacute;mites permitidos por la LOPDP. </div>
        </li>
        <li>
          <div align="justify">El Titular       tiene derecho a presentar requerimientos, peticiones, quejas o reclamaciones       relacionadas con el ejercicio de sus derechos, conforme a la LOPDP. </div>
        </li>
        <li>
          <div align="justify">Contecon       proteger&aacute; la informaci&oacute;n personal de los empleados y terceros de quienes       se recopila la informaci&oacute;n personal y tomar&aacute; las medidas adecuadas para       proteger las copias f&iacute;sicas y digitales de la informaci&oacute;n. </div>
        </li>
        <li>
          <div align="justify">Contecon       se asegurar&aacute; de que la informaci&oacute;n personal sea recopilada y procesada       &uacute;nicamente por personal autorizado para fines leg&iacute;timos de la Compa&ntilde;&iacute;a. </div>
        </li>
        <li>
          <div align="justify">Cualquier       informaci&oacute;n que sea declarada obsoleta con base en los procedimientos       internos de privacidad y retenci&oacute;n de la Compa&ntilde;&iacute;a deber&aacute; ser eliminada de       manera segura y legal. </div>
        </li>
        <li>
          <div align="justify">Cualquier       incumplimiento, sospechado o real, de la Pol&iacute;tica de Protecci&oacute;n de Datos       Personales de Contecon debe informarse al Responsable o Encargado del       tratamiento de datos personales (RTDP). </div>
        </li>
        <li>
          <div align="justify">Los Titulares       &#8203;&#8203;pueden consultar o solicitar informaci&oacute;n al RTDP, con respecto a       cualquier asunto relacionado con el procesamiento de sus datos personales       bajo la custodia de Contecon, incluidas las pol&iacute;ticas de privacidad y seguridad       de datos implementadas para garantizar la protecci&oacute;n de sus datos       personales. </div>
        </li>
      </ol>
      <p><strong>Contecon reconoce la importancia de los derechos de  un Titular de datos bajo la </strong><strong>LOPDP</strong> <strong>de la  siguiente manera:</strong><strong> </strong></p>
      <ol start="1" type="1">
        <li>
          <div align="justify">Derecho       a ser informado sobre si se tratar&aacute;n, se est&aacute;n tratando o se han tratado       datos personales que le conciernen. </div>
        </li>
        <li>
          <div align="justify">Derecho       a oponerse al procesamiento de su informaci&oacute;n personal proporcionada y la       oportunidad de negar el consentimiento para el procesamiento en caso de       cambios o enmiendas a la informaci&oacute;n proporcionada. </div>
        </li>
        <li>
          <div align="justify">Derecho       de acceso, previa solicitud, al contenido de sus datos personales que       fueron procesados, fuentes de donde se obtuvieron los datos personales,       nombres y direcciones de los destinatarios de los datos personales, forma       en que se procesaron dichos datos, razones para la divulgaci&oacute;n de los       datos personales a destinatarios, si los hubiere, entre otros. </div>
        </li>
        <li>
          <div align="justify">Derecho       de rectificaci&oacute;n y actualizaci&oacute;n para corregir la       inexactitud o error en los datos personales y su correcci&oacute;n inmediata. </div>
        </li>
        <li>
          <div align="justify">Derecho       a la eliminaci&oacute;n de sus datos personales. </div>
        </li>
        <li>
          <div align="justify">Derecho       a la portabilidad de datos o la capacidad de mover datos de una plataforma       o servicio a otro. </div>
        </li>
        <li>
          <div align="justify">Transmisibilidad       de los Derechos a los leg&iacute;timos herederos y causahabientes del Titular. </div>
        </li>
      </ol>
      <p align="justify"><strong>Recopilaci&oacute;n de informaci&oacute;n personal</strong><br /><br />
  Al recopilar su informaci&oacute;n personal, Contecon se asegura de que el Titular  conozca la naturaleza, el prop&oacute;sito y el alcance del procesamiento de su  informaci&oacute;n personal.&nbsp;El tratamiento de la informaci&oacute;n deber&aacute; ser  adecuado, pertinente, apropiado, necesario y no excesivo en relaci&oacute;n con una  finalidad declarada y especificada. <br />
  Informaci&oacute;n personal se refiere a cualquier informaci&oacute;n, ya sea  registrada en una forma material o no, a partir de la cual la identidad de un  individuo sea evidente o puede ser razonable y directamente determinada por la  entidad que posee la informaci&oacute;n, o cuando combinada con otra informaci&oacute;n  directa y ciertamente identificar&iacute;a a un individuo. <br />
  En el desempe&ntilde;o de nuestros servicios, o como parte de nuestras  transacciones y tratos, procesamos y recopilamos la siguiente informaci&oacute;n  personal que puede incluir, entre otros: </p>
      <div align="justify">
        <ol start="1" type="1">
          <li>Nombre,       estado civil, n&uacute;mero de identificaci&oacute;n fiscal, edad, direcci&oacute;n, educaci&oacute;n,       profesi&oacute;n, experiencia comercial, afiliaci&oacute;n comercial, afiliaci&oacute;n       familiar y cualquier informaci&oacute;n a partir de la cual la identidad de un       individuo sea evidente o pueda determinarse razonable y directamente, ya       sea registrada en forma material o no, como filmaciones de CCTV y otras       grabaciones visuales como parte de nuestras transacciones con usted. </li>
          <li>Historial       de empleo, curr&iacute;culum y fotos enviadas con &eacute;l, compensaci&oacute;n y beneficios,       antecedentes educativos, afiliaci&oacute;n organizacional, g&eacute;nero, fecha de       nacimiento, religi&oacute;n, etnia, estado civil, ciudadan&iacute;a, historial m&eacute;dico       f&iacute;sico, antecedentes penales y/o administrativos, informaci&oacute;n de       identificaci&oacute;n emitida por Entidades gubernamentales, identificaciones       profesionales, pasaporte y certificados de nacimiento, informaci&oacute;n de       n&oacute;mina de los solicitantes de empleo y empleados actuales. </li>
          <li>Informaci&oacute;n       de la empresa, desempe&ntilde;o, historial, finanzas y capital, durante la       acreditaci&oacute;n de proveedores para participar en transacciones comerciales       con nosotros. </li>
          <li>Informaci&oacute;n       que nos proporciona cuando visita o utiliza el sitio web de nuestra       empresa y otras aplicaciones m&oacute;viles y en l&iacute;nea, y cualquier informaci&oacute;n       que env&iacute;e a nuestros representantes comerciales o relaciones con los       clientes para actualizar sus registros o informaci&oacute;n. </li>
        </ol>
      </div>
      <ol start="1" type="1">
      </ol>
      <p>&nbsp;<strong>Uso de informaci&oacute;n personal</strong><strong> </strong><br />
        El Procesamiento de Informaci&oacute;n Personal es para prop&oacute;sitos de:&nbsp; </p>
      <ol start="1" type="1">
        <li>
          <div align="justify">Identificaci&oacute;n       en los registros de la empresa y transparencia corporativa; </div>
        </li>
        <li>
          <div align="justify">Transacciones       corporativas y operaciones comerciales; </div>
        </li>
        <li>
          <div align="justify">Cumplimiento       de los requisitos, procedimientos, certificaciones, incluidas las       obligaciones de presentaci&oacute;n de informes, de los distintos Entes de       Control a la que se encuentra sometido Contecon y en general a todas las       autoridades administrativas del Estado ecuatoriano. </div>
        </li>
        <li>
          <div align="justify">Mantenimiento       de la seguridad dentro y alrededor de las instalaciones. </div>
        </li>
        <li>
          <div align="justify">Cumplimiento       de procesos o requerimientos legales de &oacute;rganos judiciales, o cualquier       otra Entidad gubernamental;&nbsp;y </div>
        </li>
        <li>
          <div align="justify">Cualquier       otra actividad comercial leg&iacute;tima de Contecon. </div>
        </li>
      </ol>
      <p align="justify"><strong>Compartir informaci&oacute;n personal</strong><strong> </strong><br />
        CONTECON puede compartir o divulgar informaci&oacute;n personal de los Titulares  en los siguientes casos: </p>
      <div align="justify">
        <ul>
          <li>Por consentimiento del titular para el  tratamiento de sus datos personales, para una o varias finalidades espec&iacute;ficas; </li>
          <li>Que sea realizado por el responsable del  tratamiento en cumplimiento de una obligaci&oacute;n legal; </li>
          <li>Que sea realizado por el responsable del  tratamiento, por orden judicial, debiendo observarse lo dispuesto en la LOPDP; </li>
          <li>Que el tratamiento de datos personales se  sustente en el cumplimiento de una misi&oacute;n realizada en inter&eacute;s p&uacute;blico o en el  ejercicio de poderes p&uacute;blicos conferidos al responsable, derivados de una  competencia atribuida por una norma legal, sujeto al cumplimiento de los  est&aacute;ndares internacionales de derechos humanos aplicables a la materia, al  cumplimiento de los principios de esta ley y a los criterios de legalidad,  proporcionalidad y necesidad; </li>
          <li>Para la ejecuci&oacute;n de medidas  precontractuales a petici&oacute;n del titular o para el cumplimiento de obligaciones  contractuales perseguidas por el responsable del tratamiento de datos  personales, encargado del tratamiento de datos personales o por un tercero  legalmente habilitado; </li>
          <li>Para proteger intereses vitales, del Titular  o de otra persona natural, como su vida, salud o integridad; </li>
          <li>Para tratamiento de datos personales que  consten en bases de datos de acceso p&uacute;blico; </li>
          <li>Para satisfacer un inter&eacute;s leg&iacute;timo del  responsable de tratamiento o de tercero, siempre que no prevalezca el inter&eacute;s o  derechos fundamentales de los titulares al amparo de lo dispuesto en esta  norma. </li>
        </ul>
      </div>
      <ul>
      </ul>
      <p align="justify"><strong>Retenci&oacute;n y seguridad de la  informaci&oacute;n personal</strong><strong> </strong><br />
        La informaci&oacute;n personal recopilada se conservar&aacute; durante el tiempo que  sea necesario para: </p>
      <div align="justify">
        <ol start="1" type="1">
          <li>Cumplir       con los fines declarados, especificados y leg&iacute;timos previstos en la LOPDP, o cuando se haya completado o terminado       el proceso relacionado con el fin para el que fueron proporcionados los       datos personales; </li>
          <li>Ejercer       o defender reclamaciones o requerimientos legales;&nbsp; </li>
          <li>Cumplir       con las leyes, reglamentos u &oacute;rdenes judiciales legales. </li>
        </ol>
      </div>
      <p align="justify">A partir de entonces, sus datos personales se eliminar&aacute;n o descartar&aacute;n  de manera segura que impidan el procesamiento posterior, el acceso no  autorizado o la divulgaci&oacute;n a cualquier otra parte o a terceros. <br />
        Contecon asegura la integridad y confidencialidad de su informaci&oacute;n  personal al proporcionar medidas, pol&iacute;ticas y procedimientos de seguridad  organizativos, f&iacute;sicos y t&eacute;cnicos apropiados y adecuados destinados a reducir  los riesgos de destrucci&oacute;n o p&eacute;rdida accidental, o la divulgaci&oacute;n o el acceso  no autorizados a dicha informaci&oacute;n.&nbsp;El acceso f&iacute;sico a los servidores y  equipos de red est&aacute; altamente restringido solo al personal autorizado.&nbsp;Se  emplean varios aparatos y dispositivos de seguridad para salvaguardar la red de  Contecon y sus sistemas. <br />
  <strong>Actualizaci&oacute;n de la pol&iacute;tica</strong><strong> </strong><br />
        La Pol&iacute;tica de protecci&oacute;n y tratamiento de datos personales se  actualizar&aacute; peri&oacute;dicamente para cumplir con las leyes, normas y reglamentos  aplicables y para mejorar a&uacute;n m&aacute;s los procedimientos de seguridad en el  procesamiento de Informaci&oacute;n personal. &nbsp; <br />
        Para cualquier denuncia o inquietud con respecto a la Pol&iacute;tica de  Privacidad de Datos, puede comunicarse con nuestro Responsable de Protecci&oacute;n de  Datos en: </p>
      <ul type="disc">
        <li>Direcci&oacute;n postal: Av. De la Marina, Puerto       Mar&iacute;timo </li>
        <li>Direcci&oacute;n de correo       electr&oacute;nico:&nbsp;&nbsp;<a href="mailto:privacy@ictsi.com?subject=ICTSI%20Privacy%20Policy%20suggestions%2Fcomments">privacidad@CONTECON.com</a> </li>
        <li>Tel&eacute;fono: +593 (04) 6006300&nbsp; - 3901700 Ext. 7003&nbsp; </li>
      </ul>
      <p align="justify">Declaro:&nbsp; que he le&iacute;do la Pol&iacute;tica de Protecci&oacute;n y  Tratamiento de Datos personales de Contecon Guayaquil S.A., por lo que acepto  su contenido y alcance, por estar acorde con la LOPDP.</p>
      <div>
        <div> </div>
        <div> </div>
    </div></td>
  </tr>
</table>

                  </div>
                  <div class="modal-footer"> 
                        <asp:Button ID="BtnGrabar" runat="server"  class="btn btn-primary"  Text="Aceptar"  OnClick="BtnGrabar_Click"  />
                        <asp:Button ID="btccerrar" runat="server"  class="btn btn-primary"  Text="&nbsp;&nbsp;&nbsp;Salir&nbsp;&nbsp;&nbsp;" OnClick="btccerrar_Click" />
                     
                  </div>
              </div>
         </div>
    </asp:Panel>
     <!-- Modal -->


     <!-- Modal -->

    <script src="https://code.jquery.com/jquery-3.5.1.slim.min.js" integrity="sha384-DfXdz2htPH0lsSSs5nCTpuj/zy4C+OGpamoFVy38MVBnE+IbbVYUew+OrCXaRkfj" crossorigin="anonymous"></script>
    <script>window.jQuery || document.write('<script src="../assets/js/vendor/jquery.slim.min.js"><\/script>')</script>
    <script src="../js/bootstrap.bundle.min.js"></script>
  <%--  <script src="https://cdnjs.cloudflare.com/ajax/libs/feather-icons/4.9.0/feather.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/Chart.js/2.7.3/Chart.min.js"></script>--%>
    <script src="../js/dashboard.js"></script>
</asp:Content>
