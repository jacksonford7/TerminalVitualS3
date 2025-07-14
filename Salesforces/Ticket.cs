using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AccesoDatos;
using Configuraciones;
using Respuesta;

namespace Salesforces
{
    public enum TipoCarga
    {
        Contenedores, General, CFS, BRBK
    }

    public enum TipoCategoria
    {
        Importacion, Exportacion, Trasbordo, Almacenamiento
    }
    public class Ticket:ModuloBase
    {
        public override void OnInstanceCreate()
        {
            this.alterClase = "SALESFORCES";
            base.OnInstanceCreate();
            this.Accesorio.ConfiguracionBase = "DATACON";
        }

        public string Ruc { get; set; }
  

        public string Asunto { get; set; }
        public string Contenido { get; set; }
        public string PalabraClave { get; set; }


        public string Usuario { get; set; }
        public string Email { get; set; }

        public string Copias { get; set; }
        public string Aplicacion { get; set; }
        public string Modulo { get; set; }
        public string Tipo { get; set; } //Error, Sugerencia, Queja, Problema, Otros
        public string Categoria { get; set; }//  Impo,Expo,Otros


        public DateTime? Registro { get; set; }

        public Int64 Id { get; set; }

        public ResultadoOperacion<Int64> NuevoCaso()
        {
            this.actualMetodo = MethodBase.GetCurrentMethod().Name;
            string pv = string.Empty;
            if (!this.Accesorio.Inicializar(out pv))
            {
                return Respuesta.ResultadoOperacion<Int64>.CrearFalla(pv);
            }
            var tt = SetMessage("NO_NULO", actualMetodo, Usuario);
            if (string.IsNullOrEmpty(Ruc))
            {
                return Respuesta.ResultadoOperacion<Int64>.CrearFalla(string.Format(tt.Item1, nameof(Ruc)));
            }

            if (string.IsNullOrEmpty(Categoria))
            {
                return Respuesta.ResultadoOperacion<Int64>.CrearFalla(string.Format(tt.Item1, nameof(Ruc)));
            }

            if (string.IsNullOrEmpty(Usuario))
            {
                return Respuesta.ResultadoOperacion<Int64>.CrearFalla(string.Format(tt.Item1, nameof(Usuario)));
            }
            if (string.IsNullOrEmpty(Aplicacion))
            {
                return Respuesta.ResultadoOperacion<Int64>.CrearFalla(string.Format(tt.Item1, nameof(Aplicacion)));
            }

            if (string.IsNullOrEmpty(Contenido))
            {
                return Respuesta.ResultadoOperacion<Int64>.CrearFalla(string.Format(tt.Item1, nameof(Contenido)));
            }

            if (string.IsNullOrEmpty(Tipo))
            {
                return Respuesta.ResultadoOperacion<Int64>.CrearFalla(string.Format(tt.Item1, nameof(Tipo)));
            }
            //aca grabar

      
            this.Parametros.Clear();
            this.Parametros.Add(nameof(Ruc),Ruc);
            this.Parametros.Add(nameof(Categoria), Categoria);
            this.Parametros.Add(nameof(Contenido), Contenido);
            this.Parametros.Add(nameof(PalabraClave), PalabraClave);
            this.Parametros.Add(nameof(Usuario), Usuario);
            this.Parametros.Add(nameof(Email), Email);
            this.Parametros.Add(nameof(Aplicacion), Aplicacion);
            this.Parametros.Add(nameof(Modulo), Modulo);
            this.Parametros.Add(nameof(Tipo), Tipo);
            this.Parametros.Add(nameof(Copias), Copias);
            var bcon = this.Accesorio.ObtenerConfiguracion("Billion")?.valor;
            //YA_ASIGNADO   
#if DEBUG
            this.LogEvent(Usuario, this.actualMetodo, "Traza");
#endif
            var result = BDOpe.ComandoInsertUpdateDeleteID(bcon, "[Bill].[nuevo_ticket]", this.Parametros);
            /*bill.upsert_pago_asignado*/
            if (!result.Exitoso)
            {
                return Respuesta.ResultadoOperacion<Int64>.CrearFalla(result.MensajeProblema);

            }
            this.Id = result.Resultado.HasValue ? result.Resultado.Value : -1;
            return Respuesta.ResultadoOperacion<Int64>.CrearResultadoExitoso(result.Resultado.HasValue ? result.Resultado.Value : -1);
        }

