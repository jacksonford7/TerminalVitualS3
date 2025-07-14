using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;


namespace ControlOPC.Entidades
{
    public class Vessel_Visit : Base
    {
        private static void OnInit()
        {
            sql_pointer = (sql_pointer == null) ? SQLHandler.sql_handler.GetInstance() : sql_pointer;
            parametros = new Dictionary<string, object>();

        }

        #region "Variables"
        private bool _craneLoaded;
        private bool _turnLoaded;
        private long _GKEY = 0;
        private string _NAME = string.Empty;
        private DateTime? _ETA;
        private DateTime? _ETD;
        private DateTime? _ATA;
        private DateTime? _ATD;
        private DateTime? _START_WORK;
        private DateTime? _END_WORK;

        private string _VOYAGE_IN = string.Empty;
        private string _VOYAGE_OUT = string.Empty;
        private string _BERTH = string.Empty;
        private DateTime? _FECHA_CITA;
        private string _ESTADOS = string.Empty;
        private long IdGruaAmarre = 9999;
        private string _detalle_opc = string.Empty;
        //private string _tipo_carga = string.Empty;

        #endregion

        #region "Propiedades"

        public List<Vessel_Crane> Cranes { get; set; }
        public List<Crane_Turn> Turns { get; set; }

        public bool IsCraneLoaded { get { return _craneLoaded; } }
        public bool IsTurnLoaded { get { return _turnLoaded; } }

        public Vessel_Visit()
        {
            base.init();
            init_list();

        }

        private void init_list()
        {
            this.Cranes = new List<Vessel_Crane>();
            this.Turns = new List<Crane_Turn>();
            this._craneLoaded = false;
            this._turnLoaded = false;
        }

        public string TIPO_CARGA { get; set; }
        public Int64 ID { get; set; }

        public long GKEY
        {
            get
            {
                return this._GKEY;
            }
            set
            {
                this._GKEY = value;
            }
        } //gkey de la nave

        public string REFERENCE { get; set; } //referencia de la nave


        public string detalle_opc
        {
            get
            {
                return this._detalle_opc;
            }
            set
            {
                this._detalle_opc = value;
            }
        } //nombre d ela nave

        public string NAME
        {
            get
            {
                return this._NAME;
            }
            set
            {
                this._NAME = value;
            }
        } //nombre d ela nave

        public DateTime? ETA
        {
            get
            {
                return this._ETA;
            }
            set
            {
                this._ETA = value;
            }
        } //estimado de arribo

        public DateTime? ETD
        {
            get
            {
                return this._ETD;
            }
            set
            {
                this._ETD = value;
            }
        } //estimado de salida

        public DateTime? ATA
        {
            get
            {
                return this._ATA;
            }
            set
            {
                this._ATA = value;
            }
        } //actual atraque

        public DateTime? ATD
        {
            get
            {
                return this._ATD;
            }
            set
            {
                this._ATD = value;
            }
        } //actual salida de nave

        public DateTime? START_WORK
        {
            get
            {
                return this._START_WORK;
            }
            set
            {
                this._START_WORK = value;
            }
        } //nave inicio a trabajar

        public DateTime? END_WORK
        {
            get
            {
                return this._END_WORK;
            }
            set
            {
                this._END_WORK = value;
            }
        } //nave finalizo el trabajo

        public string VOYAGE_IN
        {
            get
            {
                return this._VOYAGE_IN;
            }
            set
            {
                this._VOYAGE_IN = value;
            }
        } //viaje in

        public string VOYAGE_OUT
        {
            get
            {
                return this._VOYAGE_OUT;
            }
            set
            {
                this._VOYAGE_OUT = value;
            }
        } //viaje out

        public string BERTH
        {
            get
            {
                return this._BERTH;
            }
            set
            {
                this._BERTH = value;
            }
        } // muelle

        public DateTime? FECHA_CITA
        {
            get
            {
                return this._FECHA_CITA;
            }
            set
            {
                this._FECHA_CITA = value;
            }
        } //fecha de cita

        public string STATUS { get; set; }


        public string ESTADOS
        {
            get
            {
                return this._ESTADOS;
            }
            set
            {
                this._ESTADOS = value;
            }
        }

        //public string Tipo_carga
        //{
        //    get
        //    {
        //        return this._tipo_carga;
        //    }
        //    set
        //    {
        //        this._tipo_carga = value;
        //    }
        //}


        #endregion

