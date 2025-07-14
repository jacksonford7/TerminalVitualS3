using csl_log;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml;
using System.Xml.Linq;

namespace CSLSite.app_start
{
    public class ecuapass_dae
    {

       

        public string DeclarationOfficeID { get; set; }
        public string DeclarationNumber { get; set; }
        public string ExporterDocumentID { get; set; }
        public string ExporterName { get; set; }
        public string AgentDocumentID { get; set; }
        public string AgentID { get; set; }
        public string AgentName { get; set; }
        public int TransportEquipmentQuantity { get; set; }
        public string UnloadingLocation { get; set; }
        public string BallastOrCargoTypeCode { get; set; }
        public string DestinationID { get; set; }
        public string StatusID { get; set; }
        public string ExaminationChannelID { get; set; }
        public string ExaminerName { get; set; }
        public DateTime EffectiveEndDate { get; set; }
        


        public ecuapass_dae
            (string _DeclarationOfficeID,
            string _DeclarationNumber,
            string _ExporterDocumentID,
            string _ExporterName,
            string _AgentID,
            string _AgentDocumentID,
            string _AgentName,
            int _TransportEquipmentQuantity,
            string _UnloadingLocation,
            string _BallastOrCargoTypeCode,
            string _DestinationID,
            string _StatusID,
            string _ExaminationChannelID,
            string _ExaminerName,
          
            DateTime _EffectiveEndDate

            )
        {
            this.DeclarationOfficeID = _DeclarationOfficeID;
            this.DeclarationNumber = _DeclarationNumber;
            this.ExporterDocumentID = _ExporterDocumentID;
            this.ExporterName = _ExporterName;

            this.AgentID = _AgentID;
            this.AgentDocumentID = _AgentDocumentID;
            this.AgentName = _AgentName;

            this.TransportEquipmentQuantity = _TransportEquipmentQuantity;
            this.UnloadingLocation = _UnloadingLocation;
            this.BallastOrCargoTypeCode = _BallastOrCargoTypeCode;
            this.DestinationID = _DestinationID;
            this.StatusID = _StatusID;
            this.ExaminationChannelID = _ExaminationChannelID;
            this.ExaminerName = _ExaminerName;
            this.EffectiveEndDate = _EffectiveEndDate;
         
        }
        public ecuapass_dae()
        {

        }

        
        public Int64 Guardar(string responseXML)
        {
            try
            {
                var cn = System.Configuration.ConfigurationManager.ConnectionStrings["ecuapass"]?.ConnectionString;
                if (cn == null)
                {
                    return -4;
                }
                using (var conexion = new SqlConnection(cn))
                {
                    try
                    {
                        using (var comando = conexion.CreateCommand())
                        {
                            comando.CommandType = CommandType.StoredProcedure;
                            comando.CommandText = "pc_insertar_dae_s3";
                            comando.Parameters.AddWithValue("@XML_RESPUESTA", responseXML);
                            comando.Parameters.AddWithValue("@TIPO_DOC", this.DeclarationOfficeID);
                            comando.Parameters.AddWithValue("@DAE", this.DeclarationNumber);
                            comando.Parameters.AddWithValue("@ID_EXPORTADOR", this.ExporterDocumentID);
                            comando.Parameters.AddWithValue("@NOMBRE_EXPORTADOR", this.ExporterName);
                            comando.Parameters.AddWithValue("@ID_AGENTE", this.AgentID);
                            comando.Parameters.AddWithValue("@AGENTE", this.AgentDocumentID);
                            comando.Parameters.AddWithValue("@NOMBRE_AGENTE", this.AgentName);
                            comando.Parameters.AddWithValue("@CANTIDAD", this.TransportEquipmentQuantity);
                            comando.Parameters.AddWithValue("@PUERTO", this.UnloadingLocation);
                            comando.Parameters.AddWithValue("@TIPO_CARGA", this.BallastOrCargoTypeCode);
                            comando.Parameters.AddWithValue("@DestinationID", this.DestinationID);
                            comando.Parameters.AddWithValue("@StatusDeclaration", this.StatusID);
                            comando.Parameters.AddWithValue("@CANAL_AFORO", this.ExaminerName);
                            comando.Parameters.AddWithValue("@ID_AFORO", this.ExaminationChannelID);
                            comando.Parameters.AddWithValue("@EffectiveEndDate", this.EffectiveEndDate);
                            conexion.Open();
                            object sale = comando.ExecuteScalar();
                            conexion.Close();
                            if (sale != null && sale.GetType() != typeof(DBNull))
                            {
                                return Int64.Parse(sale.ToString());
                            }
                            else
                            {
                                return -2;
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
                        var t = log_csl.save_log<ApplicationException>(new ApplicationException(sb.ToString()), "ecuapass_dae", "Guardar", this.DeclarationNumber, "sistema");
                        return t*-1;
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
                var t = log_csl.save_log<Exception>(ex, "ecuapass_dae", "Guardar", this.DeclarationNumber, "sistema"); 
                return t*-1;
            }
        }

        public static ecuapass_dae conver_to_dae(XDocument xml)
        {
            if (xml == null)
            {
                return null;
            }
            var r = new ecuapass_dae();
            XElement cursor;
            cursor = xml.Descendants("DeclarationOfficeID").FirstOrDefault();
            r.DeclarationOfficeID = cursor != null ? cursor.Value : "VALOR NULO";
            /*---------------------------------------------------------------*/
            cursor = xml.Descendants("DeclarationNumber").FirstOrDefault();
            r.DeclarationNumber = cursor != null ? cursor.Value : "VALOR NULO";
            /*---------------------------------------------------------------*/
            cursor = xml.Descendants("Exporter").FirstOrDefault()?.Descendants("DocumentID").FirstOrDefault();
            r.ExporterDocumentID = cursor != null ? cursor.Value : "VALOR NULO";
            /*---------------------------------------------------------------*/
            cursor = xml.Descendants("Exporter").FirstOrDefault()?.Descendants("Name").FirstOrDefault();
            r.ExporterName= cursor != null ? cursor.Value : "VALOR NULO";
            /*---------------------------------------------------------------*/
            cursor = xml.Descendants("Agent").FirstOrDefault()?.Descendants("ID").FirstOrDefault();
            r.AgentID = cursor != null ? cursor.Value : "VALOR NULO";
            /*---------------------------------------------------------------*/
            cursor = xml.Descendants("Agent").Descendants("DocumentID").FirstOrDefault();
            r.AgentDocumentID = cursor != null ? cursor.Value : "VALOR NULO";
            /*---------------------------------------------------------------*/
            cursor = xml.Descendants("Agent").Descendants("Name").FirstOrDefault();
            r.AgentName= cursor != null ? cursor.Value : "VALOR NULO";
            /*---------------------------------------------------------------*/
            int c =0;
            var tval = xml.Descendants("TransportEquipmentQuantity").FirstOrDefault();
            if (int.TryParse(tval.Value, out c))
            {
                r.TransportEquipmentQuantity = c;
            }
            else
            {
                r.TransportEquipmentQuantity = 0;
            }
            /*---------------------------------------------------------------*/
            cursor = xml.Descendants("UnloadingLocation").FirstOrDefault();
            r.UnloadingLocation = cursor != null ? cursor.Value : "VALOR NULO";
            /*---------------------------------------------------------------*/
            cursor = xml.Descendants("BallastOrCargoTypeCode").FirstOrDefault();
            r.BallastOrCargoTypeCode = cursor != null ? cursor.Value : "VALOR NULO";
            /*---------------------------------------------------------------*/
            cursor = xml.Descendants("DestinationID").FirstOrDefault();
            r.DestinationID = cursor != null ? cursor.Value : "VALOR NULO";
            /*---------------------------------------------------------------*/
            cursor = xml.Descendants("StatusID").FirstOrDefault();
            r.StatusID = cursor != null ? cursor.Value : "VALOR NULO";
            /*---------------------------------------------------------------*/
            cursor = xml.Descendants("ExaminationChannelID").FirstOrDefault();
            r.ExaminationChannelID = cursor != null ? cursor.Value : "VALOR NULO";
            /*---------------------------------------------------------------*/
            cursor = xml.Descendants("ExaminerName").FirstOrDefault();
            r.ExaminerName = cursor != null ? cursor.Value : "VALOR NULO";
            /*---------------------------------------------------------------*/

            DateTime xc;
            CultureInfo enUS = new CultureInfo("en-US");
            var xdate = xml.Descendants("EffectiveEndDate").FirstOrDefault();
            if(DateTime.TryParseExact(xdate.Value, "dd/MM/yyyy", enUS, DateTimeStyles.None, out xc))
            {
                r.EffectiveEndDate = xc;
            }
            else
            {
                r.EffectiveEndDate = DateTime.Now;
            }
            return r;
        }

        public static bool buscar_dae_ecuapass(string dae, string ruta, out string novedad)
        {
            string ec_usuario = System.Configuration.ConfigurationManager.AppSettings["ec_usuario"] as string;
            string ec_password = System.Configuration.ConfigurationManager.AppSettings["ec_password"] as string;
            string ec_oce = System.Configuration.ConfigurationManager.AppSettings["ecu_oce"] as string;
            string ec_ruc = System.Configuration.ConfigurationManager.AppSettings["ecu_ruc"] as string;
            //crear consulta
            var ec_obj = new EcuapassConsulta(ruta);
            ec_obj.clave_ecuapass = ec_password;
            ec_obj.usuario_ecuapass = ec_usuario;
            ec_obj.codigo_oce = ec_oce;
            ec_obj.ruc_oce = ec_ruc;
            //hacer la petición
            var ec_con = new EcuapassConector("http://cdes.aduana.gob.ec/cdes_svr/SENAE_ExportDespachoService_EC", "requestExportDespachoData");
            //
            if (ec_con == null)
            {
                novedad = "No fue posible conectar con los servicios del ecuapass";
                return false;
            }
            var xDx = new XmlDocument();
            string c = ec_obj.ConsultaDAE(dae);
            xDx.LoadXml(c);
            //al ecuapass
            var xi = ec_con.IniciarPeticion(xDx);
            if (xi == null)
            {
                novedad = "La respuesta de los servicios de ecuapass esta muy lenta";
                return false;
            }
            XElement xData = xi.Descendants().Where(x => x.Name.LocalName == "Data").FirstOrDefault();
            if (xData == null)
            {
                novedad = string.Format("Verifique en el Ecuapass que la DAE se encuentre generada y que el almacén de lugar de partida sea Contecon Guayaquil S.A. ", dae);
                return false;
            }
            XDocument x_Dae = new XDocument();
            try
            {
               x_Dae = XDocument.Parse(xData.Value);
            }
            catch (XmlException)
            {
                var s = xData.Value.Replace("&", " Y ");
                xData.Value = s;
                x_Dae = XDocument.Parse(xData.Value);
            }

            var dr = ecuapass_dae.conver_to_dae(x_Dae);
            var k = dr.Guardar(xi.ToString());
            if (k < 0)
            {
                novedad = string.Format("Fue imposible almacenar la información en nuestros registros documento {0}, por favor intente mas tarde", dae);
                return false;
            }
            novedad = string.Format("La DAE {0} esta lista para la generación del AISV",dae);
            return true;
        }

        public static bool ValidarDAE(string DAE)
        {
            try
            {
                var cn = System.Configuration.ConfigurationManager.ConnectionStrings["ecuapass"]?.ConnectionString;
                if (cn == null)
                {
                    return false;
                }
                using (var conexion = new SqlConnection(cn))
                {
                    try
                    {
                        using (var comando = conexion.CreateCommand())
                        {
                            comando.CommandType = CommandType.StoredProcedure;
                            comando.CommandText = "fna_fun_valida_dae_sav";
                            comando.Parameters.AddWithValue("@dae", DAE);
                            
                            conexion.Open();
                            var sale = comando.ExecuteScalar();
                            conexion.Close();
                            if (sale != null && sale.GetType() != typeof(DBNull))
                            {
                                return bool.Parse(sale.ToString());
                            }
                            else
                            {
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
                        var t = log_csl.save_log<ApplicationException>(new ApplicationException(sb.ToString()), "ecuapass_dae", "ValidarDAE", DAE, "sistema");
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
                var t = log_csl.save_log<Exception>(ex, "ecuapass_dae", "ValidarDAE", DAE, "sistema");
                return false;
            }
        }
    }
}