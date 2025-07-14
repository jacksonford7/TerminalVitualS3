using BillionEntidades;
using SqlConexion;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace BreakBulk
{
    public class productos : Cls_Bil_Base
    {
        #region "Propiedades"
        public int? id { get; set; }
        public string nombre { get; set; }
        public int idManiobra { get; set; }
        public int idManiobra2 { get; set; }
        public maniobra Maniobra { get; set; }
        public maniobra Maniobra2 { get; set; }
        public int item { get; set; }
        public items Items { get; set; }
        public bool? estado { get; set; }
        public string usuarioCrea { get; set; }
        public DateTime? fechaCreacion { get; set; }
        public string usuarioModifica { get; set; }
        public DateTime? fechaModifica { get; set; }
        #endregion

        public productos() : base()
        {
            init();
        }

        private static void OnInit(string Base)
        {
            sql_puntero = (sql_puntero == null) ? Cls_Conexion.Conexion() : sql_puntero;
            parametros = new Dictionary<string, object>();
            nueva_conexion = Cls_Conexion.Nueva_Conexion(Base);
        }

        public static List<productos> listadoProductos( out string OnError)
        {
            OnInit("N4Middleware");
            parametros.Clear();
            return sql_puntero.ExecuteSelectControl<productos>(nueva_conexion, 4000, "[brbk].consultarProductos", parametros, out OnError);
        }

        public static productos GetProducto( int _id)
        {
            OnInit("N4Middleware");
            parametros.Clear();
            parametros.Add("i_id", _id);
            var obj = sql_puntero.ExecuteSelectOnly<productos>(nueva_conexion, 4000, "[brbk].consultarProductos", parametros);
            try
            {
                obj.Maniobra = maniobra.GetManiobra(obj.idManiobra);
                obj.Maniobra2 = maniobra.GetManiobra(obj.idManiobra2);
                obj.Items = items.GetItems(obj.item);
            }catch { }
            return obj;
        }

        public Int64? Save_Update(out string OnError)
        {

            OnInit("N4Middleware");
            parametros.Clear();
            parametros.Add("i_id", this.id);
            parametros.Add("i_nombre", this.nombre);
            parametros.Add("i_idManiobra", this.idManiobra);
            parametros.Add("i_idManiobra2", this.idManiobra2);
            parametros.Add("i_item", this.item);
            parametros.Add("i_estado", this.estado);
            parametros.Add("i_usuarioCrea", this.usuarioCrea);
            parametros.Add("i_usuarioModifica", this.usuarioModifica);

            var db = sql_puntero.ExecuteInsertUpdateDeleteReturn(nueva_conexion, 6000, "[brbk].insertarProducto", parametros, out OnError);
            if (!db.HasValue || db.Value < 0)
            {
                return null;
            }
            OnError = string.Empty;
            return db.Value;

        }

        public static DataView consultaProductos()
        {
            OnInit("N4Middleware");
            SqlConnection cn = new SqlConnection(nueva_conexion);

            var d = new DataTable();
            using (var c = cn)
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandText = "[brbk].consultarProductos";
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

    }
}
