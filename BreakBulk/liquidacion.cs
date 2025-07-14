using BillionEntidades;
using SqlConexion;
using System;
using System.Collections.Generic;
using N4Ws;
using N4Ws.Entidad;
using Configuraciones;
using System.Reflection;
using System.Data;
using System.Data.SqlClient;
using BreakBulk;

namespace BreakBulk
{
    public class liquidacion : Cls_Bil_Base
    {
        #region "Propiedades"
        public long? idliquidacion { get; set; }
        public long idTarjaDet { get; set; }
        public tarjaDet TarjaDet { get; set; }
        public string mrn { get; set; }
        public string msn { get; set; }
        public string hsn { get; set; }
        public decimal? cantidad { get; set; }
        public decimal? peso { get; set; }
        public decimal? cubicaje { get; set; }
        public string idConsignatario { get; set; }
        public string ubicacion { get; set; }
        public int idServicio { get; set; }
        public servicios Servicio { get; set; }
        public string comentario { get; set; }
        public string estado { get; set; }
        public bool sobredimensionado { get; set; }
        public estados Estados { get; set; }
        public string usuarioCrea { get; set; }
        public DateTime? fechaCreacion { get; set; }
        public string usuarioModifica { get; set; }
        public DateTime? fechaModifica { get; set; }
        public Int64 BRBK_CONSECUTIVO { get; set; }
        #endregion


        public liquidacion() : base()
        {
            init();
        }
        private static void OnInit(string Base)
        {
            sql_puntero = (sql_puntero == null) ? Cls_Conexion.Conexion() : sql_puntero;
            parametros = new Dictionary<string, object>();
            nueva_conexion = Cls_Conexion.Nueva_Conexion(Base);
        }

        public static List<liquidacion> listadoLiquidacion(long _idTarjaDet, out string OnError)
        {
            OnInit("N4Middleware");
            parametros.Clear();
            parametros.Add("i_idTarjaDet", _idTarjaDet);
            return sql_puntero.ExecuteSelectControl<liquidacion>(nueva_conexion, 4000, "[brbk].consultarLiquidacion", parametros, out OnError);
        }

        public static liquidacion GetLiquidacion(long _id)
        {
            OnInit("N4Middleware");
            parametros.Clear();
            parametros.Add("i_idliquidacion", _id);
            var obj = sql_puntero.ExecuteSelectOnly<liquidacion>(nueva_conexion, 4000, "[brbk].consultarLiquidacion", parametros);
            try
            {
                obj.Servicio = servicios.GetServicio(obj.idServicio);
                obj.TarjaDet = tarjaDet.GetTarjaDet(obj.idTarjaDet);
                obj.Estados = estados.GetEstado(obj.estado);
            }
            catch { }
            return obj;
        }


        public static Int64 consultaGKeyCarga(string numeroCarga)
        {
            OnInit("N5");
            parametros.Clear();
            parametros.Add("MRN_MSN_HSN ", numeroCarga);
            var obj = sql_puntero.ExecuteSelectOnly<liquidacion>(nueva_conexion, 4000, "[brbk].BRBK_BULK_IMPO_GKEY", parametros);

            return obj.BRBK_CONSECUTIVO;
        }

        public Int64? Save_Update(long gkey, servicios oServicio, out string OnError)
        {
            //carga el servicio a facturar en N4
            var resp = ServicioBRBK.CargarServicioAdicional(gkey, this.usuarioCrea, oServicio);
            if (resp.Exitoso)
            {
                OnInit("N4Middleware");
                parametros.Clear();

                parametros.Add("i_idliquidacion", this.idliquidacion);
                parametros.Add("i_idTarjaDet", this.TarjaDet.idTarjaDet);
                parametros.Add("i_mrn", this.TarjaDet.mrn);
                parametros.Add("i_msn", this.TarjaDet.msn);
                parametros.Add("i_hsn", this.TarjaDet.hsn);
                parametros.Add("i_cantidad", this.TarjaDet.cantidad);
                parametros.Add("i_peso", this.TarjaDet.kilos);
                parametros.Add("i_cubicaje", this.TarjaDet.cubicaje);
                parametros.Add("i_idConsignatario", this.TarjaDet.idConsignatario);
                parametros.Add("i_ubicacion", this.ubicacion);
                parametros.Add("i_idServicio", this.idServicio);
                parametros.Add("i_comentario", this.comentario);
                parametros.Add("i_estado", this.estado);
                parametros.Add("i_sobredimensionado", this.sobredimensionado);
                parametros.Add("i_usuarioCrea", this.usuarioCrea);
                parametros.Add("i_usuarioModifica", this.usuarioModifica);

                var db = sql_puntero.ExecuteInsertUpdateDeleteReturn(nueva_conexion, 6000, "[brbk].insertarLiquidacion", parametros, out OnError);
                if (!db.HasValue || db.Value < 0)
                {
                    return null;
                }
                OnError = string.Empty;
                return db.Value;
            }
            else
            {
                OnError = "Error al crear evento en N4:" + resp.MensajeProblema;
            }
            return -1;
        }
    }

