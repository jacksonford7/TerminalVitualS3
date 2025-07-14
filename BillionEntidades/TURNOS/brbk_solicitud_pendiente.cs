using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlConexion;

namespace BillionEntidades
{
    public class brbk_solicitud_pendiente : Cls_Bil_Base
    {

        #region "Variables"
        private static Int64? lm = -3;
        private Int64 _ID_SOL;
        private string _NUMERO_CARGA = string.Empty;
        private DateTime? _FECHA_SOL = null;
      
        private int _CANTIDAD_VEHI;
        private int _TOTAL_BUTLOS;

        private string _BODEGA = string.Empty;
        private string _TIPO_PRODUCTO = string.Empty;
        private string _AGENTE_DESC = string.Empty;
        private string _IMPORTADOR_DESC = string.Empty;
        private string _USUARIO_DESC = string.Empty;

        private int _APROBADOS;
        private int _PENDIENTES;
        private int _RECHAZADOS;


        private string _MRN = string.Empty;
        private string _MSN = string.Empty;
        private string _HSN = string.Empty;
        private int _ID_TIPO_PRODU;

        private Int64 _SECUENCIA;
        private DateTime? _FECHA_EXPIRACION = null;
        private int _CANTIDAD_CARGA;
        private int _CANTIDAD_VEHICULOS;
        private int _TOTAL_CARGA;
        private Int64 _ID_HORARIO;
        private string _HORARIO = string.Empty;
        private string _USUARIO_ESTADO = string.Empty;
        private Int64 _ID_TIPO_TURNO;
        private string _TIPO_TURNO = string.Empty;
        #endregion

        #region "Propiedades"

        public Int64 ID_SOL { get => _ID_SOL; set => _ID_SOL = value; }
        public string NUMERO_CARGA { get => _NUMERO_CARGA; set => _NUMERO_CARGA = value; }
        public DateTime? FECHA_SOL { get => _FECHA_SOL; set => _FECHA_SOL = value; }
      
        public int CANTIDAD_VEHI { get => _CANTIDAD_VEHI; set => _CANTIDAD_VEHI = value; }
        public int TOTAL_BUTLOS { get => _TOTAL_BUTLOS; set => _TOTAL_BUTLOS = value; }

        public string BODEGA { get => _BODEGA; set => _BODEGA = value; }
        public string TIPO_PRODUCTO { get => _TIPO_PRODUCTO; set => _TIPO_PRODUCTO = value; }

        public string AGENTE_DESC { get => _AGENTE_DESC; set => _AGENTE_DESC = value; }
        public string IMPORTADOR_DESC { get => _IMPORTADOR_DESC; set => _IMPORTADOR_DESC = value; }
        public string USUARIO_DESC { get => _USUARIO_DESC; set => _USUARIO_DESC = value; }

        public int APROBADOS { get => _APROBADOS; set => _APROBADOS = value; }
        public int PENDIENTES { get => _PENDIENTES; set => _PENDIENTES = value; }
        public int RECHAZADOS { get => _RECHAZADOS; set => _RECHAZADOS = value; }

        public string MRN { get => _MRN; set => _MRN = value; }
        public string MSN { get => _MSN; set => _MSN = value; }
        public string HSN { get => _HSN; set => _HSN = value; }
        public int ID_TIPO_PRODU { get => _ID_TIPO_PRODU; set => _ID_TIPO_PRODU = value; }


        public Int64 SECUENCIA { get => _SECUENCIA; set => _SECUENCIA = value; }
        public DateTime? FECHA_EXPIRACION { get => _FECHA_EXPIRACION; set => _FECHA_EXPIRACION = value; }
        public int CANTIDAD_CARGA { get => _CANTIDAD_CARGA; set => _CANTIDAD_CARGA = value; }
        public int CANTIDAD_VEHICULOS { get => _CANTIDAD_VEHICULOS; set => _CANTIDAD_VEHICULOS = value; }
        public int TOTAL_CARGA { get => _TOTAL_CARGA; set => _TOTAL_CARGA = value; }

        public Int64 ID_HORARIO { get => _ID_HORARIO; set => _ID_HORARIO = value; }
        public string HORARIO { get => _HORARIO; set => _HORARIO = value; }
        public string USUARIO_ESTADO { get => _USUARIO_ESTADO; set => _USUARIO_ESTADO = value; }

        public Int64 ID_TIPO_TURNO { get => _ID_TIPO_TURNO; set => _ID_TIPO_TURNO = value; }
        public string TIPO_TURNO { get => _TIPO_TURNO; set => _TIPO_TURNO = value; }


        public List<brbk_solicitud_pendiente_det> Detalle { get; set; }
        #endregion


        public brbk_solicitud_pendiente()
        {
            init();


            this.Detalle = new List<brbk_solicitud_pendiente_det>();
        }


     

