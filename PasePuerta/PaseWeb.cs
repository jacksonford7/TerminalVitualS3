using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Configuraciones;
using AccesoDatos;
using Respuesta;

namespace PasePuerta
{
    public class Pase_Web : ModuloBase
    {
        public Int64 ID_PPWEB { get; set; } //
        public string MRN { get; set; }
        public string MSN { get; set; }
        public string HSN { get; set; }
        public string FACTURA { get; set; }
        public string AGENTE { get; set; }
        public string FACTURADO { get; set; }
        public bool? PAGADO { get; set; }
        public Int64? GKEY { get; set; }
        public string REFERENCIA { get; set; }
        public string CONTENEDOR { get; set; }
        public string DOCUMENTO { get; set; }
        public string CIATRANS { get; set; }
        public string CHOFER { get; set; }
        public string PLACA { get; set; }
        public DateTime? CAS { get; set; }
        public DateTime? FECHA_SALIDA { get; set; }
        public DateTime? FECHA_AUT_PPWEB { get; set; }
        public string HORA_AUT_PPWEB { get; set; }
        public string TIPO_CNTR { get; set; }
        public int? ID_TURNO { get; set; }
        public Int64? TURNO { get; set; }
        public string D_TURNO { get; set; }
        public double? ID_PASE { get; set; }
        public string ESTADO { get; set; }
        public bool? ENVIADO { get; set; }
        public bool? AUTORIZADO { get; set; }
        public bool? VENTANILLA { get; set; }
        public string USUARIO_ING { get; set; }
        public DateTime? FECHA_ING { get; set; }
        public string USUARIO_MOD { get; set; }
        public DateTime? FECHA_MOD { get; set; }
        public string COMENTARIO { get; set; }
        public string USUARIO_AUT { get; set; }
        public DateTime? FECHA_AUT { get; set; }
        public bool? CAMBIO_FECHA { get; set; }


        public bool? CNTR_DD { get; set; }
        public string AGENTE_DESC { get; set; }
        public string FACTURADO_DESC { get; set; }
        public string IMPORTADOR { get; set; }
        public string IMPORTADOR_DESC { get; set; }

        public string TIPO_CONSULTA { get; set; }

        public override void OnInstanceCreate()
        {
            this.alterClase = "PASEPTA";
            base.OnInstanceCreate();
            this.Accesorio.ConfiguracionBase = "DATACON";
        }
        public Pase_Web()
        {
            OnInstanceCreate();
            this.FECHA_ING = DateTime.Now;
            this.ESTADO = "N";
            this.ENVIADO = false;
            this.AUTORIZADO = false;
            this.VENTANILLA = false;
            this.CAMBIO_FECHA = false;

        }

