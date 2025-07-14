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
    public class tarjaDet : Cls_Bil_Base
    {
        #region "Propiedades"
        public long? idTarjaDet { get; set; }
        public long? idTarja { get; set; }

        public string idNave { get; set; }
        public string idAgente { get; set; }
        public string Agente { get; set; }
        public decimal? arrastre { get; set; }
        public decimal? pendiente { get; set; }

        public tarjaCab tarjaCab { get; set; }
        public string bl { get; set; }
        public string mrn { get; set; }
        public string msn { get; set; }
        public string hsn { get; set; }
        public string idConsignatario { get; set; }
        public string Consignatario { get; set; }
        public string carrier_id { get; set; }
        public string productoEcuapass { get; set; }
        public int idProducto { get; set; }
        public productos producto { get; set; }
        public int idManiobra { get; set; }
        public maniobra maniobra { get; set; }
        public int idManiobra2 { get; set; }
        public maniobra maniobra2 { get; set; }
        public int? idItem { get; set; }
        public int idCondicion { get; set; }
        public condicion condicion { get; set; }
        public decimal cantidad { get; set; }
        public int kilos { get; set; }
        public decimal? cubicaje { get; set; }
        public string descripcion { get; set; }
        public string contenido { get; set; }
        public string observacion { get; set; }
        public decimal? tonelaje { get; set; }
        public string ubicacion { get; set; }
        public string estado { get; set; }
        public estados Estados { get; set; }

        public string carga { get; set; }
        public string consigna { get; set; }

        public bool imdt{ get; set; }
        public string imdt_num { get; set; }
        public DateTime fecha_imdt { get; set; }
        public string usuario_imdt { get; set; }
        public bool n4 { get; set; }
        public DateTime fecha_n4 { get; set; }
        public string usuario_n4 { get; set; }
        public string imo { get; set; }
        public string gKey_unit_BL { get; set; }

        public string usuarioCrea { get; set; }
        public DateTime? fechaCreacion { get; set; }
        public string usuarioModifica { get; set; }
        public DateTime? fechaModifica { get; set; }
        #endregion

        public tarjaDet() : base()
        {
            //init();
        }

        private static void OnInit(string Base)
        {
            sql_puntero = (sql_puntero == null) ? Cls_Conexion.Conexion() : sql_puntero;
            parametros = new Dictionary<string, object>();
            nueva_conexion = Cls_Conexion.Nueva_Conexion(Base);
        }

        public static List<tarjaDet> listadoTarjaDet(long _idTarja, out string OnError)
        {
            OnInit("N4Middleware");
            parametros.Clear();
            parametros.Add("i_idTarja", _idTarja);
            return sql_puntero.ExecuteSelectControl<tarjaDet>(nueva_conexion, 4000, "[brbk].consultartarjaDet", parametros, out OnError);
        }

        public static List<tarjaDet> ConsultaDataEcuapass( string _carrier_id, string _mrn, out string OnError)
        {
            OnInit("N4Middleware");
            parametros.Clear();
            parametros.Add("i_carrier_id", _carrier_id);
            parametros.Add("i_mrn", _mrn);
            return sql_puntero.ExecuteSelectControl<tarjaDet>(nueva_conexion, 4000, "[brbk].consultarDataEcuapass", parametros, out OnError);
        }

        public static List<tarjaDet> ConsultaDataEcuapassCGSA(string _mrn, out string OnError)
        {
            OnInit("N4Middleware");
            parametros.Clear();
            parametros.Add("i_mrn", _mrn);
            return sql_puntero.ExecuteSelectControl<tarjaDet>(nueva_conexion, 4000, "[brbk].consultarDataEcuapass_cgsa", parametros, out OnError);
        }

        public static tarjaDet GetTarjaDet(long _idTarjaDet)
        {
            OnInit("N4Middleware");
            parametros.Clear();
            parametros.Add("i_idTarjaDet", _idTarjaDet);
            var obj = sql_puntero.ExecuteSelectOnly<tarjaDet>(nueva_conexion, 4000, "[brbk].consultartarjaDet", parametros);
            try
            {
                obj.carga = string.Format("{0}-{1}-{2}", obj.mrn, obj.msn, obj.hsn);
                obj.consigna = string.Format("{0} {1} ", obj.idConsignatario.Trim(), obj.Consignatario);
                obj.Estados = estados.GetEstado(obj.estado);
                obj.producto = productos.GetProducto(obj.idProducto);
                obj.condicion = condicion.GetCondicion(obj.idCondicion);
                obj.maniobra = obj.producto.Maniobra;
                obj.maniobra2 = obj.producto.Maniobra2;
                obj.tarjaCab = tarjaCab.GetTarjaCab( long.Parse(obj.idTarja.ToString()));
            }
            catch { }
            
            return obj;
        }

        public static tarjaDet GetTarjaDet(string _mrn, string _msn, string _hsn)
        {
            OnInit("N4Middleware");
            parametros.Clear();
            parametros.Add("i_mrn", _mrn);
            parametros.Add("i_msn", _msn);
            parametros.Add("i_hsn", _hsn);
            var obj = sql_puntero.ExecuteSelectOnly<tarjaDet>(nueva_conexion, 4000, "[brbk].consultarTarjaDetXcarga", parametros);
            try
            {
                obj.carga = string.Format("{0}-{1}-{2}", obj.mrn, obj.msn, obj.hsn);
                obj.consigna = string.Format("{0} {1} ", obj.idConsignatario.Trim(), obj.Consignatario);
                obj.Estados = estados.GetEstado(obj.estado);
                obj.producto = productos.GetProducto(obj.idProducto);
                obj.condicion = condicion.GetCondicion(obj.idCondicion);
                obj.maniobra = obj.producto.Maniobra;
                obj.tarjaCab = tarjaCab.GetTarjaCab(long.Parse(obj.idTarja.ToString()));
            }
            catch { }

            return obj;
        }

        public static List<tarjaDet> GetTarjaDetXNave(string  _nave, out string OnError)
        {
            OnInit("N4Middleware");
            parametros.Clear();
            parametros.Add("i_idNave", _nave);
            
            var obj = sql_puntero.ExecuteSelectControl<tarjaDet>(nueva_conexion, 4000, "[brbk].consultarTarjaDetPorNave", parametros, out OnError);
            try
            {
                foreach (tarjaDet oDet in obj) {
                    oDet.Estados = estados.GetEstado(oDet.estado);
                    oDet.carga = string.Format("{0}-{1}-{2}", oDet.mrn, oDet.msn, oDet.hsn);
                    oDet.consigna = string.Format("{0} {1} ", oDet.idConsignatario.Trim(), oDet.Consignatario);
                }
                
            }
            catch { }

            return obj;
        }

        public Int64? Save_Update(out string OnError)
        {
            if (this.idTarjaDet > 0)
            {
                OnInit("N4Middleware");
            }
            parametros.Clear();
            parametros.Add("i_idTarjaDet", this.idTarjaDet);
            parametros.Add("i_idTarja", this.idTarja);
            parametros.Add("i_bl", this.bl);
            parametros.Add("i_mrn", this.mrn);
            parametros.Add("i_msn", this.msn);
            parametros.Add("i_hsn", this.hsn);
            parametros.Add("i_imo", this.imo);
            parametros.Add("i_idConsignatario", this.idConsignatario);
            parametros.Add("i_Consignatario", this.Consignatario);
            parametros.Add("i_carrier_id", this.carrier_id);
            parametros.Add("i_productoEcuapass", this.productoEcuapass);
            parametros.Add("i_idProducto", this.idProducto);
            parametros.Add("i_idManiobra", this.idManiobra);
            parametros.Add("i_idManiobra2", this.idManiobra2);
            parametros.Add("i_idItem", this.idItem);
            parametros.Add("i_idCondicion", this.idCondicion);
            parametros.Add("i_cantidad", this.cantidad);
            parametros.Add("i_kilos", this.kilos);
            parametros.Add("i_cubicaje", this.cubicaje);
            parametros.Add("i_descripcion", this.descripcion);
            parametros.Add("i_contenido", this.contenido);
            parametros.Add("i_observacion", this.observacion);
            parametros.Add("i_tonelaje", this.tonelaje);
            parametros.Add("i_ubicacion", this.ubicacion);
            parametros.Add("i_estado", this.estado);
            parametros.Add("i_usuarioCrea", this.usuarioCrea);
            parametros.Add("i_usuarioModifica", this.usuarioModifica);

            var db = sql_puntero.ExecuteInsertUpdateDeleteReturn(nueva_conexion, 6000, "[brbk].insertarTarjaDet", parametros, out OnError);
            if (!db.HasValue || db.Value < 0)
            {
                return null;
            }
            OnError = string.Empty;
            return db.Value;

        }

        public Int64? Update_IMDT(out string OnError)
        {
            if (this.idTarjaDet > 0)
            {
                OnInit("N4Middleware");
            }
            parametros.Clear();
            parametros.Add("i_idTarjaDet", this.idTarjaDet);
            parametros.Add("i_imdt", this.imdt);
            parametros.Add("i_imdt_num", this.imdt_num);
            parametros.Add("i_usuario_imdt", this.usuario_imdt);

            var db = sql_puntero.ExecuteInsertUpdateDeleteReturn(nueva_conexion, 6000, "[brbk].updateTarjaDet_IMDT", parametros, out OnError);
            if (!db.HasValue || db.Value < 0)
            {
                return null;
            }
            OnError = string.Empty;
            return db.Value;

        }

        public Int64? Update_BL_N4(string solictud, string respuesta,out string OnError)
        {
            if (this.idTarjaDet > 0)
            {
                OnInit("N4Middleware");
            }
            parametros.Clear();
            parametros.Add("i_idTarjaDet", this.idTarjaDet);
            parametros.Add("i_n4", this.n4);
            parametros.Add("i_usuario_n4", this.usuario_n4);
            parametros.Add("i_gKey_unit_BL", this.gKey_unit_BL);
            parametros.Add("i_solicitud_xml", solictud);
            parametros.Add("i_respuesta_xml", respuesta);

            var db = sql_puntero.ExecuteInsertUpdateDeleteReturn(nueva_conexion, 6000, "[brbk].updateTarjaDet_N4", parametros, out OnError);
            if (!db.HasValue || db.Value < 0)
            {
                return null;
            }
            OnError = string.Empty;
            return db.Value;

        }

        public static int consultaSecuenciaIMDT(string _mrn, string _msn, string _hsn)
        {
            OnInit("N4Middleware");
            SqlConnection cn = new SqlConnection(nueva_conexion);

            var d = new DataTable();
            using (var c = cn)
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandText = "[brbk].consultarSecuenciaIMDT";
                coman.Parameters.AddWithValue("@i_mrn", _mrn);
                coman.Parameters.AddWithValue("@i_msn", _msn);
                coman.Parameters.AddWithValue("@i_hsn", _hsn);
                try
                {
                    c.Open();
                    d.Load(coman.ExecuteReader(CommandBehavior.CloseConnection));
                }
                catch
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
            return int.Parse(d.Rows[0].ItemArray[0].ToString());
        }

        public static DataTable rptConsultaPredescargaFinal(string _nave, out string OnError)
        {
            OnInit("N4Middleware");
            SqlConnection cn = new SqlConnection(nueva_conexion);
            OnError = string.Empty;
            var d = new DataTable();
            using (var c = cn)
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandText = "[brbk].rptPredescargaFinal";
                coman.Parameters.AddWithValue("@i_idNave", _nave);
                try
                {
                    c.Open();
                    d.Load(coman.ExecuteReader(CommandBehavior.CloseConnection));
                }
                catch (Exception ex)
                {
                    OnError = ex.Message;
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

            if (OnError != string.Empty)
            {
                return null;
            }
                        
            return d;
        }

        public static DataTable rptConsultaLiquidacionNave(string _nave, out string OnError)
        {
            OnInit("N4Middleware");
            SqlConnection cn = new SqlConnection(nueva_conexion);
            OnError = string.Empty;
            var d = new DataTable();
            using (var c = cn)
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandText = "[brbk].rptLiquidacionBuque";
                coman.Parameters.AddWithValue("@i_idNave", _nave);
                try
                {
                    c.Open();
                    d.Load(coman.ExecuteReader(CommandBehavior.CloseConnection));
                }
                catch (Exception ex)
                {
                    OnError = ex.Message;
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

            if (OnError != string.Empty)
            {
                return null;
            }

            return d;

        }

        public static DataTable rptConsultaInventario(string _nave,string _bodega, out string OnError)
        {
            OnInit("N4Middleware");
            SqlConnection cn = new SqlConnection(nueva_conexion);
            OnError = string.Empty;
            var d = new DataTable();
            using (var c = cn)
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandText = "brbk.rptInventario";
                coman.Parameters.AddWithValue("@i_idNave", _nave);
                coman.Parameters.AddWithValue("@i_bodega", _bodega);
                try
                {
                    c.Open();
                    d.Load(coman.ExecuteReader(CommandBehavior.CloseConnection));
                }
                catch (Exception ex)
                {
                    OnError = ex.Message;
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

            if (OnError != string.Empty)
            {
                return null;
            }

            return d;

        }

        public static DataTable rptConsultaTarjaBodega(string _mrn, out string OnError)
        {
            OnInit("N4Middleware");
            SqlConnection cn = new SqlConnection(nueva_conexion);
            OnError = string.Empty;
            var d = new DataTable();
            using (var c = cn)
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandText = "[brbk].rptTarjaBodega";
                coman.Parameters.AddWithValue("@i_mrn", _mrn);
                try
                {
                    c.Open();
                    d.Load(coman.ExecuteReader(CommandBehavior.CloseConnection));
                }
                catch (Exception ex)
                {
                    OnError = ex.Message;
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

            if (OnError != string.Empty)
            {
                return null;
            }

            return d;

        }

        public static List<tarjaDet> rptIngresosParciales(string _nave, out string OnError)
        {
            OnInit("N4Middleware");
            parametros.Clear();
            parametros.Add("i_idNave", _nave);

            var obj = sql_puntero.ExecuteSelectControl<tarjaDet>(nueva_conexion, 4000, "[brbk].consultarTarjaDetPorNave", parametros, out OnError);
            try
            {
                foreach (tarjaDet oDet in obj)
                {
                    oDet.Estados = estados.GetEstado(oDet.estado);
                    oDet.carga = string.Format("{0}-{1}-{2}", oDet.mrn, oDet.msn, oDet.hsn);
                    oDet.consigna = string.Format("{0} {1} ", oDet.idConsignatario.Trim(), oDet.Consignatario);
                    oDet.tarjaCab = tarjaCab.GetTarjaCab(long.Parse(oDet.idTarja.ToString()));
                    oDet.producto = productos.GetProducto(oDet.idProducto);
                }

            }
            catch { }

            return obj;
        }

    }
}

