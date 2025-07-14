using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Configuraciones;
using AccesoDatos;
using Respuesta;
using System.Reflection;

namespace Aduana.Importacion
{
    [Serializable]
    [XmlRoot(ElementName = "ecu_validacion_cntr")]
    public class ecu_validacion_cntr : ModuloBase
    {

        public override void OnInstanceCreate()
        {
            this.alterClase = "ADUANA";
            base.OnInstanceCreate();
            this.Accesorio.ConfiguracionBase = "DATACON";
        }


        public ecu_validacion_cntr() : base()
        {

        }


        public ResultadoOperacion<List<ecu_validacion_cntr>> CargaPorManifiestoImpo(string usuario, string mrn, string msn, string hsn)
        {
            this.actualMetodo = MethodBase.GetCurrentMethod().Name;
            //inicializa
            string pv = string.Empty;
            if (!this.Accesorio.Inicializar(out pv))
            {
                return Respuesta.ResultadoOperacion<List<ecu_validacion_cntr>>.CrearFalla(pv);
            }
            //fin inicializa

            var tt = SetMessage("NO_NULO", actualMetodo, usuario);
            if (string.IsNullOrEmpty(mrn))
            {
                return Respuesta.ResultadoOperacion<List<ecu_validacion_cntr>>.CrearFalla(string.Format(tt.Item1, nameof(mrn)));
            }
            if (string.IsNullOrEmpty(msn))
            {
                return Respuesta.ResultadoOperacion<List<ecu_validacion_cntr>>.CrearFalla(string.Format(tt.Item1, nameof(msn)));
            }
            if (string.IsNullOrEmpty(hsn))
            {
                return Respuesta.ResultadoOperacion<List<ecu_validacion_cntr>>.CrearFalla(string.Format(tt.Item1, nameof(hsn)));
            }
            if (string.IsNullOrEmpty(usuario))
            {
                return Respuesta.ResultadoOperacion<List<ecu_validacion_cntr>>.CrearFalla(string.Format(tt.Item1, nameof(usuario)));
            }



            this.Parametros.Add("mrn", mrn); this.Parametros.Add("msn", msn); this.Parametros.Add("hsn", hsn);

            var bcon = this.Accesorio.ObtenerConfiguracion("ECUAPASS")?.valor;

            var rp = BDOpe.ComandoSelectALista<ecu_validacion_cntr>(bcon, "[Bill].[validacion_cntr_impo_mrn]", this.Parametros);
            return rp.Exitoso ? ResultadoOperacion<List<ecu_validacion_cntr>>.CrearResultadoExitoso(rp.Resultado) : ResultadoOperacion<List<ecu_validacion_cntr>>.CrearFalla(rp.MensajeProblema);
        }

