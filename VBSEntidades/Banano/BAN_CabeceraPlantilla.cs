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

namespace VBSEntidades.Banano
{
    public class BAN_CabeceraPlantilla : Cls_Bil_Base
    {
        #region "Variables"
        private static Int64? lm = -3;
        #endregion

        #region "Propiedades"
        public BAN_TurnosCab TurnoCab { get; set; }
        public long SECUENCIA { get; set; }
        public DateTime FECHA_CREACION { get; set; }
        public DateTime VIGENCIA_INICIAL { get; set; }
        public DateTime VIGENCIA_FINAL { get; set; }
        public bool ESTADO { get; set; }
        public string NOMBRE_PLANTILLA { get; set; }
        public string USUARIO_CREACION { get; set; }
        public string USUARIO_MODIFICACION { get; set; }
        public string xmlTurnos { get; set; }
        public long ID_CABECERA { get; set; }
        public List<BAN_DetallePlantilla> DETALLE_PLANTILLA { get; set; }
        #endregion

        public BAN_CabeceraPlantilla()
        {
            init();
            this.DETALLE_PLANTILLA = new List<BAN_DetallePlantilla>();
        }

        public BAN_CabeceraPlantilla(Int64 _SECUENCIA, DateTime _FECHA_CREACION, bool _ESTADO, DateTime _VIGENCIA_INICIAL, DateTime _VIGENCIA_FINAL, string _USUARIO_CREACION, string _USUARIO_MODIFICACION, string _NOMBRE_PLANTILLA, long _ID_CABECERA         )
        {
            this.ID_CABECERA = _ID_CABECERA;
            this.SECUENCIA = _SECUENCIA;
            this.ESTADO = _ESTADO;
            this.USUARIO_CREACION = _USUARIO_CREACION;
            this.USUARIO_MODIFICACION = _USUARIO_MODIFICACION;
            this.FECHA_CREACION = _FECHA_CREACION;
            this.VIGENCIA_INICIAL = _VIGENCIA_INICIAL;
            this.VIGENCIA_FINAL = _VIGENCIA_FINAL;
            this.DETALLE_PLANTILLA = new List<BAN_DetallePlantilla>();
        }

        public ResultadoOperacion<List<BAN_NombrePlantillas>> GetNombrePlantillas(string usuario, string pista)
        {
            parametros.Clear();
            parametros.Add("NOMBRE_PLANTILLA ", pista);
            var rp = BDOpe.ComandoSelectALista<BAN_NombrePlantillas>(sql_punteroVBS.Conexion_LocalVBS, "BAN_CONSULTAR_PLANTILLA", parametros);
            return rp.Exitoso ? ResultadoOperacion<List<BAN_NombrePlantillas>>.CrearResultadoExitoso(rp.Resultado) : ResultadoOperacion<List<BAN_NombrePlantillas>>.CrearFalla(rp.MensajeProblema);
        }

        public string GetParametrosValida(string pista)
        {
            parametros.Clear();
            parametros.Add("NOMBRE_PARAMETRO ", pista);
            var rp = BDOpe.ComandoTransaccionString(sql_punteroVBS.Conexion_LocalVBS, "BAN_CONSULTAR_PARAMETRO", parametros);
            return rp.Resultado;
        }

        public string GetBloqueValida(string codigo)
        {
            parametros.Clear();
            parametros.Add("Codigo_Bloque ", codigo);
            var rp = BDOpe.ComandoTransaccionString(sql_punteroVBS.Conexion_LocalVBS, "BAN_CONSULTAR_BLOQUE", parametros);
            return rp.Resultado;
        }

        public string GetMaquinaValida(string codigo)
        {
            parametros.Clear();
            parametros.Add("Codigo_Maquina ", codigo);
            var rp = BDOpe.ComandoTransaccionString(sql_punteroVBS.Conexion_LocalVBS, "BAN_CONSULTAR_MAQUINA", parametros);
            return rp.Resultado;
        }

       

        public ResultadoOperacion<List<VBS_LISTA_PARAMETRO>> GetListaParametro()
        {
            parametros.Clear();
            parametros.Add("fechaActual", DateTime.Now);
            var rp = BDOpe.ComandoSelectALista<VBS_LISTA_PARAMETRO>(sql_punteroVBS.Conexion_LocalVBS, "BAN_CONSULTAR_LISTA_PARAMETRO", parametros);
            return rp.Exitoso ? ResultadoOperacion<List<VBS_LISTA_PARAMETRO>>.CrearResultadoExitoso(rp.Resultado) : ResultadoOperacion<List<VBS_LISTA_PARAMETRO>>.CrearFalla(rp.MensajeProblema);
        }

