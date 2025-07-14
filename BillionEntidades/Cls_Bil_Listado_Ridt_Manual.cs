using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlConexion;

namespace BillionEntidades
{
    public class Cls_Bil_Listado_Ridt_Manual : Cls_Bil_Base
    {
        #region "Variables"

        private string _carga = string.Empty;
        private string _mrn = string.Empty;
        private string _msn = string.Empty;
        private string _hsn = string.Empty;
        private string _id_importador = string.Empty;
        private string _nombre_importador = string.Empty;
        private string _id_agente = string.Empty;
        private string _numero_declaracion = string.Empty;
        private string _usuario_registra = string.Empty;
        private string _comentarios = string.Empty;

        private DateTime? _fecha_registro = null;
      

        #endregion

        #region "Propiedades"

        public string carga { get => _carga; set => _carga = value; }
        public string mrn { get => _mrn; set => _mrn = value; }
        public string msn { get => _msn; set => _msn = value; }
        public string hsn { get => _hsn; set => _hsn = value; }

        public string id_importador { get => _id_importador; set => _id_importador = value; }
        public string nombre_importador { get => _nombre_importador; set => _nombre_importador = value; }
        public string id_agente { get => _id_agente; set => _id_agente = value; }
        public string numero_declaracion { get => _numero_declaracion; set => _numero_declaracion = value; }
        public string usuario_registra { get => _usuario_registra; set => _usuario_registra = value; }
        public string comentarios { get => _comentarios; set => _comentarios = value; }

        public DateTime? fecha_registro { get => _fecha_registro; set => _fecha_registro = value; }


        #endregion

        public Cls_Bil_Listado_Ridt_Manual()
        {
            init();
        }

        private static void OnInit()
        {
            sql_puntero = (sql_puntero == null) ? Cls_Conexion.Conexion() : sql_puntero;
            parametros = new Dictionary<string, object>();
            nueva_conexion = Cls_Conexion.Nueva_Conexion("PORTAL_BILLION");
        }

        public static List<Cls_Bil_Listado_Ridt_Manual> Listado_Ridt_Manual(DateTime FECHA_DESDE, DateTime FECHA_HASTA, out string OnError)
        {

            OnInit();
            parametros.Clear();
            parametros.Add("desde", FECHA_DESDE);
            parametros.Add("hasta", FECHA_HASTA);
            return sql_puntero.ExecuteSelectControl<Cls_Bil_Listado_Ridt_Manual>(sql_puntero.Conexion_Local, 6000, "[Bill].[listar_ridt_manual]", parametros, out OnError);

        }
    }
}
