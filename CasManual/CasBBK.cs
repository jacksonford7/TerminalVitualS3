using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Configuraciones;
using Aduana;
using Aduanas.Entidades;
using AccesoDatos;

namespace CasManual
{
    public class CasBBK:ModuloBase
    {
        public override void OnInstanceCreate()
        {
            this.alterClase = "CAS";
            base.OnInstanceCreate();
            this.Accesorio.ConfiguracionBase = "DATACON";
        }
        public Int64 id { get; set; }
        public string mrn { get; set; }
        public string msn { get; set; }
        public string hsn { get; set; }

        //obtenido desde el manifiestp
        public int? total_items_manifiesto { get;  set; }
        public DateTime? fecha_manifiesto { get;  set; }
        public string consignatario_manifiesto { get; set; }

         public string consignatario_manifiesto_id { get; set; }

     public string contenedor_manifiesto { get; set; }
        public string bl_manifiesto { get; set; }
         public string descripcion_manifiesto { get; set; }
        public string desconsolidador_manifiesto { get; set; }

          public Int64? id_manifiesto { get;  set; }
        public Int64? id_manifiesto_detalle { get; set; }

        public DateTime? fecha_libera { get; set; }
        public string usuario_libera { get; set; }

        public DateTime? fecha_modifica { get; set; }
        public string usuario_modifica { get; set; }

        public DateTime? fecha_registro { get; set; }
        public decimal? peso_total { get; set; }

        public bool activo { get; set; }

        //nuemro de carga debe existir,

        public string desconsolidador_asigna_id { get; set; }
        public string desconsolidador_asigna_nombre { get; set; }

        public string desconsolidador_naviera { get; set; }

        public Respuesta.ResultadoOperacion<Int64> NuevaCas()
        {
            this.actualMetodo = MethodBase.GetCurrentMethod().Name;
            //inicializa
            string pv = string.Empty;
            if (!this.Accesorio.Inicializar(out pv))
            {
                return Respuesta.ResultadoOperacion<Int64>.CrearFalla(pv);
            }
            var tt = SetMessage("NO_NULO", actualMetodo, usuario_libera);
            if (!this.fecha_libera.HasValue)
            {
                return Respuesta.ResultadoOperacion<Int64>.CrearFalla(string.Format(tt.Item1, nameof(fecha_libera)));
            }

            if (string.IsNullOrEmpty(this.mrn))
            {
               return Respuesta.ResultadoOperacion<Int64>.CrearFalla(string.Format(tt.Item1,nameof(mrn)));
            }
            if (string.IsNullOrEmpty(this.msn))
            {
                return Respuesta.ResultadoOperacion<Int64>.CrearFalla(string.Format(tt.Item1, nameof(msn)));
            }
            if (string.IsNullOrEmpty(this.hsn))
            {
                return Respuesta.ResultadoOperacion<Int64>.CrearFalla(string.Format(tt.Item1, nameof(hsn)));
            }
            if (string.IsNullOrEmpty(this.usuario_libera))
            {
                return Respuesta.ResultadoOperacion<Int64>.CrearFalla(string.Format(tt.Item1, nameof(usuario_libera)));
            }
            this.fecha_libera = fecha_libera.HasValue ? fecha_libera.Value : DateTime.Now;
            this.fecha_registro = DateTime.Now;
            this.id = 0;
 
            //LLame a ecuapas traiga el Bl

          // tt = SetMessage("NO_MRN", actualMetodo, usuario_libera);
            //var ecu_resp = Manifiesto.ObtenerManifiesto(this.usuario_libera, this.mrn, this.msn, this.hsn);
            //if (!ecu_resp.Exitoso)
            //{
            //    return Respuesta.ResultadoOperacion<Int64>.CrearFalla(string.Format(tt.Item1, mrn+msn+hsn), tt.Item2);
            //}

            //this.total_items_manifiesto = ecu_resp.Resultado.equipos;
            //this.fecha_manifiesto = ecu_resp.Resultado.fecha;
            //this.consignatario_manifiesto = ecu_resp.Resultado.importador_id;
            //ok preparar la entidad



            this.Parametros.Clear();
            this.Parametros.Add(nameof(mrn),mrn);
            this.Parametros.Add(nameof(msn), msn);
            this.Parametros.Add(nameof(hsn), hsn);
           
            this.Parametros.Add(nameof(total_items_manifiesto), total_items_manifiesto);
            this.Parametros.Add(nameof(fecha_manifiesto), fecha_manifiesto);
            this.Parametros.Add(nameof(consignatario_manifiesto), consignatario_manifiesto);

            this.Parametros.Add(nameof(fecha_libera), fecha_libera);
            this.Parametros.Add(nameof(usuario_libera), usuario_libera);

            this.Parametros.Add(nameof(consignatario_manifiesto_id), consignatario_manifiesto_id);
            this.Parametros.Add(nameof(contenedor_manifiesto), contenedor_manifiesto);
            this.Parametros.Add(nameof(bl_manifiesto), bl_manifiesto);
            this.Parametros.Add(nameof(descripcion_manifiesto), descripcion_manifiesto);
            this.Parametros.Add(nameof(desconsolidador_manifiesto), desconsolidador_manifiesto);
           
            this.Parametros.Add(nameof(id_manifiesto), id_manifiesto);
            this.Parametros.Add(nameof(id_manifiesto_detalle), id_manifiesto_detalle);

            //obtener forwarder.
           

            this.Parametros.Add(nameof(desconsolidador_asigna_id), desconsolidador_asigna_id);
            this.Parametros.Add(nameof(desconsolidador_asigna_nombre), desconsolidador_asigna_nombre);


            var bcon = this.Accesorio.ObtenerConfiguracion("Billion")?.valor;
            //YA_ASIGNADO   
#if DEBUG
            this.LogEvent(usuario_libera,this.actualMetodo,"Traza");
#endif
            var result =  BDOpe.ComandoInsertUpdateDeleteID(bcon, "[Bill].[nueva_cas_manual]",this.Parametros);
            /*bill.upsert_pago_asignado*/
            if (!result.Exitoso)
            {
                return Respuesta.ResultadoOperacion<Int64>.CrearFalla(result.MensajeProblema);

            }
            this.id = result.Resultado.HasValue ? result.Resultado.Value : -1;
            return Respuesta.ResultadoOperacion<Int64>.CrearResultadoExitoso(result.Resultado.HasValue?result.Resultado.Value:-1);
        }


