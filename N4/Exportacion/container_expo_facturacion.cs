using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Configuraciones;
using AccesoDatos;
using Respuesta;
using System.Reflection;

namespace N4.Exportacion
{
  public   class container_expo_facturacion:ModuloBase
    {
        public override void OnInstanceCreate()
        {
            this.alterClase = "N4";
            base.OnInstanceCreate();
            this.Accesorio.ConfiguracionBase = "DATACON";
        }

        public Int64 id { get; set; }
        public Int64? cab_id { get; set; }
        public string booking { get; set; }
        public string referencia { get; set; }
        public int? cantidad_unidades { get; set; }
        public string id_suceso { get; set; }
        public string mensaje_suceso { get; set; }
        public int? resultado_suceso { get; set; }
        public Int64? id_traza { get; set; }
        public DateTime? fecha_registro { get; set; }
        public string usuario_registra { get; set; }
        public bool? estado_registro { get; set; }



        public ResultadoOperacion<Int64> NuevoLog()
        {
            this.actualMetodo = MethodBase.GetCurrentMethod().Name;
            string pv = string.Empty;
            if (!this.Accesorio.Inicializar(out pv))
            {
                return Respuesta.ResultadoOperacion<Int64>.CrearFalla(pv);
            }
            var tt = SetMessage("NO_NULO", actualMetodo, usuario_registra);
            if (string.IsNullOrEmpty(usuario_registra))
            {
                return Respuesta.ResultadoOperacion<Int64>.CrearFalla(string.Format(tt.Item1, nameof(usuario_registra)));
            }

            if (!this.cab_id.HasValue || this.cab_id<=0)
            {
                return Respuesta.ResultadoOperacion<Int64>.CrearFalla(string.Format(tt.Item1, nameof(cab_id)));
            }
            this.Parametros.Clear();
            this.Parametros.Add(nameof(usuario_registra), usuario_registra);
            this.Parametros.Add(nameof(cab_id), cab_id);
            this.Parametros.Add(nameof(booking), booking);
            this.Parametros.Add(nameof(referencia), referencia);
            this.Parametros.Add(nameof(cantidad_unidades), cantidad_unidades);
            this.Parametros.Add(nameof(id_suceso), id_suceso);
            this.Parametros.Add(nameof(mensaje_suceso), mensaje_suceso);
            this.Parametros.Add(nameof(resultado_suceso), resultado_suceso);
            this.Parametros.Add(nameof(id_traza), id_traza);
      
            var bcon = this.Accesorio.ObtenerConfiguracion("Billion")?.valor;
            //YA_ASIGNADO   
#if DEBUG
            this.LogEvent(usuario_registra, this.actualMetodo, "Traza");
#endif
            var result = BDOpe.ComandoInsertUpdateDeleteID(bcon, "[Bill].[nuevo_container_expo_cab_facturacion]", this.Parametros);
            /*bill.upsert_pago_asignado*/
            if (!result.Exitoso)
            {
                return Respuesta.ResultadoOperacion<Int64>.CrearFalla(result.MensajeProblema);

            }
            this.id = result.Resultado.HasValue ? result.Resultado.Value : -1;
            return Respuesta.ResultadoOperacion<Int64>.CrearResultadoExitoso(result.Resultado.HasValue ? result.Resultado.Value : -1);
        }

        public static ResultadoOperacion<List<container_expo_facturacion>> ListaCasos(DateTime desde, DateTime hasta, string usuario)
        {
            var p = new container_expo_facturacion();
            p.actualMetodo = MethodBase.GetCurrentMethod().Name;
            string pv;
            if (!p.Accesorio.Inicializar(out pv))
            {
                return null;
            }
            var tt = p.SetMessage("NO_NULO", p.actualMetodo, usuario);
            p.Parametros.Clear();
            if (string.IsNullOrEmpty(usuario))
            {
                return Respuesta.ResultadoOperacion<List<container_expo_facturacion>>.CrearFalla(string.Format(tt.Item1, nameof(usuario)), tt.Item2);
            }

            p.Parametros.Add(nameof(desde), desde);
            p.Parametros.Add(nameof(hasta), hasta);
      

            var bcon = p.Accesorio.ObtenerConfiguracion("Billion")?.valor;
            var result = BDOpe.ComandoSelectALista<container_expo_facturacion>(bcon, "[Bill].[listar_expo_cab_facturacion]", p.Parametros);
            if (!result.Exitoso)
            {
                p.LogEvent(usuario, p.actualMetodo, result.MensajeProblema);
                return Respuesta.ResultadoOperacion<List<container_expo_facturacion>>.CrearFalla(result.MensajeProblema, result.MensajeInformacion);
            }
            return Respuesta.ResultadoOperacion<List<container_expo_facturacion>>.CrearResultadoExitoso(result.Resultado, result.MensajeInformacion);
        }

        public static ResultadoOperacion<List<container_expo_facturacion>> ListaCasos(Int64 cab_id,string usuario)
        {
            var p = new container_expo_facturacion();
            p.actualMetodo = MethodBase.GetCurrentMethod().Name;
            string pv;
            if (!p.Accesorio.Inicializar(out pv))
            {
                return null;
            }
            var tt = p.SetMessage("NO_NULO", p.actualMetodo, usuario);
            p.Parametros.Clear();
            if (string.IsNullOrEmpty(usuario))
            {
                return Respuesta.ResultadoOperacion<List<container_expo_facturacion>>.CrearFalla(string.Format(tt.Item1, nameof(usuario)), tt.Item2);
            }

            p.Parametros.Add(nameof(cab_id), cab_id);
      


            var bcon = p.Accesorio.ObtenerConfiguracion("Billion")?.valor;
            var result = BDOpe.ComandoSelectALista<container_expo_facturacion>(bcon, "[Bill].[listar_expo_cab_facturacion_id]", p.Parametros);
            if (!result.Exitoso)
            {
                p.LogEvent(usuario, p.actualMetodo, result.MensajeProblema);
                return Respuesta.ResultadoOperacion<List<container_expo_facturacion>>.CrearFalla(result.MensajeProblema, result.MensajeInformacion);
            }
            return Respuesta.ResultadoOperacion<List<container_expo_facturacion>>.CrearResultadoExitoso(result.Resultado, result.MensajeInformacion);
        }

    }
}
