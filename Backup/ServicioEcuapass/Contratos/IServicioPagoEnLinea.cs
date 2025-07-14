using System.ServiceModel;

namespace ServicioEcuapass.Contratos
{
    [ServiceContract]
    public interface IServicioPagoEnLinea
    {
        [OperationContract]
        Respuesta IngresoPago(Pago pago);
    }
}
