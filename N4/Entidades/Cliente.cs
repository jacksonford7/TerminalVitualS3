using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Configuraciones;
using Respuesta;
using AccesoDatos;
using System.Reflection;

namespace N4.Entidades
{
    [Serializable]
    [XmlRoot(ElementName = "Agente")]
    public class Cliente : ModuloBase
    {
       
        [XmlAttribute(AttributeName = "CLNT_CUSTOMER")]
        public string CLNT_CUSTOMER { get; set; }

        [XmlAttribute(AttributeName = "CLNT_NAME")]
        public string CLNT_NAME { get; set; }


        [XmlAttribute(AttributeName = "CLNT_ADRESS")]
        public string CLNT_ADRESS { get; set; }

        [XmlAttribute(AttributeName = "CLNT_EMAIL")]
        public string CLNT_EMAIL { get; set; }

        [XmlAttribute(AttributeName = "CLNT_TYPE")]
        public string CLNT_TYPE { get; set; }

        [XmlAttribute(AttributeName = "CLNT_EBILLING")]
        public string CLNT_EBILLING { get; set; }

        [XmlAttribute(AttributeName = "CLNT_FAX_INVC")]
        public string CLNT_FAX_INVC { get; set; }

        [XmlAttribute(AttributeName = "CLNT_RFC")]
        public string CLNT_RFC { get; set; }

        [XmlAttribute(AttributeName = "CLNT_ACTIVE")]
        public string CLNT_ACTIVE { get; set; }

        [XmlAttribute(AttributeName = "CLNT_ROLE")]
        public string CLNT_ROLE { get; set; }

        [XmlAttribute(AttributeName = "CODIGO_SAP")]
        public string CODIGO_SAP { get; set; }

        [XmlIgnore]
        public Int64? DIAS_CREDITO { get; set; }

        [XmlAttribute(AttributeName = "CLNT_CITY")]
        public string CLNT_CITY { get; set; }


        public Cliente():base()
        {


        }
        public override void OnInstanceCreate()
        {
            this.alterClase = "N4";
            base.OnInstanceCreate();
            this.Accesorio.ConfiguracionBase = "DATACON";
        }
       

        public static ResultadoOperacion<Cliente> ObtenerCliente(string usuario, string codigo)
        {
             var p = new Cliente();
            p.actualMetodo = MethodBase.GetCurrentMethod().Name;
            string pv;
            if (!p.Accesorio.Inicializar(out pv))
            {
               return ResultadoOperacion<Cliente>.CrearFalla(pv);
            }
            p.Parametros.Clear();
            var bcon = p.Accesorio.ObtenerConfiguracion("N4BILL")?.valor;
            p.Parametros.Add("id", codigo);

#if DEBUG
            p.LogEvent(usuario, p.actualMetodo, codigo);
#endif

            var rp = BDOpe.ComandoSelectAEntidad<Cliente>(bcon, "[Bill].[cliente_informacion]", p.Parametros);
            //if (rp.Exitoso)
            //{
            //    p.Parametros.Clear();
            //    p.Parametros.Add("id", codigo);
            //    bcon = p.Accesorio.ObtenerConfiguracion("N4BILL")?.valor;
            //    var sc = BDOpe.ComandoSelectEscalar<Int64>(bcon, "Select [Bill].[customer_credit_info](@id)", p.Parametros);
            //    rp.Resultado.DIAS_CREDITO = sc.Resultado;
            //}
            return rp.Exitoso ? ResultadoOperacion<Cliente>.CrearResultadoExitoso(rp.Resultado) : ResultadoOperacion<Cliente>.CrearFalla(rp.MensajeProblema);
        }


        public static ResultadoOperacion<Cliente> ObtenerClienteSAV(string usuario, string codigo)
        {
            var p = new Cliente();
            p.actualMetodo = MethodBase.GetCurrentMethod().Name;
            string pv;
            if (!p.Accesorio.Inicializar(out pv))
            {
                return ResultadoOperacion<Cliente>.CrearFalla(pv);
            }
            p.Parametros.Clear();
            var bcon = p.Accesorio.ObtenerConfiguracion("N4BILL")?.valor;
            p.Parametros.Add("id", codigo);

#if DEBUG
            p.LogEvent(usuario, p.actualMetodo, codigo);
#endif

            var rp = BDOpe.ComandoSelectAEntidad<Cliente>(bcon, "[Bill].[cliente_informacion_sav]", p.Parametros);
            //if (rp.Exitoso)
            //{
            //    p.Parametros.Clear();
            //    p.Parametros.Add("id", codigo);
            //    bcon = p.Accesorio.ObtenerConfiguracion("N4BILL")?.valor;
            //    var sc = BDOpe.ComandoSelectEscalar<Int64>(bcon, "Select [Bill].[customer_credit_info](@id)", p.Parametros);
            //    rp.Resultado.DIAS_CREDITO = sc.Resultado;
            //}
            return rp.Exitoso ? ResultadoOperacion<Cliente>.CrearResultadoExitoso(rp.Resultado) : ResultadoOperacion<Cliente>.CrearFalla(rp.MensajeProblema);
        }

