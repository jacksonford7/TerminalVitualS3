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
    public class TurnoVBS: ModuloBase
    {
        public int IdPlan { get; set; }
        public int Secuencia { get; set; }
        public DateTime Inicio { get; set; }
        public DateTime Fin { get; set; }
        public string Turno { get; set; }
        public TurnoVBS() : base()
        {

        }
        public override void OnInstanceCreate()
        {
            this.alterClase = "PASEPTA";
            base.OnInstanceCreate();
            this.Accesorio.ConfiguracionBase = "DATACON";
        }
        //obtener la lista de turnos
        public static ResultadoOperacion<List<TurnoVBS>> ObtenerTurnos(string usuario,string contenedor, Int64 gkey, DateTime dia)
        {
            var p = new TurnoVBS();
            p.actualMetodo = MethodBase.GetCurrentMethod().Name;
            string pv;
            if (!p.Accesorio.Inicializar(out pv))
            {
                return ResultadoOperacion<List<TurnoVBS>>.CrearFalla(pv);
            }
            p.Parametros.Clear();
            p.Parametros.Add("FECHA", dia.Date);
            p.Parametros.Add("CONSECUTIVO", gkey);
            p.Parametros.Add("CONTAINER", contenedor);
            var bcon = p.Accesorio.ObtenerConfiguracion("N4Middleware")?.valor;
#if DEBUG
            p.LogEvent(usuario, p.actualMetodo, usuario);
#endif
            var rp = BDOpe.ComandoSelectALista<TurnoVBS>(bcon, "[Bill].[turnos_disponibles]", p.Parametros);

            //la lista de turnos son los turnos VBS  del dia actual, que no tienen reserva y ademas no estan en pendientes
            //con esta lista debo eliminar aquellos que estan en temporal

            return rp.Exitoso ? ResultadoOperacion<List<TurnoVBS>>.CrearResultadoExitoso(rp.Resultado) : ResultadoOperacion<List<TurnoVBS>>.CrearFalla(rp.MensajeProblema);
        }




    }
}
