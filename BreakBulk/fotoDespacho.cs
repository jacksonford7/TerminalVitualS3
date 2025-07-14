using BillionEntidades;
using SqlConexion;
using System;
using System.Collections.Generic;

namespace BreakBulk
{
    public class fotoDespacho : Cls_Bil_Base
    {
        #region "Propiedades"                 
        public long? id { get; set; }
        public long idDespacho { get; set; }
        public despacho Despacho { get; set; }
        public string ruta { get; set; }
        public string estado { get; set; }
        public estados Estados { get; set; }
        public string usuarioCrea { get; set; }
        public DateTime? fechaCreacion { get; set; }
        public string usuarioModifica { get; set; }
        public DateTime? fechaModifica { get; set; }
        #endregion

        public fotoDespacho() : base()
        {
            init();
        }

        private static void OnInit(string Base)
        {
            sql_puntero = (sql_puntero == null) ? Cls_Conexion.Conexion() : sql_puntero;
            parametros = new Dictionary<string, object>();
            nueva_conexion = Cls_Conexion.Nueva_Conexion(Base);
        }

        public static List<fotoDespacho> listadoFotosDespacho(long _idDespacho, out string OnError)
        {
            OnInit("N4Middleware");
            parametros.Clear();
            parametros.Add("i_idDespacho", _idDespacho);
            return sql_puntero.ExecuteSelectControl<fotoDespacho>(nueva_conexion, 4000, "[brbk].consultarFotoDespacho", parametros, out OnError);
        }

        public static fotoDespacho GetFotoDespacho(long _id)
        {
            OnInit("N4Middleware");
            parametros.Clear();
            parametros.Add("i_id", _id);
            var obj = sql_puntero.ExecuteSelectOnly<fotoDespacho>(nueva_conexion, 4000, "[brbk].consultarFotoDespacho", parametros);
            try
            {
                obj.Despacho = despacho.GetDespacho(obj.idDespacho);
                obj.Estados = estados.GetEstado(obj.estado);
            }
            catch { }
            return obj;
        }

       
    }
}