        public static ResultadoOperacion<Cliente> ObtenerClienteZAL(string usuario, string codigo)
        {
            var p = new Cliente();
            p.actualMetodo = MethodBase.GetCurrentMethod().Name;
            string pv;
            if (!p.Accesorio.Inicializar(out pv))
            {
                return ResultadoOperacion<Cliente>.CrearFalla(pv);
            }
            p.Parametros.Clear();
            var bcon = p.Accesorio.ObtenerConfiguracion("N4BILL")?.valor;
            p.Parametros.Add("id", codigo);

#if DEBUG
            p.LogEvent(usuario, p.actualMetodo, codigo);
#endif

            var rp = BDOpe.ComandoSelectAEntidad<Cliente>(bcon, "[Bill].[cliente_informacion_zal]", p.Parametros);
            //if (rp.Exitoso)
            //{
            //    p.Parametros.Clear();
            //    p.Parametros.Add("id", codigo);
            //    bcon = p.Accesorio.ObtenerConfiguracion("N4BILL")?.valor;
            //    var sc = BDOpe.ComandoSelectEscalar<Int64>(bcon, "Select [Bill].[customer_credit_info](@id)", p.Parametros);
            //    rp.Resultado.DIAS_CREDITO = sc.Resultado;
            //}
            return rp.Exitoso ? ResultadoOperacion<Cliente>.CrearResultadoExitoso(rp.Resultado) : ResultadoOperacion<Cliente>.CrearFalla(rp.MensajeProblema);
        }


        public static ResultadoOperacion<List<Cliente>> ObtenerClientes(List<string> ruc, string usuario)
        {
            var p = new Cliente();
            p.actualMetodo = MethodBase.GetCurrentMethod().Name;
            string pv;
            if (!p.Accesorio.Inicializar(out pv))
            {
                return ResultadoOperacion<List<Cliente>>.CrearFalla(pv);
            }
            p.Parametros.Clear();
            var bcon = p.Accesorio.ObtenerConfiguracion("N4BILL")?.valor;

            pv = "Lista de ruc es nulo o vacio";
            if (ruc == null || ruc.Count <= 0)
            {
                return ResultadoOperacion<List<Cliente>>.CrearFalla(pv);
            }
            StringBuilder bo = new StringBuilder();
            bo.Append("<clientes>");
            foreach (var r in ruc)
            {
                bo.AppendFormat("<cliente ruc=\"{0}\" />",r);
            }
            bo.Append("</clientes>");
            p.Parametros.Add("clientes", bo.ToString());

#if DEBUG
            p.LogEvent(usuario, p.actualMetodo, bo.ToString());
#endif
            //tengo la lista de clientes desde N4
            var rp = BDOpe.ComandoSelectALista<Cliente>(bcon, "[Bill].[cliente_informacion_ruc]", p.Parametros);
            //if (rp.Exitoso && rp.Resultado.Count > 0)
            //{
            //    var lisc = rp.Resultado;
            //    bcon = p.Accesorio.ObtenerConfiguracion("N4BILL")?.valor;
            //    var sc = BDOpe.ComandoSelectALista<ItemRetornable>(bcon, "[Bill].[customer_credit_info_ruc]", p.Parametros);
            //    if (sc.Exitoso && sc.Resultado.Count> 0)
            //    {
            //        foreach (var cl in lisc)
            //        {
            //            var v = sc.Resultado?.Where(t => t.id.Equals(cl.CLNT_CUSTOMER)).FirstOrDefault()?.valor1;
            //            if (!string.IsNullOrEmpty(v))
            //            {
            //                long g;
            //                if (long.TryParse(v, out g))
            //                {
            //                    cl.DIAS_CREDITO = g;
            //                }

            //            }
            //        }
            //    }
                
            //}
            return rp.Exitoso ? ResultadoOperacion<List<Cliente>>.CrearResultadoExitoso(rp.Resultado) : ResultadoOperacion<List<Cliente>>.CrearFalla(rp.MensajeProblema);
        }

