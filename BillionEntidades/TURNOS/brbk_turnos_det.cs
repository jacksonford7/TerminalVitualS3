using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlConexion;

namespace BillionEntidades
{
    public class brbk_turnos_det : Cls_Bil_Base
    {

        #region "Variables"

        private Int64 _ID_TURNO;
        private Int64 _SECUENCIA;
        private DateTime _FECHA_TURNO;
        private Int64 _ID_TIPO_PRODUCTO;
        private string _DESC_TIPO_PRODUCTO;
        private string _ID_BODEGA;
        private string _DESC_BODEGA;
        private Int64 _CAPACIDAD = 0;
        private Int64 _FRECUENCIA = 0;
        private bool _ESTADO = false;
        private string _USUARIO_CREA = string.Empty;
        private string _USUARIO_MOD = string.Empty;
        private bool _FINSEMANA = false;
        private string _NOMBRE_CABECERA = String.Empty;
        private Int64 _IDX_ROW;
        private string _PASES = string.Empty;
        private int _USADOS = 0;
        private int _SALDO = 0;
        private int _NUEVA_CAPACIDAD = 0;
        private bool _CHECK = false;
        private string _MENSAJE = string.Empty;
        private int _SALDO_CAPACIDAD = 0;
        #endregion

        #region "Propiedades"

        public Int64 ID_TURNO { get => _ID_TURNO; set => _ID_TURNO = value; }
        public Int64 SECUENCIA { get => _SECUENCIA; set => _SECUENCIA = value; }
        public DateTime FECHA_TURNO { get => _FECHA_TURNO; set => _FECHA_TURNO = value; }
        public Int64 ID_TIPO_PRODUCTO { get => _ID_TIPO_PRODUCTO; set => _ID_TIPO_PRODUCTO = value; }
        public string DESC_TIPO_PRODUCTO { get => _DESC_TIPO_PRODUCTO; set => _DESC_TIPO_PRODUCTO = value; }
        public string ID_BODEGA { get => _ID_BODEGA; set => _ID_BODEGA = value; }
        public string DESC_BODEGA { get => _DESC_BODEGA; set => _DESC_BODEGA = value; }
        public Int64 CAPACIDAD { get => _CAPACIDAD; set => _CAPACIDAD = value; }
        public Int64 FRECUENCIA { get => _FRECUENCIA; set => _FRECUENCIA = value; }

        public bool ESTADO { get => _ESTADO; set => _ESTADO = value; }
        public string USUARIO_CREA { get => _USUARIO_CREA; set => _USUARIO_CREA = value; }
        public string USUARIO_MOD { get => _USUARIO_MOD; set => _USUARIO_MOD = value; }
        public bool FINSEMANA { get => _FINSEMANA; set => _FINSEMANA = value; }
        public string NOMBRE_CABECERA { get => _NOMBRE_CABECERA; set => _NOMBRE_CABECERA = value; }
        public Int64 IDX_ROW { get => _IDX_ROW; set => _IDX_ROW = value; }
        public string PASES { get => _PASES; set => _PASES = value; }
        public int USADOS { get => _USADOS; set => _USADOS = value; }
        public int SALDO { get => _SALDO; set => _SALDO = value; }
        public int NUEVA_CAPACIDAD { get => _NUEVA_CAPACIDAD; set => _NUEVA_CAPACIDAD = value; }
        public bool CHECK { get => _CHECK; set => _CHECK = value; }
        public string MENSAJE { get => _MENSAJE; set => _MENSAJE = value; }
        public int SALDO_CAPACIDAD { get => _SALDO_CAPACIDAD; set => _SALDO_CAPACIDAD = value; }
        #endregion

        public brbk_turnos_det()
        {
            init();
        }