        public Vessel_Visit(long _GKEY, string _NAME, DateTime? _ETA, DateTime? _ETD, DateTime? _ATA, DateTime? _ATD, DateTime? _START_WORK, DateTime? _END_WORK,
        string _VOYAGE_IN, string _VOYAGE_OUT, string _BERTH, DateTime? _FECHA_CITA, string _ESTADOS, string _detalle_opc)
        {
            this.GKEY = _GKEY;
            this.NAME = _NAME;
            this.ETA = _ETA;
            this.ETD = _ETD;
            this.ATA = _ATA;
            this.ATD = _ATD;
            this.START_WORK = _START_WORK;
            this.END_WORK = _END_WORK;
            this.VOYAGE_IN = _VOYAGE_IN;
            this.VOYAGE_OUT = _VOYAGE_OUT;
            this.BERTH = _BERTH;
            this.FECHA_CITA = _FECHA_CITA;
            this._craneLoaded = false;
            this._turnLoaded = false;
            this.ESTADOS = _ESTADOS;
            this.detalle_opc = _detalle_opc;
            base.init();

        }

        public Vessel_Visit(Int64 _id)
        {
            base.init();
            this.ID = _id;
            init_list();
        }

        public Vessel_Visit(string _reference)
        {
            base.init();
            this.REFERENCE = _reference;
            init_list();
        }

