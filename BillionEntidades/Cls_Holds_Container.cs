using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlConexion;

namespace BillionEntidades
{
   public class Cls_Holds_Container : Cls_Bil_Base
    {
        #region "Variables"


        private string _unit = string.Empty;
        private string _vd_id = string.Empty;
        private DateTime? _vd_ata ;
        private DateTime? _vd_atd ;
        private string _liquidacion = string.Empty;
        private bool _pago = false;
        private string _booking = string.Empty;
        private string _ruc = string.Empty;
        private bool _pago_final = false;
        private string _tipo = string.Empty;

        private string _P_VALOR3 = string.Empty;

        #endregion

        #region "Propiedades"

        public string unit { get => _unit; set => _unit = value; }
        public string vd_id { get => _vd_id; set => _vd_id = value; }
        public DateTime? vd_ata { get => _vd_ata; set => _vd_ata = value; }
        public DateTime? vd_atd { get => _vd_atd; set => _vd_atd = value; }
        public string liquidacion { get => _liquidacion; set => _liquidacion = value; }
        public bool pago { get => _pago; set => _pago = value; }
        public string booking { get => _booking; set => _booking = value; }
        public string ruc { get => _ruc; set => _ruc = value; }
        public bool pago_final { get => _pago_final; set => _pago_final = value; }
        public string tipo { get => _tipo; set => _tipo = value; }
        public string P_VALOR3 { get => _P_VALOR3; set => _P_VALOR3 = value; }

        private static String v_mensaje = string.Empty;

        #endregion


        public Cls_Holds_Container()
        {
            init();


        }

        private static void OnInit()
        {
            sql_puntero = (sql_puntero == null) ? Cls_Conexion.Conexion() : sql_puntero;
            parametros = new Dictionary<string, object>();
            nueva_conexion = Cls_Conexion.Nueva_Conexion("N4Middleware");
        }


        public static List<Cls_Holds_Container> Listado_Contenedores(out string OnError)
        {
            OnInit();

            parametros.Clear();
            return sql_puntero.ExecuteSelectControl<Cls_Holds_Container>(nueva_conexion, 6000, "desbloqueo_contendores", null, out OnError);

        }

        public static List<Cls_Holds_Container> Parametros(string tipo, string subtipo ,out string OnError)
        {
            OnInit();

            parametros.Clear();
            parametros.Add("tipo", tipo);
            parametros.Add("subtipo", subtipo);
            return sql_puntero.ExecuteSelectControl<Cls_Holds_Container>(nueva_conexion, 6000, "parametro_holds", parametros, out OnError);

        }
    }
}