        public ResultadoOperacion<List<VBS_BLOQUE>> GetListaBloque()
        {
            try
            {
                parametros.Clear();
                parametros.Add("fechaActual", DateTime.Now);
                var rp = BDOpe.ComandoSelectALista<VBS_BLOQUE>(sql_punteroVBS.Conexion_LocalVBS, "BAN_CONSULTAR_LISTA_BLOQUE", parametros);
                return rp.Exitoso ? ResultadoOperacion<List<VBS_BLOQUE>>.CrearResultadoExitoso(rp.Resultado) : ResultadoOperacion<List<VBS_BLOQUE>>.CrearFalla(rp.MensajeProblema);
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
                var rp = BDOpe.ComandoSelectALista<VBS_MAQUINA>(sql_punteroVBS.Conexion_LocalVBS, "BAN_CONSULTAR_LISTA_MAQUINAS", parametros);
                return rp.Exitoso ? ResultadoOperacion<List<VBS_MAQUINA>>.CrearResultadoExitoso(rp.Resultado) : ResultadoOperacion<List<VBS_MAQUINA>>.CrearFalla(rp.MensajeProblema);
            }
            catch (Exception ex)
            {
                throw new Exception("error", ex);
            }
        }

        public ResultadoOperacion<List<VBS_TurnosDetalle_Import>> GetListaTurnosImportPorFechas(int idDetalle, string fechaDesde, string fechaHasta)
        {
            parametros.Clear();
            parametros.Add("ID_BLOQUE", idDetalle);
            parametros.Add("fechaStringDesde", fechaDesde);
            parametros.Add("fechaStringHasta", fechaHasta);
            var rp = BDOpe.ComandoSelectALista<VBS_TurnosDetalle_Import>(sql_punteroVBS.Conexion_LocalVBS, "BAN_CONSULTAR_TURNOS_DETALLE_IMPORT", parametros);
            return rp.Exitoso ? ResultadoOperacion<List<VBS_TurnosDetalle_Import>>.CrearResultadoExitoso(rp.Resultado) : ResultadoOperacion<List<VBS_TurnosDetalle_Import>>.CrearFalla(rp.MensajeProblema);
        }

        public ResultadoOperacion<List<VBS_TurnosDetalle_Import>> GetTablaImportPorDia(string fechaHasta)
        {
            parametros.Clear();
            parametros.Add("fechaStringDesde", fechaHasta);
            var rp = BDOpe.ComandoSelectALista<VBS_TurnosDetalle_Import>(sql_punteroVBS.Conexion_LocalVBS, "BAN_CONSULTAR_TABLA_IMPORT_DETALLE_DIA", parametros);
            return rp.Exitoso ? ResultadoOperacion<List<VBS_TurnosDetalle_Import>>.CrearResultadoExitoso(rp.Resultado) : ResultadoOperacion<List<VBS_TurnosDetalle_Import>>.CrearFalla(rp.MensajeProblema);
        }

        public ResultadoOperacion<List<VBS_TurnosDetalle_Import>> GetListaTurnosPorDiaImport(string fechaHasta)
        {
            parametros.Clear();
            parametros.Add("fechaStringHasta", fechaHasta);
            var rp = BDOpe.ComandoSelectALista<VBS_TurnosDetalle_Import>(sql_punteroVBS.Conexion_LocalVBS, "BAN_CONSULTAR_TURNOS_DIA", parametros);
            return rp.Exitoso ? ResultadoOperacion<List<VBS_TurnosDetalle_Import>>.CrearResultadoExitoso(rp.Resultado) : ResultadoOperacion<List<VBS_TurnosDetalle_Import>>.CrearFalla(rp.MensajeProblema);
        }

