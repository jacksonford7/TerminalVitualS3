using System.Runtime.Serialization;

namespace ServicioEcuapass.Contratos
{
    [DataContract]
    public class EstadoFactura
    {
        [DataMember]
        public bool FueOk { get; set; }
        [DataMember]
        public string Mensaje { get; set; }
        [DataMember]
        public decimal ValorFactura { get; set; }
        [DataMember]
        public decimal ValorPagado { get; set; }
        [DataMember]
        public decimal ValorPendiente { get; set; }
    }
}