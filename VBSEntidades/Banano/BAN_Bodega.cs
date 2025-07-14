using BillionEntidades;
using SqlConexion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;   

namespace VBSEntidades.Banano
{
    public class BAN_Bodega : Cls_Bil_Base
    {
        public int Id { get ; set; }
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public bool Estado { get; set; }
        public string Create_user { get; set; }
        public string Modifie_user { get; set; }
        public DateTime? Create_date { get; set; }
        public DateTime? Modifie_date { get; set; }

        public BAN_Bodega() : base()
        {
            init();
        }

        private static void OnInit(string Base)
        {
            sql_puntero = (sql_puntero == null) ? Cls_Conexion.Conexion() : sql_puntero;
            parametros = new Dictionary<string, object>();
            nueva_conexion = Cls_Conexion.Nueva_Conexion(Base);
        }

        public static List<BAN_Bodega> listadoBodegas(out string OnError)
        {
            //OnInit("N4Middleware");
            parametros.Clear();
            return sql_puntero.ExecuteSelectControl<BAN_Bodega>(sql_punteroVBS.Conexion_LocalVBS, 4000, "BAN_CONSULTAR_BODEGA", parametros, out OnError);
        }

        public static BAN_Bodega GetBodega(long _id)
        {
            //OnInit("N4Middleware");
            parametros.Clear();
            parametros.Add("i_id", _id);
            var obj = sql_puntero.ExecuteSelectOnly<BAN_Bodega>(sql_punteroVBS.Conexion_LocalVBS, 4000, "BAN_CONSULTAR_BODEGA", parametros);
            return obj;
        }

        public Int64? Save_Update(out string OnError)
        {

            //OnInit("N4Middleware");
            parametros.Clear();
            parametros.Add("i_id", this.Id);
            parametros.Add("i_codigo", this.Codigo);
            parametros.Add("i_nombre", this.Nombre);
            parametros.Add("i_Create_user", this.Create_user);
            parametros.Add("i_Modifie_user", this.Modifie_user);
            parametros.Add("i_Estado", this.Estado);

            var db = sql_puntero.ExecuteInsertUpdateDeleteReturn(sql_punteroVBS.Conexion_LocalVBS, 6000, "BAN_INSERTAR_BODEGA", parametros, out OnError);
            if (!db.HasValue || db.Value < 0)
            {
                return null;
            }
            OnError = string.Empty;
            return db.Value;

        }

    }
}
