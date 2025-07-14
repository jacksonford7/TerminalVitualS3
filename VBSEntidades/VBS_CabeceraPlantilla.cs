using AccesoDatos;
using BillionEntidades;
using ServicesEntities;
using SqlConexion;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VBSEntidades.Calendario;
using VBSEntidades.ClaseEntidades;
using VBSEntidades.Plantilla;

namespace VBSEntidades
{
    public class VBS_CabeceraPlantilla : Cls_Bil_Base
    {

        #region "Variables"

        private static Int64? lm = -3;
        private Int64 _SECUENCIA;
        private DateTime _FECHA_CREACION;
        private DateTime _VIGENCIA_INICIAL;
        private DateTime _VIGENCIA_FINAL;
        private bool _ESTADO = false;
        private string _USUARIO_CREACION = string.Empty;
        private string _USUARIO_MOD = string.Empty;
        private string _NOMBRE_PLANTILLA = string.Empty;
        private string _xmlTurnos = string.Empty;

        private int _ID_CABECERA;
        // protected static Dictionary<string, object> parametrosVBS = null;
        #endregion

        #region "Propiedades"
        public Int64 SECUENCIA { get => _SECUENCIA; set => _SECUENCIA = value; }
        public DateTime FECHA_CREACION { get => _FECHA_CREACION; set => _FECHA_CREACION = value; }

        public DateTime VIGENCIA_INICIAL { get => _VIGENCIA_INICIAL; set => _VIGENCIA_INICIAL = value; }
        public DateTime VIGENCIA_FINAL { get => _VIGENCIA_FINAL; set => _VIGENCIA_FINAL = value; }

        public bool ESTADO { get => _ESTADO; set => _ESTADO = value; }
        public string NOMBRE_PLANTILLA { get => _NOMBRE_PLANTILLA; set => _NOMBRE_PLANTILLA = value; }
        public string USUARIO_CREACION { get => _USUARIO_CREACION; set => _USUARIO_CREACION = value; }
        public string USUARIO_MODIFICACION { get => _USUARIO_MOD; set => _USUARIO_MOD = value; }
        public string xmlTurnos { get => _xmlTurnos; set => _xmlTurnos = value; }

        public int ID_CABECERA { get => _ID_CABECERA; set => _ID_CABECERA = value; }
        public List<VBS_DetallePlantilla> DETALLE_PLANTILLA { get; set; }
        #endregion

        public VBS_CabeceraPlantilla()
        {
            init();

            this.DETALLE_PLANTILLA = new List<VBS_DetallePlantilla>();
        }


        public VBS_CabeceraPlantilla(
         Int64 _SECUENCIA,
         DateTime _FECHA_CREACION,
         bool _ESTADO,
         DateTime _VIGENCIA_INICIAL,
         DateTime _VIGENCIA_FINAL,
         string _USUARIO_CREACION,
         string _USUARIO_MODIFICACION,
         string _NOMBRE_PLANTILLA,
         int _ID_CABECERA
         )
        {
            this.ID_CABECERA = _ID_CABECERA;
            this.SECUENCIA = _SECUENCIA;
            this.ESTADO = _ESTADO;
            this.USUARIO_CREACION = _USUARIO_CREACION;
            this.USUARIO_MODIFICACION = _USUARIO_MODIFICACION;
            this.FECHA_CREACION = _FECHA_CREACION;
            this.VIGENCIA_INICIAL = _VIGENCIA_INICIAL;
            this.VIGENCIA_FINAL = _VIGENCIA_FINAL;
            this.DETALLE_PLANTILLA = new List<VBS_DetallePlantilla>();
        }
        public ResultadoOperacion<List<VBS_NombrePlantillas>> GetNombrePlantillas(string usuario, string pista)
        {
            parametros.Clear();
            parametros.Add("NOMBRE_PLANTILLA ", pista);

            var rp = BDOpe.ComandoSelectALista<VBS_NombrePlantillas>(sql_punteroVBS.Conexion_LocalVBS, "VBS_CONSULTAR_PLANTILLA", parametros);

            return rp.Exitoso ? ResultadoOperacion<List<VBS_NombrePlantillas>>.CrearResultadoExitoso(rp.Resultado) : ResultadoOperacion<List<VBS_NombrePlantillas>>.CrearFalla(rp.MensajeProblema);

        }
        public string GetParametrosValida(string pista)
        {
            parametros.Clear();
            parametros.Add("NOMBRE_PARAMETRO ", pista);

            var rp = BDOpe.ComandoTransaccionString(sql_punteroVBS.Conexion_LocalVBS, "VBS_CONSULTAR_PARAMETRO", parametros);

            return rp.Resultado;

        }
        public string GetBloqueValida(string codigo)
        {
            parametros.Clear();
            parametros.Add("Codigo_Bloque ", codigo);

            var rp = BDOpe.ComandoTransaccionString(sql_punteroVBS.Conexion_LocalVBS, "VBS_CONSULTAR_BLOQUE", parametros);

            return rp.Resultado;

        }

        public string GetContainerValida(string codigo)
        {
            parametros.Clear();
            parametros.Add("container ", codigo);

            var rp = BDOpe.ComandoTransaccionString(sql_punteroVBS.Conexion_LocalVBS, "VBS_CONSULTAR_CONTAINER", parametros);

            return rp.Resultado;

        }

        public string GetMaquinaValida(string codigo)
        {
            parametros.Clear();
            parametros.Add("Codigo_Maquina ", codigo);

            var rp = BDOpe.ComandoTransaccionString(sql_punteroVBS.Conexion_LocalVBS, "VBS_CONSULTAR_MAQUINA", parametros);

            return rp.Resultado;

        }
        public ResultadoOperacion<List<VBS_TurnosDetalle>> GetListaTurnosPorDetalleId(string usuario, int idDetalleId, string fechaDesde, string fechaHasta)
        {
            parametros.Clear();
            parametros.Add("ID_DETALLE_PLANTILLA ", idDetalleId);
            parametros.Add("fechaStringDesde", fechaDesde);
            parametros.Add("fechaStringHasta", fechaHasta);
            var rp = BDOpe.ComandoSelectALista<VBS_TurnosDetalle>(sql_punteroVBS.Conexion_LocalVBS, "VBS_CONSULTAR_TURNOS_DETALLE", parametros);

            return rp.Exitoso ? ResultadoOperacion<List<VBS_TurnosDetalle>>.CrearResultadoExitoso(rp.Resultado) : ResultadoOperacion<List<VBS_TurnosDetalle>>.CrearFalla(rp.MensajeProblema);

        }


        public ResultadoOperacion<List<VBS_LISTA_PARAMETRO>> GetListaParametro()
        {
            parametros.Clear();

            parametros.Add("fechaActual", DateTime.Now);
            var rp = BDOpe.ComandoSelectALista<VBS_LISTA_PARAMETRO>(sql_punteroVBS.Conexion_LocalVBS, "VBS_CONSULTAR_LISTA_PARAMETRO", parametros);

            return rp.Exitoso ? ResultadoOperacion<List<VBS_LISTA_PARAMETRO>>.CrearResultadoExitoso(rp.Resultado) : ResultadoOperacion<List<VBS_LISTA_PARAMETRO>>.CrearFalla(rp.MensajeProblema);

        }

        public ResultadoOperacion<List<VBS_BLOQUE>> GetListaBloque()
        {
            try
            {
                parametros.Clear();

                parametros.Add("fechaActual", DateTime.Now);
                var rp = BDOpe.ComandoSelectALista<VBS_BLOQUE>(sql_punteroVBS.Conexion_LocalVBS, "VBS_CONSULTAR_LISTA_BLOQUE", parametros);

                return rp.Exitoso ? ResultadoOperacion<List<VBS_BLOQUE>>.CrearResultadoExitoso(rp.Resultado) : ResultadoOperacion<List<VBS_BLOQUE>>.CrearFalla(rp.MensajeProblema);

            }
            catch (Exception ex)
            {
                throw new Exception("error", ex);
            }

        }