        #region "Listados"
        public static List<brbk_solicitud_pendiente> Solicitud_Pendientes_Aprobar( out string OnError)
        {

         
            return sql_puntero.ExecuteSelectControl<brbk_solicitud_pendiente>(sql_puntero.Conexion_Local, 6000, "[Brbk].[LISTA_SOLICITUDES_PENDIENTES]", null, out OnError);

        }

        public static List<brbk_solicitud_pendiente> Solicitud_Pendientes_Todas(DateTime fechadesde, DateTime fechahasta, out string OnError)
        {
            parametros.Clear();
            parametros.Add("fecha_desde", fechadesde);
            parametros.Add("fecha_hasta", fechahasta);
            return sql_puntero.ExecuteSelectControl<brbk_solicitud_pendiente>(sql_puntero.Conexion_Local, 6000, "[Brbk].[LISTA_SOLICITUDES_TODAS]", parametros, out OnError);

        }

        #endregion

        #region "Listado de Solicitudes para emitir pase de puerta"
        public static List<brbk_solicitud_pendiente> Solicitud_Pendientes_EmitirPase(DateTime FECHA_DESDE, DateTime FECHA_HASTA, string RUC, out string OnError)
        {

            parametros.Clear();
            parametros.Add("FECHA_DESDE", FECHA_DESDE);
            parametros.Add("FECHA_HASTA", FECHA_HASTA);
            parametros.Add("RUC", RUC);
            return sql_puntero.ExecuteSelectControl<brbk_solicitud_pendiente>(sql_puntero.Conexion_Local, 6000, "[Brbk].[LISTA_SOLICITUDES_EMITIR_PASES]", parametros, out OnError);

        }
        #endregion


        #region "Poblar solicitud Pendientes de Aprobar"
        public bool PopulateMyData(out string OnError)
        {

            parametros.Clear();
            parametros.Add("ID_SOL", this.ID_SOL);

            var t = sql_puntero.ExecuteSelectOnly<brbk_solicitud_pendiente>(sql_puntero.Conexion_Local, 6000, "[Brbk].[CARGA_CABECERA_SOLICITUD]", parametros);
            if (t == null)
            {
                OnError = string.Format("No existe solicitud pendiente con el ID seleccionado: {0}", this.ID_SOL);
                return false;
            }

            this.ID_SOL = t.ID_SOL;
            this.MRN = t.MRN;
            this.MSN = t.MSN;
            this.HSN = t.HSN;
            this.FECHA_SOL = t.FECHA_SOL;
            this.CANTIDAD_VEHI = t.CANTIDAD_VEHI;
            this.TOTAL_BUTLOS = t.TOTAL_BUTLOS;
            this.BODEGA = t.BODEGA;
            this.TIPO_PRODUCTO = t.TIPO_PRODUCTO;
            this.ID_TIPO_PRODU = t.ID_TIPO_PRODU;
            this.USUARIO_DESC = t.USUARIO_DESC;
            this.AGENTE_DESC = t.AGENTE_DESC;
            this.IMPORTADOR_DESC = t.IMPORTADOR_DESC;

            this.ID_TIPO_TURNO = t.ID_TIPO_TURNO;
            this.TIPO_TURNO = t.TIPO_TURNO;

            this.Detalle = brbk_solicitud_pendiente_det.Detalle_Solicitud(this.ID_SOL, out OnError);

            if (!string.IsNullOrEmpty(OnError))
            {
                OnError = string.Format("Error al cargar detalle de solicitud..{0}", OnError);
                return false;
            }

            OnError = string.Empty;
            return true;
        }
        #endregion

        #region "Poblar solicitud Aprobadas para emitir pase"
        public bool PopulateMyData_Aprobados(out string OnError)
        {

            parametros.Clear();
            parametros.Add("ID_SOL", this.ID_SOL);

            var t = sql_puntero.ExecuteSelectOnly<brbk_solicitud_pendiente>(sql_puntero.Conexion_Local, 6000, "[Brbk].[CARGA_CABECERA_SOLICITUD]", parametros);
            if (t == null)
            {
                OnError = string.Format("No existe solicitud pendiente con el ID seleccionado: {0}", this.ID_SOL);
                return false;
            }

            this.ID_SOL = t.ID_SOL;
            this.MRN = t.MRN;
            this.MSN = t.MSN;
            this.HSN = t.HSN;
            this.FECHA_SOL = t.FECHA_SOL;
            this.CANTIDAD_VEHI = t.CANTIDAD_VEHI;
            this.TOTAL_BUTLOS = t.TOTAL_BUTLOS;
            this.BODEGA = t.BODEGA;
            this.TIPO_PRODUCTO = t.TIPO_PRODUCTO;
            this.ID_TIPO_PRODU = t.ID_TIPO_PRODU;
            this.USUARIO_DESC = t.USUARIO_DESC;
            this.AGENTE_DESC = t.AGENTE_DESC;
            this.IMPORTADOR_DESC = t.IMPORTADOR_DESC;
            this.ID_TIPO_TURNO = t.ID_TIPO_TURNO;
            this.TIPO_TURNO = t.TIPO_TURNO;
            this.Detalle = brbk_solicitud_pendiente_det.Detalle_Solicitud_Aprobada(this.ID_SOL, out OnError);

            if (!string.IsNullOrEmpty(OnError))
            {
                OnError = string.Format("Error al cargar detalle de solicitud..{0}", OnError);
                return false;
            }

            OnError = string.Empty;
            return true;
        }
        #endregion

