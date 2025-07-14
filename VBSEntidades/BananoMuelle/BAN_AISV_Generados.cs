using BillionEntidades;
using SqlConexion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VBSEntidades.BananoMuelle
{
    public class BAN_AISV_Generados : Cls_Bil_Base
    {
        #region "Propiedades"
        public long vbs_id_hora_cita { get; set; }
        public string aisv_codigo { get; set; }
        public int  vbs_box { get; set; }
        public string aisv_codig_clte { get; set; }
        public string aisv_numero_booking { get; set; }
        public string aisv_contenedor { get; set; }
        public string aisv_cedul_chof { get; set; }
        public string aisv_nombr_chof { get; set; }
        public string aisv_placa_vehi { get; set; }
        public string aisv_estado { get; set; }
        #endregion

        public BAN_AISV_Generados()
        {
            init();
        }

        private static void OnInit(string Base)
        {
            sql_puntero = (sql_puntero == null) ? Cls_Conexion.Conexion() : sql_puntero;
            parametros = new Dictionary<string, object>();
            nueva_conexion = Cls_Conexion.Nueva_Conexion(Base);
        }

        public static List<BAN_AISV_Generados> ConsultarListadoAISVGenerados(long idTurno, out string OnError)
        {
            OnInit("Portal_Servicios");
            parametros.Clear();
            parametros.Add("i_vbs_id_hora_cita", idTurno);
            return sql_puntero.ExecuteSelectControl<BAN_AISV_Generados>(nueva_conexion, 8000, "[BAN_AISV_consultar_X_Turno]", parametros, out OnError);
        }

        public static List<BAN_AISV_Generados> ConsultarListadoAISVGeneradosST(long idTurno, out string OnError)
        {
            OnInit("Portal_Servicios");
            parametros.Clear();
            parametros.Add("i_vbs_id_hora_cita", idTurno);
            return sql_puntero.ExecuteSelectControl<BAN_AISV_Generados>(nueva_conexion, 8000, "[BAN_AISV_consultar_X_TurnoST]", parametros, out OnError);
        }
        public static List<BAN_AISV_Generados> ConsultarListadoAISV(string codigo, out string OnError)
        {
            OnInit("Portal_Servicios");
            parametros.Clear();
            parametros.Add("i_aisv_codigo", codigo);
            return sql_puntero.ExecuteSelectControl<BAN_AISV_Generados>(nueva_conexion, 8000, "[BAN_AISV_consultar_X_AISV]", parametros, out OnError);
        }
    }
}
