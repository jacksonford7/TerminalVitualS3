using System;
using ServicioEcuapass.AccesoDatos;
using ServicioEcuapass.Contratos;

namespace ServicioEcuapass.Bases
{
    public class CoreBase : IDisposable
    {
        internal ecuapassEntities Contexto;
        internal N4MiddlewareEntities ContextoN4Middleware;
        private bool _disposed;

        internal CoreBase()
        {
            Contexto = new ecuapassEntities();
            ContextoN4Middleware = new N4MiddlewareEntities();
        }

        internal Respuesta RegistrarExcepcion(Exception exception, string usuario)
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
                return new Respuesta { Mensaje = string.Format("Ha ocurrido un inconveniente, reportelo con este código : {0}", objeto.ID) };
            }
            catch (Exception ex)
            {
                return new Respuesta { Mensaje = string.Format("Ha ocurrido un inconveniente y no se ha podido loguearlo.\nEXCEPCION INTERNA : {0}///DATA : {1}///FUENTE : {2}///SITIO_DESTINO : {3}///SEGUIMIENTO DE PILA : {4}", ex.InnerException, ex.Data, ex.Source, ex.TargetSite, ex.StackTrace) };
            }
        }

        internal Respuesta RegistrarExcepcionAduana(Exception exception)
        {
            try
            {
                var objeto = new ECU_LIQUIDACION_LOG_WCF
                {
                    DETALLE = string.Format("EXCEPCION INTERNA : {0}///DATA : {1}///FUENTE : {2}///SITIO_DESTINO : {3}///SEGUIMIENTO DE PILA : {4}", exception.InnerException, exception.Data, exception.Source, exception.TargetSite, exception.StackTrace),
                    FECHA = DateTime.Now,
                    MENSAJE = exception.Message,
                    USUARIO = "SERVICIO PEL",
                    ES_SERVICIO_ADUANA = true
                };
                Contexto.ECU_LIQUIDACION_LOGS_WCF.AddObject(objeto);
                Contexto.SaveChanges();
                return new Respuesta { Mensaje = string.Format("Ha ocurrido un inconveniente, reportelo con este código : {0}", objeto.ID) };
            }
            catch (Exception ex)
            {
                return new Respuesta { Mensaje = string.Format("Ha ocurrido un inconveniente y no se ha podido loguearlo.\nEXCEPCION INTERNA : {0}///DATA : {1}///FUENTE : {2}///SITIO_DESTINO : {3}///SEGUIMIENTO DE PILA : {4}", ex.InnerException, ex.Data, ex.Source, ex.TargetSite, ex.StackTrace) };
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;
            if (disposing)
            {
                Contexto.Dispose();
                ContextoN4Middleware.Dispose();
            }
            _disposed = true;
        }
    }
}