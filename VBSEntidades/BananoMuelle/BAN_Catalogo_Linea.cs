using BillionEntidades;
using SqlConexion;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace VBSEntidades.BananoMuelle
{
    public class BAN_Catalogo_Linea : Cls_Bil_Base
    {
        #region "Propiedades"
        public int? id { get; set; }
        public string ruc { get; set; }
        public string codLine { get; set; }
        public string nombre { get; set; }
        public bool? estado { get; set; }
        public string usuarioCrea { get; set; }
        public DateTime? fechaCreacion { get; set; }
        public string usuarioModifica { get; set; }
        public DateTime? fechaModifica { get; set; }
        #endregion

        public BAN_Catalogo_Linea()
        {
            init();
        }

        private static void OnInit(string Base)
        {
            sql_puntero = (sql_puntero == null) ? Cls_Conexion.Conexion() : sql_puntero;
            parametros = new Dictionary<string, object>();
            nueva_conexion = Cls_Conexion.Nueva_Conexion(Base);
        }

        public static List<BAN_Catalogo_Linea> ConsultarListaLineas(out string OnError)
        {
            parametros.Clear();
            return sql_puntero.ExecuteSelectControl<BAN_Catalogo_Linea>(sql_punteroVBS.Conexion_LocalVBS, 8000, "[BAN_Catalogo_Linea_Consultar]", null, out OnError);
        }

        public static DataView ConsultarListaLlenaCombo(string txtRucCliente)
        {
            SqlConnection cn = new SqlConnection(sql_punteroVBS.Conexion_LocalVBS);

            var d = new DataTable();
            using (var c = cn)
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.Parameters.AddWithValue("@i_rucCliente", txtRucCliente);
                coman.CommandText = "BAN_Catalogo_Linea_Lista";
                try
                {
                    c.Open();
                    d.Load(coman.ExecuteReader(CommandBehavior.CloseConnection));
                }
                catch (SqlException ex)
                {
                    throw;
                }
                finally
                {
                    if (c.State == ConnectionState.Open)
                    {
                        c.Close();
                    }
                    c.Dispose();
                }
            }
            return d.DefaultView;
        }


        public static BAN_Catalogo_Linea GetLinea(long _id)
        {
            parametros.Clear();
            parametros.Add("i_id", _id);
            var obj = sql_puntero.ExecuteSelectOnly<BAN_Catalogo_Linea>(sql_punteroVBS.Conexion_LocalVBS, 4000, "[BAN_Catalogo_Linea_Consultar]", parametros);
            return obj;
        }

        public Int64? Save_Update(out string OnError)
        {
            parametros.Clear();
            parametros.Add("i_id", this.id);
            parametros.Add("i_ruc", this.ruc);
            parametros.Add("i_codLine", this.codLine);
            parametros.Add("i_nombre", this.nombre);
            parametros.Add("i_usuarioCrea", this.usuarioCrea);
            parametros.Add("i_usuarioModifica", this.usuarioModifica);
            parametros.Add("i_estado", this.estado);

            var db = sql_puntero.ExecuteInsertUpdateDeleteReturn(sql_punteroVBS.Conexion_LocalVBS, 6000, "[BAN_Catalogo_Linea_Insertar]", parametros, out OnError);
            if (!db.HasValue || db.Value < 0)
            {
                return null;
            }
            OnError = string.Empty;
            return db.Value;
        }

    }
}
