using BillionEntidades;
using SqlConexion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClsAisvSav
{
    public class ExisteContenedorLinea : Cls_Bil_Base
    {

        public string CONTENEDOR { get; set; }
        public DateTime FECHA_DESPACHO { get; set; }
        public string bl { get; set; }
        public string cliente { get; set; }
        public string tipo { get; set; }
        public string nave { get; set; }
        public string viaje { get; set; }

        public string id { get; set; }
        public ExisteContenedorLinea()
        {
            init();
        }
        private static void OnInit(string Base)
        {
            sql_puntero = (sql_puntero == null) ? Cls_Conexion.Conexion() : sql_puntero;
            parametros = new Dictionary<string, object>();
            nueva_conexion = Cls_Conexion.Nueva_Conexion(Base);
        }
        public static List<ExisteContenedorLinea> ConsultarListadoAISVGenerados(string contenedor, string linea, out string OnError)
        {
            OnInit("PORTAL_BILLION");
            parametros.Clear();
            parametros.Add("CONTENEDOR", contenedor);
            parametros.Add("LINEA", linea);
            return sql_puntero.ExecuteSelectControl<ExisteContenedorLinea>(nueva_conexion, 8000, "[Sav_Bil_Existe_Contenedor_Linea]", parametros, out OnError);
        }

        public static List<ExisteContenedorLinea> Consultarbooking(string numerobooking, out string OnError)
        {
            OnInit("N5");
            parametros.Clear();
            parametros.Add("booking", numerobooking);
            return sql_puntero.ExecuteSelectControl<ExisteContenedorLinea>(nueva_conexion, 8000, "n4_sp_get_booking_iso", parametros, out OnError);
        }

        public static Int64? Save_Update(string TURNO_ID, string IDPASE_LINEA, out string OnError)
        {
            OnInit("SERVICE");
            parametros.Clear();
            parametros.Add("I_ID_TURNO", TURNO_ID);
            parametros.Add("I_ID_PASE_LINEA", IDPASE_LINEA);

            var db = sql_puntero.ExecuteInsertUpdateDeleteReturn(nueva_conexion, 6000, "[far_UPDATE_TURNO_SAV_DEPOT]", parametros, out OnError);
            if (!db.HasValue || db.Value < 0)
            {
                return null;
            }
            OnError = string.Empty;
            return db.Value;
        }

    }
}
