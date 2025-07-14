using BillionEntidades;
using SqlConexion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReceptioMtyStock
{
    public class fotoSellos : Cls_Bil_Base
    {
        #region "Propiedades"                 
        public long? id { get; set; }
        public long idSealValidation { get; set; }
        public string ruta { get; set; }
        public string estado { get; set; }
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

        public static List<fotoSellos> listadoFotosSealPreEmbarque(long _gkeyContenedor, out string OnError)
        {
            OnInit("N4Middleware");
            parametros.Clear();
            parametros.Add("i_gkey", _gkeyContenedor);
            return sql_puntero.ExecuteSelectControl<fotoSellos>(nueva_conexion, 4000, "[mty].consultarFotoSealValidation", parametros, out OnError);
        }
    }
}
