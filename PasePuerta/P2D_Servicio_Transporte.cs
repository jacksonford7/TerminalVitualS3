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
    public class P2D_Servicio_Transporte : ModuloBase
    {
        public override void OnInstanceCreate()
        {
            this.alterClase = "PASEPTA";
            base.OnInstanceCreate();
            this.Accesorio.ConfiguracionBase = "DATACON";
        }

        public static ResultadoOperacion<bool> Marcar_Servicio_Transporte_Cfs(string ID_USUARIO, Int64 id_unidades, decimal qty, int tipo)
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


            var n4 = N4Ws.Entidad.Servicios.PonerEvento_P2D(id_unidades, ID_USUARIO, qty, tipo);
            if (!n4.Exitoso)
            {
                p.LogEvent(ID_USUARIO, p.actualMetodo, string.Format("Falló la puesta del servicio AppCgsa cfs  {0}, usuario {1}", ID_USUARIO, n4.MensajeProblema));
                return Respuesta.ResultadoOperacion<bool>.CrearFalla(n4.MensajeProblema);
            }

            return ResultadoOperacion<bool>.CrearResultadoExitoso(true, "Exito al actualizar");

        }
    }
}
