using SqlConexion;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Transactions;
using System.Web;

namespace CSLSite
{
    public class EDI_bookingCab : BillionEntidades.Cls_Bil_Base
    {
        public long? id { get; set; }
        public string number { get; set; }
        public string lineOperator { get; set; }
        public string vesselVisit { get; set; }
        public string portOfLoad { get; set; }
        public string portOfDischarge { get; set; }
        public string secportOfDischarge { get; set; }
        public string POD1 { get; set; }
        public string shipper { get; set; }
        public string consigneeId { get; set; }
        public string consignee { get; set; }
        public string freightKind { get; set; }
        public bool? overrideCutoff { get; set; }
        public string specialStow { get; set; }
        public string specialStow2 { get; set; }
        public string specialStow3 { get; set; }
        public string notes { get; set; }
        public string estado { get; set; }
        public string usuarioCrea { get; set; }
        public DateTime? fechaCreacion { get; set; }
        public string usuarioModifica { get; set; }
        public DateTime? fechaModifica { get; set; }
        public long? docRef { get; set; }
        public List<EDI_bookingDet> EDI_bookingDet { get; set; }
        public List<EDI_hazard> EDI_hazard { get; set; }
        public List<EDI_bookingArchivo> EDI_bookingArchivo { get; set; }
        public List<EDI_Vessel> EDI_Vessel { get; set; }

        #region "Constructores"
        public EDI_bookingCab()
        {
            base.init();
        }
        #endregion

        #region "Metodos"
        private static void OnInit(string Base)
        {
            sql_puntero = (sql_puntero == null) ? Cls_Conexion.Conexion() : sql_puntero;
            parametros = new Dictionary<string, object>();
            nueva_conexion = Cls_Conexion.Nueva_Conexion(Base);
        }

        private int? PreValidations(out string msg)
        {

            if (string.IsNullOrEmpty(this.number))
            {
                msg = "Especifique el number";
                return 0;
            }

            if (string.IsNullOrEmpty(this.vesselVisit))
            {
                msg = "Especifique el Vessel Visit";
                return 0;
            }

            if (string.IsNullOrEmpty(this.portOfDischarge))
            {
                msg = "Especifique el Port Of Discharge";
                return 0;
            }

            if (string.IsNullOrEmpty(this.portOfLoad))
            {
                msg = "Especifique el Port Of Load";
                return 0;
            }

            if (string.IsNullOrEmpty(this.lineOperator))
            {
                msg = "Especifique el Line Operator";
                return 0;
            }

            if (string.IsNullOrEmpty(this.freightKind))
            {
                msg = "Especifique el Freight Kind";
                return 0;
            }

            msg = string.Empty;
            return 1;
        }

        private static SqlConnection conexion()
        {
            return new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["midle"].ConnectionString);
        }

        public static List<EDI_bookingCab> ListBookingCab(DateTime? _desde, DateTime? _hasta, string _container)
        {
            OnInit("N4Middleware");
            string msg;
            parametros.Clear();
            parametros.Add("i_fechaDesde", _desde);
            parametros.Add("i_fechaHasta", _hasta);
            return sql_puntero.ExecuteSelectControl<EDI_bookingCab>(nueva_conexion, 2000, "EDI_bookingCab_consultar", parametros, out msg);
        }

        public static List<EDI_bookingCab> ListBookingCabFilter(string _usuario, string _number)
        {
            OnInit("N4Middleware");
            string msg;
            parametros.Clear();
            parametros.Add("i_usuarioCrea", _usuario);
            parametros.Add("i_number", string.IsNullOrEmpty(_number) ? null : _number);
            return sql_puntero.ExecuteSelectControl<EDI_bookingCab>(nueva_conexion, 2000, "EDI_bookingCab_Filter", parametros, out msg);
        }