        public ResultadoOperacion<List<ecu_validacion_cntr>> CargaPorManifiestoImpoPro(string usuario, string mrn, string msn, string hsn, string ruc)
        {
            this.actualMetodo = MethodBase.GetCurrentMethod().Name;
            //inicializa
            string pv = string.Empty;
            if (!this.Accesorio.Inicializar(out pv))
            {
                return Respuesta.ResultadoOperacion<List<ecu_validacion_cntr>>.CrearFalla(pv);
            }
            //fin inicializa

            var tt = SetMessage("NO_NULO", actualMetodo, usuario);
            if (string.IsNullOrEmpty(mrn))
            {
                return Respuesta.ResultadoOperacion<List<ecu_validacion_cntr>>.CrearFalla(string.Format(tt.Item1, nameof(mrn)));
            }
            if (string.IsNullOrEmpty(msn))
            {
                return Respuesta.ResultadoOperacion<List<ecu_validacion_cntr>>.CrearFalla(string.Format(tt.Item1, nameof(msn)));
            }
            if (string.IsNullOrEmpty(hsn))
            {
                return Respuesta.ResultadoOperacion<List<ecu_validacion_cntr>>.CrearFalla(string.Format(tt.Item1, nameof(hsn)));
            }
            if (string.IsNullOrEmpty(usuario))
            {
                return Respuesta.ResultadoOperacion<List<ecu_validacion_cntr>>.CrearFalla(string.Format(tt.Item1, nameof(usuario)));
            }

            if (string.IsNullOrEmpty(ruc))
            {
                return Respuesta.ResultadoOperacion<List<ecu_validacion_cntr>>.CrearFalla(string.Format(tt.Item1, nameof(ruc)));
            }

            this.Parametros.Add("mrn", mrn); this.Parametros.Add("msn", msn); this.Parametros.Add("hsn", hsn); this.Parametros.Add("ruc", ruc);

            var bcon = this.Accesorio.ObtenerConfiguracion("ECUAPASS")?.valor;

            var rp = BDOpe.ComandoSelectALista<ecu_validacion_cntr>(bcon, "[Bill].[validacion_cntr_impo_mrn_ruc_pro]", this.Parametros);
            return rp.Exitoso ? ResultadoOperacion<List<ecu_validacion_cntr>>.CrearResultadoExitoso(rp.Resultado) : ResultadoOperacion<List<ecu_validacion_cntr>>.CrearFalla(rp.MensajeProblema);
        }
        public ResultadoOperacion<List<ecu_validacion_cntr>> CargaPorManifiestoImpo(string usuario, string mrn, string msn, string hsn, bool validar)
        {
            this.actualMetodo = MethodBase.GetCurrentMethod().Name;
            //inicializa
            string pv = string.Empty;
            if (!this.Accesorio.Inicializar(out pv))
            {
                return Respuesta.ResultadoOperacion<List<ecu_validacion_cntr>>.CrearFalla(pv);
            }
            //fin inicializa

            var tt = SetMessage("NO_NULO", actualMetodo, usuario);
            if (string.IsNullOrEmpty(mrn))
            {
                return Respuesta.ResultadoOperacion<List<ecu_validacion_cntr>>.CrearFalla(string.Format(tt.Item1, nameof(mrn)));
            }
            if (string.IsNullOrEmpty(msn))
            {
                return Respuesta.ResultadoOperacion<List<ecu_validacion_cntr>>.CrearFalla(string.Format(tt.Item1, nameof(msn)));
            }
            if (string.IsNullOrEmpty(hsn))
            {
                return Respuesta.ResultadoOperacion<List<ecu_validacion_cntr>>.CrearFalla(string.Format(tt.Item1, nameof(hsn)));
            }
            if (string.IsNullOrEmpty(usuario))
            {
                return Respuesta.ResultadoOperacion<List<ecu_validacion_cntr>>.CrearFalla(string.Format(tt.Item1, nameof(usuario)));
            }
        

            this.Parametros.Add("mrn", mrn);
            this.Parametros.Add("msn", msn);
            this.Parametros.Add("hsn", hsn);
            this.Parametros.Add("validar", validar);
            var bcon = this.Accesorio.ObtenerConfiguracion("ECUAPASS")?.valor;

#if DEBUG
            this.LogEvent(usuario, actualMetodo, string.Format("{0}-{1}-{2}", mrn, msn, hsn));
#endif
            var rsql = BDOpe.ComandoSelectALista<ecu_validacion_cntr>(bcon, "[Bill].[validacion_cntr_impo_mrn]", this.Parametros);
            return rsql.Exitoso ? ResultadoOperacion<List<ecu_validacion_cntr>>.CrearResultadoExitoso(rsql.Resultado, string.Format("Filas {0}", rsql.Resultado.Count)) : ResultadoOperacion<List<ecu_validacion_cntr>>.CrearFalla(rsql.MensajeProblema);

        }
        public ResultadoOperacion<List<ecu_validacion_cntr>> CargaPorRucImpo(string usuario, string ruc, string codigo )
        {
            //inicializa
            this.actualMetodo = MethodBase.GetCurrentMethod().Name;
            string pv = string.Empty;
            if (!this.Accesorio.Inicializar(out pv))
            {
                return Respuesta.ResultadoOperacion<List<ecu_validacion_cntr>>.CrearFalla(pv);
            }
            //fin inicializa

            var tt = SetMessage("NO_NULO", actualMetodo, usuario);
            if (string.IsNullOrEmpty(ruc) && string.IsNullOrEmpty(codigo))
            {
                return Respuesta.ResultadoOperacion<List<ecu_validacion_cntr>>.CrearFalla(string.Format(tt.Item1, nameof(mrn)+'/'+nameof(codigo)));
            }
            if (string.IsNullOrEmpty(usuario))
            {
                return Respuesta.ResultadoOperacion<List<ecu_validacion_cntr>>.CrearFalla(string.Format(tt.Item1, nameof(msn)));
            }

            if (!string.IsNullOrEmpty(ruc)) { this.Parametros.Add("ruc", ruc); }
            if (!string.IsNullOrEmpty(codigo)) { this.Parametros.Add("codigo", codigo); }

            var bcon = this.Accesorio.ObtenerConfiguracion("ECUAPASS")?.valor;
#if DEBUG
            this.LogEvent(usuario, actualMetodo, string.Format("{0}-{1}-{2}", mrn, msn, hsn));
#endif
            var rsql = BDOpe.ComandoSelectALista<ecu_validacion_cntr>(bcon, "[Bill].[validacion_cntr_impo_ruc]", this.Parametros);
            return rsql.Exitoso ? ResultadoOperacion<List<ecu_validacion_cntr>>.CrearResultadoExitoso(rsql.Resultado, string.Format("Filas {0}", rsql.Resultado.Count)) : ResultadoOperacion<List<ecu_validacion_cntr>>.CrearFalla(rsql.MensajeProblema);
        }
        public ResultadoOperacion<List<ecu_validacion_cntr>> CargaPorCodigoImpo(string usuario, string codigo)
        {
            //inicializa
            this.actualMetodo = MethodBase.GetCurrentMethod().Name;
            string pv = string.Empty;
            if (!this.Accesorio.Inicializar(out pv))
            {
                return Respuesta.ResultadoOperacion<List<ecu_validacion_cntr>>.CrearFalla(pv);
            }
            //fin inicializa
            var tt = SetMessage("NO_NULO", actualMetodo, usuario);
            if (string.IsNullOrEmpty(codigo))
            {
                return Respuesta.ResultadoOperacion<List<ecu_validacion_cntr>>.CrearFalla(string.Format(tt.Item1, nameof(mrn)));
            }
            if (string.IsNullOrEmpty(usuario))
            {
                return Respuesta.ResultadoOperacion<List<ecu_validacion_cntr>>.CrearFalla(string.Format(tt.Item1, nameof(msn)));
            }
            var bcon = this.Accesorio.ObtenerConfiguracion("ECUAPASS")?.valor;
            this.Parametros.Add("codigo", codigo);
#if DEBUG
            this.LogEvent(usuario, actualMetodo, codigo);
#endif
            var rp = BDOpe.ComandoSelectALista<ecu_validacion_cntr>(bcon, "[Bill].[validacion_cntr_impo_codigo]", this.Parametros);
            return rp.Exitoso ? ResultadoOperacion<List<ecu_validacion_cntr>>.CrearResultadoExitoso(rp.Resultado) : ResultadoOperacion<List<ecu_validacion_cntr>>.CrearFalla(rp.MensajeProblema);
        }
        public ResultadoOperacion<List<ecu_validacion_cntr>> CargaPorManifiestoImpo(string usuario, string agente_id, string ruc, string mrn, string msn, string hsn)
        {
            this.actualMetodo = MethodBase.GetCurrentMethod().Name;
            //inicializa
            string pv = string.Empty;
            if (!this.Accesorio.Inicializar(out pv))
            {
                return Respuesta.ResultadoOperacion<List<ecu_validacion_cntr>>.CrearFalla(pv);
            }
            //fin inicializa

            var tt = SetMessage("NO_NULO", actualMetodo, usuario);
            if (string.IsNullOrEmpty(mrn) && string.IsNullOrEmpty(agente))
            {
                return Respuesta.ResultadoOperacion<List<ecu_validacion_cntr>>.CrearFalla(string.Format(tt.Item1, nameof(mrn)+'/'+nameof(agente_id)));
            }
            if (string.IsNullOrEmpty(msn))
            {
                return Respuesta.ResultadoOperacion<List<ecu_validacion_cntr>>.CrearFalla(string.Format(tt.Item1, nameof(msn)));
            }
            if (string.IsNullOrEmpty(hsn))
            {
                return Respuesta.ResultadoOperacion<List<ecu_validacion_cntr>>.CrearFalla(string.Format(tt.Item1, nameof(hsn)));
            }
            if (string.IsNullOrEmpty(usuario))
            {
                return Respuesta.ResultadoOperacion<List<ecu_validacion_cntr>>.CrearFalla(string.Format(tt.Item1, nameof(usuario)));
            }

            if (string.IsNullOrEmpty(ruc))
            {
                return Respuesta.ResultadoOperacion<List<ecu_validacion_cntr>>.CrearFalla(string.Format(tt.Item1, nameof(ruc)));
            }

            var bcon = this.Accesorio.ObtenerConfiguracion("ECUAPASS")?.valor;
            this.Parametros.Add("mrn", mrn);
            this.Parametros.Add("msn", msn);
            this.Parametros.Add("hsn", hsn);
            if (!string.IsNullOrEmpty(ruc)) { this.Parametros.Add("ruc", ruc); }
            if (!string.IsNullOrEmpty(agente_id)) { this.Parametros.Add("codigo", agente_id); }

            
#if DEBUG
            this.LogEvent(usuario, actualMetodo, string.Format("{0}-{1}-{2},{3}", mrn, msn, hsn,ruc));
#endif
            var rp = BDOpe.ComandoSelectALista<ecu_validacion_cntr>(bcon, "[Bill].[validacion_cntr_impo_mrn_ruc]", this.Parametros);
            return rp.Exitoso ? ResultadoOperacion<List<ecu_validacion_cntr>>.CrearResultadoExitoso(rp.Resultado) : ResultadoOperacion<List<ecu_validacion_cntr>>.CrearFalla(rp.MensajeProblema);
        }
        public ResultadoOperacion<List<ecu_validacion_cntr>> CargaPorManifiestoImpo(string usuario, string ruc, string agente_id, string mrn, string msn, string hsn, bool validar)
        {
            this.actualMetodo = MethodBase.GetCurrentMethod().Name;
            //inicializa
            string pv = string.Empty;
            if (!this.Accesorio.Inicializar(out pv))
            {
                return Respuesta.ResultadoOperacion<List<ecu_validacion_cntr>>.CrearFalla(pv);
            }
            //fin inicializa

            var tt = SetMessage("NO_NULO", actualMetodo, usuario);
            if (string.IsNullOrEmpty(mrn))
            {
                return Respuesta.ResultadoOperacion<List<ecu_validacion_cntr>>.CrearFalla(string.Format(tt.Item1, nameof(mrn)));
            }
            if (string.IsNullOrEmpty(msn))
            {
                return Respuesta.ResultadoOperacion<List<ecu_validacion_cntr>>.CrearFalla(string.Format(tt.Item1, nameof(msn)));
            }
            if (string.IsNullOrEmpty(hsn))
            {
                return Respuesta.ResultadoOperacion<List<ecu_validacion_cntr>>.CrearFalla(string.Format(tt.Item1, nameof(hsn)));
            }
            if (string.IsNullOrEmpty(usuario))
            {
                return Respuesta.ResultadoOperacion<List<ecu_validacion_cntr>>.CrearFalla(string.Format(tt.Item1, nameof(usuario)));
            }

            if (string.IsNullOrEmpty(ruc) && string.IsNullOrEmpty(agente_id))
            {
                return Respuesta.ResultadoOperacion<List<ecu_validacion_cntr>>.CrearFalla(string.Format(tt.Item1, nameof(ruc)+'/'+nameof(agente_id)));
            }

            var bcon = this.Accesorio.ObtenerConfiguracion("ECUAPASS")?.valor;
            this.Parametros.Add("mrn", mrn); this.Parametros.Add("msn", msn); this.Parametros.Add("hsn", hsn);
            this.Parametros.Add("validar", validar);

            if (!string.IsNullOrEmpty(ruc)) { this.Parametros.Add("ruc", ruc); }
            if (!string.IsNullOrEmpty(agente_id)) { this.Parametros.Add("codigo", agente_id); }

#if DEBUG
            this.LogEvent(usuario, actualMetodo, string.Format("{0}-{1}-{2},{3}", mrn, msn, hsn, ruc));
#endif
            var rp = BDOpe.ComandoSelectALista<ecu_validacion_cntr>(bcon, "[Bill].[validacion_cntr_impo_mrn_ruc]", this.Parametros);
            return rp.Exitoso ? ResultadoOperacion<List<ecu_validacion_cntr>>.CrearResultadoExitoso(rp.Resultado) : ResultadoOperacion<List<ecu_validacion_cntr>>.CrearFalla(rp.MensajeProblema);
        }