        public static ResultadoOperacion<List<Ticket>> ListaCasos(DateTime desde, DateTime hasta, string usuario, string ruc=null)
        {
            var p = new Ticket();
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
                return Respuesta.ResultadoOperacion<List<Ticket>>.CrearFalla(string.Format(tt.Item1, nameof(usuario)), tt.Item2);
            }

            p.Parametros.Add("usuario", usuario);
            p.Parametros.Add(nameof(desde),desde);
            p.Parametros.Add(nameof(hasta), hasta);
            if (!string.IsNullOrEmpty(ruc))
            {
                p.Parametros.Add(nameof(ruc), ruc);
            }

            var bcon = p.Accesorio.ObtenerConfiguracion("Billion")?.valor;
            var result = BDOpe.ComandoSelectALista<Ticket>(bcon, "[Bill].[listar_tickets]", p.Parametros);
            if (!result.Exitoso)
            {
                p.LogEvent(usuario, p.actualMetodo, result.MensajeProblema);
                return Respuesta.ResultadoOperacion<List<Ticket>>.CrearFalla(result.MensajeProblema, result.MensajeInformacion);
            }
            return Respuesta.ResultadoOperacion<List<Ticket>>.CrearResultadoExitoso(result.Resultado, result.MensajeInformacion);
        }
    }

    public class SaleforcesContenido
    {

        public TipoCarga? Tipo { get; set; }
        public TipoCategoria? Categoria { get; set; }
        public string Titulo { get; set; }
        public string Novedad { get; set; }
        public List<DetalleCarga> Detalles { get; set; }
        public SaleforcesContenido()
        {
            this.Detalles = new List<DetalleCarga>();
        }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<!DOCTYPE html><html><head> <meta http-equiv='Content-Type' content='text/html; charset=utf-8'/>  <style>table#t01 { width: 100%;  border:1px solid #ccc; cellspacing:1; cellpadding:1; }  table#t01 tr:nth-child(even) { background-color: #eee;  }    table#t01 td { padding:2px; border:1px solid #ccc;  }    .fuente {font-family: Verdana, Arial, Helvetica, sans-serif;  font-size: 9px;  }  .fuente2 {font-size: 6px; font-family: Verdana, Arial, Helvetica, sans-serif;}  </style> </head> <body>");
            if (!string.IsNullOrEmpty(this.Titulo))
            {
                sb.AppendFormat("<h2>{0}</h2>", Titulo);
            }
            if (this.Tipo.HasValue)
            {
                sb.AppendFormat("<p class='lbl' ><strong>Tipo de Carga:</strong> {0}</p>", Tipo);
            }
            if (this.Categoria.HasValue)
            {
                sb.AppendFormat("<p class='lbl' ><strong> Categoría: </strong>{0}</p>", Categoria);
            }
            if (!string.IsNullOrEmpty(this.Novedad))
            {
                sb.AppendFormat("<p><strong> Situación:</strong> <br/>{0}</p>", Novedad);
            }
            if (this.Detalles != null && Detalles.Count > 0)
            {
                sb.Append("<h3><strong>Detalles del ítem de carga</strong></h3>");
                sb.Append("<table id='t01' border='1' cellspacing='1'  > ");
                foreach (var i in this.Detalles)
                {
                    sb.AppendFormat("<tr><td>{0}</td><td>{1}</td></tr>",i.Etiqueta,i.Valor as string);
                }
                sb.Append("</table>");
            }
            sb.Append("</body></html>");
            return sb.ToString();
        }
    }

    public class DetalleCarga
    {
        public string Etiqueta { get; set; }
        public object Valor { get; set; }

        public DetalleCarga()
        {

        }

        public DetalleCarga( string _etiqueta , object _valor)
        {
            this.Etiqueta = _etiqueta;
            this.Valor = _valor;
        }   

    }
}
