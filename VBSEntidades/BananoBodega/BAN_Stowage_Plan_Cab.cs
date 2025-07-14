using BillionEntidades;
using SqlConexion;
using System;
using System.Collections.Generic;
using VBSEntidades.BananoMuelle;

namespace VBSEntidades.BananoBodega
{
    public class BAN_Stowage_Plan_Cab : Cls_Bil_Base
    {
        #region "Propiedades"
        public long idStowageCab { get; set; }
        public string idNave { get; set; }
        public string nave { get; set; }
        public int idLinea { get; set; }
        public string linea { get; set; }
        public bool estado { get; set; }
        public int fechaDocumento { get; set; }
        public string usuarioCrea { get; set; }
        public DateTime fechaCreacion { get; set; }
        public string usuarioModifica { get; set; }
        public DateTime? fechaModifica { get; set; }
        public List<BAN_Exclusion> Exclusiones { get; set; }
        public List<BAN_Stowage_Plan_Det> oDetalle { get; set; }
        #endregion

        public BAN_Stowage_Plan_Cab()
        {
            init();
        }

        private static void OnInit(string Base)
        {
            sql_puntero = (sql_puntero == null) ? Cls_Conexion.Conexion() : sql_puntero;
            parametros = new Dictionary<string, object>();
            nueva_conexion = Cls_Conexion.Nueva_Conexion(Base);
        }

        public static List<BAN_Stowage_Plan_Cab> ConsultarLista(string idNave, int linea, out string OnError)
        {
            parametros.Clear();
            parametros.Add("i_idNave", idNave);
            parametros.Add("i_idLinea", linea);
            return sql_puntero.ExecuteSelectControl<BAN_Stowage_Plan_Cab>(sql_punteroVBS.Conexion_LocalVBS, 8000, "[BAN_Stowage_Plan_Cab_Consultar]", parametros, out OnError);
        }


        public static BAN_Stowage_Plan_Cab GetEntidad(int _id)
        {
            parametros.Clear();
            parametros.Add("i_idStowageCab", _id);
            var obj = sql_puntero.ExecuteSelectOnly<BAN_Stowage_Plan_Cab>(sql_punteroVBS.Conexion_LocalVBS, 4000, "[BAN_Stowage_Plan_Cab_Consultar]", parametros);
            return obj;
        }

        //
        public static BAN_Stowage_Plan_Cab GetStowagePlanCabEspecifico(long? _id, string idNave, int linea, int fechaDocumento)
        {
            parametros.Clear();
            parametros.Add("i_idStowageCab", _id);
            parametros.Add("i_idNave", idNave);
            parametros.Add("i_idLinea", linea);
            parametros.Add("i_fechaDocumento", fechaDocumento);
            var obj = sql_puntero.ExecuteSelectOnly<BAN_Stowage_Plan_Cab>(sql_punteroVBS.Conexion_LocalVBS, 4000, "[BAN_Stowage_Plan_Cab_Consultar]", parametros);

            return obj;
        }

        public Int64? Save_Update(out string OnError)
        {
            parametros.Clear();
            parametros.Add("i_idNave", this.idNave);
            parametros.Add("i_nave", this.nave);
            parametros.Add("i_idLinea", this.idLinea);
            parametros.Add("i_linea", this.linea);
            parametros.Add("i_fechaDocumento", this.fechaDocumento);
            parametros.Add("i_estado", this.estado);
            parametros.Add("i_usuarioCrea", this.usuarioCrea);

            var db = sql_puntero.ExecuteInsertUpdateDeleteReturn(sql_punteroVBS.Conexion_LocalVBS, 6000, "[BAN_Stowage_Plan_Cab_Insertar]", parametros, out OnError);
            if (!db.HasValue || db.Value < 0)
            {
                return null;
            }
            OnError = string.Empty;
            return db.Value;
        }

        public Int64? Save_Autorizacion(out string OnError)
        {
            parametros.Clear();
            parametros.Add("i_idStowageCab", this.idStowageCab);
            parametros.Add("i_usuarioCrea", this.usuarioCrea);

            var db = sql_puntero.ExecuteInsertUpdateDeleteReturn(sql_punteroVBS.Conexion_LocalVBS, 6000, "[BAN_Stowage_Plan_Cab_UpdateDate]", parametros, out OnError);
            if (!db.HasValue || db.Value < 0)
            {
                return null;
            }
            OnError = string.Empty;
            return db.Value;
        }

        public static string GetConfiguracion(string app, string name)
        {
            //OnInit("N4Middleware");
            System.Data.SqlClient.SqlConnection cn = new System.Data.SqlClient.SqlConnection(sql_punteroVBS.Conexion_LocalVBS);

            var d = new System.Data.DataTable();
            using (var c = cn)
            {
                var coman = c.CreateCommand();
                coman.CommandType = System.Data.CommandType.StoredProcedure;
                coman.CommandText = "consultarConfiguracion";
                coman.Parameters.AddWithValue("i_module", app);
                coman.Parameters.AddWithValue("i_name", name);
                try
                {
                    c.Open();
                    d.Load(coman.ExecuteReader(System.Data.CommandBehavior.CloseConnection));
                }
                catch
                {
                    throw;
                }
                finally
                {
                    if (c.State == System.Data.ConnectionState.Open)
                    {
                        c.Close();
                    }
                    c.Dispose();
                }
            }
            if (d != null)
            {
                if (d.Rows.Count > 0)
                {
                    return d.Rows[0][0].ToString();
                }
            }
            return string.Empty;






        }
    }
}
