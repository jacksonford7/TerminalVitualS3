using System;
using System.Runtime.Serialization;

namespace ServicioEcuapass.Contratos
{
    [DataContract]
    public class Pago
    {
        [DataMember]
        public string NumeroLiquidacion { get; set; }
        [DataMember]
        public string TipoTransaccion { get; set; }
        [DataMember]
        public DateTime FechaRecaudacion { get; set; }
        [DataMember]
        public DateTime FechaContable { get; set; }
        [DataMember]
        public decimal MontoRecaudado { get; set; }
        [DataMember]
        public string CodigoAceptacionPago { get; set; }
        [DataMember]
        public string CodigoPago { get; set; }
        [DataMember]
        public string BancoPago { get; set; }
        [DataMember]
        public string CanalRecaudacion { get; set; }
    }
}