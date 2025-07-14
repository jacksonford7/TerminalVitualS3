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
    public class SaldoCargaBRBK : ModuloBase
    {
        public string NUMERO_CARGA { get; set; }
        public int CANTIDAD_TOT_BL { get; set; }
        public int CANTIDAD_SALDO { get; set; }
        public Int64? GKEY_ITEMS { get; set; }
        public int CANTIDAD_RESERVADA { get; set; }
        public int SALDO_FINAL { get; set; }

       
        public SaldoCargaBRBK() : base()
        {

        }
        public override void OnInstanceCreate()
        {
            this.alterClase = "PASEPTA";
            base.OnInstanceCreate();
            this.Accesorio.ConfiguracionBase = "DATACON";
        }
        //obtener la lista de turnos
        public static ResultadoOperacion<List<SaldoCargaBRBK>> SaldoPendienteBRBK(string usuario, string BL)
        {
            var p = new SaldoCargaBRBK();
            p.actualMetodo = MethodBase.GetCurrentMethod().Name;
            string pv;
            if (!p.Accesorio.Inicializar(out pv))
            {
                return ResultadoOperacion<List<SaldoCargaBRBK>>.CrearFalla(pv);
            }
            p.Parametros.Clear();
            p.Parametros.Add("BL", BL);
            
            var tt = p.SetMessage("NO_NULO", p.actualMetodo, usuario);
            if (string.IsNullOrEmpty(usuario))
            {
                return Respuesta.ResultadoOperacion<List<SaldoCargaBRBK>>.CrearFalla(string.Format(tt.Item1, nameof(usuario)));
            }
            

            var bcon = p.Accesorio.ObtenerConfiguracion("N5")?.valor;
#if DEBUG
            p.LogEvent(usuario, p.actualMetodo, usuario);
#endif
            var rp = BDOpe.ComandoSelectALista<SaldoCargaBRBK>(bcon, "[Bill].[saldo_carga_brbk]", p.Parametros);
            return rp.Exitoso ? ResultadoOperacion<List<SaldoCargaBRBK>>.CrearResultadoExitoso(rp.Resultado) : ResultadoOperacion<List<SaldoCargaBRBK>>.CrearFalla(rp.MensajeProblema);
        }




    }
}