        //obtener el valor de la secuencia
        public Int64? next_sequence()
        {
            //EscalarFunction
            string er;
            var db = sql_pointer.EscalarFunction(sql_pointer.basic_con, 4000, "select next value for [dbo].[citacion]", parametros, out er);
            return db as Int64?;
        }
        //validaciones preavias de su cabecera
        private int? PreValidations(out string msg)
        {
            //validar los campos
            if (this.GKEY <= 0)
            {
                msg = "Campo GKEY no puede ser menor o igual a cero";
                return 0;
            }

            if (ID <= 0)
            {
                msg = "Campo ID no puede ser menor o igual a cero";
                return 0;
            }
            if (!ETA.HasValue)
            {
                msg = "Escriba el estimado de arribo (ETA)";
                return 0;
            }
            if (!ETD.HasValue)
            {
                msg = "Escriba el estimado de zarpe (ETD)";
                return 0;
            }
            if (ETD < ETA)
            {
                msg = "La fecha de zarpe no puede ser inferior al arribo";
                return 0;
            }
            msg = app_configurations.CheckString(VOYAGE_IN, 1, 50, "Viaje Entrante");
            if (!string.IsNullOrEmpty(msg))
            {
                return 0;
            }
            msg = app_configurations.CheckString(VOYAGE_OUT, 1, 50, "Viaje Saliente");
            if (!string.IsNullOrEmpty(msg))
            {
                return 0;
            }
            msg = app_configurations.CheckString(Create_user, 1, 50, "Usuario");
            if (!string.IsNullOrEmpty(msg))
            {
                return 0;
            }
            msg = app_configurations.CheckString(REFERENCE, 1, 50, "REFERENCIA");
            if (!string.IsNullOrEmpty(msg))
            {
                return 0;
            }
            if (!this.FECHA_CITA.HasValue)
            {
                msg = "La fecha de citación no no puede ser un valor nulo";
                return 0;
            }
            if (FECHA_CITA > ETD)
            {
                msg = "La fecha de citación no puede ser un valor nulo";
                return 0;
            }

            msg = string.Empty;
            return 1;
        }
        //grabado interno, gus
        private Int64? Save(out string OnError)
        {
            if (this.PreValidations(out OnError) != 1)
            {
                return -1;
            }
            parametros.Clear();
            parametros.Add("i_id", ID);
            parametros.Add("i_gkey", GKEY);
            parametros.Add("i_reference", REFERENCE);
            parametros.Add("i_name", NAME);
            parametros.Add("i_eta", ETA);
            parametros.Add("i_etd", ETD);
            parametros.Add("i_ata", ATA);
            parametros.Add("i_atd", ATD);
            parametros.Add("i_starwork", START_WORK);
            parametros.Add("i_endwork", END_WORK);
            parametros.Add("i_voyagein", VOYAGE_IN);
            parametros.Add("i_voyageout", VOYAGE_OUT);
            parametros.Add("i_berth", BERTH);
            parametros.Add("i_citation_date", FECHA_CITA);
            parametros.Add("i_create_user", Create_user);
            parametros.Add("i_tipo_carga", TIPO_CARGA);
            var db = sql_pointer.ExecuteInsertUpdateDelete(sql_pointer.basic_con, 4000, "PC_I_Vessel_visit", parametros, out OnError);

            if (!db.HasValue || db.Value < 0)
            {
                //null error en base de datos, el mensaje ya fue en onError
                return null;
            }
            OnError = string.Empty;
            return db.Value;
            // PC_I_Vessel_visit
        }
        //grabado publico
        public Int64? SaveTransaction(out string OnError)
        {
            bool result_ = false;
            string resultado_others = null;
            Int64? tx_cab = 0;
            try
            {
                //validaciones previas
                if (this.PreValidationsTransaction(out OnError) != 1)
                {
                    return -1;
                }
                //pedir la secuencia.
                tx_cab = next_sequence();
                using (var scope = new System.Transactions.TransactionScope())
                {
                    //seteo la cabecera antes de insertar
                    this.ID = tx_cab.Value;
                    //Insertar la cabecera obtener las filas afectadas
                    var afe = this.Save(out OnError);
                    if (afe <= 0)
                    {
                        return -1;
                    }
                    //Recorrer cada grua
                    foreach (var i in this.Cranes)
                    {

                        //setear el valor de la cabecera.
                        i.VesselVisit_ID = this.ID;
                        i.Create_user = this.Create_user;
                        var si = i.Save(out OnError);
                        if (!si.HasValue)
                        {
                            OnError = "No fue posible obtener la secuencia de la grúa el proceso no puede continuar";
                            return null;
                        }
                        if (si.Value < 0)
                        {
                            return null;
                        }
                        //seteo el valor del identity de esta grua
                        i.Id = si.Value;
                       
                    } //bucle de las gruas
                    //fin de la transaccion
                    scope.Complete();
                    OnError = string.Empty;
                    return tx_cab;
                }
            }
            catch (Exception ex)
            {
                result_ = false;
                resultado_others = ex.Message; ;
                var r = SQLHandler.sql_handler.LogEvent<Exception>(this.Create_user, nameof(Vessel_Visit), nameof(SaveTransaction), result_, this.ID.ToString(), null, resultado_others, ex);
                OnError = string.Format("Ha ocurrido la excepcion número: {0}", r);
                return null;
            }
        }
        //validaciones preavias de su listas
        private int? PreValidationsTransaction(out string msg)
        {
            if (this.Cranes == null && this.Cranes.Count <= 0)
            {
                msg = "No se puede agregar transacción sin grúas";
                return 0;
            }

            foreach (var i in this.Cranes)
            {
                if (i.Crane_Gkey == IdGruaAmarre)
                {
                    this.TIPO_CARGA = "B";
                    msg = string.Empty;
                    return 1;
                }
                else {
                    this.TIPO_CARGA = "C";
                    msg = string.Empty;
                    return 1;
                }
              
            } //bucle de las gruas
            /*if (this.Turns == null && this.Turns.Count <= 0)
            {
                msg = "No se puede agregar transacción sin turnos";
                return 0;
            }*/
            msg = string.Empty;
            return 1;
        }
        //estatico de existencia, retorna true: en uso, false: no esta en uso, null: se cayo
        public static bool? ReferenceHasActiveTransaction(string referencia, out string msg)
        {
            //inicializar variables.
            OnInit();
            //revisar la cadena limpia
            msg = app_configurations.CheckString(referencia, 1, 50, "Referencia");
            if (!string.IsNullOrEmpty(msg)) { return false; }
            referencia = referencia.Trim().ToUpper();
            //ahora si validar en la base de datos
            //->ExecuteSelectOnly
            parametros.Clear();
            parametros.Add("I_REFERENCE", referencia);
            //te retorna un data_message
            var dm = sql_pointer.ExecuteSelectOnly(sql_pointer.basic_con, 4000, "PC_C_Valida_buque", parametros);
            if (dm.code != 0)
            {
                msg = dm.message;
                return true;
            }
            //retorna si no existe
            msg = string.Empty;
            return false;
        }
        /// <summary>
        /// Carga las gruas desde la Base de datos, debe estar seteado el ID
        /// </summary>
        public void LoadCranes()
        {
            if (this.ID <= 0)
            {
                return;
            }

            //cargar la lista de gruas desde la db;
            string error;
            if (this.Cranes == null || this.Cranes.Count <= 0)
                this.Cranes = Vessel_Crane.ListVesselCrane(this.ID, out error);

            this._craneLoaded = this.Cranes != null && this.Cranes.Count > 0 ? true : false;
        }
        /// <summary>
        /// Carga los turnos desde la Base de datos, debe estar seteado el ID
        /// </summary>
        public void LoadTurns()
        {
            if (this.ID <= 0)
            {
                return;
            }
            //cargar la lista de gruas desde la db;
            string error;
            if (this.Turns == null || this.Turns.Count <= 0)
                this.Turns = Crane_Turn.ListCraneTurn(this.ID, out error);

            this._turnLoaded = this.Turns != null && this.Turns.Count > 0 ? true : false;
        }
        /// <summary>
        /// Recarga la lista de gruas actual desde la base de datos en caso que haya sufrido cambios
        /// </summary>
        /// <param name="OnError">Captura del mensaje</param>
        /// <returns>True: Todos funciono bien, False: Hubo errores en el proceso o no hay gruas</returns>
        public bool LoadCranes(out string OnError)
        {
            this.Cranes = null;
            this.Cranes = Vessel_Crane.ListVesselCrane(this.ID, out OnError);
            if (this.Cranes != null && this.Cranes.Count > 0)
            {
                OnError = string.Empty;
                return true;
            }
            this._craneLoaded = this.Cranes != null && this.Cranes.Count > 0 ? true : false;
            return false;
        }
        /// <summary>
        /// Recarga la lista de Turnos actual desde la base de datos en caso que haya sufrido cambios
        /// </summary>
        /// <param name="OnError">Captura del mensaje</param>
        /// <returns>True: Todos funciono bien, False: Hubo errores en el proceso o no hay gruas</returns>
        public bool LoadTurns(out string OnError)
        {
            this.Turns = null;
            this.Turns = Crane_Turn.ListCraneTurn(this.ID, out OnError);
            if (this.Turns != null && this.Turns.Count > 0)
            {
                OnError = string.Empty;
                return true;
            }
            this._turnLoaded = this.Turns != null && this.Turns.Count > 0 ? true : false;
            return false;
        }
        /// <summary>
        /// Permite obtener todos los datos de la cabecera
        /// </summary>
        /// <param name="OnError">Si es falso entonces tiene el mensaje de error</param>
        /// <returns></returns>
        public bool PopulateMyData(out string OnError)
        {
            //cargar todos los datos este documento
            if (this.ID <= 0 && string.IsNullOrEmpty(this.REFERENCE))
            {
                OnError = "Debe establecer el campo ID o REFERENCE de la visita";
                return false;
            }
            parametros.Clear();
            if (this.ID > 0)
            {
                parametros.Add("i_id", this.ID);
            }
            else
            {
                parametros.Add("i_reference", this.REFERENCE);
            }
            var t = sql_pointer.ExecuteSelectOnly<Vessel_Visit>(sql_pointer.basic_con, 4000, "PC_C_Vessel_visit", parametros);
            if (t == null)
            {
                OnError = "No fue posible obtener los datos de la visita";
                return false;
            }

            this.ID = t.ID;
            this.GKEY = t.GKEY;
            this.REFERENCE = t.REFERENCE;
            this.NAME = t.NAME;
            this.ETA = t.ETA;
            this.ETD = t.ETD;
            this.ATA = t.ATA;
            this.ATD = t.ATD;
            this.START_WORK = t.START_WORK;
            this.END_WORK = t.END_WORK;
            this.VOYAGE_IN = t.VOYAGE_IN;
            this.VOYAGE_OUT = t.VOYAGE_OUT;
            this.BERTH = t.BERTH;
            this.FECHA_CITA = t.FECHA_CITA;
            this.STATUS = t.STATUS;
            this.Create_date = t.Create_date;
            this.Create_user = t.Create_user;
            this.Mod_date = t.Mod_date;
            this.Mod_user = t.Mod_user;
            this.Mod_data = t.Mod_data;
            this.TIPO_CARGA = t.TIPO_CARGA;
            OnError = string.Empty;
            return true;
        }
        /// <summary>
        /// Retorna una lista de Objetos Vessel Visit
        /// </summary>
        /// <param name="desde">Fecha inicio de búsqueda</param>
        /// <param name="hasta">Fecha fin de búsqueda</param>
        /// <param name="referencia">Opcional, si deseas buscar una sola, null es todas</param>
        /// <param name="status">Opcional, buscar Vessel visit en estado especifico, null es todas</param>
        /// <param name="todas">Opcional, buscar Vessel visit solo activas es decir sin borrado lógico, false= solo activas, true= activas e inactivas </param>
        /// <returns></returns>
        public static List<Vessel_Visit> Get_Visits(DateTime desde, DateTime hasta, string referencia = null, string status = null, bool todas = false)
        {
            OnInit();
            parametros.Clear();
            parametros.Add("i_desde", desde);
            parametros.Add("i_hasta", hasta);
            parametros.Add("i_all", todas);
            if (!string.IsNullOrEmpty(referencia)) { parametros.Add("i_reference", referencia); }
            if (!string.IsNullOrEmpty(status)) { parametros.Add("i_stat", status); }
            string msh = string.Empty;
            return sql_pointer.ExecuteSelectControl<Vessel_Visit>(sql_pointer.basic_con, 2000, "PC_G_Vessel_visit", parametros, out msh);
        }
        /// <summary>
        /// devuelve una lista de Vessel Visist Activos y con status N - Nuevo / P - Proceso
        /// </summary>
        /// <returns></returns>
        public static List<Vessel_Visit> ListViesselVisit(string _status)
        {
            OnInit();
            return sql_pointer.ExecuteSelectControl<Vessel_Visit>(sql_pointer.basic_con, 2000, "PC_C_Vessel_visit_List", new Dictionary<string, object>() { { "i_status", _status } }, out string msg);//retornar la lista de gruas q estan en los documentos

        }