        //INSERTAR EN WEB PASE A PTA
        public Respuesta.ResultadoOperacion<Int64> Insertar()
        {
            this.actualMetodo = MethodBase.GetCurrentMethod().Name;
            //inicializa
            string pv = string.Empty;
            if (!this.Accesorio.Inicializar(out pv))
            {
                return Respuesta.ResultadoOperacion<Int64>.CrearFalla(pv);
            }
            //Validaciones-->
            var tt = SetMessage("NO_NULO", actualMetodo, USUARIO_ING);
            if (string.IsNullOrEmpty(this.MRN))
            {
                return Respuesta.ResultadoOperacion<Int64>.CrearFalla(string.Format(tt.Item1, nameof(MRN)));
            }
            if (string.IsNullOrEmpty(this.MSN))
            {
                return Respuesta.ResultadoOperacion<Int64>.CrearFalla(string.Format(tt.Item1, nameof(MSN)));
            }
            if (string.IsNullOrEmpty(this.HSN))
            {
                return Respuesta.ResultadoOperacion<Int64>.CrearFalla(string.Format(tt.Item1, nameof(HSN)));
            }
            if (string.IsNullOrEmpty(this.FACTURA))
            {
                return Respuesta.ResultadoOperacion<Int64>.CrearFalla(string.Format(tt.Item1, nameof(FACTURA)));
            }
            if (string.IsNullOrEmpty(this.AGENTE))
            {
                return Respuesta.ResultadoOperacion<Int64>.CrearFalla(string.Format(tt.Item1, nameof(AGENTE)));
            }
            if (string.IsNullOrEmpty(this.FACTURADO))
            {
                return Respuesta.ResultadoOperacion<Int64>.CrearFalla(string.Format(tt.Item1, nameof(FACTURADO)));
            }
            if (!this.PAGADO.HasValue)
            {
                return Respuesta.ResultadoOperacion<Int64>.CrearFalla(string.Format(tt.Item1, nameof(PAGADO)));
            }
            if (!this.GKEY.HasValue)
            {
                return Respuesta.ResultadoOperacion<Int64>.CrearFalla(string.Format(tt.Item1, nameof(GKEY)));
            }

            if (string.IsNullOrEmpty(this.REFERENCIA))
            {
                return Respuesta.ResultadoOperacion<Int64>.CrearFalla(string.Format(tt.Item1, nameof(REFERENCIA)));
            }
            if (string.IsNullOrEmpty(this.CONTENEDOR))
            {
                return Respuesta.ResultadoOperacion<Int64>.CrearFalla(string.Format(tt.Item1, nameof(CONTENEDOR)));
            }
            if (string.IsNullOrEmpty(this.DOCUMENTO))
            {
                return Respuesta.ResultadoOperacion<Int64>.CrearFalla(string.Format(tt.Item1, nameof(DOCUMENTO)));
            }

            if (!this.CAS.HasValue)
            {
                return Respuesta.ResultadoOperacion<Int64>.CrearFalla(string.Format(tt.Item1, nameof(CAS)));
            }

            if (!this.FECHA_SALIDA.HasValue)
            {
                return Respuesta.ResultadoOperacion<Int64>.CrearFalla(string.Format(tt.Item1, nameof(FECHA_SALIDA)));
            }
            this.FECHA_AUT_PPWEB = FECHA_SALIDA;
            this.HORA_AUT_PPWEB = FECHA_SALIDA.HasValue ? FECHA_SALIDA.Value.ToString("HH:mm") : null;


            if (string.IsNullOrEmpty(this.TIPO_CNTR))
            {
                return Respuesta.ResultadoOperacion<Int64>.CrearFalla(string.Format(tt.Item1, nameof(TIPO_CNTR)));
            }
            if (string.IsNullOrEmpty(this.USUARIO_ING))
            {
                return Respuesta.ResultadoOperacion<Int64>.CrearFalla(string.Format(tt.Item1, nameof(USUARIO_ING)));
            }

    

            this.Parametros.Clear();
            this.Parametros.Add(nameof(MRN), MRN);
            this.Parametros.Add(nameof(MSN), MSN);
            this.Parametros.Add(nameof(HSN), HSN);
            this.Parametros.Add(nameof(FACTURA), FACTURA);
            this.Parametros.Add(nameof(AGENTE), AGENTE);
            this.Parametros.Add(nameof(FACTURADO), FACTURADO);
            this.Parametros.Add(nameof(PAGADO), PAGADO);
            this.Parametros.Add(nameof(GKEY), GKEY);
            this.Parametros.Add(nameof(REFERENCIA), REFERENCIA);
            this.Parametros.Add(nameof(CONTENEDOR), CONTENEDOR);
            this.Parametros.Add(nameof(DOCUMENTO), DOCUMENTO);
            this.Parametros.Add(nameof(CIATRANS), CIATRANS);
            this.Parametros.Add(nameof(CHOFER), CHOFER);
            this.Parametros.Add(nameof(PLACA), PLACA);
            this.Parametros.Add(nameof(CAS), CAS);
            this.Parametros.Add(nameof(FECHA_SALIDA), FECHA_SALIDA);
            this.Parametros.Add(nameof(FECHA_AUT_PPWEB), FECHA_AUT_PPWEB);
            this.Parametros.Add(nameof(HORA_AUT_PPWEB), HORA_AUT_PPWEB);
            this.Parametros.Add(nameof(TIPO_CNTR), TIPO_CNTR);
            this.Parametros.Add(nameof(ID_TURNO), ID_TURNO);
            this.Parametros.Add(nameof(D_TURNO), D_TURNO);
            this.Parametros.Add(nameof(ID_PASE), ID_PASE);
            this.Parametros.Add(nameof(ESTADO), ESTADO);
            this.Parametros.Add(nameof(ENVIADO), ENVIADO);
            this.Parametros.Add(nameof(AUTORIZADO), AUTORIZADO);
            this.Parametros.Add(nameof(VENTANILLA), VENTANILLA);
            this.Parametros.Add(nameof(USUARIO_ING), USUARIO_ING);
            this.Parametros.Add(nameof(USUARIO_MOD), USUARIO_MOD);
            this.Parametros.Add(nameof(FECHA_MOD), FECHA_MOD);
            this.Parametros.Add(nameof(COMENTARIO), COMENTARIO);
            this.Parametros.Add(nameof(USUARIO_AUT), USUARIO_AUT);
            this.Parametros.Add(nameof(FECHA_AUT), FECHA_AUT);
            this.Parametros.Add(nameof(CAMBIO_FECHA), CAMBIO_FECHA);
            var bcon = this.Accesorio.ObtenerConfiguracion("N4Middleware")?.valor;
 
             var result =  BDOpe.ComandoInsertUpdateDeleteID(bcon, "[Bill].[nuevo_pase_web]",this.Parametros);
            this.ID_PPWEB = result.Resultado.HasValue ? result.Resultado.Value : -1;
            if (!result.Exitoso)
            {
                return Respuesta.ResultadoOperacion<Int64>.CrearFalla(result.MensajeProblema);
            }
            return Respuesta.ResultadoOperacion<Int64>.CrearResultadoExitoso(result.Resultado.HasValue ? result.Resultado.Value : -1);

        }

