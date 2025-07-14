using System.Runtime.Serialization;

namespace ServicioEcuapass.Contratos
{
    [DataContract]
    public class Respuesta
    {
        [DataMember]
        public bool FueOk { get; set; }
        [DataMember]
        public string Mensaje { get; set; }
    }
}