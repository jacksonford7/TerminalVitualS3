using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ControlOPC.Entidades
{
    public class ProformaCab : Base
    {
        private Int64 mId = 0;
        private bool _DetalleLoaded;
        private string mVessel_visit_reference = "";
        private string mOpc_id = "";
        private string mopc_name = "";
        private string mtipo_carga = string.Empty;
        private string mStatus = "";
        private bool mActive = false;
        private decimal mTotal_horas = 0;
        private decimal mTotal_precio = 0;
        private decimal mTotal = 0;
        private string mobservacion = "";
        private string mAdicional = "";
        #region "Propiedades"

        public Int64 Id
        {
            get
            {
                return mId;
            }
            set
            {
                mId = value;
            }
        }

      

        public string Vessel_visit_reference
        {
            get
            {
                return mVessel_visit_reference;
            }
            set
            {
                mVessel_visit_reference = value;
            }
        }

        public string Opc_id
        {
            get
            {
                return mOpc_id;
            }
            set
            {
                mOpc_id = value;
            }
        }

        public string Opc_name
        {
            get
            {
                return mopc_name;
            }
            set
            {
                mopc_name = value;
            }
        }

        public string Status { get => mStatus; set => mStatus = value; }

        public bool Active { get => mActive; set => mActive = value; }

        public decimal Total_horas
        {
            get
            {
                return mTotal_horas;
            }
            set
            {
                mTotal_horas = value;
            }
        }

        public decimal Total_precio
        {
            get
            {
                return mTotal_precio;
            }
            set
            {
                mTotal_precio = value;
            }
        }

        public decimal Total
        {
            get
            {
                return mTotal;
            }
            set
            {
                mTotal = value;
            }
        }

        public string Observacion
        {
            get
            {
                return mobservacion;
            }
            set
            {
                mobservacion = value;
            }
        }

        public string Adicional
        {
            get
            {
                return mAdicional;
            }
            set
            {
                mAdicional = value;
            }
        }

        //cAMPOS DE RIDE

        /*

  ,[]
  ,[subtotal_xml]
  ,[total_xml]
         */

        public string ruc_xml { get; set; }
        public string num_xml { get; set; }
        public string fecha_xml { get; set; }
        public decimal? subtotal_xml { get; set; }
        public decimal? total_xml { get; set; }


        #endregion

        public List<ProformaDet> ProformaDetalle { get; set; }
     //   public List<ProformaDet> DetalleProforma { get; set; }
        public string Tipo_carga { get => mtipo_carga; set => mtipo_carga = value; }
        public bool IsDetalleLoaded { get { return _DetalleLoaded; } }

        public ProformaCab(Int64 _id)
        {
            base.init();
            this.Id = _id;
            init_list();
        }

        public ProformaCab()
        {
            base.init();
            init_list();
        }

        private void init_list()
        {
            this.ProformaDetalle = new List<ProformaDet>();
           // this.DetalleProforma = new List<ProformaDet>();
            this._DetalleLoaded = false;
        }

        private static void OnInit()
        {
            sql_pointer = (sql_pointer == null) ? SQLHandler.sql_handler.GetInstance() : sql_pointer;
            parametros = new Dictionary<string, object>();
        }

        public static List<ProformaCab> ListProformasCab(string vessel_visit_reference, out string OnError)
        {
            return sql_pointer.ExecuteSelectControl<ProformaCab>(sql_pointer.basic_con, 2000, "PC_C_ProformaCab_List", new Dictionary<string, object>() { { "vessel_visit_reference", vessel_visit_reference } }, out OnError);
        }

        public static List<ProformaCab> ListProformasCabFiltros(string vessel_visit_reference, Int32 tipo,out string OnError)
        {
            return sql_pointer.ExecuteSelectControl<ProformaCab>(sql_pointer.basic_con, 2000, "PC_C_ProformaCab_Filtro", new Dictionary<string, object>() { { "i_vessel_visit_reference", vessel_visit_reference }, { "i_tipo", tipo } }, out OnError);
        }

        public static ProformaCab GetProformasCab(Int64 _proforma_id, out string OnError)
        {
            return sql_pointer.ExecuteSelectControl<ProformaCab>(sql_pointer.basic_con, 2000, "PC_C_ProformaCab_Esp", new Dictionary<string, object>() { { "i_proforma_id", _proforma_id } }, out OnError).FirstOrDefault();
        }

        public Int64? Save(out string OnError)
        {
            if (this.PreValidations(out OnError) != 1)
            {
                return -1;
            }
            parametros.Clear();
            parametros.Add("i_vessel_visit_reference", Vessel_visit_reference);
            parametros.Add("i_opc_id", Opc_id);
            parametros.Add("i_opc_name",Opc_name);
            parametros.Add("i_tipo_carga",Tipo_carga);
            parametros.Add("i_status", Status);
            parametros.Add("i_active", Active);
            parametros.Add("i_create_user", Create_user);
            parametros.Add("i_observacion", Observacion);
            parametros.Add("i_subtotal", Total);
            var db = sql_pointer.ExecuteInsertUpdateDeleteReturn(sql_pointer.basic_con, 4000, "PC_I_ProformaCab", parametros, out OnError);
            if (!db.HasValue || db.Value < 0)
            {
                //null error en base de datos, el mensaje ya fue en onError
                return null;
            }
            OnError = string.Empty;
            return db.Value;
            // PC_I_Turn
        }

        public Int64? SaveProforma(out string OnError)
        {

            if (this.PreValidations(out OnError) != 1)
            {
                return -1;
            }

            parametros.Clear();
            parametros.Add("i_reference", Vessel_visit_reference);
            parametros.Add("i_create_user", Create_user);
            var db = sql_pointer.ExecuteSelectOnly(sql_pointer.basic_con, 4000, "PC_I_ProformaCab_ProcessCreate", parametros);
            if (db == null)
            {
                //null error en base de datos, el mensaje ya fue en onError
                OnError = "Error al grabar proforma " + OnError;
                return null;
            }
            else 
            {
                if (db.code == 1)
                {
                    OnError = string.Empty;
                }
                else {
                    OnError = db.message;
                }
                            
            }

      //      OnError = string.Empty;
            return 1;
           
        }

        //grabado publico
        public Int64? SaveTransaction(out string OnError)
        {
            bool result_ = false;
            string resultado_others = null;
            try
            {
                //validaciones previas
                if (this.PreValidationsTransaction(out OnError) != 1)
                {
                    return -1;
                }
                            
                using (var scope = new System.Transactions.TransactionScope())
                {
                    //Insertar la cabecera obtener la secuencia
                    this.Id = this.Save(out OnError).Value;
                    if (this.Id <= 0)
                    {
                        return -1;
                    }

                    int linea =1;
                    //Recorrer cada detalle de la proforma
                    foreach (var i in this.ProformaDetalle)
                    {
                        //setear el valor de la cabecera.
                        i.Proforma_id  = this.Id;
                        i.Line = linea;
                        i.Create_user = this.Create_user;

                        // graba la linea de detalle
                        var si = i.Save(out OnError);
                        if (!si.HasValue)
                        {
                            OnError = "No fue posible grabar linea de detalle ";
                            return null;
                        }
                        if (si.Value < 0)
                        {
                            return null;
                        }
                        linea = linea+1;
                    } //bucle de detalle
                    //fin de la transaccion
                    scope.Complete();
                    OnError = string.Empty;
                    return this.Id;
                }
            }
            catch (Exception ex)
            {
                result_ = false;
                resultado_others = ex.Message; ;
                var r = SQLHandler.sql_handler.LogEvent<Exception>(this.Create_user, nameof(Vessel_Visit), nameof(SaveTransaction), result_, this.Id.ToString(), null, resultado_others, ex);
                OnError = string.Format("Ha ocurrido la excepcion número: {0}", r);
                return null;
            }
        }

        //validaciones preavias de su listas
        private int? PreValidationsTransaction(out string msg)
        {
            if (this.ProformaDetalle == null && this.ProformaDetalle.Count <= 0)
            {
                msg = "No se puede agregar transacción sin grúas";
                return 0;
            }
            
            msg = string.Empty;
            return 1;
        }

        private int? PreValidations(out string msg)
        {
            //1.->Validaciones de objetos.
            if (this.Vessel_visit_reference.Length <= 0)
            {
                msg = "Especifique la referencia";
                return 0;
            }
                   

            if (Id == 0)
            {
                Id = -1;
            }
            
            msg = string.Empty;
            return 1;
        }


        //PC_C_MailAutomatico



        //grabado publico
        public Int64? ProformaTransaction(out string OnError)
        {
            bool result_ = false;
            string resultado_others = null;
            try
            {
                
                using (var scope = new System.Transactions.TransactionScope())
                {
                    //Insertar la cabecera obtener la secuencia
                    var nreg = this.SaveProforma(out OnError);
                    if (!nreg.HasValue || nreg.Value < 0)
                    {
                        OnError = "Error al procesar la transaccion..Generación de Proformas";
                        this.Id = -1;
                        return null;
                    }

                    
                    this.Id = nreg.Value;
                    //fin de la transaccion
                    scope.Complete();
                    OnError = string.Empty;
                    return nreg;
                }
            }
            catch (Exception ex)
            {
                result_ = false;
                resultado_others = ex.Message; ;
                var r = SQLHandler.sql_handler.LogEvent<Exception>(this.Create_user, nameof(ProformaCab), nameof(ProformaTransaction), result_, this.Id.ToString(), null, resultado_others, ex);
                OnError = string.Format("Ha ocurrido la excepcion número: {0}", r);
                return null;
            }
        }

        public bool PopulateMyData(out string OnError)
        {
            //cargar todos los datos este documento
            if (this.Id <= 0 )
            {
                OnError = "Debe establecer el campo ID de la profoma";
                return false;
            }
            parametros.Clear();
            parametros.Add("i_id", this.Id);
           
            var t = sql_pointer.ExecuteSelectOnly<ProformaCab>(sql_pointer.basic_con, 4000, "PC_C_ProformaCab", parametros);
            if (t == null)
            {
                OnError = "No fue posible obtener los datos de la visita";
                return false;
            }

            this.Id = t.Id;
            this.Vessel_visit_reference = t.Vessel_visit_reference;
            this.Opc_id = t.Opc_id;
            this.Opc_name = t.Opc_name;
            this.Tipo_carga = t.Tipo_carga;
            this.Status = t.Status;
            this.Active = t.Active;
            this.Create_date = t.Create_date;
            this.Create_user = t.Create_user;
            this.Total_horas = t.Total_horas;
            this.Total = t.Total;
            this.Observacion = t.Observacion;

            OnError = string.Empty;
            return true;
        }

        public void LoadDetalle()
        {
            if (this.Id <= 0)
            {
                return;
            }

            //cargar la lista de gruas desde la db;
            string error;
            if (this.ProformaDetalle == null || this.ProformaDetalle.Count <= 0)
                this.ProformaDetalle = ProformaDet.ListProformasDet(this.Id, out error);

            this._DetalleLoaded = this.ProformaDetalle != null && this.ProformaDetalle.Count > 0 ? true : false;
        }

        public bool LoadDetalle(out string OnError)
        {
            this.ProformaDetalle = null;
            this.ProformaDetalle = ProformaDet.ListProformasDet(this.Id, out OnError);
            if (this.ProformaDetalle != null && this.ProformaDetalle.Count > 0)
            {
                OnError = string.Empty;
                return true;
            }
            this._DetalleLoaded = this.ProformaDetalle != null && this.ProformaDetalle.Count > 0 ? true : false;
            return false;
        }

        public static List<ProformaCab> ListProformasCab(DateTime desde, DateTime hasta, Int64 id, string referencia, string opc_id, string estado, out string OnError)
        {
            OnInit();
            parametros.Clear();
            parametros.Add("i_desde", desde);
            parametros.Add("i_hasta", hasta);
            parametros.Add("i_id", id);
            parametros.Add("i_referencia", referencia);
            parametros.Add("i_opc_id", opc_id);
            parametros.Add("i_estado", estado);
            return sql_pointer.ExecuteSelectControl<ProformaCab>(sql_pointer.basic_con, 2000, "PC_L_Proforma", parametros, out OnError);
        }

        //jsarmiento: validar ride e insertar.
        public bool? update_ride(Ride rid, out string novedad)
        {
            //primero validar
            if (PreValidationsRide(rid, out novedad) != 1)
            {
                return false;
            }
            //update de base datos
            //compar
            parametros.Clear();
            parametros.Add("i_id", this.Id);
            parametros.Add("i_mod_user", this.Mod_user);
            parametros.Add("num_xml", rid.numero);
            parametros.Add("ruc_xml", rid.ruc);
            parametros.Add("fecha_xml", rid.fechaEmision);
            parametros.Add("subtotal_xml", rid.totalSinImpuestos.Replace(",","."));
            parametros.Add("total_xml", rid.importeTotal.Replace(",","."));
            var db = sql_pointer.ExecuteInsertUpdateDelete(sql_pointer.basic_con, 4000, "PC_U_Proforma_Ride", parametros, out novedad);
            novedad = string.Empty;
            return true;
        }
        private int PreValidationsRide(Ride ri, out string msg)
        {
            if (ri == null)
            {
                msg = "El Ride no puede ser un objeto nulo";
                return 0;
            }

            if (string.IsNullOrEmpty(this.Mod_user))
            {
                msg = "Debe establecer el campo Proforma/Mod_User";
                return 0;
            }

            if (ProformaCab.RevisaCadena(ri.ruc, "Ride/RUC", out msg) == 0)
            {
                return 0;
            }
            if (ProformaCab.RevisaCadena(ri.numero, "Ride/Número Factura", out msg) == 0)
            {
                return 0;
            }
            if (ProformaCab.RevisaCadena(ri.fechaEmision, "Ride/Fecha Emisión", out msg) == 0)
            {
                return 0;
            }
            decimal? v;
            v = ProformaCab.RevisaDecimal(ri.importeTotal, out msg);
            if (!v.HasValue)
            {
                return 0;
            }
            ri.importeTotal = v.ToString();
            v = ProformaCab.RevisaDecimal(ri.totalSinImpuestos, out msg);
            if (!v.HasValue)
            {
                return 0;
            }
            ri.totalSinImpuestos = v.ToString();
            decimal pct = 0;
            var c = app_configurations.get_configuration("RANGO_ERROR");
            if (c == null || string.IsNullOrEmpty(c.value))
            {
                pct = 5;
            }
            if (!decimal.TryParse(c.value, out pct))
            {
                pct = 5;
            }

            decimal? frac = (pct / 100);
            //maximo el 5% de diferencia se soporta o el que diga la configuracion
            decimal? vpct = this.Total_precio * frac;
            decimal? rango_min = this.Total_precio - vpct;
            decimal? rango_max = this.Total_precio + vpct;
            //----------------------------------------------------------------------

            //v--> el ultimo calculo
            if (v < rango_min || v > rango_max)
            {
                msg = string.Format("El total sin impuestos del ride {0} excede o es inferior al valor de la proforma {1} en mas de un {2}%", ri.totalSinImpuestos, this.Total_precio, pct);
                return 0;
            }


            if (!this.Opc_id.Equals(ri.ruc))
            {
                msg = string.Format("El numero de ruc del ride no coincide con el de la proforma {0}",ri.ruc);
                return 0;
            }

            //----------------------------------------------------------------------
            msg = string.Empty;
            return 1;
        }


        /*anula proformas y restaura ordenes de trabajo*/
        public bool? AnularProforma(out string OnError)
        {
            bool result_ = false;
            string resultado_others = null;

            try
            {
                
                if (string.IsNullOrEmpty(this.Vessel_visit_reference))
                {
                    OnError = "no se puede anular proformas, falta la referenia del buque";
                    return result_;
                }
                if (string.IsNullOrEmpty(this.Mod_user))
                {
                    OnError = "no se puede anular proformas, falta el usuario de la transacción";
                    return result_;
                }


               
                parametros.Clear();
                parametros.Add("i_vessel_visit_reference", Vessel_visit_reference);
                parametros.Add("i_mod_user", this.Mod_user);
                parametros.Add("i_tipo", this.mActive);
                string Oe = string.Empty;
                var e = sql_pointer.ExecuteInsertUpdateDelete(sql_pointer.basic_con, 4000, "PC_D_ProformaCab", parametros, out Oe);
                if (e.HasValue && e.Value > 0)
                {
                    OnError = string.Empty;
                    return true;
                }
                OnError = string.Empty;
                return true;
            }
            catch (Exception ex)
            {
                result_ = false;
                resultado_others = ex.Message; ;
                var r = SQLHandler.sql_handler.LogEvent<Exception>(this.Create_user, nameof(Crane_Turn), nameof(AnularProforma), result_, this.Vessel_visit_reference, null, resultado_others, ex);
                OnError = string.Format("Ha ocurrido la excepción #{0}", r);
                return null;
            }

        }


        public static void SendMail(Int64 id)
        {
            parametros.Clear();
            parametros.Add("i_idProforma", id);
            string novedad;
            sql_pointer.ExecuteInsertUpdateDelete(sql_pointer.basic_con, 4000, "PC_C_MailAutomatico", parametros, out novedad);
        }




    }
}