        public Respuesta.ResultadoOperacion<int> BorrarCas()
        {
            this.actualMetodo = MethodBase.GetCurrentMethod().Name;

            var tt = SetMessage("NO_NULO", actualMetodo, usuario_modifica);
            if (string.IsNullOrEmpty(this.usuario_modifica))
            {
                return Respuesta.ResultadoOperacion<int>.CrearFalla(string.Format(tt.Item1, nameof(usuario_modifica)));
            }
            if (this.id <= 0)
            {
                return Respuesta.ResultadoOperacion<int>.CrearFalla(string.Format(tt.Item1, nameof(id)));
            }
            //ok preparar la entidad
            string pv = string.Empty;
            //FALLA INICIALIZACION
            if (!this.Accesorio.Inicializar(out pv))
            {
                return Respuesta.ResultadoOperacion<int>.CrearFalla(pv);
            }
            this.Parametros.Clear();
            this.Parametros.Add("modifica", usuario_modifica);
            this.Parametros.Add("id", id);
            var bcon = this.Accesorio.ObtenerConfiguracion("Billion")?.valor;
            //YA_ASIGNADO
#if DEBUG
            this.LogEvent(usuario_modifica, this.actualMetodo, "Traza");
#endif
            var result = BDOpe.ComandoInsertUpdateDeleteFila(bcon, "[Bill].[borrar_cas_manual]", this.Parametros);
            return Respuesta.ResultadoOperacion<int>.CrearResultadoExitoso(result.Resultado > 0 ? result.Resultado : -1);
        }
        public static Respuesta.ResultadoOperacion<List<CasBBK>> ListaAsignacion(string usuario, DateTime? desde = null, DateTime? hasta = null)
        {
            var p = new CasBBK();
            p.actualMetodo = MethodBase.GetCurrentMethod().Name;
            string pv;
            if (!p.Accesorio.Inicializar(out pv))
            {
                return null;
            }
            var tt = p.SetMessage("NO_NULO", p.actualMetodo, usuario);
            p.Parametros.Clear();
          
            if (desde.HasValue)
            {
                p.Parametros.Add("desde", desde.Value);
            }
            if (hasta.HasValue)
            {
                p.Parametros.Add("hasta", hasta.Value);
            }


            if (string.IsNullOrEmpty(usuario))
            {
                return Respuesta.ResultadoOperacion<List<CasBBK>>.CrearFalla(string.Format(tt.Item1, nameof(usuario)), tt.Item2);
            }

            p.Parametros.Add("usuario", usuario);

            var bcon = p.Accesorio.ObtenerConfiguracion("Billion")?.valor;
            var result = BDOpe.ComandoSelectALista<CasBBK>(bcon, "[Bill].[listar_pago_asignado]", p.Parametros);
            if (!result.Exitoso)
            {
                p.LogEvent(usuario, p.actualMetodo, result.MensajeProblema);
                return Respuesta.ResultadoOperacion<List<CasBBK>>.CrearFalla(result.MensajeProblema, result.MensajeInformacion);
            }
            return Respuesta.ResultadoOperacion<List<CasBBK>>.CrearResultadoExitoso(result.Resultado, result.MensajeInformacion);
        }
        public static Respuesta.ResultadoOperacion<List<CasBBK>> ListaCasPartida(string usuario, string mrn, string msn, string hsn)
        {
            var p = new CasBBK();
            p.actualMetodo = MethodBase.GetCurrentMethod().Name;
            string pv;
            if (!p.Accesorio.Inicializar(out pv))
            {
                return null;
            }
            var tt = p.SetMessage("NO_NULO", p.actualMetodo, usuario);
            p.Parametros.Clear();
            p.Parametros.Add("mrn", mrn); p.Parametros.Add("msn", msn); p.Parametros.Add("hsn", hsn);

            if (string.IsNullOrEmpty(mrn))
            {
                return Respuesta.ResultadoOperacion<List<CasBBK>>.CrearFalla(string.Format(tt.Item1, nameof(mrn)), tt.Item2);
            }
            if (string.IsNullOrEmpty(msn))
            {
                return Respuesta.ResultadoOperacion<List<CasBBK>>.CrearFalla(string.Format(tt.Item1, nameof(msn)), tt.Item2);
            }
            if (string.IsNullOrEmpty(hsn))
            {
                return Respuesta.ResultadoOperacion<List<CasBBK>>.CrearFalla(string.Format(tt.Item1, nameof(hsn)), tt.Item2);
            }

            var bcon = p.Accesorio.ObtenerConfiguracion("Billion")?.valor;
            var result = BDOpe.ComandoSelectALista<CasBBK>(bcon, "[Bill].[listar_partida_cas]", p.Parametros);
            if (!result.Exitoso)
            {
                p.LogEvent(usuario, p.actualMetodo, result.MensajeProblema);
                return Respuesta.ResultadoOperacion<List<CasBBK>>.CrearFalla(result.MensajeProblema, result.MensajeInformacion);
            }
            return Respuesta.ResultadoOperacion<List<CasBBK>>.CrearResultadoExitoso(result.Resultado, result.MensajeInformacion);
        }
        public static Respuesta.ResultadoOperacion<List<CasBBK>> ListarPendientes(string usuario,string desconsolidador_id, string mrn)
        {
            var p = new CasBBK();
            p.actualMetodo = MethodBase.GetCurrentMethod().Name;
            string pv;
            if (!p.Accesorio.Inicializar(out pv))
            {
                return null;
            }
            var tt = p.SetMessage("NO_NULO", p.actualMetodo, usuario);
            p.Parametros.Clear();
            p.Parametros.Add(nameof(mrn), mrn);
            p.Parametros.Add(nameof(desconsolidador_id), desconsolidador_id);

            if (string.IsNullOrEmpty(mrn))
            {
                return Respuesta.ResultadoOperacion<List<CasBBK>>.CrearFalla(string.Format(tt.Item1, nameof(mrn)), tt.Item2);
            }
            if (string.IsNullOrEmpty(desconsolidador_id))
            {
                return Respuesta.ResultadoOperacion<List<CasBBK>>.CrearFalla(string.Format(tt.Item1, nameof(desconsolidador_id)), tt.Item2);
            }

            if (string.IsNullOrEmpty(usuario))
            {
                return Respuesta.ResultadoOperacion<List<CasBBK>>.CrearFalla(string.Format(tt.Item1, nameof(usuario)), tt.Item2);
            }

            var bcon = p.Accesorio.ObtenerConfiguracion("Billion")?.valor;
            var result = BDOpe.ComandoSelectALista<CasBBK>(bcon, "[Bill].[listar_pendientes_cas]", p.Parametros);
            if (!result.Exitoso)
            {
                p.LogEvent(usuario, p.actualMetodo, result.MensajeProblema);
                return Respuesta.ResultadoOperacion<List<CasBBK>>.CrearFalla(result.MensajeProblema, result.MensajeInformacion);
            }
            return Respuesta.ResultadoOperacion<List<CasBBK>>.CrearResultadoExitoso(result.Resultado, result.MensajeInformacion);
        }


