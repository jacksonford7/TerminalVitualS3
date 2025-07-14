using BillionEntidades;
using SqlConexion;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;


namespace BreakBulk
{
    public class sellosImpo: Cls_Bil_Base
    {
        #region "Propiedades"
        public long? id { get; set; }
        public string referencia{ get; set; }
        public long gkey { get; set; }
        public string container { get; set; }
        public string sello_CGSA { get; set; }
        public string sello1 { get; set; }
        public string sello2 { get; set; }
        public string sello3 { get; set; }
        public string sello4 { get; set; }
        public string color { get; set; }
        public bool estado { get; set; }
        public bool diferencia { get; set; }
        public bool revisado { get; set; }
        public string ip { get; set; }
        public string mensaje { get; set; }
        public string xmlN4 { get; set; }
        public string xmlResult { get; set; }
        public string usuarioCrea { get; set; }
        public DateTime? fechaCreacion { get; set; }
        public string usuarioModifica { get; set; }
        public DateTime? fechaModifica { get; set; }
        #endregion

        public sellosImpo() : base()
        {
            //init();
        }

        private static void OnInit(string Base)
        {
            sql_puntero = null;
            sql_puntero = (sql_puntero == null) ? Cls_Conexion.Conexion() : sql_puntero;
            parametros = new Dictionary<string, object>();
            nueva_conexion = Cls_Conexion.Nueva_Conexion(Base);
        }

        public static List<sellosImpo> listadoSellosAsignados(bool? _diferencia, string _referencia, string _container, out string OnError)
        {
            OnInit("N4Middleware");
            parametros.Clear();
            parametros.Add("i_diferencia", _diferencia);
            parametros.Add("i_referencia", _referencia);
            parametros.Add("i_container", _container);
            return sql_puntero.ExecuteSelectControl<sellosImpo>(nueva_conexion, 4000, "mty.consultarSealMuelle", parametros, out OnError);
        }

        public static sellosImpo GetSelloAsignado(long _id)
        {
            OnInit("N4Middleware");
            parametros.Clear();
            parametros.Add("i_id", _id);
            var obj = sql_puntero.ExecuteSelectOnly<sellosImpo>(nueva_conexion, 4000, "mty.consultarSealMuelle", parametros);
            return obj;
        }

        public Int64? Save_Update(out string OnError)
        {
            OnInit("N4Middleware");
            parametros.Clear();
            parametros.Add("i_id", this.id);
            parametros.Add("i_sello1", this.sello1);
            parametros.Add("i_sello2", this.sello2);
            parametros.Add("i_sello3", this.sello3);
            parametros.Add("i_sello4", this.sello4);
            parametros.Add("i_usuarioModifica", this.usuarioModifica);
            parametros.Add("i_xml", this.xmlN4);
            parametros.Add("i_xmlResult", this.xmlResult);
            parametros.Add("i_estado", this.estado);
            parametros.Add("i_diferencia", this.diferencia);
            parametros.Add("i_revisado", this.revisado);

            var db = sql_puntero.ExecuteInsertUpdateDeleteReturn(nueva_conexion, 6000, "mty.updateSealMuelle", parametros, out OnError);
            if (!db.HasValue || db.Value < 0)
            {
                return null;
            }
            OnError = string.Empty;
            return db.Value;

        }

    }
}










