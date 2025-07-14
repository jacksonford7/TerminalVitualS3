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
    public class Servicio_Inspeccion : ModuloBase
    {
        public override void OnInstanceCreate()
        {
            this.alterClase = "PASEPTA";
            base.OnInstanceCreate();
            this.Accesorio.ConfiguracionBase = "DATACON";
        }

        public static ResultadoOperacion<bool> Marcar_Servicio(string VISIT, string EVENT, string ID_USUARIO, int QTY)
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
            

            var n4 = N4Ws.Entidad.Servicios.PonerEvento_InspSubAcuatica(VISIT, EVENT, ID_USUARIO, QTY);
            if (!n4.Exitoso)
            {
                p.LogEvent(ID_USUARIO, p.actualMetodo, string.Format("Falló la puesta del servicio Servicio_Inspeccion_SubAcuatica {0}, usuario {1}", ID_USUARIO, n4.MensajeProblema));
                return Respuesta.ResultadoOperacion<bool>.CrearFalla(n4.MensajeProblema);
            }

            return ResultadoOperacion<bool>.CrearResultadoExitoso(true, "Exito al actualizar");

        }

     
    }
}
