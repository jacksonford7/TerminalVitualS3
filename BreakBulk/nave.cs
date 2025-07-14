using BillionEntidades;
using SqlConexion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BreakBulk
{
    public class nave : Cls_Bil_Base
    {
        #region "Propiedades"
        public string id { get; set; }
        public string name { get; set; }
        public string in_customs_voy_nbr { get; set; }
        public DateTime published_eta { get; set; }
        public DateTime? ata{ get; set; }
        #endregion

        public nave()
        {
            init();
        }

        private static void OnInit(string Base)
        {
            sql_puntero = (sql_puntero == null) ? Cls_Conexion.Conexion() : sql_puntero;
            parametros = new Dictionary<string, object>();
            nueva_conexion = Cls_Conexion.Nueva_Conexion(Base);
        }

        public static nave GetNave(string _nave)
        {
            OnInit("N5");
            parametros.Clear();
            parametros.Add("i_nave", _nave);
            return sql_puntero.ExecuteSelectOnly<nave>(nueva_conexion, 4000, "[brbk].consultarDataNave", parametros);
        }

    }
}
