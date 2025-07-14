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
    public class Pase_Transportista : ModuloBase
    {
        public override void OnInstanceCreate()
        {
            this.alterClase = "PASEPTA";
            base.OnInstanceCreate();
            this.Accesorio.ConfiguracionBase = "DATACON";
        }
        public Pase_Transportista()
        {
            OnInstanceCreate();
            this.ESTADO = "GN";
            this.CANTIDAD_CARGA = 0;
            this.FECHA_REGISTRO = DateTime.Now;
            this.TIPO_CARGA = "CFS";
        }
        public decimal ID_PASE { get; set; } //secuencil
        public decimal? ID_CARGA { get; set; } //gkey
        public string ESTADO { get; set; } //nuevo->GN
        public DateTime? FECHA_EXPIRACION { get; set; } // *
        public string ID_PLACA { get; set; }
        public string ID_CHOFER { get; set; }
        public string CHOFER_DESC { get; set; }

        public string ID_EMPRESA { get; set; }
        public string CONSIGNATARIO_ID { get; set; }
        public string CONSIGNARIO_NOMBRE { get; set; }

        public string USUARIO_REGISTRO { get; set; }
        public DateTime? FECHA_REGISTRO { get; set; }
        public string NUMERO_PASE_N4 { get; set; }
        public string TRANSPORTISTA_DESC { get; set; }

        public int? CANTIDAD_CARGA { get; set; }

       
        public string USUARIO_ESTADO { get; set; }
        public DateTime? FECHA_ESTADO { get; set; }
        public Int64? ID_RESERVA { get; set; }
        public string TIPO_CARGA { get; set; }
      
        public string MOTIVO_CANCELACION { get; set; }
        public bool? RESERVA { get; set; }
        public bool? ESTADO_RESERVA { get; set; }
        public Int64? PPW { get; set; }


        public bool? SERVICIO { get; set; }
        public DateTime? FECHA_SERVICIO { get; set; }
        public string USUARIO_SERVICIO { get; set; }

        public Int64? ID_UNIDAD { get; set; } //gkey

        public string BODEGA { get; set; }
        public DateTime? FECHA_ULT_FACTURA { get; set; }
        public string RUC { get; set; }
        public string aisv { get; set; }

        public ResultadoOperacion<bool> Actualizar_Transportista(string USUARIO_MODIFICA)
        {
            //inicializar
            this.actualMetodo = MethodBase.GetCurrentMethod().Name;

            string pv = string.Empty;
            if (!this.Accesorio.Inicializar(out pv))
            {
                return Respuesta.ResultadoOperacion<bool>.CrearFalla(pv);
            }
            //Validaciones-->
            var tt = SetMessage("NO_NULO", actualMetodo, USUARIO_MODIFICA);
            //aca llamar a n4 y crear appointmen
            if (string.IsNullOrEmpty(USUARIO_MODIFICA))
            {
                return Respuesta.ResultadoOperacion<bool>.CrearFalla(string.Format(tt.Item1, nameof(USUARIO_MODIFICA)));
            }

            if (this.ID_PASE <= 0)
            {
                return Respuesta.ResultadoOperacion<bool>.CrearFalla(string.Format(tt.Item1, nameof(ID_PASE)));
            }

  

            //SI PASA LICENCIA ENTONCES NOMBRE OBLIGATORIO
            if (!string.IsNullOrEmpty(ID_CHOFER))
            {
                if (string.IsNullOrEmpty(CHOFER_DESC))
                    return Respuesta.ResultadoOperacion<bool>.CrearFalla(string.Format(tt.Item1, nameof(CHOFER_DESC)));
            }

            //AHORA SI GRABAR TURNO NUEVO.

            this.Parametros.Clear();
            this.Parametros.Add(nameof(ID_PASE), ID_PASE);
            this.Parametros.Add(nameof(ID_PLACA), ID_PLACA);
            this.Parametros.Add(nameof(ID_CHOFER), ID_CHOFER);
            this.Parametros.Add(nameof(CHOFER_DESC), CHOFER_DESC);
            this.Parametros.Add(nameof(ID_EMPRESA), ID_EMPRESA);
            this.Parametros.Add(nameof(USUARIO_MODIFICA), USUARIO_MODIFICA);

            var bcon = this.Accesorio.ObtenerConfiguracion("N4Middleware")?.valor;
            var result = BDOpe.ComandoInsertUpdateDeleteFila(bcon, "[Bill].[actualiza_transportistas_pase_impo]", this.Parametros);

            if (!result.Exitoso)
            {
                this.LogEvent(USUARIO_REGISTRO, actualMetodo, string.Format("Falló la ACTUALIZACION {0}/{1}/{2}", ID_PASE, ID_CARGA, result.MensajeProblema));
                return Respuesta.ResultadoOperacion<bool>.CrearFalla(result.MensajeProblema);
            }

            return Respuesta.ResultadoOperacion<bool>.CrearResultadoExitoso(true, "Actualizado con exito");

        }

        public ResultadoOperacion<bool> Actualizar_Transportista_Expo(string USUARIO_MODIFICA)
        {
            //inicializar
            this.actualMetodo = MethodBase.GetCurrentMethod().Name;

            string pv = string.Empty;
            if (!this.Accesorio.Inicializar(out pv))
            {
                return Respuesta.ResultadoOperacion<bool>.CrearFalla(pv);
            }
            //Validaciones-->
            var tt = SetMessage("NO_NULO", actualMetodo, USUARIO_MODIFICA);
            //aca llamar a n4 y crear appointmen
            if (string.IsNullOrEmpty(USUARIO_MODIFICA))
            {
                return Respuesta.ResultadoOperacion<bool>.CrearFalla(string.Format(tt.Item1, nameof(USUARIO_MODIFICA)));
            }

            if (string.IsNullOrEmpty(this.aisv))
            {
                return Respuesta.ResultadoOperacion<bool>.CrearFalla(string.Format(tt.Item1, nameof(aisv)));
            }



            //SI PASA LICENCIA ENTONCES NOMBRE OBLIGATORIO
            if (!string.IsNullOrEmpty(ID_CHOFER))
            {
                if (string.IsNullOrEmpty(CHOFER_DESC))
                    return Respuesta.ResultadoOperacion<bool>.CrearFalla(string.Format(tt.Item1, nameof(CHOFER_DESC)));
            }

            //AHORA SI GRABAR TURNO NUEVO.

            this.Parametros.Clear();
            this.Parametros.Add(nameof(aisv), aisv);
            this.Parametros.Add(nameof(ID_PLACA), ID_PLACA);
            this.Parametros.Add(nameof(ID_CHOFER), ID_CHOFER);
            this.Parametros.Add(nameof(CHOFER_DESC), CHOFER_DESC);
            this.Parametros.Add(nameof(ID_EMPRESA), ID_EMPRESA);
            this.Parametros.Add(nameof(USUARIO_MODIFICA), USUARIO_MODIFICA);

            var bcon = this.Accesorio.ObtenerConfiguracion("TRANSP_PORTAL")?.valor;
            var result = BDOpe.ComandoInsertUpdateDeleteFila(bcon, "[Bill].[actualiza_transportistas_pase_expo]", this.Parametros);

            if (!result.Exitoso)
            {
                this.LogEvent(USUARIO_REGISTRO, actualMetodo, string.Format("Falló la ACTUALIZACION {0}/{1}/{2}", ID_PASE, ID_CARGA, result.MensajeProblema));
                return Respuesta.ResultadoOperacion<bool>.CrearFalla(result.MensajeProblema);
            }

            return Respuesta.ResultadoOperacion<bool>.CrearResultadoExitoso(true, "Actualizado con exito");

        }

    }
}
