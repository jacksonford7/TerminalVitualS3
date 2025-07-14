using System.ServiceModel;

namespace ServicioEcuapass.Contratos
{
    [ServiceContract]
    public interface IServicioFacturacion
    {
        [OperationContract]
        EstadoFactura ObtenerEstadoFactura(string numeroLiquidacion, string usuario);

        [OperationContract]
        Respuesta CambiarEstadoFactura(string numeroLiquidacion, bool esReverso, string usuario);

        [OperationContract]
        Respuesta IngresoPago(string numeroLiquidacion, decimal monto, string codigoBanco);
    }
}