        public brbk_turnos_det(Int64 _ID_TURNO,
         Int64 _SECUENCIA,
         DateTime _FECHA_TURNO,
         Int64 _ID_TIPO_PRODUCTO,
         string _DESC_TIPO_PRODUCTO,
         string _ID_BODEGA,
  string _DESC_BODEGA,
         int _CAPACIDAD,
         int _FRECUENCIA,
         bool _ESTADO,
         string _USUARIO_CREA,
         string _USUARIO_MOD,
         bool _FINSEMANA)
        {

            this.ID_TURNO = _ID_TURNO;
            this.SECUENCIA = _SECUENCIA;
            this.FECHA_TURNO = _FECHA_TURNO;
            this.ID_TIPO_PRODUCTO = _ID_TIPO_PRODUCTO;
            this.DESC_TIPO_PRODUCTO = _DESC_TIPO_PRODUCTO;

            this.ID_BODEGA = _ID_BODEGA;
            this.DESC_BODEGA = _DESC_BODEGA;

            this.CAPACIDAD = _CAPACIDAD;
            this.FRECUENCIA = _FRECUENCIA;

            this.ESTADO = _ESTADO;
            this.USUARIO_CREA = _USUARIO_CREA;
            this.USUARIO_MOD = _USUARIO_MOD;
            this.FINSEMANA = _FINSEMANA;
        }

        public static List<brbk_turnos_det> Generar_Turnos(DateTime FECHA_DESDE, DateTime FECHA_HASTA, Int64 ID_TIPO_PRODUCTO, string ID_BODEGA, Int64 FRECUENCIA, Int64 CAPACIDAD, out string OnError)
        {
            parametros.Clear();
            parametros.Add("FECHA_DESDE", FECHA_DESDE);
            parametros.Add("FECHA_HASTA", FECHA_HASTA);
            parametros.Add("ID_TIPO_PRODUCTO", ID_TIPO_PRODUCTO);
            parametros.Add("ID_BODEGA", ID_BODEGA);
            parametros.Add("FRECUENCIA", FRECUENCIA);
            parametros.Add("CAPACIDAD", CAPACIDAD);
            return sql_puntero.ExecuteSelectControl<brbk_turnos_det>(sql_puntero.Conexion_Local, 6000, "[BRBK].[GENERA_TURNOS]", parametros, out OnError);
        }

        public static List<brbk_turnos_det> Generar_TurnosVBS(DateTime FECHA_DESDE, DateTime FECHA_HASTA, out string OnError)
        {
            parametros.Clear();
            parametros.Add("FECHA_DESDE", FECHA_DESDE);
            parametros.Add("FECHA_HASTA", FECHA_HASTA);
            return sql_puntero.ExecuteSelectControl<brbk_turnos_det>(sql_puntero.Conexion_Local, 6000, "[BRBK].[GENERA_TURNOS]", parametros, out OnError);
        }



        #region "Detalle de Turnos Para Aprobar"
        public static List<brbk_turnos_det> Detalle_Turnos(Int64 ID_TURNO, out string OnError)
        {

            parametros.Clear();
            parametros.Add("ID_TURNO", ID_TURNO);
            return sql_puntero.ExecuteSelectControl<brbk_turnos_det>(sql_puntero.Conexion_Local, 5000, "[Brbk].[CARGA_DETALLE_TURNOS]", parametros, out OnError);

        }



        #endregion

        #region "Detalle de Turnos Para Modificar capacidad"
        public static List<brbk_turnos_det> Detalle_Turnos_Capacidad(DateTime FECHA_TURNO, Int64 ID_TIPO_PRODUCTO, string ID_BODEGA, out string OnError)
        {

            parametros.Clear();
            parametros.Add("FECHA_TURNO", FECHA_TURNO);
            parametros.Add("ID_TIPO_PRODUCTO", ID_TIPO_PRODUCTO);
            parametros.Add("ID_BODEGA", ID_BODEGA);
            return sql_puntero.ExecuteSelectControl<brbk_turnos_det>(sql_puntero.Conexion_Local, 5000, "[Brbk].[CARGA_DETALLE_TURNOS_CAPACIDAD]", parametros, out OnError);

        }



        #endregion

    }
}
