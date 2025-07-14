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
    public class ValidaCargaBRBK : ModuloBase
    {
        public string MENSAJE { get; set; }
      

       
        public ValidaCargaBRBK() : base()
        {

        }
        public override void OnInstanceCreate()
        {
            this.alterClase = "PASEPTA";
            base.OnInstanceCreate();
            this.Accesorio.ConfiguracionBase = "DATACON";
        }
        //obtener la lista de turnos
        public static ResultadoOperacion<List<ValidaCargaBRBK>> ValidaTurno(string usuario, Int64 IDX_ROW, string MRN, string MSN, string HSN)
        {
            var p = new ValidaCargaBRBK();
            p.actualMetodo = MethodBase.GetCurrentMethod().Name;
            string pv;
            if (!p.Accesorio.Inicializar(out pv))
            {
                return ResultadoOperacion<List<ValidaCargaBRBK>>.CrearFalla(pv);
            }
            p.Parametros.Clear();
            p.Parametros.Add("IDX_ROW", IDX_ROW);
            p.Parametros.Add("MRN", MRN);
            p.Parametros.Add("MSN", MSN);
            p.Parametros.Add("HSN", HSN);

            var tt = p.SetMessage("NO_NULO", p.actualMetodo, usuario);
            if (string.IsNullOrEmpty(usuario))
            {
                return Respuesta.ResultadoOperacion<List<ValidaCargaBRBK>>.CrearFalla(string.Format(tt.Item1, nameof(usuario)));
            }
            

            var bcon = p.Accesorio.ObtenerConfiguracion("N4Middleware")?.valor;
#if DEBUG
            p.LogEvent(usuario, p.actualMetodo, usuario);
#endif
            var rp = BDOpe.ComandoSelectALista<ValidaCargaBRBK>(bcon, "[Bill].[brbk_valida_turnos]", p.Parametros);
            return rp.Exitoso ? ResultadoOperacion<List<ValidaCargaBRBK>>.CrearResultadoExitoso(rp.Resultado) : ResultadoOperacion<List<ValidaCargaBRBK>>.CrearFalla(rp.MensajeProblema);
        }




    }
}
