using BillionEntidades;
using SqlConexion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BreakBulk
{
    public class novedad : Cls_Bil_Base
    {
        #region "Propiedades"
        public long? idNovedad { get; set; }
        public long idRecepcion { get; set; }
        public recepcion Recepcion { get; set; }
        public DateTime? fecha { get; set; }
        public string descripcion { get; set; }
        public string estado { get; set; }
        public estados Estados { get; set; }
        public string usuarioCrea { get; set; }
        public DateTime? fechaCreacion { get; set; }
        public string usuarioModifica { get; set; }
        public DateTime? fechaModifica { get; set; }
        #endregion

        public novedad() : base()
        {
            init();
        }

        private static void OnInit(string Base)
        {
            sql_puntero = (sql_puntero == null) ? Cls_Conexion.Conexion() : sql_puntero;
            parametros = new Dictionary<string, object>();
            nueva_conexion = Cls_Conexion.Nueva_Conexion(Base);
        }

        public static List<novedad> listadoNovedades(long _idRecepcion, out string OnError)
        {
            OnInit("N4Middleware");
            parametros.Clear();
            parametros.Add("i_idRecepcion", _idRecepcion);
            return sql_puntero.ExecuteSelectControl<novedad>(nueva_conexion, 4000, "[brbk].consultarNovedades", parametros, out OnError);
        }

        public static novedad GetNovedad(long _id)
        {
            OnInit("N4Middleware");
            parametros.Clear();
            parametros.Add("i_idRecepcion", _id);
            var obj = sql_puntero.ExecuteSelectOnly<novedad>(nueva_conexion, 4000, "[brbk].consultarNovedades", parametros);
            try
            {
                obj.Recepcion = recepcion.GetRecepcion(obj.idRecepcion);
                obj.Estados = estados.GetEstado(obj.estado);
            }catch { }
            return obj;
        }

        public Int64? Save_Update(out string OnError)
        {

            OnInit("N4Middleware");
            parametros.Clear();
            parametros.Add("i_idNovedad", this.idNovedad);
            parametros.Add("i_idRecepcion", this.idRecepcion);
            parametros.Add("i_fecha", this.fecha);
            parametros.Add("i_descripcion", this.descripcion);
            parametros.Add("i_estado", this.estado);
            parametros.Add("i_usuarioCrea", this.usuarioCrea);
            parametros.Add("i_usuarioModifica", this.usuarioModifica);

            var db = sql_puntero.ExecuteInsertUpdateDeleteReturn(nueva_conexion, 6000, "[brbk].insertarNovedad", parametros, out OnError);
            if (!db.HasValue || db.Value < 0)
            {
                return null;
            }
            OnError = string.Empty;
            return db.Value;

        }
    }
}
