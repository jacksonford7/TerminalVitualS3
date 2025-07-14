using BillionEntidades;
using SqlConexion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BreakBulk
{
    public class recepcion : Cls_Bil_Base
    {
        #region "Propiedades"                                                   
        public long? idRecepcion { get; set; }
        public long idTarjaDet { get; set; }
        public tarjaDet TarjaDet { get; set; }
        public int idGrupo { get; set; }
        public grupos Grupo { get; set; }
        public string lugar { get; set; }
        public decimal? cantidad { get; set; }
        public string ubicacion { get; set; }
        public ubicacion Ubicaciones { get; set; }
        public string observacion { get; set; }
        public string estado { get; set; }
        public estados Estados { get; set; }
        public string usuarioCrea { get; set; }
        public DateTime? fechaCreacion { get; set; }
        public string usuarioModifica { get; set; }
        public DateTime? fechaModifica { get; set; }
        #endregion

        public recepcion() : base()
        {
            init();
        }

        private static void OnInit(string Base)
        {
            sql_puntero = (sql_puntero == null) ? Cls_Conexion.Conexion() : sql_puntero;
            parametros = new Dictionary<string, object>();
            nueva_conexion = Cls_Conexion.Nueva_Conexion(Base);
        }

        public static List<recepcion> listadoRecepcion(long _idTarjaDet,out string OnError)
        {
            OnInit("N4Middleware");
            parametros.Clear();
            parametros.Add("i_idTarjaDet", _idTarjaDet);
            return sql_puntero.ExecuteSelectControl<recepcion>(nueva_conexion, 4000, "[brbk].consultarRecepcion", parametros, out OnError);
        }

        public static recepcion GetRecepcion(long _id)
        {
            OnInit("N4Middleware");
            parametros.Clear();
            parametros.Add("i_idRecepcion", _id);
            var obj = sql_puntero.ExecuteSelectOnly<recepcion>(nueva_conexion, 4000, "[brbk].consultarRecepcion", parametros);
            try
            {
                obj.Grupo = grupos.GetGrupos(obj.idGrupo);
                obj.TarjaDet = tarjaDet.GetTarjaDet(obj.idTarjaDet);
                obj.Estados = estados.GetEstado(obj.estado);
            }catch { }
            return obj;
        }

        public Int64? Save_Update(out string OnError)
        {

            OnInit("N4Middleware");
            parametros.Clear();
            parametros.Add("i_idRecepcion", this.idRecepcion);
            parametros.Add("i_idTarjaDet", this.idTarjaDet);
            parametros.Add("i_idGrupo", this.idGrupo);
            parametros.Add("i_lugar", this.lugar);
            parametros.Add("i_cantidad", this.cantidad);
            parametros.Add("i_ubicacion", this.ubicacion);
            parametros.Add("i_observacion", this.observacion);
            parametros.Add("i_estado", this.estado);
            parametros.Add("i_usuarioCrea", this.usuarioCrea);
            parametros.Add("i_usuarioModifica", this.usuarioModifica);

            var db = sql_puntero.ExecuteInsertUpdateDeleteReturn(nueva_conexion, 6000, "[brbk].insertarRecepcion", parametros, out OnError);
            if (!db.HasValue || db.Value < 0)
            {
                return null;
            }
            OnError = string.Empty;
            return db.Value;

        }
    }
}
