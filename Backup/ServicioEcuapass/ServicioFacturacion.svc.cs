using System;
using System.Linq;
using System.Transactions;
using ServicioEcuapass.AccesoDatos;
using ServicioEcuapass.Bases;
using ServicioEcuapass.Contratos;

namespace ServicioEcuapass
{
    public class ServicioFacturacion : CoreBase, IServicioFacturacion
    {
        public EstadoFactura ObtenerEstadoFactura(string numeroLiquidacion, string usuario)
        {
            try
            {
                return ObtenerFactura(numeroLiquidacion, usuario);
            }
            catch (Exception ex)
            {
                return RegistrarExcepcionEstadoFactura(ex, usuario);
            }
        }

        private EstadoFactura ObtenerFactura(string numeroLiquidacion, string usuario)
        {
            var factura = Contexto.ECU_LIQUIDACION_SENAE.Include("ECU_LIQUIDACION_PAGO_SENAE").FirstOrDefault(f => f.NUMERO_LIQUIDACION == numeroLiquidacion);
            if (factura == null)
            {
                GrabarConsulta(new ECU_CONSULTAS_ESTADO_FACTURA
                {
                    FECHA = DateTime.Now,
                    MENSAJE = "No existe factura.",
                    NUMERO_LIQUIDACION = numeroLiquidacion,
                    USUARIO = usuario
                });
                return new EstadoFactura { Mensaje = "No existe factura." };
            }
            return ObtenerRespuestaExitosa(factura, numeroLiquidacion, usuario);
        }

        private EstadoFactura ObtenerRespuestaExitosa(ECU_LIQUIDACION_SENAE factura, string numeroLiquidacion, string usuario)
        {
            var respuesta = new EstadoFactura
            {
                FueOk = true,
                Mensaje = "",
                ValorFactura = Math.Round(factura.MONTO_TOTAL, 2),
                ValorPagado = Math.Round(factura.ECU_LIQUIDACION_PAGO_SENAE.Sum(p => p.MONTO_RECAUDADO.HasValue ? p.MONTO_RECAUDADO.Value : 0), 2),
                ValorPendiente = Math.Round(factura.MONTO_TOTAL - factura.ECU_LIQUIDACION_PAGO_SENAE.Sum(p => p.MONTO_RECAUDADO.HasValue ? p.MONTO_RECAUDADO.Value : 0), 2)
            };
            GrabarConsulta(new ECU_CONSULTAS_ESTADO_FACTURA
            {
                FUE_OK = true,
                FECHA = DateTime.Now,
                MENSAJE = "",
                NUMERO_LIQUIDACION = numeroLiquidacion,
                USUARIO = usuario,
                VALOR_FACTURA = respuesta.ValorFactura,
                VALOR_PAGADO = respuesta.ValorPagado,
                VALOR_PENDIENTE = respuesta.ValorPendiente
            });
            return respuesta;
        }

        private void GrabarConsulta(ECU_CONSULTAS_ESTADO_FACTURA consulta)
        {
            Contexto.AddToECU_CONSULTAS_ESTADO_FACTURAS(consulta);
            Contexto.SaveChanges();
        }

        public Respuesta CambiarEstadoFactura(string numeroLiquidacion, bool esReverso, string usuario)
        {
            try
            {
                using (var transaccion = new TransactionScope())
                {
                    return ObtenerFactura(transaccion, numeroLiquidacion, esReverso, usuario);
                }
            }
            catch (Exception ex)
            {
                return RegistrarExcepcion(ex, usuario);
            }
        }

        private Respuesta ObtenerFactura(TransactionScope transaccion, string numeroLiquidacion, bool esReverso, string usuario)
        {
            var factura = Contexto.ECU_LIQUIDACION_SENAE.FirstOrDefault(f => f.NUMERO_LIQUIDACION == numeroLiquidacion);
            return factura == null ? new Respuesta { Mensaje = "No existe factura." } : ActualizarFactura(factura, transaccion, esReverso, usuario);
        }

        private Respuesta ActualizarFactura(ECU_LIQUIDACION_SENAE factura, TransactionScope transaccion, bool esReverso, string usuario)
        {
            factura.TRAMITE = "M";
            factura.ESTADO_PROCESO = "A";
            factura.ESTADO = esReverso ? "C" : "D";
            Contexto.SaveChanges();
            GrabarEstado(factura, usuario);
            transaccion.Complete();
            return new Respuesta { FueOk = true, Mensaje = "Proceso OK." };
        }