        public static DataTable DtBookingCabFilter(string _usuario, string _number)
        {
            var d = new DataTable();
            using (var c = conexion())
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                coman.CommandText = "EDI_bookingCab_Filter";
                coman.Parameters.AddWithValue("@i_usuarioCrea", _usuario);
                coman.Parameters.AddWithValue("@i_number", string.IsNullOrEmpty(_number)? null : _number);
                try
                {
                    c.Open();
                    d.Load(coman.ExecuteReader(CommandBehavior.CloseConnection));
                }
                catch (SqlException ex)
                {
                    csl_log.log_csl.save_log<SqlException>(ex, "turno", "EDI_bookingCab_Filter", DateTime.Now.ToString(), "GetBookingsZAL_");
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
            return d;
        }


        public static EDI_bookingCab GetBookingCab(long _id, out string msg)
        {
            OnInit("N4Middleware");
            parametros.Clear();
            parametros.Add("i_id", _id);
            var resultado = sql_puntero.ExecuteSelectControl<EDI_bookingCab>(nueva_conexion, 2000, "EDI_bookingCab_consultar", parametros, out msg).FirstOrDefault();

            if (resultado != null)
            {
                if (resultado.id > 0)
                {
                    resultado.EDI_bookingDet = CSLSite.EDI_bookingDet.ListBookingDet(_id);
                    resultado.EDI_hazard = CSLSite.EDI_hazard.ListHazard(_id); 
                }
            }

            return resultado;
        }

        public static EDI_bookingCab GetBookingCabPorNumber(string _number, out string msg)
        {
            OnInit("N4Middleware");
            parametros.Clear();
            parametros.Add("i_number", _number);
            var resultado = sql_puntero.ExecuteSelectControl<EDI_bookingCab>(nueva_conexion, 2000, "EDI_bookingCab_consultar", parametros, out msg).FirstOrDefault();

            if (resultado != null)
            {
                if (resultado.id > 0)
                {
                    long _id = long.Parse(resultado.id.ToString());
                    resultado.EDI_bookingDet = CSLSite.EDI_bookingDet.ListBookingDet(_id);
                    resultado.EDI_hazard = CSLSite.EDI_hazard.ListHazard(_id);
                    resultado.EDI_bookingArchivo = CSLSite.EDI_bookingArchivo.ListaArchivos(_id);
                }
            }

            return resultado;
        }

        public static string GetBookingCabPorUser(string _usuario, out string msg)
        {
            OnInit("N4Middleware");
            parametros.Clear();
            parametros.Add("i_usuarioCrea", _usuario);
            string resultado = sql_puntero.ExecuteSelectOnlyString(nueva_conexion, 2000, "EDI_bookingCab_getNumber", parametros, out msg);

            return resultado;
        }

        public Int64? Save_Update(out string OnError)
        {
            OnInit("N4Middleware");
                                                                                                         			
            //using (var scope = new TransactionScope())                                                 	
            //{                                                                                          		
            parametros.Clear();
            parametros.Add("i_id", this.id);
            parametros.Add("i_number", this.number);                                                	
            parametros.Add("i_lineOperator", this.lineOperator);                                           
            parametros.Add("i_vesselVisit", this.vesselVisit);                                        			
            parametros.Add("i_portOfLoad", this.portOfLoad);                                               			
            parametros.Add("i_portOfDischarge", this.portOfDischarge);                                      		
            parametros.Add("i_2portOdDischarge", this.secportOfDischarge);                                      		
            parametros.Add("i_POD1", this.POD1);                                                      	
            parametros.Add("i_shipper", this.shipper);
            parametros.Add("i_consigneeId", this.consigneeId);
            parametros.Add("i_consignee", this.consignee);                                               	
            parametros.Add("i_freightKind", this.freightKind);                                                
            parametros.Add("i_overrideCutoff", this.overrideCutoff);                                          			
            parametros.Add("i_specialStow", this.specialStow);
            parametros.Add("i_specialStow2", this.specialStow2);
            parametros.Add("i_specialStow3", this.specialStow3);
            parametros.Add("i_notes", this.notes);
            parametros.Add("i_estado", this.estado);
            parametros.Add("i_usuarioCrea", this.usuarioCrea);
            parametros.Add("i_usuarioModifica", this.usuarioModifica);
            parametros.Add("i_idCopy", this.docRef);

            var db = sql_puntero.ExecuteInsertUpdateDeleteReturn(nueva_conexion, 6000, "EDI_bookingCab_insert", parametros, out OnError);
            if (!db.HasValue || db.Value < 0)
            {
                return null;
            }
            OnError = string.Empty;

                //foreach (var oItem in EDI_bookingDet)
                //{
                //    oItem.idBooking = db.Value;
                //    oItem.usuarioCrea = this.usuarioCrea;
                //    var dbItem = oItem.Save_Update(out OnError);

                //    if (!dbItem.HasValue || dbItem.Value < 0)
                //    {
                //        return null;
                //    }
                //}
                //scope.Complete();
            return db.Value;
            //}


        }

        //MASIVO
        public static string GetBookingValidations(string xml, out string msg)
        {
            OnInit("N5");
            parametros.Clear();
            parametros.Add("xmlData", xml);
            string resultado = sql_puntero.ExecuteSelectOnlyString(nueva_conexion, 2000, "[validations].[pc_booking_complete]", parametros, out msg);

            return resultado;
        }

        public static Int64? Save_Masivo(string Number, string Liner, string Vsl_Name, string Voyage_IB, string Voyage_OB, string POD, string POD1, string OPOD1, string FKind,
            string Shipper, string STW, string IMO, string IT_QTY, string IT_ISO, string IT_COMM, string IT_TEMP, string IT_VENT, string IT_CO2, string IT_HUMM,
            string IT_O2, string CARRIER_VISIT, string PROCESS_EMAIL, string PROCESS_SEQUENCE, string CREATE_USER, out string OnError)
        {
            OnInit("N5");

            //using (var scope = new TransactionScope())                                                 	
            //{                                                                                          		
            parametros.Clear();
            parametros.Add("Number", Number);
            parametros.Add("Liner", Liner);
            parametros.Add("Vsl_Name ", Vsl_Name);
            parametros.Add("Voyage_IB", Voyage_IB);
            parametros.Add("Voyage_OB", Voyage_OB);
            parametros.Add("POD", POD);
            parametros.Add("POD1", POD1);
            parametros.Add("OPOD1", OPOD1);
            parametros.Add("FKind", FKind);
            parametros.Add("Shipper", Shipper);
            parametros.Add("STW ", STW);
            parametros.Add("IMO ", IMO);
            parametros.Add("IT_QTY", decimal.Parse(IT_QTY));
            parametros.Add("IT_ISO", IT_ISO);
            parametros.Add("IT_COMM", IT_COMM);
            parametros.Add("IT_TEMP", decimal.Parse(IT_TEMP));
            parametros.Add("IT_VENT", decimal.Parse(IT_VENT));
            parametros.Add("IT_CO2", decimal.Parse(IT_CO2));
            parametros.Add("IT_HUMM", decimal.Parse(IT_HUMM));
            parametros.Add("IT_O2", IT_O2);

            parametros.Add("CARRIER_VISIT", CARRIER_VISIT);
            parametros.Add("PROCESS_EMAIL", PROCESS_EMAIL);
            parametros.Add("PROCESS_SEQUENCE", decimal.Parse(PROCESS_SEQUENCE));
            parametros.Add("CREATE_USER", CREATE_USER);

            var db = sql_puntero.ExecuteInsertUpdateDeleteReturn(nueva_conexion, 6000, "validations.Insertar_Booking", parametros, out OnError);
            if (!db.HasValue || db.Value < 0)
            {
                return null;
            }
            OnError = string.Empty;

            //foreach (var oItem in EDI_bookingDet)
            //{
            //    oItem.idBooking = db.Value;
            //    oItem.usuarioCrea = this.usuarioCrea;
            //    var dbItem = oItem.Save_Update(out OnError);

            //    if (!dbItem.HasValue || dbItem.Value < 0)
            //    {
            //        return null;
            //    }
            //}
            //scope.Complete();
            return db.Value;
            //}


        }

        public static DataView ConsultarListaNaves()
        {
            OnInit("N5");
            SqlConnection cn = new SqlConnection(nueva_conexion);

            var d = new DataTable();
            using (var c = cn)
            {
                var coman = c.CreateCommand();
                coman.CommandType = CommandType.StoredProcedure;
                //coman.Parameters.AddWithValue("@i_rucCliente", txtRucCliente);
                coman.CommandText = "validations.active_visits";
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

        #endregion
    }

    public class EDI_bookingArchivo : BillionEntidades.Cls_Bil_Base
    {
        public long? id { get; set; }
        public long? idBooking { get; set; }
        public string filename { get; set; }
        public string path { get; set; }
        public bool estado { get; set; }
        public string usuarioCrea { get; set; }
        public DateTime? fechaCreacion { get; set; }
        public string usuarioModifica { get; set; }
        public DateTime? fechaModifica { get; set; }

        #region "Constructores"
        public EDI_bookingArchivo()
        {
            base.init();
        }
        #endregion

        #region "Metodos"
        private static void OnInit(string Base)
        {
            sql_puntero = (sql_puntero == null) ? Cls_Conexion.Conexion() : sql_puntero;
            parametros = new Dictionary<string, object>();
            nueva_conexion = Cls_Conexion.Nueva_Conexion(Base);
        }


        public static List<EDI_bookingArchivo> ListaArchivos(long _idBooking)
        {
            OnInit("N4Middleware");
            string msg;
            parametros.Clear();
            parametros.Add("i_idbooking", _idBooking);
            return sql_puntero.ExecuteSelectControl<EDI_bookingArchivo>(nueva_conexion, 2000, "EDI_bookingArchivo_consultar", parametros, out msg);
        }

        public Int64? Save_Update(out string OnError)
        {
            parametros.Clear();
            OnInit("N4Middleware");
            if (this.id > 0)
            {
                parametros.Add("i_id", this.id);
            }

            parametros.Add("i_idBooking", this.idBooking);
            parametros.Add("i_filename", this.filename);
            parametros.Add("i_path", this.path);
            parametros.Add("i_estado", this.estado);
            parametros.Add("i_usuarioCrea", this.usuarioCrea);
            parametros.Add("i_usuarioModifica", this.usuarioModifica);

            var db = sql_puntero.ExecuteInsertUpdateDeleteReturn(nueva_conexion, 6000, "EDI_bookingArchivo_insert", parametros, out OnError);
            if (!db.HasValue || db.Value < 0)
            {
                return null;
            }
            OnError = string.Empty;
            return db.Value;

        }

        

        #endregion
    }

    public class EDI_Vessel : BillionEntidades.Cls_Bil_Base
    {
        public string VEPR_REFERENCE { get; set; }
        public string VEPR_VSSL_VESSEL { get; set; }
        public string VEPR_VSSL_NAME { get; set; }
        public string VEPR_VOYAGE { get; set; }

        #region "Constructores"
        public EDI_Vessel()
        {
            base.init();
        }
        #endregion

        #region "Metodos"
        private static void OnInit(string Base)
        {
            sql_puntero = (sql_puntero == null) ? Cls_Conexion.Conexion() : sql_puntero;
            parametros = new Dictionary<string, object>();
            nueva_conexion = Cls_Conexion.Nueva_Conexion(Base);
        }


        public static List<EDI_Vessel> ListaVessel(string _reference)
        {
            OnInit("N5");
            string msg;
            parametros.Clear();
            parametros.Add("ID_REFERENCE", _reference);
            return sql_puntero.ExecuteSelectControl<EDI_Vessel>(nueva_conexion, 2000, "FNA_FUN_GET_VESSEL", parametros, out msg);
        }

        #endregion


    }
}                     
                      