        #region "Aprobar Turno"

        private Int64? Aprobar_Update(out string OnError)
        {

            parametros.Clear();
            parametros.Add("ID_SOL", this.ID_SOL);
            parametros.Add("SECUENCIA", this.SECUENCIA);
            parametros.Add("FECHA_EXPIRACION", this.FECHA_EXPIRACION);
            parametros.Add("CANTIDAD_CARGA", this.CANTIDAD_CARGA);
            parametros.Add("CANTIDAD_VEHICULOS", this.CANTIDAD_VEHICULOS);
            parametros.Add("TOTAL_CARGA", this.TOTAL_CARGA);
            parametros.Add("ID_HORARIO", this.ID_HORARIO);
            parametros.Add("HORARIO", this.HORARIO);
            parametros.Add("USUARIO_ESTADO", this.USUARIO_ESTADO);

            var db = sql_puntero.ExecuteInsertUpdateDeleteReturn(sql_puntero.Conexion_Local, 8000, "[Brbk].[APROBAR_TURNOS_SOLICITUD]", parametros, out OnError);
            if (!db.HasValue || db.Value < 0)
            {
                return null;
            }
            OnError = string.Empty;
            return db.Value;

        }

        public Int64? SaveTransaction_Aprobar(out string OnError)
        {

            Int64 ID = 0;
            try
            {

                //grabar transaccion.
                var id = this.Aprobar_Update(out OnError);
                if (!id.HasValue)
                {
                    OnError = "*** Error: al Aprobar turnos ****";
                    return 0;
                }
                ID = id.Value;

                return ID;

            }
            catch (Exception ex)
            {

                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>("SQL", nameof(SaveTransaction_Aprobar), "SaveTransaction_Aprobar", false, null, null, ex.StackTrace, ex);
                OnError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));

                return null;
            }
        }
        #endregion

        #region "Rechazar Turno por cfs"

        private Int64? Rechazar_Update(out string OnError)
        {

            parametros.Clear();
            parametros.Add("ID_SOL", this.ID_SOL);
            parametros.Add("SECUENCIA", this.SECUENCIA);
            parametros.Add("USUARIO_ESTADO", this.USUARIO_ESTADO);

            var db = sql_puntero.ExecuteInsertUpdateDeleteReturn(sql_puntero.Conexion_Local, 8000, "[Brbk].[RECHAZAR_TURNOS_SOLICITUD]", parametros, out OnError);
            if (!db.HasValue || db.Value < 0)
            {
                return null;
            }
            OnError = string.Empty;
            return db.Value;

        }

        public Int64? SaveTransaction_Rechazar(out string OnError)
        {

            Int64 ID = 0;
            try
            {

                //grabar transaccion.
                var id = this.Rechazar_Update(out OnError);
                if (!id.HasValue)
                {
                    OnError = "*** Error: al Rechazar turnos ****";
                    return 0;
                }
                ID = id.Value;

                return ID;

            }
            catch (Exception ex)
            {

                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>("SQL", nameof(SaveTransaction_Aprobar), "SaveTransaction_Rechazar", false, null, null, ex.StackTrace, ex);
                OnError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));

                return null;
            }
        }
        #endregion

        #region "Rechazar Turno por el usuario final"

        private Int64? Rechazar_Update_User(out string OnError)
        {

            parametros.Clear();
            parametros.Add("ID_SOL", this.ID_SOL);
            parametros.Add("SECUENCIA", this.SECUENCIA);
            parametros.Add("USUARIO_ESTADO", this.USUARIO_ESTADO);

            var db = sql_puntero.ExecuteInsertUpdateDeleteReturn(sql_puntero.Conexion_Local, 8000, "[Brbk].[RECHAZAR_TURNOS_SOLICITUD_USUARIO]", parametros, out OnError);
            if (!db.HasValue || db.Value < 0)
            {
                return null;
            }
            OnError = string.Empty;
            return db.Value;

        }

        public Int64? SaveTransaction_Rechazar_User(out string OnError)
        {

            Int64 ID = 0;
            try
            {

                //grabar transaccion.
                var id = this.Rechazar_Update_User(out OnError);
                if (!id.HasValue)
                {
                    OnError = "*** Error: al Rechazar turnos ****";
                    return 0;
                }
                ID = id.Value;

                return ID;

            }
            catch (Exception ex)
            {

                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>("SQL", nameof(SaveTransaction_Rechazar_User), "SaveTransaction_Rechazar_User", false, null, null, ex.StackTrace, ex);
                OnError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));

                return null;
            }
        }
        #endregion

    }
}