        public ResultadoOperacion<List<VBS_CONTAINER_VIP>> GetListaContainer()
        {
            try
            {
                parametros.Clear();

                parametros.Add("fechaActual", DateTime.Now);
                var rp = BDOpe.ComandoSelectALista<VBS_CONTAINER_VIP>(sql_punteroVBS.Conexion_LocalVBS, "VBS_CONSULTAR_IMPO_CONTAINERS_VIP", parametros);

                return rp.Exitoso ? ResultadoOperacion<List<VBS_CONTAINER_VIP>>.CrearResultadoExitoso(rp.Resultado) : ResultadoOperacion<List<VBS_CONTAINER_VIP>>.CrearFalla(rp.MensajeProblema);

            }
            catch (Exception ex)
            {
                throw new Exception("error", ex);
            }
            
        }
        public ResultadoOperacion<List<VBS_MAQUINA>> GetListaMaquina()
        {
            try
            {
                parametros.Clear();

                parametros.Add("fechaActual", DateTime.Now);
                var rp = BDOpe.ComandoSelectALista<VBS_MAQUINA>(sql_punteroVBS.Conexion_LocalVBS, "VBS_CONSULTAR_LISTA_MAQUINAS", parametros);

                return rp.Exitoso ? ResultadoOperacion<List<VBS_MAQUINA>>.CrearResultadoExitoso(rp.Resultado) : ResultadoOperacion<List<VBS_MAQUINA>>.CrearFalla(rp.MensajeProblema);

            }
            catch (Exception ex)
            {
                throw new Exception("error", ex);
            }

        }
        public ResultadoOperacion<List<VBS_TurnosDetalle>> GetListaTurnosPorFechas(int idDetalle, string fechaDesde, string fechaHasta)
        {
            parametros.Clear();
            parametros.Add("ID_DETALLE_PLANTILLA", idDetalle);
            parametros.Add("fechaStringDesde", fechaDesde);
            parametros.Add("fechaStringHasta", fechaHasta);
            var rp = BDOpe.ComandoSelectALista<VBS_TurnosDetalle>(sql_punteroVBS.Conexion_LocalVBS, "VBS_CONSULTAR_TURNOS_DETALLE", parametros);

            return rp.Exitoso ? ResultadoOperacion<List<VBS_TurnosDetalle>>.CrearResultadoExitoso(rp.Resultado) : ResultadoOperacion<List<VBS_TurnosDetalle>>.CrearFalla(rp.MensajeProblema);

        }
        public ResultadoOperacion<List<VBS_TurnosDetalle_Import>> GetListaTurnosImportPorFechas(int idDetalle, string fechaDesde, string fechaHasta)
        {
            parametros.Clear();
            parametros.Add("ID_BLOQUE", idDetalle);
            parametros.Add("fechaStringDesde", fechaDesde);
            parametros.Add("fechaStringHasta", fechaHasta);
            var rp = BDOpe.ComandoSelectALista<VBS_TurnosDetalle_Import>(sql_punteroVBS.Conexion_LocalVBS, "VBS_CONSULTAR_TURNOS_DETALLE_IMPORT", parametros);

            return rp.Exitoso ? ResultadoOperacion<List<VBS_TurnosDetalle_Import>>.CrearResultadoExitoso(rp.Resultado) : ResultadoOperacion<List<VBS_TurnosDetalle_Import>>.CrearFalla(rp.MensajeProblema);

        }
        public ResultadoOperacion<List<VBS_TurnosDetalle>> GetListaTurnosPorDia(string fechaHasta)
        {
            parametros.Clear();
            parametros.Add("fechaStringHasta", fechaHasta);
            var rp = BDOpe.ComandoSelectALista<VBS_TurnosDetalle>(sql_punteroVBS.Conexion_LocalVBS, "VBS_CONSULTAR_TURNOS_DIA", parametros);

            return rp.Exitoso ? ResultadoOperacion<List<VBS_TurnosDetalle>>.CrearResultadoExitoso(rp.Resultado) : ResultadoOperacion<List<VBS_TurnosDetalle>>.CrearFalla(rp.MensajeProblema);

        }
        public ResultadoOperacion<List<VBS_TurnosDetalle>> GetTablaExpoPorDia(string fechaHasta)
        {
            parametros.Clear();
            parametros.Add("fechaStringDesde", fechaHasta);
            var rp = BDOpe.ComandoSelectALista<VBS_TurnosDetalle>(sql_punteroVBS.Conexion_LocalVBS, "VBS_CONSULTAR_TABLA_EXPO_DETALLE_DIA", parametros);

            return rp.Exitoso ? ResultadoOperacion<List<VBS_TurnosDetalle>>.CrearResultadoExitoso(rp.Resultado) : ResultadoOperacion<List<VBS_TurnosDetalle>>.CrearFalla(rp.MensajeProblema);

        }
        public ResultadoOperacion<List<VBS_TurnosDetalle_Import>> GetTablaImportPorDia(string fechaHasta)
        {
            parametros.Clear();
            parametros.Add("fechaStringDesde", fechaHasta);
            var rp = BDOpe.ComandoSelectALista<VBS_TurnosDetalle_Import>(sql_punteroVBS.Conexion_LocalVBS, "VBS_CONSULTAR_TABLA_IMPORT_DETALLE_DIA", parametros);

            return rp.Exitoso ? ResultadoOperacion<List<VBS_TurnosDetalle_Import>>.CrearResultadoExitoso(rp.Resultado) : ResultadoOperacion<List<VBS_TurnosDetalle_Import>>.CrearFalla(rp.MensajeProblema);

        }

        public ResultadoOperacion<List<VBS_TurnosDetalle_Import>> GetTablaImportPorDiaZAL(string fechaHasta)
        {
            parametros.Clear();
            parametros.Add("fechaStringDesde", fechaHasta);
            var rp = BDOpe.ComandoSelectALista<VBS_TurnosDetalle_Import>(sql_punteroVBS.Conexion_LocalVBS, "VBS_CONSULTAR_TABLA_IMPORT_DETALLE_DIA_ZAL", parametros);

            return rp.Exitoso ? ResultadoOperacion<List<VBS_TurnosDetalle_Import>>.CrearResultadoExitoso(rp.Resultado) : ResultadoOperacion<List<VBS_TurnosDetalle_Import>>.CrearFalla(rp.MensajeProblema);

        }
        public ResultadoOperacion<List<VBS_TurnosDetalle>> VerificarCombinacionTipoCargaContenedor(int tipoCargaId,string tipoContenedor, string fechaInicial)
        {
            parametros.Clear();
            parametros.Add("tipoCargaId ", tipoCargaId);
            parametros.Add("tipoContenedor", tipoContenedor);
            parametros.Add("fechaInicial", fechaInicial);
            var rp = BDOpe.ComandoSelectALista<VBS_TurnosDetalle>(sql_punteroVBS.Conexion_LocalVBS, "VBS_VerificarCombinacionTipoCargaContenedor", parametros);

            return rp.Exitoso ? ResultadoOperacion<List<VBS_TurnosDetalle>>.CrearResultadoExitoso(rp.Resultado) : ResultadoOperacion<List<VBS_TurnosDetalle>>.CrearFalla(rp.MensajeProblema);

        }
        public ResultadoOperacion<List<VBS_TurnosDetalle>> VerificarCombinacionTipoBloque(int tipoBloqueID, string CodigoBloque, string fechaInicial)
        {
            parametros.Clear();
            parametros.Add("tipoBloqueID ", tipoBloqueID);
            parametros.Add("CodigoBloque", CodigoBloque);
            parametros.Add("fechaInicial", fechaInicial);
            var rp = BDOpe.ComandoSelectALista<VBS_TurnosDetalle>(sql_punteroVBS.Conexion_LocalVBS, "VBS_VerificarCombinacionTipoBloque", parametros);

            return rp.Exitoso ? ResultadoOperacion<List<VBS_TurnosDetalle>>.CrearResultadoExitoso(rp.Resultado) : ResultadoOperacion<List<VBS_TurnosDetalle>>.CrearFalla(rp.MensajeProblema);

        }


        public ResultadoOperacion<List<VBS_TurnosDetalle_Import>> GetListaTurnosPorDiaImport(string fechaHasta)
        {
            parametros.Clear();
            parametros.Add("fechaStringHasta", fechaHasta);
            var rp = BDOpe.ComandoSelectALista<VBS_TurnosDetalle_Import>(sql_punteroVBS.Conexion_LocalVBS, "VBS_CONSULTAR_TURNOS_DIA", parametros);

            return rp.Exitoso ? ResultadoOperacion<List<VBS_TurnosDetalle_Import>>.CrearResultadoExitoso(rp.Resultado) : ResultadoOperacion<List<VBS_TurnosDetalle_Import>>.CrearFalla(rp.MensajeProblema);

        }
        public ResultadoOperacion<List<VBS_TurnosDetalle>> GetListaTurnosPorDosDias(string fechaDesde, string fechaHasta)
        {
            parametros.Clear();
            
            parametros.Add("fechaStringDesde", fechaDesde);
            parametros.Add("fechaStringHasta", fechaHasta);
            var rp = BDOpe.ComandoSelectALista<VBS_TurnosDetalle>(sql_punteroVBS.Conexion_LocalVBS, "VBS_CONSULTAR_TURNOS_DOS_DIAS", parametros);

            return rp.Exitoso ? ResultadoOperacion<List<VBS_TurnosDetalle>>.CrearResultadoExitoso(rp.Resultado) : ResultadoOperacion<List<VBS_TurnosDetalle>>.CrearFalla(rp.MensajeProblema);

        }
        public ResultadoOperacion<List<VBS_DETALLE_MONITOR>> GetListaTurnosExpoTBL1(string fechaDesde, string fechaHasta,int idTipoCarga)
        {
            parametros.Clear();

            parametros.Add("FechaDesde", fechaDesde);
            parametros.Add("FechaHasta", fechaHasta);
            parametros.Add("idTipoCarga", idTipoCarga);
            var rp = BDOpe.ComandoSelectALista<VBS_DETALLE_MONITOR>(sql_punteroVBS.Conexion_LocalVBS, "VBS_DETALLE_MONITOR", parametros);

            return rp.Exitoso ? ResultadoOperacion<List<VBS_DETALLE_MONITOR>>.CrearResultadoExitoso(rp.Resultado) : ResultadoOperacion<List<VBS_DETALLE_MONITOR>>.CrearFalla(rp.MensajeProblema);

        }
        public ResultadoOperacion<List<VBS_DETALLE_MONITOR2>> GetListaTurnosExpoTBL2(string fechaDesde, string fechaHasta, int idTipoCarga)
        {
            parametros.Clear();

            parametros.Add("FechaDesde", fechaDesde);
            parametros.Add("FechaHasta", fechaHasta);
            parametros.Add("idTipoCarga", idTipoCarga);
            var rp = BDOpe.ComandoSelectALista<VBS_DETALLE_MONITOR2>(sql_punteroVBS.Conexion_LocalVBS, "VBS_DETALLE_MONITOR2", parametros);

            return rp.Exitoso ? ResultadoOperacion<List<VBS_DETALLE_MONITOR2>>.CrearResultadoExitoso(rp.Resultado) : ResultadoOperacion<List<VBS_DETALLE_MONITOR2>>.CrearFalla(rp.MensajeProblema);

        }

        public ResultadoOperacion<List<VBS_DETALLE_MONITOR_IMPORT>> GetListaTurnosImportBL1(string fechaDesde, string fechaHasta, int idTipoCarga)
        {
            parametros.Clear();

            parametros.Add("FechaDesde", fechaDesde);
            parametros.Add("FechaHasta", fechaHasta);
            var rp = BDOpe.ComandoSelectALista<VBS_DETALLE_MONITOR_IMPORT>(sql_punteroVBS.Conexion_LocalVBS, "VBS_DETALLE_MONITOR_IMPORT", parametros);

            return rp.Exitoso ? ResultadoOperacion<List<VBS_DETALLE_MONITOR_IMPORT>>.CrearResultadoExitoso(rp.Resultado) : ResultadoOperacion<List<VBS_DETALLE_MONITOR_IMPORT>>.CrearFalla(rp.MensajeProblema);

        }
        public ResultadoOperacion<List<VBS_DETALLE_MONITOR2_IMPORT>> GetListaTurnosImportBL2(string fechaDesde, string fechaHasta, int idTipoCarga)
        {
            parametros.Clear();

            parametros.Add("FechaDesde", fechaDesde);
            parametros.Add("FechaHasta", fechaHasta);
            var rp = BDOpe.ComandoSelectALista<VBS_DETALLE_MONITOR2_IMPORT>(sql_punteroVBS.Conexion_LocalVBS, "VBS_DETALLE_MONITOR_IMPORT2", parametros);

            return rp.Exitoso ? ResultadoOperacion<List<VBS_DETALLE_MONITOR2_IMPORT>>.CrearResultadoExitoso(rp.Resultado) : ResultadoOperacion<List<VBS_DETALLE_MONITOR2_IMPORT>>.CrearFalla(rp.MensajeProblema);

        } 

