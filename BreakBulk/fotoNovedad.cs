using BillionEntidades;
using SqlConexion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BreakBulk
{
    public class fotoNovedad : Cls_Bil_Base
    {
        #region "Propiedades"                 
        public long? id { get; set; }
        public long idnovedad { get; set; }
        public novedad Novedad { get; set; }
        public string ruta { get; set; }
        public string estado { get; set; }
        public estados Estados { get; set; }
        public string usuarioCrea { get; set; }
        public DateTime? fechaCreacion { get; set; }
        public string usuarioModifica { get; set; }
        public DateTime? fechaModifica { get; set; }
        #endregion

        public fotoNovedad() : base()
        {
            init();
        }

        private static void OnInit(string Base)
        {
            sql_puntero = (sql_puntero == null) ? Cls_Conexion.Conexion() : sql_puntero;
            parametros = new Dictionary<string, object>();
            nueva_conexion = Cls_Conexion.Nueva_Conexion(Base);
        }

        public static List<fotoNovedad> listadoFotosNovedad(long _idNovedad, out string OnError)
        {
            OnInit("N4Middleware");
            parametros.Clear();
            parametros.Add("i_idNovedad", _idNovedad);
            return sql_puntero.ExecuteSelectControl<fotoNovedad>(nueva_conexion, 4000, "[brbk].consultarFotoNovedades", parametros, out OnError);
        }

        public static fotoNovedad GetFotoNovedad(long _id)
        {
            OnInit("N4Middleware");
            parametros.Clear();
            parametros.Add("i_id", _id);
            var obj = sql_puntero.ExecuteSelectOnly<fotoNovedad>(nueva_conexion, 4000, "[brbk].consultarFotoNovedades", parametros);
            try
            {
                obj.Novedad = novedad.GetNovedad(obj.idnovedad);
                obj.Estados = estados.GetEstado(obj.estado);
            }catch { }
            return obj;
        }

        public Int64? Save_Update(out string OnError)
        {

            OnInit("N4Middleware");
            parametros.Clear();
            parametros.Add("i_id", this.id);
            parametros.Add("i_idNovedad", this.idnovedad);
            parametros.Add("i_ruta", this.ruta);
            parametros.Add("i_estado", this.estado);
            parametros.Add("i_usuarioCrea", this.usuarioCrea);
            parametros.Add("i_usuarioModifica", this.usuarioModifica);

            var db = sql_puntero.ExecuteInsertUpdateDeleteReturn(nueva_conexion, 6000, "[brbk].insertarFotoNovedad", parametros, out OnError);
            if (!db.HasValue || db.Value < 0)
            {
                return null;
            }
            OnError = string.Empty;
            return db.Value;

        }
    }
}