        private void GrabarEstado(ECU_LIQUIDACION_SENAE factura, string usuario)
        {
            Contexto.ECU_LIQUIDACION_ESTADO.AddObject(new ECU_LIQUIDACION_ESTADO
            {
                CODIGO_UNICO = factura.CODIGO_UNICO,
                ESTADO = factura.ESTADO,
                FECHA = DateTime.Now,
                NUMERO_LIQUIDACION = factura.NUMERO_LIQUIDACION,
                USUARIO = usuario
            });
            Contexto.SaveChanges();
        }

        public Respuesta IngresoPago(string numeroLiquidacion, decimal monto, string codigoBanco)
        {
            try
            {
                return ObtenerLiquidacion(numeroLiquidacion, monto, codigoBanco);
            }
            catch (Exception ex)
            {
                return RegistrarExcepcion(ex, "FACTURACION");
            }
        }

        private Respuesta ObtenerLiquidacion(string numeroLiquidacion, decimal monto, string codigoBanco)
        {
            var liquidacion = Contexto.ECU_LIQUIDACION_SENAE.FirstOrDefault(l => l.NUMERO_LIQUIDACION == numeroLiquidacion);
            return liquidacion == null ? new Respuesta { Mensaje = "No se encontro identificacion del cliente." } : ObtenerBanco(liquidacion, monto, codigoBanco);
        }

        private Respuesta ObtenerBanco(ECU_LIQUIDACION_SENAE liquidacion, decimal monto, string codigoBanco)
        {
            var banco = Contexto.ECU_LIQUIDACION_BANCOS.FirstOrDefault(b => b.BANCO_BANRED == codigoBanco);
            return banco == null ? new Respuesta { Mensaje = "No se encontro datos del banco." } : ObtenerCodigoSap(liquidacion, monto, banco);
        }

        private Respuesta ObtenerCodigoSap(ECU_LIQUIDACION_SENAE liquidacion, decimal monto, ECU_LIQUIDACION_BANCOS banco)
        {
            var clienteSap = (from x in ContextoN4Middleware.CLIENTS_BILL where x.CLNT_CUSTOMER == liquidacion.NUMERO_IDENTIFICACION && x.ROLE == liquidacion.ROL select x.CODIGO_SAP).FirstOrDefault();
            return clienteSap == null ? ObtenerCodigoSap2(liquidacion, monto, banco) : RegistrarPago(clienteSap, liquidacion, monto, banco);
        }

        private Respuesta ObtenerCodigoSap2(ECU_LIQUIDACION_SENAE liquidacion, decimal monto, ECU_LIQUIDACION_BANCOS banco)
        {
            var clienteSap = (from x in ContextoN4Middleware.CLIENTS_BILL_SAP where x.CLNT_CUSTOMER == liquidacion.NUMERO_IDENTIFICACION select x.CODIGO_SAP).FirstOrDefault();
            return RegistrarPago(clienteSap, liquidacion, monto, banco);
        }

        private Respuesta RegistrarPago(string clienteSap, ECU_LIQUIDACION_SENAE liquidacion, decimal monto, ECU_LIQUIDACION_BANCOS banco)
        {
            Contexto.IngresoPago(liquidacion.CODIGO_UNICO.ToString(), liquidacion.NUMERO_LIQUIDACION, monto,
                                 "VENTANILLA",
                                 banco.BANCO_SENAE, DateTime.Now, liquidacion.CODIGO_UNICO.ToString(),
                                 DateTime.Now, "",
                                 clienteSap, liquidacion.NUMERO_IDENTIFICACION);
            return new Respuesta { FueOk = true };
        }

        private EstadoFactura RegistrarExcepcionEstadoFactura(Exception exception, string usuario)
        {
            try
            {
                var objeto = new ECU_LIQUIDACION_LOG_WCF
                {
                    DETALLE = string.Format("EXCEPCION INTERNA : {0}///DATA : {1}///FUENTE : {2}///SITIO_DESTINO : {3}///SEGUIMIENTO DE PILA : {4}", exception.InnerException, exception.Data, exception.Source, exception.TargetSite, exception.StackTrace),
                    FECHA = DateTime.Now,
                    MENSAJE = exception.Message,
                    USUARIO = usuario
                };
                Contexto.ECU_LIQUIDACION_LOGS_WCF.AddObject(objeto);
                Contexto.SaveChanges();
                return new EstadoFactura { Mensaje = string.Format("Ha ocurrido un inconveniente, reportelo con este código : {0}", objeto.ID) };
            }
            catch (Exception ex)
            {
                return new EstadoFactura { Mensaje = string.Format("Ha ocurrido un inconveniente y no se ha podido loguearlo.\nEXCEPCION INTERNA : {0}///DATA : {1}///FUENTE : {2}///SITIO_DESTINO : {3}///SEGUIMIENTO DE PILA : {4}", ex.InnerException, ex.Data, ex.Source, ex.TargetSite, ex.StackTrace) };
            }
        }
    }
}