          public static Respuesta.ResultadoOperacion<List<CasBBK>> ListarParaCorregir(string usuario, string mrn, string msn, string hsn)
        {
            var p = new CasBBK();
            p.actualMetodo = MethodBase.GetCurrentMethod().Name;
            string pv;
            if (!p.Accesorio.Inicializar(out pv))
            {
                return null;
            }
            var tt = p.SetMessage("NO_NULO", p.actualMetodo, usuario);
            p.Parametros.Clear();
            p.Parametros.Add(nameof(mrn), mrn);
            p.Parametros.Add(nameof(msn), msn);
            p.Parametros.Add(nameof(hsn), hsn);


            if (string.IsNullOrEmpty(mrn))
            {
                return Respuesta.ResultadoOperacion<List<CasBBK>>.CrearFalla(string.Format(tt.Item1, nameof(mrn)), tt.Item2);
            }

            if (string.IsNullOrEmpty(usuario))
            {
                return Respuesta.ResultadoOperacion<List<CasBBK>>.CrearFalla(string.Format(tt.Item1, nameof(usuario)), tt.Item2);
            }

            var bcon = p.Accesorio.ObtenerConfiguracion("Billion")?.valor;
            var result = BDOpe.ComandoSelectALista<CasBBK>(bcon, "[Bill].[listar_corregir_cas]", p.Parametros);
            if (!result.Exitoso)
            {
                p.LogEvent(usuario, p.actualMetodo, result.MensajeProblema);
                return Respuesta.ResultadoOperacion<List<CasBBK>>.CrearFalla(result.MensajeProblema, result.MensajeInformacion);
            }
            return Respuesta.ResultadoOperacion<List<CasBBK>>.CrearResultadoExitoso(result.Resultado, result.MensajeInformacion);
        }


