using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlConexion;

namespace BillionEntidades
{
    public class Cls_Bil_Lista_PagoTerceros : Cls_Bil_Base
    {
        #region "Variables"

        private Int64 _id_asignacion;
        private string _carga = string.Empty;
        private string _mrn= string.Empty;
        private string _msn = string.Empty;
        private string _hsn = string.Empty;
        private int? _total;

        private DateTime? _fecha_manifiesto = null;
        private string _ruc = string.Empty;
        private string _nombre = string.Empty;
        private DateTime? _fecha_asignado = null;
        private string _login_asigna = string.Empty;
        private string _estado = string.Empty;
        private string _login_modifica = string.Empty;
        private DateTime? _fecha_modifica = null;

        
        #endregion

        #region "Propiedades"

        public Int64 id_asignacion { get => _id_asignacion; set => _id_asignacion = value; }
        public string carga { get => _carga; set => _carga = value; }
        public string mrn { get => _mrn; set => _mrn = value; }
        public string msn { get => _msn; set => _msn = value; }
        public string hsn { get => _hsn; set => _hsn = value; }
        public int? total { get => _total; set => _total = value; }

        public DateTime? fecha_manifiesto { get => _fecha_manifiesto; set => _fecha_manifiesto = value; }
        public string ruc { get => _ruc; set => _ruc = value; }
        public string nombre { get => _nombre; set => _nombre = value; }
        public DateTime? fecha_asignado { get => _fecha_asignado; set => _fecha_asignado = value; }

        public string login_asigna { get => _login_asigna; set => _login_asigna = value; }
        public string estado { get => _estado; set => _estado = value; }
        public string login_modifica { get => _login_modifica; set => _login_modifica = value; }
        public DateTime? fecha_modifica { get => _fecha_modifica; set => _fecha_modifica = value; }


       

        #endregion

        public Cls_Bil_Lista_PagoTerceros()
        {
            init();
        }

        private static void OnInit()
        {
            sql_puntero = (sql_puntero == null) ? Cls_Conexion.Conexion() : sql_puntero;
            parametros = new Dictionary<string, object>();
            nueva_conexion = Cls_Conexion.Nueva_Conexion("PORTAL_BILLION");
        }

        /*lista todas las cartas autorizadas por rango de fechas y desconsolidadora*/
        public static List<Cls_Bil_Lista_PagoTerceros> Listado_Pagos_Terceros(DateTime FECHA_DESDE, DateTime FECHA_HASTA,  out string OnError)
        {

            OnInit();
            parametros.Clear();
            parametros.Add("desde", FECHA_DESDE);
            parametros.Add("hasta", FECHA_HASTA);
            return sql_puntero.ExecuteSelectControl<Cls_Bil_Lista_PagoTerceros>(sql_puntero.Conexion_Local, 6000, "[Bill].[listar_pago_terceros]", parametros, out OnError);

        }

    }
}