        public ResultadoOperacion<List<VBS_TurnosDetalle>> GetListaTurnosPorDiaTipoContenedor(string fechaHasta,string iso,int tiempoEspera)
        {
            parametros.Clear();
            parametros.Add("fechaStringHasta", fechaHasta);
            parametros.Add("tipoContenedor", iso);
            parametros.Add("porcentajeCantidad ", tiempoEspera);
            var rp = BDOpe.ComandoSelectALista<VBS_TurnosDetalle>(sql_punteroVBS.Conexion_LocalVBS, "VBS_CONSULTAR_TURNOS_DIA_TIPOCONTENEDOR", parametros);

            return rp.Exitoso ? ResultadoOperacion<List<VBS_TurnosDetalle>>.CrearResultadoExitoso(rp.Resultado) : ResultadoOperacion<List<VBS_TurnosDetalle>>.CrearFalla(rp.MensajeProblema);

        }
        public ResultadoOperacion<List<VBS_TurnosDetalle>> GetListaTurnosPorDiaTipoContenedorALL(string fechaHasta, string iso,int tiempoEspera)
        {
            parametros.Clear();
            parametros.Add("fechaStringHasta", fechaHasta);
            parametros.Add("tipoContenedor", iso);
            parametros.Add("porcentajeCantidad ", tiempoEspera);
            var rp = BDOpe.ComandoSelectALista<VBS_TurnosDetalle>(sql_punteroVBS.Conexion_LocalVBS, "VBS_CONSULTAR_TURNOS_DIA_TIPOCONTENEDOR_ALL", parametros);

            return rp.Exitoso ? ResultadoOperacion<List<VBS_TurnosDetalle>>.CrearResultadoExitoso(rp.Resultado) : ResultadoOperacion<List<VBS_TurnosDetalle>>.CrearFalla(rp.MensajeProblema);

        }

        public ResultadoOperacion<List<Calendario_Turnos>> GetListaCalendario(int anio, int mes)
        {
            parametros.Clear();
            parametros.Add("anio", anio);
            parametros.Add("mes", mes);
            var rp = BDOpe.ComandoSelectALista<Calendario_Turnos>(sql_punteroVBS.Conexion_LocalVBS, "VBS_CONSULTAR_TURNOS_CALENDARIO", parametros);

            return rp.Exitoso ? ResultadoOperacion<List<Calendario_Turnos>>.CrearResultadoExitoso(rp.Resultado) : ResultadoOperacion<List<Calendario_Turnos>>.CrearFalla(rp.MensajeProblema);

        }
        public ResultadoOperacion<List<Calendario_Turnos_Import>> GetListaCalendarioImport(int anio, int mes)
        {
            parametros.Clear();
            parametros.Add("anio", anio);
            parametros.Add("mes", mes);
            var rp = BDOpe.ComandoSelectALista<Calendario_Turnos_Import>(sql_punteroVBS.Conexion_LocalVBS, "VBS_CONSULTAR_TURNOS_CALENDARIO_IMPORT", parametros);

            return rp.Exitoso ? ResultadoOperacion<List<Calendario_Turnos_Import>>.CrearResultadoExitoso(rp.Resultado) : ResultadoOperacion<List<Calendario_Turnos_Import>>.CrearFalla(rp.MensajeProblema);

        }

        public ResultadoOperacion<List<Calendario_Turnos_Import>> GetListaCalendarioImportZAL(int anio, int mes)
        {
            parametros.Clear();
            parametros.Add("anio", anio);
            parametros.Add("mes", mes);
            var rp = BDOpe.ComandoSelectALista<Calendario_Turnos_Import>(sql_punteroVBS.Conexion_LocalVBS, "VBS_CONSULTAR_TURNOS_CALENDARIO_IMPORT_ZAL", parametros);

            return rp.Exitoso ? ResultadoOperacion<List<Calendario_Turnos_Import>>.CrearResultadoExitoso(rp.Resultado) : ResultadoOperacion<List<Calendario_Turnos_Import>>.CrearFalla(rp.MensajeProblema);

        }


        public ResultadoOperacion<List<VBS_LISTA_PARAMETRO>> GuardarParametro(VBS_LISTA_PARAMETRO obj)
        {
            parametros.Clear();
            parametros.Add("Tipo ", obj.Tipo);
            parametros.Add("Parametro ", obj.Parametro);
            parametros.Add("Descripcion ", obj.Descripcion);
            parametros.Add("Valor ", obj.Valor);
           



            var rp = BDOpe.ComandoSelectALista<VBS_LISTA_PARAMETRO>(sql_punteroVBS.Conexion_LocalVBS, "VBS_INSERT_PARAMETRO", parametros);

            return rp.Exitoso ? ResultadoOperacion<List<VBS_LISTA_PARAMETRO>>.CrearResultadoExitoso(rp.Resultado) : ResultadoOperacion<List<VBS_LISTA_PARAMETRO>>.CrearFalla(rp.MensajeProblema);

        }
        public ResultadoOperacion<List<VBS_BLOQUE>> GuardarBloque(VBS_BLOQUE obj)
        {
            parametros.Clear();
            parametros.Add("Codigo ", obj.Codigo);
            parametros.Add("NumeroFilas ", obj.NumeroFilas);
            parametros.Add("NumeroColumnas ", obj.NumeroColumnas);
            parametros.Add("Estado ", obj.Estado);
            parametros.Add("Visible ", obj.EsVisible);
            parametros.Add("USUARIO_REGISTRO ", obj.UsuarioRegistro);

            var rp = BDOpe.ComandoSelectALista<VBS_BLOQUE>(sql_punteroVBS.Conexion_LocalVBS, "VBS_INSERT_BLOQUE", parametros);

            return rp.Exitoso ? ResultadoOperacion<List<VBS_BLOQUE>>.CrearResultadoExitoso(rp.Resultado) : ResultadoOperacion<List<VBS_BLOQUE>>.CrearFalla(rp.MensajeProblema);

        }

        public ResultadoOperacion<List<VBS_CONTAINER_VIP>> GuardarContainer(VBS_CONTAINER_VIP obj)
        {
            parametros.Clear();
            parametros.Add("container ", obj.container);
            parametros.Add("description ", obj.description);
            parametros.Add("crea_user ", obj.crea_user);


            var rp = BDOpe.ComandoSelectALista<VBS_CONTAINER_VIP>(sql_punteroVBS.Conexion_LocalVBS, "VBS_INSERT_CONTAINER", parametros);

            return rp.Exitoso ? ResultadoOperacion<List<VBS_CONTAINER_VIP>>.CrearResultadoExitoso(rp.Resultado) : ResultadoOperacion<List<VBS_CONTAINER_VIP>>.CrearFalla(rp.MensajeProblema);

        }

        public ResultadoOperacion<List<VBS_MAQUINA>> GuardarMaquina(VBS_MAQUINA obj)
        {
            parametros.Clear();
            parametros.Add("Codigo ", obj.Codigo);
            parametros.Add("Tipo ", obj.Tipo);
            parametros.Add("CapacidadOperativa ", obj.CapacidadOperativa);
            parametros.Add("Estado ", obj.Estado);

            parametros.Add("USUARIO_REGISTRO ", obj.UsuarioRegistro);




            var rp = BDOpe.ComandoSelectALista<VBS_MAQUINA>(sql_punteroVBS.Conexion_LocalVBS, "VBS_INSERT_MAQUINA", parametros);

            return rp.Exitoso ? ResultadoOperacion<List<VBS_MAQUINA>>.CrearResultadoExitoso(rp.Resultado) : ResultadoOperacion<List<VBS_MAQUINA>>.CrearFalla(rp.MensajeProblema);

        }

        public ResultadoOperacion<List<VBS_BLOQUE>> EditarBloque(VBS_BLOQUE obj)
        {
            parametros.Clear();
            parametros.Add("IdBloque", obj.IdBloque);
            parametros.Add("Codigo ", obj.Codigo);
            parametros.Add("NumeroFilas ", obj.NumeroFilas);
            parametros.Add("NumeroColumnas ", obj.NumeroColumnas);
            parametros.Add("Estado ", obj.Estado);
            parametros.Add("Visible ", obj.EsVisible);
            parametros.Add("USUARIO_REGISTRO ", obj.UsuarioRegistro);
       


            var rp = BDOpe.ComandoSelectALista<VBS_BLOQUE>(sql_punteroVBS.Conexion_LocalVBS, "VBS_EDIT_BLOQUE", parametros);

            return rp.Exitoso ? ResultadoOperacion<List<VBS_BLOQUE>>.CrearResultadoExitoso(rp.Resultado) : ResultadoOperacion<List<VBS_BLOQUE>>.CrearFalla(rp.MensajeProblema);

        }

        public ResultadoOperacion<List<VBS_CONTAINER_VIP>> EditarContainer(VBS_CONTAINER_VIP obj)
        {
            parametros.Clear();
            parametros.Add("config_id", obj.config_id);
            parametros.Add("container ", obj.container);
            parametros.Add("description ", obj.description);
            parametros.Add("crea_user ", obj.crea_user);

            var rp = BDOpe.ComandoSelectALista<VBS_CONTAINER_VIP>(sql_punteroVBS.Conexion_LocalVBS, "VBS_EDIT_CONTAINER", parametros);

            return rp.Exitoso ? ResultadoOperacion<List<VBS_CONTAINER_VIP>>.CrearResultadoExitoso(rp.Resultado) : ResultadoOperacion<List<VBS_CONTAINER_VIP>>.CrearFalla(rp.MensajeProblema);

        }


        public ResultadoOperacion<List<VBS_NombrePlantillas>> EditarHorasImport(DateTime fechaDesde_, TimeSpan horaDesde, DateTime fechaHasta_, TimeSpan horaHasta, int idTipoBloque)
        {
            try
            {

                var fechasda = fechaDesde_.ToString();
                var fechasda_ = fechaHasta_.ToString();


                parametros.Clear();
                parametros.Add("FECHA_DESDE", fechaDesde_.ToString("dd/MM/yyyy"));
                parametros.Add("FECHA_HASTA", fechaHasta_.ToString("dd/MM/yyyy"));
                parametros.Add("HORA_DESDE", horaDesde);
                parametros.Add("HORA_HASTA", horaHasta);
                parametros.Add("idTipoBloque", idTipoBloque);


                var rp = BDOpe.ComandoSelectALista<VBS_NombrePlantillas>(sql_punteroVBS.Conexion_LocalVBS, "VBS_EDITA_TURNOS_IMPO", parametros);

                return rp.Exitoso ? ResultadoOperacion<List<VBS_NombrePlantillas>>.CrearResultadoExitoso(rp.Resultado) : ResultadoOperacion<List<VBS_NombrePlantillas>>.CrearFalla(rp.MensajeProblema);

            }
            catch(Exception ex)
            {
                throw new Exception("ha ocurrido un error", ex);
            }
           
        }

