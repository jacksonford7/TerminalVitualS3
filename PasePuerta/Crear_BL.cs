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
    public class Crear_BL : ModuloBase
    {
        public override void OnInstanceCreate()
        {
            this.alterClase = "CAS";
            base.OnInstanceCreate();
            this.Accesorio.ConfiguracionBase = "DATACON";
        }
        public Crear_BL()
        {
            OnInstanceCreate();
            
        }

        public Int64 ID_GENERADO { get; set; } //secuencil
        public string USUARIO_REGISTRO { get; set; }
        public string MENSAJE { get; set; }
        public string XML { get; set; }

        public ResultadoOperacion<Int64> Generar_BL(string NBR, string LINE, string VISIT, string CATEGORY,
            string SHIPPER, string CONSIGNEE, string CARGOFIRST, string CARGOSECOND, string OPERATION,
            string WEIGHT, string VOLUME, string CANWEIGHT, string POL, string NOTES, string ITEMNBR,
            string COMMODITY, string PRODUCT, string PACKAGING, string TOTALWEIGHT, string QTY,
            string MARKS, string POSITION )
        {
            this.actualMetodo = MethodBase.GetCurrentMethod().Name;
            string pv = string.Empty;
            if (!this.Accesorio.Inicializar(out pv))
            {
                return Respuesta.ResultadoOperacion<Int64>.CrearFalla(pv);
            }

            //Validaciones-->
            var tt = SetMessage("NO_NULO", actualMetodo, USUARIO_REGISTRO);
            //aca llamar a n4 y crear appointmen
            if (string.IsNullOrEmpty(this.USUARIO_REGISTRO))
            {
                return Respuesta.ResultadoOperacion<Int64>.CrearFalla(string.Format(tt.Item1, nameof(USUARIO_REGISTRO)));
            }

            N4Ws.Entidad.GroovyCodeExtension code = new N4Ws.Entidad.GroovyCodeExtension();
            code.name = "CGSABillOfLading";
            code.location = "code-extension";
            code.parameters.Add("NBR", NBR);
            code.parameters.Add("LINE", LINE);
            code.parameters.Add("VISIT", VISIT);
            code.parameters.Add("CATEGORY", CATEGORY);
            code.parameters.Add("SHIPPER", SHIPPER);
            code.parameters.Add("CONSIGNEE", CONSIGNEE);
            code.parameters.Add("CARGOFIRST", CARGOFIRST);
            code.parameters.Add("CARGOSECOND", CARGOSECOND);
            code.parameters.Add("OPERATION", OPERATION);
            code.parameters.Add("WEIGHT", WEIGHT);
            code.parameters.Add("VOLUME", VOLUME);
            code.parameters.Add("CANWEIGHT", CANWEIGHT);
            code.parameters.Add("POL", POL);
            code.parameters.Add("NOTES", NOTES);
            code.parameters.Add("ITEMNBR", ITEMNBR);
            code.parameters.Add("COMMODITY", COMMODITY);
            code.parameters.Add("PRODUCT", PRODUCT);
            code.parameters.Add("PACKAGING", PACKAGING);
            code.parameters.Add("TOTALWEIGHT", TOTALWEIGHT);
            code.parameters.Add("QTY", QTY);
            code.parameters.Add("MARKS", MARKS);
            code.parameters.Add("POSITION", POSITION);
            code.parameters.Add("USER", USUARIO_REGISTRO);

            XML = code.ToString();

            //crear el en N4
            var n4r = N4Ws.Entidad.Servicios.EjecutarCODEExtension(code, USUARIO_REGISTRO);


            if (n4r.response.ToString().Contains("<result>ERROR"))
            {
                var ex = new ApplicationException(n4r.status_id);
                var i = this.LogError<ApplicationException>(ex, "Generar_BL", USUARIO_REGISTRO);
                var emsg = string.Format("Ha ocurrido la novedad número {0} durante el proceso de creación del BL, favor comuníquese con SAC", i.HasValue ? i.Value : -1);

                this.MENSAJE = (n4r.response == null ? "" : n4r.response.ToString());

                this.Parametros.Clear();
                this.Parametros.Add(nameof(XML), XML);
                this.Parametros.Add(nameof(MENSAJE), MENSAJE);
                this.Parametros.Add(nameof(USUARIO_REGISTRO), USUARIO_REGISTRO);

                var bcon2 = this.Accesorio.ObtenerConfiguracion("BILLION")?.valor;
                var result2 = BDOpe.ComandoInsertUpdateDeleteID(bcon2, "BTS_LOG_BL", this.Parametros);

                this.ID_GENERADO = result2.Resultado.HasValue ? result2.Resultado.Value : -1;
                if (!result2.Exitoso)
                {
                    this.LogEvent(USUARIO_REGISTRO, actualMetodo, string.Format("Falló la inserción {0}/{1}/{2}", NBR, LINE, result2.MensajeProblema));
                }

                return Respuesta.ResultadoOperacion<Int64>.CrearFalla(emsg);

            }


            this.MENSAJE = (n4r.response == null ? "" : n4r.response.ToString());

            this.Parametros.Clear();
            this.Parametros.Add(nameof(XML), XML);
            this.Parametros.Add(nameof(MENSAJE), MENSAJE);
            this.Parametros.Add(nameof(USUARIO_REGISTRO), USUARIO_REGISTRO);
           
            var bcon = this.Accesorio.ObtenerConfiguracion("BILLION")?.valor;
            var result =  BDOpe.ComandoInsertUpdateDeleteID(bcon, "BTS_LOG_BL", this.Parametros);

            this.ID_GENERADO = result.Resultado.HasValue ? result.Resultado.Value : -1;
            if (!result.Exitoso)
            {
                this.LogEvent(USUARIO_REGISTRO, actualMetodo, string.Format("Falló la inserción {0}/{1}/{2}", NBR, LINE, result.MensajeProblema));
                return Respuesta.ResultadoOperacion<Int64>.CrearFalla(result.MensajeProblema);
            }

            this.LogEvent(USUARIO_REGISTRO, actualMetodo, string.Format("Termina creación de log BL {0}/{1}", ID_GENERADO, NBR));
            return Respuesta.ResultadoOperacion<Int64>.CrearResultadoExitoso(result.Resultado.HasValue ? result.Resultado.Value : -1);
        }

       




    }
}