        public static List<Vessel_Visit> ListViesselVisit(DateTime desde, DateTime hasta, string opc_id, string referencia)
        {
            OnInit();
            parametros.Clear();
            parametros.Add("desde",desde);
            parametros.Add("hasta", hasta);
            parametros.Add("i_reference", referencia);
            parametros.Add("i_opc_id", opc_id);
            return sql_pointer.ExecuteSelectControl<Vessel_Visit>(sql_pointer.basic_con, 2000, "PC_C_Vessel_visit_OP", parametros, out string msg);//retornar la lista de gruas q estan en los documentos
        }

        public static List<Vessel_Visit> ListVesselVisitSup()
        {
            OnInit();
            return sql_pointer.ExecuteSelectControl<Vessel_Visit>(sql_pointer.basic_con, 2000, "PC_C_Vessel_visit_List_Sup", null, out string msg);//retornar la lista de gruas q estan en los documentos

        }



        /// <summary>
        /// Retorna los VV para publicar
        /// </summary>
        /// <returns></returns>
        public static List<Vessel_Visit> ListPublicar()
        {
            OnInit();
            return sql_pointer.ExecuteSelectControl<Vessel_Visit>(sql_pointer.basic_con, 2000, "PC_P_Vessel_visit", null, out string msg);//retornar la lista de gruas q estan en los documentos

        }
    