        public static ResultadoOperacion<List<Pase_Web>> ObtenerCargaPase(string mrn, string msn, string hsn, string ruc)
        {
            var p = new Pase_Container();
            p.actualMetodo = MethodBase.GetCurrentMethod().Name;
            string pv;
            if (!p.Accesorio.Inicializar(out pv))
            {
                return ResultadoOperacion<List<Pase_Web>>.CrearFalla(pv);
            }
            p.Parametros.Clear();

            p.Parametros.Add("mrn", mrn);
            p.Parametros.Add("msn", msn);
            p.Parametros.Add("hsn", hsn);
            p.Parametros.Add("ruc", ruc);


            var bcon = p.Accesorio.ObtenerConfiguracion("N4Middleware")?.valor;

            var rp = BDOpe.ComandoSelectALista<Pase_Web>(bcon, "[Bill].[carga_pase_web]", p.Parametros);

            return rp.Exitoso ? ResultadoOperacion<List<Pase_Web>>.CrearResultadoExitoso(rp.Resultado) : ResultadoOperacion<List<Pase_Web>>.CrearFalla(rp.MensajeProblema);

        }

        public static ResultadoOperacion<List<Pase_Web>> ObtenerCargaPaseAutorizar(string mrn, string msn, string hsn, string tipo, string contenedor, string facturas )
        {
            var p = new Pase_Container();
            p.actualMetodo = MethodBase.GetCurrentMethod().Name;
            string pv;
            if (!p.Accesorio.Inicializar(out pv))
            {
                return ResultadoOperacion<List<Pase_Web>>.CrearFalla(pv);
            }
            p.Parametros.Clear();

            p.Parametros.Add("mrn", mrn);
            p.Parametros.Add("msn", msn);
            p.Parametros.Add("hsn", hsn);
            p.Parametros.Add("tipo", tipo);
            p.Parametros.Add("contenedor", contenedor);
            p.Parametros.Add("facturas", facturas);

            var bcon = p.Accesorio.ObtenerConfiguracion("N4Middleware")?.valor;

            var rp = BDOpe.ComandoSelectALista<Pase_Web>(bcon, "[Bill].[carga_pase_web_autorizar]", p.Parametros);

            return rp.Exitoso ? ResultadoOperacion<List<Pase_Web>>.CrearResultadoExitoso(rp.Resultado) : ResultadoOperacion<List<Pase_Web>>.CrearFalla(rp.MensajeProblema);

        }



        public static ResultadoOperacion<List<Pase_Web>> ObtenerListaPase(string mrn, string msn, string hsn, string ruc)
        {
            var p = new Pase_Container();
            p.actualMetodo = MethodBase.GetCurrentMethod().Name;
            string pv;
            if (!p.Accesorio.Inicializar(out pv))
            {
                return ResultadoOperacion<List<Pase_Web>>.CrearFalla(pv);
            }
            p.Parametros.Clear();

            p.Parametros.Add("mrn", mrn);
            p.Parametros.Add("msn", msn);
            p.Parametros.Add("hsn", hsn);
            p.Parametros.Add("ruc", ruc);


            var bcon = p.Accesorio.ObtenerConfiguracion("N4Middleware")?.valor;

            var rp = BDOpe.ComandoSelectALista<Pase_Web>(bcon, "[Bill].[lista_pase_web]", p.Parametros);

            return rp.Exitoso ? ResultadoOperacion<List<Pase_Web>>.CrearResultadoExitoso(rp.Resultado) : ResultadoOperacion<List<Pase_Web>>.CrearFalla(rp.MensajeProblema);

        }

        //nuevo verificar si existe antes de insercion
        public ResultadoOperacion<bool> ExisteReserva(string USUARIO, int ID_PLAN, int ID_PLAN_SECUENCIA)
        {
          
            this.actualMetodo = MethodBase.GetCurrentMethod().Name;
            string pv;
            if (!this.Accesorio.Inicializar(out pv))
            {
                return ResultadoOperacion<bool>.CrearFalla(pv);
            }

            this.Parametros.Clear();
            this.Parametros.Add(nameof(USUARIO), USUARIO);
            this.Parametros.Add(nameof(ID_PLAN), ID_PLAN);
            this.Parametros.Add(nameof(ID_PLAN_SECUENCIA), ID_PLAN_SECUENCIA);

            var bcon = this.Accesorio.ObtenerConfiguracion("N4Middleware")?.valor;
            var result = BDOpe.ComandoSelectEscalar<bool>(bcon, "SELECT [Bill].[verificar_reserva](@USUARIO,@ID_PLAN,@ID_PLAN_SECUENCIA)", this.Parametros);

            if (!result.Exitoso)
            {
                this.LogEvent(USUARIO, this.actualMetodo, string.Format("Falló la ejecucion verificar_reserva ID_PLAN:{0}/SECUENCIA:{1}/{2}", ID_PLAN, ID_PLAN_SECUENCIA, result.MensajeProblema));
                return Respuesta.ResultadoOperacion<bool>.CrearFalla(result.MensajeProblema);
            }
            return ResultadoOperacion<bool>.CrearResultadoExitoso(result.Resultado, "Exito al Ejecutar");
        }





    }
}
