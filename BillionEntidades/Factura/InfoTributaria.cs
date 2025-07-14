using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BillionEntidades.Factura
{

	[XmlRoot(ElementName = "infoTributaria")]
	public class InfoTributaria
	{

		[XmlElement(ElementName = "razonSocial")]
		public string RazonSocial { get; set; }

		[XmlElement(ElementName = "nombreComercial")]
		public string NombreComercial { get; set; }

		[XmlElement(ElementName = "ruc")]
		public string Ruc { get; set; }

		[XmlElement(ElementName = "codDoc")]
		public string CodDoc { get; set; }

		[XmlElement(ElementName = "estab")]
		public string Estab { get; set; }

		[XmlElement(ElementName = "ptoEmi")]
		public string PtoEmi { get; set; }

		[XmlElement(ElementName = "secuencial")]
		public string Secuencial { get; set; }

		[XmlElement(ElementName = "dirMatriz")]
		public string DirMatriz { get; set; }
	}
}
