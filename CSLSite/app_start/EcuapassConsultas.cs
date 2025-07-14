using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using CSLSite.app_start;

namespace CSLSite.app_start
{
    public class EcuapassConsulta
    {
        string oruta;
        public EcuapassConsulta(string ruta)
        {
            //ruta fisica del archivo
            oruta = ruta;
        }

        //Genera Todo el XML para la consulta de DAE
        public string ConsultaDAE(string dae)
        {
            var sbd = new StringBuilder();
            sbd.Append("<SOAPENV:Envelope xmlns:SOAPENV=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:web=\"http://webservice.ecl.dclt.ecuapass.aduana.gob.ec/\"><SOAPENV:Header><ns0:Header xmlns:ns0=\"http://soapinterop.org/xsd\">");
            sbd.AppendFormat("<ns0:DclrRuc>{0}</ns0:DclrRuc>",string.IsNullOrEmpty( this.ruc_oce)? "0992506717001":this.ruc_oce);
            sbd.AppendFormat("<ns0:DclrCd>{0}</ns0:DclrCd>",string.IsNullOrEmpty(codigo_oce)? "05909025":codigo_oce);
            sbd.Append("<ns0:DclrSeCd>14</ns0:DclrSeCd>");
            sbd.AppendFormat("<ns0:DclrSeId>{0}</ns0:DclrSeId>",string.IsNullOrEmpty(usuario_ecuapass)? "HFABAD":usuario_ecuapass);
            
            sbd.Append("<ns0:DocPrcsType>O</ns0:DocPrcsType>");
            sbd.Append("<ns0:RcsdEdocAfrCd>002</ns0:RcsdEdocAfrCd>");
            sbd.Append("<ns0:RcsdEdocTypeCd>002</ns0:RcsdEdocTypeCd>");

            sbd.Append("<ns0:SmtNo></ns0:SmtNo>");
            sbd.Append("<ns0:UserId></ns0:UserId>");
            var ecu_ecp = new EcuapassCryptoSign(oruta);

            //nuevo
            var managed = EcuapassCryptoSign.Generate_Passw_New();
            var clv = EcuapassCryptoSign.RSAEncrypt_Pad_New(managed.Key, false);

            sbd.AppendFormat("<ns0:SecLv1>{0}</ns0:SecLv1>", EcuapassCryptoSign.AES_Encrypt_New(this.usuario_ecuapass, managed));
            sbd.AppendFormat("<ns0:SecLv2>{0}</ns0:SecLv2>", EcuapassCryptoSign.AES_Encrypt_New(this.clave_ecuapass, managed));
            sbd.AppendFormat("<ns0:SecLv3>{0}</ns0:SecLv3>", clv);

           //sbd.AppendFormat("<ns0:SecLv1>{0}</ns0:SecLv1>", ecu_ecp.CadenaEncript(this.usuario_ecuapass));
           // sbd.AppendFormat("<ns0:SecLv2>{0}</ns0:SecLv2>", ecu_ecp.CadenaEncript(this.clave_ecuapass));
           // sbd.AppendFormat("<ns0:SecLv3>{0}</ns0:SecLv3>", ecu_ecp.clave);
            sbd.Append("</ns0:Header>");
            sbd.Append("</SOAPENV:Header>");


            sbd.Append("<SOAPENV:Body><web:requestExportDespachoData><arg0>");
            sbd.Append("<![CDATA[<DocumentMetadata xsi:schemaLocation=\"urn:wco:datamodel:EC:SDEX:1:0:0 EC_SDEX_0p1.03.xsd\" xmlns=\"urn:wco:datamodel:EC:SDEX:1:0:0\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"><WCODataModelVersion>3.0</WCODataModelVersion>");
            sbd.Append("<WCODocumentName>DEX</WCODocumentName><CountryCode>EC</CountryCode><AgencyName>SENAE</AgencyName>");
            sbd.Append("<AgencyAssignedCountrySubEntityID>String</AgencyAssignedCountrySubEntityID>");
            sbd.Append("<AgencyAssignedCustomizedDocumentName>SDEX</AgencyAssignedCustomizedDocumentName>");
            sbd.Append("<AgencyAssignedCustomizedDocumentVersion>1.0</AgencyAssignedCustomizedDocumentVersion>");
            sbd.Append("<Declaration><TypeCode>002</TypeCode>");
            sbd.Append("<Agent>");
            sbd.AppendFormat("<OceCD>{0}</OceCD>", this.codigo_oce);
            sbd.AppendFormat("<UserID>{0}</UserID>",this.usuario_ecuapass);
            sbd.Append("</Agent>");

            sbd.AppendFormat("<SequenceNumeric>{0}</SequenceNumeric>", 1);
            sbd.Append("<Consignment><ConsultTypeID>03</ConsultTypeID><DeclarationOfficeID>028</DeclarationOfficeID><ExporterDocumentID/>");//03

            sbd.Append("<DeclarationNumber>");
             sbd.AppendFormat("<DeclarationNumberID>{0}</DeclarationNumberID>",dae);



            sbd.Append("</DeclarationNumber>");

            //sbd.Append("<Period><StartDate/><EndDate/></Period>");
            sbd.Append("<Period>");
            sbd.AppendFormat("<StartDate>{0}</StartDate>", DateTime.Now.AddDays(-30).ToString("yyyy-MM-dd"));
            sbd.AppendFormat("<EndDate>{0}</EndDate>", DateTime.Now.AddDays(30).ToString("yyyy-MM-dd"));
            sbd.Append("</Period>");

            sbd.Append("</Consignment></Declaration></DocumentMetadata>]]>");
            sbd.Append("</arg0>");
            sbd.Append("</web:requestExportDespachoData>");
            sbd.Append("</SOAPENV:Body>");
            sbd.Append("</SOAPENV:Envelope>");
            

            return sbd.ToString();
        }

        //Genera Todo el XML para la consulta de DAE
        public string ConsultaDAE(DateTime desde, DateTime hasta)
        {

            return null;
        }


        public string ruc_oce { get; set; }
        public string usuario_ecuapass { get; set; }
        public string clave_ecuapass { get; set; }
        public string codigo_oce { get; set; }
   

    }

    
}