        public ResultadoOperacion<List<VBS_NombrePlantillas>> DuplicarHorasImport(DateTime fechaDesde_, TimeSpan horaDesde, DateTime fechaHasta_, TimeSpan horaHasta, int idTipoBloque, string USUARIO)
        {
            try
            {

                var fechasda = fechaDesde_.ToString();
                var fechasda_ = fechaHasta_.ToString();


                parametros.Clear();
                parametros.Add("FECHA_DESDE", fechaDesde_.ToString("dd/MM/yyyy"));
                parametros.Add("FECHA_HASTA", fechaHasta_.ToString("dd/MM/yyyy"));
                parametros.Add("HORA_DESDE", horaDesde);
                parametros.Add("HORA_HASTA", horaHasta);
                parametros.Add("idTipoBloque", idTipoBloque);
                parametros.Add("USUARIO", USUARIO);

                var rp = BDOpe.ComandoSelectALista<VBS_NombrePlantillas>(sql_punteroVBS.Conexion_LocalVBS, "VBS_COPIAR_TURNOS_IMPO", parametros);

                return rp.Exitoso ? ResultadoOperacion<List<VBS_NombrePlantillas>>.CrearResultadoExitoso(rp.Resultado) : ResultadoOperacion<List<VBS_NombrePlantillas>>.CrearFalla(rp.MensajeProblema);

            }
            catch (Exception ex)
            {
                throw new Exception("ha ocurrido un error", ex);
            }

        }

        public ResultadoOperacion<List<VBS_NombrePlantillas>> EditarHorasExpo(DateTime fechaDesde_, TimeSpan horaDesde, DateTime fechaHasta_, TimeSpan horaHasta, int idTipoCargas)
        {
            try
            {

                var fechasda = fechaDesde_.ToString();
                var fechasda_ = fechaHasta_.ToString();


                parametros.Clear();
                parametros.Add("FECHA_DESDE", fechaDesde_.ToString("dd/MM/yyyy"));
                parametros.Add("FECHA_HASTA", fechaHasta_.ToString("dd/MM/yyyy"));
                parametros.Add("HORA_DESDE", horaDesde);
                parametros.Add("HORA_HASTA", horaHasta);
                parametros.Add("idTipoCargas", idTipoCargas);


                var rp = BDOpe.ComandoSelectALista<VBS_NombrePlantillas>(sql_punteroVBS.Conexion_LocalVBS, "VBS_EDITA_TURNOS_EXPO", parametros);

                return rp.Exitoso ? ResultadoOperacion<List<VBS_NombrePlantillas>>.CrearResultadoExitoso(rp.Resultado) : ResultadoOperacion<List<VBS_NombrePlantillas>>.CrearFalla(rp.MensajeProblema);

            }
            catch (Exception ex)
            {
                throw new Exception("ha ocurrido un error", ex);
            }

        }

        public ResultadoOperacion<List<VBS_MAQUINA>> EditarMaquina(VBS_MAQUINA obj)
        {
            parametros.Clear();
            parametros.Add("IdMaquina", obj.IdMaquina);
            parametros.Add("Codigo ", obj.Codigo);
            parametros.Add("CapacidadOperativa ", obj.CapacidadOperativa);
            parametros.Add("Tipo ", obj.Tipo);
            parametros.Add("Estado ", obj.Estado);
            parametros.Add("USUARIO_REGISTRO ", obj.UsuarioRegistro);




            var rp = BDOpe.ComandoSelectALista<VBS_MAQUINA>(sql_punteroVBS.Conexion_LocalVBS, "VBS_EDIT_MAQUINA", parametros);

            return rp.Exitoso ? ResultadoOperacion<List<VBS_MAQUINA>>.CrearResultadoExitoso(rp.Resultado) : ResultadoOperacion<List<VBS_MAQUINA>>.CrearFalla(rp.MensajeProblema);

        }


        public ResultadoOperacion<List<VBS_NombrePlantillas>> GuardarTurnos(int  idCabecera,int idDetalle,
            string tipoContenedor,string tipoCargas,int cantidad,string categoria, string usuario_Creacion,DateTime fechaDesde,DateTime fechaHasta)
        {
            parametros.Clear();
            parametros.Add("IDCABECERA_PLANTILLA ", idCabecera);
            parametros.Add("IDDETALLE_PLANTILLA ", idDetalle);
            parametros.Add("TIPO_CONTENEDOR ", tipoContenedor);
            parametros.Add("TIPO_CARGAS ", tipoCargas);
            parametros.Add("CANTIDAD ", cantidad);
            parametros.Add("CATEGORIA ", categoria);
            parametros.Add("USUARIO_CREACION ", usuario_Creacion);
            parametros.Add("FECHA_DESDE ", fechaDesde);
            parametros.Add("FECHA_HASTA ", fechaHasta);



            var rp = BDOpe.ComandoSelectALista<VBS_NombrePlantillas>(sql_punteroVBS.Conexion_LocalVBS, "VBS_GENERA_TURNOS", parametros);

            return rp.Exitoso ? ResultadoOperacion<List<VBS_NombrePlantillas>>.CrearResultadoExitoso(rp.Resultado) : ResultadoOperacion<List<VBS_NombrePlantillas>>.CrearFalla(rp.MensajeProblema);

        }

        public ResultadoOperacion<List<VBS_NombrePlantillas>> GuardarTurnosImport(string bloqueId, string tipoBloque, int frecuencia, string usuario_Creacion, DateTime fechaDesde, DateTime fechaHasta)
        {
            try
            {
                parametros.Clear();
                parametros.Add("IdBloque ", bloqueId);
                parametros.Add("CodigoBloque ", tipoBloque);
                parametros.Add("FRECUENCIA ", frecuencia);
                parametros.Add("USUARIO_CREACION ", usuario_Creacion);
                parametros.Add("FECHA_DESDE ", fechaDesde);
                parametros.Add("FECHA_HASTA ", fechaHasta);



                var rp = BDOpe.ComandoSelectALista<VBS_NombrePlantillas>(sql_punteroVBS.Conexion_LocalVBS, "VBS_GENERA_TURNOS_IMPO", parametros);

                return rp.Exitoso ? ResultadoOperacion<List<VBS_NombrePlantillas>>.CrearResultadoExitoso(rp.Resultado) : ResultadoOperacion<List<VBS_NombrePlantillas>>.CrearFalla(rp.MensajeProblema);

            }
            catch (Exception ex)
            {
                throw new Exception("error", ex);
            }


       
        }

        public ResultadoOperacion<List<VBS_NombrePlantillas>> BuscarIdExiste(int idCabecera)
        {
            parametros.Clear();
            parametros.Add("IDCABECERA_PLANTILLA ", idCabecera);

            var rp = BDOpe.ComandoSelectALista<VBS_NombrePlantillas>(sql_punteroVBS.Conexion_LocalVBS, "VBS_CONSULTAR_ID_CABECERA_EXISTE", parametros);

            return rp.Exitoso ? ResultadoOperacion<List<VBS_NombrePlantillas>>.CrearResultadoExitoso(rp.Resultado) : ResultadoOperacion<List<VBS_NombrePlantillas>>.CrearFalla(rp.MensajeProblema);

        }
        public ResultadoOperacion<List<VBS_NombrePlantillas>> EditarCantidad(int idTurno, int Cantidad,string usuarioModificacion)
        {
            parametros.Clear();
            parametros.Add("ID_TURNO ", idTurno);
            parametros.Add("CANTIDAD ", Cantidad);
            parametros.Add("USUARIO_MODIFICACION ", usuarioModificacion);



            var rp = BDOpe.ComandoSelectALista<VBS_NombrePlantillas>(sql_punteroVBS.Conexion_LocalVBS, "VBS_UPDATE_TURNOS_CANTIDAD", parametros);

            return rp.Exitoso ? ResultadoOperacion<List<VBS_NombrePlantillas>>.CrearResultadoExitoso(rp.Resultado) : ResultadoOperacion<List<VBS_NombrePlantillas>>.CrearFalla(rp.MensajeProblema);

        }
        public ResultadoOperacion<List<VBS_LISTA_PARAMETRO>> EditarParametro(VBS_LISTA_PARAMETRO obj, string usuarioModificacion)
        {
            parametros.Clear();
            parametros.Add("idParametro", obj.IdParametro);
            parametros.Add("valor ", obj.Valor);
            parametros.Add("Tipo ", obj.Tipo);
            parametros.Add("Parametro ", obj.Parametro);
            parametros.Add("Descripcion ", obj.Descripcion);
            parametros.Add("USUARIO_MODIFICACION ", usuarioModificacion);



            var rp = BDOpe.ComandoSelectALista<VBS_LISTA_PARAMETRO>(sql_punteroVBS.Conexion_LocalVBS, "VBS_UPDATE_PARAMETRO", parametros);

            return rp.Exitoso ? ResultadoOperacion<List<VBS_LISTA_PARAMETRO>>.CrearResultadoExitoso(rp.Resultado) : ResultadoOperacion<List<VBS_LISTA_PARAMETRO>>.CrearFalla(rp.MensajeProblema);

        }

        public ResultadoOperacion<List<VBS_NombrePlantillas>> EditarTurnoDisponibles(int idTurno, 
            string usuarioModificacion,DateTime fechaModificacion,string aisv,string contenedor,string chofer,string placa)
        {
            parametros.Clear();
            parametros.Add("ID_TURNO ", idTurno);
            parametros.Add("USUARIO_MODIFICACION ", usuarioModificacion);
            parametros.Add("FECHA_MODIFICACION ", fechaModificacion);
            parametros.Add("AISV ", aisv);
            parametros.Add("CONTENEDOR ", contenedor);
            parametros.Add("CHOFER ", chofer);
            parametros.Add("PLACA ", placa);
        


            var rp = BDOpe.ComandoSelectALista<VBS_NombrePlantillas>(sql_punteroVBS.Conexion_LocalVBS, "VBS_UPDATE_TURNOS_DISPONIBLE", parametros);

            return rp.Exitoso ? ResultadoOperacion<List<VBS_NombrePlantillas>>.CrearResultadoExitoso(rp.Resultado) : ResultadoOperacion<List<VBS_NombrePlantillas>>.CrearFalla(rp.MensajeProblema);

        }
        public ResultadoOperacion<List<VBS_NombrePlantillas>> EditarTurnoCanceladoAISV(string aisv ,int idTurno, string usuarioModificacion, DateTime fechaModificacion)
        {
            parametros.Clear();

            parametros.Add("AISV", aisv);
            parametros.Add("ID_TURNO ", idTurno);
            parametros.Add("USUARIO_MODIFICACION ", usuarioModificacion);
            parametros.Add("FECHA_MODIFICACION ", fechaModificacion);



            var rp = BDOpe.ComandoSelectALista<VBS_NombrePlantillas>(sql_punteroVBS.Conexion_LocalVBS, "VBS_UPDATE_TURNOS_CANCELADO", parametros);

            return rp.Exitoso ? ResultadoOperacion<List<VBS_NombrePlantillas>>.CrearResultadoExitoso(rp.Resultado) : ResultadoOperacion<List<VBS_NombrePlantillas>>.CrearFalla(rp.MensajeProblema);

        }



