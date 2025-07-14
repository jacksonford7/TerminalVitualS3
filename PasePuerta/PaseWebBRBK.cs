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
    public class PaseWebBRBK : ModuloBase
    {
        public Int64 ID_PPWEB { get; set; } //ID_PPWEB
        public string MRN { get; set; } //MRN
        public string MSN { get; set; }//MSN
        public string HSN { get; set; }//HSN
        public string FACTURA { get; set; }//FACTURA
        public string AGENTE { get; set; }//AGENTE
        public string FACTURADO { get; set; }//FACTURADO
        public bool? PAGADO { get; set; }//PAGADO
        public Int64? GKEY { get; set; }//GKEY
        public string REFERENCIA { get; set; }//REFERENCIA
        public string CONTENEDOR { get; set; }//CONTENEDOR
        public string DOCUMENTO { get; set; }//DOCUMENTO
        public string CIATRANS { get; set; }//CIATRANS
        public string CHOFER { get; set; }//CHOFER
        public string PLACA { get; set; }//PLACA
        public DateTime? FECHA_SALIDA { get; set; }//FECHA_SALIDA
        public DateTime? FECHA_AUT_PPWEB { get; set; }//FECHA_AUT_PPWEB
        public string HORA_AUT_PPWEB { get; set; }//HORA_AUT_PPWEB
        public string TIPO_CNTR { get; set; }//TIPO_CNTR
        public Int64? ID_TURNO { get; set; }//ID_TURNO
        public string D_TURNO { get; set; }//D_TURNO
        public decimal? ID_PASE { get; set; }//ID_PASE
        public string ESTADO { get; set; }//ESTADO
        public bool? ENVIADO { get; set; }//ENVIADO
        public bool? AUTORIZADO { get; set; }//AUTORIZADO
        public bool? VENTANILLA { get; set; }//VENTANILLA
        public string USUARIO_ING { get; set; }//USUARIO_ING
        public DateTime? FECHA_ING { get; set; }//FECHA_ING
        public string USUARIO_MOD { get; set; }//USUARIO_MOD
        public string COMENTARIO { get; set; }//COMENTARIO
        public string USUARIO_AUT { get; set; }//USUARIO_AUT
        public DateTime? FECHA_AUT { get; set; }//FECHA_AUT
        public bool? CAMBIO_FECHA { get; set; }//CAMBIO_FECHA
        public bool? CNTR_DD { get; set; }//CNTR_DD
        public string AGENTE_DESC { get; set; }//AGENTE_DESC
        public string FACTURADO_DESC { get; set; }//FACTURADO_DESC
        public string IMPORTADOR { get; set; }//IMPORTADOR
        public string IMPORTADOR_DESC { get; set; }//IMPORTADOR_DESC
        public Int32? CANTIDAD { get; set; } //CANTIDAD_CARGA
        public string TRANSPORTISTA_DESC { get; set; }//TRANSPORTISTA_DESC
        public string CHOFER_DESC { get; set; }//CHOFER_DESC
        public string PRIMERA { get; set; } //PRIMERA
        public string MARCA { get; set; } //MARCA
        public string NUMERO_PASE_N4 { get; set; } //NUMERO_PASE_N4

        public Int64? CONSECUTIVO { get; set; }
        public DateTime? CAS { get; set; }
        public Int64? TURNO { get; set; }
        public DateTime? FECHA_MOD { get; set; }

        public Int64? ID_UNIDAD { get; set; } //gkey


        public decimal? P2D_ALTO { get; set; }
        public decimal? P2D_ANCHO { get; set; }
        public decimal? P2D_LARGO { get; set; }
        public decimal? P2D_VOLUMEN { get; set; }
        public decimal? PESO { get; set; }
        public string IMO { get; set; } //IMO

        public int? idProducto { get; set; } 
        public int? idItem { get; set; }
        public string Tipo_Producto { get; set; } 

        public override void OnInstanceCreate()
        {
            this.alterClase = "PASEPTA";
            base.OnInstanceCreate();
            this.Accesorio.ConfiguracionBase = "DATACON";
        }
        public PaseWebBRBK()
        {
            OnInstanceCreate();
            this.FECHA_ING = DateTime.Now;
            this.ESTADO = "N";
            this.ENVIADO = false;
            this.AUTORIZADO = false;
            this.VENTANILLA = false;
            this.CAMBIO_FECHA = false;
            this.CONSECUTIVO = 0;
            this.CANTIDAD = 0;

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
              this.Parametros.Add(nameof(ID_UNIDAD), ID_UNIDAD);
            var bcon = this.Accesorio.ObtenerConfiguracion("N4Middleware")?.valor;
 
             var result =  BDOpe.ComandoInsertUpdateDeleteID(bcon, "[Bill].[nuevo_pase_web_cfs]",this.Parametros);
            this.ID_PPWEB = result.Resultado.HasValue ? result.Resultado.Value : -1;
            if (!result.Exitoso)
            {
                return Respuesta.ResultadoOperacion<Int64>.CrearFalla(result.MensajeProblema);
            }
            return Respuesta.ResultadoOperacion<Int64>.CrearResultadoExitoso(result.Resultado.HasValue ? result.Resultado.Value : -1);

        }

        public static ResultadoOperacion<List<PaseWebBRBK>> ObtenerCargaPaseBRBK(string mrn, string msn, string hsn, string ruc, DateTime? fecha=null)
        {
            var p = new Pase_Container();
            p.actualMetodo = MethodBase.GetCurrentMethod().Name;
            string pv;
            if (!p.Accesorio.Inicializar(out pv))
            {
                return ResultadoOperacion<List<PaseWebBRBK>>.CrearFalla(pv);
            }
            p.Parametros.Clear();

            p.Parametros.Add("mrn", mrn);
            p.Parametros.Add("msn", msn);
            p.Parametros.Add("hsn", hsn);
            p.Parametros.Add("ruc", ruc);

             p.Parametros.Add("fecha", fecha.HasValue? fecha.Value:DateTime.Now);


            var bcon = p.Accesorio.ObtenerConfiguracion("N4Middleware")?.valor;

            var rp = BDOpe.ComandoSelectALista<PaseWebBRBK>(bcon, "[Bill].[carga_pase_web_brbk]", p.Parametros);

            return rp.Exitoso ? ResultadoOperacion<List<PaseWebBRBK>>.CrearResultadoExitoso(rp.Resultado) : ResultadoOperacion<List<PaseWebBRBK>>.CrearFalla(rp.MensajeProblema);

        }

        public static ResultadoOperacion<List<PaseWebBRBK>> ObtenerCargaExcluirBRBK(string mrn, string msn, string hsn)
        {
            var p = new Pase_Container();
            p.actualMetodo = MethodBase.GetCurrentMethod().Name;
            string pv;
            if (!p.Accesorio.Inicializar(out pv))
            {
                return ResultadoOperacion<List<PaseWebBRBK>>.CrearFalla(pv);
            }
            p.Parametros.Clear();

            p.Parametros.Add("mrn", mrn);
            p.Parametros.Add("msn", msn);
            p.Parametros.Add("hsn", hsn);
           

            var bcon = p.Accesorio.ObtenerConfiguracion("N4Middleware")?.valor;

            var rp = BDOpe.ComandoSelectALista<PaseWebBRBK>(bcon, "[Bill].[buscar_carga_brbk]", p.Parametros);

            return rp.Exitoso ? ResultadoOperacion<List<PaseWebBRBK>>.CrearResultadoExitoso(rp.Resultado) : ResultadoOperacion<List<PaseWebBRBK>>.CrearFalla(rp.MensajeProblema);

        }

        public static ResultadoOperacion<List<PaseWebBRBK>> ObtenerTarjaCFS(string mrn, string msn, string hsn, Int64 ikey)
        {
            var p = new Pase_Container();
            p.actualMetodo = MethodBase.GetCurrentMethod().Name;
            string pv;
            if (!p.Accesorio.Inicializar(out pv))
            {
                return ResultadoOperacion<List<PaseWebBRBK>>.CrearFalla(pv);
            }
            p.Parametros.Clear();
            p.Parametros.Add("mrn", mrn);
            p.Parametros.Add("msn", msn);
            p.Parametros.Add("hsn", hsn);
            p.Parametros.Add("key", ikey);
            var bcon = p.Accesorio.ObtenerConfiguracion("N4Middleware")?.valor;
            var rp = BDOpe.ComandoSelectALista<PaseWebBRBK>(bcon, "[Bill].[carga_tarja_cfs]", p.Parametros);
            return rp.Exitoso ? ResultadoOperacion<List<PaseWebBRBK>>.CrearResultadoExitoso(rp.Resultado) : ResultadoOperacion<List<PaseWebBRBK>>.CrearFalla(rp.MensajeProblema);
        }

        public static ResultadoOperacion<List<PaseWebBRBK>> ObtenerTarjaCFS_Edit(string mrn, string msn, string hsn, Int64 ikey)
        {
            var p = new Pase_Container();
            p.actualMetodo = MethodBase.GetCurrentMethod().Name;
            string pv;
            if (!p.Accesorio.Inicializar(out pv))
            {
                return ResultadoOperacion<List<PaseWebBRBK>>.CrearFalla(pv);
            }
            p.Parametros.Clear();
            p.Parametros.Add("mrn", mrn);
            p.Parametros.Add("msn", msn);
            p.Parametros.Add("hsn", hsn);
            p.Parametros.Add("key", ikey);
            var bcon = p.Accesorio.ObtenerConfiguracion("N4Middleware")?.valor;
            var rp = BDOpe.ComandoSelectALista<PaseWebBRBK>(bcon, "[Bill].[carga_tarja_cfs_edit]", p.Parametros);
            return rp.Exitoso ? ResultadoOperacion<List<PaseWebBRBK>>.CrearResultadoExitoso(rp.Resultado) : ResultadoOperacion<List<PaseWebBRBK>>.CrearFalla(rp.MensajeProblema);
        }

        public static ResultadoOperacion<List<PaseWebBRBK>> ObtenerListaPase(string mrn, string msn, string hsn, string ruc)
        {
            var p = new PaseWebBRBK();
            p.actualMetodo = MethodBase.GetCurrentMethod().Name;
            string pv;
            if (!p.Accesorio.Inicializar(out pv))
            {
                return ResultadoOperacion<List<PaseWebBRBK>>.CrearFalla(pv);
            }
            p.Parametros.Clear();

            p.Parametros.Add("mrn", mrn);
            p.Parametros.Add("msn", msn);
            p.Parametros.Add("hsn", hsn);
            p.Parametros.Add("ruc", ruc);


            var bcon = p.Accesorio.ObtenerConfiguracion("N4Middleware")?.valor;

            var rp = BDOpe.ComandoSelectALista<PaseWebBRBK>(bcon, "[Bill].[lista_pase_web_brbk]", p.Parametros);

            return rp.Exitoso ? ResultadoOperacion<List<PaseWebBRBK>>.CrearResultadoExitoso(rp.Resultado) : ResultadoOperacion<List<PaseWebBRBK>>.CrearFalla(rp.MensajeProblema);

        }


        /*
         <TARJA><VALOR CONSECUTIVO="947"/>  // string un solo valor

         */
         //retorna 1 valor
        public static ResultadoOperacion<string> Verficar_Ubicacion(string USUARIO, List<Int64> consecutivos)
        {
            var p = new Pase_CFS();
            p.actualMetodo = MethodBase.GetCurrentMethod().Name;
            string pv;
            if (!p.Accesorio.Inicializar(out pv))
            {
                return ResultadoOperacion<string>.CrearFalla(pv);
            }

            p.Parametros.Clear();
            var tt = p.SetMessage("NO_NULO", p.actualMetodo, USUARIO);
            if (string.IsNullOrEmpty(USUARIO))
            {
                return Respuesta.ResultadoOperacion<string>.CrearFalla(string.Format(tt.Item1, nameof(USUARIO)));
            }
            if (consecutivos == null || consecutivos.Count <= 0)
            {
                return Respuesta.ResultadoOperacion<string>.CrearFalla("La lista de consecutivos no puede ser nula o estar vacía");
            }
            var lst = new StringBuilder();
            lst.Append("<TARJA>");
            foreach (var i in consecutivos)
            {
                lst.AppendFormat("<VALOR CONSECUTIVO=\"{0}\"/>", i);
            }
            lst.Append("</TARJA>");
            p.Parametros.Add("subitems", lst.ToString());
            var bcon = p.Accesorio.ObtenerConfiguracion("N4Middleware")?.valor;
            var result = BDOpe.ComandoSelectEscalarRef<string>(bcon, "SELECT [Bill].[verificar_ubicacion](@subitems)", p.Parametros);

            if (!result.Exitoso)
            {
                p.LogEvent(USUARIO, p.actualMetodo,  result.MensajeProblema);
                return Respuesta.ResultadoOperacion<string>.CrearFalla(result.MensajeProblema);
            }
            return ResultadoOperacion<string>.CrearResultadoExitoso(result.Resultado, "Exito al Ejecutar");
        }


    }
}
