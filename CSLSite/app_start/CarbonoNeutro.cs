using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace CSLSite.app_start
{
    public class CarbonoNeutro
    {
        public  string ruc_exportador { get; set; }
        public string email_exportador { get; set; }
        public string email_exportador1 { get; set; }
        public string email_exportador2{ get; set; }
        public string email_exportador3 { get; set; }
        public string email_exportador4 { get; set; }
        public string nombres_exportador { get; set; }

        public string aisv_numero { get; set; }
        public string aisv_contenedor { get; set; }
        public string aisv_proforma { get; set; }
        public string aisv_login { get; set; }
        public string aisv_referencia { get; set; }
        public string aisv_booking { get; set; }
        public string aisv_producto { get; set; }

        public string cert_tipo { get; set; }
        public string cert_secuencia { get; set; }
        public string cert_numero { get; set; }
        public string cert_proceso_estado { get; set; }
        public bool  cert_valido { get; set; }
        public DateTime? cert_generado { get; set; }

        public Int64? unidad_gkey { get; set; }
        public string unidad_buque { get; set; }
        public string unidad_viaje { get; set; }
        public DateTime? unidad_fecha_embarque { get; set; }
        public DateTime? unidad_fecha_ingreso { get; set; }


        public Int64 id { get; set; }
        public DateTime fecha_registro { get; set; }
        public CarbonoNeutro()
        {
                
        }
        public CarbonoNeutro(string _ruc_exportador,string _email_exportador,string _nombres_exportador )
        {
            this.ruc_exportador = _ruc_exportador;
            this.email_exportador = _email_exportador;
            this.nombres_exportador = _nombres_exportador;
        }
        public bool Grabar(out string novedad)
        {
            try
            {
                if (string.IsNullOrEmpty(ruc_exportador))
                {
                    novedad = string.Format("Campo no debe ser nulo {0}",nameof(ruc_exportador));
                    return false;
                }
                if (string.IsNullOrEmpty(nombres_exportador))
                {
                    novedad = string.Format("Campo no debe ser nulo {0}", nameof(nombres_exportador));
                    return false;
                }
                if (string.IsNullOrEmpty(email_exportador))
                {
                    novedad = string.Format("Campo no debe ser nulo {0}", nameof(email_exportador));
                    return false;
                }
                if (string.IsNullOrEmpty(aisv_numero))
                {
                    novedad = string.Format("Campo no debe ser nulo {0}", nameof(aisv_numero));
                    return false;
                }
                if (string.IsNullOrEmpty(aisv_contenedor))
                {
                    novedad = string.Format("Campo no debe ser nulo {0}", nameof(aisv_contenedor));
                    return false;
                }
                if (string.IsNullOrEmpty(aisv_proforma))
                {
                    novedad = string.Format("Campo no debe ser nulo {0}", nameof(aisv_proforma));
                    return false;
                }
                if (string.IsNullOrEmpty(aisv_login))
                {
                    novedad = string.Format("Campo no debe ser nulo {0}", nameof(aisv_login));
                    return false;
                }
                if (string.IsNullOrEmpty(aisv_booking))
                {
                    novedad = string.Format("Campo no debe ser nulo {0}", nameof(aisv_booking));
                    return false;
                }
                if (string.IsNullOrEmpty(aisv_referencia))
                {
                    novedad = string.Format("Campo no debe ser nulo {0}", nameof(aisv_referencia));
                    return false;
                }
                if (string.IsNullOrEmpty(cert_tipo))
                {
                    novedad = string.Format("Campo no debe ser nulo {0}", nameof(cert_tipo));
                    return false;
                }


                var cn = System.Configuration.ConfigurationManager.ConnectionStrings["service"]?.ConnectionString;
                if (cn == null)
                {
                    novedad = string.Format("Cadena de conexion no existe {0}", "service");
                    return false;
                }
           

                using (var conexion = new SqlConnection(cn))
                {
                    try
                    {
                        using (var comando = conexion.CreateCommand())
                        {
                            comando.CommandType = CommandType.StoredProcedure;
                            comando.CommandText = "aisv_carbono_certificado_inserta";
                            comando.Parameters.AddWithValue(nameof(ruc_exportador), ruc_exportador);
                            comando.Parameters.AddWithValue(nameof(email_exportador), email_exportador);
                            comando.Parameters.AddWithValue(nameof(nombres_exportador), nombres_exportador);
                            comando.Parameters.AddWithValue(nameof(aisv_numero), aisv_numero);
                            comando.Parameters.AddWithValue(nameof(aisv_contenedor), aisv_contenedor);
                            comando.Parameters.AddWithValue(nameof(aisv_proforma), aisv_proforma);
                            comando.Parameters.AddWithValue(nameof(aisv_login), aisv_login);
                            comando.Parameters.AddWithValue(nameof(aisv_booking), aisv_booking);
                            comando.Parameters.AddWithValue(nameof(aisv_referencia), aisv_referencia);
                            comando.Parameters.AddWithValue(nameof(cert_tipo), cert_tipo);


                            comando.Parameters.AddWithValue(nameof(email_exportador1), email_exportador1);
                            comando.Parameters.AddWithValue(nameof(email_exportador2), email_exportador2);
                            comando.Parameters.AddWithValue(nameof(email_exportador3), email_exportador3);
                            comando.Parameters.AddWithValue(nameof(email_exportador4), email_exportador4);

                            if (!string.IsNullOrEmpty(aisv_producto))
                            {
                                comando.Parameters.AddWithValue(nameof(aisv_producto), aisv_producto);
                            }
                            conexion.Open();
                            object sale = comando.ExecuteScalar();
                            conexion.Close();
                            if (sale != null && sale.GetType() != typeof(DBNull))
                            {
                                Int64 se = 0;
                                if (!Int64.TryParse(sale.ToString() ,out se))
                                {
                                    novedad = string.Format("La conversion a entero ha fallado {0}", sale.ToString());
                                    return false;
                                }
                                this.id = se;
                                novedad = string.Empty;
                                return true;
                            }
                            else
                            {
                                novedad = "Inserción resulta nula";
                                return false;
                            }
                        }
                    }
                    catch (SqlException ex)
                    {
                        StringBuilder sb = new StringBuilder();
                        foreach (SqlError e in ex.Errors)
                        {
                            sb.AppendLine(string.Format("{0}, {1}, {2}, {3}", e.LineNumber, e.Message, e.Procedure, e.Server));
                        }
                        novedad = sb.ToString();
                        return false;
                    }
                    finally
                    {
                        if (conexion.State == ConnectionState.Open)
                        {
                            conexion.Close();
                        }
                        conexion.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                novedad = ex.Message;
                return false;
            }
        }
        public static string TipoCertificado(string proforma)
        {
            using (var con = new System.Data.SqlClient.SqlConnection())
            {
                //csl_services
                con.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["service"].ConnectionString;
                var comando = con.CreateCommand();
                comando.CommandType = System.Data.CommandType.Text;
                comando.Connection = con;
                comando.CommandText = "select [dbo].[aisv_carbono_tipo_certificado](@proforma)";
                comando.Parameters.AddWithValue("@proforma", proforma);
                try
                {
                    con.Open();
                    object sale = comando.ExecuteScalar();
                    con.Close();
                    if (sale != null && sale.GetType() != typeof(DBNull))
                    {
                        return sale.ToString();
                    }
                    else
                    {
                        return null;
                    }
                }
                catch
                {
                   
                    return null;
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
        public static Stream BarcodeStream(Int64 secuencia)
        {
            try
            {
                //configurar la pagina destino del link
                string url_destino = string.Format("https://www.cgsa.com.ec/carbono-neutro/",secuencia);
                //aca toda la URL--> cambiar esto cgdes19:8080
                string server_url = string.Format("https://{0}/barcode/handler/qr.ashx?code={1}&format=E9", "cgdes19:8080",url_destino);

                Stream stream = null;
               // string url = "http://cgdes19:8080/barcode/handler/qr.ashx?code=www.cgsa.com.ec?sid=1&format=E9";
               string url = server_url;
                System.Net.HttpWebRequest fileReq = (HttpWebRequest)HttpWebRequest.Create(url);
                HttpWebResponse fileResp = (HttpWebResponse)fileReq.GetResponse();
                if (fileReq.ContentLength > 0)
                { fileResp.ContentLength = fileReq.ContentLength; }
                stream = fileResp.GetResponseStream();
                return stream;
            }
            catch
            {
                return null;
            }
        }
        public static Stream BarcodeStream(Int64 secuencia, string destino_url, string barra_url)
        {
            try
            {
                //https://www.cgsa.com.ec/carbono-neutro/
                string url_destino = string.Format("{0}?{1}",destino_url,secuencia);
                string server_url = string.Format("http://{0}/barcode/handler/qr.ashx?code={1}&format=E9", barra_url, url_destino);
                Stream stream = null;
               string url = server_url;
                System.Net.HttpWebRequest fileReq = (HttpWebRequest)HttpWebRequest.Create(url);
                HttpWebResponse fileResp = (HttpWebResponse)fileReq.GetResponse();
                if (fileReq.ContentLength > 0)
                { fileResp.ContentLength = fileReq.ContentLength; }
                stream = fileResp.GetResponseStream();
                return stream;
            }
            catch
            {
                return null;
            }
        }

    }
}