        // ojo derco
        public ResultadoOperacion<List<ecu_validacion_cntr>> CargaPorManifiestoImpoCntr(string usuario, string ruc, string agente_id, string mrn, string msn, string hsn, string cntr, bool validar)
        {
            this.actualMetodo = MethodBase.GetCurrentMethod().Name;
            //inicializa
            string pv = string.Empty;
            if (!this.Accesorio.Inicializar(out pv))
            {
                return Respuesta.ResultadoOperacion<List<ecu_validacion_cntr>>.CrearFalla(pv);
            }
            //fin inicializa

            var tt = SetMessage("NO_NULO", actualMetodo, usuario);
       
            if (string.IsNullOrEmpty(usuario))
            {
                return Respuesta.ResultadoOperacion<List<ecu_validacion_cntr>>.CrearFalla(string.Format(tt.Item1, nameof(usuario)));
            }

            if (string.IsNullOrEmpty(ruc) && string.IsNullOrEmpty(agente_id))
            {
                return Respuesta.ResultadoOperacion<List<ecu_validacion_cntr>>.CrearFalla(string.Format(tt.Item1, nameof(ruc) + '/' + nameof(agente_id)));
            }

            var bcon = this.Accesorio.ObtenerConfiguracion("ECUAPASS")?.valor;
            this.Parametros.Add("mrn", mrn);
            this.Parametros.Add("msn", msn);
            this.Parametros.Add("hsn", hsn);
            this.Parametros.Add("cntr", cntr);
            this.Parametros.Add("validar", validar);

            if (!string.IsNullOrEmpty(ruc)) { this.Parametros.Add("ruc", ruc); }
            if (!string.IsNullOrEmpty(agente_id)) { this.Parametros.Add("codigo", agente_id); }

#if DEBUG
            this.LogEvent(usuario, actualMetodo, string.Format("{0}-{1}-{2},{3}", mrn, msn, hsn, ruc));
#endif
            var rp = BDOpe.ComandoSelectALista<ecu_validacion_cntr>(bcon, "[Bill].[validacion_cntr_impo_mrn_ruc_cntr]", this.Parametros);
            return rp.Exitoso ? ResultadoOperacion<List<ecu_validacion_cntr>>.CrearResultadoExitoso(rp.Resultado) : ResultadoOperacion<List<ecu_validacion_cntr>>.CrearFalla(rp.MensajeProblema);
        }


