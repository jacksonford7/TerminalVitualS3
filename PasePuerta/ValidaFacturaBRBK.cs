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
    public class ValidaFacturaBRBK : ModuloBase
    {
        public int resultado { get; set; }
      

       
        public ValidaFacturaBRBK() : base()
        {

        }
        public override void OnInstanceCreate()
        {
            this.alterClase = "PASEPTA";
            base.OnInstanceCreate();
            this.Accesorio.ConfiguracionBase = "DATACON";
        }
        //obtener la lista de turnos
        public static ResultadoOperacion<List<ValidaFacturaBRBK>> ValidaRubrosPendientes(string usuario,string XmlContenedor)
        {
            var p = new ValidaFacturaBRBK();
            p.actualMetodo = MethodBase.GetCurrentMethod().Name;
            string pv;
            if (!p.Accesorio.Inicializar(out pv))
            {
                return ResultadoOperacion<List<ValidaFacturaBRBK>>.CrearFalla(pv);
            }
            p.Parametros.Clear();
            p.Parametros.Add("XmlContenedor", XmlContenedor);
         
            var tt = p.SetMessage("NO_NULO", p.actualMetodo, usuario);
            if (string.IsNullOrEmpty(usuario))
            {
                return Respuesta.ResultadoOperacion<List<ValidaFacturaBRBK>>.CrearFalla(string.Format(tt.Item1, nameof(usuario)));
            }
            

            var bcon = p.Accesorio.ObtenerConfiguracion("N5")?.valor;
#if DEBUG
            p.LogEvent(usuario, p.actualMetodo, usuario);
#endif
            var rp = BDOpe.ComandoSelectALista<ValidaFacturaBRBK>(bcon, "[Bill].[Validacion_Servicio_Fact_Brbk]", p.Parametros);
            return rp.Exitoso ? ResultadoOperacion<List<ValidaFacturaBRBK>>.CrearResultadoExitoso(rp.Resultado) : ResultadoOperacion<List<ValidaFacturaBRBK>>.CrearFalla(rp.MensajeProblema);
        }




    }
}
