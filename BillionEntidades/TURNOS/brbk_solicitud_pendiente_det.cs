using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlConexion;

namespace BillionEntidades
{
   public  class brbk_solicitud_pendiente_det : Cls_Bil_Base
    {

        #region "Variables"

        private Int64 _ID_SOL;
        private Int64 _SECUENCIA;
        private string _ESTADO = string.Empty;
        private DateTime _FECHA_EXPIRACION;
        private int _CANTIDAD_CARGA = 0;
        private int _CANTIDAD_VEHICULOS = 0;
        private int _TOTAL_CARGA = 0;

        private string _USUARIO_REGISTRO;
        private DateTime? _FECHA_REGISTRO;
        private string _USUARIO_ESTADO;
        private DateTime? _FECHA_ESTADO;
        private string _CONSIGNARIO_ID;
        private string _CONSIGNARIO_NOMBRE;
        private string _TRANSPORTISTA_DESC = string.Empty;
        private string _ID_EMPRESA = string.Empty;
        private bool _FINALIZADA = false;
        private Int64? _ID_HORARIO;
        private string _HORARIO = string.Empty;
        private Int64? _ID_HORARIO_NEW;
        private string _HORARIO_NEW = string.Empty;
        private DateTime? _FECHA_NEW;
        private string _ESTADO_DESCRIPCION = string.Empty;
        private DateTime? _TURNO_INICIA;
        private DateTime? _TURNO_FIN;
        private Int64 _PASE_DESDE_SOLICITUD;
        private string _ESTADO_TURNO = string.Empty;
        private string _NUMERO_PASE_N4 = string.Empty;
        #endregion

        #region "Propiedades"

        public Int64 ID_SOL { get => _ID_SOL; set => _ID_SOL = value; }
        public Int64 SECUENCIA { get => _SECUENCIA; set => _SECUENCIA = value; }
        public string ESTADO { get => _ESTADO; set => _ESTADO = value; }

        public DateTime FECHA_EXPIRACION { get => _FECHA_EXPIRACION; set => _FECHA_EXPIRACION = value; }
        public int CANTIDAD_CARGA { get => _CANTIDAD_CARGA; set => _CANTIDAD_CARGA = value; }
        public int CANTIDAD_VEHICULOS { get => _CANTIDAD_VEHICULOS; set => _CANTIDAD_VEHICULOS = value; }
        public int TOTAL_CARGA { get => _TOTAL_CARGA; set => _TOTAL_CARGA = value; }
        public string USUARIO_REGISTRO { get => _USUARIO_REGISTRO; set => _USUARIO_REGISTRO = value; }
        public DateTime? FECHA_REGISTRO { get => _FECHA_REGISTRO; set => _FECHA_REGISTRO = value; }
        public string USUARIO_ESTADO { get => _USUARIO_ESTADO; set => _USUARIO_ESTADO = value; }
        public DateTime? FECHA_ESTADO { get => _FECHA_ESTADO; set => _FECHA_ESTADO = value; }

        public string CONSIGNARIO_ID { get => _CONSIGNARIO_ID; set => _CONSIGNARIO_ID = value; }
        public string CONSIGNARIO_NOMBRE { get => _CONSIGNARIO_NOMBRE; set => _CONSIGNARIO_NOMBRE = value; }
        public string TRANSPORTISTA_DESC { get => _TRANSPORTISTA_DESC; set => _TRANSPORTISTA_DESC = value; }
        public string ID_EMPRESA { get => _ID_EMPRESA; set => _ID_EMPRESA = value; }


        public bool FINALIZADA { get => _FINALIZADA; set => _FINALIZADA = value; }
      

