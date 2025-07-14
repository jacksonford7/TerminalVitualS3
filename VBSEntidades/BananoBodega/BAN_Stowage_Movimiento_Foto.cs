using BillionEntidades;
using SqlConexion;
using System;
using System.Collections.Generic;

namespace VBSEntidades.BananoBodega
{
    public class BAN_Stowage_Movimiento_Foto : Cls_Bil_Base
    {
        #region "Propiedades"                 
        public long id { get; set; }
        public long idMovimiento { get; set; }
        public string ruta { get; set; }
        public string estado { get; set; }
        public string usuarioCrea { get; set; }
        public DateTime? fechaCreacion { get; set; }
        public string usuarioModifica { get; set; }
        public DateTime? fechaModifica { get; set; }
        public BAN_Stowage_Movimiento oStowage_Movimiento { get; set; }
        #endregion

        public BAN_Stowage_Movimiento_Foto() : base()
        {
            init();
        }

        private static void OnInit(string Base)
        {
            sql_puntero = (sql_puntero == null) ? Cls_Conexion.Conexion() : sql_puntero;
            parametros = new Dictionary<string, object>();
            nueva_conexion = Cls_Conexion.Nueva_Conexion(Base);
        }

        public static List<BAN_Stowage_Movimiento_Foto> listadoFotosDespacho(long _idDespacho, out string OnError)
        {
            parametros.Clear();
            parametros.Add("i_idDespacho", _idDespacho);
            return sql_puntero.ExecuteSelectControl<BAN_Stowage_Movimiento_Foto>(nueva_conexion, 4000, "[]", parametros, out OnError);
        }

        public static BAN_Stowage_Movimiento_Foto GetFotoDespacho(long _id)
        {
            parametros.Clear();
            parametros.Add("i_id", _id);
            var obj = sql_puntero.ExecuteSelectOnly<BAN_Stowage_Movimiento_Foto>(nueva_conexion, 4000, "[]", parametros);
            try
            {
                obj.oStowage_Movimiento = BAN_Stowage_Movimiento.GetEntidad(obj.idMovimiento);
            }
            catch { }
            return obj;
        }


    }
}
