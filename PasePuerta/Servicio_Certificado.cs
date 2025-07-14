using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AccesoDatos;
using Configuraciones;
using Respuesta;
using N4Ws;
using System.Data;

namespace PasePuerta
{
    public class Servicio_Certificado : ModuloBase
    {

        public override void OnInstanceCreate()
        {
            this.alterClase = "PASEPTA";
            base.OnInstanceCreate();
            this.Accesorio.ConfiguracionBase = "DATACON";
        }

        public static ResultadoOperacion<bool> Marcar_Servicio(string ID_USUARIO, List<string> unidades)
        {
            var p = new Pase_Web();
            p.actualMetodo = MethodBase.GetCurrentMethod().Name;
            string pv;
            if (!p.Accesorio.Inicializar(out pv))
            {
                return ResultadoOperacion<bool>.CrearFalla(pv);
            }

            var tt = p.SetMessage("NO_NULO", p.actualMetodo, ID_USUARIO);
            if (string.IsNullOrEmpty(ID_USUARIO))
            {
                return ResultadoOperacion<bool>.CrearFalla(string.Format(tt.Item1, nameof(ID_USUARIO)));
            }
            if (unidades == null || unidades.Count < 0)
            {
                return ResultadoOperacion<bool>.CrearFalla(string.Format(tt.Item1, nameof(ID_USUARIO)));
            }


            var n4 = N4Ws.Entidad.Servicios.PonerEventoCarbonoNeutro(unidades, ID_USUARIO);
            if (!n4.Exitoso)
            {
                p.LogEvent(ID_USUARIO, p.actualMetodo, string.Format("Falló la puesta del servicio carbono neutro {0}, usuario {1}", ID_USUARIO, n4.MensajeProblema));
                return Respuesta.ResultadoOperacion<bool>.CrearFalla(n4.MensajeProblema);
            }

            return ResultadoOperacion<bool>.CrearResultadoExitoso(true, "Exito al actualizar");

        }

        public static ResultadoOperacion<bool> Marcar_Servicio_Cfs(string ID_USUARIO,Int64 id_unidades)
        {
            var p = new Pase_Web();
            p.actualMetodo = MethodBase.GetCurrentMethod().Name;
            string pv;
            if (!p.Accesorio.Inicializar(out pv))
            {
                return ResultadoOperacion<bool>.CrearFalla(pv);
            }

            var tt = p.SetMessage("NO_NULO", p.actualMetodo, ID_USUARIO);
            if (string.IsNullOrEmpty(ID_USUARIO))
            {
                return ResultadoOperacion<bool>.CrearFalla(string.Format(tt.Item1, nameof(ID_USUARIO)));
            }
            if (id_unidades <= 0)
            {
                return ResultadoOperacion<bool>.CrearFalla(string.Format(tt.Item1, nameof(ID_USUARIO)));
            }


            var n4 = N4Ws.Entidad.Servicios.PonerEventoCarbonoNeutroCFS(id_unidades, ID_USUARIO);
            if (!n4.Exitoso)
            {
                p.LogEvent(ID_USUARIO, p.actualMetodo, string.Format("Falló la puesta del servicio carbono neutro cfs  {0}, usuario {1}", ID_USUARIO, n4.MensajeProblema));
                return Respuesta.ResultadoOperacion<bool>.CrearFalla(n4.MensajeProblema);
            }

            return ResultadoOperacion<bool>.CrearResultadoExitoso(true, "Exito al actualizar");

        }

        public static ResultadoOperacion<bool> Marcar_Servicio_Expo(string ID_USUARIO, List<string> unidades)
        {
            var p = new Pase_Web();
            p.actualMetodo = MethodBase.GetCurrentMethod().Name;
            string pv;
            if (!p.Accesorio.Inicializar(out pv))
            {
                return ResultadoOperacion<bool>.CrearFalla(pv);
            }

            var tt = p.SetMessage("NO_NULO", p.actualMetodo, ID_USUARIO);
            if (string.IsNullOrEmpty(ID_USUARIO))
            {
                return ResultadoOperacion<bool>.CrearFalla(string.Format(tt.Item1, nameof(ID_USUARIO)));
            }
            if (unidades == null || unidades.Count < 0)
            {
                return ResultadoOperacion<bool>.CrearFalla(string.Format(tt.Item1, nameof(ID_USUARIO)));
            }


            var n4 = N4Ws.Entidad.Servicios.PonerEventoCarbonoNeutroExpo(unidades, ID_USUARIO);
            if (!n4.Exitoso)
            {
                p.LogEvent(ID_USUARIO, p.actualMetodo, string.Format("Falló la puesta del servicio carbono neutro exportación {0}, usuario {1}", ID_USUARIO, n4.MensajeProblema));
                return Respuesta.ResultadoOperacion<bool>.CrearFalla(n4.MensajeProblema);
            }

            return ResultadoOperacion<bool>.CrearResultadoExitoso(true, "Exito al actualizar");

        }

