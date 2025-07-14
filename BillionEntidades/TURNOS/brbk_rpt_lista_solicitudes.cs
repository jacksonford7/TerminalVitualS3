using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlConexion;

namespace BillionEntidades
{
    public class brbk_rpt_lista_solicitudes : Cls_Bil_Base
    {
        #region "Variables"

        private Int64 _ID_SOL;
        private DateTime _FECHA_SOL;
        private string _NUMERO_CARGA = string.Empty;
        private int _CANTIDAD_PASES = 0;
        private int _TOTAL_BULTOS = 0;

        private string _BODEGA = string.Empty;
        private string _TIPO_PRODUCTO = string.Empty;
        private string _USUARIO = string.Empty;
        private string _AGENTE = string.Empty;
        private string _IMPORTADOR = string.Empty;
        private string _TIPO_TURNO = string.Empty;
        private Int64 _SECUENCIA;
        private string _ESTADO_TURNO = string.Empty;
        private string _TRANSPORTISTA = string.Empty;
        private DateTime _FECHA_TURNO;
        private string _HORA_TURNO = string.Empty;
        private string _PASE_PUERTA = string.Empty;
        private int _USADOS = 0;

        private string _USUARIO_CREA = string.Empty;
        private string _USUARIO_MOD = string.Empty;


        #endregion

        #region "Propiedades"

        public Int64 ID_SOL { get => _ID_SOL; set => _ID_SOL = value; }
        public DateTime FECHA_SOL { get => _FECHA_SOL; set => _FECHA_SOL = value; }
        public string NUMERO_CARGA { get => _NUMERO_CARGA; set => _NUMERO_CARGA = value; }
        public int CANTIDAD_PASES { get => _CANTIDAD_PASES; set => _CANTIDAD_PASES = value; }
        public int TOTAL_BULTOS { get => _TOTAL_BULTOS; set => _TOTAL_BULTOS = value; }
        public string BODEGA { get => _BODEGA; set => _BODEGA = value; }
      
        public string TIPO_PRODUCTO { get => _TIPO_PRODUCTO; set => _TIPO_PRODUCTO = value; }
        public string USUARIO { get => _USUARIO; set => _USUARIO = value; }
        public string AGENTE { get => _AGENTE; set => _AGENTE = value; }
        public string IMPORTADOR { get => _IMPORTADOR; set => _IMPORTADOR = value; }

        public string TIPO_TURNO { get => _TIPO_TURNO; set => _TIPO_TURNO = value; }
        public Int64 SECUENCIA { get => _SECUENCIA; set => _SECUENCIA = value; }

        public string ESTADO_TURNO { get => _ESTADO_TURNO; set => _ESTADO_TURNO = value; }
        public string TRANSPORTISTA { get => _TRANSPORTISTA; set => _TRANSPORTISTA = value; }

        public DateTime FECHA_TURNO { get => _FECHA_TURNO; set => _FECHA_TURNO = value; }
        public string HORA_TURNO { get => _HORA_TURNO; set => _HORA_TURNO = value; }
        public string PASE_PUERTA { get => _PASE_PUERTA; set => _PASE_PUERTA = value; }

        public int USADOS { get => _USADOS; set => _USADOS = value; }

        public string USUARIO_CREA { get => _USUARIO_CREA; set => _USUARIO_CREA = value; }
        public string USUARIO_MOD { get => _USUARIO_MOD; set => _USUARIO_MOD = value; }

        private static String v_mensaje = string.Empty;

        #endregion


        public brbk_rpt_lista_solicitudes()
        {
            init();
        }

        public static List<brbk_rpt_lista_solicitudes> Listado_Solicitudes(string TIPO, DateTime FECHA_DESDE, DateTime FECHA_HASTA, out string OnError)
        {
            parametros.Clear();
            parametros.Add("TIPO", TIPO);
            parametros.Add("FECHA_DESDE", FECHA_DESDE);
            parametros.Add("FECHA_HASTA", FECHA_HASTA);

            return sql_puntero.ExecuteSelectControl<brbk_rpt_lista_solicitudes>(sql_puntero.Conexion_Local, 6000, "[Brbk].[RPT_LISTADO_SOLICITUDES_TURNOS]", parametros, out OnError);

        }

    }
}