        //--[Bill].[listar_autorizadas_cas]---//
        public static Respuesta.ResultadoOperacion<List<CasBBK>> ListarAutorizadas(string usuario, string desconsolidador_id, string mrn)
        {
            var p = new CasBBK();
            p.actualMetodo = MethodBase.GetCurrentMethod().Name;
            string pv;
            if (!p.Accesorio.Inicializar(out pv))
            {
                return null;
            }
            var tt = p.SetMessage("NO_NULO", p.actualMetodo, usuario);
            p.Parametros.Clear();
            p.Parametros.Add(nameof(mrn), mrn);
            p.Parametros.Add(nameof(desconsolidador_id), desconsolidador_id);

            if (string.IsNullOrEmpty(mrn))
            {
                return Respuesta.ResultadoOperacion<List<CasBBK>>.CrearFalla(string.Format(tt.Item1, nameof(mrn)), tt.Item2);
            }
            if (string.IsNullOrEmpty(desconsolidador_id))
            {
                return Respuesta.ResultadoOperacion<List<CasBBK>>.CrearFalla(string.Format(tt.Item1, nameof(desconsolidador_id)), tt.Item2);
            }

            if (string.IsNullOrEmpty(usuario))
            {
                return Respuesta.ResultadoOperacion<List<CasBBK>>.CrearFalla(string.Format(tt.Item1, nameof(usuario)), tt.Item2);
            }

            var bcon = p.Accesorio.ObtenerConfiguracion("Billion")?.valor;
            var result = BDOpe.ComandoSelectALista<CasBBK>(bcon, "[Bill].[listar_autorizadas_cas]", p.Parametros);
            if (!result.Exitoso)
            {
                p.LogEvent(usuario, p.actualMetodo, result.MensajeProblema);
                return Respuesta.ResultadoOperacion<List<CasBBK>>.CrearFalla(result.MensajeProblema, result.MensajeInformacion);
            }
            return Respuesta.ResultadoOperacion<List<CasBBK>>.CrearResultadoExitoso(result.Resultado, result.MensajeInformacion);
        }


