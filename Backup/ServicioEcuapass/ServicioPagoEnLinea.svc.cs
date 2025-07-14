using System;
using System.Linq;
using ServicioEcuapass.Bases;
using ServicioEcuapass.Contratos;
using ServicioEcuapass.AccesoDatos;

namespace ServicioEcuapass
{
    public class ServicioPagoEnLinea : CoreBase, IServicioPagoEnLinea
    {
        public Respuesta IngresoPago(Pago pago)
        {
            try
            {
                return ObtenerLiquidacion(pago);
            }
            catch (Exception ex)
            {
                return RegistrarExcepcionAduana(ex);
            }
        }

        private Respuesta ObtenerLiquidacion(Pago pago)
        {
            var liquidacion = (from x in Contexto.ECU_LIQUIDACION_SENAE where x.NUMERO_LIQUIDACION == pago.NumeroLiquidacion select x).FirstOrDefault();
            return liquidacion == null ? new Respuesta { Mensaje = "No se encontro la liquidación en el sistema." } : ObtenerFactura(liquidacion, pago);
        }

        private Respuesta ObtenerFactura(ECU_LIQUIDACION_SENAE liquidacion, Pago pago)
        {
            var factura = (from f in Contexto.ECU_LIQUIDACION_SENAE where f.NUMERO_LIQUIDACION == pago.NumeroLiquidacion && f.CODIGO_UNICO <= 0 select f).FirstOrDefault();
            return (factura != null && factura.MONTO_TOTAL != pago.MontoRecaudado) ? new Respuesta { Mensaje = "El monto reacudado del anticipo no es igual al ingresado por el cliente en nuestro sistema." } : ObtenerCodigoSap(liquidacion, pago);
        }

        private Respuesta ObtenerCodigoSap(ECU_LIQUIDACION_SENAE liquidacion, Pago pago)
        {
            var clienteSap = (from x in ContextoN4Middleware.CLIENTS_BILL where x.CLNT_CUSTOMER == liquidacion.NUMERO_IDENTIFICACION && x.ROLE == liquidacion.ROL select x.CODIGO_SAP).FirstOrDefault();
            return clienteSap == null ? ObtenerCodigoSap2(liquidacion, pago) : ValidarTipoTransaccion(clienteSap, pago, liquidacion);
        }

        private Respuesta ObtenerCodigoSap2(ECU_LIQUIDACION_SENAE liquidacion, Pago pago)
        {
            var clienteSap = (from x in ContextoN4Middleware.CLIENTS_BILL_SAP where x.CLNT_CUSTOMER == liquidacion.NUMERO_IDENTIFICACION select x.CODIGO_SAP).FirstOrDefault();
            return ValidarTipoTransaccion(clienteSap, pago, liquidacion);
        }

        private Respuesta ValidarTipoTransaccion(string clienteSap, Pago pago, ECU_LIQUIDACION_SENAE liquidacion)
        {
            if(liquidacion.CODIGO_UNICO <= 0)
            {
                switch (pago.TipoTransaccion)
                {
                    case "P":
                        if (HayMayorNumeroDeRegistroQueReversos(pago))
                            return new Respuesta { Mensaje = string.Format("Ya se registro un pago previo al anticipo cuya liquidacion es : {0}", pago.NumeroLiquidacion) };
                        break;
                    case "R":
                        if (!HayMayorNumeroDeRegistroQueReversos(pago))
                            return new Respuesta { Mensaje = string.Format("Ya se registro un reverso previo al anticipo cuya liquidacion es : {0}", pago.NumeroLiquidacion) };
                        break;
                    default:
                        return new Respuesta { Mensaje = string.Format("Mal formato de campo Tipo Transacción.") };
                }
            }
            return RegistrarPago(clienteSap, pago, liquidacion);
        }

        private bool HayMayorNumeroDeRegistroQueReversos(Pago pago)
        {
            var numeroPagosNormales = Contexto.ECU_LIQUIDACION_PAGO_SENAE.Count(p => p.NUMERO_LIQUIDACION == pago.NumeroLiquidacion && p.CODIGO_FACTURA <= 0 && p.TIPO_TRANSACCION == "P");
            var numeroPagosReversos = Contexto.ECU_LIQUIDACION_PAGO_SENAE.Count(p => p.NUMERO_LIQUIDACION == pago.NumeroLiquidacion && p.CODIGO_FACTURA <= 0 && p.TIPO_TRANSACCION == "R");
            return numeroPagosNormales > numeroPagosReversos;
        }

        private Respuesta RegistrarPago(string clienteSap, Pago pago, ECU_LIQUIDACION_SENAE liquidacion)
        {
            Contexto.IngresoPago(pago.CodigoPago, pago.NumeroLiquidacion, pago.MontoRecaudado,
                                    pago.CanalRecaudacion,
                                    pago.BancoPago, pago.FechaRecaudacion, pago.CodigoAceptacionPago,
                                    pago.FechaContable, pago.TipoTransaccion,
                                    clienteSap, liquidacion.NUMERO_IDENTIFICACION);
            return new Respuesta { FueOk = true };
        }
    }
}
