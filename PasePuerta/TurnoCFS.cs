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
    public class TurnoCFS: ModuloBase
    {
        public Int64 IdPlan { get; set; }
        public int? Secuencia { get; set; }
        public DateTime Inicio { get; set; }
        public DateTime Fin { get; set; }
        public string Turno { get; set; }
        public int? TotalBultos { get; set; }
        public int? Bultos { get; set; }
        public bool? Verficado { get; set; }
        


        public TurnoCFS() : base()
        {

        }
        public override void OnInstanceCreate()
        {
            this.alterClase = "PASEPTA";
            base.OnInstanceCreate();
            this.Accesorio.ConfiguracionBase = "DATACON";
        }
        //obtener la lista de turnos
        public static ResultadoOperacion<List<TurnoCFS>> ObtenerTurnos(string usuario,DateTime fecha, List<Int64> subitems, bool p2d, bool express)
        {
            var p = new TurnoCFS();
            p.actualMetodo = MethodBase.GetCurrentMethod().Name;
            string pv;
            if (!p.Accesorio.Inicializar(out pv))
            {
                return ResultadoOperacion<List<TurnoCFS>>.CrearFalla(pv);
            }
            p.Parametros.Clear();
            p.Parametros.Add("fecha", fecha.Date);
            p.Parametros.Add("p2d", p2d);
            p.Parametros.Add("express", express);
            var lst = new StringBuilder();
            lst.Append("<TARJA>");
            foreach (var i in subitems)
            {
                lst.AppendFormat("<VALOR CONSECUTIVO=\"{0}\"/>", i);
            }
            lst.Append("</TARJA>");
            p.Parametros.Add("subitems", lst.ToString());

            var tt = p.SetMessage("NO_NULO", p.actualMetodo, usuario);
            if (string.IsNullOrEmpty(usuario))
            {
                return Respuesta.ResultadoOperacion<List<TurnoCFS>>.CrearFalla(string.Format(tt.Item1, nameof(usuario)));
            }
            if (subitems == null || subitems.Count <= 0)
            {
                return ResultadoOperacion<List<TurnoCFS>>.CrearFalla("La lista de subitems no puede ser nula o cero");
            }

            var bcon = p.Accesorio.ObtenerConfiguracion("N4Middleware")?.valor;
#if DEBUG
            p.LogEvent(usuario, p.actualMetodo, usuario);
#endif
            var rp = BDOpe.ComandoSelectALista<TurnoCFS>(bcon, "[Bill].[cfs_turnos_disponibles]", p.Parametros);
            return rp.Exitoso ? ResultadoOperacion<List<TurnoCFS>>.CrearResultadoExitoso(rp.Resultado) : ResultadoOperacion<List<TurnoCFS>>.CrearFalla(rp.MensajeProblema);
        }




    }
}