        public ResultadoOperacion<List<VBS_ConsultarPlantillaDetalle>> DetallePlantillas(string usuario, int IdCabecera)
        {
            parametros.Clear();
            parametros.Add("IdCabecera ", IdCabecera);

            var rp = BDOpe.ComandoSelectALista<VBS_ConsultarPlantillaDetalle>(sql_punteroVBS.Conexion_LocalVBS, "VBS_CONSULTAR_PLANTILLAS_DETALLE", parametros);

            return rp.Exitoso ? ResultadoOperacion<List<VBS_ConsultarPlantillaDetalle>>.CrearResultadoExitoso(rp.Resultado) : ResultadoOperacion<List<VBS_ConsultarPlantillaDetalle>>.CrearFalla(rp.MensajeProblema);

        }

        public ResultadoOperacion<List<VBS_Detalle_Vacios>> ConsultarDetalleVacios( int idTurno)
        {
            parametros.Clear();
            parametros.Add("IdTurno ", idTurno);

            var rp = BDOpe.ComandoSelectALista<VBS_Detalle_Vacios>(sql_punteroVBS.Conexion_LocalVBS, "VBS_CONSULTAR_DETALLE_VACIOS", parametros);

            return rp.Exitoso ? ResultadoOperacion<List<VBS_Detalle_Vacios>>.CrearResultadoExitoso(rp.Resultado) : ResultadoOperacion<List<VBS_Detalle_Vacios>>.CrearFalla(rp.MensajeProblema);

        }


        public static List<VBS_ConsultarTipoCargas> ConsultarTipoCargas(out string OnError)
        {
            parametros.Clear();

            return sql_punteroVBS.ExecuteSelectControl<VBS_ConsultarTipoCargas>(sql_punteroVBS.Conexion_LocalVBS, 8000, "VBS_CONSULTAR_TIPO_CARGAS", null, out OnError);

        }


        public Int64? SaveTransaction(out string OnError)
        {

            Int64 ID = 0;
            try
            {

                //grabar transaccion.
                var id = this.Save(out OnError);
                if (!id.HasValue)
                {
                    OnError = "*** Error: al grabar turnos ****";
                    return 0;
                }
                ID = id.Value;

                return ID;

            }
            catch (Exception ex)
            {

                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>("SQL", nameof(SaveTransaction), "SaveTransaction", false, null, null, ex.StackTrace, ex);
                OnError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));