        public ResultadoOperacion<List<VBS_DETALLE_MONITOR>> GetListaTurnosExpoTBL1(string fechaDesde, string fechaHasta, int idTipoCarga)
        {
            parametros.Clear();
            parametros.Add("FechaDesde", fechaDesde);
            parametros.Add("FechaHasta", fechaHasta);
            parametros.Add("idTipoCarga", idTipoCarga);
            var rp = BDOpe.ComandoSelectALista<VBS_DETALLE_MONITOR>(sql_punteroVBS.Conexion_LocalVBS, "BAN_DETALLE_MONITOR", parametros);
            return rp.Exitoso ? ResultadoOperacion<List<VBS_DETALLE_MONITOR>>.CrearResultadoExitoso(rp.Resultado) : ResultadoOperacion<List<VBS_DETALLE_MONITOR>>.CrearFalla(rp.MensajeProblema);
        }

        public ResultadoOperacion<List<VBS_DETALLE_MONITOR2>> GetListaTurnosExpoTBL2(string fechaDesde, string fechaHasta, int idTipoCarga)
        {
            parametros.Clear();
            parametros.Add("FechaDesde", fechaDesde);
            parametros.Add("FechaHasta", fechaHasta);
            parametros.Add("idTipoCarga", idTipoCarga);
            var rp = BDOpe.ComandoSelectALista<VBS_DETALLE_MONITOR2>(sql_punteroVBS.Conexion_LocalVBS, "BAN_DETALLE_MONITOR2", parametros);
            return rp.Exitoso ? ResultadoOperacion<List<VBS_DETALLE_MONITOR2>>.CrearResultadoExitoso(rp.Resultado) : ResultadoOperacion<List<VBS_DETALLE_MONITOR2>>.CrearFalla(rp.MensajeProblema);
        }

        public ResultadoOperacion<List<VBS_DETALLE_MONITOR_IMPORT>> GetListaTurnosImportBL1(string fechaDesde, string fechaHasta, int idTipoCarga)
        {
            parametros.Clear();
            parametros.Add("FechaDesde", fechaDesde);
            parametros.Add("FechaHasta", fechaHasta);
            var rp = BDOpe.ComandoSelectALista<VBS_DETALLE_MONITOR_IMPORT>(sql_punteroVBS.Conexion_LocalVBS, "BAN_DETALLE_MONITOR_IMPORT", parametros);
            return rp.Exitoso ? ResultadoOperacion<List<VBS_DETALLE_MONITOR_IMPORT>>.CrearResultadoExitoso(rp.Resultado) : ResultadoOperacion<List<VBS_DETALLE_MONITOR_IMPORT>>.CrearFalla(rp.MensajeProblema);
        }

        public ResultadoOperacion<List<VBS_DETALLE_MONITOR2_IMPORT>> GetListaTurnosImportBL2(string fechaDesde, string fechaHasta, int idTipoCarga)
        {
            parametros.Clear();
            parametros.Add("FechaDesde", fechaDesde);
            parametros.Add("FechaHasta", fechaHasta);
            var rp = BDOpe.ComandoSelectALista<VBS_DETALLE_MONITOR2_IMPORT>(sql_punteroVBS.Conexion_LocalVBS, "BAN_DETALLE_MONITOR_IMPORT2", parametros);
            return rp.Exitoso ? ResultadoOperacion<List<VBS_DETALLE_MONITOR2_IMPORT>>.CrearResultadoExitoso(rp.Resultado) : ResultadoOperacion<List<VBS_DETALLE_MONITOR2_IMPORT>>.CrearFalla(rp.MensajeProblema);
        }

       

        public ResultadoOperacion<List<BAN_Calendario_Turnos>> GetListaCalendario(int anio, int mes, string referencia)
        {
            parametros.Clear();
            parametros.Add("anio", anio);
            parametros.Add("mes", mes);
            parametros.Add("referencia", referencia);
            var rp = BDOpe.ComandoSelectALista<BAN_Calendario_Turnos>(sql_punteroVBS.Conexion_LocalVBS, "BAN_CONSULTAR_TURNOS_CALENDARIO", parametros);
            return rp.Exitoso ? ResultadoOperacion<List<BAN_Calendario_Turnos>>.CrearResultadoExitoso(rp.Resultado) : ResultadoOperacion<List<BAN_Calendario_Turnos>>.CrearFalla(rp.MensajeProblema);
        }