        public static ResultadoOperacion<bool> Marcar_Servicio_Brbk(string ID_USUARIO, Int64 id_unidades)
        {
            var p = new Pase_Web();
            p.actualMetodo = MethodBase.GetCurrentMethod().Name;
            string pv;
            if (!p.Accesorio.Inicializar(out pv))
            {
                return ResultadoOperacion<bool>.CrearFalla(pv);
            }

            var tt = p.SetMessage("NO_NULO", p.actualMetodo, ID_USUARIO);
            if (string.IsNullOrEmpty(ID_USUARIO))
            {
                return ResultadoOperacion<bool>.CrearFalla(string.Format(tt.Item1, nameof(ID_USUARIO)));
            }
            if (id_unidades <= 0)
            {
                return ResultadoOperacion<bool>.CrearFalla(string.Format(tt.Item1, nameof(ID_USUARIO)));
            }


            var n4 = N4Ws.Entidad.Servicios.PonerEventoCarbonoNeutroBrbk(id_unidades, ID_USUARIO);
            if (!n4.Exitoso)
            {
                p.LogEvent(ID_USUARIO, p.actualMetodo, string.Format("Falló la puesta del servicio carbono neutro break bulk  {0}, usuario {1}", ID_USUARIO, n4.MensajeProblema));
                return Respuesta.ResultadoOperacion<bool>.CrearFalla(n4.MensajeProblema);
            }

            return ResultadoOperacion<bool>.CrearResultadoExitoso(true, "Exito al actualizar");

        }

        public static ResultadoOperacion<bool> DamageControl_Marcar_Servicio(string ID_USUARIO, List<string> unidades)
        {
            var p = new Pase_Web();
            p.actualMetodo = MethodBase.GetCurrentMethod().Name;
            string pv;
            if (!p.Accesorio.Inicializar(out pv))
            {
                return ResultadoOperacion<bool>.CrearFalla(pv);
            }

            var tt = p.SetMessage("NO_NULO", p.actualMetodo, ID_USUARIO);
            if (string.IsNullOrEmpty(ID_USUARIO))
            {
                return ResultadoOperacion<bool>.CrearFalla(string.Format(tt.Item1, nameof(ID_USUARIO)));
            }
            if (unidades == null || unidades.Count < 0)
            {
                return ResultadoOperacion<bool>.CrearFalla(string.Format(tt.Item1, nameof(ID_USUARIO)));
            }


            var n4 = N4Ws.Entidad.Servicios.PonerEventoDamageControl(unidades, ID_USUARIO);
            if (!n4.Exitoso)
            {
                p.LogEvent(ID_USUARIO, p.actualMetodo, string.Format("Falló la puesta del servicioDamage Control {0}, usuario {1}", ID_USUARIO, n4.MensajeProblema));
                return Respuesta.ResultadoOperacion<bool>.CrearFalla(n4.MensajeProblema);
            }

            return ResultadoOperacion<bool>.CrearResultadoExitoso(true, "Exito al actualizar");

        }

        public static ResultadoOperacion<bool> Marcar_Servicio_Sellos(string ID_USUARIO, List<string> unidades)
        {
            var p = new Pase_Web();
            p.actualMetodo = MethodBase.GetCurrentMethod().Name;
            string pv;
            if (!p.Accesorio.Inicializar(out pv))
            {
                return ResultadoOperacion<bool>.CrearFalla(pv);
            }

            var tt = p.SetMessage("NO_NULO", p.actualMetodo, ID_USUARIO);
            if (string.IsNullOrEmpty(ID_USUARIO))
            {
                return ResultadoOperacion<bool>.CrearFalla(string.Format(tt.Item1, nameof(ID_USUARIO)));
            }
            if (unidades == null || unidades.Count < 0)
            {
                return ResultadoOperacion<bool>.CrearFalla(string.Format(tt.Item1, nameof(ID_USUARIO)));
            }


            var n4 = N4Ws.Entidad.Servicios.PonerEventoImagenesSellos(unidades, ID_USUARIO);
            if (!n4.Exitoso)
            {
                p.LogEvent(ID_USUARIO, p.actualMetodo, string.Format("Falló la puesta del servicio imágenes de sellos {0}, usuario {1}", ID_USUARIO, n4.MensajeProblema));
                return Respuesta.ResultadoOperacion<bool>.CrearFalla(n4.MensajeProblema);
            }

            return ResultadoOperacion<bool>.CrearResultadoExitoso(true, "Exito al actualizar");

        }
    }
}
