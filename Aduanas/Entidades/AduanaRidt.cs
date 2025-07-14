using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml.Serialization;
using Configuraciones;
using AccesoDatos;
using Respuesta;
using System.Reflection;


namespace Aduanas.Entidades
{
    [Serializable]
    [XmlRoot(ElementName = "AduanaRidt")]
    public class AduanaRidt:ModuloBase
    {
        [XmlAttribute(AttributeName = "codigo_ridt")]
        public Int64 codigo_ridt { get; set; }

        [XmlAttribute(AttributeName = "mrn")]
        public string mrn { get; set; }

        [XmlAttribute(AttributeName = "msn")]
        public string msn { get; set; }

        [XmlAttribute(AttributeName = "hsn")]
        public string hsn { get; set; }

        [XmlAttribute(AttributeName = "id_importador")]
        public string id_importador { get; set; }


        [XmlAttribute(AttributeName = "nombre_importador")]
        public string nombre_importador { get; set; }

        [XmlAttribute(AttributeName = "id_agente")]
        public string id_agente { get; set; }


        [XmlAttribute(AttributeName = "nombre_agente")]
        public string nombre_agente { get; set; }


        [XmlAttribute(AttributeName = "status_code")]
        public string status_code { get; set; }


        [XmlAttribute(AttributeName = "usuario_registra")]
        public string usuario_registra { get; set; }

        [XmlAttribute(AttributeName = "numero_declaracion")]
        public string numero_declaracion { get; set; }

        [XmlAttribute(AttributeName = "comentarios")]
        public string comentarios { get; set; }

        [XmlAttribute(AttributeName = "fecha_registro")]
        public DateTime? fecha_registro { get; set; }

        public override void OnInstanceCreate()
        {
            this.alterClase = "ADUANA";
            base.OnInstanceCreate();
            this.Accesorio.ConfiguracionBase = "DATACON";
        }

         public Respuesta.ResultadoOperacion<Int64> NuevoRegistro()
        {
            this.actualMetodo = MethodBase.GetCurrentMethod().Name;
            //inicializa
            string pv = string.Empty;
            if (!this.Accesorio.Inicializar(out pv))
            {
                return Respuesta.ResultadoOperacion<Int64>.CrearFalla(pv);
            }
            var tt = SetMessage("NO_NULO", actualMetodo, usuario_registra);
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
             if (string.IsNullOrEmpty(this.id_importador))
            {
                return Respuesta.ResultadoOperacion<Int64>.CrearFalla(string.Format(tt.Item1, nameof(id_importador)));
            }
            if (string.IsNullOrEmpty(this.usuario_registra))
            {
                return Respuesta.ResultadoOperacion<Int64>.CrearFalla(string.Format(tt.Item1, nameof(usuario_registra)));
            }
            if (string.IsNullOrEmpty(this.comentarios))
            {
                return Respuesta.ResultadoOperacion<Int64>.CrearFalla(string.Format(tt.Item1, nameof(comentarios)));
            }
            if (string.IsNullOrEmpty(this.numero_declaracion))
            {
                return Respuesta.ResultadoOperacion<Int64>.CrearFalla(string.Format(tt.Item1, nameof(numero_declaracion)));
            }
            

            this.Parametros.Clear();
            this.Parametros.Add(nameof(mrn),mrn);
            this.Parametros.Add(nameof(msn), msn);
            this.Parametros.Add(nameof(hsn), hsn);
            this.Parametros.Add(nameof(usuario_registra),usuario_registra);
            this.Parametros.Add(nameof(id_importador), id_importador);
            this.Parametros.Add(nameof(nombre_importador), nombre_importador);
            this.Parametros.Add(nameof(comentarios), comentarios);
            this.Parametros.Add(nameof(id_agente), id_agente);
            this.Parametros.Add(nameof(numero_declaracion), numero_declaracion);
            

            var bcon = this.Accesorio.ObtenerConfiguracion("ECUAPASS")?.valor;
            //YA_ASIGNADO
#if DEBUG
            this.LogEvent(usuario_registra,this.actualMetodo,"Traza");
#endif
            var result =  BDOpe.ComandoInsertUpdateDeleteID(bcon, "[Bill].[nuevo_ridt_manual]", this.Parametros);
            /*bill.upsert_pago_asignado*/
            if (!result.Exitoso)
            {
                return Respuesta.ResultadoOperacion<Int64>.CrearFalla(result.MensajeProblema);
            }
            this.codigo_ridt = result.Resultado.HasValue ? result.Resultado.Value : -1;
            return Respuesta.ResultadoOperacion<Int64>.CrearResultadoExitoso(result.Resultado.HasValue?result.Resultado.Value:-1);
        }