        public static string Publish(Int64 id, string user, string status)
        {
            parametros.Clear();
            parametros.Add("i_id", id);
            parametros.Add("i_user", user);
            parametros.Add("i_estatus", status);
            string Onerror;
            var db = sql_pointer.ExecuteInsertUpdateDelete(sql_pointer.basic_con, 4000, "PC_E_Vessel_visit", parametros, out Onerror);
            return Onerror;
        }
        //inactivar transaccion
        public static string Active(Int64 id, string user)
        {
            parametros.Clear();
            parametros.Add("i_id", id);
            parametros.Add("i_mod_user", user);
            string Onerror;
            var db = sql_pointer.ExecuteInsertUpdateDelete(sql_pointer.basic_con, 4000, "PC_D_Vessel_visit", parametros, out Onerror);
            return Onerror;
        }

        //listado de transacciones con status ('N','W','S','P'), para poder inactivarlas
        public static List<Vessel_Visit> ListViesselVisitTotal()
        {
            OnInit();
            return sql_pointer.ExecuteSelectControl<Vessel_Visit>(sql_pointer.basic_con, 2000, "PC_C_Vessel_visit_Total", new Dictionary<string, object>() { }, out string msg);

        }


        public bool format_mail_opcs(out string body_mail)
        {
            //OPC_MAIL--> direcciones de mail
            var valor = app_configurations.get_configuration("MAIL_OPC")?.value;
            if (string.IsNullOrEmpty(valor))
            {
                body_mail = "Error!";
                return false;
            }
            body_mail = string.Format(valor, this.REFERENCE, this.ETA.Value.ToString("dd/MM/yyyy HH:mm"), this.ETD.Value.ToString("dd/MM/yyyy HH:mm"), this.FECHA_CITA.Value.ToString("dd/MM/yyyy HH:mm"));
            return true;
        }

        public bool format_mail_opcs_aprobado(out string body_mail)
        {
            //OPC_MAIL--> direcciones de mail
            var valor = app_configurations.get_configuration("MAIL_APROBADO")?.value;
            if (string.IsNullOrEmpty(valor))
            {
                body_mail = "Error!";
                return false;
            }
            body_mail = string.Format(valor, this.REFERENCE);
            return true;
        }


 

    }


}