                return null;
            }
        }
        private Int64? Save(out string OnError)
        {

            parametros.Clear();
            parametros.Add("NOMBRE_PLANTILLA", this.NOMBRE_PLANTILLA);
            parametros.Add("ESTADO", this.ESTADO);
            parametros.Add("USUARIO_CREACION", this.USUARIO_CREACION);
            parametros.Add("VIGENCIA_FINAL", this.VIGENCIA_FINAL);
            parametros.Add("VIGENCIA_INICIAL", this.VIGENCIA_INICIAL);
            parametros.Add("xmlTurnos", this.xmlTurnos);

            var db = sql_punteroVBS.ExecuteInsertUpdateDeleteReturn(sql_punteroVBS.Conexion_LocalVBS, 8000, "VBS_GRABAR_PLANTILLAS", parametros, out OnError);
            if (!db.HasValue || db.Value < 0)
            {
                return null;
            }
            OnError = string.Empty;
            return db.Value;

        }


        public Int64? SaveTransactionUPDATE(out string OnError)
        {
            Int64 ID = 0;
            try
            {
                // Grabar transacción.
                string error;
                var id = SaveUPDATE(out error);
                if (id == null)
                {
                    OnError = "*** Error: al grabar turnos ****";
                    return null;
                }
                ID = id.Value;
                OnError = string.Empty;
                return ID;
            }
            catch (Exception ex)
            {
                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>("SQL", nameof(SaveTransactionUPDATE), "SaveTransaction", false, null, null, ex.StackTrace, ex);
                OnError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                return null;
            }
        }

        public Int64? SaveTransactionUPDATEIMPORT(out string OnError)
        {
            Int64 ID = 0;
            try
            {
                // Grabar transacción.
                string error;
                var id = SaveUPDATEImport(out error);
                if (id == null)
                {
                    OnError = "*** Error: al grabar turnos ****";
                    return null;
                }
                ID = id.Value;
                OnError = string.Empty;
                return ID;
            }
            catch (Exception ex)
            {
                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>("SQL", nameof(SaveTransactionUPDATE), "SaveTransaction", false, null, null, ex.StackTrace, ex);
                OnError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                return null;
            }
        }

        public Int64? SaveTransactionUPDetalleVacios(out string OnError)
        {
            Int64 ID = 0;
            try
            {
                // Grabar transacción.
                string error;
                var id = SaveUPDATEDetalleVacios(out error);
                if (id == null)
                {
                    OnError = "*** Error: al grabar turnos ****";
                    return null;
                }
                ID = id.Value;
                OnError = string.Empty;
                return ID;
            }
            catch (Exception ex)
            {
                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>("SQL", nameof(SaveTransactionUPDATE), "SaveTransaction", false, null, null, ex.StackTrace, ex);
                OnError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                return null;
            }
        }
        private Int64? SaveUPDATE(out string OnError)
        {
            parametros.Clear();
            parametros.Add("xmlTurnos", this.xmlTurnos);

            int rowCount;
            var db = sql_punteroVBS.ExecuteInsertUpdateTurnos(sql_punteroVBS.Conexion_LocalVBS, 8000, "VBS_UPDATE_TURNOS_CANTIDAD", parametros, out rowCount, out OnError);
            if (db == null)
            {
                OnError = string.Empty;
                return null;
            }
            return db;
        }
    


        private Int64? SaveUPDATEImport(out string OnError)
        {
            parametros.Clear();
            parametros.Add("xmlTurnos", this.xmlTurnos);

            int rowCount;
            var db = sql_punteroVBS.ExecuteInsertUpdateTurnos(sql_punteroVBS.Conexion_LocalVBS, 8000, "VBS_UPDATE_TURNOS_CANTIDAD_IMPORT", parametros, out rowCount, out OnError);
            if (db == null)
            {
                OnError = string.Empty;
                return null;
            }
            return db;
        }

        private Int64? SaveUPDATEDetalleVacios(out string OnError)
        {
            parametros.Clear();
            parametros.Add("xmlTurnos", this.xmlTurnos);

            int rowCount;
            var db = sql_punteroVBS.ExecuteInsertUpdateTurnos(sql_punteroVBS.Conexion_LocalVBS, 8000, "VBS_UPDATE_TURNOS_CANTIDAD_Vacios", parametros, out rowCount, out OnError);
            if (db == null)
            {
                OnError = string.Empty;
                return null;
            }
            return db;
        }

        public DataTable CabeceraExpoReporte(DateTime fechaDesde,int tipoCargaId)
        {
            parametros.Clear();
            parametros.Add("FechaDesde", fechaDesde);
            parametros.Add("FechaHasta", fechaDesde);
            parametros.Add("idTipoCarga", tipoCargaId);

            var rp = BDOpe.ComadoSelectADatatable(sql_punteroVBS.Conexion_LocalVBS, "VBS_CABECERA_REPORTE_EXPO", parametros);

            if (rp.Exitoso && rp.Resultado != null)
            {
                return rp.Resultado;
            }
            else
            {
              
                throw new Exception(rp.MensajeProblema);
               
            }
        }


        public DataTable CabeceraImportReporte(DateTime fechaDesde, int tipoCargaId)
        {
            parametros.Clear();
            parametros.Add("FechaDesde", fechaDesde);
            parametros.Add("FechaHasta", fechaDesde);
            parametros.Add("idTipoBloque", tipoCargaId);

            var rp = BDOpe.ComadoSelectADatatable(sql_punteroVBS.Conexion_LocalVBS, "VBS_CABECERA_REPORTE_IMPORT", parametros);

            if (rp.Exitoso && rp.Resultado != null)
            {
                return rp.Resultado;
            }
            else
            {
              
                throw new Exception(rp.MensajeProblema);
            
            }
        }


        #region "Exportaciones por lineas"

        #region "Grabar turnos Exportaciones por lineas"

        public Int64? SaveTransaction_Expo_Lineas(out string OnError)
        {

            Int64 ID = 0;
            try
            {

                //grabar transaccion.
                var id = this.Save_Expo_Lineas(out OnError);
                if (!id.HasValue)
                {
                    OnError = "*** Error: al grabar turnos por Líneas Navieras ****";
                    return 0;
                }
                ID = id.Value;

                return ID;

            }
            catch (Exception ex)
            {

                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>("SQL", nameof(SaveTransaction), "SaveTransaction", false, null, null, ex.StackTrace, ex);
                OnError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));

                return null;
            }
        }
        private Int64? Save_Expo_Lineas(out string OnError)
        {

            parametros.Clear();
            parametros.Add("NOMBRE_PLANTILLA", this.NOMBRE_PLANTILLA);
            parametros.Add("ESTADO", this.ESTADO);
            parametros.Add("USUARIO_CREACION", this.USUARIO_CREACION);
            parametros.Add("VIGENCIA_FINAL", this.VIGENCIA_FINAL);
            parametros.Add("VIGENCIA_INICIAL", this.VIGENCIA_INICIAL);
            parametros.Add("xmlTurnos", this.xmlTurnos);

            var db = sql_punteroVBS.ExecuteInsertUpdateDeleteReturn(sql_punteroVBS.Conexion_LocalVBS, 8000, "VBS_GRABAR_PLANTILLAS_PORLINEAS", parametros, out OnError);
            if (!db.HasValue || db.Value < 0)
            {
                return null;
            }
            OnError = string.Empty;
            return db.Value;

        }

        #endregion

        #region "Valida existe linea"
        public ResultadoOperacion<List<VBS_TurnosDetalleLineas>> VerificarCombinacionLineas(string IdLinea, string fechaInicial)
        {
            parametros.Clear();
            parametros.Add("IdLinea ", IdLinea);
            parametros.Add("fechaInicial", fechaInicial);
            var rp = BDOpe.ComandoSelectALista<VBS_TurnosDetalleLineas>(sql_punteroVBS.Conexion_LocalVBS, "VBS_VerificarExieteTurnoLinea", parametros);

            return rp.Exitoso ? ResultadoOperacion<List<VBS_TurnosDetalleLineas>>.CrearResultadoExitoso(rp.Resultado) : ResultadoOperacion<List<VBS_TurnosDetalleLineas>>.CrearFalla(rp.MensajeProblema);

        }
        #endregion

        #region "Consultar trunos Exportaciones por linea"
        public ResultadoOperacion<List<VBS_ConsultarPlantillaDetalleLineas>> DetallePlantillas_Expo_Lineas(string usuario, Int64 IdCabecera)
        {
            parametros.Clear();
            parametros.Add("IdCabecera ", IdCabecera);

            var rp = BDOpe.ComandoSelectALista<VBS_ConsultarPlantillaDetalleLineas>(sql_punteroVBS.Conexion_LocalVBS, "VBS_CONSULTAR_PLANTILLAS_DETALLELINEAS", parametros);

            return rp.Exitoso ? ResultadoOperacion<List<VBS_ConsultarPlantillaDetalleLineas>>.CrearResultadoExitoso(rp.Resultado) : ResultadoOperacion<List<VBS_ConsultarPlantillaDetalleLineas>>.CrearFalla(rp.MensajeProblema);

        }
        #endregion

        #region "Grabar turnos"
        public ResultadoOperacion<List<VBS_NombrePlantillas>> GuardarTurnos_PorLineas(Int64 idCabecera, Int64 idDetalle,
          string IdLinea,  int cantidad, string categoria, string usuario_Creacion, DateTime fechaDesde, DateTime fechaHasta)
        {
            parametros.Clear();
            parametros.Add("IDCABECERA_PLANTILLA ", idCabecera);
            parametros.Add("IDDETALLE_PLANTILLA ", idDetalle);
            parametros.Add("ID_LINEA ", IdLinea);
            parametros.Add("CANTIDAD ", cantidad);
            parametros.Add("CATEGORIA ", categoria);
            parametros.Add("USUARIO_CREACION ", usuario_Creacion);
            parametros.Add("FECHA_DESDE ", fechaDesde);
            parametros.Add("FECHA_HASTA ", fechaHasta);



            var rp = BDOpe.ComandoSelectALista<VBS_NombrePlantillas>(sql_punteroVBS.Conexion_LocalVBS, "VBS_GENERA_TURNOS_PORLINEAS", parametros);

            return rp.Exitoso ? ResultadoOperacion<List<VBS_NombrePlantillas>>.CrearResultadoExitoso(rp.Resultado) : ResultadoOperacion<List<VBS_NombrePlantillas>>.CrearFalla(rp.MensajeProblema);

        }
        #endregion

        #region "Lista Calendario"

        public ResultadoOperacion<List<Calendario_Turnos_Lineas>> GetCalendarEventsExpoLineas(int anio, int mes)
        {
            parametros.Clear();
            parametros.Add("anio", anio);
            parametros.Add("mes", mes);
            var rp = BDOpe.ComandoSelectALista<Calendario_Turnos_Lineas>(sql_punteroVBS.Conexion_LocalVBS, "VBS_CONSULTAR_TURNOS_CALENDARIO_LINEAS", parametros);

            return rp.Exitoso ? ResultadoOperacion<List<Calendario_Turnos_Lineas>>.CrearResultadoExitoso(rp.Resultado) : ResultadoOperacion<List<Calendario_Turnos_Lineas>>.CrearFalla(rp.MensajeProblema);

        }

        public ResultadoOperacion<List<VBS_TurnosDetalleLineas>> GetTablaLineastPorDia(string fechaHasta)
        {
            parametros.Clear();
            parametros.Add("fechaStringDesde", fechaHasta);
            var rp = BDOpe.ComandoSelectALista<VBS_TurnosDetalleLineas>(sql_punteroVBS.Conexion_LocalVBS, "VBS_CONSULTAR_TABLA_LINEAS_DETALLE_DIA", parametros);

            return rp.Exitoso ? ResultadoOperacion<List<VBS_TurnosDetalleLineas>>.CrearResultadoExitoso(rp.Resultado) : ResultadoOperacion<List<VBS_TurnosDetalleLineas>>.CrearFalla(rp.MensajeProblema);

        }

        public ResultadoOperacion<List<VBS_TurnosDetalleLineas>> GetListaTurnosLineasPorFechas(string IdLinea, string fechaDesde, string fechaHasta)
        {
            parametros.Clear();
            parametros.Add("IdLinea", IdLinea);
            parametros.Add("fechaStringDesde", fechaDesde);
            parametros.Add("fechaStringHasta", fechaHasta);
            var rp = BDOpe.ComandoSelectALista<VBS_TurnosDetalleLineas>(sql_punteroVBS.Conexion_LocalVBS, "VBS_CONSULTAR_TURNOS_DETALLE_LINEAS", parametros);

            return rp.Exitoso ? ResultadoOperacion<List<VBS_TurnosDetalleLineas>>.CrearResultadoExitoso(rp.Resultado) : ResultadoOperacion<List<VBS_TurnosDetalleLineas>>.CrearFalla(rp.MensajeProblema);

        }

        public ResultadoOperacion<List<VBS_TurnosDetalleLineas>> GetListaTurnosPorDiaLineas(string fechaHasta)
        {
            parametros.Clear();
            parametros.Add("fechaStringHasta", fechaHasta);
            var rp = BDOpe.ComandoSelectALista<VBS_TurnosDetalleLineas>(sql_punteroVBS.Conexion_LocalVBS, "VBS_CONSULTAR_TURNOS_DIA_LINEAS", parametros);

            return rp.Exitoso ? ResultadoOperacion<List<VBS_TurnosDetalleLineas>>.CrearResultadoExitoso(rp.Resultado) : ResultadoOperacion<List<VBS_TurnosDetalleLineas>>.CrearFalla(rp.MensajeProblema);

        }

        public ResultadoOperacion<List<VBS_ReporteTurnosLineas>> GetListaReporteTurnosLineasPorFechas(string IdLinea, DateTime fechaDesde, DateTime fechaHasta)
        {
            parametros.Clear();
            parametros.Add("IdLinea", IdLinea);
            parametros.Add("fechaStringDesde", fechaDesde);
            parametros.Add("fechaStringHasta", fechaHasta);
            var rp = BDOpe.ComandoSelectALista<VBS_ReporteTurnosLineas>(sql_punteroVBS.Conexion_LocalVBS, "VBS_CONSULTAR_TURNOS_PORFECHAS_LINEAS", parametros);

            return rp.Exitoso ? ResultadoOperacion<List<VBS_ReporteTurnosLineas>>.CrearResultadoExitoso(rp.Resultado) : ResultadoOperacion<List<VBS_ReporteTurnosLineas>>.CrearFalla(rp.MensajeProblema);

        }

        #endregion

        #region "actualizar turnos de expo por lineas"
        public Int64? SaveTransactionUPDATE_LINEAS(out string OnError)
        {
            Int64 ID = 0;
            try
            {
                // Grabar transacción.
                string error;
                var id = SaveUPDATE_LINEAS(out error);
                if (id == null)
                {
                    OnError = "*** Error: al grabar turnos por líneas****";
                    return null;
                }
                ID = id.Value;
                OnError = string.Empty;
                return ID;
            }
            catch (Exception ex)
            {
                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>("SQL", nameof(SaveTransactionUPDATE_LINEAS), "SaveTransactionUPDATE_LINEAS", false, null, null, ex.StackTrace, ex);
                OnError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                return null;
            }
        }

        private Int64? SaveUPDATE_LINEAS(out string OnError)
        {
            parametros.Clear();
            parametros.Add("xmlTurnos", this.xmlTurnos);

            int rowCount;
            var db = sql_punteroVBS.ExecuteInsertUpdateTurnos(sql_punteroVBS.Conexion_LocalVBS, 8000, "VBS_UPDATE_TURNOS_CANTIDAD_LINEAS", parametros, out rowCount, out OnError);
            if (db == null)
            {
                OnError = string.Empty;
                return null;
            }
            return db;
        }

        #endregion

        #region "Inactivar horas expo vacios"
        public ResultadoOperacion<List<VBS_NombrePlantillas>> EditarHorasExpoVacios(DateTime fechaDesde_, TimeSpan horaDesde, DateTime fechaHasta_, TimeSpan horaHasta, string IdLinea)
        {
            try
            {

                var fechasda = fechaDesde_.ToString();
                var fechasda_ = fechaHasta_.ToString();


                parametros.Clear();
                parametros.Add("FECHA_DESDE", fechaDesde_.ToString("dd/MM/yyyy"));
                parametros.Add("FECHA_HASTA", fechaHasta_.ToString("dd/MM/yyyy"));
                parametros.Add("HORA_DESDE", horaDesde);
                parametros.Add("HORA_HASTA", horaHasta);
                parametros.Add("IdLinea", IdLinea);


                var rp = BDOpe.ComandoSelectALista<VBS_NombrePlantillas>(sql_punteroVBS.Conexion_LocalVBS, "VBS_EDITA_TURNOS_EXPO_LINEAS", parametros);

                return rp.Exitoso ? ResultadoOperacion<List<VBS_NombrePlantillas>>.CrearResultadoExitoso(rp.Resultado) : ResultadoOperacion<List<VBS_NombrePlantillas>>.CrearFalla(rp.MensajeProblema);

            }
            catch (Exception ex)
            {
                throw new Exception("ha ocurrido un error", ex);
            }

        }

        #endregion

        #region "Duplicar turnos vacios"

        public ResultadoOperacion<List<VBS_NombrePlantillas>> DuplicarHorasExpoVacios(DateTime fechaDesde_, TimeSpan horaDesde, DateTime fechaHasta_, TimeSpan horaHasta, string IdLinea, string USUARIO)
        {
            try
            {

                var fechasda = fechaDesde_.ToString();
                var fechasda_ = fechaHasta_.ToString();


                parametros.Clear();
                parametros.Add("FECHA_DESDE", fechaDesde_.ToString("dd/MM/yyyy"));
                parametros.Add("FECHA_HASTA", fechaHasta_.ToString("dd/MM/yyyy"));
                parametros.Add("HORA_DESDE", horaDesde);
                parametros.Add("HORA_HASTA", horaHasta);
                parametros.Add("IdLinea", IdLinea);
                parametros.Add("USUARIO", USUARIO);

                var rp = BDOpe.ComandoSelectALista<VBS_NombrePlantillas>(sql_punteroVBS.Conexion_LocalVBS, "VBS_COPIAR_TURNOS_EXPO_LINEAS", parametros);

                return rp.Exitoso ? ResultadoOperacion<List<VBS_NombrePlantillas>>.CrearResultadoExitoso(rp.Resultado) : ResultadoOperacion<List<VBS_NombrePlantillas>>.CrearFalla(rp.MensajeProblema);

            }
            catch (Exception ex)
            {
                throw new Exception("ha ocurrido un error", ex);
            }

        }
        #endregion

        #endregion


        #region "TURNO CONSOLIDACION/ACOPIO"
        public ResultadoOperacion<List<Calendario_Turnos_Consolidacion>> GetListaCalendarioConsolidacionAcopio(int anio, int mes)
        {
            parametros.Clear();
            parametros.Add("anio", anio);
            parametros.Add("mes", mes);
            var rp = BDOpe.ComandoSelectALista<Calendario_Turnos_Consolidacion>(sql_punteroVBS.Conexion_LocalVBS, "[VBS_TURNOS_CONSOLIDACION_ACOPIO_CALENDARIO_CONSULTAR_TURNOS]", parametros);
            return rp.Exitoso ? ResultadoOperacion<List<Calendario_Turnos_Consolidacion>>.CrearResultadoExitoso(rp.Resultado) : ResultadoOperacion<List<Calendario_Turnos_Consolidacion>>.CrearFalla(rp.MensajeProblema);
        }
        public ResultadoOperacion<List<VBS_NombrePlantillas>> GuardarTurnosConsolidacionAcopio(string bloqueId, string tipoBloque, int frecuencia, string usuario_Creacion, DateTime fechaDesde, DateTime fechaHasta)
        {
            try
            {
                parametros.Clear();
                parametros.Add("i_IdBloque ", bloqueId);
                parametros.Add("i_CodigoBloque ", tipoBloque);
                parametros.Add("i_FRECUENCIA ", frecuencia);
                parametros.Add("i_USUARIO_CREACION ", usuario_Creacion);
                parametros.Add("i_FECHA_DESDE ", fechaDesde);
                parametros.Add("i_FECHA_HASTA ", fechaHasta);
                var rp = BDOpe.ComandoSelectALista<VBS_NombrePlantillas>(sql_punteroVBS.Conexion_LocalVBS, "[VBS_TURNOS_CONSOLIDACION_ACOPIO_GENERAR]", parametros);
                return rp.Exitoso ? ResultadoOperacion<List<VBS_NombrePlantillas>>.CrearResultadoExitoso(rp.Resultado) : ResultadoOperacion<List<VBS_NombrePlantillas>>.CrearFalla(rp.MensajeProblema);
            }
            catch (Exception ex)
            {
                throw new Exception("error", ex);
            }
        }

        public ResultadoOperacion<List<VBS_TurnosDetalle>> VerificarCombinacionTurnosConsolidacionAcopio(string tipoBloqueID, string CodigoBloque, string fechaInicial)
        {
            parametros.Clear();
            parametros.Add("tipoBloqueID ", tipoBloqueID);
            parametros.Add("CodigoBloque", CodigoBloque);
            parametros.Add("fechaInicial", fechaInicial);
            var rp = BDOpe.ComandoSelectALista<VBS_TurnosDetalle>(sql_punteroVBS.Conexion_LocalVBS, "[VBS_TURNOS_CONSOLIDACION_ACOPIO_VALIDA]", parametros);
            return rp.Exitoso ? ResultadoOperacion<List<VBS_TurnosDetalle>>.CrearResultadoExitoso(rp.Resultado) : ResultadoOperacion<List<VBS_TurnosDetalle>>.CrearFalla(rp.MensajeProblema);
        }

        public ResultadoOperacion<List<VBS_TurnosDetalle_Consolidacion>> GetTablaTurnoConsolidacionAcopioPorDia(string fechaHasta)
        {
            parametros.Clear();
            parametros.Add("fechaStringDesde", fechaHasta);
            var rp = BDOpe.ComandoSelectALista<VBS_TurnosDetalle_Consolidacion>(sql_punteroVBS.Conexion_LocalVBS, "[VBS_TURNOS_CONSOLIDACION_ACOPIO_CONSULTAR_DETALLE_DIA]", parametros);
            return rp.Exitoso ? ResultadoOperacion<List<VBS_TurnosDetalle_Consolidacion>>.CrearResultadoExitoso(rp.Resultado) : ResultadoOperacion<List<VBS_TurnosDetalle_Consolidacion>>.CrearFalla(rp.MensajeProblema);
        }

        public ResultadoOperacion<List<VBS_TurnosDetalle_Consolidacion>> GetListaTurnoConsolidacionAcopioPorFechas(string idDetalle, string fechaDesde, string fechaHasta)
        {
            parametros.Clear();
            parametros.Add("ID_BLOQUE", idDetalle);
            parametros.Add("fechaStringDesde", fechaDesde);
            parametros.Add("fechaStringHasta", fechaHasta);
            var rp = BDOpe.ComandoSelectALista<VBS_TurnosDetalle_Consolidacion>(sql_punteroVBS.Conexion_LocalVBS, "[VBS_TURNOS_CONSOLIDACION_ACOPIO_CONSULTAR_DETALLE]", parametros);
            return rp.Exitoso ? ResultadoOperacion<List<VBS_TurnosDetalle_Consolidacion>>.CrearResultadoExitoso(rp.Resultado) : ResultadoOperacion<List<VBS_TurnosDetalle_Consolidacion>>.CrearFalla(rp.MensajeProblema);
        }

        public ResultadoOperacion<List<VBS_TurnosDetalle_Consolidacion>> GetListaTurnosConsolidacionAcopioPorDia(string fechaHasta)
        {
            parametros.Clear();
            parametros.Add("fechaStringHasta", fechaHasta);
            var rp = BDOpe.ComandoSelectALista<VBS_TurnosDetalle_Consolidacion>(sql_punteroVBS.Conexion_LocalVBS, "[VBS_TURNOS_CONSOLIDACION_ACOPIO_CONSULTAR_TURNOS_DIA]", parametros);
            return rp.Exitoso ? ResultadoOperacion<List<VBS_TurnosDetalle_Consolidacion>>.CrearResultadoExitoso(rp.Resultado) : ResultadoOperacion<List<VBS_TurnosDetalle_Consolidacion>>.CrearFalla(rp.MensajeProblema);
        }

        public Int64? SaveTransactionAC(out string OnError)
        {
            Int64 ID = 0;
            try
            {
                // Grabar transacción.
                string error;
                var id = SaveCA(out error);
                if (id == null)
                {
                    OnError = "*** Error: al grabar turnos ****";
                    return null;
                }
                ID = id.Value;
                OnError = string.Empty;
                return ID;
            }
            catch (Exception ex)
            {
                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>("SQL", nameof(SaveTransactionUPDATE), "SaveTransaction", false, null, null, ex.StackTrace, ex);
                OnError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                return null;
            }
        }
        private Int64? SaveCA(out string OnError)
        {
            parametros.Clear();
            parametros.Add("xmlTurnos", this.xmlTurnos);

            int rowCount;
            var db = sql_punteroVBS.ExecuteInsertUpdateTurnos(sql_punteroVBS.Conexion_LocalVBS, 8000, "[VBS_TURNOS_CONSOLIDACION_ACOPIO_UPDATE_TURNOS_CANTIDAD]", parametros, out rowCount, out OnError);
            if (db == null)
            {
                OnError = string.Empty;
                return null;
            }
            return db;
        }
        #endregion

        #region "TURNO CONSOLIDACION CFS"

        public ResultadoOperacion<List<Calendario_Turnos_Consolidacion>> GetListaCalendarioConsolidacionCFS(int anio, int mes)
        {
            parametros.Clear();
            parametros.Add("anio", anio);
            parametros.Add("mes", mes);
            var rp = BDOpe.ComandoSelectALista<Calendario_Turnos_Consolidacion>(sql_punteroVBS.Conexion_LocalVBS, "[VBS_TURNOS_CONSOLIDACION_CFS_CALENDARIO_CONSULTAR_TURNOS]", parametros);
            return rp.Exitoso ? ResultadoOperacion<List<Calendario_Turnos_Consolidacion>>.CrearResultadoExitoso(rp.Resultado) : ResultadoOperacion<List<Calendario_Turnos_Consolidacion>>.CrearFalla(rp.MensajeProblema);
        }

        public ResultadoOperacion<List<VBS_TurnosDetalle_Consolidacion>> GetTablaConsolidacionCFSPorDia(string fechaHasta)
        {
            parametros.Clear();
            parametros.Add("fechaStringDesde", fechaHasta);
            var rp = BDOpe.ComandoSelectALista<VBS_TurnosDetalle_Consolidacion>(sql_punteroVBS.Conexion_LocalVBS, "[VBS_TURNOS_CONSOLIDACION_CFS_CONSULTAR_TABLA_DETALLE_DIA]", parametros);
            return rp.Exitoso ? ResultadoOperacion<List<VBS_TurnosDetalle_Consolidacion>>.CrearResultadoExitoso(rp.Resultado) : ResultadoOperacion<List<VBS_TurnosDetalle_Consolidacion>>.CrearFalla(rp.MensajeProblema);
        }

        public ResultadoOperacion<List<VBS_TurnosDetalle>> VerificarCombinacionTipoBloqueTurnosConsolidacionCFS(string tipoBloqueID, string CodigoBloque, string fechaInicial)
        {
            parametros.Clear();
            parametros.Add("tipoBloqueID ", tipoBloqueID);
            parametros.Add("CodigoBloque", CodigoBloque);
            parametros.Add("fechaInicial", fechaInicial);
            var rp = BDOpe.ComandoSelectALista<VBS_TurnosDetalle>(sql_punteroVBS.Conexion_LocalVBS, "[VBS_TURNOS_CONSOLIDACION_CFS_VALIDA]", parametros);
            return rp.Exitoso ? ResultadoOperacion<List<VBS_TurnosDetalle>>.CrearResultadoExitoso(rp.Resultado) : ResultadoOperacion<List<VBS_TurnosDetalle>>.CrearFalla(rp.MensajeProblema);
        }

        public ResultadoOperacion<List<VBS_NombrePlantillas>> GuardarTurnosConsolidacionCFS(string bloqueId, string tipoBloque, int frecuencia, string usuario_Creacion, DateTime fechaDesde, DateTime fechaHasta)
        {
            try
            {
                parametros.Clear();
                parametros.Add("IdBloque ", bloqueId);
                parametros.Add("CodigoBloque ", tipoBloque);
                parametros.Add("FRECUENCIA ", frecuencia);
                parametros.Add("USUARIO_CREACION ", usuario_Creacion);
                parametros.Add("FECHA_DESDE ", fechaDesde);
                parametros.Add("FECHA_HASTA ", fechaHasta);
                var rp = BDOpe.ComandoSelectALista<VBS_NombrePlantillas>(sql_punteroVBS.Conexion_LocalVBS, "[VBS_TURNOS_CONSOLIDACION_CFS_GENERAR]", parametros);
                return rp.Exitoso ? ResultadoOperacion<List<VBS_NombrePlantillas>>.CrearResultadoExitoso(rp.Resultado) : ResultadoOperacion<List<VBS_NombrePlantillas>>.CrearFalla(rp.MensajeProblema);
            }
            catch (Exception ex)
            {
                throw new Exception("error", ex);
            }
        }

        public ResultadoOperacion<List<VBS_TurnosDetalle_Import>> GetListaTurnosConsolidacionCFSPorFechas(string idDetalle, string fechaDesde, string fechaHasta)
        {
            parametros.Clear();
            parametros.Add("ID_BLOQUE", idDetalle);
            parametros.Add("fechaStringDesde", fechaDesde);
            parametros.Add("fechaStringHasta", fechaHasta);
            var rp = BDOpe.ComandoSelectALista<VBS_TurnosDetalle_Import>(sql_punteroVBS.Conexion_LocalVBS, "[VBS_TURNOS_CONSOLIDACION_CFS_CONSULTAR_DETALLE]", parametros);
            return rp.Exitoso ? ResultadoOperacion<List<VBS_TurnosDetalle_Import>>.CrearResultadoExitoso(rp.Resultado) : ResultadoOperacion<List<VBS_TurnosDetalle_Import>>.CrearFalla(rp.MensajeProblema);
        }

        public ResultadoOperacion<List<VBS_TurnosDetalle_Consolidacion>> GetListaTurnosConsolidacionCFSPorDia(string fechaHasta)
        {
            parametros.Clear();
            parametros.Add("fechaStringHasta", fechaHasta);
            var rp = BDOpe.ComandoSelectALista<VBS_TurnosDetalle_Consolidacion>(sql_punteroVBS.Conexion_LocalVBS, "[VBS_TURNOS_CONSOLIDACION_CFS_CONSULTAR_TURNOS_DIA]", parametros);

            return rp.Exitoso ? ResultadoOperacion<List<VBS_TurnosDetalle_Consolidacion>>.CrearResultadoExitoso(rp.Resultado) : ResultadoOperacion<List<VBS_TurnosDetalle_Consolidacion>>.CrearFalla(rp.MensajeProblema);

        }

        public Int64? SaveTransactionCCFS(out string OnError)
        {
            Int64 ID = 0;
            try
            {
                // Grabar transacción.
                string error;
                var id = SaveCCFS(out error);
                if (id == null)
                {
                    OnError = "*** Error: al grabar turnos ****";
                    return null;
                }
                ID = id.Value;
                OnError = string.Empty;
                return ID;
            }
            catch (Exception ex)
            {
                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>("SQL", nameof(SaveTransactionUPDATE), "SaveTransaction", false, null, null, ex.StackTrace, ex);
                OnError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                return null;
            }
        }
        private Int64? SaveCCFS(out string OnError)
        {
            parametros.Clear();
            parametros.Add("xmlTurnos", this.xmlTurnos);

            int rowCount;
            var db = sql_punteroVBS.ExecuteInsertUpdateTurnos(sql_punteroVBS.Conexion_LocalVBS, 8000, "[VBS_TURNOS_CONSOLIDACION_CFS_UPDATE_TURNOS_CANTIDAD]", parametros, out rowCount, out OnError);
            if (db == null)
            {
                OnError = string.Empty;
                return null;
            }
            return db;
        }

        #endregion

        #region "CARGA EXPO BRBK"
        public ResultadoOperacion<List<Calendario_Turnos_carga_BRBK>> GetCalendarEventsCargaExpoBRBK(int anio, int mes)
        {
            parametros.Clear();
            parametros.Add("anio", anio);
            parametros.Add("mes", mes);
            var rp = BDOpe.ComandoSelectALista<Calendario_Turnos_carga_BRBK>(sql_punteroVBS.Conexion_LocalVBS, "[VBS_TURNOS_CARGA_EXPO_BRBK_CALENDARIO_CONSULTAR_TURNOS]", parametros);

            return rp.Exitoso ? ResultadoOperacion<List<Calendario_Turnos_carga_BRBK>>.CrearResultadoExitoso(rp.Resultado) : ResultadoOperacion<List<Calendario_Turnos_carga_BRBK>>.CrearFalla(rp.MensajeProblema);

        }

        public ResultadoOperacion<List<VBS_TurnosDetalleCargaBRBK>> GetTablaCargaExpoBRBKPorDia(string fechaHasta)
        {
            parametros.Clear();
            parametros.Add("fechaStringDesde", fechaHasta);
            var rp = BDOpe.ComandoSelectALista<VBS_TurnosDetalleCargaBRBK>(sql_punteroVBS.Conexion_LocalVBS, "[VBS_TURNOS_CARGA_EXPO_BRBK_CONSULTAR_TABLA_DETALLE_DIA]", parametros);

            return rp.Exitoso ? ResultadoOperacion<List<VBS_TurnosDetalleCargaBRBK>>.CrearResultadoExitoso(rp.Resultado) : ResultadoOperacion<List<VBS_TurnosDetalleCargaBRBK>>.CrearFalla(rp.MensajeProblema);

        }
        public ResultadoOperacion<List<VBS_TurnosDetalleLineas>> VerificarCombinacionCargaBRBK(string IdLinea, string fechaInicial)
        {
            parametros.Clear();
            parametros.Add("IdBodega ", IdLinea);
            parametros.Add("fechaInicial", fechaInicial);
            var rp = BDOpe.ComandoSelectALista<VBS_TurnosDetalleLineas>(sql_punteroVBS.Conexion_LocalVBS, "VBS_TURNOS_CARGA_EXPO_BRBK_GENERAR_VALIDA", parametros);

            return rp.Exitoso ? ResultadoOperacion<List<VBS_TurnosDetalleLineas>>.CrearResultadoExitoso(rp.Resultado) : ResultadoOperacion<List<VBS_TurnosDetalleLineas>>.CrearFalla(rp.MensajeProblema);

        }

        public ResultadoOperacion<List<VBS_NombrePlantillas>> GuardarTurnosCargaExpoBRBK(string IdBodega, string Bodega, int cantidad, string usuario_Creacion, DateTime fechaDesde, DateTime fechaHasta)
        {
            parametros.Clear();
            parametros.Add("ID_BODEGA ", IdBodega);
            parametros.Add("BODEGA ", Bodega);
            parametros.Add("CANTIDAD ", cantidad);
            parametros.Add("USUARIO_CREACION ", usuario_Creacion);
            parametros.Add("FECHA_DESDE ", fechaDesde);
            parametros.Add("FECHA_HASTA ", fechaHasta);
            var rp = BDOpe.ComandoSelectALista<VBS_NombrePlantillas>(sql_punteroVBS.Conexion_LocalVBS, "[VBS_TURNOS_CARGA_EXPO_BRBK_GENERAR]", parametros);
            return rp.Exitoso ? ResultadoOperacion<List<VBS_NombrePlantillas>>.CrearResultadoExitoso(rp.Resultado) : ResultadoOperacion<List<VBS_NombrePlantillas>>.CrearFalla(rp.MensajeProblema);
        }

        public ResultadoOperacion<List<VBS_TurnosDetalleCargaBRBK>> GetListaTurnosCargaExpoBRBKPorFechas(string IdBodega, string fechaDesde, string fechaHasta)
        {
            parametros.Clear();
            parametros.Add("IdLinea", IdBodega);
            parametros.Add("fechaStringDesde", fechaDesde);
            parametros.Add("fechaStringHasta", fechaHasta);
            var rp = BDOpe.ComandoSelectALista<VBS_TurnosDetalleCargaBRBK>(sql_punteroVBS.Conexion_LocalVBS, "[VBS_TURNOS_CARGA_EXPO_BRBK_CONSULTAR_DETALLE]", parametros);

            return rp.Exitoso ? ResultadoOperacion<List<VBS_TurnosDetalleCargaBRBK>>.CrearResultadoExitoso(rp.Resultado) : ResultadoOperacion<List<VBS_TurnosDetalleCargaBRBK>>.CrearFalla(rp.MensajeProblema);

        }

        public ResultadoOperacion<List<VBS_TurnosDetalleCargaBRBK>> GetListaTurnosCargaExpoBRBKPorDia(string fechaHasta)
        {
            parametros.Clear();
            parametros.Add("fechaStringHasta", fechaHasta);
            var rp = BDOpe.ComandoSelectALista<VBS_TurnosDetalleCargaBRBK>(sql_punteroVBS.Conexion_LocalVBS, "[VBS_TURNOS_CARGA_EXPO_BRBK_CONSULTAR_TURNOS_DIA]", parametros);

            return rp.Exitoso ? ResultadoOperacion<List<VBS_TurnosDetalleCargaBRBK>>.CrearResultadoExitoso(rp.Resultado) : ResultadoOperacion<List<VBS_TurnosDetalleCargaBRBK>>.CrearFalla(rp.MensajeProblema);

        }
        public Int64? SaveTransactionCEXPOBRBK(out string OnError)
        {
            Int64 ID = 0;
            try
            {
                // Grabar transacción.
                string error;
                var id = SaveCEXPOBRBK(out error);
                if (id == null)
                {
                    OnError = "*** Error: al grabar turnos por líneas****";
                    return null;
                }
                ID = id.Value;
                OnError = string.Empty;
                return ID;
            }
            catch (Exception ex)
            {
                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>("SQL", nameof(SaveTransactionUPDATE_LINEAS), "SaveTransactionCEXPOBRBK", false, null, null, ex.StackTrace, ex);
                OnError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));
                return null;
            }
        }

        private Int64? SaveCEXPOBRBK(out string OnError)
        {
            parametros.Clear();
            parametros.Add("xmlTurnos", this.xmlTurnos);

            int rowCount;
            var db = sql_punteroVBS.ExecuteInsertUpdateTurnos(sql_punteroVBS.Conexion_LocalVBS, 8000, "[VBS_TURNOS_CARGA_EXPO_BRBK_UPDATE_TURNOS_CANTIDAD]", parametros, out rowCount, out OnError);
            if (db == null)
            {
                OnError = string.Empty;
                return null;
            }
            return db;
        }
        #endregion
    }
}