        public ResultadoOperacion<List<ecu_validacion_cntr>> CargaPorManifiestoImpoContenedor(string usuario, string ruc, string cntr, bool validar)
        {
            this.actualMetodo = MethodBase.GetCurrentMethod().Name;
            //inicializa
            string pv = string.Empty;
            if (!this.Accesorio.Inicializar(out pv))
            {
                return Respuesta.ResultadoOperacion<List<ecu_validacion_cntr>>.CrearFalla(pv);
            }
            //fin inicializa

            var tt = SetMessage("NO_NULO", actualMetodo, usuario);
            if (string.IsNullOrEmpty(usuario))
            {
                return Respuesta.ResultadoOperacion<List<ecu_validacion_cntr>>.CrearFalla(string.Format(tt.Item1, nameof(usuario)));
            }

            if (string.IsNullOrEmpty(ruc) && string.IsNullOrEmpty(agente_id))
            {
                return Respuesta.ResultadoOperacion<List<ecu_validacion_cntr>>.CrearFalla(string.Format(tt.Item1, nameof(ruc) + '/' + nameof(agente_id)));
            }

            var bcon = this.Accesorio.ObtenerConfiguracion("ECUAPASS")?.valor;
            this.Parametros.Add("cntr", cntr);
            this.Parametros.Add("validar", validar);

#if DEBUG
            this.LogEvent(usuario, actualMetodo, string.Format("{0}", cntr));
#endif
            var rp = BDOpe.ComandoSelectALista<ecu_validacion_cntr>(bcon, "[Bill].[validacion_cntr_impo_mrn_ruc_ccntr]", this.Parametros);
            return rp.Exitoso ? ResultadoOperacion<List<ecu_validacion_cntr>>.CrearResultadoExitoso(rp.Resultado) : ResultadoOperacion<List<ecu_validacion_cntr>>.CrearFalla(rp.MensajeProblema);
        }
        /*validacion_cntr_impo_codigo*/