        #region "cas break bulk"
        public static Respuesta.ResultadoOperacion<List<CasBBK>> ListarPendientesBrbk(string usuario, string desconsolidador_id, string mrn)
        {
            var p = new CasBBK();
            p.actualMetodo = MethodBase.GetCurrentMethod().Name;
            string pv;
            if (!p.Accesorio.Inicializar(out pv))
            {
                return null;
            }
            var tt = p.SetMessage("NO_NULO", p.actualMetodo, usuario);
            p.Parametros.Clear();
            p.Parametros.Add(nameof(mrn), mrn);
            p.Parametros.Add(nameof(desconsolidador_id), desconsolidador_id);

            if (string.IsNullOrEmpty(mrn))
            {
                return Respuesta.ResultadoOperacion<List<CasBBK>>.CrearFalla(string.Format(tt.Item1, nameof(mrn)), tt.Item2);
            }
            if (string.IsNullOrEmpty(desconsolidador_id))
            {
                return Respuesta.ResultadoOperacion<List<CasBBK>>.CrearFalla(string.Format(tt.Item1, nameof(desconsolidador_id)), tt.Item2);
            }

            if (string.IsNullOrEmpty(usuario))
            {
                return Respuesta.ResultadoOperacion<List<CasBBK>>.CrearFalla(string.Format(tt.Item1, nameof(usuario)), tt.Item2);
            }

            var bcon = p.Accesorio.ObtenerConfiguracion("Billion")?.valor;
            var result = BDOpe.ComandoSelectALista<CasBBK>(bcon, "[Bill].[listar_pendientes_cas_bbk]", p.Parametros);
            if (!result.Exitoso)
            {
                p.LogEvent(usuario, p.actualMetodo, result.MensajeProblema);
                return Respuesta.ResultadoOperacion<List<CasBBK>>.CrearFalla(result.MensajeProblema, result.MensajeInformacion);
            }
            return Respuesta.ResultadoOperacion<List<CasBBK>>.CrearResultadoExitoso(result.Resultado, result.MensajeInformacion);
        }

