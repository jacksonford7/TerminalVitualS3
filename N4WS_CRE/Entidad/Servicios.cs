using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Configuraciones;
using Respuesta;
using AccesoDatos;
using System.Reflection;
using System.Xml.Linq;

namespace N4WS_CRE.Entidad
{
   public class Servicios : ModuloBase
    {

        public override void OnInstanceCreate()
        {
            this.alterClase = "N4_SERVICE";
            base.OnInstanceCreate();
            this.Accesorio.ConfiguracionBase = "DATACON";
        }
        //inicializa de la instancia de servicio
        private static Servicios InicializaServicio(out string erno)
        {
            var p = new Servicios();
            if (!p.Accesorio.Inicializar(out erno))
            {
                return null;
            }
            return p;
        }

        private static N4Configuration ObtenerInicializadorBilllingCredenciales(Servicios ser, out string novedad)
        {
            if (ser == null)
            {
                novedad = "Objeto inicializador es nulo";
                return null;
            }
            if (!ser.Accesorio.ExistenConfiguraciones)
            {
                novedad = "No existen configuraciones de inicialización";
                return null;
            }
            var ur = ser.Accesorio.ObtenerConfiguracion("URL_BILLING")?.valor;
            var us = ser.Accesorio.ObtenerConfiguracion("USUARIO_BILLING")?.valor;
            var pas = ser.Accesorio.ObtenerConfiguracion("PASSWORD_BILLING")?.valor;
            var sc = ser.Accesorio.ObtenerConfiguracion("SCOPE_BILLING")?.valor;
            novedad = string.Empty;
            return N4Configuration.GetInstance(us, pas, ur, sc);

        }

        public static N4_BasicResponse EjecutarCODEExtensionGenericoN4BillingCredenciales(GroovyCodeExtension co, string usuario)
        {
            var n = new N4_BasicResponse();
            //paso 1-> Inicializar instancia de servicio
            string pv;
            var p = InicializaServicio(out pv);
            p.actualMetodo = MethodBase.GetCurrentMethod().Name;
            if (p == null)
            {
                n.status = 3;
                n.status_id = "SEVERE";
                n.messages.Add(new N4_response_message("NO_INITIALIZED_DATA", "SEVERE", pv));
                return n;
            }
            //paso 2 -> inicializar instancia de n4configurariones
            var n4 = ObtenerInicializadorBilllingCredenciales(p, out pv);
            if (n4 == null)
            {
                n.status = 3;
                n.status_id = "SEVERE";
                n.messages.Add(new N4_response_message("NO_INITIALIZED_N4_INSTANCE", "SEVERE", pv));
                return n;
            }
            if (co == null)
            {
                n.status = 3;
                n.status_id = "SEVERE";
                n.messages.Add(new N4_response_message("NO_CODE_EXENSION_NULL", "SEVERE", "CODE_EXTENSON es nulo"));
                return n;

            }
            if (string.IsNullOrEmpty(co.location))
            {
                n.status = 3;
                n.status_id = "SEVERE";
                n.messages.Add(new N4_response_message("NO_LOCATION_FOR_CODE_EXTENSION", "SEVERE", "NO HAY LOCACION"));
                return n;

            }
            if (string.IsNullOrEmpty(co.name))
            {
                n.status = 3;
                n.status_id = "SEVERE";
                n.messages.Add(new N4_response_message("NO_NAME_FOR_CODE_EXTENSION", "SEVERE", "NO HAY NOMBRE"));
                return n;
            }
            var nbb = N4Basic.GetInstance(n4.usuario, n4.password, n4.url, n4.scope);
            var gs = co.ToString();

            var aprr = nbb.BasicInvokeService(gs, p.myClase, p.actualMetodo, usuario, 7000);
            return aprr;
        }

    }




}
