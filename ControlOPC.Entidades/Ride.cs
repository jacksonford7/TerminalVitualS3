using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace ControlOPC.Entidades
{
   public class Ride
    {
        public Int64 proformaID { get; set; }
        public string numero { get; set; }
        public string ruc { get; set; }
        public string razonSocial { get; set; }
        public string nombreComercial { get; set; }
        public string fechaEmision { get; set; }
        public string totalSinImpuestos { get; set; }
        public string importeTotal { get; set; }
        public string estab { get; set; }
        public string ptoEmi { get; set; }
        public string secuencial { get; set; }

        public Ride(Int64 pId)
        {
            this.proformaID = pId;
        }

     
       

        public static Ride getForXml(string xml, Int64 prof_id, out string novedad)
        {
            if (string.IsNullOrEmpty(xml))
            {
                novedad = "XML cadena vacía";
                return null;
            }
            var xi = new XDocument();
          
            try
            {
                //xDx.LoadXml(xml);
              xi=  XDocument.Parse(xml);
            }
            catch(Exception ex)
            {
                novedad = "El archivo no tiene un formato XML válido";
                return null;
            }
            
            XElement comprobante = xi.Descendants().Where(x => x.Name.LocalName.ToLower().Equals("comprobante")).FirstOrDefault();
            if (comprobante == null)
            {
                novedad = "El archivo no contiene el tag comprobante que es obligatorio";
                return null;
            }

            xi = XDocument.Parse(comprobante.Value);

            XElement puntero;
            //recuperar nofacr
            puntero = xi.Descendants().Where(x => x.Name.LocalName.ToLower().Equals("ruc")).FirstOrDefault();
            Ride f = new Ride(prof_id);
            f.ruc = puntero?.Value;

            puntero = xi.Descendants().Where(x => x.Name.LocalName.ToLower().Equals("razonsocial")).FirstOrDefault();
            f.razonSocial = puntero?.Value;

            puntero = xi.Descendants().Where(x => x.Name.LocalName.ToLower().Equals("nombrecomercial")).FirstOrDefault();
            f.nombreComercial = puntero?.Value;

            puntero = xi.Descendants().Where(x => x.Name.LocalName.ToLower().Equals("fechaemision")).FirstOrDefault();
            f.fechaEmision = puntero?.Value;


            puntero = xi.Descendants().Where(x => x.Name.LocalName.ToLower().Equals("totalsinimpuestos")).FirstOrDefault();
            f.totalSinImpuestos = puntero?.Value;


            puntero = xi.Descendants().Where(x => x.Name.LocalName.ToLower().Equals("importetotal")).FirstOrDefault();
            f.importeTotal = puntero?.Value;


            puntero = xi.Descendants().Where(x => x.Name.LocalName.ToLower().Equals("estab")).FirstOrDefault();
            f.estab = puntero?.Value;

            puntero = xi.Descendants().Where(x => x.Name.LocalName.ToLower().Equals("ptoemi")).FirstOrDefault();
            f.ptoEmi = puntero?.Value;

            puntero = xi.Descendants().Where(x => x.Name.LocalName.ToLower().Equals("secuencial")).FirstOrDefault();
            f.secuencial = puntero?.Value;

            f.numero = string.Format("{0}{1}{2}",f.estab,f.ptoEmi,f.secuencial);

            novedad = string.Empty;
            return f;
        }

    }

   



}
