using BillionEntidades;
using SqlConexion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BreakBulk
{
    public class fotoSellos : Cls_Bil_Base
    {
        #region "Propiedades"                 
        public long? id { get; set; }
        public long idSealMuelle { get; set; }
        public sellosImpo Sello { get; set; }
        public string ruta { get; set; }
        public string estado { get; set; }
        public estados Estados { get; set; }
        public string usuarioCrea { get; set; }
        public DateTime? fechaCreacion { get; set; }
        public string usuarioModifica { get; set; }
        public DateTime? fechaModifica { get; set; }
        #endregion

        public fotoSellos() : base()
        {
            init();
        }

        private static void OnInit(string Base)
        {
            sql_puntero = (sql_puntero == null) ? Cls_Conexion.Conexion() : sql_puntero;
            parametros = new Dictionary<string, object>();
            nueva_conexion = Cls_Conexion.Nueva_Conexion(Base);
        }

        public static List<fotoSellos> listadoFotosSealMuelle(long _idSealMuelle, out string OnError)
        {
            OnInit("N4Middleware");
            parametros.Clear();
            parametros.Add("i_idSealMuelle", _idSealMuelle);
            return sql_puntero.ExecuteSelectControl<fotoSellos>(nueva_conexion, 4000, "[mty].consultarFotoSealMuelle", parametros, out OnError);
        }
        public static List<fotoSellos> listadoFotosSealMuelleTV(long _idSealMuelle, out string OnError)
        {
            OnInit("N4Middleware");
            parametros.Clear();
            
            parametros.Add("unitGkey", _idSealMuelle);
            return sql_puntero.ExecuteSelectControl<fotoSellos>(nueva_conexion, 4000, "[mty].consultarFotoSealMuelleTV", parametros, out OnError);
        }
        public static fotoSellos GetFotoSealMuelle(long _id)
        {
            OnInit("N4Middleware");
            parametros.Clear();
            parametros.Add("i_id", _id);
            var obj = sql_puntero.ExecuteSelectOnly<fotoSellos>(nueva_conexion, 4000, "[mty].consultarFotoSealMuelle", parametros);
            try
            {
                obj.Sello = sellosImpo.GetSelloAsignado(obj.idSealMuelle);
                obj.Estados = estados.GetEstado(obj.estado);
            }
            catch { }
            return obj;
        }
    }
}
