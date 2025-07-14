using BillionEntidades;
using SqlConexion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VBSEntidades.BananoMuelle
{
    public class BAN_Loading_Program_Aisv : Cls_Bil_Base
    {
        #region "Propiedades"
        public long? id { get; set; }
        public long? idLoadingDet { get; set; }
        public string aisv { get; set; }
        public bool estado { get; set; }
        public string usuarioCrea { get; set; }
        public DateTime fechaCreacion { get; set; }
        public string usuarioModifica { get; set; }
        public DateTime? fechaModifica { get; set; }
        #endregion

        public BAN_Loading_Program_Aisv()
        {
            init();
        }

        private static void OnInit(string Base)
        {
            sql_puntero = (sql_puntero == null) ? Cls_Conexion.Conexion() : sql_puntero;
            parametros = new Dictionary<string, object>();
            nueva_conexion = Cls_Conexion.Nueva_Conexion(Base);
        }

        public static List<BAN_Loading_Program_Aisv> ConsultarListadoAISV(long idDetalle, out string OnError)
        {
            parametros.Clear();
            parametros.Add("i_idLoadingDet", idDetalle);
            return sql_puntero.ExecuteSelectControl<BAN_Loading_Program_Aisv>(sql_punteroVBS.Conexion_LocalVBS, 8000, "[BAN_Loading_Program_Aisv_Consultar]", parametros, out OnError);
        }
    }

}
