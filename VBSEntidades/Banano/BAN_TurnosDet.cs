using AccesoDatos;
using BillionEntidades;
using ServicesEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VBSEntidades.Banano
{
    public class BAN_TurnosDet : Cls_Bil_Base
    {

        #region "Variables"
       // private static Int64? lm = -3;
        #endregion

        #region "Propiedades"
        public BAN_TurnosCab TurnoCab { get; set; }
        public Int64 SECUENCIA { get; set; }
        public long IdTurno { get; set; }
        public long IdTurnoCab { get; set; }
        public long IdCabeceraPlantilla { get; set; }
        public long IdDetallePlantilla { get; set; }
        public BananoMuelle.BAN_HorarioInicial HorarioInicial { get; set; }
        public string idHoraInicio { get; set; }
        public string HoraInicio { get; set; }
        public BananoMuelle.BAN_HorarioFinal HorarioFinal { get; set; }
        public string idHoraFinal { get; set; }
        public string HoraFinal { get; set; }
        //public string HoraInicial { get; set; }
        //public int HoraInicialId { get; set; }
        public string Categoria { get; set; }
        public int Cantidad { get; set; }
        public string Estado { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public DateTime FechaModificacion { get; set; }
        public DateTime VigenciaInicial { get; set; }
        public DateTime VigenciaFinal { get; set; }
        
        public TimeSpan Horario { get; set; }
        //public string HorarioFinal { get; set; }
        public int Disponible { get; set; }
        public int Asignados { get; set; }
        //public int HorarioFinalId { get; set; }
        public int idLinea { get; set; }
        public string LineaNaviera { get; set; }
        public BAN_LineaNaviera Linea { get; set; }
        public List<BAN_TurnosCliente> SubDetalle { get; set; }
        #endregion

        public BAN_TurnosDet()
        {
            init();
        }

        public ResultadoOperacion<List<BAN_NombrePlantillas>> GuardarTurnos(long idCabecera, long idDetalle,
            string idHoraFinal, string HoraFinal, string idHoraInicial, string HoraInicial, int cantidad, string categoria, string usuario_Creacion, DateTime fechaDesde, DateTime fechaHasta, int idLinea, string lineaNaviera)
        {
            parametros.Clear();
            parametros.Add("IDCABECERA_PLANTILLA ", idCabecera);
            parametros.Add("IDDETALLE_PLANTILLA ", idDetalle);
            parametros.Add("ID_HORA_INI ", idHoraInicial);
            parametros.Add("HORA_INI ", HoraInicial);
            parametros.Add("ID_HORA_FIN ", idHoraFinal);
            parametros.Add("HORA_FINAL ", HoraFinal);
            parametros.Add("CANTIDAD ", cantidad);
            parametros.Add("CATEGORIA ", categoria);
            parametros.Add("USUARIO_CREACION ", usuario_Creacion);
            parametros.Add("FECHA_DESDE ", fechaDesde);
            parametros.Add("FECHA_HASTA ", fechaHasta);
            parametros.Add("IDLINEA ", idLinea);
            parametros.Add("LINEANAVIERA ", lineaNaviera);
            var rp = BDOpe.ComandoSelectALista<BAN_NombrePlantillas>(sql_punteroVBS.Conexion_LocalVBS, "BAN_GENERA_TURNOS", parametros);
            return rp.Exitoso ? ResultadoOperacion<List<BAN_NombrePlantillas>>.CrearResultadoExitoso(rp.Resultado) : ResultadoOperacion<List<BAN_NombrePlantillas>>.CrearFalla(rp.MensajeProblema);
        }

        public ResultadoOperacion<List<BAN_TurnosDet>> GetListaTurnosPorFechas(int idDetalle, string fechaDesde, string fechaHasta)
        {
            parametros.Clear();
            parametros.Add("ID_DETALLE_PLANTILLA", idDetalle);
            parametros.Add("fechaStringDesde", fechaDesde);
            parametros.Add("fechaStringHasta", fechaHasta);
            var rp = BDOpe.ComandoSelectALista<BAN_TurnosDet>(sql_punteroVBS.Conexion_LocalVBS, "BAN_CONSULTAR_TURNOS_DETALLE", parametros);
            return rp.Exitoso ? ResultadoOperacion<List<BAN_TurnosDet>>.CrearResultadoExitoso(rp.Resultado) : ResultadoOperacion<List<BAN_TurnosDet>>.CrearFalla(rp.MensajeProblema);
        }
        /*
        public ResultadoOperacion<List<BAN_TurnosDet>> GetListaTurnosPorDetalleId(string usuario, int idDetalleId, string fechaDesde, string fechaHasta)
        {
            parametros.Clear();
            parametros.Add("ID_DETALLE_PLANTILLA ", idDetalleId);
            parametros.Add("fechaStringDesde", fechaDesde);
            parametros.Add("fechaStringHasta", fechaHasta);
            var rp = BDOpe.ComandoSelectALista<BAN_TurnosDet>(sql_punteroVBS.Conexion_LocalVBS, "BAN_CONSULTAR_TURNOS_DETALLE", parametros);
            return rp.Exitoso ? ResultadoOperacion<List<BAN_TurnosDet>>.CrearResultadoExitoso(rp.Resultado) : ResultadoOperacion<List<BAN_TurnosDet>>.CrearFalla(rp.MensajeProblema);
        }*/

        public ResultadoOperacion<List<BAN_TurnosDet>> GetTablaExpoPorDia(string fechaHasta)
        {
            parametros.Clear();
            parametros.Add("fechaStringDesde", fechaHasta);
            var rp = BDOpe.ComandoSelectALista<BAN_TurnosDet>(sql_punteroVBS.Conexion_LocalVBS, "BAN_CONSULTAR_TABLA_DETALLE_DIA_MUELLE", parametros);
            return rp.Exitoso ? ResultadoOperacion<List<BAN_TurnosDet>>.CrearResultadoExitoso(rp.Resultado) : ResultadoOperacion<List<BAN_TurnosDet>>.CrearFalla(rp.MensajeProblema);
        }

        public ResultadoOperacion<List<BAN_TurnosDet>> GetListaTurnosPorDia(string fechaHasta)
        {
            parametros.Clear();
            parametros.Add("fechaStringHasta", fechaHasta);
            var rp = BDOpe.ComandoSelectALista<BAN_TurnosDet>(sql_punteroVBS.Conexion_LocalVBS, "BAN_CONSULTAR_TURNOS_DIA", parametros);
            return rp.Exitoso ? ResultadoOperacion<List<BAN_TurnosDet>>.CrearResultadoExitoso(rp.Resultado) : ResultadoOperacion<List<BAN_TurnosDet>>.CrearFalla(rp.MensajeProblema);
        }

        public ResultadoOperacion<List<BAN_TurnosDet>> VerificarCombinacionTipoCargaContenedor(string idHoraInicial, string idHoraFinal, string fechaInicial)
        {
            parametros.Clear();
            parametros.Add("idHoraInicial ", idHoraInicial);
            parametros.Add("idHoraFinal", idHoraFinal);
            parametros.Add("fechaInicial", fechaInicial);
            var rp = BDOpe.ComandoSelectALista<BAN_TurnosDet>(sql_punteroVBS.Conexion_LocalVBS, "BAN_VerificarCombinacionTipoCargaContenedor", parametros);
            return rp.Exitoso ? ResultadoOperacion<List<BAN_TurnosDet>>.CrearResultadoExitoso(rp.Resultado) : ResultadoOperacion<List<BAN_TurnosDet>>.CrearFalla(rp.MensajeProblema);
        }

        public ResultadoOperacion<List<BAN_TurnosDet>> VerificarCombinacionTipoBloque(int tipoBloqueID, string CodigoBloque, string fechaInicial)
        {
            parametros.Clear();
            parametros.Add("tipoBloqueID ", tipoBloqueID);
            parametros.Add("CodigoBloque", CodigoBloque);
            parametros.Add("fechaInicial", fechaInicial);
            var rp = BDOpe.ComandoSelectALista<BAN_TurnosDet>(sql_punteroVBS.Conexion_LocalVBS, "BAN_VerificarCombinacionTipoBloque", parametros);
            return rp.Exitoso ? ResultadoOperacion<List<BAN_TurnosDet>>.CrearResultadoExitoso(rp.Resultado) : ResultadoOperacion<List<BAN_TurnosDet>>.CrearFalla(rp.MensajeProblema);
        }

        public static List<BAN_TurnosDet> listadoTurnoDet(long _idTurnoCab, out string OnError)
        {
            OnError = string.Empty;
            //OnInit("N4Middleware");
            parametros.Clear();
            parametros.Add("i_idTurnoCab", _idTurnoCab);
            //return sql_puntero.ExecuteSelectControl<BAN_TurnosDet>(sql_punteroVBS.Conexion_LocalVBS, 4000, "BAN_CONSULTAR_TURNOSDET", parametros, out OnError);

            var obj = sql_puntero.ExecuteSelectControl<BAN_TurnosDet>(sql_punteroVBS.Conexion_LocalVBS, 4000, "BAN_CONSULTAR_TURNOSDET", parametros, out OnError);
            if (obj != null)
            {
                try
                {
                    //obj.Estados = estados.GetEstado(obj.estado);
                    foreach (BAN_TurnosDet oDet in obj)
                    {
                        oDet.Linea = BAN_LineaNaviera.GetLinea(long.Parse(oDet.idLinea.ToString()));
                    }
                }
                catch { OnError = ""; }

            }
            else
            {
                OnError = "";
            }

            return obj;
        }

        public static BAN_TurnosDet GetTurnoDet(long _idTurnoDet)
        {
            //OnInit("N4Middleware");
            parametros.Clear();
            parametros.Add("i_idTurnoDet", _idTurnoDet);
            var obj = sql_puntero.ExecuteSelectOnly<BAN_TurnosDet>(sql_punteroVBS.Conexion_LocalVBS, 4000, "BAN_CONSULTAR_TURNOSDET", parametros);
            try
            {
                string OnError;
                obj.Linea = BAN_LineaNaviera.GetLinea(long.Parse(obj.idLinea.ToString()));
                obj.TurnoCab = BAN_TurnosCab.GetTurnoCab(long.Parse(obj.IdTurnoCab.ToString()));
                obj.SubDetalle = BAN_TurnosCliente.listadoTurnosCliente(_idTurnoDet, out OnError);
                
                foreach (BAN_TurnosCliente oTurnoCliente in obj.SubDetalle)
                {
                    oTurnoCliente.TurnoDet = obj;
                }
            }
            catch { }

            return obj;
        }


        /*
        public ResultadoOperacion<List<BAN_TurnosDet>> GetListaTurnosPorDosDias(string fechaDesde, string fechaHasta)
        {
            parametros.Clear();
            parametros.Add("fechaStringDesde", fechaDesde);
            parametros.Add("fechaStringHasta", fechaHasta);
            var rp = BDOpe.ComandoSelectALista<BAN_TurnosDet>(sql_punteroVBS.Conexion_LocalVBS, "BAN_CONSULTAR_TURNOS_DOS_DIAS", parametros);
            return rp.Exitoso ? ResultadoOperacion<List<BAN_TurnosDet>>.CrearResultadoExitoso(rp.Resultado) : ResultadoOperacion<List<BAN_TurnosDet>>.CrearFalla(rp.MensajeProblema);
        }

        public ResultadoOperacion<List<BAN_TurnosDet>> GetListaTurnosPorDiaTipoContenedor(string fechaHasta, string iso, int tiempoEspera)
        {
            parametros.Clear();
            parametros.Add("fechaStringHasta", fechaHasta);
            parametros.Add("tipoContenedor", iso);
            parametros.Add("porcentajeCantidad ", tiempoEspera);
            var rp = BDOpe.ComandoSelectALista<BAN_TurnosDet>(sql_punteroVBS.Conexion_LocalVBS, "BAN_CONSULTAR_TURNOS_DIA_TIPOCONTENEDOR", parametros);
            return rp.Exitoso ? ResultadoOperacion<List<BAN_TurnosDet>>.CrearResultadoExitoso(rp.Resultado) : ResultadoOperacion<List<BAN_TurnosDet>>.CrearFalla(rp.MensajeProblema);
        }

        public ResultadoOperacion<List<BAN_TurnosDet>> GetListaTurnosPorDiaTipoContenedorALL(string fechaHasta, string iso, int tiempoEspera)
        {
            parametros.Clear();
            parametros.Add("fechaStringHasta", fechaHasta);
            parametros.Add("tipoContenedor", iso);
            parametros.Add("porcentajeCantidad ", tiempoEspera);
            var rp = BDOpe.ComandoSelectALista<BAN_TurnosDet>(sql_punteroVBS.Conexion_LocalVBS, "BAN_CONSULTAR_TURNOS_DIA_TIPOCONTENEDOR_ALL", parametros);
            return rp.Exitoso ? ResultadoOperacion<List<BAN_TurnosDet>>.CrearResultadoExitoso(rp.Resultado) : ResultadoOperacion<List<BAN_TurnosDet>>.CrearFalla(rp.MensajeProblema);
        }
        */

        #region "TURNOS BODEGA"
        public ResultadoOperacion<List<BAN_NombrePlantillas>> GuardarTurnosImport(string bloqueId, string tipoBloque, int frecuencia, string usuario_Creacion, DateTime fechaDesde, DateTime fechaHasta)
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
                var rp = BDOpe.ComandoSelectALista<BAN_NombrePlantillas>(sql_punteroVBS.Conexion_LocalVBS, "BAN_GENERA_TURNOS_IMPO", parametros);
                return rp.Exitoso ? ResultadoOperacion<List<BAN_NombrePlantillas>>.CrearResultadoExitoso(rp.Resultado) : ResultadoOperacion<List<BAN_NombrePlantillas>>.CrearFalla(rp.MensajeProblema);
            }
            catch (Exception ex)
            {
                throw new Exception("error", ex);
            }
        }
        #endregion
    }
}
