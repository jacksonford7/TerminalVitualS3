﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BillionEntidades.Factura
{

    [XmlRoot(ElementName = "totalConImpuestos")]
    public class TotalConImpuestos
    {

        [XmlElement(ElementName = "totalImpuesto")]
        public TotalImpuesto TotalImpuesto { get; set; }
    }
}