        public ResultadoOperacion<List<Calendario_Turnos_Import>> GetListaCalendarioImport(int anio, int mes)
        {
            parametros.Clear();
            parametros.Add("anio", anio);
            parametros.Add("mes", mes);
            var rp = BDOpe.ComandoSelectALista<Calendario_Turnos_Import>(sql_punteroVBS.Conexion_LocalVBS, "BAN_CONSULTAR_TURNOS_CALENDARIO_IMPORT", parametros);
            return rp.Exitoso ? ResultadoOperacion<List<Calendario_Turnos_Import>>.CrearResultadoExitoso(rp.Resultado) : ResultadoOperacion<List<Calendario_Turnos_Import>>.CrearFalla(rp.MensajeProblema);
        }

        public ResultadoOperacion<List<VBS_LISTA_PARAMETRO>> GuardarParametro(VBS_LISTA_PARAMETRO obj)
        {
            parametros.Clear();
            parametros.Add("Tipo ", obj.Tipo);
            parametros.Add("Parametro ", obj.Parametro);
            parametros.Add("Descripcion ", obj.Descripcion);
            parametros.Add("Valor ", obj.Valor);
            var rp = BDOpe.ComandoSelectALista<VBS_LISTA_PARAMETRO>(sql_punteroVBS.Conexion_LocalVBS, "BAN_INSERT_PARAMETRO", parametros);
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
            var rp = BDOpe.ComandoSelectALista<VBS_BLOQUE>(sql_punteroVBS.Conexion_LocalVBS, "BAN_INSERT_BLOQUE", parametros);
            return rp.Exitoso ? ResultadoOperacion<List<VBS_BLOQUE>>.CrearResultadoExitoso(rp.Resultado) : ResultadoOperacion<List<VBS_BLOQUE>>.CrearFalla(rp.MensajeProblema);
        }

        public ResultadoOperacion<List<VBS_MAQUINA>> GuardarMaquina(VBS_MAQUINA obj)
        {
            parametros.Clear();
            parametros.Add("Codigo ", obj.Codigo);
            parametros.Add("Tipo ", obj.Tipo);
            parametros.Add("CapacidadOperativa ", obj.CapacidadOperativa);
            parametros.Add("Estado ", obj.Estado);
            parametros.Add("USUARIO_REGISTRO ", obj.UsuarioRegistro);
            var rp = BDOpe.ComandoSelectALista<VBS_MAQUINA>(sql_punteroVBS.Conexion_LocalVBS, "BAN_INSERT_MAQUINA", parametros);
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
            var rp = BDOpe.ComandoSelectALista<VBS_BLOQUE>(sql_punteroVBS.Conexion_LocalVBS, "BAN_EDIT_BLOQUE", parametros);
            return rp.Exitoso ? ResultadoOperacion<List<VBS_BLOQUE>>.CrearResultadoExitoso(rp.Resultado) : ResultadoOperacion<List<VBS_BLOQUE>>.CrearFalla(rp.MensajeProblema);
        }

