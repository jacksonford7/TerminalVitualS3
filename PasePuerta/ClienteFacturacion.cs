using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AccesoDatos;
using Configuraciones;
using Respuesta;

namespace PasePuerta
{
    public class ClienteFacturacion : ModuloBase
    {
        public override void OnInstanceCreate()
        {
            this.alterClase = "PASEPTA";
            base.OnInstanceCreate();
            this.Accesorio.ConfiguracionBase = "DATACON";
        }

        public static ResultadoOperacion<bool> BloqueoTesoreria(string usuario, string ruc)
        {
            var p = new ClienteFacturacion();
            p.actualMetodo = MethodBase.GetCurrentMethod().Name;
            string pv;
            if (!p.Accesorio.Inicializar(out pv))
            {
                return ResultadoOperacion<bool>.CrearFalla(pv);
            }
            p.Parametros.Clear();
            p.Parametros.Add("id", ruc);
            var bcon = p.Accesorio.ObtenerConfiguracion("N4Middleware")?.valor;
#if DEBUG
            p.LogEvent(usuario, p.actualMetodo, usuario);
#endif
            var rp = BDOpe.ComandoSelectEscalar<bool>(bcon, "select [Bill].[bloqueo_tesoreria](@id)", p.Parametros);

            //la lista de turnos son los turnos VBS  del dia actual, que no tienen reserva y id no estan en pendientes
            //con esta lista debo eliminar aquellos que estan en temporal
            return rp.Exitoso ? ResultadoOperacion<bool>.CrearResultadoExitoso(rp.Resultado) : ResultadoOperacion<bool>.CrearFalla(rp.MensajeProblema);
        }


        public static ResultadoOperacion<bool> ClienteLiberado(string usuario, string ruc)
        {
            var p = new ClienteFacturacion();
            p.actualMetodo = MethodBase.GetCurrentMethod().Name;
            string pv;
            if (!p.Accesorio.Inicializar(out pv))
            {
                return ResultadoOperacion<bool>.CrearFalla(pv);
            }
            p.Parametros.Clear();
            p.Parametros.Add("id", ruc);
            var bcon = p.Accesorio.ObtenerConfiguracion("N4Middleware")?.valor;
#if DEBUG
            p.LogEvent(usuario, p.actualMetodo, usuario);
#endif
            var rp = BDOpe.ComandoSelectEscalar<bool>(bcon, "select [Bill].[cliente_liberado](@id)", p.Parametros);

            //la lista de turnos son los turnos VBS  del dia actual, que no tienen reserva y ademas no estan en pendientes
            //con esta lista debo eliminar aquellos que estan en temporal
            return rp.Exitoso ? ResultadoOperacion<bool>.CrearResultadoExitoso(rp.Resultado) : ResultadoOperacion<bool>.CrearFalla(rp.MensajeProblema);
        }

    }
}