        public Int64? ID_HORARIO { get => _ID_HORARIO; set => _ID_HORARIO = value; }
        public string HORARIO { get => _HORARIO; set => _HORARIO = value; }
        public Int64? ID_HORARIO_NEW { get => _ID_HORARIO_NEW; set => _ID_HORARIO_NEW = value; }
        public string HORARIO_NEW { get => _HORARIO_NEW; set => _HORARIO_NEW = value; }
        public DateTime? FECHA_NEW { get => _FECHA_NEW; set => _FECHA_NEW = value; }
        public string ESTADO_DESCRIPCION { get => _ESTADO_DESCRIPCION; set => _ESTADO_DESCRIPCION = value; }
        public DateTime? TURNO_INICIA { get => _TURNO_INICIA; set => _TURNO_INICIA = value; }
        public DateTime? TURNO_FIN { get => _TURNO_FIN; set => _TURNO_FIN = value; }
        public Int64 PASE_DESDE_SOLICITUD { get => _PASE_DESDE_SOLICITUD; set => _PASE_DESDE_SOLICITUD = value; }
        public string ESTADO_TURNO { get => _ESTADO_TURNO; set => _ESTADO_TURNO = value; }
        public string NUMERO_PASE_N4 { get => _NUMERO_PASE_N4; set => _NUMERO_PASE_N4 = value; }
        #endregion

        public brbk_solicitud_pendiente_det()
        {
            init();
        }


        public brbk_solicitud_pendiente_det(     Int64 _ID_SOL,
         Int64 _SECUENCIA,
         string _ESTADO ,
         DateTime _FECHA_EXPIRACION,
         int _CANTIDAD_CARGA ,
         int _CANTIDAD_VEHICULOS ,
         int _TOTAL_CARGA,

         string _USUARIO_REGISTRO,
         DateTime? _FECHA_REGISTRO,
         string _USUARIO_ESTADO,
         DateTime? _FECHA_ESTADO,
         string _CONSIGNARIO_ID,
         string _CONSIGNARIO_NOMBRE,
         string _TRANSPORTISTA_DESC,
         string _ID_EMPRESA,
         bool _FINALIZADA,
         Int64? _ID_HORARIO,
         string _HORARIO ,
         Int64? _ID_HORARIO_NEW,
         string _HORARIO_NEW ,
         DateTime? _FECHA_NEW)
        {

            this.ID_SOL = _ID_SOL;
            this.SECUENCIA = _SECUENCIA;
            this.ESTADO = _ESTADO;
            this.FECHA_EXPIRACION = _FECHA_EXPIRACION;
            this.CANTIDAD_CARGA = _CANTIDAD_CARGA;

            this.CANTIDAD_VEHICULOS = _CANTIDAD_VEHICULOS;
            this.TOTAL_CARGA = _TOTAL_CARGA;

            this.USUARIO_REGISTRO = _USUARIO_REGISTRO;
            this.FECHA_REGISTRO = _FECHA_REGISTRO;

            this.USUARIO_ESTADO = _USUARIO_ESTADO;
            this.FECHA_ESTADO = _FECHA_ESTADO;
            this.CONSIGNARIO_ID = _CONSIGNARIO_ID;
            this.CONSIGNARIO_NOMBRE = _CONSIGNARIO_NOMBRE;
            this.TRANSPORTISTA_DESC = _TRANSPORTISTA_DESC;
            this.ID_EMPRESA = _ID_EMPRESA;
            this.FINALIZADA = _FINALIZADA;
            this.ID_HORARIO = _ID_HORARIO;
            this.ID_HORARIO_NEW = _ID_HORARIO_NEW;
            this.HORARIO_NEW = _HORARIO_NEW;
            this.FECHA_NEW = _FECHA_NEW;
        }

    

        #region "Detalle de Turnos Para Aprobar"
        public static List<brbk_solicitud_pendiente_det> Detalle_Solicitud(Int64 ID_SOL,  out string OnError)
        {

            parametros.Clear();
            parametros.Add("ID_SOL", ID_SOL);
            return sql_puntero.ExecuteSelectControl<brbk_solicitud_pendiente_det>(sql_puntero.Conexion_Local, 6000, "[Brbk].[CARGA_DETALLE_SOLICITUD]", parametros, out OnError);

        }



        #endregion

        #region "Detalle de Turnos Aprobados"
        public static List<brbk_solicitud_pendiente_det> Detalle_Solicitud_Aprobada(Int64 ID_SOL, out string OnError)
        {

            parametros.Clear();
            parametros.Add("ID_SOL", ID_SOL);
            return sql_puntero.ExecuteSelectControl<brbk_solicitud_pendiente_det>(sql_puntero.Conexion_Local, 6000, "[Brbk].[CARGA_DETALLE_SOLICITUD_APROBADA]", parametros, out OnError);

        }



        #endregion

    }
}
