using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using csl_log;
using System.Text;

namespace CSLSite.app_start
{
    public class ProformaHelper
    {
        //esto para la nueva clase.
        //este medoto confirma que el booking sea de dicha cantidad, solo si puso bookin SOLITO
        //ES PARA SABER QUE NO HAYA CAMBIADO
        public static bool confirmarBoking(Int64 bkey, int cantidad)
        {
            var nuevaQty = BookingQty(bkey);
            if (nuevaQty != cantidad)
            {
                return false;
            }
            return true;
        }
        //reconfirma que el booking actualmente tiene la misma cantidad sino debe otra vez generar proforma
        public static Int32 BookingQty(Int64 bkey)
        {
            using (var con = new System.Data.SqlClient.SqlConnection())
            {
                //n4catalog
                con.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["N5"].ConnectionString;
                var comando = con.CreateCommand();
                comando.CommandType = System.Data.CommandType.Text;
                comando.Connection = con;
                comando.CommandText = "select [N5].[dbo].[fx_bk_qyt](@bookingKey)";
                comando.Parameters.AddWithValue("@bookingKey", bkey);
                try
                {
                    con.Open();
                    object sale = comando.ExecuteScalar();
                    con.Close();
                    if (sale != null && sale.GetType() != typeof(DBNull))
                    {
                        return int.Parse(sale.ToString());
                    }
                    else
                    {
                        return 0;
                    }
                }
                catch (Exception ex)
                {
                    log_csl.save_log<Exception>(ex, "ProformaHelper", "BookingQty", bkey.ToString(), "N4");
                    return 0;
                }
                finally
                {
                    if (con.State == System.Data.ConnectionState.Open)
                    {
                        con.Close();
                    }
                    con.Dispose();
                    comando.Dispose();
                }
            }
        }
        public static string crateServiceTable(Catalogos.ExportacionServicesDataTable tabla, int cantidad)
        {
            StringBuilder salida = new StringBuilder();
            if (tabla.Rows.Count <= 0 || cantidad<=0)
            {
                return "Error!!";
            }
            salida.Append("<table id='miProforma' class='costo'  cellpadding='1' cellspacing='1'><thead><tr><th></th><th></th><th>Código</th><th>Descripcion</th><th>Cantidad</th><th>V.Unit</th><th>V.Total</th></tr></thead><tbody>");
          
            foreach (var f in tabla)
            {
                salida.Append("<tr>");
                if (f.IsnotaNull())
                {
                    salida.Append("<td>&nbsp;</td>");
                }
                else
                {
                    salida.AppendFormat("<td><a class='tooltip' ><span class='classic'>{0}</span><img alt='' src='../shared/imgs/info.gif' class='datainfo'/></a></td>", f.nota);
                }

                if (f.opcional)
                {
                    salida.AppendFormat("<td><input type='checkbox' id='___{0}' onclick='setVal(this)' checked='checked'/><input id='{0}' type='hidden' runat='server' clientidmode='Static' /></td>",f.codigo);
                }
                else
                {
                    salida.Append("<td><input type='checkbox'  disabled='disabled' checked='checked'/></td>");
                }
                salida.AppendFormat("<td>{0}</td>", f.codigo);
                salida.AppendFormat("<td>{0}</td>", f.descripcion);
                salida.AppendFormat("<td>{0}</td>", f.cantidad);
                salida.AppendFormat("<td>{0}</td>", f.costo.ToString("C"));
                salida.AppendFormat("<td>{0}</td>", (f.costo * cantidad).ToString("C"));
                salida.Append("</tr>");
            }
            salida.AppendFormat("<tr><td colspan='6' class='filat'>Total por unidad</td><td><span id='stunit' >{0}</span></td></tr>", 0);
            salida.AppendFormat("<tr><td colspan='6' class='filat'>SUBTOTAL</td><td><span id='ssubt'>{0}</span></td></tr>", 1);
            salida.AppendFormat("<tr><td colspan='6' class='filat'>IVA 12%(+)</td><td><span id='siva'>{0}</span></td></tr>", 2);
            salida.AppendFormat("<tr><td colspan='6' class='filat'>Retención en la Fuente 0%(-)</td><td><span id='sfuente' >{0}</span></td></tr>",3);
            salida.AppendFormat("<tr><td colspan='6' class='filat'>Retención del IVA 0%(-)</td><td><span id='srtiva' >{0}</span></td></tr>",4);
            salida.AppendFormat("<tr><td colspan='6' class='filat'>TOTAL</td><td><span id='sttal' >{0}</span></td></tr>",5);
            salida.AppendFormat("</tbody></table>");
            return salida.ToString();
        }
        public static Int32 AisvActivos(string booking, string proforma)
        {
            if (string.IsNullOrEmpty(booking) || string.IsNullOrEmpty(proforma))
            {
                return 0;
            }
            using (var con = new System.Data.SqlClient.SqlConnection())
            {
                //n4catalog
                con.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["service"].ConnectionString;
                var comando = con.CreateCommand();
                comando.CommandType = System.Data.CommandType.Text;
                comando.Connection = con;
                comando.CommandText = "select [csl_services].[dbo].[fx_aisv_por_booking_full](@boking)";
                comando.Parameters.AddWithValue("@boking", booking.Trim());
                comando.Parameters.AddWithValue("@proforma", proforma.Trim());
                try
                {
                    con.Open();
                    object sale = comando.ExecuteScalar();
                    con.Close();
                    if (sale != null && sale.GetType() != typeof(DBNull))
                    {
                        return int.Parse(sale.ToString());
                    }
                    else
                    {
                        return 0;
                    }
                }
                catch (Exception ex)
                {
                    log_csl.save_log<Exception>(ex, "ProformaHelper", "AisvActivos", booking ,"N4");
                    return -1;
                }
                finally
                {
                    if (con.State == System.Data.ConnectionState.Open)
                    {
                        con.Close();
                    }
                    con.Dispose();
                    comando.Dispose();
                }
            }
        }
        public static Int32 AisvActivos(string booking)
        {
            if (string.IsNullOrEmpty(booking))
            {
                return 0;
            }
            using (var con = new System.Data.SqlClient.SqlConnection())
            {
                //n4catalog
                con.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["service"].ConnectionString;
                var comando = con.CreateCommand();
                comando.CommandType = System.Data.CommandType.Text;
                comando.Connection = con;
                comando.CommandText = "select [csl_services].[dbo].[fx_aisv_only_bok](@boking)";
                comando.Parameters.AddWithValue("@boking", booking.Trim());
                try
                {
                    con.Open();
                    object sale = comando.ExecuteScalar();
                    con.Close();
                    if (sale != null && sale.GetType() != typeof(DBNull))
                    {
                        return int.Parse(sale.ToString());
                    }
                    else
                    {
                        return 0;
                    }
                }
                catch (Exception ex)
                {
                    log_csl.save_log<Exception>(ex, "ProformaHelper", "AisvActivos", booking, "N4");
                    return -1;
                }
                finally
                {
                    if (con.State == System.Data.ConnectionState.Open)
                    {
                        con.Close();
                    }
                    con.Dispose();
                    comando.Dispose();
                }
            }
        }
        //fx_total_proforma
        public static Int32 Totalproformas(Int64 gkey)
        {
            using (var con = new System.Data.SqlClient.SqlConnection())
            {
                //n4catalog
                con.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["service"].ConnectionString;
                var comando = con.CreateCommand();
                comando.CommandType = System.Data.CommandType.Text;
                comando.Connection = con;
                comando.CommandText = "select [csl_services].[dbo].[fx_total_proforma](@gkey)";
                comando.Parameters.AddWithValue("@gkey", gkey);
                try
                {
                    con.Open();
                    object sale = comando.ExecuteScalar();
                    con.Close();
                    if (sale != null && sale.GetType() != typeof(DBNull))
                    {
                        return int.Parse(sale.ToString());
                    }
                    else
                    {
                        return 0;
                    }
                }
                catch (Exception ex)
                {
                    log_csl.save_log<Exception>(ex, "ProformaHelper", "Totalproformas", gkey.ToString(), "N4");
                    return 0;
                }
                finally
                {
                    if (con.State == System.Data.ConnectionState.Open)
                    {
                        con.Close();
                    }
                    con.Dispose();
                    comando.Dispose();
                }
            }
        }
        public static Int32 Totalproformas(string boking)
        {
            if (string.IsNullOrEmpty(boking))
            {
                return 0;
            }
            using (var con = new System.Data.SqlClient.SqlConnection())
            {
                //n4catalog
                con.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["service"].ConnectionString;
                var comando = con.CreateCommand();
                comando.CommandType = System.Data.CommandType.Text;
                comando.Connection = con;
                comando.CommandText = "select [csl_services].[dbo].[fx_total_proforma_bo](@boking)";
                comando.Parameters.AddWithValue("@boking", boking.Trim());
                try
                {
                    con.Open();
                    object sale = comando.ExecuteScalar();
                    con.Close();
                    if (sale != null && sale.GetType() != typeof(DBNull))
                    {
                        return int.Parse(sale.ToString());
                    }
                    else
                    {
                        return 0;
                    }
                }
                catch (Exception ex)
                {
                    log_csl.save_log<Exception>(ex, "ProformaHelper", "TotalproformasBo", boking, "N4");
                    return 0;
                }
                finally
                {
                    if (con.State == System.Data.ConnectionState.Open)
                    {
                        con.Close();
                    }
                    con.Dispose();
                    comando.Dispose();
                }
            }
        }
    }
}