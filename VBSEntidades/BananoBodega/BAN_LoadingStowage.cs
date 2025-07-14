using BillionEntidades;
using SqlConexion;
using System;
using System.Collections.Generic;
using VBSEntidades.BananoMuelle;

namespace VBSEntidades.BananoBodega
{
    public class BAN_LoadingStowage : Cls_Bil_Base
    {
        #region "Propiedades"
                     
        public long id { get; set; }
        public string destino { get; set; }
        public string fecha { get; set; }
        public string time { get; set; }
        public int? box { get; set; }
        public string deck { get; set; }
        public int idHold { get; set; }
        public string hold { get; set; }
        public string cargo { get; set; }
        public string marca { get; set; }
        public string exportador { get; set; }
        public string consignatario { get; set; }
        public string aisv { get; set; }
        #endregion

        public BAN_LoadingStowage()
        {
            init();
        }

        private static void OnInit(string Base)
        {
            sql_puntero = (sql_puntero == null) ? Cls_Conexion.Conexion() : sql_puntero;
            parametros = new Dictionary<string, object>();
            nueva_conexion = Cls_Conexion.Nueva_Conexion(Base);
        }

        public static List<BAN_LoadingStowage> ConsultarLista(string _idNave, int _idLinea, out string OnError)
        {
            parametros.Clear();
            parametros.Add("i_idNave", _idNave);
            parametros.Add("i_idLinea", _idLinea);
            return sql_puntero.ExecuteSelectControl<BAN_LoadingStowage>(sql_punteroVBS.Conexion_LocalVBS, 8000, "[BAN_LoadingStowage_Rpt]", parametros, out OnError);
        }
       
    }
}