        public static ResultadoOperacion<List<string>> CargaToListString(List<ecu_validacion_cntr> keys)
        {
            if (keys == null || keys.Count<=0)
            {
                return ResultadoOperacion<List<string>>.CrearFalla("La lista de entidades esta vacía o es nula");
            }
            var te = keys.Where(g => g.gkey.HasValue).Count();
            if (te <= 0)
            {
                return ResultadoOperacion<List<string>>.CrearFalla("No existen códigos de carga que enlistar (columna gkey))",te.ToString());
            }
            var fe = keys.Where(i => i.gkey.HasValue).Select(h => h.gkey.ToString()).ToList();
            return  ResultadoOperacion<List<string>>.CrearResultadoExitoso(fe);
        }

        #region "Entidad"
        [XmlAttribute(AttributeName = "uuid")]
        public Int64? uuid { get; set; }
        [XmlAttribute(AttributeName = "mrn")]
        public string mrn { get; set; }
        [XmlAttribute(AttributeName = "msn")]
        public string msn { get; set; }
        [XmlAttribute(AttributeName = "hsn")]
        public string hsn { get; set; }
        [XmlAttribute(AttributeName = "referencia")]
        public string referencia { get; set; }
        [XmlAttribute(AttributeName = "cntr")]
        public string cntr { get; set; }
        [XmlAttribute(AttributeName = "gkey")]
        public Int64? gkey { get; set; }
        [XmlAttribute(AttributeName = "tipo")]
        public string tipo { get; set; }

