using BillionEntidades;
using SqlConexion;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace BreakBulk
{
    public class device : Cls_Bil_Base
    {
        public long? Id { get; set; }
        public string Names { get; set; }
        public string Create_user { get; set; }
        public string Modifie_user { get; set; }
        public DateTime? Create_date { get; set; }
        public DateTime? Modifie_date { get; set; }
        public bool? Status { get; set; }
        public string Imei { get; set; }

        public device() : base()
        {
            init();
        }

        private static void OnInit(string Base)
        {
            sql_puntero = (sql_puntero == null) ? Cls_Conexion.Conexion() : sql_puntero;
            parametros = new Dictionary<string, object>();
            nueva_conexion = Cls_Conexion.Nueva_Conexion(Base);
        }

        public static List<device> listadoDevice(out string OnError)
        {
            OnInit("N4Middleware");
            parametros.Clear();
            return sql_puntero.ExecuteSelectControl<device>(nueva_conexion, 4000, "[brbk].[consultarDevice]", parametros, out OnError);
        }

        public static device GetDevice(long _id)
        {
            OnInit("N4Middleware");
            parametros.Clear();
            parametros.Add("i_id", _id);
            var obj = sql_puntero.ExecuteSelectOnly<device>(nueva_conexion, 4000, "[brbk].[consultarDevice]", parametros);
            return obj;
        }

        public Int64? Save_Update(out string OnError)
        {

            OnInit("N4Middleware");
            parametros.Clear();
            parametros.Add("i_id", this.Id);
            parametros.Add("i_Names", this.Names);
            parametros.Add("i_Create_user", this.Create_user);
            parametros.Add("i_Modifie_user", this.Modifie_user);
            parametros.Add("i_Status", this.Status);
            parametros.Add("i_Imei", this.Imei);

            var db = sql_puntero.ExecuteInsertUpdateDeleteReturn(nueva_conexion, 6000, "brbk.insertarDevice", parametros, out OnError);
            if (!db.HasValue || db.Value < 0)
            {
                return null;
            }
            OnError = string.Empty;
            return db.Value;

        }

    }
}
