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
    public class TurnoBRBK : ModuloBase
    {
        public Int64 IdPlan { get; set; }
        public Int64? Secuencia { get; set; }
        public DateTime Inicio { get; set; }
        public DateTime Fin { get; set; }
        public string Turno { get; set; }
        public int? TotalBultos { get; set; }
        public int? Bultos { get; set; }
        public bool? Verficado { get; set; }
        


        public TurnoBRBK() : base()
        {

        }
        public override void OnInstanceCreate()
        {
            this.alterClase = "PASEPTA";
            base.OnInstanceCreate();
            this.Accesorio.ConfiguracionBase = "DATACON";
        }
        //obtener la lista de turnos
        public static ResultadoOperacion<List<TurnoBRBK>> ObtenerTurnos(string usuario,DateTime fecha, string ubicacion, int idProducto)
        {
            var p = new TurnoBRBK();
            p.actualMetodo = MethodBase.GetCurrentMethod().Name;
            string pv;
            if (!p.Accesorio.Inicializar(out pv))
            {
                return ResultadoOperacion<List<TurnoBRBK>>.CrearFalla(pv);
            }
            p.Parametros.Clear();
            p.Parametros.Add("fecha", fecha.Date);
            p.Parametros.Add("ubicacion", ubicacion);
            p.Parametros.Add("idProducto", idProducto);
            

            var tt = p.SetMessage("NO_NULO", p.actualMetodo, usuario);
            if (string.IsNullOrEmpty(usuario))
            {
                return Respuesta.ResultadoOperacion<List<TurnoBRBK>>.CrearFalla(string.Format(tt.Item1, nameof(usuario)));
            }
            

            var bcon = p.Accesorio.ObtenerConfiguracion("N4Middleware")?.valor;
#if DEBUG
            p.LogEvent(usuario, p.actualMetodo, usuario);
#endif
            var rp = BDOpe.ComandoSelectALista<TurnoBRBK>(bcon, "[Bill].[brbk_turnos_disponibles]", p.Parametros);
            return rp.Exitoso ? ResultadoOperacion<List<TurnoBRBK>>.CrearResultadoExitoso(rp.Resultado) : ResultadoOperacion<List<TurnoBRBK>>.CrearFalla(rp.MensajeProblema);
        }



        //obtener la lista de turnos
        public static ResultadoOperacion<List<TurnoBRBK>> ObtenerTurnosSolicitud(string usuario, DateTime fecha, Int64 tipo)
        {
            var p = new TurnoBRBK();
            p.actualMetodo = MethodBase.GetCurrentMethod().Name;
            string pv;
            if (!p.Accesorio.Inicializar(out pv))
            {
                return ResultadoOperacion<List<TurnoBRBK>>.CrearFalla(pv);
            }
            p.Parametros.Clear();
            p.Parametros.Add("fecha", fecha.Date);
            p.Parametros.Add("tipo", tipo);
            var tt = p.SetMessage("NO_NULO", p.actualMetodo, usuario);
            if (string.IsNullOrEmpty(usuario))
            {
                return Respuesta.ResultadoOperacion<List<TurnoBRBK>>.CrearFalla(string.Format(tt.Item1, nameof(usuario)));
            }


            var bcon = p.Accesorio.ObtenerConfiguracion("N4Middleware")?.valor;
#if DEBUG
            p.LogEvent(usuario, p.actualMetodo, usuario);
#endif
            var rp = BDOpe.ComandoSelectALista<TurnoBRBK>(bcon, "[Bill].[brbk_turnos_solicitud_disponibles]", p.Parametros);
            return rp.Exitoso ? ResultadoOperacion<List<TurnoBRBK>>.CrearResultadoExitoso(rp.Resultado) : ResultadoOperacion<List<TurnoBRBK>>.CrearFalla(rp.MensajeProblema);
        }

    }
}
