using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Xml;
using System.IO;

namespace CSLSite
{
    public class IntegracionService
    {

        public string enviarIIE(string scope, string tipo, string usuario, string validar, string gkey, string peso, string documento, string aisv = null, string descripcion = null, string embalaje = null)
        {
            string retorno = string.Empty;
            try
            {
                using (n4Service.n4ServiceSoapClient client = new n4Service.n4ServiceSoapClient())
                {
                    string xmlRespuesta = this.generarXmlIIE(tipo,usuario,validar,gkey,peso,documento,aisv,descripcion,embalaje);
                    retorno = client.basicInvoke(scope, xmlRespuesta);
                    //listaClientes = this.generarEstructuraDesdeXMLVendedores(retorno);
                    client.Close();
                }
            }
            catch (Exception ex)
            {
                retorno = ex.ToString();
            }

            return retorno;
        }


        public string generarXmlIIE(string tipo, string usuario, string validar, string gkey, string peso, string documento, string aisv = null, string descripcion = null, string embalaje = null)
        {
            XmlDocument doc = new XmlDocument();
            XmlDeclaration xmlDeclaration = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
            doc.AppendChild(xmlDeclaration);

            XmlElement root = doc.CreateElement(string.Empty, "iie", string.Empty);
            XmlElement nodoTipo = doc.CreateElement(string.Empty, "tipo", string.Empty);
            XmlElement nodoUsuario = doc.CreateElement(string.Empty, "usuario", string.Empty);
            XmlElement nodoValidar = doc.CreateElement(string.Empty, "validar", string.Empty);
            XmlElement nodoParametros = doc.CreateElement(string.Empty, "parametros", string.Empty);
            XmlElement nodoGkey = doc.CreateElement(string.Empty, "gkey", string.Empty);
            XmlElement nodoPeso = doc.CreateElement(string.Empty, "peso", string.Empty);
            XmlElement nodoDocumento = doc.CreateElement(string.Empty, "documento", string.Empty);
           
            XmlText xmlTipo = doc.CreateTextNode(tipo);
            XmlText xmlUsuario = doc.CreateTextNode(usuario);
            XmlText xmlValidar = doc.CreateTextNode(validar);
            XmlText xmlGkey = doc.CreateTextNode(gkey);
            XmlText xmlPeso = doc.CreateTextNode(peso);
            XmlText xmlDocumento = doc.CreateTextNode(documento);
            nodoTipo.AppendChild(xmlTipo);
            nodoUsuario.AppendChild(xmlUsuario);
            nodoValidar.AppendChild(xmlValidar);
            nodoGkey.AppendChild(xmlGkey);
            nodoPeso.AppendChild(xmlPeso);
            nodoDocumento.AppendChild(xmlDocumento);
            nodoParametros.AppendChild(nodoGkey);
            nodoParametros.AppendChild(nodoPeso);

            if (aisv != null)
            {
                XmlElement nodoAisv = doc.CreateElement(string.Empty, "aisv", string.Empty);
                XmlText xmlAisv = doc.CreateTextNode(aisv);
                nodoAisv.AppendChild(xmlAisv);
                nodoParametros.AppendChild(nodoAisv);
            }

            nodoParametros.AppendChild(nodoDocumento);

            if (descripcion != null)
            {
                XmlElement nodoDescripcion = doc.CreateElement(string.Empty, "descripcion", string.Empty);
                XmlText xmlDescripcion = doc.CreateTextNode(descripcion);
                nodoDescripcion.AppendChild(xmlDescripcion);
                nodoParametros.AppendChild(nodoDescripcion);
            }

            if (embalaje != null)
            {
                XmlElement nodoEmbalaje = doc.CreateElement(string.Empty, "embalaje", string.Empty);
                XmlText xmlEmbalaje = doc.CreateTextNode(embalaje);
                nodoEmbalaje.AppendChild(xmlEmbalaje);
                nodoParametros.AppendChild(nodoEmbalaje);
            }
            
            root.AppendChild(nodoTipo);
            root.AppendChild(nodoUsuario);
            root.AppendChild(nodoValidar);
            root.AppendChild(nodoParametros);
            doc.AppendChild(root);

            string xmlRespuesta = string.Empty;
            using (var stringWriter = new StringWriter())
            {
                using (var xmlTextWriter = XmlWriter.Create(stringWriter))
                {
                    doc.WriteTo(xmlTextWriter);
                    xmlTextWriter.Flush();
                    xmlRespuesta = stringWriter.GetStringBuilder().ToString();
                }
            }

            return xmlRespuesta;
        }

    }
}