    public class ServicioBRBK : ModuloBase
    {
        public override void OnInstanceCreate()
        {
            this.alterClase = "N4_SERVICE";
            base.OnInstanceCreate();
            this.Accesorio.ConfiguracionBase = "DATACON";
        }
        //inicializa de la instancia de servicio
        private static Servicios InicializaServicio(out string erno)
        {
            var p = new Servicios();
            if (!p.Accesorio.Inicializar(out erno))
            {
                return null;
            }
            return p;
        }
        //inicializa una las configuraciones de N4
        //private static N4Configuration ObtenerInicializador(Servicios ser, out string novedad)
        //{
        //    if (ser == null)
        //    {
        //        novedad = "Objeto inicializador es nulo";
        //        return null;
        //    }
        //    if (!ser.Accesorio.ExistenConfiguraciones)
        //    {
        //        novedad = "No existen configuraciones de inicialización";
        //        return null;
        //    }
        //    var ur = ser.Accesorio.ObtenerConfiguracion("URL")?.valor;
        //    var us = ser.Accesorio.ObtenerConfiguracion("USUARIO")?.valor;
        //    var pas = ser.Accesorio.ObtenerConfiguracion("PASSWORD")?.valor;
        //    var sc = ser.Accesorio.ObtenerConfiguracion("SCOPE")?.valor;
        //    novedad = string.Empty;
        //    return N4Configuration.GetInstance(us, pas, ur, sc);

        //}


        public static Respuesta.ResultadoOperacion<bool> CargarServicioAdicional(Int64 id_unidades, string usuario, servicios oServicio)
        {

            string pv;
            var p = InicializaServicio(out pv);
            p.actualMetodo = MethodBase.GetCurrentMethod().Name;
            if (p == null)
            {
                p.LogError<ApplicationException>(new ApplicationException("No fue posible inicializar objeto servicios"), p.actualMetodo, usuario);
                //trace log not init
                return Respuesta.ResultadoOperacion<bool>.CrearFalla("La inicialización del objeto N4 falló");
            }

            if (!p.Accesorio.ExistenConfiguraciones)
            {
                return Respuesta.ResultadoOperacion<bool>.CrearFalla("No existen configuraciones");
            }

            GroovyCodeExtension code = new GroovyCodeExtension();
            code.name = "CGSAUnitEventBBKWS";
            code.location = "code-extension";
            code.parameters.Add("UNIT", id_unidades.ToString());
            code.parameters.Add("USER", usuario);
            code.parameters.Add("NOTES", oServicio.notasN4);
            code.parameters.Add("DATE", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            code.parameters.Add("EVENT", oServicio.codigoN4);

            //Poner el evento
            var n4r = N4Ws.Entidad.Servicios.EjecutarCODEExtensionGenerico(code, usuario);
            if (!n4r.response.ToString().Contains("<result>OK</result>"))
            {
                var ex = new ApplicationException(n4r.status_id);
                var i = p.LogError<ApplicationException>(ex, "PonerEventoCarbonoNeutroCFS", usuario);
                var emsg = string.Format("Ha ocurrido la novedad número {0} durante el proceso favor comuníquese con facturacion", i.HasValue ? i.Value : -1);
                return Respuesta.ResultadoOperacion<bool>.CrearFalla("Error al cargar evento");
            }

            return Respuesta.ResultadoOperacion<bool>.CrearResultadoExitoso(true, "Éxito al aplicar el evento");
        }

        //public static N4_BasicResponse EjecutarCODEExtensionGenerico(GroovyCodeExtension co, string usuario)
        //{
        //    var n = new N4_BasicResponse();
        //    //paso 1-> Inicializar instancia de servicio
        //    string pv;
        //    var p = InicializaServicio(out pv);
        //    p.actualMetodo = MethodBase.GetCurrentMethod().Name;
        //    if (p == null)
        //    {
        //        n.status = 3;
        //        n.status_id = "SEVERE";
        //        n.messages.Add(new N4_response_message("NO_INITIALIZED_DATA", "SEVERE", pv));
        //        return n;
        //    }
        //    //paso 2 -> inicializar instancia de n4configurariones
        //    var n4 = ObtenerInicializador(p, out pv);
        //    if (n4 == null)
        //    {
        //        n.status = 3;
        //        n.status_id = "SEVERE";
        //        n.messages.Add(new N4_response_message("NO_INITIALIZED_N4_INSTANCE", "SEVERE", pv));
        //        return n;
        //    }
        //    if (co == null)
        //    {
        //        n.status = 3;
        //        n.status_id = "SEVERE";
        //        n.messages.Add(new N4_response_message("NO_CODE_EXENSION_NULL", "SEVERE", "CODE_EXTENSON es nulo"));
        //        return n;

        //    }
        //    if (string.IsNullOrEmpty(co.location))
        //    {
        //        n.status = 3;
        //        n.status_id = "SEVERE";
        //        n.messages.Add(new N4_response_message("NO_LOCATION_FOR_CODE_EXTENSION", "SEVERE", "NO HAY LOCACION"));
        //        return n;

        //    }
        //    if (string.IsNullOrEmpty(co.name))
        //    {
        //        n.status = 3;
        //        n.status_id = "SEVERE";
        //        n.messages.Add(new N4_response_message("NO_NAME_FOR_CODE_EXTENSION", "SEVERE", "NO HAY NOMBRE"));
        //        return n;
        //    }
        //    var nbb = N4Basic.GetInstance(n4.usuario, n4.password, n4.url, n4.scope);
        //    var gs = co.ToString();

        //    var aprr = nbb.BasicInvokeService(gs, p.myClase, p.actualMetodo, usuario, 7000);
        //    return aprr;
        //}
    }
}