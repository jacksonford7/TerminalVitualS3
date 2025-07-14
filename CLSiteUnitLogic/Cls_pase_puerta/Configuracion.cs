using AccesoDatos;
using BillionEntidades;
using SqlConexion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLSiteUnitLogic.Cls_pase_puerta
{
    public class Configuracion : Cls_Bil_Base
    {
        public string App { get; set; }
        public string Module { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }

        public Configuracion() : base()
        {
            init();
        }

        private static void OnInit(string Base)
        {
            sql_puntero = (sql_puntero == null) ? Cls_Conexion.Conexion() : sql_puntero;
            parametros = new Dictionary<string, object>();
            nueva_conexion = Cls_Conexion.Nueva_Conexion(Base);
        }


        public static Configuracion ObtenerConfiguracion(string id)
        {
            OnInit("APPCGSA");
            parametros.Clear();
            parametros.Add("id", id);

            var ev = new Configuracion();

            var pa = new Dictionary<string, object>();
            pa.Add(nameof(id), id);

            //a base de datos: pdf.ObtenerNotificacion
            var rope = BDOpe.ComandoSelectAEntidad<Configuracion>(nueva_conexion, "pdf.ObtenerConfiguracion", pa);
            if (rope.Exitoso) { return rope.Resultado; }
            BDTraza.LogEvent<ApplicationException>("-system-", nameof(Configuracion), nameof(ObtenerConfiguracion), 3, "Evento", pa, rope.MensajeProblema, new ApplicationException(rope.MensajeProblema));
            rope = null;
            ev = null;
            return null;

        }
    }
}
