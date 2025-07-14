using BillionEntidades;
using SqlConexion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace VBSEntidades
{
    public class VBS_DetallePlantilla : Cls_Bil_Base
    {

        #region "Variables"

        private Int64 _SECUENCIA;
        private int _CANTIDAD = 0;
        private string _CATEGORIA ;
        private Int64 _FRECUENCIA = 0;
        private string _TIPO_CONTENEDOR;
        private string _TIPO_CARGAS;
        private bool _ESTADO = false;
        private string _USUARIO_CREACION = string.Empty;
        private string _USUARIO_MODIFICACION = string.Empty;
        private string _MENSAJE = string.Empty;
        private DateTime _VIGENCIA_INICIAL;
        private DateTime _VIGENCIA_FINAL;
        private DateTime _FECHA_CREACION;
        private int _ID_DETALLEPLANTILLA;

        // protected static Dictionary<string, object> parametrosVBS = null;
        #endregion

        #region "Propiedades"

        public Int64 SECUENCIA { get => _SECUENCIA; set => _SECUENCIA = value; }
        public int ID_DETALLEPLANTILLA { get => _ID_DETALLEPLANTILLA; set => _ID_DETALLEPLANTILLA = value; }
        public DateTime FECHA_CREACION { get => _FECHA_CREACION; set => _FECHA_CREACION = value; }
        public DateTime VIGENCIA_INICIAL { get => _VIGENCIA_INICIAL; set => _VIGENCIA_INICIAL = value; }
        public DateTime VIGENCIA_FINAL{ get => _VIGENCIA_FINAL; set => _VIGENCIA_FINAL = value; }
        public string TIPO_CONTENEDOR { get => _TIPO_CONTENEDOR; set => _TIPO_CONTENEDOR = value; }
        public string TIPO_CARGAS { get => _TIPO_CARGAS; set => _TIPO_CARGAS = value; }
        public string CATEGORIA { get => _CATEGORIA; set => _CATEGORIA = value; }
        public int CANTIDAD{ get => _CANTIDAD; set => _CANTIDAD = value; }
        public Int64 FRECUENCIA { get => _FRECUENCIA; set => _FRECUENCIA = value; }
        public bool ESTADO { get => _ESTADO; set => _ESTADO = value; }
        public string USUARIO_CREACION { get => _USUARIO_CREACION; set => _USUARIO_CREACION = value; }
        public string USUARIO_MODIFICACION { get => _USUARIO_MODIFICACION; set => _USUARIO_MODIFICACION = value; }
        public string MENSAJE { get => _MENSAJE; set => _MENSAJE = value; }
        #endregion

        public VBS_DetallePlantilla()
        {
            init();
            parametros = new Dictionary<string, object>();
        }


        public VBS_DetallePlantilla(
         Int64 _SECUENCIA,
         int _CANTIDAD,
         int _FRECUENCIA,
         bool _ESTADO,
         string _USUARIO_CREACION,
         string _USUARIO_MODIFICACION,
         DateTime _FECHA_CREACION,
         DateTime _VIGENCIA_INICIAL,
         DateTime _VIGENCIA_FINAL
         )
        {
            this.SECUENCIA = _SECUENCIA;
            this.CANTIDAD= _CANTIDAD;
            this.FRECUENCIA = _FRECUENCIA;
            this.ESTADO = _ESTADO;
            this.USUARIO_CREACION = _USUARIO_CREACION;
            this.USUARIO_MODIFICACION = _USUARIO_MODIFICACION;
            this.VIGENCIA_INICIAL = _VIGENCIA_INICIAL;
            this.VIGENCIA_FINAL = _VIGENCIA_FINAL;
            this.TIPO_CONTENEDOR = _TIPO_CONTENEDOR;
            this.TIPO_CARGAS = _TIPO_CARGAS;
            this.CATEGORIA = _CATEGORIA;
        }

        public static List<VBS_DetallePlantilla> Generar_Turnos(DateTime FECHA_DESDE, DateTime FECHA_HASTA, Int64 ID_TIPO_PRODUCTO, string ID_BODEGA, Int64 FRECUENCIA, Int64 CAPACIDAD, out string OnError)
        {
            var parametrosVBS = new Dictionary<string, object>();

            parametrosVBS.Clear();
            parametrosVBS.Add("FECHA_DESDE", FECHA_DESDE);
            parametrosVBS.Add("FECHA_HASTA", FECHA_HASTA);
            parametrosVBS.Add("ID_TIPO_PRODUCTO", ID_TIPO_PRODUCTO);
            parametrosVBS.Add("ID_BODEGA", ID_BODEGA);
            parametrosVBS.Add("FRECUENCIA", FRECUENCIA);
            parametrosVBS.Add("CAPACIDAD", CAPACIDAD);


            return sql_punteroVBS.ExecuteSelectControl<VBS_DetallePlantilla>(sql_punteroVBS.Conexion_LocalVBS, 6000, "VBS_GENERA_TURNOS", parametros, out OnError);

        }


        #region "Detalle de Turnos Para Aprobar"
        public static List<VBS_DetallePlantilla> Detalle_Turnos(Int64 ID_TURNO, out string OnError)
        {

            parametros.Clear();
            parametros.Add("ID_TURNO", ID_TURNO);
            return sql_punteroVBS.ExecuteSelectControl<VBS_DetallePlantilla>(sql_punteroVBS.Conexion_LocalVBS, 5000, "[Brbk].[CARGA_DETALLE_TURNOS]", parametros, out OnError);

        }



        #endregion

        #region "Detalle de Turnos Para Modificar capacidad"
        public static List<VBS_DetallePlantilla> Detalle_Turnos_Capacidad(DateTime FECHA_TURNO, Int64 ID_TIPO_PRODUCTO, string ID_BODEGA, out string OnError)
        {

            parametros.Clear();
            parametros.Add("FECHA_TURNO", FECHA_TURNO);
            parametros.Add("ID_TIPO_PRODUCTO", ID_TIPO_PRODUCTO);
            parametros.Add("ID_BODEGA", ID_BODEGA);
            return sql_punteroVBS.ExecuteSelectControl<VBS_DetallePlantilla>(sql_punteroVBS.Conexion_LocalVBS, 5000, "[Brbk].[CARGA_DETALLE_TURNOS_CAPACIDAD]", parametros, out OnError);

        }



        #endregion



    }
}