        public static Respuesta.ResultadoOperacion<AduanaRidt> ObtenerRIDT(string mrn, string msn, string hsn, string usuario)
        {

            var p = new AduanaRidt();
            p.actualMetodo = MethodBase.GetCurrentMethod().Name;
            string pv;
            if (!p.Accesorio.Inicializar(out pv))
            {
                return ResultadoOperacion<AduanaRidt>.CrearFalla(pv);
            }
            p.Parametros.Clear();
            p.Parametros.Add("mrn", mrn);
            p.Parametros.Add("msn", msn);
            p.Parametros.Add("hsn", hsn);
            var bcon = p.Accesorio.ObtenerConfiguracion("ECUAPASS")?.valor;

#if DEBUG
            p.LogEvent(usuario, p.actualMetodo, bcon);
#endif
            var rp = BDOpe.ComandoSelectAEntidad<AduanaRidt>(bcon, "[Bill].[ridt_informacion]", p.Parametros);
            return rp.Exitoso ? ResultadoOperacion<AduanaRidt>.CrearResultadoExitoso(rp.Resultado) : ResultadoOperacion<AduanaRidt>.CrearFalla(rp.MensajeProblema, rp.MensajeInformacion);


        }


        public Respuesta.ResultadoOperacion<Int32> ActualizarRegistro()
        {
            this.actualMetodo = MethodBase.GetCurrentMethod().Name;
            //inicializa
            string pv = string.Empty;
            if (!this.Accesorio.Inicializar(out pv))
            {
                return Respuesta.ResultadoOperacion<Int32>.CrearFalla(pv);
            }
            var tt = SetMessage("NO_NULO", actualMetodo, usuario_registra);



            //DEBE LLENAR AL MENOS UNO DE 2
            if (string.IsNullOrEmpty(this.id_importador) && string.IsNullOrEmpty( id_agente))
            {
                return Respuesta.ResultadoOperacion<Int32>.CrearFalla(string.Format(tt.Item1, "ID_AGENTE O ID_IMPORTADOR"));
            }
            if (string.IsNullOrEmpty(this.nombre_importador) && string.IsNullOrEmpty(this.nombre_agente))
            {
                return Respuesta.ResultadoOperacion<Int32>.CrearFalla(string.Format(tt.Item1, "NOMBRE_AGENTE O NOMBRE_IMPORTADOR"));
            }


            //EL USUARIO
            if (string.IsNullOrEmpty(this.usuario_registra))
            {
                return Respuesta.ResultadoOperacion<Int32>.CrearFalla("Agregue el campo usuario_registra, para registrar modificación");
            }

            this.comentarios = string.Format("Mod.por {0}, el {1}",usuario_registra, DateTime.Now.ToString("dd/MM/yyyy HH:mm"));
            if (string.IsNullOrEmpty(this.comentarios))
            {
                return Respuesta.ResultadoOperacion<Int32>.CrearFalla(string.Format(tt.Item1, nameof(comentarios)));
            }
            // EL CODIGO DE RIDT
            if (string.IsNullOrEmpty(this.numero_declaracion))
            {
                return Respuesta.ResultadoOperacion<Int32>.CrearFalla(string.Format(tt.Item1, nameof(numero_declaracion)));
            }


            this.Parametros.Clear();
            this.Parametros.Add(nameof(mrn), mrn);
            this.Parametros.Add(nameof(msn), msn);
            this.Parametros.Add(nameof(hsn), hsn);
            this.Parametros.Add(nameof(id_agente), id_agente);
            this.Parametros.Add(nameof(nombre_agente), nombre_agente);
            this.Parametros.Add(nameof(id_importador), id_importador);
            this.Parametros.Add(nameof(nombre_importador), nombre_importador);
            this.Parametros.Add(nameof(comentarios), comentarios);
            this.Parametros.Add(nameof(numero_declaracion), numero_declaracion);

            var bcon = this.Accesorio.ObtenerConfiguracion("ECUAPASS")?.valor;
            //YA_ASIGNADO
#if DEBUG
            this.LogEvent(usuario_registra, this.actualMetodo, "Traza");
#endif
            var result = BDOpe.ComandoInsertUpdateDeleteFila(bcon, "[Bill].[actualizar_ridt]", this.Parametros);
            /*bill.upsert_pago_asignado*/
            if (!result.Exitoso)
            {
                return Respuesta.ResultadoOperacion<Int32>.CrearFalla(result.MensajeProblema);
            }
            return Respuesta.ResultadoOperacion<Int32>.CrearResultadoExitoso(result.Resultado);
        }


    }
}
