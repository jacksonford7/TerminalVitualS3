using BillionEntidades;
using SqlConexion;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BreakBulk
{
    public class configuracionAlerta : Cls_Bil_Base
    {
        #region "Propiedades"
        public int? id { get; set; }
        public string email { get; set; }
        public int idGrupoMail { get; set; }
        public grupoMail grupoMail { get; set; }
        public bool? estado { get; set; }
        public string usuarioCrea { get; set; }
        public DateTime? fechaCreacion { get; set; }
        public string usuarioModifica { get; set; }
        public DateTime? fechaModifica { get; set; }
        #endregion

        public configuracionAlerta() : base()
        {
            init();
        }

        private static void OnInit(string Base)
        {
            sql_puntero = (sql_puntero == null) ? Cls_Conexion.Conexion() : sql_puntero;
            parametros = new Dictionary<string, object>();
            nueva_conexion = Cls_Conexion.Nueva_Conexion(Base);
        }

        public static List<configuracionAlerta> listadoConfigNotificaAlertas(out string OnError)
        {
            OnInit("N4Middleware");
            parametros.Clear();
            return sql_puntero.ExecuteSelectControl<configuracionAlerta>(nueva_conexion, 4000, "[brbk].[consultarConfiguracionAlerta]", parametros, out OnError);
        }

        public static configuracionAlerta GetConfigNotificaAlertas(long _id)
        {
            OnInit("N4Middleware");
            parametros.Clear();
            parametros.Add("i_id", _id);
            var obj = sql_puntero.ExecuteSelectOnly<configuracionAlerta>(nueva_conexion, 4000, "[brbk].[consultarConfiguracionAlerta]", parametros);
            try { obj.grupoMail = grupoMail.GetGrupos(obj.idGrupoMail); } catch { }
            return obj;
        }

        public Int64? Save_Update(out string OnError)
        {
            OnInit("N4Middleware");
            parametros.Clear();
            parametros.Add("i_id", this.id);
            parametros.Add("i_email", this.email);
            parametros.Add("i_idGrupoMail", this.idGrupoMail);
            parametros.Add("i_estado", this.estado);
            parametros.Add("i_usuarioCrea", this.usuarioCrea);
            parametros.Add("i_usuarioModifica", this.usuarioModifica);

            var db = sql_puntero.ExecuteInsertUpdateDeleteReturn(nueva_conexion, 6000, "[brbk].[insertarConfiguracionAlerta]", parametros, out OnError);
            if (!db.HasValue || db.Value < 0)
            {
                return null;
            }
            OnError = string.Empty;
            return db.Value;
        }
    }
}
