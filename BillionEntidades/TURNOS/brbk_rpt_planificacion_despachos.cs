using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlConexion;

namespace BillionEntidades
{
    public class brbk_rpt_planificacion_despachos : Cls_Bil_Base
    {
        #region "Variables"
        private string _NUMERO_CARGA = string.Empty;
        private string _IMPORTADOR = string.Empty;
        private Int64 _ID_TURNO;
        private Int64 _SECUENCIA;
        private DateTime _FECHA_TURNO;
        private string _HORA_TURNO = string.Empty;
        private Int64 _ID_TIPO_PRODUCTO;
        private string _DESC_TIPO_PRODUCTO = string.Empty;
        private string _ID_BODEGA = string.Empty;
        private string _TIPO_SOLICITUD = string.Empty;

        private Int64 _CAPACIDAD = 0;
        private Int64 _FRECUENCIA = 0;

      
        private string _ESTADO = string.Empty;
        private string _USUARIO_CREA = string.Empty;
        //private Int64 _IDX_ROW ;
        private string _PASES = string.Empty;
        private int _USADOS;
      
        private int _SALDO;




        #endregion

        #region "Propiedades"
        public string NUMERO_CARGA { get => _NUMERO_CARGA; set => _NUMERO_CARGA = value; }
        public string IMPORTADOR { get => _IMPORTADOR; set => _IMPORTADOR = value; }

        public Int64 ID_TURNO { get => _ID_TURNO; set => _ID_TURNO = value; }
        public Int64 SECUENCIA { get => _SECUENCIA; set => _SECUENCIA = value; }

        public DateTime FECHA_TURNO { get => _FECHA_TURNO; set => _FECHA_TURNO = value; }
        public string HORA_TURNO { get => _HORA_TURNO; set => _HORA_TURNO = value; }
        public Int64 ID_TIPO_PRODUCTO { get => _ID_TIPO_PRODUCTO; set => _ID_TIPO_PRODUCTO = value; }

        public string DESC_TIPO_PRODUCTO { get => _DESC_TIPO_PRODUCTO; set => _DESC_TIPO_PRODUCTO = value; }
        public string ID_BODEGA { get => _ID_BODEGA; set => _ID_BODEGA = value; }
        public string TIPO_SOLICITUD { get => _TIPO_SOLICITUD; set => _TIPO_SOLICITUD = value; }

        public Int64 CAPACIDAD { get => _CAPACIDAD; set => _CAPACIDAD = value; }
        public Int64 FRECUENCIA { get => _FRECUENCIA; set => _FRECUENCIA = value; }
     
      
        public string ESTADO { get => _ESTADO; set => _ESTADO = value; }
        public string USUARIO_CREA { get => _USUARIO_CREA; set => _USUARIO_CREA = value; }
        //public Int64 IDX_ROW { get => _IDX_ROW; set => _IDX_ROW = value; }
        public string PASES { get => _PASES; set => _PASES = value; }

        public int USADOS { get => _USADOS; set => _USADOS = value; }
        
        public int SALDO { get => _SALDO; set => _SALDO = value; }
      
    

        private static String v_mensaje = string.Empty;

        #endregion


        public brbk_rpt_planificacion_despachos()
        {
            init();
        }

        public static List<brbk_rpt_planificacion_despachos> Listado_Solicitudes(Int64 ID_TIPO_PRODUCTO, DateTime FECHA_TURNO, DateTime FECHA_HASTA, string ID_BODEGA, out string OnError)
        {
            parametros.Clear();
            parametros.Add("ID_TIPO_PRODUCTO", ID_TIPO_PRODUCTO);
            parametros.Add("FECHA_TURNO", FECHA_TURNO);
            parametros.Add("FECHA_HASTA", FECHA_HASTA);
            parametros.Add("ID_BODEGA", ID_BODEGA);

            return sql_puntero.ExecuteSelectControl<brbk_rpt_planificacion_despachos>(sql_puntero.Conexion_Local, 6000, "[Brbk].[RPT_LISTADO_PLANIFICACION_TURNOS]", parametros, out OnError);

        }

    }
}