        public ResultadoOperacion<List<BAN_NombrePlantillas>> EditarHorasImport(DateTime fechaDesde_, TimeSpan horaDesde, DateTime fechaHasta_, TimeSpan horaHasta, int idTipoBloque)
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
                var rp = BDOpe.ComandoSelectALista<BAN_NombrePlantillas>(sql_punteroVBS.Conexion_LocalVBS, "BAN_EDITA_TURNOS_IMPO", parametros);
                return rp.Exitoso ? ResultadoOperacion<List<BAN_NombrePlantillas>>.CrearResultadoExitoso(rp.Resultado) : ResultadoOperacion<List<BAN_NombrePlantillas>>.CrearFalla(rp.MensajeProblema);
            }
            catch (Exception ex)
            {
                throw new Exception("ha ocurrido un error", ex);
            }
        }

        public ResultadoOperacion<List<BAN_NombrePlantillas>> EditarHorasExpo(DateTime fechaDesde_, TimeSpan horaDesde, DateTime fechaHasta_, TimeSpan horaHasta, int idTipoCargas)
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
                var rp = BDOpe.ComandoSelectALista<BAN_NombrePlantillas>(sql_punteroVBS.Conexion_LocalVBS, "BAN_EDITA_TURNOS_EXPO", parametros);
                return rp.Exitoso ? ResultadoOperacion<List<BAN_NombrePlantillas>>.CrearResultadoExitoso(rp.Resultado) : ResultadoOperacion<List<BAN_NombrePlantillas>>.CrearFalla(rp.MensajeProblema);
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
            var rp = BDOpe.ComandoSelectALista<VBS_MAQUINA>(sql_punteroVBS.Conexion_LocalVBS, "BAN_EDIT_MAQUINA", parametros);
            return rp.Exitoso ? ResultadoOperacion<List<VBS_MAQUINA>>.CrearResultadoExitoso(rp.Resultado) : ResultadoOperacion<List<VBS_MAQUINA>>.CrearFalla(rp.MensajeProblema);
        }

        public ResultadoOperacion<List<BAN_NombrePlantillas>> BuscarIdExiste(int idCabecera)
        {
            parametros.Clear();
            parametros.Add("IDCABECERA_PLANTILLA ", idCabecera);
            var rp = BDOpe.ComandoSelectALista<BAN_NombrePlantillas>(sql_punteroVBS.Conexion_LocalVBS, "BAN_CONSULTAR_ID_CABECERA_EXISTE", parametros);
            return rp.Exitoso ? ResultadoOperacion<List<BAN_NombrePlantillas>>.CrearResultadoExitoso(rp.Resultado) : ResultadoOperacion<List<BAN_NombrePlantillas>>.CrearFalla(rp.MensajeProblema);
        }

        public ResultadoOperacion<List<BAN_NombrePlantillas>> EditarCantidad(int idTurno, int Cantidad, string usuarioModificacion)
        {
            parametros.Clear();
            parametros.Add("ID_TURNO ", idTurno);
            parametros.Add("CANTIDAD ", Cantidad);
            parametros.Add("USUARIO_MODIFICACION ", usuarioModificacion);
            var rp = BDOpe.ComandoSelectALista<BAN_NombrePlantillas>(sql_punteroVBS.Conexion_LocalVBS, "BAN_UPDATE_TURNOS_CANTIDAD", parametros);
            return rp.Exitoso ? ResultadoOperacion<List<BAN_NombrePlantillas>>.CrearResultadoExitoso(rp.Resultado) : ResultadoOperacion<List<BAN_NombrePlantillas>>.CrearFalla(rp.MensajeProblema);
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
            var rp = BDOpe.ComandoSelectALista<VBS_LISTA_PARAMETRO>(sql_punteroVBS.Conexion_LocalVBS, "BAN_UPDATE_PARAMETRO", parametros);
            return rp.Exitoso ? ResultadoOperacion<List<VBS_LISTA_PARAMETRO>>.CrearResultadoExitoso(rp.Resultado) : ResultadoOperacion<List<VBS_LISTA_PARAMETRO>>.CrearFalla(rp.MensajeProblema);
        }

        public ResultadoOperacion<List<BAN_NombrePlantillas>> EditarTurnoDisponibles(int idTurno,
            string usuarioModificacion, DateTime fechaModificacion, string aisv, string contenedor, string chofer, string placa)
        {
            parametros.Clear();
            parametros.Add("ID_TURNO ", idTurno);
            parametros.Add("USUARIO_MODIFICACION ", usuarioModificacion);
            parametros.Add("FECHA_MODIFICACION ", fechaModificacion);
            parametros.Add("AISV ", aisv);
            parametros.Add("CONTENEDOR ", contenedor);
            parametros.Add("CHOFER ", chofer);
            parametros.Add("PLACA ", placa);
            var rp = BDOpe.ComandoSelectALista<BAN_NombrePlantillas>(sql_punteroVBS.Conexion_LocalVBS, "BAN_UPDATE_TURNOS_DISPONIBLE", parametros);
            return rp.Exitoso ? ResultadoOperacion<List<BAN_NombrePlantillas>>.CrearResultadoExitoso(rp.Resultado) : ResultadoOperacion<List<BAN_NombrePlantillas>>.CrearFalla(rp.MensajeProblema);
        }

        public ResultadoOperacion<List<BAN_NombrePlantillas>> EditarTurnoCanceladoAISV(string aisv, int idTurno, string usuarioModificacion, DateTime fechaModificacion)
        {
            parametros.Clear();
            parametros.Add("AISV", aisv);
            parametros.Add("ID_TURNO ", idTurno);
            parametros.Add("USUARIO_MODIFICACION ", usuarioModificacion);
            parametros.Add("FECHA_MODIFICACION ", fechaModificacion);
            var rp = BDOpe.ComandoSelectALista<BAN_NombrePlantillas>(sql_punteroVBS.Conexion_LocalVBS, "BAN_UPDATE_TURNOS_CANCELADO", parametros);
            return rp.Exitoso ? ResultadoOperacion<List<BAN_NombrePlantillas>>.CrearResultadoExitoso(rp.Resultado) : ResultadoOperacion<List<BAN_NombrePlantillas>>.CrearFalla(rp.MensajeProblema);
        }

        public ResultadoOperacion<List<BAN_ConsultarPlantillaDetalle>> DetallePlantillas(string usuario, int IdCabecera)
        {
            parametros.Clear();
            parametros.Add("IdCabecera ", IdCabecera);
            var rp = BDOpe.ComandoSelectALista<BAN_ConsultarPlantillaDetalle>(sql_punteroVBS.Conexion_LocalVBS, "BAN_CONSULTAR_PLANTILLAS_DETALLE", parametros);
            return rp.Exitoso ? ResultadoOperacion<List<BAN_ConsultarPlantillaDetalle>>.CrearResultadoExitoso(rp.Resultado) : ResultadoOperacion<List<BAN_ConsultarPlantillaDetalle>>.CrearFalla(rp.MensajeProblema);
        }

        public static List<VBS_ConsultarTipoCargas> ConsultarTipoCargas(out string OnError)
        {
            parametros.Clear();
            return sql_punteroVBS.ExecuteSelectControl<VBS_ConsultarTipoCargas>(sql_punteroVBS.Conexion_LocalVBS, 8000, "BAN_CONSULTAR_TIPO_CARGAS", null, out OnError);
        }

        public Int64? SaveTransaction(out string OnError)
        {
            string OError;
            Int64 ID = 0;

            try
            {
                //grabar transaccion.
                using (var scope = new System.Transactions.TransactionScope())
                {
                    var id = this.Save(out OnError);
                    if (!id.HasValue)
                    {
                        OnError = "*** Error: al grabar turnos ****";
                        return 0;
                    }
                    ID = id.Value;
                    this.TurnoCab.idPlantillaCab = ID;
                    if (ID > 0)
                    {
                        BAN_TurnosCab oTurnoCab = new BAN_TurnosCab();
                        oTurnoCab = this.TurnoCab;
                        TurnoCab.id = TurnoCab.Save_Update(out OError);

                        if (OError != string.Empty)
                        {
                            throw new Exception(OError);
                        }
                        scope.Complete();
                    }

                }
                    
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
            var db = sql_punteroVBS.ExecuteInsertUpdateDeleteReturn(sql_punteroVBS.Conexion_LocalVBS, 8000, "BAN_GRABAR_PLANTILLAS", parametros, out OnError);
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
            var db = sql_punteroVBS.ExecuteInsertUpdateTurnos(sql_punteroVBS.Conexion_LocalVBS, 8000, "BAN_UPDATE_TURNOS_CANTIDAD", parametros, out rowCount, out OnError);
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
            var db = sql_punteroVBS.ExecuteInsertUpdateTurnos(sql_punteroVBS.Conexion_LocalVBS, 8000, "BAN_UPDATE_TURNOS_CANTIDAD_IMPORT", parametros, out rowCount, out OnError);
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
            var db = sql_punteroVBS.ExecuteInsertUpdateTurnos(sql_punteroVBS.Conexion_LocalVBS, 8000, "BAN_UPDATE_TURNOS_CANTIDAD_Vacios", parametros, out rowCount, out OnError);
            if (db == null)
            {
                OnError = string.Empty;
                return null;
            }
            return db;
        }

        public DataTable CabeceraExpoReporte(DateTime fechaDesde, int tipoCargaId)
        {
            parametros.Clear();
            parametros.Add("FechaDesde", fechaDesde);
            parametros.Add("FechaHasta", fechaDesde);
            parametros.Add("idTipoCarga", tipoCargaId);

            var rp = BDOpe.ComadoSelectADatatable(sql_punteroVBS.Conexion_LocalVBS, "BAN_CABECERA_REPORTE_EXPO", parametros);

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

            var rp = BDOpe.ComadoSelectADatatable(sql_punteroVBS.Conexion_LocalVBS, "BAN_CABECERA_REPORTE_IMPORT", parametros);

            if (rp.Exitoso && rp.Resultado != null)
            {
                return rp.Resultado;
            }
            else
            {
                throw new Exception(rp.MensajeProblema);
            }
        }
    }
}
