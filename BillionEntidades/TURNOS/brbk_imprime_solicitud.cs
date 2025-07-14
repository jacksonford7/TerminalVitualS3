using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlConexion;

namespace BillionEntidades
{
    public class brbk_imprime_solicitud : Cls_Bil_Base
    {

        #region "Variables"

        private Int64 _ID_SOL;
        private string _NUMERO_SOLICITUD = string.Empty;
        private string _NUMERO_CARGA = string.Empty;
        private string _MRN = string.Empty;
        private string _MSN = string.Empty;
        private string _HSN = string.Empty;
        private DateTime? _FECHA_SOL;
        private int? _CANTIDAD_VEHI;
        private int? _TOTAL_BUTLOS;

        private string _BODEGA = string.Empty;
        private int? _TIPO_PRODUCTO;
        private string _NOMBRE = string.Empty;
        private string _ESTADO = string.Empty;
        private string _USUARIO_ING = string.Empty;
        private string _USUARIO_DESC = string.Empty;
        private string _AGENTE_DESC = string.Empty;
        private string _IMPORTADOR_DESC = string.Empty;
        private DateTime? _FECHA_ING;
        private string _USUARIO_MOD = string.Empty;
        private DateTime? _FECHA_MOD;
        private string _USUARIO_AUT = string.Empty;
        private DateTime? _FECHA_AUT;
        private string _ESTADO_DET = string.Empty;
        private DateTime? _FECHA_EXPIRACION;
     
        private int? _CANTIDAD_CARGA;
        private int? _CANTIDAD_VEHICULOS;
        private int? _TOTAL_CARGA;

        private string _CONSIGNARIO_ID = string.Empty;
        private string _CONSIGNARIO_NOMBRE = string.Empty;
        private string _ID_EMPRESA = string.Empty;
        private string _TRANSPORTISTA_DESC = string.Empty;

        private bool? _FINALIZADA;
        private Int64? _ID_HORARIO;
        private string _HORARIO;
        private string _TIPO_TURNO;
        private static Int64? lm = -3;

        #endregion

        #region "Propiedades"

        public Int64 ID_SOL { get => _ID_SOL; set => _ID_SOL = value; }
        public string NUMERO_SOLICITUD { get => _NUMERO_SOLICITUD; set => _NUMERO_SOLICITUD = value; }
        public string NUMERO_CARGA { get => _NUMERO_CARGA; set => _NUMERO_CARGA = value; }
        public string MRN { get => _MRN; set => _MRN = value; }
        public string MSN { get => _MSN; set => _MSN = value; }
        public string HSN { get => _HSN; set => _HSN = value; }


        public DateTime? FECHA_SOL { get => _FECHA_SOL; set => _FECHA_SOL = value; }
        public int? CANTIDAD_VEHI { get => _CANTIDAD_VEHI; set => _CANTIDAD_VEHI = value; }
        public int? TOTAL_BUTLOS { get => _TOTAL_BUTLOS; set => _TOTAL_BUTLOS = value; }
        public string BODEGA { get => _BODEGA; set => _BODEGA = value; }
        public int? TIPO_PRODUCTO { get => _TIPO_PRODUCTO; set => _TIPO_PRODUCTO = value; }

        public string NOMBRE { get => _NOMBRE; set => _NOMBRE = value; }
        public string ESTADO { get => _ESTADO; set => _ESTADO = value; }
        public string USUARIO_ING { get => _USUARIO_ING; set => _USUARIO_ING = value; }
        public string USUARIO_DESC { get => _USUARIO_DESC; set => _USUARIO_DESC = value; }
        public string AGENTE_DESC { get => _AGENTE_DESC; set => _AGENTE_DESC = value; }
        public string IMPORTADOR_DESC { get => _IMPORTADOR_DESC; set => _IMPORTADOR_DESC = value; }
        public DateTime? FECHA_ING { get => _FECHA_ING; set => _FECHA_ING = value; }
        public string USUARIO_MOD { get => _USUARIO_MOD; set => _USUARIO_MOD = value; }
        public DateTime? FECHA_MOD { get => _FECHA_MOD; set => _FECHA_MOD = value; }
        public string USUARIO_AUT { get => _USUARIO_AUT; set => _USUARIO_AUT = value; }
        public DateTime? FECHA_AUT { get => _FECHA_AUT; set => _FECHA_AUT = value; }
        public string ESTADO_DET { get => _ESTADO_DET; set => _ESTADO_DET = value; }
        public DateTime? FECHA_EXPIRACION { get => _FECHA_EXPIRACION; set => _FECHA_EXPIRACION = value; }
        public int? CANTIDAD_CARGA { get => _CANTIDAD_CARGA; set => _CANTIDAD_CARGA = value; }
        public int? CANTIDAD_VEHICULOS { get => _CANTIDAD_VEHICULOS; set => _CANTIDAD_VEHICULOS = value; }
        public int? TOTAL_CARGA { get => _TOTAL_CARGA; set => _TOTAL_CARGA = value; }

        public string CONSIGNARIO_ID { get => _CONSIGNARIO_ID; set => _CONSIGNARIO_ID = value; }

        public string CONSIGNARIO_NOMBRE { get => _CONSIGNARIO_NOMBRE; set => _CONSIGNARIO_NOMBRE = value; }
        public string ID_EMPRESA { get => _ID_EMPRESA; set => _ID_EMPRESA = value; }
        public string TRANSPORTISTA_DESC { get => _TRANSPORTISTA_DESC; set => _TRANSPORTISTA_DESC = value; }
        public bool? FINALIZADA { get => _FINALIZADA; set => _FINALIZADA = value; }
        public Int64? ID_HORARIO { get => _ID_HORARIO; set => _ID_HORARIO = value; }
        public string HORARIO { get => _HORARIO; set => _HORARIO = value; }

        public string TIPO_TURNO { get => _TIPO_TURNO; set => _TIPO_TURNO = value; }

        private static String v_mensaje = string.Empty;


        #endregion

        public brbk_imprime_solicitud()
        {
            init();

        }

        /*carga todas las facturas*/
        public static List<brbk_imprime_solicitud> Imprime_Solicitud(Int64 ID_SOL, out string OnError)
        {
            parametros.Clear();
            parametros.Add("ID_SOL", ID_SOL);
            return sql_puntero.ExecuteSelectControl<brbk_imprime_solicitud>(sql_puntero.Conexion_Local, 7000, "[Brbk].[RPT_IMPRIME_SOLICITUD]", parametros, out OnError);

        }

        private Int64? Save(out string OnError)
        {

            parametros.Clear();

            parametros.Add("ID_SOL", this.ID_SOL);
            var db = sql_puntero.ExecuteInsertUpdateDeleteReturn(sql_puntero.Conexion_Local, 6000, "[Brbk].[ALERTA_SOLICITUD_MAIL]", parametros, out OnError);
            if (!db.HasValue || db.Value < 0)
            {
                return null;
            }
            OnError = string.Empty;
            return db.Value;

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
                    OnError = "*** Error: al enviar alerta de correo de solicitud de turnos ****";
                    return 0;
                }
                ID = id.Value;

                return ID;

            }
            catch (Exception ex)
            {

                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>("SQL", nameof(SaveTransaction), "brbk_imprime_solicitud", false, null, null, ex.StackTrace, ex);
                OnError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));

                return null;
            }
        }


    }
}