        [XmlAttribute(AttributeName = "agente_id")]
        public string agente_id { get; set; }
        [XmlAttribute(AttributeName = "agente")]
        public string agente { get; set; }
        [XmlAttribute(AttributeName = "importador_id")]
        public string importador_id { get; set; }
        [XmlAttribute(AttributeName = "importador")]
        public string importador { get; set; }
        [XmlAttribute(AttributeName = "declaracion")]
        public string declaracion { get; set; }
        [XmlAttribute(AttributeName = "ridt_estado")]
        public string ridt_estado { get; set; }

        //para saber si su imdt esta aprobado y fue corregido
        [XmlAttribute(AttributeName = "imdt_id")]
        public string imdt_id { get; set; }
        [XmlAttribute(AttributeName = "imdt_estado")]
        public string imdt_estado { get; set; }
        [XmlAttribute(AttributeName = "imdt_fecha")]
        public DateTime? imdt_fecha { get; set; }
        [XmlAttribute(AttributeName = "ciis")]
        public bool? ciis { get; set; }

        //para saber si la aduana mando aforo
        [XmlAttribute(AttributeName = "fecha_aforo")]
        public DateTime? fecha_aforo { get; set; }
        //esto es para jugarse el conteo de carga
        [XmlAttribute(AttributeName = "total_partida")]
        public int? total_partida { get; set; }
        [XmlAttribute(AttributeName = "descripcion")]
        public string descripcion { get; set; }
        [XmlAttribute(AttributeName = "documento_bl")]
        public string documento_bl { get; set; }
        [XmlAttribute(AttributeName = "manifiesto_fecha")]
        public DateTime? manifiesto_fecha { get; set; }

        //opcional campos smdt.
        [XmlAttribute(AttributeName = "smdt_id")]
        public string smdt_id { get; set; }
        [XmlAttribute(AttributeName = "smdt_estado")]
        public string smdt_estado { get; set; }
        [XmlAttribute(AttributeName = "smdt_fecha")]
        public DateTime? smdt_fecha { get; set; }


        #endregion

    }



}