        public Respuesta.ResultadoOperacion<Int64> NuevaCasBrbk()
        {
            this.actualMetodo = MethodBase.GetCurrentMethod().Name;
            //inicializa
            string pv = string.Empty;
            if (!this.Accesorio.Inicializar(out pv))
            {
                return Respuesta.ResultadoOperacion<Int64>.CrearFalla(pv);
            }
            var tt = SetMessage("NO_NULO", actualMetodo, usuario_libera);
            if (!this.fecha_libera.HasValue)
            {
                return Respuesta.ResultadoOperacion<Int64>.CrearFalla(string.Format(tt.Item1, nameof(fecha_libera)));
            }

            if (string.IsNullOrEmpty(this.mrn))
            {
                return Respuesta.ResultadoOperacion<Int64>.CrearFalla(string.Format(tt.Item1, nameof(mrn)));
            }
            if (string.IsNullOrEmpty(this.msn))
            {
                return Respuesta.ResultadoOperacion<Int64>.CrearFalla(string.Format(tt.Item1, nameof(msn)));
            }
            if (string.IsNullOrEmpty(this.hsn))
            {
                return Respuesta.ResultadoOperacion<Int64>.CrearFalla(string.Format(tt.Item1, nameof(hsn)));
            }
            if (string.IsNullOrEmpty(this.usuario_libera))
            {
                return Respuesta.ResultadoOperacion<Int64>.CrearFalla(string.Format(tt.Item1, nameof(usuario_libera)));
            }
            this.fecha_libera = fecha_libera.HasValue ? fecha_libera.Value : DateTime.Now;
            this.fecha_registro = DateTime.Now;
            this.id = 0;

            this.Parametros.Clear();
            this.Parametros.Add(nameof(mrn), mrn);
            this.Parametros.Add(nameof(msn), msn);
            this.Parametros.Add(nameof(hsn), hsn);

            this.Parametros.Add(nameof(total_items_manifiesto), total_items_manifiesto);
            this.Parametros.Add(nameof(fecha_manifiesto), fecha_manifiesto);
            this.Parametros.Add(nameof(consignatario_manifiesto), consignatario_manifiesto);

            this.Parametros.Add(nameof(fecha_libera), fecha_libera);
            this.Parametros.Add(nameof(usuario_libera), usuario_libera);

            this.Parametros.Add(nameof(consignatario_manifiesto_id), consignatario_manifiesto_id);
            this.Parametros.Add(nameof(contenedor_manifiesto), contenedor_manifiesto);
            this.Parametros.Add(nameof(bl_manifiesto), bl_manifiesto);
            this.Parametros.Add(nameof(descripcion_manifiesto), descripcion_manifiesto);
            this.Parametros.Add(nameof(desconsolidador_manifiesto), desconsolidador_manifiesto);

            this.Parametros.Add(nameof(id_manifiesto), id_manifiesto);
            this.Parametros.Add(nameof(id_manifiesto_detalle), id_manifiesto_detalle);

            //obtener forwarder.


            this.Parametros.Add(nameof(desconsolidador_asigna_id), desconsolidador_asigna_id);
            this.Parametros.Add(nameof(desconsolidador_asigna_nombre), desconsolidador_asigna_nombre);
            this.Parametros.Add(nameof(desconsolidador_naviera), desconsolidador_naviera);


            var bcon = this.Accesorio.ObtenerConfiguracion("Billion")?.valor;
            //YA_ASIGNADO   
#if DEBUG
            this.LogEvent(usuario_libera, this.actualMetodo, "Traza");
#endif
            var result = BDOpe.ComandoInsertUpdateDeleteID(bcon, "[Bill].[nueva_cas_manual_brbk]", this.Parametros);
            /*bill.upsert_pago_asignado*/
            if (!result.Exitoso)
            {
                return Respuesta.ResultadoOperacion<Int64>.CrearFalla(result.MensajeProblema);

            }
            this.id = result.Resultado.HasValue ? result.Resultado.Value : -1;
            return Respuesta.ResultadoOperacion<Int64>.CrearResultadoExitoso(result.Resultado.HasValue ? result.Resultado.Value : -1);
        }

        public static Respuesta.ResultadoOperacion<List<CasBBK>> ListaCasPartidaBrbk(string usuario, string mrn, string msn, string hsn)
        {
            var p = new CasBBK();
            p.actualMetodo = MethodBase.GetCurrentMethod().Name;
            string pv;
            if (!p.Accesorio.Inicializar(out pv))
            {
                return null;
            }
            var tt = p.SetMessage("NO_NULO", p.actualMetodo, usuario);
            p.Parametros.Clear();
            p.Parametros.Add("mrn", mrn); p.Parametros.Add("msn", msn); p.Parametros.Add("hsn", hsn);

            if (string.IsNullOrEmpty(mrn))
            {
                return Respuesta.ResultadoOperacion<List<CasBBK>>.CrearFalla(string.Format(tt.Item1, nameof(mrn)), tt.Item2);
            }
            if (string.IsNullOrEmpty(msn))
            {
                return Respuesta.ResultadoOperacion<List<CasBBK>>.CrearFalla(string.Format(tt.Item1, nameof(msn)), tt.Item2);
            }
            if (string.IsNullOrEmpty(hsn))
            {
                return Respuesta.ResultadoOperacion<List<CasBBK>>.CrearFalla(string.Format(tt.Item1, nameof(hsn)), tt.Item2);
            }

            var bcon = p.Accesorio.ObtenerConfiguracion("Billion")?.valor;
            var result = BDOpe.ComandoSelectALista<CasBBK>(bcon, "[Bill].[listar_partida_cas_brbk]", p.Parametros);
            if (!result.Exitoso)
            {
                p.LogEvent(usuario, p.actualMetodo, result.MensajeProblema);
                return Respuesta.ResultadoOperacion<List<CasBBK>>.CrearFalla(result.MensajeProblema, result.MensajeInformacion);
            }
            return Respuesta.ResultadoOperacion<List<CasBBK>>.CrearResultadoExitoso(result.Resultado, result.MensajeInformacion);
        }

        #endregion

    }

}