        public static ResultadoOperacion<List<Cliente>> ObtenerClientes(HashSet<string> ruc, string usuario)
        {
            var p = new Cliente();
            p.actualMetodo = MethodBase.GetCurrentMethod().Name;
            string pv;
            if (!p.Accesorio.Inicializar(out pv))
            {
                return ResultadoOperacion<List<Cliente>>.CrearFalla(pv);
            }
            p.Parametros.Clear();
            var bcon = p.Accesorio.ObtenerConfiguracion("N4BILL")?.valor;

            pv = "Lista de ruc es nulo o vacio";
            if (ruc == null || ruc.Count <= 0)
            {
                return ResultadoOperacion<List<Cliente>>.CrearFalla(pv);
            }
            StringBuilder bo = new StringBuilder();
            bo.Append("<clientes>");
            foreach (var r in ruc)
            {
                bo.AppendFormat("<cliente ruc=\"{0}\" />", r);
            }
            bo.Append("</clientes>");
            p.Parametros.Add("clientes", bo.ToString());

#if DEBUG
            p.LogEvent(usuario, p.actualMetodo, bo.ToString());
#endif
            //tengo la lista de clientes desde N4
            var rp = BDOpe.ComandoSelectALista<Cliente>(bcon, "[Bill].[cliente_informacion_ruc]", p.Parametros);
           
            return rp.Exitoso ? ResultadoOperacion<List<Cliente>>.CrearResultadoExitoso(rp.Resultado) : ResultadoOperacion<List<Cliente>>.CrearFalla(rp.MensajeProblema);
        }

        public static  ResultadoOperacion<System.Data.DataTable> ClientesPagoFactura(string referencia, string USUARIO)
        {
            var p = new Cliente();
            p.actualMetodo = MethodBase.GetCurrentMethod().Name;
            string pv;
            if (!p.Accesorio.Inicializar(out pv))
            {
                return ResultadoOperacion<System.Data.DataTable>.CrearFalla(pv);
            }
            var tt = p.SetMessage("NO_NULO", p.actualMetodo, USUARIO);
            if (string.IsNullOrEmpty(USUARIO))
            {
                return ResultadoOperacion<System.Data.DataTable>.CrearFalla(string.Format(tt.Item1, nameof(USUARIO)));
            }
             p.Parametros.Clear();
            p.Parametros.Add(nameof(referencia), referencia);
            var bcon = p.Accesorio.ObtenerConfiguracion("PORTAL")?.valor;
            //OBTENER LA LISTA DE PASES.
            var dtw = BDOpe.ComadoSelectADatatable(bcon, "[Bill].[clientes_pago_referencia]", p.Parametros);
            if (!dtw.Exitoso)
            {
                return ResultadoOperacion<System.Data.DataTable>.CrearFalla(dtw.MensajeProblema);
            }
            return ResultadoOperacion<System.Data.DataTable>.CrearResultadoExitoso(dtw.Resultado);

        }

        //Toda la lista para pantalla
        public static ResultadoOperacion<List<Cliente>> ObtenerClientes(string usuario, string pista)
        {
            var p = new Chofer();
            p.actualMetodo = MethodBase.GetCurrentMethod().Name;
            string pv;
            if (!p.Accesorio.Inicializar(out pv))
            {
                return ResultadoOperacion<List<Cliente>>.CrearFalla(pv);
            }
            p.Parametros.Clear();
            pista = pista?.Trim();
            p.Parametros.Add("pista", pista);
            var bcon = p.Accesorio.ObtenerConfiguracion("N4BILL")?.valor;
#if DEBUG
            p.LogEvent(usuario, p.actualMetodo, usuario);
#endif
            var rp = BDOpe.ComandoSelectALista<Cliente>(bcon, "[Bill].[clientes_lista]", p.Parametros);
            return rp.Exitoso ? ResultadoOperacion<List<Cliente>>.CrearResultadoExitoso(rp.Resultado) : ResultadoOperacion<List<Cliente>>.CrearFalla(rp.MensajeProblema);
        }

    }
}
